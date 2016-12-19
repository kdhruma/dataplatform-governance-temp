using System;
using System.Collections.Generic;
using MDM.Core;

namespace MDM.BusinessObjects.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    public interface IMDMObjectInfoCollection : ICollection<MDMObjectInfo>
    {
        /// <summary>
        /// Check by Id if MDMObjectInfo exists in collection
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Presence of MDMObjectInfo in collection</returns>
        Boolean Contains(Int32 id);

        /// <summary>
        /// Determines whether two Object instances are equal.
        /// </summary>
        /// <param name="obj">The Object to compare with the current Object.</param>
        /// <returns>true if the specified Object is equal to the current Object; otherwise, false.</returns>
        Boolean Equals(Object obj);

        /// <summary>
        /// Serves as a hash function for type
        /// </summary>
        /// <returns>A hash code for the current Object.</returns>
        Int32 GetHashCode();

        /// <summary>
        /// Remove MDMObjectInfo from collection
        /// </summary>
        /// <param name="mdmObjectInfoId">Id of MDMObjectInfo which should be removed</param>
        /// <returns>Success of removing</returns>
        Boolean Remove(Int32 mdmObjectInfoId);

        /// <summary>
        /// Get Xml representation of MDMObjectInfoCollection object
        /// </summary>
        /// <returns>Xml representation of object</returns>
        String ToXml();

        /// <summary>
        /// Get Xml representation of MDMObjectInfoCollection object
        /// </summary>
        /// <param name="serialization"></param>
        /// <returns>Xml representation of object</returns>
        String ToXml(ObjectSerialization serialization);
    }
}
