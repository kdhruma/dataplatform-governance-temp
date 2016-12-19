using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace MDM.ParallelizationManager.Processors
{
    /// <summary>
    /// MDM Parallel collection processor provides ability to run specified MDM operation for each item in collection in parallel
    /// This processor consist action works and synchronizer to return result
    /// </summary>
    public class ParallelTaskProcessor
    {
        #region Fields

        #endregion

        #region Properties

        #endregion

        #region Constructors

        #endregion

        #region Methods

        #region Public Static Methods

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items"></param>
        /// <param name="action"></param>
        /// <param name="cancellationTokenSource"></param>
        /// <param name="maxDegreeOfParallelism"></param>
        /// <returns></returns>
        public Boolean RunInParallel<T>(Collection<T> items, Action<T> action, CancellationTokenSource cancellationTokenSource, Int32 maxDegreeOfParallelism)
        {
            cancellationTokenSource = cancellationTokenSource ?? new CancellationTokenSource();

            var concurrentExclusiveSchedulerPair = new ConcurrentExclusiveSchedulerPair(TaskScheduler.Default, maxDegreeOfParallelism);

            var concurrentProcessingDataflowBlockOptions = new ExecutionDataflowBlockOptions
            {
                MaxDegreeOfParallelism = maxDegreeOfParallelism,
                TaskScheduler = concurrentExclusiveSchedulerPair.ConcurrentScheduler,
                CancellationToken = cancellationTokenSource.Token,
                //NameFormat = "ConcurrentProcessionDataFlowBlockOption"
            };

            var actionBlock = new ActionBlock<T>(action, concurrentProcessingDataflowBlockOptions);

            foreach (var item in items)
            {
                actionBlock.Post(item);
            }

            actionBlock.Complete();
            actionBlock.Completion.Wait();

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <param name="items"></param>
        /// <param name="func"></param>
        /// <param name="cancellationTokenSource"></param>
        /// <param name="maxDegreeOfParallelism"></param>
        /// <returns></returns>
        public Collection<T2> RunInParallel<T1, T2>(Collection<T1> items, Func<T1, T2> func, CancellationTokenSource cancellationTokenSource, Int32 maxDegreeOfParallelism)
        {
            var outputCollection = new Collection<T2>();
            //cancellationTokenSource = cancellationTokenSource ?? new CancellationTokenSource();
            var concurrentExclusiveSchedulerPair = new ConcurrentExclusiveSchedulerPair(TaskScheduler.Default, maxDegreeOfParallelism);

            var concurrentProcessingDataflowBlockOptions = new ExecutionDataflowBlockOptions
                {
                    MaxDegreeOfParallelism = maxDegreeOfParallelism,
                    TaskScheduler = concurrentExclusiveSchedulerPair.ConcurrentScheduler
                    //CancellationToken = cancellationTokenSource.Token,
                    //NameFormat = "ConcurrentProcessionDataFlowBlockOption"
                };

             var exclusiveProcessigDataflowBlockOptions = new ExecutionDataflowBlockOptions
                {
                    MaxDegreeOfParallelism = 1,
                    TaskScheduler = concurrentExclusiveSchedulerPair.ExclusiveScheduler
                    //CancellationToken = cancellationTokenSource.Token,
                    //NameFormat = "ExclusiveProcessionDataFlowBlockOption",
                };

            var transformBlock = new TransformBlock<T1, T2>(func, concurrentProcessingDataflowBlockOptions);
            var actionBlock = new ActionBlock<T2>(item => outputCollection.Add(item), exclusiveProcessigDataflowBlockOptions);

            var dataflowLinkOptions = new DataflowLinkOptions {PropagateCompletion = true};
            transformBlock.LinkTo(actionBlock, dataflowLinkOptions);

            foreach (var item in items)
            {
                transformBlock.Post(item);
            }

            transformBlock.Complete();
            actionBlock.Completion.Wait();

            return outputCollection;
        }

        #endregion

        #region Private Methods

        #endregion

        #endregion
    }
}
