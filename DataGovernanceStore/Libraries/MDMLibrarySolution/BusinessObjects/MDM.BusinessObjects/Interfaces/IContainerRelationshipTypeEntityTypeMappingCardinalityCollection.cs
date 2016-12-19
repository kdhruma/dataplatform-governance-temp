using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDM.BusinessObjects.Interfaces
{
    /// <summary>
    /// Exposes methods or properties to set or get the container relationship type entity type cardinality mapping collection related information.
    /// </summary>
    public interface IContainerRelationshipTypeEntityTypeMappingCardinalityCollection : IEnumerable<ContainerRelationshipTypeEntityTypeMappingCardinality>
    {
        #region Methods

        /// <summary>
        /// Get Xml representation of IContainerRelationshipTypeEntityTypeMappingCardinalityCollection object
        /// </summary>
        /// <returns>Xml string representing the IContainerRelationshipTypeEntityTypeMappingCardinalityCollection</returns>
        String ToXml();

        #endregion
    }
}
