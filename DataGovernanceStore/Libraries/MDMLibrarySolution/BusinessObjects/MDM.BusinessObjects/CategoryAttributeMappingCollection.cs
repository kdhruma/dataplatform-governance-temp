using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace MDM.BusinessObjects
{
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Specifies the category attribute mapping collection for the object
    /// </summary>
    [DataContract]
    public class CategoryAttributeMappingCollection : InterfaceContractCollection<ICategoryAttributeMapping, CategoryAttributeMapping>, ICategoryAttributeMappingCollection, IDataModelObjectCollection
    {
        #region Fields
        #endregion

        #region Constructors

        /// <summary>
        /// Parameterless constructor
        /// </summary>
        public CategoryAttributeMappingCollection() : base() { }

        /// <summary>
        /// Constructor which takes Xml as input
        /// </summary>
        public CategoryAttributeMappingCollection(String valueAsXml)
        {
            LoadCategoryAttributeMappingCollection(valueAsXml);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CategoryAttributeMappingCollection"/> class.
        /// </summary>
        /// <param name="categoryAttributeMappings">The category attribute mappings.</param>
        public CategoryAttributeMappingCollection(IEnumerable<CategoryAttributeMapping> categoryAttributeMappings)
        {
            this._items = new Collection<CategoryAttributeMapping>(categoryAttributeMappings.ToList());
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Property denoting a ObjectType for an dataModelObject object
        /// </summary>
        public ObjectType DataModelObjectType
        {
            get
            {
                return MDM.Core.ObjectType.CategoryAttributeMapping;
            }
        }

        #endregion

        #region Methods

        #region Public Methods

        /// <summary>
        /// Verify whether or not sequence contain required item
        /// </summary>
        /// <param name="categoryAttributeMappingId">Id of the categoryAttributeMapping</param>
        /// <returns>[True] if item exists, [False] otherwise</returns>
        public Boolean Contains(int categoryAttributeMappingId)
        {
            return this.Get(categoryAttributeMappingId) != null;
        }

        /// <summary>
        /// Adds the range.
        /// </summary>
        /// <param name="collection">The collection.</param>
        public ICategoryAttributeMappingCollection AddRange(ICategoryAttributeMappingCollection collection)
        {
            var newCollection = new CategoryAttributeMappingCollection();

            foreach (CategoryAttributeMapping categoryAttributeMapping in collection)
            {
                if (!this._items.Contains(categoryAttributeMapping))
                {
                    this._items.Add(categoryAttributeMapping);
                }
            }

            return this;
        }

        #region CategoryAttributeMapping Get

        /// <summary>
        /// Gets the category attribute mapping.
        /// </summary>
        /// <param name="categoryAttributeMapping">The category attribute mapping.</param>
        /// <returns></returns>
        /// <exception cref="System.NullReferenceException">There are no Category - Attribute Mappings to search in</exception>
        /// <exception cref="System.ArgumentException">Category - Attribute Mapping Id must be greater than 0</exception>
        public CategoryAttributeMapping GetCategoryAttributeMapping(Int32 categoryAttributeMapping)
        {
            if (this._items == null)
            {
                throw new NullReferenceException("There are no Category - Attribute Mappings to search in");
            }

            if (categoryAttributeMapping <= 0)
            {
                throw new ArgumentException("Category - Attribute Mapping Id must be greater than 0", categoryAttributeMapping.ToString());
            }

            return this.Get(categoryAttributeMapping) as CategoryAttributeMapping;
        }

        #endregion CategoryAttributeMapping Get

        /// <summary>
        /// Removes item by id
        /// </summary>
        /// <param name="categoryAttributeMappingId">Id of item to be removed</param>
        /// <returns>[True] if removal was successful, [False] otherwise</returns>
        public Boolean Remove(int categoryAttributeMappingId)
        {
            ICategoryAttributeMapping categoryAttributeMapping = Get(categoryAttributeMappingId);

            if (categoryAttributeMapping == null)
                throw new ArgumentException("No category - attribute mapping found for given Id :" + categoryAttributeMappingId);

            return this.Remove(categoryAttributeMapping);
        }

        /// <summary>
        /// Convert item to XML
        /// </summary>
        /// <returns>XML string</returns>
        public String ToXml()
        {
            String categoryAttributeMappingXML = String.Empty;
            StringBuilder builder = new StringBuilder();

            foreach (CategoryAttributeMapping categoryAttributemMapping in this._items)
            {
                builder.Append(categoryAttributemMapping.ToXml());
            }

            categoryAttributeMappingXML = String.Format("<CategoryAttributemMappingCollection>{0}</CategoryAttributemMappingCollection>", builder);
            return categoryAttributeMappingXML;
        }

        /// <summary>
        /// Convert item to XML with specific rule
        /// </summary>
        /// <param name="serialization">Rules of serialization</param>
        /// <returns></returns>
        public String ToXml(ObjectSerialization serialization)
        {
            return this.ToXml();
        }

        /// <summary>
        /// Clones item
        /// </summary>
        /// <returns>Cloned item</returns>
        public ICategoryAttributeMappingCollection Clone()
        {
            CategoryAttributeMappingCollection clonedAttributeMappingCollection = new CategoryAttributeMappingCollection();

            if (this._items != null && this._items.Count > 0)
            {
                foreach (CategoryAttributeMapping categoryAttributeMapping in this._items)
                {
                    ICategoryAttributeMapping clonedIhierarchy = categoryAttributeMapping.Clone();
                    clonedAttributeMappingCollection.Add(clonedIhierarchy);
                }
            }

            return clonedAttributeMappingCollection;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="categoryAttributeMappingCollection"></param>
        /// <param name="compareIds"></param>
        /// <returns></returns>
        public Boolean IsSuperSetOf(CategoryAttributeMappingCollection categoryAttributeMappingCollection, Boolean compareIds = false)
        {

            foreach (CategoryAttributeMapping categoryattributeMap in categoryAttributeMappingCollection)
            {

                IEnumerable<CategoryAttributeMapping> categoryAttributeMapping;

                if (compareIds)
                {
                    categoryAttributeMapping = this._items.Where(c => c.Id == categoryattributeMap.Id && c.LongName == categoryattributeMap.LongName);
                }
                else
                {
                    categoryAttributeMapping = this._items.Where(c => c.LongName == categoryattributeMap.LongName);
                }

                if (categoryAttributeMapping != null && categoryAttributeMapping.Count() > 0)
                {
                    CategoryAttributeMapping newCategoryAttributeMapping = categoryAttributeMapping.FirstOrDefault();

                    newCategoryAttributeMapping.IsSuperSetOf(categoryattributeMap, compareIds);
                }
                else
                {
                    return false;
                }
            }

            return true;

        }

        /// <summary>
        /// Gets the by category identifier.
        /// </summary>
        /// <param name="categoryId">The category identifier.</param>
        /// <returns></returns>
        public ICategoryAttributeMappingCollection GetByCategoryId(Int64 categoryId)
        {
            ICategoryAttributeMappingCollection categoryAttributeMappings = null;

            if (this._items != null && this._items.Count > 0)
            {
                categoryAttributeMappings = new CategoryAttributeMappingCollection();

                foreach (CategoryAttributeMapping categoryAttributeMapping in this._items)
                {
                    if (categoryAttributeMapping.CategoryId == categoryId)
                    {
                        categoryAttributeMappings.Add(categoryAttributeMapping);
                    }
                }
            }

            return categoryAttributeMappings;
        }

        /// <summary>
        /// Gets item by Id
        /// </summary>
        /// <param name="categoryAttributeMappingId">Item Id</param>
        /// <returns>Cloned item</returns>
        public ICategoryAttributeMapping Get(Int32 categoryAttributeMappingId)
        {
            if (this._items != null && this._items.Count > 0)
            {
                foreach (CategoryAttributeMapping categoryAttributeMapping in this._items)
                {
                    if (categoryAttributeMapping.Id == categoryAttributeMappingId)
                    {
                        return categoryAttributeMapping;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Gets the CategoryAttributeMapping for given CategoryName, AttributeName and AttributeParentName
        /// </summary>
        /// <param name="hierarchyName">HierarchyName to be searched in the collection</param>
        /// <param name="categoryName">CategoryName to be searched in the collection</param>
        /// <param name="path">Category path to be searched in the collection</param>
        /// <param name="attributeName">AttributeName to be searched in the collection</param>
        /// <param name="attributeParentName">attributeParentName to be searched in the collection</param>
        /// <param name="sourceFlag">Source flag to be searched in the collection. Allowed values are 'O' or 'I'.</param>
        /// <returns>CategoryAttributeMapping having given CategoryName, AttributeName and AttributeParentName</returns>
        public ICategoryAttributeMapping Get(String hierarchyName, String categoryName, String path, String attributeName, String attributeParentName, String sourceFlag)
        {
            if (this._items != null && this._items.Count > 0)
            {
                foreach (CategoryAttributeMapping categoryAttributeMapping in this._items)
                {
                    if (String.Compare(categoryAttributeMapping.HierarchyName, hierarchyName, StringComparison.InvariantCultureIgnoreCase) == 0 &&
                        String.Compare(categoryAttributeMapping.CategoryName, categoryName, StringComparison.InvariantCultureIgnoreCase) == 0 &&
                        String.Compare(categoryAttributeMapping.Path, path, StringComparison.InvariantCultureIgnoreCase) == 0 &&
                        String.Compare(categoryAttributeMapping.AttributeName, attributeName, StringComparison.InvariantCultureIgnoreCase) == 0 &&
                        String.Compare(categoryAttributeMapping.AttributeParentName, attributeParentName, StringComparison.InvariantCultureIgnoreCase) == 0 &&
                        String.Compare(categoryAttributeMapping.SourceFlag, sourceFlag, StringComparison.InvariantCultureIgnoreCase) == 0)
                    {
                        return categoryAttributeMapping;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Gets the CategoryAttributeMapping for given CategoryId, AttributeId and SourceFlag.
        /// </summary>
        /// <param name="categoryId">CategoryId to be searched in the collection.</param>
        /// <param name="attributeId">AttributeId to be searched in the collection.</param>
        /// <param name="sourceFlag">SourceFlag to be searched in the collection.</param>
        /// <returns>CategoryAttributeMapping having given CategoryId, AttributeId and SourceFlag.</returns>
        public ICategoryAttributeMapping Get(Int64 categoryId, Int32 attributeId, String sourceFlag)
        {
            if (this._items != null && this._items.Count > 0)
            {
                foreach (CategoryAttributeMapping categoryAttributeMapping in this._items)
                {
                    if (categoryAttributeMapping.CategoryId == categoryId && categoryAttributeMapping.AttributeId == attributeId &&
                        String.Compare(categoryAttributeMapping.SourceFlag, sourceFlag, StringComparison.InvariantCultureIgnoreCase) == 0)
                    {
                        return categoryAttributeMapping;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Gets the CategoryAttributeMapping for given AttributeId.
        /// </summary>
        /// <param name="attributeId">AttributeId to be searched in the collection.</param>
        /// <returns>CategoryAttributeMapping having given AttributeId.</returns>
        public ICategoryAttributeMapping GetByAttributeId(Int32 attributeId)
        {
            if (this._items != null && this._items.Count > 0)
            {
                foreach (CategoryAttributeMapping categoryAttributeMapping in this._items)
                {
                    if (categoryAttributeMapping.AttributeId == attributeId)
                    {
                        return categoryAttributeMapping;
                    }
                }
            }

            return null;
        }

        #region Implementation of IClonable

        object ICloneable.Clone()
        {
            return this.Clone();
        }

        #endregion Implementation of IClonable

        #region IDataModelObjectCollection
        /// <summary>
        /// Remove dataModelObject based on reference id.
        /// </summary>
        /// <param name="referenceIds">Indicates collection of referenceId of an CategoryAttributeMapping which is to be fetched.</param>
        /// <returns>true if item was successfully removed from the collection;otherwise, false. This method also returns false if item is not found in the original</returns>
        public bool Remove(Collection<string> referenceIds)
        {
            Boolean result = true;

            CategoryAttributeMappingCollection categoryAttributeMappings = Get(referenceIds);

            if (categoryAttributeMappings != null && categoryAttributeMappings.Count > 0)
            {
                foreach (CategoryAttributeMapping categoryAttributeMapping in categoryAttributeMappings)
                {
                    result = result && this.Remove(categoryAttributeMapping);
                }
            }

            return result;
        }

        /// <summary>
        /// Splits current collection based on batchSize
        /// </summary>
        /// <param name="batchSize">Indicates no of objects to be put in each batch</param>
        /// <returns>IDataModelObjectCollection in Batch</returns>
        public Collection<IDataModelObjectCollection> Split(Int32 batchSize)
        {
            Collection<IDataModelObjectCollection> categoryAttributeMappingsInBatch = null;

            if (this._items != null)
            {
                categoryAttributeMappingsInBatch = Utility.Split(this, batchSize);
            }

            return categoryAttributeMappingsInBatch;
        }

        /// <summary>
        /// Adds item in current collection
        /// </summary>
        /// <param name="item">Indicates item to be added in collection</param>
        public void AddDataModelObject(IDataModelObject item)
        {
            this.Add(item as CategoryAttributeMapping);
        }

        #endregion

        #endregion public Methods

        #region Private Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="valuesAsXml"></param>
        private void LoadCategoryAttributeMappingCollection(string valuesAsXml)
        {
            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                XmlTextReader reader = null;
                try
                {
                    reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "CategoryAttributeMapping")
                        {
                            String categoryAttributeMappingXml = reader.ReadOuterXml();
                            if (!String.IsNullOrEmpty(categoryAttributeMappingXml))
                            {
                                CategoryAttributeMapping categoryAttributeMapping = new CategoryAttributeMapping
                                    (categoryAttributeMappingXml);
                                this.Add(categoryAttributeMapping);
                            }
                        }
                        else
                        {
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
        /// Gets the CategoryAttributeMappingCollection for given referenceIds.
        /// </summary>
        /// <param name="referenceIds">referenceIds to be searched in the collection</param>
        /// <returns>CategoryAttributeMappingCollection having given referenceIds</returns>
        private CategoryAttributeMappingCollection Get(Collection<String> referenceIds)
        {
            CategoryAttributeMappingCollection categoryAttributeMappings = new CategoryAttributeMappingCollection();
            Int32 counter = 0;

            if (this._items != null && this._items.Count > 0 && referenceIds != null && referenceIds.Count > 0)
            {
                foreach (CategoryAttributeMapping categoryAttributeMapping in this._items)
                {
                    if (referenceIds.Contains(categoryAttributeMapping.ReferenceId))
                    {
                        categoryAttributeMappings.Add(categoryAttributeMapping);
                        counter++;
                    }

                    if (referenceIds.Count.Equals(counter))
                        break;
                }
            }
            return categoryAttributeMappings;
        }

        #endregion PrivateMethods

        #endregion Methods
    }
}