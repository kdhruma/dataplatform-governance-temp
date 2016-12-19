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
    public class SearchValidationStatesRuleGroupCollection : InterfaceContractCollection<ISearchValidationStatesRuleGroup, SearchValidationStatesRuleGroup>, ISearchXml     {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the Search ValidationStates Rule Group class.
        /// </summary>
        public SearchValidationStatesRuleGroupCollection(): base()
        {

        }

        /// <summary>
        /// Xml Constructor
        /// </summary>
        /// <param name="valuesAsXml">Xml containing value of current SearchValidationStatesRuleGroupCollection object</param>
        public SearchValidationStatesRuleGroupCollection(String valuesAsXml)
        {
            LoadSearchValidationStatesRuleGroups(valuesAsXml);
        }

        #endregion

        #region Properties
        #endregion

        #region Methods

        #region ISearchValidationStatesRuleGroupCollection

        /// <summary>
        /// Get the XML presentation of an SearchValidationStatesRuleGroupCollection
        /// </summary>
        /// <returns>Xml representation of Search ValidationStates Rule</returns>
        /// <exception cref="ArgumentNullException">Thrown when SearchValidationStatesRuleGroupCollection object passed is null</exception>
        public String ToXml()
        {
            if (this._items == null)
                throw new ArgumentNullException("SearchValidationStatesRuleGroupCollection");

            String searchValidationStatesRuleGroupXml = String.Empty;

            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            xmlWriter.WriteStartElement("SearchValidationStatesRuleGroups");

            foreach (SearchValidationStatesRuleGroup searchValidationStatesRuleGroup in this._items)
            {
                searchValidationStatesRuleGroupXml = String.Concat(searchValidationStatesRuleGroupXml, searchValidationStatesRuleGroup.ToXml());
            }
            xmlWriter.WriteRaw(searchValidationStatesRuleGroupXml);
            xmlWriter.WriteEndElement();
            xmlWriter.Flush();

            //Get the actual XML
            searchValidationStatesRuleGroupXml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return searchValidationStatesRuleGroupXml;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Load current SearchValidationStatesRuleGroupCollection from Xml
        /// </summary>
        /// <param name="valueAsXml">Xml having value for current SearchValidationStatesRuleGroupCollection
        /// </param>
        private void LoadSearchValidationStatesRuleGroups(String valueAsXml)
        {
            if (!String.IsNullOrWhiteSpace(valueAsXml))
            {
                XmlTextReader reader = null;
                try
                {
                    reader = new XmlTextReader(valueAsXml, XmlNodeType.Element, null);

                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "SearchValidationStatesRuleGroups")
                        {
                            String searchValidationStatesRuleGroupsXml = reader.ReadOuterXml();

                            if (!String.IsNullOrEmpty(searchValidationStatesRuleGroupsXml))
                            {
                                SearchValidationStatesRuleGroup searchValidationStatesRuleGroup = new SearchValidationStatesRuleGroup(searchValidationStatesRuleGroupsXml);

                                if (searchValidationStatesRuleGroup != null)
                                {
                                    this.Add(searchValidationStatesRuleGroup);
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