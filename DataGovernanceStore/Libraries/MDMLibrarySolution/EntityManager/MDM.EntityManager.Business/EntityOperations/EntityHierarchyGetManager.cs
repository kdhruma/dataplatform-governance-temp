using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace MDM.EntityManager.Business.EntityOperations
{
    using AttributeModelManager.Business;
    using BusinessObjects;
    using BusinessObjects.Diagnostics;
    using ConfigurationManager.Business;
    using Core;
    using Core.Exceptions;
    using Data;
    using DataModelManager.Business;
    using Helpers;
    using Interfaces;
    using MessageManager.Business;
    using ParallelizationManager.Processors;
    using PermissionManager.Business;
    using RelationshipManager.Business;
    using Utility;

    /// <summary>
    /// Provides methods to retrieve entity along with its hierarchy (entity as a whole) 
    /// </summary>
    public class EntityHierarchyGetManager
    {
        #region Fields

        /// <summary>
        ///     Field denoting locale Message
        /// </summary>
        private LocaleMessage _localeMessage;

        /// <summary>
        ///     Field denoting localeMessageBL
        /// </summary>
        private readonly LocaleMessageBL _localeMessageBL;

        /// <summary>
        /// Field denoting the Entity Manager
        /// </summary>
        private readonly IEntityManager _entityManager;

        /// <summary>
        /// Field denoting the Entity Type Manager
        /// </summary>
        private readonly EntityTypeBL _entityTypeManager;

        /// <summary>
        ///     Field denoting the security principal
        /// </summary>
        private readonly SecurityPrincipal _securityPrincipal;

        /// <summary>
        /// Indicates current trace settings
        /// </summary>
        private readonly TraceSettings _currentTraceSettings;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes the EntityHierarchyManager class
        /// </summary>
        public EntityHierarchyGetManager(IEntityManager entityManager)
        {
            _entityManager = entityManager;
            _securityPrincipal = SecurityPrincipalHelper.GetCurrentSecurityPrincipal();
            _entityTypeManager = new EntityTypeBL();
            _localeMessageBL = new LocaleMessageBL();
            _currentTraceSettings = MDMOperationContextHelper.GetCurrentTraceSettings();
        }

        #endregion Constructors

        #region Properties
        #endregion Properties

        #region Methods

        #region Public Methods

        /// <summary>
        /// Gets the entity object along with its hierarchy based on given entity unique identifier and the entity context.
        /// </summary>
        /// <param name="entityUniqueIdentifier">Specifies the entity identifier for which the entities needs to be retrieved</param>
        /// <param name="entityContext">Specifies the entity context for which entities needs to be retrieved</param>
        /// <param name="entityGetOptions">Specifies the options to be considered while retrieving entity object</param>
        /// <param name="callerContext">Specifies the caller context, which contains the application and module that has invoked the API</param>
        /// <returns>Returns entity with its hierarchy entities loaded based on the requested context</returns>
        public Entity GetEntityHierarchy(EntityUniqueIdentifier entityUniqueIdentifier, EntityContext entityContext, EntityGetOptions entityGetOptions, CallerContext callerContext)
        {
            #region Initial Setup

            Entity mainEntity = null;
            EntityCollection allEntities = new EntityCollection();
            Int32 maxDegreeOfParallelism = 3;//AppConfigurationHelper.GetAppConfig<Int32>("MDMCenter.EntityGetManager.ParallelEntityGet.MaxDegreeOfParallelism", 4);
            Collection<Dictionary<String, Object>> hierarchyRelatedContexts = null;
            Dictionary<EntityUniqueIdentifier, EntityContext> entityContextBasedOnUniqueIdentifier = null;
            var diagnosticActivity = new DiagnosticActivity();

            #endregion

            try
            {
                if (_currentTraceSettings.IsBasicTracingEnabled)
                {
                    var callDataContext = entityContext.GetNewCallDataContext();
                    callDataContext.EntityIdList = new Collection<Int64> { entityUniqueIdentifier.EntityId };
                    var executionContext = new ExecutionContext(callerContext, callDataContext, _securityPrincipal.GetSecurityContext(), entityGetOptions.ToXml());
                    executionContext.LegacyMDMTraceSources.Add(MDMTraceSource.EntityHierarchyGet);
                    diagnosticActivity.Start(executionContext);
                }

                ValidateInputParams(entityUniqueIdentifier, entityContext, entityGetOptions, callerContext);

                ValidateAndUpdateEntityExtensionContext(entityContext, callerContext, diagnosticActivity);

                #region Step : Get all top level entity ids across containers

                entityContextBasedOnUniqueIdentifier = GetEntityContextBasedUniqueIdentifier(entityContext, entityUniqueIdentifier, callerContext, diagnosticActivity);

                #endregion Step : Get all top level entity ids across containers

                #region Step : Run In Parallel

                if (entityContextBasedOnUniqueIdentifier.Count > 0)
                {
                    hierarchyRelatedContexts = new Collection<Dictionary<String, Object>>();

                    foreach (KeyValuePair<EntityUniqueIdentifier, EntityContext> keyValuePair in entityContextBasedOnUniqueIdentifier)
                    {
                        Dictionary<String, Object> hierachyRelatedContext = new Dictionary<String, Object>();

                        hierachyRelatedContext.Add("EntityUniqueIdentifier", keyValuePair.Key);
                        hierachyRelatedContext.Add("EntityContext", keyValuePair.Value);
                        hierachyRelatedContext.Add("EntityGetOptions", entityGetOptions);
                        hierachyRelatedContext.Add("CallerContext", callerContext);

                        hierarchyRelatedContexts.Add(hierachyRelatedContext);
                    }
                }

                //Skip the Transform Block if there is only one entity to transform
                if (hierarchyRelatedContexts != null && hierarchyRelatedContexts.Count > 1)
                {
                    Collection<Entity> entities = new ParallelTaskProcessor().RunInParallel<Dictionary<String, Object>, Entity>(hierarchyRelatedContexts, GetEntityHierarchy, null, maxDegreeOfParallelism);
                    allEntities.Add(entities.ToList());

                    #region Build extension relationships with child entities reference tree

                    if (entityContext.LoadExtensionRelationships && allEntities.Count > 0)
                    {
                        mainEntity = (Entity)allEntities.GetEntity(entityUniqueIdentifier.EntityId);

                        if (mainEntity != null)
                        {
                            BuildExtensionRelationshipsWithChildEntitiesReferenceTree(mainEntity, allEntities);
                            BuildExtensionRelationshipWithParentEntityReferenceTree(mainEntity, allEntities);
                        }
                    }

                    #endregion
                }
                else
                {
                    mainEntity = GetEntityHierarchyMain(entityUniqueIdentifier, entityContext, entityGetOptions, callerContext);
                }

                #endregion
            }
            finally
            {
                #region Final Diagnostics and tracing

                if (_currentTraceSettings.IsBasicTracingEnabled)
                {
                    diagnosticActivity.Stop();
                }

                #endregion Final Diagnostics and tracing
            }

            return mainEntity;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="entityContext"></param>
        /// <param name="entityGetOptions"></param>
        /// <param name="callerContext"></param>
        /// <returns></returns>
        public Boolean LoadEntityHierarchy(Entity entity, EntityContext entityContext, EntityGetOptions entityGetOptions, CallerContext callerContext)
        {
            Boolean successFlag = true;

            if (entity == null)
            {
                return false;
            }

            var entityUniqueIdentifier = new EntityUniqueIdentifier { EntityId = entity.Id };

            var loadedMainEntity = GetEntityHierarchy(entityUniqueIdentifier, entityContext, entityGetOptions, callerContext);

            if (loadedMainEntity != null)
            {
                entity.HierarchyRelationships = loadedMainEntity.HierarchyRelationships; // Live reference of ensured hierarchy relationships     
            }
            else
            {
                successFlag = false;
            }

            return successFlag;
        }

        /// <summary>
        /// Gets child entities for requested entity id, entity type id and attribute id list
        /// </summary>
        /// <param name="entityId">Entity Id</param>
        /// <param name="requestedChildEntityTypeId">Entity Type Id</param>
        /// <param name="locale">Data Locale</param>
        /// <param name="returnAttributeIds">Attribute Id List</param>
        /// <param name="maxRecordsToReturn">no of records to return</param>
        /// <param name="callerContext">context of caller</param>
        /// <param name="getRecursiveChildren">whether to load recursive children</param>
        /// <returns>Child entities</returns>
        public EntityCollection GetChildEntitiesInternal(Int64 entityId, Int32 requestedChildEntityTypeId, LocaleEnum locale, Collection<Int32> returnAttributeIds, Int32 maxRecordsToReturn, CallerContext callerContext, Boolean getRecursiveChildren = false)
        {
            EntityCollection allChildEntities = null;
            var diagnosticActivity = new DiagnosticActivity();

            try
            {
                if (_currentTraceSettings.IsBasicTracingEnabled)
                {
                    diagnosticActivity.Start();
                    diagnosticActivity.LogMessage(MessageClassEnum.Information, "Starting GetChildEntitiesInternal method for entity Id : " + entityId);
                }

                var fillOptions = new EntityGetOptions.EntityFillOptions();
                fillOptions.SetAllEntityPropertiesLevelFillOptionsTo(false); // No need to load entity properties during base entity get

                var entityGetOptions = new EntityGetOptions { ApplyAVS = false, ApplySecurity = false, PublishEvents = false, FillOptions = fillOptions, UpdateCacheStatusInDB = false, UpdateCache = false };

                var baseEntityContext = new EntityContext { LoadEntityProperties = true, LoadAttributes = false, Locale = locale, DataLocales = new Collection<LocaleEnum> { locale } };

                var baseEntity = new EntityBL().Get(entityId, baseEntityContext, entityGetOptions, callerContext);

                if (baseEntity != null)
                {
                    #region Create main entity context

                    // When entityContext.DataLocales is not contains system data locale, needs to add system data locale and CDL both in entityContext.DataLocales...
                    LocaleEnum systemDataLocale = GlobalizationHelper.GetSystemDataLocale();

                    var mainEntityContext = new EntityContext
                    {
                        Locale = locale,
                        DataLocales = new Collection<LocaleEnum> { systemDataLocale },
                        LoadEntityProperties = true,
                        LoadHierarchyRelationships = true,
                        ContainerId = baseEntity.ContainerId,
                        EntityTypeId = baseEntity.EntityTypeId,
                        CategoryId = baseEntity.CategoryId
                    };

                    // When entityHierarchy requested for current data locale needs to add CDL and systemDataLocale both in entityContext...
                    if (locale != systemDataLocale)
                    {
                        mainEntityContext.DataLocales.Add(locale);
                    }

                    if (returnAttributeIds != null && returnAttributeIds.Count > 0)
                    {
                        mainEntityContext.LoadAttributes = true;
                        mainEntityContext.LoadLookupDisplayValues = true;
                        mainEntityContext.AttributeIdList = returnAttributeIds;
                    }

                    fillOptions.SetAllEntityPropertiesLevelFillOptionsTo(true); // Now, we need to load all entity master info

                    #endregion

                    PopulateHierarchyContext(entityId, baseEntity.EntityTypeId, mainEntityContext, requestedChildEntityTypeId, getRecursiveChildren, callerContext);

                    var mainEntityAndHierarchyEntities = GetHierarchyEntities(entityId, mainEntityContext, entityGetOptions, callerContext);

                    if (mainEntityAndHierarchyEntities != null)
                    {
                        allChildEntities = mainEntityAndHierarchyEntities.Item2;
                        allChildEntities.Remove(entityId);

                        if (allChildEntities != null && allChildEntities.Count > 0)
                        {
                            allChildEntities = FilterChildEntitiesAsRequested(allChildEntities, requestedChildEntityTypeId, maxRecordsToReturn, callerContext);
                        }
                    }
                }
            }
            finally
            {
                #region Final Diagnostics and tracing

                if (_currentTraceSettings.IsBasicTracingEnabled)
                {
                    diagnosticActivity.Stop();
                }

                #endregion Final Diagnostics and tracing
            }

            return allChildEntities;
        }

        #endregion Public Methods

        #region Private Methods

        #region Main Get Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityId"></param>
        /// <param name="requstedEntityContext"></param>
        /// <param name="entityGetOptions"></param>
        /// <param name="callerContext"></param>
        /// <returns></returns>
        private Tuple<Entity, EntityCollection> GetHierarchyEntities(Int64 entityId, EntityContext requstedEntityContext, EntityGetOptions entityGetOptions, CallerContext callerContext)
        {
            var diagnosticActivity = new DiagnosticActivity();

            Entity mainEntity = null;
            EntityCollection entities;

            try
            {
                if (_currentTraceSettings.IsBasicTracingEnabled)
                {
                    diagnosticActivity.Start();
                }

                Dictionary<Int32, ReferenceDataBag> referenceDataBags = LoadReferenceDataBags(entityId, requstedEntityContext, entityGetOptions, callerContext);

                if (_currentTraceSettings.IsBasicTracingEnabled)
                {
                    diagnosticActivity.LogDurationInfo("Completed loading reference data");
                }

                // Load Base Entities and it's Attributes from database
                entities = LoadEntities(entityId, requstedEntityContext, callerContext, entityGetOptions, referenceDataBags);

                if (entities != null)
                {
                    if (_currentTraceSettings.IsBasicTracingEnabled)
                    {
                        diagnosticActivity.LogDurationInfo("Completed loading base entities and its attributes from database");
                    }

                    #region Load Relationships

                    // Prepare relationship context based entity collection for relationship load
                    var relationshipContextToEntityCollectionMaps = FilterEntitiesByRelationshipContexts(entities, requstedEntityContext);

                    if (relationshipContextToEntityCollectionMaps != null && relationshipContextToEntityCollectionMaps.Count > 0)
                    {
                        var relationshipBL = new RelationshipBL();
                        relationshipBL.LoadRelationships(relationshipContextToEntityCollectionMaps, _entityManager, callerContext);

                        if (_currentTraceSettings.IsBasicTracingEnabled)
                        {
                            diagnosticActivity.LogDurationInfo("Completed loading relationships for requested context");
                        }
                    }

                    #endregion

                    mainEntity = (Entity)entities.GetEntity(entityId);

                    PerfomPostLoadOperations(mainEntity, entities, requstedEntityContext, entityGetOptions, callerContext);
                }
                else
                {
                    diagnosticActivity.LogMessage(MessageClassEnum.Warning, "No Entities loaded for entity id : " + entityId);
                }
            }
            finally
            {
                if (_currentTraceSettings.IsBasicTracingEnabled)
                {
                    diagnosticActivity.Stop();
                }
            }

            return new Tuple<Entity, EntityCollection>(mainEntity, entities);
        }

        /// <summary>
        /// Gets the entity object along with its hierarchy based on given entity unique identifier and the entity context.
        /// </summary>
        /// <param name="entityUniqueIdentifier">Specifies the entity identifier for which the entities needs to be retrieved</param>
        /// <param name="entityContext">Specifies the entity context for which entities needs to be retrieved</param>
        /// <param name="entityGetOptions">Specifies the options to be considered while retrieving entity object</param>
        /// <param name="callerContext">Specifies the caller context, which contains the application and module that has invoked the API</param>
        /// <returns>Returns entity with its hierarchy entities loaded based on the requested context</returns>
        private Entity GetEntityHierarchyMain(EntityUniqueIdentifier entityUniqueIdentifier, EntityContext entityContext, EntityGetOptions entityGetOptions, CallerContext callerContext)
        {
            Entity entity;

            var diagnosticActivity = new DiagnosticActivity();

            try
            {
                if (_currentTraceSettings.IsBasicTracingEnabled)
                {
                    var callDataContext = entityContext.GetNewCallDataContext();
                    callDataContext.EntityIdList = new Collection<Int64> { entityUniqueIdentifier.EntityId };
                    var executionContext = new ExecutionContext(callerContext, callDataContext, _securityPrincipal.GetSecurityContext(), entityGetOptions.ToXml());
                    executionContext.LegacyMDMTraceSources.Add(MDMTraceSource.EntityHierarchyGet);
                    diagnosticActivity.Start(executionContext);
                }

                ValidateInputEntityUniqueIdentifier(entityUniqueIdentifier);

                //EntityIdentificationManager.ResolveEntityUniqueIdentifier(entityUniqueIdentifier, callerContext, _currentTraceSettings);

                ValidateResolvedEntitiyIdentifier(entityUniqueIdentifier);

                var entityId = entityUniqueIdentifier.EntityId;

                if (_currentTraceSettings.IsBasicTracingEnabled)
                {
                    diagnosticActivity.LogMessage(MessageClassEnum.Information, "Starting Get Entity with hierarchy for entity Id : " + entityId);
                }

                ValidateAndUpdateEntityContext(entityContext, entityUniqueIdentifier, callerContext);

                Tuple<Entity, EntityCollection> mainEntityAndHierarchyEntities = GetHierarchyEntities(entityId, entityContext, entityGetOptions, callerContext);

                entity = mainEntityAndHierarchyEntities.Item1;

                #region Build hierarchy relationships with child entities reference tree

                if (entityContext.LoadHierarchyRelationships)
                {
                    if (entity != null)
                    {
                        BuildHierarchyRelationshipsWithChildEntitiesReferenceTree(entity, mainEntityAndHierarchyEntities.Item2);
                        BuildHierarchyRelationshipWithParentEntityReferenceTree(entity, mainEntityAndHierarchyEntities.Item2);
                    }
                }

                #endregion
            }
            finally
            {
                #region Final Diagnostics and tracing

                if (_currentTraceSettings.IsBasicTracingEnabled)
                {
                    diagnosticActivity.Stop();
                }

                #endregion Final Diagnostics and tracing
            }

            return entity;
        }

        #endregion

        #region Validation Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityUniqueIdentifier"></param>
        private void ValidateInputEntityUniqueIdentifier(EntityUniqueIdentifier entityUniqueIdentifier)
        {
            if (entityUniqueIdentifier.EntityId < 1 && String.IsNullOrWhiteSpace(entityUniqueIdentifier.ExternalId))
            {
                throw new MDMOperationException("111817", "ExternalId cannot be null", "EntityHierarchyGetManager", String.Empty, "ValidateInputEntityUniqueIdentifier"); // Todo: change locale message code
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityUniqueIdentifier"></param>
        private void ValidateResolvedEntitiyIdentifier(EntityUniqueIdentifier entityUniqueIdentifier)
        {
            if (entityUniqueIdentifier.EntityId < 1)
            {
                throw new MDMOperationException("113984", String.Format("Failed to identify entity based on provided entity external id:{0}", entityUniqueIdentifier.EntityId), "EntityHierarchyGetManager", String.Empty, "ValidateResolvedEntitiyIdentifier"); // Todo: change locale message code
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityUniqueIdentifier"></param>
        /// <param name="entityContext"></param>
        /// <param name="entityGetOptions"></param>
        /// <param name="callerContext"></param>
        private void ValidateInputParams(EntityUniqueIdentifier entityUniqueIdentifier, EntityContext entityContext, EntityGetOptions entityGetOptions, CallerContext callerContext)
        {
            var diagnosticActivity = new DiagnosticActivity();

            if (_currentTraceSettings.IsBasicTracingEnabled)
            {
                diagnosticActivity.Start();
            }

            if (entityUniqueIdentifier == null)
            {
                diagnosticActivity.LogMessage(MessageClassEnum.Error, "EntityUniqueIdentifier is null or empty");
                throw new MDMOperationException("113985", "EntityUniqueIdentifier is null or empty", "EntityHierarchyGetManager", String.Empty, "ValidateInputParams"); //TODO:: update message code
            }

            if (entityContext == null)
            {
                diagnosticActivity.LogMessage(MessageClassEnum.Error, "EntityContext is null or empty");
                throw new MDMOperationException("111792", "EntityContext is null or empty", "EntityHierarchyGetManager", String.Empty, "ValidateInputParams");
            }

            if (entityGetOptions == null)
            {
                diagnosticActivity.LogMessage(MessageClassEnum.Error, "EntityGetOptions cannot be null");
                throw new MDMOperationException("113886", "EntityGetOptions cannot be null", "EntityHierarchyGetManager", String.Empty, "ValidateInputParams");
            }

            if (callerContext == null)
            {
                diagnosticActivity.LogMessage(MessageClassEnum.Error, "CallerContext cannot be null");
                throw new MDMOperationException("111846", "CallerContext cannot be null", "EntityHierarchyGetManager", String.Empty, "ValidateInputParams");
            }

            if (_currentTraceSettings.IsBasicTracingEnabled)
            {
                diagnosticActivity.Stop();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mainEntityContext"></param>
        /// <param name="entityUniqueIdentifier"></param>
        /// <param name="callerContext"></param>
        private void ValidateAndUpdateEntityContext(EntityContext mainEntityContext, EntityUniqueIdentifier entityUniqueIdentifier, CallerContext callerContext)
        {
            var diagnosticActivity = new DiagnosticActivity();

            if (_currentTraceSettings.IsBasicTracingEnabled)
            {
                diagnosticActivity.Start();
            }

            Boolean isRequiredContextParamsSet = mainEntityContext.LoadEntityProperties || mainEntityContext.IsAttributeLoadingRequired || mainEntityContext.LoadRelationships || mainEntityContext.LoadHierarchyRelationships;

            if (!isRequiredContextParamsSet)
            {
                _localeMessage = _localeMessageBL.Get(GlobalizationHelper.GetSystemUILocale(), "111787", false, callerContext);
                diagnosticActivity.LogMessage(MessageClassEnum.Error, _localeMessage.Message);
                throw new MDMOperationException("111787", _localeMessage.Message, "EntityManager", String.Empty, "Get");
            }

            mainEntityContext.ContainerId = mainEntityContext.ContainerId < 1 ? entityUniqueIdentifier.ContainerId : mainEntityContext.ContainerId;
            mainEntityContext.EntityTypeId = mainEntityContext.EntityTypeId < 1 ? entityUniqueIdentifier.EntityTypeId : mainEntityContext.EntityTypeId;
            mainEntityContext.CategoryId = mainEntityContext.CategoryId < 1 ? entityUniqueIdentifier.CategoryId : mainEntityContext.CategoryId;

            // Hierarchy gets for Categories are not supported 
            if (mainEntityContext.EntityTypeId == Constants.CATEGORY_ENTITYTYPE)
            {
                throw new MDMOperationException("113900", "Failed to get entity with hierarchy as the requested entity type is a category which is not supported by GetEntityWithHierarchy API", "EntityHierarchyGetManager", String.Empty, "ValidateAndUpdateEntityContext");
            }

            EntityContextHelper.PopulateIdsInEntityContext(mainEntityContext, _entityManager, callerContext);

            ValidateAndUpdateEntityHierarchyContext(mainEntityContext, entityUniqueIdentifier, callerContext);

            EntityOperationsCommonUtility.ValidateAndUpdateEntityContextForLocales(mainEntityContext, callerContext.Application);

            if (_currentTraceSettings.IsBasicTracingEnabled)
            {
                diagnosticActivity.LogMessageWithData("Completed filling entity context with missing values", mainEntityContext.ToXml());
                diagnosticActivity.Stop();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mainEntityContext"></param>
        /// <param name="entityUniqueIdentifier"></param>
        /// <param name="callerContext"></param>
        private void ValidateAndUpdateEntityHierarchyContext(EntityContext mainEntityContext, EntityUniqueIdentifier entityUniqueIdentifier, CallerContext callerContext)
        {
            var diagnosticActivity = new DiagnosticActivity();

            if (_currentTraceSettings.IsBasicTracingEnabled)
            {
                diagnosticActivity.Start();
            }

            if (mainEntityContext.EntityHierarchyContext != null)
            {
                var childEntityContexts = (EntityContextCollection)mainEntityContext.EntityHierarchyContext.GetEntityContexts();

                if (childEntityContexts != null && childEntityContexts.Count > 0)
                {
                    //Set main entity context's LoadHierarchyRelationships = true if we have any record for entity contexts in the EntityHierarchyContext
                    mainEntityContext.LoadHierarchyRelationships = true;

                    #region Validate And Update Hierarchy Entity Contexts

                    var entityTypeIds = new Collection<Int32>();

                    foreach (EntityContext childEntityContext in childEntityContexts)
                    {
                        if (childEntityContext == null)
                        {
                            diagnosticActivity.LogMessage(MessageClassEnum.Error, "One of the EntityContext in EntityContextCollection is null or empty");
                            throw new MDMOperationException("113883", "One of the EntityContext in EntityContextCollection is null or empty", "EntityHierarchyGetManager", String.Empty, "ValidateAndUpdateEntityHierarchyContext");
                        }

                        childEntityContext.ContainerId = entityUniqueIdentifier.ContainerId;
                        childEntityContext.ContainerName = entityUniqueIdentifier.ContainerName;
                        childEntityContext.CategoryId = entityUniqueIdentifier.CategoryId;
                        childEntityContext.CategoryPath = entityUniqueIdentifier.CategoryPath;
                        //Note: do not copy EntityTypeId from entity identifier as it would be different for each child entity context

                        EntityContextHelper.PopulateIdsInEntityContext(childEntityContext, _entityManager, callerContext);

                        // Hierarchy gets for Categories are not supported 
                        if (childEntityContext.EntityTypeId == Constants.CATEGORY_ENTITYTYPE)
                        {
                            throw new MDMOperationException("113900", "Failed to get entity with hierarchy as the requested entity type is a category which is not supported by GetEntityWithHierarchy API", "EntityHierarchyGetManager", String.Empty, "ValidateInputParams");
                        }

                        // If multiple contexts are specified, Check if any entity type id is not set or if any duplicate exists
                        if ((childEntityContext.EntityTypeId <= 0) || (entityTypeIds.Contains(childEntityContext.EntityTypeId)))
                        {
                            diagnosticActivity.LogMessage(MessageClassEnum.Error, "Invalid entity types passed in EntityContext");
                            throw new MDMOperationException("113884", "Invalid entity types passed in EntityContext", "EntityHierarchyGetManager", String.Empty, "ValidateInputParams");
                        }

                        entityTypeIds.Add(childEntityContext.EntityTypeId);
                    }

                    #endregion Validate Entity Contexts
                }
            }

            if (_currentTraceSettings.IsBasicTracingEnabled)
            {
                diagnosticActivity.Stop();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mainEntityContext"></param>
        /// <param name="callerContext"></param>
        /// <param name="diagnosticActivity"></param>
        private void ValidateAndUpdateEntityExtensionContext(EntityContext mainEntityContext, CallerContext callerContext, DiagnosticActivity diagnosticActivity)
        {
            if (_currentTraceSettings.IsBasicTracingEnabled)
            {
                diagnosticActivity.Start();
            }

            if (mainEntityContext.EntityExtensionContext != null)
            {
                var childExtendedEntityContexts = (EntityContextCollection)mainEntityContext.EntityExtensionContext.GetEntityContexts();

                if (childExtendedEntityContexts != null && childExtendedEntityContexts.Count > 0)
                {
                    //Set main entity context's LoadExtensionRelationships = true if we have any record for entity contexts in the EntityExtensionContext
                    mainEntityContext.LoadExtensionRelationships = true;

                    #region Validate And Update Extension Entity Contexts

                    foreach (EntityContext childExtendedEntityContext in childExtendedEntityContexts)
                    {
                        if (childExtendedEntityContext == null)
                        {
                            throw new MDMOperationException("113883", "One of the EntityExtesionContext in EntityContextCollection is null or empty", "EntityHierarchyGetManager", String.Empty, "ValidateAndUpdateEntityHierarchyContext");
                        }

                        // Hierarchy gets for categories are not supported 
                        if (childExtendedEntityContext.EntityTypeId == Constants.CATEGORY_ENTITYTYPE)
                        {
                            throw new MDMOperationException("113900", "Failed to get entity with hierarchy as the requested entity type is a category which is not supported by GetEntityWithHierarchy API", "EntityHierarchyGetManager", String.Empty, "ValidateInputParams");
                        }
                    }

                    #endregion Validate Entity Contexts
                }
            }

            if (_currentTraceSettings.IsBasicTracingEnabled)
            {
                diagnosticActivity.Stop();
            }
        }

        #endregion Validation Methods

        #region Reference Data Management Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityId"></param>
        /// <param name="mainEntityContext"></param>
        /// <param name="entityGetOptions"></param>
        /// <param name="callerContext"></param>
        /// <returns></returns>
        private Dictionary<Int32, ReferenceDataBag> LoadReferenceDataBags(Int64 entityId, EntityContext mainEntityContext, EntityGetOptions entityGetOptions, CallerContext callerContext)
        {
            var referenceDataBags = new Dictionary<Int32, ReferenceDataBag>();

            var entityHierarchyContext = mainEntityContext.GetEntityHierarchyContext();
            var mainEntityTypeId = mainEntityContext.EntityTypeId;

            if (entityHierarchyContext != null)
            {
                var attributeModelManager = new AttributeModelBL();
                EntityOperationsBL entityOperationsBL = new EntityOperationsBL();

                var allEntityContexts = new EntityContextCollection { mainEntityContext };
                var providedEntityContexts = entityHierarchyContext.GetEntityDataContexts();

                Dictionary<Int32, Int32> entityTypeIdToVariantLevelMappings = entityOperationsBL.GetEntityVariantLevel(entityId, callerContext);

                if (providedEntityContexts != null && providedEntityContexts.Count > 0)
                {
                    allEntityContexts.AddRange(providedEntityContexts);
                }

                #region Identify entity types to load

                var allEntityTypes = _entityTypeManager.GetAll(callerContext);

                #region Initial Setup

                var entityTypesInContext = new EntityTypeCollection();
                Collection<Int32> parentEntityTypeIdList = new Collection<Int32>();
                Collection<Int32> childEntityTypeIdList = new Collection<Int32>();
                EntityTypeCollection childEntityTypes = null;
                EntityTypeCollection parentEntityTypes = null;
                Dictionary<Int32, Int32> variantLevelToEntityTypeIdMappings = new Dictionary<Int32, Int32>();
                Int32 currentVariantLevel = -1;

                if (entityTypeIdToVariantLevelMappings.ContainsKey(mainEntityTypeId))
                {
                    currentVariantLevel = entityTypeIdToVariantLevelMappings[mainEntityTypeId]; 
                }

                #endregion

                if (currentVariantLevel > 0)
                {
                    foreach (KeyValuePair<Int32, Int32> keyValuePair in entityTypeIdToVariantLevelMappings)
                    {
                        Int32 entityTypeId = keyValuePair.Key;
                        Int32 variantLevel = keyValuePair.Value;

                        variantLevelToEntityTypeIdMappings.Add(variantLevel, entityTypeId);

                        if (variantLevel < currentVariantLevel)
                        {
                            parentEntityTypeIdList.Add(entityTypeId);
                        }
                        else if (variantLevel > currentVariantLevel)
                        {
                            childEntityTypeIdList.Add(entityTypeId);
                        }
                    }
                }

                if (parentEntityTypeIdList.Count > 0)
                {
                    parentEntityTypes = allEntityTypes.Get(parentEntityTypeIdList);

                    if (parentEntityTypes != null && parentEntityTypes.Count > 0)
                    {
                        entityTypesInContext.AddRange(parentEntityTypes);
                    }
                }

                var currentEntityType = allEntityTypes.Get(mainEntityTypeId);

                if (currentEntityType != null)
                {
                    entityTypesInContext.Add(currentEntityType);
                }

                if (childEntityTypeIdList.Count > 0)
                {
                    childEntityTypes = allEntityTypes.Get(childEntityTypeIdList);

                    if (childEntityTypes != null && childEntityTypes.Count > 0)
                    {
                        entityTypesInContext.AddRange(childEntityTypes);
                    }
                }

                var requestedEntityTypeIds = allEntityContexts.GetEntityTypeIdList();

                var minVariantLevelRequested = -100;
                var maxVariantLevelRequested = -100;

                if (requestedEntityTypeIds != null && requestedEntityTypeIds.Count > 0)
                {
                    foreach (var entityType in entityTypesInContext)
                    {
                        if (entityType != null && requestedEntityTypeIds.Contains(entityType.Id))
                        {
                            Int32 variantLevel = entityTypeIdToVariantLevelMappings[entityType.Id];

                            if (minVariantLevelRequested == -100) // to setup startup variant level number
                            {
                                minVariantLevelRequested = maxVariantLevelRequested = variantLevel;
                            }
                            else
                            {
                                if (variantLevel <= minVariantLevelRequested)
                                {
                                    minVariantLevelRequested = variantLevel;
                                }
                                else if (variantLevel >= maxVariantLevelRequested)
                                {
                                    maxVariantLevelRequested = variantLevel;
                                }
                            }
                        }
                    }
                }

                #endregion

                #region Create reference data buckets for each entity type to be loaded

                if (minVariantLevelRequested > -1 && maxVariantLevelRequested > -1)
                {
                    for (var variantLevel = minVariantLevelRequested; variantLevel <= maxVariantLevelRequested; variantLevel++)
                    {
                        Int32 id = 0;
                        variantLevelToEntityTypeIdMappings.TryGetValue(variantLevel, out id);

                        var entityType = (EntityType)entityTypesInContext.Get(id);

                        if (entityType == null)
                        {
                            continue; // Write warning log here...this is WRONG..
                        }

                        var entityTypeId = entityType.Id;

                        EntityContext entityContext = (EntityContext)allEntityContexts.GetByEntityTypeId(entityTypeId) ?? new EntityContext { LoadEntityProperties = true, EntityTypeId = entityTypeId };

                        var attributeModels = new AttributeModelCollection();

                        entityContext.ContainerId = mainEntityContext.ContainerId;
                        entityContext.CategoryId = mainEntityContext.CategoryId;
                        entityContext.DataLocales = mainEntityContext.DataLocales;
                        entityContext.LoadStateValidationAttributes = mainEntityContext.LoadStateValidationAttributes;

                        if (entityContext.IsAttributeLoadingRequired)
                        {
                            attributeModels = EntityAttributeModelHelper.GetAttributeModels(0, entityContext, entityContext.DataLocales, entityGetOptions.ApplySecurity, true, null, attributeModelManager, _currentTraceSettings);
                        }

                        var referenceDataBag = new ReferenceDataBag
                        {
                            EntityTypeId = entityTypeId,
                            EntityType = entityType,
                            VariantLevel = variantLevel,
                            EntityContext = entityContext,
                            AttributeModels = attributeModels
                        };

                        referenceDataBags.Add(entityTypeId, referenceDataBag);
                    }
                }

                #endregion
            }

            return referenceDataBags;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="referenceDataBags"></param>
        /// <returns></returns>
        private IDictionary<Int32, AttributeModelCollection> GetEntityTypeIdsToAttributeModelsMaps(Dictionary<Int32, ReferenceDataBag> referenceDataBags)
        {
            var entityTypeAtttributeModelDictionary = new Dictionary<Int32, AttributeModelCollection>();

            foreach (var currentReferenceDataBag in referenceDataBags.Values)
            {
                foreach (var childReferenceDataBag in referenceDataBags.Values)
                {
                    if (childReferenceDataBag.VariantLevel > currentReferenceDataBag.VariantLevel)
                    {
                        CopyMissingChildEntityTypeAttributeModelsToParentEntityTypeAttributeModelsForInheritanceWalk(currentReferenceDataBag, childReferenceDataBag);
                    }
                }

                entityTypeAtttributeModelDictionary.Add(currentReferenceDataBag.EntityTypeId, currentReferenceDataBag.AttributeModels);
            }

            return entityTypeAtttributeModelDictionary;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parentReferenceDataBag"></param>
        /// <param name="childReferenceDataBag"></param>
        private void CopyMissingChildEntityTypeAttributeModelsToParentEntityTypeAttributeModelsForInheritanceWalk(ReferenceDataBag parentReferenceDataBag, ReferenceDataBag childReferenceDataBag)
        {
            var parentEntityAttributeModels = parentReferenceDataBag.AttributeModels;
            var childEntityAttributeModels = childReferenceDataBag.AttributeModels;

            var parentEntityAttributeInternalIds = parentEntityAttributeModels.GetAttributeInternalUniqueKeyList();

            foreach (AttributeModel childAttributeModel in childEntityAttributeModels)
            {
                Int32 childEntityAttributeInternalId = Attribute.GetInternalUniqueKey(childAttributeModel.Id, childAttributeModel.Locale);

                if (!parentEntityAttributeInternalIds.Contains(childEntityAttributeInternalId))
                {
                    var clonedChildEntityAttributeModel = (AttributeModel)childAttributeModel.Clone();

                    clonedChildEntityAttributeModel.ExtendedProperties = Constants.ATTRIBUTE_MODEL_ONLY_FOR_INHERITED_VALUE_CALCULATION;

                    parentEntityAttributeModels.Add(clonedChildEntityAttributeModel, true);
                    parentEntityAttributeInternalIds.Add(childEntityAttributeInternalId);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="referenceDataBags"></param>
        /// <returns></returns>
        private Dictionary<Int32, EntityType> GetFlattenEntityTypes(Dictionary<Int32, ReferenceDataBag> referenceDataBags)
        {
            Dictionary<Int32, EntityType> entityTypesForVariant = new Dictionary<Int32, EntityType>();

            foreach (KeyValuePair<Int32, ReferenceDataBag> referenceDataBag in referenceDataBags)
            {
                entityTypesForVariant.Add(referenceDataBag.Value.VariantLevel, referenceDataBag.Value.EntityType);
            }

            return entityTypesForVariant;
        }

        #endregion Create reference data dictionary

        #region Load Entities from DB

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityId"></param>
        /// <param name="mainEntityContext"></param>
        /// <param name="callerContext"></param>
        /// <param name="entityGetOptions"></param>
        /// <param name="referenceDataBags"></param>
        /// <returns></returns>
        private EntityCollection LoadEntities(Int64 entityId, EntityContext mainEntityContext, CallerContext callerContext, EntityGetOptions entityGetOptions, Dictionary<Int32, ReferenceDataBag> referenceDataBags)
        {
            var diagnosticActivty = new DiagnosticActivity();

            if (_currentTraceSettings.IsBasicTracingEnabled)
            {
                diagnosticActivty.Start();
            }

            LocaleEnum systemDataLocale = GlobalizationHelper.GetSystemDataLocale();

            // Create a temporary context for DB call
            EntityContext entityContextForDbCall = GetEntityContextForDBCall(mainEntityContext, systemDataLocale);

            var attributeModelsDictionary = GetEntityTypeIdsToAttributeModelsMaps(referenceDataBags);

            var entityTypes = GetFlattenEntityTypes(referenceDataBags);

            //Get command properties
            DBCommandProperties command = DBCommandHelper.Get(callerContext.Application, callerContext.Module, MDMCenterModuleAction.Read);

            var entityDA = new EntityDA();
            EntityCollection entityCollection = entityDA.BulkGetWithHierarchy(entityId, entityContextForDbCall, systemDataLocale, attributeModelsDictionary, entityTypes, command);

            if (mainEntityContext.LoadBlankAttributes)
            {
                LoadBlankAttributes(entityCollection, mainEntityContext, systemDataLocale, referenceDataBags);

                if (_currentTraceSettings.IsBasicTracingEnabled)
                {
                    diagnosticActivty.LogMessage(MessageClassEnum.Information, "Completed Loading blank attributes as LoadBlankAttributes flag is true");
                }
            }

            if (_currentTraceSettings.IsBasicTracingEnabled)
            {
                diagnosticActivty.Stop();
            }

            return entityCollection;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mainEntityContext"></param>
        /// <param name="systemDataLocale"></param>
        /// <returns></returns>
        private EntityContext GetEntityContextForDBCall(EntityContext mainEntityContext, LocaleEnum systemDataLocale)
        {
            var dataLocales = mainEntityContext.DataLocales;

            if (dataLocales.Count < 1)
            {
                dataLocales.Add(systemDataLocale);
            }

            var entityContextForDBCall = new EntityContext
            {
                ContainerId = mainEntityContext.ContainerId,
                EntityTypeId = mainEntityContext.EntityTypeId,
                CategoryId = mainEntityContext.CategoryId,
                Locale = mainEntityContext.Locale,
                LoadEntityProperties = true,
                LoadAttributes = true,
                LoadBusinessConditions = mainEntityContext.LoadBusinessConditions,
                LoadOnlyInheritedValues = mainEntityContext.LoadOnlyInheritedValues,
                LoadOnlyOverridenValues = mainEntityContext.LoadOnlyOverridenValues,
                LoadOnlyCurrentValues = mainEntityContext.LoadOnlyCurrentValues,
                LoadRelationships = false,
                LoadHierarchyRelationships = true,
                LoadComplexChildAttributes = mainEntityContext.LoadComplexChildAttributes,
                AttributeIdList = null, // Attribute ids would be set for each entity type in the DA and this is not going to be used so dont fill in
                DataLocales = dataLocales
            };

            return entityContextForDBCall;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityCollection"></param>
        /// <param name="entityContext"></param>
        /// <param name="systemDataLocale"></param>
        /// <param name="referenceDataBags"></param>
        private void LoadBlankAttributes(EntityCollection entityCollection, EntityContext entityContext, LocaleEnum systemDataLocale, Dictionary<Int32, ReferenceDataBag> referenceDataBags)
        {
            foreach (Entity entity in entityCollection)
            {
                Int32 entityTypeId = entity.EntityTypeId;

                var referenceDataBag = referenceDataBags[entityTypeId];

                var attrModelsTobeLoaded = referenceDataBag.AttributeModels;

                Int32 totalAttributesTobeFetched = attrModelsTobeLoaded.Count;

                Collection<Int32> attributeInternalKeyListInEntity = entity.Attributes.GetAttributeInternalUniqueKeyList();

                if (totalAttributesTobeFetched == attributeInternalKeyListInEntity.Count)
                {
                    continue;
                }

                //Add the attributes which are not having values..
                //DB call returns attributes only if they are having values
                //So, loop through all the attribute models and add attribute with empty value
                foreach (var attributeModel in attrModelsTobeLoaded)
                {
                    LocaleEnum attributeModelLocale = (attributeModel.IsLocalizable) ? attributeModel.Locale : systemDataLocale;

                    Int32 attributeInternalId = Attribute.GetInternalUniqueKey(attributeModel.Id, attributeModelLocale);

                    if (attributeInternalKeyListInEntity.Contains(attributeInternalId) || attributeModel.AttributeModelType == AttributeModelType.Relationship)
                    {
                        continue;
                    }

                    if (attributeModel.ExtendedProperties == Constants.ATTRIBUTE_MODEL_ONLY_FOR_INHERITED_VALUE_CALCULATION)
                    {
                        continue;
                    }

                    var attribute = new Attribute(attributeModel);

                    //If attribute having no value and is inheritable then set the default source flag to "I"
                    if (attributeModel.Inheritable)
                    {
                        attribute.SourceFlag = AttributeValueSource.Inherited;
                    }

                    attribute.Locale = attributeModelLocale;

                    if (entityContext.LoadOnlyOverridenValues || entityContext.LoadOnlyInheritedValues)
                    {
                        if (entityContext.LoadOnlyOverridenValues && attribute.SourceFlag == AttributeValueSource.Overridden)
                        {
                            entity.Attributes.Add(attribute);
                        }
                        else if (entityContext.LoadOnlyInheritedValues && attribute.SourceFlag == AttributeValueSource.Inherited)
                        {
                            entity.Attributes.Add(attribute);
                        }
                    }
                    else
                    {
                        entity.Attributes.Add(attribute);
                    }

                    entity.AttributeModels.Add(attributeModel);
                }
            }
        }

        #endregion Load Entities from DB

        #region Load Relationships

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityCollection"></param>
        /// <param name="mainEntityContext"></param>
        /// <returns></returns>
        private Collection<KeyValuePair<RelationshipContext, EntityCollection>> FilterEntitiesByRelationshipContexts(EntityCollection entityCollection, EntityContext mainEntityContext)
        {
            var relationshipContextToEntityCollectionMaps = new Collection<KeyValuePair<RelationshipContext, EntityCollection>>();

            var allEntityContexts = new EntityContextCollection { mainEntityContext };

            if (mainEntityContext.EntityHierarchyContext != null && mainEntityContext.EntityHierarchyContext.EntityContexts != null &&
                mainEntityContext.EntityHierarchyContext.EntityContexts.Count > 0)
            {
                allEntityContexts.AddRange(mainEntityContext.EntityHierarchyContext.GetEntityContexts());
            }

            if (allEntityContexts.Count > 0)
            {
                var entityTypeAndEntitiesDictionary = FilterEntitiesBasedOnEntityType(entityCollection);

                foreach (var keyValuePair in entityTypeAndEntitiesDictionary)
                {
                    EntityContext entityContext = (EntityContext)allEntityContexts.GetByEntityTypeId(keyValuePair.Key);

                    if (entityContext != null && entityContext.LoadRelationships)
                    {
                        var relationshipContext = entityContext.RelationshipContext;

                        if (relationshipContext != null)
                        {
                            if (relationshipContext.ContainerId <= 0)
                            {
                                relationshipContext.ContainerId = mainEntityContext.ContainerId;
                            }

                            relationshipContextToEntityCollectionMaps.Add(new KeyValuePair<RelationshipContext, EntityCollection>(relationshipContext, keyValuePair.Value));
                        }
                    }
                }
            }

            return relationshipContextToEntityCollectionMaps;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityCollection"></param>
        /// <returns></returns>
        private Dictionary<Int32, EntityCollection> FilterEntitiesBasedOnEntityType(EntityCollection entityCollection)
        {
            var entityTypeAndEntitiesDictionary = new Dictionary<Int32, EntityCollection>();

            foreach (Entity entity in entityCollection)
            {
                Int32 entityTypeId = entity.EntityTypeId;
                EntityCollection entitiesByEntityType;

                if (entityTypeAndEntitiesDictionary.TryGetValue(entityTypeId, out entitiesByEntityType))
                {
                    entitiesByEntityType.Add(entity);
                }
                else
                {
                    entitiesByEntityType = new EntityCollection { entity };
                    entityTypeAndEntitiesDictionary.Add(entityTypeId, entitiesByEntityType);
                }
            }

            return entityTypeAndEntitiesDictionary;
        }

        #endregion Load Relationships

        #region Build Hierarchy Relationships

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fromEntity"></param>
        /// <param name="allEntities"></param>
        /// <returns></returns>
        private HierarchyRelationshipCollection BuildHierarchyRelationshipsWithChildEntitiesReferenceTree(Entity fromEntity, EntityCollection allEntities)
        {
            HierarchyRelationshipCollection hierarchyRelationships = null;

            var childEntities = allEntities.GetEntitiesByParentEntityId(fromEntity.Id);

            if (childEntities != null && childEntities.Count > 0)
            {
                hierarchyRelationships = new HierarchyRelationshipCollection();

                foreach (var childEntity in childEntities)
                {
                    var childEntityId = childEntity.Id;

                    var hierarchyRelationship = new HierarchyRelationship
                    {
                        Path = String.Empty,
                        RelatedEntityId = childEntityId,
                        RelatedEntity = childEntity,
                        Direction = RelationshipDirection.Down,
                        Locale = childEntity.Locale,
                        Name = childEntity.Name
                    };

                    var childHierarchyRelationships = BuildHierarchyRelationshipsWithChildEntitiesReferenceTree(childEntity, allEntities);

                    if (childHierarchyRelationships != null && childHierarchyRelationships.Count > 0)
                    {
                        foreach (var childHierarchyRelationship in childHierarchyRelationships)
                        {
                            var clonedChildHierarchyRelationship = childHierarchyRelationship.Clone(false);
                            hierarchyRelationship.RelationshipCollection.Add(clonedChildHierarchyRelationship);
                        }
                    }

                    hierarchyRelationships.Add(hierarchyRelationship);
                }

                fromEntity.HierarchyRelationships = hierarchyRelationships;
            }

            return hierarchyRelationships;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="requestedEntity"></param>
        /// <param name="allEntities"></param>
        private HierarchyRelationship BuildHierarchyRelationshipWithParentEntityReferenceTree(Entity requestedEntity, EntityCollection allEntities)
        {
            HierarchyRelationship hierarchyRelationshipForParent = null;

            if (requestedEntity != null && requestedEntity.ParentEntityTypeId != Constants.CATEGORY_ENTITYTYPE)
            {
                var parentEntity = allEntities.GetEntity(requestedEntity.ParentEntityId) as Entity;

                if (parentEntity != null)
                {
                    hierarchyRelationshipForParent = new HierarchyRelationship
                    {
                        // TODO: What about Name, Path, Other properties 
                        Path = String.Empty,
                        RelatedEntityId = parentEntity.Id,
                        RelatedEntity = parentEntity,
                        Direction = RelationshipDirection.Up,
                        Locale = parentEntity.Locale,
                        Name = parentEntity.Name
                    };

                    requestedEntity.HierarchyRelationships.Add(hierarchyRelationshipForParent);

                    var parentHierarchyRelatinoship = BuildHierarchyRelationshipWithParentEntityReferenceTree(parentEntity, allEntities);

                    if (parentHierarchyRelatinoship != null)
                    {
                        hierarchyRelationshipForParent.RelationshipCollection.Add(parentHierarchyRelatinoship.Clone(false));
                    }
                }
            }

            return hierarchyRelationshipForParent;
        }

        #endregion Build Hierarchy Relationships

        #region Build Extension Relationships

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fromEntity"></param>
        /// <param name="allEntities"></param>
        /// <returns></returns>
        private ExtensionRelationshipCollection BuildExtensionRelationshipsWithChildEntitiesReferenceTree(Entity fromEntity, EntityCollection allEntities)
        {
            ExtensionRelationshipCollection extensionRelationships = null;

            var childEntities = allEntities.GetEntitiesByParentExtensionEntityId(fromEntity.Id);

            if (childEntities != null && childEntities.Count > 0)
            {
                extensionRelationships = new ExtensionRelationshipCollection();

                foreach (var childEntity in childEntities)
                {
                    var childEntityId = childEntity.Id;

                    var extensionRelationship = new ExtensionRelationship
                    {
                        Path = String.Empty,
                        RelatedEntityId = childEntityId,
                        RelatedEntity = childEntity,
                        Direction = RelationshipDirection.Down,
                        Locale = childEntity.Locale,
                        Name = childEntity.Name
                    };

                    var childExtensionRelationships = BuildExtensionRelationshipsWithChildEntitiesReferenceTree(childEntity, allEntities);

                    if (childExtensionRelationships != null && childExtensionRelationships.Count > 0)
                    {
                        foreach (var childExtensionRelationship in childExtensionRelationships)
                        {
                            var clonedChildExtensionRelationship = childExtensionRelationship.Clone(false);
                            extensionRelationship.RelationshipCollection.Add(clonedChildExtensionRelationship);
                        }
                    }

                    extensionRelationships.Add(extensionRelationship);
                }

                fromEntity.ExtensionRelationships = extensionRelationships;
            }

            return extensionRelationships;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="requestedEntity"></param>
        /// <param name="allEntities"></param>
        private ExtensionRelationship BuildExtensionRelationshipWithParentEntityReferenceTree(Entity requestedEntity, EntityCollection allEntities)
        {
            ExtensionRelationship extensionRelationshipForParent = null;

            if (requestedEntity != null && requestedEntity.ParentExtensionEntityId > 0)
            {
                var parentEntity = allEntities.GetEntity(requestedEntity.ParentExtensionEntityId) as Entity;

                if (parentEntity != null)
                {
                    extensionRelationshipForParent = new ExtensionRelationship
                    {
                        // TODO: What about Name, Path, Other properties 
                        Path = String.Empty,
                        RelatedEntityId = parentEntity.Id,
                        RelatedEntity = parentEntity,
                        Direction = RelationshipDirection.Up,
                        Locale = parentEntity.Locale,
                        Name = parentEntity.Name
                    };

                    requestedEntity.ExtensionRelationships.Add(extensionRelationshipForParent);

                    var parentHierarchyRelatinoship = BuildExtensionRelationshipWithParentEntityReferenceTree(parentEntity, allEntities);

                    if (parentHierarchyRelatinoship != null)
                    {
                        extensionRelationshipForParent.RelationshipCollection.Add(parentHierarchyRelatinoship.Clone(false));
                    }
                }
            }

            return extensionRelationshipForParent;
        }

        #endregion Build Extension Relationships

        #region GetChildEntities Internal's helper methods

        private void PopulateHierarchyContext(Int64 entityId, Int32 mainEntityTypeId, EntityContext mainEntityContext, Int32 requestedChildEntityTypeId, Boolean getRecursiveChildren, CallerContext callerContext)
        {
            #region Identify child entity types to be loaded

            var entityTypesToLoad = new EntityTypeCollection();

            EntityTypeCollection allEntityTypes = _entityTypeManager.GetAll(callerContext);

            EntityType mainEntityType = allEntityTypes.Get(mainEntityTypeId);

            if (mainEntityType != null)
            {
                if (requestedChildEntityTypeId > 0)
                {
                    var childEntityType = allEntityTypes.Get(requestedChildEntityTypeId);

                    if (childEntityType != null)
                    {
                        entityTypesToLoad.Add(childEntityType);
                    }
                }
                else
                {
                    EntityOperationsBL entityOperationsBL = new EntityOperationsBL();
                    Collection<Int32> childEntityTypeIdList = new Collection<Int32>();
                    EntityTypeCollection childEntityTypes = null;
                    Dictionary<Int32, Int32> entityTypeIdToVariantLevelMappings = entityOperationsBL.GetEntityVariantLevel(entityId, callerContext);
                    Int32 currentVariantLevel = entityTypeIdToVariantLevelMappings[mainEntityTypeId];

                    if (currentVariantLevel > 0)
                    {
                        Int32 entityTypeId = 0;
                        Int32 variantLevel = 0;

                        foreach (KeyValuePair<Int32, Int32> keyValuePair in entityTypeIdToVariantLevelMappings)
                        {
                            entityTypeId = keyValuePair.Key;
                            variantLevel = keyValuePair.Value;

                            if (getRecursiveChildren)
                            {
                                if (variantLevel > currentVariantLevel)
                                {
                                    childEntityTypeIdList.Add(entityTypeId);
                                }
                            }
                            else if (variantLevel == currentVariantLevel + 1)
                            {
                                childEntityTypeIdList.Add(entityTypeId);
                                break;
                            }
                        }

                        if (childEntityTypeIdList.Count > 0)
                        {
                            childEntityTypes = allEntityTypes.Get(childEntityTypeIdList);

                            if (childEntityTypes != null && childEntityTypes.Count > 0)
                            {
                                entityTypesToLoad.AddRange(childEntityTypes);
                            }
                        }
                    }
                }
            }

            #endregion

            #region Load EntityHierarchyContext object with all child entity type contexts

            if (entityTypesToLoad.Count > 0)
            {
                var childEntityContexts = new EntityContextCollection();

                foreach (var entityType in entityTypesToLoad)
                {
                    var childEntityContext = (EntityContext)mainEntityContext.Clone();
                    childEntityContext.EntityTypeId = entityType.Id;
                    childEntityContexts.Add(childEntityContext);
                }

                mainEntityContext.EntityHierarchyContext.EntityContexts = childEntityContexts;
            }

            #endregion
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="childEntities"></param>
        /// <param name="requestedChildEntityTypeId"></param>
        /// <param name="maxRecordsToReturn"></param>
        /// <param name="callerContext"></param>
        /// <returns></returns>
        private EntityCollection FilterChildEntitiesAsRequested(EntityCollection childEntities, Int32 requestedChildEntityTypeId, Int32 maxRecordsToReturn, CallerContext callerContext)
        {
            var traceSettings = MDMOperationContextHelper.GetCurrentTraceSettings();
            var diagnosticActivity = new DiagnosticActivity();

            if (traceSettings.IsTracingEnabled)
            {
                if (callerContext != null)
                {
                    var ec = new ExecutionContext { CallerContext = callerContext };
                    diagnosticActivity.Start(ec);
                }
                else
                {
                    diagnosticActivity.Start();
                }
            }

            if (childEntities != null && childEntities.Count > 0)
            {
                if (requestedChildEntityTypeId > 0)
                {
                    childEntities = (EntityCollection)childEntities.GetEntitiesByEntityTypeId(requestedChildEntityTypeId);
                }

                if (maxRecordsToReturn > 0 && childEntities.Count > maxRecordsToReturn)
                {
                    if (traceSettings.IsBasicTracingEnabled)
                    {
                        diagnosticActivity.LogDurationInfo("Filtering records : MaxRecordsToReturn : " + maxRecordsToReturn + ", Child Entities Count : " + childEntities.Count());
                    }

                    //TODO: This should be moved to the DB, it will be more efficient to do in DB than filtering through collection.
                    childEntities = new EntityCollection(childEntities.Take(maxRecordsToReturn).ToList());

                    if (traceSettings.IsBasicTracingEnabled)
                    {
                        diagnosticActivity.LogDurationInfo("Filtering records : Finished");
                    }
                }
            }

            if (traceSettings.IsTracingEnabled)
            {
                diagnosticActivity.Stop();
            }

            return childEntities;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hierachyRelatedContext"></param>
        /// <returns></returns>
        private Entity GetEntityHierarchy(Dictionary<String, Object> hierachyRelatedContext)
        {
            EntityUniqueIdentifier entityUniqueIdentifier = hierachyRelatedContext["EntityUniqueIdentifier"] as EntityUniqueIdentifier;
            EntityContext entityContext = hierachyRelatedContext["EntityContext"] as EntityContext;
            EntityGetOptions entityGetOptions = hierachyRelatedContext["EntityGetOptions"] as EntityGetOptions;
            CallerContext callerContext = hierachyRelatedContext["CallerContext"] as CallerContext;

            return GetEntityHierarchyMain(entityUniqueIdentifier, entityContext, entityGetOptions, callerContext);
        }

        #endregion

        #region Post Entity Load Operation

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mainEntity"></param>
        /// <param name="allHierarchyEntities"></param>
        /// <param name="mainEntityContext"></param>
        /// <param name="entityGetOptions"></param>
        /// <param name="callerContext"></param>
        private void PerfomPostLoadOperations(Entity mainEntity, EntityCollection allHierarchyEntities, EntityContext mainEntityContext, EntityGetOptions entityGetOptions, CallerContext callerContext)
        {
            var diagnosticActivity = new DiagnosticActivity();

            if (_currentTraceSettings.IsBasicTracingEnabled)
            {
                diagnosticActivity.Start();
            }

            //WE would load WF info only for main entity and not for related hierarchy entities.
            if (mainEntityContext.LoadWorkflowInformation && mainEntity != null)
            {
                _entityManager.LoadWorkflowDetails(mainEntity, callerContext);
            }

            foreach (Entity entity in allHierarchyEntities)
            {
                if (entityGetOptions.ApplyAVS)
                {
                    ApplyAVSOnEntity(entity, callerContext);

                    if (_currentTraceSettings.IsBasicTracingEnabled)
                    {
                        diagnosticActivity.LogMessage(MessageClassEnum.Information, "Completed applying AVS for entityId : " + entity.Id);
                    }
                }
            }

            if (String.IsNullOrWhiteSpace(callerContext.ProgramName))
            {
                callerContext.ProgramName = "EntityBL.GetEntity";
            }

            entityGetOptions.FillOptions.FillLookupDisplayValues = mainEntityContext.LoadLookupDisplayValues;
            entityGetOptions.FillOptions.FillLookupRowWithValues = mainEntityContext.LoadLookupRowWithValues;
            entityGetOptions.FillOptions.FillUOMValues = false; // As of now, fill attribute info just fills UOM names and which is already coming from SP so no need to fill that.

            EntityFillHelper.FillEntities(allHierarchyEntities, entityGetOptions.FillOptions, _entityManager, callerContext);

            if (_currentTraceSettings.IsBasicTracingEnabled)
            {
                diagnosticActivity.LogMessage(MessageClassEnum.Information, "Completed entity fill process");
            }

            UpdateAttributeModels(allHierarchyEntities, mainEntityContext, entityGetOptions.ApplyAVS, callerContext);

            if (_currentTraceSettings.IsBasicTracingEnabled)
            {
                diagnosticActivity.LogMessage(MessageClassEnum.Information, "Completed updating AttributeModels for entities");
                diagnosticActivity.Stop();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityCollection"></param>
        /// <param name="mainEntityContext"></param>
        /// <param name="applyAVS"></param>
        private void UpdateAttributeModels(EntityCollection entityCollection, EntityContext mainEntityContext, Boolean applyAVS, CallerContext callerContext)
        {
            Boolean loadAttributeModels = mainEntityContext.LoadAttributeModels;
            Boolean multipleLocalesRequested = (mainEntityContext.DataLocales.Count > 1);

            // Loop only if required
            if (!loadAttributeModels || multipleLocalesRequested || applyAVS)
            {
                foreach (Entity entity in entityCollection)
                {
                    var attributeModels = entity.AttributeModels;
                    var entityLocale = entity.Locale;
                    var isViewOnlyEntity = false;

                    if (applyAVS)
                    {
                        Collection<UserAction> permissionSet = entity.PermissionSet;
                        isViewOnlyEntity = (permissionSet != null && permissionSet.Count > 0 && permissionSet.Contains(UserAction.View) && !permissionSet.Contains(UserAction.Update));
                    }

                    if (attributeModels != null && attributeModels.Count > 0)
                    {
                        if (!loadAttributeModels)
                        {
                            attributeModels.Clear();
                        }
                        else
                        {
                            var filteredAttributeModels = new AttributeModelCollection();

                            foreach (AttributeModel attributeModel in attributeModels)
                            {
                                if (isViewOnlyEntity)
                                {
                                    attributeModel.ReadOnly = true;
                                }
                                if (multipleLocalesRequested && attributeModel.Locale == entityLocale && callerContext.Module == MDMCenterModules.UIProcess)
                                {
                                    filteredAttributeModels.Add(attributeModel, true);
                                }
                            }

                            //If attributes and their models are requested in multiple locales and calls comes from UI process 
                            //then and then given attribute models in entity locale.Hence added caller context module check.
                            //This is required to avoid duplicate attributes rendering on entity editor.
                            if (multipleLocalesRequested && callerContext.Module == MDMCenterModules.UIProcess)
                            {
                                entity.AttributeModels = filteredAttributeModels;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="callerContext"></param>
        private void ApplyAVSOnEntity(Entity entity, CallerContext callerContext)
        {
            var permissionBL = new PermissionBL();
            var entityOperationResult = new EntityOperationResult();

            Permission permission = permissionBL.GetValueBasedEntityPermission(entity, entityOperationResult, _entityManager, callerContext);

            if (permission != null && permission.PermissionSet != null && permission.PermissionSet.Count > 0)
            {
                //Copy all Permission set to entity object.This will useful for external user for verifying permission set at entity level rather than looping through
                //each attribute model collection.
                entity.PermissionSet = permission.PermissionSet;
            }
            else
            {
                Error error = entityOperationResult.Errors.FirstOrDefault();

                if (error != null)
                {
                    throw new MDMOperationException(error.ErrorCode, error.ErrorMessage, "EntityHierarchyGetManager", String.Empty, "GetEntity");
                }
            }
        }

        #endregion Post Entity Load Operation

        #region Helper Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mainEntityContext"></param>
        /// <param name="entityUniqueIdentifiers"></param>
        /// <param name="requestedEntityUniqueIdentifier"></param>
        private void ResolveEntityExtensionContexts(EntityContext mainEntityContext, EntityUniqueIdentifierCollection entityUniqueIdentifiers, EntityUniqueIdentifier requestedEntityUniqueIdentifier)
        {
            EntityExtensionContext entityExtensionContext = mainEntityContext.EntityExtensionContext;

            if (entityExtensionContext != null && entityExtensionContext.EntityContexts != null && entityExtensionContext.EntityContexts.Count > 0)
            {
                EntityContext firstEntityContext = entityExtensionContext.EntityContexts.FirstOrDefault();
                Boolean isAllExtensionsRequsted = false;
                Dictionary<Int32, EntityContextCollection> containerLevelToEntityContextsMaps = new Dictionary<Int32, EntityContextCollection>();

                foreach (EntityContext entityContext in entityExtensionContext.EntityContexts)
                {
                    EntityContext updatedEntityContext = null;

                    //User has requested based on container qualifier name where container name and category path both are empty.
                    if (String.IsNullOrWhiteSpace(firstEntityContext.ContainerName) && String.IsNullOrWhiteSpace(firstEntityContext.CategoryPath))
                    {
                        // TODO: Need to discuss exact name for 'all' scenario. It is just temporary name.
                        if (entityContext.ContainerQualifierName == "all")
                        {
                            foreach (EntityUniqueIdentifier entityUniqueIdentifier in entityUniqueIdentifiers)
                            {
                                updatedEntityContext = GetNewUpdatedEntityContext(entityContext, entityUniqueIdentifier);
                                AddOrUpdateEntityContextToDictionary(entityUniqueIdentifier.ContainerLevel, updatedEntityContext, containerLevelToEntityContextsMaps);
                            }

                            isAllExtensionsRequsted = true;
                        }
                        else
                        {
                            EntityUniqueIdentifierCollection filteredEntityUniqueIdentifiers = entityUniqueIdentifiers.GetByContainerQualifierName(entityContext.ContainerQualifierName);

                            if (filteredEntityUniqueIdentifiers != null && filteredEntityUniqueIdentifiers.Count > 0)
                            {
                                foreach (EntityUniqueIdentifier filteredEntityUniqueIdentifier in filteredEntityUniqueIdentifiers)
                                {
                                    updatedEntityContext = GetNewUpdatedEntityContext(entityContext, filteredEntityUniqueIdentifier);
                                    AddOrUpdateEntityContextToDictionary(filteredEntityUniqueIdentifier.ContainerLevel, updatedEntityContext, containerLevelToEntityContextsMaps);
                                }
                            }
                        }
                    }
                    else if (String.IsNullOrWhiteSpace(firstEntityContext.CategoryPath)) // user has requested based on only container name where category path is empty.
                    {
                        EntityUniqueIdentifierCollection filteredEntityUniqueIdentifiers = entityUniqueIdentifiers.GetByContainerName(entityContext.ContainerName);

                        if (filteredEntityUniqueIdentifiers != null && filteredEntityUniqueIdentifiers.Count > 0)
                        {
                            foreach (EntityUniqueIdentifier filteredEntityUniqueIdentifier in filteredEntityUniqueIdentifiers)
                            {
                                updatedEntityContext = GetNewUpdatedEntityContext(entityContext, filteredEntityUniqueIdentifier);
                                AddOrUpdateEntityContextToDictionary(filteredEntityUniqueIdentifier.ContainerLevel, updatedEntityContext, containerLevelToEntityContextsMaps);
                            }
                        }
                    }
                    else
                    {
                        EntityUniqueIdentifier filteredEntityUniqueIdentifier = entityUniqueIdentifiers.Get(entityContext.ContainerName, entityContext.CategoryPath);

                        if (filteredEntityUniqueIdentifier != null)
                        {
                            AddOrUpdateEntityContextToDictionary(filteredEntityUniqueIdentifier.ContainerLevel, entityContext, containerLevelToEntityContextsMaps);
                    }
                }
                }

                EntityContextCollection allEntityContexts = new EntityContextCollection();

                if (isAllExtensionsRequsted)
                {
                    foreach (KeyValuePair<Int32, EntityContextCollection> keyValuePair in containerLevelToEntityContextsMaps)
                    {
                        allEntityContexts.AddRange(keyValuePair.Value);
                    }
                }
                else
                {
                    EntityUniqueIdentifier filteredEntityUniqueIdentifier = entityUniqueIdentifiers.Get(requestedEntityUniqueIdentifier.EntityId);

                    if (filteredEntityUniqueIdentifier != null)
                    {
                        mainEntityContext.ContainerName = filteredEntityUniqueIdentifier.ContainerName;
                        mainEntityContext.CategoryPath = filteredEntityUniqueIdentifier.CategoryPath;

                        AddOrUpdateEntityContextToDictionary(filteredEntityUniqueIdentifier.ContainerLevel, mainEntityContext, containerLevelToEntityContextsMaps);
                    }

                    allEntityContexts = PopulateMissingContainersToBuildExtensionTree(containerLevelToEntityContextsMaps, entityUniqueIdentifiers, mainEntityContext);
                }

                //Override entity contexts over here.
                entityExtensionContext.EntityContexts = allEntityContexts;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityContext"></param>
        /// <param name="entityUniqueIdentifier"></param>
        /// <returns></returns>
        private EntityContext GetNewUpdatedEntityContext(EntityContext entityContext, EntityUniqueIdentifier entityUniqueIdentifier)
        {
            EntityContext clonedEntityContext = (EntityContext)entityContext.Clone();

            clonedEntityContext.EntityTypeId = entityUniqueIdentifier.EntityTypeId;

            clonedEntityContext.ContainerId = entityUniqueIdentifier.ContainerId;
            clonedEntityContext.ContainerName = entityUniqueIdentifier.ContainerName;

            clonedEntityContext.CategoryId = entityUniqueIdentifier.CategoryId;
            clonedEntityContext.CategoryPath = entityUniqueIdentifier.CategoryPath;

            return clonedEntityContext;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityContext"></param>
        /// <param name="entityUniqueIdentifier"></param>
        /// <param name="callerContext"></param>
        /// <param name="diagnosticActivity"></param>
        /// <returns></returns>
        private Dictionary<EntityUniqueIdentifier, EntityContext> GetEntityContextBasedUniqueIdentifier(EntityContext entityContext, EntityUniqueIdentifier entityUniqueIdentifier, CallerContext callerContext, DiagnosticActivity diagnosticActivity)
        {
            Dictionary<EntityUniqueIdentifier, EntityContext> entityContextBasedOnUniqueIdentifier = new Dictionary<EntityUniqueIdentifier, EntityContext>();

            if (entityContext.LoadExtensionRelationships)
            {
                //Resolve entity unique identifier for requested entity
                //EntityIdentificationManager.ResolveEntityUniqueIdentifier(entityUniqueIdentifier, callerContext, _currentTraceSettings);
                EntityExtensionContext entityExtensionContext = entityContext.GetEntityExtensionContext() as EntityExtensionContext;

                if (entityExtensionContext != null && entityExtensionContext.EntityContexts != null && entityExtensionContext.EntityContexts.Count > 0)
                {
                    EntityUniqueIdentifierCollection allEntityUniqueIdentifiers = _entityManager.GetEntityUniqueIdentifiers(entityUniqueIdentifier.EntityId, null, callerContext);

                    if (allEntityUniqueIdentifiers != null && allEntityUniqueIdentifiers.Count > 0)
                    {
                        ResolveEntityExtensionContexts(entityContext, allEntityUniqueIdentifiers, entityUniqueIdentifier);

                        foreach (EntityUniqueIdentifier currentEntityuniqueIdentifier in allEntityUniqueIdentifiers)
                        {
                            if (entityUniqueIdentifier.EntityId == currentEntityuniqueIdentifier.EntityId)
                            {
                                entityContextBasedOnUniqueIdentifier.Add(entityUniqueIdentifier, entityContext);
                            }
                            else
                            {
                                EntityContext extendedEntityContext = entityExtensionContext.GetEntityDataContext(currentEntityuniqueIdentifier.ContainerName, currentEntityuniqueIdentifier.CategoryPath) as EntityContext;

                                if (extendedEntityContext != null)
                                {
                                    extendedEntityContext.ContainerId = currentEntityuniqueIdentifier.ContainerId;
                                    extendedEntityContext.EntityTypeId = currentEntityuniqueIdentifier.EntityTypeId;
                                    extendedEntityContext.CategoryId = currentEntityuniqueIdentifier.CategoryId;

                                    entityContextBasedOnUniqueIdentifier.Add(currentEntityuniqueIdentifier, extendedEntityContext);
                                }
                            }
                        }
                    }
                }
            }

            return entityContextBasedOnUniqueIdentifier;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="containerLevel"></param>
        /// <param name="toBeAddedEntityContext"></param>
        /// <param name="containerLevelToEntityContextsMaps"></param>
        private void AddOrUpdateEntityContextToDictionary(Int32 containerLevel, EntityContext toBeAddedEntityContext, Dictionary<Int32, EntityContextCollection> containerLevelToEntityContextsMaps)
        {
            if (containerLevelToEntityContextsMaps != null)
            {
                EntityContextCollection entityContexts = null;

                containerLevelToEntityContextsMaps.TryGetValue(containerLevel, out entityContexts);

                if (entityContexts == null)
                {
                    entityContexts = new EntityContextCollection() { toBeAddedEntityContext };
                    containerLevelToEntityContextsMaps.Add(containerLevel, entityContexts);
                }
                else
                {
                    entityContexts.Add(toBeAddedEntityContext);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="containerLevelToEntityContextsMaps"></param>
        /// <param name="allEntityUniqueIdentifiers"></param>
        /// <param name="mainEntityContext"></param>
        /// <returns></returns>
        private EntityContextCollection PopulateMissingContainersToBuildExtensionTree(Dictionary<Int32, EntityContextCollection> containerLevelToEntityContextsMaps, EntityUniqueIdentifierCollection allEntityUniqueIdentifiers, EntityContext mainEntityContext)
        {
            EntityContextCollection allEntityContexts = new EntityContextCollection();

            var minContainerLevelRequested = -100;
            var maxContainerLevelRequested = -100;

            if (containerLevelToEntityContextsMaps != null && containerLevelToEntityContextsMaps.Count > 0)
            {
                minContainerLevelRequested = containerLevelToEntityContextsMaps.Keys.Min();
                maxContainerLevelRequested = containerLevelToEntityContextsMaps.Keys.Max();

                if (minContainerLevelRequested > -1 && maxContainerLevelRequested > -1)
                {
                    EntityHierarchyContext clonedEntityHierarchyContext = (EntityHierarchyContext)mainEntityContext.EntityHierarchyContext.Clone();

                    if (clonedEntityHierarchyContext != null && clonedEntityHierarchyContext.EntityContexts.Count > 0)
                    {
                        foreach (EntityContext entityContext in clonedEntityHierarchyContext.EntityContexts)
                        {
                            entityContext.LoadAttributes = false;
                            entityContext.LoadRelationships = false;
                        }
                    }

                    for (var containerLevel = minContainerLevelRequested; containerLevel <= maxContainerLevelRequested; containerLevel++)
                    {
                        EntityContextCollection entityContexts = null;
                        containerLevelToEntityContextsMaps.TryGetValue(containerLevel, out entityContexts);

                        if (entityContexts == null)
                        {
                            EntityUniqueIdentifierCollection filteredEntityUniqueIdentifiers = allEntityUniqueIdentifiers.Get(containerLevel);

                            if (filteredEntityUniqueIdentifiers != null && filteredEntityUniqueIdentifiers.Count > 0)
                            {
                                foreach (EntityUniqueIdentifier currentEntityUniqueIdentifier in filteredEntityUniqueIdentifiers)
                                {
                                    EntityContext defaultEntityContext = new EntityContext()
                                    {
                                        ContainerName = currentEntityUniqueIdentifier.ContainerName,
                                        CategoryPath = currentEntityUniqueIdentifier.CategoryPath,
                                        LoadAttributes = false, //This is default extension context to build extension tree. Hence no need to load attributes.
                                        LoadRelationships = false,
                                        EntityHierarchyContext = (EntityHierarchyContext)clonedEntityHierarchyContext.Clone()
                                    };

                                    allEntityContexts.Add(defaultEntityContext);
                                }
                            }
                        }
                        else
                        {
                            allEntityContexts.AddRange(entityContexts);
                        }
                    }
                }
            }

            return allEntityContexts;
        }

        #endregion Helper Methods

        #endregion Private Methods

        #region Private Classes

        /// <summary>
        /// 
        /// </summary>
        private class ReferenceDataBag
        {
            /// <summary>
            /// 
            /// </summary>
            public Int32 EntityTypeId;

            /// <summary>
            /// 
            /// </summary>
            public EntityType EntityType;

            /// <summary>
            /// 
            /// </summary>
            public Int32 VariantLevel;

            /// <summary>
            /// 
            /// </summary>
            public EntityContext EntityContext;

            /// <summary>
            /// 
            /// </summary>
            public AttributeModelCollection AttributeModels;
        }

        #endregion

        #endregion Methods
    }
}