using System;
using System.Collections.Generic;

namespace MDM.Interfaces
{
    using System.Collections.ObjectModel;
    using MDM.BusinessObjects;
    using MDM.Core;

    /// <summary>
    /// Exposes methods or properties to set or get the collection of organization.
    /// </summary>
    public interface IOrganizationCollection : ICollection<Organization>
    {
        /// <summary>
        /// Indicates allowed user actions on the object
        /// </summary>
        Collection<UserAction> PermissionSet { get; }

        /// <summary>
        /// Check by Id if Organization exists in collection
        /// </summary>
        /// <param name="id">Id of an Organization</param>
        /// <returns>Presence of Organization in collection</returns>
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
        /// Remove Organization from collection
        /// </summary>
        /// <param name="organizationId">Id of Organization which should be removed</param>
        /// <returns>Success of removing</returns>
        Boolean Remove(Int32 organizationId);

        /// <summary>
        /// Get Xml representation of OrganizationCollection object
        /// </summary>
        /// <returns>Xml representation of object</returns>
        String ToXml();

        /// <summary>
        /// Get Xml representation of OrganizationCollection object
        /// </summary>
        /// <param name="serialization">Type of Object Serialization</param>
        /// <returns>Xml representation of object</returns>
        String ToXml(ObjectSerialization serialization);

        /// <summary>
        /// Clone Organization collection.
        /// </summary>
        /// <returns>cloned Organization collection object.</returns>
        IOrganizationCollection Clone();

        /// <summary>
        /// Add item to the collection
        /// </summary>
        /// <param name="iOrganization">Object which implements interface <see cref="IOrganization"/></param>
        void Add(IOrganization iOrganization);
    }
}
