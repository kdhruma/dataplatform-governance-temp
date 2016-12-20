using System;
using System.Data.SqlClient;
using System.Transactions;

namespace MDM.IntegrationManager.Data
{
    using MDM.BusinessObjects;
    using MDM.BusinessObjects.Integration;
    using MDM.Core;
    using MDM.Utility;

    /// <summary>
    /// Specifies inbound queue item data access
    /// </summary>
    public class InboundQueueDA : SqlClientDataAccessBase
    {
        #region Fields

        private const String _getMethodName = "MDM.IntegrationManager.Data.InboundQueueDA.Get";

        #endregion Fields

        /// <summary>
        /// Get inbound queue items
        /// </summary>
        /// <param name="command">Connection related properties</param>
        /// <returns>Inbound items</returns>
        public InboundQueueItemCollection Get(String logStatus, Int64 fromCount, Int64 toCount, String programName, String userName, Int16 serverId, DBCommandProperties command)
        {
            if (Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.StartTraceActivity(_getMethodName, MDMTraceSource.Integration, false);
            }

            SqlDataReader reader = null;
            SqlParameter[] parameters;
            InboundQueueItemCollection inboundQueueItems = new InboundQueueItemCollection();
            String storedProcedureName = "usp_IntegrationManager_IntegrationQueue_Get";
            SqlParametersGenerator generator = new SqlParametersGenerator("IntegrationManager_SqlParameters");
            parameters = generator.GetParameters("IntegrationManager_OutboundQueue_Get_ParametersArray");

            using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions() { IsolationLevel = Constants.DEFAULT_ISOLATION_LEVEL }))
            {
                try
                {
                    parameters[0].Value = (Int16) IntegrationType.Inbound;
                    parameters[1].Value = logStatus;
                    parameters[2].Value = fromCount;
                    parameters[3].Value = toCount;
                    parameters[4].Value = programName;
                    parameters[5].Value = userName;
                    parameters[6].Value = serverId;

                    reader = ExecuteProcedureReader(command.ConnectionString, parameters, storedProcedureName);

                    inboundQueueItems = ReadInboundQueueItems(reader);
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
            return inboundQueueItems;
        }

        #region Private method

        private InboundQueueItemCollection ReadInboundQueueItems(SqlDataReader reader)
        {
            InboundQueueItemCollection inboundQueueItemCollection = new InboundQueueItemCollection();

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

                    InboundQueueItem item = new InboundQueueItem();
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

                    inboundQueueItemCollection.Add(item);
                }
            }
            return inboundQueueItemCollection;
        }

        #endregion Private method
    }
}
