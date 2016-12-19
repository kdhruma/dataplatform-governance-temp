
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
	public class Synonyms
	{
		/// <summary>
		/// Private constructor
		/// </summary>		
		private Synonyms()
		{
		}
		
		
		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetCategoryKeywordsByCatalogID(SqlInt32 intCatalogID, SqlInt32 intLocaleID, SqlInt32 intCustomerID)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return Synonyms.GetCategoryKeywordsByCatalogID(intCatalogID, intLocaleID, intCustomerID, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetCategoryKeywordsByCatalogID(SqlInt32 intCatalogID, SqlInt32 intLocaleID, SqlInt32 intCustomerID, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return Synonyms.GetCategoryKeywordsByCatalogID(intCatalogID, intLocaleID, intCustomerID, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetCategoryKeywordsByCatalogID(SqlInt32 intCatalogID, SqlInt32 intLocaleID, SqlInt32 intCustomerID, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlSynonyms.GetCategoryKeywordsByCatalogID(intCatalogID, intLocaleID, intCustomerID, connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Synonyms.GetCategoryKeywordsByCatalogID for this provider: "+providerName);
					throw new ApplicationException("No implementation of Synonyms.GetCategoryKeywordsByCatalogID for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetAllSynonymList()
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return Synonyms.GetAllSynonymList(connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetAllSynonymList(IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return Synonyms.GetAllSynonymList(connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetAllSynonymList(IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlSynonyms.GetAllSynonymList(connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Synonyms.GetAllSynonymList for this provider: "+providerName);
					throw new ApplicationException("No implementation of Synonyms.GetAllSynonymList for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetAllSynonymListByID(SqlInt32 PK_SynonymList)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return Synonyms.GetAllSynonymListByID(PK_SynonymList, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetAllSynonymListByID(SqlInt32 PK_SynonymList, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return Synonyms.GetAllSynonymListByID(PK_SynonymList, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetAllSynonymListByID(SqlInt32 PK_SynonymList, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlSynonyms.GetAllSynonymListByID(PK_SynonymList, connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Synonyms.GetAllSynonymListByID for this provider: "+providerName);
					throw new ApplicationException("No implementation of Synonyms.GetAllSynonymListByID for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetAllSynonyms()
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return Synonyms.GetAllSynonyms(connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetAllSynonyms(IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return Synonyms.GetAllSynonyms(connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetAllSynonyms(IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlSynonyms.GetAllSynonyms(connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Synonyms.GetAllSynonyms for this provider: "+providerName);
					throw new ApplicationException("No implementation of Synonyms.GetAllSynonyms for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetAllSynonymsByID(SqlInt32 FK_SynonymList)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return Synonyms.GetAllSynonymsByID(FK_SynonymList, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetAllSynonymsByID(SqlInt32 FK_SynonymList, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return Synonyms.GetAllSynonymsByID(FK_SynonymList, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetAllSynonymsByID(SqlInt32 FK_SynonymList, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlSynonyms.GetAllSynonymsByID(FK_SynonymList, connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Synonyms.GetAllSynonymsByID for this provider: "+providerName);
					throw new ApplicationException("No implementation of Synonyms.GetAllSynonymsByID for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetAllSynonymsByKeyword(SqlInt32 FK_SynonymList, SqlString Keyword)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return Synonyms.GetAllSynonymsByKeyword(FK_SynonymList, Keyword, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetAllSynonymsByKeyword(SqlInt32 FK_SynonymList, SqlString Keyword, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return Synonyms.GetAllSynonymsByKeyword(FK_SynonymList, Keyword, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetAllSynonymsByKeyword(SqlInt32 FK_SynonymList, SqlString Keyword, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlSynonyms.GetAllSynonymsByKeyword(FK_SynonymList, Keyword, connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Synonyms.GetAllSynonymsByKeyword for this provider: "+providerName);
					throw new ApplicationException("No implementation of Synonyms.GetAllSynonymsByKeyword for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static void CreateSynonymList(SqlString Shortname, SqlString Description, out SqlInt32 RETURN_VALUE)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			Synonyms.CreateSynonymList(Shortname, Description, out RETURN_VALUE, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static void CreateSynonymList(SqlString Shortname, SqlString Description, out SqlInt32 RETURN_VALUE, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			Synonyms.CreateSynonymList(Shortname, Description, out RETURN_VALUE, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static void CreateSynonymList(SqlString Shortname, SqlString Description, out SqlInt32 RETURN_VALUE, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					SqlSynonyms.CreateSynonymList(Shortname, Description, out RETURN_VALUE, connection, transaction);

					break;

				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Synonyms.CreateSynonymList for this provider: "+providerName);
					throw new ApplicationException("No implementation of Synonyms.CreateSynonymList for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static void CreateSynonyms(SqlInt32 PK_SynonymList, SqlString nvchrKeyword, SqlString nvchrSynonyms, out SqlInt32 RETURN_VALUE)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			Synonyms.CreateSynonyms(PK_SynonymList, nvchrKeyword, nvchrSynonyms, out RETURN_VALUE, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static void CreateSynonyms(SqlInt32 PK_SynonymList, SqlString nvchrKeyword, SqlString nvchrSynonyms, out SqlInt32 RETURN_VALUE, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			Synonyms.CreateSynonyms(PK_SynonymList, nvchrKeyword, nvchrSynonyms, out RETURN_VALUE, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static void CreateSynonyms(SqlInt32 PK_SynonymList, SqlString nvchrKeyword, SqlString nvchrSynonyms, out SqlInt32 RETURN_VALUE, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					SqlSynonyms.CreateSynonyms(PK_SynonymList, nvchrKeyword, nvchrSynonyms, out RETURN_VALUE, connection, transaction);

					break;

				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Synonyms.CreateSynonyms for this provider: "+providerName);
					throw new ApplicationException("No implementation of Synonyms.CreateSynonyms for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static void UpdateSynonymList(SqlString Shortname, SqlString Description, SqlInt32 PK_SynonymList)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			Synonyms.UpdateSynonymList(Shortname, Description, PK_SynonymList, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static void UpdateSynonymList(SqlString Shortname, SqlString Description, SqlInt32 PK_SynonymList, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			Synonyms.UpdateSynonymList(Shortname, Description, PK_SynonymList, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static void UpdateSynonymList(SqlString Shortname, SqlString Description, SqlInt32 PK_SynonymList, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					SqlSynonyms.UpdateSynonymList(Shortname, Description, PK_SynonymList, connection, transaction);

					break;

				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Synonyms.UpdateSynonymList for this provider: "+providerName);
					throw new ApplicationException("No implementation of Synonyms.UpdateSynonymList for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static void UpdateSynonyms(SqlString nvchrKeyword, SqlString nvchrSynonyms, SqlInt32 PK_Synonym)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			Synonyms.UpdateSynonyms(nvchrKeyword, nvchrSynonyms, PK_Synonym, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static void UpdateSynonyms(SqlString nvchrKeyword, SqlString nvchrSynonyms, SqlInt32 PK_Synonym, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			Synonyms.UpdateSynonyms(nvchrKeyword, nvchrSynonyms, PK_Synonym, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static void UpdateSynonyms(SqlString nvchrKeyword, SqlString nvchrSynonyms, SqlInt32 PK_Synonym, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					SqlSynonyms.UpdateSynonyms(nvchrKeyword, nvchrSynonyms, PK_Synonym, connection, transaction);

					break;

				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Synonyms.UpdateSynonyms for this provider: "+providerName);
					throw new ApplicationException("No implementation of Synonyms.UpdateSynonyms for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static void DeleteSynonymList(SqlInt32 PK_SynonymList)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			Synonyms.DeleteSynonymList(PK_SynonymList, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static void DeleteSynonymList(SqlInt32 PK_SynonymList, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			Synonyms.DeleteSynonymList(PK_SynonymList, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static void DeleteSynonymList(SqlInt32 PK_SynonymList, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					SqlSynonyms.DeleteSynonymList(PK_SynonymList, connection, transaction);

					break;

				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Synonyms.DeleteSynonymList for this provider: "+providerName);
					throw new ApplicationException("No implementation of Synonyms.DeleteSynonymList for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static void DeleteSynonyms(SqlInt32 PK_Synonym)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			Synonyms.DeleteSynonyms(PK_Synonym, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static void DeleteSynonyms(SqlInt32 PK_Synonym, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			Synonyms.DeleteSynonyms(PK_Synonym, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static void DeleteSynonyms(SqlInt32 PK_Synonym, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					SqlSynonyms.DeleteSynonyms(PK_Synonym, connection, transaction);

					break;

				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Synonyms.DeleteSynonyms for this provider: "+providerName);
					throw new ApplicationException("No implementation of Synonyms.DeleteSynonyms for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetMatchingCategoryByKeywords(SqlInt32 intCatalogID, SqlString nvchrKeyWords, SqlInt32 intLocale, SqlInt32 intCustomerID)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return Synonyms.GetMatchingCategoryByKeywords(intCatalogID, nvchrKeyWords, intLocale, intCustomerID, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetMatchingCategoryByKeywords(SqlInt32 intCatalogID, SqlString nvchrKeyWords, SqlInt32 intLocale, SqlInt32 intCustomerID, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return Synonyms.GetMatchingCategoryByKeywords(intCatalogID, nvchrKeyWords, intLocale, intCustomerID, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetMatchingCategoryByKeywords(SqlInt32 intCatalogID, SqlString nvchrKeyWords, SqlInt32 intLocale, SqlInt32 intCustomerID, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlSynonyms.GetMatchingCategoryByKeywords(intCatalogID, nvchrKeyWords, intLocale, intCustomerID, connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Synonyms.GetMatchingCategoryByKeywords for this provider: "+providerName);
					throw new ApplicationException("No implementation of Synonyms.GetMatchingCategoryByKeywords for this provider: "+providerName);
			}
		}

	}
}		
