using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;
using MDM.Core.Extensions;

namespace MDM.BusinessObjects.Diagnostics
{
    using BusinessObjects;
    using Exports;
    using Core;
    using MDM.Interfaces;
    using MDM.Interfaces.Exports;

    /// <summary>
    /// DiagnosticReportProfile
    /// </summary>
    [DataContract]
    public class DiagnosticReportProfile : JobProfile, IDiagnosticReportProfile
    {
        #region Fields

        /// <summary>
        /// diagnostic report type
        /// </summary>
        private String _type = String.Empty;

        /// <summary>
        /// 
        /// </summary>
        private DiagnosticToolsReportType _reportType = DiagnosticToolsReportType.Unknown;

        /// <summary>
        /// 
        /// </summary>
        private DiagnosticToolsReportSubType _reportSubType = DiagnosticToolsReportSubType.Unknown;

        /// <summary>
        /// 
        /// </summary>
        private String _inputXml = String.Empty;

        /// <summary>
        /// Enable
        /// </summary>
        private Boolean _enabled = true;

        /// <summary>
        /// 
        /// </summary>
        private JobType _jobtype = JobType.DiagnosticReportExport;

        /// <summary>
        ///
        /// </summary>
        private String _fileWatcherFolderName = String.Empty;

        /// <summary>
        /// Execution Steps
        /// </summary>
        private DiagnosticReportExecutionStepCollection _executionSteps = new DiagnosticReportExecutionStepCollection();

        /// <summary>
        /// 
        /// </summary>
        private String _uiProfile = String.Empty;

        /// <summary>
        /// list of subscribers as string, comma seperated
        /// </summary>
        private String _dataSubscribers = String.Empty;



        #endregion

        #region Constructors

        /// <summary>
        /// Parameterless Constructor
        /// </summary>
        public DiagnosticReportProfile()
            : base()
        {
            JobType = JobType.DiagnosticReportExport;
            _reportType = DiagnosticToolsReportType.Unknown;
            _reportSubType = DiagnosticToolsReportSubType.Unknown;


            DiagnosticReportExecutionStep process = new DiagnosticReportExecutionStep
            {
                Name = "Process",
                NameInLowerCase = "process",
                LongName = "Process",
                StepType = ExecutionStepType.Core,
                AbortOnError = true,
                Enabled = true,


            };
            ExecutionSteps.Add(process);
        }

        /// <summary>
        /// Constructor with XML having values of object. 
        /// </summary>
        /// <param name="valuesAsXml">Indicates XML having xml value</param>
        public DiagnosticReportProfile(String valuesAsXml)
        {
            LoadDiagnosticReportProfile(valuesAsXml);
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
                return "DiagnosticReportProfile";
            }
        }


        /// <summary>
        /// Property denoting whether profile is enabled or not.
        /// </summary>
        [DataMember]
        public Boolean Enabled
        {
            get { return _enabled; }
            set { _enabled = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public String FileWatcherFolderName
        {
            get
            {
                return _fileWatcherFolderName;
            }
            set
            {
                _fileWatcherFolderName = value;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public DiagnosticToolsReportType DiagnosticToolsReportType
        {
            get
            {
                return _reportType;
            }
            set
            {
                _reportType = value;
            }
        }

        /// <summary>
        /// Processing specifications
        /// </summary>
        [DataMember]
        public DiagnosticToolsReportSubType DiagnosticToolsReportSubType
        {
            get
            {
                return _reportSubType;
            }
            set
            {
                _reportSubType = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public JobType Jobtype
        {
            get { return _jobtype; }
            set { _jobtype = value; }

        }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public String InputXml
        {
            get { return _inputXml; }
            set { _inputXml = value; }
        }

        /// <summary>
        /// Property for Execution Steps
        /// </summary>
        [DataMember]
        public DiagnosticReportExecutionStepCollection ExecutionSteps
        {
            get { return _executionSteps; }
            set { _executionSteps = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public String UIProfile
        {
            get { return _uiProfile; }
            set { _uiProfile = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public String DataSubscribers
        {
            get { return _dataSubscribers; }
            set { _dataSubscribers = value; }
        }

        #endregion

        #region Methods

        #region Load Methods

        ///<summary>
        /// Load DiagnosticReportProfile object from XML.
        /// </summary>
        /// <param name="valuesAsXml">XML having xml value</param>
        /// <example>
        ///     Sample XML:
        /// 
        /// </example>
        public void LoadDiagnosticReportProfile(String valuesAsXml)
        {
            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                XmlTextReader reader = null;
                try
                {
                    reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "UIFormat")
                        {
                            UIProfile = reader.ReadInnerXml();
                        }
                        else if (reader.NodeType == XmlNodeType.Element && reader.Name == "DataFormat")
                        {
                            String dataFormatProfile = reader.ReadInnerXml();
                            ReadDataFormatProfle(dataFormatProfile);
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

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <returns></returns>
        //public ExportSubscriberCollection GetAllSubscribers()
        //{
        //    return _exportSubscriberCollection;
        //}

        #endregion

        #region ToXml methods

        /// <summary>
        /// Get Xml representation of diagnostic report Profile
        /// </summary>
        /// <returns>Xml representation of object</returns>
        public String ToXml(Boolean includeUiProfileXml)
        {
            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            //ProfileData node start
            xmlWriter.WriteStartElement("ProfileData");

            if (includeUiProfileXml == true)
            {
                #region Write UI format xml

                //UIFormat node start
                xmlWriter.WriteStartElement("UIFormat");

                xmlWriter.WriteRaw(UIProfile);

                //UIFormat node end
                xmlWriter.WriteEndElement();

                #endregion Write UI format xml
            }
            //DataFormat node start
            xmlWriter.WriteStartElement("DataFormat");

            //DiagnosticReportProfile node start
            xmlWriter.WriteStartElement("DiagnosticReportProfile");

            #region Write Diagnostic Profile Properties

            xmlWriter.WriteAttributeString("Id", Id.ToString());
            xmlWriter.WriteAttributeString("Name", Name);
            xmlWriter.WriteAttributeString("Enabled", Enabled.ToString().ToLower());
            xmlWriter.WriteAttributeString("FileWatcherFolderName", FileWatcherFolderName.ToString());
            xmlWriter.WriteAttributeString("ReportType", _reportType.ToString());
            xmlWriter.WriteAttributeString("ReportSubType", _reportSubType.ToString());


            #endregion

            #region Write InputXml

            xmlWriter.WriteStartElement("InputXml");

            if (_inputXml != null)
            {
                xmlWriter.WriteRaw(_inputXml);
            }

            xmlWriter.WriteEndElement();

            #endregion

            #region Write ExecutionStepCollection

            xmlWriter.WriteStartElement("ExecutionSteps");

            if (ExecutionSteps != null)
            {
                foreach (var executionStep in ExecutionSteps)
                {
                    xmlWriter.WriteRaw(executionStep.ToXml());
                }
            }

            //ExecutionStepCollection node end
            xmlWriter.WriteEndElement();

            #endregion

            #region Write Data Subscriber Collection as comma seperated string

            xmlWriter.WriteStartElement("DataSubscribers");

            if (!DataSubscribers.IsNullOrEmpty())
            {
                xmlWriter.WriteRaw(_dataSubscribers);
            }

            xmlWriter.WriteEndElement();

            #endregion

            //#region Write SubscriberCollection

            //xmlWriter.WriteStartElement("SubscriberCollection");

            //if (_exportSubscriberCollection != null)
            //{
            //    foreach (var subscriber in _exportSubscriberCollection)
            //    {
            //        xmlWriter.WriteRaw(_exportSubscriberCollection.ToXml());
            //    }

            //}

            //xmlWriter.WriteEndElement();

            //#endregion WRite SubscriberCollection

            //reportProfile node end
            xmlWriter.WriteEndElement();

            //DataFormat node end
            xmlWriter.WriteEndElement();

            //ProfileData node end
            xmlWriter.WriteEndElement();

            xmlWriter.Flush();

            //get the actual XML
            var reportDiagnosticProfileXml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return reportDiagnosticProfileXml;
        }

        /// <summary>
        /// Get Xml representation of diagnostic report Profile
        /// </summary>
        /// <returns>Xml representation of object</returns>
        public String ToXml(ObjectSerialization serialization, Boolean includeUiProfileXml)
        {
            String diagnosticReportProfileXml = String.Empty;

            if (serialization == ObjectSerialization.Full)
            {
                diagnosticReportProfileXml = ToXml(includeUiProfileXml);
            }
            else
            {
                StringWriter sw = new StringWriter();
                XmlTextWriter xmlWriter = new XmlTextWriter(sw);

                //ProfileData node start
                xmlWriter.WriteStartElement("ProfileData");

                if (includeUiProfileXml == true)
                {
                    #region Write UI format xml

                    //UIFormat node start
                    xmlWriter.WriteStartElement("UIFormat");

                    xmlWriter.WriteRaw(UIProfile);

                    //UIFormat node end
                    xmlWriter.WriteEndElement();

                    #endregion Write UI format xml
                }

                //DataFormat node start
                xmlWriter.WriteStartElement("DataFormat");

                //DiagnosticReportProfile node start
                xmlWriter.WriteStartElement("DiagnosticReportProfile");

                if (serialization == ObjectSerialization.ProcessingOnly)
                {
                    #region Write DiagnosticReportProfile Properties

                    xmlWriter.WriteAttributeString("Id", Id.ToString());
                    xmlWriter.WriteAttributeString("Name", Name);
                    xmlWriter.WriteAttributeString("Enabled", Enabled.ToString().ToLower());
                    xmlWriter.WriteAttributeString("FileWatcherFolderName", FileWatcherFolderName);
                    xmlWriter.WriteAttributeString("ReportType", DiagnosticToolsReportType.ToString());
                    xmlWriter.WriteAttributeString("ReportSubtype", DiagnosticToolsReportSubType.ToString());
                    xmlWriter.WriteAttributeString("DataSubscribers", DataSubscribers);

                    #endregion
                }
                else if (serialization == ObjectSerialization.UIRender)
                {
                    #region Write DiagnosticReportProfile Properties

                    xmlWriter.WriteAttributeString("Id", Id.ToString());
                    xmlWriter.WriteAttributeString("Name", Name);
                    xmlWriter.WriteAttributeString("Enabled", Enabled.ToString().ToLower());
                    xmlWriter.WriteAttributeString("FileWatcherFolderName", FileWatcherFolderName.ToString());

                    #endregion
                }

                #region Write ExecutionStepCollection

                xmlWriter.WriteStartElement("ExecutionSteps");

                if (ExecutionSteps != null)
                {
                    foreach (var executionStep in ExecutionSteps)
                    {
                        xmlWriter.WriteRaw(executionStep.ToXml(serialization));
                    }
                }

                //ExecutionStepCollection node end
                xmlWriter.WriteEndElement();

                #endregion

                //DiagnosticReportProfile node end
                xmlWriter.WriteEndElement();

                //DataFormat node end
                xmlWriter.WriteEndElement();

                //ProfileData node end
                xmlWriter.WriteEndElement();

                xmlWriter.Flush();

                //get the actual XML
                diagnosticReportProfileXml = sw.ToString();

                xmlWriter.Close();
                sw.Close();
            }
            return diagnosticReportProfileXml;
        }

        /// <summary>
        /// Get Xml representation of diagnostic report Profile
        /// </summary>
        /// <returns>Xml representation of object</returns>
        public new String ToXml(ObjectSerialization serialization)
        {
            String diagnosticReportProfile = String.Empty;

            if (serialization == ObjectSerialization.Full)
            {
                diagnosticReportProfile = ToXml();
            }
            else
            {
                diagnosticReportProfile = ToXml(serialization, false);
            }
            return diagnosticReportProfile;
        }

        /// <summary>
        /// Get Xml representation of diagnostic report Profile
        /// </summary>
        /// <returns>Xml representation of object</returns>
        public new String ToXml()
        {
            return ToXml(false);
        }

        #endregion

        #region Utility Methods


        /// <summary>
        /// 
        /// </summary>
        /// <param name="subsetDiagnosticReportProfile"></param>
        /// <returns></returns>
        public OperationResult IsSuperSetOfOperationResult(DiagnosticReportProfile subsetDiagnosticReportProfile)
        {
            OperationResult isSuperSetOfResult = new OperationResult();

            Utility.BusinessObjectPropertyCompare("Report Type", DiagnosticToolsReportType.ToString(), subsetDiagnosticReportProfile.DiagnosticToolsReportType.ToString(), isSuperSetOfResult);

            Utility.BusinessObjectPropertyCompare("Report SubType", _reportSubType.ToString(), subsetDiagnosticReportProfile.DiagnosticToolsReportSubType.ToString(), isSuperSetOfResult);

            Utility.BusinessObjectPropertyCompare("InputParameters", InputXml.ToString(), subsetDiagnosticReportProfile.InputXml.ToString(), isSuperSetOfResult);

            Utility.BusinessObjectPropertyCompare("Locale", Locale.ToString(), subsetDiagnosticReportProfile.Locale.ToString(), isSuperSetOfResult);

            Utility.BusinessObjectPropertyCompare("ProfileName", Name.ToString(), subsetDiagnosticReportProfile.Name.ToString(), isSuperSetOfResult);

            Utility.BusinessObjectPropertyCompare("LongName", LongName.ToString(), subsetDiagnosticReportProfile.LongName.ToString(), isSuperSetOfResult);

            return isSuperSetOfResult;
        }

        #endregion

        #region Private Methods

        private void ReadDataFormatProfle(String profileXml)
        {
            if (!String.IsNullOrWhiteSpace(profileXml))
            {
                XmlTextReader reader = null;
                try
                {
                    reader = new XmlTextReader(profileXml, XmlNodeType.Element, null);

                    while (!reader.EOF)
                    {
                        #region Read DiagnosticReportProfile Properties

                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "DiagnosticReportProfile")
                        {

                            if (reader.HasAttributes)
                            {
                                if (reader.MoveToAttribute("Id"))
                                {
                                    Id = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), 0);
                                }

                                if (reader.MoveToAttribute("Name"))
                                {
                                    Name = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("Enabled"))
                                {
                                    Enabled = reader.ReadContentAsBoolean();
                                }

                                if (reader.MoveToAttribute("FileWatcherFolderName"))
                                {
                                    FileWatcherFolderName = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("ReportType"))
                                {
                                    DiagnosticToolsReportType reportType = DiagnosticToolsReportType.Unknown;
                                    if (ValueTypeHelper.EnumTryParse<DiagnosticToolsReportType>(reader.ReadContentAsString(), true, out reportType))
                                    {
                                        DiagnosticToolsReportType = reportType;
                                    }
                                }

                                if (reader.MoveToAttribute("ReportSubType"))
                                {
                                    DiagnosticToolsReportSubType reportSubtype = DiagnosticToolsReportSubType.Unknown;
                                    if (ValueTypeHelper.EnumTryParse<DiagnosticToolsReportSubType>(reader.ReadContentAsString(), true, out reportSubtype))
                                    {
                                        DiagnosticToolsReportSubType = reportSubtype;
                                    }
                                }

                                if (reader.MoveToAttribute("DataSubscribers"))
                                {
                                    DataSubscribers = reader.ReadContentAsString();
                                }
                                reader.Read();
                            }
                        }

                        #endregion

                        #region Read InputXml

                        else if (reader.NodeType == XmlNodeType.Element && reader.Name == "InputXml")
                        {
                            String inputXml = reader.ReadInnerXml();

                            if (!String.IsNullOrEmpty(inputXml))
                            {
                                InputXml = inputXml;
                            }

                        }

                        #endregion Read InputXml

                        #region Read ExecutionStepCollection

                        else if (reader.NodeType == XmlNodeType.Element && reader.Name == "ExecutionSteps")
                        {
                            String executionStepsXml = reader.ReadOuterXml();
                            if (!String.IsNullOrEmpty(executionStepsXml))
                            {
                                var executionSteps = new DiagnosticReportExecutionStepCollection(executionStepsXml);
                                ExecutionSteps = executionSteps;
                            }

                        }

                        #endregion

                        #region Read DataSubscribers

                        else if (reader.NodeType == XmlNodeType.Element && reader.Name == "DataSubscribers")
                        {
                            String dataSubscriberCollectionAsString = reader.ReadElementContentAsString();
                            DataSubscribers = dataSubscriberCollectionAsString;
                        }
                            //only read the subscriber collection as comma seperated; subscriber validation is handled in readFromXml
                        
                        #endregion Read DataSubscribers

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

        #endregion

    }
}
