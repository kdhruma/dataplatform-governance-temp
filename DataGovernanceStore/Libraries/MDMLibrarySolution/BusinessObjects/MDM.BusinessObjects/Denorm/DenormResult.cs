using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.IO;
using System.Xml;

namespace MDM.BusinessObjects.Denorm
{
    using MDM.Core;

    /// <summary>
    /// Specifies the Denorm Result
    /// </summary>
    [DataContract]
    public class DenormResult : MDMObject
    {
        #region Fields

        /// <summary>
        /// field denoting the Job Id of Result
        /// </summary>
        private Int32 _jobId = 0;

        /// <summary>
        /// field denoting the Retry Count of Result
        /// </summary>
        private Int32 _retryCount = 0;

        /// <summary>
        /// field denoting the ErrorTime of the result.
        /// </summary>
        private String _errorTime = String.Empty;

        /// <summary>
        /// field denoting the status of the result.
        /// </summary>
        private String _status = String.Empty;

        /// <summary>
        /// field denoting the entityOperationResult of per step.
        /// </summary>
        private Dictionary<Int32, EntityOperationResultCollection> _stepResult = new Dictionary<Int32, EntityOperationResultCollection>();

        /// <summary>
        /// field denoting the Operation Result of the denorm result.
        /// </summary>
        private OperationResult _operationResult = new OperationResult();

        #endregion

        #region Constructor

        /// <summary>
        /// Parameterless Constructor
        /// </summary>
        public DenormResult()
            : base()
        {
        }

        /// <summary>
        /// Constructor with XML
        /// </summary>
        /// <param name="valuesAsxml">XML having xml value</param>
        public DenormResult(String valuesAsxml)
        {
            LoadDenormResult(valuesAsxml);
        }

        #endregion

        #region Properties

        /// <summary>
        ///  Property denoting the Job Id
        /// </summary>
        [DataMember]
        public Int32 JobId
        {
            get
            {
                return this._jobId;
            }
            set
            {
                this._jobId = value;
            }
        }

        /// <summary>
        ///  Property denoting the Retry Count
        /// </summary>
        [DataMember]
        public Int32 RetryCount
        {
            get
            {
                return this._retryCount;
            }
            set
            {
                this._retryCount = value;
            }
        }

        /// <summary>
        ///  Property denoting the error time of result
        /// </summary>
        [DataMember]
        public String ErrorTime
        {
            get
            {
                return this._errorTime;
            }
            set
            {
                this._errorTime = value;
            }
        }

        /// <summary>
        ///  Property denoting the status of result
        /// </summary>
        [DataMember]
        public String Status
        {
            get
            {
                return this._status;
            }
            set
            {
                this._status = value;
            }
        }

        /// <summary>
        ///  Property denoting the EntityOperationResultCollection of per step.
        /// </summary>
        [DataMember]
        public Dictionary<Int32,EntityOperationResultCollection> StepResult
        {
            get
            {
                return this._stepResult;
            }
            set
            {
                this._stepResult = value;
            }
        }

        /// <summary>
        ///  Property denoting the Operation Result
        /// </summary>
        [DataMember]
        public OperationResult OperationResult
        {
            get
            {
                return this._operationResult;
            }
            set
            {
                this._operationResult = value;
            }
        }

        #endregion

        #region Methods

        #region Load Methods

        ///<summary>
        /// Load Denorm result object from XML.
        /// </summary>
        /// <param name="valuesAsXml">XML having xml value</param>
        /// <example>
        ///     <para>
        ///     <![CDATA[
        ///         <DenormResult
        ///             Id="101" 
        ///             RetryCount="1" 
        ///             ErrorTime="Apr 12 2012 11:50:00PM"
        ///             Status="New"
        ///         </ExecutionStep>
        ///     ]]>    
        ///     </para>
        /// </example>
        public void LoadDenormResult(String valuesAsXml)
        {
            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                XmlTextReader reader = null;
                try
                {
                    reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "DenormResult")
                        {
                            #region Read Denorm Result Properties

                            if (reader.HasAttributes)
                            {
                                if (reader.MoveToAttribute("Id"))
                                    this.Id = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), 0);

                                if (reader.MoveToAttribute("JobId"))
                                    this.JobId = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), 0);

                                if (reader.MoveToAttribute("RetryCount"))
                                    this.RetryCount = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), 0);

                                if (reader.MoveToAttribute("ErrorTime"))
                                    this.ErrorTime = reader.ReadContentAsString();

                                if (reader.MoveToAttribute("Status"))
                                    this.Status = reader.ReadContentAsString();
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
        /// Get Xml representation of Denorm Result
        /// </summary>
        /// <returns>Xml representation of object</returns>
        public override String ToXml()
        {
            String denormXml = String.Empty;
            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            //jobXml node start
            xmlWriter.WriteStartElement("DenormResult");

            #region Write Properties

            xmlWriter.WriteAttributeString("Id", this.Id.ToString());
            xmlWriter.WriteAttributeString("JobId", this.JobId.ToString());
            xmlWriter.WriteAttributeString("RetryCount", this.RetryCount.ToString());
            xmlWriter.WriteAttributeString("ErrorTime", this.ErrorTime);
            xmlWriter.WriteAttributeString("Status", this.Status);
            
            #endregion

            //Denorm Result node end
            xmlWriter.WriteEndElement();

            xmlWriter.Flush();

            //get the actual XML
            denormXml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return denormXml;
        }

        /// <summary>
        /// Get Xml representation of Denorm Result
        /// </summary>
        /// <returns>Xml representation of object</returns>
        public override String ToXml(ObjectSerialization serialization)
        {
            // No serialization implemented for now...
            return this.ToXml();
        }

        #endregion

        #endregion
    }
}
