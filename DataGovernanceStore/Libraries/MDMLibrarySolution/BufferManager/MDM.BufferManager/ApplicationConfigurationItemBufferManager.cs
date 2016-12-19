using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;

namespace MDM.BufferManager
{
    using MDM.BusinessObjects;
    using MDM.CacheManager.Business;
    using MDM.Core;
    using MDM.ExceptionManager;
    using MDM.Utility;

    /// <summary>
    /// Specifies ApplicationConfigurationItem Buffer Manager
    /// </summary>
    public class ApplicationConfigurationItemBufferManager : BusinessLogicBase
    {
        #region Fields

        /// <summary>
        /// Field denoting cache manager of cache
        /// </summary>
        private ICache _cacheManager = null;

        /// <summary>
        /// Field denoting application configuration item cache is enabled or not
        /// </summary>
        private Boolean _isApplicationConfigurationItemCacheEnabled = false;

        /// <summary>
        /// Field denotes whether the Distributed Cache with notification has been enabled
        /// </summary>
        private Boolean _isDistributedCacheWithNotificationEnabled = false;

        /// <summary>
        /// Field denotes an instance of the CacheSynchronizationHelper
        /// </summary>
        private CacheSynchronizationHelper _cacheSynchronizationHelper = null;

        #endregion

        #region Constructors

        /// <summary>
        /// Instantiate ApplicationConfigurationItem Buffer Manager
        /// </summary>
        public ApplicationConfigurationItemBufferManager()
        {
            try
            {
                //Get AppConfig which specify whether cache is enabled for application configuration item or not
                this._isApplicationConfigurationItemCacheEnabled = AppConfigurationHelper.GetAppConfig<Boolean>("MDMCenter.ApplicationConfigurationItem.Cache.Enabled", true);
                this._isDistributedCacheWithNotificationEnabled = AppConfigurationHelper.GetAppConfig<Boolean>("MDMCenter.DistributedCacheWithNotification.Enabled", false);

                if (_isApplicationConfigurationItemCacheEnabled)
                {
                    _cacheManager = CacheFactory.GetCache();
                    if (_isDistributedCacheWithNotificationEnabled)
                        _cacheSynchronizationHelper = new CacheSynchronizationHelper();
                }

                if (_cacheManager == null)
                    this._isApplicationConfigurationItemCacheEnabled = false;
            }
            catch
            {
                this._isApplicationConfigurationItemCacheEnabled = false;
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Property denoting application configuration item cache is enabled or not
        /// </summary>
        public Boolean IsApplicationConfigurationItemCacheEnabled
        {
            get
            {
                return _isApplicationConfigurationItemCacheEnabled;
            }
        }

        #endregion

        #region Methods

        #region Public Methods

        /// <summary>
        /// Finds available application configuration items from cache
        /// </summary>
        /// <param name="eventSource">Event source filter</param>
        /// <param name="eventSubscriber">Event subscriber filter</param>
        /// <returns>Collection of application configuration items if found in internal cache otherwise null</returns>
        public ApplicationConfigurationItemCollection FindApplicationConfigurationItems(EventSource eventSource, EventSubscriber? eventSubscriber)
        {
            ApplicationConfigurationItemCollection result = null;

            if (this._isApplicationConfigurationItemCacheEnabled)
            {
                String cacheKey = CacheKeyGenerator.GetApplicationConfigurationItemsListCacheKey(eventSource, eventSubscriber);

                object itemsObj = null;

                try
                {
                    itemsObj = _cacheManager.Get(cacheKey);
                }
                catch (Exception ex)
                {
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, String.Format("Error occurred : {0}", ex.Message));
                    new ExceptionHandler(ex);
                }

                ApplicationConfigurationItemCollection applicationConfigurationItemCollection = itemsObj as ApplicationConfigurationItemCollection;
                if (applicationConfigurationItemCollection != null)
                {
                    result = (ApplicationConfigurationItemCollection)applicationConfigurationItemCollection.Clone();
                }
            }

            return result;
        }

        /// <summary>
        /// Update ApplicationConfigurationItems in internal cache
        /// </summary>
        /// <param name="eventSource">Event source filter</param>
        /// <param name="eventSubscriber">Event subscriber filter</param>
        /// <param name="applicationConfigurationItems">This parameter is specifying collection of application configuration items to be update in cache</param>
        /// <param name="numberOfRetry">This parameter is specifying number of retry to update object in cache</param>
        /// <param name="isDataUpdated">Specifies whether the cache data object is updated and inserted to local cache</param>
        public void UpdateApplicationConfigurationItems(EventSource eventSource, EventSubscriber? eventSubscriber, ApplicationConfigurationItemCollection applicationConfigurationItems, Int32 numberOfRetry, Boolean isDataUpdated = false)
        {
            if (this._isApplicationConfigurationItemCacheEnabled)
            {
                if (applicationConfigurationItems == null || !applicationConfigurationItems.Any())
                    throw new ArgumentException("ApplicationConfigurationItems are not available or empty.");

                String cacheKey = CacheKeyGenerator.GetApplicationConfigurationItemsListCacheKey(eventSource, eventSubscriber);

                DateTime expiryTime = DateTime.Now.AddHours(1);

                //Clone the object before set to cache
                ApplicationConfigurationItemCollection clonedApplicationConfigurationItems = (ApplicationConfigurationItemCollection)applicationConfigurationItems.Clone();

                try
                {
                    if (_isDistributedCacheWithNotificationEnabled)
                        _cacheSynchronizationHelper.NotifyLocalCacheInsert(cacheKey, expiryTime, isDataUpdated);

                    _cacheManager.Set(cacheKey, clonedApplicationConfigurationItems, expiryTime);
                }
                catch (Exception ex)
                {
                    //Retry < 0 means just ignore update if failed..
                    if (numberOfRetry < 0)
                        return;

                    Boolean retrySuccess = false;

                    for (Int32 i = 0; i < numberOfRetry; i++)
                    {
                        try
                        {
                            if (_isDistributedCacheWithNotificationEnabled)
                                _cacheSynchronizationHelper.NotifyLocalCacheInsert(cacheKey, expiryTime, isDataUpdated);

                            _cacheManager.Set(cacheKey, applicationConfigurationItems, expiryTime);
                            
                            retrySuccess = true;
                            break;
                        }
                        catch
                        {
                        }
                    }

                    if (!retrySuccess)
                    {
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, String.Format("Error occurred : {0}", ex.Message));
                        new ExceptionHandler(ex);
                    }
                }
            }
        }

        /// <summary>
        /// Removes application configuration items (using specified cache key) from cache
        /// </summary>
        /// <param name="cacheKey">Cache key</param>
        /// <param name="publishCacheChangeEvent">This parameter is specifying publish cache change event</param>
        public void RemoveApplicationConfigurationItems(String cacheKey, Boolean publishCacheChangeEvent)
        {
            if (this._isApplicationConfigurationItemCacheEnabled)
            {
                try
                {
                    _cacheManager.Remove(cacheKey);
                    if (_isDistributedCacheWithNotificationEnabled)
                        _cacheSynchronizationHelper.NotifyLocalCacheRemoval(cacheKey);
                }
                catch (Exception ex)
                {
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, String.Format("Error occurred : {0}", ex.Message));
                    new ExceptionHandler(ex);
                }
            }
        }

        /// <summary>
        /// Removes application configuration items from cache
        /// </summary>
        /// <param name="eventSource">Event source filter</param>
        /// <param name="eventSubscriber">Event subscriber filter</param>
        /// <param name="publishCacheChangeEvent">This parameter is specifying publish cache change event</param>
        public void RemoveApplicationConfigurationItems(EventSource eventSource, EventSubscriber? eventSubscriber, Boolean publishCacheChangeEvent)
        {
            RemoveApplicationConfigurationItems(CacheKeyGenerator.GetApplicationConfigurationItemsListCacheKey(eventSource, eventSubscriber), publishCacheChangeEvent);
        }

        /// <summary>
        /// Removes application configuration all items from cache
        /// </summary>
        /// <param name="publishCacheChangeEvent">This parameter is specifying publish cache change event</param>
        public void RemoveAllApplicationConfigurationItemsFromCache(Boolean publishCacheChangeEvent)
        {
            if (this._isApplicationConfigurationItemCacheEnabled)
            {
                Collection<String> allKeys = _cacheManager.GetAllCacheKeys();
                if (allKeys.Any())
                {
                    String keyPrefix = CacheKeyGenerator.GetApplicationConfigurationItemsByEventSourceAndSubscriberCacheKeyPrefix();
                    foreach (String key in allKeys)
                    {
                        if (key.StartsWith(keyPrefix, StringComparison.OrdinalIgnoreCase))
                        {
                            RemoveApplicationConfigurationItems(key, publishCacheChangeEvent);
                        }
                    }
                }
            }
        }

        #endregion

        #region Private Methods

        #endregion

        #endregion
    }
}