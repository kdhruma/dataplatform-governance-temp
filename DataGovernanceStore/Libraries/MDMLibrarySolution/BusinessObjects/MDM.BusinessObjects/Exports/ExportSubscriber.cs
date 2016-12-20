using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml;

namespace MDM.BusinessObjects.Exports
{
    using MDM.Core;
    using MDM.Interfaces.Exports;

    /// <summary>
    /// Represent class for export subscriber
    /// </summary>
    [DataContract]
    public class ExportSubscriber : MDMObject, IExportSubscriber
    {
        #region Fields

        /// <summary>
        /// Field denoting subscriber type for export
        /// </summary>
        private ExportSubscriberType _subscriberType = ExportSubscriberType.Unknown;

        /// <summary>
        /// Field denoting collection of configuration parameters for given type of subscriber. 
        /// These values can be different per subscriber type. So it is a key value collection of configuration parameters.
        /// </summary>
        private Collection<KeyValuePair<String, String>> _configurationParameters = new Collection<KeyValuePair<String, String>>();

        #endregion Fields

        #region Constructor

        /// <summary>
        /// Parameterless Constructor
        /// </summary>
        public ExportSubscriber()
            : base()
        {
        }

        /// <summary>
        /// Constructor with Id as input parameter
        /// <param name="id">Indicates the Identity of ExportSubscriber Instance</param>
        /// </summary>
        public ExportSubscriber(Int32 id)
            : base(id)
        {
        }

        /// <summary>
        /// Constructor with Xml as input
        /// <param name="valuesAsXml">Value in Xml format</param>
        /// </summary>
        public ExportSubscriber(String valuesAsXml)
        {
            LoadSubscribersFromXml(valuesAsXml);
        }

        #endregion Constructor

        #region Properties

        /// <summary>
        /// Property denoting subscriber type for export
        /// </summary>
        [DataMember]
        public ExportSubscriberType SubscriberType
        {
            get
            {
                return _subscriberType;
            }
            set
            {
                _subscriberType = value;
            }
        }

        /// <summary>
        /// Property denoting collection of configuration parameters for given type of subscriber. 
        /// These values can be different per subscriber type. So it is a key value collection of configuration parameters.
        /// </summary>
        [DataMember]
        public Collection<KeyValuePair<String, String>> ConfigurationParameters
        {
            get
            {
                return _configurationParameters;
            }
            set
            {
                _configurationParameters = value;
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Represents ExportSubscriber in Xml format
        /// </summary>
        public override String ToXml()
        {
            String xml = String.Empty;

            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);
            //xmlWriter.WriteStartDocument();

            //ExportSubscriber node start
            xmlWriter.WriteStartElement("ExportSubscriber");

            xmlWriter.WriteAttributeString("Id", this.Id.ToString());
            xmlWriter.WriteAttributeString("Name", this.Name);
            xmlWriter.WriteAttributeString("SubscriberType", this.SubscriberType.ToString());
            xmlWriter.WriteAttributeString("Action", this.Action.ToString());

            // ConfigurationParameters start
            xmlWriter.WriteStartElement("ConfigurationParameters");

            if(this.ConfigurationParameters != null && this.ConfigurationParameters.Count >0)
            {
                foreach(KeyValuePair<String, String> param in this.ConfigurationParameters)
                {
                    xmlWriter.WriteStartElement("ConfigurationParameter");
                    
                    xmlWriter.WriteAttributeString("Key" , param.Key);
                    xmlWriter.WriteAttributeString("Value" , param.Value);

                    xmlWriter.WriteEndElement();
                }
            }

            // ConfigurationParameters end
            xmlWriter.WriteEndElement();

            //ExportSubscriber node end
            xmlWriter.WriteEndElement();

            xmlWriter.Flush();
            //get the actual XML
            xml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return xml;
        }

        /// <summary>
        /// Represents ExportSubscriber in Xml format
        /// <param name="serialization">Type of serialization to be done</param>
        /// </summary>
        public override String ToXml(ObjectSerialization serialization)
        {
            return this.ToXml();
        }

        /// <summary>
        /// Get value of given key from configuration parameters
        /// </summary>
        /// <param name="configKeyName">Configuration key name</param>
        /// <returns>Value of given key</returns>
        public String GetConfigurationValue(String configKeyName)
        {
            String value = String.Empty;
            if (this.ConfigurationParameters != null)
            {
                var expectedConfig = from config in this.ConfigurationParameters
                                     where config.Key == configKeyName
                                     select config;

                if (expectedConfig.Any())
                {
                    value = ( ( KeyValuePair<String, String> )expectedConfig.FirstOrDefault()).Value;
                }
            }
            return value;
        }

        /// <summary>
        /// Get key-value pair based on given key
        /// </summary>
        /// <param name="configKeyName">Name of key to search</param>
        /// <returns>Key-Value pair for given key</returns>
        public KeyValuePair<String, String> GetConfiguration(String configKeyName)
        {
            KeyValuePair<String, String> configParam = new KeyValuePair<String, String>();
            if (this.ConfigurationParameters != null)
            {
                var expectedConfig = from config in this.ConfigurationParameters
                                     where config.Key == configKeyName
                                     select config;
                if (expectedConfig.Any())
                {
                    configParam = ( KeyValuePair<String, String> )expectedConfig.FirstOrDefault();
                }
            }
            return configParam;
        }

        /// <summary>
        /// Remove configuration based on given key name
        /// </summary>
        /// <param name="configKeyName">Key name to remove from configuration parameter collection</param>
        /// <returns>True if item is successfully removed; otherwise, false. This method also
        ///     returns false if item was not found in the original collection
        /// </returns>
        public Boolean RemoveConfiguration(String configKeyName)
        {
            Boolean result = false;
            if (this.ConfigurationParameters != null)
            {
                var expectedConfig = from config in this.ConfigurationParameters
                                     where config.Key == configKeyName
                                     select config;
                if (expectedConfig.Any())
                {
                    result = this.ConfigurationParameters.Remove(expectedConfig.FirstOrDefault());
                }
            }
            return result;
        }

        #endregion Methods

        #region Private methods

        private void LoadSubscribersFromXml(String valuesAsXml)
        {
            #region Sample Xml

            #endregion Sample Xml

            XmlTextReader reader = null;
            try
            {
                reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                while (!reader.EOF)
                {
                    if (reader.NodeType == XmlNodeType.Element && reader.Name == "ExportSubscriber")
                    {
                        #region Read ExportSubscriber properties

                        if (reader.HasAttributes)
                        {
                            if (reader.MoveToAttribute("Id"))
                            {
                                this.Id = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), 0);
                            }

                            if (reader.MoveToAttribute("Name"))
                            {
                                this.Name = reader.ReadContentAsString();
                            }

                            if (reader.MoveToAttribute("SubscriberType"))
                            {
                                ExportSubscriberType type = ExportSubscriberType.Unknown;
                                if (Enum.TryParse(reader.ReadContentAsString(), out type))
                                {
                                    this.SubscriberType = type;
                                }
                            }

                            if (reader.MoveToAttribute("Action"))
                            {
                                ObjectAction action = ObjectAction.Read;
                                if (Enum.TryParse(reader.ReadContentAsString(), out action))
                                {
                                    this.Action = action;
                                }
                            }
                        }

                        #endregion Read ExportSubscriber properties
                    }
                    else if (reader.NodeType == XmlNodeType.Element && reader.Name == "ConfigurationParameters")
                    {
                        String configParameters = reader.ReadOuterXml();
                        this.ConfigurationParameters = ReadConfigParameters(configParameters);
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

        private Collection<KeyValuePair<String, String>> ReadConfigParameters(String configParameters)
        {
            Collection<KeyValuePair<String, String>> configParam = new Collection<KeyValuePair<String, String>>();

            if (!String.IsNullOrWhiteSpace(configParameters))
            {
                XmlTextReader reader = null;
                try
                {
                    reader = new XmlTextReader(configParameters, XmlNodeType.Element, null);

                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "ConfigurationParameter")
                        {
                            String key = String.Empty;
                            String value = String.Empty;

                            if (reader.MoveToAttribute("Key"))
                            {
                                key = reader.ReadContentAsString();
                            }

                            if (reader.MoveToAttribute("Value"))
                            {
                                value = reader.ReadContentAsString();
                            }

                            if (!String.IsNullOrWhiteSpace(key))
                            {
                                configParam.Add(new KeyValuePair<String, String>(key, value));
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
                        reader.Close();
                }
            }

            return configParam;
        }

        #endregion Private methods
    }
}
