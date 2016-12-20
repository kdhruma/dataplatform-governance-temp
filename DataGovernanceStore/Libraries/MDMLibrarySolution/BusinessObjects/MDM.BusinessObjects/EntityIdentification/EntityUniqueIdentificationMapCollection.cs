using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;

namespace MDM.BusinessObjects.EntityIdentification
{
    using Core;
    using MDM.Interfaces.EntityIdentification;

    /// <summary>
    /// Specifies EntityUniqueIdentification Map Collection
    /// </summary>
    [DataContract]
    public class EntityUniqueIdentificationMapCollection : ICollection<EntityUniqueIdentificationMap>, IEnumerable<EntityUniqueIdentificationMap>, IEntityUniqueIdentificationMapCollection
    {
        #region Fields

        /// <summary>
        /// Field denoting collection of Entity Identifier Map objects
        /// </summary>
        [DataMember]
        private Collection<EntityUniqueIdentificationMap> _entityIdentifierMapCollection = new Collection<EntityUniqueIdentificationMap>();

        #endregion

        #region Properties

        #endregion

        #region Constructors

        /// <summary>
        /// Parameterless constructor
        /// </summary>
        public EntityUniqueIdentificationMapCollection() : base() { }


        /// <summary>
        /// Initialize EntityIdentifierMapCollection from IList of value
        /// </summary>
        /// <param name="entityIdentifierMapList">List of EntityIdentifierMap object</param>
        public EntityUniqueIdentificationMapCollection(IList<EntityUniqueIdentificationMap> entityIdentifierMapList)
        {
            this._entityIdentifierMapCollection = new Collection<EntityUniqueIdentificationMap>(entityIdentifierMapList);
        }

        #endregion

        #region Methods

        #region Public Methods

        /// <summary>
        /// Gets the entity identifier map for the given reference id
        /// </summary>
        /// <param name="entityIdentifierMapId">Indicates Id of an entity for which mapping should be fetched</param>
        /// <returns>Returns Attribute interface</returns>
        /// <exception cref="ArgumentException">Attribute Id must be greater than 0</exception>
        /// <exception cref="NullReferenceException">If there are no attributes to search in</exception>
        public IEntityUniqueIdentificationMap GetEntityIdentifierMap(Int64 entityIdentifierMapId)
        {
            if (this._entityIdentifierMapCollection == null)
            {
                throw new NullReferenceException("There are no attributes to search in");
            }

            EntityUniqueIdentificationMap entityIdentifierMap = null;

            var filteredEntityIdentifierMaps = this._entityIdentifierMapCollection.Where(e => e.Id == entityIdentifierMapId);

            if (filteredEntityIdentifierMaps != null && filteredEntityIdentifierMaps.Any())
            {
                entityIdentifierMap = filteredEntityIdentifierMaps.First();
            }

            return entityIdentifierMap;
        }

        /// <summary>
        /// Gets the entity identifier map collection for the given reference id
        /// </summary>
        /// <param name="entityIdentifierMapId"></param>
        /// <returns></returns>
        public IEntityUniqueIdentificationMapCollection GetEntityIdentifierMapCollection(Int64 entityIdentifierMapId)
        {
            if (this._entityIdentifierMapCollection == null)
            {
                throw new NullReferenceException("There are no attributes to search in");
            }

            EntityUniqueIdentificationMapCollection entityIdentifierMapCollection = null;

            var filteredEntityIdentifierMaps = this._entityIdentifierMapCollection.Where(e => e.Id == entityIdentifierMapId);

            if (filteredEntityIdentifierMaps != null && filteredEntityIdentifierMaps.Any())
            {
                entityIdentifierMapCollection = new EntityUniqueIdentificationMapCollection();

                foreach (EntityUniqueIdentificationMap entityIdentifierMap in filteredEntityIdentifierMaps.ToList())
                {
                    entityIdentifierMapCollection.Add(entityIdentifierMap);
                }
            }

            return entityIdentifierMapCollection;
        }

        /// <summary>
        /// Check if EntityIdentifierMapCollection contains entity identifier map with given externalId
        /// </summary>
        /// <param name="externalId">ExternalId to search in EntityIdentifierMapCollection</param>
        /// <returns>
        /// <para>true : If entity identifier map found in EntityIdentifierMapCollection</para>
        /// <para>false : If entity identifier map not found in EntityIdentifierMapCollection</para>
        /// </returns>
        public bool Contains(String externalId)
        {
            if (GetEntityIdentifierMap(externalId) != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Remove entity identifier map object from EntityIdentifierMapCollection
        /// </summary>
        /// <param name="externalId">External Id of entity identifier map which is to be removed from collection</param>
        /// <returns>true if item is successfully removed; otherwise, false. This method also returns false if item was not found in the original collection</returns>
        public bool Remove(String externalId)
        {
            EntityUniqueIdentificationMap entityIdentifierMap = GetEntityIdentifierMap(externalId);

            if (entityIdentifierMap == null)
            {
                throw new ArgumentException("No entity mapping found for given external id");
            }
            else
            {
                return this.Remove(entityIdentifierMap);
            }
        }

        /// <summary>
        /// Get Xml representation of Entity Identifier Map Collection
        /// </summary>
        /// <returns>Xml representation of Entity Identifier Map Collection</returns>
        public String ToXml()
        {
            String returnXml = String.Empty;

            returnXml = "<EntityIdentifierMaps>";

            if (this._entityIdentifierMapCollection != null && this._entityIdentifierMapCollection.Count > 0)
            {
                foreach (EntityUniqueIdentificationMap attrModel in this._entityIdentifierMapCollection)
                {
                    returnXml = String.Concat(returnXml, attrModel.ToXml());
                }
            }

            returnXml = String.Concat(returnXml, "</EntityIdentifierMaps>");

            return returnXml;
        }

        /// <summary>
        /// Get Xml representation of Entity Identifier Map Collection based on requested object serialization
        /// </summary>
        /// <param name="objectSerialization">Type of the serialization option</param>
        /// <returns>Xml representation of Entity Identifier Map Collection</returns>
        public String ToXml(ObjectSerialization objectSerialization)
        {
            String returnXml = String.Empty;

            returnXml = "<EntityIdentifierMaps>";

            if (this._entityIdentifierMapCollection != null && this._entityIdentifierMapCollection.Count > 0)
            {
                foreach (EntityUniqueIdentificationMap attrModel in this._entityIdentifierMapCollection)
                {
                    returnXml = String.Concat(returnXml, attrModel.ToXml(objectSerialization));
                }
            }

            returnXml = String.Concat(returnXml, "</EntityIdentifierMaps>");

            return returnXml;
        }

        /// <summary>
        /// Determines whether two Object instances are equal.
        /// </summary>
        /// <param name="obj">The Object to compare with the current Object.</param>
        /// <returns>true if the specified Object is equal to the current Object; otherwise, false.</returns>
        public override bool Equals(object obj)
        {
            if (obj is EntityUniqueIdentificationMapCollection)
            {
                EntityUniqueIdentificationMapCollection objectToBeCompared = obj as EntityUniqueIdentificationMapCollection;
                Int32 entityIdentifierMapsUnion = this._entityIdentifierMapCollection.ToList().Union(objectToBeCompared.ToList()).Count();
                Int32 entityIdentifierMapsIntersect = this._entityIdentifierMapCollection.ToList().Intersect(objectToBeCompared.ToList()).Count();

                if (entityIdentifierMapsUnion != entityIdentifierMapsIntersect)
                {
                    return false;
                }

                return true;
            }

            return false;
        }

        /// <summary>
        /// Serves as a hash function for type
        /// </summary>
        /// <returns>A hash code for the current Object.</returns>
        public override int GetHashCode()
        {
            Int32 hashCode = 0;

            foreach (EntityUniqueIdentificationMap entityIdentifierMap in this._entityIdentifierMapCollection)
            {
                hashCode += entityIdentifierMap.GetHashCode();
            }

            return hashCode;
        }

        #endregion

        #region Private Methods

        private EntityUniqueIdentificationMap GetEntityIdentifierMap(String externalId)
        {
            var filteredEntityIdentifierMaps = from entityIdentifierMap in this._entityIdentifierMapCollection
                                               where (entityIdentifierMap.ExternalId == externalId)
                                               select entityIdentifierMap;

            if (filteredEntityIdentifierMaps.Any())
            {
                return filteredEntityIdentifierMaps.First();
            }
            else
            {
                return null;
            }
        }

        #endregion

        #region ICollection<EntityIdentifierMap> Members

        /// <summary>
        /// Add entity identifier map object in collection
        /// </summary>
        /// <param name="item">entity identifier map to add in collection</param>
        public void Add(EntityUniqueIdentificationMap item)
        {
            this._entityIdentifierMapCollection.Add(item);
        }

        /// <summary>
        /// Removes all entity identifier map from collection
        /// </summary>
        public void Clear()
        {
            this._entityIdentifierMapCollection.Clear();
        }

        /// <summary>
        /// Determines whether the EntityIdentifierMapCollection contains a specific entity identifier map.
        /// </summary>
        /// <param name="entityIdentifierMap">The entity identifier map object to locate in the EntityIdentifierMapCollection.</param>
        /// <returns>
        /// <para>true : If entity identifier map found in EntityIdentifierMapCollection</para>
        /// <para>false : If entity identifier map found not in EntityIdentifierMapCollection</para>
        /// </returns>
        public bool Contains(EntityUniqueIdentificationMap entityIdentifierMap)
        {
            return this._entityIdentifierMapCollection.Contains(entityIdentifierMap);
        }

        /// <summary>
        /// Copies the elements of the EntityIdentifierMapCollection to an
        /// System.Array, starting at a particular System.Array index.
        /// </summary>
        /// <param name="array"> 
        ///  The one-dimensional System.Array that is the destination of the elements
        ///  copied from EntityIdentifierMapCollection. The System.Array must have zero-based indexing.
        /// </param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
        public void CopyTo(EntityUniqueIdentificationMap[] array, int arrayIndex)
        {
            this._entityIdentifierMapCollection.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Get the count of no. of mappings in EntityIdentifierMapCollection
        /// </summary>
        public int Count
        {
            get
            {
                return this._entityIdentifierMapCollection.Count;
            }
        }

        /// <summary>
        /// Check if EntityIdentifierMapCollection is read-only.
        /// </summary>
        public bool IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// Removes the first occurrence of a specific entity identifier map from the EntityIdentifierMapCollection.
        /// </summary>
        /// <param name="entityIdentifierMap">The entity identifier map object to remove from the EntityIdentifierMapCollection.</param>
        /// <returns>true if item is successfully removed; otherwise, false. This method also returns false if item was not found in the original collection</returns>
        public bool Remove(EntityUniqueIdentificationMap entityIdentifierMap)
        {
            return this._entityIdentifierMapCollection.Remove(entityIdentifierMap);
        }

        #endregion

        #region IEnumerable<EntityIdentifierMap> Members

        /// <summary>
        /// Returns an enumerator that iterates through a EntityIdentifierMapCollection.
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection.</returns>
        public IEnumerator<EntityUniqueIdentificationMap> GetEnumerator()
        {
            return this._entityIdentifierMapCollection.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        /// <summary>
        /// Returns an enumerator that iterates through a EntityIdentifierMapCollection.
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator)this._entityIdentifierMapCollection.GetEnumerator();
        }

        #endregion

        #endregion

        #region IEntityIdentifierMapCollection Members

        /// <summary>
        /// Add Ientity identifier map object in IEntityIdentifierMapCollection
        /// </summary>
        /// <param name="iEntityIdentifierMap">entity identifier map to add in IEntityIdentifierMapCollection</param>
        public void Add(IEntityUniqueIdentificationMap iEntityIdentifierMap)
        {
            if (iEntityIdentifierMap != null)
            {
                this.Add((EntityUniqueIdentificationMap)iEntityIdentifierMap);
            }
        }

        /// <summary>
        /// Determines whether the EntityIdentifierMapCollection contains a specific entity identifier map.
        /// </summary>
        /// <param name="iEntityIdentifierMap">The entity identifier map object to locate in the IEntityIdentifierMapCollection.</param>
        /// <returns>
        /// <para>true : If iEntityIdentifierMap found in IEntityIdentifierMapCollection</para>
        /// <para>false : If iEntityIdentifierMap not found in IEntityIdentifierMapCollection</para>
        /// </returns>
        public bool Contains(IEntityUniqueIdentificationMap iEntityIdentifierMap)
        {
            if (iEntityIdentifierMap == null)
            {
                return false;
            }
            else
            {
                return this.Contains((EntityUniqueIdentificationMap)iEntityIdentifierMap);
            }
        }

        /// <summary>
        /// Removes the first occurrence of a specific entity identifier map from the IEntityIdentifierMapCollection.
        /// </summary>
        /// <param name="iEntityIdentifierMap">The entity identifier map object to remove from the IEntityIdentifierMapCollection.</param>
        /// <returns>true if item is successfully removed; otherwise, false. This method also returns false if item was not found in the original collection</returns>
        public bool Remove(IEntityUniqueIdentificationMap iEntityIdentifierMap)
        {
            if (iEntityIdentifierMap == null)
            {
                return false;
            }
            else
            {
                return this.Remove((EntityUniqueIdentificationMap)iEntityIdentifierMap);
            }
        }

        #endregion IEntityIdentifierMapCollection Members
    }
}
