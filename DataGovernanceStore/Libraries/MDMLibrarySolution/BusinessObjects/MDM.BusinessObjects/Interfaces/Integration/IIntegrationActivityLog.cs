using System;

namespace MDM.Interfaces
{
    using MDM.Core;

    /// <summary>
    /// Exposes methods or properties to set or get integration activity log related information.
    /// </summary>
    public interface IIntegrationActivityLog
    {
        #region Properties

        /// <summary>
        /// Indicates Id of integration activity log
        /// </summary>
        Int64 Id { get; set; }

        /// <summary>
        /// Indicates action to be performed on the object
        /// </summary>
        ObjectAction Action { get; set; }

        /// <summary>
        /// Indicates object id for which this activity log has been created
        /// </summary>
        Int64 MDMObjectId { get; set; }

        /// <summary>
        /// Indicates id of Object type
        /// </summary>
        Int16 MDMObjectTypeId { get; set; }

        /// <summary>
        /// Indicates Long name MDMObject type
        /// </summary>
        String MDMObjectTypeName { get; set; }

        /// <summary>
        /// Indicates type of message
        /// </summary>
        Int16 IntegrationMessageTypeId { get; set; }

        /// <summary>
        /// Indicates Long name of message type
        /// </summary>
        String IntegrationMessageTypeLongName { get; set; }

        /// <summary>
        /// Indicates Short name of message type
        /// </summary>
        String IntegrationMessageTypeName { get; set; }

        /// <summary>
        /// Indicates Id of connector for which this message is sent
        /// </summary>
        Int16 ConnectorId { get; set; }

        /// <summary>
        /// Indicates Long name of connector for which this message is sent
        /// </summary>
        String ConnectorLongName { get; set; }

        /// <summary>
        /// Indicates Short name of connector for which this message is sent
        /// </summary>
        String ConnectorName { get; set; }

        /// <summary>
        /// Indicates context for message. Typically some context information for MDMObjectId
        /// </summary>
        String Context { get; set; }

        /// <summary>
        /// Indicates no. of messages created from this update.
        /// </summary>
        Int32 MessageCount { get; set; }

        /// <summary>
        /// Indicates if loading the message for this activity is in progress
        /// </summary>
        Boolean IsLoadingInProgress { get; set; }

        /// <summary>
        /// Indicates if messages for this activity are already loaded
        /// </summary>
        Boolean IsLoaded { get; set; }

        /// <summary>
        /// Indicates if messages for this activity are already processed
        /// </summary>
        Boolean IsProcessed { get; set; }

        /// <summary>
        /// Indicates which server initiated this change
        /// </summary>
        Int32 ServerId { get; set; }

        /// <summary>
        /// Indicates weightage for this change
        /// </summary>
        Int32 Weightage { get; set; }

        /// <summary>
        /// Indicates time when creating message started.
        /// </summary>
        DateTime? MessageLoadStartTime { get; set; }

        /// <summary>
        /// Indicates time when creating message finished.
        /// </summary>
        DateTime? MessageLoadEndTime { get; set; }

        /// <summary>
        /// Indicates time when processing the messages started.
        /// </summary>
        DateTime? ProcessStartTime { get; set; }

        /// <summary>
        /// Indicates time when processing the messages finished.
        /// </summary>
        DateTime? ProcessEndTime { get; set; }

        /// <summary>
        /// Indicates Reference field for the object
        /// </summary>
        String ReferenceId { get; set; }

        /// <summary>
        /// Indicates type of integration type
        /// </summary>
        IntegrationType IntegrationType { get; set; }
        
        #endregion Properties

        #region Methods

        /// <summary>
        /// Represents IntegrationActivityLog in Xml format
        /// </summary>
        String ToXml();

        /// <summary>
        /// Clone the integration message and create a new integration message object
        /// </summary>
        /// <returns>New integration message object having same value as current one.</returns>
        IIntegrationActivityLog Clone();

        #endregion Methods
    }
}
