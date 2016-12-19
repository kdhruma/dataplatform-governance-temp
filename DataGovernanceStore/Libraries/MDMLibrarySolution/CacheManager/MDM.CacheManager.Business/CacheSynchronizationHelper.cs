using System;
using System.Diagnostics;

namespace MDM.CacheManager.Business
{
    using MDM.Core;
    using MDM.ExceptionManager;
    using MDM.ExceptionManager.Handlers;

    /// <summary>
    /// Provides methods that help in synchronization of the local caches.
    /// </summary>
    public class CacheSynchronizationHelper
    {
        #region Fields

        /// <summary>
        /// Field denoting cache manager of local cache
        /// </summary>
        private ICache _localCache = null;

        /// <summary>
        /// Field denotes an instance of the Distributed Cache with notification.
        /// </summary>
        private IDistributedCache _distributedCacheWithNotification = null;

        #endregion

        #region Constructors

        /// <summary>
        /// Instantiates the CacheSynchronizationHelper.
        /// </summary>
        public CacheSynchronizationHelper()
        {
            _localCache = CacheFactory.GetCache();
            _distributedCacheWithNotification = CacheFactory.GetDistributedCacheWithNotification();
        }

        #endregion

        #region Properties

        #endregion

        #region Methods

        #region Public Methods

        /// <summary>
        /// Notifies the Distributed cache, when the local cache data is modified.
        /// </summary>
        /// <param name="localCacheKey">Specifies the Cache key of the data to be cached.</param>
        /// <param name="expiryTime">Specifies the Cache duration.</param>
        /// <param name="isCacheDataModified">Specifies whether the cache data object is updated and inserted to local cache. The cache version is updated 
        /// only if the data is modified and not for all local cache inserts (i.e. read from DB or WCF service and inserted as local copy).</param>
        public void NotifyLocalCacheInsert(String localCacheKey, DateTime expiryTime, Boolean isCacheDataModified)
        {
            EventLogHandler eventLogHandler = new EventLogHandler();

            String distributedVersionCacheKey = GetDistributedVersionCacheKey(localCacheKey);
            String localVersionCacheKey = GetLocalVersionCacheKey(localCacheKey);

            Object versionObj = !isCacheDataModified? _distributedCacheWithNotification.Get(distributedVersionCacheKey) : null;
            if (versionObj == null)
            {
                if (Constants.TRACING_ENABLED)
                    eventLogHandler.WriteInformationLog(String.Format("Updating distributed and local cache version for key {0}.", localCacheKey), 0);

                // The version of the data is maintained in local cache and the distributed cache with notification enabled, to identify the latest data when 
                // cache notification is processed.
                Guid versionGuid = Guid.NewGuid();

                _distributedCacheWithNotification.Set(distributedVersionCacheKey, versionGuid, expiryTime);
                _localCache.Set(localVersionCacheKey, versionGuid, expiryTime);

                if (Constants.TRACING_ENABLED)
                    eventLogHandler.WriteInformationLog(String.Format("Completed updating distributed and local cache version for key {0}.", localCacheKey), 0);
            }
            else
            {
                if (Constants.TRACING_ENABLED)
                    eventLogHandler.WriteInformationLog(String.Format("Updating local cache version for key {0}.", localCacheKey), 0);

                // If data is not modified, the existing distributed cache version is update in the local cache.
                _localCache.Set(localVersionCacheKey, (Guid)versionObj, expiryTime);

                if (Constants.TRACING_ENABLED)
                    eventLogHandler.WriteInformationLog(String.Format("Completed updating local cache version for key {0}.", localCacheKey), 0);
            }
        }

        /// <summary>
        /// Notifies the Distributed cache, when the local cache data is modified.
        /// </summary>
        /// <param name="localCacheKey">Specifies the Cache key of the data to be cached.</param>
        /// <param name="timeoutInMinutes">Specifies the cache timeout value in minutes.</param>
        /// <param name="isCacheDataModified">Specifies whether the cache data object is updated and inserted to local cache. The cache version is updated 
        /// only if the data is modified and not for all local cache inserts (i.e. read from DB or WCF service and inserted as local copy).</param>
        public void NotifyLocalCacheInsert(String localCacheKey, Int32 timeoutInMinutes, Boolean isCacheDataModified)
        {
            DateTime expirationTime = DateTime.Now.AddMinutes(timeoutInMinutes);
            NotifyLocalCacheInsert(localCacheKey, expirationTime, isCacheDataModified);            
        }

        /// <summary>
        /// Removes the Distributed cache version associated with the specified cache key. This would trigger the notifications to clear all local caches.
        /// </summary>
        /// <param name="localCacheKey">The cache key of the object to be removed.</param>
        /// <returns>Specifies whether the cache removal was successful.</returns>
        public Boolean NotifyLocalCacheRemoval(String localCacheKey)
        {
            EventLogHandler eventLogHandler = new EventLogHandler();

            if (Constants.TRACING_ENABLED)
                eventLogHandler.WriteInformationLog(String.Format("Removing distributed cache version for key {0}.", localCacheKey), 0);

            String notifierCacheKey = GetDistributedVersionCacheKey(localCacheKey);
            Boolean isRemoved = _distributedCacheWithNotification.Remove(notifierCacheKey);

            if (Constants.TRACING_ENABLED)
                eventLogHandler.WriteInformationLog(String.Format("Removed distributed cache version for key {0}.", localCacheKey), 0);

            return isRemoved;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Generates the local version Cache key.
        /// </summary>
        /// <param name="localCacheKey"></param>
        /// <returns></returns>
        private String GetLocalVersionCacheKey(String localCacheKey)
        {
            return String.Format("{0}{1}", CacheKeyContants.LOCAL_VERSION_PREFIX_KEY, localCacheKey);
        }

        /// <summary>
        /// Generates the distributed version Cache key.
        /// </summary>
        /// <param name="localCacheKey"></param>
        /// <returns></returns>
        private String GetDistributedVersionCacheKey(String localCacheKey)
        {
            return String.Format("{0}{1}", CacheKeyContants.DISTRIBUTED_VERSION_PREFIX_KEY, localCacheKey);
        }

        #endregion

        #endregion
    }
}
