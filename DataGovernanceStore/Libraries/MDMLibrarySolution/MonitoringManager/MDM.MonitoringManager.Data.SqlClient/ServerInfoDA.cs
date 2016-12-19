using System;
using System.Data.SqlClient;

namespace MDM.MonitoringManager.Data.SqlClient
{
    using MDM.BusinessObjects;
    using MDM.Core;
    using MDM.Utility;

    /// <summary>
    /// Data access for server info
    /// </summary>
    public class ServerInfoDA : SqlClientDataAccessBase
    {
        #region Public Methods

        /// <summary>
        /// Gets server info collection based on server name
        /// </summary>
        /// <param name="ServerName">name of the server for which info is requested</param>
        /// <param name="command">command object which contains all the db info like connection string</param>
        /// <returns>ServerInfoCollection object with servers info</returns>
        public ServerInfoCollection GetServerInfoColletion(String ServerName, DBCommandProperties command)
        {
            if (Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.StartTraceActivity("MonitoringManager.ServerInfoDA.GetServerInfo", false);
            }

            ServerInfoCollection serverInfos = null;
            SqlDataReader reader = null;
            SqlParametersGenerator generator = new SqlParametersGenerator("MonitorManager_SqlParameters");
            SqlParameter[] parameters = generator.GetParameters("MonitoringManager_ServerInfo_Get_ParameterArray");

            try
            {
                if (String.IsNullOrEmpty(ServerName))
                {
                    parameters[0].Value = DBNull.Value;
                }
                else
                {
                    parameters[0].Value = ServerName;                    
                }

                String storedProcedureName = "usp_MonitoringManager_Monitor_Server_Get";

                reader = ExecuteProcedureReader(command.ConnectionString, parameters, storedProcedureName);

                #region Reading reader data

                serverInfos = new ServerInfoCollection();

                if (reader != null)
                {
                    while (reader.Read())
                    {
                        ServerInfo serverInfo = new ServerInfo();

                        if (reader["Id"] != null)
                        {
                            serverInfo.Id = ValueTypeHelper.Int16TryParse(reader["Id"].ToString(), -1);
                        }

                        if (reader["Name"] != null && !String.IsNullOrWhiteSpace(reader["Name"].ToString()))
                        {
                            serverInfo.Name = reader["Name"].ToString();
                        }

                        if (reader["LongName"] != null && !String.IsNullOrWhiteSpace(reader["LongName"].ToString()))
                        {
                            serverInfo.LongName = reader["LongName"].ToString();
                        }

                        serverInfos.Add(serverInfo);
                    }
                }

                #endregion
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }

                if (Constants.TRACING_ENABLED)
                {
                    MDMTraceHelper.StopTraceActivity("MonitoringManager.ServerInfo.GetServerInfo");
                }
            }

            return serverInfos;
        }

        #endregion
    }
}
