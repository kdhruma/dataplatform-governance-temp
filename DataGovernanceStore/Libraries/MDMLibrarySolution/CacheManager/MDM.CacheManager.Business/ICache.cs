using System;
using System.Collections.ObjectModel;

namespace MDM.CacheManager.Business
{
    /// <summary>
    /// Exposes distributed cache related operations
    /// </summary>
    public interface ICache
    {
        /// <summary>
        /// Gets the object stored in cache
        /// </summary>
        /// <param name="key">Indicates the cache key used to store the data</param>
        /// <returns>Returns the stored object</returns>
        Object this[String key] { get; }

        /// <summary>
        /// Gets the object stored in cache
        /// </summary>
        /// <param name="key">Indicates the cache key used to store the data</param>
        /// <returns>Returns the stored object</returns>
        Object Get(String key);

        /// <summary>
        /// Gets an object from cache
        /// </summary>
        /// <typeparam name="T">Indicates the object that can be serialized using datacontract serializer</typeparam>
        /// <param name="key">Indicates the name of the key to insert to cache</param>
        /// <returns>Returns the cached object converted to the requested type</returns>    
        T Get<T>(String key);

        /// <summary>
        /// Stores the object in cache for a specific duration.
        /// It doesn't allow the CLR to do a Garbage Collection to collect this object in case of a memory pressure.
        /// </summary>
        /// <param name="key">Indicates the cache key used to store the data</param>
        /// <param name="value">Indicates the object to be stored</param>
        /// <param name="expiryTime">Indicates the amount of time the object needs to be in cache</param>
        void Set(String key, Object value, DateTime expiryTime);

        /// <summary>
        /// Sest object in a cache.........
        /// </summary>
        /// <typeparam name="T">Indicates the object that can be serialized using DataContract serialize</typeparam>
        /// <param name="key">Indicates the key to insert to the cache</param>
        /// <param name="value">Indicates the object Instance</param>
        /// <param name="expiryTime">Indicates the time duration to hold object into cache </param>
        void Set<T>(String key, T value, DateTime expiryTime);

        /// <summary>
        /// Stores the object in cache for a sliding duration.
        /// It doesn't allow the CLR to do a Garbage Collection to collect this object in case of a memory pressure.
        /// </summary>
        /// <param name="key">Indicates the cache key used to store the data</param>
        /// <param name="value">Indicates the object to be stored</param>
        /// <param name="timeOutInMinutes">Indicates the sliding expiration time out the cache needs to be kept alive</param>
        void Set(String key, Object value, Int32 timeOutInMinutes);

        /// <summary>
        /// Stores the object in cache for a sliding duration.
        /// It doesn't allow the CLR to do a Garbage Collection to collect this object in case of a memory pressure.
        /// </summary>
        /// <typeparam name="T">Indicates the object that can be serialized using DataContract serialize</typeparam>
        /// <param name="key">Indicates the cache key used to store the data</param>
        /// <param name="value">Indicates the object to be stored</param>
        /// <param name="timeOutInMinutes">Indicates the sliding expiration time out the cache needs to be kept alive</param>
        void Set<T>(String key, T value, Int32 timeOutInMinutes);

        /// <summary>
        /// Removes the object of type from cache
        /// </summary>
        /// <typeparam name="T">Indicates the type of the object to remove so that we don't remove the wrong object</typeparam>
        /// <param name="key">Indicates the key for the cached object</param>
        /// <returns>Returns null</returns>
        T Remove<T>(String key);

        /// <summary>
        /// Removes the object from cache
        /// </summary>
        /// <param name="key">Indicates the key for the cached object</param>
        /// <returns>Returns removal status</returns>
        Boolean Remove(String key);

        /// <summary>
        /// Clears cache
        /// </summary>>
        void ClearCache();

        /// <summary>
        /// Gets all cache keys available in cache
        /// </summary>
        /// <returns>Returns collection of cache key names</returns>
        Collection<String> GetAllCacheKeys();
    }
}