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
    public class AggregationOutboundQueueItem : AggregationQueueItem, IAggregationOutboundQueueItem
    {
        /// <summary>
        /// Parameter less constructor
        /// </summary>
        public AggregationOutboundQueueItem()
            :base(IntegrationType.Outbound)
        {
        }

        /// <summary>
        /// Xml constructor
        /// </summary>
        /// <param name="valuesAsXml">Value in Xml format</param>
        public AggregationOutboundQueueItem(String valuesAsXml)
            :base(IntegrationType.Outbound, valuesAsXml)
        {
            
        }
    }
}
