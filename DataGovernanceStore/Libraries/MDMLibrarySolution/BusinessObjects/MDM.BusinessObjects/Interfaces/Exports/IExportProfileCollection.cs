using System;
using System.Collections.Generic;
using MDM.BusinessObjects;
using MDM.BusinessObjects.Exports;
using MDM.Core;

namespace MDM.Interfaces.Exports
{
    /// <summary>
    /// Exposes methods or properties to set or get the collection of export profiles.
    /// </summary>
    public interface IExportProfileCollection : IEnumerable<ExportProfile>
    {
        /// <summary>
        /// Check if ExportProfileCollection contains ExportProfile with given Id
        /// </summary>
        /// <param name="id">Id using which ExportProfile is to be searched from collection</param>
        /// <returns>
        /// <para>true : If ExportProfile found in ExportProfileCollection</para>
        /// <para>false : If ExportProfile found not in ExportProfileCollection</para>
        /// </returns>
        bool Contains(int id);

        /// <summary>
        /// Returns count of elements inside of the collection
        /// </summary>
        int Count { get; }

        /// <summary>
        /// Remove exportSubscriber object from ExportProfileCollection
        /// </summary>
        /// <param name="exportSubscriberId">exportSubscriberId of exportSubscriber which is to be removed from collection</param>
        /// <returns>true if exportSubscriber is successfully removed; otherwise, false. This method also returns false if exportSubscriber was not found in the original collection</returns>
        bool Remove(Int32 exportSubscriberId);

        /// <summary>
        /// Determines whether two Object instances are equal.
        /// </summary>
        /// <param name="obj">The Object to compare with the current Object.</param>
        /// <returns>true if the specified Object is equal to the current Object; otherwise, false.</returns>
        bool Equals(object obj);

        /// <summary>
        /// Serves as a hash function for type
        /// </summary>
        /// <returns>A hash code for the current Object.</returns>
        int GetHashCode();

        /// <summary>
        /// Get Xml representation of ExportProfileCollection
        /// </summary>
        /// <returns>Xml representation of ExportProfileCollection</returns>
        string ToXml();

        /// <summary>
        /// Get Xml representation of ExportProfileCollection
        /// </summary>
        /// <param name="serialization">Type of Object Serialization</param>
        /// <returns>Xml representation of ExportProfileCollection</returns>
        string ToXml(ObjectSerialization serialization);
    }
}
