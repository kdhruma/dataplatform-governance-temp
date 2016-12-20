using System;

namespace MDM.Interfaces
{
    using System.Collections.ObjectModel;
    using MDM.Core;

    /// <summary>
    /// Exposes methods or properties to set or get integration message queue.
    /// </summary>
    public interface IIntegrationQueueItem
    {
        #region Properties

        /// <summary>
        /// Indicates Id of type of integration message
        /// </summary>
        Int64 Id { get; set; }

        /// <summary>
        /// Indicates action to be performed on the object
        /// </summary>
        ObjectAction Action { get; set; }

        /// <summary>
        /// Indicates Id of connector for which this message will be used
        /// </summary>
        Int16 ConnectorId { get; set; }

        /// <summary>
        /// Indicates name of connector for which this message will be used
        /// </summary>
        String ConnectorLongName { get; set; }

        /// <summary>
        /// Indicates Id of integration activity log for which this message is created
        /// </summary>
        Int64 IntegrationActivityLogId { get; set; }

        /// <summary>
        /// Indicates type of integration type
        /// </summary>
        IntegrationType IntegrationType { get; set; }

        /// <summary>
        /// Indicates type of message
        /// </summary>
        Int16 IntegrationMessageTypeId { get; set; }

        /// <summary>
        /// Indicates Long name of message type
        /// </summary>
        String IntegrationMessageTypeLongName { get; set; }

        /// <summary>
        /// Indicates if qualifying process in progress
        /// </summary>
        Boolean IsInProgress { get; set; }

        /// <summary>
        /// Indicates time when qualification process started
        /// </summary>
        DateTime? StartTime { get; set; }

        /// <summary>
        /// Indicates time when qualification process finished
        /// </summary>
        DateTime? EndTime { get; set; }

        /// <summary>
        /// Indicates which server qualified this record
        /// </summary>
        Int32 ServerId { get; set; }

        /// <summary>
        /// Indicates Weitage for qualification
        /// </summary>
        Int32 Weightage { get; set; }

        /// <summary>
        /// Indicates any comments qualification or process for given item
        /// </summary>
        Collection<String> Comments { get; set; }

        #endregion Properties
    }
}
