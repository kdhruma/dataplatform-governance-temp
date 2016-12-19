using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace MDM.BusinessObjects
{
    using MDM.Interfaces;

    /// <summary>
    /// Specifies the class that contains collection of DDGImport Profile
    /// </summary>
    public class DDGImportProfileCollection : IDDGImportProfileCollection
    {
        #region Fields

        /// <summary>
        /// Field for DDGImport Profile collection
        /// </summary>
        private Collection<DDGImportProfile> _ddgImportProfileCollection = new Collection<DDGImportProfile>();

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets the number of elements contained in DDGImportProfileCollection
        /// </summary>
        public Int32 Count
        {
            get { return this._ddgImportProfileCollection.Count; }
        }

        /// <summary>
        /// Check if DDGImportProfileCollection is read-only
        /// </summary>
        public Boolean IsReadOnly
        {
            get { return false; }
        }

        #endregion Properties

        #region Constructor

        /// <summary>
        /// Default Constructor
        /// </summary>
        public DDGImportProfileCollection()
        {

        }

        #endregion Constructor

        #region Methods

        #region ICollection Methods

        /// <summary>
        /// Adds the specified ddgImportProfile
        /// </summary>
        /// <param name="ddgImportProfile">ddgImportProfile object</param>
        public void Add(DDGImportProfile ddgImportProfile)
        {
            if (ddgImportProfile != null)
            {
                this._ddgImportProfileCollection.Add(ddgImportProfile);
            }
        }

        /// <summary>
        /// Removes all items from the current DDGImportProfileCollection
        /// </summary>
        public void Clear()
        {
            this._ddgImportProfileCollection.Clear();
        }

        /// <summary>
        /// Determines whether the current DDGImportProfileCollection contains the specified importProfile
        /// </summary>
        /// <param name="ddgImportProfile">ImportProfie to be verified</param>
        /// <returns>True if found in collection else false</returns>
        public bool Contains(DDGImportProfile ddgImportProfile)
        {
            return this._ddgImportProfileCollection.Contains(ddgImportProfile);
        }

        /// <summary>
        /// Copies the elements of the DDGImportProfileCollection to an System.Array, starting at a particular System.Array index
        /// </summary>
        /// <param name="array">
        /// The one-dimensional System.Array that is the destination of the elements copied from DDGImportProfileCollection
        /// The System.Array must have zero-based indexing
        /// </param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins</param>
        public void CopyTo(DDGImportProfile[] array, int arrayIndex)
        {
            this._ddgImportProfileCollection.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the DDGImportProfileCollection
        /// </summary>
        /// <param name="ddgImportProfile">ImportProfie to be removed</param>
        /// <returns>True if mdmrule is successfully removed; otherwise, false.</returns>
        public bool Remove(DDGImportProfile ddgImportProfile)
        {
            return this._ddgImportProfileCollection.Remove(ddgImportProfile);
        }

        #endregion ICollection Methods

        #region IEnumerable Members

        /// <summary>
        /// Returns an enumerator that iterates through the collection
        /// </summary>
        /// <returns>A enumerator that can be used to iterate through the collection</returns>
        public IEnumerator<DDGImportProfile> GetEnumerator()
        {
            return this._ddgImportProfileCollection.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection
        /// </summary>
        /// <returns>An enumerator that can be used to iterate through the collection</returns>
        IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return (IEnumerator)this._ddgImportProfileCollection.GetEnumerator();
        }

        #endregion IEnumerable Members

        #region DataModelImportProfile Get

        /// <summary>
        /// Get the DDG Import profile based on requested profile id
        /// </summary>
        /// <param name="ddgImportProfileId">Indicates the profile Id</param>
        /// <returns>DDG Import profile</returns>
        public DDGImportProfile GetById(Int32 ddgImportProfileId)
        {
            DDGImportProfile ddgImportProfileToReturn = null;

            if (ddgImportProfileId <= 0)
            {
                throw new ArgumentException("DDG Import Profile Id must be greater than 0", ddgImportProfileId.ToString());
            }

            foreach (DDGImportProfile ddgImportProfile in this._ddgImportProfileCollection)
            {
                if (ddgImportProfile.Id == ddgImportProfileId)
                {
                    ddgImportProfileToReturn = ddgImportProfile;
                    break;
                }
            }

            return ddgImportProfileToReturn;
        }

        /// <summary>
        /// Get the DDG Import profile based on the file watcher folder name
        /// </summary>
        /// <param name="fileWatcherFolderName">Indicates the file watcher folder name</param>
        /// <returns>DDG Import profile</returns>
        public DDGImportProfile GetByFileWatcherFolderName(String fileWatcherFolderName)
        {
            DDGImportProfile ddgImportProfileToReturn = null;

            if (String.IsNullOrWhiteSpace(fileWatcherFolderName))
            {
                throw new ArgumentException("FileWatcher FolderName should not be empty", fileWatcherFolderName);
            }

            foreach (DDGImportProfile ddgImportProfile in this._ddgImportProfileCollection)
            {
                if (ddgImportProfile.FileWatcherFolderName.ToLower() == fileWatcherFolderName.ToLower())
                {
                    ddgImportProfileToReturn = ddgImportProfile;
                    break;
                }
            }

            return ddgImportProfileToReturn;
        }

        #endregion DataModelImportProfile Get

        #endregion Methods
    }
}
