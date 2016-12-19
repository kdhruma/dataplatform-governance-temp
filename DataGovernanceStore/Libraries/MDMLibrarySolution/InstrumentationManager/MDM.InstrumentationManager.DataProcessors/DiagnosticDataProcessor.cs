using System;
using System.Threading;
using System.Timers;
using Timer = System.Timers.Timer;

namespace MDM.InstrumentationManager.DataProcessors
{
    using MDM.ExceptionManager.Handlers;
    using MDM.BusinessObjects;
    using MDM.BusinessObjects.Diagnostics;
    using MDM.Core;
    using MDM.Core.Extensions;
    using MDM.Interfaces.Diagnostics;
    using MDM.ParallelizationManager.Interfaces;
    using MDM.ParallelizationManager.Processors;
    using MDM.Services;
    using MDM.Utility;
    using MDM.InstrumentationManager.Utility;
    using MDM.CacheManager.Business;

    /// <summary>
    /// Specifies Diagnostic Data processor
    /// </summary>
    public class DiagnosticDataProcessor : IDiagnosticDataProcessor
    {
        #region Constants

        private const Int32 NUMBER_OF_THREADS = 1;
        private const Double TIMEOUT_CHECK_INTERAL = 10000.0;

        #endregion

        #region Fields

        /// <summary>
        /// Stores the last time the data was processed 
        /// </summary>
        private static DateTime _lastDataProcessedTime = DateTime.Now;

        /// <summary>
        /// Stores the last time the amount of data was processed.
        /// </summary>
        private static Int32 _lastProcessedDataCount = 0;

        /// <summary>
        /// 
        /// </summary>
        private static String _password = "";

        /// <summary>
        /// 
        /// </summary>
        private readonly CallerContext _callerContext = new CallerContext(MDMCenterApplication.MDMCenter, MDMCenterModules.Instrumentation, "DiagnosticDataProcessor");

        /// <summary>
        /// 
        /// </summary>
        private const String _diagnosticSaveBatchSizeConfigName = "MDMCenter.ParallelProcessingEngine.DiagnosticDataProcessor.BulkSaveBatchSize";

        /// <summary>
        /// 
        /// </summary>
        private const String _diagnosticBatchTimeoutInMillisecondsConfigName = "MDMCenter.ParallelProcessingEngine.DiagnosticDataProcessor.BatchTimeoutInMilliseconds";

        /// <summary>
        /// 
        /// </summary>
        private DataProcessor _dataProcessor = null;

        /// <summary>
        /// 
        /// </summary>
        private BatchProcessor<IDiagnosticDataElement> _batchProcessor = null;

        /// <summary>
        /// 
        /// </summary>
        private static readonly Object _lockObj = new Object();

        /// <summary>
        /// 
        /// </summary>
        private static DiagnosticDataProcessor _singleton = null;

        /// <summary>
        /// 
        /// </summary>
        private static Boolean _isInitialized = false;

        /// <summary>
        /// Field to log event logs
        /// </summary>
        private EventLogHandler eventLogHandler;

        private Boolean isDistributedNotificationEnabled;
        private Timer checkTracingTimeout;

        #endregion

        #region Properties

        /// <summary>
        /// Stores the last time the amount of data was processed.
        /// </summary>
        public static DateTime LastDataProcessedTime
        {
            get { return _lastDataProcessedTime; }
        }

        /// <summary>
        /// Stores the last time the data was processed 
        /// </summary>
        public static Int32 LastProcessedDataCount
        {
            get { return _lastProcessedDataCount; }
        }

        #endregion

        #region Methods

        #region Public Methods

        /// <summary>
        /// Starts processing
        /// </summary>
        /// <returns>Returns <b>true</b> if started</returns>
        public static Boolean Start()
        {
            if (!_isInitialized)
            {
                lock (_lockObj)
                {
                    if (!_isInitialized)
                    {
                        _singleton = new DiagnosticDataProcessor();

                        _singleton.eventLogHandler = new EventLogHandler();

                        _singleton._dataProcessor = new DataProcessor(CoreDataProcessorList.DiaganosticDataProcessor.ToString());

                        TaskAction taskAction = new TaskAction(_singleton.ProcessDiagnosticDataElement);
                        CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

                        _isInitialized = _singleton._dataProcessor.Initialize(taskAction, cancellationTokenSource, NUMBER_OF_THREADS);

                        _singleton.isDistributedNotificationEnabled = CacheFactory.GetDistributedCacheWithNotification() is DistributedCacheWithNotification;

                        Int32 bulkSaveBatchSize = AppConfigurationHelper.GetAppConfig<Int32>(_diagnosticSaveBatchSizeConfigName, 200);
                        Int32 batchTimeoutInMilliseconds = AppConfigurationHelper.GetAppConfig<Int32>(_diagnosticBatchTimeoutInMillisecondsConfigName, 3000);

                        _singleton._batchProcessor = new BatchProcessor<IDiagnosticDataElement>(_singleton.WriteDiagnosticData, cancellationTokenSource, bulkSaveBatchSize, batchTimeoutInMilliseconds);

                        DiagnosticActivity.InitializeProcessor(_singleton);

                        _singleton.checkTracingTimeout = new Timer(TIMEOUT_CHECK_INTERAL) {AutoReset = false};
                        _singleton.checkTracingTimeout.Elapsed += _singleton.OnCheckTracingTimeout;
                        _singleton.checkTracingTimeout.Start();

                        _isInitialized = true;
                    }
                }
            }
            else
            {
                _singleton.eventLogHandler.WriteErrorLog("Diagnostic data process has already been initialized in this process.", 1);
            }

            return _isInitialized;
        }

        /// <summary>
        /// Stops processing
        /// </summary>
        public static void Stop()
        {
            try
            {
                _singleton.checkTracingTimeout.Stop();
                _singleton.checkTracingTimeout.Dispose();

                Int64 pendingItemCount = _singleton._dataProcessor.GetPendingItemCount() + _singleton._batchProcessor.GetPendingItemCount();
                _singleton.eventLogHandler.WriteInformationLog("Stopping DiagnosticDataProcessor with " + pendingItemCount + " pending records", 1);
                
                _singleton._dataProcessor.Complete(0, false); // Do not cancel current queue of diagnostic data
                _singleton._batchProcessor.Complete();
                _singleton._batchProcessor.Completion.Wait();
            }
            catch (AggregateException exception)
            {
                foreach (Exception ex in exception.InnerExceptions)
                {
                    _singleton.eventLogHandler.WriteErrorLog(ex.ToString(), 1);
                }
            }
            catch (Exception exception)
            {
                _singleton.eventLogHandler.WriteErrorLog(exception.ToString(), 1);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="diagnosticDataElement"></param>
        /// <returns></returns>
        public Boolean Post(IDiagnosticDataElement diagnosticDataElement)
        {
            Boolean returnValue = true;

            MDMMessagePackage mdmMessagePackage = new MDMMessagePackage { Data = diagnosticDataElement };

            if (Core.Constants.DIAGNOSTIC_TRACING_STORAGE_MODE == DiagnosticTracingStorageMode.Legacy)
            {
                this.ProcessDiagnosticDataElement(mdmMessagePackage);
            }
            else
            {
                returnValue = _dataProcessor.PostAsync(mdmMessagePackage);
            }

            return returnValue;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="messagePackage"></param>
        private void ProcessDiagnosticDataElement(MDMMessagePackage messagePackage)
        {
            try
            {
                var diagnosticDataElement = (IDiagnosticDataElement)messagePackage.Data;

                #region Populating default values for empty context data

                var executionContext = (ExecutionContext)diagnosticDataElement.GetExecutionContext();

                if (executionContext != null)
                {
                    if (executionContext.CallerContext == null)
                    {
                        executionContext.CallerContext = new CallerContext();
                    }

                    if (String.IsNullOrEmpty(executionContext.CallerContext.ServerName))
                    {
                        executionContext.CallerContext.ServerName = Environment.MachineName;
                    }
                }

                #region Populate Security information

                //if (_executionContext != null && _executionContext.SecurityContext != null)
                //{
                //    SecurityPrincipal currentSecurityPrincipal = SecurityPrincipalHelper.GetCurrentSecurityPrincipal();

                //    if (String.IsNullOrEmpty(_executionContext.SecurityContext.UserLoginName))
                //    {
                //        _executionContext.SecurityContext.UserLoginName = currentSecurityPrincipal.CurrentUserName;
                //    }

                //    if (_executionContext.SecurityContext.UserId <= 0)
                //    {
                //        _executionContext.SecurityContext.UserId = currentSecurityPrincipal.CurrentUserId;
                //    }
                //}

                #endregion

                #endregion


                if (diagnosticDataElement != null)
                {
                    #region Copy over errors / warnings into system trace
                    
                    if (diagnosticDataElement is DiagnosticRecord)
                    {
                        var diagnosticRecord = (DiagnosticRecord)diagnosticDataElement;

                        //Write all critical errors and warnings as part of system message too
                        if (diagnosticRecord.MessageClass == MessageClassEnum.Error || diagnosticRecord.MessageClass == MessageClassEnum.Warning)
                        {
                            bool skip = false;

                            if (diagnosticRecord.ExecutionContext != null)
                            {
                                var sources = Constants.DIAGNOSTIC_SYSTEMLOG_LEGACYSOURCESTOSKIP.Replace("MDMTraceSource.", "");
                                if(!String.IsNullOrEmpty(sources))
                                {
                                    var legacySourcesToSkip = ValueTypeHelper.SplitStringToEnumCollection<MDMTraceSource>(sources, '|');

                                    if (diagnosticRecord.ExecutionContext.LegacyMDMTraceSources.ContainsAny(legacySourcesToSkip))
                                        skip = true;
                                }
                            }

                            if (!skip)
                            {
                                var clonedCriticalDiagnosticRecord = diagnosticRecord.Clone() as DiagnosticRecord;

                                if (clonedCriticalDiagnosticRecord != null)
                                {
                                    clonedCriticalDiagnosticRecord.OperationId = Constants.SystemTracingOperationId;

                                    PersistDiagnosticData(clonedCriticalDiagnosticRecord);
                                }
                            }
                        }
                    }

                    #endregion

                    #region Qualify and persist data element

                    if (QualifyDiagnosticDataElementForProcessing(diagnosticDataElement))
                    {
                        PersistDiagnosticData(diagnosticDataElement);
                    }

                    #endregion
                }
            }
            catch (Exception exception)
            {
                _singleton.eventLogHandler.WriteErrorLog(exception.ToString(), 1);
            }
        }

        private void OnCheckTracingTimeout(object sender, ElapsedEventArgs elapsedEventArgs)
        {
            try
            {
                MDMOperationContextHelper.ResetOperationContext();

                if (!_singleton.isDistributedNotificationEnabled)
                {
                    AppConfigurationHelper.ReloadAppConfig("MDMCenter.TracingProfile");
                    String tracingProfileXml = AppConfigurationHelper.GetAppConfig("MDMCenter.TracingProfile", String.Empty);
                    if (!String.IsNullOrWhiteSpace(tracingProfileXml))
                    {
                        TracingProfile.LoadCurrent(tracingProfileXml);
                    }
                }

                TracingProfile tracingProfile = TracingProfile.GetCurrent();

                if (tracingProfile.TraceSettings.IsBasicTracingEnabled &&
                    tracingProfile.EndDateTime < DateTime.Now)
                {
                    if (String.IsNullOrEmpty(_password))
                    {
                        _password = AppConfigurationHelper.GetAppConfig<String>("MDM.WCFServices.MessageSecurity.IdentityKey");
                    }

                    // Create an instance of data service
                    DiagnosticService diagnosticService = new DiagnosticService("WSHttpBinding_IDiagnosticService", "System", _password);

                    diagnosticService.StopDiagnosticTraces(_callerContext);
                }
            }
            catch (Exception exception)
            {
                _singleton.eventLogHandler.WriteErrorLog(exception.ToString(), 1);
            }
            finally
            {
                _singleton.checkTracingTimeout.Start();
            }
        }

        #region Qualification Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="diagnosticDataElement"></param>
        /// <returns></returns>
        private Boolean QualifyDiagnosticDataElementForProcessing(IDiagnosticDataElement diagnosticDataElement)
        {
            Boolean status = true;

            if (diagnosticDataElement is DiagnosticRecord)
            {
                status = QualifyDiagnosticRecord((DiagnosticRecord)diagnosticDataElement);
            }
            else if (diagnosticDataElement is DiagnosticActivity)
            {
                status = QualifyDiagnosticActivity((DiagnosticActivity)diagnosticDataElement);
            }

            return status;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="diagnosticRecord"></param>
        /// <returns></returns>
        private Boolean QualifyDiagnosticRecord(DiagnosticRecord diagnosticRecord)
        {
            if (!QualifyDiagnosticActivity(diagnosticRecord.ParentActivity))
                return false;

            if (diagnosticRecord.ParentActivity != null && diagnosticRecord.ParentActivity.TraceSettings.TracingMode == TracingMode.OperationTracing)
                return true;
            
            //TODO:: Record should also be qualified for the record level ExecutionContext property. That qualification is missing right now.

            TracingProfile tracingProfile = TracingProfile.GetCurrent();

            if (tracingProfile != null)
            {
                if (tracingProfile.MessageClasses.Count > 0)
                {
                    if (!tracingProfile.MessageClasses.Contains(diagnosticRecord.MessageClass))
                        return false;
                }
                if (tracingProfile.Messages.Count > 0)
                {
                    if (String.IsNullOrEmpty(diagnosticRecord.Message))
                    {
                        return false;
                    }
                    Boolean detected = false;
                    foreach (String message in tracingProfile.Messages)
                    {
                        if (diagnosticRecord.Message.IndexOf(message, 0, StringComparison.InvariantCultureIgnoreCase) >= 0)
                        {
                            detected = true;
                            break;
                        }
                    }
                    if (!detected)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="diagnosticActivity"></param>
        /// <returns></returns>
        private Boolean QualifyDiagnosticActivity(DiagnosticActivity diagnosticActivity)
        {
            TraceSettings activityTraceSettings = diagnosticActivity.TraceSettings;

            if (activityTraceSettings == null)
            {
                activityTraceSettings = new TraceSettings();
            }

            TracingMode activityTracingMode = activityTraceSettings.TracingMode;

            if (activityTracingMode == TracingMode.OperationTracing)
            {
                return true;
            }

            if (diagnosticActivity.ExecutionContext != null &&
                diagnosticActivity.ExecutionContext.LegacyMDMTraceSources != null &&
                diagnosticActivity.ExecutionContext.LegacyMDMTraceSources.Contains(MDMTraceSource.DiagnosticService))
            {
                return false;
            }

            TracingProfile tracingProfile = TracingProfile.GetCurrent();

            if (tracingProfile == null || !tracingProfile.TraceSettings.IsTracingEnabled)
            {
                return false;
            }

            if (tracingProfile.TraceSettings.TracingMode != TracingMode.SelectiveComponentTracing)
            {
                return false;
            }

            if (tracingProfile.TraceSettings.TracingLevel != activityTraceSettings.TracingLevel)
            {
                return false;
            }

            if (diagnosticActivity.TimeStamp > tracingProfile.EndDateTime)
            {
                return false;
            }

            if (diagnosticActivity.TimeStamp < tracingProfile.StartDateTime)
            {
                return false;
            }

            return
                tracingProfile.CheckActivity(diagnosticActivity);
        }

        #endregion

        #region Persist / Save Methods

        /// <summary>
        /// 
        /// </summary>
        private void PersistDiagnosticData(IDiagnosticDataElement diagnosticDataElement)
        {
            if (Core.Constants.DIAGNOSTIC_TRACING_STORAGE_MODE == DiagnosticTracingStorageMode.Legacy)
            {
                WriteDiagnosticData(new IDiagnosticDataElement[] { diagnosticDataElement });
            }
            else
            {
                _batchProcessor.PostAsync(diagnosticDataElement);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="diagnosticDataElements"></param>
        private void WriteDiagnosticData(IDiagnosticDataElement[] diagnosticDataElements)
        {
            try
            {
                _lastDataProcessedTime = DateTime.Now;
                _lastProcessedDataCount = diagnosticDataElements.Length;

                // Store in file only if the storage mode is configured for it.
                if (Constants.DIAGNOSTIC_TRACING_STORAGE_MODE == DiagnosticTracingStorageMode.DatabaseOnly
                    || Constants.DIAGNOSTIC_TRACING_STORAGE_MODE == DiagnosticTracingStorageMode.DatabaseAndFile)
                {
                    WriteIntoDataBase(diagnosticDataElements);
                }

                if (Constants.DIAGNOSTIC_TRACING_STORAGE_MODE == DiagnosticTracingStorageMode.FileOnly
                    || Constants.DIAGNOSTIC_TRACING_STORAGE_MODE == DiagnosticTracingStorageMode.Legacy
                    || Constants.DIAGNOSTIC_TRACING_STORAGE_MODE == DiagnosticTracingStorageMode.DatabaseAndFile)
                {
                    SvcLogTraceWriter.WriteTrace(diagnosticDataElements);
                }
            }
            catch (Exception exception)
            {
                _singleton.eventLogHandler.WriteErrorLog(exception.ToString(), 1);
                // ToDo  - Note that this method excepts the failures to be handled by callee
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void WriteIntoDataBase(IDiagnosticDataElement[] diagnosticDataElements)
        {
            DiagnosticRecordCollection diagnosticRecords = new DiagnosticRecordCollection();
            DiagnosticActivityCollection diagnosticActivities = new DiagnosticActivityCollection();

            foreach (var diagnosticDataElement in diagnosticDataElements)
            {
                if (diagnosticDataElement is DiagnosticRecord)
                {
                    diagnosticRecords.Add((DiagnosticRecord)diagnosticDataElement);
                }
                else if (diagnosticDataElement is DiagnosticActivity)
                {
                    diagnosticActivities.Add((DiagnosticActivity)diagnosticDataElement);
                }
            }

            if (diagnosticActivities.Count > 0 || diagnosticRecords.Count > 0)
            {
                if (String.IsNullOrEmpty(_password))
                {
                    _password = AppConfigurationHelper.GetAppConfig<String>("MDM.WCFServices.MessageSecurity.IdentityKey");
                }

                // Create an instance of data service
                DiagnosticService diagnosticService = new DiagnosticService("WSHttpBinding_IDiagnosticService", "System", _password);

                // Call Process method 
                diagnosticService.ProcessDiagnosticData(diagnosticActivities, diagnosticRecords, _callerContext);
            }
        }

        #endregion

        #endregion

        #endregion
    }
}
