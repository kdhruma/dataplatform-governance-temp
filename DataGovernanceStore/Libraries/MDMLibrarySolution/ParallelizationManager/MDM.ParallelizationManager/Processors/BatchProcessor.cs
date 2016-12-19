using System;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace MDM.ParallelizationManager.Processors
{
    /// <summary>
    /// MDM Parallel data processor provides ability to run specified MDM operations in collect and batch mode.
    /// This processor consist of its own input queue, action workers and data source poller.
    /// </summary>
    public class BatchProcessor<T> : IDataflowBlock
    {
        #region Fields

        /// <summary>
        /// 
        /// </summary>
        private readonly Action<T[]> _processAction = null;

        /// <summary>
        /// 
        /// </summary>
        private readonly CancellationTokenSource _cancellationTokenSource = null;

        /// <summary>
        /// 
        /// </summary>
        private BufferBlock<T> _dataBufferBlock = null;

        private Task batchCompletion;

        /// <summary>
        /// 
        /// </summary>
        private readonly Int32 _batchSize = 10;

        /// <summary>
        /// 
        /// </summary>
        private readonly Int32 _batchTimeoutInMilliSeconds = 2000;

        /// <summary>
        /// 
        /// </summary>
        private readonly Int32 _threadCount = 1;

        #endregion

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public CancellationTokenSource CancellationTokenSource
        {
            get { return _cancellationTokenSource; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initialize new instance of BatchProcessor class
        /// </summary>
        /// <param name="processAction">Action to process batch data</param>
        /// <param name="cancellationTokenSource">Cancellation token</param>
        /// <param name="batchSize">Batch size</param>
        /// <param name="batchTimeoutInMilliseconds">Batch timeout in milliseconds</param>
        /// <param name="threadCount">Consumer thread count</param>
        public BatchProcessor(Action<T[]> processAction, CancellationTokenSource cancellationTokenSource, Int32 batchSize, Int32 batchTimeoutInMilliseconds, Int32 threadCount = 1)
        {
            _processAction = processAction;
            _cancellationTokenSource = cancellationTokenSource;
            _batchSize = batchSize;
            _batchTimeoutInMilliSeconds = batchTimeoutInMilliseconds;
            _threadCount = threadCount;

            Initialize();
        }

        #endregion

        #region Methods

        #region Initialize

        /// <summary>
        /// 
        /// </summary>
        private void Initialize()
        {
            _dataBufferBlock = new BufferBlock<T>(new DataflowBlockOptions
            {
                CancellationToken = CancellationTokenSource.Token,
                BoundedCapacity = DataflowBlockOptions.Unbounded
            });

            BatchBlock<T> dataBatchBlock = new BatchBlock<T>(_batchSize, new GroupingDataflowBlockOptions
            {
                CancellationToken = CancellationTokenSource.Token,
                BoundedCapacity = DataflowBlockOptions.Unbounded,
                Greedy = true
            });

            var executionDataflowBlockOptions = new ExecutionDataflowBlockOptions
            {
                MaxDegreeOfParallelism = _threadCount,
            };

            var processActionBlock = new ActionBlock<T[]>(_processAction, executionDataflowBlockOptions);

            dataBatchBlock.LinkTo(processActionBlock);

            // When the batch block completes, set the action block also to complete.
            dataBatchBlock.Completion.ContinueWith(_ => processActionBlock.Complete(), CancellationTokenSource.Token);

            // Create a deactivated timer that can trigger the KPIDataBatchBlock
            TimeSpan triggerAfter = TimeSpan.FromMilliseconds(_batchTimeoutInMilliSeconds);
            Timer timer = new Timer(_ => dataBatchBlock.TriggerBatch());
            
            // Create the transformation callback
            Func<T, T> resetTimerIdentity = value =>
            {
                timer.Change((Int64)triggerAfter.TotalMilliseconds, Timeout.Infinite);
                return value;
            };

            // Create the target end and link it to the source end
            TransformBlock<T, T> timingBlock = new TransformBlock<T, T>(resetTimerIdentity);
            timingBlock.LinkTo(dataBatchBlock);

            // Link sources to the timing block, not directly to the batch block
            _dataBufferBlock.LinkTo(timingBlock);

            _dataBufferBlock.Completion.ContinueWith(task => timingBlock.Complete());
            timingBlock.Completion.ContinueWith(task => dataBatchBlock.Complete());
            dataBatchBlock.Completion.ContinueWith(task => processActionBlock.Complete());
            batchCompletion = processActionBlock.Completion;
        }

        /// <summary>
        /// Gets count of pending items
        /// </summary>
        /// <returns>Returns number of item in buffer</returns>
        public Int64 GetPendingItemCount()
        {
            return _dataBufferBlock.Count;
        }

        #endregion

        #region Operations

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public Boolean Post(T data)
        {
            return this._dataBufferBlock.Post(data);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public Boolean PostAsync(T data)
        {
            this._dataBufferBlock.SendAsync(data);
            return true;
        }

        #endregion

        #endregion

        #region Implementation of IDataflowBlock

        /// <summary>
        /// Signals to the <see cref="T:System.Threading.Tasks.Dataflow.IDataflowBlock"/> that it should not accept nor produce any more messages nor consume any more postponed messages.
        /// </summary>
        public void Complete()
        {
            _dataBufferBlock.Complete();
        }

        /// <summary>
        /// Causes the <see cref="T:System.Threading.Tasks.Dataflow.IDataflowBlock"/> to complete in a <see cref="F:System.Threading.Tasks.TaskStatus.Faulted"/> state.
        /// </summary>
        /// <param name="exception">The <see cref="T:System.Exception"/> that caused the faulting.</param><exception cref="T:System.ArgumentNullException">The <paramref name="exception"/> is null.</exception>
        public void Fault(Exception exception)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Gets a <see cref="T:System.Threading.Tasks.Task"/> that represents the asynchronous operation and completion of the dataflow block.
        /// </summary>
        /// <returns>
        /// The task.
        /// </returns>
        public Task Completion
        {
            get { return batchCompletion; }
        }

        #endregion
    }
}
