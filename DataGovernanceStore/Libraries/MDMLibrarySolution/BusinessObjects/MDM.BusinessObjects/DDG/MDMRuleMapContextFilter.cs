using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;
using ProtoBuf;

namespace MDM.BusinessObjects
{
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Represents the class that contains MDMRule map context filter
    /// </summary>
    [DataContract]
    [ProtoContract(ImplicitFields = ImplicitFields.AllFields)]
    public class MDMRuleMapContextFilter : BaseMDMRuleContextFilter, IMDMRuleMapContextFilter
    {
        #region Fields

        /// <summary>
        /// Indicates the list of MDMRule map Ids
        /// </summary>
        private Collection<Int32> _ruleMapIds = null;

        /// <summary>
        /// Indicates the list of Event Ids
        /// </summary>
        private Collection<Int32> _eventIds = null;

        /// <summary>
        /// Indicates the workflow name
        /// </summary>
        private String _workflowName = String.Empty;

        /// <summary>
        /// Indicates the workflow activity name
        /// </summary>
        private String _workflowActivityName = String.Empty;

        /// <summary>
        /// Indicates name of the workflow action.
        /// </summary>
        private String _workflowAction = String.Empty;

        /// <summary>
        /// Indicates the workflow activity long name
        /// </summary>
        private String _workflowActivityLongName = String.Empty;

        #endregion Fields

        #region Properties

        /// <summary>
        /// Indicates the event id list
        /// </summary>
        [DataMember]
        public Collection<Int32> MDMEventIds
        {
            get
            {
                if (this._eventIds == null)
                {
                    this._eventIds = new Collection<Int32>();
                }
                return _eventIds;
            }
            set
            {
                _eventIds = value;
            }
        }

        /// <summary>
        /// Indicates the MDMRule map id list
        /// </summary>
        [DataMember]
        public Collection<Int32> MDMRuleMapIds
        {
            get
            {
                if (this._ruleMapIds == null)
                {
                    this._ruleMapIds = new Collection<Int32>();
                }

                return _ruleMapIds;
            }
            set
            {
                _ruleMapIds = value;
            }
        }

        /// <summary>
        /// Indicates the workflow short name
        /// </summary>
        [DataMember]
        public String WorkflowName
        {
            get { return _workflowName; }
            set { _workflowName = value; }
        }

        /// <summary>
        /// Indicates the workflow activity short name
        /// </summary>
        [DataMember]
        public String WorkflowActivityName
        {
            get { return _workflowActivityName; }
            set { _workflowActivityName = value; }
        }

        /// <summary>
        /// Indicates the workflow activity long name
        /// </summary>
        [DataMember]
        public String WorkflowActivityLongName
        {
            get { return _workflowActivityLongName; }
            set { _workflowActivityLongName = value; }
        }

        /// <summary>
        /// Indicates the workflow action
        /// </summary>
        [DataMember]
        public String WorkflowAction
        {
            get { return _workflowAction; }
            set { _workflowAction = value; }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Gets Xml representation of MDMRule map context filter Object
        /// </summary>
        public String ToXml()
        {
            String outputXml = String.Empty;

            using (StringWriter sw = new StringWriter())
            {
                using (XmlTextWriter xmlWriter = new XmlTextWriter(sw))
                {

                    xmlWriter.WriteStartElement("MDMRuleMapContextFilter");

                    xmlWriter.WriteAttributeString("MDMRuleType", this.MDMRuleType.ToString());
                    xmlWriter.WriteAttributeString("MDMRuleIds", ValueTypeHelper.JoinCollection<Int32>(this.MDMRuleIds, ","));
                    xmlWriter.WriteAttributeString("MDMRuleMapIds", ValueTypeHelper.JoinCollection<Int32>(this.MDMRuleMapIds, ","));
                    xmlWriter.WriteAttributeString("EntityIds", ValueTypeHelper.JoinCollection<Int64>(this.EntityIds, ","));
                    xmlWriter.WriteAttributeString("MDMEventIds", ValueTypeHelper.JoinCollection<Int32>(this.MDMEventIds, ","));

                    xmlWriter.WriteAttributeString("WorkflowName", this.WorkflowName);
                    xmlWriter.WriteAttributeString("WorkflowActivityName", this.WorkflowActivityName);
                    xmlWriter.WriteAttributeString("WorkflowActivityLongName", this.WorkflowActivityLongName);
                    xmlWriter.WriteAttributeString("WorkflowAction", this.WorkflowAction);
                    xmlWriter.WriteAttributeString("LoadValidationRules", this.LoadValidationRules.ToString());

                    if (this.ApplicationContexts != null)
                    {
                        xmlWriter.WriteRaw(this.ApplicationContexts.ToXml());

                    }

                    xmlWriter.WriteEndElement();
                }

                //Get the output XML
                outputXml = sw.ToString();
            }

            return outputXml;
        }

        /// <summary>
        /// Initialize current object with Xml
        /// </summary>
        /// <param name="valuesAsXml">Xml having values for current MDMRule map context filter
        /// </param>
        public void LoadRuleMapContextFilterFromXml(String valuesAsXml)
        {
            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                using (XmlTextReader reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null))
                {
                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "MDMRuleMapContextFilter")
                        {
                            if (reader.HasAttributes)
                            {
                                if (reader.MoveToAttribute("MDMEventIds"))
                                {
                                    this._eventIds = ValueTypeHelper.SplitStringToIntCollection(reader.ReadContentAsString(), ',');
                                }

                                if (reader.MoveToAttribute("MDMRuleMapIds"))
                                {
                                    this._ruleMapIds = ValueTypeHelper.SplitStringToIntCollection(reader.ReadContentAsString(), ',');
                                }

                                if (reader.MoveToAttribute("MDMRuleIds"))
                                {
                                    this.MDMRuleIds = ValueTypeHelper.SplitStringToIntCollection(reader.ReadContentAsString(), ',');
                                }

                                if (reader.MoveToAttribute("EntityIds"))
                                {
                                    this.EntityIds = ValueTypeHelper.SplitStringToLongCollection(reader.ReadContentAsString(), ',');
                                }

                                if (reader.MoveToAttribute("MDMRuleType"))
                                {
                                    MDMRuleType ruleType = Core.MDMRuleType.UnKnown;
                                    ValueTypeHelper.EnumTryParse<MDMRuleType>(reader.ReadContentAsString(), false, out ruleType);
                                    this.MDMRuleType = ruleType;
                                }

                                if (reader.MoveToAttribute("WorkflowName"))
                                {
                                    this._workflowName = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("WorkflowActivityName"))
                                {
                                    this._workflowActivityName = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("WorkflowActivityLongName"))
                                {
                                    this._workflowActivityLongName = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("WorkflowAction"))
                                {
                                    this._workflowAction = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("LoadValidationRules"))
                                {
                                    this.LoadValidationRules = ValueTypeHelper.BooleanTryParse(reader.ReadContentAsString(), false);
                                }

                                reader.Read();
                            }
                        }
                        else if (reader.NodeType == XmlNodeType.Element && reader.Name == "ApplicationContexts")
                        {
                            String applicationContextXml = reader.ReadOuterXml();

                            if (!String.IsNullOrEmpty(applicationContextXml))
                            {
                                this.ApplicationContexts = new ApplicationContextCollection(applicationContextXml);
                            }
                        }
                        else
                        {
                            reader.Read();
                        }
                    }
                }
            }
        }

        #endregion Methods
    }
}
