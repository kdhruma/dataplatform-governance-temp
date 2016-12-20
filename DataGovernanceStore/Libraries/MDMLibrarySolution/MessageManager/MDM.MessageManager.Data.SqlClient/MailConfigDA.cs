using System;
using System.Data.SqlClient;

namespace MDM.MessageManager.Data.SqlClient
{
	using BusinessObjects;
	using Core;
	using Utility;

	public class MailConfigDA : SqlClientDataAccessBase
	{
		/// <summary>
		/// Get configured mail config for given application and module
		/// </summary>
		/// <param name="application">Application for which mail config is to be fetched.</param>
		/// <param name="module">Module for which mail config is to be fetched.</param>
		/// <param name="command">Command object with properties like mail config, command time out etc. populated</param>
		/// <returns>mail config</returns>
		public MailConfig Get(MDMCenterApplication application, MDMCenterModules module, DBCommandProperties command)
		{
			MailConfig mailConfig = null;
			SqlDataReader reader = null;
			try
			{
				SqlParametersGenerator generator = new SqlParametersGenerator("MessageManager_SqlParameters");

				SqlParameter[] parameters = generator.GetParameters("MessageManager_MailConfig_Get_ParametersArray");

				parameters[0].Value = application.ToString();
				parameters[1].Value = module.ToString();

				const string storedProcedureName = "usp_MessageManager_MailConfig_Get";

				reader = ExecuteProcedureReader(command.ConnectionString, parameters, storedProcedureName);

                if (reader != null)
                {
                    Int32 id = 0;
                    String name = String.Empty;
                    String longName = String.Empty;
                    String from = String.Empty;
                    String host = String.Empty;
                    Int32 port = 0;
                    String userName = String.Empty;
                    String password = String.Empty;
                    Boolean enableSSL = false;

                    while (reader.Read())
                    {
                        if (reader["PK_MailConfig"] != null)
                        {
                            id = ValueTypeHelper.Int32TryParse(reader["PK_MailConfig"].ToString(), 0);
                        }
                        if (reader["ShortName"] != null)
                        {
                            name = reader["ShortName"].ToString();
                        }
                        if (reader["LongName"] != null)
                        {
                            longName = reader["LongName"].ToString();
                        }
                        if (reader["From"] != null)
                        {
                            from = reader["From"].ToString();
                        }
                        if (reader["Host"] != null)
                        {
                            host = reader["Host"].ToString();
                        }
                        if (reader["Port"] != null)
                        {
                            port = ValueTypeHelper.Int32TryParse(reader["Port"].ToString(), 0);
                        }
                        if (reader["UserName"] != null)
                        {
                            userName = reader["UserName"].ToString();
                        }
                        if (reader["Password"] != null)
                        {
                            password = reader["Password"].ToString();
                        }
                        if (reader["EnableSSL"] != null)
                        {
                            enableSSL = ValueTypeHelper.BooleanTryParse(reader["EnableSSL"].ToString(), false);
                        }

                        mailConfig = new MailConfig(id, name, longName, application, module, from, host, port, userName, password, enableSSL);
                    }
                }
			}
			finally
			{
				if (reader != null)
				{
					reader.Close();
				}
			}

			return mailConfig;
		}
	}
}
