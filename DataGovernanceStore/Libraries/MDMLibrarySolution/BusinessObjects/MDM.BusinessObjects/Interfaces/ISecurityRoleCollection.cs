using System;
using System.Collections.Generic;

namespace MDM.Interfaces
{
    using MDM.BusinessObjects;

    /// <summary>
    /// Exposes methods or properties to set or get the collection of security roles.
    /// </summary>
    public interface ISecurityRoleCollection : IEnumerable<SecurityRole>
    {
        #region Properties

        /// <summary>
        /// Check if SecurityRoleCollection is read-only.
        /// </summary>
        Boolean IsReadOnly { get; }

        /// <summary>
        /// Get the count of no. of ISecurityRoles in ISecurityRoleCollection
        /// </summary>
        Int32 Count {get;}

        #endregion

        #region Methods

        /// <summary>
        /// Add SecurityRole object in collection
        /// </summary>
        /// <param name="item">SecurityRole to add in collection</param>
        void Add(ISecurityRole item);

        /// <summary>
        /// Determines whether the SecurityRoleCollection contains a specific SecurityRole.
        /// </summary>
        /// <param name="item">The SecurityRole object to locate in the SecurityRoleCollection.</param>
        /// <returns>
        /// <para>true : If SecurityRole found in mappingCollection</para>
        /// <para>false : If SecurityRole found not in mappingCollection</para>
        /// </returns>
        Boolean Contains(ISecurityRole item);

        /// <summary>
        /// Determines whether two Object instances are equal.
        /// </summary>
        /// <param name="obj">The Object to compare with the current Object.</param>
        /// <returns>true if the specified Object is equal to the current Object; otherwise, false.</returns>
        Boolean Equals(object obj);

        /// <summary>
        /// Returns an enumerator that iterates through a SecurityRoleCollection.
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection.</returns>
        new IEnumerator<SecurityRole> GetEnumerator();

        /// <summary>
        /// Serves as a hash function for type
        /// </summary>
        /// <returns>A hash code for the current Object.</returns>
        Int32 GetHashCode();

        /// <summary>
        /// Removes the first occurrence of a specific object from the SecurityRoleCollection.
        /// </summary>
        /// <param name="item">The SecurityRole object to remove from the SecurityRoleCollection.</param>
        /// <returns>true if item is successfully removed; otherwise, false. This method also returns false if item was not found in the original SecurityRoleCollection</returns>
        Boolean Remove(ISecurityRole item);

        /// <summary>
        /// Get Xml representation of SecurityPermissionDefinitionCollection object
        /// </summary>
        /// <returns>Xml string representing the SecurityPermissionDefinitionCollection</returns>
        String ToXml();

        #endregion methods
    }
}
