using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Transactions;

namespace MDM.EntityManager.Business
{
    using MDM.BusinessObjects;
    using MDM.BusinessObjects.Diagnostics;
    using MDM.ConfigurationManager.Business;
    using MDM.ContainerManager.Business;
    using MDM.Core;
    using MDM.Core.Exceptions;
    using MDM.DataModelManager.Business;
    using MDM.EntityManager.Business.EntityOperations;
    using MDM.EntityManager.Business.EntityOperations.Helpers;
    using MDM.EntityManager.Data;
    using MDM.Interfaces;
    using MDM.MessageBrokerManager;
    using MDM.MessageManager.Business;
    using MDM.RelationshipManager.Business;
    using MDM.Utility;

    /// <summary>
    /// Represents business logic for Entity state validation  management  
    /// </summary>
    public class EntityStateValidationBL : BusinessLogicBase, IEntityStateValidationManager
    {
        #region Fields

        /// <summary>
        /// Specifies security principal for user.
        /// </summary>
        private SecurityPrincipal _securityPrincipal = null;

        /// <summary>
        /// Field denoting localeMessageBL
        /// </summary>
        private LocaleMessageBL _localeMessageBL = new LocaleMessageBL();

        /// <summary>
        /// Field denoting locale message
        /// </summary>
        private LocaleMessage _localeMessage = null;

        /// <summary>
        /// Indicates current trace settings
        /// </summary>
        private TraceSettings _currentTraceSettings = null;

        /// <summary>
        /// Field denoting system UI locale
        /// </summary>
        private LocaleEnum _systemUILocale = LocaleEnum.UnKnown;

        #endregion

        #region Properties
        #endregion

        #region Constructors

        /// <summary>
        /// Instantiate Entity Validation State BL
        /// </summary>
        public EntityStateValidationBL()
        {
            _currentTraceSettings = MDMOperationContextHelper.GetCurrentTraceSettings();
            _systemUILocale = GlobalizationHelper.GetSystemUILocale();

            this.GetSecurityPrincipal();
        }

        #endregion

        #region  Methods

        #region Public Methods

        #region StateValidation Methods

        /// <summary>
        /// Gets the state of the entity validation for single or family entities
        /// </summary>
        /// <param name="entityIds">Indicates the entity ids</param>
        /// <param name="callerContext">Indicates Application and Module name by which action is being performed</param> 
        /// <returns>Returns</returns>
        public EntityStateValidationCollection Get(Collection<Int64> entityIds, CallerContext callerContext, Boolean needGlobalFamilyErrors = false, Boolean needVariantFamilyErrors = false)
        {
            Boolean isTracingEnabled = _currentTraceSettings.IsBasicTracingEnabled;
            EntityStateValidationCollection entityStateValidations = null;
            DiagnosticActivity diagnosticActivity = new DiagnosticActivity();

            try
            {
                if (isTracingEnabled)
                {
                    diagnosticActivity.Start();
                }

                #region Validations

                if (isTracingEnabled)
                {
                    diagnosticActivity.LogInformation("Parameters validation is started.");
                }

                this.ValidateEntityIds(entityIds, callerContext, "Get");

                if (isTracingEnabled)
                {
                    diagnosticActivity.LogInformation("Parameters validation is completed.");
                }

                #endregion Validations

                if (isTracingEnabled)
                {
                    diagnosticActivity.LogInformation("Para:: CallerContext : Application - " + callerContext.Application + " Module - " + callerContext.Module);
                    diagnosticActivity.LogInformation("Para:: Fetching entity Ids - " + String.Join(",", entityIds));
                    diagnosticActivity.LogInformation("Para:: needGlobalFamilyErrors - " + needGlobalFamilyErrors);
                    diagnosticActivity.LogInformation("Para:: needVariantFamilyErrors - " + needVariantFamilyErrors);
                }

                DBCommandProperties command = DBCommandHelper.Get(callerContext, MDMCenterModuleAction.Read);
                entityStateValidations = new EntityStateValidationDA().Get(entityIds, needGlobalFamilyErrors, needVariantFamilyErrors, command);
            }
            finally
            {
                if (isTracingEnabled)
                {
                    diagnosticActivity.Stop();
                }
            }

            return entityStateValidations;
        }

        /// <summary>
        /// Gets entity validation score for all requested entity ids
        /// </summary>
        /// <param name="entityIdList">Indicates entity ids.</param>
        /// <param name="callerContext">Indicates Application and Module name by which action is being performed</param> 
        /// <returns>Returns EntityValidatioonScore collection</returns>
        public EntityStateValidationScoreCollection GetEntityStateValidationScores(Collection<Int64> entityIdList, CallerContext callerContext)
        {
            EntityStateValidationScoreCollection entityStateValidationScores = null;
            Boolean isTracingEnabled = _currentTraceSettings.IsBasicTracingEnabled;
            DiagnosticActivity diagnosticActivity = new DiagnosticActivity();

            try
            {
                if (isTracingEnabled)
                {
                    diagnosticActivity.Start();
                }

                #region Validations

                if (isTracingEnabled)
                {
                    diagnosticActivity.LogInformation("Parameters validation is started.");
                }

                this.ValidateEntityIds(entityIdList, callerContext, "GetEntityStateValidationScores");

                PopulateDefaultProgramName(callerContext, "GetEntityStateValidationScores");

                if (isTracingEnabled)
                {
                    diagnosticActivity.LogInformation("Parameters validation is completed.");
                }

                #endregion Validations

                DBCommandProperties command = DBCommandHelper.Get(callerContext, MDMCenterModuleAction.Read);
                EntityStateValidationDA entityValidationStateDA = new EntityStateValidationDA();

                entityStateValidationScores = entityValidationStateDA.GetEntityStateValidationScores(entityIdList, command);
            }
            finally
            {
                if (isTracingEnabled)
                {
                    diagnosticActivity.Stop();
                }
            }

            return entityStateValidationScores;
        }

        /// <summary>
        /// Gets entity business conditions based on given entity ids.
        /// </summary>
        /// <param name="entityIdList">Indicates the entity ids</param>
        /// <param name="callerContext">Indicates Application and Module name by which action is being performed</param> 
        /// <returns>Returns EntityBusinessConditionCollection</returns>
        public EntityBusinessConditionCollection GetEntityBusinessConditions(Collection<Int64> entityIdList, CallerContext callerContext)
        {
            Boolean isTracingEnabled = _currentTraceSettings.IsBasicTracingEnabled;
            DiagnosticActivity diagnosticActivity = new DiagnosticActivity();
            EntityBusinessConditionCollection entitiesBusinessConditions = null;

            try
            {
                if (isTracingEnabled)
                {
                    diagnosticActivity.Start();
                }

                #region Validations

                this.ValidateEntityIds(entityIdList, callerContext, "GetEntityBusinessConditions");

                if (isTracingEnabled)
                {
                    diagnosticActivity.LogInformation("Parameters validation is completed.");
                }

                #endregion Validations

                DBCommandProperties command = DBCommandHelper.Get(callerContext, MDMCenterModuleAction.Read);
                EntityStateValidationDA entityStateValidationDA = new EntityStateValidationDA();
                entitiesBusinessConditions = entityStateValidationDA.GetEntityBusinessConditions(entityIdList, command);

                //TODO : Fill business condition name in object
            }
            finally
            {
                if (isTracingEnabled)
                {
                    diagnosticActivity.LogDurationInfo("Execution is completed for EntityStateValidationBL.GetEntityBusinessConditions");
                    diagnosticActivity.Stop();
                }
            }

            return entitiesBusinessConditions;
        }

        /// <summary>
        /// Processes the state of the entity validation.
        /// </summary>
        /// <param name="entities">Indicates the entity collection.</param>
        /// <param name="entityOperationResults">Indicates entity operation results.</param>
        /// <param name="callerContext">Indicates Application and Module name by which action is being performed</param>
        public void Process(EntityCollection entities, EntityOperationResultCollection entityOperationResults, CallerContext callerContext)
        {
            Boolean isTracingEnabled = _currentTraceSettings.IsBasicTracingEnabled;
            DiagnosticActivity diagnosticActivity = new DiagnosticActivity();
            EntityStateValidationCollection deltaEntityStateValidations = null;
            try
            {
                if (isTracingEnabled)
                {
                    diagnosticActivity.Start();
                }

                #region Prepare Entity State Validation collection from Entity Operation Result

                deltaEntityStateValidations = EntityStateValidationHelper.GetEntityStateValidations(entities, entityOperationResults, callerContext, _currentTraceSettings);
                entityOperationResults.RefreshOperationResultStatus();
                
                #endregion STEP: Prepare Entity Validation State collection from Entity Operation Result

                if (deltaEntityStateValidations != null && deltaEntityStateValidations.Count > 0)
                {
                    #region Step : Load State Validation attributes

                    EntityStateValidationHelper.LoadStateValidationAttributes(entities);

                    #region Diagnostics and tracing

                    if (_currentTraceSettings.IsBasicTracingEnabled)
                    {
                        diagnosticActivity.LogDurationInfo("Loaded state validation attributes into entities.");
                    }

                    #endregion

                    #endregion Step : Load Validation States attributes

                    #region Calculate Entity Validation State

                    this.CalculateStateValidations(entities, deltaEntityStateValidations, callerContext);

                    #endregion Calculate Entity Validaiton State

                    #region Update entity validation state collection and validation states to database

                    this.ProcessInternal(entities, deltaEntityStateValidations, entityOperationResults, callerContext);

                    #endregion Update entity validation state collection and validation states to database
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
        /// Processes the entity validation states for system exception.
        /// </summary>
        /// <param name="entities">Indicates the entity collection.</param>
        /// <param name="entityOperationResults">Indicates entity operation results.</param>
        /// <param name="callerContext">Indicates Application and Module name by which action is being performed</param>
        public void ProcessForSystemException(EntityCollection entities, EntityOperationResultCollection entityOperationResults, CallerContext callerContext)
        {
            Boolean isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsTracingEnabled; ;
            DiagnosticActivity diagnosticActivity = new DiagnosticActivity();
            EntityStateValidationCollection entityStateValidation = null;

            try
            {
                if (isTracingEnabled)
                {
                    diagnosticActivity.Start();
                }

                if (entities != null && entities.Count > 0)
                {
                    entityStateValidation = new EntityStateValidationCollection();
                    foreach (Entity entity in entities)
                    {
                        //We shouldn't record entity validation states for categories as per design. Hence skipping them over here.
                        if (entity.EntityTypeId == Constants.CATEGORY_ENTITYTYPE)
                        {
                            continue;
                        }

                        //Get entity operation result             
                        EntityOperationResult entityOperationResult = entityOperationResults.SingleOrDefault(eor => eor.EntityId == entity.Id);

                        if (entityOperationResult != null && entityOperationResult.HasError)
                        {
                            Func<Error, Boolean> condition = (error) => error.ReasonType == ReasonType.SystemError;
                            ErrorCollection systemErrorEntityOperationResult = this.GetAll<ErrorCollection, Error>(entityOperationResult.Errors, condition);

                            if (systemErrorEntityOperationResult != null && systemErrorEntityOperationResult.Count > 0)
                            {
                                // Since it is a system exception for entity, we should set its metadata as invalid.
                                entity.ValidationStates.IsMetaDataValid = ValidityStateValue.InValid;

                                foreach (Error error in systemErrorEntityOperationResult)
                                {
                                    EntityStateValidation entityValidationState = new EntityStateValidation
                                    {
                                        SystemValidationStateAttribute = SystemAttributes.EntityMetaDataValid,
                                        EntityId = entity.Id,
                                        ContainerId = entity.ContainerId,
                                        AttributeId = (Int32)SystemAttributes.EntityMetaDataValid,
                                        AttributeModelType = AttributeModelType.Unknown,
                                        ReasonType = ReasonType.SystemError,
                                        Action = ObjectAction.Create,
                                        MessageCode = error.ErrorCode,
                                        MessageParameters = error.Params,
                                        Locale = entity.Locale,
                                        OperationResultType = OperationResultType.Error
                                    };

                                    entityStateValidation.Add(entityValidationState);
                                }
                            }

                        }

                        if (entityStateValidation.Count > 0)
                        {
                            this.ProcessInternal(entities, entityStateValidation, entityOperationResults, callerContext);
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
        /// Processes the validation state of the entity families.
        /// </summary>
        /// <param name="rootEntityFamilies">Indicates the entity family collection.</param>
        /// <param name="entityOperationResults">Indicates entity operation results.</param>
        /// <param name="callerContext">Indicates Application and Module name by which action is being performed</param>
        public void ProcessEntityFamilyStates(EntityCollection rootEntityFamilies, EntityOperationResultCollection entityOperationResults, CallerContext callerContext)
        {
            Boolean isTracingEnabled = _currentTraceSettings.IsBasicTracingEnabled;
            DiagnosticActivity diagnosticActivity = new DiagnosticActivity();
            EntityCollection toBeProcessedEntities = null;
            EntityStateValidationCollection toBeProcessedEntitiesStateValidation = null;
            try
            {
                if (isTracingEnabled)
                {
                    diagnosticActivity.Start();
                }

                if (rootEntityFamilies == null || rootEntityFamilies.Count == 0)
                {
                    return;
                }

                toBeProcessedEntities = new EntityCollection();
                toBeProcessedEntitiesStateValidation = new EntityStateValidationCollection();
                HashSet<Int64> inValidParentExtensionIds = new HashSet<Int64>();

                //NOTE: Lowest leaf entity family should be processed first             
                foreach (Entity rootEntityFamily in rootEntityFamilies.Reverse())
                {
                    // Flatten Entity family in this structure  Dictionary<HierarchyLevel, Dictionary<EntityID,EntityObj>>
                    Dictionary<Int16, Dictionary<Int64, Entity>> flattenEntityFamily = FlattenEntityFamily(rootEntityFamily);

                    #region STEP: Get Entities from  flatten structure

                    EntityCollection entities = GetEntityCollectionFromFlattenEntityFamily(flattenEntityFamily);

                    #endregion STEP: Get Entities from  flatten structure

                    #region STEP: Prepare delta Entity Validation State collection from Entity Operation Result

                    EntityStateValidationCollection deltaEntityValidationStateCollection = EntityStateValidationHelper.GetEntityStateValidations(entities, entityOperationResults, callerContext, _currentTraceSettings, false);
                    entityOperationResults.RefreshOperationResultStatus();

                    #endregion STEP: Prepare delta Entity Validation State collection from Entity Operation Result

                    #region STEP: Calculate and Refresh Entity Validation State

                    this.CalculateStateValidations(entities, deltaEntityValidationStateCollection, callerContext);

                    #endregion STEP: Calculate and Refresh Entity Validaiton State

                    #region STEP: RollUp Entity Variants State

                    this.RollUpEntityVariantsAndExtensionsStates(rootEntityFamily, flattenEntityFamily, inValidParentExtensionIds);

                    #endregion STEP: RollUp Entity Variants State

                    #region STEP: Add processed entities and validation states to Master Collection

                    toBeProcessedEntities.AddRange(entities);
                    toBeProcessedEntitiesStateValidation.AddRange(deltaEntityValidationStateCollection);

                    #endregion STEP: Add processed entities and validation states to Master Collection
                }

                #region Update entity validation state collection and validation states to database

                this.ProcessInternal(toBeProcessedEntities, toBeProcessedEntitiesStateValidation, entityOperationResults, callerContext);

                #endregion Update entity validation state collection and validation states to database
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
        /// Gets lock type for given entity global family id list
        /// </summary>
        /// <param name="entityGlobalFamilyIdList">Indicates entity global family id list to check lock type</param>
        /// <param name="callerContext">Indicates caller context</param>
        /// <returns>Returns dictionary pointing to lock type for each requested entity global family id</returns>
        public Dictionary<Int64, LockType> GetEntityLocks(Collection<Int64> entityGlobalFamilyIdList, CallerContext callerContext)
        {
            #region Initial Setup

            Boolean isTracingEnabled = _currentTraceSettings.IsBasicTracingEnabled;
            DiagnosticActivity diagnosticActivity = new DiagnosticActivity();
            Dictionary<Int64, LockType> lockTypeForEntityFamilies = null;

            #endregion Initial Setup

            try
            {
                if (isTracingEnabled)
                {
                    diagnosticActivity.Start();
                }

                #region Validations

                this.ValidateEntityIds(entityGlobalFamilyIdList, callerContext, "AreEntitiesLocked");

                if (isTracingEnabled)
                {
                    diagnosticActivity.LogInformation("Parameters validation is completed.");
                }

                #endregion Validations

                DBCommandProperties command = DBCommandHelper.Get(callerContext, MDMCenterModuleAction.Read);
                EntityStateValidationDA entityStateValidationDA = new EntityStateValidationDA();
                lockTypeForEntityFamilies = entityStateValidationDA.GetEntityLocks(entityGlobalFamilyIdList, command);
            }
            finally
            {
                if (isTracingEnabled)
                {
                    diagnosticActivity.LogDurationInfo("Execution is completed for EntityStateValidationBL.AreEntitiesLocked");
                    diagnosticActivity.Stop();
                }
            }

            return lockTypeForEntityFamilies;
        }

        #endregion StateValidation Methods

        #region Revalidate Methods

        /// <summary>
        /// Revalidates state for all entities in family
        /// </summary>
        /// <param name="entityFamilyQueue">Indicates entity family queue</param>
        /// <param name="callerContext">Indicates caller context</param>
        /// <returns>Returns entity operation result collection</returns>
        public EntityOperationResultCollection Revalidate(EntityFamilyQueue entityFamilyQueue, CallerContext callerContext)
        {
            EntityOperationResultCollection entityOperationResults = null;

            if (entityFamilyQueue != null && entityFamilyQueue.RevalidateContext != null && entityFamilyQueue.RevalidateContext.RevalidateMode == RevalidateMode.Delta)
            {
                entityOperationResults = DeltaRevalidate(entityFamilyQueue, callerContext);
            }
            else
            {
                entityOperationResults = FullRevalidate(entityFamilyQueue, callerContext);
            }

            return entityOperationResults;
        }

        /// <summary>
        /// Enqueue for re-validate process for given entity family queue
        /// </summary>
        /// <param name="entityFamilyQueue">Indicates entity family queue for which re-validation process is required</param>
        /// <param name="callerContext">Indicates application and module name of caller</param>
        /// <returns>Returns operation result of current operation</returns>
        public OperationResult EnqueueForRevalidate(EntityFamilyQueue entityFamilyQueue, CallerContext callerContext)
        {
            DiagnosticActivity diagnosticActivity = new DiagnosticActivity();
            Boolean isTracingEnabled = _currentTraceSettings.IsBasicTracingEnabled;
            OperationResult operationResult = new OperationResult();
            callerContext = callerContext ?? new CallerContext(MDMCenterApplication.MDMCenter, MDMCenterModules.Entity, "EntityValidationStateBL.EnqueueForRevalidate");

            try
            {
                if (isTracingEnabled)
                {
                    diagnosticActivity.Start();
                }

                #region Step : Data Validations

                ValidateInputParameters(entityFamilyQueue, operationResult, callerContext);

                Int64 entityGlobalFamilyId = entityFamilyQueue.EntityGlobalFamilyId;

                #endregion Step : Data Validations

                #region Step : Validate lock status for entities

                if (operationResult.OperationResultStatus == OperationResultStatusEnum.Successful || operationResult.OperationResultStatus == OperationResultStatusEnum.None)
                {
                    Dictionary<Int64, LockType> entitiesLockStatus = null;

                    if (entityGlobalFamilyId > 0)
                    {
                        entitiesLockStatus = new EntityStateValidationBL().GetEntityLocks(new Collection<Int64>() { entityGlobalFamilyId }, callerContext);
                    }

                    if (entitiesLockStatus != null && entitiesLockStatus.Count > 0)
                    {
                        LockType lockType = LockType.Unknown;
                        entitiesLockStatus.TryGetValue(entityGlobalFamilyId, out lockType);

                        if (lockType == LockType.Promote)
                        {
                            _localeMessage = _localeMessageBL.Get(_systemUILocale, "114344", false, callerContext); //Entity Family Queue is not available or empty.
                            operationResult.AddOperationResult(_localeMessage.Code, _localeMessage.Message, OperationResultType.Error);
                            operationResult.OperationResultStatus = OperationResultStatusEnum.Failed;
                        }
                    }
                }

                #endregion Step : Validate lock status for entities

                #region Step : Enqueue for Re-validate

                if (operationResult.OperationResultStatus == OperationResultStatusEnum.Successful || operationResult.OperationResultStatus == OperationResultStatusEnum.None)
                {
                    entityFamilyQueue.EntityActivityList = EntityActivityList.EntityRevalidate;
                    entityFamilyQueue.Action = ObjectAction.Create;
                    //EntityFamilyQueueBL entityFamilyQueueBL = new EntityFamilyQueueBL();

                    //operationResult = entityFamilyQueueBL.Process(entityFamilyQueue, callerContext);
                }

                #endregion Step : Enqueue for Re-validate
            }
            finally
            {
                if (isTracingEnabled)
                {
                    diagnosticActivity.Stop();
                }
            }

            return operationResult;
        }

        #endregion Revalidate Methods

        #endregion Public Methods

        #region Private Methods

        #region Roll Up Methods

        /// <summary>
        /// Roll up the variant and extension states of the entity.
        /// </summary>
        /// <param name="rootEntity">Indicates the root entity of Entity Family</param>
        /// <param name="flattenEntityFamily">Indicates the flatten entity family.</param>
        /// <param name="inValidParentExtensionIds">Indicates the inValid parent extension ids.</param>
        private void RollUpEntityVariantsAndExtensionsStates(Entity rootEntity, Dictionary<Int16, Dictionary<Int64, Entity>> flattenEntityFamily, HashSet<Int64> inValidParentExtensionIds)
        {
            Dictionary<Int64, Entity>.ValueCollection hierarcheyLevelEntityCollection = new Dictionary<Int64, Entity>.ValueCollection(new Dictionary<Int64, Entity>());

            // Start from leaf level , lets say , start from SKU level if  it is (Style-Color-SKU) scenario 
            for (Int16 i = (Int16)(flattenEntityFamily.Count); i > 0; i--)
            {
                Int16 currentHierarchyLevel = i;
                // Get lowest hierarchy level from flatten structure , lets say - first get all SKU from entity family , then all color and all Style subsequently 

                if (flattenEntityFamily.ContainsKey(currentHierarchyLevel))
                {
                    hierarcheyLevelEntityCollection = flattenEntityFamily[currentHierarchyLevel].Values;

                    #region  Group all children entity by their Parent , lets say : group all SKU of white color (parent), group all SKU of green color

                    IEnumerable<IGrouping<Int64, Entity>> childEntityGroupByParent = from entity in hierarcheyLevelEntityCollection
                                                                                     group entity by entity.ParentEntityId into ParentEntityGroup
                                                                                     select ParentEntityGroup;

                    #endregion Group all children entity by their Parent , lets say : group all SKU of white color (parent), group all SKU of green color (parent)

                    #region Loop all children along with their parent, lets say : Loop each (all SKU of white color , all SKU of green color,...)

                    foreach (IGrouping<Int64, Entity> childEntityWithParent in childEntityGroupByParent)
                    {
                        Int64 entityParentId = childEntityWithParent.Key;
                        Int16 parentHierarchyLevel = (Int16)(i - 1);

                        // Get parent entity reference from flatten structure of Entity family.
                        Entity parentEntity = this.GetEntityFromFlattenEntityFamily(entityParentId, parentHierarchyLevel, flattenEntityFamily);

                        if (parentEntity != null)
                        {
                            Boolean isParentEntityVariantInValid = false;

                            //Setting parent Entity variant as valid 
                            SetEntityValidationStates(parentEntity, SystemAttributes.EntityVariantValid, ValidityStateValue.Valid);

                            // Loop each children of parent, lets say - loop all children SKU of white color 
                            foreach (Entity childEntity in childEntityWithParent)
                            {
                                if (childEntity.HierarchyRelationships.Count < 1)
                                {
                                    //Setting Entity variant as valid 
                                    SetEntityValidationStates(childEntity, SystemAttributes.EntityVariantValid, ValidityStateValue.Valid);
                                }

                                #region Roll up Entity Variant States

                                if (!isParentEntityVariantInValid)
                                {
                                    if (childEntity.ValidationStates.IsSelfValid == ValidityStateValue.InValid || childEntity.ValidationStates.IsEntityVariantValid == ValidityStateValue.InValid)
                                    {
                                        // set Entity variant is in valid for parent entity.
                                        SetEntityValidationStates(parentEntity, SystemAttributes.EntityVariantValid, ValidityStateValue.InValid);
                                        isParentEntityVariantInValid = true;
                                    }
                                }

                                #endregion Roll up Entity Variant States

                                #region Roll up Entity Extension States

                                CalculateEntityExtensionsStates(inValidParentExtensionIds, childEntity);

                                #endregion Roll up Entity Extension States
                            }
                        }
                        else
                        {
                            #region STEP: Process Top Level Entity, if Entity doesn't have parent, then it is top level entity, lets say - Style doesn't have any parent.

                            foreach (Entity topLevelEntity in childEntityWithParent)
                            {
                                // Get entity reference from flatten structure of Entity family.
                                Entity entity = this.GetEntityFromFlattenEntityFamily(topLevelEntity.Id, currentHierarchyLevel, flattenEntityFamily);

                                if (entity != null)
                                {
                                    if (entity.HierarchyRelationships.Count < 1)
                                    {
                                        //Setting Entity variant as valid 
                                        SetEntityValidationStates(entity, SystemAttributes.EntityVariantValid, ValidityStateValue.Valid);
                                    }

                                    #region Roll up Entity Extension States

                                    this.CalculateEntityExtensionsStates(inValidParentExtensionIds, entity);

                                    #endregion Roll up Entity Extension States

                                }


                            }

                            #endregion STEP: Process Top Level Entity
                        }
                    }

                    #endregion Loop all children along with their parent, lets say : Loop each (all SKU of white color , all SKU of green color,...)
                }
            }
        }

        /// <summary>
        /// Calculates the entity extensions states.
        /// </summary>
        /// <param name="inValidParentExtensionIds">Defines the inValid parent extension ids.</param>
        /// <param name="entity">Defines the entity.</param>
        private void CalculateEntityExtensionsStates(HashSet<Int64> inValidParentExtensionIds, Entity entity)
        {
            // Check if entity exists in InValidParentExtension Collection, then make entity extension is InValid.
            ValidityStateValue ValidityStateValue = (inValidParentExtensionIds.Contains(entity.Id)) ? ValidityStateValue.InValid : ValidityStateValue.Valid;
            SetEntityValidationStates(entity, SystemAttributes.EntityExtensionsValid, ValidityStateValue);

            //Check if current entity has extension parent.
            if (entity.ParentExtensionEntityId > 0)
            {
                if (entity.ValidationStates.IsSelfValid == ValidityStateValue.InValid || entity.ValidationStates.IsExtensionsValid == ValidityStateValue.InValid)
                {
                    // Add Parent Extension Id in InValidParentExtension Collection, 
                    inValidParentExtensionIds.Add(entity.ParentExtensionEntityId);
                }
            }
        }

        /// <summary>
        /// Flattens the entity family.
        /// </summary>
        /// <param name="rootEntity">Indicates the root entity of Entity Family.</param>
        /// <returns>Returns flatten data structure of Entity Family </returns>
        private Dictionary<Int16, Dictionary<Int64, Entity>> FlattenEntityFamily(Entity rootEntity)
        {
            // Flatten Entity family in this structure  Dictionary<HierarchyLevel, Dictionary<EntityID,EntityObj>>
            Dictionary<Int16, Dictionary<Int64, Entity>> flattenEntityFamily = new Dictionary<Int16, Dictionary<Int64, Entity>>();

            Stack<Entity> stackEntity = new Stack<Entity>(new[] { rootEntity });
            while (stackEntity.Count > 0)
            {
                Entity entity = stackEntity.Pop();

                //add data to dictionary 
                Dictionary<Int64, Entity> enityCollection = null;

                if (!flattenEntityFamily.TryGetValue(entity.HierarchyLevel, out enityCollection))
                {
                    enityCollection = new Dictionary<Int64, Entity>();
                    flattenEntityFamily.Add(entity.HierarchyLevel, enityCollection);
                }

                if (!enityCollection.ContainsKey(entity.Id))
                {
                    enityCollection.Add(entity.Id, entity);
                }

                foreach (HierarchyRelationship hierarchyRelationship in entity.HierarchyRelationships)
                {
                    if (hierarchyRelationship.RelatedEntity != null)
                    {
                        stackEntity.Push(hierarchyRelationship.RelatedEntity);
                    }
                }
            }

            return flattenEntityFamily;
        }

        /// <summary>
        /// Gets the entity from flatten entity family.
        /// </summary>
        /// <param name="entityId">Indicates the entity identifier.</param>
        /// <param name="hierarchyLevel">Indicates the hierarchy level.</param>
        /// <param name="flattenEntityFamily">Indicates the flatten data structure of entity family.</param>
        /// <returns>Returns entity from flatten data structure of entity family.</returns>
        private Entity GetEntityFromFlattenEntityFamily(Int64 entityId, Int16 hierarchyLevel, Dictionary<Int16, Dictionary<Int64, Entity>> flattenEntityFamily)
        {
            Entity returnEntity = null;
            if (entityId == 0 || hierarchyLevel == 0 || flattenEntityFamily == null || flattenEntityFamily.Count == 0)
            {
                return returnEntity;
            }

            Dictionary<Int64, Entity> entityCollection = null;

            if (flattenEntityFamily.TryGetValue(hierarchyLevel, out entityCollection))
            {
                if (entityCollection.TryGetValue(entityId, out returnEntity))
                {
                    // entity exists
                    return returnEntity;
                }
            }

            // entity doesn't exist 
            return returnEntity;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="flattenEntityFamily"></param>
        /// <returns></returns>
        private EntityCollection GetEntityCollectionFromFlattenEntityFamily(Dictionary<Int16, Dictionary<Int64, Entity>> flattenEntityFamily)
        {
            EntityCollection entityCollection = new EntityCollection();

            if (flattenEntityFamily != null && flattenEntityFamily.Count > 0)
            {
                foreach (Dictionary<Int64, Entity> entities in flattenEntityFamily.Values)
                {
                    entityCollection.Add(entities.Values.ToList<Entity>());
                }
            }

            return entityCollection;
        }

        #endregion Roll Up Methods

        #region Process Method

        /// <summary>
        /// Calculates the validation state for entity.
        /// </summary>
        /// <param name="entity">Indicates the entity.</param>
        /// <param name="deltaEntityValidationStates">Indicates the delta entity validation states.</param>
        /// <param name="originalEntityValidationStates">Indicates the original entity validation states.</param>
        /// <param name="validationStateAttribute">Indicates the validation state attribute.</param>
        private void CalculateValidationStateForEntity(Entity entity, EntityStateValidationCollection deltaEntityValidationStates, EntityStateValidationCollection originalEntityValidationStates, SystemAttributes validationStateAttribute)
        {
            Boolean hasError = false;
            ValidityStateValue ValidityStateValue = ValidityStateValue.NotChecked;
            Func<EntityStateValidation, Boolean> condition = null;

            #region Check if delta validation state collection has any new error record for  validation state system attribute

            condition = (entityValidationState) => entityValidationState.Action == ObjectAction.Create &&
                                                entityValidationState.EntityId == entity.Id &&
                                                entityValidationState.OperationResultType == OperationResultType.Error &&
                                                entityValidationState.SystemValidationStateAttribute == validationStateAttribute;

            hasError = this.DoesItemExist(deltaEntityValidationStates, condition);

            #endregion Check if delta validation state collection has any error record for validation state system attribute

            if (!hasError)
            {
                #region Check if original validation state collection has any error record for  validation state system attribute

                // if meta data validation has passed then no need to check database error collection for metadata related errors.
                if (validationStateAttribute != SystemAttributes.EntityMetaDataValid)
                {
                    if (originalEntityValidationStates != null && originalEntityValidationStates.Count > 0)
                    {
                        hasError = this.DoesErrorExist(entity, deltaEntityValidationStates, originalEntityValidationStates, validationStateAttribute);
                    }
                }

                #endregion Check if original validation state collection has any error record for validation state system attribute
            }

            ValidityStateValue = hasError ? ValidityStateValue.InValid : ValidityStateValue.Valid;
            this.SetEntityValidationStates(entity, validationStateAttribute, ValidityStateValue);
        }

        /// <summary>
        /// Does the error exist.
        /// </summary>
        /// <param name="entity">Indicates the entity.</param>
        /// <param name="deltaEntityValidationStates">Indicates the delta entity validation states.</param>
        /// <param name="originalEntityValidationStates">Indicates the original entity validation states.</param>
        /// <param name="validationStateAttribute">Indicates the validation state attribute.</param>
        /// <returns>Returns true if it find any error records in collection otherwise return false </returns>
        private Boolean DoesErrorExist(Entity entity, EntityStateValidationCollection deltaEntityValidationStates, EntityStateValidationCollection originalEntityValidationStates, SystemAttributes validationStateAttribute)
        {
            Boolean doesErrorExist = false;

            Func<EntityStateValidation, Boolean> condition = (entityValidationState) => entityValidationState.Action == ObjectAction.Delete &&
                                                                                         entityValidationState.EntityId == entity.Id &&
                                                                                         entityValidationState.OperationResultType == OperationResultType.Error &&
                                                                                         entityValidationState.SystemValidationStateAttribute == validationStateAttribute;

            EntityStateValidationCollection passedEntityValidationStates = this.GetAll<EntityStateValidationCollection, EntityStateValidation>(deltaEntityValidationStates, condition);

            condition = (entityValidationState) => entityValidationState.EntityId == entity.Id &&
                                                   entityValidationState.OperationResultType == OperationResultType.Error &&
                                                   entityValidationState.SystemValidationStateAttribute == validationStateAttribute;

            EntityStateValidationCollection errorEntityValidationStates = this.GetAll<EntityStateValidationCollection, EntityStateValidation>(originalEntityValidationStates, condition);

            EntityStateValidation errorItem = null;
            switch (validationStateAttribute)
            {
                case SystemAttributes.EntityCommonAttributesValid:
                case SystemAttributes.EntityCategoryAttributesValid:
                    errorItem = errorEntityValidationStates.FirstOrDefault(errorEntityValidationState => !passedEntityValidationStates.Any(passedEntityValidationState => passedEntityValidationState.AttributeId == errorEntityValidationState.AttributeId));
                    break;
                case SystemAttributes.EntityRelationshipsValid:
                    errorItem = errorEntityValidationStates.FirstOrDefault(errorEntityValidationState => !passedEntityValidationStates.Any(passedEntityValidationState => passedEntityValidationState.RelationshipId == errorEntityValidationState.RelationshipId && passedEntityValidationState.AttributeId == errorEntityValidationState.AttributeId));
                    break;
            }

            if (errorItem != null)
            {
                doesErrorExist = true;
            }

            return doesErrorExist;
        }

        /// <summary>
        /// Sets the entity validation states.
        /// </summary>
        /// <param name="entity">Indicates the entity which needs to be processed.</param>
        /// <param name="validationStateAttribute">Indicates the validation state attribute.</param>
        /// <param name="validityStateValue">Indicates the validation state value.</param>
        private void SetEntityValidationStates(Entity entity, SystemAttributes validationStateAttribute, ValidityStateValue validityStateValue)
        {
            switch (validationStateAttribute)
            {
                case SystemAttributes.EntityMetaDataValid:
                    entity.ValidationStates.IsMetaDataValid = validityStateValue;
                    break;

                case SystemAttributes.EntityCommonAttributesValid:
                    entity.ValidationStates.IsCommonAttributesValid = validityStateValue;
                    break;

                case SystemAttributes.EntityCategoryAttributesValid:
                    entity.ValidationStates.IsCategoryAttributesValid = validityStateValue;
                    break;

                case SystemAttributes.EntityRelationshipsValid:
                    entity.ValidationStates.IsRelationshipsValid = validityStateValue;
                    break;

                case SystemAttributes.EntityVariantValid:
                    entity.ValidationStates.IsEntityVariantValid = validityStateValue;
                    break;

                case SystemAttributes.EntityExtensionsValid:
                    entity.ValidationStates.IsExtensionsValid = validityStateValue;
                    break;

                case SystemAttributes.EntitySelfValid:
                    entity.ValidationStates.IsSelfValid = validityStateValue;
                    break;
            }

            Attribute attribute = (Attribute)entity.GetAttribute((Int32)validationStateAttribute);
            String value = (validityStateValue == ValidityStateValue.InValid) ? "False" : "True";

            if (attribute != null)
            {
                attribute.SetValue(value);
            }
        }

        /// <summary>
        /// Processes the state of the entity validation.
        /// </summary>
        /// <param name="entities">Indicate the collection of entity.</param>
        /// <param name="entityValidationStates">Indicates the entity validation states.</param>
        /// <param name="entityOperationResults">Indicates the entity operation results.</param>
        /// <param name="callerContext">Indicates Application and Module name by which action is being performed</param>     
        private void ProcessInternal(EntityCollection entities, EntityStateValidationCollection entityValidationStates, EntityOperationResultCollection entityOperationResults, CallerContext callerContext)
        {
            Boolean isTracingEnabled = _currentTraceSettings.IsBasicTracingEnabled;
            DiagnosticActivity diagnosticActivity = new DiagnosticActivity();

            try
            {
                if (isTracingEnabled)
                {
                    diagnosticActivity.Start();
                }

                #region Validations

                if (isTracingEnabled)
                {
                    diagnosticActivity.LogInformation("Parameters validation is started.");
                }

                if (isTracingEnabled)
                {
                    diagnosticActivity.LogInformation("Parameters validation is completed.");
                }

                #endregion Validations

                this.PopulateDefaultProgramName(callerContext, "EntityManager.EntityValidationStateBL.Process");
                DBCommandProperties command = DBCommandHelper.Get(callerContext, MDMCenterModuleAction.Update);

                new EntityStateValidationDA().Process(entities, entityValidationStates, entityOperationResults, _securityPrincipal.CurrentUserName, callerContext.ProgramName, command);
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
        /// Calculates the validation states.
        /// </summary>
        /// <param name="entities">Indicates the entity collection.</param>
        /// <param name="entityValidationStates"> Indicates the entity validation states.</param>
        /// <param name="callerContext">Indicates Application and Module name by which action is being performed</param>
        private EntityStateValidationCollection CalculateStateValidations(EntityCollection entities, EntityStateValidationCollection entityValidationStates, CallerContext callerContext)
        {
            Boolean isTracingEnabled = _currentTraceSettings.IsBasicTracingEnabled;
            DiagnosticActivity diagnosticActivity = new DiagnosticActivity();
            try
            {
                if (isTracingEnabled)
                {
                    diagnosticActivity.Start();
                }

                #region Get Validation Error List from Database

                Collection<Int64> entityIds = entities.GetEntityIdList();
                EntityStateValidationCollection originalEntityValidationStates = Get(entityIds, callerContext);
                #endregion Get Validation Error List from Database

                foreach (Entity entity in entities)
                {
                    //We shouldn't record entity validation states for categories as per design. Hence skipping them over here.
                    if (entity.EntityTypeId == Constants.CATEGORY_ENTITYTYPE)
                    {
                        continue;
                    }

                    CalculateValidationStateForEntity(entity, entityValidationStates, originalEntityValidationStates, SystemAttributes.EntityMetaDataValid);
                    CalculateValidationStateForEntity(entity, entityValidationStates, originalEntityValidationStates, SystemAttributes.EntityCategoryAttributesValid);
                    CalculateValidationStateForEntity(entity, entityValidationStates, originalEntityValidationStates, SystemAttributes.EntityCommonAttributesValid);
                    CalculateValidationStateForEntity(entity, entityValidationStates, originalEntityValidationStates, SystemAttributes.EntityRelationshipsValid);

                    // Calculate Entity Self Valid 
                    ValidityStateValue isSelfValid = (entity.ValidationStates.IsMetaDataValid == ValidityStateValue.InValid) ||
                                    (entity.ValidationStates.IsCommonAttributesValid == ValidityStateValue.InValid) ||
                                    (entity.ValidationStates.IsCategoryAttributesValid == ValidityStateValue.InValid) ? ValidityStateValue.InValid : ValidityStateValue.Valid;

                    SetEntityValidationStates(entity, SystemAttributes.EntitySelfValid, isSelfValid);
                }

                return originalEntityValidationStates;
            }
            finally
            {
                if (isTracingEnabled)
                {
                    diagnosticActivity.Stop();
                }
            }
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Returns Message Object based on message code
        /// </summary>
        /// <param name="messageCode">Message Code</param>
        /// <param name="callerContext">caller context indicating who called the API</param>
        /// <returns></returns>
        private LocaleMessage GetSystemLocaleMessage(String messageCode, CallerContext callerContext)
        {
            return _localeMessageBL.Get(_systemUILocale, messageCode, false, callerContext);
        }

        /// <summary>
        /// Gets the security principal.
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
        /// <param name="callerContext"></param>
        /// <param name="defaultProgramName"></param>
        private void PopulateDefaultProgramName(CallerContext callerContext, String defaultProgramName)
        {
            if (callerContext == null)
            {
                callerContext = new CallerContext();
            }

            if (String.IsNullOrWhiteSpace(callerContext.ProgramName))
            {
                callerContext.ProgramName = defaultProgramName;
            }
        }

        /// <summary>
        /// Gets the first or default item from the collection if it pass the condition.
        /// </summary>
        /// <typeparam name="T">Indicates the generic type</typeparam>
        /// <param name="TCollection">Indicates the generic type of collection.</param>
        /// <param name="condition">Indicates the Boolean condition for getting entity validation state collection</param>
        /// <returns>Returns the first element from the collection if it successfully pass the condition  </returns>
        private T GetFirstOrDefault<T>(IEnumerable<T> TCollection, Func<T, Boolean> condition)
        {
            T value = default(T);

            foreach (T item in TCollection)
            {
                if (condition(item))
                {
                    value = item;
                    break;
                }
            }

            return value;
        }

        /// <summary>
        /// Gets all.
        /// </summary>
        /// <typeparam name="TCollection">Indicates the type of the collection.</typeparam>
        /// <typeparam name="T">Indicates the type</typeparam>
        /// <param name="tCollection">Indicates the entity validation states.</param>
        /// <param name="condition">Indicates the Boolean condition.</param>
        /// <returns>Returns the collection according to Boolean condition.</returns>
        private TCollection GetAll<TCollection, T>(TCollection tCollection, Func<T, Boolean> condition) where TCollection : ICollection<T>, new()
        {
            TCollection returnCollection = new TCollection();

            foreach (T item in tCollection)
            {
                if (condition(item))
                {
                    returnCollection.Add(item);
                }
            }

            return returnCollection;
        }

        /// <summary>
        /// Checking the existence of item in collection according to the condition. 
        /// </summary>
        /// <typeparam name="T">Indicates the generic type</typeparam>
        /// <param name="TCollection">Indicates the generic type of collection.</param>
        /// <param name="condition">Indicates the Boolean condition for getting entity validation state collection</param>
        /// <returns>Returns true if collection successfully pass the condition  </returns>
        private Boolean DoesItemExist<T>(IEnumerable<T> TCollection, Func<T, Boolean> condition)
        {
            T value = GetFirstOrDefault(TCollection, condition);
            return value != null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="attributeId"></param>
        /// <param name="locale"></param>
        /// <param name="relationshipId"></param>
        /// <returns></returns>
        private String GetKey(Int32 attributeId, LocaleEnum locale, Int64 relationshipId)
        {
            return String.Format("A{0}.L{1}.RELID{2}", attributeId, (Int32)locale, relationshipId);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityId"></param>
        /// <param name="callerContext"></param>
        /// <returns></returns>
        private EntityTypeCollection GetEntityTypesBasedOnVariantLevel(Int64 entityId, CallerContext callerContext)
        {
            EntityTypeCollection entityTypes = null;
            EntityOperationsBL entityOperationsBL = new EntityOperationsBL();

            Dictionary<Int32, Int32> entityTypeIdToVariantLevelMappings = entityOperationsBL.GetEntityVariantLevel(entityId, callerContext);
            if (entityTypeIdToVariantLevelMappings != null && entityTypeIdToVariantLevelMappings.Count > 0)
            {
                Collection<Int32> entityTypeIdList = new Collection<Int32>();

                foreach (KeyValuePair<Int32, Int32> keyValuePair in entityTypeIdToVariantLevelMappings)
                {
                    if (keyValuePair.Value != Constants.VARIANT_LEVEL_ONE)
                    {
                        entityTypeIdList.Add(keyValuePair.Key);
                    }
                }

                EntityTypeBL entityTypeManager = new EntityTypeBL();
                entityTypes = entityTypeManager.GetEntityTypesByIds(entityTypeIdList);
            }

            return entityTypes;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mainEntityId"></param>
        /// <param name="entityTypes"></param>
        /// <param name="entityUniqueIdentifiers"></param>
        /// <returns></returns>
        private EntityContext GetEntityContext(Int64 mainEntityId, EntityTypeCollection entityTypes, EntityUniqueIdentifierCollection entityUniqueIdentifiers)
        {
            EntityContext mainEntityContext = new EntityContext()
            {
                LoadAttributes = true,
                LoadRelationships = true,
                LoadAttributeModels = true,
                RelationshipContext = new RelationshipContext()
                {
                    LoadRelationshipAttributes = true
                }
            };

            EntityExtensionContext entityExtensionContext = null;

            #region Prepare extension context

            entityExtensionContext = new EntityExtensionContext();

            foreach (EntityUniqueIdentifier item in entityUniqueIdentifiers)
            {
                Container container = new ContainerBL().GetById(item.ContainerId);
                Collection<LocaleEnum> dataLocales = container.SupportedLocales.GetLocaleEnums();

                EntityHierarchyContext entityHierachyContext = GetEntityHierarchyContext(entityTypes, dataLocales);

                if (item.EntityId == mainEntityId)
                {
                    mainEntityContext.DataLocales = dataLocales;
                    mainEntityContext.SetEntityHierarchyContext(entityHierachyContext);
                }
                else
                {
                    EntityContext entityContextForExtension = new EntityContext()
                    {
                        LoadAttributes = true,
                        LoadAttributeModels = true,
                        LoadRelationships = true,
                        DataLocales = dataLocales,
                        RelationshipContext = new RelationshipContext()
                        {
                            LoadRelationshipAttributes = true,
                            DataLocales = dataLocales
                        }
                    };

                    entityContextForExtension.SetEntityHierarchyContext(entityHierachyContext);
                    entityExtensionContext.AddEntityDataContext(container.Name, item.CategoryPath, entityContextForExtension);
                }
            }

            mainEntityContext.SetEntityExtensionContext(entityExtensionContext);

            #endregion Prepare extension context

            return mainEntityContext;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityTypes"></param>
        /// <param name="dataLocales"></param>
        /// <returns></returns>
        private EntityHierarchyContext GetEntityHierarchyContext(EntityTypeCollection entityTypes, Collection<LocaleEnum> dataLocales)
        {
            EntityHierarchyContext entityHierarchyContext = null;

            if (entityTypes != null && entityTypes.Count > 0)
            {
                entityHierarchyContext = new EntityHierarchyContext();

                foreach (EntityType entityType in entityTypes)
                {
                    EntityContext entityContextForHierarchy = new EntityContext()
                    {
                        LoadAttributes = true,
                        LoadRelationships = true,
                        DataLocales = dataLocales,
                        RelationshipContext = new RelationshipContext()
                        {
                            LoadRelationshipAttributes = true,
                            DataLocales = dataLocales
                        }
                    };

                    entityHierarchyContext.AddEntityDataContext(entityType.Name, entityContextForHierarchy);
                }
            }

            return entityHierarchyContext;
        }

        #endregion Helper Methods

        #region Validation Methods

        /// <summary>
        /// Validates the entity ids.
        /// </summary>
        /// <param name="entityIds">The entity ids.</param>
        /// <param name="callerContext">The caller context.</param>
        /// <param name="methodName">Name of the method.</param>
        /// <exception cref="MDMOperationException"> MDM Operation Exception</exception>
        private void ValidateEntityIds(Collection<Int64> entityIds, CallerContext callerContext, String methodName)
        {
            String errorMessage = String.Empty;

            if (entityIds == null || entityIds.Count < 1)
            {
                errorMessage = this.GetSystemLocaleMessage("112557", callerContext).Message;
                throw new MDMOperationException("112557", errorMessage, "EntityManager.EntityValidationStateBL" + methodName, String.Empty, methodName);
            }
            else
            {
                foreach (Int64 entityId in entityIds)
                {
                    if (entityId < 1)
                    {
                        errorMessage = this.GetSystemLocaleMessage("111645", callerContext).Message;
                        throw new MDMOperationException("111645", errorMessage, "EntityValidationStateBL" + methodName, String.Empty, methodName);
                    }
                }
            }
        }

        /// <summary>
        /// Validates the input parameters
        /// </summary>
        /// <param name="entityFamilyQueue">Indicates the entity family queue information to be processed</param>
        /// <param name="operationResult">Indicates the result of the operation performed</param>
        /// <param name="callerContext">Indicates the caller context specifying the module and application which invoke the method</param>
        private void ValidateInputParameters(EntityFamilyQueue entityFamilyQueue, OperationResult operationResult, CallerContext callerContext)
        {
            if (entityFamilyQueue == null)
            {
                _localeMessage = _localeMessageBL.Get(_systemUILocale, "114339", false, callerContext); //Entity Family Queue is not available or empty.
                operationResult.AddOperationResult(_localeMessage.Code, _localeMessage.Message, OperationResultType.Error);
                operationResult.OperationResultStatus = OperationResultStatusEnum.Failed;
            }
            else
            {
                if (entityFamilyQueue.EntityGlobalFamilyId < 1 && (entityFamilyQueue.RevalidateContext == null || entityFamilyQueue.RevalidateContext.RevalidateMode != RevalidateMode.Delta))
                {
                    _localeMessage = _localeMessageBL.Get(_systemUILocale, "114340", false, callerContext); //EntityGlobalFamilyId must be greater than 0.
                    operationResult.AddOperationResult(_localeMessage.Code, _localeMessage.Message, OperationResultType.Error);
                    operationResult.OperationResultStatus = OperationResultStatusEnum.Failed;
                }
            }
        }

        #endregion Validation Methods

        #region Revalidate Helpers Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityFamilyQueue"></param>
        /// <param name="callerContext"></param>
        /// <returns></returns>
        private EntityOperationResultCollection FullRevalidate(EntityFamilyQueue entityFamilyQueue, CallerContext callerContext)
        {
            EntityOperationResultCollection entityOperationResults = null;

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
                #region Initial Setup

                EntityBL entityBL = new EntityBL();

                Entity mainEntity = null;
                EntityCollection entities = new EntityCollection();

                CallerContext clonedCallerContext = (CallerContext)callerContext.Clone();
                clonedCallerContext.Module = MDMCenterModules.Revalidate;

                DBCommandProperties command = DBCommandHelper.Get(clonedCallerContext, MDMCenterModuleAction.Update);

                EntityProcessingOptions entityProcessingOptions = new EntityProcessingOptions()
                {
                    ProcessDefaultValues = false,
                    ValidateEntities = false,
                    ProcessingMode = ProcessingMode.Async
                };

                #endregion Initial Setup

                #region Step : Get entity family

                #region Prepare Entity Hierarchy/Extension Context

                Int64 mainEntityId = entityFamilyQueue.EntityGlobalFamilyId;

                EntityUniqueIdentifier entityUniqueIdentifier = new EntityUniqueIdentifier() { EntityId = mainEntityId };

                EntityGetOptions entityGetOptions = new EntityGetOptions()
                {
                    ApplyAVS = false,
                    ApplySecurity = false,
                    PublishEvents = false,
                    UpdateCache = false,
                    UpdateCacheStatusInDB = false,
                    LoadLatestFromDB = true
                };

                EntityTypeCollection entityTypes = GetEntityTypesBasedOnVariantLevel(mainEntityId, clonedCallerContext);

                EntityUniqueIdentifierCollection entityUniqueIdentifiers = entityBL.GetEntityUniqueIdentifiers(mainEntityId, null, clonedCallerContext);

                EntityContext mainEntityContext = GetEntityContext(mainEntityId, entityTypes, entityUniqueIdentifiers);

                #endregion Prepare Entity Hierarchy Context

                EntityHierarchyGetManager entityHierarchyGetManager = new EntityHierarchyGetManager(entityBL);
                mainEntity = entityHierarchyGetManager.GetEntityHierarchy(entityUniqueIdentifier, mainEntityContext, entityGetOptions, clonedCallerContext);

                if (isTracingEnabled)
                {
                    diagnosticActivity.LogDurationInfo("Entity family loading is completed.");
                }

                #endregion Step : Get entity family

                #region Step : Prepare Entity Operation Result Schema

                entities = (EntityCollection)mainEntity.GetFlattenEntities();

                entityOperationResults = new EntityOperationResultCollection(entities);

                if (isTracingEnabled)
                {
                    diagnosticActivity.LogDurationInfo("Prepared entity operation results");
                }

                #endregion Step : Prepare Entity Operation Result Schema

                #region Step : Trigger events

                MDMRuleParams mdmRuleParams = new MDMRuleParams()
                {
                    Entities = entities,
                    EntityOperationResults = entityOperationResults,
                    UserSecurityPrincipal = _securityPrincipal,
                    CallerContext = clonedCallerContext,
                    DDGCallerModule = DDGCallerModule.Revalidate
                };

                //PreLoadContextHelper.GetEntityContext(mdmRuleParams, entityBL);

                //MDMRuleEvaluator.Evaluate(mdmRuleParams);

                #endregion Step : Trigger events

                #region Step : Fill Entity

                if (!entityProcessingOptions.ProcessOnlyEntities)
                {
                    if (isTracingEnabled)
                    {
                        diagnosticActivity.LogInformation("Entity fill process is started.");
                    }

                    try
                    {
                        EntityGetOptions.EntityFillOptions entityFillOptions = new EntityGetOptions.EntityFillOptions();
                        entityFillOptions.FillLookupRowWithValues = false;

                        EntityFillHelper.FillEntities(entities, entityFillOptions, new EntityBL(), callerContext);
                    }
                    finally
                    {
                        if (isTracingEnabled)
                        {
                            diagnosticActivity.LogDurationInfo("Entity fill process is completed.");
                        }
                    }
                }

                #endregion Step : Fill Entity

                #region Step : Data validations

                EntityValidationBL entityValidationBL = new EntityValidationBL();
                entityValidationBL.Validate(entities, entityOperationResults, clonedCallerContext, entityProcessingOption: entityProcessingOptions);

                SetEntityActionBasedOnAttrAndRelChanges(entities);

                if (isTracingEnabled)
                {
                    diagnosticActivity.LogDurationInfo("Entity data validation process completed");
                }

                #endregion Step : Data validations

                using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required, GetTransactionOptions(ProcessingMode.Async)))
                {
                    #region Step : Diagnostics and tracing

                    if (isTracingEnabled)
                    {
                        diagnosticActivity.LogInformation("Entity full re-validate transaction scope is started.");
                    }

                    #endregion

                    try
                    {
                        EntityFamilyChangeContextCollection entityFamilyChangeContexts = EntityChangeContextHelper.GetEntityFamilyChangeContexts(entities, callerContext);

                        if (entityFamilyChangeContexts != null && entityFamilyChangeContexts.Count > 0)
                        {
                            if (isTracingEnabled)
                            {
                                diagnosticActivity.LogInformation("Starting entity database process...");
                            }

                            //EntityHierarchyProcessManager entityHierarchyProcessManager = new EntityHierarchyProcessManager();

                            //foreach (EntityFamilyChangeContext entityFamilyChangeContext in entityFamilyChangeContexts)
                            //{
                            //    EntityCollection filteredEntities = (EntityCollection)entities.GetEntitiesByFamilyId(entityFamilyChangeContext.EntityFamilyId);

                            //    if (filteredEntities != null && filteredEntities.Count > 0)
                            //    {
                            //        #region Step : Perform entity updates in database

                            //        entityHierarchyProcessManager.Process(filteredEntities, entityOperationResults, entityProcessingOptions, entityFamilyChangeContext, clonedCallerContext);

                            //        #endregion Step : Perform entity updates in database
                            //    }
                            //}
                        }

                        #region Step : Perform database update for state validation

                        EntityCollection allFamilies = new EntityCollection() { mainEntity };

                        EntityCollection allExtendedEntities = (EntityCollection)mainEntity.GetAllExtendedEntities();

                        if (allExtendedEntities != null && allExtendedEntities.Count > 0)
                        {
                            allFamilies.AddRange(allExtendedEntities);
                        }

                        ProcessEntityFamilyStates(allFamilies, entityOperationResults, clonedCallerContext);

                        if (isTracingEnabled)
                        {
                            diagnosticActivity.LogDurationInfo("Updated entity family state into database.");
                        }

                        #endregion Step : Perform database update for state validation

                        #region Step : Mark Complete And Release Lock

                        Boolean hasAnySystemError = entityOperationResults.HasAnySystemError();

                        if (!hasAnySystemError)
                        {
                            //Release a lock once process is done.
                            //new EntityFamilyQueueBL().MarkCompleteAndReleaseLock(mainEntityId, clonedCallerContext);
                        }

                        #endregion Step : Mark Complete And Release Lock

                        #region Step : Commit transaction

                        if (_currentTraceSettings.IsBasicTracingEnabled)
                        {
                            diagnosticActivity.LogInformation("Starting entity re-validate process transaction commit...");
                        }

                        //Commit transaction
                        transactionScope.Complete();

                        if (_currentTraceSettings.IsBasicTracingEnabled)
                        {
                            diagnosticActivity.LogDurationInfo("Entity re-validate process transaction commit completed");
                        }

                        #endregion
                    }
                    finally
                    {
                        if (isTracingEnabled)
                        {
                            diagnosticActivity.LogInformation("Entity full re-validate transaction scope ended.");
                        }
                    }
                }
            }
            finally
            {
                if (isTracingEnabled)
                {
                    diagnosticActivity.LogDurationInfo("Execution is completed for EntityStateValidationBL.FullRevalidate.");
                    diagnosticActivity.Stop();
                }
            }

            return entityOperationResults;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityFamilyQueue"></param>
        /// <param name="callerContext"></param>
        /// <returns></returns>
        private EntityOperationResultCollection DeltaRevalidate(EntityFamilyQueue entityFamilyQueue, CallerContext callerContext)
        {
            EntityOperationResultCollection entityOperationResults = null;

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

                EntityBL entityManager = new EntityBL();
                EntityCollection entities = new EntityCollection();

                EntityProcessingOptions entityProcessingOptions = new EntityProcessingOptions()
                {
                    ProcessDefaultValues = false,
                    PublishEvents = false,
                    ApplyAVS = false,
                    ProcessingMode = ProcessingMode.Async
                };

                CallerContext clonedCallerContext = (CallerContext)callerContext.Clone();
                clonedCallerContext.Module = MDMCenterModules.Revalidate;

                #endregion Step :  Initial Setup

                #region Step : Data Validation

                if (entityFamilyQueue.RevalidateContext == null)
                {
                    _localeMessage = _localeMessageBL.Get(_systemUILocale, "114571", false, callerContext); //Failed to perform delta revalidation as revalidate context is either null or empty.
                    throw new MDMOperationException(_localeMessage.Message);
                }

                if (entityFamilyQueue.RevalidateContext.RuleMapContextIds == null || entityFamilyQueue.RevalidateContext.RuleMapContextIds.Count < 1)
                {
                    _localeMessage = _localeMessageBL.Get(_systemUILocale, "114563", false, callerContext); //Failed to perform delta revalidation as the rule map context is not provided.
                    throw new MDMOperationException(_localeMessage.Message);
                }

                #endregion Step : Data Validation

                #region Step : Get entities

                Int64 entityId = entityFamilyQueue.EntityFamilyId;

                EntityGetOptions entityGetOptions = new EntityGetOptions()
                {
                    ApplyAVS = false,
                    ApplySecurity = false,
                    PublishEvents = false,
                    UpdateCache = false,
                    UpdateCacheStatusInDB = false,
                    LoadLatestFromDB = true
                };

                EntityContext entityContext = new EntityContext()
                {
                    LoadEntityProperties = true
                };

                Entity entity = entityManager.Get(entityId, entityContext, entityGetOptions, clonedCallerContext);

                if (entity != null)
                {
                    entity.OriginalEntity = entity.CloneBasicProperties();

                    entity.RuleMapContextIdList = entityFamilyQueue.RevalidateContext.RuleMapContextIds;
                    entity.Action = ObjectAction.Update;

                    entities.Add(entity);
                }

                if (isTracingEnabled)
                {
                    diagnosticActivity.LogDurationInfo("Entity loading is completed.");
                }

                #endregion Step : Get entities

                using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required, GetTransactionOptions(ProcessingMode.Async)))
                {
                    #region Step : Diagnostics and tracing

                    if (isTracingEnabled)
                    {
                        diagnosticActivity.LogInformation("Entity delta re-validate transaction scope is started.");
                    }

                    #endregion

                    try
                    {
                        #region Step : Process entities

                        entityOperationResults = entityManager.Process(entities, entityProcessingOptions, clonedCallerContext);

                        #endregion Step : Process entities

                        #region Step : Mark Complete And Release Lock

                        Boolean hasAnySystemError = entityOperationResults.HasAnySystemError();

                        if (!hasAnySystemError)
                        {
                            //Release a lock once process is done.
                            //new EntityFamilyQueueBL().MarkCompleteAndReleaseLock(entityFamilyQueue.EntityGlobalFamilyId, clonedCallerContext);
                        }

                        #endregion Step : Mark Complete And Release Lock

                        #region Step : Commit transaction

                        if (_currentTraceSettings.IsBasicTracingEnabled)
                        {
                            diagnosticActivity.LogInformation("Starting entity re-validate process transaction commit...");
                        }

                        //Commit transaction
                        transactionScope.Complete();

                        if (_currentTraceSettings.IsBasicTracingEnabled)
                        {
                            diagnosticActivity.LogDurationInfo("Entity re-validate process transaction commit completed");
                        }

                        #endregion
                    }
                    finally
                    {
                        if (isTracingEnabled)
                        {
                            diagnosticActivity.LogInformation("Entity delta re-validate transaction scope ended.");
                        }
                    }
                }
            }
            finally
            {
                if (isTracingEnabled)
                {
                    diagnosticActivity.LogDurationInfo("Execution is completed for EntityStateValidationBL.DeltaRevalidate.");
                    diagnosticActivity.Stop();
                }
            }

            return entityOperationResults;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entities"></param>
        private void SetEntityActionBasedOnAttrAndRelChanges(EntityCollection entities)
        {
            if (entities != null && entities.Count > 0)
            {
                foreach (Entity entity in entities)
                {
                    //If entity action is read and any change in attributes then set entity action as 'Update'.
                    if (entity.Action == ObjectAction.Read && entity.Attributes.HasChanged())
                    {
                        entity.Action = ObjectAction.Update;
                    }

                    Boolean hasRelationshipsChanged = false;
                    SetRelationshipActionBasedOnRelAttrChanges(entity.Relationships, ref hasRelationshipsChanged);

                    //If entity action is still read and any change found for relationships then set entity action as 'Update'.
                    if (entity.Action == ObjectAction.Read && hasRelationshipsChanged)
                    {
                        entity.Action = ObjectAction.Update;
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="relationships"></param>
        /// <param name="hasRelationshipsChanged"></param>
        private void SetRelationshipActionBasedOnRelAttrChanges(RelationshipCollection relationships, ref Boolean hasRelationshipsChanged)
        {
            if (relationships != null && relationships.Count > 0)
            {
                foreach (Relationship relationship in relationships)
                {
                    //If relationship action is read and any change in attributes then set entity action as 'Update'.
                    if (relationship.Action == ObjectAction.Read && relationship.RelationshipAttributes.HasChanged())
                    {
                        relationship.Action = ObjectAction.Update;
                        hasRelationshipsChanged = true;
                    }

                    //Recursive call for child relationships.
                    SetRelationshipActionBasedOnRelAttrChanges(relationship.RelationshipCollection, ref hasRelationshipsChanged);
                }
            }
        }

        #endregion Revalidate Helpers Methods

        #endregion Private Methods

        #endregion Methods
    }
}