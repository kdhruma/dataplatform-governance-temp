using System;


namespace MDM.ParallelProcessingService.Interfaces
{
    using MDM.BusinessObjects;

    public interface ICoreDataProcessor
    {
        /// <summary>
        /// initialize the processor
        /// </summary>
        /// <param name="enablePolling">flag says the polling is enable for the processor</param>
        /// <param name="dataProcessorConfig">local config for the datasource. it will null if it starting for the first time</param>
        /// <returns>true- initialize is successful false- failed</returns>
        Boolean Initialize(Boolean enablePolling, DataProcessorConfig dataProcessorConfig);
    }
}
