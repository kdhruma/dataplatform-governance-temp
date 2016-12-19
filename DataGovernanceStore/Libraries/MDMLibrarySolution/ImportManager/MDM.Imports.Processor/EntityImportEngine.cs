using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using OpenXml = DocumentFormat.OpenXml;

namespace MDM.Imports.Processor
{
    using BusinessObjects.DQM;
    using MDM.AdminManager.Business;
    using MDM.BusinessObjects;
    using MDM.BusinessObjects.Diagnostics;
    using MDM.BusinessObjects.Imports;
    using MDM.BusinessObjects.Jobs;
    using MDM.CachedDataModelManager;
    using MDM.CacheManager.Business;
    using MDM.Core;
    using MDM.EntityManager.Business;
    using MDM.ExcelUtility;
    using MDM.ExceptionManager.Handlers;
    using MDM.Imports.Interfaces;
    using MDM.Interfaces;
    using MDM.JobManager.Business;
    using MDM.MessageManager.Business;
    using MDM.ParallelizationManager.Processors;
    using MDM.Services;
    using MDM.Utility;

    /// <summary>
    /// Core import engine
    /// </summary>
    public class EntityImportEngine : IImportEngine
    {
        #region Fields

        /// <summary>
        /// Identify the common application to store the temporary files.
        /// </summary>
        internal static Int32 BATCHES_TO_TRACE_THRESHOLD = AppConfigurationHelper.GetAppConfig<Int32>("MDMCenter.Diagnostics.Imports.OperationTracing.MaxBatchesToTrace");

        #region Logger

        public static EventLogHandler LogHandler = null;

        /// <summary>
        /// Indicates current trace settings
        /// </summary>
        private TraceSettings _traceSettings = new TraceSettings();

        /// <summary>
        /// Batches to trace
        /// </summary>
        Int32 _batchesToTrace;

        #endregion

        #region Thread Lock object

        internal static Object lockObject = new Object();

        internal static Object lockDenormObject = new Object();

        internal static Object lockDiagnosticsObject = new Object();

        internal static Object lockContainerLocaleMapObject = new Object();

        #endregion

        #region Some local hard coded variables

        private List<AttributeModelType> availableAttributeTypes = new List<AttributeModelType>();

        private string importUser = SecurityPrincipalHelper.GetCurrentSecurityPrincipal().CurrentUserName;
        private string importProgram = "ImportEngine";
        private string systemId = "Internal";
        private Int64 auditRefId = -1;
        private UOMCollection uoms = null;

        private Boolean isUOMsCaseSensitive = false;

        #endregion Cached Data Holders

        #region Timer variables.

        private double totalTime = 0;

        private double loadCacheTime = 0;

        #endregion

        #region Other Local variables

        private IImportProgressHandler progressHandler = new ImportProgressHandler();

        private IImportProgressHandler relationshipProgressHandler = new ImportProgressHandler();

        private DataService dataService = null;
                
        private KnowledgeBaseService _knowledgeBaseService = null;

        private Util util = new Util();

        private ICoreAttributeObjects attributeObjects = new AttributeObjects();

        private String categoryPathSeparator = " >> ";

        // Stores the sorted list of containers for MDL processing
        SortedList<Int32, Container> mdlcontainerList = new SortedList<Int32, Container>();
        SortedList<Int32, Int32> finalcontainerList = new SortedList<Int32, Int32>();
        List<Container> containers = null;

        // Stores the sorted list of entity types for Extensions processing
        SortedList<Int32, EntityType> entityTypeList = new SortedList<int, EntityType>();

        private CallerContext callerContext = new CallerContext(MDMCenterApplication.JobService, MDMCenterModules.Import);
        private CallerContext callerContextWithJobId = null;

        LocaleMessageBL localeMessageBL = new LocaleMessageBL();

        private LocaleEnum systemUILocale = LocaleEnum.en_WW;

        private LocaleEnum systemDataLocale = LocaleEnum.en_WW;

        private Collection<Int32> validationMustOnAttributeIdList = new Collection<Int32>();

        private Collection<Int32> validationMustOnRelationshipTypeIdList = new Collection<Int32>();

        private Collection<Int32> validationMustOnRelationshipAttributeIdList = new Collection<Int32>();

        private EntityOperationResultCollection _errorNWarningCollection = null;

        private RelationshipOperationResultCollection _relationshipErrorNWarningCollection = null;

        private Dictionary<Int32, ILocaleCollection> _containerLocaleMap = new Dictionary<Int32, ILocaleCollection>();

        private MDMFeatureConfig _entityDataSourceTrackingConfig = null;

        #endregion

        #region Job objects

        ICachedDataModel cachedDataModel = null;

        private Job job = null;

        private JobBL jobManager = new JobBL();

        private ImportProfile importProfile = null;

        private StepConfiguration stepConfiguration = null;

        //private IImportConfig configData = null;

        private IEntityImportSourceData sourceData = null;

        private ImportProcessingType importProcessingType = ImportProcessingType.ValidateAndProcess;

        private ImportSourceType importSourceType = ImportSourceType.UnKnown;

        private String errorMessagePrefix = String.Empty;

        private JobImportResultHandler jobImportResultHandler = null;

        EntityProcessingOptions entityProcessingOptions = null;

        Boolean jobResultSaveSuccessEntities = false;

        //Int64 matchJobId = 0;

        #endregion

        #region Local configuration values

        private String mdmVersion = "7.0";

        // How many parallel tasks we want for entity creation.
        private int numberOfEntitiesThreads = 1;   // default value..

        // How many parallel task we want for attribute creation PER entity.
        private int numberOfAttributesThreadPerEntity = 1; // default value...

        // batch size per entity thread.
        private int batchSize = 1;

        //total number of entities per thread.
        private Int64 entitybatchPerThread = 0;

        Dictionary<Int32, Int64> entitySplit = null;

        private readonly List<AttributeMap> repeatedAttributeSources = new List<AttributeMap>();

        #endregion

        #region Local Module Variables

        //Name of application which is performing action
        MDMCenterApplication application = MDMCenterApplication.JobService;

        //Name of module which is performing action
        MDMCenterModules importmodule = MDMCenterModules.Import;

        //Name of module which is performing action
        MDMCenterModules stagingModule = MDMCenterModules.Staging;

        //Name of module which is performing action
        MDMCenterModules searchModule = MDMCenterModules.Search;

        #endregion

        #region Diagnostics related variables

        private readonly IDistributedCache _distributedCacheManager = CacheFactory.GetDistributedCache();
        private Boolean _wasOperationTracingInitiatedByUser = false;
        private Boolean _isRuntimeDiagnosticsOn = false;
        private Boolean _wasRuntimeDiagnosticsEverOn = false;

        #endregion

        #endregion Private Fields

        #region Constructors

        public EntityImportEngine()
        {
            _traceSettings = MDMOperationContextHelper.GetCurrentTraceSettings();
            _wasOperationTracingInitiatedByUser = (_traceSettings.TracingMode == TracingMode.OperationTracing);
        }

        #endregion

        #region Properties

        #endregion

        #region Methods

        #region Public Methods

        #region IImportEngine Members

        /// <summary>
        /// 
        /// </summary>
        /// <param name="job"></param>
        /// <param name="importProfile"></param>
        /// <returns></returns>
        public Boolean Initialize(Job job, ImportProfile importProfile)
        {
            Boolean isTracingEnabled = _traceSettings.IsBasicTracingEnabled;
            DiagnosticActivity activity = null;
            if (isTracingEnabled)
            {
                activity = new DiagnosticActivity();
                activity.Start();
            }

            if (job == null)
            {
                if (isTracingEnabled)
                {
                    activity.LogError("Parameter is null: job");
                }
                return false;
            }
            else
            {
                if (isTracingEnabled)
                    activity.LogMessageWithData("Job configuration", job.ToXml());
            }

            _batchesToTrace = 0;
            if (job.JobData.JobParameters.Contains("BatchesToTrace"))
            {
                JobParameter jobParam = job.JobData.JobParameters["BatchesToTrace"];
                _batchesToTrace = ValueTypeHelper.Int32TryParse(jobParam.Value, 0);

                if (_batchesToTrace > 0 && BATCHES_TO_TRACE_THRESHOLD > 0 && _batchesToTrace > BATCHES_TO_TRACE_THRESHOLD)
                {
                    if (isTracingEnabled)
                    {
                        activity.LogWarning(String.Format(
                            "Specified number of batches ({0}) to trace exceeded the set threshold ({1}), defaulting to threshold value",
                            _batchesToTrace, BATCHES_TO_TRACE_THRESHOLD));
                    }

                    _batchesToTrace = BATCHES_TO_TRACE_THRESHOLD;
                }
            }

            if (importProfile == null)
            {
                if (isTracingEnabled)
                {
                    activity.LogError("Parameter is null: importProfile");
                }
                return false;
            }
            else
            {
                if (isTracingEnabled)
                    activity.LogMessageWithData("Import profile configuration", importProfile.ToXml());
            }

            this.job = job;

            this.importProfile = importProfile;

            this.importProgram = String.Format("Imports, Profile: {0}, Job Id: {1}", importProfile.Name, job.Id);

            this.callerContextWithJobId = new CallerContext(application, importmodule, importProgram, job.Id, job.ProfileId, job.ProfileName);

            _entityDataSourceTrackingConfig = MDMFeatureConfigHelper.GetMDMFeatureConfig(
                MDMCenterApplication.DataQualityManagement,
                "Entity data source tracking", "1"
            );

            if (_entityDataSourceTrackingConfig != null && _entityDataSourceTrackingConfig.IsEnabled)
            {
                SetImportSource();
            }

            CheckAndCreateEventLogHandler();

            dataService = new DataService(WCFClientConfiguration.GetConfiguration(MDMWCFServiceList.DataService, job.CreatedUser));

            
            _knowledgeBaseService = new KnowledgeBaseService(WCFClientConfiguration.GetConfiguration(MDMWCFServiceList.KnowledgeBaseService, job.CreatedUser));

            try
            {
                categoryPathSeparator = AppConfigurationHelper.GetAppConfig("Catalog.Category.PathSeparator", categoryPathSeparator);
            }
            catch
            {
                if (isTracingEnabled)
                    activity.LogInformation("Could not load AppConfig: Catalog.Category.PathSeparator");
            }

            try
            {
                isUOMsCaseSensitive = AppConfigurationHelper.GetAppConfig<Boolean>("MDMCenter.UomManager.CaseSensitiveUomNames.Enabled", false);
            }
            catch
            {
                if (isTracingEnabled)
                    activity.LogInformation("Could not load AppConfig: MDMCenter.UomManager.CaseSensitiveUomNames.Enabled");
            }

            LoadLocalAttributeType();

            if (!CreateAuditReference())
            {
                if (isTracingEnabled)
                {
                    activity.LogError("Could not create audit reference");
                }
                return false;
            }

            if (isTracingEnabled)
            {
                activity.LogInformation(string.Format("Initialize completed for job Id ({0})", job.Id));
                activity.Stop();
            }

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="stepName"></param>
        /// <param name="stepConfiguration"></param>
        /// <param name="sourceData"></param>
        /// <returns></returns>
        public Boolean RunStep(String stepName, StepConfiguration stepConfiguration, IImportSourceData source)
        {
            Boolean isTracingEnabled = _traceSettings.IsBasicTracingEnabled;
            DiagnosticActivity activity = null;
            if (isTracingEnabled)
            {
                activity = new DiagnosticActivity();
                activity.Start();
            }

            IEntityImportSourceData sourceData = (IEntityImportSourceData)source;

            Boolean successFlag = true;

            #region Parameter validation

            if (stepConfiguration == null)
            {
                if (isTracingEnabled)
                {
                    activity.LogError("Parameter is null: stepConfiguration");
                }
                return false;
            }

            if (sourceData == null)
            {
                if (isTracingEnabled)
                {
                    activity.LogError("Parameter is null: sourceData");
                }
                return false;
            }

            #endregion

            #region Set instance variables

            this.stepConfiguration = stepConfiguration;

            this.sourceData = sourceData;

            this.importProcessingType = importProfile.ProcessingSpecifications.ImportProcessingType;

            this.importSourceType = importProfile.InputSpecifications.Reader;

            if (this.importSourceType == ImportSourceType.RSExcel12)
            {
                this.errorMessagePrefix = localeMessageBL.Get(systemUILocale, "112019", false, callerContext).Message;
            }
            else if (this.importSourceType == ImportSourceType.StagingDB10)
            {
                this.errorMessagePrefix = localeMessageBL.Get(systemUILocale, "112020", false, callerContext).Message;
            }
            else // Consider all other readers have line no for now
            {
                this.errorMessagePrefix = localeMessageBL.Get(systemUILocale, "112019", false, callerContext).Message;
            }

            this.entityProcessingOptions = importProfile.ProcessingSpecifications.EntityProcessingOptions;

            this.jobResultSaveSuccessEntities = importProfile.ProcessingSpecifications.JobProcessingOptions.SaveSuccessEntitiesResult;

            this.jobImportResultHandler = new JobImportResultHandler(job.Id);

            jobImportResultHandler.ProgramName = importProgram;
            jobImportResultHandler.UserName = importUser;
            jobImportResultHandler.AuditRefId = auditRefId;

            jobImportResultHandler.ProgressHandler = progressHandler;
            jobImportResultHandler.RelationshipProgressHandler = relationshipProgressHandler;

            sourceData.JobResultHandler = jobImportResultHandler;

            entityProcessingOptions.ProcessSources = true;

            #endregion

            #region Initialize Parameters

            // Initialize local configuration from app config file
            InitializeThreadCounts();

            // Initializes the local configuration variables based on the import source.
            InitializeLocalConfiguration();

            #endregion

            #region Get CachedDataModel

            // First Get the cached data model with all the known meta data and data definitions. This way we can avoid hitting the database.
            long startTime = DateTime.Now.Ticks;

            try
            {
                cachedDataModel = CachedDataModel.GetSingleton(false);

                if (cachedDataModel != null)
                    uoms = cachedDataModel.LoadUOMCollection();

                // Set this after the singleton is initialized...this is only required for first time..
                systemUILocale = GlobalizationHelper.GetSystemUILocale();
                systemDataLocale = GlobalizationHelper.GetSystemDataLocale();

                if (isTracingEnabled)
                    activity.LogDurationInfo("Get CacheDataModel completed.");

            }
            catch (Exception ex)
            {
                String message = "Failed to load data model cache in the import engine. Job would be aborted.";

                UpdateJobStatus(JobStatus.Aborted, message);

                if (isTracingEnabled)
                {
                    message = String.Format("Job Id {0} - {1}. Internal Error: {2}", job.Id, message, ex.Message);

                    activity.LogError(message);
                }
                return false;
            }

            long endCacheTime = DateTime.Now.Ticks;
            loadCacheTime = new TimeSpan(endCacheTime - startTime).TotalSeconds;

            #endregion

            #region Prepare Import Profile object for execution

            InitializeImportProfileProperties();

            String dqmInitializeMessage;

            if (InitializeDQMJobId(out dqmInitializeMessage) == false)
            {
                UpdateJobStatus(JobStatus.Aborted, dqmInitializeMessage);

                if (isTracingEnabled)
                {
                    dqmInitializeMessage = String.Format("Job Id {0} - {1}", job.Id, dqmInitializeMessage);

                    activity.LogError(dqmInitializeMessage);
                }
                return false;
            }

            #endregion

            #region Process

            Process();

            long endWorkTime = DateTime.Now.Ticks;
            totalTime = new TimeSpan(endWorkTime - startTime).TotalSeconds;

            #endregion

            #region Record Results

            //only when the job is running, we will make it complete... all other will stay as it is
            if (job.JobStatus == JobStatus.Running)
            {
                JobStatus finalStatus = JobStatus.Completed;

                if (importProfile.ProcessingSpecifications.ImportMode != ImportMode.RelationshipLoad &&
                    importProfile.ProcessingSpecifications.ImportMode != ImportMode.RelationshipInitialLoad)
                {
                    finalStatus = Helpers.GetJobStatus(progressHandler.GetSuccessFulEntities(), progressHandler.GetFailedEntities(), progressHandler.GetPartialSuccessFulEntities());
                }
                else
                {
                    finalStatus = Helpers.GetJobStatus(relationshipProgressHandler.GetSuccessFulEntities(), relationshipProgressHandler.GetFailedEntities(), relationshipProgressHandler.GetPartialSuccessFulEntities());
                }

                UpdateJobStatus(finalStatus);

                if (finalStatus == JobStatus.CompletedWithErrors || finalStatus == JobStatus.CompletedWithWarnings || finalStatus == JobStatus.CompletedWithWarningsAndErrors)
                {
                    // Create and store errored rows
                    ProcessErroredDocument(finalStatus);
                }
            }

            string startMessage = string.Format("Processing done for job {0}. The overall time it took is {1,2:F} seconds.", job.Id.ToString(), totalTime);
            LogHandler.WriteInformationLog(startMessage, 0);
            Console.WriteLine(startMessage);

            if (Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["RecordRunResultsUsingSP"]) == true)
            {
                startMessage = "Recording the results of the run now. Be patient we are almost done.";
                Console.WriteLine(startMessage);
                LogHandler.WriteInformationLog(startMessage, 0);

                if (!RecordResults(totalTime))
                {
                }
            }

            #endregion

            if (isTracingEnabled) activity.Stop();

            return successFlag;
        }

        private void ProcessErroredDocument(JobStatus finalStatus)
        {
            string message = string.Empty;
            Boolean isTracingEnabled = _traceSettings.IsBasicTracingEnabled;
            DiagnosticActivity activity = null;
            if (isTracingEnabled)
            {
                activity = new DiagnosticActivity();
                activity.Start();
            }

            if (job.JobData.JobParameters["FileId"] != null && (importProfile.InputSpecifications.Reader == ImportSourceType.RSExcel12 || importProfile.InputSpecifications.Reader == ImportSourceType.Generic10 || importProfile.InputSpecifications.Reader == ImportSourceType.Generic11 || importProfile.InputSpecifications.Reader == ImportSourceType.Generic12))
            {
                /*Something wrong here, issue is in the multi-threading
                if (_errorCollection == null)
                {
                    while (_errorCollection == null) { }
                }*/

                // Read from Job OR and add in to _errorNWarningCollection
                _errorNWarningCollection = new EntityOperationResultCollection();

                // Read from Job and add in to _relationshipErrorNWarningCollection
                _relationshipErrorNWarningCollection = new RelationshipOperationResultCollection();
                IntegrationService integrationService = new IntegrationService();
                JobImportResultCollection errorStates = integrationService.GetErrorStates(this.job.Id, MDMCenterApplication.MDMCenter);
                if (errorStates != null)
                {
                    foreach (JobImportResult result in errorStates)
                    {
                        EntityOperationResult eor = new EntityOperationResult(result.OperationResultXML);
                        if (eor.OperationResultStatus == OperationResultStatusEnum.Failed ||
                            eor.OperationResultStatus == OperationResultStatusEnum.CompletedWithErrors ||
                            eor.OperationResultStatus == OperationResultStatusEnum.CompletedWithWarnings)
                        {
                            _errorNWarningCollection.Add(eor);

                            if (eor.RelationshipOperationResultCollection != null && eor.RelationshipOperationResultCollection.Count > 0)
                            {
                                foreach (RelationshipOperationResult relationshipOperationResult in eor.RelationshipOperationResultCollection)
                                {
                                    if (relationshipOperationResult.OperationResultStatus == OperationResultStatusEnum.Failed ||
                                     relationshipOperationResult.OperationResultStatus == OperationResultStatusEnum.CompletedWithErrors ||
                                     relationshipOperationResult.OperationResultStatus == OperationResultStatusEnum.CompletedWithWarnings)
                                    {
                                        _relationshipErrorNWarningCollection.Add(relationshipOperationResult);
                                    }
                                }
                            }
                        }
                    }
                }

                var sourceFileIdString = job.JobData.JobParameters["FileId"].Value;
                var sourceFileId = Int32.Parse(sourceFileIdString);

                var fileBl = new FileBL();
                var sourceFile = fileBl.GetFile(sourceFileId, false);
                byte[] sourceFileData = sourceFile.FileData;

                // Create stream
                using (var ms = new MemoryStream())
                {
                    ms.Write(sourceFileData, 0, sourceFileData.Length);

                    try
                    {
                        using (var document = OpenXml.Packaging.SpreadsheetDocument.Open(ms, true))
                        {
                            Worksheet entityWorkSheet = null, relationshipWorkSheet = null;

                            // Reads "Entities" sheet data
                            entityWorkSheet = ReadExcelData(document, finalStatus, RSExcelConstants.EntityDataSheetName);

                            // Reads "Relationships" sheet data
                            relationshipWorkSheet = ReadExcelData(document, finalStatus, RSExcelConstants.RelationshipSheetName);

                            if (entityWorkSheet != null || relationshipWorkSheet != null)
                            {
                                // Get result data
                                ms.Position = 0;
                                var processedFileData = new byte[ms.Length];
                                ms.Read(processedFileData, 0, (int)ms.Length);

                                // Insert '_Errors' before file extension
                                var erroredFileName = sourceFile.Name;
                                var errorString = localeMessageBL.Get(GlobalizationHelper.GetSystemDataLocale(), "112086", false, new CallerContext(application, importmodule)).Message; //"_Errors"

                                erroredFileName = erroredFileName.Insert(sourceFile.Name.Length - 5, errorString);

                                // Create file
                                var file = new BusinessObjects.File
                                {
                                    Name = erroredFileName,
                                    FileData = processedFileData,
                                    IsArchive = false,
                                    Action = ObjectAction.Create
                                };

                                CallerContext callerContext = new CallerContext(MDMCenterApplication.JobService, Utility.GetModule(job.JobType), "JobService.ThreadPool.RunJob");
                                // Store file into the database
                                var fileId = fileBl.Process(file, callerContext);

                                // Add errored file to the Job
                                job.JobData.JobParameters.Add(new JobParameter("ErroredFileId", fileId.ToString(CultureInfo.InvariantCulture)));

                                JobBL jobBl = new JobBL();
                                jobBl.Update(job, callerContext);
                            }
                            else
                            {
                                if (isTracingEnabled)
                                {
                                    activity.LogWarning(String.Format("EntityImportEngine - Failed to generate errored document for job id : '{0}'. 'Entities' sheet is not available in source file.", job.Id));
                                }
                            }
                        }
                    }
                    catch (FileFormatException exception)
                    {
                        // Case when we are trying to generate document with errors based on *.xls file
                        // OpenXML provide functionality to work only with *.xlsx files (xml-based)
                        if (isTracingEnabled)
                        {
                            activity.LogError("EntityImportEngine:ProcessErroredDocument " + exception.Message);
                        }
                    }
                }
            }

            if (isTracingEnabled) activity.Stop();
        }

        private List<uint> ProcessRows(OpenXml.Packaging.WorkbookPart workbookPart, SheetData sheetData, uint cellFormatId, JobStatus finalStatus, String excelSheetName, Worksheet worksheet)
        {
            string message = string.Empty;
            Boolean isTracingEnabled = _traceSettings.IsBasicTracingEnabled;
            DiagnosticActivity activity = null;
            if (isTracingEnabled)
            {
                activity = new DiagnosticActivity();
                activity.Start();
            }

            var rowsWithoutErrors = new List<uint>();

            Boolean errorsArePresent = finalStatus == JobStatus.CompletedWithErrors || finalStatus == JobStatus.CompletedWithWarningsAndErrors;
            Boolean warningsArePresent = finalStatus == JobStatus.CompletedWithWarnings || finalStatus == JobStatus.CompletedWithWarningsAndErrors;

            Boolean hasErrorsColumn = false;
            Boolean hasWarningsColumn = false;

            UInt32 errorsColumnIndex = 0;
            UInt32 warningsColumnIndex = 0;

            foreach (OpenXml.Spreadsheet.Row row in sheetData)
            {
                if (row.RowIndex == 1)
                {
                    UInt32 rowCount = (UInt32)row.Count();
                    OpenXml.Spreadsheet.Cell lastCell = row.Last() as OpenXml.Spreadsheet.Cell;

                    // Identifying type of last column header - general attribute / errors column / warnings column
                    if (lastCell != null && !string.IsNullOrWhiteSpace(lastCell.InnerText))
                    {
                        hasWarningsColumn = IsCellValueEqual(workbookPart, lastCell, RSExcelConstants.WarningsColumnName);
                        hasErrorsColumn = IsCellValueEqual(workbookPart, lastCell, RSExcelConstants.ErrorsColumnName);

                        // Checking cell before last value
                        if (hasWarningsColumn && !hasErrorsColumn)
                        {
                            // Getting Previous cell
                            OpenXml.Spreadsheet.Cell previousCell = lastCell.ElementsBefore().Last() as OpenXml.Spreadsheet.Cell;
                            hasErrorsColumn = IsCellValueEqual(workbookPart, previousCell, RSExcelConstants.ErrorsColumnName);
                        }
                    }

                    if (hasErrorsColumn)
                    {
                        errorsColumnIndex = hasWarningsColumn ? rowCount - 1 : rowCount;
                    }
                    else
                    {
                        errorsColumnIndex = hasWarningsColumn ? rowCount : rowCount + 1;
                    }

                    warningsColumnIndex = errorsArePresent ? errorsColumnIndex + 1 : errorsColumnIndex;

                    // Removing old header cells
                    OpenSpreadsheetUtility.RemoveDataCell(sheetData, errorsColumnIndex, row.RowIndex);
                    OpenSpreadsheetUtility.RemoveDataCell(sheetData, warningsColumnIndex, row.RowIndex);


                    var firstHeaderCell = row.First() as OpenXml.Spreadsheet.Cell;
                    var firstCellStyleIndex = OpenSpreadsheetUtility.GetCellStyleIndex(firstHeaderCell);

                    if (errorsArePresent)
                    {
                        OpenSpreadsheetUtility.AppendRowWithTextCell(row, errorsColumnIndex, RSExcelConstants.ErrorsColumnName, OpenXml.SpaceProcessingModeValues.Preserve, firstCellStyleIndex);
                    }

                    if (warningsArePresent)
                    {
                        OpenSpreadsheetUtility.AppendRowWithTextCell(row, warningsColumnIndex, RSExcelConstants.WarningsColumnName, OpenXml.SpaceProcessingModeValues.Preserve, firstCellStyleIndex);
                    }

                    OpenXml.Spreadsheet.Columns columns = worksheet.GetFirstChild<OpenXml.Spreadsheet.Columns>();

                    // Set error and warnings column width as 50
                    if (columns != null && columns.Count() > 0)
                    {
                        foreach (OpenXml.Spreadsheet.Column errorOrWarningColumn in columns)
                        {
                            if (errorOrWarningColumn.Min == errorsColumnIndex || errorOrWarningColumn.Min == warningsColumnIndex)
                            {
                                errorOrWarningColumn.Width = 50;
                            }
                        }
                    }
                }
                else
                {
                    String error = String.Empty, warning = String.Empty;

                    if (excelSheetName == RSExcelConstants.EntityDataSheetName)
                    {
                        var errorEntity = (EntityOperationResult)_errorNWarningCollection.GetByReferenceId(row.RowIndex);

                        if (errorEntity == null)
                        {
                            // Create list of rows for deletion
                            rowsWithoutErrors.Add(row.RowIndex);
                        }
                        else
                        {
                            // Get error message for entity
                            error = GetEntityImportError(errorEntity);
                            warning = GetEntityImportWarning(errorEntity);

                            AddErrorAndWarningForExcelRow(row, sheetData, error, errorsColumnIndex, warning, warningsColumnIndex, cellFormatId);
                        }
                    }
                    else if (excelSheetName == RSExcelConstants.RelationshipSheetName)
                    {
                        var errorRelationship = (RelationshipOperationResult)_relationshipErrorNWarningCollection.GetByReferenceId(row.RowIndex);

                        if (errorRelationship == null)
                        {
                            // Create list of rows for deletion
                            rowsWithoutErrors.Add(row.RowIndex);
                        }
                        else
                        {
                            // Get error message for relationships
                            error = GetRelationshipImportError(errorRelationship);
                            warning = GetRelationshipImportWarning(errorRelationship);

                            AddErrorAndWarningForExcelRow(row, sheetData, error, errorsColumnIndex, warning, warningsColumnIndex, cellFormatId);
                        }
                    }
                }
            }

            if (isTracingEnabled) activity.Stop();

            return rowsWithoutErrors;
        }

        private Boolean IsCellValueEqual(OpenXml.Packaging.WorkbookPart workbookPart, OpenXml.Spreadsheet.Cell cell, String valueToCheck)
        {
            String sharedStringText = String.Empty;
            Int32 sharedStringId = 0;
            if (Int32.TryParse(cell.InnerText, out sharedStringId))
            {
                var sharedString = OpenSpreadsheetUtility.GetSharedStringItemById(workbookPart, sharedStringId);
                if (sharedString != null)
                {
                    sharedStringText = sharedString.InnerText;
                }
            }

            Boolean result = cell.InnerText.Equals(valueToCheck, StringComparison.InvariantCultureIgnoreCase) ||
                sharedStringText.Equals(valueToCheck, StringComparison.InvariantCultureIgnoreCase);

            return result;
        }

        private String GetEntityImportError(EntityOperationResult entityOperationResult)
        {
            var errors = entityOperationResult.GetErrors();
            var stringBuilder = new StringBuilder();

            if (errors.Count > 0)
            {
                foreach (var error in errors)
                {
                    stringBuilder.AppendFormat("• {0}\n ", GetErrorMessage(error));
                }
            }

            foreach (AttributeOperationResult aor in entityOperationResult.AttributeOperationResultCollection)
            {
                foreach (Error error in aor.Errors)
                {
                    stringBuilder.AppendFormat("• {0}\n ", GetErrorMessage(error));
                }
            }

            return stringBuilder.ToString();
        }

        private String GetEntityImportWarning(EntityOperationResult entityOperationResult)
        {
            var warnings = entityOperationResult.GetWarnings();
            var stringBuilder = new StringBuilder();

            if (warnings.Count > 0)
            {
                foreach (var warning in warnings)
                {
                    stringBuilder.AppendFormat("• {0}\n ", GetWarningMessage(warning));
                }
            }

            foreach (AttributeOperationResult aor in entityOperationResult.AttributeOperationResultCollection)
            {
                foreach (Warning warning in aor.Warnings)
                {
                    stringBuilder.AppendFormat("• {0}\n ", GetWarningMessage(warning));
                }
            }

            return stringBuilder.ToString();
        }

        private String GetRelationshipImportError(RelationshipOperationResult relationshipOperationResult)
        {
            var errors = relationshipOperationResult.GetErrors();
            var stringBuilder = new StringBuilder();

            if (errors.Count > 0)
            {
                foreach (var error in errors)
                {
                    stringBuilder.AppendFormat("• {0}\n ", error.ErrorMessage);
                }
            }

            foreach (AttributeOperationResult aor in relationshipOperationResult.AttributeOperationResultCollection)
            {
                foreach (Error error in aor.Errors)
                {
                    stringBuilder.AppendFormat("• {0}\n ", error.ErrorMessage);
                }
            }

            return stringBuilder.ToString();
        }

        private String GetRelationshipImportWarning(RelationshipOperationResult relationshipOperationResult)
        {
            var warnings = relationshipOperationResult.GetWarnings();
            var stringBuilder = new StringBuilder();

            if (warnings.Count > 0)
            {
                foreach (var warning in warnings)
                {
                    stringBuilder.AppendFormat("• {0}\n ", warning.WarningMessage);
                }
            }

            foreach (AttributeOperationResult aor in relationshipOperationResult.AttributeOperationResultCollection)
            {
                foreach (Warning warning in aor.Warnings)
                {
                    stringBuilder.AppendFormat("• {0}\n ", warning.WarningMessage);
                }
            }

            return stringBuilder.ToString();
        }

        /// <summary>
        /// Gets error message based on code if messsage is empty
        /// </summary>
        private String GetErrorMessage(IError error)
        {
            String message = error.ErrorMessage;

            if (String.IsNullOrWhiteSpace(message))
            {
                LocaleMessage localeMessage = localeMessageBL.Get(systemUILocale, error.ErrorCode, error.Params.ToArray(), false, callerContext);
                if (localeMessage != null)
                {
                    message = localeMessage.Message;
                }
            }

            return message;
        }

        /// <summary>
        /// Gets warning based on code if message is empty
        /// </summary>
        private String GetWarningMessage(IWarning warning)
        {
            String message = warning.WarningMessage;

            if (String.IsNullOrWhiteSpace(message))
            {
                LocaleMessage localeMessage = localeMessageBL.Get(systemUILocale, warning.WarningCode, warning.Params.ToArray(), false, callerContext);
                if (localeMessage != null)
                {
                    message = localeMessage.Message;
                }
            }

            return message;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Job GetCurrentJob()
        {
            return job;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ImportProfile GetCurrentImportProfile()
        {
            return importProfile;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ExecutionStatus GetExecutionStatus()
        {
            return job.JobData.ExecutionStatus;
        }

        #endregion

        #endregion

        #region Private Methods

        #region Setup and Config Methods

        /// <summary>
        /// Splits the given range of entities across the available number of entity threads. Any spill over is assigned to the last thread.
        /// </summary>
        /// <param name="numberOfThreads"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="entitybatchPerThread"></param>
        /// <returns>Dictionary containing the start number for each thread. The end number is the start number of the next thread. So the dictionary
        /// will have one more entry than the number of threads.</returns>
        private Dictionary<Int32, Int64> SplitEntiesForThreads(Int32 numberOfThreads, Int64 start, Int64 end, Int64 entitybatchPerThread)
        {
            Dictionary<int, Int64> threadBatchStart = new Dictionary<int, Int64>(numberOfThreads + 1);

            for (int i = 0; i < numberOfThreads; i++)
            {
                threadBatchStart[i] = start + i * entitybatchPerThread;
            }
            // the last thread gets its batch and the spill over..
            threadBatchStart[numberOfThreads] = end + 1;

            return threadBatchStart;
        }

        /// <summary>
        /// Initializes the processing thread counts.
        /// See if the setting is available in the profile. If not get it from the config file. Otherwise use the default value.
        /// </summary>
        private void InitializeThreadCounts()
        {
            Boolean isTracingEnabled = _traceSettings.IsBasicTracingEnabled;
            DiagnosticActivity activity = null;
            if (isTracingEnabled)
            {
                activity = new DiagnosticActivity();
                activity.Start();
            }

            this.numberOfEntitiesThreads = importProfile.ProcessingSpecifications.JobProcessingOptions.NumberofEntityThreads;

            if (this.numberOfEntitiesThreads <= 0)
            {
                this.numberOfEntitiesThreads = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["ImportEngine.EntityThreadsPerJob.Size"]);
            }
            if (this.numberOfEntitiesThreads <= 0)
            {
                this.numberOfEntitiesThreads = 1;
            }

            this.numberOfAttributesThreadPerEntity = importProfile.ProcessingSpecifications.JobProcessingOptions.NumberofAttributeThreadsPerEntityThread;

            if (this.numberOfAttributesThreadPerEntity <= 0)
            {
                this.numberOfAttributesThreadPerEntity = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["ImportEngine.AttributeThreadsPerEntityThread.Size"]);
            }

            if (this.numberOfAttributesThreadPerEntity <= 0)
            {
                this.numberOfAttributesThreadPerEntity = 1;
            }

            this.batchSize = importProfile.ProcessingSpecifications.JobProcessingOptions.BatchSize;

            if (this.batchSize <= 0)
            {
                batchSize = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["ImportEngine.EntityProcessing.BatchSize"]);
            }

            if (batchSize <= 0)
            {
                this.batchSize = 1;
            }

            if (isTracingEnabled)
            {
                string message = string.Format("Thread configuration - entity threads ({0}), attribute threads ({1}), batch size ({2})",
                    numberOfEntitiesThreads, numberOfAttributesThreadPerEntity, batchSize);

                activity.LogInformation(message);
                activity.Stop();
            }
        }

        /// <summary>
        /// Initialize some of the local settings based on the import source type
        /// </summary>
        private void InitializeLocalConfiguration()
        {
            //Compute how many entities per thread we need. This is only needed for source types that can be batched or randmly read.
            switch (sourceData.GetBatchingType())
            {
                case ImportProviderBatchingType.Multiple:
                    InitializeMultipleBatchConfiguration();
                    break;
                case ImportProviderBatchingType.Single:
                    InitializeSingleBatchConfiguration();
                    break;
                case ImportProviderBatchingType.None:
                    InitializeDefaultConfiguration();
                    break;
            }
        }

        /// <summary>
        /// Initialize for database type providers configuration
        /// </summary>
        private void InitializeMultipleBatchConfiguration()
        {
            Boolean isTracingEnabled = _traceSettings.IsBasicTracingEnabled;
            DiagnosticActivity activity = null;
            if (isTracingEnabled)
            {
                activity = new DiagnosticActivity();
                activity.Start();
            }

            if (sourceData.GetEntityDataBatchSize() > 0)
                batchSize = sourceData.GetEntityDataBatchSize();

            Int64 seed = sourceData.GetEntityDataSeed();
            Int64 endMark = sourceData.GetEntityEndPoint();
            Int64 entityCount = sourceData.GetEntityDataCount();

            // if we dont have enough entities..just use 1 thread..
            if (entityCount < numberOfEntitiesThreads)
                numberOfEntitiesThreads = 1;
            entitybatchPerThread = (entityCount) / numberOfEntitiesThreads;

            if (entitybatchPerThread == 0)
            {
                if (isTracingEnabled)
                {
                    activity.LogWarning("Entities per batch is zero. Initial load has no work to do.");
                }
            }

            entitySplit = SplitEntiesForThreads(numberOfEntitiesThreads, seed, endMark, entitybatchPerThread);

            if (isTracingEnabled)
            {
                string message = string.Format("Batch configuration ({0}) for provider ({1}) - total entities to process ({2}), batch size ({3}), entities per thread ({4})",
                    "Single", importSourceType.ToString(), entityCount, batchSize, entitybatchPerThread);
                activity.LogInformation(message);
                activity.Stop();
            }
        }

        /// <summary>
        /// Initialize File based provider that supports batching
        /// </summary>
        private void InitializeSingleBatchConfiguration()
        {
            Boolean isTracingEnabled = _traceSettings.IsBasicTracingEnabled;
            DiagnosticActivity activity = null;
            if (isTracingEnabled)
            {
                activity = new DiagnosticActivity();
                activity.Start();
            }

            if (sourceData.GetEntityDataBatchSize() > 0)
                batchSize = sourceData.GetEntityDataBatchSize();

            // just some default values
            entitySplit = SplitEntiesForThreads(numberOfEntitiesThreads, 0, 0, 1);

            if (isTracingEnabled)
            {
                string message = string.Format("Batch configuration (Single) for provider ({0}) -  batch size ({1})",
                    importSourceType.ToString(), batchSize);
                activity.LogInformation(message);
                activity.Stop();
            }
        }

        /// <summary>
        /// Initialize File based provider that does not support batching
        /// </summary>
        private void InitializeDefaultConfiguration()
        {
            string message = string.Empty;

            Boolean isTracingEnabled = _traceSettings.IsBasicTracingEnabled;
            DiagnosticActivity activity = null;
            if (isTracingEnabled)
            {
                activity = new DiagnosticActivity();
                activity.Start();
            }

            if (sourceData.GetEntityDataBatchSize() > 0)
                batchSize = sourceData.GetEntityDataBatchSize();

            // these providers do not support multiple batches. So need for multiple threads.
            if (numberOfEntitiesThreads > 1)
            {
                numberOfEntitiesThreads = 1;

                if (isTracingEnabled)
                {
                    message = "The source data provider does not support batching. So number of entity threads is defaulted to 1.";
                    activity.LogWarning(message);
                }
            }

            Int64 entityCount = sourceData.GetEntityDataCount();

            // if we dont have enough entities..just use 1 thread..
            if (entityCount < numberOfEntitiesThreads)
                numberOfEntitiesThreads = 1;
            entitybatchPerThread = (entityCount) / numberOfEntitiesThreads;

            // just some default values
            entitySplit = SplitEntiesForThreads(numberOfEntitiesThreads, 0, 0, 1);

            if (isTracingEnabled)
            {
                message = string.Format("Batch configuration ({0}) for provider ({1}) - total entities to process ({2}), batch size ({3}), entities per thread ({4})",
                    "Default", importSourceType.ToString(), entityCount, batchSize, entitybatchPerThread);

                activity.LogInformation(message);
            }

            if (isTracingEnabled) activity.Stop();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private bool CheckAndCreateEventLogHandler()
        {
            bool result = true;

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
        /// 
        /// </summary>
        private void LoadLocalAttributeType()
        {
            availableAttributeTypes.Add(AttributeModelType.Common);
            availableAttributeTypes.Add(AttributeModelType.Category);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private bool CreateAuditReference()
        {
            bool bsuccess = true;

            try
            {
                AuditInfoBL auditBL = new AuditInfoBL();
                auditRefId = auditBL.Create(importUser, importProgram, true);
            }
            catch (Exception ex)
            {
                LogHandler.WriteErrorLog(ex.Message, 100);
                bsuccess = false;
            }

            if (auditRefId <= 0)
            {
                // cannot create audit ref??
                LogHandler.WriteErrorLog("Error occurred while getting an audit reference for the current user. Stop processing the imports.", 100);
                bsuccess = false;
            }
            return bsuccess;
        }

        /// <summary>
        /// Initialize the list of entity types before doing the load.
        /// </summary>
        private void InitializeEntityTypesList()
        {
            string message = string.Empty;
            Boolean isTracingEnabled = _traceSettings.IsBasicTracingEnabled;
            DiagnosticActivity activity = null;
            if (isTracingEnabled)
            {
                activity = new DiagnosticActivity();
                activity.Start();
            }

            // Get the pipe separated values from the profile..
            String entityTypesFromProfile = importProfile.ProcessingSpecifications.EntityTypeList;

            if (String.IsNullOrEmpty(entityTypesFromProfile) == true)
            {
                return;
            }

            // split and see what we have
            String[] stringList = entityTypesFromProfile.Split('|');

            if (stringList.Count() <= 0)
            {
                return;
            }

            // Loop through andmake sure we load them in the same order and they are valid.
            foreach (String entityTypeName in stringList)
            {
                EntityType entityType = GetEntityTypeByName(entityTypeName);

                if (entityType != null)
                {
                    entityTypeList.Add(entityTypeList.Count(), entityType);
                }
                else
                {
                    if (isTracingEnabled)
                    {
                        activity.LogError(String.Format("The entity type {0} specified in the profile is not valid. It will be removed from processing", entityTypeName));
                    }
                }
            }

            if (isTracingEnabled) activity.Stop();
        }

        /// <summary>
        /// Initialize the list of MDL containers based on the profile organization
        /// </summary>
        /// <returns></returns>
        private void InitializeMDLContainerList()
        {
            String message = string.Empty;

            Boolean isTracingEnabled = _traceSettings.IsBasicTracingEnabled;
            DiagnosticActivity activity = null;
            if (isTracingEnabled)
            {
                activity = new DiagnosticActivity();
                activity.Start();
            }

            if (isTracingEnabled)
            {
                message = "Initializing the list of MDL containers that need to be processed first.";
                activity.LogInformation(message);
            }

            containers = cachedDataModel.GetContainers();

            if (containers != null && containers.Count > 0)
            {
                IOrderedEnumerable<Container> sortedContainers = containers.OrderBy(container => container.Level);

                if (sortedContainers != null)
                {
                    Int32 counter = 0;

                    foreach (Container container in sortedContainers)
                    {
                        if (container.IsApproved)
                        {
                            continue;
                        }

                        mdlcontainerList.Add(counter, container);
                        counter++;
                    }
                }
            }

            if (mdlcontainerList == null || mdlcontainerList.Count <= 0)
            {
                if (isTracingEnabled)
                {
                    message = string.Format("No Containers are available for extension processing. Regular entity processing will be performed.");
                    activity.LogWarning(message);
                }
            }
            else
            {
                if (isTracingEnabled)
                {
                    message = string.Format("{0} containers are available for extension processing. Entity processing will be performed for each one of them.", mdlcontainerList.Count);
                    activity.LogInformation(message);
                }
            }

            if (isTracingEnabled)
            {
                message = string.Format("Initialized {0} MDL containers.", mdlcontainerList.Count);
                activity.LogInformation(message);
                activity.Stop();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void InitializeImportProfileProperties()
        {
            #region List down attribute sources to be expanded

            var duplicateAttributeSources = importProfile.MappingSpecifications.AttributeMaps.GroupBy(
                attributeMap => new { attributeMap.AttributeSource.Name },
                (key, group) => new { Name = key.Name, Count = group.Count() }
            ).Where(group => group.Count > 1);

            List<String> repeatedAttributeSourceNames = new List<String>();

            foreach (var attributeSource in duplicateAttributeSources)
            {
                repeatedAttributeSourceNames.Add(attributeSource.Name);
            }

            #endregion

            AttributeProcessingOptionsCollection attributeProcessingOptionsCollection = new AttributeProcessingOptionsCollection();
            //Denormalize input field names and come up with staging attribute info..
            foreach (AttributeMap attrMap in importProfile.MappingSpecifications.AttributeMaps)
            {
                String inputFieldName = attrMap.AttributeSource.Name;
                Attribute stagingAttribute = sourceData.GetAttributeInfoFromInputFieldName(inputFieldName);

                attrMap.AttributeSource.StagingAttributeInfo = stagingAttribute;

                if (repeatedAttributeSourceNames.Contains(inputFieldName))
                {
                    repeatedAttributeSources.Add(attrMap);
                }
                //The below code will check the Attribute Action flag from Attribute Target Node in Mapping Specification and assign the Add,Delete and Update Flags accordingly
                if (attrMap.AttributeTarget != null)
                {
                    //If all CanAddAttribute| CanUpdateAttribute|CanDeleteAttribute are true it will by default take the old processing flow
                    if (attrMap.AttributeTarget.CanAddAttribute && attrMap.AttributeTarget.CanUpdateAttribute && attrMap.AttributeTarget.CanDeleteAttribute)
                    {
                        continue;
                    }
                    else
                    {
                        AttributeProcessingOptions attributeProcessingOptions = new AttributeProcessingOptions();
                        attributeProcessingOptions.AttributeId = attrMap.AttributeTarget.Id;
                        attributeProcessingOptions.AttributeName = attrMap.AttributeTarget.Name;
                        attributeProcessingOptions.CanAddAttribute = attrMap.AttributeTarget.CanAddAttribute;
                        attributeProcessingOptions.CanUpdateAttribute = attrMap.AttributeTarget.CanUpdateAttribute;
                        attributeProcessingOptions.CanDeleteAttribute = attrMap.AttributeTarget.CanDeleteAttribute;
                        attributeProcessingOptions.AttributeModelType = attrMap.AttributeTarget.ModelType;
                        attributeProcessingOptionsCollection.Add(attributeProcessingOptions);
                    }
                }
            }
            entityProcessingOptions.AttributeProcessingOptionCollection = attributeProcessingOptionsCollection;

            #region Set job level properties
            if (importProfile.ProcessingSpecifications.JobProcessingOptions.AttributeValidationLevel == OperationResultType.Warning)
            {
                entityProcessingOptions.IsPartialAttributeprocessingEnabled = true;
            }
            if (importProfile.ProcessingSpecifications.JobProcessingOptions.RelationshipAttributeValidationLevel == OperationResultType.Warning)
            {
                entityProcessingOptions.IsPartialRelationshipAttributeProcessingEnabled = true;
            }
            if (importProfile.ProcessingSpecifications.JobProcessingOptions.RelationshipTypeValidationLevel == OperationResultType.Warning)
            {
                entityProcessingOptions.IsPartialRelationshipTypeProcessingEnabled = true;
            }
            #endregion


            entityProcessingOptions.ImportMode = importProfile.ProcessingSpecifications.ImportMode;

            // Fill the mandatory attribute list from the profile
            #region Set Partial Processing Properties

            FillPartialProcessingOptionsFromProfile();

            #endregion
        }

        /// <summary>
        /// Initializes the DQM job identifier.
        /// </summary>
        /// <param name="dqmInitializeMessage">The DQM initialize message.</param>
        /// <returns></returns>
        private Boolean InitializeDQMJobId(out String dqmInitializeMessage)
        {
            dqmInitializeMessage = "";
            return true;
        }

        #endregion

        #region Step 1: Import Engine Process Method

        /// <summary>
        /// The main process method for the entity engine.
        /// </summary>
        private void Process()
        {
            switch (importProfile.ProcessingSpecifications.ImportMode)
            {
                case ImportMode.ExtensionRelationshipLoad:
                    ProcessExtensionRelationships();
                    break;
                case ImportMode.EntityHierarchyLoad:
                    ProcessEntityHierarchyLoad();
                    break;
                case ImportMode.EntityExtensionRelationshipAndHierarchyLoad:
                    ProcessExtensionRelationshipAndHierarchyLoad();
                    break;
                default:
                    ProcessDefault();
                    break;
            }
        }

        /// <summary>
        /// Except the extension relationship ( MDL) every other import mode is processed using this method.
        /// </summary>
        private void ProcessDefault()
        {
            string message = string.Empty;

            Boolean isTracingEnabled = _traceSettings.IsBasicTracingEnabled;
            DiagnosticActivity activity = null;
            if (isTracingEnabled)
            {
                activity = new DiagnosticActivity();
                activity.Start();
                activity.LogInformation(string.Format("Spawning {0} threads to process entities.", numberOfEntitiesThreads));
            }

            // Get the list of containers to process in a sorted list. The ORDER is very important
            if (importProfile.ProcessingSpecifications.ImportMode == ImportMode.InitialLoad)
            {
                InitializeMDLContainerList();
            }

            try
            {
                // Parallel execute. Each task/thread is independent of other. This is simpler for our purpose and light weight than managing individual threads.
                RunInParallel(Process, entitySplit, numberOfEntitiesThreads, batchSize, numberOfAttributesThreadPerEntity);
            }
            catch (AggregateException aex)
            {
                foreach (var ex in aex.InnerExceptions)
                {
                    if (isTracingEnabled)
                    {
                        activity.LogError(ex.Message);
                    }
                }

                throw aex.InnerException;
            }
            catch (Exception ex)
            {
                if (isTracingEnabled)
                {
                    activity.LogError(ex.Message);
                }
                throw;
            }
            finally
            {
                if (isTracingEnabled) activity.Stop();
            }
            //parallel ends here..
        }

        /// <summary>
        /// Process the extension relationship. This processing will be done container by container to handle parent creation first.
        /// </summary>
        private void ProcessExtensionRelationships()
        {
            string message = string.Empty;
            Boolean isTracingEnabled = _traceSettings.IsBasicTracingEnabled;
            DiagnosticActivity activity = null;
            if (isTracingEnabled)
            {
                activity = new DiagnosticActivity();
                activity.Start();
            }

            // Get the list of containers to process in a sorted list. The ORDER is very important
            InitializeMDLContainerList();

            // Process container by container
            for (int ctr = 0; mdlcontainerList != null && ctr < mdlcontainerList.Count; ctr++)
            {
                Container container = mdlcontainerList.Values[ctr];

                if (isTracingEnabled)
                {
                    message = String.Format("Entity processing will be performed for container {0}. There are still {1} container left.",
                        container.Name, mdlcontainerList.Count - ctr - 1);
                    activity.LogInformation(message);
                }

                #region Run ProcessEntities in parallel

                RunInParallel(ProcessEntities, entitySplit, numberOfEntitiesThreads, batchSize, numberOfAttributesThreadPerEntity, container.Name);

                #endregion

                // At the end of each container reinitialize the source data...this is important for file based providers.
                sourceData.Initialize(job, importProfile);
            }

            if (isTracingEnabled)
            {
                message = String.Format("Containers level processing is done. Now the final entity processing for the rest of the containers will start.");
                activity.LogInformation(message);
            }

            #region Run ProcessEntities in parallel

            //Finally run the normal process entities for rest of the containers..
            RunInParallel(ProcessEntities, entitySplit, numberOfEntitiesThreads, batchSize, numberOfAttributesThreadPerEntity);
            //parallel ends here..

            #endregion

            if (isTracingEnabled) activity.Stop();
        }

        /// <summary>
        /// Process the extension relationship. This processing will be done container by container to handle parent creation first.
        /// </summary>
        private void ProcessExtensionRelationshipAndHierarchyLoad()
        {
            InitializeMDLContainerList();
            // Get the list of containers to process in a sorted list. The ORDER is very important
            ProcessEntityHierarchyLoad();
        }

        /// <summary>
        /// Process the extension relationship. This processing will be done container by container to handle parent creation first.
        /// </summary>
        private void ProcessEntityHierarchyLoad()
        {
            string message = string.Empty;
            Boolean isTracingEnabled = _traceSettings.IsBasicTracingEnabled;
            DiagnosticActivity activity = null;
            if (isTracingEnabled)
            {
                activity = new DiagnosticActivity();
                activity.Start();
            }

            // Get the list of containers to process in a sorted list. The ORDER is very important
            InitializeEntityTypesList();

            if (entityTypeList.Count == 0)
            {
                message = String.Format("The entity type list is for a entity hierarchy profile is empty. Import processing will stop.", MDMTraceSource.Imports);

                UpdateJobStatus(JobStatus.Aborted, message);

                if (isTracingEnabled)
                {
                    activity.LogError(message);
                }
                return;
            }

            // Process container by container
            for (int ctr = 0; entityTypeList != null && ctr < entityTypeList.Count; ctr++)
            {
                EntityType entityType = entityTypeList.Values[ctr];

                if (isTracingEnabled)
                {
                    message = String.Format("Entity processing will be performed for entity type {0}. There are still {1} entity types left.", entityType.Name, entityTypeList.Count - ctr - 1);
                    activity.LogInformation(message);
                }

                #region Run ProcessEntities in parallel

                RunInParallel(ProcessEntities, entitySplit, numberOfEntitiesThreads, batchSize, numberOfAttributesThreadPerEntity, String.Empty, String.Empty, entityType.Name);

                #endregion

                // At the end of each container reinitialize the source data...this is important for file based providers.
                // Do not do this for the last one. It will keep the file open
                if (ctr != entityTypeList.Count - 1)
                {
                    sourceData.Initialize(job, importProfile);
                }
            }

            if (isTracingEnabled)
            {
                message = String.Format("Entity process based on entity types is done.");
                activity.LogInformation(message);
                activity.Stop();
            }
        }

        /// <summary>
        /// Depending on the import mode, calls the appropriate method.
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="batchSize"></param>
        /// <param name="numberOfAttributesThreadPerEntity"></param>
        /// <param name="threadnumber"></param>
        private void Process(Int64 start, Int64 end, int batchSize, int numberOfAttributesThreadPerEntity, int threadnumber)
        {
            switch (importProfile.ProcessingSpecifications.ImportMode)
            {
                case ImportMode.InitialLoad:
                case ImportMode.Merge:
                    ProcessEntities(start, end, batchSize, numberOfAttributesThreadPerEntity, threadnumber);
                    break;
                case ImportMode.RelationshipLoad:
                case ImportMode.RelationshipInitialLoad:
                    ProcessRelationships(start, end, batchSize, numberOfAttributesThreadPerEntity, threadnumber);
                    break;
                case ImportMode.ComplextAttribute:
                    ProcessComplexAttribute(start, end, batchSize, numberOfAttributesThreadPerEntity, threadnumber);
                    break;
            }
        }

        /// <summary>
        /// Action to Process entities of given batchsize and parallelization parameters
        /// </summary>
        /// <param name="entityProcessDataContext"></param>
        /// <returns></returns>
        private void Process(EntityProcessDataContext entityProcessDataContext)
        {
            Int64 start = entityProcessDataContext.Start;
            Int64 end = entityProcessDataContext.End;
            Int32 batchsize = entityProcessDataContext.BatchSize;
            Int32 threadnumber = entityProcessDataContext.ThreadNumber;
            Int32 numberOfAttributesThreadPerEntity = entityProcessDataContext.NumberOfAttributesThreadPerEntity;
            this.callerContextWithJobId.OperationId = entityProcessDataContext.OperationId;

            Guid parentActivityId = entityProcessDataContext.ParentActivityId;

            DiagnosticActivity activity = new DiagnosticActivity(parentActivityId);
            activity.OperationId = entityProcessDataContext.OperationId;

            var callerContext = new CallerContext(MDMCenterApplication.JobService, Utility.GetModule(job.JobType), "EntityImportEngine::Process", job.Id, job.JobData.ProfileId, "");
            var executionContext = new ExecutionContext(callerContext, new CallDataContext(), new SecurityContext(0, job.CreatedUser, 0, ""), "");
            executionContext.LegacyMDMTraceSources.Add(MDMTraceSource.Imports);

            activity.Start(executionContext);

            if (_traceSettings.IsBasicTracingEnabled)
                activity.LogInformation(string.Format("Spawned for thread number: {0}", threadnumber));

            Process(start, end, batchSize, numberOfAttributesThreadPerEntity, threadnumber);

            activity.Stop();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="func"></param>
        /// <param name="entitySplit"></param>
        /// <param name="numberOfEntitiesThreads"></param>
        /// <param name="batchSize"></param>
        /// <param name="numberOfAttributesThreadPerEntity"></param>
        /// <param name="containerName"></param>
        /// <param name="organizationName"></param>
        /// <param name="entityTypeName"></param>
        private void RunInParallel(Action<EntityProcessDataContext> processAction, Dictionary<Int32, Int64> entitySplit, Int32 numberOfEntitiesThreads, Int32 batchSize, Int32 numberOfAttributesThreadPerEntity, String containerName = "", String organizationName = "", String entityTypeName = "")
        {
            DiagnosticActivity activity = new DiagnosticActivity();

            activity.Start();

            #region Run Process in parallel

            try
            {
                var entityProcessDataContexts = new Collection<EntityProcessDataContext>();

                for (int i = 0; i < numberOfEntitiesThreads; i++)
                {
                    var entityProcessDataContext = new EntityProcessDataContext
                    {
                        Start = entitySplit[i],
                        End = entitySplit[i + 1] - 1,
                        BatchSize = batchSize,
                        NumberOfAttributesThreadPerEntity = numberOfAttributesThreadPerEntity,
                        ParentActivityId = activity.ActivityId,
                        OperationId = activity.OperationId,
                        ThreadNumber = i,
                        ContainerName = containerName,
                        OrganizationName = organizationName,
                        EntityTypeName = entityTypeName
                    };

                    entityProcessDataContexts.Add(entityProcessDataContext);
                }

                new ParallelTaskProcessor().RunInParallel<EntityProcessDataContext>(entityProcessDataContexts, processAction, new CancellationTokenSource(), numberOfEntitiesThreads);
            }
            finally
            {
                activity.Stop();
            }

            #endregion
        }

        #endregion

        #region Step 1.1: Entity Imports

        #region Step 1.1: Process Method
        /// <summary>
        /// 
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="batchSize"></param>
        /// <param name="numberOfAttributesThreadPerEntity"></param>
        /// <param name="threadnumber"></param>
        private void ProcessEntities(Int64 start, Int64 end, int batchSize, int numberOfAttributesThreadPerEntity, int threadnumber)
        {
            ProcessEntities(start, end, batchSize, numberOfAttributesThreadPerEntity, threadnumber, String.Empty, String.Empty, String.Empty);
        }

        /// <summary>
        /// Runs per thread.
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="batchSize"></param>
        /// <param name="numberOfAttributesThreadPerEntity"></param>
        /// <param name="threadnumber"></param>
        private void ProcessEntities(Int64 start, Int64 end, int batchSize, int numberOfAttributesThreadPerEntity, int threadnumber, String containerName, String organizationName, String entityTypeName)
        {
            //_traceSettings.IsBasicTracingEnabled = threadnumber > 0 ? false : _traceSettings.IsBasicTracingEnabled;

            DiagnosticActivity activity = new DiagnosticActivity();
            if (_traceSettings.IsBasicTracingEnabled) activity.Start();

            string message = string.Format("Process entities started on thread ({0}) for batch size ({1})", threadnumber, batchSize);

            if (_traceSettings.IsBasicTracingEnabled)
                activity.LogInformation(message);

            #region Local Variables

            int batchNumber = 0;
            Int64 totalBatches = (end - start) / batchSize;
            // for logging purpose get the count
            int entitybatchCount = 0;
            Int64 processedentityCount = 0;
            int errorEntityCount = 0;
            bool entityProcess = false;
            message = string.Empty;
            String exceptionMessage = String.Empty;

            #endregion

            if ((totalBatches * batchSize) != (end - start))
                totalBatches++;
            // Within the given boundaries, run the individual processing in batches..
            Int64 ctr = start;

            try
            {
                while (true)
                {
                    #region Prepare for Multiple Batched entity Process

                    // This is only applicable for multiple batched source data..Rest of the providers do not use this...
                    batchNumber++;

                    // If batchesToTrace is specified and batch number is above batchesToTrace pause tracing
                    // If batch number is above the threshold value pause tracing
                    if ((_batchesToTrace > 0 && batchNumber > _batchesToTrace) ||
                        (BATCHES_TO_TRACE_THRESHOLD > 0 && batchNumber > BATCHES_TO_TRACE_THRESHOLD))
                    {
                        lock (lockDiagnosticsObject)
                        {
                            if (_traceSettings.TracingMode != TracingMode.None)
                            {
                                _traceSettings.UpdateSettings(true, TracingMode.None, TracingLevel.Basic);
                            }

                            if (activity.TraceSettings != null && activity.TraceSettings.TracingMode != TracingMode.None)
                            {
                                activity.TraceSettings.TracingMode = TracingMode.None;
                            }
                        }
                    }


                    long startBatchTime = DateTime.Now.Ticks;
                    Int64 batchStart = ctr;
                    Int64 batchEnd = ctr + batchSize - 1;
                    // make sure we dont cross the boundary..
                    if (batchEnd > end)
                        batchEnd = end;

                    #endregion

                    #region Get Source Entity Data

                    RefreshDiagnosticsOption();

                    EntityCollection entityCollection = GetEntitiesFromProvider(batchStart, batchEnd, threadnumber, containerName, organizationName, entityTypeName);

                    // Special check for handling container based processing.
                    if (String.IsNullOrEmpty(containerName) && importProfile.ProcessingSpecifications.ImportMode == ImportMode.ExtensionRelationshipLoad)
                    {
                        // if this is a MDL import then remove entities that belong to container that are already processed.
                        if (mdlcontainerList.Count > 0)
                        {
                            RemoveEntitiesFromMDLContainers(entityCollection);
                        }
                    }
                    #endregion

                    #region Process Entities

                    RefreshDiagnosticsOption();

                    if (entityCollection == null || entityCollection.Count <= 0)
                    {
                        // no data to process...continue to next batch..
                        message = string.Format("No entities for Job Id ({0}), batch # ({1}), batch start ({2}) - batch end ({3})", job.Id, batchNumber, batchStart, batchEnd);

                        if (_traceSettings.IsBasicTracingEnabled)
                            activity.LogInformation(message);

                        // when the import type is not a database type
                        if (sourceData.GetBatchingType() != ImportProviderBatchingType.Multiple)
                            return;
                    }
                    else
                    {
                        // for logging purpose get the count
                        entitybatchCount = entityCollection.Count;
                        processedentityCount = entitybatchCount;
                        errorEntityCount = 0;
                        EntityOperationResultCollection _errorCollection = null;

                        // validate also removes error entities from the list..based on mode
                        bool remove = (importProcessingType == ImportProcessingType.ValidateAndProcess || importProcessingType == ImportProcessingType.ValidateMatchAndMerge) ? true : false;

                        // Take out any ID that got put in the source entity object.
                        if (importProfile.ProcessingSpecifications.JobProcessingOptions.CleanseInternalIdsFromSourceData)
                        {
                            CleanseEntities(entityCollection);
                        }

                        EntityOperationResultCollection entityOperationResultCollection = new EntityOperationResultCollection();

                        #region Publish Import Batch Started Event

                        ImportEventArgs importEventArgs = new ImportEventArgs(entityCollection, entityOperationResultCollection, job, importProfile, MDMCenterApplication.JobService, MDMCenterModules.Import, MDMPublisher.MDM_Import);
                        ImportEventManager.Instance.OnImportBatchStarted(importEventArgs);

                        if (_traceSettings.IsBasicTracingEnabled)
                        {
                            message = string.Format("Batch # {0} - Import batch start event published", batchNumber);
                            activity.LogInformation(message);
                        }

                        #endregion

                        RefreshDiagnosticsOption();

                        EntityCollection entitiesWithMatching = null;
                        EntityCollection entitiesWithoutMatching = null;

                        // When we have to do match only, do not validate
                        //if (importProfile.ProcessingSpecifications.ImportProcessingType != ImportProcessingType.MatchOnly)
                        {
                            _errorCollection = FillAndValidateEntities(entityCollection, remove, threadnumber, out entitiesWithMatching, out entitiesWithoutMatching);
                        }

                        // Now process/create them once they passed the validation
                        try
                        {
                            RefreshDiagnosticsOption();

                            if (entityCollection.Count > 0)
                            {
                                //Key Note: Relationship initial load dont have bulk insert mode..it would go through normal entity processing only..
                                if (importProfile.ProcessingSpecifications.ImportMode == ImportMode.Merge
                                    || importProfile.ProcessingSpecifications.ImportMode == ImportMode.RelationshipLoad
                                    || importProfile.ProcessingSpecifications.ImportMode == ImportMode.ExtensionRelationshipLoad
                                    || importProfile.ProcessingSpecifications.ImportMode == ImportMode.EntityHierarchyLoad
                                    || importProfile.ProcessingSpecifications.ImportMode == ImportMode.EntityExtensionRelationshipAndHierarchyLoad)
                                {
                                    entityProcess = ProcessEntityMode(entityCollection, threadnumber, entitiesWithMatching, entitiesWithoutMatching);
                                }
                                else
                                {
                                    entityProcess = ProcessEntityBulkMode(entityCollection, numberOfAttributesThreadPerEntity, threadnumber);
                                }
                            }
                            else
                            {
                                // entity collection became empty..either we did not get any item from staging or..all failed valiation.
                                processedentityCount = 0;
                                entityProcess = true;
                            }
                        }
                        catch (Exception ex)
                        {
                            exceptionMessage = ex.Message;
                            message = String.Format("Processing entities batch {0} and {1} failed on thread {2} failed with the exception {3}.", batchStart, batchEnd, threadnumber, ex.Message);
                            message = String.Format("Job Id {0} - {1}", job.Id, message);

                            activity.LogError(message);
                            entityProcess = false;
                        }

                        #region Record Processing Results

                        // take care of updating the source with the error information
                        if (_errorCollection != null && _errorCollection.Count > 0)
                        {
                            errorEntityCount = _errorCollection.Count;
                            processedentityCount = entitybatchCount - errorEntityCount;

                            LogEntityErrors(_errorCollection, entityCollection, false);
                        }

                        UpdateJobStatus(JobStatus.Running);

                        #region Publish Import Batch Completed Event

                        importEventArgs = new ImportEventArgs(entityCollection, entityOperationResultCollection, job, importProfile, MDMCenterApplication.JobService, MDMCenterModules.Import, MDMPublisher.MDM_Import);
                        ImportEventManager.Instance.OnImportBatchCompleted(importEventArgs);

                        if (_traceSettings.IsBasicTracingEnabled)
                        {
                            message = string.Format("Batch # {0} - Import batch complete event published", batchNumber);
                            activity.LogInformation(message);
                        }

                        entityOperationResultCollection.RefreshOperationResultStatus();

                        #endregion

                        // Entity process failed..update all the entities..
                        if (entityProcess == false)
                        {
                            // when entity processing failes..we should stop this thread..
                            message = string.Format("Thread {0} encountered error in batch {1}. The batch primary key is between {2} and {3}.", threadnumber, batchNumber, batchStart, batchEnd);
                            // if there is an exception, log that also..
                            if (String.IsNullOrEmpty(exceptionMessage) == false)
                            {
                                message = String.Format("{0}. The exception message is {1}", message, exceptionMessage);
                            }
                            message = String.Format("Job Id {0} - {1}", job.Id, message);

                            if (_traceSettings.IsBasicTracingEnabled)
                            {
                                activity.LogError(message);
                            }

                            job.JobData.OperationResult.Errors.Add(new Error("100", message));

                            progressHandler.UpdateFailedEntities(entityCollection.Count);

                            UpdateJobStatus(JobStatus.Running);
                        }

                        long endBatchTime = DateTime.Now.Ticks;
                        double oneBatchTime = new TimeSpan(endBatchTime - startBatchTime).TotalSeconds;
                        // When the total batch size is not availble, print only information about the current batch.
                        if (totalBatches > 0 && totalBatches >= batchNumber)
                        {
                            double remainingTime = oneBatchTime * (((totalBatches - batchNumber) > 0) ? (totalBatches - batchNumber) : 0);

                            message = string.Format("Thread {0} completed batch {1} of {2} in {3,2:F} seconds. Estimated time left is {4,2:F} seconds. Total entity in the batch was {5}, processed were {6} and errors were {7}.",
                            threadnumber, batchNumber, totalBatches, oneBatchTime, oneBatchTime * (totalBatches - batchNumber), entitybatchCount, processedentityCount, errorEntityCount);
                        }
                        else
                        {
                            message = string.Format("Thread {0} completed batch {1} in {2,2:F} seconds. Total number of  in the batch was {3}, processed were {4} and errors were {5}.", threadnumber, batchNumber, oneBatchTime, entitybatchCount, processedentityCount, errorEntityCount);
                        }

                        message = String.Format("Job Id {0} - {1}", job.Id, message);

                        Console.WriteLine(message);
                        LogHandler.WriteInformationLog(message, 201);

                        //if (_traceSettings.IsBasicTracingEnabled)
                        //    activity.LogInformation(message);

                        #endregion
                    }

                    #endregion

                    #region Prepare next batch

                    RefreshDiagnosticsOption();

                    switch (sourceData.GetBatchingType())
                    {
                        case ImportProviderBatchingType.Multiple:
                            {
                                // check if we reached the end..
                                if (ctr == end || ctr + batchSize > end)
                                    return;
                                if (ctr + batchSize < end)
                                    ctr = ctr + batchSize;
                                else
                                    ctr = end;
                            }
                            break;
                        case ImportProviderBatchingType.Single:
                            {
                                // Single batched type providers only stops..when there are no records..so continue the loop..
                            }
                            break;
                        case ImportProviderBatchingType.None:
                            return;
                    }

                    #endregion
                }
            }
            finally
            {
                if (_traceSettings.IsBasicTracingEnabled) activity.Stop();
            }
        }

        /// <summary>
        /// Action to ProcesssEntities of given batchsize and parallelization parameters
        /// </summary>
        /// <param name="entityProcessDataContext"></param>
        /// <returns>DiagnosticActivity</returns>
        private void ProcessEntities(EntityProcessDataContext entityProcessDataContext)
        {
            Int64 start; Int64 end; int batchSize; int numberOfAttributesThreadPerEntity; int threadnumber; String containerName; String organizationName; String entityTypeName;

            DiagnosticActivity activity = new DiagnosticActivity(entityProcessDataContext.ParentActivityId);
            this.callerContextWithJobId.OperationId = entityProcessDataContext.OperationId;
            activity.OperationId = entityProcessDataContext.OperationId;

            start = entityProcessDataContext.Start;
            end = entityProcessDataContext.End;
            batchSize = entityProcessDataContext.BatchSize;
            numberOfAttributesThreadPerEntity = entityProcessDataContext.NumberOfAttributesThreadPerEntity;
            threadnumber = entityProcessDataContext.ThreadNumber;
            containerName = entityProcessDataContext.ContainerName;
            organizationName = entityProcessDataContext.OrganizationName;
            entityTypeName = entityProcessDataContext.EntityTypeName;

            try
            {
                if (_traceSettings.IsBasicTracingEnabled) activity.Start();

                ProcessEntities(start, end, batchSize, numberOfAttributesThreadPerEntity, threadnumber, containerName, organizationName, entityTypeName);
            }
            finally
            {
                if (_traceSettings.IsBasicTracingEnabled) activity.Stop();
            }
        }

        #endregion

        #region Step 1.1.1: Entity Initial Load Processing

        /// <summary>
        /// Process the entities in a fast mode. Entities are processed using entity BL and attributes are processed separately in the bulk mode.
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="numberOfAttributesThread"></param>
        private bool ProcessEntityBulkMode(EntityCollection entityCollection, int numberOfAttributesThread, int threadnumber)
        {
            string message = string.Empty;
            DiagnosticActivity activity = new DiagnosticActivity();
            if (_traceSettings.IsBasicTracingEnabled) activity.Start();

            EntityBL target = new EntityBL();
            EntityOperationResultCollection entityOperationResultCollection = new EntityOperationResultCollection();

            try
            {
                #region Entity BL Processing

                // Call the new created bulk create method. This will serialize the entity collection to a XML and call the SP..
                // The SP will return the list of 'ids' for the newly created entities. We need those to stamp the attributes.
                // The entity create is causing dead locks when called in multiple threads. So queue it up..for now. The dev team is
                // working on it.
                if (importProcessingType == ImportProcessingType.ValidateAndProcess)
                {
                    if (_traceSettings.IsBasicTracingEnabled)
                    {
                        message = String.Format("Call the entity BL for creating the entities. Total entity in this batch is {0}", entityCollection.Count());
                        activity.LogInformation(message);
                    }

                    if (entityCollection.Count > 0)
                    {
                        //Setting entityProcessingOptions.RefreshDenorm as false, so that denorm refresh is not done by core EntityProcess.
                        //As in initial load we will do DNI and DN refresh through Bulk insert.
                        foreach (Entity entity in entityCollection)
                        {
                            CleanseInvalidComplexAttributes(entity);
                        }
                        entityOperationResultCollection = target.Process(entityCollection, entityProcessingOptions, this.callerContextWithJobId);
                    }
                }
                else
                {
                    // in validate only mode, we dont process the entity in the database..just proceed, get the attributes from staging and
                    // do validate on those..
                    entityOperationResultCollection = PrepareEntityOperationResultsSchema(entityCollection);
                    EntityValidationBL entityValidation = new EntityValidationBL();
                    entityValidation.Validate(entityCollection, entityOperationResultCollection, new CallerContext(application, MDMCenterModules.Validation));
                }

                if (entityOperationResultCollection.OperationResultStatus != OperationResultStatusEnum.Successful)
                {
                    // for validate mode, do not remove from list..we can continue to validate. For process mode, remove.if entity failed
                    // no need to process attributes.
                    bool remove = (importProcessingType == ImportProcessingType.ValidateAndProcess) ? true : false;
                    LogEntityErrors(entityOperationResultCollection, entityCollection, remove);
                }

                #endregion

                #region Post BL Processing Get Ids

                // If we processed the data, get the ids out. Remember we only process entities marked for 'CREATE'
                if (importProcessingType == ImportProcessingType.ValidateAndProcess)
                {
                    foreach (Entity entity in entityCollection)
                    {
                        var filteredEntity = from original in entityOperationResultCollection
                                             where
                                             (original.ReferenceId.Equals(entity.ReferenceId))
                                             select original;

                        if (filteredEntity.Count() != 1)
                        {
                            message = string.Format("Job Id {0} - The reference id {1} has duplicate entities. It will not be processed further.", job.Id, entity.ReferenceId);

                            if (_traceSettings.IsBasicTracingEnabled)
                            {
                                activity.LogWarning(message);
                            }

                            LogHandler.WriteWarningLog(message, 50);
                            continue;
                        }
                        entity.Id = filteredEntity.First().EntityId;
                    }

                    // Check for any spill overs...audit purpose..we removed all error ones..we got id's for successful ones..
                    // anything else lying around without id???
                    // Get entity list with create action
                    List<Entity> missingList = (from entity in entityCollection
                                                where entity.Id == -1
                                                select entity).ToList();

                    // TODO - Log and remove it from list
                    // Update the source back with succes and the entity ID
                    sourceData.UpdateSuccessEntities(entityCollection, application, stagingModule);

                    LogEntitySuccess(entityOperationResultCollection);
                }

                if (_traceSettings.IsBasicTracingEnabled)
                {
                    message = String.Format("Filling the entity collection back with the entity id from database is done.");
                    activity.LogInformation(message);
                }
                #endregion

                #region Split Entities for Attribute Processing

                // Now thta the entity is done, split the entity batch in to batches based on the attributes thread count. Like filling in the buckets proportionately
                int totalEntities = entityCollection.Count();

                // if for some reason we have less number of entties than the attributes thread, then use only that many number of threads.
                if (totalEntities < numberOfAttributesThread)
                    numberOfAttributesThread = totalEntities;

                // Split the entity for the attrbute thread
                int entityCount = 0;
                int thread = 0;
                string referenceparameter = string.Empty;

                // Need to get the attrbutes from staging..have to pass this long list of reference numbers...for now, pass as comma delimited text..
                Dictionary<int, String> referenceNumberStringparam = new Dictionary<int, string>(numberOfAttributesThread);

                // go round robin and split the entities in to attribute thread bucket.
                foreach (Entity entity in entityCollection)
                {
                    if (entityCount < numberOfAttributesThread)
                        thread = entityCount;
                    else
                        thread = entityCount % numberOfAttributesThread;

                    if (referenceNumberStringparam.Keys.Count > thread)
                        referenceparameter = referenceNumberStringparam[thread];

                    // Get entity;
                    if (string.IsNullOrEmpty(referenceparameter))
                        referenceparameter = string.Format("'{0}'", entity.ReferenceId.ToString());
                    else
                        referenceparameter = string.Format("{0}, '{1}'", referenceparameter, entity.ReferenceId);

                    if (referenceNumberStringparam.Keys.Count > thread)
                        referenceNumberStringparam[thread] = referenceparameter;
                    else
                        referenceNumberStringparam.Add(thread, referenceparameter);

                    entityCount++;
                    referenceparameter = String.Empty;
                }

                if (_traceSettings.IsBasicTracingEnabled)
                {
                    message = String.Format("Spliting the entity collection for the attribute processing is done.");
                    activity.LogInformation(message);
                }
                #endregion

                #region Process Attributes

                // Now for each of the attribute types, call the corresponding processing in a separate 'Task'...
                // Need to check the connection pool size to make sure it can handle this many calls..
                DataTable dnSearchCombined = null;

                // DN search has logic at the entity level..
                IDNSearch dnSearch = attributeObjects.GetDNSearchObject();

                dnSearchCombined = dnSearch.CreateDataTable(mdmVersion);

                // The bulk insert needs to fire triggers when we don't process denorm on the initial load.
                dnSearch.FireTriggers = false;

                bool attributeTypeProcess = true;

                long attributesstartTime = DateTime.Now.Ticks;

                // Removed the parallel loop and made it a regular loop that will do one attribute type at a time.
                for (int i = 0; i < availableAttributeTypes.Count; i++)
                {
                    bool localProcess = ProcessAttributesForEntities(referenceNumberStringparam, entityCollection, availableAttributeTypes[i], numberOfAttributesThread, dnSearch, threadnumber);

                    if (localProcess == false)
                        attributeTypeProcess = false;
                }

                long attributessEndTime = DateTime.Now.Ticks;

                double attributeProcessingTime = new TimeSpan(attributessEndTime - attributesstartTime).TotalSeconds;

                if (_traceSettings.IsBasicTracingEnabled)
                {
                    message = string.Format("Attribute processing took both the types took {0} seconds total.", attributeProcessingTime);
                    activity.LogInformation(message);
                }
                #endregion

                #region Process DENORM

                long dnBulkInsertStarttime = DateTime.Now.Ticks;

                // ONLY if both the attribute type processing succeed we will bulk insert and DENORM.
                if (attributeTypeProcess)
                {
                    #region Bulk Insert and Denorm

                    // Do we have to process???
                    if (importProcessingType == ImportProcessingType.ValidateAndProcess)
                    {
                        // Are there any data?
                        if (dnSearch.GetEntityCount() > 0)
                        {
                            if (_traceSettings.IsBasicTracingEnabled)
                                activity.LogInformation("Bulk copying the DNI attrval table.");

                            //if (util.BulkCopyDataTable(dnAttrValCombined, dnAttrVal, mdmVersion, LogHandler, application, searchModule))
                            //{
                            foreach (Entity item in entityCollection)
                            {
                                DataRow dsRow = dnSearchCombined.NewRow();
                                Boolean fillRowStatus = dnSearch.FillDataRow(item, null, dsRow, mdmVersion, auditRefId);
                                if (fillRowStatus)
                                {
                                    dnSearchCombined.Rows.Add(dsRow);
                                }
                                else
                                {
                                    if (_traceSettings.IsBasicTracingEnabled)
                                    {
                                        message = string.Format("Fill data for DN Search missing value for entity {0}. This entity did not have any attributes to process.", item.Name);
                                        activity.LogWarning(message);
                                    }
                                }
                            }

                            // bulk copy
                            if (_traceSettings.IsBasicTracingEnabled)
                                activity.LogInformation("Bulk copying the DN Searchtable.");

                            // TODO - REPLACE DN Search with SP call..pass all the values..the SP will only insert/update the value.
                            if (util.BulkCopyDataTable(dnSearchCombined, dnSearch, mdmVersion, LogHandler, application, searchModule))
                            {
                            }
                            else
                            {
                                // DN_Search failed.
                                if (_traceSettings.IsBasicTracingEnabled)
                                {
                                    activity.LogError("Bulk copy of DN Search failed. Check log for more details");
                                }
                                return false;
                            }
                        }
                    }

                    #endregion
                }
                else
                {
                    message = "Job Id - " + job.Id.ToString() + "Denorm processing is skipped because one of the attribute processing failed.";

                    if (_traceSettings.IsBasicTracingEnabled)
                    {
                        activity.LogWarning(message);
                    }

                    LogHandler.WriteWarningLog(message, 50);
                    return false;
                }

                long dnBulkInsertEndTime = DateTime.Now.Ticks;
                double dnBulkInsertTime = new TimeSpan(dnBulkInsertEndTime - dnBulkInsertStarttime).TotalSeconds;

                if (_traceSettings.IsBasicTracingEnabled)
                {
                    message = string.Format("Complete Attribute processing for both the types took {0} seconds total. The DN bulk inserts took {1} seconds.", attributeProcessingTime, dnBulkInsertTime);
                    activity.LogInformation(message);
                }
                #endregion

                #region Bulk Process Impacted Entities

                if (importProcessingType == ImportProcessingType.ValidateAndProcess)
                {
                    ImpactedEntityBL impactedEntityManager = new ImpactedEntityBL();
                    EntityCollection impactedEntities = new EntityCollection();
                    IEnumerable<Entity> impactedEntityCollection = entityCollection.Where(e => e.Id > 0).ToList();
                    impactedEntities = new EntityCollection(impactedEntityCollection.ToList());
                    impactedEntityManager.ImpactedEntityBulkProcess(impactedEntities, this.callerContext);
                }
                
                #endregion Bulk Process Impacted Entities

            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (_traceSettings.IsBasicTracingEnabled) activity.Stop();
            }

            progressHandler.UpdateSuccessFulEntities(entityCollection.Count);

            return true;
        }

        /// <summary>
        /// Gets and processes the attributes ( based on type) in separate threads. The end result is merged in to a database for bulk inserting.
        /// </summary>
        /// <param name="referenceparameter"></param>
        /// <param name="entities"></param>
        /// <param name="attType"></param>
        /// <param name="numberOfAttributesThread"></param>
        /// <returns></returns>
        private bool ProcessAttributesForEntities(Dictionary<int, String> referenceparameter, EntityCollection entities, AttributeModelType attType, int numberOfAttributesThread, IDNSearch dnSearch, int threadnumber)
        {
            string message = string.Empty;
            DiagnosticActivity activity = new DiagnosticActivity();
            if (_traceSettings.IsBasicTracingEnabled) activity.Start();

            bool returnStatus = true;
            String attributeType = String.Empty;

            #region Prepare Data tables for bulk insert

            // Bulk insert performs best when the data size is big enough.. So this table will merge the results back..
            DataTable combinedTable = null;
            IBulkInsert bulkInsert = null;

            // Initialize based on attribute type..
            switch (attType)
            {
                case AttributeModelType.Common:
                    bulkInsert = attributeObjects.GetCommonAttribueObject();
                    attributeType = "C";
                    break;
                case AttributeModelType.Category:
                    bulkInsert = attributeObjects.GetTechnicalAttributeObject();
                    attributeType = "T";
                    break;
            }

            // The bulk insert needs to fire triggers when we don't process denorm on the initial load.
            bulkInsert.FireTriggers = false;

            combinedTable = bulkInsert.CreateDataTable(mdmVersion);

            if (_traceSettings.IsBasicTracingEnabled)
            {
                message = string.Format("Attribute processing for type {0}.", attType);
                activity.LogInformation(message);
            }

            #endregion

            #region Attribute Processing

            // Based on number of attirbutes threads call the attributes methods
            long attributesstartTime = DateTime.Now.Ticks;

            Parallel.For(0, numberOfAttributesThread, i =>
            {
                if (String.IsNullOrEmpty(Thread.CurrentThread.Name))
                    Thread.CurrentThread.Name = String.Format("E{0}{1}{2}", threadnumber, attributeType, i);
                bool attributeProcess = GetAttributesFromStagingAndMergeDataTable(referenceparameter[i], entities, attType, combinedTable, bulkInsert, dnSearch);
                if (!attributeProcess)
                {
                    // one of the attribute thread failed..DO not bulk insert the batch..Update the source with the error.
                    returnStatus = false;
                }
            });

            long attributessEndTime = DateTime.Now.Ticks;
            double attributeProcessingTime = new TimeSpan(attributessEndTime - attributesstartTime).TotalSeconds;

            if (_traceSettings.IsBasicTracingEnabled)
            {
                message = string.Format("Attribute processing for type {0} done. Now bulk insert the combined table", attType);
                activity.LogInformation(message);
            }

            #endregion

            #region Attribute Bulk Insert

            long bulkInsertStartTime = DateTime.Now.Ticks;
            double bulkInsertProcessingTime = 0;
            long bulkInsertEndTime = 0;

            // Only when config says we have to process..and there were no errors on the attribute thread..
            if (returnStatus && importProcessingType == ImportProcessingType.ValidateAndProcess)
            {
                // Bulk insert the combined attribute table
                if (util.BulkCopyDataTable(combinedTable, bulkInsert, mdmVersion, LogHandler, application, importmodule))
                {
                    if (_traceSettings.IsBasicTracingEnabled)
                        activity.LogInformation("Bulk insert the combined table is done. Now process the Denorm data");

                    // Update the source with the processing status..
                    if (sourceData.UpdateSuccessAttributes(attType, entities, application, stagingModule) == false)
                    {
                        //updating status failed..
                        message = string.Format("Updating the staging data source for successful bulk processing of attribute type {0} failed ", attType);
                        message = String.Format("Job Id {0} - {1}", job.Id, message);

                        if (_traceSettings.IsBasicTracingEnabled)
                        {
                            activity.LogError(message);
                        }
                        LogHandler.WriteErrorLog(message, 100);
                    }

                    progressHandler.UpdateSuccessFulAtttributeBatch(attType, entities.Count());

                }
                else
                {
                    if (_traceSettings.IsBasicTracingEnabled)
                    {
                        message = String.Format("Bulk insert for attribute type {0} failed. Check the log for more details.", attType);
                        activity.LogError(message);
                    }
                    returnStatus = false;
                }
            }

            bulkInsertEndTime = DateTime.Now.Ticks;
            bulkInsertProcessingTime = new TimeSpan(bulkInsertEndTime - bulkInsertStartTime).TotalSeconds;

            if (_traceSettings.IsBasicTracingEnabled)
            {
                message = string.Format("Attribute processing for {0} type for ALL worker threads took {1} seconds. Bulk insert took {2} seconds.", attType, attributeProcessingTime, bulkInsertProcessingTime);
                activity.LogInformation(message);
            }

            #endregion

            #region Update Error Status

            // This failure happens when the GetAttributesFromStagingAndMergeDataTable threw an exception or when bulk insert failed..
            // update all the attributes for the source entities as failed.
            if (returnStatus == false)
            {
                foreach (string entityList in referenceparameter.Values)
                {
                    sourceData.UpdateErrorAttributes(attType, entityList, "Bulk insert failed for the batch. Check the file dump folder to retry.", application, stagingModule);
                }

                progressHandler.UpdateFailedAtttributeBatch(attType, entities.Count());

                message = String.Format("Bulk process of {0} attributes failed due to an exception. Correct the errors and re run the processing for this batch", attType);

                EntityOperationResultCollection errorCollection = GetEntityErrorCollection(entities, message);
                if (errorCollection != null)
                {
                    LogEntityErrors(errorCollection, entities, false);
                }
            }

            #endregion

            if (_traceSettings.IsBasicTracingEnabled) activity.Stop();

            return returnStatus;
        }

        /// <summary>
        /// For the given batch, get the attributes from the staging tables.
        /// </summary>
        /// <param name="referenceparameter"></param>
        /// <param name="entities"></param>
        /// <param name="attType"></param>
        /// <param name="combinedTable"></param>
        /// <returns></returns>
        private bool GetAttributesFromStagingAndMergeDataTable(string referenceparameter, EntityCollection entities, MDM.Core.AttributeModelType attType, DataTable combinedTable, IBulkInsert bulkInsert, IDNSearch dnSearch)
        {
            string message = string.Empty;
            // Diagnostics/Tracing not supported yet for this method in Prallel.For - ToDo

            DiagnosticActivity activity = new DiagnosticActivity();
            //if (_traceSettings.IsBasicTracingEnabled) activity.Start();

            bool returnStatus = true;

            if (string.IsNullOrEmpty(referenceparameter))
            {
                // no entities for processing in this thread..
                return returnStatus;
            }

            #region Create Data tables and other local variables

            // Local data table to store the values..If memory is an issue, switch to a local file using the other sample...
            DataTable caltable = bulkInsert.CreateDataTable(mdmVersion);

            AttributeOperationResultCollection attributeErrors = new AttributeOperationResultCollection();
            EntityOperationResultCollection entityErrors = new EntityOperationResultCollection();

            // have a local DN Search object. This will reduce the pressure of synchronization on the parent search interface.
            DNSearch localDnSearch = new DNSearch();

            #endregion

            try
            {
                Dictionary<string, int> attributeSequenceNumber = new Dictionary<string, int>();

                long methodstartTime = DateTime.Now.Ticks;
                long startTime = DateTime.Now.Ticks;
                // Get the attribute data from staging..
                AttributeCollection attributes = sourceData.GetAttributeDataforEntityList(attType, referenceparameter, entities, application, stagingModule);

                int attributeCount = 0;

                long endTime = DateTime.Now.Ticks;

                double getsourceDataTime = new TimeSpan(endTime - startTime).TotalMilliseconds;
                double fillProcessingTime, matchingProcessingTime, validateProcessingTime, dataRowProcessingTime, DNFillRowTime, DNProcessingTime;

                fillProcessingTime = matchingProcessingTime = validateProcessingTime = dataRowProcessingTime = DNFillRowTime = DNProcessingTime = 0;

                if (entityProcessingOptions.InitialLoadProcessInheritedValues == true)
                {
                    //if (_traceSettings.IsBasicTracingEnabled)
                    //    activity.LogInformation("InitialLoadProcessInheritedValues is enabled. Hence calculating inherited attribute values.");

                    EntityBL entityManager = new EntityBL();

                    Collection<Int64> entityIds = new Collection<long>();
                    EntityContext entityContext = new EntityContext();
                    entityContext.AttributeModelType = attType;
                    entityContext.LoadAttributes = true;

                    entityIds = GetEntityIdList(entities, referenceparameter);

                    //if (_traceSettings.IsBasicTracingEnabled)
                    //    activity.LogInformation("Loading entities from database to see if any inherited values are there.");

                    //Here loadLatest is set to true it is recommended to disable cache when InitialLoad is running.
                    EntityCollection coreEntitities = entityManager.Get(entityIds, entityContext, true, MDMCenterApplication.MDMCenter, MDMCenterModules.Import, false, false);

                    //if (_traceSettings.IsBasicTracingEnabled)
                    //    activity.LogInformation("Loaded entities from database to see if any inherited values are there.");

                    foreach (Entity coreEntity in coreEntitities)
                    {
                        IAttributeCollection iCoreEntityAttributes = null;
                        AttributeCollection coreEntityAttributes = null;


                        if (attType == AttributeModelType.Common)
                        {
                            iCoreEntityAttributes = coreEntity.GetCommonAttributes();
                            if (iCoreEntityAttributes != null)
                            {
                                coreEntityAttributes = (AttributeCollection)iCoreEntityAttributes;
                            }
                        }
                        else if (attType == AttributeModelType.Category)
                        {
                            iCoreEntityAttributes = coreEntity.GetCategorySpecificAttributes();
                            if (iCoreEntityAttributes != null)
                            {
                                coreEntityAttributes = (AttributeCollection)iCoreEntityAttributes;
                            }
                        }

                        foreach (Attribute attribute in coreEntityAttributes)
                        {
                            // Only if there is any value and if the value is inherited...
                            if (attribute.HasAnyValue() && attribute.SourceFlag == AttributeValueSource.Inherited)
                            {
                                var filteredAttribute = from original in attributes
                                                        where
                                                        (original.Name.ToLowerInvariant() == attribute.Name.ToLowerInvariant()
                                                        && original.AttributeParentName.ToLower() == attribute.AttributeParentName.ToLower()
                                                        && original.IsComplex == false
                                                        && coreEntity.Id == original.InstanceRefId
                                                        )
                                                        select original;

                                if (filteredAttribute.Count() < 1)
                                {
                                    if (attributes == null)
                                    {
                                        attributes = new AttributeCollection();
                                    }

                                    attribute.EntityId = coreEntity.Id;
                                    attribute.InstanceRefId = (Int32)coreEntity.Id;
                                    attribute.SourceFlag = AttributeValueSource.Inherited;
                                    attributes.Add(attribute);
                                }
                            }
                        }
                    }
                }

                if (attributes == null || attributes.Count() <= 0)
                {
                    // no attributes found..return
                    //if (_traceSettings.IsBasicTracingEnabled)
                    //    activity.LogInformation(string.Format("Getting attributes for type {0} returned NULL.", attType));

                    return returnStatus;
                }

                //put ivalues use attr.EntityId
                //Set EntityId
                foreach (Attribute attribute in attributes)
                {
                    #region Get Matching Entity

                    startTime = DateTime.Now.Ticks;
                    // get the entity for the reference number
                    var filteredEntity = from original in entities
                                         where
                                         (original.Id == attribute.EntityId)
                                         select original;

                    if (filteredEntity.Count() != 1)
                    {
                        message = string.Format("Job Id {0} - The entity id {1} has duplicate entities. It will not be processed further.", job.Id, attribute.EntityId);

                        if (_traceSettings.IsBasicTracingEnabled)
                        {
                            activity.LogWarning(message);
                        }

                        LogHandler.WriteWarningLog(message, 50);
                        continue;
                    }

                    Entity matchedEntity = filteredEntity.First();

                    endTime = DateTime.Now.Ticks;
                    matchingProcessingTime += new TimeSpan(endTime - startTime).TotalMilliseconds;

                    #endregion

                    #region Fill in Attribute information

                    startTime = DateTime.Now.Ticks;

                    // We do not have to do the Fill for the parent entity. The attribute will be inherited.
                    if (attribute.SourceFlag != AttributeValueSource.Inherited)
                    {
                        FillAttribute(matchedEntity, attribute, attributeModels: null, attributeOperationResult: null, entityReferenceId: "");
                    }

                    // special condition for complex attribute loading from staging..
                    if (attribute.IsComplex)
                    {
                        foreach (Value value in attribute.GetCurrentValues())
                        {
                            Int32 valueRefId = -1;
                            if (Int32.TryParse(value.AttrVal.ToString(), out valueRefId))
                            {
                                value.ValueRefId = valueRefId;
                            }
                        }
                    }
                    endTime = DateTime.Now.Ticks;
                    fillProcessingTime += new TimeSpan(endTime - startTime).TotalMilliseconds;

                    #endregion

                    #region Validate Attributes

                    startTime = DateTime.Now.Ticks;

                    // the staging primary key is stored in the id property of the attribute value.
                    Int32 stagingAttributePk = ValueTypeHelper.Int32TryParse(attribute.GetCurrentValuesInvariant().ElementAt(0).Id.ToString(), -1);

                    AttributeOperationResult attributeOperationResult = new AttributeOperationResult(stagingAttributePk, attribute.Name, attribute.LongName, attType, attribute.Locale);

                    if (attribute.IsLookup)
                    {
                        SetLookupAttributeAsBlank(attribute, importProfile);
                    }

                    ILocaleMessageManager localeMessageBL = new LocaleMessageBL();
                    EntityOperationsHelper.ValidateUnmappedAttributes(matchedEntity, attribute, null, attributeOperationResult, localeMessageBL, entityProcessingOptions, callerContext, OperationResultType.Error);

                    if (importProfile.ProcessingSpecifications.ImportMode == ImportMode.InitialLoad ||
                        importProfile.ProcessingSpecifications.ImportMode == ImportMode.RelationshipInitialLoad)
                    {
                        AttributeModel attributeModel = GetAttributeModelFromCachedDataModel(matchedEntity, attribute);
                        EntityOperationsHelper.ValidateEntityAttributesForImport(matchedEntity, null, attribute, attributeModel, attributeOperationResult, entityProcessingOptions, callerContext, localeMessageBL);
                    }

                    if (attributeOperationResult.HasError)
                    {
                        // if the attribute is a collection..add error for each..
                        if (attribute.GetCurrentValuesInvariant().Count > 1)
                        {
                            AttributeOperationResultCollection resultCollection = ExplodeAttributeOperationResultForCollection(attribute, attributeOperationResult);

                            foreach (AttributeOperationResult result in resultCollection)
                            {
                                entityErrors.AddAttributeOperationResult(matchedEntity.Id, result.AttributeId, "", result.ToXml(), OperationResultType.Error);
                                attributeErrors.Add(result);
                            }
                        }
                        else
                        {
                            //Get the requested entity operation result
                            entityErrors.AddAttributeOperationResult(matchedEntity.Id, stagingAttributePk, "", attributeOperationResult.ToXml(), OperationResultType.Error);
                            attributeErrors.Add(attributeOperationResult);
                        }
                        continue;
                    }

                    AttributeModelContext attributeModelContext = new AttributeModelContext()
                    {
                        ContainerId = matchedEntity.ContainerId,
                        CategoryId = matchedEntity.CategoryId,
                        EntityTypeId = matchedEntity.EntityTypeId,
                        Locales = new Collection<LocaleEnum>() { attribute.Locale },
                        AttributeModelType = attribute.AttributeModelType
                    };

                    if (importProfile.ProcessingSpecifications.EntityProcessingOptions.ValidateEntities || importProcessingType == ImportProcessingType.ValidationOnly)
                    {
                        EntityValidationBL entityValidation = new EntityValidationBL();
                        entityValidation.ValidateAttribute(attribute, attributeModelContext, attributeOperationResult, new CallerContext(application, MDMCenterModules.Validation));

                        // if we hit a validation error, dont process further..
                        if (attributeOperationResult.OperationResultStatus != OperationResultStatusEnum.Successful)
                        {
                            entityErrors.AddAttributeOperationResult(matchedEntity.Id, attribute.Id, "", attributeOperationResult.ToXml(), OperationResultType.Error);
                            attributeErrors.Add(attributeOperationResult);
                            continue;
                        }
                    }

                    endTime = DateTime.Now.Ticks;
                    validateProcessingTime += new TimeSpan(endTime - startTime).TotalMilliseconds;

                    #endregion

                    #region Process Attributes and Denorm

                    // Validations are done..time to process..
                    if (importProcessingType == ImportProcessingType.ValidateAndProcess)
                    {
                        if (attribute.SourceFlag != AttributeValueSource.Inherited)
                        {
                            startTime = DateTime.Now.Ticks;

                            Int32 totalValueRecords = attribute.GetCurrentValuesInvariant().Count;

                            DataRow[] dataRows = new DataRow[totalValueRecords];

                            for (int rownumber = 0; rownumber < totalValueRecords; rownumber++)
                            {
                                dataRows[rownumber] = caltable.NewRow();
                            }

                            bulkInsert.FillDataRow(filteredEntity.First(), attribute, dataRows, mdmVersion, auditRefId);

                            for (int rownumber = 0; rownumber < totalValueRecords; rownumber++)
                            {
                                caltable.Rows.Add(dataRows[rownumber]);
                            }

                            endTime = DateTime.Now.Ticks;
                            dataRowProcessingTime += new TimeSpan(endTime - startTime).TotalMilliseconds;
                        }

                        // Bulk upload denorm attributes..only if requested.
                        startTime = DateTime.Now.Ticks;
                        localDnSearch.ComputeKeySearchValue(filteredEntity.First(), attribute, mdmVersion);
                        endTime = DateTime.Now.Ticks;
                        DNProcessingTime += new TimeSpan(endTime - startTime).TotalMilliseconds;
                    }

                    #endregion

                    attributeCount++;
                }

                //if (_traceSettings.IsBasicTracingEnabled)
                //    activity.LogInformation(String.Format("Attribute processing for type {0} done. Now merge the data tables", attType));

                #region Post Process Merge Data Table

                // Only when config says we have to process..
                if (importProcessingType == ImportProcessingType.ValidateAndProcess)
                {
                    //Not sure if datatable merge is thread safe..put a lock
                    startTime = DateTime.Now.Ticks;
                    lock (lockObject)
                    {
                        if (caltable.Rows.Count > 0)
                        {
                            combinedTable.Merge(caltable);
                        }

                        // Bulk upload denorm attributes..only if requested.
                        // Now from the local dn search object update the parent search interface..We are already in critical section
                        // for the attribute worker threads.
                        foreach (Int64 entityId in localDnSearch.KeySearchValues.Keys)
                        {
                            EntitySearchValuesBuilder ksValue = localDnSearch.KeySearchValues[entityId];

                            if (ksValue != null)
                            {
                                dnSearch.AddKeySearchValue(entityId, ksValue.KeyValue, ksValue.SearchValue);
                            }
                        }
                    }

                    endTime = DateTime.Now.Ticks;
                    double attributeMergeTime = new TimeSpan(endTime - startTime).TotalSeconds;
                    double attributeProcessingTime = new TimeSpan(endTime - methodstartTime).TotalSeconds;

                    // Log slower worker threads
                    //if (attributeProcessingTime > 5)
                    {
                        String timetaken = String.Format("Get source data is {0} ms Matching time is {1} ms, fill time is {2} ms validate time is {3} ms datarowprocessing time is {4} ms DN Fill Time is {5} ms DN Processing time is {6} ms Dataset and DN merge time is {7} ms.",
                            getsourceDataTime, matchingProcessingTime, fillProcessingTime, validateProcessingTime, dataRowProcessingTime, DNFillRowTime, DNProcessingTime, attributeMergeTime);

                        //if (_traceSettings.IsBasicTracingEnabled)
                        //    activity.LogInformation(string.Format("Attribute Worker Processing for {0} type took {1} seconds. {2}.",attType, attributeProcessingTime, timetaken));
                    }
                }

                #endregion
            }
            catch (Exception ex)
            {
                message = String.Format("GetAttributesFromStagingAndMergeDatatable failed with exception {0}", ex.ToString());

                activity.LogError(message);
                LogHandler.WriteErrorLog(message, 100);
                returnStatus = false;
            }

            #region Record Attrbute Errors
            long errorstartTime = DateTime.Now.Ticks;

            // Process the errors also..
            if (entityErrors != null && entityErrors.Count > 0)
            {
                LogEntityErrors(entityErrors, entities, false);
            }

            // update the attribute level errors also..
            if (attributeErrors != null && attributeErrors.Count() > 0)
            {
                //attributeErrors.AttributeType = attType;
                if (sourceData.UpdateErrorAttributes(attributeErrors, application, stagingModule) == false)
                {
                    // Updating error fails??
                    message = string.Format("Updating error status for attribute type {0} failed.", attType);
                    if (_traceSettings.IsBasicTracingEnabled)
                    {
                        activity.LogError(message);
                    }
                    LogHandler.WriteErrorLog(message, 100);
                }

            }
            long errorEndTime = DateTime.Now.Ticks;
            double errorProcessingTime = new TimeSpan(errorEndTime - errorstartTime).TotalSeconds;


            //if (_traceSettings.IsBasicTracingEnabled)
            //activity.LogInformation(string.Format("Attribute Worker Processing for {0} type took {1} seconds for processing the staging errors.",attType, errorProcessingTime));

            #endregion

            // if (_traceSettings.IsBasicTracingEnabled) activity.Stop();

            return returnStatus;
        }

        #endregion

        #region Step 1.1.2: Entity Regular Import Processing

        /// <summary>
        /// Process the entities using entity BL.
        /// </summary>
        /// <param name="entityCollection"></param>
        /// <returns></returns>
        private bool ProcessEntityMode(EntityCollection entityCollection, int threadNumber, EntityCollection entitiesWithMatching, EntityCollection entitiesWithoutMatching)
        {
            string message = string.Empty;

            DiagnosticActivity activity = new DiagnosticActivity();
            if (_traceSettings.IsBasicTracingEnabled) activity.Start();

            EntityOperationResultCollection entityOperationResultCollection = new EntityOperationResultCollection();

            //Dictionary of Entity Reference Id <Relationships, OperationResults> which contains all relationship waiting for related entity to be created in same batch.
            Dictionary<Int64, Dictionary<RelationshipCollection, RelationshipOperationResultCollection>> relationshipsWaitingForToEntityCreation = new Dictionary<Int64, Dictionary<RelationshipCollection, RelationshipOperationResultCollection>>();

            try
            {
                #region Prepare entity components

                foreach (Entity entity in entityCollection)
                {
                    //Removing Blank ComplexAttributes
                    CleanseInvalidComplexAttributes(entity);

                    EntityCollection oneItemEntityCollection = new EntityCollection();
                    oneItemEntityCollection.Add(entity);

                    EntityOperationResult entityOperationResult = new EntityOperationResult(entity.Id, entity.ReferenceId, entity.LongName, entity.ExternalId);
                    entityOperationResult.OperationResultStatus = OperationResultStatusEnum.Successful;

                    #region Common attributes - Get from Source

                    if (importProfile.ProcessingSpecifications.ImportMode != ImportMode.RelationshipLoad &&
                        importProfile.ProcessingSpecifications.ImportMode != ImportMode.RelationshipInitialLoad)
                    {
                        AttributeCollection commonAttributes = sourceData.GetAttributeDataforEntities(AttributeModelType.Common, oneItemEntityCollection, application, stagingModule);

                        // did we get anything back?
                        if (commonAttributes != null && commonAttributes.Count > 0)
                        {
                            foreach (BusinessObjects.Attribute attrib in commonAttributes)
                            {
                                entity.Attributes.Add(attrib);
                            }
                        }

                        if (_traceSettings.IsBasicTracingEnabled)
                        {
                            int count = commonAttributes == null ? 0 : commonAttributes.Count;
                            if (count > 0)
                            {
                                message = string.Format("Retrieved {0} common attributes from external entity ({1})", count, entity.ExternalId);
                                activity.LogInformation(message);
                            }
                        }
                    }

                    #endregion

                    #region Techincal attributes - Get from Source

                    if (importProfile.ProcessingSpecifications.ImportMode != ImportMode.RelationshipLoad &&
                        importProfile.ProcessingSpecifications.ImportMode != ImportMode.RelationshipInitialLoad)
                    {
                        AttributeCollection techAttributes = sourceData.GetAttributeDataforEntities(AttributeModelType.Category, oneItemEntityCollection, application, stagingModule);

                        // did we get anything back?
                        if (techAttributes != null && techAttributes.Count > 0)
                        {
                            // we need a add method that takes a collection..
                            foreach (Attribute attrib in techAttributes)
                            {
                                entity.Attributes.Add(attrib);
                            }
                        }

                        if (_traceSettings.IsBasicTracingEnabled)
                        {
                            int count = techAttributes == null ? 0 : techAttributes.Count;
                            if (count > 0)
                            {
                                message = string.Format("Retrieved {0} technical attributes from external entity ({1})", count, entity.ExternalId);
                                activity.LogInformation(message);
                            }
                        }
                    }

                    #endregion

                    #region Relationship attributes - Get from Source

                    ReadRelationshipAttributes(entity, oneItemEntityCollection, activity);

                    #endregion

                    #region Cleanse Attribute Id and other ids

                    CleanseAttributes(entity.Attributes);

                    #endregion

                    #region Common and Technical attributes - Fill and Validate

                    // now do a fill and validate on entity attributes
                    if (entity.Attributes != null && entity.Attributes.Count > 0)
                    {
                        AttributeOperationResultCollection aorCollection = FillAndValidateAttributes(entity, entity.Attributes, false, threadNumber);

                        if (aorCollection != null && aorCollection.Count > 0)
                        {
                            entityOperationResult.SetAttributeOperationResults(aorCollection);

                            //In 7.0, if any attribute is fail means stop processing for complete entity..
                            entityOperationResult.AttributeOperationResultCollection.OperationResultStatus = aorCollection.OperationResultStatus;
                            entityOperationResult.OperationResultStatus = aorCollection.OperationResultStatus;
                        }

                        if (_traceSettings.IsBasicTracingEnabled)
                        {
                            int count = aorCollection == null ? 0 : aorCollection.Count;
                            if (count > 0)
                            {
                                message = string.Format("Fill and validate complete for {0} common and technical attributes for entity ({1})", count, entity.ExternalId);
                                activity.LogInformation(message);
                            }
                        }
                    }

                    #endregion

                    #region Relationships - Fill and Validate

                    // Now do a fill and validate on entity relationships
                    if (entity.Relationships != null && entity.Relationships.Count > 0)
                    {
                        FillAndValidateRelationships(entity, entityOperationResult, activity, ref relationshipsWaitingForToEntityCreation);
                    }

                    #endregion

                    if (entityOperationResult.AttributeOperationResultCollection.OperationResultStatus == OperationResultStatusEnum.Failed &&
                        entityOperationResult.RelationshipOperationResultCollection.OperationResultStatus == OperationResultStatusEnum.Failed)
                    {
                        entityOperationResult.OperationResultStatus = OperationResultStatusEnum.Failed;
                    }

                    entityOperationResultCollection.Add(entityOperationResult);
                }

                entityOperationResultCollection.RefreshOperationResultStatus();

                #endregion
                
                #region Publish Import Batch Process Started Event

                ImportEventArgs importEventArgs = importEventArgs = new ImportEventArgs(entityCollection, entityOperationResultCollection, job, importProfile, MDMCenterApplication.JobService, MDMCenterModules.Import, MDMPublisher.MDM_Import);
                ImportEventManager.Instance.OnImportBatchProcessStarted(importEventArgs);

                if (_traceSettings.IsBasicTracingEnabled)
                {
                    message = string.Format("Import batch process start event published");
                    activity.LogInformation(message);
                }

                entityOperationResultCollection.RefreshOperationResultStatus();

                #endregion

                #region Log and Remove bad entities from processing

                // if the validation is not a success..
                // for validate mode, do not remove from list..we can continue to validate. For process mode, remove.if entity failed
                // no need to process attributes.
                bool remove = (importProcessingType == ImportProcessingType.ValidateAndProcess || importProcessingType == ImportProcessingType.ValidateMatchAndMerge) ? true : false;

                IEnumerable<Int64> warningEntityIdList = null;

                if (entityOperationResultCollection.OperationResultStatus != OperationResultStatusEnum.Successful)
                {
                    LogEntityErrors(entityOperationResultCollection, entityCollection, remove);

                    LogRelationshipErrors(entityOperationResultCollection);

                    //This list contains list of entity Id's which are warned.
                    //May this entities became error after the entity process. So need to remove from the warn entity status count
                    warningEntityIdList = from eor in entityOperationResultCollection where eor.OperationResultStatus == OperationResultStatusEnum.CompletedWithWarnings select eor.ReferenceId;
                }

                #endregion

                if (entityCollection != null && entityCollection.Count > 0)
                {
                    EntityOperationResultCollection apiEntityOperationResultCollection = ProcessEntitiesUsingAPI(entityCollection, entitiesWithMatching, entitiesWithoutMatching);

                    if (apiEntityOperationResultCollection.OperationResultStatus == OperationResultStatusEnum.Failed)
                    {
                        //Check if the reprocess of entities are required.
                        //This is based on whether any system error occurred for entities.
                        Boolean reprocessEntities = apiEntityOperationResultCollection.HasAnySystemError();

                        if (reprocessEntities)
                        {
                            String batchFailureReprocessBehavior =
                                AppConfigurationHelper.GetAppConfig<String>("MDMCenter.Imports.BatchFailureReprocessBehavior");

                            switch (batchFailureReprocessBehavior)
                            {
                                case "ReProcessBatch":
                                    apiEntityOperationResultCollection = ProcessEntitiesUsingAPI(entityCollection, entitiesWithMatching, entitiesWithoutMatching);
                                    break;
                                case "ReProcessSingleAtTime":
                                    apiEntityOperationResultCollection.Clear();
                                    foreach (Entity entity in entityCollection)
                                    {
                                        EntityOperationResult operationResult = ProcessEntityUsingAPI(entity);
                                        if (operationResult != null)
                                        {
                                            apiEntityOperationResultCollection.Add(operationResult);
                                        }
                                    }
                                    break;
                            }
                        }
                    }
                    
                    #region Fill back the ids from the operation results

                    // Fill back the ids from the operations result
                    foreach (Entity entity in entityCollection)
                    {
                        var filteredEntity = from original in apiEntityOperationResultCollection
                                             where
                                             (original.ReferenceId.Equals(entity.ReferenceId))
                                             select original;

                        // when the external id belongs to multiple container, this might return more than one.
                        // Since there is no further processing done, we are filling the first one. Ideally we want the return object
                        // entityoperationresult to also include container and entity type.
                        if (filteredEntity.Any())
                        {
                            entity.Id = filteredEntity.First().EntityId;
                        }
                        else
                        {
                            if (_traceSettings.IsBasicTracingEnabled)
                            {
                                message = string.Format("No operation result found for ExternalId:{0}", entity.ExternalId);
                                activity.LogInformation(message);
                            }
                        }
                    }

                    #endregion

                    #region Retry Logic for Relationships waiting for related entity to be created.

                    ReProcessRelationshipsWithWarnings(entityCollection, apiEntityOperationResultCollection, relationshipsWaitingForToEntityCreation);

                    #endregion

                    #region Process Results and update source

                    if (apiEntityOperationResultCollection.OperationResultStatus != OperationResultStatusEnum.Successful)
                    {
                        LogEntityErrors(apiEntityOperationResultCollection, entityCollection, remove, warningEntityIdList);

                        LogRelationshipErrors(apiEntityOperationResultCollection);
                    }

                    // Update the source with success and Ids
                    if (importProcessingType == ImportProcessingType.ValidateAndProcess ||
                        importProcessingType == ImportProcessingType.ValidateMatchAndMerge)
                    {
                        LogRelationshipSuccess(apiEntityOperationResultCollection, entityCollection);

                        #region Update SuccessFul Entities Count

                        Int32 partialsuccessEntitiesCount = 0;
                        Int32 successfulEntityCount = 0;

                        //Get the successful entity list
                        IEnumerable<EntityOperationResult> successfulEntitylist = from entityOperationResult in apiEntityOperationResultCollection
                                                                                  where (entityOperationResult.OperationResultStatus == OperationResultStatusEnum.Successful ||
                                                                                         entityOperationResult.OperationResultStatus == OperationResultStatusEnum.None)
                                                                                  select entityOperationResult;

                        if (warningEntityIdList != null && successfulEntitylist != null)
                        {
                            //If any of the warned Entity id is present in the successful Entity Entity list list then
                            //no need to update as successful Entity because it was updated as warned in the previous steps.
                            IEnumerable<EntityOperationResult> eorList = from eor in successfulEntitylist where warningEntityIdList.Contains(eor.ReferenceId) select eor;

                            if (eorList != null)
                            {
                                partialsuccessEntitiesCount = eorList.Count();
                            }
                        }

                        if (successfulEntitylist != null)
                        {
                            successfulEntityCount = successfulEntitylist.Count();
                        }

                        //Final successful entity count will be (no of successful entity count - partial successful entity count)
                        progressHandler.UpdateSuccessFulEntities(successfulEntityCount - partialsuccessEntitiesCount);

                        #endregion

                        if (importProfile.ProcessingSpecifications.ImportMode != ImportMode.RelationshipLoad &&
                            importProfile.ProcessingSpecifications.ImportMode != ImportMode.RelationshipInitialLoad)
                        {
                            LogEntitySuccess(apiEntityOperationResultCollection);

                            // updating the enity source...back
                            sourceData.UpdateSuccessEntities(entityCollection, application, stagingModule);

                            sourceData.UpdateSuccessAttributes(AttributeModelType.Common, entityCollection, application, stagingModule);
                            progressHandler.UpdateSuccessFulAtttributeBatch(AttributeModelType.Common, successfulEntityCount);

                            sourceData.UpdateSuccessAttributes(AttributeModelType.Category, entityCollection, application, stagingModule);
                            progressHandler.UpdateSuccessFulAtttributeBatch(AttributeModelType.Category, successfulEntityCount);
                        }

                        LogRelationshipAttributesSuccess(apiEntityOperationResultCollection);

                        progressHandler.UpdateSuccessFulAtttributeBatch(AttributeModelType.Relationship, successfulEntityCount);
                    }
                    else
                    {
                        // From the entity operation result find out how many were successful. The failed ones have been updated already.
                        List<EntityOperationResult> entityOperationList = (from operationResult in apiEntityOperationResultCollection
                                                                           where (operationResult.OperationResultStatus == OperationResultStatusEnum.Successful
                                                                           || operationResult.OperationResultStatus == OperationResultStatusEnum.None)
                                                                           select operationResult).ToList();
                        // update status and progress handler
                        progressHandler.UpdateSuccessFulEntities(entityOperationList.Count);
                    }

                    ImportEventArgs importEventComletedArgs = importEventArgs = new ImportEventArgs(entityCollection, apiEntityOperationResultCollection, job, importProfile, MDMCenterApplication.JobService, MDMCenterModules.Import, MDMPublisher.MDM_Import);
                    ImportEventManager.Instance.OnImportBatchProcessCompleted(importEventComletedArgs);

                    if (_traceSettings.IsBasicTracingEnabled)
                    {
                        message = string.Format("Import batch process complete event published");
                        activity.LogInformation(message);
                    }

                    #endregion
                }
            }
            catch (Exception ex)
            {
                activity.LogError(ex.Message);
                throw;
            }
            finally
            {
                if (_traceSettings.IsBasicTracingEnabled)
                {
                    message = string.Format("Process entities completed for ({0}) entities.", entityCollection.Count);
                    activity.LogInformation(message);
                    activity.Stop();
                }
            }

            return true;
        }

        #endregion

        #region Step 1.2: Relationship Initial Load

        /// <summary>
        /// Runs per thread.
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="batchSize"></param>
        /// <param name="numberOfAttributesThreadPerEntity"></param>
        /// <param name="threadnumber"></param>
        private void ProcessRelationships(Int64 start, Int64 end, int batchSize, int numberOfAttributesThreadPerEntity, int threadnumber)
        {
            string message = string.Empty;
            DiagnosticActivity activity = new DiagnosticActivity();
            if (_traceSettings.IsBasicTracingEnabled) activity.Start();

            #region Local Variables

            int batchNumber = 0;
            Int64 totalBatches = (end - start) / batchSize;
            EntityOperationResultCollection errorCollection = null;
            // for logging purpose get the count
            int entitybatchCount = 0;
            Int64 processedentityCount = 0;
            int errorEntityCount = 0;
            bool entityProcess = false;
            bool updateRelationshipProgressHandler = true;

            #endregion Local Variables

            if ((totalBatches * batchSize) != (end - start))
                totalBatches++;
            // Within the given boundaries, run the individual processing in batches..
            Int64 ctr = start;
            try
            {
                while (true)
                {
                    #region Prepare for Multiple Batched entity Process

                    // This is only applicable for multiple batched source data..Rest of the providers do not use this...
                    batchNumber++;
                    long startBatchTime = DateTime.Now.Ticks;
                    Int64 batchStart = ctr;
                    Int64 batchEnd = ctr + batchSize - 1;
                    // make sure we dont cross the boundary..
                    if (batchEnd > end)
                        batchEnd = end;

                    #endregion Prepare for Multiple Batched entity Process

                    #region Get Source Relationship Data

                    // Get the staging from the data source
                    if (_traceSettings.IsBasicTracingEnabled)
                        activity.LogInformation(String.Format("Entity processing for batch {0} and {1} started on thread {2}", batchStart, batchEnd, threadnumber));

                    EntityCollection entityCollection = null;
                    try
                    {
                        switch (sourceData.GetBatchingType())
                        {
                            case ImportProviderBatchingType.Multiple:
                                entityCollection = sourceData.GetRelationshipDataBatch(batchStart, batchEnd, application, stagingModule);
                                break;
                            case ImportProviderBatchingType.Single:
                                throw new Exception("Not Support Batching Type for Relationship");
                            case ImportProviderBatchingType.None:
                                throw new Exception("Not Support Batching Type for Relationship");
                        }
                    }
                    catch (Exception ex)
                    {
                        message = string.Format("Entity processing for batch {0} and {1} failed on thread {2}.", batchStart, batchEnd, threadnumber);
                        message = String.Format("Job Id {0} - {1}. The exception message is {2}", job.Id, message, ex.ToString());

                        activity.LogError(message);
                        LogHandler.WriteErrorLog(ex.Message, 100);

                        // This is an hard error. This thread has to stop..
                        message = String.Format("Getting data from the source provider failed with exception {0}. Fix the errors and try again.", ex.Message);
                        UpdateJobStatus(JobStatus.Aborted, message);
                        return;
                    }
                    // Get the staging from the data source
                    if (_traceSettings.IsBasicTracingEnabled)
                        activity.LogInformation(String.Format("Relationship processing for batch {0} and {1} started on thread {2}. Got staging entities from staging.", batchStart, batchEnd, threadnumber));

                    #endregion Get Source Relationship Data

                    #region Process

                    if (entityCollection == null || entityCollection.Count <= 0)
                    {
                        // no data to process...continue to next batch..
                        message = string.Format("Job Id {0} - No entities available for processing for batch {0} and {1} on thread {2}.", job.Id, batchStart, batchEnd, threadnumber);

                        if (_traceSettings.IsBasicTracingEnabled)
                            activity.LogInformation(message);

                        LogHandler.WriteInformationLog(message, 0);
                        // when the import type is not a database type
                        if (sourceData.GetBatchingType() != ImportProviderBatchingType.Multiple)
                            return;
                    }
                    else
                    {
                        // for logging purpose get the count
                        entitybatchCount = entityCollection.Count;
                        processedentityCount = entitybatchCount;
                        errorEntityCount = 0;

                        // validate also removes error entities from the list..based on mode
                        bool remove = (importProcessingType == ImportProcessingType.ValidateAndProcess || importProcessingType == ImportProcessingType.ValidateMatchAndMerge) ? true : false;

                        EntityCollection entitiesWithMatching = null;
                        EntityCollection entitiesWithoutMatching = null;

                        errorCollection = FillAndValidateEntities(entityCollection, remove, threadnumber, out entitiesWithMatching, out entitiesWithoutMatching);

                        try
                        {
                            if (entityCollection.Count > 0)
                            {
                                if (importProfile.ProcessingSpecifications.ImportMode == ImportMode.RelationshipInitialLoad)
                                {
                                    entityProcess = ProcessRelationshipsBulkMode(entityCollection, numberOfAttributesThreadPerEntity, threadnumber);
                                }
                                else
                                {
                                    entityProcess = ProcessEntityMode(entityCollection, threadnumber, entitiesWithMatching, entitiesWithoutMatching);
                                }
                            }
                            else
                            {
                                // entity collection became empty..either we did not get any item from staging or..all failed valiation.
                                processedentityCount = 0;
                                entityProcess = true;
                            }
                        }
                        catch (Exception ex)
                        {
                            message = String.Format("Processing entities batch {0} and {1} failed on thread {2} failed with the exception {3}.", batchStart, batchEnd, threadnumber, ex.Message);
                            message = String.Format("Job Id {0} - {1}", job.Id, message);
                            UpdateRelationshipOperationResults(entityCollection, errorCollection, ex);

                            activity.LogError(message);
                            LogHandler.WriteErrorLog(message, 100);
                            job.JobData.OperationResult.Errors.Add(new Error("100", message));
                            UpdateJobStatus(JobStatus.Running);
                            entityProcess = false;
                        }

                        #region Record Processing Results

                        // take care of updating the source with the error information
                        if (errorCollection != null && errorCollection.Count > 0)
                        {
                            errorEntityCount = errorCollection.Count;
                            processedentityCount = entitybatchCount - errorEntityCount;

                            if (entityProcess == false)
                                updateRelationshipProgressHandler = false;

                            LogEntityErrors(errorCollection, entityCollection, false);

                            LogRelationshipErrors(errorCollection);
                        }

                        UpdateJobStatus(JobStatus.Running);

                        // Entity process failed..update all the entities..

                        if (entityProcess == false)
                        {
                            // when entity processing failes..we should stop this thread..
                            message = string.Format("Thread {0} encountered error in batch {1}. The batch primary key is between {2} and {3}.", threadnumber, batchNumber, batchStart, batchEnd);
                            message = String.Format("Job Id {0} - {1}", job.Id, message);

                            if (_traceSettings.IsBasicTracingEnabled)
                            {
                                activity.LogError(message);
                            }
                            LogHandler.WriteErrorLog(message, 100);

                            job.JobData.OperationResult.Errors.Add(new Error("100", message));

                            if (updateRelationshipProgressHandler == true)
                            {
                                relationshipProgressHandler.UpdateFailedEntities(entityCollection.Count);
                            }

                            UpdateJobStatus(JobStatus.Running);
                        }
                        else
                        {
                            long endBatchTime = DateTime.Now.Ticks;
                            double oneBatchTime = new TimeSpan(endBatchTime - startBatchTime).TotalSeconds;
                            if (totalBatches == 0)
                                totalBatches = batchNumber;
                            message = string.Format("Thread {0} completed batch {1} of {2} in {3,2:F} seconds. Estimated time left is {4,2:F} seconds. Total entity in the batch was {5}, processed were {6} and errors were {7}.", threadnumber, batchNumber, totalBatches, oneBatchTime, oneBatchTime * (totalBatches - batchNumber), entitybatchCount, processedentityCount, errorEntityCount);
                            message = String.Format("Job Id {0} - {1}", job.Id, message);

                            Console.WriteLine(message);
                            LogHandler.WriteInformationLog(message, 0);

                            if (_traceSettings.IsBasicTracingEnabled)
                                activity.LogInformation(message);
                        }

                        #endregion Record Processing Results
                    }

                    #endregion Process

                    #region Prepare next batch

                    switch (sourceData.GetBatchingType())
                    {
                        case ImportProviderBatchingType.Multiple:
                            {
                                // check if we reached the end..
                                if (ctr == end || ctr + batchSize > end)
                                    return;
                                if (ctr + batchSize < end)
                                    ctr = ctr + batchSize;
                                else
                                    ctr = end;
                            }
                            break;
                        case ImportProviderBatchingType.Single:
                            {
                                // Single batched type providers only stops..when there are no records..so continue the loop..
                            }
                            break;
                        case ImportProviderBatchingType.None:
                            return;
                    }

                    #endregion
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (_traceSettings.IsBasicTracingEnabled) activity.Stop();
            }
        }

        /// <summary>
        /// Process the entities in a fast mode. Entities are processed using entity BL and attributes are processed separately in the bulk mode.
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="numberOfAttributesThread"></param>
        private bool ProcessRelationshipsBulkMode(EntityCollection entityCollection, int numberOfAttributesThread, int threadnumber)
        {
            string message = string.Empty;
            DiagnosticActivity activity = new DiagnosticActivity();
            if (_traceSettings.IsBasicTracingEnabled) activity.Start();

            EntityBL target = new EntityBL();
            EntityOperationResultCollection entityOperationResultCollection = new EntityOperationResultCollection();

            //TDOD : Retry in RelationshipInitial Load??? need to decide
            //Dictionary of Entity Reference Id <Relationships, OperationResults> which contains all relationship waiting for related entity to be created in same batch.
            Dictionary<Int64, Dictionary<RelationshipCollection, RelationshipOperationResultCollection>> relationshipsWaitingForToEntityCreation = new Dictionary<Int64, Dictionary<RelationshipCollection, RelationshipOperationResultCollection>>();

            try
            {

                #region Prepare entity relationships

                EntityCollection entitiesWithoutRelationships = new EntityCollection();

                foreach (Entity entity in entityCollection)
                {
                    EntityOperationResult entityOperationResult = new EntityOperationResult(entity.Id, entity.ReferenceId, entity.LongName, entity.ExternalId);
                    entityOperationResult.OperationResultStatus = OperationResultStatusEnum.Successful;

                    if (entity.Relationships != null && entity.Relationships.Count > 0)
                    {
                        FillAndValidateRelationships(entity, entityOperationResult, activity, ref relationshipsWaitingForToEntityCreation);
                    }

                    if (entity.Relationships == null || entity.Relationships.Count < 1)
                    {
                        entitiesWithoutRelationships.Add(entity);
                    }

                    entityOperationResultCollection.Add(entityOperationResult);
                }

                if (entitiesWithoutRelationships.Count > 0)
                {
                    foreach (Entity entityWithoutRelationships in entitiesWithoutRelationships)
                    {
                        entityCollection.Remove(entityWithoutRelationships);
                    }
                }

                entityOperationResultCollection.RefreshOperationResultStatus();
                if (entityOperationResultCollection.OperationResultStatus != OperationResultStatusEnum.Successful)
                {
                    // for validate mode, do not remove from list..we can continue to validate. For process mode, remove.if entity failed
                    // no need to process attributes.
                    bool remove = (importProcessingType == ImportProcessingType.ValidateAndProcess) ? true : false;
                    LogEntityErrors(entityOperationResultCollection, entityCollection, remove);
                    LogRelationshipErrors(entityOperationResultCollection);
                }

                #endregion Prepare entity relationships

                #region Entity BL Processing

                EntityOperationResultCollection apiProcessEntityOperationResultCollection = new EntityOperationResultCollection();

                // Call the new created bulk create method. This will process entitycollection using core API.
                // The SP will return the list of 'ids' for the newly created entities. We need those to stamp the attributes.
                if (importProcessingType == ImportProcessingType.ValidateAndProcess)
                {
                    if (_traceSettings.IsBasicTracingEnabled)
                    {
                        message = String.Format("Call the entity BL for creating the entityies. Total entity in this batch is {0}", entityCollection.Count());
                        activity.LogInformation(message);
                    }

                    if (entityCollection.Count > 0)
                    {
                        apiProcessEntityOperationResultCollection = target.Process(entityCollection, entityProcessingOptions, this.callerContextWithJobId);
                    }
                }
                else
                {
                    // in validate only mode, we dont process the entity in the database..just proceed, get the attributes from staging and
                    // do validate on those..
                    apiProcessEntityOperationResultCollection = PrepareEntityOperationResultsSchema(entityCollection);
                    EntityValidationBL entityValidation = new EntityValidationBL();
                    entityValidation.Validate(entityCollection, apiProcessEntityOperationResultCollection, new CallerContext(application, MDMCenterModules.Validation));
                }

                if (apiProcessEntityOperationResultCollection.OperationResultStatus != OperationResultStatusEnum.Successful)
                {
                    // for validate mode, do not remove from list..we can continue to validate. For process mode, remove.if entity failed
                    // no need to process attributes.
                    bool remove = (importProcessingType == ImportProcessingType.ValidateAndProcess) ? true : false;
                    LogEntityErrors(apiProcessEntityOperationResultCollection, entityCollection, remove);
                    LogRelationshipErrors(apiProcessEntityOperationResultCollection);
                }

                #endregion

                #region Post BL Processing Get Ids

                // If we processed the data, get the ids out.
                if (importProcessingType == ImportProcessingType.ValidateAndProcess)
                {
                    // TODO - Log and remove it from list
                    // Update the source back with succes and the entity ID
                    sourceData.UpdateSuccessEntities(entityCollection, application, stagingModule);
                    LogEntitySuccess(apiProcessEntityOperationResultCollection);
                    LogRelationshipSuccess(apiProcessEntityOperationResultCollection, entityCollection);
                }

                if (_traceSettings.IsBasicTracingEnabled)
                {
                    message = String.Format("Filling the entity collection back with the entity id from database is done.");
                    activity.LogInformation(message);
                }

                #endregion Post BL Processing Get Ids

                #region Split Entities for Relationship Attribute Processing

                // Now that the relationship is done, split the entity and relationship batch in to batches based on the attributes thread count. Like filling in the buckets proportionately
                int totalEntities = entityCollection.Count();

                // if for some reason we have less number of entties than the attributes thread, then use only that many number of threads.
                if (totalEntities < numberOfAttributesThread)
                    numberOfAttributesThread = totalEntities;

                // Split the entity for the attribute thread
                int entityCount = 0;
                int thread = 0;
                string referenceparameter = string.Empty;

                // Need to get the attrbutes from staging..have to pass this long list of reference numbers...for now, pass as comma delimited text..
                Dictionary<int, String> referenceNumberStringparam = new Dictionary<int, string>(numberOfAttributesThread);

                // go round robin and split the entities in to attribute thread bucket.
                foreach (Entity entity in entityCollection)
                {
                    if (entity.Relationships != null && entity.Relationships.Count > 0)
                    {
                        foreach (Relationship rel in entity.Relationships)
                        {

                            if (entityCount < numberOfAttributesThread)
                                thread = entityCount;
                            else
                                thread = entityCount % numberOfAttributesThread;

                            if (referenceNumberStringparam.Keys.Count > thread)
                                referenceparameter = referenceNumberStringparam[thread];

                            //Get entity's relationships;
                            if (string.IsNullOrEmpty(referenceparameter))
                                referenceparameter = string.Format("'{0}'", rel.RelationshipExternalId.ToString());
                            else
                                referenceparameter = string.Format("{0}, '{1}'", referenceparameter, rel.RelationshipExternalId);


                            if (referenceNumberStringparam.Keys.Count > thread)
                                referenceNumberStringparam[thread] = referenceparameter;
                            else
                                referenceNumberStringparam.Add(thread, referenceparameter);

                            entityCount++;
                            referenceparameter = String.Empty;
                        }
                    }
                }

                if (_traceSettings.IsBasicTracingEnabled)
                {
                    message = String.Format("Spliting the entity collection for the attribute processing is done.");
                    activity.LogInformation(message);
                }
                #endregion

                #region Process Attributes

                long attributesstartTime = DateTime.Now.Ticks;

                bool localProcess = ProcessRelationshipAttributesBulkMode(referenceNumberStringparam, entityCollection, numberOfAttributesThread, threadnumber);

                long attributessEndTime = DateTime.Now.Ticks;

                double attributeProcessingTime = new TimeSpan(attributessEndTime - attributesstartTime).TotalSeconds;

                if (_traceSettings.IsBasicTracingEnabled)
                {
                    message = string.Format("Relationship Attribute processing took both the types took {0} seconds total.", attributeProcessingTime);
                    activity.LogInformation(message);
                }

                #endregion Process Attributes

                #region Bulk Process Reload cache

                if (importProcessingType == ImportProcessingType.ValidateAndProcess)
                {
                    EntityCacheStatusCollection cacheStatusCollection = new EntityCacheStatusCollection();

                    foreach (Entity entity in entityCollection)
                    {
                        if (entity.Id > 0)
                        {
                            cacheStatusCollection.Add(new EntityCacheStatus()
                            {
                                IsRelationshipCacheDirty = true,
                                ContainerId = entity.ContainerId,
                                Action = ObjectAction.Create,
                                EntityId = entity.Id,
                                PerformedAction = MDM.Core.EntityActivityList.RelationshipCreate
                            });
                        }
                    }

                    if (cacheStatusCollection.Count > 0)
                    {
                        EntityCacheStatusBL entityCacheStatusBL = new EntityCacheStatusBL();
                        entityCacheStatusBL.Process(cacheStatusCollection, callerContext);
                    }
                }

                #endregion Bulk Process Reload cache

            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (_traceSettings.IsBasicTracingEnabled) activity.Stop();
            }

            progressHandler.UpdateSuccessFulEntities(entityCollection.Count);

            return true;
        }

        /// <summary>
        /// Fill relationships within entity and validate it.
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="entityOperationResult"></param>
        /// <param name="activity"></param>
        private void FillAndValidateRelationships(Entity entity, EntityOperationResult entityOperationResult, DiagnosticActivity activity, ref Dictionary<Int64, Dictionary<RelationshipCollection, RelationshipOperationResultCollection>> relationshipsWaitingForToEntityCreation)
        {
            Boolean removeBadRelationshipAttributesFromProcessing = false;

            // partial process of relationship attributes is ONLY supported in relationship load or relationship initial load mode ( from staging database actually).
            if (importProfile.ProcessingSpecifications.ImportMode == ImportMode.RelationshipLoad ||
                importProfile.ProcessingSpecifications.ImportMode == ImportMode.RelationshipInitialLoad)
            {
                removeBadRelationshipAttributesFromProcessing = true;
            }
           
            RelationshipOperationResultCollection errorRORCollection = FillandValidateRelationships(entity.Relationships, entity.Id, entity.ExternalId, entity.ContainerId, entity.ReferenceId, true, removeBadRelationshipAttributesFromProcessing, entity, ref relationshipsWaitingForToEntityCreation);

            if (errorRORCollection != null && errorRORCollection.Count > 0)
            {
                entityOperationResult.SetRelationshipOperationResults(errorRORCollection);

                if (entityOperationResult.RelationshipOperationResultCollection.OperationResultStatus == OperationResultStatusEnum.CompletedWithWarnings)
                {
                    entityOperationResult.OperationResultStatus = OperationResultStatusEnum.CompletedWithWarnings;
                }
                else
                {
                    //Key Note: Here, entityOperationResult would be 'CompletedWithErrors' as we dont need to stop processing entity if one of the rel has issue..
                    // partial process of relationship attributes is ONLY supported in relationship load mode and relationship initial load( from staging database actually).
                    if (importProfile.ProcessingSpecifications.ImportMode != ImportMode.RelationshipLoad &&
                        importProfile.ProcessingSpecifications.ImportMode != ImportMode.RelationshipInitialLoad)
                    {
                        entityOperationResult.RelationshipOperationResultCollection.OperationResultStatus = OperationResultStatusEnum.Failed;
                        entityOperationResult.OperationResultStatus = OperationResultStatusEnum.Failed;
                    }
                }
            }

            if (_traceSettings.IsBasicTracingEnabled)
            {
                String message = string.Format("Fill and validate complete for {0} relationships for entity ({1})", entity.Relationships.Count, entity.ExternalId);
                activity.LogInformation(message);
            }
        }

        /// <summary>
        /// Gets and processes the attributes ( based on type) in separate threads. The end result is merged in to a database for bulk inserting.
        /// </summary>
        /// <param name="referenceparameter"></param>
        /// <param name="entities"></param>
        /// <param name="attType"></param>
        /// <param name="numberOfAttributesThread"></param>
        /// <returns></returns>
        private bool ProcessRelationshipAttributesBulkMode(Dictionary<int, String> referenceparameter, EntityCollection entities, int numberOfAttributesThread, int threadnumber)
        {
            string message = string.Empty;
            DiagnosticActivity activity = new DiagnosticActivity();
            if (_traceSettings.IsBasicTracingEnabled) activity.Start();

            bool returnStatus = true;
            String attributeType = String.Empty;
            AttributeModelType attType = AttributeModelType.Relationship;

            #region Prepare Data tables for bulk insert

            // Bulk insert performs best when the data size is big enough.. So this table will merge the results back..
            DataTable combinedTable = null;
            IBulkInsert bulkInsert = null;

            bulkInsert = attributeObjects.GetRelationshipAttribueObject();
            attributeType = "R";

            // The bulk insert needs to fire triggers when we dont process denorm on the initial load.
            bulkInsert.FireTriggers = false;

            combinedTable = bulkInsert.CreateDataTable(mdmVersion);

            if (_traceSettings.IsBasicTracingEnabled)
            {
                message = string.Format("Relationship Attribute processing for type {0}.", attType);
                activity.LogInformation(message);
            }

            #endregion

            #region Attribute Processing

            // Based on number of attributes threads call the attributes methods
            long attributesstartTime = DateTime.Now.Ticks;

            Parallel.For(0, numberOfAttributesThread, i =>
            {
                if (String.IsNullOrEmpty(Thread.CurrentThread.Name))
                {
                    Thread.CurrentThread.Name = String.Format("E{0}{1}{2}", threadnumber, attributeType, i);
                }

                String refparameter = String.Empty;
                referenceparameter.TryGetValue(i, out refparameter);

                Boolean attributeProcess = GetRelationshipAttributesFromStagingAndMergeDataTable(refparameter, entities, combinedTable, bulkInsert);

                if (!attributeProcess)
                {
                    // one of the attribute thread failed..DO not bulk insert the batch..Update the source with the error.
                    returnStatus = false;
                }
            });

            long attributessEndTime = DateTime.Now.Ticks;
            double attributeProcessingTime = new TimeSpan(attributessEndTime - attributesstartTime).TotalSeconds;

            if (_traceSettings.IsBasicTracingEnabled)
            {
                message = "Relationship Attribute processing done. Now bulk insert the combined table";
                activity.LogInformation(message);
            }

            #endregion

            #region Attribute Bulk Insert

            long bulkInsertStartTime = DateTime.Now.Ticks;
            double bulkInsertProcessingTime = 0;
            long bulkInsertEndTime = 0;

            // Only when config says we have to process..and there were no errors on the attribute thread..
            if (returnStatus && importProcessingType == ImportProcessingType.ValidateAndProcess)
            {
                // Bulk insert the combined attribute table
                if (util.BulkCopyDataTable(combinedTable, bulkInsert, mdmVersion, LogHandler, application, importmodule))
                {
                    if (_traceSettings.IsBasicTracingEnabled)
                        activity.LogInformation("Bulk insert the combined table is done. Now process the Denorm data");

                    // Update the source with the processing status..
                    if (sourceData.UpdateSuccessAttributes(attType, entities, application, stagingModule) == false)
                    {
                        //updating status failed..
                        message = string.Format("Updating the staging data source for successsul bulk processing of attribute type {0} failed ", attType);
                        message = String.Format("Job Id {0} - {1}", job.Id, message);

                        if (_traceSettings.IsBasicTracingEnabled)
                        {
                            activity.LogError(message);
                        }
                        LogHandler.WriteErrorLog(message, 100);
                    }

                    progressHandler.UpdateSuccessFulAtttributeBatch(attType, entities.Count());
                }
                else
                {
                    if (_traceSettings.IsBasicTracingEnabled)
                    {
                        message = string.Format("Bulk insert for attribute type {0} failed. Check the log for more details.", attType);
                        activity.LogError(message);
                    }
                    returnStatus = false;
                }
            }

            bulkInsertEndTime = DateTime.Now.Ticks;
            bulkInsertProcessingTime = new TimeSpan(bulkInsertEndTime - bulkInsertStartTime).TotalSeconds;

            if (_traceSettings.IsBasicTracingEnabled)
            {
                message = string.Format("Attribute processing for {0} type for ALL worker threads took {1} seconds. Bulk insert took {2} seconds.", attType, attributeProcessingTime, bulkInsertProcessingTime);
                activity.LogInformation(message);
            }

            #endregion

            #region Update Error Status

            // This failure happens when the GetAttributesFromStagingAndMergeDataTable threw an exception or when bulk insert failed..
            // update all the attributes for the source entities as failed.
            if (returnStatus == false)
            {
                foreach (String failedRelationshipExternalIds in referenceparameter.Values)
                {
                    sourceData.UpdateErrorAttributes(attType, failedRelationshipExternalIds, "Bulk insert failed for the batch. Check the file dump folder to retry.", application, stagingModule);
                }

                progressHandler.UpdateFailedAtttributeBatch(attType, entities.Count());

                message = String.Format("Bulk process of {0} attributes failed due to an exception. Correct the errors and re run the processing for this batch", attType);

                EntityOperationResultCollection errorCollection = GetEntityErrorCollection(entities, message);
                if (errorCollection != null)
                {
                    LogEntityErrors(errorCollection, entities, false);
                    LogRelationshipErrors(errorCollection);
                }
            }

            #endregion

            if (_traceSettings.IsBasicTracingEnabled) activity.Stop();

            return returnStatus;
        }

        /// <summary>
        /// For the given batch, get the attributes from the staging tables.
        /// </summary>
        /// <param name="referenceparameter"></param>
        /// <param name="entities"></param>
        /// <param name="attType"></param>
        /// <param name="combinedTable"></param>
        /// <returns></returns>
        private Boolean GetRelationshipAttributesFromStagingAndMergeDataTable(String referenceparameter, EntityCollection entities, DataTable combinedTable, IBulkInsert bulkInsert)
        {
            String message = String.Empty;
            // Diagnostics/Tracing not supported yet for this method in Prallel.For - ToDo

            DiagnosticActivity activity = new DiagnosticActivity();
            if (_traceSettings.IsBasicTracingEnabled)
            {
                activity.Start();
            }

            bool returnStatus = true;
            AttributeModelType attType = AttributeModelType.Relationship;

            if (string.IsNullOrEmpty(referenceparameter))
            {
                // no entities for processing in this thread..
                return returnStatus;
            }

            #region Create Data tables and other local variables

            // Local data table to store the values..If memory is an issue, switch to a local file using the other sample...
            DataTable caltable = bulkInsert.CreateDataTable(mdmVersion);

            // Get the attribute data from staging..
            AttributeCollection attributes = sourceData.GetAttributeDataforEntityList(attType, referenceparameter, entities, application, stagingModule);

            EntityOperationResultCollection entityErrors = new EntityOperationResultCollection(entities);
            AttributeOperationResultCollection attributeErrors = new AttributeOperationResultCollection();

            #endregion

            try
            {
                Dictionary<string, int> attributeSequenceNumber = new Dictionary<string, int>();

                long methodstartTime = DateTime.Now.Ticks;
                long startTime = DateTime.Now.Ticks;

                long endTime = DateTime.Now.Ticks;

                double getsourceDataTime = new TimeSpan(endTime - startTime).TotalMilliseconds;
                double fillProcessingTime, matchingProcessingTime, validateProcessingTime, dataRowProcessingTime;

                fillProcessingTime = matchingProcessingTime = validateProcessingTime = dataRowProcessingTime = 0;

                #region Attribute processing


                RelationshipOperationResult relOperationResult = null;
                EntityOperationResult entityWithRelOR = null;
                Relationship relationship = new Relationship();
                Entity entityWithRel = new Entity();

                foreach (Attribute attribute in attributes)
                {
                    #region Find matching relationship and entity

                    if (relOperationResult == null || relationship.RelatedEntityId != attribute.EntityId)
                    {
                        Tuple<Entity, Relationship> matchingRelationshipAndEntity = FindMatchingRelationship(attribute.EntityId, entities);
                        entityWithRel = matchingRelationshipAndEntity.Item1;
                        relationship = matchingRelationshipAndEntity.Item2;

                        entityWithRelOR = entityErrors.GetByEntityId(entityWithRel.Id) as EntityOperationResult;
                        relOperationResult = entityWithRelOR.RelationshipOperationResultCollection.GetRelationshipOperationResult(relationship.Id);
                    }

                    #endregion Find matching relationship and entity

                    #region Fill in Attribute information

                    startTime = DateTime.Now.Ticks;

                    // We do not have to do the Fill for the parent entity. The attribute will be inherited.
                    if (attribute.SourceFlag != AttributeValueSource.Inherited)
                    {
                        FillAttribute(relationship, attribute);
                    }

                    // special condition for complex attribute loading from staging..
                    if (attribute.IsComplex)
                    {
                        foreach (Value value in attribute.GetCurrentValues())
                        {
                            Int32 valueRefId = -1;
                            if (Int32.TryParse(value.AttrVal.ToString(), out valueRefId))
                            {
                                value.ValueRefId = valueRefId;
                            }
                        }
                    }
                    endTime = DateTime.Now.Ticks;
                    fillProcessingTime += new TimeSpan(endTime - startTime).TotalMilliseconds;

                    #endregion

                    #region Validate Attributes

                    startTime = DateTime.Now.Ticks;

                    // the staging primary key is stored in the id property of the attribute value.
                    Int32 stagingAttributePk = ValueTypeHelper.Int32TryParse(attribute.GetCurrentValuesInvariant().ElementAt(0).Id.ToString(), -1);

                    AttributeOperationResult attributeOperationResult = new AttributeOperationResult(stagingAttributePk, attribute.Name, attribute.LongName, attType, attribute.Locale);

                    if (attribute.IsLookup)
                    {
                        SetLookupAttributeAsBlank(attribute, importProfile);
                    }

                    ILocaleMessageManager localeMessageBL = new LocaleMessageBL();
                    EntityOperationsHelper.ValidateUnmappedAttributes(entityWithRel, attribute, relationship, attributeOperationResult, localeMessageBL, entityProcessingOptions, callerContext, OperationResultType.Error);

                    if (importProfile.ProcessingSpecifications.ImportMode == ImportMode.InitialLoad ||
                        importProfile.ProcessingSpecifications.ImportMode == ImportMode.RelationshipInitialLoad)
                    {
                        AttributeModel attributeModel = GetAttributeModelFromCachedDataModel(entityWithRel, attribute);
                        EntityOperationsHelper.ValidateEntityAttributesForImport(entityWithRel, relationship, attribute, attributeModel, attributeOperationResult, entityProcessingOptions, callerContext, localeMessageBL);
                    }

                    if (attributeOperationResult.HasError)
                    {
                        // if the attribute is a collection..add error for each..
                        if (attribute.GetCurrentValuesInvariant().Count > 1)
                        {
                            AttributeOperationResultCollection resultCollection = ExplodeAttributeOperationResultForCollection(attribute, attributeOperationResult);

                            foreach (AttributeOperationResult result in resultCollection)
                            {
                                attributeErrors.Add(result);
                            }

                            relOperationResult.SetAttributeOperationResult(resultCollection);
                        }
                        else
                        {
                            //Get the requested entity operation result
                            relOperationResult.SetAttributeOperationResult(new AttributeOperationResultCollection() { attributeOperationResult });
                            attributeErrors.Add(attributeOperationResult);
                        }
                        continue;
                    }

                    AttributeModelContext attributeModelContext = new AttributeModelContext()
                    {
                        ContainerId = entityWithRel.ContainerId,
                        CategoryId = entityWithRel.CategoryId,
                        EntityTypeId = entityWithRel.EntityTypeId,
                        Locales = new Collection<LocaleEnum>() { attribute.Locale },
                        AttributeModelType = attribute.AttributeModelType,
                        RelationshipTypeId = relationship.RelationshipTypeId
                    };

                    if (importProfile.ProcessingSpecifications.EntityProcessingOptions.ValidateEntities || importProcessingType == ImportProcessingType.ValidationOnly)
                    {
                        EntityValidationBL entityValidation = new EntityValidationBL();
                        entityValidation.ValidateAttribute(attribute, attributeModelContext, attributeOperationResult, new CallerContext(application, MDMCenterModules.Validation));

                        // if we hit a validation error, don't process further..
                        if (attributeOperationResult.OperationResultStatus != OperationResultStatusEnum.Successful)
                        {
                            relOperationResult.SetAttributeOperationResult(new AttributeOperationResultCollection() { attributeOperationResult });
                            attributeErrors.Add(attributeOperationResult);
                            continue;
                        }
                    }

                    endTime = DateTime.Now.Ticks;
                    validateProcessingTime += new TimeSpan(endTime - startTime).TotalMilliseconds;

                    #endregion

                    #region Create Data Table to process attributes

                    // Validations are done..time to process..
                    if (importProcessingType == ImportProcessingType.ValidateAndProcess)
                    {
                        if (attribute.SourceFlag != AttributeValueSource.Inherited)
                        {
                            startTime = DateTime.Now.Ticks;

                            Int32 totalValueRecords = attribute.GetCurrentValuesInvariant().Count;

                            DataRow[] dataRows = new DataRow[totalValueRecords];

                            for (int rownumber = 0; rownumber < totalValueRecords; rownumber++)
                            {
                                dataRows[rownumber] = caltable.NewRow();
                            }

                            bulkInsert.FillDataRow(relationship, attribute, dataRows, mdmVersion, auditRefId);

                            for (int rownumber = 0; rownumber < totalValueRecords; rownumber++)
                            {
                                caltable.Rows.Add(dataRows[rownumber]);
                            }

                            endTime = DateTime.Now.Ticks;
                            dataRowProcessingTime += new TimeSpan(endTime - startTime).TotalMilliseconds;
                        }
                    }

                    #endregion Create Data Table to process attributes
                }

                #endregion Attribute processing

                #region Post Process Merge Data Table

                // Only when config says we have to process..
                if (importProcessingType == ImportProcessingType.ValidateAndProcess)
                {
                    //Not sure if data table merge is thread safe..put a lock
                    startTime = DateTime.Now.Ticks;
                    lock (lockObject)
                    {
                        if (caltable.Rows.Count > 0)
                        {
                            combinedTable.Merge(caltable);
                        }
                    }

                    endTime = DateTime.Now.Ticks;
                    double attributeMergeTime = new TimeSpan(endTime - startTime).TotalSeconds;
                    double attributeProcessingTime = new TimeSpan(endTime - methodstartTime).TotalSeconds;

                    String timetaken = String.Format("Get source data is {0} ms, fill time is {1} ms, validate time is {2} ms, data row processing time is {3} ms, Dataset merge time is {4}",
                        getsourceDataTime, fillProcessingTime, validateProcessingTime, dataRowProcessingTime, attributeMergeTime);
                }

                #endregion Post Process Merge Data Table

            }
            catch (Exception ex)
            {
                message = String.Format("GetAttributesFromStagingAndMergeDatatable failed with exception {0}", ex.ToString());
                returnStatus = false;
            }

            #region Record Attrbute Errors

            long errorstartTime = DateTime.Now.Ticks;

            // Process the errors also..
            if (entityErrors != null && entityErrors.Count > 0)
            {
                entityErrors.RefreshOperationResultStatus();
                LogEntityErrors(entityErrors, entities, false);
            }

            //update the attribute level errors also..
            if (attributeErrors != null && attributeErrors.Count() > 0)
            {
                //attributeErrors.AttributeType = attType;
                if (sourceData.UpdateErrorAttributes(attributeErrors, application, stagingModule) == false)
                {
                    // Updating error fails??
                    message = string.Format("Updating error status for attribute type {0} failed.", attType);

                    if (_traceSettings.IsBasicTracingEnabled)
                    {
                        activity.LogError(message);
                    }
                    LogHandler.WriteErrorLog(message, 100);
                }

            }
            long errorEndTime = DateTime.Now.Ticks;
            double errorProcessingTime = new TimeSpan(errorEndTime - errorstartTime).TotalSeconds;


            #endregion

            if (_traceSettings.IsBasicTracingEnabled) activity.Stop();

            return returnStatus;
        }

        private void ReadRelationshipAttributes(Entity entity, EntityCollection entities, DiagnosticActivity activity)
        {
            if (entity.Relationships != null && entity.Relationships.Count > 0)
            {
                foreach (Relationship relationship in entity.Relationships)
                {
                    string relationshipExternalIds = string.Format("'{0}'", relationship.RelationshipExternalId);

                    AttributeCollection relationshipAttributes = sourceData.GetAttributeDataforEntityList(AttributeModelType.Relationship, relationshipExternalIds, entities, application, stagingModule);

                    if (relationshipAttributes != null && relationshipAttributes.Count > 0)
                    {
                        relationship.RelationshipAttributes.AddRange(relationshipAttributes);
                    }

                    if (_traceSettings.IsBasicTracingEnabled)
                    {
                        int count = relationshipAttributes == null ? 0 : relationshipAttributes.Count;
                        if (count > 0)
                        {
                            String message = string.Format("Retrieved {0} relationship attributes from external relationship ({1})", count, relationship.RelationshipExternalId);
                            activity.LogInformation(message);
                        }
                    }
                }
            }
        }

        private Tuple<Entity, Relationship> FindMatchingRelationship(Int64 relatedEntityId, EntityCollection entities)
        {
            Tuple<Entity, Relationship> returnValue = null;
            foreach (Entity entity in entities)
            {
                foreach (Relationship rel in entity.Relationships)
                {
                    if (rel.RelatedEntityId == relatedEntityId)
                    {
                        returnValue = new Tuple<Entity, Relationship>(entity, rel);
                        break;
                    }
                }
            }

            return returnValue;
        }

        #endregion

        #region Step 1.3: Complex Initial Load

        /// <summary>
        /// Process the complex attribute
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="batchSize"></param>
        /// <param name="numberOfAttributesThreadPerEntity"></param>
        /// <param name="threadnumber"></param>
        private void ProcessComplexAttribute(long start, long end, int batchSize, int numberOfAttributesThreadPerEntity, int threadnumber)
        {
            ComplexAttributeProcessor complexProcess = new ComplexAttributeProcessor()
            {
                StagingTableName = "test",
                AttributeName = "test",
                AttributeParentName = "test",
                AttributeId = 1,
                StartingPk = start,
                EndingPk = end,
                BatchSize = batchSize
            };
            complexProcess.Process();
        }

        #endregion

        #endregion

        #region Entity Object Mgt

        /// <summary>
        /// Get the entities from data providers.
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="threadnumber"></param>
        /// <param name="containerName"></param>
        /// <returns></returns>
        private EntityCollection GetEntitiesFromProvider(Int64 start, Int64 end, Int32 threadnumber, String containerName, String organizationName, String entityTypeName)
        {
            string message = string.Empty;

            Boolean isTracingEnabled = _traceSettings.IsBasicTracingEnabled;
            DiagnosticActivity activity = null;
            if (isTracingEnabled)
            {
                activity = new DiagnosticActivity();
                activity.Start();
            }

            EntityCollection entityCollection = null;
            EntityProviderContext entityproviderContext = new EntityProviderContext();

            entityproviderContext.EntityProviderContextType = EntityProviderContextType.All;

            if (String.IsNullOrEmpty(containerName) == false)
            {
                entityproviderContext.EntityProviderContextType = EntityProviderContextType.Container;
                entityproviderContext.ContainerName = containerName;
            }
            else if (String.IsNullOrEmpty(organizationName) == false)
            {
                entityproviderContext.EntityProviderContextType = EntityProviderContextType.Organization;
                entityproviderContext.OrganizationName = organizationName;
            }
            else if (String.IsNullOrEmpty(entityTypeName) == false)
            {
                entityproviderContext.EntityProviderContextType = EntityProviderContextType.EntityType;
                entityproviderContext.EntityTypeName = entityTypeName;
            }

            try
            {
                switch (sourceData.GetBatchingType())
                {
                    case ImportProviderBatchingType.Multiple:
                        {
                            if (importProfile.ProcessingSpecifications.ImportMode == ImportMode.RelationshipLoad ||
                                importProfile.ProcessingSpecifications.ImportMode == ImportMode.RelationshipInitialLoad)
                            {
                                entityCollection = sourceData.GetRelationshipDataBatch(start, end, application, stagingModule);
                            }
                            else
                            {
                                entityCollection = sourceData.GetEntityDataBatch(start, end, application, stagingModule, entityproviderContext);
                            }
                            break;
                        }
                    case ImportProviderBatchingType.Single:
                        {
                            if (importProfile.ProcessingSpecifications.ImportMode == ImportMode.RelationshipLoad ||
                                importProfile.ProcessingSpecifications.ImportMode == ImportMode.RelationshipInitialLoad)
                            {
                                entityCollection = sourceData.GetRelationshipDataNextBatch(batchSize, application, stagingModule);
                            }
                            else
                            {
                                entityCollection = sourceData.GetEntityDataNextBatch(batchSize, application, stagingModule, entityproviderContext);
                            }
                        }
                        break;
                    case ImportProviderBatchingType.None:
                        if (importProfile.ProcessingSpecifications.ImportMode == ImportMode.RelationshipLoad ||
                            importProfile.ProcessingSpecifications.ImportMode == ImportMode.RelationshipInitialLoad)
                        {
                            entityCollection = sourceData.GetAllRelationshipData(application, stagingModule);
                        }
                        else
                        {
                            entityCollection = sourceData.GetAllEntityData(application, stagingModule, entityproviderContext);
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                if (isTracingEnabled)
                {
                    message = string.Format("Getting data from the source provider for batch {0} and {1} failed on thread {2}.", start, end, threadnumber);
                    message = string.Format("Job Id {0} - {1}. The exception is {2}", job.Id, message, ex.ToString());

                    activity.LogError(message);
                }
            }

            if (isTracingEnabled)
            {
                message = string.Format("Retrieved ({0}) entities from provider ({1}) for requested batch size ({2}).", entityCollection.Count, importSourceType.ToString(), batchSize);
                activity.LogInformation(message);
                activity.Stop();
            }

            return entityCollection;
        }

        /// <summary>
        /// Before we put them in core, run through some basic validations for all the entities. Just to spend some CPU cycles.
        /// </summary>
        private EntityOperationResultCollection FillAndValidateEntities(EntityCollection entityCollection, bool removeBadEntitiesFromProcessing, int threadnumber, out EntityCollection entitiesWithMatching, out EntityCollection entitiesWithoutMatching)
        {
            string message = string.Empty;

            Boolean isTracingEnabled = _traceSettings.IsBasicTracingEnabled;
            DiagnosticActivity activity = null;
            if (isTracingEnabled)
            {
                activity = new DiagnosticActivity();
                activity.Start();
            }

            // Collect all the error items..
            EntityOperationResultCollection errorCollection = new EntityOperationResultCollection();
            EntityMapCollection entityMapCollection = new EntityMapCollection();

            HashSet<Entity> badEntities = new HashSet<Entity>();

            Int64 entityIdToBeCreated = -1;
            Int64 parentEntityMapRefDiff = 1000000; // We will never have batch size more than 1000K so using this range..

            Boolean categoryMappingEnabled = importProfile.MappingSpecifications.EntityIdentificationMap.Mappings.Contains(Constants.CATEGORY_SOURCE_NAME_MAPPING, Constants.CATEGORY_TARGET_NAME_MAPPING);
            
                entitiesWithMatching = new EntityCollection();
                entitiesWithoutMatching = new EntityCollection();
            
            #region Prepare Entity Map objects

            // loop through, fill and prepare entity map collection
            foreach (Entity entity in entityCollection)
            {
                // Trim externalId to validate for duplicates after trimming (no duplicates are allowed)
                entity.ExternalId = entity.ExternalId.Trim();

                if (entity.Id < 1)
                {
                    entity.Id = entityIdToBeCreated;
                    entityIdToBeCreated--;
                }

                // we need a unique reference id for an entity in this batch. If the source provider has given it, use it. Otherwise
                // generate our own. The entityIdToBeCreated seems to be a running number ( negative). It can be used for this purpose.
                if (entity.ReferenceId < 0)
                {
                    entity.ReferenceId = entity.Id;
                }

                // do the fills...Try as much as possible to get these from cache and avoid
                // hitting the database. Remember we are in a loop here!!!
                FillEntity(entity);

                //Try to find map only for entities and ignore categories..
                if (entity.BranchLevel == ContainerBranchLevel.Component)
                {
                    EntityMap entityMap = null;

                    if (entityProcessingOptions.CanReclassifyEntities)
                    {
                        if (isTracingEnabled)
                        {
                            message = "CanReclassifyEntities flag set to true, preparing entity map without category";
                            activity.LogInformation(message);
                        }

                        //If reclassification allowed then get the entity map without category.
                        entityMap = new EntityMap(entity.Id, systemId, entity.ObjectTypeId, entity.ObjectType, entity.ExternalId, -1, entity.ContainerId, 0, entity.EntityTypeId);
                    }
                    else
                    {
                        //create entity map..
                        //Here, we are using entity.Id as EntityMap.Id too for reference purpose..
                        Int64 mappingCategoryId = 0;
                        if (categoryMappingEnabled && (entity.CategoryId >= 0 || entity.Action != ObjectAction.Update))
                        {
                            mappingCategoryId = entity.CategoryId;
                        }

                        entityMap = new EntityMap(entity.Id, systemId, entity.ObjectTypeId, entity.ObjectType, entity.ExternalId, -1, entity.ContainerId, mappingCategoryId, entity.EntityTypeId);
                    }

                    if (entityMap != null)
                    {
                        entityMapCollection.Add(entityMap);

                        if (isTracingEnabled)
                        {
                            message = string.Format("Entity map found for external entity ({0}), mapped entity ({1})", entity.ExternalId, entityMap.ExternalId);
                            activity.LogInformation(message);
                        }
                    }
                    else
                    {
                        if (isTracingEnabled)
                        {
                            message = string.Format("Entity map not found for external entity ({0}), mapped entity ({1}", entity.ExternalId);
                            activity.LogInformation(message);
                        }
                    }

                    //Do parent entity map get only when parent entity id is not yet populated
                    if (entity.ParentEntityId <= 0 && entity.ParentEntityTypeId > 0)
                    {
                        //create parent entity map..
                        Int64 parentEntityMapRefId = entity.Id - parentEntityMapRefDiff;
                        //CRITICAL: We are using entity's containerid and category id to find parent. IF any of this is not provided and uniqueness is based on that then parent entity map may get wrong map..
                        EntityMap parentEntityMap = new EntityMap(parentEntityMapRefId, systemId, entity.ObjectTypeId, entity.ObjectType, entity.ParentExternalId, -1, entity.ContainerId, entity.CategoryId, entity.ParentEntityTypeId);

                        entityMapCollection.Add(parentEntityMap);

                        if (isTracingEnabled)
                        {
                            message = string.Format("Parent entity map created for entity ({0}), mapped parent entity ({1})", entity.ExternalId, parentEntityMap.ExternalId);
                            activity.LogInformation(message);
                        }
                    }
                }
            }
            
            #endregion Prepare Entity Map objects

            #region Load Entity Map for all the entities

            if (entityMapCollection != null && entityMapCollection.Count > 0)
            {
                EntityMapBL entityMapManager = new EntityMapBL();

                try
                {
                    bool isEntityMapLoaded = entityMapManager.LoadInternalDetails(entityMapCollection, importProfile.MappingSpecifications.EntityIdentificationMap, application, importmodule);

                    if (isTracingEnabled)
                    {
                        message = string.Format("Entity map internal details loaded for {0} entities", entityMapCollection.Count);
                        activity.LogInformation(message);
                    }
                }
                catch (Exception ex)
                {
                    if (isTracingEnabled)
                    {
                        message = string.Format("Entity map internal details load failed : {0}", ex.Message);

                        activity.LogError(message);
                    }
                    throw;
                }
            }

            #endregion

            #region Fill Entity InternalId, ParentEntityId and Later Validate Entities in loop and also Get Extension Relationship from source data and Validate

            foreach (Entity entity in entityCollection)
            {
                EntityOperationResult entityOperationResult = new EntityOperationResult(entity.Id, entity.ReferenceId, entity.LongName, entity.ExternalId);

                if (entity.BranchLevel == ContainerBranchLevel.Component)
                {
                    #region Fetch Parent Entity Map from the collection

                    if (entity.ParentEntityId <= 0 && entity.ParentEntityTypeId > 0)
                    {
                        Int64 parentEntityMapRefId = entity.Id - parentEntityMapRefDiff;

                        EntityMap parentEntityMap = (EntityMap)entityMapCollection.GetEntityMap(parentEntityMapRefId);

                        //Set parent entity id..
                        if (parentEntityMap != null)
                        {
                            if (parentEntityMap.InternalId > 0)
                            {
                                entity.ParentEntityId = parentEntityMap.InternalId;
                                entity.EntityFamilyId = parentEntityMap.EntityFamilyId;
                                entity.EntityGlobalFamilyId = parentEntityMap.EntityGlobalFamilyId;
                            }
                        }
                    }

                    #endregion

                    Boolean hasMatchingProfile = false;
                    if (importProfile.ProcessingSpecifications.ImportProcessingType == ImportProcessingType.ValidateMatchAndMerge
                        && (entity.Action == ObjectAction.Read || entity.Action == ObjectAction.Create || entity.Action == ObjectAction.Update))
                    {
                        //Find the matching profile
                        ObjectAction entityAction = ObjectAction.Unknown;

                        #region Set SourceId
                        
                        if (_entityDataSourceTrackingConfig != null && _entityDataSourceTrackingConfig.IsEnabled)
                        {
                            if (importProfile.InputSpecifications.SourceId.HasValue)
                            {
                                entityAction = entity.Action;
                                entity.Action = ObjectAction.Create;

                                new SourceBL().SetSourceToAllEntityDescendants(entity,
                                importProfile.InputSpecifications.SourceId.Value);
                            }
                        }

                        #endregion

                        //IMatchingProfileCollection matchProfiles = matchingService.GetAllMatchingProfiles();
                        //MatchingProfile matchProfile = EntityOperationsHelper.GetBestMatchProfileForEntity(entity, matchProfiles, callerContext);

                        //if (matchProfile != null )
                        //{
                        //    hasMatchingProfile = true;

                        //    entitiesWithMatching.Add(entity);
                        //}
                        //else
                        //{
                        //    entity.Action = entityAction;
                        //    entity.SourceInfo = null;

                        //    entitiesWithoutMatching.Add(entity);
                        //}
                    }
                    else
                    {
                        entitiesWithoutMatching.Add(entity);
                    }
                    
                    if (!hasMatchingProfile)
                    {
                        #region Fetch Current Entity map from the map collection

                        EntityMap entityMap = (EntityMap)entityMapCollection.GetEntityMap(entity.Id);

                        if (entityMap != null)
                        {
                            if (entityMap.InternalId > 0)
                            {
                                entity.Id = entityMap.InternalId;
                                entity.EntityFamilyId = entityMap.EntityFamilyId;
                                entity.EntityGlobalFamilyId = entityMap.EntityGlobalFamilyId;

                                //Try to read certain info from the Map if not passed on
                                if (entity.ContainerId <= 0)
                                    entity.ContainerId = entityMap.ContainerId;

                                if (!categoryMappingEnabled)
                                {
                                    entity.CategoryId = entityMap.CategoryId;
                                }

                                // only when category path is NOT given, go in to this. If the category path is given and it is wrong, then we should error out.
                                if (entity.CategoryId <= 0 && String.IsNullOrWhiteSpace(entity.CategoryPath))
                                {
                                    entity.CategoryId = entityMap.CategoryId;

                                    //Category is not provided for root level items so need to set same category for that one too.
                                    if (entity.ParentEntityTypeId <= 0)
                                        entity.ParentEntityId = entityMap.CategoryId;

                                    //If CanReclassifyEntities is set as true then find the source category for the entity and populate into entity.
                                    if (entityProcessingOptions.CanReclassifyEntities)
                                    {
                                        //If CanReclassifyEntities as true then import engine won't consider source category path what ever in input file.
                                        //Because Import engine will identify the source entity category and populate the category details.

                                        //If CanReclassifyEntities is true then populate source category id to entity move context.
                                        entity.EntityMoveContext.FromCategoryId = entity.CategoryId;

                                        Category category = GetCategoryById(entity.CategoryId, entity.ContainerName);

                                        if (category != null)
                                        {
                                            //Import engine identified source entity category then populate the category details to source entity.
                                            entity.CategoryPath = category.Path;
                                            entity.CategoryName = category.Name;
                                            entity.CategoryLongName = category.LongName;
                                        }
                                        else
                                        {
                                            if (isTracingEnabled)
                                            {
                                                message =
                                                    String.Format(
                                                        "Unable to find Category path for entity ({0}) with category id ({1} and container ({2}))",
                                                entity.ExternalId, entity.CategoryId, entity.ContainerName);
                                                activity.LogError(message);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        // We found the entity mostly without using category. But the given category is different. This should be an error condition as 
                                        // the user needs to reclassify
                                        if (entity.CategoryId != entityMap.CategoryId)
                                        {
                                            Category categoryInDatabase = GetCategoryById(entityMap.CategoryId, entity.ContainerName);

                                            if (categoryInDatabase != null)
                                            {
                                                message = String.Format("Entity is present in the database with a different category: External id {0}, Container id {1}, given category path is {2}, category path in database is {3}", entity.ExternalId, entity.ContainerName, entity.CategoryPath, categoryInDatabase.Path);
                                            }
                                            else
                                            {
                                                message = String.Format("Entity is present in the database with a different category: External id {0}, Container id {1}, given category path is {2}, category id in database is {3}", entity.ExternalId, entity.ContainerName, entity.CategoryPath, entityMap.CategoryId);
                                            }
                                            entityOperationResult.AddOperationResult(String.Empty, message, OperationResultType.Error);
                                            entity.Action = ObjectAction.Unknown;
                                        }
                                    }
                                }

                                if (entity.EntityTypeId <= 0)
                                {
                                    entity.EntityTypeId = entityMap.EntityTypeId;
                                }

                                //If entity has valid target category path then action set as reclassify.
                                //If source category and target category as same then continue.
                                //When valid target category path is present and source category path is invalid in input file then entity import engine will identify and populate,
                                //But when entityProcessingOptions.CanReclassifyEntities set as false, import engine will not populate entitymovecontext.fromcategory id so will do update for that entity.
                                if (entity.EntityMoveContext.TargetCategoryId > 0 &&
                                    entity.EntityMoveContext.FromCategoryId > 0 &&
                                    entity.CategoryId != entity.EntityMoveContext.TargetCategoryId)
                                {
                                    entity.Action = ObjectAction.Reclassify;
                                }

                                // if the action is already coming from the source (delete, update or ignore) keep it. Only if it is unknown or read, set it
                                if ((!entityOperationResult.HasError) && (entity.Action == ObjectAction.Unknown || entity.Action == ObjectAction.Read || entity.Action == ObjectAction.Create))
                                {
                                    entity.Action = ObjectAction.Update;
                                }
                            }
                            else if (entityMap.InternalId == -100)
                            {
                                // if Internal id as -100 then more than one entity map found. So entity map could not identify proper entity map. So for avoiding wrong data insert into db action set as unknown.

                                message =
                                    string.Format(
                                        "More than one entity map found for entity: {0} with container id {1}. Action set to Unknown",
                                        entity.ExternalId, entityMap.ContainerId);

                                entity.Action = ObjectAction.Unknown;

                                entityOperationResult.AddOperationResult("", message, OperationResultType.Error);

                                if (isTracingEnabled)
                                {
                                    activity.LogWarning(message);
                                }
                            }
                            else
                            {
                                //The entity action will be set to unknown and entity satisfy the below criteria only, 
                                //when the entity related information is mentioned in complex or relationship sheet of excel file but missing in entities sheet.
                                //In that case, job should be completed with error with the respective error message shown in job result page.
                                if (entity.Action == ObjectAction.Unknown)
                                {
                                    if (!String.IsNullOrWhiteSpace(entity.ExtendedProperties))
                                    {
                                        //Pattern of value in extended Properties : SourceDataReadError#@@#sheetName
                                        //Eg: SourceDataReadError#@@#Relationships
                                        String[] extendedProperties =
                                            entity.ExtendedProperties.Split(new String[] { "#@@#" },
                                                StringSplitOptions.None);
                                        String entityReferenceId = String.Empty;

                                        if (extendedProperties.Count() > 1)
                                        {
                                            if (entity.ReferenceId > 0 &&
                                                !String.IsNullOrWhiteSpace(errorMessagePrefix))
                                            {
                                                entityReferenceId = String.Concat(entity.ReferenceId.ToString(),
                                                    ": ");
                                            }

                                            String errorType = extendedProperties[0];
                                            String sheetName = extendedProperties[1];

                                            if (errorType.Equals("SourceDataReadError"))
                                            {
                                                //message: Line#3: Error occurred in reading the entity information in relationships sheet. Unable to create a new entity as information for External Id: APC001 is missing in the Entities sheet.
                                                message =
                                                    String.Format(
                                                        localeMessageBL.Get(systemUILocale, "113870", false,
                                                            callerContext).Message, errorMessagePrefix,
                                                        entityReferenceId, sheetName, entity.ExternalId);
                                                entityOperationResult.AddOperationResult("113870", message,
                                                    new Collection<Object>
                                                        {
                                                        errorMessagePrefix,
                                                        entityReferenceId,
                                                        sheetName,
                                                        entity.ExternalId
                                                        }, OperationResultType.Error);
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    if (isTracingEnabled)
                                    {
                                        message =
                                            string.Format("No internal id for entity: {0}, Action set to Create",
                                                entity.ExternalId);
                                        activity.LogInformation(message);
                                    }

                                    entity.Action = ObjectAction.Create;
                                }
                            }

                            //If entity long name is not available in case of Entity Create/Rename then consider entity name as long name.
                            if ((entity.Action == ObjectAction.Create || entity.Action == ObjectAction.Rename) &&
                                String.IsNullOrWhiteSpace(entity.LongName))
                            {
                                entity.LongName = entity.Name;
                            }

                            if (isTracingEnabled)
                            {
                                message = string.Format("Entity map loaded for entity: {0}", entity.ExternalId);
                                activity.LogMessageWithData(message, entity.ToXml());
                            }
                        }
                        else
                        {
                            // how can we not find the item we just added???
                            if (isTracingEnabled)
                            {
                                message = string.Format("No entity map set for entity: {0}, Action set to Unknown",
                                    entity.ExternalId);
                                activity.LogInformation(message);
                            }

                            entity.Action = ObjectAction.Unknown;
                        }

                        #endregion

                        #region Set SourceId

                        if (_entityDataSourceTrackingConfig != null && _entityDataSourceTrackingConfig.IsEnabled)
                        {
                            if (importProfile.InputSpecifications.SourceId.HasValue)
                            {
                                new SourceBL().SetSourceToAllEntityDescendants(entity,
                                importProfile.InputSpecifications.SourceId.Value);
                            }
                        }

                        #endregion
                    }
                }

                #region Extension Relationship - Get from Source

                ExtensionRelationship extensionRelation = sourceData.GetParentExtensionRelationShip(entity, application, stagingModule);

                // did we get anything back?
                if (extensionRelation != null)
                {
                    // this is the parent information. Update the entity with these information.
                    entity.ParentExtensionEntityCategoryName = extensionRelation.CategoryName;
                    entity.ParentExtensionEntityCategoryLongName = extensionRelation.CategoryLongName;
                    entity.ParentExtensionEntityCategoryPath = extensionRelation.CategoryPath;
                    entity.ParentExtensionEntityCategoryId = extensionRelation.CategoryId;
                    entity.ParentExtensionEntityContainerName = extensionRelation.ContainerName;
                    entity.ParentExtensionEntityContainerLongName = extensionRelation.ContainerLongName;
                    entity.ParentExtensionEntityContainerId = extensionRelation.ContainerId;
                    entity.ParentExtensionEntityExternalId = extensionRelation.ExternalId;

                    if (entity.ExtensionRelationships.Contains(extensionRelation) == false)
                    {
                        entity.ExtensionRelationships.Add(extensionRelation);
                    }
                }

                #endregion

                #region Extension Relationships - Fill and Validate

                // Now do a fill and validate on entity relationships
                FillAndValidateExtensionRelationShip(entity, entityOperationResult);

                if (!entityOperationResult.HasError)
                {
                    if (importProfile.ProcessingSpecifications.ImportMode == ImportMode.ExtensionRelationshipLoad)
                    {
                        #region To get the hierarchy id of Container

                        //Scenario: Trying to import a new MDLed entity.
                        //Product master container is MDLed to Asia Product Master  and also Vendor Master.
                        //Product Hierarchy and Asia product master belongs to product hierarchy, whereas vendor master belongs to vendor hierarchy.
                        //If trying to create an MDLed entity through import in asia product master and entered category doesnot exist, 
                        //   it should take the category from its parent container(ie, product master).
                        //If trying to create an MDLed entity  through import in vendor hierarchy and category mentioned is wrong then,
                        //   it should not take the category from its parent container, instead throw error message as both are from different hiearchy.

                        ContainerContext containerContext = new ContainerContext();
                        containerContext.LoadAttributes = false;

                        List<Container> containerList = cachedDataModel.GetContainers();

                        Container parentContainer = null;
                        Container MDLedContainer = null;

                        //This count is to check whether the container trying to search is obtained or not.
                        //Once the count is 2, it means it identified the parent as well as MDLed container.
                        Int16 matchedContainer = 0;

                        foreach (Container container in containerList)
                        {
                            if (container.Id == entity.ParentExtensionEntityContainerId)
                            {
                                parentContainer = container;
                                matchedContainer++;
                            }
                            else if (container.Id == entity.ContainerId)
                            {
                                MDLedContainer = container;
                                matchedContainer++;
                            }

                            if (matchedContainer == 2)
                                break;
                        }

                        #endregion

                        if ((parentContainer != null && MDLedContainer != null && parentContainer.HierarchyId == MDLedContainer.HierarchyId)
                            && entity.CategoryId <= 0)
                        {
                            entity.CategoryId = entity.ParentExtensionEntityCategoryId;
                        }

                        // if the parent entity type is not provided and we haven't found the parent entity id, then the parent is the 
                        // category and use the category id as the parent.
                        if (entity.ParentEntityTypeId <= 0 && entity.ParentEntityId <= 0)
                        {
                            entity.ParentEntityId = entity.CategoryId;
                        }

                        foreach (ExtensionRelationship extensionRel in entity.ExtensionRelationships)
                        {
                            if (extensionRel.CategoryId < 0)
                                extensionRel.CategoryId = entity.ParentExtensionEntityCategoryId;
                        }
                    }
                }

                //Now everything possible is filled in, lets validate...
                ValidateEntity(entity, entityOperationResult);

                #endregion

                if (entityOperationResult.HasError)
                {
                    errorCollection.Add(entityOperationResult);
                    badEntities.Add(entity);

                    if (importProfile.ProcessingSpecifications.ImportMode == ImportMode.RelationshipLoad ||
                        importProfile.ProcessingSpecifications.ImportMode == ImportMode.RelationshipInitialLoad)
                    {
                        // If the import is a relationship load or relationship initial load, the 'from' entity is actually in the 'Entity' object. For proper error handling
                        // this needs to be added a a relationship operation result so that the staging data source is updated correctly.
                        String errormessage = String.Empty;
                        String relationshipExternalId = String.Empty;

                        if (entity.Relationships.Count > 0)
                        {
                            // Get all the error messages at the entity level
                            foreach (Error error in entityOperationResult.Errors)
                            {
                                errormessage = String.Concat(errormessage, error.ErrorMessage);
                            }

                            // if the from entity fails..all the relationships under that will fail
                            foreach (Relationship relationship in entity.Relationships)
                            {
                                relationshipExternalId = relationship.RelationshipExternalId;

                                RelationshipOperationResult rorForSourceEntity = new RelationshipOperationResult();
                                rorForSourceEntity.RelationshipExternalId = relationshipExternalId;
                                rorForSourceEntity.AddOperationResult(String.Empty, errormessage, OperationResultType.Error);
                                entityOperationResult.RelationshipOperationResultCollection.Add(rorForSourceEntity);
                            }
                        }

                    }

                    if (isTracingEnabled)
                    {
                        message = string.Format("Validation result for Entity({0}): {1}", entity.ExternalId, entityOperationResult.ToXml());
                        activity.LogInformation(message);
                    }

                    continue;
                }

                // if the entity action is set for create/update/delete, make sure the profile allows that.. Otherwise log the error and skip the entity from
                // further processing.
                if ((entity.Action == ObjectAction.Create && entityProcessingOptions.CanAddEntities == false) ||
                    (entity.Action == ObjectAction.Update && entityProcessingOptions.CanUpdateEntities == false) ||
                    (entity.Action == ObjectAction.Delete && entityProcessingOptions.CanDeleteEntities == false)
                    )
                {
                    string errorMessage = String.Format("Entity with external id {0} is marked for {1} but the profile does not allow {1}.", entity.ExternalId, entity.Action);
                    entityOperationResult.AddOperationResult(String.Empty, errorMessage, OperationResultType.Error);

                    errorCollection.Add(entityOperationResult);
                    badEntities.Add(entity);

                    if (isTracingEnabled)
                    {
                        activity.LogError(errorMessage);
                    }
                    continue;
                }
            }

            if (removeBadEntitiesFromProcessing)
            {
                // now remove all the error entities from the collection so we dont have to process the attributes for them
                foreach (Entity entity in badEntities)
                {
                    if (entity != null)
                    {
                        if (isTracingEnabled)
                        {
                            message = string.Format("Removing error entity ({0}) from processing", entity.ExternalId);
                            activity.LogError(message);
                        }

                        entityCollection.Remove(entity);
                    }
                }
            }

            // remove duplicates entities from the list...now that the fill and validate is done, we expect the 4 key value to be present (externalid, containerid, categoryid and entity type id)
            // do a group by having count more than 1
            var duplicateEntities = entityCollection.GroupBy(entity => new { entity.ExternalId, entity.ContainerId, entity.CategoryId, entity.EntityTypeId },
             (key, group) => new
             {
                 ExternalId = key.ExternalId,
                 ContainerId = key.ContainerId,
                 CategoryId = key.CategoryId,
                 EntityTypeId = key.EntityTypeId,
                 Count = group.Count()
             }).Where(group => group.Count > 1);

            // for each of the duplicate entities remove them from processing and log
            foreach (var duplicateitem in duplicateEntities)
            {
                // this will return multiple..
                List<Entity> internalentityCollection = entityCollection.GetEntitiesByExternalId(duplicateitem.ExternalId);
                // remove all the duplicates...
                foreach (Entity entity in internalentityCollection)
                {
                    EntityOperationResult entityOperationResult = new EntityOperationResult(entity.Id, entity.ReferenceId, entity.LongName, entity.ExternalId);
                    if (entity.ContainerId == duplicateitem.ContainerId && entity.CategoryId == duplicateitem.CategoryId && entity.EntityTypeId == duplicateitem.EntityTypeId)
                    {
                        String errorMessage = String.Format("Entity with external id {0} and container name {1} has duplicates in this batch and is removed from processing.", entity.ExternalId, entity.ContainerName);

                        entityOperationResult.AddOperationResult(String.Empty, errorMessage, OperationResultType.Error);
                        entityCollection.Remove(entity);

                        if (isTracingEnabled)
                        {
                            activity.LogError(errorMessage);
                        }
                    }
                    // if this error is not already added..
                    if (errorCollection.Contains(entity.ExternalId) == false)
                    {
                        errorCollection.Add(entityOperationResult);
                    }
                }
            }
            #endregion

            if (isTracingEnabled)
            {
                message = string.Format("Fill and validate entities completed for ({0}) entities.", entityCollection.Count);
                activity.LogInformation(message);
                activity.Stop();
            }

            return errorCollection;
        }
       

        /// <summary>
        /// fille the missing information from cached metadata
        /// </summary>
        /// <param name="entity"></param>
        private void FillEntity(Entity entity)
        {
            string message = string.Empty;

            Boolean isTracingEnabled = _traceSettings.IsBasicTracingEnabled;
            DiagnosticActivity activity = null;
            if (isTracingEnabled)
            {
                activity = new DiagnosticActivity();
                activity.Start();
            }

            ApplyExplicitMap(entity);

            if (String.IsNullOrWhiteSpace(entity.Name))
                entity.Name = entity.ExternalId;

            //1	FK_Catalog, FK_Taxonomy  => 	CatalogName
            //2	FK_CNodeParent, ParentName  =>  	ParentExternalID
            //3	FK_NodeType    =>  NodeTypeName
            // external id is stored in short name
            if (entity.OrganizationId <= 0)
            {
                Organization org = null;

                // do we have the organization name?
                if (String.IsNullOrEmpty(entity.OrganizationName))
                {
                    org = GetOrganizationById(0);
                }
                else
                {
                    org = GetOrganizationByName(entity.OrganizationName);
                }

                if (org != null)
                {
                    entity.OrganizationId = org.Id;
                    entity.OrganizationName = org.Name;
                    entity.OrganizationLongName = org.LongName;

                    if (isTracingEnabled)
                    {
                        message = "Entity: " + entity.ExternalId + ". Found Organization. Id = " + org.Id + " Name = " + org.Name;
                        activity.LogInformation(message);
                    }
                }
                else
                {
                    if (isTracingEnabled)
                    {
                        message = "Entity: " + entity.ExternalId + ". Could not find Organization: " + entity.OrganizationName;
                        activity.LogWarning(message);
                    }
                }
            }

            if (entity.ContainerId <= 0)
            {
                Container catalog = GetContainerByName(entity.ContainerName);
                if (catalog != null)
                {
                    entity.ContainerId = catalog.Id;

                    if (String.IsNullOrWhiteSpace(entity.ContainerLongName))
                        entity.ContainerLongName = catalog.LongName;

                    //item.FkTaxonomy = catalog.HierarchyId;
                    if (isTracingEnabled)
                    {
                        message = "Entity: " + entity.ExternalId + ". Found Container. Id = " + catalog.Id + " Name = " + catalog.Name;
                        activity.LogInformation(message);
                    }
                }
                else
                {
                    if (isTracingEnabled)
                    {
                        message = "Entity: " + entity.ExternalId + ". Could not find Container: " + entity.ContainerName;
                        activity.LogWarning(message);
                    }
                }
            }

            if (entity.CategoryId <= 0)
            {
                Category category = GetCategoryByPath(entity.CategoryPath, entity.ContainerName);

                if (category != null)
                {
                    entity.CategoryId = category.Id;
                    entity.CategoryName = category.Name;
                    entity.CategoryLongName = category.LongName;

                    if (isTracingEnabled)
                    {
                        message = "Entity: " + entity.ExternalId + ". Found Category. Id = " + category.Id + " Name = " + category.Name;
                        activity.LogInformation(message);
                    }
                }
                else
                {
                    if (isTracingEnabled)
                    {
                        message = "Entity: " + entity.ExternalId + ". Could not find Category: " + entity.CategoryPath;
                        activity.LogWarning(message);
                    }
                }
            }

            if (entity.EntityMoveContext != null && !String.IsNullOrWhiteSpace(entity.EntityMoveContext.TargetCategoryPath))
            {
                Category targetCategory = GetCategoryByPath(entity.EntityMoveContext.TargetCategoryPath, entity.ContainerName);
                if (targetCategory != null)
                {
                    entity.EntityMoveContext.TargetCategoryId = targetCategory.Id;
                    entity.EntityMoveContext.TargetCategoryName = targetCategory.Name;
                    entity.EntityMoveContext.FromCategoryId = entity.CategoryId;

                    if (isTracingEnabled)
                    {
                        message = "Entity: " + entity.ExternalId + ". Found target Category. Id = " + targetCategory.Id + " Name = " + targetCategory.Name; ;
                        activity.LogInformation(message);
                    }
                }
                else
                {
                    if (isTracingEnabled)
                    {
                        message = "Entity: " + entity.ExternalId + ". Could not find target category: " + entity.CategoryPath;
                        activity.LogWarning(message);
                    }
                }

            }

            EntityType entityType = GetEntityTypeByName(entity.EntityTypeName);

            if (entityType != null)
            {
                entity.EntityTypeId = entityType.Id;

                if (entityTypeList != null && entityTypeList.Count > 0)
                {
                    Int32 currentEntityLevel = entityTypeList.IndexOfValue(entityType);
                    if (currentEntityLevel > 0)
                    {
                        EntityType parentEntityType = entityTypeList.Values[currentEntityLevel - 1];
                        entity.ParentEntityTypeId = parentEntityType.Id;
                    }
                }

                if (String.IsNullOrWhiteSpace(entity.EntityTypeLongName))
                    entity.EntityTypeLongName = entityType.LongName;

                //if its category, that means BranchLevel: Node
                if (entityType.CatalogBranchLevel == 1)
                    entity.BranchLevel = ContainerBranchLevel.Node;
                else
                    entity.BranchLevel = ContainerBranchLevel.Component;

                if (isTracingEnabled)
                {
                    message = "Entity: " + entity.ExternalId + ". Found EntityType. Id = " + entityType.Id + " Name = " + entityType.Name;
                    activity.LogInformation(message);
                }
            }
            else
            {
                if (isTracingEnabled)
                {
                    message = "Entity: " + entity.ExternalId + ". Could not find EntityType: " + entity.EntityTypeName;
                    activity.LogWarning(message);
                }
            }

            if (entity.EntityTypeId < 1 && String.IsNullOrWhiteSpace(entity.EntityTypeName))
            {
                if (isTracingEnabled)
                {
                    message = "Entity: " + entity.ExternalId + ". Entity Type Id and Entity Type are not available.";
                    activity.LogWarning(message);
                }
            }

            if (entity.ParentEntityTypeId < 1)
            {
                entity.ParentEntityId = entity.CategoryId;
                entity.ParentEntityName = entity.CategoryName;
                entity.ParentEntityLongName = entity.CategoryLongName;

                if (isTracingEnabled)
                {
                    message = "Entity: " + entity.ExternalId + ". Entity is a top level entity so parent is same as category.";
                    activity.LogInformation(message);
                }
            }

            // for categories, get the id from the category cache..
            if (entity.BranchLevel == ContainerBranchLevel.Node)
            {
                String entityPath = entity.ExternalId;

                if (String.IsNullOrEmpty(entity.CategoryPath) == false)
                {
                    entityPath = String.Concat(entity.CategoryPath, categoryPathSeparator, entity.ExternalId);
                }

                Category category = GetCategoryByPath(entityPath, entity.ContainerName);
                if (category != null)
                {
                    // THIS IS VERY CRITICAL. Entity MAP will not be used for category attribute import.
                    entity.Id = category.Id;
                    entity.Action = ObjectAction.Update;

                    if (isTracingEnabled)
                    {
                        message = "Entity: " + entity.ExternalId + " found in local cache. Category Id = " + category.Id + " Name = " + category.Name;
                        activity.LogInformation(message);
                    }
                }
                else
                {
                    // Validate entity will error this item out.
                    if (isTracingEnabled)
                    {
                        message = "Entity: " + entity.ExternalId + " is a category and is not found in the local cache. The category path is : " + entityPath;
                        activity.LogWarning(message);
                    }
                }
            }

            if (isTracingEnabled)
            {
                message = string.Format("Fill entity complete for entity : {0}", entity.ExternalId);
                activity.LogMessageWithData(message, entity.ToXml());
                activity.Stop();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        private void ApplyExplicitMap(Entity entity)
        {
            //KeyNotes:
            //1. Mode = "Implicit" means no need to switch values. Use the one given by reader
            //2. Mode = "Explict" means overridde the values and use the one given in profile
            //3. Mode = "InputField" means reader will use the given field detail and populate value. This mode is handled by reader and Import Engine would never ever handle "InputField" mode.
            //4. Mode = "Custom" means some BR or pre-process would populate the value.

            #region ShortName Map

            //NOTES: ShortName does not have explicit map..It is either InputField or Custom.

            #endregion

            #region LongName Map

            //NOTES: LongName does not have explicit map..It is either InputField or Custom.

            #endregion

            #region Container Map

            // Explicit Map
            if (importProfile.MappingSpecifications.EntityMetadataMap.ContainerMap.Mode == MappingMode.Explicit)
            {
                entity.ContainerId = importProfile.MappingSpecifications.EntityMetadataMap.ContainerMap.ContainerId;
            }

            #endregion

            #region Entity Type Map

            //Explicit Map
            if (importProfile.MappingSpecifications.EntityMetadataMap.EntityTypeMap.Mode == MappingMode.Explicit)
            {
                entity.EntityTypeId = importProfile.MappingSpecifications.EntityMetadataMap.EntityTypeMap.EntityTypeId;
            }

            #endregion

            #region Source Category Map

            //Source Category Explicit Map
            if (importProfile.MappingSpecifications.EntityMetadataMap.SourceCategoryMap.Mode == MappingMode.Explicit)
            {
                entity.CategoryId = importProfile.MappingSpecifications.EntityMetadataMap.SourceCategoryMap.CategoryId;
            }

            #endregion

            #region Target Category Map

            //Target Category Explicit Map
            if (importProfile.MappingSpecifications.EntityMetadataMap.TargetCategoryMap.Mode == MappingMode.Explicit)
            {
                //TODO: We need place holder to story entity.Target category id...
                //entity.CategoryId = importProfile.MappingSpecifications.EntityMetadataMap.TargetCategoryMap.CategoryId;
            }

            #endregion

            #region Hierarchy Parent Entity Map

            //TODO:: Implement Hierarchy Entity Explicit Map 

            #endregion

            #region MDL Parent Entity Map

            //TODO:: Implement MDL Parent Entity Explicit Map 

            #endregion

        }

        /// <summary>
        /// Based on the SP, these were the validations. Need to check with Jimmy/sagesh and make sure nothing is missed.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        private bool ValidateEntity(Entity entity, EntityOperationResult entityOperationResult)
        {
            string message = string.Empty;

            Boolean isTracingEnabled = _traceSettings.IsBasicTracingEnabled;
            DiagnosticActivity activity = null;
            if (isTracingEnabled)
            {
                activity = new DiagnosticActivity();
                activity.Start();
            }

            int numberOfFailures = 0;
            String errorMessage = String.Empty;
            String entityReferenceId = String.Empty;

            if (entity.ReferenceId > 0 && !String.IsNullOrWhiteSpace(errorMessagePrefix))
            {
                entityReferenceId = String.Concat(entity.ReferenceId.ToString(), ": ");
            }

            try
            {
                //this is to decide if we need to validate ParentExternalId & CategoryPath or not
                Boolean isCategory = false;

                EntityType entityType = GetEntityTypeByName(entity.EntityTypeName);

                // if the entity type had errors, these validation will fail. So do a null check
                if (entityType == null)
                {
                    // just for the sake of continuing, assume it is not category..
                    isCategory = false;
                }
                else
                {
                    if (entityType.CatalogBranchLevel == 1)
                    {
                        isCategory = true;

                        if (isTracingEnabled)
                            activity.LogInformation(string.Format("Current entity ({0}) is a category", entity.Name));
                    }
                }

                if (!isCategory && entity.CategoryId <= 0 && String.IsNullOrEmpty(entity.CategoryPath))
                {
                    numberOfFailures++;

                    errorMessage = String.Format(localeMessageBL.Get(systemUILocale, "111678", false, callerContext).Message, errorMessagePrefix, entityReferenceId);
                    entityOperationResult.AddOperationResult("111678", errorMessage, new Collection<Object> { errorMessagePrefix, entityReferenceId }, OperationResultType.Error);

                    if (isTracingEnabled)
                    {
                        activity.LogWarning(errorMessage);
                    }
                }

                if (String.IsNullOrEmpty(entity.Name))
                {
                    numberOfFailures++;
                    errorMessage = String.Format(localeMessageBL.Get(systemUILocale, "111679", false, callerContext).Message, errorMessagePrefix, entityReferenceId); //Name is empty or not specified.
                    entityOperationResult.AddOperationResult("111679", errorMessage, new Collection<Object> { errorMessagePrefix, entityReferenceId }, OperationResultType.Error);

                    if (isTracingEnabled)
                    {
                        activity.LogWarning(errorMessage);
                    }
                }

                if (entity.EntityTypeId <= 0 && String.IsNullOrEmpty(entity.EntityTypeName))
                {
                    numberOfFailures++;

                    errorMessage = String.Format(localeMessageBL.Get(systemUILocale, "111680", false, callerContext).Message, errorMessagePrefix, entityReferenceId); //EntityTypeName is empty or not specified.
                    entityOperationResult.AddOperationResult("111680", errorMessage, new Collection<Object> { errorMessagePrefix, entityReferenceId }, OperationResultType.Error);

                    if (isTracingEnabled)
                    {
                        activity.LogWarning(errorMessage);
                    }
                }

                if (entity.ContainerId <= 0 && string.IsNullOrEmpty(entity.ContainerName))
                {
                    numberOfFailures++;

                    errorMessage = String.Format(localeMessageBL.Get(systemUILocale, "112022", false, callerContext).Message, errorMessagePrefix, entityReferenceId);//ContainerName is empty or not specified.;
                    entityOperationResult.AddOperationResult("112022", errorMessage, new Collection<Object> { errorMessagePrefix, entityReferenceId }, OperationResultType.Error);

                    if (isTracingEnabled)
                    {
                        activity.LogWarning(errorMessage);
                    }
                }

                if (string.IsNullOrEmpty(entity.ExternalId))
                {
                    numberOfFailures++;

                    errorMessage = String.Format(localeMessageBL.Get(systemUILocale, "111699", false, callerContext).Message, errorMessagePrefix, entityReferenceId); //ExternalId is empty or not specified.
                    entityOperationResult.AddOperationResult("111699", errorMessage, new Collection<Object> { errorMessagePrefix, entityReferenceId }, OperationResultType.Error);

                    if (isTracingEnabled)
                    {
                        activity.LogWarning(errorMessage);
                    }
                }

                //if (!isCategory && string.IsNullOrEmpty(entity.ParentExternalId))
                //{
                //    numberOfFailures++;

                //    errorMessage = String.Format("ParentExternalId is empty or not specified.");
                //    entityOperationResult.AddOperationResult(String.Empty, errorMessage, OperationResultType.Error);

                //    if (Constants.TRACING_ENABLED)
                //    MDMTraceHelper.EmitTraceEvent(TraceEventType.Warning, "Parent External Id is empty");
                //}

                if (entity.Locale == LocaleEnum.UnKnown)
                {
                    numberOfFailures++;

                    errorMessage = String.Format("114127", errorMessagePrefix, entityReferenceId); // {0}{1}Locale is invalid.
                    entityOperationResult.AddOperationResult("114127", errorMessage, new Collection<Object> { errorMessagePrefix, entityReferenceId }, OperationResultType.Error);

                    if (isTracingEnabled)
                    {
                        activity.LogWarning(errorMessage);
                    }
                }

                if (entity.OrganizationId <= 0)
                {
                    numberOfFailures++;

                    errorMessage = String.Format(localeMessageBL.Get(systemUILocale, "111947", false, callerContext).Message, errorMessagePrefix, entityReferenceId, entity.OrganizationName); //Organization '{0}' not found. Verify if Organization exists.
                    entityOperationResult.AddOperationResult("111947", errorMessage, new Collection<Object> { errorMessagePrefix, entityReferenceId, entity.OrganizationName }, OperationResultType.Error);

                    if (isTracingEnabled)
                    {
                        activity.LogWarning(errorMessage);
                    }
                }

                if (entity.ContainerId <= 0)
                {
                    numberOfFailures++;

                    errorMessage = String.Format(localeMessageBL.Get(systemUILocale, "112023", false, callerContext).Message, errorMessagePrefix, entityReferenceId, entity.ContainerName); //Container '{0}' not found. Verify if container exists.
                    entityOperationResult.AddOperationResult("112023", errorMessage, new Collection<Object> { errorMessagePrefix, entityReferenceId, entity.ContainerName }, OperationResultType.Error);

                    if (isTracingEnabled)
                    {
                        activity.LogWarning(errorMessage);
                    }
                }

                if (!isCategory && entity.CategoryId <= 0)
                {
                    numberOfFailures++;

                    errorMessage = String.Format(localeMessageBL.Get(systemUILocale, "111683", false, callerContext).Message, errorMessagePrefix, entityReferenceId, entity.CategoryPath, entity.CategoryName); //Category with path '{0}' and name '{1}' not found. Verify if category exists.
                    entityOperationResult.AddOperationResult("111683", errorMessage, new Collection<Object> { errorMessagePrefix, entityReferenceId, entity.CategoryPath, entity.CategoryName }, OperationResultType.Error);

                    if (isTracingEnabled)
                    {
                        activity.LogWarning(errorMessage);
                    }
                }

                if (entity.EntityTypeId <= 0)
                {
                    numberOfFailures++;

                    errorMessage = String.Format(localeMessageBL.Get(systemUILocale, "111684", false, callerContext).Message, errorMessagePrefix, entityReferenceId, entity.EntityTypeName);//Entity type '{0}' not found. Verify if entity type exists.
                    entityOperationResult.AddOperationResult("111684", errorMessage, new Collection<Object> { errorMessagePrefix, entityReferenceId, entity.EntityTypeName }, OperationResultType.Error);

                    if (isTracingEnabled)
                    {
                        activity.LogWarning(errorMessage);
                    }
                }

                // The parent entity validation is not applicable for relationship ONLY load and relationship initial load...
                if (importProfile.ProcessingSpecifications.ImportMode != ImportMode.RelationshipLoad &&
                    importProfile.ProcessingSpecifications.ImportMode != ImportMode.RelationshipInitialLoad)
                {
                    if (!isCategory && entity.Action == ObjectAction.Create && entity.ParentEntityId <= 0)
                    {
                        numberOfFailures++;

                        errorMessage = String.Format(localeMessageBL.Get(systemUILocale, "111685", false, callerContext).Message, errorMessagePrefix, entityReferenceId, entity.ParentExternalId); //ParentEntity with parent external id '{0}' not found. Verify if parent entity exists.
                        entityOperationResult.AddOperationResult("111685", errorMessage, new Collection<Object> { errorMessagePrefix, entityReferenceId, entity.ParentExternalId }, OperationResultType.Error);

                        if (isTracingEnabled)
                        {
                            activity.LogWarning(errorMessage);
                        }
                    }
                }
                // if this is category and if we do not have an id, error the entity out.
                if (isCategory && entity.Id <= 0)
                {
                    numberOfFailures++;

                    errorMessage = String.Format(localeMessageBL.Get(systemUILocale, "111686", false, callerContext).Message, errorMessagePrefix, entityReferenceId, entity.ExternalId, entity.CategoryPath);//Entity '{0}' is a category and is not found in the category cache for its category parent path {1}. Verify both exists.
                    entityOperationResult.AddOperationResult("111686", errorMessage, new Collection<Object> { errorMessagePrefix, entityReferenceId, entity.ExternalId, entity.CategoryPath }, OperationResultType.Error);

                    if (isTracingEnabled)
                    {
                        activity.LogWarning(errorMessage);
                    }
                }


                if (entity.EntityMoveContext != null && !String.IsNullOrWhiteSpace(entity.EntityMoveContext.TargetCategoryPath))
                {
                    if (entity.EntityMoveContext.TargetCategoryId <= 0 && entityProcessingOptions.CanReclassifyEntities)
                    {
                        numberOfFailures++;

                        errorMessage = String.Format(localeMessageBL.Get(systemUILocale, "112118", false, callerContext).Message, errorMessagePrefix, entityReferenceId, entity.EntityMoveContext.TargetCategoryPath, entity.ExternalId);   //TargetCategory path'{2}' is not valid for Entity '{2}]. Verify the target Category path for Reclassification.
                        entityOperationResult.AddOperationResult("112118", errorMessage, new Collection<Object> { errorMessagePrefix, entityReferenceId, entity.EntityMoveContext.TargetCategoryPath, entity.ExternalId }, OperationResultType.Error);

                        if (isTracingEnabled)
                        {
                            activity.LogWarning(errorMessage);
                        }
                    }
                    else if (entity.EntityMoveContext.TargetCategoryId == entity.CategoryId)
                    {
                        numberOfFailures++;

                        errorMessage = String.Format(localeMessageBL.Get(systemUILocale, "112119", false, callerContext).Message, errorMessagePrefix, entityReferenceId, entity.ExternalId, entity.CategoryPath, entity.EntityMoveContext.TargetCategoryPath);   //Entity's '{2}' category '{3}'  is same as of the target category '{4}'. Entity cannot be reclassified to the same category.
                        entityOperationResult.AddOperationResult("112119", errorMessage, new Collection<Object> { errorMessagePrefix, entityReferenceId, entity.ExternalId, entity.CategoryPath, entity.EntityMoveContext.TargetCategoryPath }, OperationResultType.Warning);

                        if (isTracingEnabled)
                        {
                            activity.LogWarning(errorMessage);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (isTracingEnabled)
                {
                    activity.LogError("Error during ValidateEntity for ExternalId: " + entity.ExternalId + ". Message: " + ex.ToString());
                }
            }
            finally
            {
                if (isTracingEnabled)
                {
                    message = string.Format("Validate entity complete for entity : {0}", entity.ExternalId);
                    activity.LogInformation(message);
                    activity.Stop();
                }
            }

            return numberOfFailures > 0 ? false : true;
        }

        /// <summary>
        /// When a batch of entities fails, creates an error collection for the batch with the same error message
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        private EntityOperationResultCollection GetEntityErrorCollection(EntityCollection entities, String message)
        {
            EntityOperationResultCollection errorCollection = new EntityOperationResultCollection();

            foreach (Entity item in entities)
            {
                EntityOperationResult error = new EntityOperationResult();
                error.ExternalId = item.ExternalId;
                error.ReferenceId = item.ReferenceId;
                error.Errors.Add(new Error("100", message));
                errorCollection.Add(error);
            }
            return errorCollection;
        }

        #endregion

        #region Attribute Object Mgt

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="attributeCollection"></param>
        private AttributeOperationResultCollection FillAndValidateAttributes(Entity entity, AttributeCollection attributeCollection, Boolean removeBadAttributesFromProcessing, int threadNumber = -1)
        {
            AttributeOperationResultCollection aorCollection = new AttributeOperationResultCollection();
            String entityReferenceId = String.Empty;
            AttributeCollection warningAttributes = new AttributeCollection();

            if (entity.ReferenceId > 0 && !String.IsNullOrWhiteSpace(errorMessagePrefix))
            {
                entityReferenceId = String.Concat(entity.ReferenceId.ToString(), ": ");
            }

            ApplyRepeatedSourceAttributeMappings(attributeCollection);

            ValidateMandatoryAttributes(attributeCollection, aorCollection, entityReferenceId);

            Int32 attrWithoutIdCount = -1;

            AttributeModelCollection attributesForContext = this.GetContextualAttributeModels(entity);

            Collection<String> nonLocalizedAttributeNames = new Collection<String>();
            AttributeCollection filteredAttributes = new AttributeCollection();

            foreach (Attribute attribute in attributeCollection)
            {
                AttributeModel attributeModel = attributesForContext.GetAttributeModel(attribute);

                if (attributeModel != null && attributeModel.IsLocalizable)
                {
                    filteredAttributes.Add(attribute);
                }
                else
                {
                    if (!nonLocalizedAttributeNames.Contains(attribute.Name))
                    {
                        nonLocalizedAttributeNames.Add(attribute.Name);
                    }
                }
            }

            foreach (String nonLocalizedAttributeName in nonLocalizedAttributeNames)
            {
                AttributeCollection nonLocalizedAttributes = GetAttributesByName(attributeCollection, nonLocalizedAttributeName);

                ValidateNonLocalizedAttributes(nonLocalizedAttributes, filteredAttributes, warningAttributes, aorCollection, entityReferenceId);
            }

            foreach (Attribute attribute in filteredAttributes)
            {
                //now try to find a model, and fill all properties from attribute model
                FillAttribute(entity, attribute, attributesForContext, aorCollection, entityReferenceId);

                if (attribute.Id < 1)
                {
                    attribute.Id = attrWithoutIdCount--;
                }

                // the staging primary key is stored in the id property of the attribute value.
                Int32 stagingAttributePk = -1;

                if (attribute.GetCurrentValuesInvariant() != null && attribute.GetCurrentValuesInvariant().Count > 0)
                {
                    stagingAttributePk = ValueTypeHelper.Int32TryParse(attribute.GetCurrentValuesInvariant().ElementAt(0).Id.ToString(), -1);
                }

                // if the staging value is not there..assign the attribute id.
                if (stagingAttributePk <= 0)
                {
                    stagingAttributePk = attribute.Id;
                }

                AttributeOperationResult attributeOperationResult = aorCollection.GetAttributeOperationResult(attribute.Id, attribute.Locale);
                if (attributeOperationResult == null)
                {
                    attributeOperationResult = new AttributeOperationResult(stagingAttributePk, attribute.Name, attribute.LongName, attribute.AttributeModelType, attribute.Locale);
                }

                OperationResultType attributeOperationResultType = IdentifyOperationResultType(attribute, "Attribute");

                if (attribute.IsLookup)
                {
                    SetLookupAttributeAsBlank(attribute, importProfile);
                }

                ILocaleMessageManager localeMessageBL = new LocaleMessageBL();

                EntityOperationsHelper.ValidateUnmappedAttributes(entity, attribute, null, attributeOperationResult, localeMessageBL, entityProcessingOptions, callerContext, OperationResultType.Warning);

                if (importProfile.ProcessingSpecifications.ImportMode == ImportMode.InitialLoad ||
                    importProfile.ProcessingSpecifications.ImportMode == ImportMode.RelationshipInitialLoad)
                {
                    AttributeModel attributeModel = GetAttributeModelFromCachedDataModel(entity, attribute);
                    EntityOperationsHelper.ValidateEntityAttributesForImport(entity, null, attribute, attributeModel, attributeOperationResult, entityProcessingOptions, callerContext, localeMessageBL);
                }

                if (attributeOperationResult.HasError)
                {
                    if (!aorCollection.Contains(attributeOperationResult))
                    {
                        aorCollection.Add(attributeOperationResult);
                    }
                }

                if (attributeOperationResult.HasWarnings)
                {
                    if (!aorCollection.Contains(attributeOperationResult))
                    {
                        aorCollection.Add(attributeOperationResult);
                    }

                    warningAttributes.Add(attribute);
                }
            }

            //If any attributes are added into warning collection always remove from process
            if (warningAttributes.Count > 0)
            {
                foreach (Attribute warningAttribute in warningAttributes)
                {
                    attributeCollection.Remove(warningAttribute);
                }
            }

            MergeLookupAttributes(entity, attributeCollection);

            return aorCollection;
        }

        /// <summary>
        /// Identity the operationResult type base on the JobProcessingOptions.AttributeValidationLevel flag
        /// </summary>
        /// <param name="attributeType">Field indicated relationshipAttribute or Attribute</param>
        /// <returns>OperationResultType</returns>
        private OperationResultType IdentifyOperationResultType(Attribute attribute, String attributeType)
        {
            OperationResultType profileLevelOperationResultType = OperationResultType.Warning;

            if (attributeType.ToLowerInvariant() == "RelationshipAttribute".ToLowerInvariant())
            {
                profileLevelOperationResultType = this.importProfile.ProcessingSpecifications.JobProcessingOptions.RelationshipAttributeValidationLevel;
            }
            else if (attributeType.ToLowerInvariant() == "Attribute".ToLowerInvariant())
            {
                profileLevelOperationResultType = this.importProfile.ProcessingSpecifications.JobProcessingOptions.AttributeValidationLevel;
            }

            return IdentifyOperationResultType(attribute, profileLevelOperationResultType, attributeType);
        }

        /// <summary>
        /// Identity the operationResult type base on the JobProcessingOptions.AttributeValidationLevel flag
        /// </summary>
        /// <param name="profileLevelOperationResultType">Field indicated the profileLevelOperationResultType</param>
        /// <returns>OperationResultType</returns>
        private OperationResultType IdentifyOperationResultType(Attribute attribute, OperationResultType profileLevelOperationResultType, String attributeType)
        {
            OperationResultType operationResultType = OperationResultType.Warning;

            if (profileLevelOperationResultType == OperationResultType.Error)
            {
                //Always profile level setting OVERRIDES all the attribute level settings.
                operationResultType = profileLevelOperationResultType;
            }
            else if (profileLevelOperationResultType == OperationResultType.Warning)
            {
                operationResultType = profileLevelOperationResultType;
            }

            //If profile level setting is warning then do further operations
            if (operationResultType != OperationResultType.Error)
            {
                if (attribute.Id == 0)
                {
                    return profileLevelOperationResultType;
                }
                else
                {
                    AttributeMapCollection attrMaps = this.importProfile.MappingSpecifications.AttributeMaps;

                    if (attrMaps != null)
                    {
                        var validAttrMaps = from attrMap in attrMaps where attrMap.AttributeTarget.Id == attribute.Id && attrMap.AttributeTarget.Locale == attribute.Locale select attrMap;

                        if (validAttrMaps != null && validAttrMaps.Count() != 0)
                        {
                            var failEntityOnError = from filtered in validAttrMaps select filtered.AttributeTarget.FailEntityOnError;

                            if (failEntityOnError != null && failEntityOnError.Count() > 0 && failEntityOnError.ToArray()[0])
                            {
                                operationResultType = OperationResultType.Error;
                            }
                        }
                    }
                }
            }

            return operationResultType;
        }

        /// <summary>
        /// Identity the operationResult type base on the JobProcessingOptions.RelationshipTypeValidationLevel flag
        /// </summary>
        /// <param name="relationship">Field indicated the relationship</param>
        /// <returns>OperationResultType</returns>
        private OperationResultType IdentifyOperationResultType(Relationship relationship)
        {
            OperationResultType operationResultType = OperationResultType.Warning;
            OperationResultType profileLevelOperationResultType = this.importProfile.ProcessingSpecifications.JobProcessingOptions.RelationshipTypeValidationLevel;

            if (profileLevelOperationResultType == OperationResultType.Error)
            {
                //Always profile level setting OVERRIDES all the attribute level settings.
                operationResultType = profileLevelOperationResultType;
            }
            else if (profileLevelOperationResultType == OperationResultType.Warning)
            {
                operationResultType = profileLevelOperationResultType;
            }

            //If profile level setting is warning then do further operations
            if (operationResultType != OperationResultType.Error)
            {
                if (relationship.RelationshipTypeId == 0)
                {
                    return profileLevelOperationResultType;
                }
                else
                {
                    RelationshipTypeMaps relTypeMaps = this.importProfile.MappingSpecifications.RelationshipTypeMaps;

                    if (relTypeMaps != null && relTypeMaps.Count() > 0)
                    {
                        var attrMapCollection = from relTypemap in relTypeMaps select relTypemap.AttributeMapCollection;

                        var failEntityOnError = from map in attrMapCollection.FirstOrDefault() select map.AttributeTarget.FailEntityOnError;

                        if (failEntityOnError != null && failEntityOnError.Count() > 0 && failEntityOnError.ToArray()[0])
                        {
                            operationResultType = OperationResultType.Error;
                        }
                    }
                }
            }

            return operationResultType;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="relationship"></param>
        /// <param name="removeBadAttributesFromProcessing"></param>
        /// <param name="entity"></param>
        private AttributeOperationResultCollection FillAndValidateAttributes(Relationship relationship, Boolean removeBadAttributesFromProcessing, Entity entity)
        {
            string message = string.Empty;

            Boolean isTracingEnabled = _traceSettings.IsBasicTracingEnabled;
            DiagnosticActivity activity = null;
            if (isTracingEnabled)
            {
                activity = new DiagnosticActivity();
                activity.Start();
            }

            AttributeOperationResultCollection errorAORCollection = new AttributeOperationResultCollection();

            ILocaleCollection containerLocales;

            if (relationship != null && relationship.RelationshipAttributes != null && relationship.RelationshipAttributes.Count > 0)
            {
                ApplyRepeatedSourceAttributeMappings(relationship.RelationshipAttributes);

                if (!_containerLocaleMap.TryGetValue(entity.ContainerId, out containerLocales))
                {
                    containerLocales = _knowledgeBaseService.GetLocalesByContainer(entity.ContainerId) ?? new LocaleCollection();

                    //Process comes from RunInParallel and hence there can be multiple threads trying to add to dictionary
                    lock (lockContainerLocaleMapObject)
                    {
                        if (!_containerLocaleMap.ContainsKey(entity.ContainerId))
                        {
                            _containerLocaleMap.Add(entity.ContainerId, containerLocales);
                        }
                    }
                }

                AttributeModelCollection contextualRelAttrModels = this.GetContextualAttributeModels(relationship);
                foreach (Attribute attribute in relationship.RelationshipAttributes)
                {
                    //we need to transform and switch attributes
                    //example: incoming value of Group1.Attr1 should go to CoreAttributes.ProductDescription
                    //TransformAttribute(relationship, attribute);

                    //now try to find a model, and fill all properties from attribute model
                    FillAttribute(relationship, attribute, contextualRelAttrModels);

                    // the staging primary key is stored in the id property of the attribute value.
                    Int32 stagingAttributePk = -1;

                    if (attribute.GetCurrentValuesInvariant() != null && attribute.GetCurrentValuesInvariant().Count > 0)
                    {
                        stagingAttributePk = ValueTypeHelper.Int32TryParse(attribute.GetCurrentValuesInvariant().ElementAt(0).Id.ToString(), -1);
                    }

                    AttributeOperationResult attributeOperationResult = new AttributeOperationResult(stagingAttributePk, attribute.Name, attribute.LongName, attribute.AttributeModelType, attribute.Locale);

                    Boolean localeFoundInContainerLocales = containerLocales.Contains(attribute.Locale);

                    OperationResultType attributeOperationResultType = IdentifyOperationResultType(attribute, "RelationshipAttribute");

                    if (!localeFoundInContainerLocales)
                    {
                        Collection<Object> messageParams = new Collection<Object>() {
                            String.IsNullOrWhiteSpace(attribute.LongName) ? attribute.Name : attribute.LongName, 
                            String.IsNullOrWhiteSpace(attribute.AttributeParentLongName) ? attribute.AttributeParentName : attribute.AttributeParentLongName, 
                            attribute.Locale.ToString(), 
                            String.IsNullOrWhiteSpace(entity.OrganizationLongName) ? entity.OrganizationName : entity.OrganizationLongName };

                        attributeOperationResult.AddOperationResult("114019", String.Format("Attribute with the name '{0}', parent name '{1}' and locale '{2}' is ignored as the attribute locale is not mapped to the entity organization '{3}'.", messageParams.ToArray()), messageParams, attributeOperationResultType);
                    }
                    else
                    {
                        ILocaleMessageManager localeMessageBL = new LocaleMessageBL();
                        EntityOperationsHelper.ValidateUnmappedAttributes(entity, attribute, relationship, attributeOperationResult, localeMessageBL, entityProcessingOptions, callerContext, OperationResultType.Warning);

                        if (importProfile.ProcessingSpecifications.ImportMode == ImportMode.InitialLoad ||
                            importProfile.ProcessingSpecifications.ImportMode == ImportMode.RelationshipInitialLoad)
                        {
                            AttributeModel attributeModel = GetAttributeModelFromCachedDataModel(entity, attribute);
                            EntityOperationsHelper.ValidateEntityAttributesForImport(entity, relationship, attribute, attributeModel, attributeOperationResult, entityProcessingOptions, callerContext, localeMessageBL);
                        }

                        if (attributeOperationResult.HasError)
                        {
                            errorAORCollection.Add(attributeOperationResult);
                        }

                        if (attributeOperationResult.HasWarnings)
                        {
                            errorAORCollection.Add(attributeOperationResult);
                        }
                    }
                }
            }
            MergeLookupAttributes(entity, relationship.RelationshipAttributes);

            if (isTracingEnabled)
            {
                activity.Stop();
            }

            return errorAORCollection;
        }

        /// <summary>
        /// Fill the missing information from cached metadata
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="attribute"></param>
        /// <param name="attributeModels"></param>
        /// <param name="attributeOperationResult"></param>
        /// <param name="entityReferenceId"></param>
        private void FillAttribute(Entity entity, Attribute attribute, AttributeModelCollection attributeModels, AttributeOperationResultCollection attributeOperationResult,
                                        String entityReferenceId)
        {
            Boolean isTracingEnabled = _traceSettings.IsBasicTracingEnabled;

            DiagnosticActivity activity = null;

            if (isTracingEnabled)
            {
                activity = new DiagnosticActivity();
                activity.Start();
            }

            try
            {
                //Try to apply explicit map to the attribute..
                ApplyExplicitMap(attribute);

                //Update locales in attribute in case it is missing. Priority goes - 1. Input file 2. Profile 3.SDL
                ApplyLocaleMap(attribute);

                // Now fill up the attribute id and parent id
                AttributeModel attributeModel = null;

                //In case of Initial load, we loop attributes and then find parent entity, instead of looping entity and its attributes.
                //Since in that case, we will not know the context before hand, we will compute attribute models for each attribute, and AttributeModelContext is prepared using entity.
                if (attributeModels != null)
                {
                    attributeModel = attributeModels.GetAttributeModel(attribute);
                }
                else
                {
                    attributeModel = GetAttributeModelFromCachedDataModel(entity, attribute);
                }

                if (attributeModel == null)
                {
                    if (isTracingEnabled)
                    {
                        activity.LogWarning("Could not find Attribute Model Attribute: Name=" + attribute.Name + ", Parent=" + attribute.AttributeParentName);
                    }

                    // the validate will take care of this..
                    return;
                }

                if (isTracingEnabled)
                    activity.LogInformation("Attribute Model found for Entity: " + entity.Name + " Attribute: " + attribute.Name + ". Attribute Id: " + attributeModel.Id);

                //this function can be called recursively for child attributes
                FillAttributeDetails(attribute, attributeModel, entity, attributeOperationResult, entityReferenceId);
            }
            catch (Exception ex)
            {
                if (isTracingEnabled)
                {
                    activity.LogError("FillAttribute for Entity: " + entity.ExternalId + " Attribute: " + attribute.Name + " failed with exception: " + ex.ToString());
                }
            }
            finally
            {
                if (isTracingEnabled) activity.Stop();
            }
        }

        /// <summary>
        /// Fill the missing information from cached metadata
        /// </summary>
        /// <param name="relationship"></param>
        /// <param name="attribute"></param>
        /// <param name="attributeModels"></param>
        /// <param name="attributeOperationResultCollection"></param>
        /// <param name="entityReferenceId"></param>
        private void FillAttribute(Relationship relationship, Attribute attribute, AttributeModelCollection attributeModels = null,
                                    AttributeOperationResultCollection attributeOperationResultCollection = null, String entityReferenceId = "")
        {
            string message = string.Empty;
            Boolean isTracingEnabled = _traceSettings.IsBasicTracingEnabled;

            DiagnosticActivity activity = null;

            if (isTracingEnabled)
            {
                activity = new DiagnosticActivity();
                activity.Start();
            }

            try
            {
                //Try to apply explicit map to the attribute..
                ApplyExplicitMap(attribute);

                //Update locales in attribute in case it is missing. Priority goes - 1. Input file 2. Profile 3.SDL
                ApplyLocaleMap(attribute);

                // Now fill up the attribute id and parent id
                AttributeModel attributeModel = null;

                //In case of Initial load, we loop attributes and then find parent entity, instead of looping entity and its attributes.
                //Since in that case, we will not know the context before hand, we will compute attribute models for each attribute, and AttributeModelContext is prepared using entity.
                if (attributeModels != null)
                {
                    attributeModel = attributeModels.GetAttributeModel(attribute);
                }
                else
                {
                    attributeModel = GetAttributeModelFromCachedDataModel(relationship, attribute);
                }


                if (attributeModel == null)
                {
                    if (isTracingEnabled)
                    {
                        activity.LogWarning("Could not find Attribute Model Attribute: Name=" + attribute.Name + ", Parent=" + attribute.AttributeParentName);
                    }

                    // the validate will take care of this..
                    return;
                }

                if (isTracingEnabled)
                    activity.LogInformation("Attribute Model found for Relationship: " + relationship.RelationshipExternalId + " Attribute: " + attribute.Name + ". Attribute Id: " + attributeModel.Id);

                //this function can be called recursively for child attributes
                FillAttributeDetails(attribute, attributeModel, entity: null, attributeOperationResultCollection: attributeOperationResultCollection,
                                        entityReferenceId: entityReferenceId);
            }
            catch (Exception ex)
            {
                if (isTracingEnabled)
                {
                    activity.LogError("FillAttribute for Relationship: " + relationship.RelationshipExternalId + " Attribute: " + attribute.Name + " failed with exception: " + ex.ToString());
                }
            }
            finally
            {
                if (isTracingEnabled) activity.Stop();
            }
        }

        /// <summary>
        /// this function can be called recursively for child attributes
        /// </summary>
        /// <param name="attribute"></param>
        /// <param name="attributeModel"></param>
        /// <param name="entity"></param>
        /// <param name="attributeOperationResultCollection"></param>
        /// <param name="entityReferenceId"></param>
        private void FillAttributeDetails(Attribute attribute, AttributeModel attributeModel, Entity entity,
                                            AttributeOperationResultCollection attributeOperationResultCollection, String entityReferenceId)
        {
            string message = string.Empty;
            Boolean isTracingEnabled = _traceSettings.IsBasicTracingEnabled;

            DiagnosticActivity activity = null;

            if (isTracingEnabled)
            {
                activity = new DiagnosticActivity();
                activity.Start();
            }

            attribute.Id = attributeModel.Id;
            attribute.AttributeParentId = attributeModel.AttributeParentId;
            attribute.AttributeParentName = attributeModel.AttributeParentName;
            attribute.AttributeParentLongName = attributeModel.AttributeParentLongName;

            //This need to be assigned because in excel import, the attribute shortname is not available and so long name is assigned to shortname.
            //In this scenario, correct shortname is not available in EntityProcess. 
            //Due to which Business Rule with preLoadAttributes having isChangeContextTrigger enabled does not get triggered as it filters on AttributeShortName.
            attribute.Name = attributeModel.Name;

            if (String.IsNullOrWhiteSpace(attribute.LongName))
                attribute.LongName = attributeModel.LongName;

            attribute.AttributeModelType = attributeModel.Context.AttributeModelType;
            attribute.AttributeType = attributeModel.AttributeType;
            // if the action is already coming from the source ( delete or update) keep it. Only if it is unknown or read, set it
            if (attribute.Action == ObjectAction.Unknown || attribute.Action == ObjectAction.Read)
                attribute.Action = ObjectAction.Update;
            attribute.InstanceRefId = -1;

            AttributeDataType dataType = AttributeDataType.String;
            Enum.TryParse<AttributeDataType>(attributeModel.AttributeDataTypeName, out dataType);
            attribute.AttributeDataType = dataType;

            attribute.IsCollection = attributeModel.IsCollection;
            attribute.IsComplex = attributeModel.IsComplex;
            attribute.IsLookup = attributeModel.IsLookup;
            attribute.IsLocalizable = attributeModel.IsLocalizable;
            attribute.Precision = attributeModel.Precision;

            // if the attribute is not localizable change the locale to the system data locale ( SDL )
            //We are doing it only for non lookup attributes as locale details are required in MergeLookupAttributes which happens after FillingAttributeDetails. 
            //For NonLocalizable Lookup Attributes changing attribute.Locale to SDL will be handled while merging the lookup attributes for an entity.
            if (!attributeModel.IsLookup && attributeModel.AttributeDataTypeName != AttributeDataType.Decimal.ToString()
                && attributeModel.AttributeDataTypeName != AttributeDataType.Date.ToString()
                && attributeModel.AttributeDataTypeName != AttributeDataType.DateTime.ToString()
                && !attributeModel.IsComplex
                && !attributeModel.IsLocalizable)
            {
                if (isTracingEnabled)
                    activity.LogInformation("Attribute " + attribute.Name + " is not localizable. So it will be stored in system data locale");

                SetLocaleAsSDLForNonLocalizedAttribute(new AttributeCollection() { attribute }, attributeModel);
            }

            IValueCollection values = attribute.GetCurrentValuesInvariant();
            if (attributeModel.AttributeDataTypeName == AttributeDataType.Decimal.ToString() && values != null && values.Count > 0)
            {
                foreach (Value val in values)
                {
                    Double value;
                    String attrVal = val.AttrVal.ToString();

                    if (attrVal.IndexOf("E", StringComparison.OrdinalIgnoreCase) >= 0 && Double.TryParse(attrVal, out value))
                    {
                        Decimal decValue = Convert.ToDecimal(value);
                        val.InvariantVal = decValue.ToString();
                        val.AttrVal = decValue.ToString();
                        val.NumericVal = decValue;
                    }
                }
            }

            //If the attribute is not collection and the locale is not specified at value level of attribute,
            //then attribute locale will fall back to value level locale.
            //This is needed when we do merge and compare of values in EntityBL.
            if (!attribute.IsCollection)
            {
                if (values != null && values.Count > 0)
                {
                    Value value = values.FirstOrDefault();
                    value.Locale = attribute.Locale;
                }
            }

            #region Handle Keywords

            //TODO: supporting only simple attributes, not supported for complex attribute for now
            if (importProfile.ProcessingSpecifications.KeywordProcessingOptions.EnableIgnoreKeyword)
            {
                ProcessIgnoreKeyword(attribute, importProfile.ProcessingSpecifications.KeywordProcessingOptions.IgnoreKeyword);
            }

            //If KeywordProcessingOptions.EnableBlankKeyword is enabled we do it only for non lookup attributes. As for lookup atributes later, there is validatioin on lookup refid.
            if (importProfile.ProcessingSpecifications.KeywordProcessingOptions.EnableBlankKeyword)
            {
                if (!attribute.IsLookup)
                {
                    ProcessBlankKeyword(attribute, importProfile.ProcessingSpecifications.KeywordProcessingOptions.BlankKeyword);
                }
            }

            if (importProfile.ProcessingSpecifications.KeywordProcessingOptions.EnableDeleteKeyword)
            {
                ProcessDeleteKeyword(attribute, importProfile.ProcessingSpecifications.KeywordProcessingOptions.DeleteKeyword);
            }

            if ((attribute.IsCollection)
                && !String.IsNullOrWhiteSpace(importProfile.ProcessingSpecifications.KeywordProcessingOptions.CollectionDataSeparator))
            {
                PopulateCollectionValues(attribute);
            }

            PopulateUomValues(attribute, attributeModel.DefaultUOM, attributeModel.UomType);

            #endregion Handle Keywords

            //Now we have UOM seprated from values, lets try to assign DefaultUOM if attribute has uom specified but not provided for an attribute

            // we need to fill the complex childs for delete also..Only ignore we should kignore..
            if (attribute.Action != ObjectAction.Ignore)
            {
                if (attribute.IsHierarchical)
                {
                    attribute.OverriddenValues.Clear();
                    PopulateHierarchialData(attribute, attributeModel);
                }
                else
                {
                    PopulateComplexData(attribute, attributeModel, attributeOperationResultCollection, entityReferenceId);
                    PopulateSequence(attribute);
                }

                PopulateLookupRefId(attribute, attributeModel, entity);

                FormatAttributeValuesForLocale(attribute, attributeModel);

                NormalizeAttributeValuesForType(attribute, attributeModel);
            }

            if (isTracingEnabled) activity.Stop();
        }

        private void SetLocaleAsSDLForNonLocalizedAttribute(AttributeCollection attributes, AttributeModel attributeModel)
        {
            if (attributes != null && attributes.Count > 0)
            {
                foreach (Attribute attribute in attributes)
                {
                    if (attributeModel.AttributeModels != null && attributeModel.AttributeModels.Count > 0)
                    {
                        AttributeModel childAttrModel =
                            attributeModel.AttributeModels.FirstOrDefault(
                                model => CompareAttributeNames(attribute, model));

                        //make the same check as for simple attributes. 
                        //Decimal and Date attributes should be proceed in provided locale because of possible incorrect formatting
                        if (childAttrModel == null ||
                            (String.Compare(childAttrModel.AttributeDataTypeName, AttributeDataType.Decimal.ToString(), StringComparison.InvariantCultureIgnoreCase) != 0
                             && String.Compare(childAttrModel.AttributeDataTypeName, AttributeDataType.Date.ToString(), StringComparison.InvariantCultureIgnoreCase) != 0
                             && String.Compare(childAttrModel.AttributeDataTypeName, AttributeDataType.DateTime.ToString(), StringComparison.InvariantCultureIgnoreCase) != 0))
                        {
                            attribute.Locale = systemDataLocale;
                        }
                    }
                    else
                    {
                        attribute.Locale = systemDataLocale;
                    }

                    //This should not be applied for hierarchial attributes.
                    if (!attribute.IsHierarchical)
                    {
                        SetLocaleAsSDLForNonLocalizedAttribute(attribute.Attributes, attributeModel);
                    }
                }
            }
        }

        /// <summary>
        /// Compare attribute and model
        /// </summary>
        /// <param name="attribute"></param>
        /// <param name="model"></param>
        /// <returns>Returns true if names equal</returns>
        private static bool CompareAttributeNames(Attribute attribute, AttributeModel model)
        {
            return (String.Compare(attribute.Name, model.Name, StringComparison.InvariantCultureIgnoreCase) == 0
                    ||
                    String.Compare(attribute.LongName, model.LongName,
                        StringComparison.InvariantCultureIgnoreCase) == 0)
                   &&
                   (String.Compare(attribute.AttributeParentName, model.AttributeParentName,
                       StringComparison.InvariantCultureIgnoreCase) == 0
                    ||
                    String.Compare(attribute.AttributeParentLongName, model.AttributeParentLongName,
                        StringComparison.InvariantCultureIgnoreCase) == 0);
        }

        private Boolean ApplyLocaleMap(Attribute attribute)
        {
            Boolean success = false;

            if (attribute.Locale == LocaleEnum.UnKnown)
            {
                LocaleEnum attributeLocale = LocaleEnum.UnKnown;
                attributeLocale = importProfile.MappingSpecifications.LocaleMap.Locale;

                if (attributeLocale == LocaleEnum.UnKnown)
                {
                    attributeLocale = systemDataLocale;
                }

                attribute.Locale = attributeLocale;
                success = true;
            }

            return success;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="attribute"></param>
        /// <returns></returns>
        private Boolean ApplyExplicitMap(Attribute attribute)
        {
            Boolean sucess = false;

            AttributeMap explictMap = GetExplicitAttributeMap(attribute);

            if (explictMap != null)
            {

                AssignTargetMap(explictMap, attribute);

                sucess = true;
            }

            return sucess;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="attributeMap"></param>
        /// <param name="attribute"></param>
        /// <returns></returns>
        private Boolean AssignTargetMap(AttributeMap attributeMap, Attribute attribute)
        {
            Boolean sucess = false;

            if (attributeMap != null && attribute != null)
            {
                if (attributeMap.AttributeTarget.Id > 0)
                    attribute.Id = attributeMap.AttributeTarget.Id;

                if (!String.IsNullOrWhiteSpace(attributeMap.AttributeTarget.Name))
                    attribute.Name = attributeMap.AttributeTarget.Name;

                if (!String.IsNullOrWhiteSpace(attributeMap.AttributeTarget.ParentName))
                    attribute.AttributeParentName = attributeMap.AttributeTarget.ParentName;

                if (attributeMap.AttributeTarget.Locale != LocaleEnum.UnKnown)
                    attribute.Locale = attributeMap.AttributeTarget.Locale;

                sucess = true;
            }

            return sucess;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="attribute"></param>
        private AttributeMap GetExplicitAttributeMap(Attribute attribute)
        {
            AttributeMap attributeMap = null;

            if (importProfile.MappingSpecifications.AttributeMaps.Count > 0)
            {
                foreach (AttributeMap map in this.importProfile.MappingSpecifications.AttributeMaps)
                {
                    if (!map.IsProcessed && map.AttributeSource.StagingAttributeInfo != null)
                    {
                        Attribute stagingAttribute = map.AttributeSource.StagingAttributeInfo;

                        if (stagingAttribute.Name.Equals(attribute.Name, StringComparison.InvariantCultureIgnoreCase)
                            && stagingAttribute.AttributeParentName.Equals(attribute.AttributeParentName, StringComparison.InvariantCultureIgnoreCase)
                            && stagingAttribute.Locale == attribute.Locale)
                        {
                            attributeMap = map;
                            break;
                        }
                    }
                }
            }

            return attributeMap;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="attribute"></param>
        private void PopulateCollectionValues(Attribute attribute)
        {
            String collectionSeparator = importProfile.ProcessingSpecifications.KeywordProcessingOptions.CollectionDataSeparator;

            ValueCollection newValueCollection = new ValueCollection();

            Boolean hadSplit = false;

            String[] collectionSeparatorArray = new[] { collectionSeparator };

            foreach (Value value in attribute.GetCurrentValuesInvariant())
            {
                String attrVal = value.GetStringValue();

                if (!String.IsNullOrWhiteSpace(attrVal))
                {
                    String[] attrValues = attrVal.Split(collectionSeparatorArray, StringSplitOptions.None);

                    if (attrValues.Length > 1)
                    {
                        foreach (String strVal in attrValues)
                        {
                            if (!String.IsNullOrWhiteSpace(strVal))
                            {
                                Value newVal = new Value();
                                newVal.AttrVal = strVal.Trim();
                                newVal.Uom = value.Uom;
                                newVal.UomId = value.UomId;
                                newVal.ValueRefId = value.ValueRefId;
                                newVal.Action = value.Action;
                                newVal.Locale = value.Locale;
                                if (value.SourceInfo != null)
                                {
                                    newVal.SourceInfo = (SourceInfo)value.SourceInfo.Clone();
                                }

                                newValueCollection.Add(newVal);
                                hadSplit = true;
                            }
                        }
                    }
                }
            }

            if (hadSplit)
                attribute.SetValue(newValueCollection);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="attribute"></param>
        /// <param name="defaultUom"></param>
        /// <param name="uomType"></param>
        private void PopulateUomValues(Attribute attribute, String defaultUom, String uomType)
        {
            String uomSeparator = importProfile.ProcessingSpecifications.KeywordProcessingOptions.UomDataSeparator;

            Boolean isUomSeparationEnabled = false;

            if (!String.IsNullOrWhiteSpace(uomSeparator))
                isUomSeparationEnabled = true;

            ValueCollection newValueCollection = new ValueCollection();

            Boolean hadUomSplit = false;

            StringComparison stringComparison = isUOMsCaseSensitive ? StringComparison.InvariantCulture : StringComparison.InvariantCultureIgnoreCase;

            foreach (Value value in attribute.GetCurrentValuesInvariant())
            {
                // set default value for the Uom if not provided in data
                if (String.IsNullOrWhiteSpace(value.Uom))
                {
                    value.Uom = defaultUom;
                }
                if (isUomSeparationEnabled)
                {
                    String attrVal = value.GetStringValue();

                    if (!String.IsNullOrWhiteSpace(attrVal))
                    {
                        int lastIndexOfSep = attrVal.LastIndexOf(uomSeparator);

                        if (lastIndexOfSep > -1)
                        {
                            String val = attrVal.Substring(0, lastIndexOfSep);
                            String uom = attrVal.Substring(lastIndexOfSep + uomSeparator.Length, attrVal.Length - (lastIndexOfSep + uomSeparator.Length)).Trim();

                            value.AttrVal = val.Trim();
                            value.Uom = (uom != String.Empty) ? uom : defaultUom;

                            newValueCollection.Add(value);
                            hadUomSplit = true;
                        }
                    }
                }

                IEnumerable<UOM> filteredUOM = (from uomObj in uoms
                                                where (String.Compare(uomObj.Name, value.Uom, stringComparison) == 0
                                                    && String.Compare(uomObj.UnitTypeShortName, uomType, stringComparison) == 0)
                                                select uomObj);

                if (filteredUOM != null && filteredUOM.Count() > 0)
                {
                    value.UomId = filteredUOM.FirstOrDefault().Id;
                }
            }

            if (hadUomSplit)
            {
                attribute.SetValue(newValueCollection);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="attribute"></param>
        private void PopulateSequence(Attribute attribute)
        {
            string message = string.Empty;
            Boolean isTracingEnabled = _traceSettings.IsBasicTracingEnabled;

            DiagnosticActivity activity = null;

            if (isTracingEnabled)
            {
                activity = new DiagnosticActivity();
                activity.Start();
            }

            if (attribute.IsCollection)
            {
                Int32 seq = 0;
                Int32 valueRefId = 0;
                //for complex, sequence is taken from instance records
                if (attribute.IsComplex)
                {
                    Int32 deleteOrIgnoreRecordsSeq = 10000;
                    Int32 deleteOrIgnoreRecordsValueRefId = -10000;

                    foreach (Attribute instanceAttr in attribute.Attributes)
                    {
                        //don't generate sequence for records which getting ignored or deleted
                        if (instanceAttr.Action != ObjectAction.Ignore && instanceAttr.Action != ObjectAction.Delete)
                        {
                            if (instanceAttr.GetCurrentValuesInvariant().Count == 0)
                            {
                                instanceAttr.GetCurrentValuesInvariant().Add(new Value());
                            }

                            Value complexInstanceVal = instanceAttr.GetCurrentValuesInvariant().ElementAt(0);

                            if (complexInstanceVal != null && complexInstanceVal.Action != ObjectAction.Ignore && complexInstanceVal.Action != ObjectAction.Delete)
                            {
                                //We need to make sure here that valueRefId and seq. are in sync for both instance attributes and complex parent's value collection.
                                complexInstanceVal.Sequence = seq;
                                complexInstanceVal.ValueRefId = valueRefId;

                                instanceAttr.Sequence = seq;
                                instanceAttr.InstanceRefId = valueRefId;

                                complexInstanceVal.Action = ObjectAction.Update;

                                seq++;
                                valueRefId--;
                            }
                        }
                        else
                        {
                            instanceAttr.InstanceRefId = deleteOrIgnoreRecordsValueRefId--;
                            instanceAttr.Sequence = deleteOrIgnoreRecordsSeq++;

                            var complexInstanceValues = instanceAttr.GetCurrentValuesInvariant();

                            if (complexInstanceValues != null && complexInstanceValues.Count > 0)
                            {
                                var complexInstanceVal = complexInstanceValues.ElementAt(0);

                                if (complexInstanceVal != null)
                                {
                                    complexInstanceVal.Sequence = instanceAttr.Sequence;
                                    complexInstanceVal.ValueRefId = instanceAttr.InstanceRefId;
                                }
                            }
                        }
                    }

                    seq = 0;
                    valueRefId = 0;

                    deleteOrIgnoreRecordsSeq = 10000;
                    deleteOrIgnoreRecordsValueRefId = -10000;

                    foreach (IValue value in attribute.GetCurrentValuesInvariant())
                    {
                        if (value.Action != ObjectAction.Ignore && value.Action != ObjectAction.Delete)
                        {
                            value.Sequence = seq++;
                            value.ValueRefId = valueRefId--;
                        }
                        else
                        {
                            value.ValueRefId = deleteOrIgnoreRecordsValueRefId--;
                            value.Sequence = deleteOrIgnoreRecordsSeq++;
                        }
                    }
                }
                else
                {
                    foreach (Value value in attribute.GetCurrentValuesInvariant())
                    {
                        //don't generate sequence for values which getting ignored or deleted
                        if (value.Action != ObjectAction.Ignore && value.Action != ObjectAction.Delete)
                        {
                            value.Sequence = seq++;
                            value.Action = ObjectAction.Update;
                        }
                    }
                }
            }
            else
            {
                if (attribute.IsComplex)
                {
                    if (attribute.Attributes.Count > 0)
                    {
                        //TODO :: AttributeCollection change : Populate correct Locale
                        Attribute instanceAttr = attribute.Attributes.FirstOrDefault();

                        if (instanceAttr != null && instanceAttr.Action != ObjectAction.Ignore && instanceAttr.Action != ObjectAction.Delete)
                        {
                            if (instanceAttr.GetCurrentValuesInvariant().Count == 0)
                            {
                                instanceAttr.GetCurrentValuesInvariant().Add(new Value());
                            }
                            Value complexInstanceVal = instanceAttr.GetCurrentValuesInvariant().ElementAt(0);

                            if (complexInstanceVal.Action != ObjectAction.Ignore && complexInstanceVal.Action != ObjectAction.Delete)
                            {
                                complexInstanceVal.Sequence = -1;
                                complexInstanceVal.Action = ObjectAction.Update;
                            }
                        }
                    }
                }
                else
                {
                    if (attribute.GetCurrentValuesInvariant().Count > 0)
                    {
                        Value value = attribute.GetCurrentValuesInvariant().ElementAt(0);
                        if (value.Action != ObjectAction.Ignore && value.Action != ObjectAction.Delete)
                        {
                            value.Sequence = -1;
                            value.Action = ObjectAction.Update;
                        }
                    }
                }
            }

            if (isTracingEnabled)
            {
                activity.Stop();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="attribute"></param>
        /// <param name="attributeModel"></param>
        private void PopulateLookupRefId(Attribute attribute, AttributeModel attributeModel, Entity entity = null)
        {
            string message = string.Empty;
            Boolean isTracingEnabled = _traceSettings.IsBasicTracingEnabled;

            DiagnosticActivity activity = null;
            if (isTracingEnabled)
            {
                activity = new DiagnosticActivity();
                activity.Start();
            }

            // For Intial Load, check the job processing option. If the populate look refid is set to false, no need to compute..the staging Database already has the WSID
            if (importProfile.ProcessingSpecifications.ImportMode == ImportMode.InitialLoad && importProfile.ProcessingSpecifications.JobProcessingOptions.PopulateLookupRefIdForInitialLoad == false)
                return;
            // Get the WSID for lookup attributes
            if (attributeModel.IsLookup)
            {
                Lookup lookup = cachedDataModel.GetLookupTable(attributeModel.Id, attributeModel.Locale);
                Lookup sdlLookup = null;

                if (attribute.Locale != systemDataLocale)
                    sdlLookup = cachedDataModel.GetLookupTable(attributeModel.Id, systemDataLocale);

                Boolean readExportFormat = false;
                if (!String.IsNullOrWhiteSpace(attributeModel.ExportMask) && lookup.Columns.Contains("ExportFormat"))
                {
                    readExportFormat = true;

                    if (isTracingEnabled)
                    {
                        message = string.Format("ExportMask is defined for the attribute '{0}'. Values will be read as ExportFormat.", attributeModel.LongName);
                        activity.LogInformation(message);
                    }
                }
                else
                {
                    if (isTracingEnabled)
                    {
                        message = string.Format("ExportMask is not defined for the attribute '{0}'. Values will be read as DisplayFormat.", attributeModel.LongName);
                        activity.LogInformation(message);
                    }
                }

                if (lookup != null)
                {
                    if (isTracingEnabled)
                    {
                        message = string.Format("Found lookup table for Attribute: " + attribute.Name, attributeModel.LongName);
                        activity.LogInformation(message);
                    }

                    foreach (Value value in attribute.GetCurrentValuesInvariant())
                    {
                        //don't find lookup id for values which getting ignored or deleted
                        if (value.Action != ObjectAction.Ignore && value.Action != ObjectAction.Delete)
                        {
                            //Find lookup ref Id if Blank Keyword Processing is disabled or 
                            //if Blank Keyword Processing is enabled and value does not match Blank Keyword
                            if (!importProfile.ProcessingSpecifications.KeywordProcessingOptions.EnableBlankKeyword
                                || (importProfile.ProcessingSpecifications.KeywordProcessingOptions.EnableBlankKeyword
                                && !value.InvariantVal.Equals(importProfile.ProcessingSpecifications.KeywordProcessingOptions.BlankKeyword)))
                            {
                                String valueString = value.GetStringValue();
                                if (!String.IsNullOrWhiteSpace(valueString))
                                {
                                    IRow lookupRow = null;

                                    //Try to get lookup row in CDL
                                    lookupRow = GetLookupRow(lookup, valueString, readExportFormat, entity);

                                    if (lookupRow == null && sdlLookup != null)
                                    {
                                        //Try to get lookup row in SDL
                                        lookupRow = GetLookupRow(sdlLookup, valueString, readExportFormat, entity);
                                    }

                                    if (lookupRow != null && lookupRow.GetValue(Lookup.IdColumnName) != null)
                                    {
                                        value.ValueRefId = ValueTypeHelper.Int32TryParse(lookupRow.GetValue(Lookup.IdColumnName).ToString(), value.ValueRefId);
                                        value.AttrVal = value.ValueRefId;
                                        value.InvariantVal = value.ValueRefId;

                                        Object displayValObj = lookupRow.GetValue(Lookup.DisplayFormatColumnName);

                                        if (displayValObj != null)
                                        {
                                            string displayVal = displayValObj.ToString();

                                            if (!String.IsNullOrWhiteSpace(displayVal))
                                                value.SetDisplayValue(displayVal);
                                        }

                                        Object exportValObj = lookupRow.GetValue(Lookup.ExportFormatColumnName);

                                        if (exportValObj != null)
                                        {
                                            string exportVal = exportValObj.ToString();

                                            if (!String.IsNullOrWhiteSpace(exportVal))
                                                value.SetExportValue(exportVal);
                                        }
                                    }
                                    else
                                    {
                                        if (isTracingEnabled)
                                        {
                                            activity.LogWarning("Could not find Id for value: " + valueString);
                                        }
                                    }
                                }
                            }
                            else
                            {
                                if (isTracingEnabled)
                                {
                                    activity.LogWarning("Value is blank");
                                }
                            }
                        }
                    }
                }
                else
                {
                    if (isTracingEnabled)
                    {
                        activity.LogWarning("Could not find lookup table for Attribute: " + attribute.Name);
                    }
                }
            }

            if (isTracingEnabled) activity.Stop();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lookup"></param>
        /// <param name="valueString"></param>
        /// <param name="readExportFormat"></param>
        /// <returns></returns>
        private IRow GetLookupRow(Lookup lookup, String valueString, Boolean readExportFormat, Entity entity)
        {
            IRow lookupRow = null;
            MDM.BusinessObjects.ApplicationContext applicationContext = new MDM.BusinessObjects.ApplicationContext();

            if (entity != null)
            {
                //applicationContext.CategoryPath = entity.CategoryPath;
                applicationContext.CategoryPath = entity.CategoryPath.Replace(categoryPathSeparator, "#@#").Replace(@"//", "#@#");
                applicationContext.ContainerId = entity.ContainerId;
                applicationContext.OrganizationId = entity.OrganizationId;
            }

            //Try to get lookup record based on display format..
            lookupRow = lookup.GetRecordByDisplayFormat(valueString, applicationContext);

            if (lookupRow == null && readExportFormat)
            {
                //Try to get lookup record based on export format..
                lookupRow = lookup.GetRecordByExportFormat(valueString, applicationContext);
            }

            return lookupRow;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="attribute"></param>
        /// <param name="attributeModel"></param>
        /// <param name="attributeOperationResultCollection"></param>
        /// <param name="entityReferenceId"></param>
        private void PopulateComplexData(Attribute attribute, AttributeModel attributeModel, AttributeOperationResultCollection attributeOperationResultCollection,
                                            String entityReferenceId)
        {
            if (attributeModel.IsComplex)
            {
                string message = string.Empty;
                Boolean isTracingEnabled = _traceSettings.IsBasicTracingEnabled;

                DiagnosticActivity activity = null;

                if (isTracingEnabled)
                {
                    activity = new DiagnosticActivity();
                    activity.Start();
                }

                // get the FK_Complexattribute for the complex

                Boolean isValuesAtComplexParentAvailable = true;

                if (attribute.GetCurrentValuesInvariant() != null && attribute.GetCurrentValuesInvariant().Count < 1)
                {
                    isValuesAtComplexParentAvailable = false;
                }

                //Populate attribute id for Instance Records
                if (attribute.Attributes != null && attribute.Attributes.Count > 0)
                {
                    foreach (Attribute instanceRecord in attribute.Attributes)
                    {
                        instanceRecord.Id = attributeModel.Id;
                        instanceRecord.AttributeParentId = attributeModel.AttributeParentId;
                        instanceRecord.AttributeParentLongName = attributeModel.AttributeParentLongName;
                        instanceRecord.AttributeModelType = attributeModel.AttributeModelType;

                        //Need to reset Sequence and InstanceRefId also. If user has given any value in Xml, it might be wrong.
                        //We we initialize it to -1. If attribute is collection, PopulateSequence() will take care of putting proper value.
                        instanceRecord.Sequence = -1;
                        instanceRecord.InstanceRefId = -1;

                        if (isValuesAtComplexParentAvailable == false)
                        {
                            //In case of RSXml import, complex parent is not having values populated. So we need to create <Value> for complex parent.
                            Value value = new Value();
                            value.Action = instanceRecord.Action;

                            //Add value at complex parent level.
                            attribute.AppendValueInvariant(value);
                        }

                        if (attributeModel.AttributeModels != null && attributeModel.AttributeModels.Count > 0)
                        {
                            foreach (AttributeModel childAttrModel in attributeModel.AttributeModels)
                            {
                                try
                                {
                                    Attribute childAttribute = null;

                                    foreach (Attribute attr in instanceRecord.Attributes)
                                    {
                                        //Try to apply explicit map for the attribute..

                                        ApplyExplicitMap(attr);

                                        if ((attr.Name.Equals(childAttrModel.Name, StringComparison.InvariantCultureIgnoreCase)
                                            || attr.LongName.Equals(childAttrModel.LongName, StringComparison.InvariantCultureIgnoreCase))
                                            && (attr.AttributeParentName.Equals(childAttrModel.AttributeParentName, StringComparison.InvariantCultureIgnoreCase)
                                                || attr.AttributeParentLongName.Equals(childAttrModel.AttributeParentLongName, StringComparison.InvariantCultureIgnoreCase))
                                            )
                                        {
                                            childAttribute = attr;
                                            break;
                                        }
                                    }

                                    if (childAttribute != null)
                                    {
                                        FillAttributeDetails(childAttribute, childAttrModel, entity: null, attributeOperationResultCollection: null, entityReferenceId: "");

                                        childAttribute.Locale = instanceRecord.Locale;
                                    }
                                    else
                                    {
                                        if (childAttrModel.Required)
                                        {
                                            if ((attribute.Action != ObjectAction.Delete && attribute.Action != ObjectAction.Read && attribute.Action != ObjectAction.Ignore)
                                                && (attributeOperationResultCollection != null && !String.IsNullOrWhiteSpace(entityReferenceId)))
                                            {
                                                //Message: Attribute '{0}/{1}' is a required complex child attribute and cannot be empty.
                                                //Sample: Attribute 'ComplexParentAttribute/ComplexChildReqAttr' is a required complex child attribute and cannot be empty.
                                                String errorMessage = String.Format(localeMessageBL.Get(systemUILocale, "114042", false, callerContext).Message, errorMessagePrefix, entityReferenceId);

                                                if (isTracingEnabled)
                                                {
                                                    activity.LogError(errorMessage);
                                                }

                                                OperationResultType operationResultType = IdentifyOperationResultType(attribute, "Attribute");

                                                AttributeOperationResult attributeOperationResult = new AttributeOperationResult(childAttrModel.AttributeParentId, childAttrModel.AttributeParentName,
                                                                                                                                    childAttrModel.AttributeParentLongName, childAttrModel.AttributeModelType, attribute.Locale);
                                                attributeOperationResult.AddOperationResult("114042", errorMessage, new Collection<Object> { errorMessagePrefix, entityReferenceId }, operationResultType);
                                                attributeOperationResultCollection.Add(attributeOperationResult);
                                            }
                                        }
                                        else
                                        {
                                            if (isTracingEnabled)
                                            {
                                                activity.LogWarning("No value found for child attribute: " + childAttrModel.Name);
                                            }
                                        }
                                    }
                                }
                                catch (InvalidOperationException)
                                {
                                    message = "More than one attributes found for Name: " + childAttrModel.Name + " LongName: " + childAttrModel.LongName;
                                    if (isTracingEnabled)
                                    {
                                        activity.LogError(message);
                                    }
                                    throw new InvalidOperationException(message);
                                }
                            }
                        }
                        else
                        {
                            if (isTracingEnabled)
                            {
                                activity.LogWarning("No child attributes models found for Attribute: " + attributeModel.Name);
                            }
                        }
                    }
                }
                else
                {
                    if (isTracingEnabled)
                    {
                        activity.LogWarning("No child attributes found for Attribute: " + attributeModel.Name);
                    }
                }

                if (isTracingEnabled) activity.Stop();
            }
        }

        private void PopulateHierarchialData(Attribute attribute, AttributeModel attributeModel)
        {
            if (!attributeModel.IsHierarchical)
            {
                return;
            }
            Boolean isTracingEnabled = _traceSettings.IsBasicTracingEnabled;

            DiagnosticActivity activity = null;
            if (isTracingEnabled)
            {
                activity = new DiagnosticActivity();
                activity.Start();
            }

            //Populate attribute id for Instance Records
            if (attribute.Attributes != null && attribute.Attributes.Any())
            {
                var sequence = 0;
                var instanceRefId = -1;
                foreach (Attribute instanceRecord in attribute.Attributes.ToList())
                {
                    try
                    {
                        instanceRecord.Id = attributeModel.Id;
                        instanceRecord.AttributeParentId = attributeModel.AttributeParentId;
                        instanceRecord.AttributeParentLongName = attributeModel.AttributeParentLongName;
                        instanceRecord.AttributeModelType = attributeModel.AttributeModelType;

                        //Need to reset Sequence and InstanceRefId also. If user has given any value in Xml, it might be wrong.
                        //We we initialize it to -1. If attribute is collection, PopulateSequence() will take care of putting proper value.
                        instanceRecord.Sequence = sequence;
                        instanceRecord.InstanceRefId = instanceRefId;

                        Boolean isValuesAtComplexParentAvailable = true;

                        if ((attribute.GetCurrentValuesInvariant() == null) || ((attribute.GetCurrentValuesInvariant() != null) && !attribute.GetCurrentValuesInvariant().Any()))
                        {
                            isValuesAtComplexParentAvailable = false;
                        }
                        else
                        {
                            isValuesAtComplexParentAvailable = attribute.GetCurrentValuesInvariant().Any(value => value.ValueRefId == instanceRecord.InstanceRefId);
                        }

                        if (!isValuesAtComplexParentAvailable)
                        {
                            //In case of RSXml import, complex parent is not having values populated. So we need to create <Value> for complex parent.
                            Value value = new Value();
                            value.Action = instanceRecord.Action;
                            value.ValueRefId = instanceRecord.InstanceRefId;

                            if (attribute.IsCollection)
                            {
                                value.Sequence = instanceRecord.Sequence;
                            }

                            //Add value at complex parent level.
                            attribute.AppendValueInvariant(value);
                        }

                        if (attributeModel.AttributeModels != null && attributeModel.AttributeModels.Count > 0)
                        {
                            foreach (Attribute childAttribute in instanceRecord.Attributes)
                            {
                                try
                                {
                                    AttributeModel childAttrModel = attributeModel.AttributeModels.FirstOrDefault(
                                        (model) =>
                                            (childAttribute.Name.Equals(model.Name, StringComparison.InvariantCultureIgnoreCase)
                                             || childAttribute.LongName.Equals(model.LongName, StringComparison.InvariantCultureIgnoreCase)
                                                )
                                            &&
                                            (childAttribute.AttributeParentName.Equals(model.AttributeParentName, StringComparison.InvariantCultureIgnoreCase)
                                             || childAttribute.AttributeParentLongName.Equals(model.AttributeParentLongName, StringComparison.InvariantCultureIgnoreCase))
                                        );

                                    ApplyExplicitMap(childAttribute);

                                    if (childAttrModel != null)
                                    {
                                        FillAttributeDetails(childAttribute, childAttrModel, null, null, String.Empty);
                                    }
                                    else
                                    {
                                        if (isTracingEnabled)
                                        {
                                            activity.LogWarning("No model found for child attribute: " + childAttribute.Name);
                                        }
                                    }
                                }
                                catch (InvalidOperationException ex)
                                {
                                    if (isTracingEnabled)
                                    {
                                        activity.LogWarning(ex.StackTrace);
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (isTracingEnabled)
                            {
                                activity.LogWarning("No child attributes models found for Attribute: " + attributeModel.Name);
                            }
                        }

                        //Increment the sequence and decrement the value referenceid
                        sequence++;
                        instanceRefId--;
                    }
                    catch (InvalidOperationException ex) //ValidateAttribute Method will process and log attribute data problems
                    {
                        if (isTracingEnabled)
                        {
                            activity.LogWarning(ex.Message);
                            activity.LogWarning(ex.StackTrace);
                        }
                    }
                }
            }
            else
            {
                if (isTracingEnabled)
                {
                    activity.LogWarning("No child attributes found for Attribute: " + attributeModel.Name);
                }
            }

            if (isTracingEnabled)
            {
                activity.Stop();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="attribute"></param>
        /// <param name="attributeModel"></param>
        private void FormatAttributeValuesForLocale(Attribute attribute, AttributeModel attributeModel)
        {
            //To format values, just create new value collection and call SetValue method. Set value method would do the job of formatting...

            String attributeDataTypeName = attributeModel.AttributeDataTypeName.ToUpperInvariant();

            if (attributeDataTypeName.Equals("DATE")
                || attributeDataTypeName.Equals("DATETIME")
                || attributeDataTypeName.Equals("DECIMAL"))
            {
                ValueCollection valueCollection = new ValueCollection();

                foreach (Value value in attribute.GetCurrentValuesInvariant())
                {
                    valueCollection.Add(value);
                }

                if (importProfile.ProcessingSpecifications.ImportMode == ImportMode.InitialLoad
                && attribute.SourceFlag == AttributeValueSource.Inherited)
                {
                    attribute.SetInheritedValue(valueCollection);
                }
                else
                {
                    attribute.SetValue(valueCollection);
                }

            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="attribute"></param>
        /// <param name="attributeModel"></param>
        private void NormalizeAttributeValuesForType(Attribute attribute, AttributeModel attributeModel)
        {
            if (String.Equals(attributeModel.AttributeDataTypeName, "DATE", StringComparison.InvariantCultureIgnoreCase))
            {
                foreach (Value value in attribute.GetCurrentValuesInvariant())
                {
                    if (value.InvariantVal == null)
                        continue;

                    CultureInfo cultureInfo = Constants.STORAGE_CULTUREINFO;

                    // As of 7.4, Date is always managed as MM/DD/YYYY for Storage Value inside MDMCenter, hence using en-US
                    String dateFormat = FormatHelper.GetShortDateFormat(cultureInfo);

                    DateTime dateValue;

                    if (DateTime.TryParse(value.InvariantVal.ToString(), cultureInfo, DateTimeStyles.None, out dateValue))
                    {
                        value.InvariantVal = dateValue.ToString(dateFormat, cultureInfo);
                    }
                }
            }

            if (String.Equals(attributeModel.AttributeDataTypeName, "BOOLEAN", StringComparison.InvariantCultureIgnoreCase))
            {
                foreach (Value value in attribute.GetCurrentValuesInvariant())
                {
                    if (value.InvariantVal == null)
                        continue;
                    Boolean invariantVal;
                    // If cell category in Excel worksheet is set to General then true/false text will be parsed as 1/0
                    if (TryParseExcelBoolean(value.InvariantVal.ToString(), out invariantVal))
                    {
                        value.InvariantVal = invariantVal;
                        value.AttrVal = invariantVal;
                    }
                }
            }

            if (String.Equals(attributeModel.AttributeDisplayTypeName, "DROPDOWN", StringComparison.InvariantCultureIgnoreCase))
            {
                String[] allowedValuesList = attributeModel.AllowableValues.Split(new[] { "||" }, StringSplitOptions.RemoveEmptyEntries);

                if (allowedValuesList.Length <= 0)
                    return;

                foreach (Value value in attribute.GetCurrentValuesInvariant())
                {
                    foreach (string allowedValue in allowedValuesList)
                    {
                        if (value.InvariantVal != null && String.Equals(value.InvariantVal.ToString(), allowedValue, StringComparison.InvariantCultureIgnoreCase))
                        {
                            value.InvariantVal = allowedValue;
                            value.AttrVal = allowedValue;
                            break;
                        }
                    }
                }
            }
        }

        private static Boolean TryParseExcelBoolean(String text, out Boolean result)
        {
            if (text.Equals("0"))
            {
                result = false;
                return true;
            }

            if (text.Equals("1"))
            {
                result = true;
                return true;
            }

            return Boolean.TryParse(text, out result);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="attribute"></param>
        /// <param name="keyword"></param>
        private void ProcessIgnoreKeyword(Attribute attribute, String keyword)
        {
            string message = string.Empty;
            Boolean isTracingEnabled = _traceSettings.IsBasicTracingEnabled;

            DiagnosticActivity activity = null;

            if (isTracingEnabled)
            {
                activity = new DiagnosticActivity();
                activity.Start();
            }

            IValueCollection overridenValues = attribute.GetOverriddenValuesInvariant();

            //TODO: handle complex
            if (overridenValues != null && overridenValues.Count > 0)
            {
                foreach (Value val in overridenValues)
                {
                    if (val.AttrVal != null && val.GetStringValue().Equals(keyword, StringComparison.InvariantCultureIgnoreCase)) //case in-sensitive keyword match
                    {
                        val.Action = ObjectAction.Ignore;
                        val.SetBlank(); // clear the keyword value as it is now processed..

                        if (isTracingEnabled)
                            activity.LogInformation(String.Format("Ignore keyword found for Attribute:{0}. Setting action as Ignore.", attribute.Name));
                    }
                }
            }

            if (isTracingEnabled)
            {
                activity.LogInformation(String.Format("ProcessIgnoreKeyword for Attribute:{0} Keyword:{1} completed", attribute.Name, keyword));
                activity.Stop();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="attribute"></param>
        /// <param name="keyword"></param>
        private void ProcessBlankKeyword(Attribute attribute, String keyword)
        {
            string message = string.Empty;
            Boolean isTracingEnabled = _traceSettings.IsBasicTracingEnabled;

            DiagnosticActivity activity = null;
            if (isTracingEnabled)
            {
                activity = new DiagnosticActivity();
                activity.Start();
            }

            IValueCollection overridenValues = attribute.GetOverriddenValuesInvariant();

            //TODO: handle complex
            if (overridenValues != null && overridenValues.Count > 0)
            {
                foreach (Value val in overridenValues)
                {
                    if (val.AttrVal != null && val.GetStringValue().Equals(keyword))//case sensitive keyword match
                    {
                        val.SetBlank();

                        if (isTracingEnabled)
                            activity.LogInformation(String.Format("Blank keyword found for Attribute:{0}. Setting value as blank.", attribute.Name));
                    }
                }
            }

            if (isTracingEnabled)
            {
                activity.LogInformation(String.Format("ProcessBlankKeyword for Attribute:{0} Keyword:{1} completed", attribute.Name, keyword));
                activity.Stop();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="attribute"></param>
        /// <param name="keyword"></param>
        private void ProcessDeleteKeyword(Attribute attribute, String keyword)
        {
            string message = string.Empty;
            Boolean isTracingEnabled = _traceSettings.IsBasicTracingEnabled;

            DiagnosticActivity activity = null;
            if (isTracingEnabled)
            {
                activity = new DiagnosticActivity();
                activity.Start();
            }

            IValueCollection overridenValues = attribute.GetOverriddenValuesInvariant();
            Boolean result = false;

            if (overridenValues != null && overridenValues.Count > 0)
            {
                foreach (Value val in overridenValues)
                {
                    if (val.AttrVal != null && val.GetStringValue().Equals(keyword, StringComparison.InvariantCultureIgnoreCase)) //case in-sensitive keyword match
                    {
                        result = true;
                        val.Action = ObjectAction.Delete;
                        val.SetBlank(); // clear the keyword value as it is now processed..

                        if (isTracingEnabled)
                            activity.LogInformation(String.Format("Delete keyword found for Attribute:{0}. Setting action as Delete.", attribute.Name));
                    }
                }

                if (result)
                    attribute.Action = ObjectAction.Delete;
            }

            if (isTracingEnabled)
            {
                activity.LogInformation(String.Format("ProcessDeleteKeyword for Attribute:{0} Keyword:{1} completed", attribute.Name, keyword));
                activity.Stop();
            }
        }

        /// <summary>
        /// Validate the Required/Mandatory attributes.
        /// If AttributeMap's attribute is required and which is not present in attribute collection, 
        /// Add it into operation result and bad attribute list.
        /// </summary>
        /// <param name="attributeCollection"></param>
        /// <param name="errorAORCollection"></param>
        private void ValidateMandatoryAttributes(AttributeCollection attributeCollection, AttributeOperationResultCollection errorAORCollection, String entityReferenceId)
        {
            AttributeMapCollection mapcollection = importProfile.MappingSpecifications.AttributeMaps;

            //Filter all mandatory attributes from source map collection
            var sourceMandatoryArrts = mapcollection.ToList().Where(map => map.AttributeSource.IsMandatory == true);

            //If required attribute is present in map,
            if (sourceMandatoryArrts.Count() > 0)
            {
                foreach (AttributeMap map in sourceMandatoryArrts.ToList())
                {
                    String inputFieldName = map.AttributeSource.Name;
                    Attribute sourceAttribute = sourceData.GetAttributeInfoFromInputFieldName(inputFieldName);

                    if (sourceAttribute != null)
                    {
                        var missingAttribute = attributeCollection.Where(attr => attr.Name.Equals(sourceAttribute.Name, StringComparison.InvariantCultureIgnoreCase));

                        //If required attribute is not present in attribute collection add it operation result and bad attribute list
                        if (missingAttribute != null && missingAttribute.Count() < 1)
                        {
                            AttributeOperationResult attrOperationResult = new AttributeOperationResult(sourceAttribute.Id,
                                                                            sourceAttribute.Name,
                                                                            sourceAttribute.LongName == String.Empty ? sourceAttribute.Name : sourceAttribute.LongName,
                                                                            sourceAttribute.AttributeModelType,
                                                                            sourceAttribute.Locale);

                            String attributeName = String.Format("{0}\\{1}", sourceAttribute.AttributeParentName, sourceAttribute.Name);

                            attrOperationResult.AddOperationResult("112021", String.Empty, new Collection<Object> { errorMessagePrefix, entityReferenceId, attributeName }, OperationResultType.Error); //{0} is a Required Field.
                            errorAORCollection.Add(attrOperationResult);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Merge lookup attributes
        /// </summary>
        /// <param name="entity">Indicates entity object.</param>
        /// <param name="attributes">Indicates attribute collection</param>
        private void MergeLookupAttributes(Entity entity, AttributeCollection attributes)
        {
            //if lookup attribute with sdl is there pick up that up, remove the rest.
            //if lookup attribute with sdl is not there, then pick the first non sdl attribute coming in.
            //In case of collection, irrespective values same logic as above works..
            //We are not going at the value level to decide which Attribute to consider and which not.

            AttributeCollection duplicateLookupAttributes = new AttributeCollection();
            Collection<Int32> nonLocalizableLookupAttrIdList = new Collection<Int32>();

            //Non Localizable Lookup Attribute not found in SDL, pick up one in non SDL locale and change the locale to SDL.
            foreach (Attribute attribute in attributes)
            {
                if ((attribute.IsLookup || attribute.AttributeDataType == AttributeDataType.Decimal || attribute.AttributeDataType == AttributeDataType.DateTime
                    || attribute.AttributeDataType == AttributeDataType.Date || attribute.IsComplex)
                    && !attribute.IsLocalizable
                    && attribute.Locale == systemDataLocale
                    && !nonLocalizableLookupAttrIdList.Contains(attribute.Id))
                {
                    nonLocalizableLookupAttrIdList.Add(attribute.Id);
                }
            }

            //Non Localizable Lookup Attribute not found in SDL, pick up one in non SDL locale and change the locale to SDL.
            foreach (Attribute attribute in attributes)
            {

                if ((attribute.IsLookup || attribute.AttributeDataType == AttributeDataType.Decimal || attribute.AttributeDataType == AttributeDataType.DateTime
                    || attribute.AttributeDataType == AttributeDataType.Date || attribute.IsComplex)
                    && !attribute.IsLocalizable
                    && attribute.Locale != systemDataLocale)
                {
                    if (nonLocalizableLookupAttrIdList.Contains(attribute.Id))
                    {
                        duplicateLookupAttributes.Add(attribute, true);
                    }
                    else
                    {
                        nonLocalizableLookupAttrIdList.Add(attribute.Id);
                        attribute.Locale = systemDataLocale;

                        //Set locale as SDL for child attributes as well, if it is a complex attribute
                        if (attribute.IsComplex)
                        {
                            if (attribute.Attributes != null && attribute.Attributes.Count > 0)
                            {
                                foreach (Attribute instanceRecord in attribute.Attributes)
                                {
                                    if (instanceRecord.Attributes != null || instanceRecord.Attributes.Count > 0)
                                    {
                                        foreach (Attribute childAttr in instanceRecord.Attributes)
                                        {
                                            childAttr.Locale = systemDataLocale;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            //remove duplicate attributes
            foreach (Attribute duplicateAttribute in duplicateLookupAttributes)
            {
                attributes.Remove(duplicateAttribute);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="attribute"></param>
        /// <param name="importProfile"></param>
        private void SetLookupAttributeAsBlank(Attribute attribute, ImportProfile importProfile)
        {
            foreach (Value val in attribute.GetCurrentValuesInvariant())
            {
                //Id Processing BlankKeyword is enabled then do not validate the value ref id and Value is equal to Blank Keyword, Set the Value to blank.
                if (importProfile.ProcessingSpecifications.KeywordProcessingOptions.EnableBlankKeyword
                    && val.InvariantVal.Equals(importProfile.ProcessingSpecifications.KeywordProcessingOptions.BlankKeyword))
                {
                    val.SetBlank();
                }
            }
        }

        private AttributeCollection GetAttributesByName(AttributeCollection attributes, String attributeName)
        {
            AttributeCollection attributesToReturn = new AttributeCollection();

            if (!String.IsNullOrWhiteSpace(attributeName))
            {
                foreach (Attribute attribute in attributes)
                {
                    if (String.Compare(attribute.Name, attributeName, true) == 0)
                    {
                        attributesToReturn.Add(attribute, true);
                    }
                }
            }

            return attributesToReturn;
        }

        private void ValidateNonLocalizedAttributes(AttributeCollection nonLocalizedAttributes, AttributeCollection filteredAttributes, AttributeCollection warningAttributes, AttributeOperationResultCollection aorCollection, String entityReferenceId)
        {
            if (nonLocalizedAttributes != null && nonLocalizedAttributes.Count > 0)
            {
                if (nonLocalizedAttributes.Count == 1)
                {
                    filteredAttributes.Add(nonLocalizedAttributes.FirstOrDefault());
                }
                else if (nonLocalizedAttributes.Count > 1)
                {
                    foreach (Attribute nonLocalizedAttribute in nonLocalizedAttributes)
                    {
                        if (nonLocalizedAttribute.Locale == systemDataLocale)
                        {
                            filteredAttributes.Add(nonLocalizedAttribute);
                        }
                        else
                        {
                            String errorMessage = String.Format("Entity on '{0}/{1}' has a non-localized attribute '{2}', hence the values in '{3}' locale are ignored. Only values in system data locale: '{4}' are imported.",
                                                                errorMessagePrefix, entityReferenceId, nonLocalizedAttribute.Name, nonLocalizedAttribute.Locale, systemDataLocale);
                            AttributeOperationResult attributeOperationResult = new AttributeOperationResult(nonLocalizedAttribute.Id, nonLocalizedAttribute.Name,
                                                                                                             nonLocalizedAttribute.LongName, nonLocalizedAttribute.AttributeModelType,
                                                                                                             nonLocalizedAttribute.Locale);
                            attributeOperationResult.AddOperationResult("114552", errorMessage, new Collection<Object> { errorMessagePrefix, entityReferenceId, nonLocalizedAttribute.Name, nonLocalizedAttribute.Locale, systemDataLocale }, OperationResultType.Warning);
                            aorCollection.Add(attributeOperationResult);

                            warningAttributes.Add(nonLocalizedAttribute);
                        }
                    }
                }
            }
        }

        #endregion

        #region Relationship Object Mgt

        /// <summary>
        /// 
        /// </summary>
        /// <param name="relationships"></param>
        /// <param name="fromEntityId"></param>
        /// <param name="fromEntityExternalId"></param>
        /// <param name="fromEntityContainerId"></param>
        /// <param name="removeBadRelationshipsFromProcessing"></param>
        /// <param name="removeBadRelationshipAttributesFromProcessing"></param>
        /// <returns></returns>
        private RelationshipOperationResultCollection FillandValidateRelationships(RelationshipCollection relationships, Int64 fromEntityId, String fromEntityExternalId, Int32 fromEntityContainerId, Int64 referenceId, Boolean removeBadRelationshipsFromProcessing, Boolean removeBadRelationshipAttributesFromProcessing, Entity entity, ref Dictionary<Int64, Dictionary<RelationshipCollection, RelationshipOperationResultCollection>> relationshipsWaitingForToEntityCreation, Int16 relationshipLevel = 1)
        {
            RelationshipOperationResultCollection errorRORCollection = new RelationshipOperationResultCollection();

            RelationshipCollection badRelationships = new RelationshipCollection();
            RelationshipCollection warningRelationships = new RelationshipCollection();
            Int32 tobeCreatedRelSeq = -1;
            String message = string.Empty;

            Boolean isTracingEnabled = _traceSettings.IsBasicTracingEnabled;

            DiagnosticActivity activity = null;
            if (isTracingEnabled)
            {
                activity = new DiagnosticActivity();
                activity.Start();
            }

            if (relationships != null && relationships.Count > 0)
            {
                RelationshipCollection relationshipsWithWarnings = new RelationshipCollection();
                RelationshipOperationResultCollection relationshipORWithWarnings = new RelationshipOperationResultCollection();

                foreach (Relationship relationship in relationships)
                {
                    CleanseRelationship(relationship);

                    RelationshipOperationResult relationshipOperationResult = new RelationshipOperationResult(relationship.Id, relationship.RelationshipExternalId, relationship.ToExternalId, relationship.RelationshipTypeName, relationship.ReferenceId);

                    FillRelationship(relationship, fromEntityId, fromEntityExternalId, fromEntityContainerId);

                    if (relationship.Id < 1)
                    {
                        relationship.Id = tobeCreatedRelSeq--;
                    }

                    //Set level...
                    relationship.Level = relationshipLevel;

                    ValidateRelationship(relationship, relationshipOperationResult, IdentifyOperationResultType(relationship));

                    if (relationshipOperationResult.OperationResultStatus != OperationResultStatusEnum.Failed)
                    {
                        if (relationship.RelationshipAttributes != null && relationship.RelationshipAttributes.Count > 0)
                        {
                            //clean up the relationship attributes
                            CleanseAttributes(relationship.RelationshipAttributes);

                            AttributeOperationResultCollection errorAORCollection = FillAndValidateAttributes(relationship, removeBadRelationshipAttributesFromProcessing, entity);

                            if (errorAORCollection.Count > 0)
                            {
                                relationshipOperationResult.SetAttributeOperationResult(errorAORCollection);

                                //For relationship load alone partial password is allowed..for other fail the relationship.
                                if (removeBadRelationshipAttributesFromProcessing == false || errorAORCollection.OperationResultStatus == OperationResultStatusEnum.CompletedWithWarnings)
                                {
                                    relationshipOperationResult.AttributeOperationResultCollection.OperationResultStatus = errorAORCollection.OperationResultStatus;
                                    relationshipOperationResult.OperationResultStatus = errorAORCollection.OperationResultStatus;
                                }
                            }
                        }
                    }

                    if (relationshipOperationResult.OperationResultStatus == OperationResultStatusEnum.Failed)
                    {
                        badRelationships.Add(relationship);
                    }
                    else
                    {
                        //Fill and validate child relationships...
                        if (relationship.RelationshipCollection != null && relationship.RelationshipCollection.Count > 0)
                        {
                            RelationshipOperationResultCollection childRORCollection = FillandValidateRelationships(relationship.RelationshipCollection, relationship.RelatedEntityId, relationship.RelationshipExternalId, relationship.ToContainerId, referenceId, true, removeBadRelationshipAttributesFromProcessing, entity, ref relationshipsWaitingForToEntityCreation, (Int16)(relationshipLevel + 1));

                            if (childRORCollection != null && childRORCollection.Count > 0)
                            {
                                relationshipOperationResult.SetRelationshipOperationResults(childRORCollection);
                            }
                        }
                    }

                    if (relationshipOperationResult.OperationResultStatus == OperationResultStatusEnum.CompletedWithWarnings)
                    {
                        warningRelationships.Add(relationship);
                        Boolean appendWarningsToOR = true;

                        if (relationshipOperationResult.HasWarnings)
                        {
                            foreach (Warning warning in relationshipOperationResult.Warnings)
                            {
                                if (!String.IsNullOrWhiteSpace(warning.WarningCode) && warning.WarningCode.Equals("111695"))
                                {
                                    //NOTE: Keep relationship for retry as same batch may have related entity also to be created.
                                    relationshipsWithWarnings.Add(relationship);

                                    //NOTE: Later OperationResults will get attached with entityOR if again validation will get failed. Retry once)
                                    relationshipORWithWarnings.Add(relationshipOperationResult);

                                    appendWarningsToOR = false;

                                    break;
                                }
                            }
                        }

                        if (appendWarningsToOR)
                        {
                            errorRORCollection.Add(relationshipOperationResult);
                        }
                    }

                    if (relationshipOperationResult.OperationResultStatus == OperationResultStatusEnum.Failed ||
                        relationshipOperationResult.OperationResultStatus == OperationResultStatusEnum.CompletedWithErrors)
                    {
                        errorRORCollection.Add(relationshipOperationResult);
                    }
                }

                #region Validate duplicate relationship

                // remove duplicates relationships from the list...now that the fill and validate is done, we expect the 5 key value to be present (fromentityid, relatedentityid, containerid, relationshiptypeid and level)
                // do a group by having count more than 1
                var duplicateRelationships = relationships.GroupBy(relationship => new { relationship.FromEntityId, relationship.RelatedEntityId, relationship.ContainerId, relationship.RelationshipTypeId, relationship.Level },
                 (key, group) => new
                 {
                     FromEntityId = key.FromEntityId,
                     RelatedEntityId = key.RelatedEntityId,
                     ContainerId = key.ContainerId,
                     RelationshipTypeId = key.RelationshipTypeId,
                     Level = key.Level,
                     Count = group.Count()
                 }).Where(group => group.Count > 1);

                // for each of the duplicate relationships remove them from processing and log
                foreach (var duplicateitem in duplicateRelationships)
                {
                    //Add duplicate relationship error only if related entity found into system...
                    if (duplicateitem.RelatedEntityId > 0)
                    {
                        // this will return multiple..
                        RelationshipCollection internalRelationshipCollection = relationships.GetRelationships(duplicateitem.FromEntityId, duplicateitem.RelatedEntityId, duplicateitem.ContainerId, duplicateitem.RelationshipTypeId);
                        // remove all the duplicates...
                        foreach (Relationship relationship in internalRelationshipCollection)
                        {
                            RelationshipOperationResult relationshipOperationResult = new RelationshipOperationResult(relationship.Id, relationship.RelationshipExternalId, relationship.ToExternalId, relationship.RelationshipTypeName, relationship.ReferenceId);
                            if (relationship.FromEntityId == duplicateitem.FromEntityId && relationship.RelatedEntityId == duplicateitem.RelatedEntityId && relationship.ContainerId == duplicateitem.ContainerId &&
                                relationship.RelationshipTypeId == duplicateitem.RelationshipTypeId && relationship.Level == duplicateitem.Level)
                            {
                                String errorMessage = String.Format(localeMessageBL.Get(systemUILocale, "113867", false, callerContext).Message, fromEntityExternalId, entity.EntityTypeName, entity.ContainerName,
                                    relationship.ToExternalId, relationship.ToEntityTypeName, relationship.ToContainerName); //Duplicate relationship(s) found in the batch; From: External Id {0}, Entity Type {1}, Container {2} And To: External Id {3}, Entity Type {4}, Container {5} and is removed from processing.

                                relationshipOperationResult.AddOperationResult(String.Empty, errorMessage, OperationResultType.Error);
                                relationships.Remove(relationship);

                                if (isTracingEnabled)
                                {
                                    activity.LogError(errorMessage);
                                }
                            }
                            // if this error is not already added..
                            if (errorRORCollection.Contains(relationship.RelationshipExternalId, relationship.ToExternalId, relationship.RelationshipTypeName) == false)
                            {
                                errorRORCollection.Add(relationshipOperationResult);
                            }
                        }
                    }
                }

                #endregion Validate duplicate relationship

                if (warningRelationships.Count > 0)
                {
                    foreach (Relationship warningRelationship in warningRelationships)
                    {
                        relationships.Remove(warningRelationship);
                    }
                }

                if (removeBadRelationshipsFromProcessing)
                {
                    foreach (Relationship badRelationship in badRelationships)
                    {
                        relationships.Remove(badRelationship);
                    }
                }

                if (relationshipsWithWarnings != null && relationshipsWithWarnings.Count > 0)
                {
                    if (relationshipsWaitingForToEntityCreation == null)
                    {
                        relationshipsWaitingForToEntityCreation = new Dictionary<Int64, Dictionary<RelationshipCollection, RelationshipOperationResultCollection>>();
                    }

                    if (!relationshipsWaitingForToEntityCreation.ContainsKey(referenceId))
                    {
                        //Put into Dictionary for Retry
                        Dictionary<RelationshipCollection, RelationshipOperationResultCollection> relationshipsAndROR = new Dictionary<RelationshipCollection, RelationshipOperationResultCollection>();
                        relationshipsAndROR.Add(relationshipsWithWarnings, relationshipORWithWarnings);
                        relationshipsWaitingForToEntityCreation.Add(referenceId, relationshipsAndROR);
                    }
                    else
                    {
                        //In case of Retry, flow will come here. It means again warnings, add in OR
                        errorRORCollection.AddRange(relationshipORWithWarnings);
                        errorRORCollection.RefreshOperationResultStatus();
                    }
                }
            }

            if (isTracingEnabled)
            {
                message = string.Format("Fill and validate relationships completed for ({0}) relationships.", relationships.Count);
                activity.LogInformation(message);
                activity.Stop();
            }

            return errorRORCollection;
        }

        private void FillRelationship(Relationship relationship, Int64 fromEntityId, String fromEntityExternalId, Int32 fromEntityContainerId)
        {
            string message = string.Empty;
            Boolean isTracingEnabled = _traceSettings.IsBasicTracingEnabled;

            DiagnosticActivity activity = null;

            if (isTracingEnabled)
            {
                activity = new DiagnosticActivity();
                activity.Start();
            }

            EntityMapBL entityMapManager = new EntityMapBL();

            #region Fill Relationship From and To Item details

            //KEY NOTE: In 7.0 release, relationships are always direct..no 2nd level relationship imports..
            //Supporting nth level relationship import as a part of the release of 7.6.4..
            //Currently used only for TMSConnector import..
            relationship.FromEntityId = fromEntityId < 0 ? 0 : fromEntityId;

            Entity relatedEntity = new Entity();
            relatedEntity.ExternalId = relationship.ToExternalId;
            relatedEntity.CategoryPath = relationship.ToCategoryPath;
            relatedEntity.ContainerName = relationship.ToContainerName;
            relatedEntity.EntityTypeName = relationship.ToEntityTypeName;

            FillEntity(relatedEntity); //Use fill entity method to fetch relatedEntity's containerId, entityTypeId and

            EntityMap toEntityMap = null;

            if (!String.IsNullOrWhiteSpace(relatedEntity.ExternalId))
            {
                toEntityMap = entityMapManager.Get(systemId, relatedEntity.ExternalId, relatedEntity.ContainerId, relatedEntity.EntityTypeId, relatedEntity.CategoryId, importProfile.MappingSpecifications.EntityIdentificationMap, application, importmodule);
            }
            else
            {
                if (isTracingEnabled)
                    activity.LogInformation("Relationship: " + relationship.RelationshipExternalId + ". ExternalId_To is not provided thus not able to find Related(To) entity");
            }

            if (toEntityMap != null)
            {
                if (isTracingEnabled)
                    activity.LogInformation("Relationship: " + relationship.RelationshipExternalId + ". Related(To) Entity found with internal id: " + toEntityMap.InternalId);

                relatedEntity.Id = toEntityMap.InternalId;

                relationship.RelatedEntityId = relatedEntity.Id;
                relationship.ToContainerId = relatedEntity.ContainerId;
                relationship.ToEntityTypeId = relatedEntity.EntityTypeId;

                //relationship.ToCategoryId = relatedEntity.CategoryId;  //Relationship object dont have ToCategoryId property. Why???
            }

            //Assign related entity object to relation to use it further
            relationship.RelatedEntity = relatedEntity;

            #endregion

            #region Fill Relationship ToLongName

            if (String.IsNullOrWhiteSpace(relationship.ToLongName))
            {
                String toLongName = relationship.ToExternalId;

                if (!String.IsNullOrWhiteSpace(relatedEntity.LongName))
                {
                    toLongName = relatedEntity.LongName;
                }

                relationship.ToLongName = toLongName;
            }

            #endregion

            #region Fill Relationship ContainerId

            if (relationship.ContainerId <= 0)
            {
                relationship.ContainerId = fromEntityContainerId;
            }

            #endregion

            #region Fill Relationship Type

            if (relationship.RelationshipTypeId <= 0)
            {
                RelationshipType relationshipType = GetRelationshipTypeByName(relationship.RelationshipTypeName);

                if (relationshipType != null)
                {
                    if (isTracingEnabled)
                        activity.LogInformation("Relationship: " + relationship.RelationshipExternalId + ". Found RelationshipType. Id = " + relationshipType.Id);

                    relationship.RelationshipTypeId = relationshipType.Id;
                }
                else
                {
                    if (isTracingEnabled)
                    {
                        activity.LogWarning("Relationship: " + relationship.RelationshipExternalId + ". Could not find RelationshipType: " + relationship.RelationshipTypeName);
                    }
                }
            }

            #endregion

            #region Fill Relationship Path

            relationship.Path = String.Format("{0}_{1}", relationship.FromEntityId, relationship.RelatedEntityId);

            #endregion

            #region Fill Relationship Action

            if (relationship.Action == ObjectAction.Unknown || relationship.Action == ObjectAction.Read)
                relationship.Action = ObjectAction.Create;

            #endregion

            if (isTracingEnabled)
            {
                activity.LogInformation("Fill Relationship for Entity ExternalId: " + fromEntityExternalId + ", RelationshipExternalId: " + relationship.RelationshipExternalId + " done");
                activity.Stop();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="relationship"></param>
        /// <param name="relationshipOperationResult"></param>
        private Boolean ValidateRelationship(Relationship relationship, RelationshipOperationResult relationshipOperationResult, OperationResultType operationResultType)
        {
            string message = string.Empty;
            Boolean isTracingEnabled = _traceSettings.IsBasicTracingEnabled;

            DiagnosticActivity activity = null;

            if (isTracingEnabled)
            {
                activity = new DiagnosticActivity();
                activity.Start();
            }

            string errorMessage = string.Empty;

            int numberOfFailures = 0;

            try
            {
                //KEY NOTE: FromEntity can be empty only in case of relationship IMPORT for source providers other than StagingDB (not initial load and relationship load)
                //Creating FromEntity while importing relationship is not supported for RelationshipInitialLoad and RelationshipLoad. So validate and mark it as errored in case it is not available.
                if (importProfile.ProcessingSpecifications.ImportMode == ImportMode.RelationshipInitialLoad ||
                    importProfile.ProcessingSpecifications.ImportMode == ImportMode.RelationshipLoad)
                {
                    if (relationship.FromEntityId < 1)
                    {
                        // Mark as error
                        numberOfFailures++;
                        String externalIdFrom = String.IsNullOrWhiteSpace(relationship.RelationshipSourceEntityName) ? "NA" : relationship.RelationshipSourceEntityName;

                        errorMessage = String.Format(localeMessageBL.Get(systemUILocale, "113825", false, callerContext).Message,
                                                    externalIdFrom); //Source entity not found for ExternalId:{0}.

                        if (isTracingEnabled)
                        {
                            activity.LogError(errorMessage);
                        }
                        relationshipOperationResult.AddOperationResult("113825", errorMessage, new Collection<Object> { externalIdFrom }, operationResultType);

                    }
                }

                if (relationship.RelatedEntityId < 1)
                {
                    // Mark as error
                    numberOfFailures++;

                    String externalIdTo = String.IsNullOrWhiteSpace(relationship.ToExternalId) ? "NA" : relationship.ToExternalId;
                    String containerName = String.IsNullOrWhiteSpace(relationship.ToContainerName) ? "NA" : relationship.ToContainerName;
                    String entityTypeName = String.IsNullOrWhiteSpace(relationship.ToEntityTypeName) ? "NA" : relationship.ToEntityTypeName;
                    String categoryPath = String.IsNullOrWhiteSpace(relationship.ToCategoryPath) ? "NA" : relationship.ToCategoryPath;

                    errorMessage = String.Format(localeMessageBL.Get(systemUILocale, "111695", false, callerContext).Message,
                                                externalIdTo, containerName, entityTypeName, categoryPath); //Related entity not found for ExternalId:{0}, Container:{1}, EntityType:{2} and CategoryPath:{3}

                    if (isTracingEnabled)
                    {
                        activity.LogError(errorMessage);
                    }
                    relationshipOperationResult.AddOperationResult("111695", errorMessage, new Collection<Object> { externalIdTo, containerName, entityTypeName, categoryPath }, operationResultType);
                }

                if (relationship.ContainerId < 1)
                {
                    numberOfFailures++;

                    errorMessage = String.Format(localeMessageBL.Get(systemUILocale, "111696", false, callerContext).Message); //Container provided for relationship not found. Verify if container exists.

                    relationshipOperationResult.AddOperationResult(String.Empty, errorMessage, operationResultType);

                    if (isTracingEnabled)
                    {
                        activity.LogWarning(errorMessage);
                    }
                }

                if (relationship.RelationshipTypeId < 1)
                {
                    numberOfFailures++;

                    errorMessage = String.Format(localeMessageBL.Get(systemUILocale, "111697", false, callerContext).Message, relationship.RelationshipTypeName); //Relationship type '{0}' not found. Verify if relationship type exists.
                    relationshipOperationResult.AddOperationResult("111697", errorMessage, new Collection<Object> { relationship.RelationshipTypeName }, operationResultType);

                    if (isTracingEnabled)
                    {
                        activity.LogWarning(errorMessage);
                    }
                }

                if (relationship.RelatedEntityId > 0 && relationship.FromEntityId == relationship.RelatedEntityId)
                {
                    numberOfFailures++;

                    errorMessage = String.Format(localeMessageBL.Get(systemUILocale, "111698", false, callerContext).Message, relationship.ToExternalId);//Self relationship is not allowed. ToExternalId: {0}
                    relationshipOperationResult.AddOperationResult("111698", errorMessage, new Collection<Object> { relationship.ToExternalId }, operationResultType);

                    if (isTracingEnabled)
                    {
                        activity.LogWarning(errorMessage);
                    }
                }
            }
            catch (Exception ex)
            {
                if (isTracingEnabled)
                {
                    activity.LogError(String.Format("ValidateAttribute for RelationshipExternalId:{0} failed with exception:{1}:", relationship.RelationshipExternalId, ex.Message));
                }
                throw;
            }
            finally
            {
                if (isTracingEnabled)
                {
                    activity.LogInformation(String.Format("ValidateRelationship for RelationshipExternalId:{0} completed.", relationship.RelationshipExternalId));
                    activity.Stop();
                }
            }

            return numberOfFailures > 0 ? false : true;
        }

        #endregion

        #region Data Model Get Methods

        /// <summary>
        /// The import process needs to get the entity using the path.
        /// </summary>
        /// <param name="categoryPath"></param>
        /// <returns></returns>
        private Category GetCategoryByPath(String categoryPath)
        {
            Category category = null;

            if (string.IsNullOrEmpty(categoryPath))
            {
                return null;
            }

            categoryPath = categoryPath.Replace("#@#", categoryPathSeparator);
            categoryPath = categoryPath.Replace("//", categoryPathSeparator);

            foreach (KeyValuePair<Int32, CategoryCollection> keyValuePair in this.cachedDataModel.GetCategories())
            {
                if (keyValuePair.Value != null && keyValuePair.Value.Count() > 0)
                {
                    category = keyValuePair.Value.FirstOrDefault(cat => cat.Path.Equals(categoryPath, StringComparison.InvariantCultureIgnoreCase));

                    if (category == null)
                    {
                        //try to find category by long name path..
                        category = keyValuePair.Value.FirstOrDefault(cat => cat.LongNamePath.Equals(categoryPath, StringComparison.InvariantCultureIgnoreCase));
                    }

                    if (category != null)
                        break;
                }
            }

            return category;
        }

        /// <summary>
        /// The import process needs to get the entity using the path.
        /// </summary>
        /// <param name="categoryPath"></param>
        /// <param name="containerName"></param>
        /// <returns></returns>
        private Category GetCategoryByPath(String categoryPath, String containerName)
        {
            if (string.IsNullOrEmpty(categoryPath))
            {
                return null;
            }

            categoryPath = categoryPath.Replace("#@#", categoryPathSeparator);
            categoryPath = categoryPath.Replace("//", categoryPathSeparator);

            Container container = GetContainerByName(containerName);

            //If we are not able to find container then return null because based on  hierarchy id, we will try to find category.
            //If we continue then we may be end with wrong category from different taxonomy.
            if (container == null)
                return null;

            return this.cachedDataModel.GetCategory(container.HierarchyId, categoryPath);
        }

        private Category GetCategoryById(Int64 categoryId, String containerName)
        {
            if (categoryId <= 0)
            {
                return null;
            }

            Container container = GetContainerByName(containerName);

            //If we are not able to find container then return null because based on  hierarchy id, we will try to find category.
            //If we continue then we may be end with wrong category from different taxonomy.
            if (container == null)
                return null;

            return this.cachedDataModel.GetCategory(container.HierarchyId, categoryId);
        }

        /// <summary>
        /// The import process needs to get the entity using the path.
        /// </summary>
        /// <param name="categoryPath"></param>
        /// <param name="containerName"></param>
        /// <param name="organizationName"></param>
        /// <returns></returns>
        private Category GetCategoryByPath(String categoryPath, String containerName, String organizationName)
        {
            if (string.IsNullOrEmpty(categoryPath))
            {
                return null;
            }

            categoryPath = categoryPath.Replace("#@#", categoryPathSeparator);
            categoryPath = categoryPath.Replace("//", categoryPathSeparator);

            Container container = GetContainerByNameAndOrganizationName(containerName, organizationName);

            //If we are not able to find container then return null because based on  hierarchy id, we will try to find category.
            //If we continue then we may be end with wrong category from different taxonomy.
            if (container == null)
                return null;

            return this.cachedDataModel.GetCategory(container.HierarchyId, categoryPath);
        }

        /// <summary>
        /// Get entity type based on the name. entity types are stored in static variable entityTypes
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private EntityType GetEntityTypeByName(String name)
        {
            string message = string.Empty;
            Boolean isTracingEnabled = _traceSettings.IsBasicTracingEnabled;
            DiagnosticActivity activity = null;
            if (isTracingEnabled)
            {
                activity = new DiagnosticActivity();
                activity.Start();
            }

            if (string.IsNullOrEmpty(name))
            {
                return null;
            }

            List<EntityType> entityTypes = this.cachedDataModel.GetEntityTypes();

            var filteredEntityTypes = (
                from entityType in entityTypes
                where (entityType.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase))
                select entityType
            );

            if (!filteredEntityTypes.Any())
            {
                filteredEntityTypes = (
                    from entityType in entityTypes
                    where (entityType.LongName.Equals(name, StringComparison.InvariantCultureIgnoreCase))
                    select entityType
                );
            }

            EntityType filteredEventType = null;

            if (filteredEntityTypes.Any())
            {
                filteredEventType = filteredEntityTypes.FirstOrDefault();

                if (filteredEventType != null && isTracingEnabled)
                {
                    message = String.Format("Multiple entity types found, proceeding with first entity type ({0}) - {1}",
                        filteredEventType.Id, String.Join(", ", filteredEntityTypes.Select(x => string.Format("{0}({1})", x.Name, x.Id)).ToList()));

                    activity.LogWarning(message);
                }
            }

            if (isTracingEnabled) activity.Stop();

            return filteredEventType;
        }

        /// <summary>
        /// Get Relationship type based on the name. Relationship types are stored in static variable RelationshipTypes
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private RelationshipType GetRelationshipTypeByName(String name)
        {
            string message = string.Empty;
            Boolean isTracingEnabled = _traceSettings.IsBasicTracingEnabled;
            DiagnosticActivity activity = null;
            if (isTracingEnabled)
            {
                activity = new DiagnosticActivity();
                activity.Start();
            }

            try
            {
                if (string.IsNullOrEmpty(name))
                {
                    return null;
                }

                var filteredRelationshipTypes = (
                    from relationshipType in this.cachedDataModel.GetRelationshipTypes()
                    where (relationshipType.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase))
                    select relationshipType
                );

                if (!filteredRelationshipTypes.Any())
                {
                    filteredRelationshipTypes = (
                        from relationshipType in this.cachedDataModel.GetRelationshipTypes()
                        where (relationshipType.LongName.Equals(name, StringComparison.InvariantCultureIgnoreCase))
                        select relationshipType
                    );
                }

                RelationshipType filteredRelationshipType = null;
                if (filteredRelationshipTypes.Any())
                {
                    filteredRelationshipType = filteredRelationshipTypes.FirstOrDefault();

                    if (filteredRelationshipType != null && isTracingEnabled)
                    {
                        message = String.Format("Multiple relationship types found, proceeding with first relationship type ({0}) - {1}",
                                filteredRelationshipType.Id, String.Join(", ", filteredRelationshipTypes.Select(x => string.Format("{0}({1})", x.Name, x.Id)).ToList()));

                        activity.LogWarning(message);
                    }
                }

                return filteredRelationshipType;
            }
            finally
            {
                if (isTracingEnabled) activity.Stop();
            }
        }

        /// <summary>
        /// Returns the organization from local cache
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private Organization GetOrganizationById(Int32 id)
        {
            //TODO How many orgs can there? For now return the first one..
            return cachedDataModel.GetOrganizations()[0];
        }

        /// <summary>
        /// Returns the organization from local cache
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private Organization GetOrganizationByName(String name)
        {
            string message = string.Empty;
            Boolean isTracingEnabled = _traceSettings.IsBasicTracingEnabled;
            DiagnosticActivity activity = null;
            if (isTracingEnabled)
            {
                activity = new DiagnosticActivity();
                activity.Start();
            }

            try
            {
                if (string.IsNullOrEmpty(name))
                {
                    return null;
                }

                var filteredOrgs = (
                    from organization in this.cachedDataModel.GetOrganizations()
                    where (organization.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase))
                    select organization
                );

                if (!filteredOrgs.Any())
                {
                    filteredOrgs = (
                        from organization in this.cachedDataModel.GetOrganizations()
                        where (organization.LongName.Equals(name, StringComparison.InvariantCultureIgnoreCase))
                        select organization
                    );
                }

                Organization filteredOrg = null;
                if (filteredOrgs.Any())
                {
                    filteredOrg = filteredOrgs.FirstOrDefault();

                    if (filteredOrg != null && isTracingEnabled)
                    {
                        message = String.Format("Multiple organizations found, proceeding with first organization ({0}) - {1}",
                                filteredOrg.Id, String.Join(", ", filteredOrgs.Select(x => String.Format("{0}({1})", x.Name, x.Id)).ToList()));

                        activity.LogWarning(message);
                    }
                }

                return filteredOrg;
            }
            finally
            {
                if (isTracingEnabled) activity.Stop();
            }
        }

        /// <summary>
        /// Get container based on the name. containers are stored in static variable containers
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private Container GetContainerByName(String name)
        {
            string message = string.Empty;
            Boolean isTracingEnabled = _traceSettings.IsBasicTracingEnabled;
            DiagnosticActivity activity = null;
            if (isTracingEnabled)
            {
                activity = new DiagnosticActivity();
                activity.Start();
            }

            try
            {
                if (string.IsNullOrEmpty(name))
                {
                    return null;
                }

                var filteredContainers = (
                    from container in this.cachedDataModel.GetContainers()
                    where (container.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase))
                    select container
                );

                if (!filteredContainers.Any())
                {
                    filteredContainers = (
                        from container in this.cachedDataModel.GetContainers()
                        where (container.LongName.Equals(name, StringComparison.InvariantCultureIgnoreCase))
                        select container
                    );
                }

                Container filteredContainer = null;
                if (filteredContainers.Any())
                {
                    filteredContainer = filteredContainers.FirstOrDefault();

                    if (filteredContainer != null && isTracingEnabled)
                    {
                        message = String.Format("Multiple containers found, proceeding with first container ({0}) - {1}",
                            filteredContainer.Id, String.Join(", ", filteredContainers.Select(x => String.Format("{0}({1})", x.Name, x.Id)).ToList()));

                        activity.LogWarning(message);
                    }
                }

                return filteredContainer;
            }
            finally
            {
                if (isTracingEnabled) activity.Stop();
            }
        }

        /// <summary>
        ///  Get container based on the container name and organization name. containers are stored in static variable containers
        /// </summary>
        /// <param name="containerName"></param>
        /// <param name="organizationName"></param>
        /// <returns></returns>
        private Container GetContainerByNameAndOrganizationName(String containerName, String organizationName)
        {
            string message = string.Empty;
            Boolean isTracingEnabled = _traceSettings.IsBasicTracingEnabled;
            DiagnosticActivity activity = null;
            if (isTracingEnabled)
            {
                activity = new DiagnosticActivity();
                activity.Start();
            }

            try
            {
                if (String.IsNullOrWhiteSpace(containerName) || String.IsNullOrWhiteSpace(organizationName))
                {
                    return null;
                }

                var filteredContainers = this.cachedDataModel.GetContainers().Where(
                    container => container.Name.Equals(containerName, StringComparison.InvariantCultureIgnoreCase) &&
                        container.OrganizationShortName.Equals(organizationName, StringComparison.InvariantCultureIgnoreCase)
                );

                if (!filteredContainers.Any())
                {
                    filteredContainers = this.cachedDataModel.GetContainers().Where(
                        container => container.LongName.Equals(containerName, StringComparison.InvariantCultureIgnoreCase) &&
                            container.OrganizationLongName.Equals(organizationName, StringComparison.InvariantCultureIgnoreCase)
                    );
                }

                Container filteredContainer = null;

                if (filteredContainers.Any())
                {
                    filteredContainer = filteredContainers.FirstOrDefault();

                    if (filteredContainer != null && isTracingEnabled)
                    {
                        message = String.Format("Multiple containers found, proceeding with first container ({0}) - {1}",
                            filteredContainer.Id, String.Join(", ", filteredContainers.Select(x => String.Format("{0}({1})", x.Name, x.Id)).ToList()));

                        activity.LogWarning(message);
                    }
                }

                return filteredContainer;
            }
            finally
            {
                if (isTracingEnabled) activity.Stop();
            }
        }

        /// <summary>
        /// Find if a given container is in the MDL list..
        /// </summary>
        /// <param name="containerName"></param>
        /// <returns></returns>
        private bool IsContainerInMDLList(String containerName)
        {
            foreach (Container container in mdlcontainerList.Values)
            {
                if (String.Compare(containerName, container.Name, true) == 0)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Remove all the entities that belong to containers already processed.
        /// </summary>
        /// <param name="entityCollection"></param>
        private void RemoveEntitiesFromMDLContainers(EntityCollection entityCollection)
        {
            string message = string.Empty;
            Boolean isTracingEnabled = _traceSettings.IsBasicTracingEnabled;
            DiagnosticActivity activity = null;
            if (isTracingEnabled)
            {
                activity = new DiagnosticActivity();
                activity.Start();
            }

            HashSet<Entity> removeEntities = new HashSet<Entity>();
            foreach (Entity entity in entityCollection)
            {
                if (IsContainerInMDLList(entity.ContainerName))
                {
                    removeEntities.Add(entity);
                }
            }

            if (removeEntities.Count > 0)
            {
                if (isTracingEnabled)
                    activity.LogInformation(String.Format("{0} entities will be removed from processing as they belong to containers already processed.", removeEntities.Count));

                //now remove these from the list.
                foreach (Entity entity in removeEntities)
                {
                    entityCollection.Remove(entity);
                }
            }

            if (isTracingEnabled) activity.Stop();
        }

        private AttributeModelCollection GetContextualAttributeModels(Entity entity)
        {
            Boolean isTracingEnabled = _traceSettings.IsBasicTracingEnabled;
            DiagnosticActivity activity = null;
            if (isTracingEnabled)
            {
                activity = new DiagnosticActivity();
                activity.Start();
            }

            Collection<LocaleEnum> locales = new Collection<LocaleEnum>();
            if (entity.Attributes != null && entity.Attributes.Count > 0)
            {
                locales = entity.Attributes.GetLocaleList();
            }

            //While importing category level attributes, technical attributes which are mapped at current node should also be returned
            //So, setting CategoryId as EntityId for Category
            Int64 categoryId = (entity.BranchLevel == ContainerBranchLevel.Node) ? entity.Id : entity.CategoryId;

            AttributeModelContext attributeModelContext = new AttributeModelContext()
            {
                AttributeModelType = AttributeModelType.All,
                ContainerId = entity.ContainerId,
                EntityTypeId = entity.EntityTypeId,
                CategoryId = categoryId,
                Locales = locales,
                ApplySecurity = false,
                ApplySorting = false
            };

            AttributeModelCollection models = this.cachedDataModel.GetContextualAttributeModels(attributeModelContext);

            if (isTracingEnabled)
            {
                activity.LogInformation(String.Format("GetAttributeModels for Entity: {0} AttributeModelContext: {1} completed", entity.Name,
                    attributeModelContext.ToXml()));
                activity.Stop();
            }

            return models;
        }

        private AttributeModelCollection GetContextualAttributeModels(Relationship relationship)
        {
            Boolean isTracingEnabled = _traceSettings.IsBasicTracingEnabled;
            DiagnosticActivity activity = null;
            if (isTracingEnabled)
            {
                activity = new DiagnosticActivity();
                activity.Start();
            }

            Collection<LocaleEnum> locales = new Collection<LocaleEnum>();
            if (relationship.RelationshipAttributes != null && relationship.RelationshipAttributes.Count > 0)
            {
                locales = relationship.RelationshipAttributes.GetLocaleList();
            }

            Category category = GetCategoryByPath(relationship.ToCategoryPath);

            AttributeModelContext attributeModelContext = new AttributeModelContext()
            {
                AttributeModelType = AttributeModelType.All,
                ContainerId = relationship.ContainerId,
                RelationshipTypeId = relationship.RelationshipTypeId,
                CategoryId = (category != null) ? category.Id : 0,
                Locales = locales,
                ApplySecurity = false,
                ApplySorting = false
            };

            AttributeModelCollection models = this.cachedDataModel.GetContextualAttributeModels(attributeModelContext);

            if (isTracingEnabled)
            {
                activity.LogInformation(String.Format("GetAttributeModels for Relationship: {0} AttributeModelContext: {1} completed", relationship.Name,
                    attributeModelContext.ToXml()));
                activity.Stop();
            }

            return models;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="attribute"></param>
        /// <returns></returns>
        private AttributeModel GetAttributeModelFromCachedDataModel(Entity entity, Attribute attribute)
        {
            string message = string.Empty;
            Boolean isTracingEnabled = _traceSettings.IsBasicTracingEnabled;
            DiagnosticActivity activity = null;
            if (isTracingEnabled)
            {
                activity = new DiagnosticActivity();
                activity.Start();
            }

            AttributeModelContext attributeModelContext = new AttributeModelContext();
            attributeModelContext.AttributeModelType = AttributeModelType.All;
            attributeModelContext.ContainerId = entity.ContainerId;
            attributeModelContext.EntityTypeId = entity.EntityTypeId;

            //While importing category level attributes, technical attributes which are mapped at current node should also be returned
            //So, setting CategoryId as EntityId for Category
            if (entity.BranchLevel == ContainerBranchLevel.Node)
            {
                attributeModelContext.CategoryId = entity.Id;//will always be >-1, because we always update category never create it
            }
            else
            {
                attributeModelContext.CategoryId = entity.CategoryId;
            }

            attributeModelContext.Locales = new Collection<LocaleEnum> { attribute.Locale };

            AttributeModel model = this.cachedDataModel.GetContextualAttributeModel(attributeModelContext, attribute);

            if (isTracingEnabled)
            {
                activity.LogInformation("GetAttributeModel for Entity: " + entity.Name + " Attribute: " + attribute.Name + " completed");
                activity.Stop();
            }

            return model;
        }

        private void ApplyRepeatedSourceAttributeMappings(AttributeCollection attributeCollection)
        {
            //Here, we need to create multiple attributes from one source
            if (repeatedAttributeSources.Count > 0)
            {
                Dictionary<AttributeSource, Attribute> distinctAttributeSourceAndAttributes = new Dictionary<AttributeSource, Attribute>();

                foreach (AttributeMap attrMap in repeatedAttributeSources)
                {
                    var attrs = from a in attributeCollection
                                where a.Name == attrMap.AttributeSource.StagingAttributeInfo.Name
                                   && a.AttributeParentName == attrMap.AttributeSource.StagingAttributeInfo.AttributeParentName
                                   && a.Locale == attrMap.AttributeSource.StagingAttributeInfo.Locale
                                select a;

                    if (attrs != null && attrs.Any())
                    {
                        Attribute sourceAttribute = attrs.FirstOrDefault();

                        if (sourceAttribute != null)
                        {
                            if (!distinctAttributeSourceAndAttributes.ContainsKey(attrMap.AttributeSource))
                            {
                                distinctAttributeSourceAndAttributes.Add(attrMap.AttributeSource, sourceAttribute);
                                attributeCollection.Remove(sourceAttribute);
                            }
                        }
                    }
                }

                if (distinctAttributeSourceAndAttributes.Count > 0)
                {
                    foreach (KeyValuePair<AttributeSource, Attribute> pair in distinctAttributeSourceAndAttributes)
                    {
                        AttributeSource attrSource = pair.Key;
                        Attribute sourceAttrObj = pair.Value;

                        foreach (AttributeMap map in importProfile.MappingSpecifications.AttributeMaps)
                        {
                            if (attrSource.Name.Equals(map.AttributeSource.Name, StringComparison.InvariantCultureIgnoreCase))
                            {
                                Attribute newAttr = new Attribute(sourceAttrObj.ToXmlInvariant());

                                AssignTargetMap(map, newAttr);
                                attributeCollection.Add(newAttr);
                                map.IsProcessed = true;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="relationship"></param>
        /// <param name="attribute"></param>
        /// <returns></returns>
        private AttributeModel GetAttributeModelFromCachedDataModel(Relationship relationship, Attribute attribute)
        {
            string message = string.Empty;
            Boolean isTracingEnabled = _traceSettings.IsBasicTracingEnabled;
            DiagnosticActivity activity = null;
            if (isTracingEnabled)
            {
                activity = new DiagnosticActivity();
                activity.Start();
            }

            AttributeModelContext attributeModelContext = new AttributeModelContext();
            attributeModelContext.AttributeModelType = AttributeModelType.All;
            attributeModelContext.ContainerId = relationship.ContainerId;
            attributeModelContext.RelationshipTypeId = relationship.RelationshipTypeId;

            Category category = GetCategoryByPath(relationship.ToCategoryPath);

            if (category != null)
            {
                attributeModelContext.CategoryId = category.Id;
            }

            attributeModelContext.Locales = new Collection<LocaleEnum> { attribute.Locale };

            AttributeModel model = this.cachedDataModel.GetContextualAttributeModel(attributeModelContext, attribute);

            if (isTracingEnabled)
            {
                activity.LogInformation("GetAttributeModel for Entity: " + relationship.Name + " Attribute: " + attribute.Name + " completed");
                activity.Stop();
            }

            return model;
        }

        private AttributeOperationResultCollection ExplodeAttributeOperationResultForCollection(Attribute attribute, AttributeOperationResult attributeOperationResult)
        {
            AttributeOperationResultCollection resultCollection = new AttributeOperationResultCollection();

            foreach (Value value in attribute.GetCurrentValuesInvariant())
            {
                AttributeOperationResult valueOperationResult = new AttributeOperationResult(attributeOperationResult.ToXml());
                valueOperationResult.AttributeId = ValueTypeHelper.Int32TryParse(value.Id.ToString(), -1);
                resultCollection.Add(valueOperationResult);
            }
            return resultCollection;
        }

        #endregion

        #region Logging and Error Handling

        /// <summary>
        /// Log successful entities if required.
        /// </summary>
        /// <param name="entityOperationResultCollection"></param>
        private void LogEntitySuccess(EntityOperationResultCollection entityOperationResultCollection)
        {
            if (jobResultSaveSuccessEntities)
            {
                // Get entity list with create action
                List<EntityOperationResult> entityOperationList = (from operationResult in entityOperationResultCollection
                                                                   where (operationResult.OperationResultStatus == OperationResultStatusEnum.Successful || operationResult.OperationResultStatus == OperationResultStatusEnum.None)
                                                                   select operationResult).ToList();
                // do we have anything to process?
                if (entityOperationList.Count == 0)
                    return;

                EntityOperationResultCollection successCollection = new EntityOperationResultCollection();
                foreach (EntityOperationResult item in entityOperationList)
                {
                    item.Informations.Add(new Information("112180", String.Empty, new Collection<object> { item.ExternalId })); //saved successfully
                    item.OperationResultStatus = OperationResultStatusEnum.Successful;
                    successCollection.Add(item);
                }
                // update the job import result
                jobImportResultHandler.Save(successCollection, jobResultSaveSuccessEntities);
            }
        }

        /// <summary>
        /// Update the soure with the error message for a given entity. This will also REMOVE the errored items from the entity collection from further processing.
        /// </summary>
        /// <param name="entityOperationResultCollection"></param>
        private void LogEntityErrors(EntityOperationResultCollection entityOperationResultCollection, EntityCollection entityCollection, bool removeFromProcessing, IEnumerable<Int64> warningEntityIdList = null)
        {
            string message = string.Empty;
            Boolean isTracingEnabled = _traceSettings.IsBasicTracingEnabled;
            DiagnosticActivity activity = null;
            if (isTracingEnabled)
            {
                activity = new DiagnosticActivity();
                activity.Start();
            }

            // Get entity operation list with failed status
            List<EntityOperationResult> entityOperationList = (from operationResult in entityOperationResultCollection
                                                               where operationResult.OperationResultStatus == OperationResultStatusEnum.Failed
                                                               || operationResult.OperationResultStatus == OperationResultStatusEnum.CompletedWithWarnings
                                                               || operationResult.OperationResultStatus == OperationResultStatusEnum.CompletedWithErrors
                                                               select operationResult).ToList();

            if ((entityOperationList != null) && (entityOperationList.Count() > 0))
            {
                // Dont log entity errors here for relationship load
                if (importProfile.ProcessingSpecifications.ImportMode != ImportMode.RelationshipLoad)
                {
                    InternalLogEntityErrors(entityOperationList, entityCollection, removeFromProcessing, warningEntityIdList);
                }
                else
                {
                    // when there is a error at the entity level, the relationship would not have been processed.. so update all the relationships for that entity
                    // to same error also. This will update the error in the relationship staging tables.
                    foreach (EntityOperationResult item in entityOperationList)
                    {
                        if (item.HasError)
                        {
                            foreach (RelationshipOperationResult ror in item.RelationshipOperationResultCollection)
                            {
                                ror.Errors = item.Errors;
                                ror.OperationResultStatus = OperationResultStatusEnum.Failed;
                            }
                            item.RelationshipOperationResultCollection.RefreshOperationResultStatus();
                        }
                    }
                }
            }

            if (isTracingEnabled) activity.Stop();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityOperationResultCollection"></param>
        /// <param name="entityCollection"></param>
        /// <param name="removeFromProcessing"></param>
        private void InternalLogEntityErrors(List<EntityOperationResult> entityOperationList, EntityCollection entityCollection, bool removeFromProcessing, IEnumerable<Int64> warningEntityIdList)
        {
            string message = string.Empty;
            Boolean isTracingEnabled = _traceSettings.IsBasicTracingEnabled;
            DiagnosticActivity activity = null;
            if (isTracingEnabled)
            {
                activity = new DiagnosticActivity();
                activity.Start();
            }

            EntityOperationResultCollection errorCollection = new EntityOperationResultCollection();
            AttributeOperationResultCollection errorAORCollection = new AttributeOperationResultCollection();
            Int64 failedEntitiesCount = 0;
            Int64 partialSuccessfulEntitiesCount = 0;
            Int64 unChangeEntitiesCount = 0;

            foreach (EntityOperationResult item in entityOperationList)
            {
                if (item.OperationResultStatus == OperationResultStatusEnum.CompletedWithWarnings)
                {
                    if (warningEntityIdList == null || (warningEntityIdList != null && !warningEntityIdList.Contains(item.ReferenceId)))
                    {
                        if (item.PerformedAction == ObjectAction.Read)
                        {
                            unChangeEntitiesCount++;
                        }
                        else
                        {
                            partialSuccessfulEntitiesCount++;
                        }
                    }
                    else
                    {
                        //Entity is already updated into this list(partial complete list. so no need to update the count again).
                    }
                }
                else
                {
                    if (removeFromProcessing && item.ReferenceId > 0)
                    {
                        // VERY VERY IMPORTANT...this item is removed from further processing.....
                        entityCollection.RemoveByReferenceId(item.ReferenceId);
                    }

                    failedEntitiesCount++;

                    if (warningEntityIdList != null && warningEntityIdList.Contains(item.ReferenceId))
                    {
                        //Because same entity is updated as partial complete. But after entity process it became error. 
                        //So need to reduce the partial complete count.
                        partialSuccessfulEntitiesCount--;
                    }
                }

                errorCollection.Add(item);

                AttributeOperationResultCollection aorCollection = item.AttributeOperationResultCollection;

                if (aorCollection != null && aorCollection.Count > 0)
                {
                    foreach (AttributeOperationResult aor in aorCollection)
                    {
                        if (aor.OperationResultStatus == OperationResultStatusEnum.Failed ||
                            aor.OperationResultStatus == OperationResultStatusEnum.CompletedWithErrors ||
                            aor.OperationResultStatus == OperationResultStatusEnum.CompletedWithWarnings)
                        {
                            errorAORCollection.Add(aor);
                        }
                    }
                }

            }

            // update the source
            sourceData.UpdateErrorEntities(errorCollection, application, stagingModule);

            // Update the attributes
            if (errorAORCollection.Count > 0)
            {
                sourceData.UpdateErrorAttributes(errorAORCollection, application, stagingModule);
            }

            // update the progress
            progressHandler.UpdateFailedEntities(failedEntitiesCount);

            progressHandler.UpdatePartialSuccessFulEntities(partialSuccessfulEntitiesCount);

            // Update the no change entity count
            progressHandler.UpdateUnChangeEntities(unChangeEntitiesCount);

            // update the job import result
            jobImportResultHandler.Save(errorCollection);

            if (isTracingEnabled) activity.Stop();
        }

        /// <summary>
        /// Log successful entities if required.
        /// </summary>
        /// <param name="entityOperationResultCollection"></param>
        private void LogRelationshipSuccess(EntityOperationResultCollection entityOperationResultCollection, EntityCollection entityCollection)
        {
            EntityCollection successEntityCollecion = new EntityCollection();

            RelationshipOperationResultCollection rorCollection = new RelationshipOperationResultCollection();

            foreach (EntityOperationResult entityOperationResult in entityOperationResultCollection)
            {
                Entity newEntity = new Entity();

                foreach (RelationshipOperationResult ror in entityOperationResult.RelationshipOperationResultCollection)
                {
                    if (ror.OperationResultStatus == OperationResultStatusEnum.Successful || ror.OperationResultStatus == OperationResultStatusEnum.CompletedWithErrors)
                    {
                        rorCollection.Add(ror);
                        Relationship relationship = null;
                        // Get the current entity
                        relationship = entityCollection.GetRelationshipByExternalId(ror.RelationshipExternalId);

                        if (relationship != null)
                        {
                            newEntity.Relationships.Add(relationship);
                        }
                    }
                }
                if (newEntity.Relationships.Count > 0)
                {
                    newEntity.Id = entityOperationResult.EntityId;
                    successEntityCollecion.Add(newEntity);
                }
            }

            // do we have anything to process?
            if (rorCollection.Count == 0)
                return;

            // update the source data back..This will acutally update the staging relationship table
            sourceData.UpdateSuccessEntities(successEntityCollecion, application, stagingModule);
            // update status and progress handler
            relationshipProgressHandler.UpdateSuccessFulEntities(rorCollection.Count);
        }


        /// <summary>
        /// Log successful attribiutes if required.
        /// </summary>
        /// <param name="entityOperationResultCollection"></param>
        private void LogRelationshipAttributesSuccess(EntityOperationResultCollection entityOperationResultCollection)
        {
            EntityCollection successEntityCollecion = new EntityCollection();

            foreach (EntityOperationResult entityOperationResult in entityOperationResultCollection)
            {
                Entity newEntity = new Entity();

                foreach (RelationshipOperationResult ror in entityOperationResult.RelationshipOperationResultCollection)
                {
                    if (ror.OperationResultStatus == OperationResultStatusEnum.Successful)
                    {
                        Relationship relationship = new Relationship();
                        relationship.RelationshipExternalId = ror.RelationshipExternalId;
                        newEntity.Relationships.Add(relationship);
                    }
                }

                if (newEntity.Relationships.Count > 0)
                {
                    newEntity.Id = entityOperationResult.EntityId;
                    successEntityCollecion.Add(newEntity);
                }
            }

            // do we have anything to process?
            if (successEntityCollecion.Count == 0)
                return;

            // update status and progress handler
            sourceData.UpdateSuccessAttributes(AttributeModelType.Relationship, successEntityCollecion, application, stagingModule);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityOperationResultCollection"></param>
        private void LogRelationshipErrors(EntityOperationResultCollection entityOperationResultCollection)
        {
            RelationshipOperationResultCollection errorRORCollection = new RelationshipOperationResultCollection();
            AttributeOperationResultCollection errorAORCollection = new AttributeOperationResultCollection();

            Int32 failedRelationships = 0;
            Int32 completedWithErrorRelationships = 0;
            Int64 warnedRelationships = 0;

            foreach (EntityOperationResult entityOperationResult in entityOperationResultCollection)
            {
                foreach (RelationshipOperationResult ror in entityOperationResult.RelationshipOperationResultCollection)
                {
                    if (ror.OperationResultStatus == OperationResultStatusEnum.Failed)
                    {
                        failedRelationships++;
                        errorRORCollection.Add(ror);
                    }
                    else if (ror.OperationResultStatus == OperationResultStatusEnum.CompletedWithWarnings)
                    {
                        warnedRelationships++;
                        errorRORCollection.Add(ror);
                    }

                    /*
                    if (ror.OperationResultStatus == OperationResultStatusEnum.CompletedWithErrors)
                    {
                        completedWithErrorRelationships++;
                        ror.AddOperationResult(String.Empty, "Some of the relationship attributes failed to processs due to errors. Please fix attribute errors and re-run load", OperationResultType.Error);
						
                        errorRORCollection.Add(ror);
                    }
                    */
                    AttributeOperationResultCollection aorCollection = ror.AttributeOperationResultCollection;

                    if (aorCollection != null && aorCollection.Count > 0)
                    {
                        foreach (AttributeOperationResult aor in aorCollection)
                        {
                            if (aor.OperationResultStatus == OperationResultStatusEnum.Failed || aor.OperationResultStatus == OperationResultStatusEnum.CompletedWithWarnings)
                            {
                                errorAORCollection.Add(aor);
                            }
                        }
                    }
                }
            }

            // do we have anything to process?
            if (errorRORCollection.Count > 0)
            {
                // update the source for error relationships
                sourceData.UpdateErrorRelationships(errorRORCollection, application, stagingModule);
            }

            if (errorAORCollection.Count > 0)
            {
                // update the source for error attributes
                sourceData.UpdateErrorAttributes(errorAORCollection, application, stagingModule);
            }

            // update the progress
            relationshipProgressHandler.UpdateFailedEntities(failedRelationships);
            relationshipProgressHandler.UpdatePartialSuccessFulEntities(completedWithErrorRelationships);
        }

        /// <summary>
        /// Update the job status using the progress handler
        /// </summary>
        /// <param name="jobStatus"></param>
        private void UpdateJobStatus(JobStatus jobStatus)
        {
            string message = string.Empty;
            Boolean isTracingEnabled = _traceSettings.IsBasicTracingEnabled;
            DiagnosticActivity activity = null;
            if (isTracingEnabled)
            {
                activity = new DiagnosticActivity();
                activity.Start();
            }

            String description = String.Empty;

            if (importProfile.ProcessingSpecifications.ImportMode == ImportMode.RelationshipLoad ||
                importProfile.ProcessingSpecifications.ImportMode == ImportMode.RelationshipInitialLoad)
            {
                description = string.Format("Total {0} Relationships Processed. Successful - {1}, Partial Success - {2}, No change - {3} ,and Failed - {4}.",
                                             relationshipProgressHandler.GetCompletedEntities(), relationshipProgressHandler.GetSuccessFulEntities(),
                                             relationshipProgressHandler.GetPartialSuccessFulEntities(),
                                             relationshipProgressHandler.GetUnChangedEntitiesCount(),
                                             relationshipProgressHandler.GetFailedEntities());
                if (isTracingEnabled)
                    activity.LogInformation(description);
            }
            else
            {
                Int64 completedEntitiesCount = progressHandler.GetCompletedEntities();

                description = string.Format("Total {0} Entities Processed. Successful - {1}, Partial Success - {2}, No change - {3} ,and Failed - {4}.",
                                                completedEntitiesCount,
                                                progressHandler.GetSuccessFulEntities(),
                                                progressHandler.GetPartialSuccessFulEntities(),
                                                progressHandler.GetUnChangedEntitiesCount(),
                                                progressHandler.GetFailedEntities());

                if (isTracingEnabled)
                    activity.LogInformation(description);

                if (jobStatus == JobStatus.Completed)
                {
                    if (completedEntitiesCount == 0)
                    {
                        // if the job completed and we did not process any entity, that mean we did not have anything to process.
                        description = "No entities were processed. Check and make sure the source provider has entities to process.";
                    }
                    else if (completedEntitiesCount == progressHandler.GetUnChangedEntitiesCount())
                    {
                        // Job completed with no entity changes in process.
                        description = "Import had no changed entities.";
                    }

                    // indicate to users some entities had partial attributes processed. This is only applicable in Entity Initial Load from staging database
                    if (completedEntitiesCount != progressHandler.GetFailedEntities())
                    {
                        Int64 commonAttributesFailure = progressHandler.GetFailedEntitiesForAttributes(AttributeModelType.Common);

                        if (commonAttributesFailure > 0)
                        {
                            description = String.Concat(description, String.Format(" {0} entities had common attributes that were partially processed.", commonAttributesFailure));
                        }

                        Int64 categoryAttributesFailure = progressHandler.GetFailedEntitiesForAttributes(AttributeModelType.Category);
                        if (categoryAttributesFailure > 0)
                        {
                            description = String.Concat(description, String.Format(" {0} entities had technical attributes that were partially processed.", categoryAttributesFailure));
                        }

                        if (categoryAttributesFailure > 0 || commonAttributesFailure > 0)
                        {
                            description = String.Concat(description, " Check the staging database for the error details.");
                        }

                        if (isTracingEnabled) activity.LogInformation(description);
                    }
                }
            }

            UpdateJobStatus(jobStatus, description);

            if (isTracingEnabled) activity.Stop();
        }

        /// <summary>
        /// Update the job status.
        /// </summary>
        /// <param name="jobStatus"></param>
        private void UpdateJobStatus(JobStatus jobStatus, string description)
        {
            string message = string.Empty;
            Boolean isTracingEnabled = _traceSettings.IsBasicTracingEnabled;
            DiagnosticActivity activity = null;
            if (isTracingEnabled)
            {
                activity = new DiagnosticActivity();
                activity.Start();
            }

            job.JobStatus = jobStatus;
            job.Description = description;

            DateTime endTime = DateTime.Now;
            job.JobData.ExecutionStatus.EndTime = endTime.ToString();
            DateTime startTime = DateTime.Now;

            if (DateTime.TryParse(job.JobData.ExecutionStatus.StartTime, out startTime))
            {
                job.JobData.ExecutionStatus.TotalMilliSeconds = (endTime - startTime).TotalMilliseconds;
            }

            job.JobData.ExecutionStatus.StartTime = startTime.ToString();

            IImportProgressHandler importProgressHandler = null;
            if (importProfile.ProcessingSpecifications.ImportMode == ImportMode.RelationshipLoad ||
                importProfile.ProcessingSpecifications.ImportMode == ImportMode.RelationshipInitialLoad)
            {
                importProgressHandler = relationshipProgressHandler;
            }
            else
            {
                importProgressHandler = progressHandler;
            }

            job.JobData.ExecutionStatus.TotalElementsProcessed = importProgressHandler.GetCompletedEntities();
            job.JobData.ExecutionStatus.TotalElementsSucceed = importProgressHandler.GetSuccessFulEntities();
            job.JobData.ExecutionStatus.TotalElementsPartiallySucceed = importProgressHandler.GetPartialSuccessFulEntities();
            job.JobData.ExecutionStatus.TotalElementsFailed = importProgressHandler.GetFailedEntities();
            job.JobData.ExecutionStatus.TotalElementsUnChanged = importProgressHandler.GetUnChangedEntitiesCount();
            jobManager.Update(job, new CallerContext(application, Utility.GetModule(job.JobType), importProgram));

            if (isTracingEnabled) activity.Stop();
        }

        /// <summary>
        /// Records the run information in the database.
        /// </summary>
        /// <param name="totalTime"></param>
        /// <returns></returns>
        private bool RecordResults(double totalTime)
        {
            string message = string.Empty;
            Boolean isTracingEnabled = _traceSettings.IsBasicTracingEnabled;
            DiagnosticActivity activity = null;
            if (isTracingEnabled)
            {
                activity = new DiagnosticActivity();
                activity.Start();
            }

            IDbConnection connection = null;
            try
            {
                string connectionString = System.Configuration.ConfigurationManager.AppSettings["StagingConnectionString"];
                connection = new SqlConnection(connectionString);
                connection.Open();

                // Create the Command and Parameter objects.
                SqlCommand command = new SqlCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.Connection = (SqlConnection)connection;
                command.CommandText = "usp_Analysis_recordInitialLoadRunStatistics";
                command.CommandTimeout = 120;

                SqlParameter parameter = new SqlParameter("@EntityThreadsNum", System.Data.SqlDbType.Int);
                parameter.Value = numberOfEntitiesThreads;
                parameter.Direction = ParameterDirection.Input;
                command.Parameters.Add(parameter);

                parameter = new SqlParameter("@EntityCountNum", System.Data.SqlDbType.Int);
                parameter.Value = sourceData.GetEntityEndPoint() - sourceData.GetEntityDataSeed() + 1;
                parameter.Direction = ParameterDirection.Input;
                command.Parameters.Add(parameter);

                parameter = new SqlParameter("@EntityBatchSizeNum", System.Data.SqlDbType.Int);
                parameter.Value = sourceData.GetEntityDataBatchSize();
                parameter.Direction = ParameterDirection.Input;
                command.Parameters.Add(parameter);

                parameter = new SqlParameter("@AttributesPerEntityNum", System.Data.SqlDbType.Int);
                parameter.Value = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["AttributesPerEntityNum"]);
                parameter.Direction = ParameterDirection.Input;
                command.Parameters.Add(parameter);

                parameter = new SqlParameter("@AttributeThreadsNum", System.Data.SqlDbType.Int);
                parameter.Value = numberOfAttributesThreadPerEntity;
                parameter.Direction = ParameterDirection.Input;
                command.Parameters.Add(parameter);

                parameter = new SqlParameter("@TimeTakenSecondsNum", System.Data.SqlDbType.Int);
                parameter.Value = totalTime;
                parameter.Direction = ParameterDirection.Input;
                command.Parameters.Add(parameter);

                parameter = new SqlParameter("@PrimaryIndexFlag", System.Data.SqlDbType.Char);
                parameter.Value = System.Configuration.ConfigurationManager.AppSettings["PrimaryIndexFlag"];
                parameter.Direction = ParameterDirection.Input;
                command.Parameters.Add(parameter);

                parameter = new SqlParameter("@TableLockFlag", System.Data.SqlDbType.Char);
                parameter.Value = (Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["TableLockInBulkInsert"]) == true) ? "Y" : "N";
                parameter.Direction = ParameterDirection.Input;
                command.Parameters.Add(parameter);

                parameter = new SqlParameter("@NonClusteredIndexFlag", System.Data.SqlDbType.Char);
                parameter.Value = System.Configuration.ConfigurationManager.AppSettings["NonClusteredIndexFlag"];
                parameter.Direction = ParameterDirection.Input;
                command.Parameters.Add(parameter);

                parameter = new SqlParameter("@TriggerFlag", System.Data.SqlDbType.Char);
                parameter.Value = (Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["FireTriggersInBulkInsert"]) == true) ? "Y" : "N";
                parameter.Direction = ParameterDirection.Input;
                command.Parameters.Add(parameter);

                parameter = new SqlParameter("@DenormFlag", System.Data.SqlDbType.Char);
                parameter.Value = (Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["ProcessDenormTables"]) == true) ? "Y" : "N";
                parameter.Direction = ParameterDirection.Input;
                command.Parameters.Add(parameter);

                parameter = new SqlParameter("@ClietServerString", System.Data.SqlDbType.Char);
                parameter.Value = Environment.MachineName;
                parameter.Direction = ParameterDirection.Input;
                command.Parameters.Add(parameter);

                parameter = new SqlParameter("@StagingServerString", System.Data.SqlDbType.Char);
                parameter.Value = GetInformationFromConnectionString("StagingConnectionString", "Data Source");
                parameter.Direction = ParameterDirection.Input;
                command.Parameters.Add(parameter);

                parameter = new SqlParameter("@CoreServerString", System.Data.SqlDbType.Char);
                parameter.Value = GetInformationFromConnectionString("ConnectionString", "Data Source");
                parameter.Direction = ParameterDirection.Input;
                command.Parameters.Add(parameter);

                parameter = new SqlParameter("@CoreDBString", System.Data.SqlDbType.Char);
                parameter.Value = GetInformationFromConnectionString("ConnectionString", "Initial Catalog");
                parameter.Direction = ParameterDirection.Input;
                command.Parameters.Add(parameter);

                parameter = new SqlParameter("@DenormServerString", System.Data.SqlDbType.Char);
                parameter.Value = GetInformationFromConnectionString("DenormConnectionString", "Data Source");
                parameter.Direction = ParameterDirection.Input;
                command.Parameters.Add(parameter);

                parameter = new SqlParameter("@Comments", System.Data.SqlDbType.Char);
                parameter.Value = job.Description;
                parameter.Direction = ParameterDirection.Input;
                command.Parameters.Add(parameter);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string returnValue = reader["returnVal"].ToString();
                        Console.WriteLine(returnValue);
                        LogHandler.WriteInformationLog(returnValue, 0);
                    }
                }
            }
            catch (Exception ex)
            {
                message = string.Format("Recording the results failed with the exception {0}", ex.Message);

                if (isTracingEnabled)
                {
                    activity.LogError(message);
                }
                LogHandler.WriteErrorLog(message, 100);
                return false;
            }
            finally
            {
                connection.Close();
                if (isTracingEnabled) activity.Stop();
            }

            return true;
        }

        /// <summary>
        /// Parses the given connection string and gets the specified information from it. Uses string split methods.
        /// </summary>
        /// <param name="connectionStringName"></param>
        /// <param name="informationName"></param>
        /// <returns></returns>
        private string GetInformationFromConnectionString(string connectionStringName, string informationName)
        {
            string returnValue = string.Empty;

            string serverString = System.Configuration.ConfigurationManager.AppSettings[connectionStringName];

            string[] splitconnection = serverString.Split(';');

            foreach (string str in splitconnection)
            {
                if (str.Contains(informationName))
                {
                    string[] subSplit = str.Split('=');

                    if (subSplit.Count() == 2)
                    {
                        returnValue = subSplit[1];
                    }
                }
            }

            return returnValue;
        }

        #endregion

        #region Extension Relationship Object Management

        /// <summary>
        /// Fills and validate ExtensionRelatioship
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="entityOperationResult"></param>
        private void FillAndValidateExtensionRelationShip(Entity entity, EntityOperationResult entityOperationResult)
        {
            string message = string.Empty;
            Boolean isTracingEnabled = _traceSettings.IsBasicTracingEnabled;
            DiagnosticActivity activity = null;
            if (isTracingEnabled)
            {
                activity = new DiagnosticActivity();
                activity.Start();
            }

            // Are we even supposed to processing this?
            if (importProfile.ProcessingSpecifications.ImportMode != ImportMode.ExtensionRelationshipLoad && importProfile.ProcessingSpecifications.ImportMode != ImportMode.EntityExtensionRelationshipAndHierarchyLoad
                && importProfile.ProcessingSpecifications.ImportMode != ImportMode.InitialLoad)
            {
                if ((String.IsNullOrEmpty(entity.ParentExtensionEntityExternalId) == false) || (entity.ExtensionRelationships != null && entity.ExtensionRelationships.Count > 0))
                {
                    // Log a warning and exit. We should not be processing extension relationship unless the import mode is extension load. Otherwise we cannot 
                    // gaurantee the order and it will error out..
                    message = String.Format("Entity with external id {0} and container name {1} has extenstion relationship information that will NOT be processed as this is not a Extension Relationship Profile.", entity.ExternalId, entity.ContainerName);
                    message = String.Format("Job Id {0} - {1}", job.Id, message);

                    if (isTracingEnabled)
                        activity.LogInformation(message);

                    LogHandler.WriteWarningLog(message, 100);
                    return;
                }
            }

            // first do the parent at the entity level
            if (String.IsNullOrEmpty(entity.ParentExtensionEntityExternalId) == false)
            {
                FillExtensionRelationShip(entity);
                ValidateExtensionRelationShip(entity, entityOperationResult);
            }

            //then process all the extensions under entity
            foreach (ExtensionRelationship extensionRelation in entity.ExtensionRelationships)
            {
                FillExtensionRelationShip(entity, extensionRelation);
                ValidateExtensionRelationShip(entity, extensionRelation, entityOperationResult);
            }

            if (isTracingEnabled) activity.Stop();
        }

        /// <summary>
        /// Fills the extension parent information
        /// </summary>
        /// <param name="entity"></param>
        private void FillExtensionRelationShip(Entity entity)
        {
            string message = string.Empty;
            Boolean isTracingEnabled = _traceSettings.IsBasicTracingEnabled;
            DiagnosticActivity activity = null;
            if (isTracingEnabled)
            {
                activity = new DiagnosticActivity();
                activity.Start();
            }

            if (entity.ParentExtensionEntityContainerId <= 0)
            {
                Container catalog = GetContainerByName(entity.ParentExtensionEntityContainerName);
                if (catalog != null)
                {
                    entity.ParentExtensionEntityContainerId = catalog.Id;

                    if (String.IsNullOrWhiteSpace(entity.ParentExtensionEntityCategoryLongName))
                        entity.ParentExtensionEntityCategoryLongName = catalog.LongName;

                    if (isTracingEnabled)
                    {
                        message = "Extension Relationship: " + entity.ParentExtensionEntityExternalId + ". Found Container. Id = " + catalog.Id + " Name = " + catalog.Name;
                        activity.LogInformation(message);
                    }
                }
                else
                {
                    if (isTracingEnabled)
                    {
                        message = "Extension Relationship:  " + entity.ParentExtensionEntityExternalId + ". Could not find Container: " + entity.ParentExtensionEntityCategoryName;
                        activity.LogWarning(message);
                    }
                }
            }

            if (entity.ParentExtensionEntityCategoryId <= 0)
            {
                Category category = GetCategoryByPath(entity.ParentExtensionEntityCategoryPath, entity.ParentExtensionEntityContainerName);
                if (category != null)
                {
                    entity.ParentExtensionEntityCategoryId = category.Id;
                    entity.ParentExtensionEntityCategoryName = category.Name;
                    entity.ParentExtensionEntityCategoryLongName = category.LongName;

                    if (isTracingEnabled)
                    {
                        message = "Extension Relationship:  " + entity.ParentExtensionEntityExternalId + ". Found Category. Id = " + category.Id + " Name = " + category.Name;
                        activity.LogInformation(message);
                    }
                }
                else
                {
                    if (isTracingEnabled)
                    {
                        message = "Extension Relationship: " + entity.ParentExtensionEntityExternalId + ". Could not find Category: " + entity.ParentExtensionEntityCategoryPath;
                        activity.LogWarning(message);
                    }
                }
            }

            EntityMapBL entityMapManager = new EntityMapBL();

            EntityMap entityMap = entityMapManager.Get(systemId, entity.ParentExtensionEntityExternalId, entity.ParentExtensionEntityContainerId, entity.EntityTypeId, entity.ParentExtensionEntityCategoryId, importProfile.MappingSpecifications.EntityIdentificationMap, application, importmodule);

            if (entityMap != null)
            {
                if (isTracingEnabled)
                {
                    message = "Extension Relationship: " + entity.ParentExtensionEntityExternalId + ". Extension parent Entity found with internal id: " + entityMap.InternalId;
                    activity.LogInformation(message);
                }

                entity.ParentExtensionEntityContainerId = entityMap.ContainerId;
                entity.ParentExtensionEntityCategoryId = entityMap.CategoryId;

                if (entity.Action == ObjectAction.Create)
                {
                    entity.ParentExtensionEntityId = entityMap.InternalId;
                }
                else if (entity.Action == ObjectAction.Update)
                {
                    //If both entities are available then populate EntityMoveContext to create link between two existing entities..
                    EntityMoveContext entityMoveContext = new EntityMoveContext();
                    entityMoveContext.ReParentType = ReParentTypeEnum.ExtensionReParent;
                    entityMoveContext.TargetParentExtensionEntityId = entityMap.InternalId;

                    entity.EntityMoveContext = entityMoveContext;
                }
            }

            if (isTracingEnabled) activity.Stop();
        }

        /// <summary>
        /// Validate the extension parent.
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="entityOperationResult"></param>
        private void ValidateExtensionRelationShip(Entity entity, EntityOperationResult entityOperationResult)
        {
            string message = string.Empty;
            Boolean isTracingEnabled = _traceSettings.IsBasicTracingEnabled;
            DiagnosticActivity activity = null;
            if (isTracingEnabled)
            {
                activity = new DiagnosticActivity();
                activity.Start();
            }

            int numberOfFailures = 0;
            String errorMessage = String.Empty;

            if ((String.IsNullOrEmpty(entity.ParentExtensionEntityCategoryPath) == false) || (String.IsNullOrEmpty(entity.ParentExtensionEntityContainerName) == false) || (String.IsNullOrEmpty(entity.ParentExtensionEntityExternalId) == false))
            {
                if (entity.EntityMoveContext.ReParentType != ReParentTypeEnum.ExtensionReParent && entity.ParentExtensionEntityId <= 0)
                {
                    numberOfFailures++;
                    errorMessage = String.Format("Parent extension entity is not found. The container is {0}, category path is {1} and parent external id is {2}.", entity.ParentExtensionEntityContainerName, entity.ParentExtensionEntityCategoryPath, entity.ParentExtensionEntityExternalId);
                    entityOperationResult.AddOperationResult(String.Empty, errorMessage, OperationResultType.Error);
                }
                else
                {
                    //now verify if we have a valid parent extension id, make sure it is part of the INH path.
                    ValidateExtensionParent(entity, entityOperationResult);
                }

                if (isTracingEnabled)
                {
                    activity.LogWarning(errorMessage);
                }
            }

            if ((entity.ContainerId > 0) && (entity.ParentExtensionEntityContainerId == entity.ContainerId))
            {
                numberOfFailures++;
                errorMessage = String.Format("You cannot create an extension entity within the same container. The container is {0}.", entity.ContainerName);
                entityOperationResult.AddOperationResult(String.Empty, errorMessage, OperationResultType.Error);

                if (isTracingEnabled)
                {
                    activity.LogWarning(errorMessage);
                }
            }

            if (isTracingEnabled) activity.Stop();
        }

        /// <summary>
        /// Fill the missing information from cached metadata
        /// </summary>
        /// <param name="entity"></param>
        private void FillExtensionRelationShip(Entity entity, ExtensionRelationship extensionRelation)
        {
            string message = string.Empty;
            Boolean isTracingEnabled = _traceSettings.IsBasicTracingEnabled;
            DiagnosticActivity activity = null;
            if (isTracingEnabled)
            {
                activity = new DiagnosticActivity();
                activity.Start();
            }

            if (extensionRelation.ContainerId <= 0)
            {
                Container catalog = GetContainerByName(extensionRelation.ContainerName);
                if (catalog != null)
                {
                    extensionRelation.ContainerId = catalog.Id;

                    if (String.IsNullOrWhiteSpace(extensionRelation.ContainerLongName))
                        extensionRelation.ContainerLongName = catalog.LongName;

                    if (isTracingEnabled)
                    {
                        message = "Extension Relationship: " + extensionRelation.ExternalId + ". Found Container. Id = " + catalog.Id + " Name = " + catalog.Name;
                        activity.LogInformation(message);
                    }
                }
                else
                {
                    if (isTracingEnabled)
                    {
                        message = "Extension Relationship:  " + extensionRelation.ExternalId + ". Could not find Container: " + extensionRelation.ContainerName;
                        activity.LogWarning(message);
                    }
                }
            }

            if (extensionRelation.CategoryId <= 0)
            {
                Category category = GetCategoryByPath(extensionRelation.CategoryPath, extensionRelation.ContainerName);
                if (category != null)
                {
                    extensionRelation.CategoryId = category.Id;
                    extensionRelation.CategoryName = category.Name;
                    extensionRelation.CategoryLongName = category.LongName;

                    if (isTracingEnabled)
                    {
                        message = "Extension Relationship:  " + extensionRelation.ExternalId + ". Found Category. Id = " + category.Id + " Name = " + category.Name;
                        activity.LogInformation(message);
                    }
                }
                else
                {
                    if (isTracingEnabled)
                    {
                        message = "Extension Relationship: " + extensionRelation.ExternalId + ". Could not find Category: " + extensionRelation.CategoryPath;
                        activity.LogWarning(message);
                    }
                }
            }

            EntityMapBL entityMapManager = new EntityMapBL();

            EntityMap entityMap = entityMapManager.Get(systemId, extensionRelation.ExternalId, extensionRelation.ContainerId, entity.EntityTypeId, extensionRelation.CategoryId, importProfile.MappingSpecifications.EntityIdentificationMap, application, importmodule);

            if (entityMap != null)
            {
                extensionRelation.RelatedEntityId = entityMap.InternalId;
                extensionRelation.ContainerId = entityMap.ContainerId;
                extensionRelation.CategoryId = entityMap.CategoryId;

                // When the direction is up (parent)
                if (extensionRelation.Direction == RelationshipDirection.Up)
                {
                    entity.ParentExtensionEntityId = entityMap.InternalId;
                }

                if (isTracingEnabled)
                {
                    message = "Extension Relationship: " + extensionRelation.ExternalId + ". Extension parent Entity found with internal id: " + entityMap.InternalId;
                    activity.LogInformation(message);
                }
            }

            if (isTracingEnabled) activity.Stop();
        }

        /// <summary>
        /// Validate the extension relationship.
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="extensionRelation"></param>
        /// <param name="entityOperationResult"></param>
        private void ValidateExtensionRelationShip(Entity entity, ExtensionRelationship extensionRelation, EntityOperationResult entityOperationResult)
        {
            string message = string.Empty;
            Boolean isTracingEnabled = _traceSettings.IsBasicTracingEnabled;
            DiagnosticActivity activity = null;
            if (isTracingEnabled)
            {
                activity = new DiagnosticActivity();
                activity.Start();
            }

            int numberOfFailures = 0;
            String errorMessage = String.Empty;

            if (extensionRelation.ContainerId <= 0)
            {
                numberOfFailures++;
                errorMessage = String.Format("Container is empty or not valid. The container is {0}.", extensionRelation.ContainerName);
                entityOperationResult.AddOperationResult(String.Empty, errorMessage, OperationResultType.Error);

                if (isTracingEnabled)
                {
                    activity.LogWarning(errorMessage);
                }
            }

            if (extensionRelation.CategoryId <= 0)
            {
                numberOfFailures++;
                errorMessage = String.Format("CategoryPath is empty or not valid. The extension path is {0}.", extensionRelation.CategoryPath);
                entityOperationResult.AddOperationResult(String.Empty, errorMessage, OperationResultType.Error);

                if (isTracingEnabled)
                {
                    activity.LogWarning(errorMessage);
                }
            }

            if ((extensionRelation.ContainerId > 0) && (extensionRelation.ContainerId == entity.ContainerId))
            {
                numberOfFailures++;
                errorMessage = String.Format("You cannot create an extension entity within the same container. The container is {0}.", extensionRelation.ContainerName);
                entityOperationResult.AddOperationResult(String.Empty, errorMessage, OperationResultType.Error);

                if (isTracingEnabled)
                {
                    activity.LogWarning(errorMessage);
                }
            }

            if (extensionRelation.RelatedEntityId <= 0)
            {
                numberOfFailures++;
                errorMessage = String.Format("Extension entity is not valid. The extension entity external id is {0}.", extensionRelation.ExternalId);
                entityOperationResult.AddOperationResult(String.Empty, errorMessage, OperationResultType.Error);

                if (isTracingEnabled)
                {
                    activity.LogWarning(errorMessage);
                }
            }

            if (isTracingEnabled) activity.Stop();
        }

        /// <summary>
        /// Validate if the parent container is part of the inheritance 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="entityOperationResult"></param>
        private void ValidateExtensionParent(Entity entity, EntityOperationResult entityOperationResult)
        {
            string message = string.Empty;
            Boolean isTracingEnabled = _traceSettings.IsBasicTracingEnabled;
            DiagnosticActivity activity = null;
            if (isTracingEnabled)
            {
                activity = new DiagnosticActivity();
                activity.Start();
            }

            try
            {
                String errorMessage = String.Empty;
                Boolean parentFound = false;

                Container container = containers.First(c => c.Id == entity.ContainerId);

                if (container == null)
                {
                    errorMessage = String.Format("Entity extensions processing failed. Container {0} is not available.", entity.ContainerName);
                    entityOperationResult.AddOperationResult(String.Empty, errorMessage, OperationResultType.Error);

                    if (isTracingEnabled)
                    {
                        activity.LogError(errorMessage);
                    }
                    return;
                }

                Container parentExtensionInhPath = containers.First(c => c.Id == container.ParentContainerId);

                if (parentExtensionInhPath != null)
                {
                    if (parentExtensionInhPath.Id == entity.ParentExtensionEntityContainerId)
                    {
                        parentFound = true;
                    }

                }

                // Did we find a parent..
                if (!parentFound)
                {
                    errorMessage = String.Format("Entity extensions processing failed. The extension parent container {0} is not parent of the given entity container {1}.", entity.ParentExtensionEntityContainerName, entity.ContainerName);

                    entityOperationResult.AddOperationResult(String.Empty, errorMessage, OperationResultType.Error);

                    if (isTracingEnabled)
                    {
                        activity.LogError(errorMessage);
                    }
                    return;
                }
            }
            finally
            {
                if (isTracingEnabled) activity.Stop();
            }
        }
        #endregion

        #region Process Helper Method

        /// <summary>
        /// Helper method for processing entities using API
        /// </summary>
        /// <param name="entityCollection"></param>
        /// <returns>Returns EntityOperationResultCollection</returns>
        private EntityOperationResultCollection ProcessEntitiesUsingAPI(EntityCollection entityCollection, EntityCollection entitiesWithMatching, EntityCollection entitiesWithoutMatching)
        {
            EntityOperationResultCollection apiEntityOperationResultCollection = new EntityOperationResultCollection();
            String message;

            Boolean isTracingEnabled = _traceSettings.IsBasicTracingEnabled;
            DiagnosticActivity activity = null;
            if (isTracingEnabled)
            {
                activity = new DiagnosticActivity();
                activity.Start();
            }

            try
            {
                if (importProcessingType == ImportProcessingType.ValidateAndProcess)
                {
                    apiEntityOperationResultCollection = 
                        (EntityOperationResultCollection)dataService.ProcessEntities(entityCollection, entityProcessingOptions, this.callerContextWithJobId);
                }
                else if (importProcessingType == ImportProcessingType.ValidationOnly)
                {
                    apiEntityOperationResultCollection = PrepareEntityOperationResultsSchema(entityCollection);
                    
                    new EntityValidationBL().Validate(entityCollection, apiEntityOperationResultCollection,
                        new CallerContext(application, MDMCenterModules.Validation), entityProcessingOption: entityProcessingOptions);
                }
                else if (importProcessingType == ImportProcessingType.ValidateMatchAndMerge)
                {
                    if (entitiesWithoutMatching != null && entitiesWithoutMatching.Count > 0)
                    {
                        apiEntityOperationResultCollection =
                            (EntityOperationResultCollection)dataService.ProcessEntities(entitiesWithoutMatching, entityProcessingOptions, this.callerContextWithJobId);
                    }

                    if (entitiesWithMatching != null && entitiesWithMatching.Count > 0)
                    {
                        EntityOperationResultCollection matchOperationResults = PerformMatchAndMerge(entitiesWithMatching);
                        apiEntityOperationResultCollection.CopyEntityOperationResults(matchOperationResults, false);
                    }
                }

                if (isTracingEnabled)
                {
                    message = string.Format("Completed API {0} for {1} entities", importProcessingType.ToString(),
                        apiEntityOperationResultCollection.Count);
                    activity.LogInformation(message);
                }
            }
            finally
            {
                if (isTracingEnabled)
                {
                    activity.Stop();
                }
            }
            return apiEntityOperationResultCollection;
        }

        /// <summary>
        /// Helper method for processing entity using API
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>Returns EntityOperationResultCollection</returns>
        private EntityOperationResult ProcessEntityUsingAPI(Entity entity)
        {
            return ProcessEntitiesUsingAPI(new EntityCollection() { entity }, null, null).FirstOrDefault();
        }

        /// <summary>
        /// This method is copied from the entity BL method. The import engine validation feature was not working without this. This needs to be moved to 
        /// a common code.
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        private EntityOperationResultCollection PrepareEntityOperationResultsSchema(EntityCollection entities)
        {
            string message = string.Empty;
            Boolean isTracingEnabled = _traceSettings.IsBasicTracingEnabled;
            DiagnosticActivity activity = null;
            if (isTracingEnabled)
            {
                activity = new DiagnosticActivity();
                activity.Start();
            }

            EntityOperationResultCollection entityOperationResults = new EntityOperationResultCollection();
            Int64 entityIdToBeCreated = -1;

            foreach (Entity entity in entities)
            {
                EntityOperationResult entityOperationResult = new EntityOperationResult(entity.Id, entity.ExternalId, entity.LongName);

                if (entity.Id < 1)
                {
                    entity.Id = entityIdToBeCreated;
                    entityOperationResult.EntityId = entityIdToBeCreated;
                    entityIdToBeCreated--;
                }

                entityOperationResult.ReferenceId = entity.ReferenceId;
                entityOperationResult.PerformedAction = entity.Action;

                if (entity.Attributes != null && entity.Attributes.Count > 0)
                {
                    foreach (Attribute attr in entity.Attributes)
                    {
                        AttributeOperationResult attributeOperationResult = new AttributeOperationResult(attr.Id, attr.Name, attr.LongName, attr.AttributeModelType, attr.Locale);
                        entityOperationResult.AttributeOperationResultCollection.Add(attributeOperationResult);
                    }
                }

                if (entity.Relationships != null && entity.Relationships.Count > 0)
                {
                    //Declare relationship Id which will be incremented and assigned to each relationship operation result so that we can identify the operation result uniquely
                    Int32 relationshipId = 1;

                    entityOperationResult.RelationshipOperationResultCollection = PrepareRelationshipOperationResultsSchema(entity.Relationships, relationshipId);
                }

                // prepare entity context also otherwise validation will faill
                if (entity.EntityContext == null)
                    entity.EntityContext = new EntityContext();

                if (entity.EntityContext.DataLocales.Count < 1)
                {
                    if (entity.Attributes != null)
                    {
                        //Get distinct list of locales available in entity's attributes.
                        Collection<LocaleEnum> locales = entity.Attributes.GetLocaleList();
                        if (locales != null)
                        {
                            foreach (LocaleEnum locale in locales)
                            {
                                if (!entity.EntityContext.DataLocales.Contains(locale))
                                    entity.EntityContext.DataLocales.Add(locale);
                            }
                        }
                    }

                    if (entity.Relationships != null && entity.Relationships.Count > 0)
                    {
                        //Get distinct list of locales available in each of relationship attributes.
                        foreach (Relationship relation in entity.Relationships)
                        {
                            Collection<LocaleEnum> locales = relation.RelationshipAttributes.GetLocaleList();
                            if (locales != null)
                            {
                                foreach (LocaleEnum locale in locales)
                                {
                                    if (!entity.EntityContext.DataLocales.Contains(locale))
                                        entity.EntityContext.DataLocales.Add(locale);
                                }
                            }
                        }
                    }

                    //Set entity locale as data locale too..
                    if (entity.EntityContext.DataLocales == null || entity.EntityContext.DataLocales.Count < 1)
                    {
                        entity.EntityContext.DataLocales.Add(entity.Locale);
                    }
                }
                entityOperationResults.Add(entityOperationResult);
            }

            if (isTracingEnabled) activity.Stop();

            return entityOperationResults;
        }

        private RelationshipOperationResultCollection PrepareRelationshipOperationResultsSchema(RelationshipCollection relationships, Int32 relationshipId)
        {
            string message = string.Empty;
            Boolean isTracingEnabled = _traceSettings.IsBasicTracingEnabled;
            DiagnosticActivity activity = null;
            if (isTracingEnabled)
            {
                activity = new DiagnosticActivity();
                activity.Start();
            }

            RelationshipOperationResultCollection relationshipOperationResultCollection = new RelationshipOperationResultCollection();

            foreach (Relationship relationship in relationships)
            {
                relationship.Id = relationshipId;

                RelationshipOperationResult relationshipOperationResult = new RelationshipOperationResult(relationshipId, relationship.RelationshipExternalId, relationship.ToExternalId, relationship.RelationshipTypeName, relationship.ReferenceId);
                relationshipOperationResult.OperationResultStatus = OperationResultStatusEnum.Successful;

                if (relationship.RelationshipAttributes != null && relationship.RelationshipAttributes.Count > 0)
                {
                    foreach (Attribute attr in relationship.RelationshipAttributes)
                    {
                        AttributeOperationResult attributeOperationResult = new AttributeOperationResult(attr.Id, attr.Name, attr.LongName, attr.AttributeModelType, attr.Locale);
                        relationshipOperationResult.AttributeOperationResultCollection.Add(attributeOperationResult);
                    }
                }

                if (relationship.RelationshipCollection != null && relationship.RelationshipCollection.Count > 0)
                {
                    relationshipOperationResult.RelationshipOperationResultCollection = PrepareRelationshipOperationResultsSchema(relationship.RelationshipCollection, relationshipId);
                }

                relationshipOperationResultCollection.Add(relationshipOperationResult);
                relationshipId++;
            }

            if (isTracingEnabled) activity.Stop();

            return relationshipOperationResultCollection;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entities"></param>
        private void FillEntityIdWithReferenceId(EntityCollection entities)
        {
            // Check for all entities..
            foreach (Entity entity in entities)
            {
                // if the entity id is a negative number it indicates the entity is not valid in our system.
                // We should have a valid refernce number..For excel this will be line number. For staging this will the PK..for rsxml, we need to populate
                if (entity.Id < 0 && entity.ReferenceId > 0)
                {
                    // Set it as a negative number..so we dont have any conflict with real entity Id.
                    entity.Id = -entity.ReferenceId;
                }
            }
        }

        private void FillPartialProcessingOptionsFromProfile()
        {
            // If the attribute level processing is enabled..then fill our list from the profile. This will be passed to the EntityBL for processing.
            if (entityProcessingOptions.IsPartialAttributeprocessingEnabled || entityProcessingOptions.IsPartialRelationshipAttributeProcessingEnabled)
            {
                AttributeMapCollection attrMaps = this.importProfile.MappingSpecifications.AttributeMaps;
                foreach (AttributeMap attrMap in importProfile.MappingSpecifications.AttributeMaps)
                {
                    if (attrMap.AttributeTarget != null)
                    {
                        // if the attribute level property is set, we need to honor it..
                        if (attrMap.AttributeTarget.FailEntityOnError)
                        {
                            // We are combining common/tech attributes and relationship attributes..so check separately..
                            // Relationship attributes can be set at this level or at relationship types..
                            if (entityProcessingOptions.IsPartialAttributeprocessingEnabled)
                            {
                                if (attrMap.AttributeTarget.ModelType == AttributeModelType.Common
                                        || attrMap.AttributeTarget.ModelType == AttributeModelType.Category
                                        || attrMap.AttributeTarget.ModelType == AttributeModelType.System)
                                {
                                    if (!validationMustOnAttributeIdList.Contains(attrMap.AttributeTarget.Id))
                                    {
                                        validationMustOnAttributeIdList.Add(attrMap.AttributeTarget.Id);
                                    }
                                }
                            }

                            if (entityProcessingOptions.IsPartialRelationshipAttributeProcessingEnabled)
                            {
                                if (attrMap.AttributeTarget.ModelType == AttributeModelType.Relationship)
                                {
                                    if (!validationMustOnRelationshipAttributeIdList.Contains(attrMap.AttributeTarget.Id))
                                    {
                                        validationMustOnRelationshipAttributeIdList.Add(attrMap.AttributeTarget.Id);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            // if relationship types has partial processing defined..
            if (entityProcessingOptions.IsPartialRelationshipTypeProcessingEnabled)
            {
                RelationshipTypeMaps relTypeMaps = this.importProfile.MappingSpecifications.RelationshipTypeMaps;
                if (relTypeMaps != null && relTypeMaps.Count() > 0)
                {
                    foreach (RelationshipTypeMap relType in relTypeMaps)
                    {
                        // Is failure defined at this level?
                        if (relType.FailEntityOnError)
                        {
                            // The profile has name. We need the id
                            RelationshipType relationshipType = GetRelationshipTypeByName(relType.Name);
                            if (relationshipType != null)
                            {
                                if (!validationMustOnRelationshipTypeIdList.Contains(relationshipType.Id))
                                {
                                    validationMustOnRelationshipTypeIdList.Add(relationshipType.Id);
                                }
                            }
                        }

                        if (entityProcessingOptions.IsPartialRelationshipAttributeProcessingEnabled)
                        {
                            if (relType.AttributeMapCollection != null)
                            {
                                foreach (AttributeMap attributemap in relType.AttributeMapCollection)
                                {
                                    if (attributemap.AttributeTarget.FailEntityOnError)
                                    {
                                        if (attributemap.AttributeTarget.ModelType == AttributeModelType.Relationship)
                                        {
                                            if (!validationMustOnRelationshipAttributeIdList.Contains(attributemap.AttributeTarget.Id))
                                            {
                                                validationMustOnRelationshipAttributeIdList.Add(attributemap.AttributeTarget.Id);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            // fill it back in the entity processing option..
            if (validationMustOnAttributeIdList != null && validationMustOnAttributeIdList.Count > 0)
            {
                entityProcessingOptions.ValidationMustOnAttributeIdList = validationMustOnAttributeIdList;
            }
            if (validationMustOnRelationshipAttributeIdList != null && validationMustOnRelationshipAttributeIdList.Count > 0)
            {
                entityProcessingOptions.ValidationMustOnRelationshipAttributeIdList = validationMustOnRelationshipAttributeIdList;
            }
            if (validationMustOnRelationshipTypeIdList != null && validationMustOnRelationshipTypeIdList.Count > 0)
            {
                entityProcessingOptions.ValidationMustOnRelationshipTypeIdList = validationMustOnRelationshipTypeIdList;
            }
        }

        /// <summary>
        /// Gets an entity list from the given comma separated list of reference numbers
        /// </summary>
        /// <param name="entities">Entity Collecoitn</param>
        /// <param name="referenceParameterList">Comma sepearated list of staging refernce ids</param>
        /// <returns></returns>
        private Collection<Int64> GetEntityIdList(EntityCollection entities, String referenceParameterList)
        {
            #region Parameter validation
            // the data will be of the form '1234', '1235', '1236'
            if (String.IsNullOrEmpty(referenceParameterList))
            {
                throw new Exception("Reference paraemter is null or empty. Cannot get EntityIds");
            }

            #endregion Parameter validation

            Collection<Int64> entityIdList = new Collection<Int64>();

            String[] referenceParameter = referenceParameterList.Split(',');

            foreach (String referenceParam in referenceParameter)
            {
                // Remove the single quotes which will be the first and last character
                String referenceId = referenceParam.Trim().Remove(0, 1);
                referenceId = referenceId.Remove(referenceId.Length - 1, 1);

                Entity entity = entities.GetEntityByReferenceId(Convert.ToInt64(referenceId));

                if (entityIdList != null)
                {
                    entityIdList.Add(entity.Id);
                }
            }

            return entityIdList;
        }

        private void ReProcessRelationshipsWithWarnings(EntityCollection entities, EntityOperationResultCollection apiEntityOperationResultCollection, Dictionary<Int64, Dictionary<RelationshipCollection, RelationshipOperationResultCollection>> relationshipsWaitingForToEntityCreation)
        {
            //Logical Flow
            //Step : 1 If entity got processed successfully, Validate the relationships again which are waiting for retry
            //Step : 2 If Relationship validation get passed, attach with entity and send for process
            //STep : 3 If Relationship validation get failed, attach RelationshipOperationResult in EntityOperationResult 

            if (relationshipsWaitingForToEntityCreation != null && relationshipsWaitingForToEntityCreation.Count > 0)
            {
                EntityCollection entitiesForRetry = new EntityCollection();
                EntityOperationResultCollection entityORForRetry = new EntityOperationResultCollection();

                foreach (KeyValuePair<Int64, Dictionary<RelationshipCollection, RelationshipOperationResultCollection>> relationshipsForEntity in relationshipsWaitingForToEntityCreation)
                {
                    Entity entity = entities.GetEntityByReferenceId(relationshipsForEntity.Key);

                    if (entity != null)
                    {
                        EntityOperationResult entityOperationResult = apiEntityOperationResultCollection.GetEntityOperationResult(entity.Id);
                        Dictionary<RelationshipCollection, RelationshipOperationResultCollection> values = relationshipsForEntity.Value;

                        if (entityOperationResult.OperationResultStatus != OperationResultStatusEnum.Failed || entityOperationResult.OperationResultStatus != OperationResultStatusEnum.CompletedWithErrors)
                        {
                            //Entity Processed successfuly, relationship creation retry is possible 
                            RelationshipCollection relationshipsToRetry = values.FirstOrDefault().Key;

                            if (relationshipsToRetry.Count > 0)
                            {
                                Boolean removeBadRelationshipAttributesFromProcessing = false;

                                // partial process of relationship attributes is ONLY supported in relationship load mode ( from staging database actually).
                                if (importProfile.ProcessingSpecifications.ImportMode == ImportMode.RelationshipLoad)
                                {
                                    removeBadRelationshipAttributesFromProcessing = true;
                                }

                                RelationshipOperationResultCollection errorRORCollection = FillandValidateRelationships(relationshipsToRetry, entity.Id, entity.ExternalId, entity.ContainerId, entity.ReferenceId, true, removeBadRelationshipAttributesFromProcessing, entity, ref relationshipsWaitingForToEntityCreation);

                                if (errorRORCollection != null && errorRORCollection.Count > 0)
                                {
                                    //Again some of the relationships validation got warnings
                                    //Attached Operation result into EOR
                                    entityOperationResult.RelationshipOperationResultCollection.AddRange(errorRORCollection);
                                    entityOperationResult.RelationshipOperationResultCollection.RefreshOperationResultStatus();
                                    entityOperationResult.RefreshOperationResultStatus();
                                }

                                if (relationshipsToRetry.Count > 0)
                                {
                                    if (entity.Relationships == null)
                                    {
                                        entity.Relationships = new RelationshipCollection();
                                    }

                                    entity.Relationships.AddRange(relationshipsToRetry);
                                    entity.Action = ObjectAction.Update;
                                    entitiesForRetry.Add(entity);
                                }
                            }
                        }
                        else
                        {
                            //Add RelationshipOperationResult with warnings in entityOperationResult as retry is not posssible
                            entityOperationResult.RelationshipOperationResultCollection.AddRange(values.FirstOrDefault().Value);
                            entityOperationResult.RefreshOperationResultStatus();
                        }
                    }
                }

                if (entitiesForRetry.Count > 0)
                {
                    entityORForRetry = (EntityOperationResultCollection)dataService.ProcessEntities(entitiesForRetry, entityProcessingOptions, this.callerContextWithJobId);

                    //Merge RelationshipOperationResults with apiEntityOperationResultCollection
                    //if Retry is successful, remove entityId from warningEntityIdList

                    foreach (Entity entity in entitiesForRetry)
                    {
                        EntityOperationResult originalEntityOperationResult = apiEntityOperationResultCollection.GetEntityOperationResult(entity.Id);
                        EntityOperationResult entityOperationResult = entityORForRetry.GetEntityOperationResult(entity.Id);

                        originalEntityOperationResult.RelationshipOperationResultCollection.AddRange(entityOperationResult.RelationshipOperationResultCollection);
                        originalEntityOperationResult.RefreshOperationResultStatus();
                    }
                }

                apiEntityOperationResultCollection.RefreshOperationResultStatus();
            }
        }

        #endregion

        #region Import source operations

        private void SetImportSource()
        {
            if (this.importProfile.InputSpecifications != null)
            {
                if (this.importProfile.InputSpecifications.SourceId.HasValue)
                {
                    Int32 sourceId = this.importProfile.InputSpecifications.SourceId.Value;

                    if (sourceId < 1)
                    {
                        this.importProfile.InputSpecifications.SourceId = (int)SystemSource.Unknown;
                    }
                }
                else
                {
                    this.importProfile.InputSpecifications.SourceId = (int)SystemSource.Unknown;
                }
            }
        }

        #endregion

        #region Update Relationship Operation Result

        /// <summary>
        /// Update Relationship operation result with exception occurred
        /// </summary>
        /// <param name="entities"></param>Collection of entitiesparam>
        /// <param name="entityOperationResults">Collection of entity operation results</param>
        /// <param name="exception">Exception occurred</param>
        private void UpdateRelationshipOperationResults(EntityCollection entities, EntityOperationResultCollection entityOperationResults, Exception exception)
        {
            if (entities != null)
            {
                foreach (Entity entity in entities)
                {
                    IEntityOperationResult iEntityOperationResult = entityOperationResults.GetByReferenceId(entity.Id);
                    EntityOperationResult entityOperationResult = null;

                    if (iEntityOperationResult != null)
                    {
                        entityOperationResult = (EntityOperationResult)iEntityOperationResult;
                    }

                    if (entityOperationResult == null)
                    {
                        entityOperationResult = new EntityOperationResult(entity.Id, entity.ReferenceId, entity.LongName, entity.ExternalId);

                        foreach (Relationship relationship in entity.Relationships)
                        {
                            RelationshipOperationResult relationshipOperationResult = new RelationshipOperationResult(relationship.Id, relationship.RelationshipExternalId, relationship.ToExternalId, relationship.RelationshipTypeName, relationship.ReferenceId);
                            relationshipOperationResult.AddOperationResult(String.Empty, exception.Message, OperationResultType.Error);
                            entityOperationResult.RelationshipOperationResultCollection.Add(relationshipOperationResult);
                        }

                        entityOperationResults.Add(entityOperationResult);
                    }
                    else
                    {
                        foreach (Relationship relationship in entity.Relationships)
                        {
                            RelationshipOperationResult relationshipOperationResult = entityOperationResult.RelationshipOperationResultCollection.Where(ror => ror.RelationshipId == relationship.Id).FirstOrDefault();

                            if (relationshipOperationResult == null)
                            {
                                relationshipOperationResult = new RelationshipOperationResult(relationship.Id, relationship.RelationshipExternalId, relationship.ToExternalId, relationship.RelationshipTypeName, relationship.ReferenceId);
                                entityOperationResult.RelationshipOperationResultCollection.Add(relationshipOperationResult);
                            }

                            relationshipOperationResult.AddOperationResult(String.Empty, exception.Message, OperationResultType.Error);
                        }
                    }
                }
            }

            entityOperationResults.RefreshOperationResultStatus();
        }

        #endregion Update Relationship Operation Result

        #region Cleansing Related Methods

        private void CleanseInvalidComplexAttributes(Entity entity)
        {
            if (entity.Attributes != null)
            {
                AttributeCollection attrCollection = new AttributeCollection();
                // the id of the value object is not managed internally..If we get it from the caller, we need to reset it. 
                foreach (Attribute attribute in entity.Attributes)
                {

                    if (attribute.IsComplex && IsBlankComplexAttribute(attribute))
                    {
                        attrCollection.Add(attribute);
                    }
                }

                if (attrCollection.Count > 0)
                {
                    foreach (Attribute attribute in attrCollection)
                    {
                        entity.Attributes.Remove(attribute);
                    }
                }
            }
        }

        private Boolean IsBlankComplexAttribute(Attribute attribute)
        {
            Boolean isBlank = true;

            if (attribute.Attributes != null && attribute.Attributes.Count > 0 // Level 2
                 && attribute.Attributes.FirstOrDefault().Attributes != null && attribute.Attributes.FirstOrDefault().Attributes.Count > 0) // Level 3
            {
                isBlank = false;
            }
            return isBlank;
        }

        private void CleanseEntities(EntityCollection entities)
        {
            foreach (var entity in entities)
            {
                if (entity.OrganizationId > 0)
                    entity.OrganizationId = -1;
                if (entity.ContainerId > 0)
                    entity.ContainerId = -1;
                if (entity.EntityTypeId > 0)
                    entity.EntityTypeId = -1;
                if (entity.CategoryId > 0)
                    entity.CategoryId = -1;
                if (entity.Id > 0)
                    entity.Id = -1;
                if (entity.ParentEntityId > 0)
                    entity.ParentEntityId = -1;
                if (entity.ParentEntityTypeId > 0)
                    entity.ParentEntityTypeId = -1;
                if (entity.ParentExtensionEntityCategoryId > 0)
                    entity.ParentExtensionEntityCategoryId = -1;
                if (entity.ParentExtensionEntityContainerId > 0)
                    entity.ParentExtensionEntityContainerId = -1;
                if (entity.ParentExtensionEntityId > 0)
                    entity.ParentExtensionEntityId = -1;
            }
        }

        private void CleanseAttributes(AttributeCollection attributes)
        {
            foreach (var attribute in attributes)
            {
                CleanseAttribute(attribute);
            }
        }

        private void CleanseAttribute(Attribute attribute)
        {
            if (attribute.Id > 0)
                attribute.Id = -1;
            if (attribute.AttributeParentId > 0)
                attribute.AttributeParentId = -1;
            foreach (Value value in attribute.CurrentValues)
            {
                if (value.Id > 0)
                {
                    value.Id = -1;
                    value.ValueRefId = -1;
                    value.UomId = -1;
                }
            }

            if (attribute.Attributes != null && attribute.Attributes.Count > 0)
            {
                CleanseAttributes(attribute.Attributes);
            }
        }

        private void CleanseRelationship(Relationship relationship)
        {
            if (relationship.Id > 0)
                relationship.Id = 0;
            if (relationship.RelationshipTypeId > 0)
                relationship.RelationshipTypeId = 0;
            if (relationship.ContainerId > 0)
                relationship.ContainerId = 0;
            if (relationship.FromEntityId > 0)
                relationship.FromEntityId = 0;
        }

        #endregion

        #region Match Related Methods

        /// <summary>
        /// Performs the matching.
        /// </summary>
        /// <param name="entityCollection">The entity collection.</param>
        /// <returns></returns>
        private EntityOperationResultCollection PerformMatchAndMerge(EntityCollection entityCollection)
        {
            Boolean isTracingEnabled = _traceSettings.IsBasicTracingEnabled;
            DiagnosticActivity activity = null;
            if (isTracingEnabled)
            {
                activity = new DiagnosticActivity();
                activity.Start();
            }
            
            EntityOperationResultCollection matchEntityOperationResultCollection = null;

            try
            {
                #region Perform the Match
                
                // Here the entity may not be available..we need to assign the reference id as entity id so the match result can be saved with a valid Id.
                FillEntityIdWithReferenceId(entityCollection);

               // matchEntityOperationResultCollection = matchingService.MatchAndMerge(entityCollection, job.Id, matchJobId, this.callerContextWithJobId);
                
                #endregion
            }
            catch (Exception e)
            {
                activity = activity ?? new DiagnosticActivity();
                activity.LogError(e.Message);

                matchEntityOperationResultCollection = matchEntityOperationResultCollection ?? new EntityOperationResultCollection();
                matchEntityOperationResultCollection.AddOperationResult("", String.Format("The following exception has occurred while sending entities to Match Service: {0}", e.Message), OperationResultType.Error);
            }
            finally
            {
                if (isTracingEnabled)
                {
                    activity.Stop();
                }
            }

            return matchEntityOperationResultCollection;
        }
        
        #endregion

        #region Diagnostics Related Methods

        /// <summary>
        /// Refreshes Dagnostic trace settings based at runtime.
        /// Tracing can be truned on or off by user from JobDetails page.
        /// </summary>
        private void RefreshDiagnosticsOption()
        {
            if (!_wasOperationTracingInitiatedByUser)
            {
                // Check for CacheKey
                String cacheKey = CacheKeyGenerator.GetRuntimeDiagnosticsJobIdCacheKey();
                String cacheResult = (String)_distributedCacheManager.Get(cacheKey);

                Int32 jobIdForDiagnostics;

                if (!String.IsNullOrEmpty(cacheResult) && Int32.TryParse(cacheResult, out jobIdForDiagnostics))
                {
                    if (jobIdForDiagnostics == job.Id)
                    {
                        if (!_isRuntimeDiagnosticsOn)
                        {
                            if (!_wasRuntimeDiagnosticsEverOn)
                            {
                                _wasRuntimeDiagnosticsEverOn = true;
                                _isRuntimeDiagnosticsOn = true;
                            }

                            _traceSettings.UpdateSettings(true, TracingMode.OperationTracing, TracingLevel.Basic);
                        }
                    }
                    else
                    {
                        if (_wasRuntimeDiagnosticsEverOn && _isRuntimeDiagnosticsOn)
                        {
                            _isRuntimeDiagnosticsOn = false;
                        }

                        // Leave the traceEnabled flag to true as user may enable again.
                        // Set tracing mode to none to pause storing the traces 
                        _traceSettings.UpdateSettings(true, TracingMode.None, TracingLevel.Basic);
                    }
                }
            }
        }

        private Worksheet ReadExcelData(OpenXml.Packaging.SpreadsheetDocument document, JobStatus finalStatus, String excelSheetName)
        {
            var foregroundColor = new ForegroundColor
            {
                Rgb = new OpenXml.HexBinaryValue(RSExcelConstants.DefaultErroredForeground)
            };

            var workBookPart = document.WorkbookPart;
            var workSheet = OpenSpreadsheetUtility.GetWorksheet(workBookPart, excelSheetName);

            if (workSheet != null)
            {
                var sheetData = workSheet.GetFirstChild<OpenXml.Spreadsheet.SheetData>();

                // Append new fill
                OpenSpreadsheetUtility.AppendFill(workBookPart.WorkbookStylesPart.Stylesheet, foregroundColor);

                // Append new cell format
                var cellFormatId = OpenSpreadsheetUtility.AppendErrorCellFormat(workBookPart.WorkbookStylesPart.Stylesheet);

                workBookPart.WorkbookStylesPart.Stylesheet.Save();

                // Process errored rows
                var importedRows = ProcessRows(workBookPart, sheetData, cellFormatId, finalStatus, excelSheetName, workSheet);

                // Delete imported rows
                OpenSpreadsheetUtility.RemoveRows(sheetData, importedRows);
                workSheet.Save();
            }

            return workSheet;
        }

        private void AddErrorAndWarningForExcelRow(OpenXml.Spreadsheet.Row row, SheetData sheetData, String error, UInt32 errorsColumnIndex, String warning, UInt32 warningsColumnIndex, uint cellFormatId)
        {
            Int32 cellsInRowCount = row.Count();

            // Removing old value cells
            OpenSpreadsheetUtility.RemoveDataCell(sheetData, errorsColumnIndex, row.RowIndex);
            OpenSpreadsheetUtility.RemoveDataCell(sheetData, warningsColumnIndex, row.RowIndex);

            // Removing redundant cells from row
            if (cellsInRowCount > warningsColumnIndex)
            {
                for (UInt32 i = warningsColumnIndex + 1; i <= cellsInRowCount; i++)
                {
                    var cell = OpenSpreadsheetUtility.GetDataCell(sheetData, i, row.RowIndex);
                    if (cell != null)
                    {
                        cell.Remove();
                    }
                }
            }

            if (!String.IsNullOrWhiteSpace(error))
            {
                OpenSpreadsheetUtility.AppendRowWithTextCell(row, errorsColumnIndex, error, OpenXml.SpaceProcessingModeValues.Preserve, cellFormatId);
            }
            if (!String.IsNullOrWhiteSpace(warning))
            {
                OpenSpreadsheetUtility.AppendRowWithTextCell(row, warningsColumnIndex, warning, OpenXml.SpaceProcessingModeValues.Preserve, cellFormatId);
            }
        }

        #endregion

        #endregion

        #endregion
    }
}