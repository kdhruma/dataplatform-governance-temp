using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Transactions;
using Microsoft.SqlServer.Server;

namespace MDM.IntegrationManager.Data
{
    using MDM.BusinessObjects;
    using MDM.BusinessObjects.Integration;
    using MDM.Core;
    using MDM.Utility;

    /// <summary>
    /// Specifies integration error log type data access
    /// </summary>
    public class IntegrationErrorLogDA : SqlClientDataAccessBase
    {
        #region Fields

        private const String _processMethodName = "MDM.IntegrationManager.Data.IntegrationErrorLogDA.Process";

        #endregion Fields

        #region Public methods

        /// <summary>
        /// Process integration error log
        /// </summary>
        /// <param name="integrationErrorLog">IntegrationErrorLog to process</param>
        /// <param name="programName">Name of program making the change</param>
        /// <param name="userName">Name of user making the change</param>
        /// <param name="command">Connection related properties</param>
        /// <returns>Operation result of processing of integration error log</returns>
        public void Process(IntegrationErrorLogCollection integrationErrorLogCollection, String programName, String userName, OperationResultCollection operationResultCollection, DBCommandProperties command)
        {
            if (Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.StartTraceActivity(_processMethodName, MDMTraceSource.Integration, false);
            }

            SqlDataReader reader = null;
            SqlParametersGenerator generator = new SqlParametersGenerator("IntegrationManager_SqlParameters");

            const String storedProcedureName = "usp_IntegrationManager_ErrorLog_Process";
            SqlParameter[] parameters = generator.GetParameters("IntegrationManager_IntegrationErrorLog_Process_ParametersArray");
            SqlMetaData[] sqlMetadata = generator.GetTableValueMetadata("IntegrationManager_IntegrationErrorLog_Process_ParametersArray", parameters[0].ParameterName);

            List<SqlDataRecord> integrationErrorLogRecords = GetSqlRecords(integrationErrorLogCollection, sqlMetadata);

            using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions() { IsolationLevel = Constants.DEFAULT_ISOLATION_LEVEL }))
            {
                reader = null;
                try
                {

                    parameters[0].Value = integrationErrorLogRecords;
                    parameters[1].Value = userName;
                    parameters[2].Value = programName;

                    reader = ExecuteProcedureReader(command.ConnectionString, parameters, storedProcedureName);

                    UpdateIntegrationErrorLogs(reader, operationResultCollection);
                    transactionScope.Complete();
                }
                finally
                {
                    if (reader != null)
                        reader.Close();

                    if (Constants.TRACING_ENABLED)
                    {
                        MDMTraceHelper.StopTraceActivity(_processMethodName, MDMTraceSource.Integration);
                    }
                }
            }
        }

        #endregion Public methods

        #region Private method

        private List<SqlDataRecord> GetSqlRecords(IntegrationErrorLogCollection integrationErrorLogCollection, SqlMetaData[] sqlMetadata)
        {
            List<SqlDataRecord> integrationErrorLogSqlRecords = new List<SqlDataRecord>();
            foreach (IntegrationErrorLog integrationErrorLog in integrationErrorLogCollection)
            {
                SqlDataRecord integrationErrorLogRecord = new SqlDataRecord(sqlMetadata);
                integrationErrorLogRecord.SetValue(0, integrationErrorLog.IntegrationId);
                integrationErrorLogRecord.SetValue(1, (Int16)integrationErrorLog.IntegrationType);
                integrationErrorLogRecord.SetValue(2, integrationErrorLog.IntegrationMessageTypeId);
                integrationErrorLogRecord.SetValue(3, integrationErrorLog.ConnectorId);
                integrationErrorLogRecord.SetValue(4, integrationErrorLog.MessageText);
                integrationErrorLogRecord.SetValue(5, (Int16) integrationErrorLog.CoreDataProcessorName);

                integrationErrorLogSqlRecords.Add(integrationErrorLogRecord);
            }

            return integrationErrorLogSqlRecords;
        }

        private void UpdateIntegrationErrorLogs(SqlDataReader reader, OperationResultCollection operationResultCollection)
        {
            while (reader.Read())
            {
                Boolean hasError = false;
                String errorMessage = String.Empty;

                #region Read values


                if (reader["HasError"] != null)
                {
                    hasError = ValueTypeHelper.BooleanTryParse(reader["HasError"].ToString(), false);
                }

                if (reader["ErrorMessage"] != null)
                {
                    errorMessage = reader["ErrorMessage"].ToString();
                }


                #endregion Read values

                if (hasError)
                {
                    foreach (OperationResult operationResult in operationResultCollection)
                    {
                        operationResult.AddOperationResult(String.Empty, errorMessage, OperationResultType.Error);
                    }
                }
            }
        }
        #endregion Private method
    }
}
