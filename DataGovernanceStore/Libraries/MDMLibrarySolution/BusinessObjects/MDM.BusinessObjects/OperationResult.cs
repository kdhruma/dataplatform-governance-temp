using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml;

namespace MDM.BusinessObjects
{
    using MDM.BusinessObjects.DataModel;
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Specifies the operation result object. This object can be used to communicate the result from 1 layer to another.
    /// <para>
    /// For example, if EntityBL is calling some method from AttributeManager.AttributeBL, we can add the results into OperationResult object in AttributeBL's method and can check it in EntityBL's method.
    /// Mostly errors are communicated in this way.
    /// </para>
    /// </summary>
    [DataContract]
    [KnownType(typeof(Error))]
    [KnownType(typeof(Information))]
    [KnownType(typeof(Table))]
    [KnownType(typeof(Dictionary<Int64, Int64>))]
    [KnownType(typeof(Dictionary<String, String>))]
    [KnownType(typeof(Dictionary<Int64, String>))]
    [KnownType(typeof(Dictionary<String, Int64>))]
    [KnownType(typeof(EntityOperationResult))]
    [KnownType(typeof(DataModelOperationResult))]
    [KnownType(typeof(SecurityUser))]
    [KnownType(typeof(BusinessRuleOperationResult))]
    [KnownType(typeof(AttributeDataType))]
    public class OperationResult : ObjectBase, IOperationResult
    {
        #region Fields

        /// <summary>
        /// Determines Id of operation Result
        /// </summary>
        private Int32 _id;

        /// <summary>
        /// Reference Id
        /// </summary>
        private String _referenceId;

        /// <summary>
        /// Indicates the action that was performed.
        /// </summary>
        private ObjectAction _performedAction = ObjectAction.Unknown;

        /// <summary>
        /// Field denoting errors for current operation
        /// </summary>
        private ErrorCollection _errors = new ErrorCollection();

        /// <summary>
        /// Field denoting information about current operation
        /// </summary>
        private InformationCollection _informations = new InformationCollection();

        /// <summary>
        /// Field denoting warnings about current operation
        /// </summary>
        private WarningCollection _warnings = new WarningCollection();

        /// <summary>
        /// Field denoting the collection of return objects
        /// </summary>
        private Collection<Object> _returnValues = new Collection<Object>();

        /// <summary>
        /// Indicates staus of Operation.
        /// </summary>
        private OperationResultStatusEnum _operationResultStatus = OperationResultStatusEnum.None;

        /// <summary>
        /// Field used for storing extra information for the Operation
        /// </summary>
        private Hashtable _extendedProperties = new Hashtable();

        #endregion

        #region Constructors

        /// <summary>
        /// Parameter less constructor
        /// </summary>
        public OperationResult()
        {
        }

        /// <summary>
        /// Initialize operation result from Xml
        /// </summary>
        /// <param name="valuesAsXml">Xml having values for Operation result</param>
        public OperationResult(String valuesAsXml)
        {
            LoadOperationResult(valuesAsXml);
        }
        #endregion

        #region Properties

        /// <summary>
        /// Determines Id of operation Result
        /// </summary>
        [DataMember]
        public Int32 Id
        {
            get
            {
                return this._id;
            }
            set
            {
                this._id = value;
            }
        }

        /// <summary>
        /// Reference Id
        /// </summary>
        [DataMember]
        public string ReferenceId
        {
            get
            {
                return this._referenceId;
            }
            set
            {
                this._referenceId = value;
            }
        }

        /// <summary>
        /// Performed Action
        /// </summary>
        [DataMember]
        public ObjectAction PerformedAction
        {
            get
            {
                return this._performedAction;
            }
            set
            {
                this._performedAction = value;
            }
        }

        /// <summary>
        /// Property denoting errors for current operation
        /// </summary>
        [DataMember]
        public ErrorCollection Errors
        {
            get
            {
                return _errors;
            }
            set
            {
                _errors = value;
            }
        }

        /// <summary>
        /// Property denoting the collection of return objects
        /// </summary>
        [DataMember]
        public Collection<Object> ReturnValues
        {
            get
            {
                return _returnValues;
            }
            set
            {
                _returnValues = value;
            }
        }

        /// <summary>
        /// Property denoting whether there are any error messages
        /// </summary>
        public Boolean HasError
        {
            get
            {
                return (this.Errors != null && this.Errors.Count > 0) ? true : false;
            }
        }

        /// <summary>
        /// Property denoting Information for current operation
        /// </summary>
        [DataMember]
        public InformationCollection Informations
        {
            get
            {
                return _informations;
            }
            set
            {
                _informations = value;
            }
        }

        /// <summary>
        /// Property denoting whether there are any Information messages
        /// </summary>
        public Boolean HasInformation
        {
            get
            {
                return (this.Informations != null && this.Informations.Count > 0) ? true : false;
            }
        }

        /// <summary>
        /// Property denoting Warning for current operation
        /// </summary>
        [DataMember]
        public WarningCollection Warnings
        {
            get
            {
                return _warnings;
            }
            set
            {
                _warnings = value;
            }
        }

        /// <summary>
        /// Property denoting whether there are any warning messages
        /// </summary>
        public Boolean HasWarnings
        {
            get
            {
                return (this.Warnings != null && this.Warnings.Count > 0) ? true : false;
            }
        }

        /// <summary>
        /// Indicates overall status of Operation result. Default Value is success.
        /// </summary>
        [DataMember]
        public OperationResultStatusEnum OperationResultStatus
        {
            get
            {
                return _operationResultStatus;
            }
            set
            {
                _operationResultStatus = value;
            }
        }

        /// <summary>
        /// Property used for storing extra information for 
        /// </summary>
        [DataMember]
        public Hashtable ExtendedProperties
        {
            get
            {
                return this._extendedProperties;
            }
            set
            {
                this._extendedProperties = value;
            }
        }

        #endregion

        #region Methods

        #region Public Methods

        /// <summary>
        /// Get Xml representation of Operation Result
        /// </summary>
        /// <returns>Xml representation of Operation Result object</returns>
        public String ToXml()
        {
            String operationResultXml = String.Empty;

            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            //Operation result node start
            xmlWriter.WriteStartElement("OperationResult");
            xmlWriter.WriteAttributeString("PerformedAction", this.PerformedAction.ToString());

            #region Write Errors

            //Add Error Nodes
            xmlWriter.WriteStartElement("Errors");

            foreach (Error error in this.Errors)
            {
                xmlWriter.WriteRaw(error.ToXml());
            }

            //Error nodes end
            xmlWriter.WriteEndElement();

            #endregion Write Errors

            #region Write Information

            //Add Information Nodes
            xmlWriter.WriteStartElement("Informations");

            foreach (Information information in this.Informations)
            {
                xmlWriter.WriteRaw(information.ToXml());
            }

            //Information node end
            xmlWriter.WriteEndElement();

            #endregion Write Information

            #region Write Warnings

            //Add Warning Nodes
            xmlWriter.WriteStartElement("Warnings");

            foreach (Warning warnings in this.Warnings)
            {
                xmlWriter.WriteRaw(warnings.ToXml());
            }

            //Warning node end
            xmlWriter.WriteEndElement();

            #endregion Write Information

            #region Write return values

            //Add Return Values Nodes
            xmlWriter.WriteStartElement("ReturnValues");

            foreach (Object returnValue in this.ReturnValues)
            {
                xmlWriter.WriteStartElement("ReturnValue");
                xmlWriter.WriteRaw(returnValue.ToString());
                xmlWriter.WriteEndElement();
            }

            //Return values node end
            xmlWriter.WriteEndElement();

            #endregion Write return values

            #region Write Operation Result Status

            //Add Return Values Nodes
            xmlWriter.WriteStartElement("OperationResultStatus");
            xmlWriter.WriteRaw(OperationResultStatus.ToString());
            //Return values node end
            xmlWriter.WriteEndElement();

            #endregion Write return values

            #region write Extended properties
            xmlWriter.WriteStartElement("ExtendedProperties");

            if (this.ExtendedProperties != null)
            {
                foreach (String key in this.ExtendedProperties.Keys)
                {
                    //ExtendedProperty node start
                    xmlWriter.WriteStartElement("ExtendedProperty");

                    xmlWriter.WriteAttributeString("Key", key);
                    if (this.ExtendedProperties[key] != null)
                    {
                        xmlWriter.WriteCData(this.ExtendedProperties[key].ToString());
                    }
                    //ExtendedProperty node end
                    xmlWriter.WriteEndElement();
                }
            }
            //ExtendedProperties node end
            xmlWriter.WriteEndElement();
            #endregion write Extended properties

            //Operation result node end
            xmlWriter.WriteEndElement();

            xmlWriter.Flush();

            //Get the actual XML
            operationResultXml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return operationResultXml;
        }

        /// <summary>
        /// Get Xml representation of Operation Result
        /// </summary>
        /// <returns>Xml representation of Operation Result object</returns>
        public String ToXml(ObjectSerialization serialization)
        {
            String operationResultXml = String.Empty;

            if (serialization == ObjectSerialization.Full)
            {
                operationResultXml = this.ToXml();
            }
            else
            {
                StringWriter sw = new StringWriter();
                XmlTextWriter xmlWriter = new XmlTextWriter(sw);

                //Operation result node start
                xmlWriter.WriteStartElement("OperationResult");

                #region Write Errors

                //Add Error Nodes
                xmlWriter.WriteStartElement("Errors");

                if (this.Errors != null)
                {
                    foreach (Error error in this.Errors)
                    {
                        xmlWriter.WriteRaw(error.ToXml(serialization));
                    }
                }

                //Error nodes end
                xmlWriter.WriteEndElement();

                #endregion Write Errors

                #region Write Information

                //Add Information Nodes
                xmlWriter.WriteStartElement("Informations");

                if (this.Informations != null)
                {
                    foreach (Information information in this.Informations)
                    {
                        xmlWriter.WriteRaw(information.ToXml(serialization));
                    }
                }

                //Information node end
                xmlWriter.WriteEndElement();

                #endregion Write Information

                #region Write Warnings

                //Add Warning Nodes
                xmlWriter.WriteStartElement("Warnings");

                foreach (Warning warnings in this.Warnings)
                {
                    xmlWriter.WriteRaw(warnings.ToXml(serialization));
                }

                //Warning node end
                xmlWriter.WriteEndElement();

                #endregion Write Information

                #region Write return values

                //Add Return Values Nodes
                xmlWriter.WriteStartElement("ReturnValues");

                if (this.ReturnValues != null)
                {
                    foreach (Object returnValue in this.ReturnValues)
                    {
                        xmlWriter.WriteStartElement("ReturnValue");
                        xmlWriter.WriteRaw(returnValue.ToString());
                        xmlWriter.WriteEndElement();
                    }
                }

                //Return values node end
                xmlWriter.WriteEndElement();

                #endregion Write return values

                #region Write Operation Result Status

                //Add OperationResultStatus Nodes
                xmlWriter.WriteStartElement("OperationResultStatus");
                xmlWriter.WriteRaw(OperationResultStatus.ToString());
                //OperationResultStatus node end
                xmlWriter.WriteEndElement();

                #endregion Write return values

                //Operation result node end
                xmlWriter.WriteEndElement();

                xmlWriter.Flush();

                //Get the actual XML
                operationResultXml = sw.ToString();

                xmlWriter.Close();
                sw.Close();
            }
            return operationResultXml;
        }

        #region AddOperationResult Overload Methods

        #region Without referenceId parameter

        /// <summary>
        /// Adds operation result
        /// </summary>
        /// <param name="resultCode">Result Code</param>
        /// <param name="resultMessage">Result Message</param>
        /// <param name="operationResultType">The type of the result which needs to be added</param>
        /// <returns>Boolean result saying whether add is successful or not</returns>
        public Boolean AddOperationResult(String resultCode, String resultMessage, OperationResultType operationResultType)
        {
            return AddOperationResult(resultCode, resultMessage, new Collection<Object>(), ReasonType.NotSpecified, -1, -1, operationResultType);
        }

        /// <summary>
        /// Adds operation result
        /// </summary>
        /// <param name="resultCode">Result Code</param>
        /// <param name="parameters">Additional Parameters that requires for OperationResult</param>
        /// <param name="operationResultType">The type of the result which needs to be added</param>
        /// <returns>Boolean result saying whether add is successful or not</returns>
        public Boolean AddOperationResult(String resultCode, Collection<Object> parameters, OperationResultType operationResultType)
        {
            return AddOperationResult(resultCode, String.Empty, parameters, ReasonType.NotSpecified, -1, -1, operationResultType);
        }

        /// <summary>
        /// Add operation result
        /// </summary>
        /// <param name="resultCode">Result Code</param>
        /// <param name="resultMessage">Result Message</param>
        /// <param name="parameters">Additional Parameters that requires for OperationResult</param>
        /// <param name="operationResultType">The type of the result which needs to be added</param>
        /// <returns>Boolean result saying whether add is successful or not</returns>
        public Boolean AddOperationResult(String resultCode, String resultMessage, Collection<Object> parameters, OperationResultType operationResultType)
        {
            return AddOperationResult(resultCode, resultMessage, parameters, ReasonType.NotSpecified, -1, -1, operationResultType);
        }

        /// <summary>
        /// Adds operation result
        /// </summary>
        /// <param name="resultCode">Result Code</param>
        /// <param name="resultMessage">Result Message</param>
        /// <param name="reasonType">Indicates type of the reason.</param>
        /// <param name="ruleMapContextId">Indicates rule map context identifier.</param>
        /// <param name="ruleId">Indicates rule id.</param>
        /// <param name="operationResultType">The type of the result which needs to be added</param>
        /// <param name="ignoreError">Indicates whether to show the errors on UI or not. Default value is false</param>
        /// <returns>Boolean result saying whether add is successful or not</returns>       
        public Boolean AddOperationResult(String resultCode, String resultMessage, ReasonType reasonType, Int32 ruleMapContextId, Int32 ruleId, OperationResultType operationResultType, Boolean ignoreError = false)
        {
            return AddOperationResult(resultCode, resultMessage, new Collection<Object>(), reasonType, ruleMapContextId, ruleId, operationResultType, ignoreError);
        }

        /// <summary>
        /// Adds operation result
        /// </summary>
        /// <param name="resultCode">Result Code</param>
        /// <param name="resultMessage">Result Message</param>
        /// <param name="parameters">Additional Parameters that requires for OperationResult</param>
        /// <param name="reasonType">Indicates type of the reason.</param>
        /// <param name="ruleMapContextId">Indicates rule map context identifier.</param>
        /// <param name="ruleId">Indicates rule identifier.</param>
        /// <param name="operationResultType">The type of the result which needs to be added</param>
        /// <param name="ignoreError">Indicates whether to show the errors on UI or not. Default value is false</param>
        /// <returns>Boolean result saying whether add is successful or not</returns>       
        public Boolean AddOperationResult(String resultCode, String resultMessage, Collection<Object> parameters, ReasonType reasonType, Int32 ruleMapContextId, Int32 ruleId, OperationResultType operationResultType, Boolean ignoreError = false)
        {
            Boolean addSuccess = false;

            if (operationResultType == OperationResultType.Error)
            {
                Error error = new Error(resultCode, resultMessage, parameters, reasonType, ruleMapContextId, ruleId);
                error.IgnoreError = ignoreError;

                if (this.Errors == null)
                    this.Errors = new ErrorCollection();

                this.Errors.Add(error);

                addSuccess = true;
            }
            else if (operationResultType == OperationResultType.Information)
            {
                Information information = new Information(resultCode, resultMessage, parameters, reasonType, ruleMapContextId, ruleId);

                if (this.Informations == null)
                    this.Informations = new InformationCollection();

                this.Informations.Add(information);

                addSuccess = true;
            }
            else if (operationResultType == OperationResultType.Warning)
            {
                Warning warning = new Warning(resultCode, resultMessage, parameters, reasonType, ruleMapContextId, ruleId);

                if (this.Warnings == null)
                    this.Warnings = new WarningCollection();

                this.Warnings.Add(warning);

                addSuccess = true;
            }

            RefreshOperationResultStatus();

            return addSuccess;
        }

        /// <summary>
        /// Add operation result
        /// </summary>
        /// <param name="resultCode">Result Code</param>
        /// <param name="resultMessage">Result Message</param>
        /// <param name="parameters">Additional Parameters that requires for OperationResult</param>
        /// <param name="operationResultType">The type of the result which needs to be added</param>
        /// <returns>Boolean result saying whether add is successful or not</returns>
        public Boolean AddOperationResult(String resultCode, String resultMessage, IList<Object> parameters, OperationResultType operationResultType)
        {
            return this.AddOperationResult(resultCode, resultMessage, new Collection<Object>(parameters), operationResultType);
        }

        #endregion Without referenceId parameter

        #region With referenceId parameter

        /// <summary>
        /// Adds operation result
        /// </summary>
        /// <param name="resultCode">Result Code</param>
        /// <param name="resultMessage">Result Message</param>
        /// <param name="referenceId">ReferenceId</param>
        /// <param name="operationResultType">The type of the result which needs to be added</param>
        /// <returns>Boolean result saying whether add is successful or not</returns>
        public Boolean AddOperationResult(String resultCode, String resultMessage, String referenceId, OperationResultType operationResultType)
        {
            return this.AddOperationResult(resultCode, resultMessage, referenceId, new Collection<Object>(), operationResultType);
        }

        /// <summary>
        /// Adds operation result
        /// </summary>
        /// <param name="resultCode">Result Code</param>
        /// <param name="resultMessage">Result Message</param>
        /// <param name="referenceId">ReferenceId</param>
        /// <param name="parameters">Indicates the parameters</param>
        /// <param name="operationResultType">The type of the result which needs to be added</param>
        /// <returns>Boolean result saying whether add is successful or not</returns>
        public Boolean AddOperationResult(String resultCode, String resultMessage, String referenceId, Collection<Object> parameters, OperationResultType operationResultType)
        {
            Boolean addSuccess = false;

            if (operationResultType == OperationResultType.Error)
            {
                Error error = new Error() { ErrorCode = resultCode, ErrorMessage = resultMessage, ReferenceId = referenceId, Params = parameters };

                if (this.Errors == null)
                    this.Errors = new ErrorCollection();

                this.Errors.Add(error);

                addSuccess = true;
            }
            else if (operationResultType == OperationResultType.Information)
            {
                Information information = new Information(resultCode, resultMessage, parameters);

                if (this.Informations == null)
                    this.Informations = new InformationCollection();

                this.Informations.Add(information);

                addSuccess = true;
            }
            else if (operationResultType == OperationResultType.Warning)
            {
                Warning warning = new Warning { WarningCode = resultCode, WarningMessage = resultMessage, Params = parameters };

                if (this.Warnings == null)
                    this.Warnings = new WarningCollection();

                this.Warnings.Add(warning);

                addSuccess = true;
            }

            RefreshOperationResultStatus();

            return addSuccess;
        }

        #endregion With referenceId parameter

        #endregion AddOperationResult Overload Methods

        /// <summary>
        /// Adds return value
        /// </summary>
        /// <param name="returnValue">Return value object</param>
        /// <returns>Boolean result saying whether add is successful or not</returns>
        public Boolean AddReturnValue(Object returnValue)
        {
            if (this.ReturnValues == null)
                this.ReturnValues = new Collection<Object>();

            this.ReturnValues.Add(returnValue);

            return true;
        }

        /// <summary>
        /// Copy Errors and information from operation operation result to current
        /// </summary>
        /// <param name="operationResult">OperationResult from which errors and information is to be copied</param>
        /// <exception cref="ArgumentNullException">Thrown if operationResult from which errors and information is to be copied is null</exception>
        public void CopyErrorAndInfo(OperationResult operationResult)
        {
            #region Parameter Validation

            if (operationResult == null)
            {
                throw new ArgumentNullException("operationResult");
            }

            #endregion Parameter Validation

            if (operationResult.Errors != null)
            {
                if (this.Errors == null)
                {
                    this.Errors = new ErrorCollection();
                }

                foreach (Error err in operationResult.Errors)
                {
                    this.Errors.Add(err);
                }
            }

            if (operationResult.Informations != null)
            {
                if (this.Informations == null)
                {
                    this.Informations = new InformationCollection();
                }
                foreach (Information info in operationResult.Informations)
                {
                    this.Informations.Add(info);
                }
            }
        }

        /// <summary>
        /// Copy return values from operation operation result to current
        /// </summary>
        /// <param name="operationResult">OperationResult from which errors and information is to be copied</param>
        /// <exception cref="ArgumentNullException">Thrown if operationResult from which errors and information is to be copied is null</exception>
        public void CopyReturnValues(OperationResult operationResult)
        {
            #region Parameter Validation

            if (operationResult == null)
            {
                throw new ArgumentNullException("operationResult");
            }

            #endregion Parameter Validation

            if (operationResult.ReturnValues != null)
            {
                if (this.ReturnValues == null)
                {
                    this.ReturnValues = new Collection<Object>();
                }

                foreach (Object val in operationResult.ReturnValues)
                {
                    this.ReturnValues.Add(val);
                }
            }
        }

        /// <summary>
        /// Copy Errors and information from operation operation result to current
        /// </summary>
        /// <param name="iOperationResult">OperationResult from which errors and information is to be copied</param>
        /// <exception cref="ArgumentNullException">Thrown if operationResult from which errors and information is to be copied is null</exception>
        public void CopyErrorAndInfo(IOperationResult iOperationResult)
        {
            #region Parameter Valudation

            if (iOperationResult == null)
            {
                throw new ArgumentNullException("iOperationResult");
            }

            #endregion Parameter Valudation

            OperationResult operationResult = (OperationResult)iOperationResult;
            this.CopyErrorAndInfo(operationResult);
        }

        /// <summary>
        /// Copy Errors, information and Warnings from operation operation result to current
        /// </summary>
        /// <param name="iOperationResult">OperationResult from which errors, information and Warning is to be copied</param>
        /// <exception cref="ArgumentNullException">Thrown if operationResult from which errors, information and Warning is to be copied is null</exception>
        public void CopyErrorInfoAndWarning(IOperationResult iOperationResult)
        {
            #region Parameter Validation

            if (iOperationResult == null)
            {
                throw new ArgumentNullException("iOperationResult");
            }

            #endregion Parameter Validation

            this.CopyErrorAndInfo(iOperationResult);
            OperationResult operationResult = (OperationResult)iOperationResult;

            if (iOperationResult != null)
            {
                if (this.Warnings == null)
                {
                    this.Warnings = new WarningCollection();
                }
                foreach (Warning warning in operationResult.Warnings)
                {
                    this.Warnings.Add(warning);
                }

                this.RefreshOperationResultStatus();
            }
        }

        /// <summary>
        /// Initialize current object with Xml
        /// </summary>
        /// <param name="valuesAsXml">Xml having values for current OperationResult
        /// <para>
        /// Sample Xml:
        /// <![CDATA[
        /// <OperationResult>
        ///     <Errors>
        ///         <Error Code="" Message = "EntityCreated BR failed"/>
        ///     </Errors>
        ///     <Informations>
        ///         <Information Code="" Message="Entity process successful : Id = 4376255, Name = 116674, LongName = 116674, SKU = 116674, Path = " />
        ///     </Informations>
        ///     <ReturnValues>
        ///         <ReturnValue>
        ///             <Entities>
        ///                 <Entity Id="4376255" ObjectId="318824" Name="116674" LongName="116674" SKU="116674"/>
        ///             </Entities>
        ///         </ReturnValue>
        ///         <ReturnValue>
        ///             <Entity ShortName="116674" LongName="116674" Result="Success" Message="Stage transition completed successfully" />
        ///         </ReturnValue>
        ///     </ReturnValues>
        /// </OperationResult>
        /// ]]>
        /// </para>
        /// </param>
        public void LoadOperationResult(String valuesAsXml)
        {
            #region Sample Xml
            /*
             <OperationResult>
                <Errors>
                     <Error Code="" Message = "EntityCreated BR failed"/>
                </Errors>
                <Informations>
                  <Information Code="" Message="Entity process successful : Id = 4376255, Name = 116674, LongName = 116674, SKU = 116674, Path = " />
                </Informations>
                <ReturnValues>
                  <ReturnValue>
                    <Entities>
                      <Entity Id="4376255" ObjectId="318824" Name="116674" LongName="116674" SKU="116674"/>
                    </Entities>
                  </ReturnValue>
                  <ReturnValue>
                    <Entity ShortName="116674" LongName="116674" Result="Success" Message="Stage transition completed successfully" />
                  </ReturnValue>
                </ReturnValues>
              </OperationResult>
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
                            //Read error
                            #region Read error

                            String errorXml = reader.ReadOuterXml();
                            if (!String.IsNullOrEmpty(errorXml))
                            {
                                Error error = new Error(errorXml);
                                if (error != null)
                                {
                                    if (this.Errors == null)
                                    {
                                        this.Errors = new ErrorCollection();
                                    }
                                    this.Errors.Add(error);
                                }
                            }

                            #endregion Read error
                        }
                        else if (reader.NodeType == XmlNodeType.Element && reader.Name == "Information")
                        {
                            //Read Information
                            #region Read Information

                            String infoXml = reader.ReadOuterXml();
                            if (!String.IsNullOrEmpty(infoXml))
                            {
                                Information info = new Information(infoXml);
                                if (info != null)
                                {
                                    if (this.Informations == null)
                                    {
                                        this.Informations = new InformationCollection();
                                    }
                                    this.Informations.Add(info);
                                }
                            }

                            #endregion Read Information
                        }
                        else if (reader.NodeType == XmlNodeType.Element && reader.Name == "Warning")
                        {
                            //Read Warning
                            #region Read Warning

                            String warningXml = reader.ReadOuterXml();
                            if (!String.IsNullOrEmpty(warningXml))
                            {
                                Warning warning = new Warning(warningXml);
                                if (warning != null)
                                {
                                    if (this.Warnings == null)
                                    {
                                        this.Warnings = new WarningCollection();
                                    }
                                    this.Warnings.Add(warning);
                                }
                            }

                            #endregion Read Warning
                        }
                        else if (reader.NodeType == XmlNodeType.Element && reader.Name == "ReturnValue")
                        {
                            //Read ReturnValues
                            #region Read ReturnValues

                            if (this.ReturnValues == null)
                            {
                                this.ReturnValues = new Collection<object>();
                            }

                            this.ReturnValues.Add(reader.ReadInnerXml());

                            #endregion Read ReturnValues
                        }
                        else if (reader.NodeType == XmlNodeType.Element && reader.Name == "OperationResultStatus")
                        {
                            OperationResultStatusEnum operationResultStatus = OperationResultStatusEnum.None;
                            Enum.TryParse(reader.ReadElementContentAsString(), out operationResultStatus);
                            this.OperationResultStatus = operationResultStatus;
                        }
                        else if (reader.NodeType == XmlNodeType.Element && reader.Name == "ExtendedProperties")
                        {
                            #region Read Extended Properties

                            String propXml = reader.ReadOuterXml();
                            if (!String.IsNullOrEmpty(propXml))
                            {
                                Hashtable props = this.ReadExtendedProperties(propXml);
                                if (props != null)
                                {
                                    this.ExtendedProperties = props;
                                }
                            }

                            #endregion Read Extended Properties
                        }
                        else if (reader.NodeType == XmlNodeType.Element && reader.Name == "OperationResult")
                        {
                            if (reader.HasAttributes)
                            {
                                if (reader.MoveToAttribute("PerformedAction"))
                                {
                                    ObjectAction performedAction = ObjectAction.Unknown;
                                    Enum.TryParse(reader.ReadContentAsString(), out performedAction);
                                    this.PerformedAction = performedAction;
                                }
                            }

                            reader.Read();
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
        /// Gets information of Operation Result
        /// </summary>
        /// <returns>Collection of information</returns>
        public Collection<IInformation> GetInformation()
        {
            Collection<IInformation> iInformationCollection = new Collection<IInformation>();

            foreach (Information info in this.Informations)
            {
                iInformationCollection.Add((IInformation)info);
            }

            return iInformationCollection;
        }

        /// <summary>
        /// Gets errors
        /// </summary>
        /// <returns>Collection of errors</returns>
        public Collection<IError> GetErrors()
        {
            Collection<IError> iErrorCollection = new Collection<IError>();

            foreach (Error error in this.Errors)
            {
                iErrorCollection.Add((IError)error);
            }

            return iErrorCollection;
        }

        /// <summary>
        /// Gets warnings
        /// </summary>
        /// <returns>Collection of warnings</returns>
        public IWarningCollection GetWarnings()
        {
            WarningCollection warningCollection = new WarningCollection();

            foreach (Warning warning in this.Warnings)
            {
                warningCollection.Add(warning);
            }

            return (IWarningCollection)warningCollection;
        }

        /// <summary>
        /// Determines whether the current object instance is superset of the operation result passed as parameter
        /// </summary>
        /// <param name="subsetOperationResult">Indicates the subset object to compare with the current object</param>
        /// <param name="compareIds">Indicates whether ids to be compared or not</param>
        /// <param name="compareReturnValues">Indicates whether return values to be compared or not</param>
        /// <returns>Returns true if the current object is superset of the subset instances; otherwise false</returns>
        public Boolean IsSuperSetOf(OperationResult subsetOperationResult, Boolean compareIds, Boolean compareReturnValues = false)
        {
            if (compareIds)
            {
                if (this.Id != subsetOperationResult.Id)
                    return false;

                if (this.ReferenceId != subsetOperationResult.ReferenceId)
                    return false;
            }


            if (compareReturnValues)
            {
                Int32 returnValuesUnion = this.ReturnValues.ToList().Union(subsetOperationResult.ReturnValues.ToList()).Count();
                Int32 returnValuesIntersect = this.ReturnValues.ToList().Intersect(subsetOperationResult.ReturnValues.ToList()).Count();

                if (returnValuesUnion != returnValuesIntersect)
                    return false;
            }

            if (this.OperationResultStatus != subsetOperationResult.OperationResultStatus)
                return false;

            if (this.HasWarnings != subsetOperationResult.HasWarnings)
                return false;

            if (this.HasError != subsetOperationResult.HasError)
                return false;

            if (this.HasInformation != subsetOperationResult.HasInformation)
                return false;

            if (!this.Errors.IsSuperSetOf(subsetOperationResult.Errors))
                return false;

            if (!this.Warnings.IsSuperSetOf(subsetOperationResult.Warnings))
                return false;

            if (!this.Informations.IsSuperSetOf(subsetOperationResult.Informations))
                return false;

            return true;

        }

        /// <summary>
        /// Refresh the operation result status based on the errors and warnings
        /// </summary>
        public virtual void RefreshOperationResultStatus()
        {
            if (this.Errors != null && this.Errors.Count > 0)
            {
                this.OperationResultStatus = OperationResultStatusEnum.Failed;
            }
            else if (this.Warnings != null && this.Warnings.Count > 0)
            {
                this.OperationResultStatus = OperationResultStatusEnum.CompletedWithWarnings;
            }
            else
            {
                this.OperationResultStatus = OperationResultStatusEnum.Successful;
            }
        }

        #endregion Public Methods

        #region Private Methods

#pragma warning disable 1570

        /// <summary>
        /// Read ExtendedProperties from Xml and populate NameValueCollection for those properties
        /// </summary>
        /// <param name="valuesAsXml">
        /// Xml having value
        /// Sample Xml:
        /// <para>
        /// <![CDATA[
        /// <ExtendedProperties>
        ///     <ExtendedProperty Key="TotalRecords">
        ///        <![CDATA[3006]]>
        ///     </ExtendedProperty>
        /// </ExtendedProperties>
        /// ]]>
        /// </para>
        /// </param>
        /// <returns>Extended properties in form of NameValueCollection</returns>

#pragma warning restore 1570

        private Hashtable ReadExtendedProperties(String valuesAsXml)
        {
            Hashtable extendedProperties = null;
            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                XmlTextReader reader = null;
                try
                {
                    extendedProperties = new Hashtable();

                    reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "ExtendedProperty" && reader.HasAttributes)
                        {

                            String key = String.Empty;
                            String value = String.Empty;

                            if (reader.GetAttribute("Key") != null)
                            {
                                key = reader.GetAttribute("Key");
                            }
                            value = reader.ReadElementContentAsString();

                            if (!extendedProperties.Contains(key))
                            {
                                extendedProperties.Add(key, value);
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
            return extendedProperties;
        }

        #endregion Private Methods

        #endregion Methods
    }
}