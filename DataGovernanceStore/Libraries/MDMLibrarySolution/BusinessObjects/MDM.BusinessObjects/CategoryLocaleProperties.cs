using System;
using System.Runtime.Serialization;

namespace MDM.BusinessObjects
{
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Specifies the category base properties object
    /// </summary>
    [DataContract]
    public class CategoryLocaleProperties : MDMObject, IDataModelObject
    {
        #region Fields

        /// <summary>
        /// Field denoting category id.
        /// </summary>
        private Int64 _categoryId = 0;

        /// <summary>
        /// Field denoting hierarchy id.
        /// </summary>
        private Int32 _hierarchyId = 0;

        /// <summary>
        /// Field denoting Hierarchy name
        /// </summary>
        private String _hierarchyName = String.Empty;

        /// <summary>
        /// Field denoting Hierarchy long name
        /// </summary>
        private String _hierarchyLongName = String.Empty;

        /// <summary>
        /// Field denoting the path in terms of name of the category
        /// </summary>
        private String _path = String.Empty;

        /// <summary>
        /// Field denoting the path in terms of long name of the category
        /// </summary>
        private String _longNamePath = String.Empty;

        /// <summary>
        /// Field Denoting the original category locale properties
        /// </summary>
        private CategoryLocaleProperties _originalCategoryLocaleProperties = null;

        /// <summary>
        /// Field denoting if the Category attribute has a locale properties or not
        /// </summary>
        private Boolean _hasLocaleProperties = false;

        /// <summary>
        /// Field denoting parent category name
        /// </summary>
        private String _parentCategoryName = String.Empty;

        #region IDataModelObject Fields

        /// <summary>
        /// Field denoting category locale properties key External Id from external source.
        /// </summary>
        private String _externalId = String.Empty;

        #endregion

        #endregion

        #region Constructors

        /// <summary>
        /// Parameter Less Constructor
        /// </summary>
        public CategoryLocaleProperties() { }

        #endregion

        #region Properties

        /// <summary>
        /// Specifies category id
        /// </summary>
        [DataMember]
        public Int64 CategoryId
        {
            get
            {
                return _categoryId;
            }
            set
            {
                _categoryId = value;
            }
        }

        /// <summary>
        /// Specifies hierarchy id
        /// </summary>
        [DataMember]
        public Int32 HierarchyId
        {
            get
            {
                return _hierarchyId;
            }
            set
            {
                _hierarchyId = value;
            }
        }

        /// <summary>
        /// Hierarchy Name of the Category
        /// </summary>
        [DataMember]
        public String HierarchyName
        {
            get
            {
                return _hierarchyName;
            }
            set
            {
                _hierarchyName = value;
            }
        }

        /// <summary>
        /// Hierarchy LongName of the Category
        /// </summary>
        [DataMember]
        public String HierarchyLongName
        {
            get
            {
                return _hierarchyLongName;
            }
            set
            {
                _hierarchyLongName = value;
            }
        }

        /// <summary>
        /// Property denoting the path in terms of  name of the category
        /// </summary>
        [DataMember]
        public String Path
        {
            get
            {
                return _path;
            }
            set
            {
                _path = value;
            }
        }

        /// <summary>
        /// Property denoting the path in terms of long name of the category
        /// </summary>
        [DataMember]
        public String LongNamePath
        {
            get
            {
                return _longNamePath;
            }
            set
            {
                _longNamePath = value;
            }
        }

        /// <summary>
        /// Property denoting the original category locale properties
        /// </summary>
        public CategoryLocaleProperties OriginalCategoryLocaleProperties
        {
            get
            {
                return _originalCategoryLocaleProperties;
            }
            set
            {
                this._originalCategoryLocaleProperties = value;
            }
        }

        /// <summary>
        /// Property denoting the Parent Category Name
        /// </summary>
        [DataMember]
        public String ParentCategoryName
        {
            get
            {
                return _parentCategoryName;
            }
            set
            {
                _parentCategoryName = value;
            }
        }

        /// <summary>
        /// Property denoting if the Category attribute has a locale properties or not
        /// </summary>
        [DataMember]
        public Boolean HasLocaleProperties
        {
            get
            {
                return _hasLocaleProperties;
            }
            set
            {
                this._hasLocaleProperties = value;
            }
        }

        #region IDataModelObject Properties

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

        /// <summary>
        /// Property denoting the external id for an dataModelObject object
        /// </summary>
        public String ExternalId
        {
            get
            {
                return _externalId;
            }
            set
            {
                _externalId = value;
            }
        }

        #endregion

        #endregion

        #region Methods

        #region Public Methods

        /// <summary>
        /// Clone category locale properties object
        /// </summary>
        /// <returns>cloned copy of category locale properties object.</returns>
        public CategoryLocaleProperties Clone()
        {
            CategoryLocaleProperties clonedCategoryLocaleProperties = new CategoryLocaleProperties();

            clonedCategoryLocaleProperties.Id = this.Id;
            clonedCategoryLocaleProperties.Name = this.Name;
            clonedCategoryLocaleProperties.LongName = this.LongName;
            clonedCategoryLocaleProperties.Locale = this.Locale;
            clonedCategoryLocaleProperties.Action = this.Action;
            clonedCategoryLocaleProperties.AuditRefId = this.AuditRefId;
            clonedCategoryLocaleProperties.ExtendedProperties = this.ExtendedProperties;

            clonedCategoryLocaleProperties.CategoryId = this.CategoryId;
            clonedCategoryLocaleProperties.HierarchyId = this.HierarchyId;
            clonedCategoryLocaleProperties.HierarchyName = this.HierarchyName;
            clonedCategoryLocaleProperties.HierarchyLongName = this.HierarchyLongName;
            clonedCategoryLocaleProperties.Path = this.Path;
            clonedCategoryLocaleProperties.LongNamePath = this.LongNamePath;

            return clonedCategoryLocaleProperties;
        }

        /// <summary>
        /// Determines whether specified object is equal to the current object.
        /// </summary>
        /// <param name="obj">EntityType object which needs to be compared.</param>
        /// <returns>Result of the comparison in Boolean.</returns>
        public override bool Equals(object obj)
        {
            if (obj is CategoryLocaleProperties)
            {
                CategoryLocaleProperties objectToBeCompared = obj as CategoryLocaleProperties;

                if (!base.Equals(objectToBeCompared))
                    return false;

                if (this.HierarchyLongName != objectToBeCompared.HierarchyLongName)
                    return false;

                if (this.LongNamePath != objectToBeCompared.LongNamePath)
                    return false;

                return true;
            }

            return false;
        }

        /// <summary>
        /// Delta Merge of category locale properties
        /// </summary>
        /// <param name="deltaCategoryLocaleProperties">Entity Type that needs to be merged.</param>
        /// <param name="iCallerContext">Indicates the name of application and the module that are performing the action</param>
        /// <param name="returnClonedObject">true means return new merged cloned object;otherwise, same object.</param>
        /// <returns>Merged entity type instance</returns>
        public CategoryLocaleProperties MergeDelta(CategoryLocaleProperties deltaCategoryLocaleProperties, ICallerContext iCallerContext, Boolean returnClonedObject = true)
        {
            CategoryLocaleProperties mergedCategoryLocaleProperties = (returnClonedObject == true) ? deltaCategoryLocaleProperties.Clone() : deltaCategoryLocaleProperties;

            //The category locale properties pass action as Create always. In case of Update action also, DB will internally handle Update action.
            //This is change required to handle existing behavior.
            mergedCategoryLocaleProperties.Action = (mergedCategoryLocaleProperties.Equals(this)) ? ObjectAction.Read : ObjectAction.Create;

            return mergedCategoryLocaleProperties;
        }

        /// <summary>
        ///  Serves as a hash function for CategoryLocaleProperties
        /// </summary>
        /// <returns>A hash code for the current Object.</returns>
        public override int GetHashCode()
        {
            return base.GetHashCode() ^ this._categoryId.GetHashCode() ^ this._hierarchyId.GetHashCode() ^ this._hierarchyName.GetHashCode() ^ this._hierarchyLongName.GetHashCode()
                ^ this._longNamePath.GetHashCode() ^ this._path.GetHashCode() ^ this._originalCategoryLocaleProperties.GetHashCode() ^ this._externalId.GetHashCode();
        }

        #region IDataModelObject Methods

        /// <summary>
        /// Get IDataModelObject for current object.
        /// </summary>
        /// <returns>IDataModelObject</returns>
        public IDataModelObject GetDataModelObject()
        {
            return this;
        }

        #endregion

        #endregion

        #region Private Methods
        #endregion

        #endregion
    }
}