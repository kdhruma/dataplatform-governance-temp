using System;
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
    [KnownType(typeof(CategorySearchRule))]
    [KnownType(typeof(CategorySearchRuleCollection))]
    [KnownType(typeof(CategoryField))]
    public class CategoryContext : ObjectBase, ICategoryContext
    {
        #region Fields

        /// <summary>
        /// The hierarchy Id
        /// </summary>
        private Int32 _hierarchyId = -1;

        /// <summary>
        /// Field denotes identifier of container
        /// </summary>
        private Int32 _containerId = 0;

        /// <summary>
        /// Container Name in which the category belongs
        /// </summary>
        private String _containerName;

        /// <summary>
        /// Flag for loading child categories or not
        /// </summary>
        private Boolean _loadRecursive = false;

        /// <summary>
        /// SearchRules for Category
        /// </summary>
        private CategorySearchRuleCollection _categorySearchRules = null;

        /// <summary>
        /// Field on which result should be filtered
        /// </summary>
        private CategoryField _orderByField = CategoryField.Id;

        /// <summary>
        /// Maximum records to return in search result
        /// </summary>
        private Int32 _maxRecordsToReturns = 0;

        /// <summary>
        /// Index denoting from where to return result
        /// </summary>
        private Int32 _startIndex = 0;

        /// <summary>
        /// Index denoting till what to return in result
        /// </summary>
        private Int64 _endIndex = 0;

        /// <summary>
        /// DataLocales in which result has to be returned
        /// </summary>
        private Collection<LocaleEnum> _locales = null;

        /// <summary>
        /// Flag denoting whether to apply security on search result or not
        /// </summary>
        private Boolean _applySecurity = false;

        /// <summary>
        /// Field denoting whether to load parent categories recursive or not
        /// </summary>
        private Boolean _loadParentRecursive = false;

        /// <summary>
        /// Field denoting whether to load leaf level categories
        /// </summary>
        private Boolean _loadOnlyLeafCategories = false;

        #endregion

        #region Constructor

        /// <summary>
        /// Parameter less constructor
        /// </summary>
        public CategoryContext() { }

        /// <summary>
        /// Initializes a new instance of the CategoryContext class.
        /// </summary>
        /// <param name="valuesAsXml">CategoryContext Object in XML representation</param>
        public CategoryContext(String valuesAsXml)
        {
            LoadCategoryContext(valuesAsXml);
        }

        #endregion

        #region Properties

        /// <summary>
        /// The hierarchy Id
        /// </summary>
        [DataMember]
        public Int32 HierarchyId
        {
            get { return this._hierarchyId; }
            set { this._hierarchyId = value; }
        }

        /// <summary>
        /// Indicates container identifier of category context
        /// </summary>
        [DataMember]
        public Int32 ContainerId
        {
            get { return this._containerId; }
            set { this._containerId = value; }
        }

        /// <summary>
        /// Container Name in which the category belongs
        /// </summary>
        [DataMember]
        public String ContainerName
        {
            get { return this._containerName; }
            set { this._containerName = value; }
        }

        /// <summary>
        /// Flag for loading child categories or not
        /// </summary>
        [DataMember]
        public Boolean LoadRecursive
        {
            get { return this._loadRecursive; }
            set { this._loadRecursive = value; }
        }

        /// <summary>
        /// SearchRules for Category
        /// </summary>
        [DataMember]
        public CategorySearchRuleCollection CategorySearchRules
        {
            get { return this._categorySearchRules; }
            set { this._categorySearchRules = value; }
        }

        /// <summary>
        /// Field on which result should be filtered
        /// </summary>
        [DataMember]
        public CategoryField OrderByField
        {
            get { return this._orderByField; }
            set { this._orderByField = value; }
        }

        /// <summary>
        /// Maximum records to return in search result
        /// </summary>
        [DataMember]
        public Int32 MaxRecordsToReturn
        {
            get { return this._maxRecordsToReturns; }
            set { this._maxRecordsToReturns = value; }
        }

        /// <summary>
        /// Index denoting from where to return result
        /// </summary>
        [DataMember]
        public Int32 StartIndex
        {
            get { return this._startIndex; }
            set { this._startIndex = value; }
        }

        /// <summary>
        /// Index denoting till what to return in result
        /// </summary>
        [DataMember]
        public Int64 EndIndex
        {
            get { return this._endIndex; }
            set { this._endIndex = value; }
        }

        /// <summary>
        /// DataLocales in which result has to be returned
        /// </summary>
        [DataMember]
        public Collection<LocaleEnum> Locales
        {
            get { return this._locales; }
            set { this._locales = value; }
        }

        /// <summary>
        /// Flag denoting whether to apply security on search result or not
        /// </summary>
        [DataMember]
        public Boolean ApplySecurity
        {
            get { return this._applySecurity; }
            set { this._applySecurity = value; }
        }

        /// <summary>
        /// Field denoting whether to load parent categories recursive or not
        /// </summary>
        [DataMember]
        public Boolean LoadParentRecursive
        {
            get { return this._loadParentRecursive; }
            set { this._loadParentRecursive = value; }
        }

        /// <summary>
        /// Field denoting whether to load only leaf categories
        /// </summary>
        [DataMember]
        public Boolean LoadOnlyLeafCategories
        {
            get { return this._loadOnlyLeafCategories; }
            set { this._loadOnlyLeafCategories = value; }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Get Xml representation of CategoryContext
        /// </summary>
        /// <returns>Xml representation of CategoryContext</returns>
        public String ToXml()
        {
            String categoryXml = String.Empty;

            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            //Parameter node start
            xmlWriter.WriteStartElement("CategoryContext");

            xmlWriter.WriteAttributeString("HierarchyId", this.HierarchyId.ToString());
            xmlWriter.WriteAttributeString("ContainerId", this.ContainerId.ToString());
            xmlWriter.WriteAttributeString("LoadRecursive", this.LoadRecursive.ToString());
            xmlWriter.WriteAttributeString("LoadParentRecursive", this.LoadRecursive.ToString());
            xmlWriter.WriteAttributeString("Locales", ValueTypeHelper.JoinCollection(this.Locales, ","));
            xmlWriter.WriteAttributeString("MaxRecordsToReturn", this.MaxRecordsToReturn.ToString());
            xmlWriter.WriteAttributeString("OrderByField", this.OrderByField.ToString());
            xmlWriter.WriteAttributeString("StartIndex", this.StartIndex.ToString());
            xmlWriter.WriteAttributeString("EndIndex", this.EndIndex.ToString());
            xmlWriter.WriteAttributeString("LoadOnlyLeafCategories", this.LoadOnlyLeafCategories.ToString());

            xmlWriter.WriteStartElement("CategorySearchRules");

            if (this.CategorySearchRules != null)
            {
                xmlWriter.WriteRaw(this.CategorySearchRules.ToXml());
            }

            xmlWriter.WriteEndElement();

            //Param node end
            xmlWriter.WriteEndElement();

            xmlWriter.Flush();

            //Get the actual XML
            categoryXml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return categoryXml;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Get Instance from CatgoryContext XML
        /// </summary>
        /// <param name="valuesAsXml">Category Context in XML Representation</param>
        private void LoadCategoryContext(String valuesAsXml)
        {
            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                XmlTextReader reader = null;
                try
                {
                    reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "CategoryContext")
                        {
                            #region Read Category Properties

                            if (reader.HasAttributes)
                            {
                                if (reader.MoveToAttribute("HierarchyId"))
                                {
                                    this.HierarchyId = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), this.HierarchyId);
                                }
                                if (reader.MoveToAttribute("ContainerId"))
                                {
                                    this.ContainerId = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), this.ContainerId);
                                }
                                if (reader.MoveToAttribute("LoadRecursive"))
                                {
                                    this.LoadRecursive = ValueTypeHelper.BooleanTryParse(reader.ReadContentAsString(), this.LoadRecursive);
                                }
                                if (reader.MoveToAttribute("LoadParentRecursive"))
                                {
                                    this.LoadParentRecursive = ValueTypeHelper.BooleanTryParse(reader.ReadContentAsString(), this.LoadParentRecursive);
                                }
                                if (reader.MoveToAttribute("Locales"))
                                {
                                    String strLocale = reader.GetAttribute("Locales");
                                    Collection<LocaleEnum> locales = ValueTypeHelper.SplitStringToLocaleEnumCollection(strLocale, ',');
                                    this.Locales = locales;
                                }
                                if (reader.MoveToAttribute("MaxRecordsToReturn"))
                                {
                                    this.MaxRecordsToReturn = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), this.MaxRecordsToReturn);
                                }
                                if (reader.MoveToAttribute("OrderByField"))
                                {
                                    String strOrderByField = reader.GetAttribute("OrderByField");
                                    CategoryField categoryField = CategoryField.Id;
                                    Enum.TryParse<CategoryField>(strOrderByField, out categoryField);
                                    this.OrderByField = categoryField;
                                }
                                if (reader.MoveToAttribute("StartIndex"))
                                {
                                    this.StartIndex = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), this.StartIndex);
                                }
                                if (reader.MoveToAttribute("EndIndex"))
                                {
                                    this.EndIndex = ValueTypeHelper.Int64TryParse(reader.ReadContentAsString(), this.EndIndex);
                                }
                                if (reader.MoveToAttribute("LoadOnlyLeafCategories"))
                                {
                                    this.LoadOnlyLeafCategories = ValueTypeHelper.BooleanTryParse(reader.ReadContentAsString(), this.LoadOnlyLeafCategories);
                                }
                            }

                            #endregion Read Category Properties
                        }
                        else if (reader.NodeType == XmlNodeType.Element && reader.Name == "CategorySearchRules")
                        {
                            String categorySearchRulesXml = reader.ReadOuterXml();
                            if (!String.IsNullOrEmpty(categorySearchRulesXml))
                            {
                                this.CategorySearchRules = new CategorySearchRuleCollection(categorySearchRulesXml);
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

        #endregion
    }
}