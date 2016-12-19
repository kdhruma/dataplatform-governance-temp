using System;
using System.IO;
using System.Reflection;
using System.Diagnostics;

namespace MDM.Imports.Processor
{
    using MDM.AdminManager.Business;
    using MDM.BusinessObjects;
    using MDM.BusinessObjects.Imports;
    using MDM.BusinessObjects.Jobs;
    using MDM.Core;
    using MDM.ExceptionManager;
    using MDM.Imports.Interfaces;
    using MDM.ImportSources.RSXml;
    using MDM.ImportSources.StagingDB;
    using MDM.ImportSources.Generic;

    using MDM.JobManager.Business;
    using MDM.ProfileManager.Business;
    using MDM.Utility;

    using Riversand.JobService.Interfaces;
    using MDM.ImportSources.Generic11;
    using MDM.ImportSources.Generic12;
    using MDM.BusinessObjects.Diagnostics;
    using MDM.CacheManager.Business;
    using MDM.ExceptionManager.Handlers;
    using MDM.BusinessRuleManagement.Business;
    using MDM.ImportSources.DDG;

    /// <summary>
    /// Specifies the Import Job Processor class
    /// </summary>
    public class ImportJobProcessor : IJob
    {
        #region Constants

        /// <summary>
        /// Identify the common application to store the temporary files.
        /// </summary>
        internal static String COMMON_APP_DATA = AppConfigurationHelper.GetAppConfig<String>("Jobs.TemporaryFileRoot");

        /// <summary>
        /// Import service specific area. TODO..is version required here?
        /// </summary>
        internal static String IMPORT_APP_DATA = @"Imports";

        internal static String SCHEMA_CONFIG_FOLDER = "Config";

        internal static String RSXML41_SCHEMA_NAME = "RSXml4.1.xsd";

        internal static String RSXML45_SCHEMA_NAME = "RSXml4.5.xsd";

        internal static String RSXLIFF10_SCHEMA_NAME = "RSXliff1.2.xsd";

        #endregion

        #region Fields

        private EventLogHandler elog = new EventLogHandler();

        /// <summary>
        /// 
        /// </summary>
        private Job _job = null;

        /// <summary>
        /// Indicates instance of JobBL
        /// </summary>
        private JobBL jobManager = new JobBL();

        /// <summary>
        /// Instance of Import Profile
        /// </summary>
        private ImportProfile _importProfile = new ImportProfile();

        /// <summary>
        /// Instance of lookup Import Profile.
        /// </summary>
        private LookupImportProfile _lookupImportProfile = new LookupImportProfile();

        /// <summary>
        /// Instance of datamodel Import Profile.
        /// </summary>
        private DataModelImportProfile _dataModelImportProfile = new DataModelImportProfile();

        /// <summary>
        /// Instance of ddg Import Profile.
        /// </summary>
        private DDGImportProfile _ddgImportProfile = new DDGImportProfile();

        /// <summary>
        /// Top level working folder for the imports.
        /// </summary>
        private String _importWorkingFolder = String.Empty;

        /// <summary>
        /// Job instance specific working folder.
        /// </summary>
        private String _jobWorkingFolder = String.Empty;

        /// <summary>
        /// Instance of import Engine.
        /// </summary>
        private IImportEngine _importEngine = null;

        /// <summary>
        /// Indicates current trace settings
        /// </summary>
        private TraceSettings _traceSettings = new TraceSettings();

        /// <summary>
        /// Root activity
        /// </summary>
        private DiagnosticActivity _rootActivity;

        /// <summary>
        /// Operation id
        /// </summary>
        string _operationId = "";

        #endregion

        #region Constructors

        /// <summary>
        /// 
        /// </summary>
        /// <param name="job"></param>
        public ImportJobProcessor(Job job)
        {
            if (job == null)
                throw new ArgumentNullException("job");

            this._job = job;

            _importWorkingFolder = Path.Combine(COMMON_APP_DATA, IMPORT_APP_DATA);

            // Create the import service specific data folder if not exists..
            if (!System.IO.Directory.Exists(_importWorkingFolder))
                Directory.CreateDirectory(_importWorkingFolder);
        }

        #endregion

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public Job Job
        {
            get
            {
                return _job;
            }
        }

        /// <summary>
        /// Property denoting Import Profile
        /// </summary>
        public ImportProfile ImportProfile
        {
            get
            {
                return _importProfile;
            }
        }

        /// <summary>
        /// Property denoting Lookup Import Profile
        /// </summary>
        public LookupImportProfile LookupImportProfile
        {
            get
            {
                return _lookupImportProfile;
            }
        }

        /// <summary>
        /// Property denoting DataModel Import Profile
        /// </summary>
        public DataModelImportProfile DataModelImportProfile
        {
            get
            {
                return _dataModelImportProfile;
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        public bool cancelJob
        {
            get
            {
                return false;
            }
            set
            {
            }
        }

        /// <summary>
        /// Property denoting identifier of job
        /// </summary>
        public int id
        {
            get { return _job.Id; }
            set { _job.Id = value; }
        }

        /// <summary>
        /// Property denoting description about job
        /// </summary>
        public string description
        {
            get
            {
                return _job.Description;
            }
            set
            {
                _job.Description = description;
            }
        }

        /// <summary>
        /// Proprty denoting user name who created job
        /// </summary>
        public string username
        {
            get { return _job.CreatedUser; }
        }

        /// <summary>
        /// Property denoting working folder for import
        /// </summary>
        public String ImportWorkingFolder
        {
            get { return _importWorkingFolder; }
        }

        /// <summary>
        /// Property denoting working folder for job
        /// </summary>
        public String JobWorkingFolder
        {
            get { return _jobWorkingFolder; }
        }

        #endregion

        #region Methods

        #region Public methods

        /// <summary>
        /// 
        /// </summary>
        public void Execute()
        {
            DiagnosticActivity activity = new DiagnosticActivity();

            String programName = string.Format("JobEngine Execute started job id ({0}), job name ({1})", _job.Id, _job.Name);

            ImportEventArgs importEventArgs = null;
            LookupImportEventArgs lookupImportEventArgs = null;
            DataModelImportEventArgs dataModelImportEventArgs = null;
            DDGImportEventArgs ddgImportEventArgs = null;

            CallerContext callerContext = new CallerContext(MDMCenterApplication.JobService, Utility.GetModule(_job.JobType), programName, _job.Id, _job.JobData.ProfileId, "");

            try
            {
                if (_job == null)
                    throw new ArgumentNullException("Job instance is not loaded.");

                InitializeLogging();
                
                var executionContext = new ExecutionContext(callerContext, new CallDataContext(), new SecurityContext(0, _job.CreatedUser, 0, ""), "");
                executionContext.LegacyMDMTraceSources.Add(MDMTraceSource.Imports);

                _rootActivity.Start(executionContext);
                activity.Start();

                // Error out job if it is not in Pending or Queued status
                if (!(_job.JobStatus == JobStatus.Pending || _job.JobStatus == JobStatus.Queued))
                {
                    string message = String.Format("Job can only be in Pending or Queued status, job ({0}) current status is {1}", 
                        _job.Id, _job.JobStatus.ToString());                   

                    throw new ApplicationException(message);
                }

                #region Create Job Working Area

                // Create a local working folder using the job id.
                _jobWorkingFolder = Path.Combine(_importWorkingFolder, _job.Id.ToString());

                // Create MDM local data folder if not exists..
                if (!System.IO.Directory.Exists(_jobWorkingFolder))
                    Directory.CreateDirectory(_jobWorkingFolder);

                #endregion

                #region Update job status to Running

                _job.JobStatus = Core.JobStatus.Running;
                _job.JobData.ExecutionStatus.StartTime = DateTime.Now.ToString();
                OperationResult jobUpdateOR = jobManager.Update(_job, 
                    new CallerContext(MDMCenterApplication.JobService, Utility.GetModule(_job.JobType), programName));

                #endregion

                #region Load profile for the job

                ExecutionStepCollection executionSteps = new ExecutionStepCollection();

                switch (_job.JobType)
                {
                    case JobType.EntityImport:
                        this._importProfile = GetImportProfile(_job.JobData.ProfileId);
                        executionSteps = this._importProfile.ExecutionSteps;
                        break;

                    case JobType.LookupImport:
                        this._lookupImportProfile = GetLookupImportProfile(_job.JobData.ProfileId);
                        executionSteps = this._lookupImportProfile.ExecutionSteps;
                        break;

                    case JobType.DataModelImport:
                        this._dataModelImportProfile = GetDataModelImportProfile(_job.JobData.ProfileId);
                        executionSteps = this._dataModelImportProfile.ExecutionSteps;
                        break;

                    case JobType.DDGImport:
                        this._ddgImportProfile = GetDDGImportProfile(_job.JobData.ProfileId);
                        executionSteps = this._ddgImportProfile.ExecutionSteps;
                        break;

                    default:
                        break;
                }

                #endregion

                #region Execute import steps

                foreach (ExecutionStep executionStep in executionSteps)
                {
                    if (executionStep.StepType == ExecutionStepType.Core)
                    {
                        switch (executionStep.Name.ToLower())
                        {
                            case "process":// this is the dispatcher for core import engines.
                                RunImportEngine(executionStep);
                                break;
                            default:
                                break;
                        }
                    }
                    else if (executionStep.StepType == ExecutionStepType.Custom)
                    {
                        RunExternalImportStep(executionStep);
                    }
                }

                #endregion
            }
            catch (AggregateException aex)
            {
                foreach (var ex in aex.InnerExceptions)
                {
                    AddErrorMessageToOperationResult(ex);                    
                    activity.LogError(ex.Message);
                }

                //Update job in database..
                jobManager.Update(_job, new CallerContext(MDMCenterApplication.JobService, Utility.GetModule(_job.JobType), programName));
            }
            catch (Exception ex)
            {
                AddErrorMessageToOperationResult(ex);
                activity.LogError(ex.Message);
                
                //Update job in database..
                jobManager.Update(_job, new CallerContext(MDMCenterApplication.JobService, Utility.GetModule(_job.JobType), programName));
            }
            finally
            {
                #region Clean up Job Working Area

                // Delete MDM local data folder if it exists..
                if (System.IO.Directory.Exists(_jobWorkingFolder))
                    System.IO.Directory.Delete(_jobWorkingFolder, true);

                #endregion

                activity.Stop();
                _rootActivity.Stop();

                if (_traceSettings.IsBasicTracingEnabled)
                {
                    _job.JobData.JobParameters.Add(new JobParameter("ShowTrace", _traceSettings.IsBasicTracingEnabled.ToString()));
                    _job.JobData.JobParameters.Add(new JobParameter("OperationId", _rootActivity.OperationId.ToString()));
                    jobManager.Update(_job, callerContext);
                }
            }

            #region Publish Import Completed/Aborted Event

            ExecutionStatus executionStatus = this._importEngine == null ? new ExecutionStatus() : this._importEngine.GetExecutionStatus();

            switch (_job.JobType)
            {
                case JobType.EntityImport:
                    importEventArgs = new ImportEventArgs(this._job, this._importProfile, executionStatus, MDMCenterApplication.JobService, MDMCenterModules.Import, MDMPublisher.MDM_Import);

                    if (_job != null)
                    {
                        if (_job.JobStatus == JobStatus.Completed || _job.JobStatus == JobStatus.CompletedWithErrors || _job.JobStatus == JobStatus.CompletedWithWarnings
                            || _job.JobStatus == JobStatus.CompletedWithWarningsAndErrors)
                        {
                            ImportEventManager.Instance.OnImportCompleted(importEventArgs);
                        }
                        else if (_job.JobStatus == JobStatus.Aborted)
                        {
                            ImportEventManager.Instance.OnImportAborted(importEventArgs);
                        }
                    }
                    break;

                case JobType.LookupImport:
                    lookupImportEventArgs = new LookupImportEventArgs(this._job, this._lookupImportProfile, executionStatus, MDMCenterApplication.JobService, MDMCenterModules.Import, MDMPublisher.MDM_Import);

                    if (_job != null)
                    {
                        if (_job.JobStatus == JobStatus.Completed || _job.JobStatus == JobStatus.CompletedWithErrors || _job.JobStatus == JobStatus.CompletedWithWarnings
                            || _job.JobStatus == JobStatus.CompletedWithWarningsAndErrors)
                        {
                            LookupImportEventManager.Instance.OnImportCompleted(lookupImportEventArgs);
                        }
                        else if (_job.JobStatus == JobStatus.Aborted)
                        {
                            LookupImportEventManager.Instance.OnImportAborted(lookupImportEventArgs);
                        }
                    }
                    break;

                case JobType.DataModelImport:
                    dataModelImportEventArgs = new DataModelImportEventArgs(this._job, this._dataModelImportProfile, executionStatus, MDMCenterApplication.JobService, MDMCenterModules.Import, MDMPublisher.MDM_Import);

                    if (_job != null)
                    {
                        if (_job.JobStatus == JobStatus.Completed || _job.JobStatus == JobStatus.CompletedWithErrors || _job.JobStatus == JobStatus.CompletedWithWarnings
                            || _job.JobStatus == JobStatus.CompletedWithWarningsAndErrors)
                        {
                            DataModelImportEventManager.Instance.OnImportCompleted(dataModelImportEventArgs);
                        }
                        else if (_job.JobStatus == JobStatus.Aborted)
                        {
                            DataModelImportEventManager.Instance.OnImportAborted(dataModelImportEventArgs);
                        }
                    }
                    break;
                case JobType.DDGImport:
                    ddgImportEventArgs = new DDGImportEventArgs(this._job, this._ddgImportProfile, executionStatus, new CallerContext(MDMCenterApplication.JobService, MDMCenterModules.Import), MDMPublisher.MDM_Import);

                    if (_job != null)
                    {
                        if (_job.JobStatus == JobStatus.Completed || _job.JobStatus == JobStatus.CompletedWithErrors ||
                            _job.JobStatus == JobStatus.CompletedWithWarnings || _job.JobStatus == JobStatus.CompletedWithWarningsAndErrors)
                        {
                            DDGImportEventManager.Instance.OnDDGImportCompleted(ddgImportEventArgs);
                        }
                        else if (_job.JobStatus == JobStatus.Aborted)
                        {
                            DDGImportEventManager.Instance.OnDDGImportAborted(ddgImportEventArgs);
                        }
                    }
                    break;

                default:
                    break;
            }

            #endregion
        }

        /// <summary>
        /// 
        /// </summary>
        public void CleanUp()
        {
            //TODO:: Cleanup something here...but what?? :)
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool IsComplete()
        {
            if (_job.JobStatus == JobStatus.Completed)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Check whether the job is ignored or not
        /// </summary>
        /// <returns>True if ignored else false</returns>
        public Boolean IsIgnored()
        {
            Boolean result = false;

            if (_job.JobStatus == JobStatus.Ignored)
            {
                result = true;
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        public void SaveJob()
        {
            DiagnosticActivity activity = new DiagnosticActivity();
            if (_traceSettings.IsBasicTracingEnabled) activity.Start();

            String programName = String.Format("JobEngine.ImportJobProcessor: JobId: {0}, Name:{1}", _job.Id, _job.Name);
            jobManager.Update(_job, new CallerContext(MDMCenterApplication.JobService, Utility.GetModule(_job.JobType), programName));

            if (_traceSettings.IsBasicTracingEnabled) activity.Stop();
        }

        #endregion

        #region Private Methods

        private ImportProfile GetImportProfile(Int32 profileId)
        {
            DiagnosticActivity activity = new DiagnosticActivity();
            if (_traceSettings.IsBasicTracingEnabled) activity.Start();

            if (profileId < 1)
            {
                string message = string.Format("Invalid import profile - Id ({0})", profileId);
                throw new ApplicationException(message);
            }

            ImportProfileBL importProfileManager = new ImportProfileBL();

            ImportProfile importProfile = importProfileManager.Get(profileId, 
                new CallerContext(MDMCenterApplication.JobService, MDMCenterModules.Unknown));

            if (importProfile == null)
            {
                string message = string.Format("Unable to retrieve import profile - Id ({0})", profileId);
                throw new ApplicationException(message);
            }

            if (_traceSettings.IsBasicTracingEnabled)
            {
                string message = string.Format("Retrieved import profile - Id ({0})", profileId);
                
                activity.LogInformation(message);
                activity.Stop();
            }

            return importProfile;
        }

        private LookupImportProfile GetLookupImportProfile(Int32 profileId)
        {
            DiagnosticActivity activity = new DiagnosticActivity();
            if (_traceSettings.IsBasicTracingEnabled) activity.Start();

            if (profileId < 1)
            {
                string message = string.Format("Invalid lookup import profile - Id ({0})", profileId);
                throw new ApplicationException(message);
            }

            LookupImportProfileBL lkpProfileManager = new LookupImportProfileBL();

            LookupImportProfile lookupImportProfile = lkpProfileManager.GetProfileById(profileId,
                new CallerContext(MDMCenterApplication.JobService, MDMCenterModules.Unknown));

            if (lookupImportProfile == null)
            {
                string message = string.Format("Unable to retrieve lookup import profile - Id ({0})", profileId);
                throw new ApplicationException(message);
            }

            if (_traceSettings.IsBasicTracingEnabled)
            {
                string message = string.Format("Retrieved lookup import profile - Id ({0})", profileId);

                activity.LogInformation(message);
                activity.Stop();
            }

            return lookupImportProfile;
        }

        private DataModelImportProfile GetDataModelImportProfile(Int32 profileId)
        {
            DiagnosticActivity activity = new DiagnosticActivity();
            if (_traceSettings.IsBasicTracingEnabled) activity.Start();

            if (profileId < 1)
            {
                string message = string.Format("Invalid data model import profile - Id ({0})", profileId);
                throw new ApplicationException(message);
            }

            DataModelImportProfileBL profileManager = new DataModelImportProfileBL();

            DataModelImportProfile dataModelImportProfile = profileManager.GetProfileById(profileId,
                new CallerContext(MDMCenterApplication.JobService, MDMCenterModules.Unknown));

            if (dataModelImportProfile == null)
            {
                string message = string.Format("Unable to retrieve data model import profile - Id ({0})", profileId);
                throw new ApplicationException(message);
            }

            if (_traceSettings.IsBasicTracingEnabled)
            {
                string message = string.Format("Retrieved data model import profile - Id ({0})", profileId);

                activity.LogInformation(message);
                activity.Stop();
            }
            
            return dataModelImportProfile;
        }

        private DDGImportProfile GetDDGImportProfile(Int32 profileId)
        {
            //String message = String.Empty;
            //DiagnosticActivity activity = new DiagnosticActivity();

            //if (_traceSettings.IsBasicTracingEnabled)
            //{
            //    activity.LogInformation(String.Format("Loading the ddg import profile - Id ({0})", profileId));
            //    activity.Start();
            //}

            //if (profileId < 1)
            //{
            //    message = String.Format("Invalid ddg import profile - Id ({0})", profileId);
            //    throw new ArgumentException(message);
            //}

            //DDGImportProfileBL profileManager = new DDGImportProfileBL();
            //DDGImportProfile ddgImportProfile = profileManager.GetProfileById(profileId, new CallerContext(MDMCenterApplication.JobService, MDMCenterModules.DDGImport));

            //if (ddgImportProfile == null)
            //{
            //    message = String.Format("Unable to load ddg import profile - id ({0})", profileId);
            //    throw new ApplicationException(message);
            //}

            //if (_traceSettings.IsBasicTracingEnabled)
            //{
            //    activity.LogInformation(String.Format("Loaded the ddg import profile - Id ({0})", profileId));
            //    activity.Stop();
            //}

            return null;
        }

        private Boolean RunImportEngine(ExecutionStep currentExecutionStep)
        {
            Boolean successFlag = true;

            switch (_job.JobType)
            { 
                case JobType.EntityImport:
                    successFlag = RunEntityImportEngine(currentExecutionStep);
                    break;

                case JobType.LookupImport:
                    successFlag = RunLookupDataImportEngine(currentExecutionStep);
                    break;

                case JobType.DataModelImport:
                    successFlag = RunDataModelImportEngine(currentExecutionStep);
                    break;


                default:
                    break;
            }

            return successFlag;    
        }

        private Boolean RunEntityImportEngine(ExecutionStep currentExecutionStep)
        {
            DiagnosticActivity activity = new DiagnosticActivity();
            if (_traceSettings.IsBasicTracingEnabled) activity.Start();

            Boolean successFlag = true;

            // Step: Get source data interface instance..
            IImportSourceData sourceData = GetImportSourceData(currentExecutionStep);
            if (sourceData == null)
            {
                String message = String.Format("Source data is null. Check the job profile and run the job again.");
                throw new ApplicationException(message);
            }

            // Publish Event "OnImportStarted"
            PublishImportStartedEvent();

            // Step: initialize entity import engine
            EntityImportEngine entityImportEngine = new EntityImportEngine();
            
            // Assign import engine with the current engine
            this._importEngine = entityImportEngine;

            // Initialize entity import engine
            Boolean isInitialized = entityImportEngine.Initialize(this._job, this._importProfile);            
            
            // Step: run entity import engine..
            if (isInitialized)
            {
                successFlag = entityImportEngine.RunStep(currentExecutionStep.Name, currentExecutionStep.StepConfiguration, sourceData);
            }
            
            if (_traceSettings.IsBasicTracingEnabled) activity.Stop();

            return successFlag;
        }

        private Boolean RunLookupDataImportEngine(ExecutionStep currentExecutionStep)
        {
            DiagnosticActivity activity = new DiagnosticActivity();
            if (_traceSettings.IsBasicTracingEnabled) activity.Start();

            Boolean successFlag = true;

            // Step: Get source data interface instance..
            ILookupImportSourceData lookupSourceData = GetLookupImportSourceData(currentExecutionStep);

            if (lookupSourceData == null)
            {
                String message = String.Format("Source data is null. Check the job profile and run the job again.");
                throw new ApplicationException(message);
            }

            // Publish Event "OnImportStarted"
            PublishImportStartedEvent();

            // Step: initialize lookup import engine
            LookupImportEngine importEngine = new LookupImportEngine();
            
            Boolean isInitialized = importEngine.Initialize(this._job, this._lookupImportProfile);

            // Step: run lookup import engine..
            if (isInitialized)
            {
                successFlag = importEngine.RunStep(currentExecutionStep.Name, currentExecutionStep.StepConfiguration, lookupSourceData);
            }

            if (_traceSettings.IsBasicTracingEnabled) activity.Stop();

            return successFlag;
        }
        
        /// <summary>
        /// Runs DataModel Import Engine
        /// </summary>
        /// <param name="currentExecutionStep"></param>
        /// <returns>True on succcess.</returns>
        private Boolean RunDataModelImportEngine(ExecutionStep currentExecutionStep)
        {
            DiagnosticActivity activity = new DiagnosticActivity();
            if (_traceSettings.IsBasicTracingEnabled) activity.Start();

            Boolean successFlag = true;

            // Step: Get source data interface instance..
            IDataModelImportSourceData dataModelSourceData = GetDataModelImportSourceData(currentExecutionStep);

            if (dataModelSourceData == null)
            {
                String message = String.Format("Source data is null. Check the job profile and run the job again.");
                throw new ApplicationException(message);
            }
            
            // Publish Event "OnImportStarted"
            PublishImportStartedEvent();

            // Step: initialize dataModel import engine
            DataModelImportEngine importEngine = new DataModelImportEngine();

            Boolean isInitialized = importEngine.Initialize(this._job, this._dataModelImportProfile);

            // Step: run dataModel import engine..
            if (isInitialized)
            {
                successFlag = importEngine.RunStep(currentExecutionStep.Name, currentExecutionStep.StepConfiguration, dataModelSourceData);
            }

            if (_traceSettings.IsBasicTracingEnabled) activity.Stop();
            
            return successFlag;
        }

        private Boolean RunExternalImportStep(ExecutionStep currentExecutionStep)
        {
            string message = string.Empty;
            DiagnosticActivity activity = new DiagnosticActivity();
            if (_traceSettings.IsBasicTracingEnabled) activity.Start();

            String assemblyFileName = currentExecutionStep.AssemblyFileName;
            String classFullName = currentExecutionStep.ClassFullName;
            String methodName = "RunStep"; //TODO:: How to bring interface here..

            //Check if method exists..
            if (!ExtensionHelper.CheckMethod(assemblyFileName, classFullName, methodName))
            {
                message = String.Format("Unable to run external import step. Provided assembly does not exist. ExecutionStep: {0}, Assembly Name: {1}, ClassName: {2}, Method: {3}", currentExecutionStep.Name, assemblyFileName, classFullName, methodName);
                throw new ApplicationException(message);
            }

            try
            {
                object[] parameters = new object[] { currentExecutionStep, this.Job, this.ImportProfile };
                ExtensionHelper.InvokeMethod(assemblyFileName, classFullName, methodName, parameters);

                if (_traceSettings.IsBasicTracingEnabled)
                {
                    message = string.Format("Invoked external import step - assembly ({0}), classname ({1}), methodname ({2})",
                        assemblyFileName, classFullName, methodName);

                    activity.LogInformation(message);
                }
            }
            catch (Exception ex)
            {
                message = String.Format("Unhandled exception occurred while running execution step. ExecutionStep: {0}, Assembly Name: {1}, ClassName: {2}, Method: {3}", currentExecutionStep.Name, assemblyFileName, classFullName, methodName);
                throw new ApplicationException(message, ex);
            }
            finally
            {
                if (_traceSettings.IsBasicTracingEnabled) activity.Stop();
            }            

            return true;
        }

        private void PublishImportStartedEvent()
        {
            ExecutionStatus executionStatus = new ExecutionStatus();
            ImportEventArgs importEventArgs;

            switch (_job.JobType)
            {
                case JobType.EntityImport:
                    importEventArgs = new ImportEventArgs(this._job, this._importProfile, executionStatus, MDMCenterApplication.JobService, MDMCenterModules.Import, MDMPublisher.MDM_Import);
                    ImportEventManager.Instance.OnImportStarted(importEventArgs);
                    break;

                case JobType.LookupImport:
                    LookupImportEventArgs lookupImportEventArgs = new LookupImportEventArgs(this._job, this._lookupImportProfile, executionStatus, MDMCenterApplication.JobService, MDMCenterModules.Import, MDMPublisher.MDM_Import);
                    LookupImportEventManager.Instance.OnImportStarted(lookupImportEventArgs);
                    break;

                case JobType.DataModelImport:
                    DataModelImportEventArgs dataModelImportEventArgs = new DataModelImportEventArgs(this._job, this._dataModelImportProfile, executionStatus, MDMCenterApplication.JobService, MDMCenterModules.Import, MDMPublisher.MDM_Import);
                    DataModelImportEventManager.Instance.OnImportStarted(dataModelImportEventArgs);
                    break;

                default:
                    importEventArgs = new ImportEventArgs(this._job, this._importProfile, executionStatus, MDMCenterApplication.JobService, MDMCenterModules.Import, MDMPublisher.MDM_Import);
                    ImportEventManager.Instance.OnImportStarted(importEventArgs);
                    break;
            }
        }

        #region Create Import source data

        private IImportSourceData GetImportSourceData(ExecutionStep currentExecutionStep)
        {
            DiagnosticActivity activity = new DiagnosticActivity();
            if (_traceSettings.IsBasicTracingEnabled) activity.Start();

            IImportSourceData sourceData = null;

            // Get current source type from step configuration..
            ImportSourceType sourceType = this.ImportProfile.InputSpecifications.Reader;

            // Create instance of appropriate import source
            switch (sourceType)
            {
                case ImportSourceType.StagingDB10:
                    sourceData = GetStagingDBSourceData(currentExecutionStep);
                    break;
                case ImportSourceType.RSXml41:
                case ImportSourceType.RSXml45:
                    sourceData = GetRSXmlSourceData(currentExecutionStep, sourceType);
                    break;

                case ImportSourceType.RSMAM10:
                    sourceData = GetMamSourceData(currentExecutionStep);
                    break;
                case ImportSourceType.RSExcel12:
                    sourceData = GetExcelSourceData(currentExecutionStep, sourceType);
                    break;
                case ImportSourceType.Generic10:
                    sourceData = GetGeneric10SourceData(currentExecutionStep);
                    break;
                case ImportSourceType.Generic11:
                    sourceData = GetGeneric11SourceData(currentExecutionStep);
                    break;
                case ImportSourceType.Generic12:
                    sourceData = GetGeneric12SourceData(currentExecutionStep);
                    break;
                case ImportSourceType.RSDsv10:
                    sourceData = GetRsDsvSourceData(currentExecutionStep);
                    break;
                case ImportSourceType.RSXliff10:
                    sourceData = GetXliffSourceData(currentExecutionStep, sourceType);
                    break;
                default:
                    throw new NotImplementedException("Provided import source type :" + sourceType.ToString() + " is not implemented");
            }

            if (_traceSettings.IsBasicTracingEnabled)
            {
                string message = string.Format("Initialized import source data - source type ({0}), execution step ({1})", sourceType.ToString(), currentExecutionStep.Name);
                
                activity.LogInformation(message);
                activity.Stop();
            }

            return sourceData;
        }

        private ILookupImportSourceData GetLookupImportSourceData(ExecutionStep currentExecutionStep)
        {
            DiagnosticActivity activity = new DiagnosticActivity();
            if (_traceSettings.IsBasicTracingEnabled) activity.Start();

            ILookupImportSourceData sourceData = null;

            // Get current source type from step configuration..
            ImportSourceType sourceType = this.LookupImportProfile.InputSpecifications.Reader;

            // Create instance of appropriate import source
            switch (sourceType)
            {
                case ImportSourceType.LookupData10:
                    sourceData = GetLookupSourceData(currentExecutionStep);
                    break;
                case ImportSourceType.LookupExcel10:
                    sourceData = GetLookupSourceExcelData(currentExecutionStep);
                    break;
                case ImportSourceType.RSLookupExcel10:
                case ImportSourceType.RSLookupXml10:
                case ImportSourceType.RSLookupGeneric10:
                case ImportSourceType.RSLookupDSV10:
                case ImportSourceType.RSLookupXliff10:
                    sourceData = GetLookupSourceDataSource(sourceType);
                    break;
                default:
                    throw new NotImplementedException("Provided import source type :" + sourceType.ToString() + " is not implemented");
            }

            if (_traceSettings.IsBasicTracingEnabled)
            {
                string message = string.Format("Initialized lookup import source data - source type ({0})", sourceType.ToString());
                
                activity.LogInformation(message);
                activity.Stop();
            }

            return sourceData;
        }

        /// <summary>
        /// Get DataModel Import Source Data
        /// </summary>
        /// <param name="currentExecutionStep"></param>
        /// <returns>IDataModelImportSourceData</returns>
        private IDataModelImportSourceData GetDataModelImportSourceData(ExecutionStep currentExecutionStep)
        {
            DiagnosticActivity activity = new DiagnosticActivity();
            if (_traceSettings.IsBasicTracingEnabled) activity.Start();

            IDataModelImportSourceData sourceData = null;

            // Get current source type from step configuration..
            ImportSourceType sourceType = this.DataModelImportProfile.InputSpecifications.Reader;

            // Create instance of appropriate import source
            switch (sourceType)
            {
                case ImportSourceType.RSDataModelExcel:
                    sourceData = GetDataModelSourceData(sourceType);
                    break;

                default:
                    throw new NotImplementedException("Provided import source type :" + sourceType.ToString() + " is not implemented");
            }

            if (_traceSettings.IsBasicTracingEnabled)
            {
                string message = string.Format("Initialized data model import source data - source type ({0})", sourceType.ToString());
                
                activity.LogInformation(message);
                activity.Stop();
            }

            return sourceData;
        }

        private IDDGImportSourceData GetDDGImportSourceData(ExecutionStep currentExecutionStep)
        {
            IDDGImportSourceData sourceData = null;

            DiagnosticActivity activity = new DiagnosticActivity();

            if (_traceSettings.IsBasicTracingEnabled)
            {
                activity.Start();
            }

            try
            {
                // Get current source type from step configuration..
                ImportSourceType sourceType = this._ddgImportProfile.InputSpecifications.Reader;

                String fullFileName = CreateSourceFile();

                switch (sourceType)
                {
                    case ImportSourceType.RSDDGExcel:
                        sourceData = new RSDDGExcel(fullFileName);
                        break;
                }

                AddSourceFileParameter(fullFileName);

                if (sourceData != null && sourceData.Initialize(_job, _ddgImportProfile))
                {
                    return sourceData;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (_traceSettings.IsBasicTracingEnabled)
                {
                    activity.LogInformation("DDG source initialized");
                    activity.Stop();
                }
            }
        }

        private IEntityImportSourceData GetStagingDBSourceData(ExecutionStep currentExecutionStep)
        {
            DiagnosticActivity activity = new DiagnosticActivity();
            if (_traceSettings.IsBasicTracingEnabled) activity.Start();

            DataBaseSourceData sourceData = new ImportSources.StagingDB.DataBaseSourceData(_traceSettings.IsBasicTracingEnabled);

            try
            {
                if (sourceData.Initialize(_job, _importProfile))
                    return sourceData;
                else
                    return null;
            }
            finally
            {
                if (_traceSettings.IsBasicTracingEnabled)
                {
                    string message = string.Format("Staging DB source initialized for - Entity index range {0} - {1}, Entity count {2}, Batch size {3}",
                        sourceData.EntitySeed, sourceData.EntityEndPoint, sourceData.EntityCount, sourceData.BatchSize);

                    activity.LogInformation(message);
                    activity.Stop();
                }
            }
        }

        private IEntityImportSourceData GetRSXmlSourceData(ExecutionStep currentExecutionStep, ImportSourceType sourceType)
        {
            DiagnosticActivity activity = new DiagnosticActivity();
            if (_traceSettings.IsBasicTracingEnabled) activity.Start();

            BaseXmlSource sourceData = null;

            String fullFileName = CreateSourceFile();

            MapSchemaFilePath(sourceType);

            if (sourceType == ImportSourceType.RSXml41)
            {
                sourceData = new XmlSource41(fullFileName);
            }
            else if (sourceType == ImportSourceType.RSXml45)
            {
                sourceData = new XmlSource45(fullFileName);
            }

            AddSourceFileParameter(fullFileName);

            try
            {
                if (sourceData.Initialize(_job, _importProfile))
                    return (IEntityImportSourceData)sourceData;
                else
                    return null;
            }
            finally
            {
                if (_traceSettings.IsBasicTracingEnabled)
                {
                    string message = string.Format("RSXml source initialized for - Source Type ({0}), Batching Type ({1}), Source File ({2})",
                        sourceType.ToString(), sourceData.BatchingType.ToString(), fullFileName);

                    activity.LogInformation(message);
                    activity.Stop();
                }
            }
        }

        private IEntityImportSourceData GetMamSourceData(ExecutionStep currentExecutionStep)
        {
            DiagnosticActivity activity = new DiagnosticActivity();
            if (_traceSettings.IsBasicTracingEnabled) activity.Start();

            ImportSources.MAM.MamSource sourceData = new ImportSources.MAM.MamSource();

            try
            {
                if (sourceData.Initialize(_job, _importProfile))
                {
                    return sourceData;
                }     
            }
            finally
            {
                if (_traceSettings.IsBasicTracingEnabled)
                {
                    string message = string.Format("MAM source initialized for - Source File ({0}), Batching Type ({1})", sourceData.SourceFile, sourceData.BatchingType.ToString());

                    activity.LogInformation(message);
                    activity.Stop();
                }
            }

            return null;
        }

        private IEntityImportSourceData GetExcelSourceData(ExecutionStep currentExecutionStep, ImportSourceType sourceType)
        {
            DiagnosticActivity activity = new DiagnosticActivity();
            if (_traceSettings.IsBasicTracingEnabled) activity.Start();

            IEntityImportSourceData sourceData = null;

            String fullFileName = CreateSourceFile();

            if (sourceType == ImportSourceType.RSExcel12)
            {
                sourceData = new ImportSources.RSExcel.ExcelSource12(fullFileName);
            }

            try
            {
                AddSourceFileParameter(fullFileName);

                if (sourceData != null && sourceData.Initialize(_job, _importProfile))
                {
                    return sourceData;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (_traceSettings.IsBasicTracingEnabled)
                {
                    activity.LogInformation(string.Format("Excel source initialized"));
                    activity.Stop();
                }
            }
        }

        private ILookupImportSourceData GetLookupSourceData(ExecutionStep currentExecutionStep)
        {
            DiagnosticActivity activity = new DiagnosticActivity();
            if (_traceSettings.IsBasicTracingEnabled) activity.Start();

            try
            {
                ImportSources.Lookup.LookupSource10 sourceData = null;

                String fullFileName = CreateSourceFile();

                sourceData = new ImportSources.Lookup.LookupSource10(fullFileName);

                if (sourceData.Initialize(_job, _lookupImportProfile))
                {
                    return sourceData;
                }
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (_traceSettings.IsBasicTracingEnabled)
                {
                    activity.LogInformation(string.Format("Lookup source initialized"));
                    activity.Stop();
                }
            }
        }

        private ILookupImportSourceData GetLookupSourceExcelData(ExecutionStep currentExecutionStep)
        {
            DiagnosticActivity activity = new DiagnosticActivity();
            if (_traceSettings.IsBasicTracingEnabled) activity.Start();

            try
            {
                ImportSources.Lookup.LookupSourceExcel10 sourceData = null;

                String fullFileName = CreateSourceFile();

                sourceData = new ImportSources.Lookup.LookupSourceExcel10(fullFileName);

                if (sourceData.Initialize(_job, _lookupImportProfile))
                {
                    return sourceData;
                }
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (_traceSettings.IsBasicTracingEnabled)
                {
                    activity.LogInformation(string.Format("Lookup source excel initialized"));
                    activity.Stop();
                }
            }
        }

        private ILookupImportSourceData GetLookupSourceDataSource(ImportSourceType sourceType)
        {
            DiagnosticActivity activity = new DiagnosticActivity();
            if (_traceSettings.IsBasicTracingEnabled) activity.Start();

            try
            {
                ILookupImportSourceData sourceData = null;

                String fullFileName = CreateSourceFile();

                switch (sourceType)
                {
                    case ImportSourceType.RSLookupExcel10:
                        sourceData = new ImportSources.Lookup.RSLookupExcel10(fullFileName);
                        break;
                    case ImportSourceType.RSLookupXml10:
                        sourceData = new ImportSources.Lookup.RSLookupXml10(fullFileName);
                        break;
                    case ImportSourceType.RSLookupGeneric10:
                        sourceData = new ImportSources.Lookup.RSLookupGeneric10(fullFileName);
                        break;
                    case ImportSourceType.RSLookupDSV10:
                        sourceData = new ImportSources.Lookup.RSLookupDSV10(fullFileName);
                        break;
                    
                }

                AddSourceFileParameter(fullFileName);

                if (sourceData != null && sourceData.Initialize(_job, _lookupImportProfile))
                {
                    return sourceData;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (_traceSettings.IsBasicTracingEnabled)
                {
                    activity.LogInformation(string.Format("Lookup source data initialized"));
                    activity.Stop();
                }
            }
        }

        /// <summary>
        /// Get DataModel Source Data
        /// </summary>
        /// <param name="sourceType"></param>
        /// <returns>IDataModelImportSourceData</returns>
        private IDataModelImportSourceData GetDataModelSourceData(ImportSourceType sourceType)
        {
            DiagnosticActivity activity = new DiagnosticActivity();
            if (_traceSettings.IsBasicTracingEnabled) activity.Start();

            try
            {
                IDataModelImportSourceData sourceData = null;

                String fullFileName = CreateSourceFile();

                switch (sourceType)
                {
                    case ImportSourceType.RSDataModelExcel:
                        sourceData = new ImportSources.DataModel.RSDataModelExcel(fullFileName);
                        break;
                }

                AddSourceFileParameter(fullFileName);

                if (sourceData != null && sourceData.Initialize(_job, _dataModelImportProfile))
                {
                    return sourceData;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (_traceSettings.IsBasicTracingEnabled)
                {
                    activity.LogInformation(string.Format("Data model source initialized"));
                    activity.Stop();
                }
            }
        }

        private IEntityImportSourceData GetGeneric10SourceData(ExecutionStep currentExecutionStep)
        {
            DiagnosticActivity activity = new DiagnosticActivity();
            if (_traceSettings.IsBasicTracingEnabled) activity.Start();

            try
            {
                Generic10 sourceData = new Generic10();

                if (sourceData.Initialize(_job, _importProfile))
                    return sourceData;

                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (_traceSettings.IsBasicTracingEnabled)
                {
                    activity.LogInformation(string.Format("Generic10 source data initialized"));
                    activity.Stop();
                }
            }
        }

        private IEntityImportSourceData GetGeneric11SourceData(ExecutionStep currentExecutionStep)
        {
            DiagnosticActivity activity = new DiagnosticActivity();
            if (_traceSettings.IsBasicTracingEnabled) activity.Start();

            try
            {
                Generic11 sourceData = new Generic11();

                if (sourceData.Initialize(_job, _importProfile))
                    return sourceData;

                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (_traceSettings.IsBasicTracingEnabled)
                {
                    activity.LogInformation(string.Format("Generic11 source data initialized"));
                    activity.Stop();
                }
            }
        }

        private IEntityImportSourceData GetGeneric12SourceData(ExecutionStep currentExecutionStep)
        {
            DiagnosticActivity activity = new DiagnosticActivity();
            if (_traceSettings.IsBasicTracingEnabled) activity.Start();

            try
            {
                Generic12 sourceData = new Generic12();

                if (sourceData.Initialize(_job, _importProfile))
                    return sourceData;

                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (_traceSettings.IsBasicTracingEnabled)
                {
                    activity.LogInformation(string.Format("Generic12 source data initialized"));
                    activity.Stop();
                }
            }
        }

        private IEntityImportSourceData GetRsDsvSourceData(ExecutionStep currentExecutionStep)
        {
            DiagnosticActivity activity = new DiagnosticActivity();
            if (_traceSettings.IsBasicTracingEnabled) activity.Start();

            try
            {
                String fullFileName = CreateSourceFile();

                ImportSources.RsDsv.RsDsv10 sourceData = new ImportSources.RsDsv.RsDsv10(fullFileName);

                AddSourceFileParameter(fullFileName);

                if (sourceData.Initialize(_job, _importProfile))
                    return sourceData;

                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (_traceSettings.IsBasicTracingEnabled)
                {
                    activity.LogInformation(string.Format("RS CSV source data initialized"));
                    activity.Stop();
                }
            }
        }

        private IEntityImportSourceData GetXliffSourceData(ExecutionStep currentExecutionStep, ImportSourceType sourceType)
        {
            DiagnosticActivity activity = new DiagnosticActivity();
            if (_traceSettings.IsBasicTracingEnabled) activity.Start();

            try
            {
                //BaseXliffSource sourceData = null;

                //MapSchemaFilePath(sourceType);
                //String fullFileName = CreateSourceFile();

                //sourceData = new XliffSource10(fullFileName);

                //AddSourceFileParameter(fullFileName);

                ////TO DO : Addition of schema 

                //if (sourceData.Initialize(_job, _importProfile))
                //    return (IEntityImportSourceData)sourceData;
                //else
                    return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (_traceSettings.IsBasicTracingEnabled)
                {
                    activity.LogInformation(string.Format("Xliff source data initialized"));
                    activity.Stop();
                }
            }
        }

        private String CreateSourceFile()
        {
            String fullFileName = String.Empty;

            // fist check if filepath is passed..
            JobParameter filePathParameter = this._job.JobData.JobParameters["FilePath"];

            // if available..use it
            if (filePathParameter != null)
            {
                fullFileName = filePathParameter.Value;
 
                FileInfo fileInfo = new FileInfo(fullFileName);

                if (fileInfo.Exists)
                {
                    return fullFileName;
                }
            }

            // if not..try using file id..

            JobParameter fileIdParameter = this._job.JobData.JobParameters["FileId"];

            if (fileIdParameter == null)
                throw new ArgumentException(String.Format("File id job parameter is missing in job configuration. Please check the file upload status. Job Id: {0}", _job.Id));

            // Try to get file id from the job data parameter..
            Int32 fileId = ValueTypeHelper.Int32TryParse(fileIdParameter.Value, 0);

            if (fileId < 0)
                throw new ArgumentException(String.Format("File id is not set in job configuration. Please check the file upload status. Job Id: {0}", _job.Id));

            File file = new FileBL().GetFile(fileId, false);

            if (file == null)
                throw new ArgumentException(String.Format("Failed to download import data file from storage. Please check the file upload status. Job Id: {0}, File Id: {1}", _job.Id, fileId));

            try
            {
                // if the file name is not given, assume it is RSXML
                if (String.IsNullOrEmpty(file.Name))
                {
                    String fileName = String.Format("{0}_JobId_{1}.xml", this.Job.JobType.ToString(), _job.Id);
                    fullFileName = Path.Combine(_jobWorkingFolder, fileName);
                }
                else
                {
                    fullFileName = Path.Combine(_jobWorkingFolder, file.Name);
                }

                FileInfo fileInfo = new FileInfo(fullFileName);

                if (fileInfo.Exists)
                    fileInfo.Delete();

                //Create a file to write to.
                using (FileStream fileStream = fileInfo.Create())
                {
                    fileStream.Write(file.FileData, 0, file.FileData.Length);
                }
            }
            catch (Exception ex)
            {
                String message = String.Format("Failed to create/update temporary file required during job processing. Job Id:{0}, FileName: {1}, Please verify file locks and try to run job again.", _job.Id, fullFileName);
                throw new ApplicationException(message, ex);
            }

            return fullFileName;
        }

        private void MapSchemaFilePath(ImportSourceType sourceType)
        {
            // we need the schema location..
            String currentExecutionFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            String schemaLocation = String.Empty;
            
            switch (sourceType)
            {
                case ImportSourceType.RSXml41:
                    schemaLocation = Path.Combine(currentExecutionFolder, SCHEMA_CONFIG_FOLDER, RSXML41_SCHEMA_NAME);
                    break;
                case ImportSourceType.RSXml45:
                    schemaLocation = Path.Combine(currentExecutionFolder, SCHEMA_CONFIG_FOLDER, RSXML45_SCHEMA_NAME);
                    break;
                case ImportSourceType.RSLookupXliff10:
                    schemaLocation = Path.Combine(currentExecutionFolder, SCHEMA_CONFIG_FOLDER, RSXLIFF10_SCHEMA_NAME);
                    break;
                case ImportSourceType.RSXliff10:
                    schemaLocation = Path.Combine(currentExecutionFolder, SCHEMA_CONFIG_FOLDER, RSXLIFF10_SCHEMA_NAME);
                    break;
             
            }

            FileInfo schemafileInfo = new FileInfo(schemaLocation);

            if (schemafileInfo.Exists)
            {
                JobParameter schemaLocationParameter = new JobParameter("SchemaFilePath", schemaLocation);

                this._job.JobData.JobParameters.Add(schemaLocationParameter);
            }
        }

        private void AddSourceFileParameter(String fullFilePath)
        {
            JobParameter sourceFileParameter = new JobParameter("SourceFilePath", fullFilePath);

            this._job.JobData.JobParameters.Add(sourceFileParameter);
        }

        private void AddErrorMessageToOperationResult(Exception ex)
        {
            String message = String.Empty;
            DiagnosticActivity activity = new DiagnosticActivity();

            message = String.Format("ImportJobProcessor:Execute failed - {0}", ex.Message);
            if (_traceSettings.IsBasicTracingEnabled)
            {
                activity.LogError(message);
            }

            new ExceptionHandler(ex);

            _job.JobStatus = JobStatus.Aborted;
            _job.Description = "Job has been Aborted. Please check job detail for more information.";

            if (_job.JobData != null && _job.JobData.ExecutionStatus != null)
            {
                _job.JobData.ExecutionStatus.CurrentStatusMessage = JobStatus.Aborted.ToString();
            }

            _job.JobData.OperationResult.Errors.Add(new Error(String.Empty, ex.Message));
        }

        private void AddErrorMessageToOperationResult(String errorMessage)
        {
            if (String.IsNullOrEmpty(_job.Description))
            {
                _job.Description = errorMessage;
            }
            else
            {
                _job.Description = String.Format("{0}. {1}", _job.Description, errorMessage);
            }
            _job.JobData.OperationResult.Errors.Add(new Error(String.Empty, errorMessage));
            _job.JobStatus = JobStatus.CompletedWithErrors;
        }
        
        void InitializeLogging()
        {
            if (this._job.JobData == null) return;

            bool isDiagnostics = false;
            if (this._job.JobData.JobParameters.Contains("ShowTrace"))
            {
                JobParameter showTrace = _job.JobData.JobParameters["ShowTrace"];
                isDiagnostics = ValueTypeHelper.BooleanTryParse(showTrace.Value, false);
            }            

            if (this._job.JobData.JobParameters.Contains("OperationId"))
            {
                JobParameter jobParam = _job.JobData.JobParameters["OperationId"];
                _operationId = jobParam.Value;
            }

            _traceSettings = MDMOperationContextHelper.GetCurrentTraceSettings();

            Guid operationIdGuid = string.IsNullOrEmpty(_operationId) ? Guid.Empty : Guid.Parse(_operationId);
            _rootActivity = DiagnosticActivity.GetRootActivity("ImportProcessorRootActivity", _traceSettings, operationIdGuid, isDiagnostics);
        }        

        #endregion

        #endregion

        #endregion
    }
}
