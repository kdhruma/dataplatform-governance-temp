using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Transactions;

namespace MDM.JobManager.Business
{
    using MDM.Core;
    using MDM.BusinessObjects;
    using MDM.BusinessObjects.Jobs;
    using MDM.BusinessObjects.Imports;
    using MDM.Utility;
    using MDM.JobManager.Data.SqlClient;
    using MDM.ConfigurationManager.Business;
    using Riversand.JobService;
    using Riversand.JobService.Interfaces;
    using MDM.AdminManager.Business;
    using MDM.ProfileManager.Business;
    using MDM.BusinessObjects.Diagnostics;
    using MDM.Core.Exceptions;
    using MDM.MessageManager.Business;

    /// <summary>
    /// Specifies job manager class
    /// </summary>
    public class JobBL : BusinessLogicBase
    {
        #region Fields

        /// <summary>
        /// Field denoting the security principal
        /// </summary>
        private SecurityPrincipal _securityPrincipal;

        /// <summary>
        /// Field denoting locale Message
        /// </summary>
        private LocaleMessage _localeMessage;

        /// <summary>
        /// Field denoting localeMessageBL
        /// </summary>
        private LocaleMessageBL _localeMessageBL = new LocaleMessageBL();

        /// <summary>
        /// Indicates current trace settings
        /// </summary>
        private TraceSettings _currentTraceSettings = new TraceSettings();

        #endregion

        #region Constructors

        /// <summary>
        /// Default Constructor
        /// </summary>
        public JobBL()
        {
            GetSecurityPrincipal();
            _currentTraceSettings = MDMOperationContextHelper.GetCurrentTraceSettings();
        }

        #endregion

        #region Public Methods

        #region CUD methods
        
        /// <summary>
        /// creates the specified job
        /// </summary>
        /// <param name="job">job to be created</param>
        /// <param name="callerContext">caller context indicating who called the API</param>
        public OperationResult Create(Job job, CallerContext callerContext)
        {
            ValidateInputParameters(job, callerContext);

            job.Action = ObjectAction.Create;

            return this.Process(job, callerContext);
        }

        /// <summary>
        /// Updated the job
        /// </summary>
        /// <param name="job">job to be updated</param>
        /// <param name="callerContext">caller context indicating who called the API</param>
        public OperationResult Update(Job job, CallerContext callerContext)
        {
            ValidateInputParameters(job, callerContext);
            job.Action = ObjectAction.Update;

            return this.Process(job, callerContext);
        }

        /// <summary>
        /// Deletes the specified job
        /// </summary>
        /// <param name="job">job to be deleted</param>
        /// <param name="callerContext">caller context indicating who called the API</param>
        public OperationResult Delete(Job job, CallerContext callerContext)
        {
            ValidateInputParameters(job, callerContext);
            job.Action = ObjectAction.Delete;

            return this.Process(job, callerContext);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="jobsCollection"></param>
        /// <param name="callerContext"></param>
        /// <returns></returns>
        public OperationResultCollection Process(JobCollection jobsCollection, CallerContext callerContext)
        {
            DiagnosticActivity diagnosticActivity = new DiagnosticActivity();

            if (_currentTraceSettings.IsBasicTracingEnabled)
            {
                diagnosticActivity.Start();
            }

            try
            {
                OperationResultCollection jobProcessOperationResult = new OperationResultCollection();

                if (jobsCollection == null)
                {
                    diagnosticActivity.LogError("Jobs collection cannot be null.");
                    throw new MDMOperationException("112260", "Jobs collection cannot be null.", "JobBL.Process", String.Empty, "Process");
                }

                ValidateCallerContext(callerContext);

                callerContext.ProgramName = PopulateProgramName(callerContext);

                foreach (Job job in jobsCollection)
                {
                    jobProcessOperationResult.Add(this.Process(job, callerContext));
                }
                return jobProcessOperationResult;
            }
            finally
            {
                if (_currentTraceSettings.IsBasicTracingEnabled)
                {
                    diagnosticActivity.Stop();
                }
            }
            
        }

        #endregion
        
        #region Get Methods

        public JobCollection GetJobs(JobFilterContext jobFilterContext, CallerContext callerContext)
        {
            DiagnosticActivity diagnosticActivity = new DiagnosticActivity();

            if (_currentTraceSettings.IsBasicTracingEnabled)
            {
                diagnosticActivity.Start();
            }

            
            JobCollection jobs;
            
            try
            {
                ValidateCallerContext(callerContext);

                JobDA jobDa = new JobDA();

                String userName = PopulateUserName();

                DBCommandProperties command = DBCommandHelper.Get(callerContext, MDMCenterModuleAction.Read);

                jobs = jobFilterContext.GetAll ?
                        jobDa.GetAll(userName, command, jobFilterContext.DateTimeFrom, jobFilterContext.DateTimeTo, jobFilterContext.Locale, jobFilterContext.GetOnlyUserJobs) :
                        jobDa.Get(jobFilterContext.JobId, jobFilterContext.JobType, jobFilterContext.JobSubType, jobFilterContext.JobStatus, jobFilterContext.Key, jobFilterContext.SkipJobDataLoading, command, userName, jobFilterContext.DateTimeFrom, jobFilterContext.DateTimeTo, jobFilterContext.Locale, jobFilterContext.GetOnlyUserJobs);
            }
            finally
            {
                if (_currentTraceSettings.IsBasicTracingEnabled)
                {
                    diagnosticActivity.Stop();
                }
            }

            return jobs;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="jobId"></param>
        /// <param name="application"></param>
        /// <returns></returns>
        public Job Get(Int32 jobId, MDMCenterApplication application)
        {
            Job job = new Job();

            //TODO:: How to know module when we have only Job id...??
            DBCommandProperties command = DBCommandHelper.Get(application, GetModule(String.Empty), MDMCenterModuleAction.Read);

            String userName = PopulateUserName();
            const Int32 getKey = 0;

            JobCollection jobCollection = new JobDA().Get(jobId, JobType.UnKnown, JobSubType.All, JobStatus.UnKnown, getKey, false, command, userName);
            
            if (jobCollection != null && jobCollection.Count > 0)
            {
                job = jobCollection.First();
            }
            return job;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="jobType"></param>
        /// <param name="jobId"></param>
        /// <param name="application"></param>
        /// <returns></returns>
        public Job Get(JobType jobType, Int32 jobId, MDMCenterApplication application)
        {
            DiagnosticActivity diagnosticActivity = new DiagnosticActivity();

            if (_currentTraceSettings.IsBasicTracingEnabled)
            {
                diagnosticActivity.Start();
            }

            try
            {
                Job job = new Job();

                DBCommandProperties command = DBCommandHelper.Get(application, Utility.GetModule(jobType), MDMCenterModuleAction.Read);
                String userName = PopulateUserName();
                const Int32 getKey = 0;

                JobCollection jobCollection = new JobDA().Get(jobId, JobType.UnKnown, JobSubType.All, JobStatus.UnKnown, getKey, false, command, userName);
                if (jobCollection != null && jobCollection.Count > 0)
                {
                    job = jobCollection.First();

                    if (job.ProfileId > 0)
                    {
                        if (job.JobType == JobType.EntityExport || job.JobType == JobType.LookupExport)
                        {
                            //ExportProfileBL exportProfileBl = new ExportProfileBL();
                            //MDM.BusinessObjects.Exports.ExportProfile profile = exportProfileBl.Get(job.ProfileId, new CallerContext(application, MDMCenterModules.Export));
                            //if (profile != null)
                            //{
                            //    job.ProfileType = profile.ProfileType.GetDescription();
                            //}
                            //else
                            //{
                            //    diagnosticActivity.LogError(String.Format("No Export Profile found with profile Id = {0} and JobType = {1}", job.ProfileId, job.JobType.ToString()));
                            //}
                        }
                        else
                        {
                            ImportProfileBL importProfileBl = new ImportProfileBL();
                            job.ProfileType = importProfileBl.GetProfileType(job.ProfileId, new CallerContext(application, MDMCenterModules.Import), applySecurity: false);
                        }
                    }
                    else
                    {
                        diagnosticActivity.LogError("Profile Id not available in Job, cannot fetch profile detail for JobId = " + job.Id);
                    }

                }

                return job;
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
        /// 
        /// </summary>
        /// <param name="jobType"></param>
        /// <param name="jobSubType"></param>
        /// <param name="jobStatus"></param>
        /// <param name="application"></param>
        /// <returns></returns>
        public JobCollection Get(JobType jobType, JobSubType jobSubType, JobStatus jobStatus, MDMCenterApplication application)
        {
            return Get(jobType, jobSubType, jobStatus, false, application);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="jobType"></param>
        /// <param name="jobSubType"></param>
        /// <param name="jobStatus"></param>
        /// <param name="skipJobDataLoading"></param>
        /// <param name="application"></param>
        /// <returns></returns>
        public JobCollection Get(JobType jobType, JobSubType jobSubType, JobStatus jobStatus, Boolean skipJobDataLoading, MDMCenterApplication application)
        {
            DBCommandProperties command = DBCommandHelper.Get(application, Utility.GetModule(jobType), MDMCenterModuleAction.Read);
            String userName = PopulateUserName();
            const Int32 getKey = 3;
            return new JobDA().Get(0, jobType, jobSubType, jobStatus, getKey, skipJobDataLoading, command, userName);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="jobType"></param>
        /// <param name="application"></param>
        /// <returns></returns>
        public JobCollection GetJobsByType(JobType jobType, MDMCenterApplication application)
        {
            return GetJobsByType(jobType, false, application);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="jobType"></param>
        /// <param name="skipJobDataLoading"></param>
        /// <param name="application"></param>
        /// <returns></returns>
        public JobCollection GetJobsByType(JobType jobType, Boolean skipJobDataLoading, MDMCenterApplication application)
        {
            DBCommandProperties command = DBCommandHelper.Get(application, Utility.GetModule(jobType), MDMCenterModuleAction.Read);
            String userName = PopulateUserName();
            const Int32 getKey = 1; // 1 means get by job type
            return new JobDA().Get(0, jobType, JobSubType.All, JobStatus.UnKnown, getKey, skipJobDataLoading, command, userName);
        }

        public String GetChildJobsInXml(int parentJobId, MDMCenterApplication application)
        {
            DBCommandProperties command = DBCommandHelper.Get(application, MDMCenterModules.Import, MDMCenterModuleAction.Read);

            JobDA jobDa = new JobDA();
            DataTable childs = jobDa.GetChildJobs(parentJobId, command);

            StringWriter writer = new StringWriter();
            childs.WriteXml(writer);

            return writer.ToString();
        }

        #endregion

        /// <summary>
        /// Create the Job for requested profile Id and return the Job id.
        /// And place the file into data base.
        /// </summary>
        /// <param name="file">Indicates the File which need place to DB</param>
        /// <param name="importProfileId">Indicates the Import profile Id</param>
        /// <param name="importProfileName">Indicates the profile name</param>
        /// <param name="parentJobId">Indicates the Parent Job Id</param>
        /// <param name="callerContext">Caller context</param>
        /// <param name="jobType">Indicates the Job type. By default job type is Entity Import</param>
        /// <returns>Job ID</returns>
        public Int32 CreateJobInstance(File file, Int32 importProfileId, String importProfileName, Int32 parentJobId, CallerContext callerContext, JobType jobType = JobType.EntityImport)
        {
            Int32 fileId = UploadFileInDB(file, callerContext);

            Job job = new Job();

            job.Id = -1;
            job.Name = String.Format("Import Job, Created At: {0}", DateTime.Now.ToString());
            job.Description = "Job Created";
            //                job.CreatedUser = this.LoginUser;
            job.CreatedDateTime = DateTime.Now.ToString();
            job.JobStatus = JobStatus.Queued;
            job.JobType = jobType;
            job.JobSubType = JobSubType.API;
            job.ParentJobId = parentJobId;

            //                job.JobData.ExecutionStatus.CurrentStatusMessage = this.GetLocaleMessage("110384");

            job.ProfileId = importProfileId;
            job.JobData.ProfileId = importProfileId;

            job.ProfileName = importProfileName;

            job.JobData.JobParameters.Add(new JobParameter("FileId", fileId.ToString()));
            job.FileId = fileId;

            IRequestContextData contextData = MDMOperationContextHelper.GetRequestContextData();
            if (contextData.TraceSettings.TracingMode == TracingMode.OperationTracing)
            {
                job.JobData.JobParameters.Add(new JobParameter("ShowTrace", true.ToString()));
                job.JobData.JobParameters.Add(new JobParameter("OperationId", contextData.OperationId.ToString()));
            }

            CallerContext context = (CallerContext) callerContext.Clone();
            context.ProgramName = callerContext.Module.ToString();
            context.Module = Utility.GetModule(job.JobType);
            // Create job..
            Create(job, context); // This also updates job.Id with newly created id..

            return job.Id;
        }

        #endregion

        #region Private Methods
        
        private OperationResult Process(Job job, CallerContext callerContext)
        {
            String userName = PopulateUserName();
            String systemName = Dns.GetHostName();
            OperationResult jobProcessOperationResult = new OperationResult();

            //Populate 'Priority' if it is not available in case of 'EntityImport' Job...
            //Note:: Priority is defined only for the mode 'EntityImport' in MDMCenter.
            if (job.JobType == JobType.EntityImport && job.Priority < 1 && job.Action != ObjectAction.Delete)
            {
                try
                {
                    ImportProfileBL importProfileManager = new ImportProfileBL();
                    ImportProfile importProfile = importProfileManager.Get(job.ProfileId, callerContext);

                    if (importProfile != null && importProfile.ProcessingSpecifications != null)
                    {
                        job.Priority = importProfile.ProcessingSpecifications.Priority;
                    }
                }
                catch (Exception ex)
                {
                    //Exception has been handled here as Priority population failure should not stop the Job creation.
                    String errorMessage = String.Format("Failed to populate priority. Import profile get failed for profile Id '{0}' with error '{1}'.", job.ProfileId, ex.Message);
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, errorMessage, MDMTraceSource.Imports);
                }
            }

            using (
                TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required,
                    new TransactionOptions { IsolationLevel = Constants.DEFAULT_ISOLATION_LEVEL }))
            {
                callerContext.Module = Utility.GetModule(job.JobType);

                DBCommandProperties command = DBCommandHelper.Get(callerContext, MDMCenterModuleAction.Update);

                jobProcessOperationResult = new JobDA().Process(job, systemName, userName, command, callerContext.ProgramName);
                PopulateOperationResult(job, jobProcessOperationResult);

                LocalizeErrors(callerContext, jobProcessOperationResult);

                transactionScope.Complete();
            }

            return jobProcessOperationResult;
        }

        /// <summary>
        /// //if there are any error codes coming in OR from db, populate error messages for them
        /// </summary>
        /// <param name="callerContext">The caller context</param>
        /// <param name="entityTypeProcessOperationResult">Operation result to be modified</param>
        private void LocalizeErrors(CallerContext callerContext, OperationResult entityTypeProcessOperationResult)
        {
            foreach (Error error in entityTypeProcessOperationResult.Errors)
            {
                if (!String.IsNullOrEmpty(error.ErrorCode))
                {
                    _localeMessage = _localeMessageBL.Get(GlobalizationHelper.GetSystemUILocale(), error.ErrorCode, false, callerContext);

                    if (_localeMessage != null)
                    {
                        error.ErrorMessage = _localeMessage.Message;
                    }
                }
            }
        }

        private void PopulateOperationResult(Job job, OperationResult jobProcessOperationResult)
        {
            if (job.Action == ObjectAction.Create)
            {
                if (jobProcessOperationResult.ReturnValues.Any())
                {
                    jobProcessOperationResult.Id =
                        ValueTypeHelper.Int32TryParse(jobProcessOperationResult.ReturnValues[0].ToString(), -1);
                }
            }
            else
            {
                jobProcessOperationResult.Id = job.Id;
            }

            jobProcessOperationResult.ReferenceId = String.IsNullOrEmpty(job.ReferenceId)
                ? job.Name
                : job.ReferenceId;
        }

        private void ValidateInputParameters(Job job, CallerContext callerContext)
        {
            DiagnosticActivity diagnosticActivity = new DiagnosticActivity();

            if (_currentTraceSettings.IsBasicTracingEnabled)
            {
                diagnosticActivity.Start();
            }

            try
            {
                if (callerContext == null)
                {
                    diagnosticActivity.LogError("CallerContext cannot be null.");
                    throw new MDMOperationException("111846", "CallerContext cannot be null.", "JobManager.JobBL", String.Empty, "Create");
                }

                if (job == null)
                {
                    diagnosticActivity.LogError("Job cannot be null.");
                    throw new MDMOperationException("112262", "Job cannot be null.", "JobManager.JobBL", String.Empty, "Create");
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

        private CallerContext CorrectAndValidateInputParameters(Job job, MDMCenterApplication application, String programName)
        {
            CallerContext callerContext = new CallerContext();

            ValidateInputParameters(job, callerContext);

            callerContext.Application = application;
            callerContext.ProgramName = programName;
            callerContext.Module = Utility.GetModule(job.JobType);

            return callerContext;
        }

        private MDMCenterModuleAction GetModuleAction(ObjectAction action)
        {
            switch (action)
            {
                case ObjectAction.Create:
                    return MDMCenterModuleAction.Create;
                case ObjectAction.Update:
                    return MDMCenterModuleAction.Update;
                case ObjectAction.Delete:
                    return MDMCenterModuleAction.Delete;
                default:
                    return MDMCenterModuleAction.Read;
            }
        }

        /// <summary>
        /// Getting module of MDMCenter.
        /// </summary>
        /// <param name="module">This parameter is specifying module.</param>
        /// <returns>Return the MDMCenterModule name</returns>
        private MDMCenterModules GetModule(String module)
        {
            switch (module.ToString().ToLower())
            {
                case "catalogexport":
                case "exportschedule":
                case "lookupexport":
                    return MDMCenterModules.Export;
                default:
                    return MDMCenterModules.Import;
            }
        }

        private String PopulateUserName()
        {
            String userName = String.Empty;

            if (_securityPrincipal != null)
            {
                userName = _securityPrincipal.CurrentUserName;
            }

            return userName;
        }

        private static String PopulateProgramName(CallerContext callerContext)
        {
            return String.IsNullOrWhiteSpace(callerContext.ProgramName) ? "JobBL.Process" : callerContext.ProgramName;
        }

        private void GetSecurityPrincipal()
        {
            if (_securityPrincipal == null)
            {
                _securityPrincipal = SecurityPrincipalHelper.GetCurrentSecurityPrincipal();
            }
        }

        private void ValidateCallerContext(CallerContext callerContext)
        {
            DiagnosticActivity diagnosticActivity = new DiagnosticActivity();

            if (_currentTraceSettings.IsBasicTracingEnabled)
            {
                diagnosticActivity.Start();
            }

            try
            {
                if (callerContext == null)
                {
                    diagnosticActivity.LogError("CallerContext cannot be null.");
                    throw new MDMOperationException("111846", "CallerContext cannot be null.", "JobManager.JobBL", String.Empty, "Create");
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
        /// 
        /// </summary>
        /// <param name="file"></param>
        /// <param name="callerContext"></param>
        /// <returns></returns>
        public int UploadFileInDB(File file,CallerContext callerContext)
        {
            Int32 fileId = 0;
            FileBL fileBL = new FileBL();
            file.Action = ObjectAction.Create;
            fileId = fileBL.Process(file, callerContext);
            return fileId;
        }

        #endregion

        #region Legacy methods

        /// <summary>
        /// 
        /// </summary>
        public class LegacyJobMethods
        {
            private JobDA.JobLegacyMethods _joblegacyMethodsDA = new JobDA.JobLegacyMethods();

            /// <summary>
            /// Add new job
            /// </summary>
            /// <param name="type">This parameter is specifying type of job.</param>
            /// <param name="subType">This parameter is specifying job sub type.</param>
            /// <param name="profileName">This parameter is specifying profile name.</param>
            /// <param name="shortName">This parameter is specifying job short name.</param>
            /// <param name="description">This parameter is specifying job description.</param>
            /// <param name="content">his parameter is specifying job content.</param>
            /// <param name="status">This parameter is specifying job status.</param>
            /// <param name="application">Name of application which is performing action</param>
            /// <returns>Number of jobs created.</returns>
            public Int32 Add(String type, String subType, String profileName, String shortName, String description, String content, String status, MDMCenterApplication application)
            {
                String loginUser = SecurityPrincipalHelper.GetCurrentSecurityPrincipal().CurrentUserName;
                String systemName = Dns.GetHostName();

                DBCommandProperties command = DBCommandHelper.Get(application, GetModule(type), MDMCenterModuleAction.Create);

                return _joblegacyMethodsDA.Add(type, subType, profileName, shortName, description, content, status, systemName, loginUser, command);
            }

            /// <summary>
            /// Update UserAction of Job
            /// </summary>
            /// <param name="jobId">Id of the Job</param>
            /// <param name="jobAction">UserAction of Job. UserAction can be Pause/Continue/Abort/Retry/Delete </param>
            /// <param name="jobType">Specifying type of Job</param>
            /// <param name="application">Name of application which is performing action</param>
            /// <returns>True if Update is Successful.</returns>
            public Boolean UpdateUserAction(Int32 jobId, JobAction jobAction, String jobType, MDMCenterApplication application)
            {
                Boolean result = false;
                DBCommandProperties command = DBCommandHelper.Get(application, GetModule(jobType), MDMCenterModuleAction.Update);

                result = _joblegacyMethodsDA.UpdateUserAction(jobId, jobAction, command);

                return result;
            }

            /// <summary>
            /// Get all jobs of specific job type.
            /// </summary>
            /// <param name="type">This parameter is specifying job type.</param>
            /// <param name="sql">This parameter is to filter search data based on sql.</param>
            /// <param name="application">Name of application which is performing action</param>
            /// <returns>Return collection of job</returns>
            public Collection<Job> Get(String type, String sql, MDMCenterApplication application)
            {
                String loginUser = SecurityPrincipalHelper.GetCurrentSecurityPrincipal().CurrentUserName;
                DBCommandProperties command = DBCommandHelper.Get(application, GetModule(type), MDMCenterModuleAction.Read);

                return _joblegacyMethodsDA.Get(type, loginUser, sql, command);
            }

            /// <summary>
            /// Get Job status details by the Job id.
            /// </summary>
            /// <param name="id">This parameter is specifying an job</param>
            /// <param name="jobType">This parameter is specifying type of job</param>
            /// <param name="application">Name of application which is performing action</param>
            /// <returns>return a job details in form of XML.</returns>
            public String GetJobItem(Int32 id, String jobType, MDMCenterApplication application)
            {
                DBCommandProperties command = DBCommandHelper.Get(application, GetModule(jobType), MDMCenterModuleAction.Read);

                return _joblegacyMethodsDA.GetJobItem(id, command);
            }

            /// <summary>
            /// Getting module of MDMCenter.
            /// </summary>
            /// <param name="module">This parameter is specifying module.</param>
            /// <returns>Return the MDMCenterModule name</returns>
            private MDMCenterModules GetModule(String module)
            {
                switch (module.ToString().ToLower())
                {
                    case "catalogexport":
                    case "exportschedule":
                    case "lookupexport":
                        return MDMCenterModules.Export;
                    default:
                        return MDMCenterModules.Import;
                }
            }
        }

        #endregion
    }
}
