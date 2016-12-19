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
    /// Specifies ConnectorProfile data access
    /// </summary>
    public class ConnectorProfileDA : SqlClientDataAccessBase
    {
        #region Fields

        private const String _processMethodName = "MDM.IntegrationManager.Data.ConnectorProfileDA.Process";
        private const String _getMethodName = "MDM.IntegrationManager.Data.ConnectorProfileDA.Get";

        #endregion Fields

        /// <summary>
        /// Process ConnectorProfile
        /// </summary>
        /// <param name="connectorProfile">ConnectorProfiles to process</param>
        /// <param name="programName">Name of program making the change</param>
        /// <param name="userName">Name of user making the change</param>
        /// <param name="operationResult"></param>
        /// <param name="command">Connection related properties</param>
        /// <returns>Operation result of processing of ConnectorProfile</returns>
        public void Process(ConnectorProfile connectorProfile, String programName, String userName, OperationResult operationResult, DBCommandProperties command)
        {
            if (Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.StartTraceActivity(_processMethodName, MDMTraceSource.Integration, false);
            }

            const String storedProcedureName = "usp_IntegrationManager_ConnectorProfile_Process";

            SqlParametersGenerator generator = new SqlParametersGenerator("IntegrationManager_SqlParameters");
            SqlParameter[] parameters = generator.GetParameters("IntegrationManager_ConnectorProfile_Process_ParametersArray");

            using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = Constants.DEFAULT_ISOLATION_LEVEL }))
            {
                SqlDataReader reader = null;
                try
                {
                    parameters[0].Value = connectorProfile.Name;
                    parameters[1].Value = connectorProfile.LongName;
                    parameters[2].Value = connectorProfile.ToXml();
                    parameters[3].Value = connectorProfile.Weightage;
                    parameters[4].Value = connectorProfile.Enabled;
                    parameters[5].Value = false;
                    parameters[6].Value = programName;
                    parameters[7].Value = userName;
                    parameters[8].Value = connectorProfile.Action;

                    reader = ExecuteProcedureReader(command.ConnectionString, parameters, storedProcedureName);

                    //Update ConnectorProfile object with Actual Id in case of create
                    UpdateConnectorProfile(reader, connectorProfile, operationResult);

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
        /// Get ConnectorProfiles
        /// </summary>
        /// <param name="command">Connection related properties</param>
        /// <returns>Qualifying items</returns>
        public ConnectorProfileCollection Get(Int16 connectorId, String connectorShortName, DBCommandProperties command)
        {
            if (Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.StartTraceActivity(_getMethodName, MDMTraceSource.Integration, false);
            }

            SqlDataReader reader = null;
            SqlParameter[] parameters;
            ConnectorProfileCollection connectorProfiles = new ConnectorProfileCollection();
            String storedProcedureName = "usp_IntegrationManager_ConnectorProfile_Get";

            try
            {
                SqlParametersGenerator generator = new SqlParametersGenerator("IntegrationManager_SqlParameters");

                parameters = generator.GetParameters("IntegrationManager_ConnectorProfile_Get_ParametersArray");

                parameters[0].Value = connectorId;
                parameters[1].Value = connectorShortName;

                reader = ExecuteProcedureReader(command.ConnectionString, parameters, storedProcedureName);

                connectorProfiles = ReadConnectorProfiles(reader);
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
            return connectorProfiles;
        }

        #region Private method

        private void UpdateConnectorProfile(SqlDataReader reader, ConnectorProfile connectorProfile, OperationResult operationResult)
        {
            while (reader.Read())
            {
                Int16 id = -1;
                //Boolean hasError = false;
                //String errorMessage = String.Empty;
                //String referenceId = String.Empty;

                #region Read values

                if (reader["connectorId"] != null)
                {
                    id = ValueTypeHelper.Int16TryParse(reader["connectorId"].ToString(), -1);
                }

                //if (reader["HasError"] != null)
                //{
                //    hasError = ValueTypeHelper.BooleanTryParse(reader["HasError"].ToString(), false);
                //}

                //if (reader["ErrorMessage"] != null)
                //{
                //    errorMessage = reader["ErrorMessage"].ToString();
                //}

                //if (reader["ReferenceId"] != null)
                //{
                //    referenceId = reader["ReferenceId"].ToString();
                //}

                #endregion Read values

                connectorProfile.Id = id;
                operationResult.Id = id;

                //if (hasError)
                //{
                //    operationResult.AddOperationResult(String.Empty, errorMessage, OperationResultType.Error);
                //}
            }
        }

        private ConnectorProfileCollection ReadConnectorProfiles(SqlDataReader reader)
        {
            ConnectorProfileCollection connectorProfileCollection = new ConnectorProfileCollection();

            if (reader != null)
            {
                while (reader.Read())
                {
                    #region Declaration

                    Int16 id = -1;
                    String connectorShortName = String.Empty;
                    String connectorLongName = String.Empty;
                    String profileXml = String.Empty;
                    Int32 weightage = -1;
                    Boolean enabled = true;

                    #endregion Declaration

                    #region Read properties

                    if (reader["PK_Connector"] != null)
                        id = ValueTypeHelper.Int16TryParse(reader["PK_Connector"].ToString(), -1);

                    if (reader["ShortName"] != null)
                        connectorShortName = reader["ShortName"].ToString();

                    if (reader["LongName"] != null)
                        connectorLongName = reader["LongName"].ToString();

                    if (reader["ConnectorProfile"] != null)
                        profileXml = reader["ConnectorProfile"].ToString();

                    if (reader["Weightage"] != null)
                        weightage = ValueTypeHelper.Int32TryParse(reader["Weightage"].ToString(), -1);

                    if (reader["Enabled"] != null)
                        enabled = ValueTypeHelper.BooleanTryParse(reader["Enabled"].ToString(), true);

                    #endregion Read properties

                    #region Initialize object

                    ConnectorProfile profile = new ConnectorProfile(profileXml);
                    profile.Id = id;
                    profile.Name = connectorShortName;
                    profile.LongName = connectorLongName;
                    profile.Enabled = enabled;
                    profile.Weightage = weightage;
                    

                    #endregion Initialize object

                    connectorProfileCollection.Add(profile);
                }
            }
            return connectorProfileCollection;
        }

        #endregion Private method
    }
}
