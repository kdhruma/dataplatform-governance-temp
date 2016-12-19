using System;
using System.IO;
using System.Xml;
using System.Runtime.Serialization;

namespace MDM.BusinessObjects
{
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Class that represents Feature configuration setting
    /// </summary>
    [DataContract]
    public class MDMFeatureConfigDetail : MDMObject, IMDMFeatureConfigDetail
    {
        #region Fields

        /// <summary>
        /// Field denoting application type under MDMCenter
        /// </summary>
        private ApplicationType _type;

        /// <summary>
        /// Field denoting Version of feature
        /// </summary>
        private String _version;

        /// <summary>
        /// Field denoting file name
        /// </summary>
        private String _fileName;

        /// <summary>
        /// Field denoting technology version
        /// </summary>
        private String _technologyVersion;

        /// <summary>
        /// Field denoting feature is default or not
        /// </summary>
        private Boolean _isDefault;

        /// <summary>
        /// Field denoting feature is enable or not
        /// </summary>
        private Boolean _isEnabled;
        
        #endregion

        #region Constructors

         /// <summary>
        /// Parameter-less Constructor
        /// </summary>
        public MDMFeatureConfigDetail()
            : base()
        {
        }        

        /// <summary>
        /// Constructor with application, modules, versions, isDefault and isEnabled as input parameter
        /// </summary>        
        /// <param name="type">Indicates the application type of MDMCenter</param>
        /// <param name="version">Indicates the version of feature</param>
        /// <param name="fileName">Indicates the file name.</param>
        /// <param name="technologyVersion">Indicates the technology version</param>
        /// <param name="isDefault">Indicated feature is default or not</param>
        /// <param name="isEnabled">Indicated feature is enable or not</param>
        public MDMFeatureConfigDetail(ApplicationType type, String version, String fileName, String technologyVersion, Boolean isDefault, Boolean isEnabled)
        {            
            this.Type=type;
            this.Version = version;
            this.FileName = fileName;
            this.TechnologyVersion = technologyVersion;
            this.IsDefault = isDefault;
            this.IsEnabled = isEnabled;
        }
        
        /// <summary>
        /// Constructor with XML as input parameter
        /// </summary>
        /// <param name="valuesAsXml">XML having value for MDMFeatureConfigDetail object</param>       
        public MDMFeatureConfigDetail(String valuesAsXml)
        {
            LoadMDMFeatureConfigDetail(valuesAsXml);
        }

        #endregion

        #region Property

        /// <summary>
        /// Property denoting application type under MDMCenter
        /// </summary>
        [DataMember]
        public ApplicationType Type
        {
            get { return _type; }
            set { this._type = value; }
        }

        /// <summary>
        /// Property denoting Version of feature
        /// </summary>
        [DataMember]
        public String Version
        {
            get { return _version; }
            set { this._version = value; }
        }

        /// <summary>
        /// Property denoting file name
        /// </summary>
        [DataMember]
        public String FileName
        {
            get { return _fileName; }
            set { this._fileName = value; }
        }

        /// <summary>
        /// Property denoting technology version
        /// </summary>
        [DataMember]
        public String TechnologyVersion
        {
            get { return _technologyVersion; }
            set { this._technologyVersion = value; }
        }

        /// <summary>
        /// Property denoting feature is default or not
        /// </summary>
        [DataMember]
        public Boolean IsDefault
        {
            get { return _isDefault; }
            set { this._isDefault = value; }
        }

        /// <summary>
        /// Property denoting feature is enable or not
        /// </summary>
        [DataMember]
        public Boolean IsEnabled
        {
            get { return _isEnabled; }
            set { this._isEnabled = value; }
        } 
        #endregion

        #region Public Methods

        /// <summary>
        /// Xml representation of MDMFeatureConfigDetail object
        /// </summary>
        /// <returns>Xml format of MDMFeatureConfigDetail</returns>
        public override String ToXml()
        {
            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            //MDMFeatureConfigDetail node start
            xmlWriter.WriteStartElement("MDMFeatureConfigDetail");

            xmlWriter.WriteAttributeString("ApplicationType", Type.ToString());
            xmlWriter.WriteAttributeString("Version", Version );
            xmlWriter.WriteAttributeString("FileName", FileName );
            xmlWriter.WriteAttributeString("TechnologyVersion", TechnologyVersion);
            xmlWriter.WriteAttributeString("IsDefault", IsDefault.ToString());
            xmlWriter.WriteAttributeString("IsEnabled", IsEnabled.ToString());            

            //MDMFeatureConfigDetail node end
            xmlWriter.WriteEndElement();

            xmlWriter.Flush();

            //get the actual XML
            String xml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return xml;
        }

        /// <summary>
        /// Xml representation of MDMFeatureConfigDetail object
        /// </summary>
        /// <param name="serialization">Type of serialization</param>
        /// <returns>Xml format of MDMFeatureConfigDetail</returns>
        public override String ToXml(ObjectSerialization serialization)
        {
            return ToXml();
        }

        #endregion Public Methods


        #region Private Methods

        private void LoadMDMFeatureConfigDetail(String valuesAsXml)
        {
            if (!String.IsNullOrEmpty(valuesAsXml))
            {
                XmlTextReader reader = null;

                try
                {
                    reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                    while (reader.Read())
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "MDMFeatureConfigDetail")
                        {
                            #region Read Properties

                            if (reader.HasAttributes)
                            {
                                if (reader.MoveToAttribute("ApplicationType"))
                                {
                                    String application = reader.ReadContentAsString();
                                    ApplicationType applicationType = ApplicationType.UI;
                                    Enum.TryParse(application, out applicationType);
                                    this.Type = applicationType;
                                }

                                if (reader.MoveToAttribute("Version"))
                                {
                                    this.Version = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("FileName"))
                                {
                                    this.FileName = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("TechnologyVersion"))
                                {
                                    this.TechnologyVersion = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("IsDefault"))
                                {
                                    this.IsDefault = ValueTypeHelper.BooleanTryParse(reader.ReadContentAsString(), this.IsEnabled);
                                }

                                if (reader.MoveToAttribute("IsEnabled"))
                                {
                                    this.IsEnabled = ValueTypeHelper.BooleanTryParse(reader.ReadContentAsString(), this.IsEnabled);
                                }
                            }

                            #endregion
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

        #endregion
       
    }
}
