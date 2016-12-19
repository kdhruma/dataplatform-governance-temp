
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
	public class JobBase
	{
		/// <summary>
		/// Private constructor
		/// </summary>		
		private JobBase()
		{
		}


		/// <summary>
		/// 
		/// </summary>
		public static DataTable GetJobsDT(SqlString nvchrType, SqlString nvchrUsername, SqlString Sql)
		{
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return JobBase.GetJobsDT(nvchrType, nvchrUsername, Sql, connection, transaction);
		}

		/// <summary>
		/// 
		/// </summary>
		public static DataTable GetJobsDT(SqlString nvchrType, SqlString nvchrUsername, SqlString Sql, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return JobBase.GetJobsDT(nvchrType, nvchrUsername, Sql, connection, transaction);
		}

		/// <summary>
		/// 
		/// </summary>
		public static DataTable GetJobsDT(SqlString nvchrType, SqlString nvchrUsername, SqlString Sql, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlJobBase.GetJobsDT(nvchrType, nvchrUsername, Sql, connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of JobBase.GetJobsDT for this provider: " + providerName);
					throw new ApplicationException("No implementation of JobBase.GetJobsDT for this provider: " + providerName);
			}
		}

		/// <summary>
        /// Returns the job XML
		/// </summary>
        /// <param name="IncludeInactive">Indicates whether to include inactive jobs</param>
        /// <param name="JobServiceType">Indicates the job service instance type</param>
        /// <param name="jobServiceName">Indicates the job service instance name</param>
        /// <returns>A String representing the job XML</returns>
        public static String GetJobsXml(SqlBoolean IncludeInactive, SqlInt16 JobServiceType, String jobServiceName)
		{
			IDbConnection connection = null;
			IDbTransaction transaction = null;
            return JobBase.GetJobsXml(IncludeInactive, JobServiceType, jobServiceName, connection, transaction);
		}

		/// <summary>
        /// Returns the job XML
		/// </summary>
        /// <param name="IncludeInactive">Indicates whether to include inactive jobs</param>
        /// <param name="JobServiceType">Indicates the job service instance type</param>
        /// <param name="jobServiceName">Indicates the job service instance name</param>
        /// <param name="connection">Indicates the database connection</param>
        /// <returns>A String representing the job XML</returns>
        public static String GetJobsXml(SqlBoolean IncludeInactive, SqlInt16 JobServiceType, String jobServiceName, IDbConnection connection)
		{
			IDbTransaction transaction = null;
            return JobBase.GetJobsXml(IncludeInactive, JobServiceType, jobServiceName, connection, transaction);
		}

		/// <summary>
        /// Returns the job XML
		/// </summary>
        /// <param name="IncludeInactive">Indicates whether to include inactive jobs</param>
        /// <param name="JobServiceType">Indicates the job service instance type</param>
        /// <param name="jobServiceName">Indicates the job service instance name</param>
        /// <param name="connection">Indicates the database connection</param>
        /// <param name="transaction">Indicates the database transaction</param>
        /// <returns>A String representing the job XML</returns>
        public static String GetJobsXml(SqlBoolean IncludeInactive, SqlInt16 JobServiceType, String jobServiceName, IDbConnection connection, IDbTransaction transaction)
		{
			String providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
                    return SqlJobBase.GetJobsXml(IncludeInactive, JobServiceType, jobServiceName, connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of JobBase.GetJobsXml for this provider: " + providerName);
					throw new ApplicationException("No implementation of JobBase.GetJobsXml for this provider: " + providerName);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public static string GetJobItemXml(SqlInt32 intId)
		{
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return JobBase.GetJobItemXml(intId, connection, transaction);
		}

		/// <summary>
		/// 
		/// </summary>
		public static string GetJobItemXml(SqlInt32 intId, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return JobBase.GetJobItemXml(intId, connection, transaction);
		}

		/// <summary>
		/// 
		/// </summary>
		public static string GetJobItemXml(SqlInt32 intId, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlJobBase.GetJobItemXml(intId, connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of JobBase.GetJobItemXml for this provider: " + providerName);
					throw new ApplicationException("No implementation of JobBase.GetJobItemXml for this provider: " + providerName);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public static void UpdateJobInformation(SqlInt32 intId, SqlString nvchrDescription, SqlString ntextJobData, out SqlInt32 RETURN_VALUE)
		{
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			JobBase.UpdateJobInformation(intId, nvchrDescription, ntextJobData, out RETURN_VALUE, connection, transaction);
		}

		/// <summary>
		/// 
		/// </summary>
		public static void UpdateJobInformation(SqlInt32 intId, SqlString nvchrDescription, SqlString ntextJobData, out SqlInt32 RETURN_VALUE, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			JobBase.UpdateJobInformation(intId, nvchrDescription, ntextJobData, out RETURN_VALUE, connection, transaction);
		}

		/// <summary>
		/// 
		/// </summary>
		public static void UpdateJobInformation(SqlInt32 intId, SqlString nvchrDescription, SqlString ntextJobData, out SqlInt32 RETURN_VALUE, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					SqlJobBase.UpdateJobInformation(intId, nvchrDescription, ntextJobData, out RETURN_VALUE, connection, transaction);

					break;

				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of JobBase.UpdateJobInformation for this provider: " + providerName);
					throw new ApplicationException("No implementation of JobBase.UpdateJobInformation for this provider: " + providerName);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public static void UpdateJobInformation(SqlInt32 jobId, SqlInt32 fileId, SqlString description, SqlString Status, SqlInt32 CountSuccess, SqlInt32 CountFailure, SqlInt32 CountTotal, SqlDateTime TimeStarted, SqlDateTime TimeEnded)
		{
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			JobBase.UpdateJobInformation(jobId, fileId, description, Status, CountSuccess, CountFailure, CountTotal, TimeStarted, TimeEnded, connection, transaction);
		}

		/// <summary>
		/// 
		/// </summary>
		public static void UpdateJobInformation(SqlInt32 jobId, SqlInt32 fileId, SqlString description, SqlString Status, SqlInt32 CountSuccess, SqlInt32 CountFailure, SqlInt32 CountTotal, SqlDateTime TimeStarted, SqlDateTime TimeEnded, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			JobBase.UpdateJobInformation(jobId, fileId, description, Status, CountSuccess, CountFailure, CountTotal, TimeStarted, TimeEnded, connection, transaction);
		}

		/// <summary>
		/// 
		/// </summary>
		public static void UpdateJobInformation(SqlInt32 jobId, SqlInt32 fileId, SqlString description, SqlString Status, SqlInt32 CountSuccess, SqlInt32 CountFailure, SqlInt32 CountTotal, SqlDateTime TimeStarted, SqlDateTime TimeEnded, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					SqlJobBase.UpdateJobInformation(jobId, fileId, description, Status, CountSuccess, CountFailure, CountTotal, TimeStarted, TimeEnded, connection, transaction);

					break;

				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of JobBase.UpdateJobInformation for this provider: " + providerName);
					throw new ApplicationException("No implementation of JobBase.UpdateJobInformation for this provider: " + providerName);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public static void UpdateJobData(SqlInt32 FK_JobService, SqlString XmlNodePath, SqlString XmlAttrName, SqlString XmlAttrValue)
		{
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			JobBase.UpdateJobData(FK_JobService, XmlNodePath, XmlAttrName, XmlAttrValue, connection, transaction);
		}

		/// <summary>
		/// 
		/// </summary>
		public static void UpdateJobData(SqlInt32 FK_JobService, SqlString XmlNodePath, SqlString XmlAttrName, SqlString XmlAttrValue, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			JobBase.UpdateJobData(FK_JobService, XmlNodePath, XmlAttrName, XmlAttrValue, connection, transaction);
		}

		/// <summary>
		/// 
		/// </summary>
		public static void UpdateJobData(SqlInt32 FK_JobService, SqlString XmlNodePath, SqlString XmlAttrName, SqlString XmlAttrValue, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					SqlJobBase.UpdateJobData(FK_JobService, XmlNodePath, XmlAttrName, XmlAttrValue, connection, transaction);

					break;

				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of JobBase.UpdateJobData for this provider: " + providerName);
					throw new ApplicationException("No implementation of JobBase.UpdateJobData for this provider: " + providerName);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public static void UpdateJobData(SqlInt32 FK_JobService, SqlXml xmldata)
		{
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			JobBase.UpdateJobData(FK_JobService, xmldata, connection, transaction);
		}

		/// <summary>
		/// 
		/// </summary>
		public static void UpdateJobData(SqlInt32 FK_JobService, SqlXml xmldata, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			JobBase.UpdateJobData(FK_JobService, xmldata, connection, transaction);
		}

		/// <summary>
		/// 
		/// </summary>
		public static void UpdateJobData(SqlInt32 FK_JobService, SqlXml xmldata, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					SqlJobBase.UpdateJobData(FK_JobService, xmldata, connection, transaction);

					break;

				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of JobBase.UpdateJobData for this provider: " + providerName);
					throw new ApplicationException("No implementation of JobBase.UpdateJobData for this provider: " + providerName);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public static string GetJobData(SqlInt32 FK_JobService)
		{
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return JobBase.GetJobData(FK_JobService, connection, transaction);
		}

		/// <summary>
		/// 
		/// </summary>
		public static string GetJobData(SqlInt32 FK_JobService, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return JobBase.GetJobData(FK_JobService, connection, transaction);
		}

		/// <summary>
		/// 
		/// </summary>
		public static string GetJobData(SqlInt32 FK_JobService, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlJobBase.GetJobData(FK_JobService, connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of JobBase.GetJobData for this provider: " + providerName);
					throw new ApplicationException("No implementation of JobBase.GetJobData for this provider: " + providerName);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public static void ImportProductUpdateJobInfo(SqlInt32 FK_JobService, SqlInt32 TotalBatches, SqlInt32 BatchNumber, SqlInt32 NextItem, SqlDateTime StartTime, SqlDateTime EndTime, SqlDateTime BatchStartTime, SqlDateTime BatchEndTime, SqlString stepName)
		{
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			JobBase.ImportProductUpdateJobInfo(FK_JobService, TotalBatches, BatchNumber, NextItem, StartTime, EndTime, BatchStartTime, BatchEndTime, stepName, connection, transaction);
		}

		/// <summary>
		/// 
		/// </summary>
		public static void ImportProductUpdateJobInfo(SqlInt32 FK_JobService, SqlInt32 TotalBatches, SqlInt32 BatchNumber, SqlInt32 NextItem, SqlDateTime StartTime, SqlDateTime EndTime, SqlDateTime BatchStartTime, SqlDateTime BatchEndTime, SqlString stepName, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			JobBase.ImportProductUpdateJobInfo(FK_JobService, TotalBatches, BatchNumber, NextItem, StartTime, EndTime, BatchStartTime, BatchEndTime, stepName, connection, transaction);
		}

		/// <summary>
		/// 
		/// </summary>
		public static void ImportProductUpdateJobInfo(SqlInt32 FK_JobService, SqlInt32 TotalBatches, SqlInt32 BatchNumber, SqlInt32 NextItem, SqlDateTime StartTime, SqlDateTime EndTime, SqlDateTime BatchStartTime, SqlDateTime BatchEndTime, SqlString stepName, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					SqlJobBase.ImportProductUpdateJobInfo(FK_JobService, TotalBatches, BatchNumber, NextItem, StartTime, EndTime, BatchStartTime, BatchEndTime, stepName, connection, transaction);

					break;

				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of JobBase.ImportProductUpdateJobInfo for this provider: " + providerName);
					throw new ApplicationException("No implementation of JobBase.ImportProductUpdateJobInfo for this provider: " + providerName);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public static void UpdateJobStatus(SqlInt32 intId, SqlString nvchrStatus, SqlString nvchrUserAction, out SqlInt32 RETURN_VALUE)
		{
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			JobBase.UpdateJobStatus(intId, nvchrStatus, nvchrUserAction, out RETURN_VALUE, connection, transaction);
		}

		/// <summary>
		/// 
		/// </summary>
		public static void UpdateJobStatus(SqlInt32 intId, SqlString nvchrStatus, SqlString nvchrUserAction, out SqlInt32 RETURN_VALUE, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			JobBase.UpdateJobStatus(intId, nvchrStatus, nvchrUserAction, out RETURN_VALUE, connection, transaction);
		}

		/// <summary>
		/// 
		/// </summary>
		public static void UpdateJobStatus(SqlInt32 intId, SqlString nvchrStatus, SqlString nvchrUserAction, out SqlInt32 RETURN_VALUE, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					SqlJobBase.UpdateJobStatus(intId, nvchrStatus, nvchrUserAction, out RETURN_VALUE, connection, transaction);

					break;

				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of JobBase.UpdateJobStatus for this provider: " + providerName);
					throw new ApplicationException("No implementation of JobBase.UpdateJobStatus for this provider: " + providerName);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public static void UpdateJobUserAction(SqlInt32 intId, SqlString nvchrUserAction, out SqlInt32 RETURN_VALUE)
		{
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			JobBase.UpdateJobUserAction(intId, nvchrUserAction, out RETURN_VALUE, connection, transaction);
		}

		/// <summary>
		/// 
		/// </summary>
		public static void UpdateJobUserAction(SqlInt32 intId, SqlString nvchrUserAction, out SqlInt32 RETURN_VALUE, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			JobBase.UpdateJobUserAction(intId, nvchrUserAction, out RETURN_VALUE, connection, transaction);
		}

		/// <summary>
		/// 
		/// </summary>
		public static void UpdateJobUserAction(SqlInt32 intId, SqlString nvchrUserAction, out SqlInt32 RETURN_VALUE, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					SqlJobBase.UpdateJobUserAction(intId, nvchrUserAction, out RETURN_VALUE, connection, transaction);

					break;

				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of JobBase.UpdateJobUserAction for this provider: " + providerName);
					throw new ApplicationException("No implementation of JobBase.UpdateJobUserAction for this provider: " + providerName);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public static void ResetJobUserAction(SqlInt32 intId, out SqlInt32 RETURN_VALUE)
		{
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			JobBase.ResetJobUserAction(intId, out RETURN_VALUE, connection, transaction);
		}

		/// <summary>
		/// 
		/// </summary>
		public static void ResetJobUserAction(SqlInt32 intId, out SqlInt32 RETURN_VALUE, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			JobBase.ResetJobUserAction(intId, out RETURN_VALUE, connection, transaction);
		}

		/// <summary>
		/// 
		/// </summary>
		public static void ResetJobUserAction(SqlInt32 intId, out SqlInt32 RETURN_VALUE, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					SqlJobBase.ResetJobUserAction(intId, out RETURN_VALUE, connection, transaction);

					break;

				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of JobBase.ResetJobUserAction for this provider: " + providerName);
					throw new ApplicationException("No implementation of JobBase.ResetJobUserAction for this provider: " + providerName);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public static void ResetJobStep(SqlInt32 FK_JobService, SqlString stepName)
		{
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			JobBase.ResetJobStep(FK_JobService, stepName, connection, transaction);
		}

		/// <summary>
		/// 
		/// </summary>
		public static void ResetJobStep(SqlInt32 FK_JobService, SqlString stepName, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			JobBase.ResetJobStep(FK_JobService, stepName, connection, transaction);
		}

		/// <summary>
		/// 
		/// </summary>
		public static void ResetJobStep(SqlInt32 FK_JobService, SqlString stepName, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					SqlJobBase.ResetJobStep(FK_JobService, stepName, connection, transaction);

					break;

				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of JobBase.ResetJobStep for this provider: " + providerName);
					throw new ApplicationException("No implementation of JobBase.ResetJobStep for this provider: " + providerName);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public static void ImportProductCleanup(SqlInt32 FK_JobService)
		{
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			JobBase.ImportProductCleanup(FK_JobService, connection, transaction);
		}

		/// <summary>
		/// 
		/// </summary>
		public static void ImportProductCleanup(SqlInt32 FK_JobService, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			JobBase.ImportProductCleanup(FK_JobService, connection, transaction);
		}

		/// <summary>
		/// 
		/// </summary>
		public static void ImportProductCleanup(SqlInt32 FK_JobService, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					SqlJobBase.ImportProductCleanup(FK_JobService, connection, transaction);

					break;

				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of JobBase.ImportProductCleanup for this provider: " + providerName);
					throw new ApplicationException("No implementation of JobBase.ImportProductCleanup for this provider: " + providerName);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public static void AddJob(SqlString nvchrType, SqlString nvchrSubtype, SqlString nvchrProfileName, SqlString nvchrShortName, SqlString nvchrDescription, SqlString ntextJobData, SqlString nvchrStatus, SqlString nvchrComputerName, SqlString nvchrUsername, out SqlInt32 RETURN_VALUE)
		{
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			JobBase.AddJob(nvchrType, nvchrSubtype, nvchrProfileName, nvchrShortName, nvchrDescription, ntextJobData, nvchrStatus, nvchrComputerName, nvchrUsername, out RETURN_VALUE, connection, transaction);
		}

		/// <summary>
		/// 
		/// </summary>
		public static void AddJob(SqlString nvchrType, SqlString nvchrSubtype, SqlString nvchrProfileName, SqlString nvchrShortName, SqlString nvchrDescription, SqlString ntextJobData, SqlString nvchrStatus, SqlString nvchrComputerName, SqlString nvchrUsername, out SqlInt32 RETURN_VALUE, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			JobBase.AddJob(nvchrType, nvchrSubtype, nvchrProfileName, nvchrShortName, nvchrDescription, ntextJobData, nvchrStatus, nvchrComputerName, nvchrUsername, out RETURN_VALUE, connection, transaction);
		}

		/// <summary>
		/// 
		/// </summary>
		public static void AddJob(SqlString nvchrType, SqlString nvchrSubtype, SqlString nvchrProfileName, SqlString nvchrShortName, SqlString nvchrDescription, SqlString ntextJobData, SqlString nvchrStatus, SqlString nvchrComputerName, SqlString nvchrUsername, out SqlInt32 RETURN_VALUE, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					SqlJobBase.AddJob(nvchrType, nvchrSubtype, nvchrProfileName, nvchrShortName, nvchrDescription, ntextJobData, nvchrStatus, nvchrComputerName, nvchrUsername, out RETURN_VALUE, connection, transaction);

					break;

				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of JobBase.AddJob for this provider: " + providerName);
					throw new ApplicationException("No implementation of JobBase.AddJob for this provider: " + providerName);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public static void DeleteJob(SqlInt32 intId, out SqlInt32 RETURN_VALUE)
		{
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			JobBase.DeleteJob(intId, out RETURN_VALUE, connection, transaction);
		}

		/// <summary>
		/// 
		/// </summary>
		public static void DeleteJob(SqlInt32 intId, out SqlInt32 RETURN_VALUE, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			JobBase.DeleteJob(intId, out RETURN_VALUE, connection, transaction);
		}

		/// <summary>
		/// 
		/// </summary>
		public static void DeleteJob(SqlInt32 intId, out SqlInt32 RETURN_VALUE, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					SqlJobBase.DeleteJob(intId, out RETURN_VALUE, connection, transaction);

					break;

				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of JobBase.DeleteJob for this provider: " + providerName);
					throw new ApplicationException("No implementation of JobBase.DeleteJob for this provider: " + providerName);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public static DataTable GetProfilesData(SqlString nvchrDomain, SqlString nvchrSearchStr, SqlString nvchrPermissionAction, SqlString nvchrUser)
		{
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return JobBase.GetProfilesData(nvchrDomain, nvchrSearchStr, nvchrPermissionAction, nvchrUser, connection, transaction);
		}

		/// <summary>
		/// 
		/// </summary>
		public static DataTable GetProfilesData(SqlString nvchrDomain, SqlString nvchrSearchStr, SqlString nvchrPermissionAction, SqlString nvchrUser, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return JobBase.GetProfilesData(nvchrDomain, nvchrSearchStr, nvchrPermissionAction, nvchrUser, connection, transaction);
		}

		/// <summary>
		/// 
		/// </summary>
		public static DataTable GetProfilesData(SqlString nvchrDomain, SqlString nvchrSearchStr, SqlString nvchrPermissionAction, SqlString nvchrUser, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlJobBase.GetProfilesData(nvchrDomain, nvchrSearchStr, nvchrPermissionAction, nvchrUser, connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of JobBase.GetProfilesData for this provider: " + providerName);
					throw new ApplicationException("No implementation of JobBase.GetProfilesData for this provider: " + providerName);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public static string GetProfile(SqlString nvchrName, SqlString nvchrDomain)
		{
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return JobBase.GetProfile(nvchrName, nvchrDomain, connection, transaction);
		}

		/// <summary>
		/// 
		/// </summary>
		public static string GetProfile(SqlString nvchrName, SqlString nvchrDomain, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return JobBase.GetProfile(nvchrName, nvchrDomain, connection, transaction);
		}

		/// <summary>
		/// 
		/// </summary>
		public static string GetProfile(SqlString nvchrName, SqlString nvchrDomain, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlJobBase.GetProfile(nvchrName, nvchrDomain, connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of JobBase.GetProfile for this provider: " + providerName);
					throw new ApplicationException("No implementation of JobBase.GetProfile for this provider: " + providerName);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public static string GetProfileByID(SqlInt32 intProfileID)
		{
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return JobBase.GetProfileByID(intProfileID, connection, transaction);
		}

		/// <summary>
		/// 
		/// </summary>
		public static string GetProfileByID(SqlInt32 intProfileID, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return JobBase.GetProfileByID(intProfileID, connection, transaction);
		}

		/// <summary>
		/// 
		/// </summary>
		public static string GetProfileByID(SqlInt32 intProfileID, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlJobBase.GetProfileByID(intProfileID, connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of JobBase.GetProfileByID for this provider: " + providerName);
					throw new ApplicationException("No implementation of JobBase.GetProfileByID for this provider: " + providerName);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public static string GetSubscribersAndProfiles()
		{
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return JobBase.GetSubscribersAndProfiles(connection, transaction);
		}

		/// <summary>
		/// 
		/// </summary>
		public static string GetSubscribersAndProfiles(IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return JobBase.GetSubscribersAndProfiles(connection, transaction);
		}

		/// <summary>
		/// 
		/// </summary>
		public static string GetSubscribersAndProfiles(IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlJobBase.GetSubscribersAndProfiles(connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of JobBase.GetSubscribersAndProfiles for this provider: " + providerName);
					throw new ApplicationException("No implementation of JobBase.GetSubscribersAndProfiles for this provider: " + providerName);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public static DataTable FindImportJobLog(SqlString nvchrJobID)
		{
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return JobBase.FindImportJobLog(nvchrJobID, connection, transaction);
		}

		/// <summary>
		/// 
		/// </summary>
		public static DataTable FindImportJobLog(SqlString nvchrJobID, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return JobBase.FindImportJobLog(nvchrJobID, connection, transaction);
		}

		/// <summary>
		/// 
		/// </summary>
		public static DataTable FindImportJobLog(SqlString nvchrJobID, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlJobBase.FindImportJobLog(nvchrJobID, connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of JobBase.FindImportJobLog for this provider: " + providerName);
					throw new ApplicationException("No implementation of JobBase.FindImportJobLog for this provider: " + providerName);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public static DataSet ImportJobErrorLogExists(SqlString nvchrJobID)
		{
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return JobBase.ImportJobErrorLogExists(nvchrJobID, connection, transaction);
		}

		/// <summary>
		/// 
		/// </summary>
		public static DataSet ImportJobErrorLogExists(SqlString nvchrJobID, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return JobBase.ImportJobErrorLogExists(nvchrJobID, connection, transaction);
		}

		/// <summary>
		/// 
		/// </summary>
		public static DataSet ImportJobErrorLogExists(SqlString nvchrJobID, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlJobBase.ImportJobErrorLogExists(nvchrJobID, connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of JobBase.ImportJobErrorLogExists for this provider: " + providerName);
					throw new ApplicationException("No implementation of JobBase.ImportJobErrorLogExists for this provider: " + providerName);
			}
		}

	}
}
