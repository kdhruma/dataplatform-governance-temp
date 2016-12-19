using System;

namespace MDM.Interfaces
{
    using MDM.Core;

    /// <summary>
    /// Exposes methods or properties to set or get the collection of qualifying queue item.
    /// </summary>
    public interface IQualifyingQueueItemCollection
    {
        #region Public Methods

        /// <summary>
        /// Determines whether the QualifyingQueueItemCollection contains a specific item.
        /// </summary>
        /// <param name="qualifyingQueueItemId">The qualifying queue object to locate in the QualifyingQueueItemCollection.</param>
        /// <returns>
        /// <para>true : If qualifying queue found in QualifyingQueueItemCollection</para>
        /// <para>false : If qualifying queue found not in QualifyingQueueItemCollection</para>
        /// </returns>
        Boolean Contains(Int64 qualifyingQueueItemId);

        /// <summary>
        /// Remove qualifying queue object from QualifyingQueueItemCollection
        /// </summary>
        /// <param name="qualifyingQueueItemId">QualifyingQueueItemId of qualifying queue which is to be removed from collection</param>
        /// <returns>true if qualifying queue is successfully removed; otherwise, false. This method also returns false if qualifying queue was not found in the original collection</returns>
        Boolean Remove(Int64 qualifyingQueueItemId);

        /// <summary>
        /// Get specific qualifying queue by Id
        /// </summary>
        /// <param name="qualifyingQueueItemId">Id of qualifying queue</param>
        /// <returns><see cref="IQualifyingQueueItem"/></returns>
        IQualifyingQueueItem GetQualifyingQueueItem(Int64 qualifyingQueueItemId);

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
        /// Get Xml representation of QualifyingQueueItem object
        /// </summary>
        /// <returns>Xml String representing the QualifyingQueueItemCollection</returns>
        String ToXml();

        /// <summary>
        /// Get Xml representation of Qualifying Messages collection object
        /// </summary>
        /// <param name="serialization">serialization option. Based on the value selected, the different xml representation will be there</param>
        /// <returns>Xml representation of object</returns>
        String ToXml(ObjectSerialization serialization);

        /// <summary>
        /// Clone qualifying queue collection.
        /// </summary>
        /// <returns>cloned qualifying queue collection object.</returns>
        IQualifyingQueueItemCollection Clone();

        /// <summary>
        /// Gets IQualifyingQueueItem item by id
        /// </summary>
        /// <param name="qualifyingQueueItemId">Id of the IQualifyingQueueItem</param>
        /// <returns>IQualifyingQueueItem with specified Id</returns>
        IQualifyingQueueItem Get(Int64 qualifyingQueueItemId);

        #endregion Public Methods
    }
}
