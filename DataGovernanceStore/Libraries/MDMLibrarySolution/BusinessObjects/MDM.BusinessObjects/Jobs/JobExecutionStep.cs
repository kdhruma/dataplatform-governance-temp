using System;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;

namespace MDM.BusinessObjects.Jobs
{
    using MDM.Core;

    /// <summary>
    /// Specifies the Execution step
    /// </summary>
    [DataContract]
    public class JobExecutionStep : MDMObject
    {
        #region Fields

        /// <summary>
        /// field denoting PK_DN_Step of Execution Step.
        /// </summary>
        [DataMember]
        private Int32 _pkDnStep = 0;

        /// <summary>
        /// field denoting execution status of Execution Step.
        /// </summary>
        [DataMember]
        private ExecutionStatus _executionStatus = new ExecutionStatus();

        #endregion

        #region Constructors

        /// <summary>
        /// Parameterless Constructor
        /// </summary>
        public JobExecutionStep()
            : base()
        {
        }

        /// <summary>
        /// Constructor with XML having values of object. Populate current object using XML
        /// </summary>
        /// <param name="valuesAsXml">XML having xml value</param>
        /// <example>
        ///     Sample XML:
        ///     <para>
        ///         &lt;JobExecutionStep
        ///             Id="101" 
        ///             Name="Taxonomy Category Structure Denorm" 
        ///             PK_DN_Step="1235" /&gt;
        /// </para>
        /// </example>
        public JobExecutionStep(String valuesAsXml)
        {
            LoadExecutionStep(valuesAsXml);
        }
        #endregion

        #region Properties

        /// <summary>
        ///  Property denoting PK_DN_Step
        /// </summary>
        [DataMember]
        public Int32 PK_DN_Step
        {
            get
            {
                return this._pkDnStep;
            }
            set
            {
                this._pkDnStep = value;
            }
        }

        /// <summary>
        ///  Property denoting execution status
        /// </summary>
        [DataMember]
        public ExecutionStatus ExecutionStatus
        {
            get
            {
                return this._executionStatus;
            }
            set
            {
                this._executionStatus = value;
            }
        }

        #endregion

        #region Methods

        #region Load Methods

        /// <summary>
        /// Load execution step object from XML.
        /// </summary>
        /// <param name="valuesAsXml">XML having xml value</param>
        /// <example>
        ///     Sample XML:
        ///     <para>
        ///     <![CDATA[
        ///         <JobExecutionStep
        ///             Id="101" 
        ///             Name="Taxonomy Category Structure Denorm" 
        ///         </JobExecutionStep>
        ///     ]]>    
        ///     </para>
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
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "JobExecutionStep")
                        {
                            #region Read JobExecutionStep Properties

                            if (reader.HasAttributes)
                            {
                                if (reader.MoveToAttribute("Id"))
                                {
                                    this.Id = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), 0);
                                }

                                if (reader.MoveToAttribute("Name"))
                                {
                                    this.Name = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("PK_DN_Step"))
                                {
                                    this.PK_DN_Step = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), 0);
                                }
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

            //JobExecutionStep node start
            xmlWriter.WriteStartElement("JobExecutionStep");

            #region Write JobExecutionStep Properties

            xmlWriter.WriteAttributeString("Id", this.Id.ToString());
            xmlWriter.WriteAttributeString("Name", this.Name);
            xmlWriter.WriteAttributeString("PK_DN_Step", this.PK_DN_Step.ToString());

            #endregion

            //JobExecutionStep node end
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

                //JobExecutionStep node start
                xmlWriter.WriteStartElement("JobExecutionStep");

                if (serialization == ObjectSerialization.ProcessingOnly)
                {
                    #region Write JobExecutionStep Properties

                    xmlWriter.WriteAttributeString("Id", this.Id.ToString());
                    xmlWriter.WriteAttributeString("Name", this.Name);
                    xmlWriter.WriteAttributeString("PK_DN_Step", this.PK_DN_Step.ToString());

                    #endregion
                }
                else if (serialization == ObjectSerialization.UIRender)
                {
                    #region Write JobExecutionStep Properties

                    xmlWriter.WriteAttributeString("Id", this.Id.ToString());
                    xmlWriter.WriteAttributeString("Name", this.Name);
                    xmlWriter.WriteAttributeString("PK_DN_Step", this.PK_DN_Step.ToString());

                    #endregion
                }

                //JobExecutionStep node end
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

        #region Private Methods

        #endregion

        #endregion
    }
}
