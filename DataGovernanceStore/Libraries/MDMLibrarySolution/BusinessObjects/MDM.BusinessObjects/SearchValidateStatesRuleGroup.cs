using System;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;

namespace MDM.BusinessObjects
{
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Represents the ValidationStates rule group for export syndication profile.
    /// </summary>
    [DataContract]
    public class SearchValidationStatesRuleGroup : MDMObject, ISearchValidationStatesRuleGroup
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
        /// Field specifying collection of search ValidationStates rule
        /// </summary>
        [DataMember]
        private SearchValidationStatesRuleCollection _searchValidationStatesRuleCollection = new SearchValidationStatesRuleCollection();

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the Search ValidationStates Rule group class.
        /// </summary>
        public SearchValidationStatesRuleGroup()
            : base()
        {

        }

        /// <summary>
        /// Xml Constructor
        /// </summary>
        /// <param name="valuesAsXml">Xml containing value of current SearchValidationStatesRuleGroup object</param>
        public SearchValidationStatesRuleGroup(String valuesAsXml)
        {
            LoadSearchValidationStatesRuleGroup(valuesAsXml);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Property specifies ValidationStates rule group object id
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
        /// Specifies the group operator for the ValidationStates rule group.
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
        /// Specifies the rule operator for the ValidationStates rule group.
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
        /// Property denoting collection of search ValidationStates rules.
        /// </summary>
        [DataMember]
        public SearchValidationStatesRuleCollection SearchValidationStatesRules
        {
            get
            {
                return this._searchValidationStatesRuleCollection;
            }
            set
            {
                this._searchValidationStatesRuleCollection = value;
            }
        }

        #endregion

        #region Methods

        #region Public Methods

        /// <summary>
        /// Represents SearchValidationStatesRuleGroup in Xml format
        /// </summary>
        /// <returns>String representation of current SearchValidationStatesRuleGroup object</returns>
        public override String ToXml()
        {
            String searchValidationStatesRuleGroupXML = String.Empty;
            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            //SearchValidationStatesRuleGroup node start
            xmlWriter.WriteStartElement("SearchValidationStatesRuleGroup");

            xmlWriter.WriteAttributeString("ObjectId", this.ObjectId.ToString());
            xmlWriter.WriteAttributeString("GroupOperator", this.GroupOperator.ToString());
            xmlWriter.WriteAttributeString("RuleOperator", this.RuleOperator.ToString());

            if (this._searchValidationStatesRuleCollection != null)
                xmlWriter.WriteRaw(this._searchValidationStatesRuleCollection.ToXml());

            //SearchValidationStatesRuleGroup node end
            xmlWriter.WriteEndElement();

            xmlWriter.Flush();

            //get the actual XML
            searchValidationStatesRuleGroupXML = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return searchValidationStatesRuleGroupXML;
        }

        /// <summary>
        /// Represents SearchValidationStatesRuleGroup in Xml format
        /// </summary>
        /// <param name="objectSerialization">Specifies the object serialization type</param>
        /// <returns>String representation of current SearchValidationStatesRuleGroup object</returns>
        public override String ToXml(ObjectSerialization objectSerialization)
        {
             return ToXml();
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
                if (obj is SearchValidationStatesRuleGroup)
                {
                    SearchValidationStatesRuleGroup objectToBeCompared = obj as SearchValidationStatesRuleGroup;

                    if (!this.SearchValidationStatesRules.Equals(objectToBeCompared.SearchValidationStatesRules))
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
            hashCode = base.GetHashCode() ^ this.SearchValidationStatesRules.GetHashCode();
            return hashCode;
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// Load current SearchValidationStatesRuleCollection from Xml
        /// </summary>
        /// <param name="valueAsXml">Xml having value for current SearchValidationStatesRuleCollection
        /// </param>
        private void LoadSearchValidationStatesRuleGroup(String valueAsXml)
        {
            if (!String.IsNullOrWhiteSpace(valueAsXml))
            {
                XmlTextReader reader = null;
                try
                {
                    reader = new XmlTextReader(valueAsXml, XmlNodeType.Element, null);

                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "SearchValidationStatesRuleGroup")
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
                        else if (reader.NodeType == XmlNodeType.Element && reader.Name == "SearchValidationStatesRules")
                        {
                            #region Read search ValidationStates rules collection

                            String searchValidationStatessXml = reader.ReadOuterXml();
                            if (!String.IsNullOrEmpty(searchValidationStatessXml))
                            {
                                SearchValidationStatesRuleCollection searchValidationStatesRuleCollection = new SearchValidationStatesRuleCollection(searchValidationStatessXml);
                                if (searchValidationStatesRuleCollection != null)
                                {
                                    foreach (SearchValidationStatesRule searchValidationStatesRule in searchValidationStatesRuleCollection)
                                    {
                                        if (!this.SearchValidationStatesRules.Contains(searchValidationStatesRule))
                                        {
                                            this.SearchValidationStatesRules.Add(searchValidationStatesRule);
                                        }
                                    }
                                }
                            }

                            #endregion Read search ValidationStates rules collection
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
