using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;

namespace MDM.BusinessObjects
{
    /// <summary>
    /// Interface for Collection of CategoryBaseProperties
    /// </summary>
    [DataContract]
    public class CategoryBasePropertiesCollection : ICollection<CategoryBaseProperties>, IEnumerable<CategoryBaseProperties>
    {
        #region Fields

        /// <summary>
        /// Collection of CategoryBaseProperties Instance
        /// </summary>
        [DataMember]
        private Collection<CategoryBaseProperties> _categoryBaseProperties = new Collection<CategoryBaseProperties>();

        #endregion

        #region Constructors
        
        /// <summary>
        /// Parameter-less constructor
        /// </summary>
        public CategoryBasePropertiesCollection() : base() { }

        #endregion

        #region Properties
        #endregion

        #region Methods

        #region Public Methods
        #endregion Public Methods

        #region Private Methods
        #endregion Private Methods

        #region ICollection<CategoryBaseProperties> Members

        /// <summary>
        /// Add CategoryBaseProperties object in collection
        /// </summary>
        /// <param name="item">CategoryBaseProperties to add in collection</param>
        public void Add(CategoryBaseProperties item)
        {
            this._categoryBaseProperties.Add(item);
        }

        /// <summary>
        /// Removes all CategoryBaseProperties from collection
        /// </summary>
        public void Clear()
        {
            this._categoryBaseProperties.Clear();
        }

        /// <summary>
        /// Determines whether the CategoryBasePropertiesCollection contains a specific column.
        /// </summary>
        /// <param name="item">The CategoryBaseProperties object to locate in the CategoryBasePropertiesCollection.</param>
        /// <returns>
        /// <para>true : If CategoryBaseProperties found in CategoryBasePropertiesCollection</para>
        /// <para>false : If CategoryBaseProperties found not in CategoryBasePropertiesCollection</para>
        /// </returns>
        public bool Contains(CategoryBaseProperties item)
        {
            return this._categoryBaseProperties.Contains(item);
        }

        /// <summary>
        /// Copies the elements of the CategoryBasePropertiesCollection to an
        ///  System.Array, starting at a particular System.Array index.
        /// </summary>
        /// <param name="array"> 
        ///  The one-dimensional System.Array that is the destination of the elements
        ///  copied from CategoryBasePropertiesCollection. The System.Array must have zero-based indexing.
        /// </param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
        public void CopyTo(CategoryBaseProperties[] array, int arrayIndex)
        {
            this._categoryBaseProperties.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Get the count of no. of CategoryBaseProperties in CategoryBasePropertiesCollection
        /// </summary>
        public int Count
        {
            get
            {
                return this._categoryBaseProperties.Count;
            }
        }

        /// <summary>
        /// Check if CategoryBasePropertiesCollection is read-only.
        /// </summary>
        public bool IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// Removes the first occurrence of a specific CategoryBaseProperties from the CategoryBasePropertiesCollection.
        /// </summary>
        /// <param name="item">The CategoryBaseProperties object to remove from the CategoryBasePropertiesCollection.</param>
        /// <returns>true if item is successfully removed; otherwise, false. This method also returns false if item was not found in the original CategoryBasePropertiesCollection</returns>
        public bool Remove(CategoryBaseProperties item)
        {
            return this._categoryBaseProperties.Remove(item);
        }

        #endregion

        #region IEnumerable<CategoryBaseProperties> Members

        /// <summary>
        /// Returns an enumerator that iterates through a CategoryBasePropertiesCollection.
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection.</returns>
        public IEnumerator<CategoryBaseProperties> GetEnumerator()
        {
            return this._categoryBaseProperties.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through a CategoryBasePropertiesCollection.
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator)this._categoryBaseProperties.GetEnumerator();
        }

        #endregion

        #endregion
    }
}
