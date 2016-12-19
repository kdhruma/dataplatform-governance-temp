using System;
using System.Collections.Generic;

namespace MDM.Interfaces
{
    using MDM.BusinessObjects.Integration;
    using MDM.Core;

    /// <summary>
    /// Exposes methods or properties to set or get collection of integration error log.
    /// </summary>
    public interface IIntegrationErrorLogCollection : IEnumerable<IntegrationErrorLog>
    {
        #region Public Methods

        /// <summary>
        /// Determines whether the IIntegrationErrorLogCollection contains a specific item.
        /// </summary>
        /// <param name="integrationErrorLogId">The outbound queue object to locate in the IIntegrationErrorLogCollection.</param>
        /// <returns>
        /// <para>true : If integration error log found in IIntegrationErrorLogCollection</para>
        /// <para>false : If integration error log found not in IIntegrationErrorLogCollection</para>
        /// </returns>
        Boolean Contains(Int64 integrationErrorLogId);

        /// <summary>
        /// Remove integration error log from IIntegrationErrorLogCollection
        /// </summary>
        /// <param name="integrationErrorLogId">IIntegrationErrorLog of integration error log which is to be removed from collection</param>
        /// <returns>true if integration error log is successfully removed; otherwise, false. This method also returns false if integration error log was not found in the original collection</returns>
        Boolean Remove(Int64 integrationErrorLogId);

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
        /// Get Xml representation of IIntegrationErrorLogCollection object
        /// </summary>
        /// <returns>Xml String representing the IIntegrationErrorLogCollection</returns>
        String ToXml();

        /// <summary>
        /// Get Xml representation of integration error log collection object
        /// </summary>
        /// <param name="serialization">serialization option. Based on the value selected, the different xml representation will be there</param>
        /// <returns>Xml representation of object</returns>
        String ToXml(ObjectSerialization serialization);

        /// <summary>
        /// Clone integration error log collection.
        /// </summary>
        /// <returns>cloned integration error log collection object.</returns>
        IIntegrationErrorLogCollection Clone();

        /// <summary>
        /// Gets IIntegrationErrorLog item by id
        /// </summary>
        /// <param name="integrationErrorLogId">Id of the IIntegrationErrorLog</param>
        /// <returns>IIntegrationErrorLog with specified Id</returns>
        IIntegrationErrorLog Get(Int64 integrationErrorLogId);

        #endregion Public Methods
    }
}
