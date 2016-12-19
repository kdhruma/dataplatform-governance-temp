using System;
using System.Collections.ObjectModel;

namespace MDM.Interfaces
{
	using MDM.Core;

    /// <summary>
    /// Exposes methods or properties to set or get integration message header containing context information.
    /// </summary>
    public interface IIntegrationMessageHeader
    {
        #region Properties


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
        String MDMObjectTypeLongName { get; set; }

        /// <summary>
        /// Indicates type of message
        /// </summary>
        Int16 IntegrationMessageTypeId { get; set; }

        /// <summary>
        /// Indicates Long name of message type
        /// </summary>
        String IntegrationMessageTypeLongName { get; set; }

        /// <summary>
        /// Indicates Id of connector for which this message is sent
        /// </summary>
        Int16 ConnectorId { get; set; }

        /// <summary>
        /// Indicates Long name of connector for which this message is sent
        /// </summary>
        String ConnectorName { get; set; }

        /// <summary>
        /// Indicates context for message. Typically some context information for MDMObjectId
        /// </summary>
        String Context { get; set; }

        /// <summary>
        /// Indicates the type of integration. Indicates this message is for inbound processing or outbound processing
        /// </summary>
        IntegrationType IntegrationType { get; set; }

        /// <summary>
        /// Indicates the collection of Integration Message Ids the current message is aggregated as.
        /// </summary>
        Collection<Int64> ParentMessageIds { get; set; }
        
        #endregion Properties

        #region Methods

        /// <summary>
        /// Represents IntegrationMessageHeader in Xml format
        /// </summary>
        String ToXml();

        #endregion Methods
    }
}
