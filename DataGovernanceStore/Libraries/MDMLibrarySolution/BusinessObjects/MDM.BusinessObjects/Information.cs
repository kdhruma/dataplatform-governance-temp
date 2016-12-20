using System;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml;

namespace MDM.BusinessObjects
{
    using MDM.Core;
    using MDM.Interfaces;
    using System.Collections.ObjectModel;

    /// <summary>
    /// Represents the information of operation result
    /// </summary>
    [DataContract]
    public class Information : ObjectBase, IInformation
    {
        #region Fields

        /// <summary>
        /// Field denoting information code
        /// </summary>
        private String _informationCode = String.Empty;

        /// <summary>
        /// Field denoting information message
        /// </summary>
        private String _informationMessage = String.Empty;

        /// <summary>
        /// Field denoting additional params for information 
        /// </summary>
        private Collection<Object> _params = new Collection<Object>();

        /// <summary>
        ///  Indicates Reason type
        /// </summary>
        private ReasonType _reasonType = ReasonType.Unknown;

        /// <summary>
        /// Indicates unique identifier for rule map context  
        /// </summary>
        private Int32 _ruleMapContextId = -1;

        /// <summary>
        /// Indicates id for rule
        /// </summary>
        private Int32 _ruleId = -1;

        #endregion

        #region Constructors

        /// <summary>
        /// Parameterless constructor
        /// </summary>
        public Information()
        {
        }

        /// <summary>
        /// Constructor with information code and information message as input parameters
        /// </summary>
        /// <param name="informationCode">Indicates information code</param>
        /// <param name="informationMessage">Indicates information message</param>
        public Information(String informationCode, String informationMessage)
        {
            _informationCode = informationCode;
            _informationMessage = informationMessage;
        }

        /// <summary>
        /// Constructor with information code and information params as input parameters
        /// </summary>
        /// <param name="informationCode">Indicates information code</param>
        /// <param name="Params">Indicates information message</param>
        public Information(String informationCode, Collection<Object> Params)
        {
            _informationCode = informationCode;
            _params = Params;
        }

        /// <summary>
        /// Constructor with information code and information params as input parameters
        /// </summary>
        /// <param name="informationCode">Indicates information code</param>
        /// <param name="informationMessage">Indicates information message</param>
        /// <param name="Params">Indicates information message</param>
        public Information(String informationCode, String informationMessage, Collection<Object> Params)
        {
            _informationCode = informationCode;
            _informationMessage = informationMessage;
            _params = Params;
        }

        /// <summary>
        /// Constructor with information code and information params as input parameters
        /// </summary>
        /// <param name="informationCode">Indicates information code</param>
        /// <param name="informationMessage">Indicates information message</param>
        /// <param name="Params">Indicates information message</param>
        /// <param name="reasonType">Indicates type of the reason.</param>
        /// <param name="ruleMapContextId">Indicates rule map context identifier.</param>
        /// <param name="ruleId">Indicates rule identifier.</param>
        public Information(String informationCode, String informationMessage, Collection<Object> Params, ReasonType reasonType, Int32 ruleMapContextId, Int32 ruleId)
            : this(informationCode, informationMessage, Params)
        {
            _reasonType = reasonType;
            _ruleMapContextId = ruleMapContextId;
            _ruleId = ruleId;
        }

        /// <summary>
        /// Initialize information from Xml
        /// </summary>
        /// <param name="valuesAsXml">Xml having value of information </param>
        public Information(String valuesAsXml)
        {
            LoadInformation(valuesAsXml);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Property denoting the Information Code for the operation
        /// </summary>
        [DataMember]
        public String InformationCode
        {
            get
            {
                return _informationCode;
            }
            set
            {
                _informationCode = value;
            }

        }

        /// <summary>
        /// Property denoting the Information Message for the operation
        /// </summary>
        [DataMember]
        public String InformationMessage
        {
            get
            {
                return _informationMessage;
            }
            set
            {
                _informationMessage = value;
            }
        }

        /// <summary>
        /// property denoting additional params for information 
        /// </summary>
        [DataMember]
        public Collection<Object> Params
        {
            get
            {
                return _params;
            }
            set
            {
                _params = value;
            }
        }

        /// <summary>
        /// Indicates unique identifier for rule map context
        /// </summary>
        [DataMember]
        public Int32 RuleMapContextId
        {
            get
            {
                return this._ruleMapContextId;
            }
            set
            {
                this._ruleMapContextId = value;
            }
        }

        /// <summary>
        /// Indicates id for rule 
        /// </summary>
        [DataMember]
        public Int32 RuleId
        {
            get
            {
                return this._ruleId;
            }
            set
            {
                this._ruleId = value;
            }
        }

        /// <summary>
        ///  Indicates reason type
        /// </summary>
        [DataMember]
        public ReasonType ReasonType
        {
            get
            {
                return this._reasonType;
            }
            set
            {
                this._reasonType = value;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Get Xml representation of Information
        /// </summary>
        /// <returns>Xml representation of Information object</returns>
        public String ToXml()
        {
            String informationXml = String.Empty;

            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            //Information node start
            xmlWriter.WriteStartElement("Information");

            xmlWriter.WriteAttributeString("Code", this.InformationCode);
            xmlWriter.WriteAttributeString("Message", this.InformationMessage);
            xmlWriter.WriteAttributeString("ReasonType", this.ReasonType.ToString());
            xmlWriter.WriteAttributeString("RuleMapContextId", this.RuleMapContextId.ToString());
            xmlWriter.WriteAttributeString("RuleId", this.RuleId.ToString());

            #region Write Params

            xmlWriter.WriteStartElement("Params");

            if (this.Params != null)
            {
                foreach (Object str in this.Params)
                {
                    xmlWriter.WriteStartElement("Param");
                    xmlWriter.WriteCData(str.ToString());
                    //Param Node end
                    xmlWriter.WriteEndElement();
                }
            }
            //Params node end
            xmlWriter.WriteEndElement();

            #endregion

            //Information node end
            xmlWriter.WriteEndElement();

            xmlWriter.Flush();

            //Get the actual XML
            informationXml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return informationXml;
        }

        /// <summary>
        /// Get Xml representation of Information
        /// </summary>
        /// <returns>Xml representation of Information object</returns>
        public String ToXml(ObjectSerialization serialization)
        {
            String informationXml = String.Empty;

            if (serialization == ObjectSerialization.Full)
            {
                informationXml = this.ToXml();
            }
            else
            {
                StringWriter sw = new StringWriter();
                XmlTextWriter xmlWriter = new XmlTextWriter(sw);

                //Information node start
                xmlWriter.WriteStartElement("Information");

                if (serialization == ObjectSerialization.ProcessingOnly)
                {
                    xmlWriter.WriteAttributeString("Code", this.InformationCode);
                    xmlWriter.WriteAttributeString("Message", this.InformationMessage);
                }
                else if (serialization == ObjectSerialization.UIRender)
                {
                    xmlWriter.WriteAttributeString("Code", this.InformationCode);
                    xmlWriter.WriteAttributeString("Message", this.InformationMessage);
                }
                else if (serialization == ObjectSerialization.External)
                {
                    xmlWriter.WriteAttributeString("Code", this.InformationCode);
                    xmlWriter.WriteAttributeString("Message", this.InformationMessage);
               }

                #region Write Params

                xmlWriter.WriteStartElement("Params");

                if (this.Params != null)
                {
                    foreach (Object str in this.Params)
                    {
                        xmlWriter.WriteStartElement("Param");
                        xmlWriter.WriteCData(str.ToString());
                        //Param Node end
                        xmlWriter.WriteEndElement();
                    }
                }
                //Params node end
                xmlWriter.WriteEndElement();

                #endregion

                //Information node end
                xmlWriter.WriteEndElement();

                xmlWriter.Flush();

                //Get the actual XML
                informationXml = sw.ToString();

                xmlWriter.Close();
                sw.Close();
            }
            return informationXml;
        }

        /// <summary>
        /// Initialize information from Xml
        /// </summary>
        /// <param name="valuesAsXml">Xml having value of information
        /// <para>
        /// Sample Xml
        /// <![CDATA[
        /// <Information Code="" Message = "Product description updated">
        ///     <Params>
        ///        <Param>Value1</Param>
        ///     </Params>
        ///  <Information>
        /// ]]>
        /// </para>
        /// </param>
        public void LoadInformation(String valuesAsXml)
        {
            #region Sample Xml
            /*
             <Information Code="" Message = "Product description updated">
             <Params>
                <Param>Value1</Param>
             </Params>
             <Information>
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
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "Information")
                        {
                            #region Read Information

                            if (reader.HasAttributes)
                            {
                                if (reader.MoveToAttribute("Code"))
                                {
                                    this.InformationCode = reader.ReadContentAsString();
                                }
                                if (reader.MoveToAttribute("Message"))
                                {
                                    this.InformationMessage = reader.ReadContentAsString();
                                }
                                if (reader.MoveToAttribute("Params"))
                                {
                                    this.Params.Add(reader.ReadContentAsString());
                                }

                                if (reader.MoveToAttribute("ReasonType"))
                                {
                                    ReasonType reasonType;
                                    ValueTypeHelper.EnumTryParse(reader.ReadContentAsString(), true, out reasonType);
                                    this.ReasonType = reasonType;
                                }

                                if (reader.MoveToAttribute("RuleMapContextId"))
                                {
                                    this.RuleMapContextId = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), -1);
                                }

                                if (reader.MoveToAttribute("RuleId"))
                                {
                                    this.RuleId = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), this._ruleId);
                                }
                            }
                            else
                            {
                                reader.Read();
                            }
                            #endregion Read Information
                        }
                        else if (reader.NodeType == XmlNodeType.Element && reader.Name == "Param")
                        {
                            #region Read Params

                            if (!reader.IsEmptyElement)
                            {
                                this.Params.Add(reader.ReadString());
                            }
                            else
                            {
                                this.Params.Add(String.Empty);
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

        /// <summary>
        /// Determine whether the current information object is the superset of the specified information object passed or not.
        /// </summary>
        /// <param name="subsetInformation">Indicates the information which needs to be compared</param>
        /// <returns>Returns a boolean flag indicating whether the current information object is the superset of the specified information object or not</returns>
        public Boolean IsSuperSetOf(Information subsetInformation)
        {
            if (this.InformationCode != subsetInformation.InformationCode)
            {
                return false;
            }

            if (this.InformationMessage != subsetInformation.InformationMessage)
            {
                return false;
            }

            Collection<Object> subsetInformationParams = subsetInformation.Params;
            Collection<Object> sourceInformationParams = this.Params;

            if (subsetInformationParams == null || sourceInformationParams == null)
            {
                return false;
            }

            if (subsetInformationParams != null && subsetInformationParams.Count > 0 && sourceInformationParams != null && sourceInformationParams.Count > 0)
            {
                if (this.Params.Count != subsetInformation.Params.Count)
                {
                    return false;
                }
                else
                {
                    // This will compare all the parameters which are available at same position
                    // If it will not match then it returns false, otherwise true
                    for (var index = 0; index < subsetInformationParams.Count; index++)
                    {
                        Object subsetParam = subsetInformationParams.ElementAt(index);
                        Object sourceParam = sourceInformationParams.ElementAt(index);

                        if (String.Compare(sourceParam.ToString(), subsetParam.ToString()) != 0)
                        {
                            return false;
                        }
                    }
                }
            }

            return true;
        }

        #endregion
    }
}