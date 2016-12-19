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
    /// Specifies outbound queue item data access
    /// </summary>
    public class OutboundQueueDA : SqlClientDataAccessBase
    {
        #region Fields

        private const String _processMethodName = "MDM.IntegrationManager.Data.OutboundQueueDA.Process";
        private const String _getMethodName = "MDM.IntegrationManager.Data.OutboundQueueDA.Get";
        private const String _markAsProcessedMethodName = "MDM.IntegrationManager.Data.OutboundQueueDA.MarkAsProcessed";

        #endregion Fields

        /// <summary>
        /// Process outbound queue item
        /// </summary>
        /// <param name="outboundQueueItem">OutboundQueues to process</param>
        /// <param name="programName">Name of program making the change</param>
        /// <param name="userName">Name of user making the change</param>
        /// <param name="command">Connection related properties</param>
        /// <returns>Operation result of processing of outbound queue item</returns>
        public void Process(OutboundQueueItemCollection outboundQueueItems, String programName, String userName, Int16 serverId, OperationResultCollection operationResultCollection, DBCommandProperties command)
        {
            if (Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.StartTraceActivity(_processMethodName, MDMTraceSource.Integration, false);
            }

            SqlDataReader reader = null;
            const String storedProcedureName = "usp_IntegrationManager_IntegrationQueue_Process";

            SqlParametersGenerator generator = new SqlParametersGenerator("IntegrationManager_SqlParameters");
            SqlParameter[] parameters = generator.GetParameters("IntegrationManager_OutboundQueue_Process_ParametersArray");
            SqlMetaData[] sqlMetadata = generator.GetTableValueMetadata("IntegrationManager_OutboundQueue_Process_ParametersArray", parameters[0].ParameterName);

            List<SqlDataRecord> outboundQueueItemRecords = GetSqlRecords(outboundQueueItems, sqlMetadata, serverId);

            using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions() { IsolationLevel = Constants.DEFAULT_ISOLATION_LEVEL }))
            {
                reader = null;
                try
                {

                    parameters[0].Value = outboundQueueItemRecords;
                    parameters[1].Value = programName;
                    parameters[2].Value = userName;

                    reader = ExecuteProcedureReader(command.ConnectionString, parameters, storedProcedureName);

                    //Update outbound queue item object with Actual Id in case of create
                    UpdateOutboundQueueItems(reader, outboundQueueItems, operationResultCollection);

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
        }

        /// <summary>
        /// Get outbound queue items
        /// </summary>
        /// <param name="command">Connection related properties</param>
        /// <returns>Outbound items</returns>
        public OutboundQueueItemCollection Get(String logStatus, Int64 fromCount, Int64 toCount, String programName, String userName, Int16 serverId, DBCommandProperties command)
        {
            if (Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.StartTraceActivity(_getMethodName, MDMTraceSource.Integration, false);
            }

            SqlDataReader reader = null;
            SqlParameter[] parameters;
            OutboundQueueItemCollection outboundQueueItems = new OutboundQueueItemCollection();
            String storedProcedureName = "usp_IntegrationManager_IntegrationQueue_Get";
            SqlParametersGenerator generator = new SqlParametersGenerator("IntegrationManager_SqlParameters");
            parameters = generator.GetParameters("IntegrationManager_OutboundQueue_Get_ParametersArray");

            using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions() { IsolationLevel = Constants.DEFAULT_ISOLATION_LEVEL }))
            {
                try
                {
                    parameters[0].Value = ( Int16 )IntegrationType.Outbound;
                    parameters[1].Value = logStatus;
                    parameters[2].Value = fromCount;
                    parameters[3].Value = toCount;
                    parameters[4].Value = programName;
                    parameters[5].Value = userName;
                    parameters[6].Value = serverId;

                    reader = ExecuteProcedureReader(command.ConnectionString, parameters, storedProcedureName);

                    outboundQueueItems = ReadOutboundQueueItems(reader);
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
            return outboundQueueItems;
        }

        /// <summary>
        /// Update outbound queue item status to indicate it is processed and when it should be picked up for next process
        /// </summary>
        /// <param name="outboundQueueItemId">OutboundQueueItem id to be updated</param>
        /// <param name="scheduledProcessTime">Time when a particular message should be processed</param>
        /// <param name="programName">Name of program making the change</param>
        /// <param name="userName">Name of user making the change</param>
        /// <param name="command">Connection related properties</param>
        /// <returns></returns>
        public Boolean MarkAsProcessed(Int64 outboundQueueItemId, String programName, String userName, Int16 serverId, DBCommandProperties command)
        {
            if (Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.StartTraceActivity(_markAsProcessedMethodName, MDMTraceSource.Integration, false);
            }

            SqlDataReader reader = null;
            Boolean result = true;

            SqlParametersGenerator generator = new SqlParametersGenerator("IntegrationManager_SqlParameters");

            const String storedProcedureName = "usp_IntegrationManager_IntegrationQueue_Processed";
            SqlParameter[] parameters = generator.GetParameters("IntegrationManager_OutboundQueue_MarkAsProcessed_ParametersArray");

            using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions() { IsolationLevel = Constants.DEFAULT_ISOLATION_LEVEL }))
            {
                reader = null;
                try
                {
                    parameters[0].Value = outboundQueueItemId;
                    parameters[1].Value = programName;
                    parameters[2].Value = userName;
                    parameters[3].Value = serverId;

                    reader = ExecuteProcedureReader(command.ConnectionString, parameters, storedProcedureName);

                    transactionScope.Complete();
                }
                finally
                {
                    if (reader != null)
                        reader.Close();

                    if (Constants.TRACING_ENABLED)
                    {
                        MDMTraceHelper.StopTraceActivity(_markAsProcessedMethodName, MDMTraceSource.Integration);
                    }
                }
            }

            return result;
        }

        #region Private method

        private void UpdateOutboundQueueItems(SqlDataReader reader, OutboundQueueItemCollection outboundQueueItems, OperationResultCollection operationResultCollection)
        {
            while (reader.Read())
            {
                OutboundQueueItem queueItemToUpdate = null;
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
                    queueItemToUpdate = outboundQueueItems.Where(item => item.ReferenceId == referenceId).FirstOrDefault();
                    queueItemToUpdate.Id = id;
                }

                OperationResult operationResult = operationResultCollection.Where(or => or.ReferenceId == referenceId).FirstOrDefault();

                if (hasError)
                {
                    operationResult.AddOperationResult(String.Empty, errorMessage, OperationResultType.Error);
                }

            }
        }

        private List<SqlDataRecord> GetSqlRecords(OutboundQueueItemCollection outboundQueueItemCollection, SqlMetaData[] sqlMetadata, Int32 serverId)
        {
            List<SqlDataRecord> outboundQueueItemSqlRecords = new List<SqlDataRecord>();

            foreach (OutboundQueueItem outboundQueueItem in outboundQueueItemCollection)
            {
                SqlDataRecord outboundQueueItemRecord = new SqlDataRecord(sqlMetadata);

                outboundQueueItemRecord.SetValue(0, outboundQueueItem.IntegrationActivityLogId);
                outboundQueueItemRecord.SetValue(1, ( Int16 )outboundQueueItem.IntegrationType);
                outboundQueueItemRecord.SetValue(2, outboundQueueItem.IntegrationMessageTypeId);
                outboundQueueItemRecord.SetValue(3, outboundQueueItem.ConnectorId);
                outboundQueueItemRecord.SetValue(4, outboundQueueItem.IntegrationMessageId);
                outboundQueueItemRecord.SetValue(5, outboundQueueItem.QualifyingQueueItemId);
                outboundQueueItemRecord.SetValue(6, serverId);
                outboundQueueItemRecord.SetValue(7, outboundQueueItem.Weightage);
                outboundQueueItemRecord.SetValue(8, outboundQueueItem.ScheduledProcessTime);
                //outboundQueueItemRecord.SetValue(7, outboundQueueItem.Comment);//TODO :: check how to send collection to DB

                outboundQueueItemSqlRecords.Add(outboundQueueItemRecord);
            }

            return outboundQueueItemSqlRecords;
        }

        private OutboundQueueItemCollection ReadOutboundQueueItems(SqlDataReader reader)
        {
            OutboundQueueItemCollection outboundQueueItemCollection = new OutboundQueueItemCollection();

            if (reader != null)
            {
                while (reader.Read())
                {
                    #region Declaration

                    Int64 id = -1;
                    Int16 connectorId = -1;
                    String connectorLongName = String.Empty;
                    Int64 integrationActivityLogId = -1;
                    IntegrationType integrationType = IntegrationType.Unknown;
                    Int16 integrationMessageTypeId = -1;
                    String integrationMessageTypeLongName = String.Empty;
                    Boolean isInProgress = false;
                    Boolean isProcessed = false;
                    DateTime? scheduledProcessTime = null;
                    Int64 integrationMessageId = -1;
                    DateTime? startTime = null;
                    DateTime? endTime = null;
                    Int32 serverId = -1;
                    Int32 weightage = -1;

                    #endregion Declaration

                    #region Read properties

                    if (reader["PK_Integration_Queue"] != null)
                        id = ValueTypeHelper.Int64TryParse(reader["PK_Integration_Queue"].ToString(), -1);

                    if (reader["FK_Connector"] != null)
                        connectorId = ValueTypeHelper.Int16TryParse(reader["FK_Connector"].ToString(), -1);

                    if (reader["FK_Integration_ActivityLog"] != null)
                        integrationActivityLogId = ValueTypeHelper.Int32TryParse(reader["FK_Integration_ActivityLog"].ToString(), -1);

                    if (reader["FK_Integration_Type"] != null)
                        integrationType = ( IntegrationType )ValueTypeHelper.Int32TryParse(reader["FK_Integration_Type"].ToString(), 0);

                    if (reader["FK_Integration_MessageType"] != null)
                        integrationMessageTypeId = ValueTypeHelper.Int16TryParse(reader["FK_Integration_MessageType"].ToString(), -1);

                    if (reader["IsInProcess"] != null)
                        isInProgress = ValueTypeHelper.BooleanTryParse(reader["IsInProcess"].ToString(), false);

                    if (reader["IsProcessed"] != null)
                        isProcessed = ValueTypeHelper.BooleanTryParse(reader["IsProcessed"].ToString(), false);

                    if (reader["ScheduledProcessTime"] != null)
                        scheduledProcessTime = ValueTypeHelper.ConvertToNullableDateTime(reader["ScheduledProcessTime"].ToString());

                    if (reader["FK_Integration_Message"] != null)
                        integrationMessageId = ValueTypeHelper.Int64TryParse(reader["FK_Integration_Message"].ToString(), -1);

                    if (reader["FK_Server"] != null)
                        serverId = ValueTypeHelper.Int32TryParse(reader["FK_Server"].ToString(), -1);

                    if (reader["Process_Weightage"] != null)
                        weightage = ValueTypeHelper.Int32TryParse(reader["Process_Weightage"].ToString(), -1);


                    #endregion Read properties

                    #region Initialize object

                    OutboundQueueItem item = new OutboundQueueItem();
                    item.Id = id;
                    item.ConnectorId = connectorId;
                    item.ConnectorLongName = connectorLongName;
                    item.IntegrationActivityLogId = integrationActivityLogId;
                    item.IntegrationType = integrationType;
                    item.IntegrationMessageTypeId = integrationMessageTypeId;
                    item.IntegrationMessageTypeLongName = integrationMessageTypeLongName;
                    item.IsInProgress = isInProgress;
                    item.ScheduledProcessTime = scheduledProcessTime;
                    item.IntegrationMessageId = integrationMessageId;
                    item.IsProcessed = isProcessed;
                    item.StartTime = startTime;
                    item.EndTime = endTime;
                    item.ServerId = serverId;
                    item.Weightage = weightage;
                    item.IsProcessed = isProcessed;

                    #endregion Initialize object

                    outboundQueueItemCollection.Add(item);
                }
            }
            return outboundQueueItemCollection;
        }

        #endregion Private method
    }
}
