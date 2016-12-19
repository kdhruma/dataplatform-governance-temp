using System;
using System.Runtime.Serialization;
using System.IO;
using System.Xml;

namespace MDM.BusinessObjects
{
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Class for CategorySearchRule Object
    /// </summary>
    [DataContract]
    public class CategorySearchRule : ObjectBase, ICategorySearchRule
    {
        #region Fields

        /// <summary>
        /// Property of Category on which search should happened
        /// </summary>
        private CategoryField _categoryField = CategoryField.Id;

        /// <summary>
        /// The Search Operator 
        /// </summary>
        private SearchOperator _searchOperator = SearchOperator.None;

        /// <summary>
        /// Value that needs to be searched.
        /// </summary>
        private String _searchValue = String.Empty;

        /// <summary>
        /// The value separator
        /// </summary>
        private String _valueSeperator = ",";

        #endregion

        #region Constructors

        /// <summary>
        /// Parameter less constructor
        /// </summary>
        public CategorySearchRule()
        {
        }

        /// <summary>
        /// Initialize instance with different properties
        /// </summary>
        /// <param name="categoryField">Field of category object to perform search on</param>
        /// <param name="searchOperator">Search Operator</param>
        /// <param name="searchValue">Value to be searched</param>
        public CategorySearchRule(CategoryField categoryField, SearchOperator searchOperator, String searchValue)
        {
            this.CategoryField = categoryField;
            this.SearchOperator = searchOperator;
            this.SearchValue = searchValue;
        }

        /// <summary>
        /// Load CategorySearchRule instance for XML
        /// </summary>
        /// <param name="valuesAsXml">XML representation of Object</param>
        public CategorySearchRule(String valuesAsXml)
        {
            LoadCategorySearchRule(valuesAsXml);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Property of Category on which search should happened
        /// </summary>
        [DataMember]
        public CategoryField CategoryField
        {
            get { return this._categoryField; }
            set { this._categoryField = value; }
        }

        /// <summary>
        /// The Search Operator 
        /// </summary>
        [DataMember]
        public SearchOperator SearchOperator
        {
            get { return this._searchOperator; }
            set { this._searchOperator = value; }
        }

        /// <summary>
        /// Value that needs to be searched.
        /// </summary>
        [DataMember]
        public String SearchValue
        {
            get { return this._searchValue; }
            set { this._searchValue = value; }
        }

        /// <summary>
        /// The value separator
        /// </summary>
        [DataMember]
        public String ValueSeparator
        {
            get { return this._valueSeperator; }
            set { this._valueSeperator = value; }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Get Xml representation of CategorySearchRule
        /// </summary>
        /// <returns>Xml representation of CategorySearchRule</returns>
        public String ToXml()
        {
            String categorySearchRuleXml = String.Empty;

            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            //Parameter node start
            xmlWriter.WriteStartElement("CategorySearchRule");

            xmlWriter.WriteAttributeString("CategoryField", this.CategoryField.ToString());
            xmlWriter.WriteAttributeString("SearchOperator", this.SearchOperator.ToString());
            xmlWriter.WriteAttributeString("SearchValue", this.SearchValue);

            //Param node end
            xmlWriter.WriteEndElement();

            xmlWriter.Flush();

            //Get the actual XML
            categorySearchRuleXml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return categorySearchRuleXml;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Load CategorySearchRule instance for XML
        /// </summary>
        /// <param name="valuesAsXml">XML representation of Object</param>
        private void LoadCategorySearchRule(String valuesAsXml)
        {
            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                XmlTextReader reader = null;
                try
                {
                    reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "CategorySearchRule")
                        {
                            #region Read Category Properties

                            if (reader.HasAttributes)
                            {
                                if (reader.MoveToAttribute("CategoryField"))
                                {
                                    String strcategoryField = reader.GetAttribute("CategoryField");
                                    CategoryField categoryField = CategoryField.Id;
                                    Enum.TryParse<CategoryField>(strcategoryField, out categoryField);
                                    this.CategoryField = categoryField;
                                }
                                if (reader.MoveToAttribute("SearchOperator"))
                                {
                                    String strSearchOperator = reader.GetAttribute("SearchOperator");
                                    SearchOperator searchOperator = SearchOperator.None;
                                    Enum.TryParse<SearchOperator>(strSearchOperator, out searchOperator);
                                    this.SearchOperator = searchOperator;
                                }
                                if (reader.MoveToAttribute("SearchValue"))
                                {
                                    this.SearchValue = reader.ReadContentAsString();
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

        #endregion
    }
}
