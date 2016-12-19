using System;
using System.Diagnostics;

namespace MDM.AdminManager.Business
{
    using MDM.Core;
    using MDM.Interfaces;
    using MDM.Utility;
    using MDM.BusinessObjects;    

    /// <summary>
    /// Contains methods to load MDM Feature configuration by accessing the database.
    /// </summary>
    public class MDMFeatureConfigProviderUsingDB : MDMFeatureConfigProviderBase
    {
        #region Protected Methods

        /// <summary>
        /// Returns all MDM Feature configurations thats are stored in the database.
        /// </summary>
        /// <returns></returns>
        protected override IMDMFeatureConfigCollection GetAllMDMFeatureConfigsInDB()
        {
            MDMFeatureConfigCollection mdmFeatureConfigs = null;
            CallerContext callerContext = new CallerContext(MDMCenterApplication.PIM, MDMCenterModules.Security);
            MDMFeatureConfigBL mdmFeatureConfigManager = new MDMFeatureConfigBL();

            mdmFeatureConfigs = mdmFeatureConfigManager.GetMDMFeatureConfigCollection(callerContext);

            if (mdmFeatureConfigs == null || mdmFeatureConfigs.Count < 1)
            {
                String message = "Unable to retrieve all MDM Feature Configs";
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, message, MDMTraceSource.Configuration);
            }

            return mdmFeatureConfigs;
        }
        
        /// <summary>
        /// Gets MDMFeature Config by application, module name and version
        /// </summary>
        /// <param name="application">Indicates the Application for requested feature config</param>
        /// <param name="moduleName">Indicates module short name for requested feature config</param>             
        /// <param name="version">Indicates the version  for requested feature config</param>   
        /// <returns>Returns MDMFeature Config based on application, module name and version</returns>
        protected override IMDMFeatureConfig GetMDMFeatureConfigInDB(MDMCenterApplication application, String moduleName, String version)
        {
            MDMFeatureConfig mdmFeatureConfig = null;
            CallerContext callerContext = new CallerContext(MDMCenterApplication.PIM, MDMCenterModules.Security);
            MDMFeatureConfigBL mdmFeatureConfigManager = new MDMFeatureConfigBL();

            mdmFeatureConfig = mdmFeatureConfigManager.GetFeatureConfig(application, moduleName, version, callerContext);

            if (mdmFeatureConfig == null)
            {
                String message = String.Format("MDMFeatureConfig: Application={0} , Module name={1} and Version={2} is missing", application, moduleName, version);
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, message, MDMTraceSource.Configuration);
            }

            return mdmFeatureConfig;
        }      

        #endregion

    }
}
