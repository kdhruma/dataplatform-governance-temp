using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace MDM.BusinessObjects
{
    using MDM.Interfaces;

    /// <summary>
    /// Represents a collection of the File object.
    /// </summary>
    [DataContract]
    [ProtoContract(ImplicitFields = ImplicitFields.AllFields)]
    public class EntityUniqueIdentifierCollection : InterfaceContractCollection<IEntityUniqueIdentifier, EntityUniqueIdentifier>, IEntityUniqueIdentifierCollection
    {
        #region Constructors

        /// <summary>
        /// Parameterless constructor
        /// </summary>
        public EntityUniqueIdentifierCollection() : base() { }

        #endregion

        #region Properties
        
        #endregion

        #region IEnumerable Members

        /// <summary>
        /// Returns an enumerator that iterates through a FileCollection.
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection.</returns>
        public new IEnumerator<IEntityUniqueIdentifier> GetEnumerator()
        {
            return this._items.GetEnumerator();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Get entity unique identifier collection based on specified container qualifier name
        /// </summary>
        /// <param name="containerQualifierName">Indicates qualifier name for the container</param>
        /// <returns>Returns entity unique identifier collection</returns>
        public EntityUniqueIdentifierCollection GetByContainerQualifierName(String containerQualifierName)
        {
            EntityUniqueIdentifierCollection entityUniqueIdentifiers = null;

            if (this._items.Count > 0)
            {
                entityUniqueIdentifiers = new EntityUniqueIdentifierCollection();

                foreach (EntityUniqueIdentifier entityUniqueIdentifier in this._items)
                {
                    if (String.Compare(entityUniqueIdentifier.ContainerQualifierName, containerQualifierName, true) == 0)
                    {
                        entityUniqueIdentifiers.Add(entityUniqueIdentifier);
                    }
                }
            }

            return entityUniqueIdentifiers;
        }

        /// <summary>
        /// Get entity unique identifier collection based on specified container name
        /// </summary>
        /// <param name="containerName">Indicates qualifier name for the container</param>
        /// <returns>Returns entity unique identifier collection</returns>
        public EntityUniqueIdentifierCollection GetByContainerName(String containerName)
        {
            EntityUniqueIdentifierCollection entityUniqueIdentifiers = null;

            if (this._items.Count > 0)
            {
                entityUniqueIdentifiers = new EntityUniqueIdentifierCollection();

                foreach (EntityUniqueIdentifier entityUniqueIdentifier in this._items)
                {
                    if (String.Compare(entityUniqueIdentifier.ContainerName, containerName, true) == 0)
                    {
                        entityUniqueIdentifiers.Add(entityUniqueIdentifier);
                    }
                }
            }

            return entityUniqueIdentifiers;
        }

        /// <summary>
        /// Gets entity unique identifier based on specified container name and category path
        /// </summary>
        /// <param name="containerName">Indicates container name for the container</param>
        /// <param name="categoryPath">Indicates category path for the entity</param>
        /// <returns>Returns filtered entity unique identifier based on given input parameters</returns>
        public EntityUniqueIdentifier Get(String containerName, String categoryPath)
        {
            if (this._items.Count > 0)
            {
                foreach (EntityUniqueIdentifier entityUniqueIdentifier in this._items)
                {
                    if (String.Compare(entityUniqueIdentifier.ContainerName, containerName, true) == 0 &&
                       String.Compare(entityUniqueIdentifier.CategoryPath, categoryPath, true) == 0)
                    {
                        return entityUniqueIdentifier;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Gets entity unique identifier based on specified container level
        /// </summary>
        /// <param name="containerLevel">Indicates container level for the container hierarchy</param>
        /// <returns>Returns filtered entity unique identifier based on given input parameters</returns>
        public EntityUniqueIdentifierCollection Get(Int32 containerLevel)
        {
            EntityUniqueIdentifierCollection entityUniqueIdentifiers = null;

            if (this._items.Count > 0)
            {
                entityUniqueIdentifiers = new EntityUniqueIdentifierCollection();

                foreach (EntityUniqueIdentifier entityUniqueIdentifier in this._items)
                {
                    if (entityUniqueIdentifier.ContainerLevel == containerLevel)
                    {
                        entityUniqueIdentifiers.Add(entityUniqueIdentifier);
                    }
                }
            }

            return entityUniqueIdentifiers;
        }

        /// <summary>
        ///  Gets entity unique identifier based on specified entity id
        /// </summary>
        /// <param name="entityId">Indicates entity id of the entity</param>
        /// <returns>Returns filtered entity unique identifier based on given input parameters</returns>
        public EntityUniqueIdentifier Get(Int64 entityId)
        {
            if (this._items.Count > 0)
            {
                foreach (EntityUniqueIdentifier entityUniqueIdentifier in this._items)
                {
                    if (entityUniqueIdentifier.EntityId == entityId)
                    {
                        return entityUniqueIdentifier;
                    }
                }
            }

            return null;
        }

        #endregion
    }
}
