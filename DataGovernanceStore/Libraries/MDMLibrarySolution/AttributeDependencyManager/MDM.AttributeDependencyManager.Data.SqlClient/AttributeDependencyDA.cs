using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Text;
using Microsoft.SqlServer.Server;
using System.Linq;
using System.Transactions;
using System.Diagnostics;
using System.Data;

namespace MDM.AttributeDependencyManager.Data
{
    using MDM.BusinessObjects;
    using MDM.Core;
    using MDM.Utility;
    using MDM.Interfaces;


    /// <summary>
    /// Specifies the data access operations for Dependent Attribute
    /// </summary>
    public class AttributeDependencyDA : SqlClientDataAccessBase
    {
        #region Fields

        #endregion

        #region Constructors

        #endregion

        #region Properties

        #endregion

        #region Public Methods

        /// <summary>
        /// Get dependency mapping details for requested attribute from data base
        /// This method will return the link table details. 
        /// For example if the the attribute is lookup then will return the WSID of the lookup table which mapped to the requested attribute.
        /// If the attribute is non-lookup attribute then will return the dependent values based on the mapping link.
        /// </summary>
        /// <param name="attributeId">Id of the attribute for which dependency details needs to be fetched</param>
        /// <param name="applicationContextId">Id of the filtered context of the application</param>
        /// <param name="dependentAttributes">Dependency Attribute mapping details for the attribute</param>
        /// <param name="command">Indicates SQL command</param>
        /// <returns>Collection of string values</returns>
        public Collection<String> GetDependencyMappings(Int32 attributeId, Int32 applicationContextId, DependentAttributeCollection dependentAttributes, DBCommandProperties command)
        {
            SqlParameter[] parameters;
            String connectionString = String.Empty;
            String storedProcedureName = String.Empty;
            SqlDataReader reader = null;

            Collection<String> dependentAttributesMapping = new Collection<String>();

            try
            {
                connectionString = AppConfigurationHelper.ConnectionString;

                SqlParametersGenerator generator = new SqlParametersGenerator("AttributeDependencyManager_SqlParameters");

                parameters = generator.GetParameters("AttributeDependencyManager_AttributeDependency_MapGet_ParametersArray");

                #region Populate table value parameters and also populate return result collection

                List<SqlDataRecord> dependentParentList = new List<SqlDataRecord>();

                SqlMetaData[] sqlDependentParents = generator.GetTableValueMetadata("AttributeDependencyManager_AttributeDependency_MapGet_ParametersArray", parameters[2].ParameterName);

                foreach (DependentAttribute dAttr in dependentAttributes)
                {
                    SqlDataRecord sqlDataRecord = new SqlDataRecord(sqlDependentParents);

                    sqlDataRecord.SetValue(0, dAttr.AttributeId.ToString());
                    sqlDataRecord.SetValue(1, dAttr.AttributeValue);
                    dependentParentList.Add(sqlDataRecord);
                }

                #endregion

                parameters[0].Value = attributeId;
                parameters[1].Value = applicationContextId;
                parameters[2].Value = dependentParentList;

                storedProcedureName = "usp_AttributeDependencyManager_Dependent_Search";

                reader = ExecuteProcedureReader(command.ConnectionString, parameters, storedProcedureName);

                if (reader != null)
                {
                    while (reader.Read())
                    {
                        if (reader["Result"] != null)
                        {
                            dependentAttributesMapping.Add(reader["Result"].ToString());
                        }
                    }
                }
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }

            return dependentAttributesMapping;
        }

        /// <summary>
        /// This method will take dependent child attribute and dependent parent attribute with its values, 
        /// and DB will return the indication if given value is valid or not in context of given dependency.
        /// </summary>
        /// <param name="attribute">Attribute for which dependency details needs to be validated</param>
        /// <param name="applicationContextId">ApplicationContext Id of current context for dependency.</param>
        /// <param name="dependentAttributes">Dependent of Attribute mapping details(Parent attribute mapping) for the attribute</param>
        /// <param name="command">Indicates DBCommand object having connection retated information</param>
        /// <returns>Collection of key value pair - where Key is actual value of attribute and value indicates if it is valid for given dependency or not.</returns>
        public Collection<KeyValuePair<String, Boolean>> AreDependentValuesValid(IAttribute attribute, Int32 applicationContextId, DependentAttributeCollection dependentAttributes, DBCommandProperties command)
        {
            SqlParameter[] parameters;
            String storedProcedureName = String.Empty;
            SqlDataReader reader = null;
            Collection<KeyValuePair<String, Boolean>> validationStatus = new Collection<KeyValuePair<String, Boolean>>();

            try
            {
                SqlParametersGenerator generator = new SqlParametersGenerator("AttributeDependencyManager_SqlParameters");

                parameters = generator.GetParameters("AttributeDependencyManager_AttributeDependency_IsValid_ParametersArray");

                #region Populate table value parameters and also populate return result collection

                List<SqlDataRecord> dependentParentList = null;

                if (dependentAttributes.Count > 0)
                {
                    dependentParentList = new List<SqlDataRecord>();
                    SqlMetaData[] sqlDependentParents = generator.GetTableValueMetadata("AttributeDependencyManager_AttributeDependency_IsValid_ParametersArray", parameters[2].ParameterName);

                    foreach (DependentAttribute dAttr in dependentAttributes)
                    {
                        SqlDataRecord sqlDataRecord = new SqlDataRecord(sqlDependentParents);

                        sqlDataRecord.SetValue(0, dAttr.AttributeId.ToString());
                        sqlDataRecord.SetValue(1, dAttr.AttributeValue);
                        dependentParentList.Add(sqlDataRecord);
                    }
                }

                #endregion

                #region Populate table value parameters and also populate return result collection

                List<SqlDataRecord> childValues = null;

                if (attribute.HasValue(false))
                {
                    childValues = new List<SqlDataRecord>();
                    SqlMetaData[] sqlChildValues = generator.GetTableValueMetadata("AttributeDependencyManager_AttributeDependency_IsValid_ParametersArray", parameters[3].ParameterName);
                    
                    var attrGetOverriddenValues = attribute.GetOverriddenValues();

                    if (attrGetOverriddenValues != null && attrGetOverriddenValues.Count > 0)
                    {
                        foreach (Value value in attrGetOverriddenValues)
                        {
                            if (value.AttrVal != null)
                            {
                                SqlDataRecord sqlDataRecord = new SqlDataRecord(sqlChildValues);

                                sqlDataRecord.SetValue(0, value.AttrVal);
                                childValues.Add(sqlDataRecord);
                            }
                        }
                    }
                    else
                    {
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Warning, String.Format("GetOverriddenValues: Attribute Value is null: Id - {0}", attribute.Id.ToString(), MDMTraceSource.Attribute));
                    }

                    if (childValues.Count <= 0)
                    {
                        childValues = null;
                    }
                }

                #endregion

                parameters[0].Value = attribute.Id;
                parameters[1].Value = applicationContextId;
                parameters[2].Value = dependentParentList;
                parameters[3].Value = childValues;

                storedProcedureName = "usp_AttributeDependencyManager_DependentValue_IsValid";

                reader = ExecuteProcedureReader(command.ConnectionString, parameters, storedProcedureName);

                if (reader != null)
                {
                    while (reader.Read())
                    {
                        String childValue = String.Empty;
                        Boolean isValid = false;

                        if (reader["Result"] != null)
                        {
                            childValue = reader["Result"].ToString();
                        }
                        if (reader["IsValid"] != null)
                        {
                            isValid = ValueTypeHelper.BooleanTryParse(reader["IsValid"].ToString(), false);
                        }

                        validationStatus.Add(new KeyValuePair<String, Boolean>(childValue, isValid));
                    }
                }
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }

            return validationStatus;
        }

        /// <summary>
        /// Get the dependency Details for the requested attribute from data base.
        /// </summary>
        /// <param name="attributeId">Id of the attribute for which dependency details needs to be fetched</param>
        /// <param name="applicationContextId">Id of the filtered context of the application</param>
        /// <param name="includeChildDependency">Indicates whether load children dependency details or not</param>
        /// <param name="command">Indicates SQL command</param>
        /// <returns>DependentAttributeCollection</returns>
        public DependentAttributeCollection GetDependencyDetails(Int32 attributeId, Int32 applicationContextId, Boolean includeChildDependency, DBCommandProperties command)
        {
            SqlParameter[] parameters;
            String storedProcedureName = String.Empty;
            StringBuilder returnValue = new StringBuilder();
            SqlDataReader reader = null;

            DependentAttributeCollection retrunValue = new DependentAttributeCollection();

            try
            {
                SqlParametersGenerator generator = new SqlParametersGenerator("AttributeDependencyManager_SqlParameters");

                parameters = generator.GetParameters("AttributeDependencyManager_AttributeDependency_Get_ParametersArray");

                parameters[0].Value = attributeId;
                parameters[1].Value = applicationContextId;
                parameters[2].Value = includeChildDependency;

                //Todo::Change the procedure name
                storedProcedureName = "usp_AttributeDependencyManager_Dependent_Attribute_Get";

                reader = ExecuteProcedureReader(command.ConnectionString, parameters, storedProcedureName);

                if (reader != null)
                {
                    retrunValue = ReadValues(reader);
                }
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }

            return retrunValue;
        }

        /// <summary>
        /// Get the dependent attribute data for the requested link table
        /// </summary>
        /// <param name="linkTableName">Indicates the link table name</param>
        /// <param name="returnOnlyModel">Specifies whether to return only model or complete information</param>
        /// <param name="maxRecordsToReturn">Indicates the max records to be returned.</param>
        /// <param name="locale">Indicates the locale</param>
        /// <param name="command">Indicates SQL command</param>
        /// <returns>Returns the dependent attribute data mapping collection</returns>
        public DependentAttributeDataMapCollection GetDependentData(String linkTableName, Boolean returnOnlyModel, Int32 maxRecordsToReturn, LocaleEnum locale, DBCommandProperties command)
        {
            SqlParameter[] parameters;
            String connectionString = String.Empty;
            String storedProcedureName = String.Empty;
            StringBuilder returnValue = new StringBuilder();
            SqlDataReader reader = null;
            DataTable dtModel = null;
            DataSet dsData = null;

            DependentAttributeDataMapCollection dependentAttributeDataMaps = new DependentAttributeDataMapCollection();

            try
            {
                connectionString = AppConfigurationHelper.ConnectionString;

                SqlParametersGenerator generator = new SqlParametersGenerator("AttributeDependencyManager_SqlParameters");

                parameters = generator.GetParameters("AttributeDependencyManager_AttributeDependency_GetDependentData_ParametersArray");

                parameters[0].Value = linkTableName;
                parameters[1].Value = returnOnlyModel;
                parameters[2].Value = maxRecordsToReturn;

                storedProcedureName = "usp_AttributeDependencyManager_Dependent_Get";

                reader = ExecuteProcedureReader(command.ConnectionString, parameters, storedProcedureName);

                if (reader != null)
                {
                    //First read the lookup model's table in dtModel. The get the name of tables from 1st column(FK_Attribute)
                    //give array of table names to load all tables in dsData. 
                    dtModel = new DataTable();
                    dsData = new DataSet();

                    dtModel.Load(reader);
                    if (dtModel != null && dtModel.Rows != null && dtModel.Rows.Count > 0
                        && dtModel.Columns != null && dtModel.Columns.Contains("LinkTableName"))
                    {
                        String tableNames = String.Empty;
                        foreach (DataRow row in dtModel.Rows)
                        {
                            tableNames = String.Concat(tableNames, row["LinkTableName"].ToString(), ",");
                        }

                        if (tableNames.Length > 0)
                        {
                            tableNames = tableNames.Remove(tableNames.Length - 1);
                        }

                        dtModel.TableName = "Model";
                        dsData.Tables.Add(dtModel);
                        dsData.Load(reader, LoadOption.Upsert, tableNames.Split(','));
                    }
                }

                dependentAttributeDataMaps = new DependentAttributeDataMapCollection(dsData);
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }

            return dependentAttributeDataMaps;
        }

        public OperationResultCollection Process(Int32 attributeId, DependentAttributeCollection dependentAttributes, String programName, String userName, LocaleEnum systemDataLocale, DBCommandProperties command)
        {
            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StartTraceActivity("AttributeDependencyDA.Process", false);

            SqlDataReader reader = null;
            OperationResultCollection contextProcessOperationResults = new OperationResultCollection();

            SqlParametersGenerator generator = new SqlParametersGenerator("AttributeDependencyManager_SqlParameters");

            const String storedProcedureName = "usp_AttributeModelManager_Dependent_Attribute_Process";

            using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = Constants.DEFAULT_ISOLATION_LEVEL }))
            {
                try
                {
                    List<SqlDataRecord> dependentAttributeTable;

                    SqlParameter[] parameters = generator.GetParameters("AttributeDependencyManager_DependentAttribute_Process_ParametersArray");

                    SqlMetaData[] dependentAttributeMetaData = generator.GetTableValueMetadata("AttributeDependencyManager_DependentAttribute_Process_ParametersArray", parameters[0].ParameterName);

                    CreateDependentAttributeTable(attributeId, dependentAttributes, dependentAttributeMetaData, out dependentAttributeTable);

                    parameters[0].Value = dependentAttributeTable;
                    parameters[1].Value = programName;
                    parameters[2].Value = userName;

                    reader = ExecuteProcedureReader(command.ConnectionString, parameters, storedProcedureName);

                    PopulateOperationResultsForDependentAttributes(reader, dependentAttributes, contextProcessOperationResults);
                    transactionScope.Complete();
                }
                catch (Exception exception)
                {
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, "DependentAttribute Process Failed." + exception.Message, MDMTraceSource.DataModel);
                    OperationResult or = contextProcessOperationResults.FirstOrDefault();
                    if (or == null)
                    {
                        or = new OperationResult();
                    }
                    or.AddOperationResult(exception.Message, String.Empty, OperationResultType.Error);

                }
                finally
                {
                    if (reader != null)
                        reader.Close();
                    if (Constants.TRACING_ENABLED)
                        MDMTraceHelper.StopTraceActivity("AttributeDependencyDA.Process");
                }
            }

            return contextProcessOperationResults;
        }

        /// <summary>
        /// Processes Multiple Dependent data
        /// </summary>
        /// <param name="dependentAttributeDataMaps">Dependent Attribute Data Mapping which needs to be processed</param>
        /// <param name="callerContext">Indicates Application and Module name by which action is being performed</param>
        /// <returns>Result of the process</returns>
        public OperationResultCollection ProcessDependentData(DependentAttributeDataMapCollection dependentAttributeDataMaps, String userName, String programName, DBCommandProperties command)
        {
            OperationResultCollection operationResults = new OperationResultCollection();

            using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = Constants.DEFAULT_ISOLATION_LEVEL }))
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StartTraceActivity("AttributeDependencyDA.ProcessDependentData", false);

                SqlDataReader reader = null;
                SqlParameter[] parameters;
                String storedProcedureName = String.Empty;
                String dependentAttributeDataMapsXml = dependentAttributeDataMaps.ToXml(ObjectSerialization.ProcessingOnly);

                try
                {
                    SqlParametersGenerator generator = new SqlParametersGenerator("AttributeDependencyManager_SqlParameters");

                    parameters = generator.GetParameters("AttributeDependencyManager_DependentAttribute_ProcessDependentData_ParametersArray");

                    parameters[0].Value = dependentAttributeDataMapsXml;
                    parameters[1].Value = "";//TODO:: Not used right now. Need to check what is it used for or what will be the future use.
                    parameters[2].Value = programName;
                    parameters[3].Value = userName;

                    storedProcedureName = "usp_AttributeDependencyManager_Dependent_Process";

                    reader = ExecuteProcedureReader(command.ConnectionString, parameters, storedProcedureName);

                    PopulateOperationResultsForDependentAttributeMap(reader, dependentAttributeDataMaps, operationResults);

                }
                finally
                {
                    if (reader != null)
                        reader.Close();
                }

                if (operationResults.OperationResultStatus == OperationResultStatusEnum.None || operationResults.OperationResultStatus == OperationResultStatusEnum.Successful)
                    transactionScope.Complete();

                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StopTraceActivity("AttributeDependencyDA.ProcessDependentData");
            }

            return operationResults;
        }

        #endregion Public Methods

        #region Private Methods

        private void CreateDependentAttributeTable(Int32 attributeId, DependentAttributeCollection dependentAttributes, SqlMetaData[] dependentAttributeMetaData, out List<SqlDataRecord> dependentAttributeTable)
        {
            dependentAttributeTable = null;

            if (dependentAttributes != null && dependentAttributes.Count > 0)
            {
                dependentAttributeTable = new List<SqlDataRecord>();

                foreach (DependentAttribute dependency in dependentAttributes)
                {
                    foreach (ApplicationContext context in dependency.ApplicationContexts)
                    {
                        SqlDataRecord attrRecord = new SqlDataRecord(dependentAttributeMetaData);
                        attrRecord.SetValue(0, dependency.Id);
                        attrRecord.SetValue(1, dependency.AttributeId);
                        attrRecord.SetValue(2, dependency.ParentAttributeId);
                        attrRecord.SetValue(3, context.Id);
                        attrRecord.SetValue(4, dependency.LinkTableName);
                        if (dependency.Action == ObjectAction.Create
                          || dependency.Action == ObjectAction.Delete)
                        {
                            attrRecord.SetValue(5, dependency.Action.ToString());
                        }

                        if (dependency.Action == ObjectAction.Update)
                        {
                            attrRecord.SetValue(5, context.Action.ToString());
                        }


                        dependentAttributeTable.Add(attrRecord);
                    }
                }
            }
        }

        private DependentAttributeCollection ReadValues(SqlDataReader reader)
        {
            DependentAttributeCollection dependentAttributes = new DependentAttributeCollection();

            while (reader.Read())
            {
                Int32 attributeId = 0;
                Int32 parentAttributeId = 0;
                String attributeName = String.Empty;
                String parentAttributeName = String.Empty;
                String linkTableName = String.Empty;
                DependencyType dependencyType = DependencyType.Unknown;
                Int32 dependentId = -1;
                Int32 contextId = -1;
                Int32 dependentLevel = 0;

                #region Read values

                if (reader["AttributeName"] != null)
                {
                    attributeName = reader["AttributeName"].ToString();
                }

                if (reader["ParentAttributeName"] != null)
                {
                    parentAttributeName = reader["ParentAttributeName"].ToString();
                }

                if (reader["FK_Attribute"] != null)
                {
                    attributeId = ValueTypeHelper.Int32TryParse(reader["FK_Attribute"].ToString(), 0);
                }

                if (reader["FK_Attribute_Dependent"] != null)
                {
                    parentAttributeId = ValueTypeHelper.Int32TryParse(reader["FK_Attribute_Dependent"].ToString(), 0);
                }

                if (reader["DependentLevel"] != null)
                {
                    dependentLevel = ValueTypeHelper.Int32TryParse(reader["DependentLevel"].ToString(), 0);
                }

                if (reader["LinkTableName"] != null)
                {
                    linkTableName = reader["LinkTableName"].ToString();
                }

                if (reader["DependencyType"] != null)
                {
                    DependencyType val = DependencyType.Unknown;
                    Enum.TryParse<DependencyType>(reader["DependencyType"].ToString(), out val);

                    dependencyType = val;
                }

                if (reader["PK_Dependent_Attribute"] != null)
                {
                    dependentId = ValueTypeHelper.Int32TryParse(reader["PK_Dependent_Attribute"].ToString(), 0);
                }

                if (reader["FK_Application_Context"] != null)
                {
                    contextId = ValueTypeHelper.Int32TryParse(reader["FK_Application_Context"].ToString(), 0);
                }

                #endregion Read values

                DependentAttribute dependentAttr = new DependentAttribute();
                dependentAttr.AttributeId = attributeId;
                dependentAttr.AttributeName = attributeName;
                dependentAttr.DependencyType = dependencyType;
                dependentAttr.LinkTableName = linkTableName;
                dependentAttr.Id = dependentId;
                dependentAttr.ParentAttributeId = parentAttributeId;
                dependentAttr.ParentAttributeName = parentAttributeName;
                ApplicationContext appContext = new ApplicationContext();
                appContext.Id = contextId;
                //dependentAttr.ApplicationContexts.Add(appContext);
                dependentAttr.DependentLevel = dependentLevel;
                //dependentAttributes.Add(dependentAttr);

                AddApplicationContext(dependentAttr, appContext, dependentAttributes);
            }

            return dependentAttributes;
        }

        private void AddApplicationContext(DependentAttribute dependentAttr, ApplicationContext appContext, DependentAttributeCollection dependentAttributes)
        {
            DependentAttribute attr = null;

            //Check if given attribute dependency is already added in collection.
            //Check is performed on attribute Id and parent dependent attribute Id
            var filter = from depAttr in dependentAttributes
                         where depAttr.AttributeId == dependentAttr.AttributeId && depAttr.ParentAttributeId == dependentAttr.ParentAttributeId
                         select depAttr;

            if (filter != null && filter.Any())
            {
                attr = (DependentAttribute)filter.ToList<DependentAttribute>().FirstOrDefault();
            }

            //Get dependent Attribute method will return dependent attribute always. If the attribute is not present as well.
            //Re check whether dependent attribute is empty or not.
            if (attr != null && attr.AttributeId == dependentAttr.AttributeId)
            {
                if (!attr.ApplicationContexts.Contains(appContext.Id))
                {
                    attr.ApplicationContexts.Add(appContext);
                }
            }
            else
            {
                dependentAttr.ApplicationContexts.Add(appContext);
                dependentAttributes.Add(dependentAttr);
            }
        }

        private void PopulateOperationResultsForDependentAttributeMap(SqlDataReader reader, DependentAttributeDataMapCollection dependentAttributeDataMaps, OperationResultCollection operationResults)
        {
            while (reader.Read())
            {
                Int64 id = 0;
                Int32 rowId = 0;
                Boolean hasError = false;
                String errorCode = String.Empty;
                Int64 linkTableId = 0;

                if (reader["Id"] != null)
                    Int64.TryParse(reader["Id"].ToString(), out id);
                if (reader["RowId"] != null)
                    Int32.TryParse(reader["RowId"].ToString(), out rowId);
                if (reader["LinkTableId"] != null)
                    Int64.TryParse(reader["LinkTableId"].ToString(), out linkTableId);
                if (reader["HasError"] != null)
                    Boolean.TryParse(reader["HasError"].ToString(), out hasError);
                if (reader["ErrorCode"] != null)
                    errorCode = reader["ErrorCode"].ToString();

                //Get AttributeModel
                DependentAttributeDataMap dependentAttributeDataMap = dependentAttributeDataMaps.SingleOrDefault(a => a.Id == linkTableId);

                //Get AttributeOperationResult
                OperationResult operationResult = operationResults.SingleOrDefault(or => or.Id == linkTableId);

                if (operationResult == null)
                {
                    operationResult = new OperationResult();
                    operationResults.Add(operationResult);
                }

                if (dependentAttributeDataMap != null && operationResult != null)
                {
                    if (id < 0)
                    {
                        Row row = dependentAttributeDataMap.Rows.GetRow(id);
                        //Update the id with the new entityId
                        row.Id = rowId;
                        operationResult.Id = rowId;
                    }

                    if (hasError)
                    {
                        operationResult.AddOperationResult(errorCode, String.Empty, OperationResultType.Error);

                    }
                }
            }

            operationResults.RefreshOperationResultStatus();
        }

        private void PopulateOperationResultsForDependentAttributes(SqlDataReader reader, DependentAttributeCollection dependentAttributes, OperationResultCollection operationResults)
        {
            while (reader.Read())
            {
                Int64 id = 0;
                Int32 dependentAttributeId = 0;
                Boolean hasError = false;
                String errorCode = String.Empty;

                if (reader["Id"] != null)
                    Int64.TryParse(reader["Id"].ToString(), out id);
                if (reader["DependentAttributeId"] != null)
                    Int32.TryParse(reader["DependentAttributeId"].ToString(), out dependentAttributeId);
                if (reader["HasError"] != null)
                    Boolean.TryParse(reader["HasError"].ToString(), out hasError);
                if (reader["ErrorCode"] != null)
                    errorCode = reader["ErrorCode"].ToString();

                //Get AttributeModel
                DependentAttribute dependentAttribute = dependentAttributes.SingleOrDefault(a => a.Id == id);

                //Get AttributeOperationResult
                OperationResult operationResult = operationResults.SingleOrDefault(or => or.Id == id);

                if (operationResult == null)
                {
                    operationResult = new OperationResult();
                    operationResults.Add(operationResult);
                }

                if (dependentAttribute != null && operationResult != null)
                {
                    if (id < 0)
                    {
                        //Update the id with the new entityId
                        dependentAttribute.Id = dependentAttributeId;
                        operationResult.Id = dependentAttributeId;
                    }

                    if (hasError)
                    {
                        operationResult.AddOperationResult(errorCode, String.Empty, OperationResultType.Error);

                    }
                }
            }
            operationResults.RefreshOperationResultStatus();
        }

        #endregion Private Methods
    }
}
