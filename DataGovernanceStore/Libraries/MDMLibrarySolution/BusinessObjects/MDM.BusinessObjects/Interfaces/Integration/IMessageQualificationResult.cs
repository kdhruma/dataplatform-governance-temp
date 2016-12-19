using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace MDM.Interfaces
{
    using MDM.Core;

    /// <summary>
    /// Exposes methods or properties to set or get message qualification result. It contains the message qualification status, scheduled qualification time and scheduled aggregation time.
    /// </summary>
    public interface IMessageQualificationResult
    {
        #region Properties

        /// <summary>
        /// Indicates if the message is qualified for further processing or not.
        /// </summary>
        MessageQualificationStatusEnum MessageQualificationStatus { get; set; }

        /// <summary>
        /// Indicates the next time slot when message should be picked up for qualification process. 
        /// If value is not given, then it will compute next scheduled time from ConnectorProfile
        /// </summary>
        DateTime? ScheduledQualificationTime { get; set; }

        /// <summary>
        /// Indicates the next time slot when message should be picked up for aggregation process. 
        /// If value is not given, then it will compute next scheduled time from ConnectorProfile
        /// </summary>
        DateTime? ScheduledAggregationTime { get; set; }

        /// <summary>
        /// Indicates the comments after the message qualification.
        /// </summary>
        Collection<String> Comments { get; set; }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Represents MessageQualificationResult in Xml format
        /// </summary>
        String ToXml();

        /// <summary>
        /// Clone the MessageQualificationResult and create a new MessageQualificationResult
        /// </summary>
        /// <returns>New MessageQualificationResult having same value as current one.</returns>
        IMessageQualificationResult Clone();

        #endregion Methods
    }
}
