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
    /// Specifies Collection of EntitySearchData Object
    /// </summary>
    [DataContract]
    public class EntitySearchDataCollection : ICollection<EntitySearchData>, IEnumerable<EntitySearchData>, IEntitySearchDataCollection
    {
        #region Fields

        /// <summary>
        /// Field denoting collection of EntitySearchData objects
        /// </summary>
        [DataMember]
        private Collection<EntitySearchData> _entitySearchDataCollection = new Collection<EntitySearchData>();

        #endregion

        #region Constructor

        #endregion

        #region Properties

        #endregion

        #region Methods

        #region Public Methods

        /// <summary>
        /// Gets entitySearchData with specified entitySearchDataId from current entitySearchDataCollection
        /// </summary>
        /// <param name="entitySearchDataId">Id of an EntitySearchData to search in current EntitySearchDataCollection</param>
        /// <returns>EntitySearchData Interface</returns>
        /// <exception cref="ArgumentException">EntitySearchData Id must be greater than 0</exception>
        /// <exception cref="NullReferenceException">If there are no EntitySearchData to search in</exception>
        public IEntitySearchData GetEntitySearchData(Int64 entitySearchDataId)
        {
            if (this._entitySearchDataCollection == null)
            {
                //todo:: Use MDMException - user messge code
                throw new NullReferenceException("There are no EntitySearchData to search in");
            }

            EntitySearchData entitySearchData = null;

            var filteredEntitySearchData = this._entitySearchDataCollection.Where(e => e.Id == entitySearchDataId);

            if (filteredEntitySearchData != null && filteredEntitySearchData.Any())
                entitySearchData = filteredEntitySearchData.First();

            return entitySearchData;
        }

        /// <summary>
        /// Gets entitySearchData with specified entitySearchDataId from current entitySearchDataCollection
        /// </summary>
        /// <param name="entityId">Indicates Id of an EntitySearchData to search in current EntitySearchDataCollection</param>
        /// <returns>EntitySearchData Interface</returns>
        /// <exception cref="ArgumentException">EntitySearchData Id must be greater than 0</exception>
        /// <exception cref="NullReferenceException">If there are no EntitySearchData to search in</exception>
        public IEntitySearchData GetEntitySearchDataByEntityId(Int64 entityId)
        {
            if (this._entitySearchDataCollection == null)
            {
                //todo:: Use MDMException - user messge code
                throw new NullReferenceException("There are no EntitySearchData to search in");
            }

            EntitySearchData entitySearchData = null;

            var filteredEntitySearchData = this._entitySearchDataCollection.Where(e => e.EntityId == entityId);

            if (filteredEntitySearchData != null && filteredEntitySearchData.Any())
                entitySearchData = filteredEntitySearchData.First();

            return entitySearchData;
        }

        /// <summary>
        /// Check if EntitySearchDataCollection contains entitySearchData with given entityId
        /// </summary>
        /// <param name="entityId">entityId to search in EntitySearchDataCollection</param>
        /// <returns>
        /// <para>true : If entity map found in EntitySearchDataCollection</para>
        /// <para>false : If entity map not found in EntitySearchDataCollection</para>
        /// </returns>
        public bool Contains(Int64 entityId)
        {
            if (GetEntitySerachDataByEntityId(entityId) != null)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Remove EntitySearchData object from EntitySearchDataCollection
        /// </summary>
        /// <param name="entityId">Entity Id of EntitySearchData which is to be removed from collection</param>
        /// <returns>true if item is successfully removed; otherwise, false. This method also returns false if item was not found in the original collection</returns>
        public bool Remove(Int64 entityId)
        {
            EntitySearchData entitySearchData = GetEntitySerachDataByEntityId(entityId);

            if (entitySearchData == null)
                throw new ArgumentException("No Search data found for given Entity id");
            else
                return this.Remove(entitySearchData);
        }

        /// <summary>
        /// Get Xml representation of Entity SearchData Collection
        /// </summary>
        /// <returns>Xml representation of Entity SearchData Collection</returns>
        public String ToXml()
        {
            String returnXml = String.Empty;

            returnXml = "<EntitySearchDataCollection>";

            if (this._entitySearchDataCollection != null && this._entitySearchDataCollection.Count > 0)
            {
                foreach (EntitySearchData entitySearchData in this._entitySearchDataCollection)
                {
                    returnXml = String.Concat(returnXml, entitySearchData.ToXml());
                }
            }

            returnXml = String.Concat(returnXml, "</EntitySearchDataCollection>");

            return returnXml;
        }

        /// <summary>
        /// Get Xml representation of Entity SearchData Collection based on requested object serialization
        /// </summary>
        /// <param name="objectSerialization">Type of the serialization option</param>
        /// <returns>Xml representation of Entity SearchData Collection</returns>
        public String ToXml(ObjectSerialization objectSerialization)
        {
            String returnXml = String.Empty;

            returnXml = "<EntitySearchDataCollection>";

            if (this._entitySearchDataCollection != null && this._entitySearchDataCollection.Count > 0)
            {
                foreach (EntitySearchData entitySearchData in this._entitySearchDataCollection)
                {
                    returnXml = String.Concat(returnXml, entitySearchData.ToXml(objectSerialization));
                }
            }

            returnXml = String.Concat(returnXml, "</EntitySearchDataCollection>");

            return returnXml;
        }

        /// <summary>
        /// Determines whether two Object instances are equal.
        /// </summary>
        /// <param name="obj">The Object to compare with the current Object.</param>
        /// <returns>true if the specified Object is equal to the current Object; otherwise, false.</returns>
        public override bool Equals(object obj)
        {
            if (obj is EntitySearchDataCollection)
            {
                EntitySearchDataCollection objectToBeCompared = obj as EntitySearchDataCollection;
                Int32 EntitySearchDataCollectionUnion = this._entitySearchDataCollection.ToList().Union(objectToBeCompared.ToList()).Count();
                Int32 EntitySearchDataCollectionIntersect = this._entitySearchDataCollection.ToList().Intersect(objectToBeCompared.ToList()).Count();

                if (EntitySearchDataCollectionUnion != EntitySearchDataCollectionIntersect)
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

            foreach (EntitySearchData entitySearchData in this._entitySearchDataCollection)
            {
                hashCode += entitySearchData.GetHashCode();
            }

            return hashCode;
        }

        #endregion

        #region Private Methods

        private EntitySearchData GetEntitySerachDataByEntityId(Int64 entityId)
        {
            var filteredEntitySearchData = from entitySearchData in this._entitySearchDataCollection
                                     where (entitySearchData.EntityId == entityId)
                                     select entitySearchData;

            if (filteredEntitySearchData.Any())
                return filteredEntitySearchData.First();
            else
                return null;
        }

        #endregion

        #region ICollection<EntityMap> Members

        /// <summary>
        /// Add EntitySearchData object in collection
        /// </summary>
        /// <param name="item">EntitySearchData to add in collection</param>
        public void Add(EntitySearchData item)
        {
            this._entitySearchDataCollection.Add(item);
        }

        /// <summary>
        /// Removes all EntitySearchData from collection
        /// </summary>
        public void Clear()
        {
            this._entitySearchDataCollection.Clear();
        }

        /// <summary>
        /// Determines whether the EntitySearchDataCollection contains a specific EntitySearchData.
        /// </summary>
        /// <param name="entitySearchData">The EntitySearchData object to locate in the EntitySearchDataCollection.</param>
        /// <returns>
        /// <para>true : If entity map found in EntitySearchDataCollection</para>
        /// <para>false : If entity map found not in EntitySearchDataCollection</para>
        /// </returns>
        public bool Contains(EntitySearchData entitySearchData)
        {
            return this._entitySearchDataCollection.Contains(entitySearchData);
        }

        /// <summary>
        /// Copies the elements of the EntitySearchDataCollection to an
        /// System.Array, starting at a particular System.Array index.
        /// </summary>
        /// <param name="array"> 
        ///  The one-dimensional System.Array that is the destination of the elements
        ///  copied from EntitySearchDataCollection. The System.Array must have zero-based indexing.
        /// </param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
        public void CopyTo(EntitySearchData[] array, int arrayIndex)
        {
            this._entitySearchDataCollection.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Get the count of no. of mappings in EntitySearchDataCollection
        /// </summary>
        public int Count
        {
            get
            {
                return this._entitySearchDataCollection.Count;
            }
        }

        /// <summary>
        /// Check if EntitySearchDataCollection is read-only.
        /// </summary>
        public bool IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// Removes the first occurrence of a specific EntitySearchData from the EntitySearchDataCollection.
        /// </summary>
        /// <param name="entitySearchData">The EntitySearchData object to remove from the EntitySearchDataCollection.</param>
        /// <returns>true if item is successfully removed; otherwise, false. This method also returns false if item was not found in the original collection</returns>
        public bool Remove(EntitySearchData entitySearchData)
        {
            return this._entitySearchDataCollection.Remove(entitySearchData);
        }

        #endregion

        #region IEnumerable<EntitySearchData> Members

        /// <summary>
        /// Returns an enumerator that iterates through a EntitySearchDataCollection.
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection.</returns>
        public IEnumerator<EntitySearchData> GetEnumerator()
        {
            return this._entitySearchDataCollection.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        /// <summary>
        /// Returns an enumerator that iterates through a EntitySearchDataCollection.
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator)this._entitySearchDataCollection.GetEnumerator();
        }

        #endregion

        #endregion
    }
}
