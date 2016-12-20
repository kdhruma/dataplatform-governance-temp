using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml;

namespace MDM.BusinessObjects.Integration
{
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Class for connector profile
    /// </summary>
    [DataContract]
    [KnownType(typeof(RunTimeSpecifications))]
    [KnownType(typeof(QualificationConfiguration))]
    [KnownType(typeof(ProcessingConfiguration))]
    [KnownType(typeof(AggregationConfiguration))]
    public class ConnectorProfile : MDMObject, IConnectorProfile
    {
        #region Fields

        /// <summary>
        /// Indicates Id of connector
        /// </summary>
        private Int16 _id = -1;

        /// <summary>
        /// Indicates weightage for the connector
        /// </summary>
        private Int32 _weightage = -1;

        /// <summary>
        /// Indicates if connector is enabled
        /// </summary>
        private Boolean _enabled = true;

        /// <summary>
        /// Indicates default IntegrationMessageType (ShortName) to be used for creating ActivityLog through FileWatcher.
        /// </summary>
        private String _defaultInboundIntegrationMessageTypeName = String.Empty;

        /// <summary>
        /// Contains properties for connector's run time configuration. E.g., Assembly name, class name implementing IConnector methods
        /// </summary>
        private RunTimeSpecifications _runTimeSpecifications = new RunTimeSpecifications();

        /// <summary>
        /// Contains configuration options for qualifying queue
        /// </summary>
        private QualificationConfiguration _qualificationConfiguration = new QualificationConfiguration();

        /// <summary>
        /// Contains configuration options for aggregation queue
        /// </summary>
        private AggregationConfiguration _aggregationConfiguration = new AggregationConfiguration();

        /// <summary>
        /// Contains configuration options for processing (outbound) queue
        /// </summary>
        private ProcessingConfiguration _processingConfiguration = new ProcessingConfiguration();

        /// <summary>
        /// Contains configuration options for processing (outbound) queue
        /// </summary>
        private JigsawIntegrationConfiguration _jigsawIntegartionConfiguration = new JigsawIntegrationConfiguration();

        /// <summary>
        /// Place holder for any additional setting.
        /// </summary>
        private Collection<KeyValuePair<String, String>> _additionalConfigurations = new Collection<KeyValuePair<String, String>>();

        #endregion Fields

        #region Constructor

        /// <summary>
        /// Parameterless Constructor
        /// </summary>
        public ConnectorProfile()
            : base()
        {
        }

        /// <summary>
        /// Xml constructor
        /// </summary>
        /// <param name="valuesAsXml">Value in xml format</param>
        public ConnectorProfile(String valuesAsXml)
        {
            LoadConnectorProfile(valuesAsXml);
        }

        #endregion Constructor

        #region Properties

        /// <summary>
        /// Indicates Id of connector
        /// </summary>
        [DataMember]
        new public Int16 Id
        {
            get { return _id; }
            set { _id = value; }
        }

        /// <summary>
        /// Indicates weightage for the connector
        /// </summary>
        [DataMember]
        public Int32 Weightage
        {
            get { return _weightage; }
            set { _weightage = value; }
        }

        /// <summary>
        /// Indicates if connector is enabled
        /// </summary>
        [DataMember]
        public Boolean Enabled
        {
            get { return _enabled; }
            set { _enabled = value; }
        }

        /// <summary>
        /// Indicates default IntegrationMessageType (ShortName) to be used for creating ActivityLog through FileWatcher.
        /// </summary>
        [DataMember]
        public String DefaultInboundIntegrationMessageTypeName
        {
            get { return _defaultInboundIntegrationMessageTypeName; }
            set { _defaultInboundIntegrationMessageTypeName = value; }
        }

        /// <summary>
        /// Contains properties for connector's run time configuration. E.g., Assembly name, class name implementing IConnector methods
        /// </summary>
        [DataMember]
        public RunTimeSpecifications RunTimeSpecifications
        {
            get { return _runTimeSpecifications; }
            set { _runTimeSpecifications = value; }
        }

        /// <summary>
        /// Contains configuration options for qualifying queue
        /// </summary>
        [DataMember]
        public QualificationConfiguration QualificationConfiguration
        {
            get { return _qualificationConfiguration; }
            set { _qualificationConfiguration = value; }
        }

        /// <summary>
        /// Contains configuration options for aggregation queue
        /// </summary>
        [DataMember]
        public AggregationConfiguration AggregationConfiguration
        {
            get { return _aggregationConfiguration; }
            set { _aggregationConfiguration = value; }
        }

        /// <summary>
        /// Contains configuration options for processing (outbound) queue
        /// </summary>
        [DataMember]
        public ProcessingConfiguration ProcessingConfiguration
        {
            get { return _processingConfiguration; }
            set { _processingConfiguration = value; }
        }

        /// <summary>
        /// Contains configuration options for processing (outbound) queue
        /// </summary>
        [DataMember]
        public JigsawIntegrationConfiguration JigsawIntegrationConfiguration
        {
            get { return _jigsawIntegartionConfiguration; }
            set { _jigsawIntegartionConfiguration = value; }
        }

        /// <summary>
        /// Place holder for any additional setting.
        /// </summary>
        [DataMember]
        public Collection<KeyValuePair<String, String>> AdditionalConfigurations
        {
            get { return _additionalConfigurations; }
            set { _additionalConfigurations = value; }
        }

        #endregion Properties

        #region Methods

        #region Public Methods

        /// <summary>
        /// Represents ConnectorProfile in Xml format
        /// </summary>
        public override String ToXml()
        {
            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);
            String xml = String.Empty;

            xmlWriter.WriteStartElement("ConnectorProfile");

            xmlWriter.WriteAttributeString("Id", this.Id.ToString());
            xmlWriter.WriteAttributeString("Name", this.Name);
            xmlWriter.WriteAttributeString("LongName", this.LongName);
            xmlWriter.WriteAttributeString("Weightage", this.Weightage.ToString());
            xmlWriter.WriteAttributeString("Enabled", this.Enabled.ToString().ToLowerInvariant());
            xmlWriter.WriteAttributeString("DefaultInboundIntegrationMessageTypeName", this.DefaultInboundIntegrationMessageTypeName);
            //xmlWriter.WriteAttributeString("ReferenceId", this.ReferenceId);
            //xmlWriter.WriteAttributeString("Action", this.Action.ToString());

            if (this.RunTimeSpecifications != null)
            {
                xmlWriter.WriteRaw(this.RunTimeSpecifications.ToXml());
            }

            if (this.QualificationConfiguration != null)
            {
                xmlWriter.WriteRaw(this.QualificationConfiguration.ToXml());
            }

            if (this.AggregationConfiguration != null)
            {
                xmlWriter.WriteRaw(this.AggregationConfiguration.ToXml());
            }

            if (this.ProcessingConfiguration != null)
            {
                xmlWriter.WriteRaw(this.ProcessingConfiguration.ToXml());
            }

            if (this.JigsawIntegrationConfiguration != null)
            {
                xmlWriter.WriteRaw(this.JigsawIntegrationConfiguration.ToXml());
            }

            xmlWriter.WriteStartElement("AdditionalConfigurations");

            if (this.AdditionalConfigurations != null)
            {
                foreach (KeyValuePair<String, String> configPair in this.AdditionalConfigurations)
                {
                    xmlWriter.WriteStartElement("Configuration");

                    xmlWriter.WriteAttributeString("Key", configPair.Key);
                    xmlWriter.WriteCData(configPair.Value);

                    //Configuration
                    xmlWriter.WriteEndElement();
                }
            }

            //AdditionalConfigurations
            xmlWriter.WriteEndElement();

            //ConnectorProfile end
            xmlWriter.WriteEndElement();

            xmlWriter.Flush();

            //get the actual XML
            xml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return xml;
        }

        /// <summary>
        /// Clone the ConnectorProfile object and return new instance with the same values.
        /// </summary>
        /// <returns>New cloned IConnectorProfile</returns>
        public IConnectorProfile Clone()
        {
            ConnectorProfile clonedProfile = new ConnectorProfile();
            clonedProfile.Id = this.Id;
            clonedProfile.Name = this.Name;
            clonedProfile.LongName = this.LongName;
            clonedProfile.Action = this.Action;
            clonedProfile.ReferenceId = this.ReferenceId;
            clonedProfile.Weightage = this.Weightage;
            clonedProfile.ProgramName = this.ProgramName;
            clonedProfile.RunTimeSpecifications = (RunTimeSpecifications)this.RunTimeSpecifications.Clone();
            clonedProfile.QualificationConfiguration = (QualificationConfiguration)this.QualificationConfiguration.Clone();
            clonedProfile.ProcessingConfiguration = (ProcessingConfiguration)this.ProcessingConfiguration;
            clonedProfile.AdditionalConfigurations = this.CloneAdditionalConfigurations();
            clonedProfile.AggregationConfiguration = (AggregationConfiguration)this.AggregationConfiguration.Clone();
            clonedProfile.JigsawIntegrationConfiguration = (JigsawIntegrationConfiguration)this.JigsawIntegrationConfiguration.Clone();
            return clonedProfile;
        }

        /// <summary>
        /// Get run time specification for connector
        /// </summary>
        /// <returns><see cref="IRunTimeSpecifications"/></returns>
        public IRunTimeSpecifications GetRunTimeSpecifications()
        {
            return this.RunTimeSpecifications;
        }

        /// <summary>
        /// Get qualification options for connector
        /// </summary>
        /// <returns><see cref="QualificationConfiguration"/></returns>
        public IQualificationConfiguration GetQualificationConfiguration()
        {
            return this.QualificationConfiguration;
        }

        /// <summary>
        /// Get aggregation options for connector
        /// </summary>
        /// <returns><see cref="IAggregationConfiguration"/></returns>
        public IAggregationConfiguration GetAggregationConfiguration()
        {
            return this.AggregationConfiguration;
        }

        /// <summary>
        /// Get processing configuration options for connector
        /// </summary>
        /// <returns><see cref="IProcessingConfiguration"/></returns>
        public IProcessingConfiguration GetProcessingConfiguration()
        {
            return this.ProcessingConfiguration;
        }

        /// <summary>
        /// Get additional configuration options for connector
        /// </summary>
        /// <returns>collection of additional configuration key and value</returns>
        public Collection<KeyValuePair<String, String>> GetAdditionalConfiguration()
        {
            return this.AdditionalConfigurations;
        }

        /// <summary>
        /// Add additional configuration values.
        /// Key must be unique.
        /// </summary>
        public void AddAdditionalConfiguration(String key, String value)
        {
            if (this.AdditionalConfigurations == null)
            {
                this.AdditionalConfigurations = new Collection<KeyValuePair<String, String>>();
            }

            KeyValuePair<String, String> existingPair = this.GetAdditionalConfigurationByKey(key);
            if (!String.IsNullOrWhiteSpace(existingPair.Key))
            {
                throw new ArgumentException(String.Format("AdditionalConfiguration with Key = '{0}' already exist."), key);
            }

            this.AdditionalConfigurations.Add(new KeyValuePair<String, String>(key, value));
        }

        /// <summary>
        /// Get AdditionalConfiguration value based on key
        /// </summary>
        /// <param name="key">Key of AdditionalConfiguration to search on</param>
        /// <returns>AdditionalConfiguration key-value pair having specified key</returns>
        public KeyValuePair<String, String> GetAdditionalConfigurationByKey(String key)
        {
            KeyValuePair<String, String> pair = new KeyValuePair<String, String>();

            if (this.AdditionalConfigurations != null)
            {
                pair = this.AdditionalConfigurations.Where(k => k.Key.Equals(key)).FirstOrDefault();
            }

            return pair;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectorProfile"></param>
        /// <param name="compareIds"></param>
        /// <returns></returns>
        public Boolean IsSuperSetOf(ConnectorProfile connectorProfile, Boolean compareIds = false)
        {
            if (connectorProfile != null)
            {
                if (base.IsSuperSetOf(connectorProfile, compareIds))
                {
                    if (compareIds == true)
                    {
                        if (this.Id != connectorProfile.Id)
                        {
                            return false;
                        }
                    }

                    if (this.Enabled != connectorProfile.Enabled)
                    {
                        return false;
                    }
                    if (this.Name != connectorProfile.Name)
                    {
                        return false;
                    }
                    if (this.Weightage != connectorProfile.Weightage)
                    {
                        return false;
                    }

                    if (!this.RunTimeSpecifications.IsSuperSetOf(connectorProfile.RunTimeSpecifications))
                    {
                        return false;
                    }
                    if (!this.QualificationConfiguration.IsSuperSetOf(connectorProfile.QualificationConfiguration))
                    {
                        return false;
                    }
                    if (!this.AggregationConfiguration.IsSuperSetOf(connectorProfile.AggregationConfiguration))
                    {
                        return false;
                    }
                    if (!this.ProcessingConfiguration.IsSuperSetOf(connectorProfile.ProcessingConfiguration))
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
            return true;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Initialize ConnectorProfile from xml.
        /// </summary>
        /// <param name="valuesAsXml">ConnectorProfile in xml format</param>
        private void LoadConnectorProfile(String valuesAsXml)
        {
            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                XmlTextReader reader = null;
                try
                {
                    reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);
                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "ConnectorProfile" && reader.IsStartElement())
                        {
                            #region Read ConnectorProfile Attributes

                            if (reader.HasAttributes)
                            {
                                if (reader.MoveToAttribute("Id"))
                                    this.Id = ValueTypeHelper.Int16TryParse(reader.ReadContentAsString(), -1);

                                if (reader.MoveToAttribute("ReferenceId"))
                                    this.ReferenceId = reader.ReadContentAsString();

                                if (reader.MoveToAttribute("Name"))
                                    this.Name = reader.ReadContentAsString();

                                if (reader.MoveToAttribute("LongName"))
                                    this.LongName = reader.ReadContentAsString();

                                if (reader.MoveToAttribute("Weightage"))
                                    this.Weightage = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), -1);

                                if (reader.MoveToAttribute("Enabled"))
                                    this.Enabled = ValueTypeHelper.BooleanTryParse(reader.ReadContentAsString(), true);

                                if (reader.MoveToAttribute("DefaultInboundIntegrationMessageTypeName"))
                                    this.DefaultInboundIntegrationMessageTypeName = reader.ReadContentAsString();

                                if (reader.MoveToAttribute("Action"))
                                {
                                    ObjectAction action = ObjectAction.Read;
                                    if (Enum.TryParse(reader.ReadContentAsString(), out action))
                                    {
                                        this.Action = action;
                                    }
                                }

                                reader.Read();
                            }

                            #endregion
                        }
                        else if (reader.NodeType == XmlNodeType.Element && reader.Name == "RunTimeSpecifications" && reader.IsStartElement())
                        {
                            String xml = reader.ReadOuterXml();
                            if (!String.IsNullOrWhiteSpace(xml))
                            {
                                RunTimeSpecifications spec = new RunTimeSpecifications(xml);
                                if (spec != null)
                                {
                                    this.RunTimeSpecifications = spec;
                                }
                            }
                        }
                        else if (reader.NodeType == XmlNodeType.Element && reader.Name == "QualificationConfiguration" && reader.IsStartElement())
                        {
                            String xml = reader.ReadOuterXml();
                            if (!String.IsNullOrWhiteSpace(xml))
                            {
                                QualificationConfiguration config = new QualificationConfiguration(xml);
                                if (config != null)
                                {
                                    this.QualificationConfiguration = config;
                                }
                            }
                        }
                        else if (reader.NodeType == XmlNodeType.Element && reader.Name == "AggregationConfiguration" && reader.IsStartElement())
                        {
                            String xml = reader.ReadOuterXml();
                            if (!String.IsNullOrWhiteSpace(xml))
                            {
                                AggregationConfiguration config = new AggregationConfiguration(xml);
                                if (config != null)
                                {
                                    this.AggregationConfiguration = config;
                                }
                            }
                        }
                        else if (reader.NodeType == XmlNodeType.Element && reader.Name == "ProcessingConfiguration" && reader.IsStartElement())
                        {
                            String xml = reader.ReadOuterXml();
                            if (!String.IsNullOrWhiteSpace(xml))
                            {
                                ProcessingConfiguration config = new ProcessingConfiguration(xml);
                                if (config != null)
                                {
                                    this.ProcessingConfiguration = config;
                                }
                            }
                        }
                        else if (reader.NodeType == XmlNodeType.Element && reader.Name == "JigsawIntegrationConfiguration" && reader.IsStartElement())
                        {
                            String xml = reader.ReadOuterXml();
                            if (!String.IsNullOrWhiteSpace(xml))
                            {
                                JigsawIntegrationConfiguration config = new JigsawIntegrationConfiguration(xml);
                                if (config != null)
                                {
                                    this.JigsawIntegrationConfiguration = config;
                                }
                            }
                        }
                        else if (reader.NodeType == XmlNodeType.Element && reader.Name == "AdditionalConfigurations" && reader.IsStartElement())
                        {
                            String xml = reader.ReadOuterXml();
                            if (!String.IsNullOrWhiteSpace(xml))
                            {
                                Collection<KeyValuePair<String, String>> configs = LoadAdditionalConfigurations(xml);
                                if (configs != null)
                                {
                                    this.AdditionalConfigurations = configs;
                                }
                            }
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="valuesAsXml"></param>
        /// <returns></returns>
        private Collection<KeyValuePair<String, String>> LoadAdditionalConfigurations(String valuesAsXml)
        {
            Collection<KeyValuePair<String, String>> additionalConfigs = new Collection<KeyValuePair<String, String>>();

            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                XmlTextReader reader = null;
                try
                {
                    reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "Configuration")
                        {
                            String configurationXml = reader.ReadOuterXml();

                            if (!String.IsNullOrEmpty(configurationXml))
                            {
                                KeyValuePair<String, String> cofiguration = LoadConfiguration(configurationXml);

                                if (cofiguration.Key.Length > 0 && cofiguration.Value.Length > 0)
                                {
                                    additionalConfigs.Add(cofiguration);
                                }
                            }
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

            return additionalConfigs;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="valuesAsXml"></param>
        /// <returns></returns>
        private KeyValuePair<String, String> LoadConfiguration(String valuesAsXml)
        {
            KeyValuePair<String, String> configuration = new KeyValuePair<String, String>();

            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                XmlTextReader reader = null;
                try
                {
                    reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                    while (!reader.EOF)
                    {
                        String key = String.Empty;
                        String value = String.Empty;

                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "Configuration")
                        {
                            #region Read Configurations

                            if (reader.GetAttribute("Key") != null)
                            {
                                key = reader.GetAttribute("Key");
                            }

                            value = reader.ReadElementContentAsString();

                            if (!String.IsNullOrWhiteSpace(key))
                            {
                                configuration = new KeyValuePair<String, String>(key, value);
                            }

                            #endregion
                        }

                        reader.Read();
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

            return configuration;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private Collection<KeyValuePair<String, String>> CloneAdditionalConfigurations()
        {
            Collection<KeyValuePair<String, String>> clonedConfig = new Collection<KeyValuePair<String, String>>();

            if (this.AdditionalConfigurations != null && this.AdditionalConfigurations.Count > 0)
            {
                foreach (KeyValuePair<String, String> pair in this.AdditionalConfigurations)
                {
                    KeyValuePair<String, String> clonedPair = new KeyValuePair<String, String>(pair.Key, pair.Value);
                    clonedConfig.Add(clonedPair);
                }
            }

            return clonedConfig;
        }

        #endregion Private Methods

        #endregion Methods
    }
}