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
    /// Specifies integration message type data access
    /// </summary>
    public class IntegrationMessageDA : SqlClientDataAccessBase
    {
        #region Fields

        private const String _processMethodName = "MDM.IntegrationManager.Data.IntegrationMessageDA.Process";
        private const String _getMethodName = "MDM.IntegrationManager.Data.IntegrationMessageDA.Get";

        #endregion Fields

        /// <summary>
        /// Process integration message
        /// </summary>
        /// <param name="integrationMessages">IntegrationMessages to process</param>
        /// <param name="programName">Name of program making the change</param>
        /// <param name="userName">Name of user making the change</param>
        /// <param name="command">Connection related properties</param>
        /// <returns>Operation result of processing of integration message</returns>
        public OperationResultCollection Process(IntegrationMessageCollection integrationMessages, String programName, String userName, DBCommandProperties command)
        {
            if (Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.StartTraceActivity(_processMethodName, MDMTraceSource.Integration,  false);
            }

            SqlDataReader reader = null;
            OperationResultCollection result = new OperationResultCollection();
            const String storedProcedureName = "usp_IntegrationManager_IntegrationMessage_Process";
            
            SqlParametersGenerator generator = new SqlParametersGenerator("IntegrationManager_SqlParameters");
            SqlParameter[] parameters = generator.GetParameters("IntegrationManager_IntegrationMessage_Process_ParametersArray");
            SqlMetaData[] sqlMetadata = generator.GetTableValueMetadata("IntegrationManager_IntegrationMessage_Process_ParametersArray", parameters[0].ParameterName);
            SqlMetaData[] sqlParentMetadata = generator.GetTableValueMetadata("IntegrationManager_IntegrationMessage_Process_ParametersArray", parameters[1].ParameterName);

            List<SqlDataRecord> integrationMessageRecords = null;
            List<SqlDataRecord> mappingRecords = null;

            GetSqlRecords(integrationMessages, sqlMetadata, sqlParentMetadata, out integrationMessageRecords, out mappingRecords);

            using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions() { IsolationLevel = Constants.DEFAULT_ISOLATION_LEVEL }))
            {
                reader = null;
                try
                {

                    parameters[0].Value = integrationMessageRecords;
                    parameters[1].Value = mappingRecords;
                    parameters[2].Value = programName;
                    parameters[3].Value = userName;

                    reader = ExecuteProcedureReader(command.ConnectionString, parameters, storedProcedureName);

                    //Update integration message object with Actual Id in case of create
                    UpdateIntegrationMessages(reader, integrationMessages, result);

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
        /// Get integration messages based on message id or batch size
        /// </summary>
        /// <param name="integrationMessageIds">Collection of message ids to fetch</param>
        /// <param name="batchSize">Count of no. of messages to fetch</param>
        /// <param name="command">Connection related properties</param>
        /// <returns>Integration messages</returns>
        public IntegrationMessageCollection Get(Collection<Int64> integrationMessageIds, Int32 batchSize, DBCommandProperties command)
        {
            if (Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.StartTraceActivity(_getMethodName, MDMTraceSource.Integration, false);
            }

            SqlDataReader reader = null;
            SqlParameter[] parameters;
            String storedProcedureName = String.Empty;
            IntegrationMessageCollection integrationMessages = new IntegrationMessageCollection();

            try
            {
                SqlParametersGenerator generator = new SqlParametersGenerator("IntegrationManager_SqlParameters");

                parameters = generator.GetParameters("IntegrationManager_IntegrationMessage_Get_ParametersArray");
                SqlMetaData[] sqlMetadata = generator.GetTableValueMetadata("IntegrationManager_IntegrationMessage_Get_ParametersArray", parameters[0].ParameterName);

                List<SqlDataRecord> integrationMessageRecords = GetSqlRecordsForGet(integrationMessageIds, sqlMetadata);

                storedProcedureName = "usp_IntegrationManager_IntegrationMessage_Get";

                parameters[0].Value = integrationMessageRecords;

                reader = ExecuteProcedureReader(command.ConnectionString, parameters, storedProcedureName);

                if (reader != null)
                {
                    while (reader.Read())
                    {
                        //Int64 id = -1;
                        Int64 messageId = -1;
                        Int64 aggregatedMessageId = -1;
                        Int16 mdmObjectTypeId = -1;
                        String objectTypeName = String.Empty;
                        String messageXml = String.Empty;

                        if (reader["PK_Integration_Message"] != null)
                            messageId = ValueTypeHelper.Int64TryParse(reader["PK_Integration_Message"].ToString(), -1);

                        if (reader["FK_Integration_Message_Aggregated"] != null)
                            aggregatedMessageId = ValueTypeHelper.Int64TryParse(reader["FK_Integration_Message_Aggregated"].ToString(), -1);

                        if (reader["FK_MDMObjectType"] != null)
                            mdmObjectTypeId = ValueTypeHelper.Int16TryParse(reader["FK_MDMObjectType"].ToString(), -1);

                        if (reader["MessageXml"] != null)
                            messageXml = reader["MessageXml"].ToString();

                        IntegrationMessage msg = new IntegrationMessage(messageXml);
                        msg.Id = messageId;
                        msg.MDMObjectTypeId = mdmObjectTypeId;
                        msg.AggregatedMessageId = aggregatedMessageId;

                        integrationMessages.Add(msg);
                    }
                }

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
            return integrationMessages;
        }

        #region Private method

        private void UpdateIntegrationMessages(SqlDataReader reader, IntegrationMessageCollection integrationMessages, OperationResultCollection result)
        {
            while (reader.Read())
            {
                IntegrationMessage message = null;
                Int64 id = -1;
                String errorMessage = String.Empty;
                String referenceId = String.Empty;

                #region Read values

                if (reader["Id"] != null)
                {
                    id = ValueTypeHelper.Int64TryParse(reader["Id"].ToString(), -1);
                }

                //if (reader["HasError"] != null)
                //{
                //    hasError = ValueTypeHelper.BooleanTryParse(reader["HasError"].ToString(), false);
                //}

                if (reader["Error"] != null)
                {
                    errorMessage = reader["Error"].ToString();
                }

                if (reader["ReferenceId"] != null)
                {
                    referenceId = reader["ReferenceId"].ToString();
                }

                #endregion Read values

                if (!String.IsNullOrWhiteSpace(referenceId))
                {
                    message = integrationMessages.Where(log => log.ReferenceId == referenceId).FirstOrDefault();
                    message.Id = id;
                }

                OperationResult or = new OperationResult();
                or.ReferenceId = referenceId;
                //or.Id = id; //TODO :: need to change OR.Id to Int64

                if (!String.IsNullOrWhiteSpace(errorMessage) && errorMessage.Trim().Length >0)
                {
                    or.AddOperationResult(String.Empty, errorMessage, OperationResultType.Error);
                }

                result.Add(or);
            }
        }

        private void GetSqlRecords(IntegrationMessageCollection integrationMessageCollection, SqlMetaData[] sqlMetadata, SqlMetaData[] mappingSqlMetadata, out List<SqlDataRecord> integrationMessageTVPRecords, out List<SqlDataRecord> parentMessageTVPRecords)
        {
            List<SqlDataRecord> integrationMessageSqlRecords = new List<SqlDataRecord>();
            List<SqlDataRecord> mappingSqlRecords = new List<SqlDataRecord>();
            Int32 counter = 0;

            foreach (IntegrationMessage integrationMessage in integrationMessageCollection)
            {
                SqlDataRecord integrationMessageRecord = new SqlDataRecord(sqlMetadata);

                integrationMessage.ReferenceId = (counter++).ToString();

                integrationMessageRecord.SetValue(0, integrationMessage.ReferenceId);
                integrationMessageRecord.SetValue(1, integrationMessage.Id);
                integrationMessageRecord.SetValue(2, integrationMessage.MDMObjectTypeId);
                integrationMessageRecord.SetValue(3, integrationMessage.ToXml());
                integrationMessageRecord.SetValue(4, integrationMessage.Action.ToString());

                integrationMessageSqlRecords.Add(integrationMessageRecord);

                if (integrationMessage.GetMessageHeader().ParentMessageIds != null)
                {
                    foreach (Int64 parentId in integrationMessage.GetMessageHeader().ParentMessageIds)
                    {
                        SqlDataRecord mappingRecord = new SqlDataRecord(mappingSqlMetadata);
                        mappingRecord.SetValue(0, integrationMessage.ReferenceId);
                        mappingRecord.SetValue(1, parentId);

                        mappingSqlRecords.Add(mappingRecord);
                    }
                }
            }

            if (integrationMessageSqlRecords.Count > 0)
            {
                integrationMessageTVPRecords = integrationMessageSqlRecords;
            }
            else
            {
                integrationMessageTVPRecords = null;
            }

            if (mappingSqlRecords.Count > 0)
            {
                parentMessageTVPRecords = mappingSqlRecords;
            }
            else
            {
                parentMessageTVPRecords = null;
            }
        }

        private List<SqlDataRecord> GetSqlRecordsForGet(Collection<Int64> integrationMessageIds, SqlMetaData[] sqlMetadata)
        {
            List<SqlDataRecord> integrationActivityLogSqlRecords = new List<SqlDataRecord>();
            foreach (Int64 id in integrationMessageIds)
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
