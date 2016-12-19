
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
	public class Util
	{
		/// <summary>
		/// Private constructor
		/// </summary>		
		private Util()
		{
		}
		
		
		/// <summary>
        /// 
        /// </summary>
		public static DataTable ExecSql(SqlString sql)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return Util.ExecSql(sql, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable ExecSql(SqlString sql, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return Util.ExecSql(sql, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable ExecSql(SqlString sql, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlUtil.ExecSql(sql, connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Util.ExecSql for this provider: "+providerName);
					throw new ApplicationException("No implementation of Util.ExecSql for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable sp_help(SqlString objname)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return Util.sp_help(objname, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable sp_help(SqlString objname, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return Util.sp_help(objname, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable sp_help(SqlString objname, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlUtil.sp_help(objname, connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Util.sp_help for this provider: "+providerName);
					throw new ApplicationException("No implementation of Util.sp_help for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable EstimateChanges(SqlString ChangeProgram, SqlString CNodeList)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return Util.EstimateChanges(ChangeProgram, CNodeList, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable EstimateChanges(SqlString ChangeProgram, SqlString CNodeList, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return Util.EstimateChanges(ChangeProgram, CNodeList, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable EstimateChanges(SqlString ChangeProgram, SqlString CNodeList, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlUtil.EstimateChanges(ChangeProgram, CNodeList, connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Util.EstimateChanges for this provider: "+providerName);
					throw new ApplicationException("No implementation of Util.EstimateChanges for this provider: "+providerName);
			}
		}

	}
}		
