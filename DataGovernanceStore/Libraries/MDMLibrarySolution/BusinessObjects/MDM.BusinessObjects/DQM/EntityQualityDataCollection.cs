using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Collections.ObjectModel;

namespace MDM.BusinessObjects.DQM
{
    using MDM.Interfaces;

    /// <summary>
    /// Specifies Entity quality status data collection
    /// </summary>
    [DataContract]
    public class EntityQualityDataCollection : IEntityQualityDataCollection
    {
        #region Fields

        [DataMember]
        private Collection<EntityQualityData> _entityQualityDataCollection;

        #endregion

        #region Constructors

        /// <summary>
        /// Parameterless constructor
        /// </summary>
        public EntityQualityDataCollection()
        {
            _entityQualityDataCollection = new Collection<EntityQualityData>();
        }

        /// <summary>
        /// Initialize EntityQualityDataCollection from IList
        /// </summary>        
        public EntityQualityDataCollection(IList<EntityQualityData> entityQualityDataList)
        {
            _entityQualityDataCollection = new Collection<EntityQualityData>(entityQualityDataList);
        }

        #endregion

        #region Properties

        #endregion

        #region Private methods

        /// <summary>
        /// Get the first entityQualityData from collection
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private EntityQualityData Get(Int32 id)
        {
            return _entityQualityDataCollection.FirstOrDefault(a => a.EntityId == id);
        }

        #endregion

        /// <summary>
        /// Remove entityQualityData object from EntityQualityDataCollection
        /// </summary>
        /// <param name="id">/Denotes id of the entityQualityData</param>
        /// <returns></returns>
        public Boolean Remove(Int32 id)
        {
            EntityQualityData entityQualityData = Get(id);

            return _entityQualityDataCollection != null && this.Remove(entityQualityData);
        }

        /// <summary>
        /// Determines whether two Object instances are equal
        /// </summary>
        /// <param name="obj">The Object to compare with the current Object</param>
        /// <returns>true if the specified Object is equal to the current Object; otherwise, false</returns>
        public override Boolean Equals(object obj)
        {
            if (obj != null)
            {
                EntityQualityDataCollection objectToBeCompared = obj as EntityQualityDataCollection;
                if (objectToBeCompared != null)
                {
                    if (ReferenceEquals(this, objectToBeCompared))
                    {
                        return true;
                    }

                    Int32 entitiesQualityDataUnion = _entityQualityDataCollection.ToList().Union(objectToBeCompared.ToList()).Count();
                    Int32 entitiesQualityDataIntersect = _entityQualityDataCollection.ToList().Intersect(objectToBeCompared.ToList()).Count();

                    return entitiesQualityDataUnion == entitiesQualityDataIntersect;
                }
            }
            return false;
        }

        /// <summary>
        /// Serves as a hash function for type
        /// </summary>
        /// <returns>A hash code for the current Object</returns>
        public override Int32 GetHashCode()
        {
            return _entityQualityDataCollection.Sum(item => item.GetHashCode());
        }

        /// <summary>
        /// Add entityQualityData item to entityQualityDataCollection
        /// </summary>
        /// <param name="item">entity Quality data item to add</param>
        public void Add(EntityQualityData item)
        {
            _entityQualityDataCollection.Add(item);
        }
        
        /// <summary>
        /// Add IEntityQualityData item to entityQualityDataCollection
        /// </summary>
        /// <param name="item">entity Quality data item to add</param>
        public void Add(IEntityQualityData item)
        {
            _entityQualityDataCollection.Add((EntityQualityData)item);
        }

        /// <summary>
        /// Removes all EntityQualityData from collection
        /// </summary>
        public void Clear()
        {
            _entityQualityDataCollection.Clear();
        }

        /// <summary>
        /// Determines whether the EntityQualityDataCollection contains a specific EntityQualityData
        /// </summary>
        /// <param name="item">The EntityQualityData object to locate in the EntityQualityDataCollection</param>
        /// <returns>
        /// <para>true : If EntityQualityData was found in EntityQualityDataCollection</para>
        /// <para>false : If EntityQualityData was not found in EntityQualityDataCollection</para>
        /// </returns>
        public Boolean Contains(EntityQualityData item)
        {
            return _entityQualityDataCollection.Contains(item);
        }

        /// <summary>
        /// Copies the elements of the EntityQualityDataCollection to an
        /// System.Array, starting at a particular System.Array index
        /// </summary>
        /// <param name="array"> 
        ///  The one-dimensional System.Array that is the destination of the elements copied from EntityQualityDataCollection. The System.Array must have zero-based indexing
        /// </param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins</param>
        public void CopyTo(EntityQualityData[] array, Int32 arrayIndex)
        {
            _entityQualityDataCollection.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Get the count of no. of EntityQualityData in EntityQualityDataCollection
        /// </summary>
        public Int32 Count
        {
            get { return _entityQualityDataCollection.Count; }
        }

        /// <summary>
        /// Check if EntityQualityDataCollection is read-only
        /// </summary>
        public Boolean IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the EntityQualityDataCollection
        /// </summary>
        /// <param name="item">The EntityQualityData object to remove from the EntityQualityDataCollection</param>
        /// <returns>true if item is successfully removed; otherwise, false. This method also returns false if item was not found in the original EntityQualityDataCollection</returns>
        public Boolean Remove(EntityQualityData item)
        {
            return _entityQualityDataCollection.Remove(item);
        }

        /// <summary>
        /// Returns an enumerator that iterates through a EntityQualityDataCollection
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection</returns>
        public IEnumerator<EntityQualityData> GetEnumerator()
        {
            return _entityQualityDataCollection.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through a EntityQualityDataCollection
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return _entityQualityDataCollection.GetEnumerator();
        }
    }
}