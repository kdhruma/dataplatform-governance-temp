using System;
using System.Text;
using System.Xml;
using System.IO;
using System.Runtime.Serialization;

namespace MDM.BusinessObjects.Imports
{
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Import Processing Specifications
    /// </summary>
    [DataContract]
    public class ProcessingSpecifications : ObjectBase
    {
        #region Fields

        /// <summary>
        /// Field denoting import source type
        /// </summary>
        private ImportMode _importMode = ImportMode.Merge;
        
        /// <summary>
        /// Field denoting processing types
        /// </summary>
        private ImportProcessingType _importProcessingType = ImportProcessingType.ValidateAndProcess;

        /// <summary>
        /// | separated values of entity types.
        /// </summary>
        private String _entityTypeList = String.Empty;

        /// <summary>
        /// Field denoting job priority
        /// </summary>
        private Int32 _priority = 0;

        /// <summary>
        /// Job Processing options 
        /// </summary>
        private JobProcessingOptions _jobProcessingOptions = new JobProcessingOptions();

        /// <summary>
        /// Entity Processing options 
        /// </summary>
        private EntityProcessingOptions _entityProcessingOptions = new EntityProcessingOptions();


        // Keyword Options
        private KeywordProcessingOptions _keywordProcessingOptions = new KeywordProcessingOptions();

        #endregion

        #region Constructors

        /// <summary>
        /// Parameterless Constructor
        /// /// </summary>
        public ProcessingSpecifications()
            : base()
        {
        }

        /// <summary>
        /// Parameterized Constructor with values as XML
        /// </summary>
        /// <param name="valuesAsXml">Values in XMl format which has to be set when object is initialized</param>
        public ProcessingSpecifications(String valuesAsXml)
        {
            LoadProcessingSpecifications(valuesAsXml);
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
                return "ProcessingSpecifications";
            }
        }

        /// <summary>
        /// Property defines available import modes
        /// </summary>
        [DataMember]
        public ImportMode ImportMode
        {
            get { return _importMode; }
            set { _importMode = value; }
        }

        /// <summary>
        /// Property defines available import processing types
        /// </summary>
        [DataMember]
        public ImportProcessingType ImportProcessingType
        {
            get { return _importProcessingType; }
            set { _importProcessingType = value; }
        }

        /// <summary>
        /// Property that contains the | separated list of entity types
        /// </summary>
        [DataMember]
        public String EntityTypeList
        {
            get { return _entityTypeList; }
            set { _entityTypeList = value; }
        }

        /// <summary>
        /// Property defines available import processing types
        /// </summary>
        [DataMember]
        public Int32 Priority
        {
            get { return _priority; }
            set { _priority = value; }
        }

        /// <summary>
        /// Property defines job processing options
        /// </summary>
        [DataMember]
        public JobProcessingOptions JobProcessingOptions
        {
            get { return _jobProcessingOptions; }
            set { _jobProcessingOptions = value; }
        }

        /// <summary>
        /// Property defines entity processing options
        /// </summary>
        [DataMember]
        public EntityProcessingOptions EntityProcessingOptions
        {
            get { return _entityProcessingOptions; }
            set { _entityProcessingOptions = value; }
        }

        /// <summary>
        /// Property defines keyword handling options for an entity import
        /// </summary>
        [DataMember]
        public KeywordProcessingOptions KeywordProcessingOptions
        {
            get { return _keywordProcessingOptions; }
            set { _keywordProcessingOptions = value; }
        }
        #endregion

        #region Methods

        #region Load Methods

        ///<summary>
        /// Load ProcessingSpecifications object from XML.
        /// </summary>
        /// <param name="valuesAsXml">XML having xml value</param>
        /// <example>
        ///     Sample XML:
        /// </example>
        public void LoadProcessingSpecifications(String valuesAsXml)
        {
            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                XmlTextReader reader = null;
                try
                {
                    reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "ProcessingSpecifications")
                        {
                            #region Read ProcessingSpecifications Properties

                            if (reader.HasAttributes)
                            {
                                if (reader.MoveToAttribute("ImportMode"))
                                {
                                    ImportMode importMode = ImportMode.UnKnown;
                                    Enum.TryParse(reader.ReadContentAsString(), true, out importMode);
                                    this.ImportMode = importMode;
                                }

                                if (reader.MoveToAttribute("ImportProcessingType"))
                                {
                                    ImportProcessingType importProcessingType = ImportProcessingType.ValidateAndProcess;
                                    Enum.TryParse(reader.ReadContentAsString(), true, out importProcessingType);
                                    this.ImportProcessingType = importProcessingType;
                                }

                                if (reader.MoveToAttribute("EntityTypeList"))
                                {
                                    this.EntityTypeList = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("Priority"))
                                {
                                    Int32 priority = 0;
                                    Int32.TryParse(reader.ReadContentAsString(), out priority);
                                    this.Priority = priority;
                                }
                            }

                            #endregion
                        }
                        else if (reader.NodeType == XmlNodeType.Element && reader.Name == "JobProcessingOptions")
                        {
                            #region Read JobProcessingOptions

                            String jobProcessingOptionsXml = reader.ReadOuterXml();
                            if (!String.IsNullOrEmpty(jobProcessingOptionsXml))
                            {
                                JobProcessingOptions jobProcessingOptions = new JobProcessingOptions(jobProcessingOptionsXml);
                                this.JobProcessingOptions = jobProcessingOptions;
                            }

                            #endregion
                        }
                        else if (reader.NodeType == XmlNodeType.Element && reader.Name == "EntityProcessingOptions")
                        {
                            #region Read EntityProcessingOptions

                            String entityProcessingOptionsXml = reader.ReadOuterXml();
                            if (!String.IsNullOrEmpty(entityProcessingOptionsXml))
                            {
                                EntityProcessingOptions entityProcessingOptions = new EntityProcessingOptions(entityProcessingOptionsXml);
                                this.EntityProcessingOptions = entityProcessingOptions;
                            }

                            #endregion
                        }
                        else if (reader.NodeType == XmlNodeType.Element && reader.Name == "KeywordProcessingOptions")
                        {
                            #region Read KeywordProcessingOptions

                            String keywordProcessingOptionsXml = reader.ReadOuterXml();
                            if (!String.IsNullOrEmpty(keywordProcessingOptionsXml))
                            {
                                KeywordProcessingOptions keywordProcessingOptions = new KeywordProcessingOptions(keywordProcessingOptionsXml);
                                this.KeywordProcessingOptions = keywordProcessingOptions;
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
        /// Get Xml representation of ProcessingSpecifications
        /// </summary>
        /// <returns>Xml representation of object</returns>
        public String ToXml()
        {
            String processingSpecificationXml = String.Empty;
            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            //ProcessingSpecifications node start
            xmlWriter.WriteStartElement("ProcessingSpecifications");

            #region Write ProcessingSpecifications Properties for Full ProcessingSpecifications Xml

            xmlWriter.WriteAttributeString("ImportMode", this.ImportMode.ToString());
            xmlWriter.WriteAttributeString("ImportProcessingType", this.ImportProcessingType.ToString());
            xmlWriter.WriteAttributeString("EntityTypeList", this.EntityTypeList.ToString());
            xmlWriter.WriteAttributeString("Priority", this.Priority.ToString());
            
            #endregion

            #region Write JobProcessingOptions

            if (this.JobProcessingOptions != null)
            {
                xmlWriter.WriteRaw(this.JobProcessingOptions.ToXml());
            }

            #endregion

            #region Write EntityProcessingOptions

            if (this.EntityProcessingOptions != null)
            {
                xmlWriter.WriteRaw(this.EntityProcessingOptions.ToXml());
            }

            #endregion

            #region Write KeywordProcessingOptions

            if (this.KeywordProcessingOptions != null)
            {
                xmlWriter.WriteRaw(this.KeywordProcessingOptions.ToXml());
            }

            #endregion

            //ProcessingSpecifications node end
            xmlWriter.WriteEndElement();

            xmlWriter.Flush();

            //get the actual XML
            processingSpecificationXml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return processingSpecificationXml;
        }

        /// <summary>
        /// Get Xml representation of ProcessingSpecifications
        /// </summary>
        /// <returns>Xml representation of object</returns>
        public String ToXml(ObjectSerialization serialization)
        {
            String processingSpecificationXml = String.Empty;

            if (serialization == ObjectSerialization.Full)
            {
                processingSpecificationXml = this.ToXml();
            }
            else
            {
                StringWriter sw = new StringWriter();
                XmlTextWriter xmlWriter = new XmlTextWriter(sw);

                //ProcessingSpecifications node start
                xmlWriter.WriteStartElement("ProcessingSpecifications");

                if (serialization == ObjectSerialization.ProcessingOnly)
                {
                    #region Write ProcessingSpecifications Properties for ProcessingOnly ProcessingSpecifications Xml

                    xmlWriter.WriteAttributeString("ImportMode", this.ImportMode.ToString());
                    xmlWriter.WriteAttributeString("ImportProcessingType", this.ImportProcessingType.ToString());
                    xmlWriter.WriteAttributeString("EntityTypeList", this.EntityTypeList.ToString());
                    xmlWriter.WriteAttributeString("Priority", this.Priority.ToString());

                    #endregion
                }
                else if (serialization == ObjectSerialization.UIRender)
                {
                    #region Write ProcessingSpecifications Properties for Rendering ProcessingSpecifications Xml

                    xmlWriter.WriteAttributeString("ImportMode", this.ImportMode.ToString());
                    xmlWriter.WriteAttributeString("ImportProcessingType", this.ImportProcessingType.ToString());
                    xmlWriter.WriteAttributeString("EntityTypeList", this.EntityTypeList.ToString());
                    xmlWriter.WriteAttributeString("Priority", this.Priority.ToString());

                    #endregion
                }

                #region Write JobProcessingOptions

                if (this.JobProcessingOptions != null)
                {
                    xmlWriter.WriteRaw(this.JobProcessingOptions.ToXml());
                }

                #endregion

                #region Write EntityProcessingOptions

                if (this.EntityProcessingOptions != null)
                {
                    xmlWriter.WriteRaw(this.EntityProcessingOptions.ToXml(serialization));
                }

                #endregion

                #region Write KeywordProcessingOptions

                if (this.KeywordProcessingOptions != null)
                {
                    xmlWriter.WriteRaw(this.KeywordProcessingOptions.ToXml(serialization));
                }

                #endregion

                //ProcessingSpecifications node end
                xmlWriter.WriteEndElement();

                xmlWriter.Flush();

                //get the actual XML
                processingSpecificationXml = sw.ToString();

                xmlWriter.Close();
                sw.Close();
            }
            return processingSpecificationXml;
        }

        #endregion

        #region Private Methods

        #endregion
        
        #endregion

    }
}
