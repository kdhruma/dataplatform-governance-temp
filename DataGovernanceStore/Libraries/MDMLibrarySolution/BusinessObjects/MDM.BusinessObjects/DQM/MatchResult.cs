using System;
using System.Runtime.Serialization;
using System.Xml;

namespace MDM.BusinessObjects.DQM
{

    /// <summary>
    /// Specifies Match Result
    /// </summary>
    [DataContract]
    public class MatchResult
    {
        /// <summary>
        /// Parameterless constructor
        /// </summary>
        public MatchResult() { }

        /// <summary>
        /// Constructor which takes xml node as object representation
        /// </summary>
        /// <param name="xmlNode"></param>
        public MatchResult(XmlNode xmlNode)
        {
            LoadFromXml(xmlNode);
        }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public String FieldName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public String FieldValue { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public String Locale { get; set; }

        #region Methods

        private void LoadFromXml(XmlNode xmlNode)
        {
            if (xmlNode != null && xmlNode.Attributes != null && xmlNode.Attributes.Count > 0)
            {
                if (xmlNode.Attributes["FieldName"] != null)
                {
                    FieldName = xmlNode.Attributes["FieldName"].InnerText;
                }

                if (xmlNode.Attributes["FieldValue"] != null)
                {
                    FieldValue = xmlNode.Attributes["FieldValue"].InnerText;
                }

                if (xmlNode.Attributes["Locale"] != null)
                {
                    Locale = xmlNode.Attributes["Locale"].InnerText;
                }
            }
        }

        #endregion
    }
}
