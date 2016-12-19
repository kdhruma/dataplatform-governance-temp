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
    /// Represents the class that contain MdmRule attribute context details
    /// </summary>
    [DataContract]
    [ProtoContract(ImplicitFields = ImplicitFields.AllFields)]
    public class MDMRuleAttributeContext : MDMObject, IMDMRuleAttributeContext
    {
        #region Fields

        /// <summary>
        /// Field denotes the data locale
        /// </summary>
        private LocaleEnum _dataLocale = LocaleEnum.UnKnown;

        /// <summary>
        /// Field denotes the MDMRule Attribute collection
        /// </summary>
        private MDMRuleAttributeCollection _attributes = null;

        /// <summary>
        /// Field denotes the MDMRule Attribute Group collection
        /// </summary>
        private MDMRuleAttributeGroupCollection _attributeGroups = null;

        #endregion Fields

        #region Properties

        /// <summary>
        /// Property denotes data locale.
        /// </summary>
        [DataMember]
        public LocaleEnum DataLocale
        {
            get
            {
                return this._dataLocale;
            }
            set
            {
                this._dataLocale = value;
            }
        }

        /// <summary>
        /// Property denotes the MDMRule attribute collection
        /// </summary>
        public MDMRuleAttributeCollection Attributes
        {
            get
            {
                if (this._attributes == null)
                {
                    this._attributes = new MDMRuleAttributeCollection();
                }

                return this._attributes;
            }
            set
            {
                this._attributes = value;
            }
        }

        /// <summary>
        /// Property denotes the MDMRule attribute group collection
        /// </summary>
        public MDMRuleAttributeGroupCollection AttributeGroups
        {
            get
            {
                if (this._attributeGroups == null)
                {
                    this._attributeGroups = new MDMRuleAttributeGroupCollection();
                }

                return this._attributeGroups;
            }
            set
            {
                this._attributeGroups = value;
            }
        }

        #endregion Properties

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the MdmRule attribute context
        /// </summary>
        public MDMRuleAttributeContext()
        {
        }

        /// <summary>
        /// Constructor which takes Xml as input
        /// </summary>
        /// <param name="valuesAsXml">XML having xml values</param>
        public MDMRuleAttributeContext(String valuesAsXml)
        {
            LoadAttributeContext(valuesAsXml);
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Initialize current object with Xml
        /// </summary>
        /// <param name="valuesAsXml">Xml having values for current MdmRule attribute context 
        /// </param>
        public void LoadAttributeContext(String valuesAsXml)
        {
            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                using (XmlTextReader reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null))
                {
                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "MDMRuleAttributeContext")
                        {
                            if (reader.HasAttributes)
                            {
                                if (reader.MoveToAttribute("Locale"))
                                {
                                    ValueTypeHelper.EnumTryParse<LocaleEnum>(reader.ReadContentAsString(), false, out _dataLocale);
                                }

                                reader.Read();
                            }
                        }
                        else if (reader.NodeType == XmlNodeType.Element && reader.Name == "MDMRuleAttributes")
                        {
                            String attributes = reader.ReadOuterXml();

                            if (String.IsNullOrWhiteSpace(attributes) == false)
                            {
                                this._attributes = new MDMRuleAttributeCollection(attributes);
                            }
                        }
                        else if (reader.NodeType == XmlNodeType.Element && reader.Name == "MDMRuleAttributeGroups")
                        {
                            String attributeGroups = reader.ReadOuterXml();

                            if (String.IsNullOrWhiteSpace(attributeGroups) == false)
                            {
                                this._attributeGroups = new MDMRuleAttributeGroupCollection(attributeGroups);
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

        /// <summary>
        /// Get Xml representation of MDMRule attribute context object
        /// </summary>
        /// <returns>Xml representation of MDMRule attribute context object</returns>
        public new String ToXml()
        {
            String outputXML = String.Empty;
            using (StringWriter stringWriter = new StringWriter())
            {
                using (XmlTextWriter xmlWriter = new XmlTextWriter(stringWriter))
                {
                    xmlWriter.WriteStartElement("MDMRuleAttributeContext");

                    xmlWriter.WriteAttributeString("Locale", DataLocale.ToString());

                    if (this._attributes != null)
                    {
                        xmlWriter.WriteRaw(this.Attributes.ToXml());
                    }
                    if (this._attributeGroups != null)
                    {
                        xmlWriter.WriteRaw(this.AttributeGroups.ToXml());
                    }

                    xmlWriter.WriteEndElement();
                    xmlWriter.Flush();

                    //get the actual XML
                    outputXML = stringWriter.ToString();
                }
            }
            return outputXML;
        }

        #endregion Methods
    }
}
