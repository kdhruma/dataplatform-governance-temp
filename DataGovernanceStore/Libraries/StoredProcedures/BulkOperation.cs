
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
	public class BulkOperation
	{
		/// <summary>
		/// Private constructor
		/// </summary>		
		private BulkOperation()
		{
		}
		
		
		/// <summary>
        /// 
        /// </summary>
		public static string ExtractBulkOperationAttributeMetaData(SqlString vchrTargetUserLogin, SqlString vchrUserLogin, SqlString inputDataMode, SqlString selectedNodeTypes, SqlString txtXML, SqlBoolean bitUseDraftTax)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return BulkOperation.ExtractBulkOperationAttributeMetaData(vchrTargetUserLogin, vchrUserLogin, inputDataMode, selectedNodeTypes, txtXML, bitUseDraftTax, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static string ExtractBulkOperationAttributeMetaData(SqlString vchrTargetUserLogin, SqlString vchrUserLogin, SqlString inputDataMode, SqlString selectedNodeTypes, SqlString txtXML, SqlBoolean bitUseDraftTax, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return BulkOperation.ExtractBulkOperationAttributeMetaData(vchrTargetUserLogin, vchrUserLogin, inputDataMode, selectedNodeTypes, txtXML, bitUseDraftTax, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static string ExtractBulkOperationAttributeMetaData(SqlString vchrTargetUserLogin, SqlString vchrUserLogin, SqlString inputDataMode, SqlString selectedNodeTypes, SqlString txtXML, SqlBoolean bitUseDraftTax, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlBulkOperation.ExtractBulkOperationAttributeMetaData(vchrTargetUserLogin, vchrUserLogin, inputDataMode, selectedNodeTypes, txtXML, bitUseDraftTax, connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of BulkOperation.ExtractBulkOperationAttributeMetaData for this provider: "+providerName);
					throw new ApplicationException("No implementation of BulkOperation.ExtractBulkOperationAttributeMetaData for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static string PerformBulkOperation(SqlString vchrUserLogin, SqlString txtXML, SqlString vchrProgramName, SqlInt32 JobServiceID, SqlInt32 batchSize,SqlInt32 systemDataLocale)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return BulkOperation.PerformBulkOperation(vchrUserLogin, txtXML, vchrProgramName, JobServiceID, batchSize, systemDataLocale,connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
        public static string PerformBulkOperation(SqlString vchrUserLogin, SqlString txtXML, SqlString vchrProgramName, SqlInt32 JobServiceID, SqlInt32 batchSize, IDbConnection connection, SqlInt32 systemDataLocale)
		{
			IDbTransaction transaction = null;
			return BulkOperation.PerformBulkOperation(vchrUserLogin, txtXML, vchrProgramName, JobServiceID, batchSize,systemDataLocale , connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
        public static string PerformBulkOperation(SqlString vchrUserLogin, SqlString txtXML, SqlString vchrProgramName, SqlInt32 JobServiceID, SqlInt32 batchSize, SqlInt32 systemDataLocale, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlBulkOperation.PerformBulkOperation(vchrUserLogin, txtXML, vchrProgramName, JobServiceID, batchSize,systemDataLocale, connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of BulkOperation.PerformBulkOperation for this provider: "+providerName);
					throw new ApplicationException("No implementation of BulkOperation.PerformBulkOperation for this provider: "+providerName);
			}
		}

	}
}		
