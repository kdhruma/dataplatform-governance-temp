using System;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;

namespace MDM.BusinessObjects.Integration
{
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Contains configuration options for qualifying queue
    /// </summary>
    [DataContract]
    [KnownType(typeof(ScheduleCriteria))]
    public class AggregationConfiguration : IAggregationConfiguration
    {
        #region Fields

        /// <summary>
        /// Indicates time when the inbound message needs to be taken up for aggregation
        /// </summary>
        private ScheduleCriteria _inboundScheduleCriteria = new ScheduleCriteria();

        /// <summary>
        /// Indicates time when the outbound message needs to be taken up for aggregation
        /// </summary>
        private ScheduleCriteria _outboundScheduleCriteria = new ScheduleCriteria();

        /// <summary>
        /// Indicates whether aggregation is enabled for inbound messages
        /// </summary>
        private Boolean _enabledForInbound = false;

        /// <summary>
        /// Indicated whether aggregation is enabled for outbound messages
        /// </summary>
        private Boolean _enabledForOutbound = false;

        /// <summary>
        /// Indicates the batch size for inbound messages aggregation
        /// </summary>
        private Int32 _inboundBatchSize = 0;

        /// <summary>
        /// Indicates the batch size for outbound messages aggregation
        /// </summary>
        private Int32 _outboundBatchSize = 0;

        #endregion Fields

        #region Constructor

        /// <summary>
        /// Parameterless Constructor
        /// </summary>
        public AggregationConfiguration()
            : base()
        {
        }

        /// <summary>
        /// Xml constructor
        /// </summary>
        /// <param name="valuesAsXml">Value in xml format</param>
        public AggregationConfiguration(String valuesAsXml)
        {
            LoadAggregationConfiguration(valuesAsXml);
        }

        #endregion Constructor

        #region Properties

        /// <summary>
        /// Indicates time when the inbound message needs to be taken up for aggregation
        /// </summary>
        [DataMember]
        public ScheduleCriteria InboundScheduleCriteria
        {
            get { return _inboundScheduleCriteria; }
            set { _inboundScheduleCriteria = value; }
        }

        /// <summary>
        /// Indicates time when the outbound message needs to be taken up for aggregation
        /// </summary>
        [DataMember]
        public ScheduleCriteria OutboundScheduleCriteria
        {
            get { return _outboundScheduleCriteria; }
            set { _outboundScheduleCriteria = value; }
        }

        /// <summary>
        /// Indicates whether aggregation is enabled for inbound messages
        /// </summary>
        [DataMember]
        public Boolean IsEnabledForInbound
        {
            get { return _enabledForInbound; }
            set { _enabledForInbound = value; }
        }

        /// <summary>
        /// Indicates whether aggregation is enabled for outbound messages
        /// </summary>
        [DataMember]
        public Boolean IsEnabledForOutbound
        {
            get { return _enabledForOutbound; }
            set { _enabledForOutbound = value; }
        }

        /// <summary>
        /// Indicates the batch size for inbound messages aggregation
        /// </summary>
        [DataMember]
        public Int32 InboundBatchSize
        {
            get { return _inboundBatchSize; }
            set { _inboundBatchSize = value; }
        }

        /// <summary>
        /// Indicates the batch size for outbound messages aggregation
        /// </summary>
        [DataMember]
        public Int32 OutboundBatchSize
        {
            get { return _outboundBatchSize; }
            set { _outboundBatchSize = value; }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Represents ProcessingConfiguration in Xml format
        /// </summary>
        public String ToXml()
        {
            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);
            String xml = String.Empty;

            xmlWriter.WriteStartElement("AggregationConfiguration");

            xmlWriter.WriteStartElement("Inbound");
            xmlWriter.WriteAttributeString("Enabled", this.IsEnabledForInbound.ToString());
            xmlWriter.WriteAttributeString("BatchSize", this.InboundBatchSize.ToString());


            if (this.InboundScheduleCriteria != null)
            {
                xmlWriter.WriteRaw(this.InboundScheduleCriteria.ToXml());
            }

            //Inbound end
            xmlWriter.WriteEndElement();
            
            xmlWriter.WriteStartElement("Outbound");
            xmlWriter.WriteAttributeString("Enabled", this.IsEnabledForOutbound.ToString());
            xmlWriter.WriteAttributeString("BatchSize", this.OutboundBatchSize.ToString());


            if (this.OutboundScheduleCriteria != null)
            {
                xmlWriter.WriteRaw(this.OutboundScheduleCriteria.ToXml());
            }

            //Outbound end
            xmlWriter.WriteEndElement();

            //AggregationConfiguration end
            xmlWriter.WriteEndElement();

            xmlWriter.Flush();

            //get the actual XML
            xml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return xml;
        }

        /// <summary>
        /// Clone the AggregationConfiguration object and return new instance with the same values.
        /// </summary>
        /// <returns>New cloned IAggregationConfiguration</returns>
        public IAggregationConfiguration Clone()
        {
            AggregationConfiguration clone = new AggregationConfiguration();
            clone.InboundScheduleCriteria = ( ScheduleCriteria )this.InboundScheduleCriteria.Clone();
            clone.OutboundScheduleCriteria = (ScheduleCriteria)this.OutboundScheduleCriteria.Clone();
            clone.IsEnabledForInbound = this.IsEnabledForInbound;
            clone.IsEnabledForOutbound = this.IsEnabledForOutbound;
            clone.InboundBatchSize = this.InboundBatchSize;
            clone.OutboundBatchSize = this.OutboundBatchSize;

            return clone;
        }

        /// <summary>
        /// Get schedule criteria for aggregating inbound messages
        /// </summary>
        /// <returns></returns>
        public IScheduleCriteria GetInboundScheduleCriteria()
        {
            return this.InboundScheduleCriteria;
        }

        /// <summary>
        /// Get schedule criteria for aggregating outbound messages
        /// </summary>
        /// <returns></returns>
        public IScheduleCriteria GetOutboundScheduleCriteria()
        {
            return this.OutboundScheduleCriteria;
        }

        /// <summary>
        /// Compares expected aggregationConfiguration with current aggregationConfiguration instance
        /// </summary>
        /// <param name="aggregationConfiguration">Expected value of AggregationConfiguration</param>
        /// <returns>True : Is both are same. False : otherwise</returns>
        public Boolean IsSuperSetOf(AggregationConfiguration aggregationConfiguration)
        {
            if (aggregationConfiguration != null)
            {
                if (!this.InboundScheduleCriteria.IsSuperSetOf(aggregationConfiguration.InboundScheduleCriteria))
                {
                    return false;
                }

                if (!this.OutboundScheduleCriteria.IsSuperSetOf(aggregationConfiguration.OutboundScheduleCriteria))
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Initialize AggregationConfiguration from xml.
        /// </summary>
        /// <param name="valuesAsXml">AggregationConfiguration in xml format</param>
        private void LoadAggregationConfiguration(String valuesAsXml)
        {
            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                XmlTextReader reader = null;
                try
                {
                    reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);
                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "AggregationConfiguration" && reader.IsStartElement())
                        {
                            //Nothing to read as of now
                            reader.Read();
                        }
                        else if (reader.NodeType == XmlNodeType.Element && reader.Name == "Inbound")
                        {
                            if (reader.HasAttributes)
                            {
                                if (reader.MoveToAttribute("Enabled"))
                                    this.IsEnabledForInbound = ValueTypeHelper.BooleanTryParse(reader.ReadContentAsString(), false);

                                if (reader.MoveToAttribute("BatchSize"))
                                    this.InboundBatchSize = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), 0);
                            }
                                                        
                            if (reader.Read() && reader.NodeType == XmlNodeType.Element && reader.Name == "ScheduleCriteria")
                            {
                                String xml = reader.ReadOuterXml();
                                if (!String.IsNullOrWhiteSpace(xml))
                                {
                                    ScheduleCriteria criteria = new ScheduleCriteria(xml);
                                    if (criteria != null)
                                    {
                                        this.InboundScheduleCriteria = criteria;
                                    }
                                }
                            }
                        }
                        else if (reader.NodeType == XmlNodeType.Element && reader.Name == "Outbound")
                        {
                            if (reader.HasAttributes)
                            {
                                if (reader.MoveToAttribute("Enabled"))
                                    this.IsEnabledForOutbound = ValueTypeHelper.BooleanTryParse(reader.ReadContentAsString(), false);

                                if (reader.MoveToAttribute("BatchSize"))
                                    this.OutboundBatchSize = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), 0);
                            }

                            if (reader.Read() && reader.NodeType == XmlNodeType.Element && reader.Name == "ScheduleCriteria")
                            {
                                String xml = reader.ReadOuterXml();
                                if (!String.IsNullOrWhiteSpace(xml))
                                {
                                    ScheduleCriteria criteria = new ScheduleCriteria(xml);
                                    if (criteria != null)
                                    {
                                        this.OutboundScheduleCriteria = criteria;
                                    }
                                }
                            }
                        }
                        else
                        {
                            reader.Read();
                        }
                    }
                }
                finally
                {
                    if (reader != null)
                    {
                        reader.Close();
                    }
                }
            }
        }

        #endregion Methods
    }
}
