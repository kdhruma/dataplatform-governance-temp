using System;
using System.Web;

namespace MDM.Services.ServiceClientInitializers
{
    using Core;
    using ExceptionManager;
    using MDM.BusinessObjects;
    using MDM.Interfaces;
    using MDM.Utility;
    using Services;

    /// <summary>
    /// Contains methods to load application configuration by accessing the database.
    /// </summary>
    public class AppConfigProviderUsingService : AppConfigProviderBase
    {
        #region Protected Methods

        /// <summary>
        /// Returns all application configurations thats are stored in the database.
        /// </summary>
        /// <returns></returns>
        protected override IAppConfigCollection GetAllAppConfigsInDB()
        {
            IAppConfigCollection appConfigCollection = null;
            try
            {
                CallerContext context = new CallerContext(MDMCenterApplication.PIM, MDMCenterModules.Security);

                ConfigurationService configurationService = GetConfigurationService();
                appConfigCollection = configurationService.GetAllAppConfigs(context);
            }
            catch (Exception ex)
            {
                //Log exception
                ExceptionHandler exceptionHandler = new ExceptionHandler(ex);
                throw new Exception("Unable to retrieve all appConfigs", ex);
            }
            return appConfigCollection;
        }

        /// <summary>
        /// Returns the application configuration based on the specified key.
        /// </summary>
        /// <param name="configName"></param>
        /// <returns></returns>
        protected override IAppConfig GetAppConfigInDB(String configName)
        {
            IAppConfig appConfig = null;
            try
            {
                ConfigurationService configurationService = GetConfigurationService();
                appConfig = configurationService.GetAppConfigObject(configName);
            }
            catch (Exception ex)
            {
                //Log exception
                ExceptionHandler exceptionHandler = new ExceptionHandler(ex);
                throw new Exception("Unable to retrieve AppConfig. Key:" + configName, ex);
            }
            return appConfig;
        }

        /// <summary>
        /// Processes AppConfig 
        /// </summary>
        /// <param name="appConfigKey">Specifies AppConfig Key</param>
        /// <param name="value">Specifies AppConfig value</param>
        /// <returns>Operation Result</returns>
        protected override IOperationResult ProcessAppConfig(string appConfigKey, string value)
        {
            IOperationResult operationResult = null;
            
            try
            {
                ConfigurationService configurationService = GetConfigurationService();
                IAppConfig appConfig = configurationService.GetAppConfigObject(appConfigKey);

                if (appConfig != null)
                {
                    appConfig.Value = value;
                    appConfig.Action = ObjectAction.Update;

                    ICallerContext callerContext = MDMObjectFactory.GetICallerContext(MDMCenterApplication.MDMCenter, MDMCenterModules.Instrumentation);
                    operationResult = configurationService.UpdateAppConfig(appConfig, callerContext);
                }
            }
            catch (Exception ex)
            {
                //Log exception
                ExceptionHandler exceptionHandler = new ExceptionHandler(ex);
                throw new Exception("Unable to update AppConfig. Key:" + appConfigKey, ex);
            }

            return operationResult;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override string GetConnectionStringValue()
        {
            const String dataAccessAppConfigKeyName = "MDMCenter.DataAccessIdentity.Key";

            var connectionString = GetAppConfig(dataAccessAppConfigKeyName, String.Empty);

            connectionString = !String.IsNullOrWhiteSpace(connectionString) ? BasicEncryptionHelper.Decrypt(connectionString) : AppConfiguration.GetSetting("ConnectionString");

            return connectionString;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Returns an instance of the ConfigurationService.
        /// </summary>
        /// <returns></returns>
        private ConfigurationService GetConfigurationService()
        {
            ConfigurationService configurationService = null;

            if (SecurityPrincipalHelper.IsSecurityPrincipalAvailableForCurrentUser())
            {
                configurationService = new ConfigurationService();
            }
            else
            {
                String bindingConfigName = String.Format("{0}_I{1}", WCFBindingType.WSHttpBinding.ToString(), MDMWCFServiceList.ConfigurationService);
                String userName = "superuser";
                String internalSuperUserPassword = "super user with special powers - 1234567890-=+_)(*&^%$#@!";

                configurationService = new ConfigurationService(bindingConfigName, userName, internalSuperUserPassword);
            }

            return configurationService;
        }

        #endregion
    }
}