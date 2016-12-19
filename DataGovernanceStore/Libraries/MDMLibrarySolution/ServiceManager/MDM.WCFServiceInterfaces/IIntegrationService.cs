using System;
using System.Collections.ObjectModel;
using System.ServiceModel;


namespace MDM.WCFServiceInterfaces
{
    using MDM.BusinessObjects;
    using MDM.BusinessObjects.Imports;
    using MDM.BusinessObjects.Exports;
    using MDM.BusinessObjects.Jobs;
    using MDM.BusinessObjects.Integration;
    using MDM.Core;
    using MDM.Interfaces;
    using MDM.BusinessObjects.DataModel;

    /// <summary>
    /// Defines operation contracts for MDM Configuration related operations
    /// </summary>
    [ServiceContract(Namespace = "http://wcfservices.riversand.com")]
    public interface IIntegrationService
    {
        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        Job GetJob(Int32 jobId, JobType jobType, MDMCenterApplication application);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        JobCollection GetJobs(JobType jobType, JobSubType jobSubType, JobStatus jobStatus, MDMCenterApplication application);

        [OperationContract(Name = "GetJobsWithAbilityToSkipJobDataLoading")]
        [FaultContract(typeof(MDMExceptionDetails))]
        JobCollection GetJobs(JobType jobType, JobSubType jobSubType, JobStatus jobStatus, Boolean skipJobDataLoading, MDMCenterApplication application);

        [OperationContract(Name = "GetJobsWithSqlFilter")]
        [FaultContract(typeof(MDMExceptionDetails))]
        JobCollection GetJobs(JobType jobType, String sql, MDMCenterApplication application);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        JobCollection GetJobsByType(JobType jobType, MDMCenterApplication application);

        [OperationContract(Name = "GetJobsByTypeWithAbilityToSkipJobDataLoading")]
        [FaultContract(typeof(MDMExceptionDetails))]
        JobCollection GetJobsByType(JobType jobType, Boolean skipJobDataLoading, MDMCenterApplication application);

        [OperationContract(Name = "GetFilteredJobs")]
        [FaultContract(typeof(MDMExceptionDetails))]
        JobCollection GetJobs(JobFilterContext jobFilterContext, CallerContext callerContext);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        OperationResultCollection ProcessJobs(JobCollection jobsCollection, CallerContext callerContext);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        OperationResult CreateJob(Job job, CallerContext callerContext);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        OperationResult UpdateJob(Job job, CallerContext callerContext);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        OperationResult DeleteJob(Job job, CallerContext callerContext);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        Int32 InitiateImportJob(Job job, MDMCenterApplication application, MDMCenterModules module);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        Boolean UpdateImportJob(Job job, MDMCenterApplication application, MDMCenterModules module);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        JobImportResultCollection GetImportJobResults(Int32 importJobId, MDMCenterApplication application, MDMCenterModules module);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        Int32 StartImportJob(MDMCenterApplication application, File file, String profileName, Int32 profileId, Int32 parentJobId, JobType jobType);

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
        [OperationContract(Name = "StartImportJobWithCallerContext")]
        [FaultContract(typeof(MDMExceptionDetails))]
        Int32 StartImportJob(File file, String profileName, Int32 profileId, Int32 parentJobId, JobType jobType, CallerContext callerContext);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        JobImportResultCollection GetErrorStates(int jobId, MDMCenterApplication application);

        [OperationContract]
        [FaultContract(typeof (MDMExceptionDetails))]
        File GetErrorStatesAsExcelFile(Int32 jobId, LocaleEnum locale, CallerContext callerContext);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        DataModelOperationResultCollection GetDataModelImportJobResults(Int32 importJobId, ObjectType objectType, String externalId, CallerContext callerContext);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        DataModelOperationResultSummaryCollection GetDataModelOperationResultSummary(int jobId, CallerContext callerContext);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        ImportProfileCollection GetProfiles(MDMCenterApplication application);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        JobOperationStatus GetJobExecutionStatus(int jobId, MDMCenterApplication application, MDMCenterModules module);

        #region Import Profile CUD

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        OperationResult DeleteImportProfile(ImportProfile importProfile, CallerContext callerContext);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        OperationResult UpdateImportProfile(ImportProfile importProfile, CallerContext callerContext);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        OperationResult CreateImportProfile(ImportProfile importProfile, CallerContext callerContext);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        OperationResultCollection ProcessImportProfiles(ImportProfileCollection importProfiles, CallerContext callerContext);

        #endregion Import Profile CUD

        #region Import Profile Get

        [OperationContract(Name = "GetImportProfileById")]
        [FaultContract(typeof(MDMExceptionDetails))]
        ImportProfile GetImportProfile(Int32 profileId, CallerContext callerContext);

        [OperationContract(Name = "GetImportProfileByName")]
        [FaultContract(typeof(MDMExceptionDetails))]
        ImportProfile GetImportProfile(String profileName, CallerContext callerContext);

        [OperationContract(Name = "GetAllImportProfiles")]
        [FaultContract(typeof(MDMExceptionDetails))]
        ImportProfileCollection GetAllImportProfiles(CallerContext callerContext);

        [OperationContract(Name = "GetImportProfileType")]
        [FaultContract(typeof(MDMExceptionDetails))]
        String GetProfileType(Int32 profileId, CallerContext callerContext);

        #endregion Import Profile Get

        #region Lookup Import Profile Get

        [OperationContract(Name = "GetLookupImportProfileById")]
        [FaultContract(typeof(MDMExceptionDetails))]
        LookupImportProfile GetLookupImportProfileById(Int32 profileId, CallerContext iCallerContext);

        [OperationContract(Name = "GetLookupImportProfileByName")]
        [FaultContract(typeof(MDMExceptionDetails))]
        LookupImportProfile GetLookupImportProfileByName(String profileName, CallerContext iCallerContext);

        [OperationContract(Name = "GetAllLookupImportProfiles")]
        [FaultContract(typeof(MDMExceptionDetails))]
        LookupImportProfileCollection GetAllLookupImportProfiles(CallerContext iCallerContext);

        #endregion

        #region JobSchedule CUD

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        OperationResult CreateJobSchedule(JobSchedule jobSchedule, CallerContext callerContext);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        OperationResult UpdateJobSchedule(JobSchedule jobSchedule, CallerContext callerContext);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        OperationResult DeleteJobSchedule(JobSchedule jobSchedule, CallerContext callerContext);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        OperationResultCollection ProcessJobSchedules(JobScheduleCollection jobSchedules, CallerContext callerContext);

        #endregion JobSchedule CUD

        #region JobSchedule Get

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        JobScheduleCollection GetAllJobSchedules(CallerContext callerContext);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        JobSchedule GetJobScheduleById(Int32 jobScheduleId, CallerContext callerContext);

        #endregion JobSchedule Get

        #region IntegrationActivityLog Create Methods

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
        [OperationContract(Name = "CreateIntegrationActivityLogWithShortNames")]
        [FaultContract(typeof(MDMExceptionDetails))]
        OperationResult CreateIntegrationActivityLog(Int64 mdmObjectId, String mdmObjectTypeName, String context, String connectorShortName, String integrationMessageTypeShortName, IntegrationType integrationType, Int32 weightage, Int32 messsageCount, CallerContext callerContext);

        /// <summary>
        /// Create a integration activity log
        /// </summary>
        /// <param name="integrationActivityLog">Integration activity log to be created</param>
        /// <param name="callerContext">Indicates the context of caller making call to this API</param>
        /// <returns>Status indicating the result of operation</returns>
        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        OperationResult CreateIntegrationActivityLog(IntegrationActivityLog integrationActivityLog, CallerContext callerContext);

        #endregion  IntegrationActivityLog Create Methods

        #region ConnectorProfile CUD

        /// <summary>
        /// Create a ConnectorProfile
        /// </summary>
        /// <param name="connectorProfile">ConnectorProfile to be created</param>
        /// <param name="callerContext">Indicates the context of caller making call to this API</param>
        /// <returns>Status indicating the result of operation</returns>
        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        OperationResult CreateConnectorProfile(ConnectorProfile connectorProfile, CallerContext callerContext);

        /// <summary>
        /// Update a ConnectorProfile
        /// </summary>
        /// <param name="connectorProfile">ConnectorProfile to be updated</param>
        /// <param name="callerContext">Indicates the context of caller making call to this API</param>
        /// <returns>Status indicating the result of operation</returns>
        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        OperationResult UpdateConnectorProfile(ConnectorProfile connectorProfile, CallerContext callerContext);

        /// <summary>
        /// Delete a ConnectorProfile
        /// </summary>
        /// <param name="connectorProfile">ConnectorProfile to be deleted</param>
        /// <param name="callerContext">Indicates the context of caller making call to this API</param>
        /// <returns>Status indicating the result of operation</returns>
        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        OperationResult DeleteConnectorProfile(ConnectorProfile connectorProfile, CallerContext callerContext);

        #endregion ConnectorProfile CUD

        #region Get ConnectorProfile

        /// <summary>
        /// Get all ConnectorProfile in the system
        /// </summary>
        /// <param name="callerContext">Indicates the context of caller making call to this API</param>
        /// <returns>ConnectorProfile object collection</returns>
        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        ConnectorProfileCollection GetAllConnectorProfiles(CallerContext callerContext);

        /// <summary>
        /// Get ConnectorProfile by Id
        /// </summary>
        /// <param name="connectorProfileId">Id of connector for which info needs to be fetched</param>
        /// <param name="callerContext">Indicates the context of caller making call to this API</param>
        /// <returns>ConnectorProfile object collection</returns>
        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        ConnectorProfile GetConnectorProfileById(Int16 connectorProfileId, CallerContext callerContext);

        /// <summary>
        /// Get ConnectorProfile by ShortName
        /// </summary>
        /// <param name="connectorProfileShortName">ShortName of connector for which info needs to be fetched</param>
        /// <param name="callerContext">Indicates the context of caller making call to this API</param>
        /// <returns>ConnectorProfile object collection</returns>
        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        ConnectorProfile GetConnectorProfileByName(String connectorProfileShortName, CallerContext callerContext);

        #endregion Get ConnectorProfile

        #region IntegrationMessage Get methods

        /// <summary>
        /// Get integration message by id
        /// </summary>
        /// <param name="integrationMessageId">integration message id to fetch the value for</param>
        /// <returns>integration message object</returns>
        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        IntegrationMessage GetIntegrationMessageById(Int64 integrationMessageId, CallerContext callerContext);

        #endregion IntegrationMessage Get methods

        #region Get IntegrationMessageType

        /// <summary>
        /// Get all IntegrationMessageType in the system
        /// </summary>
        /// <param name="callerContext">Indicates the context of caller making call to this API</param>
        /// <returns>IntegrationMessageType object collection</returns>
        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        IntegrationMessageTypeCollection GetAllIntegrationMessageTypes(CallerContext callerContext);

        /// <summary>
        /// Get IntegrationMessageType by Id
        /// </summary>
        /// <param name="integrationMessageTypeId">Id of integrationMessageType for which info needs to be fetched</param>
        /// <param name="callerContext">Indicates the context of caller making call to this API</param>
        /// <returns>IntegrationMessageType object collection</returns>
        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        IntegrationMessageType GetIntegrationMessageTypeById(Int16 integrationMessageTypeId, CallerContext callerContext);

        /// <summary>
        /// Get IntegrationMessageType by ShortName
        /// </summary>
        /// <param name="integrationMessageTypeShortName">ShortName of integrationMessageType for which info needs to be fetched</param>
        /// <param name="callerContext">Indicates the context of caller making call to this API</param>
        /// <returns>IntegrationMessageType object collection</returns>
        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        IntegrationMessageType GetIntegrationMessageTypeByName(String integrationMessageTypeShortName, CallerContext callerContext);

        #endregion Get IntegrationMessageType

        #region Get MDMObjectType

        /// <summary>
        /// Get all MDMObjectType in the system
        /// </summary>
        /// <param name="callerContext">Indicates the context of caller making call to this API</param>
        /// <returns>MDMObjectType object collection</returns>
        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        MDMObjectTypeCollection GetAllMDMObjectTypes(CallerContext callerContext);

        /// <summary>
        /// Get MDMObjectType by Id
        /// </summary>
        /// <param name="mdmObjectTypeId">Id of MDMObjectType for which info needs to be fetched</param>
        /// <param name="callerContext">Indicates the context of caller making call to this API</param>
        /// <returns>MDMObjectType object collection</returns>
        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        MDMObjectType GetMDMObjectTypeById(Int16 mdmObjectTypeId, CallerContext callerContext);

        /// <summary>
        /// Get MDMObjectType by ShortName
        /// </summary>
        /// <param name="mdmObjectTypeShortName">ShortName of MDMObjectType for which info needs to be fetched</param>
        /// <param name="callerContext">Indicates the context of caller making call to this API</param>
        /// <returns>MDMObjectType object collection</returns>
        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        MDMObjectType GetMDMObjectTypeByName(String mdmObjectTypeShortName, CallerContext callerContext);

        #endregion Get MDMObjectType

        #region Update IntegrationItem Status

        /// <summary>
        /// Update integration item status. Update item and dimension status for given Id/Type
        /// </summary>
        /// <param name="integrationItemStatus">Status for an item. It contains item information and status information</param>
        /// <param name="callerContext">Context of API making call to this API.</param>
        /// <returns>Result of operation.</returns>
        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        OperationResult UpdateIntegrationItemStatus(IntegrationItemStatus integrationItemStatus, CallerContext callerContext);

        #endregion Update IntegrationItem Status

        #region Search IntegrationItem Status

        /// <summary>
        /// Search for Integration item status based on given criteria
        /// </summary>
        /// <param name="integrationItemStatusSearchCriteria">Contains search criteria for IntegrationItemStatus</param>
        /// <param name="callerContext">Indicates context of caller making call to this API</param>
        /// <returns><see cref="IntegrationItemStatusInternalCollection"/> which are matching given search criteria</returns>
        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        IntegrationItemStatusInternalCollection SearchIntegrationItemStatus(IntegrationItemStatusSearchCriteria integrationItemStatusSearchCriteria, CallerContext callerContext);

        #endregion Search IntegrationItem Status

        #region Get IntegrationItemDimensionType

        /// <summary>
        /// Get all IntegrationItemDimensionTypes in the system
        /// </summary>
        /// <param name="callerContext">Indicates the context of caller making call to this API</param>
        /// <returns>MDMObjectType object collection</returns>
        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        IntegrationItemDimensionTypeCollection GetAllIntegrationItemDimensionTypes(CallerContext callerContext);

        /// <summary>
        /// Get IntegrationItemDimensionType by Id
        /// </summary>
        /// <param name="integrationItemDimensionTypeId">Id of IntegrationItemDimensionType for which info needs to be fetched</param>
        /// <param name="callerContext">Indicates the context of caller making call to this API</param>
        /// <returns>MDMObjectType object collection</returns>
        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        IntegrationItemDimensionType GetIntegrationItemDimensionTypeById(Int16 integrationItemDimensionTypeId, CallerContext callerContext);

        /// <summary>
        /// Get IntegrationItemDimensionType by ShortName
        /// </summary>
        /// <param name="integrationItemDimensionTypeShortName">ShortName of IntegrationItemDimensionType for which info needs to be fetched</param>
        /// <param name="callerContext">Indicates the context of caller making call to this API</param>
        /// <returns>MDMObjectType object collection</returns>
        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        IntegrationItemDimensionType GetIntegrationItemDimensionTypeByName(String integrationItemDimensionTypeShortName, CallerContext callerContext);

        /// <summary>
        /// Gets the integration item dimension types by connector identifier.
        /// </summary>
        /// <param name="connectorId">The connector identifier.</param>
        /// <param name="callerContext">The caller context.</param>
        /// <returns>Collection of integration item dimension types for the specified connector</returns>
        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        IntegrationItemDimensionTypeCollection GetIntegrationItemDimensionTypesByConnectorId(Int16 connectorId, CallerContext callerContext);

        #endregion Get MDMObjectType
    }
}