using System;
using System.Collections.Generic;

namespace MDM.Interfaces
{
    using MDM.BusinessObjects;

    /// <summary>
    /// Exposes methods or properties to set or get the container relationship type attribute mapping collection.
    /// </summary>
    public interface IContainerRelationshipTypeAttributeMappingCollection : IEnumerable<ContainerRelationshipTypeAttributeMapping>
    {
        #region Properties
        #endregion

        #region Methods

        /// <summary>
        /// Add ContainerRelationshipTypeAttributeMapping in collection
        /// </summary>
        /// <param name="iContainerRelationshipTypeAttributeMapping">ContainerRelationshipTypeAttributeMapping to add in collection</param>
        void Add(IContainerRelationshipTypeAttributeMapping iContainerRelationshipTypeAttributeMapping);

        #region ToXml Methods

        /// <summary>
        /// Get Xml representation of ContainerRelationshipTypeAttributeMappingCollection object
        /// </summary>
        /// <returns>Xml string representing the ContainerRelationshipTypeAttributeMappingCollection</returns>
        String ToXml();

        #endregion

        #endregion
    }
}
