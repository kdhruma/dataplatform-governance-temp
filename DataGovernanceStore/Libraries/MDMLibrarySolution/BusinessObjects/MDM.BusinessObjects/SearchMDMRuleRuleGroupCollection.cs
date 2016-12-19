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
    /// Specifies the specifications for ValidaitonStates Rules for export syndication profile. 
    /// </summary>
    [DataContract]
    public class SearchMDMRuleRuleGroupCollection : InterfaceContractCollection<ISearchMDMRuleRuleGroup, SearchMDMRuleRuleGroup>, ISearchXml 
    {
        #region Fields
         
        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the Search MDMRule Rule Group class.
        /// </summary>
        public SearchMDMRuleRuleGroupCollection(): base()
        {

        }

        /// <summary>
        /// Xml Constructor
        /// </summary>
        /// <param name="valuesAsXml">Xml containing value of current SearchMDMRuleRuleGroupCollection object</param>
        public SearchMDMRuleRuleGroupCollection(String valuesAsXml)
        {
            LoadSearchMDMRuleRuleGroups(valuesAsXml);
        }

        #endregion

        #region Properties
        #endregion

        #region Methods

        #region ISearchMDMRuleRuleGroupCollection

        /// <summary>
        /// Get the XML presentation of an SearchMDMRuleRuleGroupCollection
        /// </summary>
        /// <returns>Xml representation of Search MDMRule Rule</returns>
        /// <exception cref="ArgumentNullException">Thrown when SearchMDMRuleRuleGroupCollection object passed is null</exception>
        public String ToXml()
        {
            if (this._items == null)
                throw new ArgumentNullException("SearchMDMRuleRuleGroupCollection");

            String searchMDMRuleRuleGroupXml = String.Empty;

            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            xmlWriter.WriteStartElement("SearchMDMRuleRuleGroups");

            foreach (SearchMDMRuleRuleGroup searchMDMRuleRuleGroup in this._items)
            {
                searchMDMRuleRuleGroupXml = String.Concat(searchMDMRuleRuleGroupXml, searchMDMRuleRuleGroup.ToXml());
            }
            xmlWriter.WriteRaw(searchMDMRuleRuleGroupXml);
            xmlWriter.WriteEndElement();
            xmlWriter.Flush();

            //Get the actual XML
            searchMDMRuleRuleGroupXml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return searchMDMRuleRuleGroupXml;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Load current SearchMDMRuleRuleGroupCollection from Xml
        /// </summary>
        /// <param name="valueAsXml">Xml having value for current SearchMDMRuleRuleGroupCollection
        /// </param>
        private void LoadSearchMDMRuleRuleGroups(String valueAsXml)
        {
            if (!String.IsNullOrWhiteSpace(valueAsXml))
            {
                XmlTextReader reader = null;
                try
                {
                    reader = new XmlTextReader(valueAsXml, XmlNodeType.Element, null);

                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "SearchMDMRuleRuleGroups")
                        {
                            String searchMDMRuleRuleGroupsXml = reader.ReadOuterXml();

                            if (!String.IsNullOrEmpty(searchMDMRuleRuleGroupsXml))
                            {
                                SearchMDMRuleRuleGroup searchMDMRuleRuleGroup = new SearchMDMRuleRuleGroup(searchMDMRuleRuleGroupsXml);

                                if (searchMDMRuleRuleGroup != null)
                                {
                                    this.Add(searchMDMRuleRuleGroup);
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