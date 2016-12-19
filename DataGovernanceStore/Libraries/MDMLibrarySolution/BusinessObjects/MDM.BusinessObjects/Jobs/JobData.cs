using System;
using System.Xml;
using System.IO;
using System.Runtime.Serialization;

namespace MDM.BusinessObjects.Jobs
{
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Specifies the JobData
    /// </summary>
    [DataContract]
    public class JobData : ObjectBase, IJobData
    {
        #region Fields

        /// <summary>
        /// field denoting id of the Job profile.
        /// </summary>
        private Int32 _profileId = 0;

        /// <summary>
        /// field denoting execution status for the job
        /// </summary>
        private ExecutionStatus _executionStatus = new ExecutionStatus();

        /// <summary>
        /// field denoting job parameters
        /// </summary>
        [DataMember]
        private JobParameterCollection _jobParameters = new JobParameterCollection();

        /// <summary>
        /// field denoting job execution step collection
        /// </summary>
        [DataMember]
        private JobExecutionStepCollection _jobexecutionStep = new JobExecutionStepCollection();

        /// <summary>
        /// field denoting operation result for the job data..
        /// </summary>
        private OperationResult _operationResult = new OperationResult();

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        public JobData()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="valuesAsXml">XML with JobData</param>
        public JobData(String valuesAsXml)
        {
            LoadJobData(valuesAsXml);
        }

        #endregion

        #region Properties

        /// <summary>
        ///  Property denoting the profile id
        /// </summary>
        [DataMember]
        public Int32 ProfileId
        {
            get
            {
                return this._profileId;
            }
            set
            {
                this._profileId = value;
            }
        }

        /// <summary>
        ///  Property denoting execution status for the job
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

        /// <summary>
        ///  Property denoting job parameters
        /// </summary>
        [DataMember]
        public JobParameterCollection JobParameters
        {
            get
            {
                return this._jobParameters;
            }
            set
            {
                this._jobParameters = value;
            }
        }

        /// <summary>
        ///  Property denoting job execution step
        /// </summary>
        [DataMember]
        public JobExecutionStepCollection JobExecutionStep
        {
            get
            {
                return this._jobexecutionStep;
            }
            set
            {
                this._jobexecutionStep = value;
            }
        }

        /// <summary>
        ///  Property denoting operation result for job
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
        /// Load JobData object from XML.
        /// </summary>
        /// <param name="valuesAsXml">XML having xml value</param>
        /// <example>
        ///     Sample XML:
        /// </example>
        public void LoadJobData(String valuesAsXml)
        {
            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                XmlTextReader reader = null;
                try
                {
                    reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "JobData")
                        {
                            #region Read JobData Properties

                            if (reader.HasAttributes)
                            {
                                if (reader.MoveToAttribute("ProfileId"))
                                {
                                    this.ProfileId = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), 0);
                                }
                            }

                            #endregion
                        }
                        else if (reader.NodeType == XmlNodeType.Element && reader.Name == "ExecutionStatus")
                        {
                            #region Read ExecutionStatus

                            String executionStatusXml = reader.ReadOuterXml();
                            if (!String.IsNullOrEmpty(executionStatusXml))
                            {
                                ExecutionStatus executionStatus = new ExecutionStatus(executionStatusXml);
                                this.ExecutionStatus = executionStatus;
                            }

                            #endregion
                        }
                        else if (reader.NodeType == XmlNodeType.Element && reader.Name == "JobParameters")
                        {
                            #region Read JobParametersCollection

                            String jobParametersXml = reader.ReadOuterXml();
                            if (!String.IsNullOrEmpty(jobParametersXml))
                            {
                                JobParameterCollection jobParameters = new JobParameterCollection(jobParametersXml);
                                this.JobParameters = jobParameters;
                            }

                            #endregion
                        }
                        else if (reader.NodeType == XmlNodeType.Element && reader.Name == "OperationResult")
                        {
                            #region Read OperationResult

                            String operationResultXml = reader.ReadOuterXml();
                            if (!String.IsNullOrEmpty(operationResultXml))
                            {
                                OperationResult operationResult = new OperationResult(operationResultXml);
                                this.OperationResult = operationResult;
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
        /// Get Xml representation of JobData
        /// </summary>
        /// <returns>Xml representation of object</returns>
        public String ToXml()
        {
            String jobDataXml = String.Empty;
            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            //Mapping node start
            xmlWriter.WriteStartElement("JobData");

            #region Write JobData Properties for Full JobData Xml

            xmlWriter.WriteAttributeString("ProfileId", this.ProfileId.ToString());
          
            #endregion

            #region Write ExecutionStatus

            if (this.ExecutionStatus != null)
            {
                xmlWriter.WriteRaw(this.ExecutionStatus.ToXml());
            }

            #endregion

            #region Write JobParameterCollection

            if (this.JobParameters != null)
            {
                xmlWriter.WriteRaw(this.JobParameters.ToXml());
            }

            #endregion

            #region Write JobExecutionStepCollection

            if (this.JobExecutionStep != null)
            {
                xmlWriter.WriteRaw(this.JobExecutionStep.ToXml());
            }

            #endregion

            #region Write OperationResult

            if (this.OperationResult != null)
            {
                xmlWriter.WriteRaw(this.OperationResult.ToXml());
            }

            #endregion

            //Mapping node end
            xmlWriter.WriteEndElement();

            xmlWriter.Flush();

            //get the actual XML
            jobDataXml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return jobDataXml;
        }

        /// <summary>
        /// Get Xml representation of JobData
        /// </summary>
        /// <returns>Xml representation of object</returns>
        public String ToXml(ObjectSerialization serialization)
        {
            String jobDataXml = String.Empty;

            if (serialization == ObjectSerialization.Full)
            {
                jobDataXml = this.ToXml();
            }
            else
            {
                StringWriter sw = new StringWriter();
                XmlTextWriter xmlWriter = new XmlTextWriter(sw);

                //JobData node start
                xmlWriter.WriteStartElement("JobData");

                if (serialization == ObjectSerialization.ProcessingOnly)
                {                    
                    // TODO : Need to decide which all properties are needed for processing Xml.
                    // currently returning all properties.

                    #region Write JobData Properties for ProcessingOnly JobData Xml

                    xmlWriter.WriteAttributeString("ProfileId", this.ProfileId.ToString());

                    #endregion
                    
                }
                else if (serialization == ObjectSerialization.UIRender)
                {                    
                    // TODO : Need to decide which all properties are needed for Rendering Xml.
                    // currently returning all properties.

                    #region Write JobData Properties for Rendering JobData Xml

                    xmlWriter.WriteAttributeString("ProfileId", this.ProfileId.ToString());
                 
                    #endregion

                }

                #region Write ExecutionStatus

                if (this.ExecutionStatus != null)
                {
                    xmlWriter.WriteRaw(this.ExecutionStatus.ToXml(serialization));
                }

                #endregion

                #region Write JobParameterCollection

                if (this.JobParameters != null)
                {
                    xmlWriter.WriteRaw(this.JobParameters.ToXml(serialization));
                }

                #endregion

                #region Write JobExecutionStepCollection

                if (this.JobExecutionStep != null)
                {
                    xmlWriter.WriteRaw(this.JobExecutionStep.ToXml(serialization));
                }

                #endregion

                #region Write OperationResult

                if (this.OperationResult != null)
                {
                    xmlWriter.WriteRaw(this.OperationResult.ToXml(serialization));
                }

                #endregion

                //JobData node end
                xmlWriter.WriteEndElement();

                xmlWriter.Flush();

                //get the actual XML
                jobDataXml = sw.ToString();

                xmlWriter.Close();
                sw.Close();
            }

            return jobDataXml;
        }

        #endregion

        #region IJobData Methods

        /// <summary>
        /// Gets execution status
        /// </summary>
        /// <returns>Execution Status object</returns>
        /// <exception cref="NullReferenceException">Thrown when Execution Status is NULL</exception>
        public IExecutionStatus GetExecutionStatus()
        {
            if (this.ExecutionStatus == null)
            {
                throw new NullReferenceException("Execution Status is null");
            }

            return (IExecutionStatus)this.ExecutionStatus;
        }

        /// <summary>
        /// Sets execution status
        /// </summary>
        /// <param name="iExecutionStatus">The execution status object</param>
        /// <returns>The result of the operation</returns>
        public Boolean SetExecutionStatus(IExecutionStatus iExecutionStatus)
        {
            if (iExecutionStatus == null)
            {
                throw new ArgumentNullException("iExecutionStatus");
            }

            this.ExecutionStatus = (ExecutionStatus)iExecutionStatus;

            return true;
        }

        /// <summary>
        /// Gets job parameters
        /// </summary>
        /// <returns>Job Parameters</returns>
        /// <exception cref="NullReferenceException">Thrown when JobParameters field is NULL</exception>
        public IJobParameterCollection GetJobParameters()
        {
            if (this.JobParameters == null)
            {
                throw new NullReferenceException("Job Parameters are not available.");
            }

            return (IJobParameterCollection)this.JobParameters;
        }

        /// <summary>
        /// Sets job parameters
        /// </summary>
        /// <param name="iJobParameterCollection">Job parameters collection</param>
        /// <returns>The result of the operation</returns>
        public Boolean SetJobParameters(IJobParameterCollection iJobParameterCollection)
        {
            if (iJobParameterCollection == null)
            {
                throw new ArgumentNullException("iJobParameterCollection");
            }

            this.JobParameters = (JobParameterCollection)iJobParameterCollection;

            return true;
        }

        /// <summary>
        /// Gets job operation result
        /// </summary>
        /// <returns>Job operation result</returns>
        /// <exception cref="NullReferenceException">Thrown when OperationResult field is NULL</exception>
        public IOperationResult GetJobOperationResult()
        {
            if (this.OperationResult == null)
            {
                throw new NullReferenceException("Operation Results are not available.");
            }

            return (IOperationResult)this.OperationResult;
        }

        #endregion

        #region Private Methods

        #endregion

        #endregion
    }
}
