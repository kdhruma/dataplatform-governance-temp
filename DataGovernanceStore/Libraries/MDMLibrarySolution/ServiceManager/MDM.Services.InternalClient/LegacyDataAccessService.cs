using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.Security.Principal;
using System.ServiceModel;

namespace MDM.Services
{
    using MDM.BusinessObjects;
    using MDM.BusinessObjects.DynamicTableSchema;
    using MDM.BusinessObjects.Exports;
    using MDM.BusinessObjects.Jobs;
    using MDM.Core;
    using MDM.Interfaces;
    using MDM.Services.ServiceProxies;
    using MDM.Utility;

    /// <summary>
    /// Client consuming Legacy Data Service
    /// </summary>
    public class LegacyDataAccessService : ServiceClientBase
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
        public LegacyDataAccessService()
            : base(typeof(LegacyDataAccessService))
        {
        }

        /// <summary>
        /// Use this default constructor with binding configuration name only when calling MDM service within the execution context of an MDM Application. 
        /// Example: Service call from the MDM business rule.
        /// This would try to use current execution security context for service authentication purpose. 
        /// </summary>
        /// <param name="endPointConfigurationName">Indicates the EndPointConfigurationName</param>
        public LegacyDataAccessService(String endPointConfigurationName)
            : base(typeof(LegacyDataAccessService), endPointConfigurationName)
        {
        }

        /// <summary>
        /// Use this constructor for form based authentication passing username and password
        /// </summary>
        /// <param name="endPointConfigurationName">Indicates the EndPointConfigurationName</param>
        /// <param name="userName">Name of the Login User</param>
        /// <param name="password">Password of the User</param>
        public LegacyDataAccessService(String endPointConfigurationName, String userName, String password)
            : base(endPointConfigurationName, userName, password)
        {

        }

        /// <summary>
        /// Use this constructor for window authentication passing current windows identity.
        /// </summary>
        /// <param name="endPointConfigurationName">Indicates the EndPointConfigurationName</param>
        /// <param name="userIdentity">Indicates User's Identity</param>
        public LegacyDataAccessService(String endPointConfigurationName, IIdentity userIdentity)
            : base(endPointConfigurationName, userIdentity)
        {
        }

        /// <summary>
        /// Constructor with client configuration.
        /// </summary>
        /// <param name="wcfClientConfiguration">Indicates WCF Client Configuration</param>
        public LegacyDataAccessService(IWCFClientConfiguration wcfClientConfiguration)
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
        public LegacyDataAccessService(String endPointConfigurationName, AuthenticationType authenticationType, IIdentity userIdentity, String userName, String password)
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
        public LegacyDataAccessService(String endPointConfigurationName, EndpointAddress endPointAddress, AuthenticationType authenticationType, IIdentity userIdentity, String userName, String password)
            : base(endPointConfigurationName, endPointAddress, authenticationType, userIdentity, userName, password)
        {
        }

        #endregion

        #region Methods

        #region Private Methods

        /// <summary>
        /// Get Core Service Client
        /// </summary>
        /// <returns>ServiceClient with UerName and Password</returns>
        private MDM.WCFServiceInterfaces.ILegacyDataAccessService GetClient()
        {
            MDM.WCFServiceInterfaces.ILegacyDataAccessService legacyDataAccessServiceClient = null;

            if (WCFServiceInstanceLoader.IsLocalInstancesEnabled())
            {
                legacyDataAccessServiceClient = WCFServiceInstanceLoader.GetServiceInstance<MDM.WCFServiceInterfaces.ILegacyDataAccessService>();
            }

            if (legacyDataAccessServiceClient == null)
            {
                LegacyDataAccessServiceProxy legacyDataAccessServiceProxy = null;

                if (String.IsNullOrEmpty(this.EndPointConfigurationName) && this.EndpointAddress == null)
                    legacyDataAccessServiceProxy = new LegacyDataAccessServiceProxy();
                else if (!String.IsNullOrEmpty(this.EndPointConfigurationName) && this.EndpointAddress == null)
                    legacyDataAccessServiceProxy = new LegacyDataAccessServiceProxy(this.EndPointConfigurationName);
                else if (!String.IsNullOrEmpty(this.EndPointConfigurationName) && this.EndpointAddress != null)
                    legacyDataAccessServiceProxy = new LegacyDataAccessServiceProxy(this.EndPointConfigurationName, this.EndpointAddress);

                if (this.AuthenticationType == Core.AuthenticationType.Forms)
                {
                    legacyDataAccessServiceProxy.ClientCredentials.UserName.UserName = this.UserName;
                    legacyDataAccessServiceProxy.ClientCredentials.UserName.Password = this.Password;
                    legacyDataAccessServiceProxy.ClientCredentials.ServiceCertificate.Authentication.CertificateValidationMode = System.ServiceModel.Security.X509CertificateValidationMode.None;
                }

                legacyDataAccessServiceClient = legacyDataAccessServiceProxy;

                Boolean isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;
                if (isTracingEnabled)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("Client context for this request: IsDelegationEnabled-{0}; AuthenticationType-{1}; EndPointConfigurationName-{2}; UserName-{3}; UserIdentityName-{4}",
                    this.IsDelegationEnabled, this.AuthenticationType, this.EndPointConfigurationName, this.UserName, ((this.UserIdentity != null) ? this.UserIdentity.Name : string.Empty)));
            }

            return legacyDataAccessServiceClient;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="client"></param>
        private void DisposeClient(MDM.WCFServiceInterfaces.ILegacyDataAccessService client)
        {
            if (client == null)
                return;

            if (client.GetType().Equals(typeof(LegacyDataAccessServiceProxy)))
            {
                LegacyDataAccessServiceProxy serviceClient = (LegacyDataAccessServiceProxy)client;
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
        /// Makes the LegacyDataAccessServiceClient call. Creates the client instance, executes call delegate against it in
        /// impersonated context (if necessary) and then disposes the client.
        /// Also emits traces when necessary.
        /// </summary>
        /// <typeparam name="TResult">The type of the result of service call.</typeparam>
        /// <param name="clientMethodName">Name of the client method to include in traces.</param>
        /// <param name="serverMethodName">Name of the server method to include in traces.</param>
        /// <param name="call">The call delegate to be executed against a client instance.</param>
        /// <param name="traceSource">The type operation source for tracing</param>
        /// <returns>The value returned by service, or default.</returns>
        private TResult MakeServiceCall<TResult>(String clientMethodName, String serverMethodName, Func<MDM.WCFServiceInterfaces.ILegacyDataAccessService, TResult> call, MDMTraceSource traceSource = MDMTraceSource.General)
        {
            Boolean isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;
            if (isTracingEnabled)
            {
                //Start trace activity
                MDMTraceHelper.StartTraceActivity("LegacyDataAccesServiceClient." + clientMethodName, traceSource, false);
            }

            TResult result = default(TResult);
            MDM.WCFServiceInterfaces.ILegacyDataAccessService legacyDataAccessServiceClient = null;

            try
            {
                legacyDataAccessServiceClient = GetClient();

                ValidateContext();

                if (isTracingEnabled)
                {
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Verbose, "LegacyDataAccessServiceClient sends '" + serverMethodName + "' request message.", traceSource);
                }

                result = Impersonate(() => call(legacyDataAccessServiceClient));

                if (isTracingEnabled)
                {
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Verbose, "LegacyDataAccessServiceClient receives '" + serverMethodName + "' response message.", traceSource);
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            finally
            {
                //Close the client
                DisposeClient(legacyDataAccessServiceClient);

                if (isTracingEnabled)
                {
                    MDMTraceHelper.StopTraceActivity("EntityExportServiceClient." + clientMethodName, traceSource);
                }
            }

            return result;
        }

        #endregion Private Methods

        #region Public Methods

        #region Application Config

        /// <summary>
        /// Get Configuration data for the legacy calls
        /// </summary>
        /// <param name="eventSourceId">Indicates identifier of the event source</param>
        /// <param name="eventSubscriberId">Indicates identifier of the event subscriber</param>
        /// <param name="securityRoleId">Indicates identifier of security role</param>
        /// <param name="securityUserId">Indicates identifier of security user</param>
        /// <param name="orgId">Indicates identifier of organization</param>
        /// <param name="catalogId">Indicates identifier of catalog</param>
        /// <param name="categoryId">Indicates identifier of category</param>
        /// <param name="cNodeId">Indicates identifier of entity</param>
        /// <param name="attributeId">Indicates identifier of attribute</param>
        /// <param name="nodeTypeId">Indicates identifier of entity type</param>
        /// <param name="relationshipTypeId">Indicates identifier of relationship type</param>
        /// <param name="localId">Indicates an identifier of locale</param>
        /// <param name="applicationConfigId">Indicates identifier of application configuration</param>
        /// <param name="categoryPath">Indicates path of category</param>
        /// <param name="objectName">Indicates name of the object</param>
        /// <returns>Returns application configuration data in datatable format</returns>
        public DataTable GetApplicationConfigData(Int32 eventSourceId, Int32 eventSubscriberId, Int32 securityRoleId, Int32 securityUserId, Int32 orgId, Int32 catalogId, Int32 categoryId, Int32 cNodeId, Int32 attributeId, Int32 nodeTypeId, Int32 relationshipTypeId, Int32 localId, Int32 applicationConfigId, String categoryPath, String objectName)
        {

            return MakeServiceCall("GetApplicationConfigData", "GetApplicationConfigData", client =>
                      client.GetApplicationConfigData(eventSourceId, eventSubscriberId, securityRoleId, securityUserId, orgId, catalogId, categoryId, cNodeId, attributeId, nodeTypeId, relationshipTypeId, localId, applicationConfigId, categoryPath, objectName));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="FK_Application_ContextDefinition"></param>
        /// <param name="FK_Application_ConfigParent"></param>
        /// <param name="ShortName"></param>
        /// <param name="LongName"></param>
        /// <param name="FK_Event_Source"></param>
        /// <param name="FK_Event_Subscriber"></param>
        /// <param name="FK_Org"></param>
        /// <param name="FK_Catalog"></param>
        /// <param name="FK_Category"></param>
        /// <param name="FK_CNode"></param>
        /// <param name="FK_Attribute"></param>
        /// <param name="FK_NodeType"></param>
        /// <param name="FK_RelationshipType"></param>
        /// <param name="FK_Security_Role"></param>
        /// <param name="FK_Security_user"></param>
        /// <param name="ConfigXML"></param>
        /// <param name="Description"></param>
        /// <param name="PreCondition"></param>
        /// <param name="PostCondition"></param>
        /// <param name="XSDSchema"></param>
        /// <param name="SampleXML"></param>
        /// <param name="loginUser"></param>
        /// <param name="userProgram"></param>
        /// <param name="FK_Locale"></param>
        /// <returns></returns>
        public Int32 UpdateApplicationConfigXML(Int32 FK_Application_ContextDefinition, Int32 FK_Application_ConfigParent,
                    String ShortName, String LongName, Int32 FK_Event_Source, Int32 FK_Event_Subscriber, Int32 FK_Org, Int32 FK_Catalog,
                    Int32 FK_Category, Int32 FK_CNode, Int32 FK_Attribute, Int32 FK_NodeType, Int32 FK_RelationshipType, Int32 FK_Security_Role,
                    Int32 FK_Security_user, String ConfigXML, String Description, String PreCondition, String PostCondition, String XSDSchema,
                    String SampleXML, String loginUser, String userProgram, Int32 FK_Locale)
        {

            return MakeServiceCall("UpdateApplicationConfigXML", "UpdateApplicationConfigXML", client =>
                     client.UpdateApplicationConfigXML(FK_Application_ContextDefinition, FK_Application_ConfigParent,
                     ShortName, LongName, FK_Event_Source, FK_Event_Subscriber, FK_Org, FK_Catalog,
                     FK_Category, FK_CNode, FK_Attribute, FK_NodeType, FK_RelationshipType, FK_Security_Role,
                     FK_Security_user, ConfigXML, Description, PreCondition, PostCondition, XSDSchema,
                     SampleXML, loginUser, userProgram, FK_Locale));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="eventSourceId"></param>
        /// <param name="eventSubscriberId"></param>
        /// <returns></returns>
        public DataTable GetContextualApplicationConfigData(Int32 eventSourceId, Int32 eventSubscriberId)
        {
            return MakeServiceCall("GetContextualApplicationConfigData", "GetContextualApplicationConfigData", client =>
                      client.GetContextualApplicationConfigData(eventSourceId, eventSubscriberId));
        }

        #endregion

        #region Misc

        /// <summary>
        /// <param name="searchCriteriaID">Indicates an searchCriteriaID</param>
        /// <param name="loginId">Indicates an loginId</param>
        /// <param name="catalogId">Indicates an catalogId</param>
        /// </summary>
        /// <returns>Datatable</returns>
        public DataTable GetSearchCriterias(Int32 searchCriteriaID, Int32 loginId, Int32 catalogId)
        {
            return MakeServiceCall("GetSearchCriterias", "GetSearchCriterias", client => client.GetSearchCriterias(searchCriteriaID, loginId, catalogId));
        }

        /// <summary>
        /// Gets category navigation panel
        /// </summary>
        /// <param name="catalogID">Indicates identifier of catalog</param>
        /// <param name="sysAttrSelectionXml">Indicates system attribute selection xml</param>
        /// <param name="categorySearchColumn">Indicates name of the category search column</param>
        /// <param name="categorySearchString">Indicates value of the category search</param>
        /// <param name="parentCategoryId">Indicates identifier of parent category</param>
        /// <param name="countFrom">Indicates count which tells from where to start search</param>
        /// <param name="countTo">Indicates count which tells where to stop search</param>
        /// <param name="strvchrUserLogin">Indicates login name of the user</param>
        /// <param name="currentDataLocale">Indicates current data locale</param>
        /// <returns>Returns category navigation panel as datatable</returns>
        public DataTable GetCategoryNavPanel(Int32 catalogID, String sysAttrSelectionXml, String categorySearchColumn, String categorySearchString, Int64 parentCategoryId, Int32 countFrom, Int32 countTo, String strvchrUserLogin, Int32 currentDataLocale)
        {
            return MakeServiceCall("GetCategoryNavPanel", "GetCategoryNavPanel", client => client.GetCategoryNavPanel(catalogID, sysAttrSelectionXml, categorySearchColumn, categorySearchString, parentCategoryId,
                                           countFrom, countTo, strvchrUserLogin, currentDataLocale));
        }

        /// <summary>
        /// Process search criteria
        /// </summary>
        /// <param name="action">Indicates action like create,update or delete</param>
        /// <param name="criteriaId">Indicates identifier of criteria</param>
        /// <param name="criteriaName">Indicates name of criteria</param>
        /// <param name="loginId">Indicates identifier of login</param>
        /// <param name="catalogId">Indicates identifier of catalog</param>
        /// <param name="isGlobalSearch">Indicates true if search is global, otherwise false</param>
        /// <param name="searchXml">Indicates search xml</param>
        /// <param name="loginUser">Indicates login name of the user</param>
        /// <param name="userProgram">Indicates name of the program</param>
        /// <returns>Returns identifier of a criteria</returns>
        public int ProcessSearchCriteria(String action, Int32 criteriaId, String criteriaName, Int32 loginId, Int32 catalogId, Boolean isGlobalSearch, String searchXml, String loginUser, String userProgram)
        {
            return MakeServiceCall("ProcessSearchCriteria", "ProcessSearchCriteria", client => client.ProcessSearchCriteria(action, criteriaId, criteriaName, loginId, catalogId, isGlobalSearch, searchXml,
                                       loginUser, userProgram));
        }

        /// <summary>
        /// Gets all the categories
        /// </summary>
        /// <param name="altTaxId">Indicates identifier of hierarchy id</param>
        /// <param name="currentDataLocale">Indicates current data locale</param>
        /// <param name="filter">Indicates value of the search filter</param>
        /// <param name="loginUser">Indicates login name of the user</param>
        /// <returns>Returns datatable of categories</returns>
        public DataTable GetAllCategories(int altTaxId, int currentDataLocale, String filter, String loginUser)
        {
            return MakeServiceCall("GetAllCategories", "GetAllCategories", client => client.GetAllCategories(altTaxId, currentDataLocale, filter, loginUser));
        }

        /// <summary>
        /// Gets system attributes
        /// </summary>
        /// <param name="orgId">Indicates identifier of an organization</param>
        /// <param name="excludeSearchable">Indicates true if user wants to exclude searchable property of an attribute, otherwise false</param>
        /// <param name="currentDataLocale">Indicates current data locale</param>
        /// <returns>Returns string as a system attributes</returns>
        public String GetSystemAttributes(int orgId, Boolean excludeSearchable, Int32 currentDataLocale)
        {
            return MakeServiceCall("GetSystemAttributes", "GetSystemAttributes", client => client.GetSystemAttributes(orgId, excludeSearchable, currentDataLocale));
        }

        /// <summary>
        /// </summary>
        /// <returns>String</returns>
        public String GetCharacteristicTemplate(Int32 selectedCategoryId, Int32 catalogID, Int32 currentDataLocale, Boolean useDraftAccess, Int32 usesChilds, Int32 orgID, Boolean excludeSearchable)
        {
            return MakeServiceCall("GetCharacteristicTemplate", "GetCharacteristicTemplate", client => client.GetCharacteristicTemplate(selectedCategoryId, catalogID, currentDataLocale, useDraftAccess, usesChilds, orgID, excludeSearchable));
        }

        /// <summary>
        /// </summary>
        /// <returns>DataTable</returns>
        public DataTable GetBusinessRuleFromContext(String strContentXml)
        {
            return MakeServiceCall("GetBusinessRuleFromContext", "GetBusinessRuleFromContext", client => client.GetBusinessRuleFromContext(strContentXml));
        }

        /// <summary>
        /// </summary>
        /// <returns>DataTable</returns>
        public DataTable GetRuleViewAttributes(Int32 cNodeId, Int32 businessRuleId)
        {
            return MakeServiceCall("GetRuleViewAttributes", "GetRuleViewAttributes", client => client.GetRuleViewAttributes(cNodeId, businessRuleId));
        }

        /// <summary>
        /// </summary>
        /// <returns>Int32</returns>
        public Int32 DeleteSearchCriteria(Int32 searchCriteriaId, String loginUser, String modProgram)
        {
            return MakeServiceCall("DeleteSearchCriteria", "DeleteSearchCriteria", client => client.DeleteSearchCriteria(searchCriteriaId, loginUser, modProgram));
        }

        /// <summary>
        /// </summary>
        /// <returns>DataTable</returns>
        public DataTable GetUserVisibleOrgsDT(String targetLoginUser, String loginUser, Int32 localId, Int32 countFrom, Int32 countTo, String sortColumn, String searchColumn, String searchParameters)
        {
            return MakeServiceCall("GetUserVisibleOrgsDT", "GetUserVisibleOrgsDT", client => client.GetUserVisibleOrgsDT(targetLoginUser, loginUser, localId, countFrom,
                                                 countTo, sortColumn, searchColumn, searchParameters));
        }

        /// <summary>
        /// </summary>
        /// <returns>DataTable</returns>
        public DataTable GetUserVisibleCatalogsDT(String targetLoginUser, String loginUser, Int32 orgId, Int32 localeId, Int32 countFrom, Int32 countTo, String sortColumn, String searchColumn, String searchParameter, Int32 catalogId, Boolean includeTaxonomy, Boolean includeDynamicTaxonomy, Boolean includeCatalog, Boolean includeView, Boolean includeProduction, Boolean includeDraft)
        {
            return MakeServiceCall("GetUserVisibleCatalogsDT", "GetUserVisibleCatalogsDT", client =>
                                    client.GetUserVisibleCatalogsDT(targetLoginUser, loginUser, orgId, localeId, countFrom,
                                        countTo, sortColumn, searchColumn, searchParameter, catalogId, includeTaxonomy,
                                        includeDynamicTaxonomy, includeCatalog, includeView, includeProduction, includeDraft));
        }

        /// <summary>
        /// </summary>
        /// <returns>DataTable</returns>
        public DataTable GetAssignmentButtons(String assignmentStatus, String cNodeList, String toolbarButtonXml, String vchrUserLogin)
        {
            return MakeServiceCall("GetAssignmentButtons", "GetAssignmentButtons", client => client.GetAssignmentButtons(assignmentStatus, cNodeList, toolbarButtonXml, vchrUserLogin));
        }

        /// <summary>
        /// Gets collection of attribute versions
        /// </summary>
        /// <param name="entityId">Indicates identifier of an entity</param>
        /// <param name="attributeId">Indicates identifier of an attribute</param>
        /// <param name="localeId">Indicates identifier of locale</param>
        /// <param name="catalogId">Indicates identifier of a catalog</param>
        /// <param name="entityParentId">Indicates parent identifier of an entity</param>
        /// <param name="callerContext">Indicates the name of application and the module that are performing the action</param>
        /// <returns>Returns collection of attribute versions</returns>
        public AttributeVersionCollection Get(Int64 entityId, Int32 attributeId, Int32 localeId, Int32 catalogId, Int64 entityParentId, CallerContext callerContext)
        {
            return MakeServiceCall("Get", "Get", client => client.Get(entityId, attributeId, localeId, catalogId, entityParentId, FillDiagnosticTraces(callerContext)));
        }

        /// <summary>
        /// Gets breadcrumb attribute value
        /// </summary>
        /// <param name="breadcrumbConfigXML">Indicates breadcrumb configuration xml</param>
        /// <param name="fkOrgId">Indicates identifier of an organization</param>
        /// <param name="catalogId">Indicates identifier of a catalog</param>
        /// <param name="cNodeId">Indicates identifier of an entity</param>
        /// <param name="localeId">Indicates identifier of locale</param>
        /// <returns>Returns datatable of breadcrumb attribute value</returns>
        public DataTable GetBreadcrumbAttributeValueString(String breadcrumbConfigXML, Int32 fkOrgId, Int32 catalogId, Int64 cNodeId, Int32 localeId)
        {
            return MakeServiceCall("GetBreadcrumbAttributeValueString", "GetBreadcrumbAttributeValueString", client => client.GetBreadcrumbAttributeValueString(breadcrumbConfigXML, fkOrgId, catalogId, cNodeId, localeId));
        }

        /// <summary>
        /// Process service result based on given input parameters
        /// </summary>
        /// <param name="eventSource">Indicates identifier of an event source</param>
        /// <param name="applicationConfig">Indicates identifier of an application configuration</param>
        /// <param name="dataXmlString">Indicates data in xml format</param>
        /// <param name="loginUser">Indicates login name of the user</param>
        /// <param name="userProgram">Indicates program name</param>
        public void ProcessServiceResult(Int32 eventSource, Int32 applicationConfig, String dataXmlString, String loginUser, String userProgram)
        {

            MakeServiceCall("ProcessServiceResult", "ProcessServiceResult", client => client.ProcessServiceResult(eventSource, applicationConfig, dataXmlString, loginUser, userProgram));
        }

        /// <summary>
        /// Gets mappings based on given job type and event source
        /// </summary>
        /// <param name="eventSourceId">Indicates identifier of event source</param>
        /// <param name="jobId">Indicates identifier of job</param>
        /// <returns>Returns datatable of mappings of job type and event source</returns>
        public DataTable GetJobTypeEventSourceMapping(Int32 eventSourceId, Int32 jobId)
        {
            return MakeServiceCall("GetJobTypeEventSourceMapping", "GetJobTypeEventSourceMapping", client => client.GetJobTypeEventSourceMapping(eventSourceId, jobId));
        }

        #endregion

        #region Catalog

        /// <summary>
        /// 
        /// </summary>
        /// <param name="FK_Catalog"></param>
        /// <param name="FK_RelationshipType_Top"></param>
        /// <param name="MaxLevel"></param>
        /// <returns></returns>
        public DataTable GetRelationshipTypeHierarchy(int FK_Catalog, int FK_RelationshipType_Top, int MaxLevel)
        {
            return MakeServiceCall("GetRelationshipTypeHierarchy", "GetRelationshipTypeHierarchy", client => client.GetRelationshipTypeHierarchy(FK_Catalog, FK_RelationshipType_Top, MaxLevel));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="intExtSystemID"></param>
        /// <param name="txtXML"></param>
        /// <param name="vchrRelAttrList"></param>
        /// <returns></returns>
        public string ExtractCatalogByIDLocalRel(int intExtSystemID, string txtXML, string vchrRelAttrList)
        {
            return MakeServiceCall("ExtractCatalogByIDLocalRel", "ExtractCatalogByIDLocalRel", client => client.ExtractCatalogByIDLocalRel(intExtSystemID, txtXML, vchrRelAttrList));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vchrTargetUserLogin"></param>
        /// <param name="vchrUserLogin"></param>
        /// <param name="txtXML"></param>
        /// <param name="bitUseDraftTax"></param>
        /// <param name="localeId"></param>
        /// <param name="ignoreComplexAttributes"></param>
        /// <returns></returns>
        public string ExtractBulkAttributeMetadata(string vchrTargetUserLogin, string vchrUserLogin, string txtXML, bool bitUseDraftTax, int localeId, bool ignoreComplexAttributes)
        {
            return MakeServiceCall("ExtractBulkAttributeMetadata", "ExtractBulkAttributeMetadata", client => client.ExtractBulkAttributeMetadata(vchrTargetUserLogin, vchrUserLogin, txtXML, bitUseDraftTax, localeId, ignoreComplexAttributes));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vchrTargetUserLogin"></param>
        /// <param name="vchrUserLogin"></param>
        /// <param name="txtXML"></param>
        /// <param name="bitUseDraftTax"></param>
        /// <param name="localeId"></param>
        /// <returns></returns>
        public string ExtractBulkAttributeMetadataRel(string vchrTargetUserLogin, string vchrUserLogin, string txtXML, bool bitUseDraftTax, int localeId)
        {
            return MakeServiceCall("ExtractBulkAttributeMetadataRel", "ExtractBulkAttributeMetadataRel", client => client.ExtractBulkAttributeMetadataRel(vchrTargetUserLogin, vchrUserLogin, txtXML, bitUseDraftTax, localeId));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vchrTargetUserLogin"></param>
        /// <param name="vchrUserLogin"></param>
        /// <param name="txtXML"></param>
        /// <param name="bitUseDraftTax"></param>
        /// <returns></returns>
        public string ExtractAttributes(string vchrTargetUserLogin, string vchrUserLogin, string txtXML, bool bitUseDraftTax)
        {
            return MakeServiceCall("ExtractAttributes", "ExtractAttributes", client => client.ExtractAttributes(vchrTargetUserLogin, vchrUserLogin, txtXML, bitUseDraftTax));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="intCategoryID"></param>
        /// <param name="intCatalogID"></param>
        /// <param name="intLocaleID"></param>
        /// <returns></returns>
        public DataTable GetCategoryAttributeMap(int intCategoryID, int intCatalogID, int intLocaleID)
        {
            return MakeServiceCall("GetCategoryAttributeMap", "GetCategoryAttributeMap", client => client.GetCategoryAttributeMap(intCategoryID, intCatalogID, intLocaleID));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="intCategoryID"></param>
        /// <param name="intLocaleID"></param>
        /// <returns></returns>
        public DataTable GetCharacteristicTemplateDT(int intCategoryID, int intLocaleID)
        {
            return MakeServiceCall("GetCharacteristicTemplateDT", "GetCharacteristicTemplateDT", client => client.GetCharacteristicTemplateDT(intCategoryID, intLocaleID));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vchrTargetUserLogin"></param>
        /// <param name="vchrUserLogin"></param>
        /// <param name="FK_Org"></param>
        /// <param name="FK_Locale"></param>
        /// <param name="intCountFrom"></param>
        /// <param name="intCountTo"></param>
        /// <param name="vchrSortColumn"></param>
        /// <param name="vchrSearchColumn"></param>
        /// <param name="vchrSearchParameter"></param>
        /// <param name="PK_Catalog"></param>
        /// <param name="IncludeTaxonomy"></param>
        /// <param name="IncludeDynamicTaxonomy"></param>
        /// <param name="IncludeCatalog"></param>
        /// <param name="IncludeView"></param>
        /// <param name="IncludeProduction"></param>
        /// <param name="IncludeDraft"></param>
        /// <returns></returns>
        public string GetUserVisibleCatalogs(string vchrTargetUserLogin, string vchrUserLogin, int FK_Org, int FK_Locale, int intCountFrom, int intCountTo, string vchrSortColumn, string vchrSearchColumn, string vchrSearchParameter, int PK_Catalog, bool IncludeTaxonomy, bool IncludeDynamicTaxonomy, bool IncludeCatalog, bool IncludeView, bool IncludeProduction, bool IncludeDraft)
        {
            return MakeServiceCall("GetUserVisibleCatalogs", "GetUserVisibleCatalogs", client => client.GetUserVisibleCatalogs(vchrTargetUserLogin, vchrUserLogin, FK_Org, FK_Locale, intCountFrom, intCountTo, vchrSortColumn, vchrSearchColumn, vchrSearchParameter, PK_Catalog, IncludeTaxonomy, IncludeDynamicTaxonomy, IncludeCatalog, IncludeView, IncludeProduction, IncludeDraft));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vchrTargetUserLogin"></param>
        /// <param name="vchrUserLogin"></param>
        /// <param name="FK_Org"></param>
        /// <param name="FK_Locale"></param>
        /// <param name="intCountFrom"></param>
        /// <param name="intCountTo"></param>
        /// <param name="vchrSortColumn"></param>
        /// <param name="vchrSearchColumn"></param>
        /// <param name="vchrSearchParameter"></param>
        /// <param name="PK_Catalog"></param>
        /// <param name="IncludeTaxonomy"></param>
        /// <param name="IncludeDynamicTaxonomy"></param>
        /// <param name="IncludeCatalog"></param>
        /// <param name="IncludeView"></param>
        /// <param name="IncludeProduction"></param>
        /// <param name="IncludeDraft"></param>
        /// <returns></returns>
        public string GetCatalogPermissionsByOrg(string vchrTargetUserLogin, string vchrUserLogin, int FK_Org, int FK_Locale, int intCountFrom, int intCountTo, string vchrSortColumn, string vchrSearchColumn, string vchrSearchParameter, int PK_Catalog, bool IncludeTaxonomy, bool IncludeDynamicTaxonomy, bool IncludeCatalog, bool IncludeView, bool IncludeProduction, bool IncludeDraft)
        {
            return MakeServiceCall("GetCatalogPermissionsByOrg", "GetCatalogPermissionsByOrg", client => client.GetCatalogPermissionsByOrg(vchrTargetUserLogin, vchrUserLogin, FK_Org, FK_Locale, intCountFrom, intCountTo, vchrSortColumn, vchrSearchColumn, vchrSearchParameter, PK_Catalog, IncludeTaxonomy, IncludeDynamicTaxonomy, IncludeCatalog, IncludeView, IncludeProduction, IncludeDraft));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vchrTargetUserLogin"></param>
        /// <param name="vchrUserLogin"></param>
        /// <param name="FK_Org"></param>
        /// <param name="FK_Locale"></param>
        /// <param name="intCountFrom"></param>
        /// <param name="intCountTo"></param>
        /// <param name="vchrSortColumn"></param>
        /// <param name="vchrSearchColumn"></param>
        /// <param name="vchrSearchParameter"></param>
        /// <param name="PK_Catalog"></param>
        /// <param name="IncludeTaxonomy"></param>
        /// <param name="IncludeDynamicTaxonomy"></param>
        /// <param name="IncludeCatalog"></param>
        /// <param name="IncludeView"></param>
        /// <param name="IncludeProduction"></param>
        /// <param name="IncludeDraft"></param>
        /// <returns></returns>
        public DataTable GetCatalogDT(string vchrTargetUserLogin, string vchrUserLogin, int FK_Org, int FK_Locale, int intCountFrom, int intCountTo, string vchrSortColumn, string vchrSearchColumn, string vchrSearchParameter, int PK_Catalog, bool IncludeTaxonomy, bool IncludeDynamicTaxonomy, bool IncludeCatalog, bool IncludeView, bool IncludeProduction, bool IncludeDraft)
        {
            return MakeServiceCall("GetCatalogDT", "GetCatalogDT", client => client.GetCatalogDT(vchrTargetUserLogin, vchrUserLogin, FK_Org, FK_Locale, intCountFrom, intCountTo, vchrSortColumn, vchrSearchColumn, vchrSearchParameter, PK_Catalog, IncludeTaxonomy, IncludeDynamicTaxonomy, IncludeCatalog, IncludeView, IncludeProduction, IncludeDraft));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="orgId"></param>
        /// <param name="vchrTargetUserLogin"></param>
        /// <param name="vchrUserLogin"></param>
        /// <returns></returns>
        public string GetCatalogsByOrg(string orgId, string vchrTargetUserLogin, string vchrUserLogin)
        {
            return MakeServiceCall("GetCatalogsByOrg", "GetCatalogsByOrg", client => client.GetCatalogsByOrg(orgId, vchrTargetUserLogin, vchrUserLogin));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="txtXML"></param>
        /// <param name="PK_Org"></param>
        /// <param name="vchrUserLogin"></param>
        /// <returns></returns>
        public DataTable ProcessCatalogs(string txtXML, int PK_Org, string vchrUserLogin)
        {
            return MakeServiceCall("ProcessCatalogs", "ProcessCatalogs", client => client.ProcessCatalogs(txtXML, PK_Org, vchrUserLogin));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="PK_Catalog"></param>
        /// <param name="vchrUserLogin"></param>
        /// <returns></returns>
        public string GetCatalogLocaleByID(int PK_Catalog, string vchrUserLogin)
        {
            return MakeServiceCall("GetCatalogLocaleByID", "GetCatalogLocaleByID", client => client.GetCatalogLocaleByID(PK_Catalog, vchrUserLogin));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="txtXML"></param>
        /// <param name="PK_Org"></param>
        /// <param name="vchrUserLogin"></param>
        /// <returns></returns>
        public Boolean ProcessCatalogLocales(string txtXML, int PK_Org, string vchrUserLogin)
        {
            return MakeServiceCall("ProcessCatalogLocales", "ProcessCatalogLocales", client => client.ProcessCatalogLocales(txtXML, PK_Org, vchrUserLogin));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vchrTargetUserLogin"></param>
        /// <param name="vchrUserLogin"></param>
        /// <param name="PK_Catalog"></param>
        /// <param name="FK_ParentCNode"></param>
        /// <param name="FK_Locale"></param>
        /// <param name="FK_Customer"></param>
        /// <param name="intCountFrom"></param>
        /// <param name="intCountTo"></param>
        /// <param name="vchrSortColumn"></param>
        /// <param name="vchrSearchColumn"></param>
        /// <param name="vchrSearchParameter"></param>
        /// <param name="PK_CNode"></param>
        /// <param name="bitIncludeComponents"></param>
        /// <param name="bitEnableComponentMapping"></param>
        /// <param name="bitUseDrafTax"></param>
        /// <param name="bitEnableUnassignedCategory"></param>
        /// <param name="ToolTipAttributeList"></param>
        /// <returns></returns>
        public string GetNodePermissions(string vchrTargetUserLogin, string vchrUserLogin, int PK_Catalog, int FK_ParentCNode, int FK_Locale, int FK_Customer, int intCountFrom, int intCountTo, string vchrSortColumn, string vchrSearchColumn, string vchrSearchParameter, int PK_CNode, bool bitIncludeComponents, bool bitEnableComponentMapping, bool bitUseDrafTax, bool bitEnableUnassignedCategory, string ToolTipAttributeList)
        {
            return MakeServiceCall("GetNodePermissions", "GetNodePermissions", client => client.GetNodePermissions(vchrTargetUserLogin, vchrUserLogin, PK_Catalog, FK_ParentCNode, FK_Locale, FK_Customer, intCountFrom, intCountTo, vchrSortColumn, vchrSearchColumn, vchrSearchParameter, PK_CNode, bitIncludeComponents, bitEnableComponentMapping, bitUseDrafTax, bitEnableUnassignedCategory, ToolTipAttributeList));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vchrTargetUserLogin"></param>
        /// <param name="vchrUserLogin"></param>
        /// <param name="PK_Catalog"></param>
        /// <param name="CnodeId"></param>
        /// <param name="FK_Locale"></param>
        /// <param name="FK_Customer"></param>
        /// <param name="intCountFrom"></param>
        /// <param name="intCountTo"></param>
        /// <param name="vchrSortColumn"></param>
        /// <param name="vchrSearchColumn"></param>
        /// <param name="vchrSearchParameter"></param>
        /// <param name="bitIncludeComponents"></param>
        /// <param name="bitEnableComponentMapping"></param>
        /// <param name="bitUseDrafTax"></param>
        /// <param name="bitEnableUnassignedCategory"></param>
        /// <returns></returns>
        public string GetNodePermissionsByCNode(string vchrTargetUserLogin, string vchrUserLogin, int PK_Catalog, int CnodeId, int FK_Locale, int FK_Customer, int intCountFrom, int intCountTo, string vchrSortColumn, string vchrSearchColumn, string vchrSearchParameter, bool bitIncludeComponents, bool bitEnableComponentMapping, bool bitUseDrafTax, bool bitEnableUnassignedCategory)
        {
            return MakeServiceCall("GetNodePermissionsByCNode", "GetNodePermissionsByCNode", client => client.GetNodePermissionsByCNode(vchrTargetUserLogin, vchrUserLogin, PK_Catalog, CnodeId, FK_Locale, FK_Customer, intCountFrom, intCountTo, vchrSortColumn, vchrSearchColumn, vchrSearchParameter, bitIncludeComponents, bitEnableComponentMapping, bitUseDrafTax, bitEnableUnassignedCategory));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="intLocaleID"></param>
        /// <param name="intCustomerID"></param>
        /// <param name="intCNodeID"></param>
        /// <param name="intCNodeParentID"></param>
        /// <param name="intGroupID"></param>
        /// <param name="intCatalogID"></param>
        /// <param name="intOrgID"></param>
        /// <param name="vchrUserID"></param>
        /// <param name="intBackLocaleID"></param>
        /// <param name="vchrViewPath"></param>
        /// <param name="bitUseDraftTax"></param>
        /// <param name="ShowAtCreation"></param>
        /// <param name="AttrIDList"></param>
        /// <returns></returns>
        public string GetCoreAttrByGroup(int intLocaleID, int intCustomerID, long intCNodeID, long intCNodeParentID, int intGroupID, int intCatalogID, int intOrgID, string vchrUserID, int intBackLocaleID, string vchrViewPath, bool bitUseDraftTax, bool ShowAtCreation, string AttrIDList)
        {
            return MakeServiceCall("GetCoreAttrByGroup", "GetCoreAttrByGroup", client => client.GetCoreAttrByGroup(intLocaleID, intCustomerID, intCNodeID, intCNodeParentID, intGroupID, intCatalogID, intOrgID, vchrUserID, intBackLocaleID, vchrViewPath, bitUseDraftTax, ShowAtCreation, AttrIDList));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="txtXML"></param>
        /// <param name="intCatalogID"></param>
        /// <param name="intOrgID"></param>
        /// <param name="vchrUserID"></param>
        /// <param name="vchrProgramName"></param>
        /// <param name="LocaleId"></param>
        /// <returns></returns>
        public Int64 ProcessCoreAttr(string txtXML, int intCatalogID, int intOrgID, string vchrUserID, string vchrProgramName, int LocaleId)
        {
            return MakeServiceCall("ProcessCoreAttr", "ProcessCoreAttr", client => client.ProcessCoreAttr(txtXML, intCatalogID, intOrgID, vchrUserID, vchrProgramName, LocaleId));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="txtXML"></param>
        /// <param name="intCatalogID"></param>
        /// <param name="intOrgID"></param>
        /// <param name="vchrUserID"></param>
        /// <param name="vchrProgramName"></param>
        /// <param name="LocaleId"></param>
        /// <returns></returns>
        public Int64 ProcessTechAttr(string txtXML, int intCatalogID, int intOrgID, string vchrUserID, string vchrProgramName, int LocaleId)
        {
            return MakeServiceCall("ProcessTechAttr", "ProcessTechAttr", client => client.ProcessTechAttr(txtXML, intCatalogID, intOrgID, vchrUserID, vchrProgramName, LocaleId));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="intCnodeID"></param>
        /// <param name="intCnodeParentID"></param>
        /// <param name="intCatalogID"></param>
        /// <param name="intGroupID"></param>
        /// <param name="intLocaleID"></param>
        /// <param name="intCustomerID"></param>
        /// <param name="vchrUserID"></param>
        /// <param name="intBackupLocaleID"></param>
        /// <param name="vchrViewPath"></param>
        /// <param name="bitUseDraftTax"></param>
        /// <param name="AttrIDList"></param>
        /// <returns></returns>
        public string GetTechAttr(int intCnodeID, int intCnodeParentID, int intCatalogID, int intGroupID, int intLocaleID, int intCustomerID, string vchrUserID, int intBackupLocaleID, string vchrViewPath, bool bitUseDraftTax, string AttrIDList)
        {
            return MakeServiceCall("GetTechAttr", "GetTechAttr", client => client.GetTechAttr(intLocaleID, intCnodeParentID, intCatalogID, intGroupID, intLocaleID, intCustomerID, vchrUserID, intBackupLocaleID, vchrViewPath, bitUseDraftTax, AttrIDList));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public DataTable GetStatuses()
        {
            return MakeServiceCall("GetStatuses", "GetStatuses", client => client.GetStatuses());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="JobId"></param>
        /// <param name="UserID"></param>
        public void SchemaValidationRulesExecution(int JobId, string UserID)
        {
            MakeServiceCall("SchemaValidationRulesExecution", "SchemaValidationRulesExecution", client => client.SchemaValidationRulesExecution(JobId, UserID));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="UserLogin"></param>
        /// <returns></returns>
        public DataSet GetCatalogAttributes(string UserLogin)
        {
            return MakeServiceCall("GetCatalogAttributes", "GetCatalogAttributes", client => client.GetCatalogAttributes(UserLogin));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="intFromCatalogId"></param>
        /// <param name="intToCatalogId"></param>
        /// <param name="CreateProgram"></param>
        /// <param name="CreateUser"></param>
        /// <returns></returns>
        public Boolean CopyCatalogAttributes(int intFromCatalogId, int intToCatalogId, string CreateProgram, string CreateUser)
        {
            return MakeServiceCall("CopyCatalogAttributes", "CopyCatalogAttributes", client => client.CopyCatalogAttributes(intFromCatalogId, intToCatalogId, CreateProgram, CreateUser));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="IdList"></param>
        /// <returns></returns>
        public DataTable GetNameValCollection(string IdList)
        {
            return MakeServiceCall("GetNameValCollection", "GetNameValCollection", client => client.GetNameValCollection(IdList));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="FK_CNode"></param>
        /// <param name="ParentId"></param>
        /// <param name="FK_Catalog"></param>
        /// <param name="FK_Customer"></param>
        /// <param name="FK_Locale"></param>
        /// <param name="InheritanceMode"></param>
        /// <returns></returns>
        public string GetCollectionValues(int FK_CNode, int ParentId, int FK_Catalog, int FK_Customer, int FK_Locale, string InheritanceMode)
        {
            return MakeServiceCall("GetCollectionValues", "GetCollectionValues", client => client.GetCollectionValues(FK_CNode, ParentId, FK_Catalog, FK_Customer, FK_Locale, InheritanceMode));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="intOrgID"></param>
        /// <param name="intCatalogID"></param>
        /// <param name="nvchrNodeType"></param>
        /// <param name="intBranchLevel"></param>
        /// <param name="IncludeComplexAttrChildren"></param>
        /// <param name="ExcludeSearchable"></param>
        /// <returns></returns>
        public string GetCatalogNodeTypeAttributesXML(int intOrgID, int intCatalogID, string nvchrNodeType, int intBranchLevel, bool IncludeComplexAttrChildren, bool ExcludeSearchable)
        {
            return MakeServiceCall("GetCatalogNodeTypeAttributesXML", "GetCatalogNodeTypeAttributesXML", client => client.GetCatalogNodeTypeAttributesXML(intOrgID, intCatalogID, nvchrNodeType, intBranchLevel, IncludeComplexAttrChildren, ExcludeSearchable));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vchrSearchValue"></param>
        /// <param name="intCatalogID"></param>
        /// <param name="intParentID"></param>
        /// <param name="toolTipAttributeList"></param>
        /// <param name="vchrUserLogin"></param>
        /// <param name="dataLocale"></param>
        /// <returns></returns>
        public string GetSearchCategoriesByCriteria(string vchrSearchValue, int intCatalogID, int intParentID, string toolTipAttributeList, string vchrUserLogin, int dataLocale)
        {
            return MakeServiceCall("GetSearchCategoriesByCriteria", "GetSearchCategoriesByCriteria", client => client.GetSearchCategoriesByCriteria(vchrSearchValue, intCatalogID, intParentID, toolTipAttributeList, vchrUserLogin, dataLocale));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vchrTargetUserLogin"></param>
        /// <param name="vchrUserLogin"></param>
        /// <param name="intOrgId"></param>
        /// <param name="intCatalogId"></param>
        /// <param name="intNodeId"></param>
        /// <param name="bitRecursive"></param>
        /// <param name="bitUseDraftTaxonomy"></param>
        /// <returns></returns>
        public string GetVisibleComponents(string vchrTargetUserLogin, string vchrUserLogin, int intOrgId, int intCatalogId, int intNodeId, bool bitRecursive, bool bitUseDraftTaxonomy)
        {
            return MakeServiceCall("GetVisibleComponents", "GetVisibleComponents", client => client.GetVisibleComponents(vchrTargetUserLogin, vchrUserLogin, intOrgId, intCatalogId, intNodeId, bitRecursive, bitUseDraftTaxonomy));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="txtXML"></param>
        /// <param name="intCatalogID"></param>
        /// <param name="intOrgID"></param>
        /// <param name="vchrUserID"></param>
        /// <param name="vchrProgramName"></param>
        /// <returns></returns>
        public Int32 ProcessRelAttr(string txtXML, int intCatalogID, int intOrgID, string vchrUserID, string vchrProgramName)
        {
            return MakeServiceCall("ProcessRelAttr", "ProcessRelAttr", client => client.ProcessRelAttr(txtXML, intCatalogID, intOrgID, vchrUserID, vchrProgramName));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="configXml"></param>
        /// <returns></returns>
        public DataSet GetUserConfigMetadata(string configXml)
        {
            return MakeServiceCall("GetUserConfigMetadata", "GetUserConfigMetadata", client => client.GetUserConfigMetadata(configXml));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="catalogId"></param>
        /// <param name="searchValue"></param>
        /// <param name="cnodeId"></param>
        /// <param name="userLogin"></param>
        /// <returns></returns>
        public DataTable QuickSearchByShortName(Int32 catalogId, String searchValue, Int32 cnodeId, String userLogin)
        {
            return MakeServiceCall("QuickSearchByShortName", "QuickSearchByShortName", client => client.QuickSearchByShortName(catalogId, searchValue, cnodeId, userLogin));
        }
        #endregion

        #region Events

        /// <summary>
        /// 
        /// </summary>
        /// <param name="eventSourceId"></param>
        /// <param name="eventSourceName"></param>
        /// <returns></returns>
        public DataTable GetEventSources(int eventSourceId, string eventSourceName)
        {
            return MakeServiceCall("GetEventSources", "GetEventSources", client => client.GetEventSources(eventSourceId, eventSourceName));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="eventSubscriberId"></param>
        /// <param name="eventSubscriberName"></param>
        /// <returns></returns>
        public DataTable GetEventSubscribers(int eventSubscriberId, string eventSubscriberName)
        {
            return MakeServiceCall("GetEventSubscribers", "GetEventSubscribers", client => client.GetEventSubscribers(eventSubscriberId, eventSubscriberName));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="applicationConfigTypeId"></param>
        /// <param name="applicationConfigTypeName"></param>
        /// <returns></returns>
        public DataTable GetApplicationConfigTypes(int applicationConfigTypeId, string applicationConfigTypeName)
        {
            return MakeServiceCall("GetApplicationConfigTypes", "GetApplicationConfigTypes", client => client.GetApplicationConfigTypes(applicationConfigTypeId, applicationConfigTypeName));
        }

        /// <summary>
        /// Gets matching rule sets
        /// </summary>
        /// <param name="orgId">Indicates identifier of organization</param>
        /// <param name="catalogId">Indicates identifier of catalog</param>
        /// <param name="eventSourceId">Indicates identifier of event source</param>
        /// <param name="eventSubscriberId">Indicates identifier of event subscriber</param>
        /// <param name="securityUserId">Indicates identifier of security user</param>
        /// <returns>Returns matching rule sets in datatable format</returns>
        public DataTable GetMatchingRuleSets(Int32 orgId, Int32 catalogId, Int32 eventSourceId, Int32 eventSubscriberId, Int32 securityUserId)
        {
            return MakeServiceCall("GetMatchingRuleSets", "GetMatchingRuleSets", client => client.GetMatchingRuleSets(orgId, catalogId, eventSourceId, eventSubscriberId, securityUserId));
        }
        #endregion

        #region Rules

        /// <summary>
        /// Gets view context
        /// </summary>
        /// <param name="viewId">Indicates identifier of view context</param>
        /// <returns>Returns view context in datatable format</returns>
        public DataTable GetViewContext(int viewId)
        {
            return MakeServiceCall("GetViewContext", "GetViewContext", client => client.GetViewContext(viewId));
        }

        /// <summary>
        /// Gets view context details
        /// </summary>
        /// <param name="contextId">Indicates identifier of context</param>
        /// <param name="loginUser">Indicates login name of user</param>
        /// <returns>Returns view context details in dataset format</returns>
        public DataSet GetViewContextDetail(int contextId, string loginUser)
        {
            return MakeServiceCall("GetViewContextDetail", "GetViewContextDetail", client => client.GetViewContextDetail(contextId, loginUser));

        }

        #endregion

        #region Matching

        /// <summary>
        /// Creates service job
        /// </summary>
        /// <param name="xml">indicates xml for service job which needs to create</param>
        /// <param name="userId">Indicates identifier of user</param>
        /// <param name="serviceType">Indicates type of service</param>
        /// <returns>Returns true is service job created successfully, otherwise false</returns>
        public string CreateServicesJob(string xml, string userId, int serviceType)
        {
            return MakeServiceCall("CreateServicesJob", "CreateServicesJob", client => client.CreateServicesJob(xml, userId, serviceType));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cNodes"></param>
        /// <param name="catalogId"></param>
        /// <returns></returns>
        public DataTable GetRSPLMatchingStatus(string cNodes, int catalogId)
        {
            return MakeServiceCall("GetRSPLMatchingStatus", "GetRSPLMatchingStatus", client => client.GetRSPLMatchingStatus(cNodes, catalogId));
        }

        /// <summary>
        /// Get matched nodes in datatable format
        /// </summary>
        /// <param name="nodeName">Indicates name of the node which needs to match</param>
        /// <param name="dataXML">Indicates data of the node in xml format</param>
        /// <returns>Returns matched nodes in datatable format</returns>
        public DataTable GetMatchedCnodes(string nodeName, string dataXML)
        {
            return MakeServiceCall("GetMatchedCnodes", "GetMatchedCnodes", client => client.GetMatchedCnodes(nodeName, dataXML));
        }

        #endregion

        #region BulkOperation
        /// <summary>
        /// 
        /// </summary>
        /// <param name="targetUserLogin"></param>
        /// <param name="userLogin"></param>
        /// <param name="inputDataMode"></param>
        /// <param name="selectedNodeTypes"></param>
        /// <param name="txtXML"></param>
        /// <param name="bitUseDraftTax"></param>
        /// <returns></returns>
        public string ExtractBulkOperationAttributeMetaData(string targetUserLogin, string userLogin, string inputDataMode, string selectedNodeTypes, string txtXML, bool bitUseDraftTax)
        {
            return MakeServiceCall("ExtractBulkOperationAttributeMetaData", "ExtractBulkOperationAttributeMetaData", client => client.ExtractBulkOperationAttributeMetaData(targetUserLogin, userLogin, inputDataMode, selectedNodeTypes, txtXML, bitUseDraftTax));
        }
        #endregion

        #region Report
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public DataTable GetAllLocales()
        {
            return MakeServiceCall("GetAllLocales", "GetAllLocales", client => client.GetAllLocales());
        }
        #endregion

        #region Hierarchy

        /// <summary>
        /// Gets list of categories by given hierarchy id and other input parameters
        /// </summary>
        /// <param name="localeId">Indicates identifier of locale</param>
        /// <param name="taxonomyId">Indicates identifier of hierarchy</param>
        /// <param name="searchParameter">Indicates search parameter</param>
        /// <param name="countTo">Indicates number of count which should return as a result</param>
        /// <returns>Returns list of categories</returns>
        public Collection<Entity> GetAllCategoriesByHierarchy(int localeId, int taxonomyId, string searchParameter, int countTo)
        {
            return MakeServiceCall("GetAllCategoriesByHierarchy", "GetAllCategoriesByHierarchy", client => client.GetAllCategoriesByHierarchy(localeId, taxonomyId, searchParameter, countTo));
        }

        #endregion

        #region Knowledge
        /// <summary>
        /// </summary>
        /// <returns>Collection of TimeZone</returns>
        public Collection<TimeZone> GetAll()
        {
            return MakeServiceCall("GetAll", "GetAll", client => client.GetAll());
        }

        /// <summary>
        /// </summary>
        /// <returns>String</returns>
        public String GetUnitsXML()
        {
            return MakeServiceCall("GetUnitsXML", "GetUnitsXML", client => client.GetUnitsXML());
        }
        #endregion

        #region Relationships

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityId"></param>
        /// <param name="iEntityContext"></param>
        /// <param name="iCallerContext"></param>
        /// <param name="publishEvents"></param>
        /// <param name="applyAVS"></param>
        /// <returns></returns>
        public IEntity GetEntityRelationshipDetails(Int64 entityId, IEntityContext iEntityContext, ICallerContext iCallerContext, Boolean publishEvents = true, Boolean applyAVS = true)
        {
            String relationshipDetails = MakeServiceCall("GetEntityRelationshipDetails", "GetEntityRelationshipDetails", client => client.GetEntityRelationshipDetails(entityId, (EntityContext)iEntityContext, FillDiagnosticTraces(iCallerContext), publishEvents, applyAVS));

            return new Entity(relationshipDetails, ObjectSerialization.DataTransfer);
        }

        #endregion

        #region Organization
        /// <summary>
        /// 
        /// </summary>
        /// <param name="targetUserLogin"></param>
        /// <param name="userLogin"></param>
        /// <param name="localeId"></param>
        /// <param name="intCountFrom"></param>
        /// <param name="intCountTo"></param>
        /// <param name="sortColumn"></param>
        /// <param name="searchColumn"></param>
        /// <param name="searchParameter"></param>
        /// <returns></returns>
        public string GetUserVisibleOrgs(string targetUserLogin, string userLogin, int localeId, int intCountFrom, int intCountTo, string sortColumn, string searchColumn, string searchParameter)
        {
            return MakeServiceCall("GetUserVisibleOrgs", "GetUserVisibleOrgs", client => client.GetUserVisibleOrgs(targetUserLogin, userLogin, localeId, intCountFrom, intCountTo, sortColumn, searchColumn, searchParameter));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="targetUserLogin"></param>
        /// <param name="userLogin"></param>
        /// <param name="localeId"></param>
        /// <param name="intCountFrom"></param>
        /// <param name="intCountTo"></param>
        /// <param name="sortColumn"></param>
        /// <param name="searchColumn"></param>
        /// <param name="searchParameter"></param>
        /// <param name="orgParentId"></param>
        /// <param name="orgClassificationId"></param>
        /// <param name="catalogObjectType"></param>
        /// <returns></returns>
        public string GetOrgsWithPermissions(string targetUserLogin, string userLogin, int localeId, int intCountFrom, int intCountTo, string sortColumn, string searchColumn, string searchParameter, int orgParentId, int orgClassificationId, string catalogObjectType)
        {
            return MakeServiceCall("GetOrgsWithPermissions", "GetOrgsWithPermissions", client => client.GetOrgsWithPermissions(targetUserLogin, userLogin, localeId, intCountFrom, intCountTo, sortColumn, searchColumn, searchParameter, orgParentId, orgClassificationId, catalogObjectType));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="targetUserLogin"></param>
        /// <param name="userLogin"></param>
        /// <param name="localeId"></param>
        /// <param name="intCountFrom"></param>
        /// <param name="intCountTo"></param>
        /// <param name="sortColumn"></param>
        /// <param name="searchColumn"></param>
        /// <param name="searchParameter"></param>
        /// <param name="orgClassificationId"></param>
        /// <param name="catalogObjectType"></param>
        /// <returns></returns>
        public string GetOrgsByOrgClassification(string targetUserLogin, string userLogin, int localeId, int intCountFrom, int intCountTo, string sortColumn, string searchColumn, string searchParameter, int orgClassificationId, string catalogObjectType)
        {
            return MakeServiceCall("GetOrgsByOrgClassification", "GetOrgsByOrgClassification", client => client.GetOrgsByOrgClassification(targetUserLogin, userLogin, localeId, intCountFrom, intCountTo, sortColumn, searchColumn, searchParameter, orgClassificationId, catalogObjectType));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="orgTypeId"></param>
        /// <param name="intCountFrom"></param>
        /// <param name="intCountTo"></param>
        /// <param name="sortColumn"></param>
        /// <param name="searchColumn"></param>
        /// <param name="searchParameter"></param>
        /// <param name="userLogin"></param>
        /// <returns></returns>
        public string GetAllOrgTypes(int orgTypeId, int intCountFrom, int intCountTo, string sortColumn, string searchColumn, string searchParameter, string userLogin)
        {
            return MakeServiceCall("GetAllOrgTypes", "GetAllOrgTypes", client => client.GetAllOrgTypes(orgTypeId, intCountFrom, intCountTo, sortColumn, searchColumn, searchParameter, userLogin));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="orgTypeId"></param>
        /// <param name="intCountFrom"></param>
        /// <param name="intCountTo"></param>
        /// <param name="sortColumn"></param>
        /// <param name="searchColumn"></param>
        /// <param name="searchParameter"></param>
        /// <param name="userLogin"></param>
        /// <returns></returns>
        public string GetOrgTypes(int orgTypeId, int intCountFrom, int intCountTo, string sortColumn, string searchColumn, string searchParameter, string userLogin)
        {
            return MakeServiceCall("GetOrgTypes", "GetOrgTypes", client => client.GetOrgTypes(orgTypeId, intCountFrom, intCountTo, sortColumn, searchColumn, searchParameter, userLogin));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="txtXML"></param>
        /// <param name="userLogin"></param>
        public Boolean ProcessOrgTypes(string txtXML, string userLogin)
        {
            return MakeServiceCall("ProcessOrgTypes", "ProcessOrgTypes", client => client.ProcessOrgTypes(txtXML, userLogin));

        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string GetAllOrgHierarchies()
        {
            return MakeServiceCall("GetAllOrgHierarchies", "GetAllOrgHierarchies", client => client.GetAllOrgHierarchies());
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="orgClassificationId"></param>
        /// <param name="localeId"></param>
        /// <param name="userLogin"></param>
        /// <returns></returns>
        public string GetOrgsWithLocales(int orgClassificationId, int localeId, string userLogin)
        {
            return MakeServiceCall("GetOrgsWithLocales", "GetOrgsWithLocales", client => client.GetOrgsWithLocales(orgClassificationId, localeId, userLogin));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="txtXML"></param>
        /// <param name="userLogin"></param>
        public Boolean ProcessOrgLocales(string txtXML, string userLogin)
        {
            return MakeServiceCall("ProcessOrgLocales", "ProcessOrgLocales", client => client.ProcessOrgLocales(txtXML, userLogin));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userLogin"></param>
        /// <returns></returns>
        public DataTable GetOrgsWithPermissionsOrg(string userLogin)
        {
            return MakeServiceCall("GetOrgsWithPermissionsOrg", "GetOrgsWithPermissionsOrg", client => client.GetOrgsWithPermissionsOrg(userLogin));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="orgId"></param>
        /// <param name="catalogId"></param>
        /// <param name="findWhat"></param>
        /// <returns></returns>
        public DataTable GetOrgCatalogInfo(int orgId, int catalogId, string findWhat)
        {
            return MakeServiceCall("GetOrgCatalogInfo", "GetOrgCatalogInfo", client => client.GetOrgCatalogInfo(orgId, catalogId, findWhat));
        }
        #endregion

        #region Attribute

        /// <summary>
        /// 
        /// </summary>
        /// <param name="attributeId"></param>
        /// <param name="userLogin"></param>
        /// <returns></returns>
        public string GetAttributeDetails(int attributeId, string userLogin)
        {
            return MakeServiceCall("GetAttributeDetails", "GetAttributeDetails", client => client.GetAttributeDetails(attributeId, userLogin));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public DataTable GetAllAttributes()
        {
            return MakeServiceCall("GetAllAttributes", "GetAllAttributes", client => client.GetAllAttributes());
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cNodeId"></param>
        /// <param name="catalogId"></param>
        /// <param name="attributeId"></param>
        /// <param name="localeId"></param>
        /// <param name="userName"></param>
        /// <param name="returnAttrType"></param>
        /// <param name="showAtCreation"></param>
        /// <returns></returns>
        public string GetCNodeAttributeValue(long cNodeId, int catalogId, int attributeId, int localeId, string userName, int returnAttrType, bool showAtCreation)
        {
            return MakeServiceCall("GetCNodeAttributeValue", "GetCNodeAttributeValue", client => client.GetCNodeAttributeValue(cNodeId, catalogId, attributeId, localeId, userName, returnAttrType, showAtCreation));

        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string GetAttributeGroupsXML()
        {
            return MakeServiceCall("GetAttributeGroupsXML", "GetAttributeGroupsXML", client => client.GetAttributeGroupsXML());
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="attrParentId"></param>
        /// <returns></returns>
        public string GetTechSpecsByGroup(int attrParentId)
        {
            return MakeServiceCall("GetTechSpecsByGroup", "GetTechSpecsByGroup", client => client.GetTechSpecsByGroup(attrParentId));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="inputStr"></param>
        /// <returns></returns>
        public DataSet ComplexMetadata_GetDT(string inputStr)
        {
            return MakeServiceCall("ComplexMetadata_GetDT", "ComplexMetadata_GetDT", client => client.ComplexMetadata_GetDT(inputStr));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="attributeTypeId"></param>
        /// <param name="attributeId"></param>
        /// <param name="userId"></param>
        /// <param name="orgId"></param>
        /// <param name="catalogId"></param>
        /// <param name="cNodeId"></param>
        /// <param name="viewPath"></param>
        /// <returns></returns>
        public string GetAttributeChildrenByType(int attributeTypeId, string attributeId, string userId, int orgId, int catalogId, long cNodeId, string viewPath)
        {
            return MakeServiceCall("GetAttributeChildrenByType", "GetAttributeChildrenByType", client => client.GetAttributeChildrenByType(attributeTypeId, attributeId, userId, orgId, catalogId, cNodeId, viewPath));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="objectType"></param>
        /// <param name="objectId"></param>
        /// <param name="localeId"></param>
        public String GetSysObjectAttributesXML(string objectType, int objectId, int localeId)
        {
            return MakeServiceCall("GetSysObjectAttributesXML", "GetSysObjectAttributesXML", client => client.GetSysObjectAttributesXML(objectType, objectId, localeId));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="attributeXML"></param>
        /// <param name="objectId"></param>
        /// <param name="objectType"></param>
        /// <param name="localeId"></param>
        public Boolean ProcessObjectAttributes(string attributeXML, int objectId, string objectType, int localeId)
        {
            return MakeServiceCall("ProcessObjectAttributes", "ProcessObjectAttributes", client => client.ProcessObjectAttributes(attributeXML, objectId, objectType, localeId));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public DataSet UniqueComplxAttrid()
        {
            return MakeServiceCall("UniqueComplxAttrid", "UniqueComplxAttrid", client => client.UniqueComplxAttrid());
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public string GetAttributeConfig(string tableName, int id)
        {
            return MakeServiceCall("GetAttributeConfig", "GetAttributeConfig", client => client.GetAttributeConfig(tableName, id));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="attributeId"></param>
        /// <returns></returns>
        public string GetAttributeUsage(int attributeId)
        {
            return MakeServiceCall("GetAttributeUsage", "GetAttributeUsage", client => client.GetAttributeUsage(attributeId));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="parentId"></param>
        /// <param name="localeId"></param>
        /// <param name="userLogin"></param>
        /// <returns></returns>
        public string GetAttributeGroupChildren(Int32? parentId, int localeId, string userLogin)
        {
            return MakeServiceCall("GetAttributeGroupChildren", "GetAttributeGroupChildren", client => client.GetAttributeGroupChildren(parentId, localeId, userLogin));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parentId"></param>
        /// <param name="countFrom"></param>
        /// <param name="countTo"></param>
        /// <param name="searchParameter"></param>
        /// <param name="searchColumn"></param>
        /// <param name="sortColumn"></param>
        /// <param name="localeId"></param>
        /// <param name="bitUnusedOnly"></param>
        /// <param name="userLogin"></param>
        /// <returns></returns>
        public string GetAttributes(int parentId, int countFrom, int countTo, string searchParameter, string searchColumn, string sortColumn, int localeId, bool bitUnusedOnly, string userLogin)
        {
            return MakeServiceCall("GetAttributes", "GetAttributes", client => client.GetAttributes(parentId, countFrom, countTo, searchParameter, searchColumn, sortColumn, localeId, bitUnusedOnly, userLogin));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="attributeId"></param>
        /// <param name="cNodeId"></param>
        /// <param name="userLogin"></param>
        /// <returns></returns>
        public string GetNodeAttributeDetails(int attributeId, int cNodeId, string userLogin)
        {
            return MakeServiceCall("GetNodeAttributeDetails", "GetNodeAttributeDetails", client => client.GetNodeAttributeDetails(attributeId, cNodeId, userLogin));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userLogin"></param>
        /// <returns></returns>
        public string GetAttributeDataTypeMap(string userLogin)
        {
            return MakeServiceCall("GetAttributeDataTypeMap", "GetAttributeDataTypeMap", client => client.GetAttributeDataTypeMap(userLogin));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userLogin"></param>
        /// <returns></returns>
        public string GetAttributeDataTypes(string userLogin)
        {
            return MakeServiceCall("GetAttributeDataTypes", "GetAttributeDataTypes", client => client.GetAttributeDataTypes(userLogin));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userLogin"></param>
        /// <returns></returns>
        public string GetAttributeDisplayTypeMap(string userLogin)
        {
            return MakeServiceCall("GetAttributeDisplayTypeMap", "GetAttributeDisplayTypeMap", client => client.GetAttributeDisplayTypeMap(userLogin));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userLogin"></param>
        /// <returns></returns>
        public string GetAttributeDisplayTypes(string userLogin)
        {
            return MakeServiceCall("GetAttributeDisplayTypes", "GetAttributeDisplayTypes", client => client.GetAttributeDisplayTypes(userLogin));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string GetFormattersXML()
        {
            return MakeServiceCall("GetFormattersXML", "GetFormattersXML", client => client.GetFormattersXML());
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="txtXML"></param>
        /// <param name="targetAttributeId"></param>
        /// <param name="maxAllowableChars"></param>
        /// <param name="inTestPage"></param>
        /// <param name="retAttrValue"></param>
        /// <returns></returns>
        public string UpdateTargetAttributeValue(string txtXML, int targetAttributeId, int maxAllowableChars, string inTestPage, String retAttrValue)
        {
            return MakeServiceCall("UpdateTargetAttributeValue", "UpdateTargetAttributeValue", client => client.UpdateTargetAttributeValue(txtXML, targetAttributeId, maxAllowableChars, inTestPage, retAttrValue));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="intGroupId"></param>
        /// <param name="searchValue"></param>
        /// <param name="level"></param>
        /// <param name="getComplexChildren"></param>
        /// <param name="localeId"></param>
        /// <returns></returns>
        public string getAttributesXml(int intGroupId, string searchValue, int level, bool getComplexChildren, int localeId)
        {
            return MakeServiceCall("getAttributesXml", "getAttributesXml", client => client.getAttributesXml(intGroupId, searchValue, level, getComplexChildren, localeId));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="versionId"></param>
        /// <param name="cNodeId"></param>
        /// <param name="attributeId"></param>
        public Boolean ComplexMetadataRollBack(int versionId, int cNodeId, int attributeId)
        {
            return MakeServiceCall("ComplexMetadataRollBack", "ComplexMetadataRollBack", client => client.ComplexMetadataRollBack(versionId, cNodeId, attributeId));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="complexTableName"></param>
        /// <param name="lookUpColumnName"></param>
        /// <param name="isCheckingOnly"></param>
        /// <returns></returns>
        public DataSet GetAttributeDataForLookupControl(string complexTableName, string lookUpColumnName, int isCheckingOnly)
        {
            return MakeServiceCall("GetAttributeDataForLookupControl", "GetAttributeDataForLookupControl", client => client.GetAttributeDataForLookupControl(complexTableName, lookUpColumnName, isCheckingOnly));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="txtRulesXML"></param>
        /// <returns></returns>
        public string GetCategoriesFromRuleXML(string txtRulesXML)
        {
            return MakeServiceCall("GetCategoriesFromRuleXML", "GetCategoriesFromRuleXML", client => client.GetCategoriesFromRuleXML(txtRulesXML));
        }
        #endregion

        #region Common


        /// <summary>
        /// 
        /// </summary>
        /// <param name="objectValue"></param>
        /// <param name="objectId"></param>
        /// <param name="searchValue"></param>
        /// <param name="topRows"></param>
        /// <returns></returns>
        public DataSet GetObject(string objectValue, int objectId, string searchValue, int topRows)
        {
            return MakeServiceCall("GetObject", "GetObject", client => client.GetObject(objectValue, objectId, searchValue, topRows));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="objectValue"></param>
        /// <param name="objectId"></param>
        /// <param name="searchValue"></param>
        /// <param name="topRows"></param>
        /// <returns></returns>
        public DataTable ObjectGet(string objectValue, int objectId, string searchValue, int topRows)
        {
            return MakeServiceCall("ObjectGet", "ObjectGet", client => client.ObjectGet(objectValue, objectId, searchValue, topRows));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="statusType"></param>
        /// <returns></returns>
        public DataTable GetStatusesByType(int statusType)
        {
            return MakeServiceCall("GetStatusesByType", "GetStatusesByType", client => client.GetStatusesByType(statusType));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public DataTable GetItemMetaDataActions()
        {
            return MakeServiceCall("GetItemMetaDataActions", "GetItemMetaDataActions", client => client.GetItemMetaDataActions());
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="objectValue"></param>
        /// <param name="tableName"></param>
        /// <param name="searchValue"></param>
        /// <param name="localeId"></param>
        /// <returns></returns>
        public DataTable LookupSearch(string objectValue, string tableName, string searchValue, int localeId)
        {
            return MakeServiceCall("LookupSearch", "LookupSearch", client => client.LookupSearch(objectValue, tableName, searchValue, localeId));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="objectValue"></param>
        /// <param name="tableName"></param>
        /// <param name="searchValue"></param>
        /// <returns></returns>
        public DataTable LookupRuleDataSearch(string objectValue, string tableName, string searchValue)
        {
            return MakeServiceCall("LookupRuleDataSearch", "LookupRuleDataSearch", client => client.LookupRuleDataSearch(objectValue, tableName, searchValue));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="securityUserId"></param>
        /// <param name="orgId"></param>
        /// <param name="userConfigTypeId"></param>
        /// <param name="userConfigShortName"></param>
        /// <param name="getFromAdmin"></param>
        /// <returns></returns>
        public DataTable GetUserConfig(int securityUserId, int orgId, int userConfigTypeId, string userConfigShortName, bool getFromAdmin)
        {
            return MakeServiceCall("GetUserConfig", "GetUserConfig", client => client.GetUserConfig(securityUserId, orgId, userConfigTypeId, userConfigShortName, getFromAdmin));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objectName"></param>
        /// <param name="objectId"></param>
        /// <returns></returns>
        public DataTable ObjectDependency(string objectName, int objectId)
        {
            return MakeServiceCall("ObjectDependency", "ObjectDependency", client => client.ObjectDependency(objectName, objectId));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileId"></param>
        /// <returns></returns>
        public DataTable GetFileDetails(int fileId)
        {
            return MakeServiceCall("GetFileDetails", "GetFileDetails", client => client.GetFileDetails(fileId));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileId"></param>
        /// <param name="chrDetailsOnly"></param>
        /// <returns></returns>
        public DataTable GetFile(int fileId, string chrDetailsOnly)
        {
            return MakeServiceCall("GetFile", "GetFile", client => client.GetFile(fileId, chrDetailsOnly));
        }
        #endregion

        #region ImportExport

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Collection<UserAction> GetImportPermission()
        {
            return MakeServiceCall("GetImportPermission", "GetImportPermission", client => client.GetImportPermission());
        }

        /// <summary>
        /// Saves profile
        /// </summary>
        /// <param name="name">Indicates name of the profile</param>
        /// <param name="domain">Indicates domain name</param>
        /// <param name="textProfile">Indicates profile in text format</param>
        /// <param name="fileType">Indicates type of file</param>
        /// <param name="profileId">Indicates identifier of profile</param>
        /// <param name="orgId">Indicates identifier of organization</param>
        /// <param name="catalogId">Indicates identifier of catalog</param>
        /// <param name="userId">Indicates identifier of user</param>
        /// <param name="programName">Indicates name of program</param>
        public Int32 SaveProfile(string name, string domain, string textProfile, string fileType, int profileId, int orgId, int catalogId, string userId, string programName)
        {
            return MakeServiceCall("SaveProfile", "SaveProfile", client => client.SaveProfile(name, domain, textProfile, fileType, profileId, orgId, catalogId, userId, programName));
        }

        /// <summary>
        /// Deletes profile based on given profile identifier
        /// </summary>
        /// <param name="profileId">Indicates identifier of profile</param>
        /// <param name="domain">Indicates domain name</param>
        public Int32 DeleteProfile(string profileId, string domain)
        {
            return MakeServiceCall("DeleteProfile", "DeleteProfile", client => client.DeleteProfile(profileId, domain));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="subscriberId"></param>
        /// <param name="subscriberName"></param>
        /// <param name="searchStr"></param>
        /// <param name="inBound"></param>
        /// <param name="outBound"></param>
        /// <returns></returns>
        public DataTable GetSubscribers(int subscriberId, string subscriberName, string searchStr, bool inBound, bool outBound)
        {
            return MakeServiceCall("GetSubscribers", "GetSubscribers", client => client.GetSubscribers(subscriberId, subscriberName, searchStr, inBound, outBound));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <param name="profilesId"></param>
        /// <param name="profileName"></param>
        /// <param name="profileTypeId"></param>
        /// <param name="typeShortName"></param>
        /// <param name="searchStr"></param>
        /// <param name="bitIncludeData"></param>
        /// <returns></returns>
        public DataTable GetProfiles(string user, int profilesId, string profileName, int profileTypeId, string typeShortName, string searchStr, bool bitIncludeData)
        {
            return MakeServiceCall("GetProfiles", "GetProfiles", client => client.GetProfiles(user, profilesId, profileName, profileTypeId, typeShortName, searchStr, bitIncludeData));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public DataTable GetChannels()
        {
            return MakeServiceCall("GetChannels", "GetChannels", client => client.GetChannels());
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="catalogId"></param>
        /// <param name="cNodeId"></param>
        /// <param name="localeId"></param>
        /// <param name="includeComplexAttrChildren"></param>
        /// <returns></returns>
        public DataTable GetCatalogCharacteristicTemplate(int catalogId, int cNodeId, int localeId, bool includeComplexAttrChildren)
        {
            return MakeServiceCall("GetCatalogCharacteristicTemplate", "GetCatalogCharacteristicTemplate", client => client.GetCatalogCharacteristicTemplate(catalogId, cNodeId, localeId, includeComplexAttrChildren));
        }
        #endregion

        #region Security

        /// <summary>
        /// Gets role and menus
        /// </summary>
        /// <param name="securityRole">Indicates identifier of security role</param>
        /// <returns>Returns role and menus in string format</returns>
        public String GetRoleMenus(Int32 securityRole)
        {
            return MakeServiceCall("GetRoleMenus", "GetRoleMenus", (client) => client.GetRoleMenus(securityRole));
        }

        /// <summary>
        /// Get users
        /// </summary>
        /// <param name="securityUser">Indicates identifier of security user</param>
        /// <param name="userType">Indicates type of the user</param>
        /// <param name="countFrom">Indicates start boundary for get</param>
        /// <param name="countTo">Indicates end boundary for get</param>
        /// <param name="vchrSortColumn">Indicates name of the sort column</param>
        /// <param name="vchrSearchColumn">Indicates name of the search column</param>
        /// <param name="vchrSearchParameter">Indicates search parameter</param>
        /// <param name="vchrUserLogin">Indicates login name of the user</param>
        /// <returns>Returns users in string format</returns>
        public String GetUsers(Int32 securityUser, Int32 userType, Int32 countFrom, Int32 countTo, String vchrSortColumn, String vchrSearchColumn, String vchrSearchParameter, String vchrUserLogin)
        {
            return MakeServiceCall("GetUsers", "GetUsers", (client) => client.GetUsers(securityUser, userType, countFrom, countTo, vchrSortColumn, vchrSearchColumn, vchrSearchParameter, vchrUserLogin));
        }

        /// <summary>
        /// Gets users with roles
        /// </summary>
        /// <param name="securityUser">Indicates identifier of security user</param>
        /// <param name="countFrom">Indicates start boundary for get</param>
        /// <param name="countTo">Indicates end boundary for get</param>
        /// <param name="vchrSearchColumn">Indicates name of the search column</param>
        /// <param name="vchrSearchParameter">Indicates search parameter</param>
        /// <param name="vchrUserLogin">Indicates login name of the user</param>
        /// <returns>Returns users with roles in string format</returns>
        public String GetUsersWithRoles(Int32 securityUser, Int32 countFrom, Int32 countTo, String vchrSearchColumn, String vchrSearchParameter, String vchrUserLogin)
        {
            return MakeServiceCall("GetUsersWithRoles", "GetUsersWithRoles", (client) => client.GetUsersWithRoles(securityUser, countFrom, countTo, vchrSearchColumn, vchrSearchParameter, vchrUserLogin));
        }

        /// <summary>
        /// processes users
        /// </summary>
        /// <param name="txtXML">Indicates xml which needs to process</param>
        /// <param name="vchrUserLogin">Indicates login name of the user</param>
        /// <returns>Returns true if users processed successfully, otherwise false</returns>
        public Boolean ProcessUsers(String txtXML, String vchrUserLogin)
        {
            return MakeServiceCall("ProcessUsers", "ProcessUsers", (client) => client.ProcessUsers(txtXML, vchrUserLogin));
        }

        /// <summary>
        /// Get roles
        /// </summary>
        /// <param name="securityRole">Indicates identifier of security role</param>
        /// <param name="userType">Indicates type of the user</param>
        /// <param name="chrGetPermissionSetOnly">Indicates true if needs to get which are having only read permission</param>
        /// <param name="intCountFrom">Indicates start boundary for get</param>
        /// <param name="intCountTo">Indicates end boundary for get</param>
        /// <param name="vchrSortColumn">Indicates name of the sort column</param>
        /// <param name="vchrSearchColumn">Indicates name of the search column</param>
        /// <param name="vchrSearchParameter">Indicates search parameter</param>
        /// <param name="vchrUserLogin">Indicates login name of the user</param>
        /// <param name="bitDisplaySystemRole">Indicates true if needs to get only system role</param>
        /// <returns>Returns roles in string format</returns>
        public String GetRoles(Int32 securityRole, Int32 userType, String chrGetPermissionSetOnly, Int32 intCountFrom, Int32 intCountTo, String vchrSortColumn, String vchrSearchColumn, String vchrSearchParameter, String vchrUserLogin, Boolean bitDisplaySystemRole)
        {
            return MakeServiceCall("GetRoles", "GetRoles", (client) => client.GetRoles(securityRole, userType, chrGetPermissionSetOnly, intCountFrom, intCountTo, vchrSortColumn, vchrSearchColumn, vchrSearchParameter, vchrUserLogin, bitDisplaySystemRole));
        }

        /// <summary>
        /// Gets roles with users
        /// </summary>
        /// <param name="pkSecurityRole">Indicates identifier of security role</param>
        /// <param name="chrGetPermissionSetOnly">Indicates true if needs to get which are having only read permission</param>
        /// <param name="intCountFrom">Indicates start boundary for get</param>
        /// <param name="intCountTo">Indicates end boundary for get</param>
        /// <param name="vchrSearchColumn">Indicates name of the search column</param>
        /// <param name="vchrSearchParameter">Indicates search parameter</param>
        /// <param name="vchrUserLogin">Indicates login name of the user</param>
        /// <param name="bitDisplaySystemRole">Indicates true if needs to get only system role</param>
        /// <returns>Returns roles with users in string format</returns>
        public String GetRolesWithUsers(Int32 pkSecurityRole, String chrGetPermissionSetOnly, Int32 intCountFrom, Int32 intCountTo, String vchrSearchColumn, String vchrSearchParameter, String vchrUserLogin, Boolean bitDisplaySystemRole)
        {
            return MakeServiceCall("GetRolesWithUsers", "GetRolesWithUsers", (client) => client.GetRolesWithUsers(pkSecurityRole, chrGetPermissionSetOnly, intCountFrom, intCountTo, vchrSearchColumn, vchrSearchColumn, vchrUserLogin, bitDisplaySystemRole));
        }

        /// <summary>
        /// Processes security roles
        /// </summary>
        /// <param name="txtXML">Indicates xml which needs to be process</param>
        /// <param name="vchrUserLogin">Indicates login name of the user</param>
        /// <returns>Returns datatable for roles which are being processed</returns>
        public DataTable ProcessRoles(String txtXML, String vchrUserLogin)
        {
            return MakeServiceCall("ProcessRoles", "ProcessRoles", (client) => client.ProcessRoles(txtXML, vchrUserLogin));
        }

        /// <summary>
        /// Process role and menus
        /// </summary>
        /// <param name="txtXML">Indicates xml which needs to be process</param>
        /// <param name="vchrUserLogin">Indicates login name of the user</param>
        /// <returns>returns true if roles and menus are processed successfully, otherwise false</returns>
        public Boolean ProcessRoleMenus(String txtXML, String vchrUserLogin)
        {
            return MakeServiceCall("ProcessRoleMenus", "ProcessRoleMenus", (client) => client.ProcessRoleMenus(txtXML, vchrUserLogin));
        }

        /// <summary>
        /// Get actions of object type
        /// </summary>
        /// <returns>Returns actions of object type</returns>
        public String GetObjectTypeAction()
        {
            return MakeServiceCall("GetObjectTypeAction", "GetObjectTypeAction", (client) => client.GetObjectTypeAction());
        }

        /// <summary>
        /// Get actions of hierarchy
        /// </summary>
        /// <returns>Returns actions of hierarchy</returns>
        public String GetHierarchyAction()
        {
            return MakeServiceCall("GetHierarchyAction", "GetHierarchyAction", (client) => client.GetHierarchyAction());
        }

        /// <summary>
        /// Gets permissions for given security role and user
        /// </summary>
        /// <param name="securityRole">Indicates identifier of security role</param>
        /// <param name="chrPermissionSet">Indicates permission set</param>
        /// <param name="vchrUserLogin">Indicates login name of the user</param>
        /// <returns>Returns permission based on security role and user</returns>
        public String GetPermissions(Int32 securityRole, String chrPermissionSet, String vchrUserLogin)
        {
            return MakeServiceCall("GetPermissions", "GetPermissions", (client) => client.GetPermissions(securityRole, chrPermissionSet, vchrUserLogin));
        }

        /// <summary>
        /// Process permissions
        /// </summary>
        /// <param name="txtXML">Indicates xml which needs to process</param>
        /// <param name="vchrUserLogin">Indicates login name of the user</param>
        /// <returns>Returns true if permissions processed successfully, otherwise false</returns>
        public Boolean ProcessPermissions(String txtXML, String vchrUserLogin)
        {
            return MakeServiceCall("ProcessPermissions", "ProcessPermissions", (client) => client.ProcessPermissions(txtXML, vchrUserLogin));
        }

        /// <summary>
        /// Gets all organizations and catalogs based on user
        /// </summary>
        /// <param name="vchrUserLogin">Indicates login name of the user</param>
        /// <returns>Returns all organization and containers in string</returns>
        public String GetAllOrganizationsAndCatalogs(String vchrUserLogin)
        {
            return MakeServiceCall("GetAllOrganizationsAndCatalogs", "GetAllOrganizationsAndCatalogs", (client) => client.GetAllOrganizationsAndCatalogs(vchrUserLogin));
        }

        /// <summary>
        /// Decides the given user is system user or not
        /// </summary>
        /// <param name="nvchrUserLogin">Indicates login name of the user</param>
        /// <param name="isSystemUser">Indicates whether the given user is system user or not</param>
        /// <returns>Returns true if given user is system user, otherwise false</returns>
        public String IsSystemUser(String nvchrUserLogin, String isSystemUser)
        {
            return MakeServiceCall("IsSystemUser", "IsSystemUser", client => client.IsSystemUser(nvchrUserLogin, isSystemUser));
        }

        /// <summary>
        /// Checks permission for specified user
        /// </summary>
        /// <param name="vchrUserLogin">Indicate login name of the user</param>
        /// <param name="objectTypeSN">Indicates short name of object type</param>
        /// <param name="actionSN">Indicates short name of action</param>
        /// <param name="parentObjectTypeSN">Indicates short name of parent object type</param>
        /// <param name="orgId">Indicates identifier of organization</param>
        /// <param name="catalogId">Indicates identifier of catalog</param>
        /// <param name="categoryCNode">Indicates identifier of category</param>
        /// <param name="componentCNode">Indicates identifier of component</param>
        /// <param name="attribute">Indicates identifier of attribute</param>
        /// <param name="relationship">Indicates identifier of relationship</param>
        /// <param name="bitForDraft">Indicates boolean value for IsDraft</param>
        /// <param name="bitHasPermission">Indicates whether specific user have a permission as output parameter</param>
        /// <returns>Returns true if specified user have a permission, otherwise false</returns>
        public Boolean HasPermission(String vchrUserLogin, String objectTypeSN, String actionSN, String parentObjectTypeSN, Int32 orgId, Int32 catalogId, Int64 categoryCNode, Int64 componentCNode, Int32 attribute, Int32 relationship, Boolean bitForDraft, Boolean bitHasPermission)
        {
            return MakeServiceCall("HasPermission", "HasPermission", client => client.HasPermission(vchrUserLogin, objectTypeSN, actionSN, parentObjectTypeSN, orgId, catalogId, categoryCNode, componentCNode, attribute, relationship, bitForDraft, bitHasPermission));
        }

        #endregion

        #region Administration
        
        /// <summary>
        /// Gets xml of relationship types
        /// </summary>
        /// <returns>Returns xml of relationship types</returns>
        public String GetRelTypesXML()
        {
            return MakeServiceCall("GetRelTypesXML", "GetRelTypesXML", client => client.GetRelTypesXML());
        }

        /// <summary>
        /// Gets catalog node type attributes based on catalog id, node type id and locale id
        /// </summary>
        /// <param name="catalogId">Indicates identifier of catalog</param>
        /// <param name="nodeTypeId">Indicates identifier of node type</param>
        /// <param name="localeId">Indicates identifier of locale</param>
        /// <returns>Returns dataset of catalog node type attributes</returns>
        public DataSet GetCatalogNodeTypeAttr(Int32 catalogId, Int32 nodeTypeId, Int32 localeId)
        {
            return MakeServiceCall("GetCatalogNodeTypeAttr", "GetCatalogNodeTypeAttr", client => client.GetCatalogNodeTypeAttr(catalogId, nodeTypeId, localeId));
        }

        #endregion

        #region RegularExpression

        /// <summary>
        /// Gets regular expression of an attribute
        /// </summary>
        /// <param name="attribute">Indicates identifier of an attribute</param>
        /// <returns>Returns regular expression of an attribute in datatable</returns>
        public DataTable GetAttributeRegEx(Int32 attribute)
        {
            return MakeServiceCall("GetAttributeRegEx", "GetAttributeRegEx", client => client.GetAttributeRegEx(attribute));
        }

        #endregion

        #region Taxonomy
        
        /// <summary>
        /// Gets technical specification map from given category id and locale id
        /// </summary>
        /// <param name="intCategoryId">Indicates identifier of category</param>
        /// <param name="intLocaleId">Indicates identifier of locale</param>
        /// <returns>Returns technical specification map in datatable</returns>
        public DataTable GetTechSpecsMap(Int32 intCategoryId, Int32 intLocaleId)
        {
            return MakeServiceCall("GetTechSpecsMap", "GetTechSpecsMap", client => client.GetTechSpecsMap(intCategoryId, intLocaleId));
        }
        
        /// <summary>
        /// Gets hierarchy based on given catalog id
        /// </summary>
        /// <param name="catalogId">Indicates identifier of catalog</param>
        /// <returns>Returns hierarchy data in datatable</returns>
        public DataTable GetTaxonomyByCatalog(Int32 catalogId)
        {
            return MakeServiceCall("GetTaxonomyByCatalog", "GetTaxonomyByCatalog", client => client.GetTaxonomyByCatalog(catalogId));
        }
        
        #endregion

        #region Util
        
        /// <summary>
        /// Estimates changes from given node list
        /// </summary>
        /// <param name="changeProgram">Indicates name of the change program</param>
        /// <param name="cNodeList">Indicates list of the node</param>
        /// <returns>Returns datatable of estimate changes</returns>
        public DataTable EstimateChanges(String changeProgram, String cNodeList)
        {
            return MakeServiceCall("EstimateChanges", "EstimateChanges", client => client.EstimateChanges(changeProgram, cNodeList));
        }

        #endregion

        #region AttributeModel
        /// <summary>
        /// </summary>
        /// <returns>String</returns>

        public String GetCatalogNodeTypeAttrbiuteAsXml(Int32 organizationId, Int32 catalogId, String nodeType, Int32 branchLevel, Boolean includeComplexAttrChildren, Boolean excludeableSearchable, Collection<LocaleEnum> locales, LocaleEnum systemDataLocale)
        {
            return MakeServiceCall("GetCatalogNodeTypeAttrbiuteAsXml", "GetCatalogNodeTypeAttrbiuteAsXml", client => client.GetCatalogNodeTypeAttrbiuteAsXml(organizationId, catalogId, nodeType, branchLevel, includeComplexAttrChildren, excludeableSearchable, locales, systemDataLocale));
        }

        /// <summary>
        /// </summary>
        /// <returns>Collection</returns>
        public Collection<AttributeModel> GetAllCommonAttributes(int localeId)
        {
            return MakeServiceCall("GetAllCommonAttributes", "GetAllCommonAttributes", client => client.GetAllCommonAttributes(localeId));
        }

        /// <summary>
        /// </summary>
        /// <returns>Collection</returns>
        public Collection<AttributeModel> GetAllTechnicalAttributes(int localeId)
        {
            return MakeServiceCall("GetAllTechnicalAttributes", "GetAllTechnicalAttributes", client => client.GetAllTechnicalAttributes(localeId));
        }

        /// <summary>
        /// </summary>
        /// <returns>String</returns>
        public Collection<AttributeModel> GetCommonAttributesByContainerAndEntityType(Int32 catalogId, Int32 entityTypeId, Int32 localeId)
        {
            return MakeServiceCall("GetCatalogNodeTypeAttrbiuteAsXml", "GetCatalogNodeTypeAttrbiuteAsXml", client => client.GetCommonAttributesByContainerAndEntityType(catalogId, entityTypeId, localeId));
        }

        /// <summary>
        /// </summary>
        /// <returns>Collection</returns>
        public Collection<AttributeModel> GetTechAttributesByTaxonomyAndCategory(int categoryId, int taxonomyId, int localeId)
        {
            return MakeServiceCall("GetTechAttributesByTaxonomyAndCategory", "GetTechAttributesByTaxonomyAndCategory", client => client.GetTechAttributesByTaxonomyAndCategory(categoryId, taxonomyId, localeId));
        }

        /// <summary>
        /// </summary>
        /// <returns>Collection</returns>
        public Collection<AttributeGroup> GetByAttributeType(Int32 common, Int32 technical, Int32 relationship, Collection<LocaleEnum> locales, LocaleEnum systemLocale)
        {
            return MakeServiceCall("GetByAttributeType", "GetByAttributeType", client => client.GetByAttributeType(common, technical, relationship, locales, systemLocale));
        }

        /// <summary>
        /// </summary>
        /// <returns>Collection</returns>
        public Collection<AttributeModel> GetByAttributeGroup(int attributeGroupId, Collection<LocaleEnum> locales)
        {
            return MakeServiceCall("GetByAttributeGroup", "GetByAttributeGroup", client => client.GetByAttributeGroup(attributeGroupId, locales));
        }

        #endregion

        #region Search Entity (Entity Explorer Search)

        /// <summary>
        /// /// Search entities in system for given search criteria and return list of entities with specified context. 
        /// </summary>
        /// <param name="searchCriteria">Provides search criteria.</param>
        /// <param name="searchContext">Provides search context. Example: SearchContext.MaxRecordCount indicates max records to be fetched while searching and AttributeIdList indicates List of attributes to load in returned entities.</param>
        /// <param name="totalCount">Indicates count of results fetched</param>
        /// <param name="searchOperationResult">Indicates operation result of search</param>
        /// <param name="callerContext">Indicates application and method which called this method</param>
        /// <returns>Returns dataset which is containing datatable with entities searched and total count.</returns>
        public DataSet SearchEntities(SearchCriteria searchCriteria, SearchContext searchContext, String totalCount, OperationResult searchOperationResult, CallerContext callerContext)
        {
            return MakeServiceCall("SearchEntities", "SearchEntities", client => client.SearchEntities(searchCriteria, searchContext, totalCount, searchOperationResult, FillDiagnosticTraces(callerContext)));
        }

        #endregion

        #region EntityOperationBL

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityIds"></param>
        /// <param name="attributeIdList"></param>
        /// <param name="containerId"></param>
        /// <param name="currentDataLocaleId"></param>
        /// <param name="systemDataLocaleId"></param>
        /// <returns></returns>
        public string GetAffectedInfo(Collection<long> entityIds, string attributeIdList, int containerId, int currentDataLocaleId, int systemDataLocaleId)
        {
            return MakeServiceCall("GetAffectedInfo", "GetAffectedInfo", client => client.GetAffectedInfo(entityIds, attributeIdList, containerId, currentDataLocaleId, systemDataLocaleId));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityIdList"></param>
        /// <param name="activityId"></param>
        /// <param name="workflowType"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public String GetWorkflowStatusXml(string entityIdList, int activityId, string workflowType, int userId)
        {
            return MakeServiceCall("GetWorkflowStatusXml", "GetWorkflowStatusXml", client => client.GetWorkflowStatusXml(entityIdList, activityId, workflowType, userId));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityIdList"></param>
        /// <param name="categoryId"></param>
        /// <param name="containerId"></param>
        /// <returns></returns>
        public Collection<Entity> GetMDLsByIdList(string entityIdList, long categoryId, int containerId)
        {
            return MakeServiceCall("GetMDLsByIdList", "GetMDLsByIdList", client => client.GetMDLsByIdList(entityIdList, categoryId, containerId));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="localeId"></param>
        /// <param name="cnodeId"></param>
        /// <param name="catalogId"></param>
        /// <param name="attributeId"></param>
        /// <param name="toTotalImpacted"></param>
        /// <param name="totalAffected"></param>
        /// <param name="maxReturnCount"></param>
        /// <returns></returns>
        public Dictionary<String, Object> GetImpactedEntities(int localeId, long cnodeId, int catalogId, int attributeId, int toTotalImpacted, int totalAffected, int maxReturnCount)
        {
            Dictionary<String, Object> getImpactedEntities = new Dictionary<string, object>();
            getImpactedEntities = MakeServiceCall("GetImpactedEntities", "GetImpactedEntities", client => client.GetImpactedEntities(localeId, cnodeId, catalogId, attributeId, toTotalImpacted, totalAffected, maxReturnCount));
            // getImpactedEntities.Add("strGetImpactedEntities", strImpactedEntities);
            return getImpactedEntities;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cnodeId"></param>
        /// <param name="cnodeParentId"></param>
        /// <param name="containerId"></param>
        /// <param name="bitUseDraftTax"></param>
        /// <param name="dataLocale"></param>
        /// <returns></returns>
        public string GetParent(long cnodeId, long cnodeParentId, int containerId, bool bitUseDraftTax, int dataLocale)
        {
            return MakeServiceCall("GetParent", "GetParent", client => client.GetParent(cnodeId, cnodeParentId, containerId, bitUseDraftTax, dataLocale));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="extSystemId"></param>
        /// <param name="inputXML"></param>
        /// <param name="coreAttrList"></param>
        /// <param name="techAttrList"></param>
        /// <param name="localeId"></param>
        /// <param name="programName"></param>
        /// <param name="includeInheritedValue"></param>
        /// <param name="computeInheritedValues"></param>
        /// <returns></returns>
        public string GetAttributeValuesForMDLs(int extSystemId, string inputXML, string coreAttrList, string techAttrList, int localeId, string programName, bool includeInheritedValue, bool computeInheritedValues)
        {
            return MakeServiceCall("GetAttributeValuesForMDLs", "GetAttributeValuesForMDLs", client => client.GetAttributeValuesForMDLs(extSystemId, inputXML, coreAttrList, techAttrList, localeId, programName, includeInheritedValue, computeInheritedValues));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cnodeId"></param>
        /// <param name="catalogIdList"></param>
        /// <param name="delimiter"></param>
        /// <param name="returnSelf"></param>
        /// <returns></returns>
        public Collection<Entity> GetMDL(long cnodeId, string catalogIdList, string delimiter, bool returnSelf)
        {
            return MakeServiceCall("GetMDL", "GetMDL", client => client.GetMDL(cnodeId, catalogIdList, delimiter, returnSelf));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cnodeId"></param>
        /// <param name="cnodeParentId"></param>
        /// <param name="containerId"></param>
        /// <param name="dataLocale"></param>
        /// <returns></returns>
        public List<Entity> GetParentOPBL(long cnodeId, long cnodeParentId, int containerId, int dataLocale)
        {
            return MakeServiceCall("GetParentOPBL", "GetParentOPBL", client => client.GetParentOPBL(cnodeId, cnodeParentId, containerId, dataLocale));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cnodeId"></param>
        /// <param name="dataConfigXML"></param>
        /// <param name="catalogIdList"></param>
        /// <param name="delimiter"></param>
        /// <param name="returnSelf"></param>
        /// <returns></returns>
        public Collection<Entity> GetMDLBasedOnConfig(long cnodeId, string dataConfigXML, string catalogIdList, string delimiter, bool returnSelf)
        {
            return MakeServiceCall("GetMDLBasedOnConfig", "GetMDLBasedOnConfig", client => client.GetMDLBasedOnConfig(cnodeId, dataConfigXML, catalogIdList, delimiter, returnSelf));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cNodeId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public Collection<BusinessObjects.Workflow.WorkflowActivity> GetWorkflowTasks(long cNodeId, int userId)
        {
            return MakeServiceCall("GetWorkflowTasks", "GetWorkflowTasks", client => client.GetWorkflowTasks(cNodeId, userId));

        }

        /// <summary>
        /// Reclassifies entities given in input xml
        /// </summary>
        /// <param name="dataXml"></param>
        /// <param name="userName"></param>
        /// <param name="isCategoryReclassify"></param>
        /// <returns>DataTable</returns>
        public Boolean Reclassify(String dataXml, String userName, Boolean isCategoryReclassify)
        {
            return MakeServiceCall("Reclassify", "Reclassify", client => client.Reclassify(dataXml, userName, isCategoryReclassify));

        }
        #endregion

        #region JobManager
        #region JobBL
        /// <summary>
        /// 
        /// </summary>
        /// <param name="jobId"></param>
        /// <param name="jobAction"></param>
        /// <param name="jobType"></param>
        /// <param name="application"></param>
        /// <returns></returns>
        public bool UpdateUserAction(int jobId, JobAction jobAction, string jobType, MDMCenterApplication application)
        {
            MakeServiceCall("UpdateUserAction", "UpdateUserAction", client => client.UpdateUserAction(jobId, jobAction, jobType, application));
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parentJobId"></param>
        /// <param name="application"></param>
        /// <returns>String </returns>
        public string GetChildJobsInXml(int parentJobId, MDMCenterApplication application)
        {
            return MakeServiceCall("GetChildJobsInXml", "GetChildJobsInXml", client => client.GetChildJobsInXml(parentJobId, application));
        }
        #endregion
        #region JobScheduleBL
        /// <summary>
        /// 
        /// </summary>
        /// <param name="scheduleId"></param>
        /// <param name="jobType"></param>
        /// <param name="application"></param>
        /// <returns></returns>
        public Tuple<Collection<Job>, Collection<ExportProfile>> GetSchedule(int scheduleId, string jobType, MDMCenterApplication application)
        {
            return MakeServiceCall("GetSchedule", "GetSchedule", client => client.GetSchedule(scheduleId, jobType, application));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="scheduleName"></param>
        /// <param name="scheduleLongName"></param>
        /// <param name="content"></param>
        /// <param name="isEnable"></param>
        /// <param name="profiles"></param>
        /// <param name="programName"></param>
        /// <param name="jobType"></param>
        /// <param name="application"></param>
        /// <returns></returns>
        public bool AddSchedule(string scheduleName, string scheduleLongName, string content, bool isEnable, string profiles, string programName, string jobType, MDMCenterApplication application)
        {
            return MakeServiceCall("AddSchedule", "AddSchedule", client => client.AddSchedule(scheduleName, scheduleLongName, content, isEnable, profiles, programName, jobType, application));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="scheduleId"></param>
        /// <param name="scheduleName"></param>
        /// <param name="scheduleLongName"></param>
        /// <param name="content"></param>
        /// <param name="isEnable"></param>
        /// <param name="profiles"></param>
        /// <param name="programName"></param>
        /// <param name="jobType"></param>
        /// <param name="application"></param>
        /// <returns></returns>
        public bool UpdateSchedule(int scheduleId, string scheduleName, string scheduleLongName, string content, bool isEnable, string profiles, string programName, string jobType, MDMCenterApplication application)
        {
            return MakeServiceCall("UpdateSchedule", "UpdateSchedule", client => client.UpdateSchedule(scheduleId, scheduleName, scheduleLongName, content, isEnable, profiles, programName, jobType, application));
        }
        #endregion
        #endregion

        #region BusinessRuleBL

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ruleIds"></param>
        /// <returns></returns>
        public Collection<BusinessRule> GetBusinessRulesById(IEnumerable<Int32> ruleIds)
        {
            Collection<int> intRulesId = (Collection<int>)ruleIds;
            return MakeServiceCall("GetBusinessRuleById", "GetBusinessRuleById", client => client.GetBusinessRulesById(intRulesId));
        }

        /// <summary>
        /// Gets business rules by user
        /// </summary>
        /// <param name="loginUser">Indicates login name of user</param>
        /// <returns>Returns collection of business rule by given user</returns>
        public Collection<BusinessRule> GetBusinessRulesByUserId(String loginUser)
        {
            return MakeServiceCall("GetBusinessRulesByUserId", "GetBusinessRulesByUserId", client => client.GetBusinessRulesByUserId(loginUser));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="businessRules"></param>
        /// <param name="loginUser"></param>
        /// <param name="programName"></param>
        /// <param name="action"></param>
        public Boolean ProcessBusinessRules(Collection<BusinessRule> businessRules, String loginUser, String programName, String action)
        {
            MakeServiceCall("Process", "Process", client => client.ProcessBusinessRules(businessRules, loginUser, programName, action));
            return true;
        }

        /// <summary>
        /// Get collection of BusinessRule based on Context
        /// </summary>
        /// <param name="eventSourceID">Indicates identifier of event source for retrieving business rules</param>
        /// <param name="eventSubscriberID">Indicates identifier of event subscriber for retrieving business rules</param>
        /// <param name="loginUserID">Indicates identifier of login user for retrieving business rules</param>
        /// <param name="loginUserRole">Indicates identifier of login user role for retrieving business rules</param>
        /// <param name="orgID">Indicates identifier of organization for retrieving business rules</param>
        /// <param name="containerID">Indicates identifier of container for retrieving business rules</param>
        /// <param name="entityTypeID">Indicates identifier of entity type for retrieving business rules</param>
        /// <param name="businessRuleTypeIDs">Indicates identifiers of business rule types for retrieving business rules</param>
        /// <returns>Returns collection of BusinessRule</returns>
        public Collection<BusinessRule> GetBusinessRulesByContext(Int32 eventSourceID, Int32 eventSubscriberID, Int32 loginUserID, Int32 loginUserRole, Int32 orgID, Int32 containerID, Int32 entityTypeID, String businessRuleTypeIDs)
        {
            return MakeServiceCall("GetBusinessRulesByContext", "GetBusinessRulesByContext", client => client.GetBusinessRulesByContext(eventSourceID, eventSubscriberID, loginUserID, loginUserRole, orgID, containerID, entityTypeID, businessRuleTypeIDs));
        }


        #endregion

        #region BusinessRuleAttributeMappingBL

        /// <summary>
        /// 
        /// </summary>
        /// <param name="businessRuleId"></param>
        /// <returns></returns>
        public Collection<BusinessRuleAttributeMapping> GetBusinessRuleAttributeMappingsByRuleId(Int32 businessRuleId)
        {
            return MakeServiceCall("GetBusinessRuleAttributeMappingsByRuleId", "GetBusinessRuleAttributeMappingsByRuleId", client => client.GetBusinessRuleAttributeMappingsByRuleId(businessRuleId));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="businessRuleAttributeMappings"></param>
        /// <param name="ViewId"></param>
        /// <returns></returns>
        public Boolean ProcessBusinessRuleMappings(Collection<BusinessRuleAttributeMapping> businessRuleAttributeMappings, String ViewId)
        {

            MakeServiceCall("ProcessBusinessRuleMappings", "ProcessBusinessRuleMappings", client => client.ProcessBusinessRuleMappings(businessRuleAttributeMappings, ViewId));
            return true;

        }


        #endregion

        #region BusinessRuleContextBL
        /// <summary>
        /// 
        /// </summary>
        /// <param name="businessRuleSetRules"></param>
        /// <param name="loginUser"></param>
        /// <param name="programName"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public Boolean ProcessBusinessRuleContext(Collection<BusinessRuleSetRule> businessRuleSetRules, String loginUser, String programName, String action)
        {
            MakeServiceCall("ProcessBusinessRuleContext", "ProcessBusinessRuleContext", client => client.ProcessBusinessRuleContext(businessRuleSetRules, loginUser, programName, action));
            return true;

        }

        #endregion

        #region Lookup
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="getFromSysObj"></param>
        /// <returns></returns>
        public DataTable GetTableStructure(string tableName, bool getFromSysObj)
        {
            return MakeServiceCall("GetTableStructure", "GetTableStructure", client => client.GetTableStructure(tableName, getFromSysObj));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="TableType"></param>
        /// <returns></returns>
        public DataTable GetTableTypeTemplate(int TableType)
        {
            return MakeServiceCall("GetTableTypeTemplate", "GetTableTypeTemplate", client => client.GetTableTypeTemplate(TableType));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public DataTable GetTableTypes()
        {
            return MakeServiceCall("GetTableTypes", "GetTableTypes", client => client.GetTableTypes());
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="objectType"></param>
        /// <param name="searchString"></param>
        /// <param name="getFromSysObj"></param>
        /// <param name="getAttrCountColumn"></param>
        /// <returns></returns>
        public DataTable GetTableNames(int objectType, string searchString, bool getFromSysObj, bool getAttrCountColumn)
        {
            return MakeServiceCall("GetTableNames", "GetTableNames", client => client.GetTableNames(objectType, searchString, getFromSysObj, getAttrCountColumn));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="refColumnName"></param>
        /// <param name="refMask"></param>
        /// <param name="displayColumns"></param>
        /// <param name="sortOrder"></param>
        /// <param name="searchColumns"></param>
        /// <returns></returns>
        public DataTable GetRefTableData(string tableName, string refColumnName, string refMask, string displayColumns, string sortOrder, string searchColumns)
        {
            return MakeServiceCall("GetRefTableData", "GetRefTableData", client => client.GetRefTableData(tableName, refColumnName, refMask, displayColumns, sortOrder, searchColumns));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="jobServiceId"></param>
        /// <returns></returns>
        public DataTable GetNormalizationJobResults(int jobServiceId)
        {
            return MakeServiceCall("GetNormalizationJobResults", "GetNormalizationJobResults", client => client.GetNormalizationJobResults(jobServiceId));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="applicationContextType"></param>
        /// <param name="seqDataTableforUI"></param>
        /// <returns></returns>
        public DataSet GetUserConfigContextData(int applicationContextType, string seqDataTableforUI)
        {
            return MakeServiceCall("GetUserConfigContextData", "GetUserConfigContextData", client => client.GetUserConfigContextData(applicationContextType, seqDataTableforUI));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string GetAllUOMsByUOMType()
        {
            return MakeServiceCall("GetAllUOMsByUOMType", "GetAllUOMsByUOMType", client => client.GetAllUOMsByUOMType());
        }

        /// <summary>
        /// Gets translation memory in datatable format
        /// </summary>
        /// <param name="origLocale">Indicates original locale name</param>
        /// <param name="transLocale">Indicates name of the locale which is being translated</param>
        /// <param name="origText">Indicates original text which needs to translate</param>
        /// <returns>Returns translation memory in datatable format</returns>
        public DataTable GetTranslationMemory(String origLocale, String transLocale, String origText)
        {
            return MakeServiceCall("GetTranslationMemory", "GetTranslationMemory", client => client.GetTranslationMemory(origLocale, transLocale, origText));

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="origLocale"></param>
        /// <param name="transLocale"></param>
        /// <param name="OrigText"></param>
        /// <param name="transText"></param>
        /// <param name="moduser"></param>
        /// <param name="returnValue"></param>
        /// <returns></returns>
        public bool ProcessTranslationMemory(String origLocale, String transLocale, String OrigText, String transText, String moduser, Int32 returnValue)
        {
            SqlInt32 sqlReturnValue = returnValue;
            MakeServiceCall("ProcessTranslationMemory", "ProcessTranslationMemory", client => client.ProcessTranslationMemory(origLocale, transLocale, OrigText, transText, moduser, returnValue));
            return true;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="catalogId"></param>
        /// <param name="cnodeId"></param>
        /// <param name="viewPath"></param>
        /// <returns></returns>
        public string GetCNode(int catalogId, long cnodeId, string viewPath)
        {
            return MakeServiceCall("GetCNode", "GetCNode", client => client.GetCNode(catalogId, cnodeId, viewPath));

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="catalogId"></param>
        /// <param name="nodeTypeFrom"></param>
        /// <param name="relationshipType"></param>
        /// <returns></returns>
        public DataTable GetRelationshipCardinality(int catalogId, int nodeTypeFrom, int relationshipType)
        {
            return MakeServiceCall("GetRelationshipCardinality", "GetRelationshipCardinality", client => client.GetRelationshipCardinality(catalogId, nodeTypeFrom, relationshipType));

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="txtXML"></param>
        /// <param name="userName"></param>
        /// <param name="programName"></param>
        /// <param name="returnValue"></param>
        /// <returns></returns>
        public Int32 ProcessRelationshipCardinality(string txtXML, string userName, string programName, int returnValue)
        {
            MakeServiceCall("ProcessRelationshipCardinality", "ProcessRelationshipCardinality", client => client.ProcessRelationshipCardinality(txtXML, userName, programName, returnValue));
            return returnValue;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="txtXML"></param>
        /// <param name="userName"></param>
        /// <param name="programName"></param>
        /// <param name="returnValue"></param>
        /// <returns></returns>
        public Int32 ProcessRelationshipTypeEntityTypeCardinality(string txtXML, string userName, string programName, int returnValue)
        {
            MakeServiceCall("ProcessRelationshipTypeEntityTypeCardinality", "ProcessRelationshipTypeEntityTypeCardinality", client => client.ProcessRelationshipTypeEntityTypeCardinality(txtXML, userName, programName, returnValue));
            return returnValue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nodeTypeFrom"></param>
        /// <param name="relationshipType"></param>
        /// <returns></returns>
        public DataTable GetRelationshipTypeEntityTypeCardinality(int nodeTypeFrom, int relationshipType)
        {
            return MakeServiceCall("GetRelationshipTypeEntityTypeCardinality", "GetRelationshipTypeEntityTypeCardinality", client => client.GetRelationshipTypeEntityTypeCardinality(nodeTypeFrom, relationshipType));

        }
        #endregion

        #region Container RelationshipType EntityType Mapping

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <param name="cnodeGroupIds"></param>
        /// <param name="catalogId"></param>
        /// <returns></returns>
        public Collection<ContainerRelationshipTypeEntityTypeMapping> GetContainerRelationshipTypeEntityTypeMappingsByCnodes(String user, String cnodeGroupIds, Int32 catalogId)
        {
            return MakeServiceCall("GetContainerRelationshipTypeEntityTypeMappingsByCnodes", "GetContainerRelationshipTypeEntityTypeMappingsByCnodes", client => client.GetContainerRelationshipTypeEntityTypeMappingsByCnodes(user, cnodeGroupIds, catalogId));
        }

        #endregion  Container RelationshipType EntityType Mapping

        #region Dynamic Table Schema

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbTable"></param>
        /// <param name="dynamicTableType"></param>
        /// <param name="callerContext"></param>
        /// <returns></returns>
        public OperationResult ProcessDynamicTableSchema(DBTable dbTable, DynamicTableType dynamicTableType, CallerContext callerContext)
        {
            return MakeServiceCall("ProcessDynamicTableSchema", "ProcessDynamicTableSchema", client => client.ProcessDynamicTableSchema(dbTable, dynamicTableType, FillDiagnosticTraces(callerContext)));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbTables"></param>
        /// <param name="dynamicTableType"></param>
        /// <param name="callerContext"></param>
        /// <returns></returns>
        public OperationResult ProcessDynamicTableSchemas(DBTableCollection dbTables, DynamicTableType dynamicTableType, CallerContext callerContext)
        {
            return MakeServiceCall("ProcessDynamicTableSchemas", "ProcessDynamicTableSchemas", client => client.ProcessDynamicTableSchemas(dbTables, dynamicTableType, FillDiagnosticTraces(callerContext)));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="dynamicTableType"></param>
        /// <param name="callerContext"></param>
        /// <returns></returns>
        public OperationResult GetDynamicTableSchema(Int32 id, DynamicTableType dynamicTableType, CallerContext callerContext)
        {
            return MakeServiceCall("GetDynamicTableSchema", "GetDynamicTableSchema", client => client.GetDynamicTableSchema(id, dynamicTableType, FillDiagnosticTraces(callerContext)));
        }

        #endregion

        #region Old Catalog = Entity

        /// <summary>
        /// 
        /// </summary>
        /// <param name="localeID"></param>
        /// <param name="customerID"></param>
        /// <param name="CNodeID"></param>
        /// <param name="CNodeParentID"></param>
        /// <param name="attrGroupID"></param>
        /// <param name="catalogID"></param>
        /// <param name="orgID"></param>
        /// <param name="user"></param>
        /// <param name="backupLocaleID"></param>
        /// <param name="viewPath"></param>
        /// <param name="useDraftTaxonomy"></param>
        /// <param name="encodeCollections"></param>
        /// <param name="attrIDList"></param>
        /// <returns></returns>
        public String GetCoreAttrXml(Int32 localeID, Int32 customerID, Int64 CNodeID, Int64 CNodeParentID, Int32 attrGroupID, Int32 catalogID, Int32 orgID, String user,
                    Int32 backupLocaleID, String viewPath, Boolean useDraftTaxonomy, Boolean encodeCollections, String attrIDList)
        {
            return MakeServiceCall("GetCoreAttrXml", "GetCoreAttrXml", client => client.GetCoreAttrXml(localeID, customerID, CNodeID, CNodeParentID, attrGroupID, catalogID,
                orgID, user, backupLocaleID, viewPath, useDraftTaxonomy, encodeCollections, attrIDList));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="CNodeID"></param>
        /// <param name="CNodeParentID"></param>
        /// <param name="catalogID"></param>
        /// <param name="attrGroupID"></param>
        /// <param name="localeID"></param>
        /// <param name="customerID"></param>
        /// <param name="user"></param>
        /// <param name="backupLocaleID"></param>
        /// <param name="viewPath"></param>
        /// <param name="useDraftTaxonomy"></param>
        /// <param name="encodeCollections"></param>
        /// <param name="attrIDList"></param>
        /// <returns></returns>
        public String GetTechAttrXml(Int32 CNodeID, Int32 CNodeParentID, Int32 catalogID, Int32 attrGroupID, Int32 localeID, Int32 customerID,
            String user, Int32 backupLocaleID, String viewPath, Boolean useDraftTaxonomy, Boolean encodeCollections, String attrIDList)
        {
            return MakeServiceCall("GetTechAttrXml", "GetTechAttrXml", client => client.GetTechAttrXml(CNodeID, CNodeParentID, catalogID, attrGroupID, localeID, customerID,
             user, backupLocaleID, viewPath, useDraftTaxonomy, encodeCollections, attrIDList));
        }

        #endregion

        #endregion Public Methods

        #endregion
    }
}
