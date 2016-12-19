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
    public class MDMFeatureConfig : ObjectBase, IMDMFeatureConfig
    {
        #region Fields

        /// <summary>
        /// Field denoting application under MDMCenter
        /// </summary>
        private MDMCenterApplication _application;

        /// <summary>
        /// field denoting Module short name
        /// </summary>
        private String _moduleName;

        /// <summary>
        /// field denoting ModulePath of MDMCenterApplication
        /// </summary>
        private String _modulePath;

        /// <summary>
        /// field denoting ModuleIdPath of MDMCenterApplication
        /// </summary>
        private String _moduleIdPath;

        /// <summary>
        /// Field denoting Version of feature
        /// </summary>
        private String _version;              

        /// <summary>
        /// Field denoting details about feature
        /// </summary>
        private MDMFeatureConfigDetailCollection _featureDetails;

        /// <summary>
        /// Field denoting overridden value of feature is enable or not
        /// </summary>
        private Boolean _overriddenValue;

        /// <summary>
        /// Field denoting inherited value of feature is enable or not
        /// </summary>
        private Boolean _inheritedValue;

        /// <summary>
        /// Field denoting short name of disabled parent
        /// </summary>
        private String _disabledParent;

        /// <summary>
        /// Field denoting disable level
        /// </summary>
        private FeatureConfigDisableLevel _disableLevel;

        #endregion

        #region Constructors

        /// <summary>
        /// Parameter-less Constructor
        /// </summary>
        public MDMFeatureConfig()
            : base()
        {
        }

        /// <summary>
        /// Constructor with application, moduleName, modulePath, moduleIdPath, versions, overridden and inherited as input parameter
        /// </summary>
        /// <param name="application">Indicates the Application for requested feature config</param>
        /// <param name="moduleName">Indicates module short name for requested feature config</param> 
        /// <param name="modulePath">Indicates module path for requested feature config</param>
        /// <param name="moduleIdPath">Indicates module id path for requested feature config</param>
        /// <param name="version">Indicates the version  for requested feature config</param>   
        /// <param name="overriddenValue">Indicated overridden value of feature is enable or not</param>
        /// <param name="inheritedValue">Indicated inherited value of feature is enable or not</param>
        /// <param name="disabledParent">Indicated short name of disabled parent</param>
        /// <param name="disableLevel">Indicated disable level for requested feature config</param>
        public MDMFeatureConfig(MDMCenterApplication application, String moduleName, String modulePath, String moduleIdPath, String version, Boolean overriddenValue, Boolean inheritedValue, String disabledParent, FeatureConfigDisableLevel disableLevel)
        {
            this.Application = application;
            this.ModuleName = moduleName;
            this.ModulePath = modulePath;
            this.ModuleIdPath = moduleIdPath;
            this.Version = version;
            this.OverriddenValue = overriddenValue;
            this.InheritedValue = inheritedValue;
            this.DisabledParent = disabledParent;
            this.DisableLevel = disableLevel;
        }

        /// <summary>
        /// Constructor with XML as input parameter
        /// </summary>
        /// <param name="valuesAsXml">XML having value for MDMFeatureConfig object</param>        
        public MDMFeatureConfig(String valuesAsXml)
        {
            LoadMDMFeatureConfig(valuesAsXml);
        }

        #endregion

        #region Property

        /// <summary>
        /// Property denoting application under MDMCenter
        /// </summary>
        [DataMember]
        public MDMCenterApplication Application
        {
            get { return _application; }
            set { this._application = value; }
        }

        /// <summary>
        /// Property denoting Module short name
        /// </summary>
        [DataMember]
        public String ModuleName
        {
            get { return _moduleName; }
            set { this._moduleName = value; }
        }

        /// <summary>
        /// Property denoting Module Path of MDMCenterApplication
        /// </summary>
        [DataMember]
        public String ModulePath
        {
            get { return _modulePath; }
            set { this._modulePath = value; }
        }

        /// <summary>
        /// Property denoting Module Id Path of MDMCenterApplication
        /// </summary>
        [DataMember]
        public String ModuleIdPath
        {
            get { return _moduleIdPath; }
            set { this._moduleIdPath = value; }
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
        /// Property denoting feature is enable or not
        /// </summary>
        [DataMember]
        public Boolean IsEnabled
        {
            get { return _overriddenValue && _inheritedValue; }
            set { _overriddenValue = value; }
        }

        /// <summary>
        /// Property denoting details about feature
        /// </summary>
        [DataMember]
        public MDMFeatureConfigDetailCollection FeatureDetails
        {
            get { return _featureDetails; }
            set { this._featureDetails = value; }
        }

        /// <summary>
        /// Property denoting overridden value of feature is enable or not
        /// </summary>
        [DataMember]
        public Boolean OverriddenValue
        {
            get { return _overriddenValue; }
            set { this._overriddenValue = value; }
        }

        /// <summary>
        /// Property denoting inherited value of feature is enable or not
        /// </summary>
        [DataMember]
        public Boolean InheritedValue
        {
            get { return _inheritedValue; }
            set { this._inheritedValue = value; }
        }

        /// <summary>
        /// Property denoting short name of disabled parent
        /// </summary>
        [DataMember]
        public String DisabledParent
        {
            get { return _disabledParent; }
            set { this._disabledParent = value; }
        }

        /// <summary>
        /// Property denoting disable level
        /// </summary>
        [DataMember]
        public FeatureConfigDisableLevel DisableLevel
        {
            get { return _disableLevel; }
            set { this._disableLevel = value; }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Xml representation of MDMFeatureConfig object
        /// </summary>
        /// <returns>Xml format of MDMFeatureConfig</returns>
        public String ToXml()
        {
            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            //MDMFeatureConfig node start
            xmlWriter.WriteStartElement("MDMFeatureConfig");

            xmlWriter.WriteAttributeString("Application", this.Application.ToString());
            xmlWriter.WriteAttributeString("ModuleName", this.ModuleName);
            xmlWriter.WriteAttributeString("Version", this.Version);
            xmlWriter.WriteAttributeString("ModulePath", this.ModulePath);
            xmlWriter.WriteAttributeString("ModuleIdPath", this.ModuleIdPath);            
            xmlWriter.WriteAttributeString("IsEnabled", this.IsEnabled.ToString());
            xmlWriter.WriteAttributeString("OverriddenValue", this.OverriddenValue.ToString());
            xmlWriter.WriteAttributeString("InheritedValue", this.InheritedValue.ToString());
            xmlWriter.WriteAttributeString("DisabledParent", this.DisabledParent);
            xmlWriter.WriteAttributeString("DisableLevel", this.DisableLevel.ToString());
            //MDMFeatureConfigDetail node start
            xmlWriter.WriteStartElement("MDMFeatureConfigDetails");

            if (this.FeatureDetails != null)
            {
                foreach (MDMFeatureConfigDetail mdmFeatureConfigDetail in this.FeatureDetails)
                {
                    xmlWriter.WriteRaw(mdmFeatureConfigDetail.ToXml());
                }
            }

            //MDMFeatureConfigDetail node end
            xmlWriter.WriteEndElement();

            //MDMFeatureConfig node end
            xmlWriter.WriteEndElement();

            xmlWriter.Flush();

            //get the actual XML
            String xml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return xml;
        }

        /// <summary>
        /// Xml representation of MDMFeatureConfig object
        /// </summary>
        /// <param name="serialization">Type of serialization</param>
        /// <returns>Xml format of MDMFeatureConfig</returns>
        public String ToXml(ObjectSerialization serialization)
        {
            return ToXml();
        }       

        /// <summary>
        /// Compare MDMFeatureConfig. 
        /// </summary>
        /// <param name="mdmFeatureConfig">Indicates expected MDMFeatureConfig.</param>        
        /// <returns>If acutual MDMFeatureConfig is match then true else false</returns>
        public Boolean IsSuperSetOf(MDMFeatureConfig mdmFeatureConfig)
        {
            if (mdmFeatureConfig == null)
            {
                return false;
            }            

            if (this.Application != mdmFeatureConfig.Application)
                return false;

            if (this.ModuleName != mdmFeatureConfig.ModuleName)
                return false;            

            if (this.Version != mdmFeatureConfig.Version)
                return false;

            if (this.ModulePath != mdmFeatureConfig.ModulePath)
                return false;

            if (this.OverriddenValue != mdmFeatureConfig.OverriddenValue)
                return false;

            if (this.InheritedValue != mdmFeatureConfig.InheritedValue)
                return false;

            if (this.DisabledParent != mdmFeatureConfig.DisabledParent)
                return false;

            if (this.DisableLevel != mdmFeatureConfig.DisableLevel)
                return false;

            if (this.IsEnabled != mdmFeatureConfig.IsEnabled)
                return false;           

            return true;
        }


        #endregion Public Methods

        #region Private Methods

        private void LoadMDMFeatureConfig(String valuesAsXml)
        {
            if (!String.IsNullOrEmpty(valuesAsXml))
            {
                XmlTextReader reader = null;

                try
                {
                    reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                    while (reader.Read())
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "MDMFeatureConfig")
                        {
                            #region Read Properties

                            if (reader.HasAttributes)
                            {
                                if (reader.MoveToAttribute("Application"))
                                {
                                    String application = reader.ReadContentAsString();
                                    MDMCenterApplication mdmCenterApplication = MDMCenterApplication.MDMCenter;                                    
                                    Enum.TryParse(application, out mdmCenterApplication);
                                    this.Application = mdmCenterApplication;
                                }

                                if (reader.MoveToAttribute("ModuleName"))
                                {
                                    this.ModuleName = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("ModulePath"))
                                {
                                    this.ModulePath = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("ModuleIdPath"))
                                {
                                    this.ModuleIdPath = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("Version"))
                                {
                                    this.Version = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("OverriddenValue"))
                                {
                                    this.OverriddenValue = ValueTypeHelper.BooleanTryParse(reader.ReadContentAsString(), this.OverriddenValue);
                                }

                                if (reader.MoveToAttribute("InheritedValue"))
                                {
                                    this.InheritedValue = ValueTypeHelper.BooleanTryParse(reader.ReadContentAsString(), this.InheritedValue);
                                }

                                if (reader.MoveToAttribute("DisabledParent"))
                                {
                                    this.DisabledParent = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("DisableLevel"))
                                {
                                    String disableLevel = reader.ReadContentAsString();
                                    FeatureConfigDisableLevel featureConfigDisableLevel = FeatureConfigDisableLevel.None;
                                    Enum.TryParse(disableLevel, out featureConfigDisableLevel);
                                    this.DisableLevel = featureConfigDisableLevel;
                                }
                            }

                            #endregion
                        }
                        else if (reader.NodeType == XmlNodeType.Element && reader.Name == "MDMFeatureConfigDetails")
                        {
                            #region Read MDMFeatureConfigDetails

                            String xml = reader.ReadOuterXml();
                            if (!String.IsNullOrWhiteSpace(xml))
                            {
                                this.FeatureDetails = new MDMFeatureConfigDetailCollection(xml);
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
