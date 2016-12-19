using System;
using System.Globalization;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;

namespace MDM.BusinessObjects.DQM
{
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Specifies Normalization Result
    /// </summary>
    [DataContract]
    public class NormalizationResult : MDMObject, INormalizationResult
    {
        #region Fields

        /// <summary>
        /// Field denoting name of main node in Xml for NormalizationResult
        /// </summary>    
        private const String NodeName = "NormalizationResult";

        /// <summary>
        /// Field for NormalizationResult Id
        /// </summary>        
        private Int64 _id = -1;

        /// <summary>
        /// Field denoting id of job which create result
        /// </summary>
        private Int64 _jobId;

        /// <summary>
        /// Field denoting attribute Id which was changed
        /// </summary>
        private Int32 _attributeId;

        /// <summary>
        /// Field denoting Cnode Id which was changed
        /// </summary>
        private Int64 _cnodeId;

        /// <summary>
        /// Field denoting DateTime when attribute was changed
        /// </summary>
        private DateTime? _changeDateTime;

        /// <summary>
        /// Field denoting rule id which was applied
        /// </summary>
        private Int32 _ruleId;

        /// <summary>
        /// Field denoting attribute value which was before changing
        /// </summary>
        private String _oldAttributeValue;

        /// <summary>
        /// Field denoting attribute value which was created after changing
        /// </summary>
        private String _newAttributeValue;

        /// <summary>
        /// Field denoting if attribute was changed successfully
        /// </summary>
        private Boolean _isNormalizationSucceeded;

        /// <summary>
        /// Field denoting result message
        /// </summary>
        private String _resultMessage;

        #endregion

        #region Properties

        /// <summary>
        /// Property denoting NormalizationResult Id
        /// </summary>        
        [DataMember]
        public new Int64 Id
        {
            get { return _id; }
            set { _id = value; }
        }

        /// <summary>
        /// Property denoting id of job which create result
        /// </summary>
        [DataMember]
        public Int64 JobId
        {
            get { return _jobId; }
            set { _jobId = value; }
        }

        /// <summary>
        /// Property denoting attribute Id which was changed
        /// </summary>
        [DataMember]
        public Int32 AttributeId
        {
            get { return _attributeId; }
            set { _attributeId = value; }
        }

        /// <summary>
        /// Property denoting Cnode Id which was changed
        /// </summary>
        [DataMember]
        public Int64 CnodeId
        {
            get { return _cnodeId; }
            set { _cnodeId = value; }
        }

        /// <summary>
        /// Property denoting DateTime when attribute was changed. Please set to Null if you want to use current SQL server time as ChangeDateTime during saving operations.
        /// </summary>
        [DataMember]
        public DateTime? ChangeDateTime
        {
            get { return _changeDateTime; }
            set { _changeDateTime = value; }
        }

        /// <summary>
        /// Property denoting rule id which was applied
        /// </summary>
        [DataMember]
        public Int32 RuleId
        {
            get { return _ruleId; }
            set { _ruleId = value; }
        }

        /// <summary>
        /// Property denoting attribute value which was before changing
        /// </summary>
        [DataMember]
        public String OldAttributeValue
        {
            get { return _oldAttributeValue; }
            set { _oldAttributeValue = value; }
        }

        /// <summary>
        /// Property denoting attribute value which was created after changing
        /// </summary>
        [DataMember]
        public String NewAttributeValue
        {
            get { return _newAttributeValue; }
            set { _newAttributeValue = value; }
        }

        /// <summary>
        /// Property denoting if attribute was changed successfully
        /// </summary>
        [DataMember]
        public Boolean IsNormalizationSucceeded
        {
            get { return _isNormalizationSucceeded; }
            set { _isNormalizationSucceeded = value; }
        }

        /// <summary>
        /// Property denoting result message
        /// </summary>
        [DataMember]
        public String ResultMessage
        {
            get { return _resultMessage; }
            set { _resultMessage = value; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Parameterless Constructor
        /// </summary>
        public NormalizationResult() { }

        /// <summary>
        /// Constructor with XML-format String of an NormalizationResult as input parameter
        /// </summary>
        /// <param name="valueAsXml">Indicates an NormalizationResult in format of XML String</param>
        public NormalizationResult(String valueAsXml)
        {
            LoadNormalizationResultFromXml(valueAsXml);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Get Xml representation of NormalizationResult
        /// </summary>
        /// <returns>Xml representation of object</returns>
        public override String ToXml()
        {
            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            //MergeResults node start
            xmlWriter.WriteStartElement(NodeName);

            xmlWriter.WriteAttributeString("Id", Id.ToString(CultureInfo.InvariantCulture));

            xmlWriter.WriteStartElement("NormalizationJobId");
            xmlWriter.WriteValue(JobId.ToString(CultureInfo.InvariantCulture));
            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement("AttributeId");
            xmlWriter.WriteValue(AttributeId.ToString(CultureInfo.InvariantCulture));
            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement("EntityId");
            xmlWriter.WriteValue(CnodeId.ToString(CultureInfo.InvariantCulture));
            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement("ChangeDateTime");
            xmlWriter.WriteValue(ChangeDateTime.HasValue ? ChangeDateTime.Value.ToString(CultureInfo.InvariantCulture) : String.Empty);
            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement("RuleId");
            xmlWriter.WriteValue(RuleId.ToString(CultureInfo.InvariantCulture));
            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement("OldAttributeValue");
            xmlWriter.WriteValue(OldAttributeValue);
            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement("NewAttributeValue");
            xmlWriter.WriteValue(NewAttributeValue);
            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement("IsNormalizationSucceeded");
            xmlWriter.WriteValue(IsNormalizationSucceeded);
            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement("ResultMessage");
            xmlWriter.WriteValue(ResultMessage);
            xmlWriter.WriteEndElement();

            //MergeResult node end
            xmlWriter.WriteEndElement();

            xmlWriter.Flush();

            //Get the actual XML
            String result = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return result;
        }

        private void LoadNormalizationResultFromXml(String valueAsXml)
        {
            XmlTextReader reader = null;

            try
            {
                reader = new XmlTextReader(valueAsXml, XmlNodeType.Element, null);

                if (reader.HasAttributes)
                {
                    reader.MoveToAttribute("Id");
                    Id = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), -1);
                }

                while (!reader.EOF)
                {
                    if (reader.NodeType == XmlNodeType.Element)
                    {
                        switch (reader.Name)
                        {
                            case "NormalizationResult":
                                if (reader.HasAttributes)
                                {
                                    if (reader.MoveToAttribute("Id"))
                                    {
                                        Id = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), 0);
                                    }
                                    if (reader.MoveToAttribute("Name"))
                                    {
                                        Name = reader.ReadContentAsString();
                                    }
                                    if (reader.MoveToAttribute("LongName"))
                                    {
                                        LongName = reader.ReadContentAsString();
                                    }
                                    if (reader.MoveToAttribute("Locale"))
                                    {
                                        LocaleEnum locale;
                                        Enum.TryParse(reader.ReadContentAsString(), out locale);
                                        Locale = locale;
                                    }
                                }
                                break;
                            case "NormalizationJobId":
                                JobId = ValueTypeHelper.Int64TryParse(reader.ReadElementContentAsString(), 0);
                                break;
                            case "AttributeId":
                                AttributeId = ValueTypeHelper.Int32TryParse(reader.ReadElementContentAsString(), 0);
                                break;
                            case "EntityId":
                                CnodeId = ValueTypeHelper.Int64TryParse(reader.ReadElementContentAsString(), 0);
                                break;
                            case "ChangeDateTime":
                                ChangeDateTime = ValueTypeHelper.ConvertToNullableDateTime(reader.ReadElementContentAsString());
                                break;
                            case "RuleId":
                                RuleId = ValueTypeHelper.Int32TryParse(reader.ReadElementContentAsString(), 0);
                                break;
                            case "OldAttributeValue":
                                OldAttributeValue = reader.ReadElementContentAsString();
                                break;
                            case "NewAttributeValue":
                                NewAttributeValue = reader.ReadElementContentAsString();
                                break;
                            case "IsNormalizationSucceeded":
                                IsNormalizationSucceeded = ValueTypeHelper.BooleanTryParse(reader.ReadElementContentAsString(), false);
                                break;
                            case "ResultMessage":
                                ResultMessage = reader.ReadElementContentAsString();
                                break;
                            default:
                                reader.Read();
                                break;
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

        #endregion

        #region ICloneable Members

        /// <summary>
        /// Creates a clone copy of normalization result
        /// </summary>
        /// <returns>Returns a clone copy of normalization result</returns>
        public object Clone()
        {
            NormalizationResult result = (NormalizationResult) this.MemberwiseClone();
            return result;
        }

        #endregion
    }
}