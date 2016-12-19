
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
	public class ErrorHandling
	{
		/// <summary>
		/// Private constructor
		/// </summary>		
		private ErrorHandling()
		{
		}
		
		
		/// <summary>
        /// 
        /// </summary>
		public static void SetErrorLog(SqlString FileName, SqlString MessageID, SqlString Status, SqlString ObjectType, SqlInt32 ItemNumber, SqlString ExternalID, SqlInt32 InternalID, SqlString Description, SqlInt32 ErrorCode, SqlString ItemType, SqlString ItemName, SqlString ItemParentName)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			ErrorHandling.SetErrorLog(FileName, MessageID, Status, ObjectType, ItemNumber, ExternalID, InternalID, Description, ErrorCode, ItemType, ItemName, ItemParentName, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static void SetErrorLog(SqlString FileName, SqlString MessageID, SqlString Status, SqlString ObjectType, SqlInt32 ItemNumber, SqlString ExternalID, SqlInt32 InternalID, SqlString Description, SqlInt32 ErrorCode, SqlString ItemType, SqlString ItemName, SqlString ItemParentName, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			ErrorHandling.SetErrorLog(FileName, MessageID, Status, ObjectType, ItemNumber, ExternalID, InternalID, Description, ErrorCode, ItemType, ItemName, ItemParentName, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static void SetErrorLog(SqlString FileName, SqlString MessageID, SqlString Status, SqlString ObjectType, SqlInt32 ItemNumber, SqlString ExternalID, SqlInt32 InternalID, SqlString Description, SqlInt32 ErrorCode, SqlString ItemType, SqlString ItemName, SqlString ItemParentName, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					SqlErrorHandling.SetErrorLog(FileName, MessageID, Status, ObjectType, ItemNumber, ExternalID, InternalID, Description, ErrorCode, ItemType, ItemName, ItemParentName, connection, transaction);

					break;

				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of ErrorHandling.SetErrorLog for this provider: "+providerName);
					throw new ApplicationException("No implementation of ErrorHandling.SetErrorLog for this provider: "+providerName);
			}
		}

	}
}		
