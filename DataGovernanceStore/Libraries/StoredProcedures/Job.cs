
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
	/// Job API
	/// </summary>
	public class Job
	{
		/// <summary>
		/// Private constructor
		/// </summary>		
		private Job()
		{
		}
		
		
		/// <summary>
        /// Adds validation results
        /// </summary>
		public static void InsertValidationResult(SqlString jobID, SqlInt32 catalogID, SqlInt32 CNodeParent, SqlInt32 CNode, SqlInt32 customerID, SqlInt32 localeID, SqlString objectType, SqlString validationDescription, SqlInt32 attributeID, SqlString oldValue, SqlString newValue, SqlString message, SqlString creatingUser, SqlString creatingProgram)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			Job.InsertValidationResult(jobID, catalogID, CNodeParent, CNode, customerID, localeID, objectType, validationDescription, attributeID, oldValue, newValue, message, creatingUser, creatingProgram, connection, transaction);
		}

		/// <summary>
        /// Adds validation results
        /// </summary>
		public static void InsertValidationResult(SqlString jobID, SqlInt32 catalogID, SqlInt32 CNodeParent, SqlInt32 CNode, SqlInt32 customerID, SqlInt32 localeID, SqlString objectType, SqlString validationDescription, SqlInt32 attributeID, SqlString oldValue, SqlString newValue, SqlString message, SqlString creatingUser, SqlString creatingProgram, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			Job.InsertValidationResult(jobID, catalogID, CNodeParent, CNode, customerID, localeID, objectType, validationDescription, attributeID, oldValue, newValue, message, creatingUser, creatingProgram, connection, transaction);
		}

		/// <summary>
        /// Adds validation results
        /// </summary>
		public static void InsertValidationResult(SqlString jobID, SqlInt32 catalogID, SqlInt32 CNodeParent, SqlInt32 CNode, SqlInt32 customerID, SqlInt32 localeID, SqlString objectType, SqlString validationDescription, SqlInt32 attributeID, SqlString oldValue, SqlString newValue, SqlString message, SqlString creatingUser, SqlString creatingProgram, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					SqlJob.InsertValidationResult(jobID, catalogID, CNodeParent, CNode, customerID, localeID, objectType, validationDescription, attributeID, oldValue, newValue, message, creatingUser, creatingProgram, connection, transaction);

					break;

				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Job.InsertValidationResult for this provider: "+providerName);
					throw new ApplicationException("No implementation of Job.InsertValidationResult for this provider: "+providerName);
			}
		}

		/// <summary>
        /// Gives a result for Matching jobs
        /// </summary>
		public static DataSet GetMatchingJobs(SqlInt32 jobid)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return Job.GetMatchingJobs(jobid, connection, transaction);
		}

		/// <summary>
        /// Gives a result for Matching jobs
        /// </summary>
		public static DataSet GetMatchingJobs(SqlInt32 jobid, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return Job.GetMatchingJobs(jobid, connection, transaction);
		}

		/// <summary>
        /// Gives a result for Matching jobs
        /// </summary>
		public static DataSet GetMatchingJobs(SqlInt32 jobid, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlJob.GetMatchingJobs(jobid, connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Job.GetMatchingJobs for this provider: "+providerName);
					throw new ApplicationException("No implementation of Job.GetMatchingJobs for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static void SetApprovedMatchingCnode(SqlString MatchingXML, SqlString UpdateFlag, SqlInt32 JobID, ref SqlInt32 intOutput, SqlString vchrUserID, ref SqlString vchrReturnSourceCatalog)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			Job.SetApprovedMatchingCnode(MatchingXML, UpdateFlag, JobID, ref intOutput, vchrUserID, ref vchrReturnSourceCatalog, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static void SetApprovedMatchingCnode(SqlString MatchingXML, SqlString UpdateFlag, SqlInt32 JobID, ref SqlInt32 intOutput, SqlString vchrUserID, ref SqlString vchrReturnSourceCatalog, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			Job.SetApprovedMatchingCnode(MatchingXML, UpdateFlag, JobID, ref intOutput, vchrUserID, ref vchrReturnSourceCatalog, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static void SetApprovedMatchingCnode(SqlString MatchingXML, SqlString UpdateFlag, SqlInt32 JobID, ref SqlInt32 intOutput, SqlString vchrUserID, ref SqlString vchrReturnSourceCatalog, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					SqlJob.SetApprovedMatchingCnode(MatchingXML, UpdateFlag, JobID, ref intOutput, vchrUserID, ref vchrReturnSourceCatalog, connection, transaction);

					break;

				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Job.SetApprovedMatchingCnode for this provider: "+providerName);
					throw new ApplicationException("No implementation of Job.SetApprovedMatchingCnode for this provider: "+providerName);
			}
		}

        /// <summary>
        /// 
        /// </summary>
        public static DataTable GetJobTypeEventSourceMapping(SqlInt32 EventSourceId, SqlInt32 JobId)
        {
            IDbConnection connection = null;
            IDbTransaction transaction = null;
            return Job.GetJobTypeEventSourceMapping(EventSourceId, JobId, connection, transaction);
        }

        /// <summary>
        /// 
        /// </summary>
        public static DataTable GetJobTypeEventSourceMapping(SqlInt32 EventSourceId, SqlInt32 JobId, IDbConnection connection)
        {
            IDbTransaction transaction = null;
            return Job.GetJobTypeEventSourceMapping(EventSourceId, JobId, connection, transaction);
        }

        /// <summary>
        /// 
        /// </summary>
        public static DataTable GetJobTypeEventSourceMapping(SqlInt32 EventSourceId, SqlInt32 JobId, IDbConnection connection, IDbTransaction transaction)
        {
            string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
            switch (providerName)
            {
                case "SqlProvider":
                    return SqlJob.GetJobTypeEventSourceMapping(EventSourceId, JobId, connection, transaction);


                default:
                    MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Job.GetJobTypeEventSourceMapping for this provider: " + providerName);
                    throw new ApplicationException("No implementation of Job.GetJobTypeEventSourceMapping for this provider: " + providerName);
            }
        }

		/// <summary>
        /// 
        /// </summary>
		public static DataSet GetServiceResult(SqlInt32 ServiceID, SqlString JobID_List, SqlString CNodeID_List, SqlInt32 FK_Application_Config, SqlString GridId, SqlString JobCNodeStatusID_List, SqlInt32 FK_Locale, SqlXml FilterXML)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return Job.GetServiceResult(ServiceID, JobID_List, CNodeID_List, FK_Application_Config, GridId, JobCNodeStatusID_List, FK_Locale, FilterXML, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataSet GetServiceResult(SqlInt32 ServiceID, SqlString JobID_List, SqlString CNodeID_List, SqlInt32 FK_Application_Config, SqlString GridId, SqlString JobCNodeStatusID_List, SqlInt32 FK_Locale, SqlXml FilterXML, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return Job.GetServiceResult(ServiceID, JobID_List, CNodeID_List, FK_Application_Config, GridId, JobCNodeStatusID_List, FK_Locale, FilterXML, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataSet GetServiceResult(SqlInt32 ServiceID, SqlString JobID_List, SqlString CNodeID_List, SqlInt32 FK_Application_Config, SqlString GridId, SqlString JobCNodeStatusID_List, SqlInt32 FK_Locale, SqlXml FilterXML, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlJob.GetServiceResult(ServiceID, JobID_List, CNodeID_List, FK_Application_Config, GridId, JobCNodeStatusID_List, FK_Locale, FilterXML, connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Job.GetServiceResult for this provider: "+providerName);
					throw new ApplicationException("No implementation of Job.GetServiceResult for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static void ProcessServiceResult(SqlInt32 FK_Event_Source, SqlInt32 FK_Application_Config, SqlXml DataXML, SqlString loginUser, SqlString userProgram)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			Job.ProcessServiceResult(FK_Event_Source, FK_Application_Config, DataXML, loginUser, userProgram, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static void ProcessServiceResult(SqlInt32 FK_Event_Source, SqlInt32 FK_Application_Config, SqlXml DataXML, SqlString loginUser, SqlString userProgram, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			Job.ProcessServiceResult(FK_Event_Source, FK_Application_Config, DataXML, loginUser, userProgram, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static void ProcessServiceResult(SqlInt32 FK_Event_Source, SqlInt32 FK_Application_Config, SqlXml DataXML, SqlString loginUser, SqlString userProgram, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					SqlJob.ProcessServiceResult(FK_Event_Source, FK_Application_Config, DataXML, loginUser, userProgram, connection, transaction);

					break;

				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Job.ProcessServiceResult for this provider: "+providerName);
					throw new ApplicationException("No implementation of Job.ProcessServiceResult for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetServiceResult_SchemaValidation(SqlInt32 FK_Event_Source, SqlString JobIDList, SqlString CNodeList, SqlInt32 FK_Application_Config, SqlString GridId, SqlString JobCNodeStatusID_List, SqlInt32 FK_Locale, SqlXml FilterXML)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return Job.GetServiceResult_SchemaValidation(FK_Event_Source, JobIDList, CNodeList, FK_Application_Config, GridId, JobCNodeStatusID_List, FK_Locale, FilterXML, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetServiceResult_SchemaValidation(SqlInt32 FK_Event_Source, SqlString JobIDList, SqlString CNodeList, SqlInt32 FK_Application_Config, SqlString GridId, SqlString JobCNodeStatusID_List, SqlInt32 FK_Locale, SqlXml FilterXML, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return Job.GetServiceResult_SchemaValidation(FK_Event_Source, JobIDList, CNodeList, FK_Application_Config, GridId, JobCNodeStatusID_List, FK_Locale, FilterXML, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetServiceResult_SchemaValidation(SqlInt32 FK_Event_Source, SqlString JobIDList, SqlString CNodeList, SqlInt32 FK_Application_Config, SqlString GridId, SqlString JobCNodeStatusID_List, SqlInt32 FK_Locale, SqlXml FilterXML, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlJob.GetServiceResult_SchemaValidation(FK_Event_Source, JobIDList, CNodeList, FK_Application_Config, GridId, JobCNodeStatusID_List, FK_Locale, FilterXML, connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Job.GetServiceResult_SchemaValidation for this provider: "+providerName);
					throw new ApplicationException("No implementation of Job.GetServiceResult_SchemaValidation for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetServiceResult_DescMatching(SqlInt32 FK_Event_Source, SqlString JobIDList, SqlString CNodeList, SqlInt32 FK_Application_Config, SqlString GridId, SqlString JobCNodeStatusID_List, SqlInt32 FK_Locale, SqlXml FilterXML)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return Job.GetServiceResult_DescMatching(FK_Event_Source, JobIDList, CNodeList, FK_Application_Config, GridId, JobCNodeStatusID_List, FK_Locale, FilterXML, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetServiceResult_DescMatching(SqlInt32 FK_Event_Source, SqlString JobIDList, SqlString CNodeList, SqlInt32 FK_Application_Config, SqlString GridId, SqlString JobCNodeStatusID_List, SqlInt32 FK_Locale, SqlXml FilterXML, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return Job.GetServiceResult_DescMatching(FK_Event_Source, JobIDList, CNodeList, FK_Application_Config, GridId, JobCNodeStatusID_List, FK_Locale, FilterXML, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetServiceResult_DescMatching(SqlInt32 FK_Event_Source, SqlString JobIDList, SqlString CNodeList, SqlInt32 FK_Application_Config, SqlString GridId, SqlString JobCNodeStatusID_List, SqlInt32 FK_Locale, SqlXml FilterXML, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlJob.GetServiceResult_DescMatching(FK_Event_Source, JobIDList, CNodeList, FK_Application_Config, GridId, JobCNodeStatusID_List, FK_Locale, FilterXML, connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Job.GetServiceResult_DescMatching for this provider: "+providerName);
					throw new ApplicationException("No implementation of Job.GetServiceResult_DescMatching for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetServiceResult_AttributeExtraction(SqlInt32 FK_Event_Source, SqlString JobIDList, SqlString CNodeList, SqlInt32 FK_Application_Config, SqlString GridId, SqlString JobCNodeStatusID_List, SqlInt32 FK_Locale, SqlXml FilterXML)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return Job.GetServiceResult_AttributeExtraction(FK_Event_Source, JobIDList, CNodeList, FK_Application_Config, GridId, JobCNodeStatusID_List, FK_Locale, FilterXML, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetServiceResult_AttributeExtraction(SqlInt32 FK_Event_Source, SqlString JobIDList, SqlString CNodeList, SqlInt32 FK_Application_Config, SqlString GridId, SqlString JobCNodeStatusID_List, SqlInt32 FK_Locale, SqlXml FilterXML, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return Job.GetServiceResult_AttributeExtraction(FK_Event_Source, JobIDList, CNodeList, FK_Application_Config, GridId, JobCNodeStatusID_List, FK_Locale, FilterXML, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetServiceResult_AttributeExtraction(SqlInt32 FK_Event_Source, SqlString JobIDList, SqlString CNodeList, SqlInt32 FK_Application_Config, SqlString GridId, SqlString JobCNodeStatusID_List, SqlInt32 FK_Locale, SqlXml FilterXML, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlJob.GetServiceResult_AttributeExtraction(FK_Event_Source, JobIDList, CNodeList, FK_Application_Config, GridId, JobCNodeStatusID_List, FK_Locale, FilterXML, connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Job.GetServiceResult_AttributeExtraction for this provider: "+providerName);
					throw new ApplicationException("No implementation of Job.GetServiceResult_AttributeExtraction for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetServiceResult_Normalization(SqlInt32 FK_Event_Source, SqlString JobIDList, SqlString CNodeList, SqlInt32 FK_Application_Config, SqlString GridId, SqlString JobCNodeStatusID_List, SqlInt32 FK_Locale, SqlXml FilterXML)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return Job.GetServiceResult_Normalization(FK_Event_Source, JobIDList, CNodeList, FK_Application_Config, GridId, JobCNodeStatusID_List, FK_Locale, FilterXML, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetServiceResult_Normalization(SqlInt32 FK_Event_Source, SqlString JobIDList, SqlString CNodeList, SqlInt32 FK_Application_Config, SqlString GridId, SqlString JobCNodeStatusID_List, SqlInt32 FK_Locale, SqlXml FilterXML, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return Job.GetServiceResult_Normalization(FK_Event_Source, JobIDList, CNodeList, FK_Application_Config, GridId, JobCNodeStatusID_List, FK_Locale, FilterXML, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetServiceResult_Normalization(SqlInt32 FK_Event_Source, SqlString JobIDList, SqlString CNodeList, SqlInt32 FK_Application_Config, SqlString GridId, SqlString JobCNodeStatusID_List, SqlInt32 FK_Locale, SqlXml FilterXML, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlJob.GetServiceResult_Normalization(FK_Event_Source, JobIDList, CNodeList, FK_Application_Config, GridId, JobCNodeStatusID_List, FK_Locale, FilterXML, connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Job.GetServiceResult_Normalization for this provider: "+providerName);
					throw new ApplicationException("No implementation of Job.GetServiceResult_Normalization for this provider: "+providerName);
			}
		}

	}
}		
