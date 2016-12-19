using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;

namespace MDM.BusinessObjects.DQM
{
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Specifies Validation Profile settings
    /// </summary>
    [DataContract]
    public class ValidationProfile : DQMJobProfile, IValidationProfile
    {
        #region Fields

        /// <summary>
        /// Field denoting validation DataQualityIndicator Ids
        /// </summary>
        private Collection<Int16> _dataQualityIndicatorIds = new Collection<Int16>();

        /// <summary>
        /// Field denoting Data Locales collection
        /// </summary>
        private Collection<LocaleEnum> _dataLocales = new Collection<LocaleEnum>();

        #endregion

        #region Constuctors

        /// <summary>
        /// Constructs Validation Profile
        /// </summary>
        public ValidationProfile()
            : base(DQMJobType.Validation)
        {
            this.DataQualityIndicatorIds = null;
        }

        /// <summary>
        /// Constructs Validation Profile using specified instance data
        /// </summary>
        public ValidationProfile(ValidationProfile source)
            : base(source)
        {
            this.DataQualityIndicatorIds = source.DataQualityIndicatorIds == null ? null : new Collection<Int16>(source.DataQualityIndicatorIds);
            this.DataLocales = source.DataLocales == null ? null : new Collection<LocaleEnum>(source.DataLocales);
        }

        #endregion

        #region Properties
        
        /// <summary>
        /// Property denoting validation DataQualityIndicator Ids
        /// </summary>
        [DataMember]
        public Collection<Int16> DataQualityIndicatorIds
        {
            get { return _dataQualityIndicatorIds; }
            set { _dataQualityIndicatorIds = value; }
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
        /// Clone validation profile object
        /// </summary>
        /// <returns>Returns cloned copy of validation profile copy</returns>
        public override object Clone()
        {
            ValidationProfile profile = new ValidationProfile(this);
            return profile;
        }

        /// <summary>
        /// Get Xml representation of Validation Profile properties (excluding MDMObject's properties)
        /// </summary>
        public override String PropertiesOnlyToXml()
        {
            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            xmlWriter.WriteRaw(base.PropertiesOnlyToXml());

            #region Write KPIs Ids

            xmlWriter.WriteStartElement("KPIs");

            if (DataQualityIndicatorIds != null)
            {
                foreach (Int16 dataQualityIndicatorId in DataQualityIndicatorIds)
                {
                    xmlWriter.WriteStartElement("KPI");

                    xmlWriter.WriteAttributeString("Id", dataQualityIndicatorId.ToString(CultureInfo.InvariantCulture));

                    xmlWriter.WriteEndElement();
                }
            }

            xmlWriter.WriteEndElement();

            #endregion Write KPI Ids

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
        /// Loads Validation Profile properties (excluding MDMObject's properties) from XML
        /// </summary>
        public override void LoadPropertiesOnlyFromXml(XmlNode xmlNode)
        {
            base.LoadPropertiesOnlyFromXml(xmlNode);

            #region Read KPIs Ids

            XmlNodeList nodes = xmlNode.SelectNodes(@"KPIs/KPI");
            DataQualityIndicatorIds = new Collection<Int16>();
            if (nodes != null)
            {
                foreach (XmlNode node in nodes)
                {
                    Int16 value;
                    if (node.Attributes != null && node.Attributes["Id"] != null && Int16.TryParse(node.Attributes["Id"].Value, out value))
                    {
                        if (!DataQualityIndicatorIds.Contains(value))
                        {
                            DataQualityIndicatorIds.Add(value);
                        }
                    }
                }
            }

            #endregion Read KPIs Ids

            #region Read Data Locales

            nodes = xmlNode.SelectNodes(@"DataLocales/Locale");
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

        #endregion

        #endregion
    }
}