using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Web;
using System.Web.Caching;


namespace MDM.CacheManager.Business
{
    /// <summary>
    /// This uses the built in ASP.NET Cache system
    /// </summary>
    public class InternalCache : ICache
    {
        #region Fields

        private Cache cacheValue = null;

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor. Initializes the InternalCache class.
        /// </summary>		
        public InternalCache()
        {
            //The HttpRuntime instance of Cache is required for making it available in both Web and Web Services Applications 
            cacheValue = HttpRuntime.Cache;

            if (cacheValue == null)
            {
                throw new InvalidOperationException("This internal cache implementation is only for Web and Windows application");
            }
        }

        #endregion

        #region Properties

        private Cache CacheInstance
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
        /// Get the object stored in cache
        /// </summary>
        /// <param name="key">The cache key used to store the data</param>
        /// <returns>The stored object 
        /// <remarks>if the object is not present you would get null</remarks></returns>
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
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public object Get(string key)
        {
            Object keyValue = null;
            keyValue = CacheInstance[key];
            return keyValue;
        }

        /// <summary>
        /// Get an object or string from cache
        /// </summary>
        /// <typeparam name="T">The object or string type that can be serialized using datacontract serializer</typeparam>
        /// <param name="key">The name of the key to insert to cache</param>
        /// <returns>The cached object converted to the requested type</returns>
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
                CacheInstance.Insert(key, value, new CacheDependency(dependantFileName)); //insert the file dependency
            }
            catch
            {
                throw new ApplicationException(String.Format("Not able to insert Cache Item with File Dependency {0}", key));
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
                CacheInstance.Insert(key, value, new CacheDependency(dependantFileName)); //insert the file dependency
            }
            catch
            {
                throw new ApplicationException(String.Format("Not able to insert Cache Item with File Dependency {0}", key));
            }
        }

        /// <summary>
        /// Store the object in Cache for a specific duration and don't allow the CLR to do a Garbage Collection to collect this object in case of a memory pressure.
        /// </summary>
        /// <param name="key">The cache key used to store the data</param>
        /// <param name="value">The object to be stored</param>
        /// <param name="expiryTime">The amount of time the object needs to be in Cache</param>
        public void Set(String key, Object value, DateTime expiryTime)
        {
            try
            {
                CacheInstance.Insert(key, value,
                    null,  //no file dependency
                    expiryTime, //absolute expiry
                    Cache.NoSlidingExpiration, //no sliding expiry
                    CacheItemPriority.NotRemovable, //The cache items with this priority level will not be automatically deleted from the cache as the server frees system memory
                    null);
            }
            catch
            {
                throw new ApplicationException(String.Format("Not able to insert Cache Item with Absolute Expiry {0}", key));
            }
        }

        /// <summary>
        /// Stores the object in cache for a specific duration.
        /// It does not allow the CLR to do a garbage collection to collect this object in case of a memory pressure.
        /// </summary>
        /// <typeparam name="T">Indicates the object or string type that can be serialized using data contract serializer</typeparam>
        /// <param name="key">Indicates the cache key used to store the data</param>
        /// <param name="value">Indicates the object to be stored</param>
        /// <param name="expiryTime">Indicates the amount of time the object needs to be in Cache</param>
        public void Set<T>(string key, T value, DateTime expiryTime)
        {
            try
            {
                CacheInstance.Insert(key, value,
                    null,  //no file dependency
                    expiryTime, //absolute expiry
                    Cache.NoSlidingExpiration, //no sliding expiry
                    CacheItemPriority.NotRemovable, //The cache items with this priority level will not be automatically deleted from the cache as the server frees system memory
                    null);
            }
            catch
            {
                throw new ApplicationException(String.Format("Not able to insert Cache Item with Absolute Expiry {0}", key));
            }
        }

        /// <summary>
        /// Store the object in cache for a sliding duration.
        /// It does not allow the CLR to do a garbage collection to collect this object in case of a memory pressure.
        /// </summary>
        /// <param name="key">The cache key used to store the data</param>
        /// <param name="value">The object to be stored</param>
        /// <param name="timeOutInMinutes">The sliding expiration time out the cache needs to be kept alive</param>
        public void Set(String key, Object value, Int32 timeOutInMinutes)
        {
            try
            {
                CacheInstance.Insert( key, value,
                    null,  //no file dependency
                    System.Web.Caching.Cache.NoAbsoluteExpiration, //no absolute expiry
                    TimeSpan.FromMinutes(timeOutInMinutes), //sliding expiry
                    CacheItemPriority.NotRemovable, //The cache items with this priority level will not be automatically deleted from the cache as the server frees system memory
                    null);
            }
            catch
            {
                throw new ApplicationException(String.Format("Not able to insert Cache Item with Absolute Expiry {0}", key));
            }
        }

        /// <summary>
        /// Store the object in Cache for a sliding duration and don't allow the CLR to do a Garbage Collection to collect this object in case of a memory pressure.
        /// </summary>
        /// <param name="key">The cache key used to store the data</param>
        /// <param name="value">The object to be stored</param>
        /// <param name="timeOutInMinutes">The sliding expiration time out the cache needs to be kept alive</param>
        public void Set<T>(string key, T value, int timeOutInMinutes)
        {
            try
            {
                CacheInstance.Insert(key, value,
                    null,  //no file dependency
                    System.Web.Caching.Cache.NoAbsoluteExpiration, //no absolute expiry
                    TimeSpan.FromMinutes(timeOutInMinutes), //sliding expiry
                    CacheItemPriority.NotRemovable, //The cache items with this priority level will not be automatically deleted from the cache as the server frees system memory
                    null);
            }
            catch
            {
                throw new ApplicationException(String.Format("Not able to insert Cache Item with Absolute Expiry {0}", key));
            }
        }


        /// <summary>
        /// Remove an object or a string from cache
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
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
        /// Remove the Object from cache
        /// </summary>
        /// <param name="key">The key for the cached Object</param>
        /// <returns>Removal status</returns>
        public Boolean Remove(String key)
        {
            CacheInstance.Remove(key);
            return true;
        }

        /// <summary>
        /// Clears cache
        /// </summary>
        public void ClearCache()
        {
            foreach(DictionaryEntry item in CacheInstance)
            {
                String cacheKeyName = String.Empty;
                cacheKeyName = item.Key.ToString();
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
            foreach (DictionaryEntry item in CacheInstance)
            {
                String cacheKeyName = String.Empty;
                cacheKeyName = item.Key.ToString();
                if (!String.IsNullOrEmpty(cacheKeyName))
                {
                    cacheKeyNames.Add(cacheKeyName);
                }
            }
            return cacheKeyNames;
        }
 
        #endregion
    }
}
