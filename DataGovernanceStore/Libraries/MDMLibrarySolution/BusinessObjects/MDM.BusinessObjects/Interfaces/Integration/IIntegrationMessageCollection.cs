using System;
using System.Collections.Generic;

namespace MDM.Interfaces
{
    using MDM.BusinessObjects.Integration;

    /// <summary>
    /// Exposes methods or properties to set or get collection of integration message.
    /// </summary>
    public interface IIntegrationMessageCollection : IEnumerable<IntegrationMessage>
    {
        #region Properties

        /// <summary>
        /// Presents no. of integration message present into the collection
        /// </summary>
        Int32 Count { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Clone integration message collection.
        /// </summary>
        /// <returns>cloned integration message collection object.</returns>
        IIntegrationMessageCollection Clone();

        /// <summary>
        /// Determines whether the IntegrationMessageCollection contains a specific message.
        /// </summary>
        /// <param name="integrationMessageId">The integration message object to locate in the IntegrationMessageCollection.</param>
        /// <returns>
        /// <para>true : If integration message found in IntegrationMessageCollection</para>
        /// <para>false : If integration message found not in IntegrationMessageCollection</para>
        /// </returns>
        Boolean Contains(Int64 integrationMessageId);

        /// <summary>
        /// Determines whether two Object instances are equal.
        /// </summary>
        /// <param name="obj">The Object to compare with the current Object.</param>
        /// <returns>true if the specified Object is equal to the current Object; otherwise, false.</returns>
        Boolean Equals(Object obj);

        /// <summary>
        /// Get specific integration message by Id
        /// </summary>
        /// <param name="integrationMessageId">Id of integration message</param>
        /// <returns><see cref="IIntegrationMessage"/></returns>
        MDM.Interfaces.IIntegrationMessage Get(Int64 integrationMessageId);

        /// <summary>
        /// Serves as a hash function for type
        /// </summary>
        /// <returns>A hash code for the current Object.</returns>
        Int32 GetHashCode();

        /// <summary>
        /// Remove integration message object from IntegrationMessageCollection
        /// </summary>
        /// <param name="integrationMessageId">IntegrationMessageId of integration message which is to be removed from collection</param>
        /// <returns>true if integration message is successfully removed; otherwise, false. This method also returns false if integration message was not found in the original collection</returns>
        Boolean Remove(Int64 integrationMessageId);

        /// <summary>
        /// Add message in collection
        /// </summary>
        /// <param name="message"><see cref="IIntegrationMessage"/> to be added</param>
        void Add(IIntegrationMessage message);
         
        /// <summary>
        /// Get Xml representation of hierarchy object
        /// </summary>
        /// <returns>Xml string representing the IntegrationMessageCollection</returns>
        String ToXml();

        /// <summary>
        /// Get Xml representation of Integration Messages collection object
        /// </summary>
        /// <param name="serialization">serialization option. Based on the value selected, the different xml representation will be there</param>
        /// <returns>Xml representation of object</returns>
        String ToXml(MDM.Core.ObjectSerialization serialization);

        #endregion
    }
}