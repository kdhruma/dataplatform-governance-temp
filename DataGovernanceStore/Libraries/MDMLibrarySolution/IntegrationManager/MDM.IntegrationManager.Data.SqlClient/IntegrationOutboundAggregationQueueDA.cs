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
    using System.Collections.ObjectModel;

    /// <summary>
    /// Specifies aggregation outbound queue item data access
    /// </summary>
    public class IntegrationOutboundAggregationQueueDA : SqlClientDataAccessBase
    {
        #region Fields

        private const String _processMethodName = "MDM.IntegrationManager.Data.AggregationOutboundQueueDA.Process";
        private const String _getMethodName = "MDM.IntegrationManager.Data.AggregationOutboundQueueDA.Get";
        private const String _markAsProcessedMethodName = "MDM.IntegrationManager.Data.AggregationOutboundQueueDA.MarkAsProcessed";

        #endregion Fields

        /// <summary>
        /// Process aggregation outbound queue item
        /// </summary>
        /// <param name="aggregationOutboundQueueItems">aggregation outbound queue items to process</param>
        /// <param name="programName">Name of program making the change</param>
        /// <param name="userName">Name of user making the change</param>
        /// <param name="serverId">The server identifier.</param>
        /// <param name="operationResultCollection">The operation result collection.</param>
        /// <param name="command">Connection related properties</param>
        public void Process(AggregationOutboundQueueItemCollection aggregationOutboundQueueItems, String programName, String userName, Int16 serverId, OperationResultCollection operationResultCollection, DBCommandProperties command)
        {
            if (Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.StartTraceActivity(_processMethodName, MDMTraceSource.Integration, false);
            }

            SqlDataReader reader = null;
            const String storedProcedureName = "usp_IntegrationManager_AggregationQueue_Process";

            SqlParametersGenerator generator = new SqlParametersGenerator("IntegrationManager_SqlParameters");
            SqlParameter[] parameters = generator.GetParameters("IntegrationManager_AggregationQueue_Process_ParametersArray");
            SqlMetaData[] sqlMetadata = generator.GetTableValueMetadata("IntegrationManager_AggregationQueue_Process_ParametersArray", parameters[0].ParameterName);

            List<SqlDataRecord> outboundQueueItemRecords = GetSqlRecords(aggregationOutboundQueueItems, sqlMetadata, serverId);

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
                    UpdateAggregationOutboundQueueItems(reader, aggregationOutboundQueueItems, operationResultCollection);

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
        public AggregationOutboundQueueItemCollection Get(IntegrationType integrationType, String logStatus, Int64 fromCount, Int64 toCount, String programName, String userName, Int16 serverId, DBCommandProperties command)
        {
            if (Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.StartTraceActivity(_getMethodName, MDMTraceSource.Integration, false);
            }

            SqlDataReader reader = null;
            SqlParameter[] parameters;
            AggregationOutboundQueueItemCollection outboundQueueItems = new AggregationOutboundQueueItemCollection();
            String storedProcedureName = "usp_IntegrationManager_AggregationQueue_Get";
            SqlParametersGenerator generator = new SqlParametersGenerator("IntegrationManager_SqlParameters");
            parameters = generator.GetParameters("IntegrationManager_AggregationQueue_Get_ParametersArray");

            using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions() { IsolationLevel = Constants.DEFAULT_ISOLATION_LEVEL }))
            {
                try
                {
                    parameters[0].Value = (Int16)integrationType;
                    parameters[1].Value = logStatus;
                    parameters[2].Value = fromCount;
                    parameters[3].Value = toCount;
                    parameters[4].Value = programName;
                    parameters[5].Value = userName;
                    parameters[6].Value = serverId;

                    reader = ExecuteProcedureReader(command.ConnectionString, parameters, storedProcedureName);

                    outboundQueueItems = ReadAggregationOutboundQueueItems(reader);
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
        /// <param name="aggregationOutboundQueueItemId">OutboundQueueItem id to be updated</param>
        /// <param name="scheduledProcessTime">Time when a particular message should be processed</param>
        /// <param name="programName">Name of program making the change</param>
        /// <param name="userName">Name of user making the change</param>
        /// <param name="command">Connection related properties</param>
        /// <returns></returns>
        public Boolean MarkAsProcessed(Collection<Int64> aggregationOutboundQueueItemIds, String programName, String userName, Int16 serverId, DBCommandProperties command)
        {
            if (Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.StartTraceActivity(_markAsProcessedMethodName, MDMTraceSource.Integration, false);
            }

            SqlDataReader reader = null;
            Boolean result = true;

            SqlParametersGenerator generator = new SqlParametersGenerator("IntegrationManager_SqlParameters");

            const String storedProcedureName = "usp_IntegrationManager_AggregationQueue_Processed";
            SqlParameter[] parameters = generator.GetParameters("IntegrationManager_AggregationQueue_Processed_ParametersArray");
            SqlMetaData[] sqlMetadata = generator.GetTableValueMetadata("IntegrationManager_AggregationQueue_Processed_ParametersArray", parameters[0].ParameterName);

            List<SqlDataRecord> aggregationQueueSqlRecords = GetSqlRecordsForProcessed(aggregationOutboundQueueItemIds, sqlMetadata);

            using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions() { IsolationLevel = Constants.DEFAULT_ISOLATION_LEVEL }))
            {
                reader = null;
                try
                {
                    parameters[0].Value = aggregationQueueSqlRecords;
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

        private void UpdateAggregationOutboundQueueItems(SqlDataReader reader, AggregationOutboundQueueItemCollection aggregationOutboundQueueItems, OperationResultCollection operationResultCollection)
        {
            while (reader.Read())
            {
                AggregationOutboundQueueItem queueItemToUpdate = null;
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
                    queueItemToUpdate = aggregationOutboundQueueItems.Where(item => item.ReferenceId == referenceId).FirstOrDefault();
                    queueItemToUpdate.Id = id;
                }

                OperationResult operationResult = operationResultCollection.Where(or => or.ReferenceId == referenceId).FirstOrDefault();

                if (hasError)
                {
                    operationResult.AddOperationResult(String.Empty, errorMessage, OperationResultType.Error);
                }

            }
        }

        private List<SqlDataRecord> GetSqlRecords(AggregationOutboundQueueItemCollection aggregationOutboundQueueItemCollection, SqlMetaData[] sqlMetadata, Int32 serverId)
        {
            List<SqlDataRecord> aggregationOutboundQueueItemSqlRecords = new List<SqlDataRecord>();

            foreach (AggregationOutboundQueueItem aggregationOutboundQueueItem in aggregationOutboundQueueItemCollection)
            {
                SqlDataRecord aggregationOutboundQueueItemRecord = new SqlDataRecord(sqlMetadata);

                aggregationOutboundQueueItemRecord.SetValue(0, aggregationOutboundQueueItem.IntegrationActivityLogId);
                aggregationOutboundQueueItemRecord.SetValue(1, ( Int16 )aggregationOutboundQueueItem.IntegrationType);
                aggregationOutboundQueueItemRecord.SetValue(2, aggregationOutboundQueueItem.IntegrationMessageTypeId);
                aggregationOutboundQueueItemRecord.SetValue(3, aggregationOutboundQueueItem.ConnectorId);
                aggregationOutboundQueueItemRecord.SetValue(4, aggregationOutboundQueueItem.IntegrationMessageId);
                aggregationOutboundQueueItemRecord.SetValue(5, aggregationOutboundQueueItem.QualifyingQueueItemId);
                aggregationOutboundQueueItemRecord.SetValue(6, aggregationOutboundQueueItem.ScheduledProcessTime);
                aggregationOutboundQueueItemRecord.SetValue(7, serverId);
                aggregationOutboundQueueItemRecord.SetValue(8, aggregationOutboundQueueItem.Weightage);

                aggregationOutboundQueueItemSqlRecords.Add(aggregationOutboundQueueItemRecord);
            }

            return aggregationOutboundQueueItemSqlRecords;
        }

        private AggregationOutboundQueueItemCollection ReadAggregationOutboundQueueItems(SqlDataReader reader)
        {
            AggregationOutboundQueueItemCollection aggregationOutboundQueueItemCollection = new AggregationOutboundQueueItemCollection();

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
                    Int64 qualifyingQueueItemId = -1;

                    #endregion Declaration

                    #region Read properties

                    if (reader["PK_Aggregation_Queue"] != null)
                        id = ValueTypeHelper.Int64TryParse(reader["PK_Aggregation_Queue"].ToString(), -1);

                    if (reader["FK_Connector"] != null)
                        connectorId = ValueTypeHelper.Int16TryParse(reader["FK_Connector"].ToString(), -1);

                    if (reader["FK_Integration_ActivityLog"] != null)
                        integrationActivityLogId = ValueTypeHelper.Int32TryParse(reader["FK_Integration_ActivityLog"].ToString(), -1);

                    if (reader["FK_Integration_Type"] != null)
                        integrationType = (IntegrationType)ValueTypeHelper.Int32TryParse(reader["FK_Integration_Type"].ToString(), 0);

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

                    if (reader["FK_QualifyingQueue"] != null)
                        qualifyingQueueItemId = ValueTypeHelper.Int32TryParse(reader["FK_QualifyingQueue"].ToString(), -1);

                    #endregion Read properties

                    #region Initialize object

                    AggregationOutboundQueueItem item = new AggregationOutboundQueueItem();
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
                    item.QualifyingQueueItemId = qualifyingQueueItemId;

                    #endregion Initialize object

                    aggregationOutboundQueueItemCollection.Add(item);
                }
            }
            return aggregationOutboundQueueItemCollection;
        }

        private List<SqlDataRecord> GetSqlRecordsForProcessed(Collection<Int64> aggregationQueueIds, SqlMetaData[] sqlMetadata)
        {
            List<SqlDataRecord> integrationActivityLogSqlRecords = new List<SqlDataRecord>();
            foreach (Int64 id in aggregationQueueIds)
            {
                SqlDataRecord integrationMessageRecord = new SqlDataRecord(sqlMetadata);
                integrationMessageRecord.SetValue(0, id);
                integrationActivityLogSqlRecords.Add(integrationMessageRecord);
            }

            return integrationActivityLogSqlRecords;
        }
        #endregion Private method
    }
}
