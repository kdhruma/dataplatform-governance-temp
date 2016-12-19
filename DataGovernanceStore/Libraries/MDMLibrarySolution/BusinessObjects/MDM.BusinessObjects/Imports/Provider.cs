using System;
using System.Text;
using System.IO;
using System.Xml;
using System.Runtime.Serialization;

namespace MDM.BusinessObjects.Imports
{
    using MDM.Core;
    using MDM.Interfaces;
    using MDM.BusinessObjects;

    /// <summary>
    /// Import profile
    /// </summary>
    [DataContract]
    public class Provider : ObjectBase
    {
        #region Fields

        /// <summary>
        /// Provider Name
        /// </summary>
        private String _name = String.Empty;

        /// <summary>
        /// Import provider type
        /// </summary>
        private ImportProviderType _type = ImportProviderType.UnKnown;

        /// <summary>
        /// Enable
        /// </summary>
        private Boolean _enabled = true;

        /// <summary>
        /// 
        /// </summary>
        private ProviderSettingCollection _providerSettings = new ProviderSettingCollection();

        #endregion

        #region Constructors

        /// <summary>
        /// Parameterless Constructor
        /// </summary>
        public Provider()
            : base()
        {
        }

        /// <summary>
        /// Parameterized Constructor with values as XML
        /// </summary>
        /// <param name="valuesAsXml">Values in XMl format which has to be set when object is initialized</param>
        public Provider(String valuesAsXml)
        {
            LoadProvider(valuesAsXml);
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
                return "Provider";
            }
        }

        /// <summary>
        /// Property defining the name of a Provider
        /// </summary>
        public String Name
        {
            get
            {
                return this._name;
            }
            set
            {
                this._name = value;
            }
        }

        /// <summary>
        /// Property defining the type of a Provider
        /// </summary>
        public ImportProviderType Type
        {
            get
            {
                return this._type;
            }
            set
            {
                this._type = value;
            }
        }

        /// <summary>
        /// Property denoting whether profile is enabled or not.
        /// </summary>
        [DataMember]
        public Boolean Enabled
        {
            get { return _enabled; }
            set { _enabled = value; }
        }

        /// <summary>
        /// Provider Settings
        /// </summary>
        [DataMember]
        public ProviderSettingCollection ProviderSettings
        {
            get
            {
                return _providerSettings;
            }
            set
            {
                _providerSettings = value;
            }
        }

        #endregion

        #region Methods

        #region Load Methods

        ///<summary>
        /// Load Provider object from XML.
        /// </summary>
        /// <param name="valuesAsXml">XML having xml value</param>
        /// <example>
        ///     Sample XML:
        /// </example>
        public void LoadProvider(String valuesAsXml)
        {
            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                XmlTextReader reader = null;
                try
                {
                    reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "Provider")
                        {
                            #region Read Provider Properties

                            if (reader.HasAttributes)
                            {
                                if (reader.MoveToAttribute("Name"))
                                {
                                    this._name = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("Type"))
                                {
                                    ImportProviderType providerType = ImportProviderType.UnKnown;
                                    Enum.TryParse(reader.ReadContentAsString(), true, out providerType);
                                    this._type = providerType;
                                }

                                if (reader.MoveToAttribute("Enabled"))
                                {
                                    this.Enabled = reader.ReadContentAsBoolean();
                                }

                                reader.Read();
                            }

                            #endregion
                        }
                        else if (reader.NodeType == XmlNodeType.Element && reader.Name == "ProviderSettings")
                        {
                            #region Read ProviderSettings

                            String providerSettingsXml = reader.ReadOuterXml();
                            if (!String.IsNullOrEmpty(providerSettingsXml))
                            {
                                ProviderSettingCollection providerSettingCollection = new ProviderSettingCollection(providerSettingsXml);
                                this.ProviderSettings = providerSettingCollection;
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
        /// Get Xml representation of Import Profile
        /// </summary>
        /// <returns>Xml representation of object</returns>
        public String ToXml()
        {
            String importProfileXml = String.Empty;
            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            //Provider node start
            xmlWriter.WriteStartElement("Provider");

            #region Write Provider Properties

            xmlWriter.WriteAttributeString("Name", this.Name);
            xmlWriter.WriteAttributeString("Type", this.Type.ToString());
            xmlWriter.WriteAttributeString("Enabled", this.Enabled.ToString().ToLower());

            #endregion

            #region Write ProviderSettings

            if (this.ProviderSettings != null)
            {
                xmlWriter.WriteRaw(this.ProviderSettings.ToXml());
            }

            #endregion

            //Provider node end
            xmlWriter.WriteEndElement();

            xmlWriter.Flush();

            //get the actual XML
            importProfileXml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return importProfileXml;
        }

        /// <summary>
        /// Get Xml representation of Import Profile
        /// </summary>
        /// <returns>Xml representation of object</returns>
        public String ToXml(ObjectSerialization serialization)
        {
            String importProfileXml = String.Empty;

            if (serialization == ObjectSerialization.Full)
            {
                importProfileXml = this.ToXml();
            }
            else
            {
                StringWriter sw = new StringWriter();
                XmlTextWriter xmlWriter = new XmlTextWriter(sw);

                //Provider node start
                xmlWriter.WriteStartElement("Provider");

                if (serialization == ObjectSerialization.ProcessingOnly)
                {
                    #region Write Provider Properties

                    xmlWriter.WriteAttributeString("Name", this.Name);
                    xmlWriter.WriteAttributeString("Type", this.Type.ToString());
                    xmlWriter.WriteAttributeString("Enabled", this.Enabled.ToString().ToLower());

                    #endregion
                }
                else if (serialization == ObjectSerialization.UIRender)
                {
                    #region Write Provider Properties

                    xmlWriter.WriteAttributeString("Name", this.Name);
                    xmlWriter.WriteAttributeString("Type", this.Type.ToString());
                    xmlWriter.WriteAttributeString("Enabled", this.Enabled.ToString().ToLower());

                    #endregion
                }

                #region Write ProviderSettings

                if (this.ProviderSettings != null)
                {
                    xmlWriter.WriteRaw(this.ProviderSettings.ToXml());
                }

                #endregion

                //Provider node end
                xmlWriter.WriteEndElement();

                xmlWriter.Flush();

                //get the actual XML
                importProfileXml = sw.ToString();

                xmlWriter.Close();
                sw.Close();
            }
            return importProfileXml;
        }

        #endregion

        #region Private Methods

        #endregion
        
        #endregion
    }
}
