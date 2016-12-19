using System;

namespace MDM.Interfaces
{
    using MDM.Core;

    /// <summary>
    /// Exposes methods used for setting collection of integration item status.
    /// </summary>
    public interface IIntegrationItemStatusCollection
    {
        #region Public Methods

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
        /// Get Xml representation of IntegrationItemStatus object
        /// </summary>
        /// <returns>Xml String representing the IntegrationItemStatusCollection</returns>
        String ToXml();

        /// <summary>
        /// Get Xml representation of IntegrationItemStatus collection object
        /// </summary>
        /// <param name="serialization">serialization option. Based on the value selected, the different xml representation will be there</param>
        /// <returns>Xml representation of object</returns>
        String ToXml(ObjectSerialization serialization);

        /// <summary>
        /// Clone IntegrationItemStatus collection.
        /// </summary>
        /// <returns>cloned IntegrationItemStatus collection object.</returns>
        IIntegrationItemStatusCollection Clone();

        #endregion Public Methods
    }
}
