using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using SM = System.ServiceModel;

namespace MDM.EntityManager.Business.EntityOperations
{
    using AttributeModelManager.Business;
    using BufferManager;
    using BusinessObjects;
    using BusinessObjects.Diagnostics;
    using BusinessObjects.Workflow;
    using ConfigurationManager.Business;
    using Core;
    using Core.Exceptions;
    using Data;
    using Helpers;
    using Interfaces;
    using MDM.ContainerManager.Business;
    using MessageManager.Business;
    using PermissionManager.Business;
    using RelationshipManager.Business;
    using Utility;
    using Workflow.PersistenceManager.Business;

    /// <summary>
    /// Specifies class for entity get manager
    /// </summary>
    internal class EntityGetManager : BusinessLogicBase
    {
        #region Fields

        /// <summary>
        /// </summary>
        private static readonly Object entityGetLockObject = new object();

        /// <summary>
        /// </summary>
        private readonly EntityBufferManager _entityBufferManager;

        /// <summary>
        ///     Field denoting entity data access
        /// </summary>
        private readonly EntityDA _entityDA;

        /// <summary>
        /// </summary>
        private Boolean _isBulkGetFromDbEnabled = true; //This would be set in get entity flow only.

        /// <summary>
        ///     Specifies whether the entity loading event is enabled or not. Which is decided by the
        ///     'MDMCenter.EntityManager.EntityLoadingEvent.Enabled'appconfig key.
        /// </summary>
        private Boolean _isEntityLoadingEventEnabled; // Load of this key would happen in constructor

        /// <summary>
        ///     Field denoting locale Message
        /// </summary>
        private LocaleMessage _localeMessage;

        /// <summary>
        ///     Field denoting localeMessageBL
        /// </summary>
        private readonly LocaleMessageBL _localeMessageBL;

        /// <summary>
        ///     Field denoting the security principal
        /// </summary>
        private SecurityPrincipal _securityPrincipal;

        /// <summary>
        /// 
        /// </summary>
        private readonly IEntityManager _entityManager = null;

        #endregion

        #region Constructors

        /// <summary>
        /// 
        /// </summary>
        public EntityGetManager(IEntityManager entityManager)
        {
            _entityManager = entityManager;
            //_securityPrincipal = SecurityPrincipalHelper.GetCurrentSecurityPrincipal();
            _entityBufferManager = new EntityBufferManager();
            _entityDA = new EntityDA();
            _localeMessageBL = new LocaleMessageBL();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityManager"></param>
        /// <param name="securityPrincipal"></param>
        public EntityGetManager(IEntityManager entityManager, SecurityPrincipal securityPrincipal)
            : this(entityManager)
        {
            _securityPrincipal = securityPrincipal;
        }

        #endregion

        #region Properties

        #endregion

        #region Methods

        #region Public Methods

        /// <summary>
        ///     Get Entity By given ExternalId
        /// </summary>
        /// <param name="externalId">ExternalId for which Entity needs to be Populate</param>
        /// <param name="entityContext">entityContext for which Entity needs to be Populate</param>
        /// <param name="callerContext">Indicates Application and Module name by which action is being performed</param>
        /// <param name="publishEvents">Indicates whether to publish events or not. Default is set to 'true'</param>
        /// <param name="applyAVS">Indicates whether to apply AVS or not. Default is set to 'true'</param>
        /// <returns>Entity Object</returns>
        /// <exception cref="MDMOperationException">Thrown if externalId or entity context is null</exception>
        /// <exception cref="MDMOperationException">Thrown if Entity is Category and EntityContext is having ContainerId Zero</exception>
        public Entity GetByExternalId(String externalId, EntityContext entityContext, CallerContext callerContext, Boolean publishEvents = true, Boolean applyAVS = true)
        {
            Boolean isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;
            if (isTracingEnabled)
            {
                MDMTraceHelper.StartTraceActivity("EntityManager.Entity.GetByExternalId", MDMTraceSource.EntityGet, false);
            }

            try
            {
                #region Parameter Validation

                if (String.IsNullOrWhiteSpace(externalId))
                {
                    _localeMessage = _localeMessageBL.Get(GlobalizationHelper.GetSystemUILocale(), "111817", false, callerContext);
                    throw new MDMOperationException("111817", _localeMessage.Message, "EntityManager", String.Empty, "GetByExternalId"); //ExternalId cannot be null
                }

                if (entityContext == null)
                {
                    _localeMessage = _localeMessageBL.Get(GlobalizationHelper.GetSystemUILocale(), "111818", false, callerContext);
                    throw new MDMOperationException("111818", _localeMessage.Message, "EntityManager", String.Empty, "GetByExternalId"); //EntityContext cannot be null.
                }

                if (callerContext == null)
                {
                    throw new MDMOperationException("111823", "CallerContext is null or empty.", "EntityManager", String.Empty, "GetEntitiesByExternalIds");
                }

                #endregion

                #region Populate Ids in EntityContext

                if (entityContext.ResolveIdsByName)
                {
                    EntityContextHelper.PopulateIdsInEntityContext(entityContext, _entityManager, callerContext);
                }

                #endregion

                #region GetEntityMap for requested ExternalId

                EntityMapBL entityMapBL = new EntityMapBL();

                //Changed the system id from "1" to "Internal" as even in Process for entityMapBL, system id is "Internal".
                //All other places for entityMapBL, system id is passed as "Internal"
                EntityMap entityMap = entityMapBL.Get("Internal", externalId, entityContext.ContainerId, entityContext.EntityTypeId, entityContext.CategoryId, null, callerContext.Application, callerContext.Module);

                #endregion

                Entity entity = new Entity();

                if (entityMap != null)
                {
                    EntityContext context = (EntityContext)entityContext.Clone();
                    Int64 entityId = entityMap.InternalId;
                    context.CategoryId = entityMap.CategoryId;
                    context.EntityTypeId = entityMap.EntityTypeId;

                    if (entityMap.EntityTypeId == 6)
                    {
                        if (context.ContainerId == 0)
                        {
                            _localeMessage = _localeMessageBL.Get(GlobalizationHelper.GetSystemUILocale(), "111819",
                                false, callerContext);
                            throw new MDMOperationException("111819", _localeMessage.Message, "EntityManager",
                                String.Empty, "GetByExternalId");
                            //Entity get failed as request is for Category and Container information is not available. Please provide container information
                        }
                    }
                    else
                    {
                        context.ContainerId = entityMap.ContainerId;
                    }
                    
                    EntityGetOptions entityGetOptions = new EntityGetOptions { PublishEvents = publishEvents, ApplyAVS = applyAVS };
                    entity = GetEntityMain(new Collection<Int64> { entityId }, context, entityGetOptions, callerContext).FirstOrDefault();
                }

                return entity;
            }
            finally
            {
                if (isTracingEnabled)
                    MDMTraceHelper.StopTraceActivity("EntityManager.Entity.GetByExternalId", MDMTraceSource.EntityGet);
            }
        }

        /// <summary>
        /// Gets an entity collection based on the external id(s) and entity context
        /// </summary>
        /// <param name="externalIdList">Lists the external id(s) for which the entities needs to be retrieved</param>
        /// <param name="entityContext">Specifies the entity context for which entities needs to be retrieved</param>
        /// <param name="callerContext">Specifies the caller context, which contains the application and module that has invoked the API</param>
        /// <param name="publishEvents">Specifies whether to publish events or not</param>
        /// <param name="applyAVS">Specifies whether to apply AVS or not</param>
        /// <returns>Returns the collection of entities</returns>
        public EntityCollection GetEntitiesByExternalIds(Collection<String> externalIdList, EntityContext entityContext, CallerContext callerContext, Boolean publishEvents, Boolean applyAVS)
        {
            if (externalIdList == null || externalIdList.Count == 0)
            {
                throw new ArgumentNullException("externalIdList");
            }

            EntityCollection entityCollection = new EntityCollection();
            Entity entity = null;

            foreach (String externalId in externalIdList)
            {
                entity = GetByExternalId(externalId, entityContext, callerContext, publishEvents, applyAVS);
                entityCollection.Add(entity);
            }

            return entityCollection;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityIds"></param>
        /// <param name="entityContext"></param>
        /// <param name="entityGetOptions"></param>
        /// <param name="callerContext"></param>
        /// <returns></returns>
        public EntityCollection GetEntityMain(Collection<Int64> entityIds, EntityContext entityContext, EntityGetOptions entityGetOptions, CallerContext callerContext)
        {
            TraceSettings currentTraceSettings = MDMOperationContextHelper.GetCurrentTraceSettings();
            Boolean isTracingEnabled = currentTraceSettings.IsBasicTracingEnabled;
            EntityCollection entityCollection = new EntityCollection();

            DiagnosticActivity diagnosticActivity = new DiagnosticActivity();

            try
            {
                String traceMessage = String.Empty;

                ExecutionContext executionContext = null;

                if (isTracingEnabled)
                {
                    CallDataContext callDataContext = entityContext.GetNewCallDataContext();
                    callDataContext.EntityIdList = entityIds;
                    executionContext = new ExecutionContext(callerContext, callDataContext, _securityPrincipal.GetSecurityContext(), entityGetOptions.ToXml());
                    executionContext.LegacyMDMTraceSources.Add(MDMTraceSource.EntityGet);
                    diagnosticActivity.Start(executionContext);
                }

                #region Validate Entity Context and input

                if (!IsEntityContextParamsValid(entityContext))
                {
                    diagnosticActivity.LogMessage(MessageClassEnum.Error, "111787", "In EntityContext, any one of LoadEntityProperties, LoadAttributes or LoadRelationships must be set to true");
                    _localeMessage = _localeMessageBL.Get(GlobalizationHelper.GetSystemUILocale(), "111787", false, callerContext);
                    throw new MDMOperationException("111787", _localeMessage.Message, "EntityManager", String.Empty, "Get"); //In EntityContext, any one of LoadEntityProperties, LoadAttributes or LoadRelationships must be set to true
                }

                //Check locale detail in EntityContext and populate System Default Data Locale if no locale detail is provided.
                EntityOperationsCommonUtility.ValidateAndUpdateEntityContextForLocales(entityContext, callerContext.Application);

                if (entityIds == null || entityIds.Count < 1)
                {
                    String errorMessage = "Entity Id list provided is null or empty. Entity get process would terminate now and return empty entity collection as result";
                    diagnosticActivity.LogMessage(MessageClassEnum.Error, errorMessage);
                    return entityCollection;
                }

                if (entityIds.Any(id => id <= 0))
                {
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Warning, "Entity get request is received without entity id. Get operation is being terminated with exception.", MDMTraceSource.EntityGet);
                    _localeMessage = _localeMessageBL.Get(GlobalizationHelper.GetSystemUILocale(), "111795", false, callerContext);
                    throw new MDMOperationException("111795", _localeMessage.Message, "EntityManager", String.Empty, "Get");
                }

                #endregion Validate Entity Context and input

                #region Initial Setup

                #region Diagnostics and tracing

                if (isTracingEnabled)
                {
                    String entityIdListAsString = EntityOperationsCommonUtility.GetEntityIdListAsString(entityIds);
                    traceMessage = String.Format("Starting get entity main execution flow for Entity id list:{0}, EntityContext:{1}, EntityGetOptions:{2}, Application:{3} and Service:{3}", entityIdListAsString, entityContext.ToXml(), entityGetOptions.ToString(), callerContext.Application, callerContext.Module);
                    diagnosticActivity.LogMessage(MessageClassEnum.Information, traceMessage);
                }

                #endregion

                Int32 numberofEntities = entityIds.Count;

                Boolean isCacheEnabled = AppConfigurationHelper.GetAppConfig("MDMCenter.EntityManager.EntityCache.Enabled", true);
                Int32 numberOfEntitiesThreads = AppConfigurationHelper.GetAppConfig("MDMCenter.EntityManager.ParellelEntityGet.ThreadPoolSize", 5);
                _isBulkGetFromDbEnabled = AppConfigurationHelper.GetAppConfig("MDMCenter.EntityManager.BulkGetFromDB.Enabled", true);
                _isEntityLoadingEventEnabled = AppConfigurationHelper.GetAppConfig("MDMCenter.EntityManager.EntityLoadingEvent.Enabled", false);

                // bulkEntityGetBatchSize variable moved from class level member to method level. Because in some cases , the variable value set based on entityGetOptions.BulkGetBatchSize
                // which is passed to the child methods and later the child method calling this method again. 
                // In that time , entityGetOptions.BulkGetBatchSize not passed to this method[By default value is 0]  so the variable value set based on appconfig key value.
                // Due to that, Batch size calculation goes wrong, because the object value got changed and the same object used in parallel loop as a reference.
                Int32 bulkEntityGetBatchSize = entityGetOptions.BulkGetBatchSize > 0 ? entityGetOptions.BulkGetBatchSize : AppConfigurationHelper.GetAppConfig("MDMCenter.EntityManager.BulkEntityGet.BatchSize", 1);

                // Check locale in context, if not available use default locale of an user making request
                LocaleEnum locale = entityContext.Locale;
                if (locale == LocaleEnum.UnKnown)
                {
                    locale = _securityPrincipal.UserPreferences.DataLocale;
                    entityContext.Locale = locale;
                }

                //Get command properties
                DBCommandProperties command = DBCommandHelper.Get(callerContext.Application, callerContext.Module, MDMCenterModuleAction.Read);

                // If master cache key is enabled, then only check loadLatest flag passsed to this method, if cache key is disabled, then just loadLatest all the time
                if (!isCacheEnabled)
                {
                    entityGetOptions.LoadLatestFromDB = true;
                }

                MDMRuleParams mdmRuleParam = new MDMRuleParams()
                {
                    DDGCallerModule = DDGCallerModule.EntityGet,
                    Events = new Collection<MDMEvent>() { MDMEvent.EntityLoading, MDMEvent.EntityLoaded },
                    UserSecurityPrincipal = _securityPrincipal,
                    CallerContext = callerContext
                };

                #region Diagnostics and tracing

                if (isTracingEnabled)
                {
                    traceMessage = String.Format("AppConfig Values: [MDMCenter.EntityManager.EntityCache.Enabled: {0}, MDMCenter.EntityManager.BulkGetFromDB.Enabled:{1}, MDMCenter.EntityManager.BulkEntityGet.BatchSize:{2}, MDMCenter.EntityManager.ParellelEntityGet.ThreadPoolSize:{3}, MDMCenter.EntityManager.EntityLoadingEvent.Enabled:{4}]", isCacheEnabled, _isBulkGetFromDbEnabled, bulkEntityGetBatchSize, numberOfEntitiesThreads, _isEntityLoadingEventEnabled);

                    diagnosticActivity.LogInformation(traceMessage);
                    diagnosticActivity.LogDurationInfo("Initial setup completed");
                }

                #endregion

                #endregion

                #region Create batching and load entities in parallel using For.Parallel

                try
                {
                    if (numberofEntities > 1)
                    {
                        #region Bulk Entity Get

                        #region Calculate threads and batch sizes

                        if (callerContext.Module == MDMCenterModules.Import) // Hard coding this for now...we need to make it config if this works fine...
                        {
                            numberOfEntitiesThreads = 1;
                        }

                        // Reconfigure the thread count based on the batches required.
                        Int32 noOfBatchesRequired = numberofEntities / bulkEntityGetBatchSize;

                        if (numberofEntities % bulkEntityGetBatchSize > 0)
                            noOfBatchesRequired++;

                        numberOfEntitiesThreads = Math.Min(numberOfEntitiesThreads, noOfBatchesRequired);

                        #endregion

                        #region Diagnostics and tracing

                        if (isTracingEnabled)
                        {
                            traceMessage = String.Format("Parallel get settings: [Number of threads:{0}, Number of entities: {1}, Number of batches: {2}]", numberOfEntitiesThreads, numberofEntities, noOfBatchesRequired);
                            diagnosticActivity.LogMessage(MessageClassEnum.Information, traceMessage);
                        }

                        #endregion

                        if (numberOfEntitiesThreads > 1)
                        {
                            #region Parallel Get

                            // Equally distribute the available entities across the available tasks..The left over is given to the last one
                            Int32 entitybatchPerThread = (numberofEntities) / numberOfEntitiesThreads;

                            // Split the entities..This is the same logic used in Import engine...Need to move this to a UTILITY method..
                            Dictionary<Int32, Int32> entitySplit = SplitEntiesForThreads(numberOfEntitiesThreads, 0, numberofEntities - 1, entitybatchPerThread);

                            SM.OperationContext operationContext = SM.OperationContext.Current;
                            HttpContext httpContext = HttpContext.Current;

                            // Simple parallel..loop
                            Parallel.For(0, numberOfEntitiesThreads, i =>
                                {
                                    SM.OperationContext.Current = operationContext;
                                    HttpContext.Current = httpContext;

                                    List<Entity> entityList = GetEntityBatch(entityIds, entitySplit[i], entitySplit[i + 1] - 1, entityContext, entityGetOptions, callerContext, command, bulkEntityGetBatchSize);

                                    if (entityList != null && entityList.Count > 0)
                                    {
                                        // put back in the master collection.
                                        lock (entityGetLockObject)
                                        {
                                            foreach (Entity outputEntity in entityList)
                                            {
                                                entityCollection.Add(outputEntity);
                                            }
                                        }
                                    }
                                }
                            );

                            #endregion
                        }
                        else if (numberOfEntitiesThreads == 1)
                        {
                            entityCollection = GetEntity(entityIds, entityContext, entityGetOptions, callerContext, command, mdmRuleParam);
                        }

                        #endregion
                    }
                    else if (numberofEntities == 1)
                    {
                        EntityContext entityContextClone = (EntityContext)entityContext.Clone();
                        entityCollection = GetEntity(entityIds, entityContextClone, entityGetOptions, callerContext, command, mdmRuleParam);
                    }
                }
                catch (AggregateException ex)
                {
                    if (ex.InnerException != null)
                    {
                        traceMessage = String.Format("Entity Get failed with exception message: {0}", ex.InnerException.Message);
                        diagnosticActivity.LogMessage(MessageClassEnum.Error, traceMessage);
                        throw ex.InnerException;
                    }
                }
                catch (Exception ex)
                {
                    traceMessage = String.Format("Entity Get failed with exception message: {0}", ex.InnerException != null ? ex.InnerException.Message : ex.Message);
                    diagnosticActivity.LogMessage(MessageClassEnum.Error, traceMessage);
                    throw;
                }

                #region Diagnostics and tracing

                if (isTracingEnabled)
                {
                    diagnosticActivity.LogDurationInfo("Entity parallel get completed");
                }

                #endregion

                #endregion

                #region Load Overridden AttributeModel Properties

                foreach (Entity entity in entityCollection)
                {
                    if (entity.AttributeModels == null || entity.AttributeModels.Count == 0)
                        continue;

                    //If attributes and their models are requested in multiple locales and calls comes from UI process 
                    //then and then given attribute models in entity locale.Hence added caller context module check.
                    //This is required to avoid duplicate attributes rendering on entity editor.
                    if (entityContext.DataLocales.Count > 1 && callerContext.Module == MDMCenterModules.UIProcess)
                    {
                        AttributeModelCollection filteredAttributeModels = new AttributeModelCollection();
                        LocaleEnum entityLocale = entity.Locale;

                        IEnumerable<AttributeModel> attributeModels = entity.AttributeModels.Where(attrModel => attrModel.Locale == entityLocale);

                        foreach (AttributeModel attrModel in attributeModels)
                        {
                            filteredAttributeModels.Add(attrModel);
                        }

                        entity.AttributeModels = filteredAttributeModels;
                    }

                    if (!entityContext.LoadAttributeModels)
                    {
                        foreach (AttributeModel attrModel in entity.AttributeModels)
                        {
                            Dictionary<String, String> overridenProperties = attrModel.OverridenProperties;

                            if (overridenProperties != null && overridenProperties.Count > 0)
                            {
                                Int32 attributeId = attrModel.Id;

                                if (entity.OverridenAttributeModelProperties == null)
                                    entity.OverridenAttributeModelProperties = new Dictionary<Int32, Dictionary<String, String>>();

                                if (!entity.OverridenAttributeModelProperties.ContainsKey(attributeId))
                                {
                                    entity.OverridenAttributeModelProperties.Add(attributeId, overridenProperties);
                                }
                                else
                                {
                                    #region Diagnostics and tracing

                                    if (isTracingEnabled)
                                    {
                                        diagnosticActivity.LogWarning(String.Format("Multiple instance found for attributeName {0} and attributeGroupName {1}", attrModel.LongName, attrModel.AttributeParentLongName));
                                    }

                                    #endregion
                                }
                            }
                        }

                        entity.AttributeModels.Clear();
                    }
                }

                #region Diagnostics and tracing

                if (isTracingEnabled)
                {
                    diagnosticActivity.LogDurationInfo("Overridden attribute model properties loaded");
                }

                #endregion

                #endregion

                #region Set Entities as ReadOnly in parallel using For.Parallel

                SetEntitiesAsViewMode(entityCollection, numberOfEntitiesThreads);

                #region Diagnostics and tracing

                if (isTracingEnabled)
                {
                    diagnosticActivity.LogDurationInfo("Entity viewonly status is calculated and updated");
                }

                #endregion

                #endregion

            } //No catch here.. Let exceptions flow here. This try and finally is added just to stop activity
            finally
            {
                #region Final Diagnostics and tracing

                if (isTracingEnabled)
                {
                    diagnosticActivity.LogMessageWithData("See 'View Data' for loaded entities.", entityCollection.ToXml());
                    diagnosticActivity.Stop();
                }

                #endregion
            }

            return entityCollection;
        }

        #endregion

        #region Private Methods

        #region Main Get Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityIdList"></param>
        /// <param name="entityContext"></param>
        /// <param name="entityGetOptions"></param>
        /// <param name="callerContext"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        private EntityCollection GetEntity(Collection<Int64> entityIdList, EntityContext entityContext, EntityGetOptions entityGetOptions, CallerContext callerContext, DBCommandProperties command, MDMRuleParams ruleParams)
        {
            TraceSettings currentTraceSettings = MDMOperationContextHelper.GetCurrentTraceSettings();
            Boolean isTracingEnabled = currentTraceSettings.IsBasicTracingEnabled;
            String traceMessage;

            DiagnosticActivity diagnosticActivity = new DiagnosticActivity();

            EntityCollection entityCollection = null;

            try
            {
                #region Step : Initial Setup

                String entityIdListAsString = String.Empty;

                #region Populate Ids in EntityContext

                if (entityContext.ResolveIdsByName)
                {
                    EntityContextHelper.PopulateIdsInEntityContext(entityContext, _entityManager, callerContext);
                }

                #endregion

                #region Diagnostics and tracing

                if (isTracingEnabled)
                {
                    entityIdListAsString = EntityOperationsCommonUtility.GetEntityIdListAsString(entityIdList);
                    diagnosticActivity.Start();

                    traceMessage = String.Format("Starting get entity internal execution flow for Entity id list:{0}, EntityContext:{1}, EntityGetOptions:{2}, Application:{3} and Service:{3}", entityIdListAsString, entityContext.ToXml(), entityGetOptions.ToString(), callerContext.Application, callerContext.Module);
                    diagnosticActivity.LogInformation(traceMessage);
                }

                #endregion

                #endregion

                #region Step : Load Feature configs

                MDMFeatureConfig entityDataSourceTrackingConfig = MDMFeatureConfigHelper.GetMDMFeatureConfig(MDMCenterApplication.DataQualityManagement, "Entity data source tracking", "1");
                Boolean sourceEnabled = entityDataSourceTrackingConfig != null && entityDataSourceTrackingConfig.IsEnabled;

                if (isTracingEnabled)
                {
                    diagnosticActivity.LogDurationInfo("Loaded feature configurations");
                }

                #endregion

                #region Step : Load Entity Cache Status

                EntityCacheStatusCollection baseEntityCacheStatusCollection = new EntityCacheStatusCollection();

                if (!entityGetOptions.LoadLatestFromDB)
                {
                    baseEntityCacheStatusCollection = EntityCacheHelper.GetBaseEntityCacheStatusCollection(entityIdList, entityContext.ContainerId, callerContext);
                }

                #region Diagnostics and tracing

                if (isTracingEnabled)
                {
                    diagnosticActivity.LogDurationInfo("Base entity cache status loaded");
                }

                #endregion

                #endregion

                #region Step : Load Base Entities

                entityCollection = LoadBaseEntities(entityIdList, entityContext, baseEntityCacheStatusCollection, entityGetOptions, callerContext, command);

                if (entityCollection == null || entityCollection.Count < 1)
                {
                    #region Diagnostics and tracing

                    if (isTracingEnabled)
                    {
                        diagnosticActivity.LogWarning(String.Format("No base entity found for requested entity id list: {0}. Flow would return 0 entities.", entityIdListAsString));
                    }

                    #endregion

                    return entityCollection; //TODO:: Place proper tracing and return message
                }

                #region Diagnostics and tracing

                if (isTracingEnabled)
                {
                    diagnosticActivity.LogDurationInfo("Base entity data loaded");
                }

                #endregion

                #endregion

                #region Step : Publish Entity Loading Event

                if (entityGetOptions.PublishEvents && _isEntityLoadingEventEnabled)
                {
                    #region Step : Load Entity Context for Pre load

                    ruleParams.Entities = entityCollection;
                    //EntityContext preLoadEntityContext = PreLoadContextHelper.GetEntityContext(ruleParams, _entityManager);

                    #endregion Step : Load Entity Context for Pre load

                    #region Step : Ensure entities data for given entity context

                    var ensureEntityDataManager = new EnsureEntityDataManager(_entityManager);
                    //ensureEntityDataManager.EnsureEntityData(entityCollection, preLoadEntityContext, callerContext);

                    #endregion Step : Ensure entities data for given entity context

                    #region Diagnostics and tracing

                    if (isTracingEnabled)
                    {
                        diagnosticActivity.LogInformation("Publishing entity loading event...");
                    }

                    if (currentTraceSettings.TracingMode == TracingMode.OperationTracing)
                    {
                        diagnosticActivity.LogData("Entity data BEFORE publishing entity loading event", entityCollection.ToXml());
                    }

                    #endregion

                    //Prepare entity operation result schema

                    //TODO:: Currently there are no business scenarios which expects entity operation results. Also, the current API design is not supporting
                    //returning of operation result to the user. But the Rule framework is having a check on operation result count which is not allowing the rule 
                    //to be get fired. So, keeping in mind of the performance, the entity operation result collection is being populated with the dummy operation result.
                    EntityOperationResultCollection entityOperationResultCollection = new EntityOperationResultCollection();
                    entityOperationResultCollection.Add(new EntityOperationResult());

                    EntityOperationsCommonUtility.FireEntityEvent(entityCollection, entityCollection, null, MDMEvent.EntityLoading, _entityManager, ruleParams);

                    #region Diagnostics and tracing

                    if (isTracingEnabled)
                    {
                        diagnosticActivity.LogDurationInfo("Entity loading event processed");
                    }

                    if (currentTraceSettings.TracingMode == TracingMode.OperationTracing)
                    {
                        diagnosticActivity.LogData("Entity data AFTER entity loading event completed", entityCollection.ToXml());
                    }

                    #endregion
                }
                else
                {
                    #region Diagnostics and tracing

                    if (isTracingEnabled)
                    {
                        traceMessage = String.Format("PublishEvents/EntityLoading flags is set to FALSE. Entity loading event firing would be SKIPPED");
                        diagnosticActivity.LogInformation(traceMessage);
                    }

                    #endregion
                }

                #endregion

                #region Step : Load Entity Cache Status For Attributes(Includes Parent Entity Cache Status)

                EntityCacheStatusCollection entityCacheStatusCollection = EntityCacheHelper.GetEntityCacheStatusCollectionIncludingParents(entityCollection, baseEntityCacheStatusCollection, callerContext);

                #region Diagnostics and tracing

                if (isTracingEnabled)
                {
                    diagnosticActivity.LogDurationInfo("Entity cache status loaded for requested entities and for their parent tree");
                }

                #endregion

                #endregion

                #region Step : Load Attribute Models

                Dictionary<String, KeyValuePair<AttributeModelCollection, EntityCollection>> entitiesByAttributeModelContext = new Dictionary<String, KeyValuePair<AttributeModelCollection, EntityCollection>>();

                //Load attribute models only in case where state view or custom view is not requested..
                //If state view requested, attributes will be loaded by rule and models will be synced with attributes after entity loaded event.
                if ((entityContext.LoadAttributeModels || entityContext.LoadAttributes || entityContext.LoadOnlyInheritedValues || entityContext.LoadOnlyOverridenValues)
                    && entityContext.StateViewId < 1 && entityContext.CustomViewId < 1)
                {
                    AttributeModelBL attributeModelManager = new AttributeModelBL();

                    Dictionary<Int32, LocaleCollection> allowableLocalesForContainers = new Dictionary<Int32, LocaleCollection>();
                    LocaleCollection allowableLocales = null;
                    Collection<LocaleEnum> unmappedLocales = new Collection<LocaleEnum>();
                    ContainerContext containerContext = new ContainerContext(false, entityGetOptions.ApplySecurity, true);
                    ContainerBL containerManager = new ContainerBL();
                    
                    foreach (Entity entity in entityCollection)
                    {
                        #region Validate allowable locales

                        allowableLocalesForContainers.TryGetValue(entity.ContainerId, out allowableLocales);

                        if (allowableLocales == null)
                        {
                            Container container = containerManager.Get(entity.ContainerId, containerContext, callerContext);

                            if (container != null)
                            {
                                allowableLocales = container.SupportedLocales;
                                allowableLocalesForContainers.Add(container.Id, container.SupportedLocales);
                            }
                        }

                        foreach (LocaleEnum item in entity.EntityContext.DataLocales)
                        {
                            if (!allowableLocales.Contains(item))
                            {
                                unmappedLocales.Add(item);
                            }
                        }

                        if (unmappedLocales.Count > 0)
                        {
                            List<String> locales = new List<String>();

                            foreach (LocaleEnum item in unmappedLocales)
                            {
                                entity.EntityContext.DataLocales.Remove(item);
                                locales.Add(item.ToString());
                            }

                            String invalidLocalesString = String.Join(",", locales);

                            if (isTracingEnabled)
                            {
                                diagnosticActivity.LogWarning(String.Format("Unable to get entity: {0} for locales: {1}, as the requested data locales are not mapped to the container.", entity.Name, invalidLocalesString));
                            }
                        }

                        #endregion Validate allowable locales

                        //Category here?? This would spoil logic
                        String attrModelContextUniqueKey = String.Format("AMC_CON{0}_ET{1}_CAT{2}", entity.EntityContext.ContainerId, entity.EntityContext.EntityTypeId, entity.EntityContext.CategoryId);

                        if (entitiesByAttributeModelContext.Keys.Contains(attrModelContextUniqueKey))
                        {
                            KeyValuePair<AttributeModelCollection, EntityCollection> attributeModelCollectionAndEntityCollectionPair = entitiesByAttributeModelContext[attrModelContextUniqueKey];
                            entity.AttributeModels = attributeModelCollectionAndEntityCollectionPair.Key.Clone();
                            attributeModelCollectionAndEntityCollectionPair.Value.Add(entity);
                        }
                        else
                        {
                            AttributeModelCollection attributeModels = EntityAttributeModelHelper.GetAttributeModels(entity.Id, entity.EntityContext, entity.EntityContext.DataLocales, entityGetOptions.ApplySecurity, true, entity, attributeModelManager, currentTraceSettings, entityGetOptions.LoadAttributePermissions);
                            entity.AttributeModels = attributeModels;

                            KeyValuePair<AttributeModelCollection, EntityCollection> attributeModelCollectionAndEntityCollectionPair = new KeyValuePair<AttributeModelCollection, EntityCollection>(attributeModels, new EntityCollection { entity });
                            entitiesByAttributeModelContext.Add(attrModelContextUniqueKey, attributeModelCollectionAndEntityCollectionPair);
                        }
                    }
                }

                FilterEntitiesAttributeModelsForDisabledModules(entityCollection);

                #region Diagnostics and tracing

                if (isTracingEnabled)
                {
                    diagnosticActivity.LogDurationInfo("Attribute models loaded");
                }

                #endregion

                #endregion

                #region Step : Load Attributes

                //Currently rule based state view load is supported for Attributes..
                //So, if state view is requested, it is rule's responsibility to load attributes
                //So, skipping attributes load here when state view is requested.
                if ((entityContext.LoadAttributes || entityContext.LoadOnlyInheritedValues || entityContext.LoadOnlyOverridenValues) && entityContext.StateViewId < 1 && entityContext.CustomViewId < 1)
                {
                    foreach (KeyValuePair<String, KeyValuePair<AttributeModelCollection, EntityCollection>> pair in entitiesByAttributeModelContext)
                    {
                        KeyValuePair<AttributeModelCollection, EntityCollection> attributeModelCollectionAndEntityCollectionPair = pair.Value;

                        AttributeModelCollection attributeModels = attributeModelCollectionAndEntityCollectionPair.Key;
                        EntityCollection entities = attributeModelCollectionAndEntityCollectionPair.Value;

                        if (entities != null && entities.Count > 0)
                        {
                            LoadAttributes(entities, entityContext, attributeModels, callerContext, command, entityCacheStatusCollection, entityGetOptions);
                        }
                    }
                }
                else
                {
                    #region Diagnostics and tracing

                    if (isTracingEnabled)
                    {
                        traceMessage = String.Format("Entity attributes load would be skipped as entityContext.LoadAttributes is set to 'false'");
                        diagnosticActivity.LogInformation(traceMessage);
                    }

                    #endregion
                }

                #region Diagnostics and tracing

                if (isTracingEnabled)
                {
                    diagnosticActivity.LogDurationInfo("Entity attributes loaded");
                }

                #endregion

                #endregion

                #region Step : Load Relationships In Bulk

                if (entityContext.LoadRelationships)
                {
                    LoadRelationships(entityCollection, entityCacheStatusCollection, entityContext, entityGetOptions, callerContext);
                }
                else
                {
                    //V:if (isTracingEnabled)
                    //V:MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("Relationships load would be skipped for entity id:{0} as entityContext.LoadRelationships is set to 'false'", entity.Id), MDMTraceSource.EntityGet);
                }

                #endregion Load Relationships In Bulk

                foreach (Entity entity in entityCollection)
                {
                    Int64 entityId = entity.Id;
                    EntityCacheStatus entityCacheStatus = entityCacheStatusCollection.GetEntityCacheStatus(entityId, entity.ContainerId);

                    Boolean successFlag;

                    #region Step : Load Attribute Value based Security

                    if (entityGetOptions.ApplyAVS)
                    {
                        PermissionBL permissionBL = new PermissionBL();
                        EntityOperationResult entityOperationResult = new EntityOperationResult();

                        Permission permission = permissionBL.GetValueBasedEntityPermission(entity, entityOperationResult, _entityManager, callerContext);

                        if (permission != null && permission.PermissionSet != null && permission.PermissionSet.Count > 0)
                        {
                            //Copy all Permission set to entity object.This will useful for external user for verifying permission set at entity level rather than looping through
                            //each attribute model collection.
                            entity.PermissionSet = permission.PermissionSet;

                            if (permission.PermissionSet.Contains(UserAction.View) && !permission.PermissionSet.Contains(UserAction.Update))
                            {
                                //V:iif (isTracingEnabled)
                                //V:i    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("User '{0}' is having only View permission for entity '{1}'", _securityPrincipal.CurrentUserName, entity.Int64Name), MDMTraceSource.EntityGet);
                            }
                            else if (permission.PermissionSet.Contains(UserAction.Update))
                            {
                                //V:iif (isTracingEnabled)
                                //V:i MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("User '{0}' is having Edit/View permission for entity '{1}'", _securityPrincipal.CurrentUserName, entity.Int64Name), MDMTraceSource.EntityGet);
                            }
                        }
                        else
                        {
                            Error error = entityOperationResult.Errors.FirstOrDefault();
                            throw new MDMOperationException(error.ErrorCode, error.ErrorMessage, "EntityManager", String.Empty, "GetEntity");
                        }

                        //V:if (isTracingEnabled)
                        //V:    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("{0}ms - AVS security verified for entity id: {1}", durationHelper.GetDurationInMilliseconds(DateTime.Now), entityId), MDMTraceSource.EntityGet);
                    }
                    else
                    {
                        //V:if (isTracingEnabled)
                        //V:    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "AVS is not requested. Skipping applying of AVS on entity..", MDMTraceSource.EntityGet);
                    }

                    #endregion

                    #region Step : Load Entity Hierarchy

                    if (entityContext.LoadHierarchyRelationships)
                    {
                        if (isTracingEnabled)
                            MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("Loading entity hierarchy relationships for entity id:{0}...", entity.Id), MDMTraceSource.EntityGet);

                        successFlag = LoadHierarchyRelationships(entity, entityContext, entityGetOptions.LoadLatestFromDB, entityCacheStatus, command, entityGetOptions.UpdateCache);

                        if (successFlag && entity.HierarchyRelationships != null)
                        {
                            //V:if (isTracingEnabled)
                            //V:MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("Loaded entity hierarchy relationships for entity id:{0}. Total hierarchy relationships loaded are:{1}", entityId, entity.HierarchyRelationships.Count), MDMTraceSource.EntityGet);
                        }
                        else
                        {
                            //V:if (isTracingEnabled)
                            //V:MDMTraceHelper.EmitTraceEvent(TraceEventType.Warning, String.Format("Hierarchy relationships loading failed for entity id:{0}.", entityId), MDMTraceSource.EntityGet);
                        }

                        //V:if (isTracingEnabled)
                        //V:MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("{0}ms - Heirarchy relationships loaded for entity id: {1}", durationHelper.GetDurationInMilliseconds(DateTime.Now), entityId), MDMTraceSource.EntityGet);
                    }
                    else
                    {
                        //V:if (isTracingEnabled)
                        //V:MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("Entity hierarchy relationships load would be skipped for entity id:{0} as entityContext.LoadHierarchyRelationships is set to 'false'", entity.Id), MDMTraceSource.EntityGet);
                    }

                    #endregion

                    #region Step : Load Extensions

                    if (entityContext.LoadExtensionRelationships)
                    {
                        if (isTracingEnabled)
                            MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("Loading extensuin relationships for entity id:{0}...", entity.Id), MDMTraceSource.EntityGet);

                        successFlag = LoadExtensionRelationships(entity, entityContext, entityGetOptions.LoadLatestFromDB, entityCacheStatus, callerContext.Application, callerContext.Module, command, entityGetOptions.UpdateCache);

                        if (isTracingEnabled)
                        {
                            if (successFlag)
                            {
                                //V:MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("Loaded extension relationships for entity id:{0}. Total extension relationships loaded are:{1}", entityId, entity.ExtensionRelationships.Count), MDMTraceSource.EntityGet);
                            }
                            else
                            {
                                //V:MDMTraceHelper.EmitTraceEvent(TraceEventType.Warning, String.Format("Extension relationships loading failed for entity id:{0}.", entityId), MDMTraceSource.EntityGet);
                            }
                        }

                        //V:if (isTracingEnabled)
                        //V:MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("{0}ms - Extension relationships loaded for entity id: {1}", durationHelper.GetDurationInMilliseconds(DateTime.Now), entityId), MDMTraceSource.EntityGet);
                    }
                    else
                    {
                        //V:if (isTracingEnabled)
                        //V:MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("Extension relationships load would be skipped for entity id:{0} as entityContext.LoadExtensionRelationships is set to 'false'", entity.Id), MDMTraceSource.EntityGet);
                    }

                    #endregion

                    #region Step : Load Workflow Informations

                    if (entityContext.LoadWorkflowInformation)
                    {
                        //V:if (isTracingEnabled)
                        //V:MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("Loading Workflow Information for entity id:{0}...", entity.Id), MDMTraceSource.EntityGet);

                        successFlag = LoadWorkflowDetails(entity, callerContext.Application, callerContext.Module);

                        if (isTracingEnabled)
                        {
                            //V:MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, successFlag ? String.Format("Loaded Workflow Information for entity id:{0}", entity.Id) : String.Format("There is no Executing Workflow Information for entity id:{0}.", entityId), MDMTraceSource.EntityGet);
                        }

                        //V:if (isTracingEnabled)
                        //V:MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("{0}ms - Workflow information loaded for entity id: {1}", durationHelper.GetDurationInMilliseconds(DateTime.Now), entityId), MDMTraceSource.EntityGet);
                    }
                    else
                    {
                        //V:if (isTracingEnabled)
                        //V:MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("Workflow Information load would be skipped for entity id:{0} as entityContext.LoadWorkflowInformation is set to 'false'", entity.Id), MDMTraceSource.EntityGet);
                    }

                    #endregion

                    #region Step : Load Sources

                    if (entityContext.LoadSources && sourceEnabled)
                    {
                        //V:if (isTracingEnabled)
                        //V:MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("Loading sources for entity id:{0}...", entity.Id), MDMTraceSource.EntityGet);

                        SourceValueBL sourceValueBL = new SourceValueBL();
                        sourceValueBL.GetByEntity(entity, callerContext);

                        //V:if (isTracingEnabled)
                        //V:MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("{0}ms - sources loaded for entity id: {1}", durationHelper.GetDurationInMilliseconds(DateTime.Now), entityId), MDMTraceSource.EntityGet);
                    }
                    else
                    {
                        //V:if (isTracingEnabled)
                        //V:MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("Sources load would be skipped for entity id:{0} ", entity.Id), MDMTraceSource.EntityGet);
                    }

                    #endregion
                }

                #region Step : Fill entities

                if (String.IsNullOrWhiteSpace(callerContext.ProgramName))
                {
                    callerContext.ProgramName = "EntityBL.GetEntity"; 
                }

                entityGetOptions.FillOptions.FillLookupDisplayValues = entityContext.LoadLookupDisplayValues;
                entityGetOptions.FillOptions.FillLookupRowWithValues = entityContext.LoadLookupRowWithValues;

                EntityFillHelper.FillEntities(entityCollection, entityGetOptions.FillOptions, _entityManager, callerContext);

                #region Diagnostics and tracing

                if (isTracingEnabled)
                {
                    diagnosticActivity.LogDurationInfo("Entity fill process has been is completed");
                }

                #endregion

                #endregion

                #region Step : Update Cache Status

                // If Cache status is not dirty but the cache got evicted, then the cache status has to be updated in database.
                // When get entity is invoked as a part of the Process or through BR, the cache status should not be updated.
                if (!entityGetOptions.LoadLatestFromDB && entityGetOptions.UpdateCacheStatusInDB && entityCacheStatusCollection.IsCacheStatusUpdated())
                {
                    EntityCacheStatusBL entityCacheStatusBL = new EntityCacheStatusBL();
                    entityCacheStatusBL.Process(entityCacheStatusCollection, callerContext);

                    #region Diagnostics and tracing

                    if (isTracingEnabled)
                    {
                        diagnosticActivity.LogDurationInfo("Entity cache status is updated in database");
                    }

                    #endregion
                }
                else
                {
                    #region Diagnostics and tracing

                    if (isTracingEnabled)
                    {
                        traceMessage = String.Format("Entity cache status update to database is SKIPPED as flags are set to not do so");
                        diagnosticActivity.LogInformation(traceMessage);
                    }

                    #endregion
                }

                #endregion

                #region Publish Entity Loaded Event

                if (entityGetOptions.PublishEvents)
                {
                    #region Diagnostics and tracing

                    if (isTracingEnabled)
                    {
                        traceMessage = String.Format("Publishing entity loaded event...");
                        diagnosticActivity.LogInformation(traceMessage);
                    }

                    if (currentTraceSettings.IsDetailTracingEnabled || currentTraceSettings.TracingMode == TracingMode.OperationTracing)
                    {
                        diagnosticActivity.LogData("Entity data BEFORE EntityLoaded event", entityCollection.ToXml());
                    }

                    #endregion

                    //Prepare entity operation result schema

                    //TODO:: Currently there are no business scenarios which expects entity operation results. Also, the current API design is not supporting
                    //returning of operation result to the user. But the Rule framework is having a check on operation result count which is not allowing the rule 
                    //to be get fired. So, keeping in mind of the performance, the entity operation result collection is being populated with the dummy operation result.
                    EntityOperationResultCollection entityOperationResultCollection = new EntityOperationResultCollection();
                    entityOperationResultCollection.Add(new EntityOperationResult());

                    EntityOperationsCommonUtility.FireEntityEvent(entityCollection, entityCollection, null, MDMEvent.EntityLoaded, _entityManager, ruleParams);

                    #region Diagnostics and tracing

                    if (isTracingEnabled)
                    {
                        diagnosticActivity.LogDurationInfo("Entity loaded event processed");
                    }

                    if (currentTraceSettings.IsDetailTracingEnabled || currentTraceSettings.TracingMode == TracingMode.OperationTracing)
                    {
                        diagnosticActivity.LogData("Entity data AFTER EntityLoaded event", entityCollection.ToXml());
                    }

                    #endregion
                }
                else
                {
                    #region Diagnostics and tracing

                    if (isTracingEnabled)
                    {
                        diagnosticActivity.LogInformation("PublishEvents flags is set to FALSE. Entity loaded event firing would be SKIPPED");
                    }

                    #endregion
                }

                #endregion

            }
            finally
            {
                #region Step: Final Diagnostics and tracing

                if (isTracingEnabled)
                {
                    diagnosticActivity.LogMessageWithData("See 'View Data' for loaded entities.", entityCollection.ToXml());
                    diagnosticActivity.Stop();
                }

                #endregion
            }

            return entityCollection;
        }

        #endregion Main Get Methods

        #region Base Entity Load Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityIdList"></param>
        /// <param name="entityContext"></param>
        /// <param name="baseEntityCacheStatusCollection"></param>
        /// <param name="entityGetOptions"></param>
        /// <param name="callerContext"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        private EntityCollection LoadBaseEntities(Collection<Int64> entityIdList, EntityContext entityContext, EntityCacheStatusCollection baseEntityCacheStatusCollection, EntityGetOptions entityGetOptions, CallerContext callerContext, DBCommandProperties command)
        {
            TraceSettings currentTraceSettings = MDMOperationContextHelper.GetCurrentTraceSettings();
            Boolean isTracingEnabled = currentTraceSettings.IsBasicTracingEnabled;
            #region Step : Initial setup

            String traceMessage;

            EntityCollection baseEntityCollection = new EntityCollection();
            Collection<Int64> entityIdsToBeLoadedFromDB = new Collection<Int64>();

            LocaleEnum systemDataLocale = GlobalizationHelper.GetSystemDataLocale();

            DiagnosticActivity diagnosticActivity = new DiagnosticActivity();

            if (isTracingEnabled)
            {
                diagnosticActivity.Start();
            }

            #endregion

            try
            {
                #region Step : Try to find base entity objects into entity buffer cache

                if (!entityGetOptions.LoadLatestFromDB)
                {
                    foreach (Int64 entityIdToLoad in entityIdList)
                    {
                        EntityCacheStatus entityCacheStatus = baseEntityCacheStatusCollection.GetEntityCacheStatus(entityIdToLoad, entityContext.ContainerId);

                        //MOST IMPORTANT: Entity Buffer Cache always returns cache object which is de-serialized and not live reference
                        Boolean isBaseEntityCacheDirty = (entityCacheStatus != null) && entityCacheStatus.IsBaseEntityCacheDirty;

                        if (!isBaseEntityCacheDirty)
                        {
                            Entity entity = _entityBufferManager.FindBaseEntity(entityIdToLoad, entityContext);

                            if (entity != null)
                            {
                                entity.Action = ObjectAction.Read;
                                baseEntityCollection.Add(entity);
                            }
                            else
                            {
                                entityIdsToBeLoadedFromDB.Add(entityIdToLoad);
                                // If Cache status is not dirty and if cache is unavailable, then set for reload.
                                if (entityCacheStatus != null)
                                {
                                    entityCacheStatus.IsBaseEntityCacheDirty = true;
                                }
                            }
                        }
                        else
                        {
                            entityIdsToBeLoadedFromDB.Add(entityIdToLoad);
                        }
                    }
                }
                else
                {
                    entityIdsToBeLoadedFromDB = entityIdList;
                }


                #region Diagnostics and tracing

                if (isTracingEnabled)
                {
                    diagnosticActivity.LogInformation(String.Format("Number of entites loaded from cache-[{0}]", baseEntityCollection.Count()));
                    diagnosticActivity.LogInformation(String.Format("Number of entites to be loaded from DB-[{0}]", entityIdsToBeLoadedFromDB.Count()));
                    diagnosticActivity.LogDurationInfo("Base entities loaded from cache");
                }

                if (currentTraceSettings.TracingMode == TracingMode.OperationTracing)
                {
                    traceMessage = String.Format("List of base entities loaded from cache are: {0}", EntityOperationsCommonUtility.GetEntityIdListAsString(baseEntityCollection.GetEntityIdList()));
                    diagnosticActivity.LogInformation(traceMessage);
                }

                #endregion

                #endregion

                #region Step : if Entity object not found in cache try to load it from database..

                if (entityIdsToBeLoadedFromDB.Count > 0)
                {
                    #region Step : Load entities from DB

                    String entityIdsTobeLoadedFromDBAsString = String.Empty;

                    #region Step: Diagnostics and tracing

                    if (isTracingEnabled)
                    {
                        entityIdsTobeLoadedFromDBAsString = EntityOperationsCommonUtility.GetEntityIdListAsString(entityIdsToBeLoadedFromDB);

                        traceMessage = String.Format("List of entities TO BE loaded from database: {0}", entityIdsTobeLoadedFromDBAsString);
                        diagnosticActivity.LogInformation(traceMessage);
                    }

                    #endregion

                    EntityContext tempContext = new EntityContext();
                    tempContext.ContainerId = entityContext.ContainerId;
                    tempContext.EntityTypeId = entityContext.EntityTypeId;
                    tempContext.CategoryId = entityContext.CategoryId;

                    tempContext.Locale = entityContext.Locale;

                    tempContext.DataLocales = new Collection<LocaleEnum> {systemDataLocale};
                    if (entityContext.Locale != systemDataLocale)
                        tempContext.DataLocales.Add(entityContext.Locale);

                    tempContext.LoadEntityProperties = true;
                    tempContext.LoadAttributes = false;
                    tempContext.LoadRelationships = false;
                    tempContext.LoadHierarchyRelationships = false;
                    tempContext.LoadDependentAttributes = false;
                    tempContext.IgnoreEntityStatus = entityContext.IgnoreEntityStatus;
                    tempContext.LoadBusinessConditions = entityContext.LoadBusinessConditions;

                    EntityCollection entitiesLoadedFromDB = new EntityCollection();

                    Boolean isBulkGetDACallHappened = false;

                    //Switch to do single call / bulk get call
                    if (_isBulkGetFromDbEnabled && entityIdsToBeLoadedFromDB.Count > 1)
                    {
                        entitiesLoadedFromDB = _entityDA.BulkGet(entityIdsToBeLoadedFromDB, tempContext, systemDataLocale, null, command);
                        isBulkGetDACallHappened = true;
                    }
                    else
                    {
                        foreach (Int64 entityId in entityIdsToBeLoadedFromDB)
                        {
                            Entity entity = _entityDA.Get(entityId, tempContext, systemDataLocale, null, command, callerContext);

                            if (entity != null)
                                entitiesLoadedFromDB.Add(entity);
                        }
                    }

                    #region Step: Diagnostics and tracing

                    if (isTracingEnabled)
                    {
                        traceMessage = String.Format("Base entities loaded from database");
                        String traceMessage1 = String.Format("List of base entities loaded from database are: {0}", EntityOperationsCommonUtility.GetEntityIdListAsString(entitiesLoadedFromDB.GetEntityIdList()));
                        String traceMessage2 = String.Format("BulkGet database calls is set to: {0}", isBulkGetDACallHappened);

                        diagnosticActivity.LogDurationInfo(traceMessage);
                        diagnosticActivity.LogInformation(traceMessage1);
                        diagnosticActivity.LogInformation(traceMessage2);
                    }

                    #endregion

                    #endregion

                    #region Step : Validate entity meta-data and add entity into final return bucket

                    if (entitiesLoadedFromDB != null && entitiesLoadedFromDB.Count > 0)
                    {
                        foreach (Entity entity in entitiesLoadedFromDB)
                        {
                            #region Step : Validate, if entity type found is 'Category' then ContainerId is compulsory to provide

                            if (entity.EntityTypeId == 6 && entityContext.ContainerId < 1)
                            {
                                traceMessage = String.Format("Entity get operation failed for entity id:{0} with validation error. Exception: ContainerId is must to provide while fetching category.", entity.Id);
                                diagnosticActivity.LogError("111788", traceMessage);

                                _localeMessage = _localeMessageBL.Get(GlobalizationHelper.GetSystemUILocale(), "111788", new Object[] {entity.Id}, false, callerContext);
                                throw new MDMOperationException("111788", _localeMessage.Message, "EntityManager", String.Empty, "LoadBaseEntity"); //Entity Get Failed. ContainerId is must while fetching Category.
                            }

                            if (entityContext.EntityTypeId > 0 && entity.EntityTypeId != entityContext.EntityTypeId)
                            {
                                traceMessage = String.Format("Entity get operation failed for entity id:{0} - Requested EntityTypeId ({1}) does not match the retrieved EntityTypeId ({2}).",
                                    entity.Id, entityContext.EntityTypeId, entity.EntityTypeId);

                                diagnosticActivity.LogError("113829", traceMessage);

                                _localeMessage = _localeMessageBL.Get(GlobalizationHelper.GetSystemUILocale(), "113829", new object[] { entity.Id, entityContext.EntityTypeId, entity.EntityTypeId }, false, callerContext);
                                throw new MDMOperationException("113829", _localeMessage.Message, "EntityManager", String.Empty, "LoadBaseEntity"); //Entity Get Failed. ContainerId is must while fetching Category.
                            }

                            #endregion

                            entity.EntityContext = null; // Place null for base entity context when loaded from db. Later step would update context

                            baseEntityCollection.Add(entity);

                            if (isTracingEnabled)
                            {
                                entityIdsToBeLoadedFromDB.Remove(entity.Id);
                            }
                        }
                    }

                    #endregion

                    #region Step : Fill missing data into entity properties

                    if (entitiesLoadedFromDB != null && entitiesLoadedFromDB.Count > 0)
                    {
                        foreach (Entity entity in entitiesLoadedFromDB)
                        {
                            //Need to update entity.Locale before updating entity's metadata because we use Entity.Locale for doing Get() for metadata.
                            entity.Locale = entityContext.Locale;

                            if (entity.Action != ObjectAction.Delete)
                            {
                                EntityFillHelper.FillEntityProperties(entity, entityGetOptions.FillOptions, _entityManager, callerContext); //Fill entity properties
                            }
                        }

                        #region Diagnostics and tracing

                        if (isTracingEnabled)
                        {
                            diagnosticActivity.LogDurationInfo(String.Format("Fill entity properties for entity ids:{0}", entityIdsTobeLoadedFromDBAsString));
                        }

                        #endregion
                    }

                    #endregion

                    #region Step : Update entity base object cache

                    if (entityGetOptions.UpdateCache)
                    {
                        try
                        {
                            if (entitiesLoadedFromDB != null && entitiesLoadedFromDB.Count > 0)
                            {
                                foreach (Entity entity in entitiesLoadedFromDB)
                                {
                                    if (entity.Action != ObjectAction.Delete)
                                    {
                                        _entityBufferManager.UpdateBaseEntity(entity, entityContext);
                                    }
                                }

                                #region Diagnostics and tracing

                                if (isTracingEnabled)
                                {
                                    diagnosticActivity.LogDurationInfo(String.Format("Base entity cache updated for entity ids:{0}", entityIdsTobeLoadedFromDBAsString));
                                }

                                #endregion
                            }
                        }
                        catch (Exception ex) //Ignore the error..
                        {
                            #region Diagnostics and tracing

                            traceMessage = String.Format("Internal error occured during base entiy cache update for entity ids: {0}. Exception is: {1}", entityIdsTobeLoadedFromDBAsString, ex.ToString());
                            diagnosticActivity.LogError(traceMessage);

                            #endregion
                        }
                    }

                    #endregion
                }

                #endregion

                #region Step : Write traces if any particular entity is not found in cache / db

                if (isTracingEnabled && entityIdsToBeLoadedFromDB.Count > 0)
                {
                    traceMessage = String.Format("Base entities not found for entity ids: {0}", EntityOperationsCommonUtility.GetEntityIdListAsString(entityIdsToBeLoadedFromDB));
                    diagnosticActivity.LogWarning(traceMessage);
                }

                #endregion

                #region Step : Update entity context object in base entity

                foreach (Entity entity in baseEntityCollection)
                {
                    EntityContext clonedEntityContext = (EntityContext) entityContext.Clone();

                    clonedEntityContext.ContainerId = entity.ContainerId;
                    clonedEntityContext.EntityTypeId = entity.EntityTypeId;
                    clonedEntityContext.CategoryId = entity.CategoryId;

                    //Read entity locale from its context as cached entity locale would be neutral.
                    entity.Locale = entityContext.Locale;

                    //Need to update EntityContext here because few cache related methods will need Entity.EntityContext.Locales
                    entity.EntityContext = clonedEntityContext;
                }

                #endregion
            }
            finally
            {
                #region Step : Final traces

                if (isTracingEnabled)
                {
                    foreach (Entity entity in baseEntityCollection)
                    {
                        diagnosticActivity.LogMessageWithData(String.Format("Entity [{0}] Loaded. Details in View Data", entity.Id), entity.ToXml());
                    }

                    diagnosticActivity.Stop();
                }

                #endregion
            }

            return baseEntityCollection;
        }

        #endregion

        #region Attributes Load Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="entityContext"></param>
        /// <param name="attributeModels"></param>
        /// <param name="callerContext"></param>
        /// <param name="command"></param>
        /// <param name="entityCacheStatusCollection"></param>
        /// <param name="entityGetOptions"></param>
        /// <returns></returns>
        private Boolean LoadAttributes(EntityCollection entities, EntityContext entityContext, AttributeModelCollection attributeModels, CallerContext callerContext, DBCommandProperties command, EntityCacheStatusCollection entityCacheStatusCollection, EntityGetOptions entityGetOptions)
        {
            Boolean isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;
            String traceMessage;
            Boolean successFlag = true;

            DiagnosticActivity diagnosticActivity = new DiagnosticActivity();

            try
            {
                #region Step : Initial Setup

                if (entities == null || entities.Count < 1)
                {
                    return successFlag;
                }

                if (isTracingEnabled)
                {
                    diagnosticActivity.Start();
                }

                //DO nothing if no need to load entity attributes in context..
                if (!(entityContext.LoadAttributes || entityContext.LoadOnlyOverridenValues || entityContext.LoadOnlyInheritedValues))
                {
                    #region Diagnostics and tracing

                    if (isTracingEnabled)
                    {
                        traceMessage = String.Format("LoadAttributes flag is set to false. System would return without loading any attribute");
                        diagnosticActivity.LogInformation(traceMessage);
                    }

                    #endregion

                    return successFlag;
                }

                LocaleEnum systemDataLocale = GlobalizationHelper.GetSystemDataLocale();

                //Check locale detail in EntityContext and populate System Default Data Locale if no locale detail is provided.
                EntityOperationsCommonUtility.ValidateAndUpdateEntityContextForLocales(entityContext, callerContext.Application);

                //Since we are constructing object from scratch, we dont need this check while adding attribute object under attribute collection
                Boolean nonLocalizableDuplicateAttributeAllowed = true;

                #endregion

                #region Step : Load attribute info list to be fetched for each requested entity

                Dictionary<int, AttributeModel> attributeInfoListTobeFetched = new Dictionary<Int32, AttributeModel>();

                Int32 totalAttributesTobeFetched;

                if (attributeModels != null && attributeModels.Count > 0)
                {
                    foreach (AttributeModel attrModel in attributeModels)
                    {
                        LocaleEnum attrModelLocale = (attrModel.IsLocalizable) ? attrModel.Locale : systemDataLocale;

                        Int32 attributeInfoTobeFetched = MDM.BusinessObjects.Attribute.GetInternalUniqueKey(attrModel.Id, attrModelLocale);

                        if (!attributeInfoListTobeFetched.Keys.Contains(attributeInfoTobeFetched))
                        {
                            attributeInfoListTobeFetched.Add(attributeInfoTobeFetched, attrModel);
                        }
                    }

                    totalAttributesTobeFetched = attributeInfoListTobeFetched.Count;
                }
                else
                {
                    #region Diagnostics and tracing

                    if (isTracingEnabled)
                    {
                        traceMessage = String.Format("No attribute models found for entity context:{0}. System will return now with 'false' result", entityContext.ToXml());
                        diagnosticActivity.LogInformation(traceMessage);
                    }

                    #endregion

                    return false;
                }

                #region Diagnostics and tracing

                if (isTracingEnabled)
                {
                    traceMessage = String.Format("Attribute list(attribute id, locale id) populated to be loaded");
                    diagnosticActivity.LogDurationInfo(traceMessage);
                }

                #endregion

                #endregion Step : Load first entity attribute models and attribute info list

                #region Step : Verify entity cache status and create two buckets to load from db and load from cache

                Dictionary<Entity, EntityCacheStatus> entitiesTobeLoadedFromCache = new Dictionary<Entity, EntityCacheStatus>();
                Dictionary<Entity, EntityCacheStatus> entitiesTobeLoadedFromDB = new Dictionary<Entity, EntityCacheStatus>();

                if (!entityGetOptions.LoadLatestFromDB)
                {
                    foreach (Entity entity in entities)
                    {
                        if (entity.AttributeModels == null || entity.AttributeModels.Count < 1)
                        {
                            //Nothing to load for this entity..just continue./..WRITE TRACE HERE>>>
                            continue;
                        }

                        EntityCacheStatus entityCacheStatus = entityCacheStatusCollection.GetEntityCacheStatus(entity.Id, entity.ContainerId);

                        if (entityCacheStatus == null)
                        {
                            entityCacheStatus = new EntityCacheStatus
                            {
                                EntityId = entity.Id,
                                ContainerId = entity.ContainerId,
                                IsOverriddenAttributesCacheDirty = false,
                                IsInheritedAttributesCacheDirty = false,
                                IsCacheStatusUpdated = false
                            };
                        }

                        if (entityCacheStatus.IsOverriddenAttributesCacheDirty || entityCacheStatus.IsInheritedAttributesCacheDirty)
                        {
                            entitiesTobeLoadedFromDB.Add(entity, entityCacheStatus);
                        }
                        else
                        {
                            entitiesTobeLoadedFromCache.Add(entity, entityCacheStatus);
                        }
                    }
                }
                else
                {
                    foreach (Entity entity in entities)
                    {
                        EntityCacheStatus entityCacheStatus = new EntityCacheStatus
                        {
                            EntityId = entity.Id,
                            ContainerId = entity.ContainerId,
                            IsOverriddenAttributesCacheDirty = true,
                            IsInheritedAttributesCacheDirty = true,
                            IsCacheStatusUpdated = false
                        };

                        entitiesTobeLoadedFromDB.Add(entity, entityCacheStatus); // Load all entities from db as load latest flag is on.
                    }
                }

                #region Diagnostics and tracing

                if (isTracingEnabled)
                {
                    traceMessage = String.Format("Entity cache status verified and created two separate buckets(load from db/load from cache)");
                    diagnosticActivity.LogDurationInfo(traceMessage);

                    diagnosticActivity.LogInformation(String.Format("Number of entites to be loaded from cache-[{0}]", entitiesTobeLoadedFromCache.Count()));
                    diagnosticActivity.LogInformation(String.Format("Number of entites to be loaded from DB-[{0}]", entitiesTobeLoadedFromDB.Count()));
                }

                #endregion

                #endregion Step : Verify entity cache status and create two buckets to load from db and load from cache

                #region Step : Try to fetch entities from cache

                if (entitiesTobeLoadedFromCache.Count > 0)
                {
                    foreach (KeyValuePair<Entity, EntityCacheStatus> pair in entitiesTobeLoadedFromCache)
                    {
                        Entity entity = pair.Key;
                        EntityCacheStatus entityCacheStatus = pair.Value;

                        Int32 attrFoundInCacheCount = 0;

                        #region Step : Try to find local attributes into entity buffer cache

                        // Check if only InheritedValues has to be loaded
                        if (!entityContext.LoadOnlyInheritedValues)
                        {
                            if (!entityCacheStatus.IsOverriddenAttributesCacheDirty)
                            {
                                AttributeCollection cachedLocalAttributeCollection = _entityBufferManager.FindLocalAttributes(entity);

                                // If Cache status is not dirty and if the data is unavailable in cache, set the flag to load it
                                if (cachedLocalAttributeCollection == null)
                                {
                                    cachedLocalAttributeCollection = new AttributeCollection();
                                    entityCacheStatus.IsOverriddenAttributesCacheDirty = true;
                                }

                                foreach (Attribute localAttribute in cachedLocalAttributeCollection)
                                {
                                    if (attributeInfoListTobeFetched.ContainsKey(localAttribute.GetInternalUniqueKey()))
                                    {
                                        entity.Attributes.Add(localAttribute, nonLocalizableDuplicateAttributeAllowed);
                                        attrFoundInCacheCount++;
                                    }
                                }
                            }
                        }

                        #endregion Step : Try to find local attributes into entity buffer cache

                        #region Step : Try to find inherited attributes into entity buffer cache

                        // Check if only OverridenValues has to be loaded
                        if (!entityContext.LoadOnlyOverridenValues && attrFoundInCacheCount != totalAttributesTobeFetched)
                        {
                            if (!entityCacheStatus.IsInheritedAttributesCacheDirty)
                            {
                                AttributeCollection cachedInheritedAttributeCollection = _entityBufferManager.FindInheritedAttributes(entity);

                                // If Cache status is not dirty and if the data is unavailable in cache, set the flag to load it.
                                if (cachedInheritedAttributeCollection == null)
                                {
                                    cachedInheritedAttributeCollection = new AttributeCollection();
                                    entityCacheStatus.IsInheritedAttributesCacheDirty = true;
                                }

                                foreach (Attribute inheritedAttribute in cachedInheritedAttributeCollection)
                                {
                                    if ((attributeInfoListTobeFetched.ContainsKey(inheritedAttribute.GetInternalUniqueKey()))
                                        && (!entity.Attributes.Contains(inheritedAttribute.Id, inheritedAttribute.Locale))) //VV:: Optimize this
                                    {
                                        entity.Attributes.Add(inheritedAttribute, nonLocalizableDuplicateAttributeAllowed);
                                        attrFoundInCacheCount++;
                                    }
                                }
                            }
                        }

                        #endregion Step : Try to find inherited attributes into entity buffer cache

                        // Cache dirty is set if they are not found in cache. 
                        // If key is there and missing attributes are there then they might be blank attributes and hence not setting them to load from db
                        if (entityCacheStatus.IsOverriddenAttributesCacheDirty || entityCacheStatus.IsInheritedAttributesCacheDirty)
                        {
                            entitiesTobeLoadedFromDB.Add(entity, entityCacheStatus); //Add this entity to load from db as cache key is missing
                        }
                        else
                        {
                            if (isTracingEnabled)
                            {
                                diagnosticActivity.LogMessageWithData(String.Format("Loaded Attributes from Cache for entity [{0}]. Details in View Data", entity.Id), entity.ToXml());
                            }
                        }
                    }

                    #region Diagnostics and tracing

                    if (isTracingEnabled)
                    {
                        traceMessage = String.Format("Entity attributes loaded from cache");
                        diagnosticActivity.LogDurationInfo(traceMessage);
                    }

                    #endregion
                }

                #endregion Step : Try to fetch entities from cache

                #region Step : Load entities from the DB

                //Check if all attributes are found in cache itself, in that case just update entity object and return
                if (entitiesTobeLoadedFromDB.Count > 0)
                {
                    #region Step : Initial Setup before db call

                    Collection<Int64> entityIdListTobeLoadedFromDB = new Collection<Int64>();

                    foreach (Entity entity in entitiesTobeLoadedFromDB.Keys)
                    {
                        entityIdListTobeLoadedFromDB.Add(entity.Id);
                    }

                    //String entityIdListTobeLoadedFromDBAsString = String.Empty;

                    //if (isTracingEnabled)
                    //{
                    //    entityIdListTobeLoadedFromDBAsString = EntityOperationsCommonUtility.GetEntityIdListAsString(entityIdListTobeLoadedFromDB);
                    //}

                    #endregion Step : Initial Setup before db call

                    #region Step : Create EntityContext

                    //Get list of attributeIds from Collection<KeyValuePair<Int32, LocaleEnum>>
                    Collection<Int32> attrIdsToBeLoadedFromDB = new Collection<Int32>();
                    Collection<LocaleEnum> dataLocalesToBeLoadedFromDB = new Collection<LocaleEnum>();

                    foreach (Int32 attrInternalKey in attributeInfoListTobeFetched.Keys)
                    {
                        Int32 attrIdToBeLoadFromDB = MDM.BusinessObjects.Attribute.RetriveAttributeIdFromInternalKey(attrInternalKey);
                        LocaleEnum localeToBeLoadFromDB = MDM.BusinessObjects.Attribute.RetriveLocaleFromInternalKey(attrInternalKey);

                        if (!attrIdsToBeLoadedFromDB.Contains(attrIdToBeLoadFromDB))
                            attrIdsToBeLoadedFromDB.Add(attrIdToBeLoadFromDB);

                        if (!dataLocalesToBeLoadedFromDB.Contains(localeToBeLoadFromDB))
                            dataLocalesToBeLoadedFromDB.Add(localeToBeLoadFromDB);
                    }

                    EntityContext tempContext = new EntityContext();
                    tempContext.ContainerId = entityContext.ContainerId;
                    tempContext.EntityTypeId = entityContext.EntityTypeId;
                    tempContext.CategoryId = entityContext.CategoryId;
                    tempContext.Locale = entityContext.Locale;
                    tempContext.LoadEntityProperties = false;
                    tempContext.LoadAttributes = true;
                    tempContext.LoadBusinessConditions = entityContext.LoadBusinessConditions;
                    tempContext.LoadOnlyInheritedValues = entityContext.LoadOnlyInheritedValues;
                    tempContext.LoadOnlyOverridenValues = entityContext.LoadOnlyOverridenValues;

                    tempContext.AttributeIdList = attrIdsToBeLoadedFromDB;
                    tempContext.DataLocales = dataLocalesToBeLoadedFromDB;

                    tempContext.LoadRelationships = false;
                    tempContext.LoadHierarchyRelationships = false;
                    tempContext.LoadComplexChildAttributes = entityGetOptions.UpdateCache || entityContext.LoadComplexChildAttributes; //If we need to update cache(UpdateCache flag) then load of complex child is must else it would update cache with partial data

                    #endregion Step : Create EntityContext

                    #region Step : Get DA Call

                    EntityCollection entitiesFromDB = new EntityCollection();

                    if (_isBulkGetFromDbEnabled && entityIdListTobeLoadedFromDB.Count > 1)
                    {
                        entitiesFromDB = _entityDA.BulkGet(entityIdListTobeLoadedFromDB, tempContext, systemDataLocale, attributeModels, command);
                    }
                    else
                    {
                        foreach (Entity entity in entitiesTobeLoadedFromDB.Keys)
                        {
                            Entity tempEntity = _entityDA.Get(entity.Id, tempContext, systemDataLocale, entity.AttributeModels, command, callerContext);

                            if (tempEntity != null)
                                entitiesFromDB.Add(tempEntity);
                        }
                    }

                    #region Diagnostics and tracing

                    if (isTracingEnabled)
                    {
                        traceMessage = String.Format("Entity attributes loaded from database");
                        diagnosticActivity.LogDurationInfo(traceMessage);
                    }

                    #endregion

                    #endregion Step : Get DA Call

                    #region Step : Add attributes from DB entity to actual entity being populated

                    if (entitiesFromDB != null)
                    {
                        foreach (Entity entity in entitiesTobeLoadedFromDB.Keys)
                        {
                            Entity dbEntity = (Entity)entitiesFromDB.GetEntity(entity.Id);

                            if (dbEntity != null)
                            {
                                // UOM values are not available for complex children when loaded from database. Hence they are filled.
                                dbEntity.EntityContext = entityContext;
                                dbEntity.AttributeModels = attributeModels;

                                bool checkExists = entity.Attributes.Count > 0;

                                Collection<Int32> internalKeyList = entity.Attributes.GetAttributeInternalUniqueKeyList();

                                foreach (Attribute dbAttribute in dbEntity.Attributes)
                                {
                                    Boolean isExists = false;

                                    if (checkExists)
                                    {
                                        isExists = internalKeyList.Contains(dbAttribute.GetInternalUniqueKey());
                                    }

                                    if (!isExists)
                                    {
                                        if (entityContext.LoadOnlyOverridenValues || entityContext.LoadOnlyInheritedValues)
                                        {
                                            if (entityContext.LoadOnlyOverridenValues && dbAttribute.SourceFlag == AttributeValueSource.Overridden)
                                            {
                                                entity.Attributes.Add(dbAttribute, nonLocalizableDuplicateAttributeAllowed);
                                            }
                                            else if (entityContext.LoadOnlyInheritedValues && dbAttribute.SourceFlag == AttributeValueSource.Inherited)
                                            {
                                                entity.Attributes.Add(dbAttribute, nonLocalizableDuplicateAttributeAllowed);
                                            }
                                        }
                                        else
                                        {
                                            entity.Attributes.Add(dbAttribute, nonLocalizableDuplicateAttributeAllowed);
                                        }
                                    }
                                }

                                entity.SetValidationStates(dbEntity.GetValidationStates());
                            }

                            if (isTracingEnabled)
                            {
                                diagnosticActivity.LogMessageWithData(String.Format("Loaded Attributes from DB for entity [{0}]. Details in View Data", entity.Id), entity.ToXml());
                            }
                        }

                        #region Diagnostics and tracing

                        if (isTracingEnabled)
                        {
                            traceMessage = String.Format("Final entity objects updated for entity attributes");
                            diagnosticActivity.LogDurationInfo(traceMessage);
                        }

                        #endregion
                    }

                    #endregion Step : Add attributes from DB entity to actual entity being populated
                }

                #endregion

                #region Step : Create blank instances of attribute not found anywhere and is still missing

                if (entityContext.LoadBlankAttributes)
                {
                    foreach (Entity entity in entities)
                    {
                        if (isTracingEnabled)
                            MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "     Starting with blank attribute instance creation...", MDMTraceSource.EntityGet);

                        Collection<Int32> attributeInternalKeyListInEntity = entity.Attributes.GetAttributeInternalUniqueKeyList();

                        if (totalAttributesTobeFetched == attributeInternalKeyListInEntity.Count)
                            continue;

                        //Add the attributes which are not having values..
                        //DB call returns attributes only if they are having values
                        //So, loop through all the attribute models and add attribute with empty value
                        foreach (KeyValuePair<Int32, AttributeModel> attrInfoWithAttributeModel in attributeInfoListTobeFetched)
                        {
                            Int32 internalKey = attrInfoWithAttributeModel.Key;
                            AttributeModel attributeModel = attrInfoWithAttributeModel.Value;

                            if (!attributeInternalKeyListInEntity.Contains(internalKey) && attributeModel.AttributeModelType != AttributeModelType.Relationship)
                            {
                                Attribute attribute = new Attribute(attributeModel);

                                //If attribute having no value and is inheritable then set the default source flag to "I"
                                if (attributeModel.Inheritable)
                                {
                                    attribute.SourceFlag = AttributeValueSource.Inherited;
                                }

                                if (!attributeModel.IsLocalizable)
                                    attribute.Locale = systemDataLocale;

                                if (entityContext.LoadOnlyOverridenValues || entityContext.LoadOnlyInheritedValues)
                                {
                                    if (entityContext.LoadOnlyOverridenValues && attribute.SourceFlag == AttributeValueSource.Overridden)
                                    {
                                        entity.Attributes.Add(attribute, nonLocalizableDuplicateAttributeAllowed);
                                    }
                                    else if (entityContext.LoadOnlyInheritedValues && attribute.SourceFlag == AttributeValueSource.Inherited)
                                    {
                                        entity.Attributes.Add(attribute, nonLocalizableDuplicateAttributeAllowed);
                                    }
                                    else if (attribute.IsHierarchical && entityContext.LoadOnlyOverridenValues && attribute.SourceFlag == AttributeValueSource.Inherited)
                                    {
                                        // For hierarchical attribute, return empty attribute when asked for O values, though Source is I.
                                        entity.Attributes.Add(attribute, nonLocalizableDuplicateAttributeAllowed);
                                    }
                                }
                                else
                                {
                                    entity.Attributes.Add(attribute, nonLocalizableDuplicateAttributeAllowed);
                                }
                            }
                        }
                    }

                    #region Diagnostics and tracing

                    if (isTracingEnabled)
                    {
                        traceMessage = String.Format("Blank attribute instances created for entity attributes");
                        diagnosticActivity.LogDurationInfo(traceMessage);
                    }

                    #endregion
                }

                #endregion

                #region Step : Update attributes into respective entity buffer cache(Local or Inh), if flag says so

                if (entityGetOptions.UpdateCache)
                {
                    foreach (Entity entity in entities)
                    {
                        AttributeCollection attributeCollection = entity.Attributes;

                        if (attributeCollection != null && attributeCollection.Count > 0)
                        {
                            AttributeCollection localAttributes = new AttributeCollection();
                            AttributeCollection inheritedAttributes = new AttributeCollection();

                            foreach (Attribute attribute in attributeCollection)
                            {
                                if (attribute.SourceFlag == AttributeValueSource.Overridden)
                                {
                                    localAttributes.Add(attribute, nonLocalizableDuplicateAttributeAllowed);
                                }
                                else if (attribute.SourceFlag == AttributeValueSource.Inherited)
                                {
                                    inheritedAttributes.Add(attribute, nonLocalizableDuplicateAttributeAllowed);
                                }
                            }

                            try
                            {
                                if (!entityContext.LoadOnlyInheritedValues)
                                {
                                    //Update local attribute cache
                                    _entityBufferManager.UpdateLocalAttributes(entity, localAttributes);
                                }

                                if (!entityContext.LoadOnlyOverridenValues)
                                {
                                    //Update inherited attribute cache
                                    _entityBufferManager.UpdateInheritedAttributes(entity, inheritedAttributes);
                                }
                            }
                            catch (Exception ex) //Ignore the error..
                            {
                                #region Diagnostics and tracing

                                traceMessage = String.Format("Internal error occured during entiy attributes cache update. Exception is: {0}", ex.ToString());
                                diagnosticActivity.LogError(traceMessage);

                                #endregion
                            }
                        }
                    }

                    #region Diagnostics and tracing

                    if (isTracingEnabled)
                    {
                        traceMessage = String.Format("Entity attributes cache updated");
                        diagnosticActivity.LogDurationInfo(traceMessage);
                    }

                    #endregion
                }

                #endregion

            }
            finally
            {
                #region Step : Final traces

                if (isTracingEnabled)
                {
                    diagnosticActivity.LogMessageWithData("See 'View Data' for loaded attributes.", entities != null ? entities.ToXml() : String.Empty);
                    diagnosticActivity.Stop();
                }

                #endregion
            }

            return successFlag;
        }

        #endregion

        #region Relationships Load Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityCollection"></param>
        /// <param name="entityCacheStatusCollection"></param>
        /// <param name="entityContext"></param>
        /// <param name="entityGetOptions"></param>
        /// <param name="callerContext"></param>
        private void LoadRelationships(EntityCollection entityCollection, EntityCacheStatusCollection entityCacheStatusCollection, EntityContext entityContext, EntityGetOptions entityGetOptions, CallerContext callerContext)
        {
            Dictionary<Int32, EntityCollection> entitiesByContainer = new Dictionary<Int32, EntityCollection>();
            Dictionary<Int32, EntityCacheStatusCollection> cacheStatusesByContainer = new Dictionary<Int32, EntityCacheStatusCollection>();

            #region Create Batches per Container

            foreach (Entity entity in entityCollection)
            {
                if (entitiesByContainer.ContainsKey(entity.ContainerId))
                {
                    entitiesByContainer[entity.ContainerId].Add(entity);

                    EntityCacheStatus entityCacheStatus = entityCacheStatusCollection.GetEntityCacheStatus(entity.Id, entity.ContainerId);
                    cacheStatusesByContainer[entity.ContainerId].Add(entityCacheStatus);
                }
                else
                {
                    entitiesByContainer.Add(entity.ContainerId, new EntityCollection() { entity });

                    EntityCacheStatus entityCacheStatus = entityCacheStatusCollection.GetEntityCacheStatus(entity.Id, entity.ContainerId);
                    cacheStatusesByContainer.Add(entity.ContainerId, new EntityCacheStatusCollection() { entityCacheStatus });
                }
            }

            #endregion Create Batches per Container

            #region LoadRelationships per Container batch

            foreach (KeyValuePair<Int32, EntityCollection> pair in entitiesByContainer)
            {
                #region Create RelationshipContext

                RelationshipContext relationshipContext = entityContext.RelationshipContext;

                relationshipContext.Locale = entityContext.Locale;
                relationshipContext.ContainerId = pair.Key;
                relationshipContext.ApplyDMS = entityGetOptions.ApplySecurity;
                relationshipContext.DataLocales = entityContext.DataLocales;

                #endregion Create RelationshipContext

                EntityCacheStatusCollection cacheStatusPerContainer = cacheStatusesByContainer[pair.Key];

                Boolean successFlag = LoadRelationshipsInBulk(pair.Value, relationshipContext, cacheStatusPerContainer, entityGetOptions.LoadLatestFromDB, entityGetOptions.UpdateCache, entityGetOptions.ApplySecurity, callerContext);

                if (successFlag == false)
                {
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, String.Format("LoadRelationshipsBulk returned false for ContainerId = {0} and Entity Ids = {1}", pair.Key, ValueTypeHelper.JoinCollection<Int64>(pair.Value.GetEntityIdList(), ",")), MDMTraceSource.EntityGet);
                }
            }

            #endregion LoadRelationships per Container batch
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="relationshipContext"></param>
        /// <param name="entityCacheStatusCollection"></param>
        /// <param name="loadLatest"></param>
        /// <param name="updateCache"></param>
        /// <param name="applyDMS"></param>
        /// <param name="callerContext"></param>
        /// <returns></returns>
        private Boolean LoadRelationshipsInBulk(EntityCollection entities, RelationshipContext relationshipContext, EntityCacheStatusCollection entityCacheStatusCollection, Boolean loadLatest, Boolean updateCache, Boolean applyDMS, CallerContext callerContext)
        {
            Boolean isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;
            if (isTracingEnabled)
            {
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("Starting load relationships (in bulk) for entity ids:{0}...", ValueTypeHelper.JoinCollection<Int64>(entities.GetEntityIdList(), ",")), MDMTraceSource.EntityGet);
            }

            RelationshipBL relationshipBL = new RelationshipBL();
            Boolean successFlag = relationshipBL.LoadRelationships(entities, relationshipContext, entityCacheStatusCollection, callerContext, loadLatest, _entityManager, updateCache);

            if (isTracingEnabled)
            {
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("Done with loading relationships (in bulk) for entity ids:{0}...", ValueTypeHelper.JoinCollection<Int64>(entities.GetEntityIdList(), ",")), MDMTraceSource.EntityGet);
            }

            return successFlag;
        }

        #endregion

        #region Hierarchy Relationships Load Methods

        /// <summary>
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="entityContext"></param>
        /// <param name="loadLatest"></param>
        /// <param name="entityCacheStatus">Specifies the entity cache status</param>
        /// <param name="command"></param>
        /// <param name="updateCache">Specifies whether to update cache</param>
        /// <returns></returns>
        private Boolean LoadHierarchyRelationships(Entity entity, EntityContext entityContext, Boolean loadLatest, EntityCacheStatus entityCacheStatus, DBCommandProperties command, Boolean updateCache)
        {
            Boolean isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;
            if (isTracingEnabled)
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("Staring load hierarchy relationships logic for entity id:{0}...", entity.Id), MDMTraceSource.EntityGet);

            //Note: caching is part of hierarchy relationships BL..

            HierarchyRelationshipBL hierarchyRelationshipBL = new HierarchyRelationshipBL();
            Boolean successFlag = hierarchyRelationshipBL.LoadHierarchyRelationships(entity, false, loadLatest, entityCacheStatus, updateCache, false);

            if (isTracingEnabled)
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("Done with load hierarchy relationships logic for entity id:{0}.", entity.Id), MDMTraceSource.EntityGet);

            return successFlag;
        }

        #endregion

        #region Extension Relationships Load Methods

        /// <summary>
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="entityContext"></param>
        /// <param name="loadLatest"></param>
        /// <param name="entityCacheStatus"></param>
        /// <param name="application"></param>
        /// <param name="module"></param>
        /// <param name="command"></param>
        /// <param name="updateCache"></param>
        /// <returns></returns>
        private Boolean LoadExtensionRelationships(Entity entity, EntityContext entityContext, Boolean loadLatest, EntityCacheStatus entityCacheStatus, MDMCenterApplication application, MDMCenterModules module, DBCommandProperties command, Boolean updateCache)
        {
            Boolean isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;
            if (isTracingEnabled)
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("Staring load extension relationships logic for entity id:{0}...", entity.Id), MDMTraceSource.EntityGet);

            //Note: caching is part of extensions relationships BL..

            Boolean successFlag = true;

            ExtensionRelationshipBL extensionRelationshipBL = new ExtensionRelationshipBL();
            EntityOperationResult entityOperationResult = extensionRelationshipBL.Load(entity, loadLatest, entityCacheStatus, application, module, updateCache, false);

            if (entityOperationResult != null && entityOperationResult.HasError)
            {
                successFlag = false;
            }

            if (isTracingEnabled)
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("Done with load extension relationships logic for entity id:{0}.", entity.Id), MDMTraceSource.EntityGet);

            return successFlag;
        }

        #endregion

        #region Workflow detail Load Methods

        /// <summary>
        /// Load the workflow details as part of entity
        /// </summary>
        /// <param name="entity">Indicates the entity</param>
        /// <param name="application">Indicates the MdmCenter Application</param>
        /// <param name="module">Indicates the module name</param>
        /// <returns>Returns the boolean value based on the result</returns>
        public Boolean LoadWorkflowDetails(Entity entity, MDMCenterApplication application, MDMCenterModules module)
        {
            Boolean successFlag = false;

            if (entity != null)
            {
                CallerContext callerContext = new CallerContext(application, module);
                WorkflowInstanceBL workflowInstanceBL = new WorkflowInstanceBL();

                TrackedActivityInfoCollection trackedActivityInfoCollection = workflowInstanceBL.GetWorkflowExecutionDetails(entity.Id, entity.EntityContext.WorkflowName, callerContext, getAll: true);

                if (trackedActivityInfoCollection != null && trackedActivityInfoCollection.Count > 0)
                {
                    IEnumerable<TrackedActivityInfo> filteredActivities = trackedActivityInfoCollection.ToList().Where(act => act.Status.ToLowerInvariant() == "executing");
                    TrackedActivityInfo[] filteredTrackedActivityInfos = filteredActivities as TrackedActivityInfo[] ?? filteredActivities.ToArray();

                    if (filteredTrackedActivityInfos.Any())
                    {
                        WorkflowStateCollection workflowStateCollection = new WorkflowStateCollection();

                        foreach (TrackedActivityInfo runningActivity in filteredTrackedActivityInfos)
                        {
                            //Create WorkflowState object from tracked activity info
                            WorkflowState wfState = new WorkflowState(runningActivity);

                            TrackedActivityInfo previousActivityInfo = trackedActivityInfoCollection.Where(act => act.ActivityShortName == runningActivity.PreviousActivityShortName).OrderByDescending(a => ValueTypeHelper.ConvertToDateTime(a.EventDate)).FirstOrDefault();

                            if (previousActivityInfo != null)
                            {
                                wfState.PreviousActivityShortName = previousActivityInfo.ActivityShortName;
                                wfState.PreviousActivityLongName = previousActivityInfo.ActivityLongName;
                                wfState.PreviousActivityUserId = previousActivityInfo.ActedUserId;
                                wfState.PreviousActivityUser = previousActivityInfo.ActedUser;
                                wfState.PreviousActivityComments = previousActivityInfo.ActivityComments;
                                wfState.PreviousActivityAction = previousActivityInfo.PerformedAction;
                                wfState.PreviousActivityEventDate = runningActivity.PreviousActivityStartDateTime;
                            }
                            else //If there is no previous Activity then running activity will be the first activity. 
                            {
                                //Here Previous Activity Comments will be the first/previous activity comments which is nothing but the workflow comments. 
                                wfState.PreviousActivityComments = runningActivity.LastActivityComments;
                            }

                            workflowStateCollection.Add(wfState);
                        }

                        entity.WorkflowStates = workflowStateCollection;
                        successFlag = true;
                    }
                }
            }
            else
            {
                throw new ArgumentNullException("Requested entity object is null");
            }

            return successFlag;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityGUID"></param>
        /// <param name="workflowName"></param>
        /// <param name="callerContext"></param>
        /// <returns></returns>
        public WorkflowStateCollection LoadWorkflowDetails(String entityGUID, String workflowName, CallerContext callerContext)
        {
            WorkflowStateCollection workflowStateCollection = null; ;

            if (String.IsNullOrWhiteSpace(entityGUID) || String.IsNullOrWhiteSpace(workflowName))
            {
                // ToDO replace with error message code
            }

            WorkflowInstanceBL workflowInstanceBL = new WorkflowInstanceBL();

            TrackedActivityInfoCollection trackedActivityInfoCollection = workflowInstanceBL.GetWorkflowExecutionDetails(entityGUID, workflowName, callerContext, getAll: true);

            if (trackedActivityInfoCollection != null && trackedActivityInfoCollection.Count > 0)
            {
                IEnumerable<TrackedActivityInfo> filteredActivities = trackedActivityInfoCollection.ToList().Where(act => act.Status.ToLowerInvariant() == "executing");
                TrackedActivityInfo[] filteredTrackedActivityInfos = filteredActivities as TrackedActivityInfo[] ?? filteredActivities.ToArray();

                if (filteredTrackedActivityInfos.Any())
                {
                    workflowStateCollection = new WorkflowStateCollection();

                    foreach (TrackedActivityInfo runningActivity in filteredTrackedActivityInfos)
                    {
                        //Create WorkflowState object from tracked activity info
                        WorkflowState wfState = new WorkflowState(runningActivity);

                        TrackedActivityInfo previousActivityInfo = trackedActivityInfoCollection.Where(act => act.ActivityShortName == runningActivity.PreviousActivityShortName).OrderByDescending(a => ValueTypeHelper.ConvertToDateTime(a.EventDate)).FirstOrDefault();

                        if (previousActivityInfo != null)
                        {
                            wfState.PreviousActivityShortName = previousActivityInfo.ActivityShortName;
                            wfState.PreviousActivityLongName = previousActivityInfo.ActivityLongName;
                            wfState.PreviousActivityUserId = previousActivityInfo.ActedUserId;
                            wfState.PreviousActivityUser = previousActivityInfo.ActedUser;
                            wfState.PreviousActivityComments = previousActivityInfo.ActivityComments;
                            wfState.PreviousActivityAction = previousActivityInfo.PerformedAction;
                            wfState.PreviousActivityEventDate = runningActivity.PreviousActivityStartDateTime;
                        }
                        else //If there is no previous Activity then running activity will be the first activity. 
                        {
                            //Here Previous Activity Comments will be the first/previous activity comments which is nothing but the workflow comments. 
                            wfState.PreviousActivityComments = runningActivity.LastActivityComments;
                        }

                        workflowStateCollection.Add(wfState);
                    }
                }
            }
     
            return workflowStateCollection;
        }

        #endregion

        #region GetEntity Support Methods

        /// <summary>
        /// Gets entity batch for given entity identifier list
        /// </summary>
        /// <param name="entityIds">Indicates entity identifiers for which needs to get entity batch</param>
        /// <param name="start">Indicates start index for get entity batch</param>
        /// <param name="end">Indicates end index for get entity batch</param>
        /// <param name="entityContext">Indicates entity context for which entity needs to be populate</param>
        /// <param name="entityGetOptions">Indicates entity get options</param>
        /// <param name="callerContext">Indicates application and module name by which action is being performed</param>
        /// <param name="command">Indicates all the db command properties such as connection string etc.</param>
        /// <param name="bulkEntityGetBatchSize">Indicates bulk entity get batch size for specify in one batch how many entities needs to get</param>
        /// <returns>Returns entity list based on entity identifier and entity context</returns>
        private List<Entity> GetEntityBatch(Collection<Int64> entityIds, Int32 start, Int32 end, EntityContext entityContext, EntityGetOptions entityGetOptions, CallerContext callerContext, DBCommandProperties command, Int32 bulkEntityGetBatchSize)
        {
            Boolean isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;
            // This get method is called by the parallel loop. This will process the entities between the start and end parameter inside the entityId list collection
            DiagnosticActivity diagnosticActivity = new DiagnosticActivity();

            if (isTracingEnabled)
            {
                diagnosticActivity.Start();
                diagnosticActivity.ExecutionContext.CallDataContext.EntityIdList = entityIds;
                diagnosticActivity.LogInformation("Starting an individual thread for parallel get for the batch " + start + " to " + end + "...");
            }

            List<Entity> entityList = new List<Entity>();
            try
            {

                Int32 entityCount = entityIds.Count;

                if (isTracingEnabled)
                {
                    diagnosticActivity.LogInformation("Number of entities for this thread: " + entityCount);
                }

                List<Int64> entityIdsForBulkGet = entityIds.Skip(start).Take((end + 1) - start).ToList();

                Int32 totalEntitiesToBeLoaded = entityIdsForBulkGet.Count();
                Int32 noOfBatchesRequired = totalEntitiesToBeLoaded / bulkEntityGetBatchSize;

                if (totalEntitiesToBeLoaded % bulkEntityGetBatchSize > 0)
                {
                    noOfBatchesRequired++;
                }

                for (Int32 batchIndex = 0; batchIndex < noOfBatchesRequired; batchIndex++)
                {
                    Collection<Int64> entityIdList = new Collection<Int64>();

                    Int32 startIndex = batchIndex * bulkEntityGetBatchSize;

                    for (Int32 currentIndex = 0; currentIndex < bulkEntityGetBatchSize; currentIndex++)
                    {
                        Int32 index = startIndex + currentIndex;

                        if (index >= totalEntitiesToBeLoaded)
                            break;

                        entityIdList.Add(entityIdsForBulkGet[index]);
                    }

                    MDMRuleParams mdmRuleParam = new MDMRuleParams()
                    {
                        DDGCallerModule = DDGCallerModule.EntityGet,
                        Events = new Collection<MDMEvent>() { MDMEvent.EntityLoading, MDMEvent.EntityLoaded },
                        UserSecurityPrincipal = _securityPrincipal,
                        CallerContext = callerContext
                    };

                    EntityCollection entityCollection = GetEntity(entityIdList, entityContext, entityGetOptions, callerContext, command, mdmRuleParam);

                    if (entityCollection != null && entityCollection.Count > 0)
                    {
                        entityList.AddRange(entityCollection);
                    }
                }
            }
            finally
            {
                if (isTracingEnabled)
                {
                    diagnosticActivity.LogInformation("Done with an individual thread for parallel Get for the batch " + start + " to " + end);
                    diagnosticActivity.Stop();
                }
            }

            return entityList;
        }

        /// <summary>
        /// </summary>
        /// <param name="entityCollection"></param>
        /// <param name="numberOfEntitiesThreads"></param>
        private void SetEntitiesAsViewMode(EntityCollection entityCollection, Int32 numberOfEntitiesThreads)
        {
            Boolean isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;
            //Filter out entities having only View Permission in Entity.PermissionSet
            IEnumerable<Entity> viewOnlyEntities = entityCollection.Where(e => e.PermissionSet != null && e.PermissionSet.Count > 0 && e.PermissionSet.Contains(UserAction.View) && !e.PermissionSet.Contains(UserAction.Update));
            Entity[] viewOnlyEntityList = viewOnlyEntities.ToArray();

            if (viewOnlyEntityList.Length > 0)
            {
                if (isTracingEnabled)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Starting entity batch creation for setting entities for view mode...");

                // Get the total view only entity count 
                Int32 numberofEntities = viewOnlyEntityList.Length;

                if (isTracingEnabled)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Concat(numberofEntities, " Entities found for setting as view mode: "));

                // do we have enough entities?
                if (numberofEntities < numberOfEntitiesThreads)
                    numberOfEntitiesThreads = 1;

                // Equally distribute the available entities across the available tasks..The left over is given to the last one
                Int32 entitybatchPerThread = (numberofEntities) / numberOfEntitiesThreads;

                if (isTracingEnabled)
                {
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Number of threads to be created: " + numberOfEntitiesThreads);
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Number of entities per thread: " + entitybatchPerThread);
                }
                // Split the entities..This is the same logic used in Import engine...Need to move this to a UTILITY method..
                Dictionary<Int32, Int32> entitySplit = SplitEntiesForThreads(numberOfEntitiesThreads, 0, numberofEntities - 1, entitybatchPerThread);

                if (isTracingEnabled)
                {
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Done with entity batch creation.");
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Starting setting attribute models as read-only in parallel...");
                }

                SM.OperationContext operationContext = SM.OperationContext.Current;
                HttpContext httpContext = HttpContext.Current;

                // Simple parallel..loop
                try
                {
                    Parallel.For(0, numberOfEntitiesThreads, i =>
                    {
                        SM.OperationContext.Current = operationContext;
                        HttpContext.Current = httpContext;

                        SetAttributeModelsAsReadOnly(viewOnlyEntityList, entitySplit[i], entitySplit[i + 1] - 1);
                    });
                }
                catch (AggregateException ex)
                {
                    if (ex.InnerException != null)
                    {
                        throw ex.InnerException;
                    }
                }
                catch
                {
                    throw;
                }

                if (isTracingEnabled)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Done with entities view mode set.");
            }
            else
            {
                if (isTracingEnabled)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "No entities were found for setting as view mode.");
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        private void SetAttributeModelsAsReadOnly(Entity[] entities, Int32 start, Int32 end)
        {
            Boolean isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;
            // This get method is called by the parallel loop. This will process the entities between the start and end parameter inside the entitye list collection
            if (isTracingEnabled)
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Starting an individual thread for the batch " + start + " to " + end + "...");

            Int32 entityCount = entities.Length;

            if (isTracingEnabled)
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Number of entities for this thread: " + entityCount);

            for (Int32 i = start; i <= end; i++)
            {
                if (i < entityCount)
                {
                    Entity entity = entities[i];
                    if (entity.AttributeModels != null && entity.AttributeModels.Count > 0)
                    {
                        foreach (AttributeModel attributeModel in entity.AttributeModels)
                        {
                            attributeModel.ReadOnly = true;
                        }
                    }
                }
            }

            if (isTracingEnabled)
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Done with an individual thread for the batch " + start + " to " + end);
        }

        /// <summary>
        /// </summary>
        /// <param name="numberOfThreads"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="entitybatchPerThread"></param>
        /// <returns></returns>
        private Dictionary<Int32, Int32> SplitEntiesForThreads(Int32 numberOfThreads, Int32 start, Int32 end, Int32 entitybatchPerThread)
        {
            Dictionary<Int32, Int32> threadBatchStart = new Dictionary<Int32, Int32>(numberOfThreads + 1);

            for (Int32 i = 0; i < numberOfThreads; i++)
            {
                threadBatchStart[i] = start + i * entitybatchPerThread;
            }

            // the last thread gets its batch and the spill over..
            threadBatchStart[numberOfThreads] = end + 1;

            return threadBatchStart;
        }

        /// <summary>
        ///     Specifies if the EntityContext has valid parameters.
        /// </summary>
        private Boolean IsEntityContextParamsValid(EntityContext entityContext)
        {
            Boolean loadAttributes = (entityContext.LoadAttributes || entityContext.LoadOnlyInheritedValues || entityContext.LoadOnlyOverridenValues);
            if (entityContext.LoadEntityProperties || loadAttributes || entityContext.LoadRelationships || entityContext.LoadHierarchyRelationships ||
                entityContext.LoadExtensionRelationships || entityContext.LoadWorkflowInformation)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Filters module-dependend attributes models which modules are disabled
        /// </summary>
        /// <param name="entities">Specifies a collection of entities which attribute models require filtering</param>
        private void FilterEntitiesAttributeModelsForDisabledModules(EntityCollection entities)
        {
            if (entities == null || entities.Count == 0)
            {
                return;
            }

            // Get DQM Module info
            //MDMFeatureConfig dqmFeatureConfig = MDMFeatureConfigHelper.GetMDMFeatureConfig(MDMCenterApplication.DataQualityManagement, "DQM", "1");
            //Boolean dqmModuleIsEnabled = dqmFeatureConfig != null ? dqmFeatureConfig.IsEnabled : true;

            Boolean dqmModuleIsEnabled = true;

            // Create a list of DQM attribute ids for filtering
            List<Int32> filteredAttributeModelIds = new List<Int32>();
            if (!dqmModuleIsEnabled)
            {
                filteredAttributeModelIds.Add((Int32)SystemAttributes.DataQualityScore);
                filteredAttributeModelIds.Add((Int32)SystemAttributes.DataQualityStatus);
            }

            if (filteredAttributeModelIds.Count > 0)
            {
                foreach (Entity entity in entities)
                {
                    AttributeModelCollection attributeModels = entity.AttributeModels;

                    if (attributeModels == null || attributeModels.Count == 0)
                    {
                        continue;
                    }

                    foreach (AttributeModel attributeModel in attributeModels.Where(am => filteredAttributeModelIds.Contains(am.Id)).ToList())
                    {
                        attributeModels.Remove(attributeModel);
                    }
                }
            }
        }

        #endregion

        #endregion Private Methods

        #endregion Methods
    }
}