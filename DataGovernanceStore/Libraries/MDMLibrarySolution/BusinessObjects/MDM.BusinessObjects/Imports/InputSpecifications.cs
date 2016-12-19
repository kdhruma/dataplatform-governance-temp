using System;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;

namespace MDM.BusinessObjects.Imports
{
    using MDM.Core;

    /// <summary>
    /// Import input specifications
    /// </summary>
    [DataContract]
    public class InputSpecifications : ObjectBase
    {
        #region Fields

        /// <summary>
        /// Field denoting import source type
        /// </summary>
        private ImportSourceType _reader = ImportSourceType.UnKnown;

        /// <summary>
        /// Field denoting import source type
        /// </summary>
        private String _fileTypesSupported = String.Empty;

        /// <summary>
        /// Field denoting reader settings
        /// </summary>
        private ReaderSettingCollection _readerSettings = new ReaderSettingCollection();

        /// <summary>
        /// Property defines which program is the source of changes of object
        /// </summary>
        private Int32? _sourceId;

        #endregion

        #region Constructors

        /// <summary>
        /// Parameterless Constructor
        /// </summary>
        public InputSpecifications()
            : base()
        {
        }

        /// <summary>
        /// Constructor with XML having values of object. 
        /// </summary>
        /// <param name="valuesAsXml">Indicates XML having xml value</param>
        public InputSpecifications(String valuesAsXml)
        {
            LoadInputSpecifications(valuesAsXml);
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
                return "InputSpecifications";
            }
        }

        /// <summary>
        /// Property defines reader
        /// </summary>
        [DataMember]
        public ImportSourceType Reader
        {
            get { return _reader; }
            set { _reader = value; }
        }

        /// <summary>
        /// Property defines fileTypesSupported
        /// </summary>
        [DataMember]
        public String FileTypesSupported
        {
            get { return _fileTypesSupported; }
            set { _fileTypesSupported = value; }
        }

        /// <summary>
        /// Property defines reader settings
        /// </summary>
        [DataMember]
        public ReaderSettingCollection ReaderSettings
        {
            get
            {
                return _readerSettings;
            }
            set
            {
                _readerSettings = value;
            }
        }

        /// <summary>
        /// Property defines which program is the source of changes of object
        /// </summary>
        [DataMember]
        public Int32? SourceId
        {
            get
            {
                return _sourceId;
            }
            set
            {
                _sourceId = value;
            }
        }

        #endregion

        #region Methods

        #region Load Methods

        ///<summary>
        /// Load InputSpecifications object from XML.
        /// </summary>
        /// <param name="valuesAsXml">XML having xml value</param>
        /// <example>
        ///     Sample XML:
        /// </example>
        public void LoadInputSpecifications(String valuesAsXml)
        {
            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                XmlTextReader reader = null;
                try
                {
                    reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "InputSpecifications")
                        {
                            #region Read InputSpecifications Properties

                            if (reader.HasAttributes)
                            {
                                if (reader.MoveToAttribute("Reader"))
                                {
                                    ImportSourceType importSourceType = ImportSourceType.UnKnown;
                                    Enum.TryParse(reader.ReadContentAsString(), true, out importSourceType);
                                    this.Reader = importSourceType;
                                }

                                if (reader.MoveToAttribute("FileTypesSupported"))
                                {
                                    String fileTypesSupported = String.Empty;
                                   fileTypesSupported = Convert.ToString(reader.ReadContentAsString());
                                   this.FileTypesSupported = fileTypesSupported;
                                }

                                if (reader.MoveToAttribute("SourceId"))
                                {
                                    this.SourceId = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), -1);
                                }
                            }

                            #endregion
                        }
                        else if (reader.NodeType == XmlNodeType.Element && reader.Name == "ReaderSettings")
                        {
                            #region Read ReaderSettings

                            String readerSettingsXml = reader.ReadOuterXml();
                            if (!String.IsNullOrEmpty(readerSettingsXml))
                            {
                                ReaderSettingCollection readerSettings = new ReaderSettingCollection(readerSettingsXml);
                                this._readerSettings = readerSettings;
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
        /// Get Xml representation of InputSpecifications
        /// </summary>
        /// <returns>Xml representation of object</returns>
        public String ToXml()
        {
            String inputSpecificationsXml = String.Empty;
            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            //InputSpecifications node start
            xmlWriter.WriteStartElement("InputSpecifications");

            #region Write InputSpecifications Properties for Xml

            if (this.SourceId != null)
            {
                xmlWriter.WriteAttributeString("SourceId", this.SourceId.ToString());
            }

            xmlWriter.WriteAttributeString("Reader", this.Reader.ToString());

            xmlWriter.WriteAttributeString("FileTypesSupported", this.FileTypesSupported.ToString());
            
            #endregion

            #region Write ReaderSettings

            if (this.ReaderSettings != null)
            {
                xmlWriter.WriteRaw(this.ReaderSettings.ToXml());
            }

            #endregion

            //InputSpecifications node end
            xmlWriter.WriteEndElement();

            xmlWriter.Flush();

            //get the actual XML
            inputSpecificationsXml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return inputSpecificationsXml;
        }

        /// <summary>
        /// Get Xml representation of InputSpecifications
        /// </summary>
        /// <returns>Xml representation of object</returns>
        public String ToXml(ObjectSerialization serialization)
        {
            String inputSpecificationsXml = String.Empty;

            if (serialization == ObjectSerialization.Full)
            {
                inputSpecificationsXml = this.ToXml();
            }
            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            //InputSpecifications node start
            xmlWriter.WriteStartElement("InputSpecifications");

            if (serialization == ObjectSerialization.ProcessingOnly)
            {
                #region Write InputSpecifications Properties for Xml

                xmlWriter.WriteAttributeString("Reader", this.Reader.ToString());

                xmlWriter.WriteAttributeString("FileTypesSupported", this.FileTypesSupported.ToString());

                #endregion
            }
            else if (serialization == ObjectSerialization.UIRender)
            {
                #region Write InputSpecifications Properties for Xml

                xmlWriter.WriteAttributeString("Reader", this.Reader.ToString());

                xmlWriter.WriteAttributeString("FileTypesSupported", this.FileTypesSupported.ToString());

                #endregion
            }

            #region Write ReaderSettings

            if (this.ReaderSettings != null)
            {
                xmlWriter.WriteRaw(this.ReaderSettings.ToXml());
            }

            #endregion

            //InputSpecifications node end
            xmlWriter.WriteEndElement();

            xmlWriter.Flush();

            //get the actual XML
            inputSpecificationsXml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return inputSpecificationsXml;
        }

        #endregion

        #region Private Methods

        #endregion
        
        #endregion

    }
}
