using System;

namespace MDM.DataProviderInterfaces
{
    using BusinessObjects;
    using BusinessObjects.Diagnostics;
    using Interfaces;

    /// <summary>
    /// Represents an interface, which provides methods to manage application configurations. 
    /// </summary>
    public interface IAppConfigProvider
    {
        /// <summary>
        /// Returns the application configuration for the specified configName.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        T GetAppConfig<T>(String key, T defaultValue);

        /// <summary>
        /// Loads all the application configurations into cache.
        /// </summary>
        /// <param name="isReloadOperation"></param>
        void LoadAllAppConfigs(Boolean isReloadOperation);

        /// <summary>
        /// Loads the application configurations that are related to the transaction settings into cache. 
        /// </summary>
        void LoadTransactionSettings();

        /// <summary>
        /// Loads the tracing profile
        /// </summary>
        void LoadTracingProfile();

        /// <summary>
        /// Process TracingProfile value
        /// </summary>
        /// <param name="tracingProfile">Specifies Tracing Profile</param>
        /// <returns>Operation Result</returns>
        IOperationResult ProcessTracingProfile(TracingProfile tracingProfile);

        /// <summary>
        /// Loads the configuration for application level trace settings into cache. 
        /// </summary>
        void LoadApplicationLevelTracingSetting();

        /// <summary>
        /// Loads relationship denorm processing settings into cache
        /// </summary>
        /// <param name="areSettingsModified"></param>
        void LoadRelationshipDenormProcessingSettings(Boolean areSettingsModified);

        /// <summary>
        /// Gets relationship denorm processing settings
        /// </summary>
        RelationshipDenormProcessingSettingCollection GetRelationshipDenormProcessingSettings();

        /// <summary>
        /// Gets role mappings
        /// </summary>
        RoleMappingCollection GetRoleMappings();

        /// <summary>
        /// Gets claim types mapping
        /// </summary>
        ClaimTypesMapping GetClaimTypesMapping();

        /// <summary>
        /// Reloads the application configuration for the specified configuration key into cache. 
        /// </summary>
        /// <param name="name"></param>
        void ReloadAppConfig(String name);        

        /// <summary>
        /// Clears the application configuration cache.
        /// </summary>
        void ClearCache();

        /// <summary>
        /// 
        /// </summary>
        String GetConnectionString();
    }
}
