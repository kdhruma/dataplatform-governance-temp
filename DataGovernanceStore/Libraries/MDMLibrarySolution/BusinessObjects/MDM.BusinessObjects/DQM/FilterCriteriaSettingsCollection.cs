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
    /// Specifies FilterCriteriaSettings collection
    /// </summary>
    [DataContract]
    public class FilterCriteriaSettingsCollection : IFilterCriteriaSettingsCollection, ICloneable
    {
        #region Fields

        [DataMember]
        private Collection<FilterCriteriaSettings> _filterCriteriaSettings = new Collection<FilterCriteriaSettings>();

        #endregion

        #region Constructors

        /// <summary>
        /// Parameterless constructor
        /// </summary>
        public FilterCriteriaSettingsCollection()
        {
            this._filterCriteriaSettings = new Collection<FilterCriteriaSettings>();
        }

        /// <summary>
        /// Initialize FilterCriteriaSettingsCollection from IList
        /// </summary>
        /// <param name="filterCriteriaSettingsList">IList of FilterCriteriaSettings</param>
        public FilterCriteriaSettingsCollection(IList<FilterCriteriaSettings> filterCriteriaSettingsList)
        {
            this._filterCriteriaSettings = new Collection<FilterCriteriaSettings>(filterCriteriaSettingsList);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Indexer to getting FilterCriteriaSettings by Id
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public FilterCriteriaSettings this[Int32 index]
        {
            get { return _filterCriteriaSettings[index]; }
        }

        #endregion

        #region Methods

        #region Public Method

        /// <summary>
        /// Clone filter criteria settings collection
        /// </summary>
        /// <returns>Cloned filter criteria settings collection object</returns>
        public object Clone()
        {
            FilterCriteriaSettingsCollection clonedProfiles = new FilterCriteriaSettingsCollection();

            if (this._filterCriteriaSettings != null && this._filterCriteriaSettings.Count > 0)
            {
                foreach (FilterCriteriaSettings filterCriteriaSettings in this._filterCriteriaSettings)
                {
                    FilterCriteriaSettings clonedProfile = filterCriteriaSettings.Clone() as FilterCriteriaSettings;
                    clonedProfiles.Add(clonedProfile);
                }
            }

            return clonedProfiles;
        }

        /// <summary>
        /// Remove FilterCriteriaSettings object from FilterCriteriaSettingsCollection
        /// </summary>
        /// <param name="filterCriteriaSettingId">Id of FilterCriteriaSettings which information is to be removed from collection</param>
        /// <returns>true if item is successfully removed; otherwise, false. This method also returns false if item was not found in the original collection</returns>
        public Boolean Remove(Int32 filterCriteriaSettingId)
        {
            FilterCriteriaSettings filterCriteriaSetting = Get(filterCriteriaSettingId);
            if (filterCriteriaSetting == null)
            {
                return false;
            }
            return this.Remove(filterCriteriaSetting);
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
                FilterCriteriaSettingsCollection objectToBeCompared = obj as FilterCriteriaSettingsCollection;
                if (objectToBeCompared != null)
                {
                    if (ReferenceEquals(this, objectToBeCompared))
                    {
                        return true;
                    }

                    List<FilterCriteriaSettings> filterCriteriaSettings = this._filterCriteriaSettings.ToList();
                    List<FilterCriteriaSettings> profiles = objectToBeCompared.ToList();

                    Int32 validationProfileUnion = filterCriteriaSettings.Union(profiles).Count();
                    Int32 filterCriteriaSettingsIntersect = filterCriteriaSettings.Intersect(profiles).Count();

                    return (validationProfileUnion == filterCriteriaSettingsIntersect);
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
            return this._filterCriteriaSettings.Sum(item => item.GetHashCode());
        }

        #endregion

        #region Private Method

        /// <summary>
        /// Get FilterCriteriaSettings from current filter criteria settings based on filter criteria setting Id
        /// </summary>
        /// <param name="filterCriteriaSettingsId">Id of filter criteria setting which information is to be searched</param>
        /// <returns>FilterCriteriaSettings having given filterCriteriaSettingsId messageCode</returns>
        private FilterCriteriaSettings Get(Int32 filterCriteriaSettingsId)
        {
            return this._filterCriteriaSettings.FirstOrDefault(filterCriteriaSettings => filterCriteriaSettings.Id == filterCriteriaSettingsId);
        }

        #endregion

        #endregion

        #region ICollection<FilterCriteriaSettings> Members

        /// <summary>
        /// Add FilterCriteriaSettings object in collection
        /// </summary>
        /// <param name="item">FilterCriteriaSettings to add in collection</param>
        public void Add(FilterCriteriaSettings item)
        {
            this._filterCriteriaSettings.Add(item);
        }

        /// <summary>
        /// Add IFilterCriteriaSettings object in collection
        /// </summary>
        /// <param name="item">IFilterCriteriaSettings to add in collection</param>
        public void Add(IFilterCriteriaSettings item)
        {
            this._filterCriteriaSettings.Add((FilterCriteriaSettings)item);
        }

        /// <summary>
        /// Removes all FilterCriteriaSettings from collection
        /// </summary>
        public void Clear()
        {
            this._filterCriteriaSettings.Clear();
        }

        /// <summary>
        /// Determines whether the FilterCriteriaSettingsCollection contains a specific FilterCriteriaSettings
        /// </summary>
        /// <param name="item">The FilterCriteriaSettings object to locate in the FilterCriteriaSettingsCollection</param>
        /// <returns>
        /// <param>true : If FilterCriteriaSettings found in FilterCriteriaSettingsCollection</param>
        /// <param>false : If FilterCriteriaSettings found not in FilterCriteriaSettingsCollection</param>
        /// </returns>
        public Boolean Contains(FilterCriteriaSettings item)
        {
            return this._filterCriteriaSettings.Contains(item);
        }

        /// <summary>
        /// Copies the elements of the FilterCriteriaSettingsCollection to an
        /// System.Array, starting at a particular System.Array index
        /// </summary>
        /// <param name="array"> 
        ///  The one-dimensional System.Array that is the destination of the elements copied from FilterCriteriaSettingsCollection. The System.Array must have zero-based indexing
        /// </param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins</param>
        public void CopyTo(FilterCriteriaSettings[] array, Int32 arrayIndex)
        {
            this._filterCriteriaSettings.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Get the count of no. of FilterCriteriaSettings in FilterCriteriaSettingsCollection
        /// </summary>
        public Int32 Count
        {
            get
            {
                return this._filterCriteriaSettings.Count;
            }
        }

        /// <summary>
        /// Check if FilterCriteriaSettingsCollection is read-only
        /// </summary>
        public Boolean IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the FilterCriteriaSettingsCollection
        /// </summary>
        /// <param name="item">The FilterCriteriaSettings object to remove from the FilterCriteriaSettingsCollection</param>
        /// <returns>true if item is successfully removed; otherwise, false. This method also returns false if item was not found in the original FilterCriteriaSettingsCollection</returns>
        public Boolean Remove(FilterCriteriaSettings item)
        {
            return this._filterCriteriaSettings.Remove(item);
        }

        #endregion

        #region IEnumerable<FilterCriteriaSettings> Members

        /// <summary>
        /// Returns an enumerator that iterates through a FilterCriteriaSettingsCollection
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection</returns>
        public IEnumerator<FilterCriteriaSettings> GetEnumerator()
        {
            return this._filterCriteriaSettings.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        /// <summary>
        /// Returns an enumerator that iterates through a FilterCriteriaSettingsCollection
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this._filterCriteriaSettings.GetEnumerator();
        }

        #endregion
    }
}
