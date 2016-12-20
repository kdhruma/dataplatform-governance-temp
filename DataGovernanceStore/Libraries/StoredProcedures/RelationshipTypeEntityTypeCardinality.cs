
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
	public class RelationshipTypeEntityTypeCardinality
	{
		/// <summary>
		/// Private constructor
		/// </summary>		
		private RelationshipTypeEntityTypeCardinality()
		{
		}
		
		
		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetRelationshipTypeEntityTypeCardinality(SqlInt32 FK_NodeType_From, SqlInt32 FK_RelationshipType)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return RelationshipTypeEntityTypeCardinality.GetRelationshipTypeEntityTypeCardinality(FK_NodeType_From, FK_RelationshipType, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetRelationshipTypeEntityTypeCardinality(SqlInt32 FK_NodeType_From, SqlInt32 FK_RelationshipType, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return RelationshipTypeEntityTypeCardinality.GetRelationshipTypeEntityTypeCardinality(FK_NodeType_From, FK_RelationshipType, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetRelationshipTypeEntityTypeCardinality(SqlInt32 FK_NodeType_From, SqlInt32 FK_RelationshipType, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlRelationshipTypeEntityTypeCardinality.GetRelationshipTypeEntityTypeCardinality(FK_NodeType_From, FK_RelationshipType, connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of RelationshipTypeEntityTypeCardinality.GetRelationshipTypeEntityTypeCardinality for this provider: "+providerName);
					throw new ApplicationException("No implementation of RelationshipTypeEntityTypeCardinality.GetRelationshipTypeEntityTypeCardinality for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static void ProcessRelationshipTypeEntityTypeCardinality(SqlString txtXML, SqlString UserName, SqlString ProgramName, out SqlInt32 RETURN_VALUE)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			RelationshipTypeEntityTypeCardinality.ProcessRelationshipTypeEntityTypeCardinality(txtXML, UserName, ProgramName, out RETURN_VALUE, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static void ProcessRelationshipTypeEntityTypeCardinality(SqlString txtXML, SqlString UserName, SqlString ProgramName, out SqlInt32 RETURN_VALUE, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			RelationshipTypeEntityTypeCardinality.ProcessRelationshipTypeEntityTypeCardinality(txtXML, UserName, ProgramName, out RETURN_VALUE, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static void ProcessRelationshipTypeEntityTypeCardinality(SqlString txtXML, SqlString UserName, SqlString ProgramName, out SqlInt32 RETURN_VALUE, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					SqlRelationshipTypeEntityTypeCardinality.ProcessRelationshipTypeEntityTypeCardinality(txtXML, UserName, ProgramName, out RETURN_VALUE, connection, transaction);

					break;

				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of RelationshipTypeEntityTypeCardinality.ProcessRelationshipTypeEntityTypeCardinality for this provider: "+providerName);
					throw new ApplicationException("No implementation of RelationshipTypeEntityTypeCardinality.ProcessRelationshipTypeEntityTypeCardinality for this provider: "+providerName);
			}
		}

	}
}		
