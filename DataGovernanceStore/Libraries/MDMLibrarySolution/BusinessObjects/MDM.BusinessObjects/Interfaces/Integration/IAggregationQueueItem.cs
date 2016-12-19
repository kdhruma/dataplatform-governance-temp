using System;

namespace MDM.Interfaces
{
    /// <summary>
    /// Exposes methods or properties to set or get the aggregation queue item.
    /// </summary>
    public interface IAggregationQueueItem : IIntegrationQueueItem
    {
        /// <summary>
        /// Indicates when a particular message is to be picked up for outbound/inbound process
        /// </summary>
        DateTime? ScheduledProcessTime { get; set; }

        #region Methods

        /// <summary>
        /// Represents outbound/inbound queue item in Xml format
        /// </summary>
        String ToXml();

        /// <summary>
        /// Clone the current instance and create new instance with same values.
        /// </summary>
        /// <returns>Cloned IProcessingQueueItem</returns>
        IAggregationQueueItem Clone();

        #endregion Methods
    }
}
