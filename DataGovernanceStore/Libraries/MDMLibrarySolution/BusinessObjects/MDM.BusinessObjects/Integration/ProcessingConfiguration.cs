﻿using System;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;

namespace MDM.BusinessObjects.Integration
{
    using MDM.Interfaces;

    /// <summary>
    /// Contains configuration options for processing queue
    /// </summary>
    [DataContract]
    [KnownType(typeof(ScheduleCriteria))]
    public class ProcessingConfiguration : IProcessingConfiguration
    {
        #region Fields

        /// <summary>
        /// Indicates time when the message needs to be taken up for processing
        /// </summary>
        private ScheduleCriteria _scheduleCriteria = new ScheduleCriteria();

        #endregion Fields

        #region Constructor

        /// <summary>
        /// Parameterless Constructor
        /// </summary>
        public ProcessingConfiguration()
            : base()
        {
        }

        /// <summary>
        /// Xml constructor
        /// </summary>
        /// <param name="valuesAsXml">Value in xml format</param>
        public ProcessingConfiguration(String valuesAsXml)
        {
            LoadProcessingConfiguration(valuesAsXml);
        }

        #endregion Constructor

        #region Properties

        /// <summary>
        /// Indicates time when the message needs to be taken up for processing
        /// </summary>
        [DataMember]
        public ScheduleCriteria ScheduleCriteria
        {
            get { return _scheduleCriteria; }
            set { _scheduleCriteria = value; }
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

            xmlWriter.WriteStartElement("ProcessingConfiguration");

            if (this.ScheduleCriteria != null)
            {
                xmlWriter.WriteRaw(this.ScheduleCriteria.ToXml());
            }

            //ProcessingConfiguration end
            xmlWriter.WriteEndElement();

            xmlWriter.Flush();

            //get the actual XML
            xml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return xml;
        }

        /// <summary>
        /// Clone the ProcessingConfiguration object and return new instance with the same values.
        /// </summary>
        /// <returns>New cloned IProcessingConfiguration</returns>
        public IProcessingConfiguration Clone()
        {
            ProcessingConfiguration clone = new ProcessingConfiguration();
            clone.ScheduleCriteria = ( ScheduleCriteria )this.ScheduleCriteria.Clone();

            return clone;
        }

        /// <summary>
        /// Get schedule criteria for processing
        /// </summary>
        /// <returns></returns>
        public IScheduleCriteria GetScheduleCriteria()
        {
            return this.ScheduleCriteria;
        }

        /// <summary>
        /// Checks whether expected output is a subset of actual output or not.
        /// </summary>
        /// <param name="processingConfiguration">Indicates expected processing configuration which is compared against actual processing configuration.</param>
        /// <returns>Returns true if the specified object is subset of the current object; otherwise false.</returns>
        public Boolean IsSuperSetOf(ProcessingConfiguration processingConfiguration)
        {
            if (processingConfiguration != null)
            {
                if (!this.ScheduleCriteria.IsSuperSetOf(processingConfiguration.ScheduleCriteria))
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
        /// Initialize ProcessingConfiguration from xml.
        /// </summary>
        /// <param name="valuesAsXml">ProcessingConfiguration in xml format</param>
        private void LoadProcessingConfiguration(String valuesAsXml)
        {
            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                XmlTextReader reader = null;
                try
                {
                    reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);
                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "ProcessingConfiguration" && reader.IsStartElement())
                        {
                            //Nothing to read as of now
                            reader.Read();
                        }
                        else if (reader.NodeType == XmlNodeType.Element && reader.Name == "ScheduleCriteria")
                        {
                            String xml = reader.ReadOuterXml();
                            if (!String.IsNullOrWhiteSpace(xml))
                            {
                                ScheduleCriteria criteria = new ScheduleCriteria(xml);
                                if (criteria != null)
                                {
                                    this.ScheduleCriteria = criteria;
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
