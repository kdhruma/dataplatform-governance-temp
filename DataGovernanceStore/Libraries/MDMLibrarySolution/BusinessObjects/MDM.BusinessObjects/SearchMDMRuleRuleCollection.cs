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
    /// Specifies the specifications for MDMRuleRules for Search 
    /// </summary>
    public class SearchMDMRuleRuleCollection : InterfaceContractCollection<ISearchMDMRuleRule, SearchMDMRuleRule>, ISearchXml
    {
        #region Fields

       
        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the Search MDMRule Rule class.
        /// </summary>
        public SearchMDMRuleRuleCollection()
            : base()
        {

        }

        /// <summary>
        /// Xml Constructor
        /// </summary>
        /// <param name="valuesAsXml">Xml containing value of current SearchMDMRuleRuleCollection object</param>
        public SearchMDMRuleRuleCollection(String valuesAsXml)
        {
            LoadSearchMDMRuleRules(valuesAsXml);
        }

        /// <summary>
        /// Copy Constructor
        /// </summary>
        /// <param name="source">SearchMDMRuleRuleCollection to be copied</param>
        public SearchMDMRuleRuleCollection(SearchMDMRuleRuleCollection source)
            : base()
        {
            foreach (SearchMDMRuleRule searchMDMRuleRule in source)
            {
                this.Add((SearchMDMRuleRule)searchMDMRuleRule);
            }
        }

        #endregion

        #region Properties
        #endregion

        #region Methods

        #region Public 
        /// <summary>
        /// Get the Rule given a rule id.
        /// </summary>
        /// <param name="ruleId"></param>
        /// <returns></returns>
        public SearchMDMRuleRule GetSearchMDMRuleRule(Int32 ruleId)
        {
            foreach (SearchMDMRuleRule rule in this._items)
            {
                if (rule.MDMRule!=null & rule.MDMRule.Id == ruleId)
                {
                    return rule;
                }
            }
            return null;
        }

        #endregion

        #region ISearchMDMRuleRuleCollection

        /// <summary>
        /// Get the XML presentation of an SearchMDMRuleRuleCollection
        /// </summary>
        /// <returns>Xml representation of Search MDMRule Rule</returns>
        /// <exception cref="ArgumentNullException">Thrown when SearchMDMRuleRuleCollection object passed is null</exception>
        public String ToXml()
        {
            if (this._items == null)
            {
                throw new ArgumentNullException("SearchMDMRuleRuleCollection");
            }

            String searchMDMRuleRuleXml = String.Empty;

            using (StringWriter sw = new StringWriter())
            {
                using (XmlTextWriter xmlWriter = new XmlTextWriter(sw))
                {

                    xmlWriter.WriteStartElement("SearchMDMRuleRules");

                    foreach (SearchMDMRuleRule searchMDMRuleRule in this._items)
                    {
                        searchMDMRuleRuleXml = String.Concat(searchMDMRuleRuleXml, searchMDMRuleRule.ToXml());
                    }
                    xmlWriter.WriteRaw(searchMDMRuleRuleXml);
                    xmlWriter.WriteEndElement();
                    xmlWriter.Flush();

                    //Get the actual XML
                    searchMDMRuleRuleXml = sw.ToString();
                }
            }

            return searchMDMRuleRuleXml;
        }

        /// <summary>
        /// Get All MDMRule Ids
        /// </summary>
        /// <returns>MDMRule Ids</returns>
        public Collection<Int32> GetAllMDMRuleIds()
        {
            Collection<Int32> MDMRuleIds = null;

            if (this._items != null)
            {
                MDMRuleIds = new Collection<Int32>();

                foreach (SearchMDMRuleRule rule in this._items)
                {
                    if (rule.MDMRule != null && !MDMRuleIds.Contains(rule.MDMRule.Id))
                    {
                        MDMRuleIds.Add(rule.MDMRule.Id);
                    }
                }
            }

            return MDMRuleIds;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Load current SearchMDMRuleRuleCollection from Xml
        /// </summary>
        /// <param name="valueAsXml">Xml having value for current SearchMDMRuleRuleCollection
        /// <para>
        /// Sample Xml
        /// <![CDATA[
        ///    <SearchMDMRuleCollection>
        ///     <SearchMDMRuleRule IsCategoryAttributesValid="" IsCommonAttributesValid=""  Operator="And"/>           
        ///    </SearchMDMRuleCollection>
        /// ]]>
        /// </para>
        /// </param>
        private void LoadSearchMDMRuleRules(String valueAsXml)
        {
            #region Sample XML

            //<SearchMDMRuleCollection>
            //  <SearchMDMRuleRule IsCategoryAttributesValid="" IsCommonAttributesValid=""  Operator="And"/>         
            //</SearchMDMRuleCollection>

            #endregion

            if (!String.IsNullOrWhiteSpace(valueAsXml))
            {
                XmlTextReader reader = null;
                try
                {
                    reader = new XmlTextReader(valueAsXml, XmlNodeType.Element, null);

                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && (reader.Name == "SearchMDMRuleRule"))
                        {
                            String searchMDMRuleRulesXml = reader.ReadOuterXml();

                            if (!String.IsNullOrEmpty(searchMDMRuleRulesXml))
                            {
                                SearchMDMRuleRule searchMDMRuleRule = new SearchMDMRuleRule(searchMDMRuleRulesXml);

                                if (searchMDMRuleRule.MDMRule != null && !_items.Contains(searchMDMRuleRule))
                                {
                                    this.Add(searchMDMRuleRule);
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