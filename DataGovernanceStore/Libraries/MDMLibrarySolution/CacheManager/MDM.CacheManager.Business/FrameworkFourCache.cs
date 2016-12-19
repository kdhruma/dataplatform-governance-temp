using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Caching;

namespace MDM.CacheManager.Business
{
    /// <summary>
    /// Represents class for framework four type of cache
    /// </summary>
    /// //NOTE: FrameworkFourCache also supports fall back for distributed caching. This is just to test MDM application without distributed caching enabled. 
    /// //This option is not at all recommanded in production
    public class FrameworkFourCache : IDistributedCache
    {  
        #region Fields

        ObjectCache cacheValue = MemoryCache.Default;

        const String cacheType = "Local";

        #endregion

        #region Constructors

        /// <summary>
        /// Parameterless constructor
        /// </summary>
        public FrameworkFourCache()
        {
        }

        #endregion

        #region Properties

        private ObjectCache CacheInstance
        {
            get
            {
                return cacheValue;
            }
        }

        #endregion

        #region Methods

        #endregion

        #region ICache Members

        /// <summary>
        /// Gets the object stored in cache
        /// </summary>
        /// <param name="key">Indicates the cache key used to store the data</param>
        /// <returns>Returns the stored object</returns>
        /// <remarks>If the object is not present, it returns null</remarks>
        public Object this[String key]
        {
            get
            {
                Object keyValue = null;
                keyValue = CacheInstance[key];
                return keyValue;
            }
        }

        /// <summary>
        /// Gets the object stored in cache
        /// </summary>
        /// <param name="key">Indicates the cache key used to store the data</param>
        /// <returns>Returns the stored object</returns>
        public object Get(string key)
        {
            Object keyValue = null;
            keyValue = CacheInstance[key];
            return keyValue;
        }

        /// <summary>
        /// Gets an object or string from cache
        /// </summary>
        /// <typeparam name="T">Indicates the object or string type that can be serialized using datacontract serializer</typeparam>
        /// <param name="key">Indicates the name of the key to insert to cache</param>
        /// <returns>Returns the cached object converted to the requested type</returns>
        public T Get<T>(String key)
        {
            Object keyValue = null;

            keyValue = CacheInstance[key];

            return (T)keyValue;
        }

        /// <summary>
        /// Store the object in cache with a file dependency so that if a file changes the cache object needs to be removed
        /// </summary>
        /// <param name="key">The cache key used to store the data</param>
        /// <param name="value">The object to be stored</param>
        /// <param name="dependantFileName">The file name along with the path the object depends on </param>
        public void Set(String key, Object value, String dependantFileName)
        {
            try
            {
                CacheItemPolicy policy = new CacheItemPolicy();

                List<string> filePaths = new List<string>();
                filePaths.Add(dependantFileName);

                policy.ChangeMonitors.Add(new HostFileChangeMonitor(filePaths));

                CacheInstance.Set(key, value, policy); //insert the file dependency
            }
            catch(Exception ex)
            {
                ApplicationException appException = new ApplicationException(String.Format("{0} cache update failed for cache key: {1}. Internal Error: {2}", cacheType, key, ex.Message), ex);
                ExceptionManager.ExceptionHandler exceptionHander = new ExceptionManager.ExceptionHandler(appException);
                throw appException;
            }
        }

        /// <summary>
        /// Store the object in cache with a file dependency so that if a file changes the cache object needs to be removed
        /// </summary>
        /// <param name="key">The cache key used to store the data</param>
        /// <param name="value">The object to be stored</param>
        /// <param name="dependantFileName">The file name along with the path the object depends on </param>
        public void Set<T>(String key, T value, String dependantFileName)
        {
            try
            {
                CacheItemPolicy policy = new CacheItemPolicy();

                List<string> filePaths = new List<string>();
                filePaths.Add(dependantFileName);

                policy.ChangeMonitors.Add(new HostFileChangeMonitor(filePaths));

                CacheInstance.Set(key, value, policy); //insert the file dependency
            }
            catch (Exception ex)
            {
                ApplicationException appException = new ApplicationException(String.Format("{0} cache update failed for cache key: {1}. Internal Error: {2}", cacheType, key, ex.Message), ex);
                ExceptionManager.ExceptionHandler exceptionHander = new ExceptionManager.ExceptionHandler(appException);
                throw appException;
            }
        }

        /// <summary>
        /// Store the object in Cache for a specific duration.
        /// It does not allow the CLR to do a Garbage Collection to collect this object in case of a memory pressure.
        /// </summary>
        /// <param name="key">The cache key used to store the data</param>
        /// <param name="value">The object to be stored</param>
        /// <param name="expiryTime">The amount of time the object needs to be in Cache</param>
        public void Set(String key, Object value, DateTime expiryTime)
        {
            try
            {
                CacheItemPolicy policy = new CacheItemPolicy();

                policy.AbsoluteExpiration = expiryTime;

                CacheInstance.Set(key, value, policy);
            }
            catch (Exception ex)
            {
                ApplicationException appException = new ApplicationException(String.Format("{0} cache update failed for cache key: {1}. Internal Error: {2}", cacheType, key, ex.Message), ex);
                ExceptionManager.ExceptionHandler exceptionHander = new ExceptionManager.ExceptionHandler(appException);
                throw appException;
            }
        }

        /// <summary>
        /// Stores the object in Cache for a specific duration.
        /// It does not allow the CLR to do a Garbage Collection to collect this object in case of a memory pressure.
        /// </summary>
        /// <typeparam name="T">Indicates the Object that can be serialized using datacontract serializer</typeparam>
        /// <param name="key">Indicates the name of the key to insert to cache</param>
        /// <param name="value">Indicates value of an object to cache</param>
        /// <param name="expiryTime">Indicates the amount of time an object should reside in cache before expiration</param>
        public void Set<T>(string key, T value, DateTime expiryTime)
        {
            try
            {
                CacheItemPolicy policy = new CacheItemPolicy();
                policy.AbsoluteExpiration = expiryTime;
                CacheInstance.Set(key, value, policy);
            }
            catch (Exception ex)
            {
                ApplicationException appException = new ApplicationException(String.Format("{0} cache update failed for cache key: {1}. Internal Error: {2}", cacheType, key, ex.Message), ex);
                ExceptionManager.ExceptionHandler exceptionHander = new ExceptionManager.ExceptionHandler(appException);
                throw appException;
            }
        }

        /// <summary>
        /// Store the object in Cache for a sliding duration.
        /// It does not allow the CLR to do a Garbage Collection to collect this object in case of a memory pressure.
        /// </summary>
        /// <param name="key">The cache key used to store the data</param>
        /// <param name="value">The object to be stored</param>
        /// <param name="timeOutInMinutes">The sliding expiration time out the cache needs to be kept alive</param>
        public void Set(String key, Object value, Int32 timeOutInMinutes)
        {
            try
            {
                CacheItemPolicy policy = new CacheItemPolicy();

                policy.SlidingExpiration = TimeSpan.FromMinutes(timeOutInMinutes);

                CacheInstance.Set(key, value, policy);
            }
            catch (Exception ex)
            {
                ApplicationException appException = new ApplicationException(String.Format("{0} cache update failed for cache key: {1}. Internal Error: {2}", cacheType, key, ex.Message), ex);
                ExceptionManager.ExceptionHandler exceptionHander = new ExceptionManager.ExceptionHandler(appException);
                throw appException;
            }
        }

        /// <summary>
        /// Store the object in Cache for a sliding duration.
        /// It does not allow the CLR to do a Garbage Collection to collect this object in case of a memory pressure.
        /// </summary>
        /// <param name="key">The cache key used to store the data</param>
        /// <param name="value">The object to be stored</param>
        /// <param name="timeOutInMinutes">The sliding expiration time out the cache needs to be kept alive</param>
        public void Set<T>(string key, T value, int timeOutInMinutes)
        {
            try
            {
                CacheItemPolicy policy = new CacheItemPolicy();

                policy.SlidingExpiration = TimeSpan.FromMinutes(timeOutInMinutes);

                CacheInstance.Set(key, value, policy);
            }
            catch (Exception ex)
            {
                ApplicationException appException = new ApplicationException(String.Format("{0} cache update failed for cache key: {1}. Internal Error: {2}", cacheType, key, ex.Message), ex);
                ExceptionManager.ExceptionHandler exceptionHander = new ExceptionManager.ExceptionHandler(appException);
                throw appException;
            }
        }

        /// <summary>
        /// Remove an object or a string from cache
        /// </summary>
        /// <typeparam name="T">Indicates the Object that can be serialized using datacontract serializer</typeparam>
        /// <param name="key">Indicates name of the key to insert to cache.</param>
        /// <returns>Returns removed cached object.</returns>
        public T Remove<T>(string key)
        {
            Object cachedObject = null;
            if (CacheInstance[key] != null)
            {
                cachedObject = CacheInstance.Remove(key);
            }
            return (T)cachedObject;
        }

        /// <summary>
        /// Remove an object or a string from cache
        /// </summary>
        /// <param name="key">Indicates name of the key to insert to cache.</param>
        /// <returns>Reuturns true if object is removed from the cache successfully, otherwise false.</returns>
        public Boolean Remove(string key)
        {
            Boolean result = true;
            Object cachedObject = null;
            if (CacheInstance[key] != null)
            {
                cachedObject = CacheInstance.Remove(key);
                result = true;
            }
            return result;
        }

        /// <summary>
        /// Clears cache
        /// </summary>
        public void ClearCache() 
        {
            foreach (Object cacheObject in CacheInstance)
            {
                String cacheKeyName = ((System.Collections.Generic.KeyValuePair<string, object>)(cacheObject)).Key;
                if (!String.IsNullOrEmpty(cacheKeyName))
                {
                    CacheInstance.Remove(cacheKeyName);
                }
            }
        }

        /// <summary>
        /// Gets all cache keys
        /// </summary>
        /// <returns>Returns a collection of cache keys</returns>
        public Collection<String> GetAllCacheKeys() 
        {
            Collection<String> cacheKeyNames = new Collection<string>();
            foreach (Object cacheObject in CacheInstance)
            {
                String cacheKeyName = ((System.Collections.Generic.KeyValuePair<string, object>)(cacheObject)).Key;
                if (!String.IsNullOrEmpty(cacheKeyName))
                {
                    cacheKeyNames.Add(cacheKeyName);
                }
            }
            return cacheKeyNames;
        }

        #endregion

        #region IDistributedCache Members

        /// <summary>
        /// Gets data in bulk based on specified cache keys of specified cache region. Not supported in Windows Azure Shared Caching.
        /// </summary>
        /// <param name="keys">Indicates list of keys for the objects to retrieve, cannot be null.</param>
        /// <param name="region">Indicates name of the region, cannot be null.</param>
        /// <returns>Returns collection of key and value pair of cached data</returns>
        public IEnumerable<KeyValuePair<string, object>> BulkGet(IEnumerable<string> keys, string region)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets an object from the specified region by using the specified key. Not supported in Windows Azure Shared Caching.
        /// </summary>
        /// <param name="key">Indicates keys for which data needs to be fetched</param>
        /// <param name="region">Indicates the name of the region where the object resides</param>
        /// <returns>Returns an object that was cached using the specified key. Null is returned if the key does not exist.</returns>
        public object Get(string key, string region)
        {
            return this.Get(key);
        }

        /// <summary>
        /// Gets an enumerable list of all cached objects in specified region that have all same tags in common.Not supported in Windows Azure Shared Caching.
        /// </summary>
        /// <param name="tags">Indiactes a list of tags for which search should happen.</param>
        /// <param name="region">Indicates name of the region to search. Tags are not supported outside regions. Therefore, a region name is required.</param>
        /// <returns>Returns an enumerable list of all cached objects in the specified region that have all same tags in common.</returns>
        public IEnumerable<KeyValuePair<string, object>> GetObjectsByAllTags(IEnumerable<string> tags, string region)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets an enumerable list of all cached objects in the specified region that have any of the same tags in common.
        /// Not supported in Windows Azure Shared Caching.
        /// </summary>
        /// <param name="tags">Indicates list of tags for which search should happen.</param>
        /// <param name="region">
        /// Indicates name of the region to search. 
        /// Tags are not supported outside regions.Therefore, a region name is required.
        /// </param>
        /// <returns>Returns an enumerable list of all cached objects in the specified region that have any of the same tags in common. Null is returned if no objects in the specified region have any of the tags specified.</returns>
        public IEnumerable<KeyValuePair<string, object>> GetObjectsByAnyTags(IEnumerable<string> tags, string region)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets an enumerable list of all cached objects in the specified region that have the specified tag.Not supported in Windows Azure Shared Caching.
        /// </summary>
        /// <param name="tag">Indicates a tag for which search should happen.</param>
        /// <param name="region">Indicates name of the region to search. Tags are not supported outside regions. Therefore, a region name is required.</param>
        /// <returns>
        /// Returns an enumerable list of all cached objects in the specified region that have the specified tag. 
        /// If no objects in the specified region have the tag specified, it returns null.</returns>
        public IEnumerable<KeyValuePair<string, object>> GetObjectsByTag(string tag, string region)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets an enumerable list of all cached objects in the specified region.Not supported in Windows Azure Shared Caching.
        /// </summary>
        /// <param name="region">Indicates name of the region for which a list of all resident objects should be returned.</param>
        /// <returns>Returns an enumerable list of all cached objects in the specified region.</returns>
        public IEnumerable<KeyValuePair<string, object>> GetObjectsInRegion(string region)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets default regions for the cache.
        /// </summary>
        /// <returns>Returns an enumerable list of default regions for the cache as an IEnumerable object.</returns>
        public IEnumerable<string> GetSystemRegions()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Adds or replaces an object in the specified region. Specifies the timeout value of the cached object. Not supported in Windows Azure Shared Caching.
        /// </summary>
        /// <param name="key">Indicates unique value that is used to identify the object in the region</param>
        /// <param name="value">Indicates the object to add or replace</param>
        /// <param name="expiryTime">Indicates expiration time of the object </param>
        /// <param name="region">Indicates the name of the region the object resides in.</param>
        public void Set(string key, object value, DateTime expiryTime, string region)
        {
            this.Set(key, value, expiryTime);
        }

        /// <summary>
        /// Adds or replaces an object in the specified list of tags. Specifies the timeout value of the cached object.
        /// </summary>
        /// <param name="key">Indicates unique value that is used to identify the object in the region.</param>
        /// <param name="value">Indicates an object to add or replace</param>
        /// <param name="expiryTime">Indicates expiration time for the object</param>
        /// <param name="tags">Indicates a list of tags to associate with the object.</param>
        public void Set(string key, object value, DateTime expiryTime, List<string> tags)
        {
            this.Set(key, value, expiryTime);
        }

        /// <summary>
        /// Adds or replaces an object in the specified region having specified tags. 
        /// It Specifies the timeout value of the cached object. Not supported in Windows Azure Shared Caching.
        /// </summary>
        /// <param name="key">Indicates unique value that is used to identify the object in the region.</param>
        /// <param name="value">Indicates the object to add or replace.</param>
        /// <param name="expiryTime">Indicates the amount of time that the object should reside in the cache before expiration.</param>
        /// <param name="region">Indicates a list of tags to associate with the object.</param>
        /// <param name="tags">Indicates the name of the region the object resides in.</param>
        public void Set(string key, object value, DateTime expiryTime, string region, List<string> tags)
        {
            this.Set(key, value, expiryTime);
        }

        /// <summary>
        /// Adds or replaces an object in the specified region. Specifies the timeout value of the cached object.Not supported in Windows Azure Shared Caching.
        /// </summary>
        /// <param name="key">Indicates unique value that is used to identify the object in the region.</param>
        /// <param name="value">Indicates object to add or replace.</param>
        /// <param name="timeOutInMinutes">Indicates the amount of time that the object should reside in the cache before expiration.</param>
        /// <param name="region">Indicates the name of the region the object resides in.</param>
        public void Set(string key, object value, int timeOutInMinutes, string region)
        {
            this.Set(key, value, timeOutInMinutes);
        }

        /// <summary>
        /// Adds or replaces an object in the specified region having specified tags. Specifies the timeout value of the cached object.
        /// </summary>
        /// <param name="key">Indicates unique value that is used to identify the object in the region.</param>
        /// <param name="value">Indicates the object to add or replace.</param>
        /// <param name="timeOutInMinutes">Indicates the amount of time that the object should reside in the cache before expiration.</param>
        /// <param name="tags">Indicates the name of the region the object resides in.</param>
        public void Set(string key, object value, int timeOutInMinutes, List<string> tags)
        {
            this.Set(key, value, timeOutInMinutes);
        }

        /// <summary>
        /// Adds or replaces an object in the specified region having specified tags. 
        /// It Specifies the timeout value of the cached object.Not supported in Windows Azure Shared Caching.
        /// </summary>
        /// <param name="key">Indicates unique value that is used to identify the object in the region.</param>
        /// <param name="value">Indicates the object to add or replace.</param>
        /// <param name="timeOutInMinutes">Indicates the amount of time that the object should reside in the cache before expiration.</param>
        /// <param name="region">Indicates a list of tags to associate with the object.</param>
        /// <param name="tags">Indicates the name of the region the object resides in.</param>
        public void Set(string key, object value, int timeOutInMinutes, string region, List<string> tags)
        {
            this.Set(key, value, timeOutInMinutes);
        }

        /// <summary>
        /// Creates a region.
        /// </summary>
        /// <param name="region">Indicates the name of the region that is created.</param>
        /// <returns>Returns true if region is created succesfully and returns false if the region already exixts.</returns>
        public bool CreateRegion(string region)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Clears all regions.
        /// </summary>
        /// <returns>Returns true if all regions are cleared successfully, otherwise false.</returns>
        public Boolean ClearAll()
        {
            this.ClearCache();
            return true;
        }

        #endregion
    }
}
