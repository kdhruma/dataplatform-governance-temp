using System;
namespace MDM.Interfaces
{
    /// <summary>
    /// Exposes methods or properties to set or get collection of integration message type.
    /// </summary>
    public interface IIntegrationMessageTypeCollection
    {
        /// <summary>
        /// Clone integration message type collection.
        /// </summary>
        /// <returns>cloned integration message type collection object.</returns>
        IIntegrationMessageTypeCollection Clone();

        /// <summary>
        /// Determines whether the IntegrationMessageTypeCollection contains a specific message.
        /// </summary>
        /// <param name="integrationMessageTypeId">The integration message type object to locate in the IntegrationMessageTypeCollection.</param>
        /// <returns>
        /// <para>true : If integration message type found in IntegrationMessageTypeCollection</para>
        /// <para>false : If integration message type found not in IntegrationMessageTypeCollection</para>
        /// </returns>
        Boolean Contains(Int16 integrationMessageTypeId);

        /// <summary>
        /// Determines whether two Object instances are equal.
        /// </summary>
        /// <param name="obj">The Object to compare with the current Object.</param>
        /// <returns>true if the specified Object is equal to the current Object; otherwise, false.</returns>
        Boolean Equals(Object obj);

        /// <summary>
        /// Get specific integration message type by Id
        /// </summary>
        /// <param name="integrationMessageTypeId">Id of integration message type</param>
        /// <returns><see cref="IIntegrationMessageType"/></returns>
        MDM.Interfaces.IIntegrationMessageType Get(Int16 integrationMessageTypeId);

        /// <summary>
        /// Serves as a hash function for type
        /// </summary>
        /// <returns>A hash code for the current Object.</returns>
        Int32 GetHashCode();

        /// <summary>
        /// Remove integration message type object from IntegrationMessageTypeCollection
        /// </summary>
        /// <param name="integrationMessageTypeId">IntegrationMessageTypeId of integration message type which is to be removed from collection</param>
        /// <returns>true if integration message type is successfully removed; otherwise, false. This method also returns false if integration message type was not found in the original collection</returns>
        Boolean Remove(Int16 integrationMessageTypeId);

        /// <summary>
        /// Get Xml representation of message type object
        /// </summary>
        /// <returns>Xml string representing the IntegrationMessageTypeCollection</returns>
        String ToXml();

        /// <summary>
        /// Get Xml representation of Integration Messages collection object
        /// </summary>
        /// <param name="serialization">serialization option. Based on the value selected, the different xml representation will be there</param>
        /// <returns>Xml representation of object</returns>
        String ToXml(MDM.Core.ObjectSerialization serialization);
    }
}
