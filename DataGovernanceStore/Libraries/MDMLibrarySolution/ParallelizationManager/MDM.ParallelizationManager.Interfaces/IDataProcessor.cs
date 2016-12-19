using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading;

namespace MDM.ParallelizationManager.Interfaces
{
    using MDM.BusinessObjects;

    public interface IDataProcessor
    {
        String Name { get;}

        Int32 NumberOfThreads { get;}

        Int32 DataPollingIntervalInSeconds { get; }

        Int32 MaxBufferedItemCount { get; }

        Boolean IsPollingEnabled { get; }

        TaskAction TaskAction { get; }

        CancellationTokenSource CancellationTokenSource { get; }

        Boolean IsInitialized { get; }

        DateTime? LastPollTime { get; }

        Int64 LastResultItemCount { get; }

        Boolean Initialize(TaskAction taskAction, CancellationTokenSource cancellationTokenSource, Int32 numberOfThreads);

        Boolean InitializePolling(IDataProcessorSource dataProcessorSource, Int32 dataPollingIntervalInSeconds, Int32 maxBufferedItemCount);

        Boolean Complete();

        Boolean Complete(Int32 waitTimeoutInMiliseconds);

        Int64 GetPendingItemCount();

        Boolean Post(MDMMessagePackage messagePackage);
        
        Boolean PostAsync(MDMMessagePackage messagePackage);

        Int32 SourceDataBatchSize { get; set; }
    }
}
