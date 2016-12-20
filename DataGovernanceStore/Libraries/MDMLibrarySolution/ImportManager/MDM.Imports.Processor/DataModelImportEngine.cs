using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace MDM.Imports.Processor
{
    using MDM.AdminManager.Business;
    using MDM.AttributeModelManager.Business;
    using MDM.BusinessObjects;
    using MDM.BusinessObjects.DataModel;
    using MDM.BusinessObjects.Imports;
    using MDM.BusinessObjects.Jobs;
    using MDM.CategoryManager.Business;
    using MDM.ContainerManager.Business;
    using MDM.Core;
    using MDM.Core.DataModel;
    using MDM.Core.Exceptions;
    using MDM.DataModelManager.Business;
    using MDM.EntityManager.Business;
    using MDM.ExceptionManager.Handlers;
    using MDM.HierarchyManager.Business;
    using MDM.Imports.Interfaces;
    using MDM.Interfaces;
    using MDM.JobManager.Business;
    using MDM.OrganizationManager.Business;
    using MDM.ParallelizationManager.Processors;
    using MDM.RelationshipManager.Business;
    using MDM.Utility;
    using MDM.LookupManager.Business;

    /// <summary>
    /// Specifies the DataModel Import Engine class
    /// </summary>
    public class DataModelImportEngine : IDataModelImportEngine
    {
        #region Fields

        #region Logger

        public static EventLogHandler LogHandler = null;

        #endregion

        #region Timer variables

        private double totalTime = 0;

        #endregion

        #region Job objects

        private Job _job = null;

        private JobBL _jobManager = new JobBL();

        private DataModelImportProfile _dataModelImportProfile = null;

        private IDataModelImportSourceData _sourceData = null;

        private JobImportResultHandler _jobImportResultHandler = null;

        private static Object _lockObject = new Object();

        #endregion

        #region Other local Variables and object

        private String importProgram = "DataModelImportEngine";
        private IImportProgressHandler _progressHandler = new ImportProgressHandler();
        private MDMCenterApplication _application = MDMCenterApplication.JobService;
        JobImportResultBL _jobImportResultBL = null;
        private ConcurrentDictionary<ObjectType, DataModelOperationResultSummary> _dataModelSummaryDictionary = new ConcurrentDictionary<ObjectType, DataModelOperationResultSummary>();

        #endregion

        #endregion Private Fields

        #region Constructors

        /// <summary>
        /// Parameter less constructor.
        /// </summary>
        public DataModelImportEngine()
        {
        }

        #endregion

        #region Properties
        #endregion

        #region Methods

        #region Public Methods

        #region IImportEngine Members

        /// <summary>
        /// Initialize Job
        /// </summary>
        /// <param name="job"></param>
        /// <param name="dataModelImportProfile"></param>
        /// <returns></returns>
        public Boolean Initialize(Job job, DataModelImportProfile dataModelImportProfile)
        {
            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StartTraceActivity("DataModelImportEngine.Initialize", MDMTraceSource.DataModelImport, true);

            Boolean successFlag = true;

            if (job == null)
            {
                throw new ArgumentNullException("Job");
            }

            if (dataModelImportProfile == null)
            {
                throw new ArgumentNullException("DataModelImportProfile");
            }

            this._job = job;

            this._dataModelImportProfile = dataModelImportProfile;

            CheckAndCreateEventLogHandler();

            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StopTraceActivity("DataModelImportEngine.Initialize", MDMTraceSource.DataModelImport);

            _jobImportResultBL = new JobImportResultBL();

            return successFlag;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="stepName"></param>
        /// <param name="stepConfiguration"></param>
        /// <param name="source"></param>
        /// <returns></returns>
        public Boolean RunStep(String stepName, StepConfiguration stepConfiguration, IDataModelImportSourceData source)
        {
            if (Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.StartTraceActivity("DataModelImportEngine.RunStep", MDMTraceSource.DataModelImport, true);
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "DataModelImportEngine.RunStep starting for JobId: " + _job.Id + " Step: " + stepName, MDMTraceSource.DataModelImport);
            }

            EventLogHandler elog = new EventLogHandler();
            elog.WriteInformationLog("DataModelImportEngine.RunStep", 100);

            Boolean successFlag = true;

            IDataModelImportSourceData sourceData = (IDataModelImportSourceData)source;

            #region Parameter validation

            if (stepConfiguration == null)
            {
                throw new ArgumentNullException(String.Format("DataModelImportEngine: JobId: {0], RunStep: {1}. Error: stepConfiguration is null", _job.Id, stepName));
            }

            if (sourceData == null)
            {
                throw new ArgumentNullException(String.Format("DataModelImportEngine: JobId: {0], RunStep: {1}. Error: sourceData is null", _job.Id, stepName));
            }

            #endregion

            #region Set instance variables

            this._sourceData = sourceData;

            this._jobImportResultHandler = new JobImportResultHandler(_job.Id);

            _jobImportResultHandler.ProgramName = importProgram;
            _jobImportResultHandler.UserName = _job.CreatedUser;
            _jobImportResultHandler.ProgressHandler =_progressHandler;

            #endregion

            #region Initialize Parameters

            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Initialize Parameters starting... ", MDMTraceSource.DataModelImport);

            // Initialize local configuration from app config file
            InitializeThreadCounts();

            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Initialize Parameters complete", MDMTraceSource.DataModelImport);

            #endregion

            #region Process

            Int64 startTime = DateTime.Now.Ticks;

            String startMessage = String.Format("DataModel import engine started process for job {0}.", _job.Id.ToString());

            LogHandler.WriteInformationLog(startMessage, 0);

            Process();

            Int64 endWorkTime = DateTime.Now.Ticks;
            totalTime = new TimeSpan(endWorkTime - startTime).TotalSeconds;

            #endregion

            //TODO: Figure out how do we record import results

            //only when the job is running, we will make it complete... all other will stay as it is
            if (_job.JobStatus == JobStatus.Running)
            {
                _job.JobStatus = Helpers.GetJobStatus(_job.JobData.ExecutionStatus.TotalElementsSucceed, _job.JobData.ExecutionStatus.TotalElementsFailed, _job.JobData.ExecutionStatus.TotalElementsPartiallySucceed);

                UpdateJobStatus(_job.JobStatus, "DataModel import job completed.");

				String endMessage = String.Format("DataModel import engine completed processing for job {0}. The process took {1} seconds.", _job.Id.ToString(), totalTime);

				LogHandler.WriteInformationLog(endMessage, 0);
			}

            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StopTraceActivity("DataModelImportEngine.RunStep", MDMTraceSource.DataModelImport);

            return successFlag;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Job GetCurrentJob()
        {
            return _job;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public DataModelImportProfile GetCurrentImportProfile()
        {
            return _dataModelImportProfile;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ExecutionStatus GetExecutionStatus()
        {
            return _job.JobData.ExecutionStatus;
        }

        #endregion

        #endregion

        #region Private Methods

        #region Setup and Config Methods

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private Boolean CheckAndCreateEventLogHandler()
        {
            Boolean result = true;

            try
            {
                LogHandler = new EventLogHandler();
            }
            catch (Exception e)
            {
                result = false;
                System.Diagnostics.EventLog.WriteEntry("Riversand MDMCenter Job Service Import Process CheckAndCreateEventLogHandler Failed : ", e.Message + "\n" + e.StackTrace);
            }
            return result;
        }

        /// <summary>
        /// Splits the given range of dataModels across the available number of dataModel threads. Any spill over is assigned to the last thread.
        /// </summary>
        /// <param name="numberOfThreads"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="dataModelbatchPerThread"></param>
        /// <returns>Dictionary containing the start number for each thread. The end number is the start number of the next thread. So the dictionary
        /// will have one more entry than the number of threads</returns>
        private Dictionary<Int32, Int32> SplitDataModelsForThreads(Int32 numberOfThreads, Int32 start, Int32 end, Int32 dataModelbatchPerThread)
        {
            Dictionary<Int32, Int32> threadBatchStart = new Dictionary<Int32, Int32>(numberOfThreads + 1);

            for (Int32 i = 0; i < numberOfThreads; i++)
            {
                threadBatchStart[i] = start + i * dataModelbatchPerThread;
            }
            // the last thread gets its batch and the spill over..
            threadBatchStart[numberOfThreads] = end + 1;

            return threadBatchStart;
        }

        /// <summary>
        /// Initializes the processing thread counts.
        /// If the settings are available in the profile then get it from the profile, otherwise use the default value.
        /// </summary>
        private void InitializeThreadCounts()
        {
            return; // Single thread support only.
        }

        #endregion

        #region Step 1: Import Engine Process Method

        /// <summary>
        /// Gets source data from the data provider and processes dataModel sheets
        /// </summary>
        private void Process()
        {
            if (Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.StartTraceActivity("DataModelImportEngine.Process", MDMTraceSource.DataModelImport, false);
            }

            _jobImportResultHandler.AuditRefId = -1;

            CallerContext callerConext = new CallerContext(MDMCenterApplication.PIM, MDMCenterModules.DataModelImport, "DataModelImportEngine", _job.Id, _job.ProfileId, _job.ProfileName);
            callerConext.AdditionalProperties = new Dictionary<String, Object>();
            callerConext.AdditionalProperties.Add("UserName", _job.CreatedUser);

            try
            {
                Collection<ObjectType> dataModelObjectTypes = _sourceData.GetDataModelObjectTypesForImport(callerConext);

                // TODO order of data model object types
                foreach (ObjectType dataModelObjectType in dataModelObjectTypes)
                {
                    IDataModelObjectCollection dataModelObjects  = null;
                    IDataModelManager iDataModelManager = GetDataModelManager(dataModelObjectType);
                    Int16 currentBatch = 0;
                    Int16 lastBatch  = 1;

                    DataModelOperationResultSummary summary = new DataModelOperationResultSummary()
                    {
                        ObjectType = dataModelObjectType.ToString(),
                        SummaryObjectName = DataModelDictionary.ObjectsDictionary[dataModelObjectType],
                        ExternalId = "DataModelImportSummary",
                        InternalId = 0
                    };

                    _dataModelSummaryDictionary.TryAdd(dataModelObjectType, summary);

                    do
                    {
                        currentBatch++;

                        //Get data based on level.
                        dataModelObjects = _sourceData.GetAllDataModelObjects(dataModelObjectType, currentBatch, callerConext);
                        
                        UpdateOperationResultSummary(dataModelObjectType, dataModelObjects, null, currentBatch == 1 ? ObjectAction.Create : ObjectAction.Update);
                        
                        if (dataModelObjects != null)
                        {
                            ProcessInParallel(dataModelObjectType, iDataModelManager, dataModelObjects, callerConext);
                        }

                        //Get max level
                        lastBatch = _sourceData.GetBatchCount(dataModelObjectType, callerConext);

                    } while (lastBatch != currentBatch);
                }
            }
            catch (AggregateException ex)
            {
                if (ex.InnerException != null)
                {
                    throw ex.InnerException;
                }
            }
            catch
            {
                throw;
            }

            finally
            {
                #region Record Results
                #endregion

                if (Constants.TRACING_ENABLED)
                {
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "DataModelImportEngine.Process: Completed Process DataModelData call... ", MDMTraceSource.DataModelImport);
                }
            }
        }

        /// <summary>
        /// GetDataModelManager
        /// </summary>
        /// <param name="dataModelObjectType">IDataModelManager</param>
        /// <returns></returns>
        private IDataModelManager GetDataModelManager(ObjectType dataModelObjectType)
        {
            switch (dataModelObjectType)
            {
                case ObjectType.Organization:
                    return new OrganizationBL();

                case ObjectType.Taxonomy:
                    return new HierarchyBL();

                case ObjectType.Catalog:
                    return new ContainerBL();

                case ObjectType.ContainerLocalization:
                    return new ContainerLocaleMappingBL(new ContainerBL());

                case ObjectType.EntityType:
                    return new EntityTypeBL();

                case ObjectType.RelationshipType:
                    return new RelationshipTypeBL();

                case ObjectType.AttributeModel:
                case ObjectType.AttributeModelLocalization:
                    return new AttributeModelBL(new DataModelValidationOptions(_sourceData.GetConfigurationItems()), new LookupBL());

                case ObjectType.Category:
                    return new CategoryBL(new EntityBL(), new EntityLocaleBL());

                case ObjectType.CategoryLocalization:
                    return new EntityLocaleBL();

                case ObjectType.ContainerEntityTypeMapping:
                    return new ContainerEntityTypeMappingBL();

                case ObjectType.EntityTypeAttributeMapping:
                    return new EntityTypeAttributeMappingBL(new EntityTypeBL(), new AttributeModelBL(), new ContainerBL());

                case ObjectType.ContainerEntityTypeAttributeMapping:
                    return new ContainerEntityTypeAttributeMappingBL(new OrganizationBL(), new ContainerBL(), new EntityTypeBL(), new AttributeModelBL());

                case ObjectType.CategoryAttributeMapping:
                    return new CategoryAttributeMappingBL(new HierarchyBL(), new CategoryBL(), new AttributeModelBL(), new EntityBL());

                case ObjectType.RelationshipTypeEntityTypeMapping:
                    return new RelationshipTypeEntityTypeMappingBL(new EntityTypeBL(), new RelationshipTypeBL());

                case ObjectType.RelationshipTypeEntityTypeMappingCardinality:
                    return new RelationshipTypeEntityTypeMappingCardinalityBL(new EntityTypeBL(), new RelationshipTypeBL(), new ContainerBL());

                case ObjectType.ContainerRelationshipTypeEntityTypeMapping:
                    return new ContainerRelationshipTypeEntityTypeMappingBL(new OrganizationBL(), new ContainerBL(), new EntityTypeBL(), new RelationshipTypeBL());

                case ObjectType.ContainerRelationshipTypeEntityTypeMappingCardinality:
                    return new ContainerRelationshipTypeEntityTypeMappingCardinalityBL(new OrganizationBL(), new ContainerBL(), new EntityTypeBL(), new RelationshipTypeBL());

                case ObjectType.RelationshipTypeAttributeMapping:
                    return new RelationshipTypeAttributeMappingBL(new RelationshipTypeBL(), new AttributeModelBL(), new ContainerBL());

                case ObjectType.ContainerRelationshipTypeAttributeMapping:
                    return new ContainerRelationshipTypeAttributeMappingBL(new OrganizationBL(), new ContainerBL(), new RelationshipTypeBL(), new AttributeModelBL());

                case ObjectType.Role:
                    return new SecurityRoleBL();

                case ObjectType.User:
                    return new SecurityUserBL(new OrganizationBL(), new ContainerBL(), new HierarchyBL());

                case ObjectType.LookupModel:
                    return DynamicTableSchemaBL.GetSingleton();

                case ObjectType.EntityVariantDefinition:
                    return new EntityVariantDefinitionBL(new AttributeModelBL());

                case ObjectType.EntityVariantDefinitionMapping:
                    return new EntityVariantDefinitionMappingBL(new ContainerBL(), new CategoryBL());
            }

            throw new NotImplementedException(String.Format("GetDataModelManager: ObjectType [{0}] not implemented yet", dataModelObjectType.ToString()));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataModelObjectType"></param>
        /// <param name="dataModelManager"></param>
        /// <param name="dataModelObjects"></param>
        /// <param name="callerContext"></param>
        private void Process(ObjectType dataModelObjectType, IDataModelManager dataModelManager, IDataModelObjectCollection dataModelObjects, ICallerContext callerContext)
        {
            if (Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("DataModelImportEngine.Process Started for [{0}]", dataModelObjectType.ToString()), MDMTraceSource.DataModelImport);
            }

            var operationResults = DataModelProcessOrchestrator.Validate(dataModelManager, dataModelObjects, callerContext);

            if (operationResults != null 
                    && operationResults.OperationResultStatus != OperationResultStatusEnum.Failed
                    && dataModelObjects != null 
                    && dataModelObjects.Count > 0
                    && _dataModelImportProfile.DataModelJobProcessingOptions.DataModelImportProcessingType == ImportProcessingType.ValidateAndProcess)
            {
                operationResults = DataModelProcessOrchestrator.Process(dataModelManager, dataModelObjects, operationResults, callerContext);
            }

            MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("DataModelImportEngine.Process Completed for [{0}]", dataModelObjectType.ToString()), MDMTraceSource.DataModelImport);

            if (operationResults == null)
            {
                String message = String.Format("DataModelImportEngine.Process for [{0}] didn't return operationResult", dataModelObjectType.ToString());

                throw new MDMOperationException("", message, "DataModelImportEngine", String.Empty, "Process");
            }
            else
            {
                PopulateImportResult(dataModelObjectType, dataModelObjects, operationResults);
            }
        }

        private void ProcessInParallel(ObjectType dataModelObjectType, IDataModelManager dataModelManager, IDataModelObjectCollection dataModelObjects, ICallerContext callerContext)
        {
            Int32 countOfDataModelObjectsToProcess = dataModelObjects.Count;
            Int32 batchSize = _dataModelImportProfile.DataModelJobProcessingOptions.BatchSize;

            if (countOfDataModelObjectsToProcess < batchSize)
            {
                Process(dataModelObjectType, dataModelManager, dataModelObjects, callerContext);
            }
            else
            {
                Int32 threadPoolSize = _dataModelImportProfile.DataModelJobProcessingOptions.NumberOfThreads;
                Collection<Dictionary<String, Object>> threadContexts = new Collection<Dictionary<String, Object>>();
                Collection<IDataModelObjectCollection> dataModelObjectsInBatch = dataModelObjects.Split(batchSize);

                //Start Batching
                foreach (IDataModelObjectCollection obj in dataModelObjectsInBatch)
                {
                    Dictionary<String, Object> threadContext = new Dictionary<String, Object>();
                    threadContext.Add("DataModelObjectType", dataModelObjectType);
                    threadContext.Add("DataModelManager", dataModelManager);
                    threadContext.Add("DataModelObjects", obj);
                    threadContext.Add("CallerContext", callerContext);
                    threadContexts.Add(threadContext);
                }

                var cancellationTokenSource = new CancellationTokenSource();
                var result = new ParallelTaskProcessor().RunInParallel<Dictionary<String, Object>>(threadContexts, ProcessParallelThread, cancellationTokenSource, threadPoolSize);
            }
        }

        private void ProcessParallelThread(Dictionary<String, Object> threadContext)
        {
            ObjectType dataModelObjectType = (ObjectType)threadContext["DataModelObjectType"];
            IDataModelManager dataModelManager = (IDataModelManager)threadContext["DataModelManager"];
            IDataModelObjectCollection dataModelObjects = (IDataModelObjectCollection)threadContext["DataModelObjects"]; ;
            ICallerContext callerContext = (ICallerContext)threadContext["CallerContext"];

            Process(dataModelObjectType, dataModelManager, dataModelObjects, callerContext);
        }

        #endregion

        #region Logging and Error Handling

        /// <summary>
        /// Update the job status.
        /// </summary>
        /// <param name="jobStatus"></param>
	    /// <param name="description"></param>
        private void UpdateJobStatus(JobStatus jobStatus, String description)
        {
            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("Updating job status for Job: {0} Status: {1} - {2}", _job.Id, jobStatus, description), MDMTraceSource.DataModelImport);

            lock (_lockObject)
            {
                DateTime endTime = DateTime.Now;
                _job.JobData.ExecutionStatus.EndTime = endTime.ToString();
                DateTime startTime = DateTime.Now;
                ExecutionStatus executionStatus = _job.JobData.ExecutionStatus;

                if (DateTime.TryParse(executionStatus.StartTime, out startTime))
                {
                    executionStatus.TotalMilliSeconds = (endTime - startTime).TotalMilliseconds;
                }

                executionStatus.CurrentStatusMessage = _job.JobStatus.ToString();

                if (String.IsNullOrWhiteSpace(executionStatus.StartTime))
                {
                    executionStatus.StartTime = startTime.ToString();
                }

                #region Prepare job description

                String jobDescription = String.Empty;

                if (executionStatus.TotalElementsToProcess == 0 && executionStatus.TotalElementsProcessed == 0)
                {
                    jobDescription = "Processed job returned with no data. Check the input and retry again.";
                }
                else
                {
                    jobDescription = String.Format("DataModelImport - Total-{0}; Processed-{1};  Success-{2}; CompletedWithWarnings-{3}; Failed-{4};",
                                                                         executionStatus.TotalElementsToProcess,
                                                                         executionStatus.TotalElementsProcessed,
                                                                         executionStatus.TotalElementsSucceed,
                                                                         executionStatus.TotalElementsPartiallySucceed,
                                                                         executionStatus.TotalElementsFailed);
                }

                switch (jobStatus)
                {
                    case JobStatus.Completed:
                    case JobStatus.CompletedWithErrors:
                    case JobStatus.CompletedWithWarningsAndErrors:
                    case JobStatus.CompletedWithWarnings:
                        _job.Description = jobDescription;
                        break;
                    case JobStatus.Aborted:
                        _job.Description = "Job has been Aborted. Please check job details for more information.";
                        break;
                    default:
                        _job.Description = String.Format("{0} {1}", jobDescription, description);
                        break;
                }

                #endregion
            }

            _jobManager.Update(_job, new CallerContext(_application, Utility.GetModule(_job.JobType), importProgram));
        }

        /// <summary>
        /// Populate import result table.
        /// </summary>
        private void PopulateImportResult(ObjectType dataModelObjectType, IDataModelObjectCollection datamodelObjects, IDataModelOperationResultCollection operationResultCollection)
        {
            // Log Job level Error
            UpdateOperationResult(dataModelObjectType, datamodelObjects, operationResultCollection);

            // Update summary of ORs
            UpdateOperationResultSummary(dataModelObjectType, datamodelObjects, operationResultCollection, ObjectAction.Update);

            // Log Success ORs
            LogSuccess(dataModelObjectType, operationResultCollection);

            
            // Log Error ORs
            LogErrors(dataModelObjectType, operationResultCollection);
        }

        #region Job Notification

        /// <summary>
        /// Log successful entities if required.
        /// </summary>
	    /// <param name="dataModelObjectType"></param>
        /// <param name="objOperationResultCollection"></param>
        private void LogSuccess(ObjectType dataModelObjectType, IDataModelOperationResultCollection objOperationResultCollection)
        {

            // Get entity list with create action
            List<DataModelOperationResult> objOperationList = (from operationResult in objOperationResultCollection
                                                      where (operationResult.OperationResultStatus == OperationResultStatusEnum.Successful ||
                                                             operationResult.OperationResultStatus == OperationResultStatusEnum.None)
                                                      select operationResult).ToList();
            // do we have anything to process?
            if (objOperationList.Count == 0)
                return;

            DataModelOperationResultCollection objSuccessCollection = new DataModelOperationResultCollection();

            foreach (DataModelOperationResult item in objOperationList)
            {
                item.DataModelObjectType = dataModelObjectType;

                if (item.OperationResultStatus == OperationResultStatusEnum.Successful)
                {
                    item.Informations.Add(new Information("", String.Format("Object modifed. Action Performed [{0}].", item.PerformedAction.ToString()), new Collection<object> { item.Id })); //saved successfully
                }
                else
                {
                    item.Informations.Add(new Information("", "Object was not modified", new Collection<object> { item.Id })); //saved successfully
                }

                objSuccessCollection.Add(item);
            }

            // update the job import result
            _jobImportResultHandler.Save(objSuccessCollection, true);
        }

        
        /// <summary>
        /// Update the soure with the error message for a given entity. This will also REMOVE the errored items from the entity collection from further processing.
        /// </summary>
	    /// <param name="dataModelObjectType"></param>
        /// <param name="objOperationResultCollection"></param>
        private void LogErrors(ObjectType dataModelObjectType, IDataModelOperationResultCollection objOperationResultCollection)
        {
            // Get entity operation list with failed status
            List<DataModelOperationResult> objOperationList = (from operationResult in objOperationResultCollection
                                                      where operationResult.OperationResultStatus == OperationResultStatusEnum.Failed
                                                      || operationResult.OperationResultStatus == OperationResultStatusEnum.CompletedWithWarnings
                                                      || operationResult.OperationResultStatus == OperationResultStatusEnum.CompletedWithErrors
                                                      select operationResult).ToList();

            DataModelOperationResultCollection objErrorCollection = new DataModelOperationResultCollection();

            
            if ((objOperationList != null) && (objOperationList.Count() > 0))
            {
                foreach (DataModelOperationResult item in objOperationList)
                {
                    item.DataModelObjectType = dataModelObjectType;
                    objErrorCollection.Add(item);
                }
            }
            
            _jobImportResultHandler.Save(objErrorCollection, true);
        }
        
        /// <summary>
        /// Update job level Operation Result
        /// </summary>
        /// <param name="dataModelObjectType"></param>
        /// <param name="datamodelObjects"></param>
	    /// <param name="operationResultCollection"></param>
        private void UpdateOperationResult(ObjectType dataModelObjectType, IDataModelObjectCollection datamodelObjects, IDataModelOperationResultCollection operationResultCollection)
        {
            if (operationResultCollection == null || operationResultCollection.Count() == 0)
            {
                return;
            }

            foreach (IError error in operationResultCollection.GetErrors())
            {
                _job.JobData.OperationResult.Errors.Add(new Error(error.ErrorCode, error.ErrorMessage));
            }
        }

        /// <summary>
        /// Update OperationResult Summary
        /// </summary>
        /// <param name="dataModelObjectType"></param>
        /// <param name="datamodelObjects"></param>
        /// <param name="operationResultCollection"></param>
        /// <param name="jobResultSummaryAction">Create or Update</param>
        private void UpdateOperationResultSummary(ObjectType dataModelObjectType, IDataModelObjectCollection datamodelObjects, IDataModelOperationResultCollection operationResultCollection, ObjectAction jobResultSummaryAction)
        {
            DataModelOperationResultSummary operationResultSummary;
            
            if (!_dataModelSummaryDictionary.TryGetValue(dataModelObjectType, out operationResultSummary))
            {
                String errorMessage = String.Format("DataModelImportEngine.LogSummary DataModelSummaryDictionary miss for object type:[{0}]" + dataModelObjectType.ToString());
                throw new MDMOperationException("", errorMessage, "DataModelImportEngine", String.Empty, "LogSummary");
            }

            Int32 pending = 0;
            Int32 succeeded = 0;
            Int32 failed = 0;
            Int32 completedWithWarnings = 0;
            Int32 completedWithErrors = 0;
            Int32 processed = 0;
            Int32 total = 0;

            // Update current ObjectType's current batch counts
            if (operationResultCollection == null)
            {
                if (datamodelObjects != null)
                {
                    total += datamodelObjects.Count; // Before starting the process ORCollection is null - just update total objects to process
                }
            }
            else
            {
                total = 0;
                pending = operationResultCollection.Count(i => (i.OperationResultStatus == OperationResultStatusEnum.Pending));
                succeeded = operationResultCollection.Count(i => (i.OperationResultStatus == OperationResultStatusEnum.None || i.OperationResultStatus == OperationResultStatusEnum.Successful));
                failed = operationResultCollection.Count(i => (i.OperationResultStatus == OperationResultStatusEnum.Failed));
                completedWithWarnings = operationResultCollection.Count(i => i.OperationResultStatus == OperationResultStatusEnum.CompletedWithWarnings);
                completedWithErrors = operationResultCollection.Count(i => i.OperationResultStatus == OperationResultStatusEnum.CompletedWithErrors);
                processed = succeeded + failed + completedWithErrors + completedWithWarnings;
            }

            // Update Job wide execution numbers
            _job.JobData.ExecutionStatus.TotalElementsToProcess += total;
            _job.JobData.ExecutionStatus.TotalElementsProcessed += processed;
            _job.JobData.ExecutionStatus.TotalElementsFailed += failed;
            _job.JobData.ExecutionStatus.TotalElementsSucceed += succeeded;
            _job.JobData.ExecutionStatus.TotalElementsPartiallySucceed += completedWithWarnings;

            // Update overall job status with running count.
            UpdateJobStatus(_job.JobStatus, String.Format("Processing {0};", DataModelDictionary.ObjectsDictionary[dataModelObjectType]));

            // Update Data Model 
            operationResultSummary.UpdateSummaryCounts(total, pending, succeeded, failed, completedWithWarnings, completedWithErrors);
            operationResultSummary.UpdateSummaryStatus();

            _jobImportResultHandler.Save(new DataModelOperationResultSummaryCollection() { operationResultSummary }, jobResultSummaryAction);
        }

        #endregion

        #endregion

        #endregion

        #endregion
    }
}