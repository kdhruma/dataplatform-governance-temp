using System;
using System.Collections.Generic;

namespace MDM.BusinessObjects.Interfaces
{
    /// <summary>
    /// Exposes methods or properties to set or get the container relationship type entity type mapping collection related information.
    /// </summary>
    public interface IContainerRelationshipTypeEntityTypeMappingCollection : IEnumerable<ContainerRelationshipTypeEntityTypeMapping>
    {
        #region Properties
        #endregion

        #region Methods

        /// <summary>
        /// Add Container RelationshipType EntityType Mapping in collection
        /// </summary>
        /// <param name="iContainerRelationshipTypeEntityTypeMapping">Container RelationshipType EntityType Mapping to add in collection</param>
        void Add(IContainerRelationshipTypeEntityTypeMapping iContainerRelationshipTypeEntityTypeMapping);

        #region ToXml Methods

        /// <summary>
        /// Get Xml representation of IContainerRelationshipTypeEntityTypeMapping object
        /// </summary>
        /// <returns>Xml string representing the IContainerRelationshipTypeEntityTypeMapping</returns>
        String ToXml();

        #endregion

        #endregion
    }
}
