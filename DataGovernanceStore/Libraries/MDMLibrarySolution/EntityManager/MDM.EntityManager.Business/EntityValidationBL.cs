using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

namespace MDM.EntityManager.Business
{
    using MDM.AttributeDependencyManager.Business;
    using MDM.AttributeModelManager.Business;
    using MDM.BusinessObjects;
    using MDM.CategoryManager.Business;
    using MDM.ConfigurationManager.Business;
    using MDM.ContainerManager.Business;
    using MDM.Core;
    using MDM.Core.Exceptions;
    using MDM.Core.Extensions;
    using MDM.DataModelManager.Business;
    using MDM.EntityManager.Business.EntityOperations.Helpers;
    using MDM.EntityManager.Data;
    using MDM.HierarchyManager.Business;
    using MDM.Interfaces;
    using MDM.KnowledgeManager.Business;
    using MDM.MessageManager.Business;
    using MDM.RelationshipManager.Business;
    using MDM.Utility;

    /// <summary>
    /// Represents business logic for Validating entities
    /// </summary>
    public class EntityValidationBL : BusinessLogicBase
    {
        #region Fields

        /// <summary>
        /// Specifies security principal for user.
        /// </summary>
        private SecurityPrincipal _securityPrincipal = null;

        /// <summary>
        /// Specifies the name of the culture in the context
        /// </summary>
        private String _cultureName = String.Empty;

        /// <summary>
        /// Specifies the extensions for images allowed in the system
        /// </summary>
        private String _imageExtensions = String.Empty;

        /// <summary>
        /// Specifies the locales available in the system
        /// </summary>
        private LocaleCollection _availableLocales = null;

        /// <summary>
        /// Field denoting locale Message
        /// </summary>
        private LocaleMessage _localeMessage = null;

        /// <summary>
        /// Field denoting localeMessageBL
        /// </summary>
        private LocaleMessageBL _localeMessageBL = new LocaleMessageBL();

        /// <summary>
        /// Field denoting parameters for locale message
        /// </summary>
        private Object[] _parameters = null;

        /// <summary>
        /// Field denoting system UI locale
        /// </summary>
        private LocaleEnum _systemUILocale = GlobalizationHelper.GetSystemUILocale();

        #endregion

        #region Properties

        /// <summary>
        /// Property denoting name of the culture in the context
        /// </summary>
        public String CultureName
        {
            get
            {
                if (String.IsNullOrWhiteSpace(_cultureName))
                {
                    _cultureName = "en-US";

                    if (_securityPrincipal != null && _securityPrincipal.UserPreferences != null && !String.IsNullOrWhiteSpace(_securityPrincipal.UserPreferences.UICultureName))
                        _cultureName = _securityPrincipal.UserPreferences.UICultureName;
                }

                return _cultureName;
            }
        }

        /// <summary>
        /// Property denoting extensions for images allowed in the system
        /// </summary>
        public String ImageExtensions
        {
            get
            {
                if (String.IsNullOrWhiteSpace(_imageExtensions))
                {
                    _imageExtensions = GetImageExtensionRegexPart("Image");
                }

                return _imageExtensions;
            }
        }

        /// <summary>
        /// Property denoting locales available in the system
        /// </summary>
        public LocaleCollection AvailableLocales
        {
            get
            {
                if (_availableLocales == null)
                {
                    LocaleBL localeBL = new LocaleBL();
                    _availableLocales = localeBL.GetAvailableLocales();
                }

                return _availableLocales;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Instantiate Entity Validation BL
        /// </summary>
        public EntityValidationBL()
        {
            GetSecurityPrincipal();
        }

        #endregion

        #region Methods

        #region Public Methods

        /// <summary>
        /// Validates requested entities
        /// </summary>
        /// <param name="entityCollection">Entities which needs to be validated</param>
        /// <param name="entityOperationResults">Validation results</param>
        /// <param name="callerContext">Indicates Application and Module name by which action is being performed</param>
        /// <param name="validateBranchLevel">The flag indicating whether to validate branch levels of entities
        /// <para>Usually used for Bulk Entity Processing where entities are expected to be of single branch level</para>
        /// </param>
        /// <param name="entityProcessingOption">Entity Processing Option</param>
        /// <exception cref="MDMOperationException">Thrown when either 'entityCollection' or 'entityOperationResults' is null</exception>
        public void Validate(EntityCollection entityCollection, EntityOperationResultCollection entityOperationResults, CallerContext callerContext, Boolean validateBranchLevel = true, EntityProcessingOptions entityProcessingOption = null)
        {
            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StartTraceActivity("EntityValidationBL.ValidateEntities", false);

            try
            {
                if (entityCollection == null)
                {
                    _localeMessage = _localeMessageBL.Get(_systemUILocale, "111798", false, callerContext);
                    throw new MDMOperationException("111798", _localeMessage.Message, "EntityManager", String.Empty, "Validate", ReasonType.ApplicationError, -1); //Entities are not available.
                }

                if (entityOperationResults == null)
                {
                    _localeMessage = _localeMessageBL.Get(_systemUILocale, "111799", false, callerContext);
                    throw new MDMOperationException("111799", _localeMessage.Message, "EntityManager", String.Empty, "Validate", ReasonType.ApplicationError, -1); //Entity Operation Results are not available.
                }

                if (entityCollection.Count > 0)
                {
                    if (Constants.TRACING_ENABLED)
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Starting validation for requested entities..");

                    #region Entity Branch Level Validation

                    if (validateBranchLevel)
                    {
                        //ASSUMPTION::Bulk process will happen only if all entities belongs to the same branch level 
                        //i.e. all should be of type Category or Entity

                        //Validate branch level and if entities does not belong to same level then skip the further validation and return
                        Boolean sameBranchLevel = ValidateBranchLevels(entityCollection, entityOperationResults, callerContext);

                        if (!sameBranchLevel)
                            return;
                    }

                    #endregion

                    LocaleCollection allowableLocales = null;
                    Hierarchy hierarchy = null;

                    Dictionary<Int32, LocaleCollection> allowableLocalesForContainers = new Dictionary<Int32, LocaleCollection>();
                    Dictionary<Int64, Category> categories = new Dictionary<Int64, Category>();
                    Dictionary<Int32, Container> containers = new Dictionary<int, Container>();
                    ContainerBL containerBL = new ContainerBL();
                    Container container = null;
                    HierarchyCollection hierarchies = new HierarchyBL().GetAll(callerContext, true);

                    Dictionary<Int64, Dictionary<Int32, Int32>> extensionEntityIdCountDictionary = new Dictionary<Int64, Dictionary<Int32, Int32>>();
                    ContainerEntityTypeMappingBL containerEntityTypeMappingBL = new ContainerEntityTypeMappingBL();
                    ContainerEntityTypeMappingCollection containerEntityTypeMappings = containerEntityTypeMappingBL.GetAll(callerContext);

                    foreach (Entity entity in entityCollection)
                    {
                        try
                        {
                            if (Constants.TRACING_ENABLED)
                                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("Starting validation for Entity - Id: {0} ExternalId: {1}", entity.Id, entity.ExternalId));

                            //Get entity operation result
                            EntityOperationResult entityOperationResult = null;

                            try
                            {
                                entityOperationResult = entityOperationResults.SingleOrDefault(eor => eor.EntityId == entity.Id);
                            }
                            catch (InvalidOperationException)
                            {
                                _localeMessage = _localeMessageBL.Get(_systemUILocale, "111800", new object[] { entity.Id, entity.ExternalId }, false, callerContext);

                                throw new MDMOperationException("111800", _localeMessage.Message, "EntityManager", String.Empty, "Get", ReasonType.ApplicationError, -1); //More than one operation results found for Entity - Id: {0} ExternalId: {1}
                            }

                            #region Prepare Validation Parameters

                            container = null;

                            if (!containers.TryGetValue(entity.ContainerId, out container))
                            {
                                container = containerBL.GetById(entity.ContainerId);

                                if (container != null)
                                {
                                    containers.Add(entity.ContainerId, container);
                                }
                            }

                            if (container != null)
                            {
                                PopulateAllowableLocalesAndHierarchy(entity, container, allowableLocalesForContainers, hierarchies, out allowableLocales, out hierarchy);
                            }

                            #endregion

                            #region Entity MetaData Validation

                            if (entityProcessingOption.ValidateLeafCategory)
                            {
                                ValidateEntityMetaData(entity, hierarchy, categories, entityOperationResult, callerContext);
                            }

                            #endregion Entity MetaData Validation

                            #region Entity MoveContext Validation

                            ValidateEntityMoveContext(entity, hierarchy, categories, entityOperationResult, callerContext);

                            #endregion Entity MoveContext Validation

                            #region Duplicate Entity Validation

                            if (!String.IsNullOrWhiteSpace(entity.Name))
                            {
                                ValidateDuplicateEntity(entity, entityOperationResult, callerContext);
                            }

                            #endregion Duplicate Entity Validation

                            #region Entity Attributes Validation

                            ValidateEntityAttributes(entity, entityOperationResult, allowableLocales, callerContext, entityProcessingOption);

                            #endregion

                            #region Entity Relationships

                            ValidateEntityRelationships(entity, entityOperationResult, callerContext, entityProcessingOption);

                            #endregion

                            #region Entity User Input Validation

                            ValidateEntityFieldsForExecutableCode(entity, entityOperationResult, callerContext);

                            #endregion Entity User Input Validation

                            #region Prepare Delta Extension Entity Ids count dictionary

                            if (container != null && !(container.ContainerType == ContainerType.Upstream || container.ContainerType == ContainerType.MasterCollaboration))
                            {
                                ContainerEntityTypeMapping containerEntityTypeSpecificMapping = (ContainerEntityTypeMapping)containerEntityTypeMappings.GetByContainerAndEntityType(entity.ContainerId, entity.EntityTypeId);

                                if (containerEntityTypeSpecificMapping != null &&
                                    (containerEntityTypeSpecificMapping.MinimumExtensions > 0 || containerEntityTypeSpecificMapping.MaximumExtensions > 0))
                                {
                                    Int32 deltaOccuranceCount = 0;
                                    Dictionary<Int32, Int32> contextBasedEntitiesCountDictionary = new Dictionary<Int32, Int32>();

                                    extensionEntityIdCountDictionary.TryGetValue(entity.ParentExtensionEntityId, out contextBasedEntitiesCountDictionary);

                                    if (contextBasedEntitiesCountDictionary == null)
                                    {
                                        contextBasedEntitiesCountDictionary = new Dictionary<Int32, Int32>();
                                    }
                                    Int32 key = Utility.GetInternalUniqueKeyBasedOnParam(entity.ContainerId, entity.EntityTypeId); ;

                                    if (contextBasedEntitiesCountDictionary.Count > 0)//It means dictionary exist for current parent extension entity id.
                                    {
                                        contextBasedEntitiesCountDictionary.TryGetValue(key, out deltaOccuranceCount);
                                    }

                                    if (entity.Action == ObjectAction.Create)
                                    {
                                        deltaOccuranceCount++;
                                    }
                                    else if (entity.Action == ObjectAction.Delete)
                                    {
                                        deltaOccuranceCount--;
                                    }

                                    if (contextBasedEntitiesCountDictionary.Count > 0)
                                    {
                                        contextBasedEntitiesCountDictionary[key] = deltaOccuranceCount;
                                    }
                                    else
                                    {
                                        contextBasedEntitiesCountDictionary.Add(key, deltaOccuranceCount);
                                    }

                                    if (extensionEntityIdCountDictionary.ContainsKey(entity.ParentExtensionEntityId))
                                    {
                                        extensionEntityIdCountDictionary[entity.ParentExtensionEntityId] = contextBasedEntitiesCountDictionary;
                                    }
                                    else
                                    {
                                        extensionEntityIdCountDictionary.Add(entity.ParentExtensionEntityId, contextBasedEntitiesCountDictionary);
                                    }
                                }
                            }
                            #endregion
                        }

                        catch (MDMOperationException ex)
                        {
                            _parameters = new Object[] { ex.Message };
                            _localeMessage = _localeMessageBL.Get(_systemUILocale, "114205", _parameters, false, callerContext);
                            entityOperationResults.AddEntityOperationResult(entity.Id, "114205", _localeMessage.Message, _parameters.ToCollection(), ex.ReasonType, ex.RuleMapContextId, ex.RuleId, OperationResultType.Error);
                        }
                    }

                    #region Validate the maximum and minimum extension allowed based on context

                    //If there are no entities which are coming for process has setup for max and min extension count for its context, 
                    //then do not try to validate it for max and min extension validation check
                    if (extensionEntityIdCountDictionary != null && extensionEntityIdCountDictionary.Count > 0)
                    {
                        ExtensionRelationshipBL extensionRelationshipBL = new ExtensionRelationshipBL();

                        //Dictionary with key as parent extension entity id and value contains dictionary of combination of 
                        //container and entity type id and the respective value is the extension count in specific context.
                        Dictionary<Int64, Dictionary<Int32, Int32>> originalExtensionEntitiesCountDictionary = extensionRelationshipBL.GetExtensionsRelationshipsCount(extensionEntityIdCountDictionary.Keys.ToCollection(), callerContext);
                        Collection<Int64> erroredParentEntityIds = new Collection<Int64>();

                        foreach (Entity entity in entityCollection)
                        {
                            Dictionary<Int32, Int32> deltaExtensionEntityIdCountDictionary = null;

                            //If the entity to be processed does not have parent entity extension id with any impacted count then do not proceed with validation.
                            if (!extensionEntityIdCountDictionary.TryGetValue(entity.ParentExtensionEntityId, out deltaExtensionEntityIdCountDictionary))
                            {
                                continue;
                            }

                            //Do not apply the max extension and min extension validation to upstream and master collaboration
                            if (container == null ||
                                (container != null &&
                                    (container.ContainerType == ContainerType.Upstream || container.ContainerType == ContainerType.MasterCollaboration)))
                            {
                                continue;
                            }

                            Int32 key = Utility.GetInternalUniqueKeyBasedOnParam(entity.ContainerId, entity.EntityTypeId);
                            Dictionary<Int32, Int32> originalContextEntityCountDictionary = null;

                            if (originalExtensionEntitiesCountDictionary != null && originalExtensionEntitiesCountDictionary.Count > 0)
                            {
                                //This dictionary is container Id and count of extended entity in this container wrt parent extension entity id.
                                originalExtensionEntitiesCountDictionary.TryGetValue(entity.ParentExtensionEntityId, out originalContextEntityCountDictionary);
                            }

                            Int32 originalExtensionCount = 0;

                            if (originalContextEntityCountDictionary != null)
                            {
                                originalContextEntityCountDictionary.TryGetValue(key, out originalExtensionCount);
                            }

                            Int32 totalExtensionCount = 0;
                            Int32 deltaExtensionCount = 0;
                            deltaExtensionEntityIdCountDictionary.TryGetValue(key, out deltaExtensionCount);

                            totalExtensionCount = originalExtensionCount + deltaExtensionCount;

                            ContainerEntityTypeMapping containerEntityTypeSpecificMapping = (ContainerEntityTypeMapping)containerEntityTypeMappings.GetByContainerAndEntityType(entity.ContainerId, entity.EntityTypeId);

                            if (containerEntityTypeSpecificMapping.MaximumExtensions > 0 && totalExtensionCount > containerEntityTypeSpecificMapping.MaximumExtensions)
                            {
                                Object[] parameters = new Object[] { entity.Name, containerEntityTypeSpecificMapping.MinimumExtensions, containerEntityTypeSpecificMapping.MaximumExtensions, entity.ContainerName, entity.EntityTypeName };
                                String message = String.Format("Failed to process the Entity '{0}', as it is not within the minimum required extensions '{1}' and maximum allowed extensions '{2}' for container '{3}' and entity type '{4}'.", parameters);
                                EntityOperationResult entityOperationResult = (EntityOperationResult)entityOperationResults.GetByEntityId(entity.Id);

                                entityOperationResult.AddOperationResult("114277", message, parameters.ToCollection(), ReasonType.CardinalityCheck, -1, -1, OperationResultType.Error);
                            }
                            else if (entity.ParentExtensionEntityId > 0 && totalExtensionCount < containerEntityTypeSpecificMapping.MinimumExtensions)
                            {
                                //When minimum extensions are not met. Add warning to current entity. 
                                //Add an error to the parent with cardinality reason type. This sets the parent's extension state as invalid.

                                Object[] parameters = new Object[] { entity.Name, entity.ParentExtensionEntityLongName, entity.ContainerName, entity.EntityTypeName, containerEntityTypeSpecificMapping.MinimumExtensions };
                                String message = String.Format("Entity '{0}' creation resulted in invalid cardinal check for parent entity '{1}'. Container '{2}' and entity type '{3}' require minimum '{4}' extensions.", parameters);
                                EntityOperationResult entityOperationResult = (EntityOperationResult)entityOperationResults.GetByEntityId(entity.Id);
                                entityOperationResult.AddOperationResult("114447", message, parameters.ToCollection(), ReasonType.CardinalityCheck, -1, -1, OperationResultType.Warning);
                                Object[] parentErrorParameters = new Object[] { entity.ParentExtensionEntityLongName, containerEntityTypeSpecificMapping.MinimumExtensions, containerEntityTypeSpecificMapping.MaximumExtensions, entity.ContainerName, entity.EntityTypeName };
                                
                                //Add error to parent.
                                if (!erroredParentEntityIds.Contains(entity.ParentExtensionEntityId))
                                {
                                    message = String.Format("Failed to process the Entity '{0}', as it is not within the minimum required extensions '{1}' and maximum allowed extensions '{2}' for container '{3}' and entity type '{4}'.", parentErrorParameters);
                                    entityOperationResults.AddEntityOperationResult(entity.ParentExtensionEntityId, "114277", message, parentErrorParameters.ToCollection(), ReasonType.CardinalityCheck, -1, -1, OperationResultType.Error);
                                    erroredParentEntityIds.Add(entity.ParentExtensionEntityId);
                                }
                            }
                            else if (entity.ParentExtensionEntityId > 0 && totalExtensionCount >= containerEntityTypeSpecificMapping.MinimumExtensions)
                            {
                                //Adding information on reaching minimum extensions makes the corresponding entity validation error entry to be deleted.
                                String message = "Minimum extensions are created for container '{0}' and entity type '{1}' for parent entity '{2}. Cardinal check of parent entity '{2}'  is now valid.";
                                Object[] parameters = new Object[] { entity.ContainerName, entity.EntityTypeName, entity.ParentExtensionEntityLongName };
                                entityOperationResults.AddEntityOperationResult(entity.ParentExtensionEntityId, "114448", message, parameters.ToCollection(), ReasonType.CardinalityCheck, -1, -1, OperationResultType.Information);
                            }
                        }
                    }

                    #endregion

                    if (Constants.TRACING_ENABLED)
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Refreshing entity operation results status..");

                    //Update entity operation results status
                    entityOperationResults.RefreshOperationResultStatus();

                    if (Constants.TRACING_ENABLED)
                    {
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Refreshing of entity operation results status completed.");

                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Validation completed for requested entities.");
                    }
                }
                else
                {
                    if (Constants.TRACING_ENABLED)
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "No entities are available for validation.");
                }
            }
            finally
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StopTraceActivity("EntityValidationBL.ValidateEntities");
            }
        }

        /// <summary>
        /// Validates requested attribute
        /// </summary>
        /// <param name="attribute">Attribute which needs to be validated</param>
        /// <param name="attributeModelContext">Context of the attribute model</param>
        /// <param name="attributeOperationResult">Validation results</param>
        /// <param name="callerContext">Indicates Application and Module name by which action is being performed</param>
        /// <param name="entityProcessingOptions">Entity Processing Option</param>
        /// <exception cref="MDMOperationException">Thrown when either 'attribute' or 'attributeModelContext' or 'attributeOperationResult' is null</exception>
        public void ValidateAttribute(Attribute attribute, AttributeModelContext attributeModelContext, AttributeOperationResult attributeOperationResult, CallerContext callerContext, EntityProcessingOptions entityProcessingOptions = null)
        {
            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StartTraceActivity("EntityValidationBL.ValidateAttribute", false);
            OperationResultType operationResultType = OperationResultType.Error;

            try
            {
                if (attribute == null)
                {
                    _localeMessage = _localeMessageBL.Get(_systemUILocale, "111814", false, callerContext);
                    throw new MDMOperationException("111814", _localeMessage.Message, "EntityManager", String.Empty, "ValidateAttribute", ReasonType.ApplicationError, -1); //Attribute is not available.
                }

                if (attributeModelContext == null)
                {
                    _localeMessage = _localeMessageBL.Get(_systemUILocale, "111801", false, callerContext);
                    throw new MDMOperationException("111801", _localeMessage.Message, "EntityManager", String.Empty, "ValidateAttribute", ReasonType.ApplicationError, -1); //Attribute Model Context is not available.
                }

                if (attributeOperationResult == null)
                {
                    _localeMessage = _localeMessageBL.Get(_systemUILocale, "111802", false, callerContext);
                    throw new MDMOperationException("111802", _localeMessage.Message, "EntityManager", String.Empty, "ValidateAttribute", ReasonType.ApplicationError, -1); //Attribute Operation Result is not available.
                }

                try
                {
                    if (Constants.TRACING_ENABLED)
                    {
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Getting attribute model for below details..");
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("Attribute - Id: {0} LongName: {1} Locale: {2}", attribute.Id, attribute.LongName, attribute.Locale));
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("Attribute Model Context - AttributeModelType: {0} Container: {1} EntityType: {2} Category: {3} RelationshipType: {4}", attributeModelContext.AttributeModelType.ToString(), attributeModelContext.ContainerId, attributeModelContext.EntityTypeId, attributeModelContext.CategoryId, attributeModelContext.RelationshipTypeId));
                    }

                    //Get attribute model
                    AttributeModelBL attributeModelManager = new AttributeModelBL();
                    AttributeModelCollection attributeModels = attributeModelManager.GetById(attribute.Id, attributeModelContext);
                    AttributeModel attributeModel = null;

                    LocaleEnum locale;

                    var attributeModelContextLocales = attributeModelContext.GetLocales();
                    if (attributeModelContextLocales != null && attributeModelContextLocales.Count > 0)
                    {
                        locale = attributeModelContextLocales[0];
                    }
                    else
                    {
                        locale = attribute.Locale;
                    }

                    if (attributeModels.Count > 0)
                    {
                        attributeModel = attributeModels[attribute.Id, locale];
                    }

                    if (Constants.TRACING_ENABLED)
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Attribute Model get completed.");

                    if (attributeModel != null)
                    {
                        ValidateAttribute(attribute, attributeModel, attributeOperationResult, callerContext, entityProcessingOptions, "Attribute");
                    }
                    else
                    {
                        _parameters = new Object[] { attributeModelContext.ContainerId.ToString(), attributeModelContext.EntityTypeId.ToString(), attributeModelContext.CategoryId.ToString(), attributeModelContext.RelationshipTypeId.ToString() };
                        _localeMessage = _localeMessageBL.Get(_systemUILocale, "114208", _parameters, false, callerContext);
                        attributeOperationResult.AddOperationResult("114208", _localeMessage.Message, _parameters.ToCollection(), ReasonType.DataModelViolation, -1, -1, operationResultType);
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, _localeMessage.Message);
                    }

                }
                catch (MDMOperationException exception)
                {
                    _parameters = new Object[] { exception.Message };
                    _localeMessage = _localeMessageBL.Get(_systemUILocale, exception.MessageCode, _parameters, false, callerContext);
                    attributeOperationResult.AddOperationResult(exception.MessageCode, _localeMessage.Message, _parameters.ToCollection(), exception.ReasonType, exception.RuleMapContextId, exception.RuleId, operationResultType);
                }
                catch (Exception ex)
                {
                    _parameters = new Object[] { ex.Message };
                    _localeMessage = _localeMessageBL.Get(_systemUILocale, "114205", _parameters, false, callerContext);
                    attributeOperationResult.AddOperationResult("114205", _localeMessage.Message, _parameters.ToCollection(), ReasonType.SystemError, -1, -1, operationResultType);
                }

                //Update the status to 'Success' if there are no errors..
                //Note:: Failed status will be updated whenever an error message has been added.
                if (!attributeOperationResult.HasError)
                {
                    attributeOperationResult.OperationResultStatus = OperationResultStatusEnum.Successful;
            }
            }
            finally
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StopTraceActivity("EntityValidationBL.ValidateAttribute");
            }
        }

        /// <summary>
        /// Validate entities for their properties like container id, category id, entity type id, action
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="entityOR"></param>
        /// <param name="systemDefaultUILocale"></param>
        /// <param name="operationAction"></param>
        /// <param name="resolveIdsByNames"></param>
        /// <param name="callerContext"></param>
        public void ValidateEntityProperties(EntityCollection entities, EntityOperationResultCollection entityOR, LocaleEnum systemDefaultUILocale, String operationAction, Boolean resolveIdsByNames, CallerContext callerContext)
        {
            if (entities == null)
            {
                _localeMessage = _localeMessageBL.Get(_systemUILocale, "111565", false, callerContext);
                throw new MDMOperationException("111565", _localeMessage.Message, "EntityManager", String.Empty, "ValidateEntities", ReasonType.ApplicationError, -1);//Entities is null.
            }

            foreach (Entity entity in entities)
            {
                #region Validate Entity name for entity.Action = Update

                //Validate Entity name for entity.Action = Update
                if (entity.HasChanged())
                {
                    if (entity.Id < 1)
                    {
                        if (resolveIdsByNames)
                        {
                            this.ValidateEntityProperty(entity, "113821", "Failed to get the entity Id for ShortName: {0}. Please provide a valid entity name.",
                                systemDefaultUILocale, entityOR, callerContext, new Collection<Object>() { entity.Name }, ReasonType.MissingOrDeletedEntity, -1);
                        }
                        else
                        {
                            this.ValidateEntityProperty(entity, "111570", "Entity [ShortName : {0}] has action = \"Update\" but EntityId is not provided",
                                systemDefaultUILocale, entityOR, callerContext, new Collection<Object>() { entity.Name }, ReasonType.MissingOrDeletedEntity, -1);
                        }
                    }
                }

                #endregion Validate Entity name for entity.Action = Update

                #region Validate Container Id

                if (entity.ContainerId < 1)
                {
                    if (resolveIdsByNames)
                    {
                        this.ValidateEntityProperty(entity, "113822", "Failed to get the container Id for ShortName: {0}, entity Id: {1}, and entity name: {2}. Please provide a valid container name.",
                            systemDefaultUILocale, entityOR, callerContext, new Collection<Object>() { entity.ContainerName, entity.Id, entity.Name }, ReasonType.DataModelViolation, -1);
                    }
                    else
                    {
                        this.ValidateEntityProperty(entity, "111622", "Entity [ShortName : {0}] is not having ContainerId",
                            systemDefaultUILocale, entityOR, callerContext, new Collection<Object>() { entity.Name }, ReasonType.MissingOrDeletedEntity, -1);
                    }
                }

                #endregion Validate Container Id

                #region Validate Entity type Id

                if (entity.EntityTypeId < 1)
                {
                    if (resolveIdsByNames)
                    {
                        this.ValidateEntityProperty(entity, "113820", "Failed to get the entity type Id for ShortName: {0}, entity Id: {1}, and entity name: {2}. Please provide a valid entity type name.",
                           systemDefaultUILocale, entityOR, callerContext, new Collection<Object>() { entity.EntityTypeName, entity.Id, entity.Name }, ReasonType.MissingOrDeletedEntity, -1);
                    }
                    else
                    {
                        this.ValidateEntityProperty(entity, "111623", "Entity [ShortName : {0}] is not having EntityTypeId",
                           systemDefaultUILocale, entityOR, callerContext, new Collection<Object>() { entity.Name }, ReasonType.MissingOrDeletedEntity, -1);
                    }
                }

                #endregion Validate Entity type Id

                #region Validate Category Id

                //Do category Id  validation only if Entity being processed is Category
                //We identify if entity if category by entity.EntityTypeId = 6
                if (entity.EntityTypeId != 6 && entity.CategoryId < 1)
                {
                    if (resolveIdsByNames)
                    {
                        this.ValidateEntityProperty(entity, "113823", "Failed to get the category Id for category name: {0}, category path: {1}, entity Id: {2}, and entity name: {3}. Please provide a valid category name or category path.",
                         systemDefaultUILocale, entityOR, callerContext, new Collection<Object>() { entity.CategoryName, entity.CategoryPath, entity.Id, entity.Name }, ReasonType.IncorrectCategory, -1);
                    }
                    else
                    {
                        this.ValidateEntityProperty(entity, "111624", "Entity [ShortName : {0}] is not having Category Id",
                          systemDefaultUILocale, entityOR, callerContext, new Collection<Object>() { entity.Name }, ReasonType.IncorrectCategory, -1);
                    }
                }

                #endregion Validate Category Id
            }
        }

        /// <summary>
        /// Validate Impacted data for Entity Delete.
        /// If entity has children or category has children or 
        /// category has mapped to any technical attribute then for the entity 
        /// </summary>
        /// <param name="entities">Indicates the Entity collections</param>
        /// <param name="entityOperationResults">Indicates the Entity operation results</param>
        /// <param name="callerContext">Indicates the caller context</param>
        public void ValidateEntitiesReferencesForDelete(EntityCollection entities, EntityOperationResultCollection entityOperationResults, CallerContext callerContext)
        {
            if (Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.StartTraceActivity("EntityManager.EntityValidationBL.ValidateEntitiesReferencesForDelete", MDMTraceSource.EntityProcess, false);

                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Starting validation for Entity Impacted data for delete..", MDMTraceSource.EntityProcess);
            }

            try
            {
                //Get command
                DBCommandProperties command = DBCommandHelper.Get(callerContext.Application, callerContext.Module, MDMCenterModuleAction.Read);

                EntityValidationDA entityValidationDA = new EntityValidationDA();
                entityValidationDA.CheckForEntitiesReferences(entities, entityOperationResults, command);

                //Refresh operation result status
                entityOperationResults.RefreshOperationResultStatus();

                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Done Validation for Entity Impacted data's for delete.", MDMTraceSource.EntityProcess);
            }
            finally
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StopTraceActivity("EntityManager.EntityValidationBL.ValidateEntitiesReferencesForDelete", MDMTraceSource.EntityProcess);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="containerId"></param>
        /// <param name="containerName"></param>
        /// <param name="entityTypeId"></param>
        /// <param name="entityTypeName"></param>
        /// <param name="relationship"></param>
        /// <param name="relationshipOperationResult"></param>
        /// <param name="relationshipCardinalities"></param>
        /// <param name="callerContext"></param>
        /// <param name="operationResultType"></param>
        public void ValidateRelationshipMappings(Int32 containerId, String containerName, Int32 entityTypeId, String entityTypeName, Relationship relationship, RelationshipOperationResult relationshipOperationResult, RelationshipCardinalityCollection relationshipCardinalities, CallerContext callerContext, OperationResultType operationResultType = OperationResultType.Error)
        {
            #region Container - RelationshipType - EntityType Mapping validation

            ContainerRelationshipTypeEntityTypeMappingCollection cRTETMappings = null;
            ContainerRelationshipTypeEntityTypeMappingBL containerRelationshipTypeEntityTypeMappingBL = new ContainerRelationshipTypeEntityTypeMappingBL();

            try
            {
                //Get mappings..
                cRTETMappings = containerRelationshipTypeEntityTypeMappingBL.Get(containerId, relationship.RelationshipTypeId, callerContext);

                //Check whether the current entity type is available in these mappings
                if (cRTETMappings == null || cRTETMappings.Count < 1 || cRTETMappings.SingleOrDefault(m => m.EntityTypeId == entityTypeId) == null)
                {
                    //Mapping is not available
                    //Add error message into operation result
                    _parameters = new Object[] { entityTypeName, containerName, relationship.RelationshipTypeName };
                    _localeMessage = _localeMessageBL.Get(_systemUILocale, "114211", _parameters, false, callerContext);
                    relationshipOperationResult.AddOperationResult("114211", _localeMessage.Message, _parameters.ToCollection(), ReasonType.DataModelViolation, -1, -1, operationResultType);
                }
            }
            catch (Exception ex)
            {
                _parameters = new Object[] { ex.Message };
                _localeMessage = _localeMessageBL.Get(_systemUILocale, "114205", _parameters, false, callerContext);
                //Add error message into operation result
                relationshipOperationResult.AddOperationResult("114205", _localeMessage.Message, _parameters.ToCollection(), ReasonType.SystemError, -1, -1, operationResultType);
            }

            #endregion

            #region Relationship Cardinality validation

            if (relationshipOperationResult.OperationResultStatus != OperationResultStatusEnum.Failed || relationshipOperationResult.OperationResultStatus != OperationResultStatusEnum.CompletedWithErrors)
            {
                //Validate cardinality, only if they exist..
                //If not all entity types are allowed.
                if (relationshipCardinalities == null)
                {
                    relationshipCardinalities = new RelationshipBL().GetRelationshsipCardinalities(relationship.RelationshipTypeId, containerId, entityTypeId, new CallerContext(MDMCenterApplication.MDMCenter, MDMCenterModules.Entity));
                }

                if (relationshipCardinalities != null && relationshipCardinalities.Count > 0)
                {
                    Boolean hasRelCardinality = false;

                    foreach (RelationshipCardinality relationshipCardinality in relationshipCardinalities)
                    {
                        if (relationshipCardinality.ToEntityTypeId == relationship.ToEntityTypeId)
                        {
                            hasRelCardinality = true;
                            break;
                        }
                    }

                    if (!hasRelCardinality)
                    {
                        //Add error
                        _parameters = new Object[] { relationship.ToEntityTypeName, relationship.RelationshipTypeName };
                        _localeMessage = _localeMessageBL.Get(_systemUILocale, "114212", _parameters, false, callerContext);
                        relationshipOperationResult.AddOperationResult("114212", _localeMessage.Message, _parameters.ToCollection(), ReasonType.CardinalityCheck, -1, -1, operationResultType);
                    }
                }
            }

            #endregion
        }

        /// <summary>
        /// Validates entity metadata
        /// </summary>
        /// <param name="entity">Indicates entity to be validated</param>
        /// <param name="hierarchy">Indicates hierarchy for entity's container</param>
        /// <param name="categories">Indicates dictionary of categories</param>
        /// <param name="entityOperationResult">Indicates entity operation result</param>
        /// <param name="callerContext">Indicates caller context</param>
        public void ValidateEntityMetaData(Entity entity, Hierarchy hierarchy, Dictionary<Int64, Category> categories, EntityOperationResult entityOperationResult, CallerContext callerContext)
        {
            if (hierarchy != null && hierarchy.LeafNodeOnly && entity.EntityTypeId != Constants.CATEGORY_ENTITYTYPE)
            {
                Category category = GetCategory(categories, entity.CategoryId, hierarchy.Id, callerContext);

                if (category != null && !category.IsLeaf)
                {
                    _parameters = new Object[] { entity.CategoryLongName };

                    if (entity.Action == ObjectAction.Create)
                    {
                        _localeMessage = _localeMessageBL.Get(_systemUILocale, "114250", _parameters, false, callerContext);
                        entityOperationResult.AddOperationResult("114250", _localeMessage.Message, _parameters.ToCollection(), ReasonType.IncorrectCategory, -1, -1, OperationResultType.Error);
                    }
                    else if (entity.Action == ObjectAction.Update)
                    {
                        _localeMessage = _localeMessageBL.Get(_systemUILocale, "114251", _parameters, false, callerContext);
                        entityOperationResult.AddOperationResult("114251", _localeMessage.Message, _parameters.ToCollection(), ReasonType.IncorrectCategory, -1, -1, OperationResultType.Error);
                    }
                }
            }
        }

        /// <summary>
        /// Validates requested entity existence in same category and same container
        /// </summary>
        /// <param name="entity">Entity which needs to be validated</param>
        /// <param name="entityOperationResult">Validation results</param>
        /// <param name="callerContext">Indicates application and module name by which action is being performed</param>
        public void ValidateDuplicateEntity(Entity entity, EntityOperationResult entityOperationResult, CallerContext callerContext)
        {
            // In case of entity action create, reclassify and rename check for duplicate entity
            // Import already performs the entity map check. So for create action coming from import, we dont need to do entity map again.
            if ((entity.Action == ObjectAction.Create && callerContext.Module != MDMCenterModules.Import) || entity.Action == ObjectAction.Rename ||
                (entity.Action == ObjectAction.Reclassify && entity.EntityMoveContext != null && entity.EntityMoveContext.ReParentType == ReParentTypeEnum.CategoryReParent))
            {
                Boolean excludeSelfReference = entity.Action == ObjectAction.Rename;
                Boolean isEntityExists = false;
                EntityBL entityManager = new EntityBL();
                isEntityExists = entityManager.Exists(entity, entity.Id, entity.ContainerId, callerContext.Application, callerContext.Module, excludeSelfReference);

                if (isEntityExists)
                {
                    String errorMessage;
                    String parentName;
                    String errorCode;

                    if (entity.EntityTypeId == Constants.CATEGORY_ENTITYTYPE)
                    {
                        //113995: Child category '{0}' already exists in the specified parent category '{1}' and hierarchy '{2}'
                        errorMessage = "Child category '{0}' already exists in the specified parent category '{1}' and hierarchy '{2}'";
                        parentName = entity.ParentEntityName;
                        errorCode = "113995";
                    }
                    else
                    {
                        //113889 : Entity '{0}' already exists in the specified category '{1}' and container '{2}'
                        errorMessage = "Entity '{0}' already exists in the specified category '{1}' and container '{2}'";
                        parentName = entity.CategoryName;
                        errorCode = "113889";
                    }

                    //113889 : Entity '{0}' already exists in the specified category '{1}' and container '{2}'
                    String message = String.Format(errorMessage, entity.Name, parentName, entity.ContainerName);
                    entityOperationResult.AddOperationResult(errorCode, message, new Collection<Object>() { entity.Name, parentName, entity.ContainerName }, ReasonType.DuplicateEntity, -1, -1, OperationResultType.Error);
                }
            }
        }

        #endregion

        #region Private Methods

        #region Entity Validation Helper Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityCollection"></param>
        /// <param name="entityOperationResults"></param>
        /// <param name="callerContext"></param>
        /// <returns></returns>
        private Boolean ValidateBranchLevels(EntityCollection entityCollection, EntityOperationResultCollection entityOperationResults, CallerContext callerContext)
        {
            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Validating branch level for requested entities..");

            Boolean sameBranchLevel = true;

            //Get entities
            IEnumerable<Entity> entities = entityCollection.Where(e => e.BranchLevel == ContainerBranchLevel.Component);

            //Get categories
            IEnumerable<Entity> categories = entityCollection.Where(e => e.BranchLevel == ContainerBranchLevel.Node);

            if (entities != null && entities.Count() > 1 && categories != null && categories.Count() > 1)
            {
                //The incoming entities having both Entity and Category types
                //Entities cannot be processed

                _localeMessage = _localeMessageBL.Get(_systemUILocale, "100043", false, callerContext);
                entityOperationResults.AddOperationResult("100043", _localeMessage.Message, ReasonType.DataModelViolation, -1, -1, OperationResultType.Error);
                sameBranchLevel = false;

                MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, _localeMessage.Message);
            }

            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Branch level validation completed.");

            return sameBranchLevel;
        }

        /// <summary>
        /// Validate user input for executable code of entity fields
        /// </summary>
        /// <param name="entity">entity which needs to be validated</param>
        /// <param name="entityOperationResult">entityOperationResult which needs to be validated</param>
        /// <param name="callerContext">Indicates Application and Module name by which action is being performed</param>
        private void ValidateEntityFieldsForExecutableCode(Entity entity, EntityOperationResult entityOperationResult, CallerContext callerContext)
        {
            if (entity.Action == ObjectAction.Create || entity.Action == ObjectAction.Update || entity.Action == ObjectAction.Rename)
            {
                if (Constants.TRACING_ENABLED)
                {
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Validating Entity Name and LongName for executable code..");
                }


                if (entity.Name.ContainsExecutableCode())
                {
                    LocaleMessage errorMessage = _localeMessageBL.Get(_systemUILocale, "114133", false, callerContext);
                    _parameters = new Object[] { "Name" };
                    entityOperationResult.AddOperationResult("114133", String.Format(errorMessage.Message, "Name"), _parameters.ToCollection(), ReasonType.SystemError, -1, -1, OperationResultType.Error);
                }

                if (entity.LongName.ContainsExecutableCode())
                {
                    LocaleMessage errorMessage = _localeMessageBL.Get(_systemUILocale, "114133", false, callerContext);
                    _parameters = new Object[] { "Long Name" };
                    entityOperationResult.AddOperationResult("114133", String.Format(errorMessage.Message, "Long Name"), _parameters.ToCollection(), ReasonType.SystemError, -1, -1, OperationResultType.Error);
                }

                if (Constants.TRACING_ENABLED)
                {
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Entity Name and LongName for executable code validation completed.");
                }
            }
        }

        /// <summary>
        /// Validate Entity Move Context for Reclassification
        /// </summary>
        /// <param name="entity">entity which needs to be validated</param>
        /// <param name="entityOperationResult">entityOperationResult which needs to be validated</param>
        /// <param name="callerContext">Indicates Application and Module name by which action is being performed</param>
        private void ValidateEntityMoveContext(Entity entity, Hierarchy hierarchy, Dictionary<Int64, Category> categories, EntityOperationResult entityOperationResult, CallerContext callerContext)
        {
            #region Reclassificaton action

            if (entity.Action == ObjectAction.Reclassify)
            {
                if (entity.Id <= 0)
                {
                    _localeMessage = _localeMessageBL.Get(_systemUILocale, "111858", false, callerContext);//Entity Id is not available.Validation of EntityMoveContext cannot be performed.
                    entityOperationResult.AddOperationResult("111858", _localeMessage.Message, ReasonType.MissingOrDeletedEntity, -1, -1, OperationResultType.Error);
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, "Entity Id is not available.Validation  of EntityMoveContext cannot be performed.");
                    return;
                }

                //add error if EntityMoveContext is null
                if (entity.GetEntityMoveContext() == null)
                {
                    _localeMessage = _localeMessageBL.Get(_systemUILocale, "111859", false, callerContext);//EntityMoveContext is not available.
                    entityOperationResult.AddOperationResult("111859", _localeMessage.Message, ReasonType.SystemError, -1, -1, OperationResultType.Error);
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, "EntityMoveContext is not available.");
                    return;
                }

                if (entity.EntityMoveContext.ReParentType == ReParentTypeEnum.CategoryReParent)
                {
                    //add error if EntityMoveContext.TargetCategoryId is null
                    if (entity.EntityMoveContext.TargetCategoryId <= 0)
                    {
                        _localeMessage = _localeMessageBL.Get(_systemUILocale, "111854", false, callerContext);//TargetCategory for reclassification is not valid.
                        entityOperationResult.AddOperationResult("111854", _localeMessage.Message, ReasonType.IncorrectCategory, -1, -1, OperationResultType.Error);
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, "TargetCategory for reclassification is not valid.");
                        return;
                    }

                    //add error if EntityMoveContext.FromCategoryId is null
                    if (entity.EntityMoveContext.FromCategoryId <= 0)
                    {
                        _localeMessage = _localeMessageBL.Get(_systemUILocale, "111855", false, callerContext);//FromCategoryId for reclassification is not valid.
                        entityOperationResult.AddOperationResult("111855", _localeMessage.Message, ReasonType.IncorrectCategory, -1, -1, OperationResultType.Error);
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, "FromCategoryId for reclassification is not valid.");
                        return;
                    }

                    //add error if an entity is a child entity
                    if (entity.ParentEntityId != entity.CategoryId)
                    {
                        _parameters = new Object[] { entity.ParentEntityLongName };
                        _localeMessage = _localeMessageBL.Get(_systemUILocale, "111856", _parameters, false, callerContext);//Entity's parent '{0}' is not available in target category. Please move the parent entity.
                        entityOperationResult.AddOperationResult("111856", _localeMessage.Message, _parameters.ToCollection(), ReasonType.IncorrectCategory, -1, -1, OperationResultType.Error);
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, String.Format("Entity's parent {0} is not available in target category. Please move the parent entity.", entity.ParentEntityLongName));
                        return;
                    }

                    //add error if an entity is being moved to the same category
                    if (entity.EntityMoveContext.TargetCategoryId == entity.CategoryId)
                    {
                        _localeMessage = _localeMessageBL.Get(_systemUILocale, "111857", false, callerContext);//Entity's category is same as of the target category. Entity cannot be reclassified to the same category.
                        entityOperationResult.AddOperationResult("111857", _localeMessage.Message, ReasonType.DuplicateEntity, -1, -1, OperationResultType.Error);
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, "Entity's category is same as of the target category. Entity cannot be reclassified to the same category.");
                        return;
                    }

                    if (hierarchy != null && hierarchy.LeafNodeOnly)
                    {
                        Category category = GetCategory(categories, entity.EntityMoveContext.TargetCategoryId, hierarchy.Id, callerContext);

                        if (category != null && !category.IsLeaf)
                        {
                            _parameters = new Object[] { entity.CategoryLongName };
                            _localeMessage = _localeMessageBL.Get(_systemUILocale, "114252", false, callerContext);
                            entityOperationResult.AddOperationResult("114252", _localeMessage.Message, _parameters.ToCollection(), ReasonType.Unknown, -1, -1, OperationResultType.Error);
                        }
                    }

                }
                else if (entity.EntityMoveContext.ReParentType == ReParentTypeEnum.HiearchyReParent)
                {
                    //add error if EntityMoveContext.TargetParentEntityId is not available
                    if (entity.EntityMoveContext.TargetParentEntityId <= 0)
                    {
                        //TODO::Generate new message code
                        //localeMessage = localeMessageBL.Get(systemUILocale, "111854", false, callerContext);//TargetParentEntity for reclassification is not valid.
                        entityOperationResult.AddOperationResult("111854", "TargetParentEntity for reclassification is not valid.", ReasonType.IncorrectCategory, -1, -1, OperationResultType.Error);
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, "TargetParentEntity for reclassification is not valid.");
                        return;
                    }

                    //TODO:: add error if the current entity is not in the same Catalog and Category of the parent entity

                    //TODO:: add error if the current entity and parent entity's are not meeting defined entity hierarchy

                    //TODO:: add error if an entity is being reclassified to the same entity
                }

                //VISHAL:: WHY ARE WE ASSIGNING VALUES TO ENTITY DURING VALIDATION. THIS IS VIOLATION OF PRINCIPALS. THIS MUST NOT BE DONE.....
                //Change CategoryId and ParentEntityId to TargetCategoryId for EntityProcess 
                //Reason of changing CategoryId and ParentEntityId in between validation is that, after EntityMoveContext validation immediately we have ValidateDuplicateEntitiy
                //ValidateDuplicateEntitiy will check entity exist in same category and same container or not.
                EntityFillHelper.FillEntityUsingEntityMoveContext(entity);
            }

            #endregion

            #region Entity ReParent Validation

            if (entity.Action == ObjectAction.ReParent)
            {
                if (entity.Id <= 0)
                {
                    _localeMessage = _localeMessageBL.Get(_systemUILocale, "111858", false, callerContext);//Entity Id is not available.Validation of EntityMoveContext cannot be performed.
                    entityOperationResult.AddOperationResult("111858", _localeMessage.Message, ReasonType.MissingOrDeletedEntity, -1, -1, OperationResultType.Error);
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, "Entity Id is not available.Validation  of EntityMoveContext cannot be performed.");
                    return;
                }

                //add error if EntityMoveContext is null
                if (entity.EntityMoveContext == null)
                {
                    _localeMessage = _localeMessageBL.Get(_systemUILocale, "111859", false, callerContext);//EntityMoveContext is not available.
                    entityOperationResult.AddOperationResult("111859", _localeMessage.Message, ReasonType.SystemError, -1, -1, OperationResultType.Error);
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, "EntityMoveContext is not available.");
                    return;
                }

                if (entity.EntityMoveContext.ReParentType == ReParentTypeEnum.ExtensionReParent)
                {
                    //add error if EntityMoveContext.TargetParentExtensionEntityId is null
                    if (entity.EntityMoveContext.TargetParentExtensionEntityId <= 0)
                    {
                        //TODO:: Vishal: Correct message code here..
                        _localeMessage = _localeMessageBL.Get(_systemUILocale, "111854", false, callerContext);//TargetCategory for reclassification is not valid.
                        entityOperationResult.AddOperationResult("111854", _localeMessage.Message, ReasonType.Unknown, -1, -1, OperationResultType.Error);
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, "TargetParentExtensionEntityId for reclassification is not valid.");
                        return;
                    }

                    //add error if an entity is being moved to the same parent extension
                    if (entity.EntityMoveContext.TargetParentExtensionEntityId == entity.ParentExtensionEntityId)
                    {
                        //TODO:: Vishal: Correct message code here..
                        _localeMessage = _localeMessageBL.Get(_systemUILocale, "111857", false, callerContext);//Entity's parent extension is same as of the target parent extension. Entity cannot be reparented to the same item.
                        entityOperationResult.AddOperationResult("111857", _localeMessage.Message, ReasonType.IncorrectExtension, -1, -1, OperationResultType.Error);
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, "//Entity's parent extension is same as of the target parent extension. Entity cannot be reparented to the same item.");
                        return;
                    }
                }
            }

            #endregion  Entity ReParent Validation
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="entityOperationResult"></param>
        /// <param name="allowableLocales"></param>
        /// <param name="callerContext"></param>
        /// <param name="entityProcessingOptions"></param>
        private void ValidateEntityAttributes(Entity entity, EntityOperationResult entityOperationResult, LocaleCollection allowableLocales, CallerContext callerContext, EntityProcessingOptions entityProcessingOptions)
        {
            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Validating Entity Attributes..");

            if (entity.Attributes != null && entity.Attributes.Count > 0)
            {
                #region Attribute Models Get

                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Getting attribute models for below details..");

                AttributeModelCollection attributeModelCollection = null;

                if (entity.AttributeModels != null && entity.AttributeModels.Count > 0)
                {
                    attributeModelCollection = entity.AttributeModels;
                }
                else
                {
                    //Prepare the entity attributeId list
                    Collection<Int32> attributeIds = entity.Attributes.GetAttributeIdList();

                    //Get Attribute Models for the prepared id list
                    //TODO :: AttributeModelContext.Locales : Passing multiple locale value form EntityContext to AttributeModelContext
                    AttributeModelContext attributeModelContext = new AttributeModelContext(entity.ContainerId, entity.EntityTypeId, -1, entity.CategoryId, entity.EntityContext.DataLocales, 0, AttributeModelType.All, false, false, true);
                    if (Constants.TRACING_ENABLED)
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("Attribute Model Context - AttributeModelType: {0} Container: {1} EntityType: {2} Category: {3} RelationshipType: {4}", attributeModelContext.AttributeModelType.ToString(), entity.ContainerLongName, entity.EntityTypeLongName, entity.CategoryLongName, -1));

                    AttributeModelBL attributeModelManager = new AttributeModelBL();
                    attributeModelCollection = attributeModelManager.GetByIds(attributeIds, attributeModelContext);
                }

                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Attribute Models get completed.");

                #endregion

                Boolean isAttributeDependencyEnabled = AppConfigurationHelper.GetAppConfig<Boolean>("MDMCenter.AttributeDependency.Enabled", false);
                Boolean isCollectionAttributeDependencyEnabled = AppConfigurationHelper.GetAppConfig<Boolean>("MDMCenter.AttributeDependency.CollectionAttribute.Enabled", false);

                if (isAttributeDependencyEnabled)
                {
                    if (Constants.TRACING_ENABLED)
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Starting attribute dependency Validation...");

                    ValidateAttributeDependency(entity, callerContext, entityOperationResult, isCollectionAttributeDependencyEnabled, entityProcessingOptions.AllowInvalidData);

                    if (Constants.TRACING_ENABLED)
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Completed the attribute dependency Validation.");
                }

                AttributeOperationResultCollection aorCollection = entityOperationResult.AttributeOperationResultCollection;

                if (attributeModelCollection != null && attributeModelCollection.Count > 0)
                {
                    if (entity.Attributes != null && entity.Attributes.Count > 0)
                    {
                        //Validate attribute values against model
                        foreach (Attribute attribute in entity.Attributes)
                        {
                            if (Constants.TRACING_ENABLED)
                                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("Starting validation for Attribute - Id: {0} LongName: {1} Locale: {2}", attribute.Id, attribute.LongName, attribute.Locale.ToString()));

                            //Do not validate if action is Ignore, Delete or Read
                            if (IsQualifiedForValidation(attribute.Action, callerContext))
                            {
                                //Get the attribute model for this attribute
                                AttributeModel attributeModel = null;

                                try
                                {
                                    attributeModel = (AttributeModel)attributeModelCollection.GetAttributeModel(attribute.Id, attribute.Locale);

                                    //If Attribute dependency functionality is enabled and attribute model is null try to get it from original entity
                                    //If the flag set as 'false' then the original entity does not contains the attribute model.
                                    if (attributeModel == null && isAttributeDependencyEnabled)
                                    {
                                        //May be this attribute is here due to the impact of dependent attribute change so try to get it from original entity.
                                        if (entity.OriginalEntity != null && entity.OriginalEntity.GetAttributeModels() != null)
                                        {
                                            attributeModel = (AttributeModel)entity.GetOriginalEntity().GetAttributeModels().GetAttributeModel(attribute.Id, attribute.Locale);
                                        }
                                    }
                                }
                                catch (InvalidOperationException)
                                {
                                    _parameters = new Object[] { attribute.Id, attribute.LongName, attribute.Locale };
                                    _localeMessage = _localeMessageBL.Get(_systemUILocale, "111807", _parameters, false, callerContext);

                                    throw new MDMOperationException("111807", _localeMessage.Message, "EntityManager", String.Empty, "ValidateEntityAttributes", ReasonType.DataModelViolation, -1, _parameters); //More than one attribute models found for Attribute - Id: {0} LongName: {1} Locale: {2}
                                }

                                //Get the attribute operation result for this attribute
                                AttributeOperationResult attributeOperationResult = null;
                                try
                                {
                                    attributeOperationResult = aorCollection.GetAttributeOperationResult(attribute.Id, attribute.Locale);
                                }
                                catch (InvalidOperationException)
                                {
                                    throw new InvalidOperationException("More than one attribute operation results found for attribute Id:" + attribute.Id);
                                }

                                if (allowableLocales != null && allowableLocales.Count > 0 && attributeModel != null && allowableLocales.Contains(attributeModel.Locale))
                                {
                                    Boolean isCategoryAttributeProcess = false;

                                    if (entity.EntityTypeId == Constants.CATEGORY_ENTITYTYPE)
                                    {
                                        isCategoryAttributeProcess = true;
                                    }

                                    // Here we do validation for import also. So that, import's errors also can be logged into Validation Error table.
                                    if (callerContext.Module == MDMCenterModules.Import)
                                    {
                                        ILocaleMessageManager localeMessageManager = new LocaleMessageBL();
                                        EntityOperationsHelper.ValidateEntityAttributesForImport(entity, null, attribute, attributeModel, attributeOperationResult, entityProcessingOptions, callerContext, localeMessageManager, false);
                                    }

                                    ValidateAttribute(attribute, attributeModel, attributeOperationResult, callerContext, entityProcessingOptions, "Attribute", isCategoryAttributeProcess);
                                }
                                else
                                {
                                    Collection<Object> messageParams = new Collection<Object>() {
                                    String.IsNullOrWhiteSpace(attribute.LongName) ? attribute.Name : attribute.LongName, 
                                    String.IsNullOrWhiteSpace(attribute.AttributeParentLongName) ? attribute.AttributeParentName : attribute.AttributeParentLongName, 
                                    attribute.Locale.ToString(), 
                                    String.IsNullOrWhiteSpace(entity.ContainerName) ? entity.ContainerName : entity.ContainerLongName };
                                    //TO-DO Needs to use overload function which has reason type parameter once dependent check - in is done.
                                    attributeOperationResult.AddOperationResult("114019", String.Format("Attribute with the name '{0}', parent name '{1}' and locale '{2}' is ignored as the attribute locale is not mapped to the entity container '{3}'.", messageParams.ToArray()), messageParams, OperationResultType.Warning); //TO-DO Locale message should be updated
                                }

                                //If attribute doesn't have any error and still invalid values flag is set to true then set it as false.
                                if (attributeOperationResult != null && !attributeOperationResult.HasError && attribute.HasInvalidValues)
                                {
                                    attribute.HasInvalidValues = false;
                                    SetAttributeAction(attribute, callerContext, ObjectAction.Update);
                                }

                                if (Constants.TRACING_ENABLED)
                                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("Validation completed for Attribute - Id: {0} LongName: {1} Locale: {2}", attribute.Id, attribute.LongName, attribute.Locale.ToString()));
                            }
                            else
                            {
                                if (Constants.TRACING_ENABLED)
                                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("Skipping validation as action '{0}' does not match with validation criteria.", attribute.Action.ToString()));
                            }
                        }

                        //Update entity operation result status
                        aorCollection.RefreshOperationResultStatus();

                        if (aorCollection.OperationResultStatus == OperationResultStatusEnum.Failed || aorCollection.OperationResultStatus == OperationResultStatusEnum.CompletedWithErrors)
                        {
                            entityOperationResult.OperationResultStatus = OperationResultStatusEnum.Failed;
                        }
                        else if (aorCollection.OperationResultStatus == OperationResultStatusEnum.CompletedWithWarnings)
                        {
                            entityOperationResult.OperationResultStatus = OperationResultStatusEnum.CompletedWithWarnings;
                        }
                    }
                }
                else
                {
                    //Attribute Models are not available for the requested entity context. Add error.
                    _parameters = new Object[] { entity.ContainerLongName, entity.EntityTypeLongName, entity.CategoryLongName, -1 };
                    _localeMessage = _localeMessageBL.Get(_systemUILocale, "114208", _parameters, false, callerContext);
                    entityOperationResult.AddOperationResult("114208", _localeMessage.Message, _parameters.ToCollection(), ReasonType.DataModelViolation, -1, -1, OperationResultType.Error);
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, _localeMessage.Message);
                }
            }
            else
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "No attributes found for entity. Exiting validation.");
            }

            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Entity Attributes validation completed.");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="entityOperationResult"></param>
        /// <param name="callerContext"></param>
        /// <param name="entityProcessingOptions"></param>
        private void ValidateEntityRelationships(Entity entity, EntityOperationResult entityOperationResult, CallerContext callerContext, EntityProcessingOptions entityProcessingOptions)
        {
            if (Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Validating Entity Relationships..");
            }

            if (entity.Relationships != null && entity.Relationships.Count > 0)
            {
                OperationResultType operationResultType = OperationResultType.Error;

                RelationshipOperationResultCollection rorCollection = entityOperationResult.RelationshipOperationResultCollection;

                Dictionary<Int32, RelationshipCardinalityCollection> relationshipCardinalities = new Dictionary<Int32, RelationshipCardinalityCollection>();

                #region Validate Relationship

                foreach (Relationship relationship in entity.Relationships)
                {
                    if (Constants.TRACING_ENABLED)
                    {
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("Starting validation for Relationship - Id: {0} LongName: {1} RelationshipType: {2}", relationship.Id, relationship.LongName, relationship.RelationshipTypeName));
                    }

                    //Get the relationship operation result for this relationship
                    RelationshipOperationResult relationshipOperationResult = null;

                    //Self relationship is not allowed, so putting check here that if from entity and related entity are same then display error.
                    if (relationship.FromEntityId != relationship.RelatedEntityId)
                    {
                        if (IsQualifiedForValidation(relationship.Action, callerContext))
                        {
                            RelationshipCardinalityCollection currentRelationshipCardinality = null;

                            if (relationshipCardinalities.ContainsKey(relationship.RelationshipTypeId))
                            {
                                currentRelationshipCardinality = relationshipCardinalities[relationship.RelationshipTypeId];
                            }

                            if (currentRelationshipCardinality == null)
                            {
                                currentRelationshipCardinality = new RelationshipBL().GetRelationshsipCardinalities(relationship.RelationshipTypeId, entity.ContainerId, entity.EntityTypeId, callerContext);

                                if (currentRelationshipCardinality != null)
                                {
                                    relationshipCardinalities.Add(relationship.RelationshipTypeId, currentRelationshipCardinality);
                                }
                            }

                            relationshipOperationResult = GetRelationshipsOperationResult(relationship, rorCollection, callerContext);

                            //Validate whether the new relationship being created has already exists or sent twice for creation...
                            if (relationship.Action == ObjectAction.Create)
                            {
                                ValidateDuplicateRelationships(relationship, entity, relationshipOperationResult, OperationResultType.Error);
                            }

                            if (relationship.Action != ObjectAction.Ignore && relationshipOperationResult.OperationResultStatus != OperationResultStatusEnum.Failed)
                            {
                                ValidateRelationship(entity, entity.ContainerId, entity.ContainerLongName, entity.EntityTypeId, entity.EntityTypeLongName, relationship, entity.EntityContext.RelationshipContext.DataLocales, relationshipOperationResult, callerContext, entityProcessingOptions, currentRelationshipCardinality, relationshipCardinalities);
                            }

                            if (Constants.TRACING_ENABLED)
                            {
                                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("Validation completed for Relationship - Id: {0} LongName: {1} RelationshipType: {2}", relationship.Id, relationship.LongName, relationship.RelationshipTypeName));
                            }
                        }
                        else
                        {
                            if (Constants.TRACING_ENABLED)
                            {
                                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("Skipping validation as action '{0}' does not match with validation criteria.", relationship.Action.ToString()));
                            }
                        }
                    }
                    else
                    {
                        relationshipOperationResult = GetRelationshipsOperationResult(relationship, rorCollection, callerContext);

                        String errorMessage = String.Format(_localeMessageBL.Get(_systemUILocale, "111698", false, callerContext).Message, relationship.ToExternalId);//Self relationship is not allowed. ToExternalId: {0}
                        relationshipOperationResult.AddOperationResult("111698", errorMessage, new Collection<Object> { relationship.ToExternalId }, ReasonType.RelationshipCheck, -1, -1, operationResultType);
                    }
                }

                #endregion

                #region Validate Min and Max Cardinalities

                RelationshipCollection originalRelationships = null;

                if (entity.OriginalEntity != null)
                {
                    originalRelationships = entity.OriginalEntity.Relationships;
                }

                ValidateRelationshipCardinality(entity.Relationships, originalRelationships, relationshipCardinalities, rorCollection, operationResultType);

                #endregion

                //Update entity operation result status
                rorCollection.RefreshOperationResultStatus();

                if (rorCollection.OperationResultStatus == OperationResultStatusEnum.Failed || rorCollection.OperationResultStatus == OperationResultStatusEnum.CompletedWithErrors)
                {
                    entityOperationResult.OperationResultStatus = OperationResultStatusEnum.Failed;
                }
                else if (rorCollection.OperationResultStatus == OperationResultStatusEnum.CompletedWithWarnings)
                {
                    entityOperationResult.OperationResultStatus = OperationResultStatusEnum.CompletedWithWarnings;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="msgCode"></param>
        /// <param name="message"></param>
        /// <param name="systemDefaultUILocale"></param>
        /// <param name="entityOR"></param>
        /// <param name="callerContext"></param>
        /// <param name="parameters"></param>
        /// <param name="reasonType"></param>
        /// <param name="ruleMapContextId"></param>
        private void ValidateEntityProperty(Entity entity, String msgCode, String message, LocaleEnum systemDefaultUILocale, EntityOperationResultCollection entityOR, CallerContext callerContext, Collection<Object> parameters, ReasonType reasonType, Int32 ruleMapContextId)
        {
            var parameterArray = parameters.ToArray<Object>();
            MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, String.Format(message, parameterArray));
            String localeMsg = this.GetFormattedLocaleMessage(msgCode, systemDefaultUILocale, message, callerContext, parameterArray);
            entityOR.AddEntityOperationResult(entity.Id, msgCode, localeMsg, parameters, reasonType, -1, -1, OperationResultType.Error);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="container"></param>
        /// <param name="allowableLocalesForContainers"></param>
        /// <param name="hierarchies"></param>
        /// <param name="allowableLocales"></param>
        /// <param name="hierarchy"></param>
        private void PopulateAllowableLocalesAndHierarchy(Entity entity, Container container, Dictionary<Int32, LocaleCollection> allowableLocalesForContainers, HierarchyCollection hierarchies, out LocaleCollection allowableLocales, out Hierarchy hierarchy)
        {
            if (!allowableLocalesForContainers.TryGetValue(container.Id, out allowableLocales))
            {
                allowableLocales = container.SupportedLocales;
                allowableLocalesForContainers.Add(container.Id, container.SupportedLocales);
            }

            hierarchy = (Hierarchy)hierarchies.Get(container.HierarchyId);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="categories"></param>
        /// <param name="categoryId"></param>
        /// <param name="hierarchyId"></param>
        /// <param name="callerContext"></param>
        /// <returns></returns>
        private Category GetCategory(Dictionary<Int64, Category> categories, Int64 categoryId, Int32 hierarchyId, CallerContext callerContext)
        {
            Category category = null;

            if (!categories.TryGetValue(categoryId, out category))
            {
                category = new CategoryBL().GetById(hierarchyId, categoryId, callerContext);

                if (category != null)
                {
                    categories.Add(category.Id, category);
                }
            }

            return category;
        }

        /// <summary>
        /// Gets relationship operation result based on provided relationship
        /// </summary>
        /// <param name="relationship">Indicates relationship object for which relationship operation result to be featched</param>
        /// <param name="relationshipOperationResults">Indicates relationship object collection</param>
        /// <param name="callerContext">Indicates application and module name of caller</param>
        /// <returns>Returns relationship operation result</returns>
        private RelationshipOperationResult GetRelationshipsOperationResult(Relationship relationship, RelationshipOperationResultCollection relationshipOperationResults, CallerContext callerContext)
        {
            RelationshipOperationResult relationshipOperationResult = null;

            try
            {
                relationshipOperationResult = relationshipOperationResults.SingleOrDefault(r => r.RelationshipId == relationship.Id);

                if (relationshipOperationResult == null)
                {
                    relationshipOperationResult = relationshipOperationResults.Where(r => r.RelationshipId == relationship.RowId).FirstOrDefault();
                }
            }
            catch (InvalidOperationException)
            {
                _localeMessage = _localeMessageBL.Get(_systemUILocale, "111808", new object[] { relationship.Id }, false, callerContext);
                throw new MDMOperationException("111808", _localeMessage.Message, "EntityManager", String.Empty, "ValidateEntityRelationships", ReasonType.ApplicationError, -1); //More than one relationship operation results found for relationship Id: {0}
            }

            return relationshipOperationResult;
        }

        #endregion

        #region Attribute Validation Helper Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="attribute"></param>
        /// <param name="attributeModel"></param>
        /// <param name="attributeOperationResult"></param>
        /// <param name="callerContext"></param>
        /// <param name="entityProcessingOptions"></param>
        /// <param name="attributeType"></param>
        /// <param name="isCategoryAttributeProcess"></param>
        private void ValidateAttribute(Attribute attribute, AttributeModel attributeModel, AttributeOperationResult attributeOperationResult, CallerContext callerContext, EntityProcessingOptions entityProcessingOptions, String attributeType, Boolean isCategoryAttributeProcess = false)
        {
            OperationResultType operationResultType = OperationResultType.Error;
            Boolean isComplexAttributeValueCaseSensitive = AppConfigurationHelper.GetAppConfig<Boolean>("MDMCenter.AttributeModelManager.ComplexAttributeValues.CaseSensitive.Enabled", false);

            if (attributeModel != null)
            {
                if (attributeModel.InheritableOnly &&
                    attributeModel.AttributeModelType != AttributeModelType.Relationship && !isCategoryAttributeProcess)
                {
                    attribute.Action = ObjectAction.Ignore;

                    if (attribute.OverriddenValues != null && attribute.OverriddenValues.Count > 0)
                    {
                        _parameters = new Object[] { attribute.LongName };
                        String errorMessage = String.Format("Unable to process attribute '{0}', it is set as 'Inheritable Only'.", _parameters);
                        LocaleMessageBL localeMessageBL = new LocaleMessageBL();

                        String localeMessage = localeMessageBL.TryGet(_systemUILocale, "114198", errorMessage, _parameters, false, callerContext).Message;

                        attributeOperationResult.AddOperationResult(String.Empty, localeMessage, _parameters.ToCollection(), ReasonType.DataModelViolation, -1, -1, OperationResultType.Error);
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Warning, errorMessage);
                        return;
                    }
                }
                else
                {
                    if (Constants.TRACING_ENABLED)
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("Starting validation for Attribute - Id: {0} LongName: {1} Locale: {2}", attribute.Id, attribute.LongName, attribute.Locale));

                    if (attributeModel.IsComplex)
                    {
                        if (Constants.TRACING_ENABLED)
                            MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Attribute is of type complex");

                        #region 'DataModelSecurity' Validation

                        Boolean applyDMS = (entityProcessingOptions != null) ? entityProcessingOptions.ApplyDMS : false;

                        //If Attribute doesn't have update permission then it should show warning and skip attribute to get processed
                        if (applyDMS && attribute.CheckHasChanged() && !attributeModel.IsHierarchicalChild && attributeModel.PermissionSet != null && !attributeModel.PermissionSet.Contains(UserAction.Update))
                        {
                            _parameters = new Object[] { attribute.Id, attribute.Name };
                            _localeMessage = _localeMessageBL.Get(_systemUILocale, "100046", _parameters, false, callerContext);
                            attributeOperationResult.AddOperationResult("100046", _localeMessage.Message, _parameters.ToCollection(), ReasonType.PermissionCheck, -1, -1, operationResultType);
                            MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, _localeMessage.Message);
                            return;
                        }

                        #endregion

                        #region 'Required' Validation

                        if ((IsQualifiedForValidation(attribute.Action, callerContext))
                            && attributeModel.Required && !attribute.HasAnyValue() && !attribute.IsHierarchical)
                        {
                            _localeMessage = _localeMessageBL.Get(_systemUILocale, "114204", false, callerContext);
                            attributeOperationResult.AddOperationResult("114204", String.Format("{0}:{1}", attribute.LongName, _localeMessage.Message), null, ReasonType.RequiredAttribute, -1, -1, operationResultType);
                            MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, String.Concat(attribute.LongName, ":", _localeMessage.Message));
                            return;
                        }

                        #endregion

                        #region 'Child Attribute' Validation

                        if (attribute.Attributes != null && attribute.Attributes.Count > 0 &&
                                attributeModel.AttributeModels != null && attributeModel.AttributeModels.Count > 0)
                        {
                            if (Constants.TRACING_ENABLED)
                                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Starting instance records validation..");

                            List<String> complexAttributeValues = new List<String>();

                            foreach (Attribute attrInstance in attribute.Attributes)
                            {
                                if (attrInstance.Action != ObjectAction.Delete && attrInstance.Action != ObjectAction.Ignore)
                                {
                                    if (attrInstance.Attributes != null && attrInstance.Attributes.Count > 0)
                                    {
                                        String instanceValue = String.Empty;

                                        //Validate each child attribute
                                        foreach (Attribute childAttribute in attrInstance.Attributes)
                                        {
                                            if (childAttribute.Action != ObjectAction.Ignore && childAttribute.Action != ObjectAction.Delete)
                                            {
                                                AttributeModel childAttributeModel = null;

                                                try
                                                {
                                                    childAttributeModel = attributeModel.AttributeModels.SingleOrDefault(a => a.Id == childAttribute.Id);
                                                }
                                                catch (InvalidOperationException)
                                                {
                                                    _localeMessage = _localeMessageBL.Get(_systemUILocale, "111809", new object[] { childAttribute.Id }, false, callerContext);
                                                    throw new MDMOperationException("111809", _localeMessage.Message, "EntityManager", String.Empty, "ValidateAttribute", ReasonType.ApplicationError, -1); //More than one attribute models found for Id: {0}
                                                }

                                                if (childAttributeModel != null)
                                                {
                                                    if (childAttribute.IsHierarchical)
                                                    {
                                                        // Validate hierarchical child attribute recursively
                                                        ValidateAttribute(childAttribute, childAttributeModel, attributeOperationResult, callerContext, entityProcessingOptions, attributeType);
                                                    }
                                                    else
                                                    {
                                                        ValidateSimpleAttribute(childAttribute, childAttributeModel, attributeOperationResult, callerContext, entityProcessingOptions, attributeType, attribute.IsHierarchical);

                                                        if (childAttribute.HasInvalidOverriddenValues == true)
                                                        {
                                                            attrInstance.HasInvalidOverriddenValues = true;

                                                            attribute.GetOverriddenValues().GetBySequence(attrInstance.Sequence).HasInvalidValue = true;

                                                            //Set attribute action as update only if caller context is set to revalidate.
                                                            SetAttributeAction(attrInstance, callerContext, ObjectAction.Update);
                                                            SetAttributeAction(attribute, callerContext, ObjectAction.Update);
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    _parameters = new Object[] { attribute.Id, attribute.Name };
                                                    _localeMessage = _localeMessageBL.Get(_systemUILocale, "100046", _parameters, false, callerContext);
                                                    attributeOperationResult.AddOperationResult("100046", _localeMessage.Message, _parameters.ToCollection(), ReasonType.PermissionCheck, -1, -1, operationResultType);
                                                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, _localeMessage.Message);
                                                }

                                                //If attribute is validated okie, we need to keep track of all child attribute values for duplicate check for non hierarchical attribute
                                                if (!attribute.IsHierarchical && (attributeOperationResult.OperationResultStatus == OperationResultStatusEnum.Successful || attributeOperationResult.OperationResultStatus == OperationResultStatusEnum.None))
                                                {
                                                    Object currentValue = childAttribute.GetCurrentValue();
                                                    if (currentValue != null)
                                                    {
                                                        instanceValue = String.Concat(instanceValue, "#@#", currentValue.ToString());
                                                    }
                                                }
                                            }
                                        }

                                        if (!attribute.IsHierarchical)
                                        {
                                            //If complex attribute is not case sensitive, compare in same case.
                                            if (isComplexAttributeValueCaseSensitive == false)
                                            {
                                                instanceValue = instanceValue.ToLowerInvariant();
                                            }

                                            // Instance value check applies for complex attribute that is not of hierarchical data type.
                                            if (complexAttributeValues.Contains(instanceValue))
                                            {
                                                String messageCode = "114213";
                                                UpdateAttributeOperationResult(attribute, attributeOperationResult, messageCode, null, ReasonType.DuplicateValuesCheck, -1, -1, operationResultType, callerContext);
                                                return;
                                            }
                                            else
                                            {
                                                if (instanceValue.ContainsExecutableCode())
                                                {
                                                    String messageCode = "114133";
                                                    _parameters = new Object[] { attribute.LongName };

                                                    UpdateAttributeOperationResult(attribute, attributeOperationResult, messageCode, _parameters, ReasonType.SystemError, -1, -1, operationResultType, callerContext);
                                                    return;
                                                }
                                                else
                                                {
                                                    complexAttributeValues.Add(instanceValue);
                                                }
                                            }
                                        }
                                    }
                                }
                            }

                            if (Constants.TRACING_ENABLED)
                                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Instance records validation completed.");
                        }
                        else
                        {
                            _parameters = new Object[] { attribute.Id, attribute.Name };
                            _localeMessage = _localeMessageBL.Get(_systemUILocale, "100046", _parameters, false, callerContext);
                            attributeOperationResult.AddOperationResult("100046", _localeMessage.Message, _parameters.ToCollection(), ReasonType.PermissionCheck, -1, -1, operationResultType);
                            MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, _localeMessage.Message);
                        }

                        #endregion
                    }
                    else
                    {
                        if (Constants.TRACING_ENABLED)
                            MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Attribute is of type simple");
                        ValidateSimpleAttribute(attribute, attributeModel, attributeOperationResult, callerContext, entityProcessingOptions, attributeType);
                    }

                    if (Constants.TRACING_ENABLED)
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("Validation completed for Attribute - Id: {0} LongName: {1} Locale: {2}", attribute.Id, attribute.LongName, attribute.Locale));
                }
            }
            else
            {
                _parameters = new Object[] { attribute.Id, attribute.Name };
                _localeMessage = _localeMessageBL.Get(_systemUILocale, "100046", _parameters, false, callerContext);
                attributeOperationResult.AddOperationResult("100046", _localeMessage.Message, _parameters.ToCollection(), ReasonType.PermissionCheck, -1, -1, operationResultType);
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, _localeMessage.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="attribute"></param>
        /// <param name="attributeModel"></param>
        /// <param name="attributeOperationResult"></param>
        /// <param name="callerContext"></param>
        /// <param name="entityProcessingOptions"></param>
        /// <param name="attributeType"></param>
        /// <param name="isHierarchicalAttribute"></param>
        private void ValidateSimpleAttribute(Attribute attribute, AttributeModel attributeModel, AttributeOperationResult attributeOperationResult, CallerContext callerContext, EntityProcessingOptions entityProcessingOptions, String attributeType, Boolean isHierarchicalAttribute = false)
        {
            OperationResultType operationResultType = OperationResultType.Error;

            //Note: We do not do ReadOnly validation here 

            #region 'DataModelSecurity' Validation

            Boolean applyDMS = (entityProcessingOptions != null) ? entityProcessingOptions.ApplyDMS : false;

            //If Attribute doesn't have update permission then it should show warning and skip attribute to get processed
            //NOTE: Complex Child will not be validated for Permission/Security
            if (applyDMS && !attributeModel.IsComplexChild && !attributeModel.IsHierarchicalChild && attribute.CheckHasChanged() && attributeModel.PermissionSet != null && !attributeModel.PermissionSet.Contains(UserAction.Update))
            {
                _parameters = new Object[] { attribute.Id, attribute.Name };
                _localeMessage = _localeMessageBL.Get(_systemUILocale, "100046", _parameters, false, callerContext);
                attributeOperationResult.AddOperationResult("100046", _localeMessage.Message, _parameters.ToCollection(), ReasonType.PermissionCheck, -1, -1, operationResultType);
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, _localeMessage.Message);
                return;
            }

            #endregion

            #region 'Required' Validation

            if ((IsQualifiedForValidation(attribute.Action, callerContext))
                && attributeModel.Required && !attribute.HasValue(false) && !isHierarchicalAttribute)
            {
                _localeMessage = _localeMessageBL.Get(_systemUILocale, "114204", false, callerContext);
                String message = String.Format("{0}:{1}", attribute.LongName, _localeMessage.Message);
                attributeOperationResult.AddOperationResult("114204", message, ReasonType.RequiredAttribute, -1, -1, operationResultType);
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, message);
                return;
            }

            #endregion

            //all other validations should be done only when there is value
            if (attribute.HasValue())
            {
                #region Multiple Value Validation

                /*Bug: 24890*/
                if (!attributeModel.IsCollection && !attributeModel.IsHierarchicalChild && attribute.GetCurrentValuesInvariant() != null && attribute.GetCurrentValuesInvariant().Count > 1)
                {
                    _parameters = new Object[] { attribute.LongName };
                    _localeMessage = _localeMessageBL.Get(_systemUILocale, "114210", _parameters, false, callerContext);
                    attributeOperationResult.AddOperationResult("114210", _localeMessage.Message, _parameters.ToCollection(), ReasonType.ValueLength, -1, -1, operationResultType);
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, _localeMessage.Message);
                    return;
                }

                #endregion

                #region 'Data' Validation

                //Validate value with respect to Data Type and Locale
                if (!ValidateAttributeValues(attribute, attributeOperationResult, operationResultType, callerContext))
                {
                    return;
                }

                #endregion

                #region 'Lookup' Validation

                //Validate WSID and storage values for lookup attribute
                if (attributeModel.IsLookup)
                {
                    if (!ValidateLookupAttributeValues(attribute, callerContext))
                    {
                        _parameters = new Object[] { attributeModel.LookUpTableName };
                        String messageCode = "114209";
                        UpdateAttributeOperationResult(attribute, attributeOperationResult, messageCode, _parameters, ReasonType.ValueIncorrect, -1, -1, operationResultType, callerContext);

                        return;
                    }
                }

                #endregion

                #region 'AllowableValues' Validation

                //AllowableValues validation is applicable only for only DropDown attributes
                if (attributeModel.AttributeDisplayTypeName.ToLower() == "dropdown")
                {
                    if (!ValidateAttributeValueWithAllowableValues(attribute, attributeModel))
                    {
                        if (!String.IsNullOrWhiteSpace(attributeModel.AllowableValues))
                        {
                            UpdateAttributeOperationResult(attribute, attributeOperationResult, "114233", new Object[] { attributeModel.AllowableValues }, ReasonType.ValueIncorrect, -1, -1, operationResultType, callerContext);
                        }
                        else
                        {
                            UpdateAttributeOperationResult(attribute, attributeOperationResult, "113843", new Object[] { attribute.Name, attribute.AttributeParentName }, ReasonType.ValueIncorrect, -1, -1, operationResultType, callerContext);
                        }

                        return;
                    }
                }

                #endregion

                #region 'Length' Validation

                //Length validation is applicable only for String attributes
                if (attribute.AttributeDataType.ToString().ToLower() == "string")
                {
                    if (!ValidateAttributeValueLength(attribute, attributeModel))
                    {
                        //message can be for 'MinLength', 'MaxLength' or both
                        String messageCode = "100039";
                        ReasonType reasonType = ReasonType.ValueLength;
                        if (attributeModel.MinLength == 0)
                        {
                            messageCode = "100054";
                        }
                        else if (attributeModel.MaxLength == 0)
                        {
                            messageCode = "100053";
                        }

                        _parameters = new Object[] { attributeModel.MinLength, attributeModel.MaxLength };
                        UpdateAttributeOperationResult(attribute, attributeOperationResult, messageCode, _parameters, reasonType, -1, -1, operationResultType, callerContext);
                        return;
                    }
                }

                #endregion

                #region 'Precision' Validation

                if (attribute.AttributeDataType.ToString().ToLower() == "decimal")
                {
                    if (!ValidateAttributeValuePrecision(attribute, attributeModel))
                    {
                        String messageCode = "100044";
                        _parameters = new Object[] { attributeModel.Precision };
                        UpdateAttributeOperationResult(attribute, attributeOperationResult, messageCode, _parameters, ReasonType.ValuePrecision, -1, -1, operationResultType, callerContext);
                        return;
                    }
                }

                #endregion

                #region 'Range' Validation

                //Validate range of attributes
                if (!ValidateAttributeValueRange(attribute, attributeModel))
                {
                    //message can be for 'from', 'to' or both
                    String messageCode = "100040";
                    ReasonType reasonType = ReasonType.ValueIncorrect;
                    if (String.IsNullOrWhiteSpace(attributeModel.RangeFrom))
                    {
                        messageCode = "100052";
                        reasonType = ReasonType.ValueLength;
                    }
                    else if (String.IsNullOrWhiteSpace(attributeModel.RangeTo))
                    {
                        messageCode = "100051";
                        reasonType = ReasonType.ValueLength;
                    }

                    String[] parameters = new String[] { attributeModel.RangeFrom, attributeModel.RangeTo };
                    UpdateAttributeOperationResult(attribute, attributeOperationResult, messageCode, parameters, reasonType, -1, -1, operationResultType, callerContext);

                    return;
                }

                #endregion

                #region 'UOM' Validation

                if (!ValidateAttributeValueUOM(attribute, attributeModel))
                {
                    UpdateAttributeOperationResult(attribute, attributeOperationResult, "100042", new Object[] { attributeModel.AllowableUOM }, ReasonType.UOMCheck, -1, -1, operationResultType, callerContext);

                    return;
                }

                #endregion

                #region 'Custom Expression' Validation

                if (!ValidateAttributeCustomExpression(attribute, attributeModel))
                {
                    if (!String.IsNullOrWhiteSpace(attributeModel.RegExErrorMessage))
                    {
                        LocaleMessage localeMessage = _localeMessageBL.Get(_systemUILocale, attributeModel.RegExErrorMessage, null, false, callerContext);
                        String errorMessage = localeMessage == null ? attributeModel.RegExErrorMessage : localeMessage.Message;
                        attributeOperationResult.AddOperationResult(String.Empty, errorMessage, ReasonType.CustomExpressionCheck, -1, -1, operationResultType);
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, errorMessage);
                    }
                    else
                    {
                        //messageCode = "114207" - Value(s) failed to meet the specified attribute custom expression {0}
                        UpdateAttributeOperationResult(attribute, attributeOperationResult, "114207", new Object[] { attributeModel.AttributeRegEx }, ReasonType.CustomExpressionCheck, -1, -1, operationResultType, callerContext);
                    }

                    return;
                }

                #endregion

                #region 'Duplicate Value' Validation for Collections

                if (attributeModel.IsCollection && !attributeModel.IsHierarchicalChild)
                {
                    IValueCollection attributeValues = attribute.GetOverriddenValuesInvariant();

                    if (attributeValues != null && attributeValues.Count > 0)
                    {
                        foreach (Value value in attributeValues)
                        {
                            IEnumerable<Value> duplicateValues = attributeValues.Where(v => v.ValueEquals(value, StringComparison.InvariantCultureIgnoreCase) && v.Action != ObjectAction.Delete);

                            // Attribute OverriddenValues find itself in ValueCollection.If Duplicate Value found it will return
                            // more than 1 rows otherwise it will always return 1 row.
                            if (duplicateValues != null && duplicateValues.Count() > 1)
                            {
                                String messageCode = "114213";
                                UpdateAttributeOperationResult(attribute, attributeOperationResult, messageCode, null, ReasonType.DuplicateValuesCheck, -1, -1, operationResultType, callerContext);
                                return;
                            }
                        }
                    }
                }

                #endregion
            }
        }

        #endregion

        #region Attribute Value Validation Helper Methods

        private Boolean ValidateAttributeValues(Attribute attribute, AttributeOperationResult attributeOperationResult, OperationResultType operationResultType, CallerContext callerContext)
        {
            Boolean hasValidValues = true;

            try
            {
                if (callerContext == null)
                {
                    callerContext = new CallerContext(MDMCenterApplication.PIM, MDMCenterModules.Entity);
                }

                foreach (Value value in attribute.GetCurrentValuesInvariant())
                {
                    if (value.Action != ObjectAction.Ignore && value.Action != ObjectAction.Delete)
                    {
                        //Check whether locale is valid
                        Locale locale = null;

                        try
                        {
                            locale = this.AvailableLocales.SingleOrDefault(l => l.Name == value.Locale.ToString());
                        }
                        catch (InvalidOperationException)
                        {
                            _localeMessage = _localeMessageBL.Get(_systemUILocale, "111810", new object[] { value.Locale.ToString() }, false, callerContext);
                            throw new MDMOperationException("111810", _localeMessage.Message, "EntityManager", String.Empty, "ValidateAttributeValues", ReasonType.SystemError, -1, -1); //More than one locales found for Name: {1}
                        }

                        if (locale == null)
                        {
                            _localeMessage = _localeMessageBL.Get(_systemUILocale, "111811", false, callerContext);
                            throw new MDMOperationException("111811", _localeMessage.Message, "EntityManager", String.Empty, "ValidateAttributeValues", ReasonType.SystemError, -1, -1); //Locale is not available
                        }

                        //Get Culture Info
                        CultureInfo cultureInfo = CultureInfo.InvariantCulture;

                        String attrVal = String.Empty;

                        if (value.AttrVal != null)
                            attrVal = value.AttrVal.ToString();
                        else
                            continue;

                        switch (attribute.AttributeDataType)
                        {
                            case AttributeDataType.Date:
                                Regex dateRegex = new Regex(@"^(?:(?:(?:0?[13578]|1[02])(\/)31)\1|(?:(?:0?[1,3-9]|1[0-2])(\/)(?:29|30)\2))(?:(?:1[6-9]|[2-9]\d)?\d{4})$|^(?:0?2(\/)29\3(?:(?:(?:1[6-9]|[2-9]\d)?(?:0[48]|[2468][048]|[13579][26])|(?:(?:16|[2468][048]|[3579][26])00))))$|^(?:(?:0?[1-9])|(?:1[0-2]))(\/)(?:0?[1-9]|1\d|2[0-8])\4(?:(?:1[6-9]|[2-9]\d)?\d{4})$");

                                if (!dateRegex.IsMatch(attrVal))
                                {
                                    _localeMessage = _localeMessageBL.Get(_systemUILocale, "111813", false, callerContext);
                                    throw new MDMOperationException("111813", _localeMessage.Message, "EntityManager", String.Empty, "", ReasonType.ValidDate, -1); //Date Time is not a valid format
                                }
                                else
                                {
                                    DateTime.Parse(attrVal, cultureInfo.DateTimeFormat);
                                }
                                break;
                            case AttributeDataType.DateTime:
                                DateTime.Parse(attrVal, cultureInfo.DateTimeFormat);
                                break;
                            case AttributeDataType.Integer:
                                Int32.Parse(attrVal);
                                break;
                            case AttributeDataType.Decimal:
                                String decimalSeparator = cultureInfo.NumberFormat.CurrencyDecimalSeparator;
                                Regex decimalRegEx = new Regex(@"(?:-- Select --)|^[0-9-]*([\" + decimalSeparator + "][0-9]*)?$");

                                if (!decimalRegEx.IsMatch(attrVal))
                                {
                                    _parameters = new Object[] { attribute.AttributeDataType, attribute.Locale };
                                    throw new MDMOperationException("114206", String.Empty, "EntityManager", String.Empty, "ValidateAttributeValues", ReasonType.DataType, -1, -1, _parameters);
                                }

                                break;
                            case AttributeDataType.Boolean:
                                Boolean.Parse(attrVal);
                                break;
                            case AttributeDataType.ImageURL:

                                Regex imageURLRegEx = new Regex(@"(((((h|H)(t|T))|((f|F)))(t|T)(p|P)(s|S?)://)?([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?)?([A-Z,a-z,0-9,_,\-,\$,\#,\@,\!,\^,\&,\+,\%,\(,\),\=,\[,\],\\,\/,\s,:,\.])*" + this.ImageExtensions + "$");
                                if (!imageURLRegEx.IsMatch(attrVal))
                                {
                                    _parameters = new Object[] { attribute.AttributeDataType, attribute.Locale };
                                    throw new MDMOperationException("114206", String.Empty, "EntityManager", String.Empty, "ValidateAttributeValues", ReasonType.DataType, -1, -1, _parameters);
                                }

                                break;
                            case AttributeDataType.URL:
                                Regex urlRegEx = new Regex(@"((((((h|H)(t|T))|((f|F)))(t|T)(p|P)(s|S?)://)|(([a-zA-Z]:)|(\\{2}\w+)\$?)(\\(\w[\w].*)))?((([\w-]+\.)+[\w-]+(/[\w- ./?%&#=]*)?)|(([\w-]+/)+[\w-]+[\.]([\w- /?%&#=]*)?)))");

                                if (!urlRegEx.IsMatch(attrVal))
                                {
                                    _parameters = new Object[] { attribute.AttributeDataType, attribute.Locale };
                                    throw new MDMOperationException("114206", String.Empty, "EntityManager", String.Empty, "ValidateAttributeValues", ReasonType.DataType, -1, -1, _parameters);
                                }
                                break;
                            case AttributeDataType.String:
                                if (attrVal.ContainsExecutableCode())
                                {
                                    _parameters = new Object[] { attribute.LongName };
                                    _localeMessage = _localeMessageBL.Get(_systemUILocale, "114133", _parameters, false, callerContext);
                                    throw new MDMOperationException("114133", _localeMessage.Message, "EntityManager", String.Empty, "ValidateAttributeValues", ReasonType.SystemError, -1, _parameters);
                                }
                                break;
                            case AttributeDataType.Fraction:
                                //Validate for fraction 
                                Regex fractionRegEx = new Regex(@"^-?\d+[\ ,\-,_]{1}\d+\/{1}\d+$");

                                if (!fractionRegEx.IsMatch(attrVal))
                                {
                                    //It is not the mixed fraction. Check for valid whole number or fraction
                                    fractionRegEx = new Regex(@"^-?\d+$|^-?\d+\/{1}\d+$");

                                    if (!fractionRegEx.IsMatch(attrVal))
                                    {
                                        _parameters = new Object[] { attribute.AttributeDataType, attribute.Locale };
                                        throw new MDMOperationException("114206", String.Empty, "EntityManager", String.Empty, "ValidateAttributeValues", ReasonType.DataType, -1, -1, _parameters);
                                    }

                                    //For scenario when denominator is zero.
                                    String[] partsOfFraction = attrVal.Trim().Split('/');
                                    if (partsOfFraction != null && partsOfFraction.Length > 1)
                                    {
                                        if (partsOfFraction[0] != null)
                                        {
                                            Int64 numerator = 0;
                                            Int64.TryParse(partsOfFraction[0], out numerator);

                                            //if numerator is zero, then also it is invalid fraction so exception should be thrown.
                                            if (numerator == 0)
                                            {
                                                //In case of exception, this method catch it and return false to parent method, 
                                                //and parent method adds respective error message code to the attribute operation results.
                                                _parameters = new Object[] { attribute.AttributeDataType, attribute.Locale };
                                                throw new MDMOperationException("114206", String.Empty, "EntityManager", String.Empty, "ValidateAttributeValues", ReasonType.DataType, -1, -1, _parameters);
                                            }
                                        }

                                        if (partsOfFraction[1] != null)
                                        {
                                            Int64 denominator = 0;
                                            Int64.TryParse(partsOfFraction[1], out denominator);

                                            //if denominator is zero, then also it is invalid fraction so exception should be thrown.
                                            if (denominator <= 0)
                                            {
                                                //In case of exception, this method catch it and return false to parent method, 
                                                //and parent method adds respective error message code to the attribute operation results.
                                                _parameters = new Object[] { attribute.AttributeDataType, attribute.Locale };
                                                throw new MDMOperationException("114206", String.Empty, "EntityManager", String.Empty, "ValidateAttributeValues", ReasonType.DataType, -1, -1, _parameters);
                                            }
                                        }

                                    }
                                }
                                else
                                {
                                    //It is a mixed fraction. Check the fractional part is a proper fraction
                                    String fractionalPart = attrVal.Substring(attrVal.IndexOf(' ') + 1);

                                    String[] partsOfFraction = fractionalPart.Trim().Split('/');

                                    Int64 numerator = 0;
                                    Int64 denominator = 0;

                                    if (partsOfFraction != null && partsOfFraction[0] != null && partsOfFraction[1] != null)
                                    {
                                        Int64.TryParse(partsOfFraction[0], out numerator);
                                        Int64.TryParse(partsOfFraction[1], out denominator);

                                        if (numerator >= denominator || numerator < 1)
                                        {
                                            _parameters = new Object[] { attribute.AttributeDataType, attribute.Locale };
                                            throw new MDMOperationException("114206", String.Empty, "EntityManager", String.Empty, "ValidateAttributeValues", ReasonType.DataType, -1, -1, _parameters);
                                        }
                                    }
                                }

                                break;
                        }
                    }
                }
            }
            catch (MDMOperationException mdmOperationException)
            {

                String messageCode = mdmOperationException.MessageCode;
                _parameters = mdmOperationException.MessageArguments;
                ReasonType reasonType = mdmOperationException.ReasonType;
                Int32 ruleMapContextId = mdmOperationException.RuleMapContextId;
                Int32 ruleId = mdmOperationException.RuleId;

                UpdateAttributeOperationResult(attribute, attributeOperationResult, messageCode, _parameters, reasonType, ruleMapContextId, ruleId, operationResultType, callerContext);
                hasValidValues = false;
            }
            catch
            {
                String messageCode = "114206";
                _parameters = new Object[] { attribute.AttributeDataType, attribute.Locale };

                UpdateAttributeOperationResult(attribute, attributeOperationResult, messageCode, _parameters, ReasonType.DataType, -1, -1, operationResultType, callerContext);
                hasValidValues = false;
            }

            return hasValidValues;
        }

        private Boolean ValidateLookupAttributeValues(Attribute attribute, CallerContext callerContext)
        {
            //For lookup attributes, value.GetDisplayValue must be filled in. This filling happens as part of either entity process and/or as part of entity import engine fill attribute..
            //If lookup display value is empty then error our this lookup attribute..
            Boolean isValueValid = true;

            foreach (Value value in attribute.GetCurrentValuesInvariant())
            {
                if (value.Action != ObjectAction.Ignore && value.Action != ObjectAction.Delete && value.ValueRefId > 0)
                {
                    String lookupDisplayValue = value.GetDisplayValue();

                    if (String.IsNullOrWhiteSpace(lookupDisplayValue))
                    {
                        isValueValid = false;
                        break;
                    }
                }
            }

            return isValueValid;
        }

        private Boolean ValidateAttributeValueWithAllowableValues(Attribute attribute, AttributeModel attributeModel)
        {
            Boolean isValueValid = true;

            if (!String.IsNullOrWhiteSpace(attributeModel.AllowableValues))
            {
                String[] allowedValuesList = attributeModel.AllowableValues.Split(new String[] { "||" }, StringSplitOptions.RemoveEmptyEntries);

                foreach (Value value in attribute.GetCurrentValuesInvariant())
                {
                    if (value.Action != ObjectAction.Ignore && value.Action != ObjectAction.Delete)
                    {
                        AttributeDataType attributeDataType = AttributeDataType.String;

                        Enum.TryParse(attributeModel.AttributeDataTypeName, out attributeDataType);

                        //If Attribute Data Type is Decimal then compare attribute current value & allowable values with forming same precision value.

                        //If User set Attribute Precision Value as "3" and inserting allowable value with Precision "2",so we need Decimal Comparison 
                        // to convert both values in same precision to compare based on attribute model precision value.

                        if (attributeDataType == AttributeDataType.Decimal)
                        {
                            Boolean allowableValueFound = false;

                            foreach (String allowedValue in allowedValuesList)
                            {
                                Decimal decimalValue = ValueTypeHelper.ConvertToDecimal(allowedValue);
                                Decimal decimalAttrVal = ValueTypeHelper.ConvertToDecimal(value.AttrVal);

                                if (decimalValue != 0)
                                {
                                    if (String.Format(String.Concat("{0:F", attributeModel.Precision, "}"), decimalValue) == String.Format(String.Concat("{0:F", attributeModel.Precision, "}"), decimalAttrVal))
                                    {
                                        allowableValueFound = true;
                                        break;
                                    }
                                }
                            }

                            if (!allowableValueFound)
                            {
                                isValueValid = false;
                            }
                        }
                        else
                        {
                            if (!allowedValuesList.Contains(value.AttrVal))
                            {
                                isValueValid = false;
                                break;
                            }
                        }
                    }
                }
            }
            //In Attribute Master, for Boolean we dont have allowable values, by default true and false is added.
            //So the value in Boolean dropdown will be valid always

            if (attribute.AttributeDataType == AttributeDataType.Boolean)
            {
                isValueValid = true;
            }

            return isValueValid;
        }

        private Boolean ValidateAttributeValueRange(Attribute attribute, AttributeModel attributeModel)
        {
            Boolean isValueRangeValid = true;
            Boolean checkFrom = false;
            Boolean checkTo = false;

            switch (attribute.AttributeDataType)
            {
                case AttributeDataType.Integer:
                case AttributeDataType.Decimal:
                    Decimal dRangeFrom = 0;
                    Decimal dRangeTo = 0;
                    checkFrom = false;
                    checkTo = false;

                    if (!String.IsNullOrWhiteSpace(attributeModel.RangeFrom.Trim()) && Decimal.TryParse(attributeModel.RangeFrom, out dRangeFrom))
                    {
                        checkFrom = true;
                    }
                    if (!String.IsNullOrWhiteSpace(attributeModel.RangeTo.Trim()) && Decimal.TryParse(attributeModel.RangeTo, out dRangeTo))
                    {
                        checkTo = true;
                    }
                    foreach (Value val in attribute.GetCurrentValuesInvariant())
                    {
                        if (val.Action != ObjectAction.Ignore && val.Action != ObjectAction.Delete)
                        {
                            if (checkFrom && val.NumericVal < dRangeFrom)
                            {
                                isValueRangeValid = false;
                                break;
                            }
                            if (checkTo && val.NumericVal > dRangeTo)
                            {
                                isValueRangeValid = false;
                                break;
                            }
                        }
                    }
                    break;
                case AttributeDataType.Date:
                case AttributeDataType.DateTime:
                    DateTime dtRangeFrom = DateTime.MinValue;
                    DateTime dtRangeTo = DateTime.MaxValue;
                    checkFrom = false;
                    checkTo = false;

                    if (!String.IsNullOrWhiteSpace(attributeModel.RangeFrom.Trim()) && DateTime.TryParse(attributeModel.RangeFrom, out dtRangeFrom))
                    {
                        checkFrom = true;
                    }
                    if (!String.IsNullOrWhiteSpace(attributeModel.RangeTo.Trim()) && DateTime.TryParse(attributeModel.RangeTo, out dtRangeTo))
                    {
                        checkTo = true;
                    }
                    foreach (Value val in attribute.GetCurrentValuesInvariant())
                    {
                        if (val.Action != ObjectAction.Ignore && val.Action != ObjectAction.Delete)
                        {
                            if (checkFrom && val.DateVal < dtRangeFrom)
                            {
                                isValueRangeValid = false;
                                break;
                            }
                            if (checkTo && val.DateVal > dtRangeTo)
                            {
                                isValueRangeValid = false;
                                break;
                            }
                        }
                    }
                    break;
                case AttributeDataType.Fraction:
                    Decimal fRangeFrom = 0;
                    Decimal fRangeTo = 0;
                    checkFrom = false;
                    checkTo = false;

                    if (!String.IsNullOrWhiteSpace(attributeModel.RangeFrom.Trim()))
                    {
                        checkFrom = true;
                        fRangeFrom = ValueTypeHelper.ConvertToDecimal(attributeModel.RangeFrom);
                    }
                    if (!String.IsNullOrWhiteSpace(attributeModel.RangeTo.Trim()))
                    {
                        checkTo = true;
                        fRangeTo = ValueTypeHelper.ConvertToDecimal(attributeModel.RangeTo);
                    }

                    if (!String.IsNullOrWhiteSpace(attributeModel.RangeFrom.Trim()))
                    {
                        foreach (Value val in attribute.GetCurrentValuesInvariant())
                        {
                            if (val.Action != ObjectAction.Ignore && val.Action != ObjectAction.Delete)
                            {
                                if (val.AttrVal != null)
                                {
                                    Decimal decimalOfFraction = ValueTypeHelper.ConvertFractionToDecimal(val.AttrVal.ToString());
                                    if (checkFrom && decimalOfFraction < fRangeFrom)
                                    {
                                        isValueRangeValid = false;
                                        break;
                                    }
                                    if (checkTo && decimalOfFraction > fRangeTo)
                                    {
                                        isValueRangeValid = false;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                    break;
            }

            return isValueRangeValid;
        }

        private Boolean ValidateAttributeValueLength(Attribute attribute, AttributeModel attributeModel)
        {
            Boolean isValueLengthValid = true;

            Int32 minLength = attributeModel.MinLength;
            Int32 maxLength = attributeModel.MaxLength;

            foreach (Value value in attribute.GetCurrentValuesInvariant())
            {
                if (value.Action != ObjectAction.Ignore && value.Action != ObjectAction.Delete)
                {
                    if (value.AttrVal != null)
                    {
                        String attrVal = Regex.Replace(value.AttrVal.ToString(), "<.*?>|&amp;nbsp;", String.Empty); //Removing HTML tags including &nbsp; from the attrval
                        if (minLength > 0)
                        {
                            if (attrVal.Length < minLength)
                            {
                                isValueLengthValid = false;
                                break;
                            }
                        }
                        if (maxLength > 0)
                        {
                            if (attrVal.Length > maxLength)
                            {
                                isValueLengthValid = false;
                                break;
                            }
                        }
                    }
                }
            }

            return isValueLengthValid;
        }

        private Boolean ValidateAttributeValuePrecision(Attribute attribute, AttributeModel attributeModel)
        {
            Boolean isValuePrecisonValid = true;

            Int32 precision = attributeModel.Precision;

            if (precision > 0)
            {
                foreach (Value value in attribute.GetCurrentValuesInvariant())
                {
                    if (value.Action != ObjectAction.Ignore && value.Action != ObjectAction.Delete)
                    {
                        if (value.AttrVal != null)
                        {
                            String attrVal = value.AttrVal.ToString();

                            Locale locale = null;
                            try
                            {
                                //Check whether locale is valid
                                locale = this.AvailableLocales.SingleOrDefault(l => l.Name == value.Locale.ToString());
                            }
                            catch (InvalidOperationException)
                            {
                                _localeMessage = _localeMessageBL.Get(_systemUILocale, "111810", false, new CallerContext(MDMCenterApplication.PIM, MDMCenterModules.Entity));
                                throw new MDMOperationException("111810", _localeMessage.Message, "EntityManager", String.Empty, "ValidateAttributeValuePrecision", ReasonType.SystemError, -1); //More than one locales found for Name: {0}
                            }

                            if (locale == null)
                            {
                                _localeMessage = _localeMessageBL.Get(_systemUILocale, "111811", false, new CallerContext(MDMCenterApplication.PIM, MDMCenterModules.Entity));
                                throw new MDMOperationException("111811", _localeMessage.Message, "EntityManager", String.Empty, "ValidateAttributeValues", ReasonType.SystemError, -1); //Locale is not available
                            }

                            //Get Culture Info
                            CultureInfo cultureInfo = CultureInfo.InvariantCulture;

                            //Commented one is the RegEx used in UI validation which is failing here..
                            //TODO:: Explore why this RegEx is failing here..
                            //To continue with work, adding another RegEx which is satisfying all the scenarios
                            //Regex decimalRegEx = new Regex(@"((?:-- Select --)|[\d-]+\\" + cultureInfo.NumberFormat.CurrencyDecimalSeparator + "?\\d{0," + precision + "}?$)");
                            Regex decimalRegEx = new Regex(@"(?:-- Select --)|^[\d-]+(\" + cultureInfo.NumberFormat.CurrencyDecimalSeparator + "\\d{0," + precision + "})?$");

                            if (!decimalRegEx.IsMatch(attrVal))
                            {
                                isValuePrecisonValid = false;
                                break;
                            }
                        }
                    }
                }
            }

            return isValuePrecisonValid;
        }

        private Boolean ValidateAttributeValueUOM(Attribute attribute, AttributeModel attributeModel)
        {
            Boolean isValueUOMValid = true;

            if (!String.IsNullOrWhiteSpace(attributeModel.AllowableUOM))
            {
                String[] allowableUomList = attributeModel.AllowableUOM.Split(new String[] { "||" }, StringSplitOptions.RemoveEmptyEntries);

                foreach (Value value in attribute.GetCurrentValuesInvariant())
                {
                    if (value.Action != ObjectAction.Ignore && value.Action != ObjectAction.Delete)
                    {
                        if (String.IsNullOrWhiteSpace(value.Uom) || !allowableUomList.Contains(value.Uom))
                            isValueUOMValid = false;
                    }
                }
            }

            return isValueUOMValid;
        }

        private Boolean ValidateAttributeCustomExpression(Attribute attribute, AttributeModel attributeModel)
        {
            Boolean isValidationSuccess = true;

            if (!String.IsNullOrWhiteSpace(attributeModel.AttributeRegEx))
            {
                foreach (Value value in attribute.GetCurrentValuesInvariant())
                {
                    if (value.Action != ObjectAction.Ignore && value.Action != ObjectAction.Delete)
                    {
                        Regex customRegEx = new Regex(attributeModel.AttributeRegEx);

                        if (value.AttrVal != null)
                        {
                            String attrVal = value.AttrVal.ToString();

                            //ensure that the whole string matches the expression
                            //the logic is equal to the one used in UI validation (RegularExpressionValidator control)
                            MatchCollection matches = customRegEx.Matches(attrVal);

                            if (matches.Count == 0 || matches[0].Value != attrVal)
                            {
                                isValidationSuccess = false;
                                break;
                            }
                        }
                    }
                }
            }

            return isValidationSuccess;
        }

        #endregion

        #region Relationship Validation Helper Method

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="containerId"></param>
        /// <param name="containerName"></param>
        /// <param name="entityTypeId"></param>
        /// <param name="entityTypeName"></param>
        /// <param name="relationship"></param>
        /// <param name="dataLocales"></param>
        /// <param name="relationshipOperationResult"></param>
        /// <param name="callerContext"></param>
        /// <param name="entityProcessingOption"></param>
        /// <param name="relationshipCardinality"></param>
        /// <param name="relationshipCardinalities"></param>
        private void ValidateRelationship(Entity entity, Int32 containerId, String containerName, Int32 entityTypeId, String entityTypeName, Relationship relationship, Collection<LocaleEnum> dataLocales, RelationshipOperationResult relationshipOperationResult, CallerContext callerContext, EntityProcessingOptions entityProcessingOption, RelationshipCardinalityCollection relationshipCardinality, Dictionary<Int32, RelationshipCardinalityCollection> relationshipCardinalities)
        {
            try
            {
                #region Validate Relationship Mappings

                if (relationship.Action == ObjectAction.Create)
                {
                    ValidateRelationshipMappings(containerId, containerName, entityTypeId, entityTypeName, relationship, relationshipOperationResult, relationshipCardinality, callerContext, OperationResultType.Error);
                }

                #endregion

                if (relationshipOperationResult.OperationResultStatus != OperationResultStatusEnum.Failed || relationshipOperationResult.OperationResultStatus != OperationResultStatusEnum.CompletedWithErrors)
                {
                    #region Validate Relationship Attributes

                    ValidateRelationshipAttributes(entity, containerId, relationship, dataLocales, relationshipOperationResult, callerContext, entityProcessingOption);

                    #endregion

                    #region Validate Child Relationships

                    if (relationshipOperationResult.OperationResultStatus != OperationResultStatusEnum.Failed || relationshipOperationResult.OperationResultStatus != OperationResultStatusEnum.CompletedWithErrors)
                    {

                        ValidateChildRelationships(entity, relationship, dataLocales, relationshipOperationResult, callerContext, entityProcessingOption, relationshipCardinalities);
                    }

                    #endregion
                }
            }
            catch (MDMOperationException exception)
            {
                _parameters = new Object[] { exception.Message };
                _localeMessage = _localeMessageBL.Get(_systemUILocale, exception.MessageCode, _parameters, false, callerContext);
                //Add error message into operation result
                relationshipOperationResult.AddOperationResult(exception.MessageCode, _localeMessage.Message, _parameters.ToCollection(), exception.ReasonType, exception.RuleMapContextId, exception.RuleId, OperationResultType.Error);
            }
            catch (Exception ex)
            {
                _parameters = new Object[] { ex.Message };
                _localeMessage = _localeMessageBL.Get(_systemUILocale, "114205", _parameters, false, callerContext);
                //Add error message into operation result
                relationshipOperationResult.AddOperationResult("114205", _localeMessage.Message, _parameters.ToCollection(), ReasonType.SystemError, -1, -1, OperationResultType.Error);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="containerId"></param>
        /// <param name="relationship"></param>
        /// <param name="dataLocales"></param>
        /// <param name="relationshipOperationResult"></param>
        /// <param name="callerContext"></param>
        /// <param name="entityProcessingOptions"></param>
        public void ValidateRelationshipAttributes(Entity entity, Int32 containerId, Relationship relationship, Collection<LocaleEnum> dataLocales, RelationshipOperationResult relationshipOperationResult, CallerContext callerContext, EntityProcessingOptions entityProcessingOptions)
        {
            if (relationship.RelationshipAttributes != null && relationship.RelationshipAttributes.Count > 0)
            {
                #region Attribute Models Get

                //Prepare the entity attributeId list
                Collection<Int32> attributeIds = relationship.RelationshipAttributes.GetAttributeIdList();

                //Prepare attribute model context
                AttributeModelContext attributeModelContext = new AttributeModelContext();
                attributeModelContext.ContainerId = containerId;
                attributeModelContext.RelationshipTypeId = relationship.RelationshipTypeId;
                attributeModelContext.AttributeModelType = AttributeModelType.Relationship;

                attributeModelContext.Locales = dataLocales;

                //Get Attribute Models for the prepared id list
                AttributeModelBL attributeModelManager = new AttributeModelBL();
                AttributeModelCollection attributeModelCollection = attributeModelManager.GetByIds(attributeIds, attributeModelContext);

                #endregion

                if (attributeModelCollection != null && attributeModelCollection.Count > 0)
                {
                    AttributeOperationResultCollection attributeOperationResultCollection = relationshipOperationResult.AttributeOperationResultCollection;

                    //Validate attribute values against model
                    foreach (Attribute attribute in relationship.RelationshipAttributes)
                    {
                        //Do not validate if action is Ignore, Delete or Read
                        if (IsQualifiedForValidation(attribute.Action, callerContext))
                        {
                            //Get the attribute model for this attribute
                            AttributeModel attributeModel = null;

                            try
                            {
                                attributeModel = (AttributeModel)attributeModelCollection.GetAttributeModel(attribute.Id, attribute.Locale);
                            }
                            catch (InvalidOperationException)
                            {
                                _localeMessage = _localeMessageBL.Get(_systemUILocale, "111809", new object[] { attribute.Id }, false, callerContext);
                                throw new MDMOperationException("111809", _localeMessage.Message, "EntityManager", String.Empty, "ValidateRelationshipAttributes", ReasonType.ApplicationError, -1); //More than one attribute models found for Id: {0}
                            }

                            //Get the attribute operation result for this attribute
                            AttributeOperationResult attributeOperationResult = null;

                            try
                            {
                                attributeOperationResult = attributeOperationResultCollection.GetAttributeOperationResult(attribute.Id, attribute.Locale);
                            }
                            catch (InvalidOperationException)
                            {
                                _localeMessage = _localeMessageBL.Get(_systemUILocale, "111812", new object[] { attribute.Id }, false, callerContext);
                                throw new MDMOperationException("111812", _localeMessage.Message, "EntityManager", String.Empty, "ValidateRelationshipAttributes", ReasonType.ApplicationError, -1); //More than one attribute operation results found for attribute Id: {0}
                            }

                            if (callerContext.Module == MDMCenterModules.Import)
                            {
                                ILocaleMessageManager localeMessageManager = new LocaleMessageBL();
                                EntityOperationsHelper.ValidateEntityAttributesForImport(entity, relationship, attribute, attributeModel, attributeOperationResult, entityProcessingOptions, callerContext, localeMessageManager, false);
                            }

                            ValidateAttribute(attribute, attributeModel, attributeOperationResult, callerContext, entityProcessingOptions, "RelationshipAttribute");
                        }
                    }

                    //Update entity operation result status
                    attributeOperationResultCollection.RefreshOperationResultStatus();
                    if (attributeOperationResultCollection.OperationResultStatus == OperationResultStatusEnum.Failed || attributeOperationResultCollection.OperationResultStatus == OperationResultStatusEnum.CompletedWithErrors)
                    {
                        relationshipOperationResult.OperationResultStatus = OperationResultStatusEnum.Failed;
                    }
                    else if (attributeOperationResultCollection.OperationResultStatus == OperationResultStatusEnum.CompletedWithWarnings)
                    {
                        relationshipOperationResult.OperationResultStatus = OperationResultStatusEnum.CompletedWithWarnings;
                    }
                }
                else
                {
                    //Attribute Models are not available for the requested entity context. Add error.
                    _parameters = new Object[] { attributeModelContext.ContainerId.ToString(), attributeModelContext.EntityTypeId.ToString(), attributeModelContext.CategoryId.ToString(), attributeModelContext.RelationshipTypeId.ToString() };
                    _localeMessage = _localeMessageBL.Get(_systemUILocale, "114208", _parameters, false, callerContext);
                    relationshipOperationResult.AddOperationResult("114208", _localeMessage.Message, _parameters.ToCollection(), ReasonType.DataModelViolation, -1, -1, OperationResultType.Error);
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, _localeMessage.Message);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="relationship"></param>
        /// <param name="dataLocales"></param>
        /// <param name="relationshipOperationResult"></param>
        /// <param name="callerContext"></param>
        /// <param name="entityProcessingOption"></param>
        /// <param name="relationshipCardinalities"></param>
        private void ValidateChildRelationships(Entity entity, Relationship relationship, Collection<LocaleEnum> dataLocales, RelationshipOperationResult relationshipOperationResult, CallerContext callerContext, EntityProcessingOptions entityProcessingOption, Dictionary<Int32, RelationshipCardinalityCollection> relationshipCardinalities)
        {
            if (relationship.RelationshipCollection != null && relationship.RelationshipCollection.Count > 0)
            {
                RelationshipOperationResultCollection rorCollection = relationshipOperationResult.RelationshipOperationResultCollection;

                foreach (Relationship childRelationship in relationship.RelationshipCollection)
                {
                    if (IsQualifiedForValidation(childRelationship.Action, callerContext))
                    {
                        //Get the relationship operation result for this relationship
                        RelationshipOperationResult childRelationshipOperationResult = null;
                        RelationshipCardinalityCollection relationshipCardinality = null;

                        if (relationshipCardinalities.ContainsKey(childRelationship.RelationshipTypeId))
                        {
                            relationshipCardinality = relationshipCardinalities[childRelationship.RelationshipTypeId];
                        }

                        if (relationshipCardinality == null)
                        {
                            relationshipCardinality = new RelationshipBL().GetRelationshsipCardinalities(childRelationship.RelationshipTypeId, childRelationship.ContainerId, relationship.ToEntityTypeId, new CallerContext(MDMCenterApplication.MDMCenter, MDMCenterModules.Entity));

                            if (relationshipCardinality != null)
                            {
                                relationshipCardinalities.Add(childRelationship.RelationshipTypeId, relationshipCardinality);
                            }
                        }

                        try
                        {
                            childRelationshipOperationResult = rorCollection.SingleOrDefault(r => r.RelationshipId == childRelationship.Id);
                        }
                        catch (InvalidOperationException)
                        {
                            _localeMessage = _localeMessageBL.Get(_systemUILocale, "111808", new object[] { relationship.Id }, false, callerContext);
                            throw new MDMOperationException("111808", _localeMessage.Message, "EntityManager", String.Empty, "ValidateRelationshipAttributes", ReasonType.ApplicationError, -1); //More than one relationship operation results found for relationship Id: {0}
                        }

                        Int32 containerId = 0;
                        if (relationship.ToContainerId > 0)
                        {
                            containerId = relationship.ToContainerId;
                        }
                        else
                        {
                            containerId = childRelationship.ContainerId;
                        }

                        ValidateRelationship(entity, containerId, relationship.ToContainerName, relationship.ToEntityTypeId, relationship.ToEntityTypeName, childRelationship, dataLocales, childRelationshipOperationResult, callerContext, entityProcessingOption, relationshipCardinality, relationshipCardinalities);
                    }
                }

                //Update relationship operation result status
                rorCollection.RefreshOperationResultStatus();

                if (rorCollection.OperationResultStatus == OperationResultStatusEnum.Failed || rorCollection.OperationResultStatus == OperationResultStatusEnum.CompletedWithErrors)
                    relationshipOperationResult.OperationResultStatus = OperationResultStatusEnum.Failed;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="updatedRelationships"></param>
        /// <param name="originalEntityRelationships"></param>
        /// <param name="relationshipCardinalities"></param>
        /// <param name="relationshipOperationResultCollection"></param>
        /// <param name="operationResultType"></param>
        private void ValidateRelationshipCardinality(RelationshipCollection updatedRelationships, RelationshipCollection originalEntityRelationships, Dictionary<Int32, RelationshipCardinalityCollection> relationshipCardinalities, RelationshipOperationResultCollection relationshipOperationResultCollection, OperationResultType operationResultType)
        {
            foreach (KeyValuePair<Int32, RelationshipCardinalityCollection> cardinality in relationshipCardinalities)
            {
                Int32 relationshipTypeId = cardinality.Key;
                RelationshipCardinalityCollection relCardinality = cardinality.Value;
                String relationshipTypeName = String.Empty;

                if (relCardinality != null && relCardinality.Count > 0)
                {
                    foreach (RelationshipCardinality relationshipCardinality in relCardinality)
                    {
                        //NOTE: If ToEntityTypeId is not available or max relationships set as 0, that means allow N number of relationships and no validation will happen.

                        if (relationshipCardinality.ToEntityTypeId > 0 && relationshipCardinality.MaxRelationships > 0)
                        {
                            RelationshipCollection cardinalitySettingsMatchingRelationships = new RelationshipCollection();

                            foreach (Relationship updatedRelationship in updatedRelationships)
                            {
                                if (updatedRelationship.Action != ObjectAction.Delete && updatedRelationship.RelationshipTypeId == relationshipTypeId &&
                                    updatedRelationship.ToEntityTypeId == relationshipCardinality.ToEntityTypeId && updatedRelationship.Level == 1)
                                {
                                    cardinalitySettingsMatchingRelationships.Add(updatedRelationship);
                                    relationshipTypeName = updatedRelationship.RelationshipTypeName;
                                }
                            }

                            if (originalEntityRelationships != null && originalEntityRelationships.Count > 0)
                            {
                                //NOTE: Count already existing relationships (resides in original Entity) for min max validations 
                                foreach (Relationship originalRelationship in originalEntityRelationships)
                                {
                                    if (originalRelationship.RelationshipTypeId == relationshipTypeId && originalRelationship.ToEntityTypeId == relationshipCardinality.ToEntityTypeId &&
                                        originalRelationship.Level == 1 && !cardinalitySettingsMatchingRelationships.Contains(originalRelationship.RelatedEntityId))
                                    {
                                        if (updatedRelationships.Where(updatedRel => updatedRel.Id == originalRelationship.Id &&
                                                                       updatedRel.Action == ObjectAction.Delete).Count() == 0)
                                        {
                                            cardinalitySettingsMatchingRelationships.Add(originalRelationship);
                                            relationshipTypeName = originalRelationship.RelationshipTypeName;
                                        }
                                    }
                                }
                            }

                            if (cardinalitySettingsMatchingRelationships != null && cardinalitySettingsMatchingRelationships.Count > relationshipCardinality.MaxRelationships)
                            {
                                //Maximum relationship(s) allowed for Entity Type '{0}' and Relationship Type '{1}' is exceeding defined cardinality: '{2}'
                                String message = String.Empty;
                                message = String.Format("Maximum relationship(s) allowed for Entity type {0} and Relationship type {1} exceeds the defined cardinality: {2}", relationshipCardinality.ToEntityTypeLongName, relationshipTypeName, relationshipCardinality.MaxRelationships);
                                _parameters = new Object[] { relationshipCardinality.ToEntityTypeLongName, relationshipTypeName, relationshipCardinality.MaxRelationships };
                                relationshipOperationResultCollection.AddOperationResult("112893", _parameters.ToCollection(), message, ReasonType.CardinalityCheck, -1, -1, operationResultType);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="currentRelationship"></param>
        /// <param name="entity"></param>
        /// <param name="relationshipOperationResult"></param>
        /// <param name="operationResultType"></param>
        private void ValidateDuplicateRelationships(Relationship currentRelationship, Entity entity, RelationshipOperationResult relationshipOperationResult, OperationResultType operationResultType)
        {
            if (entity.OriginalEntity != null && entity.OriginalEntity.Relationships != null)
            {
                //Validate within existing relationships..
                RelationshipCollection originalRelationships = entity.OriginalEntity.Relationships;

                foreach (Relationship originalRelationship in originalRelationships)
                {
                    if (originalRelationship.FromEntityId == currentRelationship.FromEntityId && originalRelationship.RelatedEntityId == currentRelationship.RelatedEntityId && originalRelationship.ContainerId == currentRelationship.ContainerId &&
                            originalRelationship.RelationshipTypeId == currentRelationship.RelationshipTypeId && originalRelationship.Level == currentRelationship.Level)
                    {
                        //111627 : {0} is already in Relationship
                        relationshipOperationResult.AddOperationResult("111627", "{0} is already in Relationship", new Collection<Object>() { originalRelationship.ToExternalId }, ReasonType.RelationshipCheck, -1, -1, operationResultType);
                        return;
                    }
                }
            }

            if (entity.Relationships != null)
            {
                //Validate within relationships which are being created newly..
                foreach (Relationship updatedRelationship in entity.Relationships)
                {
                    if (updatedRelationship.Id != currentRelationship.Id && updatedRelationship.Action != ObjectAction.Ignore)
                    {
                        if (updatedRelationship.FromEntityId == currentRelationship.FromEntityId && updatedRelationship.RelatedEntityId == currentRelationship.RelatedEntityId && updatedRelationship.ContainerId == currentRelationship.ContainerId &&
                            updatedRelationship.RelationshipTypeId == currentRelationship.RelationshipTypeId && updatedRelationship.Level == currentRelationship.Level)
                        {
                            currentRelationship.Action = ObjectAction.Ignore;
                            return;
                        }
                    }
                }
            }
        }

        #endregion

        #region Miscellaneous

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
        /// <param name="type"></param>
        /// <returns></returns>
        private String GetImageExtensionRegexPart(String type)
        {
            String regex = String.Empty;

            try
            {
                //Get the AppConfig, if does not exist then exit and return blank
                String validationRules = AppConfigurationHelper.GetAppConfig<String>("MDMCenter.FileUploadValidation");

                XmlDocument xmlDoc = new XmlDocument();
                //load validation xml
                xmlDoc.LoadXml(validationRules);

                XmlNodeList validationNodes;
                //if type of file is image, then only image extensions, otherwise all
                if (type.ToLower().Equals("image"))
                {
                    validationNodes = xmlDoc.SelectNodes("//FileValidation/Rule[@Name='image']");
                }
                else
                {
                    validationNodes = xmlDoc.SelectNodes("//FileValidation/Rule");
                }

                ArrayList arrExtensions = new ArrayList();

                //loop through nodes and check extensions
                foreach (XmlNode node in validationNodes)
                {
                    //check if there is a Attribute names Extensions
                    if (node.Attributes["Extensions"] != null)
                    {
                        String extensions = node.Attributes["Extensions"].Value;

                        //its comma separated list, so split them
                        String[] values = extensions.Split(',');

                        //add in the array list
                        arrExtensions.AddRange(values);
                    }
                }

                //check if we got some extensions to prepare the regex
                //if we have *, no need of validation
                if (arrExtensions.Count > 0 && arrExtensions.IndexOf("*") == -1)
                {
                    //now we'll prepare the regex string, so go through each extension
                    foreach (String ext in arrExtensions)
                    {
                        if (!String.IsNullOrEmpty(ext.Trim()))
                        {
                            foreach (Char c in ext.ToCharArray())
                            {
                                if (c.ToString().Trim().Length > 0)
                                {
                                    //take lower and upper both case letters, as case doesn't matter for file extensions
                                    //eg. JPG, jpg, Jpg, jPg
                                    regex = String.Format("{0}({1}|{2})", regex, c.ToString().ToLower(), c.ToString().ToUpper());
                                }
                            }

                            regex = String.Concat(regex, "|");
                        }
                    }

                    //remove the last pipe
                    regex = regex.TrimEnd('|');
                    regex = String.Format(@"\.({0})", regex);
                }
            }
            catch
            {
                throw;
            }

            return regex;
        }

        /// <summary>
        /// Get locale specific message from code
        /// </summary>
        /// <param name="messageCode"></param>
        /// <param name="locale"></param>
        /// <param name="defaultMessage"></param>
        /// <param name="callerContext"></param>
        /// <param name="messageArguments"></param>
        /// <returns></returns>
        private String GetFormattedLocaleMessage(String messageCode, LocaleEnum locale, String defaultMessage, CallerContext callerContext, params Object[] messageArguments)
        {
            String msg = defaultMessage;
            LocaleMessageBL messageBL = new LocaleMessageBL();
            LocaleMessage message = messageBL.Get(locale, messageCode, false, callerContext);
            if (message != null)
            {
                if (messageArguments != null)
                {
                    msg = String.Format(message.Message, messageArguments);
                }
                else
                {
                    msg = message.Message;
                }
            }

            return msg;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="attribute"></param>
        /// <param name="attributeOperationResult"></param>
        /// <param name="messageCode"></param>
        /// <param name="parameters"></param>
        /// <param name="operationResultType"></param>
        /// <param name="callerContext"></param>
        private void UpdateAttributeOperationResult(Attribute attribute, AttributeOperationResult attributeOperationResult, String messageCode, Object[] parameters, OperationResultType operationResultType, CallerContext callerContext)
        {
            UpdateAttributeOperationResult(attribute, attributeOperationResult, messageCode, parameters, ReasonType.NotSpecified, -1, -1, operationResultType, callerContext);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="attribute"></param>
        /// <param name="attributeOperationResult"></param>
        /// <param name="messageCode"></param>
        /// <param name="parameters"></param>
        /// <param name="reasonType"></param>
        /// <param name="ruleMapContextId"></param>
        /// <param name="ruleId"></param>
        /// <param name="operationResultType"></param>
        /// <param name="callerContext"></param>
        private void UpdateAttributeOperationResult(Attribute attribute, AttributeOperationResult attributeOperationResult, String messageCode, Object[] parameters, ReasonType reasonType, Int32 ruleMapContextId, Int32 ruleId, OperationResultType operationResultType, CallerContext callerContext)
        {
            if (callerContext == null)
            {
                callerContext = new CallerContext(MDMCenterApplication.PIM, MDMCenterModules.Entity);
            }

            String defaultMessage = String.Format("Attribute model for '{0}/{1}' is not configured with a list of allowable values.", attribute.LongName, attribute.AttributeParentLongName);
            String errorMessage = _localeMessageBL.TryGet(_systemUILocale, messageCode, defaultMessage, parameters, false, callerContext).Message;

            attributeOperationResult.AddOperationResult(messageCode, errorMessage, parameters.ToCollection(), reasonType, ruleMapContextId, ruleId, operationResultType);

            MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, defaultMessage);

            attribute.HasInvalidOverriddenValues = true;

            //Set attribute action as update only if caller context is set to revalidate.
            SetAttributeAction(attribute, callerContext, ObjectAction.Update);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objectAction"></param>
        /// <param name="callerContext"></param>
        /// <returns></returns>
        private Boolean IsQualifiedForValidation(ObjectAction objectAction, CallerContext callerContext)
        {
            Boolean isQualified = false;

            if (callerContext != null && callerContext.Module == MDMCenterModules.Revalidate)
            {
                isQualified = true;
            }
            else
            {
                isQualified = (objectAction != ObjectAction.Ignore && objectAction != ObjectAction.Delete && objectAction != ObjectAction.Read);
            }

            return isQualified;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="attribute"></param>
        /// <param name="callerContext"></param>
        /// <param name="objectAction"></param>
        private void SetAttributeAction(Attribute attribute, CallerContext callerContext, ObjectAction objectAction)
        {
            if (attribute != null && callerContext != null && callerContext.Module == MDMCenterModules.Revalidate)
            {
                attribute.Action = objectAction;
            }
        }

        #endregion

        #region Dependent Attribute Helper Methods

        private void ValidateAttributeDependency(Entity entity, CallerContext callerContext, EntityOperationResult entityOperationResult, Boolean isCollectionAttributeDependencyEnabled = false, Boolean allowInvalidData = false)
        {
            #region  Logical Flow

            /* This block of code will do the following
             * 1.Filter out the dependent attribute id list from the current entity.
             * 
             * 2.From the listed dependent attribute id list, Prepare the dependent child attribute list which 
             * need to be clear if the attribute value is not correct [based on the link mapping table].
             * 
             * 3.Prepare all dependent attribute id bucket list which contains the list of child dependent attributes
             * and parent dependent attribute. And populate the attribute accordingly. 
             * - If the parent attribute is not present in the current entity get it from the original entity.
             * 
             * 4. Validate all child attribute which having the proper mapping value if is not then clear the value and
             * update into entity. If the attribute is not present in the current attribute then add into entity.
             * 
             * 5. If any of the child attribute is cleared then add warning message into entity.
             * 
             * Preparing seperate buckets for dependent childen and dependency attributes usage is one time validation happend accross the entity.
             * May be the current enity has multiple parent having same dependent child attributes or multiple child dependent attribute having same dependent parent attribute.
             * 
             */

            #endregion

            if (entity != null && entity.AttributeModels != null)
            {
                var dependencyAttributeModels = this.GetDependentAttributes(entity);

                if (dependencyAttributeModels != null && dependencyAttributeModels.Count() > 0)
                {
                    List<Int32> dependencyBucket = new List<Int32>();   //It hold the parent and child attribute dependency list.
                    List<Int32> childDependencyBucket = new List<Int32>();  //This hold only the child attribute id list. which are unique at the end and we are going to validate this list only

                    foreach (AttributeModel attrmodel in dependencyAttributeModels) //Is locale is required? for lookup not required. [when start supporting dependency for other than lookup it is required. 
                    {
                        if (attrmodel.IsCollection && !isCollectionAttributeDependencyEnabled)
                        {
                            //if attribute model is collection and collection dependency is disabled then do not validate..
                            continue;
                        }

                        if (attrmodel.HasDependentAttribute)
                        {
                            childDependencyBucket.AddRange(attrmodel.DependentChildAttributes.GetDepencyAttributeIdList());
                        }

                        if (attrmodel.IsDependentAttribute)
                        {
                            dependencyBucket.AddRange(attrmodel.DependentParentAttributes.GetDepencyAttributeIdList());
                            childDependencyBucket.Add(attrmodel.Id);

                        }

                        //Current attribute is not part of all attribute bucket so add this as well.
                        dependencyBucket.Add(attrmodel.Id);
                    }

                    //distinct child attribute id list. This list only we are going to validate at the end.
                    childDependencyBucket = childDependencyBucket.Distinct().ToList<Int32>();

                    //All child attribute list into dependency bucket list.
                    dependencyBucket.AddRange(childDependencyBucket);

                    //After adding children, the same child may present in parent attribute collection. So filter it again.
                    dependencyBucket = dependencyBucket.Distinct().ToList<Int32>();

                    DependentAttributeCollection dependencyAttributes = PopulateDependentAttributeValues(entity, dependencyBucket);

                    ValidateAndUpdateDependentAttributes(entity, dependencyAttributes, childDependencyBucket, entityOperationResult, callerContext, isCollectionAttributeDependencyEnabled, allowInvalidData);
                }

            }
        }

        private void ValidateAndUpdateDependentAttributes(Entity entity, DependentAttributeCollection dependentAttributes, List<Int32> childDependentAttributelist, EntityOperationResult entityOperationResult, CallerContext callerContext, Boolean isCollectionAttributeDependencyEnabled = false, Boolean allowInvalidData = false)
        {
            #region Logical Flow

            /* For each child attribute check whether the attribute having proper value or not.
             * This will be validated based on the dependency link mapping.
             * 
             * If the child attribute does not have the proper value then clear the existing value. 
             * ==>If the attribute present in entity then update the attribute 
             * ==>Else clear the value for the attribute and add the attribute into entity.
             * ==> Let say if the attribute is collection then validate each values. if the value is not valid then remove from it.
             */

            #endregion

            if (entity != null && childDependentAttributelist != null && childDependentAttributelist.Count > 0)
            {
                AttributeCollection updateAttributeBucket = new AttributeCollection();
                AttributeDependencyBL attributeDependencyBL = new AttributeDependencyBL();
                SecurityPrincipal securityPrincipal = SecurityPrincipalHelper.GetCurrentSecurityPrincipal();
                LocaleMessageBL localeMessageBL = new LocaleMessageBL();
                //Error message will be same but used inside the loop so loaded once.
                String attrErrorMsgCode = "112250";
                String colErrorMsgCode = "112392";
                ReasonType reasonType = ReasonType.NotSpecified;
                String attrErrorMsg = localeMessageBL.Get(_systemUILocale, attrErrorMsgCode, false, callerContext).Message; //'{0}' is cleared due to modified dependent parent attribute ({1}) values.
                String colErrorMsg = localeMessageBL.Get(_systemUILocale, colErrorMsgCode, false, callerContext).Message; //'{0}' values are partially cleared due to modified dependent parent attribute ({1}) values.
                String errorMessage = String.Empty;
                String errorMessageCode = String.Empty;
                Collection<Int32> missedAttributeIds = new Collection<Int32>();
                Boolean validateDependencyInDB = AppConfigurationHelper.GetAppConfig<Boolean>("MDMCenter.AttributeModelManager.AttributeDependency.ValidateValuesInDB", false);

                //Todo::How to populate user Role id
                ApplicationContext applicationContext = new ApplicationContext(entity.OrganizationId, entity.ContainerId, entity.EntityTypeId, -1, entity.CategoryId, entity.Id, -1, String.Empty, securityPrincipal.CurrentUserId, -1);

                foreach (Int32 dependentAttrId in childDependentAttributelist)
                {
                    Boolean isAttributeInEntity = true;
                    Boolean clearAttributeValue = true;

                    if (dependentAttrId > 0)
                    {
                        #region Get the Attribute for the dependent child

                        IAttribute attr = entity.GetAttribute(dependentAttrId); //Get based on locale

                        if (attr == null && entity.GetOriginalEntity() != null)
                        {
                            attr = (IAttribute)(entity.GetOriginalEntity().GetAttribute(dependentAttrId));
                            isAttributeInEntity = false;
                        }

                        #endregion

                        #region Validate and update the Dependent Attribute

                        if (attr != null)
                        {
                            if (attr.IsCollection && !isCollectionAttributeDependencyEnabled)
                            {
                                continue;
                            }

                            IAttributeModel attrModel = null;

                            #region Get the Attribute Model

                            if ((entity.Action == ObjectAction.Create || entity.Action == ObjectAction.Delete) && entity.GetAttributeModels() != null)
                            {
                                attrModel = entity.GetAttributeModels().GetAttributeModel(dependentAttrId, attr.Locale);
                            }
                            else if (entity.GetOriginalEntity() != null && entity.GetOriginalEntity().GetAttributeModels() != null)
                            {
                                attrModel = entity.GetOriginalEntity().GetAttributeModels().GetAttributeModel(dependentAttrId, attr.Locale);
                            }

                            #endregion

                            if (attrModel != null)
                            {
                                DependentAttributeCollection parentDependentAttrs = (DependentAttributeCollection)attrModel.DependentParentAttributes.Clone(false);

                                if (parentDependentAttrs != null && parentDependentAttrs.Count > 0)
                                {
                                    foreach (DependentAttribute item in parentDependentAttrs)
                                    {
                                        item.SetAttributeValue(dependentAttributes.GetDependentAttribute(item.AttributeId).AttributeValue);
                                    }
                                }

                                #region Validate current attribute value based on dependency Mapping.

                                if (!validateDependencyInDB)
                                {
                                    #region Validate values in API. Get all allowed values for dependent child from DB and let API make decisioin what values are valid.

                                    //Validate current attribute value
                                    Collection<String> dirtyValues = new Collection<String>();
                                    applicationContext.AttributeId = attr.Id;
                                    applicationContext.Locale = attr.Locale.ToString();

                                    Collection<String> resultedMappingIds = attributeDependencyBL.GetDependencyMappings(attr.Id, applicationContext, parentDependentAttrs, callerContext);

                                    if (resultedMappingIds != null && resultedMappingIds.Count > 0)
                                    {
                                        DependentAttribute tempAttr = new DependentAttribute();

                                        tempAttr.SetAttributeValue(attr);

                                        #region Validate child/children attribute's overridden values

                                        if (!String.IsNullOrWhiteSpace(tempAttr.AttributeValue))
                                        {
                                            if (attr.IsCollection)
                                            {
                                                //if it is collection split the attribute values,
                                                String[] collectionValues = tempAttr.AttributeValue.Split(new String[] { "$@$" }, StringSplitOptions.None);

                                                foreach (String val in collectionValues)
                                                {
                                                    if (!String.IsNullOrWhiteSpace(val))
                                                    {
                                                        var items = resultedMappingIds.Where(a => a.Equals(val));

                                                        if (items == null || items.Count() == 0)
                                                        {
                                                            dirtyValues.Add(val);
                                                        }
                                                    }
                                                }

                                                if (dirtyValues.Count == 0 && collectionValues.Count() > 0)
                                                {
                                                    //all child values are proper.
                                                    clearAttributeValue = false;
                                                }
                                            }
                                            else
                                            {
                                                var value = resultedMappingIds.Where(a => a.Equals(tempAttr.AttributeValue));

                                                if (value != null && value.Count() > 0)
                                                {
                                                    //So the current attribute value is valid for this attribute based on the dependency mapping.
                                                    //Continue...
                                                    clearAttributeValue = false;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            //If my child attribute value is empty then no need to clear
                                            clearAttributeValue = false;
                                        }

                                        #endregion
                                    }

                                    #region Delete Attribute value

                                    if (clearAttributeValue && attr.HasValue())
                                    {
                                        // clear the current attribute value.
                                        IValueCollection valuecollection = attr.GetOverriddenValues();

                                        if (valuecollection != null && valuecollection.Count > 0)
                                        {
                                            #region Update Attribute Value Action

                                            //If an attribute is collection and the dirty values are found then remove only dirty values. 
                                            //If an attribute is non - collection then remove all values. Since it is not collection will available only one value.
                                            foreach (IValue val in valuecollection)
                                            {
                                                if (attr.IsCollection && dirtyValues.Count > 0)
                                                {
                                                    var filter = dirtyValues.Where(d => d.Equals(val.AttrVal));

                                                    if (filter != null && filter.Count() > 0)
                                                    {
                                                        if (allowInvalidData == false)
                                                        {
                                                            val.Action = ObjectAction.Delete;
                                                        }
                                                        else
                                                        {
                                                            //val.Action = ObjectAction.Update;
                                                            val.HasInvalidValue = true;
                                                            val.Action = ObjectAction.Update;
                                                        }
                                                    }
                                                    //else will we a valid value so no need to change the attribute action flag.
                                                }
                                                else
                                                {
                                                    //val.Action = ObjectAction.Delete;
                                                    if (allowInvalidData == false)
                                                    {
                                                        val.Action = ObjectAction.Delete;
                                                    }
                                                    else
                                                    {
                                                        //val.Action = ObjectAction.Update;
                                                        val.HasInvalidValue = true;
                                                        val.Action = ObjectAction.Update;
                                                    }
                                                }
                                            }

                                            #endregion

                                            #region Update Attribute Source Flag and Action

                                            if (allowInvalidData == false)
                                            {
                                                if (attr.IsCollection && dirtyValues.Count > 0 && attr.HasValue(false))
                                                {
                                                    attr.Action = ObjectAction.Update;
                                                    errorMessage = colErrorMsg;
                                                    errorMessageCode = colErrorMsgCode;
                                                    reasonType = ReasonType.Unknown;
                                                }
                                                else
                                                {
                                                    if (attr.Action != ObjectAction.Ignore)
                                                    {
                                                        attr.Action = ObjectAction.Delete;
                                                        errorMessage = attrErrorMsg;
                                                        errorMessageCode = attrErrorMsgCode;
                                                        reasonType = ReasonType.Unknown;
                                                    }
                                                }
                                            }

                                            attr.SourceFlag = AttributeValueSource.Overridden;

                                            #endregion

                                            if (allowInvalidData == false)
                                            {
                                                UpdateDependencyAttributeErrorMessage(attrModel, parentDependentAttrs, entityOperationResult, errorMessage, errorMessageCode, reasonType);
                                            }
                                        }
                                    }

                                    #endregion

                                    #endregion Validate values in API. Get all allowed values for dependent child from DB and let API make decisioin what values are valid.
                                }
                                else
                                {
                                    #region Validate values in DB. Pass parent and child attribute values to DB and get boolean value back.

                                    //Validate current attribute value
                                    applicationContext.AttributeId = attr.Id;
                                    applicationContext.Locale = attr.Locale.ToString();

                                    IOperationResult dependencyValidationResult = attributeDependencyBL.AreDependentValuesValid(attr, attrModel, applicationContext, parentDependentAttrs, callerContext);

                                    if (dependencyValidationResult.OperationResultStatus != OperationResultStatusEnum.None && dependencyValidationResult.OperationResultStatus != OperationResultStatusEnum.Successful)
                                    {
                                        entityOperationResult.CopyErrorInfoAndWarning(dependencyValidationResult);
                                    }

                                    #endregion Validate values in DB. Pass parent and child attribute values to DB and get boolean value back.
                                }

                                #endregion Validate current attribute value based on dependency Mapping.

                                //if the current attribute is not present in entity add that into entity object.
                                if (!isAttributeInEntity)
                                {
                                    updateAttributeBucket.Add(attr);
                                }
                            }
                        }
                        else
                        {
                            missedAttributeIds.Add(dependentAttrId);
                        }

                        #endregion
                    }
                }

                #region Try to handled missed attribute. These might be complex child

                if (missedAttributeIds.Count > 0)
                {
                    //user Entity or OriginalEnity?
                    Boolean isAttributeInEntity = true;
                    Collection<Int32> copyMissedAttrIds = new Collection<Int32>(missedAttributeIds.ToList<Int32>());

                    AttributeCollection possibleComplexParentAttributes = new AttributeCollection();

                    //if(possibleComplexParentAttributes

                    possibleComplexParentAttributes = this.GetComplexParentForComplexChildDependentAttribute(copyMissedAttrIds, entity);

                    if (copyMissedAttrIds.Count > 0)
                    {
                        Entity originalEntity = entity.OriginalEntity;
                        if (originalEntity != null)
                        {
                            AttributeCollection possibleComplexParentAttributesFromOriginalEntity = this.GetComplexParentForComplexChildDependentAttribute(copyMissedAttrIds, originalEntity);
                            if (possibleComplexParentAttributesFromOriginalEntity != null && possibleComplexParentAttributesFromOriginalEntity.Count > 0)
                            {
                                possibleComplexParentAttributes.AddRange(possibleComplexParentAttributesFromOriginalEntity);
                                isAttributeInEntity = false;
                            }
                        }
                    }
                    Collection<Int32> updatedChildAttrIdList = new Collection<int>();

                    foreach (Attribute cxParent in possibleComplexParentAttributes)
                    {
                        if (cxParent.IsCollection && !isCollectionAttributeDependencyEnabled)
                        {
                            continue;
                        }

                        if (cxParent.IsCollection == true)
                        {
                            errorMessage = colErrorMsg;
                            errorMessageCode = colErrorMsgCode;
                            reasonType = ReasonType.Unknown;
                        }
                        else
                        {
                            errorMessage = attrErrorMsg;
                            errorMessageCode = attrErrorMsgCode;
                            reasonType = ReasonType.Unknown;
                        }

                        IAttributeModel attrModel = null;

                        #region Get the Attribute Model

                        if ((entity.Action == ObjectAction.Create || entity.Action == ObjectAction.Delete) && entity.GetAttributeModels() != null)
                        {
                            attrModel = entity.GetAttributeModels().GetAttributeModel(cxParent.Id, cxParent.Locale);
                        }
                        else if (entity.GetOriginalEntity() != null && entity.GetOriginalEntity().GetAttributeModels() != null)
                        {
                            attrModel = entity.GetOriginalEntity().GetAttributeModels().GetAttributeModel(cxParent.Id, cxParent.Locale);
                        }

                        #endregion

                        if (attrModel != null)
                        {

                            foreach (Attribute cxInstance in cxParent.Attributes)
                            {
                                AttributeCollection childAttributes = new AttributeCollection();
                                Int32 remainingCountOfUpdatedChildAttr = attrModel.GetChildAttributeModels().Count();

                                foreach (AttributeModel childAttrModel in attrModel.GetChildAttributeModels())
                                {
                                    Attribute cxChild = (Attribute)cxInstance.Attributes.GetAttribute(childAttrModel.Id);

                                    if (cxChild == null)
                                    {
                                        remainingCountOfUpdatedChildAttr--;
                                    }
                                    else if (childAttrModel.IsDependentAttribute == true || childAttrModel.HasDependentAttribute == true)
                                    {
                                        if (missedAttributeIds.Contains(childAttrModel.Id))
                                        {
                                            Boolean clearAttributeValue = true;

                                            #region Try to populate DependentAttr value

                                            DependentAttributeCollection parentDependentAttrs = (DependentAttributeCollection)childAttrModel.DependentParentAttributes.Clone(false);

                                            Collection<Int32> missedParentAttrValIds = new Collection<int>();

                                            if (parentDependentAttrs != null && parentDependentAttrs.Count > 0)
                                            {
                                                foreach (DependentAttribute item in parentDependentAttrs)
                                                {
                                                    IDependentAttribute parentVal = dependentAttributes.GetDependentAttribute(item.AttributeId);
                                                    if (parentVal != null && parentVal.AttributeId > 0)
                                                    {
                                                        if (parentVal.Action == ObjectAction.Delete)
                                                        {
                                                            item.SetAttributeValue(String.Empty);
                                                        }
                                                        else
                                                        {
                                                            item.SetAttributeValue(parentVal.AttributeValue);
                                                        }
                                                    }
                                                    else
                                                    {
                                                        missedParentAttrValIds.Add(item.AttributeId);
                                                    }
                                                }
                                            }

                                            #endregion Try to populate DependentAttr value

                                            #region Update missed parent attribute values

                                            foreach (Int32 missedParentId in missedParentAttrValIds)
                                            {
                                                IAttribute parentOfCxChild = cxInstance.Attributes.GetAttribute(missedParentId);

                                                if (parentOfCxChild != null)
                                                {
                                                    parentDependentAttrs.GetDependentAttribute(missedParentId).SetAttributeValue(parentOfCxChild);
                                                }
                                            }

                                            #endregion Update missed parent attribute values

                                            #region Get mapping data

                                            //Validate current attribute value
                                            Collection<String> dirtyValues = new Collection<String>();
                                            applicationContext.AttributeId = childAttrModel.Id;
                                            applicationContext.Locale = childAttrModel.Locale.ToString();

                                            Collection<String> resultedMappingIds = attributeDependencyBL.GetDependencyMappings(childAttrModel.Id, applicationContext, parentDependentAttrs, callerContext);

                                            #endregion Get mapping data

                                            #region Validate child/children attribute's overridden values

                                            if (resultedMappingIds != null && resultedMappingIds.Count > 0)
                                            {
                                                DependentAttribute tempAttr = new DependentAttribute();

                                                tempAttr.SetAttributeValue(cxChild);

                                                if (!String.IsNullOrWhiteSpace(tempAttr.AttributeValue))
                                                {
                                                    var value = resultedMappingIds.Where(a => a.Equals(tempAttr.AttributeValue));

                                                    if (value != null && value.Count() > 0)
                                                    {
                                                        //So the current attribute value is valid for this attribute based on the dependency mapping.
                                                        //Continue...
                                                        clearAttributeValue = false;
                                                    }
                                                }
                                                else
                                                {
                                                    //If my child attribute value is empty then no need to clear
                                                    clearAttributeValue = false;
                                                }

                                                if (clearAttributeValue == false)
                                                {
                                                    cxChild.Action = ObjectAction.Update;
                                                    if (cxChild.HasValue() == false)
                                                    {
                                                        remainingCountOfUpdatedChildAttr--;
                                                    }
                                                }
                                            }

                                            #endregion Validate child/children attribute's overridden values

                                            #region Delete Attribute value

                                            if (clearAttributeValue)
                                            {
                                                remainingCountOfUpdatedChildAttr--;
                                                // clear the current attribute value.
                                                IValueCollection valueCollection = cxChild.GetOverriddenValues();

                                                if (valueCollection != null && valueCollection.Count > 0)
                                                {
                                                    Boolean isAnyValueCleared = false;
                                                    foreach (Value val in valueCollection)
                                                    {
                                                        if (val.AttrVal != null && val.AttrVal.ToString().Length > 0)
                                                        {
                                                            val.Clear();
                                                            isAnyValueCleared = true;
                                                        }
                                                    }

                                                    if (isAnyValueCleared)
                                                    {
                                                        #region Update Attribute Source Flag and Action

                                                        cxChild.Action = ObjectAction.Update;
                                                        cxInstance.Action = ObjectAction.Update;
                                                        cxParent.Action = ObjectAction.Update;
                                                        cxChild.SourceFlag = AttributeValueSource.Overridden;
                                                        cxParent.SourceFlag = AttributeValueSource.Overridden;

                                                        #endregion

                                                        if (!updatedChildAttrIdList.Contains(childAttrModel.Id))
                                                        {
                                                            UpdateDependencyAttributeErrorMessage(childAttrModel, parentDependentAttrs, entityOperationResult, errorMessage, errorMessageCode, reasonType);
                                                            updatedChildAttrIdList.Add(childAttrModel.Id);
                                                        }
                                                    }
                                                }
                                            }

                                            #endregion Delete Attribute value
                                        }
                                        else
                                        {
                                            // This is complex attribute sibling that has dependency as well but not part of the missing attribute list
                                            // Need to update action always as complex processing is Flush and fill, so all the attributes has to go to db
                                            cxChild.Action = ObjectAction.Update;

                                            if (cxChild.HasValue() == false)
                                            {
                                                remainingCountOfUpdatedChildAttr--;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        //Need to update action always as complex processing is Flush and fill, so all the attributes has to go to db
                                        cxChild.Action = ObjectAction.Update;

                                        if (cxChild.HasValue() == false)
                                        {
                                            remainingCountOfUpdatedChildAttr--;
                                        }
                                    }

                                    //If some values are cleared and there is no value in child attribute, just remove the whole complex instance record
                                    if (remainingCountOfUpdatedChildAttr == 0 && cxInstance.InstanceRefId > 0)
                                    {
                                        cxParent.RemoveComplexChildRecordByInstanceRefId(cxInstance.InstanceRefId);
                                    }
                                }

                                //For all the updates in complex record, need to clear the value.Id. As complex process is not able to take care of delta updates.
                                if (cxInstance.InstanceRefId > 0)
                                {
                                    IValue val = cxParent.CurrentValues.GetByValueRefId(cxInstance.InstanceRefId);
                                    val.Id = -1;
                                    val.Action = ObjectAction.Update;
                                }
                            }

                            //Add attribute is update bucket only if it is not found in current entity and there is any change in Complex.
                            if (!isAttributeInEntity && cxParent.Action != ObjectAction.Read)
                            {
                                updateAttributeBucket.Add(cxParent);
                            }
                        }
                    }

                }

                #endregion Try to handled missed attribute. These might be complex child

                #region Update Entity Attribute

                if (updateAttributeBucket.Count > 0)
                {
                    //Can't add in one shot because attribute.Becaue 'AddRage' property does the following, if the attribute is already present in the collection it will not add. 

                    foreach (Attribute attr in updateAttributeBucket)
                    {
                        if (attr != null) //if it is null adding attribute will throw error. So if it is null do not add.
                        {
                            entity.Attributes.Remove(attr);
                            entity.Attributes.Add(attr);
                        }

                    }
                }

                #endregion
            }
        }

        private DependentAttributeCollection PopulateDependentAttributeValues(Entity entity, List<Int32> dependencyAttributeIds)
        {
            #region Logical Flow

            /* This method will populate dependent attribute values for the requested dependency attribute id list
             * 
             * If the requested dependent attribute is present in current entity then read the value from there.
             * Else get it from the original entity.
             * 
             */

            #endregion

            DependentAttributeCollection dependentAttributes = new DependentAttributeCollection();

            foreach (Int32 id in dependencyAttributeIds)
            {
                IAttribute attribute = entity.GetAttribute(id);  //get based on the locale value

                if (attribute == null && entity.GetOriginalEntity() != null)
                {
                    attribute = entity.GetOriginalEntity().GetAttribute(id);
                }

                if (attribute != null)
                {
                    DependentAttribute dependentAttr = new DependentAttribute();

                    dependentAttr.SetAttributeValue(attribute);
                    dependentAttr.AttributeId = id;
                    dependentAttr.Locale = dependentAttr.Locale;    //Future usage....
                    dependentAttr.Name = attribute.Name;
                    dependentAttr.LongName = attribute.LongName;
                    dependentAttr.Action = attribute.Action;

                    dependentAttributes.Add(dependentAttr);
                }
            }

            return dependentAttributes;
        }

        private void UpdateDependencyAttributeErrorMessage(IAttributeModel model, DependentAttributeCollection parentDependentAttrs, EntityOperationResult eor, String errorMessage, String errorMessageCode, ReasonType reasonType)
        {
            if (!String.IsNullOrWhiteSpace(errorMessage))
            {
                //This will be the additional over head but for parent attribute name is required in the warning message so...
                if (model != null && model.IsDependentAttribute)
                {
                    StringBuilder sb = new StringBuilder();
                    Boolean addSeperator = false;

                    foreach (DependentAttribute dAttr in model.DependentParentAttributes)
                    {
                        IDependentAttribute dependentAttribute = parentDependentAttrs.GetDependentAttribute(dAttr.AttributeId);

                        if (!String.IsNullOrWhiteSpace(dependentAttribute.AttributeValue))
                        {
                            if (addSeperator)
                            {
                                sb.Append(",");
                            }

                            //sb.Append(dependentAttribute.AttributeName + "[ " + dependentAttribute.ParentAttributeName + " ]");
                            sb.Append(dependentAttribute.AttributeName);

                            addSeperator = true;
                        }
                    }

                    eor.AddOperationResult(errorMessageCode, String.Format(errorMessage, model.LongName, sb.ToString()), reasonType, -1, -1, OperationResultType.Warning);   ////'{0}' is cleared due to modified dependent parent attribute ({1}) values.
                    if (Constants.TRACING_ENABLED)
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Warning, String.Format(errorMessage, model.LongName, sb.ToString()));
                }
            }
        }

        private AttributeModelCollection GetDependentAttributes(Entity entity)
        {
            AttributeModelCollection dependencyAttributeModels = new AttributeModelCollection();

            if (entity.AttributeModels != null)
            {
                foreach (AttributeModel attrModel in entity.AttributeModels)
                {
                    // Check the action of the attribute
                    IAttribute attribute = entity.GetAttribute(attrModel.Id, attrModel.Locale);

                    // If the attribute is there and has not changed do not need to process the dependency for it. If the child or parent is change that will take care
                    if (attribute != null && (attribute.Action == ObjectAction.Read || attribute.Action == ObjectAction.Ignore))
                    {
                        continue;
                    }

                    if (attrModel.IsDependentAttribute || attrModel.HasDependentAttribute)
                    {
                        dependencyAttributeModels.Add(attrModel);
                    }

                    if (attrModel.IsComplex)
                    {
                        foreach (AttributeModel child in attrModel.GetChildAttributeModels())
                        {
                            if (child.IsDependentAttribute || child.HasDependentAttribute)
                            {
                                // As needs to validate complex child attributes also needs to add it into dependencyAttributeModels
                                dependencyAttributeModels.Add(child);
                            }
                        }
                    }
                }
            }

            return dependencyAttributeModels;
        }

        private AttributeCollection GetComplexParentForComplexChildDependentAttribute(Collection<Int32> missingComplexChildAttributeIds, Entity entity)
        {
            AttributeCollection complexParentAttributes = new AttributeCollection();
            Int32 totalMissingAttrCount = missingComplexChildAttributeIds.Count;

            foreach (Attribute attr in entity.Attributes)
            {
                if (attr.IsComplex == true)
                {
                    foreach (Attribute complexChildAttribute in attr.GetComplexChildAttributes())
                    {
                        if (missingComplexChildAttributeIds.Contains(complexChildAttribute.Id))
                        {
                            missingComplexChildAttributeIds.Remove(complexChildAttribute.Id);
                            complexParentAttributes.Add(attr);
                            totalMissingAttrCount--;
                            //break;
                        }
                    }
                }
                if (totalMissingAttrCount < 1)
                    break;
            }

            return complexParentAttributes;
        }

        #endregion

        #endregion

        #endregion
    }
}