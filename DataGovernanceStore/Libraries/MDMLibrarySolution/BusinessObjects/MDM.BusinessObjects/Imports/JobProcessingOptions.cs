using System;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;

namespace MDM.BusinessObjects
{
    using MDM.Core;

    /// <summary>
    /// Specifies JobProcessingOptions which specifies various flags and indications to entity processing logic
    /// </summary>
    [DataContract]
    public class JobProcessingOptions : ObjectBase
    {
        #region Fields

        /// <summary>
        /// Indicates the number of entity threads/tasks that will be used for the import processing.
        /// </summary>
        private Int32 _numberofEntityThreads = -1;

        /// <summary>
        /// Indicates the number of Attribute threads per entity threads that will be used for the initial load processing.
        /// </summary>
        private Int32 _numberofAttributeThreadsPerEntityThread = -1;

        /// <summary>
        /// Batch size for the processing.
        /// </summary>
        private Int32 _batchSize = -1;

        /// <summary>
        /// Indicates the total number of failure entities to abort the job
        /// </summary>
        private Int32 _abortThreasholdEntityCount = 0;

        /// <summary>
        /// Indicates the total number of failures to abort the job
        /// </summary>
        private Int32 _abortThreasholdErrorCount = 0;

        /// <summary>
        /// Indicates if we need to save successful entities in the job result table.
        /// </summary>
        private Boolean _saveSuccessEntitiesResult = false;

        /// <summary>
        /// Indicates if we need to validate the schema or not.
        /// </summary>
        private Boolean _validateSchema = false;

        /// <summary>
        /// Process local values denorm during intial load.
        /// </summary>
        private Boolean _executeDenormProcess = true;

        /// <summary>
        /// 
        /// </summary>
        private OperationResultType _attributeValidationLevel = OperationResultType.Warning;

        /// <summary>
        /// 
        /// </summary>
        private OperationResultType _relationshipTypeValidationLevel = OperationResultType.Warning;

        /// <summary>
        /// 
        /// </summary>
        private OperationResultType _relationshipAttributeValidationLevel = OperationResultType.Warning;

        /// <summary>
        /// indicates if the engine needs to calculate the lookup reference id for the attribute values. When this is false, it is assumed that the 
        /// staging data would have popualted it and the engine will simply use that value. This setting is only valid for Initial Load
        /// </summary>
        private Boolean _populateLookupRefIdForInitialLoad = true;

        /// <summary>
        /// indicates if the engine needs to cleanse all the internal Ids / PKs given in the file
        /// </summary>
        private Boolean _cleanseInternalIdsFromSourceData = true;

        #endregion Fields

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public JobProcessingOptions()
            : base()
        { }

        /// <summary>
        /// Constructor with XML having values of object. 
        /// </summary>
        /// <param name="valuesAsXml">Indicates XML having xml value</param>
        public JobProcessingOptions(String valuesAsXml)
        {
            LoadJobProcessingOptions(valuesAsXml);
        }

        #endregion Constructor

        #region Properties

        /// <summary>
        /// Indicates the number of entity threads.
        /// </summary>
        [DataMember]
        public Int32 NumberofEntityThreads
        {
            get { return _numberofEntityThreads; }
            set { _numberofEntityThreads = value; }
        }

        /// <summary>
        /// Indicates the number of attribute threads per entity thread.
        /// </summary>
        [DataMember]
        public Int32 NumberofAttributeThreadsPerEntityThread
        {
            get { return _numberofAttributeThreadsPerEntityThread; }
            set { _numberofAttributeThreadsPerEntityThread = value; }
        }

        /// <summary>
        /// Indicates the batch size for the import processing.
        /// </summary>
        [DataMember]
        public Int32 BatchSize
        {
            get { return _batchSize; }
            set { _batchSize = value; }
        }

        /// <summary>
        /// Indicates the total number of failure entities to abort the job
        /// </summary>
        [DataMember]
        public Int32 AbortThreasholdEntityCount
        {
            get { return _abortThreasholdEntityCount; }
            set { _abortThreasholdEntityCount = value; }
        }

        /// <summary>
        /// Indicates the total number of failures to abort the job
        /// </summary>
        [DataMember]
        public Int32 AbortThreasholdErrorCount
        {
            get { return _abortThreasholdErrorCount; }
            set { _abortThreasholdErrorCount = value; }
        }

        /// <summary>
        /// Indicates if we need to save successful entities in the job result table.
        /// </summary>
        [DataMember]
        public Boolean SaveSuccessEntitiesResult
        {
            get { return _saveSuccessEntitiesResult; }
            set { _saveSuccessEntitiesResult = value; }
        }

        /// <summary>
        /// Indicates if we need to validate the schema or not.
        /// </summary>
        [DataMember]
        public Boolean ValidateSchema
        {
            get { return _validateSchema; }
            set { _validateSchema = value; }
        }

        /// <summary>
        /// Indicates if the engine needs to process local values denorm during intial load.
        /// </summary>
        [DataMember]
        public Boolean ExecuteDenormProcess
        {
            get { return _executeDenormProcess; }
            set { _executeDenormProcess = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public OperationResultType AttributeValidationLevel
        {
            get { return _attributeValidationLevel; }
            set { _attributeValidationLevel = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public OperationResultType RelationshipTypeValidationLevel
        {
            get { return _relationshipTypeValidationLevel; }
            set { _relationshipTypeValidationLevel = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public OperationResultType RelationshipAttributeValidationLevel
        {
            get { return _relationshipAttributeValidationLevel; }
            set { _relationshipAttributeValidationLevel = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public Boolean PopulateLookupRefIdForInitialLoad
        {
            get { return _populateLookupRefIdForInitialLoad; }
            set { _populateLookupRefIdForInitialLoad = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public Boolean CleanseInternalIdsFromSourceData
        {
            get { return _cleanseInternalIdsFromSourceData; }
            set { _cleanseInternalIdsFromSourceData = value; }
        }

        #endregion Properties

        #region Methods
         
        #region Public methods

        /// <summary>
        /// Represents JobProcessingOptions in Xml format
        /// </summary>
        /// <returns>String representation of current JobProcessingOptions object instance</returns>
        public String ToXml()
        {
            String xml = String.Empty;

            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            //Attribute node start
            xmlWriter.WriteStartElement("JobProcessingOptions");

            xmlWriter.WriteAttributeString("NumberofEntityThreads", this.NumberofEntityThreads.ToString());
            xmlWriter.WriteAttributeString("NumberofAttributeThreadsPerEntityThread", this.NumberofAttributeThreadsPerEntityThread.ToString());
            xmlWriter.WriteAttributeString("BatchSize", this.BatchSize.ToString());

            xmlWriter.WriteAttributeString("AbortThreasholdEntityCount", this.AbortThreasholdEntityCount.ToString());
            xmlWriter.WriteAttributeString("AbortThreasholdErrorCount", this.AbortThreasholdErrorCount.ToString());

            xmlWriter.WriteAttributeString("SaveSuccessEntitiesResult", this.SaveSuccessEntitiesResult.ToString().ToLower());
            xmlWriter.WriteAttributeString("ValidateSchema", this.ValidateSchema.ToString().ToLower());
            xmlWriter.WriteAttributeString("ExecuteDenormProcess", this.ExecuteDenormProcess.ToString().ToLower());
           
            xmlWriter.WriteAttributeString("AttributeValidationLevel", this.AttributeValidationLevel.ToString());
            xmlWriter.WriteAttributeString("RelationshipTypeValidationLevel", this.RelationshipTypeValidationLevel.ToString());
            xmlWriter.WriteAttributeString("RelationshipAttributeValidationLevel", this.RelationshipAttributeValidationLevel.ToString());
            xmlWriter.WriteAttributeString("PopulateLookupRefIdForInitialLoad", this.PopulateLookupRefIdForInitialLoad.ToString().ToLower());
            xmlWriter.WriteAttributeString("CleanseInternalIdsFromSourceData", this.CleanseInternalIdsFromSourceData.ToString().ToLower());

            //JobProcessingOptions end node
            xmlWriter.WriteEndElement();

            xmlWriter.Flush();
            //get the actual XML
            xml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return xml;
        }

        /// <summary>
        /// Represents JobProcessingOptions in Xml format
        /// </summary>
        /// <returns>String representation of current JobProcessingOptions object instance</returns>
        public String ToXml(ObjectSerialization serialization)
        {
            String xml = String.Empty;

            if (serialization == ObjectSerialization.Full)
            {
                xml = this.ToXml();
            }
            else
            {
                StringWriter sw = new StringWriter();
                XmlTextWriter xmlWriter = new XmlTextWriter(sw);

                //Attribute node start
                xmlWriter.WriteStartElement("JobProcessingOptions");

                if (serialization == ObjectSerialization.ProcessingOnly)
                {
                    xmlWriter.WriteAttributeString("NumberofEntityThreads", this.NumberofEntityThreads.ToString());
                    xmlWriter.WriteAttributeString("NumberofAttributeThreadsPerEntityThread", this.NumberofAttributeThreadsPerEntityThread.ToString());
                    xmlWriter.WriteAttributeString("BatchSize", this.BatchSize.ToString());

                    xmlWriter.WriteAttributeString("AbortThreasholdEntityCount", this.AbortThreasholdEntityCount.ToString());
                    xmlWriter.WriteAttributeString("AbortThreasholdErrorCount", this.AbortThreasholdErrorCount.ToString());

                    xmlWriter.WriteAttributeString("SaveSuccessEntitiesResult", this.SaveSuccessEntitiesResult.ToString().ToLower());
                    xmlWriter.WriteAttributeString("ValidateSchema", this.ValidateSchema.ToString().ToLower());
                    xmlWriter.WriteAttributeString("ExecuteDenormProcess", this.ExecuteDenormProcess.ToString().ToLower());

                    xmlWriter.WriteAttributeString("AttributeValidationLevel", this.AttributeValidationLevel.ToString());
                    xmlWriter.WriteAttributeString("RelationshipTypeValidationLevel", this.RelationshipTypeValidationLevel.ToString());
                    xmlWriter.WriteAttributeString("RelationshipAttributeValidationLevel", this.RelationshipAttributeValidationLevel.ToString());
                    xmlWriter.WriteAttributeString("PopulateLookupRefIdForInitialLoad", this.PopulateLookupRefIdForInitialLoad.ToString().ToLower());
                    xmlWriter.WriteAttributeString("CleanseInternalIdsFromSourceData", this.CleanseInternalIdsFromSourceData.ToString().ToLower());
                }

                if (serialization == ObjectSerialization.UIRender)
                {
                    xmlWriter.WriteAttributeString("NumberofEntityThreads", this.NumberofEntityThreads.ToString());
                    xmlWriter.WriteAttributeString("NumberofAttributeThreadsPerEntityThread", this.NumberofAttributeThreadsPerEntityThread.ToString());
                    xmlWriter.WriteAttributeString("BatchSize", this.BatchSize.ToString());

                    xmlWriter.WriteAttributeString("AbortThreasholdEntityCount", this.AbortThreasholdEntityCount.ToString());
                    xmlWriter.WriteAttributeString("AbortThreasholdErrorCount", this.AbortThreasholdErrorCount.ToString());

                    xmlWriter.WriteAttributeString("SaveSuccessEntitiesResult", this.SaveSuccessEntitiesResult.ToString().ToLower());
                    xmlWriter.WriteAttributeString("ValidateSchema", this.ValidateSchema.ToString().ToLower());
                    xmlWriter.WriteAttributeString("ExecuteDenormProcess", this.ExecuteDenormProcess.ToString().ToLower());

                    xmlWriter.WriteAttributeString("AttributeValidationLevel", this.AttributeValidationLevel.ToString());
                    xmlWriter.WriteAttributeString("RelationshipTypeValidationLevel", this.RelationshipTypeValidationLevel.ToString());
                    xmlWriter.WriteAttributeString("RelationshipAttributeValidationLevel", this.RelationshipAttributeValidationLevel.ToString());
                    xmlWriter.WriteAttributeString("PopulateLookupRefIdForInitialLoad", this.PopulateLookupRefIdForInitialLoad.ToString().ToLower());
                    xmlWriter.WriteAttributeString("CleanseInternalIdsFromSourceData", this.CleanseInternalIdsFromSourceData.ToString().ToLower());
                }

                //JobProcessingOptions end node
                xmlWriter.WriteEndElement();

                xmlWriter.Flush();
                //get the actual XML
                xml = sw.ToString();

                xmlWriter.Close();
                sw.Close();
            }
            return xml;
        }

        #endregion Public methods

        #region Private methods

        private void LoadJobProcessingOptions(String valuesAsXml)
        { 
            #region Sample Xml
            //<JobProcessingOptions ValidateEntities="true" PublishEvents="true" ProcessOnlyEntities="false" ProcessDefaultValues="true" CollectionProcessingType="Replace" AttributeValidationLevel="Warn" RelationshipTypeValidationLevel="Warn" RelationshipAttributeValidationLevel="Warn" PopulateLookupRefIdForInitialLoad = "true"/>
            #endregion Sample Xml

            XmlTextReader reader = null;
            try
            {
                reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                while (!reader.EOF)
                {
                    if (reader.NodeType == XmlNodeType.Element && reader.Name == "JobProcessingOptions")
                    {
                        #region Read JobProcessingOptions Properties

                        if (reader.HasAttributes)
                        {
                            if (reader.MoveToAttribute("NumberofEntityThreads"))
                            {
                                this.NumberofEntityThreads = Convert.ToInt32(reader.ReadContentAsString()); ;
                            }

                            if (reader.MoveToAttribute("NumberofAttributeThreadsPerEntityThread"))
                            {
                                this.NumberofAttributeThreadsPerEntityThread = Convert.ToInt32(reader.ReadContentAsString()); ;
                            }

                            if (reader.MoveToAttribute("BatchSize"))
                            {
                                this.BatchSize = Convert.ToInt32(reader.ReadContentAsString()); ;
                            }

                            if (reader.MoveToAttribute("AbortThreasholdEntityCount"))
                            {
                                this.AbortThreasholdEntityCount = Convert.ToInt32(reader.ReadContentAsString()); ;
                            }

                            if (reader.MoveToAttribute("AbortThreasholdErrorCount"))
                            {
                                this.AbortThreasholdErrorCount = Convert.ToInt32(reader.ReadContentAsString()); ;
                            }

                            if (reader.MoveToAttribute("SaveSuccessEntitiesResult"))
                            {
                                this.SaveSuccessEntitiesResult = reader.ReadContentAsBoolean();
                            }

                            if (reader.MoveToAttribute("ValidateSchema"))
                            {
                                this.ValidateSchema = reader.ReadContentAsBoolean();
                            }

                            if (reader.MoveToAttribute("ExecuteDenormProcess"))
                            {
                                this.ExecuteDenormProcess = reader.ReadContentAsBoolean();
                            }

                            if (reader.MoveToAttribute("AttributeValidationLevel"))
                            {
                                Enum.TryParse<OperationResultType>(reader.ReadContentAsString(), out  _attributeValidationLevel);
                                this.AttributeValidationLevel = _attributeValidationLevel;
                            }

                            if (reader.MoveToAttribute("RelationshipTypeValidationLevel"))
                            {
                                Enum.TryParse<OperationResultType>(reader.ReadContentAsString(), out  _relationshipTypeValidationLevel);
                                this.RelationshipTypeValidationLevel = _relationshipTypeValidationLevel;
                            }

                            if (reader.MoveToAttribute("RelationshipAttributeValidationLevel"))
                            {
                                Enum.TryParse<OperationResultType>(reader.ReadContentAsString(), out  _relationshipAttributeValidationLevel);
                                this.RelationshipAttributeValidationLevel = _relationshipAttributeValidationLevel;
                            }

                            if (reader.MoveToAttribute("PopulateLookupRefIdForInitialLoad"))
                            {
                                this.PopulateLookupRefIdForInitialLoad = reader.ReadContentAsBoolean();
                            }

                            if (reader.MoveToAttribute("CleanseInternalIdsFromSourceData"))
                            {
                                this.CleanseInternalIdsFromSourceData = reader.ReadContentAsBoolean();
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

        #endregion Private methods

        #endregion Methods

    }
}
