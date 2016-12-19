using System;
using System.Diagnostics;

namespace MDM.ProfileManager.Business
{
    using MDM.BusinessObjects;
    using MDM.CacheManager.Business;
    using MDM.Core;
    using MDM.ExceptionManager;
    using MDM.Utility;
    using RS.MDM.ConfigurationObjects;

    /// <summary>
    /// Specifies SelectorConfiguration buffer manager
    /// </summary>
    public class SelectorConfigurationBufferManager : BusinessLogicBase
    {
        #region Fields

        /// <summary>
        /// Field denoting cache manager of cache
        /// </summary>
        private readonly ICache _cacheManager = null;

        /// <summary>
        /// Field denoting cache is enabled or not
        /// </summary>
        private readonly Boolean _isCacheEnabled = false;

        /// <summary>
        /// Field denotes whether the Distributed Cache with notification has been enabled
        /// </summary>
        private readonly Boolean _isDistributedCacheWithNotificationEnabled = false;

        /// <summary>
        /// Field denotes an instance of the CacheSynchronizationHelper
        /// </summary>
        private readonly CacheSynchronizationHelper _cacheSynchronizationHelper = null;

        #endregion

        #region Constructors

        /// <summary>
        /// Instantiate SelectorConfiguration Buffer Manager
        /// </summary>
        public SelectorConfigurationBufferManager()
        {
            try
            {
                //Get AppConfig which specify whether cache is enabled or not
                this._isDistributedCacheWithNotificationEnabled = AppConfigurationHelper.GetAppConfig<Boolean>("MDMCenter.DistributedCacheWithNotification.Enabled", false);
                _isCacheEnabled = true;

                _cacheManager = CacheFactory.GetCache();
                if (_isDistributedCacheWithNotificationEnabled)
                    _cacheSynchronizationHelper = new CacheSynchronizationHelper();

                if (_cacheManager == null)
                    this._isCacheEnabled = false;
            }
            catch
            {
                this._isCacheEnabled = false;
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Property denoting cache is enabled or not
        /// </summary>
        public Boolean IsCacheEnabled
        {
            get
            {
                return _isCacheEnabled;
            }
        }

        #endregion

        #region Methods

        #region Public Methods

        /// <summary>
        /// Returns SelectorConfiguration for specified user from cache
        /// </summary>
        /// <returns>SelectorConfiguration if found in internal cache otherwise null</returns>
        public SelectorConfiguration FindUserSelectorConfiguration(Int32 userId)
        {
            SelectorConfiguration result = null;

            if (this._isCacheEnabled)
            {
                String cacheKey = CacheKeyGenerator.GetUserSelectorConfigurationCacheKey(userId);

                object cacheItemObj = null;

                try
                {
                    cacheItemObj = _cacheManager.Get(cacheKey);
                }
                catch (Exception ex)
                {
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, String.Format("Error occurred : {0}", ex.Message));
                    new ExceptionHandler(ex);
                }

                SelectorConfiguration cacheItem = cacheItemObj as SelectorConfiguration;
                if (cacheItem != null)
                {
                    result = (SelectorConfiguration)cacheItem.Clone();
                }
            }

            return result;
        }

        /// <summary>
        /// Update SelectorConfiguration in internal cache
        /// </summary>
        /// <param name="selectorConfiguration">This parameter is specifying SelectorConfiguration to be update in cache</param>
        /// <param name="userId">User Id</param>
        /// <param name="numberOfRetry">This parameter is specifying number of retry to update object in cache</param>
        /// <param name="isDataUpdated">Specifies whether the cache data object is updated and inserted to local cache</param> 
        public void UpdateUserSelectorConfiguration(SelectorConfiguration selectorConfiguration, Int32 userId, Int32 numberOfRetry, Boolean isDataUpdated = false)
        {
            if (this._isCacheEnabled)
            {
                if (selectorConfiguration == null)
                    throw new ArgumentException("SelectorConfiguration is null.");

                String cacheKey = CacheKeyGenerator.GetUserSelectorConfigurationCacheKey(userId);

                DateTime expiryTime = DateTime.Now.AddHours(12);

                //Clone the object before set to cache
                SelectorConfiguration clonedSelectorConfiguration = (SelectorConfiguration)selectorConfiguration.Clone();

                try
                {
                    if (_isDistributedCacheWithNotificationEnabled)
                        _cacheSynchronizationHelper.NotifyLocalCacheInsert(cacheKey, expiryTime, isDataUpdated);

                    _cacheManager.Set(cacheKey, clonedSelectorConfiguration, expiryTime);
                }
                catch (Exception ex)
                {
                    //Retry < 0 means just ignore update if failed..
                    if (numberOfRetry < 0)
                        return;

                    bool retrySuccess = false;

                    for (int i = 0; i < numberOfRetry; i++)
                    {
                        try
                        {
                            if (_isDistributedCacheWithNotificationEnabled)
                                _cacheSynchronizationHelper.NotifyLocalCacheInsert(cacheKey, expiryTime, isDataUpdated);

                            _cacheManager.Set(cacheKey, clonedSelectorConfiguration, expiryTime);
                            
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
        /// Removes SelectorConfiguration from cache for specified user
        /// </summary>
        /// <param name="userId">User Id</param>
        /// <param name="publishCacheChangeEvent">This parameter is specifying publish cache change event</param>
        /// <returns>True if object deleted successfully otherwise false</returns>
        public Boolean RemoveUserSelectorConfiguration(Int32 userId, Boolean publishCacheChangeEvent)
        {
            Boolean success = false;

            if (this._isCacheEnabled)
            {
                String cacheKey = CacheKeyGenerator.GetUserSelectorConfigurationCacheKey(userId);

                try
                {       
                    success = _cacheManager.Remove(cacheKey);
                    if (_isDistributedCacheWithNotificationEnabled)
                        _cacheSynchronizationHelper.NotifyLocalCacheRemoval(cacheKey);
                }
                catch (Exception ex)
                {
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, String.Format("Error occurred : {0}", ex.Message));
                    new ExceptionHandler(ex);
                }
            }

            return success;
        }

        #endregion

        #region Private Methods

        #endregion

        #endregion
    }
}