using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;


namespace MDM.Interfaces
{
    using MDM.BusinessObjects;
    using MDM.BusinessObjects.EntityIdentification;
    using MDM.BusinessObjects.Exports;
    using MDM.BusinessObjects.Integration;
    using MDM.BusinessObjects.Interfaces;
    using MDM.BusinessObjects.Jobs;
    using MDM.BusinessObjects.Workflow;
    using MDM.Core;
    using EntityIdentification;
    using Exports;

    /// <summary>
    /// Provides utility methods to initialize interfaces
    /// </summary>
    public class MDMObjectFactory
    {
        #region Get IAttributeModelContext

        /// <summary>
        /// Get IAttributeModelContext interface
        /// </summary>
        /// <returns>IAttributeModelContext interface </returns>
        public static IAttributeModelContext GetIAttributeModelContext()
        {
            return (IAttributeModelContext)new AttributeModelContext();
        }

        /// <summary>
        /// Initialize IAttributeModelContext with given values
        /// </summary>
        /// <param name="containerId">ContainerId for AttributeModelContext</param>
        /// <param name="entityTypeId">EntityTypeId for AttributeModelContext</param>
        /// <param name="relationshipTypeId">RelationshipTypeId for AttributeModelContext</param>
        /// <param name="categoryId">CategoryId for AttributeModelContext</param>
        /// <param name="locales">Locales for AttributeModelContext</param>
        /// <param name="entityId">Entity Id for which we want attribute model. Entity Id is mainly used to evaluate BR for custom views</param>
        /// <param name="attributeModelType">Indicate which type of attributes are to be loaded</param>
        /// <param name="getOnlyShowAtCreationAttributes">Indicates that only attributes those are marked as ShowAtCreation = true</param>
        /// <param name="getOnlyRequiredAttributes">Indicates that only required attributes are to be loaded</param>
        /// <param name="getCompleteDetailsOfAttribute">Indicates that all detail about attributes are to be loaded</param>
        /// <returns>IAttributeModelContext interface initialized with given information</returns>
        public static IAttributeModelContext GetIAttributeModelContext(Int32 containerId, Int32 entityTypeId, Int32 relationshipTypeId, Int64 categoryId, Collection<LocaleEnum> locales, Int64 entityId, AttributeModelType attributeModelType, Boolean getOnlyShowAtCreationAttributes, Boolean getOnlyRequiredAttributes, Boolean getCompleteDetailsOfAttribute)
        {
            AttributeModelContext context = new AttributeModelContext(containerId, entityTypeId, relationshipTypeId, categoryId, locales, entityId, attributeModelType, getOnlyShowAtCreationAttributes, getOnlyRequiredAttributes, getCompleteDetailsOfAttribute);

            return (IAttributeModelContext)context;
        }

        #endregion Get IAttributeModelContext

        #region Get IEntityUniqueIdentifer

        /// <summary>
        /// Initializes a new instance of the EntityUniqueIdentifier class
        /// </summary>
        /// <returns>IEntityUniqueIdentifier instance</returns>
        public static IEntityUniqueIdentifier GetIEntityUniqueIdentifier()
        {
            return new EntityUniqueIdentifier();
        }

        /// <summary>
        /// Initializes a new instance of the EntityUniqueIdentifier class with the given entity Id, container name, entity type name, and category path
        /// </summary>
        /// <param name="entityId">Indicates identifier of an entity</param>
        /// <param name="containerName">Indicates name of the container</param>
        /// <param name="entityTypeName">Indicates name of the entity type</param>
        /// <param name="categoryPath">Indicates category path</param>
        /// <returns>IEntityUniqueIdentifier instance initialized with the given information</returns>
        public static IEntityUniqueIdentifier GetIEntityUniqueIdentifier(Int64 entityId, String containerName = "", String entityTypeName = "", String categoryPath = "")
        {
            return new EntityUniqueIdentifier
            {
                EntityId = entityId,
                ContainerName = containerName,
                EntityTypeName = entityTypeName,
                CategoryPath = categoryPath
            };
        }

        /// <summary>
        /// Initializes a new instance of the EntityUniqueIdentifier class with given external Id, container name, entity type name, and category path
        /// </summary>
        /// <param name="externalId">Indicates external Id (shortname) of an entity</param>
        /// <param name="containerName">Indicates name of the container</param>
        /// <param name="entityTypeName">Indicates name of the entity type</param>
        /// <param name="categoryPath">Indicates category path</param>
        /// <returns>IEntityUniqueIdentifier instance initialized with the given information</returns>
        public static IEntityUniqueIdentifier GetIEntityUniqueIdentifier(String externalId, String containerName = "", String entityTypeName = "", String categoryPath = "")
        {
            return new EntityUniqueIdentifier
            {
                ExternalId = externalId,
                ContainerName = containerName,
                EntityTypeName = entityTypeName,
                CategoryPath = categoryPath
            };
        }

        /// <summary>
        /// Initializes a new instance of the EntityUniqueIdentifier class with the given external Id, container Id, entity type Id, and category Id
        /// </summary>
        /// <param name="externalId">Indicates external Id (shortname) of an entity</param>
        /// <param name="containerId">Indicates identifier of the container</param>
        /// <param name="entityTypeId">Indicates identifier of the entity type</param>
        /// <param name="categoryId">Indicates identifier of the category</param>
        /// <returns>IEntityUniqueIdentifier instance initialized with the given information</returns>
        public static IEntityUniqueIdentifier GetIEntityUniqueIdentifier(String externalId, Int32 containerId = 0, Int32 entityTypeId = 0, Int64 categoryId = 0)
        {
            return new EntityUniqueIdentifier
            {
                ExternalId = externalId,
                ContainerId = containerId,
                EntityTypeId = entityTypeId,
                CategoryId = categoryId
            };
        }

        #endregion
        
        #region Get EntityContext Objects

        #region Get IEntityContext

        /// <summary>
        /// Get IEntityContext interface 
        /// </summary>
        /// <returns>IEntityContext instance</returns>
        public static IEntityContext GetIEntityContext()
        {
            return (IEntityContext)new EntityContext();
        }

        /// <summary>
        /// Get IEntityContext interface 
        /// </summary>
        /// <param name="containerId">containerId</param>
        /// <param name="categoryId">categoryId</param>
        /// <param name="entityTypeId">entityTypeId</param>
        /// <param name="locale">locale</param>
        /// <param name="dataLocales">dataLocales</param>
        /// <param name="loadEntityProperties">loadEntityProperties</param>
        /// <param name="loadAttributes">loadAttributes</param>
        /// <param name="loadHierarchyRelationships">loadHierarchyRelationships</param>
        /// <param name="loadExtensionRelationships">loadExtensionRelationships</param>
        /// <param name="attributeGroupIdList">attributeGroupIdList</param>
        /// <param name="attributeIdList">attributeIdList</param>
        /// <param name="loadRelationships">loadRelationships</param>
        /// <param name="loadRelationshipAttributes">loadRelationshipAttributes</param>
        /// <param name="relationshipTypeIdList">relationshipTypeIdList</param>
        /// <param name="loadCreationAttributes">loadCreationAttributes</param>
        /// <param name="loadRequiredAttribtues">loadRequiredAttribtues</param>
        /// <param name="attributeModelType">attributeModelType</param>
        /// <returns>IEntityContext instance</returns>
        public static IEntityContext GetIEntityContext(Int32 containerId, Int64 categoryId, Int32 entityTypeId, LocaleEnum locale, Collection<LocaleEnum> dataLocales,
            Boolean loadEntityProperties, Boolean loadAttributes, Boolean loadHierarchyRelationships, Boolean loadExtensionRelationships, Collection<Int32> attributeGroupIdList, Collection<Int32> attributeIdList,
            Boolean loadRelationships, Boolean loadRelationshipAttributes, Collection<Int32> relationshipTypeIdList,
            Boolean loadCreationAttributes, Boolean loadRequiredAttribtues, AttributeModelType attributeModelType)
        {
            var entityContext = new EntityContext(containerId, categoryId, entityTypeId, locale, dataLocales, loadEntityProperties, loadAttributes, loadHierarchyRelationships, loadExtensionRelationships, attributeGroupIdList, attributeIdList, loadRelationships, loadRelationshipAttributes, relationshipTypeIdList, loadCreationAttributes, loadRequiredAttribtues, attributeModelType);
            return entityContext;
        }

        /// <summary>
        /// Get IEntityContext for given entity context xml 
        /// </summary>
        /// <param name="valuesAsXml">XMl of EntityContext</param>
        /// <returns>IEntityContext instance</returns>
        public static IEntityContext GetIEntityContext(String valuesAsXml)
        {
            return (IEntityContext)new EntityContext(valuesAsXml);
        }

        #endregion

        #region Get IEntityDataContext

        /// <summary>
        /// Initializes a new instance of the EntityDataContext class
        /// </summary>
        /// <returns>IEntityDataContext instance</returns>
        public static IEntityDataContext GetIEntityDataContext()
        {
            return (IEntityDataContext)new EntityContext();
        }

        /// <summary>
        /// Initializes a new instance of the EntityDataContext class with the given attribute context and locales
        /// </summary>
        /// <param name="loadAttributes">Indicates whether to load attributes or not</param>
        /// <param name="attributeContext">Indicates context for an attribute</param>
        /// <param name="dataLocales">Indicates data locales</param>
        /// <returns>IEntityDataContext instance initialized with the given information</returns>
        public static IEntityDataContext GetIEntityDataContext(Boolean loadAttributes, IAttributeContext attributeContext, params LocaleEnum[] dataLocales)
        {
            var entityContext = new EntityContext { LoadAttributes = loadAttributes };

            entityContext.SetAttributeContext(attributeContext);

            if (dataLocales != null && dataLocales.Length > 0)
            {
                entityContext.DataLocales = new Collection<LocaleEnum>(dataLocales);
            }

            return entityContext;
        }

        /// <summary>
        /// Initializes a new instance of the EntityDataContext class with the given relationship context and locales
        /// </summary>
        /// <param name="loadRelationships">Indicates whether to load relationships or not</param>
        /// <param name="relationshipContext">Indicates context for relationship</param>
        /// <param name="dataLocales">Indicates data locales</param>
        /// <returns>IEntityDataContext instance initialized with the given information</returns>
        public static IEntityDataContext GetIEntityDataContext(Boolean loadRelationships, IRelationshipContext relationshipContext, params LocaleEnum[] dataLocales)
        {
            var entityContext = new EntityContext { LoadRelationships = loadRelationships };

            entityContext.SetRelationshipContext(relationshipContext);

            if (dataLocales != null && dataLocales.Length > 0)
            {
                entityContext.DataLocales = new Collection<LocaleEnum>(dataLocales);
            }

            return entityContext;
        }

        /// <summary>
        /// Initializes a new instance of the EntityDataContext class with the given attribute context, relationship context, and locales
        /// </summary>
        /// <param name="loadAttributes">Indicates whether to load attributes or not</param>
        /// <param name="attributeContext">Indicates context for an attribute</param>
        /// <param name="loadRelationships">Indicates whether to load relationships or not</param>
        /// <param name="relationshipContext">Indicates context for relationship</param>
        /// <param name="dataLocales">Indicates data locales</param>
        /// <returns>IEntityDataContext instance initialized with the given information</returns>
        public static IEntityDataContext GetIEntityDataContext(Boolean loadAttributes, IAttributeContext attributeContext, Boolean loadRelationships, IRelationshipContext relationshipContext, params LocaleEnum[] dataLocales)
        {
            var entityContext = new EntityContext
            {
                LoadAttributes = loadAttributes,
                LoadRelationships = loadRelationships
            };

            entityContext.SetAttributeContext(attributeContext);
            entityContext.SetRelationshipContext(relationshipContext);

            if (dataLocales != null && dataLocales.Length > 0)
            {
                entityContext.DataLocales = new Collection<LocaleEnum>(dataLocales);
            }

            return entityContext;
        }

        /// <summary>
        /// Get IEntityDataContextCollection interface
        /// </summary>
        /// <returns>IEntityDataContextCollection instance</returns>
        public static IEntityDataContextCollection GetIEntityDataContextCollection()
        {
            return new EntityContextCollection();
        }

        /// <summary>
        /// Get IEntityDataContextCollection interface
        /// </summary>
        /// <param name="entityDataContexts">Indicates entity data contexts</param>
        /// <returns>IEntityDataContextCollection instance initialized with given information</returns>
        public static IEntityDataContextCollection GetIEntityDataContextCollection(params IEntityDataContext[] entityDataContexts)
        {
            var entityContextCollection = new EntityContextCollection();

            foreach (var entityDataContext in entityDataContexts)
            {
                entityContextCollection.Add(entityDataContext);    
            }

            return entityContextCollection;
        }

        #endregion

        #region Get IEntityModelContext

        /// <summary>
        /// Initializes a new instance of the EntityModelContext class
        /// </summary>
        /// <returns>IEntityModelContext instance</returns>
        public static IEntityModelContext GetIEntityModelContext()
        {
            return (IEntityModelContext)new EntityModelContext();
        }

        /// <summary>
        /// Initializes a new instance of the EntityModelContext class with the given container Id, entity type Id, and category Id
        /// </summary>
        /// <param name="containerId">Indicates identifier of the container</param>
        /// <param name="entityTypeId">Indicates identifier of an entity type</param>
        /// <param name="categoryId">Indicates identifier of the category</param>
        /// <returns>IEntityModelContext instance initialized with the given information</returns>
        public static IEntityModelContext GetIEntityModelContext(Int32 containerId, Int32 entityTypeId, Int64 categoryId)
        {
            return new EntityModelContext
            {
                ContainerId = containerId,
                EntityTypeId = entityTypeId,
                CategoryId = categoryId
            };
        }

        /// <summary>
        /// Initializes a new instance of the EntityModelContext class with the given container name, entity type name, and category name
        /// </summary>
        /// <param name="containerName">Indicates name of the container</param>
        /// <param name="entityTypeName">Indicates name of an entity type</param>
        /// <param name="categoryName">Indicates name of the category</param>
        /// <returns>IEntityModelContext interface initialized with given information</returns>
        public static IEntityModelContext GetIEntityModelContext(String containerName, String entityTypeName, String categoryName)
        {
            return new EntityModelContext
            {
                ContainerName = containerName,
                EntityTypeName = entityTypeName,
                CategoryPath = categoryName
            };
        }

        /// <summary>
        /// Initializes a new instance of the EntityModelContext class with the given container Id and relationship type Id
        /// </summary>
        /// <param name="containerId">Indicates identifier of the container</param>
        /// <param name="relationshipTypeId">Indicates identifier of the relationship type</param>
        /// <returns>IEntityModelContext interface initialized with the given information</returns>
        public static IEntityModelContext GetIEntityModelContext(Int32 containerId, Int32 relationshipTypeId)
        {
            return new EntityModelContext
            {
                ContainerId = containerId,
                RelationshipTypeIds = new Collection<Int32> {relationshipTypeId}
            };
        }

        /// <summary>
        /// Initializes a new instance of the EntityModelContext class with the given container name and relationship type name
        /// </summary>
        /// <param name="containerName">Indicates name of the container</param>
        /// <param name="relationshipTypeName">Indicates name of the relationship type</param>
        /// <returns> IEntityModelContext interface initialized with the given information</returns>
        public static IEntityModelContext GetIEntityModelContext(String containerName, String relationshipTypeName)
        {
            return new EntityModelContext
            {
                ContainerName = containerName,
                RelationshipTypeNames = new Collection<String> { relationshipTypeName }
            };
        }

        /// <summary>
        /// Initializes a new instance of the EntityModelContext class with the given container name and collection of relationship type names
        /// </summary>
        /// <param name="containerName">Indicates name of the container</param>
        /// <param name="relationshipTypeNames">Indicates names of the relationship type</param>
        /// <returns>IEntityModelContext interface initialized with the given information</returns>
        public static IEntityModelContext GetIEntityModelContext(String containerName, Collection<String> relationshipTypeNames)
        {
            return new EntityModelContext
            {
                ContainerName = containerName,
                RelationshipTypeNames = relationshipTypeNames
            };
        }

        /// <summary>
        /// Initializes a new instance of the EntityModelContext class with the given container Id and collection of relationship type Ids
        /// </summary>
        /// <param name="containerId">Indicates identifier of the container</param>
        /// <param name="relationshipTypeIds">Indicates identifiers of the relationship type</param>
        /// <returns>IEntityModelContext interface initialized with the given information</returns>
        public static IEntityModelContext GetIEntityModelContext(Int32 containerId, Collection<Int32> relationshipTypeIds)
        {
            return new EntityModelContext
            {
                ContainerId = containerId,
                RelationshipTypeIds = relationshipTypeIds
            };
        }

        #endregion
        
        #region Get IAttributeContext

        /// <summary>
        /// Initializes a new instance of the AttributeContext class
        /// </summary>
        /// <returns>IAttributeContext instance</returns>
        public static IAttributeContext GetIAttributeContext()
        {
            return (IAttributeContext)new AttributeContext();
        }

        /// <summary>
        /// Initializes a new instance of the AttributeContext class with the given attribute Ids
        /// </summary>
        /// <param name="attributeIds">Indicates identifiers of the attributes</param>
        /// <returns>IAttributeContext instance initialized with the given information</returns>
        public static IAttributeContext GetIAttributeContext(params Int32[] attributeIds)
        {
            var attributeIdList = new Collection<Int32>(attributeIds);

            return GetIAttributeContext(attributeIdList, null);
        }

        /// <summary>
        /// Initializes a new instance of the AttributeContext class with the given attribute Ids
        /// </summary>
        /// <param name="attributeGroupIds">Indicates identifiers of the attribute groups</param>
        /// <returns>IAttributeContext instance initialized with the given information</returns>
        public static IAttributeContext GetIAttributeContext(Collection<Int32> attributeGroupIds)
        {
            return GetIAttributeContext(null, attributeGroupIds);
        }

        /// <summary>
        /// Initializes a new instance of the AttributeContext class with the given attribute names
        /// </summary>
        /// <param name="attributeNames">Indicates names of the attributes</param>
        /// <returns>IAttributeContext instance initialized with the given information</returns>
        public static IAttributeContext GetIAttributeContext(params String[] attributeNames)
        {
            var attributeNameList = new Collection<String>(attributeNames);

            return GetIAttributeContext(attributeNameList);
        }

        /// <summary>
        /// Initializes a new instance of the AttributeContext class with the given attribute Ids and boolean values
        /// </summary>
        /// <param name="attributeIdList">Indicates identifiers of the attributes</param>
        /// <param name="attributeGroupIdList">Indicates identifiers of the attributes groups</param>
        /// <param name="loadLookupDisplayValues">Indicates whether to load lookup display values or not</param>
        /// <param name="loadLookupRowWithValues">Indicates whether to load lookup row with values or not</param>
        /// <returns>IAttributeContext instance initialized with the given information</returns>
        public static IAttributeContext GetIAttributeContext(Collection<Int32> attributeIdList, Collection<Int32> attributeGroupIdList, Boolean loadLookupDisplayValues = true, Boolean loadLookupRowWithValues = false)
        {
            return new AttributeContext
            {
                AttributeIdList = attributeIdList,
                AttributeGroupIdList = attributeGroupIdList,
                LoadLookupDisplayValues = loadLookupDisplayValues,
                LoadLookupRowWithValues = loadLookupRowWithValues
            };
        }

        /// <summary>
        /// Initializes a new instance of the AttributeContext class with the given attribute names and boolean values
        /// </summary>
        /// <param name="attributeNames">Indicates names of the attributes</param>
        /// <param name="loadLookupDisplayValues">Indicates whether to load lookup display values or not</param>
        /// <param name="loadLookupRowWithValues">Indicates whether to load lookup row with values or not</param>
        /// <returns>IAttributeContext instance initialized with the given information</returns>
        public static IAttributeContext GetIAttributeContext(Collection<String> attributeNames, Boolean loadLookupDisplayValues = true, Boolean loadLookupRowWithValues = false)
        {
            return new AttributeContext
            {
                AttributeNames = attributeNames,
                LoadLookupDisplayValues = loadLookupDisplayValues,
                LoadLookupRowWithValues = loadLookupRowWithValues
            };
        }

        /// <summary>
        /// Initializes a new instance of the AttributeContext class with the given attribute model type and boolean values
        /// </summary>
        /// <param name="attributeModelType">Indicates attribute model type</param>
        /// <param name="loadRequiredAttributes">Indicates whether to load required attributes or not</param>
        /// <param name="loadCreationAttributes">Indicates whether to load creation attributes or not</param>
        /// <param name="loadLookupDisplayValues">Indicates whether to load lookup display values or not</param>
        /// <param name="loadLookupRowWithValues">Indicates whether to load lookup row with values or not</param>
        /// <returns>IAttributeContext instance initialized with the given information</returns>
        public static IAttributeContext GetIAttributeContext(AttributeModelType attributeModelType, Boolean loadRequiredAttributes, Boolean loadCreationAttributes, Boolean loadLookupDisplayValues = true, Boolean loadLookupRowWithValues = false)
        {
            return new AttributeContext
            {
                AttributeModelType = attributeModelType,
                LoadRequiredAttributes = loadRequiredAttributes,
                LoadCreationAttributes = loadCreationAttributes,
                LoadLookupDisplayValues = loadLookupDisplayValues,
                LoadLookupRowWithValues = loadLookupRowWithValues
            };
        }

        #endregion

        #region Get IRelationshipContext

        /// <summary>
        /// Get IRelationshipContext interface 
        /// </summary>
        /// <returns>IRelationshipContext instance</returns>
        public static IRelationshipContext GetIRelationshipContext()
        {
            return (IRelationshipContext)new RelationshipContext();
        }

        /// <summary>
        /// Get IRelationshipContext interface 
        /// </summary>
        /// <param name="relationshipTypeIds">Indicates identifiers of the relationship type</param>
        /// <returns>IRelationshipContext instance</returns>
        public static IRelationshipContext GetIRelationshipContext(params Int32[] relationshipTypeIds)
        {
            var relationshipTypeIdsList = new Collection<Int32>(relationshipTypeIds);

            return GetIRelationshipContext(relationshipTypeIdsList);
        }

        /// <summary>
        /// Get IRelationshipContext interface 
        /// </summary>
        /// <param name="relationshipTypeNames">Indicates names of the relationship type</param>
        /// <returns>IRelationshipContext instance</returns>
        public static IRelationshipContext GetIRelationshipContext(params String[] relationshipTypeNames)
        {
            var relationshipTypeNameList = new Collection<String>(relationshipTypeNames);

            return GetIRelationshipContext(relationshipTypeNameList);
        }

        /// <summary>
        /// Get IRelationshipContext interface
        /// </summary>
        /// <param name="relationshipTypeIdList">Indicates list of relationship ids of relationships to load</param>
        /// <param name="loadRelationshipAttributes">Indicates whether to load relationship attributes or not</param>
        /// <param name="locale">Indicates locale for values</param>
        /// <param name="level">Indicates level of relationships to load</param>
        /// <returns>IRelationshipContext instance</returns>
        public static IRelationshipContext GetIRelationshipContext(Collection<Int32> relationshipTypeIdList, Boolean loadRelationshipAttributes = true, LocaleEnum locale = LocaleEnum.UnKnown, Int16 level = 10)
        {
            return new RelationshipContext
            {
                RelationshipTypeIdList = relationshipTypeIdList,
                LoadRelationshipAttributes = loadRelationshipAttributes,
                Locale = locale,
                Level = level
            };
        }

        /// <summary>
        /// Get IRelationshipContext interface 
        /// </summary>
        /// <param name="relationshipTypeNames">Indicates relationship names of relationships to load</param>
        /// <param name="loadRelationshipAttributes">Indicates whether to load relationship attributes or not</param>
        /// <returns>IRelationshipContext instance</returns>
        public static IRelationshipContext GetIRelationshipContext(Collection<String> relationshipTypeNames, Boolean loadRelationshipAttributes = true)
        {
            return new RelationshipContext
            {
                RelationshipTypeNames = relationshipTypeNames,
                LoadRelationshipAttributes = loadRelationshipAttributes
            };
        }

        #endregion Get IRelationshipContext

        #region Get IEntityHierarchyContext

        /// <summary>
        /// Initializes a new instance of the EntityHierarchyContext class
        /// </summary>
        /// <returns>IEntityHierarchyContext instance</returns>
        public static IEntityHierarchyContext GetIEntityHierarchyContext()
        {
            return (IEntityHierarchyContext)new EntityHierarchyContext();
        }

        /// <summary>
        /// Initializes a new instance of the EntityHierarchyContext class with the given entity data context collection 
        /// </summary>
        /// <param name="entityHierarchyDataContexts">Indicates entity hierarchy data contexts for which hierarchy is to be loaded</param>
        /// <returns>IEntityHierarchyContext instance initialized with the given information</returns>
        public static IEntityHierarchyContext GetIEntityHierarchyContext(IEntityDataContextCollection entityHierarchyDataContexts)
        {
            return new EntityHierarchyContext
            {
                EntityContexts = (EntityContextCollection)entityHierarchyDataContexts
            };
        }

        #endregion Get IEntityHierarchyContext

        #region Get IEntityExtensionContext

        /// <summary>
        /// Initializes a new instance of the EntityExtensionContext class
        /// </summary>
        /// <returns>IEntityExtensionContext instance</returns>
        public static IEntityExtensionContext GetIEntityExtensionContext()
        {
            return (IEntityExtensionContext)new EntityExtensionContext();
        }

        /// <summary>
        /// Initializes a new instance of the EntityExtensionContext class with the given entity data context collection 
        /// </summary>
        /// <param name="entityExtensionDataContexts">Indicates entity extension data contexts for which hierarchy is to be loaded</param>
        /// <returns>IEntityExtensionContext instance initialized with the given information</returns>
        public static IEntityExtensionContext GetIEntityExtensionContext(IEntityDataContextCollection entityExtensionDataContexts)
        {
            return new EntityExtensionContext
            {
                EntityContexts = (EntityContextCollection)entityExtensionDataContexts
            };
        }

        #endregion Get IEntityExtensionContext

        #region Get IEntityContextCollection

        /// <summary>
        /// Gets IEntityContextCollection interface
        /// </summary>
        /// <returns>IEntityContextCollection instance</returns>
        public static IEntityContextCollection GetIEntityContextCollection()
        {
            return (IEntityContextCollection)new EntityContextCollection(); 
        }

        #endregion Get IEntityConextCollection

        #endregion Get IEntityContext

        #region Get IEntityHistory

        /// <summary>
        /// Initializes a new instance of the Entity History class.
        /// </summary>
        /// <returns>IEntityHistory instance</returns>
        public static IEntityHistory GetIEntityHistory()
        {
            return (IEntityHistory)new EntityHistory();
        }

        /// <summary>
        /// Initializes a new instance of the Entity History class.
        /// </summary>
        /// <param name="valuesAsXml">Xml representation of EntityHistory</param>
        /// <returns>IEntityHistory instance</returns>
        public static IEntityHistory GetIEntityHistory(String valuesAsXml)
        {
            return (IEntityHistory)new EntityHistory(valuesAsXml);
        }

        #endregion

        #region Get IEntityGetOptions

       
        /// <summary>
        /// Initializes a new instance of the EntityGetOptions class with the given boolean values 
        /// </summary>
        /// <param name="publishEvents">Indicates whether to publish events or not</param>
        /// <param name="applyAVS">Indicates whether to apply avs or not</param>
        /// <param name="applySecurity">Indicates whether to apply security or not</param>
        /// <returns>IEntityGetOptions instance initialized with the given boolean values</returns>
        public static IEntityGetOptions GetIEntityGetOptions(Boolean publishEvents = true, Boolean applyAVS = true, Boolean applySecurity = true)
        {
            return new EntityGetOptions
        {
                ApplyAVS = applyAVS,
                ApplySecurity = applySecurity,
                PublishEvents = publishEvents
            };
        }

        #endregion

        #region Get IEntityHistoryContext

        /// <summary>
        /// Initializes a new instance of the Entity History Context class.
        /// </summary>
        /// <returns>IEntityHistoryContext instance</returns>
        public static IEntityHistoryContext GetIEntityHistoryContext()
        {
            return (IEntityHistoryContext)new EntityHistoryContext();
        }

        /// <summary>
        /// Initializes a new instance of the Entity History Context class.
        /// </summary>
        /// <param name="valuesAsXml">Xml representation of IEntityHistoryContext</param>
        /// <returns>IEntityHistoryContext instance</returns>
        public static IEntityHistoryContext GetIEntityHistoryContext(String valuesAsXml)
        {
            return (IEntityHistoryContext)new EntityHistoryContext(valuesAsXml);
        }

        #endregion

        #region Get IEntityHistoryRecord

        /// <summary>
        /// Initializes a new instance of the Entity History Record class.
        /// </summary>
        /// <returns>IEntityHistoryrecord instance</returns>
        public static IEntityHistoryRecord GetIEntityHistoryRecord()
        {
            return (IEntityHistoryRecord)new EntityHistoryRecord();
        }

        /// <summary>
        /// Initializes a new instance of the Entity History Record class.
        /// </summary>
        /// <param name="valuesAsXml">Xml representation of EntityHistoryRecord</param>
        /// <returns>IEntityHistoryRecord instance</returns>
        public static IEntityHistoryRecord GetIEntityHistoryRecord(String valuesAsXml)
        {
            return (IEntityHistoryRecord)new EntityHistoryRecord(valuesAsXml);
        }

        #endregion

        #region Get IEntityCollectionHistory

        /// <summary>
        /// Initializes a new instance of the Entity Collection History class.
        /// </summary>
        /// <returns>IEntityCollectionHistory instance</returns>
        public static IEntityCollectionHistory GetIEntityCollectionHistory()
        {
            return (IEntityCollectionHistory)new EntityCollectionHistory();
        }

        /// <summary>
        /// Initializes a new instance of the Entity Collection History class.
        /// </summary>
        /// <param name="valuesAsXml">Xml representation of EntityCollectionHistory</param>
        /// <returns>IEntityCollectionHistory instance</returns>
        public static IEntityCollectionHistory GetIEntityCollectionHistory(String valuesAsXml)
        {
            return (IEntityCollectionHistory)new EntityCollectionHistory(valuesAsXml);
        }

        #endregion

        #region Get IAttributeModelContext
        /// <summary>
        /// Get IAttributeModelContext
        /// </summary>
        /// <param name="valuesAsXml">xml having value to be used to initialize IAttributeModelContext</param>
        /// <returns>IAttributeModelContext having values specified in valuesAsXml</returns>
        public static IAttributeModelContext GetIAttributeModelContext(String valuesAsXml)
        {
            return (IAttributeModelContext)new AttributeModelContext(valuesAsXml);
        }
        #endregion

        #region Get ILookupContext

        /// <summary>
        /// Get ILookupContext interface
        /// </summary>
        /// <returns>ILookupContext</returns>
        public static ILookupContext GetILookupContext()
        {
            return (ILookupContext)new LookupContext();
        }

        /// <summary>
        /// Get ILookupContext interface
        /// </summary>
        /// <param name="valuesAsXml">Xml having value to be used to initialize ILookupContext</param>
        /// <returns>ILookupContext having values specified in valuesAsXml</returns>
        public static ILookupContext GetILookupContext(String valuesAsXml)
        {
            return (ILookupContext)new LookupContext(valuesAsXml);
        }

        #endregion Get ILookupContext

        #region Get ITableCollectioin

        /// <summary>
        /// Get ITableCollection
        /// </summary>
        /// <returns>ITableCollection instance</returns>
        public static ITableCollection GetITableCollection()
        {
            return (ITableCollection)new TableCollection();
        }

        /// <summary>
        /// Get ITableCollection
        /// </summary>
        /// <param name="valueAsXml">Xml having values to initialize ITableCollection</param>
        /// <returns>ITableCollection instance</returns>
        public static ITableCollection GetITableCollection(String valueAsXml)
        {
            return (ITableCollection)new TableCollection(valueAsXml);
        }

        #endregion Get ITableCollectioin

        #region Get IColumn

        /// <summary>
        /// Parameterless Constructor
        /// </summary>
        /// <returns>instance of IColumn</returns>
        public static IColumn GetIColumn()
        {
            return (IColumn)new Column();
        }

        /// <summary>
        /// Constructor with Id as input parameter
        /// <param name="id">Indicates the Identity of Column Instance (RSTObjectId)</param>
        /// </summary>
        /// <returns>instance of IColumn</returns>
        public static IColumn GetIColumn(Int32 id)
        {
            return (IColumn)new Column(id);
        }

        /// <summary>
        /// Constructor with all properties of Column object
        /// </summary>
        /// <param name="id">Indicates the Identity of a Column (RSTObjectId)</param>
        /// <param name="name">Indicates the Name of a Column </param>
        /// <param name="longName">Indicates the LongName of a Column </param>
        /// <param name="defaultValue">Indicates the Default Value for the Column </param>
        /// <returns>instance of IColumn</returns>
        public static IColumn GetIColumn(Int32 id, String name, String longName, Object defaultValue)
        {
            return (IColumn)new Column(id, name, longName, defaultValue);
        }

        /// <summary>
        /// Create column object from xml
        /// </summary>
        /// <param name="valuesAsXml">
        /// Xml having values which we want to populate in current object
        /// <para>
        /// Sample Xml:
        /// <![CDATA[
        /// <Column Id="" Name="LookupKey" DisplayName="LookupKey" />
        /// ]]>
        /// </para>
        /// </param>
        /// <returns>instance of IColumn</returns>
        public static IColumn GetIColumn(String valuesAsXml)
        {
            return (IColumn)new Column(valuesAsXml);
        }

        #endregion Get IColumn

        #region Get IColumnCollection

        /// <summary>
        /// Get the Column collection Interface instance. 
        /// </summary>
        /// <returns>instance of IColumnCollection</returns>
        public static IColumnCollection GetIColumnCollection()
        {
            return (IColumnCollection)new ColumnCollection();
        }

        #endregion Get IColumnCollection

        #region Get ILookupCollection

        /// <summary>
        /// Get ILookupCollection
        /// </summary>
        /// <returns>ILookupCollection instance</returns>
        public static ILookupCollection GetILookupCollection()
        {
            return (ILookupCollection)new LookupCollection();
        }

        /// <summary>
        /// Get ILookupCollection
        /// </summary>
        /// <param name="valueAsXml">Xml having values to initialize ILookupCollection</param>
        /// <returns>ILookupCollection instance</returns>
        public static ILookupCollection GetILookupCollection(String valueAsXml)
        {
            return (ILookupCollection)new LookupCollection(valueAsXml);
        }

        #endregion Get ILookupCollection

        #region Get IEntity

        /// <summary>
        /// Get IEntity object
        /// </summary>
        /// <returns>IEntity interface initialized</returns>
        public static IEntity GetIEntity()
        {
            return (IEntity)new Entity();
        }

        /// <summary>
        /// Get IEntity object
        /// </summary>
        /// <param name="id">Entity Id</param>
        /// <returns>IEntity interface initialized</returns>
        public static IEntity GetIEntity(Int32 id)
        {
            return (IEntity)new Entity(id);
        }

        /// <summary>
        /// Get IEntity object
        /// </summary>
        /// <param name="id">Entity Id</param>
        /// <param name="name">Entity short name</param>
        /// <param name="longName">Entity long name</param>
        /// <returns>IEntity interface initialized</returns>
        public static IEntity GetIEntity(Int32 id, String name, String longName)
        {
            return (IEntity)new Entity(id, name, longName);
        }

        /// <summary>
        /// Get IEntity object
        /// </summary>
        /// <param name="id">Entity Id</param>
        /// <param name="name">Entity short name</param>
        /// <param name="longName">Entity long name</param>
        /// <param name="locale">Locale for entity</param>
        /// <returns>IEntity interface initialized</returns>
        public static IEntity GetIEntity(Int32 id, String name, String longName, LocaleEnum locale)
        {
            return (IEntity)new Entity(id, name, longName, locale);
        }

        /// <summary>
        /// Get IEntity object
        /// </summary>
        /// <param name="valuesAsXml">Xml having value for entity object</param>
        /// <returns>IEntity interface initialized</returns>
        public static IEntity GetIEntity(String valuesAsXml)
        {
            return (IEntity)new Entity(valuesAsXml);
        }

        #endregion Get IEntity

        #region Get IEntityCollection

        /// <summary>
        /// Parameterless constructor
        /// </summary>
        /// <returns>IEntityCollection instance</returns>
        public static IEntityCollection GetIEntityCollection()
        {
            return (IEntityCollection)new EntityCollection();
        }

        /// <summary>
        /// Constructor which takes Xml as input
        /// </summary>
        /// <param name="valueAsXml">xml of EntityCollection</param>
        /// <returns>IEntityCollection instance</returns>
        public static IEntityCollection GetIEntityCollection(String valueAsXml)
        {
            return (IEntityCollection)new EntityCollection(valueAsXml);
        }

        #endregion Get IEntityCollection

        #region Get IOperationResult

        /// <summary>
        /// Get IOperationResult
        /// </summary>
        /// <returns>IOperationResult populated with value</returns>
        public static IOperationResult GetIOperationResult()
        {
            return (IOperationResult)new OperationResult();
        }

        /// <summary>
        /// Get IOperationResult from Xml
        /// </summary>
        /// <param name="valuesAsXml">XMl of OperationResult</param>
        /// <returns>IOperationResult populated with value</returns>
        public static IOperationResult GetIOperationResult(String valuesAsXml)
        {
            return (IOperationResult)new OperationResult(valuesAsXml);
        }

        /// <summary>
        /// Get IEntityOperationResult
        /// </summary>
        /// <returns>IEntityOperationResult populated with value</returns>
        public static IEntityOperationResult GetIEntityOperationResult()
        {
            return (IEntityOperationResult)new EntityOperationResult();
        }

        /// <summary>
        /// Get IEntityOperationResult from Xml
        /// </summary>
        /// <param name="valuesAsXml">Xml of EntityOperationResult</param>
        /// <returns>IEntityOperationResult populated with value</returns>
        public static IEntityOperationResult GetIEntityOperationResult(String valuesAsXml)
        {
            return (IEntityOperationResult)new EntityOperationResult(valuesAsXml);
        }

        /// <summary>
        /// Get IEntityOperationResult
        /// </summary>
        /// <returns>IEntityOperationResult populated with value</returns>
        public static IEntityOperationResultCollection GetIEntityOperationResultCollection()
        {
            return (IEntityOperationResultCollection)new EntityOperationResultCollection();
        }

        /// <summary>
        /// Get IEntityOperationResultCollection from  Xml
        /// </summary>
        /// <param name="valuesAsXml">XMl of EntityOperationResultCollection</param>
        /// <returns>IEntityOperationResultCollection populated with values</returns>
        public static IEntityOperationResultCollection GetIEntityOperationResultCollection(String valuesAsXml)
        {
            return (IEntityOperationResultCollection)new EntityOperationResultCollection(valuesAsXml);
        }

        #endregion Get IOperationResult

        #region Get IOperationResultCollection

        /// <summary>
        /// Get IOperationResultCollection
        /// </summary>
        /// <returns>IOperationResultCollection default instance</returns>
        public static IOperationResultCollection GetIOperationResultCollection()
        {
            return new OperationResultCollection();
        }

        #endregion Get IOperationResultCollection

        #region Get ISearchCriteria

        /// <summary>
        /// Initializes a new instance of the Search Criteria class.
        /// </summary>
        /// <returns>ISearchCriteria instance</returns>
        public static ISearchCriteria GetSearchCriteria()
        {
            return (ISearchCriteria)new SearchCriteria();
        }

        /// <summary>
        /// Initializes a new instance of the Search Criteria class for category or entity type search.
        /// </summary>
        /// <param name="organizationId">Specifying organization id.</param>
        /// <param name="containerIds">Array of container ids</param>
        /// <param name="categoryIds">Array of category ids</param>
        /// <param name="entityTypeIds">Array of entity type ids</param>
        /// <param name="locales">locales</param>
        /// <returns>ISearchCriteria instance</returns>
        public static ISearchCriteria GetSearchCriteria(Int32 organizationId, Collection<Int32> containerIds, Collection<Int64> categoryIds, Collection<Int32> entityTypeIds, Collection<LocaleEnum> locales)
        {
            return (ISearchCriteria)new SearchCriteria(organizationId, containerIds, categoryIds, entityTypeIds, locales);
        }

        /// <summary>
        /// Initializes a new instance of the Search Criteria class for workflow search.
        /// </summary>
        /// <param name="organizationId">Specifying organization id.</param>
        /// <param name="containerIds">Array of container ids</param>
        /// <param name="workflowName">Name of the workflow</param>
        /// <param name="workflowStages">Array of workflow stages</param>
        /// <param name="workflowAssignedUsers">Array of workflow assigned users</param>
        /// <param name="locales">locales</param>
        /// <returns>ISearchCriteria instance</returns>
        public static ISearchCriteria GetSearchCriteria(Int32 organizationId, Collection<Int32> containerIds, String workflowName, String[] workflowStages, String[] workflowAssignedUsers, Collection<LocaleEnum> locales)
        {
            return (ISearchCriteria)new SearchCriteria(organizationId, containerIds, workflowName, workflowStages, workflowAssignedUsers, locales);
        }

        /// <summary>
        /// Initializes a new instance of the Search Criteria class for attribute rules
        /// </summary>
        /// <param name="organizationId">Specifying organization id.</param>
        /// <param name="containerIds">Array of container ids</param>
        /// <param name="locales">locales</param>
        /// <param name="iSearchAttributeRules">Collection of search attribute rules</param>
        /// <returns>ISearchCriteria instance</returns>
        public static ISearchCriteria GetSearchCriteria(Int32 organizationId, Collection<Int32> containerIds, Collection<LocaleEnum> locales, Collection<ISearchAttributeRule> iSearchAttributeRules)
        {
            Collection<SearchAttributeRule> searchAttributeRules = new Collection<SearchAttributeRule>();
            if (iSearchAttributeRules != null)
            {
                foreach (SearchAttributeRule rule in iSearchAttributeRules)
                {
                    searchAttributeRules.Add(rule);
                }
            }
            return (ISearchCriteria)new SearchCriteria(organizationId, containerIds, locales, searchAttributeRules);
        }

        /// <summary>
        /// Initializes a new instance of the Search Criteria class
        /// </summary>
        /// <param name="organizationId">Specifying organization id.</param>
        /// <param name="containerIds">Array of container ids</param>
        /// <param name="categoryIds">Array of category ids</param>
        /// <param name="entityTypeIds">Array of entity type ids</param>
        /// <param name="workflowName">Name of the workflow</param>
        /// <param name="workflowStages">Array of workflow stages</param>
        /// <param name="workflowAssignedUsers">Array of workflow assigned users</param>
        /// <param name="locales">locales</param>
        /// <param name="iSearchAttributeRules">Collection of search attribute rules</param>
        /// <returns>ISearchCriteria instance</returns>
        public static ISearchCriteria GetSearchCriteria(Int32 organizationId, Collection<Int32> containerIds, Collection<Int64> categoryIds, Collection<Int32> entityTypeIds, String workflowName, String[] workflowStages, String[] workflowAssignedUsers, Collection<LocaleEnum> locales, Collection<ISearchAttributeRule> iSearchAttributeRules)
        {
            Collection<SearchAttributeRule> searchAttributeRules = new Collection<SearchAttributeRule>();
            if (iSearchAttributeRules != null)
            {
                foreach (SearchAttributeRule rule in iSearchAttributeRules)
                {
                    searchAttributeRules.Add(rule);
                }
            }
            return (ISearchCriteria)new SearchCriteria(organizationId, containerIds, categoryIds, entityTypeIds, workflowName, workflowStages, workflowAssignedUsers, locales, searchAttributeRules);
        }

        /// <summary>
        /// Xml constructor
        /// </summary>
        /// <param name="valuesAsXml">Xml having value for current SearchContext object</param>
        /// <returns>ISearchCriteria instance</returns>
        public static ISearchCriteria GetSearchCriteria(String valuesAsXml)
        {
            return (ISearchCriteria)new SearchCriteria(valuesAsXml);
        }

        #endregion Get ISearchCriteria

        #region Get ISearchContext

        /// <summary>
        /// Get ISearchContext instance
        /// </summary>
        /// <returns>ISearchContext instance</returns>
        public static ISearchContext GetSearchContext()
        {
            return (ISearchContext)new SearchContext();
        }

        /// <summary>
        /// Initializes a new instance of the Search Specifications class.
        /// </summary>
        /// <param name="maxRecordsToReturn"></param>
        /// <param name="returnScores"></param>
        /// <param name="returnAttributeIdList"></param>
        /// <returns>ISearchContext instance</returns>
        public static ISearchContext GetSearchContext(Int32 maxRecordsToReturn, Boolean returnScores, Collection<Attribute> returnAttributeIdList)
        {
            return (ISearchContext)new SearchContext(maxRecordsToReturn, returnScores, returnAttributeIdList);
        }

        /// <summary>
        /// Xml constructor
        /// </summary>
        /// <param name="valuesAsXml">Xml having value for current SearchContext object</param>
        /// <returns>ISearchContext instance</returns>
        public static ISearchContext GetSearchContext(String valuesAsXml)
        {
            return (ISearchContext)new SearchContext(valuesAsXml);
        }

        #endregion Get ISearchContext

        #region Get ICategory

        /// <summary>
        /// Initializes a new instance of the Category class.
        /// </summary>
        /// <returns>instance of the Category</returns>
        public static ICategory GetICategory()
        {
            return (ICategory)new Category();
        }

        /// <summary>
        /// Xml constructor
        /// </summary>
        /// <param name="valuesAsxml">Xml having value for current category object</param>
        /// <returns>instance of the Category</returns>
        public static ICategory GetICategory(String valuesAsxml)
        {
            return (ICategory)new Category(valuesAsxml);
        }

        #endregion Get ICategory

        #region Get ICategoryCollection

        /// <summary>
        /// Initializes a new instance of the Category collection class.
        /// </summary>
        /// <returns>instance of the Category collection</returns>
        public static ICategoryCollection GetICategoryCollection()
        {
            return (ICategoryCollection)new CategoryCollection();
        }

        /// <summary>
        /// Xml constructor
        /// </summary>
        /// <param name="valuesAsXml">Xml having value for categories</param>
        /// <returns>instance of the Category collection</returns>
        public static ICategoryCollection GetICategoryCollection(String valuesAsXml)
        {
            return (ICategoryCollection)new CategoryCollection(valuesAsXml);
        }

        /// <summary>
        /// Get IWorkflowActionContext
        /// </summary>
        /// <returns>IWorkflowActionContext</returns>
        public static IWorkflowActionContext GetIWorkflowActionContext()
        {
            return (IWorkflowActionContext)new WorkflowActionContext();
        }

        #endregion Get ICategoryCollection

        #region Get ICategoryAttributeMapping

        /// <summary>
        /// Gets an instance of an object that implements <see cref="ICategoryAttributeMapping"/>.
        /// </summary>
        /// <returns>An instance of an object that implements <see cref="ICategoryAttributeMapping"/></returns>
        public static ICategoryAttributeMapping GetICategoryAttributeMapping()
        {
            return new CategoryAttributeMapping();
        }

        /// <summary>
        /// Gets an instance of an object that implements <see cref="ICategoryAttributeMapping"/>.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="action">The action.</param>
        /// <returns>An instance of an object that implements <see cref="ICategoryAttributeMapping"/></returns>
        public static ICategoryAttributeMapping GetICategoryAttributeMapping(int id, ObjectAction action)
        {
            return new CategoryAttributeMapping { Id = id, Action = action };
        }

        #endregion

        #region Get ICategoryAttributeMappingCollection

        /// <summary>
        /// Gets an instance of an object that implements <see cref="ICategoryAttributeMappingCollection"/>.
        /// </summary>
        /// <returns>An instance of an object that implements <see cref="ICategoryAttributeMappingCollection"/></returns>
        public static ICategoryAttributeMappingCollection GetICategoryAttributeMappingCollection()
        {
            return new CategoryAttributeMappingCollection();
        }

        #endregion

        #region Get IHierarchyRelationship

        /// <summary>
        /// Initializes a new instance of the hierarchy relationship
        /// </summary>
        /// <returns>IHierarchyRelationship instance</returns>
        public static IHierarchyRelationship GetIHierarchyRelationship()
        {
            return (IHierarchyRelationship)new HierarchyRelationship();
        }

        /// <summary>
        /// Initializes a new instance of the hierarchy relationship with the XML having values of object
        /// </summary>
        /// <param name="valuesAsXml">xml of IHierarchyRelationship instance</param>
        /// <returns>IHierarchyRelationship instance</returns>
        public static IHierarchyRelationship GetIHierarchyRelationship(String valuesAsXml)
        {
            return (IHierarchyRelationship)new HierarchyRelationship(valuesAsXml);
        }

        #endregion Get IHierarchyRelationship

        #region Get IHierarchyRelationshipCollection

        /// <summary>
        /// Initializes a new instance of the hierarchy relationship collection
        /// </summary>
        /// <returns>IHierarchyRelationshipCollection instance</returns>
        public static IHierarchyRelationshipCollection GetIHierarchyRelationshipCollection()
        {
            return (IHierarchyRelationshipCollection)new HierarchyRelationshipCollection();
        }

        /// <summary>
        /// Initializes a new instance of the hierarchy relationship collection with the XML having values of object
        /// </summary>
        /// <param name="valuesAsXml">XML of IHierarchyRelationshipCollection </param>
        /// <returns>IHierarchyRelationshipCollection instance</returns>
        public static IHierarchyRelationshipCollection GetIHierarchyRelationshipCollection(String valuesAsXml)
        {
            return (IHierarchyRelationshipCollection)new HierarchyRelationshipCollection(valuesAsXml);
        }

        #endregion Get IHierarchyRelationshipCollection

        #region Get IExtensionRelationship

        /// <summary>
        /// Initializes a new instance of the extension relationship
        /// </summary>
        /// <returns>IExtensionRelationship instance</returns>
        public static IExtensionRelationship GetIExtenionRelationship()
        {
            return (IExtensionRelationship)new ExtensionRelationship();
        }

        /// <summary>
        /// Initializes a new instance of the extension relationship with the XML having values of object
        /// </summary>
        /// <param name="valuesAsXml">xml of ExtensionRelationship Object</param>
        /// <returns>IExtensionRelationship instance</returns>
        public static IExtensionRelationship GetIExtensionRelationship(String valuesAsXml)
        {
            return (IExtensionRelationship)new ExtensionRelationship(valuesAsXml);
        }

        #endregion Get IExtensionRelationship

        #region Get IExtensionRelationshipCollection

        /// <summary>
        /// Initializes a new instance of the extension relationship collection
        /// </summary>
        /// <returns>IExtensionRelationshipCollection instance</returns>
        public static IExtensionRelationshipCollection GetIExtensionRelationshipCollection()
        {
            return (IExtensionRelationshipCollection)new ExtensionRelationshipCollection();
        }

        /// <summary>
        /// Initializes a new instance of the extension relationship collection with the XML having values of object
        /// </summary>
        /// <param name="valuesAsXml">Xml of IExtensionRelationship instance</param>
        /// <returns>IExtensionRelationshipCollection instance</returns>
        public static IExtensionRelationshipCollection GetIExtensionRelationshipCollection(String valuesAsXml)
        {
            return (IExtensionRelationshipCollection)new ExtensionRelationshipCollection(valuesAsXml);
        }

        #endregion Get IExtensionRelationshipCollection

        #region Get IAttribute

        /// <summary>
        /// Gets interface for the attribute instance
        /// </summary>
        /// <returns>Attribute interface</returns>
        public static IAttribute GetIAttribute()
        {
            return (IAttribute)new Attribute();
        }

        /// <summary>
        /// Gets interface for the attribute instance for the requested Id
        /// </summary>
        /// <param name="attrId">Id of the attribute</param>
        /// <returns>Attribute interface</returns>
        public static IAttribute GetIAttribute(Int32 attrId)
        {
            return (IAttribute)new Attribute(attrId);
        }

        /// <summary>
        /// Gets interface for the attribute instance for the requested parameters
        /// </summary>
        /// <param name="attrId">Id of the attribute</param>
        /// <param name="name">Name of the attribute</param>
        /// <param name="longName">Long name of the attribute</param>
        /// <param name="attributeModelType">Attribute Model Type</param>
        /// <param name="attributeValue">Value of the attribute</param>
        /// <returns>Attribute interface</returns>
        public static IAttribute GetIAttribute(Int32 attrId, String name, String longName, AttributeModelType attributeModelType, Object attributeValue)
        {
            return (IAttribute)new Attribute(attrId, name, longName, attributeModelType, attributeValue);
        }

        /// <summary>
        /// Gets interface for the attribute instance for the requested attribute model
        /// </summary>
        /// <param name="attributeModel">Attribute Model</param>
        /// <returns>Attribute Interface</returns>
        public static IAttribute GetIAttribute(IAttributeModel attributeModel)
        {
            return (IAttribute)new Attribute((AttributeModel)attributeModel, attributeModel.Locale);
        }

        /// <summary>
        /// Get IAttribute XML from XML
        /// </summary>
        /// <param name="valueXml">IAttribute XML</param>
        /// <returns>IAttribute</returns>
        public static IAttribute GetIAttribute(String valueXml)
        {
            return (IAttribute)new Attribute(valueXml);
        }

        #endregion Get IAttribute

        #region Get IAttributeCollection

        /// <summary>
        /// Parameterless constructor
        /// </summary>
        /// <returns>IAttributeCollection</returns>
        public static IAttributeCollection GetIAttributeCollection()
        {
            return (IAttributeCollection)new AttributeCollection();
        }

        /// <summary>
        /// Constructor which takes Xml as input
        /// </summary>
        /// <param name="valueAsXml">XML of IAttributeCollection</param>
        /// <returns>IAttributeCollection</returns>
        public static IAttributeCollection GetIAttributeCollection(String valueAsXml)
        {
            return (IAttributeCollection)new AttributeCollection(valueAsXml);
        }

        /// <summary>
        /// Initialize IAttributeCollection from IList
        /// </summary>
        /// <param name="attributesList">IList of attributes</param>
        /// <returns>IAttributeCollection</returns>
        public static IAttributeCollection GetIAttributeCollection(IList<IAttribute> attributesList)
        {
            IList<Attribute> attributes = new List<Attribute>();

            if (attributesList != null)
            {
                foreach (Attribute attr in attributesList)
                {
                    attributes.Add(attr);
                }
            }
            return (IAttributeCollection)new AttributeCollection(attributes);
        }

        #endregion Get IAttributeCollection

        #region Get IEntityHierarchyDefinition

        /// <summary>
        /// Initializes a new instance of the EntityHierarchyDefinition class.
        /// </summary>
        /// <returns>EntityHierarchyDefinition instance</returns>
        public static IEntityVariantDefinition GetIEntityHierarchyDefinition()
        {
            return (IEntityVariantDefinition)new EntityVariantDefinition();
        }

        /// <summary>
        /// Initializes a new instance of the EntityHierarchyDefinition class using given xml.
        /// </summary>
        /// <param name="xml">Xml of EntityHierarchyDefinition</param>
        /// <returns>EntityHierarchyDefinition instance</returns>
        public static IEntityVariantDefinition GetIEntityHierarchyDefinition(String xml)
        {
            return (IEntityVariantDefinition)new EntityVariantDefinition(xml);
        }

        #endregion Get IEntityHierarchyDefinition

        #region Get IEntityProcessingOptions

        /// <summary>
        /// Initializes a new instance of the EntityProcessingOptions class
        /// </summary>
        /// <returns>instance of IEntityProcessingOptions</returns>
        public static IEntityProcessingOptions GetIEntityProcessingOptions()
        {
            return (IEntityProcessingOptions)new EntityProcessingOptions();
        }

        /// <summary>
        /// Initializes a new instance of the EntityProcessingOptions class passing various processing options
        /// </summary>
        /// <param name="validateEntities">validateEntities</param>
        /// <param name="publishEvents">publishEvents</param>
        /// <param name="processOnlyEntities">processOnlyEntities</param>
        /// <param name="processDefaultValues">processDefaultValues</param>
        /// <returns>instance of IEntityProcessingOptions</returns>
        public static IEntityProcessingOptions GetIEntityProcessingOptions(Boolean validateEntities, Boolean publishEvents, Boolean processOnlyEntities, Boolean processDefaultValues)
        {
            return (IEntityProcessingOptions)new EntityProcessingOptions(validateEntities, publishEvents, processOnlyEntities, processDefaultValues);
        }

        /// <summary>
        /// Initializes a new instance of the EntityProcessingOptions class passing xml
        /// </summary>
        /// <returns>instance of IEntityProcessingOptions</returns>
        public static IEntityProcessingOptions GetIEntityProcessingOptions(String valuesAsXml)
        {
            return (IEntityProcessingOptions)new EntityProcessingOptions(valuesAsXml);
        }

        #endregion Get IEntityProcessingOptions

        #region Get IAttributeUniqueIdentifier

        /// <summary>
        /// Initializes a new instance of the AttributeUniqueIdentifier class.
        /// </summary>
        /// <param name="attributeName">Indicates the name of the attribute</param>
        /// <param name="attributeGroupName">Indicates the group name of the attribute</param>
        /// <returns>Instance of the AttributeUniqueIdentifier</returns>
        public static IAttributeUniqueIdentifier GetIAttributeUniqueIdentifier(String attributeName, String attributeGroupName)
        {
            return (IAttributeUniqueIdentifier)new AttributeUniqueIdentifier(attributeName, attributeGroupName);
        }

        /// <summary>
        /// Initializes a new instance of the AttributeUniqueIdentifier class.
        /// </summary>
        /// <param name="attributeName">Indicates the name of the attribute</param>
        /// <param name="attributeGroupName">Indicates the group name of the attribute</param>
        /// <param name="instanceRefId">Indicates the instance ref id</param>
        /// <returns>Instance of the AttributeUniqueIdentifier</returns>
        public static IAttributeUniqueIdentifier GetIAttributeUniqueIdentifier(String attributeName, String attributeGroupName, Int32 instanceRefId)
        {
            return (IAttributeUniqueIdentifier)new AttributeUniqueIdentifier(attributeName, attributeGroupName, instanceRefId);
        }

        /// <summary>
        /// Initializes a new instance of the AttributeUniqueIdentifierCollection class.
        /// </summary>
        /// <returns>Instance of the AttributeUniqueIdentifierCollection</returns>
        public static IAttributeUniqueIdentifierCollection GetIAttributeUniqueIdentifierCollection()
        {
            return (IAttributeUniqueIdentifierCollection)new AttributeUniqueIdentifierCollection();
        }

        /// <summary>
        /// Initializes a new instance of the AttributeUniqueIdentifier class.
        /// </summary>
        /// <param name="valuesAsXml">XML of AttributeUniqueIdentifier</param>
        /// <returns>instance of the AttributeUniqueIdentifier</returns>
        public static IAttributeUniqueIdentifier GetIAttributeUniqueIdentifier(String valuesAsXml)
        {
            return (IAttributeUniqueIdentifier)new AttributeUniqueIdentifier(valuesAsXml);
        }

        #endregion

        #region Get IRelationshipCollection

        /// <summary>
        /// Parameterless constructor
        /// </summary>
        /// <returns>IRelationshipCollection instance</returns>
        public static IRelationshipCollection GetIRelationshipCollection()
        {
            return (IRelationshipCollection)new RelationshipCollection();
        }

        /// <summary>
        /// Constructor which takes Xml as input
        /// </summary>
        /// <param name="valueAsXml">XML of RelationshipCollection</param>
        /// <returns>IRelationshipCollection instance</returns>
        public static IRelationshipCollection GetIRelationshipCollection(String valueAsXml)
        {
            return (IRelationshipCollection)new RelationshipCollection(valueAsXml);
        }

        #endregion

        #region GetIAttributeModelCollection

        /// <summary>
        /// Initializes IAttributeModelCollection
        /// </summary>
        /// <returns>Returns the IAttributeModelCollection.</returns>
        public static IAttributeModelCollection GetIAttributeModelCollection()
        {
            return (IAttributeModelCollection)new AttributeModelCollection();
        }

        /// <summary>
        /// Initialize IAttributeCollection from IList
        /// </summary>
        /// <param name="attributeModelList">IList of attributes</param>
        /// <returns>Returns the IAttributeModelCollection.</returns>
        public static IAttributeModelCollection GetIAttributeModelCollection(IList<IAttributeModel> attributeModelList)
        {
            IList<AttributeModel> attributeModels = new List<AttributeModel>();

            AttributeModelCollection returnAttributeModelCollection = null;

            if (attributeModelList != null && attributeModelList.Count > 0)
            {
                foreach (AttributeModel attrModel in attributeModelList)
                {
                    attributeModels.Add(attrModel);
                }
            }

            if (attributeModels.Count > 0)
            {
                returnAttributeModelCollection = new AttributeModelCollection(attributeModels);
            }
            else
            {
                returnAttributeModelCollection = new AttributeModelCollection();
            }

            return (IAttributeModelCollection)returnAttributeModelCollection;
        }

        /// <summary>
        /// Initialize IAttributeModelCollection from Xml
        /// </summary>
        /// <param name="containerId">Indicate Container id</param>
        /// <param name="entityTypeId">Indicate entity type id</param>
        /// <param name="categoryId">Indicates category id.</param>
        /// <param name="locale">Indicates the locale</param>
        /// <param name="attributeModelType">Indicates the type of attribute model.</param>
        /// <param name="valuesAsXml">Indicates the Xml format of attribute model collection.</param>
        /// <param name="getCompleteDetailsOfAttribute">Indicates boolean value to indicates whether complete details of attribute is needed or not.</param>
        /// <returns>Returns the IAttribute Model Collection.</returns>
        public static IAttributeModelCollection GetIAttributeModelCollection(Int32 containerId, Int32 entityTypeId, Int64 categoryId, LocaleEnum locale, AttributeModelType attributeModelType, String valuesAsXml, Boolean getCompleteDetailsOfAttribute)
        {
            return (IAttributeModelCollection)new AttributeModelCollection(containerId, entityTypeId, categoryId, locale, attributeModelType, valuesAsXml, getCompleteDetailsOfAttribute);
        }

        #endregion GetIAttributeModelCollection

        #region Get IWorkflow

        /// <summary>
        /// Initializes workflow instance
        /// </summary>
        /// <returns>Workflow instance</returns>
        public static IWorkflow GetIWorkflow()
        {
            return (IWorkflow)new Workflow();
        }

        /// <summary>
        /// Initializes workflow instance for the provided workflow Id
        /// </summary>
        /// <param name="workflowId">Workflow Id</param>
        /// <returns>Workflow instance</returns>
        public static IWorkflow GetIWorkflow(Int32 workflowId)
        {
            return (IWorkflow)new Workflow(workflowId);
        }

        #endregion

        #region Get IWorkflowMDMObject

        /// <summary>
        /// Initializes workflow MDM object instance
        /// </summary>
        /// <returns>Workflow MDM object instance</returns>
        public static IWorkflowMDMObject GetIWorkflowMDMObject()
        {
            return (IWorkflowMDMObject)new WorkflowMDMObject();
        }

        /// <summary>
        /// Initializes workflow MDM object with provided Object Id and and Object Type
        /// </summary>
        /// <param name="mdmObjectId">Id of the MDM Object</param>
        /// <param name="mdmObjectType">Type of the MDM Object</param>
        /// <returns>Workflow MDM object instance</returns>
        public static IWorkflowMDMObject GetIWorkflowMDMObject(Int64 mdmObjectId, String mdmObjectType)
        {
            return (IWorkflowMDMObject)new WorkflowMDMObject(mdmObjectId, mdmObjectType);
        }

        #endregion

        #region Get IworkflowMDMObjectCollection

        /// <summary>
        /// Initializes workflow MDM object collection instance
        /// </summary>
        /// <returns>Workflow MDM object collection instance</returns>
        public static IWorkflowMDMObjectCollection GetIWorkflowMDMObjectCollection()
        {
            return (IWorkflowMDMObjectCollection)new WorkflowMDMObjectCollection();
        }

        #endregion

        #region Get ICallerContext

        /// <summary>
        /// Initializes CallerContext instance
        /// </summary>
        /// <returns>CallerContext instance</returns>
        public static ICallerContext GetICallerContext()
        {
            return (ICallerContext)new CallerContext();
        }


        /// <summary>
        /// Initializes OrganizationContext instance
        /// </summary>
        /// <returns>CallerContext instance</returns>
        public static IOrganizationContext GetIOrganizationContext()
        {
            return (IOrganizationContext)new OrganizationContext();
        }

        /// <summary>
        /// Initializes CallerContext instance for the provided application and module
        /// </summary>
        /// <param name="application">Indicates the application of the MDMCenter</param>
        /// <param name="module">Indicates the module of the MDMCenter</param>
        /// <returns>CallerContext instance</returns>
        public static ICallerContext GetICallerContext(MDMCenterApplication application, MDMCenterModules module)
        {
            return (ICallerContext)new CallerContext(application, module);
        }

        /// <summary>
        /// Initializes CallerContext instance for the provided application and module
        /// </summary>
        /// <param name="application">Indicates the application of the MDMCenter</param>
        /// <param name="module">Indicates the module of the MDMCenter</param>
        /// <param name="programName">Indicates the calling program name</param>
        /// <returns>CallerContext instance</returns>
        public static ICallerContext GetICallerContext(MDMCenterApplication application, MDMCenterModules module, String programName)
        {
            return (ICallerContext)new CallerContext(application, module, programName);
        }

        #endregion

        #region Get ISecurityUser

        /// <summary>
        /// Initializes security user
        /// </summary>
        /// <returns>Security User instance</returns>
        public static ISecurityUser GetISecurityUser()
        {
            return (ISecurityUser)new SecurityUser();
        }

        /// <summary>
        /// Initializes security user instance for the provided user Id
        /// </summary>
        /// <param name="userId">User Id</param>
        /// <returns>Security User instance</returns>
        public static ISecurityUser GetISecurityUser(Int32 userId)
        {
            return (ISecurityUser)new SecurityUser(userId);
        }

        /// <summary>
        /// Initialize ISecurityUser
        /// </summary>
        /// <param name="valuesAsXml">Xml format of ISecurityUser</param>
        /// <returns>Initialize ISecurityUser</returns>
        public static ISecurityUser GetISecurityUser(String valuesAsXml)
        {
            return (ISecurityUser)new SecurityUser(valuesAsXml);
        }

        #endregion Get ISecurityUser

        #region Get IUserCollection

        /// <summary>
        /// Initialize ISecurityUserCollection
        /// </summary>
        /// <returns>ISecurityUserCollection interface</returns>
        public static ISecurityUserCollection GetISecurityUserCollection()
        {
            return (ISecurityUserCollection)new SecurityUserCollection();
        }

        /// <summary>
        /// Initialize ISecurityUserCollection
        /// </summary>
        /// <param name="valueAsXml">Initialize interface from Xml</param>
        /// <returns>ISecurityUserCollection interface</returns>
        public static ISecurityUserCollection GetISecurityUserCollection(String valueAsXml)
        {
            return (ISecurityUserCollection)new SecurityUserCollection(valueAsXml);
        }

        #endregion Get IUserCollection

        #region Get IValue

        /// <summary>
        /// Parameterless Constructor
        /// </summary>
        /// <returns>IValue instance</returns>
        public static IValue GetIValue()
        {
            return (IValue)new Value();
        }

        /// <summary>
        /// Populate AttributeValue object from xml
        /// </summary>
        /// <example>
        /// Sample XML
        /// <para>
        /// &lt;Value 
        ///     AttrVal="cliaff.test" 
        ///     Uom="" 
        ///     ValueRefId="104" 
        ///     Locale="en_WW" 
        ///     Sequence="-1" /&gt;
        /// </para>
        /// </example>
        /// <param name="attributeDataType">Data type of in which value is to be cast and save.</param>
        /// <param name="valuesAsXml">String XML representing the value to fill in AttributeValue object</param>
        /// <returns>IValue instance</returns>
        public static IValue GetIValue(AttributeDataType attributeDataType, String valuesAsXml)
        {
            return (IValue)new Value(attributeDataType, valuesAsXml);
        }

        /// <summary>
        /// Initialize value object with AttrVal as object. Other properties like UOM, ValueRefId etc., cannot be set using this method.
        /// </summary>
        /// <param name="attrVal">Value of attribute</param>
        /// <returns>IValue interface.</returns>
        public static IValue GetIValue(Object attrVal)
        {
            return (IValue)new Value(attrVal);
        }

        #endregion Get IValue

        #region Get IValueCollection

        /// <summary>
        /// Parameterless constructor
        /// </summary>
        /// <returns>IValueCollection instance</returns>
        public static IValueCollection GetIValueCollection()
        {
            return (IValueCollection)new ValueCollection();
        }

        /// <summary>
        /// Initialize ValueCollection with Value
        /// </summary>
        /// <param name="value">Value object to add in ValueCollection</param>
        /// <returns>IValueCollection instance</returns>
        public static IValueCollection GetIValueCollection(Value value)
        {
            return (IValueCollection)new ValueCollection(value);
        }

        /// <summary>
        /// Initialize ValueCollection from Xml
        /// </summary>
        /// <param name="valuesAsXml">Xml having value for ValueCollection</param>
        /// <returns>IValueCollection instance</returns>
        public static IValueCollection GetIValueCollection(String valuesAsXml)
        {
            return (IValueCollection)new ValueCollection(valuesAsXml);
        }

        /// <summary>
        /// Initialize ValueCollection from IList of value
        /// </summary>
        /// <param name="valuesList">List of Value object</param>
        /// <returns>IValueCollection instance</returns>
        public static IValueCollection GetIValueCollection(IList<IValue> valuesList)
        {
            IList<Value> values = new List<Value>();

            if (valuesList != null)
            {
                foreach (Value val in valuesList)
                {
                    values.Add(val);
                }
            }
            return (IValueCollection)new ValueCollection(values);
        }

        #endregion Get IValueCollection

        #region Get ITable

        /// <summary>
        /// Parameterless Constructor
        /// </summary>
        /// <returns>ITable instance</returns>
        public static ITable GetITable()
        {
            return (ITable)new Table();
        }

        /// <summary>
        /// Constructor with all properties of Table object
        /// </summary>
        /// <param name="id">Indicates the Identity of a Table </param>
        /// <param name="name">Indicates the Name of a Table </param>
        /// <param name="longName">Indicates the LongName of a Table </param>
        /// <returns>ITable insatnce</returns>
        public static ITable GetITable(Int32 id, String name, String longName)
        {
            return (ITable)new Table(id, name, longName);
        }

        /// <summary>
        /// Create Table object from xml
        /// </summary>
        /// <param name="valuesAsXml">
        /// Xml having values which we want to populate in current object
        /// </param>
        /// <returns>ITable instance</returns>
        public static ITable GetITable(String valuesAsXml)
        {
            return (ITable)new Table(valuesAsXml);
        }

        #endregion

        #region Get IEntityMap

        /// <summary>
        /// Parameterless Constructor
        /// </summary>
        /// <returns>IEntityMap instance</returns>
        public static IEntityMap GetIEntityMap()
        {
            return (IEntityMap)new EntityMap();
        }

        /// <summary>
        /// Instantiates the new instance of Entity Map
        /// </summary>
        /// <param name="id">EntityMap Id</param>
        /// <param name="systemId">Id of the system from where entity has been imported</param>
        /// <param name="objectTypeId">Id of the object type</param>
        /// <param name="objectType">Type of the object type</param>
        /// <param name="externalId">Entity Id in the external system</param>
        /// <param name="internalId">Entity Internal Id</param>
        /// <param name="containerId">Container Id</param>
        /// <param name="categoryId">Category Id</param>
        /// <param name="entityTypeId">Entity Type Id</param>
        /// <returns>EntityMap Instance</returns>
        public static IEntityMap GetIEntityMap(Int64 id, String systemId, Int32 objectTypeId, String objectType, String externalId, Int64 internalId, Int32 containerId, Int64 categoryId, Int32 entityTypeId)
        {
            return (IEntityMap)new EntityMap(id, systemId, objectTypeId, objectType, externalId, internalId, containerId, categoryId, entityTypeId);
        }

        #endregion

        #region Get IEntityMapCollection

        /// <summary>
        /// Parameterless Constructor
        /// </summary>
        /// <returns>instance of IEntityMapCollection</returns>
        public static IEntityMapCollection GetIEntityMapCollection()
        {
            return (IEntityMapCollection)new EntityMapCollection();
        }

        #endregion

        #region Get IContainer

        /// <summary>
        /// Get IContainer object
        /// </summary>
        /// <returns>IContainer interface initialized</returns>
        public static IContainer GetIContainer()
        {
            return (IContainer)new Container();
        }

        /// <summary>
        /// Get IContainer object
        /// </summary>
        /// <param name="container">container object</param>
        /// <returns>IContainer interface initialized</returns>
        public static IContainer GetIContainer(Container container)
        {
            return (IContainer)container;
        }

        /// <summary>
        /// Initialize IContainerCollection
        /// </summary>
        /// <returns>Implementation of IContainerCollection</returns>
        public static IContainerCollection GetIContainerCollection()
        {
            return new ContainerCollection();
        }

        /// <summary>
        /// Get IContainer object
        /// </summary>
        /// <param name="valuesAsXml">Xml having value for container object</param>
        /// <returns>IContainer interface initialized</returns>
        public static IContainer GetIContainer(String valuesAsXml)
        {
            return (IContainer)new Container(valuesAsXml);
        }

        /// <summary>
        /// Get IContainer object
        /// </summary>
        /// <param name="valuesAsXml">Xml having value for container object</param>
        /// <returns>IContainer interface initialized</returns>
        public static IContainerCollection GetIContainerCollection(String valuesAsXml)
        {
            return (IContainerCollection)new ContainerCollection(valuesAsXml);
        }

        #endregion

        #region Get IApplicationContext

        /// <summary>
        /// Initializes ApplicationContext instance
        /// </summary>
        /// <returns>ApplicationContext instance</returns>
        public static IApplicationContext GetIApplicationContext()
        {
            return (IApplicationContext)new ApplicationContext();
        }

        /// <summary>
        /// Initializes ApplicationContext instance
        /// </summary>
        /// <param name="xmlValue">XML presentation of ApplicationContext </param>
        /// <returns>ApplicationContext instance</returns>
        public static IApplicationContext GetIApplicationContext(String xmlValue)
        {
            return (IApplicationContext)new ApplicationContext(xmlValue);
        }

        #endregion

        #region Get IContainerContext

        /// <summary>
        /// Get IContainerContext interface 
        /// </summary>
        /// <returns>insatnce of IContainerContext</returns>
        public static IContainerContext GetIContainerContext()
        {
            return (IContainerContext)new ContainerContext();
        }

        #endregion Get IContainerContext

        #region Get IDependentAttribute

        /// <summary>
        /// Initialize IDependentAttribute
        /// </summary>
        /// <returns>IDependentAttribute interface</returns>
        public static IDependentAttribute GetIDependentAttribute()
        {
            return new DependentAttribute();
        }

        #endregion Get IDependentAttribute

        #region Get IDependentAttributeCollection

        /// <summary>
        /// Initialize IDependentAttributeCollection
        /// </summary>
        /// <returns>IDependentAttributeCollection interface</returns>
        public static IDependentAttributeCollection GetIDependentAttributeCollection()
        {
            return new DependentAttributeCollection();
        }

        /// <summary>
        /// Initialize IDependentAttributeCollection
        /// </summary>
        /// <param name="list">IList of IDependentAttribute</param>
        /// <returns>IDependentAttributeCollection interface</returns>
        public static IDependentAttributeCollection GetIDependentAttributeCollection(IList<IDependentAttribute> list)
            {
            return GetDependentAttributeCollection(list);
        }

        /// <summary>
        /// Initialize IDependentAttributeCollection
        /// </summary>
        /// <param name="valuesAsXml">XML of IDependentAttributeCollection</param>
        /// <returns>IDependentAttributeCollection interface</returns>
        public static IDependentAttributeCollection GetIDependentAttributeCollection(String valuesAsXml)
        {
            return new DependentAttributeCollection(valuesAsXml);
        }

        private static IDependentAttributeCollection GetDependentAttributeCollection(IList<IDependentAttribute> list)
        {
            IList<DependentAttribute> dependentAttributes = new List<DependentAttribute>();

            if (list != null)
            {
                foreach (DependentAttribute attr in list)
        {
                    dependentAttributes.Add(attr);
                }
            }
            return new DependentAttributeCollection(dependentAttributes);
        }

        #endregion Get IDependentAttributeCollection

        #region Get IDependentAttributeDataMap

        /// <summary>
        /// Get IDependentAttributeDataMap object
        /// </summary>
        /// <returns>IDependentAttributeDataMap interface initialized</returns>
        public static IDependentAttributeDataMap GetIDependentAttributeDataMap()
        {
            return (IDependentAttributeDataMap)new DependentAttributeDataMap();
        }

        /// <summary>
        /// Get IDependentAttributeDataMap object
        /// </summary>
        /// <param name="dependentAttributeDataMap">DependentAttributeDataMap object</param>
        /// <returns>IDependentAttributeDataMap interface initialized</returns>
        public static IDependentAttributeDataMap GetIDependentAttributeDataMap(IDependentAttributeDataMap dependentAttributeDataMap)
        {
            return (IDependentAttributeDataMap)dependentAttributeDataMap;
        }

        /// <summary>
        /// Get dependentAttributeDataMap object
        /// </summary>
        /// <param name="valuesAsXml">Xml having value for container object</param>
        /// <returns>IDependentAttributeDataMap interface initialized</returns>
        public static IDependentAttributeDataMap GetIDependentAttributeDataMap(String valuesAsXml)
        {
            return (IDependentAttributeDataMap)new DependentAttributeDataMap(valuesAsXml);
        }

        /// <summary>
        /// Get IDependentAttributeDataMap object
        /// </summary>
        /// <param name="valuesAsXml">Xml having value for dependentAttributeDataMap object</param>
        /// <returns>IDependentAttributeDataMap interface initialized</returns>
        public static IDependentAttributeDataMapCollection GetIDependentAttributeDataMapCollection(String valuesAsXml)
        {
            return (IDependentAttributeDataMapCollection)new DependentAttributeDataMapCollection(valuesAsXml);
        }

        #endregion

        #region Get IAttributeModel

        /// <summary>
        /// Get IAttributeModel instance
        /// </summary>
        public static IAttributeModel GetIAttributeModel()
        {
            return new AttributeModel();
        }

        /// <summary>
        /// Parameter less constructor
        /// </summary>
        public static IAttributeModel GetIAttributeModel(Int32 id, String name, String longName)
        {
            return (IAttributeModel)new AttributeModel(id, name, longName);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="iAttributeModelBaseProperties"></param>
        /// <returns></returns>
        public static IAttributeModel GetIAttributeModel(IAttributeModelBaseProperties iAttributeModelBaseProperties)
        {
            AttributeModelBaseProperties attributeModelBaseProperties = null;

            if (iAttributeModelBaseProperties != null)
                attributeModelBaseProperties = (AttributeModelBaseProperties)iAttributeModelBaseProperties;

            return (IAttributeModel)new AttributeModel(attributeModelBaseProperties, null, null);
        }

        /// <summary>
        /// Parameter less constructor
        /// </summary>
        /// <returns></returns>
        public static IAttributeModelBaseProperties GetIAttributeModelBaseProperties()
        {
            return (IAttributeModelBaseProperties)new AttributeModelBaseProperties();
        }

        #endregion GetIAttributeModel

        #region Get ILocaleMessage

        /// <summary>
        /// Initialize ILocaleMessage
        /// </summary>
        /// <returns>ILocaleMessageinterface</returns>
        public static ILocaleMessage GetILocaleMessage()
        {
            return new LocaleMessage();
        }

        #endregion

        #region Get ILocaleMessageCollection

        /// <summary>
        /// Initialize ILocaleMessageCollection
        /// </summary>
        /// <returns>ILocaleMessageCollectioninterface</returns>
        public static ILocaleMessageCollection GetILocaleMessageCollection()
        {
            return new LocaleMessageCollection();
        }

        #endregion

        #region Get ILocaleCollection

        /// <summary>
        /// Initializes LocaleCollection instance
        /// </summary>
        /// <param name="valueAsXml">XML of LocaleCollection</param>
        /// <returns>LocaleCollection instance</returns>
        public static ILocaleCollection GetILocaleCollection(String valueAsXml)
        {
            return (ILocaleCollection)new LocaleCollection(valueAsXml);
        }

        #endregion

        #region Get IEntityExportProfile

        /// <summary>
        /// Get IEntityExportProfile object
        /// </summary>
        /// <returns>EntityExportProfile object</returns>
        public static IEntityExportProfile GetIEntityExportProfile()
        {
            return new EntityExportProfile();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityExportProfile"/> class. 
        /// Parameter-less Constructor
        /// </summary>
        public static IEntityExportProfile GetIEntityExportProfile(ExportProfileType exportProfileType)
        {
            return new EntityExportProfile(exportProfileType);
        }

        /// <summary>
        /// Get the entity export profile based on export profile type and xml
        /// </summary>
        /// <param name="exportProfileType">Indicates type of export profile</param>
        /// <param name="valuesAsXml">Indicates values as xml</param>
        /// <returns>Returns entity export profile based on export profile type and xml</returns>
        public static IEntityExportProfile GetIEntityExportProfile(ExportProfileType exportProfileType, String valuesAsXml)
        {
            return new EntityExportProfile(exportProfileType, valuesAsXml);
        }

        #endregion

        #region Get IExportProfileContext

        /// <summary>
        /// Get IExportProfileContext object
        /// </summary>
        /// <returns>ExportProfileContext object</returns>
        public static IExportProfileContext GetIExportProfileContext()
        {
            return new ExportProfileContext();
        }

        #endregion

        #region Get IEntityExportProfileCollection

        /// <summary>
        /// Get IEntityExportProfileCollection object
        /// </summary>
        /// <returns>EntityExportProfileCollection object</returns>
        public static IEntityExportProfileCollection GetIEntityExportProfileCollection()
        {
            return new EntityExportProfileCollection();
        }

        #endregion

        #region Get IRole

        /// <summary>
        /// Initialize ISecurityRole
        /// </summary>
        /// <returns>ISecurityRole interface</returns>
        public static ISecurityRole GetISecurityRole()
        {
            return (ISecurityRole)new SecurityRole();
        }

        /// <summary>
        /// Initialize ISecurityRole
        /// </summary>
        /// <param name="id">Id of ISecurityRole</param>
        /// <returns>ISecurityRole interface</returns>
        public static ISecurityRole GetISecurityRole(Int32 id)
        {
            return (ISecurityRole)new SecurityRole(id);
        }

        /// <summary>
        /// Initialize ISecurityRole
        /// </summary>
        /// <param name="id">Id of ISecurityRole</param>
        /// <param name="name">Name of role</param>
        /// <param name="longName">Long name of role</param>
        /// <returns>ISecurityRole interface</returns>
        public static ISecurityRole GetISecurityRole(Int32 id, String name, String longName)
        {
            return (ISecurityRole)new SecurityRole(id, name, longName);
        }

        /// <summary>
        /// Initialize ISecurityRole
        /// </summary>
        /// <param name="valuesAsXml">Xml format of ISecurityRole</param>
        /// <returns>Initialize ISecurityRole</returns>
        public static ISecurityRole GetISecurityRole(String valuesAsXml)
        {
            return (ISecurityRole)new SecurityRole(valuesAsXml);
        }

        #endregion Get IRole

        #region Get IRoleCollection

        /// <summary>
        /// Initialize ISecurityRoleCollection
        /// </summary>
        /// <returns>ISecurityRoleCollection interface</returns>
        public static ISecurityRoleCollection GetISecurityRoleCollection()
        {
            return (ISecurityRoleCollection)new SecurityRoleCollection();
        }

        /// <summary>
        /// Initialize ISecurityRoleCollection
        /// </summary>
        /// <param name="valueAsXml">Initialize interface from Xml</param>
        /// <returns>ISecurityRoleCollection interface</returns>
        public static ISecurityRoleCollection GetISecurityRoleCollection(String valueAsXml)
        {
            return (ISecurityRoleCollection)new SecurityRoleCollection(valueAsXml);
        }

        #endregion Get IRoleCollection

        #region Get IRelationshipType

        /// <summary>
        /// Initialize IRelationshipType
        /// </summary>
        /// <returns>IRelationshipType interface</returns>
        public static IRelationshipType GetIRelationshipType()
        {
            return (IRelationshipType)new RelationshipType();
        }

        /// <summary>
        /// Initialize IRelationshipType
        /// </summary>
        /// <param name="id">Id of IRelationshipType</param>
        /// <returns>IRelationshipType interface</returns>
        public static IRelationshipType GetIRelationshipType(Int32 id)
        {
            return (IRelationshipType)new RelationshipType(id);
        }

        /// <summary>
        /// Initialize IRelationshipType
        /// </summary>
        /// <param name="id">Id of IRelationshipType</param>
        /// <param name="name">Name of relationshipType</param>
        /// <param name="longName">Long name of relationshipType</param>
        /// <returns>IRelationshipType interface</returns>
        public static IRelationshipType GetIRelationshipType(Int32 id, String name, String longName)
        {
            return (IRelationshipType)new RelationshipType(id, name, longName);
        }

        /// <summary>
        /// Initialize IRelationshipType
        /// </summary>
        /// <param name="valuesAsXml">Xml format of IRelationshipType</param>
        /// <returns>Initialize IRelationshipType</returns>
        public static IRelationshipType GetIRelationshipType(String valuesAsXml)
        {
            return (IRelationshipType)new RelationshipType(valuesAsXml);
        }

        #endregion Get IRelationshipType

        #region Get IRelationshipTypeCollection

        /// <summary>
        /// Initialize IRelationshipTypeCollection
        /// </summary>
        /// <returns>IRelationshipTypeCollection interface</returns>
        public static IRelationshipTypeCollection GetIRelationshipTypeCollection()
        {
            return (IRelationshipTypeCollection)new RelationshipTypeCollection();
        }

        /// <summary>
        /// Initialize IRelationshipTypeCollection
        /// </summary>
        /// <param name="valueAsXml">Initialize interface from Xml</param>
        /// <returns>IRelationshipTypeCollection interface</returns>
        public static IRelationshipTypeCollection GetIRelationshipTypeCollection(String valueAsXml)
        {
            return (IRelationshipTypeCollection)new RelationshipTypeCollection(valueAsXml);
        }

        #endregion Get IRelationshipTypeCollection

        #region Get IJobSchedule

        /// <summary>
        /// Initialize IJobSchedule
        /// </summary>
        /// <returns>IJobSchedule interface</returns>
        public static IJobSchedule GetIJobSchedule()
        {
            return (IJobSchedule)new JobSchedule();
        }

        /// <summary>
        /// Initialize IJobSchedule
        /// </summary>
        /// <param name="id">Id of IJobSchedule</param>
        /// <returns>IJobSchedule interface</returns>
        public static IJobSchedule GetIJobSchedule(Int32 id)
        {
            return (IJobSchedule)new JobSchedule(id);
        }

        /// <summary>
        /// Initialize IJobSchedule
        /// </summary>
        /// <param name="valuesAsXml">Xml format of IJobSchedule</param>
        /// <returns>Initialize IJobSchedule</returns>
        public static IJobSchedule GetIJobSchedule(String valuesAsXml)
        {
            return (IJobSchedule)new JobSchedule(valuesAsXml);
        }

        #endregion Get IJobSchedule

        #region Get IJobScheduleCollection

        /// <summary>
        /// Initialize IJobScheduleCollection
        /// </summary>
        /// <returns>IJobScheduleCollection interface</returns>
        public static IJobScheduleCollection GetIJobScheduleCollection()
        {
            return (IJobScheduleCollection)new JobScheduleCollection();
        }

        /// <summary>
        /// Initialize IJobScheduleCollection
        /// </summary>
        /// <param name="valueAsXml">Initialize interface from Xml</param>
        /// <returns>IJobScheduleCollection interface</returns>
        public static IJobScheduleCollection GetIJobScheduleCollection(String valueAsXml)
        {
            return (IJobScheduleCollection)new JobScheduleCollection(valueAsXml);
        }

        #endregion Get IJobScheduleCollection

        #region Get IEntityType

        /// <summary>
        /// Initialize IEntityType
        /// </summary>
        /// <returns>Implementation of IEntityType</returns>
        public static IEntityType GetIEntityType()
        {
            return new EntityType();
        }

        #endregion Get IEntityType

        #region Get IEntityTypeCollectino

        /// <summary>
        /// Initialize IEntityTypeCollection
        /// </summary>
        /// <returns>Implementation of IEntityTypeCollection</returns>
        public static IEntityTypeCollection GetIEntityTypeCollection()
        {
            return new EntityTypeCollection();
        }

        #endregion Get IEntityTypeCollectino

        #region Get IOrganization

        /// <summary>
        /// Initialize IOrganization
        /// </summary>
        /// <returns>IOrganization interface</returns>
        public static IOrganization GetIOrganization()
        {
            return new Organization();
        }

        #endregion

        #region Get IOrganizationCollection

        /// <summary>
        /// Initialize IOrganizationCollection
        /// </summary>
        /// <returns>Implementation of IOrganizationCollection</returns>
        public static IOrganizationCollection GetIOrganizationCollection()
        {
            return new OrganizationCollection();
        }

        #endregion

        #region Get IHierarchy

        /// <summary>
        /// Initialize IHierarchy
        /// </summary>
        /// <returns>Implementation of IHierarchy</returns>
        public static IHierarchy GetIHierarchy()
        {
            return new Hierarchy();
        }

        /// <summary>
        /// Initialize IHierarchyCollection
        /// </summary>
        /// <returns>Implementation of IHierarchyCollection</returns>
        public static IHierarchyCollection GetIHierarchyCollection()
        {
            return new HierarchyCollection();
        }

        #endregion Get IHierarchy

        #region Get IError

        /// <summary>
        /// Get IError instance
        /// </summary>
        /// <returns>IError instance</returns>
        public static IError GetIError()
        {
            return (IError)new Error();
        }

        /// <summary>
        /// Get IError instance
        /// </summary>
        /// <param name="errorCode">errorCode</param>
        /// <param name="errorMessage">errorMessage</param>
        /// <param name="parameters">parameters</param>
        /// <returns>IError instance</returns>
        public static IError GetIError(String errorCode, String errorMessage, Collection<Object> parameters)
        {
            return (IError)new Error(errorCode, errorMessage, parameters);
        }

        /// <summary>
        /// Get IError instance
        /// </summary>
        /// <param name="errorCode">errorCode</param>
        /// <param name="parameters">parameters</param>
        /// <returns>IError instance</returns>
        public static IError GetIError(String errorCode, Collection<Object> parameters)
        {
            return (IError)new Error(errorCode, parameters);
        }

        /// <summary>
        /// Get IError instance
        /// </summary>
        /// <param name="errorCode">errorCode</param>
        /// <param name="errorMessage">errorMessage</param>
        /// <returns>IError instance</returns>
        public static IError GetIError(String errorCode, String errorMessage)
        {
            return (IError)new Error(errorCode, errorMessage);
        }

        /// <summary>
        /// Get IError instance
        /// </summary>
        /// <param name="valuesAsXml">xml of IError</param>
        /// <returns>IError instance</returns>
        public static IError GetIError(String valuesAsXml)
        {
            return (IError)new Error(valuesAsXml);
        }

        #endregion Get IError

        #region Get IErrorCollection

        /// <summary>
        /// Get IErrorCollection instance
        /// </summary>
        /// <returns>IErrorCollection  instance</returns>
        public static IErrorCollection GetIErrorCollection()
        {
            return (IErrorCollection)new ErrorCollection();
        }

        /// <summary>
        /// Get IErrorCollection instance
        /// </summary>
        /// <param name="error">error instance</param>
        /// <returns>IErrorCollection  instance</returns>
        public static IErrorCollection GetIErrorCollection(IError error)
        {
            return (IErrorCollection)new ErrorCollection(error as Error);
        }

        /// <summary>
        /// Get IErrorCollection instance
        /// </summary>
        /// <param name="valuesAsXml">xml of IErrorCollection</param>
        /// <returns>IErrorCollection  instance</returns>
        public static IErrorCollection GetIErrorCollection(String valuesAsXml)
        {
            return (IErrorCollection)new ErrorCollection(valuesAsXml);
        }

        #endregion Get IErrorCollection

        #region Get IJob

        /// <summary>
        /// Initialize IJob
        /// </summary>
        /// <returns>Implementation of IJob</returns>
        public static IJob GetIJob()
        {
            return new Job();
        }

        /// <summary>
        /// Initialize IJobCollection
        /// </summary>
        /// <returns>Implementation of IJobCollection</returns>
        public static IJobCollection GetIJobCollection()
        {
            return new JobCollection();
        }

        #endregion

        #region Get IMDMObjectInfo

        /// <summary>
        /// Initialize IMDMObjectInfo
        /// </summary>
        /// <returns>Implementation of IMDMObjectInfo</returns>
        public static IMDMObjectInfo GetIMDMObjectInfo()
        {
            return new MDMObjectInfo();
        }

        #endregion

        #region Get IMDMObjectInfoCollection

        /// <summary>
        /// Initialize IMDMObjectInfoCollection
        /// </summary>
        /// <returns>Implementation of IMDMObjectInfoCollection</returns>
        public static IMDMObjectInfoCollection GetIMDMObjectInfoCollection()
        {
            return new MDMObjectInfoCollection();
        }

        #endregion

        #region Get IPasswordResetRequest

        /// <summary>
        /// Initialize IPasswordResetRequest
        /// </summary>
        /// <returns>Implementation of IPasswordResetRequest</returns>
        public static IPasswordResetRequest GetIPasswordResetRequest()
        {
            return new PasswordResetRequest();
        }

        /// <summary>
        /// Initialize IPasswordResetRequest
        /// </summary>
        /// <param name="valueAsXml">xml of IPasswordResetRequest</param>
        /// <returns>Implementation of IPasswordResetRequest</returns>
        public static IPasswordResetRequest GetIPasswordResetRequest(String valueAsXml)
        {
            return new PasswordResetRequest(valueAsXml);
        }

        #endregion

        #region Get IUserCredentialRequestContext

        /// <summary>
        /// Initialize IUserCredentialRequestContext
        /// </summary>
        /// <returns>Implementation of IUserCredentialRequestContext</returns>
        public static IUserCredentialRequestContext GetIUserCredentialRequestContext()
        {
            return new UserCredentialRequestContext();
        }

        #endregion

        #region Get IIntegrationActivityLog

        /// <summary>
        /// Initialize integration activity log
        /// </summary>
        /// <returns>Returns new instance of integration activity log</returns>
        public static IIntegrationActivityLog GetIIntegrationActivityLog()
        {
            return new IntegrationActivityLog();
        }

        /// <summary>
        /// Initialize integration activity log from xml value
        /// </summary>
        /// <param name="valuesAsXml">Indicates integration activity log value as xml</param>
        /// <returns>Returns new instance of integration activity log from its xml</returns>
        public static IIntegrationActivityLog GetIIntegrationActivityLog(String valuesAsXml)
        {
            return new IntegrationActivityLog(valuesAsXml);
        }

        /// <summary>
        /// Initialize integration activity log collection from xml value
        /// </summary>
        /// <param name="valuesAsXml">Indicates integration activity log collection value as xml</param>
        /// <returns>Returns new instance of integration activity log collection from its xml</returns>
        public static IIntegrationActivityLogCollection GetIIntegrationActivityLogCollection(String valuesAsXml)
        {
            return new IntegrationActivityLogCollection(valuesAsXml);
        }

        #endregion Get IIntegrationActivityLog

        #region Get IIntegrationMessage

        /// <summary>
        /// Initialize integration message
        /// </summary>
        /// <returns>Returns new instance of integration message</returns>
        public static IIntegrationMessage GetIIntegrationMessage()
        {
            return new IntegrationMessage();
        }

        /// <summary>
        /// Initialize integration message collection
        /// </summary>
        /// <returns>Returns new instance of integration message collection</returns>
        public static IIntegrationMessageCollection GetIIntegrationMessageCollection()
        {
            return new IntegrationMessageCollection();
        }

        #endregion Get IIntegrationMessage

        #region Get IConnectorProfile

       /// <summary>
       /// Initialize connector profile
       /// </summary>
       /// <returns>Returns new instance of connector profile</returns>
        public static IConnectorProfile GetIConnectorProfile()
        {
            return new ConnectorProfile();
        }

        /// <summary>
        /// Initialize connector profile using xml
        /// </summary>
        /// <param name="valuesAsXml">Indicates connector profile in xml format</param>
        /// <returns>Returns new instance of connector profile using its xml</returns>
        public static IConnectorProfile GetIConnectorProfile(String valuesAsXml)
        {
            return new ConnectorProfile(valuesAsXml);
        }

        #endregion Get IConnectorProfile

        #region Get IMessageQualificationResult

        /// <summary>
        /// Initialize message qualification result
        /// </summary>
        /// <returns>Returns message qualification result</returns>
        public static IMessageQualificationResult GetIMessageQualificationResult()
        {
            return new MessageQualificationResult();
        }

        #endregion Get IMessageQualificationResult

        #region Get IIntegrationItemStatus / IIntegrationItemStatusCollection

        /// <summary>
        /// Initialize IntegrationItemStatus object
        /// </summary>
        /// <returns><see cref="IIntegrationItemStatus"/></returns>
        public static IIntegrationItemStatus GetIIntegrationItemStatus()
        {
            return new IntegrationItemStatus();
        }

        /// <summary>
        /// Initialize IntegrationItemStatusCollection object
        /// </summary>
        /// <returns><see cref="IIntegrationItemStatus"/></returns>
        public static IIntegrationItemStatusCollection GetIIntegrationItemStatusCollection()
        {
            return new IntegrationItemStatusCollection();
        }

        /// <summary>
        /// Initialize IntegrationItemStatusDimension object
        /// </summary>
        /// <returns><see cref="IIntegrationItemStatusDimension"/></returns>
        public static IIntegrationItemStatusDimension GetIIntegrationItemStatusDimension()
        {
            return new IntegrationItemStatusDimension();
        }

        /// <summary>
        /// Initialize IntegrationItemStatusDimensionCollection object
        /// </summary>
        /// <returns><see cref="IntegrationItemStatusDimensionCollection"/></returns>
        public static IIntegrationItemStatusDimensionCollection GetIIntegrationItemStatusDimensionCollection()
        {
            return new IntegrationItemStatusDimensionCollection();
        }

        #endregion Get IIntegrationItemStatus / IIntegrationItemStatusCollection

        #region Get IIntegrationItemStatusSearchCriteria

        /// <summary>
        /// Initialize IntegrationItemStatusSearchCriteria object
        /// </summary>
        /// <returns><see cref="IIntegrationItemStatusSearchCriteria"/></returns>
        public static IIntegrationItemStatusSearchCriteria GetIIntegrationItemStatusSearchCriteria()
        {
            return new IntegrationItemStatusSearchCriteria();
        }


        #endregion Get IIntegrationItemStatusSearchCriteria

        #region Get IEntityCopyPasteContext

        /// <summary>
        /// Initialize EntityCopyPasteContext object
        /// </summary>
        /// <returns><see cref="IEntityCopyPasteContext"/></returns>
        public static IEntityCopyPasteContext GetIEntityCopyPasteContext()
        {
            return new EntityCopyPasteContext();
        }

        #endregion

        #region Lookup Export Profile objects

        /// <summary>
        /// Initializes LookupExportProfile instance
        /// </summary>
        /// <returns>LookupExportProfile instance</returns>
        public static ILookupExportProfile GetLookupExportProfile()
        {
            return new LookupExportProfile();
        }

        /// <summary>
        /// Initializes LookupExportScope instance
        /// </summary>
        /// <returns>LookupExportScope instance</returns>
        public static ILookupExportScope GetLookupExportScope()
        {
            return new LookupExportScope();
        }

        /// <summary>
        /// Initializes LookupExportScopeCollection instance
        /// </summary>
        /// <returns>LookupExportScopeCollection instance</returns>
        public static ILookupExportScopeCollection GetLookupExportScopeCollection()
        {
            return new LookupExportScopeCollection();
        }

        /// <summary>
        /// Initializes LookupExportSyndicationProfileData instance
        /// </summary>
        /// <returns>LookupExportSyndicationProfileData instance</returns>
        public static ILookupExportSyndicationProfileData GetLookupExportSyndicationProfileData()
        {
            return new LookupExportSyndicationProfileData();
        }

        #endregion

        #region Get ICategoryContext

        /// <summary>
        /// Initialize CategoryContext object
        /// </summary>
        /// <returns><see cref="IEntityCopyPasteContext"/></returns>
        public static ICategoryContext GetICategoryContext()
        {
            return new CategoryContext();
        }

        #endregion

        #region Get ICategorySearchRules

        /// <summary>
        /// Get ICategorySearchRule instance
        /// </summary>
        /// <returns>ICategorySearchRule</returns>
        public static ICategorySearchRule GetICategorySearchRule()
        {
            return new CategorySearchRule();
        }

        /// <summary>
        /// Get ICategorySearchRuleCollection instance
        /// </summary>
        /// <returns>ICategorySearchRuleCollection</returns>
        public static ICategorySearchRuleCollection GetICategorySearchRuleCollection()
        {
            return new CategorySearchRuleCollection();
        }

        #endregion

        #region Get IPagingCriteria

        /// <summary>
        /// Get IPagingCriteria instance
        /// </summary>
        /// <returns>IPagingCriteria instance</returns>
        public static IPagingCriteria GetIPagingCriteria()
        {
            PagingCriteria pagingCriteria = new PagingCriteria();
            pagingCriteria.FilterCriteriaList = new FilterCriteriaCollection();
            pagingCriteria.GroupByParameters = new Collection<String>();
            pagingCriteria.SortParameters = new SortCriteriaCollection();
            return pagingCriteria;
        }

        #endregion Get IPagingCriteria

        #region Get ISortCriteria

        /// <summary>
        /// Get ISortCriteria instance
        /// </summary>
        /// <param name="sortParamter">fieldname on which records need to be sorted</param>
        /// <param name="isDescendingOrder">Indicates whether it is an ascending or descending order</param>
        /// <returns>ISortCriteria instance</returns>
        public static ISortCriteria GetISortCriteria(String sortParamter, bool isDescendingOrder)
        {
            return new SortCriteria { SortParameter = sortParamter, IsDescendingOrder = isDescendingOrder };
        }

        #endregion Get ISortCriteria

        #region Get IFilterCriteria

        /// <summary>
        /// Get IFilterCriteria instance
        /// </summary>
        /// <param name="filteredField">Name of the field om which filtering is needed</param>
        /// <param name="filteredFieldValue">Filter value for the field</param>
        /// <returns>IFilterCriteria instance</returns>
        public static IFilterCriteria GetIFilterCriteria(String filteredField, String filteredFieldValue)
        {
            return new FilterCriteria { FilteredField = filteredField, FilteredFieldValue = filteredFieldValue };
        }

        #endregion Get IFilterCriteria

        #region Get IUomContext

        /// <summary>
        /// Get IUomContext instance
        /// </summary>
        /// <returns></returns>
        public static IUomContext GetIUomContext()
        {
            return new UomContext();
        }

        #endregion

        #region Get ITranslationExportProfile

        /// <summary>
        /// Initialize translation export profile
        /// </summary>
        /// <returns>Returns new instance of translation export profile</returns>
        public static ITranslationExportProfile GetITranslationExportProfile()
        {
            return new TranslationExportProfile();
        }

        #endregion

        #region Get ITranslationExportProfileData

        /// <summary>
        /// Initialize translation export profile data
        /// </summary>
        /// <returns>Returns new instance of translation export profile data</returns>
        public static ITranslationExportProfileData GetITranslationExportProfileData()
        {
            return new TranslationExportProfileData();
        }

        #endregion

        #region Get Workflow Escalation context and data

        /// <summary>
        /// Initialize the workflow escalation object
        /// </summary>
        /// <returns>Returns the instance of workflow escalation object</returns>
        public static IWorkflowEscalationData GetIWorkflowEscalationData()
        {
            return new WorkflowEscalationData();
        }

        /// <summary>
        /// Initialize the workflow escalation context object
        /// </summary>
        /// <returns>Returns the instance of workflow escalation context object</returns>
        public static IWorkflowEscalationContext GetIWorkflowEscalationContext()
        {
            return new WorkflowEscalationContext();
        }

        /// <summary>
        /// Initialize the workflow escalation collection object
        /// </summary>
        /// <returns>Returns the instance of workflow escalation collection object</returns>
        public static IWorkflowEscalationDataCollection GetIWorkflowEscalationDataCollection()
        {
            return new WorkflowEscalationDataCollection();
        }
        #endregion 

        #region Lookup Metdadata

        #region Get Row & Cell

        /// <summary>
        /// Get IRowCollection interface
        /// </summary>
        /// <returns>IRowCollection interface </returns>
        public static IRowCollection GetIRowCollection()
        {
            return (IRowCollection)new RowCollection();
        }

        /// <summary>
        /// Gets the Row object
        /// </summary>
        /// <returns>Returns the Row Interface</returns>
        public static IRow GetIRow()
        {
            return new Row();
        }

        /// <summary>
        /// Gets the Cell object
        /// </summary>
        /// <returns>Returns the Cell Interface</returns>
        public static ICell GetICell()
        {
            return new Cell();
        }

        /// <summary>
        /// Gets the Cell object
        /// </summary>
        /// <param name="columnName">Indicates the column name</param>
        /// <returns>Returns the Cell Interface</returns>
        public static ICell GetICell(String columnName)
        {
            return new Cell() { ColumnName = columnName};
        }

        /// <summary>
        /// Gets the Cell collection object
        /// </summary>
        /// <returns>Returns the Cell collection Interface</returns>
        public static ICellCollection GetICellCollection()
        {
            return new CellCollection();
        }

        #endregion Get Row & Cell

        #region Get ILookup

        /// <summary>
        /// Get ILookup interface
        /// </summary>
        /// <returns>ILookup interface </returns>
        public static ILookup GetILookup()
        {
            return (ILookup)new Lookup();
        }

        #endregion Get ILookup

        #endregion

        #region Get LookupSearchRule

        /// <summary>
        /// Get ILookupSearchRule interface
        /// </summary>
        /// <returns>ILookupSearchRule interface </returns>
        public static ILookupSearchRule GetILookupSearchRule()
        {
            return (ILookupSearchRule)new LookupSearchRule();
        }

        /// <summary>
        /// Get ILookupSearchRuleCollection interface
        /// </summary>
        /// <returns>ILookupSearchRuleCollection interface </returns>
        public static ILookupSearchRuleCollection GetILookupSearchRuleCollection()
        {
            return (ILookupSearchRuleCollection)new LookupSearchRuleCollection();
        }

        #endregion Get Lookup Search Rule Collection

        #region Get Email Context

        /// <summary>
        /// Gets the Email Context object
        /// </summary>
        /// <returns></returns>
        public static IEmailContext GetIEmailContext()
        {
            return new EmailContext();
        }
        #endregion 

        #region Get IUniqueIdContext

        /// <summary>
        /// Initializes UniqueIdGenerationContext instance
        /// </summary>        
        /// <returns>UniqueIdContext instance</returns>
        public static IUniqueIdGenerationContext GetIUniqueIdGenerationContext()
        {
            return new UniqueIdGenerationContext();
        }
        
        #endregion

        #region Get WorkflowBusinessRuleContext

        /// <summary>
        /// Get IWorkflowBusinessRuleContext interface
        /// </summary>
        /// <returns>IWorkflowBusinessRuleContext interface </returns>
        public static IWorkflowBusinessRuleContext GetIWorkflowBusinessRuleContext(IWorkflowDataContext iWorkFlowDataContext)
        {
            return (IWorkflowBusinessRuleContext)new WorkflowBusinessRuleContext((WorkflowDataContext)iWorkFlowDataContext);
        }

        #endregion Get WorkflowBusinessRuleContext

        #region Get WorkflowDataContext

        /// <summary>
        /// Get IWorkflowDataContext interface
        /// </summary>
        /// <returns>IWorkflowDataContext interface </returns>
        public static IWorkflowDataContext GetWorkflowDataContext()
        {
            return (IWorkflowDataContext)new WorkflowDataContext();
        }

        #endregion Get WorkflowDataContext

        #region Get MDMEventInfo

        /// <summary>
        /// Gets the MDMEventInfo object
        /// </summary>
        /// <returns>Returns the MDMEventInfo Interface</returns>
        public static IMDMEventInfo GetIMDMEventInfo()
        {
            return new MDMEventInfo();
        }

        /// <summary>
        /// Gets the MDMEventInfo collection object
        /// </summary>
        /// <returns>Returns the MDMEventInfoCollection Interface</returns>
        public static IMDMEventInfoCollection GetIMDMEventInfoCollection()
        {
            return new MDMEventInfoCollection();
        }

        #endregion Get MDMEventInfo

        #region Get MDMEventHandler

        /// <summary>
        /// Gets the MDMEventHandler object
        /// </summary>
        /// <returns>Returns the MDMEventHandler Interface</returns>
        public static IMDMEventHandler GetIMDMEventHandler()
        {
            return new MDMEventHandler();
        }

        /// <summary>
        /// Gets the MDMEventHandlerCollection object
        /// </summary>
        /// <returns>Returns the MDMEventHandlerCollection Interface</returns>
        public static IMDMEventHandlerCollection GetIMDMEventHandlerCollection()
        {
            return new MDMEventHandlerCollection();
        }

        #endregion Get MDMEventHandler

        #region Get IEntityIdentifierMap

        /// <summary>
        /// Parameterless Constructor
        /// </summary>
        /// <returns>IEntityIdentifierMap instance</returns>
        public static IEntityUniqueIdentificationMap GetIEntityIdentifierMap()
        {
            return (IEntityUniqueIdentificationMap)new EntityUniqueIdentificationMap();
        }

        /// <summary>
        /// Instantiates the new instance of Entity Map
        /// </summary>
        /// <param name="id">EntityIdentifierMap Id</param>
        /// <param name="externalId">Entity Id in the external system</param>
        /// <param name="containerId">Container Id</param>
        /// <param name="categoryId">Category Id</param>
        /// <param name="entityTypeId">Entity Type Id</param>
        /// <param name="entityIdentifier1Value">Indicates entityIdentifier1Value</param>
        /// <param name="entityIdentifier2Value">Indicates entityIdentifier2Value</param>
        /// <param name="entityIdentifier3Value">Indicates entityIdentifier3Value</param>
        /// <param name="entityIdentifier4Value">Indicates entityIdentifier4Value</param>
        /// <param name="entityIdentifier5Value">Indicates entityIdentifier5Value</param>
        /// <returns>EntityIdentifierMap Instance</returns>
        public static IEntityUniqueIdentificationMap GetIEntityIdentifierMap(Int64 id, String externalId, Int32 containerId, Int64 categoryId, Int32 entityTypeId, String entityIdentifier1Value, String entityIdentifier2Value, String entityIdentifier3Value, String entityIdentifier4Value, String entityIdentifier5Value)            
        {
            return (IEntityUniqueIdentificationMap)new EntityUniqueIdentificationMap(id, externalId, containerId, categoryId, entityTypeId,entityIdentifier1Value,entityIdentifier2Value,entityIdentifier3Value,entityIdentifier4Value,entityIdentifier5Value);
        }

        #endregion

        #region Get IEntityIdentifierMapCollection

        /// <summary>
        /// Parameterless Constructor
        /// </summary>
        /// <returns>Instance of IEntityIdentifierMapCollection</returns>
        public static IEntityUniqueIdentificationMapCollection GetIEntityIdentifierMapCollection()
        {
            return (IEntityUniqueIdentificationMapCollection)new EntityUniqueIdentificationMapCollection();
        }

        #endregion

        #region Get IHierarchyCopyPasteContext

        /// <summary>
        /// Parameterless constructor
        /// </summary>
        /// <returns>Instance of HierarchyCopyPasteContext</returns>
        public static IHierarchyCopyPasteContext GetIHierarchyCopyPasteContext()
        {
            return new HierarchyCopyPasteContext();
        }

        /// <summary>
        /// Initialize HierarchyCopyPasteContext
        /// </summary>
        /// <param name="sourceHierarchyId">Id of source hierarchy</param>
        /// <param name="targetHierarchyId">Id of target hierarchy</param>
        /// <returns>Instance of HierarchyCopyPasteContext</returns>
        public static IHierarchyCopyPasteContext GetIHierarchyCopyPasteContext(Int32 sourceHierarchyId, Int32 targetHierarchyId)
        {
            return new HierarchyCopyPasteContext(sourceHierarchyId, targetHierarchyId);
        }

        /// <summary>
        /// Initialize HierarchyCopyPasteContext from Xml
        /// </summary>
        /// <param name="valuesAsXml">
        /// Xml having values
        /// <para> Sample:</para>
        /// <![CDATA[<HierarchyCopyPasteContext SourceHierarchyId="62" TargetHierarchyId="78" Locale="en_WW"/>]]>
        /// </param>
        /// <returns>Instance of HierarchyCopyPasteContext</returns>
        public static IHierarchyCopyPasteContext GetIHierarchyCopyPasteContext(String valuesAsXml)
        {
            return new HierarchyCopyPasteContext(valuesAsXml);
        }

        #endregion Get IHierarchyCopyPasteContext

        #region Get IHierachyCategoryMapping

        /// <summary>
        /// Parametrized Constructor
        /// </summary>
        /// <param name="hierarchyId">Hierarchy Id to be mapped</param>
        /// <param name="categoryIds">Collection of CategoryId's</param>
        /// <returns>IHierachyCategoryMapping instance</returns>
        public static IHierachyCategoryMapping GetIHierachyCategoryMapping(Int32 hierarchyId, Collection<Int64> categoryIds)
        {
            return (IHierachyCategoryMapping)new HierachyCategoryMapping(hierarchyId, categoryIds);
        }

        #endregion

        #region Get IHierachyCategoryMappingCollection

        /// <summary>
        /// Parameterless Constructor
        /// </summary>
        /// <returns>IHierachyCategoryMappingCollection instance</returns>
        public static IHierachyCategoryMappingCollection GetIHierachyCategoryMappingCollection()
        {
            return (IHierachyCategoryMappingCollection)new HierachyCategoryMappingCollection();
        }

        #endregion

        #region Get IDataModelExportProfile

        /// <summary>
        /// Get IDataModelExportProfileobject
        /// </summary>
        /// <returns>DataModelExportProfile object</returns>
        public static IDataModelExportProfile GetIDataModelExportProfile()
        {
            return new DataModelExportProfile();
        }

        #endregion Get IDataModelExportProfile

        #region Get GetIEntityVariantDefinition

        /// <summary>
        /// Parameterless Constructor
        /// </summary>
        /// <returns>GetIEntityVariantDefinition instance</returns>
        public static IEntityVariantDefinition GetIEntityVariantDefinition()
        {
            return (IEntityVariantDefinition)new EntityVariantDefinition();
        }

        #endregion

        #region Get IEntityVariantDefinitionMapping

        /// <summary>
        /// Parameterless Constructor
        /// </summary>
        /// <returns>IEntityVariantDefinitionMapping instance</returns>
        public static IEntityVariantDefinitionMapping GetIEntityVariantDefinitionMapping()
        {
            return (IEntityVariantDefinitionMapping)new EntityVariantDefinitionMapping();
        }

        #endregion

        #region Get IEntityVariantDefinitionMappingCollection

        /// <summary>
        /// Parameterless Constructor
        /// </summary>
        /// <returns>IEntityVariantDefinitionMappingCollection instance</returns>
        public static IEntityVariantDefinitionMappingCollection GetIEntityVariantDefinitionMappingCollection()
        {
            return (IEntityVariantDefinitionMappingCollection)new EntityVariantDefinitionMappingCollection();
        }

        #endregion
    }
}