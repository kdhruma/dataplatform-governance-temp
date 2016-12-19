using Microsoft.ApplicationServer.Caching;
using System;
using System.Text;

namespace MDM.CacheManager.Business
{
    using MDM.BusinessObjects;
    using MDM.Core;
    using MDM.ExceptionManager.Handlers;
    using MDM.BusinessObjects.Diagnostics;

    /// <summary>
    /// Processes the notification events for the JobService from the Distributed cache. 
    /// </summary>
    internal class JobServiceCacheNotificationProcessor : CacheNotificationProcessor, ICacheNotificationProcessor
    {
        #region Overridden Methods

        /// <summary>
        /// Removes the local cache data that are dependent on Distributed cache.
        /// </summary>
        protected override void ClearLocalCacheDataDependentOnDistributedCache()
        {
            try
            {
                ICache localCache = CacheFactory.GetCache();
                localCache.ClearCache();

                if (Constants.TRACING_ENABLED)
                {
                    EventLogHandler eventLogHandler = new EventLogHandler();
                    eventLogHandler.WriteInformationLog("Cleared all local cache data.", 0);
                }
            }
            catch (Exception ex)
            {
                String errorMessage = String.Format("Failure notification processing failed with the following exception : {0}", ex.Message);
                EventLogHandler eventLogHandler = new EventLogHandler();
                eventLogHandler.WriteErrorLog(errorMessage, 0);
            }
        }

        /// <summary>
        /// Removes the Job service local cache data.
        /// </summary>
        protected override void RemoveFromLocalCacheIfDataUpdated(String distributedVersionCacheKey, DataCacheOperations cacheOperation)
        {
            try
            {
                String localCacheKey = GetJobServiceLocalCacheKey(distributedVersionCacheKey);

                ICache localCache = CacheFactory.GetCache();
                localCache.Remove(localCacheKey);

                // Remove the local cache data dependent on the notified cache key
                RemoveDependentDataFromCache(localCache, localCacheKey); 

                if (cacheOperation == DataCacheOperations.ReplaceItem)
                {
                    ReloadApplicationConstants(localCacheKey);
                }

                if (Constants.TRACING_ENABLED)
                {
                    EventLogHandler eventLogHandler = new EventLogHandler();
                    eventLogHandler.WriteInformationLog(String.Format("Removed data associated with cache key {0} from local cache.", localCacheKey), 0);
                }
            }
            catch (Exception ex)
            {
                String errorMessage = String.Format("Cache notification processing for key {0} failed with the following exception : {1}", distributedVersionCacheKey, ex.Message);
                EventLogHandler eventLogHandler = new EventLogHandler();
                eventLogHandler.WriteErrorLog(errorMessage, 0);
            }
        }

        #endregion Overridden Methods

        #region Private Methods

        /// <summary>
        /// Returns the cache key for Job Service.
        /// </summary>
        private String GetJobServiceLocalCacheKey(String distributedVersionCacheKey)
        {
            String localCacheKey = distributedVersionCacheKey.Replace(CacheKeyContants.DISTRIBUTED_VERSION_PREFIX_KEY, String.Empty);

            Type cacheObjectType = CacheKeyGenerator.GetObjectType(localCacheKey);

            if (cacheObjectType != null)
            {
                if (cacheObjectType.Equals(typeof(Container)))
                {
                    localCacheKey = CacheKeyGenerator.GetAllCachedDataModelContainersCacheKey();
                }
                else if (cacheObjectType.Equals(typeof(EntityType)))
                {
                    localCacheKey = CacheKeyGenerator.GetAllCachedDataModelEntityTypesCacheKey();
                }
                else if (cacheObjectType.Equals(typeof(RelationshipType)))
                {
                    localCacheKey = CacheKeyGenerator.GetAllCachedDataModelRelationshipTypesCacheKey();
                }
                else if (cacheObjectType.Equals(typeof(CategoryCollection)))
                {
                    localCacheKey = CacheKeyGenerator.GetAllCachedDataModelCategoriesCacheKey();
                }
                else if (cacheObjectType.Equals(typeof(Organization)))
                {
                    localCacheKey = CacheKeyGenerator.GetAllCachedDataModelOrganizationsCacheKey();
                }
                else if (cacheObjectType.Equals(typeof(Hierarchy)))
                {
                    localCacheKey = CacheKeyGenerator.GetAllCachedDataModelHierarchiesCacheKey();
                }
            }

            return localCacheKey;
        }

        private void RemoveDependentDataFromCache(ICache localCache, String notifiedCacheKey)
        {
            if (String.Compare(notifiedCacheKey, CacheKeyGenerator.GetAvailableLocalesCacheKey(), StringComparison.InvariantCultureIgnoreCase) == 0)
            {
                localCache.Remove(CacheKeyGenerator.GetAvailableLocaleEnumsCacheKey());
            }
        }

        #endregion Private Methods
    }
}
