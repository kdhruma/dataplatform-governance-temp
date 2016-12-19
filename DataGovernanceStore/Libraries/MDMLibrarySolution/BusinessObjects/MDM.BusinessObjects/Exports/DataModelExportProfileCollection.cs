using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;

namespace MDM.BusinessObjects.Exports
{
    using MDM.Interfaces.Exports;
    using System.Collections;

    /// <summary>
    /// Data Model Export Profile Collection class
    /// </summary>
    [DataContract]
    public class DataModelExportProfileCollection : IDataModelExportProfileCollection
    {
        #region Fields

        /// <summary>
        /// Field denoting collection of data model export profiles
        /// </summary>
        [DataMember]
        private Collection<DataModelExportProfile> _profiles = new Collection<DataModelExportProfile>();

        #endregion Fields

        #region IDataModelExportProfileCollection Members

        /// <summary>
        /// Add a list of data model export profile objects to the collection.
        /// </summary>
        /// <param name="items">Indicates list of data model export profile objects</param>
        public void AddRange(ICollection<DataModelExportProfile> items)
        {
            foreach (DataModelExportProfile item in items)
            {
                if (!Contains(item))
                {
                    this.Add(item);
                }
            }
        }

        #endregion IDataModelExportProfileCollection Members

        #region ICollection<DataModelExportProfile> Members

        /// <summary>
        /// Add data model export profile object in collection
        /// </summary>
        /// <param name="item">Indicates data model export profile to add in collection</param>
        public void Add(DataModelExportProfile item)
        {
            this._profiles.Add(item);
        }

        /// <summary>
        /// Removes all data model export profiles from collection
        /// </summary>
        public void Clear()
        {
            this._profiles.Clear();
        }

        /// <summary>
        /// Determines whether the DataModelExportProfileCollection contains a specific export profile.
        /// </summary>
        /// <param name="item">The export profile object to locate in the DataModelExportProfileCollection.</param>
        /// <returns>
        /// <para>true : If export profile found in dataModelExportProfileCollection</para>
        /// <para>false : If export profile found not in dataModelExportProfileCollection</para>
        /// </returns>
        public bool Contains(DataModelExportProfile item)
        {
            return this._profiles.Contains(item);
        }

        /// <summary>
        ///  Copies the elements of the DataModelExportProfileCollection to an
        ///  System.Array, starting at a particular System.Array index.
        /// </summary>
        /// <param name="array"> 
        ///  The one-dimensional System.Array that is the destination of the elements
        ///  copied from DataModelExportProfileCollection. The System.Array must have zero-based indexing.
        /// </param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
        public void CopyTo(DataModelExportProfile[] array, int arrayIndex)
        {
            this._profiles.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Get the count of no. of export profiles in DataModelExportProfileCollection
        /// </summary>
        public int Count
        {
            get
            {
                return this._profiles.Count;
            }
        }

        /// <summary>
        /// Check if DataModelExportProfileCollection is read-only.
        /// </summary>
        public bool IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the DataModelExportProfileCollection.
        /// </summary>
        /// <param name="item">The entity object to remove from the DataModelExportProfileCollection.</param>
        /// <returns>true if item is successfully removed; otherwise, false. This method also returns false if item was not found in the original DataModelExportProfileCollection</returns>
        public bool Remove(DataModelExportProfile item)
        {
            return this._profiles.Remove(item);
        }

        /// <summary>
        /// Clone container collection.
        /// </summary>
        /// <returns>cloned container collection object.</returns>
        public IDataModelExportProfileCollection Clone()
        {
            DataModelExportProfileCollection result = new DataModelExportProfileCollection();

            if (_profiles != null && _profiles.Count > 0)
            {
                foreach (DataModelExportProfile profile in _profiles)
                {
                    DataModelExportProfile clonedProfile = profile.Clone() as DataModelExportProfile;
                    result.Add(clonedProfile);
                }
            }

            return result;
        }

        #endregion ICollection<DataModelExportProfile> Members

        #region IEnumerable<DataModelExportProfile> Members

        /// <summary>
        /// Returns an enumerator that iterates through a DataModelExportProfileCollection.
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection.</returns>
        public IEnumerator<DataModelExportProfile> GetEnumerator()
        {
            return this._profiles.GetEnumerator();
        }

        #endregion IEnumerable<DataModelExportProfile> Members

        #region IEnumerable Members

        /// <summary>
        /// Returns an enumerator that iterates through a DataModelExportProfileCollection.
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator)this._profiles.GetEnumerator();
        }

        #endregion IEnumerable Members
    }
}
