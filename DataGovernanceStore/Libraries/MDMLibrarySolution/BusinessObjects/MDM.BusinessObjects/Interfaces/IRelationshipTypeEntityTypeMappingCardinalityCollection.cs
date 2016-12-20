using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDM.BusinessObjects.Interfaces
{
    /// <summary>
    /// Specifies interface providing RelationshipType EntityType Cardinality mapping collection related information
    /// </summary>
    public interface IRelationshipTypeEntityTypeMappingCardinalityCollection : IEnumerable<RelationshipTypeEntityTypeMappingCardinality>
    {
        #region Methods

        /// <summary>
        /// Get Xml representation of IRelationshipTypeEntityTypeMappingCardinalityCollection object
        /// </summary>
        /// <returns>Xml string representing the IRelationshipTypeEntityTypeMappingCardinalityCollection</returns>
        String ToXml();

        #endregion
    }
}
