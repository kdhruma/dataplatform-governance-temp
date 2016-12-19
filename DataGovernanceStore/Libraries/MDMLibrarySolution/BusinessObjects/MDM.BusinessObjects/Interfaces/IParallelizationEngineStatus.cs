using System;

namespace MDM.Interfaces
{
    /// <summary>
    /// Exposes methods or properties to set or get parallelization engine status.
    /// </summary>
    public interface IParallelizationEngineStatus
    {
        /// <summary>
        /// Property denoting ParallelizationEngineStatus has been started or not
        /// </summary>
        bool IsParallizationProcessingEngineStarted { get; set; }

        /// <summary>
        /// Property denoting Processor count
        /// </summary>
        int ProcessorCount { get; set; }

        /// <summary>
        /// XML presentation of Object
        /// </summary>
        /// <returns>XML presentation of Object</returns>
        string ToXml();
    }
}
