using System;
using System.Xml;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

namespace MDM.BusinessObjects
{
    using MDM.Interfaces;
    using MDM.Core;

    /// <summary>
    /// Specifies Category Collection
    /// </summary>
    [DataContract]
    [KnownType(typeof(Category))]
    public class CategoryCollection : InterfaceContractCollection<ICategory, Category>, ICategoryCollection, IDataModelObjectCollection
    {
        #region Fields
        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the CategoryCollection class.
        /// </summary>
        public CategoryCollection() { }

        /// <summary>
        /// Initializes a new instance of the Category class.
        /// </summary>
        /// <param name="valuesAsXml">CategoryCollection Object in XML representation</param>
        public CategoryCollection(String valuesAsXml)
        {
            LoadCategorySearchRuleCollection(valuesAsXml);
        }

        /// <summary>
        /// Initializes a new instance of the Category class.
        /// </summary>
        /// <param name="categoryList">List of category object</param>
        public CategoryCollection(List<Category> categoryList)
        {
            if (categoryList != null)
            {
                this._items = new Collection<Category>(categoryList);
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
                return MDM.Core.ObjectType.Category;
            }
        }

        #endregion

        #region Methods

        #region Public Methods

        /// <summary>
        /// Get Xml representation of Category Collection
        /// </summary>
        /// <returns>Xml representation of Category Collection</returns>
        public String ToXml()
        {
            String categoryCollectionXml = String.Empty;

            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            //Parameter node start
            xmlWriter.WriteStartElement("Categories");

            if (this._items != null && this._items.Count > 0)
            {
                foreach (Category item in this._items)
                {
                    xmlWriter.WriteRaw(item.ToXml());
                }
            }

            //Param node end
            xmlWriter.WriteEndElement();

            xmlWriter.Flush();

            //Get the actual XML
            categoryCollectionXml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return categoryCollectionXml;
        }

        /// <summary>
        /// Get Xml representation of Category Collection based on requested object serialization
        /// </summary>
        /// <param name="objectSerialization">Type of the serialization option</param>
        /// <returns>Xml representation of Category Collection</returns>
        public String ToXml(ObjectSerialization objectSerialization)
        {
            String categoryCollectionXml = String.Empty;

            using (StringWriter sw = new StringWriter())
            {
                using (XmlTextWriter xmlWriter = new XmlTextWriter(sw))
                {
                    //Parameter node start
                    xmlWriter.WriteStartElement("Categories");

                    if (this._items != null && this._items.Count > 0)
                    {
                        foreach (Category item in this._items)
                        {
                            xmlWriter.WriteRaw(item.ToXml(objectSerialization));
                        }
                    }

                    //Param node end
                    xmlWriter.WriteEndElement();
                    xmlWriter.Flush();
                    //Get the actual XML
                    categoryCollectionXml = sw.ToString();
                }
            }

            return categoryCollectionXml;
        }

        /// <summary>
        /// Compare categoryCollection with current collection.
        /// This method will compare categoryCollection. If current collection has more categoryCollection than object to be compared, extra categoryCollection will be ignored.
        /// If category to be compared has categories which is not there in current collection, it will return false.
        /// </summary>
        /// <param name="subsetCategoryCollection">CategoryCollection to be compared with current collection</param>
        /// <param name="compareIds">Flag to determine whether id based comparison is true or not</param>
        /// <returns>True : Is both are same. False : otherwise</returns>
        public Boolean IsSuperSetOf(CategoryCollection subsetCategoryCollection, Boolean compareIds = false)
        {
            if (subsetCategoryCollection != null)
            {
                foreach (Category childCategory in subsetCategoryCollection)
                {
                    if (this._items != null && this._items.Count > 0)
                    {
                        foreach (Category sourceCategory in this._items)
                        {
                            if (sourceCategory.IsSuperSetOf(childCategory, compareIds))
                            {
                                return true;
                            }
                        }
                        return false;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Add category object in collection
        /// </summary>
        /// <param name="category">Category to add in collection</param>
        /// <param name="ignoreDuplicateCheck">Flag to indicate if we need to skip duplicate check in the current collection</param>
        public void Add(Category category, Boolean ignoreDuplicateCheck)
        {
            if (category != null)
            {
                Boolean doesCategoryExist = true;

                if (!ignoreDuplicateCheck)
                {
                    if (this.Contains(category.Id, category.Locale))
                    {
                        doesCategoryExist = false;
                    }
                }

                if (doesCategoryExist)
                {
                    this._items.Add(category);
                }
            }
        }

        /// <summary>
        /// Adds passed categories to the current collection
        /// </summary>
        /// <param name="categoryCollection">Collection of categories which needs to be added</param>
        /// <param name="excludeNonLocalizedCategories">Indicates whether to exclude categories which does not have localized property or not</param>
        public void AddRange(CategoryCollection categoryCollection, Boolean excludeNonLocalizedCategories = false)
        {
            foreach (Category category in categoryCollection)
            {
                if (!this.Contains(category.Id, category.Locale))
                {
                    if (excludeNonLocalizedCategories && !category.HasLocaleProperties)
                    {
                        continue;
                    }
                    
                    this.Add(category);
                }
            }
        }

        /// <summary>
        /// Check if CategoryCollection contains category with given categoryId and locale
        /// </summary>
        /// <param name="categoryId">Id of the Category</param>
        /// <param name="locale">Locale of an Category</param>
        /// <returns>
        /// <para>true : If Category found in CategoryCollection</para>
        /// <para>false : If Category found not in CategoryCollection</para>
        /// </returns>
        public Boolean Contains(Int64 categoryId, LocaleEnum locale)
        {
            if (GetCategoryById(categoryId, locale) != null)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Get All Child Categories under given categoryId
        /// </summary>
        /// <param name="categoryId">Id of an category</param>
        /// <returns>All Child Categories</returns>
        public CategoryCollection GetChildCategoriesRecursive(Int64 categoryId)
        {
            CategoryCollection childCategories = new CategoryCollection();

            if (this._items != null)
            {
                foreach (Category category in this._items)
                {
                    if (category.ParentCategoryId == categoryId)
                    {
                        childCategories.Add(category);

                        if (!category.IsLeaf)
                        {
                            childCategories.AddRange(GetChildCategoriesRecursive(category.Id));
                        }
                    }
                }
            }

            return childCategories;
        }

        /// <summary>
        /// Find a category from current category collection with given category and hierarchy name.
        /// </summary>
        /// <param name="hierarchyId">Indicates identity of hierarchy which is to be searched in current collection</param>
        /// <param name="path">Indicates path of category which is to be searched in current collection</param>
        /// <returns>Returns category with given name , path and hierarchy name</returns>
        public Category Get(Int32 hierarchyId, String path)
        {
            if (hierarchyId > 0 && !String.IsNullOrWhiteSpace(path))
            {
                path = path.ToLowerInvariant();

                Int32 categoryCount = _items.Count;
                Category category = null;

                for (Int32 index = 0; index < categoryCount; index++)
                {
                    category = _items[index];

                    if (category.HierarchyId == hierarchyId && String.Compare(category.Path.ToLowerInvariant(), path) == 0)
                    {
                        return category;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Get ALL Parent Categories for requested category
        /// </summary>
        /// <param name="category">Category for which parents are requested</param>
        /// <param name="categoryPathSeparator">Category LongNamePath Separate</param>
        /// <returns>All Parent Categories</returns>
        public CategoryCollection GetParentCategoriesRecursive(Category category, String categoryPathSeparator)
        {
            CategoryCollection parentCategories = new CategoryCollection();
            String[] categories = null;
                
            if (!String.IsNullOrWhiteSpace(category.LongNamePath))
                categories =  ValueTypeHelper.SplitStringToStringArray(category.LongNamePath, categoryPathSeparator, StringSplitOptions.RemoveEmptyEntries);

            if (this._items != null && categories != null)
            {
                foreach (String categoryName in categories)
                {
                    foreach (Category item in this._items)
                    {
                        if (categoryName != category.LongName && categoryName == item.LongName && category.Locale == item.Locale)
                        {
                            parentCategories.Add(item);
                            break;
                        }
                    }
                }
            }

            return parentCategories;
        }

        /// <summary>
        /// Remove dataModelObject based on reference id.
        /// </summary>
        /// <param name="referenceIds">Indicates collection of referenceId of an entity type which is to be fetched.</param>
        /// <returns>true if item was successfully removed from the collection;otherwise, false. This method also returns false if item is not found in the original</returns>
        public Boolean Remove(Collection<String> referenceIds)
        {
            Boolean result = true;

            CategoryCollection categories = GetCategories(referenceIds);

            if (categories != null && categories.Count > 0)
            {
                foreach (Category category in categories)
                {
                    result = result && this.Remove(category);
                }
            }

            return result;
        }

        /// <summary>
        /// Find a category from current category collection with given category and hierarchy name.
        /// </summary>
        /// <param name="categoryName">Name of category which is to be searched in current collection</param>
        /// <param name="path">Path of category which is to be searched in current collection</param>
        /// <param name="hierarchyName">Name of hierarchy which is to be searched in current collection</param>
        /// <returns>Category with given name , path and hierarchy name</returns>
        public Category Get(String categoryName, String path, String hierarchyName)
        {
            categoryName = categoryName.ToLowerInvariant();
            path = path.ToLowerInvariant();
            hierarchyName = hierarchyName.ToLowerInvariant();

            Int32 categoryCount = _items.Count;
            Category category = null;

            for (Int32 index = 0; index < categoryCount; index++)
            {
                category = _items[index];

                if (category.NameInLowerCase == categoryName &&
                    category.Path.ToLowerInvariant().Equals(path) &&
                    category.HierarchyName.ToLowerInvariant().Equals(hierarchyName))
                {
                    return category;
                }
            }

            return null;
        }

        /// <summary>
        /// Gets category id list from the current category collection
        /// </summary>
        /// <returns>Collection of category ids in current collection</returns>
        /// <exception cref="Exception">Thrown if there are no categories in current category collection</exception>
        public Collection<Int64> GetCategoryIdList()
        {
            #region Parameter validation

            if (this._items == null)
            {
                throw new Exception("There are no categories in collection.");
            }

            #endregion Parameter validation

            Collection<Int64> categoryIdList = new Collection<Int64>();

            foreach (Category category in this._items)
            {
                categoryIdList.Add(category.Id);
            }

            return categoryIdList;
        }

        /// <summary>
        /// Splits current collection based on batchSize
        /// </summary>
        /// <param name="batchSize">Indicates no of objects to be put in each batch</param>
        /// <returns>IDataModelObjectCollection in Batch</returns>
        public Collection<IDataModelObjectCollection> Split(Int32 batchSize)
        {
            Collection<IDataModelObjectCollection> splitedCategories = null;

            if (this._items != null)
            {
                splitedCategories = Utility.Split(this, batchSize);
            }

            return splitedCategories;
        }

        /// <summary>
        /// Adds item in current collection
        /// </summary>
        /// <param name="item">Indicates item to be added in collection</param>
        public void AddDataModelObject(IDataModelObject item)
        {
            this.Add(item as Category);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Initialize current collection of category object through Xml
        /// </summary>
        /// <param name="valuesAsXml">Xml having value for current category collection object</param>
        private void LoadCategorySearchRuleCollection(String valuesAsXml)
        {
            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                XmlTextReader reader = null;
                try
                {
                    reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "Category")
                        {
                            String categoryXml = reader.ReadOuterXml();
                            if (!String.IsNullOrEmpty(categoryXml))
                            {
                                Category category = new Category(categoryXml);

                                if (category != null)
                                {
                                    this.Add(category);
                                }
                            }
                        }
                        else
                        {
                            //Keep on reading the xml until we reach expected node.
                            reader.Read();
                        }
                    }
                }
                finally
                {
                    if (reader != null)
                    {
                        reader.Close();
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="locale"></param>
        /// <returns></returns>
        private Category GetCategoryById(Int64 categoryId, LocaleEnum locale)
        {
            Category category = null;

            if (this._items != null)
            {
                foreach (Category item in this._items)
                {
                    if (item.Id == categoryId && item.Locale == locale)
                    {
                        category = item;
                        break;
                    }
                }
            }

            return category;
        }

        /// <summary>
        ///  Gets the category collection using the reference id.
        /// </summary>
        /// <param name="referenceIds">Indicates collection of referenceId of category which is to be fetched.</param>
        /// <returns>Returns filtered categories</returns>
        private CategoryCollection GetCategories(Collection<String> referenceIds)
        {
            CategoryCollection categories = new CategoryCollection();
            Int32 counter = 0;

            if (this._items != null && this._items.Count > 0 && referenceIds != null && referenceIds.Count > 0)
            {
                foreach (Category category in this._items)
                {
                    if (referenceIds.Contains(category.ReferenceId))
                    {
                        categories.Add(category);
                        counter++;
                    }

                    if (referenceIds.Count.Equals(counter))
                        break;
                }
            }

            return categories;
        }


        #endregion

        #endregion
    }
}