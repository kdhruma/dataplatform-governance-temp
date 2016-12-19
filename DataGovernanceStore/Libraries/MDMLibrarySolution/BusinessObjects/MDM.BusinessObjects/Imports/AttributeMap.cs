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
    /// Attribute Map
    /// </summary>
    [DataContract]
    public class AttributeMap : ObjectBase
    {
        #region Fields

        /// <summary>
        /// 
        /// </summary>
        private AttributeSource _attributeSource = new AttributeSource();

        /// <summary>
        /// 
        /// </summary>
        private AttributeTarget _attributeTarget = new AttributeTarget();

        /// <summary>
        /// 
        /// </summary>
        private Boolean _isProcessed = false;

        #endregion

        #region Constructors

        /// <summary>
        /// Parameterless Constructor
        /// </summary>
        public AttributeMap()
            : base()
        {
        }

        /// <summary>
        /// Constructor with Values as XML
        /// </summary>
        /// <param name="valuesAsXml">Values which needs to be initialized as XMl format</param>
        public AttributeMap(String valuesAsXml)
        {
            LoadAttributeMap(valuesAsXml);
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
                return "AttributeMap";
            }
        }

        /// <summary>
        /// Property defines source map
        /// </summary>
        [DataMember]
        public AttributeSource AttributeSource
        {
            get { return _attributeSource; }
            set { _attributeSource = value; }
        }

        /// <summary>
        /// Property defines target map
        /// </summary>
        [DataMember]
        public AttributeTarget AttributeTarget
        {
            get { return _attributeTarget; }
            set { _attributeTarget = value; }
        }

        /// <summary>
        /// Property defines if the current attribute is processed
        /// </summary>
        public Boolean IsProcessed
        {
            get { return _isProcessed; }
            set { _isProcessed = value; }
        }

        #endregion

        #region Methods

        #region Load Methods

        ///<summary>
        /// Load AttributeMap object from XML.
        /// </summary>
        /// <param name="valuesAsXml">XML having xml value</param>
        /// <example>
        ///     Sample XML:
        /// </example>
        public void LoadAttributeMap(String valuesAsXml)
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
                            #region Read AttributeSource

                            String attributeSourceXml = reader.ReadOuterXml();
                            if (!String.IsNullOrEmpty(attributeSourceXml))
                            {
                                AttributeSource attributeSource = new AttributeSource(attributeSourceXml);
                                this.AttributeSource = attributeSource;
                            }

                            #endregion
                        }
                        else if (reader.NodeType == XmlNodeType.Element && reader.Name == "AttributeTarget")
                        {
                            #region Read AttributeTarget

                            String attributeTargetXml = reader.ReadOuterXml();
                            if (!String.IsNullOrEmpty(attributeTargetXml))
                            {
                                AttributeTarget attributeTarget = new AttributeTarget(attributeTargetXml);
                                this.AttributeTarget = attributeTarget;
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
        /// Get Xml representation of AttributeMap
        /// </summary>
        /// <returns>Xml representation of object</returns>
        public String ToXml()
        {
            String attributeMapXml = String.Empty;
            
            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            //AttributeMap node start
            xmlWriter.WriteStartElement("AttributeMap");

            #region Write AttributeMap Properties for Full AttributeMap Xml

            //xmlWriter.WriteAttributeString("Mode", this.Mode.ToString());

            #endregion

            #region Write AttributeSource

            xmlWriter.WriteRaw(this.AttributeSource.ToXml());

            #endregion

            #region Write AttributeTarget

            xmlWriter.WriteRaw(this.AttributeTarget.ToXml());

            #endregion

            //AttributeMap node end
            xmlWriter.WriteEndElement();

            xmlWriter.Flush();

            //get the actual XML
            attributeMapXml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return attributeMapXml;
        }

        ///<summary>
        /// Get Xml representation of AttributeMap
        /// </summary>
        /// <returns>Xml representation of object</returns>
        public String ToXml(ObjectSerialization serialization)
        {
            String attributeMapXml = String.Empty;

            if (serialization == ObjectSerialization.Full)
            {
                attributeMapXml = this.ToXml();
            }
            else
            {
                StringWriter sw = new StringWriter();
                XmlTextWriter xmlWriter = new XmlTextWriter(sw);

                //AttributeMap node start
                xmlWriter.WriteStartElement("AttributeMap");

                if (serialization == ObjectSerialization.ProcessingOnly)
                {
                    #region Write AttributeMap Properties for Processing Only

                    //xmlWriter.WriteAttributeString("Mode", this.Mode.ToString());

                    #endregion
                }
                else if (serialization == ObjectSerialization.UIRender)
                {
                    #region Write AttributeMap Properties for Rendering

                    //xmlWriter.WriteAttributeString("Mode", this.Mode.ToString());

                    #endregion
                }

                #region Write AttributeSource

                xmlWriter.WriteRaw(this.AttributeSource.ToXml());

                #endregion

                #region Write AttributeTarget

                xmlWriter.WriteRaw(this.AttributeTarget.ToXml());

                #endregion

                //mapping node end
                xmlWriter.WriteEndElement();

                xmlWriter.Flush();

                //get the actual XML
                attributeMapXml = sw.ToString();

                xmlWriter.Close();
                sw.Close();
            }

            return attributeMapXml;
        }

        #endregion

        #region Private Methods

        #endregion

        #endregion
    }
}
