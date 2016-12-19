using System;
using System.Collections.ObjectModel;

namespace MDM.Utility
{
    using MDM.Core;
    using MDM.BusinessObjects;
    using MDM.CacheManager.Business;
    using MDM.Interfaces;
    using MDM.DataProviderInterfaces;
    
    /// <summary>
    /// Represents an abstract class for providing MDM Feature configuration.
    /// </summary>
    public abstract class MDMFeatureConfigProviderBase : IMDMFeatureConfigProvider
    { 
        #region Methods

        #region Public Methods       

        /// <summary>
        /// Returns the MDM Feature configuration by application, module name and version
        /// </summary>        
        /// <param name="application">Indicates the Application for requested feature config</param>
        /// <param name="moduleName">Indicates module short name for requested feature config</param>             
        /// <param name="version">Indicates the version  for requested feature config</param>   
        /// <returns>Returns MDMFeature Config based on application, module name and version</returns>
        public MDMFeatureConfig GetMDMFeatureConfig(MDMCenterApplication application, String moduleName, String version)
        {
            String key = CacheKeyGenerator.GetMDMFeatureConfigKey(application, moduleName, version);
            MDMFeatureConfig mdmFeatureConfig = GetDataFromCache(key);

            if (mdmFeatureConfig == null)
            {
                mdmFeatureConfig = (MDMFeatureConfig)GetMDMFeatureConfigInDB(application, moduleName, version);
                //update the cache now
                if (mdmFeatureConfig != null)
                {
                    SetDataInCache(key, mdmFeatureConfig, false); 
                }
            }

            return mdmFeatureConfig;
        }

        /// <summary>
        /// Load all available MDM Feature configurations from the database.
        /// </summary>
        /// <param name="isReloadOperation">Specifies whether the function is invoked for a reloading of MDM feature configs.</param>
        public void LoadAllMDMFeatureConfigs(Boolean isReloadOperation = false)
        {
            ClearCache(false);
            
            //Then load MDM feature configs from database.
            LoadMDMFeatureConfigsInDB(isReloadOperation);              
        }
        

        /// <summary>
        /// Reloads the MDM Feature configuration for the specified configuration key into cache. 
        /// </summary>
        /// <param name="application">Indicates the Application for requested feature config</param>
        /// <param name="moduleName">Indicates module short name for requested feature config</param>             
        /// <param name="version">Indicates the version  for requested feature config</param>       
        public void ReloadMDMFeatureConfig(MDMCenterApplication application, String moduleName, String version)
        {
            Boolean isReloadOperation = true;
            MDMFeatureConfig mdmFeatureConfig = (MDMFeatureConfig)GetMDMFeatureConfigInDB(application, moduleName, version);            

            if (mdmFeatureConfig != null)
            {
                String key = CacheKeyGenerator.GetMDMFeatureConfigKey(application, moduleName, version);
                SetDataInCache(key, mdmFeatureConfig, isReloadOperation);
            }

        }               

        /// <summary>
        /// Clears the MDM Feature configuration cache.
        /// </summary>        
        public void ClearCache()
        {
            ClearCache(false);
        }

        #endregion

        #region Protected Abstract Methods

        /// <summary>
        /// Returns all MDM Feature configurations thats are stored in the database.
        /// </summary>
        /// <returns></returns>
        protected abstract IMDMFeatureConfigCollection GetAllMDMFeatureConfigsInDB();

        /// <summary>
        /// Gets MDM Feature configurations by application, module name and version
        /// </summary>
        /// <param name="application">Indicates the Application for requested feature config</param>
        /// <param name="moduleName">Indicates module short name for requested feature config</param>             
        /// <param name="version">Indicates the version  for requested feature config</param>   
        /// <returns>Returns MDM Feature configurations based on application, module name and version</returns>
        protected abstract IMDMFeatureConfig GetMDMFeatureConfigInDB(MDMCenterApplication application, String moduleName, String version);
        
        #endregion

        #region Private Methods 

        /// <summary>
        /// Loads MDM feature configurations from database.
        /// </summary>
        private void LoadMDMFeatureConfigsInDB(Boolean isReloadOperation)
        {
            IMDMFeatureConfigCollection mdmFeatureConfigs = GetAllMDMFeatureConfigsInDB();

            if (mdmFeatureConfigs != null && mdmFeatureConfigs.Count > 0)
            {
                ICache cacheManager = CacheFactory.GetCache();
                foreach (MDMFeatureConfig mdmFeatureConfig in mdmFeatureConfigs)
                {
                    if (mdmFeatureConfig != null)
                    {
                        String key = CacheKeyGenerator.GetMDMFeatureConfigKey(mdmFeatureConfig.Application, mdmFeatureConfig.ModuleName, mdmFeatureConfig.Version);
                        SetDataInCache(key, mdmFeatureConfig, isReloadOperation);
                    }
                }
            }
        }       

        /// <summary>
        /// Returns the data from cache for the specified cache key.
        /// </summary>
        private MDMFeatureConfig GetDataFromCache(String mdmFeatureConfigKey)
        {
            String cacheKey = String.Empty;
            MDMFeatureConfig data = null;            
           
            cacheKey = CacheKeyGenerator.GetMDMFeatureConfigCacheKey(mdmFeatureConfigKey);

            ICache cacheManager = CacheFactory.GetCache();
            data = cacheManager.Get<MDMFeatureConfig>(cacheKey);
            
            return data;
        }

        /// <summary>
        /// Inserts the data to cache for the specified cache key.
        /// The cache invalidation is performed when the configurations are reloaded (i.e. isDataUpdated is set to true). 
        /// </summary>
        private void SetDataInCache(String mdmFeatureConfigKey, Object data, Boolean isDataUpdated)
        {
            String cacheKey = String.Empty;
           
            DateTime expirationTime = DateTime.Now.AddDays(10);
            cacheKey = CacheKeyGenerator.GetMDMFeatureConfigCacheKey(mdmFeatureConfigKey);

            CacheSynchronizationHelper cacheSynchronizationHelper = new CacheSynchronizationHelper();
            cacheSynchronizationHelper.NotifyLocalCacheInsert(cacheKey, expirationTime, isDataUpdated);

            if (data != null)
            {
                ICache cacheManager = CacheFactory.GetCache();
                cacheManager.Set(cacheKey, data, expirationTime);
            }
           
        }

        /// <summary>
        /// Clears MDM Feature configurations data from Cache.
        /// </summary>
        /// <param name="notifyCacheServer">Specifies whether to notify AppFabric while clearing cache. If the method is invoked just before reloading cache, 
        /// the asynchronous cache notification might clear the data which was just loaded after the clear operation.</param>
        private void ClearCache(Boolean notifyCacheServer)
        {
            ICache cacheManager = CacheFactory.GetCache();
            CacheSynchronizationHelper cacheSynchronizationHelper = null;

            if (notifyCacheServer)
            {
                cacheSynchronizationHelper = new CacheSynchronizationHelper();                
            }

            Collection<String> cacheKeys = cacheManager.GetAllCacheKeys();
            foreach (String cacheKey in cacheKeys)
            {
                if (cacheKey.StartsWith(CacheKeyContants.MDMFEATURE_CONFIG_CACHE_KEY_PREFIX))
                {
                    cacheManager.Remove(cacheKey);

                    if (notifyCacheServer)
                        cacheSynchronizationHelper.NotifyLocalCacheRemoval(cacheKey);
                }
            }
        }        

        #endregion

        #endregion      
    }
}
