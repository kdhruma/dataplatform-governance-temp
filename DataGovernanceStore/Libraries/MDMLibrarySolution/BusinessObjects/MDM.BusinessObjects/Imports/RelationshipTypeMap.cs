using System;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;

namespace MDM.BusinessObjects.Imports
{
    using MDM.Core;

    /// <summary>
    /// RelationshipTypeMap
    /// </summary>
    [DataContract]
    public class RelationshipTypeMap : ObjectBase
    {
        #region Fields
        /// <summary>
        /// Field denoting name of the Relationship
        /// </summary>
        private String _name = String.Empty;

        /// <summary>
        ///Field defines if we need to fail entity processing when specified attribute has data error
        /// </summary>
        private Boolean _failEntityOnError = false;

        /// <summary>
        /// Field denoting the AttributeMap
        /// </summary>
        private AttributeMapCollection _attributeMaps = new AttributeMapCollection();

        #endregion

        #region Constructors
        /// <summary>
        /// Parameterless Constructor
        /// </summary>
        public RelationshipTypeMap()
            : base()
        {
        }

        /// <summary>
        /// Parametrized Constructor with Values as XMl
        /// </summary>
        /// <param name="valueAsXml">Values in XMl format which needs to be set when object is initialized.</param>
        public RelationshipTypeMap(String valueAsXml)
        {
            LoadRelationshipTypeMap(valueAsXml);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Property defines if we need to fail entity processing when specified attribute has data error
        /// </summary>
        [DataMember]
        public Boolean FailEntityOnError
        {
            get { return _failEntityOnError; }
            set { _failEntityOnError = value; }
        }

        /// <summary>
        /// Property denoting the AttributeMap
        /// </summary>
        [DataMember]
        public AttributeMapCollection AttributeMapCollection
        {
            get { return _attributeMaps; }
            set { _attributeMaps = value; }
        }

        /// <summary>
        /// Property denoting the relationship Name
        /// </summary>
        [DataMember]
        public String Name
        {
            get { return _name; }
            set { _name = value; }
        }
        #endregion

        #region Methods

        #region Public Methods

        /// <summary>
        /// Get Xml representation of RelationshipTypeMap
        /// </summary>
        /// <returns>Xml representation of object</returns>
        public String ToXml()
        {
            String relationshipTypeMapXml = String.Empty;
            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            xmlWriter.WriteStartElement("RelationshipTypeMap");

            #region write RelationshipTypeMap for Full Xml

            xmlWriter.WriteAttributeString("Name", this.Name);
            xmlWriter.WriteAttributeString("FailEntityOnError", this.FailEntityOnError.ToString().ToLowerInvariant());

            #endregion Write RelationshipTypeMap for Full Xml

            #region Write AttributeMapCollection Properties for Full RelationshipTypeMap Xml

            if (this.AttributeMapCollection != null)
                xmlWriter.WriteRaw(this.AttributeMapCollection.ToXml());

            #endregion Write AttributeMap Properties for Full RelationshipTypeMap Xml

            //RelationshipTypeMap node end
            xmlWriter.WriteEndElement();

            xmlWriter.Flush();

            //get the actual XML
            relationshipTypeMapXml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return relationshipTypeMapXml;
        }

        #endregion Public Methods

        #region Private Methods

        private void LoadRelationshipTypeMap(String valuesAsXml)
        {
            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                XmlTextReader reader = null;
                try
                {
                    reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "RelationshipTypeMap")
                        {
                            if (reader.HasAttributes)
                            {
                                #region Read AttributeMap

                                if (reader.MoveToAttribute("AttributeMaps"))
                                {
                                    String attributeMapXml = reader.ReadOuterXml();

                                    if (!String.IsNullOrEmpty(attributeMapXml))
                                    {
                                        AttributeMapCollection attributeMaps = new AttributeMapCollection(attributeMapXml);
                                        this.AttributeMapCollection = attributeMaps;
                                    }
                                }
                                #endregion

                                if (reader.MoveToAttribute("Name"))
                                {
                                    this.Name = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("FailEntityOnError"))
                                {
                                    this.FailEntityOnError = reader.ReadElementContentAsBoolean();
                                }
                            }
                            else
                            {
                                //Keep on reading the xml until we reach expected node.
                                reader.Read();
                            }
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

        #endregion Private Methods

        #endregion
    }
}
