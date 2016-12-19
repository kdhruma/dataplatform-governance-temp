using System;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;
using ProtoBuf;

namespace MDM.BusinessObjects
{
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Represents the class that contain MDMRule map detail
    /// </summary>
    [DataContract]
    [ProtoContract(ImplicitFields = ImplicitFields.AllFields)]
    public class MDMRuleMapDetail : MDMObject, IMDMRuleMapDetail, IEquatable<MDMRuleMapDetail>, ICloneable
    {
        #region Fields

        /// <summary>
        /// Field denotes the event id
        /// </summary>
        private Int32 _eventId;

        /// <summary>
        /// Field denotes the event name
        /// </summary>
        private String _eventName;

        /// <summary>
        /// Field denotes the event type
        /// </summary>
        private MDMEventType _eventType;

        /// <summary>
        /// Field denotes the application context
        /// </summary>
        private ApplicationContext _applicationContext;

        /// <summary>
        /// Field denotes the MDMRule 
        /// </summary>
        private MDMRule _mdmRule;

        /// <summary>
        /// Field denotes the sequence
        /// </summary>
        private Int32 _sequence = -1;

        /// <summary>
        /// Field denotes the value whether the rule execute in Async mode or Sync mode
        /// </summary>
        private Boolean _isAsyncRule = false;

        /// <summary>
        /// Field denotes whether the rule map is enabled or not
        /// </summary>
        private Boolean _isRuleMapEnabled = false;

        /// <summary>
        /// Field denotes the workflow name
        /// </summary>
        private String _workflowName;

        /// <summary>
        /// Field denotes the workflow activity id
        /// </summary>
        private Int32 _workflowActivityId;

        /// <summary>
        /// Field denotes the workflow activity name
        /// </summary>
        private String _workflowActivityName;

        /// <summary>
        /// Field denotes the workflow action
        /// </summary>
        private String _workflowAction;

        /// <summary>
        /// Field denotes the rule type
        /// </summary>
        private MDMRuleType _ruleType = MDMRuleType.UnKnown;

        /// <summary>
        /// Field denotes the Business condition id
        /// </summary>
        private Int32 _parentRuleId;

        /// <summary>
        /// Field denotes the parent rule name (BC rule name)
        /// </summary>
        private String _parentRuleName = String.Empty;

        /// <summary>
        /// Field denotes the ignore change context
        /// </summary>
        private Boolean _ignoreChangeContext = false;

        #endregion

        #region Properties

        /// <summary>
        /// Property denotes the event id
        /// </summary>
        [DataMember]
        public Int32 EventId
        {
            get
            {
                return _eventId;
            }
            set
            {
                _eventId = value;
            }
        }

        /// <summary>
        /// Property denotes the name of the event
        /// </summary>
        [DataMember]
        public String EventName
        {
            get
            {
                return _eventName;
            }
            set
            {
                _eventName = value;
            }
        }

        /// <summary>
        /// Property denotes the Business condition rule Id
        /// </summary>
        [DataMember]
        public Int32 ParentRuleId
        {
            get
            {
                return this._parentRuleId;
            }
            set
            {
                this._parentRuleId = value;
            }
        }

        /// <summary>
        /// Property denotes parent rule name (BC rule name)
        /// </summary>
        [DataMember]
        public String ParentRuleName
        {
            get
            {
                return _parentRuleName;
            }
            set
            {
                _parentRuleName = value;
            }
        }

        /// <summary>
        /// Property denotes the MDMEvent Type
        /// </summary>
        [DataMember]
        public MDMEventType EventType
        {
            get { return _eventType; }
            set { _eventType = value; }
        }

        /// <summary>
        /// Property denotes the application context
        /// </summary>
        [DataMember]
        public ApplicationContext ApplicationContext
        {
            get
            {
                if (_applicationContext == null)
                {
                    _applicationContext = new ApplicationContext();
                }

                return _applicationContext;
            }
            set
            {
                _applicationContext = value;
            }
        }

        /// <summary>
        /// Property denotes the MDMRule 
        /// </summary>
        [DataMember]
        public MDMRule MDMRule
        {
            get
            {
                return _mdmRule;
            }
            set
            {
                _mdmRule = value;
            }
        }

        /// <summary>
        /// Property denotes the sequence
        /// </summary>
        [DataMember]
        public Int32 Sequence
        {
            get
            {
                return _sequence;
            }
            set
            {
                _sequence = value;
            }
        }

        /// <summary>
        /// Property denotes whether the rule mapped under the current application context and event is enabled or not
        /// </summary>
        [DataMember]
        public Boolean IsRuleMapEnabled
        {
            get
            {
                return _isRuleMapEnabled;
            }
            set
            {
                _isRuleMapEnabled = value;
            }
        }

        /// <summary>
        /// Property denotes whether the rule should be executed Async mode or Sync mode
        /// </summary>
        [DataMember]
        public Boolean IsAsyncRule
        {
            get
            {
                return _isAsyncRule;
            }
            set
            {
                _isAsyncRule = value;
            }
        }

        /// <summary>
        /// Property denotes the workflow name
        /// </summary>
        [DataMember]
        public String WorkflowName
        {
            get
            {
                return _workflowName;
            }
            set
            {
                _workflowName = value;
            }
        }

        /// <summary>
        /// Property denotes the workflow activity id
        /// </summary>
        [DataMember]
        public Int32 WorkflowActivityId
        {
            get
            {
                return _workflowActivityId;
            }
            set
            {
                _workflowActivityId = value;
            }
        }

        /// <summary>
        /// Property denotes the workflow activity name
        /// </summary>
        [DataMember]
        public String WorkflowActivityName
        {
            get
            {
                return _workflowActivityName;
            }
            set
            {
                _workflowActivityName = value;
            }
        }

        /// <summary>
        /// Property denotes the workflow action
        /// </summary>
        [DataMember]
        public String WorkflowAction
        {
            get
            {
                return _workflowAction;
            }
            set
            {
                _workflowAction = value;
            }
        }

        /// <summary>
        /// Indicates the MDMRule type
        /// </summary>
        [DataMember]
        public MDMRuleType RuleType
        {
            get
            {
                return _ruleType;
            }
            set
            {
                _ruleType = value;
            }
        }

        /// <summary>
        /// Indicates the Ignore Change Context
        /// </summary>
        [DataMember]
        public Boolean IgnoreChangeContext
        {
            get
            {
                return _ignoreChangeContext;
            }
            set
            {
                _ignoreChangeContext = value;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Parameter less constructor
        /// </summary>
        public MDMRuleMapDetail()
            : base()
        {

        }

        /// <summary>
        /// Parameterized constructor having object values in xml format.
        /// Populates current object using XML.
        /// </summary>
        /// <param name="valuesAsXml">Indicates xml representation of MDMRuleMapDetail object</param>
        public MDMRuleMapDetail(String valuesAsXml)
        {
            LoadMDMRuleMapDetail(valuesAsXml);
        }

        /// <summary>
        /// Constructs the MDMRuleMapDetail using MDMRule and MDMRuleMap
        /// </summary>
        /// <param name="mdmRule">Indicates the MDMRule</param>
        /// <param name="ruleMap">Indicates the MDMRuleMap</param>
        /// <param name="bcRuleName">Indicates the BC rule name for the current Business rule</param>
        /// <param name="bcRuleId">Indicates the BC Rule Id</param>
        public MDMRuleMapDetail(MDMRule mdmRule, MDMRuleMap ruleMap, String bcRuleName = "", Int32 bcRuleId = 0)
        {
            if (mdmRule != null && ruleMap != null)
            {
                _applicationContext = ruleMap.ApplicationContext;
                _eventId = ruleMap.EventId;
                _eventName = ruleMap.EventName;
                _eventType = ruleMap.EventType;
                _isAsyncRule = ruleMap.IsAsyncRule;
                _isRuleMapEnabled = ruleMap.IsEnabled;
                _mdmRule = mdmRule;
                _ruleType = mdmRule.RuleType;
                _workflowAction = ruleMap.WorkflowInfo.WorkflowActivityAction;
                _workflowActivityId = ruleMap.WorkflowInfo.WorkflowActivityId;
                _workflowActivityName = ruleMap.WorkflowInfo.WorkflowActivityShortName;
                _workflowName = ruleMap.WorkflowInfo.WorkflowName;
                base.Name = ruleMap.Name;
                base.Id = ruleMap.Id;
                base.LongName = String.Format("{0}-{1}", ruleMap.Name, mdmRule.Name);
                _parentRuleId = bcRuleId;
                _parentRuleName = bcRuleName;
            }
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Gets the XML representation of MDMRule map detail object
        /// </summary>
        /// <returns>XML representation of MDMRule map detail object</returns>
        public override String ToXml()
        {
            String outputXML = String.Empty;
            using (StringWriter stringWriter = new StringWriter())
            {
                using (XmlTextWriter xmlWriter = new XmlTextWriter(stringWriter))
                {
                    xmlWriter.WriteStartElement("MDMRuleMapDetail");

                    xmlWriter.WriteAttributeString("Id", this.Id.ToString());
                    xmlWriter.WriteAttributeString("EventId", this._eventId.ToString());
                    xmlWriter.WriteAttributeString("EventName", this._eventName);
                    xmlWriter.WriteAttributeString("MDMEventType", this.EventType.ToString());
                    xmlWriter.WriteAttributeString("RuleType", this._ruleType.ToString());
                    xmlWriter.WriteAttributeString("IsRuleMapEnabled", this._isRuleMapEnabled.ToString());
                    xmlWriter.WriteAttributeString("Sequence", this._sequence.ToString());
                    xmlWriter.WriteAttributeString("IsAsyncRule", this._isAsyncRule.ToString());
                    xmlWriter.WriteAttributeString("WorkflowName", this._workflowName);
                    xmlWriter.WriteAttributeString("WorkflowActivityId", this._workflowActivityId.ToString());
                    xmlWriter.WriteAttributeString("WorkflowActivityName", this._workflowActivityName);
                    xmlWriter.WriteAttributeString("WorkflowAction", this._workflowAction);
                    xmlWriter.WriteAttributeString("MDMRuleMapName", this.Name);
                    xmlWriter.WriteAttributeString("ParentRuleName", this.ParentRuleName);
                    xmlWriter.WriteAttributeString("IgnoreChangeContext", this.IgnoreChangeContext.ToString());

                    if (this._mdmRule != null)
                    {
                        xmlWriter.WriteRaw(this._mdmRule.ToXml());
                    }

                    if (this._applicationContext != null)
                    {
                        xmlWriter.WriteRaw(this._applicationContext.ToXml());
                    }
                    xmlWriter.WriteEndElement();
                    xmlWriter.Flush();

                    //get the actual XML
                    outputXML = stringWriter.ToString();
                }
            }
            return outputXML;
        }

        /// <summary>
        /// Loads MDMRuleMapDetail object
        /// </summary>
        /// <param name="valuesAsXml">Indicates xml representation of MDMRuleMapDetail object</param>
        public void LoadMDMRuleMapDetail(String valuesAsXml)
        {
            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                using (XmlTextReader reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null))
                {
                    if (reader != null)
                    {
                        while (!reader.EOF)
                        {
                            if (reader.NodeType == XmlNodeType.Element && reader.Name == "MDMRuleMapDetail")
                            {
                                if (reader.HasAttributes)
                                {
                                    if (reader.MoveToAttribute("Id"))
                                    {
                                        this.Id = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), this.Id);
                                    }

                                    if (reader.MoveToAttribute("EventId"))
                                    {
                                        this._eventId = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), this._eventId);
                                    }

                                    if (reader.MoveToAttribute("MDMEventType"))
                                    {
                                        ValueTypeHelper.EnumTryParse<MDMEventType>(reader.ReadContentAsString(), false, out this._eventType);
                                    }

                                    if (reader.MoveToAttribute("EventName"))
                                    {
                                        this._eventName = reader.ReadContentAsString();
                                    }

                                    if (reader.MoveToAttribute("RuleType"))
                                    {
                                        ValueTypeHelper.EnumTryParse<MDMRuleType>(reader.ReadContentAsString(), false, out _ruleType);
                                    }

                                    if (reader.MoveToAttribute("IsRuleMapEnabled"))
                                    {
                                        this._isRuleMapEnabled = ValueTypeHelper.BooleanTryParse(reader.ReadContentAsString(), false);
                                    }

                                    if (reader.MoveToAttribute("Sequence"))
                                    {
                                        this._sequence = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), this._sequence);
                                    }

                                    if (reader.MoveToAttribute("IsAsyncRule"))
                                    {
                                        this._isAsyncRule = ValueTypeHelper.BooleanTryParse(reader.ReadContentAsString(), false);
                                    }

                                    if (reader.MoveToAttribute("WorkflowName"))
                                    {
                                        this._workflowName = reader.ReadContentAsString();
                                    }

                                    if (reader.MoveToAttribute("WorkflowActivityName"))
                                    {
                                        this._workflowActivityName = reader.ReadContentAsString();
                                    }

                                    if (reader.MoveToAttribute("WorkflowAction"))
                                    {
                                        this._workflowAction = reader.ReadContentAsString();
                                    }

                                    if (reader.MoveToAttribute("WorkflowActivityId"))
                                    {
                                        this._workflowActivityId = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), this._workflowActivityId);
                                    }

                                    if (reader.MoveToAttribute("MDMRuleMapName"))
                                    {
                                        this.Name = reader.ReadContentAsString();
                                    }

                                    if (reader.MoveToAttribute("ParentRuleName"))
                                    {
                                        this.ParentRuleName = reader.ReadContentAsString();
                                    }

                                    if (reader.MoveToAttribute("IgnoreChangeContext"))
                                    {
                                        this.IgnoreChangeContext = ValueTypeHelper.BooleanTryParse(reader.ReadContentAsString(), this.IgnoreChangeContext);
                                    }

                                    reader.Read();
                                }
                            }
                            else if (reader.NodeType == XmlNodeType.Element && reader.Name == "MDMRule")
                            {
                                String mdmRule = reader.ReadOuterXml();

                                if (!String.IsNullOrEmpty(mdmRule))
                                {
                                    this._mdmRule = new MDMRule(mdmRule);
                                }
                            }
                            else if (reader.NodeType == XmlNodeType.Element && reader.Name == "ApplicationContext")
                            {
                                String applicationContext = reader.ReadOuterXml();

                                if (!String.IsNullOrEmpty(applicationContext))
                                {
                                    this._applicationContext = new ApplicationContext(applicationContext);
                                }
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

        /// <summary>
        /// Set the application context to MDMRule map
        /// </summary>
        /// <param name="applicationContext">Indicates the application context</param>
        public void SetApplicationContext(IApplicationContext applicationContext)
        {
            this._applicationContext = (ApplicationContext)applicationContext;
        }

        /// <summary>
        /// Set the MDMRule to MDMRule map
        /// </summary>
        /// <param name="mdmRule">Indicates the MDMRule</param>
        public void SetMDMRule(IMDMRule mdmRule)
        {
            this._mdmRule = (MDMRule)mdmRule;
        }

        /// <summary>
        /// Gets the MDMRule
        /// </summary>
        /// <returns>MDMRule</returns>
        public IMDMRule GetMdmRule()
        {
            return MDMRule;
        }

        /// <summary>
        /// Denotes method for cloning
        /// </summary>
        public object Clone()
        {
            return this.MemberwiseClone();
        }

        /// <summary>
        /// Denotes method for equality comparison
        /// </summary>
        public Boolean Equals(MDMRuleMapDetail other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            if (base.Equals(other))
            {
                if (other is MDMRuleMapDetail)
                {
                    MDMRuleMapDetail objectToBeCompared = other as MDMRuleMapDetail;

                    if (this.ApplicationContext != null && objectToBeCompared.ApplicationContext != null)
                    {
                        if (!this.ApplicationContext.Equals(objectToBeCompared.ApplicationContext))
                        {
                            return false;
                        }
                    }
                    else if (!(this.ApplicationContext == null && objectToBeCompared.ApplicationContext == null))
                    {

                        return false;
                    }

                    if (this.MDMRule != null && objectToBeCompared.MDMRule != null)
                    {
                        if (!this.MDMRule.Equals(objectToBeCompared.MDMRule))
                        {
                            return false;
                        }
                    }
                    else if (!(this.MDMRule == null && objectToBeCompared.MDMRule == null))
                    {
                        return false;
                    }

                    if (this.Action != objectToBeCompared.Action)
                    {
                        return false;
                    }
                    else if (this.EventId != objectToBeCompared.EventId)
                    {
                        return false;
                    }
                    else if (this.EventName != objectToBeCompared.EventName)
                    {
                        return false;
                    }
                    else if (this.EventType != objectToBeCompared.EventType)
                    {
                        return false;
                    }
                    else if (this.Id != objectToBeCompared.Id)
                    {
                        return false;
                    }
                    else if (this.IsAsyncRule != objectToBeCompared.IsAsyncRule)
                    {
                        return false;
                    }
                    else if (this.IsRuleMapEnabled != objectToBeCompared.IsRuleMapEnabled)
                    {
                        return false;
                    }
                    else if (this.RuleType != objectToBeCompared.RuleType)
                    {
                        return false;
                    }
                    else if (this.Sequence != objectToBeCompared.Sequence)
                    {
                        return false;
                    }
                    else if (this.WorkflowAction != objectToBeCompared.WorkflowAction)
                    {
                        return false;
                    }
                    else if (this.WorkflowActivityId != objectToBeCompared.WorkflowActivityId)
                    {
                        return false;
                    }
                    else if (this.WorkflowActivityName != objectToBeCompared.WorkflowActivityName)
                    {
                        return false;
                    }
                    else if (this.WorkflowName != objectToBeCompared.WorkflowName)
                    {
                        return false;
                    }
                    else if (this.IgnoreChangeContext != objectToBeCompared.IgnoreChangeContext)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Denotes method for equality comparison
        /// </summary>
        public override Boolean Equals(Object obj)
        {
            return Equals(obj as MDMRuleMapDetail);
        }

        /// <summary>
        /// Denotes method for getting Hashcode
        /// </summary>
        public override Int32 GetHashCode()
        {
            Int32 hashCode = base.GetHashCode() ^ this.Action.GetHashCode() ^ this.EventId.GetHashCode() ^ this.EventName.GetHashCode()
                ^ this.EventType.GetHashCode() ^ this.Id.GetHashCode() ^ this.IsAsyncRule.GetHashCode() ^ this.IsRuleMapEnabled.GetHashCode()
                ^ this.MDMRule.GetHashCode() ^ this.RuleType.GetHashCode() ^ this.Sequence.GetHashCode() ^ this.WorkflowAction.GetHashCode()
                ^ this.WorkflowActivityId.GetHashCode() ^ this.WorkflowActivityName.GetHashCode() ^ this.WorkflowName.GetHashCode()
                ^ this.ApplicationContext.GetHashCode() ^ this.IgnoreChangeContext.GetHashCode();

            return hashCode;
        }

        #endregion
    }
}