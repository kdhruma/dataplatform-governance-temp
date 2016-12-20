using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Collections.ObjectModel;

namespace MDM.WCFServiceInterfaces
{
    using MDM.Core;
    using MDM.BusinessObjects;
    using MDM.BusinessObjects.DynamicTableSchema;
    using MDM.Interfaces;

    /// <summary>
    /// Defines operation contracts for MDM Data Model operations
    /// </summary>
    [ServiceContract(Namespace = "http://wcfservices.riversand.com")]
    public interface IDataModelService
    {
        #region Attribute Model Contracts

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        AttributeModelCollection GetAttributeModelsByContext(AttributeModelContext attributeModelContext, CallerContext callerContext);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        AttributeModelCollection GetAllBaseAttributeModels();

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        AttributeModelCollection GetBaseAttributeModelsByIds(Collection<Int32> attributeIds, CallerContext callerContext);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        AttributeModelCollection GetAttributeModel(Int32 attributeId, AttributeModelContext attributeModelContext, CallerContext callerContext);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        AttributeModelCollection GetAttributeModelsByIds(Collection<Int32> attributeIds, AttributeModelContext attributeModelContext);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        AttributeModelCollection GetAttributeModelsByGroupIds(Collection<Int32> attributeGroupIds, Collection<Int32> excludeAttributeIds, AttributeModelContext attributeModelContext);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        AttributeModelCollection GetAttributeModelsByCustomViewId(Int32 customViewId, Collection<Int32> excludeAttributeIds, AttributeModelContext attributeModelContext);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        AttributeModelCollection GetAttributeModelsByStateViewId(Int32 stateViewId, Collection<Int32> excludeAttributeIds, AttributeModelContext attributeModelContext);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        AttributeModelCollection GetAttributeModels(Collection<Int32> attributeIds, Collection<Int32> attributeGroupIds, Int32 customViewId, Int32 stateViewId, Collection<Int32> excludeAttributeIds, AttributeModelContext attributeModelContext);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        AttributeOperationResultCollection ProcessAttributeModels(AttributeModelCollection attributeModelCollection, String programName, CallerContext callerContext);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        OperationResultCollection ProcessVariantDefinitions(EntityVariantDefinitionCollection entityVariantDefinitions, CallerContext callerContext);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        EntityVariantDefinition GetVariantDefinitionByContext(Int32 containerId, Int64 categoryId, Int32 entityTypeId, CallerContext callerContext);

        [OperationContract]
        [FaultContract(typeof (MDMExceptionDetails))]
        EntityVariantDefinition GetVariantDefinitionById(Int32 entityVariantDefinitionId, CallerContext callerContext);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        EntityVariantDefinitionCollection GetAllVariantDefinitions(CallerContext callerContext);

        [OperationContract(Name = "GetAttributeModelByUniqueIdentifierWithLocale")]
        [FaultContract(typeof(MDMExceptionDetails))]
        AttributeModel GetAttributeModelByUniqueIdentifier(AttributeUniqueIdentifier attributeUniqueIdentifier, LocaleEnum locale, CallerContext callerContext);

        [OperationContract(Name = "GetAttributeModelsByUniqueIdentifierWithLocales")]
        [FaultContract(typeof(MDMExceptionDetails))]
        AttributeModelCollection GetAttributeModelsByUniqueIdentifier(AttributeUniqueIdentifier attributeUniqueIdentifier, Collection<LocaleEnum> locales, CallerContext callerContext);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        AttributeModel GetAttributeModelByUniqueIdentifier(AttributeUniqueIdentifier attributeUniqueIdentifier, CallerContext callerContext);

        [OperationContract(Name = "GetAttributeModelsByUniqueIdentifiersWithLocale")]
        [FaultContract(typeof(MDMExceptionDetails))]
        AttributeModelCollection GetAttributeModelsByUniqueIdentifiers(AttributeUniqueIdentifierCollection attributeUniqueIdentifiers, LocaleEnum locale, CallerContext callerContext);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        AttributeModelCollection GetAttributeModelsByUniqueIdentifiers(AttributeUniqueIdentifierCollection attributeUniqueIdentifiers, CallerContext callerContext);

        [OperationContract(Name = "GetAllStateviewAttributeModels")]
        [FaultContract(typeof(MDMExceptionDetails))]
        AttributeModelCollection GetAllStateviewAttributeModels(Collection<LocaleEnum> locales, CallerContext callerContext);

        #endregion

        #region Container Contracts

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        OperationResult CreateContainer(Container container, String programName, CallerContext callerContext);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        OperationResult UpdateContainer(Container container, String programName, CallerContext callerContext);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        OperationResult DeleteContainer(Container container, String programName, CallerContext callerContext);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        OperationResult ProcessContainer(ContainerCollection containerCollection, CallerContext callerContext);
        
        [OperationContract(Name = "GetAllContainersWithoutContext")]
        [FaultContract(typeof(MDMExceptionDetails))]
        ContainerCollection GetAllContainers(CallerContext callerContext);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        Container GetContainerById(Int32 containerId, CallerContext callerContext);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        Container GetContainerByName(String containerShortName, CallerContext callerContext);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        Container GetContainerByContainerNameAndOrgName(String containerShortName, String organizationName, CallerContext callerContext);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        ContainerCollection GetAllContainers(ContainerContext containerContext, CallerContext callerContext);

        [OperationContract(Name = "GetContainerByIdBasedOnContext")]
        [FaultContract(typeof(MDMExceptionDetails))]
        Container GetContainerById(Int32 containerId, ContainerContext containerContext, CallerContext callerContext);

        [OperationContract(Name = "GetContainerByNameBasedOnContext")]
        [FaultContract(typeof(MDMExceptionDetails))]
        Container GetContainerByName(String containerShortName, ContainerContext containerContext, CallerContext callerContext);

        [OperationContract(Name = "GetContainerByContainerNameAndOrgNameBasedOnContext")]
        [FaultContract(typeof(MDMExceptionDetails))]
        Container GetContainerByContainerNameAndOrgName(String containerShortName, String organizationName, ContainerContext containerContext, CallerContext callerContext);

        [OperationContract(Name = "GetContainerCollectionByOrganization")]
        [FaultContract(typeof(MDMExceptionDetails))]
        ContainerCollection GetContainerCollectionByOrganization(Int32 orgId);

        [OperationContract(Name = "GetChildContainersByParentContainerId")]
        [FaultContract(typeof(MDMExceptionDetails))]
        ContainerCollection GetChildContainersByParentContainerId(Int32 containerId, ContainerContext containerContext, CallerContext callerContext, Boolean loadRecursive);

        [OperationContract(Name = "GetContainerHierarchyByContainerId")]
        [FaultContract(typeof(MDMExceptionDetails))]
        ContainerCollection GetContainerHierarchyByContainerId(Int32 containerId, ContainerContext containerContext, CallerContext callerContext);

        #endregion Container Contracts

        #region Dynamic Table Schema Contracts

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        OperationResult DynamicTableSchemaProcess(DBTable dbTable, DynamicTableType dynamicTableType, CallerContext callContext);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        OperationResult DynamicTableSchemaProcesses(DBTableCollection dbTables, DynamicTableType dynamicTableType, CallerContext callContext);

        #endregion

        #region Lookup Contracts

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        Boolean LoadLookupData(CallerContext callerContext);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        Lookup GetLookupModel(String lookupTableName, CallerContext callerContext);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        Lookup GetLookupData(String lookupTableName, LocaleEnum locale, Int32 maxRecordsToReturn, Boolean getLatest, CallerContext callerContext);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        RowCollection GetLookupRows(String lookupTableName, LocaleEnum locale, LookupSearchRuleCollection searchRuleCollection, Int32 maxRecordsToReturn, CallerContext callerContext);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        Lookup GetAttributeLookupData(Int32 attributeId, LocaleEnum locale, Int32 maxRecordsToReturn, CallerContext callerContext);

        [OperationContract(Name = "GetAttributeLookupDataWithApplicationContext")]
        [FaultContract(typeof(MDMExceptionDetails))]
        Lookup GetAttributeLookupData(Int32 attributeId, LocaleEnum locale, Int32 maxRecordsToReturn, ApplicationContext applicationContext, CallerContext callerContext);

        [OperationContract(Name = "GetAttributeLookupDataWithLookupValueIdList")]
        [FaultContract(typeof(MDMExceptionDetails))]
        Lookup GetAttributeLookupData(Int32 attributeId, LocaleEnum locale, Collection<Int32> lookupValueIdList, CallerContext callerContext);

        [OperationContract(Name = "GetAttributeLookupDataWithApplicationContextAndLookupValueIdList")]
        [FaultContract(typeof(MDMExceptionDetails))]
        Lookup GetAttributeLookupData(Int32 attributeId, LocaleEnum locale, Int32 maxRecordsToReturn, Collection<Int32> lookupValueIdList, ApplicationContext applicationContext, CallerContext callerContext);

        [OperationContract(Name = "GetAttributeLookupDataFilterBasedOnDependency")]
        [FaultContract(typeof(MDMExceptionDetails))]
        Lookup GetAttributeLookupData(Int32 attributeId, LocaleEnum locale, Int32 maxRecordsToReturn, Collection<Int32> lookupValueIdList, ApplicationContext applicationContext, CallerContext callerContext, Boolean isDependent, DependentAttributeCollection dependentAttributes);

        [OperationContract(Name = "GetAttributeLookupDataFilterBasedOnDependencyUsingName")]
        [FaultContract(typeof(MDMExceptionDetails))]
        Lookup GetAttributeLookupData(String attributeName, String attributeParentName, LocaleEnum locale, Int32 maxRecordsToReturn, Collection<Int32> lookupValueIdList, ApplicationContext applicationContext, CallerContext callerContext, Boolean isDependent, DependentAttributeCollection dependentAttributeCollection);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        Lookup SearchAttributeLookupData(Int32 attributeId, LocaleEnum locale, Int32 maxRecordsToReturn, String searchValue, ApplicationContext applicationContext, CallerContext callerContext);

        [OperationContract(Name = "DependentSearchAttributeLookupData")]
        [FaultContract(typeof(MDMExceptionDetails))]
        Lookup SearchAttributeLookupData(Int32 attributeId, LocaleEnum locale, Int32 maxRecordsToReturn, String searchValue, ApplicationContext applicationContext, Boolean isDependent, DependentAttributeCollection dependentAttributes, CallerContext callerContext);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        LookupCollection GetAttributesLookupData(Collection<Int32> attributeIds, LocaleEnum locale, Int32 maxRecordsToReturn, CallerContext callerContext);

        [OperationContract(Name = "GetAttributesLookupDataWithApplicationContext")]
        [FaultContract(typeof(MDMExceptionDetails))]
        LookupCollection GetAttributesLookupData(Collection<Int32> attributeIds, LocaleEnum locale, Int32 maxRecordsToReturn, ApplicationContext applicationContext, CallerContext callerContext);        

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        Dictionary<Int32, Dictionary<Int32, String>> GetLookupAttributeDisplayValue(Dictionary<Int32, Collection<Int32>> attributeValueRefIdPair, LocaleEnum locale, Int32 maxRecordsToReturn, ApplicationContext applicationContext, CallerContext callerContext);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        OperationResult ProcessLookupData(Lookup lookup, String programName, CallerContext callerContext, Boolean invalidateCache);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        OperationResult ProcessLookups(LookupCollection lookups, String programName, CallerContext callerContext);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        LookupCollection GetRelatedLookups(Lookup lookup, CallerContext callerContext);

        [OperationContract (Name="GetLookupsSchema")]
        [FaultContract(typeof(MDMExceptionDetails))]
        LookupCollection GetLookupSchema(Collection<String> lookupNames, CallerContext callerContext);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        Lookup GetLookupSchema(String lookupName, CallerContext callerContext);

        #endregion

        #region Copy Container Mappings Contracts

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        OperationResult CopyContainerMappings(Int32 sourceContainerId, Int32 targetContainerId, ContainerTemplateCopyContext containerTempleteCopyContext, CallerContext callerContext);

        #endregion Copy Container Mappings

        #region Attribute Mappings Contracts

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        Boolean ProcessContainerEntityTypeAttributeMapping(ContainerEntityTypeAttributeMappingCollection containerEntityTypeAttributeMappings);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        Boolean ProcessContainerRelationshipTypeAttributeMapping(ContainerRelationshipTypeAttributeMappingCollection containerRelationshipTypeAttributeMappings);

        #endregion

        #region EntityType Get

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        EntityTypeCollection GetAllEntityTypes(CallerContext callerContext, Boolean getLatest);

        /// <summary>
        /// Get entity type based on Id
        /// </summary>
        /// <param name="entityTypeId">Entity type id for which data is to be fetched</param>
        /// <param name="callerContext">Indicates name of application and module.</param>
        /// <returns>Entity type for given Id</returns>
        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        EntityType GetEntityTypeById(Int32 entityTypeId, CallerContext callerContext);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        EntityTypeCollection GetEntityTypesByIds(Collection<Int32> entityTypeIds);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        EntityTypeCollection GetMappedEntityTypes(Int32 containerId);
        
        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        EntityTypeCollection GetEntityTypesByShortNames(Collection<String> entityTypeShortNames, CallerContext callerContext);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        EntityType GetEntityTypeByShortName(String entityTypeShortName, CallerContext callerContext);

        #endregion EntityType Get

        #region EntityType CUD

        /// <summary>
        /// Create operations of entities of type <see cref="EntityType"/>
        /// </summary>
        /// <param name="entityType">Entity type to create</param>
        /// <param name="callerContext">Indicates application making the API call</param>
        /// <returns>Indicates result of operation</returns>
        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        OperationResult CreateEntityType(EntityType entityType, CallerContext callerContext);

        /// <summary>
        /// Update operations of entities of type <see cref="EntityType"/>
        /// </summary>
        /// <param name="entityType">Entity type to update</param>
        /// <param name="callerContext">Indicates application making the API call</param>
        /// <returns>Indicates result of operation</returns>
        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        OperationResult UpdateEntityType(EntityType entityType, CallerContext callerContext);

        /// <summary>
        /// Delete operations of entities of type <see cref="EntityType"/>
        /// </summary>
        /// <param name="entityType">Entity type to delete</param>
        /// <param name="callerContext">Indicates application making the API call</param>
        /// <returns>Indicates result of operation</returns>
        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        OperationResult DeleteEntityType(EntityType entityType, CallerContext callerContext);

        /// <summary>
        /// Process (CRUD) operations with collection on entities of type <see cref="EntityType"/>
        /// </summary>
        /// <param name="entityTypes">Collections of Entity types</param>
        /// <param name="programName">Name of the program</param>
        /// <param name="callerContext"></param>
        /// <returns>Instance of the <see cref="OperationResult"/></returns>
        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        OperationResultCollection ProcessEntityTypes(EntityTypeCollection entityTypes, CallerContext callerContext);

        #endregion EntityType CUD

        #region RelationshipType CUD

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        OperationResult CreateRelationshipType(RelationshipType relationshipType, CallerContext callerContext);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        OperationResult UpdateRelationshipType(RelationshipType relationshipType, CallerContext callerContext);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        OperationResult DeleteRelationshipType(RelationshipType relationshipType, CallerContext callerContext);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        OperationResultCollection ProcessRelationshipTypes(RelationshipTypeCollection relationshipTypes, CallerContext callerContext);

        #endregion RelationshipType CUD

        #region RelationshipType Get

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        RelationshipTypeCollection GetAllRelationshipTypes(CallerContext callerContext);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        RelationshipType GetRelationshipTypeById(Int32 relationshipTypeId, CallerContext callerContext);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        RelationshipType GetRelationshipTypeByName(String relationshipTypeName, CallerContext callerContext);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        RelationshipTypeCollection GetRelationshipTypes(Int32 containerId, Int32 entityTypeId, CallerContext callerContext);

        #endregion RelationshipType Get

        #region hierarchy Get

        /// <summary>
        /// Get all hierarchies in system by caller context
        /// </summary>
        /// <param name="callerContext">Caller context</param>
        /// <returns>Collection on hierarchies</returns>
        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        HierarchyCollection GetAllHierarchies(CallerContext callerContext);

        /// <summary>
        /// Get hierarchy bu Id
        /// </summary>
        /// <param name="hierarchyId">Id of the hierarchy</param>
        /// <param name="callerContext">Caller context</param>
        /// <returns>Collection on hierarchies</returns>
        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        Hierarchy GetHierarchyById(Int32 hierarchyId, CallerContext callerContext);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hierarchyShortName"></param>
        /// <param name="callerContext"></param>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        Hierarchy GetHierarchyByName(String hierarchyShortName, CallerContext callerContext);


        #endregion hierarchy Get

        #region hierarchy Process

        /// <summary>
        /// Creates entity of type <see cref="Hierarchy"/>
        /// </summary>
        /// <param name="hierarchy">hierarchy</param>
        /// <param name="callerContext">Context of the caller app</param>
        /// <returns>Instance of the <see cref="OperationResult"/></returns>
        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        OperationResult CreateHierarchy(Hierarchy hierarchy, CallerContext callerContext);

        /// <summary>
        /// Updates entity of type <see cref="Hierarchy"/>
        /// </summary>
        /// <param name="hierarchy">hierarchy</param>
        /// <param name="callerContext">Context of the caller app</param>
        /// <returns>Instance of the <see cref="OperationResult"/></returns>
        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        OperationResult UpdateHierarchy(Hierarchy hierarchy, CallerContext callerContext);

        /// <summary>
        /// Deletes entity of type <see cref="Hierarchy"/>
        /// </summary>
        /// <param name="hierarchy">hierarchy</param>
        /// <param name="callerContext">Context of the caller app</param>
        /// <returns>Instance of the <see cref="OperationResult"/></returns>
        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        OperationResult DeleteHierarchy(Hierarchy hierarchy, CallerContext callerContext);

        /// <summary>
        /// Process (CRUD) operations with entity of type <see cref="HierarchyCollection"/>
        /// </summary>
        /// <param name="hierarchyCollection">Collection of Hierarchies</param>
        /// <param name="callerContext">Context of the caller app</param>
        /// <returns>Instance of the <see cref="OperationResult"/></returns>
        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        OperationResultCollection ProcessHierarchies(HierarchyCollection hierarchyCollection, CallerContext callerContext);

        #endregion hierarchy Process

        #region Organization Contracts

        #region Get Methods

        /// <summary>
        /// Get organization by Id
        /// </summary>
        /// <param name="organizationId">Id of the organization</param>
        /// <param name="organizationContext">Indicates context containing flag to load attributes or not</param>
        /// <param name="callerContext">Caller context</param>
        /// <returns>Organization</returns>
        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        Organization GetOrganizationById(Int32 organizationId, OrganizationContext organizationContext, CallerContext callerContext);

        /// <summary>
        /// Get all organizations in system by caller context
        /// </summary>
        /// <param name="organizationContext">Organization Context</param>
        /// <param name="callerContext">Caller context</param>
        /// <returns>Collection on organizations</returns>
        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        OrganizationCollection GetAllOrganizations(OrganizationContext organizationContext, CallerContext callerContext);

        /// <summary>
        /// Get all organization child in system by caller context
        /// </summary>
        /// <param name="parentOrganizationId">Organization which child are requested</param>
        /// <param name="callerContext">Caller context</param>
        /// <returns>Collection on organizations</returns>
        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        MDMObjectInfoCollection GetAllOrganizationDependencies(Int32 parentOrganizationId, CallerContext callerContext);

        /// <summary>
        /// Get all organizations in system by its unique short name
        /// </summary>
        /// <param name="organizationShortName">Organization unique short name</param>
        /// <param name="organizationContext">organization context which defines organization uniquely</param>
        /// <param name="callerContext">Caller context</param>
        /// <returns>Organization</returns>
        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        Organization GetOrganizationByName(String organizationShortName, OrganizationContext organizationContext, CallerContext callerContext);

        #endregion

        #region CUD Methods

        /// <summary>
        /// Creates entity of type <see cref="Organization"/>
        /// </summary>
        /// <param name="organization">Organization</param>
        /// <param name="callerContext">Context of the caller app</param>
        /// <returns>Instance of the <see cref="OperationResult"/></returns>
        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        OperationResult CreateOrganization(Organization organization, CallerContext callerContext);

        /// <summary>
        /// Updates entity of type <see cref="Organization"/>
        /// </summary>
        /// <param name="organization">Organization</param>
        /// <param name="callerContext">Context of the caller app</param>
        /// <returns>Instance of the <see cref="OperationResult"/></returns>
        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        OperationResult UpdateOrganization(Organization organization, CallerContext callerContext);

        /// <summary>
        /// Deletes entity of type <see cref="Organization"/>
        /// </summary>
        /// <param name="organization">Organization</param>
        /// <param name="callerContext">Context of the caller app</param>
        /// <returns>Instance of the <see cref="OperationResult"/></returns>
        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        OperationResult DeleteOrganization(Organization organization, CallerContext callerContext);

        #endregion

        #region Process Methods

        /// <summary>
        /// Process (CRUD) operations with entity of type <see cref="OrganizationCollection"/>
        /// </summary>
        /// <param name="organizationCollection">Collection of Organizations</param>
        /// <param name="callerContext">Context of the caller app</param>
        /// <returns>Instance of the <see cref="OperationResult"/></returns>
        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        OperationResultCollection ProcessOrganizations(OrganizationCollection organizationCollection, CallerContext callerContext);

        #endregion

        #endregion

        #region Category - Attribute Mapping Contracts

        #region Category - Attribute Mapping Get Contracts

        /// <summary>
        /// Get  Category - Attribute Mappings
        /// </summary>
        /// <param name="categoryId">The category identifier.</param>
        /// <param name="callerContext">Caller context</param>
        /// <returns>
        /// Collection on Category - Attribute Mapping
        /// </returns>
        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        CategoryAttributeMappingCollection GetCategoryAttributeMappingsByCategoryId(Int64 categoryId, CallerContext callerContext);

        /// <summary>
        /// Get  Category - Attribute Mappings
        /// </summary>
        /// <param name="catalogId">The catalog identifier.</param>
        /// <param name="callerContext">Caller context</param>
        /// <returns>
        /// Collection on Category - Attribute Mapping
        /// </returns>
        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        CategoryAttributeMappingCollection GetCategoryAttributeMappingsByCatalogId(Int32 catalogId, CallerContext callerContext);

        /// <summary>
        /// Get  Category - Attribute Mappings
        /// </summary>
        /// <param name="hierarchyId">The hierarchy identifier.</param>
        /// <param name="callerContext">Caller context</param>
        /// <returns>
        /// Collection on Category - Attribute Mapping
        /// </returns>
        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        CategoryAttributeMappingCollection GetCategoryAttributeMappingsByHierarchyId(Int32 hierarchyId, CallerContext callerContext);

        #endregion Category - Attribute Mapping Get Contracts

        #region Category - Attribute Mapping CUD Contracts

        /// <summary>
        /// Creates entity of type <see cref="CategoryAttributeMapping"/>
        /// </summary>
        /// <param name="categoryAttributeMapping">The Category - Attribute Mapping</param>
        /// <param name="callerContext">Context of the caller app</param>
        /// <returns>Instance of the <see cref="OperationResult"/></returns>
        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        OperationResult CreateCategoryAttributeMapping(CategoryAttributeMapping categoryAttributeMapping, CallerContext callerContext);

        /// <summary>
        /// Updates entity of type <see cref="CategoryAttributeMapping"/>
        /// </summary>
        /// <param name="categoryAttributeMapping">The Category - Attribute Mapping</param>
        /// <param name="callerContext">Context of the caller app</param>
        /// <returns>Instance of the <see cref="OperationResult"/></returns>
        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        OperationResult UpdateCategoryAttributeMapping(CategoryAttributeMapping categoryAttributeMapping, CallerContext callerContext);

        /// <summary>
        /// Deletes entity of type <see cref="CategoryAttributeMapping"/>
        /// </summary>
        /// <param name="categoryAttributeMapping">The Category - Attribute Mapping</param>
        /// <param name="callerContext">Context of the caller app</param>
        /// <returns>Instance of the <see cref="OperationResult"/></returns>
        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        OperationResult DeleteCategoryAttributeMapping(CategoryAttributeMapping categoryAttributeMapping, CallerContext callerContext);

        /// <summary>
        /// Inherits entity of type <see cref="CategoryAttributeMapping"/>
        /// </summary>
        /// <param name="categoryAttributeMapping">The Category - Attribute Mapping</param>
        /// <param name="callerContext">Context of the caller app</param>
        /// <returns>Instance of the <see cref="OperationResult"/></returns>
        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        OperationResult InheritCategoryAttributeMapping(CategoryAttributeMapping categoryAttributeMapping, CallerContext callerContext);

        /// <summary>
        /// Process (CRUD) operations with entity of type <see cref="CategoryAttributeMappingCollection"/>
        /// </summary>
        /// <param name="categoryAttributeMappingCollection">Collection of Category - Attribute Mapping</param>
        /// <param name="callerContext">Context of the caller app</param>
        /// <returns>Collection of the <see cref="OperationResult"/></returns>
        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        OperationResultCollection ProcessCategoryAttributeMappings(CategoryAttributeMappingCollection categoryAttributeMappingCollection, CallerContext callerContext);

        #endregion Category - Attribute Mapping CUD

        #endregion Category - Attribute Mapping Contracts

        #region Dependency Attribute Contracts

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        Collection<String> GetDependencyMappings(Int32 attributeId, ApplicationContext applicationContext, DependentAttributeCollection dependentAttributeCollection, CallerContext callerContext);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        DependentAttributeCollection GetDependencyDetails(Int32 attributeId, ApplicationContext applicationContext, Boolean includeChildDependency, CallerContext callerContext);

        /// <summary>
        /// Get all dependencies for given attribute.
        /// </summary>
        /// <param name="attributeId">Attribute Id for which dependencies are to be selected</param>
        /// <param name="callerContext">Context indicating application making this API call.</param>
        /// <returns>Attribute Dependencies having collection of parent attribute and context for given attribute id</returns>
        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        DependentAttributeCollection GetAttributeDependencies(Int32 attributeId, CallerContext callerContext);

        /// <summary>
        /// Get the dependent attribute data for the requested link table
        /// </summary>
        /// <param name="linkTableName">Indicates the link table name</param>
        /// <param name="locale">Indicates the locale</param>
        /// <param name="callerContext">Indicates the caller context information regarding application and module.</param>
        /// <returns>Returns the dependent attribute data mapping collection for the respective link table.</returns>
        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        DependentAttributeDataMapCollection GetDependentData(String linkTableName, LocaleEnum locale, CallerContext callerContext);

        /// <summary>
        /// Get all child dependent attribute models
        /// </summary>
        /// <param name="modelContext">Attribute model context which indicates what all attribute models to load.</param>
        /// <param name="callerContext">Context indicating the caller of the API</param>
        /// <returns>Dependent child attribute model</returns>
        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        AttributeModelCollection GetAllDependentChildAttributeModels(AttributeModelContext modelContext, CallerContext callerContext);

        #endregion

        #region Dependency Attribute CUD

        /// <summary>
        /// Create attribute dependency
        /// </summary>
        /// <param name="attributeId">AttributeId for which dependency is to be created</param>
        /// <param name="dependentAttribute">Attribute dependency to create</param>
        /// <param name="callerContext">Context which called the application</param>
        /// <returns>Result of the operation</returns>
        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        OperationResult CreateAttributeDependency(Int32 attributeId, DependentAttribute dependentAttribute, CallerContext callerContext);

        /// <summary>
        /// Update attribute dependency
        /// </summary>
        /// <param name="attributeId">AttributeId for which dependency is to be created</param>
        /// <param name="dependentAttribute">Attribute dependency to create</param>
        /// <param name="callerContext">Context which called the application</param>
        /// <returns>Result of the operation</returns>
        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        OperationResult UpdateAttributeDependency(Int32 attributeId, DependentAttribute dependentAttribute, CallerContext callerContext);


        /// <summary>
        /// Delete attribute dependency
        /// </summary>
        /// <param name="attributeId">AttributeId for which dependency is to be created</param>
        /// <param name="dependentAttribute">Attribute dependency to create</param>
        /// <param name="callerContext">Context which called the application</param>
        /// <returns>Result of the operation</returns>
        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        OperationResult DeleteAttributeDependency(Int32 attributeId, DependentAttribute dependentAttribute, CallerContext callerContext);

        /// <summary>
        /// Create - Update or Delete given attribute dependencies
        /// </summary>
        /// <param name="attributeId">AttributeId for which dependency is to be processed</param>
        /// <param name="dependentAttributes">DependentAttribute collection to process</param>
        /// <param name="callerContext">Context which called the application</param>
        /// <returns>Result of the operation</returns>
        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        OperationResultCollection ProcessAttributeDependencies(Int32 attributeId, DependentAttributeCollection dependentAttributes, CallerContext callerContext);

        /// <summary>
        /// Create attribute data dependency 
        /// </summary>
        /// <param name="dependentAttributeDataMap">DependentAttribute Data map to create</param>
        /// <param name="callerContext">Context which called the application</param>
        /// <returns>Result of the operation</returns>
        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        OperationResult CreateDependentData(DependentAttributeDataMap dependentAttributeDataMap, CallerContext callerContext);

        /// <summary>
        /// Update attribute data dependency
        /// </summary>
        /// <param name="dependentAttributeDataMap">DependentAttribute Data map to create</param>
        /// <param name="callerContext">Context which called the application</param>
        /// <returns>Result of the operation</returns>
        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        OperationResult UpdateDependentData(DependentAttributeDataMap dependentAttributeDataMap, CallerContext callerContext);

        /// <summary>
        /// Delete attribute data dependency
        /// </summary>
        /// <param name="dependentAttributeDataMap">DependentAttribute Data map to create</param>
        /// <param name="callerContext">Context which called the application</param>
        /// <returns>Result of the operation</returns>
        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        OperationResult DeleteDependentData(DependentAttributeDataMap dependentAttributeDataMap, CallerContext callerContext);

        /// <summary>
        /// Process attribute data dependency
        /// </summary>
        /// <param name="dependentAttributeDataMap">DependentAttribute Data map to create</param>
        /// <param name="callerContext">Context which called the application</param>
        /// <returns>Result of the operation</returns>
        [OperationContract(Name = "ProcessDependentDatum")]
        [FaultContract(typeof(MDMExceptionDetails))]
        OperationResult ProcessDependentData(DependentAttributeDataMap dependentAttributeDataMap, CallerContext callerContext);

        /// <summary>
        /// Create - Update or Delete given application context
        /// </summary>
        /// <param name="dependentAttributeDataMaps">DependentAttribute Data map collection to process</param>
        /// <param name="objectType">Indicates types of object</param>
        /// <param name="userName">Indicates the name of user</param>
        /// <param name="programName">Indicates the name of program</param>
        /// <param name="callerContext">Context which called the application</param>
        /// <returns>Result of the operation</returns>
        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        OperationResultCollection ProcessDependentData(DependentAttributeDataMapCollection dependentAttributeDataMaps, CallerContext callerContext);

        #endregion Dependency Attribute CUD

        #region Attribute group
        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        Collection<AttributeGroup> GetAttributeGroupsByName(String attributeGroupShortName, AttributeModelContext attributeModelContext, CallerContext callerContext);

        #endregion

        #region EntityType Attribute Mapping

        #region Get

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        EntityTypeAttributeMappingCollection GetAllEntityTypeAttributeMapping(CallerContext callerContext);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        EntityTypeAttributeMappingCollection GetEntityTypeAttributeMappingsByEntityTypeId(Int32 entityTypeId, CallerContext callerContext);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        EntityTypeAttributeMappingCollection GetEntityTypeAttributeMappingsByAttributeId(Int32 attributeId, CallerContext callerContext);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        EntityTypeAttributeMappingCollection GetEntityTypeAttributeMappings(Int32 entityTypeId, Int32 attributeGroupId, CallerContext callerContext);

        #endregion Get

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        OperationResult ProcessEntityTypeAttributeMapping(EntityTypeAttributeMappingCollection entityTypeAttributeMappings, CallerContext callerContext);

        #endregion EntityType Attribute Mapping

        #region Container EntityType Attribute Mapping

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        ContainerEntityTypeAttributeMappingCollection GetContainerEntityTypeAttributeMappings(Int32 containerId, Int32 entityTypeId, Int32 attributeGroupId, Int32 attributeId, CallerContext callerContext);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        OperationResult ProcessContainerEntityTypeAttributeMappings(ContainerEntityTypeAttributeMappingCollection containerEntityTypeAttributeMappings, CallerContext callerContext);

        #endregion Container EntityType Attribute Mapping

        #region Container EntityType Mapping

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        ContainerEntityTypeMappingCollection GetAllContainerEntityTypeMappings(CallerContext callerContext);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        ContainerEntityTypeMappingCollection GetContainerEntityTypeMappingsByContainerId(Int32 containerId, CallerContext callerContext);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        ContainerEntityTypeMappingCollection GetContainerEntityTypeMappingsByEntityTypeId(Int32 entityTypeId, CallerContext callerContext);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        EntityTypeCollection GetMappedEntityTypesWithContainer(Int32 containerId, CallerContext callerContext);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        OperationResult ProcessContainerEntityTypeMappings(ContainerEntityTypeMappingCollection containerEntityTypeMappings, CallerContext callerContext);

        #endregion Container EntityType Mapping

        #region Container RelationshipType Attribute Mapping

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        ContainerRelationshipTypeAttributeMappingCollection GetContainerRelationshipTypeAttributeMappings(Int32 containerId, Int32 relationshipTypeId, Int32 attributeGroupId, CallerContext callerContext);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        OperationResult ProcessContainerRelationshipTypeAttributeMappings(ContainerRelationshipTypeAttributeMappingCollection containerRelationshipTypeAttributeMappings, CallerContext callerContext);

        #endregion Container RelationshipType Attribute Mapping

        #region Container RelationshipType EntityType Mapping

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        ContainerRelationshipTypeEntityTypeMappingCollection GetAllContainerRelationshipTypeEntityTypeMappings(CallerContext callerContext);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        ContainerRelationshipTypeEntityTypeMappingCollection GetContainerRelationshipTypeEntityTypeMappings(Int32 containerId, Int32 relationshipTypeId, CallerContext callerContext);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        EntityTypeCollection GetMappedEntityTypesWithContainerAndRelationshipType(Int32 containerId, Int32 relationshipTypeId, CallerContext callerContext);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        OperationResult ProcessContainerRelationshipTypeEntityTypeMappings(ContainerRelationshipTypeEntityTypeMappingCollection containerRelationshipTypeEntityTypeMapping, CallerContext callerContext);

        #endregion  Container RelationshipType EntityType Mapping

        #region Container RelationshipType EntityType Mapping Cardinalities

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        ContainerRelationshipTypeEntityTypeMappingCardinalityCollection GetContainerRelationshipTypeEntityTypeMappingCardinalities(Int32 containerId, Int32 fromEntityTypeId, Int32 relationshipTypeId, CallerContext callerContext);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        OperationResultCollection ProcessContainerRelationshipTypeEntityTypeMappingCardinalities(ContainerRelationshipTypeEntityTypeMappingCardinalityCollection containerRelationshipTypeEntityTypeMappingCardinalities, CallerContext callerContext);

        #endregion Container RelationshipType EntityType Mapping Cardinalities

        #region RelationshipType Attribute Mapping

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        RelationshipTypeAttributeMappingCollection GetRelationshipTypeAttributeMappings(Int32 relationshipTypeId, Int32 attributeGroupId, CallerContext callerContext);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        OperationResult ProcessRelationshipTypeAttributeMappings(RelationshipTypeAttributeMappingCollection relationshipTypeAttributeMappings, CallerContext callerContext);

        #endregion RelationshipType Attribute Mapping

        #region RelationshipType EntityType Mapping

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        RelationshipTypeEntityTypeMappingCollection GetAllRelationshipTypeEntityTypeMappings(CallerContext callerContext);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        RelationshipTypeEntityTypeMappingCollection GetRelationshipTypeEntityTypeMappingsByRelationshipTypeId(Int32 relationshipTypeId, CallerContext callerContext);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        RelationshipTypeEntityTypeMappingCollection GetRelationshipTypeEntityTypeMappingsByEntityTypeId(Int32 entityTypeId, CallerContext callerContext);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        EntityTypeCollection GetMappedEntityTypesWithRelationshipType(Int32 relationshipTypeId, CallerContext callerContext);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        OperationResult ProcessRelationshipTypeEntityTypeMappings(RelationshipTypeEntityTypeMappingCollection relationshipTypeEntityTypeMapping, CallerContext callerContext);

        #endregion RelationshipType EntityType Mapping

        #region Category Get/Search

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        CategoryCollection GetAllCategories(Int32 hierarchyId, LocaleEnum locale, CallerContext callerContext);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        CategoryCollection GetAllCategoriesUsingContainerName(String containerName, LocaleEnum locale, CallerContext callerContext);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        Category GetCategoryById(Int32 hierarchyId, Int64 categoryId, CallerContext callerContext);

        [OperationContract]
        [FaultContract(typeof (MDMExceptionDetails))]
        CategoryCollection GetCategoriesByIds(HierachyCategoryMappingCollection mappingCollection,
            CallerContext callerContext);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        Category GetCategoryByName(Int32 hierarchyId, String categoryName, CallerContext callerContext);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        Category GetCategoryByNameUsingContainerName(String containerName, String categoryName, CallerContext callerContext);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        Category GetCategoryByPath(Int32 hierarchyId, String categoryPath, CallerContext callerContext);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        Category GetCategoryByPathUsingContainerName(String containerName, String categoryPath, CallerContext callerContext);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        CategoryCollection SearchCategories(CategoryContext categoryContext, CallerContext callerContext);

        #endregion

        #region UOM Contracts

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        UOM GetUom(UomContext uomContext, CallerContext callerContext);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        string GetUomConversionsAsXml(CallerContext callerContext);

        #endregion
        
        #region Unique Id Methods

        [OperationContract]
        Collection<String> GenerateUniqueId(CallerContext callerContext, UniqueIdGenerationContext uniqueIdGenerationContext);       

        #endregion

        #region Entity Model Contracts

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        Entity GetEntityModel(EntityContext entityContext, CallerContext callerContext);

        #endregion

        #region Entity Variant Definition Mapping 

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        EntityVariantDefinitionMappingCollection GetAllEntityVariantDefinitionMappings(CallerContext callerContext);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        OperationResultCollection ProcessEntityVariantDefinitionMappings(EntityVariantDefinitionMappingCollection entityVariantDefinitionMappings, CallerContext callerContext);

        #endregion Entity Variant Definition Mapping 

    }
}
