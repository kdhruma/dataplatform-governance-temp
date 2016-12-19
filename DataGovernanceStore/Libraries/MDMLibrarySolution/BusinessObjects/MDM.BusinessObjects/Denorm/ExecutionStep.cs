using System;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;

namespace MDM.BusinessObjects.Denorm
{
    using MDM.Core;

    /// <summary>
    /// Specifies the Execution step
    /// </summary>
    [DataContract]
    public class ExecutionStep : MDMObject
    {
        #region Fields

        /// <summary>
        /// field denoting description of Execution Step.
        /// </summary>
        [DataMember]
        private String _description = String.Empty;

        #endregion

        #region Constructors

        /// <summary>
        /// Parameterless Constructor
        /// </summary>
        public ExecutionStep()
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
        ///         &lt;ExecutionStep
        ///             Id="101" 
        ///             Name="Taxonomy Category Structure Denorm" 
        ///             Description="Denorm Delta (DN_Taxonomy)"/&gt;
        /// </para>
        /// </example>
        public ExecutionStep(String valuesAsXml)
        {
            LoadExecutionStep(valuesAsXml);
        }
        #endregion

        #region Properties

        /// <summary>
        ///  Property denoting the description of the execution step
        /// </summary>
        [DataMember]
        public String Description
        {
            get
            {
                return this._description;
            }
            set
            {
                this._description = value;
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
        ///         <ExecutionStep
        ///             Id="101" 
        ///             Name="Taxonomy Category Structure Denorm" 
        ///             Description="Denorm Delta (DN_Taxonomy)"
        ///         </ExecutionStep>
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
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "ExecutionStep")
                        {
                            #region Read ExecutionStep Properties

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

                                if (reader.MoveToAttribute("Description"))
                                {
                                    this.Description = reader.ReadContentAsString();
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
        /// Get Xml representation of Denorm Profile
        /// </summary>
        /// <returns>Xml representation of object</returns>
        public override String ToXml()
        {
            String executionStepXml = String.Empty;
            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            //ExecutionStep node start
            xmlWriter.WriteStartElement("ExecutionStep");

            #region Write ExecutionStep Properties

            xmlWriter.WriteAttributeString("Id", this.Id.ToString());
            xmlWriter.WriteAttributeString("Name", this.Name);
            xmlWriter.WriteAttributeString("Description", this.Name);

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
                    #region Write ExecutionStep Properties

                    xmlWriter.WriteAttributeString("Id", this.Id.ToString());
                    xmlWriter.WriteAttributeString("Name", this.Name);
                    xmlWriter.WriteAttributeString("Description", this.Description);

                    #endregion
                }
                else if (serialization == ObjectSerialization.UIRender)
                {
                    #region Write ExecutionStep Properties

                    xmlWriter.WriteAttributeString("Id", this.Id.ToString());
                    xmlWriter.WriteAttributeString("Name", this.Name);
                    xmlWriter.WriteAttributeString("Description", this.Description);

                    #endregion
                }

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

        #region Private Methods

        #endregion

        #endregion
    }
}
