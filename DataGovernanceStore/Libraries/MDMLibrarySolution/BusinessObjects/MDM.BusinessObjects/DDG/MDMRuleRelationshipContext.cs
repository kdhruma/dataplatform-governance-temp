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
    /// Represents the class that contain MdmRuleContext details
    /// </summary>
    [DataContract]
    [ProtoContract(ImplicitFields = ImplicitFields.AllFields)]
    public class MDMRuleRelationshipContext : MDMObject, IMDMRuleRelationshipContext
    {
        #region Fields

        /// <summary>
        /// Field denotes collection of MdmRule attribute context
        /// </summary>
        private MDMRuleAttributeContextCollection _relationshipAttributeContexts = null;

        /// <summary>
        /// Field denotes  the relationship type id of relationship.
        /// </summary>
        private Int32 _relationshipTypeId = -1;

        /// <summary>
        /// Field denotes  the relationship type name of relationship.
        /// </summary>
        private String _relationshipTypeName = String.Empty;

        #endregion Fields

        #region Properties

        /// <summary>
        /// Property denotes MdmRule relationship attribute context collection
        /// </summary>
        public MDMRuleAttributeContextCollection RelationshipAttributeContexts
        {
            get
            {
                if (this._relationshipAttributeContexts == null)
                {
                    this._relationshipAttributeContexts = new MDMRuleAttributeContextCollection();
                }

                return this._relationshipAttributeContexts;
            }
            set
            {
                this._relationshipAttributeContexts = value;
            }
        }

        /// <summary>
        /// Property denotes the relationship type name
        /// </summary>
        [DataMember]
        public Int32 RelationshipTypeId
        {
            get { return _relationshipTypeId; }
            set { _relationshipTypeId = value; }
        }

        /// <summary>
        /// Property denotes the relationship type name
        /// </summary>
        [DataMember]
        public String RelationshipTypeName
        {
            get { return _relationshipTypeName; }
            set { _relationshipTypeName = value; }
        }
        #endregion Properties

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the MdmRule relationship context
        /// </summary>
        public MDMRuleRelationshipContext()
            : base()
        {
        }

        /// <summary>
        /// Constructor which takes Xml as input
        /// </summary>
        /// <param name="valuesAsXml">XML having xml values</param>
        public MDMRuleRelationshipContext(String valuesAsXml)
        {
            LoadRelationshipContext(valuesAsXml);
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Initialize current object with Xml
        /// </summary>
        /// <param name="valuesAsXml">Xml having values for current MdmRule Relationship context 
        /// </param>
        public void LoadRelationshipContext(String valuesAsXml)
        {
            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                using (XmlTextReader reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null))
                {
                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "MDMRuleRelationshipContext")
                        {
                            if (reader.HasAttributes)
                            {
                                if (reader.MoveToAttribute("RelationshipTypeId"))
                                {
                                    this._relationshipTypeId = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), this._relationshipTypeId);
                                }

                                if (reader.MoveToAttribute("RelationshipTypeName"))
                                {
                                    this._relationshipTypeName = reader.ReadContentAsString();
                                }

                                reader.Read();
                            }
                        }
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "MDMRuleAttributeContexts")
                        {
                            String contexts = reader.ReadOuterXml();

                            if (!String.IsNullOrWhiteSpace(contexts))
                            {
                                MDMRuleAttributeContextCollection attributeContexts = new MDMRuleAttributeContextCollection(contexts);

                                if (attributeContexts != null)
                                {
                                    this._relationshipAttributeContexts = attributeContexts;
                                }
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
        /// Get Xml representation of MDMRule relationship context object
        /// </summary>
        /// <returns>Xml representation of MDMRule relationship context object</returns>
        public new String ToXml()
        {
            String outputXML = String.Empty;
            using (StringWriter stringWriter = new StringWriter())
            {
                using (XmlTextWriter xmlWriter = new XmlTextWriter(stringWriter))
                {
                    xmlWriter.WriteStartElement("MDMRuleRelationshipContext");

                    xmlWriter.WriteAttributeString("RelationshipTypeId", RelationshipTypeId.ToString());
                    xmlWriter.WriteAttributeString("RelationshipTypeName", RelationshipTypeName);

                    if (this._relationshipAttributeContexts != null)
                    {
                        xmlWriter.WriteRaw(this._relationshipAttributeContexts.ToXml());
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
