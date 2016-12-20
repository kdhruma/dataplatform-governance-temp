using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Runtime.Serialization;

namespace MDM.BusinessObjects
{
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    [KnownType(typeof(DataProcessorStatusCollection))]
    public class ParallelizationEngineStatus : ObjectBase, IParallelizationEngineStatus
    {
        #region Lock Object

        // lock object for the load all..singleton
        private static Object lockObj = new Object();

        #endregion Lock Object

        #region Private fields

        /// <summary>
        /// Indicates if engine is started or not
        /// </summary>
        private Boolean _isParallizationProcessingEngineStarted = false;

        /// <summary>
        /// Indicates number of processors in engine
        /// </summary>
        private Int32 _processorCount = -1;

        /// <summary>
        /// Indicates status of all processors available with engine
        /// </summary>
        private DataProcessorStatusCollection _dataProcessorStatusCollection = new DataProcessorStatusCollection();

        #endregion Private fields

        #region Constructor

        /// <summary>
        ///  Parameterless Constructor
        /// </summary>
        public ParallelizationEngineStatus()
        {
        }

        /// <summary>
        ///  Parameterized Constructor with Values as xml
        /// </summary>
        /// <param name="valueAsXml">String values in XML format which has to be set when object is initialized.</param>
        public ParallelizationEngineStatus(String valueAsXml)
        {
            LoadParallelizationEngineStatus(valueAsXml);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Indicates if engine is started or not
        /// </summary>
        [DataMember]
        public Boolean IsParallizationProcessingEngineStarted
        {
            get { return _isParallizationProcessingEngineStarted; }
            set { _isParallizationProcessingEngineStarted = value; }
        }

        /// <summary>
        /// Indicates number of processors in engine
        /// </summary>
        [DataMember]
        public Int32 ProcessorCount
        {
            get { return _processorCount; }
            set { _processorCount = value; }
        }

        /// <summary>
        /// Indicates status of all processors available with engine
        /// </summary>
        [DataMember]
        public DataProcessorStatusCollection DataProcessorStatusCollection
        {
            get { return _dataProcessorStatusCollection; }
            set { _dataProcessorStatusCollection = value; }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Get Xml representation of status
        /// </summary>
        /// <returns>Xml representation of object</returns>
        public String ToXml()
        {
            String xml = String.Empty;
            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            //ParallelizationEngineStatus node start
            xmlWriter.WriteStartElement("ParallelizationEngineStatus");

            #region Write Properties

            xmlWriter.WriteAttributeString("IsParallizationProcessingEngineOn", this.IsParallizationProcessingEngineStarted.ToString().ToLowerInvariant());
            xmlWriter.WriteAttributeString("ProcessorCount", this.ProcessorCount.ToString());

            xmlWriter.WriteRaw(this.DataProcessorStatusCollection.ToXml());

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
        /// Loads the parallelization Engine current Status
        /// </summary>
        /// <param name="valueAsXml">String Values in XMl format which has value for current engine status.</param>
        public void LoadParallelizationEngineStatus(String valueAsXml)
        {
            if (!String.IsNullOrWhiteSpace(valueAsXml))
            {
                XmlTextReader reader = null;
                try
                {
                    reader = new XmlTextReader(valueAsXml, XmlNodeType.Element, null);

                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name.ToLowerInvariant() == "parallelizationenginestatus")
                        {
                            if (reader.HasAttributes)
                            {
                                if (reader.MoveToAttribute("IsParallizationProcessingEngineOn"))
                                {
                                    this.IsParallizationProcessingEngineStarted = ValueTypeHelper.ConvertToBoolean(reader.ReadContentAsString());
                                }

                                if (reader.MoveToAttribute("ProcessorCount"))
                                {
                                    this.ProcessorCount = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), 0);
                                }
                            }
                            else
                            {
                                reader.Read();
                            }
                        }
                        else if (reader.NodeType == XmlNodeType.Element && reader.Name.ToLowerInvariant() == "dataprocessorstatuscollection")
                        {
                            String DataProcessorStatusXml = reader.ReadOuterXml();
                            if (!String.IsNullOrEmpty(DataProcessorStatusXml))
                            {
                                DataProcessorStatusCollection dataProcessorStatus = new DataProcessorStatusCollection(DataProcessorStatusXml);
                                this.DataProcessorStatusCollection = dataProcessorStatus;
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
