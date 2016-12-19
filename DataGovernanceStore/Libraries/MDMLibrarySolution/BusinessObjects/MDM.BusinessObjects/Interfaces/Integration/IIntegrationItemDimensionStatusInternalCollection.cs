using System;

namespace MDM.Interfaces
{
    using MDM.Core;

    /// <summary>
    /// Exposes methods used for setting integration item dimension status internal collection.
    /// </summary>
    public interface IIntegrationItemDimensionStatusInternalCollection
    {
        #region Public Methods

        /// <summary>
        /// Determines whether the IntegrationItemDimensionStatusInternalCollection contains a specific IntegrationItemDimensionStatusInternal.
        /// </summary>
        /// <param name="integrationItemDimensionStatusInternalId">The IntegrationItemDimensionStatusInternal object to locate in the IntegrationItemDimensionStatusInternalCollection.</param>
        /// <returns>
        /// <para>true : If IntegrationItemDimensionStatusInternal found in IntegrationItemDimensionStatusInternalCollection</para>
        /// <para>false : If IntegrationItemDimensionStatusInternal found not in IntegrationItemDimensionStatusInternalCollection</para>
        /// </returns>
        Boolean Contains(Int32 integrationItemDimensionStatusInternalId);

        /// <summary>
        /// Remove IntegrationItemDimensionStatusInternal object from IntegrationItemDimensionStatusInternalCollection
        /// </summary>
        /// <param name="integrationItemDimensionStatusInternalId">IntegrationItemDimensionStatusInternalId of IntegrationItemDimensionStatusInternal which is to be removed from collection</param>
        /// <returns>true if IntegrationItemDimensionStatusInternal is successfully removed; otherwise, false. This method also returns false if IntegrationItemDimensionStatusInternal was not found in the original collection</returns>
        Boolean Remove(Int32 integrationItemDimensionStatusInternalId);

        /// <summary>
        /// Get specific IntegrationItemDimensionStatusInternal by Id
        /// </summary>
        /// <param name="integrationItemDimensionStatusInternalId">Id of IntegrationItemDimensionStatusInternal</param>
        /// <returns>Object of type IIntegrationItemDimensionStatusInternal</returns>
        IIntegrationItemStatusDimensionInternal GetIntegrationItemStatusDimensionInternal(Int32 integrationItemDimensionStatusInternalId);

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
        /// Get Xml representation of IntegrationItemDimensionStatusInternal object
        /// </summary>
        /// <returns>Xml String representing the IntegrationItemDimensionStatusInternalCollection</returns>
        String ToXml();

        /// <summary>
        /// Get Xml representation of IntegrationItemDimensionStatusInternal collection object
        /// </summary>
        /// <param name="serialization">serialization option. Based on the value selected, the different xml representation will be there</param>
        /// <returns>Xml representation of object</returns>
        String ToXml(ObjectSerialization serialization);

        /// <summary>
        /// Clone IntegrationItemDimensionStatusInternal collection.
        /// </summary>
        /// <returns>cloned IntegrationItemDimensionStatusInternal collection object.</returns>
        IIntegrationItemDimensionStatusInternalCollection Clone();

        /// <summary>
        /// Gets IntegrationItemDimensionStatusInternal item by id
        /// </summary>
        /// <param name="integrationItemDimensionStatusInternalId">Id of the IntegrationItemDimensionStatusInternal</param>
        /// <returns>IntegrationItemDimensionStatusInternal with specified Id</returns>
        IIntegrationItemStatusDimensionInternal Get(Int32 integrationItemDimensionStatusInternalId);

        /// <summary>
        /// Add status for item dimensions
        /// </summary>
        /// <param name="dimensionTypeId"></param>
        /// <param name="integrationItemDimensionTypeName"></param>
        /// <param name="integrationItemDimensionTypeLongName"></param>
        /// <param name="integrationItemDimensionValue"></param>
        void Add(Int32 dimensionTypeId, String integrationItemDimensionTypeName, String integrationItemDimensionTypeLongName, String integrationItemDimensionValue);

        #endregion Public Methods
    }
}
