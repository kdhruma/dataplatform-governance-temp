
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
	public class Common
	{
		/// <summary>
		/// Private constructor
		/// </summary>		
		private Common()
		{
		}
		
		
		/// <summary>
        /// 
        /// </summary>
		public static string GetAllUOMs()
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return Common.GetAllUOMs(connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static string GetAllUOMs(IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return Common.GetAllUOMs(connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static string GetAllUOMs(IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlCommon.GetAllUOMs(connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Common.GetAllUOMs for this provider: "+providerName);
					throw new ApplicationException("No implementation of Common.GetAllUOMs for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static string GetAllLocaleByOrgXml(SqlInt32 intOrgID)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return Common.GetAllLocaleByOrgXml(intOrgID, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static string GetAllLocaleByOrgXml(SqlInt32 intOrgID, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return Common.GetAllLocaleByOrgXml(intOrgID, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static string GetAllLocaleByOrgXml(SqlInt32 intOrgID, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlCommon.GetAllLocaleByOrgXml(intOrgID, connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Common.GetAllLocaleByOrgXml for this provider: "+providerName);
					throw new ApplicationException("No implementation of Common.GetAllLocaleByOrgXml for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static void PutFile(SqlString vchrFileName, SqlString vchrFileType, SqlBinary imgFileData, SqlBoolean bitArchive, SqlString vchrUserID, SqlString vchrProgramName, out SqlInt32 RETURN_VALUE)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			Common.PutFile(vchrFileName, vchrFileType, imgFileData, bitArchive, vchrUserID, vchrProgramName, out RETURN_VALUE, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static void PutFile(SqlString vchrFileName, SqlString vchrFileType, SqlBinary imgFileData, SqlBoolean bitArchive, SqlString vchrUserID, SqlString vchrProgramName, out SqlInt32 RETURN_VALUE, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			Common.PutFile(vchrFileName, vchrFileType, imgFileData, bitArchive, vchrUserID, vchrProgramName, out RETURN_VALUE, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static void PutFile(SqlString vchrFileName, SqlString vchrFileType, SqlBinary imgFileData, SqlBoolean bitArchive, SqlString vchrUserID, SqlString vchrProgramName, out SqlInt32 RETURN_VALUE, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					SqlCommon.PutFile(vchrFileName, vchrFileType, imgFileData, bitArchive, vchrUserID, vchrProgramName, out RETURN_VALUE, connection, transaction);

					break;

				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Common.PutFile for this provider: "+providerName);
					throw new ApplicationException("No implementation of Common.PutFile for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetFile(SqlInt32 intFileID, SqlString chrDetailsOnly)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return Common.GetFile(intFileID, chrDetailsOnly, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetFile(SqlInt32 intFileID, SqlString chrDetailsOnly, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return Common.GetFile(intFileID, chrDetailsOnly, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetFile(SqlInt32 intFileID, SqlString chrDetailsOnly, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlCommon.GetFile(intFileID, chrDetailsOnly, connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Common.GetFile for this provider: "+providerName);
					throw new ApplicationException("No implementation of Common.GetFile for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetFileDetails(SqlInt32 intFileID)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return Common.GetFileDetails(intFileID, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetFileDetails(SqlInt32 intFileID, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return Common.GetFileDetails(intFileID, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetFileDetails(SqlInt32 intFileID, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlCommon.GetFileDetails(intFileID, connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Common.GetFileDetails for this provider: "+providerName);
					throw new ApplicationException("No implementation of Common.GetFileDetails for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static void ReleaseFileReference(SqlInt32 intFileID, out SqlInt32 RETURN_VALUE)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			Common.ReleaseFileReference(intFileID, out RETURN_VALUE, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static void ReleaseFileReference(SqlInt32 intFileID, out SqlInt32 RETURN_VALUE, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			Common.ReleaseFileReference(intFileID, out RETURN_VALUE, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static void ReleaseFileReference(SqlInt32 intFileID, out SqlInt32 RETURN_VALUE, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					SqlCommon.ReleaseFileReference(intFileID, out RETURN_VALUE, connection, transaction);

					break;

				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Common.ReleaseFileReference for this provider: "+providerName);
					throw new ApplicationException("No implementation of Common.ReleaseFileReference for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable ObjectGet(SqlString Object, SqlInt32 ObjectID, SqlString SearchValue, SqlInt32 TopRows)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return Common.ObjectGet(Object, ObjectID, SearchValue, TopRows, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable ObjectGet(SqlString Object, SqlInt32 ObjectID, SqlString SearchValue, SqlInt32 TopRows, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return Common.ObjectGet(Object, ObjectID, SearchValue, TopRows, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable ObjectGet(SqlString Object, SqlInt32 ObjectID, SqlString SearchValue, SqlInt32 TopRows, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlCommon.ObjectGet(Object, ObjectID, SearchValue, TopRows, connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Common.ObjectGet for this provider: "+providerName);
					throw new ApplicationException("No implementation of Common.ObjectGet for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataSet GetObject(SqlString Object, SqlInt32 ObjectID, SqlString SearchValue, SqlInt32 TopRows)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return Common.GetObject(Object, ObjectID, SearchValue, TopRows, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataSet GetObject(SqlString Object, SqlInt32 ObjectID, SqlString SearchValue, SqlInt32 TopRows, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return Common.GetObject(Object, ObjectID, SearchValue, TopRows, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataSet GetObject(SqlString Object, SqlInt32 ObjectID, SqlString SearchValue, SqlInt32 TopRows, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlCommon.GetObject(Object, ObjectID, SearchValue, TopRows, connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Common.GetObject for this provider: "+providerName);
					throw new ApplicationException("No implementation of Common.GetObject for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable LookupRuleDataSearch(SqlString Object, SqlString TableName, SqlString SearchValue)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return Common.LookupRuleDataSearch(Object, TableName, SearchValue, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable LookupRuleDataSearch(SqlString Object, SqlString TableName, SqlString SearchValue, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return Common.LookupRuleDataSearch(Object, TableName, SearchValue, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable LookupRuleDataSearch(SqlString Object, SqlString TableName, SqlString SearchValue, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlCommon.LookupRuleDataSearch(Object, TableName, SearchValue, connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Common.LookupRuleDataSearch for this provider: "+providerName);
					throw new ApplicationException("No implementation of Common.LookupRuleDataSearch for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static string GetAppCofigAttributeInfo()
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return Common.GetAppCofigAttributeInfo(connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static string GetAppCofigAttributeInfo(IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return Common.GetAppCofigAttributeInfo(connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static string GetAppCofigAttributeInfo(IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlCommon.GetAppCofigAttributeInfo(connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Common.GetAppCofigAttributeInfo for this provider: "+providerName);
					throw new ApplicationException("No implementation of Common.GetAppCofigAttributeInfo for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static void IsLookupExist(SqlString TableName, out SqlInt32 RETURN_VALUE)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			Common.IsLookupExist(TableName, out RETURN_VALUE, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static void IsLookupExist(SqlString TableName, out SqlInt32 RETURN_VALUE, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			Common.IsLookupExist(TableName, out RETURN_VALUE, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static void IsLookupExist(SqlString TableName, out SqlInt32 RETURN_VALUE, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					SqlCommon.IsLookupExist(TableName, out RETURN_VALUE, connection, transaction);

					break;

				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Common.IsLookupExist for this provider: "+providerName);
					throw new ApplicationException("No implementation of Common.IsLookupExist for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable LookupSearch(SqlString Object, SqlString TableName, SqlString SearchValue, SqlInt32 Locale)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return Common.LookupSearch(Object, TableName, SearchValue, Locale, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable LookupSearch(SqlString Object, SqlString TableName, SqlString SearchValue, SqlInt32 Locale, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return Common.LookupSearch(Object, TableName, SearchValue, Locale, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable LookupSearch(SqlString Object, SqlString TableName, SqlString SearchValue, SqlInt32 Locale, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlCommon.LookupSearch(Object, TableName, SearchValue, Locale, connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Common.LookupSearch for this provider: "+providerName);
					throw new ApplicationException("No implementation of Common.LookupSearch for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static void UpdateAppconfig(SqlInt32 id, SqlString name, SqlString value, SqlString description, SqlString longdescription, SqlString validationrule, SqlString validationmethod, SqlString domain, SqlString client, SqlString row_source_type, SqlString row_source, SqlString user, SqlString program, out SqlInt32 RETURN_VALUE)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			Common.UpdateAppconfig(id, name, value, description, longdescription, validationrule, validationmethod, domain, client, row_source_type, row_source, user, program, out RETURN_VALUE, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static void UpdateAppconfig(SqlInt32 id, SqlString name, SqlString value, SqlString description, SqlString longdescription, SqlString validationrule, SqlString validationmethod, SqlString domain, SqlString client, SqlString row_source_type, SqlString row_source, SqlString user, SqlString program, out SqlInt32 RETURN_VALUE, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			Common.UpdateAppconfig(id, name, value, description, longdescription, validationrule, validationmethod, domain, client, row_source_type, row_source, user, program, out RETURN_VALUE, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static void UpdateAppconfig(SqlInt32 id, SqlString name, SqlString value, SqlString description, SqlString longdescription, SqlString validationrule, SqlString validationmethod, SqlString domain, SqlString client, SqlString row_source_type, SqlString row_source, SqlString user, SqlString program, out SqlInt32 RETURN_VALUE, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					SqlCommon.UpdateAppconfig(id, name, value, description, longdescription, validationrule, validationmethod, domain, client, row_source_type, row_source, user, program, out RETURN_VALUE, connection, transaction);

					break;

				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Common.UpdateAppconfig for this provider: "+providerName);
					throw new ApplicationException("No implementation of Common.UpdateAppconfig for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static void LookupColumnRename(SqlString tablename_columname, SqlString newcolumnname, SqlString altertype)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			Common.LookupColumnRename(tablename_columname, newcolumnname, altertype, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static void LookupColumnRename(SqlString tablename_columname, SqlString newcolumnname, SqlString altertype, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			Common.LookupColumnRename(tablename_columname, newcolumnname, altertype, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static void LookupColumnRename(SqlString tablename_columname, SqlString newcolumnname, SqlString altertype, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					SqlCommon.LookupColumnRename(tablename_columname, newcolumnname, altertype, connection, transaction);

					break;

				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Common.LookupColumnRename for this provider: "+providerName);
					throw new ApplicationException("No implementation of Common.LookupColumnRename for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetUserConfig(SqlInt32 FK_SecurityUser, SqlInt32 FK_Org, SqlInt32 FK_UserConfigType, SqlString UserConfigShortName, SqlBoolean GetFromAdmin)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return Common.GetUserConfig(FK_SecurityUser, FK_Org, FK_UserConfigType, UserConfigShortName, GetFromAdmin, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetUserConfig(SqlInt32 FK_SecurityUser, SqlInt32 FK_Org, SqlInt32 FK_UserConfigType, SqlString UserConfigShortName, SqlBoolean GetFromAdmin, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return Common.GetUserConfig(FK_SecurityUser, FK_Org, FK_UserConfigType, UserConfigShortName, GetFromAdmin, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetUserConfig(SqlInt32 FK_SecurityUser, SqlInt32 FK_Org, SqlInt32 FK_UserConfigType, SqlString UserConfigShortName, SqlBoolean GetFromAdmin, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlCommon.GetUserConfig(FK_SecurityUser, FK_Org, FK_UserConfigType, UserConfigShortName, GetFromAdmin, connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Common.GetUserConfig for this provider: "+providerName);
					throw new ApplicationException("No implementation of Common.GetUserConfig for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
        public static void UpdateUserConfig(SqlInt32 PK_UserConfig, SqlInt32 FK_UserConfigType, SqlInt32 FK_SecurityUser, SqlInt32 FK_Org, SqlString ConfigXml, SqlString CreateUser, SqlString CreateProgram, SqlString UserConfigShortName, out SqlInt32 RETURN_VALUE)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
            Common.UpdateUserConfig(PK_UserConfig, FK_UserConfigType, FK_SecurityUser, FK_Org, ConfigXml, CreateUser, CreateProgram, UserConfigShortName, out RETURN_VALUE, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
        public static void UpdateUserConfig(SqlInt32 PK_UserConfig, SqlInt32 FK_UserConfigType, SqlInt32 FK_SecurityUser, SqlInt32 FK_Org, SqlString ConfigXml, SqlString CreateUser, SqlString CreateProgram, SqlString UserConfigShortName, out SqlInt32 RETURN_VALUE, IDbConnection connection)
		{
			IDbTransaction transaction = null;
            Common.UpdateUserConfig(PK_UserConfig, FK_UserConfigType, FK_SecurityUser, FK_Org, ConfigXml, CreateUser, CreateProgram, UserConfigShortName, out RETURN_VALUE, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
        public static void UpdateUserConfig(SqlInt32 PK_UserConfig, SqlInt32 FK_UserConfigType, SqlInt32 FK_SecurityUser, SqlInt32 FK_Org, SqlString ConfigXml, SqlString CreateUser, SqlString CreateProgram, SqlString UserConfigShortName, out SqlInt32 RETURN_VALUE, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
                    SqlCommon.UpdateUserConfig(PK_UserConfig, FK_UserConfigType, FK_SecurityUser, FK_Org, ConfigXml, CreateUser, CreateProgram, UserConfigShortName, out RETURN_VALUE, connection, transaction);

					break;

				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Common.UpdateUserConfig for this provider: "+providerName);
					throw new ApplicationException("No implementation of Common.UpdateUserConfig for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static void DeleteUserConfig(SqlInt32 FK_SecurityUser, SqlInt32 FK_Org, SqlInt32 FK_UserConfigType, SqlString UserConfigShortName)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			Common.DeleteUserConfig(FK_SecurityUser, FK_Org, FK_UserConfigType, UserConfigShortName, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static void DeleteUserConfig(SqlInt32 FK_SecurityUser, SqlInt32 FK_Org, SqlInt32 FK_UserConfigType, SqlString UserConfigShortName, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			Common.DeleteUserConfig(FK_SecurityUser, FK_Org, FK_UserConfigType, UserConfigShortName, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static void DeleteUserConfig(SqlInt32 FK_SecurityUser, SqlInt32 FK_Org, SqlInt32 FK_UserConfigType, SqlString UserConfigShortName, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					SqlCommon.DeleteUserConfig(FK_SecurityUser, FK_Org, FK_UserConfigType, UserConfigShortName, connection, transaction);

					break;

				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Common.DeleteUserConfig for this provider: "+providerName);
					throw new ApplicationException("No implementation of Common.DeleteUserConfig for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable ObjectDependency(SqlString objectName, SqlInt32 objectID)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return Common.ObjectDependency(objectName, objectID, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable ObjectDependency(SqlString objectName, SqlInt32 objectID, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return Common.ObjectDependency(objectName, objectID, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable ObjectDependency(SqlString objectName, SqlInt32 objectID, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlCommon.ObjectDependency(objectName, objectID, connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Common.ObjectDependency for this provider: "+providerName);
					throw new ApplicationException("No implementation of Common.ObjectDependency for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetItemMetaDataActions()
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return Common.GetItemMetaDataActions(connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetItemMetaDataActions(IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return Common.GetItemMetaDataActions(connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetItemMetaDataActions(IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlCommon.GetItemMetaDataActions(connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Common.GetItemMetaDataActions for this provider: "+providerName);
					throw new ApplicationException("No implementation of Common.GetItemMetaDataActions for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static void AddInboxMessage(SqlString vchrMessageType, SqlString vchrMessageCategory, SqlString vchrMessageID, SqlInt32 intToUserID, SqlString vchrSubject, SqlString ntextContent, SqlString nvchrStatus, SqlString chrApprovalFlag, SqlString vchrUserID, SqlString vchrProgramName, out SqlInt32 RETURN_VALUE)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			Common.AddInboxMessage(vchrMessageType, vchrMessageCategory, vchrMessageID, intToUserID, vchrSubject, ntextContent, nvchrStatus, chrApprovalFlag, vchrUserID, vchrProgramName, out RETURN_VALUE, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static void AddInboxMessage(SqlString vchrMessageType, SqlString vchrMessageCategory, SqlString vchrMessageID, SqlInt32 intToUserID, SqlString vchrSubject, SqlString ntextContent, SqlString nvchrStatus, SqlString chrApprovalFlag, SqlString vchrUserID, SqlString vchrProgramName, out SqlInt32 RETURN_VALUE, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			Common.AddInboxMessage(vchrMessageType, vchrMessageCategory, vchrMessageID, intToUserID, vchrSubject, ntextContent, nvchrStatus, chrApprovalFlag, vchrUserID, vchrProgramName, out RETURN_VALUE, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static void AddInboxMessage(SqlString vchrMessageType, SqlString vchrMessageCategory, SqlString vchrMessageID, SqlInt32 intToUserID, SqlString vchrSubject, SqlString ntextContent, SqlString nvchrStatus, SqlString chrApprovalFlag, SqlString vchrUserID, SqlString vchrProgramName, out SqlInt32 RETURN_VALUE, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					SqlCommon.AddInboxMessage(vchrMessageType, vchrMessageCategory, vchrMessageID, intToUserID, vchrSubject, ntextContent, nvchrStatus, chrApprovalFlag, vchrUserID, vchrProgramName, out RETURN_VALUE, connection, transaction);

					break;

				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Common.AddInboxMessage for this provider: "+providerName);
					throw new ApplicationException("No implementation of Common.AddInboxMessage for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static void UpdateInboxMessage(SqlString vchrMessageID, SqlString ntextContent, SqlString nvchrStatus, SqlInt32 intToUserID, SqlString vchrOverwrite, SqlString vchrUserID, SqlString vchrProgramName, out SqlInt32 RETURN_VALUE)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			Common.UpdateInboxMessage(vchrMessageID, ntextContent, nvchrStatus, intToUserID, vchrOverwrite, vchrUserID, vchrProgramName, out RETURN_VALUE, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static void UpdateInboxMessage(SqlString vchrMessageID, SqlString ntextContent, SqlString nvchrStatus, SqlInt32 intToUserID, SqlString vchrOverwrite, SqlString vchrUserID, SqlString vchrProgramName, out SqlInt32 RETURN_VALUE, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			Common.UpdateInboxMessage(vchrMessageID, ntextContent, nvchrStatus, intToUserID, vchrOverwrite, vchrUserID, vchrProgramName, out RETURN_VALUE, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static void UpdateInboxMessage(SqlString vchrMessageID, SqlString ntextContent, SqlString nvchrStatus, SqlInt32 intToUserID, SqlString vchrOverwrite, SqlString vchrUserID, SqlString vchrProgramName, out SqlInt32 RETURN_VALUE, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					SqlCommon.UpdateInboxMessage(vchrMessageID, ntextContent, nvchrStatus, intToUserID, vchrOverwrite, vchrUserID, vchrProgramName, out RETURN_VALUE, connection, transaction);

					break;

				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Common.UpdateInboxMessage for this provider: "+providerName);
					throw new ApplicationException("No implementation of Common.UpdateInboxMessage for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static void AddStatus(SqlString nvchrShortName, SqlString nvchrLongName, SqlString vchrUserID, SqlString vchrProgramName, out SqlInt32 RETURN_VALUE)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			Common.AddStatus(nvchrShortName, nvchrLongName, vchrUserID, vchrProgramName, out RETURN_VALUE, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static void AddStatus(SqlString nvchrShortName, SqlString nvchrLongName, SqlString vchrUserID, SqlString vchrProgramName, out SqlInt32 RETURN_VALUE, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			Common.AddStatus(nvchrShortName, nvchrLongName, vchrUserID, vchrProgramName, out RETURN_VALUE, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static void AddStatus(SqlString nvchrShortName, SqlString nvchrLongName, SqlString vchrUserID, SqlString vchrProgramName, out SqlInt32 RETURN_VALUE, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					SqlCommon.AddStatus(nvchrShortName, nvchrLongName, vchrUserID, vchrProgramName, out RETURN_VALUE, connection, transaction);

					break;

				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Common.AddStatus for this provider: "+providerName);
					throw new ApplicationException("No implementation of Common.AddStatus for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static void UpdateStatus(SqlInt32 PK_Status, SqlString nvchrShortName, SqlString nvchrLongName, SqlString vchrUserID, SqlString vchrProgramName)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			Common.UpdateStatus(PK_Status, nvchrShortName, nvchrLongName, vchrUserID, vchrProgramName, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static void UpdateStatus(SqlInt32 PK_Status, SqlString nvchrShortName, SqlString nvchrLongName, SqlString vchrUserID, SqlString vchrProgramName, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			Common.UpdateStatus(PK_Status, nvchrShortName, nvchrLongName, vchrUserID, vchrProgramName, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static void UpdateStatus(SqlInt32 PK_Status, SqlString nvchrShortName, SqlString nvchrLongName, SqlString vchrUserID, SqlString vchrProgramName, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					SqlCommon.UpdateStatus(PK_Status, nvchrShortName, nvchrLongName, vchrUserID, vchrProgramName, connection, transaction);

					break;

				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Common.UpdateStatus for this provider: "+providerName);
					throw new ApplicationException("No implementation of Common.UpdateStatus for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static void DeleteStatus(SqlInt32 PK_Status, out SqlInt32 RETURN_VALUE)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			Common.DeleteStatus(PK_Status, out RETURN_VALUE, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static void DeleteStatus(SqlInt32 PK_Status, out SqlInt32 RETURN_VALUE, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			Common.DeleteStatus(PK_Status, out RETURN_VALUE, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static void DeleteStatus(SqlInt32 PK_Status, out SqlInt32 RETURN_VALUE, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					SqlCommon.DeleteStatus(PK_Status, out RETURN_VALUE, connection, transaction);

					break;

				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Common.DeleteStatus for this provider: "+providerName);
					throw new ApplicationException("No implementation of Common.DeleteStatus for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetStatuses()
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return Common.GetStatuses(connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetStatuses(IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return Common.GetStatuses(connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetStatuses(IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlCommon.GetStatuses(connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Common.GetStatuses for this provider: "+providerName);
					throw new ApplicationException("No implementation of Common.GetStatuses for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetStatusesByType(SqlInt32 intStatusType)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return Common.GetStatusesByType(intStatusType, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetStatusesByType(SqlInt32 intStatusType, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return Common.GetStatusesByType(intStatusType, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetStatusesByType(SqlInt32 intStatusType, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlCommon.GetStatusesByType(intStatusType, connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Common.GetStatusesByType for this provider: "+providerName);
					throw new ApplicationException("No implementation of Common.GetStatusesByType for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static void ObjectExists(SqlString Object_Name, SqlString Object_Type, SqlInt32 Object_ID, SqlString Object_Parent_Name, SqlInt32 Object_Parent_ID, SqlBoolean Raise_Error, ref SqlString Error_Message, out SqlInt32 RETURN_VALUE)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			Common.ObjectExists(Object_Name, Object_Type, Object_ID, Object_Parent_Name, Object_Parent_ID, Raise_Error, ref Error_Message, out RETURN_VALUE, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static void ObjectExists(SqlString Object_Name, SqlString Object_Type, SqlInt32 Object_ID, SqlString Object_Parent_Name, SqlInt32 Object_Parent_ID, SqlBoolean Raise_Error, ref SqlString Error_Message, out SqlInt32 RETURN_VALUE, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			Common.ObjectExists(Object_Name, Object_Type, Object_ID, Object_Parent_Name, Object_Parent_ID, Raise_Error, ref Error_Message, out RETURN_VALUE, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static void ObjectExists(SqlString Object_Name, SqlString Object_Type, SqlInt32 Object_ID, SqlString Object_Parent_Name, SqlInt32 Object_Parent_ID, SqlBoolean Raise_Error, ref SqlString Error_Message, out SqlInt32 RETURN_VALUE, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					SqlCommon.ObjectExists(Object_Name, Object_Type, Object_ID, Object_Parent_Name, Object_Parent_ID, Raise_Error, ref Error_Message, out RETURN_VALUE, connection, transaction);

					break;

				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Common.ObjectExists for this provider: "+providerName);
					throw new ApplicationException("No implementation of Common.ObjectExists for this provider: "+providerName);
			}
		}

	}
}		
