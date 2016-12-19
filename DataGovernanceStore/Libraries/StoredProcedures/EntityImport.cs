
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
	public class EntityImport
	{
		/// <summary>
		/// Private constructor
		/// </summary>		
		private EntityImport()
		{
		}
		
		
		/// <summary>
        /// 
        /// </summary>
		public static void StageData(SqlXml dataXml, SqlInt32 jobId, SqlString rsXmlSchema, out SqlInt32 RETURN_VALUE)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			EntityImport.StageData(dataXml, jobId, rsXmlSchema, out RETURN_VALUE, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static void StageData(SqlXml dataXml, SqlInt32 jobId, SqlString rsXmlSchema, out SqlInt32 RETURN_VALUE, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			EntityImport.StageData(dataXml, jobId, rsXmlSchema, out RETURN_VALUE, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static void StageData(SqlXml dataXml, SqlInt32 jobId, SqlString rsXmlSchema, out SqlInt32 RETURN_VALUE, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					SqlEntityImport.StageData(dataXml, jobId, rsXmlSchema, out RETURN_VALUE, connection, transaction);

					break;

				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of EntityImport.StageData for this provider: "+providerName);
					throw new ApplicationException("No implementation of EntityImport.StageData for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static void Process(SqlInt32 FK_JobService, SqlString UserLogin, SqlBoolean ToLoadCategories, SqlBoolean SchemaValidation, out SqlInt32 RETURN_VALUE)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			EntityImport.Process(FK_JobService, UserLogin, ToLoadCategories, SchemaValidation, out RETURN_VALUE, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static void Process(SqlInt32 FK_JobService, SqlString UserLogin, SqlBoolean ToLoadCategories, SqlBoolean SchemaValidation, out SqlInt32 RETURN_VALUE, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			EntityImport.Process(FK_JobService, UserLogin, ToLoadCategories, SchemaValidation, out RETURN_VALUE, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static void Process(SqlInt32 FK_JobService, SqlString UserLogin, SqlBoolean ToLoadCategories, SqlBoolean SchemaValidation, out SqlInt32 RETURN_VALUE, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					SqlEntityImport.Process(FK_JobService, UserLogin, ToLoadCategories, SchemaValidation, out RETURN_VALUE, connection, transaction);

					break;

				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of EntityImport.Process for this provider: "+providerName);
					throw new ApplicationException("No implementation of EntityImport.Process for this provider: "+providerName);
			}
		}

	}
}		
