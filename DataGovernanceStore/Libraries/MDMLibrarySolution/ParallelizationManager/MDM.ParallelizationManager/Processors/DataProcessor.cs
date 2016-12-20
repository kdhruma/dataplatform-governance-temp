using System;
using System.Linq;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using System.Timers;

namespace MDM.ParallelizationManager.Processors
{
    using Core;
    using BusinessObjects;
    using Interfaces;
    using MDM.BusinessObjects.Diagnostics;
    using MDM.Utility;

    /// <summary>
    /// MDM Parallel data processor provides ability to run specified MDM operations in parallel.
    /// This processor consist of its own input queue, action workers and data source poller.
    /// </summary>
    public class DataProcessor : IDataProcessor
    {
        #region Fields

        /// <summary>
        /// 
        /// </summary>
        private String _name = String.Empty;

        /// <summary>
        /// 
        /// </summary>
        private Int32 _numberOfThreads = 1;

        /// <summary>
        /// 
        /// </summary>
        private TaskAction _taskAction = null;

        /// <summary>
        /// 
        /// </summary>
        private ActionBlock<MDMMessagePackage> _processActionBlock = null;

        /// <summary>
        /// 
        /// </summary>
        private CancellationTokenSource _cancellationTokenSource = null;

        /// <summary>
        /// 
        /// </summary>
        private IDataProcessorSource _dataProcessorSource = null;

        /// <summary>
        /// 
        /// </summary>
        private Boolean _isInitialized = false;

        /// <summary>
        /// 
        /// </summary>
        private Boolean _isPollingEnabled = false;

        /// <summary>
        /// 
        /// </summary>
        private Int32 _dataPollingIntervalInSeconds = 30;

        /// <summary>
        /// 
        /// </summary>
        private Int32 _maxBufferedItemCount = 450;

        /// <summary>
        /// No. of items to read from database for each batch.
        /// </summary>
        private Int32 _sourceDataBatchSize = 0;

        /// <summary>
        /// Counter for the number of no data calls
        /// </summary>
        private Int32 _numberOfNoDataCalls = 0;

        /// <summary>
        /// Threshold for number of consecutive no data calls
        /// </summary>
        private Int32 _maximumNumberOfConsecutiveNoDataCalls = 5;

        /// <summary>
        /// Polling interval during the throttled period
        /// </summary>
        private Int32 _throttledPollingIntervalInSeconds = 60;

        #region Monitoring fields

        private DateTime? _lastPollTime = null;

        private Int64 _lastResultItemCount = -1;

        #endregion Monitoring fields

        #region Local Fields

        /// <summary>
        /// 
        /// </summary>
        private ConcurrentExclusiveSchedulerPair _concurrentExclusiveSchedulerPair = null;

        /// <summary>
        /// 
        /// </summary>
        private System.Timers.Timer _dataPollingTimer;

        #endregion

        #endregion

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public String Name
        {
            get { return _name; }
        }

        /// <summary>
        /// 
        /// </summary>
        public Int32 NumberOfThreads
        {
            get { return _numberOfThreads; }
        }

        /// <summary>
        /// 
        /// </summary>
        public TaskAction TaskAction
        {
            get { return _taskAction; }
        }

        /// <summary>
        /// 
        /// </summary>
        public ActionBlock<MDMMessagePackage> ProcessActionBlock
        {
            get { return _processActionBlock; }
        }

        /// <summary>
        /// 
        /// </summary>
        public CancellationTokenSource CancellationTokenSource
        {
            get { return _cancellationTokenSource; }
        }

        /// <summary>
        /// 
        /// </summary>
        public IDataProcessorSource DataProcessorSource
        {
            get { return _dataProcessorSource; }
        }

        /// <summary>
        /// 
        /// </summary>
        public Boolean IsInitialized
        {
            get { return _isInitialized; }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsPollingEnabled
        {
            get { return _isPollingEnabled; }
        }

        /// <summary>
        /// 
        /// </summary>
        public Int32 DataPollingIntervalInSeconds
        {
            get { return _dataPollingIntervalInSeconds; }
        }

        /// <summary>
        /// 
        /// </summary>
        public Int32 MaxBufferedItemCount
        {
            get { return _maxBufferedItemCount; }
        }

        /// <summary>
        /// No. of items to read from database for each batch.
        /// </summary>
        public Int32 SourceDataBatchSize
        {
            get { return _sourceDataBatchSize; }
            set { this._sourceDataBatchSize = value; }
        }

        #region Monitoring  Properties

        public DateTime? LastPollTime
        {
            get { return _lastPollTime; }
            set { _lastPollTime = value; }
        }

        public Int64 LastResultItemCount
        {
            get { return _lastResultItemCount; }
            set { _lastResultItemCount = value; }
        }

        #endregion Monitoring Properties

        #endregion

        #region Constructors

        /// <summary>
        /// 
        /// </summary>
        /// <param name="processorName"></param>
        public DataProcessor(String processorName)
        {
            this._name = processorName;
        }

        #endregion

        #region Methods

        #region Initialize

        /// <summary>
        /// 
        /// </summary>
        /// <param name="taskAction"></param>
        /// <param name="taskCallbackAction"></param>
        /// <param name="cancellationTokenSource"></param>
        /// <param name="numberOfThreads"></param>
        /// <returns></returns>
        public Boolean Initialize(TaskAction taskAction, CancellationTokenSource cancellationTokenSource, Int32 numberOfThreads)
        {
            var diagnosticActivity = new DiagnosticActivity();

            if (Constants.TRACING_ENABLED)
            {
                ExecutionContext executionContext = new ExecutionContext(MDMTraceSource.ParallelProcessingEngine);
                diagnosticActivity.Start(executionContext);
                diagnosticActivity.LogInformation("DataProcessorName: " + this._name);
            }

            this._taskAction = taskAction;
            //this._taskCallbackAction = taskCallbackAction;

            if (cancellationTokenSource == null)
                this._cancellationTokenSource = new CancellationTokenSource();
            else
                this._cancellationTokenSource = cancellationTokenSource;

            this._numberOfThreads = numberOfThreads;

            _concurrentExclusiveSchedulerPair = new ConcurrentExclusiveSchedulerPair(TaskScheduler.Default, _numberOfThreads);

            var executionDataflowBlockOptions = new ExecutionDataflowBlockOptions
                {
                    MaxDegreeOfParallelism = _numberOfThreads,
                    TaskScheduler = _concurrentExclusiveSchedulerPair.ConcurrentScheduler
                };

            var processAction = new Action<MDMMessagePackage>(taskAction);
            _processActionBlock = new ActionBlock<MDMMessagePackage>(processAction, executionDataflowBlockOptions);

            this._isInitialized = true;

            if (Constants.TRACING_ENABLED)
            {
                diagnosticActivity.Stop();
            }

            return _isInitialized;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataProcessorSource"></param>
        /// <param name="dataPollingIntervalInSeconds"></param>
        /// <param name="maxBufferedItemCount"></param>
        /// <returns></returns>
        public Boolean InitializePolling(IDataProcessorSource dataProcessorSource, int dataPollingIntervalInSeconds, int maxBufferedItemCount)
        {
            var diagnosticActivity = new DiagnosticActivity();

            if (Constants.TRACING_ENABLED)
            {
                ExecutionContext executionContext = new ExecutionContext(MDMTraceSource.ParallelProcessingEngine);
                diagnosticActivity.Start(executionContext);
                diagnosticActivity.LogInformation("DataProcessorName: " + this._name);
            }
            
            var returnFlag = false;

            this._dataProcessorSource = dataProcessorSource;
            this._dataPollingIntervalInSeconds = dataPollingIntervalInSeconds;
            this._maxBufferedItemCount = maxBufferedItemCount;

            //Initialize and start message polling task
            if (this._dataProcessorSource != null)
            {
                Task.Factory.StartNew(StartPollingTimer, this._cancellationTokenSource.Token, TaskCreationOptions.LongRunning, TaskScheduler.Current);               
                this._isPollingEnabled = true;
                returnFlag = true;
            }

            if (Constants.TRACING_ENABLED)
            {
                diagnosticActivity.Stop();
            }

            return returnFlag;
        }

        #endregion

        #region Operations

        /// <summary>
        /// 
        /// </summary>
        /// <param name="messagePackage"></param>
        /// <returns></returns>
        public Boolean Post(MDMMessagePackage messagePackage)
        {
            if (!this._isInitialized)
            {
                throw new ApplicationException("Data processor with name:" + this.Name + " is not yet initialized. Please initialize processor before queing data");
            }

            this._processActionBlock.Post(messagePackage);

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="messagePackage"></param>
        /// <returns></returns>
        public Boolean PostAsync(MDMMessagePackage messagePackage)
        {
            if (!this._isInitialized)
            {
                throw new ApplicationException("Data processor with name:" + this.Name + " is not yet initialized. Please initialize processor before queing data");
            }

            this._processActionBlock.SendAsync(messagePackage);

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Boolean Complete()
        {
            return Complete(0);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual Boolean Complete(Int32 waitTimeoutInMiliseconds)
        {
            return Complete(waitTimeoutInMiliseconds, true);
        }

        /// <summary>
        /// Completes the data processor
        /// </summary>
        /// <param name="waitTimeoutInMiliseconds">Wait timeout in milliseconds</param>
        /// <param name="cancelJob">Indicates that current job of data processor will be canceled</param>
        /// <returns>Returns <b>true</b> if completed execution within the allotted time; otherwise, <b>false</b>.</returns>
        public virtual Boolean Complete(Int32 waitTimeoutInMiliseconds, Boolean cancelJob)
        {
            var diagnosticActivity = new DiagnosticActivity();

            try
            {
                if (Constants.TRACING_ENABLED)
                {
                    ExecutionContext executionContext = new ExecutionContext(MDMTraceSource.ParallelProcessingEngine);
                    diagnosticActivity.Start(executionContext);
                    diagnosticActivity.LogInformation("DataProcessorName: " + this._name);
                }

                // the order is , stop the poller first so no new messages are retrieved.
                // Complete the action block
                // Wait until it clears the current queue for a given period of time.
                if (cancelJob)
                {
                    this._cancellationTokenSource.Cancel(); //Stops the poller
                }
                this._processActionBlock.Complete();//Any task pending will completes it.

                if (waitTimeoutInMiliseconds > 0)
                    _processActionBlock.Completion.Wait(waitTimeoutInMiliseconds);
                else
                    _processActionBlock.Completion.Wait();

                return true;
            }
            catch (AggregateException exception)
            {
                foreach (Exception ex in exception.InnerExceptions)
                {
                    diagnosticActivity.LogError(ex.ToString());
                }
            }
            catch (Exception exception)
            {
                diagnosticActivity.LogError(exception.ToString());
            }
            finally
            {
                if (Constants.TRACING_ENABLED)
                {
                    diagnosticActivity.Stop();
                }
            }

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Int64 GetPendingItemCount()
        {
            return this._processActionBlock.InputCount;
        }

        #endregion

        #region Private Methods

        #region Polling

        /// <summary>
        /// 
        /// </summary>
        private void StartPollingTimer()
        {
            var diagnosticActivity = new DiagnosticActivity();

            if (Constants.TRACING_ENABLED)
            {
                ExecutionContext executionContext = new ExecutionContext(MDMTraceSource.ParallelProcessingEngine);
                diagnosticActivity.Start(executionContext);
                diagnosticActivity.LogInformation("DataProcessorName: " + this._name);
            }

            var intervalInMillSecs = this._dataPollingIntervalInSeconds * 1000;

            _maximumNumberOfConsecutiveNoDataCalls = AppConfigurationHelper.GetAppConfig<Int32>(
                                                    "MDMCenter.ParallelProcessingEngine.IdleTime.NumberOfNoDataCalls",
                                                    _maximumNumberOfConsecutiveNoDataCalls);

            _throttledPollingIntervalInSeconds = AppConfigurationHelper.GetAppConfig<Int32>(
                                                    "MDMCenter.ParallelProcessingEngine.IdleTime.ThrottledPollingIntervalInSeconds",
                                                    _throttledPollingIntervalInSeconds);
            if (intervalInMillSecs > 0)
            {
                _dataPollingTimer = new System.Timers.Timer(intervalInMillSecs);
                _dataPollingTimer.Elapsed += ProduceMessages;

                //Call produce messages for first time before enabling timer
                ProduceMessages(null, null);

                _dataPollingTimer.Enabled = true;
            }
            else if(Constants.TRACING_ENABLED)
            {
                diagnosticActivity.LogError("Failed to start elapsed timer instances for polling as polling interval is not defined or < 1ms for data processor:" + Name);
            }

            if (Constants.TRACING_ENABLED)
                    {
                diagnosticActivity.Stop();
            }
                    }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ProduceMessages(Object sender, ElapsedEventArgs e)
        {
            _dataPollingTimer.Enabled = false;

            var isCancellationRequested = false;

            var itemsSubmitted = 0;
            MDMMessagePackageCollection messagePackageCollection = null;

            try
            {
                if (this._cancellationTokenSource.IsCancellationRequested) //Cancel the producer if cancellation is requested.
                {
                    isCancellationRequested = true;
                }
                else if (this.GetPendingItemCount() <= this._maxBufferedItemCount) //Check if queue has reached max threshold..load next batch only when not..
                {
                    messagePackageCollection = _dataProcessorSource.GetNextBatch(_dataProcessorSource.BatchSize);
                    this.LastPollTime = DateTime.Now;
                    this.LastResultItemCount = messagePackageCollection.Count;

                    if (messagePackageCollection == null || messagePackageCollection.Count == 0)
                    {
                        // we did not get anything from the DB..count how many times
                        _numberOfNoDataCalls++;

                        // if we reached a threshold..increase the polling...
                        if (_numberOfNoDataCalls > _maximumNumberOfConsecutiveNoDataCalls)
                        {
                            // if the polling is less than a throttled interval..make it a minute.otherwise..it is high enough a number.already..
                            if (_dataPollingTimer.Interval < _throttledPollingIntervalInSeconds * 1000)
                            {
                                _dataPollingTimer.Interval = _throttledPollingIntervalInSeconds * 1000;
                            }

                            // Reset the zero calls
                            _numberOfNoDataCalls = 0;
                        }
                        // dont continue the process...
                        return;
                    }

                    // if we get data..back to normal..
                    _numberOfNoDataCalls = 0;
                    // reset the interval also..
                    if (_dataPollingTimer.Interval != this._dataPollingIntervalInSeconds * 1000)
                    {
                        _dataPollingTimer.Interval = this._dataPollingIntervalInSeconds * 1000;
                    }

                    foreach (MDMMessagePackage messagePackage in messagePackageCollection)
                    {
                        Post(messagePackage);
                        itemsSubmitted++;
                    }
                }
                }
                catch (Exception exception)
                {
                #region Log processor error

                    try
                    {
                    var failedMessagePackageCollection = new MDMMessagePackageCollection();

                        if (messagePackageCollection != null)
                        {
                            for (Int32 i = itemsSubmitted; i < messagePackageCollection.Count; i++)
                            {
                                MDMMessagePackage messagePackage = messagePackageCollection.ElementAt(i);
                                failedMessagePackageCollection.Add(messagePackage);
                            }

                            if (failedMessagePackageCollection.Count > 0)
                            {
                                _dataProcessorSource.HandleException(failedMessagePackageCollection, exception);
                            }
                        }

                    //V:MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, String.Format("DataProcessor: {0}:ProduceMessages failed. Exception:{1}", this._name, exception.ToString()), MDMTraceSource.ParallelProcessingEngine);
                    }
                    catch (Exception)
                    {
                    //V:MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, String.Format("DataProcessor: {0}:Failed to log the Error. Exception:{1}", this._name, exception.ToString()), MDMTraceSource.ParallelProcessingEngine);
                    }

                #endregion
                }
            finally
            {
                if (!isCancellationRequested)
                    _dataPollingTimer.Enabled = true;
            }
        }

        #endregion

        #endregion

        #endregion
    }
}
