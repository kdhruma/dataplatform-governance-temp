using System;
using System.Globalization;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;
using System.Collections.ObjectModel;

namespace MDM.BusinessObjects.DQM
{
    using MDM.Interfaces;
    using MDM.Core;

    /// <summary>
    /// Specifies filter criteria settings
    /// </summary>
    [DataContract]
    public class FilterCriteriaSettings : MDMObject, IFilterCriteriaSettings
    {
        #region Constants

        private const String dateTimeFormat = "yyyy-MM-ddTHH:mm:ss.fffffff";

        #endregion

        #region Constructors

        /// <summary>
        /// Parameterless constructor
        /// </summary>
        public FilterCriteriaSettings()
        {
            CategoryIds = new Collection<Int64>();
            EntityTypeIds = new Collection<Int32>();
            DataQualityIndicatorIds = new Collection<Int16>();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Property denoting user id
        /// </summary>
        [DataMember]
        public Int32 UserId { get; set; }

        /// <summary>
        /// Property denoting container id
        /// </summary>
        [DataMember]
        public Int32? ContainerId { get; set; }

        /// <summary>
        /// Property denoting category ids
        /// </summary>
        [DataMember]
        public Collection<Int64> CategoryIds { get; set; }

        /// <summary>
        /// Property denoting entity type ids
        /// </summary>
        [DataMember]
        public Collection<Int32> EntityTypeIds { get; set; }

        /// <summary>
        /// Property denoting DataQualityIndicator ids
        /// </summary>
        [DataMember]
        public Collection<Int16> DataQualityIndicatorIds { get; set; }

        /// <summary>
        /// Property denoting quality data Date time
        /// </summary>
        [DataMember]
        public DateTime? QualityDate { get; set; }

        /// <summary>
        /// Property denoting start trends date Date time
        /// </summary>
        [DataMember]
        public DateTime? StartTrendsDate { get; set; }

        /// <summary>
        /// Property denoting end trends date Date time
        /// </summary>
        [DataMember]
        public DateTime? EndTrendsDate { get; set; }

        #endregion

        #region Public Methods
        /// <summary>
        /// Create a new object of the same type and copy over the properties
        /// </summary>
        /// <returns>Returns the newly creared cloned object of filter criteria settings</returns>
        public object Clone()
        {
            FilterCriteriaSettings filterCriteriaSettings = (FilterCriteriaSettings)this.MemberwiseClone();

            filterCriteriaSettings.ContainerId = this.ContainerId;
            if (this.CategoryIds != null)
            {
                filterCriteriaSettings.CategoryIds = new Collection<Int64>(this.CategoryIds);
            }
            if (this.EntityTypeIds != null)
            {
                filterCriteriaSettings.EntityTypeIds = new Collection<Int32>(this.EntityTypeIds);
            }
            if (this.DataQualityIndicatorIds != null)
            {
                filterCriteriaSettings.DataQualityIndicatorIds = new Collection<Int16>(this.DataQualityIndicatorIds);
            }

            return filterCriteriaSettings;
        }

        /// <summary>
        /// Get Xml representation of Settings only properties (MDMObject properties and UserId property will be excluded)
        /// </summary>
        public String SettingsOnlyPropertiesToXml(Boolean convertDateTimePropertiesToUtc)
        {
            String operationResultXml = String.Empty;

            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            //Filter criteria node start
            xmlWriter.WriteStartElement("FilterCriteria");

            #region Write Container Ids

            xmlWriter.WriteStartElement("Containers");
            
            if (ContainerId.HasValue)
            {
                xmlWriter.WriteStartElement("Container");

                xmlWriter.WriteAttributeString("Id", ContainerId.Value.ToString(CultureInfo.InvariantCulture));

                xmlWriter.WriteEndElement();
            }

            xmlWriter.WriteEndElement();

            #endregion Write Container Ids

            #region Write Category Ids

            xmlWriter.WriteStartElement("Categories");

            if (CategoryIds != null)
            {
                foreach (var categoryId in CategoryIds)
                {
                    xmlWriter.WriteStartElement("Category");

                    xmlWriter.WriteAttributeString("Id", categoryId.ToString(CultureInfo.InvariantCulture));

                    xmlWriter.WriteEndElement();
                }
            }

            xmlWriter.WriteEndElement();

            #endregion Write Category Ids

            #region Write Entity Type Ids

            xmlWriter.WriteStartElement("EntityTypes");

            if (EntityTypeIds != null)
            {
                foreach (var entityTypeId in EntityTypeIds)
                {
                    xmlWriter.WriteStartElement("EntityType");

                    xmlWriter.WriteAttributeString("Id", entityTypeId.ToString(CultureInfo.InvariantCulture));

                    xmlWriter.WriteEndElement();
                }
            }

            xmlWriter.WriteEndElement();

            #endregion Write Entity Type Ids

            #region Write DataQualityIndicator Ids

            xmlWriter.WriteStartElement("DataQualityIndicators");

            if (DataQualityIndicatorIds != null)
            {
                foreach (var dataQualityIndicatorId in DataQualityIndicatorIds)
                {
                    xmlWriter.WriteStartElement("DataQualityIndicator");

                    xmlWriter.WriteAttributeString("Id", dataQualityIndicatorId.ToString(CultureInfo.InvariantCulture));

                    xmlWriter.WriteEndElement();
                }
            }

            xmlWriter.WriteEndElement();

            #endregion Write DataQualityIndicator Ids

            #region Write Date Filters

            WritedDateXmlNode(xmlWriter, "QualityDate", QualityDate, convertDateTimePropertiesToUtc);
            WritedDateXmlNode(xmlWriter, "StartTrendsDate", StartTrendsDate, convertDateTimePropertiesToUtc);
            WritedDateXmlNode(xmlWriter, "EndTrendsDate", EndTrendsDate, convertDateTimePropertiesToUtc);

            #endregion Write Date Filters

            //Filter criteria node end
            xmlWriter.WriteEndElement();

            xmlWriter.Flush();

            //Get the actual XML
            operationResultXml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return operationResultXml;
        }

        /// <summary>
        /// Loads Settings only properties (MDMObject properties and UserId property will be excluded) from XML
        /// </summary>
        public void LoadSettingsOnlyPropertiesFromXml(String xmlData)
        {
            XmlDocument xDoc = new XmlDocument();
            xDoc.LoadXml(xmlData);

            #region Read Container Ids

            XmlNodeList nodes = xDoc.SelectNodes(@"/FilterCriteria/Containers/Container");
            ContainerId = null;
            if (nodes != null)
            {
                foreach (XmlNode node in nodes)
                {
                    Int32 value;
                    if (node.Attributes != null && node.Attributes["Id"] != null && Int32.TryParse(node.Attributes["Id"].Value, out value))
                    {
                        ContainerId = value;
                    }
                }
            }

            #endregion Read Container Ids

            #region Read Category Ids

            nodes = xDoc.SelectNodes(@"/FilterCriteria/Categories/Category");
            CategoryIds = null;
            if (nodes != null)
            {
                foreach (XmlNode node in nodes)
                {
                    Int64 value;
                    if (node.Attributes != null && node.Attributes["Id"] != null && Int64.TryParse(node.Attributes["Id"].Value, out value))
                    {
                        if (CategoryIds == null)
                        {
                            CategoryIds = new Collection<Int64>();
                        }
                        if (!CategoryIds.Contains(value))
                        {
                            CategoryIds.Add(value);
                        }
                    }
                }
            }

            #endregion Read Category Ids

            #region Read Entity Type Ids

            nodes = xDoc.SelectNodes(@"/FilterCriteria/EntityTypes/EntityType");
            EntityTypeIds = null;
            if (nodes != null)
            {
                foreach (XmlNode node in nodes)
                {
                    Int32 value;
                    if (node.Attributes != null && node.Attributes["Id"] != null && Int32.TryParse(node.Attributes["Id"].Value, out value))
                    {
                        if (EntityTypeIds == null)
                        {
                            EntityTypeIds = new Collection<Int32>();
                        }
                        if (!EntityTypeIds.Contains(value))
                        {
                            EntityTypeIds.Add(value);
                        }
                    }
                }
            }

            #endregion Read Entity Type Ids

            #region Read DataQualityIndicator Ids

            nodes = xDoc.SelectNodes(@"/FilterCriteria/DataQualityIndicators/DataQualityIndicator");
            DataQualityIndicatorIds = null;
            if (nodes != null)
            {
                foreach (XmlNode node in nodes)
                {
                    Int16 value;
                    if (node.Attributes != null && node.Attributes["Id"] != null && Int16.TryParse(node.Attributes["Id"].Value, out value))
                    {
                        if (DataQualityIndicatorIds == null)
                        {
                            DataQualityIndicatorIds = new Collection<Int16>();
                        }
                        if (!DataQualityIndicatorIds.Contains(value))
                        {
                            DataQualityIndicatorIds.Add(value);
                        }
                    }
                }
            }

            #endregion Read DataQualityIndicator Ids

            #region Read Date Filters

            QualityDate = ReadDateFromXmlNode(xDoc, @"/FilterCriteria/QualityDate");
            StartTrendsDate = ReadDateFromXmlNode(xDoc, @"/FilterCriteria/StartTrendsDate");
            EndTrendsDate = ReadDateFromXmlNode(xDoc, @"/FilterCriteria/EndTrendsDate");

            #endregion Read Date Filters
        }

        #endregion

        #region Private Methods

        private void WritedDateXmlNode(XmlWriter writer, String nodeName, DateTime? dateTime, Boolean convertToUtc)
        {
            writer.WriteStartElement(nodeName);
            if (dateTime.HasValue)
            {
                DateTime date = convertToUtc ? dateTime.Value.ToUniversalTime() : dateTime.Value;
                writer.WriteAttributeString("Kind", date.Kind.ToString());
                writer.WriteAttributeString("Format", dateTimeFormat);
                writer.WriteValue(date.ToString(dateTimeFormat, CultureInfo.InvariantCulture));
            }
            writer.WriteEndElement();
        }

        private DateTime? ReadDateFromXmlNode(XmlDocument xmlDoc, String nodeXPath)
        {
            XmlNodeList nodes = xmlDoc.SelectNodes(nodeXPath);
            DateTime? result = null;
            if (nodes != null)
            {
                foreach (XmlNode node in nodes)
                {
                    if (node.InnerText != null && !String.IsNullOrWhiteSpace(node.InnerText))
                    {
                        String formatXml = null;
                        DateTimeKind? kindXml = null;
                        if (node.Attributes != null)
                        {
                            if (node.Attributes["Format"] != null && !String.IsNullOrWhiteSpace(node.Attributes["Format"].Value))
                            {
                                formatXml = node.Attributes["Format"].Value;
                            }
                            if (node.Attributes["Kind"] != null && !String.IsNullOrWhiteSpace(node.Attributes["Kind"].Value))
                            {
                                DateTimeKind kind;
                                if (DateTimeKind.TryParse(node.Attributes["Kind"].Value, true, out kind))
                                {
                                    kindXml = kind;
                                }
                            }
                        }

                        DateTime date;
                        if (DateTime.TryParseExact(node.InnerText, formatXml ?? dateTimeFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
                        {
                            result = new DateTime(date.Ticks, kindXml ?? DateTimeKind.Local).ToLocalTime();
                        }
                    }
                }
            }
            return result;
        }

        #endregion
    }
}