using System;
using System.Xml;
using System.Text;
using System.Runtime.Serialization;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace MDM.BusinessObjects
{
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Specifies CategoryTreeItem Collection
    /// </summary>
    [DataContract]
    public class CategoryTreeItemCollection : ICollection<CategoryTreeItem>, ICategoryTreeItemCollection
    {
        #region Fields

        /// <summary>
        /// Field denoting collection of CategoryTreeItems
        /// </summary>
        [DataMember]
        private Collection<CategoryTreeItem> _categories = new Collection<CategoryTreeItem>();

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the CategoryTreeItem collection class
        /// </summary>
        public CategoryTreeItemCollection() : base() { }

        /// <summary>
        /// Xml constructor
        /// </summary>
        /// <param name="valuesAsXml">Xml having value for categories</param>
        public CategoryTreeItemCollection(String valuesAsXml)
        {
            LoadCategoryTreeItemCollection(valuesAsXml);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Find CategoryTreeItem from CategoryTreeItemCollection based on categoryId
        /// </summary>
        /// <param name="categoryId">Category Id</param>
        /// <returns>Category object having given categoryID</returns>
        public CategoryTreeItem this[Int64 categoryId]
        {
            get
            {
                var category = GetCategoryTreeItem(categoryId);

                if (category == null)
                {
                    throw new ArgumentException("No category found for given category id");
                }

                return category;
            }
            set
            {
                bool exists = false;
                for (int i = 0; i < _categories.Count; i++)
                {
                    if (_categories[i].Id == categoryId)
                    {
                        _categories[i] = value;
                        exists = true;
                    }
                }
                if (!exists)
                {
                    throw new ArgumentException("No category found for given category id");
                }
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Check if CategoryTreeItemCollection contains CategoryTreeItem with given categoryId
        /// </summary>
        /// <param name="categoryId">CategoryId to search in CategoryTreeItemCollection</param>
        /// <returns>
        /// <para>true : If CategoryTreeItem found in CategoryTreeItemCollection</para>
        /// <para>false : If CategoryTreeItem found not in CategoryTreeItemCollection</para>
        /// </returns>
        public bool Contains(Int64 categoryId)
        {
            return _categories.Any(x => x.Id == categoryId);
        }

        /// <summary>
        /// Remove CategoryTreeItem object from CategoryTreeItemCollection
        /// </summary>
        /// <param name="categoryId">Id of CategoryTreeItem which is to be removed from collection</param>
        /// <returns>true if item is successfully removed; otherwise, false. This method also returns false if item was not found in the original collection</returns>
        public bool Remove(Int64 categoryId)
        {
            bool founded = false;
            for (int i = _categories.Count - 1; i >= 0; i--)
            {
                if (_categories[i].Id == categoryId)
                {
                    _categories.RemoveAt(i);
                    founded = true;
                }
            }
            return founded;
        }

        /// <summary>
        /// Get Xml representation of CategoryTreeItem Collection
        /// </summary>
        /// <returns>Xml representation of CategoryTreeItem Collection</returns>
        public String ToXml()
        {
            var returnXml = new StringBuilder();
            returnXml.Append("<CategoryTreeItems>");

            if (_categories != null && _categories.Count > 0)
            {
                foreach (var category in _categories)
                {
                    returnXml.Append(category.ToXml());
                }
            }

            returnXml.Append("</CategoryTreeItems>");

            return returnXml.ToString();
        }

        /// <summary>
        /// Get Xml representation of CategoryTreeItem Collection based on requested object serialization
        /// </summary>
        /// <param name="objectSerialization">Type of the serialization option</param>
        /// <returns>Xml representation of CategoryTreeItem Collection</returns>
        public String ToXml(ObjectSerialization objectSerialization)
        {
            var returnXml = new StringBuilder();
            returnXml.Append("<CategoryTreeItems>");

            if (_categories != null && _categories.Count > 0)
            {
                foreach (var category in _categories)
                {
                    returnXml.Append(category.ToXml(objectSerialization));
                }
            }

            returnXml.Append("</CategoryTreeItems>");

            return returnXml.ToString();
        }

        /// <summary>
        /// Determines whether two Object instances are equal (invariant to items order)
        /// </summary>
        /// <param name="obj">The Object to compare with the current Object</param>
        /// <returns>true if the specified Object is equal to the current Object; otherwise, false</returns>
        public override bool Equals(object obj)
        {
            if (obj is CategoryTreeItemCollection)
            {
                var objectToBeCompared = obj as CategoryTreeItemCollection;
                Int32 categoriesUnion = _categories.ToList().Union(objectToBeCompared.ToList()).Count();
                Int32 categoriesIntersect = _categories.ToList().Intersect(objectToBeCompared.ToList()).Count();

                return (categoriesUnion == categoriesIntersect);
            }
            return false;
        }

        /// <summary>
        /// Serves as a hash function for type (invariant to items order)
        /// </summary>
        /// <returns>A hash code for the current Object</returns>
        public override int GetHashCode()
        {
            Int32 hashCode = 0;

            foreach (var category in _categories)
            {
                hashCode = unchecked(hashCode + category.GetHashCode());
            }

            return hashCode;
        }

        /// <summary>
        /// Initialize current object with xml
        /// </summary>
        /// <param name="valuesAsXml">Xml having value for current CategoryTreeItem collection</param>
        public void LoadCategoryTreeItemCollection(String valuesAsXml)
        {
            Clear();
            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                using (var reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null))
                {
                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "CategoryTreeItem")
                        {
                            String categoryXml = reader.ReadOuterXml();
                            if (!String.IsNullOrEmpty(categoryXml))
                            {
                                var category = new CategoryTreeItem(categoryXml);
                                if (category != null)
                                {
                                    Add(category);
                                }
                            }
                        }
                        else
                        {
                            reader.Read();
                        }
                    }
                }
            }
        }

        #endregion

        #region Private Methods

        private CategoryTreeItem GetCategoryTreeItem(Int64 categoryId)
        {
            return _categories.FirstOrDefault(x => x.Id == categoryId);
        }

        #endregion

        #region ICollection<CategoryTreeItem> Members

        /// <summary>
        /// Add CategoryTreeItem object in collection
        /// </summary>
        /// <param name="item">CategoryTreeItem to add in collection</param>
        public void Add(CategoryTreeItem item)
        {
            _categories.Add(item);
        }

        /// <summary>
        /// Removes all CategoryTreeItem from collection
        /// </summary>
        public void Clear()
        {
            _categories.Clear();
        }

        /// <summary>
        /// Determines whether the CategoryTreeItemCollection contains a specific CategoryTreeItem
        /// </summary>
        /// <param name="item">The CategoryTreeItem object to locate in the CategoryTreeItemCollection</param>
        /// <returns>
        /// <para>true : If category found in categoryTreeItemCollection</para>
        /// <para>false : If category not found in categoryTreeItemCollection</para>
        /// </returns>
        public bool Contains(CategoryTreeItem item)
        {
            return _categories.Contains(item);
        }

        /// <summary>
        /// Copies the elements of the CategoryTreeItemCollection to an
        /// System.Array, starting at a particular System.Array index
        /// </summary>
        /// <param name="array"> 
        /// The one-dimensional System.Array that is the destination of the elements
        /// copied from CategoryTreeItemCollection. The System.Array must have zero-based indexing
        /// </param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins</param>
        public void CopyTo(CategoryTreeItem[] array, int arrayIndex)
        {
            _categories.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Get the count of no. of categories in CategoryTreeItemCollection
        /// </summary>
        public int Count
        {
            get
            {
                return _categories.Count;
            }
        }

        /// <summary>
        /// Check if categoryTreeItemCollection is read-only
        /// </summary>
        public bool IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// Removes the first occurrence of a specific CategoryTreeItem from the categoryTreeItemCollection
        /// </summary>
        /// <param name="item">The CategoryTreeItem object to remove from the categoryTreeItemCollection</param>
        /// <returns>true if item is successfully removed; otherwise, false. This method also returns false if item was not found in the original collection</returns>
        public bool Remove(CategoryTreeItem item)
        {
            return _categories.Remove(item);
        }

        #endregion

        #region IEnumerable<CategoryTreeItem> Members

        /// <summary>
        /// Returns an enumerator that iterates through a CategoryTreeItemCollection
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection</returns>
        public IEnumerator<CategoryTreeItem> GetEnumerator()
        {
            return _categories.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        /// <summary>
        /// Returns an enumerator that iterates through a CategoryTreeItemCollection
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return _categories.GetEnumerator();
        }

        #endregion
    }
}