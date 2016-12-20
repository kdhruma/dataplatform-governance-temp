using System;

namespace MDM.Interfaces
{
    using MDM.Core;

    /// <summary>
    /// Exposes methods or properties to set or get collection of integration item dimension type.
    /// </summary>
    public interface IIntegrationItemDimensionTypeCollection
    {
        #region Public Methods

        /// <summary>
        /// Determines whether the IntegrationItemDimensionTypeCollection contains a specific IntegrationItemDimensionType.
        /// </summary>
        /// <param name="integrationItemDimensionTypeId">The IntegrationItemDimensionType object to locate in the IntegrationItemDimensionTypeCollection.</param>
        /// <returns>
        /// <para>true : If IntegrationItemDimensionType found in IntegrationItemDimensionTypeCollection</para>
        /// <para>false : If IntegrationItemDimensionType found not in IntegrationItemDimensionTypeCollection</para>
        /// </returns>
        Boolean Contains(Int32 integrationItemDimensionTypeId);

        /// <summary>
        /// Remove IntegrationItemDimensionType object from IntegrationItemDimensionTypeCollection
        /// </summary>
        /// <param name="integrationItemDimensionTypeId">IntegrationItemDimensionTypeId of IntegrationItemDimensionType which is to be removed from collection</param>
        /// <returns>true if IntegrationItemDimensionType is successfully removed; otherwise, false. This method also returns false if IntegrationItemDimensionType was not found in the original collection</returns>
        Boolean Remove(Int32 integrationItemDimensionTypeId);

        /// <summary>
        /// Get IntegrationItemDimensionType based on given integration item dimension type identifier
        /// </summary>
        /// <param name="integrationItemDimensionTypeId">Indicates identifier of integration item dimension type</param>
        /// <returns>Returns IntegrationItemDimensionType based on given integration item dimension type identifier</returns>
        IIntegrationItemDimensionType GetIntegrationItemDimensionType(Int32 integrationItemDimensionTypeId);

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
        /// Get Xml representation of IntegrationItemDimensionType object
        /// </summary>
        /// <returns>Xml String representing the IntegrationItemDimensionTypeCollection</returns>
        String ToXml();

        /// <summary>
        /// Get Xml representation of IntegrationItemDimensionType collection object
        /// </summary>
        /// <param name="serialization">serialization option. Based on the value selected, the different xml representation will be there</param>
        /// <returns>Xml representation of object</returns>
        String ToXml(ObjectSerialization serialization);

        /// <summary>
        /// Clone IntegrationItemDimensionType collection.
        /// </summary>
        /// <returns>cloned IntegrationItemDimensionType collection object.</returns>
        IIntegrationItemDimensionTypeCollection Clone();

        /// <summary>
        /// Gets IntegrationItemDimensionType item by id
        /// </summary>
        /// <param name="integrationItemDimensionTypeId">Id of the IntegrationItemDimensionType</param>
        /// <returns>IntegrationItemDimensionType with specified Id</returns>
        IIntegrationItemDimensionType Get(Int32 integrationItemDimensionTypeId);

        /// <summary>
        /// Gets IntegrationItemDimensionType by IntegrationItemDimensionType ShortName
        /// </summary>
        /// <param name="integrationItemDimensionTypeShortName">Short name of the IntegrationItemDimensionType</param>
        /// <returns>IntegrationItemDimensionType with specified ShortName</returns>
        IIntegrationItemDimensionTypeCollection Get(String integrationItemDimensionTypeShortName);

        /// <summary>
        /// Gets IntegrationItemDimensionType by IntegrationItemDimensionType ShortName and connector short name
        /// </summary>
        /// <param name="integrationItemDimensionTypeShortName">Short name of the IntegrationItemDimensionType</param>
        /// <param name="connectorName"> Name of the connector</param>
        /// <returns>IntegrationItemDimensionType with specified ShortName</returns>
        IIntegrationItemDimensionType Get(String integrationItemDimensionTypeShortName, String connectorName);

        /// <summary>
        /// Gets IntegrationItemDimensionType by IntegrationItemDimensionType ShortName and connector Id
        /// </summary>
        /// <param name="integrationItemDimensionTypeShortName">Short name of the IntegrationItemDimensionType</param>
        /// <param name="connectorId"> ID of the connector</param>
        /// <returns>IntegrationItemDimensionType with specified ShortName</returns>
        IIntegrationItemDimensionType Get(String integrationItemDimensionTypeShortName, Int16 connectorId);

        /// <summary>
        /// Gets IntegrationItemDimensionType by connector ShortName
        /// </summary>
        /// <param name="connectorName">Short name of the connector</param>
        /// <returns>IntegrationItemDimensionType with specified connector ShortName</returns>
        IIntegrationItemDimensionTypeCollection GetIntegrationItemDimensionTypesByConnectorName(String connectorName);
        
        /// <summary>
        /// Gets IntegrationItemDimensionType by connector Id
        /// </summary>
        /// <param name="connectorId">Id of the connector</param>
        /// <returns>IntegrationItemDimensionType with specified connector Id</returns>
        IIntegrationItemDimensionTypeCollection GetIntegrationItemDimensionTypesByConnectorId(Int16 connectorId);

        #endregion Public Methods
    }
}
