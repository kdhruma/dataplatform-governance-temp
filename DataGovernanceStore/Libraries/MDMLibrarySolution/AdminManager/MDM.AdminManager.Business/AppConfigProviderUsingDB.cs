using System;

namespace MDM.AdminManager.Business
{
    using MDM.Core;
    using MDM.Interfaces;
    using MDM.Utility;
    using MDMBO = MDM.BusinessObjects;

    /// <summary>
    /// Contains methods to load application configuration by accessing the database.
    /// </summary>
    public class AppConfigProviderUsingDB : AppConfigProviderBase
    {
        #region Protected Methods

        /// <summary>
        /// Returns all application configurations thats are stored in the database.
        /// </summary>
        /// <returns></returns>
        protected override IAppConfigCollection GetAllAppConfigsInDB()
        {
            MDMBO.CallerContext context = new MDMBO.CallerContext(MDMCenterApplication.PIM, MDMCenterModules.Security);

            AppConfigBL appConfigManager = new AppConfigBL();
            MDMBO.AppConfigCollection appConfigs = appConfigManager.GetAll(context);

            return appConfigs;
        }

        /// <summary>
        /// Returns the application configuration based on the specified key.
        /// </summary>
        /// <param name="optionName"></param>
        /// <returns></returns>
        protected override IAppConfig GetAppConfigInDB(string optionName)
        {
            MDMBO.AppConfig appConfig = null;

            try
            {
                AppConfigBL appConfigManager = new AppConfigBL();
                appConfig = appConfigManager.Get(optionName);
            }
            catch (InvalidCastException)
            {
                throw new ApplicationException(String.Format("AppConfig [{0}] key is missing", optionName));
            }

            // This would not be called cause the Stored Procedure generated code doesn't do a null check before converting. Having this in place so that when we rewrite 
            // stored procedure we can use this functionality. This is a case where the key exists but the value for the key is null in the database
            if (appConfig == null || appConfig.Value == null)
            {
                throw new ApplicationException(String.Format("AppConfig [{0}] value is missing", optionName));
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

           IAppConfig appconfig = GetAppConfigInDB(appConfigKey);

           if (appconfig != null)
           {
               appconfig.Value = value;
               appconfig.Action = ObjectAction.Update;

               MDMBO.CallerContext callerContext = new MDMBO.CallerContext(MDMCenterApplication.MDMCenter, MDMCenterModules.Instrumentation);
               operationResult = (IOperationResult)new AppConfigBL().Update((MDMBO.AppConfig)appconfig, callerContext);
           }

           return operationResult;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override string GetConnectionStringValue()
        {
            return AppConfiguration.GetSetting("ConnectionString");
        }

        #endregion

    }
}
