using System;
using System.ServiceModel;
using MDM.BusinessObjects.Exports;

namespace MDM.Services.ServiceProxies
{
    using MDM.BusinessObjects;
    using MDM.BusinessObjects.Imports;
    using MDM.BusinessObjects.Jobs;
    using MDM.Core;
    using MDM.Core.Exceptions;
    using MDM.Services.IntegrationServiceClient;

    /// <summary>
    /// Proxy class for IntegrationService
    /// </summary>
    public class IntegrationServiceProxy : IntegrationServiceClient, MDM.WCFServiceInterfaces.IIntegrationService
    {
        #region Constructors

        /// <summary>
        /// Parameterless Constructor
        /// </summary>
        public IntegrationServiceProxy()
        {

        }

        /// <summary>
        /// Constructor with endpoint configuration name
        /// </summary>
        /// <param name="endpointConfigurationName">Indicates the endpoint configuration name</param>
        public IntegrationServiceProxy(String endpointConfigurationName)
            : base(endpointConfigurationName)
        {
        }

        /// <summary>
        /// Constructor with endpoint configuration name and remote address
        /// </summary>
        /// <param name="endpointConfigurationName">Indicates the endpoint configuration name</param>
        /// <param name="remoteAddress">Indicates the remote address</param>
        public IntegrationServiceProxy(String endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress)
            : base(endpointConfigurationName, remoteAddress)
        {
        }

        #endregion

        /* Note: Below are methods which has different name in the WCF contract so not coming up as part of Service Reference class
         * We need to explicitly divert call for all the mismatched method names.
         */

        #region IIntegrationService Members

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
        public JobCollection GetJobs(JobType jobType, JobSubType jobSubType, JobStatus jobStatus, Boolean skipJobDataLoading, MDMCenterApplication application)
        {
            return GetJobsWithAbilityToSkipJobDataLoading(jobType, jobSubType, jobStatus, skipJobDataLoading, application);
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
        public JobCollection GetJobs(JobType jobType, String sql, MDMCenterApplication application) 
        {
            return GetJobsWithSqlFilter(jobType, sql, application);
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
        public JobCollection GetJobsByType(JobType jobType, Boolean skipJobDataLoading, MDMCenterApplication application)
        { 
            return GetJobsByTypeWithAbilityToSkipJobDataLoading(jobType,skipJobDataLoading, application); 
        }

        /// <summary>
        /// Get all jobs in system by caller context
        /// </summary>
        /// <param name="jobFilterContext">Indicates context for job filter</param>
        /// <param name="callerContext">Indicates application and module name by which action is being performed</param>
        /// <returns>Returns collection of jobs in system by caller context</returns>
        public JobCollection GetJobs(JobFilterContext jobFilterContext, CallerContext callerContext) 
        {
            return GetFilteredJobs(jobFilterContext, callerContext);
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
            return StartImportJobWithCallerContext(file, profileName, profileId, parentJobId, jobType, callerContext);
        }

        /// <summary>
        /// Get import profile from the system based on given profile id
        /// </summary>
        /// <param name="profileId">Indicates identifier of expected import profile</param>
        /// <param name="callerContext">Indicates application and module name by which action is being performed</param>
        /// <returns>Returns import profile with given id</returns>
        public ImportProfile GetImportProfile(Int32 profileId, CallerContext callerContext) 
        {
            return GetImportProfileById(profileId, callerContext);
        }

        /// <summary>
        /// Get import profile from the system based on given profile name
        /// </summary>
        /// <param name="profileName">Indicates name of expected import profile</param>
        /// <param name="callerContext">Indicates application and module name by which action is being performed</param>
        /// <returns>Returns import profile with given name</returns>
        public ImportProfile GetImportProfile(String profileName, CallerContext callerContext) 
        {
            return GetImportProfileByName(profileName, callerContext);
        }

        /// <summary>
        /// Get import profile type
        /// </summary>
        /// <param name="profileId">Indicates import profile identifier</param>
        /// <param name="callerContext">Indicates application and module name by which action is being performed</param>
        /// <returns>Returns type of import profile of the operations</returns>
        public String GetProfileType(Int32 profileId, CallerContext callerContext) 
        {
            return GetImportProfileType(profileId, callerContext);
        }
        
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
            return CreateIntegrationActivityLogWithShortNames(mdmObjectId, mdmObjectTypeName, context, connectorShortName, integrationMessageTypeShortName,
                    integrationType, weightage, messsageCount, callerContext);
        }

        #endregion IIntegrationService Members

    }
}
