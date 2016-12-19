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
    /// Contains methods to load MDM Feature configuration by accessing the database.
    /// </summary>
    public class MDMFeatureConfigProviderUsingService : MDMFeatureConfigProviderBase
    {
        #region Protected Methods

        /// <summary>
        /// Returns all MDM Feature configurations thats are stored in the database.
        /// </summary>
        /// <returns></returns>
        protected override IMDMFeatureConfigCollection GetAllMDMFeatureConfigsInDB()
        {
            IMDMFeatureConfigCollection mdmFeatureConfigCollection = null;
            try
            {
                CallerContext callerContext = new CallerContext(MDMCenterApplication.MDMCenter, MDMCenterModules.Security);

                ConfigurationService configurationService = GetConfigurationService();
                mdmFeatureConfigCollection = configurationService.GetMDMFeatureConfigCollection(callerContext);
            }
            catch (Exception ex)
            {
                //Log exception
                ExceptionHandler exceptionHandler = new ExceptionHandler(ex);
                throw new Exception("Unable to retrieve all MDM Feature Configs", ex);
            }
            return mdmFeatureConfigCollection;
        }

        /// <summary>
        /// Gets MDMFeature Config by application, module name, version
        /// </summary>
        /// <param name="application">Indicates the Application for requested feature config</param>
        /// <param name="moduleName">Indicates module short name for requested feature config</param>             
        /// <param name="version">Indicates the version  for requested feature config</param>     
        /// <returns>Returns MDMFeature Config based on application, module name and version</returns>
        protected override IMDMFeatureConfig GetMDMFeatureConfigInDB(MDMCenterApplication application, String moduleName, String version)
        {
            IMDMFeatureConfig mdmFeatureConfig = null;
            try
            {
                CallerContext callerContext = new CallerContext(MDMCenterApplication.MDMCenter, MDMCenterModules.Security);
                ConfigurationService configurationService = GetConfigurationService();
                mdmFeatureConfig = configurationService.GetFeatureConfig(application, moduleName, version, callerContext);
            }
            catch (Exception ex)
            {
                //Log exception
                ExceptionHandler exceptionHandler = new ExceptionHandler(ex);
                throw new Exception(String.Format("Unable to retrieve MDMFeature Config: Application={0} , Module name={1} and Version={2}", application, moduleName, version), ex);
            }
            return mdmFeatureConfig;
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