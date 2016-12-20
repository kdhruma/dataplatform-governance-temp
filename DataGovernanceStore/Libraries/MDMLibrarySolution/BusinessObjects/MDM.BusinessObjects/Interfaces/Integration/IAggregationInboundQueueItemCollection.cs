using System;
using System.Collections.Generic;

namespace MDM.Interfaces
{
    using MDM.BusinessObjects.Integration;
    using MDM.Core;

    /// <summary>
    /// Exposes methods or properties to set or get the inbound queue item collection.
    /// </summary>
    public interface IAggregationInboundQueueItemCollection : IEnumerable<AggregationInboundQueueItem>
    {
        #region Public Methods

        /// <summary>
        /// Determines whether the IAggregationInboundQueueItemCollection contains a specific item.
        /// </summary>
        /// <param name="inboundQueueItemId">The inbound queue object to locate in the IAggregationInboundQueueItemCollection.</param>
        /// <returns>
        /// <para>true : If inbound queue found in IAggregationInboundQueueItemCollection</para>
        /// <para>false : If inbound queue found not in IAggregationInboundQueueItemCollection</para>
        /// </returns>
        Boolean Contains(Int64 inboundQueueItemId);

        /// <summary>
        /// Remove inbound queue object from IAggregationInboundQueueItemCollection
        /// </summary>
        /// <param name="inboundQueueItemId">IAggregationInboundQueueItem of inbound queue which is to be removed from collection</param>
        /// <returns>true if inbound queue is successfully removed; otherwise, false. This method also returns false if inbound queue was not found in the original collection</returns>
        Boolean Remove(Int64 inboundQueueItemId);

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
        /// Get Xml representation of IAggregationInboundQueueItemCollection object
        /// </summary>
        /// <returns>Xml String representing the IAggregationInboundQueueItemCollection</returns>
        String ToXml();

        /// <summary>
        /// Get Xml representation of Inbound Messages collection object
        /// </summary>
        /// <param name="serialization">serialization option. Based on the value selected, the different xml representation will be there</param>
        /// <returns>Xml representation of object</returns>
        String ToXml(ObjectSerialization serialization);

        /// <summary>
        /// Clone aggregation inbound queue collection.
        /// </summary>
        /// <returns>cloned aggregation inbound queue collection object.</returns>
        IAggregationInboundQueueItemCollection Clone();

        /// <summary>
        /// Gets IAggregationInboundQueueItem item by id
        /// </summary>
        /// <param name="inboundQueueItemId">Id of the IAggregationInboundQueueItem</param>
        /// <returns>IAggregationInboundQueueItem with specified Id</returns>
        IAggregationInboundQueueItem Get(Int64 inboundQueueItemId);

        #endregion Public Methods
    }
}
