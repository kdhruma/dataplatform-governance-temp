using System;
using System.Xml;
using System.Data;
using System.Xml.Schema;

namespace MDM.ImportSources.RSXliff
{
    using MDM.BusinessObjects;
    using MDM.BusinessObjects.Imports;
    using MDM.BusinessObjects.Jobs;
    using MDM.Core;
    using MDM.Imports.Interfaces;
    using MDM.Interfaces;
    using MDM.TranslationUtility;
    using System.IO;
    

    /// <summary>
    /// This class implements the source data from a RSXliff source.
    /// </summary>
    public class BaseXliffSource
    {
        #region Fields

        private Object readerLock = new Object();

        private XmlReader rsXliffReader = null;

        ImportProviderBatchingType _batchingType = ImportProviderBatchingType.Single;

        String _sourceFile = String.Empty;

        String _schemaFilePath = String.Empty;

        String errorMessage = String.Empty;

        Int32 errorCount = 0;

        Int32 warningCount = 0;

        Int32 _batchSize = 0;

        #endregion

        #region Constructor

        /// <summary>
        /// Base Xliff source parameter less constructor
        /// </summary>
        public BaseXliffSource()
        {

        }

        /// <summary>
        /// Base Xliff source constructor having filePath as the parameter
        /// </summary>
        /// <param name="filePath"></param>
        public BaseXliffSource(String filePath)
        {
            _sourceFile = filePath;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Indicates the batching mode the provider supports
        /// </summary>
        public ImportProviderBatchingType BatchingType
        {
            get { return _batchingType; }
            set { _batchingType = value; }
        }   
   
        #endregion

        #region IImportSourceData Methods For Entity

        /// <summary>
        /// Provide an opportunity for the source data to initialize itself with some configuration from the job.
        /// </summary>
        /// <param name="job"></param>
        /// <param name="importProfile"></param>
        /// <returns></returns>
        public Boolean Initialize(Job job, ImportProfile importProfile)
        {
            if (string.IsNullOrEmpty(_sourceFile))
            {
                throw new ArgumentNullException("RSXliff file is not available");
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
                    throw new ArgumentNullException("Rs Xliff Schema file path parameter is not set by the import engine.");
                }

                _schemaFilePath = schemaFilePathParameter.Value;

                if (String.IsNullOrEmpty(_schemaFilePath))
                {
                    throw new ArgumentNullException("Rs Xliff Schema file path parameter set by import engine is empty.");
                }

                FileInfo fileInfo = new FileInfo(_schemaFilePath);

                if (!fileInfo.Exists)
                {
                    throw new ArgumentNullException("Rs Xliff Schema file path parameter set by import engine is not valid.");
                }

                if (!ValidateSchema(_sourceFile))
                    return false;
            }

            if (System.IO.File.Exists(_sourceFile))
            {
                rsXliffReader = XmlReader.Create(_sourceFile);
            }
            else
            {
                throw new ArgumentException(String.Format("RSXliff file is not available in the specified location {0}", _sourceFile));
            }

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Int64 GetEntityDataCount()
        {
            return 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Int64 GetEntityDataSeed()
        {
            return 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Int64 GetEntityEndPoint()
        {
            return 0;
        }

        /// <summary>
        /// Indicates the batching mode the provider supports.
        /// </summary>
        /// <returns>Returns ImportProviderBatchingType</returns>
        public ImportProviderBatchingType GetBatchingType()
        {
            return BatchingType;
        }

        /// <summary>
        /// Gets Entity Batch Size to be processed
        /// </summary>
        /// <returns></returns>
        public Int32 GetEntityDataBatchSize()
        {
            return _batchSize;
        }

        /// <summary>
        /// Gets Entity Data
        /// </summary>
        /// <param name="application"></param>
        /// <param name="module"></param>
        /// <param name="entityProviderContext"></param>
        /// <returns>EntityCollection</returns>
        public BusinessObjects.EntityCollection GetAllEntityData(MDMCenterApplication application, MDMCenterModules module, IEntityProviderContext entityProviderContext)
        {
            return null;
        }

        /// <summary>
        /// Gets Entity data batch for processing
        /// </summary>
        /// <param name="startPK"></param>
        /// <param name="endPK"></param>
        /// <param name="application"></param>        
        /// <param name="module"></param>
        /// <param name="entityProviderContext"></param>
        /// <returns></returns>
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
        /// <param name="entityProviderContext"></param>
        /// <returns></returns>
        public BusinessObjects.EntityCollection GetEntityDataNextBatch(Int32 batchSize, MDMCenterApplication application, MDMCenterModules module, IEntityProviderContext entityProviderContext)
        {
            EntityCollection entityCollection = new EntityCollection();

            Int32 numberOfEntities = 0;

            // if the reader available?
            if (rsXliffReader == null)
            {
                throw new ArgumentException(String.Format("RSXliff reader is not available for processing. The file is : {0}", _sourceFile));
            }

            // only one thread can read the contents at a time...
            lock (readerLock)
            {
                while ((rsXliffReader.ReadState == ReadState.Initial || rsXliffReader.ReadState == ReadState.Interactive) && (numberOfEntities < batchSize))
                {
                    if (rsXliffReader.NodeType == XmlNodeType.Element && rsXliffReader.Name == "file")
                    {
                        String entityXliff = rsXliffReader.ReadOuterXml();

                        if (!String.IsNullOrWhiteSpace(entityXliff))
                        {
                            Entity entity = TranslationHelper.ConvertRSXliff1_0ToEntity(entityXliff);                          
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
                        rsXliffReader.Read();
                    }
                }

                // if we reached the end of the file..close it
                if (rsXliffReader.EOF)
                {
                    rsXliffReader.Close();
                }
            }

            return entityCollection;
        }

        /// <summary>
        /// Returns the collection of attribute data based on Attribute type and entity collection
        /// </summary>
        /// <param name="attributeType"></param>
        /// <param name="entityCollection"></param>
        /// <param name="application"></param>
        /// <param name="module"></param>
        /// <returns>AttributeCollection</returns>
        public BusinessObjects.AttributeCollection GetAttributeDataforEntities(Core.AttributeModelType attributeType, BusinessObjects.EntityCollection entityCollection, MDMCenterApplication application, MDMCenterModules module)
        {
            return null;
        }

        /// <summary>
        /// Gets Attribute collection for Entites based on attribute type
        /// </summary>
        /// <param name="attributeType"></param>
        /// <param name="entityList"></param>
        /// <param name="entityCollection"></param>
        /// <param name="application"></param>
        /// <param name="module"></param>
        /// <returns></returns>
        public BusinessObjects.AttributeCollection GetAttributeDataforEntityList(Core.AttributeModelType attributeType, string entityList, BusinessObjects.EntityCollection entityCollection, MDMCenterApplication application, MDMCenterModules module)
        {
            return null;
        }

        /// <summary>
        /// Update collection of entities which generated error while processing
        /// </summary>
        /// <param name="errorEntities"></param>
        /// <param name="application"></param>
        /// <param name="module"></param>
        /// <returns></returns>
        public bool UpdateErrorEntities(BusinessObjects.EntityOperationResultCollection errorEntities, MDMCenterApplication application, MDMCenterModules module)
        {
            return true;
        }

        /// <summary>
        /// Update collection of relationships which generated error while processing
        /// </summary>
        /// <param name="errorRelationships"></param>
        /// <param name="application"></param>
        /// <param name="module"></param>
        /// <returns></returns>
        public bool UpdateErrorRelationships(BusinessObjects.RelationshipOperationResultCollection errorRelationships, MDMCenterApplication application, MDMCenterModules module)
        {
            return true;
        }

        /// <summary>
        /// Update collection of attributes which generated error while processing
        /// </summary>
        /// <param name="errorAttributes"></param>
        /// <param name="application"></param>
        /// <param name="module"></param>
        /// <returns></returns>
        public bool UpdateErrorAttributes(BusinessObjects.AttributeOperationResultCollection errorAttributes, MDMCenterApplication application, MDMCenterModules module)
        {
            return true;
        }

        /// <summary>
        /// Update collection of attributes which generated error while processing based on Entity collection and attribute type
        /// </summary>
        /// <param name="attributeType"></param>
        /// <param name="entityList"></param>
        /// <param name="errorMessage"></param>
        /// <param name="application"></param>
        /// <param name="module"></param>
        /// <returns></returns>
        public bool UpdateErrorAttributes(Core.AttributeModelType attributeType, string entityList, string errorMessage, MDMCenterApplication application, MDMCenterModules module)
        {
            return true;
        }

        /// <summary>
        /// Update the collection of entities which got successfully processed
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="application"></param>
        /// <param name="module"></param>
        /// <returns></returns>
        public bool UpdateSuccessEntities(BusinessObjects.EntityCollection entities, MDMCenterApplication application, MDMCenterModules module)
        {
            return true;
        }

        /// <summary>
        /// Update the collection of attributes which got successfully processed
        /// </summary>
        /// <param name="attributeType"></param>
        /// <param name="entities"></param>
        /// <param name="application"></param>
        /// <param name="module"></param>
        /// <returns></returns>
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
            DataSet ds = new DataSet();
          
            return ds;
        }

        #endregion

        #region IImportSourceData Methods For Relationship

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Int64 GetRelationshipDataCount()
        {
            return 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Int64 GetRelationshipDataSeed()
        {
            return 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Int64 GetRelationshipEndPoint()
        {
            return 0;
        }

        /// <summary>
        /// Returns the size of batch to be processed
        /// </summary>
        /// <returns></returns>
        public Int32 GetRelationshipDataBatchSize()
        {
            return _batchSize;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="application"></param>
        /// <param name="module"></param>
        /// <returns></returns>
        public BusinessObjects.EntityCollection GetAllRelationshipData(MDMCenterApplication application, MDMCenterModules module)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="startPK"></param>
        /// <param name="endPK"></param>
        /// <param name="application"></param>
        /// <param name="module"></param>
        /// <returns></returns>
        public BusinessObjects.EntityCollection GetRelationshipDataBatch(Int64 startPK, Int64 endPK, MDMCenterApplication application, MDMCenterModules module)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///  Gets the next available batch of relationship data for processing. 
        /// </summary>
        /// <param name="batchSize"></param>
        /// <param name="application"></param>
        /// <param name="module"></param>
        /// <returns></returns>
        public BusinessObjects.EntityCollection GetRelationshipDataNextBatch(Int32 batchSize, MDMCenterApplication application, MDMCenterModules module)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Validation Methods

        private bool ValidateSchema(String filePath)
        {
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.Schemas.Add("http://www.riversand.com/schemas", _schemaFilePath);
            settings.Schemas.Compile();
            settings.ValidationType = ValidationType.Schema;
            settings.ValidationFlags |= XmlSchemaValidationFlags.ReportValidationWarnings;
            settings.ValidationEventHandler += new System.Xml.Schema.ValidationEventHandler(rsXliff_ValidationEventHandler);

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
                throw new Exception(String.Format("Schema validation was requested but the RSXliff file {0} does not have the schema header information.", filePath));
            }

            if (errorCount > 0)
            {
                // the schema information is missing.
                throw new Exception(String.Format("The RSXliff file {0} failed schema validation. The errors are {1}.", filePath, errorMessage));
            }
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        public void rsXliff_ValidationEventHandler(object sender, ValidationEventArgs args)
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
