using System;
using System.Collections.Generic;

namespace MDM.CacheManager.Business
{
    /// <summary>
    /// Exposes distributed cache related operations
    /// </summary>
    public interface IDistributedCache : ICache
    {
        // TODO: Require lock and unlock methods..
        /// <summary>
        /// Gets data in bulk based on specified cache keys of specified chache region. Not supported in Windows Azure Shared Caching.
        /// </summary>
        /// <param name="keys">Indicates list of keys for the objects to retrieve, cannot be null.</param>
        /// <param name="region">Indicates name of the region, cannot be null.</param>
        /// <returns>Returns collection of key and value pair of cached data</returns>
        IEnumerable<KeyValuePair<String, Object>> BulkGet(IEnumerable<String> keys, String region);

        /// <summary>
        /// Gets an object from the specified region by using the specified key. Not supported in Windows Azure Shared Caching.
        /// </summary>
        /// <param name="key">Indicates keys for which data needs to be fetched</param>
        /// <param name="region">Indicates the name of the region where the object resides</param>
        /// <returns>Returns an object that was cached using the specified key. Null is returned if the key does not exist.</returns>
        Object Get(String key, String region);

        /// <summary>
        /// Gets an enumerable list of all cached objects in specified region that have all same tags in common.Not supported in Windows Azure Shared Caching.
        /// </summary>
        /// <param name="tags">Indicates a list of tags for which search should happen.</param>
        /// <param name="region">Indicates name of the region to search. Tags are not supported outside regions. Therefore, a region name is required.</param>
        /// <returns>Returns an enumerable list of all cached objects in the specified region that have all same tags in common.</returns>
        IEnumerable<KeyValuePair<String, Object>> GetObjectsByAllTags(IEnumerable<String> tags, String region);

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
        IEnumerable<KeyValuePair<String, Object>> GetObjectsByAnyTags(IEnumerable<String> tags, String region);

        /// <summary>
        /// Gets an enumerable list of all cached objects in the specified region that have the specified tag.Not supported in Windows Azure Shared Caching.
        /// </summary>
        /// <param name="tag">Indicates a tag for which search should happen.</param>
        /// <param name="region">Indicates name of the region to search. Tags are not supported outside regions. Therefore, a region name is required.</param>
        /// <returns>
        /// Returns an enumerable list of all cached objects in the specified region that have the specified tag.
        /// If no objects in the specified region have the tag specified, it returns null.
        /// </returns>
        IEnumerable<KeyValuePair<String, Object>> GetObjectsByTag(String tag, String region);

        /// <summary>
        /// Gets an enumerable list of all cached objects in the specified region.Not supported in Windows Azure Shared Caching.
        /// </summary>
        /// <param name="region">Indicates name of the region for which a list of all resident objects should be returned.</param>
        /// <returns>Returns an enumerable list of all cached objects in the specified region.</returns>
        IEnumerable<KeyValuePair<String, Object>> GetObjectsInRegion(String region);

        /// <summary>
        /// Gets default regions for the cache.
        /// </summary>
        /// <returns>Returns an enumerable list of default regions for the cache as an IEnumerable object.</returns>
        IEnumerable<String> GetSystemRegions();

        /// <summary>
        /// Adds or replaces an object in the specified region. Specifies the timeout value of the cached object. Not supported in Windows Azure Shared Caching.
        /// </summary>
        /// <param name="key">Indicates unique value that is used to identify the object in the region</param>
        /// <param name="value">Indicates the object to add or replace</param>
        /// <param name="expiryTime">Indicates expiration time of the object </param>
        /// <param name="region">Indicates the name of the region the object resides in.</param>
        void Set(String key, Object value, DateTime expiryTime, String region);

        /// <summary>
        /// Adds or replaces an object in the specified list of tags. Specifies the timeout value of the cached object.
        /// </summary>
        /// <param name="key">Indicates unique value that is used to identify the object in the region.</param>
        /// <param name="value">Indicates an object to add or replace</param>
        /// <param name="expiryTime">Indicates expiration time for the object</param>
        /// <param name="tags">Indicates a list of tags to associate with the object.</param>
        void Set(String key, Object value, DateTime expiryTime, List<String> tags);

        /// <summary>
        /// Adds or replaces an object in the specified region having specified tags. 
        /// Specifies the timeout value of the cached object. Not supported in Windows Azure Shared Caching.
        /// </summary>
        /// <param name="key">Indicates unique value that is used to identify the object in the region.</param>
        /// <param name="value">Indicates the object to add or replace.</param>
        /// <param name="expiryTime">Indicates the amount of time that the object should reside in the cache before expiration.</param>
        /// <param name="region">Indicates a list of tags to associate with the object.</param>
        /// <param name="tags">Indicates the name of the region the object resides in.</param>
        void Set(String key, Object value, DateTime expiryTime, String region, List<String> tags);

        /// <summary>
        /// Adds or replaces an object in the specified region. Specifies the timeout value of the cached object.Not supported in Windows Azure Shared Caching.
        /// </summary>
        /// <param name="key">Indicates unique value that is used to identify the object in the region.</param>
        /// <param name="value">Indicates object to add or replace.</param>
        /// <param name="timeOutInMinutes">Indicates the amount of time that the object should reside in the cache before expiration.</param>
        /// <param name="region">Indicates the name of the region the object resides in.</param>
        void Set(String key, Object value, Int32 timeOutInMinutes, String region);

        /// <summary>
        /// Adds or replaces an object in the specified region having specified tags. Specifies the timeout value of the cached object.
        /// </summary>
        /// <param name="key">Indicates unique value that is used to identify the object in the region.</param>
        /// <param name="value">Indicates the object to add or replace.</param>
        /// <param name="timeOutInMinutes">Indicates the amount of time that the object should reside in the cache before expiration.</param>
        /// <param name="tags">Indicates the name of the region the object resides in.</param>
        void Set(String key, Object value, Int32 timeOutInMinutes, List<String> tags);

        /// <summary>
        /// Adds or replaces an object in the specified region having specified tags. 
        /// Specifies the timeout value of the cached object.Not supported in Windows Azure Shared Caching.
        /// </summary>
        /// <param name="key">Indicates unique value that is used to identify the object in the region.</param>
        /// <param name="value">Indicates the object to add or replace.</param>
        /// <param name="timeOutInMinutes">Indicates the amount of time that the object should reside in the cache before expiration.</param>
        /// <param name="region">Indicates a list of tags to associate with the object.</param>
        /// <param name="tags">Indicates the name of the region the object resides in.</param>
        void Set(String key, Object value, Int32 timeOutInMinutes, String region, List<String> tags);

        /// <summary>
        /// Creates a region.
        /// </summary>
        /// <param name="region">Indicates the name of the region that is created.</param>
        /// <returns>Returns true if region is created succesfully and returns false if the region already exists.</returns>
        Boolean CreateRegion(String region);

        /// <summary>
        /// Clears all regions.
        /// </summary>
        /// <returns>Returns true if all regions are cleared successfully, otherwise false.</returns>
        Boolean ClearAll();
    }
}
