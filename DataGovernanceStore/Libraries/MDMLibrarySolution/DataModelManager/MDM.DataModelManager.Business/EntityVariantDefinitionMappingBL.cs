using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDM.DataModelManager.Business
{

    using MDM.ApplicationServiceManager.Business;
    using MDM.BufferManager;
    using MDM.BusinessObjects;
    using MDM.BusinessObjects.DataModel;
    using MDM.BusinessObjects.Diagnostics;
    using MDM.Core;
    using MDM.Core.Exceptions;
    using MDM.Interfaces;
    using MDM.MessageManager.Business;
    using MDM.Utility;
    using MDM.Core.Extensions;

    /// <summary>
    /// 
    /// </summary>
    public class EntityVariantDefinitionMappingBL : BusinessLogicBase, IDataModelManager
    {
        #region Fields

        /// <summary>
        /// 
        /// </summary>
        private IContainerManager _iContainerManager = null;

        /// <summary>
        /// 
        /// </summary>
        private ICategoryManager _iCategoryManager = null;

        /// <summary>
        /// Indicates system UI locale
        /// </summary>
        private LocaleEnum _systemUILocale = LocaleEnum.UnKnown;

        /// <summary>
        /// Indicates current trace settings
        /// </summary>
        private TraceSettings _currentTraceSettings = new TraceSettings();

        /// <summary>
        /// Indicates whether diagnostic tracing is enable or not
        /// </summary>
        private Boolean _isTracingEnabled = false;

        /// <summary>
        /// Indicates locale message 
        /// </summary>
        private LocaleMessageBL _localeMessageManager = new LocaleMessageBL();

        /// <summary>
        /// Field denoting Mapping Buffer Manager
        /// </summary>
        private MappingBufferManager _mappingBufferManager = new MappingBufferManager();

        #endregion Fields

        #region Constructors

        /// <summary>
        /// 
        /// </summary>
        public EntityVariantDefinitionMappingBL()
        {
            this._systemUILocale = GlobalizationHelper.GetSystemUILocale();
            this._currentTraceSettings = MDMOperationContextHelper.GetCurrentTraceSettings();
            this._isTracingEnabled = _currentTraceSettings.IsBasicTracingEnabled;
        }

        /// <summary>
        /// 
        /// </summary>
        public EntityVariantDefinitionMappingBL(IContainerManager iContainerManager, ICategoryManager iCategoryManager)
        {
            this._systemUILocale = GlobalizationHelper.GetSystemUILocale();
            this._currentTraceSettings = MDMOperationContextHelper.GetCurrentTraceSettings();
            this._isTracingEnabled = _currentTraceSettings.IsBasicTracingEnabled;
            this._iContainerManager = iContainerManager;
            this._iCategoryManager = iCategoryManager;
        }

        #endregion Constructors

        #region Methods

        #region Public Methods

        #region Get Methods

        /// <summary>
        /// Get all entity variant definition mappings
        /// </summary>
        /// <param name="callerContext">Indicates the caller context specifying the caller application and module.</param>
        /// <returns>Returns collection of entity variant definition mappings</returns>
        public EntityVariantDefinitionMappingCollection GetAll(CallerContext callerContext)
        {
            EntityVariantDefinitionMappingCollection entityVariantDefinitionMappings = null;

            #region Step : Diagnostic Activity initialization

            DiagnosticActivity diagnosticActivity = new DiagnosticActivity();

            String traceMessage = String.Empty;

            if (_isTracingEnabled)
            {
                diagnosticActivity.Start();
                diagnosticActivity.LogInformation("Starting get all internal execution flow for Entity Variant Definition Mappings.");
            }

            #endregion

            try
            {

                #region Load Entity Variant Definitions from cache

                var bufferManager = new CacheBufferManager<EntityVariantDefinitionMappingCollection>(CacheKeyGenerator.GetAllEntityVariantDefinitionMappings(), String.Empty);
                entityVariantDefinitionMappings = bufferManager.GetAllObjectsFromCache();

                if (_isTracingEnabled)
                {
                    diagnosticActivity.LogDurationInfo("Entity Variant Definition Mappings loaded from cache.");
                }

                #endregion

                #region Load Entity Variant Definitions from DB

                if (entityVariantDefinitionMappings == null || (entityVariantDefinitionMappings != null && entityVariantDefinitionMappings.Count < 1))
                {
                    ApplicationContextBL applicationContextManager = new ApplicationContextBL();
                    EntityVariantDefinitionBL entityVariantDefinitionManager = new EntityVariantDefinitionBL();

                    ApplicationContextObjectMappingCollection applicationContextObjectMappings = applicationContextManager.GetApplicationContextObjectMappings(null, 1, ApplicationContextMatchType.MatchByObjectTypeMap, callerContext);
                    ApplicationContextCollection allApplicationContext = applicationContextManager.Get(callerContext);
                    EntityVariantDefinitionCollection entityVariantDefinitions = entityVariantDefinitionManager.GetAll(callerContext);

                    if (allApplicationContext != null && applicationContextObjectMappings != null && allApplicationContext.Count > 0 && applicationContextObjectMappings.Count > 0)
                    {
                        entityVariantDefinitionMappings = new EntityVariantDefinitionMappingCollection();

                        foreach (ApplicationContextObjectMapping item in applicationContextObjectMappings)
                        {
                            IApplicationContext applicationContext = allApplicationContext.GetApplicationContext(item.ApplicationContextId);
                            EntityVariantDefinition entityVariantDefinition = entityVariantDefinitions.GetById(ValueTypeHelper.Int32TryParse(item.ObjectId.ToString(), 0));

                            if (applicationContext != null && entityVariantDefinition != null)
                            {
                                EntityVariantDefinitionMapping entityVariantDefinitionmapping = new EntityVariantDefinitionMapping();

                                entityVariantDefinitionmapping.Id = item.Id;
                                entityVariantDefinitionmapping.EntityVariantDefinitionId = entityVariantDefinition.Id;
                                entityVariantDefinitionmapping.EntityVariantDefinitionName = entityVariantDefinition.Name;
                                entityVariantDefinitionmapping.ContainerId = applicationContext.ContainerId;
                                entityVariantDefinitionmapping.ContainerName = applicationContext.ContainerName;
                                entityVariantDefinitionmapping.CategoryId = applicationContext.CategoryId;
                                entityVariantDefinitionmapping.CategoryName = applicationContext.CategoryName;
                                entityVariantDefinitionmapping.CategoryPath = applicationContext.CategoryPath;

                                entityVariantDefinitionMappings.Add(entityVariantDefinitionmapping);
                            }
                        }
                    }

                    if (_isTracingEnabled)
                    {
                        diagnosticActivity.LogDurationInfo("Entity Variant Definition Mappings loaded from DB.");
                    }

                    #region Update Entity Variant Definition Object Mappings in Cache

                    if (entityVariantDefinitionMappings != null && entityVariantDefinitionMappings.Count > 0)
                    {
                        bufferManager.SetBusinessObjectsToCache(entityVariantDefinitionMappings, 10);

                        if (_isTracingEnabled)
                        {
                            diagnosticActivity.LogInformation("Entity Variant Definition Mapping updated in Cache.");
                        }
                    }
                    else
                    {
                        if (_isTracingEnabled)
                        {
                            diagnosticActivity.LogInformation("No records for Entity Variant Definition Mappings are available in the Database.");
                        }
                    }

                    #endregion Update Entity Variant Definition object Mappings in Cache

                }

                #endregion Load Entity Variant Definitions from DB
            }
            finally
            {
                #region Step: Final Diagnostics and tracing

                if (_isTracingEnabled)
                {
                    diagnosticActivity.LogMessageWithData("See 'View Data' for Entity Variant Definition Mappings after get all.", entityVariantDefinitionMappings.ToXml());
                    diagnosticActivity.Stop();
                }

                #endregion
            }
            
            return entityVariantDefinitionMappings;
        }

        #endregion Get Methods

        #region Process Methods

        /// <summary>
        /// Create, Update and Delete entity variant definition mapping 
        /// </summary>
        /// <param name="entityVariantDefinitionMappings">Indicates collection of entity variant definition mappings to be processed.</param>
        /// <param name="callerContext">Indicates the caller context specifying the caller application and module.</param>
        /// <returns>Returns operation result.</returns>
        public OperationResultCollection Process(EntityVariantDefinitionMappingCollection entityVariantDefinitionMappings, CallerContext callerContext)
        {
            OperationResultCollection operationResults = new OperationResultCollection();

            OperationResultCollection applicationContextOperationResults = new OperationResultCollection();
            OperationResultCollection objectMappingOperationResults = new OperationResultCollection();

            ApplicationContextBL applicationContextManager = new ApplicationContextBL();
            
            operationResults.OperationResultStatus = OperationResultStatusEnum.Failed;

            #region Diagnostic Activity initialization

            DiagnosticActivity diagnosticActivity = new DiagnosticActivity();

            String traceMessage = String.Empty;

            if (_isTracingEnabled)
            {
                diagnosticActivity.Start();
                diagnosticActivity.LogInformation("Starting Process internal execution flow for Entity Variant Definition Mappings.");
            }

            #endregion

            #region Validation

            ValidateInputParameters(entityVariantDefinitionMappings, callerContext);

            if (_isTracingEnabled)
            {
                diagnosticActivity.LogDurationInfo("Completed validation for Entity Variant Definition Mappings.");
            }

            #endregion Validation

            #region Fill Application Context Data

            ApplicationContextCollection applicationContexts = new ApplicationContextCollection();
            ApplicationContextObjectMappingCollection applicationContextObjectMappings = new ApplicationContextObjectMappingCollection();

            foreach (EntityVariantDefinitionMapping entityVariantDefinitionMapping in entityVariantDefinitionMappings)
            {
                ApplicationContext applicationContext = new ApplicationContext(0, entityVariantDefinitionMapping.ContainerId, 0, 0, entityVariantDefinitionMapping.CategoryId, 0, 0, GlobalizationHelper.GetSystemDataLocale().ToString(), 0, 0, ApplicationContextType.CC, 1, "Entity Variant Definition");

                applicationContext.ReferenceId = ValueTypeHelper.Int32TryParse(entityVariantDefinitionMapping.ReferenceId, 0);

                applicationContexts.Add(applicationContext);
            }

            if (_isTracingEnabled)
            {
                diagnosticActivity.LogDurationInfo("Fill application context data completed for Entity Variant Definition Mappings.");
            }

            #endregion Fill Application Context Data

            #region DB Operation For Application Context

            applicationContextOperationResults = applicationContextManager.CreateAndGetApplicationContexts(applicationContexts, callerContext);

            if (_isTracingEnabled)
            {
                diagnosticActivity.LogDurationInfo("Completed process of application contexts for Entity Variant Definition Mappings.");
            }

            #endregion DB Operation For Application Context

            #region Fill Application Context Object Mapping Data

            foreach (EntityVariantDefinitionMapping entityVariantDefinitionMapping in entityVariantDefinitionMappings)
            {
                String applicationContextName = String.Format(Constants.APPLICATION_CONTEXT_NAME, 0, entityVariantDefinitionMapping.ContainerId, entityVariantDefinitionMapping.CategoryId, 0, 0, 0, 0, 0, 0, 1);

                OperationResult applicationContextOR = applicationContextOperationResults.GetOperationResultByReferenceId(applicationContextName);

                if (applicationContextOR != null && applicationContextOR.HasError)
                {
                    applicationContextOR.ReferenceId = entityVariantDefinitionMapping.ReferenceId;
                    operationResults.Add(applicationContextOR);
                }
                else
                {
                    ApplicationContext applicationContext = applicationContexts.GetApplicationContext(applicationContextName);

                    if (applicationContext != null)
                    {
                        ApplicationContextObjectMapping applicationContextObjectMapping = new ApplicationContextObjectMapping();
                        applicationContextObjectMapping.ReferenceId = entityVariantDefinitionMapping.ReferenceId;
                        applicationContextObjectMapping.Id = entityVariantDefinitionMapping.Id;
                        applicationContextObjectMapping.ApplicationContextId = applicationContext.Id;
                        applicationContextObjectMapping.ApplicationContextType = ApplicationContextType.CC;
                        applicationContextObjectMapping.ContextObjectTypeId = 1;
                        applicationContextObjectMapping.ObjectId = entityVariantDefinitionMapping.EntityVariantDefinitionId;
                        applicationContextObjectMapping.Action = entityVariantDefinitionMapping.Action;

                        applicationContextObjectMappings.Add(applicationContextObjectMapping);
                    }
                }
            }

            if (_isTracingEnabled)
            {
                diagnosticActivity.LogDurationInfo("Fill application context object mapping data completed for Entity Variant Definition Mappings.");
            }

            #endregion Fill Application Context Object Mapping Data

            #region Validate Entity Variant Definition Mappings Data

            ValidateEntityVariantDefinitionMappingsData(applicationContextObjectMappings, entityVariantDefinitionMappings, operationResults, callerContext);

            if (_isTracingEnabled)
            {
                diagnosticActivity.LogDurationInfo("Completed validation of entity variant definition mappings data.");
            }

            #endregion Validate Entity Variant Definition Mappings Data

            #region DB Operation For Application Context Object Mapping

            objectMappingOperationResults = applicationContextManager.ProcessObjectMapping(applicationContextObjectMappings, callerContext);

            if (_isTracingEnabled)
            {
                diagnosticActivity.LogDurationInfo("Completed process of application context object mappings for Entity Variant Definition Mappings.");
            }

            #endregion DB Operation For Application Context Object Mapping

            #region Invalidate Cache

            InvalidateCache();

            #endregion Invalidate Cache

            if (objectMappingOperationResults != null && objectMappingOperationResults.Count > 0)
            {
                operationResults.AddRange(objectMappingOperationResults);
            }

            return operationResults;
        }

        #endregion Process Methods

        #region IDataModelManager Methods

        /// <summary>
        /// Fills missing information to data model objects.
        /// </summary>
        /// <param name="iDataModelObjects">Collection of data model objects to be filled up.</param>
        /// <param name="iCallerContext">Context of caller making call to this API.</param>
        public void FillDataModel(IDataModelObjectCollection iDataModelObjects, ICallerContext iCallerContext)
        {
            EntityVariantDefinitionMappingCollection entityVariantDefinitionMappings = iDataModelObjects as EntityVariantDefinitionMappingCollection;
            CallerContext callerContext = iCallerContext as CallerContext;

            #region Step : Diagnostic Activity initialization

            DiagnosticActivity diagnosticActivity = new DiagnosticActivity();

            String traceMessage = String.Empty;

            if (_isTracingEnabled)
            {
                diagnosticActivity.Start();
                diagnosticActivity.LogInformation("Starting fill data model internal execution flow for Entity Variant Definition Mappings.");
            }

            #endregion

            try
            {
                FillEntityVariantDefinitionMappings(entityVariantDefinitionMappings, callerContext);
            }
            finally
            {
                #region Step: Final Diagnostics and tracing

                if (_isTracingEnabled)
                {
                    diagnosticActivity.LogMessageWithData("See 'View Data' for Entity Variant Definition Mappings after FillDataModel.", entityVariantDefinitionMappings.ToXml());
                    diagnosticActivity.Stop();
                }

                #endregion
            }
        }

        /// <summary>
        /// Validates DataModelObjects and populate OperationResults
        /// </summary>
        /// <param name="iDataModelObjects">Collection of data model objects to be filled up.</param>
        /// <param name="operationResults">Collection of DataModel OperationResult</param>
        /// <param name="iCallerContext">Indicates the context of the caller</param>
        /// <returns>DataModel Operation Result Collection</returns>
        public void Validate(IDataModelObjectCollection iDataModelObjects, BusinessObjects.DataModel.DataModelOperationResultCollection operationResults, ICallerContext iCallerContext)
        {
            EntityVariantDefinitionMappingCollection entityVariantDefinitionMappings = iDataModelObjects as EntityVariantDefinitionMappingCollection;
            CallerContext callerContext = iCallerContext as CallerContext;

            #region Step : Diagnostic Activity initialization

            DiagnosticActivity diagnosticActivity = new DiagnosticActivity();

            String traceMessage = String.Empty;

            if (_isTracingEnabled)
            {
                diagnosticActivity.Start();
                diagnosticActivity.LogInformation("Starting validate internal execution flow for Entity Variant Definition Mappings.");
            }

            #endregion

            try
            {
                ValidateInputParameters(entityVariantDefinitionMappings, operationResults, callerContext);
                ValidateEntityVariantDefinitionMappingsProperties(entityVariantDefinitionMappings, operationResults, callerContext);
            }
            finally
            {
                #region Step: Final Diagnostics and tracing

                if (_isTracingEnabled)
                {
                    diagnosticActivity.LogMessageWithData("See 'View Data' for Entity Variant Definition Mappings after Validate.", operationResults.ToXml());
                    diagnosticActivity.Stop();
                }

                #endregion
            }
        }

        /// <summary>
        /// Load original data model objects
        /// </summary>
        /// <param name="iDataModelObjects">Collection of data model objects to load original object.</param>
        /// <param name="operationResult">Collection of DataModel OperationResult</param>
        /// <param name="iCallerContext">Context of caller making call to this API.</param>
        public void LoadOriginal(IDataModelObjectCollection iDataModelObjects, BusinessObjects.DataModel.DataModelOperationResultCollection operationResult, ICallerContext iCallerContext)
        {
            EntityVariantDefinitionMappingCollection entityVariantDefinitionMappings = iDataModelObjects as EntityVariantDefinitionMappingCollection;
            CallerContext callerContext = iCallerContext as CallerContext;

            #region Step : Diagnostic Activity initialization

            DiagnosticActivity diagnosticActivity = new DiagnosticActivity();

            String traceMessage = String.Empty;

            if (_isTracingEnabled)
            {
                diagnosticActivity.Start();
                diagnosticActivity.LogInformation("Starting load original internal execution flow for Entity Variant Definition Mappings.");
            }

            #endregion

            try
            {
                LoadOriginalEntityVariantDefinitionMappings(entityVariantDefinitionMappings, callerContext);
            }
            finally
            {
                #region Step: Final Diagnostics and tracing

                if (_isTracingEnabled)
                {
                    diagnosticActivity.LogMessageWithData("See 'View Data' for Entity Variant Definition Mappings after LoadOriginal.", entityVariantDefinitionMappings.ToXml());
                    diagnosticActivity.Stop();
                }

                #endregion
            }
        }

        /// <summary>
        /// Compare and merge data model object collection with current collection
        /// </summary>
        /// <param name="iDataModelObjects">Collection of data model objects to be compare and merge.</param>
        /// <param name="operationResult">Collection of DataModel OperationResult</param>
        /// <param name="iCallerContext">Context of caller making call to this API.</param>
        /// <returns>Returns merged data model objects.</returns>
        public void CompareAndMerge(IDataModelObjectCollection iDataModelObjects, BusinessObjects.DataModel.DataModelOperationResultCollection operationResult, ICallerContext iCallerContext)
        {
            EntityVariantDefinitionMappingCollection entityVariantDefinitionMappings = iDataModelObjects as EntityVariantDefinitionMappingCollection;
            CallerContext callerContext = iCallerContext as CallerContext;

            #region Step : Diagnostic Activity initialization

            DiagnosticActivity diagnosticActivity = new DiagnosticActivity();

            String traceMessage = String.Empty;

            if (_isTracingEnabled)
            {
                diagnosticActivity.Start();
                diagnosticActivity.LogInformation("Starting compare and merge internal execution flow for Entity Variant Definition Mappings.");
            }

            #endregion

            try
            {
                CompareAndMerge(entityVariantDefinitionMappings, operationResult, callerContext);
            }
            finally
            {
                #region Step: Final Diagnostics and tracing

                if (_isTracingEnabled)
                {
                    diagnosticActivity.LogMessageWithData("See 'View Data' for Entity Variant Definitions after CompareAndMerge.", entityVariantDefinitionMappings.ToXml());
                    diagnosticActivity.Stop();
                }

                #endregion
            }
        }

        /// <summary>
        /// Process data model collection
        /// </summary>
        /// <param name="iDataModelObjects">Collection of data model objects to be processed.</param>
        /// <param name="operationResult">Collection of DataModel OperationResult</param>
        /// <param name="iCallerContext">Context of caller making call to this API.</param>
        /// <returns>Result of operation</returns>
        public void Process(IDataModelObjectCollection iDataModelObjects, BusinessObjects.DataModel.DataModelOperationResultCollection operationResult, ICallerContext iCallerContext)
        {
            OperationResultCollection operationResults = Process(iDataModelObjects as EntityVariantDefinitionMappingCollection, iCallerContext as CallerContext);

            foreach (OperationResult item in operationResults)
            {
                DataModelOperationResult dataModelOperationResult = operationResult.GetByReferenceId(item.ReferenceId) as DataModelOperationResult;
                dataModelOperationResult.CopyErrorInfoAndWarning(item);
            }
        }
      
        ///<summary>
        /// Processes the Entity variant cache status for data model objects
        /// </summary>
        /// <param name="iDataModelObjects">Collection of data model objects to be process entity cache load.</param>
        /// <param name="operationResult">Collection of DataModel OperationResult</param>
        /// <param name="iCallerContext">Context of caller making call to this API.</param>
        public void InvalidateCache(IDataModelObjectCollection iDataModelObjects, BusinessObjects.DataModel.DataModelOperationResultCollection operationResult, ICallerContext iCallerContext)
        {
            InvalidateCache();
        }

        /// <summary>
        /// Generate and OperationResult Schema for given DataModelObjectCollection
        /// </summary>
        /// <param name="iDataModelObjects">Container entity type attribute mapping object</param>
        /// <param name="operationResult">Collection of DataModel OperationResult</param>
        public void PrepareOperationResultsSchema(IDataModelObjectCollection iDataModelObjects, BusinessObjects.DataModel.DataModelOperationResultCollection operationResults)
        {
            #region Step : Diagnostic Activity initialization

            DiagnosticActivity diagnosticActivity = new DiagnosticActivity();

            String traceMessage = String.Empty;

            if (_isTracingEnabled)
            {
                diagnosticActivity.Start();
                diagnosticActivity.LogInformation("Starting prepare operation results schema internal execution flow for Entity Variant Definition Mappings.");
            }

            #endregion

            try
            {
                String separator = AppConfigurationHelper.GetAppConfig<String>("MDMCenter.RSDataModelExcel.Separator", "//").Trim();
                EntityVariantDefinitionMappingCollection entityVariantDefinitionMappings = iDataModelObjects as EntityVariantDefinitionMappingCollection;

                if (entityVariantDefinitionMappings != null && entityVariantDefinitionMappings.Count > 0)
                {
                    if (operationResults.Count > 0)
                    {
                        operationResults.Clear();
                    }

                    int entityVariantDefinitionIdToBeCreated = -1;

                    foreach (EntityVariantDefinitionMapping entityVariantDefinitionMapping in entityVariantDefinitionMappings)
                    {
                        DataModelOperationResult EntityVariantDefinitioinOperationResult = new DataModelOperationResult(entityVariantDefinitionMapping.Id, entityVariantDefinitionMapping.Name, entityVariantDefinitionMapping.ExternalId, entityVariantDefinitionMapping.ReferenceId);

                        if (String.IsNullOrEmpty(EntityVariantDefinitioinOperationResult.ExternalId))
                        {
                            EntityVariantDefinitioinOperationResult.ExternalId = String.Format("{1} {0} {2} {0} {3}", separator, entityVariantDefinitionMapping.EntityVariantDefinitionName, entityVariantDefinitionMapping.ContainerName, entityVariantDefinitionMapping.Name);
                        }

                        if (entityVariantDefinitionMapping.Id < 1)
                        {
                            entityVariantDefinitionMapping.Id = entityVariantDefinitionIdToBeCreated;
                            EntityVariantDefinitioinOperationResult.Id = entityVariantDefinitionIdToBeCreated;
                            entityVariantDefinitionIdToBeCreated--;
                        }

                        operationResults.Add(EntityVariantDefinitioinOperationResult);
                    }
                }
            }
            finally
            {
                #region Step: Final Diagnostics and tracing

                if (_isTracingEnabled)
                {
                    diagnosticActivity.LogMessageWithData("See 'View Data' for Entity Variant Definition Mappings operation results after PrepareOperationResultsSchema.", operationResults.ToXml());
                    diagnosticActivity.Stop();
                }

                #endregion
            }
        }

        #endregion IDataModelManager Methods

        #endregion Public Methods

        #region Private Methods

        private void FillEntityVariantDefinitionMappings(EntityVariantDefinitionMappingCollection entityVariantDefinitionMappings, CallerContext callerContext)
        {
            if (entityVariantDefinitionMappings != null && entityVariantDefinitionMappings.Count() > 0)
            {
                EntityVariantDefinition entityVariantDefiniton = null;
                Container container = null;
                Int32 hierarchyId = 0;
                
                foreach (EntityVariantDefinitionMapping entityVariantDefinitionMapping in entityVariantDefinitionMappings)
                {
                    EntityVariantDefinitionMapping originalEntityVariantDefinitionMaping = entityVariantDefinitionMapping.OriginalEntityVariantDefinitionMapping;

                    if (entityVariantDefinitionMapping.OriginalEntityVariantDefinitionMapping != null && String.Compare(entityVariantDefinitionMapping.EntityVariantDefinitionName, originalEntityVariantDefinitionMaping.EntityVariantDefinitionName) == 0)
                    {
                        entityVariantDefinitionMapping.Id = originalEntityVariantDefinitionMaping.Id;
                        entityVariantDefinitionMapping.EntityVariantDefinitionId = originalEntityVariantDefinitionMaping.EntityVariantDefinitionId;
                    }
                    else
                    {
                        entityVariantDefiniton = new EntityVariantDefinitionBL().GetByName(entityVariantDefinitionMapping.EntityVariantDefinitionName, callerContext);

                        if (entityVariantDefiniton != null)
                        {
                            entityVariantDefinitionMapping.EntityVariantDefinitionId = entityVariantDefiniton.Id;
                        }
                    }

                    if (String.IsNullOrWhiteSpace(entityVariantDefinitionMapping.ContainerName))
                    {
                        entityVariantDefinitionMapping.ContainerId = 0;
                    }
                    else
                    {

                        container = _iContainerManager.Get(entityVariantDefinitionMapping.ContainerName, callerContext, true);

                        if(container != null)
                        {
                            entityVariantDefinitionMapping.ContainerId = container.Id;
                            hierarchyId = container.HierarchyId;
                        }
                    }

                    if (entityVariantDefinitionMapping.OriginalEntityVariantDefinitionMapping != null && String.Compare(entityVariantDefinitionMapping.CategoryPath, originalEntityVariantDefinitionMaping.CategoryPath) == 0)
                    {
                        entityVariantDefinitionMapping.CategoryId = originalEntityVariantDefinitionMaping.CategoryId;
                        entityVariantDefinitionMapping.CategoryName = originalEntityVariantDefinitionMaping.CategoryName;
                    }
                    else
                    {
                        if (String.IsNullOrWhiteSpace(entityVariantDefinitionMapping.CategoryPath))
                        {
                            entityVariantDefinitionMapping.CategoryId = 0;
                        }
                        else
                        {
                            Category category = _iCategoryManager.GetByPath(hierarchyId, entityVariantDefinitionMapping.CategoryPath, callerContext);

                            if (category != null)
                            {
                                entityVariantDefinitionMapping.CategoryId = category.Id;
                                entityVariantDefinitionMapping.CategoryName = category.Name;
                            }
                        }
                    }
                }
            }
        }

        private void ValidateInputParameters(EntityVariantDefinitionMappingCollection entityVariantDefinitionMappings, DataModelOperationResultCollection operationResults, CallerContext callerContext)
        {
            if (entityVariantDefinitionMappings != null && entityVariantDefinitionMappings.Count() == 0)
            {
                DataModelHelper.AddOperationResults(operationResults, "114253", String.Empty, null, OperationResultType.Error, TraceEventType.Error, callerContext);
            }

            if (callerContext == null)
            {
                DataModelHelper.AddOperationResults(operationResults, "111846", String.Empty, null, OperationResultType.Error, TraceEventType.Error, callerContext);
            }
        }

        private void ValidateInputParameters(EntityVariantDefinitionMappingCollection entityVariantDefinitionMappings, CallerContext callerContext)
        {
            String errorMessage;

            if (entityVariantDefinitionMappings == null || entityVariantDefinitionMappings.Count < 1)
            {
                errorMessage = "Entity variant definition mapping collection is not available or empty";
                throw new MDMOperationException("", errorMessage, "DataModelManager.EntityVariantDefinitionMappingBL", String.Empty, "Create");
            }

            if (callerContext == null)
            {
                errorMessage = "CallerContext cannot be null.";
                throw new MDMOperationException("", "CallerContext cannot be null.", "DataModelManager.EntityVariantDefinitionMappingBL", String.Empty, "Create");
            }
        }

        private void ValidateEntityVariantDefinitionMappingsProperties(EntityVariantDefinitionMappingCollection entityVariantDefinitionMappings, DataModelOperationResultCollection operationResults, CallerContext callerContext)
        {
            EntityVariantDefinitionCollection uniqueEntityVariantDefinitions = new EntityVariantDefinitionCollection();

            foreach (EntityVariantDefinitionMapping entityVariantDefinitionMapping in entityVariantDefinitionMappings)
            {
                IDataModelOperationResult operationResult = operationResults.GetByReferenceId(entityVariantDefinitionMapping.ReferenceId);

                if (entityVariantDefinitionMapping.Action == ObjectAction.Read || entityVariantDefinitionMapping.Action == ObjectAction.Delete
                    || entityVariantDefinitionMapping.Action == ObjectAction.Ignore)
                {
                    continue;
                }

                if (String.IsNullOrWhiteSpace(entityVariantDefinitionMapping.EntityVariantDefinitionName))
                {
                    DataModelHelper.AddOperationResult(operationResult, "114254", String.Empty, null, OperationResultType.Error, TraceEventType.Error, callerContext);
                }
            }
        }

        private void LoadOriginalEntityVariantDefinitionMappings(EntityVariantDefinitionMappingCollection entityVariantDefinitionMappings, CallerContext callerContext)
        {
            if (entityVariantDefinitionMappings != null && entityVariantDefinitionMappings.Count() > 0)
            {
                EntityVariantDefinitionMappingCollection allEntityVariantDefinitionMappings = GetAll(callerContext);

                if (allEntityVariantDefinitionMappings != null && allEntityVariantDefinitionMappings.Count() > 0)
                {
                    foreach (EntityVariantDefinitionMapping entityVariantDefinitionMapping in entityVariantDefinitionMappings)
                    {
                        entityVariantDefinitionMapping.OriginalEntityVariantDefinitionMapping = allEntityVariantDefinitionMappings.Get(entityVariantDefinitionMapping.EntityVariantDefinitionName, entityVariantDefinitionMapping.ContainerName, entityVariantDefinitionMapping.CategoryPath);
                    }
                }
            }
        }

        private void CompareAndMerge(EntityVariantDefinitionMappingCollection entityVariantDefinitionMappings, DataModelOperationResultCollection operationResults, CallerContext callerContext)
        {
            if (entityVariantDefinitionMappings != null && entityVariantDefinitionMappings.Count() > 0)
            {
                foreach (EntityVariantDefinitionMapping deltaEntityVariantDefinitionMapping in entityVariantDefinitionMappings)
                {
                    IDataModelOperationResult operationResult = operationResults.GetByReferenceId(deltaEntityVariantDefinitionMapping.ReferenceId);

                    if (deltaEntityVariantDefinitionMapping.Action == ObjectAction.Read || deltaEntityVariantDefinitionMapping.Action == ObjectAction.Ignore)
                    {
                        continue;
                    }

                    EntityVariantDefinitionMapping originalEntityVariantDefinitionMapping = deltaEntityVariantDefinitionMapping.OriginalEntityVariantDefinitionMapping;

                    if (originalEntityVariantDefinitionMapping != null)
                    {
                        if (deltaEntityVariantDefinitionMapping.Action != ObjectAction.Delete)
                        {
                            originalEntityVariantDefinitionMapping.MergeDelta(deltaEntityVariantDefinitionMapping, callerContext, false);
                        }
                    }
                    else
                    {
                        if (deltaEntityVariantDefinitionMapping.Action == ObjectAction.Delete)
                        {
                            Object[] parameters = new Object[] { deltaEntityVariantDefinitionMapping.EntityVariantDefinitionName, deltaEntityVariantDefinitionMapping.ContainerName, deltaEntityVariantDefinitionMapping.CategoryPath };
                            DataModelHelper.AddOperationResult(operationResult, "114255", String.Empty, parameters, OperationResultType.Error, TraceEventType.Error, callerContext);
                        }
                        else
                        {
                            if (deltaEntityVariantDefinitionMapping.EntityVariantDefinitionId < 1)
                            {
                                Object[] parameters = new Object[] { deltaEntityVariantDefinitionMapping.EntityVariantDefinitionName };
                                DataModelHelper.AddOperationResult(operationResult, "114259", String.Empty, parameters, OperationResultType.Error, TraceEventType.Error, callerContext);
                            }

                            if (deltaEntityVariantDefinitionMapping.ContainerId < 0)
                            {
                                Object[] parameters = new Object[] { deltaEntityVariantDefinitionMapping.ContainerName };
                                DataModelHelper.AddOperationResult(operationResult, "114256", String.Empty, parameters, OperationResultType.Error, TraceEventType.Error, callerContext);
                            }

                            if (deltaEntityVariantDefinitionMapping.CategoryId < 0)
                            {
                                Object[] parameters = new Object[] { deltaEntityVariantDefinitionMapping.CategoryPath };
                                DataModelHelper.AddOperationResult(operationResult, "114257", String.Empty, parameters, OperationResultType.Error, TraceEventType.Error, callerContext);
                            }
                        }
                    }
                }
            }
        }

        private void InvalidateCache()
        {
            String cacheKey = CacheKeyGenerator.GetAllEntityVariantDefinitionMappings();
            var cacheBufferManager = new CacheBufferManager<EntityVariantDefinitionMappingCollection>(cacheKey, String.Empty);
            cacheBufferManager.RemoveBusinessObjectFromCache(true, cacheKey);
        }

        private void ValidateEntityVariantDefinitionMappingsData(ApplicationContextObjectMappingCollection applicationContextObjectMappings, EntityVariantDefinitionMappingCollection entityVariantDefinitionMappings, OperationResultCollection operationResults, CallerContext callerContext)
        {

            EntityVariantDefinitionBL entityVariantDefinitionManager = new EntityVariantDefinitionBL();

            Boolean isValidCategory = false;
            CategoryCollection categories = null;
            EntityVariantDefinition entityVariantDefinition = null;
            ApplicationContextObjectMapping applicationContextObjectMapping = null;
            ApplicationContextBL appContextBL = new ApplicationContextBL();
            Dictionary<Int32, CategoryCollection> contaienrBaseCategories = new Dictionary<int, CategoryCollection>();
          
            EntityVariantDefinitionCollection allEntityVariantDefinition = entityVariantDefinitionManager.GetAll(callerContext);
            ApplicationContextObjectMappingCollection allApplicationContextObjectMappingsFromDB = appContextBL.GetApplicationContextObjectMappings(null, 1, ApplicationContextMatchType.MatchByObjectTypeMap, callerContext);

            foreach (EntityVariantDefinitionMapping item in entityVariantDefinitionMappings)
            {
                if (allEntityVariantDefinition != null && entityVariantDefinitionMappings != null)
                {
                    entityVariantDefinition = allEntityVariantDefinition.GetById(item.EntityVariantDefinitionId);

                    if (item.Action == ObjectAction.Create)
                    {
                        applicationContextObjectMapping = (ApplicationContextObjectMapping)applicationContextObjectMappings.GetByReferenceId(item.ReferenceId);
                    }
                    else
                    {
                        applicationContextObjectMapping = (ApplicationContextObjectMapping)applicationContextObjectMappings.GetById(item.Id);
                    }

                    if (entityVariantDefinition != null && applicationContextObjectMapping != null)
                    {
                        #region Check for valid category

                        if (item.ContainerId > 0 && item.CategoryId > 0)
                        {
                            Container container = _iContainerManager.Get(item.ContainerId, callerContext, true);

                            if (!contaienrBaseCategories.TryGetValue(container.HierarchyId, out categories))
                            {
                                categories = _iCategoryManager.GetAllCategories(container.HierarchyId, callerContext);
                                contaienrBaseCategories.Add(container.HierarchyId, categories);
                            }

                            if (categories != null && categories.Count > 0)
                            {
                                foreach (var category in categories)
                                {
                                    if (category.Id == item.CategoryId)
                                    {
                                        isValidCategory = true;
                                        break;
                                    }
                                }
                            }

                            if (!isValidCategory)
                            {
                                OperationResult operationResult = new OperationResult();
                                Object[] parameters = new Object[] { entityVariantDefinition.RootEntityTypeName, item.ContainerName, item.CategoryPath };
                                LocaleMessage _localeMessage = _localeMessageManager.Get(_systemUILocale, "", parameters, false, callerContext);

                                operationResult.AddOperationResult("", "invalid category: {0} > {1} > {2}", item.ReferenceId, parameters.ToCollection(), OperationResultType.Error);
                                operationResults.Add(operationResult);

                                applicationContextObjectMappings.Remove(applicationContextObjectMapping);
                            }
                        }

                        #endregion Check for valid category

                        #region Unique root entity type check

                        ApplicationContextObjectMappingCollection objectMapppingsFromDBFilteredByAppContext = (ApplicationContextObjectMappingCollection)allApplicationContextObjectMappingsFromDB.GetByApplicationContextId(applicationContextObjectMapping.ApplicationContextId);

                        if (objectMapppingsFromDBFilteredByAppContext != null && objectMapppingsFromDBFilteredByAppContext.Count > 0)
                        {
                            foreach (ApplicationContextObjectMapping mapping in objectMapppingsFromDBFilteredByAppContext)
                            {
                                if (item.Action != ObjectAction.Delete)
                                {
                                    if (mapping.ObjectId == item.EntityVariantDefinitionId)
                                    {
                                        UpdateOperationResult(entityVariantDefinition.RootEntityTypeName, item, operationResults, callerContext);
                                        applicationContextObjectMappings.Remove(applicationContextObjectMapping);
                                    }
                                    else
                                    {
                                        EntityVariantDefinition evd = allEntityVariantDefinition.Get((Int32)mapping.ObjectId);
                                        if (evd != null)
                                        {
                                            if (evd.RootEntityTypeId == entityVariantDefinition.RootEntityTypeId)
                                            {
                                                UpdateOperationResult(entityVariantDefinition.RootEntityTypeName, item, operationResults, callerContext);
                                                applicationContextObjectMappings.Remove(applicationContextObjectMapping);
                                            } 
                                        }
                                    }
                                }
                            }
                        }

                        #endregion Unique root entity check
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rootEntityTypeName"></param>
        /// <param name="item"></param>
        /// <param name="operationResults"></param>
        /// <param name="callerContext"></param>
        private void UpdateOperationResult(String rootEntityTypeName, EntityVariantDefinitionMapping item, OperationResultCollection operationResults, CallerContext callerContext)
        {
            OperationResult operationResult = new OperationResult();
            Object[] parameters = new Object[] { rootEntityTypeName, item.ContainerName, item.CategoryPath };
            LocaleMessage _localeMessage = _localeMessageManager.Get(_systemUILocale, "114258", parameters, false, callerContext);

            operationResult.AddOperationResult("114258", _localeMessage.Message, item.ReferenceId, parameters.ToCollection(), OperationResultType.Error);
            operationResult.ReferenceId = item.ReferenceId;
            operationResults.Add(operationResult);
        }

        #endregion Private Methods

        #endregion Methods
    }
}
