using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDM.Interfaces
{
    using MDM.Core;

    /// <summary>
    /// Exposes methods or properties to set or get the container template copy context.
    /// </summary>
    public interface IContainerTemplateCopyContext : IMDMObject
    {
        #region Properties

        /// <summary>
        /// Indicates if container entitytype mappings are to be copied
        /// </summary>
        Boolean CopyContainerEntityTypeMappings { get; set; }

        /// <summary>
        /// Indicates if Container Entitytype Mappings Attribute are to be copied
        /// </summary>
        Boolean CopyContainerEntityTypeAttributeMappings { get; set; }

        /// <summary>
        /// Indicates if Container RelationshipType Mappings are to be copied
        /// </summary>
        Boolean CopyContainerRelationshipTypeMappings { get; set; }

        /// <summary>
        /// Indicates if Container RelationshipType Attribute Mappings are to be copied
        /// </summary>
        Boolean CopyContainerRelationshipTypeAttributeMappings { get; set; }

        /// <summary>
        /// Indicates if Container BranchLevel One is to be copied
        /// </summary>
        Boolean CopyContainerBranchLevelOne { get; set; }

        /// <summary>
        /// Indicates if copying is flush and fill or the incremental
        /// </summary>
        Boolean FlushAndFillTargetContainer { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Get Xml representation of ContainerTempleteCopyContext object
        /// </summary>
        /// <returns>Xml representation of object</returns>
        String ToXml();

        /// <summary>
        /// Get Xml representation of ContainerTempleteCopyContext object
        /// </summary>
        /// <param name="serialization">serialization option. Based on the value selected, the different xml representation will be there</param>
        /// <returns>Xml representation of object</returns>
        String ToXml(ObjectSerialization serialization);

        #endregion
    }
}
