using System;

namespace MDM.CacheManager.Business
{
    using MDM.Core;
    using MDM.ExceptionManager;

    /// <summary>
    /// Creates a Cache controller based on the settings set in config file
    /// The Individual Cache controllers should implement the ICache Interface
    /// </summary>
    public class CacheFactory
    {
        #region Fields

        private static CacheType _defaultCacheMode = CacheType.UnKnown;

        private static CacheType _defaultDistributedCacheMode = CacheType.UnKnown;

        private static CacheType _defaultNotificationCacheMode = CacheType.UnKnown;

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor. Initializes the CacheFactory class.
        /// </summary>		
        public CacheFactory()
        {

        }

        #endregion

        #region Properties

        /// <summary>
        /// The Cache Mode determines which Cache Controller to load
        /// </summary>
        /// <remarks>The configuration should have a CacheMode App Settings Key</remarks>
        public static CacheType DefaultInternalCacheType
        {
            get
            {
                String cacheModeValue = String.Empty;
                if (_defaultCacheMode == CacheType.UnKnown)
                {
                    try
                    {
                        cacheModeValue = AppConfiguration.GetSetting("InternalCacheType");

                        if (String.IsNullOrWhiteSpace(cacheModeValue))
                        {
                            cacheModeValue = "Framework4";
                        }
                    }
                    catch
                    {
                        cacheModeValue = "Framework4";
                    }

                    _defaultCacheMode = (CacheType)Enum.Parse(typeof(CacheType), cacheModeValue);
                }

                return _defaultCacheMode;
            }
        }

        /// <summary>
        /// The Cache Mode determines which Cache Controller to load
        /// </summary>
        /// <remarks>The configuration should have a CacheMode App Settings Key</remarks>
        public static CacheType DefaultDistributedCacheType
        {
            get
            {
                String cacheModeValue = String.Empty;
                if (_defaultDistributedCacheMode == CacheType.UnKnown)
                {
                    try
                    {
                        cacheModeValue = AppConfiguration.GetSetting("DistributedCacheType");
                    }
                    catch
                    {
                        cacheModeValue = "None";
                    }
                    _defaultDistributedCacheMode = (CacheType)Enum.Parse(typeof(CacheType), cacheModeValue);
                }

                return _defaultDistributedCacheMode;
            }
        }

        /// <summary>
        /// The Cache Mode determines which Cache Controller to load
        /// </summary>
        /// <remarks>The configuration should have a CacheMode App Settings Key</remarks>
        public static CacheType DefaultNotificationCacheType
        {
            get
            {
                String cacheModeValue = String.Empty;
                if (_defaultNotificationCacheMode == CacheType.UnKnown)
                {
                    try
                    {
                        cacheModeValue = AppConfiguration.GetSetting("NotificationCacheType");
                    }
                    catch
                    {
                        cacheModeValue = "None";
                    }
                    _defaultNotificationCacheMode = (CacheType)Enum.Parse(typeof(CacheType), cacheModeValue);
                }

                return _defaultNotificationCacheMode;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Get the Cache Controller based on the Application Settings "DefaultCacheMode"
        /// </summary>
        /// <returns>The Cache Controller exposed as an Interface
        /// <remarks>Check for null before proceeding</remarks>
        /// </returns>
        public static ICache GetCache()
        {
            CacheType cacheType = CacheFactory.DefaultInternalCacheType;
            ICache cache = null;

            try
            {
                switch (cacheType)
                {
                    //Implement Memcached
                    case CacheType.Memcached:
                        {
                            cache = new MemcachedCache();
                            break;
                        }
                    //Use ASP.NET Caching
                    case CacheType.AspNet:
                        {
                            cache = new InternalCache();
                            break;
                        }
                    //Use .NET framework 4.0 Caching
                    case CacheType.Framework4:
                        {
                            cache = new FrameworkFourCache();
                            break;
                        }
                    //Use AppFabric Caching
                    case CacheType.AppFabric:
                        {
                            cache = AppFabricCache.GetAppFabricCahe();
                            break;
                        }
                    // Use Distributed (Appfabric) with Local Cache enabled
                    case CacheType.AppFabricWithNotificationEnabled:
                        {
                            cache = DistributedCacheWithNotification.GetDistributedCacheWithNotification();
                            break;
                        }
                    //No cache
                    case CacheType.None:
                    case CacheType.UnKnown:
                        {
                            cache = null;
                            break;
                        }
                    //No cache
                    default:
                        {
                            //do nothing
                            //return null
                            break;
                        }
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler exHandler = new ExceptionHandler(ex);
            }
            finally
            {
                if (cache == null)
                {
                    cache = new FrameworkFourCache();
                }
            }
            return cache;

        }

        /// <summary>
        /// Get the Cache Controller for passed cache mode and for the default named instance
        /// </summary>
        /// <returns></returns>
        public static IDistributedCache GetDistributedCache()
        {
            return GetDistributedCache(String.Empty);
        }

        /// <summary>
        /// Get the Cache Controller for passed cache mode. Example: "AppFabric", "Memcached"
        /// </summary>
        /// <returns>The Cache Controller exposed as an Interface
        /// <remarks>Check for null before proceeding</remarks>
        /// </returns>
        public static IDistributedCache GetDistributedCache(String cacheInstance)
        {
            CacheType cacheType = CacheFactory.DefaultDistributedCacheType;

            IDistributedCache cache = null;

            try
            {
                switch (cacheType)
                {
                    //Implement Memcached
                    case CacheType.Memcached:
                        {
                            cache  = new MemcachedCache();
                            break;
                        }
                    //Use ASP.NET Caching
                    case CacheType.AspNet:
                        {
                            throw new ApplicationException(String.Format("CacheMode {0} is not an applicable caching mode for distributed cache", cacheType.ToString()));
                        }
                    //NOTE: FrameworkFourCache also supports fall back for distributed caching. This is just to test MDM application without distributed caching enabled. 
                    //This option is not at all recommanded in production
                    case CacheType.Framework4:
                        {
                            cache = new FrameworkFourCache();
                            break;
                        }
                    //Use AppFabric Caching
                    case CacheType.AppFabric:
                        {
                            cache = AppFabricCache.GetAppFabricCahe(cacheInstance);
                            break;
                        }
                    // Use Distributed (Appfabric) with Local Cache enabled
                    case CacheType.AppFabricWithNotificationEnabled:
                        {
                            cache = DistributedCacheWithNotification.GetDistributedCacheWithNotification();
                            break;
                        }
                    //No cache
                    case CacheType.None:
                        {
                            cache = new DummyDistributedCache();
                            break;
                        }
                    case CacheType.UnKnown:
                        {
                            cache = null;
                            break;
                        }
                    //No cache
                    default:
                        {
                            //do nothing
                            //return null
                            break;
                        }
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler exHandler = new ExceptionHandler(ex);
            }
            finally
            {
             
            }
            return cache;
        }

        /// <summary>
        /// Get the Cache Controller for Distributed cache with local enabled. Example: "AppFabricWithLocalCacheEnabled", "AppFabric" 
        /// </summary>
        /// <returns>The Cache Controller exposed as an Interface
        /// <remarks>Check for null before proceeding</remarks>
        /// </returns>
        public static IDistributedCache GetDistributedCacheWithNotification()
        {
            IDistributedCache cache = null;            
            try
            {
                CacheType cacheType = CacheFactory.DefaultNotificationCacheType;
                switch (cacheType)
                {
                    //Implement Memcached
                    case CacheType.Memcached:
                        {
                            cache = new MemcachedCache();
                            break;
                        }
                    //Use ASP.NET Caching
                    case CacheType.AspNet:
                        {
                            throw new ApplicationException(String.Format("CacheMode {0} is not an applicable caching mode for distributed cache", cacheType.ToString()));
                        }
                    //NOTE: FrameworkFourCache also supports fall back for distributed caching. This is just to test MDM application without distributed caching enabled. 
                    //This option is not at all recommanded in production
                    case CacheType.Framework4:
                        {
                            cache = new FrameworkFourCache();
                            break;
                        }
                    //Use AppFabric Caching
                    case CacheType.AppFabric:
                        {
                            cache = AppFabricCache.GetAppFabricCahe();
                            break;
                        }
                    // Use Distributed (Appfabric) with Local Cache enabled
                    case CacheType.AppFabricWithNotificationEnabled:
                        {
                            cache = DistributedCacheWithNotification.GetDistributedCacheWithNotification();
                            break;
                        }
                    //No cache
                    case CacheType.None:
                        {
                            cache = new DummyDistributedCache();
                            break;
                        }
                    case CacheType.UnKnown:
                        {
                            cache = null;
                            break;
                        }
                    //No cache
                    default:
                        {
                            //do nothing
                            //return null
                            break;
                        }
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler exHandler = new ExceptionHandler(ex);
            }
            return cache;
        }

        #endregion
    }
}
