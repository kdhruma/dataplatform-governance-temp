using System;

namespace MDM.Interfaces
{
    using MDM.Core;

    /// <summary>
    /// Exposes methods or properties used for qualifying message queue item.
    /// </summary>
    public interface IQualifyingQueueItem : IIntegrationQueueItem
    {
        #region Properties

        /// <summary>
        /// Indicates status of message qualification
        /// </summary>
        MessageQualificationStatusEnum MessageQualificationStatus { get; set; }

        /// <summary>
        /// Indicates when a particular message is to be picked up for qualification process
        /// </summary>
        DateTime? ScheduledQualifierTime { get; set; }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Represents IQualifyingQueueItem in Xml format
        /// </summary>
        String ToXml();

        /// <summary>
        /// Clone the current instance and create new instance with same values.
        /// </summary>
        /// <returns>Cloned IQualifyingQueueItem</returns>
        IQualifyingQueueItem Clone();

        #endregion Methods
    }
}
