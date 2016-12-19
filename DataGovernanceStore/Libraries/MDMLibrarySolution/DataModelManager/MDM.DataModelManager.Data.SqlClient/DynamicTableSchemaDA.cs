using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Transactions;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace MDM.DataModelManager.Data
{
    using MDM.Core;
    using MDM.Core.DataModel;
    using MDM.Utility;
    using MDM.BusinessObjects;
    using MDM.BusinessObjects.DynamicTableSchema;
    using MDM.BusinessObjects.DataModel;

    /// <summary>
    /// Specifies Dynamic Table Schema Data Access
    /// </summary>
    public class DynamicTableSchemaDA : SqlClientDataAccessBase
    {
        #region Fields
        #endregion

        #region Constructors
        #endregion

        #region Properties
        #endregion

        #region Methods

        #region Public Methods

        /// <summary>
        /// Process table
        /// </summary>
        /// <param name="dbTable">This parameter is specifying instance of table to be processed</param>
        /// <param name="dynamicTableType">This parameter is specifying dynamic table type</param>
        /// <param name="loginUser">This parameter is specifying login user name</param>
        /// <param name="programName">This parameter is specifying program name</param>
        /// <param name="command">This parameter is specifying to which server we have to connect</param>
        /// <returns>Results of the operation having errors and information if any</returns>
        public OperationResult Process(DBTableCollection dbTables, DynamicTableType dynamicTableType, String loginUser, String programName, DBCommandProperties command)
        {
            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StartTraceActivity("DataModelManager.DynamicSchemaDA.Process", false);

            SqlDataReader reader = null;
            SqlParameter[] parameters;
            String storedProcedureName = String.Empty;
            OperationResult operationResult = new OperationResult();

            try
            {
                SqlParametersGenerator generator = new SqlParametersGenerator("DataModelManager_SqlParameters");

                parameters = generator.GetParameters("DataModelManager_DynamicTableSchema_Process_ParametersArray");

                parameters[0].Value = dbTables.ToXml();
                parameters[1].Value = dynamicTableType.ToString();
                parameters[2].Value = loginUser;
                parameters[3].Value = programName;

                storedProcedureName = "usp_DataModelManager_DynamicSchema_Process";

                reader = ExecuteProcedureReader(command.ConnectionString, parameters, storedProcedureName);

                if (reader != null)
                {
                    while (reader.Read())
                    {
                        if (reader["IsError"] != null)
                        {
                            if (reader["IsError"].ToString() == "1")
                            {
                                String errorMessage = String.Empty;
                                String errorCode = String.Empty;

                                if (reader["ErrorMessage"] != null)
                                {
                                    errorMessage = reader["ErrorMessage"].ToString();
                                }

                                if (reader["ErrorCode"] != null)
                                {
                                    errorCode = reader["ErrorCode"].ToString();
                                }

                                operationResult.AddOperationResult(errorCode, errorMessage, OperationResultType.Error);
                                
                                if (!String.IsNullOrWhiteSpace(errorMessage))
                                {
                                    //Procedure will return only SQL unhandle exception in error message column, only those messages need to be logged.
                                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, String.Format("Error occurred while processing DynamicTableSchema ErrorCode :{0}, ErrorMessage :{1}", errorCode, errorMessage), MDMTraceSource.DataModel);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, "DynamicTableSchema Process Failed." + exception.Message);
                operationResult.AddOperationResult(String.Empty, exception.Message, OperationResultType.Error);
            }

            finally
            {
                if (reader != null)
                    reader.Close();

                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StopTraceActivity("DataModelManager.DynamicSchemaDA.Process");
            }

            return operationResult;
        }

        /// <summary>
        /// Process table
        /// </summary>
        /// <param name="dbTable">This parameter is specifying instance of table to be processed</param>
        /// <param name="dynamicTableType">This parameter is specifying dynamic table type</param>
        /// <param name="loginUser">This parameter is specifying login user name</param>
        /// <param name="programName">This parameter is specifying program name</param>
        /// <param name="command">This parameter is specifying to which server we have to connect</param>
        /// <returns>Results of the operation having errors and information if any</returns>
        public void Process(Dictionary<DynamicTableType, DBTableCollection> dbTables, DataModelOperationResultCollection operationResults, String loginUser, String programName, DBCommandProperties command)
        {
            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StartTraceActivity("DataModelManager.DynamicSchemaDA.Process", false);

            SqlDataReader reader = null;
            SqlParameter[] parameters;
            String storedProcedureName = String.Empty;

            try
            {
                using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required))
                {
                    SqlParametersGenerator generator = new SqlParametersGenerator("DataModelManager_SqlParameters");

                    if (dbTables != null && dbTables.Count > 0)
                    {
                        foreach (DynamicTableType dynamicTableType in dbTables.Keys)
                        {
                            DBTableCollection dbTableCollection = dbTables[dynamicTableType];

                            if (dbTableCollection != null && dbTableCollection.Count > 0)
                            {
                                parameters = generator.GetParameters("DataModelManager_DynamicTableSchema_Process_ParametersArray");

                                parameters[0].Value = dbTableCollection.ToXml();
                                parameters[1].Value = dynamicTableType.ToString();
                                parameters[2].Value = loginUser;
                                parameters[3].Value = programName;

                                storedProcedureName = "usp_DataModelManager_DynamicSchema_Process";

                                reader = ExecuteProcedureReader(command.ConnectionString, parameters, storedProcedureName);

                                //In case if all lookup have read action then dbTableCollection count will be Zero.
                                //In that case reader will not be set, and will be null. 
                                //So populate operation result only when there is any lookup table schema to process.
                                PopulateOperationResult(reader, dbTableCollection, operationResults);

                                operationResults.RefreshOperationResultStatus();
                            }
                        }
                    }

                    transactionScope.Complete();
                }
            }
            catch (Exception exception)
            {
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, "DynamicTableSchema Process Failed." + exception.Message);
                operationResults.AddOperationResult(String.Empty, exception.Message, OperationResultType.Error);
            }
            finally
            {
                if (reader != null)
                    reader.Close();

                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StopTraceActivity("DataModelManager.DynamicSchemaDA.Process");
            }
        }

        /// <summary>
        /// Get Lookup, Complex and UOM Tables based on Table Name and Table Type
        /// </summary>
        /// <param name="tableName">This parameter is specifying Table Name.</param>
        /// <param name="dynamicTableType">This parameter is specifying Table Type.</param>
        /// <param name="command">This parameter is specifying to which server we have to connect</param>
        /// <param name="loadInternalColumns">Indicates whether to get internal columns of lookup or not.</param>
        /// <returns>returns DBTable object</returns>
        public DBTableCollection Get(String tableName, DynamicTableType dynamicTableType, DBCommandProperties command, Boolean loadInternalColumns)
        {
            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StartTraceActivity("DataModelManager.DynamicSchemaDA.Get", false);

            SqlDataReader reader = null;
            SqlParameter[] parameters;
            String storedProcedureName = "usp_DataModelManager_DynamicSchema_Get";
            DBTableCollection dbTables = new DBTableCollection();

            try
            {
                SqlParametersGenerator generator = new SqlParametersGenerator("DataModelManager_SqlParameters");
                parameters = generator.GetParameters("DataModelManager_DynamicTableSchema_Get_ParametersArray");

                parameters[0].Value = tableName;
                parameters[1].Value = (Int16)dynamicTableType;

                reader = ExecuteProcedureReader(command.ConnectionString, parameters, storedProcedureName);

                PopulateDBTables(reader, dbTables, loadInternalColumns);
            }
            finally
            {
                if (reader != null)
                    reader.Close();

                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StopTraceActivity("DataModelManager.DynamicSchemaDA.Get");
            }

            return dbTables;
        }

        /// <summary>
        /// Get Meta of Complex Table
        /// </summary>
        /// <param name="id">This parameter is specifying complex Attribute Id.</param>
        /// <param name="command">This parameter is specifying to which server we have to connect</param>
        /// <returns>returns DBTable object</returns>
        public DBTable Get(Int32 id, DBCommandProperties command)
        {
            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StartTraceActivity("DataModelManager.DynamicSchemaDA.Get", false);

            SqlDataReader reader = null;
            SqlParameter[] parameters;
            String storedProcedureName = String.Empty;
            DBTable dbTable = new DBTable();
            DBColumnCollection dbColumsCollection = new DBColumnCollection();

            try
            {
                SqlParametersGenerator generator = new SqlParametersGenerator("DataModelManager_SqlParameters");
                parameters = generator.GetParameters("DataModelManager_DynamicTableSchema_ComplexSchemaGet_ParametersArray");

                parameters[0].Value = id;

                storedProcedureName = "usp_DataModelManager_ComplexSchema_Get";

                reader = ExecuteProcedureReader(command.ConnectionString, parameters, storedProcedureName);

                if (reader != null)
                {
                    while (reader.Read())
                    {
                        DBColumn dbColumn = new DBColumn();

                        #region Read Table Properties

                        if (reader["TableName"] != null)
                            dbTable.Name = reader["TableName"].ToString();

                        if (reader["TableAction"] != null)
                        {
                            ObjectAction objectAction = ObjectAction.Unknown;
                            Enum.TryParse(reader["TableAction"].ToString(), out objectAction);
                            dbTable.Action = objectAction;
                        }

                        dbTable.PopulateRSTObject = true;

                        #endregion

                        #region Read Column Properties

                        if (reader["ColumnName"] != null)
                            dbColumn.Name = reader["ColumnName"].ToString();

                        if (reader["ColumnOldName"] != null)
                            dbColumn.OldName = reader["ColumnOldName"].ToString();

                        if (reader["ColumnDataType"] != null)
                            dbColumn.DataType = reader["ColumnDataType"].ToString();

                        if (reader["ColumnLength"] != null)
                            dbColumn.Length = ValueTypeHelper.Int32TryParse(reader["ColumnLength"].ToString(), 0);

                        if (reader["ColumnPrecision"] != null)
                            dbColumn.Precision = ValueTypeHelper.Int32TryParse(reader["ColumnPrecision"].ToString(), 0);

                        if (reader["ColumnDefaultValue"] != null)
                            dbColumn.DefaultValue = reader["ColumnDefaultValue"].ToString();

                        if (reader["ColumnNullable"] != null && !String.IsNullOrEmpty(reader["ColumnNullable"].ToString()))
                            dbColumn.Nullable = ValueTypeHelper.ConvertToBoolean(reader["ColumnNullable"].ToString());

                        if (reader["ColumnUnique"] != null && !String.IsNullOrEmpty(reader["ColumnUnique"].ToString()))
                            dbColumn.IsUnique = ValueTypeHelper.ConvertToBoolean(reader["ColumnUnique"].ToString());

                        if (reader["ColumnSequence"] != null)
                            dbColumn.Sequence = ValueTypeHelper.Int32TryParse(reader["ColumnSequence"].ToString(), 0);

                        if (reader["ColumnAction"] != null)
                        {
                            ObjectAction objectAction = ObjectAction.Unknown;
                            Enum.TryParse(reader["ColumnAction"].ToString(), out objectAction);
                            dbColumn.Action = objectAction;
                        }

                        dbColumsCollection.Add(dbColumn);

                        #endregion
                    }

                    dbTable.Columns = dbColumsCollection;
                }
            }
            finally
            {
                if (reader != null)
                    reader.Close();

                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StopTraceActivity("DataModelManager.DynamicSchemaDA.Get");
            }

            return dbTable;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dynamicTableType"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        public DBColumnCollection GetDefaultTemplate(DynamicTableType dynamicTableType, DBCommandProperties command)
        {
            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StartTraceActivity("DataModelManager.DynamicSchemaDA.GetDefaultTemplate", false);

            SqlDataReader reader = null;
            SqlParameter[] parameters;
            String storedProcedureName = "usp_Lookup_GetTableTypeTemplate";
            DBColumnCollection dbColumns = new DBColumnCollection();

            try
            {
                SqlParametersGenerator generator = new SqlParametersGenerator("DataModelManager_SqlParameters");
                parameters = generator.GetParameters("DataModelManager_DynamicTableTemplate_Get_ParametersArray");

                parameters[0].Value = (Int32)dynamicTableType;

                reader = ExecuteProcedureReader(command.ConnectionString, parameters, storedProcedureName);

                PopulateDefaultTemplate(reader, dbColumns);
            }
            finally
            {
                if (reader != null)
                    reader.Close();

                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StopTraceActivity("DataModelManager.DynamicSchemaDA.GetDefaultTemplate");
            }

            return dbColumns;
        }

        #endregion

        #region Private Methods

        /// <summary>
        ///
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="operationResults"></param>
        /// <returns></returns>
        private void PopulateOperationResult(SqlDataReader reader, DBTableCollection dbTables, DataModelOperationResultCollection operationResults)
        {
            while (reader.Read())
            {
                Boolean hasError = false;
                String errorMessage = String.Empty;
                String dbTableName = String.Empty;
                String errorCode = String.Empty;

                if (reader["TableName"] != null)
                {
                    dbTableName = reader["TableName"].ToString();
                }

                if (reader["IsError"] != null)
                {
                    hasError = ValueTypeHelper.BooleanTryParse(reader["IsError"].ToString(), false);
                }

                if (reader["ErrorMessage"] != null)
                {
                    errorMessage = reader["ErrorMessage"].ToString();
                }

                if (reader["ErrorCode"] != null)
                {
                    errorCode = reader["ErrorCode"].ToString();
                }

                DBTable dbTable = dbTables.Get(dbTableName);

                if (dbTable != null)
                {
                    DataModelOperationResult operationResult = operationResults.GetByReferenceId(dbTable.ReferenceId) as DataModelOperationResult;

                    if (operationResult != null)
                    {
                        if (hasError)
                        {
                            if (!String.IsNullOrWhiteSpace(errorMessage))
                            {
                                //Procedure will return only SQL unhandle exception in error message column, only those messages need to be logged.
                                MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, String.Format("Error occurred while processing DynamicTableSchema for Table {0} with ErrorCode :{1}, ErrorMessage :{2}", dbTableName, errorCode, errorMessage), MDMTraceSource.DataModel);
                            }
                            operationResult.AddOperationResult(errorCode, errorMessage, OperationResultType.Error);
                        }
                        else
                        {
                            operationResult.OperationResultStatus = OperationResultStatusEnum.Successful;
                        }
                    }
                    else
                    {
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, String.Format("{0} table could not be found.", dbTableName), MDMTraceSource.DataModel);
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="dbTables"></param>
        /// <param name="loadInternalColumns"></param>
        private void PopulateDBTables(SqlDataReader reader, DBTableCollection dbTables, Boolean loadInternalColumns)
        {
            while (reader.Read())
            {
                String tableName = String.Empty;
                String checkConstraintValue = String.Empty;

                //Specifies the internal column names of lookup.
                Collection<String> internalColumnNames = InternalObjectCollection.LookupInternalColumnNames;
                

                if (reader["TableName"] != null)
                    tableName = reader["TableName"].ToString().Replace("tblk_", String.Empty);

                DBTable dbTable = dbTables.Get(tableName);

                if (dbTable == null)
                {
                    dbTable = new DBTable();

                    if (reader["Id"] != null)
                    {
                        dbTable.Id = ValueTypeHelper.Int32TryParse(reader["Id"].ToString(), dbTable.Id);
                    }

                    if (reader["Type"] != null)
                    {
                        Int32 dynamicTableType = 0;
                        dynamicTableType = ValueTypeHelper.Int32TryParse(reader["Type"].ToString(), dynamicTableType);
                        dbTable.DynamicTableType = (DynamicTableType)dynamicTableType;
                    }

                    dbTable.Name = tableName;
                    dbTables.Add(dbTable);
                }

                #region Read Column Properties

                DBColumn dbColumn = new DBColumn();
                String columnName = String.Empty;

                if (reader["ColumnName"] != null)
                   columnName= reader["ColumnName"].ToString();

                //If internal column should not be included and the current column is an internal column, then do not add it to the collection.
                if (!loadInternalColumns && internalColumnNames.Contains(columnName.ToLowerInvariant()))
                {
                    continue;
                }
               
                dbColumn.Name = columnName;
               
                if (reader["Sequence"] != null)
                    dbColumn.Sequence = ValueTypeHelper.Int32TryParse(reader["Sequence"].ToString(), dbColumn.Sequence);

                if (reader["DataType"] != null)
                    dbColumn.DataType = GetDataType(reader["DataType"].ToString());

                if (reader["Width"] != null)
                    dbColumn.Length = ValueTypeHelper.Int32TryParse(reader["Width"].ToString(), dbColumn.Length);

                if (reader["Precision"] != null)
                    dbColumn.Precision = ValueTypeHelper.Int32TryParse(reader["Precision"].ToString(), dbColumn.Precision);

                if (reader["Nullable"] != null)
                    dbColumn.Nullable = ValueTypeHelper.BooleanTryParse(reader["Nullable"].ToString(), dbColumn.Nullable);

                if (reader["IsUnique"] != null)
                    dbColumn.IsUnique = ValueTypeHelper.BooleanTryParse(reader["IsUnique"].ToString(), dbColumn.IsUnique);

                if (reader["DefaultValue"] != null)
                    dbColumn.DefaultValue = reader["DefaultValue"].ToString();

                if (reader["CheckConstraint"] != null)
                    checkConstraintValue = reader["CheckConstraint"].ToString();

                dbTable.Columns.Add(dbColumn);

                #endregion

                #region Read Constraint Properties

                DBConstraint dbConstraint = null;

                if (!String.IsNullOrWhiteSpace(dbColumn.DefaultValue))
                {
                    dbConstraint = new DBConstraint();

                    dbConstraint.ColumnName = dbColumn.Name;
                    dbConstraint.ConstraintType = ConstraintType.DefaultValue;
                    dbConstraint.Value = dbColumn.DefaultValue;
                    dbConstraint.Action = ObjectAction.Read;

                    dbTable.Constraints.Add(dbConstraint);
                }

                if (!String.IsNullOrWhiteSpace(checkConstraintValue))
                {
                    dbConstraint = new DBConstraint();

                    dbConstraint.ColumnName = dbColumn.Name;
                    dbConstraint.ConstraintType = ConstraintType.Check;
                    dbConstraint.Value = checkConstraintValue;
                    dbConstraint.Action = ObjectAction.Read;

                    dbTable.Constraints.Add(dbConstraint);
                }

                if (dbColumn.IsUnique)
                {
                    dbConstraint = new DBConstraint();

                    dbConstraint.ColumnName = dbColumn.Name;
                    dbConstraint.ConstraintType = ConstraintType.Unique;
                    dbConstraint.Value = "True";
                    dbConstraint.Action = ObjectAction.Read;

                    dbTable.Constraints.Add(dbConstraint);
                }

                #endregion
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="dbColumns"></param>
        private void PopulateDefaultTemplate(SqlDataReader reader, DBColumnCollection dbColumns)
        {
            while (reader.Read())
            {
                DBColumn dbColumn = new DBColumn();

                if (reader["ID"] != null)
                {
                    dbColumn.Id = ValueTypeHelper.Int32TryParse(reader["ID"].ToString(), dbColumn.Id);
                }

                if (reader["ColumnName"] != null)
                {
                    dbColumn.Name = reader["ColumnName"].ToString();
                    dbColumn.LongName = dbColumn.Name;
                }

                if (reader["DataType"] != null)
                {
                    dbColumn.DataType = GetDataType(reader["DataType"].ToString());
                }

                if (reader["Width"] != null)
                {
                    dbColumn.Length = ValueTypeHelper.Int32TryParse(reader["Width"].ToString(), dbColumn.Length);
                }

                if (reader["IsRequired"] != null)
                {
                    dbColumn.Nullable = ValueTypeHelper.BooleanTryParse(reader["IsRequired"].ToString(), dbColumn.Nullable);
                }

                if (reader["IsUnique"] != null)
                {
                    dbColumn.IsUnique = ValueTypeHelper.BooleanTryParse(reader["IsUnique"].ToString(), dbColumn.IsUnique);
                }

                dbColumns.Add(dbColumn);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataTypeName"></param>
        /// <returns></returns>
        private String GetDataType(String dataTypeName)
        {
            switch (dataTypeName.ToLower())
            {
                case "nvarchar":
                case "varchar":
                    return AttributeDataType.String.ToString();
                case "smalldatetime":
                    return AttributeDataType.DateTime.ToString();
                case "int":
                    return AttributeDataType.Integer.ToString();
                case "bit":
                    return AttributeDataType.Boolean.ToString();
                default:
                    return String.Empty;
            }
        }

        #endregion

        #endregion
    }
}