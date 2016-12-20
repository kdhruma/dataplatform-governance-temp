using System.Collections.Generic;
using System.Runtime.Serialization;

namespace MDM.BusinessObjects
{
    using MDM.Interfaces;

    /// <summary>
    /// Represents a collection of the File object.
    /// </summary>
    [DataContract]
    public class FileCollection : InterfaceContractCollection<IFile, File>, IFileCollection
    {
        #region Constructors

        /// <summary>
        /// Parameterless constructor
        /// </summary>
        public FileCollection() : base() { }

        #endregion

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public File this[int index]
        {
            get
            {
                return this._items[index];
            }
        }

        #endregion

        #region IEnumerable Members

        /// <summary>
        /// Returns an enumerator that iterates through a FileCollection.
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection.</returns>
        public new IEnumerator<IFile> GetEnumerator()
        {
            return this._items.GetEnumerator();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Add a list of file objects to the collection.
        /// </summary>
        /// <param name="items">Indicates list of file objects</param>
        public void Add(List<File> items)
        {
            foreach (File item in items)
            {
                this.Add(item);
            }
        }

        #endregion
    }
}
