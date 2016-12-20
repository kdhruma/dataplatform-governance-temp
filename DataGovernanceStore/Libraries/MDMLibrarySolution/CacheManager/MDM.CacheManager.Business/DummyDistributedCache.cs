using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace MDM.CacheManager.Business
{

    /// <summary>
    /// Class for DummyDistributedCache cache.
    /// </summary>
    public sealed class DummyDistributedCache : IDistributedCache
    {
        #region Fields

        #endregion

        #region Constructors

        /// <summary>
        /// Parameterless constructor
        /// </summary>
        public DummyDistributedCache()
        { 
        
        }

        #endregion

        #region Properties

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
                return null;
            }
        }

        /// <summary>
        /// Gets an object from cache
        /// </summary>
        /// <typeparam name="T">Indicates the object that can be serialized using datacontract serializer</typeparam>
        /// <param name="key">Indicates the name of the key to insert to cache</param>
        /// <returns>Returns the cached object converted to the requested type</returns>    
        public T Get<T>(String key)
        {
            return default(T);
        }

        /// <summary>
        /// Gets the object stored in cache
        /// </summary>
        /// <param name="key">Indicates the cache key used to store the data</param>
        /// <returns>Returns the stored object</returns>
        public Object Get(String key)
        {
            return null;
        }

        /// <summary>
        /// Removes the object from cache
        /// </summary>
        /// <param name="key">Indicates the key for the cached object</param>
        /// <returns>Returns removal status</returns>
        public Boolean Remove(String key)
        {
            return true;
        }

        /// <summary>
        /// Stores the object in Cache for a specific duration.
        /// It does not allow CLR to do a Garbage Collection to collect this object in case of a memory pressure.
        /// </summary>
        /// <param name="key">Indicates the cache key used to store the data</param>
        /// <param name="value">Indicates the object to be stored</param>
        /// <param name="expiryTime">Indicates the amount of time the object needs to be in Cache</param>
        public void Set(String key, Object value, DateTime expiryTime)
        {
           
        }

        /// <summary>
        /// Sets object in a Cache
        /// </summary>
        /// <typeparam name="T">Indicates the object that can be serialized using DataContract serialize</typeparam>
        /// <param name="key">Indicates the key to insert to the Cache</param>
        /// <param name="instance">Indicates the object Instance</param>
        /// <param name="expiryTime">Indicates the time duration to hold object into Cache </param>
        public void Set<T>(String key, T instance, DateTime expiryTime)
        {
            
        }

        /// <summary>
        /// Stores the object in Cache for a sliding duration.
        /// It does not allow CLR to do a Garbage Collection to collect this object in case of a memory pressure.
        /// </summary>
        /// <param name="key">Indicates the cache key used to store the data</param>
        /// <param name="value">Indicates the object to be stored</param>
        /// <param name="timeOutInMinutes">Indicates the sliding expiration time out the cache needs to be kept alive</param>
        public void Set(String key, Object value, Int32 timeOutInMinutes)
        {
        }

        /// <summary>
        /// Stores the object in Cache for a sliding duration and don't allow the CLR to do a Garbage Collection to collect this object in case of a memory pressure.
        /// </summary>
        /// <param name="key">Indicates the cache key used to store the data</param>
        /// <param name="value">Indicates the object to be stored</param>
        /// <param name="timeOutInMinutes">Indicates the sliding expiration time out the cache needs to be kept alive</param>
        public void Set<T>(String key, T value, int timeOutInMinutes)
        {
           
        }

        /// <summary>
        /// Removes the object of type from cache
        /// </summary>
        /// <typeparam name="T">Indicates the type of the object to remove so that we don't remove the wrong object</typeparam>
        /// <param name="key">Indicates the key for the cached object</param>
        /// <returns>Null</returns>
        public T Remove<T>(String key)
        {
            return default(T);
        }

        /// <summary>
        /// Clears Cache
        /// </summary>
        public void ClearCache()
        {

        }

        /// <summary>
        /// Gets all cache keys available in cache
        /// </summary>
        /// <returns>Returns collection of cache key names</returns>
        public Collection<String> GetAllCacheKeys()
        {
            return new Collection<String>();
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
            return null;
        }

        /// <summary>
        /// Gets an object from the specified region by using the specified key. Not supported in Windows Azure Shared Caching.
        /// </summary>
        /// <param name="key">Indicates keys for which data needs to be fetched</param>
        /// <param name="region">Indiactes the name of the region where the object resides</param>
        /// <returns>Returns an object that was cached using the specified key. Null is returned if the key does not exist.</returns>
        public Object Get(String key, String region)
        {
            return null;
        }

        /// <summary>
        /// Gets an enumerable list of all cached objects in specified region that have all same tags in common.Not supported in Windows Azure Shared Caching.
        /// </summary>
        /// <param name="tags">Indiactes a list of tags for which search should happen.</param>
        /// <param name="region">Indicates name of the region to search. Tags are not supported outside regions. Therefore, a region name is required.</param>
        /// <returns>Returns an enumerable list of all cached objects in the specified region that have all same tags in common.</returns>
        public IEnumerable<KeyValuePair<String, Object>> GetObjectsByAllTags(IEnumerable<String> tags, String region)
        {
            return null;
        }

        /// <summary>
        /// Gets an enumerable list of all cached objects in the specified region that have any of the same tags in common.
        /// Not supported in Windows Azure Shared Caching.
        /// </summary>
        /// <param name="tags">Indicates list of tags for which search should happen.</param>
        /// <param name="region">Indicates name of the region to search. Tags are not supported outside regions. Therefore, a region name is required.</param>
        /// <returns>
        /// Returns an enumerable list of all cached objects in the specified region that have any of the same tags in common.
        /// Null is returned if no objects in the specified region have any of the tags specified.
        /// </returns>
        public IEnumerable<KeyValuePair<String, Object>> GetObjectsByAnyTags(IEnumerable<String> tags, String region)
        {
            return null;
        }

        /// <summary>
        /// Gets an enumerable list of all cached objects in the specified region that have the specified tag.Not supported in Windows Azure Shared Caching.
        /// </summary>
        /// <param name="tag">Indicates a tag for which search should happen.</param>
        /// <param name="region">Indicates name of the region to search. Tags are not supported outside regions. Therefore, a region name is required.</param>
        /// <returns>
        /// Returns an enumerable list of all cached objects in the specified region that have the specified tag.
        /// Null is returned if no objects in the specified region have the tag specified.
        /// </returns>
        public IEnumerable<KeyValuePair<String, Object>> GetObjectsByTag(String tag, String region)
        {
            return null;
        }

        /// <summary>
        /// Gets an enumerable list of all cached objects in the specified region.Not supported in Windows Azure Shared Caching.
        /// </summary>
        /// <param name="region">Indicates name of the region for which a list of all resident objects should be returned.</param>
        /// <returns>Returns an enumerable list of all cached objects in the specified region.</returns>
        public IEnumerable<KeyValuePair<String, Object>> GetObjectsInRegion(String region)
        {
            return null;
        }

        /// <summary>
        /// Gets default regions for the cache.
        /// </summary>
        /// <returns>Returns an enumerable list of default regions for the cache as an IEnumerable object.</returns>
        public IEnumerable<String> GetSystemRegions()
        {
            return null;
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
            
        }

        /// <summary>
        /// Adds or replaces an object in the specified region having specified tags. 
        /// Specifies the timeout value of the cached object. Not supported in Windows Azure Shared Caching.
        /// </summary>
        /// <param name="key">Indicates unique value that is used to identify the object in the region.</param>
        /// <param name="value">Indicates the object to add or replace.</param>
        /// <param name="expiryTime">Indicates the amount of time that the object should reside in the cache before expiration.</param>
        /// <param name="region">Indicates a list of tags to associate with the object.</param>
        /// <param name="tags">Indicates the name of the region the object resides in.</param>
        public void Set(String key, Object value, DateTime expiryTime, String region, List<String> tags)
        {
           
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
          
        }

        /// <summary>
        /// Adds or replaces an object in the specified region having specified tags. Specifies the timeout value of the cached object.Not supported in Windows Azure Shared Caching.
        /// </summary>
        /// <param name="key">Indicates unique value that is used to identify the object in the region.</param>
        /// <param name="value">Indicates the object to add or replace.</param>
        /// <param name="timeOutInMinutes">Indicates the amount of time that the object should reside in the cache before expiration.</param>
        /// <param name="region">Indicates a list of tags to associate with the object.</param>
        /// <param name="tags">Indicates the name of the region the object resides in.</param>
        public void Set(String key, Object value, int timeOutInMinutes, String region, List<String> tags)
        {
         
        }

        /// <summary>
        /// Creates a region.
        /// </summary>
        /// <param name="region">Indicates the name of the region that is created.</param>
        /// <returns>Returns true if region is created succesfully and false if the region already exixts.</returns>
        public Boolean CreateRegion(String region)
        {
            return true;
        }

        /// <summary>
        /// Clears all regions.
        /// </summary>
        /// <returns>Returns true if all regions are cleared successfully, otherwise false.</returns>
        public Boolean ClearAll()
        {
            return true;
        }

        #endregion
    }
}
