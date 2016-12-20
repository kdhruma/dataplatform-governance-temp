using System;

namespace MDM.Interfaces
{
    using MDM.Core;

    /// <summary>
    /// Exposes methods or properties to set or get data processor configuration.
    /// </summary>
    public interface IDataProcessorConfig 
    {
        /// <summary>
        /// Property denoting Processor Name
        /// </summary>
        String ProcessorName { get; set; }
        
        /// <summary>
        /// Property denoting SourceDataBatchSize
        /// </summary>
        Int32 SourceDataBatchSize { get; set; }
        
        /// <summary>
        /// Property denoting DataBufferThreshold
        /// </summary>
        Int32 DataBufferThreshold { get; set; }
        
        /// <summary>
        /// Property denoting ThreadCount
        /// </summary>
        Int32 ThreadCount { get; set; }
        
        /// <summary>
        /// Property denoting DataPollingIntervalInSeconds
        /// </summary>
        Int32 DataPollingIntervalInSeconds { get; set; }
        
        /// <summary>
        /// XML Representation of an Object
        /// </summary>
        /// <returns>XML Representation of an Object</returns>
        String ToXml();
    }
}

