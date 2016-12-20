using System;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;

namespace MDM.BusinessObjects
{
    using MDM.Core;
    using MDM.BusinessObjects.Interfaces;

    /// <summary>
    /// Specifies SurvivorshipRule
    /// </summary>
    [DataContract]
    public class SurvivorshipRule : ISurvivorshipRule
    {
        #region Fields

        /// <summary>
        /// Field for Conditions
        /// </summary>
        private SurvivorshipRuleAttributeValueConditionCollection _conditions = new SurvivorshipRuleAttributeValueConditionCollection();

        /// <summary>
        /// Field for Sources
        /// </summary>
        private SurvivorshipRuleSourceListItemCollection _sources = new SurvivorshipRuleSourceListItemCollection();

        /// <summary>
        /// Field for ExternalStrategy
        /// </summary>
        private String _externalStrategy = String.Empty;

        /// <summary>
        /// Field for CollectionStrategy
        /// </summary>
        private CollectionStrategy _collectionStrategy = Core.CollectionStrategy.Unknown;

        #endregion

        #region Properties

        /// <summary>
        /// Property denoting Conditions
        /// </summary>
        [DataMember]
        public SurvivorshipRuleAttributeValueConditionCollection Conditions
        {
            get { return _conditions; }
            set { _conditions = value; }
        }

        /// <summary>
        /// Property denoting Sources
        /// </summary>
        [DataMember]
        public SurvivorshipRuleSourceListItemCollection Sources
        {
            get { return _sources; }
            set { _sources = value; }
        }

        /// <summary>
        /// Property denoting ExternalStrategy
        /// </summary>
        [DataMember]
        public String ExternalStrategy
        {
            get { return _externalStrategy; }
            set { _externalStrategy = value; }
        }

        /// <summary>
        /// Property denoting CollectionStrategy
        /// </summary>
        [DataMember]
        public CollectionStrategy CollectionStrategy
        {
            get { return _collectionStrategy; }
            set { _collectionStrategy = value; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructs SurvivorshipRule
        /// </summary>
        public SurvivorshipRule()
        {

        }

        /// <summary>
        /// Copy constructor. Constructs SurvivorshipRule using specified instance data
        /// </summary>
        public SurvivorshipRule(SurvivorshipRule source)
            : base()
        {
            this.Conditions = (SurvivorshipRuleAttributeValueConditionCollection)source.Conditions.Clone();
            this.Sources = (SurvivorshipRuleSourceListItemCollection)source.Sources.Clone();
            this.ExternalStrategy = source.ExternalStrategy;
            this.CollectionStrategy = source.CollectionStrategy;
        }

        #endregion
        
        #region Public Methods

        /// <summary>
        /// Get Xml representation of MatchingRule
        /// </summary>
        public String ToXml()
        {
            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);
            
            //SurvivorshipRule node start
            xmlWriter.WriteStartElement("SurvivorshipRule");
            
            xmlWriter.WriteStartElement("Conditions");
            xmlWriter.WriteRaw(Conditions.ToXml());
            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement("Sources");
            xmlWriter.WriteRaw(Sources.ToXml());
            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement("ExternalStrategy");
            xmlWriter.WriteAttributeString("Name", ExternalStrategy ?? String.Empty);
            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement("CollectionStrategy");
            xmlWriter.WriteAttributeString("Name", CollectionStrategy.ToString());
            xmlWriter.WriteEndElement();

            //SurvivorshipRule node end
            xmlWriter.WriteEndElement();

            xmlWriter.Flush();

            //Get the actual XML
            String result = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return result;
        }

        /// <summary>
        /// Loads SurvivorshipRule from XML node
        /// </summary>
        public void LoadFromXml(XmlNode node)
        {
            Clear();
            foreach (XmlNode child in node.ChildNodes)
            {
                if (child.Name == "Conditions")
                {
                    Conditions.LoadFromXml(child);
                }
                else if (child.Name == "Sources")
                {
                    Sources.LoadFromXml(child);
                }
                else if ((child.Name == "CustomStrategy" || child.Name == "ExternalStrategy") && child.Attributes["Name"] != null)
                {
                    ExternalStrategy = child.Attributes["Name"].Value;
                }
                else if (child.Name == "CollectionStrategy" && child.Attributes["Name"] != null)
                {
                    CollectionStrategy collectionStrategyTmp;
                    if (Enum.TryParse(child.Attributes["Name"].Value, true, out collectionStrategyTmp))
                    {
                        CollectionStrategy = collectionStrategyTmp;
                    }
                }
            }
        }

        /// <summary>
        /// Loads SurvivorshipRule from string xml
        /// </summary>
        public void LoadFromXml(String xml)
        {
            Clear();
            if (!String.IsNullOrWhiteSpace(xml))
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xml);

                XmlNode node = doc.SelectSingleNode("SurvivorshipRule");
                if (node != null)
                {
                    LoadFromXml(node);
                }
            }
        }

        #endregion

        #region ICloneable Members

        /// <summary>
        /// Clone SurvivorshipRule
        /// </summary>
        /// <returns>Cloned SurvivorshipRule object</returns>
        public object Clone()
        {
            SurvivorshipRule cloned = new SurvivorshipRule(this);
            return cloned;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Clears SurvivorshipRule data
        /// </summary>
        private void Clear()
        {
            this.Conditions.Clear();
            this.Sources.Clear();
            this.ExternalStrategy = String.Empty;
            this.CollectionStrategy = CollectionStrategy.Unknown;
        }

        #endregion
    }
}
