
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
	public class Administration
	{
		/// <summary>
		/// Private constructor
		/// </summary>		
		private Administration()
		{
		}
		
		
		/// <summary>
        /// 
        /// </summary>
		public static DataSet getListTypes()
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return Administration.getListTypes(connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataSet getListTypes(IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return Administration.getListTypes(connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataSet getListTypes(IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlAdministration.getListTypes(connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error, "No implementation of Administration.getListTypes for this provider: "+providerName);
					throw new ApplicationException("No implementation of Administration.getListTypes for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static void updateListType(SqlInt32 PK_ListType, SqlString ShortName, SqlString LongName, SqlString DeleteFlag, SqlString UserId, SqlString ProgramName, SqlBoolean ForInsert, out SqlInt32 RETURN_VALUE)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			Administration.updateListType(PK_ListType, ShortName, LongName, DeleteFlag, UserId, ProgramName, ForInsert, out RETURN_VALUE, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static void updateListType(SqlInt32 PK_ListType, SqlString ShortName, SqlString LongName, SqlString DeleteFlag, SqlString UserId, SqlString ProgramName, SqlBoolean ForInsert, out SqlInt32 RETURN_VALUE, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			Administration.updateListType(PK_ListType, ShortName, LongName, DeleteFlag, UserId, ProgramName, ForInsert, out RETURN_VALUE, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static void updateListType(SqlInt32 PK_ListType, SqlString ShortName, SqlString LongName, SqlString DeleteFlag, SqlString UserId, SqlString ProgramName, SqlBoolean ForInsert, out SqlInt32 RETURN_VALUE, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					SqlAdministration.updateListType(PK_ListType, ShortName, LongName, DeleteFlag, UserId, ProgramName, ForInsert, out RETURN_VALUE, connection, transaction);

					break;

				default:
                    MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error, "No implementation of Administration.updateListType for this provider: " + providerName);
					throw new ApplicationException("No implementation of Administration.updateListType for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataSet getAllBreakerSets()
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return Administration.getAllBreakerSets(connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataSet getAllBreakerSets(IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return Administration.getAllBreakerSets(connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataSet getAllBreakerSets(IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlAdministration.getAllBreakerSets(connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Administration.getAllBreakerSets for this provider: "+providerName);
					throw new ApplicationException("No implementation of Administration.getAllBreakerSets for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataSet getAllLists()
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return Administration.getAllLists(connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataSet getAllLists(IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return Administration.getAllLists(connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataSet getAllLists(IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlAdministration.getAllLists(connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Administration.getAllLists for this provider: "+providerName);
					throw new ApplicationException("No implementation of Administration.getAllLists for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static void updateList(SqlInt32 PK_List, SqlString ShortName, SqlString LongName, SqlInt32 FK_Word_ListType, SqlInt32 FK_Word_BreakerSet, SqlString DeleteFlag, SqlString UserId, SqlString ProgramName, SqlBoolean ForInsert, out SqlInt32 RETURN_VALUE)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			Administration.updateList(PK_List, ShortName, LongName, FK_Word_ListType, FK_Word_BreakerSet, DeleteFlag, UserId, ProgramName, ForInsert, out RETURN_VALUE, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static void updateList(SqlInt32 PK_List, SqlString ShortName, SqlString LongName, SqlInt32 FK_Word_ListType, SqlInt32 FK_Word_BreakerSet, SqlString DeleteFlag, SqlString UserId, SqlString ProgramName, SqlBoolean ForInsert, out SqlInt32 RETURN_VALUE, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			Administration.updateList(PK_List, ShortName, LongName, FK_Word_ListType, FK_Word_BreakerSet, DeleteFlag, UserId, ProgramName, ForInsert, out RETURN_VALUE, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static void updateList(SqlInt32 PK_List, SqlString ShortName, SqlString LongName, SqlInt32 FK_Word_ListType, SqlInt32 FK_Word_BreakerSet, SqlString DeleteFlag, SqlString UserId, SqlString ProgramName, SqlBoolean ForInsert, out SqlInt32 RETURN_VALUE, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					SqlAdministration.updateList(PK_List, ShortName, LongName, FK_Word_ListType, FK_Word_BreakerSet, DeleteFlag, UserId, ProgramName, ForInsert, out RETURN_VALUE, connection, transaction);

					break;

				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Administration.updateList for this provider: "+providerName);
					throw new ApplicationException("No implementation of Administration.updateList for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static void processNodeTypeRelType(SqlString txtXml, SqlString vchrUserId, SqlString vchrProgramName, out SqlInt32 RETURN_VALUE)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			Administration.processNodeTypeRelType(txtXml, vchrUserId, vchrProgramName, out RETURN_VALUE, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static void processNodeTypeRelType(SqlString txtXml, SqlString vchrUserId, SqlString vchrProgramName, out SqlInt32 RETURN_VALUE, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			Administration.processNodeTypeRelType(txtXml, vchrUserId, vchrProgramName, out RETURN_VALUE, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static void processNodeTypeRelType(SqlString txtXml, SqlString vchrUserId, SqlString vchrProgramName, out SqlInt32 RETURN_VALUE, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					SqlAdministration.processNodeTypeRelType(txtXml, vchrUserId, vchrProgramName, out RETURN_VALUE, connection, transaction);

					break;

				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Administration.processNodeTypeRelType for this provider: "+providerName);
					throw new ApplicationException("No implementation of Administration.processNodeTypeRelType for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataSet getCatalogNodeTypeAttr(SqlInt32 CatalogId, SqlInt32 NodeTypeID, SqlInt32 localeId)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return Administration.getCatalogNodeTypeAttr(CatalogId, NodeTypeID, localeId, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataSet getCatalogNodeTypeAttr(SqlInt32 CatalogId, SqlInt32 NodeTypeID, SqlInt32 localeId, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return Administration.getCatalogNodeTypeAttr(CatalogId, NodeTypeID, localeId, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataSet getCatalogNodeTypeAttr(SqlInt32 CatalogId, SqlInt32 NodeTypeID, SqlInt32 localeId, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlAdministration.getCatalogNodeTypeAttr(CatalogId, NodeTypeID, localeId, connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Administration.getCatalogNodeTypeAttr for this provider: "+providerName);
					throw new ApplicationException("No implementation of Administration.getCatalogNodeTypeAttr for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static void setCatalogNodeTypeAttr(SqlString txtXML, SqlString vchrUserID, SqlString vchrProgramName, out SqlInt32 RETURN_VALUE)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			Administration.setCatalogNodeTypeAttr(txtXML, vchrUserID, vchrProgramName, out RETURN_VALUE, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static void setCatalogNodeTypeAttr(SqlString txtXML, SqlString vchrUserID, SqlString vchrProgramName, out SqlInt32 RETURN_VALUE, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			Administration.setCatalogNodeTypeAttr(txtXML, vchrUserID, vchrProgramName, out RETURN_VALUE, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static void setCatalogNodeTypeAttr(SqlString txtXML, SqlString vchrUserID, SqlString vchrProgramName, out SqlInt32 RETURN_VALUE, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					SqlAdministration.setCatalogNodeTypeAttr(txtXML, vchrUserID, vchrProgramName, out RETURN_VALUE, connection, transaction);

					break;

				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Administration.setCatalogNodeTypeAttr for this provider: "+providerName);
					throw new ApplicationException("No implementation of Administration.setCatalogNodeTypeAttr for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataSet getCatalogRelTypeAttr(SqlInt32 CatalogId, SqlInt32 RelationshipTypeID)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return Administration.getCatalogRelTypeAttr(CatalogId, RelationshipTypeID, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataSet getCatalogRelTypeAttr(SqlInt32 CatalogId, SqlInt32 RelationshipTypeID, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return Administration.getCatalogRelTypeAttr(CatalogId, RelationshipTypeID, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataSet getCatalogRelTypeAttr(SqlInt32 CatalogId, SqlInt32 RelationshipTypeID, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlAdministration.getCatalogRelTypeAttr(CatalogId, RelationshipTypeID, connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Administration.getCatalogRelTypeAttr for this provider: "+providerName);
					throw new ApplicationException("No implementation of Administration.getCatalogRelTypeAttr for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static void setCatalogRelTypeAttr(SqlString txtXML, SqlString vchrUserID, SqlString vchrProgramName, out SqlInt32 RETURN_VALUE)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			Administration.setCatalogRelTypeAttr(txtXML, vchrUserID, vchrProgramName, out RETURN_VALUE, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static void setCatalogRelTypeAttr(SqlString txtXML, SqlString vchrUserID, SqlString vchrProgramName, out SqlInt32 RETURN_VALUE, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			Administration.setCatalogRelTypeAttr(txtXML, vchrUserID, vchrProgramName, out RETURN_VALUE, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static void setCatalogRelTypeAttr(SqlString txtXML, SqlString vchrUserID, SqlString vchrProgramName, out SqlInt32 RETURN_VALUE, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					SqlAdministration.setCatalogRelTypeAttr(txtXML, vchrUserID, vchrProgramName, out RETURN_VALUE, connection, transaction);

					break;

				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Administration.setCatalogRelTypeAttr for this provider: "+providerName);
					throw new ApplicationException("No implementation of Administration.setCatalogRelTypeAttr for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static string GetRelTypesXML()
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return Administration.GetRelTypesXML(connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static string GetRelTypesXML(IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return Administration.GetRelTypesXML(connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static string GetRelTypesXML(IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlAdministration.GetRelTypesXML(connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Administration.GetRelTypesXML for this provider: "+providerName);
					throw new ApplicationException("No implementation of Administration.GetRelTypesXML for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static void UpdateRelationshipType(SqlInt32 PK_RelationshipType, SqlString ShortName, SqlString LongName, SqlString DeleteFlag, SqlString UserId, SqlString ProgramName, SqlBoolean ForInsert, out SqlInt32 RETURN_VALUE)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			Administration.UpdateRelationshipType(PK_RelationshipType, ShortName, LongName, DeleteFlag, UserId, ProgramName, ForInsert, out RETURN_VALUE, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static void UpdateRelationshipType(SqlInt32 PK_RelationshipType, SqlString ShortName, SqlString LongName, SqlString DeleteFlag, SqlString UserId, SqlString ProgramName, SqlBoolean ForInsert, out SqlInt32 RETURN_VALUE, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			Administration.UpdateRelationshipType(PK_RelationshipType, ShortName, LongName, DeleteFlag, UserId, ProgramName, ForInsert, out RETURN_VALUE, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static void UpdateRelationshipType(SqlInt32 PK_RelationshipType, SqlString ShortName, SqlString LongName, SqlString DeleteFlag, SqlString UserId, SqlString ProgramName, SqlBoolean ForInsert, out SqlInt32 RETURN_VALUE, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					SqlAdministration.UpdateRelationshipType(PK_RelationshipType, ShortName, LongName, DeleteFlag, UserId, ProgramName, ForInsert, out RETURN_VALUE, connection, transaction);

					break;

				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Administration.UpdateRelationshipType for this provider: "+providerName);
					throw new ApplicationException("No implementation of Administration.UpdateRelationshipType for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetAllRelationshipType()
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return Administration.GetAllRelationshipType(connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetAllRelationshipType(IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return Administration.GetAllRelationshipType(connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetAllRelationshipType(IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlAdministration.GetAllRelationshipType(connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Administration.GetAllRelationshipType for this provider: "+providerName);
					throw new ApplicationException("No implementation of Administration.GetAllRelationshipType for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static string GetInhPathExceptionTypes(SqlString UserLogin)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return Administration.GetInhPathExceptionTypes(UserLogin, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static string GetInhPathExceptionTypes(SqlString UserLogin, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return Administration.GetInhPathExceptionTypes(UserLogin, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static string GetInhPathExceptionTypes(SqlString UserLogin, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlAdministration.GetInhPathExceptionTypes(UserLogin, connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Administration.GetInhPathExceptionTypes for this provider: "+providerName);
					throw new ApplicationException("No implementation of Administration.GetInhPathExceptionTypes for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static string GetInhPathExceptions(SqlString UserLogin, SqlInt32 FK_InhPath_Exception)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return Administration.GetInhPathExceptions(UserLogin, FK_InhPath_Exception, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static string GetInhPathExceptions(SqlString UserLogin, SqlInt32 FK_InhPath_Exception, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return Administration.GetInhPathExceptions(UserLogin, FK_InhPath_Exception, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static string GetInhPathExceptions(SqlString UserLogin, SqlInt32 FK_InhPath_Exception, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlAdministration.GetInhPathExceptions(UserLogin, FK_InhPath_Exception, connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Administration.GetInhPathExceptions for this provider: "+providerName);
					throw new ApplicationException("No implementation of Administration.GetInhPathExceptions for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static string SetInhPathExceptions(SqlString vchrUserLogin, SqlString xmldata)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return Administration.SetInhPathExceptions(vchrUserLogin, xmldata, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static string SetInhPathExceptions(SqlString vchrUserLogin, SqlString xmldata, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return Administration.SetInhPathExceptions(vchrUserLogin, xmldata, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static string SetInhPathExceptions(SqlString vchrUserLogin, SqlString xmldata, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlAdministration.SetInhPathExceptions(vchrUserLogin, xmldata, connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Administration.SetInhPathExceptions for this provider: "+providerName);
					throw new ApplicationException("No implementation of Administration.SetInhPathExceptions for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetCatalogTechAttributesDT(SqlInt32 CatalogId)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return Administration.GetCatalogTechAttributesDT(CatalogId, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetCatalogTechAttributesDT(SqlInt32 CatalogId, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return Administration.GetCatalogTechAttributesDT(CatalogId, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetCatalogTechAttributesDT(SqlInt32 CatalogId, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlAdministration.GetCatalogTechAttributesDT(CatalogId, connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Administration.GetCatalogTechAttributesDT for this provider: "+providerName);
					throw new ApplicationException("No implementation of Administration.GetCatalogTechAttributesDT for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataSet getNodeTypeByRelTypeCatalog(SqlInt32 RelId, SqlInt32 CatalogId)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return Administration.getNodeTypeByRelTypeCatalog(RelId, CatalogId, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataSet getNodeTypeByRelTypeCatalog(SqlInt32 RelId, SqlInt32 CatalogId, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return Administration.getNodeTypeByRelTypeCatalog(RelId, CatalogId, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataSet getNodeTypeByRelTypeCatalog(SqlInt32 RelId, SqlInt32 CatalogId, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlAdministration.getNodeTypeByRelTypeCatalog(RelId, CatalogId, connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Administration.getNodeTypeByRelTypeCatalog for this provider: "+providerName);
					throw new ApplicationException("No implementation of Administration.getNodeTypeByRelTypeCatalog for this provider: "+providerName);
			}
		}

	}
}		
