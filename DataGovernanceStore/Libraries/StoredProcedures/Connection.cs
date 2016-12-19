using System;
using System.Data;
using System.Configuration;
using MDM.Core;
using MDM.Utility;
using System.Data.SqlClient;
//using Oracle.DataAccess.Client;

namespace Riversand.StoredProcedures
{
	/// <summary>
	/// Summary description for Connection.
	/// </summary>
	public class DbShell
	{
		public static IDbConnection GetConnection()
		{
			string providerName = ConfigurationManager.AppSettings["databaseProvider"];
			switch (providerName)
			{
				case "SqlProvider":
				{
					return new SqlConnection(MDM.Utility.AppConfigurationHelper.ConnectionString);
				}
//				case "OracleProvider":
//				{
//					return new OracleConnection(ConfigurationManager.AppSettings["ConnectionString"]);
//				}
				default:
					throw new ApplicationException("provider type not supported");
			}
		}
	}
}
