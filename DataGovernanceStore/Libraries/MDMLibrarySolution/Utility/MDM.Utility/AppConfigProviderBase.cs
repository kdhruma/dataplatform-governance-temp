using System;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Xml;
using System.Collections.ObjectModel;

namespace MDM.Utility
{
    using Core;

    using BusinessObjects;
    using BusinessObjects.Caching;
    using BusinessObjects.Diagnostics;

    using CacheManager.Business;
    using ExceptionManager.Handlers;

    using Interfaces;
    using DataProviderInterfaces;

    /// <summary>
    /// Represents an abstract class for providing application configuration.
    /// </summary>
    public abstract class AppConfigProviderBase : IAppConfigProvider
    {
        #region Private Members

        /// <summary>
        /// Represents the application configuration key for trace configuration.
        /// </summary>
        private const String _keyForTraceConfig = "MDMCenter.TracingProfile";

        #endregion

        #region Methods

        #region Public Methods

        /// <summary>
        /// Returns the application configuration for the specified configName.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public T GetAppConfig<T>(String key, T defaultValue)
        {
            T returnVal = defaultValue;

            Boolean isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;
            try
            {
                String strValue = GetAppConfig(key);

                // If T is of type string, allow empty values to be returned. 
                if (!String.IsNullOrWhiteSpace(strValue) || typeof(T) == typeof(String))
                {
                    returnVal = (T)Convert.ChangeType(strValue, typeof(T));
                }
            }
            catch (InvalidCastException)
            {
                WriteLog(key, TraceEventType.Error, "AppConfig - '" + key + "'value is not as per the required format. Would assume " +
                        defaultValue + " and continue");
            }
            catch
            {
                WriteLog(key, TraceEventType.Error, "Missing AppConfig - '" + key + "'. Will assume value = " + defaultValue + " and continue");
            }

            return returnVal;
        }

        /// <summary>
        /// Function to load all available AppConfigs, from config files and database
        /// </summary>
        /// <param name="isReloadOperation">Specifies whether the function is invoked for a reloading of app configs.</param>
        public void LoadAllAppConfigs(Boolean isReloadOperation = false)
        {
            ClearCache(false);

            LoadAppSettings(isReloadOperation);

            //then load app configs from database
            LoadAppConfigsInDB(isReloadOperation);

            //Load relationship denorm processing settings
            LoadRelationshipDenormProcessingSettings();

            // Load Diagnostics Storage mode
            LoadDiagnosticStorageMode();

            // Load Diagnostics SystemLog LegacySourcesToSkip mode
            LoadDiagnosticSystemLogLegacySourcesToSkip();

            //Load current tracing profile instance
            LoadTracingProfile();

            //Load role mappings
            LoadRoleMappings(isReloadOperation);

            //Load claim types mapping
            LoadClaimTypesMapping(isReloadOperation);

            // Loads track history settings with source tracking module enabled
            LoadTrackHistoryOnValueChange();
        }

        /// <summary>
        /// Reloads the application configuration for the specified configuration key into cache. 
        /// </summary>
        /// <param name="name"></param>
        public void ReloadAppConfig(String name)
        {
            Boolean isReloadOperation = true;

            IAppConfig appConfig = GetAppConfigInDB(name.Trim());

            if (appConfig != null && !String.IsNullOrEmpty(appConfig.Name) && appConfig.Value != null)
            {
                SetDataInCache(appConfig.Name, appConfig.Value, isReloadOperation);
            }

            switch (name)
            {
                case "MDMCenter.TransactionSettings":
                    {
                        LoadTransactionSettings();
                        break;
                    }
                case "MDMCenter.Diagnostics.Tracing.Enabled":
                case "MDMCenter.Diagnostics.PerformanceTracing.Enabled":
                    {
                        LoadApplicationLevelTracingSetting();
                        break;
                    }
                case "MDMCenter.Diagnostics.Tracing.StorageMode":
                    {
                        LoadDiagnosticStorageMode();
                        break;
                    }
                case "MDMCenter.Diagnostics.SystemLog.LegacyTraceSourcesToSkip":
                    {
                        LoadDiagnosticSystemLogLegacySourcesToSkip();
                        break;
                    }
                case "MDMCenter.TracingProfile":
                    {
                        LoadTracingProfile();
                        break;
                    }
                case "MDMCenter.RelationshipDenorm.ProcessingSettings":
                    {
                        LoadRelationshipDenormProcessingSettings(true);
                        break;
                    }
                case "MDMCenter.ClaimsBasedAuthentication.RoleMappings":
                    {
                        LoadRoleMappings(true);
                        break;
                    }
                case "MDMCenter.ClaimsBasedAuthentication.ClaimTypesMappingConfiguration":
                    {
                        LoadClaimTypesMapping(true);
                        break;
                    }
                case "MDMCenter.Entity.SourceTracking.TrackHistoryOnlyOnValueChange":
                    {
                        LoadTrackHistoryOnValueChange();
                        break;
                    }
            }
        }

        /// <summary>
        /// Loads the application configurations that are related to the transaction settings into cache. 
        /// </summary>
        public void LoadTransactionSettings()
        {
            try
            {
                Boolean isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;
                if (isTracingEnabled)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Verbose, "Loading MDMCenter.TransactionSettings..");

                String transactionSettingsXml = GetAppConfig("MDMCenter.TransactionSettings");

                if (!String.IsNullOrWhiteSpace(transactionSettingsXml))
                {
                    //Read transaction settings and populate respective transaction constants..
                    XmlTextReader reader = null;
                    try
                    {
                        reader = new XmlTextReader(transactionSettingsXml, XmlNodeType.Element, null);
                        IDistributedCache cacheManager = CacheFactory.GetDistributedCache();
                        DateTime expirationTime = DateTime.Now.AddDays(10);

                        while (!reader.EOF)
                        {
                            if (reader.NodeType == XmlNodeType.Element && reader.Name == "TransactionSettings")
                            {
                                if (reader.MoveToAttribute("IsolationLevel"))
                                {
                                    Constants.DEFAULT_ISOLATION_LEVEL = (System.Transactions.IsolationLevel)Enum.Parse(typeof(System.Transactions.IsolationLevel), reader.ReadContentAsString());
                                    cacheManager.Set(CacheKeyContants.DEFAULT_ISOLATION_LEVEL, Constants.DEFAULT_ISOLATION_LEVEL, expirationTime);
                                }

                                if (reader.MoveToAttribute("SyncProcessTimeout"))
                                {
                                    Int32 timeOutInSecs = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), 60);
                                    Constants.TRANSACTION_TIMEOUT_SYNCPROCESS = new TimeSpan(0, 0, timeOutInSecs);
                                    cacheManager.Set(CacheKeyContants.TRANSACTION_TIMEOUT_SYNCPROCESS, timeOutInSecs, expirationTime);
                                }

                                if (reader.MoveToAttribute("AsyncProcessTimeout"))
                                {
                                    Int32 timeOutInSecs = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), 300);
                                    Constants.TRANSACTION_TIMEOUT_ASYNCPROCESS = new TimeSpan(0, 0, timeOutInSecs);
                                    cacheManager.Set(CacheKeyContants.TRANSACTION_TIMEOUT_ASYNCPROCESS, timeOutInSecs, expirationTime);
                                }
                            }
                            else
                            {
                                //Keep on reading the xml until we reach expected node.
                                reader.Read();
                            }
                        }
                    }
                    finally
                    {
                        if (reader != null)
                        {
                            reader.Close();
                        }
                    }
                }

                if (isTracingEnabled)
                {
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Verbose, String.Format("Loaded Transaction Settings are : Isolation Level - '{0}'; SyncProcessTimeout - '{1}'; AsyncProcessTimeout - '{2}'", Constants.DEFAULT_ISOLATION_LEVEL.ToString(), Constants.TRANSACTION_TIMEOUT_SYNCPROCESS.ToString(), Constants.TRANSACTION_TIMEOUT_ASYNCPROCESS.ToString()));
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Verbose, "Loading of MDMCenter.TransactionSettings completed.");
                }
            }
            catch (Exception ex)
            {
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, String.Format("Error occurred while loading Transaction settings. Error: {0}", ex.Message));

                //Do not rethrow..
                //Application uses default transaction settings 
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void LoadTracingProfile()
        {
            String tracingProfileAsXml = String.Empty;

            try
            {
                tracingProfileAsXml = GetAppConfig("MDMCenter.TracingProfile");

                // We need to set this in the distribute cache for the notification processors to use.
                IDistributedCache cacheManager = CacheFactory.GetDistributedCache();
                DateTime expirationTime = DateTime.Now.AddDays(10);
                cacheManager.Set(CacheKeyContants.DIAGNOSTIC_TRACING_PROFILE, tracingProfileAsXml, expirationTime);
            }
            catch { }

            if (!String.IsNullOrWhiteSpace(tracingProfileAsXml))
            {
                TracingProfile.LoadCurrent(tracingProfileAsXml);
            }
            else
            {
                TracingProfile.LoadCurrent(new TracingProfile());
            }
        }

        /// <summary>
        /// Processes Tracing Profile
        /// </summary>
        /// <param name="tracingProfile">Specifies Tracing Profile Object</param>
        /// <returns>Operation Result</returns>
        public IOperationResult ProcessTracingProfile(TracingProfile tracingProfile)
        {
            IOperationResult operationResult = null;
            String tracingProfileAppConfigKey = "MDMCenter.TracingProfile";

            if (tracingProfile != null)
            {
                operationResult = ProcessAppConfig(tracingProfileAppConfigKey, tracingProfile.ToXml());

                if (operationResult != null && operationResult.OperationResultStatus != OperationResultStatusEnum.Failed)
                {
                    ReloadAppConfig(tracingProfileAppConfigKey);
                }
            }

            return operationResult;
        }

        /// <summary>
        /// Loads the configuration for application level trace settings into cache. 
        /// </summary>
        public void LoadApplicationLevelTracingSetting()
        {
            //TODO:: This method is not needed now.. would be removed in next check-in
            //IDistributedCache cacheManager = CacheFactory.GetDistributedCache();
            //DateTime expirationTime = DateTime.Now.AddDays(10);

            //Constants.TRACING_ENABLED = GetAppConfig<Boolean>("MDMCenter.Diagnostics.Tracing.Enabled", false);
            //cacheManager.Set(CacheKeyContants.TRACING_ENABLED, Constants.TRACING_ENABLED, expirationTime);

            //Constants.PERFORMANCE_TRACING_ENABLED = GetAppConfig<Boolean>("MDMCenter.Diagnostics.PerformanceTracing.Enabled", false);
            //cacheManager.Set(CacheKeyContants.PERFORMANCE_TRACING_ENABLED, Constants.PERFORMANCE_TRACING_ENABLED, expirationTime);
        }

        /// <summary>
        /// Loads the storage mode. 
        /// </summary>
        public void LoadDiagnosticStorageMode()
        {
            IDistributedCache cacheManager = CacheFactory.GetDistributedCache();
            DateTime expirationTime = DateTime.Now.AddDays(10);

            Constants.DIAGNOSTIC_TRACING_STORAGE_MODE = (DiagnosticTracingStorageMode)GetAppConfig<Int16>("MDMCenter.Diagnostics.Tracing.StorageMode", 0);
            cacheManager.Set(CacheKeyContants.DIAGNOSTIC_TRACING_STORAGE_MODE, Constants.DIAGNOSTIC_TRACING_STORAGE_MODE, expirationTime);
        }

        /// <summary>
        /// Loads the storage mode. 
        /// </summary>
        public void LoadDiagnosticSystemLogLegacySourcesToSkip()
        {
            IDistributedCache cacheManager = CacheFactory.GetDistributedCache();
            DateTime expirationTime = DateTime.Now.AddDays(10);

            Constants.DIAGNOSTIC_SYSTEMLOG_LEGACYSOURCESTOSKIP = GetAppConfig<string>("MDMCenter.Diagnostics.SystemLog.LegacyTraceSourcesToSkip", "");
            cacheManager.Set(CacheKeyContants.DIAGNOSTIC_SYSTEMLOG_LEGACYSOURCESTOSKIP, Constants.DIAGNOSTIC_SYSTEMLOG_LEGACYSOURCESTOSKIP, expirationTime);
        }

        /// <summary>
        /// Loads relationship denorm processing settings into cache
        /// </summary>
        /// <param name="areSettingsModified"></param>
        public void LoadRelationshipDenormProcessingSettings(Boolean areSettingsModified = false)
        {
            EventLogHandler eventLogHandler = new EventLogHandler();
            String strRelationshipDenormProcessingSettings = GetAppConfig("MDMCenter.RelationshipDenorm.ProcessingSettings");

            if (!String.IsNullOrWhiteSpace(strRelationshipDenormProcessingSettings))
            {
                try
                {
                    RelationshipDenormProcessingSettingCollection relationshipDenormProcessingSettings = new RelationshipDenormProcessingSettingCollection(strRelationshipDenormProcessingSettings);

                    //Put into cache
                    if (relationshipDenormProcessingSettings != null)
                    {
                        DateTime expirationTime = DateTime.Now.AddDays(100);
                        String cacheKey = CacheKeyGenerator.GetRelationshipDenormProcessingSettingsCacheKey();

                        if (AppConfigurationHelper.IsDistributedCacheWithNotificationEnabled)
                        {
                            CacheSynchronizationHelper cacheSynchronizationHelper = new CacheSynchronizationHelper();
                            cacheSynchronizationHelper.NotifyLocalCacheInsert(cacheKey, expirationTime, areSettingsModified);
                        }

                        ICache cacheManager = CacheFactory.GetCache();
                        cacheManager.Set(cacheKey, relationshipDenormProcessingSettings, expirationTime);
                    }
                }
                catch (Exception ex)
                {
                    eventLogHandler.WriteErrorLog("Error occurred while inserting Relationship  in local cache. Internal error : " + ex.Message, 0);
                    ExceptionManager.ExceptionHandler exceptionHander = new ExceptionManager.ExceptionHandler(ex);
                }
            }
        }

        /// <summary>
        /// Loads track history settings for value change with source tracking module enabled
        /// </summary>
        public void LoadTrackHistoryOnValueChange()
        {
            IDistributedCache cacheManager = CacheFactory.GetDistributedCache();
            DateTime expirationTime = DateTime.Now.AddDays(10);

            Constants.TRACK_HISTORY_ONLY_ON_VALUE_CHANGE = GetAppConfig<Boolean>("MDMCenter.Entity.SourceTracking.TrackHistoryOnlyOnValueChange", true);
            cacheManager.Set(CacheKeyContants.TRACK_HISTORY_ONLY_ON_VALUE_CHANGE, Constants.TRACK_HISTORY_ONLY_ON_VALUE_CHANGE, expirationTime);
        }

        /// <summary>
        /// Gets relationship denorm processing settings
        /// </summary>
        /// <returns></returns>
        public RelationshipDenormProcessingSettingCollection GetRelationshipDenormProcessingSettings()
        {
            RelationshipDenormProcessingSettingCollection relationshipDenormProcessingSettings = null;
            EventLogHandler eventLogHandler = new EventLogHandler();

            Boolean isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;
            try
            {
                String cacheKey = CacheKeyGenerator.GetRelationshipDenormProcessingSettingsCacheKey();

                ICache cacheManager = CacheFactory.GetCache();
                relationshipDenormProcessingSettings = cacheManager.Get<RelationshipDenormProcessingSettingCollection>(cacheKey);

                if (relationshipDenormProcessingSettings == null)
                {
                    // If data is unavailable in local cache, try retrieving from DB
                    if (isTracingEnabled)
                    {
                        eventLogHandler.WriteInformationLog("Relationship denorm processing settings are unavailable is local cache. Fetching from DB..", 0);
                    }

                    String strRelationshipDenormProcessingSettings = GetAppConfig("MDMCenter.RelationshipDenorm.ProcessingSettings");

                    relationshipDenormProcessingSettings = new RelationshipDenormProcessingSettingCollection(strRelationshipDenormProcessingSettings);

                    //Put into cache
                    if (relationshipDenormProcessingSettings != null)
                    {
                        DateTime expirationTime = DateTime.Now.AddDays(100);

                        if (AppConfigurationHelper.IsDistributedCacheWithNotificationEnabled)
                        {
                            CacheSynchronizationHelper cacheSynchronizationHelper = new CacheSynchronizationHelper();
                            cacheSynchronizationHelper.NotifyLocalCacheInsert(cacheKey, expirationTime, false);
                        }

                        cacheManager.Set(cacheKey, relationshipDenormProcessingSettings, expirationTime);
                    }
                }
            }
            catch (Exception ex)
            {
                eventLogHandler.WriteErrorLog("Error occurred while retrieving relationship denorm processing settings. Internal error : " + ex.Message, 0);
                ExceptionManager.ExceptionHandler exceptionHandler = new ExceptionManager.ExceptionHandler(ex);
            }

            return relationshipDenormProcessingSettings;
        }

        /// <summary>
        /// Gets role mappings
        /// </summary>
        /// <returns></returns>
        public RoleMappingCollection GetRoleMappings()
        {
            RoleMappingCollection roleMappingCollection = null;
            EventLogHandler eventLogHandler = new EventLogHandler();

            Boolean isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;
            try
            {
                String cacheKey = CacheKeyGenerator.GetRoleMappingsCacheKey();

                ICache cacheManager = CacheFactory.GetCache();
                roleMappingCollection = cacheManager.Get<RoleMappingCollection>(cacheKey);

                if (roleMappingCollection == null)
                {
                    // If data is unavailable in local cache, try retrieving from DB
                    if (isTracingEnabled)
                    {
                        eventLogHandler.WriteInformationLog("Role mappings are unavailable is local cache. Fetching from DB..", 0);
                    }

                    roleMappingCollection = LoadRoleMappings();
                }
            }
            catch (Exception ex)
            {
                eventLogHandler.WriteErrorLog("Error occurred while retrieving role mappings. Internal error : " + ex.Message, 0);
                ExceptionManager.ExceptionHandler exceptionHandler = new ExceptionManager.ExceptionHandler(ex);
            }

            return roleMappingCollection;
        }

        /// <summary>
        /// Gets claim types mapping
        /// </summary>
        /// <returns></returns>
        public ClaimTypesMapping GetClaimTypesMapping()
        {
            ClaimTypesMapping claimTypesMapping = null;
            EventLogHandler eventLogHandler = new EventLogHandler();

            Boolean isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;
            try
            {
                String cacheKey = CacheKeyGenerator.GetClaimTypesMappingCacheKey();

                ICache cacheManager = CacheFactory.GetCache();
                claimTypesMapping = cacheManager.Get<ClaimTypesMapping>(cacheKey);

                if (claimTypesMapping == null)
                {
                    // If data is unavailable in local cache, try retrieving from DB
                    if (isTracingEnabled)
                    {
                        eventLogHandler.WriteInformationLog("Claim types mapping are unavailable is local cache. Fetching from DB..", 0);
                    }

                    claimTypesMapping = LoadClaimTypesMapping();
                }
            }
            catch (Exception ex)
            {
                eventLogHandler.WriteErrorLog("Error occurred while retrieving claim types mappings. Internal error : " + ex.Message, 0);
                ExceptionManager.ExceptionHandler exceptionHandler = new ExceptionManager.ExceptionHandler(ex);
            }

            return claimTypesMapping;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public String GetCacheConfigs()
        {
            return GetAppConfig(CacheKeyContants.MDMCENTER_CACHEOBJECT_CONFIG);
        }

        /// <summary>
        /// Clears the application configuration cache.
        /// </summary>        
        public void ClearCache()
        {
            ClearCache(false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string GetConnectionString()
        {
            return GetConnectionStringValue();
        }

        #endregion

        #region Protected Abstract Methods

        /// <summary>
        /// Returns all application configurations thats are stored in the database.
        /// </summary>
        /// <returns></returns>
        protected abstract IAppConfigCollection GetAllAppConfigsInDB();

        /// <summary>
        /// Returns the application configuration based on the specified key.
        /// </summary>
        /// <param name="optionName"></param>
        /// <returns></returns>
        protected abstract IAppConfig GetAppConfigInDB(String optionName);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected abstract String GetConnectionStringValue();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected abstract IOperationResult ProcessAppConfig(String appConfigKey, String value);

        #endregion

        #region Private Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="optionName"></param>
        /// <returns></returns>
        private String GetAppConfig(string optionName)
        {
            optionName = optionName.Trim();

            String returnValue = GetDataFromCache(optionName);
            if (returnValue == null)
            {
                if (GetAppConfigValueByKey(optionName))
                {
                    returnValue = System.Configuration.ConfigurationManager.AppSettings[optionName];
                }
                else
                {
                    IAppConfig configObj = GetAppConfigInDB(optionName);

                    if (configObj != null)
                    {
                        returnValue = configObj.Value;
                    }
                }

                if (returnValue != null)
                {
                    //SetDataInCache(optionName, returnValue, false);
                }
            }

            return returnValue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private Boolean GetAppConfigValueByKey(String key)
        {
            return System.Configuration.ConfigurationManager.AppSettings[key] == null ? false : true;
        }

        /// <summary>
        /// Loads the appsettings from configuration file.
        /// </summary>
        private void LoadAppSettings(Boolean isReloadOperation)
        {
            //Load all the AppConfigs from Config files first
            String configValue = null;
            NameValueCollection appSettings = System.Configuration.ConfigurationManager.AppSettings;

            foreach (String name in appSettings)
            {
                if (!String.IsNullOrEmpty(name))
                {
                    configValue = appSettings[name];
                    if (!String.IsNullOrEmpty(configValue))
                    {
                        SetDataInCache(name, configValue, isReloadOperation);
                    }
                }
            }
        }

        /// <summary>
        /// Loads app configs from database.
        /// </summary>
        private void LoadAppConfigsInDB(Boolean isReloadOperation)
        {
            IAppConfigCollection appConfigs = GetAllAppConfigsInDB();

            if (appConfigs != null)
            {
                ICache cacheManager = CacheFactory.GetCache();
                foreach (AppConfig appConfig in appConfigs)
                {
                    if (!String.IsNullOrEmpty(appConfig.Name) && appConfig.Value != null)
                    {
                        // Check if AppConfig is already loaded using configuration file.
                        if (cacheManager.Get(CacheKeyGenerator.GetAppConfigCacheKey(appConfig.Name)) == null)
                        {
                            SetDataInCache(appConfig.Name, appConfig.Value, isReloadOperation);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Loads claim types mappings into cache
        /// </summary>
        /// <param name="areClaimTypesMappingsModified"></param>
        private ClaimTypesMapping LoadClaimTypesMapping(Boolean areClaimTypesMappingsModified = false)
        {
            ClaimTypesMapping claimTypesMapping = null;
            String strClaimTypesMappings = GetAppConfig("MDMCenter.ClaimsBasedAuthentication.ClaimTypesMappingConfiguration");
            if (!String.IsNullOrWhiteSpace(strClaimTypesMappings))
            {
                try
                {
                    claimTypesMapping = new ClaimTypesMapping(strClaimTypesMappings);

                    //Put into cache
                    if (claimTypesMapping != null)
                    {
                        DateTime expirationTime = DateTime.Now.AddDays(100);
                        String cacheKey = CacheKeyGenerator.GetClaimTypesMappingCacheKey();

                        if (AppConfigurationHelper.IsDistributedCacheWithNotificationEnabled)
                        {
                            CacheSynchronizationHelper cacheSynchronizationHelper = new CacheSynchronizationHelper();
                            cacheSynchronizationHelper.NotifyLocalCacheInsert(cacheKey, expirationTime, areClaimTypesMappingsModified);
                        }

                        ICache cacheManager = CacheFactory.GetCache();
                        cacheManager.Set(cacheKey, claimTypesMapping, expirationTime);
                    }
                }
                catch (Exception ex)
                {
                    EventLogHandler eventLogHandler = new EventLogHandler();
                    eventLogHandler.WriteErrorLog("Error occurred while inserting Claim Types Mappings in local cache. Internal error : " + ex.Message, 0);
                    ExceptionManager.ExceptionHandler exceptionHander = new ExceptionManager.ExceptionHandler(ex);
                }
            }
            return claimTypesMapping;
        }

        /// <summary>
        /// Loads role mappings into cache
        /// </summary>
        /// <param name="areRoleMappingsModified"></param>
        private RoleMappingCollection LoadRoleMappings(Boolean areRoleMappingsModified = false)
        {
            RoleMappingCollection roleMappingCollection = null;
            String strRoleMappings = GetAppConfig("MDMCenter.ClaimsBasedAuthentication.RoleMappings");

            if (!String.IsNullOrWhiteSpace(strRoleMappings))
            {
                try
                {
                    roleMappingCollection = new RoleMappingCollection(strRoleMappings);

                    //Put into cache
                    if (roleMappingCollection != null)
                    {
                        DateTime expirationTime = DateTime.Now.AddDays(100);
                        String cacheKey = CacheKeyGenerator.GetRoleMappingsCacheKey();

                        if (AppConfigurationHelper.IsDistributedCacheWithNotificationEnabled)
                        {
                            CacheSynchronizationHelper cacheSynchronizationHelper = new CacheSynchronizationHelper();
                            cacheSynchronizationHelper.NotifyLocalCacheInsert(cacheKey, expirationTime, areRoleMappingsModified);
                        }

                        ICache cacheManager = CacheFactory.GetCache();
                        cacheManager.Set(cacheKey, roleMappingCollection, expirationTime);
                    }
                }
                catch (Exception ex)
                {
                    EventLogHandler eventLogHandler = new EventLogHandler();
                    eventLogHandler.WriteErrorLog("Error occurred while inserting Role Mappings in local cache. Internal error : " + ex.Message, 0);
                    ExceptionManager.ExceptionHandler exceptionHander = new ExceptionManager.ExceptionHandler(ex);
                }
            }
            return roleMappingCollection;
        }

        private CacheConfigurationCollection GetCacheObjectConfiguration(bool isReload)
        {
            var cacheconfigxml = GetAppConfig(CacheKeyContants.MDMCENTER_CACHEOBJECT_CONFIG);

            var col = new CacheConfigurationCollection();
            col.LoadFromXml(cacheconfigxml);

            return col;
        }

        /// <summary>
        /// Returns the data from cache for the specified cache key.
        /// </summary>
        private String GetDataFromCache(String appConfigKey)
        {
            String cacheKey = String.Empty;
            String data = null;
            try
            {
                cacheKey = CacheKeyGenerator.GetAppConfigCacheKey(appConfigKey);

                ICache cacheManager = CacheFactory.GetCache();
                data = cacheManager.Get<String>(cacheKey);
            }
            catch (Exception ex)
            {
                WriteLog(appConfigKey, TraceEventType.Error, String.Format("Error occurred while retrieving data for key {0}. Internal error : {1}",
                    cacheKey, ex.Message));
            }
            return data;
        }

        /// <summary>
        /// Inserts the data to cache for the specified cache key.
        /// The cache invalidation is performed when the configurations are reloaded (i.e. isDataUpdated is set to true). 
        /// </summary>
        private void SetDataInCache(String appConfigKey, Object data, Boolean isDataUpdated)
        {
            String cacheKey = String.Empty;
            try
            {
                DateTime expirationTime = DateTime.Now.AddDays(10);
                cacheKey = CacheKeyGenerator.GetAppConfigCacheKey(appConfigKey);

                CacheSynchronizationHelper cacheSynchronizationHelper = new CacheSynchronizationHelper();
                cacheSynchronizationHelper.NotifyLocalCacheInsert(cacheKey, expirationTime, isDataUpdated);

                if (data != null)
                {
                    ICache cacheManager = CacheFactory.GetCache();
                    cacheManager.Set(cacheKey, data, expirationTime);
                }
            }
            catch (Exception ex)
            {
                WriteLog(appConfigKey, TraceEventType.Error, String.Format("Error occurred while inserting {0} into cache. Internal error : {1}",
                    cacheKey, ex.Message));
            }
        }

        /// <summary>
        /// Clears AppConfig data from Cache.
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
                String traceConfigCacheKey = CacheKeyGenerator.GetMDMTraceConfigCacheKey();
                cacheSynchronizationHelper.NotifyLocalCacheRemoval(traceConfigCacheKey);
            }

            Collection<String> cacheKeys = cacheManager.GetAllCacheKeys();
            foreach (String cacheKey in cacheKeys)
            {
                if (cacheKey.StartsWith(CacheKeyContants.APP_CONFIG_CACHE_KEY_PREFIX))
                {
                    cacheManager.Remove(cacheKey);

                    if (notifyCacheServer)
                        cacheSynchronizationHelper.NotifyLocalCacheRemoval(cacheKey);
                }
            }
        }

        /// <summary>
        /// Writes the specified message in log based on the event type. If the appconfig request is for trace configuration, the message is written into event log as 
        /// accessing trace configuration would result in a infinite loop.
        /// </summary>
        /// <param name="appConfigKey"></param>
        /// <param name="traceEventType"></param>
        /// <param name="message"></param>
        private void WriteLog(String appConfigKey, TraceEventType traceEventType, String message)
        {
            if (appConfigKey.Equals(_keyForTraceConfig))
            {
                EventLogHandler eventLogHandler = new EventLogHandler();
                switch (traceEventType)
                {
                    case TraceEventType.Error:
                        eventLogHandler.WriteErrorLog(message, 0);
                        break;
                    case TraceEventType.Warning:
                        eventLogHandler.WriteWarningLog(message, 0);
                        break;
                    case TraceEventType.Information:
                    case TraceEventType.Verbose:
                        eventLogHandler.WriteInformationLog(message, 0);
                        break;
                }
            }
            else
            {
                MDMTraceHelper.EmitTraceEvent(traceEventType, message);
            }
        }

        #endregion

        #endregion
    }
}
