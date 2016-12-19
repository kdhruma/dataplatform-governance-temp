
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
	public class Denorm
	{
		/// <summary>
		/// Private constructor
		/// </summary>		
		private Denorm()
		{
		}
		
		
		/// <summary>
        /// 
        /// </summary>
		public static DataSet GetCurrentJobStatus()
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return Denorm.GetCurrentJobStatus(connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataSet GetCurrentJobStatus(IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return Denorm.GetCurrentJobStatus(connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataSet GetCurrentJobStatus(IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlDenorm.GetCurrentJobStatus(connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Denorm.GetCurrentJobStatus for this provider: "+providerName);
					throw new ApplicationException("No implementation of Denorm.GetCurrentJobStatus for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetJobType()
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return Denorm.GetJobType(connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetJobType(IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return Denorm.GetJobType(connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetJobType(IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlDenorm.GetJobType(connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Denorm.GetJobType for this provider: "+providerName);
					throw new ApplicationException("No implementation of Denorm.GetJobType for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataSet GetJobList(SqlString JobType, SqlString FromDate, SqlString ToDate, SqlBoolean DisplayNonEmptyJobs, SqlBoolean DisplayErrorJobs, SqlInt32 DisplayRows)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return Denorm.GetJobList(JobType, FromDate, ToDate, DisplayNonEmptyJobs, DisplayErrorJobs, DisplayRows, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataSet GetJobList(SqlString JobType, SqlString FromDate, SqlString ToDate, SqlBoolean DisplayNonEmptyJobs, SqlBoolean DisplayErrorJobs, SqlInt32 DisplayRows, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return Denorm.GetJobList(JobType, FromDate, ToDate, DisplayNonEmptyJobs, DisplayErrorJobs, DisplayRows, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataSet GetJobList(SqlString JobType, SqlString FromDate, SqlString ToDate, SqlBoolean DisplayNonEmptyJobs, SqlBoolean DisplayErrorJobs, SqlInt32 DisplayRows, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlDenorm.GetJobList(JobType, FromDate, ToDate, DisplayNonEmptyJobs, DisplayErrorJobs, DisplayRows, connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Denorm.GetJobList for this provider: "+providerName);
					throw new ApplicationException("No implementation of Denorm.GetJobList for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetJobStepDetails(SqlInt32 JobID)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return Denorm.GetJobStepDetails(JobID, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetJobStepDetails(SqlInt32 JobID, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return Denorm.GetJobStepDetails(JobID, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetJobStepDetails(SqlInt32 JobID, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlDenorm.GetJobStepDetails(JobID, connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Denorm.GetJobStepDetails for this provider: "+providerName);
					throw new ApplicationException("No implementation of Denorm.GetJobStepDetails for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataSet GetJobErrorDetails(SqlString Mode, SqlInt32 JobID, SqlInt32 DisplayRows)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return Denorm.GetJobErrorDetails(Mode, JobID, DisplayRows, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataSet GetJobErrorDetails(SqlString Mode, SqlInt32 JobID, SqlInt32 DisplayRows, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return Denorm.GetJobErrorDetails(Mode, JobID, DisplayRows, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataSet GetJobErrorDetails(SqlString Mode, SqlInt32 JobID, SqlInt32 DisplayRows, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlDenorm.GetJobErrorDetails(Mode, JobID, DisplayRows, connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Denorm.GetJobErrorDetails for this provider: "+providerName);
					throw new ApplicationException("No implementation of Denorm.GetJobErrorDetails for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static void ReleaseDenormLock()
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			Denorm.ReleaseDenormLock(connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static void ReleaseDenormLock(IDbConnection connection)
		{
			IDbTransaction transaction = null;
			Denorm.ReleaseDenormLock(connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static void ReleaseDenormLock(IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					SqlDenorm.ReleaseDenormLock(connection, transaction);

					break;

				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Denorm.ReleaseDenormLock for this provider: "+providerName);
					throw new ApplicationException("No implementation of Denorm.ReleaseDenormLock for this provider: "+providerName);
			}
		}

	}
}		
