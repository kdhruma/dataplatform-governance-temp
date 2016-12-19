using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml;

namespace MDM.BusinessObjects.Integration
{
    using MDM.Interfaces;

    /// <summary>
    /// Class for integration data
    /// </summary>
    [DataContract]
    [KnownType(typeof(OperationResult))]
    public class IntegrationData : IIntegrationData
    {
        #region Fields

        /// <summary>
        /// Holds id for integration data
        /// </summary>
        private String _id = String.Empty;

        /// <summary>
        /// Holds data for Integration object
        /// </summary>
        [DataMember]
        private String _data = String.Empty;

        /// <summary>
        /// Place holder for any additional data.
        /// </summary>
        [DataMember]
        private Collection<KeyValuePair<String, String>> _additionalData = new Collection<KeyValuePair<String, String>>();

        #endregion

        #region Constructor

        /// <summary>
        /// Parameterless Constructor
        /// </summary>
        public IntegrationData()
            : base()
        {
        }

        /// <summary>
        /// Xml constructor
        /// </summary>
        /// <param name="valuesAsXml">Value in xml format</param>
        public IntegrationData(String valuesAsXml)
        {
            LoadIntegrationData(valuesAsXml);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Specificities id for integration data
        /// </summary>
        [DataMember]
        public String Id
        {
            get { return _id; }
            set { _id = value; }
        }

        #endregion

        #region Methods

        #region Public Methods

        /// <summary>
        /// Get XML representation of integration data
        /// </summary>
        /// <returns>XML representation of integration data</returns>
        public String ToXml()
        {
            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);
            String xml = String.Empty;

            xmlWriter.WriteStartElement("IntegrationData");
            xmlWriter.WriteAttributeString("Id", this.Id);

            xmlWriter.WriteStartElement("MessageData");

            String data = this._data == null ? String.Empty : this._data.ToString();
            xmlWriter.WriteCData(data);

            //End element for MessageData
            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement("AdditionalData");

            if (this._additionalData != null)
            {
                foreach (KeyValuePair<String, String> dataPair in this._additionalData)
                {
                    xmlWriter.WriteStartElement("Data");

                    xmlWriter.WriteAttributeString("Key", dataPair.Key);
                    xmlWriter.WriteCData(dataPair.Value);

                    //Configuration
                    xmlWriter.WriteEndElement();
                }
            }

            //AdditionalData
            xmlWriter.WriteEndElement();

            //IntegrationData end
            xmlWriter.WriteEndElement();

            xmlWriter.Flush();

            //get the actual XML
            xml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return xml;
        }

        /// <summary>
        /// Load integration data from XML.
        /// </summary>
        /// <param name="valuesAsXml">XML having xml value</param>
        public void LoadIntegrationData(String valuesAsXml)
        {
            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                XmlTextReader reader = null;
                try
                {
                    reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "IntegrationData" && reader.IsStartElement())
                        {
                            #region Read IntegrationData Attributes

                            if (reader.HasAttributes)
                            {
                                if (reader.MoveToAttribute("Id"))
                                    this._id = reader.ReadContentAsString();
                            }

                            #endregion
                        }
                        else if (reader.NodeType == XmlNodeType.Element && reader.Name == "MessageData" && reader.IsStartElement())
                        {
                            String xml = reader.ReadOuterXml();

                            if (!String.IsNullOrWhiteSpace(xml))
                            {
                                this._data = xml;
                            }
                        }
                        else if (reader.NodeType == XmlNodeType.Element && reader.Name == "AdditionalData" && reader.IsStartElement())
                        {
                            String xml = reader.ReadOuterXml();

                            if (!String.IsNullOrWhiteSpace(xml))
                            {
                                Collection<KeyValuePair<String, String>> additionalData = LoadAdditionalData(xml);

                                if (additionalData != null)
                                {
                                    this._additionalData = additionalData;
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
        /// Get integration data
        /// </summary>
        /// <returns></returns>
        public String GetData()
        {
            return _data;
        }

        /// <summary>
        /// Set integration data
        /// </summary>
        /// <param name="data">Holds value to be set</param>
        public void SetData(String data)
        {
            _data = data;
        }

        /// <summary>
        /// Get additional data for Integration object 
        /// </summary>
        /// <returns>collection of additional data - key and value pair collection</returns>
        public Collection<KeyValuePair<String, String>> GetAdditionalData()
        {
            return this._additionalData;
        }

        /// <summary>
        /// Add additional data - key and value pair.
        /// Key must be unique.
        /// </summary>
        /// <param name="key">Key of AdditionalData to be add</param>
        /// <param name="value">Value of AdditionalData to be add</param>
        public void AddAdditionalData(String key, String value)
        {
            if (this._additionalData == null)
            {
                this._additionalData = new Collection<KeyValuePair<String, String>>();
            }

            KeyValuePair<String, String> existingPair = GetAdditionalDataByKey(key);

            if (!String.IsNullOrWhiteSpace(existingPair.Key))
            {
                throw new ArgumentException(String.Format("AdditionalConfiguration with Key = '{0}' already exist."), key);
            }

            this._additionalData.Add(new KeyValuePair<String, String>(key, value));
        }

        /// <summary>
        /// Get AdditionalData based on key
        /// </summary>
        /// <param name="key">Key of AdditionalData to search on</param>
        /// <returns>AdditionlData key-value pair having specified key</returns>
        public String GetAdditionalData(String key)
        {
            String value = String.Empty;

            if(!String.IsNullOrWhiteSpace(key))
            {
                if (this._additionalData != null && this._additionalData.Count > 0)
                {
                    foreach (KeyValuePair<String, String> keyValuePair in this._additionalData)
                    {
                        if (keyValuePair.Key == key)
                        {
                            value = keyValuePair.Value;
                            break;
                        }
                    }
                }
            }

            return value;
        }

        /// <summary>
        /// Remove AdditionalData based on key
        /// </summary>
        /// <param name="key">Key of AdditionalData to be delete</param>
        /// <returns>true if item is successfully removed; otherwise, false.</returns>
        public Boolean RemoveAdditionalData(String key)
        {
            KeyValuePair<String,String> keyValuePair = GetAdditionalDataByKey(key);

            return this._additionalData.Remove(keyValuePair);
        }

        /// <summary>
        /// Check if additional data contains key value pair with given key
        /// </summary>
        /// <param name="key">key to search in additional data collection</param>
        /// <returns> true if key is found in the additional data collection; otherwise,false.</returns>
        public Boolean Contains(String key)
        {
            Boolean result = false;

            KeyValuePair<String, String> existingPair = GetAdditionalDataByKey(key);

            if (!String.IsNullOrWhiteSpace(existingPair.Key))
            {
                result = true;
            }

            return result;
        }

        /// <summary>
        /// Clone the IntegrationData object and return new instance with the same values.
        /// </summary>
        /// <returns>New cloned IIntegrationData</returns>
        public IIntegrationData Clone()
        {
            IntegrationData clonedIntegrationData = new IntegrationData();

            clonedIntegrationData._id = this._id;
            clonedIntegrationData._data = this._data;
            clonedIntegrationData._additionalData = this.CloneAdditionalData();

            return clonedIntegrationData;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private Collection<KeyValuePair<String, String>> CloneAdditionalData()
        {
            Collection<KeyValuePair<String, String>> clonedAdditionalData = new Collection<KeyValuePair<String, String>>();

            if (this._additionalData != null && this._additionalData.Count > 0)
            {
                foreach (KeyValuePair<String, String> pair in this._additionalData)
                {
                    KeyValuePair<String, String> clonedPair = new KeyValuePair<String, String>(pair.Key, pair.Value);
                    clonedAdditionalData.Add(clonedPair);
                }
            }

            return clonedAdditionalData;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="valuesAsXml"></param>
        /// <returns></returns>
        private Collection<KeyValuePair<String, String>> LoadAdditionalData(String valuesAsXml)
        {
            Collection<KeyValuePair<String, String>> additionalData = new Collection<KeyValuePair<String, String>>();

            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                XmlTextReader reader = null;
                try
                {
                    reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "AdditionalData")
                        {
                            String dataXml = reader.ReadOuterXml();

                            if (!String.IsNullOrEmpty(dataXml))
                            {
                                KeyValuePair<String, String> data = LoadData(dataXml);

                                if (data.Key.Length > 0 && data.Value.Length > 0)
                                {
                                    additionalData.Add(data);
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

            return additionalData;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="valuesAsXml"></param>
        /// <returns></returns>
        private KeyValuePair<String, String> LoadData(String valuesAsXml)
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

                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "Data")
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
        /// Get AdditionalData based on key
        /// </summary>
        /// <param name="key">Key of AdditionalData to search on</param>
        /// <returns>AdditionlData key-value pair having specified key</returns>
        private KeyValuePair<String, String> GetAdditionalDataByKey(String key)
        {
            KeyValuePair<String, String> pair = new KeyValuePair<String, String>();

            if (this._additionalData != null)
            {
                pair = this._additionalData.Where(k => k.Key.Equals(key)).FirstOrDefault();
            }

            return pair;
        }

        #endregion

        #endregion
    }
}