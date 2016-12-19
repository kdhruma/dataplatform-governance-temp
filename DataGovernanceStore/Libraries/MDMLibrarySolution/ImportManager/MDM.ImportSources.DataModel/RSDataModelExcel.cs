using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.Linq;

namespace MDM.ImportSources.DataModel
{
    using MDM.BusinessObjects;
    using MDM.BusinessObjects.DynamicTableSchema;
    using MDM.BusinessObjects.Imports;
    using MDM.BusinessObjects.Jobs;
    using MDM.Core;
    using MDM.Core.DataModel;
    using MDM.ExcelUtility;
    using MDM.ExceptionManager;
    using MDM.Imports.Interfaces;
    using MDM.Interfaces;
    using MDM.Utility;
    using MDM.BusinessObjects.DataModel;

    /// <summary>
    /// This class implements the source data from a RS Excel source. It reads the data in to a data set and process them as per the request from the data model engine.
    /// </summary>
    public class RSDataModelExcel : IDataModelImportSourceData
    {
        #region Fields

        /// <summary>
        /// Single forward only reader needs to be synchronized.
        /// </summary>
        internal static Object _readerLock = new Object();

        /// <summary>
        /// Field for data model types
        /// </summary>
        Collection<ObjectType> _dataModelTypesToProcess = new Collection<ObjectType>();

        /// <summary>
        /// Field for batching type
        /// </summary>
        ImportProviderBatchingType _batchingType = ImportProviderBatchingType.Single;

        /// <summary>
        /// Field for batch size. Default is 100
        /// </summary>
        Int16 _batchSize = 100;

        /// <summary>
        /// Field for source file
        /// </summary>
        String _sourceFile = String.Empty;

        /// <summary>
        ///  Stores config values from meta data sheet
        /// </summary>
        Dictionary<ObjectType, Boolean> _metadata = new Dictionary<ObjectType, Boolean>();

        /// <summary>
        ///  Stores config values from configuration data sheet
        /// </summary>
        IDataModelConfigurationItemDictionary _configuration = new DataModelConfigurationItemDictionary();

        /// <summary>
        /// Field for system data locale
        /// </summary>
        private LocaleEnum _systemDataLocale = LocaleEnum.UnKnown;

        /// <summary>
        /// Field for maximum batch. Default is 1.
        /// </summary>
        private Int16 _maxBatch = 1;

        /// <summary>
        /// Field for level based dictionary
        /// </summary>
        private Dictionary<Int16, IDataModelObjectCollection> _levelBasedDictionary = null;

        /// <summary>
        /// Constant for level one
        /// </summary>
        private const Int16 LEVEL_ONE = 1;

        /// <summary>
        /// Field for separator.
        /// </summary>
        readonly String _separator = "//";

        /// <summary>
        /// Field for category path separator
        /// </summary>
        private String _categoryPathSeparator = "»";

        #endregion

        #region Constructors

        /// <summary>
        /// Parameter less constructor for RSDataModel Excel
        /// </summary>
        /// <param name="filePath">Indicates the file path</param>
        public RSDataModelExcel(String filePath)
        {
            _sourceFile = filePath;
            _systemDataLocale = GlobalizationHelper.GetSystemDataLocale();
            _separator = AppConfigurationHelper.GetAppConfig<String>("MDMCenter.RSDataModelExcel.Separator", _separator).Trim();

            //Category Path separator should be read from the app config if available.
            _categoryPathSeparator = AppConfigurationHelper.GetAppConfig<String>("Catalog.Category.PathSeparator", _categoryPathSeparator);
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Property denoting the batching type
        /// </summary>
        public ImportProviderBatchingType BatchingType
        {
            get { return _batchingType; }
            set { _batchingType = value; }
        }

        #endregion

        #region DataModel Data

        /// <summary>
        /// Provide an opportunity for the source data to initializes itself with some configuration from the job.
        /// </summary>
        /// <param name="job">Indicates the job</param>
        /// <param name="dataModelImportProfile">Indicates the data model import file</param>
        /// <returns>True. If the data model import profile is initialized else false.</returns>
        public Boolean Initialize(Job job, DataModelImportProfile dataModelImportProfile)
        {
            if (String.IsNullOrEmpty(_sourceFile))
            {
                throw new ArgumentNullException("DataModel Excel file is not available");
            }

            // If validation is required...
            ExecutionStep coreStep = null;

            foreach (ExecutionStep executionStep in dataModelImportProfile.ExecutionSteps)
            {
                if (executionStep.StepType == ExecutionStepType.Core)
                {
                    coreStep = executionStep;
                }
            }

            _batchSize = ValueTypeHelper.Int16TryParse(dataModelImportProfile.DataModelJobProcessingOptions.BatchSize.ToString(), _batchSize);

            // Initialize Excel Config
            InitializeExcelSheetConfiguration();

            if (System.IO.File.Exists(_sourceFile))
            {
                GetMetadata();
                ReadConfiguration();
            }
            else
            {
                throw new ArgumentException(String.Format("DataModel XML file is not available in the specified location {0}", _sourceFile));
            }

            return true;
        }

        /// <summary>
        /// Total number of DataModelObjects types available for processing.
        /// </summary>
        /// <param name="callerContext">Indicates the caller context</param>
        /// <returns>Returns the collection of object types </returns>
        public Collection<ObjectType> GetDataModelObjectTypesForImport(ICallerContext iCallerContext)
        {
            var objectTypeList = (from config in _metadata
                                  where (config.Value)
                                  select config.Key).ToList();
            if (objectTypeList != null)
            {
                Collection<ObjectType> objectTypes = new Collection<ObjectType>(objectTypeList);
                return objectTypes;
            }

            return new Collection<ObjectType>();

        }

        /// <summary>
        /// Indicates the batching mode the provider supports.
        /// </summary>
        /// <returns></returns>
        public ImportProviderBatchingType GetBatchingType()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the batch count
        /// </summary>
        /// <param name="dataModelObjectType">Indicates the data model object type</param>
        /// <param name="callerConext">Indicates the caller context</param>
        /// <returns>Returns the batch count</returns>
        public Int16 GetBatchCount(ObjectType dataModelObjectType, ICallerContext callerConext)
        {
            return _maxBatch;
        }

        /// <summary>
        /// Gets all the data model import objects
        /// </summary>
        /// <param name="dataModelObjectType">Indicates the data model object type</param>
        /// <param name="batch">Indicates the batch number</param>
        /// <param name="callerConext">Indicates the caller context</param>
        /// <returns>Returns the data model object collection interface</returns>
        public IDataModelObjectCollection GetAllDataModelObjects(ObjectType dataModelObjectType, Int16 batch, ICallerContext iCallerContext)
        {
            MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Information, String.Format("GetAllDataModelObjects Start for [{0}]", dataModelObjectType.ToString()), MDMTraceSource.DataModelImport);

            IDataModelObjectCollection dataModelObjects = null;

            if (DataModelDictionary.ObjectsDictionary.ContainsKey(dataModelObjectType) == false)
            {
                MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error, String.Format("GetAllDataModelObjects Completed for [{0}] is not supported.", dataModelObjectType.ToString()), MDMTraceSource.DataModelImport);
                return dataModelObjects;
            }

            if (_levelBasedDictionary == null || _levelBasedDictionary.Count < 1)
            {
                LoadDataSetFromFile(dataModelObjectType);

                //Assign max batch only if we are finding at least one record to be processed.
                if (_levelBasedDictionary != null && _levelBasedDictionary.Count > 0)
                {
                    _maxBatch = (Int16)_levelBasedDictionary.Keys.Count;
                }
            }

            if (_levelBasedDictionary != null && _levelBasedDictionary.Count > 0)
            {
                _levelBasedDictionary.TryGetValue(batch, out dataModelObjects);

                _levelBasedDictionary.Remove(batch);
            }

            MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Information, String.Format("GetAllDataModelObjects Completed for [{0}]", dataModelObjectType.ToString()), MDMTraceSource.DataModelImport);

            return dataModelObjects;
        }

        /// <summary>
        /// Get next available set of data model objects of given batch size.
        /// </summary>
        /// <param name="batchSize">Indicates the batch size</param>
        /// <param name="dataModelObject">Indicates the data model object</param>
        /// <param name="callerContext">Indicates the caller context</param>
        /// <returns>IDataModelObjectCollection</returns>
        public IDataModelObjectCollection GetDataModelObjectsNextBatch(Int32 batchSize, ObjectType dataModelObjectType, ICallerContext iCallerContext)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets a dictionary of template configuration items
        /// </summary>
        /// <returns>Returns a dictionary of template configuration items</returns>
        public IDataModelConfigurationItemDictionary GetConfigurationItems()
        {
            return _configuration;
        }

        #endregion

        #region DataModel Config Setup

        /// <summary>
        /// 
        /// </summary>
        private void InitializeExcelSheetConfiguration()
        {
            // Todo - Any initializations
        }

        #endregion

        #region Excel Method

        /// <summary>
        /// Read Meta data Config
        /// </summary>
        private void GetMetadata()
        {
            try
            {
                MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Information, "Reading file starting....", MDMTraceSource.DataModelImport);

                DataSet dataSet = ExternalFileReader.ReadExternalFileSAXParsing(_sourceFile, DataModelDictionary.ObjectsDictionary[ObjectType.DataModelMetadata], false);

                if (dataSet != null && dataSet.Tables != null && dataSet.Tables.Count > 0)
                {
                    FillMetadata(dataSet.Tables[0]);
                }

            }
            catch (Exception ex)
            {
                ExceptionHandler exHandler = new ExceptionHandler(ex);
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, String.Format("Error: {0} ", ex.Message));

                // Re-Throwing exception for further processing.
                throw ex;
            }
        }

        /// <summary>
        /// Reads Configuration Config
        /// </summary>
        private void ReadConfiguration()
        {
            try
            {
                MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Information, "Reading file starting....", MDMTraceSource.DataModelImport);

                DataSet dataSet = ExternalFileReader.ReadExternalFileSAXParsing(_sourceFile, DataModelDictionary.ObjectsDictionary[ObjectType.Configuration], false);

                if (dataSet != null && dataSet.Tables != null && dataSet.Tables.Count > 0)
                {
                    FillConfiguration(dataSet.Tables[0]);
                }

            }
            catch (Exception ex)
            {
                ExceptionHandler exHandler = new ExceptionHandler(ex);
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, String.Format("Error: {0} ", ex.Message));

                // Re-Throwing exception for further processing.
                throw ex;
            }
        }

        #endregion

        #region DataModel Sheet Reads

        /// <summary>
        /// Load the data model from the input file
        /// </summary>
        /// <param name="dataModelObjectType">Indicates the data model object type</param>
        private void LoadDataSetFromFile(ObjectType dataModelObjectType)
        {
            MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Information, String.Format("LoadDataSetFromFile Start for [{0}]", dataModelObjectType.ToString()), MDMTraceSource.DataModelImport);

            if (DataModelDictionary.ObjectsDictionary.ContainsKey(dataModelObjectType))
            {
                String dataModelSheetName = DataModelDictionary.ObjectsDictionary[dataModelObjectType];
                DataSet dataSet = ExternalFileReader.ReadExternalFileSAXParsing(_sourceFile, dataModelSheetName, false);

                //Added sheetname in data table name so that if exception is thrown for any data table, then sheetname can be displayed.
                //TODO: Change this to handle each columns in all data models separately and display missing column message for all data models.

                if (dataSet != null && dataSet.Tables != null && dataSet.Tables.Count > 0)
                {
                    DataTable dataTable = dataSet.Tables[0];
                    dataTable.TableName = dataModelSheetName;

                    switch (dataModelObjectType)
                    {
                        case ObjectType.Organization:
                            LoadOrganizations(dataTable);
                            break;

                        case ObjectType.Taxonomy:
                            LoadHierarchies(dataTable);
                            break;

                        case ObjectType.Catalog:
                            LoadContainers(dataTable);
                            break;

                        case ObjectType.ContainerLocalization:
                            LoadContainerLocalizations(dataTable);
                            break;

                        case ObjectType.EntityType:
                            LoadEntityTypes(dataTable);
                            break;

                        case ObjectType.RelationshipType:
                            LoadRelationshipTypes(dataTable);
                            break;

                        case ObjectType.AttributeModel:
                            LoadAttributeModels(dataTable);
                            break;

                        case ObjectType.AttributeModelLocalization:
                            LoadAttributeModelLocalization(dataTable);
                            break;

                        case ObjectType.Category:
                            LoadCategories(dataTable);
                            break;

                        case ObjectType.CategoryLocalization:
                            LoadCategoryLocalizations(dataTable);
                            break;

                        case ObjectType.ContainerEntityTypeMapping:
                            LoadContainerEntityTypeMappings(dataTable);
                            break;

                        case ObjectType.EntityTypeAttributeMapping:
                            LoadEntityTypeAttributeMappings(dataTable);
                            break;

                        case ObjectType.ContainerEntityTypeAttributeMapping:
                            LoadContainerEntityTypeAttributeMappings(dataTable);
                            break;

                        case ObjectType.CategoryAttributeMapping:
                            LoadCategoryAttributeMappings(dataTable);
                            break;

                        case ObjectType.RelationshipTypeEntityTypeMapping:
                            LoadRelationshipTypeEntityTypeMappings(dataTable);
                            break;

                        case ObjectType.RelationshipTypeEntityTypeMappingCardinality:
                            LoadRelationshipTypeEntityTypeCardinalities(dataTable);
                            break;

                        case ObjectType.ContainerRelationshipTypeEntityTypeMapping:
                            LoadContainerRelationshipTypeEntityTypeMappings(dataTable);
                            break;

                        case ObjectType.ContainerRelationshipTypeEntityTypeMappingCardinality:
                            LoadContainerRelationshipTypeEntityTypeCardinalities(dataTable);
                            break;

                        case ObjectType.RelationshipTypeAttributeMapping:
                            LoadRelationshipTypeAttributeMappings(dataTable);
                            break;

                        case ObjectType.ContainerRelationshipTypeAttributeMapping:
                            LoadContainerRelationshipTypeAttributeMappings(dataTable);
                            break;

                        case ObjectType.Role:
                            LoadSecurityRoles(dataTable);
                            break;

                        case ObjectType.User:
                            LoadUsersWithPreferences(dataTable);
                            break;

                        case ObjectType.LookupModel:
                            LoadLookupModels(dataTable);
                            break;

                        case ObjectType.WordList:
                            LoadWordListModels(dataTable);
                            break;
                        case ObjectType.WordElement:
                            LoadWordElementModels(dataTable);
                            break;

                        case ObjectType.EntityVariantDefinition:
                            LoadEntityVariantDefinitions(dataTable);
                            break;

                        case ObjectType.EntityVariantDefinitionMapping:
                            LoadEntityVariantDefinitionMappings(dataTable);
                            break;
                    }
                }
            }
            else
            {
                // ToDo - Prasad - only during development. This will be replaced with validation message after all sheets are implemented
                throw new NotImplementedException(String.Format("DataModel ObjectType: [{0}] - not implemented yet", dataModelObjectType.ToString()));
            }

            if (Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Information, String.Format("GetAllDataModelObjects Completed for [{0}]", dataModelObjectType.ToString()), MDMTraceSource.DataModelImport);
            }
        }

        // Meta data
        /// <summary>
        /// Prepares Organization  collection by from excel rows 
        /// </summary>
        /// <param name="dataTable">Represents a single row read from excel sheet</param>
        private void FillMetadata(DataTable dataTable)
        {
            lock (_readerLock)
            {
                if (_metadata.Count > 1)
                {
                    // initialize only once
                    return;
                }

                String configName = String.Empty;
                String configValue = String.Empty;

                foreach (DataRow row in dataTable.Rows)
                {
                    configName = row[DataModelDictionary.MetadataDictionary[DataModelMetadata.SheetName]].ToString().Trim();

                    if (!String.IsNullOrEmpty(configName) && DataModelDictionary.ConfigToObjectTypeMap.ContainsKey(configName))
                    {
                        configValue = row[DataModelDictionary.MetadataDictionary[DataModelMetadata.Processdatasheet]].ToString().Trim();

                        if (!String.IsNullOrEmpty(configValue))
                        {
                            _metadata.Add(DataModelDictionary.ConfigToObjectTypeMap[configName], ValueTypeHelper.BooleanTryParse(configValue, false));
                        }
                    }
                } //foreach data row
            } // lock
        }

        /// <summary>
        /// Prepares Configuration collection by from excel rows 
        /// </summary>
        /// <param name="dataTable">Represents data table containing Configuration</param>
        private void FillConfiguration(DataTable dataTable)
        {
            lock (_readerLock)
            {
                if (_configuration.Count > 1)
                {
                    // initialize only once
                    return;
                }

                String configName = String.Empty;
                String configValue = String.Empty;

                foreach (DataRow row in dataTable.Rows)
                {
                    configName = row[DataModelDictionary.ConfigurationDictionary[DataModelConfiguration.ConfigurationKey]].ToString().Trim();
                    
                    if (!String.IsNullOrEmpty(configName) && DataModelDictionary.ConfigurationItemsMap.ContainsKey(configName))
                    {
                        configValue = row[DataModelDictionary.ConfigurationDictionary[DataModelConfiguration.ConfigurationValue]].ToString().Trim();

                        if (!String.IsNullOrEmpty(configValue))
                        {
                            _configuration.Add(DataModelDictionary.ConfigurationItemsMap[configName], configValue);
                        }
                    }
                } //foreach data row
            } // lock
        }

        // S1 - Organization
        /// <summary>
        /// Prepares Organization  collection from excel rows 
        /// </summary>
        /// <param name="dataTable">Represents a single row read from excel sheet</param>
        private void LoadOrganizations(DataTable dataTable)
        {
            _levelBasedDictionary = new Dictionary<Int16, IDataModelObjectCollection>();

            Int32 counter = 2;
            Dictionary<String, Int16> organizationNameBasedDictionary = new Dictionary<String, Int16>();
            Dictionary<Int16, OrganizationCollection> levelBasedOrganizationDictionary = new Dictionary<Int16, OrganizationCollection>();

            foreach (DataRow dataRow in dataTable.Rows)
            {
                ObjectAction objectAction = GetActionFromDataRow(dataRow);
                Int16 level = LEVEL_ONE;

                if (objectAction == ObjectAction.Read || objectAction == ObjectAction.Ignore)
                    continue;

                Organization organization = new Organization()
                {
                    ReferenceId = counter.ToString(),
                    Action = objectAction,
                    ExternalId = dataRow[DataModelDictionary.OrganizationDictionary[DataModelOrganization.OrganizationLongName]].ToString().Trim(),
                    Name = dataRow[DataModelDictionary.OrganizationDictionary[DataModelOrganization.OrganizationName]].ToString().Trim(),
                    LongName = dataRow[DataModelDictionary.OrganizationDictionary[DataModelOrganization.OrganizationLongName]].ToString().Trim(),
                    ParentOrganizationName = dataRow[DataModelDictionary.OrganizationDictionary[DataModelOrganization.OrganizationParentName]].ToString().Trim()
                };

                if (!String.IsNullOrWhiteSpace(organization.ParentOrganizationName))
                {
                    organizationNameBasedDictionary.TryGetValue(organization.ParentOrganizationName, out level);
                    level++;
                }

                AddORUpdateOrganizationInDictionary(level, organization, levelBasedOrganizationDictionary);

                if (!organizationNameBasedDictionary.ContainsKey(organization.Name))
                {
                    organizationNameBasedDictionary.Add(organization.Name, level);
                }

                counter++;
            }

            Int16 i = 1;

            foreach (KeyValuePair<Int16, OrganizationCollection> organizations in levelBasedOrganizationDictionary.OrderBy(o => o.Key))
            {
                _levelBasedDictionary.Add(i, organizations.Value);
                i++;
            }
        }

        // S2 - Hierarchy
        /// <summary>
        /// Prepares Hierarchy collection from excel rows 
        /// </summary>
        /// <param name="dataTable">Represents a single row read from excel sheet</param>
        private void LoadHierarchies(DataTable dataTable)
        {
            _levelBasedDictionary = new Dictionary<Int16, IDataModelObjectCollection>();

            Int32 counter = 2;
            HierarchyCollection hierarchies = new HierarchyCollection();

            foreach (DataRow dataRow in dataTable.Rows)
            {
                ObjectAction objectAction = GetActionFromDataRow(dataRow);

                if (objectAction == ObjectAction.Read || objectAction == ObjectAction.Ignore)
                {
                    continue;
                }

                Hierarchy hierarchy = new Hierarchy()
                {
                    ReferenceId = counter.ToString(),
                    Action = objectAction,
                    ExternalId = dataRow[DataModelDictionary.HierarchyDictionary[DataModelHierarchy.HierarchyName]].ToString().Trim(),
                    Name = dataRow[DataModelDictionary.HierarchyDictionary[DataModelHierarchy.HierarchyName]].ToString().Trim(),
                    LongName = dataRow[DataModelDictionary.HierarchyDictionary[DataModelHierarchy.HierarchyLongName]].ToString().Trim(),
                    LeafNodeOnly = ValueTypeHelper.BooleanTryParse(dataRow[DataModelDictionary.HierarchyDictionary[DataModelHierarchy.LeafNodeOnly]].ToString().Trim(), false)
                };

                hierarchies.Add(hierarchy);
                counter++;
            }

            _levelBasedDictionary.Add(LEVEL_ONE, hierarchies);
            }

        // S3 - Container
        /// <summary>
        /// Prepares Container collection from excel rows 
        /// </summary>
        /// <param name="dataTable">Represents a single row read from excel sheet</param>
        private void LoadContainers(DataTable dataTable)
        {
            _levelBasedDictionary = new Dictionary<Int16, IDataModelObjectCollection>();

            Int32 counter = 2;
            Dictionary<String, Int16> containerNameBasedDictionary = new Dictionary<String, Int16>();
            Dictionary<Int16, ContainerCollection> levelBasedContainerDictionary = new Dictionary<Int16, ContainerCollection>();
            ContainerCollection containers = new ContainerCollection();

            foreach (DataRow dataRow in dataTable.Rows)
            {
                ObjectAction objectAction = GetActionFromDataRow(dataRow);
                Int16 level = LEVEL_ONE;

                if (objectAction == ObjectAction.Read || objectAction == ObjectAction.Ignore)
                {
                    continue;
                }

                ContainerType containerType = ContainerType.Unknown;
                WorkflowType workflowType = WorkflowType.Unknown;
                String secQualifiersCellValue = dataRow[DataModelDictionary.ContainerDictionary[DataModelContainer.ContainerSecondaryQualifiers]].ToString().Trim();

                List<String> containerSecondaryQualifiersList = secQualifiersCellValue.Split(new String[] { Constants.STRING_PATH_SEPARATOR }, StringSplitOptions.RemoveEmptyEntries).ToList();
                Collection<String> containerSecondaryQualifiers = new Collection<String>(containerSecondaryQualifiersList);

                Enum.TryParse(dataRow[DataModelDictionary.ContainerDictionary[DataModelContainer.ContainerType]].ToString().Trim(), out containerType);
                Enum.TryParse(dataRow[DataModelDictionary.ContainerDictionary[DataModelContainer.WorkflowType]].ToString().Trim(), out workflowType);

                Container container = new Container()
                {
                    ReferenceId = counter.ToString(),
                    Action = objectAction,
                    ExternalId = dataRow[DataModelDictionary.ContainerDictionary[DataModelContainer.ContainerName]].ToString().Trim(),
                    Name = dataRow[DataModelDictionary.ContainerDictionary[DataModelContainer.ContainerName]].ToString().Trim(),
                    LongName = dataRow[DataModelDictionary.ContainerDictionary[DataModelContainer.ContainerLongName]].ToString().Trim(),
                    OrganizationShortName = dataRow[DataModelDictionary.ContainerDictionary[DataModelContainer.OrganizationName]].ToString().Trim(),
                    HierarchyShortName = dataRow[DataModelDictionary.ContainerDictionary[DataModelContainer.HierarchyName]].ToString().Trim(),
                    ContainerType = containerType,
                    ContainerQualifierName = dataRow[DataModelDictionary.ContainerDictionary[DataModelContainer.ContainerQualifier]].ToString().Trim(),
                    ContainerSecondaryQualifiers = containerSecondaryQualifiers,
                    ParentContainerName = dataRow[DataModelDictionary.ContainerDictionary[DataModelContainer.ParentContainerName]].ToString().Trim(),
                    NeedsApprovedCopy = ValueTypeHelper.BooleanTryParse(dataRow[DataModelDictionary.ContainerDictionary[DataModelContainer.NeedsApprovedCopy]].ToString().Trim(), false),
                    WorkflowType = workflowType
                };

                String autoExtensionEnabled = DataModelDictionary.ContainerDictionary[DataModelContainer.AutoExtensionEnabled];

                //Check AutoExtensionEnabled column exists or not.
                if (dataTable.Columns.Contains(autoExtensionEnabled))
                {
                    container.AutoExtensionEnabled = ValueTypeHelper.BooleanTryParse(dataRow[autoExtensionEnabled].ToString(), container.AutoExtensionEnabled);
                }

                if (!String.IsNullOrWhiteSpace(container.ParentContainerName))
                {
                    containerNameBasedDictionary.TryGetValue(container.ParentContainerName.ToLowerInvariant(), out level);
                    level++;
                }

                AddORUpdateContainerInDictionary(level, container, levelBasedContainerDictionary);

                if (!containerNameBasedDictionary.ContainsKey(container.NameInLowerCase))
                {
                    containerNameBasedDictionary.Add(container.NameInLowerCase, level);
                }

                counter++;
            }

            Int16 i = 1;

            foreach (KeyValuePair<Int16, ContainerCollection> conatiners in levelBasedContainerDictionary.OrderBy(e => e.Key))
            {
                _levelBasedDictionary.Add(i, conatiners.Value);
                i++;
            }
        }

        // S4 - Container - Locale
        /// <summary>
        /// Prepares Container - Locale collection from excel rows 
        /// </summary>
        /// <param name="dataTable">represents a single row read from excel sheet</param>
        private void LoadContainerLocalizations(DataTable dataTable)
        {
            _levelBasedDictionary = new Dictionary<Int16, IDataModelObjectCollection>();

            Int32 counter = 2;
            ContainerCollection containers = new ContainerCollection();

            foreach (DataRow dataRow in dataTable.Rows)
            {
                ObjectAction objectAction = GetActionFromDataRow(dataRow);

                if (objectAction == ObjectAction.Read || objectAction == ObjectAction.Ignore)
                {
                    continue;
                }

                String containerName = dataRow[DataModelDictionary.ContainerLocalizationDictionary[DataModelContainerLocalization.ContainerName]].ToString().Trim();

                //Try to find container based on name
                Container container = (Container)containers.GetContainerByName(containerName);

                if (container == null)
                {
                    container = new Container();
                    container.ReferenceId = counter.ToString();
                    container.Action = ObjectAction.Create;
                    container.Name = containerName;

                    containers.Add(container);

                    counter++;
                }

                LocaleEnum dataLocale = LocaleEnum.UnKnown;
                ValueTypeHelper.EnumTryParse<LocaleEnum>(dataRow[DataModelDictionary.ContainerLocalizationDictionary[DataModelContainerLocalization.Locale]].ToString().Trim(), true, out dataLocale);

                if (dataLocale != LocaleEnum.UnKnown && (objectAction != ObjectAction.Delete || dataLocale == _systemDataLocale))
                {
                    Locale locale = new Locale() {
                        Id = (Int32)dataLocale,
                        Name = dataLocale.ToString(),
                        LongName = dataLocale.GetDescription(),
                        Locale = dataLocale,
                        Action = objectAction
                    };
                    container.SupportedLocales.Add(locale);
                }
            }

            _levelBasedDictionary.Add(LEVEL_ONE, containers);
        }

        // S5 - Entity Type
        /// <summary>
        /// Prepares Entity type collection from excel rows 
        /// </summary>
        /// <param name="dataTable">Represents a single row read from excel sheet</param>
        private void LoadEntityTypes(DataTable dataTable)
        {
            _levelBasedDictionary = new Dictionary<Int16, IDataModelObjectCollection>();
            EntityTypeCollection entityTypes = new EntityTypeCollection();
            Int32 counter = 2;

            foreach (DataRow dataRow in dataTable.Rows)
            {
                ObjectAction objectAction = GetActionFromDataRow(dataRow);
                
                if (objectAction == ObjectAction.Read || objectAction == ObjectAction.Ignore)
                {
                    continue;
                }

                EntityType entityType = new EntityType()
                {
                    ReferenceId = counter.ToString(),
                    Action = objectAction,
                    ExternalId = dataRow[DataModelDictionary.EntityTypeDictionary[DataModelEntityType.EntityTypeName]].ToString().Trim(),
                    Name = dataRow[DataModelDictionary.EntityTypeDictionary[DataModelEntityType.EntityTypeName]].ToString().Trim(),
                    LongName = dataRow[DataModelDictionary.EntityTypeDictionary[DataModelEntityType.EntityTypeLongName]].ToString().Trim()
                };

                entityTypes.Add(entityType);
                counter++;
            }

            _levelBasedDictionary.Add(LEVEL_ONE, entityTypes);
        }

        // S6 - Relationship Type
        /// <summary>
        /// Prepares Relationship Type collection from excel rows 
        /// </summary>
        /// <param name="dataTable">Represents a single row read from excel sheet</param>
        private void LoadRelationshipTypes(DataTable dataTable)
        {
            _levelBasedDictionary = new Dictionary<Int16, IDataModelObjectCollection>();

            Int32 counter = 2;
            RelationshipTypeCollection relationshipTypes = new RelationshipTypeCollection();

            foreach (DataRow dataRow in dataTable.Rows)
            {
                ObjectAction objectAction = GetActionFromDataRow(dataRow);

                if (objectAction == ObjectAction.Read || objectAction == ObjectAction.Ignore)
                {
                    continue;
                }

                RelationshipType relationshipType = new RelationshipType()
                {
                    ReferenceId = counter.ToString(),
                    Action = objectAction,
                    ExternalId = dataRow[DataModelDictionary.RelationshipTypeDictionary[DataModelRelationshipType.RelationshipTypeName]].ToString().Trim(),
                    Name = dataRow[DataModelDictionary.RelationshipTypeDictionary[DataModelRelationshipType.RelationshipTypeName]].ToString().Trim(),
                    LongName = dataRow[DataModelDictionary.RelationshipTypeDictionary[DataModelRelationshipType.RelationshipTypeLongName]].ToString().Trim(),
                    EnforceRelatedEntityStateOnSourceEntity = ValueTypeHelper.BooleanTryParse(dataRow[DataModelDictionary.RelationshipTypeDictionary[DataModelRelationshipType.EnforceRelatedEntityStateOnSourceEntity]].ToString().Trim(), false),
                    CheckRelatedEntityPromoteStatusOnPromote = ValueTypeHelper.BooleanTryParse(dataRow[DataModelDictionary.RelationshipTypeDictionary[DataModelRelationshipType.CheckRelatedEntityPromoteStatusOnPromote]].ToString().Trim(), false)
                };

                relationshipTypes.Add(relationshipType);
                counter++;
            }

            _levelBasedDictionary.Add(LEVEL_ONE, relationshipTypes);
        }

        // S7 - Attribute
        /// <summary>
        /// Prepares Attribute Model collection from excel rows 
        /// </summary>
        /// <param name="dataTable">Represents a single row read from excel sheet</param>
        private void LoadAttributeModels(DataTable dataTable)
        {
            _levelBasedDictionary = new Dictionary<Int16, IDataModelObjectCollection>();
            AttributeModelCollection simpleAttributeModelCollection = null;

            Int32 counter = 2;
            Dictionary<Int16, AttributeModelCollection> levelBasedAttributeModelDictionary = new Dictionary<Int16, AttributeModelCollection>();

            simpleAttributeModelCollection = new AttributeModelCollection();

            foreach (DataRow dataRow in dataTable.Rows)
            {
                ObjectAction objectAction = GetActionFromDataRow(dataRow);
                String attributeModelType = dataRow[DataModelDictionary.AttributeModelDictionary[DataModelAttributeModel.AttributeType]].ToString().Trim();

                Int16 level = 0;
                AttributeModelCollection attributeModels = null;

                if (objectAction == ObjectAction.Read || objectAction == ObjectAction.Ignore)
                {
                    continue;
                }

                AttributeModelBaseProperties attributeModelBaseProperties = new AttributeModelBaseProperties()
                {
                    //ExternalId = dataRow[DataModelDictionary.RelationshipTypeDictionary[DataModelRelationshipType.RelationshipTypeName]].ToString(),
                    Name = dataRow[DataModelDictionary.AttributeModelDictionary[DataModelAttributeModel.AttributeName]].ToString().Trim(),
                    LongName = dataRow[DataModelDictionary.AttributeModelDictionary[DataModelAttributeModel.AttributeLongName]].ToString().Trim(),
                    AttributeParentName = dataRow[DataModelDictionary.AttributeModelDictionary[DataModelAttributeModel.AttributeParentName]].ToString().Trim(),
                    AttributeDataTypeName = dataRow[DataModelDictionary.AttributeModelDictionary[DataModelAttributeModel.DataType]].ToString().Trim(),
                    AttributeDisplayTypeName = dataRow[DataModelDictionary.AttributeModelDictionary[DataModelAttributeModel.DisplayType]].ToString().Trim(),
                    IsCollection = ValueTypeHelper.BooleanTryParse(dataRow[DataModelDictionary.AttributeModelDictionary[DataModelAttributeModel.IsCollection]].ToString(), false),
                    Inheritable = ValueTypeHelper.BooleanTryParse(dataRow[DataModelDictionary.AttributeModelDictionary[DataModelAttributeModel.IsInheritable]].ToString(), false),
                    IsLocalizable = ValueTypeHelper.BooleanTryParse(dataRow[DataModelDictionary.AttributeModelDictionary[DataModelAttributeModel.IsLocalizable]].ToString(), false),
                    IsComplex = ValueTypeHelper.BooleanTryParse(dataRow[DataModelDictionary.AttributeModelDictionary[DataModelAttributeModel.IsComplex]].ToString(), false),
                    IsLookup = ValueTypeHelper.BooleanTryParse(dataRow[DataModelDictionary.AttributeModelDictionary[DataModelAttributeModel.IsLookup]].ToString(), false),
                    Required = ValueTypeHelper.BooleanTryParse(dataRow[DataModelDictionary.AttributeModelDictionary[DataModelAttributeModel.IsRequired]].ToString(), false),
                    ReadOnly = ValueTypeHelper.BooleanTryParse(dataRow[DataModelDictionary.AttributeModelDictionary[DataModelAttributeModel.IsReadOnly]].ToString(), false),
                    IsHidden = ValueTypeHelper.BooleanTryParse(dataRow[DataModelDictionary.AttributeModelDictionary[DataModelAttributeModel.IsHidden]].ToString(), false),
                    ShowAtCreation = ValueTypeHelper.BooleanTryParse(dataRow[DataModelDictionary.AttributeModelDictionary[DataModelAttributeModel.ShowAtEntityCreation]].ToString(), false),
                    Searchable = ValueTypeHelper.BooleanTryParse(dataRow[DataModelDictionary.AttributeModelDictionary[DataModelAttributeModel.IsSearchable]].ToString(), false),
                    AllowNullSearch = ValueTypeHelper.BooleanTryParse(dataRow[DataModelDictionary.AttributeModelDictionary[DataModelAttributeModel.IsNullValueSearchRequired]].ToString(), false),
                    DefaultValue = dataRow[DataModelDictionary.AttributeModelDictionary[DataModelAttributeModel.DefaultValue]].ToString().Trim(),
                    UomType = dataRow[DataModelDictionary.AttributeModelDictionary[DataModelAttributeModel.UOMType]].ToString().Trim(),
                    AllowableUOM = dataRow[DataModelDictionary.AttributeModelDictionary[DataModelAttributeModel.AllowedUOMs]].ToString().Trim(),
                    DefaultUOM = dataRow[DataModelDictionary.AttributeModelDictionary[DataModelAttributeModel.DefaultUOM]].ToString().Trim(),
                    LookUpTableName = dataRow[DataModelDictionary.AttributeModelDictionary[DataModelAttributeModel.LookUpTableName]].ToString().Trim(),
                    LkDisplayColumns = dataRow[DataModelDictionary.AttributeModelDictionary[DataModelAttributeModel.LookupDisplayColumns]].ToString().Trim(),
                    LkSearchColumns = dataRow[DataModelDictionary.AttributeModelDictionary[DataModelAttributeModel.LookupSearchColumns]].ToString().Trim(),
                    LkDisplayFormat = dataRow[DataModelDictionary.AttributeModelDictionary[DataModelAttributeModel.LookupDisplayFormat]].ToString().Trim(),
                    LkSortOrder = dataRow[DataModelDictionary.AttributeModelDictionary[DataModelAttributeModel.LookupSortOrder]].ToString().Trim(),
                    ExportMask = dataRow[DataModelDictionary.AttributeModelDictionary[DataModelAttributeModel.ExportFormat]].ToString().Trim(),
                    SortOrder = ValueTypeHelper.Int32TryParse(dataRow[DataModelDictionary.AttributeModelDictionary[DataModelAttributeModel.SortOrder]].ToString(), 0),
                    Definition = dataRow[DataModelDictionary.AttributeModelDictionary[DataModelAttributeModel.Definition]].ToString().Trim(),
                    AttributeExample = dataRow[DataModelDictionary.AttributeModelDictionary[DataModelAttributeModel.Example]].ToString().Trim(),
                    BusinessRule = dataRow[DataModelDictionary.AttributeModelDictionary[DataModelAttributeModel.BusinessRule]].ToString().Trim(),
                    Label = dataRow[DataModelDictionary.AttributeModelDictionary[DataModelAttributeModel.Label]].ToString().Trim(),
                    Extension = dataRow[DataModelDictionary.AttributeModelDictionary[DataModelAttributeModel.Extension]].ToString().Trim(),
                    WebUri = dataRow[DataModelDictionary.AttributeModelDictionary[DataModelAttributeModel.WebURI]].ToString().Trim(),

                    //Default value for ApplyLocaleFormat and ApplyTimeZoneConversion is True. Since we are not getting any value for these columns from data model import so setting value as false.
                    ApplyLocaleFormat = false,
                    ApplyTimeZoneConversion = false
                };

                String isPrecisionArbitary = DataModelDictionary.AttributeModelDictionary[DataModelAttributeModel.IsPrecisionArbitrary];
                String enableHistory = DataModelDictionary.AttributeModelDictionary[DataModelAttributeModel.EnableHistory];
                String applyTimeZoneConversion = DataModelDictionary.AttributeModelDictionary[DataModelAttributeModel.ApplyTimeZoneConversion];

                if ((String.Compare(attributeModelBaseProperties.AttributeDataTypeName, AttributeDataType.String.ToString(), StringComparison.InvariantCultureIgnoreCase)) == 0 &&
                    (String.Compare(attributeModelBaseProperties.AttributeDisplayTypeName, AttributeDisplayType.DropDown.ToString(), StringComparison.InvariantCultureIgnoreCase)) == 0)
                {
                    attributeModelBaseProperties.AllowableValues = dataRow[DataModelDictionary.AttributeModelDictionary[DataModelAttributeModel.AllowableValues]].ToString().Trim();
                }

                if ((String.Compare(attributeModelBaseProperties.AttributeDataTypeName, AttributeDataType.Decimal.ToString(), StringComparison.InvariantCultureIgnoreCase) == 0) &&
                    (String.Compare(attributeModelBaseProperties.AttributeDisplayTypeName, AttributeDisplayType.NumericTextBox.ToString(), StringComparison.InvariantCultureIgnoreCase) == 0))
                {
                    attributeModelBaseProperties.Precision = ValueTypeHelper.Int32TryParse(dataRow[DataModelDictionary.AttributeModelDictionary[DataModelAttributeModel.Precision]].ToString(), 0);
                }
                else
                {
                    //Default value is -1 and DB updates the record in DB with -1. But from UI, the value comes as 0.
                    attributeModelBaseProperties.Precision = 0;
                }

                if ((String.Compare(attributeModelBaseProperties.AttributeDataTypeName, AttributeDataType.Date.ToString(), StringComparison.InvariantCultureIgnoreCase) == 0) ||
                    (String.Compare(attributeModelBaseProperties.AttributeDataTypeName, AttributeDataType.Integer.ToString(), StringComparison.InvariantCultureIgnoreCase) == 0) ||
                    (String.Compare(attributeModelBaseProperties.AttributeDataTypeName, AttributeDataType.Decimal.ToString(), StringComparison.InvariantCultureIgnoreCase) == 0) ||
                    (String.Compare(attributeModelBaseProperties.AttributeDataTypeName, AttributeDataType.Fraction.ToString(), StringComparison.InvariantCultureIgnoreCase) == 0) ||
                    (String.Compare(attributeModelBaseProperties.AttributeDataTypeName, AttributeDataType.DateTime.ToString(), StringComparison.InvariantCultureIgnoreCase) == 0))
                {
                    String rangeFrom = dataRow[DataModelDictionary.AttributeModelDictionary[DataModelAttributeModel.RangeFrom]].ToString().Trim(),
                        rangeTo = dataRow[DataModelDictionary.AttributeModelDictionary[DataModelAttributeModel.RangeTo]].ToString().Trim(),
                        isRangeFromInclusive = dataRow[DataModelDictionary.AttributeModelDictionary[DataModelAttributeModel.IsRangeFromInclusive]].ToString().Trim(),
                        isRangeToInclusive = dataRow[DataModelDictionary.AttributeModelDictionary[DataModelAttributeModel.IsRangeToInclusive]].ToString().Trim();
                    attributeModelBaseProperties.RangeFrom = rangeFrom;
                    attributeModelBaseProperties.RangeTo = rangeTo;
                    attributeModelBaseProperties.MinInclusive = GetRange(rangeFrom, isRangeFromInclusive, isInclusiveVariable: true);
                    attributeModelBaseProperties.MinExclusive = GetRange(rangeFrom, isRangeFromInclusive, isInclusiveVariable: false);
                    attributeModelBaseProperties.MaxInclusive = GetRange(rangeTo, isRangeToInclusive, isInclusiveVariable: true);
                    attributeModelBaseProperties.MaxExclusive = GetRange(rangeTo, isRangeToInclusive, isInclusiveVariable: false);
                }

                if ((String.Compare(attributeModelBaseProperties.AttributeDataTypeName, AttributeDataType.String.ToString(), StringComparison.InvariantCultureIgnoreCase) == 0) &&
                    (String.Compare(attributeModelBaseProperties.AttributeDisplayTypeName, AttributeDisplayType.LookupTable.ToString(), StringComparison.InvariantCultureIgnoreCase) != 0))
                {
                    attributeModelBaseProperties.MinLength = ValueTypeHelper.Int32TryParse(dataRow[DataModelDictionary.AttributeModelDictionary[DataModelAttributeModel.MinimumLength]].ToString(), 0);
                    attributeModelBaseProperties.MaxLength = ValueTypeHelper.Int32TryParse(dataRow[DataModelDictionary.AttributeModelDictionary[DataModelAttributeModel.MaximumLength]].ToString(), 0);
                }

                DataColumnCollection columns = dataTable.Columns;

                //Check IsPrecisionArbitartColumn exists or not.
                if (columns.Contains(isPrecisionArbitary))
                {
                    attributeModelBaseProperties.IsPrecisionArbitrary = ValueTypeHelper.BooleanTryParse(dataRow[isPrecisionArbitary].ToString(), attributeModelBaseProperties.IsPrecisionArbitrary);
                }
                if (columns.Contains(enableHistory))
                {
                    attributeModelBaseProperties.EnableHistory = ValueTypeHelper.BooleanTryParse(dataRow[enableHistory].ToString(), attributeModelBaseProperties.EnableHistory);
                }
                if (columns.Contains(applyTimeZoneConversion))
                {
                    attributeModelBaseProperties.ApplyTimeZoneConversion = ValueTypeHelper.BooleanTryParse(dataRow[applyTimeZoneConversion].ToString(), attributeModelBaseProperties.ApplyTimeZoneConversion);
                }

                //Not considering below proerties when Attribute Data Type is Hierarchical
                if (attributeModelBaseProperties.AttributeDataTypeName.ToLower().Equals("hierarchical"))
                {
                    attributeModelBaseProperties.IsLocalizable = false;
                    attributeModelBaseProperties.Searchable = false;
                    attributeModelBaseProperties.AllowNullSearch = false;
                    attributeModelBaseProperties.UomType = "";
                    attributeModelBaseProperties.AllowableUOM = "";
                    attributeModelBaseProperties.DefaultUOM = "";
                    attributeModelBaseProperties.ComplexTableName = "Not Applicable";
                }

                String attributeRegEx = DataModelDictionary.AttributeModelDictionary[DataModelAttributeModel.AttributeRegExp];

                columns = dataTable.Columns;

                //Check AttributeRegEx exists or not.
                if (columns.Contains(attributeRegEx))
                {
                    attributeModelBaseProperties.AttributeRegEx = dataRow[attributeRegEx].ToString().Trim(); 
                }

                attributeModelBaseProperties.AttributeModelType = GetAttributeTypeFromDataRow(attributeModelType);

                AttributeModel attributeModel = new AttributeModel(attributeModelBaseProperties, null, null);
                attributeModel.ReferenceId = counter.ToString();
                attributeModel.Action = objectAction;
                attributeModel.Locale = _systemDataLocale;

                if (attributeModel.AttributeModelType == AttributeModelType.CommonAttributeGroup
                    || attributeModel.AttributeModelType == AttributeModelType.CategoryAttributeGroup
                    || attributeModel.AttributeModelType == AttributeModelType.RelationshipAttributeGroup)
                {
                    //Setting level 1 for the attribute groups
                    level = 1;
                }
                else if (attributeModel.AttributeDataTypeName.ToLower().Equals("hierarchical"))
                {
                    AttributeModel parentAttributeModel = null;
                    //Finding parent from exisitng model dictionary

                    foreach (KeyValuePair<Int16, AttributeModelCollection> parentAttributeModels in levelBasedAttributeModelDictionary.OrderBy(a => a.Key))
                    {
                        parentAttributeModel = null;
                        parentAttributeModel = parentAttributeModels.Value.FirstOrDefault(parent => parent.Name.ToLower().Equals(attributeModel.AttributeParentName.ToLower()));
                        if (parentAttributeModel != null)
                        {
                            level = parentAttributeModels.Key;
                            level++;
                            break;
                        }
                    }

                    if (level == 0)
                    {
                        /*Level 3 when Hierarchical attributes parent name(Attribute Group) is not defined as a row in DMD sheet
                            * Above foreach loop returns level as 0 and hierarchical attributes would be added into simple attribute collection 
                            * Now Hierarchical simple child attributes also gets added into simple attribute collection which leads to processing failed.
                            * To address this problem for Hierarchical attributes with out parent definition in DMD sheet would be mapped to level 3
                            Hierarchical attribute will fall under simple attribute */
                        level = 3;
                    }
                }
                else if (attributeModel.IsComplex)
                {
                    level = 2;
                }

                if (level == 0)
                {
                    simpleAttributeModelCollection.Add(attributeModel, true);
                }
                else if (!levelBasedAttributeModelDictionary.ContainsKey(level))
                {
                    attributeModels = new AttributeModelCollection() { attributeModel };
                    levelBasedAttributeModelDictionary.Add(level, attributeModels);
                }
                else
                {
                    attributeModels = levelBasedAttributeModelDictionary[level];
                    attributeModels.Add(attributeModel, true);
                }

                counter++;
            }

            // Process simple attributes at the end of all others (attribute groups, complex and hierarchy attributes)
            if (simpleAttributeModelCollection != null && simpleAttributeModelCollection.Count > 0)
            {
                Int16 maxLevel = levelBasedAttributeModelDictionary.Any() ? levelBasedAttributeModelDictionary.Max(level => level.Key) : (Int16)0;

                maxLevel++;
                levelBasedAttributeModelDictionary.Add(maxLevel, simpleAttributeModelCollection);
            }

            Int16 i = 1;

            foreach (KeyValuePair<Int16, AttributeModelCollection> attributeModels in levelBasedAttributeModelDictionary.OrderBy(a => a.Key))
            {
                _levelBasedDictionary.Add(i, attributeModels.Value);
                i++;
            }
        }

        //S8 - Attribute - Locale
        /// <summary>
        /// Prepares attribute model collection for localized from excel rows 
        /// </summary>
        /// <param name="dataTable">Represents a single row read from excel sheet</param>
        private void LoadAttributeModelLocalization(DataTable dataTable)
        {
            _levelBasedDictionary = new Dictionary<Int16, IDataModelObjectCollection>();

            Int32 counter = 2;
            AttributeModelCollection attributeModels = new AttributeModelCollection();

            foreach (DataRow dataRow in dataTable.Rows)
            {
                String attributeLongName = dataRow[DataModelDictionary.AttributeModelLocalizationDictionary[DataModelAttributeModelLocalization.AttributeLongName]].ToString().Trim();

                if (String.Compare(attributeLongName, RSExcelConstants.BlankText, true) == 0)
                {
                    continue;
                }

                String attributePath = String.Empty;
                String attributeName = String.Empty;
                String attributeParentName = String.Empty;
                AttributeModelBaseProperties attributeModelBaseProperties = new AttributeModelBaseProperties();

                ObjectAction objectAction = GetActionFromDataRow(dataRow);

                if (objectAction == ObjectAction.Read || objectAction == ObjectAction.Ignore)
                {
                    continue;
                }

                LocaleEnum dataLocale = LocaleEnum.UnKnown;
                String locale = dataRow[DataModelDictionary.AttributeModelLocalizationDictionary[DataModelAttributeModelLocalization.LocaleName]].ToString().Trim();
                ValueTypeHelper.EnumTryParse<LocaleEnum>(locale, true, out dataLocale);

                attributePath = dataRow[DataModelDictionary.AttributeModelLocalizationDictionary[DataModelAttributeModelLocalization.AttributePath]].ToString().Trim();
                PopulateAttributeNameAndParentNameBasedOnPath(attributePath, ref attributeName, ref attributeParentName);

                if (dataLocale != _systemDataLocale)
                {
                    attributeModelBaseProperties.AttributeParentName = attributeParentName;
                    attributeModelBaseProperties.Name = attributeName;
                    attributeModelBaseProperties.LongName = attributeLongName;
                    attributeModelBaseProperties.MinLength = ValueTypeHelper.Int32TryParse(dataRow[DataModelDictionary.AttributeModelLocalizationDictionary[DataModelAttributeModelLocalization.MinimumLength]].ToString(), attributeModelBaseProperties.MinLength);
                    attributeModelBaseProperties.MaxLength = ValueTypeHelper.Int32TryParse(dataRow[DataModelDictionary.AttributeModelLocalizationDictionary[DataModelAttributeModelLocalization.MaximumLength]].ToString(), attributeModelBaseProperties.MaxLength);

                    String definition = DataModelDictionary.AttributeModelLocalizationDictionary[DataModelAttributeModelLocalization.Definition];
                    String attributeExample = DataModelDictionary.AttributeModelLocalizationDictionary[DataModelAttributeModelLocalization.AttributeExample];
                    String businessRule = DataModelDictionary.AttributeModelLocalizationDictionary[DataModelAttributeModelLocalization.BusinessRule];
                    DataColumnCollection columns = dataTable.Columns;

                    if (columns.Contains(definition))
                    {
                        attributeModelBaseProperties.Definition = dataRow[definition].ToString().Trim();
                    }
                    if (columns.Contains(attributeExample))
                    {
                        attributeModelBaseProperties.AttributeExample = dataRow[attributeExample].ToString().Trim();
                    }
                    if (columns.Contains(businessRule))
                    {
                        attributeModelBaseProperties.BusinessRule = dataRow[businessRule].ToString().Trim();
                    }

                    AttributeModel attributeModel = new AttributeModel(attributeModelBaseProperties, null, null);
                    attributeModel.ReferenceId = counter.ToString();
                    attributeModel.Locale = dataLocale;
                    attributeModel.Action = objectAction;

                    attributeModels.Add(attributeModel, true);
                    counter++;
                }
                else
                {
                    if (Constants.TRACING_ENABLED)
                    {
                        String sheetName = DataModelDictionary.ObjectsDictionary[ObjectType.AttributeModelLocalization];
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Warning, String.Format("'{0}' : Attribute '{1}' under '{2}' is specified in system data locale, attribute will not be processed further.", sheetName, attributeName, attributeParentName), MDMTraceSource.DataModelImport);
                    }
                }
            }

            _levelBasedDictionary.Add(LEVEL_ONE, attributeModels);
        }

        //S9 - Category
        /// <summary>
        /// Prepares category collection from excel rows 
        /// </summary>
        /// <param name="dataTable">Represents a single row read from excel sheet</param>
        private void LoadCategories(DataTable dataTable)
        {
            _levelBasedDictionary = new Dictionary<Int16, IDataModelObjectCollection>();

            Int32 counter = 2;
            Dictionary<Int16, CategoryCollection> levelBasedCategoryDictionary = new Dictionary<Int16, CategoryCollection>();

            foreach (DataRow dataRow in dataTable.Rows)
            {
                ObjectAction objectAction = GetActionFromDataRow(dataRow);
                String[] splittedCategoryPath = null;
                Int16 Key = 0;

                if (objectAction == ObjectAction.Read || objectAction == ObjectAction.Ignore)
                {
                    continue;
                }

                CategoryBaseProperties categoryBaseProperties = new CategoryBaseProperties()
                {
                    Name = dataRow[DataModelDictionary.CategoryDictionary[DataModelCategory.CategoryName]].ToString().Trim(),
                    LongName = dataRow[DataModelDictionary.CategoryDictionary[DataModelCategory.CategoryLongName]].ToString().Trim(),
                    HierarchyName = dataRow[DataModelDictionary.CategoryDictionary[DataModelCategory.HierarchyName]].ToString().Trim(),
                    Path = dataRow[DataModelDictionary.CategoryDictionary[DataModelCategory.CategoryName]].ToString().Trim(),
                    Locale = _systemDataLocale
                };

                String parentCategoryPath = dataRow[DataModelDictionary.CategoryDictionary[DataModelCategory.ParentCategoryPath]].ToString().Trim();

                if (!String.IsNullOrWhiteSpace(parentCategoryPath))
                {
                    splittedCategoryPath = parentCategoryPath.Split(new String[] { _separator }, StringSplitOptions.RemoveEmptyEntries);
                    categoryBaseProperties.ParentCategoryName = (splittedCategoryPath != null && splittedCategoryPath.Length > 0) ? splittedCategoryPath[splittedCategoryPath.Length - 1] : categoryBaseProperties.ParentCategoryName;
                    categoryBaseProperties.Path = String.Concat(parentCategoryPath.Replace(_separator, _categoryPathSeparator), _categoryPathSeparator, categoryBaseProperties.Name);
                }

                Category category = new Category(categoryBaseProperties, null);
                category.ReferenceId = counter.ToString();
                category.Locale = _systemDataLocale;
                category.Action = objectAction;

                //If parent category path is not available then assume as root category.
                Key = (splittedCategoryPath == null || splittedCategoryPath.Length < 1) ? LEVEL_ONE : (Int16)(splittedCategoryPath.Length + 1);

                AddORUpdateCategoryInDictionary(Key, category, levelBasedCategoryDictionary);

                counter++;
            }

            Int16 i = 1;

            foreach (KeyValuePair<Int16, CategoryCollection> categories in levelBasedCategoryDictionary.OrderBy(c => c.Key))
            {
                _levelBasedDictionary.Add(i, categories.Value);
                i++;
            }
        }

        //S10 - Category - Locale
        /// <summary>
        /// Prepares category collection for localized values from excel rows 
        /// </summary>
        /// <param name="dataTable">Represents a single row read from excel sheet</param>
        private void LoadCategoryLocalizations(DataTable dataTable)
        {
            _levelBasedDictionary = new Dictionary<Int16, IDataModelObjectCollection>();

            Int32 counter = 2;
            CategoryLocalePropertiesCollection categoryLocalePropertiesCollection = new CategoryLocalePropertiesCollection();

            foreach (DataRow dataRow in dataTable.Rows)
            {
                String longName = dataRow[DataModelDictionary.CategoryLocalizationDictionary[DataModelCategoryLocalization.CategoryLongName]].ToString().Trim();

                if (longName.Equals(RSExcelConstants.BlankText))
                {
                    continue;
                }

                ObjectAction objectAction = GetActionFromDataRow(dataRow);

                if (objectAction == ObjectAction.Read || objectAction == ObjectAction.Ignore)
                    continue;

                LocaleEnum dataLocale = LocaleEnum.UnKnown;
                ValueTypeHelper.EnumTryParse<LocaleEnum>(dataRow[DataModelDictionary.CategoryLocalizationDictionary[DataModelCategoryLocalization.Locale]].ToString().Trim(), true, out dataLocale);

                String categoryName = dataRow[DataModelDictionary.CategoryLocalizationDictionary[DataModelCategoryLocalization.CategoryName]].ToString().Trim();
                String parentCategoryPath = dataRow[DataModelDictionary.CategoryDictionary[DataModelCategory.ParentCategoryPath]].ToString().Trim();
                String hierarchyName = dataRow[DataModelDictionary.CategoryLocalizationDictionary[DataModelCategoryLocalization.HierarchyName]].ToString().Trim();

                if (dataLocale != _systemDataLocale)
                {
                    CategoryLocaleProperties categoryLocaleProperties = new CategoryLocaleProperties()
                    {
                        ReferenceId = counter.ToString(),
                        Name = categoryName,
                        LongName = longName,
                        HierarchyName = hierarchyName,
                        Path = categoryName,
                        Locale = dataLocale,
                        Action = objectAction
                    };

                    if (!String.IsNullOrWhiteSpace(parentCategoryPath))
                    {
                        categoryLocaleProperties.Path = String.Concat(parentCategoryPath.Replace(_separator, _categoryPathSeparator), _categoryPathSeparator, categoryLocaleProperties.Name);
                    }

                    categoryLocalePropertiesCollection.Add(categoryLocaleProperties);
                    counter++;
                }
                else
                {
                    if (Constants.TRACING_ENABLED)
                    {
                        String sheetName = DataModelDictionary.ObjectsDictionary[ObjectType.CategoryLocalization];
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Warning, String.Format(" '{0}' : Category '{1}' under '{2}' for hierarchy '{3}' is specified in system data locale, category will not be processed further.", sheetName, categoryName, parentCategoryPath, hierarchyName), MDMTraceSource.DataModelImport);
                    }
                }
            }

            _levelBasedDictionary.Add(LEVEL_ONE, categoryLocalePropertiesCollection);
        }

        //// S11 - Container - Entity Type
        /// <summary>
        /// Prepares Container Entity Type Mapping collection from excel rows 
        /// </summary>
        /// <param name="dataTable">Represents a single row read from excel sheet</param>
        private void LoadContainerEntityTypeMappings(DataTable dataTable)
        {
            _levelBasedDictionary = new Dictionary<Int16, IDataModelObjectCollection>();

            Int32 counter = 2;
            ContainerEntityTypeMappingCollection containerEntityTypeMappings = new ContainerEntityTypeMappingCollection();

            foreach (DataRow dataRow in dataTable.Rows)
            {
                ObjectAction objectAction = GetActionFromDataRow(dataRow);

                if (objectAction == ObjectAction.Read || objectAction == ObjectAction.Ignore)
                {
                    continue;
                }

                ContainerEntityTypeMapping containerEntityTypeMapping = new ContainerEntityTypeMapping();
                containerEntityTypeMapping.ReferenceId = counter.ToString();
                containerEntityTypeMapping.Action = objectAction;
                containerEntityTypeMapping.OrganizationName = dataRow[DataModelDictionary.ContainerEntityTypeMappingDictionary[DataModelContainerEntityTypeMapping.OrganizationName]].ToString().Trim();
                containerEntityTypeMapping.ContainerName = dataRow[DataModelDictionary.ContainerEntityTypeMappingDictionary[DataModelContainerEntityTypeMapping.ContainerName]].ToString().Trim();
                containerEntityTypeMapping.EntityTypeName = dataRow[DataModelDictionary.ContainerEntityTypeMappingDictionary[DataModelContainerEntityTypeMapping.EntityTypeName]].ToString().Trim();
                containerEntityTypeMapping.ShowAtCreation = ValueTypeHelper.BooleanTryParse(dataRow[DataModelDictionary.ContainerEntityTypeMappingDictionary[DataModelContainerEntityTypeMapping.ShowAtCreation]].ToString().Trim(), false);
                containerEntityTypeMapping.MinimumExtensions = ValueTypeHelper.Int32TryParse(dataRow[DataModelDictionary.ContainerEntityTypeMappingDictionary[DataModelContainerEntityTypeMapping.MinimumExtensions]].ToString().Trim(), 0);
                containerEntityTypeMapping.MaximumExtensions = ValueTypeHelper.Int32TryParse(dataRow[DataModelDictionary.ContainerEntityTypeMappingDictionary[DataModelContainerEntityTypeMapping.MaximumExtensions]].ToString().Trim(), 0);
                containerEntityTypeMapping.ExternalId = String.Format("[{1}]{0}[{2}]{0}[{3}]", _separator, containerEntityTypeMapping.OrganizationName, containerEntityTypeMapping.ContainerName, containerEntityTypeMapping.EntityTypeName);

                containerEntityTypeMappings.Add(containerEntityTypeMapping);
                counter++;
            }

            _levelBasedDictionary.Add(LEVEL_ONE, containerEntityTypeMappings);
        }

        // S12 - Entity Type - Attribute
        /// <summary>
        /// Prepares Entity Type Attribute Mapping collection from excel rows 
        /// </summary>
        /// <param name="dataTable">Represents a single row read from excel sheet</param>
        private void LoadEntityTypeAttributeMappings(DataTable dataTable)
        {
            _levelBasedDictionary = new Dictionary<Int16, IDataModelObjectCollection>();

            Int32 counter = 2;
            EntityTypeAttributeMappingCollection entityTypeAttributeMappings = new EntityTypeAttributeMappingCollection();

            foreach (DataRow dataRow in dataTable.Rows)
            {
                ObjectAction objectAction = GetActionFromDataRow(dataRow);

                if (objectAction == ObjectAction.Read || objectAction == ObjectAction.Ignore)
                {
                    continue;
                }

                String attributePath = String.Empty;
                String attributeName = String.Empty;
                String attributeParentName = String.Empty;

                EntityTypeAttributeMapping entityTypeAttributeMapping = new EntityTypeAttributeMapping();
                entityTypeAttributeMapping.ReferenceId = counter.ToString();
                entityTypeAttributeMapping.Action = objectAction;

                entityTypeAttributeMapping.EntityTypeName = dataRow[DataModelDictionary.EntityTypeAttributeMappingDictionary[DataModelEntityTypeAttributeMapping.EntityTypeName]].ToString().Trim();
                entityTypeAttributeMapping.Extension = dataRow[DataModelDictionary.EntityTypeAttributeMappingDictionary[DataModelEntityTypeAttributeMapping.Extension]].ToString().Trim();
                entityTypeAttributeMapping.ReadOnly = ValueTypeHelper.BooleanTryParse(dataRow[DataModelDictionary.EntityTypeAttributeMappingDictionary[DataModelEntityTypeAttributeMapping.ReadOnly]].ToString(), false);
                entityTypeAttributeMapping.Required = ValueTypeHelper.BooleanTryParse(dataRow[DataModelDictionary.EntityTypeAttributeMappingDictionary[DataModelEntityTypeAttributeMapping.Required]].ToString(), false);
                entityTypeAttributeMapping.ShowAtCreation = ValueTypeHelper.BooleanTryParse(dataRow[DataModelDictionary.EntityTypeAttributeMappingDictionary[DataModelEntityTypeAttributeMapping.ShowAtCreation]].ToString(), false);
                entityTypeAttributeMapping.SortOrder = ValueTypeHelper.Int32TryParse(dataRow[DataModelDictionary.EntityTypeAttributeMappingDictionary[DataModelEntityTypeAttributeMapping.SortOrder]].ToString(), 0);

                attributePath = dataRow[DataModelDictionary.EntityTypeAttributeMappingDictionary[DataModelEntityTypeAttributeMapping.AttributePath]].ToString().Trim();
                PopulateAttributeNameAndParentNameBasedOnPath(attributePath, ref attributeName, ref attributeParentName);

                entityTypeAttributeMapping.AttributeName = attributeName;
                entityTypeAttributeMapping.AttributeParentName = attributeParentName;
                entityTypeAttributeMapping.ExternalId = String.Format("[{1}]{0}[{2}]{0}[{3}]", _separator, entityTypeAttributeMapping.EntityTypeName, entityTypeAttributeMapping.AttributeParentName, entityTypeAttributeMapping.AttributeName);
                entityTypeAttributeMappings.Add(entityTypeAttributeMapping);
                counter++;
            }

            _levelBasedDictionary.Add(LEVEL_ONE, entityTypeAttributeMappings);
        }

        // S13 - Container - Entity Type - Attribute
        /// <summary>
        /// Prepares Container Entity Type Attribute Mapping collection from excel rows 
        /// </summary>
        /// <param name="dataTable">Represents a single row read from excel sheet</param>
        private void LoadContainerEntityTypeAttributeMappings(DataTable dataTable)
        {
            _levelBasedDictionary = new Dictionary<Int16, IDataModelObjectCollection>();

            Int32 counter = 2;
            ContainerEntityTypeAttributeMappingCollection containerEntityTypeAttributeMappings = new ContainerEntityTypeAttributeMappingCollection();

            foreach (DataRow dataRow in dataTable.Rows)
            {
                ObjectAction objectAction = GetActionFromDataRow(dataRow);

                if (objectAction == ObjectAction.Read || objectAction == ObjectAction.Ignore)
                {
                    continue;
                }

                String attributePath = String.Empty;
                String attributeName = String.Empty;
                String attributeParentName = String.Empty;
                ContainerEntityTypeAttributeMapping containerEntityTypeAttributeMapping = new ContainerEntityTypeAttributeMapping();

                containerEntityTypeAttributeMapping.ReferenceId = counter.ToString();
                containerEntityTypeAttributeMapping.Action = objectAction;

                containerEntityTypeAttributeMapping.OrganizationName = dataRow[DataModelDictionary.ContainerEntityTypeAttributeMappingDictionary[DataModelContainerEntityTypeAttributeMapping.OrganizationName]].ToString().Trim();
                containerEntityTypeAttributeMapping.ContainerName = dataRow[DataModelDictionary.ContainerEntityTypeAttributeMappingDictionary[DataModelContainerEntityTypeAttributeMapping.ContainerName]].ToString().Trim();
                containerEntityTypeAttributeMapping.EntityTypeName = dataRow[DataModelDictionary.ContainerEntityTypeAttributeMappingDictionary[DataModelContainerEntityTypeAttributeMapping.EntityTypeName]].ToString().Trim();
                containerEntityTypeAttributeMapping.Extension = dataRow[DataModelDictionary.ContainerEntityTypeAttributeMappingDictionary[DataModelContainerEntityTypeAttributeMapping.Extension]].ToString().Trim();
                containerEntityTypeAttributeMapping.ReadOnly = ValueTypeHelper.BooleanTryParse(dataRow[DataModelDictionary.ContainerEntityTypeAttributeMappingDictionary[DataModelContainerEntityTypeAttributeMapping.ReadOnly]].ToString(), false);
                containerEntityTypeAttributeMapping.Required = ValueTypeHelper.BooleanTryParse(dataRow[DataModelDictionary.ContainerEntityTypeAttributeMappingDictionary[DataModelContainerEntityTypeAttributeMapping.Required]].ToString(), false);
                containerEntityTypeAttributeMapping.ShowAtCreation = ValueTypeHelper.BooleanTryParse(dataRow[DataModelDictionary.ContainerEntityTypeAttributeMappingDictionary[DataModelContainerEntityTypeAttributeMapping.ShowAtCreation]].ToString(), false);
                containerEntityTypeAttributeMapping.InheritableOnly = ValueTypeHelper.BooleanTryParse(dataRow[DataModelDictionary.ContainerEntityTypeAttributeMappingDictionary[DataModelContainerEntityTypeAttributeMapping.InheritableOnly]].ToString(), false);
                containerEntityTypeAttributeMapping.AutoPromotable = ValueTypeHelper.BooleanTryParse(dataRow[DataModelDictionary.ContainerEntityTypeAttributeMappingDictionary[DataModelContainerEntityTypeAttributeMapping.AutoPromotable]].ToString(), false);
                containerEntityTypeAttributeMapping.SortOrder = ValueTypeHelper.Int32TryParse(dataRow[DataModelDictionary.ContainerEntityTypeAttributeMappingDictionary[DataModelContainerEntityTypeAttributeMapping.SortOrder]].ToString(), 0);

                attributePath = dataRow[DataModelDictionary.EntityTypeAttributeMappingDictionary[DataModelEntityTypeAttributeMapping.AttributePath]].ToString().Trim();
                PopulateAttributeNameAndParentNameBasedOnPath(attributePath, ref attributeName, ref attributeParentName);

                containerEntityTypeAttributeMapping.AttributeName = attributeName;
                containerEntityTypeAttributeMapping.AttributeParentName = attributeParentName;
                containerEntityTypeAttributeMapping.ExternalId = String.Format("[{1}]{0}[{2}]{0}[{3}]{0}[{4}]{0}[{5}]", _separator, containerEntityTypeAttributeMapping.OrganizationName, containerEntityTypeAttributeMapping.ContainerName, containerEntityTypeAttributeMapping.EntityTypeName, containerEntityTypeAttributeMapping.AttributeParentName, containerEntityTypeAttributeMapping.AttributeName);
                containerEntityTypeAttributeMappings.Add(containerEntityTypeAttributeMapping);
                counter++;
            }

            _levelBasedDictionary.Add(LEVEL_ONE, containerEntityTypeAttributeMappings);
        }

        // S14 - Category - Attribute
        /// <summary>
        /// Prepares Category Attribute Mapping collection from excel rows 
        /// </summary>
        /// <param name="dataTable">Represents a single row read from excel sheet</param>
        private void LoadCategoryAttributeMappings(DataTable dataTable)
        {
            _levelBasedDictionary = new Dictionary<Int16, IDataModelObjectCollection>();
            CategoryAttributeMappingCollection categoryAttributeMappings = new CategoryAttributeMappingCollection();

            Int32 counter = 2;

            foreach (DataRow dataRow in dataTable.Rows)
            {
                ObjectAction objectAction = GetActionFromDataRow(dataRow);

                if (objectAction == ObjectAction.Read || objectAction == ObjectAction.Ignore)
                    continue;

                String attributepath = String.Empty;
                String attributeName = String.Empty;
                String attributeParentName = String.Empty;
                CategoryAttributeMapping categoryAttributeMapping = new CategoryAttributeMapping();

                categoryAttributeMapping.ReferenceId = counter.ToString();
                categoryAttributeMapping.Action = objectAction;
                categoryAttributeMapping.SourceFlag = "O";
                categoryAttributeMapping.HierarchyName = dataRow[DataModelDictionary.CategoryAttributeMappingDictionary[DataModelCategoryAttributeMapping.HierarchyName]].ToString().Trim();
                categoryAttributeMapping.CategoryName = dataRow[DataModelDictionary.CategoryAttributeMappingDictionary[DataModelCategoryAttributeMapping.CategoryName]].ToString().Trim();
                categoryAttributeMapping.Path = categoryAttributeMapping.CategoryName;
                String parentCategoryPath = dataRow[DataModelDictionary.CategoryAttributeMappingDictionary[DataModelCategoryAttributeMapping.ParentCategoryPath]].ToString().Trim();

                if (!String.IsNullOrWhiteSpace(parentCategoryPath))
                {
                    categoryAttributeMapping.Path = String.Concat(parentCategoryPath.Replace(_separator, _categoryPathSeparator), _categoryPathSeparator, categoryAttributeMapping.CategoryName);
                }

                attributepath = dataRow[DataModelDictionary.EntityTypeAttributeMappingDictionary[DataModelEntityTypeAttributeMapping.AttributePath]].ToString().Trim();
                PopulateAttributeNameAndParentNameBasedOnPath(attributepath, ref attributeName, ref attributeParentName);

                categoryAttributeMapping.AttributeParentName = attributeParentName;
                categoryAttributeMapping.AttributeName = attributeName;
                categoryAttributeMapping.Required = ValueTypeHelper.BooleanTryParse(dataRow[DataModelDictionary.CategoryAttributeMappingDictionary[DataModelCategoryAttributeMapping.IsRequired]].ToString(), false);
                categoryAttributeMapping.ReadOnly = ValueTypeHelper.BooleanTryParse(dataRow[DataModelDictionary.CategoryAttributeMappingDictionary[DataModelCategoryAttributeMapping.IsReadOnly]].ToString(), false);
                categoryAttributeMapping.DefaultValue = dataRow[DataModelDictionary.CategoryAttributeMappingDictionary[DataModelCategoryAttributeMapping.DefaultValue]].ToString().Trim();
                categoryAttributeMapping.MinLength = ValueTypeHelper.Int32TryParse(dataRow[DataModelDictionary.CategoryAttributeMappingDictionary[DataModelCategoryAttributeMapping.MinimumLength]].ToString(), 0);
                categoryAttributeMapping.MaxLength = ValueTypeHelper.Int32TryParse(dataRow[DataModelDictionary.CategoryAttributeMappingDictionary[DataModelCategoryAttributeMapping.MaximumLength]].ToString(), 0);
                categoryAttributeMapping.Precision = ValueTypeHelper.Int32TryParse(dataRow[DataModelDictionary.CategoryAttributeMappingDictionary[DataModelCategoryAttributeMapping.Precision]].ToString(), 0);
                categoryAttributeMapping.InheritableOnly = ValueTypeHelper.BooleanTryParse(dataRow[DataModelDictionary.CategoryAttributeMappingDictionary[DataModelCategoryAttributeMapping.InheritableOnly]].ToString(), false);
                categoryAttributeMapping.AutoPromotable = ValueTypeHelper.BooleanTryParse(dataRow[DataModelDictionary.CategoryAttributeMappingDictionary[DataModelCategoryAttributeMapping.AutoPromotable]].ToString(), false);

                String rangeFrom = dataRow[DataModelDictionary.AttributeModelDictionary[DataModelAttributeModel.RangeFrom]].ToString().Trim(),
                        rangeTo = dataRow[DataModelDictionary.AttributeModelDictionary[DataModelAttributeModel.RangeTo]].ToString().Trim(),
                        isRangeFromInclusive = dataRow[DataModelDictionary.AttributeModelDictionary[DataModelAttributeModel.IsRangeFromInclusive]].ToString().Trim(),
                        isRangeToInclusive = dataRow[DataModelDictionary.AttributeModelDictionary[DataModelAttributeModel.IsRangeToInclusive]].ToString().Trim();

                categoryAttributeMapping.MinInclusive = GetRange(rangeFrom, isRangeFromInclusive, isInclusiveVariable: true);
                categoryAttributeMapping.MinExclusive = GetRange(rangeFrom, isRangeFromInclusive, isInclusiveVariable: false);
                categoryAttributeMapping.MaxInclusive = GetRange(rangeTo, isRangeToInclusive, isInclusiveVariable: true);
                categoryAttributeMapping.MaxExclusive = GetRange(rangeTo, isRangeToInclusive, isInclusiveVariable: false);

                categoryAttributeMapping.AllowableUOM = dataRow[DataModelDictionary.CategoryAttributeMappingDictionary[DataModelCategoryAttributeMapping.AllowedUOMs]].ToString().Trim();
                categoryAttributeMapping.DefaultUOM = dataRow[DataModelDictionary.CategoryAttributeMappingDictionary[DataModelCategoryAttributeMapping.DefaultUOM]].ToString().Trim();

                if (dataRow.Table.Columns.Contains(DataModelDictionary.CategoryAttributeMappingDictionary[DataModelCategoryAttributeMapping.AllowableValues]))
                {
                    categoryAttributeMapping.AllowableValues = dataRow[DataModelDictionary.CategoryAttributeMappingDictionary[DataModelCategoryAttributeMapping.AllowableValues]].ToString().Trim();
                }

                categoryAttributeMapping.SortOrder = ValueTypeHelper.Int32TryParse(dataRow[DataModelDictionary.CategoryAttributeMappingDictionary[DataModelCategoryAttributeMapping.SortOrder]].ToString(), 0);
                categoryAttributeMapping.Definition = dataRow[DataModelDictionary.CategoryAttributeMappingDictionary[DataModelCategoryAttributeMapping.Definition]].ToString().Trim();
                categoryAttributeMapping.AttributeExample = dataRow[DataModelDictionary.CategoryAttributeMappingDictionary[DataModelCategoryAttributeMapping.Example]].ToString().Trim();
                categoryAttributeMapping.BusinessRule = dataRow[DataModelDictionary.CategoryAttributeMappingDictionary[DataModelCategoryAttributeMapping.BusinessRule]].ToString().Trim();

                categoryAttributeMapping.ExternalId = String.Format("[{1}]{0}[{2}]{0}[{3}]{0}[{4}]", _separator, categoryAttributeMapping.HierarchyName, categoryAttributeMapping.CategoryName, categoryAttributeMapping.AttributeParentName, categoryAttributeMapping.AttributeName);
                categoryAttributeMappings.Add(categoryAttributeMapping);
                counter++;
            }

            _levelBasedDictionary.Add(LEVEL_ONE, categoryAttributeMappings);
        }

        // S15 - Relationship Type - Entity Type
        /// <summary>
        /// Prepares Relationship Type Entity Type Mapping collection from excel rows 
        /// </summary>
        /// <param name="dataTable">Represents a single row read from excel sheet</param>
        private void LoadRelationshipTypeEntityTypeMappings(DataTable dataTable)
        {
            _levelBasedDictionary = new Dictionary<Int16, IDataModelObjectCollection>();

            Int32 counter = 2;
            RelationshipTypeEntityTypeMappingCollection relationshipTypeEntityTypeMappings = new RelationshipTypeEntityTypeMappingCollection();

            foreach (DataRow dataRow in dataTable.Rows)
            {
                ObjectAction objectAction = GetActionFromDataRow(dataRow);

                if (objectAction == ObjectAction.Read || objectAction == ObjectAction.Ignore)
                {
                    continue;
                }

                RelationshipTypeEntityTypeMapping relationshipTypeEntityTypeMapping = new RelationshipTypeEntityTypeMapping();

                relationshipTypeEntityTypeMapping.ReferenceId = counter.ToString();
                relationshipTypeEntityTypeMapping.Action = objectAction;
                relationshipTypeEntityTypeMapping.EntityTypeName = dataRow[DataModelDictionary.RelationshipTypeEntityTypeMappingDictionary[DataModelRelationshipTypeEntityTypeMapping.EntityTypeName]].ToString().Trim();
                relationshipTypeEntityTypeMapping.RelationshipTypeName = dataRow[DataModelDictionary.RelationshipTypeEntityTypeMappingDictionary[DataModelRelationshipTypeEntityTypeMapping.RelationshipTypeName]].ToString().Trim();
                relationshipTypeEntityTypeMapping.DrillDown = ValueTypeHelper.BooleanTryParse(dataRow[DataModelDictionary.RelationshipTypeEntityTypeMappingDictionary[DataModelRelationshipTypeEntityTypeMapping.DrillDown]].ToString(), false);
                relationshipTypeEntityTypeMapping.Excludable = ValueTypeHelper.BooleanTryParse(dataRow[DataModelDictionary.RelationshipTypeEntityTypeMappingDictionary[DataModelRelationshipTypeEntityTypeMapping.Excludable]].ToString(), false);
                relationshipTypeEntityTypeMapping.IsDefaultRelation = ValueTypeHelper.BooleanTryParse(dataRow[DataModelDictionary.RelationshipTypeEntityTypeMappingDictionary[DataModelRelationshipTypeEntityTypeMapping.IsDefaultRelation]].ToString(), false);
                relationshipTypeEntityTypeMapping.ReadOnly = ValueTypeHelper.BooleanTryParse(dataRow[DataModelDictionary.RelationshipTypeEntityTypeMappingDictionary[DataModelRelationshipTypeEntityTypeMapping.ReadOnly]].ToString(), false);
                relationshipTypeEntityTypeMapping.ExternalId = String.Format("[{1}]{0}[{2}]", _separator, relationshipTypeEntityTypeMapping.RelationshipTypeName, relationshipTypeEntityTypeMapping.EntityTypeName);

                relationshipTypeEntityTypeMappings.Add(relationshipTypeEntityTypeMapping);
                counter++;
            }

            _levelBasedDictionary.Add(LEVEL_ONE, relationshipTypeEntityTypeMappings);
        }

        // S16 - Relationship Type - Entity Type - Cardinality
        /// <summary>
        /// Prepares Relationship Type Entity Type Cardinality collection from excel rows 
        /// </summary>
        /// <param name="dataTable">Represents a single row read from excel sheet</param>
        private void LoadRelationshipTypeEntityTypeCardinalities(DataTable dataTable)
        {
            _levelBasedDictionary = new Dictionary<Int16, IDataModelObjectCollection>();

            Int32 counter = 2;
            RelationshipTypeEntityTypeMappingCardinalityCollection relationshipTypeEntityTypeMappingCardinalities = new RelationshipTypeEntityTypeMappingCardinalityCollection();

            foreach (DataRow dataRow in dataTable.Rows)
            {
                ObjectAction objectAction = GetActionFromDataRow(dataRow);

                if (objectAction == ObjectAction.Read || objectAction == ObjectAction.Ignore)
                {
                    continue;
                }

                RelationshipTypeEntityTypeMappingCardinality relationshipTypeEntityTypeMappingCardinality = new RelationshipTypeEntityTypeMappingCardinality();

                relationshipTypeEntityTypeMappingCardinality.ReferenceId = counter.ToString();
                relationshipTypeEntityTypeMappingCardinality.Action = objectAction;
                relationshipTypeEntityTypeMappingCardinality.EntityTypeName = dataRow[DataModelDictionary.RelationshipTypeEntityTypeMappingCardinalityDictionary[DataModelRelationshipTypeEntityTypeMappingCardinality.EntityTypeName]].ToString().Trim();
                relationshipTypeEntityTypeMappingCardinality.RelationshipTypeName = dataRow[DataModelDictionary.RelationshipTypeEntityTypeMappingCardinalityDictionary[DataModelRelationshipTypeEntityTypeMappingCardinality.RelationshipTypeName]].ToString().Trim();
                relationshipTypeEntityTypeMappingCardinality.ToEntityTypeName = dataRow[DataModelDictionary.RelationshipTypeEntityTypeMappingCardinalityDictionary[DataModelRelationshipTypeEntityTypeMappingCardinality.ToEntityTypeName]].ToString().Trim();
                relationshipTypeEntityTypeMappingCardinality.MinRelationships = ValueTypeHelper.Int32TryParse(dataRow[DataModelDictionary.RelationshipTypeEntityTypeMappingCardinalityDictionary[DataModelRelationshipTypeEntityTypeMappingCardinality.MinRequired]].ToString(), 0);
                relationshipTypeEntityTypeMappingCardinality.MaxRelationships = ValueTypeHelper.Int32TryParse(dataRow[DataModelDictionary.RelationshipTypeEntityTypeMappingCardinalityDictionary[DataModelRelationshipTypeEntityTypeMappingCardinality.MaxAllowed]].ToString(), 0);
                relationshipTypeEntityTypeMappingCardinality.ExternalId = String.Format("[{1}]{0}[{2}]{0}[{3}]", _separator, relationshipTypeEntityTypeMappingCardinality.RelationshipTypeName, relationshipTypeEntityTypeMappingCardinality.EntityTypeName, relationshipTypeEntityTypeMappingCardinality.ToEntityTypeName);

                relationshipTypeEntityTypeMappingCardinalities.Add(relationshipTypeEntityTypeMappingCardinality);
                counter++;
            }

            _levelBasedDictionary.Add(LEVEL_ONE, relationshipTypeEntityTypeMappingCardinalities);
        }

        // S17 - Container - RelationshipType  - Entity Type 
        /// <summary>
        /// Prepares Container Relationship Type Entity Type Mapping collection from excel rows 
        /// </summary>
        /// <param name="dataTable">Represents a single row read from excel sheet</param>
        private void LoadContainerRelationshipTypeEntityTypeMappings(DataTable dataTable)
        {
            _levelBasedDictionary = new Dictionary<Int16, IDataModelObjectCollection>();

            Int32 counter = 2;
            ContainerRelationshipTypeEntityTypeMappingCollection containerRelationshipTypeEntityTypeMappings = new ContainerRelationshipTypeEntityTypeMappingCollection();

            foreach (DataRow dataRow in dataTable.Rows)
            {
                ObjectAction objectAction = GetActionFromDataRow(dataRow);

                if (objectAction == ObjectAction.Read || objectAction == ObjectAction.Ignore)
                {
                    continue;
                }

                ContainerRelationshipTypeEntityTypeMapping containerRelationshipTypeEntityTypeMapping = new ContainerRelationshipTypeEntityTypeMapping();

                containerRelationshipTypeEntityTypeMapping.ReferenceId = counter.ToString();
                containerRelationshipTypeEntityTypeMapping.Action = objectAction;

                containerRelationshipTypeEntityTypeMapping.OrganizationName = dataRow[DataModelDictionary.ContainerRelationshipTypeEntityTypeMappingDictionary[DataModelContainerRelationshipTypeEntityTypeMapping.OrganizationName]].ToString().Trim();
                containerRelationshipTypeEntityTypeMapping.ContainerName = dataRow[DataModelDictionary.ContainerRelationshipTypeEntityTypeMappingDictionary[DataModelContainerRelationshipTypeEntityTypeMapping.ContainerName]].ToString().Trim();
                containerRelationshipTypeEntityTypeMapping.EntityTypeName = dataRow[DataModelDictionary.ContainerRelationshipTypeEntityTypeMappingDictionary[DataModelContainerRelationshipTypeEntityTypeMapping.EntityTypeName]].ToString().Trim();
                containerRelationshipTypeEntityTypeMapping.RelationshipTypeName = dataRow[DataModelDictionary.ContainerRelationshipTypeEntityTypeMappingDictionary[DataModelContainerRelationshipTypeEntityTypeMapping.RelationshipTypeName]].ToString().Trim();
                containerRelationshipTypeEntityTypeMapping.DrillDown = ValueTypeHelper.BooleanTryParse(dataRow[DataModelDictionary.ContainerRelationshipTypeEntityTypeMappingDictionary[DataModelContainerRelationshipTypeEntityTypeMapping.DrillDown]].ToString(), false);
                containerRelationshipTypeEntityTypeMapping.Excludable = ValueTypeHelper.BooleanTryParse(dataRow[DataModelDictionary.ContainerRelationshipTypeEntityTypeMappingDictionary[DataModelContainerRelationshipTypeEntityTypeMapping.Excludable]].ToString(), false);
                containerRelationshipTypeEntityTypeMapping.IsDefaultRelation = ValueTypeHelper.BooleanTryParse(dataRow[DataModelDictionary.ContainerRelationshipTypeEntityTypeMappingDictionary[DataModelContainerRelationshipTypeEntityTypeMapping.IsDefaultRelation]].ToString(), false);
                containerRelationshipTypeEntityTypeMapping.ReadOnly = ValueTypeHelper.BooleanTryParse(dataRow[DataModelDictionary.ContainerRelationshipTypeEntityTypeMappingDictionary[DataModelContainerRelationshipTypeEntityTypeMapping.ReadOnly]].ToString(), false);

                containerRelationshipTypeEntityTypeMapping.ExternalId = String.Format("[{1}]{0}[{2}]{0}[{3}]{0}[{4}]", _separator, containerRelationshipTypeEntityTypeMapping.OrganizationName, containerRelationshipTypeEntityTypeMapping.ContainerName, containerRelationshipTypeEntityTypeMapping.RelationshipTypeName, containerRelationshipTypeEntityTypeMapping.EntityTypeName);
                containerRelationshipTypeEntityTypeMappings.Add(containerRelationshipTypeEntityTypeMapping);
                counter++;
            }

            _levelBasedDictionary.Add(LEVEL_ONE, containerRelationshipTypeEntityTypeMappings);
        }

        // S18 - Container - Relationship Type - Entity Type - Cardinality
        /// <summary>
        /// Prepares Container Relationship Type Entity Type Cardinality collection from excel rows 
        /// </summary>
        /// <param name="dataTable">Represents a single row read from excel sheet</param>
        private void LoadContainerRelationshipTypeEntityTypeCardinalities(DataTable dataTable)
        {
            _levelBasedDictionary = new Dictionary<Int16, IDataModelObjectCollection>();

            Int32 counter = 2;
            ContainerRelationshipTypeEntityTypeMappingCardinalityCollection containerRelationshipTypeEntityTypeMappingCardinalities = new ContainerRelationshipTypeEntityTypeMappingCardinalityCollection();

            foreach (DataRow dataRow in dataTable.Rows)
            {
                ObjectAction objectAction = GetActionFromDataRow(dataRow);

                if (objectAction == ObjectAction.Read || objectAction == ObjectAction.Ignore)
                {
                    continue;
                }

                ContainerRelationshipTypeEntityTypeMappingCardinality containerRelationshipTypeEntityTypeMappingCardinality = new ContainerRelationshipTypeEntityTypeMappingCardinality();

                containerRelationshipTypeEntityTypeMappingCardinality.ReferenceId = counter.ToString();
                containerRelationshipTypeEntityTypeMappingCardinality.Action = objectAction;
                containerRelationshipTypeEntityTypeMappingCardinality.OrganizationName = dataRow[DataModelDictionary.ContainerRelationshipTypeEntityTypeMappingCardinalityDictionary[DataModelContainerRelationshipTypeEntityTypeMappingCardinality.OrganizationName]].ToString().Trim();
                containerRelationshipTypeEntityTypeMappingCardinality.ContainerName = dataRow[DataModelDictionary.ContainerRelationshipTypeEntityTypeMappingCardinalityDictionary[DataModelContainerRelationshipTypeEntityTypeMappingCardinality.ContainerName]].ToString().Trim();
                containerRelationshipTypeEntityTypeMappingCardinality.EntityTypeName = dataRow[DataModelDictionary.ContainerRelationshipTypeEntityTypeMappingCardinalityDictionary[DataModelContainerRelationshipTypeEntityTypeMappingCardinality.EntityTypeName]].ToString().Trim();
                containerRelationshipTypeEntityTypeMappingCardinality.RelationshipTypeName = dataRow[DataModelDictionary.ContainerRelationshipTypeEntityTypeMappingCardinalityDictionary[DataModelContainerRelationshipTypeEntityTypeMappingCardinality.RelationshipTypeName]].ToString().Trim();
                containerRelationshipTypeEntityTypeMappingCardinality.ToEntityTypeName = dataRow[DataModelDictionary.ContainerRelationshipTypeEntityTypeMappingCardinalityDictionary[DataModelContainerRelationshipTypeEntityTypeMappingCardinality.ToEntityTypeName]].ToString().Trim();
                containerRelationshipTypeEntityTypeMappingCardinality.MinRelationships = ValueTypeHelper.Int32TryParse(dataRow[DataModelDictionary.ContainerRelationshipTypeEntityTypeMappingCardinalityDictionary[DataModelContainerRelationshipTypeEntityTypeMappingCardinality.MinRequired]].ToString(), 0);
                containerRelationshipTypeEntityTypeMappingCardinality.MaxRelationships = ValueTypeHelper.Int32TryParse(dataRow[DataModelDictionary.ContainerRelationshipTypeEntityTypeMappingCardinalityDictionary[DataModelContainerRelationshipTypeEntityTypeMappingCardinality.MaxAllowed]].ToString(), 0);
                containerRelationshipTypeEntityTypeMappingCardinality.ExternalId = String.Format("[{1}]{0}[{2}]{0}[{3}]{0}[{4}]{0}[{5}]", _separator, containerRelationshipTypeEntityTypeMappingCardinality.OrganizationName, containerRelationshipTypeEntityTypeMappingCardinality.ContainerName, containerRelationshipTypeEntityTypeMappingCardinality.RelationshipTypeName, containerRelationshipTypeEntityTypeMappingCardinality.EntityTypeName, containerRelationshipTypeEntityTypeMappingCardinality.ToEntityTypeName);

                containerRelationshipTypeEntityTypeMappingCardinalities.Add(containerRelationshipTypeEntityTypeMappingCardinality);
                counter++;
            }

            _levelBasedDictionary.Add(LEVEL_ONE, containerRelationshipTypeEntityTypeMappingCardinalities);
        }

        // S19 - Relationship Type - Attribute
        /// <summary>
        /// Prepares Relationship Type Attribute Mapping collection from excel rows 
        /// </summary>
        /// <param name="dataTable">Represents a single row read from excel sheet</param>
        private void LoadRelationshipTypeAttributeMappings(DataTable dataTable)
        {
            _levelBasedDictionary = new Dictionary<Int16, IDataModelObjectCollection>();

            RelationshipTypeAttributeMappingCollection relationshipTypeAttributeMappings = new RelationshipTypeAttributeMappingCollection();

            Int32 counter = 2;

            foreach (DataRow dataRow in dataTable.Rows)
            {
                ObjectAction objectAction = GetActionFromDataRow(dataRow);

                if (objectAction == ObjectAction.Read || objectAction == ObjectAction.Ignore)
                    continue;

                String attributePath = String.Empty;
                String attributeName = String.Empty;
                String attributeParentName = String.Empty;

                RelationshipTypeAttributeMapping relationshipTypeAttributeMapping = new RelationshipTypeAttributeMapping();
                relationshipTypeAttributeMapping.ReferenceId = counter.ToString();
                relationshipTypeAttributeMapping.Action = objectAction;

                relationshipTypeAttributeMapping.RelationshipTypeName = dataRow[DataModelDictionary.RelationshipTypeAttributeMappingDictionary[DataModelRelationshipTypeAttributeMapping.RelationshipTypeName]].ToString().Trim();
                relationshipTypeAttributeMapping.ReadOnly = ValueTypeHelper.BooleanTryParse(dataRow[DataModelDictionary.RelationshipTypeAttributeMappingDictionary[DataModelRelationshipTypeAttributeMapping.ReadOnly]].ToString(), false);
                relationshipTypeAttributeMapping.Required = ValueTypeHelper.BooleanTryParse(dataRow[DataModelDictionary.RelationshipTypeAttributeMappingDictionary[DataModelRelationshipTypeAttributeMapping.Required]].ToString(), false);
                relationshipTypeAttributeMapping.ShowInline = ValueTypeHelper.BooleanTryParse(dataRow[DataModelDictionary.RelationshipTypeAttributeMappingDictionary[DataModelRelationshipTypeAttributeMapping.ShowInline]].ToString(), false);
                relationshipTypeAttributeMapping.SortOrder = ValueTypeHelper.Int32TryParse(dataRow[DataModelDictionary.RelationshipTypeAttributeMappingDictionary[DataModelRelationshipTypeAttributeMapping.SortOrder]].ToString(), 0);

                attributePath = dataRow[DataModelDictionary.RelationshipTypeAttributeMappingDictionary[DataModelRelationshipTypeAttributeMapping.AttributePath]].ToString().Trim();
                PopulateAttributeNameAndParentNameBasedOnPath(attributePath, ref attributeName, ref attributeParentName);

                relationshipTypeAttributeMapping.AttributeParentName = attributeParentName;
                relationshipTypeAttributeMapping.AttributeName = attributeName;
                relationshipTypeAttributeMapping.ExternalId = String.Format("[{1}]{0}[{2}]{0}[{3}]", _separator, relationshipTypeAttributeMapping.RelationshipTypeName, relationshipTypeAttributeMapping.AttributeParentName, relationshipTypeAttributeMapping.AttributeName);

                relationshipTypeAttributeMappings.Add(relationshipTypeAttributeMapping);
                counter++;
            }

            _levelBasedDictionary.Add(LEVEL_ONE, relationshipTypeAttributeMappings);
        }

        // S20 - Container - Relationship Type - Attribute
        /// <summary>
        /// Prepares Container Relationship Type Attribute Mapping collection from excel rows 
        /// </summary>
        /// <param name="dataTable">Represents a single row read from excel sheet</param>
        private void LoadContainerRelationshipTypeAttributeMappings(DataTable dataTable)
        {
            _levelBasedDictionary = new Dictionary<Int16, IDataModelObjectCollection>();

            ContainerRelationshipTypeAttributeMappingCollection containerRelationshipTypeAttributeMappings = new ContainerRelationshipTypeAttributeMappingCollection();

            Int32 counter = 2;

            foreach (DataRow dataRow in dataTable.Rows)
            {
                ObjectAction objectAction = GetActionFromDataRow(dataRow);

                if (objectAction == ObjectAction.Read || objectAction == ObjectAction.Ignore)
                    continue;

                String attributePath = String.Empty;
                String attributeName = String.Empty;
                String attributeParentName = String.Empty;
                ContainerRelationshipTypeAttributeMapping containerRelationshipTypeAttributeMapping = new ContainerRelationshipTypeAttributeMapping();

                containerRelationshipTypeAttributeMapping.ReferenceId = counter.ToString();
                containerRelationshipTypeAttributeMapping.Action = objectAction;
                containerRelationshipTypeAttributeMapping.OrganizationName = dataRow[DataModelDictionary.ContainerRelationshipTypeAttributeMappingDictionary[DataModelContainerRelationshipTypeAttributeMapping.OrganizationName]].ToString().Trim();
                containerRelationshipTypeAttributeMapping.ContainerName = dataRow[DataModelDictionary.ContainerRelationshipTypeAttributeMappingDictionary[DataModelContainerRelationshipTypeAttributeMapping.ContainerName]].ToString().Trim();
                containerRelationshipTypeAttributeMapping.RelationshipTypeName = dataRow[DataModelDictionary.ContainerRelationshipTypeAttributeMappingDictionary[DataModelContainerRelationshipTypeAttributeMapping.RelationshipTypeName]].ToString().Trim();
                containerRelationshipTypeAttributeMapping.ReadOnly = ValueTypeHelper.BooleanTryParse(dataRow[DataModelDictionary.ContainerRelationshipTypeAttributeMappingDictionary[DataModelContainerRelationshipTypeAttributeMapping.ReadOnly]].ToString(), false);
                containerRelationshipTypeAttributeMapping.Required = ValueTypeHelper.BooleanTryParse(dataRow[DataModelDictionary.ContainerRelationshipTypeAttributeMappingDictionary[DataModelContainerRelationshipTypeAttributeMapping.Required]].ToString(), false);
                containerRelationshipTypeAttributeMapping.ShowInline = ValueTypeHelper.BooleanTryParse(dataRow[DataModelDictionary.ContainerRelationshipTypeAttributeMappingDictionary[DataModelContainerRelationshipTypeAttributeMapping.ShowInline]].ToString(), false);
                containerRelationshipTypeAttributeMapping.SortOrder = ValueTypeHelper.Int32TryParse(dataRow[DataModelDictionary.ContainerRelationshipTypeAttributeMappingDictionary[DataModelContainerRelationshipTypeAttributeMapping.SortOrder]].ToString(), 0);
                containerRelationshipTypeAttributeMapping.AutoPromotable = ValueTypeHelper.BooleanTryParse(dataRow[DataModelDictionary.ContainerRelationshipTypeAttributeMappingDictionary[DataModelContainerRelationshipTypeAttributeMapping.AutoPromotable]].ToString(), false);

                attributePath = dataRow[DataModelDictionary.ContainerRelationshipTypeAttributeMappingDictionary[DataModelContainerRelationshipTypeAttributeMapping.AttributePath]].ToString().Trim();
                PopulateAttributeNameAndParentNameBasedOnPath(attributePath, ref attributeName, ref attributeParentName);

                containerRelationshipTypeAttributeMapping.AttributeParentName = attributeParentName;
                containerRelationshipTypeAttributeMapping.AttributeName = attributeName;
                containerRelationshipTypeAttributeMapping.ExternalId = String.Format("[{1}]{0}[{2}]{0}[{3}]{0}[{4}]{0}[{5}]", _separator, containerRelationshipTypeAttributeMapping.OrganizationName, containerRelationshipTypeAttributeMapping.ContainerName, containerRelationshipTypeAttributeMapping.RelationshipTypeName, containerRelationshipTypeAttributeMapping.AttributeParentName, containerRelationshipTypeAttributeMapping.AttributeName);

                containerRelationshipTypeAttributeMappings.Add(containerRelationshipTypeAttributeMapping);
                counter++;
            }

            _levelBasedDictionary.Add(LEVEL_ONE, containerRelationshipTypeAttributeMappings);
        }

        // S21 - Security Role
        /// <summary>
        /// Prepares security role collection from excel rows 
        /// </summary>
        /// <param name="dataTable">Represents a single row read from excel sheet</param>
        private void LoadSecurityRoles(DataTable dataTable)
        {
            _levelBasedDictionary = new Dictionary<Int16, IDataModelObjectCollection>();

            Int32 counter = 2;
            SecurityRoleCollection securityRoles = new SecurityRoleCollection();

            foreach (DataRow dataRow in dataTable.Rows)
            {
                ObjectAction objectAction = GetActionFromDataRow(dataRow);

                if (objectAction == ObjectAction.Read || objectAction == ObjectAction.Ignore)
                {
                    continue;
                }

                SecurityRole securityRole = new SecurityRole()
                {
                    ReferenceId = counter.ToString(),
                    Action = objectAction,
                    ExternalId = dataRow[DataModelDictionary.SecurityRoleDictionary[DataModelSecurityRole.LongName]].ToString().Trim(),
                    Name = dataRow[DataModelDictionary.SecurityRoleDictionary[DataModelSecurityRole.ShortName]].ToString().Trim(),
                    LongName = dataRow[DataModelDictionary.SecurityRoleDictionary[DataModelSecurityRole.LongName]].ToString().Trim(),
                    IsPrivateRole = ValueTypeHelper.BooleanTryParse(dataRow[DataModelDictionary.SecurityRoleDictionary[DataModelSecurityRole.IsPrivateRole]].ToString(), false),
                };

                securityRoles.Add(securityRole);
                counter++;
            }

            _levelBasedDictionary.Add(LEVEL_ONE, securityRoles);
        }

        // S22 - Security User with preferences
        /// <summary>
        /// Prepares User collection from excel rows 
        /// </summary>
        /// <param name="dataTable">Represents a single row read from excel sheet</param>
        private void LoadUsersWithPreferences(DataTable dataTable)
        {
            _levelBasedDictionary = new Dictionary<Int16, IDataModelObjectCollection>();

            Int32 counter = 2;
            SecurityUserCollection users = new SecurityUserCollection();

            foreach (DataRow dataRow in dataTable.Rows)
            {
                ObjectAction objectAction = GetActionFromDataRow(dataRow);

                if (objectAction == ObjectAction.Read || objectAction == ObjectAction.Ignore)
                {
                    continue;
                }

                AuthenticationType authenticationType = AuthenticationType.Unknown;
                ValueTypeHelper.EnumTryParse(dataRow[DataModelDictionary.SecurityUserDictionary[DataModelSecurityUser.AuthenticationType]].ToString().Trim(), true, out authenticationType);

                Collection<String> roles = new Collection<String>();

                roles = ValueTypeHelper.SplitStringToStringCollection(dataRow[DataModelDictionary.SecurityUserDictionary[DataModelSecurityUser.Roles]].ToString().Trim(), ",");
                SecurityRoleCollection securityRoles = new SecurityRoleCollection();

                if (roles != null && roles.Count > 0)
                {
                    foreach (String roleName in roles)
                    {
                        SecurityRole securityRole = new SecurityRole();
                        securityRole.Name = roleName.Trim();
                        securityRoles.Add(securityRole);
                    }
                }

                SecurityUser user = new SecurityUser()
                {
                    ReferenceId = counter.ToString(),
                    Action = objectAction,
                    ExternalId = dataRow[DataModelDictionary.SecurityUserDictionary[DataModelSecurityUser.Login]].ToString().Trim(),
                    SecurityUserLogin = dataRow[DataModelDictionary.SecurityUserDictionary[DataModelSecurityUser.Login]].ToString().Trim(),
                    Password = BasicHashingHelper.MD5HashString(dataRow[DataModelDictionary.SecurityUserDictionary[DataModelSecurityUser.Password]].ToString().Trim()),
                    AuthenticationType = authenticationType,
                    Roles = securityRoles,
                    ManagerLogin = dataRow[DataModelDictionary.SecurityUserDictionary[DataModelSecurityUser.ManagerName]].ToString().Trim(),
                    Initials = dataRow[DataModelDictionary.SecurityUserDictionary[DataModelSecurityUser.Initials]].ToString().Trim(),
                    FirstName = dataRow[DataModelDictionary.SecurityUserDictionary[DataModelSecurityUser.FirstName]].ToString().Trim(),
                    LastName = dataRow[DataModelDictionary.SecurityUserDictionary[DataModelSecurityUser.LastName]].ToString().Trim(),
                    Smtp = dataRow[DataModelDictionary.SecurityUserDictionary[DataModelSecurityUser.Email]].ToString()
                };

                LocaleEnum UILocale = LocaleEnum.UnKnown;
                ValueTypeHelper.EnumTryParse(dataRow[DataModelDictionary.SecurityUserDictionary[DataModelSecurityUser.DefaultUILocale]].ToString(), true, out UILocale);

                LocaleEnum dataLocale = LocaleEnum.UnKnown;
                ValueTypeHelper.EnumTryParse(dataRow[DataModelDictionary.SecurityUserDictionary[DataModelSecurityUser.DefaultDataLocale]].ToString(), true, out dataLocale);

                UserPreferences userPreference = new UserPreferences()
                {
                    ReferenceId = counter.ToString(),
                    Action = objectAction,
                    ExternalId = dataRow[DataModelDictionary.SecurityUserDictionary[DataModelSecurityUser.Login]].ToString(),
                    LoginName = dataRow[DataModelDictionary.SecurityUserDictionary[DataModelSecurityUser.Login]].ToString(),
                    UILocale = UILocale,
                    DataLocale = dataLocale,
                    DefaultOrgName = dataRow[DataModelDictionary.SecurityUserDictionary[DataModelSecurityUser.DefaultOrganization]].ToString(),
                    DefaultHierarchyName = dataRow[DataModelDictionary.SecurityUserDictionary[DataModelSecurityUser.DefaultHierarchy]].ToString(),
                    DefaultContainerName = dataRow[DataModelDictionary.SecurityUserDictionary[DataModelSecurityUser.DefaultContainer]].ToString(),
                    DefaultRoleName = dataRow[DataModelDictionary.SecurityUserDictionary[DataModelSecurityUser.DefaultRole]].ToString(),
                    DefaultTimeZoneShortName = dataRow[DataModelDictionary.SecurityUserDictionary[DataModelSecurityUser.DefaultTimeZone]].ToString(),
                    MaxTableRows = ValueTypeHelper.Int32TryParse(dataRow[DataModelDictionary.SecurityUserDictionary[DataModelSecurityUser.NoOfRecordsToShowPerPageInDisplayTable]].ToString(), 0),
                    MaxTablePages = ValueTypeHelper.Int32TryParse(dataRow[DataModelDictionary.SecurityUserDictionary[DataModelSecurityUser.NoOfPagesToShowForDisplayTable]].ToString(), 0)
                };

                user.UserPreference = userPreference;
                users.Add(user);
                counter++;
            }

            _levelBasedDictionary.Add(LEVEL_ONE, users);
        }

        // S23 - Lookup Model
        /// <summary>
        /// Prepares Lookup Model from excel rows 
        /// </summary>
        /// <param name="dataTable">Represents a single row read from excel sheet</param>
        private void LoadLookupModels(DataTable dataTable)
        {
            _levelBasedDictionary = new Dictionary<Int16, IDataModelObjectCollection>();

            DBTableCollection dbTables = new DBTableCollection();

            Int32 counter = 2;

            foreach (DataRow dataRow in dataTable.Rows)
            {
                ObjectAction objectAction = GetActionFromDataRow(dataRow);

                if (objectAction == ObjectAction.Read || objectAction == ObjectAction.Ignore)
                {
                    continue;
                }

                String tableName = dataRow[DataModelDictionary.LookupModelDictionary[DataModelLookupModel.TableName]].ToString().Trim();

                //Try to find DBTable based on table name.
                DBTable dbTable = dbTables.Get(tableName);

                if (dbTable == null)
                {
                    #region Read Table Properties

                    dbTable = new DBTable();
                    dbTable.ReferenceId = counter.ToString();
                    dbTable.Action = ObjectAction.Create;
                    dbTable.Name = tableName;
                    dbTable.ExternalId = dbTable.Name;
                    dbTable.DynamicTableType = DynamicTableType.Lookup;

                    dbTables.Add(dbTable);

                    #endregion

                    counter++;
                }

                #region Read Column Properties

                DBColumn dbColumn = new DBColumn();

                dbColumn.Action = objectAction;
                dbColumn.Sequence = ValueTypeHelper.Int32TryParse(dataRow[DataModelDictionary.LookupModelDictionary[DataModelLookupModel.Sequence]].ToString(), dbColumn.Sequence);
                dbColumn.Name = dataRow[DataModelDictionary.LookupModelDictionary[DataModelLookupModel.ColumnName]].ToString().Trim();
                dbColumn.DataType = dataRow[DataModelDictionary.LookupModelDictionary[DataModelLookupModel.DataType]].ToString().Trim();
                dbColumn.Length = ValueTypeHelper.Int32TryParse(dataRow[DataModelDictionary.LookupModelDictionary[DataModelLookupModel.Width]].ToString(), dbColumn.Length);
                dbColumn.Precision = ValueTypeHelper.Int32TryParse(dataRow[DataModelDictionary.LookupModelDictionary[DataModelLookupModel.Precision]].ToString(), dbColumn.Precision);
                dbColumn.Nullable = ValueTypeHelper.BooleanTryParse(dataRow[DataModelDictionary.LookupModelDictionary[DataModelLookupModel.Nullable]].ToString(), dbColumn.Nullable);
                dbColumn.IsUnique = ValueTypeHelper.BooleanTryParse(dataRow[DataModelDictionary.LookupModelDictionary[DataModelLookupModel.IsUnique]].ToString(), dbColumn.IsUnique);
                dbColumn.DefaultValue = dataRow[DataModelDictionary.LookupModelDictionary[DataModelLookupModel.DefaultValue]].ToString().Trim();

                dbTable.Columns.Add(dbColumn);

                #endregion

                #region Read Constraint Properties

                DBConstraint dbConstraint = null;
                String checkConstraintValue = dataRow[DataModelDictionary.LookupModelDictionary[DataModelLookupModel.CheckConstraint]].ToString();

                if (!String.IsNullOrWhiteSpace(dbColumn.DefaultValue))
                {
                    dbConstraint = new DBConstraint();
                    dbConstraint.ColumnName = dbColumn.Name;
                    dbConstraint.ConstraintType = ConstraintType.DefaultValue;
                    dbConstraint.Value = dbColumn.DefaultValue;
                    dbConstraint.Action = dbColumn.Action;

                    dbTable.Constraints.Add(dbConstraint);
                }

                if (!String.IsNullOrWhiteSpace(checkConstraintValue))
                {
                    dbConstraint = new DBConstraint();

                    dbConstraint.ColumnName = dbColumn.Name;
                    dbConstraint.ConstraintType = ConstraintType.Check;
                    dbConstraint.Value = checkConstraintValue;
                    dbConstraint.Action = dbColumn.Action;

                    dbTable.Constraints.Add(dbConstraint);
                }

                #endregion
            }

            _levelBasedDictionary.Add(LEVEL_ONE, dbTables);
        }

        // S24 - Word List
        /// <summary>
        /// Prepares Word List Model from excel rows 
        /// </summary>
        /// <param name="dataTable">Represents a single row read from excel sheet</param>
        private void LoadWordListModels(DataTable dataTable)
        {
            _levelBasedDictionary = new Dictionary<Int16, IDataModelObjectCollection>();

            Int32 counter = 2;
            ImportWordListDTOCollection wordListsCollection = new ImportWordListDTOCollection();

            foreach (DataRow dataRow in dataTable.Rows)
            {
                ObjectAction objectAction = GetActionFromDataRow(dataRow);

                if (objectAction == ObjectAction.Read || objectAction == ObjectAction.Ignore)
                {
                    continue;
                }

                ImportWordListDTO wordList = new ImportWordListDTO
                {
                    ReferenceId = counter.ToString(CultureInfo.InvariantCulture),
                    Action = objectAction,
                    Name = dataRow[DataModelDictionary.WordListModelDictionary[DataModelImportWordList.ImportWordListShortName]].ToString().Trim(),
                    LongName = dataRow[DataModelDictionary.WordListModelDictionary[DataModelImportWordList.ImportWordListLongName]].ToString().Trim(),
                    IsFlushAndFillMode = ValueTypeHelper.BooleanTryParse(dataRow[DataModelDictionary.WordListModelDictionary[DataModelImportWordList.ImportWordListIsFlushAndFillMode]].ToString(), false)
                };

                wordListsCollection.Add(wordList);
                counter++;
            }

            _levelBasedDictionary.Add(LEVEL_ONE, wordListsCollection);
        }

        // S25 - Word Element
        /// <summary>
        /// Prepares Word Element Model from excel rows 
        /// </summary>
        /// <param name="dataTable">Represents a single row read from excel sheet</param>
        private void LoadWordElementModels(DataTable dataTable)
        {
            _levelBasedDictionary = new Dictionary<Int16, IDataModelObjectCollection>();

            Int32 counter = 2;
            WordElementsCollection wordElementsCollection = new WordElementsCollection();

            foreach (DataRow dataRow in dataTable.Rows)
            {
                ObjectAction objectAction = GetActionFromDataRow(dataRow);

                if (objectAction == ObjectAction.Read || objectAction == ObjectAction.Ignore)
                {
                    continue;
                }

                WordElement wordElement = new WordElement
                {
                    ReferenceId = counter.ToString(CultureInfo.InvariantCulture),
                    Action = objectAction,
                    Word = dataRow[DataModelDictionary.WordElementModelDictionary[DataModelWordElement.Word]].ToString(),
                    Substitute = dataRow[DataModelDictionary.WordElementModelDictionary[DataModelWordElement.Substitute]].ToString(),
                    WordListName = dataRow[DataModelDictionary.WordElementModelDictionary[DataModelWordElement.WordListName]].ToString().Trim(),
                };

                wordElementsCollection.Add(wordElement);
                counter++;
            }

            _levelBasedDictionary.Add(LEVEL_ONE, wordElementsCollection);
        }

        // S26 - Entity Variant Definition
        /// <summary>
        /// Prepares Entity Variant Definition from excel rows 
        /// </summary>
        /// <param name="dataTable">Represents a single row read from excel sheet</param>
        private void LoadEntityVariantDefinitions(DataTable dataTable)
        {
            _levelBasedDictionary = new Dictionary<Int16, IDataModelObjectCollection>();

            Int32 definitionCounter = 2;
            Int32 levelCounter = 1;
            Int32 rowsToDelete = 0;
            Int32 totalRowsInEVD = 0;
            EntityVariantDefinitionCollection entityVariantDefinitionCollection = new EntityVariantDefinitionCollection();

            foreach (DataRow dataRow in dataTable.Rows)
            {
                ObjectAction objectAction = GetActionFromDataRow(dataRow);

                if (objectAction == ObjectAction.Read || objectAction == ObjectAction.Ignore)
                {
                    continue;
                }

                String entityVariantDefinitionName = dataRow[DataModelDictionary.EntityVariantDefinitionDictionary[DataModelEntityVariantDefinition.EntityVariantDefinitionName]].ToString().Trim();

                EntityVariantDefinition definition = entityVariantDefinitionCollection.Get(entityVariantDefinitionName);

                if (definition == null)
                {
                    if (entityVariantDefinitionCollection.Count > 0)
                    {
                        if (totalRowsInEVD < dataTable.Rows.Count && rowsToDelete == totalRowsInEVD)
                        {
                            entityVariantDefinitionCollection.Last().Action = ObjectAction.Delete;
                        } 
                    }
                    
                    // Reset counters while each new  creation of EVD object
                    totalRowsInEVD = 1;
                    rowsToDelete = 0;
                    
                    definition = new EntityVariantDefinition
                    {
                        ReferenceId = definitionCounter.ToString(CultureInfo.InvariantCulture),
                        Action = ObjectAction.Create,
                        Name = entityVariantDefinitionName,
                        RootEntityTypeName = dataRow[DataModelDictionary.EntityVariantDefinitionDictionary[DataModelEntityVariantDefinition.RootEntityType]].ToString().Trim(),
                    };

                    String hasDimensionAttributes = DataModelDictionary.EntityVariantDefinitionDictionary[DataModelEntityVariantDefinition.HasDimensionAttributes];

                    //Check HasDimensionAttributes exists or not.
                    if (dataTable.Columns.Contains(hasDimensionAttributes))
                    {
                        definition.HasDimensionAttributes = ValueTypeHelper.BooleanTryParse(dataRow[hasDimensionAttributes].ToString(), definition.HasDimensionAttributes);
                    }

                    definition.EntityVariantLevels = new EntityVariantLevelCollection();
                    entityVariantDefinitionCollection.Add(definition);

                    definitionCounter++;
                }
                else
                {
                    totalRowsInEVD += 1;
                }

                if (objectAction == ObjectAction.Delete)
                {
                    rowsToDelete += 1;
                    continue;
                }

                String childEntityTypeName = dataRow[DataModelDictionary.EntityVariantDefinitionDictionary[DataModelEntityVariantDefinition.ChildEntityType]].ToString().Trim();
                EntityVariantLevel variantLevel = definition.EntityVariantLevels.GetByEntityType(childEntityTypeName);

                levelCounter++;

                Int32 rank = ValueTypeHelper.Int32TryParse(dataRow[DataModelDictionary.EntityVariantDefinitionDictionary[DataModelEntityVariantDefinition.ChildLevel]].ToString().Trim(), 0);

                // Create new entity variant level object if there are different levels passed with same entity types.
                // That validation will be taken care in EntityValidationBL .
                if (variantLevel == null || variantLevel.Rank != rank)
                {
                    variantLevel = new EntityVariantLevel
                    {
                        ReferenceId = levelCounter.ToString(CultureInfo.InvariantCulture),
                        Action = ObjectAction.Create,
                        EntityTypeName = childEntityTypeName,
                        Rank = rank
                    };

                    variantLevel.RuleAttributes = new EntityVariantRuleAttributeCollection();
                    definition.EntityVariantLevels.Add(variantLevel);
                }

                EntityVariantRuleAttribute ruleAttribute = new EntityVariantRuleAttribute
                {
                    ReferenceId = levelCounter.ToString(CultureInfo.InvariantCulture),
                    Action = objectAction,
                    SourceAttributeName = dataRow[DataModelDictionary.EntityVariantDefinitionDictionary[DataModelEntityVariantDefinition.SourceAttribute]].ToString(),
                    TargetAttributeName = dataRow[DataModelDictionary.EntityVariantDefinitionDictionary[DataModelEntityVariantDefinition.TargetAttribute]].ToString(),
                    IsOptional = ValueTypeHelper.BooleanTryParse(dataRow[DataModelDictionary.EntityVariantDefinitionDictionary[DataModelEntityVariantDefinition.IsOptional]].ToString(), false)
                };

                variantLevel.RuleAttributes.Add(ruleAttribute);
            }

            if (entityVariantDefinitionCollection.Count == 1)
            {
                if (dataTable.Rows.Count == totalRowsInEVD && rowsToDelete == totalRowsInEVD)
                {
                    entityVariantDefinitionCollection.FirstOrDefault().Action = ObjectAction.Delete;
                } 
            }

            _levelBasedDictionary.Add(LEVEL_ONE, entityVariantDefinitionCollection);
        }

        //S27 - EVD Mapping
        /// <summary>
        /// Prepares EVD Mapping from excel rows 
        /// </summary>
        /// <param name="dataTable">Represents a single row read from excel sheet</param>
        private void LoadEntityVariantDefinitionMappings(DataTable dataTable)
        {
            _levelBasedDictionary = new Dictionary<Int16, IDataModelObjectCollection>();

            Int32 counter = 2;
            EntityVariantDefinitionMappingCollection entityVariantDefinitionMappings = new EntityVariantDefinitionMappingCollection();

            foreach (DataRow dataRow in dataTable.Rows)
            {
                ObjectAction objectAction = GetActionFromDataRow(dataRow);

                if (objectAction == ObjectAction.Read || objectAction == ObjectAction.Ignore)
                {
                    continue;
                }

                String categoryPath = dataRow[DataModelDictionary.EntityVariantDefinitionMappingDictionary[DataModelEntityVariantDefinitionMapping.CategoryPath]].ToString().Trim();

                EntityVariantDefinitionMapping entityVariantDefinitionMapping = new EntityVariantDefinitionMapping();
                entityVariantDefinitionMapping.ReferenceId = counter.ToString();
                entityVariantDefinitionMapping.Action = objectAction;
                entityVariantDefinitionMapping.EntityVariantDefinitionName = dataRow[DataModelDictionary.EntityVariantDefinitionMappingDictionary[DataModelEntityVariantDefinitionMapping.EntityVariantDefinitionName]].ToString().Trim();
                entityVariantDefinitionMapping.ContainerName = dataRow[DataModelDictionary.EntityVariantDefinitionMappingDictionary[DataModelEntityVariantDefinitionMapping.ContainerName]].ToString().Trim();
                
                if(!String.IsNullOrWhiteSpace(categoryPath))
                {
                    entityVariantDefinitionMapping.CategoryPath = categoryPath.Replace(_separator, _categoryPathSeparator);
                }

                entityVariantDefinitionMappings.Add(entityVariantDefinitionMapping);
                counter++;
            }

            _levelBasedDictionary.Add(LEVEL_ONE, entityVariantDefinitionMappings);
        }

        #endregion

        #region Utility Methods

        /// <summary>
        /// Add or update the container to the level based dictionary
        /// </summary>
        /// <param name="level">Indicates the level</param>
        /// <param name="organization">Indicates the container</param>
        /// <param name="levelBasedOrganizationDictionary">Indicates the level based container dictionary object</param>
        private void AddORUpdateOrganizationInDictionary(Int16 level, Organization organization, Dictionary<Int16, OrganizationCollection> levelBasedOrganizationDictionary)
        {
            OrganizationCollection organizations = null;

            levelBasedOrganizationDictionary.TryGetValue(level, out organizations);

            if (organizations == null)
            {
                levelBasedOrganizationDictionary.Add(level, new OrganizationCollection() { organization });
            }
            else
            {
                organizations.Add(organization);
            }
        }

        /// <summary>
        /// Add or update the container to the level based dictionary
        /// </summary>
        /// <param name="level">Indicates the level of container level based dictionary</param>
        /// <param name="container">Indicates the container</param>
        /// <param name="levelBasedContainerDictionary">Indicates the level based container dictionary object</param>
        private void AddORUpdateContainerInDictionary(Int16 level, Container container, Dictionary<Int16, ContainerCollection> levelBasedContainerDictionary)
        {
            ContainerCollection containers = null;

            levelBasedContainerDictionary.TryGetValue(level, out containers);

            if (containers == null)
            {
                levelBasedContainerDictionary.Add(level, new ContainerCollection() { container });
            }
            else
            {
                containers.Add(container);
            }
        }

        /// <summary>
        /// Add or update the category to the level based dictionary
        /// </summary>
        /// <param name="level">Indicates the level</param>
        /// <param name="category">Indicates the category</param>
        /// <param name="levelBasedCategoryDictionary">Indicates the level based category dictionary object</param>
        private void AddORUpdateCategoryInDictionary(Int16 level, Category category, Dictionary<Int16, CategoryCollection> levelBasedCategoryDictionary)
        {
            CategoryCollection categories = null;

            levelBasedCategoryDictionary.TryGetValue(level, out categories);

            if (categories == null)
            {
                levelBasedCategoryDictionary.Add(level, new CategoryCollection() { category });
            }
            else
            {
                categories.Add(category);
            }
        }

        /// <summary>
        /// Get ObjectAction From DataRow
        /// </summary>
        /// <param name="dataRow">DataRow </param>
        /// <returns>ObjectAction</returns>
        private ObjectAction GetActionFromDataRow(DataRow dataRow)
        {
            //Set default object Action as Create.
            ObjectAction objectAction = ObjectAction.Create;

            String action = dataRow[DataModelDictionary.CommonHeadersDictionary[DataModelCommonHeaders.Action]].ToString().Trim();

            if (!String.IsNullOrWhiteSpace(action))
            {
                Enum.TryParse(action, true, out objectAction);
            }

            return objectAction;
        }

        /// <summary>
        /// Get AttributeModelType From DataRow
        /// </summary>
        /// <param name="attributeModelType">AttributeModelType String</param>
        /// <returns>AttributeModelType</returns>
        private AttributeModelType GetAttributeTypeFromDataRow(String attributeModelType)
        {
            switch (attributeModelType.ToLower())
            {
                case "system":
                    return AttributeModelType.System;

                case "metadataattribute":
                    return AttributeModelType.MetaDataAttribute;

                case "common attribute group":
                    return AttributeModelType.CommonAttributeGroup;

                case "category attribute group":
                    return AttributeModelType.CategoryAttributeGroup;

                case "relationship attribute group":
                    return AttributeModelType.RelationshipAttributeGroup;

                case "common":
                    return AttributeModelType.Common;

                case "category":
                    return AttributeModelType.Category;

                case "relationship":
                    return AttributeModelType.Relationship;

                default:
                    return AttributeModelType.Unknown;
            }
        }

        /// <summary>
        /// Fill the attribute name and attribute parent name based on path
        /// </summary>
        /// <param name="attributePath">Indicates the attribute path</param>
        /// <param name="attributeName">Indicates the attribute name</param>
        /// <param name="attributeParentName">Indicates the attribute parent name</param>
        private void PopulateAttributeNameAndParentNameBasedOnPath(String attributePath, ref String attributeName, ref String attributeParentName)
        {
            if (!String.IsNullOrEmpty(attributePath) && attributePath.Contains(_separator))
            {
                String[] splittedValues = attributePath.Split(new String[] { _separator }, StringSplitOptions.RemoveEmptyEntries);

                if (splittedValues != null)
                {
                    if (splittedValues.Length > 0)
                    {
                        attributeParentName = splittedValues[0].Trim();
                    }
                    if (splittedValues.Length > 1)
                    {
                        attributeName = splittedValues[1].Trim();
                    }
                }
            }
        }

        /// <summary>
        /// Returns range for inclusive flag and variable type
        /// </summary>
        /// <param name="range">Specifies a range that should be returned</param>
        /// <param name="isInclusiveFlagString">Specifies inclusive flag</param>
        /// <param name="isInclusiveVariable">Specifies type of variable (inclusive or exclusive)</param>
        /// <returns>Returns a range value or empty string</returns>
        private static String GetRange(String range, String isInclusiveFlagString, Boolean isInclusiveVariable)
        {
            const String defaultValue = "";
            Boolean? isInclusiveFlag = IsRangeInclusive(isInclusiveFlagString);

            if (!String.IsNullOrEmpty(range))
            {
                isInclusiveFlag = (isInclusiveFlag == true) ? true : false;
                if (isInclusiveFlag == isInclusiveVariable)
                {
                    return range;
                }
            }
            return defaultValue;
        }

        /// <summary>
        /// Denotes the boolean value of inclusiveFlag
        /// </summary>
        /// <param name="inclusiveFlag">Specifies inclusiveFlag</param>
        /// <returns>Returns corresponding boolean value or null</returns>
        private static Boolean? IsRangeInclusive(String inclusiveFlag)
        {
            if (inclusiveFlag.ToUpper().Equals("YES"))
            {
                return true;
            }
            if (inclusiveFlag.ToUpper().Equals("NO"))
            {
                return false;
            }
            return null;
        }

        #endregion
    }
}