using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;


namespace MDM.CacheManager.Business
{
    using NMemcached.Client;

    /// <summary>
    /// 
    /// </summary>
    public sealed class MemcachedCache : IDistributedCache
    {
        #region Fields

        private static MemcachedClient _cacheInstance;

        private static object syncRoot = new Object();

        #endregion
        
        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>		
        public MemcachedCache()
        {
        }

        #endregion

        #region Properties

        private static MemcachedClient CacheInstance
        {
            get
            {
                if (_cacheInstance == null)
                {
                    lock (syncRoot)
                    {
                        if (_cacheInstance == null)
                            _cacheInstance = MemcachedClient.Create();
                    }
                }

                return _cacheInstance;
            }
        }

        #endregion

        #region Methods

        private Byte[] Serialize<T>(T objectInstance)
        {
            Byte[] objectBytes = null;
            XmlDictionaryWriter binaryWriter = null;
            MemoryStream stream = null;
            DataContractSerializer serializer = null;

            if (objectInstance != null)
            {
                stream = new MemoryStream();
                binaryWriter = XmlDictionaryWriter.CreateBinaryWriter(stream);

                serializer = new DataContractSerializer(typeof(T));
                serializer.WriteObject(binaryWriter, objectInstance);

                //required to move the binary writer memory to the underlying memory stream.
                binaryWriter.Flush();

                stream.Seek(0, SeekOrigin.Begin);

                //capture the bytes
                objectBytes = stream.ToArray();
                
                //close resources
                binaryWriter.Close();
                stream.Close();
            }

            return objectBytes;
        }

        private T DeSerialize<T>(byte[] objectBytes)
        {
            DataContractSerializer serializer = new DataContractSerializer(typeof(T));
            MemoryStream stream = null;
            object cachedObject = null;
            XmlDictionaryReader binaryReader = null;

            if (objectBytes != null)
            {
                stream = new MemoryStream(objectBytes);
                stream.Position = 0;

                binaryReader = XmlDictionaryReader.CreateBinaryReader(stream, new XmlDictionaryReaderQuotas());
                cachedObject = serializer.ReadObject(binaryReader);

                //close resources
                binaryReader.Close();
                stream.Close();
            }

            return (T)cachedObject;
        }

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
            get { throw new NotImplementedException(); }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public object Get(string key)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Get an object or string from cache
        /// </summary>
        /// <typeparam name="T">The object or string type that can be serialized using datacontract serializer</typeparam>
        /// <param name="key">The name of the key to insert to cache</param>
        /// <returns>The cached object converted to the requested type</returns>
        public T Get<T>(String key)
        {
            Type instanceType = typeof(T);
            Byte[] cachedBytes = null;
            T instanceObject;

            //for strings get directly
            if (instanceType.ToString().Equals("System.String") || instanceType.ToString().Contains("Byte"))
            {
                instanceObject = (T)CacheInstance.Get(key);
            }
            else
            {                    
                cachedBytes = CacheInstance.Get(key) as byte[];
                instanceObject = DeSerialize<T>(cachedBytes);
            }

            return instanceObject;
        }

        /// <summary>
        /// Sets an object or a string in cache
        /// </summary>
        /// <typeparam name="T">Indicates the object or string type that can be serialized using datacontract serializer</typeparam>
        /// <param name="key">Indicates the key to insert to the cache</param>
        /// <param name="value">Indicates the object instance</param>
        /// <param name="expiryTime">Indicates the time duration to hold the object in cache</param>
        public void Set<T>(String key, T value, DateTime expiryTime)
        {
            ResponseCode response;
            Byte[] objectBytes;

            //for strings store directly
            if (value.GetType().ToString().Equals("System.String") || value.GetType().ToString().Contains("Byte"))
            {
                response = CacheInstance.Set(key, value, expiryTime);
            }
            else
            {
                objectBytes = Serialize<T>(value);
                response = CacheInstance.Set(key, objectBytes, expiryTime);
            }
        }

        /// <summary>
        /// Store the object in cache with a file dependency so that if a file changes the cache object needs to be removed
        /// </summary>
        /// <param name="key">The cache key used to store the data</param>
        /// <param name="value">The object to be stored</param>
        /// <param name="dependantFileName">The file name along with the path the object depends on </param>
        public void Set(String key, Object value, String dependantFileName)
        {
            throw new NotImplementedException(); 
        }

        /// <summary>
        /// Store the object in cache with a file dependency so that if a file changes the cache object needs to be removed
        /// </summary>
        /// <param name="key">The cache key used to store the data</param>
        /// <param name="value">The object to be stored</param>
        /// <param name="dependantFileName">The file name along with the path the object depends on </param>
        public void Set<T>(String key, T value, String dependantFileName)
        {
            throw new NotImplementedException(); 
        }

        /// <summary>
        /// Store the object in Cache for a specific duration and don't allow the CLR to do a Garbage Collection to collect this object in case of a memory pressure.
        /// </summary>
        /// <param name="key">The cache key used to store the data</param>
        /// <param name="value">The object to be stored</param>
        /// <param name="expiryTime">The amount of time the object needs to be in Cache</param>
        public void Set(String key, Object value, DateTime expiryTime)
        {
            throw new NotImplementedException(); 
        }

        /// <summary>
        /// Store the object in Cache for a sliding duration and don't allow the CLR to do a Garbage Collection to collect this object in case of a memory pressure.
        /// </summary>
        /// <param name="key">The cache key used to store the data</param>
        /// <param name="value">The object to be stored</param>
        /// <param name="timeOutInMinutes">The sliding expiration time out the cache needs to be kept alive</param>
        public void Set(String key, Object value, Int32 timeOutInMinutes)
        {
            throw new NotImplementedException();   
        }

        /// <summary>
        /// Store the object in Cache for a sliding duration and don't allow the CLR to do a Garbage Collection to collect this object in case of a memory pressure.
        /// </summary>
        /// <param name="key">The cache key used to store the data</param>
        /// <param name="value">The object to be stored</param>
        /// <param name="timeOutInMinutes">The sliding expiration time out the cache needs to be kept alive</param>
        public void Set<T>(string key, T value, int timeOutInMinutes)
        {
            throw new NotImplementedException(); 
        }

        /// <summary>
        /// Remove the object of type from cache
        /// </summary>
        /// <typeparam name="T">The type of the object to remove so that we dont remove the wrong object</typeparam>
        /// <param name="key">The key for the cached object</param>
        /// <returns>The object that was cached, null in case the key doesn't exit</returns>
        public T Remove<T>(string key)
        {
            ResponseCode response;
            T cachedObject;

            cachedObject = Get<T>(key);

            response = CacheInstance.Delete(key);

            return (T)cachedObject;
        }

        /// <summary>
        /// Remove an object stored in cache and return status
        /// </summary>
        /// <param name="key">The key for the object to be removed</param>
        /// <returns>The removed object</returns>
        public Boolean Remove(String key)
        {
            ResponseCode responseCode;

            responseCode = CacheInstance.Delete(key);

            if (responseCode == ResponseCode.NoError)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Clears cache
        /// </summary>
        public void ClearCache() 
        {
            throw new NotImplementedException(); 
        }

        /// <summary>
        /// Gets all cache keys
        /// </summary>
        /// <returns>Returns a collection of cache keys</returns>
        public Collection<String> GetAllCacheKeys()
        {
            throw new NotImplementedException(); 
        }

        #endregion

        #region IDistributedCache Members

        /// <summary>
        /// Gets data in bulk based on specified cache keys of specified chache region. Not supported in Windows Azure Shared Caching.
        /// </summary>
        /// <param name="keys">Indicates list of keys for the objects to retrieve, cannot be null.</param>
        /// <param name="region">Indicates name of the region, cannot be null.</param>
        /// <returns>Returns collection of key and value pair of cached data</returns>
        public System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<string, object>> BulkGet(System.Collections.Generic.IEnumerable<string> keys, string region)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets an object from the specified region by using the specified key. Not supported in Windows Azure Shared Caching.
        /// </summary>
        /// <param name="key">Indicates keys for which data needs to be fetched</param>
        /// <param name="region">Indiactes the name of the region where the object resides</param>
        /// <returns>Returns an object that was cached using the specified key. Null is returned if the key does not exist.</returns>
        public object Get(string key, string region)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets an enumerable list of all cached objects in specified region that have all same tags in common.Not supported in Windows Azure Shared Caching.
        /// </summary>
        /// <param name="tags">Indiactes a list of tags for which search should happen.</param>
        /// <param name="region">Indicates name of the region to search. Tags are not supported outside regions. Therefore, a region name is required.</param>
        /// <returns>Returns an enumerable list of all cached objects in the specified region that have all same tags in common.</returns>
        public System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<string, object>> GetObjectsByAllTags(System.Collections.Generic.IEnumerable<string> tags, string region)
        {
            throw new NotImplementedException();
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
        public System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<string, object>> GetObjectsByAnyTags(System.Collections.Generic.IEnumerable<string> tags, string region)
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
        /// Null is returned if no objects in the specified region have the tag specified.
        /// </returns>
        public System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<string, object>> GetObjectsByTag(string tag, string region)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets an enumerable list of all cached objects in the specified region. Not supported in Windows Azure Shared Caching.
        /// </summary>
        /// <param name="region">Indicates the name of the region for which to return a list of all resident objects.</param>
        /// <returns>Returns an enumerable list of all cached objects in the specified region.</returns>
        public System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<string, object>> GetObjectsInRegion(string region)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets regions for the cache. Not supported in Windows Azure Shared Caching.
        /// </summary>
        /// <returns>Returns an enumerable list of default regions</returns>
        public System.Collections.Generic.IEnumerable<string> GetSystemRegions()
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
            throw new NotImplementedException();
        }

        /// <summary>
        /// Adds or replaces an object in the specified list of tags. Specifies the timeout value of the cached object.
        /// </summary>
        /// <param name="key">Indicates unique value that is used to identify the object in the region.</param>
        /// <param name="value">Indicates an object to add or replace</param>
        /// <param name="expiryTime">Indicates expiration time for the object</param>
        /// <param name="tags">Indicates a list of tags to associate with the object.</param>
        public void Set(string key, object value, DateTime expiryTime, System.Collections.Generic.List<string> tags)
        {
            throw new NotImplementedException();
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
        public void Set(string key, object value, DateTime expiryTime, string region, System.Collections.Generic.List<string> tags)
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }

        /// <summary>
        /// Adds or replaces an object in the specified region having specified tags. Specifies the timeout value of the cached object.
        /// </summary>
        /// <param name="key">Indicates unique value that is used to identify the object in the region.</param>
        /// <param name="value">Indicates the object to add or replace.</param>
        /// <param name="timeOutInMinutes">Indicates the amount of time that the object should reside in the cache before expiration.</param>
        /// <param name="tags">Indicates the name of the region the object resides in.</param>
        public void Set(string key, object value, int timeOutInMinutes, System.Collections.Generic.List<string> tags)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Adds or replaces an object in the specified region having specified tags. 
        /// Specifies the timeout value of the cached object.Not supported in Windows Azure Shared Caching.
        /// </summary>
        /// <param name="key">Indicates unique value that is used to identify the object in the region.</param>
        /// <param name="value">Indicates the object to add or replace.</param>
        /// <param name="timeOutInMinutes">Indicates the amount of time that the object should reside in the cache before expiration.</param>
        /// <param name="region">Indicates a list of tags to associate with the object.</param>
        /// <param name="tags">Indicates the name of the region the object resides in.</param>
        public void Set(string key, object value, int timeOutInMinutes, string region, System.Collections.Generic.List<string> tags)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Creates a region.
        /// </summary>
        /// <param name="region">Indicates the name of the region that is created.</param>
        /// <returns>Returns true if region is created succesfully and false if the region already exists.</returns>
        public Boolean CreateRegion(String region)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Clears all regions.
        /// </summary>
        /// <returns>Returns true if all regions are cleared successfully, otherwise false.</returns>
        public Boolean ClearAll()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
