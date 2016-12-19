using System;

namespace MDM.Interfaces
{
    using MDM.Core;

    /// <summary>
    /// Exposes methods or properties to set or get integration activity that occured in integration system (inbound action triggered or outbound action triggered)
    /// </summary>
    public interface IIntegrationActivity
    {
        #region Properties

        /// <summary>
        /// Indicates Id of integration activity log
        /// </summary>
        Int64 Id { get; }

        /// <summary>
        /// Indicates object id for which this activity log has been created
        /// </summary>
        Int64 MDMObjectId { get; }

        /// <summary>
        /// Indicates id of Object type
        /// </summary>
        Int16 MDMObjectTypeId { get; }

        /// <summary>
        /// Indicates Long name MDMObject type
        /// </summary>
        String MDMObjectTypeName { get; }

        /// <summary>
        /// Indicates type of message
        /// </summary>
        Int16 IntegrationMessageTypeId { get; }

        /// <summary>
        /// Indicates Long name of message type
        /// </summary>
        String IntegrationMessageTypeLongName { get; }

        /// <summary>
        /// Indicates Short name of message type
        /// </summary>
        String IntegrationMessageTypeName { get; }


        /// <summary>
        /// Indicates Id of connector for which this message is sent
        /// </summary>
        Int16 ConnectorId { get; }

        /// <summary>
        /// Indicates Long name of connector for which this message is sent
        /// </summary>
        String ConnectorLongName { get; }

        /// <summary>
        /// Indicates Short name of connector for which this message is sent
        /// </summary>
        String ConnectorName { get; }

        /// <summary>
        /// Indicates context for message. Typically some context information for MDMObjectId in xml format.
        /// </summary>
        String Context { get; }

        /// <summary>
        /// Indicates the type of integration. Is this activity log for inbound processing or outbound processing
        /// </summary>
        IntegrationType IntegrationType { get; }

        /// <summary>
        /// Field denoting blob of data for the activity. It can contain file data as well.
        /// </summary>
        Byte[] Data { get; }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Represents IntegrationActivity in Xml format
        /// </summary>
        String ToXml();

        /// <summary>
        /// Clone the integration activity and create a new integration activity
        /// </summary>
        /// <returns>New integration activity having same value as current one.</returns>
        IIntegrationActivity Clone();

        #endregion Methods
    }
}
