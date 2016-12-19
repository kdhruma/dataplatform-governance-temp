
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
	public class Documents
	{
		/// <summary>
		/// Private constructor
		/// </summary>		
		private Documents()
		{
		}
		
		
		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetDocumentAssociations(SqlString DocumentNames, SqlString DocumentBasePath, SqlInt32 OrgID, SqlString AttributesList, SqlInt32 FK_Locale)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return Documents.GetDocumentAssociations(DocumentNames, DocumentBasePath, OrgID, AttributesList, FK_Locale, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetDocumentAssociations(SqlString DocumentNames, SqlString DocumentBasePath, SqlInt32 OrgID, SqlString AttributesList, SqlInt32 FK_Locale, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return Documents.GetDocumentAssociations(DocumentNames, DocumentBasePath, OrgID, AttributesList, FK_Locale, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetDocumentAssociations(SqlString DocumentNames, SqlString DocumentBasePath, SqlInt32 OrgID, SqlString AttributesList, SqlInt32 FK_Locale, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlDocuments.GetDocumentAssociations(DocumentNames, DocumentBasePath, OrgID, AttributesList, FK_Locale, connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Documents.GetDocumentAssociations for this provider: "+providerName);
					throw new ApplicationException("No implementation of Documents.GetDocumentAssociations for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static void ProcessDocument(SqlInt32 OrgID, SqlXml txtXML, SqlString Action, SqlInt32 FK_Locale, SqlString userProgram, SqlString loginUser, out SqlInt32 RETURN_VALUE)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			Documents.ProcessDocument(OrgID, txtXML, Action, FK_Locale, userProgram, loginUser, out RETURN_VALUE, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static void ProcessDocument(SqlInt32 OrgID, SqlXml txtXML, SqlString Action, SqlInt32 FK_Locale, SqlString userProgram, SqlString loginUser, out SqlInt32 RETURN_VALUE, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			Documents.ProcessDocument(OrgID, txtXML, Action, FK_Locale, userProgram, loginUser, out RETURN_VALUE, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static void ProcessDocument(SqlInt32 OrgID, SqlXml txtXML, SqlString Action, SqlInt32 FK_Locale, SqlString userProgram, SqlString loginUser, out SqlInt32 RETURN_VALUE, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					SqlDocuments.ProcessDocument(OrgID, txtXML, Action, FK_Locale, userProgram, loginUser, out RETURN_VALUE, connection, transaction);

					break;

				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Documents.ProcessDocument for this provider: "+providerName);
					throw new ApplicationException("No implementation of Documents.ProcessDocument for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetDocumentCNodes(SqlString DocumentNames, SqlString DocumentBasePath, SqlInt32 OrgID)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return Documents.GetDocumentCNodes(DocumentNames, DocumentBasePath, OrgID, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetDocumentCNodes(SqlString DocumentNames, SqlString DocumentBasePath, SqlInt32 OrgID, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return Documents.GetDocumentCNodes(DocumentNames, DocumentBasePath, OrgID, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetDocumentCNodes(SqlString DocumentNames, SqlString DocumentBasePath, SqlInt32 OrgID, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlDocuments.GetDocumentCNodes(DocumentNames, DocumentBasePath, OrgID, connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Documents.GetDocumentCNodes for this provider: "+providerName);
					throw new ApplicationException("No implementation of Documents.GetDocumentCNodes for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetDocumentEnabledOrganizations()
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return Documents.GetDocumentEnabledOrganizations(connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetDocumentEnabledOrganizations(IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return Documents.GetDocumentEnabledOrganizations(connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetDocumentEnabledOrganizations(IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlDocuments.GetDocumentEnabledOrganizations(connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Documents.GetDocumentEnabledOrganizations for this provider: "+providerName);
					throw new ApplicationException("No implementation of Documents.GetDocumentEnabledOrganizations for this provider: "+providerName);
			}
		}

	}
}		
