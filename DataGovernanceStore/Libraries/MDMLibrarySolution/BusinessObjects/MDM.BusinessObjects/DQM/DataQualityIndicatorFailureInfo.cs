using System;
using System.IO;
using System.Linq;
using System.Xml;
using System.Runtime.Serialization;
using System.Collections.ObjectModel;

namespace MDM.BusinessObjects.DQM
{
    using MDM.Core;
    using MDM.Interfaces;
    using MDM.Core.Extensions;

    /// <summary>
    /// Specifies DataQualityIndicatorFailureInfo object for storing failure information for DataQualityIndicator
    /// </summary>
    [DataContract]
    public class DataQualityIndicatorFailureInfo : IDataQualityIndicatorFailureInfo
    {
        #region Fields

        private String _failureMessageCode = String.Empty;

        private String _failureMessage = String.Empty;

        private Collection<Object> _params = new Collection<Object>();

        private Int32? _attributeId = null;

        private String _attributeValue = String.Empty;

        private Int64? _relationshipId = null;

        private Int16? _localeId = null;

        #endregion

        #region Constructors

        /// <summary>
        /// Parameterless constructor
        /// </summary>
        public DataQualityIndicatorFailureInfo()
        {
        }

        /// <summary>
        /// Constructor with failure code and failure message as input parameter
        /// </summary>
        /// <param name="failureMessageCode">Indicates code of the failure</param>
        /// <param name="failureMessage">Indicates the failure message</param>
        /// <param name="parameters">Indicates collection of parameters</param>
        /// <param name="attributeId">Indicates the attributeId</param>
        /// <param name="relationshipId">Indicates the relationshipId</param>
        public DataQualityIndicatorFailureInfo(String failureMessageCode, String failureMessage, Collection<Object> parameters, Int32? attributeId, Int64? relationshipId)
        {
            _failureMessageCode = failureMessageCode;
            _failureMessage = failureMessage;
            _params = parameters;
            _attributeId = attributeId;
            _relationshipId = relationshipId;
        }

        /// <summary>
        /// Constructor that creates and object from provided xml
        /// </summary>
        /// <param name="xml"></param>
        public DataQualityIndicatorFailureInfo(String xml)
        {
            LoadFromXmlWithOuterNode(xml);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Denotes FailureMessageCode for the operation failure
        /// </summary>
        [DataMember]
        public String FailureMessageCode
        {
            get
            {
                return _failureMessageCode;
            }
            set
            {
                _failureMessageCode = value;
            }

        }

        /// <summary>
        /// Denotes FailureMessage for the operation failure
        /// </summary>
        [DataMember]
        public String FailureMessage
        {
            get
            {
                return _failureMessage;
            }
            set
            {
                _failureMessage = value;
            }
        }

        /// <summary>
        /// Denotes property for additional params for FailureMessageCode 
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
        /// Represents the failed Attribute Id
        /// </summary>
        [DataMember]
        public Int32? AttributeId
        {
            get
            {
                return _attributeId;
            }
            set
            {
                _attributeId = value;
            }
        }
        
        /// <summary>
        /// Represents the failed Attribute Value
        /// </summary>
        [DataMember]
        public String AttributeValue
        {
            get
            {
                return _attributeValue;
            }
            set
            {
                _attributeValue = value;
            }
        }
        
        /// <summary>
        /// Represents the failed Relationship Id
        /// </summary>
        [DataMember]
        public Int64? RelationshipId
        {
            get
            {
                return _relationshipId;
            }
            set
            {
                _relationshipId = value;
            }
        }

        /// <summary>
        /// Represents message Locale Id
        /// </summary>
        [DataMember]
        public Int16? LocaleId
        {
            get
            {
                return _localeId;
            }
            set
            {
                _localeId = value;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Returns formatted and localized message (if message code is provided) or unlocalized FailureMessage otherwise
        /// </summary>
        /// <param name="getLocaleMessage">Indicates function for getting locale messages</param>
        /// <returns>Returns error message</returns>
        public String GetFailureMessage(Func<String, String> getLocaleMessage)
        {
            return (String.IsNullOrEmpty(FailureMessageCode)) 
                ? FailureMessage 
                : String.Format(getLocaleMessage(FailureMessageCode), Params.ToArray());
        }

        #region Xml Serialization

        /// <summary>
        /// Get Xml representation of DataQualityIndicatorFailureInfo
        /// </summary>
        /// <returns>Xml representation of DataQualityIndicatorFailureInfo object</returns>
        public String ToXml()
        {
            String xml = String.Empty;

            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            //Information node start
            xmlWriter.WriteStartElement("DataQualityIndicatorFailureInfo");

            xmlWriter.WriteAttributeString("AttributeId", AttributeId.ToString());

            xmlWriter.WriteAttributeString("AttributeValue", AttributeValue);

            xmlWriter.WriteAttributeString("RelationshipId", RelationshipId.ToString());

            xmlWriter.WriteAttributeString("LocaleId", LocaleId.ToString());

            xmlWriter.WriteAttributeString("Code", FailureMessageCode);

            xmlWriter.WriteStartElement("Message");
            xmlWriter.WriteRaw(FailureMessage.WrapWithCDataBlock());
            xmlWriter.WriteEndElement();

            if (this.Params != null)
            {
                xmlWriter.WriteStartElement("Params");
                foreach (Object str in this.Params)
                {
                    xmlWriter.WriteStartElement("Param");
                    xmlWriter.WriteRaw(str.ToString().WrapWithCDataBlock());
                    xmlWriter.WriteEndElement();
                }
                xmlWriter.WriteEndElement();
            }

            //Information node end
            xmlWriter.WriteEndElement();

            xmlWriter.Flush();

            //Get the actual XML
            xml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return xml;
        }

        /// <summary>
        /// Loads DataQualityIndicatorFailureInfo from XmlNode
        /// </summary>
        /// <param name="node">XmlNode for deserialization</param>
        public void LoadFromXml(XmlNode node)
        {
            if (node == null)
            {
                return;
            }

            if (node.Attributes != null)
            {
                if (node.Attributes["AttributeId"] != null)
                {
                    AttributeId = ValueTypeHelper.ConvertToNullableInt32(node.Attributes["AttributeId"].Value);
                }
                if (node.Attributes["AttributeValue"] != null)
                {
                    AttributeValue = node.Attributes["AttributeValue"].Value;
                }
                if (node.Attributes["RelationshipId"] != null)
                {
                    RelationshipId = ValueTypeHelper.ConvertToNullableInt64(node.Attributes["RelationshipId"].Value);
                }
                if (node.Attributes["LocaleId"] != null)
                {
                    LocaleId = ValueTypeHelper.ConvertToNullableInt16(node.Attributes["LocaleId"].Value);
                }
                if (node.Attributes["Code"] != null)
                {
                    FailureMessageCode = node.Attributes["Code"].Value;
                }
            }

            foreach (XmlNode child in node.ChildNodes)
            {
                String nodeName = child.Name;
                switch (nodeName)
                {
                    case "Message":
                        if (child is XmlCDataSection)
                        {
                            this.FailureMessage = (child as XmlCDataSection).Value;
                        }
                        break;
                    case "Params":
                        foreach (XmlCDataSection param in child.ChildNodes)
                        {
                            this.Params.Add(param.Value);
                        }
                        break;
                }
            }
        }

        /// <summary>
        /// Loads DataQualityIndicatorFailureInfo from XML with outer node
        /// </summary>
        /// <param name="xmlWithOuterNode">Xml for deserialization</param>
        public void LoadFromXmlWithOuterNode(String xmlWithOuterNode)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xmlWithOuterNode);
            XmlNode node = doc.SelectSingleNode("DataQualityIndicatorFailureInfo");
            if (node != null)
            {
                LoadFromXml(node);
            }
        }

        #endregion
        
        #region Equality Comparision

        /// <summary>
        /// Represents method for equality comparision
        /// </summary>
        /// <param name="other">Object for comparision</param>
        /// <returns>True if objects are equal, false otherwise</returns>
        public Boolean Equals(DataQualityIndicatorFailureInfo other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return String.Equals(_failureMessageCode, other._failureMessageCode) && String.Equals(_failureMessage, other._failureMessage) &&
                   _attributeId == other._attributeId && _relationshipId == other._relationshipId && _localeId == other._localeId &&
                   _params.SequenceEqual(other._params);
        }

        /// <summary>
        /// Represents method for equality comparision
        /// </summary>
        /// <param name="obj">Object for comparision</param>
        /// <returns>True if objects are equal, false otherwise</returns>
        public override Boolean Equals(object obj)
        {
            return Equals(obj as DataQualityIndicatorFailureInfo);
        }

        /// <summary>
        /// Represents method for generating HashCode for current object
        /// </summary>
        /// <returns>HashCode</returns>
        public override Int32 GetHashCode()
        {
            unchecked
            {
                Int32 hashCode = (_failureMessageCode != null ? _failureMessageCode.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (_failureMessage != null ? _failureMessage.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ _attributeId.GetHashCode();
                hashCode = (hashCode * 397) ^ _relationshipId.GetHashCode();
                hashCode = (hashCode * 397) ^ _localeId.GetHashCode();

                if (!_params.IsNullOrEmpty())
                {
                    hashCode = _params.Aggregate(hashCode, (current, param) => (current*397) ^ (param != null ? param.GetHashCode() : 0));
                }

                return hashCode;
            }
        }

        #endregion

        #endregion
    }
}