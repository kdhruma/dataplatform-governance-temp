using MDM.BusinessObjects.Exports;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.ServiceModel;
using System.Text.RegularExpressions;
using System.Xml;

namespace MDM.WCFServices
{
    using MDM.AttributeModelManager.Business;
    using MDM.BusinessObjects;
    using MDM.BusinessObjects.DynamicTableSchema;
    using MDM.BusinessObjects.Jobs;
    using MDM.Core;
    using MDM.DataModelManager.Business;
    using MDM.EntityManager.Business;
    using MDM.HierarchyManager.Business;
    using MDM.JobManager.Business;
    using MDM.KnowledgeManager.Business;
    using MDM.RelationshipManager.Business;
    using MDM.SearchManager.Business;
    using MDM.Utility;
    using MDM.WCFServiceInterfaces;
    using SP = Riversand.StoredProcedures;
    using MDM.AdminManager.Business;
    using MDM.OrganizationManager.Business;
    using MDM.ContainerManager.Business;
    using MDM.CategoryManager.Business;
    using MDM.CacheManager.Business;

    [DiagnosticActivity]
    [ServiceBehavior(Namespace = "http://wcfservices.riversand.com", InstanceContextMode = InstanceContextMode.PerCall)]
    public class LegacyDataAccessService : MDMWCFBase, ILegacyDataAccessService
    {
        #region private members

        /// <summary>
        /// Specifies distributed cache
        /// </summary>
        private ICache _distributedCache = null;

        /// <summary>
        /// Field denotes an instance of the CacheSynchronizationHelper.
        /// </summary>
        //private UserCacheHelper _cacheHelper = null;

        /// <summary>
        /// Is Distributed cache notification enabled
        /// </summary>
        private Boolean _isDistributedCacheWithNotificationEnabled = false;

        #endregion

        #region Constructors

        public LegacyDataAccessService()
            : base(true)
        {

        }

        public LegacyDataAccessService(Boolean loadSecurityPrincipal)
            : base(loadSecurityPrincipal)
        {

        }

        #endregion

        #region Methods

        #region Private Methods

        /// <summary>
        /// 
        /// </summary>
        private void Init()
        {
            _isDistributedCacheWithNotificationEnabled = MDM.Utility.AppConfigurationHelper.IsDistributedCacheWithNotificationEnabled;

            if (_isDistributedCacheWithNotificationEnabled)
            {
                _distributedCache = CacheFactory.GetDistributedCache();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="methodCall"></param>
        /// <returns></returns>
        private void MakeMethodCall(Action methodCall)
        {
            try
            {
                methodCall();
            }
            catch (Exception ex)
            {
                LogException(ex);
                throw WrapException(ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="methodCall"></param>
        /// <returns></returns>
        private TResult MakeMethodCall<TResult>(Func<TResult> methodCall)
        {
            TResult resultValue = default(TResult);

            try
            {
                resultValue = methodCall();
            }
            catch (Exception ex)
            {
                LogException(ex);
                throw WrapException(ex);
            }

            return resultValue;
        }

        #endregion

        #region Public Methods

        #region Application Config

        public DataTable GetApplicationConfigData(Int32 eventSourceId, Int32 eventSubscriberId, Int32 securityRoleId, Int32 securityUserId, Int32 orgId, Int32 catalogId, Int32 categoryId, Int32 cNodeId, Int32 attributeId, Int32 nodeTypeId, Int32 relationshipTypeId, Int32 localId, Int32 applicationConfigId, String categoryPath, String objectName)
        {
            return MakeMethodCall(() => SP.Events.GetApplicationConfigXML(eventSourceId, eventSubscriberId, securityRoleId, securityUserId, orgId, catalogId, categoryId, cNodeId, attributeId, nodeTypeId, relationshipTypeId, applicationConfigId, localId, categoryPath, objectName));
        }

        // Note: The method is used only for Entity Translation Configuration Page
        public DataTable GetContextualApplicationConfigData(Int32 eventSourceId, Int32 eventSubscriberId)
        {
            DataTable allConfiguration = MakeMethodCall(() => SP.Events.GetApplicationConfigXML(1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, String.Empty, String.Empty));
            DataTable filteredConfigs = new DataTable("FilteredConfigs");
            allConfiguration.Columns.Add("OrgName", typeof(String));
            allConfiguration.Columns.Add("CatalogName", typeof(String));
            allConfiguration.Columns.Add("CategoryName", typeof(String));

            foreach (System.Data.DataColumn col in allConfiguration.Columns)
            {
                filteredConfigs.Columns.Add(col.ColumnName, col.DataType);
            }

            foreach (DataRow row in allConfiguration.Rows)
            {
                if (row["FK_Event_Subscriber"].ToString().Equals(eventSubscriberId.ToString(), StringComparison.OrdinalIgnoreCase) && row["FK_Event_Source"].ToString().Equals(eventSourceId.ToString(), StringComparison.OrdinalIgnoreCase))
                {
                    Int32 orgId = ValueTypeHelper.Int32TryParse(row["FK_Org"].ToString(), 0);
                    Int32 catalogId = ValueTypeHelper.Int32TryParse(row["FK_Catalog"].ToString(), 0); ;
                    Int64 categoryId = ValueTypeHelper.Int64TryParse(row["FK_Category"].ToString(), 0); ;

                    OrganizationBL orgManager = new OrganizationBL();
                    ContainerBL containerManager = new ContainerBL();

                    OrganizationContext orgContext = new OrganizationContext();
                    orgContext.LoadAttributes = false;

                    CallerContext callerContext = new CallerContext(MDMCenterApplication.MDMCenter, MDMCenterModules.TMSConnector);

                    row["OrgName"] = orgId > 0 ? orgManager.GetById(orgId, orgContext, callerContext).LongName : String.Empty;
                    if (catalogId > 0)
                    {
                        Container catalog = new ContainerBL().GetById(catalogId);
                        row["CatalogName"] = catalog.LongName;
                        row["CategoryName"] = categoryId > 0 ? new CategoryBL().GetById(catalog.HierarchyId, categoryId, callerContext).LongName : String.Empty;
                    }
                    filteredConfigs.ImportRow(row);
                }
            }

            return filteredConfigs;
        }

        public Int32 UpdateApplicationConfigXML(Int32 FK_Application_ContextDefinition, Int32 FK_Application_ConfigParent,
                    String ShortName, String LongName, Int32 FK_Event_Source, Int32 FK_Event_Subscriber, Int32 FK_Org, Int32 FK_Catalog,
                    Int32 FK_Category, Int32 FK_CNode, Int32 FK_Attribute, Int32 FK_NodeType, Int32 FK_RelationshipType, Int32 FK_Security_Role,
                    Int32 FK_Security_user, String ConfigXML, String Description, String PreCondition, String PostCondition, String XSDSchema,
                    String SampleXML, String loginUser, String userProgram, Int32 FK_Locale)
        {
            SqlInt32 returnValue = 0;
            MakeMethodCall(() => SP.Events.UpdateApplicationConfigXML(FK_Application_ContextDefinition, FK_Application_ConfigParent > 0 ? new SqlInt32(FK_Application_ConfigParent) : SqlInt32.Null,
                         ShortName, LongName, FK_Event_Source, FK_Event_Subscriber, FK_Org, FK_Catalog,
                         FK_Category, FK_CNode, FK_Attribute, FK_NodeType, FK_RelationshipType, FK_Security_Role,
                         FK_Security_user, ConfigXML, Description, PreCondition, PostCondition, XSDSchema,
                         SampleXML, loginUser, userProgram, FK_Locale, out returnValue));
            return returnValue.Value;
        }

        #endregion

        #region Misc

        /// <summary>
        /// <param name="searchCriteriaID">Indicates an searchCriteriaID</param>
        /// <param name="loginId">Indicates an loginId</param>
        /// <param name="catalogId">Indicates an catalogId</param>
        /// </summary>
        /// <returns>Datatable</returns>
        public DataTable GetSearchCriterias(Int32 searchCriteriaID, Int32 loginId, Int32 catalogId)
        {
            try
            {
                return SP.Catalog.GetSearchCriterias(searchCriteriaID, loginId, catalogId);
            }
            catch (Exception ex)
            {
                this.LogException(ex);
                throw WrapException(ex);
            }
        }

        /// <summary>

        /// </summary>
        /// <returns>Datatable</returns>
        public DataTable GetCategoryNavPanel(Int32 catalogID, String sysAttrSelectionXml, String categorySearchColumn, String categorySearchString, Int64 parentCategoryId, Int32 countFrom, Int32 countTo, String strvchrUserLogin, Int32 currentDataLocale)
        {
            try
            {
                return SP.Catalog.GetCategoryNavPanel(catalogID, sysAttrSelectionXml, categorySearchColumn, categorySearchString, parentCategoryId,
                                                    countFrom, countTo, strvchrUserLogin, currentDataLocale);
            }
            catch (Exception ex)
            {
                this.LogException(ex);
                throw WrapException(ex);
            }
        }

        /// <summary>

        /// </summary>
        /// <returns>int Criteria Id</returns>
        public int ProcessSearchCriteria(String action, Int32 criteriaId, String criteriaName, Int32 loginId, Int32 catalogId, Boolean isGlobalSearch, String searchXml, String loginUser, String userProgram)
        {
            try
            {
                SqlInt32 newCriteriaId = new SqlInt32();
                SP.Catalog.ProcessSearchCriteria(action, criteriaId, criteriaName, loginId, catalogId, isGlobalSearch, searchXml, loginUser, userProgram, out newCriteriaId);
                return newCriteriaId.Value;
            }
            catch (Exception ex)
            {
                this.LogException(ex);
                throw WrapException(ex);
            }

        }

        /// <summary>

        /// </summary>
        /// <returns>Datatable</returns>
        public DataTable GetAllCategories(int altTaxId, int currentDataLocale, String filter, String loginUser)
        {
            try
            {
                return SP.Catalog.GetAllCategories(altTaxId, currentDataLocale, filter, loginUser);
            }
            catch (Exception ex)
            {
                this.LogException(ex);
                throw WrapException(ex);
            }
        }

        /// <summary>

        /// </summary>
        /// <returns>String</returns>
        public String GetSystemAttributes(int orgId, Boolean excludeSearchable, Int32 currentDataLocale)
        {
            try
            {
                return SP.Attribute.GetSystemAttributes(orgId, excludeSearchable, currentDataLocale);
            }
            catch (Exception ex)
            {
                this.LogException(ex);
                throw WrapException(ex);
            }
        }

        /// <summary>

        /// </summary>
        /// <returns>String</returns>
        public String GetCharacteristicTemplate(Int32 selectedCategoryId, Int32 catalogID, Int32 currentDataLocale, Boolean useDraftAccess, Int32 usesChilds, Int32 orgID, Boolean excludeSearchable)
        {
            try
            {
                return SP.Catalog.GetCharacteristicTemplate(selectedCategoryId, catalogID, currentDataLocale, useDraftAccess, usesChilds, orgID, excludeSearchable);
            }
            catch (Exception ex)
            {
                this.LogException(ex);
                throw WrapException(ex);
            }
        }

        /// <summary>

        /// </summary>
        /// <returns>DataTable</returns>
        public DataTable GetBusinessRuleFromContext(String strContentXml)
        {
            try
            {
                return SP.Rules.GetBusinessRuleFromContext(strContentXml);
            }
            catch (Exception ex)
            {
                this.LogException(ex);
                throw WrapException(ex);
            }
        }

        /// <summary>

        /// </summary>
        /// <returns>String</returns>
        public DataTable GetRuleViewAttributes(Int32 cNodeId, Int32 businessRuleId)
        {
            try
            {
                return SP.Rules.GetRuleViewAttributes(cNodeId, businessRuleId);
            }
            catch (Exception ex)
            {
                this.LogException(ex);
                throw WrapException(ex);
            }
        }

        /// <summary>

        /// </summary>
        /// <returns>int</returns>
        public Int32 DeleteSearchCriteria(Int32 searchCriteriaId, String loginUser, String modProgram)
        {
            try
            {
                SqlInt32 returnVal = new SqlInt32();
                SP.Catalog.DeleteSearchCriteria(searchCriteriaId, loginUser, modProgram, out returnVal);
                return returnVal.Value;
            }
            catch (Exception ex)
            {
                this.LogException(ex);
                throw WrapException(ex);
            }
        }

        /// <summary>

        /// </summary>
        /// <returns>DataTable</returns>
        public DataTable GetUserVisibleOrgsDT(String targetLoginUser, String loginUser, Int32 localId, Int32 countFrom, Int32 countTo, String sortColumn, String searchColumn, String searchParameters)
        {
            try
            {
                return SP.Organization.GetUserVisibleOrgsDT(targetLoginUser, loginUser, localId, countFrom, countTo, sortColumn,
                                                        searchColumn, searchParameters);
            }
            catch (Exception ex)
            {
                this.LogException(ex);
                throw WrapException(ex);
            }
        }

        /// <summary>

        /// </summary>
        /// <returns>DataTable</returns>
        public DataTable GetUserVisibleCatalogsDT(String targetLoginUser, String loginUser, Int32 orgId, Int32 localeId, Int32 countFrom, Int32 countTo, String sortColumn, String searchColumn, String searchParameter, Int32 catalogId, Boolean includeTaxonomy, Boolean includeDynamicTaxonomy, Boolean includeCatalog, Boolean includeView, Boolean includeProduction, Boolean includeDraft)
        {
            try
            {
                return SP.Catalog.GetUserVisibleCatalogsDT(targetLoginUser, loginUser, orgId, localeId, countFrom, countTo, sortColumn,
                                                         searchColumn, searchParameter, catalogId, includeTaxonomy, includeDynamicTaxonomy, includeCatalog,
                                                         includeView, includeProduction, includeDraft);
            }
            catch (Exception ex)
            {
                this.LogException(ex);
                throw WrapException(ex);
            }
        }

        /// <summary>

        /// </summary>
        /// <returns>Table</returns>
        public DataTable GetAssignmentButtons(String assignmentStatus, String cNodeList, String toolbarButtonXml, String vchrUserLogin)
        {
            try
            {
                return SP.Catalog.GetAssignmentButtons(assignmentStatus, cNodeList, toolbarButtonXml, vchrUserLogin);
            }
            catch (Exception ex)
            {
                this.LogException(ex);
                throw WrapException(ex);
            }
        }
        
        /// <summary>

        /// </summary>
        /// <returns>void</returns>
        public int ProcessServiceResult(Int32 eventSource, Int32 applicationConfig, String dataXmlString, String loginUser, String userProgram)
        {
            int successStatus = 0;
            try
            {

                System.Data.SqlTypes.SqlXml sqldataXml = null;
                using (System.IO.StringReader s = new System.IO.StringReader(dataXmlString))
                {
                    sqldataXml = new System.Data.SqlTypes.SqlXml(XmlReader.Create(s));
                }

                SP.Job.ProcessServiceResult(eventSource, applicationConfig, sqldataXml, loginUser, userProgram);
                successStatus = 1;
            }
            catch (Exception ex)
            {
                this.LogException(ex);
                throw WrapException(ex);

            }
            return successStatus;
        }

        /// <summary>

        /// </summary>
        /// <returns>DataTable</returns>
        public DataTable GetJobTypeEventSourceMapping(Int32 eventSourceId, Int32 jobId)
        {
            return MakeMethodCall(() => SP.Job.GetJobTypeEventSourceMapping(eventSourceId, jobId));
        }

        #endregion

        #region Catalog

        public DataTable GetRelationshipTypeHierarchy(int FK_Catalog, int FK_RelationshipType_Top, int MaxLevel)
        {
            return MakeMethodCall(() => SP.Catalog.GetRelationshipTypeHierarchy(FK_Catalog, FK_RelationshipType_Top, MaxLevel));
        }

        public string ExtractCatalogByIDLocalRel(int intExtSystemID, string txtXML, string vchrRelAttrList)
        {
            return MakeMethodCall(() => SP.Catalog.ExtractCatalogByIDLocalRel(intExtSystemID, txtXML, vchrRelAttrList));
        }

        public string ExtractBulkAttributeMetadata(string vchrTargetUserLogin, string vchrUserLogin, string txtXML, bool bitUseDraftTax, int localeId, bool ignoreComplexAttributes)
        {
            return MakeMethodCall(() => SP.Catalog.ExtractBulkAttributeMetadata(vchrTargetUserLogin, vchrUserLogin, txtXML, bitUseDraftTax, localeId, ignoreComplexAttributes));
        }

        public string ExtractBulkAttributeMetadataRel(string vchrTargetUserLogin, string vchrUserLogin, string txtXML, bool bitUseDraftTax, int localeId)
        {
            return MakeMethodCall(() => SP.Catalog.ExtractBulkAttributeMetadataRel(vchrTargetUserLogin, vchrUserLogin, txtXML, bitUseDraftTax, localeId));
        }

        public string ExtractAttributes(string vchrTargetUserLogin, string vchrUserLogin, string txtXML, bool bitUseDraftTax)
        {
            return MakeMethodCall(() => SP.Catalog.ExtractAttributes(vchrTargetUserLogin, vchrUserLogin, txtXML, bitUseDraftTax));
        }

        public DataTable GetCategoryAttributeMap(int intCategoryID, int intCatalogID, int intLocaleID)
        {
            return MakeMethodCall(() => SP.Catalog.GetCategoryAttributeMap(intCategoryID, intCatalogID, intLocaleID));
        }

        public DataTable GetCharacteristicTemplateDT(int intCategoryID, int intLocaleID)
        {
            return MakeMethodCall(() => SP.Catalog.GetCharacteristicTemplateDT(intCategoryID, intLocaleID));
        }

        public string GetUserVisibleCatalogs(string vchrTargetUserLogin, string vchrUserLogin, int FK_Org, int FK_Locale, int intCountFrom, int intCountTo, string vchrSortColumn, string vchrSearchColumn, string vchrSearchParameter, int PK_Catalog, bool IncludeTaxonomy, bool IncludeDynamicTaxonomy, bool IncludeCatalog, bool IncludeView, bool IncludeProduction, bool IncludeDraft)
        {
            return MakeMethodCall(() => SP.Catalog.GetUserVisibleCatalogs(vchrTargetUserLogin, vchrUserLogin, FK_Org, FK_Locale, intCountFrom, intCountTo, vchrSortColumn, vchrSearchColumn, vchrSearchParameter, PK_Catalog, IncludeTaxonomy, IncludeDynamicTaxonomy, IncludeCatalog, IncludeView, IncludeProduction, IncludeDraft));
        }

        public string GetCatalogPermissionsByOrg(string vchrTargetUserLogin, string vchrUserLogin, int FK_Org, int FK_Locale, int intCountFrom, int intCountTo, string vchrSortColumn, string vchrSearchColumn, string vchrSearchParameter, int PK_Catalog, bool IncludeTaxonomy, bool IncludeDynamicTaxonomy, bool IncludeCatalog, bool IncludeView, bool IncludeProduction, bool IncludeDraft)
        {
            return MakeMethodCall(() => SP.Catalog.GetCatalogPermissionsByOrg(vchrTargetUserLogin, vchrUserLogin, FK_Org, FK_Locale, intCountFrom, intCountTo, vchrSortColumn, vchrSearchColumn, vchrSearchParameter, PK_Catalog, IncludeTaxonomy, IncludeDynamicTaxonomy, IncludeCatalog, IncludeView, IncludeProduction, IncludeDraft));
        }

        public DataTable GetCatalogDT(string vchrTargetUserLogin, string vchrUserLogin, int FK_Org, int FK_Locale, int intCountFrom, int intCountTo, string vchrSortColumn, string vchrSearchColumn, string vchrSearchParameter, int PK_Catalog, bool IncludeTaxonomy, bool IncludeDynamicTaxonomy, bool IncludeCatalog, bool IncludeView, bool IncludeProduction, bool IncludeDraft)
        {
            return MakeMethodCall(() => SP.Catalog.GetCatalogDT(vchrTargetUserLogin, vchrUserLogin, FK_Org, FK_Locale, intCountFrom, intCountTo, vchrSortColumn, vchrSearchColumn, vchrSearchParameter, PK_Catalog, IncludeTaxonomy, IncludeDynamicTaxonomy, IncludeCatalog, IncludeView, IncludeProduction, IncludeDraft));
        }

        public string GetCatalogsByOrg(string orgId, string vchrTargetUserLogin, string vchrUserLogin)
        {
            return MakeMethodCall(() => SP.Catalog.GetCatalogsByOrg(orgId, vchrTargetUserLogin, vchrUserLogin));
        }

        public DataTable ProcessCatalogs(string txtXML, int PK_Org, string vchrUserLogin)
        {
            return MakeMethodCall(() => SP.Catalog.ProcessCatalogs(txtXML, PK_Org, vchrUserLogin));
        }

        public string GetCatalogLocaleByID(int PK_Catalog, string vchrUserLogin)
        {
            return MakeMethodCall(() => SP.Catalog.GetCatalogLocaleByID(PK_Catalog, vchrUserLogin));
        }

        public Boolean ProcessCatalogLocales(string txtXML, int PK_Org, string vchrUserLogin)
        {
            MakeMethodCall(() => SP.Catalog.ProcessCatalogLocales(txtXML, PK_Org, vchrUserLogin));
            return true;
        }

        public string GetNodePermissions(string vchrTargetUserLogin, string vchrUserLogin, int PK_Catalog, int FK_ParentCNode, int FK_Locale, int FK_Customer, int intCountFrom, int intCountTo, string vchrSortColumn, string vchrSearchColumn, string vchrSearchParameter, int PK_CNode, bool bitIncludeComponents, bool bitEnableComponentMapping, bool bitUseDrafTax, bool bitEnableUnassignedCategory, string ToolTipAttributeList)
        {
            return MakeMethodCall(() => SP.Catalog.GetNodePermissions(vchrTargetUserLogin, vchrUserLogin, PK_Catalog, FK_ParentCNode, FK_Locale, FK_Customer, intCountFrom, intCountTo, vchrSortColumn, vchrSearchColumn, vchrSearchParameter, PK_CNode, bitIncludeComponents, bitEnableComponentMapping, bitUseDrafTax, bitEnableUnassignedCategory, ToolTipAttributeList));
        }

        public string GetNodePermissionsByCNode(string vchrTargetUserLogin, string vchrUserLogin, int PK_Catalog, int CnodeId, int FK_Locale, int FK_Customer, int intCountFrom, int intCountTo, string vchrSortColumn, string vchrSearchColumn, string vchrSearchParameter, bool bitIncludeComponents, bool bitEnableComponentMapping, bool bitUseDrafTax, bool bitEnableUnassignedCategory)
        {
            return MakeMethodCall(() => SP.Catalog.GetNodePermissionsByCNode(vchrTargetUserLogin, vchrUserLogin, PK_Catalog, CnodeId, FK_Locale, FK_Customer, intCountFrom, intCountTo, vchrSortColumn, vchrSearchColumn, vchrSearchParameter, bitIncludeComponents, bitEnableComponentMapping, bitUseDrafTax, bitEnableUnassignedCategory));
        }

        public string GetCoreAttrByGroup(int intLocaleID, int intCustomerID, long intCNodeID, long intCNodeParentID, int intGroupID, int intCatalogID, int intOrgID, string vchrUserID, int intBackLocaleID, string vchrViewPath, bool bitUseDraftTax, bool ShowAtCreation, string AttrIDList)
        {
            return MakeMethodCall(() => SP.Catalog.GetCoreAttrByGroup(intLocaleID, intCustomerID, intCNodeID, intCNodeParentID, intGroupID, intCatalogID, intOrgID, vchrUserID, intBackLocaleID, vchrViewPath, bitUseDraftTax, ShowAtCreation, AttrIDList));
        }

        public Int64 ProcessCoreAttr(string txtXML, int intCatalogID, int intOrgID, string vchrUserID, string vchrProgramName, int LocaleId)
        {
            SqlInt64 returnVal = 0;

            try
            {
                SP.Catalog.ProcessCoreAttr(txtXML, intCatalogID, intOrgID, vchrUserID, vchrProgramName, LocaleId, out returnVal);
            }
            catch (Exception ex)
            {
                LogException(ex);
                throw WrapException(ex);
            }

            return returnVal.Value;
        }

        public Int64 ProcessTechAttr(string txtXML, int intCatalogID, int intOrgID, string vchrUserID, string vchrProgramName, int LocaleId)
        {
            SqlInt64 returnVal = 0;

            try
            {
                SP.Catalog.ProcessTechAttr(txtXML, intCatalogID, intOrgID, vchrUserID, vchrProgramName, LocaleId, out returnVal);
            }
            catch (Exception ex)
            {
                LogException(ex);
                throw WrapException(ex);
            }

            return returnVal.Value;
        }

        public string GetTechAttr(int intCnodeID, int intCnodeParentID, int intCatalogID, int intGroupID, int intLocaleID, int intCustomerID, string vchrUserID, int intBackupLocaleID, string vchrViewPath, bool bitUseDraftTax, string AttrIDList)
        {
            return MakeMethodCall(() => SP.Catalog.GetTechAttr(intLocaleID, intCnodeParentID, intCatalogID, intGroupID, intLocaleID, intCustomerID, vchrUserID, intBackupLocaleID, vchrViewPath, bitUseDraftTax, AttrIDList));
        }

        public DataTable GetStatuses()
        {
            return MakeMethodCall(() => SP.Catalog.GetStatuses());
        }

        public Boolean SchemaValidationRulesExecution(int JobId, string UserID)
        {
            MakeMethodCall(() => SP.Catalog.SchemaValidationRulesExecution(JobId, UserID));
            return true;
        }

        public DataSet GetCatalogAttributes(string UserLogin)
        {
            return MakeMethodCall(() => SP.Catalog.GetCatalogAttributes(UserLogin));
        }

        public Boolean CopyCatalogAttributes(int intFromCatalogId, int intToCatalogId, string CreateProgram, string CreateUser)
        {
            MakeMethodCall(() => SP.Catalog.CopyCatalogAttributes(intFromCatalogId, intToCatalogId, CreateProgram, CreateUser));
            return true;
        }

        public DataTable GetNameValCollection(string IdList)
        {
            return MakeMethodCall(() => SP.Catalog.GetNameValCollection(IdList));
        }

        public string GetCollectionValues(int FK_CNode, int ParentId, int FK_Catalog, int FK_Customer, int FK_Locale, string InheritanceMode)
        {
            return MakeMethodCall(() => SP.Catalog.GetCollectionValues(FK_CNode, ParentId, FK_Catalog, FK_Customer, FK_Locale, InheritanceMode));
        }

        public string GetCatalogNodeTypeAttributesXML(int intOrgID, int intCatalogID, string nvchrNodeType, int intBranchLevel, bool IncludeComplexAttrChildren, bool ExcludeSearchable)
        {
            return MakeMethodCall(() => SP.Catalog.GetCatalogNodeTypeAttributesXML(intOrgID, intCatalogID, nvchrNodeType, intBranchLevel, IncludeComplexAttrChildren, ExcludeSearchable));
        }

        public string GetSearchCategoriesByCriteria(string vchrSearchValue, int intCatalogID, int intParentID, string toolTipAttributeList, string vchrUserLogin, int dataLocale)
        {
            return MakeMethodCall(() => SP.Catalog.GetSearchCategoriesByCriteria(vchrSearchValue, intCatalogID, intParentID, toolTipAttributeList, vchrUserLogin, dataLocale));
        }

        public string GetVisibleComponents(string vchrTargetUserLogin, string vchrUserLogin, int intOrgId, int intCatalogId, int intNodeId, bool bitRecursive, bool bitUseDraftTaxonomy)
        {
            return MakeMethodCall(() => SP.Catalog.GetVisibleComponents(vchrTargetUserLogin, vchrUserLogin, intOrgId, intCatalogId, intNodeId, bitRecursive, bitUseDraftTaxonomy));
        }

        public int ProcessRelAttr(string txtXML, int intCatalogID, int intOrgID, string vchrUserID, string vchrProgramName)
        {
            SqlInt32 returnVal = 0;

            try
            {
                SP.Catalog.ProcessRelAttr(txtXML, intCatalogID, intOrgID, vchrUserID, vchrProgramName, out returnVal);
            }
            catch (Exception ex)
            {
                LogException(ex);
                throw WrapException(ex);
            }

            return returnVal.Value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="configXml"></param>
        /// <returns></returns>
        public DataSet GetUserConfigMetadata(string configXml)
        {
            XmlDocument xmlDoc = new XmlDocument();
            SqlXml sqlConfigXml = null; ;
            xmlDoc.LoadXml(configXml);
            using (XmlNodeReader xnr = new XmlNodeReader(xmlDoc))
            {
                sqlConfigXml = new SqlXml(xnr);
            }
            return MakeMethodCall(() => SP.Catalog.GetUserConfigMetadata(sqlConfigXml));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="catalogId"></param>
        /// <param name="searchValue"></param>
        /// <param name="cnodeId"></param>
        /// <param name="userLogin"></param>
        /// <returns></returns>
        public DataTable QuickSearchByShortName(Int32 catalogId, String searchValue, Int32 cnodeId, String userLogin)
        {
            return MakeMethodCall(() => SP.Catalog.QuickSearch(catalogId, searchValue, cnodeId, userLogin));
        }
        #endregion

        #region Events

        /// <summary>

        /// </summary>
        /// <returns>DataTable</returns>
        public DataTable GetEventSources(int eventSourceId, string eventSourceName)
        {
            try
            {
                return SP.Events.GetEventSources(eventSourceId, eventSourceName);
            }
            catch (Exception ex)
            {

                this.LogException(ex);
                throw WrapException(ex);
            }
        }

        /// <summary>

        /// </summary>
        /// <returns>DataTable</returns>
        public DataTable GetEventSubscribers(int eventSubscriberId, string eventSubscriberName)
        {
            try
            {
                return SP.Events.GetEventSubscribers(eventSubscriberId, eventSubscriberName);
            }
            catch (Exception ex)
            {

                this.LogException(ex);
                throw WrapException(ex);
            }
        }

        /// <summary>

        /// </summary>
        /// <returns>DataTable</returns>
        public DataTable GetApplicationConfigTypes(int applicationConfigTypeId, string applicationConfigTypeName)
        {
            try
            {
                return SP.Events.GetApplicationConfigTypes(applicationConfigTypeId, applicationConfigTypeName);
            }
            catch (Exception ex)
            {

                this.LogException(ex);
                throw WrapException(ex);
            }
        }

        /// <summary>

        /// </summary>
        /// <returns>DataTable</returns>
        public DataTable GetMatchingRuleSets(int orgId, int catalogId, int eventSourceId, int eventSubscriberId, int securityUserId)
        {
            try
            {
                return SP.Events.GetMatchinRuleSets(orgId, catalogId, eventSourceId, eventSubscriberId, securityUserId);
            }
            catch (Exception ex)
            {

                this.LogException(ex);
                throw WrapException(ex);
            }
        }

        #endregion

        #region Rules

        /// <summary>

        /// </summary>
        /// <returns>DataTable</returns>
        public DataTable GetViewContext(int viewId)
        {
            try
            {
                return SP.Rules.GetViewContext(viewId);
            }
            catch (Exception ex)
            {

                this.LogException(ex);
                throw WrapException(ex);
            }
        }

        /// <summary>

        /// </summary>
        /// <returns>DataTable</returns>
        public DataSet GetViewContextDetail(int contextId, string loginUser)
        {
            try
            {
                return SP.Rules.GetViewContextDetail(contextId, loginUser);
            }
            catch (Exception ex)
            {

                this.LogException(ex);
                throw WrapException(ex);
            }
        }

        #endregion

        #region Matching

        /// <summary>

        /// </summary>
        /// <returns>DataTable</returns>
        public string CreateServicesJob(string xml, string userId, int serviceType)
        {
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                SqlXml xmlData = null; ;
                xmlDoc.LoadXml(xml);
                using (XmlNodeReader xnr = new XmlNodeReader(xmlDoc))
                {
                    xmlData = new SqlXml(xnr);
                }


                return SP.Matching.CreateServicesJob(xmlData, userId, serviceType);
            }
            catch (Exception ex)
            {

                this.LogException(ex);
                throw WrapException(ex);
            }
        }

        /// <summary>

        /// </summary>
        /// <returns>DataTable</returns>
        public DataTable GetRSPLMatchingStatus(string cNodes, int catalogId)
        {
            try
            {
                return SP.Matching.GetRSPLMatchingStatus(cNodes, catalogId);
            }
            catch (Exception ex)
            {

                this.LogException(ex);
                throw WrapException(ex);
            }
        }

        /// <summary>

        /// </summary>
        /// <returns>DataTable</returns>
        public DataTable GetMatchedCnodes(string nodeName, string DataXML)
        {

            try
            {
                SqlXml xmlData = null;
                using (System.IO.StringReader reader = new System.IO.StringReader(DataXML))
                {
                    xmlData = new SqlXml(XmlReader.Create(reader));
                    return SP.Matching.GetMatchedCnodes(nodeName, xmlData);
                }
            }
            catch (Exception ex)
            {

                this.LogException(ex);
                throw WrapException(ex);
            }
        }

        #endregion

        #region BulkOperation
        /// <summary>
        /// 
        /// </summary>
        /// <param name="targetUserLogin"></param>
        /// <param name="userLogin"></param>
        /// <param name="inputDataMode"></param>
        /// <param name="selectedNodeTypes"></param>
        /// <param name="txtXML"></param>
        /// <param name="bitUseDraftTax"></param>
        /// <returns></returns>
        public string ExtractBulkOperationAttributeMetaData(string targetUserLogin, string userLogin, string inputDataMode, string selectedNodeTypes, string txtXML, bool bitUseDraftTax)
        {
            return MakeMethodCall(() => SP.BulkOperation.ExtractBulkOperationAttributeMetaData(targetUserLogin, userLogin, inputDataMode, selectedNodeTypes, txtXML, bitUseDraftTax));
        }
        #endregion

        #region Report
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public DataTable GetAllLocales()
        {
            return MakeMethodCall(() => SP.Report.GetAllLocales());
        }
        #endregion

        #region Knowledge
        /// <summary>
        /// </summary>
        /// <returns>Collection of TimeZone</returns>
        public Collection<TimeZone> GetAll()
        {
            return MakeMethodCall(() => new TimeZoneBL().GetAll());
        }

        /// <summary>
        /// </summary>
        /// <returns>String</returns>
        public String GetUnitsXML()
        {
            return MakeMethodCall(() => new UomBL().GetAll());
        }
        #endregion

        #region Relationship

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityId"></param>
        /// <param name="entityContext"></param>
        /// <param name="callerContext"></param>
        /// <param name="publishEvents"></param>
        /// <param name="applyAVS"></param>
        /// <returns></returns>
        public String GetEntityRelationshipDetails(Int64 entityId, EntityContext entityContext, CallerContext callerContext, Boolean publishEvents = true, Boolean applyAVS = true)
        {
            if (Constants.PERFORMANCE_TRACING_ENABLED || Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.StartTraceActivity("LegacyDataAccessService.GetEntityRelationshipDetails", MDMTraceSource.RelationshipGet, false);
            }

            String relationshipDetails = String.Empty;
            DurationHelper durationHelper = new DurationHelper(DateTime.Now);

            try
            {
                if (Constants.TRACING_ENABLED)
                {
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "LegacyDataAccessService has received 'GetEntityRelationshipDetails' request message.", MDMTraceSource.RelationshipGet);
                }

                //Check whether Relationship Denorm is needed before getting details...
                //This is needed only when second level relationships are requested...
                //if (entityContext != null && entityContext.RelationshipContext != null && entityContext.RelationshipContext.Level > 1)
                //{
                //    //Request is for second level relationship get...
                //    //Try to process relationship tree denorm.. If not done.. Based on settings...
                //    RelationshipDenormBL relationshipDenormBL = new RelationshipDenormBL();
                //    relationshipDenormBL.EnsureInheritedEntityRelationships(new Collection<Int64>() { entityId }, entityContext, false, false, false,
                //                true, false, true, new EntityBL(), null,
                //        callerContext);
                //}

                EntityBL entityManager = new EntityBL();
                Entity entityObject = entityManager.Get(entityId, entityContext, callerContext.Application, callerContext.Module, publishEvents, applyAVS);

                if (Constants.PERFORMANCE_TRACING_ENABLED || Constants.TRACING_ENABLED)
                {
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("{0}ms - WCF: Total time taken to load entity with relationships for entity Id: {1}", durationHelper.GetDurationInMilliseconds(DateTime.Now), entityId), MDMTraceSource.APIFramework);
                }

                relationshipDetails = GetEntityXml(entityObject);

                if (Constants.TRACING_ENABLED)
                {
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "LegacyDataAccessService has sent 'GetEntityRelationshipDetails' response message.", MDMTraceSource.RelationshipGet);
                }
            }
            catch (Exception ex)
            {
                this.LogException(ex);
                throw base.WrapException(ex);
            }
            finally
            {
                if (Constants.PERFORMANCE_TRACING_ENABLED || Constants.TRACING_ENABLED)
                {
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("{0}ms - WCF with serialization: Total time taken to load entity with relationships for entity Id: {1}", durationHelper.GetDurationInMilliseconds(DateTime.Now), entityId), MDMTraceSource.RelationshipGet);

                    MDMTraceHelper.StopTraceActivity("LegacyDataAccessService.GetEntityRelationshipDetails", MDMTraceSource.RelationshipGet);
                }
            }

            return relationshipDetails;
        }

        #endregion

        #region Hierarchy

        public Collection<Entity> GetAllCategoriesByHierarchy(int localeId, int taxonomyId, string searchParameter, int countTo)
        {
            return MakeMethodCall(() => new HierarchyBL().GetAllCategoriesByHierarchy(localeId, taxonomyId, searchParameter, countTo));

        }

        #endregion Hierarchy

        #region Security

        public String GetRoleMenus(Int32 securityRole)
        {
            return MakeMethodCall(() => SP.Security.GetRoleMenus(securityRole));
        }

        public String GetUsers(Int32 securityUser, Int32 userType, Int32 countFrom, Int32 countTo, String vchrSortColumn, String vchrSearchColumn, String vchrSearchParameter, String vchrUserLogin)
        {
            return MakeMethodCall(() => SP.Security.GetUsers(securityUser, userType, countFrom, countTo, vchrSortColumn, vchrSearchColumn, vchrSearchParameter, vchrUserLogin));
        }

        public String GetUsersWithRoles(Int32 securityUser, Int32 countFrom, Int32 countTo, String vchrSearchColumn, String vchrSearchParameter, String vchrUserLogin)
        {
            return MakeMethodCall(() => SP.Security.GetUsersWithRoles(securityUser, countFrom, countTo, vchrSearchColumn, vchrSearchParameter, vchrUserLogin));
        }

        public Boolean ProcessUsers(String txtXML, String vchrUserLogin)
        {
            MakeMethodCall(() => SP.Security.ProcessUsers(txtXML, vchrUserLogin));
            return true;
        }

        public String GetRoles(Int32 securityRole, Int32 userType, String chrGetPermissionSetOnly, Int32 intCountFrom, Int32 intCountTo, String vchrSortColumn, String vchrSearchColumn, String vchrSearchParameter, String vchrUserLogin, Boolean bitDisplaySystemRole)
        {
            return MakeMethodCall(() => SP.Security.GetRoles(securityRole, userType, chrGetPermissionSetOnly, intCountFrom, intCountTo, vchrSortColumn, vchrSearchColumn, vchrSearchParameter, vchrUserLogin, bitDisplaySystemRole));
        }

        public String GetRolesWithUsers(Int32 pkSecurityRole, String chrGetPermissionSetOnly, Int32 intCountFrom, Int32 intCountTo, String vchrSearchColumn, String vchrSearchParameter, String vchrUserLogin, Boolean bitDisplaySystemRole)
        {
            return MakeMethodCall(() => SP.Security.GetRolesWithUsers(pkSecurityRole, chrGetPermissionSetOnly, intCountFrom, intCountTo, vchrSearchColumn, vchrSearchColumn, vchrUserLogin, bitDisplaySystemRole));
        }

        public DataTable ProcessRoles(String txtXML, String vchrUserLogin)
        {
            return MakeMethodCall(() => SP.Security.ProcessRoles(txtXML, vchrUserLogin));
        }

        public Boolean ProcessRoleMenus(String txtXML, String vchrUserLogin)
        {
            MakeMethodCall(() => SP.Security.ProcessRoleMenus(txtXML, vchrUserLogin));
            return true;
        }

        public String GetObjectTypeAction()
        {
            return MakeMethodCall(() => SP.Security.GetObjectTypeAction());
        }

        public String GetHierarchyAction()
        {
            return MakeMethodCall(() => SP.Security.GetHierarchyAction());
        }

        public String GetPermissions(Int32 securityRole, String chrPermissionSet, String vchrUserLogin)
        {
            return MakeMethodCall(() => SP.Security.GetPermissions(securityRole, chrPermissionSet, vchrUserLogin));
        }

        public Boolean ProcessPermissions(String txtXML, String vchrUserLogin)
        {
            MakeMethodCall(() => SP.Security.ProcessPermissions(txtXML, vchrUserLogin));
            return true;
        }

        public String GetAllOrganizationsAndCatalogs(String vchrUserLogin)
        {
            return MakeMethodCall(() => SP.Security.GetAllOrganizationsAndCatalogs(vchrUserLogin));
        }

        public String IsSystemUser(String nvchrUserLogin, String isSystemUser)
        {
            SqlString sqlstring = new SqlString(isSystemUser);
            try
            {
                SP.Security.IsSystemUser(nvchrUserLogin, ref sqlstring);

            }
            catch (Exception ex)
            {
                LogException(ex);
                throw WrapException(ex);
            }
            return sqlstring.Value;

        }
        public Boolean HasPermission(String vchrUserLogin, String objectTypeSN, String actionSN, String parentObjectTypeSN, Int32 orgId, Int32 catalogId, Int64 categoryCNode, Int64 componentCNode, Int32 attribute, Int32 relationship, Boolean bitForDraft, Boolean bitHasPermission)
        {
            SqlBoolean sqlBool = new SqlBoolean(bitHasPermission);
            try
            {
                SP.Security.HasPermission(vchrUserLogin, objectTypeSN, actionSN, parentObjectTypeSN, orgId, catalogId, categoryCNode, componentCNode, attribute, relationship, bitForDraft, ref sqlBool);

            }
            catch (Exception ex)
            {
                LogException(ex);
                throw WrapException(ex);
            }
            return sqlBool.Value;
        }

        #endregion

        #region Organization
        /// <summary>
        /// 
        /// </summary>
        /// <param name="targetUserLogin"></param>
        /// <param name="userLogin"></param>
        /// <param name="localeId"></param>
        /// <param name="intCountFrom"></param>
        /// <param name="intCountTo"></param>
        /// <param name="sortColumn"></param>
        /// <param name="searchColumn"></param>
        /// <param name="searchParameter"></param>
        /// <returns></returns>
        public string GetUserVisibleOrgs(string targetUserLogin, string userLogin, int localeId, int intCountFrom, int intCountTo, string sortColumn, string searchColumn, string searchParameter)
        {
            return MakeMethodCall(() => SP.Organization.GetUserVisibleOrgs(targetUserLogin, userLogin, localeId, intCountFrom, intCountTo, sortColumn, searchColumn, searchParameter));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="targetUserLogin"></param>
        /// <param name="userLogin"></param>
        /// <param name="localeId"></param>
        /// <param name="intCountFrom"></param>
        /// <param name="intCountTo"></param>
        /// <param name="sortColumn"></param>
        /// <param name="searchColumn"></param>
        /// <param name="searchParameter"></param>
        /// <param name="orgParentId"></param>
        /// <param name="orgClassificationId"></param>
        /// <param name="catalogObjectType"></param>
        /// <returns></returns>
        public string GetOrgsWithPermissions(string targetUserLogin, string userLogin, int localeId, int intCountFrom, int intCountTo, string sortColumn, string searchColumn, string searchParameter, int orgParentId, int orgClassificationId, string catalogObjectType)
        {
            return MakeMethodCall(() => SP.Organization.GetOrgsWithPermissions(targetUserLogin, userLogin, localeId, intCountFrom, intCountTo, sortColumn, searchColumn, searchParameter, orgParentId, orgClassificationId, catalogObjectType));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="targetUserLogin"></param>
        /// <param name="userLogin"></param>
        /// <param name="localeId"></param>
        /// <param name="intCountFrom"></param>
        /// <param name="intCountTo"></param>
        /// <param name="sortColumn"></param>
        /// <param name="searchColumn"></param>
        /// <param name="searchParameter"></param>
        /// <param name="orgClassificationId"></param>
        /// <param name="catalogObjectType"></param>
        /// <returns></returns>
        public string GetOrgsByOrgClassification(string targetUserLogin, string userLogin, int localeId, int intCountFrom, int intCountTo, string sortColumn, string searchColumn, string searchParameter, int orgClassificationId, string catalogObjectType)
        {
            return MakeMethodCall(() => SP.Organization.GetOrgsByOrgClassification(targetUserLogin, userLogin, localeId, intCountFrom, intCountTo, sortColumn, searchColumn, searchParameter, orgClassificationId, catalogObjectType));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="orgTypeId"></param>
        /// <param name="intCountFrom"></param>
        /// <param name="intCountTo"></param>
        /// <param name="sortColumn"></param>
        /// <param name="searchColumn"></param>
        /// <param name="searchParameter"></param>
        /// <param name="userLogin"></param>
        /// <returns></returns>
        public string GetAllOrgTypes(int orgTypeId, int intCountFrom, int intCountTo, string sortColumn, string searchColumn, string searchParameter, string userLogin)
        {
            return MakeMethodCall(() => SP.Organization.GetAllOrgTypes(orgTypeId, intCountFrom, intCountTo, sortColumn, searchColumn, searchParameter, userLogin));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="orgTypeId"></param>
        /// <param name="intCountFrom"></param>
        /// <param name="intCountTo"></param>
        /// <param name="sortColumn"></param>
        /// <param name="searchColumn"></param>
        /// <param name="searchParameter"></param>
        /// <param name="userLogin"></param>
        /// <returns></returns>
        public string GetOrgTypes(int orgTypeId, int intCountFrom, int intCountTo, string sortColumn, string searchColumn, string searchParameter, string userLogin)
        {
            return MakeMethodCall(() => SP.Organization.GetOrgTypes(orgTypeId, intCountFrom, intCountTo, sortColumn, searchColumn, searchParameter, userLogin));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="txtXML"></param>
        /// <param name="userLogin"></param>
        public Boolean ProcessOrgTypes(string txtXML, string userLogin)
        {
            MakeMethodCall(() => SP.Organization.ProcessOrgTypes(txtXML, userLogin));
            return true;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string GetAllOrgHierarchies()
        {
            return MakeMethodCall(() => SP.Organization.GetAllOrgHierarchies());
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="orgClassificationId"></param>
        /// <param name="localeId"></param>
        /// <param name="userLogin"></param>
        /// <returns></returns>
        public string GetOrgsWithLocales(int orgClassificationId, int localeId, string userLogin)
        {
            return MakeMethodCall(() => SP.Organization.GetOrgsWithLocales(orgClassificationId, localeId, userLogin));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="txtXML"></param>
        /// <param name="userLogin"></param>
        public Boolean ProcessOrgLocales(string txtXML, string userLogin)
        {
            MakeMethodCall(() => SP.Organization.ProcessOrgLocales(txtXML, userLogin));
            return true;

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userLogin"></param>
        /// <returns></returns>
        public DataTable GetOrgsWithPermissionsOrg(string userLogin)
        {
            return MakeMethodCall(() => SP.Organization.GetOrgsWithPermissions(userLogin));
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="orgId"></param>
        /// <param name="catalogId"></param>
        /// <param name="findWhat"></param>
        /// <returns></returns>
        public DataTable GetOrgCatalogInfo(int orgId, int catalogId, string findWhat)
        {
            return MakeMethodCall(() => SP.Organization.GetOrgCatalogInfo(orgId, catalogId, findWhat));
        }

        #endregion

        #region Attribute

        /// <summary>
        /// 
        /// </summary>
        /// <param name="attributeId"></param>
        /// <param name="userLogin"></param>
        /// <returns></returns>
        public string GetAttributeDetails(int attributeId, string userLogin)
        {
            return MakeMethodCall(() => SP.Attribute.GetAttributeDetails(attributeId, userLogin));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public DataTable GetAllAttributes()
        {
            return MakeMethodCall(() => SP.Attribute.GetAllAttributes());
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cNodeId"></param>
        /// <param name="catalogId"></param>
        /// <param name="attributeId"></param>
        /// <param name="localeId"></param>
        /// <param name="userName"></param>
        /// <param name="returnAttrType"></param>
        /// <param name="showAtCreation"></param>
        /// <returns></returns>
        public string GetCNodeAttributeValue(long cNodeId, int catalogId, int attributeId, int localeId, string userName, int returnAttrType, bool showAtCreation)
        {
            return MakeMethodCall(() => SP.Attribute.GetCNodeAttributeValue(cNodeId, catalogId, attributeId, localeId, userName, returnAttrType, showAtCreation));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string GetAttributeGroupsXML()
        {
            return MakeMethodCall(() => SP.Attribute.GetAttributeGroupsXML());
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="attrParentId"></param>
        /// <returns></returns>
        public string GetTechSpecsByGroup(int attrParentId)
        {
            return MakeMethodCall(() => SP.Attribute.GetTechSpecsByGroup(attrParentId));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="inputStr"></param>
        /// <returns></returns>
        public DataSet ComplexMetadata_GetDT(string inputStr)
        {
            return MakeMethodCall(() => SP.Attribute.ComplexMetadata_GetDT(inputStr));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="attributeTypeId"></param>
        /// <param name="attributeId"></param>
        /// <param name="userId"></param>
        /// <param name="orgId"></param>
        /// <param name="catalogId"></param>
        /// <param name="cNodeId"></param>
        /// <param name="viewPath"></param>
        /// <returns></returns>
        public string GetAttributeChildrenByType(int attributeTypeId, string attributeId, string userId, int orgId, int catalogId, long cNodeId, string viewPath)
        {
            return MakeMethodCall(() => SP.Attribute.GetAttributeChildrenByType(attributeTypeId, attributeId, userId, orgId, catalogId, cNodeId, viewPath));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="objectType"></param>
        /// <param name="objectId"></param>
        /// <param name="localeId"></param>
        public String GetSysObjectAttributesXML(string objectType, int objectId, int localeId)
        {
            return MakeMethodCall(() => SP.Attribute.GetSysObjectAttributesXML(objectType, objectId, localeId));

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="attributeXML"></param>
        /// <param name="objectId"></param>
        /// <param name="objectType"></param>
        /// <param name="localeId"></param>
        public Boolean ProcessObjectAttributes(string attributeXML, int objectId, string objectType, int localeId)
        {
            MakeMethodCall(() => SP.Attribute.ProcessObjectAttributes(attributeXML, objectId, objectType, localeId));
            return true;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public DataSet UniqueComplxAttrid()
        {
            return MakeMethodCall(() => SP.Attribute.UniqueComplxAttrid());
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public string GetAttributeConfig(string tableName, int id)
        {
            return MakeMethodCall(() => SP.Attribute.GetAttributeConfig(tableName, id));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="attributeId"></param>
        /// <returns></returns>
        public string GetAttributeUsage(int attributeId)
        {
            return MakeMethodCall(() => SP.Attribute.GetAttributeUsage(attributeId));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="parentId"></param>
        /// <param name="localeId"></param>
        /// <param name="userLogin"></param>
        /// <returns></returns>
        public string GetAttributeGroupChildren(Int32? parentId, int localeId, string userLogin)
        {
            SqlInt32 sqlParentId = SqlInt32.Null;
            if (parentId != null)
                sqlParentId = new SqlInt32(parentId.Value);
            return MakeMethodCall(() => SP.Attribute.GetAttributeGroupChildren(sqlParentId, localeId, userLogin));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parentId"></param>
        /// <param name="countFrom"></param>
        /// <param name="countTo"></param>
        /// <param name="searchParameter"></param>
        /// <param name="searchColumn"></param>
        /// <param name="sortColumn"></param>
        /// <param name="localeId"></param>
        /// <param name="bitUnusedOnly"></param>
        /// <param name="userLogin"></param>
        /// <returns></returns>
        public string GetAttributes(int parentId, int countFrom, int countTo, string searchParameter, string searchColumn, string sortColumn, int localeId, bool bitUnusedOnly, string userLogin)
        {
            return MakeMethodCall(() => SP.Attribute.GetAttributes(parentId, countFrom, countTo, searchParameter, searchColumn, sortColumn, localeId, bitUnusedOnly, userLogin));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="attributeId"></param>
        /// <param name="cNodeId"></param>
        /// <param name="userLogin"></param>
        /// <returns></returns>
        public string GetNodeAttributeDetails(int attributeId, int cNodeId, string userLogin)
        {
            return MakeMethodCall(() => SP.Attribute.GetNodeAttributeDetails(attributeId, cNodeId, userLogin));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userLogin"></param>
        /// <returns></returns>
        public string GetAttributeDataTypeMap(string userLogin)
        {
            return MakeMethodCall(() => SP.Attribute.GetAttributeDataTypeMap(userLogin));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userLogin"></param>
        /// <returns></returns>
        public string GetAttributeDataTypes(string userLogin)
        {
            return MakeMethodCall(() => SP.Attribute.GetAttributeDataTypes(userLogin));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userLogin"></param>
        /// <returns></returns>
        public string GetAttributeDisplayTypeMap(string userLogin)
        {
            return MakeMethodCall(() => SP.Attribute.GetAttributeDisplayTypeMap(userLogin));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userLogin"></param>
        /// <returns></returns>
        public string GetAttributeDisplayTypes(string userLogin)
        {
            return MakeMethodCall(() => SP.Attribute.GetAttributeDisplayTypes(userLogin));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string GetFormattersXML()
        {
            return MakeMethodCall(() => SP.Attribute.GetFormattersXML());
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="txtXML"></param>
        /// <param name="targetAttributeId"></param>
        /// <param name="maxAllowableChars"></param>
        /// <param name="inTestPage"></param>
        /// <param name="retAttrValue"></param>
        /// <returns></returns>
        public string UpdateTargetAttributeValue(string txtXML, int targetAttributeId, int maxAllowableChars, string inTestPage, String retAttrValue)
        {


            SqlString str = (SqlString)retAttrValue;
            try
            {
                SP.Attribute.UpdateTargetAttributeValue(txtXML, targetAttributeId, maxAllowableChars, inTestPage, ref str);
            }
            catch (Exception ex)
            {
                this.LogException(ex);
                throw WrapException(ex);
            }
            return str.Value;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="intGroupId"></param>
        /// <param name="searchValue"></param>
        /// <param name="level"></param>
        /// <param name="getComplexChildren"></param>
        /// <param name="localeId"></param>
        /// <returns></returns>
        public string getAttributesXml(int intGroupId, string searchValue, int level, bool getComplexChildren, int localeId)
        {
            return MakeMethodCall(() => SP.Attribute.getAttributesXml(intGroupId, searchValue, level, getComplexChildren, localeId));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="versionId"></param>
        /// <param name="cNodeId"></param>
        /// <param name="attributeId"></param>
        public Boolean ComplexMetadataRollBack(int versionId, int cNodeId, int attributeId)
        {
            MakeMethodCall(() => SP.Attribute.ComplexMetadataRollBack(versionId, cNodeId, attributeId));
            return true;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="complexTableName"></param>
        /// <param name="lookUpColumnName"></param>
        /// <param name="isCheckingOnly"></param>
        /// <returns></returns>
        public DataSet GetAttributeDataForLookupControl(string complexTableName, string lookUpColumnName, int isCheckingOnly)
        {
            return MakeMethodCall(() => SP.Attribute.GetAttributeDataForLookupControl(complexTableName, lookUpColumnName, isCheckingOnly));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="txtRulesXML"></param>
        /// <returns></returns>
        public string GetCategoriesFromRuleXML(string txtRulesXML)
        {
            return MakeMethodCall(() => SP.Attribute.GetCategoriesFromRuleXML(txtRulesXML));
        }
        #endregion

        #region Common


        /// <summary>
        /// 
        /// </summary>
        /// <param name="objectValue"></param>
        /// <param name="objectId"></param>
        /// <param name="searchValue"></param>
        /// <param name="topRows"></param>
        /// <returns></returns>
        public DataSet GetObject(string objectValue, int objectId, string searchValue, int topRows)
        {
            return MakeMethodCall(() => SP.Common.GetObject(objectValue, objectId, searchValue, topRows));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="objectValue"></param>
        /// <param name="objectId"></param>
        /// <param name="searchValue"></param>
        /// <param name="topRows"></param>
        /// <returns></returns>
        public DataTable ObjectGet(string objectValue, int objectId, string searchValue, int topRows)
        {
            return MakeMethodCall(() => SP.Common.ObjectGet(objectValue, objectId, searchValue, topRows));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="statusType"></param>
        /// <returns></returns>
        public DataTable GetStatusesByType(int statusType)
        {
            return MakeMethodCall(() => SP.Common.GetStatusesByType(statusType));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public DataTable GetItemMetaDataActions()
        {
            return MakeMethodCall(() => SP.Common.GetItemMetaDataActions());
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="objectValue"></param>
        /// <param name="tableName"></param>
        /// <param name="searchValue"></param>
        /// <param name="localeId"></param>
        /// <returns></returns>
        public DataTable LookupSearch(string objectValue, string tableName, string searchValue, int localeId)
        {
            return MakeMethodCall(() => SP.Common.LookupSearch(objectValue, tableName, searchValue, localeId));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="objectValue"></param>
        /// <param name="tableName"></param>
        /// <param name="searchValue"></param>
        /// <returns></returns>
        public DataTable LookupRuleDataSearch(string objectValue, string tableName, string searchValue)
        {
            return MakeMethodCall(() => SP.Common.LookupRuleDataSearch(objectValue, tableName, searchValue));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="securityUserId"></param>
        /// <param name="orgId"></param>
        /// <param name="userConfigTypeId"></param>
        /// <param name="userConfigShortName"></param>
        /// <param name="getFromAdmin"></param>
        /// <returns></returns>
        public DataTable GetUserConfig(int securityUserId, int orgId, int userConfigTypeId, string userConfigShortName, bool getFromAdmin)
        {
            return MakeMethodCall(() => SP.Common.GetUserConfig(securityUserId, orgId, userConfigTypeId, userConfigShortName, getFromAdmin));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objectName"></param>
        /// <param name="objectId"></param>
        /// <returns></returns>
        public DataTable ObjectDependency(string objectName, int objectId)
        {
            return MakeMethodCall(() => SP.Common.ObjectDependency(objectName, objectId));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileId"></param>
        /// <returns></returns>
        public DataTable GetFileDetails(int fileId)
        {
            return MakeMethodCall(() => SP.Common.GetFileDetails(fileId));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileId"></param>
        /// <param name="chrDetailsOnly"></param>
        /// <returns></returns>
        public DataTable GetFile(int fileId, string chrDetailsOnly)
        {
            return MakeMethodCall(() => SP.Common.GetFile(fileId, chrDetailsOnly));
        }
        #endregion

        #region ImportExport

        public Collection<UserAction> GetImportPermission()
        {
            Collection<UserAction> userAction = null;

            try
            {
                Permission permission = null;
                DataSecurityBL dataSecurityManager = new DataSecurityBL();
                Int32 objectTypeId = (Int32)ObjectType.Catalog;
                Int32 currentUserId = SecurityPrincipalHelper.GetCurrentSecurityPrincipal().CurrentUserId;
                PermissionContext permissionContext = new PermissionContext(0, 0, 0, 0, 0, 0, 0, 0, currentUserId, 0);

                permission = dataSecurityManager.GetMDMObjectPermission(0, objectTypeId, ObjectType.Catalog.ToString(), permissionContext);

                userAction = (permission != null && permission.PermissionSet != null) ? permission.PermissionSet : new Collection<UserAction>();
            }
            catch (Exception ex)
            {
                this.LogException(ex);
                throw WrapException(ex);
            }

            return userAction;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="domain"></param>
        /// <param name="textProfile"></param>
        /// <param name="fileType"></param>
        /// <param name="profileId"></param>
        /// <param name="orgId"></param>
        /// <param name="catalogId"></param>
        /// <param name="userId"></param>
        /// <param name="programName"></param>
        /// <param name="returnValue"></param>
        public Int32 SaveProfile(string name, string domain, string textProfile, string fileType, int profileId, int orgId, int catalogId, string userId, string programName)
        {
            SqlInt32 sqlIntReturnValue = 0;
            try
            {
                SP.ImportExport.SaveProfile(name, domain, textProfile, fileType, profileId, orgId, catalogId, userId, programName, out sqlIntReturnValue);
            }
            catch (Exception ex)
            {
                this.LogException(ex);
                throw WrapException(ex);
            }

            return sqlIntReturnValue.Value;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="profileId"></param>
        /// <param name="domain"></param>
        /// <param name="returnValue"></param>
        public Int32 DeleteProfile(string profileId, string domain)
        {
            SqlInt32 sqlInt = 0;
            try
            {
                SP.ImportExport.DeleteProfile(profileId, domain, out sqlInt);
            }
            catch (Exception ex)
            {
                this.LogException(ex);
                throw WrapException(ex);
            }
            return sqlInt.Value;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="subscriberId"></param>
        /// <param name="subscriberName"></param>
        /// <param name="searchStr"></param>
        /// <param name="inBound"></param>
        /// <param name="outBound"></param>
        /// <returns></returns>
        public DataTable GetSubscribers(int subscriberId, string subscriberName, string searchStr, bool inBound, bool outBound)
        {
            return MakeMethodCall(() => SP.ImportExport.GetSubscribers(subscriberId, subscriberName, searchStr, inBound, outBound));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <param name="profilesId"></param>
        /// <param name="profileName"></param>
        /// <param name="profileTypeId"></param>
        /// <param name="typeShortName"></param>
        /// <param name="searchStr"></param>
        /// <param name="bitIncludeData"></param>
        /// <returns></returns>
        public DataTable GetProfiles(string user, int profilesId, string profileName, int profileTypeId, string typeShortName, string searchStr, bool bitIncludeData)
        {
            return MakeMethodCall(() => SP.ImportExport.GetProfiles(user, profilesId, profileName, profileTypeId, typeShortName, searchStr, bitIncludeData));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public DataTable GetChannels()
        {
            return MakeMethodCall(() => SP.ImportExport.GetChannels());
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="catalogId"></param>
        /// <param name="cNodeId"></param>
        /// <param name="localeId"></param>
        /// <param name="includeComplexAttrChildren"></param>
        /// <returns></returns>
        public DataTable GetCatalogCharacteristicTemplate(int catalogId, int cNodeId, int localeId, bool includeComplexAttrChildren)
        {
            return MakeMethodCall(() => SP.ImportExport.GetCatalogCharacteristicTemplate(catalogId, cNodeId, localeId, includeComplexAttrChildren));
        }
        #endregion

        #region Administration
        /// <summary>

        /// </summary>
        /// <returns>String</returns>
        public String GetRelTypesXML()
        {
            return MakeMethodCall(() => SP.Administration.GetRelTypesXML());
        }
        /// <summary>

        /// </summary>
        /// <returns>DataSet</returns>
        public DataSet GetCatalogNodeTypeAttr(Int32 catalogId, Int32 nodeTypeId, Int32 localeId)
        {
            return MakeMethodCall(() => SP.Administration.getCatalogNodeTypeAttr(catalogId, nodeTypeId, localeId));
        }
        #endregion

        #region RegularExpression

        /// <summary>

        /// </summary>
        /// <returns>DataTable</returns>
        public DataTable GetAttributeRegEx(Int32 attribute)
        {
            return MakeMethodCall(() => SP.RegularExpressions.GetAttributeRegEx(attribute));
        }
        #endregion

        #region Taxonomy
        /// <summary>

        /// </summary>
        /// <returns>DataTable</returns>
        public DataTable GetTechSpecsMap(Int32 intCategoryId, Int32 intLocaleId)
        {
            return MakeMethodCall(() => SP.Taxonomy.GetTechSpecsMap(intCategoryId, intLocaleId));
        }
        /// <summary>

        /// </summary>
        /// <returns>DataTable</returns>
        public DataTable GetTaxonomyByCatalog(Int32 catalogId)
        {
            return MakeMethodCall(() => SP.Taxonomy.GetTaxonomyByCatalog(catalogId));
        }
        
        #endregion

        #region Util
        /// <summary>

        /// </summary>
        /// <returns>Boolean</returns>
        public DataTable EstimateChanges(String changeProgram, String cNodeList)
        {
            return MakeMethodCall(() => SP.Util.EstimateChanges(changeProgram, cNodeList));
        }
        #endregion

        #region AttributeModel
        /// <summary>
        /// </summary>
        /// <returns>String</returns>

        public String GetCatalogNodeTypeAttrbiuteAsXml(Int32 organizationId, Int32 catalogId, String nodeType, Int32 branchLevel, Boolean includeComplexAttrChildren, Boolean excludeableSearchable, Collection<LocaleEnum> locales, LocaleEnum systemDataLocale)
        {
            return MakeMethodCall(() => new AttributeModelOperationsBL().GetCatalogNodeTypeAttrbiuteAsXml(organizationId, catalogId, nodeType, branchLevel, includeComplexAttrChildren, excludeableSearchable, locales, systemDataLocale));
        }

        /// <summary>
        /// </summary>
        /// <returns>Collection</returns>
        public Collection<AttributeModel> GetAllCommonAttributes(int localeId)
        {
            return MakeMethodCall(() => new AttributeModelOperationsBL().GetAllCommonAttributes(localeId));
        }

        /// <summary>
        /// </summary>
        /// <returns>Collection</returns>
        public Collection<AttributeModel> GetAllTechnicalAttributes(int localeId)
        {
            return MakeMethodCall(() => new AttributeModelOperationsBL().GetAllTechnicalAttributes(localeId));
        }

        /// <summary>
        /// </summary>
        /// <returns>String</returns>
        public Collection<AttributeModel> GetCommonAttributesByContainerAndEntityType(Int32 catalogId, Int32 entityTypeId, Int32 localeId)
        {
            return MakeMethodCall(() => new AttributeModelOperationsBL().GetCommonAttributesByContainerAndEntityType(catalogId, entityTypeId, localeId));
        }

        /// <summary>
        /// </summary>
        /// <returns>Collection</returns>
        public Collection<AttributeModel> GetTechAttributesByTaxonomyAndCategory(int categoryId, int taxonomyId, int localeId)
        {
            return MakeMethodCall(() => new AttributeModelOperationsBL().GetTechAttributesByTaxonomyAndCategory(categoryId, taxonomyId, localeId));
        }

        /// <summary>
        /// </summary>
        /// <returns>Collection</returns>
        public Collection<AttributeGroup> GetByAttributeType(Int32 common, Int32 technical, Int32 relationship, Collection<LocaleEnum> locales, LocaleEnum systemLocale)
        {
            return MakeMethodCall(() => new AttributeGroupBL().GetByAttributeType(common, technical, relationship, locales, systemLocale));
        }

        /// <summary>
        /// </summary>
        /// <returns>Collection</returns>
        public Collection<AttributeModel> GetByAttributeGroup(int attributeGroupId, Collection<LocaleEnum> locales)
        {
            return MakeMethodCall(() => new AttributeModelOperationsBL().GetByAttributeGroup(attributeGroupId, locales));
        }
        #endregion
        
        #region Syndication Export

        ///// <summary>
        ///// Get profile permissions.
        ///// </summary>
        ///// <param name="objectType">This parameter is specifying object Type.</param>
        ///// <param name="type">This parameter is specifying type of job.</param>
        ///// <param name="application">Name of application which is performing action</param>
        ///// <returns>collection of profile</returns>
        //public Collection<ExportProfile> GetProfilePermissions(String objectType, String type, MDMCenterApplication application)
        //{
        //    return MakeMethodCall(() => new ExportBL().GetProfilePermissions(objectType, type, application));
        //}

        ///// <summary>
        ///// Get all profile group.
        ///// </summary>
        ///// <param name="profileType">This parameter is specifying profile type.</param>
        ///// <param name="type">This parameter is specifying type of job.</param>
        ///// <param name="application">Name of application which is performing action</param>
        ///// <returns>collection of profile group.</returns>
        //public Collection<ExportProfile> GetProfileGroupPermissions(String profileType, String type, MDMCenterApplication application)
        //{
        //    return MakeMethodCall(() => new ExportBL().GetProfileGroupPermissions(profileType, type, application));
        //}

        ///// <summary>
        ///// Get the export profiles based on the profile type
        ///// </summary>
        ///// <param name="profileId">This parameter is specifying profile Id.</param>
        ///// <param name="profileName">This parameter is specifying profile name.</param>
        ///// <param name="profileTypeId">This parameter is specifying profile type id.</param>
        ///// <param name="type">This parameter is specifying profile type.</param>
        ///// <param name="filter">This parameter is specifying search value</param>
        ///// <param name="isInclue">This parameter is specifying whether it is include or not</param>
        ///// <param name="application">Name of application which is performing action</param>
        ///// <returns>collection of profiles </returns>
        //public Collection<ExportProfile> GetProfileBasedonType(Int32 profileId, String profileName, Int32 profileTypeId, String type, String filter, Boolean isInclue, MDMCenterApplication application)
        //{
        //    return MakeMethodCall(() => new ExportBL().GetProfileBasedonType(profileId, profileName, profileTypeId, type, filter, isInclue, application));
        //}

        ///// <summary>
        /////  Check for duplicate Export profile
        ///// </summary>
        ///// <param name="profileId">This parameter is specifying profile Id.</param>
        ///// <param name="profileName">This parameter is specifying profile name.</param>
        ///// <param name="type">This parameter is specifying job type.</param>
        ///// <param name="application">Name of application which is performing action</param>
        ///// <returns>'1' if duplicate profile found else '0' no duplicate profile found</returns>
        //public Int32 IsExportProfileExists(Int32 profileId, String profileName, String type, MDMCenterApplication application)
        //{
        //    return MakeMethodCall(() => new ExportBL().IsProfileExists(profileId, profileName, type, application));
        //}

        #endregion

        #region EntityOperationBL
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityIds"></param>
        /// <param name="attributeIdList"></param>
        /// <param name="containerId"></param>
        /// <param name="currentDataLocaleId"></param>
        /// <param name="systemDataLocaleId"></param>
        /// <returns></returns>
        public string GetAffectedInfo(Collection<long> entityIds, string attributeIdList, int containerId, int currentDataLocaleId, int systemDataLocaleId)
        {
            EntityOperationsBL entityBL = new EntityOperationsBL();
            return MakeMethodCall(() => entityBL.GetAffectedInfo(entityIds, attributeIdList, containerId, currentDataLocaleId, systemDataLocaleId));

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityIdList"></param>
        /// <param name="activityId"></param>
        /// <param name="workflowType"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public String GetWorkflowStatusXml(string entityIdList, int activityId, string workflowType, int userId)
        {
            EntityOperationsBL entityBL = new EntityOperationsBL();

            return MakeMethodCall(() => entityBL.GetWorkflowStatusXml(entityIdList, activityId, workflowType, userId));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityIdList"></param>
        /// <param name="categoryId"></param>
        /// <param name="containerId"></param>
        /// <returns></returns>
        public Collection<Entity> GetMDLsByIdList(string entityIdList, long categoryId, int containerId)
        {
            EntityOperationsBL entityBL = new EntityOperationsBL();
            return MakeMethodCall(() => entityBL.GetMDLsByIdList(entityIdList, categoryId, containerId));

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="localeId"></param>
        /// <param name="cnodeId"></param>
        /// <param name="catalogId"></param>
        /// <param name="attributeId"></param>
        /// <param name="toTotalImpacted"></param>
        /// <param name="totalAffected"></param>
        /// <param name="maxReturnCount"></param>
        /// <returns></returns>
        public Dictionary<String, Object> GetImpactedEntities(int localeId, long cnodeId, int catalogId, int attributeId, int toTotalImpacted, int totalAffected, int maxReturnCount)
        {
            int countImpacted = toTotalImpacted;
            int countAffected = totalAffected;
            int countMax = maxReturnCount;
            Dictionary<String, Object> getImactedEntities = new Dictionary<string, object>();

            EntityOperationsBL entityBL = new EntityOperationsBL();

            String strImactedEntities = MakeMethodCall(() => entityBL.GetImpactedEntities(localeId, cnodeId, catalogId, attributeId, ref countImpacted, ref countAffected, ref countMax));
            getImactedEntities.Add("getImpactedEntities", strImactedEntities);
            getImactedEntities.Add("countImpacted", countImpacted);
            getImactedEntities.Add("countAffected", countAffected);
            getImactedEntities.Add("countMax", countMax);

            return getImactedEntities;

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cnodeId"></param>
        /// <param name="cnodeParentId"></param>
        /// <param name="containerId"></param>
        /// <param name="bitUseDraftTax"></param>
        /// <param name="dataLocale"></param>
        /// <returns></returns>
        public string GetParent(long cnodeId, long cnodeParentId, int containerId, bool bitUseDraftTax, int dataLocale)
        {
            EntityOperationsBL entityBL = new EntityOperationsBL();
            return MakeMethodCall(() => entityBL.GetParent(cnodeId, cnodeParentId, containerId, bitUseDraftTax, dataLocale));

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="extSystemId"></param>
        /// <param name="inputXML"></param>
        /// <param name="coreAttrList"></param>
        /// <param name="techAttrList"></param>
        /// <param name="localeId"></param>
        /// <param name="programName"></param>
        /// <param name="includeInheritedValue"></param>
        /// <param name="computeInheritedValues"></param>
        /// <returns></returns>
        public string GetAttributeValuesForMDLs(int extSystemId, string inputXML, string coreAttrList, string techAttrList, int localeId, string programName, bool includeInheritedValue, bool computeInheritedValues)
        {
            EntityOperationsBL entityBL = new EntityOperationsBL();
            return MakeMethodCall(() => entityBL.GetAttributeValuesForMDLs(extSystemId, inputXML, coreAttrList, techAttrList, localeId, programName, includeInheritedValue, computeInheritedValues));

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cnodeId"></param>
        /// <param name="catalogIdList"></param>
        /// <param name="delimiter"></param>
        /// <param name="returnSelf"></param>
        /// <returns></returns>
        public Collection<Entity> GetMDL(long cnodeId, string catalogIdList, string delimiter, bool returnSelf)
        {
            EntityOperationsBL entityBL = new EntityOperationsBL();
            return MakeMethodCall(() => entityBL.GetMDL(cnodeId, catalogIdList, delimiter, returnSelf));

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cnodeId"></param>
        /// <param name="cnodeParentId"></param>
        /// <param name="containerId"></param>
        /// <param name="dataLocale"></param>
        /// <returns></returns>
        public List<Entity> GetParentOPBL(long cnodeId, long cnodeParentId, int containerId, int dataLocale)
        {
            EntityOperationsBL entityBL = new EntityOperationsBL();
            return MakeMethodCall(() => entityBL.GetParent(cnodeId, cnodeParentId, containerId, dataLocale));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cnodeId"></param>
        /// <param name="dataConfigXML"></param>
        /// <param name="catalogIdList"></param>
        /// <param name="delimiter"></param>
        /// <param name="returnSelf"></param>
        /// <returns></returns>
        public Collection<Entity> GetMDLBasedOnConfig(long cnodeId, string dataConfigXML, string catalogIdList, string delimiter, bool returnSelf)
        {
            EntityOperationsBL entityBL = new EntityOperationsBL();
            return MakeMethodCall(() => entityBL.GetMDLBasedOnConfig(cnodeId, dataConfigXML, catalogIdList, delimiter, returnSelf));

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cNodeId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public Collection<BusinessObjects.Workflow.WorkflowActivity> GetWorkflowTasks(long cNodeId, int userId)
        {
            return MakeMethodCall(() => EntityOperationsBL.GetWorkflowTasks(cNodeId, userId));

        }

        /// <summary>
        /// Reclassifies entities given in input xml
        /// </summary>
        /// <param name="dataXml"></param>
        /// <param name="userName"></param>
        /// <param name="isCategoryReclassify"></param>
        /// <returns>Boolean</returns>
        public Boolean Reclassify(String dataXml, String userName, Boolean isCategoryReclassify)
        {
            Boolean hasError = false;
            EntityOperationsBL entityOperationsBl = new EntityOperationsBL();
            DataTable dtClassificationResult = entityOperationsBl.Reclassify(dataXml, userName, isCategoryReclassify);
            if (dtClassificationResult != null && dtClassificationResult.Rows.Count > 0)
            {
                DataRow row = dtClassificationResult.Rows[0];
                if (row != null)
                    hasError = ValueTypeHelper.ConvertToBoolean(row["Error"].ToString());
            }
            return hasError;
        }

        #endregion

        #region JobManager

        #region JobBL
        /// <summary>
        /// 
        /// </summary>
        /// <param name="jobId"></param>
        /// <param name="jobAction"></param>
        /// <param name="jobType"></param>
        /// <param name="application"></param>
        /// <returns></returns>
        public bool UpdateUserAction(int jobId, JobAction jobAction, string jobType, MDMCenterApplication application)
        {
            return MakeMethodCall(() => new JobBL.LegacyJobMethods().UpdateUserAction(jobId, jobAction, jobType, application));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parentJobId"></param>
        /// <param name="application"></param>
        /// <returns>String </returns>
        public string GetChildJobsInXml(int parentJobId, MDMCenterApplication application)
        {
            return MakeMethodCall(() => new JobBL().GetChildJobsInXml(parentJobId, application));
        }
        #endregion

        #region JobScheduleBL
        /// <summary>
        /// 
        /// </summary>
        /// <param name="scheduleId"></param>
        /// <param name="jobType"></param>
        /// <param name="application"></param>
        /// <returns></returns>
        public Tuple<Collection<Job>, Collection<ExportProfile>> GetSchedule(int scheduleId, string jobType, MDMCenterApplication application)
        {
            return MakeMethodCall(() => new JobScheduleBL().GetSchedule(scheduleId, jobType, application));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="scheduleName"></param>
        /// <param name="scheduleLongName"></param>
        /// <param name="content"></param>
        /// <param name="isEnable"></param>
        /// <param name="profiles"></param>
        /// <param name="programName"></param>
        /// <param name="jobType"></param>
        /// <param name="application"></param>
        /// <returns></returns>
        public bool AddSchedule(string scheduleName, string scheduleLongName, string content, bool isEnable, string profiles, string programName, string jobType, MDMCenterApplication application)
        {
            MakeMethodCall(() => new JobScheduleBL().AddSchedule(scheduleName, scheduleLongName, content, isEnable, profiles, programName, jobType, application));
            return true;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="scheduleId"></param>
        /// <param name="scheduleName"></param>
        /// <param name="scheduleLongName"></param>
        /// <param name="content"></param>
        /// <param name="isEnable"></param>
        /// <param name="profiles"></param>
        /// <param name="programName"></param>
        /// <param name="jobType"></param>
        /// <param name="application"></param>
        /// <returns></returns>
        public bool UpdateSchedule(int scheduleId, string scheduleName, string scheduleLongName, string content, bool isEnable, string profiles, string programName, string jobType, MDMCenterApplication application)
        {
            MakeMethodCall(() => new JobScheduleBL().UpdateSchedule(scheduleId, scheduleName, scheduleLongName, content, isEnable, profiles, programName, jobType, application));
            return true;
        }
        #endregion
        #endregion JobManager

        #region Lookup
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="getFromSysObj"></param>
        /// <returns></returns>
        public DataTable GetTableStructure(string tableName, bool getFromSysObj)
        {
            return MakeMethodCall(() => SP.Lookup.GetTableStructure(tableName, getFromSysObj));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="TableType"></param>
        /// <returns></returns>
        public DataTable GetTableTypeTemplate(int TableType)
        {
            return MakeMethodCall(() => SP.Lookup.GetTableTypeTemplate(TableType));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public DataTable GetTableTypes()
        {
            return MakeMethodCall(() => SP.Lookup.GetTableTypes());
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="objectType"></param>
        /// <param name="searchString"></param>
        /// <param name="getFromSysObj"></param>
        /// <param name="getAttrCountColumn"></param>
        /// <returns></returns>
        public DataTable GetTableNames(int objectType, string searchString, bool getFromSysObj, bool getAttrCountColumn)
        {
            return MakeMethodCall(() => SP.Lookup.GetTableNames(objectType, searchString, getFromSysObj, getAttrCountColumn, false));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="refColumnName"></param>
        /// <param name="refMask"></param>
        /// <param name="displayColumns"></param>
        /// <param name="sortOrder"></param>
        /// <param name="searchColumns"></param>
        /// <returns></returns>
        public DataTable GetRefTableData(string tableName, string refColumnName, string refMask, string displayColumns, string sortOrder, string searchColumns)
        {
            return MakeMethodCall(() => SP.Lookup.GetRefTableData(tableName, refColumnName, refMask, displayColumns, sortOrder, searchColumns));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="jobServiceId"></param>
        /// <returns></returns>
        public DataTable GetNormalizationJobResults(int jobServiceId)
        {
            return MakeMethodCall(() => SP.Lookup.GetNormalizationJobResults(jobServiceId));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="applicationContextType"></param>
        /// <param name="seqDataTableforUI"></param>
        /// <returns></returns>
        public DataSet GetUserConfigContextData(int applicationContextType, String seqDataTableforUI)
        {
            SqlString seqDataForUI = seqDataTableforUI;
            DataSet ds = MakeMethodCall<DataSet>(() => SP.Catalog.GetUserConfigContextData(applicationContextType, ref seqDataForUI)); ;

            //Create a dummy table for the other one
            DataTable dtOtherTable = new DataTable();
            dtOtherTable.Columns.Add("SequenceName", typeof(String));
            DataRow dr = dtOtherTable.NewRow();
            dr["SequenceName"] = seqDataForUI;
            dtOtherTable.Rows.Add(dr);
            ds.Tables.Add(dtOtherTable);

            return ds;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string GetAllUOMsByUOMType()
        {
            return MakeMethodCall(() => SP.Catalog.GetAllUOMsByUOMType());
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="OrigLocale"></param>
        /// <param name="TransLocale"></param>
        /// <param name="OrigText"></param>
        /// <returns></returns>
        public DataTable GetTranslationMemory(String origLocale, String transLocale, String origText)
        {
            return MakeMethodCall(() => SP.Lookup.GetTranslationMemory(origLocale, transLocale, origText));
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="catalogId"></param>
        /// <param name="cnodeId"></param>
        /// <param name="viewPath"></param>
        /// <returns></returns>
        public string GetCNode(int catalogId, long cnodeId, string viewPath)
        {
            return MakeMethodCall(() => SP.Catalog.GetCNode(catalogId, cnodeId, viewPath));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="catalogId"></param>
        /// <param name="nodeTypeFrom"></param>
        /// <param name="relationshipType"></param>
        /// <returns></returns>
        public DataTable GetRelationshipCardinality(int catalogId, int nodeTypeFrom, int relationshipType)
        {
            return MakeMethodCall(() => SP.Catalog.GetRelationshipCardinality(catalogId, nodeTypeFrom, relationshipType));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="txtXML"></param>
        /// <param name="userName"></param>
        /// <param name="programName"></param>
        /// <param name="returnValue"></param>
        /// <returns></returns>
        public Int32 ProcessRelationshipCardinality(string txtXML, string userName, string programName, int returnValue)
        {
            SqlInt32 sqlReturnValue = returnValue;
            MakeMethodCall(() => SP.Catalog.ProcessRelationshipCardinality(txtXML, userName, programName, out sqlReturnValue));
            return sqlReturnValue.Value;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="txtXML"></param>
        /// <param name="userName"></param>
        /// <param name="programName"></param>
        /// <param name="returnValue"></param>
        /// <returns></returns>
        public Int32 ProcessRelationshipTypeEntityTypeCardinality(string txtXML, string userName, string programName, int returnValue)
        {
            SqlInt32 sqlReturnValue = returnValue;
            MakeMethodCall(() => SP.RelationshipTypeEntityTypeCardinality.ProcessRelationshipTypeEntityTypeCardinality(txtXML, userName, programName, out sqlReturnValue));
            return sqlReturnValue.Value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nodeTypeFrom"></param>
        /// <param name="relationshipType"></param>
        /// <returns></returns>
        public DataTable GetRelationshipTypeEntityTypeCardinality(int nodeTypeFrom, int relationshipType)
        {
            return MakeMethodCall(() => SP.RelationshipTypeEntityTypeCardinality.GetRelationshipTypeEntityTypeCardinality(nodeTypeFrom, relationshipType));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="origLocale"></param>
        /// <param name="transLocale"></param>
        /// <param name="OrigText"></param>
        /// <param name="transText"></param>
        /// <param name="moduser"></param>
        /// <param name="returnValue"></param>
        /// <returns></returns>
        public bool ProcessTranslationMemory(String origLocale, String transLocale, String OrigText, String transText, String moduser, Int32 returnValue)
        {
            SqlInt32 sqlReturnValue = returnValue;
            MakeMethodCall(() => SP.Lookup.ProcessTranslationMemory(origLocale, transLocale, OrigText, transText, moduser, out sqlReturnValue));
            return true;
        }


        #endregion

        #region Container RelationshipType EntityType Mapping

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <param name="cnodeGroupIds"></param>
        /// <param name="catalogId"></param>
        /// <returns></returns>
        public Collection<ContainerRelationshipTypeEntityTypeMapping> GetContainerRelationshipTypeEntityTypeMappingsByCnodes(String user, String cnodeGroupIds, Int32 catalogId)
        {
            return MakeMethodCall(() => new ContainerRelationshipTypeEntityTypeMappingBL().GetMappingsByCnodes(user, cnodeGroupIds, catalogId));
        }

        #endregion  Container RelationshipType EntityType Mapping

        #region Dynamic Table Schema

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbTable"></param>
        /// <param name="dynamicTableType"></param>
        /// <param name="callerContext"></param>
        /// <returns></returns>
        public OperationResult ProcessDynamicTableSchema(DBTable dbTable, DynamicTableType dynamicTableType, CallerContext callerContext)
        {
            return MakeMethodCall(() => new DynamicTableSchemaBL().Process(dbTable, dynamicTableType, callerContext));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbTables"></param>
        /// <param name="dynamicTableType"></param>
        /// <param name="callerContext"></param>
        /// <returns></returns>
        public OperationResult ProcessDynamicTableSchemas(DBTableCollection dbTables, DynamicTableType dynamicTableType, CallerContext callerContext)
        {
            return MakeMethodCall(() => new DynamicTableSchemaBL().Process(dbTables, dynamicTableType, callerContext));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="dynamicTableType"></param>
        /// <param name="callerContext"></param>
        /// <returns></returns>
        public OperationResult GetDynamicTableSchema(Int32 id, DynamicTableType dynamicTableType, CallerContext callerContext)
        {
            return MakeMethodCall(() => new DynamicTableSchemaBL().Get(id, dynamicTableType, callerContext));
        }

        #endregion

        #region Old Catalog = Entity

        /// <summary>
        /// 
        /// </summary>
        /// <param name="localeID"></param>
        /// <param name="customerID"></param>
        /// <param name="CNodeID"></param>
        /// <param name="CNodeParentID"></param>
        /// <param name="attrGroupID"></param>
        /// <param name="catalogID"></param>
        /// <param name="orgID"></param>
        /// <param name="user"></param>
        /// <param name="backupLocaleID"></param>
        /// <param name="viewPath"></param>
        /// <param name="useDraftTaxonomy"></param>
        /// <param name="encodeCollections"></param>
        /// <param name="attrIDList"></param>
        /// <returns></returns>
        public String GetCoreAttrXml(Int32 localeID, Int32 customerID, Int64 CNodeID, Int64 CNodeParentID, Int32 attrGroupID, Int32 catalogID, Int32 orgID, String user,
                    Int32 backupLocaleID, String viewPath, Boolean useDraftTaxonomy, Boolean encodeCollections, String attrIDList)
        {
            return MakeMethodCall(() => new EntityOperationsBL().GetCoreAttrXml(localeID, customerID, CNodeID, CNodeParentID, attrGroupID, catalogID,
                orgID, user, backupLocaleID, viewPath, useDraftTaxonomy, encodeCollections, attrIDList));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="CNodeID"></param>
        /// <param name="CNodeParentID"></param>
        /// <param name="catalogID"></param>
        /// <param name="attrGroupID"></param>
        /// <param name="localeID"></param>
        /// <param name="customerID"></param>
        /// <param name="user"></param>
        /// <param name="backupLocaleID"></param>
        /// <param name="viewPath"></param>
        /// <param name="useDraftTaxonomy"></param>
        /// <param name="encodeCollections"></param>
        /// <param name="attrIDList"></param>
        /// <returns></returns>
        public String GetTechAttrXml(Int32 CNodeID, Int32 CNodeParentID, Int32 catalogID, Int32 attrGroupID, Int32 localeID, Int32 customerID,
            String user, Int32 backupLocaleID, String viewPath, Boolean useDraftTaxonomy, Boolean encodeCollections, String attrIDList)
        {
            return MakeMethodCall(() => new EntityOperationsBL().GetTechAttrXml(CNodeID, CNodeParentID, catalogID, attrGroupID, localeID, customerID,
             user, backupLocaleID, viewPath, useDraftTaxonomy, encodeCollections, attrIDList));
        }

        #endregion

        #endregion Public Methods

        #region Private Methods

        private String GetEntityXml(Entity entity)
        {
            String entityXml = String.Empty;
            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            WriteEntityAsXml(entity, xmlWriter);

            xmlWriter.Flush();

            //get the actual XML
            entityXml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return entityXml;
        }

        private void WriteEntityAsXml(Entity entity, XmlWriter xmlWriter)
        {
            xmlWriter.WriteStartElement("Entity");

            xmlWriter.WriteAttributeString("Id", entity.Id.ToString());
            xmlWriter.WriteAttributeString("N", entity.Name);
            xmlWriter.WriteAttributeString("LN", entity.LongName);
            xmlWriter.WriteAttributeString("CatLNP", entity.CategoryLongNamePath);
            xmlWriter.WriteAttributeString("PEId", entity.ParentEntityId.ToString());
            xmlWriter.WriteAttributeString("PETId", entity.ParentEntityTypeId.ToString());
            xmlWriter.WriteAttributeString("PELN", entity.ParentEntityLongName);
            xmlWriter.WriteAttributeString("PExEId", entity.ParentExtensionEntityId.ToString());
            xmlWriter.WriteAttributeString("PExELN", entity.ParentExtensionEntityLongName);
            xmlWriter.WriteAttributeString("CatId", entity.CategoryId.ToString());
            xmlWriter.WriteAttributeString("CatLN", entity.CategoryLongName);
            xmlWriter.WriteAttributeString("ContId", entity.ContainerId.ToString());
            xmlWriter.WriteAttributeString("ContLN", entity.ContainerLongName);
            xmlWriter.WriteAttributeString("OId", entity.OrganizationId.ToString());
            xmlWriter.WriteAttributeString("OLN", entity.OrganizationLongName);
            xmlWriter.WriteAttributeString("ETId", entity.EntityTypeId.ToString());
            xmlWriter.WriteAttributeString("ETLN", entity.EntityTypeLongName);
            xmlWriter.WriteAttributeString("L", entity.Locale.ToString());
            xmlWriter.WriteAttributeString("A", entity.Action.ToString());
            xmlWriter.WriteAttributeString("PS", Utility.GetPermissionsAsString(entity.PermissionSet));

            if (entity.Attributes != null)
            {
                xmlWriter.WriteStartElement("Attributes");

                foreach (Attribute attribute in entity.Attributes)
                {
                    WriteAttributeAsXml(attribute, xmlWriter);
                }

                //Attributes node end
                xmlWriter.WriteEndElement();
            }

            if (entity.Relationships != null)
            {
                xmlWriter.WriteStartElement("Relationships");
                xmlWriter.WriteAttributeString("PS", Utility.GetPermissionsAsString(entity.Relationships.PermissionSet));
                xmlWriter.WriteAttributeString("IsInhRelsDirty", entity.Relationships.IsDenormalizedRelationshipsDirty.ToString().ToLowerInvariant());

                foreach (Relationship relationship in entity.Relationships)
                {
                    WriteRelationshipAsXml(relationship, xmlWriter);
                }

                //Relationships node end
                xmlWriter.WriteEndElement();
            }

            //Entity node end
            xmlWriter.WriteEndElement();
        }

        private void WriteAttributeAsXml(Attribute attribute, XmlWriter xmlWriter)
        {
            xmlWriter.WriteStartElement("Attribute");

            xmlWriter.WriteAttributeString("Id", attribute.Id.ToString());
            xmlWriter.WriteAttributeString("N", attribute.Name);
            xmlWriter.WriteAttributeString("LN", System.Net.WebUtility.HtmlEncode(attribute.LongName));
            xmlWriter.WriteAttributeString("DT", attribute.AttributeDataType.ToString());
            xmlWriter.WriteAttributeString("IRId", attribute.InstanceRefId.ToString());
            xmlWriter.WriteAttributeString("Seq", attribute.Sequence.ToString());
            xmlWriter.WriteAttributeString("PId", attribute.AttributeParentId.ToString());
            xmlWriter.WriteAttributeString("PN", attribute.AttributeParentName);
            xmlWriter.WriteAttributeString("PLN", attribute.AttributeParentLongName);
            xmlWriter.WriteAttributeString("Col", attribute.IsCollection.ToString());
            xmlWriter.WriteAttributeString("Com", attribute.IsComplex.ToString());
            xmlWriter.WriteAttributeString("LP", attribute.IsLookup.ToString());
            xmlWriter.WriteAttributeString("Loc", attribute.IsLocalizable.ToString());
            xmlWriter.WriteAttributeString("RO", attribute.ReadOnly.ToString());
            xmlWriter.WriteAttributeString("R", attribute.Required.ToString());
            xmlWriter.WriteAttributeString("ALF", attribute.ApplyLocaleFormat.ToString());
            xmlWriter.WriteAttributeString("ATZC", attribute.ApplyTimeZoneConversion.ToString());
            xmlWriter.WriteAttributeString("P", attribute.Precision.ToString());
            xmlWriter.WriteAttributeString("SF", Utility.GetSourceFlagString(attribute.SourceFlag));
            xmlWriter.WriteAttributeString("L", attribute.Locale.ToString());
            xmlWriter.WriteAttributeString("A", attribute.Action.ToString());
            xmlWriter.WriteAttributeString("HIOV", attribute.HasInvalidOverriddenValues.ToString().ToLowerInvariant());
            xmlWriter.WriteAttributeString("HIIV", attribute.HasInvalidInheritedValues.ToString().ToLowerInvariant());
            xmlWriter.WriteAttributeString("HIV", attribute.HasInvalidValues.ToString().ToLowerInvariant());

            if (!(attribute.AttributeType == AttributeTypeEnum.Complex || attribute.AttributeType == AttributeTypeEnum.ComplexCollection))
            {
                xmlWriter.WriteStartElement("Values");

                //For 'external' serialization, Values are always overridden.. So not writing SourceFlag
                String sourceFlag = Utility.GetSourceFlagString(Core.AttributeValueSource.Overridden);
                xmlWriter.WriteAttributeString("SF", sourceFlag);

                ValueCollection overriddenValues = (ValueCollection)attribute.GetOverriddenValues(attribute.Locale);

                if (overriddenValues != null)
                {
                    foreach (Value value in overriddenValues)
                    {
                        WriteValueAsXml(value, xmlWriter, attribute.AttributeDataType);
                    }
                }

                //Overridden Values node end
                xmlWriter.WriteEndElement();
            }

            xmlWriter.WriteStartElement("Values");

            String sFlag = Utility.GetSourceFlagString(Core.AttributeValueSource.Inherited);
            xmlWriter.WriteAttributeString("SF", sFlag);

            ValueCollection inheritedValues = (ValueCollection)attribute.GetInheritedValues(attribute.Locale);

            if (inheritedValues != null)
            {
                foreach (Value value in inheritedValues)
                {
                    WriteValueAsXml(value, xmlWriter, attribute.AttributeDataType);
                }
            }

            //Inherited Values node end
            xmlWriter.WriteEndElement();

            #region Write child attributes xml

            if (attribute.IsComplex && attribute.Attributes != null)
            {
                xmlWriter.WriteStartElement("Attributes");

                foreach (Attribute childAttribute in attribute.Attributes)
                {
                    WriteAttributeAsXml(childAttribute, xmlWriter);
                }

                //Attributes node end
                xmlWriter.WriteEndElement();
            }

            #endregion Write child attributes xml

            //Attributes node end
            xmlWriter.WriteEndElement();
        }

        private void WriteValueAsXml(Value value, XmlWriter xmlWriter, AttributeDataType attributeDataType)
        {
            String attrVal = (value.AttrVal == null) ? String.Empty : value.AttrVal.ToString();

            xmlWriter.WriteStartElement("Value");

            xmlWriter.WriteAttributeString("Id", value.Id.ToString());
            xmlWriter.WriteAttributeString("UId", value.UomId.ToString());
            xmlWriter.WriteAttributeString("U", value.Uom);
            xmlWriter.WriteAttributeString("VRId", value.ValueRefId.ToString());
            xmlWriter.WriteAttributeString("Seq", value.Sequence.ToString());
            xmlWriter.WriteAttributeString("DispV", value.GetDisplayValue());
            xmlWriter.WriteAttributeString("L", value.Locale.ToString());
            xmlWriter.WriteAttributeString("A", value.Action.ToString());

            if (value.InvariantVal != null)
            {
                xmlWriter.WriteAttributeString("IV", value.InvariantVal.ToString());
            }

            xmlWriter.WriteAttributeString("HIVV", value.HasInvalidValue.ToString().ToLowerInvariant());

            if (!String.IsNullOrWhiteSpace(attrVal))
            {
                if (attributeDataType == AttributeDataType.DateTime || attributeDataType == AttributeDataType.Date)
                {
                    String dateAttrVal = String.Empty;
                    Nullable<DateTime> nullableDateTime = value.GetDateTimeValue();

                    //value data type check is already done by validation, so it will always be correct
                    //the only case is keyword, for example DELETE, then value doesn't match data type
                    //so if we cant convert the value to DateTime, put blank
                    if (nullableDateTime != null)
                    {
                        dateAttrVal = nullableDateTime.Value.ToString("yyyy-MM-ddTHH:mm:ss.fff", CultureInfo.InvariantCulture);
                    }

                    xmlWriter.WriteAttributeString("DV", dateAttrVal);
                }
                else if ((attributeDataType == AttributeDataType.Integer || attributeDataType == AttributeDataType.Decimal) && value.NumericVal != null)
                {
                    //in case of keyword coming as value, this may be a non-numeric values here, don't get scared!
                    xmlWriter.WriteAttributeString("NV", value.NumericVal.ToString());
                }
                else
                {
                    xmlWriter.WriteAttributeString("SV", attrVal);
                }
            }

            //Write attribute value as value of node. Not as attribute.
            xmlWriter.WriteCData(XmlSerializationHelper.CleanInvalidXmlChars(attrVal));

            //Value node end
            xmlWriter.WriteEndElement();
        }

        private void WriteRelationshipAsXml(Relationship relationship, XmlWriter xmlWriter)
        {
            xmlWriter.WriteStartElement("Relationship");

            xmlWriter.WriteAttributeString("Id", relationship.Id.ToString());
            xmlWriter.WriteAttributeString("RTId", relationship.RelationshipTypeId.ToString());
            xmlWriter.WriteAttributeString("RTN", relationship.RelationshipTypeName);
            xmlWriter.WriteAttributeString("FEId", relationship.FromEntityId.ToString());
            xmlWriter.WriteAttributeString("REId", relationship.RelatedEntityId.ToString());
            xmlWriter.WriteAttributeString("RPId", relationship.RelationshipParentId.ToString());
            xmlWriter.WriteAttributeString("L", relationship.Locale.ToString());
            xmlWriter.WriteAttributeString("SF", Utility.GetSourceFlagString(relationship.SourceFlag));
            xmlWriter.WriteAttributeString("A", relationship.Action.ToString());
            xmlWriter.WriteAttributeString("ContId", relationship.ContainerId.ToString());
            xmlWriter.WriteAttributeString("ToCatLNP", relationship.ToCategoryLongNamePath);
            xmlWriter.WriteAttributeString("ToContId", relationship.ToContainerId.ToString());
            xmlWriter.WriteAttributeString("ToContN", relationship.ToContainerName);
            xmlWriter.WriteAttributeString("ToETId", relationship.ToEntityTypeId.ToString());
            xmlWriter.WriteAttributeString("ToETN", relationship.ToEntityTypeName);
            xmlWriter.WriteAttributeString("ToExId", relationship.ToExternalId);
            xmlWriter.WriteAttributeString("ToLN", relationship.ToLongName);
            xmlWriter.WriteAttributeString("Lvl", relationship.Level.ToString());
            xmlWriter.WriteAttributeString("IM", relationship.InheritanceMode.ToString());
            xmlWriter.WriteAttributeString("S", relationship.Status.ToString());
            xmlWriter.WriteAttributeString("PS", Utility.GetPermissionsAsString(relationship.PermissionSet));

            if (relationship.RelatedEntity != null)
            {
                WriteEntityAsXml(relationship.RelatedEntity, xmlWriter);
            }

            if (relationship.RelationshipAttributes != null)
            {
                xmlWriter.WriteStartElement("Attributes");

                foreach (Attribute attribute in relationship.RelationshipAttributes)
                {
                    WriteAttributeAsXml(attribute, xmlWriter);
                }

                //Attributes node end
                xmlWriter.WriteEndElement();
            }

            #region Write Relationships

            if (relationship.RelationshipCollection != null)
            {
                xmlWriter.WriteStartElement("Relationships");

                foreach (Relationship childRelationship in relationship.RelationshipCollection)
                {
                    WriteRelationshipAsXml(childRelationship, xmlWriter);
                }

                xmlWriter.WriteEndElement();
            }

            #endregion

            //Relationship node end
            xmlWriter.WriteEndElement();
        }

        #endregion

        #endregion Methods
    }
}