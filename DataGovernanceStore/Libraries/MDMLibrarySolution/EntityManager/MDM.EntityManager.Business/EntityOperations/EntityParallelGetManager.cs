using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks.Dataflow;
using SM = System.ServiceModel;

namespace MDM.EntityManager.Business.EntityOperations
{
    using Core;
    using Core.Exceptions;
    using Data;
    using BusinessObjects;
    using BusinessObjects.Workflow;
    using BusinessObjects.Diagnostics;
    using AttributeModelManager.Business;
    using ConfigurationManager.Business;
    using Interfaces;
    using MessageManager.Business;
    using PermissionManager.Business;
    using RelationshipManager.Business;
    using Utility;
    using Helpers;
    using Workflow.PersistenceManager.Business;

    /// <summary>
    /// Specifies class for entity get manager
    /// </summary>
    internal class EntityParallelGetManager : BusinessLogicBase
    {
        #region Fields

        /// <summary>
        ///     Field denoting entity data access
        /// </summary>
        private readonly EntityDA _entityDA;

        /// <summary>
        /// Field denoting permission BL
        /// </summary>
        private readonly PermissionBL _permissionBL;

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

        /// <summary>
        /// Indicates current trace settings
        /// </summary>
        private TraceSettings _currentTraceSettings;

        #endregion

        #region Constructors

        /// <summary>
        /// 
        /// </summary>
        internal EntityParallelGetManager()
        {
            _currentTraceSettings = MDMOperationContextHelper.GetCurrentTraceSettings();
        }

        /// <summary>
        /// 
        /// </summary>
        public EntityParallelGetManager(IEntityManager entityManager)
            : this()
        {
            _entityManager = entityManager;
            _securityPrincipal = SecurityPrincipalHelper.GetCurrentSecurityPrincipal();
            _entityDA = new EntityDA();
            _localeMessageBL = new LocaleMessageBL();
            _permissionBL = new PermissionBL();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityManager"></param>
        /// <param name="securityPrincipal"></param>
        public EntityParallelGetManager(IEntityManager entityManager, SecurityPrincipal securityPrincipal)
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
        /// Gets entity read result based on entityscopes provided
        /// </summary>
        /// <param name="entityScopeCollection">EntityScope list based on which entities to be retrieved</param>
        /// <param name="entityGetOptions">Options to retrieve entities</param>
        /// <param name="callerContext">Indicates Application and Module name by which action is being performed</param>
        /// <returns>EntityReadResult containing EntityCollection and EntityOperationResultCollection</returns>
        public EntityReadResult GetEntities(EntityScopeCollection entityScopeCollection, EntityGetOptions entityGetOptions, CallerContext callerContext)
        {
            String traceMessage;

            DiagnosticActivity diagnosticActivity = new DiagnosticActivity();
            EntityReadResult entityReadResult = new EntityReadResult();

            try
            {
                #region Step : Diagnostics and tracing

                if (_currentTraceSettings.IsBasicTracingEnabled)
                {
                    CallDataContext callDataContext = new CallDataContext();
                    callDataContext.EntityIdList = entityScopeCollection.GetRequestedEntityIdList();

                    ExecutionContext executionContext = new ExecutionContext(callerContext, callDataContext, _securityPrincipal.GetSecurityContext(), entityGetOptions.ToXml());
                    executionContext.LegacyMDMTraceSources.Add(MDMTraceSource.EntityProcess);

                    diagnosticActivity.Start(executionContext);
                }

                #endregion Step : Diagnostics and tracing

                #region Step : Parameter Validation

                ValidateGetEntitiesByScopeParams(entityScopeCollection, callerContext, entityReadResult, entityGetOptions);

                #endregion Step : Parameter Validation

                if (_currentTraceSettings.IsBasicTracingEnabled)
                {
                    traceMessage = String.Format("Starting GetEntities execution flow for EntityScope List:{0}, EntityGetOptions:{2}, Application:{3} and Service:{3}", entityScopeCollection.ToXml(), entityGetOptions.ToString(), callerContext.Application, callerContext.Module);
                    diagnosticActivity.LogInformation(traceMessage);
                }

                #region Step : Check if it is a Hierarchy get and retrieve children scopes

                entityScopeCollection = GetChildrenEntityIdsAndScopes(entityScopeCollection);

                #endregion Step : Check if it is a Hierarchy get and retrieve children scopes

                #region Step : Load Base Entities & Attributes in Parallel

                Collection<EntityCacheStatusLoadRequest> cacheStatusRequests = null;

                GetBaseEntitiesAndAttributesInParallel(entityScopeCollection, entityGetOptions, callerContext, entityReadResult, out cacheStatusRequests);

                if (entityReadResult.EntityCollection == null || entityReadResult.EntityCollection.Count < 1)
                {
                    #region Diagnostics and tracing

                    if (_currentTraceSettings.IsBasicTracingEnabled)
                    {
                        diagnosticActivity.LogWarning(String.Format("No entity found for requested entity scope collection: {0}. Flow would return 0 entities.", entityScopeCollection.ToXml()));
                    }

                    #endregion

                    return entityReadResult;
                }

                #endregion Step : Load Base Entities & Attributes in Parallel

                #region Step : Diagnostics and tracing

                if (_currentTraceSettings.IsBasicTracingEnabled)
                {
                    diagnosticActivity.LogDurationInfo("Parallel BaseEntities along with Attributes Completed");
                }

                #endregion Step : Diagnostics and tracing

                #region Step : Load Entity Cache Status

                EntityCacheStatusCollection entityCacheStatusCollection = null;

                if (!entityGetOptions.LoadLatestFromDB)
                {
                    entityCacheStatusCollection = EntityCacheHelper.GetEntityCacheStatus(cacheStatusRequests, callerContext);

                    #region Diagnostics and tracing

                    if (_currentTraceSettings.IsBasicTracingEnabled)
                    {
                        diagnosticActivity.LogDurationInfo("Entity cache status loaded");
                    }

                    #endregion
                }

                #endregion

                #region Step : Run Parallel Blocks

                FillEntitiesInParallel(entityReadResult.EntityCollection, entityCacheStatusCollection, entityGetOptions, callerContext);

                #endregion Step : Run Parallel Blocks
            }
            finally
            {
                #region Step : Final Diagnostics and tracing

                if (_currentTraceSettings.IsBasicTracingEnabled)
                {
                    diagnosticActivity.LogMessageWithData("See 'View Data' for loaded entities.", entityReadResult.ToXml());
                    diagnosticActivity.Stop();
                }

                #endregion Step : Final Diagnostics and tracing
            }

            return entityReadResult;
        }

        #endregion

        #region Private Methods

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
            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("Staring load extension relationships logic for entity id:{0}...", entity.Id), MDMTraceSource.EntityGet);

            //Note: caching is part of extensions relationships BL..

            Boolean successFlag = true;

            var extensionRelationshipBL = new ExtensionRelationshipBL();
            EntityOperationResult entityOperationResult = extensionRelationshipBL.Load(entity, loadLatest, entityCacheStatus, application, module, updateCache, false);

            if (entityOperationResult != null && entityOperationResult.HasError)
            {
                successFlag = false;
            }

            if (Constants.TRACING_ENABLED)
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
                var callerContext = new CallerContext(application, module);
                var workflowInstanceBL = new WorkflowInstanceBL();
                TrackedActivityInfoCollection trackedActivityInfoCollection = workflowInstanceBL.GetWorkflowExecutionDetails(entity.Id, entity.EntityContext.WorkflowName, callerContext);

                if (trackedActivityInfoCollection != null && trackedActivityInfoCollection.Count > 0)
                {
                    IEnumerable<TrackedActivityInfo> filteredActivities = trackedActivityInfoCollection.ToList().Where(act => act.Status.ToLowerInvariant() == "executing");

                    var trackedActivityInfos = filteredActivities as TrackedActivityInfo[] ?? filteredActivities.ToArray();
                    if (trackedActivityInfos.Any())
                    {
                        var workflowStateCollection = new WorkflowStateCollection();

                        foreach (TrackedActivityInfo runningActivity in trackedActivityInfos)
                        {
                            //Create WorkflowState object from tracked activity info
                            var wfState = new WorkflowState(runningActivity);

                            TrackedActivityInfo previousActivityInfo = trackedActivityInfoCollection.Where(act => act.ActivityShortName == runningActivity.PreviousActivityShortName).OrderByDescending(a => ValueTypeHelper.ConvertToDateTime(a.EventDate)).FirstOrDefault();

                            if (previousActivityInfo != null)
                            {
                                wfState.PreviousActivityShortName = previousActivityInfo.ActivityShortName;
                                wfState.PreviousActivityLongName = previousActivityInfo.ActivityLongName;
                                wfState.PreviousActivityUserId = previousActivityInfo.ActedUserId;
                                wfState.PreviousActivityUser = previousActivityInfo.ActedUser;
                                wfState.PreviousActivityComments = previousActivityInfo.ActivityComments;
                                wfState.PreviousActivityAction = previousActivityInfo.PerformedAction;
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

        #endregion

        #region GetEntity Support Methods

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
        /// 
        /// </summary>
        /// <param name="entityScopeCollection"></param>
        /// <param name="entityGetOptions"></param>
        /// <param name="callerContext"></param>
        /// <param name="entityReadResult"></param>
        /// <param name="cacheStatusRequests"></param>
        private void GetBaseEntitiesAndAttributesInParallel(EntityScopeCollection entityScopeCollection, EntityGetOptions entityGetOptions, CallerContext callerContext, EntityReadResult entityReadResult, out Collection<EntityCacheStatusLoadRequest> cacheStatusRequests)
        {
            Int32 batchsize = AppConfigurationHelper.GetAppConfig<Int32>("MDMCenter.EntityGetManager.ParallelEntityGet.BatchSize", -1);

            Int32 maxDegreeOfParallelism = AppConfigurationHelper.GetAppConfig<Int32>("MDMCenter.EntityGetManager.ParallelEntityGet.MaxDegreeOfParallelism", 4);

            if (batchsize == -1 && entityScopeCollection != null && entityScopeCollection.Count > 0)
            {
                batchsize = entityScopeCollection.GetRequestedEntityIdList().Count();
            }

            var combineEntities = new BatchedJoinBlock<Entity, EntityOperationResult>(batchsize);
            var result = new Dictionary<String, Tuple<IList<Entity>, IList<EntityOperationResult>>>();
            SourceValueBL sourceValueBL = new SourceValueBL();
            cacheStatusRequests = new Collection<EntityCacheStatusLoadRequest>();
            var diagnosticActivity = new DiagnosticActivity();
            //Get command properties
            DBCommandProperties command = DBCommandHelper.Get(callerContext.Application, callerContext.Module, MDMCenterModuleAction.Read);
            LocaleEnum systemDataLocale = GlobalizationHelper.GetSystemDataLocale();

            if (_currentTraceSettings.IsBasicTracingEnabled)
            {
                diagnosticActivity.Start();
            }

            #region Step : Load Feature configs

            MDMFeatureConfig entityDataSourceTrackingConfig =
                MDMFeatureConfigHelper.GetMDMFeatureConfig(MDMCenterApplication.DataQualityManagement,
                    "Entity data source tracking", "1");
            Boolean sourceEnabled = entityDataSourceTrackingConfig != null && entityDataSourceTrackingConfig.IsEnabled;

            if (_currentTraceSettings.IsBasicTracingEnabled)
            {
                diagnosticActivity.LogDurationInfo("Loaded feature configurations");
            }

            #endregion

            #region DataFlow blocks to get entities & attributes in parallel

            if (_currentTraceSettings.IsBasicTracingEnabled)
            {
                diagnosticActivity.LogMessage(MessageClassEnum.Information,
                    String.Format("Parallel get started with batch size : {0} and maxDegreeOfParallelism : {1}",
                        batchsize, maxDegreeOfParallelism));
            }

            // Create an ActionBlock<Tuple<Int64, EntityContext>> that performs get entity operation. 
            var getEntityBlock = new ActionBlock<Tuple<Int64, EntityContext>>(
                idAndContext =>
                {
                    try
                    {
                        Entity entity = null;
                        var entityContext = idAndContext.Item2;

                        #region Step : Retrieve Entity

                        entity = GetSingleEntity(idAndContext.Item1, entityContext, systemDataLocale, command, callerContext);

                        #endregion Step : Retrieve Entity

                        #region Step : Load Attribute Value based Security

                        if (entityGetOptions.ApplyAVS)
                        {
                            var entityOperationResult = new EntityOperationResult();

                            Permission permission = _permissionBL.GetValueBasedEntityPermission(entity,
                                entityOperationResult, _entityManager, callerContext);

                            if (permission != null && permission.PermissionSet != null &&
                                permission.PermissionSet.Count > 0)
                            {
                                //Copy all Permission set to entity object.This will useful for external user for verifying permission set at entity level rather than looping through
                                //each attribute model collection.
                                entity.PermissionSet = permission.PermissionSet;
                            }
                            else
                            {
                                Error error = entityOperationResult.Errors.FirstOrDefault();
                                throw new MDMOperationException(error.ErrorCode, error.ErrorMessage, "EntityManager",
                                    String.Empty, "GetEntity");
                            }
                        }

                        #endregion

                        #region Step : Load Sources

                        if (entityContext.LoadSources && sourceEnabled)
                        {
                            sourceValueBL.GetByEntity(entity, callerContext);
                        }

                        #endregion

                        combineEntities.Target1.Post(entity);
                    }
                        //Dont throw exception as other tasks will End
                    catch (Exception ex)
                    {
                        EntityOperationResult failedResult = new EntityOperationResult(idAndContext.Item1, "");
                        failedResult.OperationResultStatus = OperationResultStatusEnum.Failed;
                        failedResult.Errors.Add(new Error
                        {
                            ErrorMessage = ex.Message
                        });
                        combineEntities.Target2.Post(failedResult);
                    }
                },
                // Specify a maximum degree of parallelism. 
                new ExecutionDataflowBlockOptions
                {
                    MaxDegreeOfParallelism = maxDegreeOfParallelism
                });

            //Trying to execute more work in single iteration of scope/id/context collection
            foreach (EntityScope entityScope in entityScopeCollection)
            {
                EntityContext entityContext = entityScope.EntityContextCollection.FirstOrDefault();

                #region Verify & Correct the locale vlaues in context

                entityContext.DataLocales = new Collection<LocaleEnum> {systemDataLocale};

                if (entityContext.Locale != systemDataLocale && !entityContext.DataLocales.Contains(entityContext.Locale))
                {
                    entityContext.DataLocales.Add(entityContext.Locale);
                }

                #endregion Verify & Correct the locale values in context

                foreach (Int64 entityId in entityScope.EntityIdList)
                {
                    if (!entityGetOptions.LoadLatestFromDB)
                    {
                        cacheStatusRequests.Add(new EntityCacheStatusLoadRequest
                        {
                            EntityId = entityId,
                            ContainerId = entityContext.ContainerId
                        });
                    }
                    //Posting entity id & entity context to start get entity in parallel
                    getEntityBlock.Post(Tuple.Create(entityId, entityContext));
                }
            }

            getEntityBlock.Complete();

            // Wait for all messages to propagate through the network.
            getEntityBlock.Completion.Wait();

            #endregion  DataFlow blocks to get multiple entities in parallel

            #region Preparing result collection using ActionBlock and then Populate EntityCollection

            var entityResultBlock =
                new ActionBlock<Tuple<IList<Entity>, IList<EntityOperationResult>>>(data =>
                {
                    result.Add(Guid.NewGuid().ToString(), data);
                });

            // Link the batched join block to the action block.
            combineEntities.LinkTo(entityResultBlock);

            // When the batched join block completes, set the action block also to complete.
            combineEntities.Completion.ContinueWith(delegate { entityResultBlock.Complete(); });

            // Set the batched join block to the completed state and wait for  
            // all retrieval operations to complete.
            combineEntities.Complete();
            entityResultBlock.Completion.Wait();

            if (result.Count > 0)
            {
                foreach (var resultData in result.Values)
                {
                    //Successful entity result
                    if (resultData.Item1 != null && resultData.Item1.Count > 0)
                    {
                        entityReadResult.EntityCollection.AddRange(new EntityCollection(resultData.Item1));
                    }
                    //Failed entity result
                    if (resultData.Item2 != null && resultData.Item2.Count > 0)
                    {
                        resultData.Item2.ToList().ForEach(x => entityReadResult.EntityOperationResultCollection.Add(x));
                    }
                }
            }

            if (_currentTraceSettings.IsBasicTracingEnabled)
            {
                diagnosticActivity.LogDurationInfo("Loaded All entities in Parallel");
            }

            #endregion Preparing result collection using ActionBlock and then Populate EntityCollection

            if (_currentTraceSettings.IsBasicTracingEnabled)
            {
                diagnosticActivity.Stop();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityCollection"></param>
        /// <param name="entityCacheStatusCollection"></param>
        /// <param name="entityGetOptions"></param>
        /// <param name="callerContext"></param>
        private void FillEntitiesInParallel(EntityCollection entityCollection, EntityCacheStatusCollection entityCacheStatusCollection, EntityGetOptions entityGetOptions, CallerContext callerContext)
        {
            DiagnosticActivity diagnosticActivity = new DiagnosticActivity();

            Int32 maxDegreeOfParallelism = AppConfigurationHelper.GetAppConfig<Int32>("MDMCenter.EntityGetManager.ParallelEntityGet.MaxDegreeOfParallelism", 4);

            if (_currentTraceSettings.IsBasicTracingEnabled)
            {
                diagnosticActivity.Start();
                diagnosticActivity.LogMessage(MessageClassEnum.Information, String.Format("Starting Filling Entities in parallel with maxDegreeOfParallelism : {0}", maxDegreeOfParallelism));
            }

            #region Step: Load Relationships

            var loadRelationshipsAndFillEntityBlock = new ActionBlock<Entity>(
                entity =>
                {
                    if (entity.EntityContext.LoadRelationships)
                    {
                        EntityCacheStatus entityCacheStatus = null;
                        if (entityCacheStatusCollection != null)
                        {
                            entityCacheStatus = entityCacheStatusCollection.GetEntityCacheStatus(entity.Id, entity.ContainerId);
                        }

                        //Load Relationships
                        LoadRelationships(entity, entityGetOptions, entityCacheStatus, callerContext);
                    }

                    #region Step : Fill Entity

                    if (String.IsNullOrWhiteSpace(callerContext.ProgramName))
                    {
                        callerContext.ProgramName = "EntityBL.GetEntity";
                    }

                    entityGetOptions.FillOptions.FillLookupDisplayValues = entity.EntityContext.LoadLookupDisplayValues;
                    entityGetOptions.FillOptions.FillLookupRowWithValues = entity.EntityContext.LoadLookupRowWithValues;

                    EntityFillHelper.FillEntity(entity, entityGetOptions.FillOptions, _entityManager, callerContext);

                    #endregion
                },
                // Specify a maximum degree of parallelism. 
                new ExecutionDataflowBlockOptions
                {
                    MaxDegreeOfParallelism = maxDegreeOfParallelism
                }
                );

            #endregion Step: Load Relationships

            #region Step: Load Extension relationships

            //Get command properties
            DBCommandProperties command = DBCommandHelper.Get(callerContext.Application, callerContext.Module, MDMCenterModuleAction.Read);

            var loadExtensionRelationshipsBlock = new ActionBlock<Entity>(
                entity =>
                {
                    EntityCacheStatus entityCacheStatus = null;
                    if (entityCacheStatusCollection != null)
                    {
                        entityCacheStatus = entityCacheStatusCollection.GetEntityCacheStatus(entity.Id, entity.ContainerId);
                    }
                    //Load Extension Relationships
                    LoadExtensionRelationships(entity, entity.EntityContext, entityGetOptions.LoadLatestFromDB, entityCacheStatus, callerContext.Application, callerContext.Module, command, entityGetOptions.UpdateCache);
                },
                // Specify a maximum degree of parallelism. 
                new ExecutionDataflowBlockOptions
                {
                    MaxDegreeOfParallelism = maxDegreeOfParallelism
                });

            #endregion Step: Load Extension relationships

            #region Step : Load Workflow Informations

            var loadWorkflowInfoBlock = new ActionBlock<Entity>(
                entity =>
                {
                    LoadWorkflowDetails(entity, callerContext.Application, callerContext.Module);
                }
                );

            #endregion Step : Load Workflow Informations

            foreach (Entity entity in entityCollection)
            {
                //Loadrelationships flag is being checked inside the block
                loadRelationshipsAndFillEntityBlock.Post(entity);

                if (entity.EntityContext.LoadExtensionRelationships)
                {
                    loadExtensionRelationshipsBlock.Post(entity);
                }

                if (entity.EntityContext.LoadWorkflowInformation)
                {
                    loadWorkflowInfoBlock.Post(entity);
                }
            }

            loadRelationshipsAndFillEntityBlock.Complete();
            loadExtensionRelationshipsBlock.Complete();

            loadRelationshipsAndFillEntityBlock.Completion.Wait();
            loadExtensionRelationshipsBlock.Completion.Wait();

            if (_currentTraceSettings.IsBasicTracingEnabled)
            {
                diagnosticActivity.LogMessage(MessageClassEnum.Information, "Completed filling entities in parallel");
                diagnosticActivity.Stop();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityId"></param>
        /// <param name="entityContext"></param>
        /// <param name="locale"></param>
        /// <param name="command"></param>
        /// <param name="callerContext"></param>
        /// <returns></returns>
        private Entity GetSingleEntity(Int64 entityId, EntityContext entityContext, LocaleEnum locale, DBCommandProperties command, CallerContext callerContext)
        {
            DiagnosticActivity diagnosticActivity = new DiagnosticActivity();

            if (_currentTraceSettings.IsBasicTracingEnabled)
            {
                diagnosticActivity.Start();
            }

            AttributeModelCollection attributeModelCollection = null;
            Entity entity = null;

            #region Step : Load Attribute Models

            //Load attribute models only in case where state view or custom view is not requested..
            //If state view requested, attributes will be loaded by rule and models will be synced with attributes after entity loaded event.
            if ((entityContext.LoadAttributeModels || entityContext.LoadAttributes || entityContext.LoadOnlyInheritedValues || entityContext.LoadOnlyOverridenValues)
                && entityContext.StateViewId < 1 && entityContext.CustomViewId < 1)
            {
                var attributeModelManager = new AttributeModelBL();

                AttributeModelCollection attributeModels = EntityAttributeModelHelper.GetAttributeModels(entityId, entityContext, entityContext.DataLocales);
                attributeModelCollection = attributeModelManager.SortAttributeModels(attributeModels);

                if (_currentTraceSettings.IsBasicTracingEnabled)
                {
                    diagnosticActivity.LogDurationInfo("Loaded Attribute Models for an Entity : " + entity.Name);
                }
            }

            #endregion

            #region Step : Load Entity with Attributes

            entity = _entityDA.Get(entityId, entityContext, locale, attributeModelCollection, command, callerContext);
            entity.Action = ObjectAction.Read;
            entity.CategoryId = entityContext.CategoryId;
            entity.ContainerId = entityContext.ContainerId;
            entity.EntityTypeId = entityContext.EntityTypeId;
            entity.EntityContext = (EntityContext) entityContext.Clone();

            if (_currentTraceSettings.IsBasicTracingEnabled)
            {
                diagnosticActivity.LogDurationInfo("Loaded Attributes from DB for an Entity : " + entity.Name);
            }

            #endregion

            if (_currentTraceSettings.IsBasicTracingEnabled)
            {
                diagnosticActivity.Stop();
            }

            return entity;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="entityGetOptions"></param>
        /// <param name="entityCacheStatus"></param>
        /// <param name="callerContext"></param>
        /// <returns></returns>
        private Boolean LoadRelationships(Entity entity, EntityGetOptions entityGetOptions, EntityCacheStatus entityCacheStatus, CallerContext callerContext)
        {
            //var relationshipBL = new RelationshipBL();
            //bool successFlag = relationshipBL.LoadRelationship(entity, entityCacheStatus, callerContext, false, _entityManager);

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityScopeCollection"></param>
        /// <returns></returns>
        private EntityScopeCollection GetChildrenEntityIdsAndScopes(EntityScopeCollection entityScopeCollection)
        {
            DiagnosticActivity diagnosticActivity = new DiagnosticActivity();

            if (_currentTraceSettings.IsBasicTracingEnabled)
            {
                diagnosticActivity.Start();
                diagnosticActivity.LogMessage(MessageClassEnum.Information,
                    "Started Getting child entity ids and scopes from entity scopes");
            }

            EntityScopeCollection updatedEntityScopes = new EntityScopeCollection();
            EntityDA entityDA = new EntityDA();
            foreach (EntityScope entityScope in entityScopeCollection)
            {
                EntityContext entityContext = entityScope.EntityContextCollection.FirstOrDefault();
                if (entityScope.EntityContextCollection.Count > 1 ||
                    (entityContext != null && entityContext.LoadHierarchyRelationships))
                {
                    EntityTypeCollection entityTypes = null;
                    Int64 parentEntityId = entityScope.EntityIdList.FirstOrDefault();

                    //If more than one entity context found then result should be filtered based on entity types available in contexts
                    if (entityScope.EntityContextCollection.Count > 1)
                    {
                        entityTypes = new EntityTypeCollection();

                        foreach (var context in entityScope.EntityContextCollection)
                        {
                            entityTypes.Add(new EntityType {Id = context.EntityTypeId});
                        }
                    }

                    if (_currentTraceSettings.IsBasicTracingEnabled)
                    {
                        diagnosticActivity.LogMessage(MessageClassEnum.Information,
                            String.Format(
                                "Calling GetChildrenByParentEntityId for ParentEntityId : {0}, Container : {1}",
                                parentEntityId, entityContext.ContainerId));
                    }

                    EntityScopeCollection childEntityScopes = entityDA.GetChildrenByParentEntityId(parentEntityId,
                        entityContext.ContainerId, entityTypes);

                    if (childEntityScopes != null && childEntityScopes.Count > 0)
                    {
                        if (_currentTraceSettings.IsBasicTracingEnabled)
                        {
                            diagnosticActivity.LogMessageWithData(
                                String.Format(
                                    "ChildEntityScope loaded from DB for ParentEntityId - {0}, Container - {1}",
                                    parentEntityId, entityContext.ContainerId), childEntityScopes.ToXml());
                        }

                        foreach (EntityScope childEntityScope in childEntityScopes)
                        {
                            EntityContext childEntityContext = childEntityScope.EntityContextCollection.FirstOrDefault();
                            EntityContext matchingEntityContext =
                                entityScope.EntityContextCollection.FirstOrDefault(
                                    x => childEntityContext != null && x.EntityTypeId == childEntityContext.EntityTypeId);
                            if (matchingEntityContext != null)
                            {
                                childEntityContext = (EntityContext) matchingEntityContext.Clone();
                            }
                            else
                            {
                                Int32 entityTypeId = childEntityContext.EntityTypeId;
                                childEntityContext = (EntityContext) entityContext.Clone();
                                childEntityContext.EntityTypeId = entityTypeId;
                            }
                            childEntityScope.EntityContextCollection.Clear();
                            childEntityScope.EntityContextCollection.Add(childEntityContext);

                            updatedEntityScopes.Add(childEntityScope);
                        }
                    }
                }
                else
                {
                    if (_currentTraceSettings.IsBasicTracingEnabled)
                    {
                        diagnosticActivity.LogMessage(MessageClassEnum.Information,
                            "No EntityContext found for child entities and LoadHierarchyRelationship flag is false for Entity Id : " +
                            entityScope.EntityIdList.FirstOrDefault());
                    }
                    if (entityScope.EntityContextCollection != null && entityScope.EntityContextCollection.Count != 0)
                    {
                        updatedEntityScopes.Add(entityScope);
                    }
                }
            }

            if (_currentTraceSettings.IsBasicTracingEnabled)
            {
                diagnosticActivity.Stop();
            }

            return updatedEntityScopes;
        }

        #endregion

        #region Parameter Validation Methods

        /// <summary>
        /// Validates given parameters
        /// </summary>
        /// <param name="entityScopeCollection">Indicates entity scopes for which the entities are retrieved</param>
        /// <param name="callerContext">Indicates caller context, which contains the application and module that has invoked the API/param>
        /// <param name="entityReadResult">Indicates EntityReadResult containing EntityCollection (for requested entity ids) and failed operationresult collection</param>
        /// <param name="entityGetOptions">Indicates options available while retrieving entity data</param>
        private void ValidateGetEntitiesByScopeParams(EntityScopeCollection entityScopeCollection, CallerContext callerContext, EntityReadResult entityReadResult, EntityGetOptions entityGetOptions)
        {
            DiagnosticActivity diagnosticActivity = new DiagnosticActivity();

            if (_currentTraceSettings.IsBasicTracingEnabled)
            {
                diagnosticActivity.Start();
                diagnosticActivity.LogMessage(MessageClassEnum.Information,
                    "Started Validating GetEntities Scope parameters");
            }

            if (entityScopeCollection == null || entityScopeCollection.Count < 1)
            {
                String errorMessage = "Entity Scope Collection cannot be null or empty";
                diagnosticActivity.LogMessage(MessageClassEnum.Error, errorMessage);
                throw new MDMOperationException("113894", "Entity Scope Collection cannot be null or empty",
                    "EntityGetManager", String.Empty, "GetEntities");
            }

            if (entityGetOptions == null)
            {
                _localeMessage = _localeMessageBL.Get(locale: GlobalizationHelper.GetSystemUILocale(), messageCode: "113886", loadLastest: false, callerContext: callerContext);
                diagnosticActivity.LogMessage(messageClass: MessageClassEnum.Error, message: _localeMessage.Message);
                throw new MDMOperationException(messageCode: "113886", message: _localeMessage.Message, source: "EntityGetManager", stackTrace: String.Empty, targetSite: "GetEntities");
            }

            if (entityScopeCollection.All(x => x.EntityIdList == null || x.EntityIdList.Count < 1))
            {
                String errorMessage = "Entity Id list provided in all entity scopes are null or empty";
                diagnosticActivity.LogMessage(MessageClassEnum.Error, errorMessage);
                throw new MDMOperationException("113895", "Entity Id list provided in all entity scopes are null or empty",
                    "EntityGetManager", String.Empty, "GetEntities");
            }

            foreach (EntityScope entityScope in entityScopeCollection)
            {
                if (entityScope.EntityContextCollection == null || entityScope.EntityContextCollection.Count == 0)
                {
                    //EntityContextCollection cannot be null
                    _localeMessage = _localeMessageBL.Get(GlobalizationHelper.GetSystemUILocale(), "113882", false,
                        callerContext);
                    entityScope.EntityIdList.ToList().ForEach((x) =>
                    {
                        EntityOperationResult failedResult = entityReadResult.GetFailedOperationResult(x);
                        failedResult.Errors.Add(new Error
                        {
                            ErrorMessage = _localeMessage.Message,
                            ErrorCode = "113882"
                        });
                    });
                    entityScope.EntityIdList.Clear();
                    diagnosticActivity.LogMessage(MessageClassEnum.Error, _localeMessage.Message);
                }
                else if (entityScope.EntityContextCollection.Count > 1)
                {
                    String traceMessage =
                        String.Format(
                            "More than one EntityContext found for entity scope: {0}, only first one will be used for get operation",
                            entityScope.ToXml());
                    diagnosticActivity.LogMessage(MessageClassEnum.Warning, traceMessage);

                    EntityContext entityContext = entityScope.EntityContextCollection.FirstOrDefault();

                    if (entityContext.EntityTypeId < 1 || entityContext.ContainerId < 1 || entityContext.CategoryId < 1)
                    {
                        _localeMessage = _localeMessageBL.Get(GlobalizationHelper.GetSystemUILocale(), "113885", false,
                            callerContext);
                        entityScope.EntityIdList.ToList().ForEach((x) =>
                        {
                            EntityOperationResult failedResult = entityReadResult.GetFailedOperationResult(x);
                            failedResult.Errors.Add(new Error
                            {
                                ErrorMessage = _localeMessage.Message,
                                ErrorCode = "113885"
                            });
                        });
                        entityScope.EntityIdList.Clear();
                        diagnosticActivity.LogMessage(MessageClassEnum.Error, _localeMessage.Message);
                    }

                    if (!IsEntityContextParamsValid(entityContext))
                    {
                        //In EntityContext, any one of LoadEntityProperties, LoadAttributes or LoadRelationships must be set to true
                        _localeMessage = _localeMessageBL.Get(GlobalizationHelper.GetSystemUILocale(), "111787", false,
                            callerContext);
                        entityScope.EntityIdList.ToList().ForEach((x) =>
                        {
                            EntityOperationResult failedResult = entityReadResult.GetFailedOperationResult(x);
                            failedResult.Errors.Add(new Error
                            {
                                ErrorMessage = _localeMessage.Message,
                                ErrorCode = "111787"
                            });
                        });
                        entityScope.EntityIdList.Clear();
                        diagnosticActivity.LogMessage(MessageClassEnum.Error, _localeMessage.Message);
                    }
                }
            }

            if (_currentTraceSettings.IsBasicTracingEnabled)
            {
                diagnosticActivity.LogMessageWithData("Completed validating Get Entities scope parameters",
                    entityReadResult.ToXml());
                diagnosticActivity.Stop();
            }
        }

        #endregion Parameter Validation Methods

        #endregion Private Methods

        #endregion Methods
    }
}