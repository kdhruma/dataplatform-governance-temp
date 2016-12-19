using System;
using System.Diagnostics;

namespace MDM.BufferManager
{
    using MDM.CacheManager.Business;
    using MDM.Core;
    using MDM.Utility;

    /// <summary>
    /// Generic cache manager 
    /// </summary>
    /// <typeparam name="TBusinessObjectCollection">The type of the business object collection.</typeparam>
    public class CacheBufferManager<TBusinessObjectCollection> : BusinessLogicBase where TBusinessObjectCollection : ICloneable
    {
        #region Fields

        /// <summary>
        /// field denoting cache manager of cache
        /// </summary>
        private ICache _cacheManager = null;

        /// <summary>
        /// field indicates whether cache is enabled or not.
        /// </summary>
        private Boolean _isCacheEnabled = false;
        
        /// <summary>
        /// The cache key
        /// </summary>
        private String _cacheKey = String.Empty;

        /// <summary>
        /// Field denotes whether the Distributed Cache with notification has been enabled.
        /// </summary>
        private Boolean _isDistributedCacheWithNotificationEnabled = false;

        /// <summary>
        /// Field denotes an instance of the CacheSynchronizationHelper.
        /// </summary>
        private CacheSynchronizationHelper _cacheSynchronizationHelper = null;


        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CacheBufferManager{TBusinessObjectCollection}"/> class.
        /// </summary>
        /// <param name="cacheKey">The cache key.</param>
        /// <param name="isCacheEnabledKey">The path in config identifies whether cache enabled for the specific entity</param>
        public CacheBufferManager(String cacheKey, String isCacheEnabledKey)
        {
            try
            {
                if (!String.IsNullOrWhiteSpace(isCacheEnabledKey))
                {
                    //Get AppConfig which specify whether cache is enabled for entity or not
                    Boolean isCacheEnabled = AppConfigurationHelper.GetAppConfig<Boolean>(isCacheEnabledKey, true);
                    this._isCacheEnabled = isCacheEnabled;
                }
                else
                {
                    this._isCacheEnabled = true;
                }

                this._cacheKey = cacheKey;
                this._isDistributedCacheWithNotificationEnabled = AppConfigurationHelper.GetAppConfig<Boolean>("MDMCenter.DistributedCacheWithNotification.Enabled", false);

                if (_isCacheEnabled)
                {
                    _cacheManager = CacheFactory.GetCache();

                    if (_isDistributedCacheWithNotificationEnabled)
                        _cacheSynchronizationHelper = new CacheSynchronizationHelper();
                }

                if (_cacheManager == null)
                {
                    this._isCacheEnabled = false;
                }
            }
            catch
            {
                this._isCacheEnabled = false;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CacheBufferManager{TBusinessObjectCollection}"/> class.
        /// </summary>
        /// <param name="isCacheEnabledKey">The is cache enabled key.</param>
        public CacheBufferManager(String isCacheEnabledKey)
            : this(string.Empty, isCacheEnabledKey)
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// Property denoting business object cache is enabled or not.
        /// </summary>
        public Boolean IsCacheEnabled
        {
            get
            {
                return _isCacheEnabled;
            }
        }

        #endregion Properties

        /// <summary>
        /// Finds all available business objects from cache
        /// </summary>
        /// <param name="cacheKey">Indicates the cache key which stores all the availible business objects</param>
        /// <returns>Returns collection of business objects if found in internal cache; otherwise null</returns>
        public TBusinessObjectCollection GetAllObjectsFromCache(String cacheKey = "")
        {
            TBusinessObjectCollection entityCollection = default(TBusinessObjectCollection);

            if (!String.IsNullOrEmpty(cacheKey))
            {
                this._cacheKey = cacheKey;
            }

            if (this._isCacheEnabled)
            {
                object cacheObj = null;

                try
                {
                    cacheObj = _cacheManager.Get(_cacheKey);
                }
                catch (Exception ex)
                {
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, String.Format("Error occurred : {0}", ex.Message));
                }

                if (cacheObj is TBusinessObjectCollection)
                {
                    TBusinessObjectCollection businessObjectCollection = (TBusinessObjectCollection)cacheObj;
                    entityCollection = (TBusinessObjectCollection)businessObjectCollection.Clone();
                }
            }

            return entityCollection;
        }

        /// <summary>
        /// Set collection of business objects into internal cache
        /// </summary>
        /// <param name="businessObjectCollection">Indicates collection of business objects to be set into cache.</param>
        /// <param name="numberOfRetry">Indicates number of retry allowed to update object in cache.</param>
        /// <param name="cacheKey">Indicates a key to store object in cache.</param>
        /// <param name="isDataModified">Indicates whether data is modified or not.</param>
        public void SetBusinessObjectsToCache(TBusinessObjectCollection businessObjectCollection, Int32 numberOfRetry, String cacheKey = "", Boolean isDataModified = false)
        {
            if (this._isCacheEnabled)
            {

                DateTime expiryTime = DateTime.Now.AddMonths(5);

                //Clone the object before set to cache.
                TBusinessObjectCollection clonnedObjects = (TBusinessObjectCollection)businessObjectCollection.Clone();

                if (!String.IsNullOrEmpty(cacheKey))
                {
                    this._cacheKey = cacheKey;
                }

                try
                {
                    if (_isDistributedCacheWithNotificationEnabled)
                        _cacheSynchronizationHelper.NotifyLocalCacheInsert(_cacheKey, expiryTime, isDataModified);
                    _cacheManager.Set(_cacheKey, clonnedObjects, expiryTime);
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
                                _cacheSynchronizationHelper.NotifyLocalCacheInsert(_cacheKey, expiryTime, isDataModified);

                            _cacheManager.Set(_cacheKey, businessObjectCollection, expiryTime);
                            retrySuccess = true;
                            break;
                        }
                        catch {}
                    }

                    if (!retrySuccess)
                    {
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, String.Format("Error occurred : {0}", ex.Message));
                    }
                }
            }
        }


        /// <summary>
        /// Removes all business objects from cache.
        /// </summary>
        /// <param name="publishCacheChangeEvent">Indicates publish cache change event.</param>
        /// <param name="cacheKey">Indicates cache key.</param>
        /// <param name="ObjectName">Indicates type of object to be removed from cache.</param>
        /// <returns>Returns true if object is deleted successfully; otherwise false.</returns>
        public Boolean RemoveBusinessObjectFromCache(Boolean publishCacheChangeEvent, String cacheKey = "", String ObjectName = "")
        {
            // TODO - Need to implement delete functionality by ObjectName
            Boolean success = false;

            if (this._isCacheEnabled)
            {
                if (!String.IsNullOrEmpty(cacheKey))
                {
                    this._cacheKey = cacheKey;
                }

                try
                {
                    if (_isDistributedCacheWithNotificationEnabled)
                        _cacheSynchronizationHelper.NotifyLocalCacheRemoval(_cacheKey);   

                    success = _cacheManager.Remove(_cacheKey);
                }
                catch (Exception ex)
                {
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, String.Format("Error occurred : {0}", ex.Message));
                }
            }

            return success;
        }
    }
}
