using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Collections.ObjectModel;
using System.Collections;

namespace MDM.BusinessObjects.DQM
{
    using MDM.Interfaces;

    /// <summary>
    /// Specifies DataQualityClassStatistics Collection
    /// </summary>
    [DataContract]
    public class DataQualityClassStatisticsCollection : IDataQualityClassStatisticsCollection
    {
        #region Fields

        [DataMember]
        private Collection<DataQualityClassStatistics> _dqcStatistics = new Collection<DataQualityClassStatistics>();

        #endregion

        #region Constructors

        /// <summary>
        /// Parameterless constructor
        /// </summary>
        public DataQualityClassStatisticsCollection()
        {
            this._dqcStatistics = new Collection<DataQualityClassStatistics>();
        }

        /// <summary>
        /// Initialize DataQualityClassStatisticsCollection from IList
        /// </summary>
        /// <param name="dataQualityClassStatisticsList">IList of DataQualityClassStatisticss</param>
        public DataQualityClassStatisticsCollection(IList<DataQualityClassStatistics> dataQualityClassStatisticsList)
        {
            this._dqcStatistics = new Collection<DataQualityClassStatistics>(dataQualityClassStatisticsList);
        }

        #endregion

        #region Properties
        
        /// <summary>
        /// Indexer to getting data quality classes by Id
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public DataQualityClassStatistics this[Int32 index]   
        {            
            get { return _dqcStatistics [index]; }                        
        }

        #endregion

        #region Methods

        #region Public Method

        /// <summary>
        /// Remove DataQualityClassStatistics object from DataQualityClassStatisticsCollection
        /// </summary>
        /// <param name="dataQualityClassId">Id of DataQualityClass which information is to be removed from collection</param>
        /// <returns>true if item is successfully removed; otherwise, false. This method also returns false if item was not found in the original collection</returns>
        public Boolean Remove(Int16 dataQualityClassId)
        {
            DataQualityClassStatistics dataQualityClassStatistics = Get(dataQualityClassId);

            if (dataQualityClassStatistics == null)
                return false;
            else
                return this.Remove(dataQualityClassStatistics);
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
                DataQualityClassStatisticsCollection objectToBeCompared = obj as DataQualityClassStatisticsCollection;
                if (objectToBeCompared != null)
                {
                    if (ReferenceEquals(this, objectToBeCompared))
                    {
                        return true;
                    }

                    Int32 dataQualityClassStatisticsUnion = this._dqcStatistics.ToList().Union(objectToBeCompared.ToList()).Count();
                    Int32 dataQualityClassStatisticssIntersect = this._dqcStatistics.ToList().Intersect(objectToBeCompared.ToList()).Count();

                    return (dataQualityClassStatisticsUnion == dataQualityClassStatisticssIntersect);
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
            Int32 hashCode = 0;
            if (this._dqcStatistics != null)
            {
                foreach (DataQualityClassStatistics item in this._dqcStatistics)
                {
                    hashCode += item.GetHashCode();
                }
            }
            return hashCode;
        }

        #endregion

        #region Private Method

        /// <summary>
        /// Get DataQualityClassStatistics from current DataQualityClassStatistics collection based on DataQualityClass Id
        /// </summary>
        /// <param name="dataQualityClassId">Id of DataQualityClass which information is to be searched</param>
        /// <returns>DataQualityClassStatistics having given DataQualityClassStatisticsId messageCode</returns>
        private DataQualityClassStatistics Get(Int16 dataQualityClassId)
        {
            IEnumerable<DataQualityClassStatistics> filteredDataQualityClassStatistics = null;

            filteredDataQualityClassStatistics = this._dqcStatistics.Where(a => a.DataQualityClassId == dataQualityClassId);

            if (filteredDataQualityClassStatistics != null && filteredDataQualityClassStatistics.Any())
                return filteredDataQualityClassStatistics.First();
            else
                return null;
        }

        #endregion

        #endregion

        #region ICollection<DataQualityClassStatistics> Members

        /// <summary>
        /// Add DataQualityClassStatistics object in collection
        /// </summary>
        /// <param name="item">DataQualityClassStatistics to add in collection</param>
        public void Add(DataQualityClassStatistics item)
        {
            this._dqcStatistics.Add(item);
        }

        /// <summary>
        /// Add IDataQualityClassStatistics object in collection
        /// </summary>
        /// <param name="item">IDataQualityClassStatistics to add in collection</param>
        public void Add(IDataQualityClassStatistics item)
        {
            this._dqcStatistics.Add((DataQualityClassStatistics)item);
        }

        /// <summary>
        /// Removes all DataQualityClassStatisticss from collection
        /// </summary>
        public void Clear()
        {
            this._dqcStatistics.Clear();
        }

        /// <summary>
        /// Determines whether the DataQualityClassStatisticsCollection contains a specific DataQualityClassStatistics
        /// </summary>
        /// <param name="item">The DataQualityClassStatistics object to locate in the DataQualityClassStatisticsCollection</param>
        /// <returns>
        /// <para>true : If DataQualityClassStatistics found in DataQualityClassStatisticsCollection</para>
        /// <para>false : If DataQualityClassStatistics found not in DataQualityClassStatisticsCollection</para>
        /// </returns>
        public Boolean Contains(DataQualityClassStatistics item)
        {
            return this._dqcStatistics.Contains(item);
        }

        /// <summary>
        /// Copies the elements of the DataQualityClassStatisticsCollection to an
        /// System.Array, starting at a particular System.Array index
        /// </summary>
        /// <param name="array"> 
        ///  The one-dimensional System.Array that is the destination of the elements copied from DataQualityClassStatisticsCollection. The System.Array must have zero-based indexing
        /// </param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins</param>
        public void CopyTo(DataQualityClassStatistics[] array, Int32 arrayIndex)
        {
            this._dqcStatistics.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Get the count of no. of DataQualityClassStatisticss in DataQualityClassStatisticsCollection
        /// </summary>
        public Int32 Count
        {
            get
            {
                return this._dqcStatistics.Count;
            }
        }

        /// <summary>
        /// Check if DataQualityClassStatisticsCollection is read-only
        /// </summary>
        public Boolean IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the DataQualityClassStatisticsCollection
        /// </summary>
        /// <param name="item">The DataQualityClassStatistics object to remove from the DataQualityClassStatisticsCollection</param>
        /// <returns>true if item is successfully removed; otherwise, false. This method also returns false if item was not found in the original DataQualityClassStatisticsCollection</returns>
        public Boolean Remove(DataQualityClassStatistics item)
        {
            return this._dqcStatistics.Remove(item);
        }

        #endregion

        #region IEnumerable<DataQualityClassStatistics> Members

        /// <summary>
        /// Returns an enumerator that iterates through a DataQualityClassStatisticsCollection
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection</returns>
        public IEnumerator<DataQualityClassStatistics> GetEnumerator()
        {
            return this._dqcStatistics.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        /// <summary>
        /// Returns an enumerator that iterates through a DataQualityClassStatisticsCollection
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator)this._dqcStatistics.GetEnumerator();
        }

        #endregion
    }
}
