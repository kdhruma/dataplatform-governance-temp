using System;
using System.Collections.Generic;
using System.Linq;

namespace MDM.Interfaces
{
    using MDM.Core;

    /// <summary>
    /// Exposes methods or properties to set or get an extension relationship.
    /// </summary>
    public interface IExtensionRelationship : IRelationshipBase
    {
        #region Properties

        /// <summary>
        /// Property denoting the container id of the extended entity
        /// </summary>
        Int32 ContainerId { get; set; }
        
        /// <summary>
        /// Property denoting the container name of the extended entity
        /// </summary>
        String ContainerName { get; set; }
        
        /// <summary>
        /// Property denoting the container long name of the extended entity
        /// </summary>
        String ContainerLongName { get; set; }
        
        /// <summary>
        /// Property denoting the category id of the extended entity
        /// </summary>
        Int64 CategoryId { get; set; }
        
        /// <summary>
        /// Property denoting the category name of the extended entity
        /// </summary>
        String CategoryName { get; set; }
        
        /// <summary>
        /// Property denoting the container long name of the extended entity
        /// </summary>
        String CategoryLongName { get; set; }

        /// <summary>
        /// Field denoting category path of the extended entity
        /// </summary>
        String CategoryPath { get; set; }

        /// <summary>
        /// Field denoting category long name path of the extended entity
        /// </summary>
        String CategoryLongNamePath { get; set; }

        /// <summary>
        /// Field denoting the external Id of the extended entity
        /// </summary>
        String ExternalId { get; set; }
        #endregion

        #region Methods

        /// <summary>
        /// Gets extension relationships
        /// </summary>
        /// <returns>Extension Relationship collection interface</returns>
        new IExtensionRelationshipCollection GetRelationships();

        /// <summary>
        /// Sets extension relationships
        /// </summary>
        /// <param name="iExtensionRelationshipCollection">Extension Relationship collection interface</param>
        /// <exception cref="ArgumentNullException">Raised when passed relationship collection is null</exception>
        void SetRelationships(IExtensionRelationshipCollection iExtensionRelationshipCollection);

        #endregion
    }
}
