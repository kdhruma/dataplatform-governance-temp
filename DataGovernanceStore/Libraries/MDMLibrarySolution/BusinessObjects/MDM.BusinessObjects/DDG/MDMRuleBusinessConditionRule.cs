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
    /// Represents the class that contains BusinessConditionRule information
    /// </summary>
    [DataContract]
    [ProtoContract(ImplicitFields = ImplicitFields.AllFields)]
    public class MDMRuleBusinessConditionRule : MDMObject, IMDMRuleBusinessConditionRule, IBusinessRuleObject
    {
        #region Fields

        /// <summary>
        /// Field denoting the businesscondition id
        /// </summary>
        private Int32 _businessConditionId = -1;

        /// <summary>
        /// Field denoting the businesscondition name
        /// </summary>
        private String _businessConditionName = String.Empty;

        /// <summary>
        /// Field denoting the businesscondition status
        /// </summary>
        private RuleStatus _businessConditionStatus = RuleStatus.Unknown;

        /// <summary>
        /// Field denoting the businessconditionrule id
        /// </summary>
        private Int32 _businessConditionRuleId = -1;

        /// <summary>
        /// Field denoting the businessconditionrule name
        /// </summary>
        private String _businessConditionRuleName = String.Empty;

        /// <summary>
        /// Field denoting the businessconditionrule status
        /// </summary>
        private RuleStatus _businessConditionRuleStatus = RuleStatus.Unknown;

        /// <summary>
        /// Field denoting the businessconditionrule sequence
        /// </summary>
        private Int32 _sequence = Constants.DDG_DEFAULT_SORTORDER;

        #endregion Fields

        #region Properties

        /// <summary>
        /// Property denoting the businesscondition id
        /// </summary>
        [DataMember]
        public Int32 BusinessConditionId
        {
            get { return _businessConditionId; }
            set { _businessConditionId = value; }
        }

        /// <summary>
        /// Property denoting the businesscondition name
        /// </summary>
        [DataMember]
        public String BusinessConditionName
        {
            get { return _businessConditionName; }
            set { _businessConditionName = value; }
        }

        /// <summary>
        /// Property denoting the businesscondition status
        /// </summary>
        [DataMember]
        public RuleStatus BusinessConditionStatus
        {
            get { return _businessConditionStatus; }
            set { _businessConditionStatus = value; }
        }

        /// <summary>
        /// Property denoting the businessconditionrule id
        /// </summary>
        [DataMember]
        public Int32 BusinessConditionRuleId
        {
            get { return _businessConditionRuleId; }
            set { _businessConditionRuleId = value; }
        }

        /// <summary>
        /// Property denoting the businessconditionrule name
        /// </summary>
        [DataMember]
        public String BusinessConditionRuleName
        {
            get { return _businessConditionRuleName; }
            set { _businessConditionRuleName = value; }
        }

        /// <summary>
        /// Property denoting the businessconditionrule status
        /// </summary>
        [DataMember]
        public RuleStatus BusinessConditionRuleStatus
        {
            get { return _businessConditionRuleStatus; }
            set { _businessConditionRuleStatus = value; }
        }

        /// <summary>
        /// Property denoting the businessconditionrule sequence
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
        public MDMRuleBusinessConditionRule()
        {
        }

        /// <summary>
        /// Parameterized Constructor
        /// </summary>
        /// <param name="valuesAsXml">Indicates the MDMRuleBusinessConditionRule as Xml</param>
        public MDMRuleBusinessConditionRule(String valuesAsXml)
        {
            LoadMDMRuleBusinessConditionRuleFromXml(valuesAsXml);
        }

        #endregion Constructor

        #region Methods

        #region Public Methods

        #region Override Method

        /// <summary>
        /// Determines whether specified object is equal to the current object
        /// </summary>
        /// <param name="obj">MDMRuleBusinessConditionRule object which needs to be compared</param>
        /// <returns>Result of the comparison in Boolean</returns>
        public override Boolean Equals(Object obj)
        {
            if (obj is MDMRuleBusinessConditionRule)
            {
                MDMRuleBusinessConditionRule objectToBeCompared = obj as MDMRuleBusinessConditionRule;

                if (this.BusinessConditionId != objectToBeCompared.BusinessConditionId)
                {
                    return false;
                }

                if (String.Compare(this.BusinessConditionName, objectToBeCompared.BusinessConditionName) != 0)
                {
                    return false;
                }

                if (this.BusinessConditionStatus != objectToBeCompared.BusinessConditionStatus)
                {
                    return false;
                }

                if (this.BusinessConditionRuleId != objectToBeCompared.BusinessConditionRuleId)
                {
                    return false;
                }

                if (String.Compare(this.BusinessConditionRuleName, objectToBeCompared.BusinessConditionRuleName) != 0)
                {
                    return false;
                }

                if (this.BusinessConditionRuleStatus != objectToBeCompared.BusinessConditionRuleStatus)
                {
                    return false;
                }

                if (this.Sequence != objectToBeCompared.Sequence)
                {
                    return false;
                }

                return true;
            }

            return false;
        }

        /// <summary>
        /// Returns the hash code for this instance
        /// </summary>
        /// <returns>Hash code of the instance</returns>
        public override Int32 GetHashCode()
        {
            Int32 hashCode = 0;

            hashCode = base.GetHashCode() ^ this.BusinessConditionId.GetHashCode() ^ this.BusinessConditionStatus.GetHashCode() ^ this.BusinessConditionRuleId.GetHashCode() ^
                       this.BusinessConditionRuleStatus.GetHashCode() ^ this.Sequence.GetHashCode();

            if (!String.IsNullOrWhiteSpace(BusinessConditionName))
            {
                hashCode = hashCode ^ this.BusinessConditionName.GetHashCode();
            }

            if (!String.IsNullOrWhiteSpace(BusinessConditionRuleName))
            {
                hashCode = hashCode ^ this.BusinessConditionRuleName.GetHashCode();
            }

            return hashCode;
        }

        /// <summary>
        /// Gets Xml representation of MDMRuleBusinessConditionRule Object
        /// </summary>
        /// <returns>MDMRuleBusinessConditionRule Object as Xml</returns>
        public override String ToXml()
        {
            String outputXml = String.Empty;

            using (StringWriter sw = new StringWriter())
            {
                using (XmlTextWriter xmlWriter = new XmlTextWriter(sw))
                {
                    // MDMRuleBusinessConditionRule node start
                    xmlWriter.WriteStartElement("MDMRuleBusinessConditionRule");

                    xmlWriter.WriteAttributeString("BusinessConditionId", this.BusinessConditionId.ToString());
                    xmlWriter.WriteAttributeString("BusinessConditionName", this.BusinessConditionName);
                    xmlWriter.WriteAttributeString("BusinessConditionStatus", this.BusinessConditionStatus.ToString());
                    xmlWriter.WriteAttributeString("BusinessConditionRuleId", this.BusinessConditionRuleId.ToString());
                    xmlWriter.WriteAttributeString("BusinessConditionRuleName", this.BusinessConditionRuleName);
                    xmlWriter.WriteAttributeString("BusinessConditionRuleStatus", this.BusinessConditionRuleStatus.ToString());
                    xmlWriter.WriteAttributeString("Sequence", this.Sequence.ToString());

                    // MDMRuleBusinessConditionRule node end
                    xmlWriter.WriteEndElement();
                }

                // Get the output XML
                outputXml = sw.ToString();
            }

            return outputXml;
        }

        #endregion Override Method

        #endregion Public Methods

        #region Private Methods

        private void LoadMDMRuleBusinessConditionRuleFromXml(String valuesAsXml)
        {
            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                using (XmlTextReader reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null))
                {
                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "MDMRuleBusinessConditionRule")
                        {
                            #region Read MDMRuleBusinessConditionRule Attributes

                            if (reader.HasAttributes)
                            {
                                if (reader.MoveToAttribute("BusinessConditionId"))
                                {
                                    this.BusinessConditionId = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), this.BusinessConditionId);
                                }

                                if (reader.MoveToAttribute("BusinessConditionName"))
                                {
                                    this.BusinessConditionName = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("BusinessConditionStatus"))
                                {
                                    ValueTypeHelper.EnumTryParse<RuleStatus>(reader.ReadContentAsString(), true, out this._businessConditionStatus);
                                }

                                if (reader.MoveToAttribute("BusinessConditionRuleId"))
                                {
                                    this.BusinessConditionRuleId = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), this.BusinessConditionRuleId);
                                }

                                if (reader.MoveToAttribute("BusinessConditionRuleName"))
                                {
                                    this.BusinessConditionRuleName = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("BusinessConditionRuleStatus"))
                                {
                                    ValueTypeHelper.EnumTryParse<RuleStatus>(reader.ReadContentAsString(), true, out this._businessConditionRuleStatus);
                                }

                                if (reader.MoveToAttribute("Sequence"))
                                {
                                    this.Sequence = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), this.Sequence);
                                }

                                reader.Read();
                            }

                            #endregion Read MDMRuleBusinessConditionRule Attributes
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

        #endregion Private Methods

        #endregion Methods
    }
}
