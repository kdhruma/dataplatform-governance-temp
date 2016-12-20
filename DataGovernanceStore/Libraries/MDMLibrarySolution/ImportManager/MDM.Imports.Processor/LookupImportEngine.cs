using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Text;

namespace MDM.Imports.Processor
{
    using MDM.BusinessObjects;
    using MDM.BusinessObjects.Imports;
    using MDM.BusinessObjects.Jobs;
    using MDM.Core;
    using MDM.Core.Exceptions;
    using MDM.ExceptionManager.Handlers;
    using MDM.Imports.Interfaces;
    using MDM.Interfaces;
    using MDM.JobManager.Business;
    using MDM.LookupManager.Business;
    using MDM.Services;
    using MDM.Utility;

    /// <summary>
    /// Specifies the Lookup Import Engine class
    /// </summary>
    public class LookupImportEngine : ILookupImportEngine
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

        private LookupImportProfile _lookupImportProfile = null;

        private ILookupImportSourceData _sourceData = null;

        private ImportSourceType _importSourceType = ImportSourceType.UnKnown;

        private JobImportResultHandler _jobImportResultHandler = null;

        Dictionary<String, LookupImportProgressHandler> _jobResultHandlerList = null;

        private static Object _lockObject = new Object();

        #endregion

        #region Local configuration values

        // How many parallel tasks we want for lookup creation.
        private int _numberOfLookupsThreads = 1;   // default value..

        // How many parallel task we want for rows creation PER lookup.
        private int _numberOfRecordThreadsPerLookup = 1; // default value...

        // batch size per lookup record thread.
        private int _lookupRecordBatchSize = 1;

        private Dictionary<Int32, Int32> _lookupSplit = null;

        private Object syncObject = null;

        #endregion

        #region Other local Variables and object

        private String importUser = SecurityPrincipalHelper.GetCurrentSecurityPrincipal().CurrentUserName;
        private String importProgram = "LookupImportEngine";
        private IImportProgressHandler _progressHandler = new ImportProgressHandler();
        private MDMCenterApplication _application = MDMCenterApplication.JobService;
        private MDMCenterModules _module = MDMCenterModules.Import;
        private ICallerContext _iCallerContext = MDMObjectFactory.GetICallerContext(MDMCenterApplication.JobService, MDMCenterModules.Import, "LookupImportEngine");
        private DataModelService _dataModelService = null;
        JobImportResultBL _jobImportResultBL = null;

        private LookupBL _lookupBL = new LookupBL();
        private ConcurrentDictionary<String, Lookup> _lookupsProcessedDictionary = new ConcurrentDictionary<String, Lookup>();
        private LookupCollection _erroredLookups = null;
        private Dictionary<Lookup, Collection<String>> _invalidLookupUniqueColumnsDictionary = new Dictionary<Lookup, Collection<String>>();
        private Dictionary<Lookup, Collection<String>> _invalidLookupNonUniqueColumnsDictionary = new Dictionary<Lookup, Collection<String>>();
        private Collection<String> _missingOrInvalidLookupNames = null;
        
        #endregion

        #endregion Private Fields

        #region Constructors

        public LookupImportEngine()
        {
        }

        #endregion

        #region Properties

        #endregion

        #region Public Methods

        #region IImportEngine Members

        /// <summary>
        /// Initialize Job
        /// </summary>
        /// <param name="job"></param>
        /// <param name="lookupImportProfile"></param>
        /// <returns></returns>
        public Boolean Initialize(Job job, LookupImportProfile lookupImportProfile)
        {
            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StartTraceActivity("LookupImportEngine.Initialize", MDMTraceSource.LookupImport, true);

            Boolean successFlag = true;

            if (job == null)
            {
                throw new ArgumentNullException("Job");
            }

            if (lookupImportProfile == null)
            {
                throw new ArgumentNullException("LookupImportProfile");
            }

            this._job = job;

            this._lookupImportProfile = lookupImportProfile;

            CheckAndCreateEventLogHandler();

            _dataModelService = new DataModelService(WCFClientConfiguration.GetConfiguration(MDMWCFServiceList.DataModelService, job.CreatedUser));

            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StopTraceActivity("LookupImportEngine.Initialize", MDMTraceSource.LookupImport);

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
        public Boolean RunStep(String stepName, StepConfiguration stepConfiguration, ILookupImportSourceData source)
        {
            if (Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.StartTraceActivity("LookupImportEngine.RunStep", MDMTraceSource.LookupImport, true);
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "LookupImportEngine.RunStep starting for JobId: " + _job.Id + " Step: " + stepName, MDMTraceSource.LookupImport);
            }

            EventLogHandler elog = new EventLogHandler();
            elog.WriteInformationLog("LookupImportEngine.RunStep", 100);

            Boolean successFlag = true;

            ILookupImportSourceData sourceData = (ILookupImportSourceData)source;

            #region Parameter validation

            if (stepConfiguration == null)
            {
                throw new ArgumentNullException(String.Format("LookupImportEngine: JobId: {0], RunStep: {1}. Error: stepConfiguration is null", _job.Id, stepName));
            }

            if (sourceData == null)
            {
                throw new ArgumentNullException(String.Format("LookupImportEngine: JobId: {0], RunStep: {1}. Error: sourceData is null", _job.Id, stepName));
            }

            #endregion

            #region Set instance variables

            this._sourceData = sourceData;

            this._importSourceType = _lookupImportProfile.InputSpecifications.Reader;

            this._jobImportResultHandler = new JobImportResultHandler(_job.Id);

            _jobImportResultHandler.ProgramName = importProgram;
            _jobImportResultHandler.UserName = importUser;

            _jobResultHandlerList = new Dictionary<String, LookupImportProgressHandler>();
            #endregion

            #region Initialize Parameters

            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Initialize Parameters starting... ", MDMTraceSource.LookupImport);

            // Initialize local configuration from app config file
            InitializeThreadCounts();

            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Initialize Parameters complete", MDMTraceSource.LookupImport);

            #endregion

            #region Process

            Int64 startTime = DateTime.Now.Ticks;

            String startMessage = String.Format("Lookup import engine started process for job {0}.", _job.Id.ToString());

            LogHandler.WriteInformationLog(startMessage, 0);

            Process();

            Int64 endWorkTime = DateTime.Now.Ticks;
            totalTime = new TimeSpan(endWorkTime - startTime).TotalSeconds;

            #endregion

            if (_job.JobStatus == JobStatus.Running)
            {
                UpdateJobStatus(JobStatus.Completed, "Lookup import job completed.");
            }

            String endMessage = String.Format("Lookup import engine completed processing for job Id : {0}. The process took {1} seconds.", _job.Id.ToString(), totalTime);
            LogHandler.WriteInformationLog(endMessage, 0);

            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StopTraceActivity("LookupImportEngine.RunStep", MDMTraceSource.LookupImport);

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
        public LookupImportProfile GetCurrentImportProfile()
        {
            return _lookupImportProfile;
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
        /// Splits the given range of lookups across the available number of lookup threads. Any spill over is assigned to the last thread.
        /// </summary>
        /// <param name="numberOfThreads"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="lookupbatchPerThread"></param>
        /// <returns>Dictionary containing the start number for each thread. The end number is the start number of the next thread. So the dictionary
        /// will have one more entry than the number of threads.</returns>
        private Dictionary<int, Int32> SplitLookupsForThreads(int numberOfThreads, Int32 start, Int32 end, Int32 lookupbatchPerThread)
        {
            Dictionary<int, Int32> threadBatchStart = new Dictionary<int, Int32>(numberOfThreads + 1);

            for (int i = 0; i < numberOfThreads; i++)
            {
                threadBatchStart[i] = start + i * lookupbatchPerThread;
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
            this._numberOfLookupsThreads = _lookupImportProfile.LookupJobProcessingOptions.NumberofLookupThreads;

            if (this._numberOfLookupsThreads <= 0)
            {
                this._numberOfLookupsThreads = 1;
            }

            this._numberOfRecordThreadsPerLookup = _lookupImportProfile.LookupJobProcessingOptions.NumberofLookupRecordThreadsPerLookupThread;

            if (this._numberOfRecordThreadsPerLookup <= 0)
            {
                this._numberOfRecordThreadsPerLookup = 1;
            }

            this._lookupRecordBatchSize = _lookupImportProfile.LookupJobProcessingOptions.BatchSize;

            if (_lookupRecordBatchSize <= 0)
            {
                this._lookupRecordBatchSize = 1;
            }
        }

        #endregion

        #region Step 1: Import Engine Process Method

        /// <summary>
        /// Gets source data from the data provider and processes lookup rows
        /// </summary>
        private void Process()
        {
            if (Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.StartTraceActivity("LookupImportEngine.Process", MDMTraceSource.LookupImport, false);
            }

            LookupCollection lookups = null;

            try
            {
                lookups = _sourceData.GetAllLookupTables(_application, _module);

                OperationResult dataSourceOperationResult = (OperationResult)_sourceData.GetOperationResult();

                if (dataSourceOperationResult != null && (dataSourceOperationResult.HasWarnings || dataSourceOperationResult.HasError))
                {
                    if (dataSourceOperationResult.HasError)
                    {
                        _job.JobData.OperationResult.Errors.AddRange(dataSourceOperationResult.GetErrors());
                    }
                    if (dataSourceOperationResult.HasWarnings)
                    {
                        _job.JobData.OperationResult.Warnings.AddRange(dataSourceOperationResult.GetWarnings());
                    }
                }

                LocaleEnum systemDataLocale = GlobalizationHelper.GetSystemDataLocale();
                LookupCollection sdlLookups = new LookupCollection();
                Dictionary<LocaleEnum, LookupCollection> nonSdlLookupDictionary = new Dictionary<LocaleEnum, LookupCollection>();
                LookupImportProgressHandler handler = null;

                if (lookups != null && lookups.Count > 0)
                {
                    Collection<String> errorLookupNames = this.FillAndValidateLookups(lookups);

                    foreach (Lookup lookup in lookups)
                    {
                        if (lookup != null && lookup.Rows != null && lookup.Rows.Count > 0)
                        {
                            Int32 noOfLookupRows = lookup.Rows.Count;

                            if (errorLookupNames.Contains(lookup.Name))
                            {
                                // If any of the lookup failed with validation, skip from the process and update the job result.
                                if (!_jobResultHandlerList.ContainsKey(lookup.Name))
                                {
                                    handler = new LookupImportProgressHandler();
                                    //Update the handler to match the lookup and lookup record count for the job result.
                                    handler.UpdateCompletedLookupBatch(successfulLookupRecords: 0, failedLookupRecords: noOfLookupRows - 1);    // Actual error will be update as part of 'Table' level errors processed.
                                    handler.AddLocale(lookup.Locale);
                                    handler.SetTotalLookupRecords(noOfLookupRows);
                                    _jobResultHandlerList.Add(lookup.Name, handler);
                                }

                                continue;   //Lookup does not have the unique column so no need to process the current lookup.
                            }

                            if (systemDataLocale == lookup.Locale)
                            {
                                sdlLookups.Add(lookup);
                            }
                            else
                            {
                                if (nonSdlLookupDictionary.ContainsKey(lookup.Locale))
                                {
                                    LookupCollection tempLookup = nonSdlLookupDictionary[lookup.Locale];
                                    tempLookup.Add(lookup);

                                    nonSdlLookupDictionary[lookup.Locale] = tempLookup;
                                }
                                else
                                {
                                    nonSdlLookupDictionary.Add(lookup.Locale, new LookupCollection() { lookup });
                                }
                            }

                            if (_jobResultHandlerList.ContainsKey(lookup.Name))
                            {
                                handler = _jobResultHandlerList[lookup.Name];
                                handler.AddLocale(lookup.Locale);
                            }
                            else
                            {
                                handler = new LookupImportProgressHandler();
                                handler.AddLocale(lookup.Locale);
                                _jobResultHandlerList.Add(lookup.Name, handler);

                                if (lookup.Rows != null)
                                {
                                    //Assumed in different locale also no. of records are same. But what will happen for Lookup xml ?
                                    handler.SetTotalLookupRecords(lookup.Rows.Count);
                                }
                            }
                        }
                    }

                    this.UpdateJobResult();      //Update the job result if any errors are there.

                    if (sdlLookups.Count > 0)
                    {
                        Process(sdlLookups);

                        //Invalidate all the sdl lookups cache which are processed successfully before process non SDL lookups
                        //This needs to be added because when lookup process happens for non sdl, it first try to find sdl lookup is existing or not in cache
                        //If the value is not found in cache then non sdl lookup process will fail
                        InvalidateLookupDataCache();
                    }

                    if (nonSdlLookupDictionary.Count > 0)
                    {
                        foreach (LookupCollection nonSdlLookups in nonSdlLookupDictionary.Values)
                        {
                            Process(nonSdlLookups);
                        }
                    }
                }
                else
                {
                    //Need to log that lookup is null or count is less than zero or if data is not in correct format.
                    String message = "No lookup table is found from the source data. You must check for the data entered in correct format.";
                    throw new MDMOperationException("112900", message, "LookupImportEngine", String.Empty, "Process");
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
            //parallel ends here..
            finally
            {
                //Invalidate all the lookups cache which are processed successfully
                InvalidateLookupDataCache();

                if (Constants.TRACING_ENABLED)
                {
                    MDMTraceHelper.StopTraceActivity("LookupImportEngine.Process", MDMTraceSource.LookupImport);
                }
            }
        }

        private void Process(LookupCollection lookups)
        {
            Int32 numberofLookups = -1;

            if (lookups != null)
            {
                numberofLookups = lookups.Count;
            }

            if (numberofLookups > 0)
            {
                if (Constants.TRACING_ENABLED)
                {
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Starting lookup batch creation...", MDMTraceSource.LookupImport);
                }

                //Reconfigure the thread count based on the batches required
                if (numberofLookups < _numberOfLookupsThreads)
                    _numberOfLookupsThreads = numberofLookups;

                Int32 numberOflookupPerThread = numberofLookups / _numberOfLookupsThreads;

                //Split the lookup per threads
                _lookupSplit = SplitLookupsForThreads(_numberOfLookupsThreads, 0, numberofLookups - 1, numberOflookupPerThread);

                if (Constants.TRACING_ENABLED)
                {
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Number of lookup threads to be created: " + _numberOfLookupsThreads, MDMTraceSource.LookupImport);
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Distributing lookup into parallel thread started.", MDMTraceSource.LookupImport);
                }

                //Used in case multiple threads are trying to update the job results with the import progress
                syncObject = new Object();

                // Parallel execute. Each task/thread is independent of other. This is simpler for our purpose and light weight than managing individual threads.
                Parallel.For(0, _numberOfLookupsThreads, i =>
                {
                    Process(lookups, _lookupSplit[i], _lookupSplit[i + 1] - 1, _numberOfRecordThreadsPerLookup, _lookupRecordBatchSize, i);
                }
                );

                if (Constants.TRACING_ENABLED)
                {
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Distributing lookup into parallel thread completed.", MDMTraceSource.LookupImport);
                }
            }
        }

        private void Process(LookupCollection lookups, Int32 start, Int32 end, Int32 numberOfRecordThreadsPerLookup, Int32 batchSize, Int32 threadnumber)
        {

            Lookup lookup = null;
            LookupCollection filteredLookups = new LookupCollection();

            try
            {
                //Prepare different bucket for each lookup thread
                for (Int32 j = start; j <= end; j++)
                {
                    lookup = lookups.ElementAt(j);

                    if (lookup != null)
                    {
                        filteredLookups.Add(lookup);
                    }
                }

                foreach (Lookup selectedlookup in filteredLookups)
                {
                    Int32 totalNumberofLookupRows = selectedlookup.Rows.Count();

                    Int32 numberOfRowBatchesRequired = totalNumberofLookupRows / _lookupRecordBatchSize;

                    if (totalNumberofLookupRows % _lookupRecordBatchSize > 0)
                        numberOfRowBatchesRequired++;

                    _numberOfRecordThreadsPerLookup = Math.Min(numberOfRowBatchesRequired, _numberOfRecordThreadsPerLookup);

                    if (_numberOfRecordThreadsPerLookup < 1)
                        _numberOfRecordThreadsPerLookup = 1;

                    Int32 noOfRecordsPerRecordBatch = totalNumberofLookupRows / _numberOfRecordThreadsPerLookup;

                    if (noOfRecordsPerRecordBatch < 1)
                        noOfRecordsPerRecordBatch = 1;

                    if (Constants.TRACING_ENABLED)
                    {
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Number of record threads per lookup thread: " + _numberOfRecordThreadsPerLookup + "and number of records per lookup thread: " + noOfRecordsPerRecordBatch, MDMTraceSource.LookupImport);
                    }

                    //Split the lookup per threads
                    Dictionary<Int32, Int32> _lookupRecordsSplit = SplitLookupsForThreads(_numberOfRecordThreadsPerLookup, 0, totalNumberofLookupRows - 1, noOfRecordsPerRecordBatch);

                    // Parallel execute. Each task/thread is independent of other. This is simpler for our purpose and light weight than managing individual threads.
                    Parallel.For(0, _numberOfRecordThreadsPerLookup, i =>
                    {
                        Process(selectedlookup, _lookupRecordsSplit[i], _lookupRecordsSplit[i + 1] - 1, _lookupRecordBatchSize, i, numberOfRowBatchesRequired);
                    }
                    );
                }
            }
            catch
            {
                throw;
            }
        }

        private void Process(Lookup lookup, Int32 start, Int32 end, Int32 batchSize, Int32 threadNumber, Int32 totalNoOfBatches)
        {
            Int32 currentBatchStart = 0;
            Int32 currentBatchEnd = 0;
            Int32 lastBatchEnd = 0;

            if ((lookup != null) && (lookup.Rows != null && lookup.Rows.Count > 0))
            {
                for (Int32 batchIndex = 0; batchIndex < totalNoOfBatches; batchIndex++)
                {
                    currentBatchEnd = (batchIndex + 1) * batchSize;

                    Lookup lookupToProcess = lookup.CopyStructure();

                    if (currentBatchEnd > end + 1)
                    {
                        currentBatchEnd = end + 1;
                    }

                    for (; currentBatchStart < currentBatchEnd; currentBatchStart++)
                    {
                        Row row = lookup.Rows.ElementAt(currentBatchStart);

                        if (row != null)
                        {
                            lookupToProcess.Rows.Add(row);
                        }
                    }

                    if ((lookupToProcess != null) && (lookupToProcess.Rows != null && lookupToProcess.Rows.Count > 0))
                    {
                        this.Process(lookupToProcess, lastBatchEnd, currentBatchEnd, threadNumber);
                    }

                    lastBatchEnd = currentBatchEnd;
                }
            }
        }

        private void Process(Lookup lookup, Int32 start, Int32 end, Int32 threadnumber)
        {
            // This get method is called by the parallel loop. This will process the lookup rows between the start and end parameter.
            String message = String.Empty;
            IOperationResult operationResult = MDMObjectFactory.GetIOperationResult();
            LocaleEnum systemDataLocale = GlobalizationHelper.GetSystemDataLocale();

            #region Code for lookup process

            if (Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "LookupImportEngine.Process: Started Process LookupData call... ", MDMTraceSource.LookupImport);
            }

            try
            {
                operationResult = _dataModelService.ProcessLookupData(lookup as ILookup, _iCallerContext, false);

                //Prepare a seperate list of lookup which are processed successfully so that cache can be invalidated.
                if (operationResult.OperationResultStatus == OperationResultStatusEnum.None || operationResult.OperationResultStatus == OperationResultStatusEnum.Successful)
                {
                    //If lookup is imported for SDL then no need to include the lookupKey for nonSDL in the dictionary,
                    //because when cleaning cache for lookup when locale is SDL, it clears cache for all other allowable locale also.
                    //Moreover, this type of key is introduced to avoid inconsistency when multiple threads executes.
                    String lookupKey = String.Empty;
                    if (lookup.Locale == systemDataLocale)
                    {
                        lookupKey = lookup.Name;
                    }
                    else if (!_lookupsProcessedDictionary.ContainsKey(lookupKey))
                    {
                        lookupKey = String.Format("{0}_{1}", lookup.Name, lookup.Locale);
                    }

                    if (!String.IsNullOrEmpty(lookupKey))
                    {
                        _lookupsProcessedDictionary.TryAdd(lookupKey, lookup);
                    }
                }
            }

            catch (Exception ex)
            {
                String exceptionMessage = ex.Message;

                message = String.Format("Processing lookup batch {0} and {1} failed on thread {2} with the exception {3}.", start, end, threadnumber, ex.Message);
                message = String.Format("Job Id {0} - {1}", _job.Id, message);

                HandleError(message);
                operationResult.AddOperationResult("", message, OperationResultType.Error);
            }

            #endregion

            finally
            {
                #region Record Results

                if (operationResult != null && operationResult.OperationResultStatus != OperationResultStatusEnum.None)
                {
                    lock (syncObject)
                    {
                        PopulateImportResult((OperationResult)operationResult, lookup, start, end);
                    }
                }
                #endregion

                if (Constants.TRACING_ENABLED)
                {
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "LookupImportEngine.Process: Completed Process LookupData call... ", MDMTraceSource.LookupImport);
                }
            }
        }

        #endregion

        #region Logging and Error Handling

        /// <summary>
        /// Populate import result table.
        /// </summary>
        private void PopulateImportResult(OperationResult operationResult, Lookup lookup, Int32 start, Int32 end)
        {
            OperationResult lastBatchOperationResult = _jobImportResultBL.GetLookupJobOperationResult(_job.Id, lookup.Name, (CallerContext)_iCallerContext);

            if (lastBatchOperationResult == null)  //create 
            {
                JobImportResultCollection jobImportResults = new JobImportResultCollection();

                JobImportResult jobResult = new JobImportResult() { JobId = _job.Id, ExternalId = lookup.Name, InternalId = lookup.Id, ObjectType = ObjectType.Lookup };
                JobImportResultBL jobImportResultBL = new JobImportResultBL();
                LookupImportProgressHandler handler = _jobResultHandlerList[lookup.Name];

                jobResult.Description = String.Format("Processing lookup batch {0} and {1}", start, end);
                jobResult.Status = operationResult.OperationResultStatus.ToString();

                this.PopulateOperationResultExtendendProperties(operationResult, handler, start, end);

                jobResult.OperationResultXML = operationResult.ToXml();
                jobResult.Action = ObjectAction.Create;
                jobImportResults.Add(jobResult);

                jobImportResultBL.Save(jobImportResults, _application, _module, importProgram, importUser);
            }

            //If start and end batch are same then there is no need to update the result. This will be same when lookup table itself invalid.
            this.UpdateJobResult(lastBatchOperationResult, operationResult, lookup, start, end);

            this.UpdateJobStatus(_job.JobStatus, String.Format("Processing lookup batch {0} and {1}", start, end));
        }

        private void RefreshJobStatus()
        {
            if (_jobResultHandlerList != null)
            {
                JobStatus overAllStatus = JobStatus.UnKnown;
                Boolean isFinalBatch = true;

                foreach (var items in _jobResultHandlerList)
                {
                    LookupImportProgressHandler handler = items.Value;

                    if (handler != null)
                    {
                        JobStatus status = handler.GetJobStatus();

                        if (status == JobStatus.Running || status == JobStatus.UnKnown)
                        {
                            _job.JobStatus = JobStatus.Running;
                            isFinalBatch = false;
                            break;
                        }
                        else if (status == JobStatus.CompletedWithErrors)
                        {
                            overAllStatus = _job.JobStatus = status;
                        }
                        else if (status == JobStatus.CompletedWithWarnings)
                        {
                            overAllStatus = _job.JobStatus = status;
                        }
                    }
                }

                if (isFinalBatch == true)
                {
                    if (overAllStatus == JobStatus.CompletedWithWarnings)
                    {
                        _job.JobStatus = JobStatus.CompletedWithWarnings;
                    }
                    else if (overAllStatus != JobStatus.CompletedWithErrors)
                    {
                        _job.JobStatus = JobStatus.Completed;
                    }
                }
            }
        }

        private void UpdateJobResult(OperationResult lastBatchOperationResult, OperationResult runningBatchOperationResult, Lookup lookup, Int32 startBatch, Int32 endBatch)
        {
            LookupImportProgressHandler handler = _jobResultHandlerList[lookup.Name];
            //Get the error messages from existing operation result
            OperationResult finalOutput = new OperationResult();

            if (lastBatchOperationResult != null)
            {
                finalOutput.Errors.AddRange(lastBatchOperationResult.GetErrors());
                finalOutput.Warnings.AddRange(lastBatchOperationResult.GetWarnings());
                finalOutput.ExtendedProperties = lastBatchOperationResult.ExtendedProperties;
            }

            if (runningBatchOperationResult != null)
            {
                foreach (DictionaryEntry properties in runningBatchOperationResult.ExtendedProperties)
                {
                    if (finalOutput.ExtendedProperties.ContainsKey(properties.Key))
                    {
                        finalOutput.ExtendedProperties[properties.Key] = properties.Value;
                    }
                    else
                    {
                        finalOutput.ExtendedProperties.Add(properties.Key, properties.Value);
                    }
                }

                this.UpdateErrors(runningBatchOperationResult, finalOutput, handler);

                this.UpdateWarnings(runningBatchOperationResult, finalOutput, handler);

                if (endBatch != startBatch) //When the lookup itself not valid we will not process the lookup but will update the job result status.
                {
                    Int64 sucessfulRecordCount = (endBatch - startBatch) - runningBatchOperationResult.Errors.Count;
                    handler.UpdateCompletedLookupBatch(sucessfulRecordCount);
                }

                //check whether the current lookup is processed or not 
                if (handler.GetTotalLookupRecords() == handler.GetCompletedLookupRecords())
                {
                    handler.AddProcessedLookupLocale(lookup.Locale);
                }

                this.PopulateOperationResultExtendendProperties(finalOutput, handler, startBatch, endBatch);
                _jobResultHandlerList[lookup.Name] = handler;

                //Update the final operation result to data base
                _jobImportResultBL.UpdateLookupOperationResult(_job.Id, lookup.Name, finalOutput, (CallerContext)_iCallerContext);
            }
        }

        private void UpdateErrors(OperationResult sourceResult, OperationResult resultResult, LookupImportProgressHandler handler)
        {
            if (sourceResult != null && handler != null)
            {
                foreach (Error error in sourceResult.GetErrors())
                {
                    if (!String.IsNullOrWhiteSpace(error.ReferenceId))
                    {
                        if (!handler.GetReferenceCodes().Contains(error.ReferenceId))
                        {
                            handler.AddReferenceCode(error.ReferenceId);
                            resultResult.Errors.Add(error);
                        }
                        else
                        {
                            handler.UpdateCompletedLookupBatch(successfulLookupRecords: 0, failedLookupRecords: 1);
                        }
                    }
                    else
                    {
                        resultResult.Errors.Add(error);
                        handler.UpdateCompletedLookupBatch(0, 1);
                    }
                }
            }
        }

        private void UpdateWarnings(OperationResult sourceResult, OperationResult resultResult, LookupImportProgressHandler handler)
        {
            if (sourceResult != null && handler != null)
            {
                foreach (Warning warning in sourceResult.GetWarnings())
                {
                    handler.UpdateWarningLookupBatch(1);
                    resultResult.Warnings.Add(warning);
                }
            }
        }

	    /// <summary>
	    /// Update the job status.
	    /// </summary>
	    /// <param name="jobStatus"></param>
	    /// <param name="description"></param>
	    private void UpdateJobStatus(JobStatus jobStatus, String description)
        {
            if (Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("Updating job status for Job: {0} Status: {1}", _job.Id, jobStatus), MDMTraceSource.LookupImport);
            }

            RefreshJobStatus();

            lock (_lockObject)
            {
                DateTime endTime = DateTime.Now;
                _job.JobData.ExecutionStatus.EndTime = endTime.ToString();
                DateTime startTime = DateTime.Now;

                if (DateTime.TryParse(_job.JobData.ExecutionStatus.StartTime, out startTime))
                {
                    _job.JobData.ExecutionStatus.TotalMilliSeconds = (endTime - startTime).TotalMilliseconds;
                }

                _job.JobData.ExecutionStatus.CurrentStatusMessage = _job.JobStatus.ToString();

                if (String.IsNullOrWhiteSpace(_job.JobData.ExecutionStatus.StartTime))
                {
                    _job.JobData.ExecutionStatus.StartTime = startTime.ToString();
                }

                #region Prepare job description
                String jobDescription = String.Empty;

                if ((_erroredLookups != null && _erroredLookups.Count > 0 && _job.JobData.ExecutionStatus.TotalElementsProcessed == _erroredLookups.Count) ||
                    (_invalidLookupUniqueColumnsDictionary != null && _invalidLookupUniqueColumnsDictionary.Count > 0 && _job.JobData.ExecutionStatus.TotalElementsProcessed == _invalidLookupUniqueColumnsDictionary.Count))    //When total records and failed records are same then all are failed.
                {
                    jobDescription = "No lookups were processed. Please check job details for more information.";
                }
                else
                {
                    jobDescription = String.Format("Total {0} Lookups processed. Successful {1} and Failed {2} .", _jobResultHandlerList.Count, _job.JobData.ExecutionStatus.TotalElementsSucceed, _job.JobData.ExecutionStatus.TotalElementsFailed);
                }

                switch (_job.JobStatus)
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
                        _job.Description = description;
                        break;
                }

                #endregion
            }
            _jobManager.Update(_job, new CallerContext(_application, Utility.GetModule(_job.JobType), importProgram));
        }

        private void HandleError(String errorMessage)
        {
            MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, string.Format("Error in lookup import for job " + _job.Id + ": " + errorMessage), MDMTraceSource.LookupImport);

            _job.JobStatus = JobStatus.CompletedWithErrors;
            _job.Description = errorMessage;
            DateTime endTime = DateTime.Now;
            _job.JobData.ExecutionStatus.EndTime = endTime.ToString();
            DateTime startTime = DateTime.Now;
            if (DateTime.TryParse(_job.JobData.ExecutionStatus.StartTime, out startTime))
            {
                _job.JobData.ExecutionStatus.TotalMilliSeconds = (endTime - startTime).TotalMilliseconds;
            }
            _job.JobData.ExecutionStatus.StartTime = startTime.ToString();

            _job.JobData.OperationResult.AddOperationResult("", errorMessage, OperationResultType.Error);
            _jobManager.Update(_job, new CallerContext(_application, Utility.GetModule(_job.JobType), importProgram));
        }

        private void HandleException(Exception ex)
        {
            MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, "Exception occurred: " + ex.Message, MDMTraceSource.LookupImport);
            String message = string.Format("Lookup processing failed with exception: " + ex.Message);
            LogHandler.WriteErrorLog(ex.Message, 100);
            _job.JobStatus = JobStatus.Aborted;
            UpdateJobStatus(JobStatus.Aborted, "Exception occurred: " + ex.Message);
        }

        private void PopulateOperationResultExtendendProperties(OperationResult operationResult, LookupImportProgressHandler handler, Int32 startBatch, Int32 endBatch)
        {
            if (operationResult != null && operationResult.ExtendedProperties != null && handler != null)
            {
                JobStatus status = handler.GetJobStatus();

                if (status == JobStatus.Completed || status == JobStatus.CompletedWithErrors || status == JobStatus.CompletedWithWarnings)
                {
                    if (operationResult.ExtendedProperties.ContainsKey("SuccessfullRecords"))
                    {
                        operationResult.ExtendedProperties["SuccessfullRecords"] = handler.GetSuccessfulLookupRecords();
                    }
                    else
                    {
                        operationResult.ExtendedProperties.Add("SuccessfullRecords", handler.GetSuccessfulLookupRecords());
                    }

                    if (operationResult.ExtendedProperties.ContainsKey("FailedRecords"))
                    {
                        operationResult.ExtendedProperties["FailedRecords"] = handler.GetFailedLookupRecords();
                    }
                    else
                    {
                        operationResult.ExtendedProperties.Add("FailedRecords", handler.GetFailedLookupRecords());
                    }

                    lock (_lockObject)
                    {

                        _job.JobData.ExecutionStatus.TotalElementsProcessed = _job.JobData.ExecutionStatus.TotalElementsProcessed + 1;

                        if (_job.JobData.ExecutionStatus.TotalElementsToProcess <= 0)
                        {
                            _job.JobData.ExecutionStatus.TotalElementsToProcess = _jobResultHandlerList.Count - 1;  //total number of lookups
                        }
                        else
                        {
                            _job.JobData.ExecutionStatus.TotalElementsToProcess = _job.JobData.ExecutionStatus.TotalElementsToProcess - 1;
                        }

                        if (handler.GetFailedLookupRecords() > 0)
                        {
                            _job.JobData.ExecutionStatus.TotalElementsFailed = _job.JobData.ExecutionStatus.TotalElementsFailed + 1;
                        }
                        else
                        {
                            _job.JobData.ExecutionStatus.TotalElementsSucceed = _job.JobData.ExecutionStatus.TotalElementsSucceed + 1;
                        }
                    }
                }
                // Update the Total records to operation result if operation result does not contains
                if (!operationResult.ExtendedProperties.ContainsKey("TotalRecords"))
                {
                    operationResult.ExtendedProperties.Add("TotalRecords", handler.GetTotalLookupRecords());
                }

                if (operationResult.ExtendedProperties.ContainsKey("Status"))
                {
                    operationResult.ExtendedProperties["Status"] = handler.GetJobStatus();
                }
                else
                {
                    operationResult.ExtendedProperties.Add("Status", handler.GetJobStatus());
                }

                String lastUpdatedDateTime = FormatHelper.FormatDate(DateTime.Now.ToString(), System.Threading.Thread.CurrentThread.CurrentCulture.Name, LocaleEnum.en_US.GetCultureName());

                if (operationResult.ExtendedProperties.ContainsKey("LastUpdatedDateTime"))
                {
                    operationResult.ExtendedProperties["LastUpdatedDateTime"] = lastUpdatedDateTime;
                }
                else
                {
                    operationResult.ExtendedProperties.Add("LastUpdatedDateTime", lastUpdatedDateTime);
                }

                String description = String.Format("Processing lookup batch {0} and {1}", startBatch, endBatch);
                if (operationResult.ExtendedProperties.ContainsKey("Description"))
                {
                    operationResult.ExtendedProperties["Description"] = description;
                }
                else
                {
                    operationResult.ExtendedProperties.Add("Description", description);
                }
            }
        }

        /// <summary>
        /// Invalidate cache for all lookups which are processed successfully.
        /// </summary>
        private void InvalidateLookupDataCache()
        {
            //Invalidate all the lookups cache which are processed successfully
            if (_lookupsProcessedDictionary.Count > 0)
            {
                LookupCollection lookupsProcessed = new LookupCollection();

                foreach (Lookup lookup in _lookupsProcessedDictionary.Values)
                {
                    lookupsProcessed.Add(lookup);
                }

                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Started Invalidation of lookupCache.", MDMTraceSource.LookupImport);

                _lookupBL.InvalidateLookupData(lookupsProcessed);

                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Completed invalidation of lookupCache.", MDMTraceSource.LookupImport);
            }
        }

        /// <summary>
        /// Update the job results if any invalid lookups are found in current running job.
        /// </summary>
        private void UpdateJobResult()
        {
            if (_erroredLookups != null && _erroredLookups.Count > 0)
            {
                foreach (Lookup lookup in _erroredLookups)
                {
                    String errorMessage = String.Format("Unique column(s) does not exist for lookup [{0}]. Add one or more unique column(s).", lookup.Name);
                    UpdateOperationResult("112895", errorMessage, lookup, new Collection<Object> { lookup.Name }, OperationResultType.Error);
                }
            }

            if (_invalidLookupUniqueColumnsDictionary != null && _invalidLookupUniqueColumnsDictionary.Count > 0)
            {
                foreach (Lookup invalidLookupColumns in _invalidLookupUniqueColumnsDictionary.Keys)
                {
                    String columnNames = ValueTypeHelper.JoinColumnCollectionWithEscaping(_invalidLookupUniqueColumnsDictionary[invalidLookupColumns], "|");

                    if (columnNames.Length > 0)
                    {
                        String errorMessage = String.Format("Unique column(s) '{0}' does not exist in the '{1}' Lookup Table", columnNames, invalidLookupColumns.Name);
                        UpdateOperationResult("113983", errorMessage, invalidLookupColumns, new Collection<Object> { columnNames, invalidLookupColumns.Name }, 
                                                    OperationResultType.Error);
                    }
                }
            }

            if (_invalidLookupNonUniqueColumnsDictionary != null && _invalidLookupNonUniqueColumnsDictionary.Count > 0)
            {
                foreach (Lookup invalidNonUniqueLookup in _invalidLookupNonUniqueColumnsDictionary.Keys)
                {
                    String columnNames = ValueTypeHelper.JoinColumnCollectionWithEscaping(_invalidLookupNonUniqueColumnsDictionary[invalidNonUniqueLookup], "|");

                    if (columnNames.Length > 0)
                    {
                        String warningMessage = String.Format("Column(s) '{0}' does not exist in the Lookup Table '{1}'.", columnNames, invalidNonUniqueLookup.Name);
                        UpdateOperationResult("114092", warningMessage, invalidNonUniqueLookup, new Collection<Object> { columnNames, invalidNonUniqueLookup.Name },
                                                    OperationResultType.Warning);
                    }
                }
            }
        }

        private void UpdateOperationResult(String messageCode, String message, Lookup lookup, Collection<Object> parameters, OperationResultType operationResultType)
        {
            OperationResult operationResult = new OperationResult();

            if (operationResultType == OperationResultType.Error)
            {
                operationResult.ExtendedProperties.Add("ErrorLevel", "Table");
            }

            operationResult.AddOperationResult(messageCode, message, "-1" , parameters, operationResultType);
            this.PopulateImportResult(operationResult, lookup, start: 0, end: 0);
        }

        #endregion

        #region Validations

        /// <summary>
        /// Gets the list of lookup names which does not have any unique key column.
        /// </summary>
        /// <param name="lookups">Indicates the list of lookups</param>
        /// <returns>List of lookup names which does not have any unique key column</returns>
        private Collection<String> FillAndValidateLookups(LookupCollection lookups)
        {
            //Logical Flow:
            //1. Cleanse all Ids from the table data 
            //2. Get the lookup schema for requested lookups
            //3. Check at-least one column has unique column or not for each lookup.
            //   If not then returns the list of lookups which does not have the unique key column

            CleanseInternalIds(lookups);

            Collection<String> lookupNames = null;

            if (lookups != null && lookups.Count > 0)
            {
                lookupNames = new Collection<String>();

                foreach (Lookup lookup in lookups)
                {
                    if (!(lookup == null || lookupNames.Contains(lookup.Name)))
                    {
                        lookupNames.Add(lookup.Name);
                    }
                }
            }

            if (lookupNames.Count > 0)
            {
                LocaleEnum systemDataLocale = GlobalizationHelper.GetSystemDataLocale();
                LookupCollection lookupModels = _lookupBL.GetLookupSchema(lookupNames, (CallerContext)_iCallerContext);

                if (lookupModels != null)
                {
                    _erroredLookups = new LookupCollection();
                    _missingOrInvalidLookupNames = new Collection<String>();
                   
                    foreach (Lookup inputLookup in lookups)
                    {
                        var lookupModel = lookupModels.GetLookup(inputLookup.Name, inputLookup.Locale);

                        #region Filter Missing UniqueKey Lookups From Input File

                        if (!(lookupModel == null || lookupModel.HasUniqueColumns))
                        {
                            _missingOrInvalidLookupNames.Add(lookupModel.Name);
                            _erroredLookups.Add(lookupModel);
                            continue;
                        }

                        #endregion Filter Missing UniqueKey Lookups From Input File

                        #region Fill lookup schema if not provided in the input file

                        if (_importSourceType == ImportSourceType.RSLookupXml10 && inputLookup.Columns == null || inputLookup.Columns.Count < 1)
                        {
                            inputLookup.Columns = (ColumnCollection)lookupModel.Columns.Clone();
                        }

                        #endregion

                        if (lookupModel != null)
                        {
                            #region Filter Invalid UniqueKey Lookups

                            FilterInvalidLookups(lookupModel, inputLookup, true, _invalidLookupUniqueColumnsDictionary);

                            #endregion Filter Invalid UniqueKey Lookups

                            #region Filter Invalid NonUnique Lookups

                            FilterInvalidLookups(lookupModel, inputLookup, false, _invalidLookupNonUniqueColumnsDictionary);

                            #endregion
                        }
                    }
                }
            }

            return _missingOrInvalidLookupNames;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lookups"></param>
        private void CleanseInternalIds(LookupCollection lookups)
        {
            Int32 tableIdSequence = -1;

            foreach(Lookup lookup in lookups)
            {
                Int32 rowIdSequence = -1;
                Int32 columnIdSequence = -1;

                lookup.Id = tableIdSequence--;

                if (lookup.Columns != null && lookup.Columns.Count > 0)
                {
                    foreach (var column in lookup.Columns)
                    {
                        column.Id = columnIdSequence--;
                    }
                }

                if(lookup.Rows != null && lookup.Rows.Count > 0)
                {
                    foreach(var row in lookup.Rows)
                    {
                        row.Id = rowIdSequence--;
                    }
                }
            }
        }

        /// <summary>
        /// Filters the Lookups by Comparing LookupModel available in InputFile with LookupModel available in Database
        /// </summary>
        /// <param name="lookupModel">Lookup which is defined in Database</param>
        /// <param name="lookups">Lookup collection available in InputFile</param>
        private void FilterInvalidLookups(Lookup lookupFromDB, Lookup inputLookup, Boolean isUniqueColumn, 
                                                        Dictionary<Lookup, Collection<String>> invalidLookupColumnsDictionary)
        {
            #region Get LookupUniqueColumns Available in Database

            Collection<String> lookupColumnNamesInDB = GetValidLookupColumnNamesList(lookupFromDB, isUniqueColumn);
            Collection<String> invalidLookupColumnNames = new Collection<String>();

            #endregion Get LookupUniqueColumns Available in Database

            #region Compare Column of Lookup in Input File with Column Available in Database

            if (inputLookup == null || (isUniqueColumn && !inputLookup.HasUniqueColumns))
            {
                _missingOrInvalidLookupNames.Add(lookupFromDB.Name);
                _erroredLookups.Add(lookupFromDB);
            }
            else
            {
                Collection<String> inputFilelookupColumnNames = GetValidLookupColumnNamesList(inputLookup, isUniqueColumn); 

                if (lookupColumnNamesInDB.Count > 0)
                {
                    foreach (String lookupColumnName in inputFilelookupColumnNames)
                    {
                        if (!lookupColumnNamesInDB.Contains(lookupColumnName))
                        {
                            invalidLookupColumnNames.Add(lookupColumnName);
                        }
                    }
                }

                if (invalidLookupColumnNames.Count > 0)
                {
                    if (isUniqueColumn)
                    {
                        _missingOrInvalidLookupNames.Add(lookupFromDB.Name);
                    }

                    if (!invalidLookupColumnsDictionary.ContainsKey(inputLookup))
                    {
                        invalidLookupColumnsDictionary.Add(inputLookup, invalidLookupColumnNames);
                    }
                }
            }
          
            #endregion Compare Column of Lookup in Input File with UniqueKey Column Available in Database
        }

        private Collection<String> GetValidLookupColumnNamesList(Lookup lookup, Boolean isUniqueColumn)
        {
            ColumnCollection lookupColumns = lookup.Columns;

            Collection<String> lookupColumnNames= new Collection<String>();

            foreach (Column column in lookupColumns)
            {
                if((!String.IsNullOrWhiteSpace(column.Name)) &&
                    ((isUniqueColumn && column.IsUnique) || (!isUniqueColumn && !column.IsUnique)))
                {
                    lookupColumnNames.Add(column.Name);
                }
            }

            return lookupColumnNames;
        }

        #endregion

        #endregion
    }
}
