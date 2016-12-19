
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
	public class Security
	{
		/// <summary>
		/// Private constructor
		/// </summary>		
		private Security()
		{
		}
		
		
		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetUsersByRole(SqlString nvchrRole)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return Security.GetUsersByRole(nvchrRole, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetUsersByRole(SqlString nvchrRole, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return Security.GetUsersByRole(nvchrRole, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetUsersByRole(SqlString nvchrRole, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlSecurity.GetUsersByRole(nvchrRole, connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Security.GetUsersByRole for this provider: "+providerName);
					throw new ApplicationException("No implementation of Security.GetUsersByRole for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
        public static void HasPermission( SqlString vchrUserLogin, SqlString ObjectTypeSN, SqlString ActionSN, SqlString ParentObjectTypeSN, SqlInt32 FK_Org, SqlInt32 FK_Catalog, SqlInt64 FK_CategoryCNode, SqlInt64 FK_ComponentCNode, SqlInt32 FK_Attribute, SqlInt32 FK_Relationship, SqlBoolean bitForDraft, ref SqlBoolean bitHasPermission )
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			Security.HasPermission(vchrUserLogin, ObjectTypeSN, ActionSN, ParentObjectTypeSN, FK_Org, FK_Catalog, FK_CategoryCNode, FK_ComponentCNode, FK_Attribute, FK_Relationship, bitForDraft, ref bitHasPermission, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
        public static void HasPermission( SqlString vchrUserLogin, SqlString ObjectTypeSN, SqlString ActionSN, SqlString ParentObjectTypeSN, SqlInt32 FK_Org, SqlInt32 FK_Catalog, SqlInt64 FK_CategoryCNode, SqlInt64 FK_ComponentCNode, SqlInt32 FK_Attribute, SqlInt32 FK_Relationship, SqlBoolean bitForDraft, ref SqlBoolean bitHasPermission, IDbConnection connection )
		{
			IDbTransaction transaction = null;
			Security.HasPermission(vchrUserLogin, ObjectTypeSN, ActionSN, ParentObjectTypeSN, FK_Org, FK_Catalog, FK_CategoryCNode, FK_ComponentCNode, FK_Attribute, FK_Relationship, bitForDraft, ref bitHasPermission, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
        public static void HasPermission( SqlString vchrUserLogin, SqlString ObjectTypeSN, SqlString ActionSN, SqlString ParentObjectTypeSN, SqlInt32 FK_Org, SqlInt32 FK_Catalog, SqlInt64 FK_CategoryCNode, SqlInt64 FK_ComponentCNode, SqlInt32 FK_Attribute, SqlInt32 FK_Relationship, SqlBoolean bitForDraft, ref SqlBoolean bitHasPermission, IDbConnection connection, IDbTransaction transaction )
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					SqlSecurity.HasPermission(vchrUserLogin, ObjectTypeSN, ActionSN, ParentObjectTypeSN, FK_Org, FK_Catalog, FK_CategoryCNode, FK_ComponentCNode, FK_Attribute, FK_Relationship, bitForDraft, ref bitHasPermission, connection, transaction);

					break;

				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Security.HasPermission for this provider: "+providerName);
					throw new ApplicationException("No implementation of Security.HasPermission for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static void IsSystemUser(SqlString nvchrUserLogin, ref SqlString isSystemUser)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			Security.IsSystemUser(nvchrUserLogin, ref isSystemUser, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static void IsSystemUser(SqlString nvchrUserLogin, ref SqlString isSystemUser, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			Security.IsSystemUser(nvchrUserLogin, ref isSystemUser, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static void IsSystemUser(SqlString nvchrUserLogin, ref SqlString isSystemUser, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					SqlSecurity.IsSystemUser(nvchrUserLogin, ref isSystemUser, connection, transaction);

					break;

				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Security.IsSystemUser for this provider: "+providerName);
					throw new ApplicationException("No implementation of Security.IsSystemUser for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static string GetUsers(SqlInt32 intPK_Security_User, SqlInt32 intUserType, SqlInt32 intCountFrom, SqlInt32 intCountTo, SqlString vchrSortColumn, SqlString vchrSearchColumn, SqlString vchrSearchParameter, SqlString vchrUserLogin)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return Security.GetUsers(intPK_Security_User, intUserType, intCountFrom, intCountTo, vchrSortColumn, vchrSearchColumn, vchrSearchParameter, vchrUserLogin, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static string GetUsers(SqlInt32 intPK_Security_User, SqlInt32 intUserType, SqlInt32 intCountFrom, SqlInt32 intCountTo, SqlString vchrSortColumn, SqlString vchrSearchColumn, SqlString vchrSearchParameter, SqlString vchrUserLogin, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return Security.GetUsers(intPK_Security_User, intUserType, intCountFrom, intCountTo, vchrSortColumn, vchrSearchColumn, vchrSearchParameter, vchrUserLogin, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static string GetUsers(SqlInt32 intPK_Security_User, SqlInt32 intUserType, SqlInt32 intCountFrom, SqlInt32 intCountTo, SqlString vchrSortColumn, SqlString vchrSearchColumn, SqlString vchrSearchParameter, SqlString vchrUserLogin, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlSecurity.GetUsers(intPK_Security_User, intUserType, intCountFrom, intCountTo, vchrSortColumn, vchrSearchColumn, vchrSearchParameter, vchrUserLogin, connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Security.GetUsers for this provider: "+providerName);
					throw new ApplicationException("No implementation of Security.GetUsers for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static string GetUsersWithRoles(SqlInt32 intPK_Security_User, SqlInt32 intCountFrom, SqlInt32 intCountTo, SqlString vchrSearchColumn, SqlString vchrSearchParameter, SqlString vchrUserLogin)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return Security.GetUsersWithRoles(intPK_Security_User, intCountFrom, intCountTo, vchrSearchColumn, vchrSearchParameter, vchrUserLogin, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static string GetUsersWithRoles(SqlInt32 intPK_Security_User, SqlInt32 intCountFrom, SqlInt32 intCountTo, SqlString vchrSearchColumn, SqlString vchrSearchParameter, SqlString vchrUserLogin, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return Security.GetUsersWithRoles(intPK_Security_User, intCountFrom, intCountTo, vchrSearchColumn, vchrSearchParameter, vchrUserLogin, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static string GetUsersWithRoles(SqlInt32 intPK_Security_User, SqlInt32 intCountFrom, SqlInt32 intCountTo, SqlString vchrSearchColumn, SqlString vchrSearchParameter, SqlString vchrUserLogin, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlSecurity.GetUsersWithRoles(intPK_Security_User, intCountFrom, intCountTo, vchrSearchColumn, vchrSearchParameter, vchrUserLogin, connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Security.GetUsersWithRoles for this provider: "+providerName);
					throw new ApplicationException("No implementation of Security.GetUsersWithRoles for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static string GetRoles(SqlInt32 intPK_Security_Role, SqlInt32 intUserType, SqlString chrGetPermissionSetOnly, SqlInt32 intCountFrom, SqlInt32 intCountTo, SqlString vchrSortColumn, SqlString vchrSearchColumn, SqlString vchrSearchParameter, SqlString vchrUserLogin, SqlBoolean bitDisplaySystemRole)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return Security.GetRoles(intPK_Security_Role, intUserType, chrGetPermissionSetOnly, intCountFrom, intCountTo, vchrSortColumn, vchrSearchColumn, vchrSearchParameter, vchrUserLogin, bitDisplaySystemRole, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static string GetRoles(SqlInt32 intPK_Security_Role, SqlInt32 intUserType, SqlString chrGetPermissionSetOnly, SqlInt32 intCountFrom, SqlInt32 intCountTo, SqlString vchrSortColumn, SqlString vchrSearchColumn, SqlString vchrSearchParameter, SqlString vchrUserLogin, SqlBoolean bitDisplaySystemRole, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return Security.GetRoles(intPK_Security_Role, intUserType, chrGetPermissionSetOnly, intCountFrom, intCountTo, vchrSortColumn, vchrSearchColumn, vchrSearchParameter, vchrUserLogin, bitDisplaySystemRole, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static string GetRoles(SqlInt32 intPK_Security_Role, SqlInt32 intUserType, SqlString chrGetPermissionSetOnly, SqlInt32 intCountFrom, SqlInt32 intCountTo, SqlString vchrSortColumn, SqlString vchrSearchColumn, SqlString vchrSearchParameter, SqlString vchrUserLogin, SqlBoolean bitDisplaySystemRole, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlSecurity.GetRoles(intPK_Security_Role, intUserType, chrGetPermissionSetOnly, intCountFrom, intCountTo, vchrSortColumn, vchrSearchColumn, vchrSearchParameter, vchrUserLogin, bitDisplaySystemRole, connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Security.GetRoles for this provider: "+providerName);
					throw new ApplicationException("No implementation of Security.GetRoles for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static string GetRolesWithUsers(SqlInt32 intPK_Security_Role, SqlString chrGetPermissionSetOnly, SqlInt32 intCountFrom, SqlInt32 intCountTo, SqlString vchrSearchColumn, SqlString vchrSearchParameter, SqlString vchrUserLogin, SqlBoolean bitDisplaySystemRole)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return Security.GetRolesWithUsers(intPK_Security_Role, chrGetPermissionSetOnly, intCountFrom, intCountTo, vchrSearchColumn, vchrSearchParameter, vchrUserLogin, bitDisplaySystemRole, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static string GetRolesWithUsers(SqlInt32 intPK_Security_Role, SqlString chrGetPermissionSetOnly, SqlInt32 intCountFrom, SqlInt32 intCountTo, SqlString vchrSearchColumn, SqlString vchrSearchParameter, SqlString vchrUserLogin, SqlBoolean bitDisplaySystemRole, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return Security.GetRolesWithUsers(intPK_Security_Role, chrGetPermissionSetOnly, intCountFrom, intCountTo, vchrSearchColumn, vchrSearchParameter, vchrUserLogin, bitDisplaySystemRole, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static string GetRolesWithUsers(SqlInt32 intPK_Security_Role, SqlString chrGetPermissionSetOnly, SqlInt32 intCountFrom, SqlInt32 intCountTo, SqlString vchrSearchColumn, SqlString vchrSearchParameter, SqlString vchrUserLogin, SqlBoolean bitDisplaySystemRole, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlSecurity.GetRolesWithUsers(intPK_Security_Role, chrGetPermissionSetOnly, intCountFrom, intCountTo, vchrSearchColumn, vchrSearchParameter, vchrUserLogin, bitDisplaySystemRole, connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Security.GetRolesWithUsers for this provider: "+providerName);
					throw new ApplicationException("No implementation of Security.GetRolesWithUsers for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static void ProcessUsers(SqlString txtXML, SqlString vchrUserLogin)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			Security.ProcessUsers(txtXML, vchrUserLogin, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static void ProcessUsers(SqlString txtXML, SqlString vchrUserLogin, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			Security.ProcessUsers(txtXML, vchrUserLogin, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static void ProcessUsers(SqlString txtXML, SqlString vchrUserLogin, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					SqlSecurity.ProcessUsers(txtXML, vchrUserLogin, connection, transaction);

					break;

				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Security.ProcessUsers for this provider: "+providerName);
					throw new ApplicationException("No implementation of Security.ProcessUsers for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable ProcessRoles(SqlString txtXML, SqlString vchrUserLogin)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return Security.ProcessRoles(txtXML, vchrUserLogin, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable ProcessRoles(SqlString txtXML, SqlString vchrUserLogin, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return Security.ProcessRoles(txtXML, vchrUserLogin, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable ProcessRoles(SqlString txtXML, SqlString vchrUserLogin, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlSecurity.ProcessRoles(txtXML, vchrUserLogin, connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Security.ProcessRoles for this provider: "+providerName);
					throw new ApplicationException("No implementation of Security.ProcessRoles for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static string GetObjectTypeAction()
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return Security.GetObjectTypeAction(connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static string GetObjectTypeAction(IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return Security.GetObjectTypeAction(connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static string GetObjectTypeAction(IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlSecurity.GetObjectTypeAction(connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Security.GetObjectTypeAction for this provider: "+providerName);
					throw new ApplicationException("No implementation of Security.GetObjectTypeAction for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static string GetHierarchyAction()
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return Security.GetHierarchyAction(connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static string GetHierarchyAction(IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return Security.GetHierarchyAction(connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static string GetHierarchyAction(IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlSecurity.GetHierarchyAction(connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Security.GetHierarchyAction for this provider: "+providerName);
					throw new ApplicationException("No implementation of Security.GetHierarchyAction for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static string GetPermissions(SqlInt32 intPK_Security_Role, SqlString chrPermissionSet, SqlString vchrUserLogin)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return Security.GetPermissions(intPK_Security_Role, chrPermissionSet, vchrUserLogin, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static string GetPermissions(SqlInt32 intPK_Security_Role, SqlString chrPermissionSet, SqlString vchrUserLogin, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return Security.GetPermissions(intPK_Security_Role, chrPermissionSet, vchrUserLogin, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static string GetPermissions(SqlInt32 intPK_Security_Role, SqlString chrPermissionSet, SqlString vchrUserLogin, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlSecurity.GetPermissions(intPK_Security_Role, chrPermissionSet, vchrUserLogin, connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Security.GetPermissions for this provider: "+providerName);
					throw new ApplicationException("No implementation of Security.GetPermissions for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static void ProcessPermissions(SqlString txtXML, SqlString vchrUserLogin)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			Security.ProcessPermissions(txtXML, vchrUserLogin, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static void ProcessPermissions(SqlString txtXML, SqlString vchrUserLogin, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			Security.ProcessPermissions(txtXML, vchrUserLogin, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static void ProcessPermissions(SqlString txtXML, SqlString vchrUserLogin, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					SqlSecurity.ProcessPermissions(txtXML, vchrUserLogin, connection, transaction);

					break;

				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Security.ProcessPermissions for this provider: "+providerName);
					throw new ApplicationException("No implementation of Security.ProcessPermissions for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static string GetAllOrganizationsAndCatalogs(SqlString vchrUserLogin)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return Security.GetAllOrganizationsAndCatalogs(vchrUserLogin, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static string GetAllOrganizationsAndCatalogs(SqlString vchrUserLogin, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return Security.GetAllOrganizationsAndCatalogs(vchrUserLogin, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static string GetAllOrganizationsAndCatalogs(SqlString vchrUserLogin, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlSecurity.GetAllOrganizationsAndCatalogs(vchrUserLogin, connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Security.GetAllOrganizationsAndCatalogs for this provider: "+providerName);
					throw new ApplicationException("No implementation of Security.GetAllOrganizationsAndCatalogs for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static void AuthenticateUser(SqlString vchrUserLogin, SqlString vchrPassword, out SqlInt32 RETURN_VALUE)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			Security.AuthenticateUser(vchrUserLogin, vchrPassword, out RETURN_VALUE, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static void AuthenticateUser(SqlString vchrUserLogin, SqlString vchrPassword, out SqlInt32 RETURN_VALUE, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			Security.AuthenticateUser(vchrUserLogin, vchrPassword, out RETURN_VALUE, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static void AuthenticateUser(SqlString vchrUserLogin, SqlString vchrPassword, out SqlInt32 RETURN_VALUE, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					SqlSecurity.AuthenticateUser(vchrUserLogin, vchrPassword, out RETURN_VALUE, connection, transaction);

					break;

				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Security.AuthenticateUser for this provider: "+providerName);
					throw new ApplicationException("No implementation of Security.AuthenticateUser for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static string GetMenus(SqlString vchrUserLogin)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return Security.GetMenus(vchrUserLogin, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static string GetMenus(SqlString vchrUserLogin, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return Security.GetMenus(vchrUserLogin, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static string GetMenus(SqlString vchrUserLogin, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlSecurity.GetMenus(vchrUserLogin, connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Security.GetMenus for this provider: "+providerName);
					throw new ApplicationException("No implementation of Security.GetMenus for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static string GetUserPreferences(SqlString vchrTargetUserLogin, SqlString vchrUserLogin)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return Security.GetUserPreferences(vchrTargetUserLogin, vchrUserLogin, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static string GetUserPreferences(SqlString vchrTargetUserLogin, SqlString vchrUserLogin, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return Security.GetUserPreferences(vchrTargetUserLogin, vchrUserLogin, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static string GetUserPreferences(SqlString vchrTargetUserLogin, SqlString vchrUserLogin, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlSecurity.GetUserPreferences(vchrTargetUserLogin, vchrUserLogin, connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Security.GetUserPreferences for this provider: "+providerName);
					throw new ApplicationException("No implementation of Security.GetUserPreferences for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static string GetRoleMenus(SqlInt32 PK_Security_Role)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return Security.GetRoleMenus(PK_Security_Role, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static string GetRoleMenus(SqlInt32 PK_Security_Role, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return Security.GetRoleMenus(PK_Security_Role, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static string GetRoleMenus(SqlInt32 PK_Security_Role, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlSecurity.GetRoleMenus(PK_Security_Role, connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Security.GetRoleMenus for this provider: "+providerName);
					throw new ApplicationException("No implementation of Security.GetRoleMenus for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static void ProcessUserPreferences(SqlString txtXML, SqlString vchrTargetUserLogin, SqlString vchrUserLogin)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			Security.ProcessUserPreferences(txtXML, vchrTargetUserLogin, vchrUserLogin, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static void ProcessUserPreferences(SqlString txtXML, SqlString vchrTargetUserLogin, SqlString vchrUserLogin, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			Security.ProcessUserPreferences(txtXML, vchrTargetUserLogin, vchrUserLogin, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static void ProcessUserPreferences(SqlString txtXML, SqlString vchrTargetUserLogin, SqlString vchrUserLogin, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					SqlSecurity.ProcessUserPreferences(txtXML, vchrTargetUserLogin, vchrUserLogin, connection, transaction);

					break;

				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Security.ProcessUserPreferences for this provider: "+providerName);
					throw new ApplicationException("No implementation of Security.ProcessUserPreferences for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static void ProcessRoleMenus(SqlString txtXML, SqlString vchrUserLogin)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			Security.ProcessRoleMenus(txtXML, vchrUserLogin, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static void ProcessRoleMenus(SqlString txtXML, SqlString vchrUserLogin, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			Security.ProcessRoleMenus(txtXML, vchrUserLogin, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static void ProcessRoleMenus(SqlString txtXML, SqlString vchrUserLogin, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					SqlSecurity.ProcessRoleMenus(txtXML, vchrUserLogin, connection, transaction);

					break;

				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Security.ProcessRoleMenus for this provider: "+providerName);
					throw new ApplicationException("No implementation of Security.ProcessRoleMenus for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static void GetDraftID(SqlInt32 productionID, out SqlInt32 RETURN_VALUE)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			Security.GetDraftID(productionID, out RETURN_VALUE, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static void GetDraftID(SqlInt32 productionID, out SqlInt32 RETURN_VALUE, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			Security.GetDraftID(productionID, out RETURN_VALUE, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static void GetDraftID(SqlInt32 productionID, out SqlInt32 RETURN_VALUE, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					SqlSecurity.GetDraftID(productionID, out RETURN_VALUE, connection, transaction);

					break;

				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Security.GetDraftID for this provider: "+providerName);
					throw new ApplicationException("No implementation of Security.GetDraftID for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static void GetAppSetting(SqlString optionName, ref SqlString optionValue)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			Security.GetAppSetting(optionName, ref optionValue, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static void GetAppSetting(SqlString optionName, ref SqlString optionValue, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			Security.GetAppSetting(optionName, ref optionValue, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static void GetAppSetting(SqlString optionName, ref SqlString optionValue, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					SqlSecurity.GetAppSetting(optionName, ref optionValue, connection, transaction);

					break;

				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Security.GetAppSetting for this provider: "+providerName);
					throw new ApplicationException("No implementation of Security.GetAppSetting for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataSet GetAppconfigNameVals()
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return Security.GetAppconfigNameVals(connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataSet GetAppconfigNameVals(IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return Security.GetAppconfigNameVals(connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataSet GetAppconfigNameVals(IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlSecurity.GetAppconfigNameVals(connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Security.GetAppconfigNameVals for this provider: "+providerName);
					throw new ApplicationException("No implementation of Security.GetAppconfigNameVals for this provider: "+providerName);
			}
		}

	}
}		
