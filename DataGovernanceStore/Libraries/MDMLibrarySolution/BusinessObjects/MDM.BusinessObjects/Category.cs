using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Xml;
using System.IO;
using System.Runtime.Serialization;

namespace MDM.BusinessObjects
{
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Specifies the category object
    /// </summary>
    [DataContract]
    [KnownType(typeof(MDMObject))]
    [KnownType(typeof(ObjectAction))]
    [KnownType(typeof(CategoryBaseProperties))]
    [KnownType(typeof(CategoryBasePropertiesCollection))]
    [KnownType(typeof(CategoryLocaleProperties))]
    [KnownType(typeof(CategoryLocalePropertiesCollection))]
    public class Category : MDMObject, ICategory, IDataModelObject
    {
        #region Fields

        /// <summary>
        /// Field denoting Base Properties of Category
        /// </summary>
        [DataMember(Order = 0)]
        CategoryBaseProperties _categoryBaseProperties = new CategoryBaseProperties();

        /// <summary>
        /// Field denoting Locale based Properties of Category
        /// </summary>
        [DataMember(Order = 1)]
        CategoryLocaleProperties _categoryLocaleProperties = new CategoryLocaleProperties();

        /// <summary>
        /// Field denoting permission set for the current Category.
        /// </summary>
        [DataMember(Order = 100)]
        private Collection<UserAction> _permissionSet = new Collection<UserAction>();

        /// <summary>
        /// Field denoting Overridden Properties of Category
        /// </summary>
        [DataMember(Order = 100)]
        private Dictionary<String, String> _overridenProperties = null;

        /// <summary>
        /// Field Denoting the original category
        /// </summary>
        private Category _originalCategory = null;

        #region IDataModelObject Fields

        /// <summary>
        /// Field denoting category key External Id from external source.
        /// </summary>
        private String _externalId = String.Empty;

        #endregion

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the Category class.
        /// </summary>
        public Category()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the Category class.
        /// </summary>
        /// <param name="id">Id of an Category</param>
        /// <param name="name">Name of an Category</param>
        /// <param name="LongName">LongName of an Category</param>
        public Category(Int64 id, String name, String LongName)
        {
            this._categoryBaseProperties.Id = id;
            this._categoryBaseProperties.Name = name;
            this._categoryBaseProperties.LongName = LongName;
        }

        /// <summary>
        /// Initializes a new instance of the Category class.
        /// </summary>
        /// <param name="id">Id of an Category</param>
        /// <param name="name">Name of an Category</param>
        /// <param name="longName">LongName if an Category</param>
        /// <param name="locale">Locale</param>
        public Category(Int64 id, String name, String longName, LocaleEnum locale)
        {
            this._categoryBaseProperties.Id = id;
            this._categoryBaseProperties.Name = name;
            this._categoryBaseProperties.LongName = longName;
            this.Locale = locale;
        }

        /// <summary>
        /// Initializes a new instance of the Category class.
        /// </summary>
        /// <param name="valuesAsXml">Category Object in XML representation</param>
        public Category(String valuesAsXml)
        {
            LoadCategory(valuesAsXml);
        }

        /// <summary>
        /// Initializes a new instance of the Category class.
        /// </summary>
        /// <param name="categoryBaseProperties">Contains Base Properties for Category</param>
        /// <param name="categoryLocaleProperties">Contains Locale based properties for Category</param>
        public Category(CategoryBaseProperties categoryBaseProperties, CategoryLocaleProperties categoryLocaleProperties)
        {
            if (categoryBaseProperties != null)
            {
                this._categoryBaseProperties = categoryBaseProperties;
            }

            if (categoryLocaleProperties != null)
            {
                this._categoryLocaleProperties = categoryLocaleProperties;
                base.Locale = categoryLocaleProperties.Locale;
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Property for the id of the Category
        /// </summary>
        public new Int64 Id
        {
            get
            {
                if (this._overridenProperties != null && this._overridenProperties.ContainsKey("Id"))
                    return GetOverridenProperties<Int64>("Id");
                else
                    return this._categoryBaseProperties.Id;
            }
            set
            {
                SetOverridenProperties<Int64>("Id", value);
            }
        }

        /// <summary>
        /// Property for the LongName of the Category
        /// </summary>
        public new String LongName
        {
            get
            {
                if (this._categoryLocaleProperties != null && !String.IsNullOrWhiteSpace(this._categoryLocaleProperties.LongName))
                    return this._categoryLocaleProperties.LongName;
                else
                    return this._categoryBaseProperties.LongName;
            }
            set { }
        }

        /// <summary>
        /// Property for the Name of the Category
        /// </summary>
        public new String Name
        {
            get
            {
                return this._categoryBaseProperties.Name;
            }
            set { }
        }

        /// <summary>
        /// Indicates the Name of an object in lower case.
        /// </summary>
        public new String NameInLowerCase
        {
            get
            {
                return this._categoryBaseProperties.NameInLowerCase;
            }
            set { }
        }

        /// <summary>
        /// Hierarchy Id of the Category
        /// </summary>
        public Int32 HierarchyId
        {
            get
            {
                if (this._overridenProperties != null && this._overridenProperties.ContainsKey("HierarchyId"))
                    return GetOverridenProperties<Int32>("HierarchyId");
                else
                    return _categoryBaseProperties.HierarchyId;
            }
            set
            {
                SetOverridenProperties<Int32>("HierarchyId", value);
            }
        }

        /// <summary>
        /// Hierarchy Name of the Category
        /// </summary>
        public String HierarchyName
        {
            get
            {
                return _categoryBaseProperties.HierarchyName;
            }
        }

        /// <summary>
        /// Hierarchy LongName of the Category
        /// </summary>
        public String HierarchyLongName
        {
            get
            {
                if (this._overridenProperties != null && this._overridenProperties.ContainsKey("HierarchyLongName"))
                    return GetOverridenProperties<String>("HierarchyLongName");
                else if (this._categoryLocaleProperties != null && !String.IsNullOrWhiteSpace(this._categoryLocaleProperties.HierarchyLongName))
                    return this._categoryLocaleProperties.HierarchyLongName;
                else
                    return this._categoryBaseProperties.HierarchyLongName;
            }
            set
            {
                SetOverridenProperties<String>("HierarchyLongName", value);
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
        public Int32 Level
        {
            get
            {
                if (this._overridenProperties != null && this._overridenProperties.ContainsKey("Level"))
                    return GetOverridenProperties<Int32>("Level");
                else
                    return _categoryBaseProperties.Level;
            }
            set
            {
                SetOverridenProperties<Int32>("Level", value);
            }
        }

        /// <summary>
        /// Property denoting the path of the category
        /// </summary>
        public String Path
        {
            get
            {
                if (this._overridenProperties != null && this._overridenProperties.ContainsKey("Path"))
                    return GetOverridenProperties<String>("Path");
                else
                    return _categoryBaseProperties.Path;
            }
            set
            {
                SetOverridenProperties<String>("Path", value);
            }
        }

        /// <summary>
        /// Property denoting the path in terms of long name of the category
        /// </summary>
        public String LongNamePath
        {
            get
            {
                if (this._overridenProperties != null && this._overridenProperties.ContainsKey("LongNamePath"))
                    return GetOverridenProperties<String>("LongNamePath");
                else if (this._categoryLocaleProperties != null && !String.IsNullOrWhiteSpace(this._categoryLocaleProperties.LongNamePath))
                    return this._categoryLocaleProperties.LongNamePath;
                else
                    return this._categoryBaseProperties.LongNamePath;
            }
            set
            {
                SetOverridenProperties<String>("LongNamePath", value);
            }
        }

        /// <summary>
        /// Property denoting the category has child categories or not
        /// </summary>
        public Boolean IsLeaf
        {
            get
            {
                if (this._overridenProperties != null && this._overridenProperties.ContainsKey("IsLeaf"))
                    return GetOverridenProperties<Boolean>("IsLeaf");
                else
                    return _categoryBaseProperties.IsLeaf;
            }
            set
            {
                SetOverridenProperties<Boolean>("IsLeaf", value);
            }
        }

        /// <summary>
        /// Property denoting Parent Category Id
        /// </summary>
        public Int64 ParentCategoryId
        {
            get
            {
                if (this._overridenProperties != null && this._overridenProperties.ContainsKey("ParentCategoryId"))
                    return GetOverridenProperties<Int64>("ParentCategoryId");
                else
                    return _categoryBaseProperties.ParentCategoryId;
            }
            set
            {
                SetOverridenProperties<Int64>("ParentCategoryId", value);
            }
        }

        /// <summary>
        /// Property denoting parent category name
        /// </summary>
        public String ParentCategoryName
        {
            get
            {
                if (this._overridenProperties != null && this._overridenProperties.ContainsKey("ParentCategoryName"))
                {
                    return GetOverridenProperties<String>("ParentCategoryName");
                }
                else if (_categoryLocaleProperties != null && !String.IsNullOrWhiteSpace(_categoryLocaleProperties.ParentCategoryName))
                {
                    return _categoryLocaleProperties.ParentCategoryName;
                }
                else
                {
                    return _categoryBaseProperties.ParentCategoryName;
                }
            }
            set
            {
                SetOverridenProperties<String>("ParentCategoryName", value);
            }
        }

        /// <summary>
        /// Property denoting the original category
        /// </summary>
        public Category OriginalCategory
        {
            get
            {
                return _originalCategory;
            }
            set
            {
                this._originalCategory = value;
            }
        }

        /// <summary>
        /// Property denoting if the Category attribute has a locale properties or not
        /// </summary>
        public Boolean HasLocaleProperties
        {
            get
            {
                if (this._categoryLocaleProperties != null)
                    return this._categoryLocaleProperties.HasLocaleProperties;
                else
                    return false;
            }
        }

        /// <summary>
        /// Property denoting IDPath of category
        /// </summary>
        public String IdPath
        {
            get
            {
                return _categoryBaseProperties.IdPath;
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
        /// Determines whether two Object instances are equal.
        /// </summary>
        /// <param name="obj">The Object to compare with the current Object.</param>
        /// <returns>true if the specified Object is equal to the current Object; otherwise, false.</returns>
        public override Boolean Equals(object obj)
        {
            if (base.Equals(obj))
            {
                if (obj is Category)
                {
                    Category objectToBeCompared = obj as Category;

                    if (this.HierarchyId != objectToBeCompared.HierarchyId)
                        return false;

                    if (this.HierarchyLongName != objectToBeCompared.HierarchyLongName)
                        return false;

                    if (this.HierarchyName != objectToBeCompared.HierarchyName)
                        return false;

                    if (this.IsLeaf != objectToBeCompared.IsLeaf)
                        return false;

                    if (this.Level != objectToBeCompared.Level)
                        return false;

                    if (this.Locale != objectToBeCompared.Locale)
                        return false;

                    if (this.LongName != objectToBeCompared.LongName)
                        return false;

                    if (this.LongNamePath != objectToBeCompared.LongNamePath)
                        return false;

                    if (this.Name != objectToBeCompared.Name)
                        return false;

                    if (this.ObjectType != objectToBeCompared.ObjectType)
                        return false;

                    if (this.Path != objectToBeCompared.Path)
                        return false;

                    if (this.PermissionSet != objectToBeCompared.PermissionSet)
                        return false;

                    if (this.ParentCategoryId != objectToBeCompared.ParentCategoryId)
                        return false;

                    if (this.ParentCategoryName != objectToBeCompared.ParentCategoryName)
                        return false;

                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Serves as a hash function for type
        /// </summary>
        /// <returns>A hash code for the current Object.</returns>
        public override Int32 GetHashCode()
        {
            int hashCode = base.GetHashCode() ^ this.HierarchyId.GetHashCode() ^ this.HierarchyLongName.GetHashCode() ^ this.HierarchyName.GetHashCode() ^ this.Id.GetHashCode() ^
                this.IsLeaf.GetHashCode() ^ this.Level.GetHashCode() ^ this.LongName.GetHashCode() ^ this.LongNamePath.GetHashCode() ^ this.Name.GetHashCode() ^
                this.ObjectType.GetHashCode() ^ this.Path.GetHashCode() ^ this.ParentCategoryId.GetHashCode() ^ this.PermissionSet.GetHashCode();

            return hashCode;
        }

        /// <summary>
        /// Get Xml representation of Category
        /// </summary>
        /// <returns>Xml representation of Category</returns>
        public override String ToXml()
        {
            String categoryXml = String.Empty;

            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            //Parameter node start
            xmlWriter.WriteStartElement("Category");

            xmlWriter.WriteAttributeString("Id", this.Id.ToString());
            xmlWriter.WriteAttributeString("HierarchyId", this.HierarchyId.ToString());
            xmlWriter.WriteAttributeString("HierarchyName", this.HierarchyName);
            xmlWriter.WriteAttributeString("HierarchyLongName", this.HierarchyLongName);
            xmlWriter.WriteAttributeString("Name", this.Name);
            xmlWriter.WriteAttributeString("LongName", this.LongName);
            xmlWriter.WriteAttributeString("Path", this.Path);
            xmlWriter.WriteAttributeString("LongNamePath", this.LongNamePath);
            xmlWriter.WriteAttributeString("ParentCategoryId", this.ParentCategoryId.ToString());
            xmlWriter.WriteAttributeString("IsLeaf", this.IsLeaf.ToString());
            xmlWriter.WriteAttributeString("Level", this.Level.ToString());
            xmlWriter.WriteAttributeString("PermissionSet", this.PermissionSet == null ? String.Empty : ValueTypeHelper.JoinCollection(this.PermissionSet, ","));
            xmlWriter.WriteAttributeString("Locale", this.Locale.ToString());
            xmlWriter.WriteAttributeString("Action", this.Action.ToString());
            xmlWriter.WriteAttributeString("IdPath", this.IdPath);

            //Parameter node end
            xmlWriter.WriteEndElement();

            xmlWriter.Flush();

            //Get the actual XML
            categoryXml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return categoryXml;
        }

        /// <summary>
        /// Get Xml representation of Category based on requested object serialization
        /// </summary>
        /// <param name="objectSerialization">Type of the serialization option</param>
        /// <returns>Xml representation of Category</returns>
        public override String ToXml(ObjectSerialization objectSerialization)
        {
            String returnXml = String.Empty;

            if (objectSerialization == ObjectSerialization.Full)
            {
                returnXml = this.ToXml();
            }
            else if (objectSerialization == ObjectSerialization.ProcessingOnly)
            {
                StringWriter sw = new StringWriter();
                XmlTextWriter xmlWriter = new XmlTextWriter(sw);

                //Parameter node start
                xmlWriter.WriteStartElement("Category");

                xmlWriter.WriteAttributeString("Id", this.Id.ToString());
                xmlWriter.WriteAttributeString("HierarchyId", this.HierarchyId.ToString());
                xmlWriter.WriteAttributeString("Name", this.Name);
                xmlWriter.WriteAttributeString("LongName", this.LongName);
                xmlWriter.WriteAttributeString("Path", this.Path);
                xmlWriter.WriteAttributeString("LongNamePath", this.LongNamePath);
                xmlWriter.WriteAttributeString("ParentCategoryId", this.ParentCategoryId.ToString());
                xmlWriter.WriteAttributeString("IsLeaf", this.IsLeaf.ToString());
                xmlWriter.WriteAttributeString("Level", this.Level.ToString());
                xmlWriter.WriteAttributeString("Locale", this.Locale.ToString());
                xmlWriter.WriteAttributeString("PermissionSet", ValueTypeHelper.JoinCollection(this.PermissionSet, ","));
                xmlWriter.WriteAttributeString("Action", this.Action.ToString());
                xmlWriter.WriteAttributeString("IdPath", this.IdPath);

                //Param node end
                xmlWriter.WriteEndElement();

                xmlWriter.Flush();

                //Get the actual XML
                returnXml = sw.ToString();

                xmlWriter.Close();
                sw.Close();
            }
            else if (objectSerialization == ObjectSerialization.Compact)
            {
                StringWriter sw = new StringWriter();
                XmlTextWriter xmlWriter = new XmlTextWriter(sw);

                //Parameter node start
                xmlWriter.WriteStartElement("Category");

                xmlWriter.WriteAttributeString("Id", this.Id.ToString());
                xmlWriter.WriteAttributeString("HierarchyId", this.HierarchyId.ToString());
                xmlWriter.WriteAttributeString("Name", this.Name);
                xmlWriter.WriteAttributeString("LN", this.LongName);
                xmlWriter.WriteAttributeString("Path", this.Path);
                xmlWriter.WriteAttributeString("LNPath", this.LongNamePath);
                xmlWriter.WriteAttributeString("PCId", this.ParentCategoryId.ToString());
                xmlWriter.WriteAttributeString("IL", this.IsLeaf.ToString());
                xmlWriter.WriteAttributeString("Level", this.Level.ToString());
                xmlWriter.WriteAttributeString("Locale", this.Locale.ToString());
                xmlWriter.WriteAttributeString("PSet", ValueTypeHelper.JoinCollection(this.PermissionSet, ","));
                xmlWriter.WriteAttributeString("IdPath", this.IdPath);

                //Param node end
                xmlWriter.WriteEndElement();

                xmlWriter.Flush();

                //Get the actual XML
                returnXml = sw.ToString();

                xmlWriter.Close();
                sw.Close();
            }

            return returnXml;
        }

        /// <summary>
        /// Compare Category with current category.
        /// This method will compare category.
        /// </summary>
        /// <param name="subSetCategory">Category to be compared with current category</param>
        /// <param name="compareIds">Flag to determine whether id based comparison is true or not</param>
        /// <returns>true if the specified Object is equal to the current Object; otherwise, false.</returns>
        public Boolean IsSuperSetOf(Category subSetCategory, Boolean compareIds = false)
        {
            if (subSetCategory != null)
            {
                if (base.IsSuperSetOf(subSetCategory, compareIds))
                {
                    if (compareIds)
                    {
                        if (this.Id != subSetCategory.Id)
                            return false;

                        if (this.HierarchyId != subSetCategory.HierarchyId)
                            return false;

                        if (this.ParentCategoryId != subSetCategory.ParentCategoryId)
                            return false;
                    }

                    if (this.Name != subSetCategory.Name)
                        return false;

                    if (this.LongName != subSetCategory.LongName)
                        return false;

                    if (this.HierarchyName != subSetCategory.HierarchyName)
                        return false;

                    if (this.HierarchyLongName != subSetCategory.HierarchyLongName)
                        return false;

                    if (this.Path != subSetCategory.Path)
                        return false;

                    if (this.LongNamePath != subSetCategory.LongNamePath)
                        return false;

                    if (this.IsLeaf != subSetCategory.IsLeaf)
                        return false;

                    if (this.Level != subSetCategory.Level)
                        return false;

                    if (this.PermissionSet != subSetCategory.PermissionSet)
                        return false;

                    if (this.Locale != subSetCategory.Locale)
                        return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Clone category object
        /// </summary>
        /// <returns>cloned copy of category object.</returns>
        public ICategory Clone()
        {
            Category clonedCategory = new Category();

            clonedCategory._categoryBaseProperties.Id = this._categoryBaseProperties.Id;
            clonedCategory._categoryBaseProperties.Name = this._categoryBaseProperties.Name;
            clonedCategory._categoryBaseProperties.LongName = this._categoryBaseProperties.LongName;
            clonedCategory._categoryBaseProperties.Locale = this._categoryBaseProperties.Locale;
            clonedCategory._categoryBaseProperties.Action = this._categoryBaseProperties.Action;
            clonedCategory._categoryBaseProperties.AuditRefId = this._categoryBaseProperties.AuditRefId;
            clonedCategory._categoryBaseProperties.ExtendedProperties = this._categoryBaseProperties.ExtendedProperties;

            clonedCategory._categoryBaseProperties.HierarchyId = this._categoryBaseProperties.HierarchyId;
            clonedCategory._categoryBaseProperties.HierarchyName = this._categoryBaseProperties.HierarchyName;
            clonedCategory._categoryBaseProperties.HierarchyLongName = this._categoryBaseProperties.HierarchyLongName;
            clonedCategory._categoryBaseProperties.Level = this._categoryBaseProperties.Level;
            clonedCategory._categoryBaseProperties.Path = this._categoryBaseProperties.Path;
            clonedCategory._categoryBaseProperties.LongNamePath = this._categoryBaseProperties.LongNamePath;
            clonedCategory._categoryBaseProperties.IsLeaf = this._categoryBaseProperties.IsLeaf;
            clonedCategory._categoryBaseProperties.ParentCategoryId = this._categoryBaseProperties.ParentCategoryId;
            clonedCategory._categoryBaseProperties.ParentCategoryName = this._categoryBaseProperties.ParentCategoryName;
            clonedCategory._categoryBaseProperties.IdPath = this._categoryBaseProperties.IdPath;

            clonedCategory._categoryLocaleProperties.LongName = this._categoryLocaleProperties.LongName;
            clonedCategory._categoryLocaleProperties.HierarchyLongName = this._categoryLocaleProperties.HierarchyLongName;
            clonedCategory._categoryLocaleProperties.LongNamePath = this._categoryLocaleProperties.LongNamePath;

            #region Clone PermissionSet

            Collection<UserAction> clonedPermissionSet = new Collection<UserAction>();

            if (this._permissionSet != null)
            {
                foreach (UserAction userAction in this._permissionSet)
                {
                    clonedPermissionSet.Add(userAction);
                }
            }

            clonedCategory._permissionSet = clonedPermissionSet;

            #endregion Clone PermissionSet

            return clonedCategory;
        }

        /// <summary>
        /// Delta Merge of category
        /// </summary>
        /// <param name="deltaCategory">Category that needs to be merged.</param>
        /// <param name="iCallerContext">Indicates the name of application and the module that are performing the action</param>
        /// <param name="returnClonedObject">true means return new merged cloned object;otherwise, same object.</param>
        /// <returns>Merged category instance</returns>
        public ICategory MergeDelta(ICategory deltaCategory, ICallerContext iCallerContext, Boolean returnClonedObject = true)
        {
            ICategory mergedCategory = (returnClonedObject == true) ? deltaCategory.Clone() : deltaCategory;

            //The category action is changed to Rename if shortName is getting changed else Update. In case of Update action, DB changes will take place only if there are any attribute changes.
            //Here we do not have any attributes changed.
            //Also, any change in object will be sent with Action Rename other than ShortName change

            if (!mergedCategory.Name.Equals(this.Name))
            {
                mergedCategory.Action = ObjectAction.Rename;
            }
            else if (mergedCategory.Equals(this))
            {
                mergedCategory.Action = ObjectAction.Read;
            }
            else if (String.Compare(mergedCategory.HierarchyName, this.HierarchyName, true) == 0 && String.Compare(mergedCategory.Path, this.Path, true) == 0)
            {
                mergedCategory.Action = ObjectAction.Update;
            }

            return mergedCategory;
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

        /// <summary>
        /// Initialize current category object through Xml
        /// </summary>
        /// <param name="valuesAsXml">Xml having value for current category object</param>
        private void LoadCategory(String valuesAsXml)
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
                            #region Read Category Properties

                            if (reader.HasAttributes)
                            {
                                if (reader.MoveToAttribute("Id"))
                                {
                                    this._categoryBaseProperties.Id = ValueTypeHelper.Int64TryParse(reader.ReadContentAsString(), this.Id);
                                }
                                if (reader.MoveToAttribute("HierarchyId"))
                                {
                                    this._categoryBaseProperties.HierarchyId = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), this.HierarchyId);
                                }
                                if (reader.MoveToAttribute("HierarchyName"))
                                {
                                    this._categoryBaseProperties.HierarchyName = reader.ReadContentAsString();
                                }
                                if (reader.MoveToAttribute("HierarchyLongName"))
                                {
                                    this._categoryBaseProperties.HierarchyLongName = reader.ReadContentAsString();
                                }
                                if (reader.MoveToAttribute("Name"))
                                {
                                    this._categoryBaseProperties.Name = reader.ReadContentAsString();
                                }
                                if (reader.MoveToAttribute("LongName"))
                                {
                                    this._categoryBaseProperties.LongName = reader.ReadContentAsString();
                                }
                                if (reader.MoveToAttribute("Path"))
                                {
                                    this._categoryBaseProperties.Path = reader.ReadContentAsString();
                                }
                                if (reader.MoveToAttribute("LongNamePath"))
                                {
                                    this._categoryBaseProperties.LongNamePath = reader.ReadContentAsString();
                                }
                                if (reader.MoveToAttribute("Locale"))
                                {
                                    String strLocale = reader.GetAttribute("Locale");
                                    Core.LocaleEnum locale = LocaleEnum.UnKnown;
                                    Enum.TryParse<Core.LocaleEnum>(strLocale, out locale);
                                    this.Locale = locale;
                                }
                                if (reader.MoveToAttribute("IsLeaf"))
                                {
                                    this._categoryBaseProperties.IsLeaf = ValueTypeHelper.BooleanTryParse(reader.ReadContentAsString(), this.IsLeaf);
                                }
                                if (reader.MoveToAttribute("Level"))
                                {
                                    this._categoryBaseProperties.Level = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), this.Level);
                                }
                                if (reader.MoveToAttribute("PermissionSet"))
                                {
                                    this._categoryBaseProperties.PermissionSet = ValueTypeHelper.SplitStringToUserActionCollection(reader.ReadContentAsString(), ',');
                                }
                                if (reader.MoveToAttribute("ParentCategoryId"))
                                {
                                    this._categoryBaseProperties.ParentCategoryId = ValueTypeHelper.Int64TryParse(reader.ReadContentAsString(), this.ParentCategoryId);
                                }
                                if (reader.MoveToAttribute("Action"))
                                {                                    
                                    this.Action = ValueTypeHelper.GetAction(reader.ReadContentAsString());
                                }
                                if (reader.MoveToAttribute("ReferenceId"))
                                {
                                    this.ReferenceId = reader.ReadContentAsString();
                                }
                                if (reader.MoveToAttribute("IdPath"))
                                {
                                    this._categoryBaseProperties.IdPath = reader.ReadContentAsString();
                                }
                            }

                            #endregion Read Category Properties
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
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        private void SetOverridenProperties<T>(String key, T value)
        {
            if (!String.IsNullOrWhiteSpace(key) && value != null)
            {
                if (this._overridenProperties == null)
                    this._overridenProperties = new Dictionary<String, String>();

                if (this._overridenProperties.ContainsKey(key))
                {
                    //If user is trying to set overridden properties again then remove from dictionary and add it again.
                    this._overridenProperties.Remove(key);
                }

                this._overridenProperties.Add(key, value.ToString());
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        private T GetOverridenProperties<T>(String key)
        {
            T returnVal = default(T);

            if (!String.IsNullOrWhiteSpace(key))
            {
                returnVal = (T)Convert.ChangeType(this._overridenProperties[key], typeof(T));
            }

            return returnVal;
        }

        #endregion

        #endregion
    }
}