using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MDM.Core;

namespace MDM.Interfaces
{
    /// <summary>
    /// Exposes methods or properties to set or get integration item. All items being processed from Integration processors need to implement this interface.
    /// </summary>
    public interface IIntegrationItem
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

        #endregion Properties
    }
}
