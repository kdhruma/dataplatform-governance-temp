using System;
using System.IO;
using System.Xml;
using System.Runtime.Serialization;

namespace MDM.BusinessObjects.Imports
{
    using MDM.Core;
    using MDM.Interfaces;
    using MDM.BusinessObjects;

    /// <summary>
    /// Represents the Lookup Import profile Object.
    /// </summary>
    [DataContract]
    public class LookupImportProfile :  JobProfile, ILookupImportProfile
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
        /// Field denoting folder name for the file watcher
        /// </summary>
        private String _fileWatcherFolderName = String.Empty;

        /// <summary>
        /// Field denoting the execution steps
        /// </summary>
        private ExecutionStepCollection _executionSteps = new ExecutionStepCollection();

        /// <summary>
        /// Field denoting the specification about input file.
        /// </summary>
        private InputSpecifications _inputSpecifications = new InputSpecifications();

        /// <summary>
        /// Field denoting lookup job processing options 
        /// </summary>
        private LookupJobProcessingOptions _lookupJobProcessingOptions = new LookupJobProcessingOptions();

        #endregion

        #region Constructors

        /// <summary>
        /// Parameterless Constructor
        /// </summary>
        public LookupImportProfile()
            : base()
        {
        }

        /// <summary>
        /// Constructor which takes Xml as input
        /// </summary>
        /// <param name="valuesAsXml">XML having xml values</param>
        public LookupImportProfile(String valuesAsXml)
        {
            LoadLookupImportProfile(valuesAsXml);
        }
        #endregion

        #region Properties

        /// <summary>
        /// Property indicating type of import
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
        /// Property denoting folder name for file watcher import
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
        /// Property for lookup job processing options
        /// </summary>
        [DataMember]
        public LookupJobProcessingOptions LookupJobProcessingOptions
        {
            get { return _lookupJobProcessingOptions; }
            set { _lookupJobProcessingOptions = value; }
        }
        #endregion

        #region Methods

        #region Load Methods

        ///<summary>
        /// Load LookupImportProfile object from XML.
        /// </summary>
        /// <param name="profileXml">Indicates XML having lookup export profile xml value</param>
        /// <example>
        ///     Sample XML:
        ///     <LookupImportProfile Id="-1" Name="MDMCenter - RSXml 1.1 - Default Lookup Profile" ImportType="LookupImport" Enabled="true" FileWatcherFolderName="RSXmlLookup11">
	    ///     <ExecutionSteps>
		///         <ExecutionStep Name="Process" StepType="Core" AssemblyFileName="" ClassFullName="" AbortOnError="true" Enabled="true" />
	    ///     </ExecutionSteps>
	    ///     <ReaderSettings />
	    ///     <InputSpecifications Reader="RSXmlLookup11">
	    ///     </InputSpecifications>
	    ///     <LookupJobProcessingOptions NumberofLookupThreads="2" NumberofRecordThreadsPerLookupThread="2" BatchSize="100" />
        ///     </LookupImportProfile>	
        /// </example>
        public void LoadLookupImportProfile(String profileXml)
        {
             if (!String.IsNullOrWhiteSpace(profileXml))
            {
                XmlTextReader reader = null;
                try
                {
                    reader = new XmlTextReader(profileXml, XmlNodeType.Element, null);

                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "LookupImportProfile")
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

                            #endregion
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

                            #endregion
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

                            #endregion
                        }
                        else if (reader.NodeType == XmlNodeType.Element && reader.Name == "LookupJobProcessingOptions")
                        {
                            #region Read LookupJobProcessingOptions

                            String lookupJobProcessingOptionsXml = reader.ReadOuterXml();
                            if (!String.IsNullOrEmpty(lookupJobProcessingOptionsXml))
                            {
                                LookupJobProcessingOptions lookupJobProcessingOptions = new LookupJobProcessingOptions(lookupJobProcessingOptionsXml);
                                this.LookupJobProcessingOptions = lookupJobProcessingOptions;
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
        /// Get Xml representation of Import Profile
        /// </summary>
        /// <returns>Xml representation of object</returns>
        public override String ToXml()
        {
            String importProfileXml = String.Empty;
            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            //ImportProfile node start
            xmlWriter.WriteStartElement("LookupImportProfile");

            #region Write ImportProfile Properties

            xmlWriter.WriteAttributeString("Id", this.Id.ToString());
            xmlWriter.WriteAttributeString("Name", this.Name);
            xmlWriter.WriteAttributeString("ImportType", this.ImportType);
            xmlWriter.WriteAttributeString("Enabled", this.Enabled.ToString().ToLowerInvariant());
            xmlWriter.WriteAttributeString("FileWatcherFolderName", this.FileWatcherFolderName.ToString());

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

            #endregion

            #region Write InputSpecifications

            if (this.InputSpecifications != null)
            {
                xmlWriter.WriteRaw(this.InputSpecifications.ToXml());
            }

            #endregion

            #region Write ProcessingSpecifications

            if (this.LookupJobProcessingOptions != null)
            {
                xmlWriter.WriteRaw(this.LookupJobProcessingOptions.ToXml());
            }

            #endregion

            //LookupImportProfile node end
            xmlWriter.WriteEndElement();

            xmlWriter.Flush();

            //get the actual XML
            importProfileXml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return importProfileXml;
        }

        /// <summary>
        /// Get Xml representation of Lookup Import Profile
        /// </summary>
        /// <returns>Xml representation of object</returns>
        public new String ToXml(ObjectSerialization serialization)
        {
            return this.ToXml();
        }

        /// <summary>
        /// Determines whether two Object instances are equal.
        /// </summary>
        /// <param name="objectToBeCompared">LookupImportProfile Object to compare with the current Object.</param>
        /// <returns>true if the specified Object is equal to the current Object; otherwise, false.</returns>
        public Boolean Equals(LookupImportProfile objectToBeCompared)
        {
            if (this.Id != objectToBeCompared.Id)
                return false;

            if (this.Name != objectToBeCompared.Name)
                return false;

            if (this.ImportType != objectToBeCompared.ImportType)
                return false;

            if (this.Enabled != objectToBeCompared.Enabled)
                return false;

            if (this.FileWatcherFolderName != objectToBeCompared.FileWatcherFolderName)
                return false;

            if (this.ExecutionSteps != objectToBeCompared.ExecutionSteps)
                return false;

            if (this.InputSpecifications != objectToBeCompared.InputSpecifications)
                return false;

            if (this.LookupJobProcessingOptions != objectToBeCompared.LookupJobProcessingOptions)
                return false;

            return true;
        }

        /// <summary>
        /// Serves as a hash function for type
        /// </summary>
        /// <returns>A hash code for the current Object.</returns>
        public override int GetHashCode()
        {
            int hashCode = this.Id.GetHashCode() ^ this.Name.GetHashCode() ^ this.ImportType.GetHashCode() ^ this.Enabled.GetHashCode() ^ this.FileWatcherFolderName.GetHashCode() ^ this.ExecutionSteps.GetHashCode() ^ this.InputSpecifications.GetHashCode() ^ this.LookupJobProcessingOptions.GetHashCode();
            return hashCode;
        }

        /// <summary>
        /// Determines whether invoking object is superset of parameterised object.
        /// </summary>
        /// <param name="subsetLookupImportProfile">Indicate the subset lookup import profile object</param>
        /// <returns>Returns true if it is superset, otherwise false.</returns>
        public Boolean IsSuperSetOf(LookupImportProfile subsetLookupImportProfile)
        {
            if (this.Id != subsetLookupImportProfile.Id)
                return false;

            if (this.Name != subsetLookupImportProfile.Name)
                return false;

            if (this.ImportType != subsetLookupImportProfile.ImportType)
                return false;

            if (this.Enabled != subsetLookupImportProfile.Enabled)
                return false;

            if (this.FileWatcherFolderName != subsetLookupImportProfile.FileWatcherFolderName)
                return false;

            if (this.ExecutionSteps != subsetLookupImportProfile.ExecutionSteps)
                return false;

            if (this.InputSpecifications != subsetLookupImportProfile.InputSpecifications)
                return false;

            if (this.LookupJobProcessingOptions != subsetLookupImportProfile.LookupJobProcessingOptions)
                return false;

            return true;
        }
        #endregion

        #region Private Methods

        #endregion

        #endregion
    }
}
