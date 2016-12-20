using System;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;

namespace MDM.BusinessObjects.Imports
{
    using MDM.Core;

    /// <summary>
    /// Locale Map
    /// </summary>
    [DataContract]
    public class LocaleMap : ObjectBase
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
        private LocaleEnum _locale = LocaleEnum.UnKnown;

        #endregion

        #region Constructors

        /// <summary>
        /// Parameterless Constructor
        /// </summary>
        public LocaleMap()
            : base()
        {
        }

        /// <summary>
        /// Constructor with XML having values of object. 
        /// </summary>
        /// <param name="valuesAsXml">Indicates XML having xml value</param>
        public LocaleMap(String valuesAsXml)
        {
            LoadLocaleMap(valuesAsXml);
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
                return "LocaleMap";
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
        public LocaleEnum Locale
        {
            get
            {
                return _locale;
            }
            set
            {
                _locale = value;
            }
        }

        #endregion

        #region Methods

        #region Load Methods

        ///<summary>
        /// Load LocaleMap object from XML.
        /// </summary>
        /// <param name="valuesAsXml">XML having xml value</param>
        /// <example>
        ///     Sample XML:
        /// </example>
        public void LoadLocaleMap(String valuesAsXml)
        {
            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                XmlTextReader reader = null;
                try
                {
                    reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "LocaleMap")
                        {
                            #region Read LocaleMap Properties

                            if (reader.HasAttributes)
                            {
                                if (reader.MoveToAttribute("Mode"))
                                {
                                    MappingMode mode = MappingMode.Implicit;
                                    Enum.TryParse(reader.ReadContentAsString(), true, out mode);
                                    this.Mode= mode;
                                }

                                if (reader.MoveToAttribute("InputFieldName"))
                                {
                                    this.InputFieldName = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("Locale"))
                                {
                                    LocaleEnum locale = LocaleEnum.UnKnown;
                                    Enum.TryParse(reader.ReadContentAsString(), true, out locale);
                                    this.Locale = locale;
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
        /// Get Xml representation of LocaleMap
        /// </summary>
        /// <returns>Xml representation of object</returns>
        public String ToXml()
        {            
            String mappingXml = String.Empty;
            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            //LocaleMap node start
            xmlWriter.WriteStartElement("LocaleMap");

            #region Write LocaleMap Properties for Full LocaleMap Xml 

            xmlWriter.WriteAttributeString("Mode", this.Mode.ToString());
            xmlWriter.WriteAttributeString("InputFieldName", this.InputFieldName);
            xmlWriter.WriteAttributeString("Locale", this.Locale.ToString());
            
            #endregion

            //LocaleMap node end
            xmlWriter.WriteEndElement();

            xmlWriter.Flush();

            //get the actual XML
            mappingXml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return mappingXml;
        }

        ///<summary>
        /// Get Xml representation of LocaleMap
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

                //LocaleMap node start
                xmlWriter.WriteStartElement("LocaleMap");

                if (serialization == ObjectSerialization.ProcessingOnly)
                {
                    #region Write LocaleMap Properties for Processing Only

                    xmlWriter.WriteAttributeString("Mode", this.Mode.ToString());
                    xmlWriter.WriteAttributeString("InputFieldName", this.InputFieldName);
                    xmlWriter.WriteAttributeString("Locale", this.Locale.ToString());

                    #endregion
                }
                else if (serialization == ObjectSerialization.UIRender)
                {
                    #region Write LocaleMap Properties for Rendering 

                    xmlWriter.WriteAttributeString("Mode", this.Mode.ToString());
                    xmlWriter.WriteAttributeString("InputFieldName", this.InputFieldName);
                    xmlWriter.WriteAttributeString("Locale", this.Locale.ToString());

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
