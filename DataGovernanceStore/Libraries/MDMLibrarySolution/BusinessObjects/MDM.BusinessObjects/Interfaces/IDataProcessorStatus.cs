using System;

namespace MDM.Interfaces
{
    using MDM.Core;

    /// <summary>
    /// Exposes methods or properties to set or get data processor status.
    /// </summary>
    public interface IDataProcessorStatus 
    {
        /// <summary>
        /// Property Denoting whether processor is initialized or not
        /// </summary>
        bool IsInitialized { get; set; }

        /// <summary>
        /// Property Denoting LastPollTime of Processor
        /// </summary>
        DateTime? LastPollTime { get; set; }

        /// <summary>
        /// Property Denoting Last ResultItem Count
        /// </summary>
        long LastResultItemCount { get; set; }

        /// <summary>
        /// Property Denoting Pending Item Count
        /// </summary>
        long PendingItemCount { get; set; }

        /// <summary>
        /// Property Denoting Processor Name
        /// </summary>
        string ProcessorName { get; set; }

        /// <summary>
        /// Property Denoting Batch Size of Source Data
        /// </summary>
        Int32 SourceDataBatchSize { get; set; }

        /// <summary>
        /// Property Denoting DataBufferThreshold
        /// </summary>
        Int32 DataBufferThreshold { get; set; }

        /// <summary>
        /// Property Denoting Thread count
        /// </summary>
        Int32 ThreadCount { get; set; }

        /// <summary>
        /// Property Denoting DataPollingInterval in Seconds
        /// </summary>
        Int32 DataPollingIntervalInSeconds { get; set; }

        /// <summary>
        /// XML representaion of an Object
        /// </summary>
        /// <returns>XML representation of an Object</returns>
        string ToXml();
    }
}
