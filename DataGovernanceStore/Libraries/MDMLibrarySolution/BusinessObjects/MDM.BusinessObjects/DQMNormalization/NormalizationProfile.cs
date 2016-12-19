using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;

namespace MDM.BusinessObjects.DQMNormalization
{
    using MDM.BusinessObjects.DQM;
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Represents class for normalization profile
    /// </summary>
    [DataContract]
    public class NormalizationProfile : DQMJobProfile, INormalizationProfile
    {
        #region Constants

        /// <summary>
        /// Field describing Outer Node name in ProfileData column
        /// </summary>
        private const String ProfileNodeName = "NormalizationProfile";

        #endregion

        #region Fields

        /// <summary>
        /// Field denoting Rule sets collection
        /// </summary>
        private NormalizationRulesetsCollection _rulesetsCollection = new NormalizationRulesetsCollection();

        /// <summary>
        /// Field denoting job simulation status
        /// </summary>
        private Boolean _isSimulation = false;

        /// <summary>
        /// Field denoting Data Locales collection
        /// </summary>
        private Collection<LocaleEnum> _dataLocales = new Collection<LocaleEnum>();

        #endregion

        #region Constructors

        /// <summary>
        /// Constructs Normalization Profile
        /// </summary>
        public NormalizationProfile()
            : base(DQMJobType.Normalization)
        {
        }

        /// <summary>
        /// Constructs Normalization Profile using specified instance data
        /// </summary>
        public NormalizationProfile(NormalizationProfile source)
            : base(source)
        {
            this.RulesetsCollection = source.RulesetsCollection == null ? null : (NormalizationRulesetsCollection)source.RulesetsCollection.Clone();
            this.IsSimulation = source.IsSimulation;
            this.DataLocales = source.DataLocales == null ? null : new Collection<LocaleEnum>(source.DataLocales);
        }

        /// <summary>
        /// Constructs Normalization Profile using xml string
        /// </summary>
        /// <param name="valuesAsXml"></param>
        public NormalizationProfile(String valuesAsXml): base(DQMJobType.Normalization)
        {
            LoadFromXmlWithOuterNode(valuesAsXml, false);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Property denoting Rule sets collection
        /// </summary>
        [DataMember]
        public NormalizationRulesetsCollection RulesetsCollection
        {
            get { return _rulesetsCollection; }
            set { _rulesetsCollection = value; }
        }

        /// <summary>
        /// Property denoting job simulation status
        /// </summary>
        [DataMember]
        public Boolean IsSimulation
        {
            get { return _isSimulation; }
            set { _isSimulation = value; }
        }

        /// <summary>
        /// Property denoting Data Locales collection
        /// </summary>
        [DataMember]
        public Collection<LocaleEnum> DataLocales
        {
            get { return _dataLocales; }
            set { _dataLocales = value; }
        }

        #endregion

        #region Methods

        #region Public Methods

        /// <summary>
        /// Create a clone copy of normalization profile object
        /// </summary>
        /// <returns>Returns a clone copy of normalization profile object</returns>
        public override object Clone()
        {
            NormalizationProfile profile = new NormalizationProfile(this);
            return profile;
        }

        /// <summary>
        /// Get Xml representation of Normalization Profile properties (excluding MDMObject's properties)
        /// </summary>
        public override String PropertiesOnlyToXml()
        {
            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            xmlWriter.WriteRaw(base.PropertiesOnlyToXml());

            #region Write Rulesets Collection

            xmlWriter.WriteStartElement("RulesetsCollection");

            if (RulesetsCollection != null)
            {
                foreach (NormalizationRuleset rule in RulesetsCollection)
                {
                    xmlWriter.WriteRaw(rule.PropertiesOnlyToXml());
                }
            }

            xmlWriter.WriteEndElement();

            #endregion Write Rulesets Collection

            xmlWriter.WriteStartElement("IsSimulation");

            xmlWriter.WriteValue(IsSimulation);
            
            xmlWriter.WriteEndElement();

            #region Write Data Locales

            xmlWriter.WriteStartElement("DataLocales");

            if (DataLocales != null)
            {
                foreach (LocaleEnum locale in DataLocales)
                {
                    xmlWriter.WriteStartElement("Locale");

                    xmlWriter.WriteAttributeString("Name", locale.ToString());

                    xmlWriter.WriteEndElement();
                }
            }

            xmlWriter.WriteEndElement();

            #endregion Write Data Locales

            xmlWriter.Flush();

            //Get the actual XML
            String result = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return result;
        }

        /// <summary>
        /// Loads Normalization Profile properties (excluding MDMObject's properties) from XML
        /// </summary>
        public override void LoadPropertiesOnlyFromXml(XmlNode xmlNode)
        {
            base.LoadPropertiesOnlyFromXml(xmlNode);

            #region Read Rulesets Collection

            XmlNode rulesetNode = xmlNode.SelectSingleNode("RulesetsCollection");
            if (rulesetNode != null)
            {
                RulesetsCollection = new NormalizationRulesetsCollection(rulesetNode.OuterXml);
            }

            #endregion Read Rulesets Collection

            XmlNode isSimulation = xmlNode.SelectSingleNode(@"IsSimulation");
            if (isSimulation != null)
            {
                this.IsSimulation = ValueTypeHelper.BooleanTryParse(isSimulation.InnerText, false);
            }

            #region Read Data Locales

            XmlNodeList nodes = xmlNode.SelectNodes(@"DataLocales/Locale");
            DataLocales = new Collection<LocaleEnum>();
            if (nodes != null)
            {
                foreach (XmlNode node in nodes)
                {
                    LocaleEnum value = LocaleEnum.UnKnown;
                    if (node.Attributes != null && node.Attributes["Name"] != null && Enum.TryParse(node.Attributes["Name"].Value, out value))
                    {
                        if (!DataLocales.Contains(value))
                        {
                            DataLocales.Add(value);
                        }
                    }
                }
            }

            #endregion Read Data Locales
        }

        /// <summary>
        /// Get Xml representation (including MDMObject's properties) of NormalizationProfile
        /// </summary>
        /// <returns></returns>
        public override String ToXml()
        {
            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            //NormalizationProfile node starts
            xmlWriter.WriteStartElement(ProfileNodeName);

            xmlWriter.WriteAttributeString("Id", Id.ToString(CultureInfo.InvariantCulture));
            xmlWriter.WriteAttributeString("Name", Name);
            xmlWriter.WriteAttributeString("LongName", LongName);
            xmlWriter.WriteAttributeString("Action", Action.ToString());
            xmlWriter.WriteAttributeString("AuditRefId", AuditRefId.ToString(CultureInfo.InvariantCulture));
            xmlWriter.WriteAttributeString("ReferenceId", ReferenceId);

            //Write NormalizationProfile Profile's properties
            xmlWriter.WriteRaw(PropertiesOnlyToXml());

            //NormalizationProfile node end
            xmlWriter.WriteEndElement();

            xmlWriter.Flush();

            //get the actual XML
            String normalizationProfileAsXml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return normalizationProfileAsXml;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Loads NormalizationProfile from XML
        /// </summary>
        /// <param name="valuesAsXml">NormalizationProfile xml string</param>
        /// <param name="propertiesOnly">If true, only NormalizationProfile's properties will be loaded 
        /// (excluding MDMObject's properties)</param>
        private void LoadFromXmlWithOuterNode(String valuesAsXml, Boolean propertiesOnly)
        {
            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(valuesAsXml);
                XmlNode node = doc.SelectSingleNode(ProfileNodeName);
                if (node != null)
                {
                    LoadFromXml(node, propertiesOnly);
                }
            }
        }

        /// <summary>
        /// Loads NormalizationProfile from XML node
        /// </summary>
        /// <param name="node">NormalizationProfile xml node</param>
        /// <param name="propertiesOnly">If true, only NormalizationProfile's properties will be loaded 
        /// (excluding MDMObject's properties)</param>
        public void LoadFromXml(XmlNode node, Boolean propertiesOnly)
        {
            if (node == null)
            {
                return;
            }

            #region Read Profile's properties

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

            #endregion Read Organization properties

            LoadPropertiesOnlyFromXml(node);
        }

        #endregion

        #endregion
    }
}