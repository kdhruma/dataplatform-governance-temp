using System;
using System.Collections.Generic;

namespace MDM.Interfaces
{
    using MDM.BusinessObjects;

    /// <summary>
    /// Exposes methods or properties used for providing container entity type mapping collection related information.
    /// </summary>
    public interface IContainerEntityTypeMappingCollection : IEnumerable<ContainerEntityTypeMapping>
    {
        #region Properties
        #endregion

        #region Methods

        /// <summary>
        /// Add ContainerEntityTypeMapping in collection
        /// </summary>
        /// <param name="iContainerEntityTypeMapping">Container entity type mapping to add in collection</param>
        void Add(IContainerEntityTypeMapping iContainerEntityTypeMapping);

        /// <summary>
        /// Get ContainerEntityTypeMapping for given container and entity type identifiers
        /// </summary>
        /// <param name="containerId">Indicates container identifier</param>
        /// <param name="entityTypeId">Indicates entity type identifier</param>
        /// <returns>Returns ContainerEntityTypeMapping for given container and entity type identifiers.</returns>
        IContainerEntityTypeMapping GetByContainerAndEntityType(Int32 containerId, Int32 entityTypeId);

        #region ToXml Methods

        /// <summary>
        /// Get Xml representation of IContainerEntityTypeMappingCollection object
        /// </summary>
        /// <returns>Xml string representing the IContainerEntityTypeMappingCollection</returns>
        String ToXml();

        #endregion

        #endregion
    }
}
