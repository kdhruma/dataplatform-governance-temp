using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace MDM.Interfaces
{
    using MDM.Core;
    using MDM.BusinessObjects;

    /// <summary>
    /// Exposes methods or properties to set or get entity collection.
    /// </summary>
    public interface IEntityCollection : IEnumerable<Entity>
    {
        #region Properties

        /// <summary>
        /// Presents no. of entity present into the collection
        /// </summary>
        Int32 Count { get; }

        #endregion

        #region Fields



        #endregion

        #region Methods

        /// <summary>
        /// Gets entity from the collection based on EntityId
        /// </summary>
        /// <param name="entityId">Indicates the Id of entity to search in current collection</param>
        /// <returns>Entity having given entity Id</returns>
        IEntity GetEntity(Int64 entityId);

        /// <summary>
        /// Gets entity from the collection based on External Id
        /// </summary>
        /// <param name="entityExternalId">Indicates the external Id of entity to search in current collection</param>
        /// <returns>Entity having given entity external Id</returns>
        IEntity GetEntity(String entityExternalId);

        /// <summary>
        /// Gets Xml representation of the EntityCollection object
        /// </summary>
        /// <returns>Xml string representing the EntityCollection</returns>
        String ToXml();

        /// <summary>
        /// Gets Xml representation of EntityCollection object
        /// </summary>
        /// <param name="objectSerialization">Indicates the serialization option. Based on the value selected, the different xml representation will be there</param>
        /// <returns>Xml representation of object</returns>
        String ToXml(ObjectSerialization objectSerialization);

        /// <summary>
        /// Gets XML representation of Entity object
        /// </summary>
        /// <param name="objectSerialization">Indicates the serialization option. Based on the value selected, the different xml representation will be there</param>
        /// <param name="serializeOnlyRootElements">
        /// Indciates whether to serialize whole Entity object including attributes and relationships or serialize only entity metadata
        /// <para>
        /// if true then returns only entity metadata. If false then returns attributes also.
        /// </para>
        /// </param>
        /// <returns>XML representation of Entity object</returns>
        String ToXml(ObjectSerialization objectSerialization, Boolean serializeOnlyRootElements);

        /// <summary>
        /// Gets entity id list from the current entity collection
        /// </summary>
        /// <returns>Collection of entity ids in current collection</returns>
        /// <exception cref="Exception">Thrown if there are no entities in current entity collection</exception>
        Collection<Int64> GetEntityIdList();

        /// <summary>
        /// Adds entity in collection
        /// </summary>
        /// <param name="entity">Indicates entity to add in collection</param>
        void Add(IEntity entity);

        /// <summary>
        /// Add Entities in collection
        /// </summary>
        /// <param name="iEntityCollection">Indicates entities to add in collection</param>
        /// <param name="checkDuplicates">Indicates whether duplicate entities can be added in current collection of entities</param>
        void AddRange(IEntityCollection iEntityCollection, Boolean checkDuplicates = false);

        /// <summary>
        /// Gets collection of entities based on the specified entity type Id
        /// </summary>
        /// <param name="entityTypeId">Indicates entity type id</param>
        /// <returns>Collection of entities based on the specified entity type Id</returns>
        IEntityCollection GetEntitiesByEntityTypeId(Int32 entityTypeId);

        #endregion Methods
    }
}
