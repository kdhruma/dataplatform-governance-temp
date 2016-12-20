using System;
namespace MDM.Interfaces
{
    using MDM.Core;

    /// <summary>
    /// Exposes methods used for setting collection of integration activity log related information.
    /// </summary>
    public interface IIntegrationActivityLogCollection
    {
        #region Public Methods

        /// <summary>
        /// Determines whether the IntegrationActivityLogCollection contains a specific message.
        /// </summary>
        /// <param name="integrationActivityLogId">The IntegrationActivityLog object to locate in the IntegrationActivityLogCollection.</param>
        /// <returns>
        /// <para>true : If IntegrationActivityLog found in IntegrationActivityLogCollection</para>
        /// <para>false : If IntegrationActivityLog found not in IntegrationActivityLogCollection</para>
        /// </returns>
        Boolean Contains(Int64 integrationActivityLogId);

        /// <summary>
        /// Remove IntegrationActivityLog object from IntegrationActivityLogCollection
        /// </summary>
        /// <param name="integrationActivityLogId">IntegrationActivityLogId of IntegrationActivityLog which is to be removed from collection</param>
        /// <returns>true if IntegrationActivityLog is successfully removed; otherwise, false. This method also returns false if IntegrationActivityLog was not found in the original collection</returns>
        Boolean Remove(Int64 integrationActivityLogId);

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
        /// Get Xml representation of hierarchy object
        /// </summary>
        /// <returns>Xml String representing the IntegrationActivityLogCollection</returns>
        String ToXml();

        /// <summary>
        /// Get Xml representation of IntegrationActivityLogs collection object
        /// </summary>
        /// <param name="serialization">serialization option. Based on the value selected, the different xml representation will be there</param>
        /// <returns>Xml representation of object</returns>
        String ToXml(ObjectSerialization serialization);

        /// <summary>
        /// Clone IntegrationActivityLog collection.
        /// </summary>
        /// <returns>cloned IntegrationActivityLog collection object.</returns>
        IIntegrationActivityLogCollection Clone();

        /// <summary>
        /// Gets hierarchy item by id
        /// </summary>
        /// <param name="integrationActivityLogId">Id of the hierarchy</param>
        /// <returns>hierarchy with specified Id</returns>
        IIntegrationActivityLog Get(Int64 integrationActivityLogId);

        /// <summary>
        /// Get specific IntegrationActivityLog by Id
        /// </summary>
        /// <param name="integrationActivityLogId">Id of IntegrationActivityLog</param>
        /// <returns><see cref="IIntegrationActivityLog"/></returns>
        IIntegrationActivityLog GetIntegrationActivityLog(Int64 integrationActivityLogId);

        #endregion Public Methods
    }
}
