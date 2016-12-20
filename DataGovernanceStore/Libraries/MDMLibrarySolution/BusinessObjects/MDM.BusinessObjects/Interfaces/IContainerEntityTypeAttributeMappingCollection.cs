using System;
using System.Collections.Generic;

namespace MDM.Interfaces
{
    using MDM.BusinessObjects;

    /// <summary>
    /// Exposes methods or properties used for specifying container entity type attribute mapping collection information.
    /// </summary>
    public interface IContainerEntityTypeAttributeMappingCollection : IEnumerable<ContainerEntityTypeAttributeMapping>
    {
        #region Properties
        #endregion

        #region Methods

        /// <summary>
        /// Add ContainerEntityTypeAttributeMapping in collection
        /// </summary>
        /// <param name="iContainerEntityTypeAttributeMapping">entity to add in collection</param>
        void Add(IContainerEntityTypeAttributeMapping iContainerEntityTypeAttributeMapping);

        #region ToXml Methods

        /// <summary>
        /// Get Xml representation of IContainerEntityTypeAttributeMappingCollection object
        /// </summary>
        /// <returns>Xml string representing the IContainerEntityTypeAttributeMappingCollection</returns>
        String ToXml();

        #endregion

        #endregion
    }
}
