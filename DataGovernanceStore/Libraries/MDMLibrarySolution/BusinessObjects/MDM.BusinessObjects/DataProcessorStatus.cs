using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Runtime.Serialization;
using System.Globalization;

namespace MDM.BusinessObjects
{
    using MDM.Core;
    using MDM.Interfaces;
    
    /// <summary>
    /// Represents class for data processor status
    /// </summary>
    [DataContract]
    public class DataProcessorStatus : ObjectBase, IDataProcessorStatus
    {
        #region Private fields

        /// <summary>
        /// Indicates pending item counts which are yet to be processed
        /// </summary>
        private Int64 _pendingItemCount = -1;

        /// <summary>
        /// Indicates name of processor
        /// </summary>
        private String _processorName = String.Empty;

        /// <summary>
        /// Indicates if the processor is initialized
        /// </summary>
        private Boolean _isInitialized = false;

        /// <summary>
        /// Indicates the time stamp when last batch of data was read from system
        /// </summary>
        private DateTime? _lastPollTime = null;

        /// <summary>
        /// Indicates how many items were fetched in last data poll
        /// </summary>
        private Int64 _lastResultItemCount = -1;

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

        #endregion Private fields

        #region Constructors

        /// <summary>
        /// Parameterless constructor
        /// </summary>
        public DataProcessorStatus()
        {
        }

        /// <summary>
        /// Constructor which takes xml as input parameters
        /// </summary>
        /// <param name="valueAsXml">Indicates values as xml</param>
        public DataProcessorStatus(String valueAsXml)
        {
            LoadDataProcessorStatus(valueAsXml);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Indicates pending item counts which are yet to be processed
        /// </summary>
        [DataMember]
        public Int64 PendingItemCount
        {
            get { return _pendingItemCount; }
            set { _pendingItemCount = value; }
        }

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
        /// Indicates if the processor is initialized
        /// </summary>
        [DataMember]
        public Boolean IsInitialized
        {
            get { return _isInitialized; }
            set { _isInitialized = value; }
        }

        /// <summary>
        /// Indicates the time stamp when last batch of data was read from system
        /// </summary>
        [DataMember]
        public Int64 LastResultItemCount
        {
            get { return _lastResultItemCount; }
            set { _lastResultItemCount = value; }
        }

        /// <summary>
        /// Indicates how many items were fetched in last data poll
        /// </summary>
        [DataMember]
        public DateTime? LastPollTime
        {
            get { return _lastPollTime; }
            set { _lastPollTime = value; }
        }

        /// <summary>
        /// No. of items to read from database for each batch.
        /// </summary>
        public Int32 SourceDataBatchSize
        {
            get { return this._sourceDataBatchSize; }
            set { this._sourceDataBatchSize = value; }
        }

        /// <summary>
        /// Indicates how many items to put in memory queue for processing.
        /// </summary>
        public Int32 DataBufferThreshold
        {
            get { return this._dataBufferThreshold; }
            set { this._dataBufferThreshold = value; }
        }

        /// <summary>
        /// No. of threads per processor.
        /// </summary>
        public Int32 ThreadCount
        {
            get { return this._threadCount; }
            set { this._threadCount = value; }
        }

        /// <summary>
        /// polling interval (In seconds) for reading the next batch for processing.
        /// </summary>
        public Int32 DataPollingIntervalInSeconds
        {
            get { return this._dataPollingIntervalInSeconds; }
            set { this._dataPollingIntervalInSeconds = value; }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Get Xml representation of DataProcessorStatus
        /// </summary>  
        /// <returns>Xml representation of object</returns>
        public String ToXml()
        {
            String xml = String.Empty;
            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            //DataProcessorStatus node start
            xmlWriter.WriteStartElement("DataProcessorStatus");

            #region Write Properties

            xmlWriter.WriteAttributeString("ProcessorName", this.ProcessorName.ToString());
            xmlWriter.WriteAttributeString("IsInitialized", this.IsInitialized.ToString().ToLowerInvariant());
            //G: 6/15/2008 9:15:07 PM (datetime format)
            xmlWriter.WriteAttributeString("LastPollTime", this.LastPollTime.GetValueOrDefault().ToString("G", CultureInfo.CreateSpecificCulture("en-US")));
            xmlWriter.WriteAttributeString("LastResultItemCount", this.LastResultItemCount.ToString());
            xmlWriter.WriteAttributeString("PendingItemCount", this.PendingItemCount.ToString());
            xmlWriter.WriteAttributeString("SourceDataBatchSize", this.SourceDataBatchSize.ToString());
            xmlWriter.WriteAttributeString("DataBufferThreshold", this.DataBufferThreshold.ToString());
            xmlWriter.WriteAttributeString("DataPollingIntervalInSeconds", this.DataPollingIntervalInSeconds.ToString());
            xmlWriter.WriteAttributeString("ThreadCount", this.ThreadCount.ToString());

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
        /// Load data processor status object from xml
        /// </summary>
        /// <param name="valueAsXml">Indicates xml representation of data processor status</param>
        public void LoadDataProcessorStatus(String valueAsXml)
        {
            if (!String.IsNullOrWhiteSpace(valueAsXml))
            {
                XmlTextReader reader = null;
                try
                {
                    reader = new XmlTextReader(valueAsXml, XmlNodeType.Element, null);

                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name.ToLowerInvariant() == "dataprocessorstatus")
                        {
                            if (reader.HasAttributes)
                            {
                                if (reader.MoveToAttribute("ProcessorName"))
                                {
                                    this.ProcessorName = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("IsInitialized"))
                                {
                                    this.IsInitialized = ValueTypeHelper.ConvertToBoolean(reader.ReadContentAsString());
                                }

                                if (reader.MoveToAttribute("LastPollTime"))
                                {
                                    String lastPollTime = reader.ReadContentAsString();
                                    if (!String.IsNullOrWhiteSpace(lastPollTime))
                                    {
                                        DateTime result;
                                        CultureInfo cultureInfo = new CultureInfo("en-US");
                                        DateTimeStyles styles = DateTimeStyles.None;
                                        if (DateTime.TryParse(lastPollTime, cultureInfo, styles, out result))
                                        {
                                            this.LastPollTime = result;
                                        }
                                    }
                                }

                                if (reader.MoveToAttribute("LastResultItemCount"))
                                {
                                    this.LastResultItemCount = ValueTypeHelper.Int64TryParse(reader.ReadContentAsString(), 0);
                                }

                                if (reader.MoveToAttribute("PendingItemCount"))
                                {
                                    this.PendingItemCount = ValueTypeHelper.Int64TryParse(reader.ReadContentAsString(), 0);
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
