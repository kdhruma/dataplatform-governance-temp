using System;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using System.IO;
using System.Xml;
using System.Linq;

namespace MDM.BusinessObjects
{
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Represents the warning of operation result
    /// </summary>
    [DataContract]
    public class Warning : ObjectBase, IWarning
    {
        #region Fields

        /// <summary>
        /// Field denoting Warning code
        /// </summary>
        private String _warningCode = String.Empty;

        /// <summary>
        /// Field denoting Warning message
        /// </summary>
        private String _warningMessage = String.Empty;

        /// <summary>
        /// Field denoting additional params for Warning 
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
        public Warning()
        {
        }

        /// <summary>
        /// Constructor with Warning code and Warning message as input parameters
        /// </summary>
        /// <param name="WarningCode">Indicates Warning code</param>
        /// <param name="WarningMessage">Indicates Warning message</param>
        public Warning(String WarningCode, String WarningMessage)
        {
            _warningCode = WarningCode;
            _warningMessage = WarningMessage;
        }

        /// <summary>
        /// Constructor with Warning code and Warning params as input parameters
        /// </summary>
        /// <param name="WarningCode">Indicates Warning code</param>
        /// <param name="Params">Indicates Warning message</param>
        public Warning(String WarningCode, Collection<Object> Params)
        {
            _warningCode = WarningCode;
            _params = Params;
        }

        /// <summary>
        /// Constructor with Warning code and Warning params as input parameters
        /// </summary>
        /// <param name="warningCode">Indicates Warning code</param>
        /// <param name="warningMessage">Indicates Warning message</param>
        /// <param name="Params">Indicates Warning message</param>
        public Warning(String warningCode, String warningMessage, Collection<Object> Params)
        {
            _warningCode = warningCode;
            _warningMessage = warningMessage;
            _params = Params;
        }


        /// <summary>
        /// Constructor with Warning code and Warning params as input parameters
        /// </summary>
        /// <param name="warningCode">Indicates Warning code</param>
        /// <param name="warningMessage">Indicates Warning message</param>
        /// <param name="Params">Indicates Warning message</param>
        /// <param name="reasonType">Indicates type of the reason.</param>
        /// <param name="ruleMapContextId">Indicates rule map context identifier.</param>
        /// <param name="ruleId">Indicates rule identifier.</param>
        public Warning(String warningCode, String warningMessage, Collection<Object> Params, ReasonType reasonType, Int32 ruleMapContextId, Int32 ruleId)
            : this(warningCode, warningMessage, Params)
        {
            _reasonType = reasonType;
            _ruleMapContextId = ruleMapContextId;
            _ruleId = ruleId;
        }

        /// <summary>
        /// Initialize Warning from Xml
        /// </summary>
        /// <param name="valuesAsXml">Xml having value of Warning </param>
        public Warning(String valuesAsXml)
        {
            LoadWarning(valuesAsXml);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Property denoting the Warning Code for the operation
        /// </summary>
        [DataMember]
        public String WarningCode
        {
            get
            {
                return _warningCode;
            }
            set
            {
                _warningCode = value;
            }

        }

        /// <summary>
        /// Property denoting the Warning Message for the operation
        /// </summary>
        [DataMember]
        public String WarningMessage
        {
            get
            {
                return _warningMessage;
            }
            set
            {
                _warningMessage = value;
            }
        }

        /// <summary>
        /// property denoting additional params for Warning 
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
        /// Get Xml representation of Warning
        /// </summary>
        /// <returns>Xml representation of Warning object</returns>
        public String ToXml()
        {
            String WarningXml = String.Empty;

            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            //Warning node start
            xmlWriter.WriteStartElement("Warning");

            xmlWriter.WriteAttributeString("Code", this.WarningCode);
            xmlWriter.WriteAttributeString("Message", this.WarningMessage);
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

            //Warning node end
            xmlWriter.WriteEndElement();

            xmlWriter.Flush();

            //Get the actual XML
            WarningXml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return WarningXml;
        }

        /// <summary>
        /// Get Xml representation of Warning
        /// </summary>
        /// <returns>Xml representation of Warning object</returns>
        public String ToXml(ObjectSerialization serialization)
        {
            String WarningXml = String.Empty;

            if (serialization == ObjectSerialization.Full)
            {
                WarningXml = this.ToXml();
            }
            else
            {
                StringWriter sw = new StringWriter();
                XmlTextWriter xmlWriter = new XmlTextWriter(sw);

                //Warning node start
                xmlWriter.WriteStartElement("Warning");

                if (serialization == ObjectSerialization.ProcessingOnly)
                {
                    xmlWriter.WriteAttributeString("Code", this.WarningCode);
                    xmlWriter.WriteAttributeString("Message", this.WarningMessage);
                }
                else if (serialization == ObjectSerialization.UIRender)
                {
                    xmlWriter.WriteAttributeString("Code", this.WarningCode);
                    xmlWriter.WriteAttributeString("Message", this.WarningMessage);
                }
                else if (serialization == ObjectSerialization.External)
                {
                    xmlWriter.WriteAttributeString("Code", this.WarningCode);
                    xmlWriter.WriteAttributeString("Message", this.WarningMessage);
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

                //Warning node end
                xmlWriter.WriteEndElement();

                xmlWriter.Flush();

                //Get the actual XML
                WarningXml = sw.ToString();

                xmlWriter.Close();
                sw.Close();
            }
            return WarningXml;
        }

        /// <summary>
        /// Initialize Warning from Xml
        /// </summary>
        /// <param name="valuesAsXml">Xml having value of Warning
        /// <para>
        /// Sample Xml
        /// <![CDATA[
        /// <Warning Code="" Message = "Product description updated">
        ///     <Params>
        ///        <Param>Value1</Param>
        ///     </Params>
        ///  <Warning>
        /// ]]>
        /// </para>
        /// </param>
        public void LoadWarning(String valuesAsXml)
        {
            #region Sample Xml
            /*
             <Warning Code="" Message = "Product description updated">
             <Params>
                <Param>Value1</Param>
             </Params>
             <Warning>
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
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "Warning")
                        {
                            #region Read Warning

                            if (reader.HasAttributes)
                            {
                                if (reader.MoveToAttribute("Code"))
                                {
                                    this.WarningCode = reader.ReadContentAsString();
                                }
                                if (reader.MoveToAttribute("Message"))
                                {
                                    this.WarningMessage = reader.ReadContentAsString();
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
                            #endregion Read Warning
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
        /// Determines whether one object instance is superset of other
        /// </summary>
        /// <param name="subsetWarning">Indicates the warning subset instance which to be compared with the superset warning instance</param>
        /// <returns>Returns true if one object instance is superset of other; otherwise false</returns>
        public Boolean IsSuperSetOf(Warning subsetWarning)
        {
            if (this.WarningCode != subsetWarning.WarningCode)
            {
                return false;
            }

            if (this.WarningMessage != subsetWarning.WarningMessage)
            {
                return false;
            }

            Collection<Object> subsetWarningParams = subsetWarning.Params;
            Collection<Object> sourceWarningParams = this.Params;

            if (subsetWarningParams == null || sourceWarningParams == null)
            {
                return false;
            }

            if (subsetWarningParams != null && subsetWarningParams.Count > 0 && sourceWarningParams != null && sourceWarningParams.Count > 0)
            {
                if (this.Params.Count != subsetWarning.Params.Count)
                {
                    return false;
                }
                else
                {
                    // This will compare all the parameters which are available at same position
                    // If it will not match then it returns false, otherwise true
                    for (var index = 0; index < subsetWarningParams.Count; index++)
                    {
                        Object subsetParam = subsetWarningParams.ElementAt(index);
                        Object sourceParam = sourceWarningParams.ElementAt(index);

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