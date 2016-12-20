
using System;
using System.Xml;
using System.Data;
using System.Text;
using System.Data.SqlTypes;
using System.Configuration;
using MDM.Utility;

namespace Riversand.StoredProcedures
{
	/// <summary>
	/// 
	/// </summary>
	public class Scripts
	{
		/// <summary>
		/// Private constructor
		/// </summary>		
		private Scripts()
		{
		}
		
		
		/// <summary>
        /// 
        /// </summary>
		public static string GetUserScriptNames(SqlString nvchrDomain)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return Scripts.GetUserScriptNames(nvchrDomain, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static string GetUserScriptNames(SqlString nvchrDomain, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return Scripts.GetUserScriptNames(nvchrDomain, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static string GetUserScriptNames(SqlString nvchrDomain, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlScripts.GetUserScriptNames(nvchrDomain, connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Scripts.GetUserScriptNames for this provider: "+providerName);
					throw new ApplicationException("No implementation of Scripts.GetUserScriptNames for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static string GetUserScripts(SqlString nvchrName, SqlString nvchrDomain)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return Scripts.GetUserScripts(nvchrName, nvchrDomain, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static string GetUserScripts(SqlString nvchrName, SqlString nvchrDomain, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return Scripts.GetUserScripts(nvchrName, nvchrDomain, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static string GetUserScripts(SqlString nvchrName, SqlString nvchrDomain, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlScripts.GetUserScripts(nvchrName, nvchrDomain, connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Scripts.GetUserScripts for this provider: "+providerName);
					throw new ApplicationException("No implementation of Scripts.GetUserScripts for this provider: "+providerName);
			}
		}

	}
}		
