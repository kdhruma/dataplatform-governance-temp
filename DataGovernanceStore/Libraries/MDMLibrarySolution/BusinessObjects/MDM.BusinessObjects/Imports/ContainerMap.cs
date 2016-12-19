using System;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;

namespace MDM.BusinessObjects.Imports
{
    using MDM.Core;

    /// <summary>
    /// Entity Container Map
    /// </summary>
    [DataContract]
    public class ContainerMap : ObjectBase
    {
        #region Fields

        /// <summary>
        ///
        /// </summary>
        private MappingMode _mode = MappingMode.Implicit;

        /// <summary>
        ///
        /// </summary>
        private String _inputFieldName = String.Empty;

        /// <summary>
        ///
        /// </summary>
        private Int32 _containerId = 0;

        #endregion

        #region Constructors

        /// <summary>
        /// Parameterless Constructor
        /// </summary>
        public ContainerMap()
            : base()
        {
        }

        /// <summary>
        /// Constructor with XML having values of object. 
        /// </summary>
        /// <param name="valuesAsXml">Indicates XML having xml value</param>
        public ContainerMap(String valuesAsXml)
        {
            LoadContainerMap(valuesAsXml);
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
                return "ContainerMap";
            }
        }

        /// <summary>
        /// Property defines mode of the Map
        /// </summary>
        [DataMember]
        public MappingMode Mode
        {
            get { return _mode; }
            set { _mode = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public String InputFieldName
        {
            get
            {
                return _inputFieldName;
            }
            set
            {
                _inputFieldName = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public Int32 ContainerId
        {
            get
            {
                return _containerId;
            }
            set
            {
                _containerId = value;
            }
        }

        #endregion

        #region Methods

        #region Load Methods

        ///<summary>
        /// Load ContainerMap object from XML.
        /// </summary>
        /// <param name="valuesAsXml">XML having xml value</param>
        /// <example>
        ///     Sample XML:
        /// </example>
        public void LoadContainerMap(String valuesAsXml)
        {
            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                XmlTextReader reader = null;
                try
                {
                    reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "ContainerMap")
                        {
                            #region Read ContainerMap Properties

                            if (reader.HasAttributes)
                            {
                                if (reader.MoveToAttribute("Mode"))
                                {
                                    MappingMode mode = MappingMode.Implicit;
                                    Enum.TryParse(reader.ReadContentAsString(), true, out mode);
                                    this.Mode = mode;
                                }

                                if (reader.MoveToAttribute("InputFieldName"))
                                {
                                    this.InputFieldName = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("ContainerId"))
                                {
                                    this.ContainerId = reader.ReadContentAsInt();
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
        /// Get Xml representation of ContainerMap
        /// </summary>
        /// <returns>Xml representation of object</returns>
        public String ToXml()
        {
            String mappingXml = String.Empty;
            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            //ContainerMap node start
            xmlWriter.WriteStartElement("ContainerMap");

            #region Write ContainerMap Properties for Full ContainerMap Xml

            xmlWriter.WriteAttributeString("Mode", this.Mode.ToString());
            xmlWriter.WriteAttributeString("InputFieldName", this.InputFieldName);
            xmlWriter.WriteAttributeString("ContainerId", this.ContainerId.ToString());

            #endregion

            //ContainerMap node end
            xmlWriter.WriteEndElement();

            xmlWriter.Flush();

            //get the actual XML
            mappingXml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return mappingXml;
        }

        ///<summary>
        /// Get Xml representation of ContainerMap
        /// </summary>
        /// <returns>Xml representation of object</returns>
        public String ToXml(ObjectSerialization serialization)
        {
            String mappingXml = String.Empty;

            if (serialization == ObjectSerialization.Full)
            {
                mappingXml = this.ToXml();
            }
            else
            {
                StringWriter sw = new StringWriter();
                XmlTextWriter xmlWriter = new XmlTextWriter(sw);

                //ContainerMap node start
                xmlWriter.WriteStartElement("ContainerMap");

                if (serialization == ObjectSerialization.ProcessingOnly)
                {
                    #region Write ContainerMap Properties for Processing Only

                    xmlWriter.WriteAttributeString("Mode", this.Mode.ToString());
                    xmlWriter.WriteAttributeString("InputFieldName", this.InputFieldName);
                    xmlWriter.WriteAttributeString("ContainerId", this.ContainerId.ToString());

                    #endregion
                }
                else if (serialization == ObjectSerialization.UIRender)
                {
                    #region Write ContainerMap Properties for Rendering

                    xmlWriter.WriteAttributeString("Mode", this.Mode.ToString());
                    xmlWriter.WriteAttributeString("InputFieldName", this.InputFieldName);
                    xmlWriter.WriteAttributeString("ContainerId", this.ContainerId.ToString());

                    #endregion
                }

                //mapping node end
                xmlWriter.WriteEndElement();

                xmlWriter.Flush();

                //get the actual XML
                mappingXml = sw.ToString();

                xmlWriter.Close();
                sw.Close();
            }

            return mappingXml;
        }

        #endregion

        #region Private Methods

        #endregion

        #endregion
    }
}
