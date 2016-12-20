using Microsoft.ApplicationServer.Caching;
using System;
using System.Collections.ObjectModel;
using System.Text;

namespace MDM.CacheManager.Business
{
    using MDM.BusinessObjects;
    using MDM.BusinessObjects.Diagnostics;
    using MDM.Core;
    using MDM.ExceptionManager.Handlers;

    /// <summary>
    /// Processes the notification events from the Distributed cache. 
    /// </summary>
    internal class CacheNotificationProcessor : ICacheNotificationProcessor
    {
        #region Constants

        private const Int32 START_EVENT_ID = 1003;
        private const string START_EVENT = "Trace '{0}' was started";
        private const Int32 STOP_EVENT_ID = 1004;
        private const string STOP_EVENT = "Trace '{0}' was stopped";

        #endregion

        #region Methods

        /// <summary>
        /// Captures and processes any Distributed cache notification failures, by removing the corresponding caches from local cache.
        /// </summary>        
        /// <param name="cacheName">The Cache name for which the notification occurred.</param>
        /// <param name="notificationDescriptor">Specifies the cache notification descriptor details.</param>
        public void FailureNotificationCallBack(String cacheName, DataCacheNotificationDescriptor notificationDescriptor)
        {
            LogFailureNotification(cacheName, notificationDescriptor);

            ClearLocalCacheDataDependentOnDistributedCache();
        }

        /// <summary>
        /// Captures and processed any Distributed cache notification call backs, by removing the corresponding cache data from local cache.
        /// </summary>
        /// <param name="cacheName">The Cache name for which the notification occurred.</param>
        /// <param name="regionName">The region name for which the notification occurred.</param>
        /// <param name="key">The Cache key for which the notification occurred.</param>
        /// <param name="version">The version of the cached object for which the notification occurred.</param>
        /// <param name="cacheOperation">The Cache operation which caused the notification.</param>
        /// <param name="notificationDescriptor">Specifies the cache notification descriptor details.</param>
        public void CacheNotificationCallBack(String cacheName, String regionName, String key, DataCacheItemVersion version,
            DataCacheOperations cacheOperation, DataCacheNotificationDescriptor notificationDescriptor)
        {
            LogCacheNotification(cacheName, regionName, key, version, cacheOperation, notificationDescriptor);

            // If any cache region is cleared or removed, then the entire local cache data which are dependent on the distributed cache is cleared.
            if (cacheOperation == DataCacheOperations.ClearRegion || cacheOperation == DataCacheOperations.RemoveRegion)
                ClearLocalCacheDataDependentOnDistributedCache();

            if (cacheOperation == DataCacheOperations.RemoveItem || cacheOperation == DataCacheOperations.ReplaceItem)
            {
                RemoveFromLocalCacheIfDataUpdated(key, cacheOperation);

                TriggerCacheNotificationEvents(cacheOperation, key);
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Triggers the cache notification events that have been subscribed to
        /// </summary>
        /// <param name="cacheOperation">Represents the cache operation type</param>
        /// <param name="distributedVersionCacheKey">Represents the cache key for which notification is triggered</param>
        private void TriggerCacheNotificationEvents(DataCacheOperations cacheOperation, String distributedVersionCacheKey)
        {
            try
            {
                CacheNotificationEventArgs eventArg = new CacheNotificationEventArgs();
                eventArg.CacheKey = distributedVersionCacheKey.Replace(CacheKeyContants.DISTRIBUTED_VERSION_PREFIX_KEY, String.Empty);
                
                switch (cacheOperation)
                {
                    case DataCacheOperations.ReplaceItem:
                        CacheNotificationEventManager.Instance.OnCacheUpdate(eventArg);
                        break;
                    case DataCacheOperations.RemoveItem:
                        CacheNotificationEventManager.Instance.OnCacheRemoval(eventArg);
                        break;
                }
            }
            catch(Exception ex)
            {
                String errorMessage = String.Format("Cache notification event triggering for key {0} failed with the following exception : {1}", distributedVersionCacheKey, ex.Message);
                EventLogHandler eventLogHandler = new EventLogHandler();
                eventLogHandler.WriteErrorLog(errorMessage, 0);
            }
        }

        /// <summary>
        /// Removes the local cache data depending on the cache operation.
        /// </summary>
        protected virtual void RemoveFromLocalCacheIfDataUpdated(String distributedVersionCacheKey, DataCacheOperations cacheOperation)
        {
            try
            {
                String localVersionCacheKey = distributedVersionCacheKey.Replace(CacheKeyContants.DISTRIBUTED_VERSION_PREFIX_KEY,
                    CacheKeyContants.LOCAL_VERSION_PREFIX_KEY);

                // If notification is invoked for CacheOperations - RemoveItem or if the latest version does not exist in local cache 
                // (for CacheOperation - ReplaceItem), then the local cache has to be removed.
                if (cacheOperation == DataCacheOperations.RemoveItem || !IsLatestVersionAvailableInLocalCache(distributedVersionCacheKey, localVersionCacheKey))
                {
                    RemoveLocalCacheDataAndVersion(localVersionCacheKey);

                    if (cacheOperation == DataCacheOperations.ReplaceItem)
                        ReloadApplicationConstants(localVersionCacheKey.Replace(CacheKeyContants.LOCAL_VERSION_PREFIX_KEY, String.Empty));
                }
                else if (Constants.TRACING_ENABLED)
                {
                    String localDataCacheKey = distributedVersionCacheKey.Replace(CacheKeyContants.DISTRIBUTED_VERSION_PREFIX_KEY, String.Empty);
                    EventLogHandler eventLogHandler = new EventLogHandler();
                    eventLogHandler.WriteInformationLog(String.Format("Data for cache key {0} is not removed from local cache as it is up to date.", localDataCacheKey), 0);
                }
            }
            catch (Exception ex)
            {
                String errorMessage = String.Format("Cache notification processing for key {0} failed with the following exception : {1}", distributedVersionCacheKey, ex.Message);
                EventLogHandler eventLogHandler = new EventLogHandler();
                eventLogHandler.WriteErrorLog(errorMessage, 0);
            }
        }

        /// <summary>
        /// Checks if the latest version is already available in the local cache.
        /// </summary>
        private Boolean IsLatestVersionAvailableInLocalCache(String distributedVersionCacheKey, String localVersionCacheKey)
        {
            Guid distributedVersion = GetVersionFromDistributedCache(distributedVersionCacheKey);
            Guid localVersion = GetVersionFromLocalCache(localVersionCacheKey);

            // Check if local version and distributed version match and also that both do not have the default id.
            return (localVersion.CompareTo(distributedVersion) == 0 && localVersion.CompareTo(Guid.Empty) != 0);
        }

        /// <summary>
        /// Returns the version of the object in distributed cache.
        /// </summary>
        private Guid GetVersionFromDistributedCache(String distributedVersionCacheKey)
        {
            Guid versionInDistributedCache = Guid.Empty;

            IDistributedCache distributedCache = CacheFactory.GetDistributedCacheWithNotification();
            Object versionObj = distributedCache.Get(distributedVersionCacheKey);
            if (versionObj != null)
                versionInDistributedCache = (Guid)versionObj;

            return versionInDistributedCache;
        }

        /// <summary>
        /// Returns the version of the object in local cache.
        /// </summary>
        private Guid GetVersionFromLocalCache(String localVersionCacheKey)
        {
            Guid versionInLocalCache = Guid.Empty;

            ICache localCache = CacheFactory.GetCache();
            Object versionObj = localCache.Get(localVersionCacheKey);
            if (versionObj != null)
                versionInLocalCache = (Guid)versionObj;

            return versionInLocalCache;
        }

        /// <summary>
        /// Removes the data and its version details from the local cache.
        /// </summary>
        private void RemoveLocalCacheDataAndVersion(String localVersionCacheKey)
        {
            String localCacheKey = localVersionCacheKey.Replace(CacheKeyContants.LOCAL_VERSION_PREFIX_KEY, String.Empty);

            ICache localCache = CacheFactory.GetCache();
            localCache.Remove(localCacheKey);
            localCache.Remove(localVersionCacheKey);

            #region Clear Model data

            if (String.Compare(localCacheKey, CacheKeyContants.ATTRIBUTE_MODELS_CHANGED_KEY) == 0)
            {
                ClearCacheBasedOnMasterKey(CacheKeyContants.ATTRIBUTE_SEARCH_CACHE_KEY_PREFIX);
            }

            else if (String.Compare(localCacheKey, CacheKeyContants.COMMON_ATTRIBUTE_CHANGED_KEY) == 0)
            {
                ClearCacheBasedOnMasterKey(CacheKeyContants.COMMON_ATTRIBUTE_CACHE_KEY_PREFIX);
            }

            else if (String.Compare(localCacheKey, CacheKeyContants.TECHNICAL_ATTRIBUTE_CHANGED_KEY) == 0)
            {
                ClearCacheBasedOnMasterKey(CacheKeyContants.TECHNICAL_ATTRIBUTE_CACHE_KEY_PREFIX);
            }

            else if (String.Compare(localCacheKey, CacheKeyContants.RELATIONSHIP_HIERARCHY_CHANGED_KEY) == 0)
            {
                ClearCacheBasedOnMasterKey(CacheKeyContants.RELATIONSHIP_HIERARCHY_CACHE_KEY_PREFIX);
            }

            else if (String.Compare(localCacheKey, CacheKeyContants.CONTAINER_ENTITYTYPE_MAPPING_CHANGED_KEY) == 0)
            {
                ClearCacheBasedOnMasterKey(CacheKeyContants.CONTAINER_ENTITYTYPE_MAPPING_CACHE_KEY_PREFIX);
                ClearCacheBasedOnMasterKey(CacheKeyContants.METADATA_PAGE_CONFIG_KEY_PREFIX);
            }

            else if (localCacheKey.Contains(CacheKeyContants.USERROLE_PERMISSIONS_CHANGED_KEY))
            {
                String userId = localCacheKey.Replace(CacheKeyContants.USERROLE_PERMISSIONS_CHANGED_KEY + '_', "");
                ClearCacheBasedOnMasterKey(CacheKeyGenerator.GetUserRolePermissionsCacheKey(ValueTypeHelper.ConvertToInt64(userId)));
            }

            #endregion

            if (localCacheKey.Equals(CacheKeyContants.CATEGORIES_CHANGED_KEY))
            {
                ClearLocalCategoriesCache();
            }

            if (Constants.TRACING_ENABLED)
            {
                EventLogHandler eventLogHandler = new EventLogHandler();
                eventLogHandler.WriteInformationLog(String.Format("Removed data associated with cache key {0} from local cache.", localCacheKey), 0);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cacheKey"></param>
        protected void ReloadApplicationConstants(String cacheKey)
        {
            try
            {
                if (cacheKey == CacheKeyContants.TRACING_ENABLED || cacheKey == CacheKeyContants.PERFORMANCE_TRACING_ENABLED)
                {
                    IDistributedCache distributedCache = CacheFactory.GetDistributedCache();
                    Object cachedData = distributedCache.Get(cacheKey);
                    if (cachedData != null)
                    {
                        if (cacheKey == CacheKeyContants.PERFORMANCE_TRACING_ENABLED)
                            Constants.PERFORMANCE_TRACING_ENABLED = (Boolean)cachedData;
                        else
                            Constants.TRACING_ENABLED = (Boolean)cachedData;
                    }
                }
                else if (cacheKey == CacheKeyContants.DIAGNOSTIC_TRACING_STORAGE_MODE)
                {
                    IDistributedCache distributedCache = CacheFactory.GetDistributedCache();
                    Object cachedData = distributedCache.Get(cacheKey);
                    if (cachedData != null)
                    {
                        Constants.DIAGNOSTIC_TRACING_STORAGE_MODE = (DiagnosticTracingStorageMode)cachedData;
                    }
                }
                else if (cacheKey == CacheKeyContants.DIAGNOSTIC_SYSTEMLOG_LEGACYSOURCESTOSKIP)
                {
                    IDistributedCache distributedCache = CacheFactory.GetDistributedCache();
                    Object cachedData = distributedCache.Get(cacheKey);
                    if (cachedData != null)
                    {
                        Constants.DIAGNOSTIC_SYSTEMLOG_LEGACYSOURCESTOSKIP = cachedData.ToString();
                    }
                }
                else if (cacheKey == CacheKeyContants.DIAGNOSTIC_TRACING_PROFILE)
                {
                    LoadTracingProfile(cacheKey);
                }
                else if (cacheKey == CacheKeyContants.TRANSACTION_SETTINGS)
                {
                    IDistributedCache distributedCache = CacheFactory.GetDistributedCache();

                    Object timeOutAsyncProcess = distributedCache.Get(CacheKeyContants.TRANSACTION_TIMEOUT_ASYNCPROCESS);
                    if (timeOutAsyncProcess != null)
                        Constants.TRANSACTION_TIMEOUT_ASYNCPROCESS = new TimeSpan(0, 0, (Int32)timeOutAsyncProcess);

                    Object timeOutSyncProcess = distributedCache.Get(CacheKeyContants.TRANSACTION_TIMEOUT_SYNCPROCESS);
                    if (timeOutSyncProcess != null)
                        Constants.TRANSACTION_TIMEOUT_SYNCPROCESS = new TimeSpan(0, 0, (Int32)timeOutSyncProcess);

                    Object isolationLevel = distributedCache.Get(CacheKeyContants.DEFAULT_ISOLATION_LEVEL);
                    if (isolationLevel != null)
                        Constants.DEFAULT_ISOLATION_LEVEL = (System.Transactions.IsolationLevel)isolationLevel;
                }
                else if (cacheKey == CacheKeyContants.TRACK_HISTORY_ONLY_ON_VALUE_CHANGE)
                {
                    IDistributedCache distributedCache = CacheFactory.GetDistributedCache();
                    Object cachedData = distributedCache.Get(cacheKey);
                    if (cachedData != null)
                    {
                        Constants.TRACK_HISTORY_ONLY_ON_VALUE_CHANGE = (Boolean)cachedData;
                    }
                }
            }
            catch (Exception exception)
            {
                String errorMessage = String.Format("Reload constants failed with the following exception : {0}", exception.Message);
                EventLogHandler eventLogHandler = new EventLogHandler();
                eventLogHandler.WriteErrorLog(errorMessage, 0);
            }
        }

        private void LoadTracingProfile(String cacheKey)
        {
            IDistributedCache distributedCache = CacheFactory.GetDistributedCache();
            Object cachedData = distributedCache.Get(cacheKey).ToString();

            if (cachedData != null)
            {
                TracingProfile oldProfile = TracingProfile.GetCurrent();
                String tracingProfileAsXml = cachedData.ToString();

                if (!String.IsNullOrWhiteSpace(tracingProfileAsXml))
                {
                    TracingProfile.LoadCurrent(tracingProfileAsXml);
                }
                else
                {
                    TracingProfile.LoadCurrent(new TracingProfile());
                }

                TracingProfile profile = TracingProfile.GetCurrent();
                if (profile.TraceSettings.IsBasicTracingEnabled != oldProfile.TraceSettings.IsBasicTracingEnabled)
                {
                    EventLogHandler eventLogHandler = new EventLogHandler();
                    if (profile.TraceSettings.IsBasicTracingEnabled)
                    {
                        eventLogHandler.WriteInformationLog(String.Format(START_EVENT, profile.Name), START_EVENT_ID);
                    }
                    else
                    {
                        eventLogHandler.WriteInformationLog(String.Format(STOP_EVENT, profile.Name), STOP_EVENT_ID);
                    }
                }
            }
        }

        /// <summary>
        /// Removes the local cache data that are dependent on Distributed cache. i.e. Lookup etc.
        /// </summary>
        protected virtual void ClearLocalCacheDataDependentOnDistributedCache()
        {
            try
            {
                ICache localCache = CacheFactory.GetCache();

                Collection<String> cacheKeys = localCache.GetAllCacheKeys();
                foreach (String cacheKey in cacheKeys)
                {
                    // Finds the local version cache keys and remove them along with the data they represent.
                    if (cacheKey.StartsWith(CacheKeyContants.LOCAL_VERSION_PREFIX_KEY))
                        RemoveLocalCacheDataAndVersion(cacheKey);
                }
            }
            catch (Exception ex)
            {
                String errorMessage = String.Format("Failure notification processing failed with the following exception : {0}", ex.Message);
                EventLogHandler eventLogHandler = new EventLogHandler();
                eventLogHandler.WriteErrorLog(errorMessage, 0);
            }
        }

        private void LogFailureNotification(String cacheName, DataCacheNotificationDescriptor notificationDescriptor)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("A Failure notification has been triggered with the following details:");
            stringBuilder.AppendLine("Trigger Time : " + DateTime.Now);
            stringBuilder.AppendLine("Cache Name: " + cacheName);
            stringBuilder.AppendLine("Notification Descriptor Delegate Id: " + notificationDescriptor.DelegateId);
            stringBuilder.AppendLine("Notification Descriptor Cache name: " + notificationDescriptor.CacheName);

            EventLogHandler eventLogHandler = new EventLogHandler();
            eventLogHandler.WriteWarningLog(stringBuilder.ToString(), 0);
        }

        private void LogCacheNotification(String cacheName, String regionName, String key, DataCacheItemVersion version,
            DataCacheOperations cacheOperation, DataCacheNotificationDescriptor notificationDescriptor)
        {
            if (Constants.TRACING_ENABLED)
            {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.AppendLine("A cache-level notification has been triggered with the following details:");
                stringBuilder.AppendLine("Trigger Time : " + DateTime.Now);
                stringBuilder.AppendLine("Cache Name: " + cacheName);
                stringBuilder.AppendLine("Region Name: " + regionName);
                stringBuilder.AppendLine("Cache Key: " + key);
                stringBuilder.AppendLine("Operation: " + cacheOperation.ToString());
                stringBuilder.AppendLine("Notification Descriptor Delegate Id: " + notificationDescriptor.DelegateId);
                stringBuilder.AppendLine("Finished executing the cache notification callback.");

                EventLogHandler eventLogHandler = new EventLogHandler();
                eventLogHandler.WriteInformationLog(stringBuilder.ToString(), 0);
            }
        }
      
        /// <summary>
        /// Removes the data from the cache.
        /// </summary>
        /// <param name="masterCacheKey">Indicates the master cache key whose value to be cleared.</param>
        private void ClearCacheBasedOnMasterKey(String masterCacheKey)
        {
            ICache localeCache = CacheFactory.GetCache();

            Collection<String> cacheKeys = localeCache.GetAllCacheKeys();

            foreach (String key in cacheKeys)
            {
                if (key.StartsWith(masterCacheKey) || key.EndsWith(masterCacheKey))
                {
                    localeCache.Remove(key);
                }
            }

            if (Constants.TRACING_ENABLED)
            {
                EventLogHandler eventLogHandler = new EventLogHandler();
                eventLogHandler.WriteInformationLog(String.Format("Removed data associated with cache key {0} from local cache.", masterCacheKey), 0);
            }
        }

        /// <summary>
        /// Removes the local cache data that are related to Categories search.
        /// </summary>
        private void ClearLocalCategoriesCache()
        {
            ICache localCache = CacheFactory.GetCache();

            Collection<String> cacheKeys = localCache.GetAllCacheKeys();

            foreach (String key in cacheKeys)
            {
                if (key.StartsWith(CacheKeyContants.CATEGORY_SEARCH_FILTER_KEY_PREFIX))
                {
                    localCache.Remove(key);
                }
            }

            if (Constants.TRACING_ENABLED)
            {
                EventLogHandler eventLogHandler = new EventLogHandler();
                eventLogHandler.WriteInformationLog(String.Format("Removed data associated with cache key {0} from local cache.", CacheKeyContants.CATEGORY_SEARCH_FILTER_KEY_PREFIX), 0);
            }
        }

        #endregion
    }
}
