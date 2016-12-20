using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Xml;

namespace MDM.SearchManager.Data
{
    using BusinessObjects;
    using Core;
    using Utility;

    /// <summary>
    /// Specifies the data access operations for search category
    /// </summary>
    public class CategorySearchDA : SqlClientDataAccessBase
    {
        #region Constants

        private const Char WorkflowIdListSeparator = ',';

        #endregion 

        #region Fields

        #endregion

        #region Constructors

        #endregion

        #region Properties

        #endregion

        #region Methods

        #region Public Methods

        /// <summary>
        /// Search Categories for given search criteria and return list of category ids with specified context.
        /// </summary>
        /// <param name="searchCriteria">Search Criteria</param>
        /// <param name="searchContext">Search Context</param>
        /// <param name="searchConfigurationXml">Search ConfigurationXml Xml</param>
        /// <param name="systemDataLocale">Indicates system data locale</param>
        /// <param name="securityPrincipal">Indicates logged in user</param>
        /// <param name="command">Indicates Sql Command</param>
        /// <returns>Collection of Category ids</returns>
        public Collection<Int64> SearchCategories(SearchCriteria searchCriteria, SearchContext searchContext, String searchConfigurationXml, LocaleEnum systemDataLocale, SecurityPrincipal securityPrincipal, DBCommandProperties command)
        {
            SqlParameter[] parameters;
            String storedProcedureName = String.Empty;
            SqlDataReader reader = null;

            Collection<Int64> categoryIdList = new Collection<Int64>();

            try
            {
                //Get parameters populated
                parameters = GenerateInputParameters(searchContext, searchCriteria, searchConfigurationXml, systemDataLocale, securityPrincipal);

                storedProcedureName = "usp_SearchManager_Category_Search";

                reader = ExecuteProcedureReader(command.ConnectionString, parameters, storedProcedureName);

                if (reader != null)
                {
                    while (reader.Read())
                    {
                        Int64 categoryId = 0;

                        if (reader["CategoryId"] != null)
                            categoryId = ValueTypeHelper.Int64TryParse(reader["CategoryId"].ToString(), categoryId);

                        categoryIdList.Add(categoryId);
                    }
                }
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }

            //return 
            return categoryIdList;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Generates Container Id Table from Collcetion of ContainerIds
        /// </summary>
        /// <param name="containerIdList">Indicates containerId List in SearchCriteria</param>
        /// <param name="generator">Indicates parameter generator</param>
        /// <param name="parameters">Indicates list of parameters</param>
        /// <returns>List of Container Ids</returns>
        private List<SqlDataRecord> CreateContainerIdTable(Collection<Int32> containerIdList, SqlParametersGenerator generator, SqlParameter[] parameters)
        {
            List<SqlDataRecord> sqlContainerIdList = null;

            SqlMetaData[] sqlContainerIdMetadata = generator.GetTableValueMetadata("SearchManager_Entity_CategorySearch_ParametersArray", parameters[0].ParameterName);

            if (containerIdList.Count > 0)
            {
                sqlContainerIdList = new List<SqlDataRecord>();
                foreach (Int32 containerId in containerIdList)
                {
                    SqlDataRecord containerIdRecord = new SqlDataRecord(sqlContainerIdMetadata);
                    containerIdRecord.SetValue(0, containerId);
                    sqlContainerIdList.Add(containerIdRecord);
                }
            }
            return sqlContainerIdList;

        }

        /// <summary>
        /// Creates Category Id Table
        /// </summary>
        /// <param name="categoryIdList">Indicates list of category ids</param>
        /// <param name="generator">Indicates parameter generator</param>
        /// <param name="parameters">Indicates list of parameters</param>
        /// <returns>List of Category Ids</returns>
        private List<SqlDataRecord> CreateCategoryIdTable(Collection<Int64> categoryIdList, SqlParametersGenerator generator, SqlParameter[] parameters)
        {
            List<SqlDataRecord> categoryIdRecordList = null;

            SqlMetaData[] sqlCategoryIdList = generator.GetTableValueMetadata("SearchManager_Entity_CategorySearch_ParametersArray", parameters[1].ParameterName);

            if (categoryIdList.Count > 0)
            {
                categoryIdRecordList = new List<SqlDataRecord>();
                foreach (Int64 categoryId in categoryIdList)
                {
                    SqlDataRecord categoryIdRecord = new SqlDataRecord(sqlCategoryIdList);
                    categoryIdRecord.SetValue(0, categoryId);
                    categoryIdRecordList.Add(categoryIdRecord);
                }
            }

            return categoryIdRecordList;
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
            List<SqlDataRecord> attributeDetailsList = new List<SqlDataRecord>();

            SqlMetaData[] sqlAttributeDetailsList = generator.GetTableValueMetadata("SearchManager_Entity_CategorySearch_ParametersArray", parameters[2].ParameterName);

            foreach (SearchAttributeRule searchAttributeRule in searchAttributeRules)
            {
                //following Attribute Ids belong to workflow and needs to be passed to workflow search param table and if found notify telling workflow attributes found by setting field _containsWorkFlowAttributes true.
                //if (searchAttributeRule.Attribute.Id == 96 || searchAttributeRule.Attribute.Id == 97 || searchAttributeRule.Attribute.Id == 98 || searchAttributeRule.Attribute.Id == 99 || searchAttributeRule.Attribute.Id == 100)
                //{
                //    _containsWorkFlowAttributes = true;
                //    continue;
                //}
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

            return attributeDetailsList;

        }

        /// <summary>
        /// Creates Entity Type Table
        /// </summary>
        /// <param name="entityTypeIdList">Indicates entity types in Search Criteria</param>
        /// <param name="generator">Indicates parameter generator</param>
        /// <param name="parameters">Indicates list of parameters</param>
        /// <returns>Returns list of entity types</returns>
        private List<SqlDataRecord> CreateEntityTypeTable(Collection<Int32> entityTypeIdList, SqlParametersGenerator generator, SqlParameter[] parameters)
        {
            List<SqlDataRecord> entityTypeIdRecordList = new List<SqlDataRecord>();
            SqlMetaData[] sqlEntityTypeIdMetadata = generator.GetTableValueMetadata("SearchManager_Entity_CategorySearch_ParametersArray", parameters[5].ParameterName);

            foreach (Int32 entityTypeId in entityTypeIdList)
            {
                SqlDataRecord entityTypeIdRecord = new SqlDataRecord(sqlEntityTypeIdMetadata);
                entityTypeIdRecord.SetValue(0, entityTypeId);
                entityTypeIdRecordList.Add(entityTypeIdRecord);
            }

            return entityTypeIdRecordList;

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

            SqlMetaData[] sqlDisplayAttributeIdList = generator.GetTableValueMetadata("SearchManager_Entity_CategorySearch_ParametersArray", parameters[7].ParameterName);

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

            SqlMetaData[] sqlSearchConfigurationList = generator.GetTableValueMetadata("SearchManager_Entity_CategorySearch_ParametersArray", parameters[3].ParameterName);
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
                            else if (parameterName.ToLower().Equals("RelationshipSearchSource"))
                            {
                                searchConfigurationRecord.SetValue(8, xmlReader.ReadElementContentAsString());
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
            return searchConfigurationList;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchCriteria"></param>
        /// <param name="generator"></param>
        /// <param name="parameters"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        private List<SqlDataRecord> CreateAdvancedWorkflowAttributeTable(SearchCriteria searchCriteria, SqlParametersGenerator generator, SqlParameter[] parameters, Int32 userId)
        {
            List<SqlDataRecord> advancedWorkflowSearchParamList = new List<SqlDataRecord>();

            SqlMetaData[] sqlAdvancedWorkflowSearchParamtableMetadata = generator.GetTableValueMetadata("SearchManager_Entity_CategorySearch_ParametersArray", parameters[2].ParameterName);

            if (searchCriteria.WorkflowAssignedUsers != null)
            {
                foreach (String assignedTo in searchCriteria.WorkflowAssignedUsers)
                {
                    SqlDataRecord workflowAssignedUsersRecord = new SqlDataRecord(sqlAdvancedWorkflowSearchParamtableMetadata);
                    workflowAssignedUsersRecord.SetValue(0, 94);

                    //TODO:: UGLY WAY TO FIND OUT assignment..Correct it ASAP>>>
                    if (assignedTo == "0" || assignedTo.Equals("unassigned"))
                    {
                        workflowAssignedUsersRecord.SetValue(1, "0");
                        workflowAssignedUsersRecord.SetValue(3, "=");
                    }
                    else if (assignedTo == "1" || assignedTo.Equals("useactivityassignmentsettings") || assignedTo.Equals("assignedtocurrentuser"))
                    {
                        workflowAssignedUsersRecord.SetValue(1, userId.ToString());
                        workflowAssignedUsersRecord.SetValue(3, "=");
                    }
                    else if (assignedTo == "-1" || assignedTo.Equals("assignedtootherusers"))
                    {
                        workflowAssignedUsersRecord.SetValue(1, userId.ToString());
                        workflowAssignedUsersRecord.SetValue(3, "!=");
                    }
                    else //default is current user..cross  check this one..
                    {
                        workflowAssignedUsersRecord.SetValue(1, userId.ToString());
                        workflowAssignedUsersRecord.SetValue(3, "=");
                    }

                    // workflowAssignedUsersRecord.SetValue(1, id);
                    workflowAssignedUsersRecord.SetValue(2, null);

                    advancedWorkflowSearchParamList.Add(workflowAssignedUsersRecord);
                }
            }

            if (!String.IsNullOrWhiteSpace(searchCriteria.WorkflowName))
            {
                SqlDataRecord workflowNameRecord = new SqlDataRecord(sqlAdvancedWorkflowSearchParamtableMetadata);
                workflowNameRecord.SetValue(0, 91);
                workflowNameRecord.SetValue(1, searchCriteria.WorkflowName);
                workflowNameRecord.SetValue(2, null);
                workflowNameRecord.SetValue(3, "=");
                advancedWorkflowSearchParamList.Add(workflowNameRecord);
            }

            if (searchCriteria.WorkflowStages != null && searchCriteria.WorkflowStages.Length > 0)
            {
                foreach (var id in searchCriteria.WorkflowStages)
                {
                    SqlDataRecord workflowVersionRecord = new SqlDataRecord(sqlAdvancedWorkflowSearchParamtableMetadata);
                    workflowVersionRecord.SetValue(0, 92);
                    workflowVersionRecord.SetValue(1, id);
                    workflowVersionRecord.SetValue(2, null);
                    workflowVersionRecord.SetValue(3, "=");
                    advancedWorkflowSearchParamList.Add(workflowVersionRecord);
                }
            }

            if (advancedWorkflowSearchParamList.Count == 0)
            {
                advancedWorkflowSearchParamList = null;
            }

            return advancedWorkflowSearchParamList;
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

            SqlMetaData[] sqlAdvancedWorkflowSearchParamtableMetadata = generator.GetTableValueMetadata("SearchManager_Entity_CategorySearch_ParametersArray", parameters[6].ParameterName);

            if (!String.IsNullOrWhiteSpace(searchCriteria.WorkflowType))
            {
                SqlDataRecord workflowTypeRecord = new SqlDataRecord(sqlAdvancedWorkflowSearchParamtableMetadata);
                workflowTypeRecord.SetValue(0, "WorkflowType");
                workflowTypeRecord.SetValue(1, searchCriteria.WorkflowType);
                workflowTypeRecord.SetValue(2, "=");
                advancedWorkflowSearchParamList.Add(workflowTypeRecord);
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

            if (advancedWorkflowSearchParamList.Count == 0)
            {
                advancedWorkflowSearchParamList = null;
            }

            return advancedWorkflowSearchParamList;
        }

        /// <summary>
        /// Creates a record for SearchWeightage attributes
        /// </summary>
        /// <param name="searchCriteria">Indicates Search Criteria</param>
        /// <param name="generator">Indicates parameter generator</param>
        /// <param name="parameters">Indicates list of parameters</param>
        /// <returns>Returns list of search weightage Attribute Values</returns>
        private List<SqlDataRecord> CreateSearchWeightageAttributesTable(SearchCriteria searchCriteria, SqlParametersGenerator generator, SqlParameter[] parameters)
        {
            SqlMetaData[] searchWeightageAttributes = generator.GetTableValueMetadata("SearchManager_Entity_CategorySearch_ParametersArray", parameters[8].ParameterName);
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
        /// Generates list of  parameters
        /// </summary>
        /// <param name="searchContext">Indicates Search Context</param>
        /// <param name="searchCriteria">Indicates Search Criteria</param>
        /// <param name="searchConfigurationXml">Indicates searchConfigurationXml</param>
        /// <param name="systemDataLocale">Indicates systemDataLocale</param>
        /// <param name="securityPrincipal">Indicates user</param>
        /// <returns>Returns list pf parameters populates</returns>
        SqlParameter[] GenerateInputParameters(SearchContext searchContext, SearchCriteria searchCriteria, String searchConfigurationXml, LocaleEnum systemDataLocale, SecurityPrincipal securityPrincipal)
        {
            SqlParameter[] parameters;
            Int32 roleId = 0;
            Int32 userId = 0;
            String currentUser = String.Empty;

            SqlParametersGenerator generator = new SqlParametersGenerator("SearchManager_SqlParameters");

            parameters = generator.GetParameters("SearchManager_Entity_CategorySearch_ParametersArray");

            if (securityPrincipal != null)
            {
                currentUser = securityPrincipal.CurrentUserName;
                roleId = securityPrincipal.UserPreferences.DefaultRoleId;
                userId = securityPrincipal.UserPreferences.LoginId;
            }

            //Create list of Container Ids
            List<SqlDataRecord> containerIdList = null;
            if (searchCriteria.ContainerIds != null)
            {
                containerIdList = new List<SqlDataRecord>();
                containerIdList = CreateContainerIdTable(searchCriteria.ContainerIds, generator, parameters);
            }

            //Create list of Category ids
            List<SqlDataRecord> categoryIdList = null;
            if (searchCriteria.CategoryIds != null)
            {
                categoryIdList = new List<SqlDataRecord>();
                categoryIdList = CreateCategoryIdTable(searchCriteria.CategoryIds, generator, parameters);
            }

            //Create list of attribute details (attributes to be searched.
            List<SqlDataRecord> attributeDetailsList = null;
            if (searchCriteria.SearchAttributeRules != null)
            {
                attributeDetailsList = new List<SqlDataRecord>();
                attributeDetailsList = CreateSearchAttributeTable(searchCriteria.SearchAttributeRules, generator, parameters);
            }

            //Create searchConfiguration List reading Search Configuration Xml
            List<SqlDataRecord> searchConfigurationList = null;
            if (searchConfigurationXml != null)
            {
                searchConfigurationList = new List<SqlDataRecord>();
                searchConfigurationList = CreateSearchConfigurationTable(searchConfigurationXml, searchContext, generator, parameters);
            }

            //Create list of Locales
            List<SqlDataRecord> localeDetailsList = null;
            if (searchCriteria.Locales != null)
            {
                localeDetailsList = new List<SqlDataRecord>();
                SqlMetaData[] sqlLocalesMetadata = generator.GetTableValueMetadata("SearchManager_Entity_CategorySearch_ParametersArray", parameters[4].ParameterName);
                localeDetailsList = EntityDataReaderUtility.CreateLocaleTable(searchCriteria.Locales, (Int32)systemDataLocale, sqlLocalesMetadata);
            }

            //Create list of entity type id
            List<SqlDataRecord> entityTypeList = null;
            if (searchCriteria.EntityTypeIds != null)
            {
                entityTypeList = new List<SqlDataRecord>();
                entityTypeList = CreateEntityTypeTable(searchCriteria.EntityTypeIds, generator, parameters);
            }

            //Create list of Display Attributes
            List<SqlDataRecord> displayAttributeIdList = null;
            if (searchContext.ReturnAttributeList != null)
            {
                displayAttributeIdList = new List<SqlDataRecord>();
                displayAttributeIdList = CreateDisplayAttributeTable(searchContext.ReturnAttributeList, generator, parameters);
            }

            List<SqlDataRecord> advancedWorkflowAttributeIdList = CreateAdvancedWorkflowAttributeTable(searchCriteria, generator, parameters, userId);
            if (advancedWorkflowAttributeIdList != null && advancedWorkflowAttributeIdList.Count > 0)
            {
                if (attributeDetailsList == null)
                    attributeDetailsList = new List<SqlDataRecord>();

                attributeDetailsList.AddRange(advancedWorkflowAttributeIdList);
            }

            List<SqlDataRecord> workflowConfigList = null;
            if (advancedWorkflowAttributeIdList != null && advancedWorkflowAttributeIdList.Count > 0)
            {
                workflowConfigList = new List<SqlDataRecord>();
                workflowConfigList = CreateWorkflowConfigurationTable(searchCriteria, generator, parameters);
            }

            //Create Search Weightage
            List<SqlDataRecord> searchWeightageAttributes = null;
            searchWeightageAttributes = CreateSearchWeightageAttributesTable(searchCriteria, generator, parameters);

            parameters[0].Value = containerIdList;
            parameters[1].Value = categoryIdList;
            parameters[2].Value = attributeDetailsList;
            parameters[3].Value = searchConfigurationList;
            parameters[4].Value = localeDetailsList;
            parameters[5].Value = entityTypeList;
            parameters[6].Value = workflowConfigList;
            parameters[7].Value = displayAttributeIdList; //How do we get Name and Locale
            parameters[8].Value = searchWeightageAttributes;
            parameters[9].Value = currentUser;
            //parameters[10].Value  SP's @TotalCount output parameter

            return parameters;

        }

        #endregion

        #endregion
    }
}
