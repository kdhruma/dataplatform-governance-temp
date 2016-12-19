using System;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;

namespace MDM.BusinessObjects.Integration
{
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Represents inbound queue item
    /// </summary>
    public class AggregationInboundQueueItem : AggregationQueueItem, IAggregationInboundQueueItem
    {
        /// <summary>
        /// Parameter less constructor
        /// </summary>
        public AggregationInboundQueueItem()
            :base(IntegrationType.Inbound)
        {
        }

        /// <summary>
        /// Xml constructor
        /// </summary>
        /// <param name="valuesAsXml">Value in Xml format</param>
        public AggregationInboundQueueItem(String valuesAsXml)
            :base(IntegrationType.Inbound, valuesAsXml)
        {
            
        }
    }
}
