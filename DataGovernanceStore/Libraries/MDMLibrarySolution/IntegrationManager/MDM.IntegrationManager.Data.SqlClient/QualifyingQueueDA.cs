using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Specifies qualifying queue item data access
    /// </summary>
    public class QualifyingQueueDA : SqlClientDataAccessBase
    {
        #region Fields

        private const String _processMethodName = "MDM.IntegrationManager.Data.QualifyingQueueDA.Process";
        private const String _getMethodName = "MDM.IntegrationManager.Data.QualifyingQueueDA.Get";
        private const String _updateQualificationStatusMethodName = "MDM.IntegrationManager.Data.QualifyingQueueDA.UpdateQualificationStatus";

        #endregion Fields

        /// <summary>
        /// Process qualifying queue item
        /// </summary>
        /// <param name="qualifyingQueueItem">QualifyingQueues to process</param>
        /// <param name="programName">Name of program making the change</param>
        /// <param name="userName">Name of user making the change</param>
        /// <param name="command">Connection related properties</param>
        /// <returns>Operation result of processing of qualifying queue item</returns>
        public void Process(QualifyingQueueItemCollection qualifyingQueueItems, String programName, String userName, Int16 serverId, OperationResultCollection operationResultCollection, DBCommandProperties command)
        {
            if (Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.StartTraceActivity(_processMethodName, MDMTraceSource.Integration, false);
            }

            SqlDataReader reader = null;
            const String storedProcedureName = "usp_IntegrationManager_QualifyingQueue_Process";

            SqlParametersGenerator generator = new SqlParametersGenerator("IntegrationManager_SqlParameters");
            SqlParameter[] parameters = generator.GetParameters("IntegrationManager_QualifyingQueue_Process_ParametersArray");
            SqlMetaData[] sqlMetadata = generator.GetTableValueMetadata("IntegrationManager_QualifyingQueue_Process_ParametersArray", parameters[0].ParameterName);

            List<SqlDataRecord> qualifyingQueueItemRecords = GetSqlRecords(qualifyingQueueItems, sqlMetadata, serverId);

            using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions() { IsolationLevel = Constants.DEFAULT_ISOLATION_LEVEL }))
            {
                reader = null;
                try
                {
                    parameters[0].Value = qualifyingQueueItemRecords;
                    parameters[1].Value = programName;
                    parameters[2].Value = userName;

                    reader = ExecuteProcedureReader(command.ConnectionString, parameters, storedProcedureName);

                    //Update qualifying queue item object with Actual Id in case of create
                    UpdateQualifyingQueueItems(reader, qualifyingQueueItems, operationResultCollection);

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
        /// Get qualifying queue items
        /// </summary>
        /// <param name="command">Connection related properties</param>
        /// <returns>Qualifying items</returns>
        public QualifyingQueueItemCollection Get(String logStatus, Int64 fromCount, Int64 toCount, String programName, String userName, Int16 serverId, DBCommandProperties command)
        {
            if (Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.StartTraceActivity(_getMethodName, MDMTraceSource.Integration, false);
            }

            SqlDataReader reader = null;
            SqlParameter[] parameters;
            QualifyingQueueItemCollection qualifyingQueueItems = new QualifyingQueueItemCollection();
            String storedProcedureName = "usp_IntegrationManager_QualifyingQueue_Get";
            SqlParametersGenerator generator = new SqlParametersGenerator("IntegrationManager_SqlParameters");
            parameters = generator.GetParameters("IntegrationManager_QualifyingQueue_Get_ParametersArray");

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

                    qualifyingQueueItems = ReadQualifyingQueueItems(reader);
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
            return qualifyingQueueItems;
        }

        /// <summary>
        /// Update qualifying queue item status if it is qualified or not
        /// </summary>
        /// <param name="qualifyingQueueItemId">QualifyingQueueItem id to be updated</param>
        /// <param name="programName">Name of program making the change</param>
        /// <param name="userName">Name of user making the change</param>
        /// <param name="command">Connection related properties</param>
        /// <returns></returns>
        public Boolean UpdateQualificationStatus(Int64 qualifyingQueueItemId, MessageQualificationStatusEnum qualificationStatus, DateTime? scheduledQualifierTime, Collection<String> comments, String programName, String userName, Int16 serverId, DBCommandProperties command)
        {
            if (Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.StartTraceActivity(_updateQualificationStatusMethodName, MDMTraceSource.Integration, false);
            }

            SqlDataReader reader = null;
            Boolean result = true;

            SqlParametersGenerator generator = new SqlParametersGenerator("IntegrationManager_SqlParameters");

            const String storedProcedureName = "usp_IntegrationManager_QualifyingQueue_Loaded";
            SqlParameter[] parameters = generator.GetParameters("IntegrationManager_QueueItem_UpdateQualificationStatus_ParametersArray");

            using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions() { IsolationLevel = Constants.DEFAULT_ISOLATION_LEVEL }))
            {
                reader = null;
                try
                {

                    parameters[0].Value = qualifyingQueueItemId;
                    parameters[1].Value = ( Byte )qualificationStatus;
                    parameters[2].Value = scheduledQualifierTime;
                    //TODO :: Comments storing needs to be corrected in another PBL.
                    parameters[3].Value = comments == null ? String.Empty : ValueTypeHelper.JoinCollection<String>(comments, "||");
                    parameters[4].Value = programName;
                    parameters[5].Value = userName;
                    parameters[6].Value = serverId;

                    reader = ExecuteProcedureReader(command.ConnectionString, parameters, storedProcedureName);

                    transactionScope.Complete();
                }
                finally
                {
                    if (reader != null)
                        reader.Close();

                    if (Constants.TRACING_ENABLED)
                    {
                        MDMTraceHelper.StopTraceActivity(_updateQualificationStatusMethodName, MDMTraceSource.Integration);
                    }
                }
            }

            return result;
        }

        #region Private method

        private void UpdateQualifyingQueueItems(SqlDataReader reader, QualifyingQueueItemCollection qualifyingQueueItems, OperationResultCollection operationResultCollection)
        {
            while (reader.Read())
            {
                QualifyingQueueItem queueItemToUpdate = null;
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
                    queueItemToUpdate = qualifyingQueueItems.Where(item => item.ReferenceId == referenceId).FirstOrDefault();
                    queueItemToUpdate.Id = id;
                }

                OperationResult operationResult = operationResultCollection.Where(or => or.ReferenceId == referenceId).FirstOrDefault();

                if (hasError)
                {
                    operationResult.AddOperationResult(String.Empty, errorMessage, OperationResultType.Error);
                }
            }
        }

        private List<SqlDataRecord> GetSqlRecords(QualifyingQueueItemCollection qualifyingQueueItemCollection, SqlMetaData[] sqlMetadata, Int32 serverId)
        {
            List<SqlDataRecord> qualifyingQueueItemSqlRecords = new List<SqlDataRecord>();

            foreach (QualifyingQueueItem qualifyingQueueItem in qualifyingQueueItemCollection)
            {
                SqlDataRecord qualifyingQueueItemRecord = new SqlDataRecord(sqlMetadata);
                qualifyingQueueItemRecord.SetValue(0, qualifyingQueueItem.IntegrationActivityLogId);
                qualifyingQueueItemRecord.SetValue(1, ( Int16 )qualifyingQueueItem.IntegrationType);
                qualifyingQueueItemRecord.SetValue(2, qualifyingQueueItem.IntegrationMessageTypeId);
                qualifyingQueueItemRecord.SetValue(3, qualifyingQueueItem.ConnectorId);
                qualifyingQueueItemRecord.SetValue(4, qualifyingQueueItem.IntegrationMessageId);
                qualifyingQueueItemRecord.SetValue(5, serverId);
                qualifyingQueueItemRecord.SetValue(6, qualifyingQueueItem.Weightage);
                //qualifyingQueueItemRecord.SetValue(7, qualifyingQueueItem.Comment);//TODO :: check how to send collection to DB

                qualifyingQueueItemSqlRecords.Add(qualifyingQueueItemRecord);
            }

            return qualifyingQueueItemSqlRecords;
        }

        private QualifyingQueueItemCollection ReadQualifyingQueueItems(SqlDataReader reader)
        {
            QualifyingQueueItemCollection qualifyingQueueItemCollection = new QualifyingQueueItemCollection();

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
                    MessageQualificationStatusEnum qualifyingStatus = MessageQualificationStatusEnum.Unknown;
                    DateTime? scheduledQualifierTime = null;
                    Int64 integrationMessageId = -1;
                    Boolean isProcessed = false;
                    DateTime? startTime = null;
                    DateTime? endTime = null;
                    Int32 serverId = -1;
                    Int32 weightage = -1;

                    #endregion Declaration

                    #region Read properties

                    if (reader["PK_QualifyingQueue"] != null)
                        id = ValueTypeHelper.Int64TryParse(reader["PK_QualifyingQueue"].ToString(), -1);

                    if (reader["FK_Connector"] != null)
                        connectorId = ValueTypeHelper.Int16TryParse(reader["FK_Connector"].ToString(), -1);

                    if (reader["FK_Integration_ActivityLog"] != null)
                        integrationActivityLogId = ValueTypeHelper.Int32TryParse(reader["FK_Integration_ActivityLog"].ToString(), -1);

                    if (reader["FK_Integration_Type"] != null)
                        integrationType = ( IntegrationType )ValueTypeHelper.Int32TryParse(reader["FK_Integration_Type"].ToString(), 0);

                    if (reader["FK_Integration_MessageType"] != null)
                        integrationMessageTypeId = ValueTypeHelper.Int16TryParse(reader["FK_Integration_MessageType"].ToString(), -1);

                    if (reader["IsQualifyingInProgress"] != null)
                        isInProgress = ValueTypeHelper.BooleanTryParse(reader["IsQualifyingInProgress"].ToString(), false);

                    if (reader["QualifyingStatus"] != null)
                    {
                        qualifyingStatus = ( MessageQualificationStatusEnum )ValueTypeHelper.Int32TryParse(reader["QualifyingStatus"].ToString(), 0);
                    }

                    if (reader["ScheduledQualifyingTime"] != null)
                        scheduledQualifierTime = ValueTypeHelper.ConvertToNullableDateTime(reader["ScheduledQualifyingTime"].ToString());

                    if (reader["FK_Integration_Message"] != null)
                        integrationMessageId = ValueTypeHelper.Int64TryParse(reader["FK_Integration_Message"].ToString(), -1);

                    if (reader["FK_Server"] != null)
                        serverId = ValueTypeHelper.Int32TryParse(reader["FK_Server"].ToString(), -1);

                    if (reader["Qualifying_Weightage"] != null)
                        weightage = ValueTypeHelper.Int32TryParse(reader["Qualifying_Weightage"].ToString(), -1);

                    #endregion Read properties

                    #region Initialize object

                    QualifyingQueueItem item = new QualifyingQueueItem();
                    item.Id = id;
                    item.ConnectorId = connectorId;
                    item.ConnectorLongName = connectorLongName;
                    item.IntegrationActivityLogId = integrationActivityLogId;
                    item.IntegrationType = integrationType;
                    item.IntegrationMessageTypeId = integrationMessageTypeId;
                    item.IntegrationMessageTypeLongName = integrationMessageTypeLongName;
                    item.IsInProgress = isInProgress;
                    item.MessageQualificationStatus = qualifyingStatus;
                    item.ScheduledQualifierTime = scheduledQualifierTime;
                    item.IntegrationMessageId = integrationMessageId;
                    item.IsProcessed = isProcessed;
                    item.StartTime = startTime;
                    item.EndTime = endTime;
                    item.ServerId = serverId;
                    item.Weightage = weightage;

                    #endregion Initialize object

                    qualifyingQueueItemCollection.Add(item);
                }
            }
            return qualifyingQueueItemCollection;
        }

        #endregion Private method
    }
}
