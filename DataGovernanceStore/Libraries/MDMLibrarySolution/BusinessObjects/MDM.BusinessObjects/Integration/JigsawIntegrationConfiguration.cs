using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Runtime.Serialization;
using System.Web.UI.WebControls;
using System.Xml;
using System.Xml.Serialization;
using MDM.Core.Extensions;

namespace MDM.BusinessObjects.Integration
{
    using MDM.Interfaces;
    using MDM.Core;

    /// <summary>
    /// Specifies JigsawIntegrationConfiguration class
    /// </summary>
    [DataContract]
    public class JigsawIntegrationConfiguration : IJigsawIntegrationConfiguration
    {
        /// <summary>
        /// Specifies ZookeeperConfiguration
        /// </summary>
        [DataMember]
        public Collection<JigsawNode> ZookeeperConfiguration { get; set; }

        /// <summary>
        /// Specifies KafkaConfiguration
        /// </summary>
        [DataMember]
        public Collection<JigsawNode> KafkaConfiguration { get; set; }

        /// <summary>
        /// Specifies TopicConfiguration
        /// </summary>
        [DataMember]
        public Collection<JigsawTopic> TopicConfiguration { get; set; }

        /// <summary>
        /// Gets or sets the report configuration.
        /// </summary>
        [DataMember]
        public Collection<JigsawReportConfigurationInstance> ReportConfiguration { get; set; }

        /// <summary>
        /// Specifies TenantConfiguration
        /// </summary>
        [DataMember]
        public JigsawTenant TenantConfiguration { get; set; }

        /// <summary>
        /// Property denoting the match configuration.
        /// </summary>
        [DataMember]
        public JigsawMatchConfiguration MatchConfiguration
        {
            get;
            set;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="JigsawIntegrationConfiguration"/> class.
        /// </summary>
        public JigsawIntegrationConfiguration()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="JigsawIntegrationConfiguration"/> class.
        /// </summary>
        /// <param name="xml">The XML.</param>
        public JigsawIntegrationConfiguration(String xml)
        {
            LoadJigsawIntegrationConfiguration(xml);
        }

        /// <summary>
        /// To the XML.
        /// </summary>
        /// <returns></returns>
        public String ToXml()
        {
            String xml = String.Empty;

            using (StringWriter sw = new StringWriter())
            using (XmlTextWriter xmlWriter = new XmlTextWriter(sw))
            {
                xmlWriter.WriteStartElement("ZookeeperConfiguration");
                if (this.ZookeeperConfiguration != null)
                {
                    foreach (JigsawNode jigsawNode in ZookeeperConfiguration)
                    {
                        xmlWriter.WriteStartElement("Node");
                        jigsawNode.WriteXml(xmlWriter);
                        xmlWriter.WriteEndElement();
                    }
                }
                xmlWriter.WriteEndElement();

                xmlWriter.WriteStartElement("KafkaConfiguration");
                if (this.KafkaConfiguration != null)
                {
                    foreach (JigsawNode jigsawNode in KafkaConfiguration)
                    {
                        xmlWriter.WriteStartElement("Node");
                        jigsawNode.WriteXml(xmlWriter);
                        xmlWriter.WriteEndElement();
                    }
                }
                xmlWriter.WriteEndElement();

                xmlWriter.WriteStartElement("TopicConfiguration");
                if (this.TopicConfiguration != null)
                {
                    foreach (JigsawTopic jigsawTopic in TopicConfiguration)
                    {
                        xmlWriter.WriteStartElement("Topic");
                        jigsawTopic.WriteXml(xmlWriter);
                        xmlWriter.WriteEndElement();
                    }
                }
                xmlWriter.WriteEndElement();

                xmlWriter.WriteStartElement("TenantConfiguration");
                if (this.TenantConfiguration != null)
                {
                    TenantConfiguration.WriteXml(xmlWriter);
                }
                xmlWriter.WriteEndElement();
                
                xmlWriter.WriteStartElement("ReportConfiguration");
                if (this.ReportConfiguration != null)
                {
                    foreach (JigsawReportConfigurationInstance reportConfig in ReportConfiguration)
                    {
                        xmlWriter.WriteStartElement("Config");
                        reportConfig.WriteXml(xmlWriter);
                        xmlWriter.WriteEndElement();
                    }
                }
                xmlWriter.WriteEndElement();

                xmlWriter.WriteStartElement("MatchConfiguration");
                if (this.MatchConfiguration != null)
                {
                    MatchConfiguration.WriteXml(xmlWriter);
                }
                xmlWriter.WriteEndElement();

                xmlWriter.Flush();

                //get the actual XML
                xml = sw.ToString();
            }

            return xml;
        }

        /// <summary>
        /// Creates a new object that is a copy of the current instance.
        /// </summary>
        /// <returns>
        /// A new object that is a copy of this instance.
        /// </returns>
        public Object Clone()
        {
            JigsawIntegrationConfiguration clonedConfiguration = new JigsawIntegrationConfiguration();

            clonedConfiguration.KafkaConfiguration = this.KafkaConfiguration.CloneCollection();
            clonedConfiguration.ZookeeperConfiguration = this.ZookeeperConfiguration.CloneCollection();
            clonedConfiguration.TopicConfiguration = this.TopicConfiguration.CloneCollection();
            clonedConfiguration.ReportConfiguration = this.ReportConfiguration.CloneCollection();
            if (this.TenantConfiguration != null)
            {
                clonedConfiguration.TenantConfiguration = this.TenantConfiguration.Clone() as JigsawTenant;
            }

            if (this.MatchConfiguration != null)
            {
                clonedConfiguration.MatchConfiguration = this.MatchConfiguration.Clone() as JigsawMatchConfiguration;
            }

            return clonedConfiguration;
        }

        /// <summary>
        /// Initialize ConnectorProfile from xml.
        /// </summary>
        /// <param name="valuesAsXml">ConnectorProfile in xml format</param>
        private void LoadJigsawIntegrationConfiguration(String valuesAsXml)
        {
            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                using (XmlTextReader reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null))
                {
                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "JigsawIntegrationConfiguration" && reader.IsStartElement())
                        {
                            //Nothing to read as of now
                            reader.Read();
                        }
                        else if (reader.NodeType == XmlNodeType.Element)
                        {
                            if (reader.Name == "ZookeeperConfiguration")
                            {
                                String xml = reader.ReadInnerXml();
                                if (!String.IsNullOrWhiteSpace(xml))
                                {
                                    Collection<JigsawNode> configs = LoadCollection<JigsawNode>(xml, "Node");
                                    if (configs != null)
                                    {
                                        this.ZookeeperConfiguration = configs;
                                    }
                                }
                            }
                            if (reader.Name == "KafkaConfiguration")
                            {
                                String xml = reader.ReadInnerXml();
                                if (!String.IsNullOrWhiteSpace(xml))
                                {
                                    Collection<JigsawNode> configs = LoadCollection<JigsawNode>(xml, "Node");
                                    if (configs != null)
                                    {
                                        this.KafkaConfiguration = configs;
                                    }
                                }
                            }
                            if (reader.Name == "TopicConfiguration")
                            {
                                String xml = reader.ReadInnerXml();
                                if (!String.IsNullOrWhiteSpace(xml))
                                {
                                    Collection<JigsawTopic> configs = LoadCollection<JigsawTopic>(xml, "Topic");
                                    if (configs != null)
                                    {
                                        this.TopicConfiguration = configs;
                                    }
                                }
                            }
                            if (reader.Name == "TenantConfiguration")
                            {
                                TenantConfiguration = new JigsawTenant();
                                TenantConfiguration.ReadXml(reader);
                                reader.Read();
                            }
                            if (reader.Name == "ReportConfiguration")
                            {
                                String xml = reader.ReadInnerXml();
                                if (!String.IsNullOrWhiteSpace(xml))
                                {
                                    Collection<JigsawReportConfigurationInstance> configs = LoadCollection<JigsawReportConfigurationInstance>(xml, "Config");
                                    if (configs != null)
                                    {
                                        this.ReportConfiguration = configs;
                                    }
                                }
                            }
                            if (reader.Name == "MatchConfiguration")
                            {
                                MatchConfiguration = new JigsawMatchConfiguration();
                                MatchConfiguration.ReadXml(reader);
                                reader.Read();
                            }
                        }
                        else
                        {
                            reader.Read();
                        }
                    }
                }
            }
        }

        private Collection<T> LoadCollection<T>(String xml, String nodeName) where T : IXmlSerializable, new()
        {
            Collection<T> result = new Collection<T>();

            using (XmlReader reader = new XmlTextReader(xml, XmlNodeType.Element, null))
            {
                while (!reader.EOF)
                {
                    if (reader.NodeType == XmlNodeType.Element && reader.Name == nodeName)
                    {
                        T item = new T();
                        item.ReadXml(reader);
                        result.Add(item);
                    }

                    reader.Read();
                }
            }

            return result;
        }
    }
}