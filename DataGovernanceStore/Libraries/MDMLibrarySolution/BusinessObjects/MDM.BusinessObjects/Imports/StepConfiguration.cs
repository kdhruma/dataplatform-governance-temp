using System;
using System.Text;
using System.IO;
using System.Xml;
using System.Runtime.Serialization;

namespace MDM.BusinessObjects.Imports
{
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Import execution step configuration
    /// </summary>
    [DataContract]
    public class StepConfiguration : ObjectBase
    {
        #region Fields

        #endregion

        #region Constructors

        /// <summary>
        /// Parameterless Constructor
        /// </summary>
        public StepConfiguration()
            : base()
        {
        }

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="valuesAsXml">Values which needs to be set when object is initialized in XMl format</param>
        public StepConfiguration(String valuesAsXml)
        {
            LoadStepConfiguration(valuesAsXml);
        }
        #endregion

        #region Properties
        
        /// <summary>
        /// Property defining the type of the MDM object
        /// </summary>
        public new String ObjectType
        {
            get
            {
                return "StepConfiguration";
            }
        }

        #endregion

        #region Methods

        #region Load Methods

        ///<summary>
        /// Load StepConfiguration object from XML.
        /// </summary>
        /// <param name="valuesAsXml">XML having xml value</param>
        /// <example>
        ///     Sample XML:
        /// </example>
        public void LoadStepConfiguration(String valuesAsXml)
        {
            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            { 
                XmlTextReader reader = null;
                try
                {
                    reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                    while (!reader.EOF)
                    {
                        //Keep on reading the xml until we reach expected node.
                        reader.Read();                        
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

        #endregion

        #region ToXml methods

        /// <summary>
        /// Get Xml representation of StepConfiguration
        /// </summary>
        /// <returns>Xml representation of object</returns>
        public String ToXml()
        {
            String stepConfigurationXml = String.Empty;
            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            //StepConfiguration node start
            xmlWriter.WriteStartElement("StepConfiguration");

            //StepConfiguration node end
            xmlWriter.WriteEndElement();

            xmlWriter.Flush();

            //get the actual XML
            stepConfigurationXml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return stepConfigurationXml;
        }

        /// <summary>
        /// Get Xml representation of StepConfiguration
        /// </summary>
        /// <returns>Xml representation of object</returns>
        public String ToXml(ObjectSerialization serialization)
        {
            String stepConfigurationXml = String.Empty;

            if (serialization == ObjectSerialization.Full)
            {
                stepConfigurationXml = this.ToXml();
            }
            else
            {
                StringWriter sw = new StringWriter();
                XmlTextWriter xmlWriter = new XmlTextWriter(sw);

                //StepConfiguration node start
                xmlWriter.WriteStartElement("StepConfiguration");

                //StepConfiguration node end
                xmlWriter.WriteEndElement();

                xmlWriter.Flush();

                //get the actual XML
                stepConfigurationXml = sw.ToString();

                xmlWriter.Close();
                sw.Close();
            }
            return stepConfigurationXml;
        }

        #endregion

        #region Private Methods

        #endregion
        
        #endregion
    }
}
