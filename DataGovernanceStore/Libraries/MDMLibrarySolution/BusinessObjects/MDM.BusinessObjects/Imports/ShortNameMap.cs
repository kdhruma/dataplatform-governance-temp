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
    /// Entity ShortName Map
    /// </summary>
    [DataContract]
    public class ShortNameMap : ObjectBase
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

        #endregion

        #region Constructors

        /// <summary>
        /// Parameterless Constructor
        /// </summary>
        public ShortNameMap()
            : base()
        {
        }

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="valuesAsXml">Values which needs to be set when object is initialized in XMl format</param>
        public ShortNameMap(String valuesAsXml)
        {
            LoadShortNameMap(valuesAsXml);
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
                return "ShortNameMap";
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

        #endregion

        #region Methods

        #region Load Methods

        ///<summary>
        /// Load ShortNameMap object from XML.
        /// </summary>
        /// <param name="valuesAsXml">XML having xml value</param>
        /// <example>
        ///     Sample XML:
        /// </example>
        public void LoadShortNameMap(String valuesAsXml)
        {
            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                XmlTextReader reader = null;
                try
                {
                    reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "ShortNameMap")
                        {
                            #region Read ShortNameMap Properties

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
        /// Get Xml representation of ShortNameMap
        /// </summary>
        /// <returns>Xml representation of object</returns>
        public String ToXml()
        {            
            String mappingXml = String.Empty;
            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            //ShortNameMap node start
            xmlWriter.WriteStartElement("ShortNameMap");

            #region Write ShortNameMap Properties for Full ShortNameMap Xml 

            xmlWriter.WriteAttributeString("Mode", this.Mode.ToString());
            xmlWriter.WriteAttributeString("InputFieldName", this.InputFieldName);
            
            #endregion

            //ShortNameMap node end
            xmlWriter.WriteEndElement();

            xmlWriter.Flush();

            //get the actual XML
            mappingXml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return mappingXml;
        }

        ///<summary>
        /// Get Xml representation of ShortNameMap
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

                //ShortNameMap node start
                xmlWriter.WriteStartElement("ShortNameMap");

                if (serialization == ObjectSerialization.ProcessingOnly)
                {
                    #region Write ShortNameMap Properties for Processing Only

                    xmlWriter.WriteAttributeString("Mode", this.Mode.ToString());
                    xmlWriter.WriteAttributeString("InputFieldName", this.InputFieldName);

                    #endregion
                }
                else if (serialization == ObjectSerialization.UIRender)
                {
                    #region Write ShortNameMap Properties for Rendering 

                    xmlWriter.WriteAttributeString("Mode", this.Mode.ToString());
                    xmlWriter.WriteAttributeString("InputFieldName", this.InputFieldName);

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
