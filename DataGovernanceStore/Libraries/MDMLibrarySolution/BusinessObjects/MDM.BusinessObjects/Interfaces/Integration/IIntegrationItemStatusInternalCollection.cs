using System;

namespace MDM.Interfaces
{
    using MDM.Core;

    /// <summary>
    /// Exposes methods or properties to set or get integration item status internal collection. 
    /// </summary>
    public interface IIntegrationItemStatusInternalCollection
    {
        #region Public Methods

        /// <summary>
        /// Determines whether the IntegrationItemStatusInternalCollection contains a specific IntegrationItemStatusInternal.
        /// </summary>
        /// <param name="integrationItemStatusInternalId">The IntegrationItemStatusInternal object to locate in the IntegrationItemStatusInternalCollection.</param>
        /// <returns>
        /// <para>true : If IntegrationItemStatusInternal found in IntegrationItemStatusInternalCollection</para>
        /// <para>false : If IntegrationItemStatusInternal found not in IntegrationItemStatusInternalCollection</para>
        /// </returns>
        Boolean Contains(Int64 integrationItemStatusInternalId);

        /// <summary>
        /// Remove IntegrationItemStatusInternal object from IntegrationItemStatusInternalCollection
        /// </summary>
        /// <param name="integrationItemStatusInternalId">IntegrationItemStatusInternalId of IntegrationItemStatusInternal which is to be removed from collection</param>
        /// <returns>true if IntegrationItemStatusInternal is successfully removed; otherwise, false. This method also returns false if IntegrationItemStatusInternal was not found in the original collection</returns>
        Boolean Remove(Int64 integrationItemStatusInternalId);

        /// <summary>
        /// Get IntegrationItemStatusInternal based on integration item status internal id
        /// </summary>
        /// <param name="integrationItemStatusInternalId">Indicates identifier of IntegrationItemStatusInternal</param>
        /// <returns>Returns IntegrationItemStatusInternal based on integration item status internal id</returns>
        IIntegrationItemStatusInternal GetIntegrationItemStatusInternal(Int64 integrationItemStatusInternalId);

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
        /// Get Xml representation of IntegrationItemStatusInternal object
        /// </summary>
        /// <returns>Xml String representing the IntegrationItemStatusInternalCollection</returns>
        String ToXml();

        /// <summary>
        /// Get Xml representation of IntegrationItemStatusInternal collection object
        /// </summary>
        /// <param name="serialization">serialization option. Based on the value selected, the different xml representation will be there</param>
        /// <returns>Xml representation of object</returns>
        String ToXml(ObjectSerialization serialization);

        /// <summary>
        /// Clone IntegrationItemStatusInternal collection.
        /// </summary>
        /// <returns>cloned IntegrationItemStatusInternal collection object.</returns>
        IIntegrationItemStatusInternalCollection Clone();

        #endregion Public Methods
    }
}
