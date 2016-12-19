using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Security.Principal;
using System.ServiceModel;

namespace MDM.Services
{
    using MDM.BusinessObjects;
    using MDM.BusinessObjects.DataModel;
    using MDM.BusinessObjects.Exports;
    using MDM.BusinessObjects.Imports;
    using MDM.BusinessObjects.Integration;
    using MDM.BusinessObjects.Interfaces;
    using MDM.BusinessObjects.Jobs;
    using MDM.Core;
    using MDM.Core.Exceptions;
    using MDM.Interfaces;
    using MDM.Interfaces.Exports;
    using MDM.Services.ServiceProxies;
    using MDM.Utility;
    using MDM.WCFServiceInterfaces;
    using MDM.BusinessObjects.Interfaces.Exports;

    ///<summary>
    /// Integration Service facilitates to work with MDMCenter integration system. 
    /// This includes creating and initializing MDMCenter data load jobs, accessing job results, and accessing job profiles.
    /// </summary>
    public class IntegrationService : ServiceClientBase
    {
        #region Fields

        #endregion

        #region Constructors

        /// <summary>
        /// Use this default constructor only when calling MDM service within the execution context of an MDM Application. 
        /// Example: Service call from the MDM business rule.
        /// This would try to use current execution security context for service authentication purpose. 
        /// </summary>
        public IntegrationService()
            : base(typeof(IntegrationService))
        {
        }

        /// <summary>
        /// Use this default constructor with binding configuration name only when calling MDM service within the execution context of an MDM Application. 
        /// Example: Service call from the MDM business rule.
        /// This would try to use current execution security context for service authentication purpose. 
        /// </summary>
        /// <param name="endPointConfigurationName">Indicates the EndPointConfigurationName</param>
        public IntegrationService(String endPointConfigurationName)
            : base(typeof(IntegrationService), endPointConfigurationName)
        {
        }

        /// <summary>
        /// Use this constructor for form based authentication passing username and password
        /// </summary>
        /// <param name="endPointConfigurationName">Indicates the EndPointConfigurationName</param>
        /// <param name="userName">Name of the Login User</param>
        /// <param name="password">Password of the User</param>
        public IntegrationService(String endPointConfigurationName, String userName, String password)
            : base(endPointConfigurationName, userName, password)
        {

        }

        /// <summary>
        /// Use this constructor for window authentication passing current windows identity.
        /// </summary>
        /// <param name="endPointConfigurationName">Indicates the EndPointConfigurationName</param>
        /// <param name="userIdentity">Indicates User's Identity</param>
        public IntegrationService(String endPointConfigurationName, IIdentity userIdentity)
            : base(endPointConfigurationName, userIdentity)
        {
        }

        /// <summary>
        /// Constructor with client configuration.
        /// </summary>
        /// <param name="wcfClientConfiguration">Indicates WCF Client Configuration</param>
        public IntegrationService(IWCFClientConfiguration wcfClientConfiguration)
            : base(wcfClientConfiguration)
        {
        }

        /// <summary>
        /// Constructor with endPointConfigurationName, authenticationType, userIdentity, userName and password
        /// </summary>
        /// <param name="endPointConfigurationName">Indicates the EndPointConfigurationName</param>
        /// <param name="authenticationType">Indicates Type of Authentication i.e Windows/Forms</param>
        /// <param name="userIdentity">Indicates Identity of the Login User</param>
        /// <param name="userName">Name of the Login User</param>
        /// <param name="password">Password of the User</param>
        public IntegrationService(String endPointConfigurationName, AuthenticationType authenticationType, IIdentity userIdentity, String userName, String password)
            : base(endPointConfigurationName, authenticationType, userIdentity, userName, password)
        {
        }

        /// <summary>
        /// Constructor with endPointConfigurationName, endPointAddress, authenticationType, userIdentity, userName and password
        /// </summary>
        /// <param name="endPointConfigurationName">Indicates the EndPointConfigurationName</param>
        /// <param name="endPointAddress"> Provides a unique network address that a client uses to communicate with a service endpoint.</param>
        /// <param name="authenticationType">Indicates Type of Authentication i.e Windows/Forms</param>
        /// <param name="userIdentity">Indicates Identity of the Login User</param>
        /// <param name="userName">Name of the Login User</param>
        /// <param name="password">Password of the User</param>
        public IntegrationService(String endPointConfigurationName, EndpointAddress endPointAddress, AuthenticationType authenticationType, IIdentity userIdentity, String userName, String password)
            : base(endPointConfigurationName, endPointAddress, authenticationType, userIdentity, userName, password)
        {
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets the requested job
        /// </summary>
        /// <param name="jobId">Id of the Job</param>
        /// <param name="jobType">Type of the Job</param>
        /// <param name="application">Name of application which is performing action</param>
        /// <returns>The job object</returns>
        /// <exception cref="EndpointNotFoundException">Thrown when a remote endpoint could not be found or reached</exception>
        /// <exception cref="InvalidOperationException">Thrown when a method call is invalid</exception>
        /// <exception cref="TimeoutException">Thrown when the time allotted for an operation has expired</exception>
        /// <exception cref="MDMOperationException">Thrown when the MDM operation fails</exception>
        /// <exception cref="CommunicationException">Thrown when there is a communication error in either the service or client application</exception>
        public IJob GetJob(Int32 jobId, JobType jobType, MDMCenterApplication application)
        {
            return MakeServiceCall("GetJob",
                                   "GetJob",
                                   service =>
                                       {
                                           Boolean isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;
                                           if (isTracingEnabled)
                                           {
                                               MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("Requested Get Job for JobId: {0}, JobType: {1}", jobId, jobType));
                                           }
                                           return service.GetJob(jobId, jobType, application);
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
        /// <exception cref="EndpointNotFoundException">Thrown when a remote endpoint could not be found or reached</exception>
        /// <exception cref="InvalidOperationException">Thrown when a method call is invalid</exception>
        /// <exception cref="TimeoutException">Thrown when the time allotted for an operation has expired</exception>
        /// <exception cref="MDMOperationException">Thrown when the MDM operation fails</exception>
        /// <exception cref="CommunicationException">Thrown when there is a communication error in either the service or client application</exception>
        public IJobCollection GetJobs(JobType jobType, JobSubType jobSubType, JobStatus jobStatus, MDMCenterApplication application)
        {
            return GetJobs(jobType, jobSubType, jobStatus, false, application);
        }

        /// <summary>
        /// Gets jobs with the requested JobType and SQL filter
        /// </summary>
        /// <param name="jobType">Type of the Job</param>
        /// <param name="sql">This parameter is to filter search data based on sql</param>
        /// <param name="application">Name of application which is performing action</param>
        /// <returns>Collection of requested Jobs</returns>
        /// <exception cref="EndpointNotFoundException">Thrown when a remote endpoint could not be found or reached</exception>
        /// <exception cref="InvalidOperationException">Thrown when a method call is invalid</exception>
        /// <exception cref="TimeoutException">Thrown when the time allotted for an operation has expired</exception>
        /// <exception cref="MDMOperationException">Thrown when the MDM operation fails</exception>
        /// <exception cref="CommunicationException">Thrown when there is a communication error in either the service or client application</exception>
        public IJobCollection GetJobs(JobType jobType, String sql, MDMCenterApplication application)
        {
            return MakeServiceCall("GetJobs",
                                   "GetJobs",
                                   service =>
                                       {
                                           Boolean isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;
                                           if (isTracingEnabled)
                                           {
                                               MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("Requested Get Jobs for JobType: {0}, SQL filter: {1}", jobType, sql));
                                           }

                                           return service.GetJobs(jobType, sql, application);
                                       });
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
        /// <exception cref="EndpointNotFoundException">Thrown when a remote endpoint could not be found or reached</exception>
        /// <exception cref="InvalidOperationException">Thrown when a method call is invalid</exception>
        /// <exception cref="TimeoutException">Thrown when the time allotted for an operation has expired</exception>
        /// <exception cref="MDMOperationException">Thrown when the MDM operation fails</exception>
        /// <exception cref="CommunicationException">Thrown when there is a communication error in either the service or client application</exception>
        public IJobCollection GetJobs(JobType jobType, JobSubType jobSubType, JobStatus jobStatus, Boolean skipJobDataLoading, MDMCenterApplication application)
        {
            return MakeServiceCall("GetJobs",
                                   "GetJobs",
                                   service =>
                                       {
                                           Boolean isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;
                                           if (isTracingEnabled)
                                           {
                                               MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("Requested Get Jobs for JobType: {0}, JobSubType: {1}, JobStatus: {2}", jobType, jobSubType, jobStatus));
                                           }
                                           return service.GetJobs(jobType,
                                                                  jobSubType,
                                                                  jobStatus,
                                                                  skipJobDataLoading,
                                                                  application);
                                       });
        }

        /// <summary>
        /// Gets jobs with the requested JobType
        /// </summary>
        /// <param name="jobType">Type of the Job</param>
        /// <param name="application">Name of application which is performing action</param>
        /// <returns>Collection of requested Jobs</returns>
        /// <exception cref="EndpointNotFoundException">Thrown when a remote endpoint could not be found or reached</exception>
        /// <exception cref="InvalidOperationException">Thrown when a method call is invalid</exception>
        /// <exception cref="TimeoutException">Thrown when the time allotted for an operation has expired</exception>
        /// <exception cref="MDMOperationException">Thrown when the MDM operation fails</exception>
        /// <exception cref="CommunicationException">Thrown when there is a communication error in either the service or client application</exception>
        public IJobCollection GetJobsByType(JobType jobType, MDMCenterApplication application)
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
        /// <exception cref="EndpointNotFoundException">Thrown when a remote endpoint could not be found or reached</exception>
        /// <exception cref="InvalidOperationException">Thrown when a method call is invalid</exception>
        /// <exception cref="TimeoutException">Thrown when the time allotted for an operation has expired</exception>
        /// <exception cref="MDMOperationException">Thrown when the MDM operation fails</exception>
        /// <exception cref="CommunicationException">Thrown when there is a communication error in either the service or client application</exception>
        public IJobCollection GetJobsByType(JobType jobType, Boolean skipJobDataLoading, MDMCenterApplication application)
        {
            return MakeServiceCall("GetJobsByType",
                                   "GetJobsByType",
                                   service => service.GetJobsByType(jobType, skipJobDataLoading, application));
        }

        /// <summary>
        /// Gets jobs with the requested JobType
        /// </summary>
        /// <param name="jobType">Type of the Job</param>
        /// <param name="userName">Name of the User</param>
        /// <param name="application">Name of application which is performing action</param>
        /// <returns>Collection of requested Jobs</returns>
        /// <exception cref="EndpointNotFoundException">Thrown when a remote endpoint could not be found or reached</exception>
        /// <exception cref="InvalidOperationException">Thrown when a method call is invalid</exception>
        /// <exception cref="TimeoutException">Thrown when the time allotted for an operation has expired</exception>
        /// <exception cref="MDMOperationException">Thrown when the MDM operation fails</exception>
        /// <exception cref="CommunicationException">Thrown when there is a communication error in either the service or client application</exception>
        public IJobCollection GetJobsByType(JobType jobType, String userName, MDMCenterApplication application)
        {
            return MakeServiceCall("GetJobsByType",
                                   "GetJobsByType",
                                   service => service.GetJobsByType(jobType, application));
        }

        /// <summary>
        /// Get all jobs in system by caller context
        /// </summary>
        /// <param name="jobFilterContext"></param>
        /// <param name="iCallerContext">Caller context</param>
        /// <returns></returns>
        public IJobCollection GetJobs(IJobFilterContext jobFilterContext, ICallerContext iCallerContext)
        {
            return MakeServiceCall("GetJobs", "GetFilteredJobs", client => client.GetJobs(jobFilterContext as JobFilterContext, FillDiagnosticTraces(iCallerContext)));
        }

        /// <summary>
        /// Process all jobs in collection
        /// </summary>
        /// <param name="iJobCollection"></param>
        /// <param name="iCallerContext"></param>
        /// <returns></returns>
        public OperationResultCollection ProcessJobs(IJobCollection iJobCollection, ICallerContext iCallerContext)
        {
            return MakeServiceCall("ProcessJobs", "ProcessJobs",
                client => client.ProcessJobs(iJobCollection as JobCollection, FillDiagnosticTraces(iCallerContext)));
        }

        /// <summary>
        /// Create job
        /// </summary>
        /// <param name="iJob"></param>
        /// <param name="iCallerContext"></param>
        /// <returns></returns>
        public OperationResult CreateJob(IJob iJob, ICallerContext iCallerContext)
        {
            return MakeServiceCall("CreateJob", "CreateJob",
                client => client.CreateJob(iJob as Job, FillDiagnosticTraces(iCallerContext)));
        }

        /// <summary>
        /// Update job
        /// </summary>
        /// <param name="iJob"></param>
        /// <param name="iCallerContext"></param>
        /// <returns></returns>
        public OperationResult UpdateJob(IJob iJob, ICallerContext iCallerContext)
        {
            return MakeServiceCall("UpdateJob", "UpdateJob",
                client => client.UpdateJob(iJob as Job, FillDiagnosticTraces(iCallerContext)));
        }

        /// <summary>
        /// Delete job
        /// </summary>
        /// <param name="iJob"></param>
        /// <param name="iCallerContext"></param>
        /// <returns></returns>
        public OperationResult DeleteJob(IJob iJob, ICallerContext iCallerContext)
        {
            return MakeServiceCall("DeleteJob", "DeleteJob",
                client => client.DeleteJob(iJob as Job, FillDiagnosticTraces(iCallerContext)));
        }

        /// <summary>
        /// Creates import job and notifies to the appropriate service that a change has occurred
        /// </summary>
        /// <example> 
        /// <code> 
        ///private Int32 InitiateImportJob()
        ///{
        ///    // Id of job to be created (return value of the method)
        ///    Int32 jobId = 0;
        ///
        ///    // Id of file to be created
        ///    Int32 fileId = 0;
        ///
        ///    // Get MDM integration service
        ///    IntegrationService integrationService = GetMDMIntegrationServiceService();
        ///
        ///    // Assumption: Hard coded variables are considered in this sample
        ///    Int32 profileId = 2;
        ///    String profileName = "test profile name";
        ///
        ///    String filePath = Path.Combine(Environment.CurrentDirectory, "Data", "Profiles", "SampleImport.xlsx");
        ///
        ///    // Below enum defines caller MDM application. For release 7.0, use "PIM" all the time.
        ///    MDMCenterApplication callerApplication = MDMCenterApplication.PIM;
        ///
        ///    // Below enum defines caller MDM module. For release 7.0, use "Entity" all the time.
        ///    MDMCenterModules callerModule = MDMCenterModules.Entity;
        ///
        ///    // Make sure the input file exist
        ///    if (!System.IO.File.Exists(filePath))
        ///    {
        ///        Console.WriteLine("Input file : \"" + filePath + "\" doesn't exist");
        ///        return -1;
        ///    }
        ///
        ///    // Save the file into the database and get its id for reference
        ///    fileId = UploadFile(filePath);
        ///
        ///    // Create a new job and set its properties
        ///    Job job = new Job
        ///    {
        ///        Id = -1,
        ///        Name = String.Format("Import Job, Created At: {0}", DateTime.Now),
        ///        Description = "Job Created",
        ///        CreatedUser = "cfadmin",
        ///        CreatedDateTime = DateTime.Now.ToString(),
        ///        JobStatus = JobStatus.Queued,
        ///        JobType = JobType.EntityImport,
        ///        JobSubType = JobSubType.API
        ///    };
        ///
        ///    job.JobData.ExecutionStatus.CurrentStatusMessage = "Status of the job";
        ///    job.ProfileId = profileId;
        ///    job.JobData.ProfileId = profileId;
        ///    job.ProfileName = profileName;
        ///
        ///    // Enter the file id 
        ///    job.JobData.JobParameters.Add(new JobParameter("FileId", fileId.ToString()));
        ///    job.FileId = fileId;
        ///
        ///    // Make a WCF call to initiate the import job and return Id of newly created job as a result
        ///    jobId = integrationService.InitiateImportJob(job, callerApplication, callerModule);
        ///
        ///    // Return results of the calling method
        ///    return jobId;
        ///}
        /// </code>
        /// </example>
        /// <param name="iJob">Job which needs to be created</param>
        /// <param name="application">Name of application which is performing action</param>
        /// <param name="module">Name of module which is performing action</param>
        /// <returns>Id of the created import job</returns>
        /// <exception cref="EndpointNotFoundException">Thrown when a remote endpoint could not be found or reached</exception>
        /// <exception cref="InvalidOperationException">Thrown when a method call is invalid</exception>
        /// <exception cref="TimeoutException">Thrown when the time allotted for an operation has expired</exception>
        /// <exception cref="MDMOperationException">Thrown when the MDM operation fails</exception>
        /// <exception cref="CommunicationException">Thrown when there is a communication error in either the service or client application</exception>
        public Int32 InitiateImportJob(IJob iJob, MDMCenterApplication application, MDMCenterModules module)
        {
            return MakeServiceCall("InitiateImportJob",
                                   "InitiateImportJob",
                                   service => service.InitiateImportJob((Job) iJob, application, module));
        }

        /// <summary>
        /// Processes import job
        /// </summary>
        /// <param name="iJob">Job which needs to be processed</param>
        /// <param name="application">Name of application which is performing action</param>
        /// <param name="module">Name of module which is performing action</param>
        /// <returns>Process result</returns>
        /// <exception cref="EndpointNotFoundException">Thrown when a remote endpoint could not be found or reached</exception>
        /// <exception cref="InvalidOperationException">Thrown when a method call is invalid</exception>
        /// <exception cref="TimeoutException">Thrown when the time allotted for an operation has expired</exception>
        /// <exception cref="MDMOperationException">Thrown when the MDM operation fails</exception>
        /// <exception cref="CommunicationException">Thrown when there is a communication error in either the service or client application</exception>
        public Boolean ProcessImportJob(IJob iJob, MDMCenterApplication application, MDMCenterModules module)
        {
            return MakeServiceCall("ProcessImportJob",
                                   "ProcessImportJob",
                                   service => service.UpdateImportJob((Job) iJob, application, module));
        }

        /// <summary>
        /// Gets results of requested import job
        /// </summary>
        /// <param name="importJobId">Id of the job for which results are required</param>
        /// <param name="application">Name of application which is performing action</param>
        /// <param name="module">Name of module which is performing action</param>
        /// <returns>Collection of job import results</returns>
        /// <exception cref="EndpointNotFoundException">Thrown when a remote endpoint could not be found or reached</exception>
        /// <exception cref="InvalidOperationException">Thrown when a method call is invalid</exception>
        /// <exception cref="TimeoutException">Thrown when the time allotted for an operation has expired</exception>
        /// <exception cref="MDMOperationException">Thrown when the MDM operation fails</exception>
        /// <exception cref="CommunicationException">Thrown when there is a communication error in either the service or client application</exception>
        public IJobImportResultCollection GetImportJobResults(Int32 importJobId, MDMCenterApplication application, MDMCenterModules module)
        {
            return MakeServiceCall("GetImportJobResults",
                                   "GetImportJobResults",
                                   service => service.GetImportJobResults(importJobId, application, module));
        }

        /// <summary>
        /// Gets All available profiles for import
        /// </summary>
        /// <param name="application">Name of application which is performing action</param>
        /// <returns>Collection of profiles</returns>
        /// <exception cref="EndpointNotFoundException">Thrown when a remote endpoint could not be found or reached</exception>
        /// <exception cref="InvalidOperationException">Thrown when a method call is invalid</exception>
        /// <exception cref="TimeoutException">Thrown when the time allotted for an operation has expired</exception>
        /// <exception cref="MDMOperationException">Thrown when the MDM operation fails</exception>
        /// <exception cref="CommunicationException">Thrown when there is a communication error in either the service or client application</exception>
        public ImportProfileCollection GetProfileCollection(MDMCenterApplication application)
        {
            return MakeServiceCall("GetProfileCollection",
                                   "GetProfiles",
                                   service => service.GetProfiles(application));
        }

        /// <summary>
        ///  Create the Job for requested profile Id and return the Job id.
        ///  Place the file into data base.
        /// </summary>
        /// <example>
        /// <code>
        /// // Create an instance of integration service
        /// IntegrationService integrationService = GetMDMIntegrationService();
        /// 
        /// // Assuming the values of profile ids based on River Works Data Model
        /// // Profile ID is required to request for lookup import profile it can RSLookupXml|RSLookupExcel
        /// // Assuming hard coded values for demo purpose
        /// Int32 profileId = 20; //RSLookupExcel
        /// 
        /// // Assuming the values of profile name based on River Works Data Model
        /// // Profile Name is required to request for lookup import profile
        /// String profileName = "MDMCenter - RSLookupExcel 1.0 - Default Lookup Profile";
        /// 
        /// Int32 parentJobId = 0;
        /// 
        /// // File for uploading to DB
        /// MDM.BusinessObjects.File file = new File();
        /// 
        /// // Below snippet will make a WCF call to start the job which will read the values from the excel file 
        /// int startJob = integrationService.StartJob(MDMCenterApplication.PIM, file, profileName, profileId, parentJobId);
        /// </code>
        /// </example>
        /// <param name="application">Indicates the name of application which is performing action</param>
        /// <param name="file">File for uploading to DB</param>
        /// <param name="profileName">Indicates the profile name</param>
        /// <param name="profileId">Indicates the profile name</param>
        /// <param name="parentJobId">Indicates the Parent Job Id</param>
        /// <param name="jobType">Indicates the type of the job which is EntityImport</param>
        /// <exception cref="EndpointNotFoundException">Thrown when a remote endpoint could not be found or reached</exception>
        /// <exception cref="InvalidOperationException">Thrown when a method call is invalid</exception>
        /// <exception cref="TimeoutException">Thrown when the time allotted for an operation has expired</exception>
        /// <exception cref="MDMOperationException">Thrown when the MDM operation fails</exception>
        /// <exception cref="CommunicationException">Thrown when there is a communication error in either the service or client application</exception>
        /// <returns>Returns Id of the Job which has been started.</returns>
        public Int32 StartJob(MDMCenterApplication application, File file, String profileName, Int32 profileId, Int32 parentJobId, JobType jobType = JobType.EntityImport)
        {
            return MakeServiceCall("StartJob",
                                   "StartImportJob",
                                   service => service.StartImportJob(application,
                                                                     file,
                                                                     profileName,
                                                                     profileId,
                                                                     parentJobId,
                                                                     jobType));
        }

        /// <summary>
        ///  Create the Job for requested profile Id and return the Job id.
        ///  Place the file into data base.
        /// </summary>
        /// <example>
        /// <code>
        /// // Create an instance of integration service
        /// IntegrationService integrationService = GetMDMIntegrationService();
        /// 
        /// // Assuming the values of profile ids based on River Works Data Model
        /// // Profile ID is required to request for lookup import profile it can RSLookupXml|RSLookupExcel
        /// // Assuming hard coded values for demo purpose
        /// Int32 profileId = 20; //RSLookupExcel
        /// 
        /// // Assuming the values of profile name based on River Works Data Model
        /// // Profile Name is required to request for lookup import profile
        /// String profileName = "MDMCenter - RSLookupExcel 1.0 - Default Lookup Profile";
        /// 
        /// Int32 parentJobId = 0;
        /// 
        /// // File for uploading to DB
        /// MDM.BusinessObjects.File file = new File();
        /// 
        /// // Below snippet will make a WCF call to start the job which will read the values from the excel file 
        /// int startJob = integrationService.StartJob(MDMCenterApplication.PIM, file, profileName, profileId, parentJobId);
        /// </code>
        /// </example>
        /// <param name="file">File for uploading to DB</param>
        /// <param name="profileName">Indicates the profile name</param>
        /// <param name="profileId">Indicates the profile name</param>
        /// <param name="parentJobId">Indicates the Parent Job Id</param>
        /// <param name="callerContext">Caller context</param>
        /// <param name="jobType">Indicates the type of the job which is EntityImport</param>
        /// <exception cref="EndpointNotFoundException">Thrown when a remote endpoint could not be found or reached</exception>
        /// <exception cref="InvalidOperationException">Thrown when a method call is invalid</exception>
        /// <exception cref="TimeoutException">Thrown when the time allotted for an operation has expired</exception>
        /// <exception cref="MDMOperationException">Thrown when the MDM operation fails</exception>
        /// <exception cref="CommunicationException">Thrown when there is a communication error in either the service or client application</exception>
        /// <returns>Returns Id of the Job which has been started.</returns>
        public Int32 StartJob(File file, String profileName, Int32 profileId, Int32 parentJobId, CallerContext callerContext, JobType jobType = JobType.EntityImport)
        {
            return MakeServiceCall("StartJob",
                                   "StartImportJob",
                                   service => service.StartImportJob(file,
                                                                     profileName,
                                                                     profileId,
                                                                     parentJobId,
                                                                     jobType,
                                                                     FillDiagnosticTraces(callerContext)));
        }

        /// <summary>
        /// Get Error state of the requested job
        /// </summary>
        /// <param name="jobId">Id of the job</param>
        /// <param name="application">Name of application which is performing action</param>
        /// <returns>Collection of JobImport result</returns>
        public JobImportResultCollection GetErrorStates(int jobId, MDMCenterApplication application)
        {
            return MakeServiceCall("GetErrorStates",
                                   "GetErrorStates",
                                   service => service.GetErrorStates(jobId, application));
        }

        /// <summary>
        /// Gets the file with error states of the requested job.
        /// </summary>
        /// <param name="jobId">Indicates the Job Id</param>
        /// <param name="locale">Indicates Data Locale</param>
        /// <param name="iCallerContext">Indicates the caller context which contains application and module which invoked the API</param>
        /// <returns>File which contains error states of the job at current moment</returns>
        /// <exception cref="EndpointNotFoundException">Thrown when a remote endpoint could not be found or reached</exception>
        /// <exception cref="InvalidOperationException">Thrown when a method call is invalid</exception>
        /// <exception cref="TimeoutException">Thrown when the time allotted for an operation has expired</exception>
        /// <exception cref="MDMOperationException">Thrown when the MDM operation fails</exception>
        /// <exception cref="CommunicationException">Thrown when there is a communication error in either the service or client application</exception>
        /// <example>
        /// <code language="c#" title="Get Error States As Excel File" source="..\MDM.APISamples\Import\ImportDetails.cs" region="Get Error States As Excel File" />
        /// </example>
        public IFile GetErrorStatesAsExcelFile(Int32 jobId, LocaleEnum locale, ICallerContext iCallerContext)
        {
            return MakeServiceCall("GetErrorStatesAsExcelFile", "GetErrorStatesAsExcelFile",
                client =>
                client.GetErrorStatesAsExcelFile(jobId, locale, FillDiagnosticTraces(iCallerContext)));
        }

        /// <summary>
        /// Get DataModel Import JobResults
        /// </summary>
        /// <param name="importJobId"></param>
        /// <param name="objectType"></param>
        /// <param name="externalId"></param>
        /// <param name="callerContext"></param>
        /// <returns></returns>
        public DataModelOperationResultCollection GetDataModelImportJobResults(Int32 importJobId, ObjectType objectType, String externalId, CallerContext callerContext)
        {
            return MakeServiceCall("GetDataModelImportJobResults", "GetDataModelImportJobResults",
                client =>
                client.GetDataModelImportJobResults(importJobId, objectType, externalId, callerContext));
        }

        /// <summary>
        /// Get DataModelOperation Result Summary
        /// </summary>
        /// <param name="jobId"></param>
        /// <param name="callerContext"></param>
        /// <returns></returns>
        public DataModelOperationResultSummaryCollection GetDataModelOperationResultSummary(int jobId, CallerContext callerContext)
        {
            return MakeServiceCall("GetDataModelOperationResultSummary", "GetDataModelOperationResultSummary",
                client =>
                client.GetDataModelOperationResultSummary(jobId, callerContext));
        }

        #endregion

        #region Import Profile

        #region CUD operations

        /// <summary>
        /// Creates import profile
        /// </summary>
        /// <param name="importProfile">Import profile data</param>
        /// <param name="iCallerContext">Indicates Application and Module name by which action is being performed</param>
        /// <returns>
        /// The result of the operations
        /// </returns>
        public OperationResult CreateImportProfile(IImportProfile importProfile, ICallerContext iCallerContext)
        {
            return MakeServiceCall("CreateImportProfile", "CreateImportProfile",
                client =>
                client.CreateImportProfile(importProfile as ImportProfile, FillDiagnosticTraces(iCallerContext)));
        }

        /// <summary>
        /// Updates import profile
        /// </summary>
        /// <param name="importProfile">Import profile data</param>
        /// <param name="iCallerContext">Indicates Application and Module name by which action is being performed</param>
        /// <returns>The result of the operations</returns>
        public OperationResult UpdateImportProfile(IImportProfile importProfile, ICallerContext iCallerContext)
        {
            return MakeServiceCall("UpdateImportProfile", "UpdateImportProfile",
                client =>
                client.UpdateImportProfile(importProfile as ImportProfile, FillDiagnosticTraces(iCallerContext)));
        }

        /// <summary>
        /// Deletes import profile
        /// </summary>
        /// <param name="importProfile">Import profile data</param>
        /// <param name="iCallerContext">Indicates Application and Module name by which action is being performed</param>
        /// <returns>The result of the operations</returns>
        public OperationResult DeleteImportProfile(IImportProfile importProfile, ICallerContext iCallerContext)
        {
            return MakeServiceCall("DeleteImportProfile", "DeleteImportProfile",
                client =>
                client.DeleteImportProfile(importProfile as ImportProfile, FillDiagnosticTraces(iCallerContext)));
        }

        /// <summary>
        /// Process import profiles
        /// </summary>
        /// <param name="importProfiles">Import profile data</param>
        /// <param name="iCallerContext">Indicates Application and Module name by which action is being performed</param>
        /// <returns>The result of the operations</returns>
        public OperationResultCollection ProcessImportProfiles(IImportProfileCollection importProfiles, ICallerContext iCallerContext)
        {
            return MakeServiceCall("ProcessImportProfiles", "ProcessImportProfiles",
                client =>
                client.ProcessImportProfiles(importProfiles as ImportProfileCollection, FillDiagnosticTraces(iCallerContext)));
        }

        #endregion

        #region Get operations

        /// <summary>
        /// Get import profile from the system based on given profile Id
        /// </summary>
        /// <param name="profileId">Id of expected import profile</param>
        /// <param name="iCallerContext">Context which called the application</param>
        /// <returns>Import profile with given Id</returns>
        public IImportProfile GetImportProfile(Int32 profileId, ICallerContext iCallerContext)
        {
            return MakeServiceCall("GetImportProfile", "GetImportProfileById",
                client =>
                client.GetImportProfile(profileId, FillDiagnosticTraces(iCallerContext)));
        }

        /// <summary>
        /// Get import profile from the system based on given profile Name
        /// </summary>
        /// <param name="profileName">Name of expected import profile</param>
        /// <param name="iCallerContext">Context which called the application</param>
        /// <returns>Import profile with given Name</returns>
        public IImportProfile GetImportProfile(String profileName, ICallerContext iCallerContext)
        {
            return MakeServiceCall("GetImportProfile", "GetImportProfileByName",
                client =>
                client.GetImportProfile(profileName, FillDiagnosticTraces(iCallerContext)));
        }

        /// <summary>
        /// Gets all import profiles available
        /// </summary>
        /// <param name="iCallerContext">Indicates Application and Module name by which action is being performed</param>
        /// <returns>Collection of import profiles</returns>
        public IImportProfileCollection GetAllImportProfiles(ICallerContext iCallerContext)
        {
            return MakeServiceCall("GetAllImportProfiles", "GetAllImportProfiles",
                client =>
                client.GetAllImportProfiles(FillDiagnosticTraces(iCallerContext)));
        }

        /// <summary>
        /// Get import profile type
        /// </summary>
        /// <param name="profileId">Import profile id</param>
        /// <param name="iCallerContext">Indicates Application and Module name by which action is being performed</param>
        /// <returns>The type of import profile of the operations</returns>
        public String GetProfileType(Int32 profileId, ICallerContext iCallerContext)
        {
            return MakeServiceCall("GetProfileType", "GetImportProfileType",
                client =>
                client.GetProfileType(profileId, FillDiagnosticTraces(iCallerContext)));
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
            return MakeServiceCall("GetJobExecutionStatus", "GetJobExecutionStatus",
                client =>
                client.GetJobExecutionStatus(jobId, application, module));
        }

        #endregion

        #endregion

        #region Get Lookup Import Profile

        /// <summary>
        /// Gets the lookup import profile based on the profile id
        /// </summary>
        /// <example>
        /// <code>
        /// 
        /// //Create an instance of integration service
        /// IntegrationService integrationService = GetMDMIntegrationService();
        /// 
        /// //Assuming the values of profile ids based on River Works Data Model
        /// //Profile ID is required to request for lookup import profile it can RSLookupXml|RSLookupExcel
        /// Int32 profileId = 20; //RSLookupXml as per River Works Data Model
        /// 
        /// //Indicates name of application and module
        /// ICallerContext callerContext = <![CDATA[MDMObjectFactory.GetICallerContext()]]>;
        /// callerContext.Application = MDMCenterApplication.PIM;
        /// callerContext.Module = MDMCenterModules.Entity;
        /// 
        /// // Below will make WCF call which will get lookup import profile based on profile id and caller context
        /// ILookupImportProfile getLookupImportProfileById = integrationService.GetLookupImportProfileById(profileId, callerContext);
        /// 
        /// </code>
        /// </example>
        /// <param name="profileId">Indicates the Id of lookup import profile</param>
        /// <param name="iCallerContext">Indicates Application and Module name by which action is being performed</param>
        /// <returns>Lookup Import profile with given input lookup profile Id</returns>
        public ILookupImportProfile GetLookupImportProfileById(Int32 profileId, ICallerContext iCallerContext)
        {
            return MakeServiceCall("GetLookupImportProfileById", "GetLookupImportProfileById",
                client =>
                client.GetLookupImportProfileById(profileId, FillDiagnosticTraces(iCallerContext)));
        }

        /// <summary>
        /// Gets the lookup import profile based on profile name
        /// </summary>
        /// <example>
        /// <code>
        /// //Create an instance of integration service
        /// IntegrationService integrationService = GetMDMIntegrationService();
        /// 
        /// // Key Note: CallerContext has properties Application and Module which are mandatory to be set
        /// // Indicates name of application and module
        /// ICallerContext callerContext = <![CDATA[MDMObjectFactory.GetICallerContext()]]>;
        /// callerContext.Application = MDMCenterApplication.PIM;
        /// callerContext.Module = MDMCenterModules.Entity;
        /// 
        /// //Assuming the values of profile name based on River Works Data Model
        /// //Profile Name is required to request for lookup import profile
        /// String profileName = "MDMCenter - RSLookupExcel 1.0 - Default Lookup Profile";
        /// 
        /// // Below will make WCF call which will get lookup import profile based on profile name and caller context
        /// ILookupImportProfile getLookupImportProfileByName = integrationService.GetLookupImportProfileByName(profileName, callerContext);
        /// </code>
        /// </example>
        /// <param name="profileName">Indicates the name of expected lookup import profile</param>
        /// <param name="iCallerContext">Indicates Application and Module name by which action is being performed</param>
        /// <returns>Lookup Import profile with given input lookup profile name</returns>
        public ILookupImportProfile GetLookupImportProfileByName(String profileName, ICallerContext iCallerContext)
        {
            return MakeServiceCall("GetLookupImportProfileByName", "GetLookupImportProfileByName",
                client =>
                client.GetLookupImportProfileByName(profileName, FillDiagnosticTraces(iCallerContext)));
        }

        /// <summary>
        /// Gets all available Lookup import profiles
        /// </summary>
        /// <example>
        /// <code>
        /// //Create an instance of integration service
        /// IntegrationService integrationService = GetMDMIntegrationService();
        /// 
        /// // Key Note: CallerContext has properties Application and Module which are mandatory to be set
        /// // Indicates name of application and module
        /// // Here we need all the profiles based on caller context
        /// ICallerContext callerContext = <![CDATA[MDMObjectFactory.GetICallerContext()]]>;
        /// callerContext.Application = MDMCenterApplication.PIM;
        /// callerContext.Module = MDMCenterModules.Entity;
        /// 
        /// // Below will make WCF call which will get all the lookup import profiles based on caller context
        /// ILookupImportProfileCollection getAllLookupImportProfiles = integrationService.GetAllLookupImportProfiles(callerContext);
        /// 
        /// </code>
        /// </example>
        /// <param name="iCallerContext">Indicates Application and Module name by which action is being performed</param>
        /// <returns>Collection of Lookup import profiles</returns>
        public ILookupImportProfileCollection GetAllLookupImportProfiles(ICallerContext iCallerContext)
        {
            return MakeServiceCall("GetAllLookupImportProfiles", "GetAllLookupImportProfiles",
                client =>
                client.GetAllLookupImportProfiles(FillDiagnosticTraces(iCallerContext)));
        }

        #endregion

        #region JobSchedule CUD

        /// <summary>
        /// Create new JobSchedule
        /// </summary>
        /// <param name="iJobSchedule">Represent JobSchedule Object</param>
        /// <param name="iCallerContext">Context of application making call for this API</param>
        /// <returns>OperationResult of JobSchedule Creation</returns>
        /// <exception cref="ArgumentNullException">If JobSchedule Object is Null</exception>
        /// <exception cref="ArgumentNullException">If JobSchedule ShortName is Null or having empty String</exception>
        /// <exception cref="ArgumentNullException">If JobSchedule LongName is Null or having empty String</exception>
        public IOperationResult CreateJobSchedule(IJobSchedule iJobSchedule, ICallerContext iCallerContext)
        {
            return MakeServiceCall("CreateJobSchedule", "CreateJobSchedule",
               client =>
               client.CreateJobSchedule(iJobSchedule as JobSchedule, FillDiagnosticTraces(iCallerContext)));
        }

        /// <summary>
        /// Update JobSchedule
        /// </summary>
        /// <param name="iJobSchedule">Represent JobSchedule Object</param>
        /// <param name="iCallerContext">Context of application making call for this API</param>
        /// <returns>OperationResult of JobSchedule Updating</returns>
        /// <exception cref="ArgumentNullException">If JobSchedule Object is Null</exception>
        /// <exception cref="ArgumentNullException">If JobSchedule ShortName is Null or having empty String</exception>
        /// <exception cref="ArgumentNullException">If JobSchedule LongName is Null or having empty String</exception>
        public IOperationResult UpdateJobSchedule(IJobSchedule iJobSchedule, ICallerContext iCallerContext)
        {
            return MakeServiceCall("UpdateJobSchedule", "UpdateJobSchedule",
               client =>
               client.UpdateJobSchedule(iJobSchedule as JobSchedule, FillDiagnosticTraces(iCallerContext)));
        }

        /// <summary>
        /// Delete JobSchedule
        /// </summary>
        /// <param name="iJobSchedule">Represent JobSchedule Object</param>
        /// <param name="iCallerContext">Context of application making call for this API</param>
        /// <returns>OperationResult of JobSchedule Deletion</returns>
        /// <exception cref="ArgumentNullException">If JobSchedule Object is Null</exception>
        public IOperationResult DeleteJobSchedule(IJobSchedule iJobSchedule, ICallerContext iCallerContext)
        {
            return MakeServiceCall("DeleteJobSchedule", "DeleteJobSchedule",
               client =>
               client.DeleteJobSchedule(iJobSchedule as JobSchedule, FillDiagnosticTraces(iCallerContext)));
        }

        /// <summary>
        /// Process JobSchedules
        /// </summary>
        /// <param name="iJobSchedules">Represent JobSchedule Object collection to process</param>
        /// <param name="iCallerContext">Context of application making call for this API</param>
        /// <returns>OperationResult of JobSchedules process</returns>
        /// <exception cref="ArgumentNullException">If JobSchedules Object is Null</exception>
        public IOperationResultCollection ProcessJobSchedules(IJobScheduleCollection iJobSchedules, ICallerContext iCallerContext)
        {
            return MakeServiceCall("ProcessJobSchedules", "ProcessJobSchedules",
               client =>
               client.ProcessJobSchedules(iJobSchedules as JobScheduleCollection, FillDiagnosticTraces(iCallerContext)));
        }

        #endregion JobSchedule CUD

        #region JobSchedule Get

        /// <summary>
        /// Get all jobSchedules
        /// </summary>
        /// <param name="iCallerContext">Context of application making call for this API</param>
        /// <returns>All schedule criteria</returns>
        public IJobScheduleCollection GetAllJobSchedules(ICallerContext iCallerContext)
        {
            return MakeServiceCall("GetAllJobSchedules", "GetAllJobSchedules",
               client =>
               client.GetAllJobSchedules(FillDiagnosticTraces(iCallerContext)));
        }

        /// <summary>
        /// Get JobSchedule by Id
        /// </summary>
        /// <param name="scheduleId">If of schedule to fetch object</param>
        /// <param name="iCallerContext">Context of application making call for this API</param>
        /// <returns>Schedule criteria for given Id</returns>
        public IJobSchedule GetJobScheduleById(Int32 scheduleId, ICallerContext iCallerContext)
        {
            return MakeServiceCall("GetJobScheduleById", "GetJobScheduleById",
               client =>
               client.GetJobScheduleById(scheduleId, FillDiagnosticTraces(iCallerContext)));
        }

        #endregion JobSchedule Get

        #region IIntegrationActivityLog Create Methods

        /// <summary>
        /// Creates an integration activity log.
        /// Integration queues will pick this entry and calls the connector interface methods based on IntegrationType specified.
        /// </summary>
        /// <example>
        /// <code>
        /// // Create a caller context to indicate the application name and module name.
        /// ICallerContext callerContext = MDMObjectFactory.GetICallerContext();
        /// callerContext.Application = MDMCenterApplication.PIM;
        /// callerContext.Module = MDMCenterModules.Integration;
        /// 
        /// // Create the integration service
        /// IntegrationService integrationService = new IntegrationService();
        ///
        /// // Make the API call
        /// integrationService.CreateIntegrationActivityLog(
        ///     1005, // MDM Object Id
        ///     "Entity", // MDM Object Type Name
        ///     "DemoContext", // Context to be sent with the message
        ///     "1WorldSync", // Connector Short name
        ///     "ITEM ADD", // Integration message type short name
        ///     IntegrationType.Outbound, // Type of integration. Outbound/Inbound
        ///     0, // Weightage for the message
        ///     0, // Message count. This will be updated if connector disintegrates this message into multiple messages.
        ///     callerContext // Caller context
        ///     );
        /// </code>
        /// </example>
        /// <param name="mdmObjectId">Indicates the MDMObjectId which for which activity log is to be created.</param>
        /// <param name="mdmObjectTypeName">Indicates the name of MDMObject.</param>
        /// <param name="context">Represents the context for passed MDMObjectId</param>
        /// <param name="connectorShortName">Indicates the connector short name for which ActivityLog is to be created</param>
        /// <param name="integrationMessageTypeShortName">Indicates the short name of MessageType for which ActivityLog is to be created</param>
        /// <param name="integrationType">Represents the type of integration (inbound/outbound)</param>
        /// <param name="weightage">Indicates the weightage for activity</param>
        /// <param name="messsageCount">Indicates the count of messages generated from given activity</param>
        /// <param name="iCallerContext">Represents the context of caller making call to this API.</param>
        /// <returns>Returns the status indicating the result of operation</returns>
        public IOperationResult CreateIntegrationActivityLog(Int64 mdmObjectId, String mdmObjectTypeName, String context, String connectorShortName, String integrationMessageTypeShortName,
            IntegrationType integrationType, Int32 weightage, Int32 messsageCount, ICallerContext iCallerContext)
        {
            return this.MakeServiceCall<IOperationResult>("CreateIntegrationActivityLog", "CreateIntegrationActivityLogWithShortNames", client => client.CreateIntegrationActivityLog(mdmObjectId, mdmObjectTypeName, context, connectorShortName, integrationMessageTypeShortName,
                    integrationType, weightage, messsageCount, FillDiagnosticTraces(iCallerContext)), MDMTraceSource.Integration);
        }

        /// <summary>
        /// Creates an integration activity log.
        /// Integration queues will pick this entry and calls the connector interface methods based on IntegrationType specified.
        /// </summary>
        /// <example>
        /// <code>
        /// // Create a caller context to indicate the application name and module name.
        /// ICallerContext callerContext = MDMObjectFactory.GetICallerContext();
        /// callerContext.Application = MDMCenterApplication.PIM;
        /// callerContext.Module = MDMCenterModules.Integration;
        ///
        /// // Create the integration service
        /// IntegrationService integrationService = new IntegrationService();
        ///
        /// IIntegrationActivityLog activityLog = MDMObjectFactory.GetIIntegrationActivityLog();
        /// activityLog.ConnectorId = 1;
        /// activityLog.ConnectorLongName = "One World Sync";
        /// activityLog.Context = "Additional context information for the message";
        /// activityLog.IntegrationMessageTypeId = 1; //ITEM ADD
        /// activityLog.IntegrationMessageTypeLongName = "ITEM ADD";
        /// activityLog.MDMObjectId = 1001;
        /// activityLog.MDMObjectTypeId = 1;
        /// activityLog.MDMObjectTypeName = "Entity";
        /// activityLog.Weightage = 1;
        /// activityLog.Action = ObjectAction.Create;
        /// activityLog.IntegrationType = IntegrationType.Outbound;
        ///
        /// // Make the API call
        /// IOperationResult result = integrationService.CreateIntegrationActivityLog(activityLog, callerContext);
        /// </code>
        /// </example>
        /// <param name="iIntegrationActivityLog">Integration activity log to be created</param>
        /// <param name="iCallerContext">Indicates the context of caller making call to this API</param>
        /// <returns>Status indicating the result of operation</returns>
        public IOperationResult CreateIntegrationActivityLog(IIntegrationActivityLog iIntegrationActivityLog, ICallerContext iCallerContext)
        {
            return this.MakeServiceCall<IOperationResult>("CreateIntegrationActivityLog", "CreateIntegrationActivityLog",
                client => client.CreateIntegrationActivityLog(iIntegrationActivityLog as IntegrationActivityLog, FillDiagnosticTraces(iCallerContext)), MDMTraceSource.Integration);
        }

        #endregion  IIntegrationActivityLog Create Methods

        #region ConnectorProfile CUD

        /// <summary>
        /// Creates a connector profile
        /// </summary>
        /// <example>
        /// <code>
        /// // Create the integration service
        /// IntegrationService integrationService = new IntegrationService();
        ///
        /// // Create a caller context to indicate the application name and module name.
        /// ICallerContext callerContext = MDMObjectFactory.GetICallerContext();
        /// callerContext.Application = MDMCenterApplication.PIM;
        /// callerContext.Module = MDMCenterModules.Integration;
        /// 
        /// // Create an instance of IConnectorProfile and set required details
        /// IConnectorProfile profile = MDMObjectFactory.GetIConnectorProfile();
        /// profile.LongName = "One World Sync";
        /// profile.Name = "1WorldSync";
        /// profile.Weightage = 1;
        /// profile.DefaultInboundIntegrationMessageTypeName = "ITEM ADD";
        /// profile.GetRunTimeSpecifications().AssemblyName = "1WorldSyncConnector.dll";
        /// profile.GetRunTimeSpecifications().ClassName = "_1WorldSyncConnector._1WorldSyncConnector";
        /// profile.GetRunTimeSpecifications().FileWatcherFolderName = "1WorldSync";
        /// profile.GetRunTimeSpecifications().UseInplaceOrchestration = false;
        /// profile.AddAdditionalConfiguration("SenderGLN", "1100001016822");
        /// // Set the configuration for qualification, aggregation, processing configuration and schedule as required.
        /// profile.GetQualificationConfiguration().GetScheduleCriteria().DailyFrequency = DailyFrequencyOptions.EveryMinute;
        /// profile.GetProcessingConfiguration().GetScheduleCriteria().DailyFrequency = DailyFrequencyOptions.EveryMinute;
        /// profile.GetAggregationConfiguration().IsEnabledForInbound = false;
        /// profile.GetAggregationConfiguration().IsEnabledForOutbound = true;
        /// profile.GetAggregationConfiguration().OutboundBatchSize = 10;
        ///
        /// // Make the API call
        /// IOperationResult result = integrationService.CreateConnectorProfile(profile, callerContext);
        /// </code>
        /// </example>
        /// <param name="iConnectorProfile">Represents the connector profile to be created</param>
        /// <param name="iCallerContext">Indicates the context of caller making call to this API</param>
        /// <returns>Returns the status indicating the result of operation</returns>
        public IOperationResult CreateConnectorProfile(IConnectorProfile iConnectorProfile, ICallerContext iCallerContext)
        {
            return this.MakeServiceCall<IOperationResult>("CreateConnectorProfile", "CreateConnectorProfile", client => client.CreateConnectorProfile(iConnectorProfile as ConnectorProfile, FillDiagnosticTraces(iCallerContext)), MDMTraceSource.Integration);
        }

        /// <summary>
        /// Updates a connector profile
        /// </summary>
        /// <example>
        /// <code>
        /// // Create the integration service
        /// IntegrationService integrationService = new IntegrationService();
        ///
        /// // Create a caller context to indicate the application name and module name.
        /// ICallerContext callerContext = MDMObjectFactory.GetICallerContext();
        /// callerContext.Application = MDMCenterApplication.PIM;
        /// callerContext.Module = MDMCenterModules.Integration;
        /// 
        /// IConnectorProfile profile = MDMObjectFactory.GetIConnectorProfile();
        /// profile.Id = 1;
        /// profile.LongName = "One World Sync";
        /// profile.Name = "1WorldSync";
        /// profile.Weightage = 1;
        /// profile.DefaultInboundIntegrationMessageTypeName = "ITEM ADD";
        /// profile.GetRunTimeSpecifications().AssemblyName = "1WorldSyncConnector.dll";
        /// profile.GetRunTimeSpecifications().ClassName = "_1WorldSyncConnector._1WorldSyncConnector";
        /// profile.GetRunTimeSpecifications().FileWatcherFolderName = "1WorldSync";
        /// profile.GetRunTimeSpecifications().UseInplaceOrchestration = false;
        /// profile.AddAdditionalConfiguration("SenderGLN", "1100001016822");
        /// //Set the configuration for qualification, aggregation, processing configuration and schedule as required.
        /// profile.GetQualificationConfiguration().GetScheduleCriteria().DailyFrequency = DailyFrequencyOptions.EveryMinute;
        /// profile.GetProcessingConfiguration().GetScheduleCriteria().DailyFrequency = DailyFrequencyOptions.EveryMinute;
        /// profile.GetAggregationConfiguration().IsEnabledForInbound = false;
        /// profile.GetAggregationConfiguration().IsEnabledForOutbound = true;
        /// profile.GetAggregationConfiguration().OutboundBatchSize = 10;
        ///
        /// // Make the API call
        /// IOperationResult result = integrationService.UpdateConnectorProfile(profile, callerContext);
        /// </code>
        /// </example>
        /// <param name="iConnectorProfile">Represents the connector profile to be created</param>
        /// <param name="iCallerContext">Indicates the context of caller making call to this API</param>
        /// <returns>Returns the status indicating the result of operation</returns>
        public IOperationResult UpdateConnectorProfile(IConnectorProfile iConnectorProfile, ICallerContext iCallerContext)
        {
            return this.MakeServiceCall<IOperationResult>("UpdateConnectorProfile", "UpdateConnectorProfile", client => client.UpdateConnectorProfile(iConnectorProfile as ConnectorProfile, FillDiagnosticTraces(iCallerContext)), MDMTraceSource.Integration);
        }

        /// <summary>
        /// Deletes a connector profile
        /// </summary>
        /// <example>
        /// <code>
        /// // Create the integration service
        /// IntegrationService integrationService = new IntegrationService();
        ///
        /// // Create a caller context to indicate the application name and module name.
        /// ICallerContext callerContext = MDMObjectFactory.GetICallerContext();
        /// callerContext.Application = MDMCenterApplication.PIM;
        /// callerContext.Module = MDMCenterModules.Integration;
        /// 
        /// IConnectorProfile profile = MDMObjectFactory.GetIConnectorProfile();
        /// profile.Id = 1;
        /// profile.LongName = "One World Sync";
        /// profile.Name = "1WorldSync";
        /// profile.Weightage = 1;
        /// profile.DefaultInboundIntegrationMessageTypeName = "ITEM ADD";
        /// profile.GetRunTimeSpecifications().AssemblyName = "1WorldSyncConnector.dll";
        /// profile.GetRunTimeSpecifications().ClassName = "_1WorldSyncConnector._1WorldSyncConnector";
        /// profile.GetRunTimeSpecifications().FileWatcherFolderName = "1WorldSync";
        /// profile.GetRunTimeSpecifications().UseInplaceOrchestration = false;
        /// profile.AddAdditionalConfiguration("SenderGLN", "1100001016822");
        /// profile.GetQualificationConfiguration().GetScheduleCriteria().DailyFrequency = DailyFrequencyOptions.EveryMinute;
        /// profile.GetProcessingConfiguration().GetScheduleCriteria().DailyFrequency = DailyFrequencyOptions.EveryMinute;
        /// profile.GetAggregationConfiguration().IsEnabledForInbound = false;
        /// profile.GetAggregationConfiguration().IsEnabledForOutbound = true;
        /// profile.GetAggregationConfiguration().OutboundBatchSize = 10;
        ///
        /// // Make the API call
        /// IOperationResult result = integrationService.DeleteConnectorProfile(profile, callerContext);
        /// </code>
        /// </example>
        /// <param name="iConnectorProfile">Represents the connector profile to be created</param>
        /// <param name="iCallerContext">Indicates the context of caller making call to this API</param>
        /// <returns>Returns the status indicating the result of operation</returns>
        public IOperationResult DeleteConnectorProfile(IConnectorProfile iConnectorProfile, ICallerContext iCallerContext)
        {
            return this.MakeServiceCall<IOperationResult>("DeleteConnectorProfile", "DeleteConnectorProfile", client => client.DeleteConnectorProfile(iConnectorProfile as ConnectorProfile, FillDiagnosticTraces(iCallerContext)), MDMTraceSource.Integration);
        }

        #endregion ConnectorProfile CUD

        #region Get ConnectorProfile

        /// <summary>
        /// Gets all connector profiles in the system
        /// </summary>
        /// <example>
        /// <code>
        /// // Create the integration service
        /// IntegrationService integrationService = new IntegrationService();
        ///
        /// // Create a caller context to indicate the application name and module name.
        /// ICallerContext callerContext = MDMObjectFactory.GetICallerContext();
        /// callerContext.Application = MDMCenterApplication.PIM;
        /// callerContext.Module = MDMCenterModules.Integration;
        /// 
        /// // Make the API call
        /// IConnectorProfileCollection connectorProfiles = integrationService.GetAllConnectorProfiles(callerContext);
        /// </code>
        /// </example>
        /// <param name="iCallerContext">Indicates the context of caller making call to this API</param>
        /// <returns>Returns collection of connector profiles</returns>
        public IConnectorProfileCollection GetAllConnectorProfiles(ICallerContext iCallerContext)
        {
            return this.MakeServiceCall<IConnectorProfileCollection>("GetAllConnectorProfile", "GetAllConnectorProfile", client => client.GetAllConnectorProfiles(FillDiagnosticTraces(iCallerContext)), MDMTraceSource.Integration);
        }

        /// <summary>
        /// Gets the connector profile for a given id
        /// </summary>
        /// <example>
        /// <code>
        /// // Create the integration service
        /// IntegrationService integrationService = new IntegrationService();
        ///
        /// // Create a caller context to indicate the application name and module name.
        /// ICallerContext callerContext = MDMObjectFactory.GetICallerContext();
        /// callerContext.Application = MDMCenterApplication.PIM;
        /// callerContext.Module = MDMCenterModules.Integration;
        /// 
        /// // Make the API call
        /// IConnectorProfile connectorProfile = integrationService.GetConnectorProfileById(1,callerContext);
        /// </code>
        /// </example>
        /// <param name="connectorProfileId">Indicates the id of connector for which info needs to be fetched</param>
        /// <param name="iCallerContext">Indicates the context of caller making call to this API</param>
        /// <returns>Returns the connector profile</returns>
        public IConnectorProfile GetConnectorProfileById(Int16 connectorProfileId, ICallerContext iCallerContext)
        {
            return this.MakeServiceCall<IConnectorProfile>("GetConnectorProfileById", "GetConnectorProfileById", client => client.GetConnectorProfileById(connectorProfileId, FillDiagnosticTraces(iCallerContext)), MDMTraceSource.Integration);
        }

        /// <summary>
        /// Gets the connector profile for a given short name
        /// </summary>
        /// <example>
        /// <code>
        /// // Create the integration service
        /// IntegrationService integrationService = new IntegrationService();
        ///
        /// // Create a caller context to indicate the application name and module name.
        /// ICallerContext callerContext = MDMObjectFactory.GetICallerContext();
        /// callerContext.Application = MDMCenterApplication.PIM;
        /// callerContext.Module = MDMCenterModules.Integration;
        /// 
        /// // Make the API call
        /// IConnectorProfile connectorProfile = integrationService.GetConnectorProfileByName("Connector Short Name", callerContext);
        /// </code>
        /// </example>
        /// <param name="connectorProfileShortName">Indicates the short name of connector for which info needs to be fetched</param>
        /// <param name="iCallerContext">Indicates the context of caller making call to this API</param>
        /// <returns>Returns the connector profile</returns>
        public IConnectorProfile GetConnectorProfileByName(String connectorProfileShortName, ICallerContext iCallerContext)
        {
            return this.MakeServiceCall<IConnectorProfile>("GetConnectorProfileByName", "GetConnectorProfileByName", client => client.GetConnectorProfileByName(connectorProfileShortName, FillDiagnosticTraces(iCallerContext)), MDMTraceSource.Integration);
        }

        #endregion Get ConnectorProfile

        #region IntegrationMessage Get methods

        /// <summary>
        /// Gets integration message by id
        /// </summary>
        /// <example>
        /// <code>
        /// // Create the integration service
        /// IntegrationService integrationService = new IntegrationService();
        ///
        /// // Create a caller context to indicate the application name and module name.
        /// ICallerContext callerContext = MDMObjectFactory.GetICallerContext();
        /// callerContext.Application = MDMCenterApplication.PIM;
        /// callerContext.Module = MDMCenterModules.Integration;
        /// 
        /// // Make the API call
        /// IIntegrationMessage connectorProfile = integrationService.GetIntegrationMessageById(1,callerContext);
        /// </code>
        /// </example>
        /// <param name="integrationMessageId">Indicates the id of integration message to fetch the info for</param>
        /// <param name="iCallerContext">Indicates the context of caller making call to this API</param>
        /// <returns>Returns integration message object</returns>
        public IIntegrationMessage GetIntegrationMessageById(Int64 integrationMessageId, ICallerContext iCallerContext)
        {
            return this.MakeServiceCall<IIntegrationMessage>("GetIntegrationMessageById", "GetIntegrationMessageById",
                client => client.GetIntegrationMessageById(integrationMessageId, FillDiagnosticTraces(iCallerContext)), MDMTraceSource.Integration);
        }

        #endregion IntegrationMessage Get methods

        #region Get IntegrationMessageType

        /// <summary>
        /// Gets all integration message types in the system
        /// </summary>
        /// <example>
        /// <code>
        /// // Create the integration service
        /// IntegrationService integrationService = new IntegrationService();
        ///
        /// // Create a caller context to indicate the application name and module name.
        /// ICallerContext callerContext = MDMObjectFactory.GetICallerContext();
        /// callerContext.Application = MDMCenterApplication.PIM;
        /// callerContext.Module = MDMCenterModules.Integration;
        /// 
        /// // Make the API call
        /// IIntegrationMessageTypeCollection integrationMessageTypes = integrationService.GetAllIntegrationMessageTypes(callerContext);
        /// </code>
        /// </example>
        /// <param name="iCallerContext">Indicates the context of caller making call to this API</param>
        /// <returns>Returns collection of IntegrationMessageType</returns>
        public IIntegrationMessageTypeCollection GetAllIntegrationMessageTypes(ICallerContext iCallerContext)
        {
            return this.MakeServiceCall<IIntegrationMessageTypeCollection>("GetAllIntegrationMessageType", "GetAllIntegrationMessageType", client => client.GetAllIntegrationMessageTypes(FillDiagnosticTraces(iCallerContext)), MDMTraceSource.Integration);
        }

        /// <summary>
        /// Gets the integration message type for a given id
        /// </summary>
        /// <example>
        /// <code>
        /// // Create the integration service
        /// IntegrationService integrationService = new IntegrationService();
        ///
        /// // Create a caller context to indicate the application name and module name.
        /// ICallerContext callerContext = MDMObjectFactory.GetICallerContext();
        /// callerContext.Application = MDMCenterApplication.PIM;
        /// callerContext.Module = MDMCenterModules.Integration;
        /// 
        /// // Make the API call
        /// IIntegrationMessageType integrationMessageType = integrationService.GetIntegrationMessageTypeById(1, callerContext);
        /// </code>
        /// </example>
        /// <param name="integrationMessageTypeId">Indicates the id of IntegrationMessageType for which info needs to be fetched</param>
        /// <param name="iCallerContext">Indicates the context of caller making call to this API</param>
        /// <returns>Returns integration message type object</returns>
        public IIntegrationMessageType GetIntegrationMessageTypeById(Int16 integrationMessageTypeId, ICallerContext iCallerContext)
        {
            return this.MakeServiceCall<IIntegrationMessageType>("GetIntegrationMessageTypeById", "GetIntegrationMessageTypeById", client => client.GetIntegrationMessageTypeById(integrationMessageTypeId, FillDiagnosticTraces(iCallerContext)), MDMTraceSource.Integration);
        }

        /// <summary>
        /// Gets the integration message type for a given short name
        /// </summary>
        /// <example>
        /// <code>
        /// // Create the integration service
        /// IntegrationService integrationService = new IntegrationService();
        ///
        /// // Create a caller context to indicate the application name and module name.
        /// ICallerContext callerContext = MDMObjectFactory.GetICallerContext();
        /// callerContext.Application = MDMCenterApplication.PIM;
        /// callerContext.Module = MDMCenterModules.Integration;
        /// 
        /// // Make the API call
        /// IIntegrationMessageType integrationMessageType = integrationService.GetIntegrationMessageTypeByName("Integration Message Type Short Name", callerContext);
        /// </code>
        /// </example>
        /// <param name="integrationMessageTypeShortName">Indicates the short name of IntegrationMessageType for which info needs to be fetched</param>
        /// <param name="iCallerContext">Indicates the context of caller making call to this API</param>
        /// <returns>Returns integration message type object</returns>
        public IIntegrationMessageType GetIntegrationMessageTypeByName(String integrationMessageTypeShortName, ICallerContext iCallerContext)
        {
            return this.MakeServiceCall<IIntegrationMessageType>("GetIntegrationMessageTypeByName", "GetIntegrationMessageTypeByName", client => client.GetIntegrationMessageTypeByName(integrationMessageTypeShortName, FillDiagnosticTraces(iCallerContext)), MDMTraceSource.Integration);
        }

        #endregion Get IntegrationMessageType

        #region Get MDMObjectType

        /// <summary>
        /// Gets all MDM object types in the system
        /// </summary>
        /// <example>
        /// <code>
        /// // Create the integration service
        /// IntegrationService integrationService = new IntegrationService();
        ///
        /// // Create a caller context to indicate the application name and module name.
        /// ICallerContext callerContext = MDMObjectFactory.GetICallerContext();
        /// callerContext.Application = MDMCenterApplication.PIM;
        /// callerContext.Module = MDMCenterModules.Integration;
        /// 
        /// // Make the API call
        /// IMDMObjectTypeCollection mdmObjectTypeCollection = integrationService.GetAllMDMObjectTypes(callerContext);
        /// </code>
        /// </example>
        /// <param name="iCallerContext">Indicates the context of caller making call to this API</param>
        /// <returns>Returns MDMObjectType object collection</returns>
        public IMDMObjectTypeCollection GetAllMDMObjectTypes(ICallerContext iCallerContext)
        {
            return this.MakeServiceCall<IMDMObjectTypeCollection>("GetAllMDMObjectType", "GetAllMDMObjectType", client => client.GetAllMDMObjectTypes(FillDiagnosticTraces(iCallerContext)), MDMTraceSource.Integration);
        }

        /// <summary>
        /// Gets the MDM object type for a given id
        /// </summary>
        /// <example>
        /// <code>
        /// // Create the integration service
        /// IntegrationService integrationService = new IntegrationService();
        ///
        /// // Create a caller context to indicate the application name and module name.
        /// ICallerContext callerContext = MDMObjectFactory.GetICallerContext();
        /// callerContext.Application = MDMCenterApplication.PIM;
        /// callerContext.Module = MDMCenterModules.Integration;
        /// 
        /// // Make the API call
        /// IMDMObjectType mdmObjectType = integrationService.GetMDMObjectTypeById(1, callerContext);
        /// </code>
        /// </example>
        /// <param name="mdmObjectTypeId">Indicates the Id of MDMObjectType for which info needs to be fetched</param>
        /// <param name="iCallerContext">Indicates the context of caller making call to this API</param>
        /// <returns>Returns MDMObjectType object</returns>
        public IMDMObjectType GetMDMObjectTypeById(Int16 mdmObjectTypeId, ICallerContext iCallerContext)
        {
            return this.MakeServiceCall<IMDMObjectType>("GetMDMObjectTypeById", "GetMDMObjectTypeById", client => client.GetMDMObjectTypeById(mdmObjectTypeId, FillDiagnosticTraces(iCallerContext)), MDMTraceSource.Integration);
        }

        /// <summary>
        /// Gets the MDM object type for a given short name
        /// </summary>
        /// <example>
        /// <code>
        /// // Create the integration service
        /// IntegrationService integrationService = new IntegrationService();
        ///
        /// // Create a caller context to indicate the application name and module name.
        /// ICallerContext callerContext = MDMObjectFactory.GetICallerContext();
        /// callerContext.Application = MDMCenterApplication.PIM;
        /// callerContext.Module = MDMCenterModules.Integration;
        /// 
        /// // Make the API call
        /// IMDMObjectType mdmObjectType = integrationService.GetMDMObjectTypeByName("MDM Object Type Short Name", callerContext);
        /// </code>
        /// </example>
        /// <param name="mdmObjectTypeName">Indicates the short name of MDMObjectType for which info needs to be fetched</param>
        /// <param name="iCallerContext">Indicates the context of caller making call to this API</param>
        /// <returns>Returns MDMObjectType object</returns>
        public IMDMObjectType GetMDMObjectTypeByName(String mdmObjectTypeName, ICallerContext iCallerContext)
        {
            return this.MakeServiceCall<IMDMObjectType>("GetMDMObjectTypeByName", "GetMDMObjectTypeByName", client => client.GetMDMObjectTypeByName(mdmObjectTypeName, FillDiagnosticTraces(iCallerContext)), MDMTraceSource.Integration);
        }

        #endregion Get MDMObjectType

        #region Update IntegrationItem Status

        /// <summary>
        /// Updates integration item status. Update item and dimension status for given Id/Type
        /// </summary>
        /// <example>
        /// <code>
        /// // Create the integration service
        /// IntegrationService integrationService = new IntegrationService();
        ///
        /// // Create a caller context to indicate the application name and module name.
        /// ICallerContext callerContext = MDMObjectFactory.GetICallerContext();
        /// callerContext.Application = MDMCenterApplication.PIM;
        /// callerContext.Module = MDMCenterModules.Integration;
        ///
        /// // Create IIntegrationItemStatus
        /// IIntegrationItemStatus integrationItemStatus = MDMObjectFactory.GetIIntegrationItemStatus();
        ///
        /// // Set the connector to which this status intends to
        /// integrationItemStatus.ConnectorName = "ConnectorShortName";
        ///
        /// // Hard coded MDM object id for the demo
        /// integrationItemStatus.MDMObjectId = 32;
        ///
        /// // Represents the MDM object type, MDMObjectId refers to
        /// integrationItemStatus.MDMObjectTypeName = "Entity";
        ///
        /// // Represents the identifier of MDMObjectId in external system
        /// integrationItemStatus.ExternalId = "IdReferencedByExternalSystem";
        ///
        /// // Represents the name of the external object MDMObjectId refers in external system
        /// integrationItemStatus.ExternalObjectTypeName = "ExternalSystemObjectName";
        ///
        /// // Represents the current status of the connector call to be logged
        /// integrationItemStatus.Status = "Current Status of Connector Call";
        ///
        /// // Comments associated with the current status of the connector call to be logged
        /// integrationItemStatus.Comments = "Comments associated with the current status update call.";
        ///
        /// // Type of update status call. Can be Information/Error/Warning
        /// integrationItemStatus.StatusType = OperationResultType.Information;
        ///
        /// // Represents the flag if this status needs to be displayed in summary of Integration Status UI
        /// integrationItemStatus.IsExternalStatus = false;
        ///
        /// // Create a dimension collection to associate with the current status.
        /// // Dimensions provide a way to add connector specific status details. It is presumed that tb_Integration_ItemDimensionType is being populated with the dimensions.
        /// <b>// KEY NOTE: Collection should have only one dimension of a particular type.</b>
        /// IIntegrationItemStatusDimensionCollection dimensionCollection = MDMObjectFactory.GetIIntegrationItemStatusDimensionCollection();
        /// dimensionCollection.Add("DimenstionTypeShortName1", "Value of DimenstionTypeShortName1");
        /// dimensionCollection.Add("DimenstionTypeShortName2", "Value of DimenstionTypeShortName2");
        ///
        /// integrationItemStatus.SetStatusDimensions(dimensionCollection);
        ///
        /// // Make the call to service API
        /// IOperationResult result = integrationService.UpdateIntegrationItemStatus(integrationItemStatus, callerContext);
        /// </code>
        /// </example>
        /// <param name="iIntegrationItemStatus">Represents status for an item. It contains item information and status information</param>
        /// <param name="iCallerContext">Indicates the context of API making call to this API.</param>
        /// <returns>Returns the result of operation.</returns>
        public IOperationResult UpdateIntegrationItemStatus(IIntegrationItemStatus iIntegrationItemStatus, ICallerContext iCallerContext)
        {
            return this.MakeServiceCall<IOperationResult>("UpdateIntegrationItemStatus", "UpdateIntegrationItemStatus", client => client.UpdateIntegrationItemStatus(iIntegrationItemStatus as IntegrationItemStatus, FillDiagnosticTraces(iCallerContext)), MDMTraceSource.Integration);
        }

        #endregion Update IntegrationItem Status

        #region Search IntegrationItem Status

        /// <summary>
        /// Returns a collection of Integration item status based on given search criteria
        /// </summary>
        /// <example>
        /// <code>
        /// // Create the integration service
        /// IntegrationService integrationService = new IntegrationService();
        ///
        /// // Create a caller context to indicate the application name and module name.
        /// ICallerContext callerContext = MDMObjectFactory.GetICallerContext();
        /// callerContext.Application = MDMCenterApplication.PIM;
        /// callerContext.Module = MDMCenterModules.Integration;
        /// 
        /// IIntegrationItemStatusSearchCriteria criteria = MDMObjectFactory.GetIIntegrationItemStatusSearchCriteria();
        ///
        /// criteria.ConnectorId = 1; // Filter for a specific connector
        /// criteria.AddMDMObjectTypeIdAndValues(1, "1,2"); // Filter for specific MDMObject Type and ids
        ///
        /// criteria.AddExternalObjectTypeIdAndValues(1, "1,2"); // Filter for specific External Object Type and ids
        /// criteria.AddDimensionValuesAndStatus(1, "DimensionValue1, DimensionValue2"); // Filter for specific integration item dimension Type and ids
        ///
        /// // Make the API call
        /// IIntegrationItemStatusInternalCollection integrationItemStatusInternalCollection = integrationService.SearchIntegrationItemStatus(criteria, callerContext);
        /// </code>
        /// </example>
        /// <param name="iIntegrationItemStatusSearchCriteria">Represents the search criteria for IntegrationItemStatus</param>
        /// <param name="iCallerContext">Indicates context of caller making call to this API</param>
        /// <returns>Returns the integration item status records which are matching the given search criteria</returns>
        public IIntegrationItemStatusInternalCollection SearchIntegrationItemStatus(IIntegrationItemStatusSearchCriteria iIntegrationItemStatusSearchCriteria, ICallerContext iCallerContext)
        {
            return this.MakeServiceCall<IIntegrationItemStatusInternalCollection>("SearchIntegrationItemStatus", "SearchIntegrationItemStatus", client => client.SearchIntegrationItemStatus(iIntegrationItemStatusSearchCriteria as IntegrationItemStatusSearchCriteria, FillDiagnosticTraces(iCallerContext)), MDMTraceSource.Integration);
        }

        #endregion Search IntegrationItem Status

        #region Search IntegrationItemDimension Type

        /// <summary>
        /// Gets all integration item dimension types in the system
        /// </summary>
        /// <example>
        /// <code>
        /// // Create the integration service
        /// IntegrationService integrationService = new IntegrationService();
        ///
        /// // Create a caller context to indicate the application name and module name.
        /// ICallerContext callerContext = MDMObjectFactory.GetICallerContext();
        /// callerContext.Application = MDMCenterApplication.PIM;
        /// callerContext.Module = MDMCenterModules.Integration;
        /// 
        /// // Make the API call
        /// IIntegrationItemDimensionTypeCollection integrationItemDimensionTypes = integrationService.GetAllIntegrationItemDimensionTypes(callerContext);
        /// </code>
        /// </example>
        /// <param name="iCallerContext">Indicates the context of caller making call to this API</param>
        /// <returns>Returns collection of IntegrationItemDimensionType objects</returns>
        public IIntegrationItemDimensionTypeCollection GetAllIntegrationItemDimensionTypes(ICallerContext iCallerContext)
        {
            return this.MakeServiceCall<IIntegrationItemDimensionTypeCollection>("GetAllIntegrationItemDimensionTypes", "GetAllIntegrationItemDimensionTypes", client => client.GetAllIntegrationItemDimensionTypes(FillDiagnosticTraces(iCallerContext)), MDMTraceSource.Integration);
        }

        /// <summary>
        /// Gets the integration item dimension type for a given Id
        /// </summary>
        /// <example>
        /// <code>
        /// // Create the integration service
        /// IntegrationService integrationService = new IntegrationService();
        ///
        /// // Create a caller context to indicate the application name and module name.
        /// ICallerContext callerContext = MDMObjectFactory.GetICallerContext();
        /// callerContext.Application = MDMCenterApplication.PIM;
        /// callerContext.Module = MDMCenterModules.Integration;
        /// 
        /// // Make the API call
        /// IIntegrationItemDimensionType integrationItemDimensionType = integrationService.GetIntegrationItemDimensionTypeById(1, callerContext);
        /// </code>
        /// </example>
        /// <param name="integrationItemDimensionTypeId">Indicates the Id of IntegrationItemDimensionType for which info needs to be fetched</param>
        /// <param name="iCallerContext">Indicates the context of caller making call to this API</param>
        /// <returns>Returns the IntegrationItemDimensionType object</returns>
        public IIntegrationItemDimensionType GetIntegrationItemDimensionTypeById(Int16 integrationItemDimensionTypeId,
            ICallerContext iCallerContext)
        {
            return this.MakeServiceCall<IIntegrationItemDimensionType>("GetIntegrationItemDimensionTypeById", "GetIntegrationItemDimensionTypeById", client => client.GetIntegrationItemDimensionTypeById(integrationItemDimensionTypeId, FillDiagnosticTraces(iCallerContext)), MDMTraceSource.Integration);
        }

        /// <summary>
        /// Gets the integration item dimension type for a given short name
        /// </summary>
        /// <example>
        /// <code>
        /// // Create the integration service
        /// IntegrationService integrationService = new IntegrationService();
        ///
        /// // Create a caller context to indicate the application name and module name.
        /// ICallerContext callerContext = MDMObjectFactory.GetICallerContext();
        /// callerContext.Application = MDMCenterApplication.PIM;
        /// callerContext.Module = MDMCenterModules.Integration;
        /// 
        /// // Make the API call
        /// IIntegrationItemDimensionType integrationItemDimensionType = integrationService.GetIntegrationItemDimensionTypeByName("Integration Item Dimension Type Short Name", callerContext);
        /// </code>
        /// </example>
        /// <param name="integrationItemDimensionTypeShortName">Indicates the short name of IntegrationItemDimensionType for which info needs to be fetched</param>
        /// <param name="iCallerContext">Indicates the context of caller making call to this API</param>
        /// <returns>IntegrationItemDimensionType object</returns>
        public IIntegrationItemDimensionType GetIntegrationItemDimensionTypeByName(String integrationItemDimensionTypeShortName,
            ICallerContext iCallerContext)
        {
            return this.MakeServiceCall<IIntegrationItemDimensionType>("GetIntegrationItemDimensionTypeByName", "GetIntegrationItemDimensionTypeByName", client => client.GetIntegrationItemDimensionTypeByName(integrationItemDimensionTypeShortName, FillDiagnosticTraces(iCallerContext)), MDMTraceSource.Integration);
        }

        /// <summary>
        /// Gets the integration item dimension types for a given connector profile id.
        /// </summary>
        /// <example>
        /// <code>
        /// // Create the integration service
        /// IntegrationService integrationService = new IntegrationService();
        ///
        /// // Create a caller context to indicate the application name and module name.
        /// ICallerContext callerContext = MDMObjectFactory.GetICallerContext();
        /// callerContext.Application = MDMCenterApplication.PIM;
        /// callerContext.Module = MDMCenterModules.Integration;
        /// 
        /// // Make the API call
        /// IIntegrationItemDimensionTypeCollection integrationItemDimensionTypes = integrationService.GetIntegrationItemDimensionTypesByConnectorId(1, callerContext);
        /// </code>
        /// </example>
        /// <param name="connectorId">Indicates the Id of connector profile for which integration item dimension types needs to be fetched</param>
        /// <param name="iCallerContext">Indicates the context of caller making call to this API</param>
        /// <returns>Returns Collection of integration item dimension types for the specified connector</returns>
        public IIntegrationItemDimensionTypeCollection GetIntegrationItemDimensionTypesByConnectorId(Int16 connectorId, ICallerContext iCallerContext)
        {
            return this.MakeServiceCall<IIntegrationItemDimensionTypeCollection>("GetIntegrationItemDimensionTypesByConnectorId", "GetIntegrationItemDimensionTypesByConnectorId",
                client => client.GetIntegrationItemDimensionTypesByConnectorId(connectorId, FillDiagnosticTraces(iCallerContext)), MDMTraceSource.Integration);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Get Integration Service Client
        /// </summary>
        /// <returns>ServiceClient with UerName and Password</returns>
        private IIntegrationService GetClient()
        {
            IIntegrationService serviceClient = null;

            if (WCFServiceInstanceLoader.IsLocalInstancesEnabled())
            {
                serviceClient = WCFServiceInstanceLoader.GetServiceInstance<IIntegrationService>();
            }

            if (serviceClient == null) //Means the given type is not implemented for local load..
            {
                IntegrationServiceProxy integrationServiceProxy = null;

                if (String.IsNullOrEmpty(this.EndPointConfigurationName) && this.EndpointAddress == null)
                    integrationServiceProxy = new IntegrationServiceProxy();
                else if (!String.IsNullOrEmpty(this.EndPointConfigurationName) && this.EndpointAddress == null)
                    integrationServiceProxy = new IntegrationServiceProxy(this.EndPointConfigurationName);
                else if (!String.IsNullOrEmpty(this.EndPointConfigurationName) && this.EndpointAddress != null)
                    integrationServiceProxy = new IntegrationServiceProxy(this.EndPointConfigurationName, this.EndpointAddress);

                if (this.AuthenticationType == Core.AuthenticationType.Forms)
                {
                    integrationServiceProxy.ClientCredentials.UserName.UserName = this.UserName;
                    integrationServiceProxy.ClientCredentials.UserName.Password = this.Password;
                    integrationServiceProxy.ClientCredentials.ServiceCertificate.Authentication.CertificateValidationMode = System.ServiceModel.Security.X509CertificateValidationMode.None;
                }

                serviceClient = integrationServiceProxy;
            }

            return serviceClient;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="client"></param>
        private void DisposeClient(IIntegrationService client)
        {
            if (client == null)
                return;

            if (client.GetType().Equals(typeof(IntegrationServiceProxy)))
            {
                IntegrationServiceProxy serviceClient = (IntegrationServiceProxy)client;
                if (serviceClient.State == CommunicationState.Created || serviceClient.State == CommunicationState.Opened || serviceClient.State == CommunicationState.Opening)
                {
                    serviceClient.Close();
                }
                else if (serviceClient.State == CommunicationState.Faulted)
                {
                    serviceClient.Abort();
                }
            }
            else
            {
                //Do nothing...
            }
        }

        /// <summary>
        /// Makes the IntegrationService call. Creates the client instance, executes call delegate against it in
        /// impersonated context (if necessary) and then disposes the client.
        /// Also emits traces when necessary.
        /// </summary>
        /// <typeparam name="TResult">Indicates type of the result of service call.</typeparam>
        /// <param name="clientMethodName">Indicates name of the client method to include in traces.</param>
        /// <param name="serverMethodName">Indicates name of the server method to include in traces.</param>
        /// <param name="call">Indicates call delegate to be executed against a client instance.</param>
        /// <param name="traceSource">Indicates name of the trace source</param>
        /// <returns>Returns value returned by service, or default.</returns>
        private TResult MakeServiceCall<TResult>(string clientMethodName, string serverMethodName, Func<IIntegrationService, TResult> call, MDMTraceSource traceSource = MDMTraceSource.General)
        {
            Boolean isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;
            //Start trace activity
            if (isTracingEnabled)
            {
                MDMTraceHelper.StartTraceActivity("IntegrationServiceClient." + clientMethodName, traceSource, false);
            }

            TResult result = default(TResult);
            IIntegrationService integrationService = null;

            try
            {
                integrationService = GetClient();

                ValidateContext();
                if (isTracingEnabled)
                {
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Verbose, "IntegrationServiceClient sends '" + serverMethodName + "' request message.", traceSource);
                }

                result = Impersonate(() => call(integrationService));

                if (isTracingEnabled)
                {
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Verbose, "IntegrationServiceClient receives '" + serverMethodName + "' response message.", traceSource);
                }
            }
            catch (Exception ex) //Here we will catch all types of exceptions and then real handling happens in the base class method 'HandleException'
            {
                HandleException(ex);
            }
            finally
            {
                //Close the client
                DisposeClient(integrationService);
                if (isTracingEnabled)
                {
                    MDMTraceHelper.StopTraceActivity("IntegrationServiceClient." + clientMethodName, traceSource);
                }
            }

            return result;
        }

        #endregion
    }
}