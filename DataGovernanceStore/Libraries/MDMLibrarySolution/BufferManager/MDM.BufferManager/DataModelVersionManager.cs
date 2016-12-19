using System;
using MDM.Interfaces;
using MDM.Services;

namespace MDM.BufferManager
{
    using MDM.BusinessObjects;
    using MDM.Core;
    using MDM.Utility;

    /// <summary>
    /// Manages current data model version numbers as appconfig keys
    /// </summary>
    public class DataModelVersionManager
    {
        #region Methods

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Boolean UpdateAttributeModelsVersionNumber()
        {
            String versionNumberAppConfigKeyName = "MDMCenter.DataModelManager.AttributeModels.CurrentVersionNumber";
            return UpdateVersionNumber(versionNumberAppConfigKeyName);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Boolean UpdateLookupModelsVersionNumber()
        {
            String versionNumberAppConfigKeyName = "MDMCenter.DataModelManager.LookupModels.CurrentVersionNumber";
            return UpdateVersionNumber(versionNumberAppConfigKeyName);
        }

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <returns></returns>
        //public Boolean UpdateHierarchiesVersionNumber()
        //{
        //    String versionNumberAppConfigKeyName = "MDMCenter.DataModelManager.Hierarchies.CurrentVersionNumber";
        //    return UpdateVersionNumber(versionNumberAppConfigKeyName);
        //}

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <returns></returns>
        //public Boolean UpdateContainersVersionNumber()
        //{
        //    String versionNumberAppConfigKeyName = "MDMCenter.DataModelManager.Containers.CurrentVersionNumber";
        //    return UpdateVersionNumber(versionNumberAppConfigKeyName);
        //}

        #region Private Methods
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private Boolean UpdateVersionNumber(String versionNumberAppConfigKeyName)
        {
            Boolean returnVal = true;

            Int32 currentVersionNumber = 1;
            Int32 newVersionNumber = 1;
            CallerContext callerContext = new CallerContext(MDMCenterApplication.PIM, MDMCenterModules.Modeling);

            var configurationService = new ConfigurationService();

            var appConfigObj = (AppConfig)configurationService.GetAppConfigObject(versionNumberAppConfigKeyName);

            if (appConfigObj != null)
            {
                string strValue = appConfigObj.Value != null ? appConfigObj.Value.ToString() : String.Empty;

                if (!String.IsNullOrWhiteSpace(strValue))
                {
                    currentVersionNumber = ValueTypeHelper.Int32TryParse(strValue, currentVersionNumber);
                    newVersionNumber = currentVersionNumber + 1;
                }
            }

            if (!currentVersionNumber.Equals(newVersionNumber) && appConfigObj != null)
            {
                appConfigObj.Value = newVersionNumber.ToString();
                appConfigObj.Action = ObjectAction.Update;

                configurationService.ProcessAppConfigs(new AppConfigCollection() { appConfigObj }, callerContext);
                AppConfigurationHelper.ReloadAppConfig(versionNumberAppConfigKeyName);
            }
            
            return returnVal;
        }

        #endregion

        #endregion
    }
}
