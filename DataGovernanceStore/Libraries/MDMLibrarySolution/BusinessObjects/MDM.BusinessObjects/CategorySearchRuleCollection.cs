using System;
using System.IO;
using System.Xml;
using System.Runtime.Serialization;

namespace MDM.BusinessObjects
{
    using MDM.Core;
    using MDM.Interfaces;
    
    /// <summary>
    /// Class for CategorySearchRule Collection instance
    /// </summary>
    [DataContract]
    public class CategorySearchRuleCollection : InterfaceContractCollection<ICategorySearchRule, CategorySearchRule>, ICategorySearchRuleCollection
    {
        #region Constructor

        /// <summary>
        /// Parameter less constructor
        /// </summary>
        public CategorySearchRuleCollection() { }

        /// <summary>
        /// Load CategorySearchRule instance from XML
        /// </summary>
        /// <param name="valuesAsXml">XML representation of Object</param>
        public CategorySearchRuleCollection(String valuesAsXml)
        {
            LoadCategorySearchRuleCollection(valuesAsXml);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Get Xml representation of CategorySearchRuleCollection
        /// </summary>
        /// <returns>Xml representation of CategorySearchRuleCollection</returns>
        public String ToXml()
        {
            String categoryXml = String.Empty;

            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            //Parameter node start
            xmlWriter.WriteStartElement("CategorySearchRules");

            if (this._items != null && this._items.Count > 0)
            {
                foreach (CategorySearchRule item in this._items)
                {
                    xmlWriter.WriteRaw(item.ToXml());
                }
            }

            //Param node end
            xmlWriter.WriteEndElement();

            xmlWriter.Flush();

            //Get the actual XML
            categoryXml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return categoryXml;
        }

        /// <summary>
        /// Add CategorySearchRule in collection
        /// </summary>
        /// <param name="item">CategorySearchRule to add in collection</param>
        public new void Add(ICategorySearchRule item)
        {
            this._items.Add((CategorySearchRule)item);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Load CategorySearchRule instance from XML
        /// </summary>
        /// <param name="valuesAsXml">XML representation of Object</param>
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
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "CategorySearchRules")
                        {
                            String categorySearchRulesXml = reader.ReadOuterXml();
                            if (!String.IsNullOrEmpty(categorySearchRulesXml))
                            {
                                CategorySearchRule categorySearchRule = new CategorySearchRule(categorySearchRulesXml);

                                if (categorySearchRule != null)
                                {
                                    this.Add(categorySearchRule);
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

        #endregion
    }
}
