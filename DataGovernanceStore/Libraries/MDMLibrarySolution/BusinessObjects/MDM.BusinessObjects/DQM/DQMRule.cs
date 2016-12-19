using System;
using System.Globalization;
using System.IO;
using System.Xml;

namespace MDM.BusinessObjects.DQM
{
    using MDM.Interfaces;
    using MDM.Core;

    /// <summary>
    /// Represents class for DQM rule
    /// </summary>
    public class DQMRule : DQMJobProfile, IDQMRule
    {
        /// <summary>
        /// Constructs DQM rule
        /// </summary>
        public DQMRule()
            : base(DQMJobType.Matching)
        {
        }

        /// <summary>
        /// Constructs DQM rule using specified instance data
        /// </summary>
        public DQMRule(DQMRule source)
            : base(source)
        {
            Id = source.Id;
            ShortName = source.ShortName;
            LongName = source.LongName;
            Description = source.Description;
            RuleData = source.RuleData;
            Weightage = source.Weightage;
            Enabled = source.Enabled;
            DeleteFlag = source.DeleteFlag;
            JobType = source.JobType;
            AuditRef = source.AuditRef;
        }

        #region ICloneable Members

        /// <summary>
        /// Clone DQM rule
        /// </summary>
        /// <returns>Cloned DQM rule object</returns>
        public override object Clone()
        {
            DQMRule clonedRule = new DQMRule(this);
            return clonedRule;
        }

        #endregion

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
        public String RuleData { get; set; }

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


        #region Xml Serialization

        /// <summary>
        /// Gets Xml representation of current object
        /// </summary>
        /// <returns>Returns Xml representation of current object as string</returns>
        public override String ToXml()
        {
            String result = String.Empty;

            using (StringWriter sw = new StringWriter())
            using (XmlTextWriter xmlWriter = new XmlTextWriter(sw))
            {
                xmlWriter.WriteStartElement("DQMRule");

                xmlWriter.WriteAttributeString("Id", Id.ToString(CultureInfo.InvariantCulture));
                xmlWriter.WriteAttributeString("ShortName", ShortName);
                xmlWriter.WriteAttributeString("LongName", LongName);
                xmlWriter.WriteAttributeString("Description", Description);
                xmlWriter.WriteAttributeString("Weightage", Weightage.ToString(CultureInfo.InvariantCulture));
                xmlWriter.WriteAttributeString("Enabled", Enabled.ToString());
                xmlWriter.WriteAttributeString("DeleteFlag", DeleteFlag.ToString());
                xmlWriter.WriteAttributeString("JobType", JobType.ToString());
                xmlWriter.WriteAttributeString("AuditRef", AuditRef.ToString(CultureInfo.InvariantCulture));

                xmlWriter.WriteStartElement("RuleData");
                xmlWriter.WriteRaw(RuleData);
                xmlWriter.WriteEndElement();

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
                if (node.Attributes["Id"] != null)
                {
                    Id = ValueTypeHelper.Int32TryParse(node.Attributes["Id"].Value, Id);
                }
                if (node.Attributes["ShortName"] != null)
                {
                    Name = node.Attributes["ShortName"].Value;
                }
                if (node.Attributes["LongName"] != null)
                {
                    LongName = node.Attributes["LongName"].Value;
                }
                if (node.Attributes["Description"] != null)
                {
                    Description = node.Attributes["Description"].Value;
                }
                if (node.Attributes["Weightage"] != null)
                {
                    Weightage = ValueTypeHelper.Int32TryParse(node.Attributes["Weightage"].Value, Weightage);
                }
                if (node.Attributes["Enabled"] != null)
                {
                    Enabled = ValueTypeHelper.BooleanTryParse(node.Attributes["Enabled"].Value, Enabled);
                }
                if (node.Attributes["DeleteFlag"] != null)
                {
                    DeleteFlag = ValueTypeHelper.BooleanTryParse(node.Attributes["DeleteFlag"].Value, DeleteFlag);
                }
                if (node.Attributes["JobType"] != null)
                {
                    DQMJobType tempJobType;
                    ValueTypeHelper.EnumTryParse(node.Attributes["JobType"].Value, true, out tempJobType);
                    JobType = tempJobType;
                }
                if (node.Attributes["AuditRef"] != null)
                {
                    AuditRef = ValueTypeHelper.Int64TryParse(node.Attributes["AuditRef"].Value, AuditRef);
                }

                XmlNode tempNode = node.SelectSingleNode("RuleData");
                if (tempNode != null)
                {
                    RuleData = tempNode.InnerText;
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
            XmlNode node = doc.SelectSingleNode("DQMRule");
            if (node != null)
            {
                LoadFromXml(node);
            }
        }

        #endregion


    }
}
