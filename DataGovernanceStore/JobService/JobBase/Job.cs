using System;
using System.Xml;
using System.Data;
using System.Data.SqlTypes;
using System.Configuration;
using System.Data.SqlClient;
//using Oracle.DataAccess.Client;

namespace Riversand.JobService
{
    using MDM.Core;
    using MDM.Utility;
    using MDM.BusinessObjects.Diagnostics;
    using Riversand.JobService.Interfaces;

	/// <summary>
	/// Summary description for Job.
	/// </summary>
	public abstract class Job : Interfaces.IJob
	{
		/// <summary>
		/// Base contructor that takes the job id
		/// </summary>
		/// <param name="id">the id of the job</param>
		protected Job(XmlElement jobXmlNode, int jobId)
		{
            _currentTraceSettings = MDMOperationContextHelper.GetCurrentTraceSettings();

			_id = jobId;
			this.jobXmlNode = jobXmlNode;
			_description = jobXmlNode.GetAttribute("description");
			_username = jobXmlNode.GetAttribute("username");
			XmlDocument jobDataXmlDoc = new XmlDocument();

            String jobType = jobXmlNode.GetAttribute("type");
            if (jobType.Equals("CatalogExport", StringComparison.OrdinalIgnoreCase))
            {
                _jobType = JobType.EntityExport;
            }
            if (jobType.ToLowerInvariant() == "lookupexport")
            {
                _jobType = JobType.LookupExport;
            }
            else
            {
                jobDataXmlDoc.LoadXml(jobXmlNode.SelectSingleNode("data").InnerXml);
                //jobDataXmlDoc.LoadXml(jobXmlNode.GetAttribute("data"));
                jobXmlWrapper = new JobXmlWrapper(jobDataXmlDoc);
                jobXmlWrapper.AddJobDetails("username", _username);
                jobXmlWrapper.AddJobDetails("description", _description);
                jobXmlWrapper.AddJobDetails("id", jobId.ToString());
            }
			_transaction = null;
			_connection = null;
		}
		/// <summary>
		///	Property to identify if the job is transactional
		/// </summary>
		public virtual bool isTransactional
		{   
			get 
			{
				return false;
			}
		}
		/// <summary>
		/// Transaction Property for a Transactional Job
		/// </summary>
		private IDbTransaction _transaction;

        private JobType _jobType = JobType.UnKnown;

        /// <summary>
        /// Indicates current trace settings
        /// </summary>
        private TraceSettings _currentTraceSettings = new TraceSettings();

		public IDbTransaction Transaction
		{   
            get { return _transaction; }
		}
		/// <summary>
		///	Connection Property for a Transactional Job
		/// </summary>
		private IDbConnection _connection;
		public IDbConnection Connection
		{   
            get { return _connection; }
		}

        /// <summary>
        
        /// </summary>
        public abstract void Initialize();

		/// <summary>
		/// Override this function with the main task
		/// public void Run()
		/// {
		///		// Initialization code goes here
		///		
		///		while(!cancelJob)
		///		{
		///			// Do a part of the job
		///			description =  "Completed 20 of 500 elements";
		///			//Update jobDataXmlDoc
		///			SaveJob();
		///		}
		/// }
		/// </summary>
		public abstract void Run();

        public void Execute()
        {
            DiagnosticActivity diagnosticActivity = new DiagnosticActivity();

            if (_currentTraceSettings.IsBasicTracingEnabled)
            {
                diagnosticActivity.Start();
                }
                
            try
                {
                _connection = new SqlConnection(MDM.Utility.AppConfigurationHelper.ConnectionString);
                    
                if (_currentTraceSettings.IsBasicTracingEnabled)
                {
                    diagnosticActivity.LogInformation(String.Format("Execute job with job type: {0}", _jobType));
            }
            
            if (_jobType == JobType.LookupExport)
            {
                try
                {
                    Run();
                }
                catch (Exception ex)
                {
                    String message = "Job.Execute() failed.";
                    LogException(message, ex);
                        diagnosticActivity.LogError(String.Concat(message, " (ex.Message =  ", ex.ToString(), ")"));
                    throw new Exception(String.Concat("||", message, " (ex.Message =  ", ex.Message, ")"), ex);
                }
            }
            else
            {
                string _OrigJobXML = jobXmlWrapper.innerxml;

                    if (_currentTraceSettings.IsBasicTracingEnabled)
                    {
                        diagnosticActivity.LogInformation(String.Format("Execute job is transactional: {0}", isTransactional));
                    }

                if (isTransactional)
                {
                    try
                    {
                        _connection.Open();
                        _transaction = _connection.BeginTransaction();
                        Run();
                        _transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        _transaction.Rollback();
                        jobXmlWrapper.ReBuildJobXml(_OrigJobXML);
                        String message = "Job.Execute() failed.";
                        LogException(String.Concat("||", message, " (ex.Message = ", ex.Message, ")"), ex);
                            diagnosticActivity.LogError(String.Concat(message, " (ex.Message =  ", ex.ToString(), ")"));
                        SaveJob();
                        throw new Exception(String.Concat("||", message, " (ex.Message =  ", ex.Message, ")"), ex);
                    }
                    finally
                    {
                        if (_connection.State == ConnectionState.Open)
                        {
                            _connection.Close();
                        }
                    }
                }
                else
                {
                    try
                    {
                        Run();
                    }
                    catch (Exception ex)
                    {
                        jobXmlWrapper.ReBuildJobXml(_OrigJobXML);
                        String message = "Job.Execute() failed.";
                        LogException(message, ex);
                            diagnosticActivity.LogError(String.Concat(message, " (ex.Message =  ", ex.ToString(), ")"));
                        // CatalogExport is saving the job details in SyndicationJobBase.cs Run() method.
                        // Below code is introduced to avoid overwrite of job details.
                        if (_jobType != JobType.EntityExport)
                        {
                            SaveJob();
                        }
                        throw new Exception(String.Concat("||", message, " (ex.Message =  ", ex.Message, ")"), ex);
                    }
                    }
                }
            }
            finally
            {
                if (_currentTraceSettings.IsBasicTracingEnabled)
                {
                    diagnosticActivity.Stop();
                }
            }
        }

		/// <summary>
		/// Override this method with post process clean-up
		/// </summary>
		public abstract void CleanUp();

		/// <summary>
		///	Implement this function to inficate if the job was completed succesfully
		/// </summary>
		/// <returns>true if completed, false otherwise</returns>
		public abstract bool IsComplete();

		/// <summary>
		///	Check whether the job is Ignored or not.
		/// </summary>
		/// <returns>True if Ignored else False </returns>
		public abstract Boolean IsIgnored();
		
		/// <summary>
		/// Indicates if the job should stop execution
		/// </summary>
		private volatile bool _cancelJob = false;
		public bool cancelJob
		{
			get { return _cancelJob; }
			set { _cancelJob = value; }
		}

		/// <summary>
		/// The id of the job
		/// </summary>
		private readonly int _id;
		public int id
		{
			get { return _id; }
		}

		/// <summary>
		/// The jobXmlNode that was passed into the constructor
		/// </summary>
		protected readonly XmlElement jobXmlNode;

		/// <summary>
		/// The description of the job
		/// </summary>
		private string _description;
		public string description
		{
			get { return _description; }
			set { _description = value; }
		}

		/// <summary>
		/// The username that submitted the job
		/// </summary>
		private readonly string _username;
		public string username
		{
			get { return _username; }
		}

		/// <summary>
		/// Saves the job attributes to the database
		/// </summary>
		public void SaveJob()
		{
            SaveJob(jobXmlWrapper.xml);
		}

        /// <summary>
        /// Saves the job attributes to the database
        /// </summary>
        /// <param name="jobData">Indicates the job data </param>
        public void SaveJob(String jobData)
        {
            DiagnosticActivity diagnosticActivity = new DiagnosticActivity();

            if (_currentTraceSettings.IsBasicTracingEnabled)
            {
                diagnosticActivity.Start();
            }

            try
            {
                SqlInt32 ret;
                Riversand.StoredProcedures.JobBase.UpdateJobInformation(id, description, jobData, out ret);
            }
            catch (Exception ex)
            {
                String message = "Job.SaveJob() failed.";
                diagnosticActivity.LogError(String.Concat(message, " (ex.Message = ", ex.ToString(), ")"));
                throw new Exception(String.Concat(message, " (ex.Message = ", ex.ToString(), ")"), ex);
            }
            finally
            {
                if (_currentTraceSettings.IsBasicTracingEnabled)
                {
                    diagnosticActivity.Stop();
                }
            }
        }

        public static string UpdateJobStatus(int jobId, string step, int BatchCounter, int BatchTotal, string StartTime)
        {
            
            
            System.Data.SqlTypes.SqlDateTime dtnull = System.Data.SqlTypes.SqlDateTime.Null;
            string EndTime = System.DateTime.Now.ToString("MMM dd yyyy HH:mm:ss:fff");
            try
            {
                System.Data.SqlTypes.SqlDateTime sqlStarttime = new SqlDateTime(System.DateTime.Now);
                System.Data.SqlTypes.SqlDateTime sqlEndtime = new SqlDateTime(System.DateTime.Now);
                Riversand.StoredProcedures.JobBase.ImportProductUpdateJobInfo(jobId, BatchTotal, BatchCounter, BatchCounter + 1, sqlStarttime, sqlEndtime, dtnull, dtnull, step);
            }
            catch (Exception ex)
            {
                string message = "UpdateJobStatusData failed.  " + "jobid: " + jobId + " BatchTotal: " + BatchTotal + " BatchCounter: " + BatchCounter + " StartTime: " + StartTime + " EndTime: " + EndTime + " (ex.Message = " + ex.ToString() + ")";
                DiagnosticActivity diagnosticActivity = new DiagnosticActivity();
                diagnosticActivity.LogError(message);
            }
            StartTime = System.DateTime.Now.ToString("MMM dd yyyy HH:mm:ss:fff");

            return StartTime;
        }

		public void LogException(string message, Exception e)
		{
			jobXmlWrapper.LogException(message, e);
		}

        /// <summary>
        /// Log exception
        /// </summary>
        /// <param name="error">Indicates the error</param>
        /// <param name="errorDetails">Indicates the error message</param>
        public void LogException(String error, String errorDetails)
        {
            jobXmlWrapper.LogException(error, errorDetails);
        }

		/// <summary>
		/// The JobXmlWrapper containing the job details, update it as the job progresses
		/// </summary>
		protected readonly JobXmlWrapper jobXmlWrapper;

	}
}
