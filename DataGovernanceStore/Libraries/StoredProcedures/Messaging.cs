
using System;
using System.Xml;
using System.Data;
using System.Text;
using System.Data.SqlTypes;
using System.Configuration;
using MDM.Core;
using MDM.Utility;

namespace Riversand.StoredProcedures
{
	/// <summary>
	/// 
	/// </summary>
	public class Messaging
	{
		/// <summary>
		/// Private constructor
		/// </summary>		
		private Messaging()
		{
		}
		
		
		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetMessageDetails(SqlInt32 intMessageID)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return Messaging.GetMessageDetails(intMessageID, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetMessageDetails(SqlInt32 intMessageID, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return Messaging.GetMessageDetails(intMessageID, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetMessageDetails(SqlInt32 intMessageID, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlMessaging.GetMessageDetails(intMessageID, connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Messaging.GetMessageDetails for this provider: "+providerName);
					throw new ApplicationException("No implementation of Messaging.GetMessageDetails for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetIntegratedInbox(SqlString nvchrUsername)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return Messaging.GetIntegratedInbox(nvchrUsername, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetIntegratedInbox(SqlString nvchrUsername, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return Messaging.GetIntegratedInbox(nvchrUsername, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetIntegratedInbox(SqlString nvchrUsername, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlMessaging.GetIntegratedInbox(nvchrUsername, connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Messaging.GetIntegratedInbox for this provider: "+providerName);
					throw new ApplicationException("No implementation of Messaging.GetIntegratedInbox for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static void DeleteMessages(SqlString strTableName, SqlString strPkMessages)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			Messaging.DeleteMessages(strTableName, strPkMessages, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static void DeleteMessages(SqlString strTableName, SqlString strPkMessages, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			Messaging.DeleteMessages(strTableName, strPkMessages, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static void DeleteMessages(SqlString strTableName, SqlString strPkMessages, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					SqlMessaging.DeleteMessages(strTableName, strPkMessages, connection, transaction);

					break;

				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Messaging.DeleteMessages for this provider: "+providerName);
					throw new ApplicationException("No implementation of Messaging.DeleteMessages for this provider: "+providerName);
			}
		}

	}
}		
