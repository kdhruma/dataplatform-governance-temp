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
    /// Specifies integration queue message data access
    /// </summary>
    public class IntegrationQueueDA : SqlClientDataAccessBase
    {
        #region Fields

        private const String _processMethodName = "MDM.IntegrationManager.Data.IntegrationQueueDA.Process";
        private const String _getMethodName = "MDM.IntegrationManager.Data.IntegrationQueueDA.Get";

        #endregion Fields

        /// <summary>
        /// Process integration queue message
        /// </summary>
        /// <param name="integrationQueueItem">IntegrationQueues to process</param>
        /// <param name="programName">Name of program making the change</param>
        /// <param name="userName">Name of user making the change</param>
        /// <param name="command">Connection related properties</param>
        /// <returns>Operation result of processing of integration queue message</returns>
        public OperationResultCollection Process(IntegrationQueueItemCollection integrationQueueItems, String programName, String userName, DBCommandProperties command)
        {
            if (Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.StartTraceActivity(_processMethodName, MDMTraceSource.Integration, false);
            }

            SqlDataReader reader = null;
            OperationResultCollection result = new OperationResultCollection();
            const String storedProcedureName = "usp_IntegrationManager_IntegrationQueue_Process";

            SqlParametersGenerator generator = new SqlParametersGenerator("IntegrationManager_SqlParameters");
            SqlParameter[] parameters = generator.GetParameters("IntegrationManager_IntegrationQueue_Process_ParametersArray");
            SqlMetaData[] sqlMetadata = generator.GetTableValueMetadata("IntegrationManager_IntegrationQueue_Process_ParametersArray", parameters[0].ParameterName);

            List<SqlDataRecord> integrationQueueItemRecords = GetSqlRecords(integrationQueueItems, sqlMetadata);

            using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions() { IsolationLevel = Constants.DEFAULT_ISOLATION_LEVEL }))
            {
                reader = null;
                try
                {

                    parameters[0].Value = integrationQueueItemRecords;
                    parameters[1].Value = userName;
                    parameters[2].Value = programName;

                    reader = ExecuteProcedureReader(command.ConnectionString, parameters, storedProcedureName);

                    //Update integration queue message object with Actual Id in case of create
                    UpdateIntegrationQueueItems(reader, integrationQueueItems, result);

                    transactionScope.Complete();
                }
                finally
                {
                    if (reader != null)
                        reader.Close();

                    if (Constants.TRACING_ENABLED)
                    {
                        MDMTraceHelper.StartTraceActivity(_processMethodName, MDMTraceSource.Integration, false);
                    }
                }

            }

            return result;
        }

        /// <summary>
        /// Get integration queue messages based on message id or batch size
        /// </summary>
        /// <param name="integrationQueueItemId">Id of message to fetch</param>
        /// <param name="batchSize">Count of no. of messages to fetch</param>
        /// <param name="command">Connection related properties</param>
        /// <returns>Integration messages</returns>
        public IntegrationQueueItemCollection Get(Int64 integrationQueueItemId, Int32 batchSize, DBCommandProperties command)
        {
            if (Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.StartTraceActivity(_getMethodName, MDMTraceSource.Integration, false);
            }

            SqlDataReader reader = null;
            SqlParameter[] parameters;
            String storedProcedureName = String.Empty;
            IntegrationQueueItemCollection integrationQueueItems = new IntegrationQueueItemCollection();

            try
            {
                SqlParametersGenerator generator = new SqlParametersGenerator("IntegrationManager_SqlParameters");

                parameters = generator.GetParameters("IntegrationManager_IntegrationQueue_Get_ParametersArray");

                parameters[0].Value = integrationQueueItemId;
                parameters[1].Value = batchSize;

                storedProcedureName = "usp_IntegrationManager_IntegrationQueue_Get";

                reader = ExecuteProcedureReader(command.ConnectionString, parameters, storedProcedureName);

                integrationQueueItems = ReadIntegrationQueueItems(reader);

            }
            finally
            {
                if (reader != null)
                    reader.Close();

                if (Constants.TRACING_ENABLED)
                {
                    MDMTraceHelper.StartTraceActivity(_getMethodName, MDMTraceSource.Integration, false);
                }
            }
            return integrationQueueItems;
        }

        #region Qualifying Queue

        /// <summary>
        /// Process integration queue message
        /// </summary>
        /// <param name="integrationQueueItem">IntegrationQueues to process</param>
        /// <param name="programName">Name of program making the change</param>
        /// <param name="userName">Name of user making the change</param>
        /// <param name="command">Connection related properties</param>
        /// <returns>Operation result of processing of integration queue message</returns>
        public OperationResultCollection ProcessQualifyingQueueItems(IntegrationQueueItemCollection integrationQueueItems, String programName, String userName, DBCommandProperties command)
        {
            if (Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.StartTraceActivity(_processMethodName, MDMTraceSource.Integration, false);
            }

            SqlDataReader reader = null;
            OperationResultCollection result = new OperationResultCollection();
            const String storedProcedureName = "usp_IntegrationManager_QualifyingQueue_Process";

            SqlParametersGenerator generator = new SqlParametersGenerator("IntegrationManager_SqlParameters");
            SqlParameter[] parameters = generator.GetParameters("IntegrationManager_QualifyingQueue_Process_ParametersArray");
            SqlMetaData[] sqlMetadata = generator.GetTableValueMetadata("IntegrationManager_QualifyingQueue_Process_ParametersArray", parameters[0].ParameterName);

            List<SqlDataRecord> integrationQueueItemRecords = GetSqlRecords(integrationQueueItems, sqlMetadata);

            using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions() { IsolationLevel = Constants.DEFAULT_ISOLATION_LEVEL }))
            {
                reader = null;
                try
                {

                    parameters[0].Value = integrationQueueItemRecords;
                    parameters[1].Value = userName;
                    parameters[2].Value = programName;

                    reader = ExecuteProcedureReader(command.ConnectionString, parameters, storedProcedureName);

                    //Update integration queue message object with Actual Id in case of create
                    UpdateIntegrationQueueItems(reader, integrationQueueItems, result);

                    transactionScope.Complete();
                }
                finally
                {
                    if (reader != null)
                        reader.Close();

                    if (Constants.TRACING_ENABLED)
                    {
                        MDMTraceHelper.StartTraceActivity(_processMethodName, MDMTraceSource.Integration, false);
                    }
                }

            }

            return result;
        }

        /// <summary>
        /// Get integration queue messages based on message id or batch size
        /// </summary>
        /// <param name="integrationQueueItemId">Id of message to fetch</param>
        /// <param name="batchSize">Count of no. of messages to fetch</param>
        /// <param name="command">Connection related properties</param>
        /// <returns>Integration messages</returns>
        public IntegrationQueueItemCollection GetQualifyingQueueItems(Int64 integrationQueueItemId, Int32 batchSize, String logStatus, Int64 fromCount, Int64 toCount, DBCommandProperties command)
        {
            if (Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.StartTraceActivity(_getMethodName, MDMTraceSource.Integration, false);
            }

            SqlDataReader reader = null;
            SqlParameter[] parameters;
            String storedProcedureName = String.Empty;
            IntegrationQueueItemCollection qualifyingQueueItems = new IntegrationQueueItemCollection();

            try
            {
                SqlParametersGenerator generator = new SqlParametersGenerator("IntegrationManager_SqlParameters");

                parameters = generator.GetParameters("IntegrationManager_QualifyingQueue_Get_ParametersArray");

                //parameters[0].Value = integrationMessageId;
                //parameters[1].Value = batchSize;
                //parameters[2].Value = logStatus;
                //parameters[3].Value = fromCount;
                //parameters[4].Value = toCount;

                parameters[0].Value = logStatus;
                parameters[1].Value = fromCount;
                parameters[2].Value = toCount;

                storedProcedureName = "usp_IntegrationManager_QualifyingQueue_Get";

                reader = ExecuteProcedureReader(command.ConnectionString, parameters, storedProcedureName);

                qualifyingQueueItems = ReadQualifyingQueueItems(reader);

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
            return qualifyingQueueItems;
        }

        /// <summary>
        /// Process Integration queue item
        /// </summary>
        /// <param name="integrationQueueItemCollection">IntegrationQueueItems to process</param>
        /// <param name="programName">Name of program making the change</param>
        /// <param name="userName">Name of user making the change</param>
        /// <param name="command">Connection related properties</param>
        /// <returns>Operation result of processing of Integration queue item</returns>
        public Boolean MarkQualifyingQueueItemAsQualified(Int64 qualifyingQueueItemId, String programName, String userName, DBCommandProperties command)
        {
            if (Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.StartTraceActivity(_processMethodName, MDMTraceSource.Integration, false);
            }

            SqlDataReader reader = null;
            Boolean result = false;

            SqlParametersGenerator generator = new SqlParametersGenerator("IntegrationManager_SqlParameters");

            const String storedProcedureName = "usp_IntegrationManager_QualifyingQueue_Loaded";
            SqlParameter[] parameters = generator.GetParameters("IntegrationManager_QueueItem_MarkAsQualified_ParametersArray");

            using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions() { IsolationLevel = Constants.DEFAULT_ISOLATION_LEVEL }))
            {
                reader = null;
                try
                {

                    parameters[0].Value = qualifyingQueueItemId;
                    parameters[1].Value = userName;
                    parameters[2].Value = programName;

                    reader = ExecuteProcedureReader(command.ConnectionString, parameters, storedProcedureName);

                    //Update Integration queue item object with Actual Id in case of create
                    //UpdateIntegrationQueueItems(reader, integrationQueueItemCollection, result);

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

        #endregion Qualifying Queue

        #region Outbound queue

        /// <summary>
        /// Get outbound queue messages based on message id or batch size
        /// </summary>
        /// <param name="integrationQueueItemId">Id of message to fetch</param>
        /// <param name="batchSize">Count of no. of messages to fetch</param>
        /// <param name="command">Connection related properties</param>
        /// <returns>Integration messages</returns>
        public IntegrationQueueItemCollection GetOutboundQueueItems(Int64 outboundQueueItemId, Int32 batchSize, String logStatus, Int64 fromCount, Int64 toCount, DBCommandProperties command)
        {
            if (Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.StartTraceActivity(_getMethodName, MDMTraceSource.Integration, false);
            }

            SqlDataReader reader = null;
            SqlParameter[] parameters;
            String storedProcedureName = String.Empty;
            IntegrationQueueItemCollection outboundQueueItems = new IntegrationQueueItemCollection();

            try
            {
                SqlParametersGenerator generator = new SqlParametersGenerator("IntegrationManager_SqlParameters");

                parameters = generator.GetParameters("IntegrationManager_OutboundQueue_Get_ParametersArray");

                //parameters[0].Value = integrationMessageId;
                //parameters[1].Value = batchSize;
                //parameters[2].Value = logStatus;
                //parameters[3].Value = fromCount;
                //parameters[4].Value = toCount;

                parameters[0].Value = logStatus;
                parameters[1].Value = fromCount;
                parameters[2].Value = toCount;

                storedProcedureName = "usp_IntegrationManager_OutBoundQueue_Get";

                reader = ExecuteProcedureReader(command.ConnectionString, parameters, storedProcedureName);

                outboundQueueItems = ReadOutboundQueueItems(reader);

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
            return outboundQueueItems;
        }

        /// <summary>
        /// Process Integration queue item
        /// </summary>
        /// <param name="integrationQueueItemCollection">IntegrationQueueItems to process</param>
        /// <param name="programName">Name of program making the change</param>
        /// <param name="userName">Name of user making the change</param>
        /// <param name="command">Connection related properties</param>
        /// <returns>Operation result of processing of Integration queue item</returns>
        public Boolean MarkOutboundQueueItemAsProcessed(Int64 outboundQueueItemId, String programName, String userName, DBCommandProperties command)
        {
            if (Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.StartTraceActivity(_processMethodName, MDMTraceSource.Integration, false);
            }

            SqlDataReader reader = null;
            Boolean result = false;

            SqlParametersGenerator generator = new SqlParametersGenerator("IntegrationManager_SqlParameters");

            const String storedProcedureName = "usp_IntegrationManager_OutBoundQueue_Processed";
            SqlParameter[] parameters = generator.GetParameters("IntegrationManager_OutBoundQueue_MarkAsProcessed_ParametersArray");

            using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions() { IsolationLevel = Constants.DEFAULT_ISOLATION_LEVEL }))
            {
                reader = null;
                try
                {

                    parameters[0].Value = outboundQueueItemId;
                    parameters[1].Value = userName;
                    parameters[2].Value = programName;

                    reader = ExecuteProcedureReader(command.ConnectionString, parameters, storedProcedureName);

                    //Update Integration queue item object with Actual Id in case of create
                    //UpdateIntegrationQueueItems(reader, integrationQueueItemCollection, result);

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

        #endregion Outbound queue

        #region Private method

        private void UpdateIntegrationQueueItems(SqlDataReader reader, IntegrationQueueItemCollection integrationQueueItems, OperationResultCollection result)
        {
            while (reader.Read())
            {
                IntegrationQueueItem queueItemToUpdate = null;
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
                    queueItemToUpdate = integrationQueueItems.Where(log => log.ReferenceId == referenceId).FirstOrDefault();
                    queueItemToUpdate.Id = id;
                }

                OperationResult or = new OperationResult();
                or.ReferenceId = referenceId;

                if (hasError)
                {
                    or.AddOperationResult(String.Empty, errorMessage, OperationResultType.Error);
                }

                result.Add(or);
            }
        }

        private List<SqlDataRecord> GetSqlRecords(IntegrationQueueItemCollection integrationQueueItemCollection, SqlMetaData[] sqlMetadata)
        {
            List<SqlDataRecord> integrationQueueItemSqlRecords = new List<SqlDataRecord>();
            Int32 counter = 0;

            foreach (IntegrationQueueItem integrationQueueItem in integrationQueueItemCollection)
            {
                SqlDataRecord integrationQueueItemRecord = new SqlDataRecord(sqlMetadata);
                //if (String.IsNullOrWhiteSpace(integrationQueueItem.ReferenceId))
                //{
                //    integrationQueueItem.ReferenceId = ( counter-- ).ToString();
                //}

                //integrationQueueItemRecord.SetValue(0, integrationQueueItem.Id);
                //integrationQueueItemRecord.SetValue(1, integrationQueueItem.ReferenceId);
                //integrationQueueItemRecord.SetValue(2, integrationQueueItem.ConnectorId);
                //integrationQueueItemRecord.SetValue(3, integrationQueueItem.IntegrationActivityLogId);
                //integrationQueueItemRecord.SetValue(4, integrationQueueItem.IntegrationTypeId);
                //integrationQueueItemRecord.SetValue(5, integrationQueueItem.IntegrationMessageTypeId);
                //integrationQueueItemRecord.SetValue(6, integrationQueueItem.IsQualifierInProcess);
                //integrationQueueItemRecord.SetValue(7, integrationQueueItem.IsQualified);
                //integrationQueueItemRecord.SetValue(8, integrationQueueItem.QualifierTime);
                //integrationQueueItemRecord.SetValue(9, integrationQueueItem.IntegrationMessageId);
                //integrationQueueItemRecord.SetValue(10, integrationQueueItem.ProcessTime);
                //integrationQueueItemRecord.SetValue(11, integrationQueueItem.IsInProcess);
                //integrationQueueItemRecord.SetValue(12, integrationQueueItem.IsProcessed);
                //integrationQueueItemRecord.SetValue(13, integrationQueueItem.QualifierStartTime);
                //integrationQueueItemRecord.SetValue(14, integrationQueueItem.QualifierEndTime);
                //integrationQueueItemRecord.SetValue(15, integrationQueueItem.ProcessStartTime);
                //integrationQueueItemRecord.SetValue(16, integrationQueueItem.ProcessEndTime);
                //integrationQueueItemRecord.SetValue(13, integrationQueueItem.QualifyingServerId);
                //integrationQueueItemRecord.SetValue(14, integrationQueueItem.ProcessServerId);
                //integrationQueueItemRecord.SetValue(15, integrationQueueItem.QualifyingWeitage);
                //integrationQueueItemRecord.SetValue(16, integrationQueueItem.ProcessWeitage);
                //integrationQueueItemRecord.SetValue(17, integrationQueueItem.Action);

                integrationQueueItemRecord.SetValue(0, integrationQueueItem.IntegrationActivityLogId);
                integrationQueueItemRecord.SetValue(1, (Int16)integrationQueueItem.IntegrationType);
                integrationQueueItemRecord.SetValue(2, integrationQueueItem.IntegrationMessageTypeId);
                integrationQueueItemRecord.SetValue(3, integrationQueueItem.ConnectorId);
                integrationQueueItemRecord.SetValue(4, integrationQueueItem.IntegrationMessageId);
                integrationQueueItemRecord.SetValue(5, integrationQueueItem.QualifyingServerId);
                integrationQueueItemRecord.SetValue(6, integrationQueueItem.QualifyingWeihtage);
                integrationQueueItemRecord.SetValue(7, integrationQueueItem.QualificationComment);
                
                integrationQueueItemSqlRecords.Add(integrationQueueItemRecord);
            }

            return integrationQueueItemSqlRecords;
        }

        private IntegrationQueueItemCollection ReadIntegrationQueueItems(SqlDataReader reader)
        {
            IntegrationQueueItemCollection integrationQueueItemCollection = new IntegrationQueueItemCollection();

            if (reader != null)
            {
                while (reader.Read())
                {
                    #region Declaration

                    Int64 id = -1;
                    Int16 connectorId = -1;
                    String connectorLongName = String.Empty;
                    Int64 integrationActivityLogId = -1;
                    IntegrationType integrationType = IntegrationType.Outbound;
                    Int16 integrationMessageTypeId = -1;
                    String integrationMessageTypeLongName = String.Empty;
                    Boolean isQualifierInProcess = false;
                    Boolean isQualified = false;
                    DateTime qualifierTime = DateTime.Now;
                    Int64 integrationMessageId = -1;
                    DateTime processTime = DateTime.Now;
                    Boolean isInProcess = false;
                    Boolean isProcessed = false;
                    DateTime qualifierStartTime = DateTime.Now;
                    DateTime qualifierEndTime = DateTime.Now;
                    DateTime processStartTime = DateTime.Now;
                    DateTime processEndTime = DateTime.Now;
                    Int32 qualifyingServerId = -1;
                    Int32 processServerId = -1;
                    Int32 qualifyingWeitage = -1;
                    Int32 processWeitage = -1;
                    String referenceId = String.Empty;

                    #endregion Declaration

                    #region Read properties

                    if (reader["Id"] != null)
                        id = ValueTypeHelper.Int64TryParse(reader["Id"].ToString(), -1);

                    if (reader["ConnectorId"] != null)
                        connectorId = ValueTypeHelper.Int16TryParse(reader["ConnectorId"].ToString(), -1);

                    if (reader["ConnectorName"] != null)
                        connectorLongName = reader["ConnectorName"].ToString();

                    if (reader["IntegrationActivityLogId"] != null)
                        integrationActivityLogId = ValueTypeHelper.Int32TryParse(reader["IntegrationActivityLogId"].ToString(), -1);

                    if (reader["IntegrationTypeId"] != null)
                    {
                        IntegrationType iType = IntegrationType.Outbound;
                        if (Enum.TryParse<IntegrationType>(reader["IntegrationTypeId"].ToString(), out iType))
                        {
                            integrationType = iType;
                        }
                    }

                    if (reader["IntegrationMessageTypeId"] != null)
                        integrationMessageTypeId = ValueTypeHelper.Int16TryParse(reader["IntegrationMessageTypeId"].ToString(), -1);

                    if (reader["IntegrationMessageTypeLongName"] != null)
                        integrationMessageTypeLongName = reader["IntegrationMessageTypeLongName"].ToString();

                    if (reader["IsQualifierInProcess"] != null)
                        isQualifierInProcess = ValueTypeHelper.ConvertToBoolean(reader["IsQualifierInProcess"].ToString());

                    if (reader["IsQualified"] != null)
                        isQualified = ValueTypeHelper.ConvertToBoolean(reader["IsQualified"].ToString());

                    if (reader["QualifierTime"] != null)
                        qualifierTime = ValueTypeHelper.ConvertToDateTime(reader["QualifierTime"].ToString());

                    if (reader["IntegrationMessageId"] != null)
                        integrationMessageId = ValueTypeHelper.Int64TryParse(reader["IntegrationMessageId"].ToString(), -1);

                    if (reader["ProcessTime"] != null)
                        processTime = ValueTypeHelper.ConvertToDateTime(reader["ProcessTime"].ToString());

                    if (reader["IsInProcess"] != null)
                        isInProcess = ValueTypeHelper.ConvertToBoolean(reader["IsInProcess"].ToString());

                    if (reader["IsProcessed"] != null)
                        isProcessed = ValueTypeHelper.ConvertToBoolean(reader["IsProcessed"].ToString());

                    if (reader["QualifierStartTime"] != null)
                        qualifierStartTime = ValueTypeHelper.ConvertToDateTime(reader["QualifierStartTime"].ToString());

                    if (reader["QualifierEndTime"] != null)
                        qualifierEndTime = ValueTypeHelper.ConvertToDateTime(reader["QualifierEndTime"].ToString());

                    if (reader["ProcessStartTime"] != null)
                        processStartTime = ValueTypeHelper.ConvertToDateTime(reader["ProcessStartTime"].ToString());

                    if (reader["ProcessEndTime"] != null)
                        processEndTime = ValueTypeHelper.ConvertToDateTime(reader["ProcessEndTime"].ToString());

                    if (reader["QualifyingServerId"] != null)
                        qualifyingServerId = ValueTypeHelper.Int32TryParse(reader["QualifyingServerId"].ToString(), -1);

                    if (reader["ProcessServerId"] != null)
                        processServerId = ValueTypeHelper.Int32TryParse(reader["ProcessServerId"].ToString(), -1);

                    if (reader["QualifyingWeitage"] != null)
                        qualifyingWeitage = ValueTypeHelper.Int32TryParse(reader["QualifyingWeitage"].ToString(), -1);

                    if (reader["ProcessWeitage"] != null)
                        processWeitage = ValueTypeHelper.Int32TryParse(reader["ProcessWeitage"].ToString(), -1);
                    

                    #endregion Read properties

                    #region Initialize object

                    IntegrationQueueItem item = new IntegrationQueueItem();
                    item.Id = id;
                    item.ConnectorId = connectorId;
                    item.ConnectorLongName = connectorLongName;
                    item.IntegrationActivityLogId = integrationActivityLogId;
                    item.IntegrationType = integrationType;
                    item.IntegrationMessageTypeId = integrationMessageTypeId;
                    item.IntegrationMessageTypeLongName = integrationMessageTypeLongName;
                    item.IsQualifierInProcess = isQualifierInProcess;
                    item.IsQualified = isQualified;
                    item.QualifierTime = qualifierTime;
                    item.IntegrationMessageId = integrationMessageId;
                    item.ProcessTime = processTime;
                    item.IsInProcess = isInProcess;
                    item.IsProcessed = isProcessed;
                    item.QualifierStartTime = qualifierStartTime;
                    item.QualifierEndTime = qualifierEndTime;
                    item.ProcessStartTime = processStartTime;
                    item.ProcessEndTime = processEndTime;
                    item.QualifyingServerId = qualifyingServerId;
                    item.ProcessServerId = processServerId;
                    item.QualifyingWeihtage = qualifyingWeitage;
                    item.ProcessWeitage = processWeitage;

                    #endregion Initialize object

                    integrationQueueItemCollection.Add(item);
                }
            }
            return integrationQueueItemCollection;
        }

        private IntegrationQueueItemCollection ReadQualifyingQueueItems(SqlDataReader reader)
        {
            IntegrationQueueItemCollection integrationQueueItemCollection = new IntegrationQueueItemCollection();

            if (reader != null)
            {
                while (reader.Read())
                {
                    #region Declaration

                    Int64 id = -1;
                    Int16 connectorId = -1;
                    String connectorLongName = String.Empty;
                    Int64 integrationActivityLogId = -1;
                    IntegrationType integrationType = IntegrationType.Outbound;
                    Int16 integrationMessageTypeId = -1;
                    String integrationMessageTypeLongName = String.Empty;
                    Boolean isQualifierInProcess = false;
                    Boolean isQualified = false;
                    DateTime qualifierTime = DateTime.Now;
                    Int64 integrationMessageId = -1;
                    DateTime processTime = DateTime.Now;
                    Boolean isInProcess = false;
                    Boolean isProcessed = false;
                    DateTime qualifierStartTime = DateTime.Now;
                    DateTime qualifierEndTime = DateTime.Now;
                    DateTime processStartTime = DateTime.Now;
                    DateTime processEndTime = DateTime.Now;
                    Int32 qualifyingServerId = -1;
                    Int32 processServerId = -1;
                    Int32 qualifyingWeitage = -1;
                    Int32 processWeitage = -1;
                    String referenceId = String.Empty;

                    #endregion Declaration

                    #region Read properties

                    if (reader["PK_QualifyingQueue"] != null)
                        id = ValueTypeHelper.Int64TryParse(reader["PK_QualifyingQueue"].ToString(), -1);

                    if (reader["FK_Connector"] != null)
                        connectorId = ValueTypeHelper.Int16TryParse(reader["FK_Connector"].ToString(), -1);

                    //if (reader["ConnectorName"] != null)
                    //    connectorLongName = reader["ConnectorName"].ToString();

                    if (reader["FK_IntegrationActivityLog"] != null)
                        integrationActivityLogId = ValueTypeHelper.Int32TryParse(reader["FK_IntegrationActivityLog"].ToString(), -1);

                    if (reader["FK_IntegrationType"] != null)
                    {
                        IntegrationType iType = IntegrationType.Outbound;
                        if (Enum.TryParse<IntegrationType>(reader["FK_IntegrationType"].ToString(), out iType))
                        {
                            integrationType = iType;
                        }
                    }

                    if (reader["FK_IntegrationMessageType"] != null)
                        integrationMessageTypeId = ValueTypeHelper.Int16TryParse(reader["FK_IntegrationMessageType"].ToString(), -1);

                    //if (reader["IntegrationMessageTypeLongName"] != null)
                    //    integrationMessageTypeLongName = reader["IntegrationMessageTypeLongName"].ToString();

                    if (reader["IsQualifierInProgress"] != null)
                        isQualifierInProcess = ValueTypeHelper.ConvertToBoolean(reader["IsQualifierInProgress"].ToString());

                    if (reader["IsQualified"] != null)
                        isQualified = ValueTypeHelper.ConvertToBoolean(reader["IsQualified"].ToString());

                    if (reader["QualifierTime"] != null)
                        qualifierTime = ValueTypeHelper.ConvertToDateTime(reader["QualifierTime"].ToString());

                    if (reader["FK_IntegrationMessage"] != null)
                        integrationMessageId = ValueTypeHelper.Int64TryParse(reader["FK_IntegrationMessage"].ToString(), -1);

                    //if (reader["ProcessTime"] != null)
                    //    processTime = ValueTypeHelper.ConvertToDateTime(reader["ProcessTime"].ToString());

                    //if (reader["IsInProcess"] != null)
                    //    isInProcess = ValueTypeHelper.ConvertToBoolean(reader["IsInProcess"].ToString());

                    //if (reader["IsProcessed"] != null)
                    //    isProcessed = ValueTypeHelper.ConvertToBoolean(reader["IsProcessed"].ToString());

                    //if (reader["QualifierStartTime"] != null)
                    //    qualifierStartTime = ValueTypeHelper.ConvertToDateTime(reader["QualifierStartTime"].ToString());

                    //if (reader["QualifierEndTime"] != null)
                    //    qualifierEndTime = ValueTypeHelper.ConvertToDateTime(reader["QualifierEndTime"].ToString());

                    //if (reader["ProcessStartTime"] != null)
                    //    processStartTime = ValueTypeHelper.ConvertToDateTime(reader["ProcessStartTime"].ToString());

                    //if (reader["ProcessEndTime"] != null)
                    //    processEndTime = ValueTypeHelper.ConvertToDateTime(reader["ProcessEndTime"].ToString());

                    if (reader["FK_Qualifying_Server"] != null)
                        qualifyingServerId = ValueTypeHelper.Int32TryParse(reader["FK_Qualifying_Server"].ToString(), -1);

                    //if (reader["ProcessServerId"] != null)
                    //    processServerId = ValueTypeHelper.Int32TryParse(reader["ProcessServerId"].ToString(), -1);

                    if (reader["Qualifying_Weightage"] != null)
                        qualifyingWeitage = ValueTypeHelper.Int32TryParse(reader["Qualifying_Weightage"].ToString(), -1);

                    //if (reader["ProcessWeitage"] != null)
                    //    processWeitage = ValueTypeHelper.Int32TryParse(reader["ProcessWeitage"].ToString(), -1);


                    #endregion Read properties

                    #region Initialize object

                    IntegrationQueueItem item = new IntegrationQueueItem();
                    item.Id = id;
                    item.ConnectorId = connectorId;
                    item.ConnectorLongName = connectorLongName;
                    item.IntegrationActivityLogId = integrationActivityLogId;
                    item.IntegrationType = integrationType;
                    item.IntegrationMessageTypeId = integrationMessageTypeId;
                    item.IntegrationMessageTypeLongName = integrationMessageTypeLongName;
                    item.IsQualifierInProcess = isQualifierInProcess;
                    item.IsQualified = isQualified;
                    item.QualifierTime = qualifierTime;
                    item.IntegrationMessageId = integrationMessageId;
                    item.ProcessTime = processTime;
                    item.IsInProcess = isInProcess;
                    item.IsProcessed = isProcessed;
                    item.QualifierStartTime = qualifierStartTime;
                    item.QualifierEndTime = qualifierEndTime;
                    item.ProcessStartTime = processStartTime;
                    item.ProcessEndTime = processEndTime;
                    item.QualifyingServerId = qualifyingServerId;
                    item.ProcessServerId = processServerId;
                    item.QualifyingWeihtage = qualifyingWeitage;
                    item.ProcessWeitage = processWeitage;

                    #endregion Initialize object

                    integrationQueueItemCollection.Add(item);
                }
            }
            return integrationQueueItemCollection;
        }

        private IntegrationQueueItemCollection ReadOutboundQueueItems(SqlDataReader reader)
        {
            IntegrationQueueItemCollection integrationQueueItemCollection = new IntegrationQueueItemCollection();

            if (reader != null)
            {
                while (reader.Read())
                {
                    #region Declaration

                    Int64 id = -1;
                    Int16 connectorId = -1;
                    String connectorLongName = String.Empty;
                    Int64 integrationActivityLogId = -1;
                    IntegrationType integrationType = IntegrationType.Outbound;
                    Int16 integrationMessageTypeId = -1;
                    String integrationMessageTypeLongName = String.Empty;
                    Boolean isQualifierInProcess = false;
                    Boolean isQualified = false;
                    DateTime qualifierTime = DateTime.Now;
                    Int64 integrationMessageId = -1;
                    DateTime processTime = DateTime.Now;
                    Boolean isInProcess = false;
                    Boolean isProcessed = false;
                    DateTime qualifierStartTime = DateTime.Now;
                    DateTime qualifierEndTime = DateTime.Now;
                    DateTime processStartTime = DateTime.Now;
                    DateTime processEndTime = DateTime.Now;
                    Int32 qualifierServerId = -1;
                    Int32 processServerId = -1;
                    Int32 outboundWeitage = -1;
                    Int32 processWeitage = -1;
                    String referenceId = String.Empty;

                    #endregion Declaration

                    #region Read properties

                    if (reader["PK_OutboundQueue"] != null)
                        id = ValueTypeHelper.Int64TryParse(reader["PK_OutboundQueue"].ToString(), -1);

                    if (reader["FK_Connector"] != null)
                        connectorId = ValueTypeHelper.Int16TryParse(reader["FK_Connector"].ToString(), -1);

                    //if (reader["ConnectorName"] != null)
                    //    connectorLongName = reader["ConnectorName"].ToString();

                    if (reader["FK_IntegrationActivityLog"] != null)
                        integrationActivityLogId = ValueTypeHelper.Int32TryParse(reader["FK_IntegrationActivityLog"].ToString(), -1);

                    if (reader["FK_IntegrationType"] != null)
                    {
                        IntegrationType iType = IntegrationType.Outbound;
                        if (Enum.TryParse<IntegrationType>(reader["FK_IntegrationType"].ToString(), out iType))
                        {
                            integrationType = iType;
                        }
                    }

                    if (reader["FK_IntegrationMessageType"] != null)
                        integrationMessageTypeId = ValueTypeHelper.Int16TryParse(reader["FK_IntegrationMessageType"].ToString(), -1);

                    //if (reader["IntegrationMessageTypeLongName"] != null)
                    //    integrationMessageTypeLongName = reader["IntegrationMessageTypeLongName"].ToString();

                    //if (reader["IsQualifierInProgress"] != null)
                    //    isQualifierInProcess = ValueTypeHelper.ConvertToBoolean(reader["IsQualifierInProgress"].ToString());

                    //if (reader["IsQualified"] != null)
                    //    isQualified = ValueTypeHelper.ConvertToBoolean(reader["IsQualified"].ToString());

                    //if (reader["QualifierTime"] != null)
                    //    qualifierTime = ValueTypeHelper.ConvertToDateTime(reader["QualifierTime"].ToString());

                    if (reader["FK_IntegrationMessage"] != null)
                        integrationMessageId = ValueTypeHelper.Int64TryParse(reader["FK_IntegrationMessage"].ToString(), -1);

                    if (reader["ProcessTime"] != null)
                        processTime = ValueTypeHelper.ConvertToDateTime(reader["ProcessTime"].ToString());

                    if (reader["IsInProcess"] != null)
                        isInProcess = ValueTypeHelper.ConvertToBoolean(reader["IsInProcess"].ToString());

                    if (reader["IsProcessed"] != null)
                        isProcessed = ValueTypeHelper.ConvertToBoolean(reader["IsProcessed"].ToString());

                    //if (reader["QualifierStartTime"] != null)
                    //    qualifierStartTime = ValueTypeHelper.ConvertToDateTime(reader["QualifierStartTime"].ToString());

                    //if (reader["QualifierEndTime"] != null)
                    //    qualifierEndTime = ValueTypeHelper.ConvertToDateTime(reader["QualifierEndTime"].ToString());

                    //if (reader["ProcessStartTime"] != null)
                    //    processStartTime = ValueTypeHelper.ConvertToDateTime(reader["ProcessStartTime"].ToString());

                    //if (reader["ProcessEndTime"] != null)
                    //    processEndTime = ValueTypeHelper.ConvertToDateTime(reader["ProcessEndTime"].ToString());

                    if (reader["FK_Process_Server"] != null)
                        processServerId = ValueTypeHelper.Int32TryParse(reader["FK_Process_Server"].ToString(), -1);

                    //if (reader["ProcessServerId"] != null)
                    //    processServerId = ValueTypeHelper.Int32TryParse(reader["ProcessServerId"].ToString(), -1);

                    if (reader["Process_Weightage"] != null)
                        outboundWeitage = ValueTypeHelper.Int32TryParse(reader["Process_Weightage"].ToString(), -1);

                    //if (reader["Process_Weightage"] != null)
                    //    processWeitage = ValueTypeHelper.Int32TryParse(reader["Process_Weightage"].ToString(), -1);


                    #endregion Read properties

                    #region Initialize object

                    IntegrationQueueItem item = new IntegrationQueueItem();
                    item.Id = id;
                    item.ConnectorId = connectorId;
                    item.ConnectorLongName = connectorLongName;
                    item.IntegrationActivityLogId = integrationActivityLogId;
                    item.IntegrationType = integrationType;
                    item.IntegrationMessageTypeId = integrationMessageTypeId;
                    item.IntegrationMessageTypeLongName = integrationMessageTypeLongName;
                    item.IsQualifierInProcess = isQualifierInProcess;
                    item.IsQualified = isQualified;
                    item.QualifierTime = qualifierTime;
                    item.IntegrationMessageId = integrationMessageId;
                    item.ProcessTime = processTime;
                    item.IsInProcess = isInProcess;
                    item.IsProcessed = isProcessed;
                    item.QualifierStartTime = qualifierStartTime;
                    item.QualifierEndTime = qualifierEndTime;
                    item.ProcessStartTime = processStartTime;
                    item.ProcessEndTime = processEndTime;
                    //item.OutboundServerId = outboundServerId;
                    item.ProcessServerId = processServerId;
                    //item.OutboundWeihtage = outboundWeitage;
                    item.ProcessWeitage = processWeitage;

                    #endregion Initialize object

                    integrationQueueItemCollection.Add(item);
                }
            }
            return integrationQueueItemCollection;
        }

        #endregion Private method
    }
}
