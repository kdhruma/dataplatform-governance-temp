using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Collections.ObjectModel;

namespace MDM.EntityManager.Business
{
    using BusinessObjects;
    using BusinessObjects.Diagnostics;
    using ConfigurationManager.Business;
    using Core;
    using EntityManager.Business.EntityOperations.Helpers;
    using EntityManager.Data;
    using Utility;
    using RelationshipManager.Business;
    using Interfaces;
    using DataModelManager.Business;
    using ContainerManager.Business;

    /// <summary>
    /// Provides methods to process entity along with its hierarchy (entity as a whole) 
    /// </summary>
    public class EntityHierarchyProcessManager : BusinessLogicBase
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

        #endregion Fields

        #region Properties
        #endregion Properties

        #region Constructors

        /// <summary>
        /// 
        /// </summary>
        public EntityHierarchyProcessManager()
        {
            _securityPrincipal = SecurityPrincipalHelper.GetCurrentSecurityPrincipal();
            _currentTraceSettings = MDMOperationContextHelper.GetCurrentTraceSettings();
        }

        #endregion Constructors

        #region Methods

        #region Public Methods

        /// <summary>
        /// Processes the family entities by levels.
        /// </summary>
        /// <param name="allEntities">All entities.</param>
        /// <param name="entityOperationResults">The entity operation results.</param>
        /// <param name="entityProcessingOptions">Indicates entity processing options</param>
        /// <param name="entityFamilyChangeContext">Indicates entity family change context</param>
        /// <param name="callerContext">The caller context.</param>
        /// <param name="containerLevels">Indicates all containers</param>
        public void Process(EntityCollection allEntities, EntityOperationResultCollection entityOperationResults, EntityProcessingOptions entityProcessingOptions, EntityFamilyChangeContext entityFamilyChangeContext,CallerContext callerContext, ContainerCollection containers= null)
        {
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
                #region Step : Initial Setup

                Entity rootEntity = (Entity)allEntities.GetEntity(entityFamilyChangeContext.EntityFamilyId);

                if (containers == null)
                {
                    ContainerContext containerContext = new ContainerContext { ApplySecurity = false, LoadAttributes = false };
                    containers = new ContainerBL().GetAll(containerContext, callerContext);
                }

                //Get the container level dictionary
                Dictionary<Int32, ContainerCollection> containerLevels = containers.GetContainerHierarchyByLevels(rootEntity.ContainerId);

                #endregion Step : Initial Setup

                #region Step : Data Preparation

                //Prepare level dictionary
                Dictionary<Int32, Dictionary<Int32, EntityCollection>> levelBasedDictionary = new Dictionary<Int32, Dictionary<Int32, EntityCollection>>();

                //Prepare the entity batches based on container and entity type levels
                PopulateLevelBasedDictionary(rootEntity, allEntities, containerLevels, levelBasedDictionary, callerContext);

                if (isTracingEnabled)
                {
                    diagnosticActivity.LogDurationInfo("Completed preparing the dictionary of entity family level by level.");
                }

                #endregion Step : Data Preparation

                #region Step : Process Levels

                //Perform Process level by level
                foreach (var containerLevelItem in levelBasedDictionary)
                {
                    Dictionary<Int32, EntityCollection> containerLevelEntities = containerLevelItem.Value;

                    foreach (var entityTypeLevelEntitiesItem in containerLevelEntities)
                    {
                        EntityCollection currentLevelEntities = entityTypeLevelEntitiesItem.Value;

                        if (containerLevelItem.Key == Constants.CONTAINER_LEVEL_ONE && entityTypeLevelEntitiesItem.Key == Constants.VARIANT_LEVEL_ONE)
                        {
                            ProcessMain(currentLevelEntities, entityOperationResults, entityProcessingOptions, entityFamilyChangeContext, callerContext);
                        }
                        else
                        {
                            #region Id Updates

                            //Update the current level new entities with the parent ids which is saved in the previous level
                            foreach (Entity currentLevelEntity in currentLevelEntities)
                            {
                                //Update variant parent details
                                if (currentLevelEntity.Action == ObjectAction.Create && entityTypeLevelEntitiesItem.Key != Constants.VARIANT_LEVEL_ONE && entityTypeLevelEntitiesItem.Key != 100)
                                {
                                    //Get the parent entity type level entity
                                    String parentName = currentLevelEntity.ParentEntityName;
                                    String containerName = currentLevelEntity.ContainerName;

                                    //Get parent level entity from current container
                                    Int32 entityTypeId = entityTypeLevelEntitiesItem.Key - 1;

                                    if (containerLevelEntities.ContainsKey(entityTypeId))
                                    {
                                        Entity parentEntity = containerLevelEntities[entityTypeId].GetEntityByNameAndContainerName(parentName, containerName);
                                        currentLevelEntity.ParentEntityId = parentEntity.Id;
                                    }
                                }

                                //Update parent extension details
                                if (currentLevelEntity.Action == ObjectAction.Create && containerLevelItem.Key != Constants.CONTAINER_LEVEL_ONE)
                                {
                                    //Get the extension parent entity
                                    String parentExtensionEntityName = currentLevelEntity.ParentExtensionEntityName;
                                    String parentExtensionEntityCategoryPath = currentLevelEntity.ParentExtensionEntityCategoryPath;

                                    //Get parent container's same level entity by name and category path as extension can be based on different category
                                    Int32 containerId = containerLevelItem.Key - 1;
                                    Int32 entityTypeId = entityTypeLevelEntitiesItem.Key;

                                    if (levelBasedDictionary.ContainsKey(containerId) && levelBasedDictionary[containerId].ContainsKey(entityTypeId))
                                    {
                                        Entity parentExtensionEntity = levelBasedDictionary[containerId][entityTypeId].GetEntityByNameAndCategoryPath(parentExtensionEntityName, parentExtensionEntityCategoryPath);
                                        currentLevelEntity.ParentExtensionEntityId = parentExtensionEntity.Id;
                                    }
                                }
                            }

                            #endregion Id Updates

                            ProcessMain(currentLevelEntities, entityOperationResults, entityProcessingOptions, entityFamilyChangeContext, callerContext);
                        }
                    }
                    if (isTracingEnabled)
                    {
                        diagnosticActivity.LogDurationInfo(String.Format("Completed processing {0} variant levels for container level {1}", containerLevelEntities.Count, containerLevelItem.Key));
                    }
                }

                if (isTracingEnabled)
                {
                    diagnosticActivity.LogDurationInfo("Completed processing entire family level by level");
                }

                #endregion Step : Process Levels
            }
            finally
            {
                if (isTracingEnabled)
                {
                    diagnosticActivity.Stop();
                }
            }

        }

        #endregion Public Methods

        #region Private Methods

        #region Process Methods

        /// <summary>
        /// Process  entity hierarchy
        /// </summary>
        /// <param name="entity">Indicates entity having entire hierarchy to be processed</param>
        /// <param name="entity">Indicates entity operation result collection</param>
        /// <param name="entityProcessingOptions">Indicates entity processing options</param>
        /// <param name="entityFamilyChangeContext">Indicates entity family change context</param>
        /// <param name="callerContext">Indicates caller context</param>
        private void ProcessMain(EntityCollection entities, EntityOperationResultCollection entityOperationResults, EntityProcessingOptions entityProcessingOptions, EntityFamilyChangeContext entityFamilyChangeContext, CallerContext callerContext)
        {
            #region Step : Diagnostic Activity initialization

            DiagnosticActivity diagnosticActivity = new DiagnosticActivity();
            Boolean isTracingEnabled = _currentTraceSettings.IsBasicTracingEnabled;

            if (isTracingEnabled)
            {
                diagnosticActivity.Start();
            }

            #endregion Step : Diagnostic Activity initialization

            try
            {
                #region Step : Initial Setup

                Boolean isMasterRecord = false;
                Boolean hasRelationshipsChanged = false;
                Boolean hasEntitiesCreated = false;

                if (entityFamilyChangeContext != null)
                {
                    isMasterRecord = entityFamilyChangeContext.IsMasterCollaborationRecord;
                    hasRelationshipsChanged = entityFamilyChangeContext.HasRelationshipsChanged();
                    hasEntitiesCreated = entityFamilyChangeContext.HasEntitiesCreated();
                }

                CallerContext clonedCallerContext = (CallerContext)callerContext.Clone();
                clonedCallerContext.Module = MDMCenterModules.Denorm;

                #endregion Step : Initial Setup

                #region Step : Prepare Entity Operation Result Schema

                if (entityOperationResults == null)
                {
                    entityOperationResults = new EntityOperationResultCollection(entities);

                    if (isTracingEnabled)
                    {
                        diagnosticActivity.LogDurationInfo("Prepared entity operation results.");
                    }
                }

                #endregion Step : Prepare Entity Operation Result Schema

                #region Step : Remove all entities has action  = Read

                EntityCollection allEntities = new EntityCollection();
                EntityCollection entitiesWithReadAction = new EntityCollection();

                foreach (Entity entity in entities)
                {
                    ObjectAction entityAction = ObjectAction.Unknown;
                    if (entityFamilyChangeContext != null)
                    {
                        EntityChangeContext entityChangeContext = entityFamilyChangeContext.GetEntityChangeContext(entity.Id);

                        if (entityChangeContext != null)
                        {
                            entityAction = entityChangeContext.Action;
                        }
                    }

                    //if entity family change context has relationship changed or entities are created, allow entity process even-though there is no change.
                    //This is required to push relationship denorm changes to impacted entities.
                    if ((entity.Action != ObjectAction.Read && entity.Action != ObjectAction.Ignore) || hasRelationshipsChanged || entityAction == ObjectAction.Create)
                    {
                        //Refresh entity context based on data..do not trust input entity.EntityContext...
                        entity.EntityContext = new EntityContext(entity);
                        allEntities.Add(entity);
                    }
                    else
                    {
                        entitiesWithReadAction.Add(entity);
                    }
                }

                if (entitiesWithReadAction.Count > 0)
                {
                    LocaleMessage readEntityLocaleMessage = new LocaleMessage { Code = "114087", Message = "Entity has no changes, hence it has been removed from further processing.", MessageClass = MessageClassEnum.Warning };
                    EntityOperationsCommonUtility.UpdateEntityOperationResults(entitiesWithReadAction, entityOperationResults, readEntityLocaleMessage, readEntityLocaleMessage.Message);

                    foreach (Entity entityWithReadAction in entitiesWithReadAction)
                    {
                        allEntities.Remove(entityWithReadAction);
                    }
                }

                #endregion Step : Remove all entities has action  = Read

                #region Step : Return call if all entities has action = Read

                if (allEntities.Count < 1)
                {
                    if (isTracingEnabled)
                    {
                        diagnosticActivity.LogWarning("None of entities have action set to 'Create/Update/Delete'. Entity family processing would terminate now");
                    }

                    return;
                }

                #endregion

                #region Step : Initial Setup

                EntityValidationBL entityValidationManager = new EntityValidationBL();

                //Get AppConfig value and check whether validation is turned on
                Boolean validate = AppConfigurationHelper.GetAppConfig<Boolean>("MDMCenter.Entity.Process.Validate", true);

                //Get AppConfig which specify whether relationship inheritance feature is enabled or not.
                Boolean isRelationshipInheritanceEnabled = AppConfigurationHelper.GetAppConfig("RelationshipManager.Relationship.Inheritance.Enabled", true);

                LocaleEnum systemUILocale = GlobalizationHelper.GetSystemUILocale();
                String loginUser = _securityPrincipal.CurrentUserName;
                DBCommandProperties command = DBCommandHelper.Get(callerContext, MDMCenterModuleAction.Update);

                #endregion Step : Initial Setup

                #region Step : Validate Base properties of entity

                String operationAction = String.Empty;
                entityValidationManager.ValidateEntityProperties(allEntities, entityOperationResults, systemUILocale, operationAction, entityProcessingOptions.ResolveIdsByNames, callerContext);

                if (isTracingEnabled)
                {
                    diagnosticActivity.LogDurationInfo("Entities base properties validation completed.");
                }

                #endregion Step : Validate Base properties of entity

                #region Step : Fix collection attribute for sequencing

                foreach (Entity entity in allEntities)
                {
                    ReCalculateSequenceForCollectionAttributes(entity);
                }

                if (isTracingEnabled)
                {
                    diagnosticActivity.LogDurationInfo("Sequence recalculation for collection attributes completed.");
                }

                #endregion Step : Fix collection attribute for sequencing

                #region Step : Load attribute models

                try
                {
                    if (!entityProcessingOptions.ProcessOnlyEntities && !entityProcessingOptions.ProcessOnlyRelationships)
                    {
                        LoadAttributeModels(allEntities, entityProcessingOptions);
                    }
                }
                finally
                {
                    if (isTracingEnabled)
                    {
                        diagnosticActivity.LogDurationInfo("Loading entity attribute models completed.");
                    }
                }

                #endregion Step : Load attribute models

                #region Step : Populate default attribute values for new entities

                if (entityProcessingOptions.ProcessDefaultValues)
                {
                    try
                    {
                        EntityCollection entitiesForCreate = new EntityCollection();

                        foreach (Entity flattenEntity in allEntities)
                        {
                            if (flattenEntity.Action == ObjectAction.Create)
                            {
                                entitiesForCreate.Add(flattenEntity);
                            }
                        }

                        if (entitiesForCreate.Count > 0)
                        {
                            EntityAttributeDefaultValueHelper.PopulateDefaultAttributeValues(entitiesForCreate, entityOperationResults, false, null, _securityPrincipal.CurrentUserName);
                        }

                        entityProcessingOptions.ProcessDefaultValues = false;
                    }
                    finally
                    {
                        if (isTracingEnabled)
                        {
                            diagnosticActivity.LogDurationInfo("Populating default attribute values completed.");
                        }
                    }
                }

                #endregion Step : Populate default attribute values for new entities

                #region Step : Fill Entity

                if (!entityProcessingOptions.ProcessOnlyEntities)
                {
                    if (isTracingEnabled)
                    {
                        diagnosticActivity.LogInformation("Entity hierarchy fill process is started.");
                    }

                    try
                    {
                        EntityGetOptions.EntityFillOptions entityFillOptions = new EntityGetOptions.EntityFillOptions();
                        entityFillOptions.FillLookupRowWithValues = false;

                        EntityFillHelper.FillEntities(allEntities, entityFillOptions, new EntityBL(), callerContext);
                    }
                    finally
                    {
                        if (isTracingEnabled)
                        {
                            diagnosticActivity.LogDurationInfo("Entity hierarchy fill process is completed.");
                        }
                    }
                }

                #endregion Step : Fill Entity

                #region Step : Data validations

                if (isTracingEnabled)
                {
                    diagnosticActivity.LogInformation("Entity hierarchy data validation process is started.");
                }

                try
                {
                    if (entityProcessingOptions.ValidateEntities && validate)
                    {
                        entityValidationManager.Validate(allEntities, entityOperationResults, callerContext, true, entityProcessingOptions);
                    }

                    Boolean continueProcess = EntityOperationsHelper.ScanAndFilterEntitiesBasedOnResults(allEntities, null, entityOperationResults, callerContext);

                    if (!continueProcess)
                    {
                        if (isTracingEnabled)
                        {
                            diagnosticActivity.LogWarning("None of entities have passed validation process. Entity processing would terminate now.");
                        }

                        return;
                    }
                }
                finally
                {
                    if (isTracingEnabled)
                    {
                        diagnosticActivity.LogDurationInfo("Entity hierarchy data validation process is completed.");
                    }
                }

                #endregion Step : Data validations

                using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required, GetTransactionOptions(entityProcessingOptions.ProcessingMode)))
                {
                    if (isTracingEnabled)
                    {
                        diagnosticActivity.LogInformation("Entity hierarchy process transaction scope");
                    }

                    try
                    {
                        #region Step : Perform entity updates in database

                        if (isTracingEnabled)
                        {
                            diagnosticActivity.LogInformation("Starting entity family database process...");
                        }

                        if (allEntities.Count > 0)
                        {
                            EntityDA entityDA = new EntityDA();
                            entityDA.Process(allEntities, entityProcessingOptions, loginUser, GlobalizationHelper.GetSystemDataLocale(), callerContext.ProgramName, entityOperationResults, command, callerContext, true);

                            Boolean canContinue = EntityOperationsHelper.ScanAndFilterEntitiesBasedOnResults(allEntities, null, entityOperationResults, callerContext);

                            if (!canContinue)
                            {
                                return;
                            }
                        }
                        else
                        {
                            if (isTracingEnabled)
                            {
                                diagnosticActivity.LogWarning("Entity pre-processing has removed all the entities from process. Skipping EntityDA call, please check business rules if this is not expected.");
                            }
                        }

                        if (isTracingEnabled)
                        {
                            diagnosticActivity.LogDurationInfo("Entity hierarchy database process completed");
                        }

                        #endregion

                        if (isRelationshipInheritanceEnabled && isMasterRecord && (hasRelationshipsChanged || hasEntitiesCreated))
                        {
                            #region Step : Denormalized Relationships Get

                            RelationshipCollection denormRelsToBeProcessed = GetDenormalizedRelationships(allEntities, clonedCallerContext, diagnosticActivity);

                            #endregion Step : Denormalized Relationships Get

                            #region Step : Push Inherited Relationships

                            PushDenormalizedRelationships(allEntities, denormRelsToBeProcessed, entityOperationResults, clonedCallerContext, diagnosticActivity);

                            //Refresh operation results schema,pushing of de-normalized relationships is done.
                            entityOperationResults.RefreshOperationResultsSchema(allEntities);

                            #endregion Step : Push Inherited Relationships
                        }

                        #region Step : Perform Relationship Updates in database

                        if (isTracingEnabled)
                        {
                            diagnosticActivity.LogInformation("Starting entity family database process...");
                        }

                        ProcessRelationshipsByLevel(allEntities, entityOperationResults, entityProcessingOptions, clonedCallerContext);

                        if (isTracingEnabled)
                        {
                            diagnosticActivity.LogDurationInfo("Entity hierarchy database process completed");
                        }

                        #endregion Step : Perform Relationship Updates in database

                        #region Step : Commit transaction

                        if (isTracingEnabled)
                        {
                            diagnosticActivity.LogInformation("Starting entity hierarchy process transaction commit...");
                        }

                        //Commit transaction
                        transactionScope.Complete();

                        if (isTracingEnabled)
                        {
                            diagnosticActivity.LogDurationInfo("Entity hierarchy process transaction commit completed");
                        }

                        #endregion Step : Commit transaction
                    }
                    finally
                    {
                        if (isTracingEnabled)
                        {
                            diagnosticActivity.LogDurationInfo("Entity hierarchy process transaction scope completed.");
                        }
                    }
                }

                #region Step : Populate Entity Operation Result status

                EntityOperationsCommonUtility.UpdateEntityOperationResultStatus(allEntities, entityOperationResults, operationAction);

                if (isTracingEnabled)
                {
                    diagnosticActivity.LogDurationInfo("Entity operation result status update process completed.");
                }

                #endregion Step : Populate Entity Operation Result status
            }
            finally
            {
                if (isTracingEnabled)
                {
                    diagnosticActivity.LogDurationInfo("Entity hierarchy process completed.");
                    diagnosticActivity.Stop();
                }
            }
        }

        /// <summary>
        /// Populates the family level based dictionary.
        /// </summary>
        /// <param name="rootEntity">The root entity.</param>
        /// <param name="allEntities">All entities.</param>
        /// <param name="containerLevels">indicates containers hierarchy</param>
        /// <param name="levelBasedDictionary">The level based dictionary.</param>
        /// <param name="callerContext">The caller context.</param>
        private void PopulateLevelBasedDictionary(Entity rootEntity, EntityCollection allEntities, Dictionary<Int32, ContainerCollection> containerLevels, Dictionary<Int32, Dictionary<Int32, EntityCollection>> levelBasedDictionary, CallerContext callerContext)
        {
            //Get the Variant entity type level dictionary
            Dictionary<Int32, Int32> entityTypeIdToVariantLevelMappings = GetEntityTypeHierarchyLevels(rootEntity, callerContext);

            foreach (var containerLevel in containerLevels)
            {
                ContainerCollection levelContainers = containerLevel.Value;

                foreach (Container container in levelContainers)
                {
                    EntityCollection containerEntities = (EntityCollection)allEntities.GetEntitiesByContainerId(container.Id);

                    foreach (Entity entity in containerEntities)
                    {
                        Dictionary<Int32, EntityCollection> containerLevelDictionary = null;
                        if (!levelBasedDictionary.TryGetValue(containerLevel.Key, out containerLevelDictionary))
                        {
                            containerLevelDictionary = new Dictionary<Int32, EntityCollection>();
                            levelBasedDictionary.Add(containerLevel.Key, containerLevelDictionary);
                        }

                        EntityCollection containerLevelEntities = null;
                        Int32 entityLevel = 100; // This is that these entities are not following the EVD defined.

                        if (entityTypeIdToVariantLevelMappings.ContainsKey(entity.EntityTypeId))
                        {
                            entityLevel = entityTypeIdToVariantLevelMappings[entity.EntityTypeId];
                        }

                        if (!containerLevelDictionary.TryGetValue(entityLevel, out containerLevelEntities))
                        {
                            containerLevelEntities = new EntityCollection();
                            containerLevelDictionary.Add(entityLevel, containerLevelEntities);
                        }

                        containerLevelEntities.Add(entity);
                    }
                }
            }
        }

        /// <summary>
        /// Gets the entity type hierarchy levels.
        /// </summary>
        /// <param name="rootEntity">The root entity.</param>
        /// <param name="callerContext">The caller context.</param>
        /// <returns></returns>
        private Dictionary<Int32, Int32> GetEntityTypeHierarchyLevels(Entity rootEntity, CallerContext callerContext)
        {
            Dictionary<Int32, Int32> entityTypeIdToVariantLevelMappings = new Dictionary<Int32, Int32>();
            Int32 level = 0;

            EntityVariantDefinition variantDefinition = new EntityVariantDefinitionBL().GetByContext(rootEntity.ContainerId, rootEntity.CategoryId, rootEntity.EntityTypeId, callerContext);

            entityTypeIdToVariantLevelMappings.Add(rootEntity.EntityTypeId, 1);

            if (variantDefinition != null && variantDefinition.EntityVariantLevels != null && variantDefinition.EntityVariantLevels.Count > 0)
            {
                foreach (var variantLevel in variantDefinition.EntityVariantLevels)
                {
                    entityTypeIdToVariantLevelMappings.TryGetValue(variantLevel.EntityTypeId, out level);

                    // Only add value if variantLevel.EntityTypeId key is not present in dictionary.
                    if (level == 0)
                    {
                        entityTypeIdToVariantLevelMappings.Add(variantLevel.EntityTypeId, variantLevel.Rank + 1);
                    }
                }
            }

            return entityTypeIdToVariantLevelMappings;
        }

        #endregion Process Methods

        #region Helper Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entities"></param>
        private void LoadAttributeModels(EntityCollection entities, EntityProcessingOptions entityProcessingOptions)
        {
            Int32 key = 0;
            var currentTraceSettings = MDMOperationContextHelper.GetCurrentTraceSettings();
            Dictionary<Int32, AttributeModelCollection> containerEntityTypeAttributeModelMappings = new Dictionary<Int32, AttributeModelCollection>();

            foreach (Entity entity in entities)
            {
                if (entity.AttributeModels != null || entity.AttributeModels.Count > 0)
                {
                    continue;
                }

                EntityContext entityContext = entity.EntityContext;
                key = GetInternalKey(entityContext.ContainerId, entityContext.EntityTypeId);
                AttributeModelCollection contextBasedAttributeModels = null;

                containerEntityTypeAttributeModelMappings.TryGetValue(key, out contextBasedAttributeModels);

                if (contextBasedAttributeModels == null)
                {
                    contextBasedAttributeModels = EntityAttributeModelHelper.GetAttributeModels(entity.Id, entityContext, entityContext.DataLocales, false, false, entity, null, currentTraceSettings, entityProcessingOptions.ApplyDMS);
                    containerEntityTypeAttributeModelMappings.Add(key, contextBasedAttributeModels);
                }

                entity.AttributeModels = contextBasedAttributeModels;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        private void ReCalculateSequenceForCollectionAttributes(Entity entity)
        {
            foreach (Attribute attribute in entity.Attributes)
            {
                if (attribute.IsCollection)
                {
                    ValueCollection values = (ValueCollection)attribute.GetCurrentValuesInvariant();

                    //Re-sequence values..
                    IList<Value> sortedValues = values.OrderBy(v => v.Sequence).ToList();
                    bool isDeltaChange = false;

                    Int32 seq = 0;
                    foreach (Value val in sortedValues)
                    {
                        if (val.Action != ObjectAction.Delete && val.Action != ObjectAction.Ignore)
                        {
                            if (val.Sequence != seq)
                            {
                                val.Sequence = seq++;
                                // Set action to Update as sequence changed..
                                if (val.Action == ObjectAction.Read)
                                {
                                    isDeltaChange = true;
                                    val.Action = ObjectAction.Update;
                                }
                            }
                            else
                            {
                                seq++;
                            }
                        }
                    }

                    if (isDeltaChange)
                    {
                        attribute.Action = ObjectAction.Update;
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="containerId"></param>
        /// <param name="entitytypeId"></param>
        /// <returns></returns>
        private Int32 GetInternalKey(Int32 containerId, Int32 entitytypeId)
        {
            return (containerId << 7) + entitytypeId;
        }

        #endregion

        #region Relationship Level based Processing Methods

        /// <summary>
        /// Processes the relationships by level.
        /// </summary>
        /// <param name="allEntities">All entities.</param>
        /// <param name="callerContext">The caller context.</param>
        private void ProcessRelationshipsByLevel(EntityCollection allEntities, EntityOperationResultCollection entityOperationResults, EntityProcessingOptions entityProcessingOptions, CallerContext callerContext)
        {
            Dictionary<Int32, Dictionary<Int64, RelationshipCollection>> levelRelationships = new Dictionary<Int32, Dictionary<Int64, RelationshipCollection>>();

            #region Level Dictionary Preparation

            foreach (Entity entity in allEntities)
            {
                PopulateRelationshipBasedOnLevels(levelRelationships, entity.Relationships, entity.Id);
            }

            #endregion Level Dictionary Preparation

            #region Save Level by Level

            foreach (var levelRelationshipItem in levelRelationships)
            {
                Dictionary<Int64, RelationshipCollection> currentLevelRelationships = levelRelationshipItem.Value;

                RelationshipBL relationshipBL = new RelationshipBL();
                relationshipBL.Process(allEntities, entityOperationResults, null, callerContext, entityProcessingOptions, null, currentLevelRelationships, false);

                foreach (KeyValuePair<Int64, RelationshipCollection> currentLevelRelationship in currentLevelRelationships)
                {
                    RelationshipCollection relationships = currentLevelRelationship.Value;

                    if (relationships != null && relationships.Count > 0)
                    {
                        foreach (Relationship currentRelationship in relationships)
                        {
                            RelationshipCollection childRelationships = currentRelationship.RelationshipCollection;

                            if (childRelationships != null && childRelationships.Count > 0)
                            {
                                foreach (Relationship childRelationship in childRelationships)
                                {
                                    if (childRelationship.Action == ObjectAction.Create &&
                                        (childRelationship.InheritanceMode == InheritanceMode.Derived || childRelationship.InheritanceMode == InheritanceMode.InheritedDerived ||
                                        childRelationship.InheritanceMode == InheritanceMode.Inherited))
                                    {
                                        childRelationship.RelationshipParentId = currentRelationship.Id;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            #endregion Save Level by Level
        }

        /// <summary>
        /// Populates the child relationship levels.
        /// </summary>
        /// <param name="levelRelationships">The level relationships.</param>
        /// <param name="entityRelationshipCollection">The entity relationship collection.</param>
        /// <param name="entityId">The entity id</param>
        private void PopulateRelationshipBasedOnLevels(Dictionary<Int32, Dictionary<Int64, RelationshipCollection>> levelRelationships, RelationshipCollection relationships, Int64 entityId)
        {
            if (relationships != null && relationships.Count > 0)
            {
                foreach (Relationship relationship in relationships)
                {
                    if (relationship.FromEntityId < 1 && relationship.Level == Constants.RELATIONSHIP_LEVEL_ONE)
                    {
                        relationship.FromEntityId = entityId;
                    }

                    Dictionary<Int64, RelationshipCollection> currentLevelRelationships = null;

                    if (!levelRelationships.TryGetValue(relationship.Level, out currentLevelRelationships))
                    {
                        currentLevelRelationships = new Dictionary<Int64, RelationshipCollection>();
                        currentLevelRelationships.Add(entityId, new RelationshipCollection() { relationship });

                        levelRelationships.Add(relationship.Level, currentLevelRelationships);
                    }
                    else
                    {
                        RelationshipCollection entityRelationships = null;
                        currentLevelRelationships.TryGetValue(entityId, out entityRelationships);

                        if (entityRelationships == null)
                        {
                            currentLevelRelationships.Add(entityId, new RelationshipCollection() { relationship });
                        }
                        else
                        {
                            entityRelationships.Add(relationship);
                        }
                    }

                    //Recursive call
                    PopulateRelationshipBasedOnLevels(levelRelationships, relationship.RelationshipCollection, entityId);
                }
            }
        }

        #endregion Relationship Level based Processing Methods

        #region Relationships Denorm Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityId"></param>
        /// <param name="entities"></param>
        /// <param name="entityFamilyChangeContext"></param>
        /// <param name="callerContext"></param>
        /// <param name="diagnosticActivity"></param>
        /// <returns></returns>
        private RelationshipCollection GetDenormalizedRelationships(EntityCollection entities, CallerContext callerContext, DiagnosticActivity diagnosticActivity)
        {
            Boolean isTracingEnabled = _currentTraceSettings.IsBasicTracingEnabled;
            RelationshipCollection denormRelsToBeProcessed = null;

            try
            {
                if (isTracingEnabled)
                {
                    diagnosticActivity.Start();
                }

                if (entities != null && entities.Count > 0)
                {
                    Dictionary<Int64, Int32> entityContainerIds = new Dictionary<Int64, Int32>();
                    RelationshipDenormBL relationshipDenormBL = new RelationshipDenormBL();

                    foreach (Entity entity in entities)
                    {
                        entityContainerIds.Add(entity.Id, entity.ContainerId);
                    }

                    denormRelsToBeProcessed = relationshipDenormBL.GetDenormalizedRelationships(entityContainerIds, 0, 0, true, true, true, true, true, true, callerContext);
                }
            }
            finally
            {
                if (isTracingEnabled)
                {
                    diagnosticActivity.Stop();
                }
            }

            return denormRelsToBeProcessed;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="denormRelsToBeProcessed"></param>
        /// <param name="callerContext"></param>
        /// <param name="diagnosticActivity"></param>
        private void PushDenormalizedRelationships(EntityCollection entities, RelationshipCollection denormRelsToBeProcessed, EntityOperationResultCollection entityOperationResults, CallerContext callerContext, DiagnosticActivity diagnosticActivity)
        {
            Boolean isTracingEnabled = _currentTraceSettings.IsBasicTracingEnabled;

            try
            {
                if (isTracingEnabled)
                {
                    diagnosticActivity.Start();
                }

                if (denormRelsToBeProcessed != null && denormRelsToBeProcessed.Count > 0)
                {
                    Dictionary<Int32, AttributeModelCollection> masterAttributeModels = new Dictionary<Int32, AttributeModelCollection>();
                    Dictionary<Int64, RelationshipCollection> toBeProcessedRelationships = new Dictionary<Int64, RelationshipCollection>();

                    Int32 relationshipCreateId = -1;
                    EntityProcessingOptions entityProcessingOptions = new EntityProcessingOptions();

                    //Get AppConfig which specify whether relationship inheritance feature is enabled or not.
                    Boolean isRelationshipInheritanceEnabled = AppConfigurationHelper.GetAppConfig("RelationshipManager.Relationship.Inheritance.Enabled", true);

                    Boolean flushExistingValues = entityProcessingOptions.CollectionProcessingType != CollectionProcessingType.Merge;

                    StringComparison stringComparision = StringComparison.InvariantCulture;
                    entityProcessingOptions.IgnoreCase = !AppConfigurationHelper.GetAppConfig("MDMCenter.EntityManager.EntityProcessingOptions.ValueComparison.CaseSensitive.Enabled", true);

                    if (entityProcessingOptions.IgnoreCase)
                    {
                        stringComparision = StringComparison.InvariantCultureIgnoreCase;
                    }

                    foreach (Relationship denormalizedRelationship in denormRelsToBeProcessed)
                    {
                        if (denormalizedRelationship.RelationshipProcessingContext != null)
                        {
                            Int64 entityId = denormalizedRelationship.RelationshipProcessingContext.EntityId;
                            Int32 relationshipTypeId = denormalizedRelationship.RelationshipTypeId;

                            if (!entities.Contains(entityId))
                            {
                                continue;
                            }

                            Collection<LocaleEnum> dataLocales = denormalizedRelationship.RelationshipAttributes.GetLocaleList();

                            //Remove Unmapped relationship attributes
                            denormalizedRelationship.SetRelationshipAttributes(RemoveUnMappedRelationshipAttributes(denormalizedRelationship.RelationshipAttributes, masterAttributeModels, relationshipTypeId, denormalizedRelationship.ContainerId, dataLocales));

                            Entity entity = (Entity)entities.GetEntity(entityId);
                            EntityOperationResult entityOperationResult = (EntityOperationResult)entityOperationResults.GetByEntityId(entityId);

                            RelationshipBL relationshipBL = new RelationshipBL();
                            RelationshipCollection updatedRelationships = new RelationshipCollection() { denormalizedRelationship };
                            EntityOperationsCommonUtility.CleanseRelationships(updatedRelationships);

                            relationshipBL.CompareAndMerge(entity, updatedRelationships, entity.Relationships, ref relationshipCreateId, flushExistingValues, stringComparision, ProcessingMode.Async, entityOperationResult, callerContext, isRelationshipInheritanceEnabled);

                            RelationshipCollection relationships = null;

                            toBeProcessedRelationships.TryGetValue(entityId, out relationships);

                            if (relationships == null)
                            {
                                toBeProcessedRelationships.Add(entityId, new RelationshipCollection() { denormalizedRelationship });
                            }
                            else
                            {
                                relationships.Add(denormalizedRelationship);
                            }
                        }
                    }

                    foreach (Entity entity in entities)
                    {
                        RelationshipCollection relationships = null;

                        toBeProcessedRelationships.TryGetValue(entity.Id, out relationships);

                        if (relationships != null)
                        {
                            //clear existing relationships.
                            entity.Relationships.Clear();
                            entity.Relationships.AddRange(relationships);
                        }
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
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="relationshipAttributes"></param>
        /// <param name="masterAttributeModels"></param>
        /// <param name="relationshipTypeId"></param>
        /// <param name="containerId"></param>
        /// <param name="dataLocales"></param>
        /// <returns></returns>
        private AttributeCollection RemoveUnMappedRelationshipAttributes(AttributeCollection relationshipAttributes, Dictionary<Int32, AttributeModelCollection> masterAttributeModels, Int32 relationshipTypeId, Int32 containerId, Collection<LocaleEnum> dataLocales)
        {
            AttributeModelCollection attributeModels = null;
            AttributeCollection filteredRlationshipAttributes = new AttributeCollection();
            RelationshipBL relationshipBL = new RelationshipBL();

            if (relationshipAttributes != null && relationshipAttributes.Count() > 0)
            {
                if (masterAttributeModels.ContainsKey(relationshipTypeId))
                {
                    attributeModels = masterAttributeModels[relationshipTypeId];
                }
                else
                {
                    RelationshipContext relationshipContext = new RelationshipContext();
                    relationshipContext.RelationshipTypeIdList.Add(relationshipTypeId);
                    relationshipContext.ContainerId = containerId;
                    relationshipContext.DataLocales = dataLocales;

                    Dictionary<Int32, AttributeModelCollection> attributeModelsPerRT = relationshipBL.GetRelationshipAttributeModelsByContext(relationshipContext);

                    if (attributeModelsPerRT != null)
                    {
                        attributeModels = attributeModelsPerRT[relationshipTypeId];

                        if (attributeModels != null && attributeModels.Count > 0)
                        {
                            masterAttributeModels.Add(relationshipTypeId, attributeModels);
                        }
                    }
                }

                if (attributeModels != null && attributeModels.Count > 0)
                {
                    foreach (AttributeModel attributeModel in attributeModels)
                    {
                        IAttribute iRelationshipAttribute = relationshipAttributes.GetAttribute(attributeModel.Id, attributeModel.Locale);

                        if (iRelationshipAttribute != null)
                        {
                            filteredRlationshipAttributes.Add(iRelationshipAttribute);
                        }
                    }
                }
            }

            return filteredRlationshipAttributes;
        }

        #endregion Relationships Denorm Methods

        #endregion Private Methods

        #endregion Methods
    }
}