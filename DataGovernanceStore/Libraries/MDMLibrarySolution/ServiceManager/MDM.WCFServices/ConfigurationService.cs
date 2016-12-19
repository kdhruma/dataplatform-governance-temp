using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.ServiceModel;

namespace MDM.WCFServices
{
    using MDM.AdminManager.Business;
    using MDM.ApplicationServiceManager.Business;
    using MDM.BusinessObjects;
    using MDM.ConfigurationManager.Business;
    using MDM.ConfigurationPrimitivesManager.Business;
    using MDM.Core;
    using MDM.MessageManager.Business;
    using MDM.Utility;
    using MDM.WCFServiceInterfaces;

    [DiagnosticActivity]
    [ServiceBehavior(Namespace = "http://wcfservices.riversand.com", InstanceContextMode = InstanceContextMode.PerCall)]
    public class ConfigurationService : MDMWCFBase, IConfigurationService
    {
        #region Constructors

        public ConfigurationService()
            : base(true)
        {

        }

        public ConfigurationService(Boolean loadSecurityPrincipal)
            : base(loadSecurityPrincipal)
        {

        }

        #endregion

        #region Application Config Contracts

        /// <summary>
        /// Gets application configurations for the requested context parameters
        /// </summary>
        /// <param name="eventSourceId">Indicates an Id of the event source</param>
        /// <param name="eventSubscriberId">Indicates an Id of the event subscriber</param>
        /// <param name="securityRoleId">Indicates an id of a security role</param>
        /// <param name="securityUserId">Indicates an id of a security user</param>
        /// <param name="orgId">Indicates an Id of an organization</param>
        /// <param name="catalogId">Indicates an Id of a catalog</param>
        /// <param name="categoryId">Indicates an Id of a category</param>
        /// <param name="cNodeId">Indicates an Id of a CNode</param>
        /// <param name="attributeId">Indicates an Id of an attribute</param>
        /// <param name="nodeTypeId">Indicates an Id of a node type</param>
        /// <param name="relationshipTypeId">Indicates an Id of a relationship type</param>
        /// <param name="localeId">Indicates an Id of a locale</param>
        /// <param name="applicationConfigId">Indicates application configuration Id</param>
        /// <returns>Object containing all the configurations for the requested context in the key-value pair</returns>
        public Dictionary<String, String> GetApplicationConfigurations(Int32 eventSourceId, Int32 eventSubscriberId, Int32 securityRoleId, Int32 securityUserId, Int32 orgId, Int32 catalogId, Int64 categoryId, Int64 cNodeId, Int32 attributeId, Int32 nodeTypeId, Int32 relationshipTypeId, Int32 localId, Int32 applicationConfigId)
        {
            Dictionary<String, String> configurationXmlDictionary = null;

            try
            {
                ApplicationConfigurationBL applicationConfigurationBL = new ApplicationConfigurationBL();
                configurationXmlDictionary = applicationConfigurationBL.GetApplicationConfigurations(new RS.MDM.Configuration.ApplicationConfiguration(eventSourceId, eventSubscriberId, securityRoleId, securityUserId, orgId, catalogId, categoryId, cNodeId, attributeId, nodeTypeId, relationshipTypeId, localId, applicationConfigId));
            }
            catch (Exception ex)
            {
                this.LogException(ex);
                throw base.WrapException(ex);
            }

            return configurationXmlDictionary;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="applicationConfigurationItems"></param>
        /// <param name="callerContext"></param>
        /// <returns></returns>
        public OperationResultCollection DeleteApplicationConfigurations(ApplicationConfigurationItemCollection applicationConfigurationItems, CallerContext callerContext)
        {
            OperationResultCollection result = new OperationResultCollection();
            Boolean isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;
            if (isTracingEnabled)
                MDMTraceHelper.StartTraceActivity("ConfigurationService.DeleteApplicationConfigurations", MDMTraceSource.Configuration, false);

            try
            {
                if (isTracingEnabled) MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "ConfigurationService has received 'DeleteApplicationConfigurations' request message.", MDMTraceSource.Configuration);

                foreach (ApplicationConfigurationItem configItem in applicationConfigurationItems)
                {
                    configItem.Action = ObjectAction.Delete;
                }

                ApplicationConfigurationItemBL manager = new ApplicationConfigurationItemBL();
                result = manager.Process(applicationConfigurationItems, callerContext);

                if (isTracingEnabled) MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "ConfigurationService has sent 'DeleteApplicationConfigurations' response message.", MDMTraceSource.Configuration);
            }
            catch (Exception ex)
            {
                this.LogException(ex);
                throw base.WrapException(ex);
            }

            if (isTracingEnabled)
                MDMTraceHelper.StopTraceActivity("ConfigurationService.DeleteApplicationConfigurations", MDMTraceSource.Configuration);

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="applicationConfigurationItems"></param>
        /// <param name="callerContext"></param>
        /// <returns></returns>
        public OperationResultCollection CreateApplicationConfigurations(ApplicationConfigurationItemCollection applicationConfigurationItems, CallerContext callerContext)
        {
            OperationResultCollection result = new OperationResultCollection();
            Boolean isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;
            if (isTracingEnabled)
                MDMTraceHelper.StartTraceActivity("ConfigurationService.CreateApplicationConfigurations", MDMTraceSource.Configuration, false);

            try
            {
                if (isTracingEnabled) MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "ConfigurationService has received 'CreateApplicationConfigurations' request message.", MDMTraceSource.Configuration);

                foreach (ApplicationConfigurationItem configItem in applicationConfigurationItems)
                {
                    configItem.Action = ObjectAction.Create;
                }

                ApplicationConfigurationItemBL manager = new ApplicationConfigurationItemBL();
                result = manager.Process(applicationConfigurationItems, callerContext);

                if (isTracingEnabled) MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "ConfigurationService has sent 'CreateApplicationConfigurations' response message.", MDMTraceSource.Configuration);
            }
            catch (Exception ex)
            {
                this.LogException(ex);
                throw base.WrapException(ex);
            }

            if (isTracingEnabled)
                MDMTraceHelper.StopTraceActivity("ConfigurationService.CreateApplicationConfigurations", MDMTraceSource.Configuration);

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="applicationConfigurationItems"></param>
        /// <param name="callerContext"></param>
        /// <returns></returns>
        public OperationResultCollection UpdateApplicationConfigurations(ApplicationConfigurationItemCollection applicationConfigurationItems, CallerContext callerContext)
        {
            OperationResultCollection result = new OperationResultCollection();
            Boolean isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;
            if (isTracingEnabled)
                MDMTraceHelper.StartTraceActivity("ConfigurationService.UpdateApplicationConfigurations", MDMTraceSource.Configuration, false);

            try
            {
                if (isTracingEnabled) MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "ConfigurationService has received 'UpdateApplicationConfigurations' request message.", MDMTraceSource.Configuration);

                foreach (ApplicationConfigurationItem configItem in applicationConfigurationItems)
                {
                    configItem.Action = ObjectAction.Update;
                }

                ApplicationConfigurationItemBL manager = new ApplicationConfigurationItemBL();
                result = manager.Process(applicationConfigurationItems, callerContext);

                if (isTracingEnabled) MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "ConfigurationService has sent 'UpdateApplicationConfigurations' response message.", MDMTraceSource.Configuration);
            }
            catch (Exception ex)
            {
                this.LogException(ex);
                throw base.WrapException(ex);
            }

            if (isTracingEnabled)
                MDMTraceHelper.StopTraceActivity("ConfigurationService.UpdateApplicationConfigurations", MDMTraceSource.Configuration);

            return result;
        }

        #endregion

        #region AppConfig Get

        /// <summary>
        /// Gets app config value for the requested key
        /// </summary>
        /// <param name="appConfigKey">AppConfig key</param>
        /// <returns>Value of the app config key</returns>
        public String GetAppConfig(String appConfigKey)
        {
            String appConfigValue = String.Empty;

            try
            {
                appConfigValue = AppConfigurationHelper.GetAppConfig<String>(appConfigKey);
            }
            catch (Exception ex)
            {
                this.LogException(ex);
                throw base.WrapException(ex);
            }

            return appConfigValue;
        }

        /// <summary>
        /// Gets app config value for the requested key
        /// </summary>
        /// <param name="appConfigKey">AppConfig key</param>
        /// <returns>Value of the app config object</returns>
        public AppConfig GetAppConfigObject(String appConfigKey)
        {
            AppConfig appConfig = null;

            try
            {
                AppConfigBL appConfigManager = new AppConfigBL();
                appConfig = appConfigManager.Get(appConfigKey);
            }
            catch (Exception ex)
            {
                this.LogException(ex);
                throw base.WrapException(ex);
            }

            return appConfig;
        }

        /// <summary>
        /// Get All AppConfigs from the system
        /// </summary>
        /// <param name="callerContext">Context which called the application</param>
        /// <returns>Collection of AppConfigs</returns>
        public AppConfigCollection GetAllAppConfigs(CallerContext callerContext)
        {
            return MakeBusinessLogicCall<AppConfigBL, AppConfigCollection>("GetAppConfigs", businessLogic => businessLogic.GetAll(callerContext));
        }

        #endregion AppConfig Get

        #region AppConfig CUD

        /// <summary>
        /// Create new AppConfig
        /// </summary>
        /// <param name="appConfig">Represent AppConfig Object</param>
        /// <param name="callerContext">Context of application making call for this API</param>
        /// <returns>OperationResult of AppConfig Creation</returns>
        /// <exception cref="ArgumentNullException">If AppConfig Object is Null</exception>
        public OperationResult CreateAppConfig(AppConfig appConfig, CallerContext callerContext)
        {
            return MakeBusinessLogicCall<AppConfigBL, OperationResult>("CreateAppConfig",
                businessLogic => businessLogic.Create(appConfig, callerContext));
        }

        /// <summary>
        /// Update AppConfig
        /// </summary>
        /// <param name="appConfig">Represent AppConfig Object</param>
        /// <param name="callerContext">Context of application making call for this API</param>
        /// <returns>OperationResult of AppConfig Updating</returns>
        /// <exception cref="ArgumentNullException">If AppConfig Object is Null</exception>
        public OperationResult UpdateAppConfig(AppConfig appConfig, CallerContext callerContext)
        {
            return MakeBusinessLogicCall<AppConfigBL, OperationResult>("UpdateAppConfig",
                businessLogic => businessLogic.Update(appConfig, callerContext));

        }

        /// <summary>
        /// Delete AppConfig
        /// </summary>
        /// <param name="appConfig">Represent AppConfig Object</param>
        /// <param name="callerContext">Context of application making call for this API</param>
        /// <returns>OperationResult of AppConfig Updating</returns>
        /// <exception cref="ArgumentNullException">If AppConfig Object is Null</exception>
        public OperationResult DeleteAppConfig(AppConfig appConfig, CallerContext callerContext)
        {
            return MakeBusinessLogicCall<AppConfigBL, OperationResult>("DeleteAppConfig",
                businessLogic => businessLogic.Delete(appConfig, callerContext));

        }

        /// <summary>
        /// Process AppConfigs
        /// </summary>
        /// <param name="appConfigs">Represent AppConfig Object collection to process</param>
        /// <param name="callerContext">Context of application making call for this API</param>
        /// <returns>OperationResult of AppConfigs process</returns>
        /// <exception cref="ArgumentNullException">If AppConfigs Object is Null</exception>
        public OperationResultCollection ProcessAppConfigs(AppConfigCollection appConfigs, CallerContext callerContext)
        {
            return MakeBusinessLogicCall<AppConfigBL, OperationResultCollection>("ProcessAppConfigs",
                businessLogic => businessLogic.Process(appConfigs, callerContext));

        }

        /// <summary>
        /// Process User Configs
        /// </summary>
        ///  <param name="userConfig">Represent UserConfig Object collection to process</param>
        /// <returns>The process status</returns>
        public Int32 ProcessUserConfig(UserConfig userConfig)
        {
            //Currently it returns dummy value which is not used but has to be the OperationalResult 
            int status = 0;
            try
            {
                UserConfigBL userConfigBL = new UserConfigBL();
                userConfigBL.Process(userConfig);
                status = 1;//Success
            }
            catch (Exception ex)
            {
                this.LogException(ex);
                throw base.WrapException(ex);
            }
            return status;
        }

        #endregion AppConfig CUD

        #region ApplicationContext Get

        public Collection<ApplicationContext> getApplicationContext(Int32 ApplicationContextId, Int32 contextType)
        {
            Collection<ApplicationContext> returnVal = new Collection<ApplicationContext>();
            try
            {
                ApplicationContextBL _applicationContextBL = new ApplicationContextBL();
                returnVal = _applicationContextBL.GetApplicationContext(ApplicationContextId, (ApplicationContextType)contextType);
            }
            catch (ArgumentException ex)
            {
                this.LogException(ex);
                throw base.WrapException(ex);
            }
            catch (Exception ex)
            {
                this.LogException(ex);
                throw base.WrapException(ex);
            }
            return returnVal;
        }

        #endregion ApplicationContext Get

        #region Application Context Get

        /// <summary>
        /// Get All ApplicationContexts
        /// </summary>
        /// <param name="callerContext">Indicates the name of Application and Module which invoked the API</param>
        /// <returns>ApplicationContext Collection</returns>
        public ApplicationContextCollection GetAllApplicationContexts(CallerContext callerContext)
        {
            return this.MakeBusinessLogicCall<ApplicationContextBL, ApplicationContextCollection>("GetAllApplicationContexts", businessLogic => businessLogic.Get(callerContext));
        }

        /// <summary>
        /// Get application context by id
        /// </summary>
        /// <param name="applicationContextId">Id of context</param>
        /// <param name="callerContext">Context indicating application calling this API</param>
        /// <returns>ApplicationContext populated with values (Id and LongName)</returns>
        public ApplicationContext GetApplicationContextById(Int32 applicationContextId, CallerContext callerContext)
        {
            return this.MakeBusinessLogicCall<ApplicationContextBL, ApplicationContext>("GetApplicationContextById", businessLogic => businessLogic.GetById(applicationContextId, callerContext));
        }

        #endregion Application Context Get

        #region Application Context CUD

        /// <summary>
        /// Create application context
        /// </summary>
        /// <param name="applicationContext">Application context to create/param>
        /// <param name="callerContext">Context which called the application</param>
        /// <returns>Result of the operation</returns>
        public OperationResult CreateApplicationContext(ApplicationContext applicationContext, CallerContext callerContext)
        {
            return this.MakeBusinessLogicCall<ApplicationContextBL, OperationResult>("CreateApplicationContext", businessLogic => businessLogic.Create(applicationContext, callerContext));
        }

        /// <summary>
        /// Update application context
        /// </summary>
        /// <param name="applicationContext">Application context to Update/param>
        /// <param name="callerContext">Context which called the application</param>
        /// <returns>Result of the operation</returns>
        public OperationResult UpdateApplicationContext(ApplicationContext applicationContext, CallerContext callerContext)
        {
            return this.MakeBusinessLogicCall<ApplicationContextBL, OperationResult>("UpdateApplicationContext", businessLogic => businessLogic.Update(applicationContext, callerContext));
        }

        /// <summary>
        /// Delete application context
        /// </summary>
        /// <param name="applicationContext">Application context to delete/param>
        /// <param name="callerContext">Context which called the application</param>
        /// <returns>Result of the operation</returns>
        public OperationResult DeleteApplicationContext(ApplicationContext applicationContext, CallerContext callerContext)
        {
            return this.MakeBusinessLogicCall<ApplicationContextBL, OperationResult>("DeleteApplicationContext", businessLogic => businessLogic.Delete(applicationContext, callerContext));
        }

        /// <summary>
        /// Process (Create , update, delete) application context
        /// </summary>
        /// <param name="applicationContexts">Application context collection to process</param>
        /// <param name="callerContext">Context which called the application</param>
        /// <returns>Result of the operation</returns>
        public OperationResultCollection ProcessApplicationContexts(ApplicationContextCollection applicationContexts, CallerContext callerContext)
        {
            return this.MakeBusinessLogicCall<ApplicationContextBL, OperationResultCollection>("ProcessApplicationContexts", businessLogic => businessLogic.Process(applicationContexts, callerContext));
        }

        #endregion Application Context CUD

        #region Locale Message Contracts

        /// <summary>
        /// Process Locale Message based on locale
        /// </summary>
        /// <param name="localeMessage">This parameter is specifying instance of locale Message to be processed</param>
        /// <param name="callerContext">Name of application & Module which are performing action</param>
        /// <returns>Results of the operation having errors and information if any</returns>
        public OperationResult ProcessLocaleMessage(LocaleMessage localeMessage, CallerContext callerContext)
        {
            OperationResult opearionResult = new OperationResult();

            try
            {
                LocaleMessageBL localeMessageBL = new LocaleMessageBL();
                opearionResult = localeMessageBL.Process(localeMessage, callerContext);
            }
            catch (Exception ex)
            {
                this.LogException(ex);
                throw base.WrapException(ex);
            }

            return opearionResult;
        }

        /// <summary>
        /// Process Locale Message based on different locale
        /// </summary>
        /// <param name="localeMessages">This parameter is specifying lists of instances of locale Message to be processed</param>
        /// <param name="callerContext">Name of application & Module which are performing action</param>
        /// <returns>Results of the operation having errors and information if any</returns>
        public OperationResult ProcessLocaleMessages(LocaleMessageCollection localeMessages, CallerContext callerContext)
        {
            OperationResult opearionResult = new OperationResult();

            try
            {
                LocaleMessageBL localeMessageBL = new LocaleMessageBL();
                opearionResult = localeMessageBL.Process(localeMessages, callerContext);
            }
            catch (Exception ex)
            {
                this.LogException(ex);
                throw base.WrapException(ex);
            }

            return opearionResult;
        }

        /// <summary>
        /// Get Locale Message based on locale and message code list
        /// </summary>
        /// <param name="locale">Indicates the Message Locale</param>
        /// <param name="messageCodeList">Indicates the Message Code List</param>
        /// <returns>Returns LocaleMessageCollection</returns>
        public LocaleMessageCollection GetLocaleMessages(LocaleEnum locale, Collection<String> messageCodeList, Boolean loadLatest, CallerContext callerContext)
        {
            LocaleMessageCollection localeMessageCollection = new LocaleMessageCollection();

            try
            {
                LocaleMessageBL localeMessageBL = new LocaleMessageBL();
                localeMessageCollection = localeMessageBL.Get(locale, messageCodeList, loadLatest, callerContext);
            }
            catch (Exception ex)
            {
                this.LogException(ex);
                throw base.WrapException(ex);
            }

            return localeMessageCollection;
        }

        #endregion

        #region MDMFeatureConfig Get

        /// <summary>
        /// Get All MDM Feature configurations from the system
        /// </summary>
        /// <param name="callerContext">Context which called the application</param>
        /// <returns>Collection of MDM FeatureConfig</returns>
        public MDMFeatureConfigCollection GetMDMFeatureConfigCollection(CallerContext callerContext)
        {
            return MakeBusinessLogicCall<MDMFeatureConfigBL, MDMFeatureConfigCollection>("GetMDMFeatureConfigCollection", businessLogic => businessLogic.GetMDMFeatureConfigCollection(callerContext));
        }

        /// <summary>
        /// Get MDM Feature configuration from the system
        /// </summary>
        /// <param name="application">Indicates the Application for requested feature config</param>
        /// <param name="moduleName">Indicates module short name for requested feature config</param>             
        /// <param name="version">Indicates the version  for requested feature config</param>   
        /// <param name="callerContext">context indicates the properties of the caller who called the API like program name,Module</param>
        /// <returns>Returns MDM Feature Config</returns>
        public MDMFeatureConfig GetFeatureConfig(MDMCenterApplication application, String moduleName, String version, CallerContext callerContext)
        {
            return MakeBusinessLogicCall<MDMFeatureConfigBL, MDMFeatureConfig>("GetFeatureConfig", businessLogic => businessLogic.GetFeatureConfig(application, moduleName, version, callerContext));
        }

        #endregion MDMFeatureConfig Get

        #region Private Methods

        /// <summary>
        /// Makes calls of ConfigurationService Business logic.
        /// Also emits traces when necessary
        /// </summary>
        /// <typeparam name="TResult">The type of the result of method of business logic</typeparam>
        /// <typeparam name="TBusinessLogic">Type of business logic</typeparam>
        /// <param name="methodName">Name of the method for tracing</param>
        /// <param name="call">The call delegate to be executed against business logic instance</param>
        /// <returns>The value returned by business logic or default</returns>
        private TResult MakeBusinessLogicCall<TBusinessLogic, TResult>(string methodName, Func<TBusinessLogic, TResult> call) where TBusinessLogic : BusinessLogicBase, new()
        {
            Boolean isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;
            if (isTracingEnabled)
                MDMTraceHelper.StartTraceActivity("ConfigurationService." + methodName, MDMTraceSource.Configuration, false);

            TResult operationResult;

            try
            {
                if (isTracingEnabled)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "ConfigurationService receives" + methodName + " request message.", MDMTraceSource.Configuration);

                operationResult = call(new TBusinessLogic());

                if (isTracingEnabled)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "ConfigurationService receives" + methodName + " response message.", MDMTraceSource.Configuration);
            }
            catch (Exception ex)
            {
                this.LogException(ex);
                throw base.WrapException(ex);
            }

            if (isTracingEnabled)
                MDMTraceHelper.StopTraceActivity("ConfigurationService." + methodName, MDMTraceSource.Configuration);

            return operationResult;
        }

        #endregion #region Private Methods
    }
}
