using System;

namespace MDM.Interfaces
{
    using MDM.Core;

    /// <summary>
    /// Exposes methods used for setting the collection of outbound queue items.
    /// </summary>
    public interface IOutboundQueueItemCollection
    {
        #region Public Methods

        /// <summary>
        /// Determines whether the OutboundQueueItemCollection contains a specific item.
        /// </summary>
        /// <param name="outboundQueueItemId">The outbound queue object to locate in the OutboundQueueItemCollection.</param>
        /// <returns>
        /// <para>true : If outbound queue found in OutboundQueueItemCollection</para>
        /// <para>false : If outbound queue found not in OutboundQueueItemCollection</para>
        /// </returns>
        Boolean Contains(Int64 outboundQueueItemId);

        /// <summary>
        /// Remove outbound queue object from OutboundQueueItemCollection
        /// </summary>
        /// <param name="outboundQueueItemId">OutboundQueueItemId of outbound queue which is to be removed from collection</param>
        /// <returns>true if outbound queue is successfully removed; otherwise, false. This method also returns false if outbound queue was not found in the original collection</returns>
        Boolean Remove(Int64 outboundQueueItemId);

        /// <summary>
        /// Get specific outbound queue by Id
        /// </summary>
        /// <param name="outboundQueueItemId">Id of outbound queue</param>
        /// <returns><see cref="IOutboundQueueItem"/></returns>
        IOutboundQueueItem GetOutboundQueueItem(Int64 outboundQueueItemId);

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
        /// Get Xml representation of OutboundQueueItem object
        /// </summary>
        /// <returns>Xml String representing the OutboundQueueItemCollection</returns>
        String ToXml();

        /// <summary>
        /// Get Xml representation of Outbound Messages collection object
        /// </summary>
        /// <param name="serialization">serialization option. Based on the value selected, the different xml representation will be there</param>
        /// <returns>Xml representation of object</returns>
        String ToXml(ObjectSerialization serialization);

        /// <summary>
        /// Clone outbound queue collection.
        /// </summary>
        /// <returns>cloned outbound queue collection object.</returns>
        IOutboundQueueItemCollection Clone();

        /// <summary>
        /// Gets IOutboundQueueItem item by id
        /// </summary>
        /// <param name="outboundQueueItemId">Id of the OutboundQueueItem</param>
        /// <returns>IOutboundQueueItem with specified Id</returns>
        IOutboundQueueItem Get(Int64 outboundQueueItemId);

        #endregion Public Methods
    }
}
