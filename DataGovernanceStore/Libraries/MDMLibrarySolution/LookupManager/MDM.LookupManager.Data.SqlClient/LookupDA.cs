using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Data;
using Microsoft.SqlServer.Server;
using System.Transactions;

namespace MDM.LookupManager.Data
{
    using MDM.Core;
    using MDM.BusinessObjects;
    using MDM.Utility;

    /// <summary>
    /// Specifies data access operations for Lookup
    /// </summary>
    public class LookupDA : SqlClientDataAccessBase
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
        /// Gets lookup data for the requested lookup table and locales
        /// </summary>
        /// <param name="lookupTableName">Name of the lookup table</param>
        /// <param name="localeDictionary">Locales for which lookup data needs to be fetched</param>
        /// <param name="fromPK">Id of the record from which fetching should start</param>
        /// <param name="toPK">Id of the record till which fetching needs to be done</param>
        /// <param name="command">>Object having command properties</param>
        /// <returns>Lookup data for all the requested locales</returns>
        public LookupCollection Get(String lookupTableName, Dictionary<LocaleEnum, Boolean> localeDictionary, Int64 fromPK, Int64 toPK, DBCommandProperties command)
        {
            LookupCollection lookupCollection = null;
            DataTable dtModel = null;
            DataSet dsData = null;

            SqlDataReader reader = null;
            SqlParameter[] parameters;
            String storedProcedureName = String.Empty;
            StringBuilder xml = new StringBuilder();

            try
            {
                SqlParametersGenerator generator = new SqlParametersGenerator("LookupManager_SqlParameters");

                parameters = generator.GetParameters("LookupManager_Lookup_Get_ParametersArray");

                parameters[0].Value = lookupTableName;

                #region Populate table value parameters

                List<SqlDataRecord> localeList = new List<SqlDataRecord>();

                SqlMetaData[] localeMetadata = generator.GetTableValueMetadata("LookupManager_Lookup_Get_ParametersArray", parameters[1].ParameterName);

                foreach (KeyValuePair<LocaleEnum, Boolean> locale in localeDictionary)
                {
                    SqlDataRecord localeRecord = new SqlDataRecord(localeMetadata);

                    localeRecord.SetValues((Int32)locale.Key, locale.Key.ToString(), locale.Value);

                    localeList.Add(localeRecord);
                }

                #endregion

                parameters[1].Value = localeList;
                parameters[2].Value = fromPK;
                parameters[3].Value = toPK;

                storedProcedureName = "usp_LookupManager_Lookup_Get";

                reader = ExecuteProcedureReader(command.ConnectionString, parameters, storedProcedureName);

                if (reader != null)
                {
                    //First read the lookup model's table in dtModel. The get the name of tables from 1st column(FK_Attribute)
                    //give array of table names to load all tables in dsData. 
                    dtModel = new DataTable();
                    dsData = new DataSet();

                    dtModel.Load(reader);
                    if (dtModel != null && dtModel.Rows != null && dtModel.Rows.Count > 0
                        && dtModel.Columns != null && dtModel.Columns.Contains("TableName"))
                    {
                        String tableNames = String.Empty;
                        foreach (DataRow row in dtModel.Rows)
                        {
                            tableNames = String.Concat(tableNames, row["TableName"].ToString(), ",");
                        }

                        if (tableNames.Length > 0)
                        {
                            tableNames = tableNames.Remove(tableNames.Length - 1);
                        }

                        dtModel.TableName = "TableModel";
                        dsData.Tables.Add(dtModel);
                        dsData.Load(reader, LoadOption.Upsert, tableNames.Split(','));
                    }
                }

                lookupCollection = new LookupCollection(dsData);
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }

            return lookupCollection;
        }

        /// <summary>
        /// Process Multiple Lookup Data
        /// </summary>
        /// <param name="lookupCollection">Collection of Lookup Data to be processed</param>
        /// <param name="userLogin">Login User Name</param>
        /// <param name="systemDataLocale">System Data Locale</param>
        /// <param name="programName">program name which needs lookups to be processed</param>
        /// <param name="command">command having connection properties</param>
        /// <param name="operationResult">Indicates the operationResult</param>
        public void Process(LookupCollection lookupCollection, String userLogin, LocaleEnum systemDataLocale, String programName, DBCommandProperties command, OperationResult operationResult)
        {
            using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required))
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StartTraceActivity("LookupDA.Lookup.Process", false);

                List<SqlDataRecord> lookupTable;
                SqlDataReader reader = null;
                SqlParameter[] parameters;
                String storedProcedureName = String.Empty;
                SqlParametersGenerator generator = new SqlParametersGenerator("LookupManager_SqlParameters");
                parameters = generator.GetParameters("LookupManager_Lookup_Process_ParametersArray");
                SqlMetaData[] lookupMetaData = generator.GetTableValueMetadata("LookupManager_Lookup_Process_ParametersArray", parameters[0].ParameterName);
                lookupTable = CreateLookupTable(lookupCollection, lookupMetaData);

                if (lookupTable != null)
                {
                    try
                    {
                   
                        parameters[0].Value = lookupTable;
                        parameters[1].Value = (Int32)systemDataLocale;
                        parameters[2].Value = userLogin;
                        parameters[3].Value = programName;

                        storedProcedureName = "usp_LookupManager_Lookup_Process";

                        reader = ExecuteProcedureReader(command.ConnectionString, parameters, storedProcedureName);
                        //Read the parameter and set row id return by SP - i.e- which row to process
                        PopulateLookups(lookupCollection, reader, operationResult);
                     
                    }
                    
                    finally
                    {
                        if (reader != null)
                            reader.Close();
                    }

                }

                transactionScope.Complete();

                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StopTraceActivity("LookupDA.Lookup.Process");
            }
        }
        
        /// <summary>
        /// Gets All Related Lookup Table Names for Current Lookup Table
        /// </summary>
        /// <param name="lookupTableName">Current Lookup Table Name</param>
        /// <param name="command">command having connection properties</param>
        /// <returns>Collection of Lookup Table Name</returns>
        public Collection<String> GetRelatedLookups(String lookupTableName, DBCommandProperties command)
        {
            Collection<String> referrerTables = new Collection<String>();


            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StartTraceActivity("LookupDA.Lookup.GetRelatedLookups", false);

            DataTable dtModel = null;

            SqlDataReader reader = null;
            SqlParameter[] parameters;
            String storedProcedureName = String.Empty;
            String tablename = lookupTableName;

            try
            {
                SqlParametersGenerator generator = new SqlParametersGenerator("LookupManager_SqlParameters");

                parameters = generator.GetParameters("LookupManager_Lookup_Get_Referrers");

                parameters[0].Value = tablename;

                storedProcedureName = "usp_LookupManager_Lookup_Reference_Get";

                reader = ExecuteProcedureReader(command.ConnectionString, parameters, storedProcedureName);

                if (reader != null)
                {

                    dtModel = new DataTable();

                    dtModel.Load(reader);
                    if (dtModel != null && dtModel.Rows != null && dtModel.Rows.Count > 0
                        && dtModel.Columns != null && dtModel.Columns.Contains("ShortName"))
                    {
                        foreach (DataRow row in dtModel.Rows)
                        {
                            referrerTables.Add(row["ShortName"].ToString());
                        }

                    }
                }
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }



            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StopTraceActivity("LookupDA.Lookup.GetRelatedLookups");


            return referrerTables;
        }

        /// <summary>
        /// Get all lookup models based on dynamic table type
        /// </summary>
        /// <param name="dynamicTableType">Indicates dynamic table type.</param>
        /// <param name="filteredLookupTableNames">Indicates list of lookup table name to be filtered.</param>
        /// <param name="callerContext">Indicates the name of application and the module that are performing the action.</param>
        /// <returns>collection of lookup models.</returns>
        public LookupModelCollection GetLookupModels(DynamicTableType dynamicTableType, Collection<String> filteredLookupTableNames, DBCommandProperties command)
        {
            LookupModelCollection lookupModels = new LookupModelCollection();
            SqlDataReader reader = null;

            try
            {
                
                SqlParametersGenerator generator = new SqlParametersGenerator("LookupManager_SqlParameters");

                SqlParameter[] parameters = generator.GetParameters("LookupManager_Lookup_GetModels_ParametersArray");

                #region Populate table value parameters

                List<SqlDataRecord> lookupTableNames = null;

                if (filteredLookupTableNames != null && filteredLookupTableNames.Count > 0)
                {
                    SqlMetaData[] localeMetadata = generator.GetTableValueMetadata("LookupManager_Lookup_GetModels_ParametersArray", parameters[5].ParameterName);
                    lookupTableNames = new List<SqlDataRecord>();

                    foreach (String lookupTableName in filteredLookupTableNames)
                    {
                        SqlDataRecord localeRecord = new SqlDataRecord(localeMetadata);
                        localeRecord.SetValue(0, lookupTableName);
                        lookupTableNames.Add(localeRecord);
                    }
                }

                #endregion

                parameters[0].Value = (Int32)dynamicTableType;
                parameters[1].Value = String.Empty; // Setting this parameters as String.Empty since this parameters performs like search.
                parameters[2].Value = false; // This parameters decides to load system table or not. Since we are interested only lookup hence setting as false.
                parameters[3].Value = false; //This parameters decides to load lookup table name with attribute count. Example : if color lookup table name is mapped to 2 attributes then result would be color2.
                parameters[4].Value = false; //This parameters decides to get only lookup table with unique column.
                parameters[5].Value = lookupTableNames;

                const String storedProcedureName = "usp_Lookup_GetTableNames";

                reader = ExecuteProcedureReader(command.ConnectionString, parameters, storedProcedureName);

                if (reader != null)
                {
                    while (reader.Read())
                    {
                        LookupModel lookupModel = new LookupModel();

                        if (reader["Id"] != null)
                        {
                            lookupModel.Id = ValueTypeHelper.Int32TryParse(reader["Id"].ToString(), 0);
                        }

                        if (reader["TableName"] != null)
                        {
                            lookupModel.TableName = reader["TableName"].ToString();
                        }

                        if (reader["DisplayTableName"] != null)
                        {
                            lookupModel.DisplayTableName = reader["DisplayTableName"].ToString();
                        }

                        if (reader["IsView"] != null)
                        {
                            lookupModel.IsViewBasedLookup = ValueTypeHelper.BooleanTryParse(reader["IsView"].ToString(), lookupModel.IsViewBasedLookup);
                        }

                        lookupModels.Add(lookupModel);
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

            return lookupModels;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Creates lookup table from lookup collection
        /// </summary>
        /// <param name="lookupCollection"></param>
        /// <param name="metadata"></param>
        /// <param name="lookupTable"></param>
        private List<SqlDataRecord> CreateLookupTable(LookupCollection lookupCollection, SqlMetaData[] metadata)
        {
            List<SqlDataRecord> lookupTable = new List<SqlDataRecord>();
            foreach (Lookup lookup in lookupCollection)
            {
                Int32 localeId = (Int32)lookup.Locale;

                foreach (Row lookupRow in lookup.Rows)
                {
                    Int32 lookupRowId = Int32.Parse(lookupRow.Id.ToString());
                    foreach (Cell lookupColumn in lookupRow.Cells)
                    {
                        if (lookupColumn.ColumnName.ToLowerInvariant() != "id") //Id column no need to send to db.
                        {
                            String lookupColumnValue = ( lookupColumn.Value != null ? lookupColumn.Value.ToString() : String.Empty );
                            SqlDataRecord lookupRecord = new SqlDataRecord(metadata);
                            lookupRecord.SetValue(0, lookup.Id);
                            lookupRecord.SetValue(1, lookup.Name);
                            lookupRecord.SetValue(2, lookupRowId);
                            lookupRecord.SetValue(3, lookupColumn.ColumnName);
                            lookupRecord.SetValue(4, lookupColumnValue);
                            lookupRecord.SetValue(5, localeId);
                            lookupRecord.SetValue(6, lookupRow.Action.ToString());

                            lookupTable.Add(lookupRecord);
                        }
                    }
                }

            }

            //we cannot pass no records in the SqlDataRecord enumeration. To send a table-valued parameter with no rows, use a null reference for the value instead.
            if (lookupTable.Count == 0)
                lookupTable = null;

            return lookupTable;
        }

        private void PopulateLookups(LookupCollection lookups, SqlDataReader reader, OperationResult operationResult)
        {
            while (reader.Read())
            {
                String resultCode = String.Empty;
                Boolean hasError = false;
                Collection<Object> param = new Collection<Object>();
                String lookupTableName = String.Empty;
                LocaleEnum locale = LocaleEnum.UnKnown;
                Int64 rowId = 0;
                Int64 id = 0;

                if (reader["LookupId"] != null)
                {
                    param.Add(reader["LookupId"].ToString());
                }
                if (reader["LookupTableName"] != null)
                {
                    lookupTableName = reader["LookupTableName"].ToString();
                    param.Add(lookupTableName);
                }

                if (reader["Locale"] != null)
                {
                    param.Add(reader["Locale"].ToString());
                    Enum.TryParse<LocaleEnum>(reader["Locale"].ToString(), out locale);
                }

                if (reader["ErrorCode"] != null)
                {
                    resultCode = reader["ErrorCode"].ToString();
                }

                if (reader["Id"] != null)
                {
                    id = ValueTypeHelper.Int64TryParse(reader["Id"].ToString(), id);
                }

                if (reader["LookupId"] != null)
                {
                    rowId = ValueTypeHelper.Int64TryParse(reader["LookupId"].ToString(), rowId);
                }

                if (reader["HasError"] != null && !String.IsNullOrEmpty(reader["HasError"].ToString()))
                {
                    hasError = ValueTypeHelper.ConvertToBoolean(reader["HasError"].ToString());
                }

                var lookup = lookups.GetLookup(lookupTableName.Replace("tblk_", String.Empty), locale);

                foreach (Row row in lookup.Rows)
                {
                    //Compare rowId and id returns by SP
                    if (id < 0)
                    {
                        if (row.Id == id)
                        {
                            row.Id = rowId;
                        }
                    }
                }

                if (hasError)
                {
                    operationResult.AddOperationResult(resultCode, param, OperationResultType.Error);
                }
            }
        }

        #endregion

        #endregion
    }
}
