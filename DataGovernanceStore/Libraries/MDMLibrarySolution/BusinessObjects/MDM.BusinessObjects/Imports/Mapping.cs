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
    /// Import mapping specifications
    /// </summary>
    [DataContract]
    public class Mapping : ObjectBase
    {
        #region Fields

        /// <summary>
        ///
        /// </summary>
        private MappingDataType _sourceType = MappingDataType.UnKnown;

        /// <summary>
        ///
        /// </summary>
        private String _sourceName = String.Empty;

        /// <summary>
        ///
        /// </summary>
        private MappingDataType  _targetType = MappingDataType.UnKnown;

        /// <summary>
        ///
        /// </summary>
        private String _targetName = String.Empty;

        #endregion

        #region Constructors

        /// <summary>
        /// Parameterless Constructor
        /// </summary>
        public Mapping()
            : base()
        {
        }

        /// <summary>
        /// Load mapping object based on mapping xml
        /// </summary>
        /// <param name="valuesAsXml">Indicates mapping object in xml format</param>
        public Mapping(String valuesAsXml)
        {
            LoadMapping(valuesAsXml);
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
                return "Mapping";
            }
        }

        /// <summary>
        /// Property defines Source for the mapping
        /// </summary>
        [DataMember]
        public MappingDataType SourceType
        {
            get { return _sourceType; }
            set { _sourceType = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public String SourceName
        {
            get
            {
                return _sourceName;
            }
            set
            {
                _sourceName = value;
            }
        }

        /// <summary>
        /// Property defines Source for the mapping
        /// </summary>
        [DataMember]
        public MappingDataType TargetType
        {
            get { return _targetType; }
            set { _targetType = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public String TargetName
        {
            get
            {
                return _targetName;
            }
            set
            {
                _targetName = value;
            }
        }

        #endregion

        #region Methods

        #region Load Methods

        ///<summary>
        /// Load Mapping object from XML.
        /// </summary>
        /// <param name="valuesAsXml">XML having xml value</param>
        /// <example>
        ///     Sample XML:
        /// </example>
        public void LoadMapping(String valuesAsXml)
        {
            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                XmlTextReader reader = null;
                try
                {
                    reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "Mapping")
                        {
                            #region Read Mapping Properties

                            if (reader.HasAttributes)
                            {
                                if (reader.MoveToAttribute("SourceType"))
                                {
                                    MappingDataType mappingSourceType = MappingDataType.UnKnown;
                                    Enum.TryParse(reader.ReadContentAsString(), true, out mappingSourceType);
                                    this.SourceType = mappingSourceType;
                                }

                                if (reader.MoveToAttribute("SourceName"))
                                {
                                    this.SourceName = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("TargetType"))
                                {
                                    MappingDataType mappingTargetType = MappingDataType.UnKnown;
                                    Enum.TryParse(reader.ReadContentAsString(), true, out mappingTargetType);
                                    this.TargetType = mappingTargetType;
                                }

                                if (reader.MoveToAttribute("TargetName"))
                                {
                                    this.TargetName = reader.ReadContentAsString();
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
        /// Get Xml representation of Mapping
        /// </summary>
        /// <returns>Xml representation of object</returns>
        public String ToXml()
        {            
            String mappingXml = String.Empty;
            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            //Mapping node start
            xmlWriter.WriteStartElement("Mapping");

            #region Write Mapping Properties for Full Mapping Xml 

            xmlWriter.WriteAttributeString("SourceType", this.SourceType.ToString());
            xmlWriter.WriteAttributeString("SourceName", this.SourceName);
            xmlWriter.WriteAttributeString("TargetType", this.TargetType.ToString());
            xmlWriter.WriteAttributeString("TargetName", this.TargetName);

            #endregion

            //Mapping node end
            xmlWriter.WriteEndElement();

            xmlWriter.Flush();

            //get the actual XML
            mappingXml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return mappingXml;
        }

        ///<summary>
        /// Get Xml representation of Mapping
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

                //Mapping node start
                xmlWriter.WriteStartElement("Mapping");

                if (serialization == ObjectSerialization.ProcessingOnly)
                {
                    #region Write Mapping Properties for Processing Only

                    // TODO : Need to decide which all properties are needed for processing Xml.
                    // currently returning all properties.

                    xmlWriter.WriteAttributeString("SourceType", this.SourceType.ToString());
                    xmlWriter.WriteAttributeString("SourceName", this.SourceName);
                    xmlWriter.WriteAttributeString("TargetType", this.TargetType.ToString());
                    xmlWriter.WriteAttributeString("TargetName", this.TargetName);

                    #endregion
                }
                else if (serialization == ObjectSerialization.UIRender)
                {
                    #region Write Mapping Properties for Rendering 

                    // TODO : Need to decide which all properties are needed for Rendering Xml.
                    // currently returning all properties.

                    xmlWriter.WriteAttributeString("SourceType", this.SourceType.ToString());
                    xmlWriter.WriteAttributeString("SourceName", this.SourceName);
                    xmlWriter.WriteAttributeString("TargetType", this.TargetType.ToString());
                    xmlWriter.WriteAttributeString("TargetName", this.TargetName);

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
