using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;

namespace MDM.BusinessObjects
{
    using MDM.Interfaces;

    /// <summary>
    /// Represents class for collection of base profiles
    /// </summary>
    [DataContract]
    public class BaseProfileCollection : IBaseProfileCollection
    {
        #region Fields

        [DataMember]
        private Collection<BaseProfile> _baseProfiles = new Collection<BaseProfile>();

        #endregion

        #region Constructors

        /// <summary>
        /// Parameterless constructor
        /// </summary>
        public BaseProfileCollection()
        {
        }

        /// <summary>
        /// Initialize BaseProfileCollection from IList
        /// </summary>
        /// <param name="baseProfilesList">IList of BaseProfiles</param>
        public BaseProfileCollection(IList<BaseProfile> baseProfilesList)
        {
            _baseProfiles = new Collection<BaseProfile>(baseProfilesList);
        }

        #endregion

        #region Properties
        
        /// <summary>
        /// Indexer to getting Base Profiles by Id
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public BaseProfile this[Int32 index]
        {
            get { return _baseProfiles[index]; }
        }

        #endregion

        #region IEnumerable<NormalizationProfiles> Members

        /// <summary>
        /// Get an enumerator that iterates through a base profile collection
        /// </summary>
        /// <returns>Returns an enumerator that iterates through a base profile collection</returns>
        public IEnumerator<BaseProfile> GetEnumerator()
        {
            return this._baseProfiles.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        /// <summary>
        /// Get an enumerator that iterates through a base profile collection
        /// </summary>
        /// <returns>Returns an enumerator that iterates through a base profile collection</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this._baseProfiles.GetEnumerator();
        }

        #endregion

        #region ICollection<BaseProfile> Members

        /// <summary>
        /// Add base profile to the collection
        /// </summary>
        /// <param name="item">Indicates base profile to add in collection</param>
        public void Add(BaseProfile item)
        {
            this._baseProfiles.Add(item);
        }

        /// <summary>
        /// Clear the base profile collection
        /// </summary>
        public void Clear()
        {
            this._baseProfiles.Clear();
        }

        /// <summary>
        /// Determines whether the base profile collection contains a specific base profile
        /// </summary>
        /// <param name="item">Indicates the base profile to locate in base profile collection</param>
        /// <returns>
        /// <para>true : If base profile is found in base profile collection</para>
        /// <para>false : If base profile is not found in base profile collection</para>
        /// </returns>
        public bool Contains(BaseProfile item)
        {
            return this._baseProfiles.Contains(item);
        }

        /// <summary>
        /// Copies the elements of the base profile collection to an
        /// System.Array, starting at a particular System.Array index.
        /// </summary>
        /// <param name="array"> 
        ///  Indicates the one-dimensional System.Array that is the destination of the elements
        ///  copied from base profile collection. The System.Array must have zero-based indexing.
        /// </param>
        /// <param name="arrayIndex">Indicates the zero-based index in array at which copying begins</param>
        public void CopyTo(BaseProfile[] array, int arrayIndex)
        {
            this._baseProfiles.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Removes the first occurrence of a specific base profile from the base profile collection
        /// </summary>
        /// <param name="item">Indicates the base profile object to remove from the base profile collection</param>
        /// <returns>Returns true if specified base profile is successfully removed; otherwise false
        /// This method also returns false if base profile was not found in the original collection
        /// </returns>
        public bool Remove(BaseProfile item)
        {
            return this._baseProfiles.Remove(item);
        }

        /// <summary>
        /// Get the count of base profile in base profile collection
        /// </summary>
        public int Count
        {
            get { return this._baseProfiles.Count; }
        }

        /// <summary>
        /// Check if base profile collection is read only
        /// </summary>
        public bool IsReadOnly
        {
            get { return false; }
        }

        #endregion
    }
}
