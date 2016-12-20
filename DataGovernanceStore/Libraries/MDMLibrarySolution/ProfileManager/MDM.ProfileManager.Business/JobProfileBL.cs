using System;
using System.Diagnostics;
using System.Text;

namespace MDM.ProfileManager.Business
{
    using MDM.Core;
    using MDM.BusinessObjects;
    using MDM.BusinessObjects.Imports;
    using MDM.ConfigurationManager.Business;
    using MDM.Utility;

    using MDM.ProfileManager.Data;
    using MDM.Core.Exceptions;
    using MDM.MessageManager.Business;

    public class JobProfileBL : BusinessLogicBase
    {
        #region Fields

        private SecurityPrincipal _securityPrincipal = null;
        private LocaleMessageBL _localeMessageBL = new LocaleMessageBL();
        private LocaleMessage _localeMessage = null;
        private LocaleEnum systemDataLocale = GlobalizationHelper.GetSystemUILocale();

        #endregion

        #region Constructor

        public JobProfileBL()
        {
            GetSecurityPrincipal();
        }

        #endregion Constructor

        #region CUD methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="jobProfile"></param>
        /// <param name="operationResult"></param>
        /// <param name="programName"></param>
        /// <returns></returns>
        public OperationResult Create(JobProfile jobProfile, CallerContext callerContext)
        {
            ValidateProfile(jobProfile, "Create");

            jobProfile.Action = ObjectAction.Create;
            return Process(jobProfile, callerContext);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="jobProfile"></param>
        /// <param name="operationResult"></param>
        /// <param name="programName"></param>
        /// <returns></returns>
        public OperationResult Update(JobProfile jobProfile, CallerContext callerContext)
        {
            ValidateProfile(jobProfile, "Update");

            jobProfile.Action = ObjectAction.Update;
            return Process(jobProfile, callerContext);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="jobProfile"></param>
        /// <param name="operationResult"></param>
        /// <param name="programName"></param>
        /// <returns></returns>
        public OperationResult Delete(JobProfile jobProfile, CallerContext callerContext)
        {
            ValidateProfile(jobProfile, "Delete");

            jobProfile.Action = ObjectAction.Delete;
            return Process(jobProfile, callerContext);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="jobProfile"></param>
        /// <param name="application"></param>
        /// <param name="programName"></param>
        /// <returns></returns>
        public OperationResultCollection Process(JobProfileCollection jobProfiles, CallerContext callerContext)
        {
            if (jobProfiles == null)
            {
                throw new MDMOperationException("333", "JobProfile cannot be null.", "CustomProfileBL.Process" , String.Empty, "Process");
            }

            OperationResultCollection operationResults = new OperationResultCollection();
            foreach (JobProfile profile in jobProfiles)
            {
                operationResults.Add(this.Process(profile, callerContext));
            }

            return operationResults;
        }

        #endregion

        #region Get Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="profileId"></param>
        /// <returns></returns>
        public JobProfile Get(Int32 profileId, MDMCenterApplication application, MDMCenterModules module)
        {
            MDMTraceHelper.EmitTraceEvent(TraceEventType.Verbose, "Getting command", MDMTraceSource.JobService);

            DBCommandProperties command = DBCommandHelper.Get(application, module, MDMCenterModuleAction.Read);

            MDMTraceHelper.EmitTraceEvent(TraceEventType.Verbose, "Got command : Connection string : " + command.ConnectionString, MDMTraceSource.JobService);
            MDMTraceHelper.EmitTraceEvent(TraceEventType.Verbose, "Start : DB call to get job profile by id ", MDMTraceSource.JobService);

            JobProfile jobProfile = new JobProfileDA().Get(profileId, String.Empty, command);

            MDMTraceHelper.EmitTraceEvent(TraceEventType.Verbose, "End : DB call to get job profile by id ", MDMTraceSource.JobService);

            return jobProfile;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="profileId"></param>
        /// <returns></returns>
        public JobProfile Get(String profileName, MDMCenterApplication application, MDMCenterModules module)
        {
            MDMTraceHelper.EmitTraceEvent(TraceEventType.Verbose, "Getting command", MDMTraceSource.JobService);

            DBCommandProperties command = DBCommandHelper.Get(application, module, MDMCenterModuleAction.Read);

            MDMTraceHelper.EmitTraceEvent(TraceEventType.Verbose, "Got command : Connection string : " + command.ConnectionString, MDMTraceSource.JobService);
            MDMTraceHelper.EmitTraceEvent(TraceEventType.Verbose, "Start : DB call to get job profile by name ", MDMTraceSource.JobService);

            JobProfile jobProfile = new JobProfileDA().Get(0, profileName, command);

            MDMTraceHelper.EmitTraceEvent(TraceEventType.Verbose, "End : DB call to get job profile by name ", MDMTraceSource.JobService);

            return jobProfile;
        }

        /// <summary>
        /// Get all profiles based on requested job type.
        /// Job type parameter is optional. By default is unknown.
        /// If it is unknown it will return all available profile else requested job type profiles will be return.
        /// </summary>
        /// <param name="application">Indicates the mdmcenter application</param>
        /// <param name="module">Indicates the the mdmcenter module</param>
        /// <param name="profileId">Indicted the profile id</param>
        /// <param name="jobType">Indicates the the job type. It is a optional parameter</param>
        /// <returns>Return the Job profile collection</returns>
        public JobProfileCollection GetAll(MDMCenterApplication application, MDMCenterModules module, JobType jobType = JobType.UnKnown)
        {
            MDMTraceHelper.EmitTraceEvent(TraceEventType.Verbose, "Getting command", MDMTraceSource.JobService);

            DBCommandProperties command = DBCommandHelper.Get(application, module, MDMCenterModuleAction.Read);

            MDMTraceHelper.EmitTraceEvent(TraceEventType.Verbose, "Got command : Connection string : " + command.ConnectionString, MDMTraceSource.JobService);
            MDMTraceHelper.EmitTraceEvent(TraceEventType.Verbose, "Start : DB call to get all job profiles ", MDMTraceSource.JobService);

            String jobTypeAsString = String.Empty;

            if (jobType != JobType.UnKnown)
            {
                jobTypeAsString = jobType.ToString();
            }

            JobProfileCollection jobProfileCollection = new JobProfileDA().GetAll(command, jobTypeAsString);

            MDMTraceHelper.EmitTraceEvent(TraceEventType.Verbose, "End : DB call to get all job profiles ", MDMTraceSource.JobService);

            return jobProfileCollection;
        }
        #endregion

        #region Private Methods

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

        private void ValidateProfile(JobProfile profile, String methodName)
        {
            if (profile == null)
            {
                throw new MDMOperationException("333", "JobProfile cannot be null.", "CustomProfileBL." + methodName, String.Empty, methodName);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="jobProfile"></param>
        /// <param name="application"></param>
        /// <param name="programName"></param>
        /// <returns></returns>
        private OperationResult Process(JobProfile jobProfile, CallerContext callerContext)
        {
            #region Parameter Validation

            if (jobProfile == null)
            {
                throw new MDMOperationException("333", "JobProfile cannot be null.", "JobProfileBL.Process", String.Empty, "Process");
            }

            if (callerContext == null)
            {
                throw new MDMOperationException("111846", "CallerContext cannot be null.", "JobProfileBL.Process", String.Empty, "Process");
            }

            #endregion

            String userName = SecurityPrincipalHelper.GetCurrentSecurityPrincipal().CurrentUserName;

            if (callerContext.Module == MDMCenterModules.Unknown)
            {
                callerContext.Module = MDMCenterModules.Import;
            }

            DBCommandProperties command = DBCommandHelper.Get(callerContext, GetModuleAction(jobProfile.Action));

            if (String.IsNullOrWhiteSpace(callerContext.ProgramName))
            {
                callerContext.ProgramName = "MDM.ProfileManager.Business.JobProfileBL.Process";
            }

            OperationResult or = new JobProfileDA().Process(jobProfile, userName, command, callerContext.ProgramName);

            if (or.OperationResultStatus != OperationResultStatusEnum.None && or.OperationResultStatus != OperationResultStatusEnum.Successful)
            {
                #region Update OR with messages for error code

                //if there are any error codes coming in OR from db, populate error messages for them
                foreach (Error error in or.Errors)
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

            return or;
        }

        private void GetSecurityPrincipal()
        {
            if (_securityPrincipal == null)
            {
                _securityPrincipal = SecurityPrincipalHelper.GetCurrentSecurityPrincipal();
            }
        }

        #endregion
    }

}