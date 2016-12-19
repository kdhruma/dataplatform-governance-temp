
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
	public class AttExtract
	{
		/// <summary>
		/// Private constructor
		/// </summary>		
		private AttExtract()
		{
		}
		
		
		/// <summary>
        /// 
        /// </summary>
		public static void GenerateSequences(SqlInt32 FK_Job)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			AttExtract.GenerateSequences(FK_Job, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static void GenerateSequences(SqlInt32 FK_Job, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			AttExtract.GenerateSequences(FK_Job, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static void GenerateSequences(SqlInt32 FK_Job, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					SqlAttExtract.GenerateSequences(FK_Job, connection, transaction);

					break;

				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of AttExtract.GenerateSequences for this provider: "+providerName);
					throw new ApplicationException("No implementation of AttExtract.GenerateSequences for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static void Extract(SqlInt32 FK_AttExtract_Ruleset, SqlString NodeIds, SqlInt32 FK_Catalog, SqlInt32 FK_Org, SqlInt32 Batch_Size, SqlInt32 Start_Count, SqlInt32 End_Count)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			AttExtract.Extract(FK_AttExtract_Ruleset, NodeIds, FK_Catalog, FK_Org, Batch_Size, Start_Count, End_Count, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static void Extract(SqlInt32 FK_AttExtract_Ruleset, SqlString NodeIds, SqlInt32 FK_Catalog, SqlInt32 FK_Org, SqlInt32 Batch_Size, SqlInt32 Start_Count, SqlInt32 End_Count, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			AttExtract.Extract(FK_AttExtract_Ruleset, NodeIds, FK_Catalog, FK_Org, Batch_Size, Start_Count, End_Count, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static void Extract(SqlInt32 FK_AttExtract_Ruleset, SqlString NodeIds, SqlInt32 FK_Catalog, SqlInt32 FK_Org, SqlInt32 Batch_Size, SqlInt32 Start_Count, SqlInt32 End_Count, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					SqlAttExtract.Extract(FK_AttExtract_Ruleset, NodeIds, FK_Catalog, FK_Org, Batch_Size, Start_Count, End_Count, connection, transaction);

					break;

				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of AttExtract.Extract for this provider: "+providerName);
					throw new ApplicationException("No implementation of AttExtract.Extract for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static void ExtractJob(SqlInt32 FK_Job)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			AttExtract.ExtractJob(FK_Job, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static void ExtractJob(SqlInt32 FK_Job, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			AttExtract.ExtractJob(FK_Job, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static void ExtractJob(SqlInt32 FK_Job, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					SqlAttExtract.ExtractJob(FK_Job, connection, transaction);

					break;

				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of AttExtract.ExtractJob for this provider: "+providerName);
					throw new ApplicationException("No implementation of AttExtract.ExtractJob for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataSet SimulateExtractAttribute(SqlString MatchingPattern, SqlString ExtractionPattern, SqlInt32 Occurrence, SqlString Attrval, SqlString DelimiterList)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return AttExtract.SimulateExtractAttribute(MatchingPattern, ExtractionPattern, Occurrence, Attrval, DelimiterList, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataSet SimulateExtractAttribute(SqlString MatchingPattern, SqlString ExtractionPattern, SqlInt32 Occurrence, SqlString Attrval, SqlString DelimiterList, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return AttExtract.SimulateExtractAttribute(MatchingPattern, ExtractionPattern, Occurrence, Attrval, DelimiterList, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataSet SimulateExtractAttribute(SqlString MatchingPattern, SqlString ExtractionPattern, SqlInt32 Occurrence, SqlString Attrval, SqlString DelimiterList, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlAttExtract.SimulateExtractAttribute(MatchingPattern, ExtractionPattern, Occurrence, Attrval, DelimiterList, connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of AttExtract.SimulateExtractAttribute for this provider: "+providerName);
					throw new ApplicationException("No implementation of AttExtract.SimulateExtractAttribute for this provider: "+providerName);
			}
		}

	}
}		
