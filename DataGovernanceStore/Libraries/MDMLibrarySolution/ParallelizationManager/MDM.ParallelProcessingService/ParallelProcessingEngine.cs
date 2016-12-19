using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace MDM.ParallelProcessingService
{
    using MDM.BusinessObjects;
    using MDM.Core;
    using MDM.EventIntegrator.Business;
    using MDM.MonitoringManager.Business;
    using MDM.ParallelizationManager.Interfaces;
    using MDM.ParallelizationManager.Processors;
    using MDM.ParallelProcessingService.Interfaces;
    using MDM.Utility;
    using MDM.ExceptionManager.Handlers;
    using MDM.InstrumentationManager.DataProcessors;
    using System.Collections;
    public sealed class ParallelProcessingEngine
    {
        #region Fields

        /// <summary>
        /// Lock object
        /// </summary>
        private static Object lockObj = new Object();

        private String _userName = "System";
        
        /// <summary>
        /// Field denoting the security principal
        /// </summary>
        private SecurityPrincipal _securityPrincipal = null;

        /// <summary>
        /// Field to log event logs
        /// </summary>
        private EventLogHandler eventLogHandler = new EventLogHandler();

        /// <summary>
        /// 
        /// </summary>
        private static ParallelProcessingEngine _instance = null;

        /// <summary>
        /// Field to denote if parallization processor engine is started or not
        /// </summary>
        private Boolean _isParallizationProcessingEngineStarted = false;

        /// <summary>
        /// 
        /// </summary>
        private MonitorBL _monitorBL = new MonitorBL();

        #endregion

        #region Properties

        public Boolean IsParallizationProcessingEngineStarted
        {
            get { return _isParallizationProcessingEngineStarted; }
        }

        public Int32 ProcessorCount
        {
            get
            {
                Int32 count = 0;
                if (DataProcessorFactory.GetProcessors() != null)
                {
                    count = DataProcessorFactory.GetProcessors().Count;
                }
                return count;
            }
        }

        #endregion

        #region Constructors
        
        public ParallelProcessingEngine()
        {
        }
        
        #endregion

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Boolean Start(Boolean checkPreviousStatusFromDb = true)
        {
            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "ParallelProcessing Engine starting...");

            DataProcessorConfigCollection dataProcessorConfigCollection = _monitorBL.GetProcessorConfigurationCollection(Environment.MachineName, MDMServiceType.APIEngine, MDMServiceSubType.ParallelProcessingEngine, new CallerContext(MDMCenterApplication.MDMCenter, MDMCenterModules.Monitoring));
            if (checkPreviousStatusFromDb)
            {
                if (WasEngineStoppedManuallyLastTime(dataProcessorConfigCollection))
                {
                    LogMessage("Parallel Processing Engine");
                    return false;
                }
            }

            DataProcessorFactory.ClearProcessors();

            Boolean successFlag = true;

            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "ParallelProcessing Engine starting...");


            #region Initialize all the processors in the system

            List<CoreDataProcessorList> coreDataProcessorList = GetAllProcessors();
            Int32 totalProcessorsStarted = 0;
            Int32 totalThreadsForProcessors = 0;
            foreach (CoreDataProcessorList coreDataProcessor in coreDataProcessorList)
            {
                DataProcessorConfig processorConfig = GetProcessorConfig(dataProcessorConfigCollection, coreDataProcessor.ToString());
                successFlag = StartDataProcessor(coreDataProcessor, checkPreviousStatusFromDb, processorConfig);
                if (successFlag)
                {
                    totalProcessorsStarted++;
                    IDataProcessor iDataProcessorProcessor = DataProcessorFactory.GetProcessor(coreDataProcessor.ToString());
                    totalThreadsForProcessors += iDataProcessorProcessor.NumberOfThreads;
                }
            }

            #endregion Initialize all the processors in the system

            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "ParallelProcessing Engine Start status : " + successFlag);
            
            if (totalProcessorsStarted > 0)
            {
                SetUserName();
                String message = String.Format("ParallelProcessing Engine Started by {0}  at {1}. Total number of processors started are {2} and total threads used by them are {3}.", _userName, DateTime.Now.ToString("HH:mm:ss tt"), totalProcessorsStarted, totalThreadsForProcessors);
                eventLogHandler.WriteInformationLog(message, 101);
            }

            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StopTraceActivity("ParallelProcessingService.ParallelProcessingEngine");

            
            this._isParallizationProcessingEngineStarted = true;

            return successFlag;
        }

        /// <summary>
        /// Stops the parallel Processing Engine
        /// </summary>
        /// <returns></returns>
        public Boolean Stop(Boolean updateStatusToDb = false)
        {
            if (Constants.TRACING_ENABLED)
			{
                MDMTraceHelper.StartTraceActivity("ParallelProcessingService.ParallelProcessingEngine.Stop", false);
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "ParallelProcessing Engine stopping");
			}

            Boolean result = DataProcessorFactory.Shutdown();

            if (result)
                DataProcessorFactory.ClearProcessors();

            if (updateStatusToDb)
                UpdateEngineStatus(false, null, null);

            this._isParallizationProcessingEngineStarted = false;

            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "ParallelProcessing Engine stopped");

            
            SetUserName();
            eventLogHandler.WriteInformationLog("ParallelProcessing Engine stopped by " + _userName + " at " + DateTime.Now.ToString("HH:mm:ss tt"), 1);

            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StopTraceActivity("ParallelProcessingService.ParallelProcessingEngine.Stop");


            return result;
        }

        /// <summary>
        /// Restarts the Parallel Processing Engine
        /// </summary>
        /// <returns></returns>
        public Boolean Restart()
        {
            Boolean successFlag = this.Stop();
            successFlag = successFlag & this.Start(false);
            return successFlag & UpdateEngineStatus(true, null, null);
        }

        /// <summary>
        /// Create single ton instance of parallel processing engine
        /// </summary>
        /// <returns></returns>
        public static ParallelProcessingEngine GetSingleton()
        {
            if (_instance == null)
            {
                lock (lockObj)
                {
                    if (_instance == null)
                    {
                        _instance = new ParallelProcessingEngine();
                    }
                }
            }

            return _instance;
        }

        /// <summary>
        /// Get the status of ParallelizationEngine and data processors in it.
        /// </summary>
        /// <returns>Status of ParallelizationEngine</returns>
        public static ParallelizationEngineStatus GetStatus()
        {
            ParallelProcessingEngine parallelProcessingEngine = GetSingleton();

            ParallelizationEngineStatus status = new ParallelizationEngineStatus
                {
                    IsParallizationProcessingEngineStarted =
                        parallelProcessingEngine._isParallizationProcessingEngineStarted,
                    ProcessorCount = parallelProcessingEngine.ProcessorCount
                };

            //Update engine status

            //Get all the processors in engine
            ConcurrentDictionary<String, IDataProcessor> processors = DataProcessorFactory.GetProcessors();

            if (processors != null)
            {
                DateTime? queueProcessorLastPollTime = null;
                Int32 queueProcessorInterval = 0;
                Int32 queueProcessorDatasize = 0;
                Int64 queueProcessorLastResultCount = 0;
                if (processors.ContainsKey(CoreDataProcessorList.EntityQueueProcessor.ToString())) // queue for most processors
                {
                    IDataProcessor dataProcessor = null;
                    Boolean success = processors.TryGetValue(CoreDataProcessorList.EntityQueueProcessor.ToString(), out dataProcessor);
                    if (success)
                    {
                        queueProcessorLastPollTime = dataProcessor.LastPollTime;
                        queueProcessorDatasize = dataProcessor.SourceDataBatchSize;
                        queueProcessorInterval = dataProcessor.DataPollingIntervalInSeconds;
                        queueProcessorLastResultCount = dataProcessor.LastResultItemCount;
                    }
                }

                String[] processorsWithEntityQueuePolling =
                    {
                        CoreDataProcessorList.EntityHierarchyProcessor.ToString(),
                        CoreDataProcessorList.EntityExtensionProcessor.ToString()
                    };

                String[] processorsWithOwnQueuePolling =
                    {
                        CoreDataProcessorList.DQMJobInitializationProcessor.ToString(),
                        CoreDataProcessorList.DQMJobFinalizationProcessor.ToString(),
                        CoreDataProcessorList.NormalizationProcessor.ToString(),
                        CoreDataProcessorList.SummarizationProcessor.ToString(),
                        CoreDataProcessorList.ValidationProcessor.ToString()
                    };

                SortedList sortedStatusList = new SortedList();

                //Get all processors and get the status from that.
                foreach (KeyValuePair<String, IDataProcessor> pair in processors)
                {
                    String processorName = pair.Key;
                    IDataProcessor processor = pair.Value;

                    DataProcessorStatus processorStatus = new DataProcessorStatus
                        {
                            ProcessorName = processorName,
                            IsInitialized = processor.IsInitialized,
                            LastPollTime = processor.LastPollTime,
                            LastResultItemCount = processor.LastResultItemCount,
                            PendingItemCount = processor.GetPendingItemCount(),
                            ThreadCount = processor.NumberOfThreads,
                            SourceDataBatchSize = processor.SourceDataBatchSize,
                            DataBufferThreshold = processor.MaxBufferedItemCount,
                            DataPollingIntervalInSeconds = processor.DataPollingIntervalInSeconds
                        };

                    if (processorsWithEntityQueuePolling.Any(x => x.Equals(processorName)))
                    {
                        processorStatus.LastPollTime = queueProcessorLastPollTime;
                        processorStatus.DataPollingIntervalInSeconds = queueProcessorInterval;
                        processorStatus.SourceDataBatchSize = queueProcessorDatasize;
                        processorStatus.LastResultItemCount = queueProcessorLastResultCount;
                    }

                    if (processorsWithOwnQueuePolling.Any(x => x.Equals(processorName)))
                    {
                        IDataProcessor dataProcessor;
                        Boolean success = processors.TryGetValue(processorName, out dataProcessor);
                        if (success)
                        {
                            processorStatus.LastPollTime = dataProcessor.LastPollTime;
                            processorStatus.DataPollingIntervalInSeconds = dataProcessor.DataPollingIntervalInSeconds;
                            processorStatus.SourceDataBatchSize = dataProcessor.SourceDataBatchSize;
                            processorStatus.LastResultItemCount = dataProcessor.LastResultItemCount;
                        }
                    }

                    sortedStatusList.Add(processorStatus.ProcessorName, processorStatus);

                    //status.DataProcessorStatusCollection.Add(processorStatus);
                }

                //Adding Diagnostic Data Processor status
                DataProcessorStatus diagProcessorStatus = new DataProcessorStatus
                {
                    ProcessorName = CoreDataProcessorList.DiaganosticDataProcessor.ToString(),
                    IsInitialized = true,
                    LastPollTime = DiagnosticDataProcessor.LastDataProcessedTime,
                    LastResultItemCount = DiagnosticDataProcessor.LastProcessedDataCount,
                    PendingItemCount = 0,
                    ThreadCount = 1,
                    SourceDataBatchSize = 200,
                    DataBufferThreshold = 200,
                    DataPollingIntervalInSeconds = 3
                };

                sortedStatusList.Add(diagProcessorStatus.ProcessorName, diagProcessorStatus);
                //status.DataProcessorStatusCollection.Add(diagProcessorStatus);

                for (int i = 0; i < sortedStatusList.Count; i++)
                    status.DataProcessorStatusCollection.Add((DataProcessorStatus)sortedStatusList.GetByIndex(i));
            }

            CheckForStoppedProcessors(status.DataProcessorStatusCollection);
            return status;
        }

        /// <summary>
        /// This method checks whether any of the processor is stopped,if yes, then it adds the old Value to the new status.
        /// </summary>
        /// <param name="dataProcessorStatusCollection"></param>
        private static void CheckForStoppedProcessors(DataProcessorStatusCollection dataProcessorStatusCollection)
        {
            MonitorBL monitorBL = new MonitorBL();
            DataProcessorStatusCollection oldDataProcessorStatusCollection = monitorBL.GetDataProcessorsStatus(System.Environment.MachineName, MDMServiceType.APIEngine, MDMServiceSubType.ParallelProcessingEngine, new CallerContext(MDMCenterApplication.MDMCenter, MDMCenterModules.Monitoring));
            if (oldDataProcessorStatusCollection != null)
            {
                foreach (DataProcessorStatus dataProcessorStatus in oldDataProcessorStatusCollection)
                {
                    if (!dataProcessorStatusCollection.Contains(dataProcessorStatus.ProcessorName))
                        dataProcessorStatusCollection.Add(dataProcessorStatus);
                }
            }
        }

        /// <summary>
        /// Stops the given processor
        /// </summary>
        /// <param name="dataProcessor">processor that needs to be stopped</param>
        /// <returns></returns>
        public Boolean StopProcessor(CoreDataProcessorList dataProcessor)
        {
            #region Logic
            //First get the processor configuration from the DB,if exists then update processor status to false.
            //if doesnt exist, then create new config for the processor and update all processor info along with processor status.
            //Stop the processor and remove from the processor from the collection. 
            //update the proessor status in Db
            #endregion Logic

            Boolean successFlag = true;
           
            IDataProcessor iDataProcessorProcessor = DataProcessorFactory.GetProcessor(dataProcessor);
            if (iDataProcessorProcessor != null)
            {
                DataProcessorConfigCollection dataProcessorConfigCollection = _monitorBL.GetProcessorConfigurationCollection(Environment.MachineName, MDMServiceType.APIEngine, MDMServiceSubType.ParallelProcessingEngine, new CallerContext(MDMCenterApplication.MDMCenter, MDMCenterModules.Monitoring));
                DataProcessorConfig dataProcessorConfig = GetUpdatedProcessorStatus(false, dataProcessor, iDataProcessorProcessor, dataProcessorConfigCollection);
                successFlag = successFlag & iDataProcessorProcessor.Complete();
                successFlag = successFlag & DataProcessorFactory.RemoveProcessor(iDataProcessorProcessor);
                if (successFlag)
                {
                    UpdateEngineStatus(true, dataProcessorConfigCollection, dataProcessorConfig);
                    SetUserName();
                    eventLogHandler.WriteInformationLog(String.Format("{0} stopped successfully by {1} at {2}. Processor Configurations while stopping are - BatchSize: {3}, No of Threads: {4}, Data polling interval (in seconds): {5}, Threshold: {6}",
                                                        iDataProcessorProcessor.Name,
                                                        _userName,
                                                        DateTime.Now.ToString("HH:mm:ss tt"),
                                                        iDataProcessorProcessor.SourceDataBatchSize,
                                                        iDataProcessorProcessor.NumberOfThreads,
                                                        iDataProcessorProcessor.DataPollingIntervalInSeconds,
                                                        iDataProcessorProcessor.MaxBufferedItemCount), 1);
                }
            }
            return successFlag;
        }

        /// <summary>
        /// Restarts the given processor
        /// </summary>
        /// <param name="dataProcessor">processor that needs to restarted</param>
        /// <returns></returns>
        public Boolean RestartProcessor(CoreDataProcessorList dataProcessor)
        {
            Boolean successFlag = true;
            try
            {
                successFlag = StopProcessor(dataProcessor);
            }
            catch
            {
                //Do nothing-Usually exception will be processor not initialized
            }

            return successFlag & StartProcessor(dataProcessor);
        }

        /// <summary>
        /// Starts the given processor
        /// </summary>
        /// <param name="dataProcessorList">processor that needs to be started</param>
        /// <param name="dataProcessorConfigCollection">dataconfigurarion</param>
        /// <returns></returns>
        private Boolean StartProcessor(List<CoreDataProcessorList> dataProcessorList, DataProcessorConfigCollection dataProcessorConfigCollection)
        {
            Boolean result = true;

            foreach (CoreDataProcessorList dataProcessor in dataProcessorList)
            {
                DataProcessorConfig oldConfig = GetProcessorConfig(dataProcessorConfigCollection, dataProcessor.ToString());
                result = this.StartDataProcessor(dataProcessor, false, oldConfig);

                #region Update Status in DB

                IDataProcessor iDataProcessorProcessor = DataProcessorFactory.GetProcessor(dataProcessor);

                if (iDataProcessorProcessor != null)
                {
                    DataProcessorConfig dataProcessorConfig = GetUpdatedProcessorStatus(true, dataProcessor, iDataProcessorProcessor, dataProcessorConfigCollection);
                    UpdateEngineStatus(true, dataProcessorConfigCollection, dataProcessorConfig);
                    SetUserName();
                    // The event message is now recorded inside the 'StartDataProcessor' method.
                }

                #endregion
            }
            return result;
        }

        /// <summary>
        /// Starts the given processor
        /// </summary>
        /// <param name="dataProcessor">processor that needs to be started</param>
        /// <returns></returns>
        public Boolean StartProcessor(CoreDataProcessorList dataProcessor)
        {
            Boolean result = true;
            DataProcessorConfigCollection dataProcessorConfigCollection = _monitorBL.GetProcessorConfigurationCollection(Environment.MachineName, MDMServiceType.APIEngine, MDMServiceSubType.ParallelProcessingEngine, new CallerContext(MDMCenterApplication.MDMCenter, MDMCenterModules.Monitoring));
            List<CoreDataProcessorList> queueProcessors = new List<CoreDataProcessorList> { dataProcessor };
            if (dataProcessor == CoreDataProcessorList.EntityQueueProcessor)
            {
                queueProcessors.Add(CoreDataProcessorList.EntityHierarchyProcessor);
                queueProcessors.Add(CoreDataProcessorList.EntityExtensionProcessor);
                result = StartProcessor(queueProcessors, dataProcessorConfigCollection);
            }
            else
            {
                result = StartProcessor(queueProcessors, dataProcessorConfigCollection);
            }
            this._isParallizationProcessingEngineStarted = true;
            return result;
        }

        #region Private Methods

        /// <summary>
        /// 
        /// </summary>
        private void SetUserName()
        {
            if (_securityPrincipal == null)
            {
                try
                {
                    _securityPrincipal = SecurityPrincipalHelper.GetCurrentSecurityPrincipal();
                    _userName = _securityPrincipal.CurrentUserName;
                }
                catch
                {
                    eventLogHandler.WriteErrorLog("Failed to load current user name so considering default username as 'System'", 1);
                }
            }
        }

        /// <summary>
        /// Starts DataProcessor
        /// </summary>
        /// <returns></returns>
        private Boolean StartDataProcessor(CoreDataProcessorList processor, Boolean checkPreviousStatusFromDb, DataProcessorConfig processorConfig)
        {
            String processorName = processorConfig != null ? processorConfig.ProcessorName : processor.ToString();
            Boolean isProcessorEnabled = AppConfigurationHelper.GetAppConfig<Boolean>(String.Format("MDMCenter.ParallelProcessingEngine.{0}.Enabled", processorName), false);
            Boolean isProcessorPollingEnabled = AppConfigurationHelper.GetAppConfig<Boolean>(String.Format("MDMCenter.ParallelProcessingEngine.{0}Polling.Enabled", processorName), true);
            if (checkPreviousStatusFromDb && processorConfig != null && !processorConfig.IsProcessorRunning)
            {
                LogMessage(processorName);
                return false;
            }

            if (!isProcessorEnabled)
            {
                LogProcessorDisabledMessage(processorName);
                return false;
            }

            ICoreDataProcessor processorInstance = null;

            if (Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("{0} starting..", processorName));
            }

            Boolean isProcessorInitialized = processorInstance != null && processorInstance.Initialize(isProcessorPollingEnabled, processorConfig);

            if (Constants.TRACING_ENABLED)
			{
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("{0} end..", processorName));
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("{0} initialize status : {1}", processorName, isProcessorInitialized));
			}

            IDataProcessor iDataProcessorProcessor = DataProcessorFactory.GetProcessor(processorName);

            LogProcessorStartMessage(iDataProcessorProcessor, _userName);

            return isProcessorInitialized;
        }

        /// <summary>
        /// Updates the Engine Status
        /// </summary>
        /// <param name="status"></param>
        /// <param name="dataProcessorConfigCollection"></param>
        /// <param name="newdataProcessorConfig"></param>
        private Boolean UpdateEngineStatus(Boolean status, DataProcessorConfigCollection dataProcessorConfigCollection, DataProcessorConfig newdataProcessorConfig)
        {
            if (dataProcessorConfigCollection == null)
                dataProcessorConfigCollection = _monitorBL.GetProcessorConfigurationCollection(Environment.MachineName, MDMServiceType.APIEngine, MDMServiceSubType.ParallelProcessingEngine, new CallerContext(MDMCenterApplication.MDMCenter, MDMCenterModules.Monitoring));
            if (dataProcessorConfigCollection == null)
            {
                dataProcessorConfigCollection = new DataProcessorConfigCollection
                    {
                        IsParallelProcessingEngineRunning = status
                    };
            }
            else
            {
                dataProcessorConfigCollection.IsParallelProcessingEngineRunning = status;
            }
            String processorConfigXml = dataProcessorConfigCollection.UpdateProcessorConfigXml(newdataProcessorConfig);
            Boolean result = _monitorBL.ProcessEngineStatus(Environment.MachineName, MDMServiceType.APIEngine, MDMServiceSubType.ParallelProcessingEngine, processorConfigXml, new CallerContext(MDMCenterApplication.MDMCenter, MDMCenterModules.Monitoring));
            return result;
        }

        /// <summary>
        /// Gets the Processor status from db, if exists update the processor status, else creates new processorconfig object and assigns the value from the processor 
        /// </summary>
        /// <param name="status">Status to be updated (processor running or not</param>
        /// <param name="processorName">name of the processor</param>
        /// <param name="iDataProcessorProcessor">processor object</param>
        /// <param name="dataProcessorConfigCollection">processor config collection</param>
        /// <returns></returns>
        private DataProcessorConfig GetUpdatedProcessorStatus(Boolean status, CoreDataProcessorList processorName, IDataProcessor iDataProcessorProcessor, DataProcessorConfigCollection dataProcessorConfigCollection)
        {
            DataProcessorConfig dataProcessorConfig = null;
            if (dataProcessorConfigCollection != null)
                dataProcessorConfig = dataProcessorConfigCollection.GetDataProcessorConfig(processorName.ToString());

            if (dataProcessorConfig == null)
            {
                dataProcessorConfig = new DataProcessorConfig
                {
                    ProcessorName = iDataProcessorProcessor.Name,
                    IsProcessorRunning = status,
                    DataPollingIntervalInSeconds = iDataProcessorProcessor.DataPollingIntervalInSeconds,
                    ThreadCount = iDataProcessorProcessor.NumberOfThreads,
                    DataBufferThreshold = iDataProcessorProcessor.MaxBufferedItemCount,
                    SourceDataBatchSize = iDataProcessorProcessor.SourceDataBatchSize
                };
            }
            else
            {
                dataProcessorConfig.IsProcessorRunning = status;
            }
            return dataProcessorConfig;
        }

        /// <summary>
        /// checks whether the engine was previously stopped or not
        /// </summary>
        /// <returns>true- if it was stopped</returns>
        private Boolean WasEngineStoppedManuallyLastTime(DataProcessorConfigCollection dataProcessorConfigCollection)
        {
            if (dataProcessorConfigCollection == null)
                return false;
            return !dataProcessorConfigCollection.IsParallelProcessingEngineRunning;
        }

        /// <summary>
        /// Log message 
        /// </summary>
        /// <param name="processorName">processor name</param>
        private static void LogMessage(String processorName)
        {
            String message = processorName + " was stopped previously.To restart the " + processorName + ", please click on reset button in the Server Status page";

            MDMTraceHelper.EmitTraceEvent(TraceEventType.Warning, message, MDMTraceSource.ParallelProcessingEngine);

            EventLogHandler eventLogHander = new EventLogHandler();
            eventLogHander.WriteWarningLog(message, 104);
        }

        private static void LogProcessorDisabledMessage(String processorName)
        {
            String message = processorName + " is disabled on this server.To enable the " + processorName + " please use the Server Status page.";

            MDMTraceHelper.EmitTraceEvent(TraceEventType.Warning, message, MDMTraceSource.ParallelProcessingEngine);
            
            EventLogHandler eventLogHander = new EventLogHandler();
            eventLogHander.WriteWarningLog(message, 103);
        }

        private static void LogProcessorStartMessage(IDataProcessor iDataProcessorProcessor, String userName)
        {
            EventLogHandler eventLogHandler = new EventLogHandler();

            if (iDataProcessorProcessor != null)
            {
                String message = String.Format(
                    "{0} started successfully by {1} at {2}. Processor Configurations when started are - BatchSize: {3}, No of Threads: {4}, Data polling interval (in seconds): {5}, Threshold: {6}",
                    iDataProcessorProcessor.Name,
                    userName,
                    DateTime.Now.ToString("HH:mm:ss tt"),
                    iDataProcessorProcessor.SourceDataBatchSize,
                    iDataProcessorProcessor.NumberOfThreads,
                    iDataProcessorProcessor.DataPollingIntervalInSeconds,
                    iDataProcessorProcessor.MaxBufferedItemCount);

                eventLogHandler.WriteInformationLog(message, 102);
            }
            else
            {
                eventLogHandler.WriteInformationLog("Data processor configuration is null", 102);
            }
        }

        /// <summary>
        /// Gets the config for the given processor from the collection
        /// </summary>
        /// <param name="dataProcessorConfigCollection">data processor config collection</param>
        /// <param name="dataProcessor">data processor</param>
        /// <returns>null- if there is no config else returns config</returns>
        private DataProcessorConfig GetProcessorConfig(DataProcessorConfigCollection dataProcessorConfigCollection, String dataProcessor)
        {
            DataProcessorConfig dataProcessorConfig = null;
            if (dataProcessorConfigCollection != null)
                dataProcessorConfig = dataProcessorConfigCollection.GetDataProcessorConfig(dataProcessor);
            return dataProcessorConfig;
        }

        /// <summary>
        /// Checks MDM Feature configurations status by application, modulePath and version
        /// </summary>
        /// <param name="application">Application under MDMCenter</param>
        /// <param name="modulePath">Module Path of MDMCenter Application</param>
        /// <param name="version">Version of feature</param>
        /// <returns>MDM Feature configurations based on application, modulePath and version</returns>
        private Boolean CheckIfMDMFeatureIsEnabled(MDMCenterApplication application, String modulePath, String version)
        {
            MDMFeatureConfig dqmFeatureConfig = MDMFeatureConfigHelper.GetMDMFeatureConfig(application, modulePath, version);
            return dqmFeatureConfig != null && dqmFeatureConfig.IsEnabled;
        }

        /// <summary>
        /// Returns all the core data processors in the system
        /// </summary>
        /// <returns></returns>
        private List<CoreDataProcessorList> GetAllProcessors()
        {
            List<CoreDataProcessorList> coreDataProcessorList = new List<CoreDataProcessorList>
               {
                   CoreDataProcessorList.EntityActivityLogProcessor,
                   CoreDataProcessorList.EntityCacheProcessor,
                   CoreDataProcessorList.StronglyTypedEntityCacheProcessor, //[Phani]TODO: Need feature toggle for this
                   CoreDataProcessorList.EntityFamilyProcessor,
                   CoreDataProcessorList.PromoteProcessor,
                   CoreDataProcessorList.DDGProcessor,
                   CoreDataProcessorList.MetadataChangeProcessor
               };

            #region DQM Processors
            
            if (CheckIfMDMFeatureIsEnabled(MDMCenterApplication.DataQualityManagement, "DQM Matching", "1"))
            {
				coreDataProcessorList.Add(CoreDataProcessorList.JigsawDataSynchronizationProcessor);
			}
            
            #endregion

            if (CheckIfMDMFeatureIsEnabled(MDMCenterApplication.MDMCenter, "IntegrationFramework", "1"))
            {
                coreDataProcessorList.Add(CoreDataProcessorList.IntegrationQualifyingQueueLoadProcessor);
                coreDataProcessorList.Add(CoreDataProcessorList.IntegrationQualifyingQueueProcessor);
                coreDataProcessorList.Add(CoreDataProcessorList.IntegrationOutboundQueueProcessor);
                coreDataProcessorList.Add(CoreDataProcessorList.IntegrationInboundQueueProcessor);
                coreDataProcessorList.Add(CoreDataProcessorList.IntegrationOutboundAggregationQueueProcessor);
                coreDataProcessorList.Add(CoreDataProcessorList.IntegrationInboundAggregationQueueProcessor);
            }
            return coreDataProcessorList;
        }

        #endregion

        #endregion
    }
}
