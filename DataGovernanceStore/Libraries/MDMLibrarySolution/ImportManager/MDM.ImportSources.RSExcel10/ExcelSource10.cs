using System;
using System.Xml;
using System.Xml.Schema;
using System.Linq;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace MDM.ImportSources.RSExcel
{
    using MDM.BusinessObjects;
    using MDM.Core;
    using MDM.Imports.Interfaces;
    using MDM.BusinessObjects.Jobs;
    using MDM.BusinessObjects.Imports;
    using MDM.Interfaces;
    using DocumentFormat.OpenXml.Packaging;
    using DocumentFormat.OpenXml.Spreadsheet;
    using MDM.ExcelUtility;
    using MDM.Utility;

    /// <summary>
    /// This class implements the source data from a RS Excel source. It reads the data in to a data set and process them as per the request from the import engine.
    /// </summary>
    public class ExcelSource10 : IEntityImportSourceData
    {
        #region Fields

        private IJobResultHandler _jobResultHandler = null;

        /// <summary>
        /// Single forward only reader needs to be synchronized.
        /// </summary>
        private Object readerLock = new Object();

        ImportProviderBatchingType _batchingType = ImportProviderBatchingType.Single;

        private String _sourceFile = string.Empty;

        private String _schemaFilePath = string.Empty;

        private String _errorMessage = string.Empty;

        private Int32 _batchSize = 0;

        private Hashtable _complexSheetMappings = new Hashtable();

        private String _metadataSheetName = "Metadata";
        //private String _attributeMetadataSheetName = "Attribute Metadata";
        private String _entityDataSheetName = "Entities";
        private String _relationshipSheetname = "Relationships";

        private LocaleEnum _defaultLocale = LocaleEnum.UnKnown;

        private Dictionary<MetadataTemplateFieldEnum, KeyValuePair<String, String>> _metadataTemplateFields = new Dictionary<MetadataTemplateFieldEnum, KeyValuePair<String, String>>();

        private Dictionary<EntityDataTemplateFieldEnum, String> _entityDataTemplateColumns = new Dictionary<EntityDataTemplateFieldEnum, String>();
        private Dictionary<RelationshipDataTemplateFieldEnum, String> _relationshipDataTemplateColumns = new Dictionary<RelationshipDataTemplateFieldEnum, String>();
        private Dictionary<String, String> _attributeMetadataTemplateColumns = new Dictionary<String, String>();

        private DataTable _dtEntities = new DataTable();
        
        private DataTable _dtRelationships = new DataTable();

        private List<Entity> _entityList = new List<Entity>();

        private Int32 _currentReadCount = 0;
        private Boolean _isInitialized = false;

        private Collection<String> _entityAttributesList = new Collection<String>();
        private Collection<String> _relationshipAttributesList = new Collection<String>();

        #endregion

        #region Constructors

        public ExcelSource10(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                throw new ArgumentNullException("Rs Excel file is not available");
            }

            _sourceFile = filePath;

            BuildTemplateColumnDictionaries();
        }
        
        #endregion

        #region Private Enums

        private enum DataLoadType
        { 
            EntityWithAttributes,
            EntityWithAttributesAndRelationships,
            EntityRelationshipsOnly
        }

        private enum MetadataTemplateFieldEnum
        {
            DefaultOrganization,
            DefaultContainer,
            DefaultEntityType,
            DefaultCateogryPath,
            DefaultParentExtensionContainer,
            DefaultParentExtensionCategoryPath,
            DefaultDataLocale,

            DefaultFromEntityOrganization,
            DefaultFromEntityContainer,
            DefaultFromEntityEntityType,
            DefaultFromEntityCateogryPath,

            DefaultToEntityOrganization,
            DefaultToEntityContainer,
            DefaultToEntityEntityType,
            DefaultToEntityCateogryPath,

            CollectionSeparator,
            UomSeparator,
            DeleteKeyword,
            BlankKeyword
        }

        private enum EntityDataTemplateFieldEnum
        {
            Id,
            ExtenalId,
            LongName,
            EntityType,
            CategoryPath,
            Container,
            Organization,
            ParentExternalId,
            ParentExtensionExternalId,
            ParentExtensionCategoryPath,
            ParentExtensionContainer,
            ParentExtensionOrganization,
            IsProcessed,
            Action
        }

        private enum RelationshipDataTemplateFieldEnum
        {
            Id,
            RelationshipExternalId,
            RelationshipType,

            FromEntityExternalId,
            FromEntityEntityType,
            FromEntityCategoryPath,
            FromEntityContainer,
            FromEntityOrganization,

            ToEntityExternalId,
            ToEntityEntityType,
            ToEntityCategoryPath,
            ToEntityContainer,
            ToEntityOrganization,

            IsProcessed
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
            if (!_isInitialized)
            {
                #region Read Metadata Tab

                ReadMetadataTab();

                #endregion

                #region Set Profile level properties

                if (importProfile != null)
                {
                    //Set profile for the file level metadata settings(Examples: UOM Separator, DeleteKeyword, etc)
                    if (!String.IsNullOrWhiteSpace(_metadataTemplateFields[MetadataTemplateFieldEnum.CollectionSeparator].Value))
                    {
                        importProfile.ProcessingSpecifications.KeywordProcessingOptions.CollectionDataSeparator = _metadataTemplateFields[MetadataTemplateFieldEnum.CollectionSeparator].Value;
                    }
                    else
                    {
                        String collectionSeperator = AppConfigurationHelper.GetAppConfig<String>("MDM.Exports.RSExcelFormatter.CollectionSeparator");

                        if (!String.IsNullOrWhiteSpace(collectionSeperator))
                        {
                            importProfile.ProcessingSpecifications.KeywordProcessingOptions.CollectionDataSeparator = collectionSeperator;
                        }
                    }

                    if (!String.IsNullOrWhiteSpace(_metadataTemplateFields[MetadataTemplateFieldEnum.UomSeparator].Value))
                    {
                        importProfile.ProcessingSpecifications.KeywordProcessingOptions.UomDataSeparator = _metadataTemplateFields[MetadataTemplateFieldEnum.UomSeparator].Value;
                    }
                    else
                    {
                        String uomSeperator = AppConfigurationHelper.GetAppConfig<String>("MDM.Exports.RSExcelFormatter.UomSeparator");

                        if (!String.IsNullOrWhiteSpace(uomSeperator))
                        {
                            importProfile.ProcessingSpecifications.KeywordProcessingOptions.UomDataSeparator = uomSeperator;
                        }
                    }

                    if (!String.IsNullOrWhiteSpace(_metadataTemplateFields[MetadataTemplateFieldEnum.DeleteKeyword].Value))
                    {
                        importProfile.ProcessingSpecifications.KeywordProcessingOptions.EnableDeleteKeyword = true;
                        importProfile.ProcessingSpecifications.KeywordProcessingOptions.DeleteKeyword = _metadataTemplateFields[MetadataTemplateFieldEnum.DeleteKeyword].Value;
                    }

                    if (!String.IsNullOrWhiteSpace(_metadataTemplateFields[MetadataTemplateFieldEnum.BlankKeyword].Value))
                    {
                        importProfile.ProcessingSpecifications.KeywordProcessingOptions.EnableBlankKeyword = true;
                        importProfile.ProcessingSpecifications.KeywordProcessingOptions.BlankKeyword = _metadataTemplateFields[MetadataTemplateFieldEnum.BlankKeyword].Value;
                    }

                    string defaultLocale = _metadataTemplateFields[MetadataTemplateFieldEnum.DefaultDataLocale].Value;
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
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();

            //Read "Entity Metadata" sheet, "Entity" and "Relationships" sheet from excel and merge the columns in 1 data table

            //Both "Entity" and "Relationships" sheets are already read and stored in _dtEntities and _dtRelationships data tables. So here we need to read metadata only.
            #region Read "Entity Metadata" sheet from RsExcel workbook

            foreach (KeyValuePair<MetadataTemplateFieldEnum, KeyValuePair<String, String>> pair in _metadataTemplateFields)
            {
                String colName = String.Concat("[", _metadataSheetName, "].[", pair.Value.Key, "]");
                DataColumn dc = new DataColumn(colName);
                dt.Columns.Add(dc);
            }

            #endregion

            #region Read "Entity" sheet - read "_dtEntities"  datatable and copy columns in dummy data table

            string validSheetName = ExternalFileReader.ValidateExcelWorkSheet(_sourceFile, _entityDataSheetName);

            if (_entityDataSheetName.ToLower() == validSheetName.Replace("$", "").ToLower())
            {
                //Read Excel data in dataset
                DataSet dsEntity = ExternalFileReader.ReadExternalFile("excel", _sourceFile, _entityDataSheetName, "", false, null, null, null, 0, "ANSI");

                //Add each column in another datatable.
                if (dsEntity != null && dsEntity.Tables != null && dsEntity.Tables.Count > 0)
                {
                    DataTable dtEntities = dsEntity.Tables[0];
                    if (dtEntities != null)
                    {
                        foreach (DataColumn column in dtEntities.Columns)
                        {
                            if (!column.ColumnName.Equals(_entityDataTemplateColumns[EntityDataTemplateFieldEnum.IsProcessed], StringComparison.InvariantCultureIgnoreCase))
                            {
                                dt.Columns.Add(String.Concat("[", _entityDataSheetName, "].[", column.ColumnName, "]"));
                            }
                        }
                    }
                }
            }

            #endregion

            #region Read "Relationship" sheet

            validSheetName = ExternalFileReader.ValidateExcelWorkSheet(_sourceFile, _relationshipSheetname);

            if (_relationshipSheetname.ToLower() == validSheetName.Replace("$", "").ToLower())
            {
                //Read Excel data in dataset
                DataSet dsRelationship = ExternalFileReader.ReadExternalFile("excel", _sourceFile, _relationshipSheetname, "", false, null, null, null, 0, "ANSI");

                //Add each column in another datatable.
                if (dsRelationship != null && dsRelationship.Tables != null && dsRelationship.Tables.Count > 0)
                {
                    DataTable dtRelationships = dsRelationship.Tables[0];

                    foreach (DataColumn column in dtRelationships.Columns)
                    {
                        //"$Processed$" is column used for internal purpose. So remove this as it is not needed on UI.
                        if (!column.ColumnName.Equals(_relationshipDataTemplateColumns[RelationshipDataTemplateFieldEnum.IsProcessed], StringComparison.InvariantCultureIgnoreCase))
                        {
                            dt.Columns.Add(String.Concat("[", _relationshipSheetname, "].[", column.ColumnName, "]"));
                        }
                    }
                }
            }
        
            #endregion

            ds.Tables.Add(dt);

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

        private void BuildTemplateColumnDictionaries()
        {
            //Fields on Metadata sheet
            //Entity metadata
            _metadataTemplateFields.Add(MetadataTemplateFieldEnum.DefaultOrganization, new KeyValuePair<String, String>("Default Organization", String.Empty));
            _metadataTemplateFields.Add(MetadataTemplateFieldEnum.DefaultContainer, new KeyValuePair<String, String>("Default Container", String.Empty));
            _metadataTemplateFields.Add(MetadataTemplateFieldEnum.DefaultEntityType, new KeyValuePair<String, String>("Default Entity Type", String.Empty));
            _metadataTemplateFields.Add(MetadataTemplateFieldEnum.DefaultCateogryPath, new KeyValuePair<String, String>("Default Category Path", String.Empty));
            _metadataTemplateFields.Add(MetadataTemplateFieldEnum.DefaultParentExtensionContainer, new KeyValuePair<String, String>("Default Parent Extension Container", String.Empty));
            _metadataTemplateFields.Add(MetadataTemplateFieldEnum.DefaultParentExtensionCategoryPath, new KeyValuePair<String, String>("Default Parent Extension Category Path", String.Empty));
            _metadataTemplateFields.Add(MetadataTemplateFieldEnum.DefaultDataLocale, new KeyValuePair<String, String>("Default Data Locale", String.Empty));

            //Relationship metadata
            _metadataTemplateFields.Add(MetadataTemplateFieldEnum.DefaultFromEntityOrganization, new KeyValuePair<String, String>("Default From Entity Organization", String.Empty));
            _metadataTemplateFields.Add(MetadataTemplateFieldEnum.DefaultFromEntityContainer, new KeyValuePair<String, String>("Default From Entity Container", String.Empty));
            _metadataTemplateFields.Add(MetadataTemplateFieldEnum.DefaultFromEntityEntityType, new KeyValuePair<String, String>("Default From Entity Type", String.Empty));
            _metadataTemplateFields.Add(MetadataTemplateFieldEnum.DefaultFromEntityCateogryPath, new KeyValuePair<String, String>("Default From Entity Category Path", String.Empty));
            _metadataTemplateFields.Add(MetadataTemplateFieldEnum.DefaultToEntityOrganization, new KeyValuePair<String, String>("Default To Entity Organization", String.Empty));
            _metadataTemplateFields.Add(MetadataTemplateFieldEnum.DefaultToEntityContainer, new KeyValuePair<String, String>("Default To Entity Container", String.Empty));
            _metadataTemplateFields.Add(MetadataTemplateFieldEnum.DefaultToEntityEntityType, new KeyValuePair<String, String>("Default To Entity Type", String.Empty));
            _metadataTemplateFields.Add(MetadataTemplateFieldEnum.DefaultToEntityCateogryPath, new KeyValuePair<String, String>("Default To Entity Category Path", String.Empty));
            
            //Processing options
            _metadataTemplateFields.Add(MetadataTemplateFieldEnum.CollectionSeparator, new KeyValuePair<String, String>("Collection Separator", String.Empty));
            _metadataTemplateFields.Add(MetadataTemplateFieldEnum.UomSeparator, new KeyValuePair<String, String>("UOM Separator", String.Empty));
            _metadataTemplateFields.Add(MetadataTemplateFieldEnum.DeleteKeyword, new KeyValuePair<String, String>("Keyword To Delete Value", String.Empty));
            _metadataTemplateFields.Add(MetadataTemplateFieldEnum.BlankKeyword, new KeyValuePair<String, String>("Keyword To Clear Value", String.Empty));

            //Fields on Entity Sheet
            _entityDataTemplateColumns.Add(EntityDataTemplateFieldEnum.Id, "Id");
            _entityDataTemplateColumns.Add(EntityDataTemplateFieldEnum.ExtenalId, "External Id");
            _entityDataTemplateColumns.Add(EntityDataTemplateFieldEnum.LongName, "Long Name");
            _entityDataTemplateColumns.Add(EntityDataTemplateFieldEnum.EntityType, "Entity Type");
            _entityDataTemplateColumns.Add(EntityDataTemplateFieldEnum.CategoryPath, "Category Path");
            _entityDataTemplateColumns.Add(EntityDataTemplateFieldEnum.Container, "Container");
            _entityDataTemplateColumns.Add(EntityDataTemplateFieldEnum.Organization, "Organization");
            _entityDataTemplateColumns.Add(EntityDataTemplateFieldEnum.ParentExternalId, "Parent External Id");
            _entityDataTemplateColumns.Add(EntityDataTemplateFieldEnum.ParentExtensionExternalId, "Parent Extension External Id");
            _entityDataTemplateColumns.Add(EntityDataTemplateFieldEnum.ParentExtensionCategoryPath, "Parent Extension Category Path");
            _entityDataTemplateColumns.Add(EntityDataTemplateFieldEnum.ParentExtensionContainer, "Parent Extension Container");
            _entityDataTemplateColumns.Add(EntityDataTemplateFieldEnum.ParentExtensionOrganization, "Parent Extension Organization");
            _entityDataTemplateColumns.Add(EntityDataTemplateFieldEnum.Action, "Entity Action");
            _entityDataTemplateColumns.Add(EntityDataTemplateFieldEnum.IsProcessed, "$Processed$");
            
            //Fields on Relationship Sheet
            _relationshipDataTemplateColumns.Add(RelationshipDataTemplateFieldEnum.Id, "Id");
            _relationshipDataTemplateColumns.Add(RelationshipDataTemplateFieldEnum.RelationshipExternalId, "Relationship External Id");
            _relationshipDataTemplateColumns.Add(RelationshipDataTemplateFieldEnum.RelationshipType, "Relationship Type");

            _relationshipDataTemplateColumns.Add(RelationshipDataTemplateFieldEnum.FromEntityExternalId, "From Entity External Id");
            _relationshipDataTemplateColumns.Add(RelationshipDataTemplateFieldEnum.FromEntityEntityType, "From Entity Type");
            _relationshipDataTemplateColumns.Add(RelationshipDataTemplateFieldEnum.FromEntityCategoryPath, "From Entity Category Path");
            _relationshipDataTemplateColumns.Add(RelationshipDataTemplateFieldEnum.FromEntityContainer, "From Entity Container");
            _relationshipDataTemplateColumns.Add(RelationshipDataTemplateFieldEnum.FromEntityOrganization, "From Entity Organization");
            
            _relationshipDataTemplateColumns.Add(RelationshipDataTemplateFieldEnum.ToEntityExternalId, "To Entity External Id");
            _relationshipDataTemplateColumns.Add(RelationshipDataTemplateFieldEnum.ToEntityEntityType, "To Entity Type");
            _relationshipDataTemplateColumns.Add(RelationshipDataTemplateFieldEnum.ToEntityCategoryPath, "To Entity Category Path");
            _relationshipDataTemplateColumns.Add(RelationshipDataTemplateFieldEnum.ToEntityContainer, "To Entity Container");
            _relationshipDataTemplateColumns.Add(RelationshipDataTemplateFieldEnum.ToEntityOrganization, "To Entity Organization");

            _relationshipDataTemplateColumns.Add(RelationshipDataTemplateFieldEnum.IsProcessed, "$Processed$");
        }

        private void ReadMetadataTab()
        {
            String filename = _sourceFile;

            DataSet dsMetadata = ExternalFileReader.ReadExternalFile("excel", filename, _metadataSheetName, "", false, null, null, null, 0, "ANSI");

            //Metadata tab header processing
            if (dsMetadata != null && dsMetadata.Tables != null && dsMetadata.Tables.Count > 0)
            {
                DataTable dtMetadata = dsMetadata.Tables[0];

                if (dtMetadata.Columns != null && dtMetadata.Columns.Count > 0)
                {
                    DataColumnCollection metadataColumns = dtMetadata.Columns;

                    if (dtMetadata.Rows.Count > 0 && dtMetadata.Columns.Count >=8)
                    {
                        Dictionary<MetadataTemplateFieldEnum, KeyValuePair<String, String>> tempMetadataTemplateFields = new Dictionary<MetadataTemplateFieldEnum, KeyValuePair<String, String>>();

                        for (int i = 1; i < dtMetadata.Rows.Count; i++)
                        {
                            DataRow dataRow = dtMetadata.Rows[i];

                            String key1 = dataRow[0] != null ? dataRow[0].ToString() : String.Empty;
                            String value1 = dataRow[1] != null ? dataRow[1].ToString() : String.Empty;
                            String key2 = dataRow[3] != null ? dataRow[3].ToString() : String.Empty;
                            String value2 = dataRow[4] != null ? dataRow[4].ToString() : String.Empty;
                            String key3 = dataRow[6] != null ? dataRow[6].ToString() : String.Empty;
                            String value3 = dataRow[7] != null ? dataRow[7].ToString() : String.Empty;

                            foreach (KeyValuePair<MetadataTemplateFieldEnum, KeyValuePair<String, String>> pair in _metadataTemplateFields)
                            {
                                MetadataTemplateFieldEnum templeteField = pair.Key;
                                KeyValuePair<String, String> valuePair = pair.Value;
                                String valueKey = valuePair.Key;
                                
                                if (valueKey.Equals(key1, StringComparison.InvariantCultureIgnoreCase))
                                {
                                    //_metadataTemplateFields[templeteField] = new KeyValuePair<String, String>(key1, value1);
                                    tempMetadataTemplateFields.Add(templeteField, new KeyValuePair<string,string>(key1, value1));
                                }
                                else if (valueKey.Equals(key2, StringComparison.InvariantCultureIgnoreCase))
                                {
                                    //_metadataTemplateFields[templeteField] = new KeyValuePair<String, String>(key2, value2);
                                    tempMetadataTemplateFields.Add(templeteField, new KeyValuePair<string,string>(key2, value2));
                                }
                                else if (valueKey.Equals(key3, StringComparison.InvariantCultureIgnoreCase))
                                {
                                    //_metadataTemplateFields[templeteField] = new KeyValuePair<String, String>(key3, value3);
                                    tempMetadataTemplateFields.Add(templeteField, new KeyValuePair<string,string>(key3, value3));
                                }
                            }
                        }

                        //Now set the value to actual dictionary. We need to do this as KeyValuePair is read only object once created..
                        foreach (KeyValuePair<MetadataTemplateFieldEnum, KeyValuePair<String, String>> pair in tempMetadataTemplateFields)
                        {
                            _metadataTemplateFields[pair.Key] = pair.Value;
                        }
                    }
                }
            }
            else
            {
                //log.Debug("Metadata Sheet is not Provided For Job : " + jobId);
            }
        }

        private void ReadEntityDataTabs()
        {
            String filename = _sourceFile;
            
            List<Entity> entityList = new List<Entity>();

            #region Read Entity Tab

            String validEntitySheetName = ExternalFileReader.ValidateExcelWorkSheet(filename, _entityDataSheetName);

            if (_entityDataSheetName.Equals(validEntitySheetName.Replace("$", ""), StringComparison.InvariantCultureIgnoreCase))
            {
                DataSet dsEntity = ExternalFileReader.ReadExternalFile("excel", filename, _entityDataSheetName, "", false, null, null, null, 0, "ANSI");
                
                if(dsEntity!=null && dsEntity.Tables !=null && dsEntity.Tables.Count > 0)
                {
                    this._dtEntities = dsEntity.Tables[0];


                    Dictionary<String, Int32> mappings = GetAttributeColumnIndexMappings(filename, RSExcelConstants.EntityDataSheetName, _entityAttributesList, RSExcelConstants.EntityDataTemplateColumns.Values.ToList());

                    if (_dtEntities.Rows.Count > 0)
                    {
                        FillEntities(entityList, _dtEntities.Rows, mappings);
                    }
                }
            }

            #endregion

            #region Read Relationship Tab
            
            String validRelationshipSheetName = ExternalFileReader.ValidateExcelWorkSheet(filename, _relationshipSheetname);

            if (_relationshipSheetname.Equals(validRelationshipSheetName.Replace("$", ""), StringComparison.InvariantCultureIgnoreCase))
            {
                DataSet dsRelationship = ExternalFileReader.ReadExternalFile("excel", filename, _relationshipSheetname, "", false, null, null, null, 0, "ANSI");

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
            }

            #endregion

            this._entityList = entityList;
        }

        private void PopulateEntityAttributesList(DataTable dtEntities)
        {
            if (dtEntities != null)
            {
                foreach (DataColumn dc in dtEntities.Columns)
                {
                    if (!_entityDataTemplateColumns.ContainsValue(dc.ColumnName))
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
                    if (!_relationshipDataTemplateColumns.ContainsValue(dc.ColumnName))
                    {
                        _relationshipAttributesList.Add(dc.ColumnName);
                    }
                }
            }
        }

        #endregion

        #region Fill Methods

        private Boolean FillEntities(List<Entity> entitiesTobeProcessed, DataRowCollection dataRows, Dictionary<String, Int32> attributeColIndexMappings)
        {
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

            return successFlag;
        }

        private Boolean FillEntityRelationships(List<Entity> entitiesTobeProcessed, DataRowCollection dataRows,Dictionary<String, Int32> attributeColIndexMappings)
        {
            Boolean successFlag = true;

            foreach (DataRow dataRow in dataRows)
            {
                Relationship relationship = new Relationship();

                String fromEntityExternalId = String.Empty;
                String fromEntityEntityType = _metadataTemplateFields[MetadataTemplateFieldEnum.DefaultFromEntityEntityType].Value;
                String fromEntityCategoryPath = _metadataTemplateFields[MetadataTemplateFieldEnum.DefaultFromEntityCateogryPath].Value;
                String fromEntityContainer = _metadataTemplateFields[MetadataTemplateFieldEnum.DefaultFromEntityContainer].Value;
                String fromEntityOrganization = _metadataTemplateFields[MetadataTemplateFieldEnum.DefaultFromEntityOrganization].Value;

                #region Read Relationship from data

                if (this._dtRelationships.Columns.Contains(_relationshipDataTemplateColumns[RelationshipDataTemplateFieldEnum.FromEntityExternalId]))
                {
                    String rowLevelFromEntityExternalId = dataRow[_relationshipDataTemplateColumns[RelationshipDataTemplateFieldEnum.FromEntityExternalId]].ToString();

                    if (!String.IsNullOrWhiteSpace(rowLevelFromEntityExternalId))
                    {
                        fromEntityExternalId = rowLevelFromEntityExternalId;
                    }
                }

                if (this._dtRelationships.Columns.Contains(_relationshipDataTemplateColumns[RelationshipDataTemplateFieldEnum.FromEntityEntityType]))
                {
                    String rowLevelFromEntityEntityType = dataRow[_relationshipDataTemplateColumns[RelationshipDataTemplateFieldEnum.FromEntityEntityType]].ToString();

                    if (!String.IsNullOrWhiteSpace(rowLevelFromEntityEntityType ))
                    {
                        fromEntityEntityType = rowLevelFromEntityEntityType ;
                    }
                }

                if (this._dtRelationships.Columns.Contains(_relationshipDataTemplateColumns[RelationshipDataTemplateFieldEnum.FromEntityCategoryPath]))
                {
                    String rowLevelFromEntityCategoryPath = dataRow[_relationshipDataTemplateColumns[RelationshipDataTemplateFieldEnum.FromEntityCategoryPath]].ToString();

                    if (!String.IsNullOrWhiteSpace(rowLevelFromEntityCategoryPath))
                    {
                        fromEntityCategoryPath = rowLevelFromEntityCategoryPath;
                    }
                }

                if (this._dtRelationships.Columns.Contains(_relationshipDataTemplateColumns[RelationshipDataTemplateFieldEnum.FromEntityContainer]))
                {
                    String rowLevelFromContainer = dataRow[_relationshipDataTemplateColumns[RelationshipDataTemplateFieldEnum.FromEntityContainer]].ToString();

                    if (!String.IsNullOrWhiteSpace(rowLevelFromContainer))
                    {
                        fromEntityContainer = rowLevelFromContainer;
                    }
                }

                if (this._dtRelationships.Columns.Contains(_relationshipDataTemplateColumns[RelationshipDataTemplateFieldEnum.FromEntityOrganization]))
                {
                    String rowLevelFromEntityOrganization = dataRow[_relationshipDataTemplateColumns[RelationshipDataTemplateFieldEnum.FromEntityOrganization]].ToString();

                    if (!String.IsNullOrWhiteSpace(rowLevelFromEntityOrganization))
                    {
                        fromEntityOrganization = rowLevelFromEntityOrganization;
                    }
                }

                #endregion

                FillRelationshipProperties(relationship, dataRow);

                FillRelationshipAttributes(relationship, dataRow, attributeColIndexMappings);

                Entity entity = null;

                //Link relationship to existing entity else create new entity and add relationships there..
                var filteredEntities = from ent in entitiesTobeProcessed
                                       where (ent.ExternalId.Equals(fromEntityExternalId)
                                                && ent.EntityTypeName.Equals(fromEntityEntityType)
                                                && ent.CategoryPath.Equals(fromEntityCategoryPath)
                                                && ent.ContainerName.Equals(fromEntityContainer)
                                                && ent.OrganizationName.Equals(fromEntityOrganization))
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

                    entitiesTobeProcessed.Add(entity);
                }

                if (entity != null)
                {
                    entity.Relationships.Add(relationship);
                }
            }

            return successFlag;
        }

        private void FillEntityProperties(Entity entity, DataRow dataRow)
        {
            if (this._dtEntities.Columns.Contains(_entityDataTemplateColumns[EntityDataTemplateFieldEnum.Id]))
            {
                String rowLevelRefId = dataRow[_entityDataTemplateColumns[EntityDataTemplateFieldEnum.Id]].ToString();

                if (!String.IsNullOrWhiteSpace(rowLevelRefId))
                {
                    entity.ReferenceId = ValueTypeHelper.Int64TryParse(rowLevelRefId, -1);
                }
            }

            if (this._dtEntities.Columns.Contains(_entityDataTemplateColumns[EntityDataTemplateFieldEnum.ExtenalId]))
            {
                String rowLevelExternalId = dataRow[_entityDataTemplateColumns[EntityDataTemplateFieldEnum.ExtenalId]].ToString();

                if (!String.IsNullOrWhiteSpace(rowLevelExternalId))
                {
                    entity.ExternalId = rowLevelExternalId;
                    entity.Name = rowLevelExternalId;
                }
            }

            if (this._dtEntities.Columns.Contains(_entityDataTemplateColumns[EntityDataTemplateFieldEnum.LongName]))
            {
                String rowLevelLongName = dataRow[_entityDataTemplateColumns[EntityDataTemplateFieldEnum.LongName]].ToString();

                if (!String.IsNullOrWhiteSpace(rowLevelLongName))
                {
                    entity.LongName = rowLevelLongName;
                }
            }

            String entityType = _metadataTemplateFields[MetadataTemplateFieldEnum.DefaultEntityType].Value;
            if (this._dtEntities.Columns.Contains(_entityDataTemplateColumns[EntityDataTemplateFieldEnum.EntityType]))
            {
                String rowLevelEntityType = dataRow[_entityDataTemplateColumns[EntityDataTemplateFieldEnum.EntityType]].ToString();

                if (!String.IsNullOrWhiteSpace(rowLevelEntityType))
                    entityType = rowLevelEntityType;
            }
            entity.EntityTypeName = entityType;

            String categoryPath = _metadataTemplateFields[MetadataTemplateFieldEnum.DefaultCateogryPath].Value;
            if (this._dtEntities.Columns.Contains(_entityDataTemplateColumns[EntityDataTemplateFieldEnum.CategoryPath]))
            {
                String rowLevelCategoryPath = dataRow[_entityDataTemplateColumns[EntityDataTemplateFieldEnum.CategoryPath]].ToString();

                if (!String.IsNullOrWhiteSpace(rowLevelCategoryPath))
                    categoryPath = rowLevelCategoryPath;
            }
            entity.CategoryPath = categoryPath;

            String containerName = _metadataTemplateFields[MetadataTemplateFieldEnum.DefaultContainer].Value; ;
            if (this._dtEntities.Columns.Contains(_entityDataTemplateColumns[EntityDataTemplateFieldEnum.Container]))
            {
                String rowLevelContainerName = dataRow[_entityDataTemplateColumns[EntityDataTemplateFieldEnum.Container]].ToString();

                if (!String.IsNullOrWhiteSpace(rowLevelContainerName))
                    containerName = rowLevelContainerName;
            }
            entity.ContainerName = containerName;

            String organizationName = _metadataTemplateFields[MetadataTemplateFieldEnum.DefaultOrganization].Value; ;
            if (this._dtEntities.Columns.Contains(_entityDataTemplateColumns[EntityDataTemplateFieldEnum.Organization]))
            {
                String rowLevelOrganizationName = dataRow[_entityDataTemplateColumns[EntityDataTemplateFieldEnum.Organization]].ToString();

                if (!String.IsNullOrWhiteSpace(rowLevelOrganizationName))
                    organizationName = rowLevelOrganizationName;
            }
            entity.OrganizationName = organizationName;

            if (this._dtEntities.Columns.Contains(_entityDataTemplateColumns[EntityDataTemplateFieldEnum.ParentExternalId]))
            {
                String rowLevelHierarchyParentExternalId = dataRow[_entityDataTemplateColumns[EntityDataTemplateFieldEnum.ParentExternalId]].ToString();

                if (!String.IsNullOrWhiteSpace(rowLevelHierarchyParentExternalId))
                {
                    entity.ParentExternalId = rowLevelHierarchyParentExternalId;
                }
            }

            if (this._dtEntities.Columns.Contains(_entityDataTemplateColumns[EntityDataTemplateFieldEnum.ParentExtensionExternalId]))
            {
                String rowLevelExtensionParentExternalId = dataRow[_entityDataTemplateColumns[EntityDataTemplateFieldEnum.ParentExtensionExternalId]].ToString();

                if (!String.IsNullOrWhiteSpace(rowLevelExtensionParentExternalId))
                {
                    entity.ParentExtensionEntityExternalId = rowLevelExtensionParentExternalId;
                }
            }

            String parentExtensionCategoryPath = _metadataTemplateFields[MetadataTemplateFieldEnum.DefaultParentExtensionCategoryPath].Value; ;
            if (this._dtEntities.Columns.Contains(_entityDataTemplateColumns[EntityDataTemplateFieldEnum.ParentExtensionCategoryPath]))
            {
                String rowLevelParentExtensionCategoryPath = dataRow[_entityDataTemplateColumns[EntityDataTemplateFieldEnum.ParentExtensionCategoryPath]].ToString();

                if (!String.IsNullOrWhiteSpace(rowLevelParentExtensionCategoryPath))
                    parentExtensionCategoryPath = rowLevelParentExtensionCategoryPath;
            }
            entity.ParentExtensionEntityCategoryPath = parentExtensionCategoryPath;

            String parentExtensionContainer = _metadataTemplateFields[MetadataTemplateFieldEnum.DefaultParentExtensionContainer].Value; ;
            if (this._dtEntities.Columns.Contains(_entityDataTemplateColumns[EntityDataTemplateFieldEnum.ParentExtensionContainer]))
            {
                String rowLevelParentExtensionContainer = dataRow[_entityDataTemplateColumns[EntityDataTemplateFieldEnum.ParentExtensionContainer]].ToString();

                if (!String.IsNullOrWhiteSpace(rowLevelParentExtensionContainer))
                    parentExtensionContainer = rowLevelParentExtensionContainer;
            }
            entity.ParentExtensionEntityContainerName = parentExtensionContainer;

            if (entity.EntityTypeName.Equals("Category", StringComparison.InvariantCultureIgnoreCase))
                entity.BranchLevel = ContainerBranchLevel.Node;
            else
                entity.BranchLevel = ContainerBranchLevel.Component;

            if (this._dtEntities.Columns.Contains(_entityDataTemplateColumns[EntityDataTemplateFieldEnum.Action]))
            {
                String action = dataRow[_entityDataTemplateColumns[EntityDataTemplateFieldEnum.Action]].ToString();

                if (!String.IsNullOrWhiteSpace(action))
                {
                    ObjectAction entityAction = ObjectAction.Unknown;
                    Enum.TryParse<ObjectAction>(action, out entityAction);

                    if (entityAction != ObjectAction.Unknown)
                    {
                        entity.Action = entityAction;
                    }
                }
            }

        }

        private void FillEntityAttributes(Entity entity, DataRow dataRow,Dictionary<String, Int32> attributeColIndexMappings)
        {
            AttributeCollection attributeCollection = BuildAttributeCollection(_entityAttributesList, dataRow, attributeColIndexMappings);

            entity.Attributes = attributeCollection;   
        }

        private void FillRelationshipProperties(Relationship relationship, DataRow dataRow)
        {
            if (this._dtRelationships.Columns.Contains(_relationshipDataTemplateColumns[RelationshipDataTemplateFieldEnum.RelationshipExternalId]))
            {
                String rowLevelRelationshipExternalId = dataRow[_relationshipDataTemplateColumns[RelationshipDataTemplateFieldEnum.RelationshipExternalId]].ToString();

                if (!String.IsNullOrWhiteSpace(rowLevelRelationshipExternalId))
                {
                    relationship.RelationshipExternalId = rowLevelRelationshipExternalId;
                }
            }

            if (this._dtRelationships.Columns.Contains(_relationshipDataTemplateColumns[RelationshipDataTemplateFieldEnum.RelationshipType]))
            {
                String rowLevelRelationshipType = dataRow[_relationshipDataTemplateColumns[RelationshipDataTemplateFieldEnum.RelationshipType]].ToString();

                if (!String.IsNullOrWhiteSpace(rowLevelRelationshipType))
                {
                    relationship.RelationshipTypeName = rowLevelRelationshipType;
                }
            }

            if (this._dtRelationships.Columns.Contains(_relationshipDataTemplateColumns[RelationshipDataTemplateFieldEnum.ToEntityExternalId]))
            {
                String rowLevelToEntityExternalId = dataRow[_relationshipDataTemplateColumns[RelationshipDataTemplateFieldEnum.ToEntityExternalId]].ToString();

                if (!String.IsNullOrWhiteSpace(rowLevelToEntityExternalId))
                {
                    relationship.ToExternalId = rowLevelToEntityExternalId;
                }
            }

            String toEntityEntityType = _metadataTemplateFields[MetadataTemplateFieldEnum.DefaultToEntityEntityType].Value;
            if (this._dtRelationships.Columns.Contains(_relationshipDataTemplateColumns[RelationshipDataTemplateFieldEnum.ToEntityEntityType]))
            {
                String rowLevelToEntityType = dataRow[_relationshipDataTemplateColumns[RelationshipDataTemplateFieldEnum.ToEntityEntityType]].ToString();

                if (!String.IsNullOrWhiteSpace(rowLevelToEntityType))
                {
                    toEntityEntityType = rowLevelToEntityType;
                }
            }
            relationship.ToEntityTypeName = toEntityEntityType;

            String toEntityCategoryPath = _metadataTemplateFields[MetadataTemplateFieldEnum.DefaultToEntityCateogryPath].Value;
            if (this._dtRelationships.Columns.Contains(_relationshipDataTemplateColumns[RelationshipDataTemplateFieldEnum.ToEntityCategoryPath]))
            {
                String rowLevelToEntityCategoryPath = dataRow[_relationshipDataTemplateColumns[RelationshipDataTemplateFieldEnum.ToEntityCategoryPath]].ToString();

                if (!String.IsNullOrWhiteSpace(rowLevelToEntityCategoryPath))
                {
                    toEntityCategoryPath = rowLevelToEntityCategoryPath;
                }
            }
            relationship.ToCategoryPath = toEntityCategoryPath;

            String toEntityContainer = _metadataTemplateFields[MetadataTemplateFieldEnum.DefaultToEntityContainer].Value; ;
            if (this._dtRelationships.Columns.Contains(_relationshipDataTemplateColumns[RelationshipDataTemplateFieldEnum.ToEntityContainer]))
            {
                String rowLevelToEntityContainer = dataRow[_relationshipDataTemplateColumns[RelationshipDataTemplateFieldEnum.ToEntityContainer]].ToString();

                if (!String.IsNullOrWhiteSpace(rowLevelToEntityContainer))
                {
                    toEntityContainer = rowLevelToEntityContainer;
                }
            }
            relationship.ToContainerName = toEntityContainer;

            String toEntityOrganization = _metadataTemplateFields[MetadataTemplateFieldEnum.DefaultToEntityOrganization].Value; ;
            if (this._dtRelationships.Columns.Contains(_relationshipDataTemplateColumns[RelationshipDataTemplateFieldEnum.ToEntityOrganization]))
            {
                String rowLevelToEntityOrganization = dataRow[_relationshipDataTemplateColumns[RelationshipDataTemplateFieldEnum.ToEntityOrganization]].ToString();

                if (!String.IsNullOrWhiteSpace(rowLevelToEntityOrganization))
                {
                    toEntityOrganization = rowLevelToEntityOrganization;
                }
            }
            //TODO:: how to give org name in relationship object

        }

        private void FillRelationshipAttributes(Relationship relationship, DataRow dataRow,Dictionary<String,Int32> attributeColIndexMappings)
        {
            AttributeCollection attributeCollection = BuildAttributeCollection(_relationshipAttributesList, dataRow, attributeColIndexMappings);

            relationship.RelationshipAttributes = attributeCollection;
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
                            if (colIndex >= 255)
                            {
                                break;
                            }
                            var text = ReadExcelCell(cell, workbookPart).Trim();

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
                        attribute.Name = arrAttributeHeader[1];
                        attribute.LongName = arrAttributeHeader[1];
                        attribute.AttributeParentName = arrAttributeHeader[0];

                        MDM.BusinessObjects.Value value = new MDM.BusinessObjects.Value();
                        String attrVal = dataRow[attrColumnIndex].ToString();
                        value.AttrVal = attrVal;

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

                        attribute.Locale = _defaultLocale;

                        MDM.BusinessObjects.Value value = new MDM.BusinessObjects.Value();
                        String attrVal = dataRow[attrColumnIndex].ToString();
                        value.AttrVal = attrVal;

                        attribute.SetValue((IValue)value);
                        attributeCollection.Add(attribute);
                    }

                    if (arrAttributeHeader.Length == 3)
                    {
                        // This is for a future scenario, when a locale will be added to the name.
                        attribute.AttributeParentName = arrAttributeHeader[0];
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

                        attribute.SetValue((IValue)value);
                        attributeCollection.Add(attribute);
                    }
                }
            }

            return attributeCollection;
        }

        #endregion

        #endregion
    }
}
