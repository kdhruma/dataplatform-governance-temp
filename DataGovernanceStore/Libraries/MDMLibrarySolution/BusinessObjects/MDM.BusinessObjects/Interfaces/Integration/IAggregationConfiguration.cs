using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDM.Interfaces
{
    /// <summary>
    /// Exposes methods or properties to set or get configuration options for aggregation queue.
    /// </summary>
    public interface IAggregationConfiguration
    {
        #region Methods

        /// <summary>
        /// Represents AggregationConfiguration in Xml format
        /// </summary>
        String ToXml();

        /// <summary>
        /// Clone the AggregationConfiguration object and return new instance with the same values.
        /// </summary>
        /// <returns>New cloned IAggregationConfiguration</returns>
        IAggregationConfiguration Clone();

        /// <summary>
        /// Get schedule criteria for aggregating inbound messages
        /// </summary>
        /// <returns></returns>
        IScheduleCriteria GetInboundScheduleCriteria();

        /// <summary>
        /// Get schedule criteria for aggregating outbound messages
        /// </summary>
        /// <returns></returns>
        IScheduleCriteria GetOutboundScheduleCriteria();

        /// <summary>
        /// Indicates whether aggregation is enabled for inbound messages
        /// </summary>
        Boolean IsEnabledForInbound {get;set;}

        /// <summary>
        /// Indicates whether aggregation is enabled for outbound messages
        /// </summary>
        Boolean IsEnabledForOutbound { get; set; }

        /// <summary>
        /// Indicates the batch size for inbound messages aggregation
        /// </summary>
        Int32 InboundBatchSize { get; set; }

        /// <summary>
        /// Indicates the batch size for outbound messages aggregation
        /// </summary>
        Int32 OutboundBatchSize { get; set; }

        #endregion Methods
    }
}
