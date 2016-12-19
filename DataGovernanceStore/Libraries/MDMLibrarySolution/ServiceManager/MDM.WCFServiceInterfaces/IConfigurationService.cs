using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Collections.ObjectModel;

namespace MDM.WCFServiceInterfaces
{
    using MDM.BusinessObjects;
    using MDM.Core;
    /// <summary>
    /// Defines operation contracts for MDM Configuration related operations
    /// </summary>
    [ServiceContract(Namespace = "http://wcfservices.riversand.com")]
    public interface IConfigurationService
    {
        #region Application Config Contracts

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        Dictionary<String, String> GetApplicationConfigurations(Int32 EventSourceId, Int32 EventSubscriberId, Int32 SecurityRoleId, Int32 SecurityUserId, Int32 OrgId, Int32 CatalogId, Int64 CategoryId, Int64 CNodeId, Int32 AttributeId, Int32 NodeTypeId, Int32 RelationshipTypeId, Int32 LocalId, Int32 ApplicationConfigId);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        OperationResultCollection DeleteApplicationConfigurations(ApplicationConfigurationItemCollection applicationConfigurationItems, CallerContext callerContext);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        OperationResultCollection CreateApplicationConfigurations(ApplicationConfigurationItemCollection applicationConfigurationItems, CallerContext callerContext);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        OperationResultCollection UpdateApplicationConfigurations(ApplicationConfigurationItemCollection applicationConfigurationItems, CallerContext callerContext);

        #endregion

        #region AppConfig Get

        /// <summary>
        /// Gets app config value for the requested key
        /// </summary>
        /// <param name="appConfigKey">AppConfig key</param>
        /// <returns>Value of the app config key</returns>
        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        String GetAppConfig(String appConfigKey);

        /// <summary>
        /// Gets app config value for the requested key
        /// </summary>
        /// <param name="callerContext">Context which called the application</param>
        /// <param name="appConfigKey">AppConfig key</param>
        /// <returns>Value of the app config object</returns>
        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        AppConfig GetAppConfigObject(String appConfigKey);

        /// <summary>
        /// Get All AppConfigs from the system
        /// </summary>
        /// <param name="callerContext">Context which called the application</param>
        /// <returns>Collection of AppConfigs</returns>
        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        AppConfigCollection GetAllAppConfigs(CallerContext callerContext);

        #endregion AppConfig Get

        #region AppConfig CUD

        /// <summary>
        /// Create new AppConfig
        /// </summary>
        /// <param name="appConfig">Represent AppConfig Object</param>
        /// <param name="callerContext">Context of application making call for this API</param>
        /// <returns>OperationResult of AppConfig Creation</returns>
        /// <exception cref="ArgumentNullException">If AppConfig Object is Null</exception>
        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        OperationResult CreateAppConfig(AppConfig appConfig, CallerContext callerContext);

        /// <summary>
        /// Update AppConfig
        /// </summary>
        /// <param name="appConfig">Represent AppConfig Object</param>
        /// <param name="callerContext">Context of application making call for this API</param>
        /// <returns>OperationResult of AppConfig Updating</returns>
        /// <exception cref="ArgumentNullException">If AppConfig Object is Null</exception>
        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        OperationResult UpdateAppConfig(AppConfig appConfig, CallerContext callerContext);

        /// <summary>
        /// Delete AppConfig
        /// </summary>
        /// <param name="appConfig">Represent AppConfig Object</param>
        /// <param name="callerContext">Context of application making call for this API</param>
        /// <returns>OperationResult of AppConfig Updating</returns>
        /// <exception cref="ArgumentNullException">If AppConfig Object is Null</exception>
        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        OperationResult DeleteAppConfig(AppConfig appConfig, CallerContext callerContext);

        /// <summary>
        /// Process AppConfigs
        /// </summary>
        /// <param name="appConfigs">Represent AppConfig Object collection to process</param>
        /// <param name="callerContext">Context of application making call for this API</param>
        /// <returns>OperationResult of AppConfigs process</returns>
        /// <exception cref="ArgumentNullException">If AppConfigs Object is Null</exception>
        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        OperationResultCollection ProcessAppConfigs(AppConfigCollection appConfigs, CallerContext callerContext);

        /// <summary>
        /// Process User Configs
        /// </summary>
        ///  <param name="userConfig">Represent UserConfig Object collection to process</param>
        /// <returns>The process status</returns>
        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        Int32 ProcessUserConfig(UserConfig userConfig);
        #endregion AppConfig CUD
        
        #region Application Context Get

        /// <summary>
        /// Get all application contexts
        /// </summary>
        /// <param name="callerContext">Indicates the name of Application and Module which invoked the API</param>
        /// <returns>ApplicationContext Collection</returns>
        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        ApplicationContextCollection GetAllApplicationContexts(CallerContext callerContext);

        /// <summary>
        /// Get application context by Id
        /// </summary>
        /// <param name="applicationContextId">Indicates the application context Id</param>
        /// <param name="callerContext">Indicates the name of Application and Module which invoked the API</param>
        /// <returns>ApplicationContext</returns>
        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        ApplicationContext GetApplicationContextById(Int32 applicationContextId, CallerContext callerContext);

        #endregion Application Context Get

        #region Application Context CUD

        /// <summary>
        /// Create application context
        /// </summary>
        /// <param name="applicationContext">Application context to create/param>
        /// <param name="callerContext">Context which called the application</param>
        /// <returns>Result of the operation</returns>
        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        OperationResult CreateApplicationContext(ApplicationContext applicationContext, CallerContext callerContext);

        /// <summary>
        /// Update application context
        /// </summary>
        /// <param name="applicationContext">Application context to Update/param>
        /// <param name="callerContext">Context which called the application</param>
        /// <returns>Result of the operation</returns>
        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        OperationResult UpdateApplicationContext(ApplicationContext applicationContext, CallerContext callerContext);

        /// <summary>
        /// Delete application context
        /// </summary>
        /// <param name="applicationContext">Application context to delete/param>
        /// <param name="callerContext">Context which called the application</param>
        /// <returns>Result of the operation</returns>
        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        OperationResult DeleteApplicationContext(ApplicationContext applicationContext, CallerContext callerContext);

        /// <summary>
        /// Process (Create , update, delete) application context
        /// </summary>
        /// <param name="applicationContexts">Application context collection to process/param>
        /// <param name="callerContext">Context which called the application</param>
        /// <returns>Result of the operation</returns>
        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        OperationResultCollection ProcessApplicationContexts(ApplicationContextCollection applicationContexts, CallerContext callerContext);

        #endregion Application Context CUD

        #region Locale Message Contracts

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        LocaleMessageCollection GetLocaleMessages(LocaleEnum locale, Collection<String> messageCodeList, Boolean loadLatest, CallerContext callerContext);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        OperationResult ProcessLocaleMessage(LocaleMessage localeMessage, CallerContext callerContext);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        OperationResult ProcessLocaleMessages(LocaleMessageCollection localeMessages, CallerContext callerContext);

        #endregion

        #region MDMFeatureConfig Get

        /// <summary>
        /// Get All MDM Feature Config from the system
        /// </summary>
        /// <param name="callerContext">Context which called the application</param>
        /// <returns>Collection of MDM Feature Config</returns>
        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        MDMFeatureConfigCollection GetMDMFeatureConfigCollection(CallerContext CallerContext);

        /// <summary>
        /// Get MDM Feature Config from the system
        /// </summary>
        /// <param name="application">Indicates the Application for requested feature config</param>
        /// <param name="moduleName">Indicates module short name for requested feature config</param>             
        /// <param name="version">Indicates the version  for requested feature config</param>   
        /// <param name="callerContext">context indicates the properties of the caller who called the API like program name,Module</param>
        /// <returns>Returns MDM Feature Config</returns>
        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        MDMFeatureConfig GetFeatureConfig(MDMCenterApplication application, String moduleName, String version, CallerContext callerContext);

        #endregion MDMFeatureConfig Get
    }
}
