using System;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;

namespace MDM.BusinessObjects.Diagnostics
{
    using MDM.Core;

    /// <summary>
    /// Execution step
    /// </summary>
    [DataContract]
    public class DiagnosticReportExecutionStep : MDMObject
    {
        #region Fields

        /// <summary>
        /// Execution step type
        /// </summary>
        private ExecutionStepType _stepType = ExecutionStepType.Core;

        /// <summary>
        /// Field denoting name of an assembly file(In complete path)
        /// </summary>
        private String _assemblyFileName = String.Empty;

        /// <summary>
        /// Field denoting name of an full class name
        /// </summary>
        private String _classFullName = String.Empty;

        /// <summary>
        /// Field indicates if we need to abort execution if step fails
        /// </summary>
        private Boolean _abortOnError = true;

        /// <summary>
        /// Field denoting whether this execution step is enabled or not
        /// </summary>
        private Boolean _enabled = true;

        /// <summary>
        /// Step configuration
        /// </summary>
        private DiagnosticReportExecutionStepConfiguration _stepConfiguration = new DiagnosticReportExecutionStepConfiguration();

        #endregion

        #region Constructors

        /// <summary>
        /// Parameterless Constructor
        /// </summary>
        public DiagnosticReportExecutionStep()
            : base()
        {
        }

        /// <summary>
        /// Constructor with XML having values of object. 
        /// </summary>
        /// <param name="valuesAsXml">Indicates XML having xml value</param>
        public DiagnosticReportExecutionStep(String valuesAsXml)
        {
            LoadExecutionStep(valuesAsXml);
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
                return "ExecutionStep";
            }
        }

        /// <summary>
        /// Property for Step Type
        /// </summary>
        [DataMember]
        public ExecutionStepType StepType
        {
            get { return _stepType; }
            set { _stepType = value; }
        }

        /// <summary>
        /// Property defining assembly name for an execution step(Name should be in full form including complete path)
        /// </summary>
        [DataMember]
        public String AssemblyFileName
        {
            get
            {
                return _assemblyFileName;
            }
            set
            {
                _assemblyFileName = value;
            }
        }

        /// <summary>
        /// Property defining full class name
        /// </summary>
        [DataMember]
        public String ClassFullName
        {
            get
            {
                return _classFullName;
            }
            set
            {
                _classFullName = value;
            }
        }

        /// <summary>
        /// Property defining if we need to abort execution if step fails
        /// </summary>
        [DataMember]
        public Boolean AbortOnError
        {
            get
            {
                return _abortOnError;
            }
            set
            {
                _abortOnError = value;
            }
        }

        /// <summary>
        /// Property defining whether this execution step is enabled or not
        /// </summary>
        [DataMember]
        public Boolean Enabled
        {
            get
            {
                return _enabled;
            }
            set
            {
                _enabled = value;
            }
        }

        /// <summary>
        /// Property defining whether this execution step is enabled or not
        /// </summary>
        [DataMember]
        public DiagnosticReportExecutionStepConfiguration StepConfiguration
        {
            get
            {
                return _stepConfiguration;
            }
            set
            {
                _stepConfiguration = value;
            }
        }

        #endregion

        #region Methods

        #region Load Methods

        ///<summary>
        /// Load ExecutionStep object from XML.
        /// </summary>
        /// <param name="valuesAsXml">XML having xml value</param>
        /// <example>
        ///     Sample XML:
        /// </example>
        public void LoadExecutionStep(String valuesAsXml)
        {
            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                XmlTextReader reader = null;
                try
                {
                    reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "ExecutionStep")
                        {
                            #region Read ExecutionStep Properties

                            if (reader.HasAttributes)
                            {
                                if (reader.MoveToAttribute("Name"))
                                {
                                    this.Name = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("StepType"))
                                {
                                    ExecutionStepType executionStepType = ExecutionStepType.UnKnown;
                                    Enum.TryParse(reader.ReadContentAsString(), true, out executionStepType);
                                    this.StepType = executionStepType;
                                }

                                if (reader.MoveToAttribute("AssemblyFileName"))
                                {
                                    this.AssemblyFileName = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("ClassFullName"))
                                {
                                    this.ClassFullName = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("AbortOnError"))
                                {
                                    this.AbortOnError = reader.ReadContentAsBoolean();
                                }

                                if (reader.MoveToAttribute("Enabled"))
                                {
                                    this.Enabled = reader.ReadContentAsBoolean();
                                }
                            }

                            #endregion
                        }
                        else if (reader.NodeType == XmlNodeType.Element && reader.Name == "StepConfiguration")
                        {
                            #region Read StepConfiguration

                            String StepConfigurationXml = reader.ReadOuterXml();
                            if (!String.IsNullOrEmpty(StepConfigurationXml))
                            {
                                DiagnosticReportExecutionStepConfiguration stepConfiguration = new DiagnosticReportExecutionStepConfiguration(StepConfigurationXml);
                                this.StepConfiguration = stepConfiguration;
                            }

                            #endregion
                        }
                        else
                        {
                            //Keep on reading the xml until we reach expected node.
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

        #endregion

        #region ToXml methods

        /// <summary>
        /// Get Xml representation of Execution Step
        /// </summary>
        /// <returns>Xml representation of object</returns>
        public override String ToXml()
        {
            String executionStepXml = String.Empty;
            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            //ExecutionStep node start
            xmlWriter.WriteStartElement("ExecutionStep");

            #region Write ExecutionStep Properties for Full ExecutionStep Xml

            xmlWriter.WriteAttributeString("Name", this.Name.ToString());
            xmlWriter.WriteAttributeString("StepType", this.StepType.ToString());
            xmlWriter.WriteAttributeString("AssemblyFileName", this.AssemblyFileName);
            xmlWriter.WriteAttributeString("ClassFullName", this.ClassFullName);
            xmlWriter.WriteAttributeString("AbortOnError", this.AbortOnError.ToString().ToLower());
            xmlWriter.WriteAttributeString("Enabled", this.Enabled.ToString().ToLower());

            #endregion

            #region Write StepConfiguration

            if (this.StepConfiguration != null)
            {
                xmlWriter.WriteRaw(this.StepConfiguration.ToXml());
            }

            #endregion

            //ExecutionStep node end
            xmlWriter.WriteEndElement();

            xmlWriter.Flush();

            //get the actual XML
            executionStepXml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return executionStepXml;
        }

        /// <summary>
        /// Get Xml representation of Execution Step
        /// </summary>
        /// <returns>Xml representation of object</returns>
        public override String ToXml(ObjectSerialization serialization)
        {
            String executionStepXml = String.Empty;

            if (serialization == ObjectSerialization.Full)
            {
                executionStepXml = this.ToXml();
            }
            else
            {
                StringWriter sw = new StringWriter();
                XmlTextWriter xmlWriter = new XmlTextWriter(sw);

                //ExecutionStep node start
                xmlWriter.WriteStartElement("ExecutionStep");

                if (serialization == ObjectSerialization.ProcessingOnly)
                {
                    #region Write ExecutionStep Properties for Processing Only

                    // TODO : Need to decide which all properties are needed for processing Xml.
                    // currently returning all properties.

                    xmlWriter.WriteAttributeString("Name", this.Name.ToString());
                    xmlWriter.WriteAttributeString("StepType", this.StepType.ToString());
                    xmlWriter.WriteAttributeString("AssemblyFileName", this.AssemblyFileName);
                    xmlWriter.WriteAttributeString("ClassFullName", this.ClassFullName);
                    xmlWriter.WriteAttributeString("AbortOnError", this.AbortOnError.ToString().ToLower());
                    xmlWriter.WriteAttributeString("Enabled", this.Enabled.ToString().ToLower());

                    #endregion
                }
                else if (serialization == ObjectSerialization.UIRender)
                {
                    #region Write ExecutionStep Properties for Rendering

                    // TODO : Need to decide which all properties are needed for Rendering Xml.
                    // currently returning all properties.

                    xmlWriter.WriteAttributeString("Name", this.Name.ToString());
                    xmlWriter.WriteAttributeString("StepType", this.StepType.ToString());
                    xmlWriter.WriteAttributeString("AssemblyFileName", this.AssemblyFileName);
                    xmlWriter.WriteAttributeString("ClassFullName", this.ClassFullName);
                    xmlWriter.WriteAttributeString("AbortOnError", this.AbortOnError.ToString().ToLower());
                    xmlWriter.WriteAttributeString("Enabled", this.Enabled.ToString().ToLower());

                    #endregion
                }

                #region Write StepConfiguration

                if (this.StepConfiguration != null)
                {
                    xmlWriter.WriteRaw(this.StepConfiguration.ToXml(serialization));
                }

                #endregion Write StepConfiguration

                //ExecutionStep node end
                xmlWriter.WriteEndElement();

                xmlWriter.Flush();

                //get the actual XML
                executionStepXml = sw.ToString();

                xmlWriter.Close();
                sw.Close();
            }

            return executionStepXml;
        }

        #endregion

        #endregion
    }
}
