using System;

namespace MDM.Interfaces
{
    using MDM.Core;

    /// <summary>
    /// Exposes methods and properties to set or get entity identifier properties.
    /// </summary>
    public interface IEntityUniqueIdentifier
    {
        #region Properties

        /// <summary>
        /// Specifies the entity id
        /// </summary>
        Int64 EntityId { get; set; }

        /// <summary>
        /// specifies the shortname of the entity
        /// </summary>
        string ExternalId { get; set; }

        /// <summary>
        /// specifies the category id of the entity
        /// </summary>
        Int64 CategoryId { get; set; }

        /// <summary>
        /// Specifies the category path of the entity
        /// </summary>
        String CategoryPath { get; set; }

        /// <summary>
        /// Specifies the catalog id of the entity
        /// </summary>
        Int32 ContainerId { get; set; }

        /// <summary>
        /// Specifies the catalog name of the entity
        /// </summary>
        String ContainerName { get; set; }

        /// <summary>
        /// Specifies the entity type id 
        /// </summary>
        Int32 EntityTypeId { get; set; }

        /// <summary>
        /// specifies the entity type name
        /// </summary>
        String EntityTypeName { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Get Xml representation of Entity Identifier
        /// </summary>
        /// <returns>Xml representation of Entity Identifier</returns>
        String ToXml();

        /// <summary>
        /// Get Xml representation of Entity Identifier based on requested object serialization
        /// </summary>
        /// <param name="objectSerialization">Type of the serialization option</param>
        /// <returns>Xml representation of Entity Identifier</returns>
        String ToXml(ObjectSerialization objectSerialization);

        #endregion
    }
}
