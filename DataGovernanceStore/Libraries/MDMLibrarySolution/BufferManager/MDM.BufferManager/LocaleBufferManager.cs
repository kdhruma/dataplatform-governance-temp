using System;
using System.Collections.ObjectModel;

namespace MDM.BufferManager
{
    using MDM.BusinessObjects;
    using MDM.CacheManager.Business;
    using MDM.Core;
    using MDM.ExceptionManager;
    using MDM.Utility;

    /// <summary>
    ///  Specifies locale buffer manager
    /// </summary>
    public class LocaleBufferManager : BusinessLogicBase
    {
        #region Fields

        /// <summary>
        /// field denoting cache manager of ICache
        /// </summary>
        private ICache _cacheManager = null;

        /// <summary>
        /// field denoting cache manager of distributed cache
        /// </summary>
        private IDistributedCache _distributedCacheManager = null;

        /// <summary>
        /// field denoting locale cache is enabled or not.
        /// </summary>
        private Boolean _isLocaleCacheEnabled = false;

        /// <summary>
        /// Field denotes whether the Distributed Cache with notification has been enabled.
        /// </summary>
        private Boolean _isDistributedCacheWithNotificationEnabled = false;

        /// <summary>
        /// Field denotes an instance of the CacheSynchronizationHelper.
        /// </summary>
        private CacheSynchronizationHelper _cacheSynchronizationHelper = null;

        #endregion

        #region Properties

        /// <summary>
        /// Property denoting locale cache is enabled or not.
        /// </summary>
        public Boolean IsLocaleCacheEnabled
        {
            get
            {
                return _isLocaleCacheEnabled;
            }
        }

        /// <summary>
        /// Property denoting Distributed cache notification is enabled or not.
        /// </summary>
        public Boolean IsDistributedCacheWithNotificationEnabled
        {
            get
            {
                return _isDistributedCacheWithNotificationEnabled;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Instantiate Locale Buffer Manager
        /// </summary>
        public LocaleBufferManager()
        {
            try
            {
                this._isDistributedCacheWithNotificationEnabled = AppConfigurationHelper.GetAppConfig<Boolean>("MDMCenter.DistributedCacheWithNotification.Enabled", false);

                if (_isDistributedCacheWithNotificationEnabled)
                {
                    _cacheSynchronizationHelper = new CacheSynchronizationHelper();
                    _cacheManager = CacheFactory.GetCache();

                    this._isLocaleCacheEnabled = _cacheManager != null ? true : false;
                }
                else
                {
                    _distributedCacheManager = CacheFactory.GetDistributedCache();
                    this._isLocaleCacheEnabled = _distributedCacheManager != null ? true : false;
                }
            }
            catch
            {
                this._isLocaleCacheEnabled = false;
            }
        }

        #endregion

        #region Methods

        #region Public Methods

        /// <summary>
        /// Get all the locales in system
        /// </summary>
        /// <returns>collection of locales </returns>
        public LocaleCollection GetAllLocales()
        {
            return GetLocalesByCacheKey(CacheKeyGenerator.GetAllLocalesCacheKey());
        }

        /// <summary>
        /// Finds all available locales from cache
        /// </summary>
        /// <returns>collection of locales if found in distributed cache otherwise null</returns>
        public LocaleCollection FindAvailableLocales()
        {
            return GetLocalesByCacheKey(CacheKeyGenerator.GetAvailableLocalesCacheKey());
        }
        
        /// <summary>
        /// Update locales in cache
        /// </summary>
        /// <param name="locales">This parameter is specifying collection of locale to be update in cache.</param>
        /// <param name="numberOfRetry">This parameter is specifying number of retry to update object in cache.</param>
        public void UpdateAvailableLocales(LocaleCollection locales, Int32 numberOfRetry)
        {
            UpdateLocaleCache(CacheKeyGenerator.GetAvailableLocalesCacheKey(), locales, numberOfRetry);
        }

        /// <summary>
        /// Update locales in cache
        /// </summary>
        /// <param name="locales">This parameter is specifying collection of locale to be update in cache.</param>
        /// <param name="numberOfRetry">Retry strategy to update cache</param>
        public void UpdateAllLocales(LocaleCollection locales, Int32 numberOfRetry)
        {
            UpdateLocaleCache(CacheKeyGenerator.GetAllLocalesCacheKey(), locales, numberOfRetry);
        }
        
        /// <summary>
        /// Removes all available locales from cache.
        /// </summary>
        /// <param name="publishCacheChangeEvent">This parameter is specifying publish cache change event.</param>
        /// <returns>True if object deleted successfully otherwise false.</returns>
        public Boolean RemoveAvailableLocales(Boolean publishCacheChangeEvent)
        {
            return RemoveLocalesFromCache(CacheKeyGenerator.GetAvailableLocalesCacheKey());
        }

        #endregion

        #region Private Methods

        #region Cache Utility Helper Methods

        private void UpdateLocaleCache(String cacheKey, LocaleCollection locales, Boolean isDataModified = false)
        {
            if (this.IsLocaleCacheEnabled)
            {
                DateTime expiryTime = DateTime.Now.AddDays(10);

                if (IsDistributedCacheWithNotificationEnabled)
                {
                    _cacheSynchronizationHelper.NotifyLocalCacheInsert(cacheKey, expiryTime, isDataModified);
                    _cacheManager.Set(cacheKey, locales, expiryTime);
                }
                else
                {
                    _distributedCacheManager.Set(cacheKey, locales, expiryTime);
                }
            }
        }

        #endregion

        private LocaleCollection GetLocalesByCacheKey(String cacheKey)
        {
            //Logic:
            //If the Distributed Cache notification is enabled then the cache type will be ICache
            //Else cache type will be Distributed(Appfabric) cache.
            //Reason : For the performance reason moved locale object to ICache when Distributed cache notification is enabled.
            //So if the cache object modified it will notify other servers.
            //Let say if Distributed Cache notification is disabled can't notify(update/clear data if it modified) all servers so Cache type will be Distributed cache.

            LocaleCollection locales = null;

            if (this.IsLocaleCacheEnabled)
            {
                object localeObj = null;

                try
                {
                    if (IsDistributedCacheWithNotificationEnabled)
                    {
                        localeObj = _cacheManager.Get(cacheKey);
                    }
                    else
                    {
                        localeObj = _distributedCacheManager.Get(cacheKey);
                    }
                }
                catch
                {
                }
                if (localeObj != null && localeObj is LocaleCollection)
                    locales = (LocaleCollection)localeObj;
            }

            return locales;
        }

        private Boolean RemoveLocalesFromCache(String cacheKey)
        {
            //Logic:
            //If the Distributed Cache notification is enabled then the cache type will be ICache
            //Else cache type will be Distributed(Appfabric) cache.
            //Reason : For the performance reason moved locale object to ICache when Distributed cache notification is enabled.
            //So if the cache object modified it will notify other servers.
            //Let say if Distributed Cache notification is disabled can't notify(update/clear data if it modified) all servers so Cache type will be Distributed cache.

            Boolean success = false;

            if (this.IsLocaleCacheEnabled)
            {
                try
                {
                    if (_isDistributedCacheWithNotificationEnabled)
                    {
                        _cacheSynchronizationHelper.NotifyLocalCacheRemoval(cacheKey);
                        success = _cacheManager.Remove(cacheKey);
                    }
                    else
                    {
                        success = _distributedCacheManager.Remove(cacheKey);
                    }

                }
                catch
                {
                }
            }
            return success;
        }

        private void UpdateLocaleCache(String cacheKey, LocaleCollection locales, Int32 numberOfRetry)
        {
            //Logic:
            //If the Distributed Cache notification is enabled then the cache type will be ICache
            //Else cache type will be Distributed(Appfabric) cache.
            //Reason : For the performance reason moved locale object to ICache when Distributed cache notification is enabled.
            //So if the cache object modified it will notify other servers.
            //Let say if Distributed Cache notification is disabled can't notify(update/clear data if it modified) all servers so Cache type will be Distributed cache.

            if (this.IsLocaleCacheEnabled)
            {
                try
                {
                    UpdateLocaleCache(cacheKey, locales);
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
                            UpdateLocaleCache(cacheKey, locales);

                            retrySuccess = true;
                            break;
                        }
                        catch
                        {
                        }
                    }

                    if (!retrySuccess)
                    {
                        //TODO:: What to do if update fails after retry too..
                        ExceptionHandler exceptionHandler = new ExceptionHandler(ex);
                    }
                }
            }

        }

        #endregion

        #endregion
    }
}
