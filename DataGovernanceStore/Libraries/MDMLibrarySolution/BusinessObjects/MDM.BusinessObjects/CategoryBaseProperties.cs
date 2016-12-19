using System;
using System.Xml;
using System.IO;
using System.Runtime.Serialization;

namespace MDM.BusinessObjects
{
    using MDM.Core;

    /// <summary>
    /// Specifies the category base properties object
    /// </summary>
    [DataContract]
    public class CategoryBaseProperties : MDMObject
    {
        #region Fields

        /// <summary>
        /// Field for the id of a Category
        /// </summary>
        private Int64 _id = -1;

        /// <summary>
        /// Field denoting Hierarchy Id
        /// </summary>
        private Int32 _hierarchyId = -1;

        /// <summary>
        /// Field denoting Hierarchy name
        /// </summary>
        private String _hierarchyName = String.Empty;

        /// <summary>
        /// Field denoting Hierarchy long name
        /// </summary>
        private String _hierarchyLongName = String.Empty;

        /// <summary>
        /// Field denoting level of the category in the category tree
        /// </summary>
        private Int32 _level = 0;

        /// <summary>
        /// Field denoting the path of the category
        /// </summary>
        private String _path = String.Empty;

        /// <summary>
        /// Field denoting the path in terms of long name of the category
        /// </summary>
        private String _longNamePath = String.Empty;

        /// <summary>
        /// Field denoting whether the Category has child categories or not
        /// </summary>
        private Boolean _isLeaf = false;

        /// <summary>
        /// Field denoting Parent Category Id 
        /// </summary>
        private Int64 _parentCategoryId = 0;

        /// <summary>
        /// Field denoting parent category name
        /// </summary>
        private String _parentCategoryName = String.Empty;

        /// <summary>
        /// Field denoting the id Path of category
        /// </summary>
        private String _idPath = String.Empty;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the CategoryBaseProperties class.
        /// </summary>
        public CategoryBaseProperties()
            : base()
        {

        }

        #endregion

        #region Properties

        /// <summary>
        /// Property for the id of the Category
        /// </summary>
        [DataMember]
        public new Int64 Id
        {
            get { return _id; }
            set { _id = value; }
        }

        /// <summary>
        /// Hierarchy Id of the Category
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
        /// Property defining the type of the MDM object
        /// </summary>
        public new String ObjectType
        {
            get
            {
                return "Category";
            }
        }

        /// <summary>
        /// Property denoting level of the category in the category tree
        /// </summary>
        [DataMember]
        public Int32 Level
        {
            get
            {
                return _level;
            }
            set
            {
                _level = value;
            }
        }

        /// <summary>
        /// Property denoting the path of the category
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
        /// Field denoting whether the Category has child categories or not
        /// </summary>
        [DataMember]
        public Boolean IsLeaf
        {
            get
            {
                return _isLeaf;
            }
            set
            {
                _isLeaf = value;
            }
        }

        /// <summary>
        /// Field denoting parent category Id
        /// </summary>
        [DataMember]
        public Int64 ParentCategoryId
        {
            get { return this._parentCategoryId; }
            set { this._parentCategoryId = value; }
        }

        /// <summary>
        /// Specifies parent category name
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
        /// Property denoting the IDPath of category
        /// </summary>
        [DataMember]
        public String IdPath
        {
            get
            {
                return this._idPath;
            }
            set
            {
                this._idPath = value;
            }
        }

        #endregion

        #region Methods

        #region Public Methods

        #endregion

        #region Private Methods
        #endregion

        #endregion
    }
}