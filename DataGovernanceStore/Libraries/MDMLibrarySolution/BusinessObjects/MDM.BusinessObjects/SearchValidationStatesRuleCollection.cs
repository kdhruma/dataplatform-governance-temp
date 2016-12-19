using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.ObjectModel;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;

namespace MDM.BusinessObjects
{
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Specifies the specifications for ValidationStatesRules for Search 
    /// </summary>
    public class SearchValidationStatesRuleCollection : InterfaceContractCollection<ISearchValidationStatesRule, SearchValidationStatesRule>, ISearchXml 
    {
        #region Fields
 
        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the Search ValidationStates Rule class.
        /// </summary>
        public SearchValidationStatesRuleCollection()
            : base()
        {

        }

        /// <summary>
        /// Xml Constructor
        /// </summary>
        /// <param name="valuesAsXml">Xml containing value of current SearchValidationStatesRuleCollection object</param>
        public SearchValidationStatesRuleCollection(String valuesAsXml)
        {
            LoadSearchValidationStatesRules(valuesAsXml);
        }

        /// <summary>
        /// Copy Constructor
        /// </summary>
        /// <param name="source">SearchValidationStatesRuleCollection to be copied</param>
        public SearchValidationStatesRuleCollection(SearchValidationStatesRuleCollection source)
            : base()
        {
            foreach (SearchValidationStatesRule searchValidationStatesRule in source)
            {
                this.Add((SearchValidationStatesRule)searchValidationStatesRule);
            }
        }

        #endregion

        #region Properties
        #endregion

        #region Methods

        #region ISearchValidationStatesRuleCollection

        /// <summary>
        /// Get the XML presentation of an SearchValidationStatesRuleCollection
        /// </summary>
        /// <returns>Xml representation of Search ValidationStates Rule</returns>
        /// <exception cref="ArgumentNullException">Thrown when SearchValidationStatesRuleCollection object passed is null</exception>
        public String ToXml()
        {
            if (this._items == null)
            {
                throw new ArgumentNullException("SearchValidationStatesRuleCollection");
            }
            var resultXML = String.Empty;
            StringWriter searchValidationStatesXML = new StringWriter();
            using (XmlTextWriter xmlWriter = new XmlTextWriter(searchValidationStatesXML))
            {
                xmlWriter.WriteStartElement("SearchValidationStatesRules");

                foreach (SearchValidationStatesRule searchValidationStatesRule in this._items)
                {
                    xmlWriter.WriteRaw(searchValidationStatesRule.ToXml()); 
                }
                xmlWriter.WriteEndElement();

                resultXML = searchValidationStatesXML.ToString();

                xmlWriter.Flush(); 
            }
            return resultXML;
        }

        /// <summary>
        /// Get the search rule for a given system attribute from collection
        /// </summary>
        /// <param name="attributeId"></param>
        /// <returns></returns>
        public SearchValidationStatesRule GetSearchValidationStatesRule(SystemAttributes attributeId)
        {

            foreach (SearchValidationStatesRule searchValidationRule in this._items)
            {
                if (searchValidationRule.AttributeId == attributeId)
                {
                    return searchValidationRule;
                }
            }

            return null;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Load current SearchValidationStatesRuleCollection from Xml
        /// </summary>
        /// <param name="valueAsXml">Xml having value for current SearchValidationStatesRuleCollection
        /// <para>
        /// Sample Xml
        /// <![CDATA[
        ///    <SearchValidationStatesCollection>
        ///     <SearchValidationStatesRule IsCategoryAttributesValid="" IsCommonAttributesValid=""  Operator="And"/>           
        ///    </SearchValidationStatesCollection>
        /// ]]>
        /// </para>
        /// </param>
        private void LoadSearchValidationStatesRules(String valueAsXml)
        {
            #region Sample XML

            //<SearchValidationStatesCollection>
            //  <SearchValidationStatesRule IsCategoryAttributesValid="" IsCommonAttributesValid=""  Operator="And"/>         
            //</SearchValidationStatesCollection>

            #endregion

            if (!String.IsNullOrWhiteSpace(valueAsXml))
            {
                XmlTextReader reader = null;
                try
                {
                    reader = new XmlTextReader(valueAsXml, XmlNodeType.Element, null);

                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && (reader.Name == "SearchValidationStatesRule"))
                        {
                            String searchValidationStatesRulesXml = reader.ReadOuterXml();

                            if (!String.IsNullOrEmpty(searchValidationStatesRulesXml))
                            {
                                SearchValidationStatesRule searchValidationStatesRule = new SearchValidationStatesRule(searchValidationStatesRulesXml);

                                if (!_items.Contains(searchValidationStatesRule))
                                {
                                    this.Add(searchValidationStatesRule);
                                }
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

        #endregion

        #endregion
    }
}