using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Security.Principal;
using System.ServiceModel;

namespace MDM.Services
{
    using MDM.BusinessObjects;
    using MDM.Core;
    using MDM.Core.Exceptions;
    using MDM.Interfaces;
    using MDM.Services.ServiceProxies;
    using MDM.Utility;
    using MDM.WCFServiceInterfaces;

    /// <summary>
    /// Configuration Service facilitates to work with MDMCenter configuration system. 
    /// This includes fetching MDMCenter application configuration keys, application configuration settings, user specific display settings, and so on.
    /// </summary>
    public class ConfigurationService : ServiceClientBase
    {
        #region Fields

        #endregion

        #region Properties

        #endregion

        #region Constructors

        /// <summary>
        /// Use this default constructor only when calling MDM service within the execution context of an MDM Application. 
        /// Example: Service call from the MDM business rule.
        /// This would try to use current execution security context for service authentication purpose. 
        /// </summary>
        public ConfigurationService()
            : base(typeof(ConfigurationService))
        {
        }

        /// <summary>
        /// Use this default constructor with binding configuration name only when calling MDM service within the execution context of an MDM Application. 
        /// Example: Service call from the MDM business rule.
        /// This would try to use current execution security context for service authentication purpose. 
        /// </summary>
        /// <param name="endPointConfigurationName">Indicates the EndPointConfigurationName</param>
        public ConfigurationService(String endPointConfigurationName)
            : base(typeof(ConfigurationService), endPointConfigurationName)
        {
        }

        /// <summary>
        /// Use this constructor for form based authentication passing username and password
        /// </summary>
        /// <param name="endPointConfigurationName">Indicates the EndPointConfigurationName</param>
        /// <param name="userName">Name of the Login User</param>
        /// <param name="password">Password of the User</param>
        public ConfigurationService(String endPointConfigurationName, String userName, String password)
            : base(endPointConfigurationName, userName, password)
        {

        }

        /// <summary>
        /// Use this constructor for window authentication passing current windows identity.
        /// </summary>
        /// <param name="endPointConfigurationName">Indicates the EndPointConfigurationName</param>
        /// <param name="userIdentity">Indicates User's Identity</param>
        public ConfigurationService(String endPointConfigurationName, IIdentity userIdentity)
            : base(endPointConfigurationName, userIdentity)
        {
        }

        /// <summary>
        /// Constructor with client configuration.
        /// </summary>
        /// <param name="wcfClientConfiguration">Indicates Client Configuration properties for MDM WCF Service</param>
        public ConfigurationService(IWCFClientConfiguration wcfClientConfiguration)
            : base(wcfClientConfiguration)
        {
        }

        /// <summary>
        /// Constructor with endPointConfigurationName, authenticationType, userIdentity, userName and password
        /// </summary>
        /// <param name="endPointConfigurationName">Indicates the EndPointConfigurationName</param>
        /// <param name="authenticationType">Indicates Type of Authentication i.e Windows/Forms</param>
        /// <param name="userIdentity">Indicates Identity of the Login User</param>
        /// <param name="userName">Name of the Login User</param>
        /// <param name="password">Password of the User</param>
        public ConfigurationService(String endPointConfigurationName, AuthenticationType authenticationType, IIdentity userIdentity, String userName, String password)
            : base(endPointConfigurationName, authenticationType, userIdentity, userName, password)
        {
        }

        /// <summary>
        /// Constructor with endPointConfigurationName, endPointAddress, authenticationType, userIdentity, userName and password
        /// </summary>
        /// <param name="endPointConfigurationName">Indicates the EndPointConfigurationName</param>
        /// <param name="endPointAddress"> Provides a unique network address that a client uses to communicate with a service endpoint.</param>
        /// <param name="authenticationType">Indicates Type of Authentication i.e Windows/Forms</param>
        /// <param name="userIdentity">Indicates Identity of the Login User</param>
        /// <param name="userName">Name of the Login User</param>
        /// <param name="password">Password of the User</param>
        public ConfigurationService(String endPointConfigurationName, EndpointAddress endPointAddress, AuthenticationType authenticationType, IIdentity userIdentity, String userName, String password)
            : base(endPointConfigurationName, endPointAddress, authenticationType, userIdentity, userName, password)
        {
        }

        #endregion

        #region Application Config Methods

        /// <summary>
        /// Gets application configurations for the requested context parameters
        /// </summary>
        /// <example> 
        /// <code> 
        ///  public Dictionary<![CDATA[<String, String>]]> GetApplicationConfigurations()
        ///   {
        ///    // Get configuration service
        ///     ConfigurationService configurationService = GetMDMConfigurationService();
        /// 
        ///    // Dictinary parameterKey\parameterXML as a result of calling service
        ///     Dictionary<![CDATA[<String, String>]]> applicationConfiguration = new Dictionary<![CDATA[<string, string>]]>();
        /// 
        ///     #region Preparing parameters
        /// 
        ///    // Assumption: Hardcode values for parameters to get specific application configuraion is considered for this sample
        ///     Int32 eventSourceId = (Int32) EventSource.MDMCenter;
        ///     Int32 eventSubscriberId = (Int32) EventSubscriber.LocaleConfiguration;
        ///     Int32 roleId = 2; // role cfadmin
        ///     Int32 userId = 1; //cfadmin
        ///     Int32 organizationId = 1;
        ///     Int32 catalogId =  5; // Product master
        ///     Int64 categoryId = 1; //Apparel
        ///     Int32 nodeTypeId = 16; // Style
        ///     Int32 relationshipTypeId = 1; // Accessories
        ///     Int32 localeId = (Int32) LocaleEnum.en_WW;
        ///     Int32 applicationConfigurationId = 70;
        /// 
        ///    // No need to pass cNode and attribute Id, they are initiated as 0
        ///     Int32 cNodeId = 0;
        ///     Int32 attributeId = 0;
        /// 
        ///     #endregion Preparing parameters
        /// 
        ///     applicationConfiguration = configurationService.GetApplicationConfigurations(eventSourceId, eventSubscriberId, roleId, userId, organizationId, catalogId, categoryId, cNodeId, attributeId, nodeTypeId, relationshipTypeId, localeId, applicationConfigurationId);
        /// 
        ///     return applicationConfiguration;
        ///   }
        /// </code>
        /// </example>
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
        /// <param name="localId">Indicates an Id of a locale</param>
        /// <param name="applicationConfigId">Indicates application configuration Id</param>
        /// <returns>Object containing all the configurations for the requested context in the key-value pair</returns>
        /// <exception cref="EndpointNotFoundException">Thrown when a remote endpoint could not be found or reached</exception>
        /// <exception cref="InvalidOperationException">Thrown when a method call is invalid</exception>
        /// <exception cref="TimeoutException">Thrown when the time allotted for an operation has expired</exception>
        /// <exception cref="MDMOperationException">Thrown when the MDM operation fails</exception>
        /// <exception cref="CommunicationException">Thrown when there is a communication error in either the service or client application</exception>
        public Dictionary<String, String> GetApplicationConfigurations(Int32 eventSourceId, Int32 eventSubscriberId, Int32 securityRoleId, Int32 securityUserId, Int32 orgId, Int32 catalogId, Int64 categoryId, Int64 cNodeId, Int32 attributeId, Int32 nodeTypeId, Int32 relationshipTypeId, Int32 localId, Int32 applicationConfigId)
        {
            return MakeServiceCall("GetApplicationConfigurations",
                                   "GetApplicationConfigurations",
                                   service =>
                                       {
                                           Boolean isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;
                                           if (isTracingEnabled)
                                           {
                                               MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("Get Application Configurations for eventSourceId: {0}, eventSubscriberId: {1}, securityUserId: {2}, orgId: {3}, catalogId: {4}, cNodeId: {5}, attributeId: {6}, nodeTypeId: {7}, relationshipTypeId: {8}, localId: {9}, applicationConfigId: {10}", eventSourceId, eventSubscriberId, securityUserId, orgId, catalogId, cNodeId, attributeId, nodeTypeId, relationshipTypeId, localId, applicationConfigId));
                                           }
                                           return service.GetApplicationConfigurations(eventSourceId,
                                                                                       eventSubscriberId,
                                                                                       securityRoleId,
                                                                                       securityUserId,
                                                                                       orgId,
                                                                                       catalogId,
                                                                                       categoryId,
                                                                                       cNodeId,
                                                                                       attributeId,
                                                                                       nodeTypeId,
                                                                                       relationshipTypeId,
                                                                                       localId,
                                                                                       applicationConfigId);
                                       });
        }

        /// <summary>
        /// Deletes application configurations for the specified context
        /// </summary>
        /// <example>
        /// <code>
        /// // Gets MDM configuration service
        /// ConfigurationService configurationService = GetMDMConfigurationService();
        /// //Creates the new instance of ApplicationConfigurationItemCollection and ApplicationConfigurationItem   
        /// ApplicationConfigurationItemCollection configs = new ApplicationConfigurationItemCollection();
        /// ApplicationConfigurationItem config = new ApplicationConfigurationItem();
        ///
        /// Populates the application configuration item
        /// config.Id = 1; //Specifies id of the application configuration to delete
        /// configs.Add(config);
        /// // Gets new instance of ICallerContext using MDMObjectFactory
        /// ICallerContext callerContext = <![CDATA[MDMObjectFactory.GetICallerContext()]]>;
        ///
        /// // Specifies application and module performing the action
        /// callerContext.Application = MDMCenterApplication.MDMCenter;
        /// callerContext.Module = MDMCenterModules.TMSConnector;
        ///
        /// OperationResult iOperationResult = configurationService.DeleteApplicationConfigurations(configs, callerContext);
        ///
        /// </code>
        /// </example>
        /// <param name="iApplicationConfigurationItems">Indicates List of application configuration items to be deleted</param>
        /// <param name="iCallerContext">Indicates the name of application and the module that are performing the action</param>
        /// <returns>Returns operation result collection</returns>
        /// <exception cref="EndpointNotFoundException">Thrown when a remote endpoint could not be found or reached</exception>
        /// <exception cref="InvalidOperationException">Thrown when a method call is invalid</exception>
        /// <exception cref="TimeoutException">Thrown when the time allotted for an operation has expired</exception>
        /// <exception cref="MDMOperationException">Thrown when the MDM operation fails</exception>
        /// <exception cref="CommunicationException">Thrown when there is a communication error in either the service or client application</exception>
        public IOperationResultCollection DeleteApplicationConfigurations(IApplicationConfigurationItemCollection iApplicationConfigurationItems, ICallerContext iCallerContext)
        {
            return MakeServiceCall<IOperationResultCollection>("DeleteApplicationConfigurations", "DeleteApplicationConfigurations",
                                                  client =>
                                                      client.DeleteApplicationConfigurations(iApplicationConfigurationItems as ApplicationConfigurationItemCollection, FillDiagnosticTraces(iCallerContext)));
        }

        /// <summary>
        /// Creates the application configurations for the specified context
        /// </summary>
        /// <example>
        /// <code>
        /// // Gets MDM configuration service
        /// ConfigurationService configurationService = GetMDMConfigurationService();
        /// //Creates the new instance of ApplicationConfigurationItemCollection and ApplicationConfigurationItem   
        /// ApplicationConfigurationItemCollection configs = new ApplicationConfigurationItemCollection();
        /// ApplicationConfigurationItem config = new ApplicationConfigurationItem();
        ///
        /// Populates the application configuration item
        /// String configName = "MDMCenter - TranslationConfig - " + Guid.NewGuid();
        /// config.Name = configName;
        /// config.LongName = configName;
        /// config.OrganizationId = 2; //Specifies the organization id for which configuration is defined
        /// config.ContainerId = 5; //Specifies the container id for which configuration is defined
        /// config.CategoryId = 1033; //Specifies the category id for which configuration is defined
        /// config.ContextDefinitionId = 177; //Specifies the application context id for application configuration
        /// config.ConfigXml = 'Please put configuration in xml format here';
        /// configs.Add(config);
        /// // Gets new instance of ICallerContext using MDMObjectFactory
        /// ICallerContext callerContext = <![CDATA[MDMObjectFactory.GetICallerContext()]]>;
        ///
        /// // Specifies application and module performing the action
        /// callerContext.Application = MDMCenterApplication.MDMCenter;
        /// callerContext.Module = MDMCenterModules.TMSConnector;
        ///
        /// OperationResult iOperationResult = configurationService.CreateApplicationConfigurations(configs, callerContext);
        ///
        /// </code>
        /// </example>
        /// <param name="iApplicationConfigurationItems">List of application configuration items to be created</param>
        /// <param name="iCallerContext">Indicates the name of application and the module that are performing the action</param>
        /// <returns>Returns operation result collection</returns>
        /// <exception cref="EndpointNotFoundException">Thrown when a remote endpoint could not be found or reached</exception>
        /// <exception cref="InvalidOperationException">Thrown when a method call is invalid</exception>
        /// <exception cref="TimeoutException">Thrown when the time allotted for an operation has expired</exception>
        /// <exception cref="MDMOperationException">Thrown when the MDM operation fails</exception>
        /// <exception cref="CommunicationException">Thrown when there is a communication error in either the service or client application</exception>
        public IOperationResultCollection CreateApplicationConfigurations(IApplicationConfigurationItemCollection iApplicationConfigurationItems, ICallerContext iCallerContext)
        {
            return MakeServiceCall<IOperationResultCollection>("CreateApplicationConfigurations", "CreateApplicationConfigurations",
                                                  client =>
                                                      client.CreateApplicationConfigurations(iApplicationConfigurationItems as ApplicationConfigurationItemCollection, FillDiagnosticTraces(iCallerContext)));
        }

        /// <summary>
        /// Updates application configurations for the specified context
        /// </summary>
        /// <example>
        /// <code>
        /// // Gets MDM configuration service
        /// ConfigurationService configurationService = GetMDMConfigurationService();
        /// //Creates the new instance of ApplicationConfigurationItemCollection and ApplicationConfigurationItem   
        /// ApplicationConfigurationItemCollection configs = new ApplicationConfigurationItemCollection();
        /// ApplicationConfigurationItem config = new ApplicationConfigurationItem();
        ///
        /// Populates the application configuration item
        /// String configName = "MDMCenter - TranslationConfig - " + Guid.NewGuid();
        /// config.Id = 1; //Specifies id of the application configuration to update
        /// String configName = "MDMCenter - TranslationConfig - Update "
        /// config.Name = configName;
        /// config.LongName = configName;
        /// config.OrganizationId = 2; //Specifies the organization id for which configuration is defined
        /// config.ContainerId = 5; //Specifies the container id for which configuration is defined
        /// config.CategoryId = 1033; //Specifies the category id for which configuration is defined
        /// config.ContextDefinitionId = 177; //Specifies the application context id for application configuration
        /// config.ConfigXml = 'Please put configuration in xml format here';
        /// configs.Add(config);
        /// // Gets new instance of ICallerContext using MDMObjectFactory
        /// ICallerContext callerContext = <![CDATA[MDMObjectFactory.GetICallerContext()]]>;
        ///
        /// // Specifies application and module performing the action
        /// callerContext.Application = MDMCenterApplication.MDMCenter;
        /// callerContext.Module = MDMCenterModules.TMSConnector;
        ///
        /// OperationResult iOperationResult = configurationService.UpdateApplicationConfigurations(configs, callerContext);
        ///
        /// </code>
        /// </example>
        /// <param name="iApplicationConfigurationItems">Indicates List of application configuration items to be updated</param>
        /// <param name="iCallerContext">Indicates the name of application and the module that are performing the action</param>
        /// <returns>Returns operation result collection</returns>
        /// <exception cref="EndpointNotFoundException">Thrown when a remote endpoint could not be found or reached</exception>
        /// <exception cref="InvalidOperationException">Thrown when a method call is invalid</exception>
        /// <exception cref="TimeoutException">Thrown when the time allotted for an operation has expired</exception>
        /// <exception cref="MDMOperationException">Thrown when the MDM operation fails</exception>
        /// <exception cref="CommunicationException">Thrown when there is a communication error in either the service or client application</exception>
        public IOperationResultCollection UpdateApplicationConfigurations(IApplicationConfigurationItemCollection iApplicationConfigurationItems, ICallerContext iCallerContext)
        {
            return MakeServiceCall<IOperationResultCollection>("UpdateApplicationConfigurations", "UpdateApplicationConfigurations",
                                                  client =>
                                                      client.UpdateApplicationConfigurations(iApplicationConfigurationItems as ApplicationConfigurationItemCollection, FillDiagnosticTraces(iCallerContext)));
        }

        #endregion
        
        #region AppConfig Methods

        /// <summary>
        /// Gets the app config value for the requested app config key
        /// </summary>
        /// <param name="appConfigKey">AppConfig key</param>
        /// <returns>Value of the app config key</returns>
        /// <exception cref="EndpointNotFoundException">Thrown when a remote endpoint could not be found or reached</exception>
        /// <exception cref="InvalidOperationException">Thrown when a method call is invalid</exception>
        /// <exception cref="TimeoutException">Thrown when the time allotted for an operation has expired</exception>
        /// <exception cref="MDMOperationException">Thrown when the MDM operation fails</exception>
        /// <exception cref="CommunicationException">Thrown when there is a communication error in either the service or client application</exception>
        /// <example>
        /// <code language="c#" title="Get AppConfig" source="..\MDM.APISamples\Administration\ConfigManager\ManageConfig.cs" region="GetAppConfigValue" />
        /// </example>
        public new String GetAppConfig(String appConfigKey)
        {
            return MakeServiceCall("GetAppConfig",
                                   "GetAppConfig",
                                   service =>
                                       {
                                           Boolean isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;
                                           if (isTracingEnabled)
                                           {
                                               MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("Get AppConfig for AppConfigKey: {0}", appConfigKey));
                                           }
                                           return service.GetAppConfig(appConfigKey);
                                       });
        }

        /// <summary>
        /// Get All AppConfigs from the system
        /// </summary>
        /// <param name="iCallerContext">Context which called the application</param>
        /// <returns>Collection of AppConfigs</returns>
        public IAppConfigCollection GetAllAppConfigs(ICallerContext iCallerContext)
        {
            return MakeServiceCall("GetAllAppConfigs", "GetAllAppConfigs",
               client =>
               client.GetAllAppConfigs(FillDiagnosticTraces(iCallerContext)));
        }

        /// <summary>
        /// Gets app config value for the requested key
        /// </summary>
        /// <example>
        /// <code>
        /// // Key to which you want to know the value
        ///    String appConfigKey = "Lookup.MaxRecord";
        ///    String appConfig = String.Empty;
        ///
        /// // Get Configuration Service
        ///    ConfigurationService configurationService = new ConfigurationService();
        ///       
        /// // Make a WCF call for configuration Service to get the AppConfig Value
        ///    appConfig = configurationService.GetAppConfig(appConfigKey);
        /// 
        /// </code>
        /// </example>
        /// <param name="appConfigKey">AppConfig key</param>
        /// <returns>app config object</returns>
        /// <exception cref="EndpointNotFoundException">Thrown when a remote endpoint could not be found or reached</exception>
        /// <exception cref="InvalidOperationException">Thrown when a method call is invalid</exception>
        /// <exception cref="TimeoutException">Thrown when the time allotted for an operation has expired</exception>
        /// <exception cref="MDMOperationException">Thrown when the MDM operation fails</exception>
        /// <exception cref="CommunicationException">Thrown when there is a communication error in either the service or client application</exception>
        public IAppConfig GetAppConfigObject(String appConfigKey)
        {
            return MakeServiceCall("GetAppConfigObject",
                                   "GetAppConfigObject",
                                   service =>
                                       {
                                           Boolean isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;
                                           if (isTracingEnabled)
                                           {
                                               MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("Get AppConfig for AppConfigKey: {0}", appConfigKey));
                                           }
                                           return service.GetAppConfigObject(appConfigKey);
                                       });
        }

        #endregion AppConfig Get

        #region AppConfig CUD

        /// <summary>
        /// Create new AppConfig
        /// </summary>
        /// <param name="iAppConfig">Represent AppConfig Object</param>
        /// <param name="iCallerContext">Context of application making call for this API</param>
        /// <returns>OperationResult of AppConfig Creation</returns>
        /// <exception cref="ArgumentNullException">If AppConfig Object is Null</exception>
        public IOperationResult CreateAppConfig(IAppConfig iAppConfig, ICallerContext iCallerContext)
        {
            return MakeServiceCall("CreateAppConfig", "CreateAppConfig",
               client =>
               client.CreateAppConfig(iAppConfig as AppConfig, FillDiagnosticTraces(iCallerContext)));
        }

        /// <summary>
        /// Update AppConfig
        /// </summary>
        /// <param name="iAppConfig">Represent AppConfig Object</param>
        /// <param name="iCallerContext">Context of application making call for this API</param>
        /// <returns>OperationResult of AppConfig Updating</returns>
        /// <exception cref="ArgumentNullException">If AppConfig Object is Null</exception>
        public IOperationResult UpdateAppConfig(IAppConfig iAppConfig, ICallerContext iCallerContext)
        {
            return MakeServiceCall("UpdateAppConfig", "UpdateAppConfig",
               client =>
               client.UpdateAppConfig(iAppConfig as AppConfig, FillDiagnosticTraces(iCallerContext)));
        }

        /// <summary>
        /// Delete AppConfig
        /// </summary>
        /// <param name="iAppConfig">Represent AppConfig Object</param>
        /// <param name="iCallerContext">Context of application making call for this API</param>
        /// <returns>OperationResult of AppConfig Deletion</returns>
        /// <exception cref="ArgumentNullException">If AppConfig Object is Null</exception>
        public IOperationResult DeleteAppConfig(IAppConfig iAppConfig, ICallerContext iCallerContext)
        {
            return MakeServiceCall("DeleteAppConfig", "DeleteAppConfig",
               client =>
               client.DeleteAppConfig(iAppConfig as AppConfig, FillDiagnosticTraces(iCallerContext)));
        }

        /// <summary>
        /// Process AppConfigs
        /// </summary>
        /// <param name="iAppConfigs">Represent AppConfig Object collection to process</param>
        /// <param name="iCallerContext">Context of application making call for this API</param>
        /// <returns>OperationResult of AppConfigs process</returns>
        /// <exception cref="ArgumentNullException">If AppConfigs Object is Null</exception>
        public IOperationResultCollection ProcessAppConfigs(IAppConfigCollection iAppConfigs, ICallerContext iCallerContext)
        {

            return MakeServiceCall("ProcessAppConfigs", "ProcessAppConfigs",
               client =>
               client.ProcessAppConfigs(iAppConfigs as AppConfigCollection, FillDiagnosticTraces(iCallerContext)));
        }

        #endregion AppConfig CUD

        #region Application Context Get

        /// <summary>
        /// Get All ApplicationContexts
        /// </summary>
        /// <param name="callerContext">Indicates the name of Application and Module which invoked the API</param>
        /// <returns>ApplicationContext Collection</returns>
        public IApplicationContextCollection GetAllApplicationContexts(ICallerContext callerContext)
        {
            return this.MakeServiceCall<IApplicationContextCollection>("GetAllApplicationContexts", "GetAllApplicationContexts", client => client.GetAllApplicationContexts(FillDiagnosticTraces(callerContext)));
        }

        /// <summary>
        /// Get application context by id
        /// </summary>
        /// <param name="applicationContextId">Id of context</param>
        /// <param name="callerContext">Context indicating application calling this API</param>
        /// <returns>ApplicationContext populated with values (Id and LongName)</returns>
        public IApplicationContext GetApplicationContextById(Int32 applicationContextId, ICallerContext callerContext)
        {
            return this.MakeServiceCall<IApplicationContext>("GetApplicationContextById", "GetApplicationContextById", client => client.GetApplicationContextById(applicationContextId, FillDiagnosticTraces(callerContext)));
        }

        #endregion Application Context Get

        #region Application Context CUD

        /// <summary>
        /// Creates application context
        /// </summary>
        /// <param name="applicationContext">Indicates the application context to create</param>
        /// <param name="callerContext">Indicates the context which called the application</param>
        /// <returns>Returns the result of the operation</returns>
        public IOperationResult CreateApplicationContext(ApplicationContext applicationContext, CallerContext callerContext)
        {
            return this.MakeServiceCall<IOperationResult>("CreateApplicationContext", "CreateApplicationContext", client => client.CreateApplicationContext(applicationContext as ApplicationContext, FillDiagnosticTraces(callerContext)));
        }

        /// <summary>
        /// Update application context
        /// </summary>
        /// <param name="applicationContext">Application context to create</param>
        /// <param name="callerContext">Context which called the application</param>
        /// <returns>Result of the operation</returns>
        public IOperationResult UpdateApplicationContext(ApplicationContext applicationContext, CallerContext callerContext)
        {
            return this.MakeServiceCall<IOperationResult>("UpdateApplicationContext", "UpdateApplicationContext", client => client.UpdateApplicationContext(applicationContext as ApplicationContext, FillDiagnosticTraces(callerContext)));
        }

        /// <summary>
        /// Deletes application context
        /// </summary>
        /// <param name="applicationContext">Indicates the application context to create</param>
        /// <param name="callerContext">Indicates the context which called the application</param>
        /// <returns>Returns the result of the operation</returns>
        public IOperationResult DeleteApplicationContext(ApplicationContext applicationContext, CallerContext callerContext)
        {
            return this.MakeServiceCall<IOperationResult>("DeleteApplicationContext", "DeleteApplicationContext", client => client.DeleteApplicationContext(applicationContext as ApplicationContext, FillDiagnosticTraces(callerContext)));
        }

        /// <summary>
        /// Process (Create , update, delete) application context
        /// </summary>
        /// <param name="applicationContexts">Application context collection to process</param>
        /// <param name="callerContext">Context which called the application</param>
        /// <returns>Result of the operation</returns>
        public IOperationResultCollection ProcessApplicationContexts(IApplicationContextCollection applicationContexts, ICallerContext callerContext)
        {
            return this.MakeServiceCall<IOperationResultCollection>("ProcessApplicationContexts", "ProcessApplicationContexts", client => client.ProcessApplicationContexts(applicationContexts as ApplicationContextCollection, FillDiagnosticTraces(callerContext)));
        }

        /// <summary>
        /// Processes the user configuration value with an userconfig object specified
        /// </summary>
        /// <example>
        /// <code>
        /// // Get MDM configuration service
        /// ConfigurationService configurationService = GetMDMConfigurationService();
        /// 
        /// // Specifies the user config to be processed
        /// IUserConfig userConfig = new UserConfig();
        /// 
        /// // Populate the UserConfig object with values
        /// // The below values in the ConfigXml as per River Works Data Model
        /// // Attribute Name = Attribute Group Name 
        /// // MappedName =  Attribute Short Name
        /// // ID = Attribute ID
        /// userConfig.ConfigXml = "<UserConfig><Attribute Name="ACAdapter [General]" ID="4902" MappedName="ACAdapter" Selected="true" SelectedSeqNo="1" /><Attribute Name="Accessories [General]" ID="4903" MappedName="Accessories" Selected="true" SelectedSeqNo="0" /></UserConfig>";        
        /// 
        /// // UserConfigType is 5 whiuch is Search Attributes
        /// userConfig.UserConfigTypeId = 5;
        /// 
        /// // Provide the organisation id as River works
        /// userConfig.OrgId = 2;
        /// 
        /// // Below will make WCF call which will process (Create)  user configuration value
        /// int processUserConfig = configurationService.ProcessUserConfig(userConfig);
        /// </code>
        /// </example>
        /// <param name="iUserConfig">Indicates UserConfig Object which will be processed</param>
        /// <returns>The status of the operation which is 1 for Success and 0 for Failure</returns>
        public Int32 ProcessUserConfig(IUserConfig iUserConfig)
        {
            return MakeServiceCall("ProcessUserConfig", "ProcessUserConfig", client => client.ProcessUserConfig(iUserConfig as UserConfig));
        }

        #endregion Application Context CUD

        #region Locale Message Methods
        
        /// <summary>
        /// Get Locale Message collection based on locale and message code list
        /// </summary>
        /// <param name="locale">Indicates the Message Locale</param>
        /// <param name="messageCodeList">Indicates the Message Code List</param>
        /// <param name="loadLatest">Indicates whether to load latest messages or not</param>
        /// <param name="callerContext">Indicates name of Application and Module which are performing action</param>
        /// <returns>Returns LocaleMessageCollection Interface</returns>
        [Obsolete("This method has been obsoleted. Please use ConfigurationService.GetLocaleMessages(LocaleEnum locale, Collection<String> messageCodeList, Boolean loadLatest, ICallerContext iCallerContext) method instead of this")]
        public ILocaleMessageCollection GetLocaleMessages(LocaleEnum locale, Collection<String> messageCodeList, Boolean loadLatest, CallerContext callerContext)
        {
            return MakeServiceCall("GetLocaleMessages",
                                   "GetLocaleMessages",
                                   service =>
                                       {
                                           Boolean isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;
                                           if (isTracingEnabled)
                                           {
                                               MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("Get Locale Messages for Locale: {0}", locale));
                                           }
                                           return service.GetLocaleMessages(locale,
                                                                            messageCodeList,
                                                                            loadLatest,
                                                                            FillDiagnosticTraces(callerContext));
                                       });
        }

        /// <summary>
        /// Process Locale Message based on locale
        /// </summary>
        /// <param name="iLocaleMessage">This parameter is specifying instance of locale Message to be processed</param>
        /// <param name="iCallerContext">Name of application and Module which are performing action</param>
        /// <returns>Results of the operation having errors and information if any</returns>
        public IOperationResult Process(ILocaleMessage iLocaleMessage, ICallerContext iCallerContext)
        {
            return this.MakeServiceCall<IOperationResult>("ProcessLocaleMessage", "ProcessLocaleMessage", client => client.ProcessLocaleMessage(iLocaleMessage as LocaleMessage, FillDiagnosticTraces(iCallerContext)));
        }

        /// <summary>
        /// Process Locale Message based on different locale
        /// </summary>
        /// <param name="iLocaleMessages">This parameter is specifying lists of instances of locale Message to be processed</param>
        /// <param name="iCallerContext">Name of application and Module which are performing action</param>
        /// <returns>Results of the operation having errors and information if any</returns>
        public IOperationResult Process(ILocaleMessageCollection iLocaleMessages, ICallerContext iCallerContext)
        {
            return this.MakeServiceCall<IOperationResult>("ProcessLocaleMessages", "ProcessLocaleMessages", client => client.ProcessLocaleMessages(iLocaleMessages as LocaleMessageCollection, FillDiagnosticTraces(iCallerContext)));
        }

        /// <summary>
        /// Gets the locale message collection based on the locale and message code list from the cache
        /// </summary>
        /// <param name="locale">Indicates the locale in which the locale message is requested</param>
        /// <param name="messageCodeList">Indicates the list of message codes</param>
        /// <param name="iCallerContext">Indicates the caller context which contains the name of application and module that has invoked the API</param>
        /// <returns>Returns a collection of locale messages for the specified locale and message code list</returns>
        /// <example>
        /// <code language="c#" title="Get LocaleMessages" source="..\MDM.APISamples\Administration\ConfigManager\ConfigurationManager.cs" region="Get LocaleMessages" />
        /// </example>
        public ILocaleMessageCollection GetLocaleMessages(LocaleEnum locale, Collection<String> messageCodeList, ICallerContext iCallerContext)
        {
            return GetLocaleMessages(locale, messageCodeList, false, iCallerContext);
        }

        /// <summary>
        /// Gets the locale message collection based on the locale and message code list 
        /// </summary>
        /// <param name="locale">Indicates the locale in which the locale message is requested</param>
        /// <param name="messageCodeList">Indicates the list of message codes</param>
        /// <param name="loadLatest">Indicates whether to load latest messages or not.</param>
        /// <param name="iCallerContext">Indicates the caller context which contains the application and module that has invoked the API</param>
        /// <returns>Returns a collection of locale messages for the specified locale and message code list</returns>
        /// <example>
        /// <code language="c#" title="Get LocaleMessages" source="..\MDM.APISamples\Administration\ConfigManager\ConfigurationManager.cs" region="Get Latest LocaleMessages" />
        /// </example>
        public ILocaleMessageCollection GetLocaleMessages(LocaleEnum locale, Collection<String> messageCodeList, Boolean loadLatest, ICallerContext iCallerContext)
        {
            return this.MakeServiceCall<ILocaleMessageCollection>("GetLocaleMessages", "GetLocaleMessages", client => client.GetLocaleMessages(locale, messageCodeList, loadLatest, FillDiagnosticTraces(iCallerContext)));
        }

        #endregion

        #region MDMFeatureConfig Methods

        /// <summary>
        /// Gets all MDM feature configurations from the system
        /// </summary>
        /// <example>
        /// <code>
        /// // It is HIGHLY RECOMMENDED to set caller context before sending to WCF
        ///    ICallerContext callerContext = MDMObjectFactory.GetICallerContext(MDMCenterApplication.MDMCenter,  MDMCenterModules.Security);
        ///    
        ///    IMDMFeatureConfigCollection mdmFeatureConfigCollection = null;
        ///
        /// // Get Configuration Service
        ///    ConfigurationService configurationService = GetMDMConfigurationService();
        ///       
        /// // Make a WCF call for configuration Service to get MDM Feature configurations
        ///    mdmFeatureConfigCollection = configurationService.GetMDMFeatureConfigCollection(callerContext);
        /// </code>
        /// </example>
        /// <param name="icallerContext">Indicates context which called the application</param>
        /// <returns>Returns collection of MDM Feature configurations</returns>
        /// <exception cref="EndpointNotFoundException">Thrown when a remote endpoint could not be found or reached</exception>
        /// <exception cref="InvalidOperationException">Thrown when a method call is invalid</exception>
        /// <exception cref="TimeoutException">Thrown when the time allotted for an operation has expired</exception>
        /// <exception cref="MDMOperationException">Thrown when the MDM operation fails</exception>
        /// <exception cref="CommunicationException">Thrown when there is a communication error in either the service or client application</exception>
        public IMDMFeatureConfigCollection GetMDMFeatureConfigCollection(ICallerContext icallerContext)
        {
            return MakeServiceCall("GetMDMFeatureConfigCollection", "GetMDMFeatureConfigCollection",
               client =>
               client.GetMDMFeatureConfigCollection(FillDiagnosticTraces(icallerContext)));
        }

        /// <summary>
        /// Gets MDM feature configuration by application, module name and version
        /// </summary>
        /// <example>
        /// <code>
        /// // Application under MDMCenter for which you want to know the MDM feature config value
        ///    MDMCenterApplication application = MDMCenterApplication.PIM;
        ///    
        /// // Module Path of MDMCenter Application for which you want to know the MDM feature config value
        ///    String moduleName = "Entity";
        ///    
        /// // Version of feature for which you want to know the MDM feature config value
        ///    String version = "1";
        ///    
        /// // It is HIGHLY RECOMMENDED to set caller context before sending to WCF
        ///    ICallerContext callerContext = MDMObjectFactory.GetICallerContext(MDMCenterApplication.MDMCenter,  MDMCenterModules.Security);
        ///    
        ///    IMDMFeatureConfig mdmFeatureConfig = null;
        ///    
        /// // Get Configuration Service
        ///    ConfigurationService configurationService = GetMDMConfigurationService();
        ///       
        /// // Make a WCF call for configuration Service to get the MDM feature config
        ///    mdmFeatureConfig = configurationService.GetFeatureConfig(application, moduleName, version, callerContext);        
        /// </code>
        /// </example>
        /// <param name="application">Indicates the Application for requested feature config</param>
        /// <param name="moduleName">Indicates module short name for requested feature config</param>             
        /// <param name="version">Indicates the version  for requested feature config</param>   
        /// <param name="icallerContext">Indicates the properties of the caller who called the API like program name,Module</param>
        /// <returns>Returns MDM Feature Config</returns>
        /// <exception cref="EndpointNotFoundException">Thrown when a remote endpoint could not be found or reached</exception>
        /// <exception cref="InvalidOperationException">Thrown when a method call is invalid</exception>
        /// <exception cref="TimeoutException">Thrown when the time allotted for an operation has expired</exception>
        /// <exception cref="MDMOperationException">Thrown when the MDM operation fails</exception>
        /// <exception cref="CommunicationException">Thrown when there is a communication error in either the service or client application</exception>
        public IMDMFeatureConfig GetFeatureConfig(MDMCenterApplication application, String moduleName, String version, ICallerContext icallerContext)
        {
            return MakeServiceCall("GetFeatureConfig", "GetFeatureConfig",
               client =>
               client.GetFeatureConfig(application, moduleName, version, FillDiagnosticTraces(icallerContext)));
        }

        #endregion MDMFeatureConfig Get

        #region Private Methods

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private IConfigurationService GetClient()
        {
            IConfigurationService serviceClient = null;

            if (WCFServiceInstanceLoader.IsLocalInstancesEnabled())
            {
                serviceClient = WCFServiceInstanceLoader.GetServiceInstance<IConfigurationService>();
            }

            if (serviceClient == null) //Means the given type is not implemented for local load..
            {
                ConfigurationServiceProxy configurationServiceProxy = null;

                if (String.IsNullOrEmpty(this.EndPointConfigurationName) && this.EndpointAddress == null)
                    configurationServiceProxy = new ConfigurationServiceProxy();
                else if (!String.IsNullOrEmpty(this.EndPointConfigurationName) && this.EndpointAddress == null)
                    configurationServiceProxy = new ConfigurationServiceProxy(this.EndPointConfigurationName);
                else if (!String.IsNullOrEmpty(this.EndPointConfigurationName) && this.EndpointAddress != null)
                    configurationServiceProxy = new ConfigurationServiceProxy(this.EndPointConfigurationName, this.EndpointAddress);

                if (this.AuthenticationType == Core.AuthenticationType.Forms)
                {
                    configurationServiceProxy.ClientCredentials.UserName.UserName = this.UserName;
                    configurationServiceProxy.ClientCredentials.UserName.Password = this.Password;
                    configurationServiceProxy.ClientCredentials.ServiceCertificate.Authentication.CertificateValidationMode = System.ServiceModel.Security.X509CertificateValidationMode.None;
                }

                serviceClient = configurationServiceProxy;
            }

            return serviceClient;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="client"></param>
        private void DisposeClient(IConfigurationService client)
        {
            if (client == null)
                return;

            if (client.GetType().Equals(typeof(ConfigurationServiceProxy)))
            {
                ConfigurationServiceProxy serviceClient = (ConfigurationServiceProxy)client;
                if (serviceClient.State == CommunicationState.Created || serviceClient.State == CommunicationState.Opened || serviceClient.State == CommunicationState.Opening)
                {
                    serviceClient.Close();
                }
                else if (serviceClient.State == CommunicationState.Faulted)
                {
                    serviceClient.Abort();
                }
            }
            else
            {
                //Do nothing...
            }
        }

        /// <summary>
        /// Makes the ConfigurationServiceClient call. Creates the client instance, executes call delegate against it in
        /// impersonated context (if necessary) and then disposes the client.
        /// Also emits traces when necessary.
        /// </summary>
        /// <typeparam name="TResult">Indicates type of the result of service call.</typeparam>
        /// <param name="clientMethodName">Indicates name of the client method to include in traces.</param>
        /// <param name="serverMethodName">Indicates name of the server method to include in traces.</param>
        /// <param name="call">Indicates call delegate to be executed against a client instance.</param>
        /// <param name="traceSource">Indicates name of the trace source</param>
        /// <returns>Returns value returned by service, or default.</returns>
        private TResult MakeServiceCall<TResult>(string clientMethodName, string serverMethodName, Func<IConfigurationService, TResult> call, MDMTraceSource traceSource = MDMTraceSource.General)
        {
            Boolean isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;
            //Start trace activity
            if (isTracingEnabled)
                MDMTraceHelper.StartTraceActivity("ConfigurationServiceClient." + clientMethodName, traceSource, false);

            TResult result = default(TResult);
            IConfigurationService configurationServiceClient = null;

            try
            {
                configurationServiceClient = GetClient();

                ValidateContext();

                if (isTracingEnabled)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Verbose, "ConfigurationServiceClient sends '" + serverMethodName + "' request message.", traceSource);

                result = Impersonate(() => call(configurationServiceClient));

                if (isTracingEnabled)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Verbose, "ConfigurationServiceClient receives '" + serverMethodName + "' response message.", traceSource);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            finally
            {
                //Close the client
                DisposeClient(configurationServiceClient);

                if (isTracingEnabled)
                    MDMTraceHelper.StopTraceActivity("ConfigurationServiceClient." + clientMethodName, traceSource);
            }

            return result;
        }

        #endregion
    }
}
