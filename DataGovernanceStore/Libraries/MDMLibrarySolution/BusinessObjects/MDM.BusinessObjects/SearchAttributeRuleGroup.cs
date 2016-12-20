using System;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;

namespace MDM.BusinessObjects
{
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Represents the attribute rule group for export syndication profile.
    /// </summary>
    [DataContract]
    public class SearchAttributeRuleGroup : MDMObject, ISearchAttributeRuleGroup
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
        /// Field specifying collection of search attribute rule
        /// </summary>
        [DataMember]
        private SearchAttributeRuleCollection _searchAttributeRuleCollection = new SearchAttributeRuleCollection();

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the Search Attribute Rule group class.
        /// </summary>
        public SearchAttributeRuleGroup()
            : base()
        {

        }

        /// <summary>
        /// Xml Constructor
        /// </summary>
        /// <param name="valuesAsXml">Xml containing value of current SearchAttributeRuleGroup object</param>
        public SearchAttributeRuleGroup(String valuesAsXml)
        {
            LoadSearchAttributeRuleGroup(valuesAsXml);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Property specifies attribute rule group object id
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
        /// Specifies the group operator for the attribute rule group.
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
        /// Specifies the rule operator for the attribute rule group.
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
        /// Property denoting collection of search attribute rules.
        /// </summary>
        [DataMember]
        public SearchAttributeRuleCollection SearchAttributeRules
        {
            get
            {
                return this._searchAttributeRuleCollection;
            }
            set
            {
                this._searchAttributeRuleCollection = value;
            }
        }

        #endregion

        #region Methods

        #region Public Methods

        /// <summary>
        /// Represents SearchAttributeRuleGroup in Xml format
        /// </summary>
        /// <returns>String representation of current SearchAttributeRuleGroup object</returns>
        public override String ToXml()
        {
            String searchAttributeRuleGroupXML = String.Empty;
            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            //SearchAttributeRuleGroup node start
            xmlWriter.WriteStartElement("SearchAttributeRuleGroup");

            xmlWriter.WriteAttributeString("ObjectId", this.ObjectId.ToString());
            xmlWriter.WriteAttributeString("GroupOperator", this.GroupOperator.ToString());
            xmlWriter.WriteAttributeString("RuleOperator", this.RuleOperator.ToString());

            if (this._searchAttributeRuleCollection != null)
                xmlWriter.WriteRaw(this._searchAttributeRuleCollection.ToXml());

            //SearchAttributeRuleGroup node end
            xmlWriter.WriteEndElement();

            xmlWriter.Flush();

            //get the actual XML
            searchAttributeRuleGroupXML = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return searchAttributeRuleGroupXML;
        }

        /// <summary>
        /// Represents SearchAttributeRuleGroup in Xml format
        /// </summary>
        /// <param name="objectSerialization">Specifies the object serialization type</param>
        /// <returns>String representation of current SearchAttributeRuleGroup object</returns>
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
                if (obj is SearchAttributeRuleGroup)
                {
                    SearchAttributeRuleGroup objectToBeCompared = obj as SearchAttributeRuleGroup;

                    if (!this.SearchAttributeRules.Equals(objectToBeCompared.SearchAttributeRules))
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
            hashCode = base.GetHashCode() ^ this.SearchAttributeRules.GetHashCode();
            return hashCode;
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// Load current SearchAttributeRuleCollection from Xml
        /// </summary>
        /// <param name="valueAsXml">Xml having value for current SearchAttributeRuleCollection
        /// </param>
        private void LoadSearchAttributeRuleGroup(String valueAsXml)
        {
            if (!String.IsNullOrWhiteSpace(valueAsXml))
            {
                XmlTextReader reader = null;
                try
                {
                    reader = new XmlTextReader(valueAsXml, XmlNodeType.Element, null);

                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "SearchAttributeRuleGroup")
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
                        else if (reader.NodeType == XmlNodeType.Element && reader.Name == "SearchAttributeRules")
                        {
                            #region Read search attribute rules collection

                            String searchAttributesXml = reader.ReadOuterXml();
                            if (!String.IsNullOrEmpty(searchAttributesXml))
                            {
                                SearchAttributeRuleCollection searchAttributeRuleCollection = new SearchAttributeRuleCollection(searchAttributesXml);
                                if (searchAttributeRuleCollection != null)
                                {
                                    foreach (SearchAttributeRule searchAttributeRule in searchAttributeRuleCollection)
                                    {
                                        if (!this.SearchAttributeRules.Contains(searchAttributeRule))
                                        {
                                            this.SearchAttributeRules.Add(searchAttributeRule);
                                        }
                                    }
                                }
                            }

                            #endregion Read search attribute rules collection
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
