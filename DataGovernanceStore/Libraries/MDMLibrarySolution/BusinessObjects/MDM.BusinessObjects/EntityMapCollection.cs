using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;

namespace MDM.BusinessObjects
{
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Specifies Entity Operation Result Collection
    /// </summary>
    [DataContract]
    public class EntityMapCollection : ICollection<EntityMap>, IEnumerable<EntityMap>, IEntityMapCollection
    {
        #region Fields

        /// <summary>
        /// Field denoting collection of Entity Map objects
        /// </summary>
        [DataMember]
        private Collection<EntityMap> _entityMapCollection = new Collection<EntityMap>();

        #endregion

        #region Properties

        #endregion

        #region Constructors

        /// <summary>
        /// Parameterless constructor
        /// </summary>
        public EntityMapCollection() : base() { }


        /// <summary>
        /// Initialize EntityMapCollection from IList of value
        /// </summary>
        /// <param name="entityMapList">List of EntityMap object</param>
        public EntityMapCollection(IList<EntityMap> entityMapList)
        {
            this._entityMapCollection = new Collection<EntityMap>(entityMapList);
        }

        #endregion

        #region Methods

        #region Public Methods

        /// <summary>
        /// Gets the entity map for the given reference id
        /// </summary>
        /// <param name="entityMapId">Indicates Id of an entity for which mapping should be fetched</param>
        /// <returns>Returns Attribute interface</returns>
        /// <exception cref="ArgumentException">Attribute Id must be greater than 0</exception>
        /// <exception cref="NullReferenceException">If there are no attributes to search in</exception>
        public IEntityMap GetEntityMap(Int64 entityMapId)
        {
            if (this._entityMapCollection == null)
            {
                throw new NullReferenceException("There are no attributes to search in");
            }

            EntityMap entityMap = null;

            var filteredEntityMaps = this._entityMapCollection.Where(e => e.Id == entityMapId);

            if (filteredEntityMaps != null && filteredEntityMaps.Any())
                entityMap = filteredEntityMaps.First();

            return entityMap;
        }

        /// <summary>
        /// Gets the entity map collection for the given reference id
        /// </summary>
        /// <param name="entityMapId"></param>
        /// <returns></returns>
        public IEntityMapCollection GetEntityMapCollection(Int64 entityMapId)
        {
            if (this._entityMapCollection == null)
            {
                throw new NullReferenceException("There are no attributes to search in");
            }

            EntityMapCollection entityMapCollection = null;

            var filteredEntityMaps = this._entityMapCollection.Where(e => e.Id == entityMapId);

            if (filteredEntityMaps != null && filteredEntityMaps.Any())
            {
                entityMapCollection = new EntityMapCollection();

                foreach (EntityMap entityMap in filteredEntityMaps.ToList())
                {
                    entityMapCollection.Add(entityMap);
                }
            }

            return entityMapCollection;
        }

        /// <summary>
        /// Check if EntityMapCollection contains entity map with given externalId
        /// </summary>
        /// <param name="externalId">ExternalId to search in EntityMapCollection</param>
        /// <returns>
        /// <para>true : If entity map found in EntityMapCollection</para>
        /// <para>false : If entity map not found in EntityMapCollection</para>
        /// </returns>
        public bool Contains(String externalId)
        {
            if (GetEntityMap(externalId) != null)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Remove entity map object from EntityMapCollection
        /// </summary>
        /// <param name="externalId">External Id of entity map which is to be removed from collection</param>
        /// <returns>true if item is successfully removed; otherwise, false. This method also returns false if item was not found in the original collection</returns>
        public bool Remove(String externalId)
        {
            EntityMap entityMap = GetEntityMap(externalId);

            if (entityMap == null)
                throw new ArgumentException("No entity mapping found for given external id");
            else
                return this.Remove(entityMap);
        }

        /// <summary>
        /// Get Xml representation of Entity Map Collection
        /// </summary>
        /// <returns>Xml representation of Entity Map Collection</returns>
        public String ToXml()
        {
            String returnXml = String.Empty;

            returnXml = "<EntityMaps>";

            if (this._entityMapCollection != null && this._entityMapCollection.Count > 0)
            {
                foreach (EntityMap attrModel in this._entityMapCollection)
                {
                    returnXml = String.Concat(returnXml, attrModel.ToXml());
                }
            }

            returnXml = String.Concat(returnXml, "</EntityMaps>");

            return returnXml;
        }

        /// <summary>
        /// Get Xml representation of Entity Map Collection based on requested object serialization
        /// </summary>
        /// <param name="objectSerialization">Type of the serialization option</param>
        /// <returns>Xml representation of Entity Map Collection</returns>
        public String ToXml(ObjectSerialization objectSerialization)
        {
            String returnXml = String.Empty;

            returnXml = "<EntityMaps>";

            if (this._entityMapCollection != null && this._entityMapCollection.Count > 0)
            {
                foreach (EntityMap attrModel in this._entityMapCollection)
                {
                    returnXml = String.Concat(returnXml, attrModel.ToXml(objectSerialization));
                }
            }

            returnXml = String.Concat(returnXml, "</EntityMaps>");

            return returnXml;
        }

        /// <summary>
        /// Determines whether two Object instances are equal.
        /// </summary>
        /// <param name="obj">The Object to compare with the current Object.</param>
        /// <returns>true if the specified Object is equal to the current Object; otherwise, false.</returns>
        public override bool Equals(object obj)
        {
            if (obj is EntityMapCollection)
            {
                EntityMapCollection objectToBeCompared = obj as EntityMapCollection;
                Int32 entityMapsUnion = this._entityMapCollection.ToList().Union(objectToBeCompared.ToList()).Count();
                Int32 entityMapsIntersect = this._entityMapCollection.ToList().Intersect(objectToBeCompared.ToList()).Count();

                if (entityMapsUnion != entityMapsIntersect)
                    return false;

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

            foreach (EntityMap entityMap in this._entityMapCollection)
            {
                hashCode += entityMap.GetHashCode();
            }

            return hashCode;
        }

        #endregion

        #region Private Methods

        private EntityMap GetEntityMap(String externalId)
        {
            var filteredEntityMaps = from entityMap in this._entityMapCollection
                                     where (entityMap.ExternalId == externalId)
                                     select entityMap;

            if (filteredEntityMaps.Any())
                return filteredEntityMaps.First();
            else
                return null;
        }

        #endregion

        #region ICollection<EntityMap> Members

        /// <summary>
        /// Add entity map object in collection
        /// </summary>
        /// <param name="item">entity map to add in collection</param>
        public void Add(EntityMap item)
        {
            this._entityMapCollection.Add(item);
        }

        /// <summary>
        /// Removes all entity map from collection
        /// </summary>
        public void Clear()
        {
            this._entityMapCollection.Clear();
        }

        /// <summary>
        /// Determines whether the EntityMapCollection contains a specific entity map.
        /// </summary>
        /// <param name="entityMap">The entity map object to locate in the EntityMapCollection.</param>
        /// <returns>
        /// <para>true : If entity map found in EntityMapCollection</para>
        /// <para>false : If entity map found not in EntityMapCollection</para>
        /// </returns>
        public bool Contains(EntityMap entityMap)
        {
            return this._entityMapCollection.Contains(entityMap);
        }

        /// <summary>
        /// Copies the elements of the EntityMapCollection to an
        /// System.Array, starting at a particular System.Array index.
        /// </summary>
        /// <param name="array"> 
        ///  The one-dimensional System.Array that is the destination of the elements
        ///  copied from EntityMapCollection. The System.Array must have zero-based indexing.
        /// </param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
        public void CopyTo(EntityMap[] array, int arrayIndex)
        {
            this._entityMapCollection.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Get the count of no. of mappings in EntityMapCollection
        /// </summary>
        public int Count
        {
            get
            {
                return this._entityMapCollection.Count;
            }
        }

        /// <summary>
        /// Check if EntityMapCollection is read-only.
        /// </summary>
        public bool IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// Removes the first occurrence of a specific entity map from the EntityMapCollection.
        /// </summary>
        /// <param name="entityMap">The entity map object to remove from the EntityMapCollection.</param>
        /// <returns>true if item is successfully removed; otherwise, false. This method also returns false if item was not found in the original collection</returns>
        public bool Remove(EntityMap entityMap)
        {
            return this._entityMapCollection.Remove(entityMap);
        }

        #endregion

        #region IEnumerable<EntityMap> Members

        /// <summary>
        /// Returns an enumerator that iterates through a EntityMapCollection.
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection.</returns>
        public IEnumerator<EntityMap> GetEnumerator()
        {
            return this._entityMapCollection.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        /// <summary>
        /// Returns an enumerator that iterates through a EntityMapCollection.
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator)this._entityMapCollection.GetEnumerator();
        }

        #endregion

        #endregion

        #region IEntityMapCollection Members

        /// <summary>
        /// Add IEntity map object in IEntityMapCollection
        /// </summary>
        /// <param name="iEntityMap">entity map to add in IEntityMapCollection</param>
        public void Add(IEntityMap iEntityMap)
        {
            if (iEntityMap != null)
            {
                this.Add((EntityMap)iEntityMap);
            }
        }

        /// <summary>
        /// Determines whether the EntityMapCollection contains a specific entity map.
        /// </summary>
        /// <param name="iEntityMap">The entity map object to locate in the IEntityMapCollection.</param>
        /// <returns>
        /// <para>true : If iEntityMap found in IEntityMapCollection</para>
        /// <para>false : If iEntityMap not found in IEntityMapCollection</para>
        /// </returns>
        public bool Contains(IEntityMap iEntityMap)
        {
            if (iEntityMap == null)
                return false;
            else
                return this.Contains((EntityMap)iEntityMap);
        }

        /// <summary>
        /// Removes the first occurrence of a specific entity map from the IEntityMapCollection.
        /// </summary>
        /// <param name="iEntityMap">The entity map object to remove from the IEntityMapCollection.</param>
        /// <returns>true if item is successfully removed; otherwise, false. This method also returns false if item was not found in the original collection</returns>
        public bool Remove(IEntityMap iEntityMap)
        {
            if (iEntityMap == null)
                return false;
            else
                return this.Remove((EntityMap)iEntityMap);
        }

        #endregion IEntityMapCollection Members
    }
}
