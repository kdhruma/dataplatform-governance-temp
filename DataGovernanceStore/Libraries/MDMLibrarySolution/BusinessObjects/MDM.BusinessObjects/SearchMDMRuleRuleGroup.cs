using System;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;

namespace MDM.BusinessObjects
{
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Represents the MDMRule rule group for export syndication profile.
    /// </summary>
    [DataContract]
    public class SearchMDMRuleRuleGroup : MDMObject, ISearchMDMRuleRuleGroup
    {
        #region Fields

        /// <summary>
        /// Field specifying export scope object id
        /// </summary>
        private Int64 _objectId = 0;

        /// <summary>
        /// Represents search operator for group.
        /// </summary>
        private ConditionalOperator _groupOperator = ConditionalOperator.NONE;

        /// <summary>
        /// Represents search operator for rule.
        /// </summary>
        private ConditionalOperator _ruleOperator = ConditionalOperator.NONE;

        /// <summary>
        /// Field specifying collection of search MDM rules rule
        /// </summary>
       private SearchMDMRuleRuleCollection _searchMDMRuleRuleCollection = new SearchMDMRuleRuleCollection();

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the Search MDM Rules Rule group class.
        /// </summary>
        public SearchMDMRuleRuleGroup()
            : base()
        {

        }

        /// <summary>
        /// Xml Constructor
        /// </summary>
        /// <param name="valuesAsXml">Xml containing value of current SearchMDMRuleRuleGroup object</param>
        public SearchMDMRuleRuleGroup(String valuesAsXml)
        {
            LoadSearchMDMRuleRuleGroup(valuesAsXml);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Property specifies MDM Rules rule group object id
        /// </summary>
        [DataMember]
        public Int64 ObjectId
        {
            get
            {
                return _objectId;
            }
            set
            {
                _objectId = value;
            }
        }

        /// <summary>
        /// Specifies the group operator for the MDMD Rules rule group.
        /// </summary>
        [DataMember]
        public ConditionalOperator GroupOperator
        {
            get
            {
                return _groupOperator;
            }
            set
            {
                _groupOperator = value;
            }
        }

        /// <summary>
        /// Specifies the rule operator for the MDM Rules rule group.
        /// </summary>
        [DataMember]
        public ConditionalOperator RuleOperator
        {
            get
            {
                return _ruleOperator;
            }
            set
            {
                _ruleOperator = value;
            }
        }

        /// <summary>
        /// Property denoting collection of search MDMRule rules.
        /// </summary>
        [DataMember]
        public SearchMDMRuleRuleCollection SearchMDMRuleRules
        {
            get
            {
                return this._searchMDMRuleRuleCollection;
            }
            set
            {
                this._searchMDMRuleRuleCollection = value;
            }
        }

        #endregion

        #region Methods

        #region Public Methods

        /// <summary>
        /// Represents SearchMDMRuleRuleGroup in Xml format
        /// </summary>
        /// <returns>String representation of current Search MDMRuleRuleGroup object</returns>
        public override String ToXml()
        {
            String searchNDNRulesRuleGroupXML = String.Empty;
            using (StringWriter sw = new StringWriter())
            {
                using (XmlTextWriter xmlWriter = new XmlTextWriter(sw))
                {

                    //SearchMDMRuleRuleGroup node start
                    xmlWriter.WriteStartElement("SearchMDMRuleRuleGroup");

                    xmlWriter.WriteAttributeString("ObjectId", this.ObjectId.ToString());
                    xmlWriter.WriteAttributeString("GroupOperator", this.GroupOperator.ToString());
                    xmlWriter.WriteAttributeString("RuleOperator", this.RuleOperator.ToString());

                    if (this._searchMDMRuleRuleCollection != null)
                        xmlWriter.WriteRaw(this._searchMDMRuleRuleCollection.ToXml());

                    //SearchMDMRuleRuleGroup node end
                    xmlWriter.WriteEndElement();

                    xmlWriter.Flush();

                    //get the actual XML
                    searchNDNRulesRuleGroupXML = sw.ToString();
                }
            }

            return searchNDNRulesRuleGroupXML;
        }

        /// <summary>
        /// Represents SearchMDMRuleRuleGroup in Xml format
        /// </summary>
        /// <param name="objectSerialization">Specifies the object serialization type</param>
        /// <returns>String representation of current SearchMDMRuleRuleGroup object</returns>
        public override String ToXml(ObjectSerialization objectSerialization)
        {
            String exportScopeXml = String.Empty;

            //TODO:: Write logic as per the different object serialization type whenever needed..
            exportScopeXml = this.ToXml();

            return exportScopeXml;
        }

        /// <summary>
        /// Determines whether two Object instances are equal.
        /// </summary>
        /// <param name="obj">The Object to compare with the current Object.</param>
        /// <returns>true if the specified Object is equal to the current Object; otherwise, false.</returns>
        public override bool Equals(object obj)
        {
            if (base.Equals(obj))
            {
                if (obj is SearchMDMRuleRuleGroup)
                {
                    SearchMDMRuleRuleGroup objectToBeCompared = obj as SearchMDMRuleRuleGroup;

                    if (!this.SearchMDMRuleRules.Equals(objectToBeCompared.SearchMDMRuleRules))
                        return false;

                    if (!this.GroupOperator.Equals(objectToBeCompared.GroupOperator))
                        return false;

                    if (!this.RuleOperator.Equals(objectToBeCompared.RuleOperator))
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
            Int32 hashCode = 0;
            hashCode = base.GetHashCode() ^ this.SearchMDMRuleRules.GetHashCode();
            return hashCode;
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// Load current SearchMDMRuleRuleCollection from Xml
        /// </summary>
        /// <param name="valueAsXml">Xml having value for current SearchMDMRuleRuleCollection
        /// </param>
        private void LoadSearchMDMRuleRuleGroup(String valueAsXml)
        {
            if (!String.IsNullOrWhiteSpace(valueAsXml))
            {
                XmlTextReader reader = null;
                try
                {
                    reader = new XmlTextReader(valueAsXml, XmlNodeType.Element, null);

                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "SearchMDMRuleRuleGroup")
                        {
                            #region Read export scope Properties

                            if (reader.HasAttributes)
                            {
                                if (reader.MoveToAttribute("ObjectId"))
                                {
                                    this.ObjectId = ValueTypeHelper.ConvertToInt64(reader.ReadContentAsString());
                                }

                                if (reader.MoveToAttribute("GroupOperator"))
                                {
                                    ConditionalOperator conditionalOperator = ConditionalOperator.NONE;
                                    Enum.TryParse<ConditionalOperator>(reader.ReadContentAsString(), out conditionalOperator);
                                    this.GroupOperator = conditionalOperator;
                                }

                                if (reader.MoveToAttribute("RuleOperator"))
                                {
                                    ConditionalOperator conditionalOperator = ConditionalOperator.NONE;
                                    Enum.TryParse<ConditionalOperator>(reader.ReadContentAsString(), out conditionalOperator);
                                    this.RuleOperator = conditionalOperator;
                                }
                            }

                            #endregion
                        }
                        else if (reader.NodeType == XmlNodeType.Element && reader.Name == "SearchMDMRuleRules")
                        {
                            #region Read search MDMRule rules collection

                            String searchMDMRulesXml = reader.ReadOuterXml();
                            if (!String.IsNullOrEmpty(searchMDMRulesXml))
                            {
                                SearchMDMRuleRuleCollection searchMDMRuleRuleCollection = new SearchMDMRuleRuleCollection(searchMDMRulesXml);
                                if (searchMDMRuleRuleCollection != null)
                                {
                                    foreach (SearchMDMRuleRule searchMDMRuleRule in searchMDMRuleRuleCollection)
                                    {
                                        if (!this.SearchMDMRuleRules.Contains(searchMDMRuleRule))
                                        {
                                            this.SearchMDMRuleRules.Add(searchMDMRuleRule);
                                        }
                                    }
                                }
                            }

                            #endregion Read search MDMRule rules collection
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
