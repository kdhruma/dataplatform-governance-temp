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
    /// Represents class for normalization ruleset
    /// </summary>
    [DataContract]
    public class NormalizationRuleset : MDMObject, INormalizationRuleset, ICloneable
    {
        #region Properties

        /// <summary>
        /// Property denoting Attribute Id
        /// </summary>
        [DataMember]
        public Int32? AttributeId { get; set; }

        /// <summary>
        /// Property denoting value of Ruleset
        /// </summary>
        [DataMember]
        public String Value { get; set; }

        /// <summary>
        /// Property denoting Rule Id
        /// </summary>
        [DataMember]
        public Int32 RuleId { get; set; }

        /// <summary>
        /// Property denoting Relationship Type Id
        /// </summary>
        [DataMember]
        public Int32? RelationshipTypeId { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Creates a clone copy of normalization ruleset object
        /// </summary>
        /// <returns>Returns a clone copy of normalization ruleset object</returns>
        public object Clone()
        {
            NormalizationRuleset ruleset = (NormalizationRuleset)this.MemberwiseClone();
            return ruleset;
        }

        /// <summary>
        /// Determines whether two Object instances are equal
        /// </summary>
        /// <param name="obj">The Object to compare with the current Object</param>
        /// <returns>true if the specified Object is equal to the current Object; otherwise, false</returns>
        public override Boolean Equals(object obj)
        {
            if (base.Equals(obj))
            {
                if (obj is NormalizationRuleset)
                {
                    NormalizationRuleset objectToBeCompared = obj as NormalizationRuleset;

                    return
                        this.RuleId == objectToBeCompared.RuleId &&
                        this.Value == objectToBeCompared.Value &&
                        this.AttributeId == objectToBeCompared.AttributeId &&
                        this.RelationshipTypeId == objectToBeCompared.RelationshipTypeId; 

                }
            }
            return false;
        }

        /// <summary>
        /// Serves as a hash function for type
        /// </summary>
        /// <returns>A hash code for the current Object</returns>
        public override Int32 GetHashCode()
        {
            return
                base.GetHashCode()
                ^ this.RuleId.GetHashCode()
                ^ this.Value.GetHashCode()
                ^ this.AttributeId.GetHashCode();
        }

        /// <summary>
        /// Get Xml representation of Settings only properties (MDMObject properties and UserId property will be excluded)
        /// </summary>
        public String PropertiesOnlyToXml()
        {
            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            //Ruleset node start
            xmlWriter.WriteStartElement("Ruleset");

            xmlWriter.WriteAttributeString("RuleId", string.Format("{0}", RuleId));

            xmlWriter.WriteAttributeString("AttributeId", string.Format("{0}", AttributeId));

            xmlWriter.WriteAttributeString("Value", Value.ToString(CultureInfo.InvariantCulture));

            xmlWriter.WriteAttributeString("RelationshipTypeId", string.Format("{0}", RelationshipTypeId));


            //Ruleset node end
            xmlWriter.WriteEndElement();

            xmlWriter.Flush();

            //Get the actual XML
            String operationResultXml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return operationResultXml;
        }

        /// <summary>
        /// Loads Settings only properties (MDMObject properties and UserId property will be excluded) from XML
        /// </summary>
        public void LoadPropertiesOnlyFromXml(String xmlData)
        {
            XmlDocument xDoc = new XmlDocument();
            xDoc.LoadXml(xmlData);

            #region Read Ruleset

            XmlNodeList nodes = xDoc.SelectNodes(@"/Ruleset");
            if (nodes != null)
            {
                foreach (XmlNode node in nodes)
                {

                    if (node.Attributes != null)
                    {
                        Int32 ruleId;
                        if (node.Attributes["RuleId"] != null && Int32.TryParse(node.Attributes["RuleId"].Value, out ruleId))
                        {
                            RuleId = ruleId;
                        }

                        Int32 attributeId;
                        if (node.Attributes["AttributeId"] != null && Int32.TryParse(node.Attributes["AttributeId"].Value, out attributeId))
                        {
                            AttributeId = attributeId;
                        }

                        if (node.Attributes["Value"] != null)
                        {
                            Value = node.Attributes["Value"].Value;
                        }

                        if (node.Attributes["RelationshipTypeId"] != null)
                        {
                            RelationshipTypeId = ValueTypeHelper.ConvertToNullableInt32(node.Attributes["RelationshipTypeId"].Value);
                        }

                    }
                }
            }

            #endregion Read Rulesets
        }

        #endregion
    }
}