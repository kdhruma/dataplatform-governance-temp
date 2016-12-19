using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.ServiceModel;


namespace MDM.WCFServices
{
    using Core;
    using Core.Exceptions;
    using BusinessObjects;
    using BusinessObjects.Imports;
    using MDM.BusinessObjects.Exports;
    using BusinessObjects.Jobs;
    using JobManager.Business;
    using MDM.Utility;
    using ProfileManager.Business;
    using MDM.BusinessObjects.Integration;
    using MDM.IntegrationManager.Business;
    using WCFServiceInterfaces;
    using MDM.BusinessObjects.DataModel;

    [DiagnosticActivity]
    [ServiceBehavior(Namespace = "http://wcfservices.riversand.com", InstanceContextMode = InstanceContextMode.PerCall)]
    public class IntegrationService : MDMWCFBase, IIntegrationService
    {
        /// <summary>
        /// Gets the requested job
        /// </summary>
        /// <param name="jobId">Id of the Job</param>
        /// <param name="jobType">Type of the Job</param>
        /// <param name="application">Name of application which is performing action</param>
        /// <returns>The job object</returns>
        public Job GetJob(Int32 jobId, JobType jobType, MDMCenterApplication application)
        {
            return MakeBusinessLogicCall<JobBL, Job>(
                bl => bl.Get(jobType, jobId, application),
                context =>
                {
                    context.CallerContext.JobId = jobId;
                });
        }

        /// <summary>
        /// Gets jobs with the requested JobType, JobSubType and JobStatus
        /// </summary>
        /// <param name="jobType">Type of the Job</param>
        /// <param name="jobSubType">Subtype of the Job</param>
        /// <param name="jobStatus">Status of the Job</param>
        /// <param name="application">Name of application which is performing action</param>
        /// <returns>Collection of requested Jobs</returns>
        public JobCollection GetJobs(JobType jobType, JobSubType jobSubType, JobStatus jobStatus, MDMCenterApplication application)
        {
            return GetJobs(jobType, jobSubType, jobStatus, false, application);
        }

        /// <summary>
        /// Gets jobs with the requested JobType, JobSubType and JobStatus
        /// </summary>
        /// <param name="jobType">Type of the Job</param>
        /// <param name="jobSubType">Subtype of the Job</param>
        /// <param name="jobStatus">Status of the Job</param>
        /// <param name="skipJobDataLoading">Please set to True if you want to skip <code>Job.JobData</code> and <code>Job.JobDataXml</code> information loading</param>
        /// <param name="application">Name of application which is performing action</param>
        /// <returns>Collection of requested Jobs</returns>
        public JobCollection GetJobs(JobType jobType, JobSubType jobSubType, JobStatus jobStatus, Boolean skipJobDataLoading, MDMCenterApplication application)
        {
            JobCollection jobCollection = null;

            try
            {
                JobBL jobManager = new JobBL();
                jobCollection = jobManager.Get(jobType, jobSubType, jobStatus, skipJobDataLoading, application);
            }
            catch (Exception ex)
            {
                this.LogException(ex);
                throw base.WrapException(ex);
            }

            return jobCollection;
        }

        /// <summary>
        /// Gets jobs with the requested JobType and SQL filter
        /// </summary>
        /// <param name="jobType">Type of the Job</param>
        /// <param name="sql">This parameter is to filter search data based on sql</param>
        /// <param name="application">Name of application which is performing action</param>
        /// <returns>The job object</returns>
        public JobCollection GetJobs(JobType jobType, String sql, MDMCenterApplication application)
        {
            JobCollection result = null;

            try
            {
                JobBL.LegacyJobMethods jobManager = new JobBL.LegacyJobMethods();
                Collection<Job> items = jobManager.Get(jobType.ToString(), sql, application);
                result = new JobCollection(items);
            }
            catch (Exception ex)
            {
                this.LogException(ex);
                throw base.WrapException(ex);
            }

            return result;
        }

        /// <summary>
        /// Gets jobs with the requested JobType
        /// </summary>
        /// <param name="jobType">Type of the Job</param>
        /// <param name="application">Name of application which is performing action</param>
        /// <returns>Collection of requested Jobs</returns>
        public JobCollection GetJobsByType(JobType jobType, MDMCenterApplication application)
        {
            return GetJobsByType(jobType, false, application);
        }

        /// <summary>
        /// Gets jobs with the requested JobType
        /// </summary>
        /// <param name="jobType">Type of the Job</param>
        /// <param name="skipJobDataLoading">Please set to True if you want to skip <code>Job.JobData</code> and <code>Job.JobDataXml</code> information loading</param>
        /// <param name="application">Name of application which is performing action</param>
        /// <returns>Collection of requested Jobs</returns>
        public JobCollection GetJobsByType(JobType jobType, Boolean skipJobDataLoading, MDMCenterApplication application)
        {
            JobCollection jobCollection = null;

            try
            {
                JobBL jobManager = new JobBL();
                jobCollection = jobManager.GetJobsByType(jobType, skipJobDataLoading, application);
            }
            catch (Exception ex)
            {
                this.LogException(ex);
                throw base.WrapException(ex);
            }

            return jobCollection;
        }

        public JobCollection GetJobs(JobFilterContext jobFilterContext, CallerContext callerContext)
        {
            return MakeBusinessLogicCall<JobBL, JobCollection>("GetJobs",
                                         businessLogic =>
                                             businessLogic.GetJobs(jobFilterContext, callerContext));
        }

        public OperationResultCollection ProcessJobs(JobCollection jobsCollection, CallerContext callerContext)
        {
            return MakeBusinessLogicCall<JobBL, OperationResultCollection>("Process",
                businessLogic => businessLogic.Process(jobsCollection, callerContext));
        }

        public OperationResult CreateJob(Job job, CallerContext callerContext)
        {
            return MakeBusinessLogicCall<JobBL, OperationResult>("Create",
                             businessLogic =>
                                 businessLogic.Create(job, callerContext));
        }

        public OperationResult UpdateJob(Job job, CallerContext callerContext)
        {
            return MakeBusinessLogicCall<JobBL, OperationResult>("Update",
                             businessLogic =>
                                 businessLogic.Update(job, callerContext));
        }

        public OperationResult DeleteJob(Job job, CallerContext callerContext)
        {
            return MakeBusinessLogicCall<JobBL, OperationResult>("Delete",
                             businessLogic =>
                                 businessLogic.Delete(job, callerContext));
        }

        /// <summary>
        /// Creates import job and notifies to the appropriate service that a change has occurred
        /// </summary>
        /// <param name="job">Job which needs to be created</param>
        /// <param name="application">Name of application which is performing action</param>
        /// <param name="module">Name of module which is performing action</param>
        /// <returns>Id of the created import job</returns>
        public Int32 InitiateImportJob(Job job, MDMCenterApplication application, MDMCenterModules module)
        {
            try
            {
                JobBL jobManager = new JobBL();
                jobManager.Create(job, new CallerContext(application, Utility.GetModule(job.JobType), module.ToString()));
            }
            catch (Exception ex)
            {
                this.LogException(ex);
                throw base.WrapException(ex);
            }

            //Why we are returning job id? why not operation result....
            return job == null ? 0 : job.Id;
        }

        /// <summary>
        /// Processes import job
        /// </summary>
        /// <param name="job">Job which needs to be processed</param>
        /// <param name="application">Name of application which is performing action</param>
        /// <param name="module">Name of module which is performing action</param>
        /// <returns>Process result</returns>
        public Boolean UpdateImportJob(Job job, MDMCenterApplication application, MDMCenterModules module)
        {
            Boolean result = false;

            try
            {
                JobBL jobManager = new JobBL();
                OperationResult or = jobManager.Update(job, new CallerContext(application, Utility.GetModule(job.JobType), module.ToString()));
                if (or.OperationResultStatus == OperationResultStatusEnum.None || or.OperationResultStatus == OperationResultStatusEnum.Successful)
                {
                    result = true;
                }
            }
            catch (Exception ex)
            {
                this.LogException(ex);
                throw base.WrapException(ex);
            }

            return result;
        }

        /// <summary>
        /// Gets results of requested import job
        /// </summary>
        /// <param name="importJobId">Id of the job for which results are required</param>
        /// <param name="application">Name of application which is performing action</param>
        /// <param name="module">Name of module which is performing action</param>
        /// <returns>Collection of job import results</returns>
        public JobImportResultCollection GetImportJobResults(Int32 importJobId, MDMCenterApplication application, MDMCenterModules module)
        {
            JobImportResultCollection jobImportResultCollection = null;

            try
            {
                JobImportResultBL jobImportResultManager = new JobImportResultBL();
                jobImportResultCollection = jobImportResultManager.Get(importJobId, application, module);
            }
            catch (Exception ex)
            {
                this.LogException(ex);
                throw base.WrapException(ex);
            }

            return jobImportResultCollection;
        }

        // TODO PRASAD
        /// <summary>
        /// Gets results of requested import job
        /// </summary>
        /// <param name="importJobId">Id of the job for which results are required</param>
        /// <param name="application">Name of application which is performing action</param>
        /// <param name="module">Name of module which is performing action</param>
        /// <returns>Collection of job import results</returns>
        public DataModelOperationResultCollection GetDataModelImportJobResults(Int32 importJobId, ObjectType objectType, String externalId, CallerContext callerContext)
        {
            DataModelOperationResultCollection jobImportResultCollection = new DataModelOperationResultCollection();

            try
            {
                JobImportResultBL jobImportResultManager = new JobImportResultBL();
                jobImportResultCollection = jobImportResultManager.GetDataModelOperationResultCollection(importJobId, objectType, externalId, callerContext);
            }
            catch (Exception ex)
            {
                this.LogException(ex);
                throw base.WrapException(ex);
            }

            return jobImportResultCollection;
        }

        /// <summary>
        /// Get all available import profiles
        /// </summary>
        /// <param name="application">Name of application which is performing action</param>
        /// <returns>Collection of profiles</returns>
        public ImportProfileCollection GetProfiles(MDMCenterApplication application)
        {
            ImportProfileCollection profileCollection;

            try
            {
                ImportProfileBL bl = new ImportProfileBL();
                profileCollection = bl.GetAll(new CallerContext(MDMCenterApplication.VendorPortal, MDMCenterModules.Unknown));
            }
            catch (Exception ex)
            {
                this.LogException(ex);
                throw base.WrapException(ex);
            }

            return profileCollection;
        }

        /// <summary>
        /// Create the Job for requested profile Id and return the Job id.
        /// And place the file into data base.
        /// </summary>
        /// <param name="application">Name of application which is performing action</param>
        /// <param name="file">File which need place to DB</param>
        /// <param name="profileName">Using profile</param>
        /// <param name="profileId">Using profile ID</param>
        /// <param name="parentJobId">Indicates the Parent Job Id</param>
        /// <param name="jobType">Indicates the Job type. By default job type is Entity Import</param>
        /// <returns>Job ID</returns>
        public Int32 StartImportJob(MDMCenterApplication application, File file, String profileName, Int32 profileId, Int32 parentJobId, JobType jobType = JobType.EntityImport)
        {
            CallerContext callerContext = new CallerContext(application, MDMCenterModules.UIProcess);
            return StartImportJob(file, profileName, profileId, parentJobId, jobType, callerContext);
        }

        /// <summary>
        /// Create the Job for requested profile Id and return the Job id.
        /// And place the file into data base.
        /// </summary>
        /// <param name="file">File which need place to DB</param>
        /// <param name="profileName">Using profile</param>
        /// <param name="profileId">Using profile ID</param>
        /// <param name="parentJobId">Indicates the Parent Job Id</param>
        /// <param name="jobType">Indicates the Job type. By default job type is Entity Import</param>
        /// <param name="callerContext">Caller context</param>
        /// <returns>Job ID</returns>
        public Int32 StartImportJob(File file, String profileName, Int32 profileId, Int32 parentJobId, JobType jobType, CallerContext callerContext)
        {
            return MakeBusinessLogicCall<JobBL, Int32>(
                bl => bl.CreateJobInstance(file, profileId, profileName, parentJobId, callerContext, jobType),
                context =>
                {
                    context.CallerContext.ProfileId = profileId;
                    context.CallerContext.ProfileName = profileName;
                    if (parentJobId > 0)
                    {
                        context.CallerContext.JobId = parentJobId;
                    }
                });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="jobId"></param>
        /// <param name="application"></param>
        /// <returns></returns>
        public JobImportResultCollection GetErrorStates(Int32 jobId, MDMCenterApplication application)
        {
            return MakeBusinessLogicCall<JobImportResultBL, JobImportResultCollection>(
                bl => bl.Get(jobId, application, MDMCenterModules.Import),
                context =>
                {
                    context.CallerContext.JobId = jobId;
                });
        }

        /// <summary>
        /// Gets Integration job errors collection in form of Excel file
        /// </summary>
        /// <param name="jobId">Indicates the job Id</param>
        /// <param name="locale">Indicates the locale</param>
        /// <param name="callerContext">Indicates the caller context of application</param>
        /// <returns></returns>
        public File GetErrorStatesAsExcelFile(Int32 jobId, LocaleEnum locale, CallerContext callerContext)
        {
            File result = null;
            try
            {
                JobImportResultBL resultManager = new JobImportResultBL();
                result = resultManager.GetAsExcelFile(jobId, locale, callerContext);
            }
            catch (Exception ex)
            {
                this.LogException(ex);
                throw base.WrapException(ex);
            }
            return result;
        }

        /// <summary>
        /// Get all Data Model objects' OperationResult Summary Collection for given jobid
        /// </summary>
        /// <param name="jobId">Job Id</param>
        /// <param name="callerContext">Caller Context</param>
        /// <returns></returns>
        public DataModelOperationResultSummaryCollection GetDataModelOperationResultSummary(int jobId, CallerContext callerContext)
        {
            DataModelOperationResultSummaryCollection results = new DataModelOperationResultSummaryCollection();

            try
            {
                JobImportResultBL resultManager = new JobImportResultBL();
                results = resultManager.GetDataModelOperationResultSummaryCollection(jobId, ObjectType.None, "DataModelImportSummary", callerContext);
            }
            catch (Exception ex)
            {
                this.LogException(ex);
                throw base.WrapException(ex);
            }
            return results;
        }

        /// <summary>
        /// Gets execution status of requested job
        /// </summary>
        /// <param name="jobId">Id of the job which execution status are required</param>
        /// <param name="application">Name of application which is performing action</param>
        /// <param name="module">Name of module which is performing action</param>
        /// <returns>Execution status of requested job</returns>
        public JobOperationStatus GetJobExecutionStatus(Int32 jobId, MDMCenterApplication application, MDMCenterModules module)
        {
            Double progressValue = -1.0;
            String status = "Preparing the file for processing";

            JobBL jobManager = new JobBL();
            Job job = jobManager.Get(jobId, application);

            if (job.JobStatus == JobStatus.Aborted || job.JobStatus == JobStatus.Completed || job.JobStatus == JobStatus.CompletedWithErrors || job.JobStatus == JobStatus.CompletedWithWarnings || job.JobStatus == JobStatus.CompletedWithWarningsAndErrors || job.JobStatus == JobStatus.Deleted)
            {
                progressValue = 100.0;
                status = "Done";
            }
            else
            {
                Double totalElementsToProcess = job.JobData.ExecutionStatus.TotalElementsToProcess;
                if (totalElementsToProcess > 0)
                {
                    Double totalElementsProcessed = job.JobData.ExecutionStatus.TotalElementsProcessed;
                    if (totalElementsToProcess > totalElementsProcessed)
                    {
                        progressValue = totalElementsProcessed / totalElementsToProcess * 100.0;
                        status = String.Format("Processing file... {0} of {1} items completed", totalElementsProcessed, totalElementsToProcess);
                    }
                    else
                    {
                        progressValue = 100.0;
                        status = "Done";
                    }
                }
            }

            var result = new JobOperationStatus
            {
                Status = status,
                ProgressValue = (Int32)progressValue
            };

            return result;
        }        

        #region Import profile CUD

        public OperationResult CreateImportProfile(ImportProfile importProfile, CallerContext callerContext)
        {
            return MakeBusinessLogicCall<ImportProfileBL, OperationResult>("CreateImportProfile", businessLogic => businessLogic.Create(importProfile, callerContext));
        }

        public OperationResult DeleteImportProfile(ImportProfile importProfile, CallerContext callerContext)
        {
            return MakeBusinessLogicCall<ImportProfileBL, OperationResult>("DeleteImportProfile", businessLogic => businessLogic.Delete(importProfile, callerContext));
        }

        public OperationResult UpdateImportProfile(ImportProfile importProfile, CallerContext callerContext)
        {
            return MakeBusinessLogicCall<ImportProfileBL, OperationResult>("UpdateImportProfile", businessLogic => businessLogic.Update(importProfile, callerContext));
        }

        public OperationResultCollection ProcessImportProfiles(ImportProfileCollection importProfiles, CallerContext callerContext)
        {
            return MakeBusinessLogicCall<ImportProfileBL, OperationResultCollection>("ProcessImportProfiles", businessLogic => businessLogic.Process(importProfiles, callerContext));
        }

        #endregion

        #region Import profile Get

        public ImportProfile GetImportProfile(Int32 profileId, CallerContext callerContext)
        {
            return MakeBusinessLogicCall<ImportProfileBL, ImportProfile>(
                businessLogic => businessLogic.Get(profileId, callerContext),
                context =>
                {
                    context.CallerContext.ProfileId = profileId;
                });
        }

        public ImportProfile GetImportProfile(String profileName, CallerContext callerContext)
        {
            return MakeBusinessLogicCall<ImportProfileBL, ImportProfile>("GetImportProfile",
                businessLogic => businessLogic.Get(profileName, callerContext));
        }

        public ImportProfileCollection GetAllImportProfiles(CallerContext callerContext)
        {
            return MakeBusinessLogicCall<ImportProfileBL, ImportProfileCollection>("GetAllImportProfiles",
                businessLogic => businessLogic.GetAll(callerContext));
        }

        public String GetProfileType(Int32 profileId, CallerContext callerContext)
        {
            return MakeBusinessLogicCall<ImportProfileBL, String>("GetProfileType",
                businessLogic => businessLogic.GetProfileType(profileId, callerContext));
        }

        #endregion

        #region Lookup Import profile Get

        /// <summary>
        /// Gets the lookup import profile by id
        /// </summary>
        /// <param name="profileId">Indicates the profile Id</param>
        /// <param name="callerContext">Indicates the caller context.</param>
        /// <returns>Return the Lookup Import profile object</returns>
        public LookupImportProfile GetLookupImportProfileById(Int32 profileId, CallerContext callerContext)
        {
            return MakeBusinessLogicCall<LookupImportProfileBL, LookupImportProfile>("GetProfileById",
                businessLogic => businessLogic.GetProfileById(profileId, callerContext));
        }

        /// <summary>
        /// Gets the specified lookup profile by name.
        /// </summary>
        /// <param name="profileName">Indicate name of the profile.</param>
        /// <param name="callerContext">Indicates the caller context.</param>
        /// <returns>Return the Lookup Import profile</returns>
        public LookupImportProfile GetLookupImportProfileByName(String profileName, CallerContext callerContext)
        {
            return MakeBusinessLogicCall<LookupImportProfileBL, LookupImportProfile>("GetProfileByName",
                businessLogic => businessLogic.GetProfileByName(profileName, callerContext));
        }

        /// <summary>
        /// Gets all Lookup import profiles.
        /// </summary>
        /// <param name="callerContext">Indicates the caller context.</param>
        /// <returns>Lookup Import profile collection</returns>
        public LookupImportProfileCollection GetAllLookupImportProfiles(CallerContext callerContext)
        {
            return MakeBusinessLogicCall<LookupImportProfileBL, LookupImportProfileCollection>("GetAllLookupImportProfiles",
                businessLogic => businessLogic.GetAllLookupImportProfiles(callerContext));
        }

        #endregion

        #region JobSchedule CUD

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
        public OperationResult CreateJobSchedule(JobSchedule jobSchedule, CallerContext callerContext)
        {
            return MakeBusinessLogicCall<JobScheduleBL, OperationResult>("CreateJobSchedule", businessLogic => businessLogic.Create(jobSchedule, callerContext));
        }

        /// <summary>
        /// Update JobSchedule
        /// </summary>
        /// <param name="jobSchedule">Represent JobSchedule Object</param>
        /// <param name="callerContext">Context of application making call for this API</param>
        /// <param name="programName">Name of program making change</param>
        /// <returns>OperationResult of JobSchedule Updating</returns>
        /// <exception cref="ArgumentNullException">If JobSchedule Object is Null</exception>
        /// <exception cref="ArgumentNullException">If JobSchedule ShortName is Null or having empty String</exception>
        /// <exception cref="ArgumentNullException">If JobSchedule LongName is Null or having empty String</exception>
        public OperationResult UpdateJobSchedule(JobSchedule jobSchedule, CallerContext callerContext)
        {
            return MakeBusinessLogicCall<JobScheduleBL, OperationResult>("UpdateJobSchedule", businessLogic => businessLogic.Update(jobSchedule, callerContext));
        }

        /// <summary>
        /// Delete JobSchedule
        /// </summary>
        /// <param name="jobSchedule">Represent JobSchedule Object</param>
        /// <param name="callerContext">Context of application making call for this API</param>
        /// <param name="programName">Name of program making change</param>
        /// <returns>OperationResult of JobSchedule Updating</returns>
        /// <exception cref="ArgumentNullException">If JobSchedule Object is Null</exception>
        public OperationResult DeleteJobSchedule(JobSchedule jobSchedule, CallerContext callerContext)
        {
            return MakeBusinessLogicCall<JobScheduleBL, OperationResult>("DeleteJobSchedule", businessLogic => businessLogic.Delete(jobSchedule, callerContext));
        }

        /// <summary>
        /// Process JobSchedules
        /// </summary>
        /// <param name="jobSchedules">Represent JobSchedule Object collection to process</param>
        /// <param name="callerContext">Context of application making call for this API</param>
        /// <param name="programName">Name of program making change</param>
        /// <returns>OperationResult of JobSchedules process</returns>
        /// <exception cref="ArgumentNullException">If JobSchedules Object is Null</exception>
        public OperationResultCollection ProcessJobSchedules(JobScheduleCollection jobSchedules, CallerContext callerContext)
        {
            return MakeBusinessLogicCall<JobScheduleBL, OperationResultCollection>("ProcessJobSchedules", businessLogic => businessLogic.Process(jobSchedules, callerContext));
        }

        #endregion JobSchedule CUD

        #region JobSchedule Get

        /// <summary>
        /// Get all jobSchedules
        /// </summary>
        /// <param name="callerContext">Context of application making call for this API</param>
        /// <returns>All Relationship types</returns>
        public JobScheduleCollection GetAllJobSchedules(CallerContext callerContext)
        {
            return MakeBusinessLogicCall<JobScheduleBL, JobScheduleCollection>("GetAllJobSchedules", businessLogic => businessLogic.GetAll(callerContext));
        }

        /// <summary>
        /// Get jobSchedule for given Id
        /// </summary>
        /// <param name="jobScheduleId">Job schedule id for which detail is to be fetched.</param>
        /// <param name="callerContext">Context of application making call for this API</param>
        /// <returns>Job schedule for given id</returns>
        public JobSchedule GetJobScheduleById(Int32 jobScheduleId, CallerContext callerContext)
        {
            return MakeBusinessLogicCall<JobScheduleBL, JobSchedule>("GetJobScheduleById", businessLogic => businessLogic.GetById(jobScheduleId, callerContext));
        }

        #endregion JobSchedule Get

        #region IntegrationActivityLog CUD Methods

        /// <summary>
        /// Create IntegrationActivityLog with ShortNames as parameters instead of Id
        /// </summary>
        /// <param name="mdmObjectId">MDMObjectId which for which activity log is to be created.</param>
        /// <param name="mdmObjectTypeName">Name of MDMObject.</param>
        /// <param name="context">Context for passed MDMObjectId</param>
        /// <param name="connectorShortName">ConnectorShortName for which ActivityLog is to be created</param>
        /// <param name="integrationMessageTypeShortName">ShortName of MessageType for which ActivityLog is to be created</param>
        /// <param name="integrationType">Type of integration (inbound/outbound)</param>
        /// <param name="weightage">Weightage for activity</param>
        /// <param name="messsageCount">Count of messages generated from given activity</param>
        /// <param name="callerContext">Context of caller making call to this API.</param>
        /// <returns>Status indicating the result of operation</returns>
        public OperationResult CreateIntegrationActivityLog(Int64 mdmObjectId, String mdmObjectTypeName, String context, String connectorShortName, String integrationMessageTypeShortName,
            IntegrationType integrationType, Int32 weightage, Int32 messsageCount, CallerContext callerContext)
        {
            return this.MakeBusinessLogicCall<IntegrationActivityLogBL, OperationResult>("Create", blMethod => blMethod.Create(mdmObjectId, mdmObjectTypeName, context, connectorShortName, integrationMessageTypeShortName,
                    integrationType, weightage, messsageCount, callerContext), MDMTraceSource.Integration);
        }

        /// <summary>
        /// Create a integration activity log
        /// </summary>
        /// <param name="integrationActivityLog">Integration activity log to be created</param>
        /// <param name="callerContext">Indicates the context of caller making call to this API</param>
        /// <returns>Status indicating the result of operation</returns>
        public OperationResult CreateIntegrationActivityLog(IntegrationActivityLog integrationActivityLog, CallerContext callerContext)
        {
            return this.MakeBusinessLogicCall<IntegrationActivityLogBL, OperationResult>("Create", blMethod => blMethod.Create(integrationActivityLog, callerContext), MDMTraceSource.Integration);
        }

        #endregion  IntegrationActivityLog CUD Methods

        #region ConnectorProfile CUD

        /// <summary>
        /// Create a ConnectorProfile
        /// </summary>
        /// <param name="connectorProfile">ConnectorProfile to be created</param>
        /// <param name="callerContext">Indicates the context of caller making call to this API</param>
        /// <returns>Status indicating the result of operation</returns>
        public OperationResult CreateConnectorProfile(ConnectorProfile connectorProfile, CallerContext callerContext)
        {
            return this.MakeBusinessLogicCall<ConnectorProfileBL, OperationResult>("CreateConnectorProfile", blMethod => blMethod.Create(connectorProfile, callerContext), MDMTraceSource.Integration);
        }

        /// <summary>
        /// Update a ConnectorProfile
        /// </summary>
        /// <param name="connectorProfile">ConnectorProfile to be updated</param>
        /// <param name="callerContext">Indicates the context of caller making call to this API</param>
        /// <returns>Status indicating the result of operation</returns>
        public OperationResult UpdateConnectorProfile(ConnectorProfile connectorProfile, CallerContext callerContext)
        {
            return this.MakeBusinessLogicCall<ConnectorProfileBL, OperationResult>("UpdateConnectorProfile", blMethod => blMethod.Update(connectorProfile, callerContext), MDMTraceSource.Integration);
        }

        /// <summary>
        /// Delete a ConnectorProfile
        /// </summary>
        /// <param name="connectorProfile">ConnectorProfile to be deleted</param>
        /// <param name="callerContext">Indicates the context of caller making call to this API</param>
        /// <returns>Status indicating the result of operation</returns>
        public OperationResult DeleteConnectorProfile(ConnectorProfile connectorProfile, CallerContext callerContext)
        {
            return this.MakeBusinessLogicCall<ConnectorProfileBL, OperationResult>("DeleteConnectorProfile", blMethod => blMethod.Delete(connectorProfile, callerContext), MDMTraceSource.Integration);
        }

        #endregion ConnectorProfile CUD

        #region Get ConnectorProfile

        /// <summary>
        /// Get all ConnectorProfile in the system
        /// </summary>
        /// <param name="callerContext">Indicates the context of caller making call to this API</param>
        /// <returns>ConnectorProfile object collection</returns>
        public ConnectorProfileCollection GetAllConnectorProfiles(CallerContext callerContext)
        {
            return this.MakeBusinessLogicCall<ConnectorProfileBL, ConnectorProfileCollection>("GetAllConnectorProfile", blMethod => blMethod.GetAll(callerContext), MDMTraceSource.Integration);
        }

        /// <summary>
        /// Get ConnectorProfile by Id
        /// </summary>
        /// <param name="connectorProfileId">Id of connector for which info needs to be fetched</param>
        /// <param name="callerContext">Indicates the context of caller making call to this API</param>
        /// <returns>ConnectorProfile object collection</returns>
        public ConnectorProfile GetConnectorProfileById(Int16 connectorProfileId, CallerContext callerContext)
        {
            return this.MakeBusinessLogicCall<ConnectorProfileBL, ConnectorProfile>("GetConnectorProfileById", blMethod => blMethod.GetById(connectorProfileId, callerContext), MDMTraceSource.Integration);
        }

        /// <summary>
        /// Get ConnectorProfile by ShortName
        /// </summary>
        /// <param name="connectorProfileShortName">ShortName of connector for which info needs to be fetched</param>
        /// <param name="callerContext">Indicates the context of caller making call to this API</param>
        /// <returns>ConnectorProfile object collection</returns>
        public ConnectorProfile GetConnectorProfileByName(String connectorProfileShortName, CallerContext callerContext)
        {
            return this.MakeBusinessLogicCall<ConnectorProfileBL, ConnectorProfile>("GetConnectorProfileByName", blMethod => blMethod.GetByName(connectorProfileShortName, callerContext), MDMTraceSource.Integration);
        }

        #endregion Get ConnectorProfile

        #region IntegrationMessage Get methods

        /// <summary>
        /// Get integration message by id
        /// </summary>
        /// <param name="integrationMessageId">integration message id to fetch the value for</param>
        /// <param name="callerContext">Indicates the context of caller making call to this API</param>
        /// <returns>integration message object</returns>
        public IntegrationMessage GetIntegrationMessageById(Int64 integrationMessageId, CallerContext callerContext)
        {
            return this.MakeBusinessLogicCall<IntegrationMessageBL, IntegrationMessage>("GetById", blMethod => blMethod.GetById(integrationMessageId, callerContext), MDMTraceSource.Integration);
        }

        #endregion IntegrationMessage Get methods

        #region Get IntegrationMessageType

        /// <summary>
        /// Get all IntegrationMessageType in the system
        /// </summary>
        /// <param name="callerContext">Indicates the context of caller making call to this API</param>
        /// <returns>IntegrationMessageType object collection</returns>
        public IntegrationMessageTypeCollection GetAllIntegrationMessageTypes(CallerContext callerContext)
        {
            return this.MakeBusinessLogicCall<IntegrationMessageTypeBL, IntegrationMessageTypeCollection>("GetAllIntegrationMessageType", blMethod => blMethod.GetAll(callerContext), MDMTraceSource.Integration);
        }

        /// <summary>
        /// Get IntegrationMessageType by Id
        /// </summary>
        /// <param name="integrationMessageTypeId">Id of IntegrationMessageType for which info needs to be fetched</param>
        /// <param name="callerContext">Indicates the context of caller making call to this API</param>
        /// <returns>IntegrationMessageType object collection</returns>
        public IntegrationMessageType GetIntegrationMessageTypeById(Int16 integrationMessageTypeId, CallerContext callerContext)
        {
            return this.MakeBusinessLogicCall<IntegrationMessageTypeBL, IntegrationMessageType>("GetIntegrationMessageTypeById", blMethod => blMethod.GetById(integrationMessageTypeId, callerContext), MDMTraceSource.Integration);
        }

        /// <summary>
        /// Get IntegrationMessageType by ShortName
        /// </summary>
        /// <param name="integrationMessageTypeShortName">ShortName of IntegrationMessageType for which info needs to be fetched</param>
        /// <param name="callerContext">Indicates the context of caller making call to this API</param>
        /// <returns>IntegrationMessageType object collection</returns>
        public IntegrationMessageType GetIntegrationMessageTypeByName(String integrationMessageTypeShortName, CallerContext callerContext)
        {
            return this.MakeBusinessLogicCall<IntegrationMessageTypeBL, IntegrationMessageType>("GetIntegrationMessageTypeByName", blMethod => blMethod.GetByName(integrationMessageTypeShortName, callerContext), MDMTraceSource.Integration);
        }

        #endregion Get IntegrationMessageType

        #region Get MDMObjectType

        /// <summary>
        /// Get all MDMObjectType in the system
        /// </summary>
        /// <param name="callerContext">Indicates the context of caller making call to this API</param>
        /// <returns>MDMObjectType object collection</returns>
        public MDMObjectTypeCollection GetAllMDMObjectTypes(CallerContext callerContext)
        {
            return this.MakeBusinessLogicCall<MDMObjectTypeBL, MDMObjectTypeCollection>("GetAllMDMObjectType", blMethod => blMethod.GetAll(callerContext), MDMTraceSource.Integration);
        }

        /// <summary>
        /// Get MDMObjectType by Id
        /// </summary>
        /// <param name="mdmObjectTypeId">Id of MDMObjectType for which info needs to be fetched</param>
        /// <param name="callerContext">Indicates the context of caller making call to this API</param>
        /// <returns>MDMObjectType object collection</returns>
        public MDMObjectType GetMDMObjectTypeById(Int16 mdmObjectTypeId, CallerContext callerContext)
        {
            return this.MakeBusinessLogicCall<MDMObjectTypeBL, MDMObjectType>("GetMDMObjectTypeById", blMethod => blMethod.GetById(mdmObjectTypeId, callerContext), MDMTraceSource.Integration);
        }

        /// <summary>
        /// Get MDMObjectType by ShortName
        /// </summary>
        /// <param name="mdmObjectTypeShortName">ShortName of MDMObjectType for which info needs to be fetched</param>
        /// <param name="callerContext">Indicates the context of caller making call to this API</param>
        /// <returns>MDMObjectType object collection</returns>
        public MDMObjectType GetMDMObjectTypeByName(String mdmObjectTypeShortName, CallerContext callerContext)
        {
            return this.MakeBusinessLogicCall<MDMObjectTypeBL, MDMObjectType>("GetMDMObjectTypeByName", blMethod => blMethod.GetByName(mdmObjectTypeShortName, callerContext), MDMTraceSource.Integration);
        }

        #endregion Get MDMObjectType

        #region Update IntegrationItem Status

        /// <summary>
        /// Update integration item status. Update item and dimension status for given Id/Type
        /// </summary>
        /// <param name="integrationItemStatus">Status for an item. It contains item information and status information</param>
        /// <param name="callerContext">Context of API making call to this API.</param>
        /// <returns>Result of operation.</returns>
        public OperationResult UpdateIntegrationItemStatus(IntegrationItemStatus integrationItemStatus, CallerContext callerContext)
        {
            return this.MakeBusinessLogicCall<IntegrationItemStatusBL, OperationResult>("UpdateIntegrationItemStatus", blMethod => blMethod.UpdateStatus(integrationItemStatus, callerContext), MDMTraceSource.Integration);
        }

        #endregion Update IntegrationItem Status

        #region Search IntegrationItem Status

        /// <summary>
        /// Search for Integration item status based on given criteria
        /// </summary>
        /// <param name="integrationItemStatusSearchCriteria">Contains search criteria for IntegrationItemStatus</param>
        /// <param name="callerContext">Indicates context of caller making call to this API</param>
        /// <returns><see cref="IntegrationItemStatusInternalCollection"/> which are matching given search criteria</returns>
        public IntegrationItemStatusInternalCollection SearchIntegrationItemStatus(IntegrationItemStatusSearchCriteria integrationItemStatusSearchCriteria, CallerContext callerContext)
        {
            return this.MakeBusinessLogicCall<IntegrationItemStatusBL, IntegrationItemStatusInternalCollection>("SearchIntegrationItemStatus", blMethod => blMethod.SearchIntegrationItemStatus(integrationItemStatusSearchCriteria, callerContext), MDMTraceSource.Integration);
        }

        #endregion Search IntegrationItem Status

        #region Search IntegrationItemDimension Type

        public IntegrationItemDimensionTypeCollection GetAllIntegrationItemDimensionTypes(CallerContext callerContext)
        {
            return this.MakeBusinessLogicCall<IntegrationItemDimensionTypeBL, IntegrationItemDimensionTypeCollection>("GetAllIntegrationItemDimensionTypes", blMethod => blMethod.GetAll(callerContext), MDMTraceSource.Integration);
        }

        public IntegrationItemDimensionType GetIntegrationItemDimensionTypeById(short integrationItemDimensionTypeId,
            CallerContext callerContext)
        {
            return this.MakeBusinessLogicCall<IntegrationItemDimensionTypeBL, IntegrationItemDimensionType>("GetIntegrationItemDimensionTypeById", blMethod => blMethod.GetById(integrationItemDimensionTypeId, callerContext), MDMTraceSource.Integration);
        }

        public IntegrationItemDimensionType GetIntegrationItemDimensionTypeByName(string integrationItemDimensionTypeShortName,
            CallerContext callerContext)
        {
            return this.MakeBusinessLogicCall<IntegrationItemDimensionTypeBL, IntegrationItemDimensionType>("GetIntegrationItemDimensionTypeByName", blMethod => blMethod.GetByName(integrationItemDimensionTypeShortName, callerContext), MDMTraceSource.Integration);
        }

        public IntegrationItemDimensionTypeCollection GetIntegrationItemDimensionTypesByConnectorId(Int16 connectorId, CallerContext callerContext)
        {
            return this.MakeBusinessLogicCall<IntegrationItemDimensionTypeBL, IntegrationItemDimensionTypeCollection>("GetIntegrationItemDimensionTypesByConnectorId", blMethod => blMethod.GetByConnectorId(connectorId, callerContext), MDMTraceSource.Integration);
        }
        #endregion

        #region Private Methods

        /// <summary>
        /// Makes calls of Integration Business logic.
        /// Also emits traces when necessary
        /// </summary>
        /// <typeparam name="TResult">The type of the result of method of business logic</typeparam>
        /// <typeparam name="TBusinessLogic">Type of business logic</typeparam>
        /// <param name="methodName">Name of the method for tracing</param>
        /// <param name="call">The call delegate to be executed against business logic instance</param>
        /// <returns>The value returned by business logic or default</returns>
        private TResult MakeBusinessLogicCall<TBusinessLogic, TResult>(string methodName, Func<TBusinessLogic, TResult> call, MDMTraceSource traceSource = MDMTraceSource.General) where TBusinessLogic : BusinessLogicBase, new()
        {
            Boolean isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;

            if (isTracingEnabled)
            {
                MDMTraceHelper.StartTraceActivity("IntegrationService." + methodName, traceSource, false);
            }

            TResult operationResult;

            try
            {
                if (isTracingEnabled)
                {
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "IntegrationService receives" + methodName + " request message.", traceSource);
                }

                operationResult = call(new TBusinessLogic());

                if (isTracingEnabled)
                {
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "IntegrationService receives" + methodName + " response message.", traceSource);
                }
            }
            catch (Exception ex)
            {
                this.LogException(ex);
                throw base.WrapException(ex);
            }

            if (isTracingEnabled)
            {
                MDMTraceHelper.StopTraceActivity("IntegrationService." + methodName, traceSource);
            }

            return operationResult;
        }

        #endregion #region Private Methods
    }
}
