using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;

namespace MDM.BusinessObjects.Imports
{
    using MDM.BusinessObjects;
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Import profile
    /// </summary>
    [DataContract]
    public class ImportProfile : JobProfile,  IImportProfile
    {
        #region Fields

        /// <summary>
        /// Import type
        /// </summary>
        private String _type = String.Empty;

        /// <summary>
        /// Enable
        /// </summary>
        private Boolean _enabled = true;

        /// <summary>
        ///
        /// </summary>
        private String _fileWatcherFolderName = String.Empty;

        /// <summary>
        /// 
        /// </summary>
        private ProviderCollection _providers = new ProviderCollection();

        /// <summary>
        /// 
        /// </summary>
        private InputSpecifications _inputSpecifications = new InputSpecifications();

        /// <summary>
        /// 
        /// </summary>
        private MappingSpecifications _mappingSpecifications = new MappingSpecifications();

        /// <summary>
        /// 
        /// </summary>
        private ProcessingSpecifications _processingSpecifications = new ProcessingSpecifications();

        /// <summary>
        /// Execution Steps
        /// </summary>
        private ExecutionStepCollection _executionSteps = new ExecutionStepCollection();

        /// <summary>
        /// Field for UI profile xml. UI Rendering understands this Xml. This should be removed once UI is corrected to work on new import profile object.
        /// </summary>
        private String _uiProfile = String.Empty;

        #endregion

        #region Constructors

        /// <summary>
        /// Parameterless Constructor
        /// </summary>
        public ImportProfile()
            : base()
        {
        }

        /// <summary>
        /// Constructor with XML having values of object. 
        /// </summary>
        /// <param name="valuesAsXml">Indicates XML having xml value</param>
        public ImportProfile(String valuesAsXml)
        {
            LoadImportProfile(valuesAsXml);
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
                return "ImportProfile";
            }
        }

        /// <summary>
        /// Property for Type
        /// </summary>
        [DataMember]
        public String ImportType
        {
            get { return _type; }
            set { _type = value; }
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
                return this._fileWatcherFolderName;
            }
            set
            {
                this._fileWatcherFolderName = value;
            }
        }

        /// <summary>
        /// Providers
        /// </summary>
        [DataMember]
        public ProviderCollection Providers
        {
            get
            {
                return _providers;
            }
            set
            {
                _providers = value;
            }
        }

        /// <summary>
        /// Input specifications
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
        /// Mapping specifications
        /// </summary>
        [DataMember]
        public MappingSpecifications MappingSpecifications
        {
            get
            {
                return _mappingSpecifications;
            }
            set
            {
                _mappingSpecifications = value;
            }
        }

        /// <summary>
        /// Processing specifications
        /// </summary>
        [DataMember]
        public ProcessingSpecifications ProcessingSpecifications
        {
            get
            {
                return _processingSpecifications;
            }
            set
            {
                _processingSpecifications = value;
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
        /// Property for UI profile xml. UI Rendering understands this Xml. This should be removed once UI is corrected to work on new import profile object.
        /// </summary>
        [DataMember]
        public String UIProfile
        {
            get
            {
                return _uiProfile;
            }
            set
            {
                _uiProfile = value;
            }
        }

        #endregion

        #region Methods

        #region Load Methods

        ///<summary>
        /// Load ImportProfile object from XML.
        /// </summary>
        /// <param name="valuesAsXml">XML having xml value</param>
        /// <example>
        ///     Sample XML:
        /// </example>
        public void LoadImportProfile(String valuesAsXml)
        {
            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                XmlTextReader reader = null;
                try
                {
                    reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                    while (!reader.EOF)
                    {
                        if(reader.NodeType == XmlNodeType.Element && reader.Name == "UIFormat")
                        {
                            this.UIProfile = reader.ReadInnerXml();
                        }
                        else if(reader.NodeType == XmlNodeType.Element && reader.Name == "DataFormat")
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

        #endregion

        #region ToXml methods

        /// <summary>
        /// Get Xml representation of Import Profile
        /// </summary>
        /// <returns>Xml representation of object</returns>
        public String ToXml(Boolean includeUIProfileXml)
        {
            String importProfileXml = String.Empty;
            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            //ProfileData node start
            xmlWriter.WriteStartElement("ProfileData");

            if(includeUIProfileXml == true)
            {
                #region Write UI format xml

                //UIFormat node start
                xmlWriter.WriteStartElement("UIFormat");

                xmlWriter.WriteRaw(this.UIProfile);

                //UIFormat node end
                xmlWriter.WriteEndElement();

                #endregion Write UI format xml
            }
            //DataFormat node start
            xmlWriter.WriteStartElement("DataFormat");

            //ImportProfile node start
            xmlWriter.WriteStartElement("ImportProfile");

            #region Write ImportProfile Properties

            xmlWriter.WriteAttributeString("Id", this.Id.ToString());
            xmlWriter.WriteAttributeString("Name", this.Name);
            xmlWriter.WriteAttributeString("ImportType", this.ImportType);
            xmlWriter.WriteAttributeString("Enabled", this.Enabled.ToString().ToLower());
            xmlWriter.WriteAttributeString("FileWatcherFolderName", this.FileWatcherFolderName.ToString());

            #endregion

            #region Write Providers

            if (this.Providers != null)
            {
                xmlWriter.WriteRaw(this.Providers.ToXml());
            }

            #endregion

            #region Write InputSpecifications

            if (this.InputSpecifications != null)
            {
                xmlWriter.WriteRaw(this.InputSpecifications.ToXml());
            }

            #endregion

            #region Write MappingSpecifications

            if (this.MappingSpecifications != null)
            {
                xmlWriter.WriteRaw(this.MappingSpecifications.ToXml());
            }

            #endregion

            #region Write ProcessingSpecifications

            if (this.ProcessingSpecifications != null)
            {
                xmlWriter.WriteRaw(this.ProcessingSpecifications.ToXml());
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

            #endregion

            //ImportProfile node end
            xmlWriter.WriteEndElement();

            //DataFormat node end
            xmlWriter.WriteEndElement();

            //ProfileData node end
            xmlWriter.WriteEndElement();

            xmlWriter.Flush();

            //get the actual XML
            importProfileXml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return importProfileXml;
        }

        /// <summary>
        /// Get Xml representation of Import Profile
        /// </summary>
        /// <returns>Xml representation of object</returns>
        public String ToXml(ObjectSerialization serialization, Boolean includeUIProfileXml)
        {
            String importProfileXml = String.Empty;

            if(serialization == ObjectSerialization.Full)
            {
                importProfileXml = this.ToXml(includeUIProfileXml);
            }
            else
            {
                StringWriter sw = new StringWriter();
                XmlTextWriter xmlWriter = new XmlTextWriter(sw);

                //ProfileData node start
                xmlWriter.WriteStartElement("ProfileData");

                if(includeUIProfileXml == true)
                {
                    #region Write UI format xml

                    //UIFormat node start
                    xmlWriter.WriteStartElement("UIFormat");

                    xmlWriter.WriteRaw(this.UIProfile);

                    //UIFormat node end
                    xmlWriter.WriteEndElement();

                    #endregion Write UI format xml
                }

                //DataFormat node start
                xmlWriter.WriteStartElement("DataFormat");

                //ImportProfile node start
                xmlWriter.WriteStartElement("ImportProfile");

                if(serialization == ObjectSerialization.ProcessingOnly)
                {
                    #region Write ImportProfile Properties

                    xmlWriter.WriteAttributeString("Id", this.Id.ToString());
                    xmlWriter.WriteAttributeString("Name", this.Name);
                    xmlWriter.WriteAttributeString("ImportType", this.ImportType);
                    xmlWriter.WriteAttributeString("Enabled", this.Enabled.ToString().ToLower());
                    xmlWriter.WriteAttributeString("FileWatcherFolderName", this.FileWatcherFolderName.ToString());

                    #endregion
                }
                else if(serialization == ObjectSerialization.UIRender)
                {
                    #region Write ImportProfile Properties

                    xmlWriter.WriteAttributeString("Id", this.Id.ToString());
                    xmlWriter.WriteAttributeString("Name", this.Name);
                    xmlWriter.WriteAttributeString("ImportType", this.ImportType);
                    xmlWriter.WriteAttributeString("Enabled", this.Enabled.ToString().ToLower());
                    xmlWriter.WriteAttributeString("FileWatcherFolderName", this.FileWatcherFolderName.ToString());

                    #endregion
                }

                #region Write Providers

                if(this.Providers != null)
                {
                    xmlWriter.WriteRaw(this.Providers.ToXml());
                }

                #endregion

                #region Write InputSpecifications

                if(this.InputSpecifications != null)
                {
                    xmlWriter.WriteRaw(this.InputSpecifications.ToXml());
                }

                #endregion

                #region Write MappingSpecifications

                if(this.MappingSpecifications != null)
                {
                    xmlWriter.WriteRaw(this.MappingSpecifications.ToXml());
                }

                #endregion

                #region Write ProcessingSpecifications

                if(this.ProcessingSpecifications != null)
                {
                    xmlWriter.WriteRaw(this.ProcessingSpecifications.ToXml());
                }
                #endregion

                #region Write ExecutionStepCollection

                xmlWriter.WriteStartElement("ExecutionSteps");

                if(this.ExecutionSteps != null)
                {
                    foreach(ExecutionStep executionStep in this.ExecutionSteps)
                    {
                        xmlWriter.WriteRaw(executionStep.ToXml(serialization));
                    }
                }

                //ExecutionStepCollection node end
                xmlWriter.WriteEndElement();

                #endregion

                //ImportProfile node end
                xmlWriter.WriteEndElement();

                //DataFormat node end
                xmlWriter.WriteEndElement();

                //ProfileData node end
                xmlWriter.WriteEndElement();

                xmlWriter.Flush();

                //get the actual XML
                importProfileXml = sw.ToString();

                xmlWriter.Close();
                sw.Close();
            }
            return importProfileXml;
        }

        /// <summary>
        /// Get Xml representation of Import Profile
        /// </summary>
        /// <returns>Xml representation of object</returns>
        public new String ToXml(ObjectSerialization serialization)
        {
            String importProfileXml = String.Empty;

            if (serialization == ObjectSerialization.Full)
            {
                importProfileXml = this.ToXml();
            }
            else
            {
                importProfileXml = this.ToXml(serialization, false);
            }
            return importProfileXml;
        }

        /// <summary>
        /// Get Xml representation of Import Profile
        /// </summary>
        /// <returns>Xml representation of object</returns>
        public new String ToXml()
        {
            return this.ToXml(false);
        }

        #endregion

        #region Utility Methods

        /// <summary>
        /// Returns collection of key value pair specifing entity metadata fields to be filled in with the file data.
        /// Example: entity.LongName = [Sheet1$].[General//Product Desc] (excel file column named "Product Desc")
        /// This method shall be used by import reader while entity metadata with corrosponding input field values.
        /// </summary>
        /// <returns></returns>
        public Collection<KeyValuePair<EntityMetadataMapObjectType, String>> GetEntityMetadataInputFields()
        {
            Collection<KeyValuePair<EntityMetadataMapObjectType, String>> result = new Collection<KeyValuePair<EntityMetadataMapObjectType, String>>();

            if(this.MappingSpecifications.EntityMetadataMap.ShortNameMap.Mode == MappingMode.InputField)
                result.Add(new KeyValuePair<EntityMetadataMapObjectType,String>(EntityMetadataMapObjectType.ShortNameMap, this.MappingSpecifications.EntityMetadataMap.ShortNameMap.InputFieldName));

            if (this.MappingSpecifications.EntityMetadataMap.LongNameMap.Mode == MappingMode.InputField)
                result.Add(new KeyValuePair<EntityMetadataMapObjectType, String>(EntityMetadataMapObjectType.LongNameMap, this.MappingSpecifications.EntityMetadataMap.ShortNameMap.InputFieldName));

            if (this.MappingSpecifications.EntityMetadataMap.ContainerMap.Mode == MappingMode.InputField)
                result.Add(new KeyValuePair<EntityMetadataMapObjectType, String>(EntityMetadataMapObjectType.ContainerMap, this.MappingSpecifications.EntityMetadataMap.ContainerMap.InputFieldName));

            if (this.MappingSpecifications.EntityMetadataMap.EntityTypeMap.Mode == MappingMode.InputField)
                result.Add(new KeyValuePair<EntityMetadataMapObjectType, String>(EntityMetadataMapObjectType.EntityTypeMap, this.MappingSpecifications.EntityMetadataMap.EntityTypeMap.InputFieldName));

            if (this.MappingSpecifications.EntityMetadataMap.SourceCategoryMap.Mode == MappingMode.InputField)
                result.Add(new KeyValuePair<EntityMetadataMapObjectType, String>(EntityMetadataMapObjectType.SourceCategoryMap, this.MappingSpecifications.EntityMetadataMap.SourceCategoryMap.InputFieldName));

            if (this.MappingSpecifications.EntityMetadataMap.TargetCategoryMap.Mode == MappingMode.InputField)
                result.Add(new KeyValuePair<EntityMetadataMapObjectType, String>(EntityMetadataMapObjectType.TargetCategoryMap, this.MappingSpecifications.EntityMetadataMap.TargetCategoryMap.InputFieldName));

            if (this.MappingSpecifications.EntityMetadataMap.HierarchyParentEntityMap.Mode == MappingMode.InputField)
                result.Add(new KeyValuePair<EntityMetadataMapObjectType, String>(EntityMetadataMapObjectType.HierarchyParentEntityMap, this.MappingSpecifications.EntityMetadataMap.HierarchyParentEntityMap.InputFieldName));

            if (this.MappingSpecifications.EntityMetadataMap.MDLParentEntityMap.Mode == MappingMode.InputField)
                result.Add(new KeyValuePair<EntityMetadataMapObjectType, String>(EntityMetadataMapObjectType.MDLParentEntityMap, this.MappingSpecifications.EntityMetadataMap.MDLParentEntityMap.InputFieldName));

            return result;
     
        }

        #endregion

        #region Private Methods

        private void ReadDataFormatProfle(String profileXml)
        {
            if(!String.IsNullOrWhiteSpace(profileXml))
            {
                XmlTextReader reader = null;
                try
                {
                    reader = new XmlTextReader(profileXml, XmlNodeType.Element, null);

                    while(!reader.EOF)
                    {
                        if(reader.NodeType == XmlNodeType.Element && reader.Name == "ImportProfile")
                        {
                            #region Read ImportProfile Properties

                            if(reader.HasAttributes)
                            {
                                if(reader.MoveToAttribute("Id"))
                                {
                                    this.Id = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), 0);
                                }

                                if(reader.MoveToAttribute("Name"))
                                {
                                    this.Name = reader.ReadContentAsString();
                                }

                                if(reader.MoveToAttribute("ImportType"))
                                {
                                    this.ImportType = reader.ReadContentAsString();
                                }

                                if(reader.MoveToAttribute("Enabled"))
                                {
                                    this.Enabled = reader.ReadContentAsBoolean();
                                }

                                if(reader.MoveToAttribute("FileWatcherFolderName"))
                                {
                                    this.FileWatcherFolderName = reader.ReadContentAsString();
                                }
                                reader.Read();
                            }

                            #endregion
                        }
                        else if(reader.NodeType == XmlNodeType.Element && reader.Name == "Providers")
                        {
                            #region Read Providers

                            String providersXml = reader.ReadOuterXml();
                            if(!String.IsNullOrEmpty(providersXml))
                            {
                                ProviderCollection providers = new ProviderCollection(providersXml);
                                this.Providers = providers;
                            }

                            #endregion
                        }
                        else if(reader.NodeType == XmlNodeType.Element && reader.Name == "InputSpecifications")
                        {
                            #region Read InputSpecifications

                            String inputSpecificationsXml = reader.ReadOuterXml();
                            if(!String.IsNullOrEmpty(inputSpecificationsXml))
                            {
                                InputSpecifications inputSpecifications = new InputSpecifications(inputSpecificationsXml);
                                this.InputSpecifications = inputSpecifications;
                            }

                            #endregion
                        }
                        else if(reader.NodeType == XmlNodeType.Element && reader.Name == "MappingSpecifications")
                        {
                            #region Read MappingSpecifications

                            String mappingSpecificationsXml = reader.ReadOuterXml();
                            if(!String.IsNullOrEmpty(mappingSpecificationsXml))
                            {
                                MappingSpecifications mappingSpecifications = new MappingSpecifications(mappingSpecificationsXml);
                                this.MappingSpecifications = mappingSpecifications;
                            }

                            #endregion
                        }
                        else if(reader.NodeType == XmlNodeType.Element && reader.Name == "ProcessingSpecifications")
                        {
                            #region Read ProcessingSpecifications

                            String processingSpecificationsXml = reader.ReadOuterXml();
                            if(!String.IsNullOrEmpty(processingSpecificationsXml))
                            {
                                ProcessingSpecifications processingSpecifications = new ProcessingSpecifications(processingSpecificationsXml);
                                this.ProcessingSpecifications = processingSpecifications;
                            }

                            #endregion
                        }
                        else if (reader.NodeType == XmlNodeType.Element && reader.Name == "ExecutionSteps")
                        {
                            #region Read ExecutionStepCollection

                            String executionStepsXml = reader.ReadOuterXml();
                            if(!String.IsNullOrEmpty(executionStepsXml))
                            {
                                ExecutionStepCollection executionSteps = new ExecutionStepCollection(executionStepsXml);
                                this.ExecutionSteps = executionSteps;
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
                    if(reader != null)
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
