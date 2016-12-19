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
    /// Specifies Entity Quality report data collection
    /// </summary>
    [DataContract]
    public class EntityQualityReportDataCollection : IEntityQualityReportDataCollection
    {
        #region Fields

        [DataMember]
        private Collection<EntityQualityReportData> _entityQualityReportDataCollection = new Collection<EntityQualityReportData>();

        #endregion

        #region Public Method

         #region Constructors

        /// <summary>
        /// Parameterless constructor
        /// </summary>
        public EntityQualityReportDataCollection()
        {
            _entityQualityReportDataCollection = new Collection<EntityQualityReportData>();
        }

        /// <summary>
        /// Initialize EntityQualityReportDataCollection from IList
        /// </summary>
        /// <param name="entityQualityReportDataList">IList of EntityQualityReportData</param>
        public EntityQualityReportDataCollection(IList<EntityQualityReportData> entityQualityReportDataList)
        {
            _entityQualityReportDataCollection = new Collection<EntityQualityReportData>(entityQualityReportDataList);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Denotes total entities count for all pages of EntityQualityReportDataCollection records
        /// </summary>
        [DataMember] 
        public Int64 TotalEntitiesCount { get; set; }

        #endregion

        /// <summary>
        /// Remove entityQualityReport object from EntityQualityReportDataCollection
        /// </summary>
        /// <param name="id">Denotes the id of entityQualityReportData to remove from collection</param>
        /// <returns>true if success</returns>
        public Boolean Remove(Int32 id)
        {
            EntityQualityReportData entityQualityReportData = Get(id);

            return entityQualityReportData != null && this.Remove(entityQualityReportData);
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
                EntityQualityReportDataCollection objectToBeCompared = obj as EntityQualityReportDataCollection;
                if (objectToBeCompared != null)
                {
                    if (ReferenceEquals(this, objectToBeCompared))
                    {
                        return true;
                    }

                    Int32 entitiesQualityReportDataUnion = _entityQualityReportDataCollection.ToList().Union(objectToBeCompared.ToList()).Count();
                    Int32 entitiesQualityReportDataIntersect = _entityQualityReportDataCollection.ToList().Intersect(objectToBeCompared.ToList()).Count();

                    return entitiesQualityReportDataUnion == entitiesQualityReportDataIntersect;
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
            return _entityQualityReportDataCollection.Sum(item => item.GetHashCode());
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Get EntityQualityReportData from current EntityQualityReportData collection based on EntityQualityReportData Id
        /// </summary>
        /// <param name="id">Id of EntityQualityReportData which is to be searched</param>
        /// <returns>EntityQualityReportData having given EntityQualityReportDataId messageCode</returns>
        private EntityQualityReportData Get(Int32 id)
        {            
            return _entityQualityReportDataCollection.FirstOrDefault(a => a.Id == id);            
        }

        #endregion

        #region ICollection<EntityQualityReportData> Members

        /// <summary>
        /// Add EntityQualityReportData object to collection
        /// </summary>
        /// <param name="item">EntityQualityReportData to add in collection</param>
        public void Add(EntityQualityReportData item)
        {
            _entityQualityReportDataCollection.Add(item);
        }

        /// <summary>
        /// Add IEntityQualityReportData object to collection
        /// </summary>
        /// <param name="item">IEntityQualityReportData to add in collection</param>
        public void Add(IEntityQualityReportData item)
        {
            _entityQualityReportDataCollection.Add((EntityQualityReportData)item);
        }

        /// <summary>
        /// Removes all EntityQualityReportData from collection
        /// </summary>
        public void Clear()
        {
            _entityQualityReportDataCollection.Clear();
        }

        /// <summary>
        /// Determines whether the EntityQualityReportDataCollection contains a specific EntityQualityReportData
        /// </summary>
        /// <param name="item">The EntityQualityReportData object to locate in the EntityQualityReportDataCollection</param>
        /// <returns>
        /// <para>true : If EntityQualityReportData was found in EntityQualityReportDataCollection</para>
        /// <para>false : If EntityQualityReportData was not found in EntityQualityReportDataCollection</para>
        /// </returns>
        public Boolean Contains(EntityQualityReportData item)
        {
            return _entityQualityReportDataCollection.Contains(item);
        }

        /// <summary>
        /// Copies the elements of the EntityQualityReportDataCollection to an
        /// System.Array, starting at a particular System.Array index
        /// </summary>
        /// <param name="array"> 
        ///  The one-dimensional System.Array that is the destination of the elements copied from EntityQualityReportDataCollection. The System.Array must have zero-based indexing
        /// </param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins</param>
        public void CopyTo(EntityQualityReportData[] array, Int32 arrayIndex)
        {
            _entityQualityReportDataCollection.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Get the count of no. of EntityQualityReportData in EntityQualityReportDataCollection
        /// </summary>
        public Int32 Count
        {
            get
            {
                return _entityQualityReportDataCollection.Count;
            }
        }

        /// <summary>
        /// Check if EntityQualityReportDataCollection is read-only
        /// </summary>
        public Boolean IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the EntityQualityReportDataCollection
        /// </summary>
        /// <param name="item">The EntityQualityReportData object to remove from the EntityQualityReportDataCollection</param>
        /// <returns>true if item is successfully removed; otherwise, false. This method also returns false if item was not found in the original EntityQualityReportDataCollection</returns>
        public Boolean Remove(EntityQualityReportData item)
        {
            return _entityQualityReportDataCollection.Remove((EntityQualityReportData)item);
        }

        #endregion

        #region IEnumerable<EntityQualityReportData> Members

        /// <summary>
        /// Returns an enumerator that iterates through a EntityQualityReportDataCollection
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection</returns>
        public IEnumerator<EntityQualityReportData> GetEnumerator()
        {
            return _entityQualityReportDataCollection.GetEnumerator();
        }

        #endregion               
    
        #region IEnumerable Members

        /// <summary>
        /// Returns an enumerator that iterates through a EntityQualityReportCollection
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return _entityQualityReportDataCollection.GetEnumerator();
        }

        #endregion
    }
}
