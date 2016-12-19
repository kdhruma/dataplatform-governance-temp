using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Collections.ObjectModel;
using System.Text;

namespace MDM.DataModelManager.Business
{
    using MDM.BusinessObjects;
    using MDM.BusinessObjects.DataModel;
    using MDM.Core;
    using MDM.Interfaces;
    using MDM.MessageManager.Business;
    using MDM.Utility;

    /// <summary>
    /// Specifies the helper class for data model.
    /// </summary>
    internal class DataModelHelper
    {
        #region Fields

        /// <summary>
        /// Field denoting locale Message
        /// </summary>
        private static LocaleEnum _systemUILocale = GlobalizationHelper.GetSystemUILocale();

        /// <summary>
        /// Field denoting localeMessageBL
        /// </summary>
        private static LocaleMessageBL _localeMessageBL = new LocaleMessageBL();

        /// <summary>
        /// 
        /// </summary>
        private static String defaultErrorMessage = " {0} were not loaded. Either they are not available in the database or there were issues loading it. This needs to be corrected before doing any imports.";

        #endregion Fields

        #region Methods

        #region Public Methods

        #region Data Validation Methods

        /// <summary>
        /// Validate the organization name
        /// </summary>
        /// <param name="organizationName">Indicates the name of organization</param>
        /// <param name="iDataModelOperationResult">Indicates the result of operation</param>
        /// <param name="callerContext">Indicates the context of caller</param>
        public static void ValidateOrganizationName(String organizationName, IDataModelOperationResult iDataModelOperationResult, CallerContext callerContext)
        {
            if (String.IsNullOrWhiteSpace(organizationName))
            {
                AddOperationResult(iDataModelOperationResult, "111865", "Organization Name is empty or not specified.", null, OperationResultType.Error, TraceEventType.Error, callerContext);
            }
        }

        /// <summary>
        /// Validate the container name
        /// </summary>
        /// <param name="containerName">Indicates the name of container</param>
        /// <param name="iDataModelOperationResult">Indicates the result of operation</param>
        /// <param name="callerContext">Indicates the context of caller</param>
        public static void ValidateContainerName(String containerName, IDataModelOperationResult iDataModelOperationResult, CallerContext callerContext)
        {
            if (String.IsNullOrWhiteSpace(containerName))
            {
                AddOperationResult(iDataModelOperationResult, "111681", "Container Name is empty or not specified", null, OperationResultType.Error, TraceEventType.Error, callerContext);
            }
        }

        /// <summary>
        /// Validate the entity type name
        /// </summary>
        /// <param name="entityTypeName">Indicates the name of entity type</param>
        /// <param name="iDataModelOperationResult">Indicates the result of operation</param>
        /// <param name="callerContext">Indicates the context of caller</param>
        public static void ValidateEntityTypeName(String entityTypeName, IDataModelOperationResult iDataModelOperationResult, CallerContext callerContext)
        {
            if (String.IsNullOrWhiteSpace(entityTypeName))
            {
                AddOperationResult(iDataModelOperationResult, "112648", "EntityType Name is empty or not specified", null, OperationResultType.Error, TraceEventType.Error, callerContext);
            }
        }

        /// <summary>
        /// Validate the attribute unique identifier
        /// </summary>
        /// <param name="attributeName">Indicates the name of attribute</param>
        /// <param name="attributeParentName">Indicates the name parent name of attribute</param>
        /// <param name="iDataModelOperationResult">Indicates the result of operation</param>
        /// <param name="callerContext">Indicates the context of caller</param>
        public static void ValidateAttributeUniqueIdentifier(String attributeName, String attributeParentName, IDataModelOperationResult iDataModelOperationResult, CallerContext callerContext)
        {
            if (String.IsNullOrWhiteSpace(attributeName))
            {
                AddOperationResult(iDataModelOperationResult, "112646", "AttributeName is empty or not specified.", null, OperationResultType.Error, TraceEventType.Error, callerContext);
            }

            if (String.IsNullOrWhiteSpace(attributeParentName))
            {
                AddOperationResult(iDataModelOperationResult, "112690", "Attribute group name is empty or not specified.", null, OperationResultType.Error, TraceEventType.Error, callerContext);
            }
        }

        /// <summary>
        /// Validate the hierarchy name
        /// </summary>
        /// <param name="hierarchyName">Indicates the name of hierarchy</param>
        /// <param name="iDataModelOperationResult">Indicates the result of operation</param>
        /// <param name="callerContext">Indicates the context of caller</param>
        public static void ValidateHierarchyName(String hierarchyName, IDataModelOperationResult iDataModelOperationResult, CallerContext callerContext)
        {
            if (String.IsNullOrWhiteSpace(hierarchyName))
            {
                AddOperationResult(iDataModelOperationResult, "112688", "Hierarchy name is empty or not specified.", null, OperationResultType.Error, TraceEventType.Error, callerContext);
            }
        }

        /// <summary>
        /// Validate the category name
        /// </summary>
        /// <param name="categoryName">Indicates the name of category</param>
        /// <param name="iDataModelOperationResult">Indicates the result of operation</param>
        /// <param name="callerContext">Indicates the context of caller</param>
        public static void ValidateCategoryName(String categoryName, IDataModelOperationResult iDataModelOperationResult, CallerContext callerContext)
        {
            if (String.IsNullOrWhiteSpace(categoryName))
            {
                AddOperationResult(iDataModelOperationResult, "113685", "Category name is empty or not specified.", null, OperationResultType.Error, TraceEventType.Error, callerContext);
            }
        }

        /// <summary>
        /// Validate the relationship type name
        /// </summary>
        /// <param name="relationshipTypeName">Indicates the name of relationship type</param>
        /// <param name="iDataModelOperationResult">Indicates the result of operation</param>
        /// <param name="callerContext">Indicates the context of caller</param>
        public static void ValidateRelationshipTypeName(String relationshipTypeName, IDataModelOperationResult iDataModelOperationResult, CallerContext callerContext)
        {
            if (String.IsNullOrWhiteSpace(relationshipTypeName))
            {
                AddOperationResult(iDataModelOperationResult, "113973", "Relationship type name is empty or not specified.", null, OperationResultType.Error, TraceEventType.Error, callerContext);
            }
        }

        /// <summary>
        /// Validates complex attributes for having children
        /// </summary>
        /// <param name="attributeModels">Specifies attribute models for validation</param>
        /// <param name="operationResult">Specifies validation operation result</param>
        public static void ValidateComplexAttributes(IAttributeModelCollection attributeModels, IOperationResult operationResult)
        {
            foreach (AttributeModel attributeModel in attributeModels)
            {
                if (attributeModel.IsComplex)
                {
                    IAttributeModelCollection childAttributeModels = attributeModel.GetChildAttributeModels();
                    if (childAttributeModels == null || !childAttributeModels.Any())
                    {
                        Object[] parameters = 
                        {
                            String.Format("{0}/{1}", attributeModel.AttributeParentName, attributeModel.Name)
                        };
                        operationResult.AddOperationResult("114055",
                            String.Format("Cannot map attribute {0}. Mapping complex attributes with no children is forbidden.", parameters), parameters,
                            OperationResultType.Error);
                    }
                    else if (attributeModel.IsHierarchical)
                    {
                        ValidateComplexAttributes(childAttributeModels, operationResult);
                    }
                }
            }
        }

        #endregion Data Validation Methods

        #region Get Methods

        /// <summary>
        /// Gets the organization id for given name.
        /// </summary>
        /// <param name="iOrganizationManager">Represents the reference of OrgsnizationBL.</param>
        /// <param name="organizationName">Specifies name of organization</param>
        /// <param name="organizations">Specifies collection of organizations</param>
        /// <param name="callerContext">>Context of caller making call to this utility.</param>
        /// <returns>organization id.</returns>
        public static Int32 GetOrganizationId(IOrganizationManager iOrganizationManager, String organizationName, ref OrganizationCollection organizations, CallerContext callerContext)
        {
            Int32 organizationId = 0;

            if (!String.IsNullOrWhiteSpace(organizationName))
            {
                if (iOrganizationManager != null && (organizations == null || organizations.Count < 1))
                {
                    organizations = iOrganizationManager.GetAll(new OrganizationContext(), callerContext);
                }

                if (organizations != null && organizations.Count > 0)
                {
                    Organization organization = organizations.Get(organizationName);

                    if (organization != null)
                    {
                        organizationId = organization.Id;
                    }
                }
                else
                {
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, String.Format(defaultErrorMessage, "Organizations"), MDMTraceSource.DataModelImport);
                }
            }

            return organizationId;
        }

        /// <summary>
        /// Gets the hierarchy id for given hierarchy name.
        /// </summary>
        /// <param name="iHierarchyManager">Represents the reference of HierarchyBL.</param>
        /// <param name="hierarchyName">Specifies name of hierarchy</param>
        /// <param name="hierarchies">Specifies collection of hierarchies.</param>
        /// <param name="callerContext">>Context of caller making call to this utility.</param>
        /// <returns>container id</returns>
        public static Int32 GetHierarchyId(IHierarchyManager iHierarchyManager, String hierarchyName, ref HierarchyCollection hierarchies, CallerContext callerContext)
        {
            Int32 hierarchyId = 0;

            if (!String.IsNullOrWhiteSpace(hierarchyName))
            {
                if (iHierarchyManager != null && (hierarchies == null || hierarchies.Count < 1))
                {
                    hierarchies = iHierarchyManager.GetAll(callerContext, false);
                }

                if (hierarchies != null && hierarchies.Count > 0)
                {
                    Hierarchy hierarchy = hierarchies.Get(hierarchyName);

                    if (hierarchy != null)
                    {
                        hierarchyId = hierarchy.Id;
                    }
                }
                else
                {
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, String.Format(defaultErrorMessage, "Hierarchies"), MDMTraceSource.DataModelImport);
                }
            }

            return hierarchyId;
        }

        /// <summary>
        /// Gets the container id for given container and organization name.
        /// </summary>
        /// <param name="iContainerManager">Represents the reference of ContainerBL.</param>
        /// <param name="containerName">Specifies name of container</param>
        /// <param name="organizationName">Specifies name of organization</param>
        /// <param name="containers">Specifies collection of containers.</param>
        /// <param name="callerContext">>Context of caller making call to this utility.</param>
        /// <returns>container id</returns>
        public static Int32 GetContainerId(IContainerManager iContainerManager, String containerName, String organizationName, ref ContainerCollection containers, CallerContext callerContext)
        {
            Int32 containerId = 0;

            if (!String.IsNullOrWhiteSpace(containerName) && !String.IsNullOrWhiteSpace(organizationName))
            {
                if (iContainerManager != null && (containers == null || containers.Count < 1))
                {
                    containers = iContainerManager.GetAll(callerContext, true);
                }

                if (containers != null && containers.Count > 0)
                {
                    Container container = containers.GetContainer(containerName, organizationName);

                    if (container != null)
                    {
                        containerId = container.Id;
                    }
                }
                else
                {
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, String.Format(defaultErrorMessage, "Containers"), MDMTraceSource.DataModelImport);
                }
            }

            return containerId;
        }

        /// <summary>
        /// Gets the category id for given category and hierarchy name.
        /// </summary>
        /// <param name="iCategoryManager">Represents the reference of CatergoryBl.</param>
        /// <param name="iHierarchyManager">Represents the reference of HierarchyBL.</param>
        /// <param name="categoryName">Specifies name of hierarchy</param>
        /// <param name="path">Specifies path of category</param>
        /// <param name="hierarchyName">Specifies name of hierarchy</param>
        /// <param name="hierarchyIdBaseCategories">Specifies hierarchy id based categories dictionary.</param>
        /// <param name="hierarchies">Specifies collection of hierarchies.</param>
        /// <param name="callerContext">>Context of caller making call to this utility.</param>
        /// <returns>container id</returns>
        public static Int64 GetCategoryId(ICategoryManager iCategoryManager, IHierarchyManager iHierarchyManager, String categoryName, String path, String hierarchyName, ref Dictionary<Int32, CategoryCollection> hierarchyIdBaseCategories, ref HierarchyCollection hierarchies, CallerContext callerContext)
        {
            Int64 categoryId = 0;

            Int32 hierarchyId = GetHierarchyId(iHierarchyManager, hierarchyName, ref hierarchies, callerContext);

            if (!String.IsNullOrWhiteSpace(categoryName) && !String.IsNullOrWhiteSpace(hierarchyName) && hierarchyId > 0)
            {
                CategoryCollection categories = null;
                hierarchyIdBaseCategories.TryGetValue(hierarchyId, out categories);

                //If categories are not available in the dictionary then get all categories based on hierarchy id.
                if (iCategoryManager != null && (categories == null || categories.Count < 1))
                {
                    categories = iCategoryManager.GetAllCategories(hierarchyId, callerContext, GlobalizationHelper.GetSystemDataLocale());

                    if (categories != null)
                    {
                        hierarchyIdBaseCategories.Add(hierarchyId, categories);
                    }
                }

                if (categories != null && categories.Count > 0)
                {
                    Category category = categories.Get(categoryName, path, hierarchyName);

                    if (category != null)
                    {
                        categoryId = category.Id;
                    }
                }
                else
                {
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, String.Format(defaultErrorMessage, "Categories"), MDMTraceSource.DataModelImport);
                }
            }

            return categoryId;
        }

        /// <summary>
        /// Gets the entity type id for given name.
        /// </summary>
        /// <param name="iEntityTypeManager">Represents the reference of EntityTypeBL.</param>
        /// <param name="entityTypeName">Specifies name of entity type</param>
        /// <param name="entityTypes">Specifies collection of entityTypes.</param>
        /// <param name="callerContext">>Context of caller making call to this utility.</param>
        /// <returns>entity type id.</returns>
        public static Int32 GetEntityTypeId(IEntityTypeManager iEntityTypeManager, String entityTypeName, ref EntityTypeCollection entityTypes, CallerContext callerContext)
        {
            Int32 entityTypeId = 0;

            if (!String.IsNullOrWhiteSpace(entityTypeName))
            {
                if (iEntityTypeManager != null && (entityTypes == null || entityTypes.Count < 1))
                {
                    entityTypes = iEntityTypeManager.GetAll(callerContext);
                }

                if (entityTypes != null && entityTypes.Count > 0)
                {
                    EntityType entityType = entityTypes.Get(entityTypeName);

                    if (entityType != null)
                    {
                        entityTypeId = entityType.Id;
                    }
                }
                else
                {
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, String.Format(defaultErrorMessage, "EntityTypes"), MDMTraceSource.DataModelImport);
                }
            }

            return entityTypeId;
        }

        /// <summary>
        /// Gets the relationship type id for given name.
        /// </summary>
        /// <param name="iRelationshipTypeManager">Represents the reference of RelationshipTypeBL</param>
        /// <param name="relationshipTypeName">Specifies name of relationship type</param>
        /// <param name="relationshipTypes">Specifies collection of relationshipTypes.</param>
        /// <param name="callerContext">>Context of caller making call to this utility.</param>
        /// <returns>relationship type id</returns>
        public static Int32 GetRelationshipTypeId(IRelationshipTypeManager iRelationshipTypeManager, String relationshipTypeName, ref RelationshipTypeCollection relationshipTypes, CallerContext callerContext)
        {
            Int32 relationshipTypeId = 0;

            if (!String.IsNullOrWhiteSpace(relationshipTypeName))
            {
                if (iRelationshipTypeManager != null && (relationshipTypes == null || relationshipTypes.Count < 1))
                {
                    relationshipTypes = iRelationshipTypeManager.GetAll(callerContext);
                }

                if (relationshipTypes != null && relationshipTypes.Count > 0)
                {
                    RelationshipType relationshipType = relationshipTypes.Get(relationshipTypeName);

                    if (relationshipType != null)
                    {
                        relationshipTypeId = relationshipType.Id;
                    }
                }
                else
                {
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, String.Format(defaultErrorMessage, "RelationshipTypes"), MDMTraceSource.DataModelImport);
                }
            }

            return relationshipTypeId;
        }

        /// <summary>
        /// Gets the AttributeModel for given attribute identifier or attributeName and attributeParentName.
        /// </summary>
        /// <param name="iAttributeModelManager">Indicates name of attribute model manager</param>
        /// <param name="attributeId">Indicates identifier of attribute.</param>
        /// <param name="attributeName">Indicates name of attribute name</param>
        /// <param name="attributeParentName">Indicates name of attribute parent name</param>
        /// <param name="attributeModels">Indicates collection of attributeModels.</param>
        /// <param name="callerContext">Indicates Context of caller making call to this utility.</param>
        /// <returns>AttributeModel</returns>
        public static AttributeModel GetAttributeModel(IAttributeModelManager iAttributeModelManager, Int32 attributeId, String attributeName, String attributeParentName, ref AttributeModelCollection attributeModels, CallerContext callerContext)
        {
            AttributeModel attributeModel = null;

            if (!String.IsNullOrWhiteSpace(attributeName) || attributeId > 0)
            {
                if (iAttributeModelManager != null && (attributeModels == null || attributeModels.Count < 1))
                {
                    attributeModels = iAttributeModelManager.GetAllBaseAttributeModels();
                }

                if (attributeModels != null && attributeModels.Count > 0)
                {
                    AttributeUniqueIdentifier attributeUniqueIdentifier = new AttributeUniqueIdentifier(attributeName, attributeParentName);

                    attributeModel = attributeModels.GetAttributeModel(attributeId, attributeUniqueIdentifier, GlobalizationHelper.GetSystemDataLocale(), true);
                }
                else
                {
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, String.Format(defaultErrorMessage, "AttributeModels"), MDMTraceSource.DataModelImport);
                }
            }

            return attributeModel;
        }

        /// <summary>
        /// Gets the relationship type entity type id for given relationship type and entity type name.
        /// </summary>
        /// <param name="relationshipTypeName">Specifies name of relationship type</param>
        /// <param name="entityTypeName">Specifies name of entity type</param>
        /// <param name="relationshipTypeEntityTypeMappings">Specifies collection of relationshipTypeEntityTypeMappings.</param>
        /// <param name="callerContext">>Context of caller making call to this utility.</param>
        /// <returns>relationship type entity type id.</returns>
        public static Int32 GetRelationshipTypeEntityTypeId(String relationshipTypeName, String entityTypeName, ref RelationshipTypeEntityTypeMappingCollection relationshipTypeEntityTypeMappings, CallerContext callerContext)
        {
            Int32 relationshipTypeEntityTypeId = 0;

            if (!String.IsNullOrWhiteSpace(relationshipTypeName) && !String.IsNullOrWhiteSpace(entityTypeName))
            {
                if (relationshipTypeEntityTypeMappings == null || relationshipTypeEntityTypeMappings.Count < 1)
                {
                    relationshipTypeEntityTypeMappings = new RelationshipTypeEntityTypeMappingBL().GetAll(callerContext);
                }

                if (relationshipTypeEntityTypeMappings != null && relationshipTypeEntityTypeMappings.Count > 0)
                {
                    RelationshipTypeEntityTypeMapping relationshipTypeEntityTypeMapping = relationshipTypeEntityTypeMappings.Get(entityTypeName, relationshipTypeName);

                    if (relationshipTypeEntityTypeMapping != null)
                    {
                        relationshipTypeEntityTypeId = relationshipTypeEntityTypeMapping.Id;
                    }
                }
                else
                {
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, String.Format(defaultErrorMessage, "RelationshipTypeEntityTypes"), MDMTraceSource.DataModelImport);
                }
            }

            return relationshipTypeEntityTypeId;
        }

        /// <summary>
        /// Gets the container relationship type entity type id for given organization , container , entity type and relationship type name.
        /// </summary>
        /// <param name="containerName">Specifies name of container</param>
        /// <param name="organizationName">Specifies name of organization</param>
        /// <param name="entityTypeName">Specifies name of entity type</param>
        /// <param name="relationshipTypeName">Specifies name of relationship type</param>
        /// <param name="containerRelationshipTypeEntityTypeMappings">Specifies collection of containerRelationshipTypeEntityTypeMappings.</param>
        /// <param name="callerContext"></param>
        /// <returns></returns>
        public static Int32 GetContainerRelationshipTypeEntityTypeId(String organizationName, String containerName, String entityTypeName, String relationshipTypeName, ref ContainerRelationshipTypeEntityTypeMappingCollection containerRelationshipTypeEntityTypeMappings, CallerContext callerContext)
        {
            Int32 containerRelationshipTypeEntityTypeId = 0;

            if (!String.IsNullOrWhiteSpace(organizationName) && !String.IsNullOrWhiteSpace(containerName) &&
                !String.IsNullOrWhiteSpace(entityTypeName) && !String.IsNullOrWhiteSpace(relationshipTypeName))
            {
                if (containerRelationshipTypeEntityTypeMappings == null || containerRelationshipTypeEntityTypeMappings.Count < 1)
                {
                    containerRelationshipTypeEntityTypeMappings = new ContainerRelationshipTypeEntityTypeMappingBL().GetAll(callerContext);
                }

                if (containerRelationshipTypeEntityTypeMappings != null && containerRelationshipTypeEntityTypeMappings.Count > 0)
                {
                    ContainerRelationshipTypeEntityTypeMapping containerRelationshipTypeEntityTypeMapping = containerRelationshipTypeEntityTypeMappings.Get(organizationName, containerName, entityTypeName, relationshipTypeName);

                    if (containerRelationshipTypeEntityTypeMapping != null)
                    {
                        containerRelationshipTypeEntityTypeId = containerRelationshipTypeEntityTypeMapping.Id;
                    }
                }
                else
                {
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, String.Format(defaultErrorMessage, "ContainerRelationshipTypeEntityTypes"), MDMTraceSource.DataModelImport);
                }
            }

            return containerRelationshipTypeEntityTypeId;
        }

        #endregion Get Methods

        #region Misc. Methods

        /// <summary>
        /// Add to the Operation result Collection
        /// </summary>
        public static void AddOperationResults(DataModelOperationResultCollection operationResults, String messageCode, String message, Object[] parameters, OperationResultType operationResultType, TraceEventType traceEventType, CallerContext callerContext)
        {
            if (callerContext == null)
            {
                callerContext = new CallerContext(MDMCenterApplication.MDMCenter, MDMCenterModules.DataModelImport);
            }

            LocaleMessage localeMessage = GetLocaleMessage(messageCode, message, parameters, callerContext);

            if (localeMessage != null)
            {
                operationResults.AddOperationResult(localeMessage.Code, localeMessage.Message, operationResultType);
            }
        }

        /// <summary>
        /// Add to the Operation result
        /// </summary>
        public static void AddOperationResult(IDataModelOperationResult operationResult, String messageCode, String message, Object[] parameters, OperationResultType operationResultType, TraceEventType traceEventType, CallerContext callerContext)
        {
            if (callerContext == null)
            {
                callerContext = new CallerContext(MDMCenterApplication.MDMCenter, MDMCenterModules.DataModelImport);
            }

            LocaleMessage localeMessage = GetLocaleMessage(messageCode, message, parameters, callerContext);

            if (localeMessage != null)
            {
                operationResult.AddOperationResult(localeMessage.Code, localeMessage.Message, parameters, operationResultType);
            }
        }

        /// <summary>
        /// Validates min and max relationships for relationship cardinality
        /// </summary>
        /// <param name="minRelationships">Indicates minimum relationships which are given in cardinality</param>
        /// <param name="maxRelationships">Indicates maximum relationships which are given in cardinality</param>
        /// <param name="operationResult">Indicates data model operation result</param>
        /// <param name="callerContext">Indicates caller context</param>
        public static void ValidateMinMaxRelationships(Int32 minRelationships, Int32 maxRelationships, IDataModelOperationResult operationResult, CallerContext callerContext)
        {
            #region Min and Max relationships validations

            if (minRelationships < 0)
            {
                AddOperationResult(operationResult, "101185", "Minimum relationship(s) value must be a non-negative number", null, OperationResultType.Error, TraceEventType.Error, callerContext);
            }

            if (maxRelationships < 0)
            {
                AddOperationResult(operationResult, "101186", "Maximum relationship(s) value must be a non-negative number", null, OperationResultType.Error, TraceEventType.Error, callerContext);
            }

            if (maxRelationships < minRelationships)
            {
                AddOperationResult(operationResult, "113864", "Maximum relationship(s) value must be greater than minimum relationship(s) value", null, OperationResultType.Error, TraceEventType.Error, callerContext);
            }

            #endregion Min and Max relationships validations
        }

        /// <summary>
        /// Get the unique identifier based on specified parameters
        /// </summary>
        /// <param name="parameters">Indicates the context parameter for unique identifier</param>
        /// <returns>Returns the unique identifier based on specified parameters</returns>
        public static String GetUniqueIdentifier(Collection<String> parameters)
        {
            String uniqueIdentifier = String.Empty;
            String delimiter = "_";

            if(parameters != null && parameters.Count() > 0)
            {
                uniqueIdentifier = ValueTypeHelper.JoinCollection(parameters, delimiter);
            }

            return uniqueIdentifier;
        }

        /// <summary>
        /// Get locale message text for given message code or message with given parameters
        /// </summary>
        /// <param name="messageCode">Indicates message code of message</param>
        /// <param name="message">Indicates default message if message code is not available</param>
        /// <param name="parameters">Indicates parameters needs to be appended in locale message</param>
        /// <param name="callerContext">Indicates caller identity</param>
        /// <returns>Returns locale message text for given message code or message</returns>
        public static String GetLocaleMessageText(String messageCode, String message, Object[] parameters, CallerContext callerContext)
        {
            LocaleMessage localeMessage = GetLocaleMessage(messageCode, message, parameters, callerContext);

            if (localeMessage != null)
            {
                return localeMessage.Message;
            }

            return message;
        }

        /// <summary>
        /// Populates operation results with error messages regarding non existent domain objects e.g EntityType, Organization e.t.c
        /// </summary>
        /// <param name="operationResult">Result of operation</param>
        /// <param name="errorMessages">List of error messages</param>
        /// <param name="parameters">List of parameters for error messages</param>
        /// <param name="callerContext">Context of the caller</param>
        public static void AddInvalidNamesErrorsToOperationResult(IDataModelOperationResult operationResult, List<String> errorMessages,
            List<String> parameters, CallerContext callerContext)
        {
            StringBuilder errorTextBuilder = new StringBuilder();

            errorTextBuilder.Append(GetLocaleMessageText("112635", String.Empty, null, callerContext)); // 112635 - Invalid

            for (int i = 0; i < errorMessages.Count; i++)
            {
                errorTextBuilder.Append(" " + errorMessages[i] + " : {" + i + "},");
            }

            AddOperationResult(operationResult, String.Empty, errorTextBuilder.ToString().Trim(','), parameters.ToArray(),
                OperationResultType.Error, TraceEventType.Error, callerContext);
        }

        #endregion
        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// Get locale message for given message code or message with given parameters
        /// </summary>
        /// <param name="messageCode">Indicates message code of message</param>
        /// <param name="message">Indicates default message if message code is not available</param>
        /// <param name="parameters">Indicates parameters needs to be appended in locale message</param>
        /// <param name="callerContext">Indicates caller identity</param>
        /// <returns>Returns locale message for given message code or message</returns>
        private static LocaleMessage GetLocaleMessage(String messageCode, String message, Object[] parameters, CallerContext callerContext)
        {
            LocaleMessage localeMessage;
            if (parameters != null && parameters.Count() > 0)
            {
                localeMessage = _localeMessageBL.TryGet(_systemUILocale, messageCode, message, parameters, false, callerContext);
            }
            else
            {
                localeMessage = _localeMessageBL.TryGet(_systemUILocale, messageCode, message, false, callerContext);
            }

            return localeMessage;
        }

        #endregion

        #endregion Methods
    }
}