using System;
using System.Collections.Generic;
using System.Linq;

namespace MDM.Interfaces
{
    using MDM.Core;
    using MDM.BusinessObjects;

    /// <summary>
    /// Exposes methods or properties to set or get extension relationship in collection.
    /// </summary>
    public interface IExtensionRelationshipCollection : IEnumerable<ExtensionRelationship>
    {
        #region Properties

        /// <summary>
        /// Property denoting Action
        /// </summary>
        ObjectAction Action { get; set; }

        /// <summary>
        /// Property Denoting EntityChangeContext
        /// </summary>
        EntityChangeContext ChangeContext { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Gets Xml representation of Extension Relationship Collection
        /// </summary>
        /// <returns>Xml representation of Extension Relationship Collection</returns>
        String ToXml();

        /// <summary>
        /// Gets Xml representation of Extension Relationship Collection based on requested object serialization
        /// </summary>
        /// <param name="objectSerialization">Indicates type of serialization option</param>
        /// <returns>Xml representation of Extension Relationship Collection</returns>
        String ToXml(ObjectSerialization objectSerialization);

        /// <summary>
        /// Denormalizes extension hierarchy into flat structure
        /// </summary>
        /// <returns>Denormalized Extension Relationships</returns>
        IExtensionRelationshipCollection Denormalize();

        /// <summary>
        /// Adds extension relationship object in collection
        /// </summary>
        /// <param name="iExtensionRelationship">Indicates the extension Relationship to add in collection</param>
        void Add(IExtensionRelationship iExtensionRelationship);

        /// <summary>
        /// Finds extension relationship in the specified container Id 
        /// </summary>
        /// <param name="containerId">Indicates identifier of the container</param>
        /// <returns>ExtensionRelationship in the specified container Id </returns>
        IExtensionRelationship FindByContainerId(Int32 containerId);

        /// <summary>
        /// Find ExtensionRelationship from Collection by Related Entity Id
        /// </summary>
        /// <param name="relatedEntityId">Indicates identifier of the related entity</param>
        /// <returns>ExtensionRelationship if match found or null if not</returns>
        IExtensionRelationship FindByRelatedEntityId(Int64 relatedEntityId);

        /// <summary>
        /// Find ExtensionRelationship from the collection by ToExternalId, ToContainerId, and ToCategoryId
        /// </summary>
        /// <param name="toExternalId">Indicates external identifier of the extended entity</param>
        /// <param name="toContainerId">Indicates container identifier of the extended entity</param>
        /// <param name="toCategoryId">Indicates the category identifier of the extended entity</param>
        /// <returns>ExtensionRelationship if match found or null if not</returns>
        IExtensionRelationship FindByToExternalIdContainerIdCategoryId(String toExternalId, Int32 toContainerId, Int64 toCategoryId);

        /// <summary>
        /// Find ExtensionRelationship from Collection by ToExternalId, ContainerName, and CategoryPath
        /// </summary>
        /// <param name="toExternalId">Indicates external identifier of the extended entity</param>
        /// <param name="toContainerName">Indicates container name of the extended entity</param>
        /// <param name="toCategoryPath">Indicates the category path of the extended entity</param>
        /// <returns>Returns ExtensionRelationship if match found or null if not.</returns>
        IExtensionRelationship FindByToExternalIdContainerNameCategoryPath(String toExternalId, String toContainerName, String toCategoryPath);

        #endregion
    }
}
