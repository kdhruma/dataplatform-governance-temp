using System;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;

namespace MDM.BusinessObjects
{
    using MDM.Core;
    using MDM.Interfaces;
    using MDM.BusinessRuleManagement.Business;

    /// <summary>
    /// Specifies results for Business rule operations
    /// </summary>
    [DataContract]
    public class BusinessRuleOperationResult : OperationResult, IBusinessRuleOperationResult
    {
        #region Fields
        /// <summary>
        /// Field denotes the identifier of the MDMRule
        /// </summary>
        private Int32 _ruleId = -1;

        /// <summary>
        /// Field denotes the identifier of the LocaleMessage
        /// </summary>
        private Int32 _ddgLocaleMessageId = -1;

        /// <summary>
        /// Field denotes the code of the LocaleMessage
        /// </summary>
        private String _ddgLocaleMessageCode = String.Empty;

        /// <summary>
        /// Field denotes the locale of the LocaleMessage
        /// </summary>
        private LocaleEnum _locale = LocaleEnum.UnKnown;

        /// <summary>
        /// Field denotes the name of the MDMRule
        /// </summary>
        private String _ruleName = String.Empty;

        /// <summary>
        /// Field denotes the MDMRule type
        /// </summary>
        private MDMRuleType _ruleType = MDMRuleType.UnKnown;

        /// <summary>
        /// Field denotes the identifier of the MDMRule Map
        /// </summary>
        private Int32 _ruleMapId = -1;

        /// <summary>
        /// Field denotes the identifier of the MDMRule Map Name
        /// </summary>
        private String _ruleMapName = String.Empty;

        /// <summary>
        /// Field denotes the reference Id
        /// </summary>
        private Int64 _referenceId = -1;

        /// <summary>
        /// Field denotes the object type of dynamic data governance.
        /// </summary>
        private ObjectType _ddgObjectType;

        #endregion Fields

        #region Properties

        /// <summary>
        /// Property denotes the Reference Id
        /// </summary>
        [DataMember]
        public new Int64 ReferenceId
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
        /// Property denotes the identifier of the MDMRule
        /// </summary>
        [DataMember]
        public Int32 RuleId
        {
            get
            {
                return _ruleId;
            }
            set
            {
                _ruleId = value;
            }
        }

        /// <summary>
        /// Property denotes the name of the MDMRule
        /// </summary>
        [DataMember]
        public String RuleName
        {
            get
            {
                return _ruleName;
            }
            set
            {
                _ruleName = value;
            }
        }

        /// <summary>
        /// Property denotes the identifier of the MDMRule Map
        /// </summary>
        [DataMember]
        public Int32 RuleMapId
        {
            get
            {
                return _ruleMapId;
            }
            set
            {
                _ruleMapId = value;
            }
        }

        /// <summary>
        /// Property denotes the identifier of the MDMRule Map
        /// </summary>
        [DataMember]
        public String RuleMapName
        {
            get
            {
                return _ruleMapName;
            }
            set
            {
                _ruleMapName = value;
            }
        }

        /// <summary>
        /// Property denotes the MDMRule type
        /// </summary>
        [DataMember]
        public MDMRuleType RuleType
        {
            get
            {
                return _ruleType;
            }
            set
            {
                _ruleType = value;
            }
        }

        /// <summary>
        /// Property denotes the DDG locale message id
        /// </summary>
        [DataMember]
        public Int32 DDGLocaleMessageId
        {
            get
            {
                return _ddgLocaleMessageId;
            }
            set
            {
                _ddgLocaleMessageId = value;
            }

        }

        /// <summary>
        /// Property denotes the DDG locale message code
        /// </summary>
        [DataMember]
        public String DDGLocaleMessageCode
        {
            get
            {
                return _ddgLocaleMessageCode;
            }
            set
            {
                _ddgLocaleMessageCode = value;
            }

        }

        /// <summary>
        /// Property denotes the DDG locale message locale id
        /// </summary>
        [DataMember]
        public LocaleEnum Locale
        {
            get
            {
                return _locale;
            }
            set
            {
                _locale = value;
            }
        }

        /// <summary>
        /// Property denotes the DDG object Type
        /// </summary>
        [DataMember]
        public ObjectType DDGObjectType
        {
            get
            {
                return _ddgObjectType;
            }
            set
            {
                _ddgObjectType = value;
            }
        }

        #endregion Properties

        #region Constructors

        /// <summary>
        /// Parameter less constructor
        /// </summary>
        public BusinessRuleOperationResult()
            : base()
        {

        }

        /// <summary>
        /// Parameterised constructor having object values in xml format.
        /// Populates current object using XML.
        /// </summary>
        /// <param name="valuesAsXml">Indicates xml representation of Business rule operation result object</param>
        public BusinessRuleOperationResult(String valuesAsXml)
        {
            LoadOperationResult(valuesAsXml);
        }

        /// <summary>
        /// Initializes a new instance of Business rule operation result using MDMRuleMapRule
        /// </summary>
        /// <param name="mdmRuleMapRule">Indicates the MDMRuleMapRule</param>
        public BusinessRuleOperationResult(MDMRuleMapRule mdmRuleMapRule)
        {
            if (mdmRuleMapRule != null)
            {
                this.RuleId = mdmRuleMapRule.RuleId;
                this.RuleName = mdmRuleMapRule.RuleName;
                this.RuleType = mdmRuleMapRule.RuleType;
            }
        }

        /// <summary>
        /// Initializes a new instance of Business rule operation result for the requested MDMRule
        /// </summary>
        /// <param name="mdmRule">Indicates the MDMRule</param>
        public BusinessRuleOperationResult(MDMRule mdmRule)
        {
            if (mdmRule != null)
            {
                this.RuleId = mdmRule.Id;
                this.RuleName = mdmRule.Name;
                this.RuleType = mdmRule.RuleType;
                this.ReferenceId = mdmRule.ReferenceId;
            }
        }

        /// <summary>
        /// Initializes a new instance of Business rule operation result for the requested MDMRule Map
        /// </summary>
        /// <param name="mdmRuleMap">Indicates the MDMRule map</param>
        public BusinessRuleOperationResult(MDMRuleMap mdmRuleMap)
        {
            if (mdmRuleMap != null)
            {
                this.RuleMapId = mdmRuleMap.Id;
                this.RuleMapName = mdmRuleMap.Name;
                this.ReferenceId = mdmRuleMap.ReferenceId;
                //Todo..
                //this._ruleId = mdmRuleMap.MDMRuleId;
                //this._ruleName = mdmRuleMap.MDMRuleName;
            }
        }

        /// <summary>
        /// Initializes a new instance of Business rule operation result for the requested locale message
        /// </summary>
        /// <param name="localeMessage">Indicates locale message using which the BusinessRuleOperationResult needs to be created</param>
        public BusinessRuleOperationResult(LocaleMessage localeMessage)
        {
            if (localeMessage != null)
            {
                this.ReferenceId = ValueTypeHelper.Int32TryParse(localeMessage.ReferenceId, -1);
                this.Id = localeMessage.Id;
            }
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Initialize current object with Xml
        /// </summary>
        /// <param name="valuesAsXml">Xml having values for current Business rule operationResult
        /// </param>
        public new void LoadOperationResult(String valuesAsXml)
        {
            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                using (XmlTextReader reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null))
                {
                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "BusinessRuleOperationResult")
                        {
                            if (reader.HasAttributes)
                            {
                                if (reader.MoveToAttribute("Id"))
                                {
                                    this.Id = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), this.Id);
                                }

                                if (reader.MoveToAttribute("ReferenceId"))
                                {
                                    this.ReferenceId = ValueTypeHelper.Int64TryParse(reader.ReadContentAsString(), _referenceId);
                                }

                                if (reader.MoveToAttribute("RuleName"))
                                {
                                    this.RuleName = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("RuleType"))
                                {
                                    MDMRuleType ruleType = MDMRuleType.UnKnown;
                                    ValueTypeHelper.EnumTryParse<MDMRuleType>(reader.ReadContentAsString(), false, out ruleType);
                                    this.RuleType = ruleType;
                                }

                                if (reader.MoveToAttribute("ObjectType"))
                                {
                                    ObjectType objectType = Core.ObjectType.None;
                                    ValueTypeHelper.EnumTryParse<ObjectType>(reader.ReadContentAsString(), false, out objectType);
                                    this.DDGObjectType= objectType;
                                }

                                if (reader.MoveToAttribute("RuleId"))
                                {
                                    this.RuleId = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), this._ruleId);
                                }

                                if (reader.MoveToAttribute("DDGLocaleMessageId"))
                                {
                                    this.DDGLocaleMessageId = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), this._ddgLocaleMessageId);
                                }

                                if (reader.MoveToAttribute("DDGLocaleMessageCode"))
                                {
                                    this.DDGLocaleMessageCode = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("Locale"))
                                {
                                    LocaleEnum localeEnum;
                                    ValueTypeHelper.EnumTryParse<LocaleEnum>(reader.ReadContentAsString(), true, out localeEnum);
                                    this.Locale = localeEnum;
                                }

                                if (reader.MoveToAttribute("RuleMapId"))
                                {
                                    this.RuleMapId = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), this._ruleMapId);
                                }

                                if (reader.MoveToAttribute("RuleMapName"))
                                {
                                    this.RuleMapName = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("Status"))
                                {
                                    OperationResultStatusEnum operationResultStatus = OperationResultStatusEnum.None;
                                    String status = reader.ReadContentAsString();

                                    if (!String.IsNullOrWhiteSpace(status))
                                    {
                                        Enum.TryParse<OperationResultStatusEnum>(status, out operationResultStatus);
                                        this.OperationResultStatus = operationResultStatus;
                                    }
                                }

                                if (reader.MoveToAttribute("PerformedAction"))
                                {
                                    ObjectAction performedAction = ObjectAction.Unknown;
                                    Enum.TryParse(reader.ReadContentAsString(), out performedAction);
                                    this.PerformedAction = performedAction;
                                }

                                       reader.Read();
                            }
                        }
                        else if (reader.NodeType == XmlNodeType.Element && reader.Name == "Error")
                        {
                            //Read error
                            #region Read error

                            String errorXml = reader.ReadOuterXml();

                            if (!String.IsNullOrWhiteSpace(errorXml))
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

                            if (!String.IsNullOrWhiteSpace(infoXml))
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

                            if (!String.IsNullOrWhiteSpace(warningXml))
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
                        else
                        {
                            reader.Read();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Get Xml representation of Business rule operation result
        /// </summary>
        /// <returns>Xml representation of Business rule Operation Result object</returns>
        public new String ToXml()
        {
            String outputXML = String.Empty;
            using (StringWriter stringWriter = new StringWriter())
            {
                using (XmlTextWriter xmlWriter = new XmlTextWriter(stringWriter))
                {
                    #region Write BusinessRuleOperationResults

                    //Add BusinessRuleOperationResult Node
                    xmlWriter.WriteStartElement("BusinessRuleOperationResult");

                    xmlWriter.WriteAttributeString("Id", this.Id.ToString());
                    xmlWriter.WriteAttributeString("ReferenceId", this.ReferenceId.ToString());
                    xmlWriter.WriteAttributeString("RuleId", this.RuleId.ToString());
                    xmlWriter.WriteAttributeString("RuleName", this.RuleName);
                    xmlWriter.WriteAttributeString("RuleType", this.RuleType.ToString());
                    xmlWriter.WriteAttributeString("RuleMapId", this.RuleMapId.ToString());
                    xmlWriter.WriteAttributeString("RuleMapName", this.RuleMapName);
                    xmlWriter.WriteAttributeString("Status", this.OperationResultStatus.ToString());
                    xmlWriter.WriteAttributeString("ObjectType", this.ObjectType.ToString());
                    xmlWriter.WriteAttributeString("PerformedAction", this.PerformedAction.ToString());
                    xmlWriter.WriteAttributeString("DDGLocaleMessageId", this.DDGLocaleMessageId.ToString());
                    xmlWriter.WriteAttributeString("DDGLocaleMessageCode", this.DDGLocaleMessageCode);
                    xmlWriter.WriteAttributeString("Locale", ((Int32)this.Locale).ToString());

                    #endregion Write BusinessRuleOperationResults

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

                    #endregion Write Warnings

                    //BusinessRuleOperationResult Node End
                    xmlWriter.WriteEndElement();

                    xmlWriter.Flush();

                    xmlWriter.Close();

                    //get the actual XML
                    outputXML = stringWriter.ToString();
                }
            }
            return outputXML;
        }

        /// <summary>
        /// Get Reference Name for business rule object type. 
        /// </summary>
        /// <returns>Reference Name for given ObjectType</returns>
        public String GetReferenceNameByObjectType(ObjectType objectType)
        {
            String referenceName = String.Empty;

            if (DDGDictionary.ObjectsDictionary.ContainsKey(objectType))
            {
                referenceName = DDGDictionary.ObjectsDictionary[objectType].ToString();
            }

            return referenceName;
        }

        /// <summary>
        /// Compares BusinessRuleOperationResult object with current BusinessRuleOperationResult object
        /// This method will compare object, its attributes and Values.
        /// If current object has more attributes than object to be compared, extra attributes will be ignored.
        /// If attribute to be compared has attributes which is not there in current collection, it will return false.
        /// </summary>
        /// <param name="subSetBusinessRuleOperationResult">Indicates BusinessRuleOperationResult object to be compared with current BusinessRuleOperationResult object</param>
        /// <param name="compareIds">Indicates whether to compare ids for the current object or not</param>
        /// <returns>Returns True : If both are same. False : otherwise</returns>
        public Boolean IsSuperSetOf(BusinessRuleOperationResult subSetBusinessRuleOperationResult, Boolean compareIds = false)
        {
            if (compareIds)
            {
                if (this.Id != subSetBusinessRuleOperationResult.Id)
                {
                    return false;
                }

                if (this.RuleId != subSetBusinessRuleOperationResult.RuleId)
                {
                    return false;
                }

                if (this.RuleMapId != subSetBusinessRuleOperationResult.RuleMapId)
                {
                    return false;
                }

                if (this.DDGLocaleMessageId != subSetBusinessRuleOperationResult.DDGLocaleMessageId)
                {
                    return false;
                }
            }

            if (!base.IsSuperSetOf(subSetBusinessRuleOperationResult, compareIds))
            {
                return false;
            }

            if (String.Compare(this.DDGLocaleMessageCode, subSetBusinessRuleOperationResult.DDGLocaleMessageCode) != 0)
            {
                return false;
            }

            if (String.Compare(this.RuleName, subSetBusinessRuleOperationResult.RuleName) != 0)
            {
                return false;
            }

            if (this.RuleType != subSetBusinessRuleOperationResult.RuleType)
            {
                return false;
            }

            if (String.Compare(this.RuleMapName, subSetBusinessRuleOperationResult.RuleMapName) != 0)
            {
                return false;
            }

            if (this.ObjectType != subSetBusinessRuleOperationResult.ObjectType)
            {
                return false;
            }

            if (this.OperationResultStatus != subSetBusinessRuleOperationResult.OperationResultStatus)
            {
                return false;
            }

            if (this.Locale != subSetBusinessRuleOperationResult.Locale)
            {
                return false;
            }

            return true;
        }

        #endregion Methods
    }
}