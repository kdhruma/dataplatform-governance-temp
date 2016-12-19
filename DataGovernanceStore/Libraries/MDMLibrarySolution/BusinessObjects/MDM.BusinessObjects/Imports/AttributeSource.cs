using System;
using System.Text;
using System.Xml;
using System.IO;
using System.Runtime.Serialization;

namespace MDM.BusinessObjects.Imports
{
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// EntityType Map
    /// </summary>
    [DataContract]
    public class AttributeSource : ObjectBase
    {
        #region Fields

        /// <summary>
        ///
        /// </summary>
        private String _name = String.Empty;

        /// <summary>
        ///
        /// </summary>
        private Boolean _isMandatory = false;

        /// <summary>
        /// 
        /// </summary>
        private Attribute _stagingAttributeInfo = new Attribute();

        #endregion

        #region Constructors

        /// <summary>
        /// Parameterless Constructor
        /// </summary>
        public AttributeSource()
            : base()
        {
        }

        /// <summary>
        /// Parameterized Constructor with Values as xml parameter
        /// </summary>
        /// <param name="valuesAsXml">Values as a XMl format which has to be initialized </param>
        public AttributeSource(String valuesAsXml)
        {
            LoadAttributeSource(valuesAsXml);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Property defining the type of the MDM object
        /// </summary>
        public new String ObjectType
        {
            get
            {
                return "AttributeSource";
            }
        }

        /// <summary>
        /// Property defines source name
        /// </summary>
        [DataMember]
        public String Name
        {
            get { return _name; }
            set { _name = value; }
        }

        /// <summary>
        /// Property defines if source is mandatory
        /// </summary>
        [DataMember]
        public Boolean IsMandatory
        {
            get { return _isMandatory; }
            set { _isMandatory = value; }
        }

        /// <summary>
        /// Property defines the staging attribute associated with the AttributeSource
        /// Notes: This properties is not serializable as part of ToXml()
        /// </summary>
        [DataMember]
        public Attribute StagingAttributeInfo 
        {
            get { return _stagingAttributeInfo; }
            set { _stagingAttributeInfo = value; }
        }

        #endregion

        #region Methods

        #region Load Methods

        ///<summary>
        /// Load AttributeSource object from XML.
        /// </summary>
        /// <param name="valuesAsXml">XML having xml value</param>
        /// <example>
        ///     Sample XML:
        /// </example>
        public void LoadAttributeSource(String valuesAsXml)
        {
            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                XmlTextReader reader = null;
                try
                {
                    reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "AttributeSource")
                        {
                            #region Read AttributeSource Properties

                            if (reader.HasAttributes)
                            {
                                if (reader.MoveToAttribute("Name"))
                                {
                                    this.Name = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("IsMandatory"))
                                {
                                    this.IsMandatory = reader.ReadContentAsBoolean();
                                }
                            }

                            #endregion
                        }
                        else
                        {
                            //Keep on reading the xml until we reach expected node.
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

        #region ToXml methods

        /// <summary>
        /// Get Xml representation of AttributeSource
        /// </summary>
        /// <returns>Xml representation of object</returns>
        public String ToXml()
        {
            String attributeSourceXml = String.Empty;

            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            //AttributeSource node start
            xmlWriter.WriteStartElement("AttributeSource");

            #region Write AttributeSource Properties for Full AttributeSource Xml

            xmlWriter.WriteAttributeString("Name", this.Name);
            xmlWriter.WriteAttributeString("IsMandatory", this.IsMandatory.ToString().ToLower());

            #endregion

            //AttributeSource node end
            xmlWriter.WriteEndElement();

            xmlWriter.Flush();

            //get the actual XML
            attributeSourceXml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return attributeSourceXml;
        }

        ///<summary>
        /// Get Xml representation of AttributeSource
        /// </summary>
        /// <returns>Xml representation of object</returns>
        public String ToXml(ObjectSerialization serialization)
        {
            String attributeSourceXml = String.Empty;

            if (serialization == ObjectSerialization.Full)
            {
                attributeSourceXml = this.ToXml();
            }
            else
            {
                StringWriter sw = new StringWriter();
                XmlTextWriter xmlWriter = new XmlTextWriter(sw);

                //AttributeSource node start
                xmlWriter.WriteStartElement("AttributeSource");

                if (serialization == ObjectSerialization.ProcessingOnly)
                {
                    #region Write AttributeSource Properties for Processing Only

                    xmlWriter.WriteAttributeString("Name", this.Name);
                    xmlWriter.WriteAttributeString("IsMandatory", this.IsMandatory.ToString().ToLower());

                    #endregion
                }
                else if (serialization == ObjectSerialization.UIRender)
                {
                    #region Write AttributeSource Properties for Rendering

                    xmlWriter.WriteAttributeString("Name", this.Name);
                    xmlWriter.WriteAttributeString("IsMandatory", this.IsMandatory.ToString().ToLower());

                    #endregion
                }

                //mapping node end
                xmlWriter.WriteEndElement();

                xmlWriter.Flush();

                //get the actual XML
                attributeSourceXml = sw.ToString();

                xmlWriter.Close();
                sw.Close();
            }

            return attributeSourceXml;
        }

        #endregion

        #region Private Methods

        #endregion

        #endregion
    }
}
