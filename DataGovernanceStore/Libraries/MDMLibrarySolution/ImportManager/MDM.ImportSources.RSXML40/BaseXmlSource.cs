using System;
using System.Xml;
using System.Xml.Schema;
using System.Data;

namespace MDM.ImportSources.RSXml
{
    using MDM.BusinessObjects;
    using MDM.Core;
    using MDM.Imports.Interfaces;
    using MDM.BusinessObjects.Jobs;
    using MDM.BusinessObjects.Imports;
    using System.IO;
    using MDM.Interfaces;
    using MDM.BusinessObjects.Diagnostics;
    using MDM.Utility;

    /// <summary>
    /// This class implements the source data from a RS XML source. It has one forward only reader.
    /// </summary>
    public class BaseXmlSource
    {
        /// <summary>
        /// Single forward only reader needs to be synchronized.
        /// </summary>
        private Object readerLock = new Object();

        private XmlReader rsxmlReader = null;

        ImportProviderBatchingType _batchingType = ImportProviderBatchingType.Single;

        string _sourceFile = string.Empty;

        string _schemaFilePath = String.Empty;

        int errorCount = 0;

        int warningCount = 0;

        string errorMessage = string.Empty;

        Int32 _batchSize = 0;

        /// <summary>
        /// Indicates current trace settings
        /// </summary>
        private TraceSettings _traceSettings = new TraceSettings();

        public BaseXmlSource()
        {
            _traceSettings = MDMOperationContextHelper.GetCurrentTraceSettings(); 
        }

        public BaseXmlSource(string filePath)
        {
            _sourceFile = filePath;

            // Set the global trace flag
            _traceSettings = MDMOperationContextHelper.GetCurrentTraceSettings(); 
        }

        #region Public Properties

        public ImportProviderBatchingType BatchingType
        {
            get { return _batchingType; }
            set { _batchingType = value; }
        }   
   
        #endregion

        #region IImportSourceData Methods For Entity

        /// <summary>
        /// Provide an opputunity for the source data to initializes itself with some configuration from the job.
        /// </summary>
        /// <param name="job"></param>
        /// <param name="importProfile"></param>
        /// <returns></returns>
        public Boolean Initialize(Job job, ImportProfile importProfile)
        {
            string message = string.Empty;
            DiagnosticActivity activity = new DiagnosticActivity();
            if (_traceSettings.IsBasicTracingEnabled) activity.Start();

            if (string.IsNullOrEmpty(_sourceFile))
            {
                throw new ArgumentNullException("Rs XML file is not available");
            }

            // If validation is required...
            ExecutionStep coreStep = null;
            foreach (ExecutionStep executionStep in importProfile.ExecutionSteps)
            {
                if (executionStep.StepType == ExecutionStepType.Core)
                {
                    coreStep = executionStep;
                }
            }

            if (importProfile.ProcessingSpecifications.JobProcessingOptions.ValidateSchema)
            {
                JobParameter schemaFilePathParameter = job.JobData.JobParameters["SchemaFilePath"];

                if (schemaFilePathParameter == null)
                {
                    throw new ArgumentNullException("Rs XML Schema file path parameter is not set by the import engine.");
                }

                _schemaFilePath = schemaFilePathParameter.Value;

                if (String.IsNullOrEmpty(_schemaFilePath))
                {
                    throw new ArgumentNullException("Rs XML Schema file path parameter set by import engine is empty.");
                }

                FileInfo fileInfo = new FileInfo(_schemaFilePath);

                if (!fileInfo.Exists)
                {
                    throw new ArgumentNullException("Rs XML Schema file path parameter set by import engine is not valid.");
                }

                if (!ValidateSchema(_sourceFile))
                    return false;
            }

            if (System.IO.File.Exists(_sourceFile))
            {
                rsxmlReader = XmlReader.Create(_sourceFile);
            }
            else
            {
                throw new ArgumentException(String.Format("Rs XML file is not available in the specified location {0}", _sourceFile));
            }
            
            if (_traceSettings.IsBasicTracingEnabled) activity.Stop();

            return true;
        }

        public Int64 GetEntityDataCount()
        {
            return 0;
        }

        public Int64 GetEntityDataSeed()
        {
            return 0;
        }

        public Int64 GetEntityEndPoint()
        {
            return 0;
        }

        /// <summary>
        /// Indicates the batching mode the proiver supports.
        /// </summary>
        /// <returns></returns>
        public ImportProviderBatchingType GetBatchingType()
        {
            return BatchingType;
        }

        public Int32 GetEntityDataBatchSize()
        {
            return _batchSize;
        }

        public BusinessObjects.EntityCollection GetAllEntityData(MDMCenterApplication application, MDMCenterModules module, IEntityProviderContext entityProviderContext)
        {
            return null;
        }

        public BusinessObjects.EntityCollection GetEntityDataBatch(Int64 startPK, Int64 endPK, MDMCenterApplication application, MDMCenterModules module, IEntityProviderContext entityProviderContext)
        {
            return null;
        }

        /// <summary>
        /// Gets the next available batch of data for processing.
        /// </summary>
        /// <param name="batchSize"></param>
        /// <param name="application"></param>
        /// <param name="module"></param>
        /// <returns></returns>
        public BusinessObjects.EntityCollection GetEntityDataNextBatch(Int32 batchSize, MDMCenterApplication application, MDMCenterModules module, IEntityProviderContext entityProviderContext)
        {
            string message = string.Empty;
            DiagnosticActivity activity = new DiagnosticActivity();
            if (_traceSettings.IsBasicTracingEnabled) activity.Start();

            EntityCollection entityCollection = new EntityCollection();
            Int32 numberOfEntities = 0;
            // if the reader available?
            if (rsxmlReader == null)
            {
                throw new ArgumentException(String.Format("RS XML reader is not available for processing. The file is : {0}", _sourceFile));
            }
            // only one thread can read the contents at a time...
            lock (readerLock)
            {
                while ((rsxmlReader.ReadState == ReadState.Initial || rsxmlReader.ReadState == ReadState.Interactive) && (numberOfEntities < batchSize))
                {
                    if (rsxmlReader.NodeType == XmlNodeType.Element && rsxmlReader.Name == "Entity")
                    {
                        String entityXml = rsxmlReader.ReadOuterXml();
                        if (!String.IsNullOrEmpty(entityXml))
                        {
                            Entity entity = new Entity(entityXml, ObjectSerialization.External);
                            if (entity != null)
                            {
                                Boolean successFlag = Importutils.AddEntityMatchingProviderContext(entityCollection, entity, (EntityProviderContext)entityProviderContext);

                                if (successFlag)
                                {
                                    numberOfEntities++;
                                }
                            }
                        }
                    }
                    else
                    {
                        rsxmlReader.Read();
                    }
                }
                // if we reached the end of the file..close it
                if (rsxmlReader.EOF)
                    rsxmlReader.Close();
            }
            
            if (_traceSettings.IsBasicTracingEnabled) activity.Stop();

            return entityCollection;
        }

        public BusinessObjects.AttributeCollection GetAttributeDataforEntities(Core.AttributeModelType attributeType, BusinessObjects.EntityCollection entityCollection, MDMCenterApplication application, MDMCenterModules module)
        {
            return null;
        }

        public BusinessObjects.AttributeCollection GetAttributeDataforEntityList(Core.AttributeModelType attributeType, string entityList, BusinessObjects.EntityCollection entityCollection, MDMCenterApplication application, MDMCenterModules module)
        {
            return null;
        }

        public bool UpdateErrorEntities(BusinessObjects.EntityOperationResultCollection errorEntities, MDMCenterApplication application, MDMCenterModules module)
        {
            return true;
        }

        public bool UpdateErrorRelationships(BusinessObjects.RelationshipOperationResultCollection errorRelationships, MDMCenterApplication application, MDMCenterModules module)
        {
            return true;
        }

        public bool UpdateErrorAttributes(BusinessObjects.AttributeOperationResultCollection errorAttributes, MDMCenterApplication application, MDMCenterModules module)
        {
            return true;
        }

        public bool UpdateErrorAttributes(Core.AttributeModelType attributeType, string entityList, string errorMessage, MDMCenterApplication application, MDMCenterModules module)
        {
            return true;
        }

        public bool UpdateSuccessEntities(BusinessObjects.EntityCollection entities, MDMCenterApplication application, MDMCenterModules module)
        {
            return true;
        }

        public bool UpdateSuccessAttributes(Core.AttributeModelType attributeType, BusinessObjects.EntityCollection entities, MDMCenterApplication application, MDMCenterModules module)
        {
            return true;
        }

        /// <summary>
        /// Get input specification.
        /// This is mostly used on UI for profile management. It will get list of entity metadata and attributes, which can be used later for changing attribute mapping.
        /// </summary>
        /// <param name="entityCountToRead">No. of entities to read from sample.</param>
        /// <param name="callerContext">Indicates who called this method</param>
        /// <returns>DataSet representing schema available in excel.</returns>
        public DataSet GetInputSpecification(Int32 entityCountToRead, CallerContext callerContext)
        {
            string message = string.Empty;
            DiagnosticActivity activity = new DiagnosticActivity();
            if (_traceSettings.IsBasicTracingEnabled) activity.Start();

            Int32 sampleEntityThreshold = 20; //TODO :: Do we need this to be configurable??

            #region Validation

            if(entityCountToRead < 1)
            {
                throw new Exception("Sample entity count cannot be less than 1");
            }

            //Check entityCountToRead is not more than threshold
            //If it is more, throw error
            if(entityCountToRead > sampleEntityThreshold)
            {
                throw new Exception("Sample file is having more entities than sample entity threshold count");
            }

            #endregion Validation

            DataSet ds = new DataSet();
            DataTable dtEntity = new DataTable();

            #region Create entity metadata columns in Data table

            dtEntity.Columns.Add("Id");
            dtEntity.Columns.Add("ExternalId");
            dtEntity.Columns.Add("Name");
            dtEntity.Columns.Add("LongName");
            dtEntity.Columns.Add("ParentExternalId");
            dtEntity.Columns.Add("ParentEntityName");
            dtEntity.Columns.Add("ParentEntityLongName");
            dtEntity.Columns.Add("CategoryPath");
            dtEntity.Columns.Add("CategoryName");
            dtEntity.Columns.Add("CategoryLongName");
            dtEntity.Columns.Add("ContainerName");
            dtEntity.Columns.Add("EntityTypeName");
            dtEntity.Columns.Add("Action");
            dtEntity.Columns.Add("OrganizationName");
            dtEntity.Columns.Add("Locale");

            #endregion Create entity metadata columns in Data table

            if(System.IO.File.Exists(_sourceFile))
            {
                rsxmlReader = XmlReader.Create(_sourceFile);
            }
            else
            {
                throw new ArgumentException(String.Format("Rs XML file is not available in the specified location {0}", _sourceFile));
            }

            EntityProviderContext entityproviderContext = new EntityProviderContext();

            entityproviderContext.EntityProviderContextType = EntityProviderContextType.All;

            EntityCollection entities = this.GetEntityDataNextBatch(entityCountToRead, callerContext.Application, callerContext.Module, entityproviderContext);

            if(rsxmlReader.ReadState != ReadState.Closed)
            {
                rsxmlReader.Close();
            }

            if(entities != null)
            {
                #region Get all attributes from entity and create column in data table

                foreach(Entity entity in entities)
                {
                    foreach(Attribute attribute in entity.Attributes)
                    {
                        String columnName = String.Concat(attribute.AttributeParentName, "//", attribute.Name, "//", attribute.Locale);
                        if(!dtEntity.Columns.Contains(columnName))
                        {
                            dtEntity.Columns.Add(columnName);
                        }
                    }

                    entityCountToRead--;

                    if(entityCountToRead < 1)
                    {
                        break;
                    }
                }
                #endregion Get all attributes from entity and create column in data table
            }

            ds.Tables.Add(dtEntity);
            
            if (_traceSettings.IsBasicTracingEnabled) activity.Stop();

            return ds;
        }

        #endregion

        #region IImportSourceData Methods For Relationship

        public Int64 GetRelationshipDataCount()
        {
            return 0;
        }

        public Int64 GetRelationshipDataSeed()
        {
            return 0;
        }

        public Int64 GetRelationshipEndPoint()
        {
            return 0;
        }

        public Int32 GetRelationshipDataBatchSize()
        {
            return _batchSize;
        }

        public BusinessObjects.EntityCollection GetAllRelationshipData(MDMCenterApplication application, MDMCenterModules module)
        {
            throw new NotImplementedException();
        }

        public BusinessObjects.EntityCollection GetRelationshipDataBatch(Int64 startPK, Int64 endPK, MDMCenterApplication application, MDMCenterModules module)
        {
            throw new NotImplementedException();
        }

        public BusinessObjects.EntityCollection GetRelationshipDataNextBatch(Int32 batchSize, MDMCenterApplication application, MDMCenterModules module)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Validation Methods

        private bool ValidateSchema(String filePath)
        {
            string message = string.Empty;
            DiagnosticActivity activity = new DiagnosticActivity();
            if (_traceSettings.IsBasicTracingEnabled) activity.Start();

            XmlReaderSettings settings = new XmlReaderSettings();
            settings.Schemas.Add("http://schemas.riversand.com/mdmcenter", _schemaFilePath);
            settings.Schemas.Compile();
            settings.ValidationType = ValidationType.Schema;
            settings.ValidationFlags |= XmlSchemaValidationFlags.ReportValidationWarnings;
            settings.ValidationEventHandler += new System.Xml.Schema.ValidationEventHandler(rsXML_ValidationEventHandler);

            //Create the schema validating reader.
            XmlReader schemaReader = null;
            try
            {
                schemaReader = XmlReader.Create(filePath, settings);
            }
            catch
            {
                throw;
            }

            // just read through the document...
            while (schemaReader.Read()) { }

            //Close the reader.
            schemaReader.Close();

            if (warningCount > 0)
            {
                // the schema information is missing.
                throw new Exception(String.Format("Schema validation was requested but the RSXML file {0} does not have the schema header information.", filePath));
            }

            if (errorCount > 0)
            {
                // the schema information is missing.
                throw new Exception(String.Format("The RSXML file {0} failed schema validation. The errors are {1}.", filePath, errorMessage));
            }
            
            if (_traceSettings.IsBasicTracingEnabled) activity.Stop();

            return true;
        }

        public void rsXML_ValidationEventHandler(object sender, ValidationEventArgs args)
        {
            if (args.Severity == XmlSeverityType.Warning)
            {
                warningCount++;
            }
            else
            {
                errorMessage = errorMessage + args.Message + "\r\n";
                errorCount++;
            }
        }

        #endregion

        #region IImportSourceData methods for Extension Relationships
        /// <summary>
        /// Gets the parent extension relationship for a given entity
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="application"></param>
        /// <param name="module"></param>
        /// <returns></returns>
        public ExtensionRelationship GetParentExtensionRelationShip(Entity entity, MDMCenterApplication application, MDMCenterModules module)
        {
            return null;
        }
        #endregion

        #region Mapping Related Interface Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inputFieldName"></param>
        /// <returns></returns>
        public Attribute GetAttributeInfoFromInputFieldName(String inputFieldName)
        {
            Attribute attr = null;

            //KEYNOTE: HERE input field format normally comes as AttributeGroupName//AttributeName//Locale
            char[] sep = { '/', '/' };

            String[] fieldComponents = inputFieldName.Split(sep);

            if (fieldComponents != null && fieldComponents.Length == 5)
            {
                attr = new Attribute();

                attr.Id = 0;
                attr.AttributeParentName = fieldComponents[0];
                attr.Name = fieldComponents[2];

                String strLocale = fieldComponents[4];
                LocaleEnum locale = LocaleEnum.UnKnown;
                Enum.TryParse<LocaleEnum>(strLocale, out locale);
                attr.Locale = locale;
            }

            return attr;
        }

        #endregion
    }
}
