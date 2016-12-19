using System;
using System.Diagnostics;

namespace MDM.Utility
{
    using Core;
    using BusinessObjects;
    using BusinessObjects.Diagnostics;
    using Interfaces;
    using DataProviderInterfaces;

    /// <summary>
    /// This is helper class for Application Configurations.
    /// </summary>
    public class AppConfigurationHelper
    {
        #region Fields

        /// <summary>
        /// 
        /// </summary>
        private static String _connectionString = String.Empty;

        /// <summary>
        /// 
        /// </summary>
        private static String _storedProcedureSuffix = null;

        /// <summary>
        /// Field specifies whether the Distributed Cache With Notification is Enabled.
        /// </summary>
        private static Boolean _isDistributedCacheWithNotificationEnabled = true;

        /// <summary>
        /// Field holds the instance of the AppConfig Provider.
        /// </summary>
        private static IAppConfigProvider _appConfigProvider = null;

        #endregion

        #region Constructors

        #endregion

        #region Properties

        /// <summary>
        /// <para>Gets the DB connection string from Application Configuration file (web.config).</para>
        /// </summary>
        public static String ConnectionString
        {
            get
            {
                if (_connectionString.Equals(String.Empty))
                {
                    _connectionString = GetConnectionString();
                }

                return _connectionString;
            }
        }

        /// <summary>
        /// <para>Gets the DB connection string from Application Configuration file (web.config).</para>
        /// </summary>
        public static String StoredProcedureSuffix
        {
            get
            {
                if (_storedProcedureSuffix == null)
                {
                    try
                    {
                        _storedProcedureSuffix = AppConfiguration.GetSetting("MDMCenter.BackwardCompatibility.DBObjectRenames.Suffix");

                        if(_storedProcedureSuffix == null)
                            _storedProcedureSuffix = String.Empty;
                        else if (_storedProcedureSuffix.Equals("[NONE]"))
                            _storedProcedureSuffix = String.Empty;
                    }
                    catch { }
                }
                return _storedProcedureSuffix;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static Boolean IsDistributedCacheWithNotificationEnabled
        {
            get
            {
                return _isDistributedCacheWithNotificationEnabled;
            }
        }

        #endregion

        #region Methods

        #region Public Methods

        /// <summary>
        /// <para>Gets the Absolute path of application with AppSettings value for the given key appended.</para>
        /// </summary>
        /// <param name="key">The key in appSettings node.</param>
        /// <returns>String containing absolute path of application with AppSettings value for the given key appended</returns>
        public static string GetSettingAbsolutePath(string key)
        {

            String returnValue = String.Empty;
            String appSettingValue = String.Empty;

            //get the value
            appSettingValue = AppConfiguration.GetSetting(key);

            //since the values would be entered in relative path keeping
            //the calling application (website or winapp)in mind
            //the path conversion can be done here.

            returnValue = string.Format("{0}\\{1}", System.AppDomain.CurrentDomain.BaseDirectory, appSettingValue);

            return returnValue;

        }

        #endregion

        #region Private Methods

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private static String GetConnectionString()
        {
            return _appConfigProvider.GetConnectionString();
        }

        #endregion

        #region AppConfig Methods

        /// <summary>
        /// Sets the instance of IAppConfigProvider.
        /// </summary>
        /// <param name="appConfigProvider">Specifies an instance of the application configuration provider</param>
        /// <param name="loadAllAppConfigs">Specifies whether all application configurations should be loaded during initialization</param>
        public static void InitializeAppConfig(IAppConfigProvider appConfigProvider, Boolean loadAllAppConfigs = true)
        {
            if (_appConfigProvider == null)
            {
                // Initialize IAppConfigProvider.
                _appConfigProvider = appConfigProvider;

                try
                {
                    _isDistributedCacheWithNotificationEnabled = _appConfigProvider.GetAppConfig("MDMCenter.DistributedCacheWithNotification.Enabled", true);

                    _appConfigProvider.LoadTracingProfile();                  
                }
                catch
                {
                    //Ignore
                }

                //Load application level tracing settings
                LoadApplicationLevelTracingSetting();

                if (loadAllAppConfigs)
                {
                    //Load all the AppConfigs
                    LoadAllAppConfigs();
                }
                
                //Load transaction settings.
                LoadTransactionSettings();
            }
        }

        /// <summary>
        /// Returns the application configuration for the specified configName.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="configName"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static T GetAppConfig<T>(String configName, T defaultValue = default(T))
        {
            ValidateAppConfigProvider();

            return _appConfigProvider.GetAppConfig<T>(configName, defaultValue);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="configName"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static T GetAppConfigEnum<T>(String configName, T defaultValue) where T : struct
        {
            T returnVal = defaultValue;

            String strValue = GetAppConfig(configName, String.Empty);

            if (!String.IsNullOrWhiteSpace(strValue))
            {
                Enum.TryParse<T>(strValue, true, out returnVal);
            }

            return returnVal;
        }

        /// <summary>
        /// Loads the application configurations that are related to the transaction settings into cache. 
        /// </summary>
        public static void LoadTransactionSettings()
        {
            ValidateAppConfigProvider();

            _appConfigProvider.LoadTransactionSettings();
        }

        /// <summary>
        /// Loads the configuration for application level trace settings into cache. 
        /// </summary>
        public static void LoadApplicationLevelTracingSetting()
        {
            ValidateAppConfigProvider();

            _appConfigProvider.LoadApplicationLevelTracingSetting();
        }

        /// <summary>
        /// Gets relationship denorm processing settings
        /// </summary>
        /// <returns></returns>
        public static RelationshipDenormProcessingSettingCollection GetRelationshipDenormProcessingSettings()
        {
            ValidateAppConfigProvider();

            RelationshipDenormProcessingSettingCollection relationshipDenormProcessingSettings = _appConfigProvider.GetRelationshipDenormProcessingSettings();

            return relationshipDenormProcessingSettings;
        }

        /// <summary>
        /// Gets role mappings
        /// </summary>
        /// <returns></returns>
        public static RoleMappingCollection GetRoleMappings()
        {
            ValidateAppConfigProvider();

            RoleMappingCollection roleMappingCollection = _appConfigProvider.GetRoleMappings();

            return roleMappingCollection;
        }

        /// <summary>
        /// Gets claim types mapping
        /// </summary>
        /// <returns></returns>
        public static ClaimTypesMapping GetClaimTypesMapping()
        {
            ValidateAppConfigProvider();

            ClaimTypesMapping claimTypesMapping = _appConfigProvider.GetClaimTypesMapping();

            return claimTypesMapping;
        }

        /// <summary>
        /// Reloads the application configuration for the specified configuration key into cache. 
        /// </summary>
        /// <param name="name"></param>
        public static void ReloadAppConfig(String name)
        {
            ValidateAppConfigProvider();

            _appConfigProvider.ReloadAppConfig(name);
        }

        /// <summary>
        /// Loads all the application configurations into cache.
        /// </summary>
        /// <param name="isReloadOperation"></param>
        public static void LoadAllAppConfigs(Boolean isReloadOperation = false)
        {
            ValidateAppConfigProvider();

            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Loading app configs keys...");

            _appConfigProvider.LoadAllAppConfigs(isReloadOperation);

            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Loaded app configs keys");
        }

        /// <summary>
        /// Processes Tracing Profile
        /// </summary>
        /// <param name="tracingProfile">Specifies Tracing Profile Object</param>
        /// <returns>Operation Result</returns>
        public static IOperationResult ProcessTracingProfile(TracingProfile tracingProfile)
        {
            ValidateAppConfigProvider();

            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Processing Tracing Profile...");

            IOperationResult iOperationResult = _appConfigProvider.ProcessTracingProfile(tracingProfile);

            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Process Tracing Profile");

            return iOperationResult;
        }

        /// <summary>
        /// Clears the application configuration cache.
        /// </summary>
        public static void ClearCache()
        {
            ValidateAppConfigProvider();

            _appConfigProvider.ClearCache();
        }

        /// <summary>
        /// 
        /// </summary>
        private static void ValidateAppConfigProvider()
        {
            if (_appConfigProvider == null)
                throw new ApplicationException("AppConfigProvider is not initialized");
        }

        #endregion

        #endregion

    }
}
