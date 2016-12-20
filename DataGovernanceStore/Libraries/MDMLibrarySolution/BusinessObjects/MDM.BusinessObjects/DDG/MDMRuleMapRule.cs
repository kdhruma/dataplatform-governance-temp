using ProtoBuf;
using System;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;

namespace MDM.BusinessObjects
{
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Represents the class that contain MDMRuleMapRule information
    /// </summary>
    [DataContract]
    [ProtoContract(ImplicitFields = ImplicitFields.AllFields)]
    public class MDMRuleMapRule : MDMObject, IMDMRuleMapRule, ICloneable, IBusinessRuleObject
    {
        #region Fields

        /// <summary>
        /// Field denoting the rulemap id
        /// </summary>
        private Int32 _ruleMapId = -1;

        /// <summary>
        /// Field denoting the rulemap name
        /// </summary>
        private String _ruleMapName = String.Empty;

        /// <summary>
        /// Field denoting the rule id
        /// </summary>
        private Int32 _ruleId = -1;

        /// <summary>
        /// Field denoting the rule name
        /// </summary>
        private String _ruleName = String.Empty;

        /// <summary>
        /// Field denoting the rule type
        /// </summary>
        private MDMRuleType _ruleType = MDMRuleType.UnKnown;

        /// <summary>
        /// Field denoting the rule status
        /// </summary>
        private RuleStatus _ruleStatus = RuleStatus.Draft;

        /// <summary>
        /// Field denoting the ignorechangecontext
        /// </summary>
        private Boolean _ignoreChangeContext = false;

        /// <summary>
        /// Field denoting the sequence
        /// </summary>
        private Int32 _sequence = Constants.DDG_DEFAULT_SORTORDER;

        #endregion Fields

        #region Properties

        /// <summary>
        /// Property denoting the rulemap id
        /// </summary>
        [DataMember]
        public Int32 RuleMapId
        {
            get { return _ruleMapId; }
            set { _ruleMapId = value; }
        }

        /// <summary>
        /// Property denoting the rulemap name
        /// </summary>
        [DataMember]
        public String RuleMapName
        {
            get { return _ruleMapName; }
            set { _ruleMapName = value; }
        }

        /// <summary>
        /// Property denoting the rule id
        /// </summary>
        [DataMember]
        public Int32 RuleId
        {
            get { return _ruleId; }
            set { _ruleId = value; }
        }

        /// <summary>
        /// Property denoting the rule name
        /// </summary>
        [DataMember]
        public String RuleName
        {
            get { return _ruleName; }
            set { _ruleName = value; }
        }

        /// <summary>
        /// Property denoting the rule type
        /// </summary>
        [DataMember]
        public MDMRuleType RuleType
        {
            get { return _ruleType; }
            set { _ruleType = value; }
        }

        /// <summary>
        /// Property denoting the rule status
        /// </summary>
        [DataMember]
        public RuleStatus RuleStatus
        {
            get { return _ruleStatus; }
            set { _ruleStatus = value; }
        }

        /// <summary>
        /// Property denoting the ignorechangecontext
        /// </summary>
        [DataMember]
        public Boolean IgnoreChangeContext
        {
            get { return _ignoreChangeContext; }
            set { _ignoreChangeContext = value; }
        }

        /// <summary>
        /// Property denoting the sequence
        /// </summary>
        [DataMember]
        public Int32 Sequence
        {
            get { return _sequence; }
            set { _sequence = value; }
        }

        #endregion Properties

        #region Constructor

        /// <summary>
        /// Default Constructor
        /// </summary>
        public MDMRuleMapRule()
        {

        }

        /// <summary>
        /// Parameterized Constructor
        /// </summary>
        public MDMRuleMapRule(String valuesAsXml)
        {
            LoadMDMRuleMapRuleFromXml(valuesAsXml);
        }

        #endregion Constructor

        #region Methods

        #region Public Methods

        #region Override Methods

        /// <summary>
        /// Determines whether two Object instances are equal.
        /// </summary>
        /// <param name="obj">The Object to compare with the current Object.</param>
        /// <returns>true if the specified Object is equal to the current Object; otherwise, false.</returns>
        public override Boolean Equals(Object obj)
        {
            if (obj is MDMRuleMapRule)
            {
                MDMRuleMapRule objectToBeCompared = obj as MDMRuleMapRule;

                if (this.RuleMapId != objectToBeCompared.RuleMapId)
                {
                    return false;
                }

                if (String.Compare(this.RuleMapName, objectToBeCompared.RuleMapName, true) != 0)
                {
                    return false;
                }

                if (this.RuleId != objectToBeCompared.RuleId)
                {
                    return false;
                }

                if (String.Compare(this.RuleName, objectToBeCompared.RuleName, true) != 0)
                {
                    return false;
                }

                if (this.RuleType != objectToBeCompared.RuleType)
                {
                    return false;
                }

                if (this.RuleStatus != objectToBeCompared.RuleStatus)
                {
                    return false;
                }

                if (this.Sequence != objectToBeCompared.Sequence)
                {
                    return false;
                }

                if (this.IgnoreChangeContext != objectToBeCompared.IgnoreChangeContext)
                {
                    return false;
                }

                return true;
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

            hashCode = base.GetHashCode() ^ this.RuleMapId.GetHashCode() ^ this.RuleId.GetHashCode() ^ this.RuleType.GetHashCode() ^ this.RuleStatus.GetHashCode() ^
                       this.Sequence.GetHashCode() ^ this.IgnoreChangeContext.GetHashCode();

            if (!String.IsNullOrWhiteSpace(RuleMapName))
            {
                hashCode = hashCode ^ this.RuleMapName.GetHashCode();
            }

            if (!String.IsNullOrWhiteSpace(RuleName))
            {
                hashCode = hashCode ^ this.RuleName.GetHashCode();
            }

            return hashCode;
        }

        /// <summary>
        /// Get Xml representation of MDMRuleMapRule
        /// </summary>
        /// <returns>Returns xml representation of MDMRuleRule</returns>
        public new String ToXml()
        {
            String output = String.Empty;

            using (StringWriter sw = new StringWriter())
            {
                using (XmlTextWriter xmlWriter = new XmlTextWriter(sw))
                {
                    // MDMRuleMapRule node start
                    xmlWriter.WriteStartElement("MDMRuleMapRule");

                    xmlWriter.WriteAttributeString("RuleMapId", this.RuleMapId.ToString());
                    xmlWriter.WriteAttributeString("RuleMapName", this.RuleMapName);
                    xmlWriter.WriteAttributeString("RuleId", this.RuleId.ToString());
                    xmlWriter.WriteAttributeString("RuleName", this.RuleName);
                    xmlWriter.WriteAttributeString("RuleType", this.RuleType.ToString());
                    xmlWriter.WriteAttributeString("RuleStatus", this.RuleStatus.ToString());
                    xmlWriter.WriteAttributeString("Sequence", this.Sequence.ToString());
                    xmlWriter.WriteAttributeString("IgnoreChangeContext", this.IgnoreChangeContext.ToString());

                    // MDMRuleMapRule node end
                    xmlWriter.WriteEndElement();
                }

                //Get the actual XML
                output = sw.ToString();
            }

            return output;
        }

        /// <summary>
        /// Compares MDMRuleMapRuleCollection with current collection
        /// </summary>
        /// <param name="subsetMDMRuleMapRule">Indicates MDMRuleMapRuleCollection to be compare with the current collection</param>
        /// <param name="compareIds">Indicates whether to compare ids for the current object or not</param>
        /// <returns>Returns True : If both are same. False : otherwise</returns>
        public Boolean IsSuperSetOf(MDMRuleMapRule subsetMDMRuleMapRule, Boolean compareIds = false)
        {
            if (!base.IsSuperSetOf(subsetMDMRuleMapRule, compareIds))
            {
                return false;
            }

            if (compareIds)
            {
                if (this.RuleMapId != subsetMDMRuleMapRule.RuleMapId)
                {
                    return false;
                }

                if (this.RuleId != subsetMDMRuleMapRule.RuleId)
                {
                    return false;
                }
            }

            if (String.Compare(this.RuleMapName, subsetMDMRuleMapRule.RuleMapName) != 0)
            {
                return false;
            }

            if (String.Compare(this.RuleName, subsetMDMRuleMapRule.RuleName) != 0)
            {
                return false;
            }

            if (this.RuleType != subsetMDMRuleMapRule.RuleType)
            {
                return false;
            }

            if (this.RuleStatus != subsetMDMRuleMapRule.RuleStatus)
            {
                return false;
            }

            if (this.Sequence != subsetMDMRuleMapRule.Sequence)
            {
                return false;
            }

            if (this.IgnoreChangeContext != subsetMDMRuleMapRule.IgnoreChangeContext)
            {
                return false;
            }

            return true;
        }

        #endregion Override Methods

        #region MergeDelta & Clone Methods

        /// <summary>
        /// Gets a cloned instance of the current MDMRuleMapRule object
        /// </summary>
        /// <returns>Cloned instance of the current MDMRuleMapRule object</returns>
        public MDMRuleMapRule Clone()
        {
            MDMRuleMapRule clonedObject = new MDMRuleMapRule();

            clonedObject.RuleMapId = this.RuleMapId;
            clonedObject.RuleMapName = this.RuleMapName;
            clonedObject.RuleId = this.RuleId;
            clonedObject.RuleName = this.RuleName;
            clonedObject.RuleType = this.RuleType;
            clonedObject.RuleStatus = this.RuleStatus;
            clonedObject.Sequence = this.Sequence;
            clonedObject.IgnoreChangeContext = this.IgnoreChangeContext;

            return clonedObject;
        }

        #endregion Clone Methods

        #endregion Public Methods

        #region ICloneable Members

        /// <summary>
        /// Gets a cloned instance of the current MDMRuleMapRule object
        /// </summary>
        /// <returns>Cloned instance of the current MDMRuleMapRule object</returns>
        Object ICloneable.Clone()
        {
            return this.Clone();
        }

        #endregion ICloneable Members

        #region Private Methods

        private void LoadMDMRuleMapRuleFromXml(String valuesAsXml)
        {
            #region Sample Xml

            /*
             
            */

            #endregion Sample Xml

            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                using (XmlTextReader reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null))
                {
                    if (reader != null)
                    {
                        while (!reader.EOF)
                        {
                            if (reader.NodeType == XmlNodeType.Element && reader.Name == "MDMRuleMapRule")
                            {
                                #region Read MDMRuleMapRule Attributes

                                if (reader.HasAttributes)
                                {
                                    if (reader.MoveToAttribute("RuleMapId"))
                                    {
                                        this.RuleMapId = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), this.RuleMapId);
                                    }

                                    if (reader.MoveToAttribute("RuleMapName"))
                                    {
                                        this.RuleMapName = reader.ReadContentAsString();
                                    }

                                    if (reader.MoveToAttribute("RuleId"))
                                    {
                                        this.RuleId = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), this.RuleId);
                                    }

                                    if (reader.MoveToAttribute("RuleName"))
                                    {
                                        this.RuleName = reader.ReadContentAsString();
                                    }

                                    if (reader.MoveToAttribute("RuleType"))
                                    {
                                        ValueTypeHelper.EnumTryParse<MDMRuleType>(reader.ReadContentAsString(), true, out this._ruleType);
                                    }

                                    if (reader.MoveToAttribute("RuleStatus"))
                                    {
                                        ValueTypeHelper.EnumTryParse<RuleStatus>(reader.ReadContentAsString(), true, out this._ruleStatus);
                                    }

                                    if (reader.MoveToAttribute("Sequence"))
                                    {
                                        this.Sequence = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), this.Sequence);
                                    }

                                    if (reader.MoveToAttribute("IgnoreChangeContext"))
                                    {
                                        this.IgnoreChangeContext = ValueTypeHelper.BooleanTryParse(reader.ReadContentAsString(), this.IgnoreChangeContext);
                                    }

                                    reader.Read();
                                }

                                #endregion Read MDMRuleMapRule Attributes
                            }
                            else
                            {
                                //Keep on reading the xml until we reach expected node.
                                reader.Read();
                            }
                        }
                    }
                }
            }
        }

        #endregion Private Methods

        #endregion Methods
    }
}
