using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading;
using System.Threading.Tasks;

using System.Collections.Concurrent;

namespace MDM.ParallelizationManager.Interfaces
{
    using MDM.BusinessObjects;

    public interface IDataProcessorSource
    {
        Int32 BatchSize { get; }

        MDMMessagePackageCollection GetNextBatch(Int32 batchSize);

        void HandleException(MDMMessagePackageCollection failedMessagePackageCollection, Exception exception);
    }
}
