using System;
using System.Runtime.Serialization;
using System.Xml;

namespace MDM.BusinessObjects.DQM
{
    using MDM.Core;

    /// <summary>
    /// Match Source Attribute
    /// </summary>
    [DataContract]
    public class MatchSourceAttribute
    {
        /// <summary>
        /// Parameterless Constructor
        /// </summary>
        public MatchSourceAttribute() { }

        /// <summary>
        /// Constructor which takes xml node as object representation
        /// </summary>
        /// <param name="xmlNode"></param>
        public MatchSourceAttribute(XmlNode xmlNode)
        {
            LoadFromXml(xmlNode);
        }

        /// <summary>
        /// Property indicating attribute Id
        /// </summary>
        [DataMember]
        public Int32 AttributeId { get; set; }

        /// <summary>
        /// Property indicating attribute parent Id
        /// </summary>
        [DataMember]
        public Int32 AttributeParentId { get; set; }

        /// <summary>
        /// Property indicating attribute parent name
        /// </summary>
        [DataMember]
        public String AttributeParentName { get; set; }

        /// <summary>
        /// Property indicating attribute name
        /// </summary>
        [DataMember]
        public String AttributeName { get; set; }

        /// <summary>
        /// property indicating locale of match source attribute
        /// </summary>
        [DataMember]
        public LocaleEnum Locale { get; set; }

        /// <summary>
        /// Property indicating whether complex child flag is enabled or not
        /// </summary>
        [DataMember]
        public bool IsComplexChild { get; set; }

        /// <summary>
        /// Property indicating the attribute data type
        /// </summary>
        [IgnoreDataMember]
        public DataStoreFieldType AttributeDataType { get; set; }

        #region Methods

        private void LoadFromXml(XmlNode xmlNode)
        {
            if (xmlNode != null && xmlNode.Attributes != null && xmlNode.Attributes.Count > 0)
            {
                if (xmlNode.Attributes["AttributeId"] != null)
                {
                    AttributeId = ValueTypeHelper.Int32TryParse(xmlNode.Attributes["AttributeId"].InnerText, 0);
                }

                if (xmlNode.Attributes["AttributeParentId"] != null)
                {
                    AttributeParentId = ValueTypeHelper.Int32TryParse(xmlNode.Attributes["AttributeParentId"].InnerText, 0);
                }

                if (xmlNode.Attributes["AttributeName"] != null)
                {
                    AttributeName = xmlNode.Attributes["AttributeName"].InnerText;
                }

                if (xmlNode.Attributes["AttributeParentName"] != null)
                {
                    AttributeParentName = xmlNode.Attributes["AttributeParentName"].InnerText;
                }

                if (xmlNode.Attributes["Locale"] != null)
                {
                    LocaleEnum localeEnum;
                    ValueTypeHelper.EnumTryParse(xmlNode.Attributes["Locale"].InnerText, true, out localeEnum);

                    Locale = localeEnum;
                }

                if (xmlNode.Attributes["IsComplexChild"] != null)
                {
                    IsComplexChild = ValueTypeHelper.BooleanTryParse(xmlNode.Attributes["IsComplexChild"].InnerText, false);
                }
            }
        }

        #endregion
    }
}