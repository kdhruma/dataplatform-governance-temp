
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
	public class Language
	{
		/// <summary>
		/// Private constructor
		/// </summary>		
		private Language()
		{
		}
		
		
		/// <summary>
        /// 
        /// </summary>
		public static string GetLanguages(SqlInt32 intPK_Lang, SqlInt32 intCountFrom, SqlInt32 intCountTo, SqlString vchrSortColumn, SqlString vchrSearchColumn, SqlString vchrSearchParameter, SqlString vchrUserLogin)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return Language.GetLanguages(intPK_Lang, intCountFrom, intCountTo, vchrSortColumn, vchrSearchColumn, vchrSearchParameter, vchrUserLogin, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static string GetLanguages(SqlInt32 intPK_Lang, SqlInt32 intCountFrom, SqlInt32 intCountTo, SqlString vchrSortColumn, SqlString vchrSearchColumn, SqlString vchrSearchParameter, SqlString vchrUserLogin, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return Language.GetLanguages(intPK_Lang, intCountFrom, intCountTo, vchrSortColumn, vchrSearchColumn, vchrSearchParameter, vchrUserLogin, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static string GetLanguages(SqlInt32 intPK_Lang, SqlInt32 intCountFrom, SqlInt32 intCountTo, SqlString vchrSortColumn, SqlString vchrSearchColumn, SqlString vchrSearchParameter, SqlString vchrUserLogin, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlLanguage.GetLanguages(intPK_Lang, intCountFrom, intCountTo, vchrSortColumn, vchrSearchColumn, vchrSearchParameter, vchrUserLogin, connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Language.GetLanguages for this provider: "+providerName);
					throw new ApplicationException("No implementation of Language.GetLanguages for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static void ProcessLanguages(SqlString txtXML, SqlString vchrUserLogin)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			Language.ProcessLanguages(txtXML, vchrUserLogin, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static void ProcessLanguages(SqlString txtXML, SqlString vchrUserLogin, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			Language.ProcessLanguages(txtXML, vchrUserLogin, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static void ProcessLanguages(SqlString txtXML, SqlString vchrUserLogin, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					SqlLanguage.ProcessLanguages(txtXML, vchrUserLogin, connection, transaction);

					break;

				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Language.ProcessLanguages for this provider: "+providerName);
					throw new ApplicationException("No implementation of Language.ProcessLanguages for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static string GetRegions(SqlInt32 intPK_Region, SqlInt32 intCountFrom, SqlInt32 intCountTo, SqlString vchrSortColumn, SqlString vchrSearchColumn, SqlString vchrSearchParameter, SqlString vchrUserLogin)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return Language.GetRegions(intPK_Region, intCountFrom, intCountTo, vchrSortColumn, vchrSearchColumn, vchrSearchParameter, vchrUserLogin, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static string GetRegions(SqlInt32 intPK_Region, SqlInt32 intCountFrom, SqlInt32 intCountTo, SqlString vchrSortColumn, SqlString vchrSearchColumn, SqlString vchrSearchParameter, SqlString vchrUserLogin, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return Language.GetRegions(intPK_Region, intCountFrom, intCountTo, vchrSortColumn, vchrSearchColumn, vchrSearchParameter, vchrUserLogin, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static string GetRegions(SqlInt32 intPK_Region, SqlInt32 intCountFrom, SqlInt32 intCountTo, SqlString vchrSortColumn, SqlString vchrSearchColumn, SqlString vchrSearchParameter, SqlString vchrUserLogin, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlLanguage.GetRegions(intPK_Region, intCountFrom, intCountTo, vchrSortColumn, vchrSearchColumn, vchrSearchParameter, vchrUserLogin, connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Language.GetRegions for this provider: "+providerName);
					throw new ApplicationException("No implementation of Language.GetRegions for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static void ProcessRegions(SqlString txtXML, SqlString vchrUserLogin)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			Language.ProcessRegions(txtXML, vchrUserLogin, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static void ProcessRegions(SqlString txtXML, SqlString vchrUserLogin, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			Language.ProcessRegions(txtXML, vchrUserLogin, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static void ProcessRegions(SqlString txtXML, SqlString vchrUserLogin, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					SqlLanguage.ProcessRegions(txtXML, vchrUserLogin, connection, transaction);

					break;

				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Language.ProcessRegions for this provider: "+providerName);
					throw new ApplicationException("No implementation of Language.ProcessRegions for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static string GetLocales(SqlInt32 intPK_Locale, SqlInt32 intCountFrom, SqlInt32 intCountTo, SqlString vchrSortColumn, SqlString vchrSearchColumn, SqlString vchrSearchParameter, SqlString vchrUserLogin)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return Language.GetLocales(intPK_Locale, intCountFrom, intCountTo, vchrSortColumn, vchrSearchColumn, vchrSearchParameter, vchrUserLogin, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static string GetLocales(SqlInt32 intPK_Locale, SqlInt32 intCountFrom, SqlInt32 intCountTo, SqlString vchrSortColumn, SqlString vchrSearchColumn, SqlString vchrSearchParameter, SqlString vchrUserLogin, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return Language.GetLocales(intPK_Locale, intCountFrom, intCountTo, vchrSortColumn, vchrSearchColumn, vchrSearchParameter, vchrUserLogin, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static string GetLocales(SqlInt32 intPK_Locale, SqlInt32 intCountFrom, SqlInt32 intCountTo, SqlString vchrSortColumn, SqlString vchrSearchColumn, SqlString vchrSearchParameter, SqlString vchrUserLogin, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlLanguage.GetLocales(intPK_Locale, intCountFrom, intCountTo, vchrSortColumn, vchrSearchColumn, vchrSearchParameter, vchrUserLogin, connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Language.GetLocales for this provider: "+providerName);
					throw new ApplicationException("No implementation of Language.GetLocales for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static void ProcessLocales(SqlString txtXML, SqlString vchrUserLogin)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			Language.ProcessLocales(txtXML, vchrUserLogin, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static void ProcessLocales(SqlString txtXML, SqlString vchrUserLogin, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			Language.ProcessLocales(txtXML, vchrUserLogin, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static void ProcessLocales(SqlString txtXML, SqlString vchrUserLogin, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					SqlLanguage.ProcessLocales(txtXML, vchrUserLogin, connection, transaction);

					break;

				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Language.ProcessLocales for this provider: "+providerName);
					throw new ApplicationException("No implementation of Language.ProcessLocales for this provider: "+providerName);
			}
		}

	}
}		
