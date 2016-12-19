using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDM.Interfaces
{
    using MDM.BusinessObjects;

    /// <summary>
    /// Exposes methods or properties to set or get the relationship type entity type mapping collection related information.
    /// </summary>
    public interface IRelationshipTypeEntityTypeMappingCollection : IEnumerable<RelationshipTypeEntityTypeMapping>
    {
        #region Properties
        #endregion

        #region Methods

        /// <summary>
        /// Add RelationshipType EntityType Mapping in collection
        /// </summary>
        /// <param name="iRelationshipTypeEntityTypeMapping">RelationshipType EntityType Mapping to add in collection</param>
        void Add(IRelationshipTypeEntityTypeMapping iRelationshipTypeEntityTypeMapping);

        #region ToXml Methods

        /// <summary>
        /// Get Xml representation of IRelationshipTypeEntityTypeMapping object
        /// </summary>
        /// <returns>Xml string representing the IRelationshipTypeEntityTypeMapping</returns>
        String ToXml();

        #endregion

        #endregion
    }
}
