using System;
using System.Linq;
using System.Collections.ObjectModel;
    
namespace MDM.EntityManager.Business.EntityOperations.Helpers
{
    using MDM.Core;
    using MDM.BusinessObjects;
    using BusinessObjects.Diagnostics;
    using MDM.Interfaces;

    /// <summary>
    /// Represents utility methods for calculating entity state validation and state attributes
    /// </summary>
    internal sealed class EntityStateValidationHelper
    {
        #region Methods

        #region Public Methods
       
        /// <summary>
        /// Prepares the entity state validations for given entities
        /// </summary>
        /// <param name="entities">Indicates entity collection.</param>
        /// <param name="entityOperationResults">Indicates entity operation results.</param>
        /// <param name="callerContext">Indicates Application and Module name by which action is being performed</param>
        /// <param name="traceSettings">Indicates the trace settings</param>
        /// <param name="addMissingEntities">Indicates whether add missing entities to be added or not</param>
        /// <returns>Returns the entity validation state collection which holds Error, Warning and Information related records</returns>
        public static EntityStateValidationCollection GetEntityStateValidations(EntityCollection entities, EntityOperationResultCollection entityOperationResults, CallerContext callerContext, TraceSettings traceSettings = null, Boolean addMissingEntities = true)
        {
            Boolean isTracingEnabled = traceSettings != null && traceSettings.IsBasicTracingEnabled;
            DiagnosticActivity diagnosticActivity = new DiagnosticActivity();
            EntityStateValidationCollection entityStateValidations = new EntityStateValidationCollection();
            Collection<Int64> missingEntityIds = new Collection<Int64>();
            EntityBL entityBL = new EntityBL();

            try
            {
                if (isTracingEnabled)
                {
                    diagnosticActivity.Start();
                }

                if (entityOperationResults != null && entities != null)
                {
                    foreach (EntityOperationResult entityOperationResult in entityOperationResults)
                    {
                        Entity entity = (Entity)entities.GetEntity(entityOperationResult.EntityId);

                        if (entity != null)
                        {
                            //We shouldn't record entity state validations  for categories as per design. Hence skipping them over here.
                            if (entity.EntityTypeId == Constants.CATEGORY_ENTITYTYPE)
                            {
                                continue;
                            }

                            if (isTracingEnabled)
                            {
                                diagnosticActivity.LogInformation(String.Format("Starting calculating state validations for Entity - Id: {0} ExternalId: {1}", entity.Id, entity.ExternalId));
                            }

                            if (isTracingEnabled)
                            {
                                diagnosticActivity.LogInformation(String.Format("Starting  processing entityOperationResult for setting state validation and preparing validation state collection for Entity - Id: {0} ExternalId: {1}", entity.Id, entity.ExternalId));
                            }

                            #region Prepare Entity State Validation collection for Error/ Warning / Information for given operation result

                            PrepareEntityStateValidationsFromOperationResult(entity, entityOperationResult, entityStateValidations, callerContext, traceSettings);

                            #endregion Preapare Entity State Validation collection for Error/ Warning / Information for given operation result
                        }
                        else if (addMissingEntities && !missingEntityIds.Contains(entityOperationResult.EntityId))
                        {
                            missingEntityIds.Add(entityOperationResult.EntityId);
                        }
                    }

                    #region Minimum extension for parent

                    if (missingEntityIds.Count > 0)
                    {
                        EntityCollection missingEntities = entityBL.Get(missingEntityIds, new EntityContext(), new EntityGetOptions { PublishEvents = false, ApplyAVS = false, ApplySecurity = false }, callerContext);

                        if (missingEntities != null && missingEntities.Count > 0)
                        {
                            foreach (Entity entity in missingEntities)
                            {
                                EntityOperationResult entityOperationResult = (EntityOperationResult)entityOperationResults.GetByEntityId(entity.Id);

                                if (entityOperationResult != null)
                                {
                                    PrepareEntityStateValidationsFromOperationResult(entity, entityOperationResult, entityStateValidations, callerContext, traceSettings);
                                }

                                //add the entity to entity collection. This enables the parent entity to be sent for jigsaw processing
                                entities.Add(entity);
                            }
                        }
                    }

                    #endregion Minimum extension for parent
                }
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
        /// Loads the state validation attributes into entity collection
        /// </summary>
        /// <param name="entities">Indicates entity collection</param>
        /// <param name="traceSettings">Indicates the trace settings</param>
        public static void LoadStateValidationAttributes(EntityCollection entities, TraceSettings traceSettings = null)
        {
            AttributeModelCollection attributeModels = EntityAttributeModelHelper.GetStateValidationAttributeModels(traceSettings);

            if (attributeModels != null && attributeModels.Count > 0)
            {
                AttributeCollection stateValidationAttributes = new AttributeCollection(attributeModels);

                foreach (Entity entity in entities)
                {
                    //If current entity is category then skip loading of validation states attributes
                    if (entity.EntityTypeId == Constants.CATEGORY_ENTITYTYPE)
                    {
                        continue;
                    }

                    entity.Attributes.AddRange(stateValidationAttributes, true);
                }
            }
        }

        #endregion Public Methods

        #region Private Methods
    
        /// <summary>
        /// Prepares the entity state validation from operation result.
        /// </summary>
        /// <param name="entity"> Indicates the entity.</param>
        /// <param name="entityOperationResult"> Indicates the entity operation result.</param>
        /// <param name="entityStateValidations"> Indicates the entity validation states.</param>
        /// <param name="callerContext"> Indicates the caller context.</param>
        /// <param name="traceSettings"> Indicates the trace settings.</param>
        private static void PrepareEntityStateValidationsFromOperationResult(Entity entity, EntityOperationResult entityOperationResult, EntityStateValidationCollection entityStateValidations, CallerContext callerContext, TraceSettings traceSettings = null)
        {
            Boolean isTracingEnabled = traceSettings != null && traceSettings.IsBasicTracingEnabled;
            DiagnosticActivity diagnosticActivity = new DiagnosticActivity();

            try
            {
                if (isTracingEnabled)
                {
                    diagnosticActivity.Start();
                }

                if (entity != null && entityOperationResult != null)
                {
                    #region Populate Error / Warning / Information for MetaData attribute

                    PopulateEntityStateValidations(entityOperationResult, entityStateValidations, SystemAttributes.EntityMetaDataValid, entity.Id, entity.ContainerId, jobId: callerContext.JobId, locale: entity.Locale);

                    #endregion Populate Error / Warning / Information for MetaData system attribute

                    #region Populate Error / Warning / Information for (Category / Common ) attribute

                    if (entityOperationResult.AttributeOperationResultCollection != null && entityOperationResult.AttributeOperationResultCollection.Count() > 0)
                    {
                        #region Populate Error / Warning / Information for Common attribute

                        AttributeOperationResultCollection commonAttributeOperationResults = entityOperationResult.AttributeOperationResultCollection.Get(AttributeModelType.Common);

                        if (commonAttributeOperationResults != null && commonAttributeOperationResults.Count() > 0)
                        {
                            foreach (AttributeOperationResult commonAttributeOperationResult in commonAttributeOperationResults)
                            {
                                Attribute commonAttribute = (Attribute)entity.GetAttribute(commonAttributeOperationResult.AttributeId, commonAttributeOperationResult.Locale);

                                PopulateEntityStateValidations(commonAttributeOperationResult, entityStateValidations, SystemAttributes.EntityCommonAttributesValid, entity.Id, entity.ContainerId, jobId: callerContext.JobId, locale: commonAttributeOperationResult.Locale, attribute: commonAttribute);
                            }
                        }

                        #endregion Populate Error / Warning / Information for Common system attribute

                        #region Populate Error / Warning / Information for Category attribute

                        AttributeOperationResultCollection categoryAttributeOperationResults = entityOperationResult.AttributeOperationResultCollection.Get(AttributeModelType.Category);

                        if (categoryAttributeOperationResults != null && categoryAttributeOperationResults.Count() > 0)
                        {
                            foreach (AttributeOperationResult categoryAttributeOperationResult in categoryAttributeOperationResults)
                            {
                                Attribute categoryAttribute = (Attribute)entity.GetAttribute(categoryAttributeOperationResult.AttributeId, categoryAttributeOperationResult.Locale);

                                PopulateEntityStateValidations(categoryAttributeOperationResult, entityStateValidations, SystemAttributes.EntityCategoryAttributesValid, entity.Id, entity.ContainerId, jobId: callerContext.JobId, locale: categoryAttributeOperationResult.Locale, attribute: categoryAttribute);
                            }
                        }

                        #endregion Populate Error / Warning / Information for Category system attribute
                    }

                    #endregion Populate Error / Warning / Information for (Category / Common ) attribute

                    #region Populate Error / Warning / Information for Relationships and relationship attributes

                    if (entityOperationResult.RelationshipOperationResultCollection != null && entityOperationResult.RelationshipOperationResultCollection.Count > 0)
                    {
                        PopulateRelationshipValue(entityOperationResult.RelationshipOperationResultCollection, entityStateValidations, entity.Id, entity.ContainerId, entity.Relationships);
                    }

                    #endregion Populate Error / Warning / Information for Relationships and relationship attributes

                    #region Refresh Results

                    entityOperationResult.AttributeOperationResultCollection.RefreshOperationResultStatus();
                    entityOperationResult.RelationshipOperationResultCollection.RefreshOperationResultStatus();
                    entityOperationResult.RefreshOperationResultStatus();

                    #endregion Refresh Results
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
        /// Populates the state of the entity validation.
        /// </summary>
        /// <param name="operationResult"> Indicates the operation result.</param>
        /// <param name="entityStateValidations"> Indicates the entity validation states.</param>
        /// <param name="stateValidationSystemAttribute"> Indicates the validation state system attribute.</param>
        /// <param name="entityId"> Indicates the entity identifier.</param>
        /// <param name="containerId"> Indicates the container identifier.</param>
        /// <param name="locale">Indicates the locale for entity state validation</param>
        /// <param name="relationshipId"> Indicates the relationship identifier.</param>
        /// <param name="relationshipAttributeId"> Indicates the relationship attribute identifier.</param>
        /// <param name="jobId">Indicates the job id if any.</param>
        /// <param name="attribute">Indicates attribute instance</param>
        private static void PopulateEntityStateValidations(OperationResult operationResult, EntityStateValidationCollection entityStateValidations, SystemAttributes stateValidationSystemAttribute, Int64 entityId, Int32 containerId, LocaleEnum locale, Int64 relationshipId = -1, Int32 relationshipAttributeId = -1, Int32 jobId = -1, Attribute attribute = null)
        {
            //If attribute isn't changed and we don't have any errors/informations/warnings then treat them as unchanged attribute. 
            //and don't touch the existing state validation for unchanged attribute.
            //In case system has to clear any existing errors for requested attribute then attribute should be changed or it has to pass some information for that attribute.
            if (attribute != null && attribute.Action == ObjectAction.Read && !operationResult.HasError && !operationResult.HasWarnings && !operationResult.HasInformation)
            {
                return;
            }

            #region  Populate Error and Passed  Data

            if (operationResult.HasError)
            {
                #region Populate Error Data

                ErrorCollection errorsToBeRemoved = new ErrorCollection();

                foreach (Error error in operationResult.Errors)
                {
                    EntityStateValidation entityStateValidation = new EntityStateValidation()
                    {
                        SystemValidationStateAttribute = stateValidationSystemAttribute,
                        EntityId = entityId,
                        ContainerId = containerId,
                        ReasonType = error.ReasonType,
                        MessageCode = error.ErrorCode,
                        MessageParameters = error.Params,
                        RuleMapContextId = error.RuleMapContextId,
                        RuleId = error.RuleId,
                        Action = ObjectAction.Create,
                        JobId = jobId,
                        OperationResultType = OperationResultType.Error,
                        Locale = locale
                    };

                    PopulateAttributeValue(operationResult, stateValidationSystemAttribute, entityStateValidation, relationshipId, relationshipAttributeId);
                    entityStateValidations.Add(entityStateValidation);

                    if (error.IgnoreError == true)
                    {
                        errorsToBeRemoved.Add(error);
                    }
                }

                if (errorsToBeRemoved.Count > 0)
                {
                    foreach (Error error in errorsToBeRemoved)
                    {
                        operationResult.Errors.Remove(error);
                    }
                    operationResult.RefreshOperationResultStatus();
                }

                #endregion
            }
            else
            {
                #region Populate Passed Data

                EntityStateValidation entityStateValidation = new EntityStateValidation()
                {
                    SystemValidationStateAttribute = stateValidationSystemAttribute,
                    EntityId = entityId,
                    ContainerId = containerId,
                    JobId = jobId,
                    Action = ObjectAction.Delete,
                    OperationResultType = OperationResultType.Error,
                    Locale = locale
                };

                PopulateAttributeValue(operationResult, stateValidationSystemAttribute, entityStateValidation, relationshipId, relationshipAttributeId);
                entityStateValidations.Add(entityStateValidation);

                #endregion
            }

            #endregion Populate Error Data

            #region  Populate Warning Data

            if (operationResult.HasWarnings)
            {
                foreach (Warning warning in operationResult.Warnings)
                {
                    EntityStateValidation entityStateValidation = new EntityStateValidation()
                    {
                        SystemValidationStateAttribute = stateValidationSystemAttribute,
                        EntityId = entityId,
                        ContainerId = containerId,
                        ReasonType = warning.ReasonType,
                        MessageCode = warning.WarningCode,
                        MessageParameters = warning.Params,
                        RuleMapContextId = warning.RuleMapContextId,
                        RuleId = warning.RuleId,
                        JobId = jobId,
                        Action = ObjectAction.Create,
                        OperationResultType = OperationResultType.Warning,
                        Locale = locale
                    };

                    PopulateAttributeValue(operationResult, stateValidationSystemAttribute, entityStateValidation, relationshipId, relationshipAttributeId);
                    entityStateValidations.Add(entityStateValidation);
                }
            }

            #endregion Populate Warning Data

            #region  Populate Information Data

            if (operationResult.HasInformation)
            {
                foreach (Information information in operationResult.Informations)
                {
                    EntityStateValidation entityStateValidation = new EntityStateValidation()
                    {
                        SystemValidationStateAttribute = stateValidationSystemAttribute,
                        EntityId = entityId,
                        ContainerId = containerId,
                        ReasonType = information.ReasonType,
                        MessageCode = information.InformationCode,
                        MessageParameters = information.Params,
                        RuleMapContextId = information.RuleMapContextId,
                        RuleId = information.RuleId,
                        JobId = jobId,
                        Action = ObjectAction.Create,
                        OperationResultType = OperationResultType.Information,
                        Locale = locale
                    };

                    PopulateAttributeValue(operationResult, stateValidationSystemAttribute, entityStateValidation, relationshipId, relationshipAttributeId);
                    entityStateValidations.Add(entityStateValidation);
                }
            }

            #endregion Populate information Data
        }

        /// <summary>
        /// Populates the attribute value.
        /// </summary>
        /// <param name="operationResult"> Indicates the operation result.</param>
        /// <param name="validationStateSystemAttribute"> Indicates the validation state system attribute.</param>
        /// <param name="entityStateValidation">State of the entity validation.</param>
        /// <param name="relationshipId"> Indicates the relationship identifier.</param>
        /// <param name="relationshipAttributeId"> Indicates the relationship attribute identifier.</param>
        private static void PopulateAttributeValue(OperationResult operationResult, SystemAttributes validationStateSystemAttribute, EntityStateValidation entityStateValidation, Int64 relationshipId = -1, Int32 relationshipAttributeId = -1)
        {
            switch (validationStateSystemAttribute)
            {
                case SystemAttributes.EntityMetaDataValid:
                    entityStateValidation.AttributeId = (Int32)validationStateSystemAttribute;
                    break;
                case SystemAttributes.EntityCommonAttributesValid:
                    if (operationResult is AttributeOperationResult)
                    {
                        entityStateValidation.AttributeModelType = AttributeModelType.Common;
                        entityStateValidation.AttributeId = ((AttributeOperationResult)operationResult).AttributeId;
                        entityStateValidation.AttributeName = ((AttributeOperationResult)operationResult).AttributeShortName;
                    }
                    break;
                case SystemAttributes.EntityCategoryAttributesValid:
                    if (operationResult is AttributeOperationResult)
                    {
                        entityStateValidation.AttributeModelType = AttributeModelType.Category;
                        entityStateValidation.AttributeId = ((AttributeOperationResult)operationResult).AttributeId;
                        entityStateValidation.AttributeName = ((AttributeOperationResult)operationResult).AttributeShortName;
                    }
                    break;
                case SystemAttributes.EntityRelationshipsValid:
                    entityStateValidation.AttributeModelType = AttributeModelType.Relationship;
                    entityStateValidation.RelationshipId = relationshipId;
                    entityStateValidation.AttributeId = relationshipAttributeId;
                    break;
            }
        }

        /// <summary>
        /// Populates the relationship value.
        /// </summary>
        /// <param name="relationshipOperationResults"> Indicates the relationship operation results.</param>
        /// <param name="entityStateValidations"> Indicates the entity validation states.</param>
        /// <param name="entityId"> Indicates the entity identifier.</param>
        /// <param name="containerId"> Indicates the container identifier.</param>
        /// <param name="entityRelationships">Indicates relationships</param>
        private static void PopulateRelationshipValue(RelationshipOperationResultCollection relationshipOperationResults, EntityStateValidationCollection entityStateValidations, Int64 entityId, Int32 containerId, RelationshipCollection entityRelationships)
        {
            if (relationshipOperationResults != null && relationshipOperationResults.Count > 0)
            {
                foreach (RelationshipOperationResult relationshipOperationResult in relationshipOperationResults)
                {
                    if (entityRelationships != null && entityRelationships.Count > 0)
                    {
                        Relationship relationship = (Relationship)entityRelationships.GetRelationshipById(relationshipOperationResult.RelationshipId);

                        if (relationship != null)
                        {
                            PopulateEntityStateValidations(relationshipOperationResult, entityStateValidations, SystemAttributes.EntityRelationshipsValid, entityId, containerId, relationship.Locale, relationshipOperationResult.RelationshipId);

                            if (relationshipOperationResult.AttributeOperationResultCollection != null && relationshipOperationResult.AttributeOperationResultCollection.Count > 0)
                            {
                                foreach (AttributeOperationResult relationshipAttributeOperationResult in relationshipOperationResult.AttributeOperationResultCollection)
                                {
                                    Attribute relationshipAttribute = (Attribute)relationship.RelationshipAttributes.GetAttribute(relationshipAttributeOperationResult.AttributeId, relationshipAttributeOperationResult.Locale);

                                    PopulateEntityStateValidations(relationshipAttributeOperationResult, entityStateValidations, SystemAttributes.EntityRelationshipsValid, entityId, containerId, relationshipAttributeOperationResult.Locale, relationshipOperationResult.RelationshipId, relationshipAttributeOperationResult.AttributeId, attribute: relationshipAttribute);
                                }
                            }

                            if (relationshipOperationResult.RelationshipOperationResultCollection != null && relationshipOperationResult.RelationshipOperationResultCollection.Count > 0)
                            {
                                //Recursive call 
                                PopulateRelationshipValue(relationshipOperationResult.RelationshipOperationResultCollection, entityStateValidations, entityId, containerId, relationship.RelationshipCollection);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Determines whether the specified attribute  model type collection has error or not.
        /// </summary> 
        /// <param name="attributeOperationResults">Specifies attribute operation results.</param>
        /// <param name="attributeModelType">Specifies type of the attribute model.</param>
        /// <returns>'true' if attribute operation result does have error;otherwise 'false'</returns>
        private static Boolean HasError(AttributeOperationResultCollection attributeOperationResults, AttributeModelType attributeModelType)
        {
            foreach (AttributeOperationResult attributeOperationResult in attributeOperationResults)
            {
                if (attributeOperationResult.AttributeModelType == attributeModelType && attributeOperationResult.HasError)
                {
                    return true;
                }
            }

            return false;
        }       

        #endregion Private Methods

        #endregion Methods
    }
}