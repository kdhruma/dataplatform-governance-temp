using System;
using System.Collections.Generic;

namespace MDM.Interfaces
{
    using MDM.BusinessObjects;

    /// <summary>
    /// Exposes methods or properties to set or get role mapping collection.
    /// </summary>
    public interface IRoleMappingCollection : IEnumerable<RoleMapping>, ICollection<RoleMapping>
    {
        #region Properties
        #endregion

        #region Methods

        /// <summary>
        /// Get XML representation of RoleMapping Collection
        /// </summary>
        /// <returns>XML representation of RoleMapping Collection</returns>
        String ToXml();

        #endregion
    }
}
