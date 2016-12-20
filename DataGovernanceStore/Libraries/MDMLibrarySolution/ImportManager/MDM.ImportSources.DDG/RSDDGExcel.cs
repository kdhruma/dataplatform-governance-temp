using System;
using System.Collections.ObjectModel;
using System.Data;

namespace MDM.ImportSources.DDG
{
    using MDM.BusinessObjects;
    using MDM.BusinessObjects.Diagnostics;
    using MDM.BusinessObjects.Exports;
    using MDM.BusinessObjects.Jobs;
    using MDM.BusinessRuleManagement.Business;
    using MDM.Core;
    using MDM.ExceptionManager;
    using MDM.Imports.Interfaces;
    using MDM.Interfaces;
    using MDM.Utility;

    /// <summary>
    /// This class implements the source data interface.It reads the data into a dataset and process them as per the request from the DDG import engine.
    /// </summary>
    public class RSDDGExcel : IDDGImportSourceData
    {
        #region Fields

        /// <summary>
        /// Single forward only reader needs to be synchronized.
        /// </summary>
        private Object _readerLock = new Object();

        /// <summary>
        /// Field for source file
        /// </summary>
        private String _sourceFile = String.Empty;

        /// <summary>
        /// Field for batching type
        /// </summary>
        private ImportProviderBatchingType _batchingType = ImportProviderBatchingType.Single;

        /// <summary>
        /// Field for batch size. Default is 100
        /// </summary>
        private Int16 _batchSize = 100;

        /// <summary>
        /// Indicates the current trace setting 
        /// </summary>
        private TraceSettings _currentTraceSettings = null;

        /// <summary>
        /// Indicates the diagnostic activity
        /// </summary>
        private DiagnosticActivity _diagnosticActivity = null;

        /// <summary>
        /// Field denotes the category path separator
        /// </summary>
        private String _categoryPathSeparator;

        #endregion Fields

        #region Properties

        /// <summary>
        /// Property denoting the batching type
        /// </summary>
        public ImportProviderBatchingType BatchingType
        {
            get { return _batchingType; }
            set { _batchingType = value; }
        }

        #endregion Properties

        #region Constructor

        /// <summary>
        /// Default Constructor
        /// </summary>
        public RSDDGExcel(String filePath)
        {
            _sourceFile = filePath;
            _currentTraceSettings = MDMOperationContextHelper.GetCurrentTraceSettings();

        }

        #endregion Constructor

        #region Methods

        #region IDDGImportSourceData Methods

        /// <summary>
        /// Provide an opportunity for the source data to initializes itself with some configuration from the job.
        /// </summary>
        /// <param name="job">Indicates the job which needs to be executed</param>
        /// <param name="ddgImportProfile">Indicates the import profile under which the job will be executed</param>
        /// <returns>True - If source data is initialized, False - If source data is failed to initialize</returns>
        public Boolean Initialize(Job job, IDDGImportProfile ddgImportProfile)
        {
            _diagnosticActivity = new DiagnosticActivity();

            try
            {
                if (_currentTraceSettings.IsBasicTracingEnabled)
                {
                    _diagnosticActivity.Start();
                    _diagnosticActivity.LogInformation("Initializing DDG Excel Source Data.");
                }

                if (String.IsNullOrWhiteSpace(_sourceFile))
                {
                    throw new ArgumentNullException("DDG Excel file is not available");
                }

                _batchSize = ValueTypeHelper.Int16TryParse(ddgImportProfile.DDGJobProcessingOptions.BatchSize.ToString(), _batchSize);
                _categoryPathSeparator = AppConfigurationHelper.GetAppConfig<String>("Catalog.Category.PathSeparator", " » ");

                if (System.IO.File.Exists(_sourceFile))
                {
                    ReadMetadata();
                    ReadConfiguration();
                }
                else
                {
                    throw new ArgumentException(String.Format("DDG excel file is not available at the specified location {0}", _sourceFile));
                }

                return true;
            }
            finally
            {
                if (_currentTraceSettings.IsBasicTracingEnabled)
                {
                    _diagnosticActivity.LogInformation("Initialized DDG Excel Source Data.");
                    _diagnosticActivity.Stop();
                }
            }
        }

        /// <summary>
        /// Returns all DDG Object Types For Import
        /// </summary>
        /// <param name="callerContext">Indicates the name of the Application and Module which invoked the API</param>
        /// <returns>Collection of DDG Object Types</returns>
        public Collection<ObjectType> GetAllDDGObjectTypesForImport(ICallerContext callerContext)
        {
            _diagnosticActivity = new DiagnosticActivity();

            try
            {
                if (_currentTraceSettings.IsBasicTracingEnabled)
                {
                    _diagnosticActivity.Start();
                    _diagnosticActivity.LogInformation("Preparing collection of DDGObject Types to be processed by DDGImportEngine.");
                }

                Collection<ObjectType> ddgObjectTypesToReturn = new Collection<ObjectType>();

                foreach (ObjectType metadataConfigKey in DDGDictionary.MetadataItemsDictionary.Keys)
                {
                    if (DDGDictionary.MetadataItemsDictionary[metadataConfigKey])
                    {
                        ddgObjectTypesToReturn.Add(metadataConfigKey);
                    }
                }

                return ddgObjectTypesToReturn;
            }
            finally
            {
                if (_currentTraceSettings.IsBasicTracingEnabled)
                {
                    _diagnosticActivity.LogInformation("Prepared collection of DDGObject Types to be processed by DDGImportEngine.");
                    _diagnosticActivity.Stop();
                }
            }
        }

        /// <summary>
        /// Returns All DDG Objects based on the Object Type 
        /// </summary>
        /// <param name="ddgObject">Indicates the type of DDG Object</param>
        /// <param name="iCallerContext">Indicates the name of the Application and Module which invoked the API</param>
        /// <returns>Collection of DDG Objects</returns>
        public IBusinessRuleObjectCollection GetAllDDGObjects(ObjectType ddgObjectType, ICallerContext iCallerContext)
        {
            IBusinessRuleObjectCollection ddgObjects = null;
            _diagnosticActivity = new DiagnosticActivity();

            try
            {
                if (_currentTraceSettings.IsBasicTracingEnabled)
                {
                    _diagnosticActivity.Start();
                    _diagnosticActivity.LogInformation(String.Format("Reading of records from [{0}] started.", ddgObjectType.ToString()));
                }

                if (DDGDictionary.ObjectsDictionary.ContainsKey(ddgObjectType) == false)
                {
                    if (_currentTraceSettings.IsBasicTracingEnabled)
                    {
                        _diagnosticActivity.LogError(String.Format("Reading of records from [{0}] completed as it is not supported.", ddgObjectType.ToString()));
                    }
                    return ddgObjects;
                }

                ddgObjects = ReadDataSetFromFile(ddgObjectType);
            }
            finally
            {
                if (_currentTraceSettings.IsBasicTracingEnabled)
                {
                    _diagnosticActivity.LogInformation(String.Format("Reading of records from [{0}] completed successfully.", ddgObjectType.ToString()));
                    _diagnosticActivity.Stop();
                }
            }

            return ddgObjects;
        }

        /// <summary>
        /// Returns All DDG Objects based on the Object Type in a Batch
        /// </summary>
        /// <param name="ddgObject">Indicates the Type of DDG Object</param>
        /// <param name="batchSize">Indicate the size of Batch</param>
        /// <param name="iCallerContext">Indicates the name of the Application and Module which invoked the API</param>
        /// <returns>Collection of DDG Object Types</returns>
        public IBusinessRuleObjectCollection GetDDGObjectsNextBatch(ObjectType ddgObject, Int32 batchSize, ICallerContext iCallerContext)
        {
            throw new NotImplementedException();
        }

        #endregion IDDGImportSourceData Methods

        #region Private Methods

        private void ReadMetadata()
        {
            try
            {
                if (_currentTraceSettings.IsBasicTracingEnabled)
                {
                    _diagnosticActivity = new DiagnosticActivity();
                    _diagnosticActivity.Start();
                    _diagnosticActivity.LogInformation("Begin reading metadata sheet.");
                }

                DataSet dataSet = ExternalFileReader.ReadExternalFileSAXParsing(_sourceFile, DDGDictionary.ObjectsDictionary[ObjectType.Metadata], false);

                if (dataSet != null && dataSet.Tables != null && dataSet.Tables.Count > 0)
                {
                    DataTable dataTable = dataSet.Tables[0];

                    lock (_readerLock)
                    {
                        if (DDGDictionary.MetadataItemsDictionary.Count > 1)
                        {
                            DDGDictionary.MetadataItemsDictionary.Clear();
                        }

                        String metadataConfigName = String.Empty;
                        String metadataConfigValue = String.Empty;

                        foreach (DataRow row in dataTable.Rows)
                        {
                            metadataConfigName = row[DDGDictionary.MetadataDictionary[Metadata.PhysicalSheetName]].ToString().Trim();

                            if (!String.IsNullOrWhiteSpace(metadataConfigName) && DDGDictionary.SheetNameToObjectTypeMap.ContainsKey(metadataConfigName))
                            {
                                metadataConfigValue = row[DDGDictionary.MetadataDictionary[Metadata.Processdatasheet]].ToString().Trim();

                                if (!String.IsNullOrWhiteSpace(metadataConfigValue))
                                {
                                    DDGDictionary.MetadataItemsDictionary.Add(DDGDictionary.SheetNameToObjectTypeMap[metadataConfigName], ValueTypeHelper.BooleanTryParse(metadataConfigValue, false));
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler exHandler = new ExceptionHandler(ex);

                // Re-Throwing exception for further processing.
                throw ex;
            }
            finally
            {
                if (_currentTraceSettings.IsBasicTracingEnabled)
                {
                    _diagnosticActivity.LogError("Finished reading metadata sheet.");
                    _diagnosticActivity.Stop();
                }
            }
        }

        private void ReadConfiguration()
        {
            try
            {
                if (_currentTraceSettings.IsBasicTracingEnabled)
                {
                    _diagnosticActivity = new DiagnosticActivity();
                    _diagnosticActivity.Start();
                    _diagnosticActivity.LogInformation("Begin reading configuration sheet.");
                }

                DataSet dataSet = ExternalFileReader.ReadExternalFileSAXParsing(_sourceFile, DDGDictionary.ObjectsDictionary[ObjectType.Configuration], false);

                if (dataSet != null && dataSet.Tables != null && dataSet.Tables.Count > 0)
                {
                    DataTable dataTable = dataSet.Tables[0];

                    lock (_readerLock)
                    {
                        if (DDGDictionary.ConfigurationItemsDictionary.Count > 1)
                        {
                            DDGDictionary.ConfigurationItemsDictionary.Clear();
                        }

                        String configName = String.Empty;
                        String configValue = String.Empty;

                        foreach (DataRow row in dataTable.Rows)
                        {
                            configName = row[DDGDictionary.ConfigurationDictionary[Configuration.ConfigurationKey]].ToString().Trim();

                            if (!String.IsNullOrWhiteSpace(configName) && DDGDictionary.ConfigurationItemMapDictionary.ContainsKey(configName))
                            {
                                configValue = row[DDGDictionary.ConfigurationDictionary[Configuration.ConfigurationValue]].ToString().Trim();

                                if (!String.IsNullOrWhiteSpace(configValue))
                                {
                                    DDGDictionary.ConfigurationItemsDictionary.Add(DDGDictionary.ConfigurationItemMapDictionary[configName], configValue);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler exHandler = new ExceptionHandler(ex);

                // Re-Throwing exception for further processing.
                throw ex;
            }
            finally
            {
                if (_currentTraceSettings.IsBasicTracingEnabled)
                {
                    _diagnosticActivity.LogInformation("Finished reading configuration sheet.");
                    _diagnosticActivity.Stop();
                }
            }
        }

        private IBusinessRuleObjectCollection ReadDataSetFromFile(ObjectType ddgObjectType)
        {
            IBusinessRuleObjectCollection ddgObjects = null;

            try
            {
                if (_currentTraceSettings.IsBasicTracingEnabled)
                {
                    _diagnosticActivity = new DiagnosticActivity();
                    _diagnosticActivity.Start();
                }

                if (DDGDictionary.ObjectsDictionary.ContainsKey(ddgObjectType))
                {
                    String sheetName = DDGDictionary.ObjectsDictionary[ddgObjectType];
                    DataSet dataSet = ExternalFileReader.ReadExternalFileSAXParsing(_sourceFile, sheetName, false);
                    if (dataSet != null && dataSet.Tables != null && dataSet.Tables.Count > 0)
                    {
                        DataTable dataTable = dataSet.Tables[0];
                        dataTable.TableName = sheetName;

                        switch (ddgObjectType)
                        {
                            case ObjectType.BusinessRules:
                                ddgObjects = FillBusinessRules(dataTable);
                                break;

                            case ObjectType.BusinessConditions:
                                ddgObjects = FillBusinessConditions(dataTable);
                                break;

                            case ObjectType.BusinessConditionSorting:
                                ddgObjects = FillBusinessConditionsSorting(dataTable);
                                break;

                            case ObjectType.DynamicGovernance:
                                ddgObjects = FillDynamicGovernance(dataTable);
                                break;

                            case ObjectType.DynamicGovernanceSorting:
                                ddgObjects = FillDynamicGovernanceSorting(dataTable);
                                break;

                            case ObjectType.SystemMessages:
                                ddgObjects = FillDDGLocaleMessages(dataTable);
                                break;
                        }
                    }
                }
                else
                {
                    if (_currentTraceSettings.IsBasicTracingEnabled)
                    {
                        _diagnosticActivity.LogError(String.Format("Reading [{0}] sheet completed as it is not supported.", ddgObjectType.ToString()));
                    }
                }
            }
            finally
            {
                if (_currentTraceSettings.IsBasicTracingEnabled)
                {
                    _diagnosticActivity.Stop();
                }
            }

            return ddgObjects;
        }

        private IBusinessRuleObjectCollection FillBusinessRules(DataTable dataTable)
        {
            MDMRuleCollection mdmRules = new MDMRuleCollection();
            Int32 counter = -2;

            try
            {
                if (_currentTraceSettings.IsBasicTracingEnabled)
                {
                    _diagnosticActivity = new DiagnosticActivity();
                    _diagnosticActivity.Start();
                    _diagnosticActivity.LogInformation(String.Format("Begin reading [{0}] sheet.", ObjectType.BusinessRules.ToString()));
                }

                foreach (DataRow dataRow in dataTable.Rows)
                {
                    ObjectAction objectAction = GetActionFromDataRow(dataRow);

                    if (objectAction == ObjectAction.Ignore)
                    {
                        continue;
                    }

                    MDMRule mdmRule = new MDMRule()
                    {
                        ReferenceId = counter,
                        Id = counter,
                        Action = objectAction,
                        Name = dataRow[DDGDictionary.ExportImportCommonHeadersDictionary[ExportImportCommonHeaders.Name]].ToString().Trim(),
                        IsEnabled = ValueTypeHelper.BooleanTryParse(dataRow[DDGDictionary.BusinessRuleDictionary[BusinessRuleEnum.IsEnabled]].ToString().Trim(), false),
                        Description = dataRow[DDGDictionary.BusinessRuleDictionary[BusinessRuleEnum.Description]].ToString().Trim(),
                        RuleDefinition = dataRow[DDGDictionary.BusinessRuleDictionary[BusinessRuleEnum.RuleDefinition]].ToString().Trim(),
                        TargetAttributeName = dataRow[DDGDictionary.BusinessRuleDictionary[BusinessRuleEnum.TargetAttributeName]].ToString().Trim()
                    };

                    MDMRuleType ruleType = MDMRuleType.UnKnown;

                    String ruleTypeFromFile = dataRow[DDGDictionary.BusinessRuleDictionary[BusinessRuleEnum.RuleType]].ToString().Trim();

                    if (!ValueTypeHelper.EnumTryParse<MDMRuleType>(ruleTypeFromFile, true, out ruleType))
                    {
                        //This is to support the backward compatibility. If the user uses the old template for import, then that also should be supported.
                        //'Validation', 'Computation', 'DataModel' and 'Others' Rule type is merged into one rule type 'Governance'
                        switch (ruleTypeFromFile)
                        {
                            case "Validation":
                            case "Computation":
                            case "DataModel":
                            case "Others": ruleType = MDMRuleType.Governance;
                                break;
                        }
                    }

                    mdmRule.RuleType = ruleType;

                    RuleStatus ruleStatus = RuleStatus.Draft;
                    ValueTypeHelper.EnumTryParse<RuleStatus>(dataRow[DDGDictionary.BusinessRuleDictionary[BusinessRuleEnum.Status]].ToString().Trim(), true, out ruleStatus);
                    mdmRule.RuleStatus = ruleStatus;

                    DisplayType displayType = DisplayType.Unknown;
                    ValueTypeHelper.EnumTryParse<DisplayType>(dataRow[DDGDictionary.BusinessRuleDictionary[BusinessRuleEnum.DisplayType]].ToString().Trim(), true, out displayType);
                    mdmRule.DisplayType = displayType;

                    Collection<String> objectNames = ValueTypeHelper.SplitStringToStringCollection(dataRow[DDGDictionary.BusinessRuleDictionary[BusinessRuleEnum.DisplayList]].ToString().Trim(), DDGDictionary.ConfigurationItemsDictionary[ConfigurationItem.DefaultSeperator]);
                    mdmRule.DisplayList.SetDisplayListByNames(objectNames);

                    LocaleEnum targetLocale = LocaleEnum.UnKnown;
                    ValueTypeHelper.EnumTryParse<LocaleEnum>(dataRow[DDGDictionary.BusinessRuleDictionary[BusinessRuleEnum.TargetLocale]].ToString().Trim(), true, out targetLocale);
                    mdmRule.TargetLocale = targetLocale;

                    mdmRules.Add(mdmRule);
                    counter--;
                }
            }
            finally
            {
                if (_currentTraceSettings.IsBasicTracingEnabled)
                {
                    _diagnosticActivity.LogInformation(String.Format("Finished reading [{0}] sheet.", ObjectType.BusinessRules.ToString()));
                    _diagnosticActivity.Stop();
                }
            }

            return mdmRules;
        }

        private IBusinessRuleObjectCollection FillBusinessConditions(DataTable dataTable)
        {
            MDMRuleCollection mdmRules = new MDMRuleCollection();
            Int32 counter = -2;

            try
            {
                if (_currentTraceSettings.IsBasicTracingEnabled)
                {
                    _diagnosticActivity = new DiagnosticActivity();
                    _diagnosticActivity.Start();
                    _diagnosticActivity.LogInformation(String.Format("Begin reading [{0}] sheet.", ObjectType.BusinessConditions.ToString()));
                }

                foreach (DataRow dataRow in dataTable.Rows)
                {
                    ObjectAction objectAction = GetActionFromDataRow(dataRow);

                    if (objectAction == ObjectAction.Ignore)
                    {
                        continue;
                    }

                    MDMRule mdmRule = new MDMRule()
                    {
                        ReferenceId = counter,
                        Id = counter,
                        Action = objectAction,
                        Name = dataRow[DDGDictionary.ExportImportCommonHeadersDictionary[ExportImportCommonHeaders.Name]].ToString().Trim(),
                        RuleType = MDMRuleType.BusinessCondition,
                        IsEnabled = ValueTypeHelper.BooleanTryParse(dataRow[DDGDictionary.BusinessConditionsDictionary[BusinessConditionEnum.IsEnabled]].ToString().Trim(), false),
                        Description = dataRow[DDGDictionary.BusinessConditionsDictionary[BusinessConditionEnum.Description]].ToString().Trim(),
                    };

                    Collection<String> publishedBCRulesNames = ValueTypeHelper.SplitStringToStringCollection(dataRow[DDGDictionary.BusinessConditionsDictionary[BusinessConditionEnum.PublishedBusinessConditionRulesNames]].ToString().Trim(), DDGDictionary.ConfigurationItemsDictionary[ConfigurationItem.DefaultSeperator]);
                    mdmRule.SetBusinessConditionRules(publishedBCRulesNames, RuleStatus.Published);

                    Collection<String> draftBCRulesNames = ValueTypeHelper.SplitStringToStringCollection(dataRow[DDGDictionary.BusinessConditionsDictionary[BusinessConditionEnum.DraftBusinessConditionRulesNames]].ToString().Trim(), DDGDictionary.ConfigurationItemsDictionary[ConfigurationItem.DefaultSeperator]);
                    mdmRule.SetBusinessConditionRules(draftBCRulesNames, RuleStatus.Draft);

                    RuleStatus ruleStatus = RuleStatus.Draft;
                    ValueTypeHelper.EnumTryParse<RuleStatus>(dataRow[DDGDictionary.BusinessRuleDictionary[BusinessRuleEnum.Status]].ToString().Trim(), true, out ruleStatus);
                    mdmRule.RuleStatus = ruleStatus;

                    mdmRules.Add(mdmRule);
                    counter--;
                }
            }
            finally
            {
                if (_currentTraceSettings.IsBasicTracingEnabled)
                {
                    _diagnosticActivity.LogInformation(String.Format("Finished reading [{0}] sheet.", ObjectType.BusinessConditions.ToString()));
                    _diagnosticActivity.Stop();
                }
            }

            return mdmRules;
        }

        private IBusinessRuleObjectCollection FillBusinessConditionsSorting(DataTable dataTable)
        {
            MDMRuleCollection mdmRules = new MDMRuleCollection();
            Int32 counter = -2;

            try
            {
                if (_currentTraceSettings.IsBasicTracingEnabled)
                {
                    _diagnosticActivity = new DiagnosticActivity();
                    _diagnosticActivity.Start();
                    _diagnosticActivity.LogInformation(String.Format("Begin reading [{0}] sheet.", ObjectType.BusinessConditionSorting.ToString()));
                }

                foreach (DataRow dataRow in dataTable.Rows)
                {
                    ObjectAction objectAction = GetActionFromDataRow(dataRow);

                    if (objectAction == ObjectAction.Ignore)
                    {
                        continue;
                    }

                    MDMRule mdmRule = new MDMRule()
                    {
                        ReferenceId = counter,
                        Id = counter,
                        Name = dataRow[DDGDictionary.BusinessConditionSortingDictionary[BusinessConditionSortingEnum.BusinessConditionName]].ToString().Trim()
                    };

                    RuleStatus businesConditionStatus = RuleStatus.Draft;
                    ValueTypeHelper.EnumTryParse<RuleStatus>(dataRow[DDGDictionary.BusinessConditionSortingDictionary[BusinessConditionSortingEnum.BusinessConditionStatus]].ToString().Trim(), true, out businesConditionStatus);
                    mdmRule.RuleStatus = businesConditionStatus;


                    MDMRule businessConditionRule = new MDMRule()
                    {
                        Name = dataRow[DDGDictionary.BusinessConditionSortingDictionary[BusinessConditionSortingEnum.BusinessRuleName]].ToString().Trim(),
                        Sequence = ValueTypeHelper.ConvertToInt32(dataRow[DDGDictionary.BusinessConditionSortingDictionary[BusinessConditionSortingEnum.Sequence]].ToString().Trim())
                    };

                    RuleStatus businessConditionRuleStatus = RuleStatus.Draft;
                    ValueTypeHelper.EnumTryParse<RuleStatus>(dataRow[DDGDictionary.BusinessConditionSortingDictionary[BusinessConditionSortingEnum.BusinessRuleStatus]].ToString().Trim(), true, out businessConditionRuleStatus);
                    businessConditionRule.RuleStatus = businessConditionRuleStatus;


                    if (!mdmRules.Contains(mdmRule.Name))
                    {
                        mdmRule.BusinessConditionRules.Add(businessConditionRule);
                        mdmRules.Add(mdmRule);
                    }
                    else
                    {
                        MDMRule businessCondition = mdmRules.GetMDMRuleByName(mdmRule.Name);

                        if (!businessCondition.BusinessConditionRules.Contains(businessConditionRule.Name))
                        {
                            businessCondition.BusinessConditionRules.Add(businessConditionRule);
                        }
                        else
                        {
                            if (_currentTraceSettings.IsBasicTracingEnabled)
                            {
                                _diagnosticActivity.LogError(String.Format("Failed to read Business Condition Sorting information on row {0} as, Business Rule: '{1}' is already mapped to Business Condition: '{2}'.", Math.Abs(counter), businessConditionRule.Name, businessCondition.Name));
                            }
                        }
                    }

                    counter--;
                }
            }
            finally
            {
                if (_currentTraceSettings.IsBasicTracingEnabled)
                {
                    _diagnosticActivity.LogInformation(String.Format("Finished reading [{0}] sheet.", ObjectType.BusinessConditionSorting.ToString()));
                    _diagnosticActivity.Stop();
                }
            }

            return mdmRules;
        }

        private IBusinessRuleObjectCollection FillDynamicGovernance(DataTable dataTable)
        {
            MDMRuleMapCollection mdmRuleMaps = new MDMRuleMapCollection();
            Int32 counter = -2;

            try
            {
                if (_currentTraceSettings.IsBasicTracingEnabled)
                {
                    _diagnosticActivity = new DiagnosticActivity();
                    _diagnosticActivity.Start();
                    _diagnosticActivity.LogInformation(String.Format("Begin reading [{0}] sheet.", ObjectType.DynamicGovernance.ToString()));
                }

                foreach (DataRow dataRow in dataTable.Rows)
                {
                    ObjectAction objectAction = GetActionFromDataRow(dataRow);

                    if (objectAction == ObjectAction.Ignore)
                    {
                        continue;
                    }

                    MDMRuleMap mdmRuleMap = new MDMRuleMap()
                    {
                        ReferenceId = counter,
                        Id = counter,
                        Action = objectAction
                    };

                    #region Metadata

                    mdmRuleMap.Name = dataRow[DDGDictionary.ExportImportCommonHeadersDictionary[ExportImportCommonHeaders.Name]].ToString().Trim();
                    mdmRuleMap.IsEnabled = ValueTypeHelper.BooleanTryParse(dataRow[DDGDictionary.DynamicGovernanceDictionary[DynamicGovernance.IsEnabled]].ToString().Trim(), false);
                    mdmRuleMap.EventName = dataRow[DDGDictionary.DynamicGovernanceDictionary[DynamicGovernance.EventName]].ToString().Trim();

                    String inputEventType = dataRow[DDGDictionary.DynamicGovernanceDictionary[DynamicGovernance.EventSource]].ToString().Trim();

                    if (String.IsNullOrWhiteSpace(inputEventType) == false)
                    {
                        MDMEventType eventType = MDMEventType.Unknown;
                        ValueTypeHelper.EnumTryParse<MDMEventType>(inputEventType, true, out eventType);
                        mdmRuleMap.EventType = eventType;
                    }
                    else
                    {
                        mdmRuleMap.EventType = MDMEventType.All;
                    }

                    MDMRuleMapRuleCollection ruleMapRuleCollection = new MDMRuleMapRuleCollection();

                    Collection<String> publishedBusinessRuleNames = ValueTypeHelper.SplitStringToStringCollection(dataRow[DDGDictionary.DynamicGovernanceDictionary[DynamicGovernance.PublishedBusinessRules]].ToString().Trim(), DDGDictionary.ConfigurationItemsDictionary[ConfigurationItem.DefaultSeperator]);
                    //TODO: Need to check here how to populate rule type.
                    this.PrepareRuleMapRules(mdmRuleMap, ruleMapRuleCollection, publishedBusinessRuleNames, MDMRuleType.Governance, RuleStatus.Published);

                    Collection<String> publishedBusinessConditionNames = ValueTypeHelper.SplitStringToStringCollection(dataRow[DDGDictionary.DynamicGovernanceDictionary[DynamicGovernance.PublishedBusinessConditions]].ToString().Trim(), DDGDictionary.ConfigurationItemsDictionary[ConfigurationItem.DefaultSeperator]);
                    this.PrepareRuleMapRules(mdmRuleMap, ruleMapRuleCollection, publishedBusinessConditionNames, MDMRuleType.BusinessCondition, RuleStatus.Published);

                    //TODO: Need to check here how to populate rule type.
                    Collection<String> draftBusinessRuleNames = ValueTypeHelper.SplitStringToStringCollection(dataRow[DDGDictionary.DynamicGovernanceDictionary[DynamicGovernance.DraftBusinessRules]].ToString().Trim(), DDGDictionary.ConfigurationItemsDictionary[ConfigurationItem.DefaultSeperator]);
                    this.PrepareRuleMapRules(mdmRuleMap, ruleMapRuleCollection, draftBusinessRuleNames, MDMRuleType.Governance, RuleStatus.Draft);

                    Collection<String> draftBusinessConditionNames = ValueTypeHelper.SplitStringToStringCollection(dataRow[DDGDictionary.DynamicGovernanceDictionary[DynamicGovernance.DraftBusinessConditions]].ToString().Trim(), DDGDictionary.ConfigurationItemsDictionary[ConfigurationItem.DefaultSeperator]);
                    this.PrepareRuleMapRules(mdmRuleMap, ruleMapRuleCollection, draftBusinessConditionNames, MDMRuleType.BusinessCondition, RuleStatus.Draft);

                    mdmRuleMap.MDMRuleMapRules = ruleMapRuleCollection;

                    #endregion Metadata

                    #region Application Context

                    ApplicationContext applicationContext = new ApplicationContext(5); // 5 ==> tb_ObjectType table's DDG entry. Need to see alternative as Enum for this.
                    applicationContext.ReferenceId = mdmRuleMap.ReferenceId;
                    applicationContext.OrganizationName = dataRow[DDGDictionary.DynamicGovernanceDictionary[DynamicGovernance.Organization]].ToString().Trim();
                    applicationContext.ContainerName = dataRow[DDGDictionary.DynamicGovernanceDictionary[DynamicGovernance.Container]].ToString().Trim();

                    String categoryPath = dataRow[DDGDictionary.DynamicGovernanceDictionary[DynamicGovernance.Category]].ToString().Trim();

                    if (!String.IsNullOrWhiteSpace(categoryPath))
                    {
                        applicationContext.CategoryPath = categoryPath.Replace(DDGDictionary.ConfigurationItemsDictionary[ConfigurationItem.DefaultSeperator], _categoryPathSeparator);
                    }
                    applicationContext.EntityTypeName = dataRow[DDGDictionary.DynamicGovernanceDictionary[DynamicGovernance.EntityType]].ToString().Trim();
                    applicationContext.RelationshipTypeName = dataRow[DDGDictionary.DynamicGovernanceDictionary[DynamicGovernance.RelationshipType]].ToString().Trim();
                    applicationContext.RoleName = dataRow[DDGDictionary.DynamicGovernanceDictionary[DynamicGovernance.SecurityRole]].ToString().Trim();
                    mdmRuleMap.ApplicationContext = applicationContext;

                    #endregion Application Context

                    #region Workflow Detail

                    WorkflowInfo workflowInfo = new WorkflowInfo();
                    workflowInfo.WorkflowName = dataRow[DDGDictionary.DynamicGovernanceDictionary[DynamicGovernance.WorkflowName]].ToString().Trim();

                    String WorkflowActivityName = dataRow[DDGDictionary.DynamicGovernanceDictionary[DynamicGovernance.WorkflowActivityName]].ToString().Trim();
                    if (FormatHelper.IsValidGuid(WorkflowActivityName))
                    {
                        workflowInfo.WorkflowActivityShortName = WorkflowActivityName;
                    }
                    else
                    {
                        workflowInfo.WorkflowActivityLongName = WorkflowActivityName;
                    }

                    workflowInfo.WorkflowActivityAction = dataRow[DDGDictionary.DynamicGovernanceDictionary[DynamicGovernance.WorkflowAction]].ToString().Trim();

                    mdmRuleMap.WorkflowInfo = workflowInfo;

                    #endregion Workflow Detail

                    mdmRuleMaps.Add(mdmRuleMap);
                    counter--;
                }
            }
            finally
            {
                if (_currentTraceSettings.IsBasicTracingEnabled)
                {
                    _diagnosticActivity.LogInformation(String.Format("Finished reading [{0}] sheet.", ObjectType.DynamicGovernance.ToString()));
                    _diagnosticActivity.Stop();
                }
            }

            return mdmRuleMaps;
        }

        private void PrepareRuleMapRules(MDMRuleMap mdmRuleMap, MDMRuleMapRuleCollection ruleMapRules, 
                                                Collection<String> ruleNames, MDMRuleType ruleType, RuleStatus ruleStatus)
        {
            if(ruleNames != null && ruleNames.Count > 0)
            {
                MDMRuleMapRule ruleMapRule = null;
                Int32 counter = -1;
                foreach (String ruleName in ruleNames)
                {
                    ruleMapRule = new MDMRuleMapRule
                        {
                            RuleId = counter, 
                            RuleName = ruleName, 
                            RuleMapId = mdmRuleMap.Id, 
                            RuleMapName = mdmRuleMap.Name, 
                            RuleType = ruleType, 
                            RuleStatus = ruleStatus
                        };

                    if (!ruleMapRules.Contains(ruleMapRule))
                    {
                        ruleMapRules.Add(ruleMapRule);
                        counter --;
                    }
                }
            }
        }

        private IBusinessRuleObjectCollection FillDynamicGovernanceSorting(DataTable dataTable)
        {
            MDMRuleMapRuleCollection mdmRuleMapRules = new MDMRuleMapRuleCollection();
            Int32 counter = -2;

            try
            {
                if (_currentTraceSettings.IsBasicTracingEnabled)
                {
                    _diagnosticActivity = new DiagnosticActivity();
                    _diagnosticActivity.Start();
                    _diagnosticActivity.LogInformation(String.Format("Begin reading [{0}] sheet.", ObjectType.DynamicGovernanceSorting.ToString()));
                }

                foreach (DataRow dataRow in dataTable.Rows)
                {
                    ObjectAction objectAction = GetActionFromDataRow(dataRow);

                    if (objectAction == ObjectAction.Ignore)
                    {
                        continue;
                    }

                    MDMRuleMapRule mdmRuleMapRule = new MDMRuleMapRule()
                    {
                        RuleMapName = dataRow[DDGDictionary.ExportImportCommonHeadersDictionary[ExportImportCommonHeaders.Name]].ToString().Trim(),
                        RuleName = dataRow[DDGDictionary.DynamicGovernanceSortingDictionary[DynamicGovernanceSortingEnum.MDMRule]].ToString().Trim(),

                    };

                    RuleStatus ruleStatus = RuleStatus.Draft;
                    ValueTypeHelper.EnumTryParse<RuleStatus>(dataRow[DDGDictionary.DynamicGovernanceSortingDictionary[DynamicGovernanceSortingEnum.Status]].ToString().Trim(), true, out ruleStatus);
                    mdmRuleMapRule.RuleStatus = ruleStatus;

                    MDMRuleType ruleType = MDMRuleType.UnKnown;
                    ValueTypeHelper.EnumTryParse<MDMRuleType>(dataRow[DDGDictionary.DynamicGovernanceSortingDictionary[DynamicGovernanceSortingEnum.RuleType]].ToString().Trim(), true, out ruleType);
                    mdmRuleMapRule.RuleType = ruleType;

                    mdmRuleMapRule.Sequence = ValueTypeHelper.Int32TryParse(dataRow[DDGDictionary.DynamicGovernanceSortingDictionary[DynamicGovernanceSortingEnum.Sequence]].ToString().Trim(), mdmRuleMapRule.Sequence);
                    mdmRuleMapRule.IgnoreChangeContext = ValueTypeHelper.BooleanTryParse(dataRow[DDGDictionary.DynamicGovernanceSortingDictionary[DynamicGovernanceSortingEnum.IgnoreChangeContext]].ToString().Trim(), mdmRuleMapRule.IgnoreChangeContext);

                    if (!mdmRuleMapRules.Contains(mdmRuleMapRule))
                    {
                        mdmRuleMapRules.Add(mdmRuleMapRule);
                    }
                    else
                    {
                        if (_currentTraceSettings.IsBasicTracingEnabled)
                        {
                            _diagnosticActivity.LogError(String.Format("Failed to read MDMRuleMapRule Sorting information on row {0} as, RuleMap having a name: '{1}' with Business Rule/Business Condition: '{2}' is already available in the collection.", Math.Abs(counter), mdmRuleMapRule.RuleMapName, mdmRuleMapRule.RuleName));
                        }
                    }

                    counter--;
                }
            }
            finally
            {
                if (_currentTraceSettings.IsBasicTracingEnabled)
                {
                    _diagnosticActivity.LogInformation(String.Format("Finished reading [{0}] sheet.", ObjectType.DynamicGovernanceSorting.ToString()));
                    _diagnosticActivity.Stop();
                }
            }

            return mdmRuleMapRules;
        }

        private IBusinessRuleObjectCollection FillDDGLocaleMessages(DataTable dataTable)
        {
            LocaleMessageCollection localeMessages = new LocaleMessageCollection();
            Int32 counter = -2;

            try
            {
                if (_currentTraceSettings.IsBasicTracingEnabled)
                {
                    _diagnosticActivity = new DiagnosticActivity();
                    _diagnosticActivity.Start();
                    _diagnosticActivity.LogInformation(String.Format("Begin reading [{0}] sheet.", ObjectType.SystemMessages.ToString()));
                }

                foreach (DataRow dataRow in dataTable.Rows)
                {
                    ObjectAction objectAction = GetActionFromDataRow(dataRow);

                    if (objectAction == ObjectAction.Ignore)
                    {
                        continue;
                    }

                    LocaleEnum dataLocale = LocaleEnum.UnKnown;
                    String locale = dataRow[DDGDictionary.DDGLocaleMessageDictionary[DDGLocaleMessageEnum.Locale]].ToString().Trim();
                    ValueTypeHelper.EnumTryParse<LocaleEnum>(locale, true, out dataLocale);

                    MessageClassEnum messageClass = MessageClassEnum.UnKnown;
                    ValueTypeHelper.EnumTryParse(dataRow[DDGDictionary.DDGLocaleMessageDictionary[DDGLocaleMessageEnum.MessageType]].ToString().Trim(), true, out messageClass);

                    LocaleMessage localeMessage = new LocaleMessage()
                    {
                        ReferenceId = counter.ToString(),
                        Id = counter,
                        Action = objectAction,

                        MessageClass = messageClass,
                        Code = dataRow[DDGDictionary.DDGLocaleMessageDictionary[DDGLocaleMessageEnum.MessageCode]].ToString().Trim(),
                        Message = dataRow[DDGDictionary.DDGLocaleMessageDictionary[DDGLocaleMessageEnum.Message]].ToString().Trim(),
                        Description = dataRow[DDGDictionary.DDGLocaleMessageDictionary[DDGLocaleMessageEnum.Description]].ToString().Trim(),
                        WhereUsed = dataRow[DDGDictionary.DDGLocaleMessageDictionary[DDGLocaleMessageEnum.WhereUsed]].ToString().Trim(),
                        HelpLink = dataRow[DDGDictionary.DDGLocaleMessageDictionary[DDGLocaleMessageEnum.HelpfulLinks]].ToString().Trim(),
                        Locale = dataLocale
                    };

                    localeMessages.Add(localeMessage);
                    counter--;
                }

            }
            finally
            {
                if (_currentTraceSettings.IsBasicTracingEnabled)
                {
                    _diagnosticActivity.LogInformation(String.Format("Finished reading [{0}] sheet.", ObjectType.SystemMessages.ToString()));
                    _diagnosticActivity.Stop();
                }
            }

            return localeMessages;
        }

        private ObjectAction GetActionFromDataRow(DataRow dataRow)
        {
            //Set default object Action as Create.
            ObjectAction objectAction = ObjectAction.Create;

            String action = dataRow[DDGDictionary.ExportImportCommonHeadersDictionary[ExportImportCommonHeaders.Action]].ToString().Trim();

            if (!String.IsNullOrWhiteSpace(action))
            {
                Enum.TryParse(action, true, out objectAction);
            }

            return objectAction;
        }

        #endregion Private Methods

        #endregion Methods
    }
}
