using System;
using System.Collections.Generic;
using System.Linq;

namespace MDM.Interfaces
{
    using MDM.Core;

    /// <summary>
    /// Exposes methods or properties to set or get hierarchy relationship.
    /// </summary>
    public interface IHierarchyRelationship : IRelationshipBase
    {
        #region Properties

        #endregion

        #region Methods

        /// <summary>
        /// Gets hierarchy relationships
        /// </summary>
        /// <returns>Hierarchy Relationship collection interface</returns>
        new IHierarchyRelationshipCollection GetRelationships();

        /// <summary>
        /// Sets hierarchy relationships
        /// </summary>
        /// <param name="iHierarchyRelationshipCollection">Hierarchy Relationship collection interface</param>
        /// <exception cref="ArgumentNullException">Raised when passed relationship collection is null</exception>
        void SetRelationships(IHierarchyRelationshipCollection iHierarchyRelationshipCollection);

        #endregion
    }
}
