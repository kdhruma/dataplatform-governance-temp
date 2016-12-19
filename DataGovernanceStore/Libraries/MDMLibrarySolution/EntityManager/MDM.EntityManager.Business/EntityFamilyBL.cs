using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Transactions;

namespace MDM.EntityManager.Business
{
    using ActivityLogManager.Business;
    using BusinessObjects;
    using BusinessObjects.Diagnostics;
    using ContainerManager.Business;
    using Core;
    using Core.Extensions;
    using DataModelManager.Business;
    using EntityManager.Business.EntityOperations.Helpers;
    using HierarchyManager.Business;
    using Interfaces;
    using JigsawIntegrationManager;
    using KnowledgeManager.Business;
    using MDM.Workflow.Utility;
    using MessageManager.Business;
    using ParallelizationManager.Processors;
    using RelationshipManager.Business;
    using SearchManager.Business;
    using Utility;

    /// <summary>
    /// Specifies the business operations for entity group queue.
    /// </summary>
    public class EntityFamilyBL : BusinessLogicBase
    {
        #region Fields

        /// <summary>
        /// Field denoting the security principal
        /// </summary>
        private readonly SecurityPrincipal _securityPrincipal;

        /// <summary>
        /// Indicates current trace settings
        /// </summary>
        private TraceSettings _currentTraceSettings = null;

        /// <summary>
        /// Indicates instance of Entity BL
        /// </summary>
        private readonly IEntityManager _entityManager = null;

        /// <summary>
        /// Indicates family change context master record
        /// </summary>
        private Boolean _isMasterRecord = false;

        /// <summary>
        /// 
        /// </summary>
        private LocaleMessageBL _localeMessageBL = new LocaleMessageBL();

        /// <summary>
        /// 
        /// </summary>
        private LocaleMessage _localeMessage = null;

        /// <summary>
        /// Indicates system data locale
        /// </summary>
        private LocaleEnum _systemDataLocale = LocaleEnum.UnKnown;

        /// <summary>
        /// Indicates collection of available locale
        /// </summary>
        private Collection<LocaleEnum> _availableLocales = null;

        /// <summary>
        /// Indicates the lifecycle status attribute identifier
        /// </summary>
        private Int32 _lifecycleStatusAttributeId = (Int32)SystemAttributes.LifecycleStatus;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        internal EntityFamilyBL()
        {
            _currentTraceSettings = MDMOperationContextHelper.GetCurrentTraceSettings();
            _securityPrincipal = SecurityPrincipalHelper.GetCurrentSecurityPrincipal();
            _systemDataLocale = GlobalizationHelper.GetSystemDataLocale();
        }

        /// <summary>
        /// Initialize with instance of entity manager
        /// </summary>
        /// <param name="entityManager">Instance of entity manager</param>
        public EntityFamilyBL(IEntityManager entityManager)
            : this()
        {
            _entityManager = entityManager;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Specifies collection of available locale
        /// </summary>
        private Collection<LocaleEnum> AvailableLocales
        {
            get
            {
                if (_availableLocales == null)
                {
                    _availableLocales = GetAvailableLocales();
                }

                return _availableLocales;
            }
        }

        #endregion Properties

        #region  Methods

        #region Public Methods

        /// <summary>
        /// Process entity group queue
        /// </summary>
        /// <param name="entityFamilyQueue">Indicates entity group queue to be processed</param>
        /// <param name="callerContext">Indicates the caller context</param>
        /// <returns>Returns operation result</returns>
        public EntityOperationResultCollection Process(EntityFamilyQueue entityFamilyQueue, CallerContext callerContext)
        {
            #region Initial Setup

            EntityOperationResultCollection masterEntityOperationResults = new EntityOperationResultCollection();
            EntityFamilyQueueBL entityFamilyQueueBL = new EntityFamilyQueueBL();

            #endregion Initial Setup

            #region Step : Diagnostic Activity initialization

            DiagnosticActivity diagnosticActivity = new DiagnosticActivity();
            Boolean isTracingEnabled = _currentTraceSettings.IsBasicTracingEnabled;

            if (isTracingEnabled)
            {
                diagnosticActivity.Start();
            }

            #endregion

            try
            {
                foreach (EntityFamilyChangeContext entityFamilyChangeContext in entityFamilyQueue.EntityFamilyChangeContexts)
                {
                    EntityOperationResultCollection entityOperationResults = null;

                    Boolean isCategoryChange = entityFamilyChangeContext.EntityActivityList == EntityActivityList.CategoryChange;
                    Boolean isWorkflowChange = entityFamilyChangeContext.EntityActivityList == EntityActivityList.EntityAsyncWorkflowActivityBusinessRules;
                    Boolean isMetadaChange = entityFamilyChangeContext.EntityActivityList == EntityActivityList.MetadataChange;

                    Int64 entityId = (isCategoryChange || isMetadaChange) ? entityFamilyQueue.EntityFamilyId : entityFamilyChangeContext.EntityFamilyId;

                    Boolean hasRootEntityDeleted = IsRootEntityDeleted(entityFamilyChangeContext, entityId);

                    if (hasRootEntityDeleted)
                    {
                        EntitySearchDataBL entitySearchDataManager = new EntitySearchDataBL(_entityManager);
                        EntityOperationResultCollection dnSearchEntityOperationResults = entitySearchDataManager.RefreshEntitiesSearchData(new EntityCollection(), entityFamilyChangeContext, callerContext);

                        // Do not copy entity meta-data here as current entity operation results already have it. So passing false here.
                        masterEntityOperationResults.CopyEntityOperationResults(dnSearchEntityOperationResults, false);
                    }
                    else if (isCategoryChange)
                    {
                        entityOperationResults = PushCategoryValues(entityId, entityFamilyChangeContext, callerContext);
                    }
                    else if (isWorkflowChange)
                    {
                        EntityOperationResult entityOperationResult = Process(entityFamilyQueue, entityFamilyChangeContext, callerContext, diagnosticActivity);
                        masterEntityOperationResults.Add(entityOperationResult);
                    }
                    else
                    {
                        entityOperationResults = ProcessMain(entityId, entityFamilyChangeContext, callerContext);
                    }

                    if (entityOperationResults != null && entityOperationResults.Count > 0)
                    {
                        masterEntityOperationResults.AddRange(entityOperationResults);
                    }

                    masterEntityOperationResults.RefreshOperationResultStatus();
                }

                if (!masterEntityOperationResults.HasAnySystemError())
                {
                    //Release a lock once process is done.
                    OperationResult operationResult = entityFamilyQueueBL.MarkCompleteAndReleaseLock(entityFamilyQueue.EntityGlobalFamilyId, callerContext);

                    if (operationResult != null && operationResult.HasError)
                    {
                        masterEntityOperationResults.CopyOperationResults(new OperationResultCollection() { operationResult });
                        masterEntityOperationResults.RefreshOperationResultStatus();

                        diagnosticActivity.LogError(String.Format("Unable to release lock from Family Processor for Entity Global Family Id: {0}", entityFamilyQueue.EntityGlobalFamilyId));
                    }
                }
            }
            finally
            {
                if (isTracingEnabled)
                {
                    diagnosticActivity.Stop();
                }
            }

            return masterEntityOperationResults;
        }

        #endregion Public Methods

        #region Private Methods

        #region Get Methods

        /// <summary>
        /// Gets entity family based on given entity id
        /// </summary>
        /// <param name="entityId">Indicates entity id</param>
        /// <param name="entityFamilyChangeContext">Indicates entity family change context</param>
        /// <param name="preLoadEntityContext">Indicates pre-load entity context</param>
        /// <param name="loadAllExtensions"></param>
        /// <param name="loadLookupDisplayValues"></param>
        /// <param name="loadComplexChildAttributes"></param>
        /// <param name="callerContext">Indicates the caller context</param>
        /// <returns>Return entire entity family in hierarchy manner</returns>
        private Entity Get(Int64 entityId, EntityFamilyChangeContext entityFamilyChangeContext, EntityContext preLoadEntityContext, Boolean loadAllExtensions, Boolean loadLookupDisplayValues, Boolean loadComplexChildAttributes, CallerContext callerContext)
        {
            Entity mainEntity = null;

            #region Initial Setup

            //Family get will be always from root entity type...
            EntityContext mainEntityContext = new EntityContext();
            mainEntityContext.LoadStateValidationAttributes = true;
            mainEntityContext.LoadAttributeModels = true;
            mainEntityContext.LoadOnlyCurrentValues = (preLoadEntityContext != null) ? preLoadEntityContext.LoadOnlyCurrentValues : false;
            mainEntityContext.LoadLookupDisplayValues = loadLookupDisplayValues;
            mainEntityContext.LoadComplexChildAttributes = loadComplexChildAttributes;

            EntityUniqueIdentifier entityUniqueIdentifier = new EntityUniqueIdentifier(entityId);

            EntityChangeContextCollection entityChangeContexts = null;
            ExtensionChangeContextCollection extensionChangeContexts = null;
            EntityTypeCollection entityTypes = null;
            RelationshipContext relationshipContext = null;
            EntityOperationsBL entityOperationsBL = new EntityOperationsBL();
            Dictionary<Int32, Int32> entityTypeIdToVariantLevelMappings = entityOperationsBL.GetEntityVariantLevel(entityId, callerContext);
            Collection<String> extendedContainerNameList = new Collection<String>();

            if (entityTypeIdToVariantLevelMappings != null && entityTypeIdToVariantLevelMappings.Count > 0)
            {
                Collection<Int32> entityTypeIdList = new Collection<Int32>();

                foreach (KeyValuePair<Int32, Int32> keyValuePair in entityTypeIdToVariantLevelMappings)
                {
                    entityTypeIdList.Add(keyValuePair.Key);
                }

                EntityTypeBL entityTypeManager = new EntityTypeBL();
                entityTypes = entityTypeManager.GetEntityTypesByIds(entityTypeIdList);
            }

            Boolean hasEntitiesCreated = false;

            #endregion Initial Setup

            #region Prepare Entity hierarchy and extension context for family get

            #region  Prepare Entity Hierarchy Context

            if (entityFamilyChangeContext != null)
            {
                entityChangeContexts = entityFamilyChangeContext.VariantsChangeContext.EntityChangeContexts;
                extensionChangeContexts = entityFamilyChangeContext.ExtensionChangeContexts;
                hasEntitiesCreated = entityFamilyChangeContext.HasEntitiesCreated();

                if (_isMasterRecord)
                {
                    relationshipContext = GetRelationshipChangeContext(entityFamilyChangeContext, hasEntitiesCreated);
                }
            }

            UpdateEntityVariantsContext(entityChangeContexts, entityTypes, mainEntityContext, entityTypeIdToVariantLevelMappings, preLoadEntityContext, relationshipContext, hasEntitiesCreated);

            #endregion  Prepare Entity Hierarchy Context

            #region  Prepare Entity Extension Context

            if (loadAllExtensions)
            {
                EntityContext extensionContext = new EntityContext();
                extensionContext.ContainerQualifierName = "all";
                extensionContext.LoadAttributeModels = true;

                UpdateEntityVariantsContext(entityChangeContexts, entityTypes, extensionContext, entityTypeIdToVariantLevelMappings, preLoadEntityContext, relationshipContext, hasEntitiesCreated);
                mainEntityContext.EntityExtensionContext.AddEntityDataContext("all", extensionContext);
            }
            else if (extensionChangeContexts != null && extensionChangeContexts.Count > 0)
            {
                foreach (ExtensionChangeContext extensionChangeContext in extensionChangeContexts)
                {
                    EntityContext entityContext = new EntityContext();
                    entityContext.ContainerId = extensionChangeContext.ContainerId;
                    entityContext.ContainerName = extensionChangeContext.ContainerName;
                    entityContext.ContainerQualifierName = extensionChangeContext.ContainerQualifierName;
                    entityContext.LoadAttributeModels = true;

                    entityChangeContexts = extensionChangeContext.VariantsChangeContext.EntityChangeContexts;
                    UpdateEntityVariantsContext(entityChangeContexts, entityTypes, entityContext, entityTypeIdToVariantLevelMappings, preLoadEntityContext, relationshipContext, hasEntitiesCreated);

                    mainEntityContext.EntityExtensionContext.AddEntityDataContext(extensionChangeContext.ContainerName, String.Empty, entityContext);
                    extendedContainerNameList.Add(extensionChangeContext.ContainerName);
                }
            }

            if (preLoadEntityContext != null)
            {
                foreach (EntityContext extendedEntityContext in preLoadEntityContext.EntityExtensionContext.EntityContexts)
                {
                    if (!extendedContainerNameList.Contains(extendedEntityContext.ContainerName))
                    {
                        EntityContext entityContext = new EntityContext();
                        entityContext.ContainerName = extendedEntityContext.ContainerName;

                        UpdateEntityVariantsContext(null, entityTypes, entityContext, entityTypeIdToVariantLevelMappings, null, null, false);

                        mainEntityContext.EntityExtensionContext.AddEntityDataContext(extendedEntityContext.ContainerName, String.Empty, entityContext);
                    }
                }
            }

            #endregion Prepare Entity Extension Context

            #endregion Prepare Entity hierarchy and extension context for family get

            #region Family Get including extensions if any

            EntityBL entityBL = new EntityBL();
            EntityGetOptions entityGetOptions = PrepareEntityGetOptions();

            mainEntity = entityBL.GetEntityHierarchy(entityUniqueIdentifier, mainEntityContext, entityGetOptions, callerContext);

            #endregion Family Get including extensions if any

            return mainEntity;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityFamilyChangeContext"></param>
        /// <param name="loadLookupDisplayValues"></param>
        /// <param name="loadComplexChildAttributes"></param>
        /// <param name="callerContext"></param>
        /// <returns></returns>
        private EntityCollection GetEntities(EntityFamilyChangeContext entityFamilyChangeContext, Boolean loadLookupDisplayValues, Boolean loadComplexChildAttributes, CallerContext callerContext)
        {
            EntityCollection entities = new EntityCollection();

            if (entityFamilyChangeContext != null)
            {
                Collection<ObjectAction> objectAction = new Collection<ObjectAction>() { ObjectAction.Create, ObjectAction.Reclassify, ObjectAction.ReParent };

                Collection<Int64> entityIdList = entityFamilyChangeContext.GetEntityIdList(objectAction);

                Int64 entityId = entityFamilyChangeContext.EntityFamilyId;

                if (entityIdList != null && entityIdList.Count > 0)
                {
                    EntityGetOptions entityGetOptions = PrepareEntityGetOptions();

                    //In case of newly created, we need to refresh full DN search. Hence loading all attributes.
                    EntityContext entityRefreshContext = new EntityContext()
                    {
                        LoadAttributes = true,
                        LoadAttributeModels = true,
                        AttributeIdList = new Collection<Int32>(),
                        DataLocales = this.AvailableLocales,
                        LoadComplexChildAttributes = loadComplexChildAttributes,
                        LoadBlankAttributes = false,
                        LoadLookupDisplayValues = loadLookupDisplayValues
                    };

                    if (entityIdList != null && entityIdList.Count > 0)
                    {
                        EntityCollection freshEntities = _entityManager.Get(entityIdList, entityRefreshContext, entityGetOptions, callerContext);

                        if (freshEntities != null && freshEntities.Count > 0)
                        {
                            entities.AddRange(freshEntities);
                        }
                    }
                }

                EntityContext entityContext = entityFamilyChangeContext.GetEntityContext();
                entityContext.RelationshipContext = null;
                entityContext.LoadRelationships = false;
                entityContext.LoadOnlyCurrentValues = true; // Load only current values to populate DN search.

                EntityUniqueIdentifier entityUniqueIdentifier = new EntityUniqueIdentifier(entityId);

                // If root entity is not deleted, then and then get fresh entities for DN_Search.
                // if it is deleted, do not do get call here and mark entity search data action as delete based on entityfamilychangecontext
                Boolean hasRootEntityDeleted = IsRootEntityDeleted(entityFamilyChangeContext, entityId);

                if (!hasRootEntityDeleted)
                {
                    Entity entity = Get(entityId, null, entityContext, true, loadLookupDisplayValues, loadComplexChildAttributes, callerContext);

                    if (entity != null)
                    {
                        EntityCollection flattenEntities = (EntityCollection)entity.GetFlattenEntities();

                        if (flattenEntities != null && flattenEntities.Count > 0)
                        {
                            //If any entity create/reclassify/re-parent was done then perform duplicate check before adding into master collection.
                            Boolean checkDuplicates = (entities.Count > 0) ? true : false;

                            entities.AddRange(flattenEntities, checkDuplicates);
                        }
                    } 
                }
            }

            return entities;
        }

        #endregion Get Methods

        #region Process Methods

        /// <summary>
        /// Process entity group queue
        /// </summary>
        /// <param name="entityId">Indicates entity id of root entity type to be processed</param>
        /// <param name="entityFamilyChangeContext">Indicates the entity family change context</param>
        /// <param name="callerContext">Indicates the caller context</param>
        /// <returns>Returns operation result</returns>
        private EntityOperationResultCollection ProcessMain(Int64 entityId, EntityFamilyChangeContext entityFamilyChangeContext, CallerContext callerContext)
        {
            EntityOperationResultCollection entityOperationResults = null;
            EntityCollection impactedEntitiesWithInheritedValues = null;

            #region Step : Diagnostic Activity initialization

            DiagnosticActivity diagnosticActivity = new DiagnosticActivity();
            Boolean isTracingEnabled = _currentTraceSettings.IsBasicTracingEnabled;
            ExecutionContext executionContext = null;

            if (isTracingEnabled)
            {
                CallDataContext callDataContext = new CallDataContext();
                callDataContext.EntityIdList.Add(entityId);
                executionContext = new ExecutionContext(callerContext, callDataContext, _securityPrincipal.GetSecurityContext(), entityFamilyChangeContext.ToXml());
                executionContext.LegacyMDMTraceSources.Add(MDMTraceSource.EntityFamilyProcess);
                diagnosticActivity.Start(executionContext);
            }

            #endregion

            try
            {
                #region Step : Initial Setup

                Entity mainEntity = null;
                EntityCollection allEntities = null;

                Boolean isRelationshipInheritanceEnabled = AppConfigurationHelper.GetAppConfig("RelationshipManager.Relationship.Inheritance.Enabled", true);
                String markedForDeletionBehavior = AppConfigurationHelper.GetAppConfig("MDMCenter.MatchMerge.MarkedForDeletionBehavior", "Keep");

                Boolean deleteByLifecycleStatus = (String.Compare(markedForDeletionBehavior, "Keep", true) != 0);

                Boolean isTranslationEnabled = EntityOperationsCommonUtility.IsTranslationEnabled();
                Int32 maxDegreeOfParallelism = 3; //TODO : Replace with proper config 
                Boolean hasVariantsToProcess = false;

                _isMasterRecord = entityFamilyChangeContext.IsMasterCollaborationRecord;
                Boolean isMetadataChange = entityFamilyChangeContext.EntityActivityList == EntityActivityList.MetadataChange;
                Boolean hasRelationshipsChanged = entityFamilyChangeContext.HasRelationshipsChanged();
                Boolean hasEntitiesCreated = entityFamilyChangeContext.HasEntitiesCreated();
                Boolean loadAllExtensions = (_isMasterRecord) ? true : false;

                EntityChangeContextCollection variantsFamilyChangeContexts = entityFamilyChangeContext.VariantsChangeContext.EntityChangeContexts;

                if (variantsFamilyChangeContexts.Count > 0)
                {
                    hasVariantsToProcess = true;
                }

                var entityProcessingOptions = new EntityProcessingOptions();
                entityProcessingOptions.ProcessingMode = ProcessingMode.Async;
                entityProcessingOptions.ValidateLeafCategory = false;

                ContainerContext containerContext = new ContainerContext { ApplySecurity = false, LoadAttributes = false };
                ContainerCollection allContainers = new ContainerBL().GetAll(containerContext, callerContext);

                #endregion Step : Initial Setup

                #region Step : Load Entity Context for Pre load

                MDMRuleParams ruleParams = new MDMRuleParams()
                {
                    EntityOperationResults = entityOperationResults,
                    UserSecurityPrincipal = _securityPrincipal,
                    CallerContext = callerContext,
                    EntityFamilyChangeContext = entityFamilyChangeContext,
                    DDGCallerModule = DDGCallerModule.EntityFamilyProcess,
                    Events = new Collection<MDMEvent>()
                    {   MDMEvent.EntityCreatePostProcessStarting, MDMEvent.EntityUpdatePostProcessStarting,
                        MDMEvent.EntityDeletePostProcessStarting, MDMEvent.EntityVariantsChanging, MDMEvent.EntityExtensionsChanging
                    }
                };

                //EntityContext preLoadEntityContext = PreLoadContextHelper.GetEntityContext(ruleParams, _entityManager);
                //EntityContextHelper.PopulateIdsInEntityContext(preLoadEntityContext, _entityManager, callerContext);

                #endregion Step : Load Entity Context for Pre load

                #region Step : Entity Family Get

                //mainEntity = Get(entityId, entityFamilyChangeContext, preLoadEntityContext, loadAllExtensions, loadLookupDisplayValues: false, loadComplexChildAttributes: false, callerContext: callerContext);
                
                if (mainEntity != null)
                {
                    allEntities = (EntityCollection)mainEntity.GetFlattenEntities();
                }

                #endregion Step : Entity Family Get

                #region Step : Handle entities marked for deletion

                if (_isMasterRecord)
                {
                    EntityCollection entitiesToBeDeleted = new EntityCollection();

                    //Find the entities from variant change context
                    IEntityChangeContextCollection variantChangeContexts = entityFamilyChangeContext.VariantsChangeContext.GetEntityChangeContexts();

                    if (variantChangeContexts != null && variantChangeContexts.Count > 0)
                    {
                        PopulateEntitiesToBeDeleted(allEntities, entitiesToBeDeleted, variantChangeContexts);
                    }

                    //Find the entities from extension change context
                    foreach (var extensionChangeContext in entityFamilyChangeContext.ExtensionChangeContexts)
                    {
                        IEntityChangeContextCollection extensionEntityChangeContexts = extensionChangeContext.VariantsChangeContext.GetEntityChangeContexts();

                        if (extensionEntityChangeContexts != null && extensionEntityChangeContexts.Count > 0)
                        {
                            PopulateEntitiesToBeDeleted(allEntities, entitiesToBeDeleted, extensionEntityChangeContexts);
                        }
                    }

                    if (entitiesToBeDeleted.Count > 0)
                    {
                        HandleDeletedEntities(entitiesToBeDeleted, deleteByLifecycleStatus);
                    }
                }

                #endregion Step : Handle entities marked for deletion

                #region Step : Perform Auto Extension

                if (!isMetadataChange && hasEntitiesCreated && _isMasterRecord)
                {
                    PerformAutoExtension(mainEntity, allContainers);

                    if (isTracingEnabled)
                    {
                        diagnosticActivity.LogDurationInfo("Performed auto extension.");
                    }

                    //Refresh all entities bucket...
                    allEntities.Clear();
                    allEntities = (EntityCollection)mainEntity.GetFlattenEntities();
                }

                #endregion Step : Perform Auto Extension
                
                #region Step : Prepare Entity Operation Result Schema

                ruleParams.EntityOperationResults = entityOperationResults = new EntityOperationResultCollection(allEntities);

                if (isTracingEnabled)
                {
                    diagnosticActivity.LogDurationInfo("Prepared entity operation results");
                }

                #endregion Step : Prepare Entity Operation Result Schema

                using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required, GetTransactionOptions(ProcessingMode.Async)))
                {
                    if (!isMetadataChange)
                    {
                        #region Step : Invoke Async BR's

                        if (hasVariantsToProcess)
                        {
                            EntityCollection entitiesForCreate = new EntityCollection();
                            EntityCollection entitiesForUpdate = new EntityCollection();
                            EntityCollection entitiesForDelete = new EntityCollection();

                            EntityCollection allVariantsEntitiesIncludingSelf = (EntityCollection)mainEntity.GetFlattenEntities(includeExtensionsWithVariants: false);

                            foreach (EntityChangeContext entityChangeContext in variantsFamilyChangeContexts)
                            {
                                Entity filteredEntity = null;

                                if (allVariantsEntitiesIncludingSelf != null && allVariantsEntitiesIncludingSelf.Count > 0)
                                {
                                    filteredEntity = (Entity)allVariantsEntitiesIncludingSelf.GetEntity(entityChangeContext.EntityId);
                                }

                                if (filteredEntity != null)
                                {
                                    ObjectAction action = entityChangeContext.Action;

                                    switch (action)
                                    {
                                        case ObjectAction.Create:
                                            entitiesForCreate.Add(filteredEntity);
                                            break;
                                        case ObjectAction.Update:
                                        case ObjectAction.Reclassify:
                                        case ObjectAction.Rename:
                                            entitiesForUpdate.Add(filteredEntity);
                                            break;
                                        case ObjectAction.Delete:
                                            entitiesForDelete.Add(filteredEntity);
                                            break;
                                    }
                                }
                            }

                            //Skip the Transform Block if there is only one entity to transform

                            if (entitiesForCreate.Count > 1)
                            {
                                Collection<Dictionary<String, Object>> entityRelatedContexts = PrepareEntityRelatedContexts(entitiesForCreate, allEntities, MDMEvent.EntityCreatePostProcessStarting, ruleParams);
                                new ParallelTaskProcessor().RunInParallel<Dictionary<String, Object>, Boolean>(entityRelatedContexts, FireAsyncEvents, null, maxDegreeOfParallelism);
                            }
                            else if (entitiesForCreate.Count == 1)
                            {
                                if (!EntityOperationsCommonUtility.FireEntityEvent(entitiesForCreate, entitiesForCreate, allEntities, MDMEvent.EntityCreatePostProcessStarting, _entityManager, ruleParams))
                                {
                                    return entityOperationResults;
                                }
                            }

                            if (entitiesForUpdate.Count > 1)
                            {
                                Collection<Dictionary<String, Object>> entityRelatedContexts = PrepareEntityRelatedContexts(entitiesForUpdate, allEntities, MDMEvent.EntityUpdatePostProcessStarting, ruleParams);
                                new ParallelTaskProcessor().RunInParallel<Dictionary<String, Object>, Boolean>(entityRelatedContexts, FireAsyncEvents, null, maxDegreeOfParallelism);
                            }
                            else if (entitiesForUpdate.Count == 1)
                            {
                                if (!EntityOperationsCommonUtility.FireEntityEvent(entitiesForUpdate, entitiesForUpdate, allEntities, MDMEvent.EntityUpdatePostProcessStarting, _entityManager, ruleParams))
                                {
                                    return entityOperationResults;
                                }
                            }

                            if (entitiesForDelete.Count > 1)
                            {
                                Collection<Dictionary<String, Object>> entityRelatedContexts = PrepareEntityRelatedContexts(entitiesForDelete, allEntities, MDMEvent.EntityDeletePostProcessStarting, ruleParams);
                                new ParallelTaskProcessor().RunInParallel<Dictionary<String, Object>, Boolean>(entityRelatedContexts, FireAsyncEvents, null, maxDegreeOfParallelism);
                            }
                            else if (entitiesForDelete.Count == 1)
                            {
                                if (!EntityOperationsCommonUtility.FireEntityEvent(entitiesForDelete, entitiesForDelete, allEntities, MDMEvent.EntityDeletePostProcessStarting, _entityManager, ruleParams))
                                {
                                    return entityOperationResults;
                                }
                            }
                        }

                        #endregion Step : Invoke Async BR's

                        #region Step : Invoke family events

                        EntityCollection familyEntities = new EntityCollection();
                        familyEntities.Add(mainEntity);

                        //Fire the EntityVariantsChanging event
                        if (hasVariantsToProcess && !EntityOperationsCommonUtility.FireEntityEvent(familyEntities, familyEntities, allEntities, MDMEvent.EntityVariantsChanging, _entityManager, ruleParams))
                        {
                            return entityOperationResults;
                        }

                        //Fire the EntityExtensionsChanging event
                        if (_isMasterRecord && !EntityOperationsCommonUtility.FireEntityEvent(familyEntities, familyEntities, allEntities, MDMEvent.EntityExtensionsChanging, _entityManager, ruleParams))
                        {
                            return entityOperationResults;
                        }

                        #endregion Step : Invoke family events

                        #region Step : Validate meta-data

                        EntityCollection allFamilies = new EntityCollection();
                        allFamilies.Add(mainEntity);
                        EntityCollection allExtendedEntities = (EntityCollection)mainEntity.GetAllExtendedEntities();

                        if (allExtendedEntities != null && allExtendedEntities.Count > 0)
                        {
                            allFamilies.AddRange(allExtendedEntities);
                        }

                        ValidateEntitiesMetadata(allFamilies, allContainers, entityOperationResults, callerContext);

                        #endregion Step : Validate meta-data

                        #region Step : Save family to database

                        var entityHierarchyProcessManager = new EntityHierarchyProcessManager();
                        entityHierarchyProcessManager.Process(allEntities, entityOperationResults, entityProcessingOptions, entityFamilyChangeContext, callerContext, allContainers);

                        #endregion Step : Save family to database

                        #region Step : Save Entity states to database

                        EntityStateValidationBL entityStateValidationBL = new EntityStateValidationBL();
                        entityStateValidationBL.ProcessEntityFamilyStates(allFamilies, entityOperationResults, callerContext);

                        #endregion Step : Save Entity states to database
                    }

                    #region Step : Refresh Entity Family Change Contexts

                    var originalEntityFamilyChangeContext = new EntityFamilyChangeContext(entityFamilyChangeContext.ToXml());

                    EntityChangeContextHelper.RefreshEntityFamilyChangeContext(allEntities, entityFamilyChangeContext, callerContext, _currentTraceSettings);

                    #endregion Step : Refresh Entity Family Change Contexts

                    #region Step : Populate DN Search all entities

                    if (_isMasterRecord)
                    {
                        Boolean loadValuesForJigsaw = JigsawConstants.IsJigsawIntegrationEnabled;

                        entityFamilyChangeContext.EntityFamilyId = entityId;
                        EntityCollection freshEntities = GetEntities(entityFamilyChangeContext, loadLookupDisplayValues: loadValuesForJigsaw, loadComplexChildAttributes: loadValuesForJigsaw, callerContext: callerContext);

                        EntitySearchDataBL entitySearchDataManager = new EntitySearchDataBL(_entityManager);
                        EntityOperationResultCollection dnSearchEntityOperationResults = entitySearchDataManager.RefreshEntitiesSearchData(freshEntities, entityFamilyChangeContext, callerContext);

                        // Do not copy entity metadata here as current entity operation results already have it. So passing false here.
                        entityOperationResults.CopyEntityOperationResults(dnSearchEntityOperationResults, false);

                        if (JigsawConstants.IsJigsawIntegrationEnabled)
                        {
                            impactedEntitiesWithInheritedValues = GetAdjustedAttributeActionEntityCollection(freshEntities, entityFamilyChangeContext, originalEntityFamilyChangeContext, callerContext);
                        }
                    }

                    #endregion Step : Populate DN Search all entities

                    if (!isMetadataChange)
                    {
                        #region Step : Queue for extension processing

                        if (!_isMasterRecord)
                        {
                            Container container = allContainers.GetContainer(mainEntity.ContainerId);

                            EntityFamilyChangeContext entityFamilyChangeContextForExtension = GetExtensionChangeContext(mainEntity, container, entityFamilyChangeContext, allContainers);

                            EntityFamilyQueue entityFamilyQueueForExtension = new EntityFamilyQueue();
                            entityFamilyQueueForExtension.EntityFamilyId = mainEntity.EntityGlobalFamilyId;
                            entityFamilyQueueForExtension.EntityGlobalFamilyId = mainEntity.EntityGlobalFamilyId;
                            entityFamilyQueueForExtension.ContainerId = entityFamilyChangeContextForExtension.ContainerId;
                            entityFamilyQueueForExtension.EntityFamilyChangeContexts.Add(entityFamilyChangeContextForExtension);
                            entityFamilyQueueForExtension.EntityActivityList = EntityActivityList.ExtensionChange;
                            entityFamilyQueueForExtension.Action = ObjectAction.Create;

                            EntityFamilyQueueBL entityFamilyQueueBL = new EntityFamilyQueueBL();
                            OperationResult operationResult = entityFamilyQueueBL.Process(entityFamilyQueueForExtension, callerContext);

                            //What to do if operation result comes back with failed status??
                        }

                        #endregion Step : Queue for extension processing

                        #region Step : Queue for Auto Promote

                        if (_isMasterRecord)
                        {
                            EnqueueAutoPromoteQueueItem(mainEntity, entityFamilyChangeContext, callerContext);
                        }

                        #endregion Step : Queue for Auto Promote

                        #region Step : Queue for where used relationship processing

                        if (_isMasterRecord && hasRelationshipsChanged)
                        {
                            EnqueueWhereUsedRelationships(entityId, entityFamilyChangeContext, callerContext);
                        }

                        #endregion Step : Queue for where used relationship processing

                        #region Step : Create Entity Activity Logs

                        EntityActivityLogBL entityActivityLogManager = new EntityActivityLogBL();
                        entityActivityLogManager.Process(allEntities, entityProcessingOptions, callerContext, isRelationshipInheritanceEnabled);

                        #endregion Step : Create Entity Activity Logs

                        #region Step : Create Integration Activity Log for Translation

                        if (isTranslationEnabled)
                        {
                            if (_currentTraceSettings.IsBasicTracingEnabled)
                            {
                                diagnosticActivity.LogInformation("Starting integration activity log creation process...");
                            }

                            EntityOperationsCommonUtility.EnqueueIntegrationActivity(allEntities, callerContext);

                            if (_currentTraceSettings.IsBasicTracingEnabled)
                            {
                                diagnosticActivity.LogDurationInfo("Integration activity log creation process completed");
                            }
                        }

                        #endregion
                    }

                    #region Step : Commit transaction

                    if (isTracingEnabled)
                    {
                        diagnosticActivity.LogInformation("Starting entity family process transaction commit...");
                    }

                    //Commit transaction
                    transactionScope.Complete();

                    if (isTracingEnabled)
                    {
                        diagnosticActivity.LogDurationInfo("Entity family process transaction commit completed");
                    }

                    #endregion
                }

                #region Step : Send entities processed to Jigsaw

                if (JigsawConstants.IsJigsawIntegrationEnabled && allEntities.Count > 0)
                {
                    EntityJigsawIntegrationHelper.SendToJigsaw(allEntities, null, _securityPrincipal, callerContext);

                    if (_currentTraceSettings.IsBasicTracingEnabled)
                    {
                        diagnosticActivity.LogDurationInfo("Sending Entities to Jigsaw step completed.");
                    }
                }

                if (JigsawConstants.IsJigsawIntegrationEnabled && impactedEntitiesWithInheritedValues != null && impactedEntitiesWithInheritedValues.Count > 0)
                {
                    EntityJigsawIntegrationHelper.SendToJigsaw(impactedEntitiesWithInheritedValues, null, _securityPrincipal, callerContext);

                    if (_currentTraceSettings.IsBasicTracingEnabled)
                    {
                        diagnosticActivity.LogDurationInfo("Sending Entities with Inherited Attribute Values to Jigsaw step completed.");
                    }
                }

                if (JigsawConstants.IsJigsawIntegrationEnabled && hasVariantsToProcess)
                {
                    EntityCollection allReclassifiedEntities = new EntityCollection(GetAllReclassifiedChildEntities(entityFamilyChangeContext, allEntities, callerContext));

                    if ((allReclassifiedEntities != null) && allReclassifiedEntities.Any())
                    {
                        EntityJigsawIntegrationHelper.SendToJigsaw(allReclassifiedEntities, null, _securityPrincipal, callerContext);
                    }
                }
                
                #endregion

                #region Step : Update entity maps

                EntityMapBL entityMapManager = new EntityMapBL();
                entityMapManager.UpdateEntityMaps(allEntities);

                #endregion

            }
            finally
            {
                if (isTracingEnabled)
                {
                    diagnosticActivity.Stop();
                }
            }

            return entityOperationResults;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityFamilyChangeContext"></param>
        /// <param name="allEntities"></param>
        /// <param name="callerContext"></param>
        /// <returns></returns>
        private List<Entity> GetAllReclassifiedChildEntities(EntityFamilyChangeContext entityFamilyChangeContext, EntityCollection allEntities, CallerContext callerContext)
        {
            Collection<Int64> entityIdList = entityFamilyChangeContext.GetEntityIdList(ObjectAction.Reclassify);

            List<Entity> reclassifiedChildEntities = new List<Entity>();

            if (entityIdList != null && entityIdList.Count > 0)
            {
                foreach (Int64 entityId in entityIdList)
                {
                   reclassifiedChildEntities.AddRange(GetAllChildEntities(entityId,allEntities));
                }

                foreach (Entity entity in reclassifiedChildEntities)
                {
                    entity.Action = ObjectAction.ImpactedReclassify;
                }
            }
            return reclassifiedChildEntities;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityId"></param>
        /// <param name="allEntities"></param>
        /// <returns></returns>
        private List<Entity> GetAllChildEntities(Int64 entityId,EntityCollection allEntities)
        {
            List<Entity> childEntities = allEntities.GetEntitiesByParentEntityId(entityId).ToList();

            List<Entity> children = new List<Entity>();

            foreach (Entity entity in childEntities)
            {
                children.AddRange(GetAllChildEntities(entity.Id, allEntities));
            }

            childEntities.AddRange(children);

            return childEntities;

        }

        /// <summary>
        /// Gets the entities to be deleted.
        /// </summary>
        /// <param name="allEntities">All entities.</param>
        /// <param name="entitiestoBeDeleted">The entities deleted.</param>
        /// <param name="entityChangeContexts">The entity change contexts.</param>
        private void PopulateEntitiesToBeDeleted(EntityCollection allEntities, EntityCollection entitiestoBeDeleted, IEntityChangeContextCollection entityChangeContexts)
        {
            foreach (EntityChangeContext entityChangeContext in entityChangeContexts)
            {
                if (entityChangeContext.AttributeIdList.Contains(_lifecycleStatusAttributeId))
                {
                    var entity = allEntities.GetEntity(entityChangeContext.EntityId);

                    if (entity != null)
                    {
                        IAttribute lifecycleStatusAttribute = entity.GetAttribute(_lifecycleStatusAttributeId);

                        if (lifecycleStatusAttribute != null)
                        {
                            String statusValue = lifecycleStatusAttribute.GetCurrentValue<String>();

                            if (String.Compare(statusValue, LifecycleStatusValues.MarkedForDeletion, true) == 0)
                            {
                                entitiestoBeDeleted.Add(entity);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Handles the deleted entities.
        /// </summary>
        /// <param name="entitiesToBeDeleted">The entities to be deleted.</param>
        /// <param name="deleteByLifecycleStatus">if set to <c>true</c> [delete by lifecycle status].</param>
        private void HandleDeletedEntities(EntityCollection entitiesToBeDeleted, Boolean deleteByLifecycleStatus)
        {
            EntityCollection extensionEntitiesTobeDeleted = new EntityCollection();

            foreach (var deleteEntity in entitiesToBeDeleted)
            {
                //Delete the current entity
                if (deleteByLifecycleStatus)
                {
                    deleteEntity.Action = ObjectAction.Delete;
                }

                extensionEntitiesTobeDeleted.Add(deleteEntity);

                //Delete all the children
                IEntityCollection childEntities = deleteEntity.GetAllChildEntities();

                if (childEntities!= null && childEntities.Count > 0)
                {
                    foreach (var childEntity in childEntities)
                    {
                        if (deleteByLifecycleStatus)
                        {
                            childEntity.Action = ObjectAction.Delete;
                        }
                        else
                        {
                            IAttribute lifecycleStatusAttribute = childEntity.GetAttribute(_lifecycleStatusAttributeId);

                            if (lifecycleStatusAttribute != null)
                            {
                                //Pre load will load this attribute for all child and extension entities if it is modified anywhere
                                lifecycleStatusAttribute.SetValue(LifecycleStatusValues.MarkedForDeletion);
                            }
                        }
                    }
                }
            }

            //Handle the deletion of extension entities
            if (extensionEntitiesTobeDeleted.Count > 0)
            {
                HandleDeletedExtensionEntities(extensionEntitiesTobeDeleted, deleteByLifecycleStatus);
            }

        }

        /// <summary>
        /// Handles the deleted extension entities.
        /// </summary>
        /// <param name="entitiesToBeDeleted">The entities to be deleted.</param>
        /// <param name="deleteByLifecycleStatus">if set to <c>true</c> [delete by lifecycle status].</param>
        private void HandleDeletedExtensionEntities(EntityCollection entitiesToBeDeleted, Boolean deleteByLifecycleStatus)
        {
            EntityCollection extensionEntitiesTobeDeleted = new EntityCollection();

            foreach (var deleteEntity in entitiesToBeDeleted)
            {
                foreach (var extensionRelationship in deleteEntity.GetExtensionsRelationships())
                {
                    if (extensionRelationship!= null && extensionRelationship.Direction == RelationshipDirection.Down)
                    {
                        Entity relatedEntity = extensionRelationship.RelatedEntity;
                        extensionEntitiesTobeDeleted.Add(relatedEntity);

                        if (deleteByLifecycleStatus)
                        {
                            relatedEntity.Action = ObjectAction.Delete;
                        }
                        else
                        {
                            IAttribute lifecycleStatusAttribute = relatedEntity.GetAttribute(_lifecycleStatusAttributeId);

                            if (lifecycleStatusAttribute != null)
                            {
                                //Pre load will load this attribute for all child and extension entities if it is modified anywhere
                                lifecycleStatusAttribute.SetValue(LifecycleStatusValues.MarkedForDeletion);
                            }
                        }
                    }
                }
            }

            //Handle the deletion of extended entities and its children
            if (extensionEntitiesTobeDeleted.Count > 0)
            {
                HandleDeletedEntities(extensionEntitiesTobeDeleted, deleteByLifecycleStatus);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rootEntities"></param>
        /// <param name="allContainers"></param>
        /// <param name="entityOperationResults"></param>
        /// <param name="callerContext"></param>
        private void ValidateEntitiesMetadata(EntityCollection rootEntities, ContainerCollection allContainers, EntityOperationResultCollection entityOperationResults, CallerContext callerContext)
        {
            if (rootEntities != null && rootEntities.Count > 0)
            {
                #region Initial Setup

                Container container = null;
                Hierarchy hierarchy = null;
                EntityOperationResult entityOperationResult = null;
                Dictionary<Int64, Category> categories = new Dictionary<Int64, Category>();
                HierarchyCollection allhierarchies = null;
                HierarchyBL hierarchyBL = new HierarchyBL();
                EntityValidationBL entityValidationManager = new EntityValidationBL();

                #endregion Initial Setup

                //Get all hierarchies
                allhierarchies = hierarchyBL.GetAll(callerContext, false);

                foreach (Entity rootEntity in rootEntities)
                {
                    container = allContainers.GetContainer(rootEntity.ContainerId);
                    hierarchy = allhierarchies.GetHierarchy(container.HierarchyId);

                    if (container != null && hierarchy != null)
                    {
                        #region Validate root entity meta-data from each family

                        entityOperationResult = (EntityOperationResult)entityOperationResults.GetByEntityId(rootEntity.Id);
                        entityValidationManager.ValidateEntityMetaData(rootEntity, hierarchy, categories, entityOperationResult, callerContext);

                        #endregion Validate root entity metadata from each family

                        #region Validate variants of root entity

                        EntityCollection variantEntities = (EntityCollection)rootEntity.GetAllChildEntities();
                        if (variantEntities != null && variantEntities.Count > 0)
                        {
                            foreach (Entity variantEntity in variantEntities)
                            {
                                if (variantEntity.CategoryId != rootEntity.CategoryId)
                                {
                                    entityOperationResult = (EntityOperationResult)entityOperationResults.GetByEntityId(variantEntity.Id);

                                    _localeMessage = _localeMessageBL.Get(GlobalizationHelper.GetSystemUILocale(), "114424", false, callerContext); // Unable to process the entity '{0}', as its category: '{1}' is different from the root parent category: '{2}'.
                                    Collection<Object> parameters = new Collection<Object>() { variantEntity.LongName, variantEntity.CategoryLongName, rootEntity.CategoryLongName };
                                    entityOperationResult.AddOperationResult(_localeMessage.Code, _localeMessage.Message, parameters, ReasonType.IncorrectCategory, -1, -1, OperationResultType.Error);
                                }
                                else
                                {
                                    // using same categories dictionary to reduce extra category get calls
                                    entityValidationManager.ValidateEntityMetaData(variantEntity, hierarchy, categories, entityOperationResult, callerContext);
                                }
                            }
                        }

                        #endregion Validate variants of root entity
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityId"></param>
        /// <param name="entityFamilyChangeContext"></param>
        /// <param name="callerContext"></param>
        /// <returns></returns>
        private EntityOperationResultCollection PushCategoryValues(Int64 entityId, EntityFamilyChangeContext entityFamilyChangeContext, CallerContext callerContext)
        {
            EntityOperationResultCollection entityOperationResults = null;

            #region Step : Diagnostic Activity initialization

            DiagnosticActivity diagnosticActivity = new DiagnosticActivity();
            Boolean isTracingEnabled = _currentTraceSettings.IsBasicTracingEnabled;
            ExecutionContext executionContext = null;

            if (isTracingEnabled)
            {
                CallDataContext callDataContext = new CallDataContext();
                callDataContext.EntityIdList.Add(entityId);
                executionContext = new ExecutionContext(callerContext, callDataContext, _securityPrincipal.GetSecurityContext(), entityFamilyChangeContext.ToXml());
                executionContext.LegacyMDMTraceSources.Add(MDMTraceSource.EntityFamilyProcess);
                diagnosticActivity.Start(executionContext);
            }

            #endregion

            try
            {
                #region Step : Entity Family Get
                Boolean loadValuesForJigsaw = JigsawConstants.IsJigsawIntegrationEnabled;

                EntityContext entityContext = entityFamilyChangeContext.GetEntityContext();
                Entity mainEntity = Get(entityId, null, entityContext, true, loadLookupDisplayValues: loadValuesForJigsaw, loadComplexChildAttributes: loadValuesForJigsaw, callerContext: callerContext);
                EntityCollection freshEntities = null;

                #endregion Step : Entity Family Get

                using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required, GetTransactionOptions(ProcessingMode.Async)))
                {
                    #region Step : Populate DN Search all entities

                    if (mainEntity != null)
                    {
                        freshEntities = (EntityCollection)mainEntity.GetFlattenEntities();

                        entityOperationResults = new EntityOperationResultCollection(freshEntities);

                        EntitySearchDataBL entitySearchDataManager = new EntitySearchDataBL(_entityManager);
                        EntityOperationResultCollection dnSearchEntityOperationResults = entitySearchDataManager.RefreshEntitiesSearchData(freshEntities, null, callerContext);

                        // Do not copy entity metadata here as current entity operation results already have it. So passing false here.
                        entityOperationResults.CopyEntityOperationResults(dnSearchEntityOperationResults, false);

                        if (JigsawConstants.IsJigsawIntegrationEnabled)
                        {
                            var entitiestoTransfer = GetAdjustedAttributeActionEntityCollection(freshEntities, entityFamilyChangeContext, null, callerContext);

                            if (entitiestoTransfer != null && entitiestoTransfer.Count > 0)
                            {
                                EntityJigsawIntegrationHelper.SendToJigsaw(entitiestoTransfer, null, _securityPrincipal, callerContext);
                            }
                        }
                    }

                    #endregion Step : Populate DN Search all entities

                    #region Step : Queue for Category Promote

                    EnqueueCategoryPromoteQueueItem(mainEntity, freshEntities, entityFamilyChangeContext, callerContext);

                    #endregion Step : Queue for Category Promote

                    #region Step : Commit transaction

                    if (isTracingEnabled)
                    {
                        diagnosticActivity.LogInformation("Starting push category values transaction commit...");
                    }

                    //Commit transaction
                    transactionScope.Complete();

                    if (isTracingEnabled)
                    {
                        diagnosticActivity.LogDurationInfo("Push category values transaction commit completed");
                    }

                    #endregion
                }
            }
            finally
            {
                if (isTracingEnabled)
                {
                    diagnosticActivity.Stop();
                }
            }

            return entityOperationResults;
        }

        #endregion Process Methods

        #region Helper Methods

        /// <summary>
        /// Gets entity get options
        /// </summary>
        /// <returns>Returns entity get options</returns>
        private EntityGetOptions PrepareEntityGetOptions()
        {
            EntityGetOptions groupEntityGetOptions = new EntityGetOptions();

            groupEntityGetOptions.ApplyAVS = false;
            groupEntityGetOptions.ApplySecurity = false;
            groupEntityGetOptions.PublishEvents = false;
            groupEntityGetOptions.UpdateCache = false;
            groupEntityGetOptions.UpdateCacheStatusInDB = false;
            groupEntityGetOptions.LoadLatestFromDB = true;

            return groupEntityGetOptions;
        }

        /// <summary>
        /// Gets extension change context for current entity 
        /// </summary>
        /// <param name="entity">Indicates entity object</param>
        /// <param name="container">Indicates container object</param>
        /// <param name="entityFamilyChangeContext">Indicate entity family change context</param>
        /// <param name="allContainers">Indicates all containers</param>
        /// <returns>Returns entity family change context for extension</returns>
        private EntityFamilyChangeContext GetExtensionChangeContext(Entity entity, Container container, EntityFamilyChangeContext entityFamilyChangeContext, ContainerCollection allContainers)
        {
            EntityFamilyChangeContext entityFamilyChangeContextForExtension = new EntityFamilyChangeContext();
            entityFamilyChangeContextForExtension.EntityFamilyId = entityFamilyChangeContext.EntityGlobalFamilyId;
            entityFamilyChangeContextForExtension.EntityGlobalFamilyId = entityFamilyChangeContext.EntityGlobalFamilyId;

            // Here, passing parent containerId as a parameter because current 'container' variable is already a non-master container. 
            // So starting from its parent directly and reaching to the root container.
            entityFamilyChangeContextForExtension.ContainerId = GetMasterCollaborationContainerId(container.ParentContainerId, allContainers);
            
            ExtensionChangeContext extensionChangeContext = new ExtensionChangeContext();
            extensionChangeContext.OrganizationId = entity.OrganizationId;
            extensionChangeContext.OrganizationName = entity.OrganizationName;
            extensionChangeContext.ContainerId = entity.ContainerId;
            extensionChangeContext.ContainerName = entity.ContainerName;
            extensionChangeContext.ContainerType = container.ContainerType;
            extensionChangeContext.ContainerQualifierName = container.ContainerQualifierName;
            extensionChangeContext.VariantsChangeContext = entityFamilyChangeContext.VariantsChangeContext;

            entityFamilyChangeContextForExtension.ExtensionChangeContexts.Add(extensionChangeContext);

            return entityFamilyChangeContextForExtension;
        }
       
        /// <summary>
        /// Gets root container id of given container
        /// </summary>
        /// <param name="containerId">Indicates container id to get container from container collection</param>
        /// <param name="allContainers">Indicates all containers</param>
        /// <returns>Returns root container Id</returns>
        private Int32 GetMasterCollaborationContainerId(Int32 containerId, ContainerCollection allContainers)
        {
            Int32 masterContainerId = -1;
            
            if (allContainers != null)
            {
                Container container = allContainers.GetContainer(containerId);

                if (container != null)
                {
                    if (container.ContainerType != ContainerType.MasterCollaboration)
                    {
                        masterContainerId = GetMasterCollaborationContainerId(container.ParentContainerId, allContainers);
                    }
                    else
                    {
                        masterContainerId = container.Id;
                    }
                } 
            }

            return masterContainerId;
        }

        /// <summary>
        /// Updates entity variants context based on given entity change contexts
        /// </summary>
        /// <param name="entityChangeContexts">Indicates entity change contexts</param>
        /// <param name="entityTypes">Indicates entity type collection</param>
        /// <param name="mainEntityContext">Indicates main entity type context</param>
        /// <param name="entityTypeIdToVariantLevelMappings">Indicates entity type id to variant level mapping</param>
        /// <param name="preLoadEntityContext">Indicates preLoad entity context</param>
        /// <param name="relationshipContext">Indicates relationship context</param>
        /// <param name="hasEntitiesCreated">Indicates whether any entities are created or not. If any entities are created then get all relationships also. This is required to pull inherited relationships.</param>
        private void UpdateEntityVariantsContext(EntityChangeContextCollection entityChangeContexts, EntityTypeCollection entityTypes, EntityContext mainEntityContext, Dictionary<Int32, Int32> entityTypeIdToVariantLevelMappings, EntityContext preLoadEntityContext, RelationshipContext relationshipContext, Boolean hasEntitiesCreated)
        {
            if (entityTypes != null && entityTypes.Count > 0)
            {
                foreach (EntityType entityType in entityTypes)
                {
                    Int32 entityTypeId = entityType.Id;
                    Int32 variantLevel = entityTypeIdToVariantLevelMappings[entityTypeId];
                    String entityTypeName = entityType.Name;
                    Collection<LocaleEnum> availableLocales = null;

                    EntityChangeContextCollection filteredEntityChangeContexts = null;

                    if (entityChangeContexts != null)
                    {
                        filteredEntityChangeContexts = entityChangeContexts.GetByEntityTypeId(entityTypeId);
                    }

                    if (variantLevel == Constants.VARIANT_LEVEL_ONE)
                    {
                        if (relationshipContext != null)
                        {
                            mainEntityContext.RelationshipContext = relationshipContext.Clone();
                            availableLocales = relationshipContext.DataLocales;
                        }

                        UpdateEntityContext(mainEntityContext, filteredEntityChangeContexts, entityTypeId, entityTypeName, preLoadEntityContext, hasEntitiesCreated);

                        if (availableLocales != null)
                        {
                            mainEntityContext.RelationshipContext.DataLocales = availableLocales;
                        }
                    }
                    else
                    {
                        EntityContext childEntityContext = new EntityContext();
                        childEntityContext.LoadAttributeModels = true;

                        if (relationshipContext != null)
                        {
                            childEntityContext.RelationshipContext = relationshipContext.Clone();
                            availableLocales = relationshipContext.DataLocales;
                        }

                        UpdateEntityContext(childEntityContext, filteredEntityChangeContexts, entityTypeId, entityTypeName, preLoadEntityContext, hasEntitiesCreated);

                        if (availableLocales != null)
                        {
                            childEntityContext.RelationshipContext.DataLocales = availableLocales;
                        }

                        mainEntityContext.EntityHierarchyContext.AddEntityDataContext(entityTypeName, childEntityContext);
                    }
                }
            }
        }

        /// <summary>
        /// Update entity context to load data based on entity change contexts
        /// </summary>
        /// <param name="entityContext">Indicates entity context which is required to update</param>
        /// <param name="filteredEntityChangeContexts">Indicates filtered entity change contexts</param>
        /// <param name="entityTypeId">Indicates entity type id to be updated in entity context</param>
        /// <param name="entityTypeName">Indicates entity name to be updated in entity context</param>
        /// <param name="preLoadEntityContext">Indicates preLoad entity context</param>
        /// <param name="hasEntitiesCreated">Indicates whether any entities are created or not. If any entities are created then get all relationships also. This is required to pull inherited relationships.</param>
        private void UpdateEntityContext(EntityContext entityContext, EntityChangeContextCollection filteredEntityChangeContexts, Int32 entityTypeId, String entityTypeName, EntityContext preLoadEntityContext, Boolean hasEntitiesCreated)
        {
            if (entityContext != null)
            {
                #region Update Entity Context from entity change contexts

                if (filteredEntityChangeContexts != null)
                {
                    entityContext.AttributeIdList = filteredEntityChangeContexts.GetAttributeIdList();
                    entityContext.DataLocales = filteredEntityChangeContexts.GetAttributeLocaleList();

                    Collection<Int32> relationshipTypeIdList = filteredEntityChangeContexts.GetRelationshipTypeIdList();

                    if (relationshipTypeIdList != null && relationshipTypeIdList.Count > 0)
                    {
                        entityContext.RelationshipContext.RelationshipTypeIdList.AddRange<Int32>(relationshipTypeIdList);
                        Collection<Int32> relationshipAttributeIdList = filteredEntityChangeContexts.GetRelationshipAttributeIdList();

                        if (relationshipAttributeIdList != null && relationshipAttributeIdList.Count > 0)
                        {
                            entityContext.RelationshipContext.LoadRelationshipAttributes = true;
                            entityContext.RelationshipContext.LoadRelationshipAttributeModels = true;
                            entityContext.RelationshipContext.DataLocales = filteredEntityChangeContexts.GetRelationshipAttributeLocaleList();
                        }
                    }
                }

                #endregion Update Entity Context from entity change contexts

                #region Update Current Entity Context from pre-load context

                if (preLoadEntityContext != null)
                {
                    entityContext.AttributeIdList.AddRange<Int32>(preLoadEntityContext.AttributeIdList);
                    entityContext.DataLocales.AddRange<LocaleEnum>(preLoadEntityContext.DataLocales);

                    Collection<Int32> relationshipTypeIdList = preLoadEntityContext.RelationshipContext.RelationshipTypeIdList;

                    if (relationshipTypeIdList != null && relationshipTypeIdList.Count > 0)
                    {
                        entityContext.RelationshipContext.RelationshipTypeIdList.AddRange<Int32>(relationshipTypeIdList);

                        if (preLoadEntityContext.RelationshipContext.LoadRelationshipAttributes)
                        {
                            entityContext.RelationshipContext.LoadRelationshipAttributes = true;
                            entityContext.RelationshipContext.LoadRelationshipAttributeModels = true;
                            entityContext.RelationshipContext.DataLocales = preLoadEntityContext.RelationshipContext.DataLocales;
                        }
                    }
                }

                #endregion Update Current Entity Context from pre-load context

                entityContext.LoadEntityProperties = true;
                entityContext.EntityTypeId = entityTypeId;
                entityContext.EntityTypeName = entityTypeName;
                entityContext.Locale = _systemDataLocale;

                if (!entityContext.DataLocales.Contains(_systemDataLocale))
                {
                    entityContext.DataLocales.Add(_systemDataLocale);
                }

                if (entityContext.AttributeIdList.Count > 0)
                {
                    entityContext.LoadAttributes = true;
                }

                if (entityContext.RelationshipContext.RelationshipTypeIdList.Count > 0 || hasEntitiesCreated)
                {
                    entityContext.LoadRelationships = true;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityFamilyChangeContext"></param>
        /// <param name="hasEntitiesCreated"></param>
        /// <returns></returns>
        private RelationshipContext GetRelationshipChangeContext(EntityFamilyChangeContext entityFamilyChangeContext, Boolean hasEntitiesCreated)
        {
            RelationshipContext relationshipContext = new RelationshipContext();
            relationshipContext.Locale = _systemDataLocale;
            relationshipContext.RelationshipTypeIdList = entityFamilyChangeContext.GetRelationshipTypeIdList();

            Collection<Int32> relationshipAttributeIdList = entityFamilyChangeContext.GetRelationshipAttributeIdList();

            if ((relationshipAttributeIdList != null && relationshipAttributeIdList.Count > 0) || hasEntitiesCreated)
            {
                relationshipContext.LoadRelationshipAttributes = true;
                relationshipContext.LoadRelationshipAttributeModels = true;

                //Getting all available locales over here to avoid duplicate value creation. Since this processor handles inherited relationships process also and
                //De-normalized get is not having mechanism to pass list of locales. Hence loading relationships attributes in all locales.
                relationshipContext.DataLocales = this.AvailableLocales;
            }

            return relationshipContext;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entitiesFilteredByAction"></param>
        /// <param name="allEntities"></param>
        /// <param name="entityOperationResults"></param>
        /// <param name="callerContext"></param>
        /// <param name="mdmEvent"></param>
        /// <param name="qualifiedMDMRules"></param>
        /// <returns></returns>
        private Collection<Dictionary<String, Object>> PrepareEntityRelatedContexts(EntityCollection entitiesFilteredByAction, EntityCollection allEntities, MDMEvent mdmEvent, MDMRuleParams mdmRuleParams)
        {
            Collection<Dictionary<String, Object>> entityRelatedContexts = new Collection<Dictionary<String, Object>>();

            foreach (Entity entity in entitiesFilteredByAction)
            {
                Dictionary<String, Object> entityRelatedContext = new Dictionary<String, Object>();

                entityRelatedContext.Add("Entity", entity);
                entityRelatedContext.Add("AllEntities", allEntities);
                entityRelatedContext.Add("MDMEvent", mdmEvent);
                entityRelatedContext.Add("MDMRuleParams", mdmRuleParams);

                entityRelatedContexts.Add(entityRelatedContext);
            }

            return entityRelatedContexts;
        }

        /// <summary>
        /// Gets all available locale locales from system.
        /// </summary>
        /// <returns>Returns collection of locale enum</returns>
        private Collection<LocaleEnum> GetAvailableLocales()
        {
            LocaleCollection availableLocales = new LocaleBL().GetAvailableLocales();
            Collection<LocaleEnum> allLocales = null;

            if (availableLocales != null && availableLocales.Count > 0)
            {
                allLocales = availableLocales.GetLocaleEnums();
            }

            return allLocales;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityRelatedContext"></param>
        private Boolean FireAsyncEvents(Dictionary<String, Object> entityRelatedContext)
        {
            Entity entity = entityRelatedContext["Entity"] as Entity;
            EntityCollection allEntities = entityRelatedContext["AllEntities"] as EntityCollection;
            MDMEvent mdmEvent = (MDMEvent)entityRelatedContext["MDMEvent"];
            MDMRuleParams ruleParams = entityRelatedContext["MDMRuleParams"] as MDMRuleParams;

            EntityCollection entities = new EntityCollection() { entity };

            return EntityOperationsCommonUtility.FireEntityEvent(entities, entities, allEntities, mdmEvent, _entityManager, ruleParams);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityId"></param>
        /// <param name="entityFamilyChangeContext"></param>
        /// <param name="callerContext"></param>
        private void EnqueueWhereUsedRelationships(Int64 entityId, EntityFamilyChangeContext entityFamilyChangeContext, CallerContext callerContext)
        {
            Collection<Int32> relationshipTypeIdList = entityFamilyChangeContext.GetRelationshipTypeIdList();

            RelationshipBL relationshipManager = new RelationshipBL();
            RelationshipCollection relationships = relationshipManager.GetWhereUsed(new Collection<Int64>() { entityId }, relationshipTypeIdList, null, _systemDataLocale, _entityManager, callerContext, false);

            if (relationships != null && relationships.Count > 0)
            {
                Collection<Int64> relatedEntityIdList = relationships.GetRelatedEntityIdList();

                if (relatedEntityIdList != null && relatedEntityIdList.Count > 0)
                {
                    LocaleChangeContextCollection localeChangeContexts = new LocaleChangeContextCollection();
                    EntityFamilyQueueCollection entityFamilyQueues = new EntityFamilyQueueCollection();

                    #region Populate Variants Entities Relationships Changed

                    Boolean hasVariantsEntitiesRelationshipsChanged = entityFamilyChangeContext.VariantsChangeContext.EntityChangeContexts.HasRelationshipsChanged();

                    if (hasVariantsEntitiesRelationshipsChanged)
                    {
                        EntityChangeContextCollection entityChangeContexts = entityFamilyChangeContext.VariantsChangeContext.EntityChangeContexts;

                        if (entityChangeContexts != null && entityChangeContexts.Count > 0)
                        {
                            foreach (EntityChangeContext entityChangeContext in entityChangeContexts)
                            {
                                foreach (LocaleChangeContext localeChangeContext in entityChangeContext.LocaleChangeContexts)
                                {
                                    LocaleChangeContext relLocaleChangeContext = new LocaleChangeContext();
                                    relLocaleChangeContext.DataLocale = localeChangeContext.DataLocale;
                                    relLocaleChangeContext.RelationshipChangeContexts = localeChangeContext.RelationshipChangeContexts;

                                    localeChangeContexts.Add(relLocaleChangeContext);
                                }
                            }
                        }
                    }

                    #endregion Populate Variants Entities Relationships Changed

                    #region Populate Extended Entities Relationships Changed

                    Boolean hasExtendedEntitiesRelationshipsChanged = entityFamilyChangeContext.ExtensionChangeContexts.HasRelationshipsChanged();

                    if (hasExtendedEntitiesRelationshipsChanged)
                    {
                        foreach (ExtensionChangeContext extensionChangeContext in entityFamilyChangeContext.ExtensionChangeContexts)
                        {
                            foreach (EntityChangeContext entityChangeContext in extensionChangeContext.VariantsChangeContext.EntityChangeContexts)
                            {
                                foreach (LocaleChangeContext localeChangeContext in entityChangeContext.LocaleChangeContexts)
                                {
                                    LocaleChangeContext relLocaleChangeContext = localeChangeContexts.Get(localeChangeContext.DataLocale);

                                    if (relLocaleChangeContext == null)
                                    {
                                        relLocaleChangeContext = new LocaleChangeContext();
                                        relLocaleChangeContext.DataLocale = localeChangeContext.DataLocale;
                                        relLocaleChangeContext.RelationshipChangeContexts = localeChangeContext.RelationshipChangeContexts;

                                        localeChangeContexts.Add(relLocaleChangeContext);
                                    }
                                    else
                                    {
                                        relLocaleChangeContext.RelationshipChangeContexts.AddRange(localeChangeContext.RelationshipChangeContexts);
                                    }
                                }
                            }
                        }
                    }

                    #endregion Populate Extended Entities Relationships Changed

                    foreach (Int64 relatedEntityId in relatedEntityIdList)
                    {
                        EntityFamilyChangeContext changeContext = new EntityFamilyChangeContext(relatedEntityId, relatedEntityId, entityFamilyChangeContext.OrganizationId, entityFamilyChangeContext.ContainerId);

                        EntityChangeContext entityChangeContext = new EntityChangeContext()
                        {
                            EntityId = relatedEntityId,
                            LocaleChangeContexts = localeChangeContexts
                        };

                        changeContext.VariantsChangeContext.EntityChangeContexts.Add(entityChangeContext);

                        EntityFamilyQueue entityFamilyQueueForWhereUsedRelationships = new EntityFamilyQueue();
                        entityFamilyQueueForWhereUsedRelationships.EntityFamilyId = relatedEntityId;
                        entityFamilyQueueForWhereUsedRelationships.EntityGlobalFamilyId = relatedEntityId;
                        entityFamilyQueueForWhereUsedRelationships.ContainerId = entityFamilyChangeContext.ContainerId;
                        entityFamilyQueueForWhereUsedRelationships.EntityFamilyChangeContexts.Add(changeContext);
                        entityFamilyQueueForWhereUsedRelationships.EntityActivityList = EntityActivityList.VariantsChange;
                        entityFamilyQueueForWhereUsedRelationships.Action = ObjectAction.Create;

                        entityFamilyQueues.Add(entityFamilyQueueForWhereUsedRelationships);
                    }

                    EntityFamilyQueueBL entityFamilyQueueBL = new EntityFamilyQueueBL();
                    OperationResultCollection operationResults = entityFamilyQueueBL.Process(entityFamilyQueues, callerContext);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityFamilyChangeContext"></param>
        /// <param name="hasRootEntityDeleted"></param>
        /// <param name="entityId"></param>
        /// <returns></returns>
        private Boolean IsRootEntityDeleted(EntityFamilyChangeContext entityFamilyChangeContext, Int64 entityId)
        {
            Boolean hasRootEntityDeleted = false;

            EntityChangeContext entityChangeContext = entityFamilyChangeContext.GetEntityChangeContext(entityId);

            if (entityChangeContext != null)
            {
                hasRootEntityDeleted = (entityChangeContext.Action == ObjectAction.Delete);
            }

            return hasRootEntityDeleted;
        }

        #endregion Helper Methods

        #region Promote Helper Methods

        /// <summary>
        /// Queues the auto promote queue item.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="familyChangeContext">The family change context.</param>
        /// <param name="callerContext">The caller context.</param>
        private void EnqueueAutoPromoteQueueItem(Entity entity, EntityFamilyChangeContext familyChangeContext, CallerContext callerContext)
        {
            if (entity.CrossReferenceId <= 0)
            {
                return;
            }

            EntityCollection flatEntities = (EntityCollection)entity.GetFlattenEntities();

            EntityFamilyChangeContext autoPromoteChangeContext = new EntityFamilyChangeContext(familyChangeContext.EntityFamilyId, familyChangeContext.EntityGlobalFamilyId, familyChangeContext.OrganizationId, familyChangeContext.ContainerId);

            PrepareVariantChangeContextForAutoPromote(familyChangeContext.VariantsChangeContext.EntityChangeContexts, flatEntities, autoPromoteChangeContext.VariantsChangeContext.EntityChangeContexts);

            foreach (ExtensionChangeContext extensionChangeContext in familyChangeContext.ExtensionChangeContexts)
            {
                ExtensionChangeContext autoPromoteExtensionChangeContext = new ExtensionChangeContext()
                {
                    ContainerId = extensionChangeContext.ContainerId,
                    ContainerName = extensionChangeContext.ContainerName,
                    ContainerType = extensionChangeContext.ContainerType,
                    ContainerQualifierName = extensionChangeContext.ContainerQualifierName
                };

                PrepareVariantChangeContextForAutoPromote(extensionChangeContext.VariantsChangeContext.EntityChangeContexts, flatEntities, autoPromoteExtensionChangeContext.VariantsChangeContext.EntityChangeContexts);

                if (autoPromoteExtensionChangeContext.VariantsChangeContext.EntityChangeContexts.Count > 0)
                {
                    autoPromoteChangeContext.ExtensionChangeContexts.Add(autoPromoteExtensionChangeContext);
                }
            }

            if (autoPromoteChangeContext.VariantsChangeContext.EntityChangeContexts.Count > 0 || autoPromoteChangeContext.ExtensionChangeContexts.Count > 0)
            {
                new EntityFamilyQueueBL().Process(new EntityFamilyQueue()
                                                    {
                                                        EntityFamilyId = entity.EntityFamilyId,
                                                        EntityGlobalFamilyId = entity.EntityGlobalFamilyId,
                                                        ContainerId = entity.ContainerId,
                                                        EntityActivityList = EntityActivityList.AutoPromote,
                                                        EntityFamilyChangeContexts = new EntityFamilyChangeContextCollection() { autoPromoteChangeContext },
                                                        Action = ObjectAction.Create
                                                    },
                                                    callerContext);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="entityFamilyChangeContext"></param>
        /// <param name="callerContext"></param>
        private void EnqueueCategoryPromoteQueueItem(Entity entity, EntityCollection allEntities, EntityFamilyChangeContext entityFamilyChangeContext, CallerContext callerContext)
        {
            EntityFamilyChangeContext categoryPromoteChangeContext = new EntityFamilyChangeContext(entity.EntityFamilyId, entity.EntityGlobalFamilyId, entity.OrganizationId, entity.ContainerId);

            ContainerBL containerBL = new ContainerBL();
            ContainerContext containerContext = new ContainerContext { ApplySecurity = false, LoadAttributes = false };
            ContainerCollection childContainers = containerBL.GetChildContainers(entity.ContainerId, containerContext, callerContext, true);

            //Get all the entities same as the root entity type
            EntityCollection filteredEntities = (EntityCollection)allEntities.GetEntitiesByEntityTypeId(entity.EntityTypeId);

            EntityFamilyQueueBL familyQueueBL = new EntityFamilyQueueBL();

            if (entityFamilyChangeContext != null && entityFamilyChangeContext.VariantsChangeContext.EntityChangeContexts.Count > 0)
            {
                //Add the master item for category promote
                familyQueueBL.Process(new EntityFamilyQueue()
                                        {
                                            EntityFamilyId = entity.EntityFamilyId,
                                            EntityGlobalFamilyId = entity.EntityGlobalFamilyId,
                                            ContainerId = entity.ContainerId,
                                            EntityActivityList = EntityActivityList.CategoryPromote,
                                            EntityFamilyChangeContexts = new EntityFamilyChangeContextCollection() { entityFamilyChangeContext },
                                            Action = ObjectAction.Create
                                        },
                                        callerContext);


                //Add each extension root entity for category promote where the container has a workflow of its own
                foreach (Entity containerFamilyEntity in filteredEntities)
                {
                    Container container = childContainers.GetContainer(containerFamilyEntity.ContainerId);

                    if (container != null && container.WorkflowType != WorkflowType.InheritParent)
                    {
                        familyQueueBL.Process(new EntityFamilyQueue()
                                                {
                                                    EntityFamilyId = containerFamilyEntity.EntityFamilyId,
                                                    EntityGlobalFamilyId = containerFamilyEntity.EntityGlobalFamilyId,
                                                    ContainerId = containerFamilyEntity.ContainerId,
                                                    EntityActivityList = EntityActivityList.CategoryPromote,
                                                    EntityFamilyChangeContexts = new EntityFamilyChangeContextCollection() { entityFamilyChangeContext },
                                                    Action = ObjectAction.Create
                                                },
                                            callerContext);

                    }
                }
            }
        }

        /// <summary>
        /// Prepares the variant change context for auto promote.
        /// </summary>
        /// <param name="variantChangeContexts">The variant change contexts.</param>
        /// <param name="flatEntities">The flat entities.</param>
        /// <param name="autoPromoteVariantContexts">The auto promote variant contexts.</param>
        private void PrepareVariantChangeContextForAutoPromote(EntityChangeContextCollection variantChangeContexts, EntityCollection flatEntities, EntityChangeContextCollection autoPromoteVariantContexts)
        {
            //No need of null checks as the property getter is returning new collection objects
            foreach (EntityChangeContext variantChangeContext in variantChangeContexts)
            {
                Entity currentEntity = (Entity)flatEntities.GetEntity(variantChangeContext.EntityId);

                if (currentEntity == null || currentEntity.CrossReferenceId <= 0)
                {
                    continue;
                }

                AttributeModelCollection currentEntityAttributeModels = currentEntity.AttributeModels;

                EntityChangeContext autoPromoteVariantContext = new EntityChangeContext
                {
                    EntityId = variantChangeContext.EntityId,
                    ParentEntityId = variantChangeContext.ParentEntityId,
                    ParentExtensionEntityId = variantChangeContext.ParentExtensionEntityId,
                    EntityTypeId = variantChangeContext.EntityTypeId,
                    EntityTypeName = variantChangeContext.EntityTypeName,
                    VariantLevel = variantChangeContext.VariantLevel,
                    Action = variantChangeContext.Action
                };

                foreach (LocaleChangeContext variantLocaleContext in variantChangeContext.LocaleChangeContexts)
                {
                    LocaleChangeContext autoPromoteLocaleContext = new LocaleChangeContext { DataLocale = variantLocaleContext.DataLocale };

                    AttributeChangeContextCollection attributeChangeContexts = variantLocaleContext.AttributeChangeContexts;
                    AttributeChangeContextCollection autoPromoteAttributeChangeContexts = autoPromoteLocaleContext.AttributeChangeContexts;

                    LocaleEnum localeContextDataLocale = variantLocaleContext.DataLocale;

                    PrepareAttributeChangeContextForAutoPromote(currentEntityAttributeModels, attributeChangeContexts, autoPromoteAttributeChangeContexts, localeContextDataLocale);

                    RelationshipChangeContextCollection autoPromoteRelationshipsChangeContexts = autoPromoteLocaleContext.RelationshipChangeContexts;

                    foreach (RelationshipChangeContext variantRelationshipContext in variantLocaleContext.RelationshipChangeContexts)
                    {
                        Int32 relationshipTypeId = variantRelationshipContext.RelationshipTypeId;

                        RelationshipChangeContext autoPromoteRelationshipChangeContext = new RelationshipChangeContext(variantRelationshipContext.RelationshipId,
                            variantRelationshipContext.FromEntityId, variantRelationshipContext.RelatedEntityId, relationshipTypeId,
                            variantRelationshipContext.RelationshipTypeName, variantRelationshipContext.Action);

                        AttributeModelCollection relationshipAttributeModels = null;

                        currentEntity.RelationshipsAttributeModels.TryGetValue(relationshipTypeId, out relationshipAttributeModels);

                        if (relationshipAttributeModels != null)
                        {
                            PrepareAttributeChangeContextForAutoPromote(relationshipAttributeModels, variantRelationshipContext.AttributeChangeContexts, autoPromoteRelationshipChangeContext.AttributeChangeContexts, localeContextDataLocale);

                            if (autoPromoteRelationshipChangeContext.AttributeChangeContexts.Count > 0)
                            {
                                autoPromoteRelationshipsChangeContexts.Add(autoPromoteRelationshipChangeContext);
                            }
                        }
                    }

                    if (autoPromoteAttributeChangeContexts.Count > 0 || autoPromoteRelationshipsChangeContexts.Count > 0)
                    {
                        autoPromoteVariantContext.LocaleChangeContexts.Add(autoPromoteLocaleContext);
                    }
                }
                if (autoPromoteVariantContext.LocaleChangeContexts.Count > 0)
                {
                    autoPromoteVariantContexts.Add(autoPromoteVariantContext);
                }
            }
        }

        /// <summary>
        /// Prepares the attribute change context for auto promote.
        /// </summary>
        /// <param name="currentEntityAttributeModels">The current entity attribute models.</param>
        /// <param name="attributeChangeContexts">The attribute change contexts.</param>
        /// <param name="autoPromoteAttributeChangeContexts">The auto promote attribute change contexts.</param>
        /// <param name="localeContextDataLocale">The locale context data locale.</param>
        private void PrepareAttributeChangeContextForAutoPromote(AttributeModelCollection currentEntityAttributeModels, AttributeChangeContextCollection attributeChangeContexts, AttributeChangeContextCollection autoPromoteAttributeChangeContexts, LocaleEnum localeContextDataLocale)
        {
            foreach (AttributeChangeContext variantAttributeChangeContext in attributeChangeContexts)
            {
                AttributeChangeContext autoPromoteAttributeContext = new AttributeChangeContext { Action = variantAttributeChangeContext.Action };

                foreach (AttributeInfo attributeInfo in variantAttributeChangeContext.AttributeInfoCollection)
                {
                    AttributeModel currentAttributeModel = currentEntityAttributeModels.GetAttributeModel(attributeInfo.Id, localeContextDataLocale) as AttributeModel;

                    if (currentAttributeModel != null && currentAttributeModel.AutoPromotable)
                    {
                        AttributeInfo currentAttributeInfo = new AttributeInfo(currentAttributeModel.Id, currentAttributeModel.Name, currentAttributeModel.AttributeParentId, currentAttributeModel.AttributeParentName);
                        autoPromoteAttributeContext.AttributeInfoCollection.Add(currentAttributeInfo);
                    }
                }

                if (autoPromoteAttributeContext.AttributeInfoCollection.Count > 0)
                {
                    autoPromoteAttributeChangeContexts.Add(autoPromoteAttributeContext);
                }
            }
        }

        #endregion Promote Helper Methods

        #region AutoExtension Helper methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mainEntity"></param>
        /// <param name="allContainers"></param>
        private void PerformAutoExtension(Entity mainEntity, ContainerCollection allContainers)
        {
            EntityCollection extendedEntities = (EntityCollection)mainEntity.GetAllExtendedEntities();
            HierarchyRelationshipCollection mainHierarchyRelationships = mainEntity.HierarchyRelationships;

            if (extendedEntities != null && extendedEntities.Count > 0)
            {
                foreach (Entity extendedEntity in extendedEntities) // all extended containers styles                
                {
                    Container extenedEntityContainer = allContainers.GetContainer(extendedEntity.ContainerId);

                    // Only perform auto extension if categories are same for both the entities and auto extension is enabled for extended container.
                    if (extenedEntityContainer != null && extenedEntityContainer.AutoExtensionEnabled &&
                        mainEntity.CategoryId == extendedEntity.CategoryId &&
                        mainEntity.Action != ObjectAction.Delete && extendedEntity.Action != ObjectAction.Delete)
                    {
                        CompareAndMergeHierarchyRelationships(mainHierarchyRelationships, extendedEntity);
                    }
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="currentEntityHierarchyRelationships"></param>
        /// <param name="extendedParentEntity"></param>
        private void CompareAndMergeHierarchyRelationships(HierarchyRelationshipCollection currentEntityHierarchyRelationships, Entity extendedParentEntity)
        {
            if (extendedParentEntity != null)
            {
                HierarchyRelationshipCollection extendedEntityHierarchyRelationships = extendedParentEntity.HierarchyRelationships;

                if (currentEntityHierarchyRelationships != null && currentEntityHierarchyRelationships.Count > 0 && extendedEntityHierarchyRelationships != null)
                {
                    foreach (HierarchyRelationship currentEntityHierarchyRelationship in currentEntityHierarchyRelationships)
                    {
                        if (currentEntityHierarchyRelationship.RelatedEntity != null && currentEntityHierarchyRelationship.RelatedEntity.Action != ObjectAction.Delete)
                        {
                            HierarchyRelationship extendedEntityHierarchyRelationship = extendedEntityHierarchyRelationships.GetHierarchyRelationshipByName(currentEntityHierarchyRelationship.Name);

                            if (extendedEntityHierarchyRelationship == null)
                            {
                                extendedEntityHierarchyRelationship = GetNewHierarchyRelationship(currentEntityHierarchyRelationship, extendedParentEntity);
                                extendedEntityHierarchyRelationships.Add(extendedEntityHierarchyRelationship);
                            }

                            CompareAndMergeHierarchyRelationships(currentEntityHierarchyRelationship.RelatedEntity.HierarchyRelationships, extendedEntityHierarchyRelationship.RelatedEntity);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="currentHierarchyRelationship"></param>
        /// <param name="extendedEntity"></param>
        /// <returns></returns>
        private HierarchyRelationship GetNewHierarchyRelationship(HierarchyRelationship currentHierarchyRelationship, Entity extendedEntity)
        {
            HierarchyRelationship extendedHierarchyRelationship = currentHierarchyRelationship.CloneBasicProperties(false);
            Entity currentHierarchyRelatedEntity = currentHierarchyRelationship.RelatedEntity;

            //Not using entity.CloneBasicProperties() because almost all the properties we need to override here.
            //So manually assigning all properties here after creating new Entity();
            Entity entity = new Entity();
            entity.Id = -1;
            entity.Action = ObjectAction.Create;
            entity.BranchLevel = currentHierarchyRelatedEntity.BranchLevel;
            entity.CategoryId = extendedEntity.CategoryId;
            entity.CategoryLongName = extendedEntity.CategoryLongName;
            entity.CategoryName = extendedEntity.CategoryName;
            entity.CategoryPath = extendedEntity.CategoryPath;
            entity.CategoryLongNamePath = extendedEntity.CategoryLongNamePath;
            entity.ContainerId = extendedEntity.ContainerId;
            entity.ContainerLongName = extendedEntity.ContainerLongName;
            entity.ContainerName = extendedEntity.ContainerName;
            entity.EntityContext = currentHierarchyRelatedEntity.EntityContext;
            entity.EntityTypeId = currentHierarchyRelatedEntity.EntityTypeId;
            entity.EntityTypeLongName = currentHierarchyRelatedEntity.EntityTypeLongName;
            entity.EntityTypeName = currentHierarchyRelatedEntity.EntityTypeName;
            entity.EntityGuid = null;
            entity.ExternalId = currentHierarchyRelatedEntity.ExternalId;
            entity.Locale = currentHierarchyRelatedEntity.Locale;
            entity.LongName = currentHierarchyRelatedEntity.LongName;
            entity.Name = currentHierarchyRelatedEntity.Name;
            entity.OrganizationId = extendedEntity.OrganizationId;
            entity.OrganizationName = extendedEntity.OrganizationName;
            entity.OrganizationLongName = extendedEntity.OrganizationLongName;
            entity.ParentEntityId = extendedEntity.Id;
            entity.ParentEntityLongName = extendedEntity.LongName;
            entity.ParentEntityName = extendedEntity.Name;
            entity.ParentEntityTypeId = extendedEntity.EntityTypeId;
            entity.ParentExtensionEntityCategoryId = currentHierarchyRelatedEntity.CategoryId;
            entity.ParentExtensionEntityCategoryLongName = currentHierarchyRelatedEntity.CategoryLongName;
            entity.ParentExtensionEntityCategoryName = currentHierarchyRelatedEntity.CategoryName;
            entity.ParentExtensionEntityCategoryPath = currentHierarchyRelatedEntity.CategoryPath;
            entity.ParentExtensionEntityCategoryLongNamePath = currentHierarchyRelatedEntity.CategoryLongNamePath;
            entity.ParentExtensionEntityContainerId = currentHierarchyRelatedEntity.ContainerId;
            entity.ParentExtensionEntityContainerLongName = currentHierarchyRelatedEntity.ContainerLongName;
            entity.ParentExtensionEntityContainerName = currentHierarchyRelatedEntity.ContainerName;
            entity.ParentExtensionEntityExternalId = currentHierarchyRelatedEntity.ExternalId;
            entity.ParentExtensionEntityId = currentHierarchyRelatedEntity.Id;
            entity.ParentExtensionEntityLongName = currentHierarchyRelatedEntity.LongName;
            entity.ParentExtensionEntityName = currentHierarchyRelatedEntity.Name;
            entity.ParentExternalId = extendedEntity.ExternalId;
            entity.Path = String.Format("{0}{1}{2}", extendedEntity.Path, Constants.STRING_PATH_SEPARATOR, entity.Name);
            entity.EntityFamilyId = extendedEntity.EntityFamilyId;
            entity.EntityFamilyLongName = extendedEntity.EntityFamilyLongName;
            entity.EntityGlobalFamilyId = extendedEntity.EntityGlobalFamilyId;
            entity.EntityGlobalFamilyLongName = extendedEntity.EntityGlobalFamilyLongName;
            entity.HierarchyLevel = currentHierarchyRelatedEntity.HierarchyLevel;

            if (currentHierarchyRelatedEntity.SourceInfo != null)
            {
                entity.SourceInfo = (SourceInfo)currentHierarchyRelatedEntity.SourceInfo.Clone();
            }
            else
            {
                entity.SourceInfo = null;
            }

            extendedHierarchyRelationship.RelatedEntity = entity;

            return extendedHierarchyRelationship;
        }

        #endregion AutoExtension Helper methods

        #region Workflow Async Business Rules Helper Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityFamilyQueue"></param>
        /// <param name="entityFamilyChangeContext"></param>
        /// <param name="callerContext"></param>
        /// <param name="diagnosticActivity"></param>
        /// <returns></returns>
        private EntityOperationResult Process(EntityFamilyQueue entityFamilyQueue, EntityFamilyChangeContext entityFamilyChangeContext, CallerContext callerContext, DiagnosticActivity diagnosticActivity)
        {
            Boolean isTracingEnabled = _currentTraceSettings.IsBasicTracingEnabled;
            EntityBL entityBL = new EntityBL();
            String errorMsg = String.Empty;
            EntityOperationResult entityOperationResult = null;

            EntityContext entityContext = new EntityContext()
            {
                LoadAttributes = true, //TODO: Set this to false or remove it once ensure data takes care of this in WorkflowHelper.EvaluateWorkflowBusinessRules call
                LoadEntityProperties = true,
                LoadStateValidationAttributes = false,
                LoadWorkflowInformation = false
            };

            Entity entity = entityBL.Get(entityFamilyQueue.EntityFamilyId, entityContext, MDMCenterApplication.PIM, MDMCenterModules.WindowsWorkflow);
            EntityCollection entities = new EntityCollection { entity };

            EntityOperationResultCollection resultCollection = WorkflowHelper.EvaluateWorkflowBusinessRules(entityFamilyQueue, entities, MDMEvent.WorkflowAsyncBusinessRuleExecution, callerContext);
            entityOperationResult = (resultCollection != null) ? (EntityOperationResult)resultCollection.GetByEntityId(entityFamilyQueue.EntityFamilyId) : new EntityOperationResult();

            if (!entityOperationResult.HasError)
            {
                EntityOperationResultCollection entityOperationResults = entityBL.Process(entities, new EntityProcessingOptions(false, false, false, false), callerContext);
                entityOperationResult = (EntityOperationResult)entityOperationResults.GetByEntityId(entityFamilyQueue.EntityFamilyId);

                //Entity process failed
                if (entityOperationResult != null && entityOperationResult.HasError)
                {
                    diagnosticActivity.LogError(entityOperationResult.ToXml());
                }
            }
            else
            {
                diagnosticActivity.LogError(entityOperationResult.ToXml());
            }

            //Resume the workflow irrespective of failure.Send the information via workflow action context.
            EntityOperationsCommonUtility.ResumeWorkflowInstance(entityFamilyQueue, entityOperationResult, entities, callerContext);

            if (entityOperationResult != null && entityOperationResult.HasError)
            {
                errorMsg = String.Format("Unable to resume the workflow for Entity Global Family Id: {0}", entityFamilyQueue.EntityGlobalFamilyId);
                diagnosticActivity.LogError(errorMsg);
            }

            return entityOperationResult;
        }

        #endregion Workflow Async Business Rules Helper Methods

        #region Jigsaw Integration Methods

        /// <summary>
        /// Creates entity id to entity change context map for each entity given family.
        /// Individual change conetxt is calculated based on the following logic:
        ///     1. Current EntityChangeContext from given entityFamilyChangeContext
        ///     2. Adds immediate Parent EntityChangeContext if exisits.
        ///     3. Adds Extension Parent EntityChangeContext if exisits.
        ///  The updated the map if updated/merged changed context is not null.
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="entityFamilyChangeContext"></param>
        /// <param name="callerContext"></param>
        /// <returns>Aggregated EntityChangeContextsMap of enitty id to entity change context for each entity given family.</returns>
        private Dictionary<Int64, EntityChangeContext> GetEntityChangeContextsMap(EntityCollection entities, EntityFamilyChangeContext entityFamilyChangeContext, CallerContext callerContext)
        {
            #region Parameter validation & Initialization

            if (entities == null || entities.Count == 0)
            {
                return null;
            }

            Dictionary<Int64, EntityChangeContext> entityChangeContextsMap = new Dictionary<long, EntityChangeContext>();

            #endregion

            foreach (var entity in entities)
            {
                #region Initialization of current, parent and extension parent change contexts.

                if (entityChangeContextsMap.ContainsKey(entity.Id))
                {
                    continue;
                }

                EntityChangeContext currentEntityChangeContext;
                EntityChangeContext parentEntityChangeContext;
                EntityChangeContext extensionParentEntityChangeContext;

                // Get current context from family change context.
                currentEntityChangeContext = entityFamilyChangeContext.GetEntityChangeContext(entity.Id);

                // Get parent and extension parent from the map.
                entityChangeContextsMap.TryGetValue(entity.ParentEntityId, out parentEntityChangeContext);
                entityChangeContextsMap.TryGetValue(entity.ParentExtensionEntityId, out extensionParentEntityChangeContext);

                #endregion

                #region Update current EntityChangeContext with Parent EntityChangeContext

                if (currentEntityChangeContext != null)
                {
                    if (parentEntityChangeContext != null)
                    {
                        currentEntityChangeContext.Merge(parentEntityChangeContext);
                    }
                }
                else
                {
                    currentEntityChangeContext = parentEntityChangeContext;
                }

                #endregion

                #region Update current EntityChangeContext with Extension Parent EntityChangeContext

                if (currentEntityChangeContext != null)
                {
                    if (extensionParentEntityChangeContext != null)
                    {
                        currentEntityChangeContext.Merge(extensionParentEntityChangeContext);
                    }
                }
                else
                {
                    currentEntityChangeContext = extensionParentEntityChangeContext;
                }

                #endregion

                #region Add current EntityChangeContext to the map if not null

                if (currentEntityChangeContext != null)
                {
                    entityChangeContextsMap.Add(entity.Id, currentEntityChangeContext);
                }

                #endregion
            }

            return entityChangeContextsMap;
        }

        /// <summary>
        /// Finds inherited attributes of each entity and updates each such attributes action from respective entity change context.
        /// When a CateogryChange comes for processing, there is only one EntityChangeContext, that of the category which we use for all impacted children.
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="entityFamilyChangeContext"></param>
        /// <param name="originalEntityFamilyChangeContext"></param>
        /// <param name="callerContext"></param>
        /// <returns></returns>
        private EntityCollection GetAdjustedAttributeActionEntityCollection(EntityCollection entities, EntityFamilyChangeContext entityFamilyChangeContext, EntityFamilyChangeContext originalEntityFamilyChangeContext, CallerContext callerContext)
        {
            DiagnosticActivity diagnosticActivity = null;
            Boolean isTracingEnabled = _currentTraceSettings.IsBasicTracingEnabled;

            try
            {
                #region Parameter validation & Initialization

                if (isTracingEnabled)
                {
                    diagnosticActivity.Start();
                }

                EntityCollection adjustedEntities = new EntityCollection();

                if (entityFamilyChangeContext == null)
                {
                    return adjustedEntities;
                }

                Dictionary<Int64, EntityChangeContext> entityChangeContextsMap = null;
                EntityChangeContext categoryChangeContext = null;
                EntityChangeContext entityChangeContext = null;

                bool isCategoryChange = entityFamilyChangeContext.EntityActivityList == EntityActivityList.CategoryChange;

                if (!isCategoryChange)
                {
                    entityChangeContextsMap = GetEntityChangeContextsMap(entities, entityFamilyChangeContext, callerContext);

                    if (entityChangeContextsMap == null || entityChangeContextsMap.Count == 0)
                    {
                        return adjustedEntities;
                    }
                }
                else
                {
                    if (entityFamilyChangeContext.VariantsChangeContext != null && entityFamilyChangeContext.VariantsChangeContext.EntityChangeContexts != null)
                    {
                        // For category change, only one entity change context will be in the incoming family Context.

                        if (isTracingEnabled)
                        {
                            diagnosticActivity.LogWarning("More than one entity found for EntityActivityList.CategoryChange in the Family Context: " + entityFamilyChangeContext.ToXml());
                        }

                        categoryChangeContext = entityFamilyChangeContext.VariantsChangeContext.EntityChangeContexts.FirstOrDefault();
                    }
                }

                #endregion

                foreach (var entity in entities)
                {
                    #region Check for entity change context in the map or use the category change context coming in.

                    entityChangeContext = null;

                    if (!isCategoryChange)
                    {
                        if (!entityChangeContextsMap.TryGetValue(entity.Id, out entityChangeContext) || entityChangeContext == null)
                        {
                            continue;
                        }
                    }
                    else
                    {
                        entityChangeContext = categoryChangeContext;
                    }

                    #endregion

                    #region Check for any mapped inherited attributes to be processed for this entity

                    var entityClone = entity.Clone();

                    // Adjust entity action
                    entityClone.Action = ObjectAction.ImpactedUpdate;

                    // Adjust entity attributes - keep only inherited attriutes
                    AttributeCollection inheritedAttributes = GetInheritedAttributesAction(entityClone, originalEntityFamilyChangeContext);

                    if (inheritedAttributes == null || inheritedAttributes.Count == 0)
                    {
                        continue;
                    }

                    entityClone.Attributes.Clear();
                    entityClone.Attributes.AddRange(inheritedAttributes);

                    var localeActionAttributeChangeContextMap = entityChangeContext.GetAttributeChangeContexts(entityClone.Attributes);

                    if (localeActionAttributeChangeContextMap == null || localeActionAttributeChangeContextMap.Count == 0)
                    {
                        continue;
                    }

                    #endregion

                    #region  Adjust each inherited attributes Action
                    Boolean hasImpactedChange = false;

                    foreach (var locale in localeActionAttributeChangeContextMap.Keys)
                    {
                        var actionToAttributeChangeContextMap = localeActionAttributeChangeContextMap[locale];

                        foreach (var action in actionToAttributeChangeContextMap.Keys)
                        {
                            var attributeChangeContext = actionToAttributeChangeContextMap[action];

                            foreach (var attributeInfo in attributeChangeContext.AttributeInfoCollection)
                            {
                                var attribute = entityClone.GetAttribute(attributeInfo.Id, locale);

                                if (attribute != null)
                                {
                                    if (!attribute.HasAnyValue())
                                    {
                                        // Attribute was loaded but has neither I or O values.
                                        // This means entity process deleted it or an O->I swithc happend and parent doesn't have any values.
                                        attribute.Action = ObjectAction.Delete;
                                        hasImpactedChange = true;
                                    }
                                    else if (attribute.SourceFlag == AttributeValueSource.Overridden)
                                    {
                                        // Don't send O Values as part of ImpactedUpdate
                                        attribute.Action = ObjectAction.Ignore;
                                    }
                                    else
                                    {
                                        // I Valules have to be sent.
                                        // Note that there is no-way to NOT send I Values of SKUs when say Color as O Value and Style modified the attr.
                                        // Techincally since this change will not affect color or skus in question, all sku I values, though have not bee changed
                                        // will be still sent to Jigsaw.
                                        attribute.Action = action;
                                        hasImpactedChange = true;
                                    }
                                }
                            }
                        }
                    }

                    if (hasImpactedChange)
                    {
                        adjustedEntities.Add(entityClone);
                    }

                    #endregion
                }

                return adjustedEntities;
            }
            finally
            {
                if (isTracingEnabled)
                {
                    diagnosticActivity.Stop();
                }

            }
        }

        /// <summary>
        /// Returns inherited attributes (cloned) of given entity.
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="originalEntityFamilyChangeContext"></param>
        /// <returns></returns>
        private AttributeCollection GetInheritedAttributesAction(Entity entity, EntityFamilyChangeContext originalEntityFamilyChangeContext)
        {
            if (entity == null)
            {
                return null;
            }

            AttributeCollection inheritedAttriutes = new AttributeCollection();
            EntityChangeContext originalEntityChangeContext = null;

            if (originalEntityFamilyChangeContext != null)
            {
                originalEntityChangeContext = originalEntityFamilyChangeContext.GetEntityChangeContext(entity.Id);
            }

            foreach (var attribute in entity.Attributes)
            {
                var model = entity.AttributeModels.GetAttributeModel(attribute.Id, attribute.Locale);

                if (model != null)
                {
                    // Check for attribute qualification  first. 
                    var isAttributeInheritable = model.Inheritable && model.Searchable;

                    // Each entity that is going thru entity process, send appropriate message to Jigsaw duirng Entity Process flow (Original Change Context).
                    // The same entity will come for procesing here in Family Processor flow. If an attribute is present in originla change context, its value 
                    // would have been already sent. So unless it was Delete Action (in O -> I scenario, we do need to process Delete action and send I value)
                    // we don't need to include attributes to be sent.
                    // An entity with no attributes to send, will be eliminated from jigsaw send logic and this is how we aovid sending impactedUpdate message
                    // with same exact attributes and values like in update message. 

                    var isAttributeQualifiedWhenComparedOriginalEntityChangeContext =
                                  originalEntityChangeContext == null                                       // Check if this entity has original change context (for style 
                               || originalEntityChangeContext.AttributeIdList == null                       // Check if there are any attributes and
                               || !originalEntityChangeContext.AttributeIdList.Contains(attribute.Id)       // Check if this attribute is NOT part of original changes.
                               || IsOriginalAttributeActionDelete(originalEntityChangeContext, attribute);  // Check if original action was delete (O -> I)

                    if (isAttributeInheritable && isAttributeQualifiedWhenComparedOriginalEntityChangeContext)
                    {
                        var attribuetClone = attribute.Clone();

                        // This action will be later over written by value in family Context.
                        attribuetClone.Action = ObjectAction.Update;
                        inheritedAttriutes.Add(attribuetClone);
                    }
                }
            }

            return inheritedAttriutes;
        }


        /// <summary>
        /// Determines whether attribute action is Delete in the specified original entity change context.
        /// </summary>
        /// <param name="originalEntityChangeContext">The original entity change context.</param>
        /// <param name="attribute">The attribute.</param>
        /// <returns></returns>
        private Boolean IsOriginalAttributeActionDelete(EntityChangeContext originalEntityChangeContext, Attribute attribute)
        {
            if (attribute != null && originalEntityChangeContext != null && originalEntityChangeContext.LocaleChangeContexts != null)
            {
                var localeChangeContext = originalEntityChangeContext.LocaleChangeContexts.SingleOrDefault(localeContext => localeContext.DataLocale == attribute.Locale);

                if (localeChangeContext != null && localeChangeContext.AttributeChangeContexts != null)
                {
                    var deleteActionContext = localeChangeContext.AttributeChangeContexts.SingleOrDefault(attributeContext => attributeContext.Action == ObjectAction.Delete);

                    if (deleteActionContext != null)
                    {
                        if (deleteActionContext.AttributeInfoCollection.Contains(attribute.Id))
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        #endregion

        #endregion Private Methods

        #endregion Methods
    }
}