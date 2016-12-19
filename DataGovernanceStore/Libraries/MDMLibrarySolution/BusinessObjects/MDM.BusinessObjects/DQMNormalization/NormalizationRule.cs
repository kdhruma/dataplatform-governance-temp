using System;
using System.Globalization;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;

namespace MDM.BusinessObjects.DQMNormalization
{
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Specifies Normalization Rule
    /// </summary>
    [DataContract]
    public class NormalizationRule : MDMObject, INormalizationRule, ICloneable
    {
        #region Fields

        /// <summary>
        /// Field describing Outer Node name in NormalizationRule column
        /// </summary>
        private const String RuleNodeName = "NormalizationRule";

        #endregion

        #region Properties

        /// <summary>
        /// Specifies rule description
        /// </summary>
        [DataMember]
        public String Description { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Creates a clone copy of the normalization rule object
        /// </summary>
        /// <returns>Returns a clone copy of the normalization rule object</returns>
        public Object Clone()
        {
            NormalizationRule rule = (NormalizationRule)this.MemberwiseClone();
            return rule;
        }

        /// <summary>
        /// Get Xml representation (including MDMObject's properties) of NormalizationRule
        /// </summary>
        /// <returns></returns>
        public override String ToXml()
        {
            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            //NormalizationRule node starts
            xmlWriter.WriteStartElement(RuleNodeName);

            xmlWriter.WriteAttributeString("Id", Id.ToString(CultureInfo.InvariantCulture));
            xmlWriter.WriteAttributeString("Name", Name);
            xmlWriter.WriteAttributeString("LongName", LongName);
            xmlWriter.WriteAttributeString("Action", Action.ToString());
            xmlWriter.WriteAttributeString("AuditRefId", AuditRefId.ToString(CultureInfo.InvariantCulture));
            xmlWriter.WriteAttributeString("ReferenceId", ReferenceId);

            //Write NormalizationRule properties
            xmlWriter.WriteRaw(PropertiesOnlyToXml());

            //NormalizationRule node end
            xmlWriter.WriteEndElement();

            xmlWriter.Flush();

            //get the actual XML
            String normalizationRuleAsString = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return normalizationRuleAsString;
        }

        /// <summary>
        /// Get Xml representation of Settings only properties (MDMObject properties and Description property will be excluded)
        /// </summary>
        public String PropertiesOnlyToXml()
        {
            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            xmlWriter.WriteStartElement("Description");

            xmlWriter.WriteRaw(Description);

            xmlWriter.WriteEndElement();

            xmlWriter.Flush();

            //Get the actual XML
            String description = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return description;
        }

        /// <summary>
        /// Loads NormalizationRule properties from XML
        /// </summary>
        /// <param name="xmlData"></param>
        /// <param name="onlyProperties"></param>
        public void LoadFromXml(String xmlData, Boolean onlyProperties)
        {
            XmlDocument xDoc = new XmlDocument();
            xDoc.LoadXml(xmlData);

            XmlNode node = xDoc.SelectSingleNode(RuleNodeName);
            if (node != null)
            {
                LoadFromXml(node, onlyProperties);
            }
        }

        /// <summary>
        /// Loads Settings only properties (MDMObject properties and Description property will be excluded) from XML
        /// </summary>
        public void LoadPropertiesOnlyFromXml(String xmlData)
        {
            LoadFromXml(xmlData, true);
        }

        /// <summary>
        /// Loads NormalizationRule from XML node
        /// </summary>
        /// <param name="node">NormalizationRule xml node</param>
        /// <param name="propertiesOnly">Flag which indicates if MDMObject's properties should be avoided</param>
        private void LoadFromXml(XmlNode node, Boolean propertiesOnly)
        {
            if (node == null)
            {
                return;
            }
            
            if (!propertiesOnly && node.Attributes != null && node.Attributes.Count > 0)
            {
                if (node.Attributes["Id"] != null)
                {
                    Id = ValueTypeHelper.Int32TryParse(node.Attributes["Id"].InnerText, 0);
                }

                if (node.Attributes["Name"] != null)
                {
                    Name = node.Attributes["Name"].InnerText;
                }

                if (node.Attributes["LongName"] != null)
                {
                    LongName = node.Attributes["LongName"].InnerText;
                }

                if (node.Attributes["AuditRefId"] != null)
                {
                    AuditRefId = ValueTypeHelper.Int32TryParse(node.Attributes["AuditRefId"].InnerText, 0);
                }

                if (node.Attributes["Action"] != null)
                {
                    ObjectAction action;
                    Enum.TryParse(node.Attributes["Action"].InnerText, out action);
                    Action = action;
                }
            }

            XmlNode descriptionNode = node.SelectSingleNode(@"Description");
            if (descriptionNode != null)
            {
                Description = descriptionNode.InnerText;
            }
        }

        #endregion
    }
}