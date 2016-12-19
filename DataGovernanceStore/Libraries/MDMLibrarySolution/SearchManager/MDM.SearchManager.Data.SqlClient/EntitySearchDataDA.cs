using System;
using System.Data.SqlClient;
using System.Collections.ObjectModel;
using System.Transactions;
using System.Collections.Generic;
using Microsoft.SqlServer.Server;

namespace MDM.SearchManager.Data
{
    using MDM.Core;
    using MDM.Utility;
    using MDM.BusinessObjects;
    using MDM.BusinessObjects.Diagnostics;

    /// <summary>
    /// Specifies the data access operations for search data
    /// </summary>
    public class EntitySearchDataDA : SqlClientDataAccessBase
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
        /// Get EntitySearchData for given entities.
        /// </summary>
        /// <param name="entityIdList">Provides Collection of entity ids for which searchData needs to be found.</param>
        /// <param name="command">Indicates SQL Command</param>
        /// <returns>EntitySearchData Collection</returns>
        public EntitySearchDataCollection Get(Collection<Int64> entityIdList, DBCommandProperties command)
        {
            SqlDataReader reader = null;
            EntitySearchDataCollection entitySearchDataCollection = null;

            #region Diagnostics & Tracing

            DiagnosticActivity diagnosticActivity = new DiagnosticActivity();
            var currentTraceSettings = MDMOperationContextHelper.GetCurrentTraceSettings();
            Boolean isTracingEnabled = currentTraceSettings.IsBasicTracingEnabled;

            #endregion Diagnostics & Tracing

            try
            {
                if (isTracingEnabled)
                {
                    diagnosticActivity.Start();
                }

                SqlParametersGenerator generator = new SqlParametersGenerator("SearchManager_SqlParameters");

                SqlParameter[] parameters = generator.GetParameters("SearchManager_EntitySearchData_Get_ParametersArray");

                #region Populate table value parameters

                SqlMetaData[] entityMetadata = generator.GetTableValueMetadata("SearchManager_EntitySearchData_Get_ParametersArray", parameters[0].ParameterName);

                List<SqlDataRecord> entityIds = EntityDataReaderUtility.CreateEntityIdTable(entityIdList, entityMetadata);

                #endregion

                parameters[0].Value = entityIds;

                String storedProcedureName = "usp_SearchManager_EntitySearchData_Get";

                reader = ExecuteProcedureReader(command.ConnectionString, parameters, storedProcedureName);

                if (isTracingEnabled)
                {
                    diagnosticActivity.LogDurationInfo(String.Format("Execution for procedure : {0} is completed", storedProcedureName));
                }

                entitySearchDataCollection = ReadEntitySearchData(reader);

                if (isTracingEnabled)
                {
                    diagnosticActivity.LogDurationInfo("Entity search data reading is completed.");
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

            return entitySearchDataCollection;
        }

        /// <summary>
        /// Process Entity Search Data Collection.
        /// </summary>
        /// <param name="entitySearchDataCollection">Provides SerachData to be processed.</param>
        /// <param name="entityOperationResults">Indicates entityOperationResult.</param>
        /// <param name="command">Indicates SQL Command</param>
        /// <param name="processingMode">Mode of processing (Sync or Async)</param>
        /// <returns>True if serachData have been processed successfully.</returns>
        public Boolean Process(EntitySearchDataCollection entitySearchDataCollection, EntityOperationResultCollection entityOperationResults, DBCommandProperties command, ProcessingMode processingMode = ProcessingMode.Sync)
        {
            SqlDataReader reader = null;
            Boolean result = false;

            #region Diagnostics & Tracing

            DiagnosticActivity diagnosticActivity = new DiagnosticActivity();
            var currentTraceSettings = MDMOperationContextHelper.GetCurrentTraceSettings();
            Boolean isTracingEnabled = currentTraceSettings.IsBasicTracingEnabled;

            #endregion Diagnostics & Tracing

            try
            {
                if (isTracingEnabled)
                {
                    diagnosticActivity.Start();
                }

                using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required, BusinessLogicBase.GetTransactionOptions(processingMode)))
                {
                    SqlParametersGenerator generator = new SqlParametersGenerator("SearchManager_SqlParameters");

                    SqlParameter[] parameters = generator.GetParameters("SearchManager_EntitySearchData_Process_ParametersArray");

                    #region Populate table value parameters

                    List<SqlDataRecord> entitySearchDataList = new List<SqlDataRecord>();
                    SqlMetaData[] entitySearchMetadata = generator.GetTableValueMetadata("SearchManager_EntitySearchData_Process_ParametersArray", parameters[0].ParameterName);
                    String storedProcedureName = "usp_SearchManager_EntitySearchData_Process";
                    SqlDataRecord entitySearchDataRecord = null;

                    foreach (EntitySearchData entitySearchData in entitySearchDataCollection)
                    {
                        entitySearchDataRecord = new SqlDataRecord(entitySearchMetadata);
                        entitySearchDataRecord.SetValue(0, entitySearchData.EntityId);
                        entitySearchDataRecord.SetValue(1, entitySearchData.ContainerId);
                        entitySearchDataRecord.SetValue(2, entitySearchData.SearchValue);
                        entitySearchDataRecord.SetValue(3, entitySearchData.KeyValue);
                        entitySearchDataRecord.SetValue(4, entitySearchData.Action.ToString());
                        entitySearchDataList.Add(entitySearchDataRecord);
                    }

                    if (isTracingEnabled)
                    {
                        diagnosticActivity.LogInformation("Preparing TVP from entity search data object is completed.");
                        DiagnosticsUtility.LogSqlTVPInformation(diagnosticActivity, storedProcedureName, "EntitySearchDataTable", entitySearchMetadata, entitySearchDataList);
                    }

                    #endregion

                    parameters[0].Value = entitySearchDataList;

                    reader = ExecuteProcedureReader(command.ConnectionString, parameters, storedProcedureName);

                    if (isTracingEnabled)
                    {
                        diagnosticActivity.LogDurationInfo(String.Format("Execution for procedure : {0} is completed", storedProcedureName));
                    }

                    result = UpdateEntityOperationResults(reader, entityOperationResults);

                    transactionScope.Complete();
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

            return result;
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        private EntitySearchDataCollection ReadEntitySearchData(SqlDataReader reader)
        {
            EntitySearchDataCollection entitySearchDataCollection = new EntitySearchDataCollection();

            if (reader != null)
            {
                while (reader.Read())
                {
                    EntitySearchData entitySearchData = new EntitySearchData();

                    if (reader["PK_DN_Search"] != null)
                    {
                        entitySearchData.Id = ValueTypeHelper.Int64TryParse(reader["PK_DN_Search"].ToString(), 0);
                    }

                    if (reader["FK_CNode"] != null)
                    {
                        entitySearchData.EntityId = ValueTypeHelper.Int64TryParse(reader["FK_CNode"].ToString(), 0);
                    }

                    if (reader["FK_Catalog"] != null)
                    {
                        entitySearchData.ContainerId = ValueTypeHelper.Int32TryParse(reader["FK_Catalog"].ToString(), 0);
                    }

                    if (reader["SearchVal"] != null)
                    {
                        entitySearchData.SearchValue = reader["SearchVal"].ToString();
                    }

                    if (reader["KeyVal"] != null)
                    {
                        entitySearchData.KeyValue = reader["KeyVal"].ToString();
                    }

                    if (reader["IDPath"] != null)
                    {
                        entitySearchData.IdPath = reader["IDPath"].ToString();
                    }

                    entitySearchDataCollection.Add(entitySearchData);
                }
            }

            return entitySearchDataCollection;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="entityOperationResults"></param>
        private Boolean UpdateEntityOperationResults(SqlDataReader reader, EntityOperationResultCollection entityOperationResults)
        {
            Boolean result = false;

            if (reader != null)
            {
                while (reader.Read())
                {
                    if (reader["EntityId"] != null)
                    {
                        if (reader["MessageCode"] != null && reader["IsError"] != null)
                        {
                            if (reader["IsError"].ToString() == "1")
                            {
                                result = false;
                                entityOperationResults.AddEntityOperationResult(ValueTypeHelper.Int64TryParse(reader["EntityId"].ToString(), 0), reader["MessageCode"].ToString(), "", OperationResultType.Error);
                            }
                            else
                            {
                                result = true;
                                entityOperationResults.AddEntityOperationResult(ValueTypeHelper.Int64TryParse(reader["EntityId"].ToString(), 0), reader["MessageCode"].ToString(), "", OperationResultType.Information);
                            }
                        }
                    }
                }
            }

            return result;
        }

        #endregion Private Methods

        #endregion
    }
}