using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Collections.Generic;

namespace MDM.DataModelManager.Business
{
    using MDM.BusinessObjects;
    using MDM.BusinessObjects.DataModel;
    using MDM.BusinessObjects.Diagnostics;
    using MDM.ConfigurationManager.Business;
    using MDM.Core;
    using MDM.DataModelManager.Data;
    using MDM.Interfaces;
    using MDM.MessageManager.Business;
    using MDM.Utility;
    using MDM.BufferManager;
    using MDM.ApplicationServiceManager.Business;

    /// <summary>
    /// Specifies business logic operations for entity variant definition
    /// </summary>
    public class EntityVariantDefinitionBL : IDataModelManager
    {
        #region Fields

        /// <summary>
        /// Field denoting the IAttributeModelManager
        /// </summary>
        private IAttributeModelManager _iAttributeModelManager = null;

        /// <summary>
        /// 
        /// </summary>
        private LocaleEnum _systemUILocale = LocaleEnum.UnKnown;

        /// <summary>
        /// Indicates current trace settings
        /// </summary>
        private TraceSettings _currentTraceSettings = new TraceSettings();

        /// <summary>
        /// Indicates whether diagnostic tracing is enable or not
        /// </summary>
        private Boolean isTracingEnabled = false;

        /// <summary>
        /// Indicates locale message 
        /// </summary>
        private LocaleMessageBL localeMessageManager = new LocaleMessageBL();

        #endregion

        #region Constructors

        /// <summary>
        /// 
        /// </summary>
        public EntityVariantDefinitionBL()
        {
            this._systemUILocale = GlobalizationHelper.GetSystemUILocale();
            _currentTraceSettings = MDMOperationContextHelper.GetCurrentTraceSettings();
            isTracingEnabled = _currentTraceSettings.IsBasicTracingEnabled;
        }

        /// <summary>
        /// 
        /// </summary>
        public EntityVariantDefinitionBL(IAttributeModelManager iAttributeModelManager)
        {
            this._systemUILocale = GlobalizationHelper.GetSystemUILocale();
            this._iAttributeModelManager = iAttributeModelManager;
            _currentTraceSettings = MDMOperationContextHelper.GetCurrentTraceSettings();
            isTracingEnabled = _currentTraceSettings.IsBasicTracingEnabled;
        }

        #endregion Constructors

        #region Methods

        #region Get Methods

        /// <summary>
        /// Get all the entity variant definition collection based on the specified application context
        /// </summary>
        /// <param name="dataModelContext">Indicates context of application based on which the entity variant definition collection to be returned</param>
        /// <returns>Returns all the entity variant definition collection based on the specified application context</returns>
        public EntityVariantDefinitionCollection GetByContext(ApplicationContext applicationContext)
        {
            //TO-DO : needs to move while implementing EntityVariantDefinition Mapping
            return this.Get();
        }

        /// <summary>
        /// Get all the entity variant definition collection
        /// </summary>
        /// <returns>Returns all the entity variant definition collection</returns>
        public EntityVariantDefinitionCollection GetAll(CallerContext callerContext)
        {
            EntityVariantDefinitionCollection entityVariantDefinitions = new EntityVariantDefinitionCollection();
            DiagnosticActivity diagnosticActivity = new DiagnosticActivity();

            if (isTracingEnabled)
            {
                diagnosticActivity.Start();
            }

            #region Load Entity Variant Definitions from cache

            var bufferManager = new CacheBufferManager<EntityVariantDefinitionCollection>(CacheKeyGenerator.GetAllEntityVariantDefinitions(), String.Empty);
            entityVariantDefinitions = bufferManager.GetAllObjectsFromCache();

            #endregion

            if (entityVariantDefinitions == null || entityVariantDefinitions.Count < 1)
            {
                #region Load Entity Variant Definitions from DB

                EntityVariantDefinitionDA entityVariantDefinitionDA = new EntityVariantDefinitionDA();

                if (isTracingEnabled)
                {
                    diagnosticActivity.LogInformation("Loading Entity Variant Definition from Database.");
                }

                DBCommandProperties command = DBCommandHelper.Get(callerContext, MDMCenterModuleAction.Read);

                entityVariantDefinitions = entityVariantDefinitionDA.GetAll(command, callerContext);

                if (isTracingEnabled)
                {
                    diagnosticActivity.LogData("Loaded Entity Variant Definition from Database.", entityVariantDefinitions.ToXml());
                }

                #endregion

                #region Update Entity Variant Definitions in Cache

                if (entityVariantDefinitions != null && entityVariantDefinitions.Count > 0)
                {
                    bufferManager.SetBusinessObjectsToCache(entityVariantDefinitions, 10);

                    if (isTracingEnabled)
                    {
                        diagnosticActivity.LogInformation("Entity Variant Definition updated in Cache.");
                    }
                }
                else
                {
                    if (isTracingEnabled)
                    {
                        diagnosticActivity.LogInformation("No records for Entity Variant Definitions are available in the Database.");
                    }
                }

                #endregion Update Entity Variant Definitions in Cache
            }

            if (isTracingEnabled)
            {
                diagnosticActivity.Stop();
            }

            return entityVariantDefinitions;
        }

        /// <summary>
        /// Get all entity variant definitions based on context
        /// </summary>
        /// <param name="containerId">Indicates the identifier of container, as a part of context</param>
        /// <param name="categoryId">Indicates the identifier of category, as a part of context</param>
        /// <param name="entityTypeId">Indicates the identifier of base entity type for which definition will be fetched</param>
        /// <param name="callerContext">Indicates the caller context specifying the caller application and module</param>
        /// <returns>Returns all entity variant definitions based on context</returns>
        public EntityVariantDefinition GetByContext(Int32 containerId, Int64 categoryId, Int32 entityTypeId, CallerContext callerContext)
        {

            EntityVariantDefinition entityVariantDefinition = null;
            DiagnosticActivity diagnosticActivity = new DiagnosticActivity();
            ApplicationContextBL applicationContextManager = new ApplicationContextBL();

            if (isTracingEnabled)
            {
                diagnosticActivity.Start();
            }

            try
            {
                ApplicationContext applicationContext = new ApplicationContext(0, 0, 0, 0, 0, 0, 0, GlobalizationHelper.GetSystemDataLocale().ToString(), 0, 0, ApplicationContextType.CC, 1, "");

                applicationContext.ContainerId = containerId;
                applicationContext.CategoryId = categoryId;

                ApplicationContextObjectMappingCollection applicationContextObjectMappings = applicationContextManager.GetApplicationContextObjectMappings(new ApplicationContextCollection() { applicationContext }, 1, ApplicationContextMatchType.NearestMatch, callerContext);

                if (isTracingEnabled && applicationContextObjectMappings != null)
                {
                    diagnosticActivity.LogDurationInfo(String.Format("Get application context object mappings based on container id {0} and category id {1} completed with {2} number of mappings result.", containerId, categoryId, applicationContextObjectMappings.Count));
                }

                if (applicationContextObjectMappings != null && applicationContextObjectMappings.Count > 0)
                {
                    EntityVariantDefinitionCollection allEntityVariantDefinitions = GetAll(callerContext);

                    if (allEntityVariantDefinitions != null && allEntityVariantDefinitions.Count > 0)
                    {
                        foreach (ApplicationContextObjectMapping item in applicationContextObjectMappings)
                        {
                            entityVariantDefinition = allEntityVariantDefinitions.GetById(ValueTypeHelper.Int32TryParse(item.ObjectId.ToString(), 0));

                            if (entityVariantDefinition != null && entityVariantDefinition.RootEntityTypeId == entityTypeId)
                            {
                                return entityVariantDefinition;
                            }
                        }
                    }
                }
            }
            finally
            {
                if (isTracingEnabled)
                {
                    diagnosticActivity.LogMessageWithData("See 'View Data' for loaded entity variant definitions.", entityVariantDefinition.ToXml());
                    diagnosticActivity.Stop();
                }
            }

            return entityVariantDefinition;
        }

        /// <summary>
        /// Get entity variant definitions, generic implementation
        /// </summary>
        /// <param name="applicationContext">Indicates the object with context properties</param>
        /// <param name="callerContext">Indicates the caller context specifying the caller application and module</param>
        /// <returns>Returns entity variant definitions, generic implementation</returns>
        public EntityVariantDefinitionCollection Get(CallerContext callerContext = null)
        {
            DiagnosticActivity diagnosticActivity = new DiagnosticActivity();

            if (isTracingEnabled)
            {
                if (callerContext != null)
                {
                    var ec = new ExecutionContext();
                    ec.CallerContext = callerContext;

                    diagnosticActivity.Start(ec);
                }
                else
                    diagnosticActivity.Start();
            }

            EntityVariantDefinitionCollection entityVariantDefinitions = new EntityVariantDefinitionCollection();

            #region Load Entity Variant Definitions from DB

            EntityVariantDefinitionDA entityVariantDefinitionDA = new EntityVariantDefinitionDA();
            DBCommandProperties command = DBCommandHelper.Get(callerContext, MDMCenterModuleAction.Read);
            entityVariantDefinitions = entityVariantDefinitionDA.GetAll(command, callerContext);

            #endregion

            if (isTracingEnabled)
            {
                diagnosticActivity.Stop();
            }

            return entityVariantDefinitions;
        }

        /// <summary>
        /// Get entity variant definition by Id
        /// </summary>
        /// <param name="id">Indicates the identifier of the entity variant definition</param>
        /// <param name="callerContext">Indicates the caller context specifying the caller application and module</param>
        /// <returns>Returns entity variant definition by Id</returns>
        public EntityVariantDefinition GetById(Int32 id, CallerContext callerContext)
        {
            EntityVariantDefinitionCollection entityVariantDefinitions = this.Get(callerContext);
            if (entityVariantDefinitions != null && entityVariantDefinitions.Count() > 0)
            {
                return entityVariantDefinitions.GetById(id);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Get entity variant definition by Id
        /// </summary>
        /// <param name="id">Indicates the identifier of the entity variant definition</param>
        /// <param name="callerContext">Indicates the caller context specifying the caller application and module</param>
        /// <returns>Returns entity variant definition by Id</returns>
        public EntityVariantDefinition GetByName(String entityVariantDefinitionName, CallerContext callerContext)
        {
            EntityVariantDefinitionCollection entityVariantDefinitions = this.Get(callerContext);
            if (entityVariantDefinitions != null && entityVariantDefinitions.Count() > 0)
            {
                return entityVariantDefinitions.GetByName(entityVariantDefinitionName);
            }
            else
            {
                return null;
            }
        }

        #endregion Get Methods

        #region Process Methods

        /// <summary>
        /// Process entity variant definitions based on action specified in each object
        /// </summary>
        /// <param name="entityVariantDefinitions">Indicates the entity variant definition collection to be processed.</param>
        /// <param name="applicationContext">Indicates the context of application</param>
        /// <param name="callerContext">Indicates the context of the caller specifying the caller application and module.</param>
        /// <returns>Returns operation result of processed entity variant definitions.</returns>
        public OperationResultCollection Process(EntityVariantDefinitionCollection entityVariantDefinitions, CallerContext callerContext)
        {
            Int32 result = 0;
            String loginUser = SecurityPrincipalHelper.GetCurrentSecurityPrincipal().CurrentUserName;
            OperationResultCollection operationResults = new OperationResultCollection();

            if (String.IsNullOrWhiteSpace(callerContext.ProgramName))
            {
                callerContext.ProgramName = "EntityVariantDefinitionBL";
            }

            #region Step : Diagnostic Activity initialization

            DiagnosticActivity diagnosticActivity = new DiagnosticActivity();

            String traceMessage = String.Empty;

            if (isTracingEnabled)
            {
                diagnosticActivity.Start();
                diagnosticActivity.LogInformation("Starting process internal execution flow for Entity Variant Definitions.");
            }

            #endregion

            try
            {

                #region Validation

                AttributeModelCollection attributeModels = new AttributeModelCollection();
                Object[] parameters = null;
                LocaleMessage localeMessage = null;

                foreach (EntityVariantDefinition entityVariantDefinition in entityVariantDefinitions)
                {
                    if (entityVariantDefinition.Action != ObjectAction.Delete && entityVariantDefinition.Action != ObjectAction.Read)
                    {
                        OperationResult operationResult = new OperationResult();
                        operationResult.ReferenceId = entityVariantDefinition.ReferenceId;

                        EntityVariantLevelCollection entityVariantLevels = entityVariantDefinition.EntityVariantLevels;

                        if (String.IsNullOrWhiteSpace(entityVariantDefinition.Name))
                        {
                            localeMessage = localeMessageManager.Get(_systemUILocale, "114168", false, callerContext);
                            operationResult.AddOperationResult(localeMessage.Code, localeMessage.Message, OperationResultType.Error);
                        }

                        if (entityVariantDefinition.RootEntityTypeId < 1)
                        {
                            parameters = new Object[] { entityVariantDefinition.Name };
                            localeMessage = localeMessageManager.Get(_systemUILocale, "114169", parameters, false, callerContext);
                            operationResult.AddOperationResult(localeMessage.Code, localeMessage.Message, parameters, OperationResultType.Error);
                        }

                        if (entityVariantLevels.Count() < 1)
                        {
                            parameters = new Object[] { entityVariantDefinition.Name };
                            localeMessage = localeMessageManager.Get(_systemUILocale, "114459", parameters, false, callerContext); // Unable to process entity variant definition '{0}', as it has no levels. Entity variant definition must have at least one entity variant level to process.
                            operationResult.AddOperationResult(localeMessage.Code, localeMessage.Message, parameters, OperationResultType.Error);
                        }

                        if (entityVariantLevels.Count() > 0)
                        {
                            Dictionary<Int32, Int32> entityTypeMappedToLevels = new Dictionary<Int32, Int32>(); // Key : rank, Value : entityType id
                            Collection<Int32> levelRanks = new Collection<Int32>();
                            Dictionary<Int32, Int32> uniqueAttributesPair = new Dictionary<Int32, Int32>();

                            foreach (EntityVariantLevel entityVariantLevel in entityVariantLevels)
                            {
                                if (entityVariantLevel.Rank < 1)
                                {
                                    parameters = new Object[] { entityVariantDefinition.Name };
                                    localeMessage = localeMessageManager.Get(_systemUILocale, "114173", parameters, false, callerContext);
                                    operationResult.AddOperationResult(localeMessage.Code, localeMessage.Message, parameters, OperationResultType.Error);
                                }
                                
                                if (entityVariantLevel.EntityTypeId < 1)
                                {
                                    parameters = new Object[] { entityVariantDefinition.Name };
                                    localeMessage = localeMessageManager.Get(_systemUILocale, "114171", parameters, false, callerContext);
                                    operationResult.AddOperationResult(localeMessage.Code, localeMessage.Message, parameters, OperationResultType.Error);
                                }
                                else
                                {
                                    if (entityTypeMappedToLevels.ContainsValue(entityVariantLevel.EntityTypeId))
                                    {
                                        parameters = new Object[] { entityVariantDefinition.Name, entityVariantLevel.EntityTypeName };
                                        localeMessage = localeMessageManager.Get(_systemUILocale, "114170", parameters, false, callerContext);
                                        operationResult.AddOperationResult(localeMessage.Code, localeMessage.Message, parameters, OperationResultType.Error);
                                    }
                                    else
                                    {
                                        Int32 entityTypeId = 0;
                                        entityTypeMappedToLevels.TryGetValue(entityVariantLevel.Rank, out entityTypeId);
                                        if (entityTypeId != 0) // i.e. at this level, already one entity type is mapped. So we should restrict current entity type not to be added at the same level
                                        {
                                            parameters = new Object[] { entityVariantDefinition.Name, entityVariantLevel.Rank };
                                            localeMessage = localeMessageManager.Get(_systemUILocale, "114475", parameters, false, callerContext); // Unable to process Entity Variant Definition '{0}', as it has different entity types mapped at same entity variant level {1}.
                                            operationResult.AddOperationResult(localeMessage.Code, localeMessage.Message, parameters, OperationResultType.Error);
                                        }
                                        else
                                        {
                                            // add it into the dictionary for level as for this level, there is no entity type exist in the dictionary yet.
                                            entityTypeMappedToLevels.Add(entityVariantLevel.Rank, entityVariantLevel.EntityTypeId);
                                        }
                                    }
                                }

                                if (entityVariantLevel.RuleAttributes.Count() == 1)
                                {
                                    if (entityVariantLevel.RuleAttributes.FirstOrDefault().IsOptional)
                                    {
                                        parameters = new Object[] { entityVariantDefinition.Name, entityVariantLevel.Rank };
                                        localeMessage = localeMessageManager.Get(_systemUILocale, "114432", parameters, false, callerContext); //Unable to process entity variant definition '{0}', as there is only one attribute at level {1} that is marked as optional. You must set IsOptional to false if you have only one attribute at a entity variant level.
                                        operationResult.AddOperationResult(localeMessage.Code, localeMessage.Message, parameters, OperationResultType.Error);
                                    }
                                }

                                if (entityVariantDefinition.HasDimensionAttributes && entityVariantLevel.RuleAttributes.Count() > 0)
                                {
                                    foreach (EntityVariantRuleAttribute entityVariantRuleAttribute in entityVariantLevel.RuleAttributes)
                                    {
                                        AttributeModel sourceAttributeModel = null;
                                        AttributeModel targetAttributeModel = null;

                                        if (entityVariantRuleAttribute.SourceAttributeId < 1)
                                        {
                                            parameters = new Object[] { entityVariantRuleAttribute.SourceAttributeName, entityVariantDefinition.Name, entityVariantLevel.EntityTypeName };
                                            localeMessage = localeMessageManager.Get(_systemUILocale, "114177", parameters, false, callerContext);//Source attribute: {0} is not specified or does not exists in the system for entity variant definition: {1} and child entity type: {2}.
                                            operationResult.AddOperationResult(localeMessage.Code, localeMessage.Message, parameters, OperationResultType.Error);
                                        }
                                        else
                                        {
                                            if (uniqueAttributesPair.ContainsKey(entityVariantRuleAttribute.SourceAttributeId))
                                            {
                                                parameters = new Object[] { entityVariantDefinition.Name };
                                                localeMessage = localeMessageManager.Get(_systemUILocale, "114460", parameters, false, callerContext); // Unable to process entity variant definition '{0}', as it has multiple occurrences for source attribute.
                                                operationResult.AddOperationResult(localeMessage.Code, localeMessage.Message, parameters, OperationResultType.Error);
                                            }

                                            sourceAttributeModel = DataModelHelper.GetAttributeModel(_iAttributeModelManager, entityVariantRuleAttribute.SourceAttributeId, entityVariantRuleAttribute.SourceAttributeName, String.Empty, ref attributeModels, callerContext);

                                            if (sourceAttributeModel != null && (!sourceAttributeModel.IsCollection || sourceAttributeModel.IsComplex || sourceAttributeModel.IsHierarchical || sourceAttributeModel.IsComplexChild || sourceAttributeModel.IsHierarchicalChild))
                                            {
                                                parameters = new Object[] { entityVariantRuleAttribute.SourceAttributeName, entityVariantDefinition.Name, entityVariantLevel.EntityTypeName };
                                                localeMessage = localeMessageManager.Get(_systemUILocale, "114172", parameters, false, callerContext);
                                                operationResult.AddOperationResult(localeMessage.Code, localeMessage.Message, parameters, OperationResultType.Error);
                                            }
                                        }

                                        if (entityVariantRuleAttribute.TargetAttributeId < 1)
                                        {
                                            parameters = new Object[] { entityVariantRuleAttribute.SourceAttributeName, entityVariantDefinition.Name, entityVariantLevel.EntityTypeName };
                                            localeMessage = localeMessageManager.Get(_systemUILocale, "114174", parameters, false, callerContext);
                                            operationResult.AddOperationResult(localeMessage.Code, localeMessage.Message, parameters, OperationResultType.Error);
                                        }
                                        else
                                        {
                                            if (uniqueAttributesPair.ContainsValue(entityVariantRuleAttribute.TargetAttributeId))
                                            {
                                                parameters = new Object[] { entityVariantDefinition.Name };
                                                localeMessage = localeMessageManager.Get(_systemUILocale, "114462", parameters, false, callerContext); // Unable to process entity variant definition '{0}', as it has multiple occurrences for target attribute.
                                                operationResult.AddOperationResult(localeMessage.Code, localeMessage.Message, parameters, OperationResultType.Error);
                                            }

                                            targetAttributeModel = DataModelHelper.GetAttributeModel(_iAttributeModelManager, entityVariantRuleAttribute.TargetAttributeId, entityVariantRuleAttribute.TargetAttributeName, String.Empty, ref attributeModels, callerContext);

                                            if (targetAttributeModel != null && (targetAttributeModel.IsCollection || targetAttributeModel.IsComplex || targetAttributeModel.IsHierarchical || targetAttributeModel.IsComplexChild || targetAttributeModel.IsHierarchicalChild))
                                            {
                                                parameters = new Object[] { entityVariantRuleAttribute.SourceAttributeName, entityVariantDefinition.Name, entityVariantLevel.EntityTypeName };
                                                localeMessage = localeMessageManager.Get(_systemUILocale, "114175", parameters, false, callerContext);
                                                operationResult.AddOperationResult(localeMessage.Code, localeMessage.Message, parameters, OperationResultType.Error);
                                            }
                                        }

                                        if (sourceAttributeModel != null && targetAttributeModel != null)
                                        {
                                            if (String.Compare(sourceAttributeModel.AttributeDataTypeName, targetAttributeModel.AttributeDataTypeName) != 0 ||
                                                sourceAttributeModel.IsLookup != targetAttributeModel.IsLookup)
                                            {
                                                parameters = new Object[] { entityVariantDefinition.Name, entityVariantLevel.EntityTypeName };
                                                localeMessage = localeMessageManager.Get(_systemUILocale, "114583", parameters, false, callerContext);
                                                operationResult.AddOperationResult(localeMessage.Code, localeMessage.Message, parameters, OperationResultType.Error);
                                            }
                                        }

                                        // Add attributes to unique dictionary only if both of them make a unique pair together
                                        if (entityVariantRuleAttribute.SourceAttributeId > 0 && entityVariantRuleAttribute.TargetAttributeId > 0 &&
                                            !uniqueAttributesPair.ContainsKey(entityVariantRuleAttribute.SourceAttributeId) && !uniqueAttributesPair.ContainsValue(entityVariantRuleAttribute.TargetAttributeId))
                                        {
                                            uniqueAttributesPair.Add(entityVariantRuleAttribute.SourceAttributeId, entityVariantRuleAttribute.TargetAttributeId);
                                        }
                                    }
                                }

                                if (!levelRanks.Contains(entityVariantLevel.Rank))
                                {
                                    levelRanks.Add(entityVariantLevel.Rank);
                                }
                            }

                            // Validate levels' sequence
                            ValidateEntityVariantLevelsSequence(levelRanks, entityVariantDefinition, operationResult, callerContext);
                        }

                        operationResults.Add(operationResult);
                    }
                }

                #endregion Validation

                #region Step: Diagnostics and tracing

                if (isTracingEnabled)
                {
                    diagnosticActivity.LogDurationInfo("Completed validation for entity variation definition.");
                }

                #endregion

                #region scan and filter entity variant definition based on operation result

                Collection<String> referenceIds = new Collection<String>();

                foreach (OperationResult item in operationResults)
                {
                    if (item.HasError)
                    {
                        referenceIds.Add(item.ReferenceId);
                    }
                }

                if (referenceIds.Count > 0)
                {
                    entityVariantDefinitions.Remove(referenceIds);
                }

                #endregion scan and filter entity variant definition based on operation result

                if (entityVariantDefinitions.Count > 0)
                {
                    DBCommandProperties command = DBCommandHelper.Get(callerContext, MDMCenterModuleAction.Execute);
                    EntityVariantDefinitionDA entityVariantDefinitionDA = new EntityVariantDefinitionDA();
                    entityVariantDefinitionDA.Process(entityVariantDefinitions, operationResults, command, loginUser, callerContext.ProgramName);

                    InvalidateCache();
                }

            }
            finally
            {
                #region Step: Final Diagnostics and tracing

                if (isTracingEnabled)
                {
                    diagnosticActivity.LogInformation(String.Format("Completed process for entity variant definition with result {0}", result.ToString()));
                    diagnosticActivity.Stop();
                }

                #endregion
            }

            return operationResults;
        }

        /// <summary>
        /// Process the entity variant definition based on provided application context.
        /// </summary>
        /// <param name="entityVariantDefinition">Indicates the entity variant definition to be processed</param>
        /// <param name="callerContext">Indicates the caller context specifying the caller application and module</param>
        /// <returns>Returns operation result</returns>
        public OperationResultCollection Process(EntityVariantDefinition entityVariantDefinition, CallerContext callerContext)
        {
            EntityVariantDefinitionCollection entityVariantDefinitions = new EntityVariantDefinitionCollection();
            entityVariantDefinitions.Add(entityVariantDefinition);
            return this.Process(entityVariantDefinitions, callerContext);
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

            EntityVariantDefinitionCollection entityVariantDefinitions = iDataModelObjects as EntityVariantDefinitionCollection;
            CallerContext callerContext = iCallerContext as CallerContext;

            #region Step : Diagnostic Activity initialization

            DiagnosticActivity diagnosticActivity = new DiagnosticActivity();

            String traceMessage = String.Empty;

            if (isTracingEnabled)
            {
                diagnosticActivity.Start();
                diagnosticActivity.LogInformation("Starting fill data model internal execution flow for Entity Variant Definitions.");
            }

            #endregion

            try
            {
                FillEntityVariantDefinition(entityVariantDefinitions, callerContext);
            }
            finally
            {
                #region Step: Final Diagnostics and tracing

                if (isTracingEnabled)
                {
                    diagnosticActivity.LogMessageWithData("See 'View Data' for Entity Variant Definitions after FillDataModel.", entityVariantDefinitions.ToXml());
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
            EntityVariantDefinitionCollection entityVariantDefinitions = iDataModelObjects as EntityVariantDefinitionCollection;
            CallerContext callerContext = iCallerContext as CallerContext;

            #region Step : Diagnostic Activity initialization

            DiagnosticActivity diagnosticActivity = new DiagnosticActivity();

            String traceMessage = String.Empty;

            if (isTracingEnabled)
            {
                diagnosticActivity.Start();
                diagnosticActivity.LogInformation("Starting validate internal execution flow for Entity Variant Definitions.");
            }

            #endregion

            try
            {
                ValidateInputParameters(entityVariantDefinitions, operationResults, callerContext);
                ValidateEntityVariantDefinitionsProperties(entityVariantDefinitions, operationResults, callerContext);
            }
            finally
            {
                #region Step: Final Diagnostics and tracing

                if (isTracingEnabled)
                {
                    diagnosticActivity.LogMessageWithData("See 'View Data' for Entity Variant Definitions after Validate.", operationResults.ToXml());
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
            EntityVariantDefinitionCollection entityVariantDefinitions = iDataModelObjects as EntityVariantDefinitionCollection;
            CallerContext callerContext = iCallerContext as CallerContext;

            #region Step : Diagnostic Activity initialization

            DiagnosticActivity diagnosticActivity = new DiagnosticActivity();

            String traceMessage = String.Empty;

            if (isTracingEnabled)
            {
                diagnosticActivity.Start();
                diagnosticActivity.LogInformation("Starting load original internal execution flow for Entity Variant Definitions.");
            }

            #endregion

            try
            {
                LoadOriginalEntityVariantDefinitions(entityVariantDefinitions, callerContext);
            }
            finally
            {
                #region Step: Final Diagnostics and tracing

                if (isTracingEnabled)
                {
                    diagnosticActivity.LogMessageWithData("See 'View Data' for Entity Variant Definitions after LoadOriginal.", entityVariantDefinitions.ToXml());
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
            EntityVariantDefinitionCollection entityVariantDefinitions = iDataModelObjects as EntityVariantDefinitionCollection;
            CallerContext callerContext = iCallerContext as CallerContext;

            #region Step : Diagnostic Activity initialization

            DiagnosticActivity diagnosticActivity = new DiagnosticActivity();

            String traceMessage = String.Empty;

            if (isTracingEnabled)
            {
                diagnosticActivity.Start();
                diagnosticActivity.LogInformation("Starting compare and merge internal execution flow for Entity Variant Definitions.");
            }

            #endregion

            try
            {
                CompareAndMerge(entityVariantDefinitions, operationResult, callerContext);
            }
            finally
            {
                #region Step: Final Diagnostics and tracing

                if (isTracingEnabled)
                {
                    diagnosticActivity.LogMessageWithData("See 'View Data' for Entity Variant Definitions after CompareAndMerge.", entityVariantDefinitions.ToXml());
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
        public void Process(IDataModelObjectCollection iDataModelObjects, BusinessObjects.DataModel.DataModelOperationResultCollection operationResults, ICallerContext iCallerContext)
        {
            EntityVariantDefinitionCollection entityVariantDefinitions = iDataModelObjects as EntityVariantDefinitionCollection;
            CallerContext callerContext = iCallerContext as CallerContext;
            String loginUser = SecurityPrincipalHelper.GetCurrentSecurityPrincipal().CurrentUserName;

            #region Step : Diagnostic Activity initialization

            DiagnosticActivity diagnosticActivity = new DiagnosticActivity();

            String traceMessage = String.Empty;

            if (isTracingEnabled)
            {
                diagnosticActivity.Start();
                diagnosticActivity.LogInformation("Starting process internal execution flow for Entity Variant Definitions.");
            }

            #endregion

            try
            {
                DBCommandProperties command = DBCommandHelper.Get(callerContext, MDMCenterModuleAction.Execute);
                EntityVariantDefinitionDA entityVariantDefinitionDA = new EntityVariantDefinitionDA();
                entityVariantDefinitionDA.Process(entityVariantDefinitions, operationResults, command, loginUser, callerContext.ProgramName);
            }
            finally
            {
                #region Step: Final Diagnostics and tracing

                if (isTracingEnabled)
                {
                    diagnosticActivity.LogMessageWithData("See 'View Data' for Entity Variant Definitions operation results after Process.", operationResults.ToXml());
                    diagnosticActivity.Stop();
                }

                #endregion
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

            if (isTracingEnabled)
            {
                diagnosticActivity.Start();
                diagnosticActivity.LogInformation("Starting prepare operation results schema internal execution flow for Entity Variant Definitions.");
            }

            #endregion

            try
            {
                EntityVariantDefinitionCollection entityVariantDefinitions = iDataModelObjects as EntityVariantDefinitionCollection;

                if (entityVariantDefinitions != null && entityVariantDefinitions.Count > 0)
                {
                    if (operationResults.Count > 0)
                    {
                        operationResults.Clear();
                    }

                    int entityVariantDefinitionIdToBeCreated = -1;

                    foreach (EntityVariantDefinition entityVariantDefinition in entityVariantDefinitions)
                    {
                        DataModelOperationResult EntityVariantDefinitioinOperationResult = new DataModelOperationResult(entityVariantDefinition.Id, entityVariantDefinition.Name, entityVariantDefinition.ExternalId, entityVariantDefinition.ReferenceId);

                        if (String.IsNullOrEmpty(EntityVariantDefinitioinOperationResult.ExternalId))
                        {
                            EntityVariantDefinitioinOperationResult.ExternalId = entityVariantDefinition.Name;
                        }

                        if (entityVariantDefinition.Id < 1)
                        {
                            entityVariantDefinition.Id = entityVariantDefinitionIdToBeCreated;
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

                if (isTracingEnabled)
                {
                    diagnosticActivity.LogMessageWithData("See 'View Data' for Entity Variant Definitions operation results after PrepareOperationResultsSchema.", operationResults.ToXml());
                    diagnosticActivity.Stop();
                }

                #endregion
            }
        }

        #endregion IDataModelManager Methods

        #region Private Methods

        private void FillEntityVariantDefinition(EntityVariantDefinitionCollection entityVariantDefinitions, CallerContext callerContext)
        {
            EntityTypeBL entityTypeManager = new EntityTypeBL();
            AttributeModelCollection attributeModels = new AttributeModelCollection();

            foreach (EntityVariantDefinition entityVariantDefinition in entityVariantDefinitions)
            {
                if (entityVariantDefinition.OriginalEntityVariantDefinition != null)
                {
                    entityVariantDefinition.Id = entityVariantDefinition.OriginalEntityVariantDefinition.Id;
                }

                EntityType rootEntityType = entityTypeManager.GetByNames(new Collection<String>() { entityVariantDefinition.RootEntityTypeName }, callerContext).FirstOrDefault();
                if (rootEntityType != null)
                {
                    entityVariantDefinition.RootEntityTypeId = rootEntityType.Id;
                }

                foreach (EntityVariantLevel entityVariantLevel in entityVariantDefinition.EntityVariantLevels)
                {
                    EntityType entityVariantLevelEntityType = entityTypeManager.GetByNames(new Collection<String>() { entityVariantLevel.EntityTypeName }, callerContext).FirstOrDefault();
                    if (entityVariantLevelEntityType != null)
                    {
                        entityVariantLevel.EntityTypeId = entityVariantLevelEntityType.Id;
                    }

                    FillEntityVariantRuleAttributes(entityVariantLevel.RuleAttributes, attributeModels, callerContext);
                }

            }
        }

        private void FillEntityVariantRuleAttributes(EntityVariantRuleAttributeCollection entityVariantRuleAttributes, AttributeModelCollection attributeModels, CallerContext callerContext)
        {
            if (entityVariantRuleAttributes != null && entityVariantRuleAttributes.Count() > 0)
            {
                foreach (EntityVariantRuleAttribute entityVariantRuleAttribute in entityVariantRuleAttributes)
                {
                    AttributeModel sourceAttributeModel = DataModelHelper.GetAttributeModel(_iAttributeModelManager, entityVariantRuleAttribute.SourceAttributeId, entityVariantRuleAttribute.SourceAttributeName, String.Empty, ref attributeModels, callerContext);
                    AttributeModel targetAttributeModel = DataModelHelper.GetAttributeModel(_iAttributeModelManager, entityVariantRuleAttribute.TargetAttributeId, entityVariantRuleAttribute.TargetAttributeName, String.Empty, ref attributeModels, callerContext);

                    if (sourceAttributeModel != null)
                    {
                        entityVariantRuleAttribute.SourceAttributeId = sourceAttributeModel.Id;
                        entityVariantRuleAttribute.SourceAttributeLongName = sourceAttributeModel.LongName;
                        entityVariantRuleAttribute.SourceAttributeName = sourceAttributeModel.Name;
                    }

                    if (targetAttributeModel != null)
                    {
                        entityVariantRuleAttribute.TargetAttributeId = targetAttributeModel.Id;
                        entityVariantRuleAttribute.TargetAttributeLongName = targetAttributeModel.LongName;
                        entityVariantRuleAttribute.TargetAttributeName = targetAttributeModel.Name;
                    }
                }
            }
        }

        private void ValidateInputParameters(EntityVariantDefinitionCollection entityVariantDefinitions, DataModelOperationResultCollection operationResults, CallerContext callerContext)
        {
            if (entityVariantDefinitions != null && entityVariantDefinitions.Count() == 0)
            {
                DataModelHelper.AddOperationResults(operationResults, "114176", String.Empty, null, OperationResultType.Error, TraceEventType.Error, callerContext);
            }

            if (callerContext == null)
            {
                DataModelHelper.AddOperationResults(operationResults, "111846", String.Empty, null, OperationResultType.Error, TraceEventType.Error, callerContext);
            }
        }

        private void ValidateEntityVariantDefinitionsProperties(EntityVariantDefinitionCollection entityVariantDefinitions, DataModelOperationResultCollection operationResults, CallerContext callerContext)
        {
            EntityVariantDefinitionCollection uniqueEntityVariantDefinitions = new EntityVariantDefinitionCollection();
            Object[] parameters = null;

            foreach (EntityVariantDefinition entityVariantDefinition in entityVariantDefinitions)
            {
                IDataModelOperationResult operationResult = operationResults.GetByReferenceId(entityVariantDefinition.ReferenceId);

                if (entityVariantDefinition.Action == ObjectAction.Read || entityVariantDefinition.Action == ObjectAction.Delete
                    || entityVariantDefinition.Action == ObjectAction.Ignore)
                {
                    continue;
                }

                if (String.IsNullOrWhiteSpace(entityVariantDefinition.Name))
                {
                    DataModelHelper.AddOperationResult(operationResult, "114178", String.Empty, null, OperationResultType.Error, TraceEventType.Error, callerContext);
                }

                if (String.IsNullOrWhiteSpace(entityVariantDefinition.RootEntityTypeName))
                {
                    parameters = new Object[] { entityVariantDefinition.Name };
                    DataModelHelper.AddOperationResult(operationResult, "114180", String.Empty, parameters, OperationResultType.Error, TraceEventType.Error, callerContext);
                }

                EntityVariantDefinition duplicateEntityVariantDefinition = uniqueEntityVariantDefinitions.GetByName(entityVariantDefinition.Name);

                if (duplicateEntityVariantDefinition != null)
                {
                    parameters = new Object[] { entityVariantDefinition.Name };
                    DataModelHelper.AddOperationResult(operationResult, "114181", String.Empty, parameters, OperationResultType.Error, TraceEventType.Error, callerContext);
                }
                else
                {
                    uniqueEntityVariantDefinitions.Add(entityVariantDefinition);
                }

                EntityVariantLevelCollection entityVariantLevels = entityVariantDefinition.EntityVariantLevels;

                if (entityVariantLevels.Count() < 1)
                {
                    parameters = new Object[] { entityVariantDefinition.Name };
                    DataModelHelper.AddOperationResult(operationResult, "114459", String.Empty, parameters, OperationResultType.Error, TraceEventType.Error, callerContext); // Unable to process entity variant definition '{0}', as it has no levels. Entity variant definition must have at least one entity variant level to process.
                }

                if (entityVariantLevels.Count() > 0)
                {
                    Collection<Int32> levelRanks = new Collection<Int32>();
                    Dictionary<Int32, String> entityTypeMappedToLevels = new Dictionary<Int32, String>();

                    foreach (EntityVariantLevel entityVariantLevel in entityVariantLevels)
                    {
                        if (entityVariantLevel.Rank <= 0)
                        {
                            parameters = new Object[] { entityVariantDefinition.Name };
                            DataModelHelper.AddOperationResult(operationResult, "114182", String.Empty, parameters, OperationResultType.Error, TraceEventType.Error, callerContext);
                        }

                        if (String.IsNullOrWhiteSpace(entityVariantLevel.EntityTypeName))
                        {
                            parameters = new Object[] { entityVariantDefinition.Name };
                            DataModelHelper.AddOperationResult(operationResult, "114179", String.Empty, parameters, OperationResultType.Error, TraceEventType.Error, callerContext);
                        }
                        else
                        {
                            if (entityTypeMappedToLevels.ContainsValue(entityVariantLevel.EntityTypeName))
                            {
                                parameters = new Object[] { entityVariantDefinition.Name, entityVariantLevel.EntityTypeName };
                                DataModelHelper.AddOperationResult(operationResult, "114191", String.Empty, parameters, OperationResultType.Error, TraceEventType.Error, callerContext);
                            }
                            else
                            {
                                String entityTypeName = String.Empty;
                                entityTypeMappedToLevels.TryGetValue(entityVariantLevel.Rank, out entityTypeName);
                                if (!String.IsNullOrWhiteSpace(entityTypeName)) // i.e. at this level, already one entity type is mapped. So we should restrict current entity type not to be added at the same level
                                {
                                    parameters = new Object[] { entityVariantDefinition.Name, entityVariantLevel.Rank };
                                    DataModelHelper.AddOperationResult(operationResult, "114475", String.Empty, parameters, OperationResultType.Error, TraceEventType.Error, callerContext); // Unable to process Entity Variant Definition '{0}', as it has different entity types mapped at same entity variant level {1}.
                                }
                                else
                                {
                                    // add it into the dictionary for level as for this level, there is no entity type exist in the dictionary yet.
                                    entityTypeMappedToLevels.Add(entityVariantLevel.Rank, entityVariantLevel.EntityTypeName);
                                }
                            } 
                        }

                        if (entityVariantLevel.RuleAttributes.Count() == 1)
                        {
                            if (entityVariantLevel.RuleAttributes.FirstOrDefault().IsOptional)
                            {
                                parameters = new Object[] { entityVariantDefinition.Name, entityVariantLevel.Rank };
                                DataModelHelper.AddOperationResult(operationResult, "114432", String.Empty, parameters, OperationResultType.Error, TraceEventType.Error, callerContext); // Unable to process entity variant definition '{0}', as there is only one attribute at level {1} that is marked as optional. You must set IsOptional to false if you have only one attribute at a entity variant level.
                            }
                        }

                        if (entityVariantDefinition.HasDimensionAttributes)
                        {
                            if (entityVariantLevel.RuleAttributes.Count() > 0)
                            {
                                foreach (EntityVariantRuleAttribute entityVariantRuleAttribute in entityVariantLevel.RuleAttributes)
                                {
                                    String sourceAttributeName = entityVariantRuleAttribute.SourceAttributeName;
                                    String targetAttributeName = entityVariantRuleAttribute.TargetAttributeName;

                                    if (String.IsNullOrWhiteSpace(sourceAttributeName))
                                    {
                                        parameters = new Object[] { entityVariantDefinition.Name, entityVariantLevel.EntityTypeName };
                                        DataModelHelper.AddOperationResult(operationResult, "114183", String.Empty, parameters, OperationResultType.Error, TraceEventType.Error, callerContext);
                                    }

                                    if (String.IsNullOrWhiteSpace(targetAttributeName))
                                    {
                                        parameters = new Object[] { entityVariantDefinition.Name, entityVariantLevel.EntityTypeName };
                                        DataModelHelper.AddOperationResult(operationResult, "114184", String.Empty, parameters, OperationResultType.Error, TraceEventType.Error, callerContext);
                                    }

                                    if (String.Compare(sourceAttributeName, targetAttributeName) == 0)
                                    {
                                        parameters = new Object[] { entityVariantDefinition.Name, entityVariantLevel.EntityTypeName };
                                        DataModelHelper.AddOperationResult(operationResult, "114185", String.Empty, parameters, OperationResultType.Error, TraceEventType.Error, callerContext);
                                    }
                                }
                            }
                            else
                            {
                                parameters = new Object[] { entityVariantDefinition.Name, entityVariantLevel.EntityTypeName };
                                DataModelHelper.AddOperationResult(operationResult, "114186", String.Empty, parameters, OperationResultType.Error, TraceEventType.Error, callerContext);
                            }
                        }

                        if (!levelRanks.Contains(entityVariantLevel.Rank))
                        {
                            levelRanks.Add(entityVariantLevel.Rank);
                        }
                    }

                    // Validate levels' sequence
                    ValidateEntityVariantLevelsSequence(levelRanks, entityVariantDefinition, (OperationResult)operationResult, callerContext);
                }
            }
        }

        private void LoadOriginalEntityVariantDefinitions(EntityVariantDefinitionCollection entityVariantDefinitions, CallerContext callerContext)
        {
            if (entityVariantDefinitions != null && entityVariantDefinitions.Count() > 0)
            {
                EntityVariantDefinitionCollection allEntityVariantDefinitions = GetAll(callerContext);

                if (allEntityVariantDefinitions != null && allEntityVariantDefinitions.Count() > 0)
                {
                    foreach (EntityVariantDefinition entityVariantDefinition in entityVariantDefinitions)
                    {
                        entityVariantDefinition.OriginalEntityVariantDefinition = allEntityVariantDefinitions.GetByName(entityVariantDefinition.Name);
                    }
                }
            }
        }

        private void CompareAndMerge(EntityVariantDefinitionCollection entityVariantDefinitions, DataModelOperationResultCollection operationResults, CallerContext callerContext)
        {
            if (entityVariantDefinitions != null && entityVariantDefinitions.Count() > 0)
            {
                AttributeModelCollection attributeModels = new AttributeModelCollection();
                Object[] parameters = null;

                foreach (EntityVariantDefinition deltaEntityVariantDefinition in entityVariantDefinitions)
                {
                    IDataModelOperationResult operationResult = operationResults.GetByReferenceId(deltaEntityVariantDefinition.ReferenceId);
                    Dictionary<Int32, Int32> uniqueAttributesPair = null; 

                    if (deltaEntityVariantDefinition.Action == ObjectAction.Read || deltaEntityVariantDefinition.Action == ObjectAction.Ignore)
                    {
                        continue;
                    }

                    EntityVariantDefinition originalEntityVariantDefinition = deltaEntityVariantDefinition.OriginalEntityVariantDefinition;

                    if (originalEntityVariantDefinition != null)
                    {
                        if (deltaEntityVariantDefinition.Action != ObjectAction.Delete)
                        {
                            originalEntityVariantDefinition.MergeDelta(deltaEntityVariantDefinition, callerContext, true);
                        }
                    }
                    else
                    {
                        if (deltaEntityVariantDefinition.Action == ObjectAction.Delete)
                        {
                            parameters = new Object[] { deltaEntityVariantDefinition.Name };
                            DataModelHelper.AddOperationResult(operationResult, "114187", String.Empty, parameters, OperationResultType.Error, TraceEventType.Error, callerContext);
                        }
                        else
                        {
                            if (deltaEntityVariantDefinition.RootEntityTypeId < 1)
                            {
                                parameters = new Object[] { deltaEntityVariantDefinition.RootEntityTypeName, deltaEntityVariantDefinition.Name };
                                DataModelHelper.AddOperationResult(operationResult, "114188", String.Empty, parameters, OperationResultType.Error, TraceEventType.Error, callerContext);
                            }

                            uniqueAttributesPair = new Dictionary<Int32, Int32>();

                            if (deltaEntityVariantDefinition.EntityVariantLevels.Count() > 0)
                            {
                                foreach (EntityVariantLevel entityVariantLevel in deltaEntityVariantDefinition.EntityVariantLevels)
                                {
                                    if (entityVariantLevel.EntityTypeId < 1)
                                    {
                                        parameters = new Object[] { entityVariantLevel.EntityTypeName };
                                        DataModelHelper.AddOperationResult(operationResult, "114189", String.Empty, parameters, OperationResultType.Error, TraceEventType.Error, callerContext);
                                    }

                                    if (deltaEntityVariantDefinition.HasDimensionAttributes && entityVariantLevel.RuleAttributes.Count() > 0)
                                    {
                                        foreach (EntityVariantRuleAttribute entityVariantRuleAttribute in entityVariantLevel.RuleAttributes)
                                        {
                                            AttributeModel sourceAttributeModel = null;
                                            AttributeModel targetAttributeModel = null;

                                            if (entityVariantRuleAttribute.SourceAttributeId < 1)
                                            {
                                                parameters = new Object[] { entityVariantRuleAttribute.SourceAttributeName, deltaEntityVariantDefinition.Name, entityVariantLevel.EntityTypeName };
                                                DataModelHelper.AddOperationResult(operationResult, "114190", String.Empty, parameters, OperationResultType.Error, TraceEventType.Error, callerContext);
                                            }
                                            else
                                            {
                                                if (uniqueAttributesPair.ContainsKey(entityVariantRuleAttribute.SourceAttributeId))
                                                {
                                                    parameters = new Object[] { deltaEntityVariantDefinition.Name };
                                                    DataModelHelper.AddOperationResult(operationResult, "114460", String.Empty, parameters, OperationResultType.Error, TraceEventType.Error, callerContext); // Unable to process entity variant definition '{0}', as it has multiple occurrences for source attribute.
                                                }
                                                
                                                sourceAttributeModel = DataModelHelper.GetAttributeModel(_iAttributeModelManager, entityVariantRuleAttribute.SourceAttributeId, entityVariantRuleAttribute.SourceAttributeName, String.Empty, ref attributeModels, callerContext);

                                                if (sourceAttributeModel != null && (!sourceAttributeModel.IsCollection || sourceAttributeModel.IsComplex || sourceAttributeModel.IsHierarchical || sourceAttributeModel.IsComplexChild || sourceAttributeModel.IsHierarchicalChild))
                                                {
                                                    parameters = new Object[] { entityVariantRuleAttribute.SourceAttributeName, deltaEntityVariantDefinition.Name, entityVariantLevel.EntityTypeName };
                                                    DataModelHelper.AddOperationResult(operationResult, "114193", String.Empty, parameters, OperationResultType.Error, TraceEventType.Error, callerContext);
                                                }
                                            }

                                            if (entityVariantRuleAttribute.TargetAttributeId < 1)
                                            {
                                                parameters = new Object[] { entityVariantRuleAttribute.TargetAttributeName, deltaEntityVariantDefinition.Name, entityVariantLevel.EntityTypeName };
                                                DataModelHelper.AddOperationResult(operationResult, "114192", String.Empty, parameters, OperationResultType.Error, TraceEventType.Error, callerContext);
                                            }
                                            else
                                            {
                                                if (uniqueAttributesPair.ContainsValue(entityVariantRuleAttribute.TargetAttributeId))
                                                {
                                                    parameters = new Object[] { deltaEntityVariantDefinition.Name };
                                                    DataModelHelper.AddOperationResult(operationResult, "114462", String.Empty, parameters, OperationResultType.Error, TraceEventType.Error, callerContext); // Unable to process entity variant definition '{0}', as it has multiple occurrences for target attribute.
                                                }

                                                targetAttributeModel = DataModelHelper.GetAttributeModel(_iAttributeModelManager, entityVariantRuleAttribute.TargetAttributeId, entityVariantRuleAttribute.TargetAttributeName, String.Empty, ref attributeModels, callerContext);

                                                if (targetAttributeModel != null && (targetAttributeModel.IsCollection || targetAttributeModel.IsComplex || targetAttributeModel.IsHierarchical || targetAttributeModel.IsComplexChild || targetAttributeModel.IsHierarchicalChild))
                                                {
                                                    parameters = new Object[] { entityVariantRuleAttribute.TargetAttributeName, deltaEntityVariantDefinition.Name, entityVariantLevel.EntityTypeName };
                                                    DataModelHelper.AddOperationResult(operationResult, "114194", String.Empty, parameters, OperationResultType.Error, TraceEventType.Error, callerContext);
                                                }
                                            }

                                            if (sourceAttributeModel != null && targetAttributeModel != null)
                                            {
                                                if (String.Compare(sourceAttributeModel.AttributeDataTypeName, targetAttributeModel.AttributeDataTypeName) != 0 ||
                                                    sourceAttributeModel.IsLookup != targetAttributeModel.IsLookup)
                                                {
                                                    parameters = new Object[] { deltaEntityVariantDefinition.Name, entityVariantLevel.EntityTypeName };
                                                    DataModelHelper.AddOperationResult(operationResult, "114583", String.Empty, parameters, OperationResultType.Error, TraceEventType.Error, callerContext);
                                                }
                                            }

                                            // Add attributes to unique dictionary only if both of them make a unique pair together
                                            if (entityVariantRuleAttribute.SourceAttributeId > 0 && entityVariantRuleAttribute.TargetAttributeId > 0 &&
                                                !uniqueAttributesPair.ContainsKey(entityVariantRuleAttribute.SourceAttributeId) && !uniqueAttributesPair.ContainsValue(entityVariantRuleAttribute.TargetAttributeId))
                                            {
                                                uniqueAttributesPair.Add(entityVariantRuleAttribute.SourceAttributeId, entityVariantRuleAttribute.TargetAttributeId);
                                            }

                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private LocaleMessage GetSystemLocaleMessage(String messageCode, CallerContext callerContext)
        {
            return localeMessageManager.Get(_systemUILocale, messageCode, false, callerContext);
        }

        private void InvalidateCache()
        {
            String cacheKey = CacheKeyGenerator.GetAllEntityVariantDefinitions();
            var cacheBufferManager = new CacheBufferManager<EntityVariantDefinitionCollection>(cacheKey, String.Empty);
            cacheBufferManager.RemoveBusinessObjectFromCache(true, cacheKey);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="levelRanks"></param>
        /// <param name="entityVariantDefinition"></param>
        /// <param name="operationResult"></param>
        /// <param name="callerContext"></param>
        private void ValidateEntityVariantLevelsSequence(Collection<Int32> levelRanks, EntityVariantDefinition entityVariantDefinition, OperationResult operationResult, CallerContext callerContext)
        {
            if (levelRanks != null && levelRanks.Count > 0)
            {
                Object[] parameters = null;
                LocaleMessage localeMessage = null;
                IOrderedEnumerable<Int32> sortedLevels = levelRanks.OrderByDescending(item => item); // e.g 4,3,2,1

                if (!sortedLevels.Contains(Constants.VARIANT_LEVEL_ONE))
                {
                    parameters = new Object[] { entityVariantDefinition.Name };
                    localeMessage = localeMessageManager.Get(_systemUILocale, "114458", parameters, false, callerContext); // Unable to process entity variant definition '{0}'. Entity variant levels sequence must start from 1.
                    operationResult.AddOperationResult(localeMessage.Code, localeMessage.Message, parameters, OperationResultType.Error);
                }
                else
                {
                    foreach (Int32 currentLevel in sortedLevels)
                    {
                        Int32 parentLevel = currentLevel - 1;
                        if (parentLevel > 0 && !levelRanks.Contains(parentLevel))
                        {
                            parameters = new Object[] { entityVariantDefinition.Name, parentLevel, currentLevel };
                            localeMessage = localeMessageManager.Get(_systemUILocale, "114461", parameters, false, callerContext); // Unable to process entity variant definition '{0}', as parent level {1} is missing for child level {2}.
                            operationResult.AddOperationResult(localeMessage.Code, localeMessage.Message, parameters, OperationResultType.Error);
                            break;
                        }
                    }
                }
            }
        }

        #endregion

        #endregion Methods
    }
}