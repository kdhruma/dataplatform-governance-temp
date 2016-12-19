using System;
using System.Collections.Generic;

namespace MDM.Interfaces
{
    using MDM.BusinessObjects.Integration;
    using MDM.Core;

    /// <summary>
    /// Exposes methods or properties to set or get the outbound queue item collection.
    /// </summary>
    public interface IAggregationOutboundQueueItemCollection : IEnumerable<AggregationOutboundQueueItem>
    {
        #region Public Methods

        /// <summary>
        /// Determines whether the IAggregationOutboundQueueItemCollection contains a specific item.
        /// </summary>
        /// <param name="outboundQueueItemId">The outbound queue object to locate in the IAggregationOutboundQueueItemCollection.</param>
        /// <returns>
        /// <para>true : If outbound queue found in IAggregationOutboundQueueItemCollection</para>
        /// <para>false : If outbound queue found not in IAggregationOutboundQueueItemCollection</para>
        /// </returns>
        Boolean Contains(Int64 outboundQueueItemId);

        /// <summary>
        /// Remove outbound queue object from IAggregationOutboundQueueItemCollection
        /// </summary>
        /// <param name="outboundQueueItemId">IAggregationOutboundQueueItem of outbound queue which is to be removed from collection</param>
        /// <returns>true if outbound queue is successfully removed; otherwise, false. This method also returns false if outbound queue was not found in the original collection</returns>
        Boolean Remove(Int64 outboundQueueItemId);

        /// <summary>
        /// Determines whether two Object instances are equal.
        /// </summary>
        /// <param name="obj">The Object to compare with the current Object.</param>
        /// <returns>true if the specified Object is equal to the current Object; otherwise, false.</returns>
        Boolean Equals(object obj);

        /// <summary>
        /// Serves as a hash function for type
        /// </summary>
        /// <returns>A hash code for the current Object.</returns>
        Int32 GetHashCode();

        /// <summary>
        /// Get Xml representation of IAggregationOutboundQueueItemCollection object
        /// </summary>
        /// <returns>Xml String representing the IAggregationOutboundQueueItemCollection</returns>
        String ToXml();

        /// <summary>
        /// Get Xml representation of Outbound Messages collection object
        /// </summary>
        /// <param name="serialization">serialization option. Based on the value selected, the different xml representation will be there</param>
        /// <returns>Xml representation of object</returns>
        String ToXml(ObjectSerialization serialization);

        /// <summary>
        /// Clone aggregation outbound queue collection.
        /// </summary>
        /// <returns>cloned aggregation outbound queue collection object.</returns>
        IAggregationOutboundQueueItemCollection Clone();

        /// <summary>
        /// Gets IAggregationOutboundQueueItem item by id
        /// </summary>
        /// <param name="outboundQueueItemId">Id of the IAggregationOutboundQueueItem</param>
        /// <returns>IAggregationOutboundQueueItem with specified Id</returns>
        IAggregationOutboundQueueItem Get(Int64 outboundQueueItemId);

        #endregion Public Methods
    }
}
