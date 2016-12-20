using System;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;

namespace MDM.BusinessObjects
{
    using MDM.BusinessObjects.Imports;
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Specifies the class for BusinessRule Import Profile
    /// </summary>
    [DataContract]
    public class DDGImportProfile : JobProfile, IDDGImportProfile
    {
        #region Fields

        /// <summary>
        /// Field denoting type of import
        /// </summary>
        private String _type = String.Empty;

        /// <summary>
        /// Field denoting whether the input file is enabled or not
        /// </summary>
        private Boolean _enabled = true;

        /// <summary>
        /// Field denoting the FileWatcher Folder Name
        /// </summary>
        private String _fileWatcherFolderName = String.Empty;

        /// <summary>
        /// Field denoting the specification about input file.
        /// </summary>
        private InputSpecifications _inputSpecifications = new InputSpecifications();

        /// <summary>
        /// Field denoting the execution steps
        /// </summary>
        private ExecutionStepCollection _executionSteps = new ExecutionStepCollection();

        /// <summary>
        /// Field denoting the DDG Job Processing options 
        /// </summary>
        private DDGJobProcessingOptions _ddgJobProcessingOptions = new DDGJobProcessingOptions();

        #endregion Fields

        #region Properties

        /// <summary>
        /// Property denoting type of import
        /// </summary>
        [DataMember]
        public String ImportType
        {
            get { return _type; }
            set { _type = value; }
        }

        /// <summary>
        /// Property denoting whether input file is enabled or not.
        /// </summary>
        [DataMember]
        public Boolean Enabled
        {
            get { return _enabled; }
            set { _enabled = value; }
        }

        /// <summary>
        /// Property denoting the FileWatcher Folder Name
        /// </summary>
        [DataMember]
        public String FileWatcherFolderName
        {
            get
            {
                return this._fileWatcherFolderName;
            }
            set
            {
                this._fileWatcherFolderName = value;
            }
        }

        /// <summary>
        /// Property denoting specifications about the input file
        /// </summary>
        [DataMember]
        public InputSpecifications InputSpecifications
        {
            get
            {
                return _inputSpecifications;
            }
            set
            {
                _inputSpecifications = value;
            }
        }

        /// <summary>
        /// Property for Execution Steps
        /// </summary>
        [DataMember]
        public ExecutionStepCollection ExecutionSteps
        {
            get { return _executionSteps; }
            set { _executionSteps = value; }
        }

        /// <summary>
        /// Property for DDG Job Processing options
        /// </summary>
        [DataMember]
        public DDGJobProcessingOptions DDGJobProcessingOptions
        {
            get { return _ddgJobProcessingOptions; }
            set { _ddgJobProcessingOptions = value; }
        }

        #endregion Properties

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public DDGImportProfile()
            : base()
        { }

        /// <summary>
        /// Constructor which takes Xml as input
        /// </summary>
        /// <param name="valuesAsXml">DDGImportProfile Object having xml values</param>
        public DDGImportProfile(String valuesAsXml)
        {
            LoadDDGImportProfile(valuesAsXml);
        }

        #endregion Constructor

        #region Methods

        #region Public Methods

        /// <summary>
        /// Get Xml representation of Import Profile
        /// </summary>
        /// <returns>Xml representation of object</returns>
        public override String ToXml()
        {
            #region Sample Xml

            /*
            <ProfileData>
	            <UIFormat />
	            <DataFormat>
		            <DDGImportProfile Id="-1" Name="MDMCenter - RSDDGExcel - Default Profile" ImportType="DDGImport" Enabled="true" FileWatcherFolderName="RSDDGExcel">
			            <InputSpecifications Reader="RSDDGExcel">
				            <ReaderSettings />
			            </InputSpecifications>
			            <ExecutionSteps>
				            <ExecutionStep Name="Process" StepType="Core" AssemblyFileName="" ClassFullName="" AbortOnError="true" Enabled="true">
					            <StepConfiguration />
				            </ExecutionStep>
			            </ExecutionSteps>
			            <DDGJobProcessingOptions ImportProcessingType="ValidateAndProcess" BatchSize="100" />
		            </DDGImportProfile>
	            </DataFormat>
            </ProfileData>
            */

            #endregion Sample Xml

            String importProfileXml = String.Empty;

            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            //ImportProfile node start
            xmlWriter.WriteStartElement("DDGImportProfile");

            #region Write ImportProfile Properties

            xmlWriter.WriteAttributeString("Id", this.Id.ToString());
            xmlWriter.WriteAttributeString("Name", this.Name);
            xmlWriter.WriteAttributeString("ImportType", this.ImportType);
            xmlWriter.WriteAttributeString("Enabled", this.Enabled.ToString().ToLowerInvariant());
            xmlWriter.WriteAttributeString("FileWatcherFolderName", this.FileWatcherFolderName.ToString());

            #endregion Write ImportProfile Properties

            #region Write InputSpecifications

            if (this.InputSpecifications != null)
            {
                xmlWriter.WriteRaw(this.InputSpecifications.ToXml());
            }

            #endregion

            #region Write ExecutionStepCollection

            xmlWriter.WriteStartElement("ExecutionSteps");

            if (this.ExecutionSteps != null)
            {
                foreach (ExecutionStep executionStep in this.ExecutionSteps)
                {
                    xmlWriter.WriteRaw(executionStep.ToXml());
                }
            }

            //ExecutionStepCollection node end
            xmlWriter.WriteEndElement();

            #endregion Write ExecutionStepCollection

            #region Write ProcessingSpecifications

            if (this.DDGJobProcessingOptions != null)
            {
                xmlWriter.WriteRaw(this.DDGJobProcessingOptions.ToXml());
            }

            #endregion Write ProcessingSpecifications

            //ImportProfile node end
            xmlWriter.WriteEndElement();

            xmlWriter.Flush();

            //get the actual XML
            importProfileXml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return importProfileXml;
        }

        /// <summary>
        /// Determines whether two Object instances are equal.
        /// </summary>
        /// <param name="objectToBeCompared">DDGImportProfile Object to compare with the current Object.</param>
        /// <returns>true if the specified Object is equal to the current Object; otherwise, false.</returns>
        public Boolean Equals(DDGImportProfile objectToBeCompared)
        {
            if (this.Id != objectToBeCompared.Id)
                return false;

            if (this.Name != objectToBeCompared.Name)
                return false;

            if (this.ImportType != objectToBeCompared.ImportType)
                return false;

            if (this.Enabled != objectToBeCompared.Enabled)
                return false;

            if (this.InputSpecifications != objectToBeCompared.InputSpecifications)
                return false;

            if (this.ExecutionSteps != objectToBeCompared.ExecutionSteps)
                return false;

            if (this.DDGJobProcessingOptions != objectToBeCompared.DDGJobProcessingOptions)
                return false;

            return true;
        }

        /// <summary>
        /// Serves as a hash function for type
        /// </summary>
        /// <returns>A hash code for the current Object.</returns>
        public override Int32 GetHashCode()
        {
            Int32 hashCode = this.Id.GetHashCode() ^ this.Name.GetHashCode() ^ this.ImportType.GetHashCode() ^ this.Enabled.GetHashCode() ^ 
                             this.ExecutionSteps.GetHashCode() ^ this.InputSpecifications.GetHashCode() ^ this.DDGJobProcessingOptions.GetHashCode();
            return hashCode;
        }

        /// <summary>
        /// Determines whether invoking object is superset of parameterised object.
        /// </summary>
        /// <param name="subsetDDGImportProfile">Indicate the subset DDG import profile object</param>
        /// <param name="compareIds">Indicate the subset DDG import profile object</param>
        /// <returns>Returns true if it is superset, otherwise false.</returns>
        public Boolean IsSuperSetOf(DDGImportProfile subsetDDGImportProfile, Boolean compareIds = false)
        {
            // Note: Is this method really required ?

            if (compareIds)
            {
                if (this.Id != subsetDDGImportProfile.Id)
                    return false;
            }

            if (this.Name != subsetDDGImportProfile.Name)
            {
                return false;
            }

            if (this.ImportType != subsetDDGImportProfile.ImportType)
            {
                return false;
            }

            if (this.Enabled != subsetDDGImportProfile.Enabled)
            {
                return false;
            }

            // Are below checks required ?

            //if (this.ExecutionSteps != subsetBusinessRuleImportProfile.ExecutionSteps)
            //    return false;

            //if (this.InputSpecifications != subsetBusinessRuleImportProfile.InputSpecifications)
            //    return false;

            //if (this.BusinessRuleJobProcessingOptions != subsetBusinessRuleImportProfile.BusinessRuleJobProcessingOptions)
            //    return false;

            return true;
        }

        #endregion Public Methods

        #region Private Methods

        private void LoadDDGImportProfile(String profileXml)
        {
            #region Sample Xml

            /*
            <ProfileData>
	            <UIFormat />
	            <DataFormat>
		            <DDGImportProfile Id="-1" Name="MDMCenter - RSDDGExcel - Default Profile" ImportType="DDGImport" Enabled="true" FileWatcherFolderName="RSDDGExcel">
			            <InputSpecifications Reader="RSDDGExcel">
				            <ReaderSettings />
			            </InputSpecifications>
			            <ExecutionSteps>
				            <ExecutionStep Name="Process" StepType="Core" AssemblyFileName="" ClassFullName="" AbortOnError="true" Enabled="true">
					            <StepConfiguration />
				            </ExecutionStep>
			            </ExecutionSteps>
			            <DDGJobProcessingOptions ImportProcessingType="ValidateAndProcess" BatchSize="100" />
		            </DDGImportProfile>
	            </DataFormat>
            </ProfileData>
            */

            #endregion Sample Xml

            if (!String.IsNullOrWhiteSpace(profileXml))
            {
                using (XmlTextReader reader = new XmlTextReader(profileXml, XmlNodeType.Element, null))
                {
                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "DDGImportProfile")
                        {
                            #region Read ImportProfile Properties

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

                                if (reader.MoveToAttribute("ImportType"))
                                {
                                    this.ImportType = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("Enabled"))
                                {
                                    this.Enabled = reader.ReadContentAsBoolean();
                                }

                                if (reader.MoveToAttribute("FileWatcherFolderName"))
                                {
                                    this.FileWatcherFolderName = reader.ReadContentAsString();
                                }

                                reader.Read();
                            }

                            #endregion Read ImportProfile Properties
                        }
                        else if (reader.NodeType == XmlNodeType.Element && reader.Name == "InputSpecifications")
                        {
                            #region Read InputSpecifications

                            String inputSpecificationsXml = reader.ReadOuterXml();

                            if (!String.IsNullOrEmpty(inputSpecificationsXml))
                            {
                                InputSpecifications inputSpecifications = new InputSpecifications(inputSpecificationsXml);
                                this.InputSpecifications = inputSpecifications;
                            }

                            #endregion Read InputSpecifications
                        }
                        else if (reader.NodeType == XmlNodeType.Element && reader.Name == "ExecutionSteps")
                        {
                            #region Read ExecutionStepCollection

                            String executionStepsXml = reader.ReadOuterXml();

                            if (!String.IsNullOrEmpty(executionStepsXml))
                            {
                                ExecutionStepCollection executionSteps = new ExecutionStepCollection(executionStepsXml);
                                this.ExecutionSteps = executionSteps;
                            }

                            #endregion Read ExecutionStepCollection
                        }
                        else if (reader.NodeType == XmlNodeType.Element && reader.Name == "DDGJobProcessingOptions")
                        {
                            #region Read BusinessRuleJobProcessingOptions

                            String DDGJobProcessingOptionsXml = reader.ReadOuterXml();

                            if (!String.IsNullOrEmpty(DDGJobProcessingOptionsXml))
                            {
                                DDGJobProcessingOptions ddgJobProcessingOptions = new DDGJobProcessingOptions(DDGJobProcessingOptionsXml);
                                this.DDGJobProcessingOptions = ddgJobProcessingOptions;
                            }

                            #endregion Read BusinessRuleJobProcessingOptions
                        }
                        else
                        {
                            //Keep on reading the xml until we reach expected node.
                            reader.Read();
                        }
                    }
                }
            }
        }

        #endregion Private Methods

        #endregion Methods
    }
}
