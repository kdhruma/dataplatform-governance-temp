using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDM.Interfaces
{
    using MDM.BusinessObjects;

    /// <summary>
    /// Exposes methods or properties to set or get the collection of security users. 
    /// </summary>
    public interface ISecurityUserCollection : IEnumerable<SecurityUser>
    {
        #region Properties

        #endregion

        #region Methods

        /// <summary>
        /// Add ISecurityUser object in collection
        /// </summary>
        /// <param name="iSecurityUser">ISecurityUser to add in collection</param>
        void Add(ISecurityUser iSecurityUser);

        /// <summary>
        /// Removes the first occurrence of a specific object from the SecurityUserCollection.
        /// </summary>
        /// <param name="iSecurityUser">The SecurityUser object to remove from the SecurityUserCollection.</param>
        /// <returns>true if item is successfully removed; otherwise, false. This method also returns false if item was not found in the original SecurityUserCollection</returns>
        Boolean Remove(ISecurityUser iSecurityUser);

        /// <summary>
        /// Get Xml representation of Security user collection object
        /// </summary>
        /// <returns>Xml string representing the Security user collection</returns>
        String ToXml();

        #endregion
    }
}
