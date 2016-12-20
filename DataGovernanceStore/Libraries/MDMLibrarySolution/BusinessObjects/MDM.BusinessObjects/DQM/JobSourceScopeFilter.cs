using System;
using System.Runtime.Serialization;
using System.IO;
using System.Xml;

namespace MDM.BusinessObjects.DQM
{
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Specifies Job Source Scope Filter
    /// </summary>
    [DataContract]
    public class JobSourceScopeFilter : JobScopeFilter, IJobSourceScopeFilter
    {
        #region Fields

        /// <summary>
        /// Field for Delta Mode
        /// </summary>
        private Boolean _isDeltaMode = false;

        #endregion

        #region Properties

        /// <summary>
        /// Delta mode
        /// </summary>
        [DataMember]
        public Boolean IsDeltaMode
        {
            get { return _isDeltaMode; }
            set { _isDeltaMode = value; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructs JobSourceScopeFilter
        /// </summary>
        public JobSourceScopeFilter()
        {
        }

        /// <summary>
        /// Constructs JobSourceScopeFilter using specified instance data
        /// </summary>
        public JobSourceScopeFilter(JobSourceScopeFilter source)
            : base(source)
        {
            this.IsDeltaMode = source.IsDeltaMode;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Clones Job Source Scope Filter
        /// </summary>
        public override object Clone()
        {
            JobSourceScopeFilter result = new JobSourceScopeFilter(this);
            return result;
        }

        /// <summary>
        /// Get Xml representation of JobSourceScopeFilter
        /// </summary>
        public override String ToXml()
        {
            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            xmlWriter.WriteStartElement("IsDeltaMode");
            xmlWriter.WriteValue(IsDeltaMode);
            xmlWriter.WriteEndElement();

            xmlWriter.WriteRaw(base.ToXml());

            xmlWriter.Flush();

            //Get the actual XML
            String resultXml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return resultXml;
        }

        /// <summary>
        /// Get Xml representation of JobSourceScopeFilter with outer node
        /// </summary>
        public String ToXmlWithOuterNode()
        {
            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            xmlWriter.WriteStartElement("SourceScopeFilter");

            xmlWriter.WriteStartElement("IsDeltaMode");
            xmlWriter.WriteValue(IsDeltaMode);
            xmlWriter.WriteEndElement();

            xmlWriter.WriteRaw(base.ToXml());

            xmlWriter.WriteEndElement();

            xmlWriter.Flush();

            //Get the actual XML
            String resultXml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return resultXml;
        }

        /// <summary>
        /// Loads JobSourceScopeFilter from XML node
        /// </summary>
        public override void LoadFromXml(XmlNode xmlNode)
        {
            IsDeltaMode = false;

            base.LoadFromXml(xmlNode);
            
            XmlNode isDelta = xmlNode.SelectSingleNode(@"IsDeltaMode");
            if (isDelta != null)
            {
                this.IsDeltaMode = ValueTypeHelper.BooleanTryParse(isDelta.InnerText, false);
            }
            else
            {
                XmlNodeList nodes = xmlNode.SelectNodes(@"ValidationExtent");
                if (nodes != null)
                {
                    foreach (XmlNode node in nodes)
                    {
                        if (node.Attributes != null && node.Attributes["Extent"] != null && node.Attributes["Extent"].Value.Equals("DeltaContainerValidation", StringComparison.InvariantCultureIgnoreCase))
                        {
                            this.IsDeltaMode = true;
                            break;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Loads JobSourceScopeFilter from XML with outer node
        /// </summary>
        public void LoadFromXmlWithOuterNode(String xml)
        {
            if (!String.IsNullOrWhiteSpace(xml))
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xml);
                XmlNode node = doc.SelectSingleNode("SourceScopeFilter");
                if (node != null)
                {
                    LoadFromXml(node);
                }
            }
        }

        #endregion
    }
}