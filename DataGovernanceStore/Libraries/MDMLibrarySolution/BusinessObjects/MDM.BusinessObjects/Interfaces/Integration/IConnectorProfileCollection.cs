using System;

namespace MDM.Interfaces
{
    using MDM.Core;

    /// <summary>
    /// Exposes methods used for setting the collection of connector profile.
    /// </summary>
    public interface IConnectorProfileCollection
    {
        #region Public Methods

        /// <summary>
        /// Determines whether the ConnectorProfileCollection contains a specific item.
        /// </summary>
        /// <param name="connectorProfileId">The connector profile object to locate in the ConnectorProfileCollection.</param>
        /// <returns>
        /// <para>true : If connector profile found in ConnectorProfileCollection</para>
        /// <para>false : If connector profile found not in ConnectorProfileCollection</para>
        /// </returns>
        Boolean Contains(Int16 connectorProfileId);

        /// <summary>
        /// Remove connector profile object from ConnectorProfileCollection
        /// </summary>
        /// <param name="connectorProfileId">ConnectorProfileId of connector profile which is to be removed from collection</param>
        /// <returns>true if connector profile is successfully removed; otherwise, false. This method also returns false if connector profile was not found in the original collection</returns>
        Boolean Remove(Int16 connectorProfileId);

        /// <summary>
        /// Get specific connector profile by Id
        /// </summary>
        /// <param name="connectorProfileId">Id of connector profile</param>
        /// <returns><see cref="IConnectorProfile"/></returns>
        IConnectorProfile GetConnectorProfile(Int16 connectorProfileId);

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
        /// Get Xml representation of ConnectorProfile object
        /// </summary>
        /// <returns>Xml String representing the ConnectorProfileCollection</returns>
        String ToXml();

        /// <summary>
        /// Get Xml representation of connector profile collection object
        /// </summary>
        /// <param name="serialization">serialization option. Based on the value selected, the different xml representation will be there</param>
        /// <returns>Xml representation of object</returns>
        String ToXml(ObjectSerialization serialization);

        /// <summary>
        /// Clone connector profile collection.
        /// </summary>
        /// <returns>cloned connector profile collection object.</returns>
        IConnectorProfileCollection Clone();

        /// <summary>
        /// Gets IConnectorProfile item by id
        /// </summary>
        /// <param name="connectorProfileId">Id of the IConnectorProfile</param>
        /// <returns>IConnectorProfile with specified Id</returns>
        IConnectorProfile Get(Int16 connectorProfileId);

        #endregion Public Methods
    }
}
