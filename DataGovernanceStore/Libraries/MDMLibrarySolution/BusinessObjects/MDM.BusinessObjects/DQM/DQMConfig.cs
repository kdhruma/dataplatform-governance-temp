using System;
using System.Globalization;
using System.IO;
using System.Xml;

namespace MDM.BusinessObjects.DQM
{
    using MDM.Interfaces;
    using MDM.Core;

    /// <summary>
    /// Indicates class for DQM Configuration
    /// </summary>
    public class DQMConfig : DQMJobProfile, IDQMConfig
    {
        #region Constructors

        /// <summary>
        /// Constructs DQMConfig
        /// </summary>
        public DQMConfig()
            : base(DQMJobType.Matching)
        {
        }

        /// <summary>
        /// Constructs DQMConfig using specified instance data
        /// </summary>
        public DQMConfig(DQMConfig source)
            : base(source)
        {
            ConfigId = source.ConfigId;
            ShortName = source.ShortName;
            LongName = source.LongName;
            Description = source.Description;
            ConfigData = source.ConfigData;
            Weightage = source.Weightage;
            Enabled = source.Enabled;
            DeleteFlag = source.DeleteFlag;
            JobType = source.JobType;
            AuditRef = source.AuditRef;
        }

        #endregion

        #region ICloneable Members

        /// <summary>
        /// Clone DQMConfig
        /// </summary>
        /// <returns>Cloned DQMConfig object</returns>
        public override object Clone()
        {
            DQMConfig clonedRule = new DQMConfig(this);
            return clonedRule;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Config Id
        /// </summary>
        public Int64 ConfigId { get; set; }

        /// <summary>
        /// Short Name
        /// </summary>
        public String ShortName { get; set; }

        /// <summary>
        /// Long Name
        /// </summary>
        public override String LongName { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        public String Description { get; set; }

        /// <summary>
        /// Rule Xml
        /// </summary>
        public String ConfigData { get; set; }

        /// <summary>
        /// Weightage
        /// </summary>
        public new Int32 Weightage { get; set; }

        /// <summary>
        /// Enabled
        /// </summary>
        public new Boolean Enabled { get; set; }

        /// <summary>
        /// Enabled
        /// </summary>
        public Boolean DeleteFlag { get; set; }

        /// <summary>
        /// Audit Ref
        /// </summary>
        public Int64 AuditRef { get; set; }

        #endregion
        
        #region Xml Serialization

        /// <summary>
        /// Gets Xml representation of current object
        /// </summary>
        /// <returns>Returns Xml representation of current object as string</returns>
        public String ToXmlWithOuterNode()
        {
            String result = String.Empty;

            using (StringWriter sw = new StringWriter())
            using (XmlTextWriter xmlWriter = new XmlTextWriter(sw))
            {
                xmlWriter.WriteStartElement("DQMConfig");

                xmlWriter.WriteAttributeString("ConfigId", Id.ToString(CultureInfo.InvariantCulture));
                xmlWriter.WriteAttributeString("ShortName", ShortName);
                xmlWriter.WriteAttributeString("LongName", LongName);
                xmlWriter.WriteAttributeString("AuditRef", AuditRef.ToString(CultureInfo.InvariantCulture));
                xmlWriter.WriteAttributeString("DeleteFlag", DeleteFlag.ToString());
                xmlWriter.WriteAttributeString("Description", Description);
                xmlWriter.WriteAttributeString("Enabled", Enabled.ToString());
                xmlWriter.WriteAttributeString("Weightage", Weightage.ToString(CultureInfo.InvariantCulture));

                xmlWriter.WriteElementString("ConfigData", ConfigData);

                xmlWriter.WriteEndElement();
                xmlWriter.Flush();

                // Get the actual XML
                result = sw.ToString();
            }

            return result;
        }

        /// <summary>
        /// Loads current object from provided XmlNode
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
                if (node.Attributes["ConfigId"] != null)
                {
                    ConfigId = ValueTypeHelper.Int64TryParse(node.Attributes["ConfigId"].Value, ConfigId);
                }
                if (node.Attributes["ShortName"] != null)
                {
                    ShortName = node.Attributes["ShortName"].Value;
                }
                if (node.Attributes["LongName"] != null)
                {
                    LongName = node.Attributes["LongName"].Value;
                }
                if (node.Attributes["AuditRef"] != null)
                {
                    AuditRef = ValueTypeHelper.Int64TryParse(node.Attributes["AuditRef"].Value, AuditRef);
                }
                if (node.Attributes["DeleteFlag"] != null)
                {
                    DeleteFlag = ValueTypeHelper.BooleanTryParse(node.Attributes["DeleteFlag"].Value, DeleteFlag);
                }
                if (node.Attributes["Description"] != null)
                {
                    Description = node.Attributes["Description"].Value;
                }
                if (node.Attributes["Enabled"] != null)
                {
                    Enabled = ValueTypeHelper.BooleanTryParse(node.Attributes["Enabled"].Value, Enabled);
                }
                if (node.Attributes["Weightage"] != null)
                {
                    Weightage = ValueTypeHelper.Int32TryParse(node.Attributes["Weightage"].Value, Weightage);
                }

                XmlNode tempNode = node.SelectSingleNode("ConfigData");
                if (tempNode != null)
                {
                    ConfigData = tempNode.InnerText;
                }
            }
        }

        /// <summary>
        /// Loads current object from provided Xml
        /// </summary>
        /// <param name="xml">Xml string for deserialization</param>
        public void LoadFromXml(String xml)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);
            XmlNode node = doc.SelectSingleNode("DQMConfig");
            if (node != null)
            {
                LoadFromXml(node);
            }
        }

        #endregion

    }
}