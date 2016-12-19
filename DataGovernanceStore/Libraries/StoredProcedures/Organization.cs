
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
	public class Organization
	{
		/// <summary>
		/// Private constructor
		/// </summary>		
		private Organization()
		{
		}
		
		
		/// <summary>
        /// 
        /// </summary>
		public static string GetCatalogNodeTypeAttributeGroups(SqlInt32 intCatalogID, SqlInt32 intNodeTypeID)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return Organization.GetCatalogNodeTypeAttributeGroups(intCatalogID, intNodeTypeID, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static string GetCatalogNodeTypeAttributeGroups(SqlInt32 intCatalogID, SqlInt32 intNodeTypeID, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return Organization.GetCatalogNodeTypeAttributeGroups(intCatalogID, intNodeTypeID, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static string GetCatalogNodeTypeAttributeGroups(SqlInt32 intCatalogID, SqlInt32 intNodeTypeID, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlOrganization.GetCatalogNodeTypeAttributeGroups(intCatalogID, intNodeTypeID, connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Organization.GetCatalogNodeTypeAttributeGroups for this provider: "+providerName);
					throw new ApplicationException("No implementation of Organization.GetCatalogNodeTypeAttributeGroups for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetOrgsWithPermissions(SqlString vchrUserLogin)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return Organization.GetOrgsWithPermissions(vchrUserLogin, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetOrgsWithPermissions(SqlString vchrUserLogin, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return Organization.GetOrgsWithPermissions(vchrUserLogin, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetOrgsWithPermissions(SqlString vchrUserLogin, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlOrganization.GetOrgsWithPermissions(vchrUserLogin, connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Organization.GetOrgsWithPermissions for this provider: "+providerName);
					throw new ApplicationException("No implementation of Organization.GetOrgsWithPermissions for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static string GetOrgsWithPermissions(SqlString vchrTargetUserLogin, SqlString vchrUserLogin, SqlInt32 FK_Locale, SqlInt32 intCountFrom, SqlInt32 intCountTo, SqlString vchrSortColumn, SqlString vchrSearchColumn, SqlString vchrSearchParameter, SqlInt32 FK_OrgParent, SqlInt32 FK_OrgClassification, SqlString CatalogObjectType)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return Organization.GetOrgsWithPermissions(vchrTargetUserLogin, vchrUserLogin, FK_Locale, intCountFrom, intCountTo, vchrSortColumn, vchrSearchColumn, vchrSearchParameter, FK_OrgParent, FK_OrgClassification, CatalogObjectType, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static string GetOrgsWithPermissions(SqlString vchrTargetUserLogin, SqlString vchrUserLogin, SqlInt32 FK_Locale, SqlInt32 intCountFrom, SqlInt32 intCountTo, SqlString vchrSortColumn, SqlString vchrSearchColumn, SqlString vchrSearchParameter, SqlInt32 FK_OrgParent, SqlInt32 FK_OrgClassification, SqlString CatalogObjectType, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return Organization.GetOrgsWithPermissions(vchrTargetUserLogin, vchrUserLogin, FK_Locale, intCountFrom, intCountTo, vchrSortColumn, vchrSearchColumn, vchrSearchParameter, FK_OrgParent, FK_OrgClassification, CatalogObjectType, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static string GetOrgsWithPermissions(SqlString vchrTargetUserLogin, SqlString vchrUserLogin, SqlInt32 FK_Locale, SqlInt32 intCountFrom, SqlInt32 intCountTo, SqlString vchrSortColumn, SqlString vchrSearchColumn, SqlString vchrSearchParameter, SqlInt32 FK_OrgParent, SqlInt32 FK_OrgClassification, SqlString CatalogObjectType, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlOrganization.GetOrgsWithPermissions(vchrTargetUserLogin, vchrUserLogin, FK_Locale, intCountFrom, intCountTo, vchrSortColumn, vchrSearchColumn, vchrSearchParameter, FK_OrgParent, FK_OrgClassification, CatalogObjectType, connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Organization.GetOrgsWithPermissions for this provider: "+providerName);
					throw new ApplicationException("No implementation of Organization.GetOrgsWithPermissions for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static string GetOrgsByOrgClassification(SqlString vchrTargetUserLogin, SqlString vchrUserLogin, SqlInt32 FK_Locale, SqlInt32 intCountFrom, SqlInt32 intCountTo, SqlString vchrSortColumn, SqlString vchrSearchColumn, SqlString vchrSearchParameter, SqlInt32 FK_OrgClassification, SqlString CatalogObjectType)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return Organization.GetOrgsByOrgClassification(vchrTargetUserLogin, vchrUserLogin, FK_Locale, intCountFrom, intCountTo, vchrSortColumn, vchrSearchColumn, vchrSearchParameter, FK_OrgClassification, CatalogObjectType, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static string GetOrgsByOrgClassification(SqlString vchrTargetUserLogin, SqlString vchrUserLogin, SqlInt32 FK_Locale, SqlInt32 intCountFrom, SqlInt32 intCountTo, SqlString vchrSortColumn, SqlString vchrSearchColumn, SqlString vchrSearchParameter, SqlInt32 FK_OrgClassification, SqlString CatalogObjectType, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return Organization.GetOrgsByOrgClassification(vchrTargetUserLogin, vchrUserLogin, FK_Locale, intCountFrom, intCountTo, vchrSortColumn, vchrSearchColumn, vchrSearchParameter, FK_OrgClassification, CatalogObjectType, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static string GetOrgsByOrgClassification(SqlString vchrTargetUserLogin, SqlString vchrUserLogin, SqlInt32 FK_Locale, SqlInt32 intCountFrom, SqlInt32 intCountTo, SqlString vchrSortColumn, SqlString vchrSearchColumn, SqlString vchrSearchParameter, SqlInt32 FK_OrgClassification, SqlString CatalogObjectType, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlOrganization.GetOrgsByOrgClassification(vchrTargetUserLogin, vchrUserLogin, FK_Locale, intCountFrom, intCountTo, vchrSortColumn, vchrSearchColumn, vchrSearchParameter, FK_OrgClassification, CatalogObjectType, connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Organization.GetOrgsByOrgClassification for this provider: "+providerName);
					throw new ApplicationException("No implementation of Organization.GetOrgsByOrgClassification for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static string GetUserVisibleOrgs(SqlString vchrTargetUserLogin, SqlString vchrUserLogin, SqlInt32 FK_Locale, SqlInt32 intCountFrom, SqlInt32 intCountTo, SqlString vchrSortColumn, SqlString vchrSearchColumn, SqlString vchrSearchParameter)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return Organization.GetUserVisibleOrgs(vchrTargetUserLogin, vchrUserLogin, FK_Locale, intCountFrom, intCountTo, vchrSortColumn, vchrSearchColumn, vchrSearchParameter, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static string GetUserVisibleOrgs(SqlString vchrTargetUserLogin, SqlString vchrUserLogin, SqlInt32 FK_Locale, SqlInt32 intCountFrom, SqlInt32 intCountTo, SqlString vchrSortColumn, SqlString vchrSearchColumn, SqlString vchrSearchParameter, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return Organization.GetUserVisibleOrgs(vchrTargetUserLogin, vchrUserLogin, FK_Locale, intCountFrom, intCountTo, vchrSortColumn, vchrSearchColumn, vchrSearchParameter, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static string GetUserVisibleOrgs(SqlString vchrTargetUserLogin, SqlString vchrUserLogin, SqlInt32 FK_Locale, SqlInt32 intCountFrom, SqlInt32 intCountTo, SqlString vchrSortColumn, SqlString vchrSearchColumn, SqlString vchrSearchParameter, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlOrganization.GetUserVisibleOrgs(vchrTargetUserLogin, vchrUserLogin, FK_Locale, intCountFrom, intCountTo, vchrSortColumn, vchrSearchColumn, vchrSearchParameter, connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Organization.GetUserVisibleOrgs for this provider: "+providerName);
					throw new ApplicationException("No implementation of Organization.GetUserVisibleOrgs for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetUserVisibleOrgsDT(SqlString vchrTargetUserLogin, SqlString vchrUserLogin, SqlInt32 FK_Locale, SqlInt32 intCountFrom, SqlInt32 intCountTo, SqlString vchrSortColumn, SqlString vchrSearchColumn, SqlString vchrSearchParameter)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return Organization.GetUserVisibleOrgsDT(vchrTargetUserLogin, vchrUserLogin, FK_Locale, intCountFrom, intCountTo, vchrSortColumn, vchrSearchColumn, vchrSearchParameter, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetUserVisibleOrgsDT(SqlString vchrTargetUserLogin, SqlString vchrUserLogin, SqlInt32 FK_Locale, SqlInt32 intCountFrom, SqlInt32 intCountTo, SqlString vchrSortColumn, SqlString vchrSearchColumn, SqlString vchrSearchParameter, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return Organization.GetUserVisibleOrgsDT(vchrTargetUserLogin, vchrUserLogin, FK_Locale, intCountFrom, intCountTo, vchrSortColumn, vchrSearchColumn, vchrSearchParameter, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetUserVisibleOrgsDT(SqlString vchrTargetUserLogin, SqlString vchrUserLogin, SqlInt32 FK_Locale, SqlInt32 intCountFrom, SqlInt32 intCountTo, SqlString vchrSortColumn, SqlString vchrSearchColumn, SqlString vchrSearchParameter, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlOrganization.GetUserVisibleOrgsDT(vchrTargetUserLogin, vchrUserLogin, FK_Locale, intCountFrom, intCountTo, vchrSortColumn, vchrSearchColumn, vchrSearchParameter, connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Organization.GetUserVisibleOrgsDT for this provider: "+providerName);
					throw new ApplicationException("No implementation of Organization.GetUserVisibleOrgsDT for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static string GetAllOrgTypes(SqlInt32 intPK_OrgType, SqlInt32 intCountFrom, SqlInt32 intCountTo, SqlString vchrSortColumn, SqlString vchrSearchColumn, SqlString vchrSearchParameter, SqlString vchrUserLogin)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return Organization.GetAllOrgTypes(intPK_OrgType, intCountFrom, intCountTo, vchrSortColumn, vchrSearchColumn, vchrSearchParameter, vchrUserLogin, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static string GetAllOrgTypes(SqlInt32 intPK_OrgType, SqlInt32 intCountFrom, SqlInt32 intCountTo, SqlString vchrSortColumn, SqlString vchrSearchColumn, SqlString vchrSearchParameter, SqlString vchrUserLogin, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return Organization.GetAllOrgTypes(intPK_OrgType, intCountFrom, intCountTo, vchrSortColumn, vchrSearchColumn, vchrSearchParameter, vchrUserLogin, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static string GetAllOrgTypes(SqlInt32 intPK_OrgType, SqlInt32 intCountFrom, SqlInt32 intCountTo, SqlString vchrSortColumn, SqlString vchrSearchColumn, SqlString vchrSearchParameter, SqlString vchrUserLogin, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlOrganization.GetAllOrgTypes(intPK_OrgType, intCountFrom, intCountTo, vchrSortColumn, vchrSearchColumn, vchrSearchParameter, vchrUserLogin, connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Organization.GetAllOrgTypes for this provider: "+providerName);
					throw new ApplicationException("No implementation of Organization.GetAllOrgTypes for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static string GetOrgTypes(SqlInt32 intPK_OrgType, SqlInt32 intCountFrom, SqlInt32 intCountTo, SqlString vchrSortColumn, SqlString vchrSearchColumn, SqlString vchrSearchParameter, SqlString vchrUserLogin)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return Organization.GetOrgTypes(intPK_OrgType, intCountFrom, intCountTo, vchrSortColumn, vchrSearchColumn, vchrSearchParameter, vchrUserLogin, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static string GetOrgTypes(SqlInt32 intPK_OrgType, SqlInt32 intCountFrom, SqlInt32 intCountTo, SqlString vchrSortColumn, SqlString vchrSearchColumn, SqlString vchrSearchParameter, SqlString vchrUserLogin, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return Organization.GetOrgTypes(intPK_OrgType, intCountFrom, intCountTo, vchrSortColumn, vchrSearchColumn, vchrSearchParameter, vchrUserLogin, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static string GetOrgTypes(SqlInt32 intPK_OrgType, SqlInt32 intCountFrom, SqlInt32 intCountTo, SqlString vchrSortColumn, SqlString vchrSearchColumn, SqlString vchrSearchParameter, SqlString vchrUserLogin, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlOrganization.GetOrgTypes(intPK_OrgType, intCountFrom, intCountTo, vchrSortColumn, vchrSearchColumn, vchrSearchParameter, vchrUserLogin, connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Organization.GetOrgTypes for this provider: "+providerName);
					throw new ApplicationException("No implementation of Organization.GetOrgTypes for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static string GetAllOrgHierarchies()
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return Organization.GetAllOrgHierarchies(connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static string GetAllOrgHierarchies(IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return Organization.GetAllOrgHierarchies(connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static string GetAllOrgHierarchies(IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlOrganization.GetAllOrgHierarchies(connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Organization.GetAllOrgHierarchies for this provider: "+providerName);
					throw new ApplicationException("No implementation of Organization.GetAllOrgHierarchies for this provider: "+providerName);
			}
		}
        
		/// <summary>
        /// 
        /// </summary>
		public static DataSet GetWithLocales(SqlString vchrUserLogin, SqlInt32 PK_Org)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return Organization.GetWithLocales(vchrUserLogin, PK_Org, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataSet GetWithLocales(SqlString vchrUserLogin, SqlInt32 PK_Org, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return Organization.GetWithLocales(vchrUserLogin, PK_Org, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataSet GetWithLocales(SqlString vchrUserLogin, SqlInt32 PK_Org, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlOrganization.GetWithLocales(vchrUserLogin, PK_Org, connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Organization.GetWithLocales for this provider: "+providerName);
					throw new ApplicationException("No implementation of Organization.GetWithLocales for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static string GetOrgsWithLocales(SqlInt32 FK_OrgClassification, SqlInt32 FK_Locale, SqlString vchrUserLogin)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return Organization.GetOrgsWithLocales(FK_OrgClassification, FK_Locale, vchrUserLogin, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static string GetOrgsWithLocales(SqlInt32 FK_OrgClassification, SqlInt32 FK_Locale, SqlString vchrUserLogin, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return Organization.GetOrgsWithLocales(FK_OrgClassification, FK_Locale, vchrUserLogin, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static string GetOrgsWithLocales(SqlInt32 FK_OrgClassification, SqlInt32 FK_Locale, SqlString vchrUserLogin, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlOrganization.GetOrgsWithLocales(FK_OrgClassification, FK_Locale, vchrUserLogin, connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Organization.GetOrgsWithLocales for this provider: "+providerName);
					throw new ApplicationException("No implementation of Organization.GetOrgsWithLocales for this provider: "+providerName);
			}
		}
        
		/// <summary>
        /// 
        /// </summary>
		public static DataTable ProcessOrgs(SqlString txtXML, SqlString vchrUserLogin)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return Organization.ProcessOrgs(txtXML, vchrUserLogin, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable ProcessOrgs(SqlString txtXML, SqlString vchrUserLogin, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return Organization.ProcessOrgs(txtXML, vchrUserLogin, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable ProcessOrgs(SqlString txtXML, SqlString vchrUserLogin, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlOrganization.ProcessOrgs(txtXML, vchrUserLogin, connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Organization.ProcessOrgs for this provider: "+providerName);
					throw new ApplicationException("No implementation of Organization.ProcessOrgs for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static void ProcessOrgLocales(SqlString txtXML, SqlString vchrUserLogin)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			Organization.ProcessOrgLocales(txtXML, vchrUserLogin, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static void ProcessOrgLocales(SqlString txtXML, SqlString vchrUserLogin, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			Organization.ProcessOrgLocales(txtXML, vchrUserLogin, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static void ProcessOrgLocales(SqlString txtXML, SqlString vchrUserLogin, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					SqlOrganization.ProcessOrgLocales(txtXML, vchrUserLogin, connection, transaction);

					break;

				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Organization.ProcessOrgLocales for this provider: "+providerName);
					throw new ApplicationException("No implementation of Organization.ProcessOrgLocales for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static void ProcessOrgTypes(SqlString txtXML, SqlString vchrUserLogin)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			Organization.ProcessOrgTypes(txtXML, vchrUserLogin, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static void ProcessOrgTypes(SqlString txtXML, SqlString vchrUserLogin, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			Organization.ProcessOrgTypes(txtXML, vchrUserLogin, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static void ProcessOrgTypes(SqlString txtXML, SqlString vchrUserLogin, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					SqlOrganization.ProcessOrgTypes(txtXML, vchrUserLogin, connection, transaction);

					break;

				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Organization.ProcessOrgTypes for this provider: "+providerName);
					throw new ApplicationException("No implementation of Organization.ProcessOrgTypes for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetAvailableChildrenOrgs(SqlString vchrTargetUserLogin, SqlString vchrUserLogin, SqlInt32 FK_Locale, SqlInt32 intCountFrom, SqlInt32 intCountTo, SqlString vchrSortColumn, SqlString vchrSearchColumn, SqlString vchrSearchParameter, SqlInt32 FK_OrgParent, SqlInt32 FK_OrgClassification, SqlInt32 PK_Org, SqlString CatalogObjectType)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return Organization.GetAvailableChildrenOrgs(vchrTargetUserLogin, vchrUserLogin, FK_Locale, intCountFrom, intCountTo, vchrSortColumn, vchrSearchColumn, vchrSearchParameter, FK_OrgParent, FK_OrgClassification, PK_Org, CatalogObjectType, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetAvailableChildrenOrgs(SqlString vchrTargetUserLogin, SqlString vchrUserLogin, SqlInt32 FK_Locale, SqlInt32 intCountFrom, SqlInt32 intCountTo, SqlString vchrSortColumn, SqlString vchrSearchColumn, SqlString vchrSearchParameter, SqlInt32 FK_OrgParent, SqlInt32 FK_OrgClassification, SqlInt32 PK_Org, SqlString CatalogObjectType, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return Organization.GetAvailableChildrenOrgs(vchrTargetUserLogin, vchrUserLogin, FK_Locale, intCountFrom, intCountTo, vchrSortColumn, vchrSearchColumn, vchrSearchParameter, FK_OrgParent, FK_OrgClassification, PK_Org, CatalogObjectType, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetAvailableChildrenOrgs(SqlString vchrTargetUserLogin, SqlString vchrUserLogin, SqlInt32 FK_Locale, SqlInt32 intCountFrom, SqlInt32 intCountTo, SqlString vchrSortColumn, SqlString vchrSearchColumn, SqlString vchrSearchParameter, SqlInt32 FK_OrgParent, SqlInt32 FK_OrgClassification, SqlInt32 PK_Org, SqlString CatalogObjectType, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlOrganization.GetAvailableChildrenOrgs(vchrTargetUserLogin, vchrUserLogin, FK_Locale, intCountFrom, intCountTo, vchrSortColumn, vchrSearchColumn, vchrSearchParameter, FK_OrgParent, FK_OrgClassification, PK_Org, CatalogObjectType, connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Organization.GetAvailableChildrenOrgs for this provider: "+providerName);
					throw new ApplicationException("No implementation of Organization.GetAvailableChildrenOrgs for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static void AddOrgRelationship(SqlInt32 intChildOrgID, SqlInt32 intParentOrgID, SqlString vchrUserID, SqlString vchrProgramName)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			Organization.AddOrgRelationship(intChildOrgID, intParentOrgID, vchrUserID, vchrProgramName, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static void AddOrgRelationship(SqlInt32 intChildOrgID, SqlInt32 intParentOrgID, SqlString vchrUserID, SqlString vchrProgramName, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			Organization.AddOrgRelationship(intChildOrgID, intParentOrgID, vchrUserID, vchrProgramName, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static void AddOrgRelationship(SqlInt32 intChildOrgID, SqlInt32 intParentOrgID, SqlString vchrUserID, SqlString vchrProgramName, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					SqlOrganization.AddOrgRelationship(intChildOrgID, intParentOrgID, vchrUserID, vchrProgramName, connection, transaction);

					break;

				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Organization.AddOrgRelationship for this provider: "+providerName);
					throw new ApplicationException("No implementation of Organization.AddOrgRelationship for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetOrgByName(SqlString ShortName)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return Organization.GetOrgByName(ShortName, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetOrgByName(SqlString ShortName, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return Organization.GetOrgByName(ShortName, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetOrgByName(SqlString ShortName, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlOrganization.GetOrgByName(ShortName, connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Organization.GetOrgByName for this provider: "+providerName);
					throw new ApplicationException("No implementation of Organization.GetOrgByName for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetUserVisibleOrgsByOrgClassificationDT(SqlString vchrTargetUserLogin, SqlString vchrUserLogin, SqlInt32 FK_Locale, SqlInt32 intCountFrom, SqlInt32 intCountTo, SqlString vchrSortColumn, SqlString vchrSearchColumn, SqlString vchrSearchParameter)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return Organization.GetUserVisibleOrgsByOrgClassificationDT(vchrTargetUserLogin, vchrUserLogin, FK_Locale, intCountFrom, intCountTo, vchrSortColumn, vchrSearchColumn, vchrSearchParameter, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetUserVisibleOrgsByOrgClassificationDT(SqlString vchrTargetUserLogin, SqlString vchrUserLogin, SqlInt32 FK_Locale, SqlInt32 intCountFrom, SqlInt32 intCountTo, SqlString vchrSortColumn, SqlString vchrSearchColumn, SqlString vchrSearchParameter, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return Organization.GetUserVisibleOrgsByOrgClassificationDT(vchrTargetUserLogin, vchrUserLogin, FK_Locale, intCountFrom, intCountTo, vchrSortColumn, vchrSearchColumn, vchrSearchParameter, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetUserVisibleOrgsByOrgClassificationDT(SqlString vchrTargetUserLogin, SqlString vchrUserLogin, SqlInt32 FK_Locale, SqlInt32 intCountFrom, SqlInt32 intCountTo, SqlString vchrSortColumn, SqlString vchrSearchColumn, SqlString vchrSearchParameter, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlOrganization.GetUserVisibleOrgsByOrgClassificationDT(vchrTargetUserLogin, vchrUserLogin, FK_Locale, intCountFrom, intCountTo, vchrSortColumn, vchrSearchColumn, vchrSearchParameter, connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Organization.GetUserVisibleOrgsByOrgClassificationDT for this provider: "+providerName);
					throw new ApplicationException("No implementation of Organization.GetUserVisibleOrgsByOrgClassificationDT for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetOrgCatalogInfo(SqlInt32 OrgId, SqlInt32 CatalogId, SqlString FindWhat)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return Organization.GetOrgCatalogInfo(OrgId, CatalogId, FindWhat, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetOrgCatalogInfo(SqlInt32 OrgId, SqlInt32 CatalogId, SqlString FindWhat, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return Organization.GetOrgCatalogInfo(OrgId, CatalogId, FindWhat, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetOrgCatalogInfo(SqlInt32 OrgId, SqlInt32 CatalogId, SqlString FindWhat, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlOrganization.GetOrgCatalogInfo(OrgId, CatalogId, FindWhat, connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Organization.GetOrgCatalogInfo for this provider: "+providerName);
					throw new ApplicationException("No implementation of Organization.GetOrgCatalogInfo for this provider: "+providerName);
			}
		}

	}
}		
