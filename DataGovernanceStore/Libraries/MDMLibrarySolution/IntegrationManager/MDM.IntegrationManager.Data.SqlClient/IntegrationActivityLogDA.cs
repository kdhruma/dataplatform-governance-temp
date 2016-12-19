using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Transactions;

namespace MDM.IntegrationManager.Data
{
    using MDM.BusinessObjects;
    using MDM.BusinessObjects.Integration;
    using MDM.Core;
    using MDM.Utility;

    /// <summary>
    /// Specifies integration activity log type data access
    /// </summary>
    public class IntegrationActivityLogDA : SqlClientDataAccessBase
    {
        #region Fields

        private const String _processMethodName = "MDM.IntegrationManager.Data.IntegrationActivityLogDA.Process";
        private const String _getMethodName = "MDM.IntegrationManager.Data.IntegrationActivityLogDA.Get";

        #endregion Fields

        #region Public methods

        /// <summary>
        /// Process integration activity log
        /// </summary>
        /// <param name="integrationActivityLogCollection">IntegrationActivityLogs to process</param>
        /// <param name="programName">Name of program making the change</param>
        /// <param name="userName">Name of user making the change</param>
        /// <param name="command">Connection related properties</param>
        /// <returns>Operation result of processing of integration activity log</returns>
        public void Process(IntegrationActivityLogCollection integrationActivityLogCollection, String programName, String userName,  Int16 serverId, OperationResultCollection operationResultCollection, DBCommandProperties command)
        {
            if (Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.StartTraceActivity(_processMethodName, MDMTraceSource.Integration, false);
            }

            SqlDataReader reader = null;
            SqlParametersGenerator generator = new SqlParametersGenerator("IntegrationManager_SqlParameters");

            const String storedProcedureName = "usp_IntegrationManager_IntegrationActivityLog_Process";
            SqlParameter[] parameters = generator.GetParameters("IntegrationManager_IntegrationActivityLog_Process_ParametersArray");

            SqlMetaData[] sqlMetadata = generator.GetTableValueMetadata("IntegrationManager_IntegrationActivityLog_Process_ParametersArray", parameters[0].ParameterName);
            List<SqlDataRecord> integrationActivityLogRecords = GetSqlRecords(integrationActivityLogCollection, sqlMetadata);

            using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions() { IsolationLevel = Constants.DEFAULT_ISOLATION_LEVEL }))
            {
                reader = null;
                try
                {
                    parameters[0].Value = integrationActivityLogRecords;
                    parameters[1].Value = programName;
                    parameters[2].Value = userName;
                    parameters[3].Value = serverId;

                    reader = ExecuteProcedureReader(command.ConnectionString, parameters, storedProcedureName);

                    //Update integration activity log object with Actual Id in case of create
                    UpdateIntegrationActivityLogs(reader, integrationActivityLogCollection, operationResultCollection);

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

        /// <summary>
        /// Get integration activity log for a given connector and range of Ids
        /// </summary>
        /// <param name="logStatus">Integration activity log status based on which data is to be  fetched</param>
        /// <param name="fromCount">Start of range of values to be fetched</param>
        /// <param name="toCount">End of range of values to be fetched</param>
        /// <param name="callerContext">Indicates the context of caller making call to this API</param>        
        public IntegrationActivityLogCollection Get(String logStatus, Int64 fromCount, Int64 toCount, String programName, String userName, Int16 serverId, DBCommandProperties command)
        {
            if (Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.StartTraceActivity(_getMethodName, MDMTraceSource.Integration, false);
            }

            SqlDataReader reader = null;
            SqlParameter[] parameters;
            IntegrationActivityLogCollection integrationActivityLog = new IntegrationActivityLogCollection();
            String storedProcedureName = "usp_IntegrationManager_IntegrationActivityLog_Get";

            SqlParametersGenerator generator = new SqlParametersGenerator("IntegrationManager_SqlParameters");
            parameters = generator.GetParameters("IntegrationManager_IntegrationActivityLog_Get_ParametersArray");

            using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions() { IsolationLevel = Constants.DEFAULT_ISOLATION_LEVEL }))
            {
                try
                {
                    parameters[0].Value = logStatus;
                    parameters[1].Value = fromCount;
                    parameters[2].Value = toCount;
                    parameters[3].Value = programName;
                    parameters[4].Value = userName;
                    parameters[5].Value = serverId;

                    reader = ExecuteProcedureReader(command.ConnectionString, parameters, storedProcedureName);

                    integrationActivityLog = ReadIntegrationActivityLog(reader);
                    transactionScope.Complete();
                }
                finally
                {
                    if (reader != null)
                        reader.Close();

                    if (Constants.TRACING_ENABLED)
                    {
                        MDMTraceHelper.StopTraceActivity(_getMethodName, MDMTraceSource.Integration);
                    }
                }
            }
            return integrationActivityLog;
        }

        /// <summary>
        /// Get integration activity log for a given connector and range of Ids
        /// </summary>
        /// <param name="connectorId">Connector Id for which integratoin items will be filtered against.</param>
        /// <param name="logStatus">Integration activity log status based on which data is to be  fetched</param>
        /// <param name="fromCount">Start of range of values to be fetched</param>
        /// <param name="toCount">End of range of values to be fetched</param>
        /// <param name="callerContext">Indicates the context of caller making call to this API</param>
        /// <returns>Integration activity log object</returns>
        public IntegrationActivityLogCollection GetByConnectorId(Int32 connectorId, String logStatus, Int64 fromCount, Int64 toCount, String programName, String userName, Int16 serverId, DBCommandProperties command)
        {
            if (Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.StartTraceActivity(_getMethodName, MDMTraceSource.Integration, false);
            }

            SqlDataReader reader = null;
            SqlParameter[] parameters;
            IntegrationActivityLogCollection integrationActivityLog = new IntegrationActivityLogCollection();
            String storedProcedureName = "usp_IntegrationManager_IntegrationActivityLog_GetByConnector";

            SqlParametersGenerator generator = new SqlParametersGenerator("IntegrationManager_SqlParameters");
            parameters = generator.GetParameters("IntegrationManager_IntegrationActivityLog_GetByConnector_ParametersArray");

            using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions() { IsolationLevel = Constants.DEFAULT_ISOLATION_LEVEL }))
            {
                try
                {
                    parameters[0].Value = connectorId;
                    parameters[1].Value = logStatus;
                    parameters[2].Value = fromCount;
                    parameters[3].Value = toCount;
                    parameters[4].Value = programName;
                    parameters[5].Value = userName;
                    parameters[6].Value = serverId;

                    reader = ExecuteProcedureReader(command.ConnectionString, parameters, storedProcedureName);

                    integrationActivityLog = ReadIntegrationActivityLog(reader);
                    transactionScope.Complete();
                }
                finally
                {
                    if (reader != null)
                        reader.Close();

                    if (Constants.TRACING_ENABLED)
                    {
                        MDMTraceHelper.StopTraceActivity(_getMethodName, MDMTraceSource.Integration);
                    }
                }
            }
            return integrationActivityLog;
        }

        /// <summary>
        /// Mark the activity log as loaded, and update the message count created from given integration activity log
        /// </summary>
        /// <param name="integrationActivityLogId">IntegrationActivityLogs to process</param>
        /// <param name="programName">Name of program making the change</param>
        /// <param name="messageCount">No. of messages created from given ActivityLog</param>
        /// <param name="userName">Name of user making the change</param>
        /// <param name="command">Connection related properties</param>
        /// <returns>Operation result of processing of integration activity log</returns>
        public Boolean MarkAsLoaded(Int64 integrationActivityLogId, Int32 messageCount, String programName, String userName, Int16 serverId, DBCommandProperties command)
        {
            if (Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.StartTraceActivity(_processMethodName, MDMTraceSource.Integration, false);
            }

            SqlDataReader reader = null;
            Boolean result = false;

            SqlParametersGenerator generator = new SqlParametersGenerator("IntegrationManager_SqlParameters");

            const String storedProcedureName = "usp_IntegrationManager_IntegrationActivityLog_Loaded";
            SqlParameter[] parameters = generator.GetParameters("IntegrationManager_IntegrationActivityLog_MarkAsLoaded_ParametersArray");

            using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions() { IsolationLevel = Constants.DEFAULT_ISOLATION_LEVEL }))
            {
                reader = null;
                try
                {
                    parameters[0].Value = integrationActivityLogId;
                    parameters[1].Value = messageCount;
                    parameters[2].Value = userName;
                    parameters[3].Value = programName;
                    parameters[4].Value = serverId;

                    reader = ExecuteProcedureReader(command.ConnectionString, parameters, storedProcedureName);

                    transactionScope.Complete();
                    result = true;
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

            return result;
        }

        /// <summary>
        /// Mark the activity log as processed, and update the message count created from given integration activity log
        /// </summary>
        /// <param name="integrationActivityLogId">IntegrationActivityLogs to process</param>
        /// <param name="programName">Name of program making the change</param>
        /// <param name="messageCount">No. of messages created from given ActivityLog</param>
        /// <param name="userName">Name of user making the change</param>
        /// <param name="command">Connection related properties</param>
        /// <returns>Operation result of processing of integration activity log</returns>
        public Boolean MarkAsProcessed(Int64 integrationActivityLogId, Int32 messageCount, String programName, String userName, Int16 serverId, DBCommandProperties command)
        {
            if (Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.StartTraceActivity(_processMethodName, MDMTraceSource.Integration, false);
            }

            SqlDataReader reader = null;
            Boolean result = false;

            SqlParametersGenerator generator = new SqlParametersGenerator("IntegrationManager_SqlParameters");

            const String storedProcedureName = "usp_IntegrationManager_IntegrationActivityLog_Processed";
            SqlParameter[] parameters = generator.GetParameters("IntegrationManager_IntegrationActivityLog_MarkAsProcessed_ParametersArray");

            using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions() { IsolationLevel = Constants.DEFAULT_ISOLATION_LEVEL }))
            {
                reader = null;
                try
                {
                    parameters[0].Value = integrationActivityLogId;
                    parameters[1].Value = messageCount;
                    parameters[2].Value = userName;
                    parameters[3].Value = programName;
                    parameters[4].Value = serverId;

                    reader = ExecuteProcedureReader(command.ConnectionString, parameters, storedProcedureName);

                    transactionScope.Complete();
                    result = true;
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

            return result;
        }

        #endregion Public methods

        #region Private method

        private void UpdateIntegrationActivityLogs(SqlDataReader reader, IntegrationActivityLogCollection integrationActivityLog, OperationResultCollection operationResultCollection)
        {
            while (reader.Read())
            {
                IntegrationActivityLog activityLogToUpdate = null;
                Int64 id = -1;
                Boolean hasError = false;
                String errorMessage = String.Empty;
                String referenceId = String.Empty;

                #region Read values

                if (reader["Id"] != null)
                {
                    id = ValueTypeHelper.Int64TryParse(reader["Id"].ToString(), -1);
                }

                if (reader["HasError"] != null)
                {
                    hasError = ValueTypeHelper.BooleanTryParse(reader["HasError"].ToString(), false);
                }

                if (reader["ErrorMessage"] != null)
                {
                    errorMessage = reader["ErrorMessage"].ToString();
                }

                if (reader["ReferenceId"] != null)
                {
                    referenceId = reader["ReferenceId"].ToString();
                }

                #endregion Read values

                if (!String.IsNullOrWhiteSpace(referenceId))
                {
                    activityLogToUpdate = integrationActivityLog.Where(log => log.ReferenceId == referenceId).FirstOrDefault();
                    activityLogToUpdate.Id = id;
                }

                OperationResult operationResult = operationResultCollection.Where(or => or.ReferenceId == referenceId).FirstOrDefault();

                if (hasError)
                {
                    operationResult.AddOperationResult(String.Empty, errorMessage, OperationResultType.Error);
                }
            }
        }

        private List<SqlDataRecord> GetSqlRecords(IntegrationActivityLogCollection integrationActivityLogCollection, SqlMetaData[] sqlMetadata)
        {
            List<SqlDataRecord> integrationActivityLogSqlRecords = new List<SqlDataRecord>();

            foreach (IntegrationActivityLog integrationActivityLog in integrationActivityLogCollection)
            {
                SqlDataRecord integrationActivityLogRecord = new SqlDataRecord(sqlMetadata);

                integrationActivityLogRecord.SetValue(0, integrationActivityLog.MDMObjectId);
                integrationActivityLogRecord.SetValue(1, integrationActivityLog.MDMObjectTypeId);
                integrationActivityLogRecord.SetValue(2, ( Int16 )integrationActivityLog.IntegrationType);
                integrationActivityLogRecord.SetValue(3, integrationActivityLog.IntegrationMessageTypeId);
                integrationActivityLogRecord.SetValue(4, integrationActivityLog.ConnectorId);
                integrationActivityLogRecord.SetValue(5, integrationActivityLog.Context);
                integrationActivityLogRecord.SetValue(6, integrationActivityLog.MessageCount);
                integrationActivityLogRecord.SetValue(7, integrationActivityLog.Weightage);

                integrationActivityLogSqlRecords.Add(integrationActivityLogRecord);
            }

            return integrationActivityLogSqlRecords;
        }

        private IntegrationActivityLogCollection ReadIntegrationActivityLog(SqlDataReader reader)
        {
            IntegrationActivityLogCollection integrationActivityLogCollection = new IntegrationActivityLogCollection();

            if (reader != null)
            {
                while (reader.Read())
                {
                    #region Declaration

                    Int64 id = -1;
                    Int64 mdmObjectId = -1;
                    Int16 mdmObjectTypeId = -1;
                    String mdmObjectTypeName = String.Empty;
                    Int16 messageTypeId = -1;
                    String messageTypeLongName = String.Empty;
                    String messageTypeName = String.Empty;
                    Int16 connectorId = -1;
                    String connectorShortName = String.Empty;
                    String connectorLongName = String.Empty;
                    String context = String.Empty;
                    Int32 messageCount = -1;
                    Boolean isLoadingInProgress = false;
                    Boolean isLoaded = false;
                    Boolean isProcessed = false;
                    Int32 serverId = -1;
                    Int32 weightage = -1;
                    DateTime? qualifierStartTime = null;
                    DateTime? qualifierEndTime = null;
                    DateTime? processStartTime = null;
                    DateTime? processEndTime = null;
                    IntegrationType iType = IntegrationType.Unknown;

                    #endregion Declaration

                    #region Read properties

                    if (reader["PK_Integration_ActivityLog"] != null)
                        id = ValueTypeHelper.Int64TryParse(reader["PK_Integration_ActivityLog"].ToString(), -1);

                    if (reader["MDMObjectId"] != null)
                        mdmObjectId = ValueTypeHelper.Int64TryParse(reader["MDMObjectId"].ToString(), -1);

                    if (reader["FK_MDMObjectType"] != null)
                        mdmObjectTypeId = ValueTypeHelper.Int16TryParse(reader["FK_MDMObjectType"].ToString(), -1);

                    if (reader["MDMObjectTypeName"] != null)
                        mdmObjectTypeName = reader["MDMObjectTypeName"].ToString();

                    if (reader["FK_Integration_MessageType"] != null)
                        messageTypeId = ValueTypeHelper.Int16TryParse(reader["FK_Integration_MessageType"].ToString(), -1);

                    if (reader["IntegrationMessageTypeLongName"] != null)
                        messageTypeLongName = reader["IntegrationMessageTypeLongName"].ToString();

                    if (reader["IntegrationMessageTypeName"] != null)
                        messageTypeName = reader["IntegrationMessageTypeName"].ToString();

                    if (reader["FK_Connector"] != null)
                        connectorId = ValueTypeHelper.Int16TryParse(reader["FK_Connector"].ToString(), -1);

                    if (reader["ConnectorName"] != null)
                        connectorShortName = reader["ConnectorName"].ToString();

                    if (reader["ConnectorLongName"] != null)
                        connectorLongName = reader["ConnectorLongName"].ToString();

                    if (reader["MessageContext"] != null)
                        context = reader["MessageContext"].ToString();

                    if (reader["MessageCount"] != null)
                        messageCount = ValueTypeHelper.Int32TryParse(reader["MessageCount"].ToString(), -1);

                    if (reader["IsLoadingInProgress"] != null)
                        isLoadingInProgress = ValueTypeHelper.BooleanTryParse(reader["IsLoadingInProgress"].ToString(), false);

                    if (reader["IsLoaded"] != null)
                        isLoaded = ValueTypeHelper.BooleanTryParse(reader["IsLoaded"].ToString(), false);

                    if (reader["IsProcessed"] != null)
                        isProcessed = ValueTypeHelper.BooleanTryParse(reader["IsProcessed"].ToString(), false);

                    if (reader["FK_Server"] != null)
                        serverId = ValueTypeHelper.Int32TryParse(reader["FK_Server"].ToString(), -1);

                    if (reader["Weightage"] != null)
                        weightage = ValueTypeHelper.Int32TryParse(reader["Weightage"].ToString(), -1);

                    if (reader["MessageLoadStartTime"] != null)
                        qualifierStartTime = ValueTypeHelper.ConvertToNullableDateTime(reader["MessageLoadStartTime"].ToString());

                    if (reader["MessageLoadEndTime"] != null)
                        qualifierEndTime = ValueTypeHelper.ConvertToNullableDateTime(reader["MessageLoadEndTime"].ToString());

                    if (reader["ProcessStartTime"] != null)
                        processStartTime = ValueTypeHelper.ConvertToNullableDateTime(reader["ProcessStartTime"].ToString());

                    if (reader["ProcessEndTime"] != null)
                        processEndTime = ValueTypeHelper.ConvertToNullableDateTime(reader["ProcessEndTime"].ToString());

                    if (reader["FK_Integration_Type"] != null)
                        iType = ( IntegrationType )ValueTypeHelper.Int32TryParse(reader["FK_Integration_Type"].ToString(), 0);
                    
                    #endregion Read properties

                    #region Initialize object

                    IntegrationActivityLog log = new IntegrationActivityLog();
                    log.Id = id;
                    log.MDMObjectId = mdmObjectId;
                    log.MDMObjectTypeId = mdmObjectTypeId;
                    log.MDMObjectTypeName = mdmObjectTypeName;
                    log.IntegrationMessageTypeId = messageTypeId;
                    log.IntegrationMessageTypeLongName = messageTypeLongName;
                    log.IntegrationMessageTypeName = messageTypeName;
                    log.ConnectorId = connectorId;
                    log.ConnectorLongName = connectorLongName;
                    log.ConnectorName = connectorShortName;
                    log.Context = context;
                    log.MessageCount = messageCount;
                    log.IsLoadingInProgress = isLoadingInProgress;
                    log.IsLoaded = isLoaded;
                    log.IsProcessed = isProcessed;
                    log.ServerId = serverId;
                    log.Weightage = weightage;
                    log.MessageLoadStartTime = qualifierStartTime;
                    log.MessageLoadEndTime = qualifierEndTime;
                    log.ProcessStartTime = processStartTime;
                    log.ProcessEndTime = processEndTime;
                    log.IntegrationType = iType;

                    #endregion Initialize object

                    integrationActivityLogCollection.Add(log);
                }
            }
            return integrationActivityLogCollection;
        }

        #endregion Private method
    }
}
