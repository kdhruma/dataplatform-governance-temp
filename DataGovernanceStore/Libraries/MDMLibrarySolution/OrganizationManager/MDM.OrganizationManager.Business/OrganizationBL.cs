using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Transactions;

namespace MDM.OrganizationManager.Business
{
    using MDM.AdminManager.Business;
    using MDM.AttributeModelManager.Business;
    using MDM.BufferManager;
    using MDM.BusinessObjects;
    using MDM.ConfigurationManager.Business;
    using MDM.Core;
    using MDM.Core.Exceptions;
    using MDM.Interfaces;
    using MDM.LookupManager.Business;
    using MDM.MessageManager.Business;
    using MDM.OrganizationManager.Data;
    using MDM.Utility;
    using Attribute = MDM.BusinessObjects.Attribute;
    using MDM.KnowledgeManager.Business;
    using MDM.ActivityLogManager.Business;
    using MDM.BusinessObjects.DataModel;
    using MDM.BusinessObjects.Diagnostics;

    /// <summary>
    /// Specifies organization manager
    /// </summary>
    public class OrganizationBL : BusinessLogicBase, IOrganizationManager, IDataModelManager
    {
        #region Fields

        /// <summary>
        /// Field denoting the security principal
        /// </summary>
        private SecurityPrincipal _securityPrincipal;

        /// <summary>
        /// Data access layer for entity types
        /// </summary>
        private OrganizationDA _organizationDA = new OrganizationDA();

        /// <summary>
        /// Field denoting locale Message
        /// </summary>
        private LocaleMessage _localeMessage = null;

        /// <summary>
        /// Field denoting localeMessageBL
        /// </summary>
        private LocaleMessageBL _localeMessageBL = new LocaleMessageBL();

        /// <summary>
        /// Field denoting Organization Buffer Manager
        /// </summary>
        private OrganizationBufferManager _organizationBufferManager = new OrganizationBufferManager();

        /// <summary>
        /// Field denoting Container Buffer Manager
        /// </summary>
        private ContainerBufferManager _containerBufferManager = new ContainerBufferManager();

        /// <summary>
        /// Field denoting utility object
        /// </summary>
        private Utility _utility = new Utility();

        /// <summary>
        /// Data Security Manager object
        /// </summary>
        DataSecurityBL _dataSecurityManager = new DataSecurityBL();

        /// <summary>
        /// Filed denoting DataLocale
        /// </summary>
        private LocaleEnum _systemUILocale = LocaleEnum.UnKnown;

        /// <summary>
        /// Indicates current trace settings
        /// </summary>
        private TraceSettings _currentTraceSettings = new TraceSettings();

        #endregion

        #region Constructors

        /// <summary>
        /// 
        /// </summary>
        public OrganizationBL()
        {
            GetSecurityPrincipal();

            _systemUILocale = GlobalizationHelper.GetSystemUILocale();
            _currentTraceSettings = MDMOperationContextHelper.GetCurrentTraceSettings();
        }

        #endregion

        #region Properties
        #endregion

        #region Methods

        #region Public Methods

        #region CUD Methods

        /// <summary>
        /// creates the specified organization
        /// </summary>
        /// <param name="organization">organization to be created</param>
        /// <param name="callerContext">caller context indicating who called the API</param>
        public OperationResult Create(Organization organization, CallerContext callerContext)
        {
            return ProcessOrganization(organization, ObjectAction.Create, callerContext);
        }

        /// <summary>
        /// Updated the organization
        /// </summary>
        /// <param name="organization">organization to be updated</param>
        /// <param name="callerContext">caller context indicating who called the API</param>
        public OperationResult Update(Organization organization, CallerContext callerContext)
        {
            return ProcessOrganization(organization, ObjectAction.Update, callerContext);
        }

        /// <summary>
        /// Deletes the specified organization
        /// </summary>
        /// <param name="organization">organization to be deleted</param>
        /// <param name="callerContext">caller context indicating who called the API</param>
        public OperationResult Delete(Organization organization, CallerContext callerContext)
        {
            return ProcessOrganization(organization, ObjectAction.Delete, callerContext);
        }

        /// <summary>
        /// Create - Update or Delete given organizations
        /// </summary>
        /// <param name="organizations">Collection of organizations to process</param>
        /// <param name="callerContext">Context which called the application</param>
        /// <returns>Result of the operation</returns>
        public OperationResultCollection Process(OrganizationCollection organizations, CallerContext callerContext)
        {
            OperationResultCollection organizationProcessOperationResult = new OperationResultCollection();

            foreach (var organization in organizations)
            {
                if (!ValidateUserPermission(organization, _utility.ObjectActionToUserActionMap(organization.Action)))
                {
                    OperationResult operationResult = new OperationResult();
                    operationResult = PopulatePermissionErrorInOperationResult(organization);
                    organizationProcessOperationResult.Add((IOperationResult)operationResult);
                }
                else
                {
                    organizationProcessOperationResult.Add(this.Process(organization, callerContext));
                }
            }

            InvalidateCache(organizations, null, callerContext);

            return organizationProcessOperationResult;
        }

        #endregion

        #region Get Methods

        /// <summary>
        /// Gets organization by Id
        /// </summary>
        /// <param name="organizationId">Indicates organizationId</param>
        /// <param name="organizationContext">Indicates context containing flag to load attributes or not</param>
        /// <param name="callerContext">context indicates the properties of the caller who called the API like program name,Module</param>
        /// <returns>Organization</returns>
        public Organization GetById(Int32 organizationId, OrganizationContext organizationContext, CallerContext callerContext)
        {
            Organization organization = null;

            MDMTraceHelper.InitializeTraceSource();
            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StartTraceActivity("OrganizationBL.Get", MDMTraceSource.DataModel, false);

            try
            {
                #region Validation

                ValidateInputParameters(organizationId, organizationContext, callerContext);

                #endregion Validaton

                if (Constants.TRACING_ENABLED)
                {
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Organization Id : " + organizationId, MDMTraceSource.DataModel);
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "CallerContext : Application - " + callerContext.Application + " Module - " + callerContext.Module, MDMTraceSource.DataModel);
                }

                #region Validate User Permission

                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Validating user permission started...", MDMTraceSource.DataModel);

                if (!ValidateUserPermission(new Organization(organizationId), UserAction.View))
                {
                    throw new MDMOperationException("112254", "You do not have sufficient permission to perform to get organization", "OrganizationManager.OrganizationBL", String.Empty, "Get");
                }

                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Validating user permission completed...", MDMTraceSource.DataModel);

                #endregion

                OrganizationCollection availableOrganizations = GetAllOrganizations(organizationContext, callerContext);

                if (availableOrganizations != null && availableOrganizations.Count > 0)
                {
                    IEnumerable<Organization> organizationsEnumerableColl = availableOrganizations.Where(org => org.Id == organizationId);

                    if (organizationsEnumerableColl != null && organizationsEnumerableColl.Any())
                    {
                        organization = organizationsEnumerableColl.FirstOrDefault();
                    }
                }
            }
            finally
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StopTraceActivity("OrganizationBL.Get", MDMTraceSource.DataModel);
            }

            return organization;
        }

        /// <summary>
        /// Get All Organizations from the system
        /// </summary>
        /// <param name="organizationContext">Indicates context containing flag to load attributes or not</param>
        /// <param name="callerContext">context indicates the properties of the caller who called the API like program name,Module</param>
        /// <returns>Collection of Organizations</returns>
        public OrganizationCollection GetAll(OrganizationContext organizationContext, CallerContext callerContext)
        {
            OrganizationCollection organizations;

            MDMTraceHelper.InitializeTraceSource();
            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StartTraceActivity("OrganizationBL.GetAll", MDMTraceSource.DataModel, false);

            try
            {
                String errorMessage;

                if (callerContext == null)
                {
                    errorMessage = "CallerContext cannot be null.";
                    throw new MDMOperationException("111846", errorMessage, "OrganizationManager.OrganizationBL", String.Empty, "Get");
                }

                if (organizationContext == null)
                {
                    errorMessage = "OrganizationContext cannot be null.";
                    throw new MDMOperationException("112190", errorMessage, "OrganizationManager.OrganizationBL", String.Empty, "Get");
                }

                organizations = GetAllOrganizations(organizationContext, callerContext);
            }
            finally
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StopTraceActivity("OrganizationBL.GetAll", MDMTraceSource.DataModel);
            }

            return organizations;
        }

        /// <summary>
        /// Get All Organizations from the system
        /// </summary>
        /// <param name="organizationIds">Indicates collection of organization identifiers which needs to get</param>
        /// <param name="organizationContext">Indicates context containing flag to load attributes or not</param>
        /// <param name="callerContext">context indicates the properties of the caller who called the API like program name,Module</param>
        /// <returns>Collection of Organizations</returns>
        public OrganizationCollection GetOrganizationsByIds(Collection<Int32> organizationIds, OrganizationContext organizationContext, CallerContext callerContext)
        {
            var diagnosticActivity = new DiagnosticActivity();
            OrganizationCollection filteredOrganizations = new OrganizationCollection();

            try
            {
                if (_currentTraceSettings.IsBasicTracingEnabled)
                {
                    diagnosticActivity.Start();
                }

                #region Parameter validation

                String errorMessage;

                if (callerContext == null)
                {
                    errorMessage = "CallerContext cannot be null";
                    diagnosticActivity.LogError("111846", errorMessage);
                    throw new MDMOperationException("111846", errorMessage, "OrganizationManager.OrganizationBL", String.Empty, "Get");
                }

                if (organizationContext == null)
                {
                    errorMessage = "OrganizationContext cannot be null.";
                    diagnosticActivity.LogError("112190", errorMessage);
                    throw new MDMOperationException("112190", errorMessage, "OrganizationManager.OrganizationBL", String.Empty, "Get");
                }

                if (organizationIds == null || organizationIds.Count < 1)
                {
                    _localeMessage = _localeMessageBL.Get(GlobalizationHelper.GetSystemUILocale(), "113960", new Object[] { "OrganizationIdList" }, false, callerContext);
                    diagnosticActivity.LogError("113960", _localeMessage.Message);
                    throw new MDMOperationException("113960", _localeMessage.Message, "OrganizationBL.GetByIds", String.Empty, "Get");
                }

                #endregion Parameter validation

                #region Get all Organizations

                OrganizationCollection organizations = GetAllOrganizations(organizationContext, callerContext);

                #endregion Get all Organizations

                #region Filter Organizations

                if (organizations != null && organizations.Count > 0)
                {
                    foreach (Organization organization in organizations)
                    {
                        if (organizationIds.Contains(organization.Id))
                        {
                            filteredOrganizations.Add(organization);
                        }
                    }
                }

                #endregion Filter Organizations
            }
            finally
            {
                if (_currentTraceSettings.IsBasicTracingEnabled)
                {
                    diagnosticActivity.Stop();
                }
            }

            return filteredOrganizations;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="organizationShortName"></param>
        /// <param name="organizationContext"></param>
        /// <param name="callerContext">caller context indicating who called the API</param>
        /// <returns></returns>
        public Organization GetByName(String organizationShortName, OrganizationContext organizationContext, CallerContext callerContext)
        {
            Organization organization = null;

            MDMTraceHelper.InitializeTraceSource();
            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StartTraceActivity("OrganizationBL.GetByName", MDMTraceSource.DataModel, false);

            try
            {
                #region Validation

                String errorMessage;

                if (String.IsNullOrEmpty(organizationShortName))
                {
                    errorMessage = this.GetSystemLocaleMessage("111865", callerContext).Message;
                    throw new MDMOperationException("111865", errorMessage, "OrganizationManager.OrganizationBL", String.Empty, "GetByName");
                }

                if (organizationContext == null)
                {
                    errorMessage = "OrganizationContext cannot be null.";
                    throw new MDMOperationException("112190", errorMessage, "OrganizationManager.OrganizationBL", String.Empty, "GetByName", MDMTraceSource.DataModel);
                }

                if (callerContext == null)
                {
                    errorMessage = "CallerContext cannot be null.";
                    throw new MDMOperationException("111846", errorMessage, "OrganizationManager.OrganizationBL", String.Empty, "GetByName", MDMTraceSource.DataModel);
                }

                #endregion Validaton

                if (Constants.TRACING_ENABLED)
                {
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Organization Name : " + organizationShortName, MDMTraceSource.DataModel);
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "CallerContext : Application - " + callerContext.Application + " Module - " + callerContext.Module, MDMTraceSource.DataModel);
                }

                OrganizationCollection availableOrganizations = GetAllOrganizations(organizationContext, callerContext);

                if (availableOrganizations != null && availableOrganizations.Count > 0)
                {
                    organization = availableOrganizations.Get(organizationShortName);
                }
            }
            finally
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StopTraceActivity("OrganizationBL.GetByName", MDMTraceSource.DataModel);
            }

            return organization;
        }

        /// <summary>
        /// Get All Organizations childs
        /// </summary>
        /// <param name="parentOrganizationId"></param>
        /// <param name="callerContext">context indicates the properties of the caller who called the API like program name,Module</param>
        /// <returns>Collection of Organizations</returns>
        public MDMObjectInfoCollection GetAllOrganizationDependencies(Int32 parentOrganizationId, CallerContext callerContext)
        {
            MDMObjectInfoCollection organizationChilds;

            MDMTraceHelper.InitializeTraceSource();
            MDMTraceHelper.StartTraceActivity("OrganizationBL.GetAllOrganizationDependencies", MDMTraceSource.DataModel, false);

            try
            {
                String errorMessage;

                if (parentOrganizationId < 0)
                {
                    errorMessage = this.GetSystemLocaleMessage("111864", callerContext).Message;
                    throw new MDMOperationException("111864", errorMessage, "OrganizationManager.OrganizationBL", String.Empty, "Get");
                }

                if (callerContext == null)
                {
                    errorMessage = "CallerContext cannot be null.";
                    throw new MDMOperationException("111846", errorMessage, "OrganizationManager.OrganizationBL", String.Empty, "Get");
                }

                OrganizationDA organizationDA = new OrganizationDA();

                DBCommandProperties command = DBCommandHelper.Get(callerContext, MDMCenterModuleAction.Read);

                organizationChilds = organizationDA.GetAllOrganizationDependencies(parentOrganizationId, command);
            }
            finally
            {
                MDMTraceHelper.StopTraceActivity("OrganizationBL.GetAllOrganizationDependencies", MDMTraceSource.DataModel);
            }

            return organizationChilds;
        }

        #endregion

        #region IDataModelManager Methods

        /// <summary>
        /// Validates DataModelObjects and populate OperationResults
        /// </summary>
        /// <param name="iDataModelObjects">Collection of data model objects to be filled up.</param>
        /// <param name="operationResult">Collection of DataModel OperationResult</param>
        /// <param name="iCallerContext">Context of caller making call to this API.</param>
        public void Validate(IDataModelObjectCollection iDataModelObjects, DataModelOperationResultCollection operationResults, ICallerContext iCallerContext)
        {
            #region Parameter Validations

            OrganizationCollection organizations = iDataModelObjects as OrganizationCollection;
            CallerContext callerContext = iCallerContext as CallerContext;

            ValidateInputParameters(organizations as OrganizationCollection, operationResults, callerContext);

            #endregion

            #region Validate Permission

            foreach (Organization organization in organizations as OrganizationCollection)
            {
                if (!ValidateUserPermission(organization, _utility.ObjectActionToUserActionMap(organization.Action)))
                {
                    PopulatePermissionErrorInOperationResult(operationResults, organization);
                }
            }

            #endregion
        }

        /// <summary>
        /// Process data model collection
        /// </summary>
        /// <param name="iDataModelObjects">Collection of data model objects to be processed.</param>
        /// <param name="iCallerContext">Context of caller making call to this API.</param>
        /// <returns>Result of operation</returns>
        public void Process(IDataModelObjectCollection iDataModelObjects, DataModelOperationResultCollection operationResults, ICallerContext iCallerContext)
        {
            OrganizationCollection organizations = iDataModelObjects as OrganizationCollection;
            CallerContext callerContext = (CallerContext)iCallerContext;

            if (organizations.Count > 0)
            {
                String userName = PopulateUserName();
                PopulateProgramName((CallerContext)iCallerContext);
                LocaleEnum systemDataLocale = GlobalizationHelper.GetSystemDataLocale();

                DBCommandProperties command = DBCommandHelper.Get((CallerContext)iCallerContext, MDMCenterModuleAction.Execute);

                #region Perform organization updates in database

                using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions() { IsolationLevel = Constants.DEFAULT_ISOLATION_LEVEL }))
                {

                    _organizationDA.Process(organizations, operationResults, callerContext.ProgramName, userName, systemDataLocale, command);

                    transactionScope.Complete();
                }

                LocalizeErrors(callerContext, operationResults);

                #endregion

            }
        }

        /// <summary>
        /// Load original data model objects
        /// </summary>
        /// <param name="iDataModelObjects">Collection of data model objects to load original object.</param>
        /// <param name="operationResult">Collection of DataModel OperationResult</param>
        /// <param name="iCallerContext">Context of caller making call to this API.</param>
        public void LoadOriginal(IDataModelObjectCollection iDataModelObjects, DataModelOperationResultCollection operationResult, ICallerContext iCallerContext)
        {
            OrganizationCollection organizations = iDataModelObjects as OrganizationCollection;

            if (organizations != null)
            {
                LoadOriginalOrganizations(organizations, iCallerContext as CallerContext);
            }
        }

        /// <summary>
        /// Compare and merge data model object collection with current collection
        /// </summary>
        /// <param name="iDataModelObjects">Collection of data model objects to be compare and merge.</param>
        /// <param name="operationResult">Collection of DataModel OperationResult</param>
        /// <param name="iCallerContext">Context of caller making call to this API.</param>
        /// <returns>Returns merged data model objects.</returns>
        public void CompareAndMerge(IDataModelObjectCollection iDataModelObjects, DataModelOperationResultCollection operationResults, ICallerContext iCallerContext)
        {
            CompareAndMerge(iDataModelObjects as OrganizationCollection, operationResults, iCallerContext as CallerContext);
        }

        /// <summary>
        /// Processes the Entity cache statuses for data model objects
        /// </summary>
        /// <param name="iDataModelObjects">Collection of data model objects to be process entity cache load.</param>
        /// <param name="iCallerContext">Context of caller making call to this API.</param>
        public void InvalidateCache(IDataModelObjectCollection iDataModelObjects, DataModelOperationResultCollection operationResult, ICallerContext iCallerContext)
        {
            // clear buffer manager
            _organizationBufferManager.RemoveOrganizations(true);

            // Clear container cache
            _containerBufferManager.RemoveContainers();
        }

        /// <summary>
        /// Fills missing information to data model objects.
        /// </summary>
        /// <param name="iDataModelObjects">Collection of data model objects to be filled up.</param>
        /// <param name="iCallerContext">Context of caller making call to this API.</param>
        public void FillDataModel(IDataModelObjectCollection iDataModelObjects, ICallerContext iCallerContext)
        {
            FillOrganizations(iDataModelObjects as OrganizationCollection, iCallerContext as CallerContext);
        }

        /// <summary>
        /// Generate and OperationResult Schema for given DataModelObjectCollection
        /// </summary>
        /// <param name="iDataModelObjects"></param>
        public void PrepareOperationResultsSchema(IDataModelObjectCollection iDataModelObjects, DataModelOperationResultCollection operationResultCollection)
        {
            OrganizationCollection organizations = iDataModelObjects as OrganizationCollection;

            if (organizations != null && organizations.Count > 0)
            {
                if (operationResultCollection.Count > 0)
                {
                    operationResultCollection.Clear();
                }

                int organizationIdToBeCreated = -1;

                foreach (Organization organization in organizations)
                {
                    DataModelOperationResult organizationOperationResult = new DataModelOperationResult(organization.Id, organization.LongName, organization.ExternalId, organization.ReferenceId);

                    organizationOperationResult.DataModelObjectType = ObjectType.Organization;

                    if (String.IsNullOrEmpty(organizationOperationResult.ExternalId))
                    {
                        organizationOperationResult.ExternalId = organization.Name;
                    }

                    if (organization.Id < 1)
                    {
                        organization.Id = organizationIdToBeCreated;
                        organizationOperationResult.Id = organizationIdToBeCreated;
                        organizationIdToBeCreated--;
                    }

                    operationResultCollection.Add(organizationOperationResult);
                }
            }
        }

        #endregion

        #endregion

        #region Private Methods

        #region Validation Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="organizationId"></param>
        /// <param name="organizationContext"></param>
        /// <param name="callerContext"></param>
        private void ValidateInputParameters(Int32 organizationId, OrganizationContext organizationContext, CallerContext callerContext)
        {
            String errorMessage;

            if (organizationId < 0)
            {
                errorMessage = this.GetSystemLocaleMessage("111864", callerContext).Message;
                throw new MDMOperationException("111864", errorMessage, "OrganizationManager.OrganizationBL", String.Empty, "Get");
            }

            if (organizationContext == null)
            {
                errorMessage = "OrganizationContext cannot be null.";
                throw new MDMOperationException("112190", errorMessage, "OrganizationManager.OrganizationBL", String.Empty, "Get", MDMTraceSource.DataModel);
            }

            if (callerContext == null)
            {
                errorMessage = "CallerContext cannot be null.";
                throw new MDMOperationException("111846", errorMessage, "OrganizationManager.OrganizationBL", String.Empty, "Get", MDMTraceSource.DataModel);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="organization"></param>
        /// <param name="callerContext"></param>
        private void ValidateInputParameters(Organization organization, CallerContext callerContext)
        {
            if (callerContext == null)
            {
                throw new MDMOperationException("111846", "CallerContext cannot be null.", "OrganizationManager.OrganizationBL", String.Empty, "Create");
            }

            if (organization == null)
            {
                throw new MDMOperationException("112192", "Organization cannot be null.", "OrganizationManager.OrganizationBL", String.Empty, "Create");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="organizations"></param>
        /// <param name="callerContext"></param>
        private void ValidateInputParameters(OrganizationCollection organizations, DataModelOperationResultCollection operationResults, CallerContext callerContext)
        {
            Collection<String> shortNames = new Collection<String>();

            if (callerContext == null)
            {
                AddOperationResults(operationResults, "111846", String.Empty, null, OperationResultType.Error, TraceEventType.Error, callerContext);
            }
            if (organizations == null || organizations.Count < 1)
            {
                AddOperationResults(operationResults, "113592", String.Empty, null, OperationResultType.Error, TraceEventType.Error, callerContext);
            }
            else
            {
                foreach (Organization organization in organizations)
                {
                    IDataModelOperationResult operationResult = operationResults.GetByReferenceId(organization.ReferenceId);

                    if (String.IsNullOrWhiteSpace(organization.Name))
                    {
                        AddOperationResult(operationResult, "111865", String.Empty, null, OperationResultType.Error, TraceEventType.Error, callerContext);
                    }
                    else
                    {
                        if (shortNames.Contains(organization.Name))
                        {
                            AddOperationResult(operationResult, "112187", String.Empty, null, OperationResultType.Error, TraceEventType.Error, callerContext);
                        }
                        else
                        {
                            shortNames.Add(organization.Name);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="organization"></param>
        /// <param name="userAction"></param>
        /// <returns></returns>
        private Boolean ValidateUserPermission(Organization organization, UserAction userAction)
        {
            Permission permission = null;
            Int32 objectTypeId = (Int32)ObjectType.Organization;

            PermissionContext permissionContext = new PermissionContext(organization.Id, 0, 0, 0, 0, 0, 0, 0, _securityPrincipal.CurrentUserId, 0);
            permission = _dataSecurityManager.GetMDMObjectPermission(organization.Id, objectTypeId, ObjectType.Organization.ToString(), permissionContext);

            if (permission != null)
                organization.PermissionSet = permission.PermissionSet;

            return (permission == null ? false : permission.PermissionSet.Contains(userAction));
        }

        #region ValidateCircularDependency

        private void ValidateCircularDependency(OrganizationCollection organizations, DataModelOperationResultCollection operationResults, CallerContext callerContext)
        {
            if (organizations != null)
            {
                OrganizationContext organizationContext = new OrganizationContext();
                organizationContext.LoadAttributes = true;

                OrganizationCollection orgs = GetAllOrganizations(organizationContext, callerContext);

                if (orgs == null)
                {
                    orgs = organizations;
                }
                else
                {
                    orgs.AddRange(organizations);
                }

                foreach (Organization org in organizations)
                {
                    Boolean hasCircularDependency = HasCircularDependency(orgs, org, new Collection<String>() { org.Name });

                    if (hasCircularDependency)
                    {
                        IDataModelOperationResult organizationOperationResult = operationResults.GetByReferenceId(org.ReferenceId);
                        AddOperationResult(organizationOperationResult, "110641", String.Empty, null, OperationResultType.Error, TraceEventType.Error, callerContext);
                    }
                }
            }
        }

        private Boolean HasCircularDependency(OrganizationCollection orgs, Organization childOrganisation, Collection<String> organizationNames)
        {
            Boolean hasCircularDependency = false;

            if (orgs != null && childOrganisation != null)
            {
                Organization parentOrganization = orgs.GetOrganizationByParentName(childOrganisation.Name);

                if (parentOrganization != null)
                {
                    organizationNames = organizationNames ?? new Collection<String>();
                    String parentOrganizationName = parentOrganization.Name;

                    if (organizationNames.Contains(parentOrganizationName))
                    {
                        //Found the circular dependency.
                        hasCircularDependency = true;
                    }
                    else
                    {
                        //There are more parent find the dependency...
                        organizationNames.Add(parentOrganizationName);
                        hasCircularDependency = this.HasCircularDependency(orgs, parentOrganization, organizationNames);
                    }
                }
            }

            return hasCircularDependency;
        }

        #endregion ValidateCircularDependency

        #endregion

        #region Get Methods

        /// <summary>
        /// Gets all organizations
        /// </summary>
        /// <param name="organizationContext"></param>
        /// <param name="callerContext">caller context indicating who called the API</param>
        /// <returns>Organizatin Collection</returns>
        private OrganizationCollection GetAllOrganizations(OrganizationContext organizationContext, CallerContext callerContext)
        {
            OrganizationCollection permittedOrganizations = new OrganizationCollection();

            OrganizationCollection organizations = organizationContext.LoadAttributes ?
                _organizationBufferManager.FindAllOrganizationsWithAttributes() : _organizationBufferManager.FindAllOrganizations();

            if (organizations == null || organizations.Count < 1)
            {
                OrganizationDA organizationDa = new OrganizationDA();

                DBCommandProperties command = DBCommandHelper.Get(callerContext, MDMCenterModuleAction.Read);

                organizations = organizationDa.GetAll(organizationContext, command);

                if (organizationContext.LoadAttributes)
                {
                    GetOrganizationAttributesUpdated(organizations, callerContext);
                }

                if (organizations != null && organizations.Count > 0)
                {
                    _organizationBufferManager.UpdateOrganizations(organizations, organizationContext, 3);
                }
            }

            #region Validate User Permission

            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Validating user permission started...", MDMTraceSource.DataModel);

            //Set default value for has permission as true.In case no organizations are available in database.
            Boolean hasPermission = true;

            if (organizations != null && organizations.Count > 0)
            {
                hasPermission = false;

                foreach (Organization organization in organizations)
                {
                    if (ValidateUserPermission(organization, UserAction.View))
                    {
                        permittedOrganizations.Add(organization);
                        hasPermission = true;
                    }
                }
            }

            if (!hasPermission)
            {
                throw new MDMOperationException("112254", "You do not have sufficient permission to  get organization", "OrganizationManager.OrganizationBL", String.Empty, "Get");
            }
            else
            {
                Int32 objectTypeId = (Int32)ObjectType.Organization;
                PermissionContext permissionContext = new PermissionContext(0, 0, 0, 0, 0, 0, 0, 0, _securityPrincipal.CurrentUserId, 0);
                Permission permission = _dataSecurityManager.GetMDMObjectPermission(0, objectTypeId, ObjectType.Organization.ToString(), permissionContext);

                if (permission != null)
                    permittedOrganizations.PermissionSet = permission.PermissionSet;
            }

            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Validating user permission completed...", MDMTraceSource.DataModel);

            #endregion

            return permittedOrganizations;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="organizationCollection"></param>
        /// <param name="callerContext"></param>
        private void GetOrganizationAttributesUpdated(OrganizationCollection organizationCollection, CallerContext callerContext)
        {
            if (organizationCollection != null)
            {
                //AttributeModelCollection attributeModelCollection = new AttributeModelCollection();
                Collection<Int32> attributeIds = new Collection<Int32>();

                AttributeModelBL attributeModelManager = new AttributeModelBL();

                //Get unique list of organization attributes
                foreach (Organization organization in organizationCollection)
                {
                    if (organization.Attributes != null && organization.Attributes.Count > 0)
                    {
                        foreach (Attribute attribute in organization.Attributes)
                        {
                            if (!attributeIds.Contains(attribute.Id))
                            {
                                attributeIds.Add(attribute.Id);
                            }
                        }
                    }
                }

                //get attribute models
                AttributeModelContext attributeModelContext = new AttributeModelContext();
                attributeModelContext.GetCompleteDetailsOfAttribute = true;
                attributeModelContext.Locales.Add(GlobalizationHelper.GetSystemDataLocale());
                attributeModelContext.AttributeModelType = AttributeModelType.System;

                if (attributeIds.Count > 0)
                {
                    AttributeModelCollection attributeModels = attributeModelManager.GetByIds(attributeIds, attributeModelContext);

                    foreach (Organization organization in organizationCollection)
                    {
                        if (organization.Attributes != null && organization.Attributes.Count > 0)
                        {
                            AttributeCollection rawOrganizationAttributeCollection = new AttributeCollection(organization.Attributes.ToXml());
                            ApplicationContext applicationContext = new ApplicationContext();
                            PopulateApplicationContext(organization, applicationContext);

                            organization.Attributes.Clear();

                            foreach (Attribute attribute in rawOrganizationAttributeCollection)
                            {
                                IAttributeModel iOrganizationAttributeModel = attributeModels.GetAttributeModel(attribute.Id, attribute.Locale);

                                if (iOrganizationAttributeModel != null)
                                {
                                    Attribute organizationAttribute = new Attribute((AttributeModel)iOrganizationAttributeModel)
                                    {
                                        SourceFlag = attribute.SourceFlag,
                                        AttributeModelType = attribute.AttributeModelType,
                                        Locale = attribute.Locale,
                                        Action = ObjectAction.Read
                                    };

                                    IValueCollection values = attribute.GetCurrentValuesInvariant();

                                    if (values != null)
                                    {
                                        organizationAttribute.SetValueInvariant(values);
                                    }

                                    if (iOrganizationAttributeModel.IsLookup)
                                    {
                                        PopulateLookupValues(organizationAttribute, applicationContext, callerContext);
                                    }

                                    organization.Attributes.Add(organizationAttribute);

                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="organization"></param>
        /// <param name="applicationContext"></param>
        private void PopulateApplicationContext(Organization organization, ApplicationContext applicationContext)
        {
            applicationContext.OrganizationId = organization.Id;
            applicationContext.OrganizationName = organization.Name;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="attribute"></param>
        /// <param name="applicationContext"></param>
        /// <param name="callerContext"></param>
        private void PopulateLookupValues(Attribute attribute, ApplicationContext applicationContext, CallerContext callerContext)
        {
            LookupBL lookupManager = new LookupBL();

            ValueCollection values = (ValueCollection)attribute.GetCurrentValuesInvariant();

            if (values != null && values.Count > 0)
            {
                //Create list of lookup PK list to fetch
                Collection<Int32> lookupValueRefIdList = new Collection<Int32>();
                foreach (Value value in values)
                {
                    if (value.AttrVal != null)
                    {
                        lookupValueRefIdList.Add(ValueTypeHelper.Int32TryParse(value.AttrVal.ToString(), -1));
                    }
                }

                if (lookupValueRefIdList.Count > 0)
                {
                    //Get lookup table with provided list of PK..
                    Lookup lookup = lookupManager.Get(attribute.Id, attribute.Locale, -1, lookupValueRefIdList, applicationContext, callerContext, false);

                    if (lookup != null && lookup.Rows.Count > 0)
                    {
                        foreach (Value value in values)
                        {
                            if (value.AttrVal != null)
                            {
                                value.ValueRefId = ValueTypeHelper.Int32TryParse(value.AttrVal.ToString(), -1);
                            }

                            if (value.ValueRefId < 1)
                                continue;

                            Row lookupRow = (Row)lookup.GetRecordById(value.ValueRefId);

                            if (lookupRow != null)
                            {
                                Object displayValObj = lookupRow.GetValue(Lookup.DisplayFormatColumnName);

                                if (displayValObj != null)
                                {
                                    String displayVal = displayValObj.ToString();

                                    if (!String.IsNullOrWhiteSpace(displayVal))
                                        value.SetDisplayValue(displayVal);
                                }

                                Object exportValObj = lookupRow.GetValue(Lookup.ExportFormatColumnName);

                                if (exportValObj != null)
                                {
                                    String exportVal = exportValObj.ToString();

                                    if (!String.IsNullOrWhiteSpace(exportVal))
                                        value.SetExportValue(exportVal);
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="organizations"></param>
        /// <param name="callerContext"></param>
        private void LoadOriginalOrganizations(OrganizationCollection organizations, CallerContext callerContext)
        {
            OrganizationContext organizationContext = new OrganizationContext();
            organizationContext.LoadAttributes = true;

            OrganizationCollection originalOrganizations = GetAllOrganizations(organizationContext, callerContext);

            if (originalOrganizations != null && originalOrganizations.Count > 0)
            {
                foreach (Organization organization in organizations)
                {
                    organization.OriginalOrganization = originalOrganizations.Get(organization.Name);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="organizations"></param>
        /// <param name="callerContext"></param>
        private void FillOrganizations(OrganizationCollection organizations, CallerContext callerContext)
        {
            foreach (Organization organization in organizations)
            {
                if (organization.OriginalOrganization != null)
                {
                    organization.Id = organization.OriginalOrganization.Id;
                    organization.OrganizationTypeId = organization.OriginalOrganization.OrganizationTypeId;
                    organization.OrganizationParent = organization.OriginalOrganization.OrganizationParent;

                    if (String.IsNullOrWhiteSpace(organization.LongName))
                    {
                        organization.LongName = organization.OriginalOrganization.LongName;
                    }
                }

                if (organization.OrganizationParent < 1 && !String.IsNullOrWhiteSpace(organization.ParentOrganizationName))
                {
                    Organization parentOrganization = GetByName(organization.ParentOrganizationName, new OrganizationContext(), callerContext);

                    if (parentOrganization != null)
                    {
                        organization.OrganizationParent = parentOrganization.Id;
                    }
                }
            }
        }

        /// <summary>
        /// Processes the organization based on action
        /// </summary>
        /// <param name="organization">Organization to be processed</param>
        /// <param name="objectAction">Indicates action of an organization</param>
        /// <param name="callerContext">Caller context indicating who called the API</param>
        private OperationResult ProcessOrganization(Organization organization, ObjectAction objectAction, CallerContext callerContext)
        {
            OperationResult operationResult = new OperationResult();

            ValidateInputParameters(organization, callerContext);
            organization.Action = objectAction;
            OrganizationCollection organizations = new OrganizationCollection() { organization };

            operationResult = this.Process(organization, callerContext);

            InvalidateCache(organizations, null, callerContext);

            return operationResult;
        }

        #endregion

        #region Process Methods

        /// <summary>
        /// Create - Update or Delete given taxonomy
        /// </summary>
        /// <param name="organization">Taxonomy to process</param>
        /// <param name="callerContext">Context which called the application</param>
        /// <returns>Result of the operation</returns>
        private OperationResult Process(Organization organization, CallerContext callerContext)
        {
            MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "CallerContext : Application - " + callerContext.Application + " Module - " + callerContext.Module, MDMTraceSource.DataModel);

            OperationResult organizationProcessOperationResult = new OperationResult();

            String userName = String.Empty;

            userName = PopulateUserName();

            PopulateProgramName(callerContext);

            LocaleEnum systemDataLocale = GlobalizationHelper.GetSystemDataLocale();
            OrganizationCollection organizations = new OrganizationCollection() { organization };

            try
            {
                #region Validate Permission

                if (!ValidateUserPermission(organization, _utility.ObjectActionToUserActionMap(organization.Action)))
                {
                    return PopulatePermissionErrorInOperationResult(organization);
                }

                #endregion

                using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions() { IsolationLevel = Constants.DEFAULT_ISOLATION_LEVEL }))
                {
                    DBCommandProperties command = DBCommandHelper.Get(callerContext, MDMCenterModuleAction.Execute);

                    organizationProcessOperationResult = _organizationDA.Process(organization, callerContext.ProgramName, userName, systemDataLocale, command);

                    LocalizeErrors(callerContext, organizationProcessOperationResult);

                    transactionScope.Complete();
                }

                InvalidateCache(organizations, null, callerContext);
            }
            catch (Exception ex)
            {
                throw new MDMOperationException(ex.Message, ex);
            }

            PopulateOperationResult(organization, organizationProcessOperationResult);

            #region activity log

            if (organizationProcessOperationResult.OperationResultStatus == OperationResultStatusEnum.Successful && AppConfigurationHelper.GetAppConfig<Boolean>(Constants.ACTIVITYLOG_DATAMODEL_ENABLED_APPCONFIG_KEY, false))
            {
                LogDataModelChanges(organizations, callerContext);
            }

            #endregion activity log


            return organizationProcessOperationResult;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="organizations"></param>
        /// <param name="callerContext"></param>
        private void CompareAndMerge(OrganizationCollection organizations, DataModelOperationResultCollection operationResults, CallerContext callerContext)
        {
            foreach (Organization deltaOrganization in organizations)
            {
                IDataModelOperationResult organizationOperationResult = operationResults.GetByReferenceId(deltaOrganization.ReferenceId);

                //If delta object Action is Read/Ignore then skip Merge Delta Process.
                if (deltaOrganization.Action == ObjectAction.Read || deltaOrganization.Action == ObjectAction.Ignore)
                {
                    continue;
                }

                IOrganization origOrganization = deltaOrganization.OriginalOrganization;

                if (origOrganization != null)
                {
                    bool organizationMatched = origOrganization.Equals(deltaOrganization);

                    if (deltaOrganization.Action != ObjectAction.Delete)
                    {
                        if (organizationMatched)
                        {
                            deltaOrganization.Action = ObjectAction.Read;
                        }
                        else
                        {
                            deltaOrganization.Action = ObjectAction.Update;
                            deltaOrganization.SetAttributes(origOrganization.GetAttributes());
                        }
                    }
                }
                else
                {
                    if (deltaOrganization.Action == ObjectAction.Delete)
                    {
                        AddOperationResult(organizationOperationResult, "113598", String.Empty, new Object[] { deltaOrganization.Name }, OperationResultType.Error, TraceEventType.Error, callerContext);
                    }
                    else
                    {
                        if (!String.IsNullOrWhiteSpace(deltaOrganization.LongName) && (String.IsNullOrWhiteSpace(deltaOrganization.ParentOrganizationName) || deltaOrganization.OrganizationParent > 0))
                        {
                            if (deltaOrganization.OrganizationTypeId == 0)
                            {
                                deltaOrganization.OrganizationTypeId = 1; // CORP/Corporate by default
                            }
                        }
                        else
                        {
                            if (!String.IsNullOrWhiteSpace(deltaOrganization.ParentOrganizationName) && deltaOrganization.OrganizationParent < 1)
                            {
                                AddOperationResult(organizationOperationResult, "113670", "Organization parent: {0} is invalid.", new Object[] { deltaOrganization.ParentOrganizationName }, OperationResultType.Error, TraceEventType.Error, callerContext);
                            }

                            if (String.IsNullOrWhiteSpace(deltaOrganization.LongName))
                            {
                                AddOperationResult(organizationOperationResult, "113672", "Organization long name is empty or not specified.", null, OperationResultType.Error, TraceEventType.Error, callerContext);
                            }
                        }
                        deltaOrganization.Action = ObjectAction.Create;
                    }
                }
                organizationOperationResult.PerformedAction = deltaOrganization.Action;
            }

            ValidateCircularDependency(organizations, operationResults, callerContext);
        }
        #endregion

        #region Misc. Methods

        /// <summary>
        /// 
        /// </summary>
        private void GetSecurityPrincipal()
        {
            if (_securityPrincipal == null)
            {
                _securityPrincipal = SecurityPrincipalHelper.GetCurrentSecurityPrincipal();
            }
        }

        /// <summary>
        /// //if there are any error codes coming in OR from db, populate error messages for them
        /// </summary>
        /// <param name="callerContext">The caller context</param>
        /// <param name="entityTypeProcessOperationResult">Operation result to be modified</param>
        private void LocalizeErrors(CallerContext callerContext, OperationResult entityTypeProcessOperationResult)
        {
            foreach (Error error in entityTypeProcessOperationResult.Errors)
            {
                if (!String.IsNullOrEmpty(error.ErrorCode))
                {
                    _localeMessage = _localeMessageBL.Get(_systemUILocale, error.ErrorCode, false, callerContext);

                    if (_localeMessage != null)
                    {
                        error.ErrorMessage = _localeMessage.Message;
                    }
                }
            }
        }

        /// <summary>
        /// if there are any error codes coming in OR from db, populate error messages for them
        /// </summary>
        /// <param name="callerContext">CallerContext</param>
        /// <param name="operationResults">DataModelOperationResultCollection</param>
        private void LocalizeErrors(CallerContext callerContext, DataModelOperationResultCollection operationResults)
        {
            foreach (DataModelOperationResult operationResult in operationResults)
            {
                foreach (Error error in operationResult.Errors)
                {
                    if (!String.IsNullOrEmpty(error.ErrorCode) && String.IsNullOrEmpty(error.ErrorMessage))
                    {
                        if (String.Compare(error.ErrorCode, "112189") == 0)
                        {
                           //Here the parameters will not be having any value as this is only required to show the link when trying to delete the organization from UI.
                            Object[] parameters = new Object[] { " " };
                            _localeMessage = _localeMessageBL.Get(_systemUILocale, error.ErrorCode, parameters, false, callerContext);
                        }
                        else
                        {
                            _localeMessage = _localeMessageBL.Get(_systemUILocale, error.ErrorCode, false, callerContext);
                        }

                        if (_localeMessage != null)
                        {
                            error.ErrorMessage = _localeMessage.Message;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Returns Message Object based on message code
        /// </summary>
        /// <param name="messageCode">Message Code</param>
        /// <param name="callerContext">caller context indicating who called the API</param>
        /// <returns></returns>
        private LocaleMessage GetSystemLocaleMessage(String messageCode, CallerContext callerContext)
        {
            LocaleMessageBL localeMessageBL = new LocaleMessageBL();

            return localeMessageBL.Get(_systemUILocale, messageCode, false, callerContext);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private String PopulateUserName()
        {
            String userName = String.Empty;

            if (_securityPrincipal != null)
            {
                userName = _securityPrincipal.CurrentUserName;
            }

            return userName;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="callerContext"></param>
        private static void PopulateProgramName(CallerContext callerContext)
        {
            if (String.IsNullOrWhiteSpace(callerContext.ProgramName))
            {
                callerContext.ProgramName = "OrganizationBL.Process";

            }
        }
    
        /// <summary>
        /// Log changes to tb_datamodelactivitylog if DA call is sucessfull and appconfig key datamodelactivitylog.enable == true
        /// </summary>
        /// <param name="organizationCollection"></param>
        /// <param name="callerContext"></param>
        private void LogDataModelChanges(OrganizationCollection organizationCollection, CallerContext callerContext)
        {
            #region step: populate datamodelactivitylog object

            DataModelActivityLogBL dataModelActivityLogManager = new DataModelActivityLogBL();
            DataModelActivityLogCollection activityLogCollection = dataModelActivityLogManager.FillDataModelActivityLogCollection(organizationCollection);

            #endregion step: populate datamodelactivitylog object

            #region step: make api call

            if (activityLogCollection != null) // null activitylog collection means there was error in mapping
            {
                dataModelActivityLogManager.Process(activityLogCollection, callerContext);
            }

            #endregion step: make api call
        }

        #endregion

        #region Populate Operation Result Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="organization"></param>
        /// <param name="organizationProcessOperationResult"></param>
        private void PopulateOperationResult(Organization organization, OperationResult organizationProcessOperationResult)
        {
            if (organization.Action == ObjectAction.Create)
            {
                if (organizationProcessOperationResult.ReturnValues.Any())
                {
                    organizationProcessOperationResult.Id =
                        ValueTypeHelper.Int32TryParse(organizationProcessOperationResult.ReturnValues[0].ToString(), -1);
                }
            }
            else
            {
                organizationProcessOperationResult.Id = organization.Id;
            }

            organizationProcessOperationResult.ReferenceId = String.IsNullOrEmpty(organization.ReferenceId)
                ? organization.Name
                : organization.ReferenceId;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="organization"></param>
        /// <returns></returns>
        private OperationResult PopulatePermissionErrorInOperationResult(Organization organization)
        {
            OperationResult operationResult = new OperationResult();

            PopulateOperationResult(organization, operationResult);

            operationResult.AddOperationResult("112255", String.Format("You do not have sufficient permission to perform {0} operation on Organization : {1}", organization.Action, organization.LongName), OperationResultType.Error);
            MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, String.Format("You do not have sufficient permission to perform {0} operation on Organization : {1}", organization.Action, organization.LongName), MDMTraceSource.APIFramework);

            return operationResult;
        }

        /// <summary>
        /// Find and populate Permission Error in DMORC
        /// </summary>
        /// <param name="operationResults">DataModelOperationResultCollection</param>
        /// <param name="organization">Organization</param>
        /// <returns></returns>
        private void PopulatePermissionErrorInOperationResult(DataModelOperationResultCollection operationResults, Organization organization)
        {
            IOperationResult operationResult = operationResults.GetByReferenceId(organization.ReferenceId);

            operationResult.AddOperationResult("112255", String.Format("You do not have sufficient permission to perform {0} operation on Organization : {1}", organization.Action, organization.LongName), OperationResultType.Error);
            MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, String.Format("You do not have sufficient permission to perform {0} operation on Organization : {1}", organization.Action, organization.LongName), MDMTraceSource.APIFramework);

        }

        /// <summary>
        /// 
        /// </summary>
        private void AddOperationResults(DataModelOperationResultCollection operationResults, String messageCode, String message, Object[] parameters, OperationResultType operationResultType, TraceEventType traceEventType, CallerContext callerContext)
        {
            if (parameters != null && parameters.Count() > 0)
            {
                _localeMessage = _localeMessageBL.TryGet(_systemUILocale, messageCode, message, parameters, false, callerContext);
            }
            else
            {
                _localeMessage = _localeMessageBL.TryGet(_systemUILocale, messageCode, message, false, callerContext);
            }

            if (_localeMessage != null)
            {
                operationResults.AddOperationResult(_localeMessage.Code, _localeMessage.Message, operationResultType);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void AddOperationResult(IDataModelOperationResult operationResult, String messageCode, String message, Object[] parameters, OperationResultType operationResultType, TraceEventType traceEventType, CallerContext callerContext)
        {
            if (parameters != null && parameters.Count() > 0)
            {
                _localeMessage = _localeMessageBL.TryGet(_systemUILocale, messageCode, message, parameters, false, callerContext);
            }
            else
            {
                _localeMessage = _localeMessageBL.TryGet(_systemUILocale, messageCode, message, false, callerContext);
            }

            if (_localeMessage != null)
            {
                operationResult.AddOperationResult(_localeMessage.Code, _localeMessage.Message, operationResultType);
            }
        }

        #endregion

        #endregion

        #endregion
    }
}