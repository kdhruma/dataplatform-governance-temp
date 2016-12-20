using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MDM.Core;

namespace MDM.Interfaces
{
    /// <summary>
    /// Exposes methods or properties to set or get integration error log related information.
    /// </summary>
    public interface IIntegrationErrorLog
    {
        #region Properties

        /// <summary>
        /// Indicates Id of IntegrationErrorLog table
        /// </summary>
        Int64 Id { get; set; }

        /// <summary>
        /// Indicates Id of queue item for which error occurred. It may contains Id of IntegrationActivityLog/QualifyingQueue/OutboundQueue
        /// </summary>
        Int64 IntegrationId { get; }

        /// <summary>
        /// Indicates type of integration - is it inbound or outbound
        /// </summary>
        IntegrationType IntegrationType { get; set; }

        /// <summary>
        /// Indicates message type for integration message.
        /// </summary>
        Int16 IntegrationMessageTypeId { get; set; }

        /// <summary>
        /// Indicates Id of connector for which error occurred.
        /// </summary>
        Int16 ConnectorId { get; set; }

        /// <summary>
        /// Indicates the (error/info/warning) message.
        /// </summary>
        String MessageText { get; set; }

        /// <summary>
        /// Indicates if message is Error or information or warning.
        /// </summary>
        OperationResultType MessageType { get; set; }

        /// <summary>
        /// Indicates the name of processor from which error occurred.
        /// </summary>
        CoreDataProcessorList CoreDataProcessorName { get; set; }

        /// <summary>
        /// Indicates action to be performed on current object.
        /// </summary>
        ObjectAction Action { get; set; }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Represents IntegrationErrorLog in Xml format
        /// </summary>
        String ToXml();

        /// <summary>
        /// Clone the integration message and create a new integration message object
        /// </summary>
        /// <returns>New integration message object having same value as current one.</returns>
        IIntegrationErrorLog Clone();

        #endregion Methods
    }
}
