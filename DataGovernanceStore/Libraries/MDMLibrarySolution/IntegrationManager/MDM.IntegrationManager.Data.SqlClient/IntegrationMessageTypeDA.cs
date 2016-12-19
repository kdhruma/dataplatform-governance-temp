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
    /// Specifies integration message type type data access
    /// </summary>
    public class IntegrationMessageTypeDA : SqlClientDataAccessBase
    {
        #region Fields

        private const String _processMethodName = "MDM.IntegrationManager.Data.IntegrationMessageTypeDA.Process";
        private const String _getMethodName = "MDM.IntegrationManager.Data.IntegrationMessageTypeDA.Get";

        #endregion Fields

        // Uncomment this code for Process API

        /*
        /// <summary>
        /// Process integration message type
        /// </summary>
        /// <param name="integrationMessageTypes">IntegrationMessageTypes to process</param>
        /// <param name="programName">Name of program making the change</param>
        /// <param name="userName">Name of user making the change</param>
        /// <param name="command">Connection related properties</param>
        /// <returns>Operation result of processing of integration message type</returns>
        public OperationResultCollection Process(IntegrationMessageTypeCollection integrationMessageTypes, String programName, String userName, DBCommandProperties command)
        {
            if (Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.StartTraceActivity(_processMethodName, MDMTraceSource.Integration, false);
            }

            SqlDataReader reader = null;
            OperationResultCollection result = new OperationResultCollection();
            const String storedProcedureName = "usp_IntegrationManager_IntegrationMessageType_Process";

            SqlParametersGenerator generator = new SqlParametersGenerator("IntegrationManager_SqlParameters");
            SqlParameter[] parameters = generator.GetParameters("IntegrationManager_IntegrationMessageType_Process_ParametersArray");
            SqlMetaData[] sqlMetadata = generator.GetTableValueMetadata("IntegrationManager_IntegrationMessageType_Process_ParametersArray", parameters[0].ParameterName);

            List<SqlDataRecord> integrationMessageTypeRecords = GetSqlRecords(integrationMessageTypes, sqlMetadata);

            using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions() { IsolationLevel = Constants.DEFAULT_ISOLATION_LEVEL }))
            {
                reader = null;
                try
                {

                    parameters[0].Value = integrationMessageTypeRecords;
                    parameters[1].Value = programName;
                    parameters[2].Value = userName;

                    reader = ExecuteProcedureReader(command.ConnectionString, parameters, storedProcedureName);

                    //Update integration message type object with Actual Id in case of create
                    UpdateIntegrationMessageTypes(reader, integrationMessageTypes, result);

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
        */

        /// <summary>
        /// Get IntegrationMessageTypes
        /// </summary>
        /// <param name="command">Connection related properties</param>
        /// <returns>Qualifying items</returns>
        public IntegrationMessageTypeCollection Get(Int16 messageTypeId, String connectorShortName, DBCommandProperties command)
        {
            if (Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.StartTraceActivity(_getMethodName, MDMTraceSource.Integration, false);
            }

            SqlDataReader reader = null;
            SqlParameter[] parameters;
            IntegrationMessageTypeCollection integrationMessageTypes = new IntegrationMessageTypeCollection();
            String storedProcedureName = "usp_IntegrationManager_IntegrationMessageType_Get";

            try
            {
                SqlParametersGenerator generator = new SqlParametersGenerator("IntegrationManager_SqlParameters");

                parameters = generator.GetParameters("IntegrationManager_IntegrationMessageType_Get_ParametersArray");

                parameters[0].Value = messageTypeId;
                parameters[1].Value = connectorShortName;

                reader = ExecuteProcedureReader(command.ConnectionString, parameters, storedProcedureName);

                integrationMessageTypes = ReadIntegrationMessageTypes(reader);
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
            return integrationMessageTypes;
        }

        #region Private method

        // Uncomment this code for Process API

        /*
        private void UpdateIntegrationMessageTypes(SqlDataReader reader, IntegrationMessageTypeCollection integrationMessageTypes, OperationResultCollection result)
        {
            while (reader.Read())
            {
                IntegrationMessageType message = null;
                Int64 id = -1;
                Boolean hasError = false;
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
                    message = integrationMessageTypes.Where(log => log.ReferenceId == referenceId).FirstOrDefault();
                    message.Id = id;
                }

                OperationResult or = new OperationResult();
                or.ReferenceId = referenceId;
                //or.Id = id; //TODO :: need to change OR.Id to Int64

                if (!String.IsNullOrWhiteSpace(errorMessage) && errorMessage.Trim().Length > 0)
                {
                    or.AddOperationResult(String.Empty, errorMessage, OperationResultType.Error);
                }

                result.Add(or);
            }
        }

        private List<SqlDataRecord> GetSqlRecords(IntegrationMessageTypeCollection integrationMessageTypeCollection, SqlMetaData[] sqlMetadata)
        {
            List<SqlDataRecord> integrationActivityLogSqlRecords = new List<SqlDataRecord>();
            Int32 counter = 0;

            foreach (IntegrationMessageType integrationMessageType in integrationMessageTypeCollection)
            {
                SqlDataRecord integrationMessageTypeRecord = new SqlDataRecord(sqlMetadata);
                if (String.IsNullOrWhiteSpace(integrationMessageType.ReferenceId))
                {
                    integrationMessageType.ReferenceId = ( counter-- ).ToString();
                }

                integrationMessageTypeRecord.SetValue(0, integrationMessageType.ReferenceId);
                integrationMessageTypeRecord.SetValue(1, integrationMessageType.Id);
                integrationMessageTypeRecord.SetValue(2, integrationMessageType.MDMObjectTypeId);
                integrationMessageTypeRecord.SetValue(3, integrationMessageType.ToXml());
                integrationMessageTypeRecord.SetValue(4, integrationMessageType.Action.ToString());

                integrationActivityLogSqlRecords.Add(integrationMessageTypeRecord);
            }

            return integrationActivityLogSqlRecords;
        }

        */
        private IntegrationMessageTypeCollection ReadIntegrationMessageTypes(SqlDataReader reader)
        {
            IntegrationMessageTypeCollection messageTypeCollection = new IntegrationMessageTypeCollection();

            if (reader != null)
            {
                while (reader.Read())
                {
                    #region Declaration

                    Int16 id = -1;
                    String msgTypeShortName = String.Empty;
                    String msgLongName = String.Empty;
                    Int16 connectorId = -1;
                    Int32 weightage = -1;

                    #endregion Declaration

                    #region Read properties

                    if (reader["Id"] != null)
                        id = ValueTypeHelper.Int16TryParse(reader["Id"].ToString(), -1);

                    if (reader["Name"] != null)
                        msgTypeShortName = reader["Name"].ToString();

                    if (reader["LongName"] != null)
                        msgLongName = reader["LongName"].ToString();

                    if (reader["ConnectorId"] != null)
                        connectorId = ValueTypeHelper.Int16TryParse(reader["ConnectorId"].ToString(),-1);

                    if (reader["Weightage"] != null)
                        weightage = ValueTypeHelper.Int32TryParse(reader["Weightage"].ToString(), -1);


                    #endregion Read properties

                    #region Initialize object

                    IntegrationMessageType messageType = new IntegrationMessageType();
                    messageType.Id = id;
                    messageType.Name = msgTypeShortName;
                    messageType.LongName = msgLongName;
                    messageType.ConnectorId = connectorId;
                    messageType.Weightage = weightage;

                    #endregion Initialize object

                    messageTypeCollection.Add(messageType);
                }
            }
            return messageTypeCollection;
        }

        #endregion Private method

    }
}
