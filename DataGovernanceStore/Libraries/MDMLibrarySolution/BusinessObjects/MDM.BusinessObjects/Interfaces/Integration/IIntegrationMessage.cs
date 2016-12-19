using System;
namespace MDM.Interfaces
{
    using MDM.Core;

    /// <summary>
    /// Exposes methods or properties to set or get integration message.
    /// </summary>
    public interface IIntegrationMessage
    {
        #region Properties

        /// <summary>
        /// Indicates Id of integration message
        /// </summary>
        Int64 Id { get; set; }

        /// <summary>
        /// Indicates action to be performed on the object
        /// </summary>
        ObjectAction Action { get; set; }

        /// <summary>
        /// Indicates body of message
        /// </summary>
        String MessageBody { get; set; }

        /// <summary>
        /// Indicates Reference field for the object
        /// </summary>
        String ReferenceId { get; set; }

        /// <summary>
        /// Indicates type of message
        /// </summary>
        Int16 MDMObjectTypeId { get; set; }

        /// <summary>
        /// Indicates the Integration Message Id of the message it is aggregated into by aggregation logic
        /// </summary>
        Int64 AggregatedMessageId { get; set; }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Represents IntegrationMessage in Xml format
        /// </summary>
        String ToXml();

        /// <summary>
        /// Clone the message object and return new instance with the same values.
        /// </summary>
        /// <returns>New cloned IntegrationMessage</returns>
        IIntegrationMessage Clone();

        /// <summary>
        /// Indicates Integration Message for integration system
        /// </summary>
        IIntegrationMessageHeader GetMessageHeader();

        #endregion Methods
    }
}
