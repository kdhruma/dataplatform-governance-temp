using Microsoft.ApplicationServer.Caching;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;

namespace MDM.CacheManager.Business
{
    using MDM.Core;

    /// <summary>
    /// Class for Distributed cache with notification enabled.
    /// </summary>
    public sealed class DistributedCacheWithNotification : IDistributedCache
    {        
        #region Fields

        private DataCache _cacheInstance = null;

        private String _cacheName = String.Empty;

        private static object lockObject = new object();

        private static DistributedCacheWithNotification _instance = null;

        const String cacheType = "Distributed notification based local";

        #endregion

        #region Constructors

        /// <summary>
        /// Initialize Distributed Cache with notification.
        /// </summary>
        public static DistributedCacheWithNotification GetDistributedCacheWithNotification()
        {
            if (_instance == null || _instance.CacheInstance == null)
            {
                lock (lockObject)
                {
                    if (_instance == null || _instance.CacheInstance == null)
                    {
                        _instance = new DistributedCacheWithNotification();
                        _instance._cacheName = AppConfiguration.GetSetting("NotificationCacheName");
                        _instance.Initialize("DistributedCacheWithNotification");
                    }
                }
            }

            return _instance;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Cache instance
        /// </summary>
        public DataCache CacheInstance
        {
            get
            {
                return _cacheInstance;
            }
        }

        #endregion

        #region Methods

        #region Private Methods

        private void Initialize(String clientConfigurationName)
        {
            DataCacheFactory factory = null;
            DataCacheFactoryConfiguration dataCacheFactoryConfiguration = null;

            try
            {
                if (_cacheInstance == null)
                {
                    dataCacheFactoryConfiguration = new DataCacheFactoryConfiguration(clientConfigurationName);

                    factory = new DataCacheFactory(dataCacheFactoryConfiguration);

                    _cacheInstance = factory.GetCache(this._cacheName);

                    ICacheNotificationProcessor cacheNotificationCallBackProcessor = GetCacheNotificationProcessor();
                    _cacheInstance.AddFailureNotificationCallback(cacheNotificationCallBackProcessor.FailureNotificationCallBack);
                    
                    DataCacheOperations dataCacheOperationsForNotification = GetDataCacheOperationsForNotification();
                    _cacheInstance.AddCacheLevelCallback(dataCacheOperationsForNotification, cacheNotificationCallBackProcessor.CacheNotificationCallBack);
                }
            }
            catch (Exception ex)
            {
                ApplicationException appException = new ApplicationException(String.Format("{0} cache client initialization failed. Internal Error: {1}", cacheType, ex.Message), ex);
                ExceptionManager.ExceptionHandler exceptionHander = new ExceptionManager.ExceptionHandler(appException);
                throw appException;
            }
        }

        /// <summary>
        /// Instantiates the CacheNotificationProcessor based on the executing program.
        /// </summary>
        /// <returns></returns>
        private ICacheNotificationProcessor GetCacheNotificationProcessor()
        {
            ICacheNotificationProcessor cacheNotificationProcessor = null;

            if (IsJobService())
                cacheNotificationProcessor = new JobServiceCacheNotificationProcessor();
            else
                cacheNotificationProcessor = new CacheNotificationProcessor();
            
            return cacheNotificationProcessor;
        }

        /// <summary>
        /// Checks if the current executing program is 'JobService'. 
        /// </summary>
        private Boolean IsJobService()
        {
            Boolean isJobService = false;

            String serviceType = ConfigurationManager.AppSettings.Get("ServiceType");
            if (!String.IsNullOrWhiteSpace(serviceType))
            {
                switch (serviceType.ToLower())
                {
                    case "imports":
                    case "exports":
                    case "all":
                        isJobService = true;
                        break;
                }
            }
            return isJobService;
        }

        private DataCacheOperations GetDataCacheOperationsForNotification()
        {
            DataCacheOperations dataCacheOperationsForNotification = DataCacheOperations.RemoveItem | DataCacheOperations.ReplaceItem | 
                DataCacheOperations.ClearRegion | DataCacheOperations.RemoveRegion;
            return dataCacheOperationsForNotification;
        }

        #endregion

        #endregion

        #region ICache Members

        /// <summary>
        /// Get the Object stored in cache
        /// </summary>
        /// <param name="key">The cache key used to store the data</param>
        /// <returns>The stored Object 
        /// <remarks>if the Object is not present you would get null</remarks></returns>
        public Object this[String key]
        {
            get
            {
                return Get(key);
            }
        }

        /// <summary>
        /// Get an Object from cache
        /// </summary>
        /// <typeparam name="T">The Object that can be serialized using datacontract serializer</typeparam>
        /// <param name="key">The name of the key to insert to cache</param>
        /// <returns>The cached Object converted to the requested type</returns>    
        public T Get<T>(String key)
        {
            Object keyValue = null;
            keyValue = Get(key);
            return (T)keyValue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public Object Get(String key)
        {
            Object keyValue = null;
            try
            {
                keyValue = CacheInstance.Get(key);
            }
            catch (Exception ex)
            {
                ApplicationException appException = new ApplicationException(String.Format("{0} cache get failed for cache key: {1}. Internal Error: {2}", cacheType, key, ex.Message), ex);
                ExceptionManager.ExceptionHandler exceptionHander = new ExceptionManager.ExceptionHandler(appException);
            }

            return keyValue;
        }

        /// <summary>
        /// Remove the Object from cache. The method returns true even if the item does not exists in the cache.
        /// </summary>
        /// <param name="key">The key for the cached Object</param>
        /// <returns>true</returns>
        public Boolean Remove(String key)
        {
            Boolean result = true;

            try
            {
                // do not use the return value from the appfabric cache. This returns false when the item is not in cache. The item will not be there
                // for vairous reasons like cache eviction or cache restart.
                result = CacheInstance.Remove(key);
            }
            catch (Exception ex)
            {
                ApplicationException appException = new ApplicationException(String.Format("{0} cache remove failed for cache key: {1}. Internal Error: {2}", cacheType, key, ex.Message), ex);
                ExceptionManager.ExceptionHandler exceptionHander = new ExceptionManager.ExceptionHandler(appException);
            }

            return result;
        }

        /// <summary>
        /// Store the Object in Cache for a specific duration and don't allow the CLR to do a Garbage Collection to collect this Object in case of a memory pressure.
        /// </summary>
        /// <param name="key">The cache key used to store the data</param>
        /// <param name="value">The Object to be stored</param>
        /// <param name="expiryTime">The amount of time the Object needs to be in Cache</param>
        public void Set(String key, Object value, DateTime expiryTime)
        {
            try
            {
                TimeSpan interval = expiryTime - DateTime.Now;
                CacheInstance.Put(key, value, interval);
            }
            catch (Exception ex)
            {
                ApplicationException appException = new ApplicationException(String.Format("{0} cache update failed for cache key: {1}. Internal Error: {2}", cacheType, key, ex.Message), ex);
                ExceptionManager.ExceptionHandler exceptionHander = new ExceptionManager.ExceptionHandler(appException);
                throw appException;
            }
        }

        /// <summary>
        /// Set Object in a Cache
        /// </summary>
        /// <typeparam name="T">The Object that can be serialized using DataContract serialize</typeparam>
        /// <param name="key">The Key to insert to the Cache</param>
        /// <param name="instance">The Object Instance</param>
        /// <param name="expiryTime">The Time Duration to hold Object into Cache </param>
        public void Set<T>(String key, T instance, DateTime expiryTime)
        {
            try
            {
                TimeSpan interval = expiryTime - DateTime.Now;
                CacheInstance.Put(key, instance, interval);
            }
            catch (Exception ex)
            {
                ApplicationException appException = new ApplicationException(String.Format("{0} cache update failed for cache key: {1}. Internal Error: {2}", cacheType, key, ex.Message), ex);
                ExceptionManager.ExceptionHandler exceptionHander = new ExceptionManager.ExceptionHandler(appException);
                throw appException;
            }
        }

        /// <summary>
        /// Store the Object in Cache for a sliding duration and don't allow the CLR to do a Garbage Collection to collect this Object in case of a memory pressure.
        /// </summary>
        /// <param name="key">The cache key used to store the data</param>
        /// <param name="value">The Object to be stored</param>
        /// <param name="timeOutInMinutes">The sliding expiration time out the cache needs to be kept alive</param>
        public void Set(String key, Object value, Int32 timeOutInMinutes)
        {
            try
            {
                DateTime dt = DateTime.Now.AddMinutes(timeOutInMinutes);
                TimeSpan interval = dt - DateTime.Now;
                CacheInstance.Put(key, value, interval);
            }
            catch (Exception ex)
            {
                ApplicationException appException = new ApplicationException(String.Format("{0} cache update failed for cache key: {1}. Internal Error: {2}", cacheType, key, ex.Message), ex);
                ExceptionManager.ExceptionHandler exceptionHander = new ExceptionManager.ExceptionHandler(appException);
                throw appException;
            }
        }

        /// <summary>
        /// Store the Object in Cache for a sliding duration and don't allow the CLR to do a Garbage Collection to collect this Object in case of a memory pressure.
        /// </summary>
        /// <param name="key">The cache key used to store the data</param>
        /// <param name="value">The Object to be stored</param>
        /// <param name="timeOutInMinutes">The sliding expiration time out the cache needs to be kept alive</param>
        public void Set<T>(String key, T value, int timeOutInMinutes)
        {
            try
            {
                DateTime dt = DateTime.Now.AddMinutes(timeOutInMinutes);
                TimeSpan interval = dt - DateTime.Now;
                CacheInstance.Put(key, value, interval);
            }
            catch (Exception ex)
            {
                ApplicationException appException = new ApplicationException(String.Format("{0} cache update failed for cache key: {1}. Internal Error: {2}", cacheType, key, ex.Message), ex);
                ExceptionManager.ExceptionHandler exceptionHander = new ExceptionManager.ExceptionHandler(appException);
                throw appException;
            }
        }

        /// <summary>
        /// Remove the Object of type from cache
        /// </summary>
        /// <typeparam name="T">The type of the Object to remove so that we don't remove the wrong Object</typeparam>
        /// <param name="key">The key for the cached Object</param>
        /// <returns>Null</returns>
        public T Remove<T>(String key)
        {
            Boolean result = true;
            Object cachedObject = default(T);
            try
            {
                result = CacheInstance.Remove(key);
            }
            catch (Exception ex)
            {
                ApplicationException appException = new ApplicationException(String.Format("{0} cache remove failed for cache key: {1}. Internal Error: {2}", cacheType, key, ex.Message), ex);
                ExceptionManager.ExceptionHandler exceptionHander = new ExceptionManager.ExceptionHandler(appException);
            }

            return (T)cachedObject;
        }

        /// <summary>
        /// Clears cache.
        /// </summary>
        public void ClearCache()
        {
            foreach (String region in CacheInstance.GetSystemRegions())
            {
                CacheInstance.ClearRegion(region);
            }
        }

        /// <summary>
        /// Gets all cache keys
        /// </summary>
        /// <returns>Returns a colection of cache keys</returns>
        public Collection<String> GetAllCacheKeys()
        {
            Collection<String> cacheKeyNames = new Collection<String>();
            foreach (String region in CacheInstance.GetSystemRegions())
            {
                foreach (KeyValuePair<String, Object> cachedObject in CacheInstance.GetObjectsInRegion(region))
                {
                    cacheKeyNames.Add(cachedObject.Key);
                }
            }
            return cacheKeyNames;
        }

        #endregion

        #region IDistributedCache Members

        /// <summary>
        /// Gets data in bulk based on specified cache keys of specified chache region. Not supported in Windows Azure Shared Caching.
        /// </summary>
        /// <param name="keys">Indicates list of keys for the objects to retrieve, cannot be null.</param>
        /// <param name="region">Indicates name of the region, cannot be null.</param>
        /// <returns>Returns collection of key and value pair of cached data</returns>
        public IEnumerable<KeyValuePair<String, Object>> BulkGet(IEnumerable<String> keys, String region)
        {
            IEnumerable<KeyValuePair<String, Object>> cacheValue = null;
            try
            {
                cacheValue = CacheInstance.BulkGet(keys, region);
            }
            catch (Exception ex)
            {
                ApplicationException appException = new ApplicationException(String.Format("{0} cache bulk get failed for cache keys: {1}. Internal Error: {2}", cacheType, keys.ToString(), ex.Message), ex);
                ExceptionManager.ExceptionHandler exceptionHander = new ExceptionManager.ExceptionHandler(appException);
            }

            return cacheValue;
        }

        /// <summary>
        /// Gets an object from the specified region by using the specified key. Not supported in Windows Azure Shared Caching.
        /// </summary>
        /// <param name="key">Indicates keys for which data needs to be fetched</param>
        /// <param name="region">Indicates the name of the region where the object resides</param>
        /// <returns>Returns an object that was cached using the specified key. Null is returned if the key does not exist.</returns>
        public Object Get(String key, String region)
        {
            Object cacheValue = null;
            try
            {
                cacheValue = CacheInstance.Get(key, region);
            }
            catch (Exception ex)
            {
                ExceptionManager.ExceptionHandler exceptionHandler = new ExceptionManager.ExceptionHandler(ex);
            }
            return cacheValue;
        }

        /// <summary>
        /// Gets an enumerable list of all cached objects in specified region that have all same tags in common.Not supported in Windows Azure Shared Caching.
        /// </summary>
        /// <param name="tags">Indicates a list of tags for which search should happen.</param>
        /// <param name="region">Indicates name of the region to search. Tags are not supported outside regions. Therefore, a region name is required.</param>
        /// <returns>Retruns an enumerable list of all cached objects in the specified region that have all same tags in common.</returns>
        public IEnumerable<KeyValuePair<String, Object>> GetObjectsByAllTags(IEnumerable<String> tags, String region)
        {
            List<DataCacheTag> dataCacheTags = new List<DataCacheTag>();

            foreach (String tag in tags)
            {
                DataCacheTag dataCacheTag = new DataCacheTag(tag);
                dataCacheTags.Add(dataCacheTag);
            }

            return CacheInstance.GetObjectsByAllTags(dataCacheTags, region);
        }

        /// <summary>
        /// Gets an enumerable list of all cached objects in the specified region that have any of the same tags in common.
        /// Not supported in Windows Azure Shared Caching.
        /// </summary>
        /// <param name="tags">Indicates list of tags for which search should happen.</param>
        /// <param name="region">Indicates name of the region to search. Tags are not supported outside regions. Therefore, a region name is required.</param>
        /// <returns>
        /// Returns an enumerable list of all cached objects in the specified region that have any of the same tags in common.
        /// Null is returned if no objects in the specified region have any of the tags specified.</returns>
        public IEnumerable<KeyValuePair<String, Object>> GetObjectsByAnyTags(IEnumerable<String> tags, String region)
        {
            List<DataCacheTag> dataCacheTags = new List<DataCacheTag>();

            foreach (String tag in tags)
            {
                DataCacheTag dataCacheTag = new DataCacheTag(tag);
                dataCacheTags.Add(dataCacheTag);
            }

            return CacheInstance.GetObjectsByAnyTag(dataCacheTags, region);
        }

        /// <summary>
        /// Gets an enumerable list of all cached objects in the specified region that have the specified tag.
        /// Not supported in Windows Azure Shared Caching.
        /// </summary>
        /// <param name="tag">Indicates a tag for which search should happen.</param>
        /// <param name="region">Indicates name of the region to search. Tags are not supported outside regions. Therefore, a region name is required.</param>
        /// <returns>
        /// Returns an enumerable list of all cached objects in the specified region that have the specified tag.
        /// Null is returned if no objects in the specified region have the tag specified.</returns>
        public IEnumerable<KeyValuePair<String, Object>> GetObjectsByTag(String tag, String region)
        {

            DataCacheTag dataCacheTag = new DataCacheTag(tag);
            return CacheInstance.GetObjectsByTag(dataCacheTag, region);
        }

        /// <summary>
        /// Gets an enumerable list of all cached objects in the specified region. Not supported in Windows Azure Shared Caching.
        /// </summary>
        /// <param name="region">Indicates the name of the region for which to return a list of all resident objects.</param>
        /// <returns>Returns an enumerable list of all cached objects in the specified region.</returns>
        public IEnumerable<KeyValuePair<String, Object>> GetObjectsInRegion(String region)
        {
            return CacheInstance.GetObjectsInRegion(region);
        }

        /// <summary>
        /// Gets regions for the cache. Not supported in Windows Azure Shared Caching.
        /// </summary>
        /// <returns>Returns an enumerable list of default regions</returns>
        public IEnumerable<String> GetSystemRegions()
        {
            return CacheInstance.GetSystemRegions();
        }

        /// <summary>
        /// Adds or replaces an object in the specified region. Specifies the timeout value of the cached object. Not supported in Windows Azure Shared Caching.
        /// </summary>
        /// <param name="key">Indicates unique value that is used to identify the object in the region</param>
        /// <param name="value">Indicates the object to add or replace</param>
        /// <param name="expiryTime">Indicates expiration time of the object </param>
        /// <param name="region">Indicates the name of the region the object resides in.</param>
        public void Set(String key, Object value, DateTime expiryTime, String region)
        {
            try
            {
                TimeSpan interval = expiryTime - DateTime.Now;
                CacheInstance.Put(key, value, interval, region);
            }
            catch (Exception ex)
            {
                ApplicationException appException = new ApplicationException(String.Format("{0} cache update failed for cache key: {1}. Internal Error: {2}", cacheType, key, ex.Message), ex);
                ExceptionManager.ExceptionHandler exceptionHander = new ExceptionManager.ExceptionHandler(appException);
                throw appException;
            }
        }

        /// <summary>
        /// Adds or replaces an object in the specified list of tags. Specifies the timeout value of the cached object.
        /// </summary>
        /// <param name="key">Indicates unique value that is used to identify the object in the region.</param>
        /// <param name="value">Indicates an object to add or replace</param>
        /// <param name="expiryTime">Indicates expiration time for the object</param>
        /// <param name="tags">Indicates a list of tags to associate with the object.</param>
        public void Set(String key, Object value, DateTime expiryTime, List<String> tags)
        {
            try
            {
                List<DataCacheTag> dataCacheTags = new List<DataCacheTag>();

                foreach (String tag in tags)
                {
                    DataCacheTag dataCacheTag = new DataCacheTag(tag);
                    dataCacheTags.Add(dataCacheTag);
                }

                TimeSpan interval = expiryTime - DateTime.Now;
                CacheInstance.Put(key, value, interval, dataCacheTags);
            }
            catch (Exception ex)
            {
                ApplicationException appException = new ApplicationException(String.Format("{0} cache update failed for cache key: {1}. Internal Error: {2}", cacheType, key, ex.Message), ex);
                ExceptionManager.ExceptionHandler exceptionHander = new ExceptionManager.ExceptionHandler(appException);
                throw appException;
            }
        }

        /// <summary>
        /// Adds or replaces an object in the specified region having specified tags. Specifies the timeout value of the cached object. Not supported in Windows Azure Shared Caching.
        /// </summary>
        /// <param name="key">Indicates unique value that is used to identify the object in the region.</param>
        /// <param name="value">Indicates the object to add or replace.</param>
        /// <param name="expiryTime">Indicates the amount of time that the object should reside in the cache before expiration.</param>
        /// <param name="region">Indicates a list of tags to associate with the object.</param>
        /// <param name="tags">Indicates the name of the region the object resides in.</param>
        public void Set(String key, Object value, DateTime expiryTime, String region, List<String> tags)
        {
            try
            {
                List<DataCacheTag> dataCacheTags = new List<DataCacheTag>();

                foreach (String tag in tags)
                {
                    DataCacheTag dataCacheTag = new DataCacheTag(tag);
                    dataCacheTags.Add(dataCacheTag);
                }

                TimeSpan interval = expiryTime - DateTime.Now;
                CacheInstance.Put(key, value, interval, dataCacheTags, region);
            }
            catch (Exception ex)
            {
                ApplicationException appException = new ApplicationException(String.Format("{0} cache update failed for cache key: {1}. Internal Error: {2}", cacheType, key, ex.Message), ex);
                ExceptionManager.ExceptionHandler exceptionHander = new ExceptionManager.ExceptionHandler(appException);
                throw appException;
            }
        }

        /// <summary>
        /// Adds or replaces an object in the specified region. Specifies the timeout value of the cached object.Not supported in Windows Azure Shared Caching.
        /// </summary>
        /// <param name="key">Indicates unique value that is used to identify the object in the region.</param>
        /// <param name="value">Indicates object to add or replace.</param>
        /// <param name="timeOutInMinutes">Indicates the amount of time that the object should reside in the cache before expiration.</param>
        /// <param name="region">Indicates the name of the region the object resides in.</param>
        public void Set(String key, Object value, int timeOutInMinutes, String region)
        {
            try
            {
                DateTime dt = DateTime.Now.AddMinutes(timeOutInMinutes);
                TimeSpan interval = dt - DateTime.Now;
                CacheInstance.Put(key, value, interval, region);
            }
            catch (Exception ex)
            {
                ApplicationException appException = new ApplicationException(String.Format("{0} cache update failed for cache key: {1}. Internal Error: {2}", cacheType, key, ex.Message), ex);
                ExceptionManager.ExceptionHandler exceptionHander = new ExceptionManager.ExceptionHandler(appException);
                throw appException;
            }
        }

        /// <summary>
        /// Adds or replaces an object in the specified region having specified tags. Specifies the timeout value of the cached object.
        /// </summary>
        /// <param name="key">Indicates unique value that is used to identify the object in the region.</param>
        /// <param name="value">Indicates the object to add or replace.</param>
        /// <param name="timeOutInMinutes">Indicates the amount of time that the object should reside in the cache before expiration.</param>
        /// <param name="tags">Indicates the name of the region the object resides in.</param>
        public void Set(String key, Object value, int timeOutInMinutes, List<String> tags)
        {
            try
            {
                List<DataCacheTag> dataCacheTags = new List<DataCacheTag>();

                foreach (String tag in tags)
                {
                    DataCacheTag dataCacheTag = new DataCacheTag(tag);
                    dataCacheTags.Add(dataCacheTag);
                }

                DateTime dt = DateTime.Now.AddMinutes(timeOutInMinutes);
                TimeSpan interval = dt - DateTime.Now;
                CacheInstance.Put(key, value, interval, dataCacheTags);
            }
            catch (Exception ex)
            {
                ApplicationException appException = new ApplicationException(String.Format("{0} cache update failed for cache key: {1}. Internal Error: {2}", cacheType, key, ex.Message), ex);
                ExceptionManager.ExceptionHandler exceptionHander = new ExceptionManager.ExceptionHandler(appException);
                throw appException;
            }
        }

        /// <summary>
        /// Adds or replaces an object in the specified region having specified tags. 
        /// Specifies the timeout value of the cached object.Not supported in Windows Azure Shared Caching.
        /// </summary>
        /// <param name="key">Indicates unique value tha    t is used to identify the object in the region.</param>
        /// <param name="value">Indicates the object to add or replace.</param>
        /// <param name="timeOutInMinutes">Indicates the amount of time that the object should reside in the cache before expiration.</param>
        /// <param name="region">Indicates a list of tags to associate with the object.</param>
        /// <param name="tags">Indicates the name of the region the object resides in.</param>
        public void Set(String key, Object value, int timeOutInMinutes, String region, List<String> tags)
        {
            try
            {
                List<DataCacheTag> dataCacheTags = new List<DataCacheTag>();

                foreach (String tag in tags)
                {
                    DataCacheTag dataCacheTag = new DataCacheTag(tag);
                    dataCacheTags.Add(dataCacheTag);
                }

                DateTime dt = DateTime.Now.AddMinutes(timeOutInMinutes);
                TimeSpan interval = dt - DateTime.Now;
                CacheInstance.Put(key, value, interval, dataCacheTags, region);
            }
            catch (Exception ex)
            {
                ApplicationException appException = new ApplicationException(String.Format("{0} cache update failed for cache key: {1}. Internal Error: {2}", cacheType, key, ex.Message), ex);
                ExceptionManager.ExceptionHandler exceptionHander = new ExceptionManager.ExceptionHandler(appException);
                throw appException;
            }
        }

        /// <summary>
        /// Creates a region.
        /// </summary>
        /// <param name="region">Indicates the name of the region that is created.</param>
        /// <returns>Returns true if region is created succesfully and returns false if the region already exixts.</returns>
        public Boolean CreateRegion(String region)
        {
            return CacheInstance.CreateRegion(region);
        }

        /// <summary>
        /// Clears all regions.
        /// </summary>
        /// <returns>Returns true if all regions are cleared successfully , otherwise false.</returns>
        public Boolean ClearAll()
        {
            Boolean successFlag = false;
            try
            {
                IEnumerable<String> regionNames = CacheInstance.GetSystemRegions();
                foreach (String regionName in regionNames)
                {
                    CacheInstance.ClearRegion(regionName);
                }
                successFlag = true;
            }
            catch (Exception ex)
            {
                ApplicationException appException = new ApplicationException(String.Format("{0} cache clear all failed. Internal Error: {1}", cacheType, ex.Message), ex);
                ExceptionManager.ExceptionHandler exceptionHander = new ExceptionManager.ExceptionHandler(appException);
                throw appException;
            }

            return successFlag;
        }

        #endregion
    }

    
}
