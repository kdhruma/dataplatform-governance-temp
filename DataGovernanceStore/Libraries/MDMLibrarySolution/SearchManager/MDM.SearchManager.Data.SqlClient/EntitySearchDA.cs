using System;
using System.Data;
using System.Data.SqlClient;
using System.Xml;
using System.Collections.ObjectModel;
using Microsoft.SqlServer.Server;
using System.Collections.Generic;

namespace MDM.SearchManager.Data
{
    using MDM.Core;
    using MDM.Utility;
    using MDM.BusinessObjects;
    using MDM.BusinessObjects.Diagnostics;

    /// <summary>
    /// Specifies the data access operations for search
    /// </summary>
    public class EntitySearchDA : SqlClientDataAccessBase
    {
        #region Fields

        /// <summary>
        /// This field denotes whether the searchCriteria contains workflow attributes
        /// </summary>
        private Boolean _containsWorkFlowAttributes = false;

        /// <summary>
        /// 
        /// </summary>
        private const Char WorkflowIdListSeparator = ',';

        #endregion

        #region Constructors
        #endregion

        #region Properties
        #endregion

        #region Methods

        #region Public Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchCriteria"></param>
        /// <param name="searchContext"></param>
        /// <param name="searchConfigurationXml"></param>
        /// <param name="userLogin"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        public T SearchEntities<T>(SearchCriteria searchCriteria, SearchContext searchContext, String searchConfigurationXml, String userLogin, DBCommandProperties command)
        {
            DiagnosticActivity diagnosticActivity = new DiagnosticActivity();
            var currentTraceSettings = MDMOperationContextHelper.GetCurrentTraceSettings();
            Boolean isTracingEnabled = currentTraceSettings.IsBasicTracingEnabled;

            SqlDataReader reader = null;
            LocaleEnum systemDataLocale = GlobalizationHelper.GetSystemDataLocale();
            T returnVal = default(T);

            try
            {
                #region Diagnostics & Tracing

                if (isTracingEnabled)
                {
                    diagnosticActivity.Start();
                }

                #endregion Diagnostics & Tracing

                SqlParameter[] parameters = GenerateInputParameters(searchContext, searchCriteria, searchConfigurationXml, systemDataLocale, userLogin);

                String storedProcedureName = "usp_SearchManager_Entity_Search";

                reader = ExecuteProcedureReader(command.ConnectionString, parameters, storedProcedureName);

                if (isTracingEnabled)
                {
                    diagnosticActivity.LogDurationInfo(String.Format("Execution for procedure : {0} is completed", storedProcedureName));
                }

                if (searchContext.IsRetrunAttributeListConfigured)
                {
                    returnVal = (T)Convert.ChangeType(GetEntityIdList(reader), typeof(T));
                }
                else
                {
                    returnVal = (T)Convert.ChangeType(EntityDataReaderUtility.ReadEntities(reader, MDMCenterModules.Search, false, false, false, false, null, false), typeof(T));
                }
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }

                if (isTracingEnabled)
                {
                    diagnosticActivity.Stop();
                }
            }

            return returnVal;
        }

        /// <summary>
        /// Search Entities for given extended search criteria and return list of entities with specified context.
        /// </summary>
        /// <param name="searchCriteria">Extended Search Criteria</param>
        /// <param name="searchContext">Search Context</param>
        /// <param name="systemDataLocale">Indicates system data locale</param>
        /// <param name="userLogin">Indicates logged in user id</param>
        /// <param name="command">Indicates Sql Command</param>
        /// <returns>Collection of Entity maps</returns>
        public EntityMapCollection SearchEntitiesByExtendedSearchCriteria(ExtendedSearchCriteria searchCriteria, SearchContext searchContext, LocaleEnum systemDataLocale, String userLogin, DBCommandProperties command)
        {
            DiagnosticActivity diagnosticActivity = new DiagnosticActivity();
            var currentTraceSettings = MDMOperationContextHelper.GetCurrentTraceSettings();
            Boolean isTracingEnabled = currentTraceSettings.IsBasicTracingEnabled;

            SqlDataReader reader = null;
            EntityMapCollection entityMaps = new EntityMapCollection();

            try
            {
                #region Diagnostics & Tracing

                if (isTracingEnabled)
                {
                    diagnosticActivity.Start();
                }

                #endregion Diagnostics & Tracing

                //Get parameters populated
                SqlParameter[] parameters = GenerateInputParameters(searchContext, searchCriteria, systemDataLocale, userLogin);
                String storedProcedureName = "usp_SearchManager_AuditReport_Get";
                reader = ExecuteProcedureReader(command.ConnectionString, parameters, storedProcedureName);

                if (reader != null)
                {
                    while (reader.Read())
                    {
                        Int64 cNodeId = 0;
                        Int32 catalogId = 0;
                        Int32 entityTypeId = -1;

                        if (reader["EntityId"] != null)
                        {
                            Int64.TryParse(reader["EntityId"].ToString(), out cNodeId);
                        }
                        if (reader["EntityTypeId"] != null)
                        {
                            Int32.TryParse(reader["EntityTypeId"].ToString(), out entityTypeId);
                        }

                        EntityMap entityMap = new EntityMap() { Id = cNodeId, EntityTypeId = entityTypeId, ContainerId = catalogId };
                        entityMaps.Add(entityMap);
                    }
                }
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }

                if (isTracingEnabled)
                {
                    diagnosticActivity.Stop();
                }
            }

            return entityMaps;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Generates list of  parameters
        /// </summary>
        /// <param name="searchContext">Indicates Search Context</param>
        /// <param name="searchCriteria">Indicates Search Criteria</param>
        /// <param name="searchConfigurationXml">Indicates searchConfigurationXml</param>
        /// <param name="systemDataLocale">Indicates systemDataLocale</param>
        /// <param name="currentUser">Indicates user login</param>
        /// <returns>Returns list pf parameters populates</returns>
        SqlParameter[] GenerateInputParameters(SearchContext searchContext, SearchCriteria searchCriteria, String searchConfigurationXml, LocaleEnum systemDataLocale, String currentUser)
        {
            SqlParametersGenerator generator = new SqlParametersGenerator("SearchManager_SqlParameters");
            String parameterConfigName = "SearchManager_Entity_Search_ParametersArray";

            SqlParameter[] parameters = generator.GetParameters(parameterConfigName);

            SqlMetaData[] containerMetadata = generator.GetTableValueMetadata(parameterConfigName, parameters[0].ParameterName);
            List<SqlDataRecord> containerIdList = EntityDataReaderUtility.CreateIntegerIdTable(searchCriteria.ContainerIds, containerMetadata);

            SqlMetaData[] categoryMetadata = generator.GetTableValueMetadata(parameterConfigName, parameters[1].ParameterName);
            List<SqlDataRecord> categoryIdList = EntityDataReaderUtility.CreateEntityIdTable(searchCriteria.CategoryIds, categoryMetadata);

            List<SqlDataRecord> attributeDetailsList = CreateSearchAttributeTable(searchCriteria.SearchAttributeRules, generator, parameters);

            List<SqlDataRecord> searchConfigurationList = CreateSearchConfigurationTable(searchConfigurationXml, searchContext, generator, parameters);

            SqlMetaData[] sqlLocalesMetadata = generator.GetTableValueMetadata(parameterConfigName, parameters[4].ParameterName);
            List<SqlDataRecord> localeDetailsList = EntityDataReaderUtility.CreateLocaleTable(searchCriteria.Locales, (Int32)systemDataLocale, sqlLocalesMetadata);

            SqlMetaData[] entityTypeMetadata = generator.GetTableValueMetadata(parameterConfigName, parameters[5].ParameterName);
            List<SqlDataRecord> entityTypeList = EntityDataReaderUtility.CreateIntegerIdTable(searchCriteria.EntityTypeIds, entityTypeMetadata);

            List<SqlDataRecord> displayAttributeIdList = CreateDisplayAttributeTable(searchContext.ReturnAttributeList, generator, parameters);

            List<SqlDataRecord> workflowConfigList = CreateWorkflowConfigurationTable(searchCriteria, generator, parameters);

            List<SqlDataRecord> searchWeightageAttributes = CreateSearchWeightageAttributesTable(searchCriteria, generator, parameters);

            SqlMetaData[] relationshipTypeMetadata = generator.GetTableValueMetadata(parameterConfigName, parameters[11].ParameterName);
            List<SqlDataRecord> relationshipTypeList = EntityDataReaderUtility.CreateIntegerIdTable(searchCriteria.RelationshipTypeIds, relationshipTypeMetadata);

            SqlMetaData[] businessConditionsSearchParamtableMetadata = generator.GetTableValueMetadata(parameterConfigName, parameters[12].ParameterName);
            List<SqlDataRecord> businessConditionList = CreateBusinessConditionsTable(searchCriteria, generator, businessConditionsSearchParamtableMetadata);

            parameters[0].Value = containerIdList;
            parameters[1].Value = categoryIdList;
            parameters[2].Value = attributeDetailsList;
            parameters[3].Value = searchConfigurationList;
            parameters[4].Value = localeDetailsList;
            parameters[5].Value = entityTypeList;
            parameters[6].Value = workflowConfigList;
            parameters[7].Value = displayAttributeIdList;
            parameters[8].Value = searchWeightageAttributes;
            parameters[9].Value = currentUser;
            parameters[10].Value = searchContext.IncludeCategoryPathInResult;
            parameters[11].Value = relationshipTypeList;
            parameters[12].Value = businessConditionList;
            parameters[13].Value = !searchContext.IsRetrunAttributeListConfigured;

            return parameters;
        }

        /// <summary>
        /// Generates list of  parameters
        /// </summary>
        /// <param name="searchContext">Indicates Search Context</param>
        /// <param name="searchCriteria">Indicates Search Criteria</param>
        /// <param name="systemDataLocale">Indicates systemDataLocale</param>
        /// <param name="userLogin">Indicates userLogin</param>
        /// <returns>Returns list pf parameters populates</returns>
        private SqlParameter[] GenerateInputParameters(SearchContext searchContext, ExtendedSearchCriteria searchCriteria, LocaleEnum systemDataLocale, String userLogin)
        {
            SqlParameter[] parameters;

            SqlParametersGenerator generator = new SqlParametersGenerator("SearchManager_SqlParameters");

            String paramKey = "SearchManager_AuditReport_Get_ParametersArray";

            parameters = generator.GetParameters(paramKey);

            SqlMetaData[] containerMetadata = generator.GetTableValueMetadata(paramKey, parameters[0].ParameterName);
            List<SqlDataRecord> containerIdList = EntityDataReaderUtility.CreateIntegerIdTable(searchCriteria.ContainerIds, containerMetadata);

            SqlMetaData[] entityTypeMetadata = generator.GetTableValueMetadata(paramKey, parameters[1].ParameterName);
            List<SqlDataRecord> entityTypeList = EntityDataReaderUtility.CreateIntegerIdTable(searchCriteria.EntityTypeIds, entityTypeMetadata);

            SqlMetaData[] categoryMetadata = generator.GetTableValueMetadata(paramKey, parameters[2].ParameterName);
            List<SqlDataRecord> categoryIdList = EntityDataReaderUtility.CreateEntityIdTable(searchCriteria.CategoryIds, categoryMetadata);

            //Create list of attribute details (attributes to be searched.
            List<SqlDataRecord> attributeDetailsList = null;
            List<SqlDataRecord> hasValueAttributeList = null;
            List<SqlDataRecord> entityShortNameList = null;
            Int32 returnType = 2; // 0 - Has No Value, 1 = Has Value, 2- All Values

            if (searchCriteria.SearchAttributeRules != null)
            {
                attributeDetailsList = new List<SqlDataRecord>();
                hasValueAttributeList = new List<SqlDataRecord>();
                entityShortNameList = new List<SqlDataRecord>();

                SqlMetaData[] sqlAttributeDetailsList = generator.GetTableValueMetadata(paramKey, parameters[3].ParameterName);
                SqlMetaData[] sqlHasValueAttributeList = generator.GetTableValueMetadata(paramKey, parameters[6].ParameterName);
                SqlMetaData[] sqlEntityShortNameList = generator.GetTableValueMetadata(paramKey, parameters[4].ParameterName);

                foreach (SearchAttributeRule searchAttributeRule in searchCriteria.SearchAttributeRules)
                {
                    //following Attribute Ids belong to workflow and needs to be passed to workflow search param table and if found notify telling workflow attributes found by setting field _containsWorkFlowAttributes true.
                    if (searchAttributeRule.Attribute.Id == 96 || searchAttributeRule.Attribute.Id == 97 || searchAttributeRule.Attribute.Id == 98 || searchAttributeRule.Attribute.Id == 99 || searchAttributeRule.Attribute.Id == 100)
                    {
                        _containsWorkFlowAttributes = true;
                        continue;
                    }

                    if (searchAttributeRule.Attribute.Id == 22)
                    {
                        String[] shortNames = ValueTypeHelper.SplitStringToStringArray(searchAttributeRule.Attribute.GetCurrentValue() as String, ',');

                        foreach (String shortName in shortNames)
                        {
                            SqlDataRecord entityShortNameDataRecord = new SqlDataRecord(sqlEntityShortNameList);
                            entityShortNameDataRecord.SetValue(0, shortName.Trim());
                            entityShortNameList.Add(entityShortNameDataRecord);
                        }
                        continue;
                    }
                    if (searchAttributeRule.Operator == SearchOperator.HasNoValue || searchAttributeRule.Operator == SearchOperator.HasValue || searchAttributeRule.Operator == SearchOperator.None)
                    {
                        SqlDataRecord hasValueAttributeDataRecord = new SqlDataRecord(sqlHasValueAttributeList);
                        hasValueAttributeDataRecord.SetValue(0, searchAttributeRule.Attribute.Id);
                        hasValueAttributeList.Add(hasValueAttributeDataRecord);
                        if (searchAttributeRule.Operator == SearchOperator.HasValue)
                        {
                            returnType = 1;
                        }
                        else if (searchAttributeRule.Operator == SearchOperator.HasNoValue && returnType == 2)
                        {
                            returnType = 0;
                        }
                    }
                    else
                    {
                        ValueCollection values = (ValueCollection)searchAttributeRule.Attribute.GetCurrentValues();
                        foreach (Value value in values)
                        {
                            SqlDataRecord attributeDetailsRecord = new SqlDataRecord(sqlAttributeDetailsList);
                            attributeDetailsRecord.SetValue(0, searchAttributeRule.Attribute.Id);
                            attributeDetailsRecord.SetValue(1, value.AttrVal);
                            attributeDetailsRecord.SetValue(2, null);
                            attributeDetailsRecord.SetValue(3, "=");
                            attributeDetailsList.Add(attributeDetailsRecord);
                        }
                    }
                }

                if (attributeDetailsList.Count == 0)
                {
                    attributeDetailsList = null;
                }
                if (hasValueAttributeList.Count == 0)
                {
                    hasValueAttributeList = null;
                }
                if (entityShortNameList.Count == 0)
                {
                    entityShortNameList = null;
                }
            }

            //Create included containers table
            List<SqlDataRecord> includedContainerIdList = null;
            if (searchCriteria.AdditionalCatalogIds != null)
            {
                SqlMetaData[] sqlContainerIdMetadata = generator.GetTableValueMetadata(paramKey, parameters[10].ParameterName);

                if (searchCriteria.AdditionalCatalogIds.Count > 0)
                {
                    includedContainerIdList = new List<SqlDataRecord>();
                    foreach (KeyValuePair<Int32, String> containerCategoryMap in searchCriteria.AdditionalCatalogIds)
                    {
                        String[] categories = ValueTypeHelper.SplitStringToStringArray(containerCategoryMap.Value, ",");

                        if (categories != null)
                        {
                            foreach (String category in categories)
                            {
                                SqlDataRecord containerIdRecord = new SqlDataRecord(sqlContainerIdMetadata);
                                containerIdRecord.SetValue(0, containerCategoryMap.Key);
                                containerIdRecord.SetValue(1, category);
                                includedContainerIdList.Add(containerIdRecord);
                            }
                        }
                        else
                        {
                            SqlDataRecord containerIdRecord = new SqlDataRecord(sqlContainerIdMetadata);
                            containerIdRecord.SetValue(0, containerCategoryMap.Key);
                            containerIdRecord.SetValue(1, containerCategoryMap.Value);
                            includedContainerIdList.Add(containerIdRecord);
                        }
                    }
                }
            }

            if (includedContainerIdList != null & includedContainerIdList.Count == 0)
            {
                includedContainerIdList = null;
            }

            //Create list of included entity type id
            List<SqlDataRecord> includedEntityTypeList = null;
            if (searchCriteria.AdditionalEntityTypeIds != null)
            {
                SqlMetaData[] sqlEntityTypeIdMetadata = generator.GetTableValueMetadata(paramKey, parameters[11].ParameterName);

                if (searchCriteria.AdditionalEntityTypeIds.Count > 0)
                {
                    includedEntityTypeList = new List<SqlDataRecord>();

                    foreach (Int32 entityTypeId in searchCriteria.AdditionalEntityTypeIds)
                    {
                        SqlDataRecord entityTypeIdRecord = new SqlDataRecord(sqlEntityTypeIdMetadata);
                        entityTypeIdRecord.SetValue(0, entityTypeId);
                        includedEntityTypeList.Add(entityTypeIdRecord);
                    }
                }
            }

            if (includedEntityTypeList != null & includedEntityTypeList.Count == 0)
            {
                includedEntityTypeList = null;
            }

            parameters[0].Value = containerIdList;
            parameters[1].Value = entityTypeList;
            parameters[2].Value = categoryIdList;
            parameters[3].Value = attributeDetailsList;
            parameters[4].Value = entityShortNameList;
            parameters[5].Value = returnType;
            parameters[6].Value = hasValueAttributeList;
            parameters[7].Value = searchCriteria.IsSearchInMasterContainer;
            parameters[8].Value = systemDataLocale;
            parameters[9].Value = ((includedContainerIdList != null ? includedContainerIdList.Count : 0) + (includedEntityTypeList != null ? includedEntityTypeList.Count : 0)) > 0;
            parameters[10].Value = includedContainerIdList;
            parameters[11].Value = includedEntityTypeList;
            parameters[12].Value = userLogin;

            return parameters;
        }

        /// <summary>
        /// Creates SearchAttributeTable
        /// </summary>
        /// <param name="searchAttributeRules"></param>
        /// <param name="generator">Indicates parameter generator</param>
        /// <param name="parameters">Indicates list of parameters</param>
        /// <returns>Returns list of search Attribute Values</returns>
        private List<SqlDataRecord> CreateSearchAttributeTable(Collection<SearchAttributeRule> searchAttributeRules, SqlParametersGenerator generator, SqlParameter[] parameters)
        {
            List<SqlDataRecord> attributeDetailsList = null;

            SqlMetaData[] sqlAttributeDetailsList = generator.GetTableValueMetadata("SearchManager_Entity_Search_ParametersArray", parameters[2].ParameterName);

            if (searchAttributeRules != null && searchAttributeRules.Count > 0)
            {
                attributeDetailsList = new List<SqlDataRecord>();

                foreach (SearchAttributeRule searchAttributeRule in searchAttributeRules)
                {
                    //following Attribute Ids belong to workflow and needs to be passed to workflow search param table and if found notify telling workflow attributes found by setting field _containsWorkFlowAttributes true.
                    if (searchAttributeRule.Attribute.Id == 96 || searchAttributeRule.Attribute.Id == 97 || searchAttributeRule.Attribute.Id == 98 || searchAttributeRule.Attribute.Id == 99 || searchAttributeRule.Attribute.Id == 100)
                    {
                        _containsWorkFlowAttributes = true;
                        continue;
                    }
                    ValueCollection values = (ValueCollection)searchAttributeRule.Attribute.GetCurrentValues();

                    foreach (Value value in values)
                    {
                        SqlDataRecord attributeDetailsRecord = new SqlDataRecord(sqlAttributeDetailsList);
                        attributeDetailsRecord.SetValue(0, searchAttributeRule.Attribute.Id);
                        attributeDetailsRecord.SetValue(1, value.AttrVal);
                        attributeDetailsRecord.SetValue(2, null);
                        String searchOperator = String.Empty;
                        if (searchAttributeRule.Operator == SearchOperator.GreaterThan)
                        {
                            searchOperator = ">";
                        }
                        else if (searchAttributeRule.Operator == SearchOperator.LessThan)
                        {
                            searchOperator = "<";
                        }
                        else if (searchAttributeRule.Operator == SearchOperator.GreaterThanOrEqualTo)
                        {
                            searchOperator = ">=";
                        }
                        else if (searchAttributeRule.Operator == SearchOperator.LessThanOrEqualTo)
                        {
                            searchOperator = "<=";
                        }
                        else
                        {
                            searchOperator = Utility.GetOperatorString(searchAttributeRule.Operator);
                        }
                        attributeDetailsRecord.SetValue(3, searchOperator);
                        attributeDetailsList.Add(attributeDetailsRecord);
                    }
                }

                if (attributeDetailsList.Count == 0)
                {
                    attributeDetailsList = null;
                }
            }

            return attributeDetailsList;
        }

        /// <summary>
        /// Creates Display Attribute Table
        /// </summary>
        /// <param name="displayAttributeList">indicates display attribute list</param>
        /// <param name="generator">Indicates parameter generator</param>
        /// <param name="parameters">Indicates list of parameters</param>
        /// <returns></returns>
        private List<SqlDataRecord> CreateDisplayAttributeTable(Collection<Attribute> displayAttributeList, SqlParametersGenerator generator, SqlParameter[] parameters)
        {
            List<SqlDataRecord> displayAttributeIdList = null;

            SqlMetaData[] sqlDisplayAttributeIdList = generator.GetTableValueMetadata("SearchManager_Entity_Search_ParametersArray", parameters[7].ParameterName);

            if (displayAttributeList.Count > 0)
            {
                displayAttributeIdList = new List<SqlDataRecord>();

                foreach (Attribute attribute in displayAttributeList)
                {
                    SqlDataRecord displayAttributeIdRecord = new SqlDataRecord(sqlDisplayAttributeIdList);
                    displayAttributeIdRecord.SetValue(0, attribute.Id);
                    displayAttributeIdRecord.SetValue(1, attribute.Name);
                    displayAttributeIdRecord.SetValue(2, (Int32)attribute.Locale);
                    displayAttributeIdList.Add(displayAttributeIdRecord);
                }
            }

            return displayAttributeIdList;
        }

        /// <summary>
        /// Creates a record for Search Configuration
        /// </summary>
        /// <param name="searchConfigurationXml">Indicates Search Configuration Xml of Search Configuration Application Configuration Object</param>
        /// <param name="searchContext"></param>
        /// <param name="generator">Indicates parameter generator</param>
        /// <param name="parameters">Indicates list of parameters</param>
        /// <returns>Returns Search Configuration details like search depth, max records to return, populate score etc.</returns>
        private List<SqlDataRecord> CreateSearchConfigurationTable(String searchConfigurationXml, SearchContext searchContext, SqlParametersGenerator generator, SqlParameter[] parameters)
        {
            List<SqlDataRecord> searchConfigurationList = new List<SqlDataRecord>();

            SqlMetaData[] sqlSearchConfigurationList = generator.GetTableValueMetadata("SearchManager_Entity_Search_ParametersArray", parameters[3].ParameterName);
            SqlDataRecord searchConfigurationRecord = new SqlDataRecord(sqlSearchConfigurationList);
            XmlTextReader xmlReader = null;

            try
            {
                xmlReader = new XmlTextReader(searchConfigurationXml, XmlNodeType.Element, null);

                while (!xmlReader.EOF)
                {
                    if (xmlReader.NodeType == XmlNodeType.Element && (xmlReader.Name == "Param"))
                    {
                        if (xmlReader.GetAttribute("Name") != null)
                        {
                            String parameterName = xmlReader.GetAttribute("Name").ToString();
                            if (parameterName.ToLower().Equals("searchtarget"))
                            {
                                searchConfigurationRecord.SetValue(0, xmlReader.ReadElementContentAsString());
                            }
                            else if (parameterName.ToLower().Equals("searchresultsource"))
                            {
                                searchConfigurationRecord.SetValue(1, xmlReader.ReadElementContentAsString());
                            }
                            else if (parameterName.ToLower().Equals("fulltextsearch"))
                            {
                                searchConfigurationRecord.SetValue(2, ValueTypeHelper.BooleanTryParse(xmlReader.ReadElementContentAsString(), false));
                            }
                            else if (parameterName.ToLower().Equals("populatescore"))
                            {
                                searchConfigurationRecord.SetValue(3, ValueTypeHelper.BooleanTryParse(xmlReader.ReadElementContentAsString(), false));
                            }
                            else if (parameterName.ToLower().Equals("searchinhierarchy"))
                            {
                                searchConfigurationRecord.SetValue(4, ValueTypeHelper.BooleanTryParse(xmlReader.ReadElementContentAsString(), false));
                            }
                            else if (parameterName.ToLower().Equals("searchdepth"))
                            {
                                if (searchContext.SearchDepth > 0)
                                {
                                    searchConfigurationRecord.SetValue(5, searchContext.SearchDepth);
                                    xmlReader.Read();
                                }
                                else
                                {
                                    searchConfigurationRecord.SetValue(5, ValueTypeHelper.Int32TryParse(xmlReader.ReadElementContentAsString(), 0));
                                }
                            }
                            else if (parameterName.ToLower().Equals("maxrecordstoreturn"))
                            {
                                if (searchContext.MaxRecordsToReturn > 0)
                                {
                                    searchConfigurationRecord.SetValue(6, searchContext.MaxRecordsToReturn);
                                    xmlReader.Read();
                                }
                                else
                                {
                                    searchConfigurationRecord.SetValue(6, ValueTypeHelper.Int32TryParse(xmlReader.ReadElementContentAsString(), 0));
                                }
                            }
                            else if (parameterName.ToLower().Equals("customprocname"))
                            {
                                searchConfigurationRecord.SetValue(7, xmlReader.ReadElementContentAsString());
                            }
                            else if (parameterName.ToLower().Equals("relationshipsearchsource"))
                            {
                                String relationshipSearchSource = xmlReader.ReadElementContentAsString();
                                relationshipSearchSource = (relationshipSearchSource != null) ? relationshipSearchSource.ToLowerInvariant() : relationshipSearchSource;
                                searchConfigurationRecord.SetValue(8, relationshipSearchSource);//TODO: Need to set this from UI.
                            }
                            else
                            {
                                xmlReader.Read();
                            }
                        }
                    }
                    else
                    {
                        //Keep on reading the xml until we reach expected node.
                        xmlReader.Read();
                    }
                }
            }
            finally
            {
                if (xmlReader != null)
                {
                    xmlReader.Close();
                }
            }

            searchConfigurationList.Add(searchConfigurationRecord);

            if (searchConfigurationList == null || searchConfigurationList.Count == 0)
            {
                searchConfigurationList = null;
            }

            return searchConfigurationList;
        }

        /// <summary>
        /// CreateAdvanced Workflow Param Table
        /// </summary>
        /// <param name="searchCriteria">Indicates Search Criteria</param>
        /// <param name="generator">Indicates parameter generator</param>
        /// <param name="parameters">Indicates list of parameters</param>
        /// <returns>Returns list workflow details in Key Value pair</returns>
        private List<SqlDataRecord> CreateWorkflowConfigurationTable(SearchCriteria searchCriteria, SqlParametersGenerator generator, SqlParameter[] parameters)
        {
            List<SqlDataRecord> advancedWorkflowSearchParamList = new List<SqlDataRecord>();

            SqlMetaData[] sqlAdvancedWorkflowSearchParamtableMetadata = generator.GetTableValueMetadata("SearchManager_Entity_Search_ParametersArray", parameters[6].ParameterName);

            if (searchCriteria.WorkflowAssignedUsers != null)
            {
                foreach (String id in searchCriteria.WorkflowAssignedUsers)
                {
                    SqlDataRecord workflowAssignedUsersRecord = new SqlDataRecord(sqlAdvancedWorkflowSearchParamtableMetadata);
                    workflowAssignedUsersRecord.SetValue(0, "WorkflowAssignment");
                    workflowAssignedUsersRecord.SetValue(1, id);
                    workflowAssignedUsersRecord.SetValue(2, "=");
                    advancedWorkflowSearchParamList.Add(workflowAssignedUsersRecord);
                }
            }

            if (!String.IsNullOrWhiteSpace(searchCriteria.WorkflowName))
            {
                SqlDataRecord workflowNameRecord = new SqlDataRecord(sqlAdvancedWorkflowSearchParamtableMetadata);
                workflowNameRecord.SetValue(0, "WorkflowName");
                workflowNameRecord.SetValue(1, searchCriteria.WorkflowName);
                workflowNameRecord.SetValue(2, "=");
                advancedWorkflowSearchParamList.Add(workflowNameRecord);
            }

            if (searchCriteria.WorkflowStages != null && searchCriteria.WorkflowStages.Length > 0)
            {
                foreach (String id in searchCriteria.WorkflowStages)
                {
                    SqlDataRecord workflowVersionRecord = new SqlDataRecord(sqlAdvancedWorkflowSearchParamtableMetadata);
                    workflowVersionRecord.SetValue(0, "WorkflowState");
                    workflowVersionRecord.SetValue(1, id);
                    workflowVersionRecord.SetValue(2, "=");
                    advancedWorkflowSearchParamList.Add(workflowVersionRecord);
                }
            }

            if (!String.IsNullOrWhiteSpace(searchCriteria.WorkflowType))
            {
                SqlDataRecord workflowTypeRecord = new SqlDataRecord(sqlAdvancedWorkflowSearchParamtableMetadata);
                workflowTypeRecord.SetValue(0, "WorkflowType");
                workflowTypeRecord.SetValue(1, searchCriteria.WorkflowType);
                workflowTypeRecord.SetValue(2, "=");
                advancedWorkflowSearchParamList.Add(workflowTypeRecord);
            }

            if (!String.IsNullOrWhiteSpace(searchCriteria.WorkflowVersion))
            {
                foreach (String id in searchCriteria.WorkflowVersion.Split(','))
                {
                    SqlDataRecord workflowVersionRecord = new SqlDataRecord(sqlAdvancedWorkflowSearchParamtableMetadata);
                    workflowVersionRecord.SetValue(0, "WorkflowVersion");
                    workflowVersionRecord.SetValue(1, id);
                    workflowVersionRecord.SetValue(2, "=");
                    advancedWorkflowSearchParamList.Add(workflowVersionRecord);
                }
            }

            if (!String.IsNullOrWhiteSpace(searchCriteria.ConfiguredWorkflowForSearch))
            {
                foreach (
                    String workflowIdToSearch in
                        ValueTypeHelper.SplitStringToStringCollection(searchCriteria.ConfiguredWorkflowForSearch,
                            WorkflowIdListSeparator))
                {
                    SqlDataRecord configuredWorkflowForSearchRecord =
                        new SqlDataRecord(sqlAdvancedWorkflowSearchParamtableMetadata);
                    configuredWorkflowForSearchRecord.SetValue(0, "ConfiguredWorkflowForSearch");
                    configuredWorkflowForSearchRecord.SetValue(1, workflowIdToSearch);
                    configuredWorkflowForSearchRecord.SetValue(2, "=");
                    advancedWorkflowSearchParamList.Add(configuredWorkflowForSearchRecord);
                }
            }

            if (searchCriteria.WorkflowAssignedUsers != null)
            {
                foreach (String item in searchCriteria.WorkflowAssignedUsers)
                {
                    String assignmentType = String.Empty;

                    switch (item)
                    {
                        case "useactivityassignmentsettings":
                            assignmentType = "useactivityassignmentsettings";
                            break;
                        case "assignedtocurrentuser":
                            assignmentType = "assignedtocurrentuser";
                            break;
                        case "unassigned":
                            assignmentType = "unassigned";
                            break;
                        case "assignedtootherusers":
                            assignmentType = "assignedtootherusers";
                            break;
                        default:
                            assignmentType = "assignedtocurrentuser";
                            break;
                    }

                    SqlDataRecord workflowAssignmentSearchType = new SqlDataRecord(sqlAdvancedWorkflowSearchParamtableMetadata);
                    workflowAssignmentSearchType.SetValue(0, "AssignmentType");
                    workflowAssignmentSearchType.SetValue(1, assignmentType);
                    workflowAssignmentSearchType.SetValue(2, "=");
                    advancedWorkflowSearchParamList.Add(workflowAssignmentSearchType);
                }
            }

            SqlDataRecord returnWorkflowResultRecord = new SqlDataRecord(sqlAdvancedWorkflowSearchParamtableMetadata);
            returnWorkflowResultRecord.SetValue(0, "ReturnWorkflowResult");
            returnWorkflowResultRecord.SetValue(1, searchCriteria.ReturnWorkflowResult.ToString());
            returnWorkflowResultRecord.SetValue(2, "=");
            advancedWorkflowSearchParamList.Add(returnWorkflowResultRecord);

            List<SqlDataRecord> workflowAttributes = AddWorkFlowAttributesToParamList(searchCriteria.SearchAttributeRules, sqlAdvancedWorkflowSearchParamtableMetadata);

            if (workflowAttributes.Count > 0)
            {
                advancedWorkflowSearchParamList.AddRange(workflowAttributes);
            }

            if (advancedWorkflowSearchParamList.Count == 0)
            {
                advancedWorkflowSearchParamList = null;
            }

            return advancedWorkflowSearchParamList;
        }

        /// <summary>
        /// Creates business conditions param table
        /// </summary>
        /// <param name="searchCriteria">Indicates Search Criteria</param>
        /// <param name="generator">Indicates parameter generator</param>
        /// <param name="businessConditionsSearchParamtableMetadata">Indicates list of parameters</param>
        /// <returns>Returns list of business conditions status in Key Value pair</returns>
        private List<SqlDataRecord> CreateBusinessConditionsTable(SearchCriteria searchCriteria, SqlParametersGenerator generator, SqlMetaData[] businessConditionsSearchParamtableMetadata)
        {
            List<SqlDataRecord> businessConditionsStatusDataRecordList = null;
            
            BusinessConditionStatusCollection businessConditionsStatus = searchCriteria.BusinessConditionsStatus;

            if (businessConditionsStatus != null && businessConditionsStatus.Count > 0)
            {
                Collection<Int32> businessConditionIdList = businessConditionsStatus.GetBusinessConditionIdList();

                if (businessConditionIdList != null && businessConditionIdList.Count > 0)
                {
                    businessConditionsStatusDataRecordList = new List<SqlDataRecord>();

                    foreach (Int32 businessConditionId in businessConditionIdList)
                    {
                        BusinessConditionStatusCollection filteredBusinessConditons = businessConditionsStatus.Get(businessConditionId);

                        if (filteredBusinessConditons != null && filteredBusinessConditons.Count > 0)
                        {
                            String valueToSearch = String.Empty;
                            SqlDataRecord businessConditionDataRecord = new SqlDataRecord(businessConditionsSearchParamtableMetadata);

                            businessConditionDataRecord.SetValue(0, businessConditionId.ToString());

                            foreach (BusinessConditionStatus filteredBusinessConditon in filteredBusinessConditons)
                            {
                                valueToSearch = String.Concat(valueToSearch, (Int32)filteredBusinessConditon.Status, ",");
                            }

                            businessConditionDataRecord.SetValue(1, valueToSearch.Substring(0, valueToSearch.Length - 1));
                            businessConditionDataRecord.SetValue(2, "in");

                            businessConditionsStatusDataRecordList.Add(businessConditionDataRecord);
                        }
                    }

                    if (businessConditionsStatusDataRecordList.Count == 0)
                    {
                        businessConditionsStatusDataRecordList = null;
                    }
                }
            }

            return businessConditionsStatusDataRecordList;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchCriteria"></param>
        /// <param name="generator"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        private List<SqlDataRecord> CreateSearchWeightageAttributesTable(SearchCriteria searchCriteria, SqlParametersGenerator generator, SqlParameter[] parameters)
        {
            SqlMetaData[] searchWeightageAttributes = generator.GetTableValueMetadata("SearchManager_Entity_Search_ParametersArray", parameters[8].ParameterName);
            List<SqlDataRecord> searchWeightageAttributeRecords = null;
            if (searchCriteria.SearchWeightageAttributes != null && searchCriteria.SearchWeightageAttributes.Count > 0)
            {
                searchWeightageAttributeRecords = new List<SqlDataRecord>();
                foreach (SearchWeightageAttribute searchWeightageAttribute in searchCriteria.SearchWeightageAttributes)
                {
                    SqlDataRecord searchWeightageAttributeRecord = new SqlDataRecord(searchWeightageAttributes);
                    searchWeightageAttributeRecord.SetValue(0, searchWeightageAttribute.AttributeId);
                    searchWeightageAttributeRecord.SetValue(1, searchWeightageAttribute.AttributeValue);
                    searchWeightageAttributeRecord.SetValue(2, searchWeightageAttribute.Weightage);
                    searchWeightageAttributeRecord.SetValue(3, searchWeightageAttribute.LocaleId);
                    searchWeightageAttributeRecords.Add(searchWeightageAttributeRecord);
                }
            }
            return searchWeightageAttributeRecords;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchAttributeRules"></param>
        /// <param name="sqlAdvancedWorkflowSearchParamtableMetadata"></param>
        /// <returns></returns>
        private List<SqlDataRecord> AddWorkFlowAttributesToParamList(Collection<SearchAttributeRule> searchAttributeRules, SqlMetaData[] sqlAdvancedWorkflowSearchParamtableMetadata)
        {
            List<SqlDataRecord> workflowAttributes = new List<SqlDataRecord>();
            //if _containsWorkFlowAttributes is set to false, then no need to loop.
            if (_containsWorkFlowAttributes && searchAttributeRules != null && searchAttributeRules.Count > 0)
            {
                foreach (SearchAttributeRule searchAttributeRule in searchAttributeRules)
                {
                    //following Attribute Ids belong to workflow and needs to be passed to workflow search param table
                    if (searchAttributeRule.Attribute.Id == 96 || searchAttributeRule.Attribute.Id == 97 || searchAttributeRule.Attribute.Id == 98 || searchAttributeRule.Attribute.Id == 99 || searchAttributeRule.Attribute.Id == 100)
                    {

                        ValueCollection values = (ValueCollection)searchAttributeRule.Attribute.GetCurrentValues();

                        foreach (Value value in values)
                        {
                            SqlDataRecord attributeDetailsRecord = new SqlDataRecord(sqlAdvancedWorkflowSearchParamtableMetadata);
                            attributeDetailsRecord.SetValue(0, searchAttributeRule.Attribute.Id.ToString());
                            attributeDetailsRecord.SetValue(1, value.AttrVal);
                            String searchOperator = String.Empty;
                            if (searchAttributeRule.Operator == SearchOperator.GreaterThan)
                            {
                                searchOperator = ">";
                            }
                            else if (searchAttributeRule.Operator == SearchOperator.LessThan)
                            {
                                searchOperator = "<";
                            }
                            else if (searchAttributeRule.Operator == SearchOperator.GreaterThanOrEqualTo)
                            {
                                searchOperator = ">=";
                            }
                            else if (searchAttributeRule.Operator == SearchOperator.LessThanOrEqualTo)
                            {
                                searchOperator = "<=";
                            }
                            else
                            {
                                searchOperator = Utility.GetOperatorString(searchAttributeRule.Operator);
                            }
                            attributeDetailsRecord.SetValue(2, searchOperator);
                            workflowAttributes.Add(attributeDetailsRecord);
                        }
                    }
                }
            }
            return workflowAttributes;
        }

        /// <summary>
        /// Gets collection of entity id list
        /// </summary>
        /// <param name="reader">Indicates SQL data reader</param>
        /// <returns>Returns collection of entity id</returns>
        private Collection<Int64> GetEntityIdList(SqlDataReader reader)
        {
            Collection<Int64> entityIdList = new Collection<Int64>();

            while (reader.Read())
            {
                if (reader["Id"] != null)
                {
                    Int64 entityId = ValueTypeHelper.Int64TryParse(reader["Id"].ToString(), -1);

                    if (entityId > 0 && !entityIdList.Contains(entityId))
                    {
                        entityIdList.Add(entityId);
                    }
                }
            }

            return entityIdList;
        }

        #endregion

        #endregion
    }
}