using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Transactions;
using MDM.ActivityLogManager.Business;

namespace MDM.HierarchyManager.Business
{
    using MDM.AdminManager.Business;
    using MDM.BufferManager;
    using MDM.BusinessObjects;
    using MDM.BusinessObjects.DataModel;
    using MDM.BusinessObjects.Diagnostics;
    using MDM.ConfigurationManager.Business;
    using MDM.Core;
    using MDM.Core.Exceptions;
    using MDM.HierarchyManager.Data;
    using MDM.Interfaces;
    using MDM.MessageManager.Business;
    using MDM.Utility;
    using Riversand.StoredProcedures;

    /// <summary>
    /// Specifies hierarchy manager
    /// </summary>
    public class HierarchyBL : BusinessLogicBase , IHierarchyManager, IDataModelManager
    {
        #region Fields

        /// <summary>
        /// Field denoting the security principal
        /// </summary>
        private SecurityPrincipal _securityPrincipal = null;

        /// <summary>
        /// Data access layer for entity types
        /// </summary>
        private HierarchyDA _hierarchyDA = new HierarchyDA();

        /// <summary>
        /// Field denoting locale Message
        /// </summary>
        private LocaleMessage _localeMessage = null;

        /// <summary>
        /// Field denoting localeMessageBL
        /// </summary>
        private LocaleMessageBL _localeMessageBL = new LocaleMessageBL();

        /// <summary>
        /// Cache manager for hierarchy objects
        /// </summary>
        private HierarchyBufferManager _hierarchyBufferManager = new HierarchyBufferManager();

        /// <summary>
        /// Field denoting utility
        /// </summary>
        private Utility _utility = new Utility();

        /// <summary>
        /// Filed denoting DataLocale
        /// </summary>
        private LocaleEnum _systemUILocale = LocaleEnum.UnKnown;

        /// <summary>
        /// Field for DataSecurityBL object to fetch security permissions
        /// </summary>
        private DataSecurityBL _dataSecurityManager = new DataSecurityBL();

        /// <summary>
        /// Indicates current trace settings
        /// </summary>
        private TraceSettings _currentTraceSettings = new TraceSettings();

        #endregion

        #region Constructors

        public HierarchyBL()
        {
            GetSecurityPrincipal();
            _systemUILocale = GlobalizationHelper.GetSystemUILocale();
            _currentTraceSettings = MDMOperationContextHelper.GetCurrentTraceSettings();
        }

        #endregion Constructors

        #region Properties
        #endregion

        #region Methods

        #region Legacy

        ///<summary>
        ///Get All Hierarchies
        ///</summary>
        public Collection<Hierarchy> GetAllHierarchies(int localeId)
        {
            String userName = SecurityPrincipalHelper.GetCurrentSecurityPrincipal().CurrentUserName;
            string strXMLData = Catalog.GetCatalogPermissionsByOrg(userName,
                                                                userName,
                                                                0,
                                                                localeId,
                                                                0,
                                                                999,
                                                                "ShortName",
                                                                "ShortName",
                                                                "",
                                                                0,
                                                                true,
                                                                false,
                                                                false,
                                                                false,
                                                                true,
                                                                true);

            Collection<Hierarchy> hierarchies = new Collection<Hierarchy>();
            if (!string.IsNullOrEmpty(strXMLData))
            {
                System.Xml.XmlDocument _xmlDocument = new System.Xml.XmlDocument();
                _xmlDocument.LoadXml(strXMLData);
                System.Xml.XmlNodeList nodelist = _xmlDocument.SelectNodes("Catalogs/Catalog");
                foreach (System.Xml.XmlNode node in nodelist)
                {
                    Hierarchy hierarchy = new Hierarchy();
                    hierarchy.Id = XmlHelper.GetXmlAttributeIntegerValue(node, "PK_Catalog");
                    hierarchy.Name = XmlHelper.GetXmlAttributeStringValue(node, "ShortName");
                    hierarchy.LongName = XmlHelper.GetXmlAttributeStringValue(node, "LongName");
                    hierarchies.Add(hierarchy);
                }
            }
            return hierarchies;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="localeId"></param>
        /// <param name="taxonomyId"></param>
        /// <param name="searchParameter"></param>
        /// <param name="countTo"></param>
        /// <returns></returns>
        public Collection<MDM.BusinessObjects.Entity> GetAllCategoriesByHierarchy(int localeId, int taxonomyId, string searchParameter, int countTo)
        {
            String userName = SecurityPrincipalHelper.GetCurrentSecurityPrincipal().CurrentUserName;
            string strXMLData = Catalog.GetNodePermissions(userName,
                userName,
                taxonomyId,
                0, 1, -1, 0, countTo, "LongName", "LongName", searchParameter, 0, false, false, false, false, "");
            Collection<MDM.BusinessObjects.Entity> categories = new Collection<MDM.BusinessObjects.Entity>();
            if (!string.IsNullOrEmpty(strXMLData))
            {
                System.Xml.XmlDocument _xmlDocument = new System.Xml.XmlDocument();
                _xmlDocument.LoadXml(strXMLData);
                System.Xml.XmlNodeList nodelist = _xmlDocument.SelectNodes("CNodes/CNode");
                foreach (System.Xml.XmlNode node in nodelist)
                {
                    MDM.BusinessObjects.Entity category = new MDM.BusinessObjects.Entity();
                    category.Id = XmlHelper.GetXmlAttributeIntegerValue(node, "PK_CNode");
                    category.Name = XmlHelper.GetXmlAttributeStringValue(node, "ShortName");
                    category.LongName = XmlHelper.GetXmlAttributeStringValue(node, "LongName");
                    categories.Add(category);
                }
            }
            return categories;
        }

        #endregion Legacy

        #region Public Methods

        #region CUD methods

        /// <summary>
        /// Creates the hierarchy
        /// </summary>
        /// <param name="hierarchy">hierarchy to be created</param>
        /// <param name="callerContext">Caller context of application</param>
        /// <returns>Result of the operation</returns>
        public OperationResult Create(Hierarchy hierarchy, CallerContext callerContext)
        {
            ValidateInputParameters(hierarchy, callerContext);

            hierarchy.Action = ObjectAction.Create;

            return this.Process(hierarchy, callerContext);
        }

        /// <summary>
        /// Updates given hierarchy
        /// </summary>
        /// <param name="hierarchy">hierarchy to be updated</param>
        /// <param name="callerContext">Caller context of application</param>
        /// <returns>Result of the operation</returns>
        public OperationResult Update(Hierarchy hierarchy, CallerContext callerContext)
        {
            ValidateInputParameters(hierarchy, callerContext);
            hierarchy.Action = ObjectAction.Update;

            return this.Process(hierarchy, callerContext);
        }

        /// <summary>
        /// Deletes given hierarchy
        /// </summary>
        /// <param name="hierarchy">hierarchy to be deleted</param>
        /// <param name="callerContext">Caller context of application</param>
        /// <returns>Result of the operation</returns>
        public OperationResult Delete(Hierarchy hierarchy, CallerContext callerContext)
        {
            ValidateInputParameters(hierarchy, callerContext);
            hierarchy.Action = ObjectAction.Delete;

            return this.Process(hierarchy, callerContext);
        }

        #endregion CUD methods

        #region Get Methods

        /// <summary>
        /// Get all hierarchies in the system
        /// </summary>
        /// <param name="callerContext">Indicates name of application and module.</param>
        /// <param name="applySecurity"></param>
        /// <returns>All hierarchies</returns>
        public HierarchyCollection GetAll(CallerContext callerContext, Boolean applySecurity = true)
        {
            MDMTraceHelper.StartTraceActivity("HierarchyBL.GetEntityTypes", MDMTraceSource.DataModel, false);
            HierarchyCollection hierarchyCollection = new HierarchyCollection();
            HierarchyCollection permittedHierarchyCollection = new HierarchyCollection();

            try
            {
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "CallerContext : Application - " + callerContext.Application + " Module - " + callerContext.Module);

                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Finding hierarchies in cache...", MDMTraceSource.DataModel);

                hierarchyCollection = _hierarchyBufferManager.FindAllTaHierarchies();

                if (hierarchyCollection == null || hierarchyCollection.Count < 1)
                {
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "No hierarchy cache found.Now all hierarchies would be loaded from database.", MDMTraceSource.DataModel);

                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Loading hierarchies from database...", MDMTraceSource.DataModel);

                    //Get command
                    DBCommandProperties command = DBCommandHelper.Get(callerContext, MDMCenterModuleAction.Read);

                    hierarchyCollection = _hierarchyDA.Get(command);

                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Loaded hierarchies from database.", MDMTraceSource.DataModel);

                    if (hierarchyCollection != null && hierarchyCollection.Count > 0)
                    {
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("Caching {0} hierarchies ...", hierarchyCollection.Count, MDMTraceSource.DataModel));

                        _hierarchyBufferManager.SetHierarchies(hierarchyCollection, 3);

                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Done with caching for hierarchies.", MDMTraceSource.DataModel);
                    }
                }
                else
                {
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("Found {0} hierarchies in cache.", hierarchyCollection.Count), MDMTraceSource.DataModel);
                }

                #region Validate User Permission

                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Validating user permission started...", MDMTraceSource.DataModel);

                if (applySecurity && hierarchyCollection.Count > 0)
                {
                    Boolean hasPermission = false;

                    foreach (Hierarchy hierarchy in hierarchyCollection)
                    {
                        if (ValidateUserPermission(hierarchy, UserAction.View))
                        {
                            permittedHierarchyCollection.Add(hierarchy);
                            hasPermission = true;
                        }
                    }

                    if (!hasPermission)
                    {
                        throw new MDMOperationException("112252", "You do not have sufficient permission to get hierarchy", "HierarchyManager.HierarchyBL", String.Empty, "Get");
                    }
                    else
                    {
                        Int32 objectTypeId = (Int32)ObjectType.Taxonomy;
                        PermissionContext permissionContext = new PermissionContext(0, 0, 0, 0, 0, 0, 0, 0, _securityPrincipal.CurrentUserId, 0);
                        Permission permission = _dataSecurityManager.GetMDMObjectPermission(0, objectTypeId, ObjectType.Taxonomy.ToString(), permissionContext);
                        if (permission != null)
                            permittedHierarchyCollection.PermissionSet = permission.PermissionSet;
                    }
                }

                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Validating user permission completed...", MDMTraceSource.DataModel);

                #endregion
            }
            finally
            {
                MDMTraceHelper.StopTraceActivity("HierarchyBL.GetEntityTypes", MDMTraceSource.DataModel);
            }

            if (applySecurity) return permittedHierarchyCollection;
            else return hierarchyCollection;
        }

        /// <summary>
        /// Get hierarchy by id
        /// </summary>
        /// <param name="id">Id using which hierarchy is to be fetched</param>
        /// <param name="callerContext">Context of the caller</param>
        /// <param name="applySecurity"></param>
        /// <returns>hierarchy with Id specified. Otherwise null</returns>
        public Hierarchy GetById(Int32 id, CallerContext callerContext, Boolean applySecurity = true)
        {
            MDMTraceHelper.StartTraceActivity("HierarchyBL.GetById", MDMTraceSource.DataModel, false);
            IHierarchy iHierarchy = null;

            try
            {
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("Requested entity type id :{0} ", id), MDMTraceSource.DataModel);

                if (id < 1)
                {
                    throw new MDMOperationException("112198", "HierarchyId cannot be less than 1...", "HierarchyBL.GetById", String.Empty, "Get");
                }

                var hierarchyCollection = GetAll(callerContext, applySecurity);
                if (hierarchyCollection != null && hierarchyCollection.Any())
                {
                    iHierarchy = hierarchyCollection.Get(id);
                }
            }
            finally
            {
                MDMTraceHelper.StopTraceActivity("HierarchyBL.GetById", MDMTraceSource.DataModel);
            }

            return iHierarchy as Hierarchy;
        }

        /// <summary>
        /// Gets hierarchy based on unique short name 
        /// </summary>
        /// <param name="hierarchyShortName">unique short name for hierarchy</param>
        /// <param name="callerContext">caller context to denote application and module which has called an API</param>
        /// <returns>Returns hierarchy based on name</returns>
        public Hierarchy GetByName(String hierarchyShortName, CallerContext callerContext)
        {
            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StartTraceActivity("HierarchyBL.GetByName", MDMTraceSource.DataModel, false);

            #region Parameter validation
            ValidateCallerContext(callerContext, "GetByName");
            if (String.IsNullOrEmpty(hierarchyShortName))
            {
                String errorMessage = this.GetSystemLocaleMessage("112688", callerContext).Message;
                throw new MDMOperationException("112688", errorMessage, "HierarchyManager.HierarchyBL", String.Empty, "GetByName");
            }
            #endregion

            Hierarchy hierarchy = null;

            try
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("Requested hierarchy name :{0}", hierarchyShortName), MDMTraceSource.DataModel);

                LocaleEnum locale = GlobalizationHelper.GetSystemDataLocale();

                Collection<Hierarchy> hierarchies = this.GetAllHierarchies((Int32)locale);

                if (hierarchies != null && hierarchies.Count > 0)
                {
                    foreach (Hierarchy tempHierarchy in hierarchies)
                    {
                        if (tempHierarchy.Name.Equals(hierarchyShortName))
                        {
                            hierarchy = tempHierarchy;
                        }
                    }
                }
            }
            finally
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StopTraceActivity("HierarchyBL.GetByName", MDMTraceSource.DataModel);
            }

            return hierarchy;
        }

        /// <summary>
        /// Get hierarchies by given ids
        /// </summary>
        /// <param name="hierachyIdList">Indicates collection of hierarchy ids which needs to be fetched</param>
        /// <param name="callerContext">Indicates context of the caller</param>
        /// <param name="applySecurity">Indicates needs to apply security or not</param>
        /// <returns>Collection of hierarchies</returns>
        public HierarchyCollection GetByIds(Collection<Int32> hierachyIdList, CallerContext callerContext, Boolean applySecurity = true)
        {
            var diagnosticActivity = new DiagnosticActivity();
            HierarchyCollection hierarchies = new HierarchyCollection(); ;

            try
            {
                if (_currentTraceSettings.IsBasicTracingEnabled)
                {
                    diagnosticActivity.Start();
                }

                if (hierachyIdList == null || hierachyIdList.Count < 1)
                {
                    _localeMessage = _localeMessageBL.Get(GlobalizationHelper.GetSystemUILocale(), "113960", new Object[] { "HierarchyIdList" }, false, callerContext);
                    diagnosticActivity.LogError("113960", _localeMessage.Message);
                    throw new MDMOperationException("113960", _localeMessage.Message, "HierarchyBL.GetByIds", String.Empty, "Get");
                }

                var hierarchyCollection = GetAll(callerContext, applySecurity);

                if (hierarchyCollection != null && hierarchyCollection.Any())
                {
                    foreach (Hierarchy hierarchy in hierarchyCollection)
                    {
                        if (hierachyIdList.Contains(hierarchy.Id))
                        {
                            if (_currentTraceSettings.IsBasicTracingEnabled)
                            {
                                diagnosticActivity.LogInformation(String.Format("Requested hierarchy id :{0} ", hierarchy.Id));
                            }

                            hierarchies.Add(hierarchy);
                        }
                    }
                }
            }
            finally
            {
                if (_currentTraceSettings.IsBasicTracingEnabled)
                {
                    diagnosticActivity.Stop();
                }
            }

            return hierarchies;
        }

        #endregion Get Methods

        #region Process methods

        /// <summary>
        /// Create - Update or Delete given hierarchies
        /// </summary>
        /// <param name="hierarchyCollection">Collection of hierarchies to process</param>
        /// <param name="callerContext">Context which called the application</param>
        /// <returns>Result of the operation</returns>
        public OperationResultCollection Process(HierarchyCollection hierarchyCollection, CallerContext callerContext)
        {
            OperationResultCollection hierarchyProcessOperationResult = new OperationResultCollection();

            ValidateInputParameters(hierarchyCollection, callerContext);

            foreach (Hierarchy hierarchy in hierarchyCollection)
            {
                if (!ValidateUserPermission(hierarchy, _utility.ObjectActionToUserActionMap(hierarchy.Action)))
                {
                    OperationResult operationResult = new OperationResult();
                    operationResult = PopulatePermissionErrorInOperationResult(hierarchy);
                    hierarchyProcessOperationResult.Add((IOperationResult)operationResult);
                }
                else
                {
                    hierarchyProcessOperationResult.Add(this.Process(hierarchy, callerContext));
                }
            }

            return hierarchyProcessOperationResult;
        }

        #endregion Process methods

        #region IDataModelManager Methods

        /// <summary>
        /// Generate and OperationResult Schema for given DataModelObjectCollection
        /// </summary>
        /// <param name="iDataModelObjects"></param>
        public void PrepareOperationResultsSchema(IDataModelObjectCollection iDataModelObjects, DataModelOperationResultCollection operationResultCollection)
        {
            HierarchyCollection hierarchies = iDataModelObjects as HierarchyCollection;

            if (hierarchies != null && hierarchies.Count > 0)
            {
                if (operationResultCollection.Count > 0)
                {
                    operationResultCollection.Clear();
                }

                Int32 hierarchyToBeCreated = -1;

                foreach (Hierarchy hierarchy in hierarchies)
                {
                    DataModelOperationResult hierachyOperationResult = new DataModelOperationResult(hierarchy.Id, hierarchy.LongName, hierarchy.ExternalId, hierarchy.ReferenceId);

                    if (String.IsNullOrEmpty(hierachyOperationResult.ExternalId))
                    {
                        hierachyOperationResult.ExternalId = hierarchy.Name;
                    }

                    if (hierarchy.Id < 1)
                    {
                        hierarchy.Id = hierarchyToBeCreated;
                        hierachyOperationResult.Id = hierarchyToBeCreated;
                        hierarchyToBeCreated--;
                    }

                    operationResultCollection.Add(hierachyOperationResult);
                }
            }
        }

        /// <summary>
        /// Validates DataModelObjects and populate OperationResults
        /// </summary>
        /// <param name="iDataModelObjects">Collection of data model objects to be filled up.</param>
        /// <param name="operationResult">Collection of DataModel OperationResult</param>
        /// <param name="iCallerContext">Context of caller making call to this API.</param>
        public void Validate(IDataModelObjectCollection iDataModelObjects, DataModelOperationResultCollection operationResults, ICallerContext iCallerContext)
        {
            HierarchyCollection hierarchies = iDataModelObjects as HierarchyCollection;

            ValidateInputParameters(hierarchies, operationResults, iCallerContext as CallerContext);

            foreach (Hierarchy hierarchy in hierarchies)
            {
                if (!ValidateUserPermission(hierarchy, _utility.ObjectActionToUserActionMap(hierarchy.Action)))
                {
                    PopulatePermissionErrorInOperationResult(hierarchy, operationResults);
                }
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
            HierarchyCollection hierarchies = iDataModelObjects as HierarchyCollection;

            if (hierarchies != null)
            {
                LoadOriginalHierarchies(hierarchies, iCallerContext as CallerContext);
            }
        }

        /// <summary>
        /// Fills missing information to data model objects.
        /// </summary>
        /// <param name="iDataModelObjects">Collection of data model objects to be filled up.</param>
        /// <param name="iCallerContext">Context of caller making call to this API.</param>
        public void FillDataModel(IDataModelObjectCollection iDataModelObjects, ICallerContext iCallerContext)
        {
            FillHierarchies(iDataModelObjects as HierarchyCollection, iCallerContext as CallerContext);
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
            CompareAndMerge(iDataModelObjects as HierarchyCollection, operationResults, iCallerContext as CallerContext);
        }

        /// <summary>
        /// Process data model collection
        /// </summary>
        /// <param name="iDataModelObjects">Collection of data model objects to be processed.</param>
        /// <param name="iCallerContext">Context of caller making call to this API.</param>
        /// <returns>Result of operation</returns>
        public void Process(IDataModelObjectCollection iDataModelObjects, DataModelOperationResultCollection operationResults, ICallerContext iCallerContext)
        {
            HierarchyCollection hierarchies = iDataModelObjects as HierarchyCollection;
            CallerContext callerContext = (CallerContext)iCallerContext;

            if (hierarchies.Count > 0)
            {
                String userName = SecurityPrincipalHelper.GetCurrentSecurityPrincipal().CurrentUserName;
                DBCommandProperties command = DBCommandHelper.Get(callerContext, MDMCenterModuleAction.Execute);
                LocaleEnum systemDataLocale = GlobalizationHelper.GetSystemDataLocale();

                #region Perform hierarchy updates in database

                using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions() { IsolationLevel = Constants.DEFAULT_ISOLATION_LEVEL }))
                {
                    _hierarchyDA.Process(hierarchies, operationResults, callerContext.ProgramName, userName, systemDataLocale, command);
                    transactionScope.Complete();
                }

                LocalizeErrors(operationResults, callerContext);

                #endregion
            }

            #region activity log

            operationResults.RefreshOperationResultStatus();
            if (operationResults.OperationResultStatus == OperationResultStatusEnum.Successful && AppConfigurationHelper.GetAppConfig<Boolean>(Constants.ACTIVITYLOG_DATAMODEL_ENABLED_APPCONFIG_KEY, false))
            {
                LogDataModelChanges(hierarchies, callerContext);
            }

            #endregion activity log

        }

        /// <summary>
        /// Processes the Entity cache statuses for data model objects
        /// </summary>
        /// <param name="iDataModelObjects">Collection of data model objects to be process entity cache load.</param>
        /// <param name="iCallerContext">Context of caller making call to this API.</param>
        public void InvalidateCache(IDataModelObjectCollection iDataModelObjects, DataModelOperationResultCollection operationResult, ICallerContext iCallerContext)
        {
            _hierarchyBufferManager.RemoveHierarchies(false);
        }

        #endregion

        #endregion

        #region Private Methods

        #region Validation Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hierarchy"></param>
        /// <param name="callerContext"></param>
        private void ValidateInputParameters(Hierarchy hierarchy, CallerContext callerContext)
        {
            if (callerContext == null)
            {
                throw new MDMOperationException("111846", "CallerContext cannot be null.", "EntityTypeBL.Process", String.Empty, "Process");
            }

            if (hierarchy == null)
            {
                throw new MDMOperationException("112182", "Hierarchy cannot be null.", "HierarchyBL.Process", String.Empty, "Process");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hierarchies"></param>
        /// <param name="callerContext"></param>
        private void ValidateInputParameters(HierarchyCollection hierarchies, CallerContext callerContext)
        {
            if (hierarchies == null || hierarchies.Count < 1)
            {
                throw new MDMOperationException("112181", "Hierarchy collection cannot be null..", "HierarchyBL.Process", String.Empty, "Process");
            }

            if (callerContext == null)
            {
                throw new MDMOperationException("111846", "CallerContext cannot be null.", "EntityTypeBL.Process", String.Empty, "Process");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hierarchies"></param>
        /// <param name="operationResults"></param>
        /// <param name="callerContext"></param>
        private void ValidateInputParameters(HierarchyCollection hierarchies, DataModelOperationResultCollection operationResults, CallerContext callerContext)
        {
            Collection<String> shortNames = new Collection<String>();

            if (callerContext == null)
            {
                AddOperationResults(operationResults, "111846", String.Empty, null, OperationResultType.Error, TraceEventType.Error, callerContext);
            }

            if (hierarchies == null || hierarchies.Count < 1)
            {
                AddOperationResults(operationResults, "113596", String.Empty, null, OperationResultType.Error, TraceEventType.Error, callerContext);
            }
            else
            {
                foreach (Hierarchy hierarchy in hierarchies)
                {
                    IDataModelOperationResult operationResult = operationResults.GetByReferenceId(hierarchy.ReferenceId);

                    if (String.IsNullOrWhiteSpace(hierarchy.Name))
                    {
                        AddOperationResult(operationResult, "112688", String.Empty, null, OperationResultType.Error, TraceEventType.Error, callerContext);
                    }
                    else
                    {
                        if (shortNames.Contains(hierarchy.Name))
                        {
                            AddOperationResult(operationResult, "113669", "Hierarchy name already exists.", null, OperationResultType.Error, TraceEventType.Error, callerContext);
                        }
                        else
                        {
                            shortNames.Add(hierarchy.Name);
                        }
                    }
                    if (String.IsNullOrWhiteSpace(hierarchy.LongName))
                    {
                        AddOperationResult(operationResult, "113671", "Hierarchy long name is empty or not specified.", null, OperationResultType.Error, TraceEventType.Error, callerContext);
                    }
                }
            }

         }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hierarchy"></param>
        /// <param name="userAction"></param>
        /// <returns></returns>
        private Boolean ValidateUserPermission(Hierarchy hierarchy, UserAction userAction)
        {
            Permission permission = null;
            Int32 objectTypeId = (Int32)ObjectType.Taxonomy;

            PermissionContext permissionContext = new PermissionContext(0, hierarchy.Id, 0, 0, 0, 0, 0, 0, _securityPrincipal.CurrentUserId, 0);
            permission = _dataSecurityManager.GetMDMObjectPermission(hierarchy.Id, objectTypeId, ObjectType.Taxonomy.ToString(), permissionContext);

            if (permission == null)
            {
                return false;
            }
            else
            {
                hierarchy.PermissionSet = permission.PermissionSet;
                return permission.PermissionSet.Contains(userAction);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="callerContext"></param>
        /// <param name="methodName"></param>
        private void ValidateCallerContext(CallerContext callerContext, String methodName)
        {
            String errorMessage = String.Empty;

            if (callerContext == null)
            {
                errorMessage = "CallerContext cannot be null.";
                throw new MDMOperationException("111846", errorMessage, "DataModelManager.EntityTypeBL", String.Empty, methodName, MDMTraceSource.DataModel);
            }
        }

        #endregion

        #region Get Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hierarchies"></param>
        /// <param name="callerContext"></param>
        private void LoadOriginalHierarchies(HierarchyCollection hierarchies, CallerContext callerContext)
        {
            HierarchyCollection orginalHierarchies = GetAll(callerContext);

            if (orginalHierarchies != null && orginalHierarchies.Count > 0)
            {
                foreach (Hierarchy hierarchy in hierarchies)
                {
                    hierarchy.OriginalHierachy = orginalHierarchies.Get(hierarchy.Name);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hierarchies"></param>
        /// <param name="callerContext"></param>
        private void FillHierarchies(HierarchyCollection hierarchies, CallerContext callerContext)
        {
            foreach (Hierarchy hierarchy in hierarchies)
            {
                if (hierarchy.Id < 1)
                {
                    hierarchy.Id = (hierarchy.OriginalHierachy != null) ? hierarchy.OriginalHierachy.Id : hierarchy.Id;
                }

                if (hierarchy.SecurityObjectTypeId < 1)
                {
                    hierarchy.SecurityObjectTypeId = (hierarchy.OriginalHierachy != null) ? hierarchy.OriginalHierachy.SecurityObjectTypeId : hierarchy.SecurityObjectTypeId;
                }
            }
        }

        #endregion

        #region Process Methods

        /// <summary>
        /// Create - Update or Delete given hierarchy
        /// </summary>
        /// <param name="hierarchy">hierarchy to process</param>
        /// <param name="callerContext">Context which called the application</param>
        /// <returns>Result of the operation</returns>
        private OperationResult Process(Hierarchy hierarchy, CallerContext callerContext)
        {
            OperationResult hierarchyProcessOperationResult = new OperationResult();

            //Get Data locale 
            LocaleEnum systemDataLocale = MDM.Utility.GlobalizationHelper.GetSystemDataLocale();
            String userName = PopulateUserName();
            PopulateProgramName(callerContext);

            //Get command
            DBCommandProperties command = DBCommandHelper.Get(callerContext, MDMCenterModuleAction.Update);

            try
            {
                #region Validate Permission

                if (!ValidateUserPermission(hierarchy, _utility.ObjectActionToUserActionMap(hierarchy.Action)))
                {
                    return PopulatePermissionErrorInOperationResult(hierarchy);
                }

                #endregion

                using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = Constants.DEFAULT_ISOLATION_LEVEL }))
                {
                    if (hierarchy.Action == ObjectAction.Create &&
                        GetAll(callerContext, false).Any(h => h.Name.Equals(hierarchy.Name, StringComparison.OrdinalIgnoreCase)))
                    {
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, "Hierarchy " + hierarchy.Name + " already exists", MDMTraceSource.DataModel);

                        Object[] parameters = new Object[] { hierarchy.Name };
                        _localeMessage = _localeMessageBL.Get(GlobalizationHelper.GetSystemUILocale(), "112586", parameters, false, callerContext);
                        hierarchyProcessOperationResult.AddOperationResult(_localeMessage.Code, _localeMessage.Message, OperationResultType.Error);
                        return hierarchyProcessOperationResult;
                    }

                    hierarchyProcessOperationResult = _hierarchyDA.Process(hierarchy, callerContext.ProgramName, userName, systemDataLocale, command);

                    LocalizeErrors(callerContext, hierarchyProcessOperationResult);

                    _hierarchyBufferManager.RemoveHierarchies(false);

                    transactionScope.Complete();
                }
            }
            catch (Exception ex)
            {
                throw new MDMOperationException(ex.Message, ex);
            }

            PopulateOperationResult(hierarchy, hierarchyProcessOperationResult);

            #region activity log

            hierarchyProcessOperationResult.RefreshOperationResultStatus();
            HierarchyCollection hierarchyCollection = new HierarchyCollection();
            hierarchyCollection.Add(hierarchy);
            if (hierarchyProcessOperationResult.OperationResultStatus == OperationResultStatusEnum.Successful && AppConfigurationHelper.GetAppConfig<Boolean>(Constants.ACTIVITYLOG_DATAMODEL_ENABLED_APPCONFIG_KEY, false))
            {
                LogDataModelChanges(hierarchyCollection, callerContext);
            }

            #endregion activity log

            return hierarchyProcessOperationResult;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hierarchies"></param>
        /// <param name="operationResults"></param>
        /// <param name="callerContext"></param>
        private void CompareAndMerge(HierarchyCollection hierarchies, DataModelOperationResultCollection operationResults, CallerContext callerContext)
        {
            foreach (Hierarchy deltaHierarchy in hierarchies)
            {
                IDataModelOperationResult operationResult = operationResults.GetByReferenceId(deltaHierarchy.ReferenceId);

                //If delta object Action is Read/Ignore then skip Merge Delta Process.
                if (deltaHierarchy.Action == ObjectAction.Read || deltaHierarchy.Action == ObjectAction.Ignore)
                    continue;

                IHierarchy origHierarchy = deltaHierarchy.OriginalHierachy;

                if (origHierarchy != null)
                {
                    //If delta object Action is Delete then skip Merge Delta process.
                    if (deltaHierarchy.Action != ObjectAction.Delete)
                    {
                        origHierarchy.MergeDelta(deltaHierarchy, callerContext, false);
                    }
                }
                else
                {
                    String errorMessage = String.Empty;
                    if (deltaHierarchy.Action == ObjectAction.Delete)
                    {
                        AddOperationResult(operationResult, "113595", String.Empty, new Object[] { deltaHierarchy.Name}, OperationResultType.Error, TraceEventType.Error, callerContext);
                    }
                    else
                    {
                        //If original object is not found then set Action as Create always.
                        deltaHierarchy.Action = ObjectAction.Create;
                    }
                }
                operationResult.PerformedAction = deltaHierarchy.Action;
            }
        }

        #endregion

        #region Utility Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="callerContext"></param>
        private void PopulateProgramName(CallerContext callerContext)
        {
            if (String.IsNullOrWhiteSpace(callerContext.ProgramName))
            {
                callerContext.ProgramName = "HierarchyBL.Process";
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private String PopulateUserName()
        {
            if (_securityPrincipal != null)
            {
                return _securityPrincipal.CurrentUserName;
            }
            return String.Empty;
        }

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
                    _localeMessage = _localeMessageBL.Get(GlobalizationHelper.GetSystemUILocale(), error.ErrorCode, false,
                                                          callerContext);

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
        /// <param name="operationResults">DataModelOperationResultCollection</param>
        /// <param name="callerContext">CallerContext</param>
        private void LocalizeErrors(DataModelOperationResultCollection operationResults, CallerContext callerContext)
        {
            foreach (DataModelOperationResult operationResult in operationResults)
            {
                foreach (Error error in operationResult.Errors)
                {
                    if (!String.IsNullOrEmpty(error.ErrorCode) && String.IsNullOrEmpty(error.ErrorMessage))
                    {
                        _localeMessage = _localeMessageBL.Get(GlobalizationHelper.GetSystemUILocale(), error.ErrorCode, false, callerContext);

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

            return localeMessageBL.Get(GlobalizationHelper.GetSystemUILocale(), messageCode, false, callerContext);
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

        /// <summary>
        /// Log changes to tb_datamodelactivitylog if DA call is sucessfull and appconfig key datamodelactivitylog.enable == true
        /// </summary>
        /// <param name="hierarchyCollection"></param>
        /// <param name="callerContext"></param>
        private void LogDataModelChanges(HierarchyCollection hierarchyCollection, CallerContext callerContext)
        {
            #region step: populate datamodelactivitylog object

            DataModelActivityLogBL dataModelActivityLogManager = new DataModelActivityLogBL();
            DataModelActivityLogCollection activityLogCollection = dataModelActivityLogManager.FillDataModelActivityLogCollection(hierarchyCollection);

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
        /// <param name="hierarchy"></param>
        /// <param name="hierarchyProcessOperationResult"></param>
        private void PopulateOperationResult(Hierarchy hierarchy, OperationResult hierarchyProcessOperationResult)
        {
            if (hierarchy.Action == ObjectAction.Create)
            {
                if (hierarchyProcessOperationResult.ReturnValues.Any())
                {
                    hierarchyProcessOperationResult.Id =
                        Convert.ToInt32(hierarchyProcessOperationResult.ReturnValues[0]);
                }
            }
            else
            {
                hierarchyProcessOperationResult.Id = hierarchy.Id;
            }

            hierarchyProcessOperationResult.ReferenceId = String.IsNullOrEmpty(hierarchy.ReferenceId)
                ? hierarchy.Name
                : hierarchy.ReferenceId;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hierarchy"></param>
        /// <returns></returns>
        private OperationResult PopulatePermissionErrorInOperationResult(Hierarchy hierarchy)
        {
            OperationResult operationResult = new OperationResult();

            PopulateOperationResult(hierarchy, operationResult);

            operationResult.AddOperationResult("112253", String.Format("You do not have sufficient permission to perform {0} operation on Hierarchy: {1}", hierarchy.Action, hierarchy.LongName), OperationResultType.Error);
            MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, String.Format("You do not have sufficient permission to perform {0} operation on Hierarchy: {1}", hierarchy.Action, hierarchy.LongName), MDMTraceSource.APIFramework);
            return operationResult;
        }

        /// <summary>
        /// Find and populate Permission Error in DMORC
        /// </summary>
        /// <param name="hierarchy">Hierarchy</param>
        /// <param name="operationResults">DataModelOperationResultCollection</param>
        /// <returns></returns>
        private void PopulatePermissionErrorInOperationResult(Hierarchy hierarchy, DataModelOperationResultCollection operationResults)
        {
            IOperationResult operationResult = operationResults.GetByReferenceId(hierarchy.ReferenceId);

            operationResult.AddOperationResult("112253", String.Format("You do not have sufficient permission to perform {0} operation on Hierarchy: {1}", hierarchy.Action, hierarchy.LongName), OperationResultType.Error);
            MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, String.Format("You do not have sufficient permission to perform {0} operation on Hierarchy: {1}", hierarchy.Action, hierarchy.LongName), MDMTraceSource.APIFramework);
        }

        #endregion

        #endregion

        #endregion
    }
}