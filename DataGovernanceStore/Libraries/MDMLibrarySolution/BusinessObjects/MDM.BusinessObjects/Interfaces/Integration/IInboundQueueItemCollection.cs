using System;

namespace MDM.Interfaces
{
    using MDM.Core;

    /// <summary>
    /// Exposes methods used for setting collection of inbound queue items.
    /// </summary>
    public interface IInboundQueueItemCollection
    {
        #region Public Methods

        /// <summary>
        /// Determines whether the InboundQueueItemCollection contains a specific item.
        /// </summary>
        /// <param name="inboundQueueItemId">The inbound queue object to locate in the InboundQueueItemCollection.</param>
        /// <returns>
        /// <para>true : If inbound queue found in InboundQueueItemCollection</para>
        /// <para>false : If inbound queue found not in InboundQueueItemCollection</para>
        /// </returns>
        Boolean Contains(Int64 inboundQueueItemId);

        /// <summary>
        /// Remove inbound queue object from InboundQueueItemCollection
        /// </summary>
        /// <param name="inboundQueueItemId">InboundQueueItemId of inbound queue which is to be removed from collection</param>
        /// <returns>true if inbound queue is successfully removed; otherwise, false. This method also returns false if inbound queue was not found in the original collection</returns>
        Boolean Remove(Int64 inboundQueueItemId);

        /// <summary>
        /// Get specific inbound queue by Id
        /// </summary>
        /// <param name="inboundQueueItemId">Id of inbound queue</param>
        /// <returns><see cref="IInboundQueueItem"/></returns>
        IInboundQueueItem GetInboundQueueItem(Int64 inboundQueueItemId);

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
        /// Get Xml representation of InboundQueueItem object
        /// </summary>
        /// <returns>Xml String representing the InboundQueueItemCollection</returns>
        String ToXml();

        /// <summary>
        /// Get Xml representation of Inbound Messages collection object
        /// </summary>
        /// <param name="serialization">serialization option. Based on the value selected, the different xml representation will be there</param>
        /// <returns>Xml representation of object</returns>
        String ToXml(ObjectSerialization serialization);

        /// <summary>
        /// Clone inbound queue collection.
        /// </summary>
        /// <returns>cloned inbound queue collection object.</returns>
        IInboundQueueItemCollection Clone();

        /// <summary>
        /// Gets IInboundQueueItem item by id
        /// </summary>
        /// <param name="inboundQueueItemId">Id of the InboundQueueItem</param>
        /// <returns>IInboundQueueItem with specified Id</returns>
        IInboundQueueItem Get(Int64 inboundQueueItemId);

        #endregion Public Methods
    }
}
