using System;
using System.ServiceModel;
using System.Collections.ObjectModel;
using System.Data;
using System.Collections.Generic;
using System.Xml;
using MDM.BusinessObjects.Exports;

namespace MDM.WCFServiceInterfaces
{
    using MDM.BusinessObjects;
    using MDM.Core;
    using MDM.BusinessObjects.Jobs;
    using MDM.BusinessObjects.DynamicTableSchema;

    /// <summary>
    /// Defines operation contracts for MDM legacy data access operations
    /// </summary>
    [ServiceContract(Namespace = "http://wcfservices.riversand.com")]
    public interface ILegacyDataAccessService
    {
        #region Application Config

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        DataTable GetApplicationConfigData(Int32 eventSourceId, Int32 eventSubscriberId, Int32 securityRoleId, Int32 securityUserId, Int32 orgId, Int32 catalogId, Int32 categoryId, Int32 cNodeId, Int32 attributeId, Int32 nodeTypeId, Int32 relationshipTypeId, Int32 localId, Int32 applicationConfigId, String categoryPath, String objectName);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        Int32 UpdateApplicationConfigXML(Int32 FK_Application_ContextDefinition, Int32 FK_Application_ConfigParent,
                    String ShortName, String LongName, Int32 FK_Event_Source, Int32 FK_Event_Subscriber, Int32 FK_Org, Int32 FK_Catalog,
                    Int32 FK_Category, Int32 FK_CNode, Int32 FK_Attribute, Int32 FK_NodeType, Int32 FK_RelationshipType, Int32 FK_Security_Role,
                    Int32 FK_Security_user, String ConfigXML, String Description, String PreCondition, String PostCondition, String XSDSchema,
                    String SampleXML, String loginUser, String userProgram, Int32 FK_Locale);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        DataTable GetContextualApplicationConfigData(Int32 eventSourceId, Int32 eventSubscriberId);

        #endregion Application Config Contracts

        #region Misc

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        DataTable GetSearchCriterias(Int32 searchCriteriaID, Int32 loginId, Int32 catalogId);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        DataTable GetCategoryNavPanel(Int32 catalogID, String sysAttrSelectionXml, String categorySearchColumn, String categorySearchString, Int64 parentCategoryId, Int32 countFrom, Int32 countTo, String strvchrUserLogin, Int32 currentDataLocale);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        int ProcessSearchCriteria(String action, Int32 criteriaId, String criteriaName, Int32 loginId, Int32 catalogId, Boolean isGlobalSearch, String searchXml, String loginUser, String userProgram);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        DataTable GetAllCategories(int altTaxId, int currentDataLocale, String filter, String loginUser);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        String GetSystemAttributes(int orgId, Boolean excludeSearchable, Int32 currentDataLocale);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        String GetCharacteristicTemplate(Int32 selectedCategoryId, Int32 catalogID, Int32 currentDataLocale, Boolean useDraftAccess, Int32 usesChilds, Int32 orgID, Boolean excludeSearchable);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        DataTable GetRuleViewAttributes(Int32 cNodeId, Int32 businessRuleId);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        DataTable GetBusinessRuleFromContext(String strContentXml);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        Int32 DeleteSearchCriteria(Int32 searchCriteriaId, String loginUser, String modProgram);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        DataTable GetUserVisibleOrgsDT(String targetLoginUser, String loginUser, Int32 localId, Int32 countFrom, Int32 countTo, String sortColumn, String searchColumn, String searchParameters);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        DataTable GetUserVisibleCatalogsDT(String targetLoginUser, String loginUser, Int32 orgId, Int32 localeId, Int32 countFrom, Int32 countTo, String sortColumn, String searchColumn, String searchParameter, Int32 catalogId, Boolean includeTaxonomy, Boolean includeDynamicTaxonomy, Boolean includeCatalog, Boolean includeView, Boolean includeProduction, Boolean includeDraft);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        DataTable GetAssignmentButtons(String assignmentStatus, String cNodeList, String toolbarButtonXml, String vchrUserLogin);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        DataTable GetBreadcrumbAttributeValueString(String breadcrumbConfigXML, Int32 fkOrgId, Int32 catalogId, Int64 cNodeId, Int32 localeId);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        AttributeVersionCollection Get(Int64 entityId, Int32 attributeId, Int32 localeId, Int32 catalogId, Int64 entityParentId, CallerContext callerContext);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        int ProcessServiceResult(Int32 eventSource, Int32 applicationConfig, String dataXmlString, String loginUser, String userProgram);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        DataTable GetJobTypeEventSourceMapping(Int32 eventSourceId, Int32 jobId);

        #endregion Misc

        #region Catalog

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        DataTable GetRelationshipTypeHierarchy(Int32 FK_Catalog, Int32 FK_RelationshipType_Top, Int32 MaxLevel);

        /// <summary>
        /// 
        /// </summary>
        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        string ExtractCatalogByIDLocalRel(Int32 intExtSystemID, String txtXML, String vchrRelAttrList);

        /// <summary>
        /// 
        /// </summary>
        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        string ExtractBulkAttributeMetadata(String vchrTargetUserLogin, String vchrUserLogin, String txtXML, Boolean bitUseDraftTax, Int32 localeId, Boolean ignoreComplexAttributes);

        /// <summary>
        /// 
        /// </summary>
        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        string ExtractBulkAttributeMetadataRel(String vchrTargetUserLogin, String vchrUserLogin, String txtXML, Boolean bitUseDraftTax, Int32 localeId);

        /// <summary>
        /// 
        /// </summary>
        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        string ExtractAttributes(String vchrTargetUserLogin, String vchrUserLogin, String txtXML, Boolean bitUseDraftTax);

        /// <summary>
        /// 
        /// </summary>
        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        DataTable GetCategoryAttributeMap(Int32 intCategoryID, Int32 intCatalogID, Int32 intLocaleID);

        /// <summary>
        /// 
        /// </summary>
        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        DataTable GetCharacteristicTemplateDT(Int32 intCategoryID, Int32 intLocaleID);

        /// <summary>
        /// 
        /// </summary>
        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        string GetUserVisibleCatalogs(String vchrTargetUserLogin, String vchrUserLogin, Int32 FK_Org, Int32 FK_Locale, Int32 intCountFrom, Int32 intCountTo, String vchrSortColumn, String vchrSearchColumn, String vchrSearchParameter, Int32 PK_Catalog, Boolean IncludeTaxonomy, Boolean IncludeDynamicTaxonomy, Boolean IncludeCatalog, Boolean IncludeView, Boolean IncludeProduction, Boolean IncludeDraft);

        /// <summary>
        /// 
        /// </summary>
        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        string GetCatalogPermissionsByOrg(String vchrTargetUserLogin, String vchrUserLogin, Int32 FK_Org, Int32 FK_Locale, Int32 intCountFrom, Int32 intCountTo, String vchrSortColumn, String vchrSearchColumn, String vchrSearchParameter, Int32 PK_Catalog, Boolean IncludeTaxonomy, Boolean IncludeDynamicTaxonomy, Boolean IncludeCatalog, Boolean IncludeView, Boolean IncludeProduction, Boolean IncludeDraft);

        /// <summary>
        /// 
        /// </summary>
        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        DataTable GetCatalogDT(String vchrTargetUserLogin, String vchrUserLogin, Int32 FK_Org, Int32 FK_Locale, Int32 intCountFrom, Int32 intCountTo, String vchrSortColumn, String vchrSearchColumn, String vchrSearchParameter, Int32 PK_Catalog, Boolean IncludeTaxonomy, Boolean IncludeDynamicTaxonomy, Boolean IncludeCatalog, Boolean IncludeView, Boolean IncludeProduction, Boolean IncludeDraft);
        /// <summary>
        /// 
        /// </summary>
        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        string GetCatalogsByOrg(String orgId, String vchrTargetUserLogin, String vchrUserLogin);

        /// <summary>
        /// 
        /// </summary>
        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        DataTable ProcessCatalogs(String txtXML, Int32 PK_Org, String vchrUserLogin);

        /// <summary>
        /// 
        /// </summary>
        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        string GetCatalogLocaleByID(Int32 PK_Catalog, String vchrUserLogin);

        /// <summary>
        /// 
        /// </summary>
        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        Boolean ProcessCatalogLocales(String txtXML, Int32 PK_Org, String vchrUserLogin);

        /// <summary>
        /// 
        /// </summary>
        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        string GetNodePermissions(String vchrTargetUserLogin, String vchrUserLogin, Int32 PK_Catalog, Int32 FK_ParentCNode, Int32 FK_Locale, Int32 FK_Customer, Int32 intCountFrom, Int32 intCountTo, String vchrSortColumn, String vchrSearchColumn, String vchrSearchParameter, Int32 PK_CNode, Boolean bitIncludeComponents, Boolean bitEnableComponentMapping, Boolean bitUseDrafTax, Boolean bitEnableUnassignedCategory, String ToolTipAttributeList);

        /// <summary>
        /// 
        /// </summary>
        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        string GetNodePermissionsByCNode(String vchrTargetUserLogin, String vchrUserLogin, Int32 PK_Catalog, Int32 CnodeId, Int32 FK_Locale, Int32 FK_Customer, Int32 intCountFrom, Int32 intCountTo, String vchrSortColumn, String vchrSearchColumn, String vchrSearchParameter, Boolean bitIncludeComponents, Boolean bitEnableComponentMapping, Boolean bitUseDrafTax, Boolean bitEnableUnassignedCategory);

        /// <summary>
        /// 
        /// </summary>
        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        string GetCoreAttrByGroup(Int32 intLocaleID, Int32 intCustomerID, Int64 intCNodeID, Int64 intCNodeParentID, Int32 intGroupID, Int32 intCatalogID, Int32 intOrgID, String vchrUserID, Int32 intBackLocaleID, String vchrViewPath, Boolean bitUseDraftTax, Boolean ShowAtCreation, String AttrIDList);

        /// <summary>
        /// 
        /// </summary>
        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        Int64 ProcessCoreAttr(String txtXML, Int32 intCatalogID, Int32 intOrgID, String vchrUserID, String vchrProgramName, Int32 LocaleId);

        /// <summary>
        /// 
        /// </summary>
        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        Int64 ProcessTechAttr(String txtXML, Int32 intCatalogID, Int32 intOrgID, String vchrUserID, String vchrProgramName, Int32 LocaleId);

        /// <summary>
        /// 
        /// </summary>
        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        string GetTechAttr(Int32 intCnodeID, Int32 intCnodeParentID, Int32 intCatalogID, Int32 intGroupID, Int32 intLocaleID, Int32 intCustomerID, String vchrUserID, Int32 intBackupLocaleID, String vchrViewPath, Boolean bitUseDraftTax, String AttrIDList);

        /// <summary>
        /// 
        /// </summary>
        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        DataTable GetStatuses();

        /// <summary>
        /// 
        /// </summary>
        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        Boolean SchemaValidationRulesExecution(Int32 JobId, String UserID);

        /// <summary>
        /// 
        /// </summary>
        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        DataSet GetCatalogAttributes(String UserLogin);

        /// <summary>
        /// 
        /// </summary>
        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        Boolean CopyCatalogAttributes(Int32 intFromCatalogId, Int32 intToCatalogId, String CreateProgram, String CreateUser);

        /// <summary>
        /// 
        /// </summary>
        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        DataTable GetNameValCollection(String IdList);

        /// <summary>
        /// 
        /// </summary>
        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        string GetCollectionValues(Int32 FK_CNode, Int32 ParentId, Int32 FK_Catalog, Int32 FK_Customer, Int32 FK_Locale, String InheritanceMode);

        /// <summary>
        /// 
        /// </summary>
        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        string GetCatalogNodeTypeAttributesXML(Int32 intOrgID, Int32 intCatalogID, String nvchrNodeType, Int32 intBranchLevel, Boolean IncludeComplexAttrChildren, Boolean ExcludeSearchable);

        /// <summary>
        /// 
        /// </summary>
        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        string GetSearchCategoriesByCriteria(String vchrSearchValue, Int32 intCatalogID, Int32 intParentID, String toolTipAttributeList, String vchrUserLogin, Int32 dataLocale);

        /// <summary>
        /// 
        /// </summary>
        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        string GetVisibleComponents(String vchrTargetUserLogin, String vchrUserLogin, Int32 intOrgId, Int32 intCatalogId, Int32 intNodeId, Boolean bitRecursive, Boolean bitUseDraftTaxonomy);

        /// <summary>
        /// 
        /// </summary>
        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        Int32 ProcessRelAttr(String txtXML, Int32 intCatalogID, Int32 intOrgID, String vchrUserID, String vchrProgramName);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        DataSet GetUserConfigContextData(Int32 applicationContextType, String seqDataTableforUI);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        string GetAllUOMsByUOMType();

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        DataSet GetUserConfigMetadata(String configXml);
        #endregion

        #region Events

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        DataTable GetEventSources(Int32 eventSourceId, String eventSourceName);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        DataTable GetEventSubscribers(Int32 eventSubscriberId, String eventSubscriberName);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        DataTable GetApplicationConfigTypes(Int32 applicationConfigTypeId, String applicationConfigTypeName);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        DataTable GetMatchingRuleSets(Int32 orgId, Int32 catalogId, Int32 eventSourceId, Int32 eventSubscriberId, Int32 securityUserId);
        #endregion

        #region Rules

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        DataTable GetViewContext(Int32 viewId);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        DataSet GetViewContextDetail(Int32 contextId, String loginUser);

        #endregion

        #region Matching

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        String CreateServicesJob(String xml, String userId, Int32 serviceType);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        DataTable GetRSPLMatchingStatus(String cNodes, Int32 catalogId);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        DataTable GetMatchedCnodes(String nodeName, String dataXML);

        #endregion

        #region BulkOperation
        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        String ExtractBulkOperationAttributeMetaData(String targetUserLogin, String userLogin, String inputDataMode, String selectedNodeTypes, String txtXML, Boolean bitUseDraftTax);
        #endregion

        #region Report
        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        DataTable GetAllLocales();
        #endregion

        #region Hierarchy
        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        Collection<Entity> GetAllCategoriesByHierarchy(int localeId, int taxonomyId, string searchParameter, int countTo);
        #endregion

        #region Knowledge
        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        Collection<TimeZone> GetAll();

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        String GetUnitsXML();
        #endregion

        #region Relationship

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        String GetEntityRelationshipDetails(Int64 entityId, EntityContext entityContext, CallerContext callerContext, Boolean publishEvents, Boolean applyAVS);

        #endregion

        #region Security
        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        String GetRoleMenus(Int32 securityRole);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        String GetUsers(Int32 securityUser, Int32 userType, Int32 countFrom, Int32 countTo, String vchrSortColumn, String vchrSearchColumn, String vchrSearchParameter, String vchrUserLogin);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        String GetUsersWithRoles(Int32 securityUser, Int32 countFrom, Int32 countTo, String vchrSearchColumn, String vchrSearchParameter, String vchrUserLogin);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        Boolean ProcessUsers(String txtXML, String vchrUserLogin);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        String GetRoles(Int32 securityRole, Int32 userType, String chrGetPermissionSetOnly, Int32 intCountFrom, Int32 intCountTo, String vchrSortColumn, String vchrSearchColumn, String vchrSearchParameter, String vchrUserLogin, Boolean bitDisplaySystemRole);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        String GetRolesWithUsers(Int32 pkSecurityRole, String chrGetPermissionSetOnly, Int32 intCountFrom, Int32 intCountTo, String vchrSearchColumn, String vchrSearchParameter, String vchrUserLogin, Boolean bitDisplaySystemRole);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        DataTable ProcessRoles(String txtXML, String vchrUserLogin);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        Boolean ProcessRoleMenus(String txtXML, String vchrUserLogin);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        String GetObjectTypeAction();

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        String GetHierarchyAction();

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        String GetPermissions(Int32 securityRole, String chrPermissionSet, String vchrUserLogin);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        Boolean ProcessPermissions(String txtXML, String vchrUserLogin);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        String GetAllOrganizationsAndCatalogs(String vchrUserLogin);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        String IsSystemUser(String nvchrUserLogin, String isSystemUser);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        Boolean HasPermission(String vchrUserLogin, String objectTypeSN, String actionSN, String parentObjectTypeSN, Int32 orgId, Int32 catalogId, Int64 categoryCNode, Int64 componentCNode, Int32 attribute, Int32 relationship, Boolean bitForDraft, Boolean bitHasPermission);

        #endregion

        #region Organization
        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        string GetUserVisibleOrgs(String targetUserLogin, String userLogin, Int32 localeId, Int32 intCountFrom, Int32 intCountTo, String sortColumn, String searchColumn, String searchParameter);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        string GetOrgsWithPermissions(String targetUserLogin, String userLogin, Int32 localeId, Int32 intCountFrom, Int32 intCountTo, String sortColumn, String searchColumn, String searchParameter, Int32 orgParentId, Int32 orgClassificationId, String catalogObjectType);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        string GetOrgsByOrgClassification(String targetUserLogin, String userLogin, Int32 localeId, Int32 intCountFrom, Int32 intCountTo, String sortColumn, String searchColumn, String searchParameter, Int32 orgClassificationId, String catalogObjectType);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        string GetAllOrgTypes(Int32 orgTypeId, Int32 intCountFrom, Int32 intCountTo, String sortColumn, String searchColumn, String searchParameter, String userLogin);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        string GetOrgTypes(Int32 orgTypeId, Int32 intCountFrom, Int32 intCountTo, String sortColumn, String searchColumn, String searchParameter, String userLogin);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        Boolean ProcessOrgTypes(String txtXML, String userLogin);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        string GetAllOrgHierarchies();

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        string GetOrgsWithLocales(Int32 orgClassificationId, Int32 localeId, String userLogin);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        Boolean ProcessOrgLocales(String txtXML, String userLogin);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        DataTable GetOrgsWithPermissionsOrg(String userLogin);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        DataTable GetOrgCatalogInfo(Int32 orgId, Int32 catalogId, String findWhat);
        #endregion

        #region Attribute

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        string GetAttributeDetails(Int32 attributeId, String userLogin);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        DataTable GetAllAttributes();

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        string GetCNodeAttributeValue(Int64 cNodeId, Int32 catalogId, Int32 attributeId, Int32 localeId, String userName, Int32 returnAttrType, Boolean showAtCreation);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        string GetAttributeGroupsXML();

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        string GetTechSpecsByGroup(Int32 attrParentId);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        DataSet ComplexMetadata_GetDT(String inputStr);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        string GetAttributeChildrenByType(Int32 attributeTypeId, String attributeId, String userId, Int32 orgId, Int32 catalogId, Int64 cNodeId, String viewPath);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        String GetSysObjectAttributesXML(String objectType, Int32 objectId, Int32 localeId);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        Boolean ProcessObjectAttributes(String attributeXML, Int32 objectId, String objectType, Int32 localeId);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        DataSet UniqueComplxAttrid();

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        string GetAttributeConfig(String tableName, Int32 id);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        string GetAttributeUsage(Int32 attributeId);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        string GetAttributeGroupChildren(Int32? parentId, Int32 localeId, String userLogin);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        string GetAttributes(Int32 parentId, Int32 countFrom, Int32 countTo, String searchParameter, String searchColumn, String sortColumn, Int32 localeId, Boolean bitUnusedOnly, String userLogin);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        string GetNodeAttributeDetails(Int32 attributeId, Int32 cNodeId, String userLogin);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        string GetAttributeDataTypeMap(String userLogin);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        string GetAttributeDataTypes(String userLogin);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        string GetAttributeDisplayTypeMap(String userLogin);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        string GetAttributeDisplayTypes(String userLogin);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        string GetFormattersXML();

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        string UpdateTargetAttributeValue(String txtXML, Int32 targetAttributeId, Int32 maxAllowableChars, String inTestPage, String retAttrValue);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        string getAttributesXml(Int32 intGroupId, String searchValue, Int32 level, Boolean getComplexChildren, Int32 localeId);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        Boolean ComplexMetadataRollBack(Int32 versionId, Int32 cNodeId, Int32 attributeId);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        DataSet GetAttributeDataForLookupControl(String complexTableName, String lookUpColumnName, Int32 isCheckingOnly);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        string GetCategoriesFromRuleXML(String txtRulesXML);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        DataTable QuickSearchByShortName(Int32 catalogId, String searchValue, Int32 cnodeId, String userLogin);
        #endregion

        #region Common
        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        DataSet GetObject(String objectValue, Int32 objectId, String searchValue, Int32 topRows);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        DataTable ObjectGet(String objectValue, Int32 objectId, String searchValue, Int32 topRows);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        DataTable GetStatusesByType(Int32 statusType);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        DataTable GetItemMetaDataActions();

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        DataTable LookupSearch(String objectValue, String tableName, String searchValue, Int32 localeId);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        DataTable LookupRuleDataSearch(String objectValue, String tableName, String searchValue);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        DataTable GetUserConfig(Int32 securityUserId, Int32 orgId, Int32 userConfigTypeId, String userConfigShortName, Boolean getFromAdmin);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        DataTable ObjectDependency(String objectName, Int32 objectId);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        DataTable GetFileDetails(Int32 fileId);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        DataTable GetFile(Int32 fileId, String chrDetailsOnly);

        #endregion

        #region ImportExport

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        Collection<UserAction> GetImportPermission();

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        Int32 SaveProfile(String name, String domain, String textProfile, String fileType, Int32 profileId, Int32 orgId, Int32 catalogId, String userId, String programName);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        Int32 DeleteProfile(String profileId, String domain);


        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        DataTable GetSubscribers(Int32 subscriberId, String subscriberName, String searchStr, Boolean inBound, Boolean outBound);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        DataTable GetProfiles(String user, Int32 profilesId, String profileName, Int32 profileTypeId, String typeShortName, String searchStr, Boolean bitIncludeData);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        DataTable GetChannels();

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        DataTable GetCatalogCharacteristicTemplate(Int32 catalogId, Int32 cNodeId, Int32 localeId, Boolean includeComplexAttrChildren);

        #endregion

        #region Administration
        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        String GetRelTypesXML();
        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        DataSet GetCatalogNodeTypeAttr(Int32 catalogId, Int32 nodeTypeId, Int32 localeId);
        #endregion

        #region RegularExpression
        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        DataTable GetAttributeRegEx(Int32 attribute);
        #endregion

        #region Taxonomy
        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        DataTable GetTechSpecsMap(Int32 intCategoryId, Int32 intLocaleId);
        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        DataTable GetTaxonomyByCatalog(Int32 catalogId);
        #endregion

        #region Util
        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        DataTable EstimateChanges(String changeProgram, String cNodeList);
        #endregion

        #region AttributeModel
        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        String GetCatalogNodeTypeAttrbiuteAsXml(Int32 organizationId, Int32 catalogId, String nodeType, Int32 branchLevel, Boolean includeComplexAttrChildren, Boolean excludeableSearchable, Collection<LocaleEnum> locales, LocaleEnum systemDataLocale);
        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        Collection<AttributeModel> GetAllCommonAttributes(int localeId);
        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        Collection<AttributeModel> GetAllTechnicalAttributes(int localeId);
        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        Collection<AttributeModel> GetCommonAttributesByContainerAndEntityType(Int32 catalogId, Int32 entityTypeId, Int32 localeId);
        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        Collection<AttributeModel> GetTechAttributesByTaxonomyAndCategory(int categoryId, int taxonomyId, int localeId);
        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        Collection<AttributeGroup> GetByAttributeType(Int32 common, Int32 technical, Int32 relationship, Collection<LocaleEnum> locales, LocaleEnum systemLocale);
        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        Collection<AttributeModel> GetByAttributeGroup(int attributeGroupId, Collection<LocaleEnum> locales);

        #endregion

        #region Search Entity (Entity Explorer Search)

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        DataSet SearchEntities(SearchCriteria searchCriteria, SearchContext searchContext, String totalCount, OperationResult searchOperationResult, CallerContext callerContext);

        #endregion

        #region Syndication Export

        //[OperationContract]
        //[FaultContract(typeof(MDMExceptionDetails))]
        //Collection<ExportProfile> GetProfilePermissions(String objectType, String type, MDMCenterApplication application);

        //[OperationContract]
        //[FaultContract(typeof(MDMExceptionDetails))]
        //Collection<ExportProfile> GetProfileGroupPermissions(String profileType, String type, MDMCenterApplication application);

        //[OperationContract]
        //[FaultContract(typeof(MDMExceptionDetails))]
        //Collection<ExportProfile> GetProfileBasedonType(Int32 profileId, String profileName, Int32 profileTypeId, String type, String filter, Boolean isInclue, MDMCenterApplication application);

        //[OperationContract]
        //[FaultContract(typeof(MDMExceptionDetails))]
        //Int32 IsExportProfileExists(Int32 profileId, String profileName, String type, MDMCenterApplication application);

        #endregion

        #region EntityOperationBL
        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        String GetAffectedInfo(Collection<Int64> entityIds, String attributeIdList, Int32 containerId, Int32 currentDataLocaleId, Int32 systemDataLocaleId);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        String GetWorkflowStatusXml(String entityIdList, Int32 activityId, String workflowType, Int32 userId);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        Collection<MDM.BusinessObjects.Entity> GetMDLsByIdList(String entityIdList, Int64 categoryId, Int32 containerId);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        Dictionary<String, Object> GetImpactedEntities(Int32 localeId, Int64 cnodeId, Int32 catalogId, Int32 attributeId, Int32 toTotalImpacted, Int32 totalAffected, Int32 maxReturnCount);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        String GetParent(Int64 cnodeId, Int64 cnodeParentId, Int32 containerId, Boolean bitUseDraftTax, Int32 dataLocale);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        String GetAttributeValuesForMDLs(Int32 extSystemId, String inputXML, String coreAttrList, String techAttrList, Int32 localeId, String programName, Boolean includeInheritedValue, Boolean computeInheritedValues);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        Collection<Entity> GetMDL(Int64 cnodeId, String catalogIdList, String delimiter, Boolean returnSelf);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        List<Entity> GetParentOPBL(Int64 cnodeId, Int64 cnodeParentId, Int32 containerId, Int32 dataLocale);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        Collection<Entity> GetMDLBasedOnConfig(Int64 cnodeId, String dataConfigXML, String catalogIdList, String delimiter, Boolean returnSelf);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        Collection<MDM.BusinessObjects.Workflow.WorkflowActivity> GetWorkflowTasks(Int64 cNodeId, Int32 userId);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        Boolean Reclassify(String dataXml, String userName, Boolean isCategoryReclassify);
        #endregion

        #region JobManager
        #region JobBL
        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        Boolean UpdateUserAction(Int32 jobId, JobAction jobAction, String jobType, MDMCenterApplication application);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        String GetChildJobsInXml(int parentJobId, MDMCenterApplication application);
        #endregion

        #region JobSchedule
        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        Tuple<Collection<Job>, Collection<ExportProfile>> GetSchedule(Int32 scheduleId, String jobType, MDMCenterApplication application);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        Boolean AddSchedule(String scheduleName, String scheduleLongName, String content, Boolean isEnable, String profiles, String programName, String jobType, MDMCenterApplication application);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        Boolean UpdateSchedule(Int32 scheduleId, String scheduleName, String scheduleLongName, String content, Boolean isEnable, String profiles, String programName, String jobType, MDMCenterApplication application);
        #endregion

        #endregion

        #region BusinessRuleBL

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        Collection<BusinessRule> GetBusinessRulesById(IEnumerable<Int32> ruleIds);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        Collection<MDM.BusinessObjects.BusinessRule> GetBusinessRulesByUserId(String loginUser);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        Boolean ProcessBusinessRules(Collection<BusinessRule> businessRules, String loginUser, String programName, String action);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        Collection<BusinessRule> GetBusinessRulesByContext(Int32 eventSourceID, Int32 eventSubscriberID, Int32 loginUserID, Int32 loginUserRole, Int32 orgID, Int32 containerID, Int32 entityTypeID, String businessRuleTypeIDs);
        #endregion

        #region BusinessRuleAttributeMappingBL

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        Collection<BusinessRuleAttributeMapping> GetBusinessRuleAttributeMappingsByRuleId(Int32 businessRuleId);


        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        Boolean ProcessBusinessRuleMappings(Collection<BusinessRuleAttributeMapping> businessRuleAttributeMappings, String ViewId);

        #endregion

        #region BusinessRuleViewContextBL

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        Boolean ProcessBusinessRuleContext(Collection<BusinessRuleSetRule> businessRuleSetRules, String loginUser, String programName, String action);

        #endregion
        #region Lookup
        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        DataTable GetTableStructure(String tableName, Boolean getFromSysObj);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        DataTable GetTableTypeTemplate(Int32 TableType);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        DataTable GetTableTypes();

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        DataTable GetTableNames(Int32 objectType, String searchString, Boolean getFromSysObj, Boolean getAttrCountColumn);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        DataTable GetRefTableData(String tableName, String refColumnName, String refMask, String displayColumns, String sortOrder, String searchColumns);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        DataTable GetNormalizationJobResults(Int32 jobServiceId);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        DataTable GetTranslationMemory(String origLocale, String transLocale, String origText);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        string GetCNode(Int32 catalogId, Int64 cnodeId, String viewPath);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        DataTable GetRelationshipCardinality(Int32 catalogId, Int32 nodeTypeFrom, Int32 relationshipType);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        Int32 ProcessRelationshipCardinality(String txtXML, String userName, String programName, Int32 returnValue);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        Int32 ProcessRelationshipTypeEntityTypeCardinality(String txtXML, String userName, String programName, Int32 returnValue);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        DataTable GetRelationshipTypeEntityTypeCardinality(Int32 nodeTypeFrom, Int32 relationshipType);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        Boolean ProcessTranslationMemory(String origLocale, String transLocale, String OrigText, String transText, String moduser, Int32 returnValue);
        #endregion Lookup

        #region Container RelationshipType EntityType Mapping

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        Collection<ContainerRelationshipTypeEntityTypeMapping> GetContainerRelationshipTypeEntityTypeMappingsByCnodes(String user, String cnodeGroupIds, Int32 catalogId);

        #endregion  Container RelationshipType EntityType Mapping

        #region Dynamic Table Schema

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        OperationResult ProcessDynamicTableSchema(DBTable dbTable, DynamicTableType dynamicTableType, CallerContext callerContext);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        OperationResult ProcessDynamicTableSchemas(DBTableCollection dbTables, DynamicTableType dynamicTableType, CallerContext callerContext);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        OperationResult GetDynamicTableSchema(Int32 id, DynamicTableType dynamicTableType, CallerContext callerContext);

        #endregion Dynamic Table Schema

        #region Old Catalog = Entity

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        String GetCoreAttrXml(Int32 localeID, Int32 customerID, Int64 CNodeID, Int64 CNodeParentID, Int32 attrGroupID, Int32 catalogID, Int32 orgID, String user,
                    Int32 backupLocaleID, String viewPath, Boolean useDraftTaxonomy, Boolean encodeCollections, String attrIDList);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        String GetTechAttrXml(Int32 CNodeID, Int32 CNodeParentID, Int32 catalogID, Int32 attrGroupID, Int32 localeID, Int32 customerID,
            String user, Int32 backupLocaleID, String viewPath, Boolean useDraftTaxonomy, Boolean encodeCollections, String attrIDList);

        #endregion
    }
}