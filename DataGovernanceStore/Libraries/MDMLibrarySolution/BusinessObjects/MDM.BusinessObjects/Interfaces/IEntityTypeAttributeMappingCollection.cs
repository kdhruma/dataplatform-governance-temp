using System;
using System.Collections.Generic;

namespace MDM.Interfaces
{
    using MDM.BusinessObjects;

    /// <summary>
    /// Exposes methods or properties to set or get entity type attribute mapping collection.
    /// </summary>
    public interface IEntityTypeAttributeMappingCollection : IEnumerable<EntityTypeAttributeMapping>
    {
        #region Properties
        #endregion

        #region ToXml Methods
        
        /// <summary>
        /// Get Xml representation of EntityTypeAttributeMappingCollection object
        /// </summary>
        /// <returns>Xml string representing the EntityTypeAttributeMappingCollection</returns>
        String ToXml();

        #endregion
    }
}