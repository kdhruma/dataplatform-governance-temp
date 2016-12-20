using System;
using System.Xml;
using System.Xml.Schema;
using System.Linq;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using MDM.CachedDataModelManager;
using MDM.Core.Extensions;


namespace MDM.ImportSources.RSExcel
{
    using MDM.BusinessObjects;
    using MDM.Core;
    using MDM.Imports.Interfaces;
    using MDM.BusinessObjects.Jobs;
    using MDM.BusinessObjects.Imports;
    using MDM.Interfaces;
    using MDM.Core.Exceptions;
    using MDM.ExcelUtility;
    using DocumentFormat.OpenXml.Spreadsheet;
    using DocumentFormat.OpenXml.Packaging;
    using MDM.Utility;
    using System.Reflection;
    using MDM.BusinessObjects.Diagnostics;

    /// <summary>
    /// This class implements the source data from a RS Excel source. It reads the data in to a data set and process them as per the request from the import engine.
    /// </summary>
    public class ExcelSource12 : IEntityImportSourceData
    {
        #region Fields

        private IJobResultHandler _jobResultHandler = null;

        /// <summary>
        /// Is SAXPaeaser Enable or not
        /// </summary>
        private bool _isSAXParserEnabled = false;

        /// <summary>
        /// Single forward only reader needs to be synchronized.
        /// </summary>
        private Object readerLock = new Object();

        protected ImportProviderBatchingType _batchingType = ImportProviderBatchingType.Single;

        protected String _sourceFile = string.Empty;
        protected Int32 _batchSize = 0;
        protected Int32 _currentReadCount = 0;
        protected Boolean _isInitialized = false;

        protected LocaleEnum _defaultLocale = LocaleEnum.UnKnown;

        protected DataTable _dtEntities = new DataTable();
        protected DataTable _dtRelationships = new DataTable();
        protected DataTable _dtComplexAttribues = new DataTable();

        protected List<Entity> _entityList = new List<Entity>();

        protected Collection<String> _entityAttributesList = new Collection<String>();
        protected Collection<String> _relationshipAttributesList = new Collection<String>();
        protected Collection<String> _complexChildAttributeList = new Collection<String>();
        protected Dictionary<String, String> _complexAttributes = new Dictionary<String, String>();

        /// <summary>
        /// Indicates current trace settings
        /// </summary>
        private TraceSettings _traceSettings = new TraceSettings();

        private ICachedDataModel CachedAttributeDataModel
        {
            get;
            set;
        }

        #endregion

        #region Constructors

        public ExcelSource12(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                throw new ArgumentNullException("Rs Excel file is not available");
            }

            _sourceFile = filePath;

            _traceSettings = MDMOperationContextHelper.GetCurrentTraceSettings();

            CachedAttributeDataModel = CachedDataModel.GetSingleton();
        }

        #endregion

        #region Public Properties

        public ImportProviderBatchingType BatchingType
        {
            get { return _batchingType; }
            set { _batchingType = value; }
        }

        #endregion

        #region IEntityImportSourceData Properties
        public IJobResultHandler JobResultHandler
        {
            get
            {
                return _jobResultHandler;
            }
            set
            {
                _jobResultHandler = value;
            }
        }
        #endregion

        #region IImportSourceData Methods For Entity

        /// <summary>
        /// Provide an opportunity for the source data to initializes itself with some configuration from the job.
        /// </summary>
        /// <param name="job"></param>
        /// <param name="importProfile"></param>
        /// <returns></returns>
        public Boolean Initialize(Job job, ImportProfile importProfile)
        {
            DiagnosticActivity activity = new DiagnosticActivity();
            if (_traceSettings.IsBasicTracingEnabled) activity.Start();

            if (!_isInitialized)
            {
                #region Read Metadata Tab

                _isSAXParserEnabled = AppConfigurationHelper.GetAppConfig<bool>("MDMCenter.ExcelImports.SAXParser.Enabled", false);

                ReadMetadataTab();

                #endregion

                #region Set Profile level properties

                if (importProfile != null)
                {
                    //Set profile for the file level metadata settings(Examples: UOM Separator, DeleteKeyword, etc)
                    if (!String.IsNullOrWhiteSpace(RSExcelConstants.MetadataTemplateFields[MetadataTemplateFieldEnum.CollectionSeparator].Value))
                    {
                        importProfile.ProcessingSpecifications.KeywordProcessingOptions.CollectionDataSeparator = RSExcelConstants.MetadataTemplateFields[MetadataTemplateFieldEnum.CollectionSeparator].Value;
                    }
                    else
                    {
                        String collectionSeperator = AppConfigurationHelper.GetAppConfig<String>("MDM.Exports.RSExcelFormatter.CollectionSeparator");

                        if (!String.IsNullOrWhiteSpace(collectionSeperator))
                        {
                            importProfile.ProcessingSpecifications.KeywordProcessingOptions.CollectionDataSeparator = collectionSeperator;
                        }
                    }

                    if (!String.IsNullOrWhiteSpace(RSExcelConstants.MetadataTemplateFields[MetadataTemplateFieldEnum.UomSeparator].Value))
                    {
                        importProfile.ProcessingSpecifications.KeywordProcessingOptions.UomDataSeparator = RSExcelConstants.MetadataTemplateFields[MetadataTemplateFieldEnum.UomSeparator].Value;
                    }
                    else
                    {
                        String uomSeperator = AppConfigurationHelper.GetAppConfig<String>("MDM.Exports.RSExcelFormatter.UomSeparator");

                        if (!String.IsNullOrWhiteSpace(uomSeperator))
                        {
                            importProfile.ProcessingSpecifications.KeywordProcessingOptions.UomDataSeparator = uomSeperator;
                        }
                    }

                    if (!String.IsNullOrWhiteSpace(RSExcelConstants.MetadataTemplateFields[MetadataTemplateFieldEnum.DeleteKeyword].Value))
                    {
                        importProfile.ProcessingSpecifications.KeywordProcessingOptions.EnableDeleteKeyword = true;
                        importProfile.ProcessingSpecifications.KeywordProcessingOptions.DeleteKeyword = RSExcelConstants.MetadataTemplateFields[MetadataTemplateFieldEnum.DeleteKeyword].Value;
                    }

                    if (!String.IsNullOrWhiteSpace(RSExcelConstants.MetadataTemplateFields[MetadataTemplateFieldEnum.BlankKeyword].Value))
                    {
                        importProfile.ProcessingSpecifications.KeywordProcessingOptions.EnableBlankKeyword = true;
                        importProfile.ProcessingSpecifications.KeywordProcessingOptions.BlankKeyword = RSExcelConstants.MetadataTemplateFields[MetadataTemplateFieldEnum.BlankKeyword].Value;
                    }

                    string defaultLocale = RSExcelConstants.MetadataTemplateFields[MetadataTemplateFieldEnum.DefaultDataLocale].Value;
                    LocaleEnum locale = LocaleEnum.UnKnown;
                    Enum.TryParse<LocaleEnum>(defaultLocale, out locale);
                    this._defaultLocale = locale;

                    //set the profile level default locale reading from the file metadata...
                    if (importProfile.MappingSpecifications.LocaleMap.Mode == MappingMode.Implicit && _defaultLocale != LocaleEnum.UnKnown)
                    {
                        importProfile.MappingSpecifications.LocaleMap.Locale = _defaultLocale;
                    }
                }

                #endregion

                #region Read Entity Tabs

                ReadEntityDataTabs();

                #endregion

                this._isInitialized = true;
            }
            else
            {
                //This means Engine has reset the data load and calling the same file again. 
                //This is use case with ordered processing(Example: MDL Load Process where we need to procss same file again and again but in order or container path)
                // Here engine would call the intialize multiple time for different containers             

                //Just set the counter bact to 0, no need to read file again and create entity object in the above case
                this._currentReadCount = 0;
            }

            if (_traceSettings.IsBasicTracingEnabled) activity.Stop();

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Int64 GetEntityDataCount()
        {
            return _entityList == null ? 0 : _entityList.Count;
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
        /// Indicates the batching mode the proiver supports.
        /// </summary>
        /// <returns></returns>
        public ImportProviderBatchingType GetBatchingType()
        {
            return BatchingType;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Int32 GetEntityDataBatchSize()
        {
            return _batchSize;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="application"></param>
        /// <param name="module"></param>
        /// <param name="entityProviderContext"></param>
        /// <returns></returns>
        public EntityCollection GetAllEntityData(MDMCenterApplication application, MDMCenterModules module, IEntityProviderContext entityProviderContext)
        {
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="startPK"></param>
        /// <param name="endPK"></param>
        /// <param name="application"></param>
        /// <param name="module"></param>
        /// <param name="entityProviderContext"></param>
        /// <returns></returns>
        public EntityCollection GetEntityDataBatch(Int64 startPK, Int64 endPK, MDMCenterApplication application, MDMCenterModules module, IEntityProviderContext entityProviderContext)
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
        public EntityCollection GetEntityDataNextBatch(Int32 batchSize, MDMCenterApplication application, MDMCenterModules module, IEntityProviderContext entityProviderContext)
        {
            EntityCollection entityCollection = new EntityCollection();
            String containerName = String.Empty;

            lock (readerLock)
            {
                Int32 numberOfEntities = 0;

                while (numberOfEntities < batchSize && _currentReadCount < _entityList.Count)
                {
                    Entity entity = this._entityList[_currentReadCount++];

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

            return entityCollection;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="attributeType"></param>
        /// <param name="entityCollection"></param>
        /// <param name="application"></param>
        /// <param name="module"></param>
        /// <returns></returns>
        public AttributeCollection GetAttributeDataforEntities(Core.AttributeModelType attributeType, BusinessObjects.EntityCollection entityCollection, MDMCenterApplication application, MDMCenterModules module)
        {
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="attributeType"></param>
        /// <param name="entityList"></param>
        /// <param name="entityCollection"></param>
        /// <param name="application"></param>
        /// <param name="module"></param>
        /// <returns></returns>
        public AttributeCollection GetAttributeDataforEntityList(Core.AttributeModelType attributeType, string entityList, BusinessObjects.EntityCollection entityCollection, MDMCenterApplication application, MDMCenterModules module)
        {
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="errorEntities"></param>
        /// <param name="application"></param>
        /// <param name="module"></param>
        /// <returns></returns>
        public bool UpdateErrorEntities(EntityOperationResultCollection errorEntities, MDMCenterApplication application, MDMCenterModules module)
        {
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="errorRelationships"></param>
        /// <param name="application"></param>
        /// <param name="module"></param>
        /// <returns></returns>
        public bool UpdateErrorRelationships(RelationshipOperationResultCollection errorRelationships, MDMCenterApplication application, MDMCenterModules module)
        {
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="errorAttributes"></param>
        /// <param name="application"></param>
        /// <param name="module"></param>
        /// <returns></returns>
        public bool UpdateErrorAttributes(AttributeOperationResultCollection errorAttributes, MDMCenterApplication application, MDMCenterModules module)
        {
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="attributeType"></param>
        /// <param name="entityList"></param>
        /// <param name="errorMessage"></param>
        /// <param name="application"></param>
        /// <param name="module"></param>
        /// <returns></returns>
        public bool UpdateErrorAttributes(AttributeModelType attributeType, string entityList, string errorMessage, MDMCenterApplication application, MDMCenterModules module)
        {
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="application"></param>
        /// <param name="module"></param>
        /// <returns></returns>
        public bool UpdateSuccessEntities(EntityCollection entities, MDMCenterApplication application, MDMCenterModules module)
        {
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="attributeType"></param>
        /// <param name="entities"></param>
        /// <param name="application"></param>
        /// <param name="module"></param>
        /// <returns></returns>
        public bool UpdateSuccessAttributes(AttributeModelType attributeType, EntityCollection entities, MDMCenterApplication application, MDMCenterModules module)
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
            DiagnosticActivity activity = new DiagnosticActivity();
            if (_traceSettings.IsBasicTracingEnabled) activity.Start();

            DataSet ds = new DataSet();
            DataTable dt = new DataTable();

            //Read "Entity Metadata" sheet, "Entity" and "Relationships" sheet from excel and merge the columns in 1 data table

            //Both "Entity" and "Relationships" sheets are already read and stored in _dtEntities and _dtRelationships data tables. So here we need to read metadata only.
            #region Read "Entity Metadata" sheet from RsExcel workbook

            foreach (KeyValuePair<MetadataTemplateFieldEnum, KeyValuePair<String, String>> pair in RSExcelConstants.MetadataTemplateFields)
            {
                String colName = String.Concat("[", RSExcelConstants.MetadataSheetName, "].[", pair.Value.Key, "]");
                DataColumn dc = new DataColumn(colName);
                dt.Columns.Add(dc);
            }

            #endregion

            #region Read "Entity" sheet - read "_dtEntities"  datatable and copy columns in dummy data table

            string validSheetName = ExternalFileReader.ValidateExcelWorkSheet(_sourceFile, RSExcelConstants.EntityDataSheetName);

            if (RSExcelConstants.EntityDataSheetName.ToLower() == validSheetName.Replace("$", "").ToLower())
            {
                DataSet dsEntity = null;
                //Read Excel data in dataset
                if (!_isSAXParserEnabled)
                {
                    dsEntity = ExternalFileReader.ReadExternalFile("excel", _sourceFile, RSExcelConstants.EntityDataSheetName, "", false, null, null, null, 0, "ANSI");
                }
                else
                {
                    dsEntity = ExternalFileReader.ReadExternalFileSAXParsing(_sourceFile, RSExcelConstants.EntityDataSheetName, false);
                }
                //Add each column in another datatable.
                if (dsEntity != null && dsEntity.Tables != null && dsEntity.Tables.Count > 0)
                {
                    DataTable dtEntities = dsEntity.Tables[0];
                    if (dtEntities != null)
                    {
                        foreach (DataColumn column in dtEntities.Columns)
                        {
                            if (!column.ColumnName.Equals(RSExcelConstants.EntityDataTemplateColumns[EntityDataTemplateFieldEnum.IsProcessed], StringComparison.InvariantCultureIgnoreCase))
                            {
                                dt.Columns.Add(String.Concat("[", RSExcelConstants.EntityDataSheetName, "].[", column.ColumnName, "]"));
                            }
                        }
                    }
                }
            }

            #endregion

            #region Read "Relationship" sheet

            validSheetName = ExternalFileReader.ValidateExcelWorkSheet(_sourceFile, RSExcelConstants.RelationshipSheetName);

            if (RSExcelConstants.RelationshipSheetName.ToLower() == validSheetName.Replace("$", "").ToLower())
            {
                DataSet dsRelationship = null;
                //Read Excel data in dataset
                if (!_isSAXParserEnabled)
                {
                    dsRelationship = ExternalFileReader.ReadExternalFile("excel", _sourceFile, RSExcelConstants.RelationshipSheetName, "", false, null, null, null, 0, "ANSI");
                }
                else
                {
                    dsRelationship = ExternalFileReader.ReadExternalFileSAXParsing(_sourceFile, RSExcelConstants.RelationshipSheetName, false);
                }
                //Add each column in another datatable.
                if (dsRelationship != null && dsRelationship.Tables != null && dsRelationship.Tables.Count > 0)
                {
                    DataTable dtRelationships = dsRelationship.Tables[0];

                    foreach (DataColumn column in dtRelationships.Columns)
                    {
                        //"$Processed$" is column used for internal purpose. So remove this as it is not needed on UI.
                        if (!column.ColumnName.Equals(RSExcelConstants.RelationshipDataTemplateColumns[RelationshipDataTemplateFieldEnum.IsProcessed], StringComparison.InvariantCultureIgnoreCase))
                        {
                            dt.Columns.Add(String.Concat("[", RSExcelConstants.RelationshipSheetName, "].[", column.ColumnName, "]"));
                        }
                    }
                }
            }

            #endregion

            ds.Tables.Add(dt);

            if (_traceSettings.IsBasicTracingEnabled) activity.Stop();

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
        /// 
        /// </summary>
        /// <returns></returns>
        public Int32 GetRelationshipDataBatchSize()
        {
            return _batchSize;
        }

        public EntityCollection GetAllRelationshipData(MDMCenterApplication application, MDMCenterModules module)
        {
            throw new NotImplementedException();
        }

        public EntityCollection GetRelationshipDataBatch(Int64 startPK, Int64 endPK, MDMCenterApplication application, MDMCenterModules module)
        {
            throw new NotImplementedException();
        }

        public EntityCollection GetRelationshipDataNextBatch(Int32 batchSize, MDMCenterApplication application, MDMCenterModules module)
        {
            throw new NotImplementedException();
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

            //KEYNOTE: HERE input field format normally comes as [SheetName$].[AttributeGroupName//AttributeName]
            String[] arrSeprators = new String[] { "//" };

            //Consider only last [] value to start with
            inputFieldName = inputFieldName.Substring(inputFieldName.LastIndexOf('[') + 1);
            inputFieldName = inputFieldName.Replace("]", String.Empty);

            String[] fieldComponents = inputFieldName.Split(arrSeprators, StringSplitOptions.None);

            if (fieldComponents != null)
            {
                attr = new Attribute();
                attr.Id = 0;
                attr.Locale = _defaultLocale;

                if (fieldComponents.Length == 2)
                {
                    attr.AttributeParentName = fieldComponents[0];
                    attr.Name = fieldComponents[1];
                    attr.LongName = fieldComponents[1];
                }
                else if (fieldComponents.Length == 1)
                {
                    attr.Name = fieldComponents[0];
                    attr.LongName = fieldComponents[0];
                    // we do not have the parent name. The idea is to find the attribute with just the name...
                    // the import engine will find with name first and only then use the parent name..
                    attr.AttributeParentName = fieldComponents[0];
                }
                else if (fieldComponents.Length == 3)
                {
                    // This is for a future scenario, when a locale will be added to the name.
                    attr.AttributeParentName = fieldComponents[0];
                    attr.Name = fieldComponents[1];
                    attr.LongName = fieldComponents[1];

                    String strlocale = fieldComponents[2];

                    LocaleEnum locale = LocaleEnum.UnKnown;
                    //try to parse only when this value is valid, otherwise TryParse will assign default value
                    if (Enum.IsDefined(typeof(LocaleEnum), strlocale))
                    {
                        Enum.TryParse<LocaleEnum>(strlocale, out locale);
                    }
                    attr.Locale = locale;
                }
            }
            return attr;
        }

        #endregion

        #region Private Methods

        #region Read Methods

        private void ReadMetadataTab()
        {
            DiagnosticActivity activity = new DiagnosticActivity();
            if (_traceSettings.IsBasicTracingEnabled) activity.Start();

            String filename = _sourceFile;
            DataSet dsMetadata = null;

            if (!_isSAXParserEnabled)
            {
                dsMetadata = ExternalFileReader.ReadExternalFile("excel", filename, RSExcelConstants.MetadataSheetName, "", false, null, null, null, 0, "ANSI");

                if (_traceSettings.IsBasicTracingEnabled)
                    activity.LogInformation(string.Format("Read {0} sheet using ACE parser", RSExcelConstants.MetadataSheetName));
            }
            else
            {
                dsMetadata = ExternalFileReader.ReadExternalFileSAXParsing(filename, RSExcelConstants.MetadataSheetName, false);

                if (_traceSettings.IsBasicTracingEnabled)
                    activity.LogInformation(string.Format("Read {0} sheet using SAX parser", RSExcelConstants.MetadataSheetName));
            }

            //Metadata tab header processing
            if (dsMetadata != null && dsMetadata.Tables != null && dsMetadata.Tables.Count > 0)
            {
                DataTable dtMetadata = dsMetadata.Tables[0];

                if (dtMetadata.Columns != null && dtMetadata.Columns.Count > 0)
                {
                    DataColumnCollection metadataColumns = dtMetadata.Columns;

                    if (dtMetadata.Rows.Count > 0 && dtMetadata.Columns.Count >= 8)
                    {
                        Dictionary<MetadataTemplateFieldEnum, KeyValuePair<String, String>> tempMetadataTemplateFields = new Dictionary<MetadataTemplateFieldEnum, KeyValuePair<String, String>>();
                        Dictionary<String, String> tempComplexAttributes = new Dictionary<String, String>();
                        Boolean readComplexAttribute = false;

                        for (int i = 1; i < dtMetadata.Rows.Count; i++)
                        {
                            DataRow dataRow = dtMetadata.Rows[i];

                            String key1 = dataRow[0] != null ? dataRow[0].ToString() : String.Empty;
                            String value1 = dataRow[1] != null ? dataRow[1].ToString() : String.Empty;
                            String key2 = dataRow[3] != null ? dataRow[3].ToString() : String.Empty;
                            String value2 = dataRow[4] != null ? dataRow[4].ToString() : String.Empty;
                            String key3 = dataRow[6] != null ? dataRow[6].ToString() : String.Empty;
                            String value3 = dataRow[7] != null ? dataRow[7].ToString() : String.Empty;

                            foreach (KeyValuePair<MetadataTemplateFieldEnum, KeyValuePair<String, String>> pair in RSExcelConstants.MetadataTemplateFields)
                            {
                                MetadataTemplateFieldEnum templeteField = pair.Key;
                                KeyValuePair<String, String> valuePair = pair.Value;
                                String valueKey = valuePair.Key;

                                if (valueKey.Equals(key1, StringComparison.InvariantCultureIgnoreCase))
                                {
                                    //RSExcel11TempleteConstants.MetadataTemplateFields[templeteField] = new KeyValuePair<String, String>(key1, value1);
                                    tempMetadataTemplateFields.Add(templeteField, new KeyValuePair<string, string>(key1, value1));
                                }
                                else if (valueKey.Equals(key2, StringComparison.InvariantCultureIgnoreCase))
                                {
                                    //RSExcel11TempleteConstants.MetadataTemplateFields[templeteField] = new KeyValuePair<String, String>(key2, value2);
                                    tempMetadataTemplateFields.Add(templeteField, new KeyValuePair<string, string>(key2, value2));
                                }
                                else if (valueKey.Equals(key3, StringComparison.InvariantCultureIgnoreCase))
                                {
                                    //RSExcel11TempleteConstants.MetadataTemplateFields[templeteField] = new KeyValuePair<String, String>(key3, value3);
                                    tempMetadataTemplateFields.Add(templeteField, new KeyValuePair<string, string>(key3, value3));
                                }
                            }

                            //check the complex attributes are allowed to read and there are any
                            if (readComplexAttribute && !String.IsNullOrWhiteSpace(key1))
                            {
                                tempComplexAttributes.Add(key1, value1);
                            }

                            //starting from next line we would read ComplexAttributes
                            if (key1.ToLower() == "Attribute Identifier".ToLower())
                            {
                                readComplexAttribute = true;
                            }
                        }

                        if (tempComplexAttributes.Count > 0)
                        {
                            _complexAttributes = tempComplexAttributes;
                        }

                        //Now set the value to actual dictionary. We need to do this as KeyValuePair is read only object once created..
                        foreach (KeyValuePair<MetadataTemplateFieldEnum, KeyValuePair<String, String>> pair in tempMetadataTemplateFields)
                        {
                            RSExcelConstants.MetadataTemplateFields[pair.Key] = pair.Value;
                        }
                    }
                }
            }
            else
            {
                if (_traceSettings.IsBasicTracingEnabled)
                {
                    string message = string.Format("Matadata sheet ({0}) not available in file - {1}", RSExcelConstants.MetadataSheetName, filename);
                    activity.LogError(message);
                }
            }

            if (_traceSettings.IsBasicTracingEnabled) activity.Stop();
        }

        // RBC
        private void ReadEntityDataTabs()
        {
            DiagnosticActivity activity = new DiagnosticActivity();
            if (_traceSettings.IsBasicTracingEnabled) activity.Start();

            String filename = _sourceFile;

            List<Entity> entityList = new List<Entity>();

            #region Read Entity Tab

            DataSet dsEntity = null;

            if (!_isSAXParserEnabled)
            {
                String validEntitySheetName = ExternalFileReader.ValidateExcelWorkSheet(filename, RSExcelConstants.EntityDataSheetName);

                if (RSExcelConstants.EntityDataSheetName.Equals(validEntitySheetName.Replace("$", ""), StringComparison.InvariantCultureIgnoreCase))
                {
                    dsEntity = ExternalFileReader.ReadExternalFile("excel", filename, RSExcelConstants.EntityDataSheetName, "", false, null, null, null, 0, "ANSI");

                    if (_traceSettings.IsBasicTracingEnabled)
                        activity.LogInformation(string.Format("Read {0} sheet using ACE parser", RSExcelConstants.EntityDataSheetName));
                }
            }
            else
            {
                String validEntitySheetName = ExternalFileReader.ValidateExcelWorkSheetSAXParser(filename, RSExcelConstants.EntityDataSheetName);

                if (!String.IsNullOrWhiteSpace(validEntitySheetName) && RSExcelConstants.EntityDataSheetName.Equals(validEntitySheetName.Replace("$", ""), StringComparison.InvariantCultureIgnoreCase))
                {
                    dsEntity = ExternalFileReader.ReadExternalFileSAXParsing(filename, RSExcelConstants.EntityDataSheetName, false);
                }
                if (_traceSettings.IsBasicTracingEnabled)
                    activity.LogInformation(string.Format("Read {0} sheet using SAX parser", RSExcelConstants.EntityDataSheetName));
            }

            if (dsEntity != null && dsEntity.Tables != null && dsEntity.Tables.Count > 0)
            {
                this._dtEntities = dsEntity.Tables[0];

                if (this._dtEntities != null)
                {
                    DataColumnCollection entitiesSheetColumns = this._dtEntities.Columns;
                    if (entitiesSheetColumns != null && entitiesSheetColumns.Count > 0)
                    {
                        ValidateEntitiesMandatoryColumns(entitiesSheetColumns);
                    }
                }
                //PopulateEntityAttributesList(_dtEntities);

                Dictionary<String, Int32> mappings = GetAttributeColumnIndexMappings(filename, RSExcelConstants.EntityDataSheetName, _entityAttributesList, RSExcelConstants.EntityDataTemplateColumns.Values.ToList());

                if (_dtEntities.Rows.Count > 0)
                {
                    FillEntities(entityList, _dtEntities.Rows, mappings);
                }
            }

            #endregion

            #region Read Relationship Tab

            DataSet dsRelationship = null;

            if (!_isSAXParserEnabled)
            {
                String validRelationshipSheetName = ExternalFileReader.ValidateExcelWorkSheet(filename, RSExcelConstants.RelationshipSheetName);

                if (RSExcelConstants.RelationshipSheetName.Equals(validRelationshipSheetName.Replace("$", ""), StringComparison.InvariantCultureIgnoreCase))
                {
                    dsRelationship = ExternalFileReader.ReadExternalFile("excel", filename, RSExcelConstants.RelationshipSheetName, "", false, null, null, null, 0, "ANSI");

                    if (_traceSettings.IsBasicTracingEnabled)
                        activity.LogInformation(string.Format("Read {0} sheet using ACE parser", RSExcelConstants.RelationshipSheetName));
                }
            }
            else
            {
                String validRelationshipSheetName = ExternalFileReader.ValidateExcelWorkSheetSAXParser(filename, RSExcelConstants.RelationshipSheetName);

                if (!String.IsNullOrWhiteSpace(validRelationshipSheetName) && RSExcelConstants.RelationshipSheetName.Equals(validRelationshipSheetName.Replace("$", ""), StringComparison.InvariantCultureIgnoreCase))
                {
                    dsRelationship = ExternalFileReader.ReadExternalFileSAXParsing(filename, RSExcelConstants.RelationshipSheetName, false);
                }
                if (_traceSettings.IsBasicTracingEnabled)
                    activity.LogInformation(string.Format("Read {0} sheet using SAX parser", RSExcelConstants.RelationshipSheetName));
            }

            if (dsRelationship != null && dsRelationship.Tables != null && dsRelationship.Tables.Count > 0)
            {
                this._dtRelationships = dsRelationship.Tables[0];

                //ReadRelationshipAttributesList(_dtRelationships);
                Dictionary<String, Int32> mappings = GetAttributeColumnIndexMappings(filename, RSExcelConstants.RelationshipSheetName, _relationshipAttributesList, RSExcelConstants.RelationshipDataTemplateColumns.Values.ToList());
                if (_dtRelationships.Rows.Count > 0)
                {
                    FillEntityRelationships(entityList, _dtRelationships.Rows, mappings);
                }
            }

            #endregion

            #region Read Complex Attribute Tabs

            if (_complexAttributes.Count > 0)
            {
                foreach (KeyValuePair<String, String> temp in _complexAttributes)
                {
                    DataSet dsComplexAttribute = null;

                    if (!_isSAXParserEnabled)
                    {
                        String validComplexAttrSheetName = ExternalFileReader.ValidateExcelWorkSheet(filename, temp.Value);

                        if (temp.Value.Equals(validComplexAttrSheetName.Replace("$", ""), StringComparison.InvariantCultureIgnoreCase))
                        {
                            dsComplexAttribute = ExternalFileReader.ReadExternalFile("excel", filename, temp.Value, "", false, null, null, null, 0, "ANSI");

                            if (_traceSettings.IsBasicTracingEnabled)
                                activity.LogInformation(string.Format("Read {0} sheet using ACE parser", temp.Value));
                        }
                        else
                        {
                            String message = String.Format("{0} sheet does not exist for the {1} Attribute Identifier", temp.Value, temp.Key);
                            throw new MDMOperationException("114080", message, "ExcelSource", String.Empty, "ValidateComplexSheetNames");
                        }
                    }
                    else
                    {
                        String validComplexAttrSheetName = ExternalFileReader.ValidateExcelWorkSheetSAXParser(filename, temp.Value);

                        if (!String.IsNullOrWhiteSpace(validComplexAttrSheetName) &&
                            temp.Value.Equals(validComplexAttrSheetName.Replace("$", ""),
                                StringComparison.InvariantCultureIgnoreCase))
                        {
                            dsComplexAttribute = ExternalFileReader.ReadExternalFileSAXParsing(filename, temp.Value,
                                false);
                        }
                        else
                        {
                            String message = String.Format("{0} sheet does not exist for the {1} Attribute Identifier", temp.Value, temp.Key);
                            throw new MDMOperationException("114080", message, "ExcelSource", String.Empty, "ValidateComplexSheetNames");
                        }
                    }

                    if (dsComplexAttribute != null && dsComplexAttribute.Tables != null && dsComplexAttribute.Tables.Count > 0)
                    {
                        this._dtComplexAttribues = dsComplexAttribute.Tables[0];

                        //ReadComplexChildAttributeList(_dtComplexAttribues);
                        Dictionary<String, Int32> mappings = GetAttributeColumnIndexMappings(filename, temp.Value, _complexChildAttributeList, RSExcelConstants.ComplexAttributeTemplateColumns.Values.ToList());
                        if (_dtComplexAttribues.Rows.Count > 0)
                        {
                            FillComplexAttributes(entityList, _dtComplexAttribues.Rows, temp.Value, String.Format("[{0}$].[{1}]", temp.Value, temp.Key), mappings); //Attribute format: [SheetName$].[AttributeGroupName//AttributeName] for reading complex attribute details
                        }
                    }
                }
            }
            #endregion

            this._entityList = entityList;

            if (_traceSettings.IsBasicTracingEnabled) activity.Stop();
        }

        private void ValidateEntitiesMandatoryColumns(DataColumnCollection excelSheetColumns)
        {
            Collection<String> mandatoryColumns = new Collection<String>();
            mandatoryColumns.Add(RSExcelConstants.EntityDataTemplateColumns[EntityDataTemplateFieldEnum.ExtenalId].ToString());
            mandatoryColumns.Add(RSExcelConstants.EntityDataTemplateColumns[EntityDataTemplateFieldEnum.LongName].ToString());
            mandatoryColumns.Add(RSExcelConstants.EntityDataTemplateColumns[EntityDataTemplateFieldEnum.EntityType].ToString());
            mandatoryColumns.Add(RSExcelConstants.EntityDataTemplateColumns[EntityDataTemplateFieldEnum.CategoryPath].ToString());
            mandatoryColumns.Add(RSExcelConstants.EntityDataTemplateColumns[EntityDataTemplateFieldEnum.Container].ToString());
            mandatoryColumns.Add(RSExcelConstants.EntityDataTemplateColumns[EntityDataTemplateFieldEnum.Organization].ToString());

            foreach (String column in mandatoryColumns)
            {
                Boolean isColumnAvailable = false;

                foreach (DataColumn excelSheetColumn in excelSheetColumns)
                {
                    if (excelSheetColumn.ColumnName.Equals(column))
                    {
                        isColumnAvailable = true;
                        break;
                    }
                }

                if (!isColumnAvailable)
                {
                    String message = String.Format("Mandatory column: {0} could not be found, aborting the import process.", column);
                    throw new MDMOperationException("113854", message, "ExcelSource", String.Empty, "ValidateEntitiesMandatoryColumns");
                }
            }
        }

        private void PopulateEntityAttributesList(DataTable dtEntities)
        {
            if (dtEntities != null)
            {
                foreach (DataColumn dc in dtEntities.Columns)
                {
                    if (!RSExcelConstants.EntityDataTemplateColumns.ContainsValue(dc.ColumnName))
                    {
                        _entityAttributesList.Add(dc.ColumnName);
                    }
                }
            }
        }

        private void ReadRelationshipAttributesList(DataTable dtRelationships)
        {
            if (dtRelationships != null)
            {
                foreach (DataColumn dc in dtRelationships.Columns)
                {
                    if (!RSExcelConstants.RelationshipDataTemplateColumns.ContainsValue(dc.ColumnName))
                    {
                        _relationshipAttributesList.Add(dc.ColumnName);
                    }
                }
            }
        }

        private void ReadComplexChildAttributeList(DataTable dtComplexAttribute)
        {
            _complexChildAttributeList = new Collection<String>();
            if (dtComplexAttribute != null)
            {
                foreach (DataColumn dc in dtComplexAttribute.Columns)
                {
                    if (!RSExcelConstants.ComplexAttributeTemplateColumns.ContainsValue(dc.ColumnName))
                    {
                        _complexChildAttributeList.Add(dc.ColumnName);
                    }
                }
            }
        }

        #endregion

        #region Fill Methods

        private Boolean FillEntities(List<Entity> entitiesTobeProcessed, DataRowCollection dataRows, Dictionary<String, Int32> attributeColIndexMappings)
        {
            DiagnosticActivity activity = new DiagnosticActivity();
            if (_traceSettings.IsBasicTracingEnabled) activity.Start();

            Boolean successFlag = true;
            Int64 entityCounter = 2;

            foreach (DataRow dataRow in dataRows)
            {

                Entity entity = new Entity();

                #region Fill Entity level properties

                FillEntityProperties(entity, dataRow);

                entity.ReferenceId = entityCounter;
                entityCounter++;
                #endregion

                #region Fill entity attribute objects

                FillEntityAttributes(entity, dataRow, attributeColIndexMappings);

                #endregion

                entitiesTobeProcessed.Add(entity);
            }

            if (_traceSettings.IsBasicTracingEnabled)
            {
                activity.LogInformation(string.Format("Fill entities completed for {0} entities", entitiesTobeProcessed.Count));
                activity.Stop();
            }

            return successFlag;
        }

        private Boolean FillEntityRelationships(List<Entity> entitiesTobeProcessed, DataRowCollection dataRows, Dictionary<String, Int32> attributeColIndexMappings)
        {
            DiagnosticActivity activity = new DiagnosticActivity();
            if (_traceSettings.IsBasicTracingEnabled) activity.Start();

            Boolean successFlag = true;
            Int64 relationshipCounter = 2;

            foreach (DataRow dataRow in dataRows)
            {
                Relationship relationship = new Relationship();

                String fromEntityExternalId = String.Empty;
                String fromEntityEntityType = RSExcelConstants.MetadataTemplateFields[MetadataTemplateFieldEnum.DefaultFromEntityEntityType].Value;
                String fromEntityCategoryPath = RSExcelConstants.MetadataTemplateFields[MetadataTemplateFieldEnum.DefaultFromEntityCateogryPath].Value;
                String fromEntityContainer = RSExcelConstants.MetadataTemplateFields[MetadataTemplateFieldEnum.DefaultFromEntityContainer].Value;
                String fromEntityOrganization = RSExcelConstants.MetadataTemplateFields[MetadataTemplateFieldEnum.DefaultFromEntityOrganization].Value;

                #region Read Relationship from data

                if (this._dtRelationships.Columns.Contains(RSExcelConstants.RelationshipDataTemplateColumns[RelationshipDataTemplateFieldEnum.FromEntityExternalId]))
                {
                    String rowLevelFromEntityExternalId = dataRow[RSExcelConstants.RelationshipDataTemplateColumns[RelationshipDataTemplateFieldEnum.FromEntityExternalId]].ToString();

                    if (!String.IsNullOrWhiteSpace(rowLevelFromEntityExternalId))
                    {
                        fromEntityExternalId = rowLevelFromEntityExternalId;
                    }
                }

                if (this._dtRelationships.Columns.Contains(RSExcelConstants.RelationshipDataTemplateColumns[RelationshipDataTemplateFieldEnum.FromEntityEntityType]))
                {
                    String rowLevelFromEntityEntityType = dataRow[RSExcelConstants.RelationshipDataTemplateColumns[RelationshipDataTemplateFieldEnum.FromEntityEntityType]].ToString();

                    if (!String.IsNullOrWhiteSpace(rowLevelFromEntityEntityType))
                    {
                        fromEntityEntityType = rowLevelFromEntityEntityType;
                    }
                }

                if (this._dtRelationships.Columns.Contains(RSExcelConstants.RelationshipDataTemplateColumns[RelationshipDataTemplateFieldEnum.FromEntityCategoryPath]))
                {
                    String rowLevelFromEntityCategoryPath = dataRow[RSExcelConstants.RelationshipDataTemplateColumns[RelationshipDataTemplateFieldEnum.FromEntityCategoryPath]].ToString();

                    if (!String.IsNullOrWhiteSpace(rowLevelFromEntityCategoryPath))
                    {
                        fromEntityCategoryPath = rowLevelFromEntityCategoryPath;
                    }
                }

                if (this._dtRelationships.Columns.Contains(RSExcelConstants.RelationshipDataTemplateColumns[RelationshipDataTemplateFieldEnum.FromEntityContainer]))
                {
                    String rowLevelFromContainer = dataRow[RSExcelConstants.RelationshipDataTemplateColumns[RelationshipDataTemplateFieldEnum.FromEntityContainer]].ToString();

                    if (!String.IsNullOrWhiteSpace(rowLevelFromContainer))
                    {
                        fromEntityContainer = rowLevelFromContainer;
                    }
                }

                if (this._dtRelationships.Columns.Contains(RSExcelConstants.RelationshipDataTemplateColumns[RelationshipDataTemplateFieldEnum.FromEntityOrganization]))
                {
                    String rowLevelFromEntityOrganization = dataRow[RSExcelConstants.RelationshipDataTemplateColumns[RelationshipDataTemplateFieldEnum.FromEntityOrganization]].ToString();

                    if (!String.IsNullOrWhiteSpace(rowLevelFromEntityOrganization))
                    {
                        fromEntityOrganization = rowLevelFromEntityOrganization;
                    }
                }

                #endregion

                FillRelationshipProperties(relationship, dataRow);

                FillRelationshipAttributes(relationship, dataRow, attributeColIndexMappings);

                relationship.ReferenceId = relationshipCounter;
                relationshipCounter++;

                Entity entity = null;

                //Link relationship to existing entity else create new entity and add relationships there..
                var filteredEntities = from ent in entitiesTobeProcessed
                                       where (ent.ExternalId.Equals(fromEntityExternalId, StringComparison.InvariantCultureIgnoreCase)
                                                && ent.EntityTypeName.Equals(fromEntityEntityType, StringComparison.InvariantCultureIgnoreCase)
                                                && ent.CategoryPath.Equals(fromEntityCategoryPath, StringComparison.InvariantCultureIgnoreCase)
                                                && ent.ContainerName.Equals(fromEntityContainer, StringComparison.InvariantCultureIgnoreCase)
                                                && ent.OrganizationName.Equals(fromEntityOrganization, StringComparison.InvariantCultureIgnoreCase))
                                       select ent;

                if (filteredEntities.Any())
                {
                    entity = filteredEntities.FirstOrDefault();
                }
                else
                {
                    entity = new Entity();

                    entity.ExternalId = fromEntityExternalId;
                    entity.EntityTypeName = fromEntityEntityType;
                    entity.CategoryPath = fromEntityCategoryPath;
                    entity.ContainerName = fromEntityContainer;
                    entity.OrganizationName = fromEntityOrganization;

                    entity.ReferenceId = relationshipCounter;
                    entity.Action = ObjectAction.Unknown;
                    entity.ExtendedProperties = String.Format("SourceDataReadError#@@#{0}", RSExcelConstants.RelationshipSheetName);
                    entitiesTobeProcessed.Add(entity);
                }

                if (entity != null)
                {
                    entity.Relationships.Add(relationship);
                }
            }


            if (_traceSettings.IsBasicTracingEnabled)
            {
                activity.LogInformation(string.Format("Fill entity relationships completed for {0} entities.", entitiesTobeProcessed.Count));
                activity.Stop();
            }

            return successFlag;
        }

        private Boolean FillComplexAttributes(List<Entity> entitiesTobeProcessed, DataRowCollection dataRows, String complexSheetName, String complexAttrSheetName, Dictionary<String, Int32> attributeColIndexMappings)
        {
            DiagnosticActivity activity = new DiagnosticActivity();
            if (_traceSettings.IsBasicTracingEnabled) activity.Start();

            Boolean successFlag = true; //why this is required?

            //Getting the attribute Definition (Dummy Attribute based in the sheet name)
            Attribute complexAttribute = GetAttributeInfoFromInputFieldName(complexAttrSheetName);

            //Check if the column Locale exists.  Locale would indicate if this is simple complex attribute or complex hierarchial attribute.
            var complexAttributeModel = CachedAttributeDataModel.GetAllBaseAttributeModels().FirstOrDefault(a => a.Name.Equals(complexAttribute.Name)
                                                                        && a.AttributeParentName.Equals(complexAttribute.AttributeParentName) && a.Locale == _defaultLocale);

            if (complexAttributeModel == null)
            {
                return false;
            }

            if (complexAttributeModel.IsComplex)
            {

                if (!complexAttributeModel.IsHierarchical)
                {
                    ProcessComplexAttributes(dataRows, attributeColIndexMappings, complexSheetName, entitiesTobeProcessed, ref complexAttribute);
                }
                else
                {
                    ProcessComplexHierarchialAttributes(dataRows, attributeColIndexMappings, complexSheetName, entitiesTobeProcessed, complexAttributeModel);
                }
            }

            if (_traceSettings.IsBasicTracingEnabled)
            {
                activity.LogInformation(string.Format("Fill entity complex attributes completed for {0} entities.",
                  entitiesTobeProcessed.Count));
                activity.Stop();
            }

            return successFlag;

        }

        /// <summary>
        ///  Build Hierarchial attribute for each Entity from the hierarchial data sheet
        /// </summary>
        /// <param name="dataRows"></param>
        /// <param name="attributeColIndexMappings"></param>
        /// <param name="complexSheetName"></param>
        /// <param name="entitiesTobeProcessed"></param>
        /// <param name="hierarchialAttributeModel"></param>
        private void ProcessComplexHierarchialAttributes(DataRowCollection dataRows, Dictionary<string, int> attributeColIndexMappings, string complexSheetName, List<Entity> entitiesTobeProcessed, AttributeModel hierarchialAttributeModel)
        {
            Int32 hierarchialCounter = 0;

            var dataRowList = dataRows.Cast<DataRow>().ToList();

            while (hierarchialCounter < dataRowList.Count)
            {

                var entity = PrepareEntityFromDataRow(dataRows[hierarchialCounter], complexSheetName, hierarchialCounter, entitiesTobeProcessed);

                if (hierarchialAttributeModel == null)
                {
                    continue;
                }

                var hierarchialAttribute = new Attribute(hierarchialAttributeModel, _defaultLocale);
                hierarchialAttribute.Id = hierarchialAttributeModel.Id;

                var filteredDataRows = dataRowList.Where(dr => dr[attributeColIndexMappings["External Id"]].ToString().Equals(entity.Name, StringComparison.InvariantCultureIgnoreCase)).ToList();

                //move the index by the number of rows returned.  If its a hierarchial collection the attribute would have multiple rows in the excel.
                hierarchialCounter += filteredDataRows.Count;

                if (filteredDataRows.Any())
                {
                    BuildComplexHierarchialAttributeCollection(_complexChildAttributeList, filteredDataRows, hierarchialAttributeModel, hierarchialAttribute, attributeColIndexMappings);

                    //This needs to be done to remove the values object at instance level.  The action flag on this by default set to "Read"  this causes
                    //the hierarchial attribute to be not created. 
                    SetOverridenValuesAction(hierarchialAttribute, ObjectAction.Create);

                    entity.Attributes.Add(hierarchialAttribute);
                }
            }
        }

        /// <summary>
        /// Recursively creates the hierarchial attributes and populated them
        /// </summary>
        /// <param name="complexChildAttributeList"></param>
        /// <param name="dataRows"></param>
        /// <param name="attributeModelCollection"></param>
        /// <param name="hierarchialInstanceAttribute"></param>
        /// <param name="attributeColIndexMappings"></param>
        /// <param name="parentColumnName"></param>
        /// <param name="rowSequence"></param>
        private void BuildComplexHierarchialAttributeCollection(Collection<string> complexChildAttributeList, List<DataRow> dataRows, IAttributeModelCollection attributeModelCollection, Attribute hierarchialInstanceAttribute, Dictionary<string, int> attributeColIndexMappings, String parentColumnName, Int32? rowSequence = null)
        {
            foreach (var attributeModel in attributeModelCollection)
            {
                var columnName = GetAttributeColumnName(attributeModel, attributeColIndexMappings);

                if (String.IsNullOrWhiteSpace(columnName))
                {
                    throw new Exception(String.Format("Hierarchical attribute {0}, does not contain the column {1}", attributeModel.AttributeParentName, columnName));
                }

                List<DataRow> rows = dataRows;

                if (attributeModel.IsHierarchical)
                {
                    if (!rowSequence.HasValue)
                    {
                        rows = dataRows.GroupBy(dr => Int32.Parse(String.IsNullOrEmpty(dr[attributeColIndexMappings[columnName]].ToString()) ? "0" : dr[attributeColIndexMappings[columnName]].ToString())).Select(dr => dr.First()).ToList();
                    }
                    else if (attributeColIndexMappings != null && attributeColIndexMappings.ContainsKey(parentColumnName))
                    {
                        rows = dataRows.Where(dr => Int32.Parse(String.IsNullOrEmpty(dr[attributeColIndexMappings[parentColumnName]].ToString()) ? "0" : dr[attributeColIndexMappings[parentColumnName]].ToString()) == rowSequence.Value).GroupBy(dr => Int32.Parse(String.IsNullOrEmpty(dr[attributeColIndexMappings[columnName]].ToString()) ? "0" : dr[attributeColIndexMappings[columnName]].ToString())).Select(dr => dr.First()).ToList();
                    }

                    var hierarachialAttribute = hierarchialInstanceAttribute.Attributes.GetAttribute(attributeModel.Name);

                    if (attributeModel.IsCollection)
                    {
                        var sequence = 0;

                        foreach (var row in rows)
                        {
                            IAttributeCollection iAttributeCollection = hierarachialAttribute.NewComplexChildRecord();
                            var childInstanceAttribute = (Attribute)hierarachialAttribute.AddComplexChildRecord(iAttributeCollection);
                            childInstanceAttribute.Id = attributeModel.Id;

                            BuildComplexHierarchialAttributeCollection(complexChildAttributeList, dataRows, attributeModel.GetChildAttributeModels(), childInstanceAttribute, attributeColIndexMappings, columnName, sequence);

                            //This needs to be done to remove the values object at instance level.  The action flag on this by default set to "Read"  this causes
                            //the hierarchial attribute to be not created.
                            //childInstanceAttribute.OverriddenValues.Clear();
                            SetOverridenValuesAction(childInstanceAttribute, ObjectAction.Create);

                            sequence++;
                        }
                    }
                    else
                    {
                        IAttributeCollection iAttributeCollection = hierarachialAttribute.NewComplexChildRecord();
                        var childInstanceAttribute = (Attribute)hierarachialAttribute.AddComplexChildRecord(iAttributeCollection);
                        childInstanceAttribute.Id = attributeModel.Id;

                        BuildComplexHierarchialAttributeCollection(complexChildAttributeList, rows, attributeModel.GetChildAttributeModels(), childInstanceAttribute, attributeColIndexMappings, columnName, 0);

                        //This needs to be done to remove the values object at instance level.  The action flag on this by default set to "Read"  this causes
                        //the hierarchial attribute to be not created.
                        //childInstanceAttribute.OverriddenValues.Clear();
                        SetOverridenValuesAction(childInstanceAttribute, ObjectAction.Create);

                    }
                }
                else
                {
                    var attributeNameList = complexChildAttributeList.Where(excelColumnName => excelColumnName.StartsWith(columnName)).ToList();

                    DataRow row = dataRows.FirstOrDefault(dr => Int32.Parse(String.IsNullOrEmpty(dr[attributeColIndexMappings[parentColumnName]].ToString()) ? "0" : dr[attributeColIndexMappings[parentColumnName]].ToString()) == rowSequence.Value);

                    //The first row should be the top level hierarchy row that would have the values.

                    if (row == null)
                    {
                        continue;
                    }

                    foreach (var attributeName in attributeNameList)
                    {
                        LocaleEnum locale = GetLocaleFromAttributeName(attributeName);

                        var instanceAttribute = (Attribute)hierarchialInstanceAttribute.Attributes.GetAttribute(attributeModel.Name, locale);

                        String attributeStringValue = row[attributeColIndexMappings[columnName]].ToString();

                        if (instanceAttribute == null)
                        {
                            instanceAttribute = new Attribute(attributeModel, locale)
                                                  {
                                                      Id = attributeModel.Id,
                                                      LongName = String.Format("{0}_{1}",
                                                      attributeModel.Name, locale)
                                                  };
                            hierarchialInstanceAttribute.Attributes.Add(instanceAttribute, true);
                        }
                        else
                        {
                            if (instanceAttribute.Locale != locale)
                            {
                                instanceAttribute = new Attribute(attributeModel, locale)
                                                    {
                                                        Id = attributeModel.Id,
                                                        LongName = String.Format("{0}_{1}",
                                                        attributeModel.Name, locale)
                                                    };
                                hierarchialInstanceAttribute.Attributes.Add(instanceAttribute, true);
                            }
                        }

                        if (!String.IsNullOrWhiteSpace(attributeStringValue))
                        {
                            BusinessObjects.Value attributeValue = new BusinessObjects.Value
                                                                    {
                                                                        AttrVal = attributeStringValue,
                                                                        Locale = locale
                                                                    };

                            instanceAttribute.SetValue(attributeValue, locale);
                        }
                        else
                        {
                            //Setting the attribute action as ignore when the value cell under the attribute in the excel sheet is blank
                            instanceAttribute.Action = ObjectAction.Ignore;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Process the Hierarchial attributes and calls the recursive BuildComplexHierarchialAttributeCollection with model collection.  
        /// </summary>
        /// <param name="complexChildAttributeList"></param>
        /// <param name="dataRows"></param>
        /// <param name="attributeModel"></param>
        /// <param name="hierarchialAttribute"></param>
        /// <param name="attributeColIndexMappings"></param>
        private void BuildComplexHierarchialAttributeCollection(Collection<string> complexChildAttributeList, List<DataRow> dataRows, AttributeModel attributeModel, Attribute hierarchialAttribute, Dictionary<string, int> attributeColIndexMappings)
        {
            var columnName = GetAttributeColumnName(attributeModel, attributeColIndexMappings);

            if (attributeModel.IsHierarchical)
            {
                if (attributeModel.IsCollection)
                {
                    if (!attributeColIndexMappings.ContainsKey(columnName))
                    {
                        throw new Exception(String.Format("Hierarchial collection attribute {0}, does not contain the sequence column {1}", attributeModel.AttributeParentName, columnName));
                    }

                    var rows = dataRows.GroupBy(dr => Int32.Parse(String.IsNullOrEmpty(dr[attributeColIndexMappings[columnName]].ToString()) ? "0" : dr[attributeColIndexMappings[columnName]].ToString())).Select(dr => dr.First()).ToList();

                    var sequence = 0;

                    foreach (var row in rows)
                    {
                        IAttributeCollection iAttributeCollection = hierarchialAttribute.NewComplexChildRecord();
                        var childInstanceAttribute = (Attribute)hierarchialAttribute.AddComplexChildRecord(iAttributeCollection);
                        childInstanceAttribute.Id = attributeModel.Id;

                        var rowsForSequence = dataRows.Where(dr => Int32.Parse(String.IsNullOrEmpty(dr[attributeColIndexMappings[columnName]].ToString()) ? "0" : dr[attributeColIndexMappings[columnName]].ToString()) == sequence).ToList();
                        
                        BuildComplexHierarchialAttributeCollection(complexChildAttributeList, rowsForSequence, attributeModel.GetChildAttributeModels(), childInstanceAttribute, attributeColIndexMappings, columnName, sequence);

                        //This needs to be done to remove the values object at instance level.  The action flag on this by default set to "Read"  this causes
                        //the hierarchial attribute to be not created.
                        //childInstanceAttribute.OverriddenValues.Clear();
                        SetOverridenValuesAction(childInstanceAttribute, ObjectAction.Create);

                        sequence++;
                    }
                }
                else
                {
                    IAttributeCollection iAttributeCollection = hierarchialAttribute.NewComplexChildRecord();
                    var childInstanceAttribute = (Attribute)hierarchialAttribute.AddComplexChildRecord(iAttributeCollection);
                    childInstanceAttribute.Id = attributeModel.Id;

                    BuildComplexHierarchialAttributeCollection(complexChildAttributeList, dataRows, attributeModel.GetChildAttributeModels(), childInstanceAttribute, attributeColIndexMappings, columnName, 0);

                    //This needs to be done to remove the values object at instance level.  The action flag on this by default set to "Read"  this causes
                    //the hierarchial attribute to be not created. 
                    //childInstanceAttribute.OverriddenValues.Clear();
                    SetOverridenValuesAction(childInstanceAttribute, ObjectAction.Create);
                }
            }
        }

        private void SetOverridenValuesAction(Attribute childInstanceAttribute, ObjectAction action)
        {
            foreach (var value in childInstanceAttribute.OverriddenValues)
            {
                value.Action = action;
            }
        }

        private LocaleEnum GetLocaleFromAttributeName(string attributeName)
        {
            var locale = _defaultLocale;

            String[] arrSeprators = { "//" };

            //Consider only last [] value to start with
            attributeName = attributeName.Substring(attributeName.LastIndexOf('[') + 1);

            attributeName = attributeName.Replace("]", String.Empty);

            String[] fieldComponents = attributeName.Split(arrSeprators, StringSplitOptions.None);

            if (fieldComponents.Length == 3)
            {
                String strlocale = fieldComponents[2];

                //try to parse only when this value is valid, otherwise TryParse will assign default value
                if (Enum.IsDefined(typeof(LocaleEnum), strlocale))
                {
                    Enum.TryParse<LocaleEnum>(strlocale, out locale);
                }
            }

            return locale;
        }

        private void ProcessComplexAttributes(DataRowCollection dataRows, Dictionary<String, Int32> attributeColIndexMappings, string complexSheetName, List<Entity> entitiesTobeProcessed, ref Attribute complexAttribute)
        {
            Int64 complexCounter = 1;
            foreach (DataRow dataRow in dataRows)
            {
                complexCounter++;
                Attribute complexAttributeBase = complexAttribute.Clone();
                var entity = PrepareEntityFromDataRow(dataRow, complexSheetName, complexCounter, entitiesTobeProcessed);

                #region Prepare parent complex Attribute


                //Assign complex Attribute Locale
                complexAttributeBase.Locale = _defaultLocale;
                complexAttributeBase.IsComplex = true;

                #endregion

                if (
                  !(String.IsNullOrWhiteSpace(complexAttributeBase.Name) ||
                    String.IsNullOrWhiteSpace(complexAttributeBase.AttributeParentName)))
                {
                    AttributeUniqueIdentifier attrUniqueIdentifier = new AttributeUniqueIdentifier(complexAttributeBase.Name,
                      complexAttributeBase.AttributeParentName);
                    IAttribute filteredAttr = entity.GetAttribute(attrUniqueIdentifier, _defaultLocale);

                    #region Populate Complex child Attributes

                    if (filteredAttr != null)
                    {

                        complexAttributeBase = (Attribute)filteredAttr;
                        complexAttributeBase.AddComplexChildRecord(BuildComplexAttributeCollection(_complexChildAttributeList, dataRow, complexAttributeBase.Name, attributeColIndexMappings, false));

                    }
                    else
                    {
                        #region Create Instance Record

                        Attribute instanceAttribute = complexAttributeBase.CloneBasicProperties();
                        instanceAttribute.Name = String.Format("{0} Instance Record", complexAttributeBase.Name);
                        instanceAttribute.LongName = String.Format("{0} Instance Record", complexAttributeBase.LongName);

                        MDM.BusinessObjects.Value attributeValue = new MDM.BusinessObjects.Value();
                        attributeValue.Action = ObjectAction.Read;
                        attributeValue.Locale = _defaultLocale;
                        attributeValue.ValueRefId = -1;
                        attributeValue.Sequence = -1;

                        //For first level InstanceRefId & sequence will be 0
                        instanceAttribute.InstanceRefId = attributeValue.ValueRefId;
                        instanceAttribute.Sequence = attributeValue.Sequence;

                        complexAttributeBase.SetValue(attributeValue);

                        complexAttributeBase.Attributes.Add(instanceAttribute);

                        #endregion

                        instanceAttribute.Attributes.AddRange(BuildComplexAttributeCollection(_complexChildAttributeList, dataRow, complexAttributeBase.Name, attributeColIndexMappings, true));
                        complexAttributeBase.IsCollection = true; //Assume complex attribute will be collection.

                        if (entity != null)
                        {
                            entity.Attributes.Add(complexAttributeBase);
                        }
                    }

                    #endregion
                }
            }
        }

        private Entity PrepareEntityFromDataRow(DataRow dataRow, String complexSheetName, Int64 complexCounter, List<Entity> entitiesTobeProcessed)
        {
            Entity entity = new Entity();
            #region Read Entity properties

            if (this._dtComplexAttribues.Columns.Contains(RSExcelConstants.ComplexAttributeTemplateColumns[ComplexAttributeTemplateFieldEnum.Id]))
            {
                String rowLevelRefId = dataRow[RSExcelConstants.ComplexAttributeTemplateColumns[ComplexAttributeTemplateFieldEnum.Id]].ToString();

                if (!String.IsNullOrWhiteSpace(rowLevelRefId))
                {
                    entity.Id = ValueTypeHelper.Int32TryParse(rowLevelRefId, -1);
                }
            }

            if (this._dtComplexAttribues.Columns.Contains(RSExcelConstants.ComplexAttributeTemplateColumns[ComplexAttributeTemplateFieldEnum.ExtenalId]))
            {
                String rowLevelExternalId = dataRow[RSExcelConstants.ComplexAttributeTemplateColumns[ComplexAttributeTemplateFieldEnum.ExtenalId]].ToString();

                if (!String.IsNullOrWhiteSpace(rowLevelExternalId))
                {
                    entity.ExternalId = rowLevelExternalId;
                    entity.Name = rowLevelExternalId;
                }
            }

            if (this._dtComplexAttribues.Columns.Contains(RSExcelConstants.ComplexAttributeTemplateColumns[ComplexAttributeTemplateFieldEnum.LongName]))
            {
                String rowLevelLongName = dataRow[RSExcelConstants.ComplexAttributeTemplateColumns[ComplexAttributeTemplateFieldEnum.LongName]].ToString();

                if (!String.IsNullOrWhiteSpace(rowLevelLongName))
                {
                    entity.LongName = rowLevelLongName;
                }
            }

            String entityType = RSExcelConstants.MetadataTemplateFields[MetadataTemplateFieldEnum.DefaultEntityType].Value;
            if (this._dtComplexAttribues.Columns.Contains(RSExcelConstants.ComplexAttributeTemplateColumns[ComplexAttributeTemplateFieldEnum.EntityType]))
            {
                String rowLevelEntityType = dataRow[RSExcelConstants.ComplexAttributeTemplateColumns[ComplexAttributeTemplateFieldEnum.EntityType]].ToString();

                if (!String.IsNullOrWhiteSpace(rowLevelEntityType))
                    entityType = rowLevelEntityType;
            }
            entity.EntityTypeName = entityType;

            String categoryPath = RSExcelConstants.MetadataTemplateFields[MetadataTemplateFieldEnum.DefaultCateogryPath].Value;
            if (this._dtComplexAttribues.Columns.Contains(RSExcelConstants.ComplexAttributeTemplateColumns[ComplexAttributeTemplateFieldEnum.CategoryPath]))
            {
                String rowLevelCategoryPath = dataRow[RSExcelConstants.ComplexAttributeTemplateColumns[ComplexAttributeTemplateFieldEnum.CategoryPath]].ToString();

                if (!String.IsNullOrWhiteSpace(rowLevelCategoryPath))
                    categoryPath = rowLevelCategoryPath;
            }
            entity.CategoryPath = categoryPath;

            String containerName = RSExcelConstants.MetadataTemplateFields[MetadataTemplateFieldEnum.DefaultContainer].Value; ;
            if (this._dtComplexAttribues.Columns.Contains(RSExcelConstants.ComplexAttributeTemplateColumns[ComplexAttributeTemplateFieldEnum.Container]))
            {
                String rowLevelContainerName = dataRow[RSExcelConstants.ComplexAttributeTemplateColumns[ComplexAttributeTemplateFieldEnum.Container]].ToString();

                if (!String.IsNullOrWhiteSpace(rowLevelContainerName))
                    containerName = rowLevelContainerName;
            }
            entity.ContainerName = containerName;

            String organizationName = RSExcelConstants.MetadataTemplateFields[MetadataTemplateFieldEnum.DefaultOrganization].Value; ;
            if (this._dtComplexAttribues.Columns.Contains(RSExcelConstants.ComplexAttributeTemplateColumns[ComplexAttributeTemplateFieldEnum.Organization]))
            {
                String rowLevelOrganizationName = dataRow[RSExcelConstants.ComplexAttributeTemplateColumns[ComplexAttributeTemplateFieldEnum.Organization]].ToString();

                if (!String.IsNullOrWhiteSpace(rowLevelOrganizationName))
                    organizationName = rowLevelOrganizationName;
            }
            entity.OrganizationName = organizationName;

            Boolean entityHasInvalidLocale = false;
            if (this._dtComplexAttribues.Columns.Contains(RSExcelConstants.ComplexAttributeTemplateColumns[ComplexAttributeTemplateFieldEnum.Locale]))
            {
                if (
                    !Enum.TryParse<LocaleEnum>(
                        dataRow[RSExcelConstants.ComplexAttributeTemplateColumns[ComplexAttributeTemplateFieldEnum.Locale]].ToString(),
                        out _defaultLocale))
                {
                    entityHasInvalidLocale = true;
                }
            }

            #endregion

            #region Prepare Entity object

            IEnumerable<Entity> filteredEntities = from ent in entitiesTobeProcessed
                                                   where (ent.ExternalId.Equals(entity.ExternalId, StringComparison.InvariantCultureIgnoreCase)
                                                   && ent.EntityTypeName.Equals(entity.EntityTypeName, StringComparison.InvariantCultureIgnoreCase)
                                                   && ent.CategoryPath.Equals(entity.CategoryPath, StringComparison.InvariantCultureIgnoreCase)
                                                   && ent.ContainerName.Equals(entity.ContainerName, StringComparison.InvariantCultureIgnoreCase)
                                                   && ent.OrganizationName.Equals(entity.OrganizationName, StringComparison.InvariantCultureIgnoreCase))
                                                   select ent;

            if (filteredEntities.Any())
            {
                entity = filteredEntities.FirstOrDefault();
            }
            else
            {
                entity.Action = ObjectAction.Unknown;
                entity.ReferenceId = complexCounter;//To get the line in excel sheet where this error occurred.
                entity.ExtendedProperties = String.Format("SourceDataReadError#@@#{0}", complexSheetName);
                entitiesTobeProcessed.Add(entity);
            }

            if (entityHasInvalidLocale)
            {
                entity.Locale = LocaleEnum.UnKnown;
            }

            #endregion

            return entity;
        }

        private void FillEntityProperties(Entity entity, DataRow dataRow)
        {
            DiagnosticActivity activity = new DiagnosticActivity();
            if (_traceSettings.IsBasicTracingEnabled) activity.Start();

            if (_defaultLocale != LocaleEnum.UnKnown)
            {
                entity.Locale = _defaultLocale;
            }

            if (this._dtEntities.Columns.Contains(RSExcelConstants.EntityDataTemplateColumns[EntityDataTemplateFieldEnum.Id]))
            {
                String rowLevelRefId = dataRow[RSExcelConstants.EntityDataTemplateColumns[EntityDataTemplateFieldEnum.Id]].ToString();

                if (!String.IsNullOrWhiteSpace(rowLevelRefId))
                {
                    entity.ReferenceId = ValueTypeHelper.Int64TryParse(rowLevelRefId, -1);
                }
            }

            if (this._dtEntities.Columns.Contains(RSExcelConstants.EntityDataTemplateColumns[EntityDataTemplateFieldEnum.ExtenalId]))
            {
                String rowLevelExternalId = dataRow[RSExcelConstants.EntityDataTemplateColumns[EntityDataTemplateFieldEnum.ExtenalId]].ToString();

                if (!String.IsNullOrWhiteSpace(rowLevelExternalId))
                {
                    entity.ExternalId = rowLevelExternalId;
                    entity.Name = rowLevelExternalId;
                }
            }

            if (this._dtEntities.Columns.Contains(RSExcelConstants.EntityDataTemplateColumns[EntityDataTemplateFieldEnum.LongName]))
            {
                String rowLevelLongName = dataRow[RSExcelConstants.EntityDataTemplateColumns[EntityDataTemplateFieldEnum.LongName]].ToString();

                if (!String.IsNullOrWhiteSpace(rowLevelLongName))
                {
                    entity.LongName = rowLevelLongName;
                }
            }

            String entityType = RSExcelConstants.MetadataTemplateFields[MetadataTemplateFieldEnum.DefaultEntityType].Value;
            if (this._dtEntities.Columns.Contains(RSExcelConstants.EntityDataTemplateColumns[EntityDataTemplateFieldEnum.EntityType]))
            {
                String rowLevelEntityType = dataRow[RSExcelConstants.EntityDataTemplateColumns[EntityDataTemplateFieldEnum.EntityType]].ToString();

                if (!String.IsNullOrWhiteSpace(rowLevelEntityType))
                    entityType = rowLevelEntityType;
            }
            entity.EntityTypeName = entityType;

            String categoryPath = RSExcelConstants.MetadataTemplateFields[MetadataTemplateFieldEnum.DefaultCateogryPath].Value;
            if (this._dtEntities.Columns.Contains(RSExcelConstants.EntityDataTemplateColumns[EntityDataTemplateFieldEnum.CategoryPath]))
            {
                String rowLevelCategoryPath = dataRow[RSExcelConstants.EntityDataTemplateColumns[EntityDataTemplateFieldEnum.CategoryPath]].ToString();

                if (!String.IsNullOrWhiteSpace(rowLevelCategoryPath))
                    categoryPath = rowLevelCategoryPath;
            }
            entity.CategoryPath = categoryPath;

            if (this._dtEntities.Columns.Contains(RSExcelConstants.EntityDataTemplateColumns[EntityDataTemplateFieldEnum.TargetCategoryPath]))
            {
                String targetCategoryPath = dataRow[RSExcelConstants.EntityDataTemplateColumns[EntityDataTemplateFieldEnum.TargetCategoryPath]].ToString();

                if (!String.IsNullOrWhiteSpace(targetCategoryPath))
                {
                    if (entity.EntityMoveContext == null)
                    {
                        entity.EntityMoveContext = new EntityMoveContext();
                    }

                    entity.EntityMoveContext.TargetCategoryPath = targetCategoryPath;
                }
            }

            String containerName = RSExcelConstants.MetadataTemplateFields[MetadataTemplateFieldEnum.DefaultContainer].Value; ;
            if (this._dtEntities.Columns.Contains(RSExcelConstants.EntityDataTemplateColumns[EntityDataTemplateFieldEnum.Container]))
            {
                String rowLevelContainerName = dataRow[RSExcelConstants.EntityDataTemplateColumns[EntityDataTemplateFieldEnum.Container]].ToString();

                if (!String.IsNullOrWhiteSpace(rowLevelContainerName))
                    containerName = rowLevelContainerName;
            }
            entity.ContainerName = containerName;

            String organizationName = RSExcelConstants.MetadataTemplateFields[MetadataTemplateFieldEnum.DefaultOrganization].Value; ;
            if (this._dtEntities.Columns.Contains(RSExcelConstants.EntityDataTemplateColumns[EntityDataTemplateFieldEnum.Organization]))
            {
                String rowLevelOrganizationName = dataRow[RSExcelConstants.EntityDataTemplateColumns[EntityDataTemplateFieldEnum.Organization]].ToString();

                if (!String.IsNullOrWhiteSpace(rowLevelOrganizationName))
                    organizationName = rowLevelOrganizationName;
            }
            entity.OrganizationName = organizationName;

            if (this._dtEntities.Columns.Contains(RSExcelConstants.EntityDataTemplateColumns[EntityDataTemplateFieldEnum.ParentExternalId]))
            {
                String rowLevelHierarchyParentExternalId = dataRow[RSExcelConstants.EntityDataTemplateColumns[EntityDataTemplateFieldEnum.ParentExternalId]].ToString();

                if (!String.IsNullOrWhiteSpace(rowLevelHierarchyParentExternalId))
                {
                    entity.ParentExternalId = rowLevelHierarchyParentExternalId;
                }
            }

            if (this._dtEntities.Columns.Contains(RSExcelConstants.EntityDataTemplateColumns[EntityDataTemplateFieldEnum.ParentExtensionExternalId]))
            {
                String rowLevelExtensionParentExternalId = dataRow[RSExcelConstants.EntityDataTemplateColumns[EntityDataTemplateFieldEnum.ParentExtensionExternalId]].ToString();

                if (!String.IsNullOrWhiteSpace(rowLevelExtensionParentExternalId))
                {
                    entity.ParentExtensionEntityExternalId = rowLevelExtensionParentExternalId;
                }
            }

            String parentExtensionCategoryPath = RSExcelConstants.MetadataTemplateFields[MetadataTemplateFieldEnum.DefaultParentExtensionCategoryPath].Value; ;
            if (this._dtEntities.Columns.Contains(RSExcelConstants.EntityDataTemplateColumns[EntityDataTemplateFieldEnum.ParentExtensionCategoryPath]))
            {
                String rowLevelParentExtensionCategoryPath = dataRow[RSExcelConstants.EntityDataTemplateColumns[EntityDataTemplateFieldEnum.ParentExtensionCategoryPath]].ToString();

                if (!String.IsNullOrWhiteSpace(rowLevelParentExtensionCategoryPath))
                    parentExtensionCategoryPath = rowLevelParentExtensionCategoryPath;
            }
            entity.ParentExtensionEntityCategoryPath = parentExtensionCategoryPath;

            String parentExtensionContainer = RSExcelConstants.MetadataTemplateFields[MetadataTemplateFieldEnum.DefaultParentExtensionContainer].Value; ;
            if (this._dtEntities.Columns.Contains(RSExcelConstants.EntityDataTemplateColumns[EntityDataTemplateFieldEnum.ParentExtensionContainer]))
            {
                String rowLevelParentExtensionContainer = dataRow[RSExcelConstants.EntityDataTemplateColumns[EntityDataTemplateFieldEnum.ParentExtensionContainer]].ToString();

                if (!String.IsNullOrWhiteSpace(rowLevelParentExtensionContainer))
                    parentExtensionContainer = rowLevelParentExtensionContainer;
            }
            entity.ParentExtensionEntityContainerName = parentExtensionContainer;

            if (entity.EntityTypeName.Equals("Category", StringComparison.InvariantCultureIgnoreCase))
                entity.BranchLevel = ContainerBranchLevel.Node;
            else
                entity.BranchLevel = ContainerBranchLevel.Component;

            if (this._dtEntities.Columns.Contains(RSExcelConstants.EntityDataTemplateColumns[EntityDataTemplateFieldEnum.Action]))
            {
                String action = dataRow[RSExcelConstants.EntityDataTemplateColumns[EntityDataTemplateFieldEnum.Action]].ToString();

                if (!String.IsNullOrWhiteSpace(action))
                {
                    ObjectAction entityAction = ObjectAction.Unknown;
                    Enum.TryParse<ObjectAction>(action, true, out entityAction);

                    if (entityAction != ObjectAction.Unknown)
                    {
                        entity.Action = entityAction;
                    }
                }
            }

            if (_traceSettings.IsBasicTracingEnabled)
            {
                activity.LogInformation(string.Format("Fill entity properties complete for entity ({0}).", entity.Name));
                activity.Stop();
            }
        }

        private void FillEntityAttributes(Entity entity, DataRow dataRow, Dictionary<String, Int32> attributeColIndexMappings)
        {
            DiagnosticActivity activity = new DiagnosticActivity();
            if (_traceSettings.IsBasicTracingEnabled) activity.Start();

            AttributeCollection attributeCollection = BuildAttributeCollection(_entityAttributesList, dataRow, attributeColIndexMappings);

            entity.Attributes = attributeCollection;

            if (_traceSettings.IsBasicTracingEnabled)
            {
                activity.LogInformation(string.Format("Fill entity attributes complete for entity ({0}).", entity.Name));
                activity.Stop();
            }
        }

        private void FillRelationshipProperties(Relationship relationship, DataRow dataRow)
        {
            DiagnosticActivity activity = new DiagnosticActivity();
            if (_traceSettings.IsBasicTracingEnabled) activity.Start();

            if (this._dtRelationships.Columns.Contains(RSExcelConstants.RelationshipDataTemplateColumns[RelationshipDataTemplateFieldEnum.RelationshipExternalId]))
            {
                String rowLevelRelationshipExternalId = dataRow[RSExcelConstants.RelationshipDataTemplateColumns[RelationshipDataTemplateFieldEnum.RelationshipExternalId]].ToString();

                if (!String.IsNullOrWhiteSpace(rowLevelRelationshipExternalId))
                {
                    relationship.RelationshipExternalId = rowLevelRelationshipExternalId;
                }
            }

            if (this._dtRelationships.Columns.Contains(RSExcelConstants.RelationshipDataTemplateColumns[RelationshipDataTemplateFieldEnum.RelationshipType]))
            {
                String rowLevelRelationshipType = dataRow[RSExcelConstants.RelationshipDataTemplateColumns[RelationshipDataTemplateFieldEnum.RelationshipType]].ToString();

                if (!String.IsNullOrWhiteSpace(rowLevelRelationshipType))
                {
                    relationship.RelationshipTypeName = rowLevelRelationshipType;
                }
            }

            if (this._dtRelationships.Columns.Contains(RSExcelConstants.RelationshipDataTemplateColumns[RelationshipDataTemplateFieldEnum.ToEntityExternalId]))
            {
                String rowLevelToEntityExternalId = dataRow[RSExcelConstants.RelationshipDataTemplateColumns[RelationshipDataTemplateFieldEnum.ToEntityExternalId]].ToString();

                if (!String.IsNullOrWhiteSpace(rowLevelToEntityExternalId))
                {
                    relationship.ToExternalId = rowLevelToEntityExternalId;
                }
            }

            String toEntityEntityType = RSExcelConstants.MetadataTemplateFields[MetadataTemplateFieldEnum.DefaultToEntityEntityType].Value;
            if (this._dtRelationships.Columns.Contains(RSExcelConstants.RelationshipDataTemplateColumns[RelationshipDataTemplateFieldEnum.ToEntityEntityType]))
            {
                String rowLevelToEntityType = dataRow[RSExcelConstants.RelationshipDataTemplateColumns[RelationshipDataTemplateFieldEnum.ToEntityEntityType]].ToString();

                if (!String.IsNullOrWhiteSpace(rowLevelToEntityType))
                {
                    toEntityEntityType = rowLevelToEntityType;
                }
            }
            relationship.ToEntityTypeName = toEntityEntityType;

            String toEntityCategoryPath = RSExcelConstants.MetadataTemplateFields[MetadataTemplateFieldEnum.DefaultToEntityCateogryPath].Value;
            if (this._dtRelationships.Columns.Contains(RSExcelConstants.RelationshipDataTemplateColumns[RelationshipDataTemplateFieldEnum.ToEntityCategoryPath]))
            {
                String rowLevelToEntityCategoryPath = dataRow[RSExcelConstants.RelationshipDataTemplateColumns[RelationshipDataTemplateFieldEnum.ToEntityCategoryPath]].ToString();

                if (!String.IsNullOrWhiteSpace(rowLevelToEntityCategoryPath))
                {
                    toEntityCategoryPath = rowLevelToEntityCategoryPath;
                }
            }
            relationship.ToCategoryPath = toEntityCategoryPath;

            String toEntityContainer = RSExcelConstants.MetadataTemplateFields[MetadataTemplateFieldEnum.DefaultToEntityContainer].Value; ;
            if (this._dtRelationships.Columns.Contains(RSExcelConstants.RelationshipDataTemplateColumns[RelationshipDataTemplateFieldEnum.ToEntityContainer]))
            {
                String rowLevelToEntityContainer = dataRow[RSExcelConstants.RelationshipDataTemplateColumns[RelationshipDataTemplateFieldEnum.ToEntityContainer]].ToString();

                if (!String.IsNullOrWhiteSpace(rowLevelToEntityContainer))
                {
                    toEntityContainer = rowLevelToEntityContainer;
                }
            }
            relationship.ToContainerName = toEntityContainer;

            String toEntityOrganization = RSExcelConstants.MetadataTemplateFields[MetadataTemplateFieldEnum.DefaultToEntityOrganization].Value; ;
            if (this._dtRelationships.Columns.Contains(RSExcelConstants.RelationshipDataTemplateColumns[RelationshipDataTemplateFieldEnum.ToEntityOrganization]))
            {
                String rowLevelToEntityOrganization = dataRow[RSExcelConstants.RelationshipDataTemplateColumns[RelationshipDataTemplateFieldEnum.ToEntityOrganization]].ToString();

                if (!String.IsNullOrWhiteSpace(rowLevelToEntityOrganization))
                {
                    toEntityOrganization = rowLevelToEntityOrganization;
                }
            }

            if (this._dtRelationships.Columns.Contains(RSExcelConstants.RelationshipDataTemplateColumns[RelationshipDataTemplateFieldEnum.Action]))
            {
                String action = dataRow[RSExcelConstants.RelationshipDataTemplateColumns[RelationshipDataTemplateFieldEnum.Action]].ToString();

                if (!String.IsNullOrWhiteSpace(action))
                {
                    ObjectAction relationshipAction = ObjectAction.Unknown;
                    Enum.TryParse<ObjectAction>(action, true, out relationshipAction);

                    if (relationshipAction != ObjectAction.Unknown)
                    {
                        relationship.Action = relationshipAction;
                    }
                }
            }

            //TODO:: how to give org name in relationship object

            if (_traceSettings.IsBasicTracingEnabled)
            {
                activity.LogInformation(string.Format("Fill relationship properties complete for relationship ({0})", relationship.Name));
                activity.Stop();
            }
        }

        private void FillRelationshipAttributes(Relationship relationship, DataRow dataRow, Dictionary<String, Int32> attributeColIndexMappings)
        {
            DiagnosticActivity activity = new DiagnosticActivity();
            if (_traceSettings.IsBasicTracingEnabled) activity.Start();

            AttributeCollection attributeCollection = BuildAttributeCollection(_relationshipAttributesList, dataRow, attributeColIndexMappings);

            relationship.RelationshipAttributes = attributeCollection;

            if (_traceSettings.IsBasicTracingEnabled)
            {
                activity.LogInformation(string.Format("Fill relationship attributes complete for relationship ({0})", relationship.Name));
                activity.Stop();
            }
        }

        private Dictionary<String, Int32> GetAttributeColumnIndexMappings(String filename, string sheetname, Collection<String> attributesListToFill, List<String> sheetNamesDictionary)
        {
            // dictionary attributename and column index mappings
            Dictionary<String, Int32> attributeMappings = new Dictionary<String, Int32>();

            using (SpreadsheetDocument document = SpreadsheetDocument.Open(filename, true))
            {
                WorkbookPart workbookPart = document.WorkbookPart;

                //Reading the sheet
                var sheets = workbookPart.Workbook.Descendants<Sheet>();
                var sheet = sheets.Where(s => s.Name == sheetname).FirstOrDefault();


                if (sheet != null)
                {
                    var workSheet = ((WorksheetPart)workbookPart
                        .GetPartById(sheet.Id)).Worksheet;

                    var sheetData = workSheet.Elements<SheetData>().First();

                    // reading rows
                    List<DocumentFormat.OpenXml.Spreadsheet.Row> rows = sheetData.Elements<DocumentFormat.OpenXml.Spreadsheet.Row>().ToList();
                    if (rows.Count > 0)
                    {
                        var row = rows[0];
                        Int32 colIndex = 0;

                        // reading cells and making the dictionary
                        foreach (DocumentFormat.OpenXml.Spreadsheet.Cell cell in row.Descendants<DocumentFormat.OpenXml.Spreadsheet.Cell>())
                        {
                            // ACE Driver reads only 255 cols from excel sheet so not considering cells after 255 index
                            if (!_isSAXParserEnabled && colIndex >= 255)
                            {
                                break;
                            }

                            var text = ReadExcelCell(cell, workbookPart);

                            if (!String.IsNullOrWhiteSpace(text))
                            {
                                if (!attributeMappings.ContainsKey(text))
                                {
                                    attributeMappings.Add(text, colIndex);
                                }
                                else
                                {
                                    attributeMappings[text] = colIndex;

                                }

                                if (!sheetNamesDictionary.Contains(text))
                                {
                                    attributesListToFill.Add(text);
                                }
                            }

                            colIndex++;
                        }
                    }
                }
            }

            return attributeMappings;
        }

        private String ReadExcelCell(DocumentFormat.OpenXml.Spreadsheet.Cell cell, WorkbookPart workbookPart)
        {
            var cellValue = cell.CellValue;
            var text = (cellValue == null) ? cell.InnerText : cellValue.Text;
            if ((cell.DataType != null) && (cell.DataType == CellValues.SharedString))
            {
                text = workbookPart.SharedStringTablePart.SharedStringTable
                    .Elements<SharedStringItem>().ElementAt(
                        Convert.ToInt32(cell.CellValue.Text)).InnerText;
            }

            return (text ?? string.Empty).Trim();
        }

        private AttributeCollection BuildAttributeCollection(Collection<String> attributeDataColumnNames, DataRow dataRow, Dictionary<String, Int32> attributeColIndexMappings)
        {
            AttributeCollection attributeCollection = new AttributeCollection();

            foreach (String attributeColumnName in attributeDataColumnNames)
            {
                String[] arrSeprators = new String[] { "//" };
                int attrColumnIndex = -1;

                if (attributeColIndexMappings != null && attributeColIndexMappings.ContainsKey(attributeColumnName))
                {
                    attrColumnIndex = attributeColIndexMappings[attributeColumnName];

                }
                else
                {
                    continue;
                }

                String[] arrAttributeHeader = attributeColumnName.Split(arrSeprators, StringSplitOptions.None);

                if ((!String.IsNullOrWhiteSpace(dataRow[attrColumnIndex].ToString())) && (arrAttributeHeader != null))
                {
                    MDM.BusinessObjects.Attribute attribute = new Attribute();
                    attribute.Locale = _defaultLocale;

                    if (arrAttributeHeader.Length == 2)
                    {
                        LocaleEnum dataLocale = _defaultLocale;

                        MDM.BusinessObjects.Value value = new MDM.BusinessObjects.Value();
                        String attrVal = dataRow[attrColumnIndex].ToString();
                        value.AttrVal = attrVal;

                        if (Enum.TryParse(arrAttributeHeader[1].ToString(), out dataLocale))
                        {
                            attribute.Name = arrAttributeHeader[0];
                            attribute.LongName = arrAttributeHeader[0];

                            attribute.Locale = value.Locale = dataLocale;
                        }
                        else
                        {
                            attribute.Name = arrAttributeHeader[1];
                            attribute.LongName = arrAttributeHeader[1];
                            attribute.AttributeParentName = arrAttributeHeader[0];
                            attribute.AttributeParentLongName = arrAttributeHeader[0];

                            attribute.Locale = value.Locale = _defaultLocale;
                        }

                        attribute.SetValue((IValue)value);
                        attributeCollection.Add(attribute);
                    }

                    if (arrAttributeHeader.Length == 1)
                    {
                        attribute.Name = arrAttributeHeader[0];
                        attribute.LongName = arrAttributeHeader[0];
                        // we do not have the parent name. The idea is to find the attribute with just the name...
                        // the import engine will find with name first and only then use the parent name..
                        attribute.AttributeParentName = arrAttributeHeader[0];
                        attribute.AttributeParentLongName = arrAttributeHeader[0];

                        attribute.Locale = _defaultLocale;

                        MDM.BusinessObjects.Value value = new MDM.BusinessObjects.Value();
                        String attrVal = dataRow[attrColumnIndex].ToString();
                        value.AttrVal = attrVal;
                        value.Locale = _defaultLocale;

                        attribute.SetValue((IValue)value);
                        attributeCollection.Add(attribute);
                    }

                    if (arrAttributeHeader.Length == 3)
                    {
                        // This is for a future scenario, when a locale will be added to the name.
                        attribute.AttributeParentName = arrAttributeHeader[0];
                        attribute.AttributeParentLongName = arrAttributeHeader[0];
                        attribute.Name = arrAttributeHeader[1];
                        attribute.LongName = arrAttributeHeader[1];

                        String strlocale = arrAttributeHeader[2];

                        LocaleEnum locale = LocaleEnum.UnKnown;
                        //try to parse only when this value is valid, otherwise TryParse will assign default value
                        if (Enum.IsDefined(typeof(LocaleEnum), strlocale))
                        {
                            Enum.TryParse<LocaleEnum>(strlocale, out locale);
                        }
                        attribute.Locale = locale;

                        MDM.BusinessObjects.Value value = new MDM.BusinessObjects.Value();
                        String attrVal = dataRow[attrColumnIndex].ToString();
                        value.AttrVal = attrVal;
                        value.Locale = locale;

                        attribute.SetValue((IValue)value);
                        attributeCollection.Add(attribute);
                    }
                }
            }

            return attributeCollection;
        }

        private AttributeCollection BuildComplexAttributeCollection(Collection<String> attributeDataColumnNames, DataRow dataRow, String complexAttrGroupName, Dictionary<String, Int32> complexAttrIndexMappings, Boolean isFirstChild = false)
        {
            AttributeCollection attributeCollection = new AttributeCollection();

            foreach (String attributeColumnName in attributeDataColumnNames)
            {
                int columnIndex = -1;
                if (complexAttrIndexMappings != null && complexAttrIndexMappings.ContainsKey(attributeColumnName))
                {
                    columnIndex = complexAttrIndexMappings[attributeColumnName];
                }
                else
                {
                    continue;
                }

                //Remove NullOrWhiteSpace validation for fix issue related with task #196373
                //Dont put entry for blank attribute
                //if (!String.IsNullOrWhiteSpace(dataRow[columnIndex].ToString()))
                //{
                Attribute attribute = new Attribute();

                attribute.Name = attributeColumnName;
                attribute.LongName = attributeColumnName;
                attribute.AttributeParentName = complexAttrGroupName;

                if (isFirstChild)
                {
                    //If complex attribute adding first child then add the sequence and InstanceRefId as 0.
                    attribute.Sequence = 0;
                    attribute.InstanceRefId = 0;
                }
                attribute.Locale = _defaultLocale;

                String attrVal = dataRow[columnIndex].ToString();
                if (!String.IsNullOrWhiteSpace(attrVal))
                {
                    MDM.BusinessObjects.Value value = new MDM.BusinessObjects.Value();                    
                    value.AttrVal = attrVal;
                    attribute.SetValue((IValue)value);
                }
                else
                {
                    //Setting the attribute action as ignore when the value cell under the attribute in the excel sheet is blank                
                    attribute.Action = ObjectAction.Ignore;
                }

                attributeCollection.Add(attribute);
                //}
            }

            return attributeCollection;
        }

        private String GetAttributeColumnName(AttributeModel attributeModel, Dictionary<String, Int32> attributeColIndexMappings)
        {
            String columnName = String.Empty;

            if (attributeColIndexMappings != null)
            {
                //AttributeAndAttributeParentShortName
                columnName = String.Format("{0}//{1}", attributeModel.AttributeParentName, attributeModel.Name);

                if (!attributeColIndexMappings.ContainsKey(columnName))
                {
                    //AttributeAndAttributeParentShortNameWithLocale
                    columnName = String.Format("{0}//{1}//{2}", attributeModel.AttributeParentName, attributeModel.Name, attributeModel.Locale);
                }

                if (!attributeColIndexMappings.ContainsKey(columnName))
                {
                    //ShortName
                    columnName = attributeModel.Name;
                }

                if (!attributeColIndexMappings.ContainsKey(columnName))
                {
                    //LongName
                    columnName = attributeModel.LongName;
                }

                if (!attributeColIndexMappings.ContainsKey(columnName))
                {
                    //AttributeAndAttributeParentLongName
                    columnName = String.Format("{0}//{1}", attributeModel.AttributeParentName, attributeModel.LongName);
                }

                if (!attributeColIndexMappings.ContainsKey(columnName))
                {
                    //AttributeAndAttributeParentLongNameWithLocale
                    columnName = String.Format("{0}//{1}//{2}", attributeModel.AttributeParentName, attributeModel.LongName, attributeModel.Locale);
                }

                if (!attributeColIndexMappings.ContainsKey(columnName))
                {
                    columnName = String.Empty;
                }
            }

            return columnName;
        }
       
        #endregion

        #endregion
    }
}