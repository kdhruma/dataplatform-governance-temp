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
    /// Represents the class that contain MDMRule Attribute information
    /// </summary>
    [DataContract]
    [ProtoContract(ImplicitFields = ImplicitFields.AllFields)]
    public class MDMRuleAttribute : MDMObject, IMDMRuleAttribute
    {
        #region Fields

        /// <summary>
        /// Field denoting whether the attribute is required or not.
        /// </summary>
        private Boolean _isRequired = false;

        #endregion Fields

        #region Properties

        /// <summary>
        /// Property denoting whether the attribute is required or not.
        /// </summary>
        [DataMember]
        public Boolean IsRequired
        {
            get
            {
                return _isRequired;
            }
            set
            {
                this._isRequired = value;
            }
        }

        #endregion Properties

        #region Constructors

        /// <summary>
        /// Parameter less constructor
        /// </summary>
        public MDMRuleAttribute()
            : base()
        {

        }

        /// <summary>
        /// Constructor with attribute name and attribute Id
        /// </summary>
        /// <param name="attributeId">Indicates the attribute Id</param>
        /// <param name="attributeName">Indicates the attribute Name</param>
        public MDMRuleAttribute(Int32 attributeId, String attributeName)
            : base(attributeId, attributeName, String.Empty)
        {

        }

        /// <summary>
        /// Parameterised constructor having object values in xml format.
        /// Populates current object using XML.
        /// </summary>
        /// <param name="valuesAsXml">Indicates xml representation of MDMRule attribute object</param>
        public MDMRuleAttribute(String valuesAsXml)
        {
            LoadMDMRuleAttributeFromXml(valuesAsXml);
        }

        #endregion Constructors

        #region Methods

        #region Public Methods

        /// <summary>
        /// Initialize current object with Xml
        /// </summary>
        /// <param name="valuesAsXml">Xml having values for current MdmRule attribute
        /// </param>
        public void LoadMDMRuleAttributeFromXml(String valuesAsXml)
        {
            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                using (XmlTextReader reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null))
                {
                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "MDMRuleAttribute")
                        {
                            if (reader.HasAttributes)
                            {
                                if (reader.MoveToAttribute("Id"))
                                {
                                    this.Id = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), this.Id);
                                }

                                if (reader.MoveToAttribute("Name"))
                                {
                                    this.Name = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("IsRequired"))
                                {
                                    this.IsRequired = ValueTypeHelper.BooleanTryParse(reader.ReadContentAsString(), false);
                                }

                                reader.Read();
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
        /// Get Xml representation of MDMRule attribute object
        /// </summary>
        /// <returns>Xml representation of MDMRule attribute object</returns>
        public new String ToXml()
        {
            String outputXML = String.Empty;
            using (StringWriter stringWriter = new StringWriter())
            {
                using (XmlTextWriter xmlWriter = new XmlTextWriter(stringWriter))
                {
                    xmlWriter.WriteStartElement("MDMRuleAttribute");

                    xmlWriter.WriteAttributeString("Id", this.Id.ToString());
                    xmlWriter.WriteAttributeString("Name", this.Name);
                    xmlWriter.WriteAttributeString("IsRequired", this.IsRequired.ToString());

                    xmlWriter.WriteEndElement();
                    xmlWriter.Flush();

                    outputXML = stringWriter.ToString();
                }
            }
            return outputXML;
        }

        #endregion Public Methods

        #endregion Methods
    }
}