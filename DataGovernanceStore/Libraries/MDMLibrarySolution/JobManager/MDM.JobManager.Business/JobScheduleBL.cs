using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Transactions;

namespace MDM.JobManager.Business
{
    using MDM.BusinessObjects;
    using MDM.BusinessObjects.Jobs;
    using MDM.BusinessObjects.Exports;
    using MDM.ConfigurationManager.Business;
    using MDM.Core;
    using MDM.Core.Exceptions;
    using MDM.Interfaces;
    using MDM.JobManager.Data.SqlClient;
    using MDM.MessageManager.Business;
    using MDM.Utility;
    using MDM.BusinessObjects.Diagnostics;

    /// <summary>
    /// Specifies job schedule manager class
    /// </summary>
    public class JobScheduleBL : BusinessLogicBase
    {
        #region Fields

        private JobScheduleDA _jobScheduleDA = new JobScheduleDA();
        private SecurityPrincipal _securityPrincipal = null;
        private LocaleMessageBL _localeMessageBL = new LocaleMessageBL();
        private LocaleMessage _localeMessage = null;
        private LocaleEnum systemDataLocale = GlobalizationHelper.GetSystemUILocale();

        /// <summary>
        /// Indicates current trace settings
        /// </summary>
        private TraceSettings _currentTraceSettings = new TraceSettings();

        #endregion

        #region Constructor

        /// <summary>
        /// Default Constructor
        /// </summary>
        public JobScheduleBL()
        {
            GetSecurityPrincipal();
            _currentTraceSettings = MDMOperationContextHelper.GetCurrentTraceSettings();
        }

        #endregion Constructor

        #region Get

        /// <summary>
        /// Get all the job Schedules
        /// </summary>
        /// <param name="scheduleId">This parameter is specifying the Schedule Id</param>
        /// <param name="jobType">This parameter is specifying the job Type</param>
        /// <param name="application">Application name which is performing action</param>
        /// <returns>Returns job collections and profile collections</returns>
        public Tuple<Collection<Job>, Collection<ExportProfile>> GetSchedule(Int32 scheduleId, String jobType, MDMCenterApplication application)
        {
            _jobScheduleDA = new JobScheduleDA();

            DBCommandProperties command = DBCommandHelper.Get(application, GetModule(jobType), MDMCenterModuleAction.Read);

            return _jobScheduleDA.GetSchedule(scheduleId, command);
        }

        /// <summary>
        /// Get all jobSchedules
        /// </summary>
        /// <param name="callerContext">Context of application making call for this API</param>
        /// <returns>All schedule Criteria</returns>
        public JobScheduleCollection GetAll(CallerContext callerContext)
        {
            DiagnosticActivity diagnosticActivity = new DiagnosticActivity();

            if (_currentTraceSettings.IsBasicTracingEnabled)
            {
                diagnosticActivity.Start();
            }

            try
            {
                diagnosticActivity.LogInformation("Start - JobScheduleBL.GetAll call");

                if (callerContext == null)
                {
                    diagnosticActivity.LogError("CallerContext cannot be null.");
                    throw new MDMOperationException("111846", "CallerContext cannot be null.", "JobScheduleBL.GetAll", String.Empty, "GetAll");
                }

                JobScheduleCollection jobSchedules = new JobScheduleCollection();

                jobSchedules = this.GetSchedules(0, callerContext);

                diagnosticActivity.LogInformation("End - JobScheduleBL.GetAll call");

                return jobSchedules;
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
        /// Get jobSchedule based on Id
        /// </summary>
        /// <param name="scheduleId">Id of schedule to be fetched</param>
        /// <param name="callerContext">Context of application making call for this API</param>
        /// <returns>Schedule with given Id</returns>
        public JobSchedule GetById(Int32 scheduleId, CallerContext callerContext)
        {
            DiagnosticActivity diagnosticActivity = new DiagnosticActivity();

            if (_currentTraceSettings.IsBasicTracingEnabled)
            {
                diagnosticActivity.Start();
            }

            try
            {
                diagnosticActivity.LogInformation("Start - JobScheduleBL.GetById call");

                if (callerContext == null)
                {
                    diagnosticActivity.LogError("CallerContext cannot be null.");
                    throw new MDMOperationException("111846", "CallerContext cannot be null.", "JobScheduleBL.GetById", String.Empty, "GetById");
                }

                JobSchedule jobSchedule = new JobSchedule();

                JobScheduleCollection schedules = this.GetSchedules(scheduleId, callerContext);
                if (schedules != null)
                {
                    jobSchedule = schedules.FirstOrDefault();
                }

                diagnosticActivity.LogInformation("End - JobScheduleBL.GetById call");
                return jobSchedule;
            }
            finally
            {
                if (_currentTraceSettings.IsBasicTracingEnabled)
                {
                    diagnosticActivity.Stop();
                }
            }

        }

        #endregion Get

        #region CUD

        /// <summary>
        /// Add the job schedule details.
        /// </summary>
        /// <param name="scheduleName">This parameter is specifying the Schedule Name</param>
        /// <param name="scheduleLongName">This parameter is specifying the Schedule Long name</param>
        /// <param name="content">This parameter is specifying the Schedule content</param>
        /// <param name="isEnable">This parameter is specifying whether it is enable or not</param>
        /// <param name="profiles">This parameter is specifying the profiles</param>
        /// <param name="programName">This parameter is specifying the program Name</param>
        /// <param name="jobType">This parameter is specifying the job Type</param>
        /// <param name="application">Name of application which is performing action</param>
        public void AddSchedule(String scheduleName, String scheduleLongName, String content, Boolean isEnable, String profiles, String programName, String jobType, MDMCenterApplication application)
        {
            _jobScheduleDA = new JobScheduleDA();
            String loginUser = SecurityPrincipalHelper.GetCurrentSecurityPrincipal().CurrentUserName;
            String systemName = Dns.GetHostName();

            DBCommandProperties command = DBCommandHelper.Get(application, GetModule(jobType), MDMCenterModuleAction.Create);

            _jobScheduleDA.AddSchedule(scheduleName, scheduleLongName, content, isEnable, systemName, profiles, loginUser, programName, command);

            //ScheduleCriteria scheduleCriteria = new ScheduleCriteria();
            //scheduleCriteria.Name = scheduleName;
            //scheduleCriteria.LongName = scheduleLongName;
            //scheduleCriteria.Enabled = isEnable;

            ////UI sends profile Ids in comma separated string. So loop and get the ids
            //foreach (String id in profiles.Split(','))
            //{
            //    KeyValuePair<Int32, String> pair = new KeyValuePair<Int32, String>(ValueTypeHelper.Int32TryParse(id, 0), String.Empty);
            //    scheduleCriteria.ProfileIdNamePair.Add(pair);
            //}

            //scheduleCriteria.Action = ObjectAction.Create;

            //this.Process(scheduleCriteria, programName, new CallerContext(application, GetModule(jobType)));
        }

        /// <summary>
        /// Update the job schedule details
        /// </summary>
        /// <param name="scheduleId">This parameter is specifying the schedule Id</param>
        /// <param name="scheduleName">This parameter is specifying the schedule Name</param>
        /// <param name="scheduleLongName">This parameter is specifying the schedule long Name</param>
        /// <param name="content">This parameter is specifying the schedule content</param>
        /// <param name="isEnable">This parameter is specifying whether it is enable or not</param>
        /// <param name="profiles">This parameter is specifying the profiles</param>
        /// <param name="programName">This parameter is specifying the program Name</param>
        /// <param name="jobType">This parameter is specifying the job Type</param>
        /// <param name="application">Name of application which is performing action</param>
        public void UpdateSchedule(Int32 scheduleId, String scheduleName, String scheduleLongName, String content, Boolean isEnable, String profiles, String programName, String jobType, MDMCenterApplication application)
        {
            String loginUser = SecurityPrincipalHelper.GetCurrentSecurityPrincipal().CurrentUserName;
            String systemName = Dns.GetHostName();

            DBCommandProperties command = DBCommandHelper.Get(application, GetModule(jobType), MDMCenterModuleAction.Update);

            _jobScheduleDA.UpdateSchedule(scheduleId, scheduleName, scheduleLongName, content, isEnable, systemName, profiles, loginUser, programName, command);
            //ScheduleCriteria scheduleCriteria = new ScheduleCriteria();
            //scheduleCriteria.Id = scheduleId;
            //scheduleCriteria.Name = scheduleName;
            //scheduleCriteria.LongName = scheduleLongName;
            //scheduleCriteria.Enabled = isEnable;

            ////UI sends profile Ids in comma separated string. So loop and get the ids
            //foreach (String id in profiles.Split(','))
            //{
            //    KeyValuePair<Int32, String> pair = new KeyValuePair<Int32, String>(ValueTypeHelper.Int32TryParse(id, 0), String.Empty);
            //    scheduleCriteria.ProfileIdNamePair.Add(pair);
            //}

            //scheduleCriteria.Action = ObjectAction.Create;

            //this.Process(scheduleCriteria, programName, new CallerContext(application, GetModule(jobType)));
        }

        /// <summary>
        /// Delete the job schedule.
        /// </summary>
        /// <param name="scheduleId">This parameter is specifying the schedule Id</param>
        /// <param name="jobType">This parameter is specifying the job Type</param>
        public void DeleteSchedule(Int32 scheduleId, String jobType, MDMCenterApplication application)
        {
            _jobScheduleDA = new JobScheduleDA();

            DBCommandProperties command = DBCommandHelper.Get(application, GetModule(jobType), MDMCenterModuleAction.Delete);

            _jobScheduleDA.DeleteSchedule(scheduleId, command);
        }

        /// <summary>
        /// Create new JobSchedule
        /// </summary>
        /// <param name="jobSchedule">Represent JobSchedule Object</param>
        /// <param name="callerContext">Context of application making call for this API</param>
        /// <param name="programName">Name of program making change</param>
        /// <returns>OperationResult of JobSchedule Creation</returns>
        /// <exception cref="ArgumentNullException">If JobSchedule Object is Null</exception>
        /// <exception cref="ArgumentNullException">If JobSchedule ShortName is Null or having empty String</exception>
        /// <exception cref="ArgumentNullException">If JobSchedule LongName is Null or having empty String</exception>
        public OperationResult Create(JobSchedule jobSchedule, CallerContext callerContext)
        {
            ValidateSchedule(jobSchedule, "Create");

            jobSchedule.Action = Core.ObjectAction.Create;
            return this.Process(jobSchedule, callerContext);
        }

        /// <summary>
        /// Update existing jobSchedule
        /// </summary>
        /// <param name="jobSchedule">Represent JobSchedule Object</param>
        /// <param name="callerContext">Context of application making call for this API</param>
        /// <param name="programName">Name of program making change</param>
        /// <returns>OperationResult of JobSchedule Updating</returns>
        /// <exception cref="ArgumentNullException">If JobSchedule Object is Null</exception>
        /// <exception cref="ArgumentNullException">If JobSchedule ShortName is Null or having empty String</exception>
        /// <exception cref="ArgumentNullException">If JobSchedule LongName is Null or having empty String</exception>
        public OperationResult Update(JobSchedule jobSchedule, CallerContext callerContext)
        {
            ValidateSchedule(jobSchedule, "Update");

            jobSchedule.Action = Core.ObjectAction.Update;
            return this.Process(jobSchedule, callerContext);
        }

        /// <summary>
        /// Delete jobSchedule
        /// </summary>
        /// <param name="callerContext">Context of application making call for this API</param>
        /// <param name="programName">Name of program making change</param>
        /// <returns>OperationResult of JobSchedule Deletion</returns>
        /// <returns>True if JobSchedule Creation is Successful</returns>
        /// <exception cref="ArgumentNullException">If JobSchedule Object is Null</exception>
        public OperationResult Delete(JobSchedule jobSchedule, CallerContext callerContext)
        {
            ValidateSchedule(jobSchedule, "Delete");

            jobSchedule.Action = Core.ObjectAction.Delete;
            return this.Process(jobSchedule, callerContext);
        }

        /// <summary>
        /// Process jobSchedules
        /// </summary>
        /// <param name="containers">JobSchedules to process</param>
        /// <param name="callerContext">Context of application making call for this API</param>
        /// <param name="programName">Name of program making change</param>
        /// <returns>OperationResult of JobSchedule Creation</returns>
        public OperationResultCollection Process(JobScheduleCollection jobSchedules, CallerContext callerContext)
        {
            DiagnosticActivity diagnosticActivity = new DiagnosticActivity();

            if (_currentTraceSettings.IsBasicTracingEnabled)
            {
                diagnosticActivity.Start();
            }

            try
            {
                if (jobSchedules == null)
                {
                    diagnosticActivity.LogError("JobSchedule cannot be null");
                    throw new MDMOperationException("112114", "JobSchedule cannot be null", "JobScheduleBL.Process", String.Empty, "Process");
                }

                OperationResultCollection operationResults = new OperationResultCollection();

                foreach (JobSchedule schedule in jobSchedules)
                {
                    OperationResult scheduleOR = this.Process(schedule, callerContext);
                    if (scheduleOR != null)
                    {
                        operationResults.Add(scheduleOR);
                    }
                }

                return operationResults;
            }
            finally
            {
                if (_currentTraceSettings.IsBasicTracingEnabled)
                {
                    diagnosticActivity.Stop();
                }
            }
        }

        #endregion CUD

        #region Private Methods

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

        /// <summary>
        /// 
        /// </summary>
        private void GetSecurityPrincipal()
        {
            if (_securityPrincipal == null)
            {
                _securityPrincipal = SecurityPrincipalHelper.GetCurrentSecurityPrincipal();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="scheduleId"></param>
        /// <param name="callerContext"></param>
        /// <returns></returns>
        private JobScheduleCollection GetSchedules(Int32 scheduleId, CallerContext callerContext)
        {
            DiagnosticActivity diagnosticActivity = new DiagnosticActivity();

            if (_currentTraceSettings.IsBasicTracingEnabled)
            {
                diagnosticActivity.Start();
            }

            try
            {
                JobScheduleCollection jobSchedules = new JobScheduleCollection();
                //Get command
                DBCommandProperties command = DBCommandHelper.Get(callerContext, Core.MDMCenterModuleAction.Read);

                diagnosticActivity.LogInformation("Start - JobScheduleDA.GetAll call");

                jobSchedules = _jobScheduleDA.Get(scheduleId, command);

                diagnosticActivity.LogInformation("End - JobScheduleDA.GetAll call");

                return jobSchedules;
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
        /// <param name="jobSchedule"></param>
        /// <param name="methodName"></param>
        private void ValidateSchedule(JobSchedule jobSchedule, String methodName)
        {
            DiagnosticActivity diagnosticActivity = new DiagnosticActivity();

            if (_currentTraceSettings.IsBasicTracingEnabled)
            {
                diagnosticActivity.Start();
            }

            try
            {
                if (jobSchedule == null)
                {
                    diagnosticActivity.LogError("JobSchedule cannot be null");
                    throw new MDMOperationException("112114", "JobSchedule cannot be null", "JobScheduleBL" + methodName, String.Empty, methodName);
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
        /// Process jobSchedule
        /// </summary>
        /// <param name="jobSchedule">JobSchedule to process</param>
        /// <param name="callerContext">Context indicating who is calling the API</param>
        /// <param name="programName">Name of program doing the change</param>
        private OperationResult Process(JobSchedule jobSchedule, CallerContext callerContext)
        {
            DiagnosticActivity diagnosticActivity = new DiagnosticActivity();

            if (_currentTraceSettings.IsBasicTracingEnabled)
            {
                diagnosticActivity.Start();
            }

            try
            {
                if (_currentTraceSettings.IsBasicTracingEnabled)
                {
                    diagnosticActivity.LogInformation("Start - JobScheduleBL.Process");
                }

                #region Parameter Validation

                if (jobSchedule == null)
                {
                    diagnosticActivity.LogError("JobSchedule cannot be null");
                    throw new MDMOperationException("112114", "JobSchedule cannot be null", "JobScheduleBL.Process", String.Empty, "Process");
                }

                if (callerContext == null)
                {
                    diagnosticActivity.LogError("CallerContext cannot be null.");
                    throw new MDMOperationException("111846", "CallerContext cannot be null.", "JobScheduleBL.Process", String.Empty, "Process");
                }

                if (String.IsNullOrWhiteSpace(jobSchedule.Name) && jobSchedule.Action != ObjectAction.Delete)
                {
                    diagnosticActivity.LogError("Schedule Criteria Short Name cannot be empty");
                    throw new MDMOperationException("100059", "Schedule Criteria Short Name cannot be empty", "JobScheduleBL.Process", String.Empty, "Process");
                }

                if (String.IsNullOrWhiteSpace(jobSchedule.LongName) && jobSchedule.Action != ObjectAction.Delete)
                {
                    diagnosticActivity.LogError("Schedule Criteria Long Name cannot be empty");
                    throw new MDMOperationException("100060", "Schedule Criteria Long Name cannot be empty", "JobScheduleBL.Process", String.Empty, "Process");
                }

                #endregion

                OperationResult scheduleOR = null;

                String userName = String.Empty;
                if (_securityPrincipal != null)
                {
                    userName = _securityPrincipal.CurrentUserName;
                }

                if (String.IsNullOrWhiteSpace(callerContext.ProgramName))
                {
                    callerContext.ProgramName = "MDM.JobManager.Business.JobScheduleBL.Process";
                }

                //Get command
                DBCommandProperties command = DBCommandHelper.Get(callerContext, Core.MDMCenterModuleAction.Update);

                using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions() { IsolationLevel = Constants.DEFAULT_ISOLATION_LEVEL }))
                {
                    diagnosticActivity.LogInformation("Start - JobScheduleDA.Process call");

                    scheduleOR = _jobScheduleDA.Process(jobSchedule, callerContext.ProgramName, userName, command);

                    diagnosticActivity.LogInformation("End - JobScheduleDA.Process call");

                    if (scheduleOR != null && scheduleOR.OperationResultStatus == Core.OperationResultStatusEnum.None || scheduleOR.OperationResultStatus == Core.OperationResultStatusEnum.Successful)
                    {
                        transactionScope.Complete();
                    }
                    else
                    {
                        #region Update OR with messages for error code

                        //if there are any error codes coming in OR from db, populate error messages for them
                        foreach (Error error in scheduleOR.Errors)
                        {
                            if (!String.IsNullOrEmpty(error.ErrorCode))
                            {
                                _localeMessage = _localeMessageBL.Get(systemDataLocale, error.ErrorCode, false, callerContext);

                                if (_localeMessage != null)
                                {
                                    error.ErrorMessage = _localeMessage.Message;
                                }
                            }
                        }

                        #endregion Update OR with messages for error code
                    }
                }
                if (_currentTraceSettings.IsBasicTracingEnabled)
                {
                    diagnosticActivity.LogInformation("End - JobScheduleBL.Process");
                }

                return scheduleOR;
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
    }
}
