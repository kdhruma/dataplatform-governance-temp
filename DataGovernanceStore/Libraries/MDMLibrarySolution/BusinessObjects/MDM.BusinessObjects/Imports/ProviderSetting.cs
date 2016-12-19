using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.IO;
using System.Runtime.Serialization;
using System.Collections.ObjectModel;

namespace MDM.BusinessObjects.Imports
{
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Specifies the Provider Setting
    /// </summary>
    [DataContract]
    public sealed class ProviderSetting : ObjectBase, IProviderSetting
    {
        #region Fields

        /// <summary>
        /// Represents the value of the parameter
        /// </summary>
        private String _name = String.Empty;

        /// <summary>
        /// Represents the value of the parameter
        /// </summary>
        private String _value = String.Empty;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the Parameter class.
        /// </summary>
        public ProviderSetting(String name, String value)
        {
            this.Name = name;
            this.Value = value;
        }

        /// <summary>
        /// Initializes a new instance of the Parameter class.
        /// </summary>
        public ProviderSetting(String valuesAsXml)
        {
            LoadProviderSetting(valuesAsXml);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Represents the name of the parameter
        /// </summary>
        [DataMember]
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
        /// Represents the value of the parameter
        /// </summary>
        [DataMember]
        public String Value
        {
            get
            {
                return this._value;
            }
            set
            {
                this._value = value;
            }
        }

        #endregion

        #region Methods

        #region Load Methods

        ///<summary>
        /// Load ProviderSetting object from XML.
        /// </summary>
        /// <param name="valuesAsXml">XML having xml value</param>
        /// <example>
        ///     Sample XML:
        /// </example>
        public void LoadProviderSetting(String valuesAsXml)
        {
            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                XmlTextReader reader = null;
                try
                {
                    reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "ProviderSetting")
                        {
                            #region Read ProviderSetting Properties

                            if (reader.HasAttributes)
                            {
                                if (reader.MoveToAttribute("Name"))
                                {
                                    this.Name = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("Value"))
                                {
                                    this.Value = reader.ReadContentAsString();
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
        /// Get Xml representation of JobData
        /// </summary>
        /// <returns>Xml representation of object</returns>
        public String ToXml()
        {
            String jobParameterXml = String.Empty;
            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            //ProviderSetting node start
            xmlWriter.WriteStartElement("ProviderSetting");

            #region Write Properties

            xmlWriter.WriteAttributeString("Name", this.Name);
            xmlWriter.WriteAttributeString("Value", this.Value);            

            #endregion

            //ProviderSetting node end
            xmlWriter.WriteEndElement();

            xmlWriter.Flush();

            //get the actual XML
            jobParameterXml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return jobParameterXml;
        }

        /// <summary>
        /// Get Xml representation of JobData
        /// </summary>
        /// <returns>Xml representation of object</returns>
        public String ToXml(ObjectSerialization serialization)
        {
            String jobParameterXml = String.Empty;

            if (serialization == ObjectSerialization.Full)
            {
                jobParameterXml = this.ToXml();
            }
            else
            {
                StringWriter sw = new StringWriter();
                XmlTextWriter xmlWriter = new XmlTextWriter(sw);

                //ProviderSetting node start
                xmlWriter.WriteStartElement("ProviderSetting");

                if (serialization == ObjectSerialization.ProcessingOnly)
                {
                    // TODO : Need to decide which all properties are needed for processing Xml.
                    // currently returning all properties.

                    #region Write ProviderSetting Properties for ProcessingOnly ProviderSetting Xml

                    xmlWriter.WriteAttributeString("Name", this.Name);
                    xmlWriter.WriteAttributeString("Value", this.Value); 

                    #endregion
                }
                else if (serialization == ObjectSerialization.UIRender)
                {
                    // TODO : Need to decide which all properties are needed for Rendering Xml.
                    // currently returning all properties.

                    #region Write JobData Properties for rendering JobData Xml

                    xmlWriter.WriteAttributeString("Name", this.Name);
                    xmlWriter.WriteAttributeString("Value", this.Value); 

                    #endregion
                }

                //ProviderSetting node end
                xmlWriter.WriteEndElement();

                xmlWriter.Flush();

                //get the actual XML
                jobParameterXml = sw.ToString();

                xmlWriter.Close();
                sw.Close();
            }

            return jobParameterXml;
        }

        #endregion

        #region Private Methods

        #endregion

        #endregion
    }
}
