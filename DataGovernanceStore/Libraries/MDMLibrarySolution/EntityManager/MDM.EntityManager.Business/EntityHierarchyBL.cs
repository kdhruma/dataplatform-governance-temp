using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;

namespace MDM.EntityManager.Business
{
    using AttributeModelManager.Business;
    using BufferManager;
    using BusinessObjects;
    using BusinessObjects.Diagnostics;
    using CacheManager.Business;
    using ConfigurationManager.Business;
    using ContainerManager.Business;
    using Core;
    using Core.Exceptions;
    using Data;
    using DataModelManager.Business;
    using EntityOperations;
    using Interfaces;
    using LookupManager.Business;
    using MessageManager.Business;
    using Utility;

    /// <summary>
    /// Specifies business logic operations for Entity Hierarchy
    /// </summary>
    public class EntityHierarchyBL : BusinessLogicBase
    {
        #region Fields

        /// <summary>
        /// 
        /// </summary>
        private SecurityPrincipal _securityPrincipal;

        /// <summary>
        /// 
        /// </summary>
        private AttributeModelCollection _attributeModels = new AttributeModelCollection();

        /// <summary>
        /// 
        /// </summary>
        private ApplicationMessageBL messageBL = new ApplicationMessageBL();

        /// <summary>
        /// 
        /// </summary>
        private const String _additionalAttributeSuffix = "AddAttribute";

        /// <summary>
        /// 
        /// </summary>
        Entity _entity = new Entity();

        /// <summary>
        /// Field denoting locale Message
        /// </summary>
        private LocaleMessage _localeMessage = null;

        /// <summary>
        /// Field denoting localeMessageBL
        /// </summary>
        private LocaleMessageBL _localeMessageBL = new LocaleMessageBL();

        /// <summary>
        /// Whether dependency attribute feature is enabled or not. Which is decided by the 'MDMCenter.AttributeDependency.Enabled'appconfig key.
        /// </summary>
        private Boolean _isAttributeDependencyEnabled = false;

        /// <summary>
        /// Specifies the name currentTimeZone
        /// </summary>
        private String _currentTimeZone = String.Empty;

        /// <summary>
        /// Specifies the name System Time Zone
        /// </summary>
        private String _systemTimeZone = String.Empty;

        #endregion

        #region Properties

        private LocaleEnum SystemAttributeLocale
        {
            get
            {
                //TODO :: Is UserPreference.DataLocale a proper locale value?
                return GlobalizationHelper.GetSystemDataLocale();
            }
        }

        /// <summary>
        /// Property denoting CurrentTimeZone
        /// </summary>
        public String CurrentTimeZone
        {
            get
            {
                if (String.IsNullOrWhiteSpace(_currentTimeZone))
                {
                    if (_securityPrincipal != null && _securityPrincipal.UserPreferences != null && !String.IsNullOrWhiteSpace(_securityPrincipal.UserPreferences.DefaultTimeZoneShortName))
                        _currentTimeZone = _securityPrincipal.UserPreferences.DefaultTimeZoneShortName;

                }
                return _currentTimeZone;
            }
        }

        /// <summary>
        /// Property denoting SystemTimeZone
        /// </summary>
        public String SystemTimeZone
        {
            get
            {
                if (String.IsNullOrWhiteSpace(_systemTimeZone))
                {
                    _systemTimeZone = AppConfigurationHelper.GetAppConfig<String>("TimeZone.SystemTimeZone");
                }

                return _systemTimeZone;
            }
        }

        #endregion Properties

        #region Constructors

        /// <summary>
        /// Default Constructor that loads the security principal from Cache if present
        /// </summary>
        public EntityHierarchyBL()
        {
            GetSecurityPrincipal();

            _isAttributeDependencyEnabled = AppConfigurationHelper.GetAppConfig<Boolean>("MDMCenter.AttributeDependency.Enabled", false);
        }

        #endregion

        #region Methods

        #region Public Methods

        #region Hierarchy Rules

        /// <summary>
        /// Gets definition with dimension values for all levels of a definition based in entity id and definition id
        /// </summary>
        /// <param name="entityId">Indicates identifier of entity for which dimension values will be fetched</param>
        /// <param name="entityVariantDefinitionId">Indicates identifier of variant definition for which the dimension values will be fetched</param>
        /// <returns>Returns entity variant definition object with model of variant and dimension values</returns>
        public EntityVariantDefinition GetHierarchyDimensionValues(Int64 entityId, Int32 entityVariantDefinitionId)
        {

            EntityVariantDefinition definition = new EntityVariantDefinition();

            EntityVariantDefinitionBL entityVariantDefinitionBL = new EntityVariantDefinitionBL(new AttributeModelBL());
            // TODO: Use Caller Context, not null
            definition = entityVariantDefinitionBL.GetById(entityVariantDefinitionId, null);

            EntityHierarchyDA entityHierarchyDataManager = new EntityHierarchyDA();
            Table table = entityHierarchyDataManager.GetDimensionValues(entityId, entityVariantDefinitionId, this.SystemAttributeLocale);

            foreach (EntityVariantLevel level in definition.EntityVariantLevels)
            {
                FillHierarchyLevelDimensionValues(level, table);
            }

            return definition;
        }

        /// <summary>
        /// Gets dimension values for all levels of given definition for given entity id
        /// </summary>
        /// <param name="entityId">Id of entity for which dimension values will be fetched</param>
        /// <param name="entityHierarchyDefinition">Hierarchy Definition for which the dimension values will be fetched</param>
        /// <returns>EntityHierarchyDefinition object with all dimension values filled in</returns>
        public EntityVariantDefinition GetHierarchyDimensionValues(Int64 entityId, EntityVariantDefinition entityHierarchyDefinition)
        {
            TraceSettings traceSettings = MDMOperationContextHelper.GetCurrentTraceSettings();
            DiagnosticActivity currentActivity = new DiagnosticActivity();

            if (traceSettings.IsBasicTracingEnabled)
                currentActivity.Start();

            try
            {

                EntityHierarchyDA entityHierarchyDataManager = new EntityHierarchyDA();
                Table table = entityHierarchyDataManager.GetDimensionValues(entityId, entityHierarchyDefinition.Id, this.SystemAttributeLocale);

                if (traceSettings.IsBasicTracingEnabled)
                    currentActivity.LogInformation("Loop: Fetching hierarchy level dimension values : Count : " + entityHierarchyDefinition.EntityVariantLevels.Count());
                DurationHelper durHelper = new DurationHelper(DateTime.Now);

                foreach (EntityVariantLevel level in entityHierarchyDefinition.EntityVariantLevels)
                {
                    FillHierarchyLevelDimensionValues(level, table);
                }

                if (traceSettings.IsBasicTracingEnabled)
                    currentActivity.LogMessageWithDuration(MessageClassEnum.Information, null, "Loop: Fetching hierarchy level dimension values : Finished", durHelper.GetCumulativeTimeSpanInMilliseconds());
            }
            catch (Exception e)
            {
                currentActivity.LogError(e.Message);
                throw e;
            }
            finally
            {
                if (traceSettings.IsBasicTracingEnabled)
                    currentActivity.Stop();
            }

            return entityHierarchyDefinition;
        }

        /// <summary>
        /// Gets definition and its dimension values based on context and for an entity
        /// </summary>
        /// <param name="entityId">Id of entity for which dimension values will be fetched</param>
        /// <param name="containerId">Id of Container for context</param>
        /// <param name="entityTypeId">Id of Entity Type for context</param>
        /// <param name="categoryId">Id of Category for context</param>
        /// <returns>EntityHierarchyDefinition object with all dimension values filled in</returns>
        public EntityVariantDefinition GetHierarchyDimensionValues(Int64 entityId, Int32 containerId, Int32 entityTypeId, Int64 categoryId)
        {
            EntityVariantDefinition definition = new EntityVariantDefinition();

            EntityVariantDefinitionBL defBL = new EntityVariantDefinitionBL(new AttributeModelBL());
            CallerContext callerContext = new CallerContext(MDMCenterApplication.PIM, MDMCenterModules.Entity, "GetHierarchyDimensionValues");
            definition = defBL.GetByContext(containerId, categoryId, entityTypeId, callerContext);

            if (definition != null)
            {
                EntityHierarchyDA entityHierarchyDataManager = new EntityHierarchyDA();
                Table table = entityHierarchyDataManager.GetDimensionValues(entityId, definition.Id, this.SystemAttributeLocale);

                foreach (EntityVariantLevel level in definition.EntityVariantLevels)
                {
                    FillHierarchyLevelDimensionValues(level, table);
                }
            }
            return definition;
        }

        /// <summary>
        /// Saves dimension values for a definition for an entity
        /// </summary>
        /// <param name="entityId">Id of Entity for which dimension values will be saved</param>
        /// <param name="definition">EntityHierarchyDefinition which holds dimension values for each level within</param>
        /// <param name="context">Caller context</param>
        /// <returns>True for now, need to change</returns>
        public Boolean ProcessHierarchyGenerationRules(Int64 entityId, EntityVariantDefinition definition, CallerContext context = null)
        {
            TraceSettings traceSettings = MDMOperationContextHelper.GetCurrentTraceSettings();

            DiagnosticActivity currentActivity = new DiagnosticActivity();

            if (traceSettings.IsTracingEnabled)
            {
                currentActivity.Start();
            }

            try
            {
                EntityHierarchyDA entityHierarchyDA = new EntityHierarchyDA();
                String userName = SecurityPrincipalHelper.GetCurrentSecurityPrincipal().CurrentUserName;
                DBCommandProperties command = DBCommandHelper.Get(MDMCenterApplication.PIM, MDMCenterModules.Entity, MDMCenterModuleAction.Update);

                if (traceSettings.IsBasicTracingEnabled)
                {
                    currentActivity.LogInformation("Calling EntityHierarchyDA.ProcessDimensionValues()");
                }

                Int32 result = entityHierarchyDA.ProcessDimensionValues(entityId, definition, userName, "EntityManager.EntityHierarchyBL.ProcessHierarchyGenerationRules", command);

                if (result <= 0)
                    return false;

            }
            finally
            {
                if (traceSettings.IsTracingEnabled)
                    currentActivity.Stop();
            }

            return true;
        }

        /// <summary>
        /// Calculates dimensions based on entity hierarchy level and rule attributes
        /// </summary>
        /// <param name="attributes">Indicates rule attributes</param>
        /// <param name="entityHierarchyLevel">Indicates entity hierarchy level</param>
        /// <returns>Returns Table having dimensions calculated</returns>
        public Table CalculateDimensions(EntityVariantRuleAttributeCollection attributes, EntityVariantLevel entityHierarchyLevel)
        {
            ValidateEntityHierarchyLevel(entityHierarchyLevel);
            return CalculateDimensionsByRuleAttributesAndEntityHierarchyLevel(attributes, entityHierarchyLevel);
        }

        /// <summary>
        /// Calculates dimensions based on specified entity hierarchy level
        /// </summary>
        /// <param name="entityHierarchyLevel">Indicates entity hierarchy level</param>
        /// <param name="context">Indicates caller context, including aplication and module name who invoked the method</param>
        /// <returns>Retrns table having dimensions calculated</returns>
        public Table CalculateDimensions(EntityVariantLevel entityHierarchyLevel, CallerContext context = null)
        {
            ValidateEntityHierarchyLevel(entityHierarchyLevel, context);
            EntityVariantRuleAttributeCollection ruleAttributes = entityHierarchyLevel.RuleAttributes;
            return CalculateDimensionsByRuleAttributesAndEntityHierarchyLevel(ruleAttributes, entityHierarchyLevel, context);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityHierarchyDefinition"></param>
        /// <returns></returns>
        public EntityVariantDefinition CalculateDimensions(EntityVariantDefinition entityHierarchyDefinition)
        {
            foreach (EntityVariantLevel entityHierarchyLevel in entityHierarchyDefinition.EntityVariantLevels)
            {
                EntityVariantRuleAttributeCollection attributes = entityHierarchyLevel.RuleAttributes;
                entityHierarchyLevel.DimensionValues = CalculateDimensionsCartesian(attributes, entityHierarchyLevel);
            }

            return entityHierarchyDefinition;
        }

        #endregion Hierarchy Rules

        #region Hierarchy Matrix

        /// <summary>
        /// Calculates hierarchy matrix from level dimension values for an entity based on a definition
        /// </summary>
        /// <param name="entityHierarchyDefinition">Hierarchy definition with dimension values at each level</param>
        /// <param name="operationResult">Operation result</param>
        /// <returns></returns>
        public Table CalculateMatrix(EntityVariantDefinition entityHierarchyDefinition)
        {
            return CalculateMatrixCartesian(entityHierarchyDefinition);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ehdef"></param>
        /// <param name="or"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public Table CalculateMatrixExtended(EntityVariantDefinition ehdef, CallerContext context)
        {
            Table returnTable = null;

            returnTable = CalculateMatrixCartesian(ehdef, context);

            return returnTable;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityId"></param>
        /// <param name="entityHierarchyDefinition"></param>
        /// <param name="operationResult"></param>
        /// <param name="application"></param>
        /// <param name="module"></param>
        /// <returns></returns>
        public Table GetHierarchyMatrix(Int64 entityId, EntityVariantDefinition entityHierarchyDefinition, OperationResult operationResult, MDMCenterApplication application, MDMCenterModules module)
        {
            Boolean calculateStatus = true;
            return GetHierarchyMatrix(entityId, entityHierarchyDefinition, operationResult, application, module, calculateStatus);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityId"></param>
        /// <param name="entityHierarchyDefinition"></param>
        /// <param name="operationResult"></param>
        /// <param name="application"></param>
        /// <param name="module"></param>
        /// <param name="calculateStatus"></param>
        /// <returns></returns>
        public Table GetHierarchyMatrix(Int64 entityId, EntityVariantDefinition entityHierarchyDefinition, OperationResult operationResult, MDMCenterApplication application, MDMCenterModules module, Boolean calculateStatus)
        {
            Boolean isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;
            if (isTracingEnabled)
                MDMTraceHelper.StartTraceActivity("EntityHierarchyBL.GetHierarchyMatrix", MDMTraceSource.EntityHierarchyGet, false);

            try
            {
                Table matrixTable = new Table();

                //::steps::
                //0. Find MatrixTable for entityId from DistributedCache
                //1. calculate matrix based on existing dimensions of each level

                //2. get existing child entities with dimension attributes

                //3. calculate Status for each combination in generated matrix
                //  a. combinations which don't match to existing child entities, NEW
                //  b. combinations which match to existing child entities, EXISTING
                //  c. child entities which do not match to combinations, INVALID

                //4. get saved matrix

                //5. and transfer Excluded flag to new matrix, matching all dimensions

                //::MATRIX READY::

                //0

                //1
                Table calculatedTable = this.CalculateMatrix(entityHierarchyDefinition);

                if (calculateStatus)
                {
                    //2
                    EntityCollection childEntities = this.GetChildEntitiesForMatrix(entityId, entityHierarchyDefinition, operationResult, application, module);

                    //3
                    matrixTable = this.CalculateStatusForMatrix(calculatedTable, childEntities, entityHierarchyDefinition, operationResult);
                }
                else
                {
                    matrixTable = calculatedTable;
                }

                //4
                //EntityHierarchyDA entityHierarchyDataManager = new EntityHierarchyDA();
                //Table existingTable = entityHierarchyDataManager.GetHierarchyMatrix(entityId, 0, entityHierarchyDefinition);

                //5
                //matrixTable = this.CalculateExcludedForMatrix(matrixTable, existingTable, entityHierarchyDefinition);

                return matrixTable;
            }
            finally
            {
                if (isTracingEnabled)
                    MDMTraceHelper.StopTraceActivity("EntityHierarchyBL.GetHierarchyMatrix", MDMTraceSource.EntityHierarchyGet);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityId"></param>
        /// <param name="ehDefinition"></param>
        /// <param name="or"></param>
        /// <param name="calculateStatus"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public Table GetHierarchyMatrixExtended(Int64 entityId, EntityVariantDefinition ehDefinition, OperationResult or, Boolean calculateStatus, CallerContext context)
        {
            TraceSettings traceSettings = MDMOperationContextHelper.GetCurrentTraceSettings();

            DiagnosticActivity currentActivity = new DiagnosticActivity();

            if (traceSettings.IsTracingEnabled)
            {
                currentActivity.Start();
            }
            try
            {
                Table matrixTable = new Table();

                //::steps::
                //0. Find MatrixTable for entityId from DistributedCache
                //1. calculate matrix based on existing dimensions of each level

                //2. get existing child entities with dimension attributes

                //3. calculate Status for each combination in generated matrix
                //  a. combinations which don't match to existing child entities, NEW
                //  b. combinations which match to existing child entities, EXISTING
                //  c. child entities which do not match to combinations, INVALID

                //4. get saved matrix

                //5. and transfer Excluded flag to new matrix, matching all dimensions

                //::MATRIX READY::

                //0

                //1
                if (ehDefinition != null)
                {
                    Table calculatedTable = this.CalculateMatrixExtended(ehDefinition, context);

                    if (calculateStatus)
                    {
                        //2
                        EntityCollection childEntities = this.GetChildEntitiesForMatrixByContext(entityId, ehDefinition, or, context);

                        //3
                        matrixTable = this.CalculateStatusForMatrixByContext(calculatedTable, childEntities, ehDefinition, or, context);
                    }
                    else
                    {
                        matrixTable = calculatedTable;
                    }
                }

                //4
                //EntityHierarchyDA entityHierarchyDataManager = new EntityHierarchyDA();
                //Table existingTable = entityHierarchyDataManager.GetHierarchyMatrix(entityId, 0, entityHierarchyDefinition);

                //5
                //matrixTable = this.CalculateExcludedForMatrix(matrixTable, existingTable, entityHierarchyDefinition);

                return matrixTable;
            }
            finally
            {
                if (traceSettings.IsTracingEnabled)
                    currentActivity.Stop();
            }
        }

        /// <summary>
        /// Processes matrix data for an entity for a hierarchy definition
        /// </summary>
        /// <param name="entityId">Id of Entity for which matrix data is to be saved</param>
        /// <param name="matrixTable">Table with matrix data to be saved</param>
        /// <param name="entityHierarchyDefinition">Definition for which matrix data is to be saved</param>
        /// <returns>Integer as status of operation</returns>
        public OperationResult ProcessHierarchyMatrix(Int64 entityId, Table matrixTable, EntityVariantDefinition entityHierarchyDefinition)
        {
            OperationResult operationResult = new OperationResult();

            String userName = MDM.Utility.SecurityPrincipalHelper.GetCurrentSecurityPrincipal().CurrentUserName;
            EntityHierarchyDA hierarchyDataManager = new EntityHierarchyDA();

            Int32 result = hierarchyDataManager.ProcessHierarchyMatrix(entityId, matrixTable, entityHierarchyDefinition, userName, "EntityHierarchyBL");

            if (result > 0)
            {
                operationResult.OperationResultStatus = OperationResultStatusEnum.Successful;
            }
            else
            {
                operationResult.OperationResultStatus = OperationResultStatusEnum.Failed;
            }

            return operationResult;
        }

        /// <summary>
        /// Generates hierarchy of entities based on matrix data for an entity for a hierarchy definition.
        /// It also saves the matrix data before generating the actual hierarchy.
        /// </summary>
        /// <param name="entity">Entity for which hierarchy is to be generated</param>
        /// <param name="matrixTable">Table with matrix data using which entities will be created</param>
        /// <param name="entityHierarchyDefinition">Definition for which matrix data is given</param>
        /// <param name="application">Calling application</param>
        /// <param name="module">Calling module</param>
        /// <returns>OperatioResult object for status of operation</returns>
        public OperationResult GenerateHierarchy(Entity entity, Table matrixTable, EntityVariantDefinition entityHierarchyDefinition, MDMCenterApplication application, MDMCenterModules module)
        {
            Boolean isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;
            if (isTracingEnabled)
                MDMTraceHelper.StartTraceActivity("EntityHierarchyBL.GenerateHierarchy", MDMTraceSource.EntityHierarchyProcess, false);

            DurationHelper completeGenerateHierarchyDurationHelper = new DurationHelper(DateTime.Now);
            try
            {
                #region Step : Initial Setup

                //TODO: Need to check change of logic for base level
                OperationResult operationResult = new OperationResult();
                EntityVariantLevel level = entityHierarchyDefinition.EntityVariantLevels.FirstOrDefault();
                EntityVariantLevel baseLevel = level;
                EntityBL entityManager = new EntityBL();
                CallerContext callerContext = new CallerContext(application, module);
                EntityCollection entities = new EntityCollection() { entity };
                EntityOperationResultCollection entityOperationResults = new EntityOperationResultCollection();
                EntityCollection childEntities = new EntityCollection();
                EntityCollection entitiesNeedToBeCreated = new EntityCollection();
                EntityCollection existingEntities = new EntityCollection();
                Boolean isAdditionalAttributesExist = false;
                Int32 currentLevelEntityTypeId = baseLevel.EntityTypeId;
                Collection<Int32> dimAttrIdsOfLevel = null;

                DurationHelper durHelperationHelper = new DurationHelper(DateTime.Now);

                #endregion

                #region Step : Prepare Entity Operation Result Schema

                entityOperationResults = PrepareEntityOperationResultsSchema(entities);

                if (isTracingEnabled)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("{0}ms - Prepared entity operation results", durHelperationHelper.GetDurationInMilliseconds(DateTime.Now)), MDMTraceSource.EntityHierarchyProcess);

                #endregion

                MDMRuleParams mdmRuleParams = new MDMRuleParams()
                {
                    Entities = entities,
                    EntityOperationResults = entityOperationResults,
                    UserSecurityPrincipal = _securityPrincipal,
                    CallerContext = callerContext,
                    DDGCallerModule = DDGCallerModule.EntityProcess,
                    Events = new Collection<MDMEvent>() { MDMEvent.EntityHierarchyUpdating, MDMEvent.EntityHierarchyUpdated }
                };

                //EntityContext preLoadEntityContext = PreLoadContextHelper.GetEntityContext(mdmRuleParams, entityManager);
                //entityManager.EnsureEntityData(entities, preLoadEntityContext, callerContext);

                #region Step : Trigger Pre Process events

                if (isTracingEnabled)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Starting Generate Hierarchy pre-process events...", MDMTraceSource.EntityHierarchyProcess);

                #region Step : Trigger updating events

                if (isTracingEnabled)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Entity Hierarchy Updating event is being triggered..", MDMTraceSource.EntityHierarchyProcess);

                if (!FireEntityEvent(entities, entities, entityOperationResults, callerContext, MDMEvent.EntityHierarchyUpdating, mdmRuleParams))
                {
                    if (isTracingEnabled)
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, "Generate Hierarchy Pre Process failed.", MDMTraceSource.EntityHierarchyProcess);

                    ApplicationMessage message = messageBL.GetMessage("40003", this.SystemAttributeLocale.GetCultureName());
                    operationResult.Errors.Add(new Error(String.Empty, message.Message));

                    PopulateOperationResult(entity, entityOperationResults.FirstOrDefault(), operationResult);

                    return operationResult;
                }

                if (isTracingEnabled)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Entity Hierarchy Updating event completed", MDMTraceSource.EntityHierarchyProcess);

                #endregion

                if (isTracingEnabled)
                {
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Done with Generate Hierarchy pre-process events", MDMTraceSource.EntityHierarchyProcess);
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("{0}ms - Generate Hierarchy pre-process events", durHelperationHelper.GetDurationInMilliseconds(DateTime.Now)), MDMTraceSource.EntityHierarchyProcess);
                }


                #endregion

                #region Populate Dimension and their Dependents of an Entity

                if (_isAttributeDependencyEnabled)
                {
                    EntityContext context = new EntityContext();
                    context.LoadAttributes = true;
                    context.LoadDependentAttributes = true;
                    context.ContainerId = entity.ContainerId;
                    context.CategoryId = entity.CategoryId;
                    context.EntityTypeId = entity.EntityTypeId;

                    Collection<Int32> dimensionAttrIdList = new Collection<Int32>();
                    foreach (EntityVariantLevel eLevel in entityHierarchyDefinition.EntityVariantLevels)
                    {
                        foreach (Attribute dimAttr in eLevel.DimensionAttributes)
                        {
                            dimensionAttrIdList.Add(dimAttr.Id);
                        }
                    }

                    context.AttributeIdList = dimensionAttrIdList;

                    entity = entityManager.Get(entity.Id, context, application, module, false, false);

                    if (isTracingEnabled)
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("{0}ms - Populate Dimension and their dependent attributes of an Entity", durHelperationHelper.GetDurationInMilliseconds(DateTime.Now)), MDMTraceSource.EntityHierarchyProcess);
                }

                #endregion

                #region Generate Hierarchy up to N Level

                while (level != null)
                {
                    #region Prepare Entity Context

                    EntityContext entityContext = new EntityContext();
                    entityContext.AttributeModelType = AttributeModelType.All;
                    entityContext.EntityTypeId = level.EntityTypeId;
                    entityContext.LoadAttributes = true;
                    entityContext.LoadDependentAttributes = _isAttributeDependencyEnabled;
                    entityContext.CategoryId = entity.CategoryId;

                    Collection<Int32> attributeIdList = new Collection<Int32>();
                    foreach (Column column in matrixTable.Columns)
                    {
                        String colName = column.Name;

                        if (colName.EndsWith(_additionalAttributeSuffix))
                        {
                            Int32 id = ValueTypeHelper.Int32TryParse(colName.Substring(0, colName.Length - _additionalAttributeSuffix.Length), -1);
                            if (id > 0)
                            {
                                isAdditionalAttributesExist = true;
                                attributeIdList.Add(id);
                            }
                        }
                    }

                    entityContext.AttributeIdList = attributeIdList;

                    #endregion

                    existingEntities = new EntityCollection();
                    dimAttrIdsOfLevel = new Collection<Int32>();

                    if (level == baseLevel)
                    {
                        if (isTracingEnabled)
                            MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Creation of entities started for Level : " + level.Id.ToString() + " and EntityTypeId : " + level.EntityTypeId.ToString(), MDMTraceSource.EntityHierarchyProcess);

                        entityContext.CategoryId = entity.CategoryId;
                        entityContext.ContainerId = entity.ContainerId;

                        foreach (Attribute dimAttr in level.DimensionAttributes)
                        {
                            dimAttrIdsOfLevel.Add(dimAttr.Id);
                            entityContext.AttributeIdList.Add(dimAttr.Id);
                        }

                        if (isTracingEnabled)
                            MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "GetEntityModel for EntityTypeId : " + level.EntityTypeId.ToString(), MDMTraceSource.EntityHierarchyProcess);

                        Entity model = entityManager.GetModel(level.EntityTypeId, entity.CategoryId, entity.Id, entityContext, new CallerContext(application, module));

                        if (model != null && LoadAttributeModels(dimAttrIdsOfLevel, model, level.LongName, operationResult))
                        {
                            //Populate required details from Parent Entity to the above obtained model..
                            model.CategoryName = entity.CategoryName;
                            model.CategoryLongName = entity.CategoryLongName;
                            model.ParentEntityId = entity.Id;
                            model.ParentEntityTypeId = entity.EntityTypeId;
                            model.ParentEntityName = entity.Name;
                            model.ParentEntityLongName = entity.LongName;
                            model.EntityFamilyId = entity.EntityFamilyId;
                            model.EntityGlobalFamilyId = entity.EntityGlobalFamilyId;

                            if (isTracingEnabled)
                                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Loading candidate child entities from the matrix...", MDMTraceSource.EntityHierarchyProcess);

                            entitiesNeedToBeCreated = this.GetUniqueEntityCandidatesFromMatrix(matrixTable, entityHierarchyDefinition, model, entity, dimAttrIdsOfLevel, operationResult, application, module, out existingEntities);

                            if (isTracingEnabled)
                                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Total number of childEntities found for entityId : " + entity.Id + " is " + entitiesNeedToBeCreated.Count.ToString(), MDMTraceSource.EntityHierarchyProcess);
                        }
                    }
                    else
                    {
                        if (isTracingEnabled)
                            MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Creation of entities started for Level : " + level.Id.ToString() + " and EntityTypeId : " + level.EntityTypeId.ToString(), MDMTraceSource.EntityHierarchyProcess);

                        EntityCollection prevLevelEntities = new EntityCollection(childEntities.Where(e => e.EntityTypeId == currentLevelEntityTypeId).ToList());

                        if (prevLevelEntities != null && prevLevelEntities.Count > 0)
                        {
                            entitiesNeedToBeCreated = new EntityCollection();
                            EntityCollection uniqueEntities = new EntityCollection();
                            EntityVariantLevel dimlevel = level;

                            //to prepare a list of dimension attributes, from me to top level
                            while (dimlevel != null)
                            {
                                foreach (Attribute dimAttr in dimlevel.DimensionAttributes)
                                {
                                    dimAttrIdsOfLevel.Add(dimAttr.Id);
                                    entityContext.AttributeIdList.Add(dimAttr.Id);
                                }

                                dimlevel = entityHierarchyDefinition.EntityVariantLevels.SingleOrDefault(ilevel => ilevel.Id == dimlevel.ParentLevelId);
                            }

                            if (isTracingEnabled)
                                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "GetEntityModel for EntityTypeId : " + level.EntityTypeId.ToString(), MDMTraceSource.EntityHierarchyProcess);

                            entityContext.CategoryId = entity.CategoryId;
                            entityContext.ContainerId = entity.ContainerId;

                            Entity model = entityManager.GetModel(level.EntityTypeId, entity.CategoryId, 0, entityContext, new CallerContext(application, module));

                            if (model != null && LoadAttributeModels(dimAttrIdsOfLevel, model))
                            {
                                model.CategoryName = entity.CategoryName;
                                model.CategoryLongName = entity.CategoryLongName;
                                model.ParentEntityTypeId = currentLevelEntityTypeId;
                                model.EntityFamilyId = entity.EntityFamilyId;
                                model.EntityGlobalFamilyId = entity.EntityGlobalFamilyId;

                                foreach (Entity nextEntity in prevLevelEntities)
                                {
                                    EntityCollection existingChild = new EntityCollection();

                                    if (isTracingEnabled)
                                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Loading candidate child entities from the matrix...", MDMTraceSource.EntityHierarchyProcess);

                                    model.ParentEntityId = nextEntity.Id;
                                    model.ParentEntityName = nextEntity.Name;
                                    model.ParentEntityLongName = nextEntity.LongName;

                                    uniqueEntities.AddRange(this.GetUniqueEntityCandidatesFromMatrix(matrixTable, entityHierarchyDefinition, model, nextEntity, dimAttrIdsOfLevel, operationResult, application, module, out existingChild));

                                    if (existingChild != null)
                                        existingEntities.AddRange(existingChild);

                                    if (isTracingEnabled)
                                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Total number of childEntities found for entityId : " + nextEntity.Id + " is " + uniqueEntities.Count.ToString(), MDMTraceSource.EntityHierarchyProcess);
                                }

                                currentLevelEntityTypeId = level.EntityTypeId;

                                entitiesNeedToBeCreated.AddRange(uniqueEntities);
                            }
                        }
                    }

                    if (entitiesNeedToBeCreated != null && entitiesNeedToBeCreated.Count > 0)
                    {
                        EntityCollection processEntities = new EntityCollection(entitiesNeedToBeCreated.Where(nEntity => nEntity.Action == ObjectAction.Create).ToList());

                        if (existingEntities != null && existingEntities.Count > 0)
                            childEntities.AddRange(existingEntities);

                        if (processEntities.Count > 0)
                        {
                            if (isTracingEnabled)
                                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Total number of childEntities found for Creation : " + processEntities.Count.ToString(), MDMTraceSource.EntityHierarchyProcess);

                            if (isAdditionalAttributesExist)
                            {
                                foreach (Entity e in processEntities)
                                {
                                    AddAdditionalAttributes(matrixTable, e, _additionalAttributeSuffix);
                                }
                            }

                            entityOperationResults = entityManager.Create(processEntities, "EntityHierarchyBL.GenerateHierarchy", application, module);

                            if (entityOperationResults.OperationResultStatus == OperationResultStatusEnum.Failed || entityOperationResults.OperationResultStatus == OperationResultStatusEnum.CompletedWithErrors)
                            {
                                if (isTracingEnabled)
                                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "childEntities failed for Creation" + entityOperationResults.FirstOrDefault().ToXml(), MDMTraceSource.EntityHierarchyProcess);

                                PopulateOperationResult(entity, entityOperationResults.FirstOrDefault(), operationResult);

                                return operationResult;
                            }
                            else
                            {
                                childEntities.AddRange(processEntities);
                                if (isTracingEnabled)
                                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Total number of childEntities found created : " + processEntities.Count.ToString(), MDMTraceSource.EntityHierarchyProcess);
                            }
                        }
                    }

                    level = entityHierarchyDefinition.EntityVariantLevels.SingleOrDefault(nextlevel => nextlevel.ParentLevelId == level.Id);
                }

                if (isTracingEnabled)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("{0}ms - Generate Hierarchy up to N Level", durHelperationHelper.GetDurationInMilliseconds(DateTime.Now)), MDMTraceSource.EntityHierarchyProcess);

                #endregion

                #region Update the status in Matrix Table

                //Table calculatedTable = this.CalculateMatrix(entityHierarchyDefinition, operationResult);

                EntityCollection leafLevelEntities = new EntityCollection(childEntities.Where(e => e.EntityTypeId == currentLevelEntityTypeId).ToList());

                //matrixTable = this.CalculateStatusForMatrix(calculatedTable, leafLevelEntities, entityHierarchyDefinition, operationResult);

                //if (matrixTable != null)
                //{
                //    operationResult.ReturnValues.Add("<EntityHierarchyMatrixInfo>" + matrixTable.ToXml() + "</EntityHierarchyMatrixInfo>");
                //}

                if (isTracingEnabled)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("{0}ms - Update the status in Matrix Table", durHelperationHelper.GetDurationInMilliseconds(DateTime.Now)), MDMTraceSource.EntityHierarchyProcess);

                #endregion

                #region Step : Remove Hierarchy Relationships cache

                EntityBufferManager entityBufferManager = new EntityBufferManager();

                entityBufferManager.RemoveHierarchyRelationships(entity.Id, entity.ContainerId, entity.EntityTypeId);

                foreach (Entity childEntity in childEntities)
                {
                    //do not cache for leaf level
                    if (!leafLevelEntities.Contains(childEntity.Id))
                        entityBufferManager.RemoveHierarchyRelationships(childEntity.Id, childEntity.ContainerId, childEntity.EntityTypeId);
                }

                if (isTracingEnabled)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("{0}ms - Remove Hierarchy Relationships cache", durHelperationHelper.GetDurationInMilliseconds(DateTime.Now)), MDMTraceSource.EntityHierarchyProcess);

                #endregion

                #region Step : Trigger Post Process event

                #region Step : Trigger updated events

                if (operationResult.OperationResultStatus != OperationResultStatusEnum.Failed)
                {
                    if (isTracingEnabled)
                    {
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Starting Generate Hierarchy post-process events...", MDMTraceSource.EntityHierarchyProcess);
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Entity Hierarchy Updated event is being triggered..", MDMTraceSource.EntityHierarchyProcess);
                    }

                    FireEntityEvent(entities, null, entityOperationResults, callerContext, MDMEvent.EntityHierarchyUpdated, mdmRuleParams);

                    if (isTracingEnabled)
                    {
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Entity Hierarchy Updated event completed.", MDMTraceSource.EntityHierarchyProcess);
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Done with Generate Hierarchy post-process events", MDMTraceSource.EntityHierarchyProcess);
                    }
                }
                else
                {
                    if (isTracingEnabled)
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Entity hierarchy generation failed. Skipping Post-Process events..", MDMTraceSource.EntityHierarchyProcess);
                }

                //In case of actual process failure also where Post-Process won't be called, we need to still populate Operation Result if any warnings or information has been added in Pre-Process event 
                PopulateOperationResult(entity, entityOperationResults.FirstOrDefault(), operationResult);

                #endregion

                if (isTracingEnabled)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("{0}ms - Generate Hierarchy Post Process event", durHelperationHelper.GetDurationInMilliseconds(DateTime.Now)), MDMTraceSource.EntityHierarchyProcess);

                #endregion

                return operationResult;
            }
            finally
            {
                if (isTracingEnabled)
                {
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("{0}ms - Overall Generate Hierarchy time", completeGenerateHierarchyDurationHelper.GetDurationInMilliseconds(DateTime.Now)), MDMTraceSource.EntityHierarchyProcess);
                    MDMTraceHelper.StopTraceActivity("EntityHierarchyBL.GenerateHierarchy", MDMTraceSource.EntityHierarchyProcess);
                }
            }
        }

        /// <summary>
        /// Generates hierarchy of entities based on matrix data for an entity for a hierarchy definition.
        /// It also saves the matrix data before generating the actual hierarchy.
        /// </summary>
        /// <param name="entity">Entity for which hierarchy is to be generated</param>
        /// <param name="entityHierachyDefinition">Definition for which matrix data is given</param>
        /// <param name="callerContext">Caller Context</param>
        /// <returns>OperatioResult object for status of operation</returns>
        public OperationResult GenerateHierarchy(Entity entity, EntityVariantDefinition entityHierachyDefinition, CallerContext callerContext)
        {
            TraceSettings traceSettings = MDMOperationContextHelper.GetCurrentTraceSettings();

            DiagnosticActivity currentActivity = new DiagnosticActivity();

            if (traceSettings.IsTracingEnabled)
            {
                currentActivity.Start();
            }

            OperationResult operationResult = new OperationResult();
            Table matrixTable = new Table();
            Boolean isDimValuesSaved = false;

            try
            {
                if (callerContext == null)
                {
                    _localeMessage = _localeMessageBL.Get(GlobalizationHelper.GetSystemUILocale(), "111823", false, callerContext);
                    currentActivity.LogError(_localeMessage.Message);
                    throw new MDMOperationException("111823", _localeMessage.Message, "EntityHierarchyManager", String.Empty, "GenerateHierarchy");//CallerContext is null or empty
                }

                if (traceSettings.IsBasicTracingEnabled)
                    currentActivity.LogInformation("Deciding if dimension values should be saved");

                isDimValuesSaved = ProcessHierarchyGenerationRules(entity.Id, entityHierachyDefinition, callerContext);

                if (isDimValuesSaved)
                {
                    matrixTable = GetHierarchyMatrixExtended(entity.Id, entityHierachyDefinition, operationResult, true, callerContext);  //GetHierarchyMatrix(entity.Id, entityHierachyDefinition, operationResult, callerContext.Application, callerContext.Module);

                    if (operationResult.OperationResultStatus == OperationResultStatusEnum.Failed || operationResult.OperationResultStatus == OperationResultStatusEnum.CompletedWithErrors)
                    {
                        return operationResult;
                    }
                    else
                    {
                        if (traceSettings.IsBasicTracingEnabled)
                            currentActivity.LogInformation("Calling GenerateHierarchy that doesn't have Diagnostic messages");

                        operationResult = GenerateHierarchy(entity, matrixTable, entityHierachyDefinition, callerContext.Application, callerContext.Module);
                    }
                }
            }
            finally
            {
                if (traceSettings.IsBasicTracingEnabled)
                    currentActivity.Stop();
            }

            return operationResult;
        }

        /// <summary>
        /// Checks whether the Definition is modified or not
        /// </summary>
        /// <param name="entityId">Entity Id for which definition is being checked</param>
        /// <param name="hierarchyDefinationId">Id of HierarchyDefinition</param>
        /// <param name="application">Calling application</param>
        /// <param name="module">Calling module</param>
        /// <returns>True if EntityHierarchyDefinition is Changed</returns>
        /// <exception cref="ArgumentException">Thrown if Entity Id is less then 0</exception>
        /// <exception cref="ArgumentException">Thrown if Definition Id is less then 0</exception>
        public Boolean IsLatestMatrix(Int64 entityId, Int32 hierarchyDefinationId, MDMCenterApplication application, MDMCenterModules module)
        {
            if (entityId < 0)
                throw new ArgumentException("Invalid EntityId : " + entityId.ToString());

            if (hierarchyDefinationId < 0)
                throw new ArgumentException("Invalid HierarchyDefinitionId : " + hierarchyDefinationId.ToString());

            EntityHierarchyDA hierarchyDataManager = new EntityHierarchyDA();
            DBCommandProperties command = DBCommandHelper.Get(application, module, MDMCenterModuleAction.Read);
            return hierarchyDataManager.IsLatestMatrix(entityId, hierarchyDefinationId, command);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityId"></param>
        /// <param name="hierarchyDefId"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public Boolean IsLatestMatrix(Int64 entityId, Int32 hierarchyDefId, CallerContext context)
        {
            TraceSettings traceSettings = MDMOperationContextHelper.GetCurrentTraceSettings();
            DiagnosticActivity currentActivity = new DiagnosticActivity();

            if (traceSettings.IsBasicTracingEnabled)
            {
                ExecutionContext ec = new ExecutionContext();
                ec.CallerContext = context;
                currentActivity.Start(ec);
            }

            Boolean islatest = false;

            try
            {
                EntityHierarchyDA hierarchyDataManager = new EntityHierarchyDA();
                DBCommandProperties command = DBCommandHelper.Get(context.Application, context.Module, MDMCenterModuleAction.Read);
                islatest = hierarchyDataManager.IsLatestMatrix(entityId, hierarchyDefId, command);
            }
            catch (Exception e)
            {
                currentActivity.LogError(e.Message);
            }
            finally
            {
                if (traceSettings.IsBasicTracingEnabled)
                {
                    currentActivity.Stop();

                    IDistributedCache cache = CacheFactory.GetDistributedCache();
                    cache.Set(context.OperationId.ToString(), currentActivity, DateTime.Now.AddHours(2));
                }
            }

            return islatest;
        }

        #endregion Hierarchy Matrix

        #region Get child entities methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="topLevelEntity"></param>
        /// <param name="entityHierarchyDefinition"></param>
        /// <param name="entityModelRunningDictionary"></param>
        /// <param name="operationResult"></param>
        /// <param name="callerContext"></param>
        /// <returns></returns>
        public EntityCollection GetPossibleChildEntities(Entity topLevelEntity, EntityVariantDefinition entityHierarchyDefinition, ref Dictionary<String, Entity> entityModelRunningDictionary, OperationResult operationResult, CallerContext callerContext)
        {
            Boolean isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;
            #region Step: Write traces

            if (isTracingEnabled)
                MDMTraceHelper.StartTraceActivity("EntityHierarchyBL.GetPossibleChildEntities", MDMTraceSource.EntityHierarchyGet, false);

            DurationHelper durHelperationHelper = new DurationHelper(DateTime.Now);
            EntityCollection finalPossibleEntities = new EntityCollection();
            try
            {
            #endregion

                #region Step: Validation

                if (callerContext == null)
                {
                    _localeMessage = _localeMessageBL.Get(GlobalizationHelper.GetSystemUILocale(), "111823", false, callerContext);
                    throw new MDMOperationException("111823", _localeMessage.Message, "EntityHierarchyManager", String.Empty, "GetChildEntitiesByDimensionValues");//CallerContext is null or empty
                }

                #endregion

                #region Step: Initial Setup

                EntityBL entityManager = new EntityBL();

                Int32 runningEntityReferenceId = -1;

                entityModelRunningDictionary = new Dictionary<String, Entity>();

                Dictionary<Int32, Dictionary<String, Entity>> possibleEntitiesDictionary = new Dictionary<Int32, Dictionary<String, Entity>>();

                LocaleEnum systemDataLocale = GlobalizationHelper.GetSystemDataLocale();

                #endregion

                #region Step: Save DimensionValues and return if save fails

                Boolean isDimValuesSaved = ProcessHierarchyGenerationRules(topLevelEntity.Id, entityHierarchyDefinition);

                if (!isDimValuesSaved)
                {
                    return finalPossibleEntities;
                }

                #endregion

                #region Step: Get Hierarchy Matrix

                Boolean calculateStatus = false;
                Table entityHierarchyMatrixTable = GetHierarchyMatrix(topLevelEntity.Id, entityHierarchyDefinition, operationResult, callerContext.Application, callerContext.Module, calculateStatus);

                if (operationResult.OperationResultStatus == OperationResultStatusEnum.Failed || operationResult.OperationResultStatus == OperationResultStatusEnum.CompletedWithErrors)
                {
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, "Error while Geting Matrix Table for EntityId : " + topLevelEntity.Id.ToString() + " : " + operationResult.Errors[0].ErrorMessage, MDMTraceSource.EntityHierarchyGet);
                    return finalPossibleEntities;
                }

                #endregion

                #region Step: Get all levels as dictionary

                Dictionary<Int32, EntityVariantLevel> entityHierarchyLevelsAsDictionary = GetEntityHierarchyLevelsAsDictionary(entityHierarchyDefinition);

                #endregion

                #region Step: Loop through levels, create possible entity objects and set its references

                foreach (KeyValuePair<Int32, EntityVariantLevel> pair in entityHierarchyLevelsAsDictionary)
                {
                    #region Step: Setup variables

                    Int32 currentLevelId = pair.Key;
                    EntityVariantLevel currentEntityHierarchyLevel = pair.Value;
                    Int32 currentLevelEntityTypeId = currentEntityHierarchyLevel.EntityTypeId;

                    EntityVariantLevel prevEntityHierarchyLevel = null;

                    // Added contains check because for EntityVariantLevel which is having Rank one its parent will not be available in EntityVariantDefinition.EntityVariantLevels
                    if (currentEntityHierarchyLevel.ParentLevelId > 0 && entityHierarchyLevelsAsDictionary.ContainsKey(currentEntityHierarchyLevel.ParentLevelId))
                    {
                        prevEntityHierarchyLevel = entityHierarchyLevelsAsDictionary[currentEntityHierarchyLevel.ParentLevelId];
                    }

                    Dictionary<String, Entity> currentLevelPossibleEntities = new Dictionary<String, Entity>();
                    Dictionary<String, Entity> prevLevelPossibleEntities = new Dictionary<String, Entity>();

                    if (prevEntityHierarchyLevel != null)
                    {
                        prevLevelPossibleEntities = possibleEntitiesDictionary[prevEntityHierarchyLevel.Id];
                    }

                    #endregion

                    #region Step: Get Entity Model

                    Entity entityModel;

                    String entityModelDictionaryKey = GetEntityModelDictionaryKey(entityHierarchyDefinition.Id, currentLevelId, currentLevelEntityTypeId);
                    if (entityModelRunningDictionary.Keys.Contains(entityModelDictionaryKey))
                    {
                        entityModel = entityModelRunningDictionary[entityModelDictionaryKey];
                    }
                    else
                    {
                        entityModel = GetEntityModel(topLevelEntity, currentLevelEntityTypeId, currentEntityHierarchyLevel.DenormalizedDimensionAttributeIdList);

                        if (entityModel != null)
                            entityModelRunningDictionary.Add(entityModelDictionaryKey, entityModel);
                    }

                    #endregion

                    #region Step: Loop through matrix and create possible entity object

                    foreach (Row matrixRow in entityHierarchyMatrixTable.Rows)
                    {
                        #region Step: Create unique keys for current row entity and parent entity

                        String currentEntityUniqueKey = String.Format("EH_UID_LE{0}", currentLevelId);
                        String parentEntityUniqueKey = String.Empty;

                        if (prevEntityHierarchyLevel != null)
                        {
                            parentEntityUniqueKey = String.Format("EH_UID_LE{0}", prevEntityHierarchyLevel.Id);

                            foreach (Int32 dimensionAttrId in prevEntityHierarchyLevel.DenormalizedDimensionAttributeIdList)
                            {
                                String uniqueValKey = GetUniqueValKeyString(matrixRow, dimensionAttrId, systemDataLocale);
                                parentEntityUniqueKey = String.Concat(parentEntityUniqueKey, "_", uniqueValKey);
                            }
                        }

                        foreach (Int32 dimensionAttrId in currentEntityHierarchyLevel.DenormalizedDimensionAttributeIdList)
                        {
                            String uniqueValKey = GetUniqueValKeyString(matrixRow, dimensionAttrId, systemDataLocale);
                            currentEntityUniqueKey = String.Concat(currentEntityUniqueKey, "_", uniqueValKey);
                        }

                        #endregion

                        #region Step: Populate Possible entity

                        if (!currentLevelPossibleEntities.ContainsKey(currentEntityUniqueKey))
                        {
                            //TODO:: clone complete entity here instead of using ToXml()
                            Entity possibleEntity = entityModel.CloneBasicProperties();
                            possibleEntity.Attributes = (AttributeCollection)entityModel.Attributes.Clone();


                            possibleEntity.Action = ObjectAction.Create;
                            possibleEntity.ReferenceId = runningEntityReferenceId--;
                            possibleEntity.ParentEntityId = -1;
                            possibleEntity.ParentEntityName = String.Empty;

                            if (!String.IsNullOrWhiteSpace(parentEntityUniqueKey) && prevLevelPossibleEntities.ContainsKey(parentEntityUniqueKey))
                            {
                                Entity parentEntity = prevLevelPossibleEntities[parentEntityUniqueKey];

                                if (parentEntity != null)
                                {
                                    possibleEntity.ParentEntityId = parentEntity.ReferenceId;
                                    possibleEntity.ParentEntityName = parentEntity.Name;
                                }
                            }

                            foreach (Int32 dimensionAttrId in currentEntityHierarchyLevel.DenormalizedDimensionAttributeIdList)
                            {
                                PopulateDimensionAttributeValue(matrixRow, possibleEntity, dimensionAttrId, systemDataLocale, topLevelEntity);
                            }

                            currentLevelPossibleEntities.Add(currentEntityUniqueKey, possibleEntity);
                        }

                        #endregion
                    }

                    #endregion

                    #region Step: Add Level wise dictionary

                    possibleEntitiesDictionary.Add(currentLevelId, currentLevelPossibleEntities);

                    #endregion
                }

                #endregion

                #region Step: Collection all possible entities and return

                foreach (KeyValuePair<Int32, Dictionary<String, Entity>> levelPair in possibleEntitiesDictionary)
                {
                    Dictionary<String, Entity> currentLevelPossibleEntities = levelPair.Value;

                    foreach (Entity entity in currentLevelPossibleEntities.Values)
                    {
                        finalPossibleEntities.Add(entity);
                    }
                }

                #endregion
            }
            finally
            {
                #region Step: Write traces

                if (isTracingEnabled)
                {
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("{0}ms - Loaded {1} possible child entities.", durHelperationHelper.GetDurationInMilliseconds(DateTime.Now), finalPossibleEntities.Count), MDMTraceSource.EntityHierarchyGet);
                    MDMTraceHelper.StopTraceActivity("EntityHierarchyBL.GetPossibleChildEntities", MDMTraceSource.EntityHierarchyGet);
                }

                #endregion
            }

            return finalPossibleEntities;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityId"></param>
        /// <param name="entityTypeId"></param>
        /// <param name="attributeInfoList"></param>
        /// <param name="application"></param>
        /// <param name="module"></param>
        /// <returns></returns>
        public EntityCollection GetChildEntitiesByEntityType(Int64 entityId, Int32 entityTypeId, Collection<KeyValuePair<Int32, LocaleEnum>> attributeInfoList, MDMCenterApplication application, MDMCenterModules module)
        {
            Boolean isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;
            if (isTracingEnabled)
            {
                MDMTraceHelper.StartTraceActivity("EntityManager.EntityHierarchyBL.GetChildEntitiesByEntityType started...", MDMTraceSource.EntityHierarchyGet, false);
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("Requested parent Entity Id: {0} and child Entity Type Id:{1}", entityId, entityTypeId), MDMTraceSource.EntityHierarchyGet);
            }
            try
            {
                EntityCollection childEntities = new EntityCollection();
                Boolean isFeatureEnabled = MDMFeatureConfigHelper.IsMDMFeatureEnabled(MDMCenterApplication.MDMCenter, "Hierarchy Get", "1");

                if (isFeatureEnabled)
                {
                    CallerContext callerContext = new CallerContext(application, module);
                    Collection<Int32> attributeIdList = new Collection<Int32>();

                    if (attributeInfoList != null &&
                        attributeInfoList.Count > 0)
                    {
                        foreach (KeyValuePair<Int32, LocaleEnum> attrInfo in attributeInfoList)
                        {
                            attributeIdList.Add(attrInfo.Key); //AttributeId
                        }
                    }

                    EntityHierarchyGetManager entityHierarchyGetManager = new EntityHierarchyGetManager(new EntityBL());
                    childEntities = entityHierarchyGetManager.GetChildEntitiesInternal(entityId, entityTypeId, this.SystemAttributeLocale, attributeIdList, -1, callerContext);
                }
                else
                {
                    childEntities = GetChildEntitiesByEntityTypeLegacy(entityId, entityTypeId, attributeInfoList, application, module);
                }

                return childEntities;
            }
            finally
            {
                if (isTracingEnabled)
                    MDMTraceHelper.StopTraceActivity("EntityManager.EntityHierarchyBL.GetChildEntitiesByEntityType Completed.....", MDMTraceSource.EntityHierarchyGet);
            }
        }

        /// <summary>
        /// Gets child entities for the requested parent entity Id.
        /// </summary>
        /// <param name="parentEntityId">parent entity id for which child entities are required.</param>
        /// <param name="childEntityTypeId">Indicates child entity type id.</param>
        /// <param name="locale">Indicates locale</param>
        /// <param name="returnAttributeIds">Indicates return attribute id along with child entities.</param>
        /// <param name="getCompleteDetailsOfEntity">Flag saying whether to load complete details of entity.
        /// If flag is true then it returns CategoryId, CategoryName/LongName , Parent EntityId , Parent EntityName/LongName etc. else EntityId,EntityName and EntityLongName.</param>
        /// <param name="getRecursiveChildren">If this flag is true, it returns recursive children of requested entity type under a parent.</param>        
        /// <returns>collection of child entities.</returns>
        public EntityCollection GetChildEntities(Int64 parentEntityId, Int32 childEntityTypeId, LocaleEnum locale, Collection<Int32> returnAttributeIds, Boolean getCompleteDetailsOfEntity, Int32 maxRecordsToReturn, CallerContext callerContext, Boolean getRecursiveChildren = false)
        {
            TraceSettings traceSettings = MDMOperationContextHelper.GetCurrentTraceSettings();
            DiagnosticActivity currentActivity = new DiagnosticActivity();

            if (traceSettings.IsTracingEnabled)
            {
                if (callerContext != null)
                {
                    ExecutionContext ec = new ExecutionContext();
                    ec.CallerContext = callerContext;
                    currentActivity.Start(ec);
                }
                else
                    currentActivity.Start();
            }

            EntityCollection childEntities = null;
            Boolean isFeatureEnable = MDMFeatureConfigHelper.IsMDMFeatureEnabled(MDMCenterApplication.MDMCenter, "Hierarchy Get", "1");

            if (isFeatureEnable)
            {
                if (traceSettings.IsBasicTracingEnabled)
                {
                    currentActivity.LogInformation("FeatureConfig is enabled so it will get data by calling GetEntityWithHierarchy");
                }

                EntityHierarchyGetManager entityHierarchyGetManager = new EntityHierarchyGetManager(new EntityBL());
                childEntities = entityHierarchyGetManager.GetChildEntitiesInternal(parentEntityId, childEntityTypeId, locale, returnAttributeIds, maxRecordsToReturn, callerContext, getRecursiveChildren);
            }
            else
            {
                if (traceSettings.IsBasicTracingEnabled)
                {
                    currentActivity.LogInformation("FeatureConfig is disabled so it will get data by calling GetChildEntitiesLegacy (Direct DB Call)");
                }

                childEntities = GetChildEntitiesLegacy(parentEntityId, childEntityTypeId, locale, returnAttributeIds, getCompleteDetailsOfEntity, maxRecordsToReturn, callerContext, getRecursiveChildren);
            }

            return childEntities;
        }

        #endregion

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        private EntityOperationResultCollection PrepareEntityOperationResultsSchema(EntityCollection entities)
        {
            EntityOperationResultCollection entityOperationResults = new EntityOperationResultCollection();

            foreach (Entity entity in entities)
            {
                EntityOperationResult entityOperationResult = new EntityOperationResult(entity.Id, entity.ReferenceId, entity.LongName, entity.ExternalId);

                entityOperationResult.ReferenceId = entity.ReferenceId;

                if (entity.Attributes != null && entity.Attributes.Count > 0)
                {
                    foreach (Attribute attr in entity.Attributes)
                    {
                        AttributeOperationResult attributeOperationResult = new AttributeOperationResult(attr.Id, attr.Name, attr.LongName, attr.AttributeModelType, attr.Locale);
                        entityOperationResult.AttributeOperationResultCollection.Add(attributeOperationResult);
                    }
                }

                //TODO: Prepare Relationship OperationResult Schema

                entityOperationResults.Add(entityOperationResult);
            }

            return entityOperationResults;
        }


        private Boolean FireEntityEvent(EntityCollection eventEntities, EntityCollection allEntities, EntityOperationResultCollection entityOperationResults, CallerContext callContext, MDMEvent eventSource, MDMRuleParams mdmRuleParams)
        {
            MDMPublisher publisher = Utility.GetMDMPublisher(callContext.Application, callContext.Module);

            if (callContext.MDMPublisher == MDMPublisher.Unknown)
                callContext.MDMPublisher = publisher;

            //Do we need to assign MDMSource event?? Is it mandatory?
            //callContext.MDMSource = eventSource;

            mdmRuleParams.Events = new Collection<MDMEvent> { eventSource };
            //MDMRuleEvaluator.Evaluate(mdmRuleParams);

            IEntityManager iEntityManager = new EntityBL();
            EntityEventArgs eventArgs = new EntityEventArgs(eventEntities, iEntityManager, entityOperationResults, _securityPrincipal.CurrentUserId, callContext);

            switch (eventSource)
            {
                case MDMEvent.EntityHierarchyUpdating:
                    EntityEventManager.Instance.OnEntityHierarchyUpdating(eventArgs);
                    break;
                case MDMEvent.EntityHierarchyUpdated:
                    EntityEventManager.Instance.OnEntityHierarchyUpdated(eventArgs);
                    break;
                default:
                    break;
            }

            entityOperationResults.RefreshOperationResultStatus();

            return (entityOperationResults.OperationResultStatus == OperationResultStatusEnum.Failed) ? false : true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="entityOperationResult"></param>
        /// <param name="operationResult"></param>
        private void PopulateOperationResult(Entity entity, EntityOperationResult entityOperationResult, OperationResult operationResult)
        {
            if (entityOperationResult != null)
            {
                #region Populate Entity Level Errors/Warnings/Informations

                if (entityOperationResult.Errors != null && entityOperationResult.Errors.Count > 0)
                {
                    foreach (Error error in entityOperationResult.Errors)
                    {
                        operationResult.Errors.Add(new Error(String.Empty, String.Format("Entity {0}: {1}", entity.Name, error.ErrorMessage)));
                    }
                }

                if (entityOperationResult.Warnings != null && entityOperationResult.Warnings.Count > 0)
                {
                    foreach (Warning warning in entityOperationResult.Warnings)
                    {
                        operationResult.Warnings.Add(new Warning(String.Empty, String.Format("Entity {0}: {1}", entity.Name, warning.WarningMessage)));
                    }
                }

                if (entityOperationResult.Informations != null && entityOperationResult.Informations.Count > 0)
                {
                    foreach (Information information in entityOperationResult.Informations)
                    {
                        operationResult.Informations.Add(new Information(String.Empty, String.Format("Entity {0}: {1}", entity.Name, information.InformationMessage)));
                    }
                }

                #endregion

                #region Populate Attribute Level Errors/Warnings/Informations

                if (entityOperationResult.AttributeOperationResultCollection != null && entityOperationResult.AttributeOperationResultCollection.Count > 0)
                {
                    foreach (AttributeOperationResult attributeOperationResult in entityOperationResult.AttributeOperationResultCollection)
                    {
                        if (attributeOperationResult.Errors != null && attributeOperationResult.Errors.Count > 0)
                        {
                            foreach (Error error in attributeOperationResult.Errors)
                            {
                                operationResult.Errors.Add(new Error(String.Empty, attributeOperationResult.AttributeLongName + ": " + error.ErrorMessage));
                            }
                        }

                        if (attributeOperationResult.Warnings != null && attributeOperationResult.Warnings.Count > 0)
                        {
                            foreach (Warning warning in attributeOperationResult.Warnings)
                            {
                                operationResult.Warnings.Add(new Warning(String.Empty, attributeOperationResult.AttributeLongName + ": " + warning.WarningMessage));
                            }
                        }

                        if (attributeOperationResult.Informations != null && attributeOperationResult.Informations.Count > 0)
                        {
                            foreach (Information information in attributeOperationResult.Informations)
                            {
                                operationResult.Informations.Add(new Information(String.Empty, attributeOperationResult.AttributeLongName + ": " + information.InformationMessage));
                            }
                        }
                    }
                }

                #endregion

                operationResult.OperationResultStatus = entityOperationResult.OperationResultStatus;

                //TODO: Populate Relationship Level Errors/Warnings/Information
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="matrixTable"></param>
        /// <param name="e"></param>
        /// <param name="suffix"></param>
        private static void AddAdditionalAttributes(Table matrixTable, Entity e, String suffix)
        {
            Row filteredRow = null;

            foreach (Row row in matrixTable.Rows)
            {
                foreach (Attribute attr in e.Attributes)
                {
                    String columnName = attr.Id + "RefId";

                    if (!attr.IsCollection && matrixTable.Columns.Any(c => c.Name.Equals(columnName)))
                    {
                        if (!row[columnName].Equals(attr.CurrentValue))
                        {
                            filteredRow = null;
                            break;
                        }

                        filteredRow = row;
                    }
                }

                if (filteredRow != null)
                {
                    break;
                }
            }

            if (filteredRow != null)
            {
                foreach (Cell cell in filteredRow.Cells)
                {
                    if (cell.ColumnName.EndsWith(suffix))
                    {
                        foreach (Attribute attr in e.Attributes)
                        {
                            if (cell.ColumnName == attr.Id + suffix && !String.IsNullOrEmpty(cell.Value.ToString()))
                            {
                                attr.SetValue(cell.Value);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// We have a table with dimensions of each level as columns. 
        /// We need to look for current level and based on level's dimension attributes, find out all unique items from table.
        /// This method does the same, and returns list of entities.
        /// </summary>
        /// <param name="matrixTable"></param>
        /// <param name="entityHierarchyDefinition"></param>
        /// <param name="model"></param>
        /// <param name="parentEntity"></param>
        /// <param name="dimAttrIdsOfLevel"></param>
        /// <param name="operationResult"></param>
        /// <returns></returns>
        private EntityCollection GetUniqueEntityCandidatesFromMatrix(Table matrixTable, EntityVariantDefinition entityHierarchyDefinition, Entity model, Entity parentEntity, Collection<Int32> dimAttrIdsOfLevel, OperationResult operationResult, MDMCenterApplication application, MDMCenterModules module, out EntityCollection existingChildren)
        {
            Boolean isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;
            if (isTracingEnabled)
                MDMTraceHelper.StartTraceActivity("EntityHierarchyBL.GetUniqueEntityCandidatesFromMatrix", MDMTraceSource.EntityHierarchyProcess, false);

            EntityCollection entityList = new EntityCollection();
            try
            {
                //steps:
                //1. prepare a list of dimension attributes for this level, from me to top level

                //2. filter rows from matrix table, which belongs to my parent level entity

                //3. Find unique rows from filtered item rows

                //4. for each unique item row, check if we already have a matching entity by comparing dim values

                //5. if we have existing entity, just add it in entityList

                //6. if we don't have, create new entity and add in entityList

                //7. return entityList

                existingChildren = new EntityCollection();
                EntityVariantLevel entityHierarchyLevel = entityHierarchyDefinition.EntityVariantLevels.SingleOrDefault(level => level.EntityTypeId == model.EntityTypeId);
                CallerContext callerContext = new CallerContext(application, module);
                //filter rows for current entity
                List<Row> filteredRows = matrixTable.Rows.ToList();

                EntityVariantLevel currentLevel = entityHierarchyLevel;

                while (currentLevel != null)
                {
                    if (isTracingEnabled)
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Finding Child Entities for Level : " + currentLevel.Name, MDMTraceSource.EntityHierarchyProcess);

                    //don't do this first time, we don't want to filter by own dimensions
                    if (currentLevel.Id != entityHierarchyLevel.Id)
                    {
                        if (isTracingEnabled)
                            MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Filter AttributeModels by DimensionAttributes", MDMTraceSource.EntityHierarchyProcess);

                        foreach (Attribute dimAttr in currentLevel.DimensionAttributes)
                        {
                            String strDimAttrId = dimAttr.Id.ToString();
                            Attribute attributeOfParentEntity =
                                (Attribute)parentEntity.GetAttribute(dimAttr.Id, this.SystemAttributeLocale);

                            if (matrixTable.Columns.Contains(strDimAttrId) && matrixTable.Columns.Contains(strDimAttrId + "RefId") && attributeOfParentEntity != null)
                            {
                                //Checking RefID only incase of LookUp
                                if (_attributeModels[dimAttr.Id, this.SystemAttributeLocale].AttributeDisplayTypeName.ToLower().Equals("lookuptable"))
                                {
                                    foreach (Row row in filteredRows)
                                    {
                                        if (row[strDimAttrId + "RefId"].ToString() == "")
                                        {
                                            row[strDimAttrId + "RefId"] = -1;
                                            row[strDimAttrId] = -1;
                                        }
                                    }

                                    if (attributeOfParentEntity.CurrentValues[0].AttrVal != null)
                                    {
                                        filteredRows = filteredRows.Where(row => row[strDimAttrId].ToString() == (String)attributeOfParentEntity.CurrentValues[0].AttrVal.ToString()
                                        || row[strDimAttrId + "RefId"].ToString() == attributeOfParentEntity.CurrentValues[0].ValueRefId.ToString()).ToList();
                                    }
                                }
                                else
                                {
                                    filteredRows = filteredRows.Where(row => row[strDimAttrId].Equals(attributeOfParentEntity.CurrentValues[0].AttrVal)).ToList();
                                }

                                if (isTracingEnabled)
                                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Total Filtered Rows : " + filteredRows.Count.ToString(), MDMTraceSource.EntityHierarchyProcess);
                            }
                            else
                            {
                                filteredRows = new List<Row>();
                            }
                        }
                    }

                    currentLevel = entityHierarchyDefinition.EntityVariantLevels.Where(level => level.Id == currentLevel.ParentLevelId).SingleOrDefault();
                }

                existingChildren = this.GetEntitiesByEntityType(parentEntity.Id, model.EntityTypeId, dimAttrIdsOfLevel, application, module);

                if (isTracingEnabled)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Total ExistingChildren found for entity : " + existingChildren.Count.ToString(), MDMTraceSource.EntityHierarchyProcess);

                foreach (Row row in filteredRows)
                {
                    List<Entity> matchingEntities = entityList.ToList();

                    if (matchingEntities != null && matchingEntities.Count > 0)
                    {
                        foreach (Attribute dimAttr in entityHierarchyLevel.DimensionAttributes)
                        {
                            String strDimAttrId = dimAttr.Id.ToString();

                            if (matrixTable.Columns.Contains(strDimAttrId)
                                && matrixTable.Columns.Contains(strDimAttrId + "RefId"))
                            {
                                Object attrVal = row[strDimAttrId];
                                Int32 refId = ValueTypeHelper.Int32TryParse(row[strDimAttrId + "RefId"].ToString(), -1);

                                if (_attributeModels[dimAttr.Id, this.SystemAttributeLocale].AttributeDisplayTypeName.ToLower().Equals("lookuptable"))
                                {
                                    if (matchingEntities.Count > 0)
                                    {
                                        foreach (Entity entity in matchingEntities)
                                        {
                                            Attribute attribute = (Attribute)entity.GetAttribute(dimAttr.Id, this.SystemAttributeLocale);

                                            if (attribute != null && attribute.CurrentValues[0].AttrVal == null)
                                            {
                                                attribute.SetValue("");
                                            }
                                        }
                                    }

                                    matchingEntities = matchingEntities.Where(entity => entity.Attributes[dimAttr.Id, this.SystemAttributeLocale] != null
                                          && (entity.Attributes[dimAttr.Id, this.SystemAttributeLocale].CurrentValues[0].AttrVal.Equals(refId)
                                          || entity.Attributes[dimAttr.Id, this.SystemAttributeLocale].CurrentValues[0].ValueRefId.Equals(refId))).ToList();
                                }
                                else
                                {
                                    matchingEntities = matchingEntities.Where(entity => entity.Attributes[dimAttr.Id, this.SystemAttributeLocale] != null
                                        && entity.Attributes[dimAttr.Id, this.SystemAttributeLocale].CurrentValues[0].AttrVal.Equals(attrVal)).ToList();
                                }
                            }
                            else
                            {
                                //if dim attr not found, no entity should be created
                                matchingEntities = entityList.ToList();

                                if (isTracingEnabled)
                                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Warning, "No column found for attribute " + dimAttr.Id + ". Entity would not be created", MDMTraceSource.EntityHierarchyProcess);
                                ApplicationMessage message = messageBL.GetMessage("30025", this.SystemAttributeLocale.GetCultureName(), dimAttr.Id);
                                operationResult.Errors.Add(new Error("", message.Message));
                            }
                        }
                    }

                    if (matchingEntities.Count() == 0)
                    {
                        if (isTracingEnabled)
                            MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "No matching entities found. Create New Entity", MDMTraceSource.EntityHierarchyProcess);

                        Entity newEntity = null;

                        //okey... found a new combination of dim values
                        //lets check if we have any existing entity for such combination
                        foreach (Entity existingChild in existingChildren)
                        {
                            newEntity = existingChild;
                            foreach (Int32 dimAttrId in dimAttrIdsOfLevel)
                            {
                                if (!IsDimensionAttributeSame(row, existingChild, dimAttrId, operationResult, callerContext))
                                {
                                    newEntity = null;
                                    break;
                                }
                            }

                            if (newEntity != null)
                            {
                                if (isTracingEnabled)
                                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Found entity : " + newEntity.Name + " with the Same Dimension Attribute", MDMTraceSource.EntityHierarchyProcess);
                                break;
                            }
                        }

                        if (newEntity == null) //means, no matches found in existing children also
                        {
                            if (isTracingEnabled)
                            {
                                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "No Entity found in existing Children.", MDMTraceSource.EntityHierarchyProcess);
                                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Prepare NewEntity to create", MDMTraceSource.EntityHierarchyProcess);
                            }

                            newEntity = new Entity(model.ToXml());
                            newEntity.Action = ObjectAction.Create;
                            newEntity.ParentEntityName = parentEntity.Name;

                            foreach (Int32 dimAttrId in dimAttrIdsOfLevel)
                            {
                                Attribute attribute = (Attribute)newEntity.GetAttribute(dimAttrId, GlobalizationHelper.GetSystemDataLocale());

                                if (attribute != null)
                                {
                                    attribute.SourceFlag = AttributeValueSource.Overridden;
                                    attribute.Action = ObjectAction.Create;

                                    Value val = new Value();
                                    val.AttrVal = row[dimAttrId.ToString()];
                                    val.Locale = GlobalizationHelper.GetSystemDataLocale();
                                    val.SetDisplayValue(row[dimAttrId + "DisplayValue"].ToString());

                                    if (attribute.IsCollection)
                                    {
                                        val.Sequence = 0;
                                    }

                                    //we set the action to ignore if RuleAttribute is optional and does not have any value
                                    foreach (EntityVariantLevel tempEntityHierarchyLevel in entityHierarchyDefinition.EntityVariantLevels)
                                    {
                                        EntityVariantRuleAttribute entityHierarchyRuleAttribute = tempEntityHierarchyLevel.GetRuleAttributeByTargetAttributeId(dimAttrId);

                                        if (entityHierarchyRuleAttribute != null)
                                        {
                                            if (entityHierarchyRuleAttribute.IsOptional &&
                                                ((val.AttrVal != null && String.IsNullOrWhiteSpace(val.AttrVal.ToString())) ||
                                                (entityHierarchyRuleAttribute.RuleAttribute.IsLookup && val.AttrVal != null && val.AttrVal.ToString() == "-1")))
                                            {
                                                attribute.Action = ObjectAction.Ignore;
                                                val.Action = ObjectAction.Ignore;
                                                break;
                                            }
                                        }
                                    }

                                    //Ref ID is required to save only for the attribute(s) having Display type LookUp
                                    //if (!newEntity.Attributes[dimAttrId].Name.Contains("Colour"))
                                    AttributeModel attrModel = (AttributeModel)_attributeModels.GetAttributeModel(dimAttrId, GlobalizationHelper.GetSystemDataLocale());

                                    if (attribute.Action != ObjectAction.Ignore && attrModel != null
                                        && attrModel.AttributeDisplayTypeName.ToLowerInvariant().Equals("lookuptable"))
                                    {
                                        Int32 refId = ValueTypeHelper.Int32TryParse(row[dimAttrId.ToString() + "RefId"].ToString(), -1);
                                        val.ValueRefId = refId;
                                        val.AttrVal = refId;
                                        val.InvariantVal = refId;
                                    }

                                    //newEntity.Attributes[dimAttrInfo.Key, dimAttrInfo.Value].OverriddenValues.Add(val);
                                    attribute.AppendValue(val, GlobalizationHelper.GetSystemDataLocale());
                                }
                                else
                                {
                                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, "Entity is not created for level " + entityHierarchyLevel.LongName + ". Attribute " + dimAttrId + " is not valid for this type of entity.", MDMTraceSource.EntityHierarchyProcess);
                                    ApplicationMessage message = messageBL.GetMessage("30026", this.SystemAttributeLocale.GetCultureName(), new object[] { entityHierarchyLevel.LongName, dimAttrId });
                                    operationResult.Errors.Add(new Error("", message.Message));
                                    newEntity = null;
                                    break;
                                }
                            }
                        }

                        if (newEntity != null)
                        {
                            PopulateDependentAttributeDetails(dimAttrIdsOfLevel, newEntity, parentEntity, _attributeModels);
                            entityList.Add(newEntity);
                        }
                    }
                }
            }
            finally
            {
                if (isTracingEnabled)
                {
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Total Entity Found : " + entityList.Count.ToString(), MDMTraceSource.EntityHierarchyProcess);
                    MDMTraceHelper.StopTraceActivity("EntityHierarchyBL.GetUniqueEntityCandidatesFromMatrix", MDMTraceSource.EntityHierarchyProcess);
                }
            }
            return entityList;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="matrixTableRow"></param>
        /// <param name="entity"></param>
        /// <param name="dimAttrId"></param>
        /// <param name="operationResult"></param>
        /// <param name="callerContext"></param>
        /// <returns></returns>
        private bool IsDimensionAttributeSame(Row matrixTableRow, Entity entity, Int32 dimAttrId, OperationResult operationResult, CallerContext callerContext)
        {
            Boolean isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;
            //dimensionAttribute.Key = AttributeId
            //dimensionAttribute.Value = Locale
            Boolean returnValue = false;

            String matrixValue = String.Empty;
            Int32 matrixRefId = -1;
            String entityValue = String.Empty;
            Int32 entityRefId = -1;

            Attribute attribute = (Attribute)entity.GetAttribute(dimAttrId, GlobalizationHelper.GetSystemDataLocale());
            if (attribute != null)
            {
                if (matrixTableRow[dimAttrId.ToString()] != null)
                {
                    matrixValue = matrixTableRow[dimAttrId.ToString()].ToString();
                }

                if (matrixTableRow[dimAttrId.ToString() + "RefId"] != null)
                {
                    matrixRefId = ValueTypeHelper.Int32TryParse(matrixTableRow[dimAttrId.ToString() + "RefId"].ToString(), -1);
                }

                if (attribute.CurrentValues.Count > 0)
                {
                    entityValue = attribute.CurrentValues[0].AttrVal == null ? String.Empty : attribute.CurrentValues[0].AttrVal.ToString();
                    entityRefId = attribute.CurrentValues[0].ValueRefId;
                }

                if (attribute.IsLookup && (matrixValue == "" || matrixValue == null))
                {
                    matrixValue = "-1";
                }

                //if attribute is missed then filling the local variable with the attribute passed to the current method
                LoadAttributeModels(new Collection<Int32>() { dimAttrId }, entity);

                AttributeModel attrModel = (AttributeModel)_attributeModels.GetAttributeModel(dimAttrId, GlobalizationHelper.GetSystemDataLocale());

                if (attrModel != null)
                {
                    if (attrModel.AttributeDisplayTypeName.ToLowerInvariant().Equals("lookuptable"))
                    {
                        if (matrixRefId.Equals(entityRefId))
                        {
                            returnValue = true;
                        }
                    }
                    else
                    {
                        if (matrixValue.Equals(entityValue))
                        {
                            returnValue = true;
                        }
                    }
                }
                else
                {
                    if (isTracingEnabled)
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Warning, "Dimension Attribute " + dimAttrId + " doesn't exist", MDMTraceSource.EntityHierarchyProcess);

                    ApplicationMessage message = messageBL.GetMessage("30027", this.SystemAttributeLocale.GetCultureName(), dimAttrId);
                    operationResult.Errors.Add(new Error("", message.Message));
                }
            }

            return returnValue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="matrixTable"></param>
        /// <param name="childEntities"></param>
        /// <param name="entityHierarchyDefinition"></param>
        /// <param name="operationResult"></param>
        /// <returns></returns>
        private Table CalculateStatusForMatrix(Table matrixTable, EntityCollection childEntities, EntityVariantDefinition entityHierarchyDefinition, OperationResult operationResult)
        {
            EntityVariantLevel leafLevel = entityHierarchyDefinition.EntityVariantLevels.OrderByDescending(l => l.Rank).FirstOrDefault();
            return this.CalculateStatus(matrixTable, childEntities, entityHierarchyDefinition, leafLevel.Id, operationResult);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="matrixTable"></param>
        /// <param name="childEntities"></param>
        /// <param name="ehDef"></param>
        /// <param name="or"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        private Table CalculateStatusForMatrixByContext(Table matrixTable, EntityCollection childEntities, EntityVariantDefinition ehDef, OperationResult or, CallerContext context)
        {
            EntityVariantLevel leafLevel = ehDef.EntityVariantLevels.OrderByDescending(l => l.Rank).FirstOrDefault();
            return this.CalculateStatus(matrixTable, childEntities, ehDef, leafLevel.Id, or, context);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="matrixTable"></param>
        /// <param name="childEntities"></param>
        /// <param name="entityHierarchyDefinition"></param>
        /// <param name="levelId"></param>
        /// <param name="operationResult"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        private Table CalculateStatus(Table matrixTable, EntityCollection childEntities, EntityVariantDefinition entityHierarchyDefinition, Int32 levelId, OperationResult operationResult, CallerContext context = null)
        {
            TraceSettings traceSettings = MDMOperationContextHelper.GetCurrentTraceSettings();
            DiagnosticActivity currentActivity = new DiagnosticActivity();

            if (traceSettings.IsTracingEnabled)
            {
                currentActivity.Start();
            }
            try
            {
                //Table finalTable = new Table();

                //  a. combinations which don't match to existing child entities, NEW
                //  b. combinations which match to existing child entities, EXISTING
                //  c. child entities which do not match to any combination, INVALID


                if (traceSettings.IsBasicTracingEnabled)
                    currentActivity.LogInformation("Creating columns");
                //create columns if do not exist
                if (!matrixTable.Columns.Contains("Status"))
                {
                    matrixTable.Columns.Add(new Column(0, "Status", "Status", "New"));
                }
                if (!matrixTable.Columns.Contains("Excluded"))
                {
                    matrixTable.Columns.Add(new Column(0, "Excluded", "Excluded", "False"));
                }

                Collection<Int64> existingEntityIdList = new Collection<Int64>();
                Int64 lastRowId = 0;
                Collection<KeyValuePair<Int32, LocaleEnum>> attributeIdList = new Collection<KeyValuePair<Int32, LocaleEnum>>();

                EntityVariantLevel entityHierarchyLevel = entityHierarchyDefinition.EntityVariantLevels.Where(level => level.Id == levelId).FirstOrDefault();
                EntityVariantLevel currentLevel = entityHierarchyLevel;
                DurationHelper durHelper = new DurationHelper(DateTime.Now);

                while (currentLevel != null)
                {
                    currentLevel.DimensionAttributes.ToList().ForEach(dimAttr => attributeIdList.Add(new KeyValuePair<Int32, LocaleEnum>(dimAttr.Id, this.SystemAttributeLocale)));
                    currentLevel = entityHierarchyDefinition.EntityVariantLevels.Where(l => l.Id == currentLevel.ParentLevelId).SingleOrDefault();

                    if (traceSettings.IsBasicTracingEnabled)
                        currentActivity.LogMessageWithDuration(MessageClassEnum.Information, null, "Getting current level, since the current level is empty.", durHelper.GetCumulativeTimeSpanInMilliseconds());
                }

                //validate if all dimension attributes are present in matrix table
                //TODO :: Do we consider Locale for MatrixTable?
                durHelper.ResetDuration();

                List<Int32> missedDimensionAttributesInTable = attributeIdList.Where(pair => !matrixTable.Columns.Contains(pair.Key.ToString())).Select(pair => pair.Key).ToList();

                if (traceSettings.IsBasicTracingEnabled)
                    currentActivity.LogMessageWithDuration(MessageClassEnum.Information, null, "Validate and Get Missed dimension Attrs from matrix table : Count : " + missedDimensionAttributesInTable.Count(), durHelper.GetCumulativeTimeSpanInMilliseconds());

                if (missedDimensionAttributesInTable.Count == 0)
                {
                    if (traceSettings.IsBasicTracingEnabled)
                        currentActivity.LogInformation("Loop : Added Missed dimension attributes : Matrix Table Row count : " + matrixTable.Rows.Count());

                    durHelper.ResetDuration();
                    //go through each row in table
                    foreach (Row row in matrixTable.Rows)
                    {
                        String status = "New";

                        if (traceSettings.IsBasicTracingEnabled)
                            currentActivity.LogInformation("    Loop2 : Child Entities : Count : " + childEntities.Count());

                        DurationHelper durHelper2 = new DurationHelper(DateTime.Now);
                        //we'll try matching the row with entity
                        foreach (Entity childEntity in childEntities)
                        {
                            Boolean foundMatchingEntity = true;

                            if (traceSettings.IsBasicTracingEnabled)
                                currentActivity.LogInformation("        Loop3 :  Match Dimension Attribute  : " + attributeIdList.Count());

                            durHelper2.ResetDuration();
                            //match each dimension attribute
                            foreach (KeyValuePair<Int32, LocaleEnum> attrInfo in attributeIdList)
                            {
                                if (!IsDimensionAttributeSame(row, childEntity, attrInfo.Key, operationResult, context))
                                {
                                    //whenever there's a mismatch in even one attribute, say NO
                                    foundMatchingEntity = false;
                                }
                            }

                            if (traceSettings.IsBasicTracingEnabled)
                                currentActivity.LogMessageWithDuration(MessageClassEnum.Information, null, "        Loop3 : Match Dimension Attribute :  Finished", durHelper2.GetCumulativeTimeSpanInMilliseconds());

                            //if current entity is matching, we'll do some stuff
                            if (foundMatchingEntity)
                            {
                                if (traceSettings.IsBasicTracingEnabled)
                                    currentActivity.LogInformation("Found Matching Entity, Processing...");

                                //first set status as Existing
                                status = "Existing";
                                //add in existing entities list
                                existingEntityIdList.Add(childEntity.Id);

                                row["EntityId"] = childEntity.Id;

                                //then figure out Excluded flag from Entity and put in table's row
                                Int32 excludedAttributeId = (Int32)SystemAttributes.Excluded;
                                if (childEntity.Attributes.Contains(excludedAttributeId, this.SystemAttributeLocale))
                                {
                                    if (traceSettings.IsBasicTracingEnabled)
                                        currentActivity.LogInformation("Found excluded attribute in the entity.  Processing ... ");

                                    String excluded = "False";
                                    if (childEntity.Attributes[excludedAttributeId, this.SystemAttributeLocale].CurrentValues.Count > 0)
                                    {
                                        if (childEntity.Attributes[excludedAttributeId, this.SystemAttributeLocale].CurrentValues[0].AttrVal != null)
                                            excluded = childEntity.Attributes[excludedAttributeId, this.SystemAttributeLocale].CurrentValues[0].AttrVal.ToString();
                                    }

                                    if (matrixTable.Columns.Contains("Excluded"))
                                    {
                                        if (traceSettings.IsBasicTracingEnabled)
                                            currentActivity.LogInformation("Adding excluded to the excluded row. : Value : " + excluded);

                                        row["Excluded"] = excluded;
                                    }
                                    else
                                    {

                                        if (traceSettings.IsBasicTracingEnabled)
                                            currentActivity.LogInformation("Cannot find Excluded column in Matrix Table");
                                    }
                                }
                                else
                                {
                                    if (traceSettings.IsBasicTracingEnabled)
                                        currentActivity.LogInformation("Cannot find excluded attribute in entity with Id : " + childEntity.Id);
                                }

                                break; //end the loop once we got the exact matching entity
                            }
                        }

                        if (traceSettings.IsBasicTracingEnabled)
                            currentActivity.LogInformation("    Loop2 : Child Entities : Finished ");
                        //if matching entity found, status will be Existing otherwise New
                        row["Status"] = status;
                        if (lastRowId < row.Id)
                        {
                            lastRowId = row.Id; //used later
                        }
                    }

                    if (traceSettings.IsBasicTracingEnabled)
                        currentActivity.LogMessageWithDuration(MessageClassEnum.Information, null, "Loop : Added Missed dimension attributes", durHelper.GetCumulativeTimeSpanInMilliseconds());


                    if (traceSettings.IsBasicTracingEnabled)
                        currentActivity.LogInformation("Populating existing invalid entities.");

                    durHelper.ResetDuration();
                    //transfer existing invalid entities into matrix table
                    EntityCollection invalidEntities = new EntityCollection();
                    invalidEntities = new EntityCollection(childEntities.Where(childEntity => !existingEntityIdList.Contains(childEntity.Id)).ToList());

                    if (traceSettings.IsBasicTracingEnabled)
                        currentActivity.LogInformation("Loop: Invalid Entitites");

                    foreach (Entity invalidEntity in invalidEntities)
                    {
                        Row invalidRow = matrixTable.NewRow(++lastRowId);
                        invalidRow["Status"] = "Invalid";
                        invalidRow["Excluded"] = "False";
                        invalidRow["EntityId"] = invalidEntity.Id;

                        if (entityHierarchyDefinition.EntityVariantLevels != null && entityHierarchyDefinition.EntityVariantLevels.Count > 0)
                        {
                            if (traceSettings.IsBasicTracingEnabled)
                                currentActivity.LogInformation("    Loop2 : Definition.EntityHierarchyLevels");

                            foreach (EntityVariantLevel ehLevel in entityHierarchyDefinition.EntityVariantLevels)
                            {
                                if (traceSettings.IsBasicTracingEnabled)
                                    currentActivity.LogInformation("        Loop3 : EH Level. DimensionAttributes");

                                foreach (Attribute dimensionAttribute in ehLevel.DimensionAttributes)
                                {
                                    Attribute attribute = (Attribute)invalidEntity.GetAttribute(dimensionAttribute.Id, this.SystemAttributeLocale);

                                    if (attribute.IsLookup)
                                    {
                                        invalidRow[dimensionAttribute.Id.ToString()] = attribute.CurrentValues[0].ValueRefId;
                                        invalidRow[dimensionAttribute.Id.ToString() + "RefId"] = attribute.CurrentValues[0].ValueRefId;
                                    }
                                    else
                                    {
                                        invalidRow[dimensionAttribute.Id.ToString()] = attribute.CurrentValues[0].AttrVal;
                                    }
                                }
                                invalidRow["Level" + ehLevel.Id.ToString() + "GroupId"] = 0;
                                if (traceSettings.IsBasicTracingEnabled)
                                    currentActivity.LogInformation("        Loop3 : EH Level.DimensionAttributes : Finished");
                            }

                            if (traceSettings.IsBasicTracingEnabled)
                                currentActivity.LogInformation("    Loop2 : Definition.EntityHierarchyLevels");
                        }

                        Int32 excludedAttributeId = (Int32)SystemAttributes.Excluded;
                        if (invalidEntity.Attributes.Contains(excludedAttributeId, this.SystemAttributeLocale))
                        {
                            if (traceSettings.IsBasicTracingEnabled)
                                currentActivity.LogInformation("Finding and processing excluded attribute in invalid attributes");

                            String excluded = "False";
                            if (invalidEntity.Attributes[excludedAttributeId, this.SystemAttributeLocale].CurrentValues.Count > 0)
                            {
                                Object value = invalidEntity.Attributes[excludedAttributeId, this.SystemAttributeLocale].CurrentValues[0].AttrVal;

                                if (value != null)
                                {
                                    excluded = value.ToString();
                                }
                            }
                            if (matrixTable.Columns.Contains("Excluded"))
                            {
                                invalidRow["Excluded"] = excluded;
                            }
                            else
                            {
                                if (traceSettings.IsBasicTracingEnabled)
                                    currentActivity.LogInformation("Entity Hierarchy: Can not find Excluded column in matrix table.");
                            }
                        }
                        else
                        {
                            currentActivity.LogWarning("Entity Hierarchy: Can not find Excluded attribute in Entity with Id=" + invalidEntity.Id.ToString());
                        }
                    }
                    if (traceSettings.IsBasicTracingEnabled)
                        if (traceSettings.IsBasicTracingEnabled)
                            currentActivity.LogMessageWithDuration(MessageClassEnum.Information, null, "Populating existing invalid entities : Finished", durHelper.GetCumulativeTimeSpanInMilliseconds());
                }
                else
                {
                    //Add Error in OP
                    string str = missedDimensionAttributesInTable.Select(item => item.ToString()).ToString();
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, "Attributes " + str + " are missing in matrix table", MDMTraceSource.EntityHierarchyProcess);
                    ApplicationMessage message = new ApplicationMessage();
                    message = messageBL.GetMessage("30023", this.SystemAttributeLocale.GetCultureName(), str);
                    operationResult.Errors.Add(new Error("", message.Message));
                    currentActivity.LogError(message.Message);
                }
            }
            finally
            {
                if (traceSettings.IsTracingEnabled)
                    currentActivity.Stop();
            }

            return matrixTable;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityId"></param>
        /// <param name="ehdef"></param>
        /// <param name="or"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        private EntityCollection GetChildEntitiesForMatrixByContext(Int64 entityId, EntityVariantDefinition ehdef, OperationResult or, CallerContext context)
        {
            TraceSettings traceSettings = MDMOperationContextHelper.GetCurrentTraceSettings();

            DiagnosticActivity currentActivity = new DiagnosticActivity();

            if (traceSettings.IsTracingEnabled)
            {
                ExecutionContext ec = new ExecutionContext();

                if (context != null)
                {
                    ec.CallerContext = context;
                    currentActivity.Start(ec);
                }
                else
                    currentActivity.Start();
            }

            EntityCollection childEntities = new EntityCollection();
            try
            {
                Int32 excludedAttributeId = (Int32)SystemAttributes.Excluded;

                if (ehdef != null)
                {
                    if (ehdef.EntityVariantLevels.Count() == 0)
                    {
                        throw new ArgumentException("Entity Hierarchy Definition level count is zero");
                    }

                    EntityVariantLevel leafLevel = ehdef.EntityVariantLevels.OrderByDescending(l => l.Rank).FirstOrDefault();

                    if (leafLevel == null)
                        throw new ArgumentException("Not able to get the leaf level of the hierarchy based on rank");

                    if (traceSettings.IsBasicTracingEnabled)
                        currentActivity.LogInformation("Got leaf hierarchy level based on Rank");

                    Collection<Int32> dimAttributeIds = new Collection<Int32>();

                    if (traceSettings.IsBasicTracingEnabled)
                        currentActivity.LogInformation("Loop : EH Def.EntityHierarchyLevels : Count : " + ehdef.EntityVariantLevels.Count());
                    DurationHelper durHelper = new DurationHelper(DateTime.Now);

                    foreach (EntityVariantLevel level in ehdef.EntityVariantLevels)
                    {
                        if (level.DimensionAttributes != null && level.DimensionAttributes.Count > 0)
                        {
                            foreach (Attribute dimAttr in level.DimensionAttributes)
                            {
                                dimAttributeIds.Add(dimAttr.Id);
                            }
                        }
                    }

                    if (traceSettings.IsBasicTracingEnabled)
                        currentActivity.LogMessageWithDuration(MessageClassEnum.Information, null, "Loop : EH Def. EntityHierarchyLevels : Finished", durHelper.GetCumulativeTimeSpanInMilliseconds());

                    //TODO :: Verify  LocaleEnum.en_WW is ok for SystemAttributes
                    dimAttributeIds.Add(excludedAttributeId);

                    childEntities = GetEntitiesByEntityType(entityId, leafLevel.EntityTypeId, dimAttributeIds, context.Application, context.Module);
                }
            }
            finally
            {
                if (traceSettings.IsTracingEnabled)
                {
                    currentActivity.LogInformation("Total childEntities found = " + childEntities.Count);
                    currentActivity.Stop();
                }
            }

            return childEntities;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityId"></param>
        /// <param name="entityHierarchyDefinition"></param>
        /// <param name="operationResult"></param>
        /// <param name="application"></param>
        /// <param name="module"></param>
        /// <returns></returns>
        private EntityCollection GetChildEntitiesForMatrix(Int64 entityId, EntityVariantDefinition entityHierarchyDefinition, OperationResult operationResult, MDMCenterApplication application, MDMCenterModules module)
        {
            Boolean isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;
            if (isTracingEnabled)
                MDMTraceHelper.StartTraceActivity("EntityHierarchyBL.GetChildEntitiesForMatrix", MDMTraceSource.EntityHierarchyGet, false);

            EntityCollection childEntities = new EntityCollection();
            try
            {
                Int32 excludedAttributeId = (Int32)SystemAttributes.Excluded;
                EntityVariantLevel leafLevel = entityHierarchyDefinition.EntityVariantLevels.OrderByDescending(l => l.Rank).FirstOrDefault();

                Collection<Int32> dimAttributeIds = new Collection<Int32>();

                foreach (EntityVariantLevel level in entityHierarchyDefinition.EntityVariantLevels)
                {
                    if (level.DimensionAttributes != null && level.DimensionAttributes.Count > 0)
                    {
                        foreach (Attribute dimAttr in level.DimensionAttributes)
                        {
                            dimAttributeIds.Add(dimAttr.Id);
                        }
                    }
                }

                //TODO :: Verify  LocaleEnum.en_WW is ok for SystemAttributes
                dimAttributeIds.Add(excludedAttributeId);

                if (leafLevel != null)
                {
                    childEntities = GetEntitiesByEntityType(entityId, leafLevel.EntityTypeId, dimAttributeIds, application, module);
                }
                else
                {
                    //log this case
                }
            }
            finally
            {
                if (isTracingEnabled)
                {
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Total childEntities found = " + childEntities.Count.ToString(), MDMTraceSource.EntityHierarchyGet);
                    MDMTraceHelper.StopTraceActivity("EntityHierarchyBL.GetChildEntitiesForMatrix", MDMTraceSource.EntityHierarchyGet);
                }
            }

            return childEntities;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityHierarchyLevel"></param>
        /// <param name="table"></param>
        private void FillHierarchyLevelDimensionValues(EntityVariantLevel entityHierarchyLevel, Table table)
        {
            TraceSettings traceSettings = MDMOperationContextHelper.GetCurrentTraceSettings();
            DiagnosticActivity currentActivity = new DiagnosticActivity();

            if (traceSettings.IsBasicTracingEnabled)
                currentActivity.Start();
            try
            {
                Table dimensionValues = new Table();

                if (traceSettings.IsBasicTracingEnabled)
                    currentActivity.LogInformation("Loop:  Dimension attributes : Count : " + entityHierarchyLevel.DimensionAttributes.Count());

                if (entityHierarchyLevel.DimensionAttributes.Count < 1)
                {
                    foreach (EntityVariantRuleAttribute evRuleAttribute in entityHierarchyLevel.RuleAttributes)
                    {
                        if (evRuleAttribute.TargetAttributeId > 0)
                        {
                            Attribute attribute = new Attribute();
                            attribute.Id = evRuleAttribute.TargetAttributeId;
                            attribute.Name = evRuleAttribute.TargetAttributeName;
                            attribute.LongName = evRuleAttribute.TargetAttributeLongName;
                            attribute.IsLookup = true;

                            entityHierarchyLevel.DimensionAttributes.Add(attribute);
                        }
                    }
                }

                foreach (Attribute attr in entityHierarchyLevel.DimensionAttributes)
                {
                    Column _column = new Column();
                    _column.Id = attr.Id;
                    _column.Name = attr.Id.ToString();
                    _column.LongName = attr.LongName.ToString();
                    dimensionValues.Columns.Add(_column);

                    Column _columnLookupRef = new Column();
                    _columnLookupRef.Name = attr.Id.ToString() + "RefId";
                    dimensionValues.Columns.Add(_columnLookupRef);
                }

                if (traceSettings.IsBasicTracingEnabled)
                    currentActivity.LogInformation("Loop:  Dimension attributes : Finished");

                Column _columnGroupId = new Column();
                _columnGroupId.Name = "GroupId";
                dimensionValues.Columns.Add(_columnGroupId);

                Column _columnLocale = new Column();
                _columnLocale.Name = "FK_Locale";
                dimensionValues.Columns.Add(_columnLocale);

                Int32 PreviousGroupID = -1;
                Row _row = new Row();

                if (traceSettings.IsBasicTracingEnabled)
                    currentActivity.LogInformation("Loop:  Table Rows : Count : " + table.Rows.Count());

                foreach (Row row in table.Rows)
                {
                    String levelId = row["EntityHierarchy_LevelId"].ToString();
                    if (levelId.Equals(entityHierarchyLevel.Id.ToString()))
                    {
                        int GroupID;
                        Int32.TryParse(row["GroupId"].ToString(), out GroupID);

                        if (PreviousGroupID == -1 || GroupID != PreviousGroupID)
                        {
                            _row = null;
                            _row = dimensionValues.NewRow(Convert.ToInt32(row["GroupId"]));
                        }

                        _row.Name = row["GroupId"].ToString();

                        String attr = row["EntityHierarchy_AttributeId"].ToString();
                        _row[attr] = row["AttrVal"].ToString();
                        _row[attr + "RefId"] = row["LookupRefId"].ToString();
                        _row["GroupId"] = GroupID;
                        _row["FK_Locale"] = row["FK_Locale"].ToString();
                        PreviousGroupID = GroupID;
                    }
                }

                if (traceSettings.IsBasicTracingEnabled)
                    currentActivity.LogInformation("Loop:  Table Rows : Finished.");

                entityHierarchyLevel.DimensionValues = dimensionValues;
            }
            finally
            {
                if (traceSettings.IsBasicTracingEnabled)
                    currentActivity.Stop();
            }
        }

        /// <summary>
        /// Cartesian operation for rule attribute values to generate the dimensions for a level
        /// </summary>
        /// <param name="attributes">Rule attributes with values</param>
        /// <param name="entityHierarchyLevel">Level to put dimension in</param>
        /// <param name="context">Caller context</param>
        /// <returns>Table of dimension values</returns>
        private Table CalculateDimensionsCartesian(EntityVariantRuleAttributeCollection attributes, EntityVariantLevel entityHierarchyLevel, CallerContext context = null)
        {
            TraceSettings traceSettings = MDMOperationContextHelper.GetCurrentTraceSettings();

            DiagnosticActivity currentActivity = new DiagnosticActivity();

            if (traceSettings.IsTracingEnabled)
            {
                ExecutionContext ec = new ExecutionContext();

                if (context != null)
                {
                    ec.CallerContext = context;
                    currentActivity.Start(ec);
                }
                else
                    currentActivity.Start();
            }

            try
            {
                //N level Cartesian, multiply each new value with all existing sets

                Table table = new Table();

                #region Create Columns

                if (traceSettings.IsBasicTracingEnabled)
                    currentActivity.LogInformation("Loop: EntityHierarchyRuleAttributeCollection : Count : " + attributes.Count());

                DurationHelper durHelper = new DurationHelper(DateTime.Now);

                foreach (EntityVariantRuleAttribute ruleAttr in attributes)
                {
                    Column _column = new Column();
                    Attribute attribute = new Attribute();
                    attribute = (from dimAttr in entityHierarchyLevel.DimensionAttributes
                                 where dimAttr.Id == ruleAttr.TargetAttributeId
                                 select dimAttr).FirstOrDefault(); //getDimensionAttributeById(attr.Value,entityHierarchyLevel);

                    if (attribute == null)
                    {
                        if (ruleAttr.TargetAttributeId < 1)
                        {
                            currentActivity.LogError("Can not find Dimension Attribute with Id=" + ruleAttr.TargetAttributeId);
                            throw new Exception("Can not find Dimension Attribute with Id=" + ruleAttr.TargetAttributeId);
                        }
                        else
                        {
                            attribute = new Attribute();
                            attribute.Id = ruleAttr.TargetAttributeId;
                            attribute.Name = ruleAttr.TargetAttributeName;
                            attribute.LongName = ruleAttr.TargetAttributeLongName;

                            entityHierarchyLevel.DimensionAttributes.Add(attribute);
                        }
                    }



                    _column.Id = attribute.Id;
                    _column.Name = attribute.Id.ToString();
                    _column.LongName = attribute.LongName;
                    table.Columns.Add(_column);

                    Column _columnLookupRef = new Column();
                    _columnLookupRef.Name = attribute.Id.ToString() + "RefId";
                    _columnLookupRef.LongName = attribute.Id.ToString() + "RefId";
                    table.Columns.Add(_columnLookupRef);
                }

                if (traceSettings.IsBasicTracingEnabled)
                    currentActivity.LogMessageWithDuration(MessageClassEnum.Information, null, "Loop: EntityHierarchyRuleAttributeCollection : Finished", durHelper.GetCumulativeTimeSpanInMilliseconds());

                Column _columnGroupId = new Column();
                _columnGroupId.Name = "GroupId";
                table.Columns.Add(_columnGroupId);

                Column _columnLocale = new Column();
                _columnLocale.Name = "FK_Locale";
                table.Columns.Add(_columnLocale);

                if (traceSettings.IsBasicTracingEnabled)
                    currentActivity.LogInformation("Column creation done");

                #endregion Create Columns

                #region Create Rows (Cartesian)

                Boolean isFirstAttr = true;

                if (traceSettings.IsBasicTracingEnabled)
                    currentActivity.LogInformation("Started creating rows");

                durHelper.ResetDuration();

                //This filter is for optional rule attribute
                //Use only those rule attribute which are marked as optional = false and attributes which are marked as optional = true and are having value.
                EntityVariantRuleAttributeCollection filteredRuleAttributes = new EntityVariantRuleAttributeCollection((from ruleAttr in attributes
                                                                                                                        where !(ruleAttr.IsOptional == true && ruleAttr.RuleAttribute.CurrentValues.Count < 1)

                                                                                                                        select ruleAttr).ToList<EntityVariantRuleAttribute>());

                if (traceSettings.IsBasicTracingEnabled)
                    currentActivity.LogMessageWithDuration(MessageClassEnum.Information, null, "Time to create filteredRuleAttrs using Linq", durHelper.GetCumulativeTimeSpanInMilliseconds());

                if (traceSettings.IsBasicTracingEnabled)
                    currentActivity.LogInformation("Loop : Filtered Rule Attrs for validation: Count : " + filteredRuleAttributes.Count());

                durHelper.ResetDuration();

                Boolean noValueFound = true;
                foreach (EntityVariantRuleAttribute EHruleAttr in filteredRuleAttributes)
                {
                    if (EHruleAttr.RuleAttribute.SourceFlag == AttributeValueSource.Overridden)
                    {
                        IValueCollection values = EHruleAttr.RuleAttribute.GetCurrentValues();
                        if (values != null && values.Count > 0 && values.FirstOrDefault().AttrVal != null && values.FirstOrDefault().AttrVal.ToString() != "")
                        {
                            noValueFound = false;
                            break;
                        }
                    }
                }

                if (traceSettings.IsBasicTracingEnabled)
                    currentActivity.LogMessageWithDuration(MessageClassEnum.Information, null, "Loop : Filtered Rule Attrs for Validation Context Finished", durHelper.GetCumulativeTimeSpanInMilliseconds());

                if (noValueFound)
                {
                    throw new Exception("Please select atleast one item to proceed.");
                }

                if (traceSettings.IsBasicTracingEnabled)
                    currentActivity.LogInformation("Loop : Filtered Rule Attrs for processing: Count : " + filteredRuleAttributes.Count());

                durHelper.ResetDuration();

                foreach (EntityVariantRuleAttribute ruleAttr in filteredRuleAttributes)
                {
                    Int32 rowCounter = 1;
                    RowCollection rows = new RowCollection(table.Rows.ToList<Row>());
                    table.Rows.Clear();

                    if (isFirstAttr)
                    {
                        if (traceSettings.IsBasicTracingEnabled)
                            currentActivity.LogInformation("   Loop2 FirstAttribute  :ruleAttr.RuleAttribute.CurrentValues: Count : " + ruleAttr.RuleAttribute.CurrentValues.Count());

                        foreach (Value val in ruleAttr.RuleAttribute.CurrentValues)
                        {
                            Row row = table.NewRow(rowCounter);
                            row[ruleAttr.TargetAttributeId] = val.AttrVal;
                            row[ruleAttr.TargetAttributeId + "RefId"] = val.ValueRefId;
                            row["GroupId"] = rowCounter;
                            row["FK_Locale"] = (Int32)this.SystemAttributeLocale;

                            rowCounter++;
                        }

                        if (traceSettings.IsBasicTracingEnabled)
                            currentActivity.LogInformation("    Loop2 FirstAttribute  :ruleAttr.RuleAttribute.CurrentValues finished");

                        isFirstAttr = false;
                    }
                    else
                    {
                        if (traceSettings.IsBasicTracingEnabled)
                            currentActivity.LogInformation("    Loop2 NotFirstAttribute  :ruleAttr.RuleAttribute.CurrentValues: Count : " + ruleAttr.RuleAttribute.CurrentValues.Count());

                        foreach (Value val in ruleAttr.RuleAttribute.CurrentValues)
                        {
                            foreach (Row row in rows)
                            {
                                Row copyRow = new Row(row.ToXml());

                                copyRow[ruleAttr.TargetAttributeId] = val.AttrVal;
                                copyRow[ruleAttr.TargetAttributeId + "RefId"] = val.ValueRefId;
                                copyRow["GroupId"] = rowCounter;
                                copyRow.Id = rowCounter;

                                table.Rows.Add(copyRow);
                                rowCounter++;
                            }
                        }

                        if (traceSettings.IsBasicTracingEnabled)
                            currentActivity.LogInformation("    Loop2 NotFirstAttribute  :ruleAttr.RuleAttribute.CurrentValues finished");
                    }
                }

                if (traceSettings.IsBasicTracingEnabled)
                    currentActivity.LogMessageWithDuration(MessageClassEnum.Information, null, "Loop : Filtered Rule Attrs for processing finished", durHelper.GetCumulativeTimeSpanInMilliseconds());

                #endregion Create Rows (Cartesian)

                return table;
            }
            finally
            {
                if (traceSettings.IsTracingEnabled)
                {
                    currentActivity.Stop();
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="definition"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        private Table CalculateMatrixCartesian(EntityVariantDefinition definition, CallerContext context = null)
        {
            TraceSettings traceSettings = MDMOperationContextHelper.GetCurrentTraceSettings();
            DiagnosticActivity currentActivity = new DiagnosticActivity();

            if (traceSettings.IsTracingEnabled)
            {
                if (context != null)
                {
                    ExecutionContext ec = new ExecutionContext();
                    ec.CallerContext = context;
                    currentActivity.Start(ec);
                }
                else
                    currentActivity.Start();
            }

            try
            {
                Table itemMatrix = new Table();

                if (definition != null)
                {

                    #region Create Columns

                    if (traceSettings.IsBasicTracingEnabled)
                        currentActivity.LogInformation("Loop : Creating columns : definition.EntityHierarchyLevels : Count : " + definition.EntityVariantLevels.Count());
                    DurationHelper durHelper = new DurationHelper(DateTime.Now);

                    foreach (EntityVariantLevel level in definition.EntityVariantLevels)
                    {
                        Table dimensionValue = level.DimensionValues;

                        if (traceSettings.IsBasicTracingEnabled)
                            currentActivity.LogInformation("    Loop2 : Creating columns : level.RuleAttributes : Count : " + definition.EntityVariantLevels.Count());

                        foreach (EntityVariantRuleAttribute ruleAttr in level.RuleAttributes)
                        {
                            Column _column = new Column();
                            Attribute attribute = new Attribute();
                            attribute = (from dimAttr in level.DimensionAttributes
                                         where dimAttr.Id == ruleAttr.TargetAttributeId
                                         select dimAttr).FirstOrDefault();//getDimensionAttributeById(attr.Value,entityHierarchyLevel);

                            if (attribute == null)
                            {
                                if (ruleAttr.TargetAttributeId < 1)
                                {
                                    throw new Exception("Can not find Dimension Attribute with Id=" + ruleAttr.TargetAttributeId);
                                }
                                else
                                {
                                    attribute = new Attribute();
                                    attribute.Id = ruleAttr.TargetAttributeId;
                                    attribute.Name = ruleAttr.TargetAttributeName;
                                    attribute.LongName = ruleAttr.TargetAttributeLongName;

                                    level.DimensionAttributes.Add(attribute);
                                }
                            }

                            if (!itemMatrix.Columns.Contains(attribute.Id.ToString()))
                            {
                                _column.Id = attribute.Id;
                                _column.Name = attribute.Id.ToString();
                                _column.LongName = attribute.LongName;
                                itemMatrix.Columns.Add(_column);
                            }

                            if (!itemMatrix.Columns.Contains(attribute.Id.ToString() + "RefId"))
                            {
                                Column _columnLookupRef = new Column();
                                _columnLookupRef.Name = attribute.Id.ToString() + "RefId";
                                _columnLookupRef.LongName = attribute.Id.ToString() + "RefId";
                                itemMatrix.Columns.Add(_columnLookupRef);
                            }

                            if (!itemMatrix.Columns.Contains(attribute.Id.ToString() + "DisplayValue"))
                            {
                                Column displayValuecolumn = new Column();
                                displayValuecolumn.Name = attribute.Id.ToString() + "DisplayValue";
                                displayValuecolumn.LongName = attribute.Id.ToString() + "DisplayValue";
                                itemMatrix.Columns.Add(displayValuecolumn);
                            }
                        }

                        if (traceSettings.IsBasicTracingEnabled)
                            currentActivity.LogMessageWithDuration(MessageClassEnum.Information, null, "    Loop2 : Creating Columns : Finished", durHelper.GetCumulativeTimeSpanInMilliseconds());

                        if (!itemMatrix.Columns.Contains("Level" + level.Id.ToString() + "GroupId"))
                        {
                            Column column = new Column();
                            column.Name = "Level" + level.Id.ToString() + "GroupId";
                            column.LongName = level.LongName + " Dimension Id";
                            itemMatrix.Columns.Add(column);
                        }
                    }

                    if (traceSettings.IsBasicTracingEnabled)
                        currentActivity.LogInformation("Loop : Creating columns : definition.EntityHierarchyLevels : Finished");

                    if (!itemMatrix.Columns.Contains("Excluded"))
                    {
                        Column Excludedcolumn = new Column();
                        Excludedcolumn.Name = "Excluded";
                        Excludedcolumn.LongName = "Excluded";
                        itemMatrix.Columns.Add(Excludedcolumn);
                    }

                    if (!itemMatrix.Columns.Contains("Status"))
                    {
                        Column Statuscolumn = new Column();
                        Statuscolumn.Name = "Status";
                        Statuscolumn.LongName = "Status";
                        itemMatrix.Columns.Add(Statuscolumn);
                    }

                    if (!itemMatrix.Columns.Contains("EntityId"))
                    {
                        Column EntityIdcolumn = new Column();
                        EntityIdcolumn.Name = "EntityId";
                        EntityIdcolumn.LongName = "EntityId";
                        itemMatrix.Columns.Add(EntityIdcolumn);
                    }

                    #endregion Create Columns

                    #region Create Rows (Cartesian)

                    if (traceSettings.IsBasicTracingEnabled)
                        currentActivity.LogInformation("Loop : Creating Rows : definition.EntityHierarchyLevels : Count : " + definition.EntityVariantLevels.Count());
                    durHelper.ResetDuration();

                    Boolean isFirstLevel = true;
                    foreach (EntityVariantLevel level in definition.EntityVariantLevels)
                    {
                        Int32 rowCounter = 1;
                        RowCollection rows = new RowCollection(itemMatrix.Rows.ToList<Row>());
                        itemMatrix.Rows.Clear();

                        if (isFirstLevel)
                        {
                            if (traceSettings.IsBasicTracingEnabled)
                                currentActivity.LogInformation("    Loop2: Creating First Level.  Dimension Row Count : " + level.DimensionValues.Rows.Count());

                            foreach (Row dimRow in level.DimensionValues.Rows)
                            {
                                Row itemRow = itemMatrix.NewRow(rowCounter);
                                if (traceSettings.IsBasicTracingEnabled)
                                    currentActivity.LogInformation("        Loop3: Creating First Level.  Dimension Attribute Count : " + level.DimensionAttributes.Count());

                                foreach (Attribute attr in level.DimensionAttributes)
                                {
                                    itemRow[attr.Id] = dimRow[attr.Id];
                                    itemRow[attr.Id + "RefId"] = dimRow[attr.Id + "RefId"];
                                    itemRow[attr.Id + "DisplayValue"] = String.Empty;
                                }

                                if (traceSettings.IsBasicTracingEnabled)
                                    currentActivity.LogInformation("        Loop3: Creating First Level.  Dimension Attribute : Finished");

                                itemRow["Level" + level.Id.ToString() + "GroupId"] = dimRow["GroupId"];
                                itemRow["Excluded"] = "False";
                                itemRow["Status"] = "New";
                                itemRow["EntityId"] = "0";
                                rowCounter++;

                            }
                            isFirstLevel = false;
                            if (traceSettings.IsBasicTracingEnabled)
                                currentActivity.LogInformation("    Loop2: Creating First Level.  Finished");
                        }
                        else
                        {
                            if (traceSettings.IsBasicTracingEnabled)
                                currentActivity.LogInformation("    Loop2: Creating Rest of the Levels.  Row Count : " + level.DimensionValues.Rows.Count());

                            foreach (Row dimRow in level.DimensionValues.Rows)
                            {
                                if (traceSettings.IsBasicTracingEnabled)
                                    currentActivity.LogInformation("        Loop3: Existing Row Collection  : Count : " + rows.Count());

                                foreach (Row existingRow in rows)
                                {
                                    Row copyRow = new Row(existingRow.ToXml());

                                    if (traceSettings.IsBasicTracingEnabled)
                                        currentActivity.LogInformation("            Loop4:  Add dimension attributes.  Dimension Attribute Count : " + level.DimensionAttributes.Count());
                                    foreach (Attribute attr in level.DimensionAttributes)
                                    {
                                        copyRow[attr.Id] = dimRow[attr.Id];
                                        copyRow[attr.Id + "RefId"] = dimRow[attr.Id + "RefId"];
                                    }

                                    if (traceSettings.IsBasicTracingEnabled)
                                        currentActivity.LogInformation("            Loop4: Add dimension attributes : Finished");

                                    copyRow["Level" + level.Id.ToString() + "GroupId"] = dimRow["GroupId"];
                                    copyRow.Id = rowCounter;

                                    itemMatrix.Rows.Add(copyRow);
                                    rowCounter++;
                                }

                                if (traceSettings.IsBasicTracingEnabled)
                                    currentActivity.LogInformation("        Loop3: Existing Row Collection : Finished");
                            }

                            if (traceSettings.IsBasicTracingEnabled)
                                currentActivity.LogInformation("    Loop2: Creating Rest of the Levels : Finished");
                        }
                    }

                    if (traceSettings.IsBasicTracingEnabled)
                        currentActivity.LogMessageWithDuration(MessageClassEnum.Information, null, "Loop: Creating Rows : Finished", durHelper.GetCumulativeTimeSpanInMilliseconds());

                    #endregion Create Rows (Cartesian)

                }

                return itemMatrix;
            }
            finally
            {
                if (traceSettings.IsTracingEnabled)
                    currentActivity.Stop();
            }
        }

        private Boolean LoadAttributeModels(Collection<Int32> attributeIdList, Entity entity, String levelLongName = "", OperationResult operationResult = null)
        {
            //1.check in private variables, and find out Ids which we don't have in models

            //2.get models for all missing ones

            //3.append newly loaded models into private variable

            Boolean status = true;

            AttributeModelBL modelManager = null;

            Collection<Int32> missingAttributeIds = new Collection<Int32>();

            //For the FIrst time the missed attributes list would be empty,So setting the default attributes(which is passed as parameter)
            if (_attributeModels.Count > 0)
            {
                foreach (Int32 attrId in attributeIdList)
                {
                    if (!_attributeModels.Contains(attrId, this.SystemAttributeLocale))
                    {
                        //Key = AttributeId
                        //Value = Locale for that attributeId
                        missingAttributeIds.Add(attrId);
                    }
                }
            }
            else
            {
                missingAttributeIds = attributeIdList;
            }

            //If any missed dimension attributes model is missed,updating the local variable attributeModels
            if (missingAttributeIds.Count > 0)
            {
                modelManager = new AttributeModelBL();

                //To DO now Container and Entity Type are setting from the entity object
                //TODO :: AttributeModelContext.Locales : Passing multiple locale value form EntityContext to AttributeModelContext
                AttributeModelContext attributeModelContext = new AttributeModelContext(entity.ContainerId, entity.EntityTypeId, 0, entity.CategoryId, new Collection<LocaleEnum>() { this.SystemAttributeLocale }, 0, AttributeModelType.All, false, false, true);
                AttributeModelCollection missedmodels = modelManager.Get(missingAttributeIds, null, null, attributeModelContext);
                Collection<Int32> missedAttributeIds = missedmodels.GetAttributeIdList();

                if (missedmodels != null && missedmodels.Count > 0)
                {
                    if (missedmodels.Count != missingAttributeIds.Count)
                    {
                        status = false;

                        /* Example: 
                         * Among dimension attributes Color and Size, assume only Color is mapped at Product Sku (child) level and not Size.
                         * Both are configured in EH definition.
                         * missingAttributeIds contains 2 attrs Color(Assuming Id:4091) and Size(Assuming Id:4395).
                         * AttributeModelBL.Get returns only one attribute model (missingmodels) which is of 4091 because 4395 is not mapped to product sku level.
                         * So because Size is not mapped and model is not found, we need to log Error in operation result.*/
                        if (operationResult != null && !String.IsNullOrEmpty(levelLongName))
                        {
                            foreach (Int32 missingAttributeId in missingAttributeIds)
                            {
                                if (missedAttributeIds != null && missedAttributeIds.Count > 0)
                                {
                                    if (!missedAttributeIds.Contains(missingAttributeId))
                                    {
                                        ApplicationMessage message = messageBL.GetMessage("30026", this.SystemAttributeLocale.GetCultureName(), new object[] { levelLongName, missingAttributeId });
                                        operationResult.Errors.Add(new Error("", message.Message));
                                        break;
                                    }
                                }
                            }
                        }
                    }

                    _attributeModels.AddRange(missedmodels);
                }

                if (!status)
                {
                    IEnumerable<Int32> notLoadedAttributeModels = missedmodels == null ? missingAttributeIds : missingAttributeIds.Except(missedmodels.Select(x => x.Id));
                    LocaleMessage errorLocaleMessage = _localeMessageBL.Get(GlobalizationHelper.GetSystemUILocale(), "113905", new Object[] { String.Join(",", notLoadedAttributeModels), entity.CategoryLongNamePath, entity.EntityTypeLongName, entity.ContainerLongName }, false, new CallerContext());

                    // "Hierarchy generation failed. Please check if attributes with following ID's : '{0}' are mapped to Category: '{1}' or Entity Type: '{2}' and Container: '{3}'. Depending on attribute type ",
                    throw new MDMOperationException(errorLocaleMessage.Message);
                }
            }

            return status;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="attributeIds"></param>
        /// <param name="entity"></param>
        /// <param name="parentEntity"></param>
        /// <param name="attributeModels"></param>
        private void PopulateDependentAttributeDetails(Collection<Int32> attributeIds, Entity entity, Entity parentEntity, AttributeModelCollection attributeModels)
        {
            if (_isAttributeDependencyEnabled && attributeIds != null && attributeIds.Count > 0)
            {
                AttributeCollection attributeCollection = new AttributeCollection();

                foreach (Int32 attributeId in attributeIds)
                {
                    AttributeModel attributeModel = (AttributeModel)attributeModels.GetAttributeModel(attributeId, GlobalizationHelper.GetSystemDataLocale());

                    if (attributeModel != null)
                    {
                        if (attributeModel.DependentParentAttributes != null && attributeModel.DependentParentAttributes.Count > 0)
                        {
                            foreach (DependentAttribute dependentAttribute in attributeModel.DependentParentAttributes)
                            {
                                if (!entity.Attributes.Contains(dependentAttribute.AttributeId, dependentAttribute.Locale))
                                {
                                    Attribute attribute = (Attribute)parentEntity.GetAttribute(dependentAttribute.AttributeId);
                                    Attribute newAttribute = new Attribute();

                                    if (attribute != null)
                                    {
                                        newAttribute = new Attribute(attribute.ToXml());
                                        newAttribute.Action = ObjectAction.Ignore;
                                        newAttribute.SourceFlag = AttributeValueSource.Inherited;
                                        newAttribute.OverriddenValues.Clear();
                                        newAttribute.InheritedValues.Clear();

                                        if (attribute.CurrentValues != null && attribute.CurrentValues.Count > 0)
                                        {
                                            foreach (Value val in attribute.CurrentValues)
                                            {
                                                Value value = new Value(val.AttrVal);
                                                value.Action = ObjectAction.Read;

                                                newAttribute.AppendInheritedValue(value);
                                            }

                                            attributeCollection.Add(newAttribute);
                                        }
                                    }

                                    entity.Attributes.AddRange(attributeCollection);
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
        private void GetSecurityPrincipal()
        {
            if (_securityPrincipal == null)
            {
                _securityPrincipal = SecurityPrincipalHelper.GetCurrentSecurityPrincipal();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityHierarchyDefinition"></param>
        /// <returns></returns>
        private Dictionary<Int32, EntityVariantLevel> GetEntityHierarchyLevelsAsDictionary(EntityVariantDefinition entityHierarchyDefinition)
        {
            Dictionary<Int32, EntityVariantLevel> levelDictionary = new Dictionary<int, EntityVariantLevel>();

            EntityVariantLevel currentLevel = entityHierarchyDefinition.EntityVariantLevels.FirstOrDefault();

            //add base level
            Collection<Int32> currentLevelDimensionAttrIdList = new Collection<int>();
            foreach (Attribute attr in currentLevel.DimensionAttributes)
            {
                currentLevelDimensionAttrIdList.Add(attr.Id);
            }

            currentLevel.DenormalizedDimensionAttributeIdList = currentLevelDimensionAttrIdList;

            levelDictionary.Add(currentLevel.Id, currentLevel);
            Boolean isChildFound = true;

            while (isChildFound)
            {
                isChildFound = false;

                foreach (EntityVariantLevel level in entityHierarchyDefinition.EntityVariantLevels)
                {
                    if (level.ParentLevelId == currentLevel.Id)
                    {
                        //add base level
                        Collection<Int32> denormalizedDimensionAttributeIdList = new Collection<int>();
                        foreach (Int32 prevLevelAttrId in currentLevel.DenormalizedDimensionAttributeIdList)
                        {
                            denormalizedDimensionAttributeIdList.Add(prevLevelAttrId);
                        }

                        foreach (Attribute attr in level.DimensionAttributes)
                        {
                            if (!denormalizedDimensionAttributeIdList.Contains(attr.Id))
                                denormalizedDimensionAttributeIdList.Add(attr.Id);
                        }

                        level.DenormalizedDimensionAttributeIdList = denormalizedDimensionAttributeIdList;
                        levelDictionary.Add(level.Id, level);
                        currentLevel = level;
                        isChildFound = true;
                        break;
                    }
                }
            }

            return levelDictionary;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityHierarchyDefinitionId"></param>
        /// <param name="currentLevelId"></param>
        /// <param name="currentLevelEntityTypeId"></param>
        /// <returns></returns>
        private String GetEntityModelDictionaryKey(Int32 entityHierarchyDefinitionId, Int32 currentLevelId, Int32 currentLevelEntityTypeId)
        {
            return String.Format("EHD{0}_LE{1}_ET{2}", entityHierarchyDefinitionId, currentLevelId, currentLevelEntityTypeId);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="matrixRow"></param>
        /// <param name="entity"></param>
        /// <param name="dimAttId"></param>
        /// <param name="locale"></param>
        /// <param name="topLevelEntity"></param>
        private void PopulateDimensionAttributeValue(Row matrixRow, Entity entity, Int32 dimAttId, LocaleEnum locale, Entity topLevelEntity)
        {
            String dimAttrIdAsString = dimAttId.ToString();

            String attrVal = matrixRow[dimAttrIdAsString].ToString();
            Int32 valueRefId = ValueTypeHelper.Int32TryParse(matrixRow[dimAttrIdAsString + "RefId"].ToString(), -1);
            String invariatVal = valueRefId > 0 ? valueRefId.ToString() : attrVal;

            Attribute attribute = (Attribute)entity.GetAttribute(dimAttId, locale);

            if (attribute != null)
            {
                attribute.SourceFlag = AttributeValueSource.Overridden;
                attribute.Action = ObjectAction.Create;

                Value val = new Value();
                val.AttrVal = attrVal;
                val.InvariantVal = invariatVal;
                val.Locale = locale;
                val.ValueRefId = valueRefId;
                val.SetDisplayValue(matrixRow[dimAttrIdAsString + "DisplayValue"].ToString());

                if (attribute.IsLookup)
                {
                    Attribute topLevelAttribute = (Attribute)topLevelEntity.GetAttribute(dimAttId, locale);

                    if (topLevelAttribute != null)
                    {
                        ValueCollection topLevelAttributeValues = new ValueCollection();

                        topLevelAttributeValues = (ValueCollection)topLevelAttribute.GetCurrentValues();

                        if (topLevelAttributeValues != null && topLevelAttributeValues.Count > 0)
                        {
                            Value toplevelValue = (Value)topLevelAttributeValues.GetByValueRefId(valueRefId);

                            if (toplevelValue != null)
                            {
                                val.SetDisplayValue(toplevelValue.GetDisplayValue());
                            }
                        }
                    }
                }

                if (attribute.IsCollection)
                    val.Sequence = 0;

                ////we set the action to ignore if RuleAttribute is optional and does not have any value
                //foreach (EntityHierarchyLevel tempEntityHierarchyLevel in entityHierarchyDefinition.EntityHierarchyLevels)
                //{
                //    EntityHierarchyRuleAttribute entityHierarchyRuleAttribute = tempEntityHierarchyLevel.GetRuleAttributeByTargetAttributeId(dimAttrId);

                //    if (entityHierarchyRuleAttribute != null)
                //    {
                //        if (entityHierarchyRuleAttribute.IsOptional &&
                //            ((val.AttrVal != null && String.IsNullOrWhiteSpace(val.AttrVal.ToString())) ||
                //            (entityHierarchyRuleAttribute.RuleAttribute.IsLookup && val.AttrVal != null && val.AttrVal.ToString() == "-1")))
                //        {
                //            attribute.Action = ObjectAction.Ignore;
                //            val.Action = ObjectAction.Ignore;
                //            break;
                //        }
                //    }
                //}

                attribute.AppendValue(val, locale);
            }
        }

        private Entity GetEntityModel(Entity topLevelEntity, Int32 currentEntityType, Collection<Int32> dimensionAttributeIdList)
        {
            Entity entity = topLevelEntity.CloneBasicProperties();

            entity.Id = -1;
            entity.Name = String.Empty;
            entity.LongName = String.Empty;
            entity.ExternalId = String.Empty;
            entity.EntityTypeId = currentEntityType;

            if (dimensionAttributeIdList != null && dimensionAttributeIdList.Count > 0)
            {
                if (entity.Attributes == null)
                {
                    entity.Attributes = new AttributeCollection();
                }

                Collection<Int32> missingAttributeIds = new Collection<Int32>();

                foreach (Int32 dimensionAttributeId in dimensionAttributeIdList)
                {
                    Attribute attribute = (Attribute)topLevelEntity.GetAttribute(dimensionAttributeId);

                    if (attribute != null)
                    {
                        entity.Attributes.Add(attribute.CloneBasicProperties());
                    }
                    else
                    {
                        missingAttributeIds.Add(dimensionAttributeId);
                    }
                }

                if (missingAttributeIds != null && missingAttributeIds.Count > 0)
                {
                    AttributeModelContext attributeModelContext = new AttributeModelContext();
                    attributeModelContext.AttributeModelType = AttributeModelType.All;
                    attributeModelContext.CategoryId = entity.CategoryId;
                    attributeModelContext.EntityTypeId = entity.EntityTypeId;
                    attributeModelContext.ContainerId = entity.ContainerId;
                    attributeModelContext.Locales = new Collection<LocaleEnum>() { entity.Locale };

                    AttributeModelBL attributeModelManager = new AttributeModelBL();
                    AttributeModelCollection missingAttributeModels = attributeModelManager.Get(missingAttributeIds, null, null, attributeModelContext);

                    if (missingAttributeModels != null && missingAttributeModels.Count > 0)
                    {
                        foreach (AttributeModel attributeModel in missingAttributeModels)
                        {
                            Attribute attribute = new Attribute(attributeModel, attributeModel.Locale);

                            if (attribute != null)
                            {
                                entity.Attributes.Add(attribute);
                            }
                        }
                    }
                }
            }

            return entity;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="matrixRow"></param>
        /// <param name="dimAttrId"></param>
        /// <param name="locale"></param>
        /// <returns></returns>
        private String GetUniqueValKeyString(Row matrixRow, Int32 dimAttrId, LocaleEnum locale)
        {
            String uniqueValKey = String.Empty;

            String dimAttrIdAsString = dimAttrId.ToString();

            String attrVal = matrixRow[dimAttrIdAsString].ToString();
            Int32 valueRefId = ValueTypeHelper.Int32TryParse(matrixRow[dimAttrIdAsString + "RefId"].ToString(), -1);
            String invariatVal = valueRefId > 0 ? valueRefId.ToString() : attrVal;

            uniqueValKey = String.Format("A{0}_L{1}_V{2}", dimAttrIdAsString, (Int32)locale, invariatVal);

            return uniqueValKey;
        }

        #region Get child entities helper methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityId"></param>
        /// <param name="entityTypeId"></param>
        /// <param name="attributeIds"></param>
        /// <param name="application"></param>
        /// <param name="module"></param>
        /// <returns></returns>
        private EntityCollection GetEntitiesByEntityType(Int64 entityId, Int32 entityTypeId, Collection<Int32> attributeIds, MDMCenterApplication application, MDMCenterModules module)
        {
            Boolean isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;
            if (isTracingEnabled)
            {
                MDMTraceHelper.StartTraceActivity("EntityManager.EntityHierarchyBL.GetChildEntitiesByEntityType started...", MDMTraceSource.EntityHierarchyProcess, false);
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("Requested parent Entity Id: {0} and child Entity Type Id:{1}", entityId, entityTypeId), MDMTraceSource.EntityHierarchyProcess);
            }

            try
            {
                EntityCollection childEntities = new EntityCollection();

                if (entityId > 0)
                {
                    childEntities = GetChildEntitiesLegacy(entityId, entityTypeId, this.SystemAttributeLocale, attributeIds, true, -1, new CallerContext(application, module));
                }
                else
                {
                    if (isTracingEnabled)
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Requested parent Entity Id is less than 0, No need to fetch child", MDMTraceSource.EntityHierarchy);
                }

                return childEntities;
            }
            finally
            {
                if (isTracingEnabled)
                    MDMTraceHelper.StopTraceActivity("EntityManager.EntityHierarchyBL.GetChildEntitiesByEntityType completed.", MDMTraceSource.EntityHierarchy);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityId"></param>
        /// <param name="entityTypeId"></param>
        /// <param name="attributeInfoList"></param>
        /// <param name="application"></param>
        /// <param name="module"></param>
        /// <returns></returns>
        private EntityCollection GetChildEntitiesByEntityTypeLegacy(Int64 entityId, Int32 entityTypeId, Collection<KeyValuePair<Int32, LocaleEnum>> attributeInfoList, MDMCenterApplication application, MDMCenterModules module)
        {
            EntityCollection childEntities = new EntityCollection();

            //attributeInfoList.Key = AttributeId
            //attributeInfoList.Value = LocaleEnum
            if (_entity != null || _entity.HierarchyRelationships.Count > 0)
            {
                EntityBL entityManager = new EntityBL();
                EntityContext eContext = new EntityContext();
                Collection<Int32> attributeIdList = new Collection<Int32>();
                HierarchyRelationshipBL hrManager = new HierarchyRelationshipBL();
                OperationResult result = new OperationResult();

                eContext.LoadHierarchyRelationships = true;
                eContext.LoadEntityProperties = true;

                _entity = entityManager.Get(entityId, eContext, application, module, false, false);

                if (_entity.HierarchyRelationships != null && _entity.HierarchyRelationships.Count > 0)
                {
                    if (attributeInfoList == null || (attributeInfoList != null && attributeInfoList.Count == 0))
                    {
                        eContext.LoadAttributes = false;
                    }
                    else
                    {
                        eContext.LoadAttributes = true;
                        eContext.LoadAttributeModels = true;
                    }

                    //Get List of AttributeIds from Collection<KeyValuePair<Int32, LocaleEnum>>
                    if (attributeInfoList != null && attributeInfoList.Count > 0)
                    {
                        foreach (KeyValuePair<Int32, LocaleEnum> attrInfo in attributeInfoList)
                        {
                            attributeIdList.Add(attrInfo.Key); //AttributeId
                            eContext.DataLocales.Add(attrInfo.Value);//Locale
                        }
                    }

                    eContext.Locale = this.SystemAttributeLocale;
                    eContext.DataLocales.Add(this.SystemAttributeLocale); //DataLocales is populated from attributeInfoList also
                    eContext.AttributeModelType = AttributeModelType.All;
                    eContext.AttributeIdList = attributeIdList;
                    eContext.CategoryId = _entity.CategoryId;

                    result = hrManager.LoadRelatedEntities(_entity.HierarchyRelationships, eContext, true, new CallerContext(application, module));
                }
            }

            childEntities = GetChildHierarchyRelationshipsByEntityTypeId(_entity.HierarchyRelationships, entityTypeId);

            return childEntities;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hierarchyRelationshipCollection"></param>
        /// <param name="entityTypeId"></param>
        /// <returns></returns>
        private EntityCollection GetChildHierarchyRelationshipsByEntityTypeId(HierarchyRelationshipCollection hierarchyRelationshipCollection, Int32 entityTypeId)
        {
            EntityCollection childEntities = new EntityCollection();

            foreach (HierarchyRelationship hr in hierarchyRelationshipCollection)
            {
                if (hr.RelatedEntity != null && hr.RelatedEntity.EntityTypeId == entityTypeId)
                {
                    childEntities.Add(hr.RelatedEntity);
                }
                else if (hr.RelationshipCollection != null && hr.RelationshipCollection.Count > 0)
                {
                    childEntities.AddRange(GetChildHierarchyRelationshipsByEntityTypeId(hr.RelationshipCollection, entityTypeId));
                }
            }

            return childEntities;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parentEntityId"></param>
        /// <param name="childEntityTypeId"></param>
        /// <param name="locale"></param>
        /// <param name="returnAttributeIds"></param>
        /// <param name="getCompleteDetailsOfEntity"></param>
        /// <param name="maxRecordsToReturn"></param>
        /// <param name="callerContext"></param>
        /// <param name="getRecursiveChildren"></param>
        /// <returns></returns>
        private EntityCollection GetChildEntitiesLegacy(Int64 parentEntityId, Int32 childEntityTypeId, LocaleEnum locale, Collection<Int32> returnAttributeIds, Boolean getCompleteDetailsOfEntity, Int32 maxRecordsToReturn, CallerContext callerContext, Boolean getRecursiveChildren = false)
        {
            TraceSettings traceSettings = MDMOperationContextHelper.GetCurrentTraceSettings();
            DiagnosticActivity currentActivity = new DiagnosticActivity();

            if (traceSettings.IsTracingEnabled)
            {
                if (callerContext != null)
                {
                    ExecutionContext ec = new ExecutionContext();
                    ec.CallerContext = callerContext;
                    currentActivity.Start(ec);
                }
                else
                {
                    currentActivity.Start();
                }
            }

            EntityCollection childEntities = null;
            DBCommandProperties command = DBCommandHelper.Get(callerContext, MDMCenterModuleAction.Read);
            Dictionary<Int32, LocaleEnum> attributeLocaleList = new Dictionary<Int32, LocaleEnum>();
            LocaleEnum systemDataLocale = GlobalizationHelper.GetSystemDataLocale();

            DurationHelper durHelper = new DurationHelper(DateTime.Now);

            try
            {
                #region Load AttributeModels and populate dictonary of attributeId and locale for requested attribute

                if (returnAttributeIds != null && returnAttributeIds.Count > 0)
                {
                    if (traceSettings.IsBasicTracingEnabled)
                    {
                        currentActivity.LogInformation("Loading attributes model for Return Attributes : " + String.Join(",", returnAttributeIds.ToArray()));
                    }

                    AttributeModelCollection attributeModels = new AttributeModelCollection();
                    AttributeModelBL attributeManager = new AttributeModelBL();
                    AttributeModelContext attributeModelContext = new AttributeModelContext();

                    attributeModelContext.AttributeModelType = AttributeModelType.AttributeMaster;
                    attributeModelContext.Locales.Add(systemDataLocale);

                    durHelper.ResetDuration();

                    attributeModels = attributeManager.Get(returnAttributeIds, null, null, attributeModelContext);

                    if (traceSettings.IsBasicTracingEnabled)
                    {
                        currentActivity.LogMessageWithDuration(MessageClassEnum.Information, null, "Got attribute models : Count : " + attributeModels.Count(), durHelper.GetCumulativeTimeSpanInMilliseconds());
                        currentActivity.LogInformation("Creating list of locales for each attribute");
                    }

                    durHelper.ResetDuration();

                    if (attributeModels != null && attributeModels.Count > 0)
                    {
                        foreach (Int32 attributeId in returnAttributeIds)
                        {
                            if (!attributeLocaleList.ContainsKey(attributeId))
                            {
                                AttributeModel attributeModel = (AttributeModel)attributeModels.GetAttributeModel(attributeId, locale);

                                if (attributeModel != null)
                                {
                                    if (attributeModel.IsLocalizable)
                                    {
                                        attributeLocaleList.Add(attributeId, locale);
                                    }
                                    else
                                    {
                                        attributeLocaleList.Add(attributeId, systemDataLocale);
                                    }
                                }
                                else
                                {
                                    attributeLocaleList.Add(attributeId, locale);
                                }
                            }
                        }
                    }

                    if (traceSettings.IsBasicTracingEnabled)
                    {
                        currentActivity.LogMessageWithDuration(MessageClassEnum.Information, null, "Creating list of locales : Finished", durHelper.GetCumulativeTimeSpanInMilliseconds());
                    }
                }

                #endregion

                #region Load from DB

                if (traceSettings.IsBasicTracingEnabled)
                {
                    currentActivity.LogInformation("Getting child entities from DB");
                }

                durHelper.ResetDuration();

                EntityHierarchyDA entityHierarchyDA = new EntityHierarchyDA();

                childEntities = entityHierarchyDA.GetChildEntitiesLegacy(parentEntityId, childEntityTypeId, locale, attributeLocaleList, getCompleteDetailsOfEntity, getRecursiveChildren, command);

                if (traceSettings.IsBasicTracingEnabled)
                {
                    currentActivity.LogMessageWithDuration(MessageClassEnum.Information, null, "Getting child entities from DB : Finished : Count : " + childEntities.Count(), durHelper.GetCumulativeTimeSpanInMilliseconds());
                }

                #endregion

                if (childEntities != null && childEntities.Count > 0)
                {
                    #region Filter data for maxRecordToReturn

                    if (maxRecordsToReturn > 0 && childEntities.Count > maxRecordsToReturn)
                    {
                        if (traceSettings.IsBasicTracingEnabled)
                        {
                            currentActivity.LogInformation("Filtering records : MaxRecordsToReturn : " + maxRecordsToReturn + ", Child Entities Count : " + childEntities.Count());
                        }

                        durHelper.ResetDuration();

                        //TODO: This should be moved to the DB, it will be more efficient to do in DB than filtering through collection.
                        childEntities = new EntityCollection(childEntities.Take(maxRecordsToReturn).ToList());

                        if (traceSettings.IsBasicTracingEnabled)
                        {
                            currentActivity.LogMessageWithDuration(MessageClassEnum.Information, null, "Filtering records : Finished", durHelper.GetCumulativeTimeSpanInMilliseconds());
                        }
                    }

                    #endregion

                    #region Populate EntityType and Container Details

                    LoadEntityPropertiesLegacy(childEntities, callerContext);

                    #endregion

                    #region Populate display format for lookup, Date , DateTime and Decimal attributes

                    if (traceSettings.IsBasicTracingEnabled)
                    {
                        currentActivity.LogInformation("Populating lookup display values for attributes in child entities.");
                    }

                    if (returnAttributeIds != null && returnAttributeIds.Count > 0)
                    {
                        durHelper.ResetDuration();

                        AttributeModelBL attributeModelManager = new AttributeModelBL();
                        AttributeModelContext attributeModelContext = new AttributeModelContext();
                        attributeModelContext.AttributeModelType = AttributeModelType.AttributeMaster;
                        attributeModelContext.Locales.Add(locale);

                        if (traceSettings.IsBasicTracingEnabled)
                        {
                            currentActivity.LogInformation("Getting attributes models for attributes : " + String.Join(",", returnAttributeIds.ToArray()));
                        }

                        //TODO: Get this one time, in the code some lines above we dothe same.
                        AttributeModelCollection attributeModels = attributeModelManager.Get(returnAttributeIds, null, null, attributeModelContext);

                        if (attributeModels != null && attributeModels.Count > 0)
                        {
                            Lookup lookup = null;
                            Attribute attribute = null;
                            ValueCollection valueObjectsForLKRefId = null;
                            LookupBL lookupManager = new LookupBL();
                            ApplicationContext applicationContext = new ApplicationContext();

                            if (traceSettings.IsBasicTracingEnabled)
                            {
                                currentActivity.LogInformation("Loop : Attribute Models : Count : " + returnAttributeIds.Count());
                            }

                            foreach (Int32 attributeId in returnAttributeIds)
                            {
                                AttributeModel attributeModel = (AttributeModel)attributeModels.GetAttributeModel(attributeId, locale);

                                if (attributeModel != null)
                                {
                                    Collection<Int32> valueRefIds = new Collection<Int32>();    //Value Ref Id list for Lookup Get call
                                    Dictionary<Int32, ValueCollection> refIdToValuesDictionary = new Dictionary<Int32, ValueCollection>();  //Dictionary of valueRefId to their Values maintained in order to efficiently put display values 

                                    if (traceSettings.IsBasicTracingEnabled)
                                    {
                                        currentActivity.LogInformation("    Loop2 : Child Entities : Count : " + childEntities.Count());
                                    }

                                    //Collect Value Ref Ids for all entities
                                    foreach (Entity entity in childEntities)
                                    {
                                        attribute = (Attribute)entity.GetAttribute(attributeId, locale);

                                        if (attribute != null)
                                        {
                                            ValueCollection attributeCurrentValues = (ValueCollection)attribute.GetCurrentValuesInvariant();

                                            if (attributeCurrentValues == null || attributeCurrentValues.Count < 1)
                                                continue;

                                            if (attributeModel.IsLookup)
                                            {
                                                Int32 valueRefId = -1;

                                                attribute.IsLookup = attributeModel.IsLookup;
                                                if (traceSettings.IsBasicTracingEnabled)
                                                {
                                                    currentActivity.LogInformation("        Loop3 :  Attribute is Lookup : lterating current values");
                                                }

                                                foreach (Value val in attributeCurrentValues)
                                                {
                                                    valueRefId = ValueTypeHelper.Int32TryParse(val.GetStringValue(), val.ValueRefId);

                                                    if (valueRefId > 0)
                                                    {
                                                        val.ValueRefId = valueRefId;

                                                        if (refIdToValuesDictionary.ContainsKey(valueRefId))
                                                        {
                                                            //This value ref Id is already present in dictionary..
                                                            //Update value field(ValueCollection) for this key with the current attribute vale

                                                            valueObjectsForLKRefId = refIdToValuesDictionary[valueRefId];
                                                            valueObjectsForLKRefId.Add(val);
                                                        }
                                                        else
                                                        {
                                                            //This value ref Id is not available in dictionary..
                                                            //Add new entry in dictionary and also in value ref Id list

                                                            valueRefIds.Add(valueRefId);

                                                            valueObjectsForLKRefId = new ValueCollection();
                                                            valueObjectsForLKRefId.Add(val);

                                                            refIdToValuesDictionary.Add(valueRefId, valueObjectsForLKRefId);
                                                        }
                                                    }
                                                }

                                                if (traceSettings.IsBasicTracingEnabled)
                                                {
                                                    currentActivity.LogInformation("        Loop3 :  Attribute is Lookup : Finished");
                                                }
                                            }
                                            else if (attributeModel.AttributeDataTypeName.ToLowerInvariant().Equals("decimal"))
                                            {
                                                attribute.AttributeDataType = AttributeDataType.Decimal;
                                                attribute.Precision = attributeModel.Precision;

                                                foreach (Value val in attributeCurrentValues)
                                                {
                                                    if (val != null && val.AttrVal != null && !String.IsNullOrWhiteSpace(val.AttrVal.ToString()))
                                                    {
                                                        String attributeValue = val.AttrVal.ToString();
                                                        val.Locale = attribute.Locale;
                                                        val.AttrVal = MDM.Core.FormatHelper.FormatNumber(attributeValue, locale.GetCultureName(), attributeModel.Precision, attributeModel.IsPrecisionArbitrary);
                                                    }
                                                }
                                            }
                                            else if (attributeModel.AttributeDataTypeName.ToLowerInvariant().Equals("datetime"))
                                            {
                                                attribute.AttributeDataType = AttributeDataType.DateTime;

                                                foreach (Value val in attributeCurrentValues)
                                                {
                                                    if (val != null && val.AttrVal != null && !String.IsNullOrWhiteSpace(val.AttrVal.ToString()))
                                                    {
                                                        String attributeValue = val.AttrVal.ToString();
                                                        DateTime selectedDateTime = DateTime.MinValue;

                                                        //get deformatted date
                                                        selectedDateTime = MDM.Core.FormatHelper.DeformatDate(attributeValue, locale.GetCultureName());

                                                        if (selectedDateTime != null && selectedDateTime != DateTime.MinValue)
                                                        {
                                                            if (!String.IsNullOrWhiteSpace(this.CurrentTimeZone))
                                                            {
                                                                //convert time zone to sstem time zone
                                                                selectedDateTime = MDM.Core.FormatHelper.ConvertToTimeZone(selectedDateTime, this.SystemTimeZone, this.CurrentTimeZone);
                                                                val.AttrVal = selectedDateTime.ToString();
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                            else if (attributeModel.AttributeDataTypeName.ToLowerInvariant().Equals("date"))
                                            {
                                                attribute.AttributeDataType = AttributeDataType.Date;

                                                foreach (Value val in attributeCurrentValues)
                                                {
                                                    if (val != null && val.AttrVal != null && !String.IsNullOrWhiteSpace(val.AttrVal.ToString()))
                                                    {
                                                        String attributeValue = val.AttrVal.ToString();
                                                        val.AttrVal = MDM.Core.FormatHelper.FormatDateOnly(attributeValue, LocaleEnum.en_US.GetCultureName(), locale.GetCultureName());
                                                    }
                                                }
                                            }
                                        }
                                    }

                                    if (traceSettings.IsBasicTracingEnabled)
                                    {
                                        currentActivity.LogInformation("    Loop2 :  Child Entities : Finished");
                                    }

                                    if (valueRefIds.Count > 0)
                                    {
                                        if (traceSettings.IsBasicTracingEnabled)
                                        {
                                            currentActivity.LogInformation("Getting lookup data using Lookup BL, RefIds : " + String.Join(",", valueRefIds.ToArray()));
                                        }

                                        //Get lookup record for thus obtained value ref Ids
                                        lookup = lookupManager.Get(attributeId, locale, 0, valueRefIds, applicationContext, callerContext, false);

                                        if (lookup != null && lookup.Rows.Count > 0)
                                        {
                                            String displayValue = String.Empty;
                                            if (traceSettings.IsBasicTracingEnabled)
                                            {
                                                currentActivity.LogInformation("    Loop2 : Lookup list : Count : " + lookup.Rows.Count());
                                            }

                                            foreach (KeyValuePair<Int32, ValueCollection> refIdToValuesPair in refIdToValuesDictionary)
                                            {
                                                displayValue = lookup.GetDisplayFormatById(refIdToValuesPair.Key);

                                                if (!String.IsNullOrWhiteSpace(displayValue))
                                                {
                                                    if (traceSettings.IsBasicTracingEnabled)
                                                    {
                                                        currentActivity.LogInformation("        Loop3 : Populating values : Count : " + refIdToValuesPair.Value);
                                                    }

                                                    //Populate all value objects having this ref Id with display value
                                                    foreach (Value value in refIdToValuesPair.Value)
                                                    {
                                                        value.InvariantVal = value.ValueRefId;
                                                        value.AttrVal = value.ValueRefId;
                                                        value.SetDisplayValue(displayValue);
                                                    }

                                                    if (traceSettings.IsBasicTracingEnabled)
                                                    {
                                                        currentActivity.LogInformation("        Loop3 : Populating Values : Finished");
                                                    }
                                                }
                                            }

                                            if (traceSettings.IsBasicTracingEnabled)
                                            {
                                                currentActivity.LogInformation("    Loop2 : Lookup list : Finished");
                                            }
                                        }
                                    }
                                }
                            }

                            if (traceSettings.IsBasicTracingEnabled)
                            {
                                currentActivity.LogMessageWithDuration(MessageClassEnum.Information, null, "Loop : Attributes Models : Finished", durHelper.GetCumulativeTimeSpanInMilliseconds());
                            }
                        }
                    }

                    if (traceSettings.IsBasicTracingEnabled)
                    {
                        currentActivity.LogMessageWithDuration(MessageClassEnum.Information, null, "Populating lookup display values for attributes in child entities : Finished", durHelper.GetCumulativeTimeSpanInMilliseconds());
                    }

                    #endregion
                }
            }
            catch (Exception ex)
            {
                currentActivity.LogError(ex.Message);
            }
            finally
            {
                if (traceSettings.IsTracingEnabled)
                {
                    currentActivity.Stop();
                }
            }

            return childEntities;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="callerContext"></param>
        private void LoadEntityPropertiesLegacy(EntityCollection entities, CallerContext callerContext)
        {
            TraceSettings traceSettings = MDMOperationContextHelper.GetCurrentTraceSettings();
            DiagnosticActivity currentActivity = new DiagnosticActivity();

            if (traceSettings.IsTracingEnabled)
            {
                if (callerContext != null)
                {
                    ExecutionContext ec = new ExecutionContext();
                    ec.CallerContext = callerContext;
                    currentActivity.Start(ec);
                }
                else
                    currentActivity.Start();
            }

            try
            {
                if (entities != null && entities.Count > 0)
                {
                    Int32 containerId = entities.FirstOrDefault().ContainerId;
                    Int32 entityTypeId = entities.FirstOrDefault().EntityTypeId;
                    Container entityContainer = null;
                    EntityType entityEntityType = null;

                    if (containerId > 0)
                    {
                        if (traceSettings.IsBasicTracingEnabled)
                            currentActivity.LogInformation("Getting Container : Container Id : " + containerId.ToString());

                        ContainerBL containerManager = new ContainerBL();
                        ContainerContext containerContext = new ContainerContext(false);
                        entityContainer = containerManager.Get(containerId, containerContext, callerContext);
                    }

                    if (entityTypeId > 0)
                    {
                        if (traceSettings.IsBasicTracingEnabled)
                            currentActivity.LogInformation("Getting Entity Type : EntityTypeId : " + entityTypeId.ToString());

                        EntityTypeBL entityTypeManager = new EntityTypeBL();
                        entityEntityType = entityTypeManager.GetById(entityTypeId, callerContext);
                    }

                    if (entityEntityType != null && entityContainer != null)
                    {
                        if (traceSettings.IsBasicTracingEnabled)
                            currentActivity.LogInformation("Populating entity objects : Count : " + entities.Count());

                        foreach (Entity entity in entities)
                        {
                            entity.EntityTypeName = entityEntityType.Name;
                            entity.EntityTypeLongName = entityEntityType.LongName;
                            entity.ContainerName = entityContainer.Name;
                            entity.ContainerLongName = entityContainer.LongName;
                            entity.OrganizationId = entityContainer.OrganizationId;
                            entity.OrganizationName = entityContainer.OrganizationShortName;
                            entity.OrganizationLongName = entityContainer.OrganizationLongName;
                        }
                    }
                }
            }
            finally
            {
                if (traceSettings.IsTracingEnabled)
                    currentActivity.Stop();
            }
        }

        private Table CalculateDimensionsByRuleAttributesAndEntityHierarchyLevel(EntityVariantRuleAttributeCollection attributes, EntityVariantLevel entityHierarchyLevel, CallerContext context = null)
        {
            Table dimensionTable = CalculateDimensionsCartesian(attributes, entityHierarchyLevel, context);

            return dimensionTable;
        }

        private void ValidateEntityHierarchyLevel(EntityVariantLevel entityHierarchyLevel, CallerContext context = null)
        {
            if (entityHierarchyLevel == null)
            {
                _localeMessage = _localeMessageBL.Get(GlobalizationHelper.GetSystemUILocale(), "113960", false, context);
                throw new MDMOperationException(messageCode: "113960", message: _localeMessage.Message, source: "EntityHierarchyBL", stackTrace: String.Empty, targetSite: "CalculateDimensions");
            }
        }

        #endregion Private Methods

        #endregion

        #endregion
    }
}