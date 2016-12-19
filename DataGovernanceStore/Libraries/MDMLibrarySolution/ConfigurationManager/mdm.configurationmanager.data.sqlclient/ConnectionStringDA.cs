using System;
using System.Data.SqlClient;
using System.Text;

namespace MDM.ConfigurationManager.Data
{
    using MDM.BusinessObjects;
    using MDM.Core;
    using MDM.Utility;

    /// <summary>
    /// Class for connection string DA methods.
    /// </summary>
    public class ConnectionStringDA : SqlClientDataAccessBase
    {
        /// <summary>
        /// Get configured connection string for given module and action
        /// </summary>
        /// <param name="application">Application for which connection string is to be fetched.</param>
        /// <param name="module">Module for which connection string is to be fetched.</param>
        /// <param name="action">Action for which connection string is to be fetched.</param>
        /// <param name="command">Command object with properties like connection string, command time out etc. populated</param>
        /// <returns>Connection string</returns>
        public ConnectionProperties Get(MDMCenterApplication application, MDMCenterModules module, MDMCenterModuleAction action, DBCommandProperties command)
        {
            ConnectionProperties connectionProp = null;
            SqlParameter[] parameters;
            String storedProcedureName = String.Empty;
            SqlDataReader reader = null;
            try
            {
                SqlParametersGenerator generator = new SqlParametersGenerator("ConfigurationManager_SqlParameters");

                parameters = generator.GetParameters("ConfigurationManager_ConnectionString_Get_ParametersArray");

                parameters[0].Value = application.ToString();
                parameters[1].Value = module.ToString();
                parameters[2].Value = action.ToString();


                storedProcedureName = "usp_ConfigurationManager_ConnectionString_Get";

                reader = ExecuteProcedureReader(command.ConnectionString, parameters, storedProcedureName);

                if ( reader != null )
                {
                    String serverId = String.Empty;
                    String userName = String.Empty;
                    String password = String.Empty;
                    String dbName = String.Empty;
                    String connectionAttributes = String.Empty;

                    while ( reader.Read() )
                    {
                        if ( reader["ServerId"] != null )
                        {
                            serverId = reader["ServerId"].ToString();
                        }
                        if ( reader["UserName"] != null )
                        {
                            userName = reader["UserName"].ToString();
                        }
                        if ( reader["Password"] != null )
                        {
                            password = reader["Password"].ToString();
                        }
                        if ( reader["DatabaseName"] != null )
                        {
                            dbName = reader["DatabaseName"].ToString();
                        }
                        if ( reader["ConnectionAttributes"] != null )
                        {
                            connectionAttributes = reader["ConnectionAttributes"].ToString();
                        }
                    }

                    connectionProp = new ConnectionProperties(serverId, userName, password, dbName, connectionAttributes);
                }
            }
            finally
            {
                if ( reader != null )
                {
                    reader.Close();
                }
            }

            return connectionProp;
        }
    }
}
