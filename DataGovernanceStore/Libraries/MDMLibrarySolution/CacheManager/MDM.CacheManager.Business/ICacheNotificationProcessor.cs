using System;
using System.Collections.ObjectModel;
using System.Text;
using Microsoft.ApplicationServer.Caching;

namespace MDM.CacheManager.Business
{
    internal interface ICacheNotificationProcessor
    {
        /// <summary>
        /// Captures and processes any Distributed cache notification failures, by removing the corresponding caches from local cache.
        /// </summary>        
        /// <param name="cacheName">The Cache name for which the notification occurred.</param>
        /// <param name="notificationDescriptor">Specifies the cache notification descriptor details.</param>
        void FailureNotificationCallBack(String cacheName, DataCacheNotificationDescriptor notificationDescriptor);

        /// <summary>
        /// Captures and processed any Distributed cache notification call backs, by removing the corresponding cache data from local cache.
        /// </summary>
        /// <param name="cacheName">The Cache name for which the notification occurred.</param>
        /// <param name="regionName">The region name for which the notification occurred.</param>
        /// <param name="key">The Cache key for which the notification occurred.</param>
        /// <param name="version">The version of the cached object for which the notification occurred.</param>
        /// <param name="cacheOperation">The Cache operation which caused the notification.</param>
        /// <param name="notificationDescriptor">Specifies the cache notification descriptor details.</param>
        void CacheNotificationCallBack(String cacheName, String regionName, String key, DataCacheItemVersion version,
            DataCacheOperations cacheOperation, DataCacheNotificationDescriptor notificationDescriptor);
    }
}
