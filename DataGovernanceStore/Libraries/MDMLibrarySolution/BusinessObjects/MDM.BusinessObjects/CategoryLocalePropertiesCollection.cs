using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using System.Linq;

namespace MDM.BusinessObjects
{
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    public class CategoryLocalePropertiesCollection : ICollection<CategoryLocaleProperties>, IEnumerable<CategoryLocaleProperties>, IDataModelObjectCollection
    {
        #region Fields

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        private Collection<CategoryLocaleProperties> _categoryLocaleProperties = new Collection<CategoryLocaleProperties>();

        #endregion

        #region Constructors
        
        /// <summary>
        /// Parameter-less constructor
        /// </summary>
        public CategoryLocalePropertiesCollection() : base() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="categoryLocalePropertyList"></param>
        public CategoryLocalePropertiesCollection(IList<CategoryLocaleProperties> categoryLocalePropertyList)
        {
            if (categoryLocalePropertyList != null)
            {
                this._categoryLocaleProperties = new Collection<CategoryLocaleProperties>(categoryLocalePropertyList);
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Property denoting a ObjectType for an dataModelObject object
        /// </summary>
        public ObjectType DataModelObjectType
        {
            get
            {
                return MDM.Core.ObjectType.CategoryLocalization;
            }
        }

        #endregion

        #region Methods

        #region Public Methods

        /// <summary>
        /// Remove dataModelObject based on reference id.
        /// </summary>
        /// <param name="referenceIds">Indicates collection of referenceId of an entity type which is to be fetched.</param>
        /// <returns>true if item was successfully removed from the collection;otherwise, false. This method also returns false if item is not found in the original</returns>
        public Boolean Remove(Collection<String> referenceIds)
        {
            Boolean result = true;

            CategoryLocalePropertiesCollection categoryLocalePropertiesCollection = GetCategoryLocalePropertiesCollection(referenceIds);

            if (categoryLocalePropertiesCollection != null && categoryLocalePropertiesCollection.Count > 0)
            {
                foreach (CategoryLocaleProperties categoryLocaleProperties in categoryLocalePropertiesCollection)
                {
                    result = result && this.Remove(categoryLocaleProperties);
                }
            }

            return result;
        }

        #region IDataModelObjectCollection Members

        /// <summary>
        /// Splits current collection based on batchSize
        /// </summary>
        /// <param name="batchSize">Indicates no of objects to be put in each batch</param>
        /// <returns>IDataModelObjectCollection in Batch</returns>
        public Collection<IDataModelObjectCollection> Split(Int32 batchSize)
        {
            Collection<IDataModelObjectCollection> categoryLocalePropertiesInBatch = null;

            if (this._categoryLocaleProperties != null)
            {
                categoryLocalePropertiesInBatch = Utility.Split(this, batchSize);
            }

            return categoryLocalePropertiesInBatch;
        }

        /// <summary>
        /// Adds item in current collection
        /// </summary>
        /// <param name="item">Indicates item to be added in collection</param>
        public void AddDataModelObject(IDataModelObject item)
        {
            this.Add(item as CategoryLocaleProperties);
        }

        #endregion

        #endregion

        #region Private Methods

        /// <summary>
        ///  Gets the category locale properties using the reference id.
        /// </summary>
        /// <param name="referenceIds">Indicates collection of referenceId of category locale properties which is to be fetched.</param>
        /// <returns>Returns filtered category locale properties collection</returns>
        private CategoryLocalePropertiesCollection GetCategoryLocalePropertiesCollection(Collection<String> referenceIds)
        {
            CategoryLocalePropertiesCollection categoryLocalePropertiesCollection = new CategoryLocalePropertiesCollection();
            Int32 counter = 0;

            if (this._categoryLocaleProperties != null && this._categoryLocaleProperties.Count > 0 && referenceIds != null && referenceIds.Count > 0)
            {
                foreach (CategoryLocaleProperties categoryLocaleProperties in this._categoryLocaleProperties)
                {
                    if (referenceIds.Contains(categoryLocaleProperties.ReferenceId))
                    {
                        categoryLocalePropertiesCollection.Add(categoryLocaleProperties);
                        counter++;
                    }

                    if (referenceIds.Count.Equals(counter))
                        break;
                }
            }

            return categoryLocalePropertiesCollection;
        }

        #endregion

        #region ICollection<CategoryLocaleProperties> Members

        /// <summary>
        /// Add CategoryLocaleProperties object in collection
        /// </summary>
        /// <param name="item">CategoryLocaleProperties to add in collection</param>
        public void Add(CategoryLocaleProperties item)
        {
            this._categoryLocaleProperties.Add(item);
        }

        /// <summary>
        /// Removes all CategoryLocaleProperties from collection
        /// </summary>
        public void Clear()
        {
            this._categoryLocaleProperties.Clear();
        }

        /// <summary>
        /// Determines whether the CategoryLocalePropertiesCollection contains a specific column.
        /// </summary>
        /// <param name="item">The CategoryLocaleProperties object to locate in the CategoryLocalePropertiesCollection.</param>
        /// <returns>
        /// <para>true : If CategoryLocaleProperties found in CategoryLocalePropertiesCollection</para>
        /// <para>false : If CategoryLocaleProperties found not in CategoryLocalePropertiesCollection</para>
        /// </returns>
        public bool Contains(CategoryLocaleProperties item)
        {
            return this._categoryLocaleProperties.Contains(item);
        }

        /// <summary>
        /// Copies the elements of the CategoryLocalePropertiesCollection to an
        ///  System.Array, starting at a particular System.Array index.
        /// </summary>
        /// <param name="array"> 
        ///  The one-dimensional System.Array that is the destination of the elements
        ///  copied from CategoryLocalePropertiesCollection. The System.Array must have zero-based indexing.
        /// </param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
        public void CopyTo(CategoryLocaleProperties[] array, int arrayIndex)
        {
            this._categoryLocaleProperties.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Get the count of no. of CategoryLocaleProperties in CategoryLocalePropertiesCollection
        /// </summary>
        public int Count
        {
            get
            {
                return this._categoryLocaleProperties.Count;
            }
        }

        /// <summary>
        /// Check if CategoryLocalePropertiesCollection is read-only.
        /// </summary>
        public bool IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// Removes the first occurrence of a specific CategoryLocaleProperties from the CategoryLocalePropertiesCollection.
        /// </summary>
        /// <param name="item">The CategoryLocaleProperties object to remove from the CategoryLocalePropertiesCollection.</param>
        /// <returns>true if item is successfully removed; otherwise, false. This method also returns false if item was not found in the original CategoryLocalePropertiesCollection</returns>
        public bool Remove(CategoryLocaleProperties item)
        {
            return this._categoryLocaleProperties.Remove(item);
        }

        #endregion

        #region IEnumerable<CategoryLocaleProperties> Members

        /// <summary>
        /// Returns an enumerator that iterates through a CategoryLocalePropertiesCollection.
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection.</returns>
        public IEnumerator<CategoryLocaleProperties> GetEnumerator()
        {
            return this._categoryLocaleProperties.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through a CategoryLocalePropertiesCollection.
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator)this._categoryLocaleProperties.GetEnumerator();
        }

        #endregion

        #endregion
    }
}