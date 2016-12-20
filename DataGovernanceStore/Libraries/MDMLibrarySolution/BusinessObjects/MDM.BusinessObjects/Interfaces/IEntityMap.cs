using System;

namespace MDM.Interfaces
{
    using MDM.Core;

    /// <summary>
    /// Exposes methods or properties to set or get entity map.
    /// </summary>
    public interface IEntityMap
    {
        #region Properties

        /// <summary>
        /// Property denoting id of the entity map used for internal operations
        /// </summary>
        Int64 Id { get; set; }

        /// <summary>
        /// Property defining the type id of the MDM object
        /// </summary>
        Int32 ObjectTypeId { get; set; }

        /// <summary>
        /// Property defining the type of the MDM object
        /// </summary>
        String ObjectType { get; set; }

        /// <summary>
        /// Property denoting the Id of the system
        /// </summary>
        String SystemId { get; set; }

        /// <summary>
        /// Property denoting entity external Id
        /// </summary>
        String ExternalId { get; set; }

        /// <summary>
        /// Property denoting entity internal Id
        /// </summary>
        Int64 InternalId { get; set; }

        /// <summary>
        /// Property denoting container Id of the entity in the context
        /// </summary>
        Int32 ContainerId { get; set; }

        /// <summary>
        /// Property denoting category Id of the entity in the context
        /// </summary>
        Int64 CategoryId { get; set; }

        /// <summary>
        /// Property denoting entity type Id of the entity in the context
        /// </summary>
        Int32 EntityTypeId { get; set; }

        /// <summary>
        /// Property denoting the parent extension id of an entity map
        /// </summary>
        Int64 ParentExtensionEntityId { get; set; }

        /// <summary>
        /// Property denoting custom data that can be used for passing attribute values
        /// </summary>
        String CustomData { get; set; }

        /// <summary>
        /// Specifies entity family id for a variant tree
        /// </summary>
        Int64 EntityFamilyId { get; set; }

        /// <summary>
        /// Specifies entity global family id across parent(including extended families)
        /// </summary>
        Int64 EntityGlobalFamilyId { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Get Xml representation of Entity Map
        /// </summary>
        /// <returns>Xml representation of Entity Map</returns>
        String ToXml();

        /// <summary>
        /// Get Xml representation of Entity Map based on requested object serialization
        /// </summary>
        /// <param name="objectSerialization">Type of the serialization option</param>
        /// <returns>Xml representation of Entity map</returns>
        String ToXml(ObjectSerialization objectSerialization);

        #endregion
    }
}