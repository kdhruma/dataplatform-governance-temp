using System;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;

namespace MDM.BusinessObjects.Integration
{
    using MDM.Interfaces;

    /// <summary>
    /// Contains configuration options for qualifying queue
    /// </summary>
    [DataContract]
    [KnownType(typeof(ScheduleCriteria))]
    public class QualificationConfiguration : IQualificationConfiguration
    {
        #region Fields

        /// <summary>
        /// Indicates time when the message needs to be taken up for qualification
        /// </summary>
        private ScheduleCriteria _scheduleCriteria = new ScheduleCriteria();

        #endregion Fields

        #region Constructor

        /// <summary>
        /// Parameterless Constructor
        /// </summary>
        public QualificationConfiguration()
            : base()
        {
        }

        /// <summary>
        /// Xml constructor
        /// </summary>
        /// <param name="valuesAsXml">Value in xml format</param>
        public QualificationConfiguration(String valuesAsXml)
        {
            LoadQualificationConfiguration(valuesAsXml);
        }

        #endregion Constructor

        #region Properties

        /// <summary>
        /// Indicates time when the message needs to be taken up for qualification
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
        /// Represents QualificationConfiguration in Xml format
        /// </summary>
        public String ToXml()
        {
            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);
            String xml = String.Empty;

            xmlWriter.WriteStartElement("QualificationConfiguration");

            if (this.ScheduleCriteria != null)
            {
                xmlWriter.WriteRaw(this.ScheduleCriteria.ToXml());
            }

            //QualificationConfiguration end
            xmlWriter.WriteEndElement();

            xmlWriter.Flush();

            //get the actual XML
            xml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return xml;
        }

        /// <summary>
        /// Clone the QualificationConfiguration object and return new instance with the same values.
        /// </summary>
        /// <returns>New cloned IQualificationConfiguration</returns>
        public IQualificationConfiguration Clone()
        {
            QualificationConfiguration clone = new QualificationConfiguration();
            clone.ScheduleCriteria = (ScheduleCriteria) this.ScheduleCriteria.Clone();

            return clone;
        }

        /// <summary>
        /// Get schedule criteria for qualification
        /// </summary>
        /// <returns></returns>
        public IScheduleCriteria GetScheduleCriteria()
        {
            return this.ScheduleCriteria;
        }

        /// <summary>
        /// Checks whether expected output is a subset of actual output or not.
        /// </summary>
        /// <param name="qualificationConfiguration">The expected qualification configuration which is compared against actual qualification configuration.</param>
        public Boolean IsSuperSetOf(QualificationConfiguration qualificationConfiguration)
        {
            if (qualificationConfiguration != null)
            {
                if (!this.ScheduleCriteria.IsSuperSetOf(qualificationConfiguration.ScheduleCriteria))
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
        /// Initialize QualificationConfiguration from xml.
        /// </summary>
        /// <param name="valuesAsXml">QualificationConfiguration in xml format</param>
        private void LoadQualificationConfiguration(String valuesAsXml)
        {
            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                XmlTextReader reader = null;
                try
                {
                    reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);
                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "QualificationConfiguration" && reader.IsStartElement())
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
