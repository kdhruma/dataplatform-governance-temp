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
    /// Represents the class that contain MDMRule context filter
    /// </summary>
    [DataContract]
    [ProtoContract(ImplicitFields = ImplicitFields.AllFields)]
    public class MDMRuleContextFilter : BaseMDMRuleContextFilter, IMDMRuleContextFilter
    {
        #region Fields

        /// <summary>
        /// Indicates the list of MDMRule names
        /// </summary>
        private Collection<String> _ruleNames = null;

        #endregion Fields

        #region Properties

        /// <summary>
        /// Property denotes the list of MDMRuleNames
        /// </summary>
        [DataMember]
        public Collection<String> MDMRuleNames
        {
            get
            {
                if (this._ruleNames == null)
                {
                    this._ruleNames = new Collection<String>();
                }

                return _ruleNames;
            }
            set
            {
                _ruleNames = value;
            }
        }

        #endregion Properties

        #region Constructor

        /// <summary>
        /// Indicates the parameter less constructor
        /// </summary>
        public MDMRuleContextFilter()
        { 
        
        }

        /// <summary>
        /// Constructor with value as Xml format
        /// </summary>
        /// <param name="valuesAsXml">Indicates the MDMRuleContextFilter object as xml format</param>
        public MDMRuleContextFilter(String valuesAsXml)
        {
            LoadRuleContextFilterFromXml(valuesAsXml);
        }

        #endregion Constructor

        #region Methods

        /// <summary>
        /// Gets Xml representation of MDMRuleContext filter Object
        /// </summary>
        public String ToXml()
        {
            String outputXml = String.Empty;

            using (StringWriter sw = new StringWriter())
            {
                using (XmlTextWriter xmlWriter = new XmlTextWriter(sw))
                {
                    xmlWriter.WriteStartElement("MDMRuleContextFilter");

                    xmlWriter.WriteAttributeString("MDMRuleType", this.MDMRuleType.ToString());
                    xmlWriter.WriteAttributeString("MDMRuleNames", ValueTypeHelper.JoinCollection<String>(this.MDMRuleNames, ","));
                    xmlWriter.WriteAttributeString("MDMRuleIds", ValueTypeHelper.JoinCollection<Int32>(this.MDMRuleIds, ","));
                    xmlWriter.WriteAttributeString("EntityIds", ValueTypeHelper.JoinCollection<Int64>(this.EntityIds, ","));
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
        /// <param name="valuesAsXml">Xml having values for current MDMRuleContext filter
        /// </param>
        public void LoadRuleContextFilterFromXml(String valuesAsXml)
        {
            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                using (XmlTextReader reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null))
                {
                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "MDMRuleContextFilter")
                        {
                            if (reader.HasAttributes)
                            {
                                if (reader.MoveToAttribute("MDMRuleNames"))
                                {
                                    this._ruleNames = ValueTypeHelper.SplitStringToStringCollection(reader.ReadContentAsString(), ',');
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
 