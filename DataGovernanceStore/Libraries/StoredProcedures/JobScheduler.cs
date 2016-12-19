
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
	/// Job Scheduler API
	/// </summary>
	public class JobScheduler
	{
		/// <summary>
		/// Private constructor
		/// </summary>		
		private JobScheduler()
		{
		}
		
		
		/// <summary>
        /// Adds a new job schedule
        /// </summary>
		public static void AddJobSchedule(SqlString nvchrShortName, SqlString nvchrLongName, SqlString txtScheduleData, SqlBoolean bitEnabled, SqlString vchrComputerName, SqlString vchrProfiles, SqlString nvchrUser, SqlString nvchrProgram)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			JobScheduler.AddJobSchedule(nvchrShortName, nvchrLongName, txtScheduleData, bitEnabled, vchrComputerName, vchrProfiles, nvchrUser, nvchrProgram, connection, transaction);
		}

		/// <summary>
        /// Adds a new job schedule
        /// </summary>
		public static void AddJobSchedule(SqlString nvchrShortName, SqlString nvchrLongName, SqlString txtScheduleData, SqlBoolean bitEnabled, SqlString vchrComputerName, SqlString vchrProfiles, SqlString nvchrUser, SqlString nvchrProgram, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			JobScheduler.AddJobSchedule(nvchrShortName, nvchrLongName, txtScheduleData, bitEnabled, vchrComputerName, vchrProfiles, nvchrUser, nvchrProgram, connection, transaction);
		}

		/// <summary>
        /// Adds a new job schedule
        /// </summary>
		public static void AddJobSchedule(SqlString nvchrShortName, SqlString nvchrLongName, SqlString txtScheduleData, SqlBoolean bitEnabled, SqlString vchrComputerName, SqlString vchrProfiles, SqlString nvchrUser, SqlString nvchrProgram, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					SqlJobScheduler.AddJobSchedule(nvchrShortName, nvchrLongName, txtScheduleData, bitEnabled, vchrComputerName, vchrProfiles, nvchrUser, nvchrProgram, connection, transaction);

					break;

				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of JobScheduler.AddJobSchedule for this provider: "+providerName);
					throw new ApplicationException("No implementation of JobScheduler.AddJobSchedule for this provider: "+providerName);
			}
		}

		/// <summary>
        /// Deletes a job schedule
        /// </summary>
		public static void DeleteJobSchedule(SqlString PK_JobSchedule)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			JobScheduler.DeleteJobSchedule(PK_JobSchedule, connection, transaction);
		}

		/// <summary>
        /// Deletes a job schedule
        /// </summary>
		public static void DeleteJobSchedule(SqlString PK_JobSchedule, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			JobScheduler.DeleteJobSchedule(PK_JobSchedule, connection, transaction);
		}

		/// <summary>
        /// Deletes a job schedule
        /// </summary>
		public static void DeleteJobSchedule(SqlString PK_JobSchedule, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					SqlJobScheduler.DeleteJobSchedule(PK_JobSchedule, connection, transaction);

					break;

				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of JobScheduler.DeleteJobSchedule for this provider: "+providerName);
					throw new ApplicationException("No implementation of JobScheduler.DeleteJobSchedule for this provider: "+providerName);
			}
		}

		/// <summary>
        /// Gets one or all job schedules
        /// </summary>
		public static DataSet GetJobSchedule(SqlInt32 PK_JobSchedule)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return JobScheduler.GetJobSchedule(PK_JobSchedule, connection, transaction);
		}

		/// <summary>
        /// Gets one or all job schedules
        /// </summary>
		public static DataSet GetJobSchedule(SqlInt32 PK_JobSchedule, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return JobScheduler.GetJobSchedule(PK_JobSchedule, connection, transaction);
		}

		/// <summary>
        /// Gets one or all job schedules
        /// </summary>
		public static DataSet GetJobSchedule(SqlInt32 PK_JobSchedule, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlJobScheduler.GetJobSchedule(PK_JobSchedule, connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of JobScheduler.GetJobSchedule for this provider: "+providerName);
					throw new ApplicationException("No implementation of JobScheduler.GetJobSchedule for this provider: "+providerName);
			}
		}

		/// <summary>
        /// Updates a job schedule
        /// </summary>
		public static void UpdateJobSchedule(SqlInt32 PK_JobSchedule, SqlString nvchrShortName, SqlString nvchrLongName, SqlString txtScheduleData, SqlBoolean bitEnabled, SqlString vchrComputerName, SqlString vchrProfiles, SqlString nvchrUser, SqlString nvchrProgram)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			JobScheduler.UpdateJobSchedule(PK_JobSchedule, nvchrShortName, nvchrLongName, txtScheduleData, bitEnabled, vchrComputerName, vchrProfiles, nvchrUser, nvchrProgram, connection, transaction);
		}

		/// <summary>
        /// Updates a job schedule
        /// </summary>
		public static void UpdateJobSchedule(SqlInt32 PK_JobSchedule, SqlString nvchrShortName, SqlString nvchrLongName, SqlString txtScheduleData, SqlBoolean bitEnabled, SqlString vchrComputerName, SqlString vchrProfiles, SqlString nvchrUser, SqlString nvchrProgram, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			JobScheduler.UpdateJobSchedule(PK_JobSchedule, nvchrShortName, nvchrLongName, txtScheduleData, bitEnabled, vchrComputerName, vchrProfiles, nvchrUser, nvchrProgram, connection, transaction);
		}

		/// <summary>
        /// Updates a job schedule
        /// </summary>
		public static void UpdateJobSchedule(SqlInt32 PK_JobSchedule, SqlString nvchrShortName, SqlString nvchrLongName, SqlString txtScheduleData, SqlBoolean bitEnabled, SqlString vchrComputerName, SqlString vchrProfiles, SqlString nvchrUser, SqlString nvchrProgram, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					SqlJobScheduler.UpdateJobSchedule(PK_JobSchedule, nvchrShortName, nvchrLongName, txtScheduleData, bitEnabled, vchrComputerName, vchrProfiles, nvchrUser, nvchrProgram, connection, transaction);

					break;

				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of JobScheduler.UpdateJobSchedule for this provider: "+providerName);
					throw new ApplicationException("No implementation of JobScheduler.UpdateJobSchedule for this provider: "+providerName);
			}
		}

		/// <summary>
        /// Updates the status of a job schedule
        /// </summary>
		public static void UpdateJobScheduleStatus(SqlInt32 PK_JobSchedule, SqlString nvchrLastRunStatus, SqlDateTime dtLastRunDate, SqlDateTime dtNextRunDate)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			JobScheduler.UpdateJobScheduleStatus(PK_JobSchedule, nvchrLastRunStatus, dtLastRunDate, dtNextRunDate, connection, transaction);
		}

		/// <summary>
        /// Updates the status of a job schedule
        /// </summary>
		public static void UpdateJobScheduleStatus(SqlInt32 PK_JobSchedule, SqlString nvchrLastRunStatus, SqlDateTime dtLastRunDate, SqlDateTime dtNextRunDate, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			JobScheduler.UpdateJobScheduleStatus(PK_JobSchedule, nvchrLastRunStatus, dtLastRunDate, dtNextRunDate, connection, transaction);
		}

		/// <summary>
        /// Updates the status of a job schedule
        /// </summary>
		public static void UpdateJobScheduleStatus(SqlInt32 PK_JobSchedule, SqlString nvchrLastRunStatus, SqlDateTime dtLastRunDate, SqlDateTime dtNextRunDate, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					SqlJobScheduler.UpdateJobScheduleStatus(PK_JobSchedule, nvchrLastRunStatus, dtLastRunDate, dtNextRunDate, connection, transaction);

					break;

				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of JobScheduler.UpdateJobScheduleStatus for this provider: "+providerName);
					throw new ApplicationException("No implementation of JobScheduler.UpdateJobScheduleStatus for this provider: "+providerName);
			}
		}

	}
}		
