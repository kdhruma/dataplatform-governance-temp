using System;
using System.Xml;
using System.IO;
using System.Runtime.Serialization;

namespace MDM.BusinessObjects.Jobs
{
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Specifies the ExecutionStatus
    /// </summary>
    [DataContract]
    public class ExecutionStatus : ObjectBase, IExecutionStatus
    {
        #region Fields

        /// <summary>
        /// field denoting start time of job
        /// </summary>
        private String _startTime = String.Empty;

        /// <summary>
        /// field denoting end time for the job
        /// </summary>
        private String _endTime = String.Empty;

        /// <summary>
        /// field denoting the estimated milliseconds to complete job
        /// </summary>
        private Double _estimatedMilliSeconds = 0;

        /// <summary>
        /// field denoting the remaining milliseconds to complete job
        /// </summary>
        private Double _remainingMilliSeconds = 0;

        /// <summary>
        /// field denoting the total milliseconds to complete job
        /// </summary>
        private Double _totalMilliSeconds = 0;

        /// <summary>
        /// field denoting the total elements to be process by job
        /// </summary>
        private Int64 _totalElementsToProcess = 0;

        /// <summary>
        /// field denoting total elements processed by job
        /// </summary>
        private Int64 _totalElementsProcessed = 0;

        /// <summary>
        /// field denoting total elements processed by job
        /// </summary>
        private Int64 _totalElementsSucceed = 0;

        /// <summary>
        /// field denoting total elements processed by job
        /// </summary>
        private Int64 _totalElementsPartiallySucceed = 0;

        /// <summary>
        /// field denoting total elements processed by job
        /// </summary>
        private Int64 _totalElementsFailed = 0;

        /// <summary>
        /// field denoting total elements warned by job
        /// </summary>
        private Int64 _totalElementsWarned = 0;

        /// <summary>
        /// Field denoting total elements not changed by job
        /// </summary>
        private Int64 _totalElementsUnChanged = 0;

        /// <summary>
        /// field denoting overall progress
        /// </summary>
        private Int32 _overAllProgress = 0;

        /// <summary>
        /// field denoting total steps of Execution Status
        /// </summary>
        private Int32 _totalSteps = 0;

        /// <summary>
        /// field denoting current execution status of job in summarized form
        /// </summary>
        private String  _currentStatusMessage = String.Empty;

        #endregion

        #region Constructor

        /// <summary>
        /// 
        /// </summary>
        public ExecutionStatus()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="valuesAsXml"></param>
        public ExecutionStatus(String valuesAsXml)
        {
            LoadExecutionStatus(valuesAsXml);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Property denoting start time of job
        /// </summary>
        [DataMember]
        public String StartTime
        {
            get
            {
                return this._startTime;
            }
            set
            {
                this._startTime = value;
            }
        }

        /// <summary>
        /// Property denoting end time of job
        /// </summary>
        [DataMember]
        public String EndTime
        {
            get
            {
                return this._endTime;
            }
            set
            {
                this._endTime  = value;
            }
        }

        /// <summary>
        /// Property denoting the estimated milliseconds to complete job
        /// </summary>
        [DataMember]
        public Double EstimatedMilliSeconds
        {
            get
            {
                return this._estimatedMilliSeconds;
            }
            set
            {
                this._estimatedMilliSeconds = value;
            }
        }

        /// <summary>
        /// Property denoting the remaining milliseconds to complete job
        /// </summary>
        [DataMember]
        public Double RemainingMilliSeconds
        {
            get
            {
                return this._remainingMilliSeconds;
            }
            set
            {
                this._remainingMilliSeconds = value;
            }
        }

        /// <summary>
        /// Property denoting the total milliseconds to complete job
        /// </summary>
        [DataMember]
        public Double TotalMilliSeconds
        {
            get
            {
                return this._totalMilliSeconds;
            }
            set
            {
                this._totalMilliSeconds = value;
            }
        }

        /// <summary>
        /// Property denoting total elements to be process by Job
        /// </summary>
        [DataMember]
        public Int64 TotalElementsToProcess
        {
            get
            {
                return this._totalElementsToProcess;
            }
            set
            {
                this._totalElementsToProcess = value;
            }
        }

        /// <summary>
        /// Property denoting total elements processed by Job
        /// </summary>
        [DataMember]
        public Int64 TotalElementsProcessed
        {
            get
            {
                return this._totalElementsProcessed;
            }
            set
            {
                this._totalElementsProcessed = value;
            }
        }

        /// <summary>
        /// Property denoting total elements succeeded
        /// </summary>
        [DataMember]
        public Int64 TotalElementsSucceed
        {
            get
            {
                return this._totalElementsSucceed;
            }
            set
            {
                this._totalElementsSucceed = value;
            }
        }

        /// <summary>
        /// Property denoting total elements partially succeeded
        /// </summary>
        [DataMember]
        public Int64 TotalElementsPartiallySucceed
        {
            get
            {
                return this._totalElementsPartiallySucceed;
            }
            set
            {
                this._totalElementsPartiallySucceed = value;
            }
        }

        /// <summary>
        /// Property denoting total elements failed
        /// </summary>
        [DataMember]
        public Int64 TotalElementsFailed
        {
            get
            {
                return this._totalElementsFailed;
            }
            set
            {
                this._totalElementsFailed = value;
            }
        }

        /// <summary>
        /// Property denoting total elements not changed
        /// </summary>
        [DataMember]
        public Int64 TotalElementsUnChanged
        {
            get
            {
                return this._totalElementsUnChanged;
            }
            set
            {
                this._totalElementsUnChanged = value;
            }
        }

        /// <summary>
        /// Property denoting total elements Warned
        /// </summary>
        [DataMember]
        public Int64 TotalElementsWarned
        {
            get
            {
                return this._totalElementsWarned;
            }
            set
            {
                this._totalElementsWarned = value;
            }
        }
      
        /// <summary>
        /// Property denoting overall progress
        /// </summary>
        [DataMember]
        public Int32 OverAllProgress
        {
            get
            {
                return this._overAllProgress;
            }
            set
            {
                this._overAllProgress = value;
            }
        }

        /// <summary>
        /// Property denoting total steps
        /// </summary>
        [DataMember]
        public Int32 TotalSteps
        {
            get
            {
                return this._totalSteps;
            }
            set
            {
                this._totalSteps = value;
            }
        }

        /// <summary>
        /// Property denoting current execution status of job in summarized form
        /// </summary>
        [DataMember]
        public String CurrentStatusMessage
        {
              get
            {
                return this._currentStatusMessage;
            }
            set
            {
                this._currentStatusMessage = value;
            }
        }

        #endregion

        #region Methods

        #region Load Methods

        ///<summary>
        /// Load ExecutionStatus object from XML.
        /// </summary>
        /// <param name="valuesAsXml">XML having xml value</param>
        /// <example>
        ///     Sample XML:
        /// </example>
        public void LoadExecutionStatus(String valuesAsXml)
        {
            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                XmlTextReader reader = null;
                try
                {
                    reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "ExecutionStatus")
                        {
                            #region Read ExecutionStatus Properties

                            if (reader.HasAttributes)
                            {
                                if (reader.MoveToAttribute("StartTime"))
                                {
                                    this.StartTime = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("EndTime"))
                                {
                                    this.EndTime = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("EstimatedMilliSeconds"))
                                {
                                    this.EstimatedMilliSeconds = ValueTypeHelper.DoubleTryParse(reader.ReadContentAsString(),0);
                                }

                                if (reader.MoveToAttribute("RemainingMilliSeconds"))
                                {
                                    this.RemainingMilliSeconds = ValueTypeHelper.DoubleTryParse(reader.ReadContentAsString(), 0);
                                }

                                if (reader.MoveToAttribute("TotalMilliSeconds"))
                                {
                                    this.TotalMilliSeconds = ValueTypeHelper.DoubleTryParse(reader.ReadContentAsString(), 0);
                                }

                                if (reader.MoveToAttribute("TotalElementsToProcess"))
                                {
                                    this.TotalElementsToProcess = ValueTypeHelper.Int64TryParse(reader.ReadContentAsString(), 0);
                                }

                                if (reader.MoveToAttribute("TotalElementsProcessed"))
                                {
                                    this.TotalElementsProcessed = ValueTypeHelper.Int64TryParse(reader.ReadContentAsString(), 0);
                                }

                                if (reader.MoveToAttribute("TotalElementsSucceed"))
                                {
                                    this.TotalElementsSucceed = ValueTypeHelper.Int64TryParse(reader.ReadContentAsString(), 0);
                                }

                                if (reader.MoveToAttribute("TotalElementsPartiallySucceed"))
                                {
                                    this.TotalElementsPartiallySucceed= ValueTypeHelper.Int64TryParse(reader.ReadContentAsString(), 0);
                                }

                                if (reader.MoveToAttribute("TotalElementsFailed"))
                                {
                                    this.TotalElementsFailed = ValueTypeHelper.Int64TryParse(reader.ReadContentAsString(), 0);
                                }

                                if (reader.MoveToAttribute("TotalElementsWarned"))
                                {
                                    this.TotalElementsWarned = ValueTypeHelper.Int64TryParse(reader.ReadContentAsString(), 0);
                                }

                                if (reader.MoveToAttribute("TotalElementsUnChanged"))
                                {
                                    this.TotalElementsUnChanged = ValueTypeHelper.Int64TryParse(reader.ReadContentAsString(), 0);
                                }

                                if (reader.MoveToAttribute("OverAllProgress"))
                                {
                                    this.OverAllProgress = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), 0);
                                }

                                if (reader.MoveToAttribute("TotalSteps"))
                                {
                                    this.TotalSteps = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), 0);
                                }

                                if (reader.MoveToAttribute("Message"))
                                {
                                    this.CurrentStatusMessage = reader.ReadContentAsString();
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
        /// Get Xml representation of JobData
        /// </summary>
        /// <returns>Xml representation of object</returns>
        public String ToXml()
        {
            String executionStatusXml = String.Empty;
            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            //ExecutionStatus node start
            xmlWriter.WriteStartElement("ExecutionStatus");

            #region Write Properties

            xmlWriter.WriteAttributeString("StartTime", this.StartTime);
            xmlWriter.WriteAttributeString("EndTime", this.EndTime);
            xmlWriter.WriteAttributeString("EstimatedMilliSeconds", this.EstimatedMilliSeconds.ToString());
            xmlWriter.WriteAttributeString("RemainingMilliSeconds", this.RemainingMilliSeconds.ToString());
            xmlWriter.WriteAttributeString("TotalMilliSeconds", this.TotalMilliSeconds.ToString());
            
            xmlWriter.WriteAttributeString("TotalElementsToProcess", this.TotalElementsToProcess.ToString());
            xmlWriter.WriteAttributeString("TotalElementsProcessed", this.TotalElementsProcessed.ToString());
            xmlWriter.WriteAttributeString("TotalElementsSucceed", this.TotalElementsSucceed.ToString());
            xmlWriter.WriteAttributeString("TotalElementsPartiallySucceed", this.TotalElementsPartiallySucceed.ToString());
            xmlWriter.WriteAttributeString("TotalElementsFailed", this.TotalElementsFailed.ToString());
            xmlWriter.WriteAttributeString("TotalElementsWarned", this.TotalElementsWarned.ToString());
            xmlWriter.WriteAttributeString("TotalElementsUnChanged", this.TotalElementsUnChanged.ToString());

            xmlWriter.WriteAttributeString("OverAllProgress", this.OverAllProgress.ToString());
            xmlWriter.WriteAttributeString("TotalSteps", this.TotalSteps.ToString());
            xmlWriter.WriteAttributeString("Message", this.CurrentStatusMessage);

            #endregion

            //ExecutionStatus node end
            xmlWriter.WriteEndElement();

            xmlWriter.Flush();

            //get the actual XML
            executionStatusXml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return executionStatusXml;
        }

        /// <summary>
        /// Get Xml representation of JobData
        /// </summary>
        /// <returns>Xml representation of object</returns>
        public String ToXml(ObjectSerialization serialization)
        {
            String executionStatusXml = String.Empty;

            if (serialization == ObjectSerialization.Full)
            {
                executionStatusXml = this.ToXml();
            }
            else
            {
                StringWriter sw = new StringWriter();
                XmlTextWriter xmlWriter = new XmlTextWriter(sw);

                //ExecutionStatus node start
                xmlWriter.WriteStartElement("ExecutionStatus");

                if (serialization == ObjectSerialization.ProcessingOnly)
                {
                    // TODO : Need to decide which all properties are needed for processing Xml.
                    // currently returning all properties.

                    #region Write ExecutionStatus Properties for ProcessingOnly ExecutionStatus Xml

                    xmlWriter.WriteAttributeString("StartTime", this.StartTime);
                    xmlWriter.WriteAttributeString("EndTime", this.EndTime);
                    xmlWriter.WriteAttributeString("EstimatedMilliSeconds", this.EstimatedMilliSeconds.ToString());
                    xmlWriter.WriteAttributeString("RemainingMilliSeconds", this.RemainingMilliSeconds.ToString());
                    xmlWriter.WriteAttributeString("TotalMilliSeconds", this.TotalMilliSeconds.ToString());
                    
                    xmlWriter.WriteAttributeString("TotalElementsToProcess", this.TotalElementsToProcess.ToString());
                    xmlWriter.WriteAttributeString("TotalElementsProcessed", this.TotalElementsProcessed.ToString());
                    xmlWriter.WriteAttributeString("TotalElementsSucceed", this.TotalElementsSucceed.ToString());
                    xmlWriter.WriteAttributeString("TotalElementsPartiallySucceed", this.TotalElementsPartiallySucceed.ToString());
                    xmlWriter.WriteAttributeString("TotalElementsFailed", this.TotalElementsFailed.ToString());
                    xmlWriter.WriteAttributeString("TotalElementsWarned", this.TotalElementsWarned.ToString());
                    xmlWriter.WriteAttributeString("TotalElementsUnChanged", this.TotalElementsUnChanged.ToString());

                    xmlWriter.WriteAttributeString("OverAllProgress", this.OverAllProgress.ToString());
                    xmlWriter.WriteAttributeString("TotalSteps", this.TotalSteps.ToString());
                    xmlWriter.WriteAttributeString("Message", this.CurrentStatusMessage);

                    #endregion                    
                }
                else if (serialization == ObjectSerialization.UIRender)
                {
                    // TODO : Need to decide which all properties are needed for Rendering Xml.
                    // currently returning all properties.

                    #region Write ExecutionStatus Properties for Rendering ExecutionStatus Xml

                    xmlWriter.WriteAttributeString("StartTime", this.StartTime);
                    xmlWriter.WriteAttributeString("EndTime", this.EndTime);
                    xmlWriter.WriteAttributeString("EstimatedMilliSeconds", this.EstimatedMilliSeconds.ToString());
                    xmlWriter.WriteAttributeString("RemainingMilliSeconds", this.RemainingMilliSeconds.ToString());
                    xmlWriter.WriteAttributeString("TotalMilliSeconds", this.TotalMilliSeconds.ToString());

                    xmlWriter.WriteAttributeString("TotalElementsToProcess", this.TotalElementsToProcess.ToString());
                    xmlWriter.WriteAttributeString("TotalElementsProcessed", this.TotalElementsProcessed.ToString());
                    xmlWriter.WriteAttributeString("TotalElementsSucceed", this.TotalElementsSucceed.ToString());
                    xmlWriter.WriteAttributeString("TotalElementsPartiallySucceed", this.TotalElementsPartiallySucceed.ToString());
                    xmlWriter.WriteAttributeString("TotalElementsFailed", this.TotalElementsFailed.ToString());
                    xmlWriter.WriteAttributeString("TotalElementsWarned", this.TotalElementsWarned.ToString());
                    xmlWriter.WriteAttributeString("TotalElementsUnChanged", this.TotalElementsUnChanged.ToString());

                    xmlWriter.WriteAttributeString("OverAllProgress", this.OverAllProgress.ToString());
                    xmlWriter.WriteAttributeString("TotalSteps", this.TotalSteps.ToString());
                    xmlWriter.WriteAttributeString("Message", this.CurrentStatusMessage);

                    #endregion                    
                }

                //ExecutionStatus node end
                xmlWriter.WriteEndElement();

                xmlWriter.Flush();

                //get the actual XML
                executionStatusXml = sw.ToString();

                xmlWriter.Close();
                sw.Close();
            }

            return executionStatusXml;
        }

        #endregion

        #region Private Methods

        #endregion

        #endregion
    }
}
