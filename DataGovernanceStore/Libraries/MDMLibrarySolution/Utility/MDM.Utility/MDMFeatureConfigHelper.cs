using System;
using System.Diagnostics;

namespace MDM.Utility
{
    using Core;
    using BusinessObjects;
    using DataProviderInterfaces;
    
    /// <summary>
    /// This is helper class for MDM Feature Configurations.
    /// </summary>
    public class MDMFeatureConfigHelper
    {
        #region Fields

        /// <summary>
        /// Field holds the instance of the MDMFeatureConfig Provider.
        /// </summary>
        private static IMDMFeatureConfigProvider _mdmFeatureConfigProvider = null;       

        #endregion         

        #region Methods

        #region Public Methods

        /// <summary>
        /// Sets the instance of IMDMFeatureConfigProvider.
        /// </summary>
        /// <param name="mdmFeatureConfigProvider">The MDM Feature configurations provider.</param>
        /// <param name="loadAllMDMFeatureConfigs">Specifies whether all feature configurations should be loaded during initialization</param>
        public static void InitializeMDMFeatureConfig(IMDMFeatureConfigProvider mdmFeatureConfigProvider, Boolean loadAllMDMFeatureConfigs = true)
        {
            if (_mdmFeatureConfigProvider == null)
            {
                // Initialize IMDMFeatureConfigProvider.
                _mdmFeatureConfigProvider = mdmFeatureConfigProvider;

                if (loadAllMDMFeatureConfigs)
                {
                    //Load all the MDM Feature configurations
                    LoadAllMDMFeatureConfigs();
                }
            }
        }

        /// <summary>
        /// Gets MDM Feature configurations by application, module name and version
        /// </summary>
        /// <param name="application">Indicates the Application for requested feature config</param>
        /// <param name="moduleName">Indicates module short name for requested feature config</param>             
        /// <param name="version">Indicates the version  for requested feature config</param>   
        /// <returns>Returns MDM Feature configurations based on application, module name and version</returns>
        public static MDMFeatureConfig GetMDMFeatureConfig(MDMCenterApplication application, String moduleName, String version)
        {
            ValidateMDMFeatureConfigProvider();

            MDMFeatureConfig mdmFeatureConfig = _mdmFeatureConfigProvider.GetMDMFeatureConfig(application, moduleName, version);

            if (mdmFeatureConfig != null && Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, mdmFeatureConfig.ToXml(), MDMTraceSource.Configuration);
            }

            return mdmFeatureConfig;
        }

        /// <summary>
        /// Gets MDM feature is enabled or not by application, module name and version
        /// </summary>
        /// <param name="application">Indicates the Application for requested feature config</param>
        /// <param name="moduleName">Indicates module short name for requested feature config</param>             
        /// <param name="version">Indicates the version  for requested feature config</param>  
        /// <param name="defaultValue">Indicates the default value  for requested feature config</param>
        /// <returns>Returns feature is enabled or not if Feature Config is not null otherwise default value</returns>
        public static Boolean IsMDMFeatureEnabled(MDMCenterApplication application, String moduleName, String version, Boolean defaultValue = false)
        {
            ValidateMDMFeatureConfigProvider();

            Boolean isEnabled = defaultValue;
            MDMFeatureConfig mdmFeatureConfig = _mdmFeatureConfigProvider.GetMDMFeatureConfig(application, moduleName, version);

            if (mdmFeatureConfig != null)
            {
                isEnabled = mdmFeatureConfig.IsEnabled;

                if (Constants.TRACING_ENABLED)
                {
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, mdmFeatureConfig.ToXml(), MDMTraceSource.Configuration);
                }
            }

            return isEnabled;
        }

        /// <summary>
        /// Reloads the MDM Feature configuration for the specified configuration key into cache. 
        /// </summary>
        /// <param name="application">Indicates the Application for requested feature config</param>
        /// <param name="moduleName">Indicates module short name for requested feature config</param>             
        /// <param name="version">Indicates the version  for requested feature config</param>   
        public static void ReloadMDMFeatureConfig(MDMCenterApplication application, String moduleName, String version)
        {
            ValidateMDMFeatureConfigProvider();

            _mdmFeatureConfigProvider.ReloadMDMFeatureConfig(application, moduleName, version);
        }

        /// <summary>
        /// Function to load all available MDM Feature configuration, from config files and database
        /// </summary>
        /// <param name="isReloadOperation">Specifies whether the function is invoked for a reloading of MDM feature configs.</param>
        public static void LoadAllMDMFeatureConfigs(Boolean isReloadOperation = false)
        {
            ValidateMDMFeatureConfigProvider();

            if (Constants.PERFORMANCE_TRACING_ENABLED || Constants.TRACING_ENABLED)
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Loading MDM feature configs keys...");

            _mdmFeatureConfigProvider.LoadAllMDMFeatureConfigs(isReloadOperation);

            if (Constants.PERFORMANCE_TRACING_ENABLED || Constants.TRACING_ENABLED)
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Loaded MDM feature configs keys");
        }

        /// <summary>
        /// Clears the MDM Feature configuration cache.
        /// </summary> 
        public static void ClearCache()
        {
            ValidateMDMFeatureConfigProvider();

            _mdmFeatureConfigProvider.ClearCache();
        }       

        #endregion

        #region Private Methods

        /// <summary>
        /// Validate MDM Feature config provider.
        /// </summary>
        private static void ValidateMDMFeatureConfigProvider()
        {
            if (_mdmFeatureConfigProvider == null)
                throw new ApplicationException("MDMFeature configurations Provider is not initialized");
        }      

        #endregion        

        #endregion

    }
}
