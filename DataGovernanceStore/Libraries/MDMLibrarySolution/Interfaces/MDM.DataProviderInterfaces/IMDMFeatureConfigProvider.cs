using System;

namespace MDM.DataProviderInterfaces
{
    using MDM.BusinessObjects;
    using MDM.Core;

    /// <summary>
    /// Represents an interface, which provides methods to manage MDM feature configurations. 
    /// </summary>
    public interface IMDMFeatureConfigProvider
    {      

        /// <summary>
        /// Returns the MDM Feature configuration by application, modulePath and version
        /// </summary>        
        /// <param name="application">Application under MDMCenter.</param>
        /// <param name="modulePath">Module Path of MDMCenter Application</param>
        /// <param name="version">Version of feature.</param>         
        /// <returns></returns>
        MDMFeatureConfig GetMDMFeatureConfig(MDMCenterApplication application, String modulePath, String version);

        /// <summary>
        /// Function to load all available MDM Feature configuration, from config files and database
        /// </summary>
        /// <param name="isReloadOperation">Specifies whether the function is invoked for a reloading of MDM feature configs.</param>
        void LoadAllMDMFeatureConfigs(Boolean isReloadOperation);

        /// <summary>
        /// Reloads the MDM Feature configuration for the specified configuration key into cache. 
        /// </summary>
        /// <param name="application">Application under MDMCenter.</param>
        /// <param name="modulePath">Module Path of MDMCenter Application</param>
        /// <param name="version">Version of feature.</param>       
        void ReloadMDMFeatureConfig(MDMCenterApplication application, String modulePath, String version);        

        /// <summary>
        /// Clears the MDM Feature configuration cache.
        /// </summary>
        void ClearCache();
        
    }
}
