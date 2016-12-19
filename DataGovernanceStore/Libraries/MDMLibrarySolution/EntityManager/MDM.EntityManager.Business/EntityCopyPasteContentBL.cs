using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Collections.ObjectModel;
using System.Linq;

namespace MDM.EntityManager.Business
{
    using MDM.BusinessObjects;
    using MDM.Core;
    using MDM.Core.Exceptions;
    using MDM.Core.Extensions;
    using MDM.MessageManager.Business;
    using MDM.Utility;
    using MDM.Interfaces;

    /// <summary>
    /// Business Class for copy paste contents of entities
    /// </summary>
    public class EntityCopyPasteContentBL : BusinessLogicBase
    {
        #region Fields

        LocaleMessageBL _localeMessageBL = null;

        private const String NotMappedOrUnavailableErrorMessage = "Attribute {0} of Group {1} was not pasted because it is not mapped or not available in Target Entity {2}";
        private const String NotMappedOrUnavailableErrorCode = "113936";
        private const OperationResultType NotMappedOrUnavailableErrorType = OperationResultType.Warning;

        private const String InvalidValuesErrorMessage = "Attribute {0} of Group {1} has invalid value and was not pasted to Target Entity {2}";
        private const String InvalidValuesErrorCode = "113935";
        private const OperationResultType InvalidValuesErrorType = OperationResultType.Warning;

        private const String EntityNotProcessedErrorCode = "113938";
        private const String EntityNotProcessedErrorMessage = "Entity {0} was not processed";
        private const OperationResultType EntityNotProcessedErrorType = OperationResultType.Warning;

        private const String PasteResultMessageCode = "113937";
        private const String PasteResultMessage = "{0} Attributes and {1} Relationships were successfully passed to Entity {2}";
        private const OperationResultType PasteResultMessageType = OperationResultType.Information;

        #endregion

        #region Constructors

        /// <summary>
        /// Default Constructor that loads the security principal from Cache if present
        /// </summary>
        public EntityCopyPasteContentBL()
        {
            _localeMessageBL = new LocaleMessageBL();
            //GetSecurityPrincipal();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// It Copies requested Attributes and Relationships from one Entity to Multiple Entities.
        /// </summary>
        /// <param name="entityCopyPasteContext">EntityCopyPasteContext conatines source and target entityId, attributes and Relationships that needs to be copied.</param>
        /// <param name="callerContext">Indicates Application and Module name by which action is being performed</param>
        /// <returns>Returns the results of the operation having errors and information, if any</returns>
        /// <exception cref="MDMOperationException">If EntityCopyPasteContext is null </exception>
        public EntityOperationResultCollection CopyPasteEntityContents(EntityCopyPasteContext entityCopyPasteContext, CallerContext callerContext)
        {
            if (Constants.PERFORMANCE_TRACING_ENABLED || Constants.TRACING_ENABLED)
                MDMTraceHelper.StartTraceActivity("EntityCopyPasteContentManager.Entity.CopyPasteContent", MDMTraceSource.EntityProcess, false);

            EntityOperationResultCollection entityOperationResults = new EntityOperationResultCollection();
            Entity sourceEntity = null;
            EntityCollection targetEntities = null;
            EntityBL entityManager = new EntityBL();
            Collection<LocaleEnum> sourceDataLocales = new Collection<LocaleEnum>();
            Collection<LocaleEnum> targetDataLocales = new Collection<LocaleEnum>();
            DurationHelper durationHelper = new DurationHelper(DateTime.Now);
            DurationHelper overallDurationHelper = new DurationHelper(DateTime.Now);
            EntityCollection entitiesToProcess = new EntityCollection();

            Dictionary<Int64, AttributeOperationResultCollection> unsuccessfullAttributeOperationResults = new Dictionary<Int64, AttributeOperationResultCollection>();
            Dictionary<Int64, RelationshipOperationResultCollection> unsuccessfullRelationshipOperationResults = new Dictionary<Int64, RelationshipOperationResultCollection>();
            EntityCollection notProcessedEntities = new EntityCollection();

            try
            {
                #region STEP : Parameter Validation

                ValidateEntityCopyPasteContext(entityCopyPasteContext, callerContext);

                #endregion

                #region STEP : Prepare EntityContext

                //Prepare EntityContext to get entities
                EntityContext entityContext = PopulateEntityContext(entityCopyPasteContext, sourceDataLocales, targetDataLocales);

                #endregion

                #region STEP : Get Source Entity

                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("Getting Source Entity for EntityId {0} has been started..", entityCopyPasteContext.FromEntityId), MDMTraceSource.EntityProcess);

                EntityCollection sourceEntities = GetEntities(entityCopyPasteContext.FromContainerId, new Collection<Int64>() { entityCopyPasteContext.FromEntityId }, entityContext, sourceDataLocales, callerContext);

                if (sourceEntities != null)
                {
                    sourceEntity = (Entity)sourceEntities.GetEntity(entityCopyPasteContext.FromEntityId);
                }

                if (sourceEntity == null)
                {
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, String.Format("Failed to load source entity {0} for copy paste operation", entityCopyPasteContext.FromEntityId), MDMTraceSource.EntityProcess);

                    entityOperationResults.AddOperationResult("113061", "Failed to load source entity {0} for copy paste operation", OperationResultType.Error);

                    return entityOperationResults;
                }

                if (Constants.PERFORMANCE_TRACING_ENABLED || Constants.TRACING_ENABLED)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("{0}ms - To load the source entity {1}", durationHelper.GetDurationInMilliseconds(DateTime.Now), entityCopyPasteContext.FromEntityId), MDMTraceSource.EntityProcess);

                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Getting Source Entity completed.", MDMTraceSource.EntityProcess);

                #endregion

                #region STEP : Get Target Entities

                if (entityCopyPasteContext.ToEntityIds != null && entityCopyPasteContext.ToEntityIds.Count > 0)
                {
                    if (Constants.TRACING_ENABLED)
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("Getting Target Entities {0} have been started..", entityCopyPasteContext.ToEntityIds.Count), MDMTraceSource.EntityProcess);

                    targetEntities = GetEntities(entityCopyPasteContext.ToContainerId, entityCopyPasteContext.ToEntityIds, entityContext, targetDataLocales, callerContext);

                    if (Constants.PERFORMANCE_TRACING_ENABLED)
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("{0}ms - To load the target entities", durationHelper.GetDurationInMilliseconds(DateTime.Now)), MDMTraceSource.EntityProcess);

                    if (Constants.TRACING_ENABLED)
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Getting Target Entities completed.", MDMTraceSource.EntityProcess);

                }

                #endregion

                #region STEP : Paste Content from Source Entity to Target Entity

                if (sourceEntity != null && targetEntities != null && targetEntities.Count > 0)
                {
                    #region Copy Common and Technical Attributes

                    if (entityCopyPasteContext.AttributeIds != null && entityCopyPasteContext.AttributeIds.Count > 0 && sourceEntity.AttributeModels != null)
                    {
                        if (Constants.TRACING_ENABLED)
                            MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Copy Paste of Common and Technical attributes have been started..", MDMTraceSource.EntityProcess);

                        CopyPasteAttributes(sourceEntity, targetEntities, unsuccessfullAttributeOperationResults);

                        if (Constants.PERFORMANCE_TRACING_ENABLED)
                            MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("{0}ms - To Paste {1} requested attributes to the target entities", durationHelper.GetDurationInMilliseconds(DateTime.Now), entityCopyPasteContext.AttributeIds.Count), MDMTraceSource.EntityProcess);

                        if (Constants.TRACING_ENABLED)
                            MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Copy Paste of Common and Technical attributes have been completed..", MDMTraceSource.EntityProcess);

                    }

                    #endregion

                    #region Copy Relationships

                    if (entityCopyPasteContext.RelationshipTypeIds != null && entityCopyPasteContext.RelationshipTypeIds.Count > 0 && sourceEntity.Relationships != null)
                    {
                        if (Constants.TRACING_ENABLED)
                            MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Copy Paste of Relationships have been started...", MDMTraceSource.EntityProcess);

                        CopyPasteRelationships(sourceEntity, targetEntities, callerContext, unsuccessfullRelationshipOperationResults);

                        if (Constants.PERFORMANCE_TRACING_ENABLED)
                            MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("{0}ms - To Paste requested relationships to the target entities", durationHelper.GetDurationInMilliseconds(DateTime.Now)), MDMTraceSource.EntityProcess);

                        if (Constants.TRACING_ENABLED)
                            MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Copy Paste of Relationships have been completed.", MDMTraceSource.EntityProcess);

                    }

                    #endregion
                }

                #endregion

                #region STEP : Process Target Entities

                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Processing of target entities started...", MDMTraceSource.EntityProcess);

                EntityProcessingOptions entityProcessingOption = new EntityProcessingOptions(true, true, false, true);

                foreach (Entity entity in targetEntities)
                {
                    if (entity.Action == ObjectAction.Update)
                    {
                        entitiesToProcess.Add(entity);
                    }
                    else
                    {
                        notProcessedEntities.Add(entity);
                    }
                }

                if (!entitiesToProcess.IsNullOrEmpty())
                {
                    entityOperationResults = entityManager.Process(entitiesToProcess, entityProcessingOption, callerContext);
                }

                #region Populate Operation Results

                if (!notProcessedEntities.IsNullOrEmpty())
                {
                    // Add operation result if Entity was not processed to inform User
                    foreach (Entity entity in notProcessedEntities)
                    {
                        EntityOperationResult entityOperationResult = new EntityOperationResult(entity.Id, entity.ExternalId, entity.LongName);
                        entityOperationResult.AddOperationResult(EntityNotProcessedErrorCode, String.Format(EntityNotProcessedErrorMessage, entity.Name), String.Empty, new Collection<Object> { entity.Name }, EntityNotProcessedErrorType);
                        entityOperationResults.Add(entityOperationResult);
                    }
                }

                if (!unsuccessfullAttributeOperationResults.IsNullOrEmpty())
                {
                    if (Constants.TRACING_ENABLED)
                    {
                        // todo[dd]: localize
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Warning, "Some of Attributes that you have selected were not pasted to Target Entity", MDMTraceSource.EntityProcess);
                    }

                    // Add Attribute Operation Result to corresponding Entity Operation Result
                    foreach (KeyValuePair<Int64, AttributeOperationResultCollection> unsuccessfullAttributeOperationResult in unsuccessfullAttributeOperationResults)
                    {
                        EntityOperationResult entityOperationResult = entityOperationResults.FirstOrDefault(entity => entity.EntityId == unsuccessfullAttributeOperationResult.Key);

                        if (entityOperationResult == null)
                        {
                            continue;
                        }

                        foreach (AttributeOperationResult attributeOperationResult in unsuccessfullAttributeOperationResult.Value)
                        {
                            entityOperationResult.AttributeOperationResultCollection.Add(attributeOperationResult);
                        }

                        entityOperationResult.AttributeOperationResultCollection.RefreshOperationResultStatus();

                        entityOperationResult.OperationResultStatus = GetEntityOperationResultStatus(entityOperationResult.AttributeOperationResultCollection.OperationResultStatus, entityOperationResult.OperationResultStatus);
                    }

                    entityOperationResults.RefreshOperationResultStatus();
                }

                if (!unsuccessfullRelationshipOperationResults.IsNullOrEmpty())
                {
                    // Add Relationships Operation Result to corresponding Entity Operation Result
                    foreach (KeyValuePair<Int64, RelationshipOperationResultCollection> unsuccessfullRelationshipOperationResult in unsuccessfullRelationshipOperationResults)
                    {
                        EntityOperationResult entityOperationResult = entityOperationResults.FirstOrDefault(entity => entity.EntityId == unsuccessfullRelationshipOperationResult.Key);

                        if (entityOperationResult == null)
                        {
                            continue;
                        }

                        foreach (RelationshipOperationResult attributeOperationResult in unsuccessfullRelationshipOperationResult.Value)
                        {
                            entityOperationResult.RelationshipOperationResultCollection.Add(attributeOperationResult);
                        }

                        entityOperationResult.RelationshipOperationResultCollection.RefreshOperationResultStatus();

                        entityOperationResult.OperationResultStatus = GetEntityOperationResultStatus(entityOperationResult.RelationshipOperationResultCollection.OperationResultStatus,
                                entityOperationResult.OperationResultStatus);
                    }

                    entityOperationResults.RefreshOperationResultStatus();
                }

                if (entityOperationResults.OperationResultStatus != OperationResultStatusEnum.Successful)
                {
                    foreach (EntityOperationResult entityOperationResult in entityOperationResults)
                    {
                        Int32 attributesPastedSuccessfully =
                            entityOperationResult.AttributeOperationResultCollection.OperationResultStatus == OperationResultStatusEnum.Successful ?
                            entityOperationResult.AttributeOperationResultCollection.Count :
                            CountSuccessfullOperationResults<AttributeOperationResult, AttributeOperationResultCollection>(entityOperationResult.AttributeOperationResultCollection);
                        
                        Int32 relationshipsPastedSuccessfully = entityOperationResult.RelationshipOperationResultCollection.OperationResultStatus == OperationResultStatusEnum.Successful ?
                            entityOperationResult.RelationshipOperationResultCollection.Count :
                            CountSuccessfullOperationResults<RelationshipOperationResult, RelationshipOperationResultCollection>(entityOperationResult.RelationshipOperationResultCollection);

                        entityOperationResult.AddOperationResult(PasteResultMessageCode, String.Format(PasteResultMessage, attributesPastedSuccessfully, relationshipsPastedSuccessfully, entityOperationResult.EntityLongName),
                            String.Empty, new Collection<Object> { attributesPastedSuccessfully, relationshipsPastedSuccessfully, entityOperationResult.EntityLongName }, PasteResultMessageType);
                    }
                }

                #endregion

                if (Constants.PERFORMANCE_TRACING_ENABLED)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("{0}ms - To process {1} target entities", durationHelper.GetDurationInMilliseconds(DateTime.Now), entitiesToProcess.Count), MDMTraceSource.EntityProcess);

                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Processing of target entities completed...", MDMTraceSource.EntityProcess);

                #endregion
            }
            finally
            {
                if (Constants.PERFORMANCE_TRACING_ENABLED || Constants.TRACING_ENABLED)
                {
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("{0}ms - Overall time to load requested entities", overallDurationHelper.GetDurationInMilliseconds(DateTime.Now)), MDMTraceSource.EntityProcess);
                    MDMTraceHelper.StopTraceActivity("EntityCopyPasteContentManager.Entity.CopyPasteContent", MDMTraceSource.EntityProcess);
                }
            }

            return entityOperationResults;
        }

        #endregion

        #region Private Methods

        private void ValidateEntityCopyPasteContext(EntityCopyPasteContext entityCopyPasteContext, CallerContext callerContext)
        {
            LocaleMessage _localeMessage = new LocaleMessage();

            if (entityCopyPasteContext == null)
            {
                _localeMessage = _localeMessageBL.Get(GlobalizationHelper.GetSystemUILocale(), "113062", false, callerContext);
                throw new MDMOperationException("113062", _localeMessage.Message, "EntityManager", String.Empty, "CopyPasteContent");
            }

            if (entityCopyPasteContext.FromEntityId < 1)
            {
                _localeMessage = _localeMessageBL.Get(GlobalizationHelper.GetSystemUILocale(), "113059", false, callerContext);
                throw new MDMOperationException("113059", _localeMessage.Message, "EntityManager", String.Empty, "CopyPasteContent");
            }

            if (entityCopyPasteContext.ToEntityIds != null)
            {
                foreach (Int64 entityId in entityCopyPasteContext.ToEntityIds)
                {
                    if (entityId < 1)
                    {
                        _localeMessage = _localeMessageBL.Get(GlobalizationHelper.GetSystemUILocale(), "113060", false, callerContext);
                        throw new MDMOperationException("113060", _localeMessage.Message, "EntityManager", String.Empty, "CopyPasteContent");
                    }
                    else if (entityId == entityCopyPasteContext.FromEntityId)
                    {
                        if (entityCopyPasteContext.LocaleMappings != null && entityCopyPasteContext.LocaleMappings.Count > 0)
                        {
                            foreach (KeyValuePair<LocaleEnum, LocaleEnum> localeMapping in entityCopyPasteContext.LocaleMappings)
                            {
                                if (localeMapping.Key == localeMapping.Value)
                                {
                                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, "Exception: Please select a different item for paste as the target item and source item are same", MDMTraceSource.EntityProcess);
                        _localeMessage = _localeMessageBL.Get(GlobalizationHelper.GetSystemUILocale(), "100798", false, callerContext);
                        throw new MDMOperationException("100798", _localeMessage.Message, "EntityManager", String.Empty, "CopyPasteContent"); //Please select a different item for paste as the target item and source item are same
                    }
                }
            }
                    }
                }
            }

            if (entityCopyPasteContext.ToContainerId < 1 || entityCopyPasteContext.FromContainerId < 1)
            {
                _localeMessage = _localeMessageBL.Get(GlobalizationHelper.GetSystemUILocale(), "113063", false, callerContext);
                throw new MDMOperationException("113063", _localeMessage.Message, "EntityManager", String.Empty, "CopyPasteContent");
            }

            if (entityCopyPasteContext.AttributeIds == null && entityCopyPasteContext.RelationshipTypeIds == null)
            {
                //110610
                _localeMessage = _localeMessageBL.Get(GlobalizationHelper.GetSystemUILocale(), "110610", false, callerContext); //There are no Attributes and Relationships to be copied
                throw new MDMOperationException("110610", _localeMessage.Message, "EntityManager", String.Empty, "CopyPasteContent");
            }

        }

        private EntityContext PopulateEntityContext(EntityCopyPasteContext entityCopyPasteContext, Collection<LocaleEnum> sourceDataLocales, Collection<LocaleEnum> targetDataLocales)
        {
            EntityContext entityContext = new EntityContext();
            entityContext.AttributeModelType = AttributeModelType.All;
            entityContext.LoadAttributeModels = true;

            if (entityCopyPasteContext.AttributeIds != null && entityCopyPasteContext.AttributeIds.Count > 0)
            {
                entityContext.AttributeIdList = entityCopyPasteContext.AttributeIds;
                entityContext.LoadAttributes = true;
            }

            if (entityCopyPasteContext.RelationshipTypeIds != null && entityCopyPasteContext.RelationshipTypeIds.Count > 0)
            {
                entityContext.LoadRelationships = true;
                entityContext.RelationshipContext.LoadRelationshipAttributes = true;
                entityContext.RelationshipContext.RelationshipTypeIdList = entityCopyPasteContext.RelationshipTypeIds;
            }

            if (entityCopyPasteContext.LocaleMappings != null && entityCopyPasteContext.LocaleMappings.Count > 0)
            {
                foreach (KeyValuePair<LocaleEnum, LocaleEnum> localeMappings in entityCopyPasteContext.LocaleMappings)
                {
                    if (!sourceDataLocales.Contains(localeMappings.Key))
                    {
                        sourceDataLocales.Add(localeMappings.Key);
                    }

                    if (!targetDataLocales.Contains(localeMappings.Value))
                    {
                        targetDataLocales.Add(localeMappings.Value);
                    }
                }
            }

            return entityContext;
        }

        private EntityCollection GetEntities(Int32 conatinerId, Collection<Int64> entityIds, EntityContext entityContext, Collection<LocaleEnum> locales, CallerContext callerContext)
        {
            EntityCollection entities = new EntityCollection();
            EntityBL entityManager = new EntityBL();

            entityContext.ContainerId = conatinerId;
            entityContext.DataLocales = locales;
            entityContext.Locale = locales[0];

            entities = entityManager.Get(entityIds, entityContext, false, callerContext.Application, callerContext.Module, false, false, false);

            return entities;
        }

        private void CopyPasteAttributes(Entity sourceEntity, EntityCollection targetEntities, Dictionary<Int64, AttributeOperationResultCollection> unsuccessfullAttributeOperationResults)
        {
            AttributeModelCollection attributeModelsWithInvalidValues = null;

            foreach (AttributeModel attributeModel in sourceEntity.AttributeModels)
            {
                if (attributeModel.AttributeModelType != AttributeModelType.Relationship)
                {
                    Attribute sourceAttribute = (Attribute)sourceEntity.GetAttribute(attributeModel.Id, attributeModel.Locale);

                    if (sourceAttribute != null)
                    {
                        if (!sourceAttribute.HasInvalidOverriddenValues)
                        {
                            if (attributeModel.Required && !sourceAttribute.HasValue(false) && sourceAttribute.SourceFlag == AttributeValueSource.Overridden)
                            {
                                //Do not copy the attribute from source entity if attribute is required and do not have any value in source entity/ Same for Complex child
                                //sourceEntity.Attributes.Remove(sourceAttribute);
                                continue;
                            }

                            foreach (Entity entity in targetEntities)
                            {
                                Attribute targetAttribute = (Attribute)entity.GetAttribute(attributeModel.Id, entity.Locale);

                                if (targetAttribute != null)
                                {
                                    if ((attributeModel.Required && attributeModel.ReadOnly) && targetAttribute.HasValue(false) && targetAttribute.SourceFlag == AttributeValueSource.Overridden)
                                    {
                                        //If attribute is required and read only and target attribute has value then do not override the existing values.
                                        continue;
                                    }

                                    if (sourceAttribute.SourceFlag != AttributeValueSource.Inherited)
                                    {
                                        targetAttribute.Action = ObjectAction.Update;
                                        
                                        if (sourceAttribute.OverriddenValues != null && sourceAttribute.OverriddenValues.Count > 0)
                                        {
                                            if (targetAttribute.IsHierarchical)
                                            {
                                                CopyPasteHierarchicalAttribute(sourceAttribute, targetAttribute);
                                            }
                                            else 
                                            {
                                                foreach (Value val in sourceAttribute.OverriddenValues)
                                                {
                                                    val.AttrVal = val.InvariantVal;
                                                    val.Locale = targetAttribute.Locale;
                                                }

                                                sourceAttribute.Locale = targetAttribute.Locale;
                                                targetAttribute.SetValue(sourceAttribute.OverriddenValues, targetAttribute.Locale);

                                                if (targetAttribute.IsComplex)
                                                {
                                                    CopyPasteComplexAttribute(sourceAttribute, targetAttribute);
                                                }
                                            }
                                        }
                                        else if (targetAttribute.SourceFlag == AttributeValueSource.Overridden && targetAttribute.HasValue(false))
                                        {
                                            //source attribute has NO value and target attribute has value then clear the target attribute value.
                                            targetAttribute.ClearValue();
                                        }
                                    }
                                    else if (targetAttribute.SourceFlag == AttributeValueSource.Overridden && targetAttribute.HasValue(false))
                                    {
                                        //If source attribute is inherited and target attribute has value then clear the target attribute value.
                                        targetAttribute.ClearValue();
                                    }

                                    entity.Action = ObjectAction.Update;
                                }
                                else
                                {
                                    // Attribute not mapped or unavailable
                                    AddAttributeOperationResultToDictionary(unsuccessfullAttributeOperationResults, entity.Id, attributeModel, NotMappedOrUnavailableErrorCode, NotMappedOrUnavailableErrorMessage,
                                        new Collection<Object> { attributeModel.Name, attributeModel.AttributeParentName, entity.LongName }, NotMappedOrUnavailableErrorType);
                                }
                            }
                        }
                        else
                        {
                            // Attribute has invalid values
                            if (attributeModelsWithInvalidValues == null)
                            {
                                attributeModelsWithInvalidValues = new AttributeModelCollection();
                            }

                            attributeModelsWithInvalidValues.Add(attributeModel);
                        }
                    }
                }
            }

            ProcessAttributesWithInvalidValues(attributeModelsWithInvalidValues, targetEntities, unsuccessfullAttributeOperationResults);
        }

        private void ProcessAttributesWithInvalidValues(AttributeModelCollection attributeModelsWithInvalidValues, EntityCollection targetEntities, Dictionary<Int64, AttributeOperationResultCollection> unsuccessfullAttributeOperationResults)
        {
            if (attributeModelsWithInvalidValues != null && attributeModelsWithInvalidValues.Any())
            {
                foreach (AttributeModel attributeModelWithInvalidValue in attributeModelsWithInvalidValues)
                {
                    foreach (Entity entity in targetEntities)
                    {
                        AddAttributeOperationResultToDictionary(unsuccessfullAttributeOperationResults, entity.Id, attributeModelWithInvalidValue, InvalidValuesErrorCode, InvalidValuesErrorMessage,
                            new Collection<Object> { attributeModelWithInvalidValue.Name, attributeModelWithInvalidValue.AttributeParentName, entity.Name }, InvalidValuesErrorType);
                    }
                }
            }
        }

        private void AddAttributeOperationResultToDictionary(Dictionary<Int64, AttributeOperationResultCollection> dictionary, Int64 key, AttributeModel attributeModel, String errorCode,
            String errorMessage, Collection<Object> parameters, OperationResultType errorMessageType)
        {
            AttributeOperationResult operationResult = new AttributeOperationResult(attributeModel.Id, attributeModel.Name, attributeModel.LongName, attributeModel.AttributeModelType, attributeModel.Locale);
            operationResult.AddOperationResult(errorCode, String.Format(errorMessage, parameters.ToArray()), String.Empty, parameters, errorMessageType);

            AddResultToDictionary(dictionary, key, operationResult);
        }
        
        private void CopyPasteRelationships(Entity sourceEntity, EntityCollection targetEntities, CallerContext callerContext, Dictionary<Int64, RelationshipOperationResultCollection> unsuccessfullRelationshipsOperationResults)
        {
            foreach (Entity targetEntity in targetEntities)
            {
                EntityOperationResult eor = new EntityOperationResult(targetEntity.Id, targetEntity.LongName);
                eor.RelationshipOperationResultCollection = new RelationshipOperationResultCollection(sourceEntity.Relationships);

                foreach (Relationship sourceRelationship in sourceEntity.Relationships)
                {
                    List<Attribute> attributesWithInvalidValues = null;

                    Relationship targetRelationship = (Relationship)targetEntity.Relationships.GetRelationship(sourceRelationship.RelatedEntityId, sourceRelationship.RelationshipTypeId);

                    RelationshipOperationResult ror = eor.RelationshipOperationResultCollection.GetRelationshipOperationResult(sourceRelationship.Id);
                    
                    if (targetRelationship != null)
                    {
                        //If Relationship Exist in Target Entity Update only relationship attributes.
                        targetRelationship.Action = ObjectAction.Update;
                        targetRelationship.Locale = targetEntity.Locale;

                        //If exist, copy relationship attributes.
                        CopyPasteRelationshipAttributes(sourceRelationship, targetRelationship, ref attributesWithInvalidValues);
                        targetEntity.Action = ObjectAction.Update;

                        ProcessRelationshipAttributesWithInvalidValues(unsuccessfullRelationshipsOperationResults, attributesWithInvalidValues, ror, targetEntity);
                    }
                    else
                    {
                        //If Relationship doesn't exist in target entity... Create the Relationship in target Entity
                        sourceRelationship.FromEntityId = targetEntity.Id;

                        //Check for the Relationship Mappings and Cardinality
                        EntityValidationBL entityValidationBL = new EntityValidationBL();
                        ror = eor.RelationshipOperationResultCollection.GetRelationshipOperationResult(sourceRelationship.Id);

                        entityValidationBL.ValidateRelationshipMappings(targetEntity.ContainerId, targetEntity.ContainerName, targetEntity.EntityTypeId, targetEntity.EntityTypeLongName, sourceRelationship, ror, null, callerContext);

                        if (ror.OperationResultStatus != OperationResultStatusEnum.Failed && ror.OperationResultStatus != OperationResultStatusEnum.CompletedWithErrors)
                        {
                            sourceRelationship.Action = ObjectAction.Create;

                            if (targetEntity.Relationships == null)
                                targetEntity.Relationships = new RelationshipCollection();

                            if (sourceRelationship.RelationshipAttributes != null && sourceRelationship.RelationshipAttributes.Count > 0)
                            {
                                foreach (Attribute attribute in sourceRelationship.RelationshipAttributes)
                                {
                                    attribute.Action = ObjectAction.Create;
                                }
                            }

                            targetEntity.Relationships.Add(sourceRelationship);
                            targetEntity.Action = ObjectAction.Update;
                        }
                        else
                        {
                            AddResultToDictionary(unsuccessfullRelationshipsOperationResults, targetEntity.Id, ror);
                        }
                    }
                }
            }
        }

        private void CopyPasteRelationshipAttributes(Relationship sourceRelationship, Relationship targetRelationship, ref List<Attribute> attributesWithInvalidValues)
        {
            foreach (Attribute sourceAttribute in sourceRelationship.RelationshipAttributes)
            {
                if (sourceAttribute.AttributeModelType == AttributeModelType.Relationship)
                {
                    if (!sourceAttribute.HasInvalidOverriddenValues)
                    {
                        if (sourceAttribute.Required && !sourceAttribute.HasValue(false) && sourceAttribute.SourceFlag == AttributeValueSource.Overridden)
                        {
                            //Do not copy the attribute from source entity if attribute is required and do not have any value in source entity/ Same for Complex child
                            //sourceEntity.Attributes.Remove(sourceAttribute);
                            continue;
                        }

                        IAttribute targetAttribute = targetRelationship.RelationshipAttributes.GetAttribute(sourceAttribute.Id, targetRelationship.Locale);

                        if (targetAttribute != null)
                        {
                            if ((sourceAttribute.Required || sourceAttribute.ReadOnly) && targetAttribute.HasValue(false) && targetAttribute.SourceFlag == AttributeValueSource.Overridden)
                            {
                                continue;
                            }

                            if (sourceAttribute.SourceFlag != AttributeValueSource.Inherited)
                            {
                                sourceAttribute.Action = ObjectAction.Update;

                                if ((sourceAttribute.OverriddenValues != null && sourceAttribute.OverriddenValues.Count > 0))
                                {
                                    foreach (Value val in sourceAttribute.OverriddenValues)
                                    {
                                        val.Action = ObjectAction.Update;
                                    }
                                }

                                targetAttribute.SetValue(sourceAttribute.OverriddenValues);
                            }
                            else if (targetAttribute.SourceFlag == AttributeValueSource.Overridden && targetAttribute.HasValue(false))
                            {
                                //If source attribute is inherited and target attribute has value then clear the target attribute value.
                                targetAttribute.ClearValue();
                            }
                        }
                    }
                    else
                    {
                        if (attributesWithInvalidValues == null)
                        {
                            attributesWithInvalidValues = new List<Attribute>();
                        }
                        attributesWithInvalidValues.Add(sourceAttribute);
                    }
                }
            }
        }

        private void ProcessRelationshipAttributesWithInvalidValues(Dictionary<Int64, RelationshipOperationResultCollection> unsuccessfullRelationshipOperationResults,
            List<Attribute> attributesWithInvalidValues, RelationshipOperationResult relationshipOperationResult, Entity targetEntity)
        {
            if (!attributesWithInvalidValues.IsNullOrEmpty())
            {
                foreach (Attribute attribute in attributesWithInvalidValues)
                {
                    AttributeOperationResult attributeOperationResult = relationshipOperationResult.AttributeOperationResultCollection.FirstOrDefault(aor => aor.AttributeId == attribute.Id);

                    if (attributeOperationResult == null)
                    {
                        attributeOperationResult = new AttributeOperationResult(attribute.Id, attribute.Name, attribute.LongName, attribute.AttributeModelType, attribute.Locale);
                    }

                    attributeOperationResult.AddOperationResult(InvalidValuesErrorCode, String.Format(InvalidValuesErrorMessage, attribute.Name, attribute.AttributeParentName, targetEntity.Name),
                        String.Empty, new Collection<Object> {attribute.Name, attribute.AttributeParentName, targetEntity.Name}, InvalidValuesErrorType);
                }

                relationshipOperationResult.AttributeOperationResultCollection.RefreshOperationResultStatus();

                AddResultToDictionary(unsuccessfullRelationshipOperationResults, targetEntity.Id, relationshipOperationResult);
            }
        }

        private void AddResultToDictionary<T, TCollection>(Dictionary<Int64, TCollection> dictionary, Int64 key, T value)
            where T : IOperationResult
            where TCollection: ICollection<T>, new()
        {
            TCollection collection;
            if (dictionary.TryGetValue(key, out collection))
            {
                collection.Add(value);
            }
            else
            {
                collection = new TCollection { value };
                dictionary.Add(key, collection);
            }
        }

        private OperationResultStatusEnum GetEntityOperationResultStatus(OperationResultStatusEnum newStatus, OperationResultStatusEnum currentStatus)
        {
            OperationResultStatusEnum resultStatus = currentStatus;

            if (newStatus != OperationResultStatusEnum.Successful)
            {
                switch (currentStatus)
                {
                    case OperationResultStatusEnum.Failed:
                        break;
                    case OperationResultStatusEnum.CompletedWithErrors:
                        break;
                    case OperationResultStatusEnum.CompletedWithWarnings:
                        if (newStatus == OperationResultStatusEnum.Failed || newStatus == OperationResultStatusEnum.CompletedWithErrors)
                        {
                            resultStatus = newStatus;
                        }
                        break;
                    case OperationResultStatusEnum.Successful:
                    case OperationResultStatusEnum.None:
                        resultStatus = newStatus;
                        break;
                }
            }

            return resultStatus;
        }

        private Int32 CountSuccessfullOperationResults<T, TCollection>(TCollection operationResults)
            where T : IOperationResult
            where TCollection : ICollection <T>
        {
            const OperationResultStatusEnum successfullStatusToCount = OperationResultStatusEnum.Successful;
            const OperationResultStatusEnum noneStatusToCount = OperationResultStatusEnum.None;

            Int32 numberOfSuccessfullOperationResults = 0;
            foreach (T operationResult in operationResults)
            {
                if (operationResult.OperationResultStatus == successfullStatusToCount || operationResult.OperationResultStatus == noneStatusToCount)
                {
                    numberOfSuccessfullOperationResults++;
                }
            }

            return numberOfSuccessfullOperationResults;
        }

        private void CopyPasteComplexAttribute(Attribute sourceAttribute, Attribute targetAttribute)
        {
            targetAttribute.Attributes = sourceAttribute.Attributes;

            IAttributeCollection targetChildInstanceRecords = targetAttribute.GetChildAttributes();

            if (targetChildInstanceRecords != null && targetChildInstanceRecords.Count > 0)
            {
                foreach (Attribute complexAttributeInstanceRecord in targetChildInstanceRecords)
                {
                    IAttributeCollection childAttributes = complexAttributeInstanceRecord.GetChildAttributes();

                    if (childAttributes != null && childAttributes.Count > 0)
                    {
                        foreach (Attribute childAttribute in childAttributes)
                        {
                            Value currentValueInstance = (Value)childAttribute.GetCurrentValueInstanceInvariant();

                            if (currentValueInstance != null)
                            {
                                childAttribute.Locale = targetAttribute.Locale;
                                currentValueInstance.Locale = targetAttribute.Locale;
                                currentValueInstance.AttrVal = currentValueInstance.InvariantVal;
                                childAttribute.SetValue(currentValueInstance, targetAttribute.Locale);
                            }
                        }
                    }
                }
            }
        }


        /// <summary>
        /// Copy all sourceAttribute rows into targetAttribute. No duplicate check is performed.
        /// </summary>
        /// <param name="sourceAttribute"></param>
        /// <param name="targetAttribute"></param>
        private void CopyPasteHierarchicalAttribute(Attribute sourceAttribute, Attribute targetAttribute)
        {
            IAttributeCollection instaceRecords = sourceAttribute.GetChildAttributes(AttributeValueSource.Overridden);

            if (instaceRecords != null && instaceRecords.Count > 0)
            {
                Int32 instanceRefId = -1;

                foreach (Attribute instanceRecord in instaceRecords)
                {
                    IAttributeCollection childAttributes = instanceRecord.GetChildAttributes();

                    if (childAttributes != null && childAttributes.Count > 0)
                    {
                        targetAttribute.AddComplexChildRecord(childAttributes, --instanceRefId, AttributeValueSource.Overridden, sourceAttribute.Locale);
                        targetAttribute.Action = ObjectAction.Update;
                    }
                }

                HierarchicalAttribute.UpdateHierarchicalAttributeAction(targetAttribute, ObjectAction.Update);
            }
        }


        #endregion
    }
}
