using System;
using System.IO;
using System.Xml;
using System.Runtime.Serialization;
using System.Collections.ObjectModel;
using System.Linq;

namespace MDM.BusinessObjects
{
    using MDM.Core;
    using MDM.Interfaces;
 

    /// <summary>
    /// Specifies error object. This object is used to pass on errors from 1 layer to another layer.
    /// </summary>
    [DataContract]
    public class Error : ObjectBase, IError
    {
        #region Fields

        /// <summary>
        /// Field denoting code of the error.
        /// </summary>
        private String _errorCode = String.Empty;

        /// <summary>
        /// Field denoting actual error message
        /// </summary>
        private String _errorMessage = String.Empty;

        /// <summary>
        /// Field denoting additional params for error 
        /// </summary>
        private Collection<Object> _params = new Collection<Object>();

        /// <summary>
        /// Field denoting reference Id of the error.
        /// </summary>
        private String _referenceId = String.Empty;

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

        /// <summary>
        /// Indicates whether to ignore the error or not.
        /// </summary>
        private Boolean _ignoreError = false;

        #endregion

        #region Constructors

        /// <summary>
        /// Parameterless constructor
        /// </summary>
        public Error()
        {
        }

        /// <summary>
        /// Constructor with error code and error message as input parameter
        /// </summary>
        /// <param name="errorCode">Indicates code of the error</param>
        /// <param name="errorMessage">Indicates the error message</param>
        public Error(String errorCode, String errorMessage)
        {
            _errorCode = errorCode;
            _errorMessage = errorMessage;
        }

        /// <summary>
        /// Constructor with error code and error params as input parameters
        /// </summary>
        /// <param name="errorCode">Indicates error code</param>
        /// <param name="parameters">Indicates error Params</param>
        public Error(String errorCode, Collection<Object> parameters)
        {
            _errorCode = errorCode;
            _params = parameters;
        }

        /// <summary>
        /// Constructor with error code and error params as input parameters
        /// </summary>
        /// <param name="errorCode">Indicates error code</param>
        /// <param name="errorMessage">Indicates the error message</param>
        /// <param name="parameters">Indicates error Params</param>
        public Error(String errorCode, String errorMessage, Collection<Object> parameters)
        {
            _errorCode = errorCode;
            _errorMessage = errorMessage;
            _params = parameters;
        }

        /// <summary>
        ///  Constructor with error code and error params as input parameters
        /// </summary>
        /// <param name="errorCode">Indicates error code</param>
        /// <param name="errorMessage">Indicates the error message</param>
        /// <param name="parameters">Indicates error Params</param>
        /// <param name="reasonType">Indicates type of the reason.</param>
        /// <param name="ruleMapContextId">Indicates rule map context identifier.</param>
        /// <param name="ruleId">Indicates rule identifier.</param>
        public Error(String errorCode, String errorMessage, Collection<Object> parameters, ReasonType reasonType, Int32 ruleMapContextId, Int32 ruleId)
            : this(errorCode, errorMessage, parameters)
        {
            _reasonType = reasonType;
            _ruleMapContextId = ruleMapContextId;
            _ruleId = ruleId;
        }

        /// <summary>
        /// Initialize error from Xml
        /// </summary>
        /// <param name="valuesAsXml">Xml having value of error</param>
        public Error(String valuesAsXml)
        {
            LoadError(valuesAsXml);
        }

        /// <summary>
        /// Constructor with error code and error message as input parameter
        /// </summary>
        /// <param name="errorCode">Indicates code of the error</param>
        /// <param name="errorMessage">Indicates the error message</param>
        /// <param name="referenceId">Indicates the referenceId</param>
        public Error(String errorCode, String errorMessage, String referenceId)
        {
            _errorCode = errorCode;
            _errorMessage = errorMessage;
            _referenceId = referenceId;
        }
        #endregion

        #region Properties

        /// <summary>
        /// The Error Code for the operation failure
        /// </summary>
        [DataMember]
        public String ErrorCode
        {
            get
            {
                return _errorCode;
            }
            set
            {
                _errorCode = value;
            }

        }

        /// <summary>
        /// The Error Message for the operation failure
        /// </summary>
        [DataMember]
        public String ErrorMessage
        {
            get
            {
                return _errorMessage;
            }
            set
            {
                _errorMessage = value;
            }
        }

        /// <summary>
        /// property denoting additional params for error 
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
        /// Represents the error reference Id
        /// </summary>
        [DataMember]
        public String ReferenceId
        {
            get
            {
                return _referenceId;
            }
            set
            {
                _referenceId = value;
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

        /// <summary>
        ///  Property denotes whether to ignore the error or not.
        /// </summary>
        [DataMember]
        public Boolean IgnoreError
        {
            get
            {
                return this._ignoreError;
            }
            set
            {
                this._ignoreError = value;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Get Xml representation of Error
        /// </summary>
        /// <returns>Xml representation of Error object</returns>
        public String ToXml()
        {
            String errorXml = String.Empty;

            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            //Information node start
            xmlWriter.WriteStartElement("Error");

            xmlWriter.WriteAttributeString("Code", this.ErrorCode);
            xmlWriter.WriteAttributeString("Message", this.ErrorMessage);
            xmlWriter.WriteAttributeString("ReferenceId", this.ReferenceId);
            xmlWriter.WriteAttributeString("ReasonType", this.ReasonType.ToString());
            xmlWriter.WriteAttributeString("RuleMapContextId", this.RuleMapContextId.ToString());
            xmlWriter.WriteAttributeString("RuleId", this.RuleId.ToString());
            xmlWriter.WriteAttributeString("IgnoreError", this.IgnoreError.ToString());

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
            errorXml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return errorXml;
        }

        /// <summary>
        /// Get Xml representation of Error
        /// </summary>
        /// <returns>Xml representation of Error object</returns>
        public String ToXml(ObjectSerialization serialization)
        {
            String errorXml = String.Empty;

            if (serialization == ObjectSerialization.Full)
            {
                errorXml = this.ToXml();
            }
            else
            {
                StringWriter sw = new StringWriter();
                XmlTextWriter xmlWriter = new XmlTextWriter(sw);

                //Information node start
                xmlWriter.WriteStartElement("Error");

                if (serialization == ObjectSerialization.ProcessingOnly)
                {
                    xmlWriter.WriteAttributeString("Code", this.ErrorCode);
                    xmlWriter.WriteAttributeString("Message", this.ErrorMessage);
                }
                else if (serialization == ObjectSerialization.UIRender)
                {
                    xmlWriter.WriteAttributeString("Code", this.ErrorCode);
                    xmlWriter.WriteAttributeString("Message", this.ErrorMessage);
                }
                else if (serialization == ObjectSerialization.External)
                {
                    xmlWriter.WriteAttributeString("Code", this.ErrorCode);
                    xmlWriter.WriteAttributeString("Message", this.ErrorMessage);
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
                errorXml = sw.ToString();

                xmlWriter.Close();
                sw.Close();
            }
            return errorXml;
        }

        /// <summary>
        /// Initialize error from Xml
        /// </summary>
        /// <param name="valuesAsXml">Xml having value of error
        /// <para>
        /// Sample Xml
        /// <![CDATA[
        /// <Error Code="" Message = "EntityCreated BR failed">
        /// <Params>
        ///        <Param>Value1</Param>
        ///     </Params>
        /// </Error>
        /// ]]>
        /// </para>
        /// </param>
        public void LoadError(String valuesAsXml)
        {
            #region Sample Xml
            /*
             * <Error Code="" Message = "EntityCreated BR failed">
                 <Params>
                    <Param>Value1</Param>
                 </Params>
             * </Error>
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
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "Error")
                        {
                            #region Read error

                            if (reader.HasAttributes)
                            {
                                if (reader.MoveToAttribute("Code"))
                                {
                                    this.ErrorCode = reader.ReadContentAsString();
                                }
                                if (reader.MoveToAttribute("Message"))
                                {
                                    this.ErrorMessage = reader.ReadContentAsString();
                                }
                                if (reader.MoveToAttribute("ReferenceId"))
                                {
                                    this.ReferenceId = reader.ReadContentAsString();
                                }
                                if (reader.MoveToAttribute("ReasonType"))
                                {
                                    ReasonType reasonType;
                                    ValueTypeHelper.EnumTryParse(reader.ReadContentAsString(), true, out reasonType);
                                    this.ReasonType = reasonType;
                                }
                                if (reader.MoveToAttribute("RuleMapContextId"))
                                {
                                    this.RuleMapContextId = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), this._ruleMapContextId);
                                }
                                if (reader.MoveToAttribute("RuleId"))
                                {
                                    this.RuleId = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), this._ruleId);
                                }
                                if (reader.MoveToAttribute("IgnoreError"))
                                {
                                    this.IgnoreError = ValueTypeHelper.BooleanTryParse(reader.ReadContentAsString(), this._ignoreError);
                                }
                            }
                            else
                            {
                                reader.Read();
                            }

                            #endregion Read errors
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
        /// 
        /// </summary>
        /// <param name="subsetError"></param>
        /// <returns></returns>
        public Boolean IsSuperSetOf(Error subsetError)
        {
            if (this.ErrorCode != subsetError.ErrorCode)
            {
                return false;
            }

            if (this.ErrorMessage != subsetError.ErrorMessage)
            {
                return false;
            }

            Collection<Object> subsetErrorParams = subsetError.Params;
            Collection<Object> sourceErrorParams = this.Params;

            if (subsetErrorParams == null || sourceErrorParams == null)
            {
                return false;
            }

            if (subsetErrorParams != null && subsetErrorParams.Count > 0 && sourceErrorParams != null && sourceErrorParams.Count > 0)
            {
                if (this.Params.Count != subsetError.Params.Count)
                {
                    return false;
                }
                else
                {
                    // This will compare all the parameters which are available at same position
                    // If it will not match then it returns false, otherwise true
                    for (var index = 0; index < subsetErrorParams.Count; index++)
                    {
                        Object subsetParam = subsetErrorParams.ElementAt(index);
                        Object sourceParam = sourceErrorParams.ElementAt(index);

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