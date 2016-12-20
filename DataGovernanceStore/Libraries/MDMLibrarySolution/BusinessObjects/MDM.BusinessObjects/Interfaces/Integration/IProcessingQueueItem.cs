using System;

namespace MDM.Interfaces
{
    /// <summary>
    /// Exposes methods or properties to set or get outbound or inbound queue item (process).
    /// </summary>
    public interface IProcessingQueueItem : IIntegrationQueueItem
    {
        /// <summary>
        /// Indicates when a particular message is to be picked up for outbound or inbound process.
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
        IProcessingQueueItem Clone();

        #endregion Methods
    }
}
