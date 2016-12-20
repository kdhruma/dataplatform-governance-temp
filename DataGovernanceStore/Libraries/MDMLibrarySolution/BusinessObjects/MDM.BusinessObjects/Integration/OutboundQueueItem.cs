using System;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;

namespace MDM.BusinessObjects.Integration
{
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Represents outbound queue item
    /// </summary>
    public class OutboundQueueItem : ProcessingQueueItem, IOutboundQueueItem
    {
        /// <summary>
        /// Parameter less constructor
        /// </summary>
        public OutboundQueueItem()
            :base(IntegrationType.Outbound)
        {
        }

        /// <summary>
        /// Xml constructor
        /// </summary>
        /// <param name="valuesAsXml">Value in Xml format</param>
        public OutboundQueueItem(String valuesAsXml)
            :base(IntegrationType.Outbound, valuesAsXml)
        {
            
        }
    }
}
