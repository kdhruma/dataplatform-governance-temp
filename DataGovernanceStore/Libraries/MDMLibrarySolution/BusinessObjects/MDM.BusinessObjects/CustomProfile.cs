using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;

namespace MDM.BusinessObjects
{
    using MDM.Core;

    /// <summary>
    /// Custom profile
    /// </summary>
    [DataContract]
    public class CustomProfile : JobProfile
    {
        #region Fields

        /// <summary>
        /// Custom job type defined by custom implementer
        /// </summary>
        private String _customJobType = String.Empty;

        /// <summary>
        /// Name of the assembly which will be used to run the custom jobs which are created for this profile
        /// </summary>
        private string _jobProcessorAssemblyName = String.Empty;

        /// <summary>
        /// Name of the class which will be used to run custom job. This class must implement Riversand.JobService.Interfaces.IJob interface
        /// </summary>
        private string _jobProcessorClassName = String.Empty;

        /// <summary>
        /// A list of key-value pairs which can be used to store additional information for a profile
        /// </summary>
        private Dictionary<String, String> _profileParameters = new Dictionary<String,String>();

        /// <summary>
        /// A blob of additional data, e.g. an xml, which will be stored within profile xml as a node
        /// </summary>
        private String _additionalData = String.Empty;

        #endregion

        #region Constructors

        /// <summary>
        /// Parameterless Constructor
        /// </summary>
        public CustomProfile()
            : base()
        {
        }

        /// <summary>
        /// Constructor with XML having values of object.
        /// </summary>
        /// <param name="valuesAsXml">Indicates string having xml value</param>
        public CustomProfile(String valuesAsXml)
        {
            LoadCustomProfile(valuesAsXml);
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
                return "CustomProfile";
            }
        }

        /// <summary>
        /// Custom job type defined by custom implementer
        /// </summary>
        [DataMember]
        public String CustomJobType
        {
            get { return _customJobType; }
            set { _customJobType = value; }
        }

        /// <summary>
        /// Name of the assembly which will be used to run the custom jobs which are created for this profile
        /// </summary>
        [DataMember]
        public String JobProcessorAssemblyName
        {
            get { return _jobProcessorAssemblyName; }
            set { _jobProcessorAssemblyName = value; }
        }

        /// <summary>
        /// Name of the class which will be used to run custom job. This class must implement Riversand.JobService.Interfaces.IJob interface
        /// </summary>
        [DataMember]
        public String JobProcessorClassName
        {
            get
            {
                return this._jobProcessorClassName;
            }
            set
            {
                this._jobProcessorClassName = value;
            }
        }

        /// <summary>
        /// A list of key-value pairs which can be used to store additional information for a profile
        /// </summary>
        [DataMember]
        public Dictionary<String, String> ProfileParameters
        {
            get { return _profileParameters; }
            set { _profileParameters = value; }
        }

        /// <summary>
        /// A blob of additional data, e.g. an xml, which will be stored within profile xml as a node
        /// </summary>
        [DataMember]
        public String AdditionalData
        {
            get
            {
                return this._additionalData;
            }
            set
            {
                this._additionalData = value;
            }
        }

        #endregion

        #region Methods

        #region Load Methods

        ///<summary>
        /// Load CustomProfile object from XML.
        /// </summary>
        /// <param name="valuesAsXml">XML having xml value</param>
        /// <example>
        ///     Sample XML:
        /// </example>
        public void LoadCustomProfile(String valuesAsXml)
        {
            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                XmlTextReader reader = null;
                try
                {
                    reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "CustomProfile")
                        {
                            #region Read ImportProfile Properties

                            if (reader.HasAttributes)
                            {
                                if (reader.MoveToAttribute("Id"))
                                {
                                    this.Id = ValueTypeHelper.Int32TryParse( reader.ReadContentAsString(),0);
                                }

                                if (reader.MoveToAttribute("CustomJobType"))
                                {
                                    this.CustomJobType = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("JobProcessorAssemblyName"))
                                {
                                    this.JobProcessorAssemblyName = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("JobProcessorClassName"))
                                {
                                    this.JobProcessorClassName = reader.ReadContentAsString();
                                }
                                reader.Read();
                            }

                            #endregion
                        }
                        else if (reader.NodeType == XmlNodeType.Element && reader.Name == "ProfileParameters")
                        {
                            #region Read ProfileParameters

                            String profileParametersXml = reader.ReadOuterXml();
                            if (!String.IsNullOrEmpty(profileParametersXml))
                            {
                                this.LoadProfileParameters(profileParametersXml);
                            }

                            #endregion
                        }
                        else if (reader.NodeType == XmlNodeType.Element && reader.Name == "AdditionalData" && !reader.IsEmptyElement)
                        {
                            #region Read AdditionalData

                            String additionalData = reader.ReadInnerXml();//inner xml is our real additional data
                            if (!String.IsNullOrEmpty(additionalData))
                            {
                                this.AdditionalData = additionalData;
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
        /// Get Xml representation of Custom Profile
        /// </summary>
        /// <returns>Xml representation of object</returns>
        public new String ToXml()
        {
            String customProfileXml = String.Empty;
            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            //ImportProfile node start
            xmlWriter.WriteStartElement("CustomProfile");

            #region Write CustomProfile Properties

            xmlWriter.WriteAttributeString("Id", this.Id.ToString());
            xmlWriter.WriteAttributeString("Name", this.Name);
            xmlWriter.WriteAttributeString("CustomJobType", this.CustomJobType);
            xmlWriter.WriteAttributeString("JobProcessorAssemblyName", this.JobProcessorAssemblyName);
            xmlWriter.WriteAttributeString("JobProcessorClassName", this.JobProcessorClassName);

            #endregion

            #region Write ExecutionStepCollection

            xmlWriter.WriteStartElement("ProfileParameters");

            if (this.ProfileParameters != null)
            {
                foreach (String key in this.ProfileParameters.Keys)
                {
                    xmlWriter.WriteStartElement("ProfileParameter");

                    xmlWriter.WriteAttributeString("Name", key);
                    xmlWriter.WriteAttributeString("Value", this.ProfileParameters[key]);//TODO::Handle xml-sensitive characters

                    //ProfileParameter node end
                    xmlWriter.WriteEndElement();
                }
            }

            //ProfileParameters node end
            xmlWriter.WriteEndElement();

            #endregion

            //write AdditionalData
            xmlWriter.WriteStartElement("AdditionalData");
            xmlWriter.WriteRaw(this.AdditionalData);
            xmlWriter.WriteEndElement();//AdditionalData node end

            //CustomProfile node end
            xmlWriter.WriteEndElement();

            xmlWriter.Flush();

            //get the actual XML
            customProfileXml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return customProfileXml;
        }

       
        #endregion

        #region Private Methods

        private void LoadProfileParameters(String valuesAsXml)
        {

            #region Sample Xml
            /*
             * <ProfileParameters></ProfileParameters>
             */
            #endregion Sample Xml

            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                XmlTextReader reader = null;
                try
                {
                    reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "ProfileParameter")
                        {

                            #region Read ProfileParameter Name & Value

                            String name = String.Empty, value = String.Empty;

                            if (reader.HasAttributes)
                            {
                                if (reader.MoveToAttribute("Name"))
                                {
                                    name = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("Value"))
                                {
                                    value = reader.ReadContentAsString();
                                }

                                if (!String.IsNullOrEmpty(name))
                                {
                                    this.ProfileParameters.Add(name, value);
                                }

                                reader.Read();
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
        
        #endregion
    }
}
