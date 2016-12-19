using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;

namespace MDM.BusinessObjects.Exports
{
    using MDM.Core;
    using MDM.Interfaces.Exports;

    /// <summary>
    /// Entity Export Profile Collection class
    /// </summary>
    [DataContract]
    public class EntityExportProfileCollection : IEntityExportProfileCollection
    {
        #region Fields

        [DataMember]
        private Collection<EntityExportProfile> _profiles = new Collection<EntityExportProfile>();

        #endregion

        #region IEntityExportProfileCollection Members

        /// <summary>
        /// Add a list of export profile objects to the collection.
        /// </summary>
        /// <param name="items">Indicates list of export profile objects</param>
        public void AddRange(ICollection<EntityExportProfile> items)
        {
            foreach (EntityExportProfile item in items)
            {
                if (!Contains(item))
                    this.Add(item);
            }
        }

        /// <summary>
        /// Gets the filtered export profiles based on the specified condition.
        /// </summary>
        /// <param name="condition">The condition.</param>
        /// <returns>Filtered entity export profile</returns>
        public EntityExportProfile Get(Func<EntityExportProfile, Boolean> condition)
        {
            if (_profiles != null && _profiles.Count > 0)
            {
                foreach (var profile in _profiles)
                {
                    if (condition(profile))
                    {
                        return profile;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Gets the filtered export profiles based on the profile name.
        /// </summary>
        /// <param name="profileName">Indicates the profile name</param>
        /// <returns>Filtered entity export profile</returns>
        public EntityExportProfile GetByName(String profileName)
        {
            EntityExportProfile profileToReturn = null;

            if (_profiles != null && _profiles.Count > 0)
            {
                foreach (var profile in _profiles)
                {
                    if (String.Compare(profile.Name, profileName) == 0)
                    {
                        profileToReturn = profile;
                    }
                }
            }

            return profileToReturn;
        }

        /// <summary>
        /// Gets the filtered export profiles based on the export profile type.
        /// </summary>
        /// <param name="profileType">Indicates the export profile name</param>
        /// <returns>Filtered entity export profiles</returns>
        public EntityExportProfileCollection GetByType(ExportProfileType profileType)
        {
            EntityExportProfileCollection profilesToReturn = new EntityExportProfileCollection();

            if (_profiles != null && _profiles.Count > 0)
            {
                foreach (var profile in _profiles)
                {
                    if (profile.ProfileType == profileType)
                    {
                        profilesToReturn.Add(profile);
                    }
                }
            }
            return profilesToReturn;
        }

        #endregion

        #region ICollection<EntityExportProfile> Members

        /// <summary>
        /// Add export profile object in collection
        /// </summary>
        /// <param name="item">export profile to add in collection</param>
        public void Add(EntityExportProfile item)
        {
            this._profiles.Add(item);
        }

        /// <summary>
        /// Removes all export profiles from collection
        /// </summary>
        public void Clear()
        {
            this._profiles.Clear();
        }

        /// <summary>
        /// Determines whether the EntityExportProfileCollection contains a specific export profile.
        /// </summary>
        /// <param name="item">The export profile object to locate in the EntityExportProfileCollection.</param>
        /// <returns>
        /// <para>true : If export profile found in exportProfileCollection</para>
        /// <para>false : If export profile found not in exportProfileCollection</para>
        /// </returns>
        public bool Contains(EntityExportProfile item)
        {
            return this._profiles.Contains(item);
        }

        /// <summary>
        ///  Copies the elements of the EntityExportProfileCollection to an
        ///  System.Array, starting at a particular System.Array index.
        /// </summary>
        /// <param name="array"> 
        ///  The one-dimensional System.Array that is the destination of the elements
        ///  copied from EntityExportProfileCollection. The System.Array must have zero-based indexing.
        /// </param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
        public void CopyTo(EntityExportProfile[] array, int arrayIndex)
        {
            this._profiles.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Get the count of no. of export profiles in EntityExportProfileCollection
        /// </summary>
        public int Count
        {
            get
            {
                return this._profiles.Count;
            }
        }

        /// <summary>
        /// Check if EntityExportProfileCollection is read-only.
        /// </summary>
        public bool IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the EntityExportProfileCollection.
        /// </summary>
        /// <param name="item">The entity object to remove from the EntityExportProfileCollection.</param>
        /// <returns>true if item is successfully removed; otherwise, false. This method also returns false if item was not found in the original EntityExportProfileCollection</returns>
        public bool Remove(EntityExportProfile item)
        {
            return this._profiles.Remove(item);
        }

        /// <summary>
        /// Clone container collection.
        /// </summary>
        /// <returns>cloned container collection object.</returns>
        public IEntityExportProfileCollection Clone()
        {
            EntityExportProfileCollection result = new EntityExportProfileCollection();

            if (_profiles != null && _profiles.Count > 0)
            {
                foreach (EntityExportProfile profile in _profiles)
                {
                    EntityExportProfile clonedIContainer = profile.Clone() as EntityExportProfile;
                    result.Add(clonedIContainer);
                }
            }

            return result;
        }

        #endregion

        #region IEnumerable<ExportProfile> Members

        /// <summary>
        /// Returns an enumerator that iterates through a EntityExportProfileCollection.
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection.</returns>
        public IEnumerator<EntityExportProfile> GetEnumerator()
        {
            return this._profiles.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        /// <summary>
        /// Returns an enumerator that iterates through a EntityExportProfileCollection.
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator)this._profiles.GetEnumerator();
        }

        #endregion
    }
}
