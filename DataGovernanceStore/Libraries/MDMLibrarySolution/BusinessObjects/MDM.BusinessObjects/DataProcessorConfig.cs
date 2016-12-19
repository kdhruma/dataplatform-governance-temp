using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;

namespace MDM.BusinessObjects
{
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Specifies class for data processor configuration
    /// </summary>
    [DataContract]
    public class DataProcessorConfig : ObjectBase, IDataProcessorConfig
    {

        #region Private fields

        /// <summary>
        /// Indicates name of processor
        /// </summary>
        private String _processorName = String.Empty;

        /// <summary>
        /// No. of items to read from database for each batch.
        /// </summary>
        private Int32 _sourceDataBatchSize = 0;

        /// <summary>
        /// Indicates how many items to put in memory queue for processing.
        /// </summary>
        private Int32 _dataBufferThreshold = 0;

        /// <summary>
        /// No. of threads per processor.
        /// </summary>
        private Int32 _threadCount = 0;

        /// <summary>
        /// polling interval (In seconds) for reading the next batch for processing.
        /// </summary>
        private Int32 _dataPollingIntervalInSeconds = 0;

        /// <summary>
        /// Status of the Processor
        /// </summary>
        private Boolean _isProcessorRunning = true;

        #endregion Private fields

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public DataProcessorConfig()
        {
        }

        /// <summary>
        /// Constructor with parameter
        /// </summary>
        /// <param name="valueAsXml">xml</param>
        public DataProcessorConfig(String valueAsXml)
        {
            LoadDataProcessorConfig(valueAsXml);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Indicates name of processor
        /// </summary>
        [DataMember]
        public String ProcessorName
        {
            get { return _processorName; }
            set { _processorName = value; }
        }

        /// <summary>
        /// No. of items to read from database for each batch.
        /// </summary>
        [DataMember]
        public Int32 SourceDataBatchSize
        {
            get { return this._sourceDataBatchSize; }
            set { this._sourceDataBatchSize = value; }
        }

        /// <summary>
        /// Indicates how many items to put in memory queue for processing.
        /// </summary>
        [DataMember]
        public Int32 DataBufferThreshold
        {
            get { return this._dataBufferThreshold; }
            set { this._dataBufferThreshold = value; }
        }

        /// <summary>
        /// No. of threads per processor.
        /// </summary>
        [DataMember]
        public Int32 ThreadCount
        {
            get { return this._threadCount; }
            set { this._threadCount = value; }
        }

        /// <summary>
        /// polling interval (In seconds) for reading the next batch for processing.
        /// </summary>
        [DataMember]
        public Int32 DataPollingIntervalInSeconds
        {
            get { return this._dataPollingIntervalInSeconds; }
            set { this._dataPollingIntervalInSeconds = value; }
        }

        /// <summary>
        /// Status of the Processor
        /// </summary>
        [DataMember]
        public Boolean IsProcessorRunning
        {
            get { return this._isProcessorRunning; }
            set { this._isProcessorRunning = value; }
        }
        #endregion Properties

        #region Methods

        /// <summary>
        /// Get Xml representation of DataProcessorConfig
        /// </summary>  
        /// <returns>Xml representation of object</returns>
        public String ToXml()
        {
            String xml = String.Empty;
            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            //DataProcessorConfig node start
            xmlWriter.WriteStartElement("DataProcessorConfig");

            #region Write Properties

            xmlWriter.WriteAttributeString("ProcessorName", this.ProcessorName.ToString());
            xmlWriter.WriteAttributeString("SourceDataBatchSize", this.SourceDataBatchSize.ToString());
            xmlWriter.WriteAttributeString("DataBufferThreshold", this.DataBufferThreshold.ToString());
            xmlWriter.WriteAttributeString("DataPollingIntervalInSeconds", this.DataPollingIntervalInSeconds.ToString());
            xmlWriter.WriteAttributeString("ThreadCount", this.ThreadCount.ToString());
            xmlWriter.WriteAttributeString("IsProcessorRunning", this.IsProcessorRunning.ToString());

            #endregion

            //Denorm Result node end
            xmlWriter.WriteEndElement();

            xmlWriter.Flush();

            //get the actual XML
            xml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return xml;
        }

        /// <summary>
        /// Load data processor configuration object from XML
        /// </summary>
        /// <param name="valueAsXml">XML having xml value</param>
        public void LoadDataProcessorConfig(String valueAsXml)
        {
            if (!String.IsNullOrWhiteSpace(valueAsXml))
            {
                XmlTextReader reader = null;
                try
                {
                    reader = new XmlTextReader(valueAsXml, XmlNodeType.Element, null);

                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name.ToLowerInvariant() == "dataprocessorconfig")
                        {
                            if (reader.HasAttributes)
                            {
                                if (reader.MoveToAttribute("ProcessorName"))
                                {
                                    this.ProcessorName = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("SourceDataBatchSize"))
                                {
                                    this.SourceDataBatchSize = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), 0);
                                }

                                if (reader.MoveToAttribute("DataBufferThreshold"))
                                {
                                    this.DataBufferThreshold = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), 0);
                                }

                                if (reader.MoveToAttribute("DataPollingIntervalInSeconds"))
                                {
                                    this.DataPollingIntervalInSeconds = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), 0);
                                }

                                if (reader.MoveToAttribute("ThreadCount"))
                                {
                                    this.ThreadCount = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), 0);
                                }

                                if (reader.MoveToAttribute("IsProcessorRunning"))
                                {
                                    this.IsProcessorRunning = ValueTypeHelper.BooleanTryParse(reader.ReadContentAsString(), false);
                                }
                            }
                            else
                            {
                                reader.Read();
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
