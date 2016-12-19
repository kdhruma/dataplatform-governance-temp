using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace MDM.Utility
{
    using BusinessObjects.DQM;
    using MDM.BusinessObjects;
    using MDM.BusinessObjects.Diagnostics;
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// This is helper class for entity operations.
    /// </summary>
    public sealed class EntityOperationsHelper
    {
        #region Fields

        /// <summary>
        /// Defines the pre-defined collection of reason type.
        /// </summary>
        private static readonly Collection<ReasonType> _reasonTypes = new Collection<ReasonType>() { ReasonType.InvalidDataLocale, ReasonType.PermissionCheck };

        #endregion Fields

        #region Methods

        #region Public Methods

        /// <summary>
        /// Performs scan and filter of entities based on results and returns boolean flag to continue process.
        /// </summary>
        /// <param name="contextEntities">Specifies context based entities collection.</param>
        /// <param name="allEntities">Specifies all entities collection bucket.</param>
        /// <param name="entityOperationResults">Specifies  entity operation results collection.</param>
        /// <param name="callerContext">Specifies the Caller context</param>
        /// <returns>'true' if operation is successful;otherwise 'false'.</returns>
        public static Boolean ScanAndFilterEntitiesBasedOnResults(EntityCollection contextEntities, EntityCollection allEntities, EntityOperationResultCollection entityOperationResults, CallerContext callerContext = null)
        {
            Boolean continueProcess = true;

            foreach (EntityOperationResult entityOperationResult in entityOperationResults)
            {
                Entity contextEntity = (Entity)contextEntities.GetEntity(entityOperationResult.EntityId);

                if (contextEntity != null)
                {
                    Int64 entityId = contextEntity.Id;

                    //Remove entity from processing for following scenarios:
                    //1.If any entities meta data is failed then remove from processing
                    //2.If category comes for create/update/delete operation, failed with any errors then also from processing.
                    if (entityOperationResult.HasError)
                    {
                        contextEntities.Remove(entityId);

                        if (contextEntity.Id < 1 && allEntities != null)
                        {
                            allEntities.Remove(entityId);

                            //Remove ignore errors only in case of Entity Create (others will be part of entity statevalidation will take care) & only for UI 
                            Boolean anyErrorsRemoved = RemoveIgnoreErrors(entityOperationResult, callerContext);

                            if (anyErrorsRemoved == true)
                            {
                                entityOperationResults.RefreshOperationResultStatus();
                            }
                        }

                        continue;
                    }
                    else if (contextEntity.Relationships != null && contextEntity.Relationships.Count > 0)
                    {
                        //if there are any Relationship errors at entity level clear the relationships from the entity processing
                        //Regarding cardinality error, the error is added at top level so cannot remove specific relationship from the collection. 
                        if (entityOperationResult.RelationshipOperationResultCollection.HasError)
                        {
                            contextEntity.Relationships.Clear();
                        }
                        else
                        {
                            //If any of the relationship has error in meta data, remove from the entities list. 
                            foreach (RelationshipOperationResult relationshipOperationResult in entityOperationResult.RelationshipOperationResultCollection)
                            {
                                if (relationshipOperationResult.HasError)
                                {
                                    Relationship existingRelationships = (Relationship)contextEntity.Relationships.GetRelationshipById(relationshipOperationResult.RelationshipId);

                                    if (existingRelationships != null)
                                    {
                                        contextEntity.Relationships.Remove(existingRelationships);
                                    }
                                }
                            }
                        }
                    }
                    else if (contextEntity.EntityTypeId == Constants.CATEGORY_ENTITYTYPE)
                    {
                        ScanAndFilterCategoriesBasedOnResult(contextEntities, allEntities, contextEntity, entityOperationResult);
                    }

                    if (entityOperationResult.OperationResultStatus == OperationResultStatusEnum.Failed ||
                        entityOperationResult.OperationResultStatus == OperationResultStatusEnum.CompletedWithErrors ||
                        entityOperationResult.OperationResultStatus == OperationResultStatusEnum.CompletedWithWarnings)
                    {
                        ScanAndFilterAttributesBasedOnReasonType(contextEntity.Attributes, entityOperationResult.AttributeOperationResultCollection);
                        ScanAndFilterRelationshipsBasedOnReasonType(contextEntity.Relationships, entityOperationResult.RelationshipOperationResultCollection);
                    }
                }
            }

            if (allEntities != null && allEntities.Count < 1)
            {
                continueProcess = false;
            }

            return continueProcess;
        }

        /// <summary>
        /// Gets the pre-defined entity state validation attributes from system attributes enum.
        /// </summary>
        /// <returns>Returns state validation attribute id list</returns>
        public static Collection<Int32> GetStateValidationAttributes()
        {
            Collection<Int32> stateValidationAttributeIdList = new Collection<Int32>()
            {
                (Int32)SystemAttributes.EntitySelfValid,
                (Int32)SystemAttributes.EntityMetaDataValid,
                (Int32)SystemAttributes.EntityCommonAttributesValid,
                (Int32)SystemAttributes.EntityCategoryAttributesValid,
                (Int32)SystemAttributes.EntityRelationshipsValid,
                (Int32)SystemAttributes.EntityVariantValid,
                (Int32)SystemAttributes.EntityExtensionsValid
            };

            return stateValidationAttributeIdList;
        }

        /// <summary>
        /// Performs validation for import as well as entity validation
        /// </summary>
        /// <param name="entity">Indicates entity to be validated</param>
        /// <param name="relationship">Indicates relationship to be validated</param>
        /// <param name="attribute">Indicates attribute to be validated</param>
        /// <param name="attributeModel">Indicates attribute model for the given attribute</param>
        /// <param name="attributeOperationResult">Indicates attribute operation result</param>
        /// <param name="entityProcessingOptions">Indicates entity processing options</param>
        /// <param name="callerContext">Indicates caller context</param>
        /// <param name="localeMessageBL">Indicates locale message manager</param>
        /// <param name="isParentComplex">Indicates whether parent is complex or not</param>
        public static void ValidateEntityAttributesForImport(Entity entity, Relationship relationship, Attribute attribute, AttributeModel attributeModel, AttributeOperationResult attributeOperationResult, EntityProcessingOptions entityProcessingOptions, CallerContext callerContext, ILocaleMessageManager localeMessageBL, Boolean isParentComplex = false)
        {
            DiagnosticActivity diagnosticActivity = null;
            Boolean isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;
            LocaleEnum systemUILocale = GlobalizationHelper.GetSystemUILocale();

            if (isTracingEnabled)
            {
                diagnosticActivity = new DiagnosticActivity();
                diagnosticActivity.Start();
            }

            String message = String.Empty;
            String messageCode = String.Empty;
            String errorMessage = String.Empty;
            Int64 entityReferenceId = entity.ReferenceId;
            String errorMessagePrefix = String.Empty;
            String attributeNameForDisplay = String.IsNullOrWhiteSpace(attribute.LongName) ? attribute.Name : attribute.LongName;
            String attributeParentNameForDisplay = String.IsNullOrWhiteSpace(attribute.AttributeParentLongName) ? attribute.AttributeParentName : attribute.AttributeParentLongName;

            if (entityProcessingOptions.ImportMode == ImportMode.InitialLoad ||
                entityProcessingOptions.ImportMode == ImportMode.RelationshipInitialLoad)
            {
                errorMessagePrefix = localeMessageBL.Get(systemUILocale, "112020", false, callerContext).Message;
            }
            else
            {
                errorMessagePrefix = localeMessageBL.Get(systemUILocale, "112019", false, callerContext).Message;
            }

            try
            {
                if (String.IsNullOrEmpty(attribute.Name))
                {
                    // Mark as error

                    errorMessage = String.Format(localeMessageBL.Get(systemUILocale, "111679", false, callerContext).Message, errorMessagePrefix, entityReferenceId); //Name is empty or not specified.

                    attributeOperationResult.AddOperationResult("111679", errorMessage, new Collection<Object> { errorMessagePrefix, entityReferenceId }, ReasonType.AttributeCheck, -1, -1, OperationResultType.Error);
                }

                if (String.IsNullOrEmpty(attribute.AttributeParentName))
                {
                    // Mark as error

                    errorMessage = String.Format(localeMessageBL.Get(systemUILocale, "111687", false, callerContext).Message, errorMessagePrefix, entityReferenceId); //AttributeParentName is empty or not specified.

                    if (isTracingEnabled)
                    {
                        diagnosticActivity.LogError(errorMessage);
                    }

                    attributeOperationResult.AddOperationResult("111687", errorMessage, new Collection<Object> { errorMessagePrefix, entityReferenceId }, ReasonType.AttributeCheck, -1, -1, OperationResultType.Error);
                }

                if (attribute.AttributeDataType == AttributeDataType.File || attribute.AttributeDataType == AttributeDataType.Image)
                {
                    String warningMessage = String.Empty;

                    if (isParentComplex)
                    {
                        //Message: {0}{1} Complex attribute '{2}' is removed from processing as attribute '{2}/{3}' data type '{4}' is not supported for import.
                        //Sample: Line#2: Complex attribute 'Address' is removed from processing as attribute 'Address/AddressImage' data type 'Image' is not supported for import.
                        messageCode = "114030";
                        warningMessage = String.Format(localeMessageBL.Get(systemUILocale, messageCode, false, callerContext).Message,
                                            errorMessagePrefix, entityReferenceId, attributeParentNameForDisplay, attributeNameForDisplay,
                                            attribute.AttributeDataType.ToString());
                    }
                    else
                    {
                        //Message: {0}{1} Attribute '{2}/{3}' value is ignored as data type: {4} is not supported for import.
                        //Sample: Line#2: Attribute 'A Group/imageAttrCommon' value is ignored as data type: Image is not supported for import.
                        messageCode = "114026";
                        warningMessage = String.Format(localeMessageBL.Get(systemUILocale, messageCode, false, callerContext).Message,
                            errorMessagePrefix, entityReferenceId, attributeParentNameForDisplay, attributeNameForDisplay, attribute.AttributeDataType.ToString());
                    }

                    if (isTracingEnabled)
                    {
                        diagnosticActivity.LogWarning(errorMessage);
                    }

                    Collection<Object> parameters = new Collection<Object>() { errorMessagePrefix, entityReferenceId, attributeParentNameForDisplay, 
                                                                                attributeNameForDisplay, attribute.AttributeDataType.ToString() };

                    attributeOperationResult.AddOperationResult(messageCode, warningMessage, parameters, ReasonType.AttributeCheck, -1, -1, OperationResultType.Warning);
                }

                //TODO :: need to check this to skip validations
                if (attribute.IsComplex)
                {
                    if (attribute.IsHierarchical)
                    {
                        var attributeOperationResultCollection = new AttributeOperationResultCollection();

                        if (attributeModel == null)
                        {
                            message = "Could not get Attribute model for the hierarchical attribute {0}:{1} from Cached Attribute models.  Could not validate the  attribute.";

                            message = String.Format("Job Id {0}. Internal Error: {1}", callerContext.JobId, String.Format(message, attribute.AttributeParentName, attribute.Name));

                            if (isTracingEnabled)
                            {
                                diagnosticActivity.LogError(errorMessage);
                            }
                        }
                        else
                        {
                            //Validate the hierarchy attribute with their Attribute Models.
                            attributeOperationResultCollection = attribute.ValidateHierarchicalAttribute(attributeModel, false);
                        }

                        if (attributeOperationResultCollection.Any())
                        {
                            messageCode = "114081";
                            var warningMessage =
                                String.Format(
                                    localeMessageBL.Get(systemUILocale, messageCode, false, callerContext).Message,
                                    errorMessagePrefix, entityReferenceId, attributeParentNameForDisplay,
                                    attributeNameForDisplay, attribute.AttributeDataType.ToString());

                            foreach (var or in attributeOperationResultCollection)
                            {
                                if (isTracingEnabled)
                                {
                                    Collection<Object> parameters = new Collection<Object>()
                                {
                                    errorMessagePrefix,
                                    entityReferenceId,
                                    attributeParentNameForDisplay,
                                    or.AttributeLongName,
                                    attribute.AttributeDataType.ToString()
                                };

                                    attributeOperationResult.AddOperationResult(messageCode, warningMessage, parameters, ReasonType.AttributeCheck, -1, -1, OperationResultType.Warning);
                                }
                            }
                        }

                        //Validate the child attributes
                        foreach (Attribute childAttributeInstance in attribute.Attributes)
                        {
                            //Validate for Attribute at Hierarchy level
                            ValidateForDuplicateAttributes(childAttributeInstance.Attributes, attributeOperationResult);

                            AttributeModelCollection modelCollection = (AttributeModelCollection) attributeModel.GetChildAttributeModels();

                            foreach (var childAttribute in childAttributeInstance.Attributes)
                            {
                                if (modelCollection != null)
                                {
                                    AttributeModel childAttributeModel = (AttributeModel) modelCollection.GetAttributeModel(childAttribute.Id, childAttribute.Locale);

                                    if (childAttributeModel != null)
                                    {
                                        ValidateEntityAttributesForImport(entity, relationship, childAttribute, childAttributeModel, attributeOperationResult, entityProcessingOptions, callerContext, localeMessageBL, isParentComplex: true);
                                    }
                                }
                            }
                        }
                    }
                    else //Complex
                    {
                        if (attribute.Attributes != null && attribute.Attributes.Count > 0)
                        {
                            foreach (Attribute instanceRecord in attribute.Attributes)
                            {
                                Boolean hasError = false;

                                if (instanceRecord.Attributes != null || instanceRecord.Attributes.Count > 0)
                                {
                                    foreach (Attribute childAttr in instanceRecord.Attributes)
                                    {
                                        if (!(attribute.Action == ObjectAction.Delete || attribute.Action == ObjectAction.Ignore))
                                        {
                                            ValidateEntityAttributesForImport(entity, relationship, childAttr, attributeModel, attributeOperationResult, entityProcessingOptions, callerContext, localeMessageBL, isParentComplex: true);
                                        }
                                    }
                                }

                                if (hasError)
                                    break;
                            }
                        }
                    }
                }

                // Check the value sequence. For collection attribute make sure the sequence is set...
                // for others it is not
                foreach (Value val in attribute.GetCurrentValuesInvariant())
                {
                    // Do the following validation only when action is not delete or ignore.
                    if (val.Action != ObjectAction.Delete && val.Action != ObjectAction.Ignore)
                    {
                        if (attribute.IsCollection)
                        {
                            if (val.Sequence < 0)
                            {
                                // this should not happen..error..
                                errorMessage = String.Format(localeMessageBL.Get(systemUILocale, "111690", false, callerContext).Message, errorMessagePrefix, entityReferenceId, attributeParentNameForDisplay, attributeNameForDisplay); // Attribute '{0}/{1}' is defined as collection but has no sequence
                                if (isTracingEnabled)
                                {
                                    diagnosticActivity.LogError(errorMessage);
                                }

                                attributeOperationResult.AddOperationResult("111690", errorMessage, new Collection<Object> { errorMessagePrefix, entityReferenceId, attributeParentNameForDisplay, attributeNameForDisplay }, ReasonType.AttributeCheck, -1, -1, OperationResultType.Error);
                            }
                        }
                        else
                        {
                            if (val.Sequence != -1)
                            {
                                // this should not happen..error..

                                errorMessage = String.Format(localeMessageBL.Get(systemUILocale, "111691", false, callerContext).Message, errorMessagePrefix, entityReferenceId, attributeParentNameForDisplay, attributeNameForDisplay); //Attribute '{0}/{1}' is defined as not a collection but has sequence

                                if (isTracingEnabled)
                                {
                                    diagnosticActivity.LogError(errorMessage);
                                }

                                attributeOperationResult.AddOperationResult("111691", errorMessage, new Collection<Object> { errorMessagePrefix, entityReferenceId, attributeParentNameForDisplay, attributeNameForDisplay }, ReasonType.AttributeCheck, -1, -1, OperationResultType.Error);
                            }
                        }

                        if (attribute.IsLookup)
                        {
                            if (val.AttrVal != null && !String.IsNullOrWhiteSpace(val.AttrVal.ToString()) && val.ValueRefId < 1)
                            {
                                // this should not happen..error..
                                attribute.HasInvalidValues = true;

                                if (attribute.IsCollection)
                                {
                                    errorMessage = String.Format(localeMessageBL.Get(systemUILocale, "112476", false, callerContext).Message, errorMessagePrefix, entityReferenceId, attributeParentNameForDisplay, attributeNameForDisplay, attribute.AttributeDataType); //{0}{1}Attribute '{2}/{3}' has one or more invalid values for its datatype: {4}.

                                    if (isTracingEnabled)
                                    {
                                        diagnosticActivity.LogError(errorMessage);
                                    }

                                    attributeOperationResult.AddOperationResult("112476", errorMessage, new Collection<Object> { errorMessagePrefix, entityReferenceId, attributeParentNameForDisplay, attributeNameForDisplay, attribute.AttributeDataType }, ReasonType.AttributeCheck, -1, -1, OperationResultType.Error);//{0}{1}Attribute '{2}/{3}' has one or more invalid values for its datatype: {4}.
                                }
                                else
                                {
                                    errorMessage = String.Format(localeMessageBL.Get(systemUILocale, "111692", false, callerContext).Message, errorMessagePrefix, entityReferenceId, attributeParentNameForDisplay, attributeNameForDisplay, val.AttrVal); //{0}{1} The lookup table for attribute '{2}/{3}' does not have the specified value: {4}. Either add this value in the lookup table or provide a correct value.
                                    if (isTracingEnabled)
                                    {
                                        diagnosticActivity.LogError(errorMessage);
                                    }

                                    attributeOperationResult.AddOperationResult("111692", errorMessage, new Collection<Object> { errorMessagePrefix, entityReferenceId, attributeParentNameForDisplay, attributeNameForDisplay, val.AttrVal }, ReasonType.AttributeCheck, -1, -1, OperationResultType.Error);//{0}{1} The lookup table for attribute '{2}/{3}' does not have the specified value: {4}. Either add this value in the lookup table or provide a correct value.
                                }
                            }
                        }
                    }

                    if (attribute.IsLocalizable)
                    {
                        if (val.Locale == LocaleEnum.UnKnown)
                        {
                            // item is localizable and locale is empty..error..

                            errorMessage = String.Format(localeMessageBL.Get(systemUILocale, "111694", false, callerContext).Message, errorMessagePrefix, entityReferenceId, attributeParentNameForDisplay, attributeNameForDisplay); //Attribute '{0}/{1}' is localizable but locale is not specified

                            if (isTracingEnabled)
                            {
                                diagnosticActivity.LogError(errorMessage);
                            }
                            attributeOperationResult.AddOperationResult("111694", errorMessage, new Collection<Object> { errorMessagePrefix, entityReferenceId, attributeParentNameForDisplay, attributeNameForDisplay }, ReasonType.AttributeCheck, -1, -1, OperationResultType.Error);
                        }
                    }
                }

                // make sure if the attribute is not collection it cannot have multiple values.
                if (!attribute.IsCollection)
                {
                    if (attribute.GetCurrentValuesInvariant().Count > 1)
                    {
                        // item is not a collection and it has multiple values.

                        errorMessage = String.Format(localeMessageBL.Get(systemUILocale, "100050", false, callerContext).Message, errorMessagePrefix, entityReferenceId, String.Format("{0}\\{1}", attributeParentNameForDisplay, attributeNameForDisplay)); //Attribute '{0}/{1}' is localizable but locale is not specified

                        if (isTracingEnabled)
                        {
                            diagnosticActivity.LogError(errorMessage);
                        }

                        attributeOperationResult.AddOperationResult("100050", errorMessage, new Collection<Object> { errorMessagePrefix, entityReferenceId, attributeParentNameForDisplay, attributeNameForDisplay }, ReasonType.AttributeCheck, -1, -1, OperationResultType.Error);
                    }
                }
            }
            finally
            {
                if (diagnosticActivity != null)
                {
                    diagnosticActivity.Stop();
                }
            }
        }

        /// <summary>
        /// Performs validation for unmapped attributes for import 
        /// </summary>
        /// <param name="entity">Indicates entity to be validated</param>
        /// <param name="attribute">Indicates attribute to be validated</param>
        /// <param name="relationship">Indicates relationship to be validated</param>
        /// <param name="attributeOperationResult">Indicates attribute operation result</param>
        /// <param name="localeMessageBL">Indicates locale message manager</param>
        /// <param name="entityProcessingOptions">Indicates entity processing options</param>
        /// <param name="callerContext">Indicates the caller context specifying the caller application and module</param>
        /// <param name="operationResultType">Indicates type of operation result</param>
        public static void ValidateUnmappedAttributes(Entity entity, Attribute attribute, Relationship relationship, AttributeOperationResult attributeOperationResult,
                                                        ILocaleMessageManager localeMessageBL, EntityProcessingOptions entityProcessingOptions, CallerContext callerContext,
                                                        OperationResultType operationResultType = OperationResultType.Error)
        {
            String messageCode = String.Empty;
            String errorMessage = String.Empty;
            Int64 entityReferenceId = entity.ReferenceId;
            String attributeNameForDisplay = String.IsNullOrWhiteSpace(attribute.LongName) ? attribute.Name : attribute.LongName;
            String attributeParentNameForDisplay = String.IsNullOrWhiteSpace(attribute.AttributeParentLongName) ? attribute.AttributeParentName : attribute.AttributeParentLongName;
            LocaleEnum systemUILocale = GlobalizationHelper.GetSystemUILocale();

            DiagnosticActivity diagnosticActivity = null;
            Boolean isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;

            if (attribute.Id <= 0)
            {
                String errorMessagePrefix = String.Empty;

                if (entityProcessingOptions.ImportMode == ImportMode.InitialLoad || entityProcessingOptions.ImportMode == ImportMode.RelationshipInitialLoad)
                {
                    errorMessagePrefix = localeMessageBL.Get(systemUILocale, "112020", false, callerContext).Message;
                }
                else
                {
                    errorMessagePrefix = localeMessageBL.Get(systemUILocale, "112019", false, callerContext).Message;
                }

                messageCode = "111688";
                Collection<Object> parameters = null;

                if (attribute.Locale == LocaleEnum.UnKnown)
                {
                    //It should not be a case. But still for proper information.
                    messageCode = "112037"; //{0}{1} Data Locale not found for Attribute '{2}/{3}'. Please check the specified Data Locale.
                }

                // Based on the method parameter identify whether it is relationship attribute or common/technical attribute.
                // and accordingly display the error message.
                if (relationship != null)
                {
                    messageCode = "114021";

                    String containerNameForDisplay = String.Empty;
                    String fromEntityTypeForDisplay = String.Empty;

                    if (entity != null)
                    {
                        containerNameForDisplay = String.IsNullOrWhiteSpace(entity.ContainerLongName) ? entity.ContainerName : entity.ContainerLongName;
                        fromEntityTypeForDisplay = String.IsNullOrWhiteSpace(entity.EntityTypeLongName) ? entity.EntityTypeName : entity.EntityTypeLongName;
                    }

                    String relationshipTypeNameForDisplay = relationship.RelationshipTypeName;
                    String toEntityTypeNameForDisplay = String.IsNullOrWhiteSpace(relationship.ToEntityTypeLongName) ? relationship.ToEntityTypeName : relationship.ToEntityTypeLongName;
                    errorMessage = String.Format(localeMessageBL.Get(systemUILocale, messageCode, false, callerContext).Message, attributeParentNameForDisplay, attributeNameForDisplay, containerNameForDisplay, relationshipTypeNameForDisplay, fromEntityTypeForDisplay, toEntityTypeNameForDisplay); //Attribute '{0}/{1}' not found. Either attribute is not yet defined or is not mapped to the current context  with container: {2}, relationship type: {3}, fromEntityType: {4}, and toEntityType: {5}.
                    parameters = new Collection<Object>() { attributeParentNameForDisplay, attributeNameForDisplay, containerNameForDisplay, relationshipTypeNameForDisplay, fromEntityTypeForDisplay, toEntityTypeNameForDisplay };
                }
                else if (entity != null)
                {
                    messageCode = "114022";
                    String containerNameForDisplay = String.IsNullOrWhiteSpace(entity.ContainerLongName) ? entity.ContainerName : entity.ContainerLongName;
                    String entityTypeNameForDisplay = String.IsNullOrWhiteSpace(entity.EntityTypeLongName) ? entity.EntityTypeName : entity.EntityTypeLongName;
                    String categoryNameForDisplay = String.IsNullOrWhiteSpace(entity.CategoryLongName) ? entity.CategoryName : entity.CategoryLongName;

                    errorMessage = String.Format(localeMessageBL.Get(systemUILocale, messageCode, false, callerContext).Message, errorMessagePrefix, entityReferenceId, attributeParentNameForDisplay, attributeNameForDisplay, containerNameForDisplay, entityTypeNameForDisplay, categoryNameForDisplay); //{0}{1} Attribute '{2}/{3}' not found. Either attribute is not yet defined or is not mapped to the current context  with container: {4}, entity type: {5}, and category: {6}.
                    parameters = new Collection<Object>() { errorMessagePrefix, entityReferenceId, attributeParentNameForDisplay, attributeNameForDisplay, containerNameForDisplay, entityTypeNameForDisplay, categoryNameForDisplay };
                }
                else
                {
                    errorMessage = String.Format(localeMessageBL.Get(systemUILocale, messageCode, false, callerContext).Message, errorMessagePrefix, entityReferenceId, attributeParentNameForDisplay, attributeNameForDisplay); //Attribute '{0}/{1}' not found. Either attribute is not yet defined or is not mapped to the current context.
                    parameters = new Collection<Object>() { errorMessagePrefix, entityReferenceId, attributeParentNameForDisplay, attributeNameForDisplay };
                }

                if (isTracingEnabled)
                {
                    if (diagnosticActivity == null)
                    {
                        diagnosticActivity = new DiagnosticActivity();
                    }
                    diagnosticActivity.LogError(errorMessage);
                }

                attributeOperationResult.AddOperationResult(messageCode, errorMessage, parameters, ReasonType.AttributeCheck, -1, -1, operationResultType);
            }

            if (attribute.IsComplex)
            {
                if (attribute.Attributes != null && attribute.Attributes.Count > 0)
                {
                    foreach (Attribute instanceRecord in attribute.Attributes)
                    {
                        if (instanceRecord.Attributes != null || instanceRecord.Attributes.Count > 0)
                        {
                            foreach (Attribute childAttr in instanceRecord.Attributes)
                            {
                                if (!(attribute.Action == ObjectAction.Delete || attribute.Action == ObjectAction.Ignore) || attribute.IsHierarchical)
                                {
                                    ValidateUnmappedAttributes(entity, childAttr, relationship, attributeOperationResult, localeMessageBL, entityProcessingOptions, callerContext, operationResultType);
                                }
                            }
                        }
                    }
                }
            }
        }


        /// <summary>
        /// Gets the best match profile for entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="matchProfiles">The match profiles.</param>
        /// <param name="callerContext">The caller context.</param>
        /// <returns></returns>
        public static MatchingProfile GetBestMatchProfileForEntity(Entity entity, IMatchingProfileCollection matchProfiles, CallerContext callerContext)
        {
            #region Diagnostics Initialization

            DiagnosticActivity diagnosticActivity = null;

            var isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;

            if (isTracingEnabled)
            {
                diagnosticActivity = new DiagnosticActivity();
                diagnosticActivity.Start();
            }

            #endregion

            try
            {
                Int32 sourceId = -1;
                MatchingProfile matchProfile = null;
                if (entity.SourceInfo != null && entity.SourceInfo.SourceId.HasValue)
                {
                    sourceId = entity.SourceInfo.SourceId.Value;
                }

                if (sourceId < 1)
                {
                    sourceId = (Int32)SystemSource.System;
                }

                if (matchProfiles != null)
                {
                    var matchProfilesByETAndSource = matchProfiles.Where(a => a.EntityTypeId == entity.EntityTypeId && a.SourceId == sourceId);

                    LocaleEnum sdl = GlobalizationHelper.GetSystemDataLocale();
                    LocaleEnum matchLocale = sdl;

                    if (matchProfilesByETAndSource != null && matchProfilesByETAndSource.Any())
                    {
                        #region Find Match Locale

                        Collection<LocaleEnum> attributeLocales = entity.Attributes.GetLocaleList();
                        if (attributeLocales != null)
                        {
                            if (attributeLocales.Count == 1)
                            {
                                LocaleEnum attrLocale = attributeLocales[0];
                                if (attrLocale != LocaleEnum.UnKnown && attrLocale != matchLocale)
                                {
                                    matchLocale = attrLocale;
                                }
                            }
                            else if (attributeLocales.Count == 2 && attributeLocales.Contains(matchLocale))
                            {
                                foreach (LocaleEnum attrLocale in attributeLocales)
                                {
                                    if (attrLocale != LocaleEnum.UnKnown && attrLocale != matchLocale)
                                    {
                                        matchLocale = attrLocale;
                                        break;
                                    }
                                }
                            }
                        }

                        #endregion Find Match Locale

                        matchProfile = matchProfilesByETAndSource.FirstOrDefault(a => a.Locale == matchLocale);

                        if (matchProfile == null && sdl != matchLocale)
                        {
                            matchProfile = matchProfilesByETAndSource.FirstOrDefault(a => a.Locale == sdl);
                        }
                    }

                    if (isTracingEnabled)
                    {
                        if (matchProfile == null)
                        {
                            diagnosticActivity.LogInformation(String.Format("Unable to find a matching profile for entity '{0}' with entity type id '{1}', source id '{2}' and locale '{3}'", entity.Name, entity.EntityTypeId, sourceId, matchLocale.ToString()));
                        }
                        else
                        {
                            diagnosticActivity.LogInformation(String.Format("Matching profile for entity '{0}' with entity type id '{1}', source id '{2}' and locale '{3}' found with id '{4}' and name '{5}'", entity.Name, entity.EntityTypeId, sourceId, matchLocale.ToString(), matchProfile.Id, matchProfile.Name));
                        }
                    }
                }

                return matchProfile;
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

        #region Private Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="contextEntities"></param>
        /// <param name="allEntities"></param>
        /// <param name="currentCategory"></param>
        /// <param name="entityOperationResult"></param>
        private static void ScanAndFilterCategoriesBasedOnResult(EntityCollection contextEntities, EntityCollection allEntities, Entity currentCategory, EntityOperationResult entityOperationResult)
        {
            if (entityOperationResult.OperationResultStatus == OperationResultStatusEnum.Failed || entityOperationResult.OperationResultStatus == OperationResultStatusEnum.CompletedWithErrors)
            {
                contextEntities.Remove(currentCategory);

                if (allEntities != null)
                {
                    allEntities.Remove(currentCategory);
                }
            }
            else if (entityOperationResult.OperationResultStatus == OperationResultStatusEnum.CompletedWithWarnings)
            {
                if (contextEntities != null)
                {
                    if (currentCategory.Attributes != null && currentCategory.Attributes.Count > 0)
                    {
                        //If any of the attributes has warnings, remove from the entities list. 
                        foreach (AttributeOperationResult attributeOperationResult in entityOperationResult.GetAttributeOperationResultCollection())
                        {
                            if (attributeOperationResult.HasWarnings && currentCategory.Attributes.Contains(attributeOperationResult.AttributeId, attributeOperationResult.Locale))
                            {
                                currentCategory.Attributes.Remove(attributeOperationResult.AttributeId, attributeOperationResult.Locale);
                            }
                        }
                    }

                    if (currentCategory.Relationships != null && currentCategory.Relationships.Count > 0)
                    {
                        //If any of the Relationship has warnings, remove from the entities list. 
                        foreach (RelationshipOperationResult relationshipOperationResult in entityOperationResult.GetRelationshipOperationResultCollection())
                        {
                            if (relationshipOperationResult.HasWarnings && currentCategory.Relationships.Contains(relationshipOperationResult.RelationshipId))
                            {
                                currentCategory.Relationships.Remove(relationshipOperationResult.RelationshipId);
                            }
                            else
                            {
                                var relationship = (Relationship)currentCategory.GetRelationships().GetRelationshipById(relationshipOperationResult.RelationshipId);

                                if (relationship != null && relationship.RelationshipAttributes != null && relationship.RelationshipAttributes.Count > 0)
                                {
                                    //If any of the relationship attributes has warnings, remove from the relationship attributes list. 
                                    foreach (AttributeOperationResult relationshipAttributeOperationResult in relationshipOperationResult.GetAttributeOperationResults())
                                    {
                                        if (relationshipAttributeOperationResult.HasWarnings && relationship.RelationshipAttributes.Contains(relationshipAttributeOperationResult.AttributeId, relationshipAttributeOperationResult.Locale))
                                        {
                                            relationship.RelationshipAttributes.Remove(relationshipAttributeOperationResult.AttributeId, relationshipAttributeOperationResult.Locale);
                                        }
                                    }
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
        /// <param name="attributeCollection"></param>
        /// <param name="attributeOperationResult"></param>
        private static void ValidateForDuplicateAttributes(AttributeCollection attributeCollection, AttributeOperationResult attributeOperationResult)
        {
            var messageCode = String.Empty;

            var duplicateNonLocalizableAttributes = attributeCollection.ToList().Where(attribute => !attribute.IsHierarchical && !attribute.IsLocalizable).GroupBy(attribute => attribute.Name).Where(a => a.Count() > 1).Select(names => names.Key).ToList();

            var duplicateAttributes = attributeCollection.Where(attr => duplicateNonLocalizableAttributes.Contains(attr.Name)).ToList();

            foreach (var attribute in duplicateAttributes)
            {
                var warningMessage = String.Format("Attribute {0} is not defined as localizable but localized values were found", attribute.Name); //String.Format(localeMessageBL.Get(systemUILocale, messageCode, false, callerContext).Message);
                attributeOperationResult.AddOperationResult(messageCode, warningMessage, OperationResultType.Warning);
                attributeCollection.Remove(attribute);
            }

            //Recursively do the same for children hierarchy attributes.
            foreach (var attribute in attributeCollection.ToList().Where(attribute => attribute.IsHierarchical))
            {
                foreach (var attributeInstance in attribute.Attributes)
                {
                    ValidateForDuplicateAttributes(attributeInstance.Attributes, attributeOperationResult);
                }
            }
        }

        /// <summary>
        /// Removes relationships attributes from processing whenever it matches with pre-defined reason type like 'InvalidDataLocale', 'PermissionCheck' etc
        /// </summary>
        /// <param name="relationships">Indicates collection of relationship</param>
        /// <param name="relationshipOperationResults">Indicates relationship operation result collection</param>
        private static void ScanAndFilterRelationshipsBasedOnReasonType(RelationshipCollection relationships, RelationshipOperationResultCollection relationshipOperationResults)
        {
            foreach (RelationshipOperationResult relationshipOperationResult in relationshipOperationResults)
            {
                Relationship filteredRelationship = (Relationship)relationships.GetRelationshipById(relationshipOperationResult.RelationshipId);

                if (filteredRelationship != null)
                {
                    ScanAndFilterAttributesBasedOnReasonType(filteredRelationship.RelationshipAttributes, relationshipOperationResult.AttributeOperationResultCollection);

                    if (filteredRelationship.RelationshipCollection != null && filteredRelationship.RelationshipCollection.Count > 0 &&
                        relationshipOperationResult.RelationshipOperationResultCollection != null && relationshipOperationResult.RelationshipOperationResultCollection.Count > 0)
                    {
                        //Recursive call.
                        ScanAndFilterRelationshipsBasedOnReasonType(filteredRelationship.RelationshipCollection, relationshipOperationResult.RelationshipOperationResultCollection);
                    }
                }
            }
        }

        /// <summary>
        /// Removes attributes from processing whenever it matches with pre-defined reason type like 'InvalidDataLocale','PermissionCheck' etc
        /// </summary>
        /// <param name="attributes">Indicates collection of attribute</param>
        /// <param name="attributeOperationResults">Indicates collection of attribute operation result</param>
        private static void ScanAndFilterAttributesBasedOnReasonType(AttributeCollection attributes, AttributeOperationResultCollection attributeOperationResults)
        {
            if (attributes != null && attributes.Count > 0 && attributeOperationResults != null && attributeOperationResults.Count > 0)
            {
                foreach (AttributeOperationResult attributeOperationResult in attributeOperationResults)
                {
                    //If attribute operation result has any pre-defined reason type like InvalidDataLocale/PermissionCheck with any error or warning then remove those attributes from processing.
                    //This is required to avoid database persistence for those attributes.
                    if (attributeOperationResult.Contains(_reasonTypes))
                    {
                        IAttribute attribute = attributes.GetAttribute(attributeOperationResult.AttributeId, attributeOperationResult.Locale);

                        if (attribute != null)
                        {
                            attributes.Remove(attribute);
                        }
                    }
                }
            }
        }

        private static Boolean RemoveIgnoreErrors(OperationResult operationResult,CallerContext callerContext)
        {
            Boolean removed = false;

            if (callerContext == null || callerContext.Module == MDMCenterModules.Import || operationResult == null || !operationResult.HasError || operationResult.Errors == null || operationResult.Errors.Count < 1)
            {
                return removed;
            }

            ErrorCollection errorsToBeRemoved = new ErrorCollection();
            foreach (Error error in operationResult.Errors)
            {
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
                removed = true;
            }
            return removed;
        }

        #endregion

        #endregion
    }
}