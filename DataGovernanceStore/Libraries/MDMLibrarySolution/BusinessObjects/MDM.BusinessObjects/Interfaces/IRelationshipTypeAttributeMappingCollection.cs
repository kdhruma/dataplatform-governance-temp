using System;
using System.Collections.Generic;

namespace MDM.Interfaces
{
    using MDM.BusinessObjects;

    /// <summary>
    /// Exposes methods or properties to set or get the relationship type attribute mapping collection.
    /// </summary>
    public interface IRelationshipTypeAttributeMappingCollection : IEnumerable<RelationshipTypeAttributeMapping>
    {
        #region Properties
        #endregion

        #region ToXml Methods

        /// <summary>
        /// Get Xml representation of RelationshipTypeAttributeMappingCollection object
        /// </summary>
        /// <returns>Xml string representing the RelationshipTypeAttributeMappingCollection</returns>
        String ToXml();

        #endregion
    }
}
