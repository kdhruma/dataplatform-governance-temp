using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.Text.RegularExpressions;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace MDM.DataModelManager.Business
{
    using MDM.BufferManager;
    using MDM.BusinessObjects;
    using MDM.BusinessObjects.DataModel;
    using MDM.BusinessObjects.Diagnostics;
    using MDM.BusinessObjects.DynamicTableSchema;
    using MDM.ConfigurationManager.Business;
    using Data;
    using MDM.Core;
    using MDM.Core.Exceptions;
    using MDM.Interfaces;
    using MDM.KnowledgeManager.Business;
    using MDM.ActivityLogManager.Business;
    using MDM.MessageManager.Business;
    using MDM.Utility;

    /// <summary>
    /// Specifies Dynamic Table Schema class
    /// </summary>
    public sealed class DynamicTableSchemaBL : BusinessLogicBase, IDataModelManager
    {
        #region Fields

        /// <summary>
        /// Field denoting locale Message
        /// </summary>
        private LocaleMessage _localeMessage = new LocaleMessage();

        /// <summary>
        /// Field denoting localeMessageBL
        /// </summary>
        private LocaleMessageBL _localeMessageBL = new LocaleMessageBL();

        /// <summary>
        /// Field denoting utility object
        /// </summary>
        private Utility _utility = new Utility();

        /// <summary>
        /// Field denoting Dynamic Table Schema data access
        /// </summary>
        private DynamicTableSchemaDA _dynamicTableSchemaDA = new DynamicTableSchemaDA();

        /// <summary>
        /// Field denoting Replication class
        /// </summary>
        private ReplicationBL _replicationBL = new ReplicationBL();

        /// <summary>
        /// Field denoting operation context
        /// </summary>
        private OperationContext _operationContext = OperationContext.Current;

        /// <summary>
        /// Specifies lookup buffer manager
        /// </summary>
        private LookupBufferManager _lookupBufferManager = new LookupBufferManager();

        /// <summary>
        /// Specifies DynamicTableSchema BL
        /// </summary>
        private static DynamicTableSchemaBL _instance = null;

        /// <summary>
        /// Specifies lockObj
        /// </summary>
        private static Object lockObj = new Object();

        /// <summary>
        /// Specifies regular expression for validating tablename
        /// </summary>
        public static Regex regex = new Regex(@"^[a-zA-Zа-яА-ЯёЁ0-9_]{0,50}$", RegexOptions.Compiled);

        /// <summary>
        /// Specifies regular expression for validating column name of lookup
        /// </summary>
        public static Regex lookupColumnNameRegex = new Regex(@"^[a-zA-Zа-яА-ЯёЁ_][a-zA-Zа-яА-ЯёЁ0-9_]{0,49}$", RegexOptions.Compiled);

        /// <summary>
        /// Indicates current trace settings
        /// </summary>
        private TraceSettings _currentTraceSettings = new TraceSettings();

        #endregion

        #region Singleton Get

        /// <summary>
        /// Default Constructor
        /// </summary>
        public static DynamicTableSchemaBL GetSingleton()
        {
            if (_instance == null)
            {
                lock (lockObj)
                {
                    if (_instance == null)
                    {
                        _instance = new DynamicTableSchemaBL();
                    }
                }
            }

            return _instance;
        }

        #endregion

        #region Properties

        /// <summary>
        ///  Property denoting the ModuleId of the Replication
        /// </summary>
        public Int32 ModuleId
        {
            get
            {
                return Convert.ToInt32(MDMCenterModules.Export);
            }
        }

        /// <summary>
        ///  Property denoting the PhysicalServerSplit is Enabled or not.
        /// </summary>
        public Boolean IsServerSplitEnabled
        {
            get
            {
                return ValueTypeHelper.ConvertToBoolean(AppConfigurationHelper.GetAppConfig<String>("MDMCenter.PhysicalServerSplit.Enabled"));
            }
        }

        /// <summary>
        ///  Property denoting the login user.
        /// </summary>
        public String LoginUser
        {
            get
            {
                return SecurityPrincipalHelper.GetCurrentSecurityPrincipal().CurrentUserName;
            }
        }

        /// <summary>
        ///  Property denoting the program Name.
        /// </summary>
        public String ProgramName
        {
            get
            {
                return "MDM.DataModelManager.Business.DynamicTableSchemaBL.Process";
            }
        }

        #endregion

        #region Methods

        #region Public Methods

        #region Process Method

        /// <summary>
        /// Process table
        /// </summary>
        /// <param name="dbTable">This parameter is specifying instance of table to be processed</param>
        /// <param name="dynamicTableType">This parameter is specifying dynamic table type</param>
        /// <param name="callerContext">Name of application & Module which are performing action</param>
        /// <returns>Results of the operation having errors and information if any</returns>
        /// <exception cref="ArgumentNullException">If DBTable is null</exception>
        public OperationResult Process(DBTable dbTable, DynamicTableType dynamicTableType, CallerContext callerContext)
        {
            #region Step : Initial Setup

            #region Parameter validations

            if (dbTable == null)
                throw new ArgumentNullException("Table", "DBTable is null or empty");

            #endregion Parameter valudations

            #endregion

            return Process(new DBTableCollection() { dbTable }, dynamicTableType, callerContext);
        }

        /// <summary>
        /// Process Multiple tables
        /// </summary>
        /// <param name="dbTables">This parameter is specifying instance of tables to be processed</param>
        /// <param name="dynamicTableType">This parameter is specifying dynamic table type</param>
        /// <param name="callerContext">Name of application & Module which are performing action</param>
        /// <returns>Results of the operation having errors and information if any<</returns>
        /// <exception cref="ArgumentNullException">If DBTable Collection is null</exception>
        public OperationResult Process(DBTableCollection dbTables, DynamicTableType dynamicTableType, CallerContext callerContext)
        {
            OperationResult operationResult = new OperationResult();

            try
            {
                #region Step : Initial Setup

                DBCommandProperties command;

                #region Parameter validations

                if (dbTables == null || dbTables.Count < 0)
                {
                    throw new ArgumentNullException("Tables", "DBTables is null or empty");
                }

                #endregion Parameter valudations

                #endregion

                if (Constants.TRACING_ENABLED)
                {
                    MDMTraceHelper.StartTraceActivity("DataModelManager.DynamicSchemaBL.Process", false);
                }

                //Dynamic table processing must be syncronized..only 1 caller thread would be running process at time and others have to wait
                lock (lockObj)
                {
                    //Now for Lookup Globalization,whenever user is creating any new lookup at that time it will pass 2 table object 
                    // i.e. tblk_color and tblk_color_locale.
                    // API is always assume that your first table is always master table i.e. nothing but tblk_color table.
                    DBTable dbTable = dbTables.FirstOrDefault();

                    if (dbTable.Action == ObjectAction.Create)
                    {
                        command = DBCommandHelper.Get(callerContext.Application, callerContext.Module, MDMCenterModuleAction.Create);

                        operationResult = Create(dbTables, dynamicTableType, operationResult, command);
                    }
                    else if (dbTable.Action == ObjectAction.Update)
                    {
                        command = DBCommandHelper.Get(callerContext.Application, callerContext.Module, MDMCenterModuleAction.Update);

                        operationResult = Update(dbTables, dynamicTableType, operationResult, command);
                    }
                    else if (dbTable.Action == ObjectAction.Delete)
                    {
                        command = DBCommandHelper.Get(callerContext.Application, callerContext.Module, MDMCenterModuleAction.Delete);

                        operationResult = Delete(dbTables, dynamicTableType, operationResult, command);
                    }

                    #region activity log

                    if (operationResult.OperationResultStatus == OperationResultStatusEnum.Successful && AppConfigurationHelper.GetAppConfig<Boolean>(Constants.ACTIVITYLOG_LOOKUPVALUES_ENABLED, false))
                    {
                        LogDataModelChanges(dbTables, callerContext);
                }

                    #endregion activity log

            }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                LocalizeErrors(callerContext, operationResult);
                if (Constants.TRACING_ENABLED)
                {
                    MDMTraceHelper.StopTraceActivity("DataModelManager.DynamicSchemaBL.Process");
            }
            }

            return operationResult;
        }

        #endregion

        #region Get Method

        /// <summary>
        /// Get All Tables for given Table Type
        /// </summary>
        /// <param name="dynamicTableType">This parameter is specifying dynamic Table type.</param>
        /// <param name="callerContext">Name of application & Module which are performing action</param>
        /// <param name="loadInternalColumns">Indicates whether to get internal columns of lookup or not.</param>
        /// <returns>DBTable</returns>
        public DBTableCollection GetAll(DynamicTableType dynamicTableType, CallerContext callerContext, Boolean loadInternalColumns = true)
        {
            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StartTraceActivity("DataModelManager.DynamicSchemaBL.GetAll", false);

            DBTableCollection dbTables = null;

            try
            {
                DBCommandProperties command = DBCommandHelper.Get(callerContext.Application, callerContext.Module, MDMCenterModuleAction.Read);
                dbTables = _dynamicTableSchemaDA.Get(String.Empty, dynamicTableType, command, loadInternalColumns);
            }
            finally
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StopTraceActivity("DataModelManager.DynamicSchemaBL.GetAll");
            }

            return dbTables;
        }

        /// <summary>
        /// Get lookup tables based on given lookup table names
        /// </summary>
        /// <param name="lookupTableNames">Indicates which lookup tables needs to get</param>
        /// <param name="callerContext">Name of application & Module which are performing action</param>
        /// <param name="loadInternalColumns">Indicates whether to get internal columns of lookup or not.</param>
        /// <returns>DBTable by given lookup table names</returns>
        public DBTableCollection GetLookupsByNames(Collection<String> lookupTableNames, CallerContext callerContext, Boolean loadInternalColumns = true)
        {
            _currentTraceSettings = MDMOperationContextHelper.GetCurrentTraceSettings();
            var diagnosticActivity = new DiagnosticActivity();

            DBTableCollection dbTables = new DBTableCollection();

            try
            {
                if (_currentTraceSettings.IsBasicTracingEnabled)
                {
                    diagnosticActivity.Start();
                }

                if (lookupTableNames == null || lookupTableNames.Count < 1)
                {
                    _localeMessage = _localeMessageBL.Get(GlobalizationHelper.GetSystemUILocale(), "113960", new Object[] { "Lookup table names" }, false, callerContext);
                    diagnosticActivity.LogError("113960", _localeMessage.Message);
                    throw new MDMOperationException("113960", _localeMessage.Message, "DynamicTableSchemaBL.GetLookupsByNames", String.Empty, "Get");
                }
                else
                {
                    DBCommandProperties command = DBCommandHelper.Get(callerContext.Application, callerContext.Module, MDMCenterModuleAction.Read);
                    DBTableCollection allDbTables = _dynamicTableSchemaDA.Get(String.Empty, DynamicTableType.Lookup, command, loadInternalColumns);

                    if (allDbTables != null && allDbTables.Count > 0)
                    {
                        foreach (DBTable dbTable in allDbTables)
                        {
                            if (lookupTableNames.Contains(dbTable.Name))
                            {
                                dbTables.Add(dbTable);
                            }
                        }
                    }
                }
            }
            finally
            {
                if (_currentTraceSettings.IsBasicTracingEnabled)
                {
                    diagnosticActivity.Stop();
                }
            }

            return dbTables;
        }

        /// <summary>
        /// Get Meta of Complex Table & pass to process method for process
        /// </summary>
        /// <param name="id">>This parameter is specifying complex Attribute Id.</param>
        /// <param name="dynamicTableType">>This parameter is specifying dynamic Table type.</param>
        /// <param name="callerContext">Name of application & Module which are performing action</param>
        /// <returns>Results of the operation having errors and information if any</returns>
        /// <exception cref="ArgumentNullException">If Complex Attribute Id is less than 0</exception>
        public OperationResult Get(Int32 id, DynamicTableType dynamicTableType, CallerContext callerContext)
        {
            OperationResult operationResult = new OperationResult();

            try
            {
                #region Step : Initial Setup

                #region Parameter validations

                if (id < 0)
                {
                    throw new ArgumentNullException("ComplexAttributeId", "ComplexAttributeId is null or empty");
                }

                #endregion Parameter valudations

                DBTable dbTable = new DBTable();
                #endregion

                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StartTraceActivity("DataModelManager.DynamicSchemaBL.Get", false);

                DBCommandProperties command = DBCommandHelper.Get(callerContext.Application, callerContext.Module, MDMCenterModuleAction.Read);
                dbTable = _dynamicTableSchemaDA.Get(id, command);

                if (dynamicTableType == DynamicTableType.Complex)
                {
                    dbTable.AttributeId = id;
                }

                operationResult = Process(dbTable, dynamicTableType, callerContext);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StopTraceActivity("DataModelManager.DynamicSchemaBL.Get");
            }

            return operationResult;
        }

        #endregion

        #region IDataModelManager Methods

        /// <summary>
        /// Generate and OperationResult Schema for given DataModelObjectCollection
        /// </summary>
        /// <param name="iDataModelObjects"></param>
        /// <param name="operationResult">Collection of DataModel OperationResult</param>
        public void PrepareOperationResultsSchema(IDataModelObjectCollection iDataModelObjects, DataModelOperationResultCollection operationResults)
        {
            try
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StartTraceActivity("DynamicTableSchemaBL.PrepareOperationResultsSchema", MDMTraceSource.DataModel, false);

                DBTableCollection dBTables = iDataModelObjects as DBTableCollection;

                if (dBTables != null && dBTables.Count > 0)
                {
                    if (operationResults.Count > 0)
                    {
                        operationResults.Clear();
                    }

                    Int32 dBTableIdToBeCreated = -1;

                    foreach (DBTable dBTable in dBTables)
                    {
                        DataModelOperationResult dbTableOperationResult = new DataModelOperationResult(dBTable.Id, dBTable.Name, dBTable.ExternalId, dBTable.ReferenceId);

                        if (String.IsNullOrWhiteSpace(dbTableOperationResult.ExternalId))
                        {
                            dbTableOperationResult.ExternalId = dBTable.Name;
                        }

                        if (dBTable.Id < 1)
                        {
                            dBTable.Id = dBTableIdToBeCreated;
                            dbTableOperationResult.Id = dBTableIdToBeCreated;
                            dBTableIdToBeCreated--;
                        }

                        operationResults.Add(dbTableOperationResult);
                    }
                }
            }
            finally
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StopTraceActivity("DynamicTableSchemaBL.PrepareOperationResultsSchema", MDMTraceSource.DataModel);
            }
        }

        /// <summary>
        /// Validates DataModelObjects and populate OperationResults
        /// </summary>
        /// <param name="iDataModelObjects">Collection of data model objects to be filled up.</param>
        /// <param name="operationResult">Collection of DataModel OperationResult</param>
        /// <returns>DataModel Operation Result Collection</returns>
        public void Validate(IDataModelObjectCollection iDataModelObjects, DataModelOperationResultCollection operationResults, ICallerContext iCallerContext)
        {
            try
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StartTraceActivity("DynamicTableSchemaBL.Validate", MDMTraceSource.DataModel, false);

                DBTableCollection dBTables = iDataModelObjects as DBTableCollection;
                CallerContext callerContext = iCallerContext as CallerContext;

                #region Parameter Validations

                ValidateInputParameters(dBTables, operationResults, callerContext);

                #endregion
            }
            finally
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StopTraceActivity("DynamicTableSchemaBL.Validate", MDMTraceSource.DataModel);
            }
        }

        /// <summary>
        /// Load original data model objects
        /// </summary>
        /// <param name="iDataModelObjects">Collection of data model objects to load original object.</param>
        /// <param name="operationResult">Collection of DataModel OperationResult</param>
        /// <param name="iCallerContext">Context of caller making call to this API.</param>
        public void LoadOriginal(IDataModelObjectCollection iDataModelObjects, DataModelOperationResultCollection operationResults, ICallerContext iCallerContext)
        {
            try
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StartTraceActivity("DynamicTableSchemaBL.LoadOriginal", MDMTraceSource.DataModel, false);

                LoadOriginalDBTables(iDataModelObjects as DBTableCollection, iCallerContext as CallerContext);
            }
            finally
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StopTraceActivity("DynamicTableSchemaBL.LoadOriginal", MDMTraceSource.DataModel);
            }

        }

        /// <summary>
        /// Fills missing information to data model objects.
        /// </summary>
        /// <param name="iDataModelObjects">Collection of data model objects to be filled up.</param>
        /// <param name="iCallerContext">Context of caller making call to this API.</param>
        public void FillDataModel(IDataModelObjectCollection iDataModelObjects, ICallerContext iCallerContext)
        {
            try
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StartTraceActivity("DynamicTableSchemaBL.FillDataModel", MDMTraceSource.DataModel, false);

                FillDBTables(iDataModelObjects as DBTableCollection, iCallerContext as CallerContext);
            }
            finally
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StopTraceActivity("DynamicTableSchemaBL.FillDataModel", MDMTraceSource.DataModel);
            }
        }
        
        /// <summary>
        /// Compare and merge data model object collection with current collection
        /// </summary>
        /// <param name="iDataModelObjects">Collection of data model objects to be compare and merge.</param>
        /// <param name="operationResult">Collection of DataModel OperationResult</param>
        /// <param name="iCallerContext">Context of caller making call to this API.</param>
        /// <returns>Returns merged data model objects.</returns>
        public void CompareAndMerge(IDataModelObjectCollection iDataModelObjects, DataModelOperationResultCollection operationResults, ICallerContext iCallerContext)
        {
            try
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StartTraceActivity("DynamicTableSchemaBL.CompareAndMerge", MDMTraceSource.DataModel, false);

                CompareAndMerge(iDataModelObjects as DBTableCollection, operationResults, iCallerContext as CallerContext);
            }
            finally
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StopTraceActivity("DynamicTableSchemaBL.CompareAndMerge", MDMTraceSource.DataModel);
            }
        }

        /// <summary>
        /// Process data model collection
        /// </summary>
        /// <param name="iDataModelObjects">Collection of data model objects to be processed.</param>
        /// <param name="operationResult">Collection of DataModel OperationResult</param>
        /// <param name="iCallerContext">Context of caller making call to this API.</param>
        /// <returns>Result of operation</returns>
        public void Process(IDataModelObjectCollection iDataModelObjects, DataModelOperationResultCollection operationResults, ICallerContext iCallerContext)
        {
            try
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StartTraceActivity("DynamicTableSchemaBL.Process", MDMTraceSource.DataModel, false);

                DBTableCollection dbTables = iDataModelObjects as DBTableCollection;
                CallerContext callerContext = iCallerContext as CallerContext;

                DBCommandProperties command = DBCommandHelper.Get(callerContext.Application, callerContext.Module, MDMCenterModuleAction.Create);

                if (dbTables.Count > 0)
                {
                    #region Perform DBTable updates in database

                    using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions() { IsolationLevel = Constants.DEFAULT_ISOLATION_LEVEL }))
                    {
                        Dictionary<DynamicTableType, DBTableCollection> dynamicTableTypeBasedDictionary = AddInternalColumns(dbTables, callerContext);

                        _dynamicTableSchemaDA.Process(dynamicTableTypeBasedDictionary, operationResults, LoginUser, ProgramName, command);

                        transactionScope.Complete();
                    }

                    LocalizeErrors(callerContext, operationResults);

                    #endregion
                }
            }
            finally
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StopTraceActivity("DynamicTableSchemaBL.Process", MDMTraceSource.DataModel);
            }
        }

        ///<summary>
        /// Processes the Entity cache statuses for data model objects
        /// </summary>
        /// <param name="iDataModelObjects">Collection of data model objects to be process entity cache load.</param>
        /// <param name="operationResult">Collection of DataModel OperationResult</param>
        /// <param name="iCallerContext">Context of caller making call to this API.</param>
        public void InvalidateCache(IDataModelObjectCollection iDataModelObjects, DataModelOperationResultCollection operationResults, ICallerContext iCallerContext)
        {
            DBTableCollection dbTables = iDataModelObjects as DBTableCollection;

            if (dbTables != null && dbTables.Count > 0)
            {
                LookupCollection lookups = new LookupCollection();
                LocaleEnum systemDataLocale = GlobalizationHelper.GetSystemDataLocale();

                foreach (DBTable dbTable in dbTables)
                {
                    if (dbTable.Action == ObjectAction.Read || dbTable.Action == ObjectAction.Ignore && dbTable.Name.EndsWith("_Locale"))
                        continue;

                    Lookup lookup = new Lookup();
                    lookup.Name = dbTable.Name.Replace("tblk_", String.Empty);
                    lookup.Locale = systemDataLocale;
                    lookup.Action = dbTable.Action;

                    lookups.Add(lookup);
                }

                _lookupBufferManager.UpdateDirtyLookupObjectWebServerListCache(lookups);

                //Set thread operation context
                OperationContext.Current = _operationContext;

                //Invalidate cache for impacted Data
                Task.Factory.StartNew(() =>
                {
                    LocaleBL _localeBL = new LocaleBL();
                    LocaleCollection localeCollection = _localeBL.GetAvailableLocales();
                    _lookupBufferManager.InvalidateImpactedData(lookups, localeCollection);
                });
            }
        }
        
        #endregion

        #endregion

        #region Private Methods

        #region Validation Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dBTables"></param>
        /// <param name="callerContext"></param>
        private void ValidateInputParameters(DBTableCollection dBTables, CallerContext callerContext)
        {
            String errorMessage;

            if (dBTables == null || dBTables.Count < 1)
            {
                errorMessage = "DBTable collection is not available or empty";
                throw new MDMOperationException("", errorMessage, "DataModelManager.DynamicTableSchemaBL", String.Empty, "Create");
            }

            if (callerContext == null)
            {
                errorMessage = "CallerContext cannot be null.";
                throw new MDMOperationException("111846", "CallerContext cannot be null.", "DataModelManager.DynamicTableSchemaBL", String.Empty, "Create");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dBTables"></param>
        /// <param name="operationResults"></param>
        /// <param name="callerContext"></param>
        private void ValidateInputParameters(DBTableCollection dBTables, DataModelOperationResultCollection operationResults, CallerContext callerContext)
        {
            String errorMessage = String.Empty;
            IDataModelOperationResult operationResult;
            LocaleEnum systemUILocale = GlobalizationHelper.GetSystemUILocale();
            
            if (dBTables == null || dBTables.Count < 1)
            {
                DataModelHelper.AddOperationResults(operationResults, "113613", String.Empty, null, OperationResultType.Error, TraceEventType.Error, callerContext);
            }
            else
            {
                foreach (DBTable dbTable in dBTables)
                {
                    operationResult = operationResults.GetByReferenceId(dbTable.ReferenceId);
                    HashSet<String> lookupColumnList = new HashSet<String>();

                    Int32 uniqueColumnsCount = 0;
                    StringBuilder optionalUniqueColumns = new StringBuilder();

                    if (String.IsNullOrWhiteSpace(dbTable.Name))
                    {
                        DataModelHelper.AddOperationResult(operationResult, "111022", String.Empty, null, OperationResultType.Error, TraceEventType.Error, callerContext);
                    }
                    else if (!regex.IsMatch(dbTable.Name))
                    {
                        DataModelHelper.AddOperationResult(operationResult, "111217", "Possible Errors : 1. Table Name should be < 50 chars OR 2. Only [Alphabets], [Numerics] and _ [Underscore] are allowed in Table Name OR 3. Table Name should not contain spaces.", null, OperationResultType.Error, TraceEventType.Error, callerContext);
                    }

                    if (dbTable.Columns != null && dbTable.Columns.Count > 0)
                    {
                        foreach (DBColumn column in dbTable.Columns)
                        {
                            if (String.IsNullOrWhiteSpace(column.Name))
                            {
                                DataModelHelper.AddOperationResult(operationResult, "112657", String.Empty, null, OperationResultType.Error, TraceEventType.Error, callerContext);
                            }
                            else if (!lookupColumnNameRegex.IsMatch(column.Name))
                            {
                                DataModelHelper.AddOperationResult(operationResult, "111219", String.Format("Check whether the column name '{0}' in the lookup table '{1}' is exceeding more than 50 characters or having a special character, other than 'Alphabets and _ [Underscore]'.", column.Name, dbTable.Name),
                                            new Object[] { column.Name, dbTable.Name }, OperationResultType.Error, TraceEventType.Error, callerContext);
                            }
                            else
                            {
                                String columnName = column.NameInLowerCase;

                                if (!lookupColumnList.Contains(columnName))
                            {
                                    lookupColumnList.Add(columnName);
                            }
                                else
                                {
                                    DataModelHelper.AddOperationResult(operationResult, "114003", "Duplicate column(s): {0} found for lookup table: {1}.", new Object[] { column.Name, dbTable.Name }, OperationResultType.Error, TraceEventType.Error, callerContext);
                                }
                            }
                                
                            //In case of String data type, if the column length (Width) is negative then it means it is MAX width. 
                            //So in that case ignore the validation for String. 
                            //In other datatype (bit or integer)it is not considering the length itself. 
                            if (column.DataType == AttributeDataType.String.ToString() && column.Length == 0)
                            {
                                DataModelHelper.AddOperationResult(operationResult, "113696", "Width cannot be less than 1 for column: {0} of lookup table: {1}", new Object[] { column.Name, dbTable.Name }, OperationResultType.Error, TraceEventType.Error, callerContext);
                            }

                            if (column.DataType != AttributeDataType.String.ToString() && column.DataType != AttributeDataType.Integer.ToString() && column.DataType != AttributeDataType.Boolean.ToString())
                            {
                                DataModelHelper.AddOperationResult(operationResult, "113697", "Data type is not valid for column: {0} of lookup table: {1}", new Object[] { column.Name, dbTable.Name }, OperationResultType.Error, TraceEventType.Error, callerContext);
                            }

                            if (column.IsUnique)
                            {
                                if (column.Nullable)
                                {
                                    optionalUniqueColumns.Append(column.Name + ",");
                        }

                                uniqueColumnsCount++;
                    }
                }
            }
                
                    if (optionalUniqueColumns.Length > 0)
                    {
                        String optionalUniqueColumnsAsString = optionalUniqueColumns.ToString();

                        optionalUniqueColumnsAsString = optionalUniqueColumnsAsString.Remove(optionalUniqueColumnsAsString.LastIndexOf(","), 1);

                        Int32 optionalUniqueColumnsCount = optionalUniqueColumnsAsString.Split(',').Length;

                        if (uniqueColumnsCount == optionalUniqueColumnsCount)
                        {
                            DataModelHelper.AddOperationResult(operationResult, "114077", "Create/Update of record(s) in a LookupTable '{0}' fails if unqiue column(s) '{1}' that are marked as nullable have empty values.", new Object[] { dbTable.Name, optionalUniqueColumnsAsString }, OperationResultType.Information, TraceEventType.Information, callerContext);
                        }
                    }
                }
            }

            if (callerContext == null)
            {
                DataModelHelper.AddOperationResults(operationResults, "111846", String.Empty, null, OperationResultType.Error, TraceEventType.Error, callerContext);
            }

        }

        #endregion

        #region Process Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbTables"></param>
        /// <param name="dynamicTableType"></param>
        /// <param name="operationResult"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        private OperationResult Create(DBTableCollection dbTables, DynamicTableType dynamicTableType, OperationResult operationResult, DBCommandProperties command)
        {
            #region Step : Call Dynamic Table Schema Process

            Boolean processResult = false;

            operationResult = _dynamicTableSchemaDA.Process(dbTables, dynamicTableType, this.LoginUser, this.ProgramName, command);

            if (!operationResult.HasError)
            {
                processResult = true;
            }

            #endregion

            String replicationResult = String.Empty;

            if (this.IsServerSplitEnabled)
            {
                #region Step : Include Table into Replication

                if (processResult)
                {
                    // No need to pass DBCommand Properties,it will always run from publisher so it will automatically take default connection string.
                    replicationResult = _replicationBL.ModifyArticle(GetTableNames(dbTables), dynamicTableType, ReplicationType.Include, this.ModuleId);

                    if (String.IsNullOrEmpty(replicationResult))
                    {
                        Error error = new Error();
                        error.ErrorMessage = "Dynamic Schema is failed due to replication,please contact your administrator.";
                        operationResult.Errors.Add(error);
                    }
                }

                #endregion

                #region Step : Add Job into the Distributor

                if (!String.IsNullOrEmpty(replicationResult))
                    _replicationBL.AddJobToDistributor(replicationResult, new CallerContext(MDMCenterApplication.PIM, MDMCenterModules.Distributor));

                #endregion
            }

            #region Step : Drop Table if Replication is failed

            if (this.IsServerSplitEnabled && String.IsNullOrEmpty(replicationResult))
            {
                SetTableActionAsDelete(dbTables);
                _dynamicTableSchemaDA.Process(dbTables, dynamicTableType, this.LoginUser, this.ProgramName, command);
            }

            #endregion

            return operationResult;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbTables"></param>
        /// <param name="dynamicTableType"></param>
        /// <param name="operationResult"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        private OperationResult Update(DBTableCollection dbTables, DynamicTableType dynamicTableType, OperationResult operationResult, DBCommandProperties command)
        {
            operationResult = new OperationResult();

            using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required, BusinessLogicBase.GetTransactionOptions(ProcessingMode.Sync)))
            {
            #region Step : Call Dynamic Table Schema Process

            //Replication is not allowing to update table schema,if API is under transaction scope.
            operationResult = _dynamicTableSchemaDA.Process(dbTables, dynamicTableType, this.LoginUser, this.ProgramName, command);

            #endregion

            #region Step: Cache Invalidation of Lookup Data

            if (dynamicTableType == DynamicTableType.Lookup &&
               (operationResult.OperationResultStatus == OperationResultStatusEnum.Successful || operationResult.OperationResultStatus == OperationResultStatusEnum.None))
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Removing Lookup Impacted Data from Cache started.....");

                //Invalidate cache for impacted Data
                Task.Factory.StartNew(() => { InvalidateImpactedData(dbTables); });

                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Removing Lookup Impacted Data from Cache completed.");
            }

            #endregion

                #region Cache Invalidation of Complex Table Update

                if (dynamicTableType == DynamicTableType.Complex && dbTables.Count > 0 &&
                    (operationResult.OperationResultStatus == OperationResultStatusEnum.Successful || operationResult.OperationResultStatus == OperationResultStatusEnum.None))
                {
                    DBTable dbTable = dbTables.FirstOrDefault();

                    CacheInvalidationOfComplexTable(dbTable.AttributeId);
                }

                #endregion Cache Invalidation of Complex Table

                if (!operationResult.HasError)
                {
                    transactionScope.Complete();
                }
            }

            return operationResult;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbTables"></param>
        /// <param name="dynamicTableType"></param>
        /// <param name="operationResult"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        private OperationResult Delete(DBTableCollection dbTables, DynamicTableType dynamicTableType, OperationResult operationResult, DBCommandProperties command)
        {
            operationResult = new OperationResult();
            String removeResult = String.Empty;

            #region Step : Remove Table from Replication

            if (this.IsServerSplitEnabled)
            {
                // No need to pass DBCommand Properties,it will always run from publisher so it will automatically take default connection string.
                removeResult = _replicationBL.ModifyArticle(GetTableNames(dbTables), dynamicTableType, ReplicationType.Remove, this.ModuleId);

                if (String.IsNullOrEmpty(removeResult))
                {
                    Error error = new Error();
                    error.ErrorMessage = "Dynamic Schema is failed due to replication,please contact your administrator.";
                    operationResult.Errors.Add(error);
                }
            }

            #endregion

            using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required, BusinessLogicBase.GetTransactionOptions(ProcessingMode.Sync)))
            {
                #region Step : Call Dynamic Table Schema Process from Publisher

                if (!String.IsNullOrEmpty(removeResult))
                {
                    operationResult = _dynamicTableSchemaDA.Process(dbTables, dynamicTableType, this.LoginUser, this.ProgramName, command);
                }

                #endregion

                #region Step : Call Dynamic Table Schema Process from Subscriber

                if (this.IsServerSplitEnabled)
                {
                    if (!operationResult.HasError)
                    {
                        IsPopulateRSTObject(dbTables);
                        DBCommandProperties dbCommand = DBCommandHelper.Get(MDMCenterApplication.PIM, MDMCenterModules.SubscriberUIProcess, MDMCenterModuleAction.Delete);
                        operationResult = _dynamicTableSchemaDA.Process(dbTables, dynamicTableType, this.LoginUser, this.ProgramName, dbCommand);
                    }
                }

                #endregion

                #region Cache Invalidation of Complex Table Update

                if (dynamicTableType == DynamicTableType.Complex && dbTables.Count > 0 &&
                    (operationResult.OperationResultStatus == OperationResultStatusEnum.Successful || operationResult.OperationResultStatus == OperationResultStatusEnum.None))
                {
                    DBTable dbTable = dbTables.FirstOrDefault();
                    CacheInvalidationOfComplexTable(dbTable.AttributeId);
                }

                #endregion Cache Invalidation of Complex Table

                if (!operationResult.HasError)
                {
                    transactionScope.Complete();
                }
            }

            if (this.IsServerSplitEnabled)
            {
                if (operationResult.HasError)
                {
                    #region Step: Include Original Table into Replication

                    // No need to pass DBCommand Properties,it will always run from publisher so it will automatically take default connection string.
                    _replicationBL.ModifyArticle(GetTableNames(dbTables), dynamicTableType, ReplicationType.Include, this.ModuleId);

                    #endregion
                }

                #region Step : Add Job into the Distributor

                if (!String.IsNullOrEmpty(removeResult))
                    _replicationBL.AddJobToDistributor(removeResult, new CallerContext(MDMCenterApplication.PIM, MDMCenterModules.Distributor));

                #endregion
            }
            else
            {
                if (operationResult.HasError == false)
                {
                    operationResult = _dynamicTableSchemaDA.Process(dbTables, dynamicTableType, this.LoginUser, this.ProgramName, command);
                }
            }

            #region Step: Cache Invalidation of Lookup Data

            if (dynamicTableType == DynamicTableType.Lookup &&
               (operationResult.OperationResultStatus == OperationResultStatusEnum.Successful || operationResult.OperationResultStatus == OperationResultStatusEnum.None))
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Removing Lookup Impacted Data from Cache started.....");

                //Invalidate cache for impacted Data
                Task.Factory.StartNew(() => { InvalidateImpactedData(dbTables); });

                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Removing Lookup Impacted Data from Cache completed.");
            }

            #endregion

            return operationResult;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dBTables"></param>
        /// <param name="operationResults"></param>
        /// <param name="callerContext"></param>
        private void CompareAndMerge(DBTableCollection dBTables, DataModelOperationResultCollection operationResults, CallerContext callerContext)
        {
            foreach (DBTable deltaDBTable in dBTables)
            {
                IDataModelOperationResult operationResult = operationResults.GetByReferenceId(deltaDBTable.ReferenceId);

                //If delta object Action is Read/Ignore then skip Merge Delta Process.
                if (deltaDBTable.Action == ObjectAction.Read || deltaDBTable.Action == ObjectAction.Ignore)
                    continue;

                IDBTable originalDBTable = deltaDBTable.OriginalDBTable;

                if (originalDBTable != null)
                {
                    //If delta object Action is Delete then skip Merge Delta process.
                    if (deltaDBTable.Action != ObjectAction.Delete)
                    {
                        originalDBTable.MergeDelta(deltaDBTable, callerContext, false);
                        if ((deltaDBTable.Action == ObjectAction.Update) && 
                            (deltaDBTable.DynamicTableType == DynamicTableType.Lookup || deltaDBTable.DynamicTableType == DynamicTableType.LocaleLookup))
                        {
                            ValidateTableChange(deltaDBTable, deltaDBTable.DynamicTableType, (DataModelOperationResult) operationResult);
                        }
                    }
                }
                else
                {
                    if (deltaDBTable.Action == ObjectAction.Delete)
                    {
                        DataModelHelper.AddOperationResult(operationResult, "113615", String.Empty, new Object[] { deltaDBTable.Name }, OperationResultType.Error, TraceEventType.Error, callerContext);
                    }
                    else
                    {
                        //If original object is not found then set Action as Create always.
                        deltaDBTable.Action = ObjectAction.Create;
                    }
                }
                operationResult.PerformedAction = deltaDBTable.Action;
            }
        }

        #endregion
        
        #region Get Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dBTables"></param>
        /// <param name="callerContext"></param>
        private void LoadOriginalDBTables(DBTableCollection dbTables, CallerContext callerContext)
        {
            DBTableCollection originalDBTables = GetAll(DynamicTableType.Unknown, callerContext, false);

            foreach (DBTable dbTable in dbTables)
            {
                dbTable.OriginalDBTable = originalDBTables.Get(dbTable.Name);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dBTables"></param>
        /// <param name="callerContext"></param>
        private void FillDBTables(DBTableCollection dBTables, CallerContext callerContext)
        {
            foreach (DBTable dbTable in dBTables)
            {
                if (dbTable.OriginalDBTable != null)
                {
                    dbTable.Id = dbTable.OriginalDBTable.Id;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dynamicTableType"></param>
        /// <param name="callerContext"></param>
        /// <returns></returns>
        private DBColumnCollection GetDefaultTemplate(DynamicTableType dynamicTableType, CallerContext callerContext)
        {
            DBCommandProperties command = DBCommandHelper.Get(callerContext.Application, callerContext.Module, MDMCenterModuleAction.Read);
            return _dynamicTableSchemaDA.GetDefaultTemplate(dynamicTableType, command);
        }
        
        #endregion

        #region Misc. Methods

        /// <summary>
        ///
        /// </summary>
        /// <param name="dbTables"></param>
        private Collection<String> GetTableNames(DBTableCollection dbTables)
        {
            Collection<String> names = null;

            IEnumerable<String> iEnumerableNames = dbTables.Select(dbtable => dbtable.Name);

            if (iEnumerableNames.Count() > 0)
            {
                names = new Collection<String>(iEnumerableNames.ToList());
            }

            return names;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="dbTables"></param>
        private void SetTableActionAsDelete(DBTableCollection dbTables)
        {
            foreach (DBTable dbTable in dbTables)
            {
                dbTable.Action = ObjectAction.Delete;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="dbTables"></param>
        private void IsPopulateRSTObject(DBTableCollection dbTables)
        {
            foreach (DBTable dbTable in dbTables)
            {
                dbTable.PopulateRSTObject = false;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="dbTables"></param>
        private void InvalidateImpactedData(DBTableCollection dbTables)
        {
            try
            {
                //Now for Lookup Globalization,whenever user is creating any new lookup at that time it will pass 2 table object 
                // i.e. tblk_color and tblk_color_locale.
                // API is always assume that your first table is always master table i.e. nothing but tblk_color table.
                DBTable dbTable = dbTables.FirstOrDefault();

                Lookup lookup = new Lookup();
                //We are caching lookup data with lookup prefix i.e.tblk_. so before invalidating cache, I am removing prefix.
                lookup.Name = dbTable.Name.Substring(5);
                lookup.Locale = MDM.Utility.GlobalizationHelper.GetSystemDataLocale();
                lookup.Action = dbTable.Action;

                //Set thread operation context
                OperationContext.Current = _operationContext;

                LocaleBL _localeBL = new LocaleBL();
                LocaleCollection localeCollection = _localeBL.GetAvailableLocales();
                _lookupBufferManager.InvalidateImpactedData(new LookupCollection() { lookup }, localeCollection);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// if there are any error codes coming in OR from db, populate error messages for them
        /// </summary>
        /// <param name="callerContext">CallerContext</param>
        /// <param name="operationResults">DataModelOperationResultCollection</param>
        private void LocalizeErrors(CallerContext callerContext, DataModelOperationResultCollection operationResults)
        {
            foreach (DataModelOperationResult operationResult in operationResults)
            {
                LocalizeErrors(callerContext, operationResult as OperationResult);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbDataType"></param>
        /// <returns></returns>
        private String GetDataType(String dbDataType)
        {
            String dataType = String.Empty;

            switch (dbDataType.ToLowerInvariant())
            {
                case "nvarchar":
                    dataType = "String";
                    break;
                case "decimal":
                    dataType = "Decimal";
                    break;
                case "bit":
                    dataType = "Boolean";
                    break;
                case "int":
                    dataType = "Integer";
                    break;
                case "bigint":
                    dataType = "Long";
                    break;
                case "smalldatetime":
                case "datetime":
                    dataType = "DateTime";
                    break;
            }
            return dataType;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbTables"></param>
        /// <param name="dynamicTableType"></param>
        /// <param name="operationResult"></param>
        /// <param name="command"></param>
        private void ValidateTableChange(DBTable dbTable, DynamicTableType dynamicTableType, DataModelOperationResult operationResult)
        {
            if (dbTable != null && dbTable.OriginalDBTable != null)
            {
                DBColumnCollection originalDbColumnCollection = dbTable.OriginalDBTable.Columns;

                if (originalDbColumnCollection != null && originalDbColumnCollection.Count > 0)
                {
                    if (dbTable.Columns != null && dbTable.Columns.Count > 0)
                    {
                        foreach (DBColumn dbColumn in dbTable.Columns)
                        {
                            DBColumn originalDBColumn = originalDbColumnCollection.Get(dbColumn.Name);

                            if (originalDBColumn != null)
                            {
                                // Here system try to check, whether user has applied unique constraint on the column which was previously non-unique
                                // If Yes, then add warning in operationresult
                                if (dbColumn.IsUnique && !originalDBColumn.IsUnique)
                                {
                                    String warningMessage = String.Format("Avoid marking '{0}' as 'Unqiue', else existing records may become invalid, if any.", dbColumn.Name);
                                    operationResult.AddOperationResult("114128", warningMessage, OperationResultType.Warning);
                                }
                            }
                        }
                    }
                }
            }
        }

        #endregion

        #region Utility Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbTables"></param>
        /// <param name="callerContext"></param>
        /// <returns></returns>
        private Dictionary<DynamicTableType, DBTableCollection> AddInternalColumns(DBTableCollection dbTables, CallerContext callerContext)
        {
            Dictionary<DynamicTableType, DBTableCollection> dynamicTableTypeBasedDictionary = new Dictionary<DynamicTableType, DBTableCollection>();
            DBTableCollection lookupTables = new DBTableCollection();
            DBTableCollection complexTables = new DBTableCollection();

            Dictionary<DynamicTableType, DBColumnCollection> dynamicTableTypeBasedDefaultTemplateDictionary = new Dictionary<DynamicTableType, DBColumnCollection>();

            if (dbTables != null && dbTables.Count > 0)
            {
                foreach (DBTable dbTable in dbTables)
                {
                    if (dbTable.Action == ObjectAction.Read || dbTable.Action == ObjectAction.Ignore)
                        continue;

                    if (!dbTable.Name.StartsWith("tblk_"))
                    {
                        dbTable.Name = String.Concat("tblk_", dbTable.Name);
                    }

                    DBColumnCollection dbColumns = null;
                    DBTable clonedTableForLocale = (dbTable.DynamicTableType == DynamicTableType.Lookup) ? dbTable.Clone() as DBTable : null;

                    if (dbTable.Action == ObjectAction.Create)
                    {
                        dynamicTableTypeBasedDefaultTemplateDictionary.TryGetValue(dbTable.DynamicTableType, out dbColumns);

                        if (dbColumns == null || dbColumns.Count < 1)
                        {
                            dbColumns = GetDefaultTemplate(dbTable.DynamicTableType, callerContext);

                            if (dbTable.DynamicTableType == DynamicTableType.Lookup)
                            {
                                RemovePredefinedColumnsFromTemplateForLookup(dbColumns);
                            }

                            SetActionAsCreate(dbColumns);
                            dynamicTableTypeBasedDefaultTemplateDictionary.Add(dbTable.DynamicTableType, dbColumns);
                        }

                        dbTable.Columns.AddRange(dbColumns);

                        if (dbTable.DynamicTableType == DynamicTableType.Lookup)
                        {
                            dbColumns = null;

                            dynamicTableTypeBasedDefaultTemplateDictionary.TryGetValue(DynamicTableType.LocaleLookup, out dbColumns);

                            if (dbColumns == null || dbColumns.Count < 1)
                            {
                                dbColumns = GetDefaultTemplate(DynamicTableType.LocaleLookup, callerContext);
                                SetActionAsCreate(dbColumns);
                                dynamicTableTypeBasedDefaultTemplateDictionary.Add(DynamicTableType.LocaleLookup, dbColumns);
                            }

                            clonedTableForLocale.Columns.AddRange(dbColumns);
                            
                        }
                    }

                    if (dbTable.DynamicTableType == DynamicTableType.Lookup)
                    {
                        dbTable.PopulateRSTObject = true;
                        lookupTables.Add(dbTable);

                        clonedTableForLocale.Name = String.Concat(clonedTableForLocale.Name, "_Locale");
                        clonedTableForLocale.PopulateRSTObject = false;
                        lookupTables.Add(clonedTableForLocale);
                    }
                    else if (dbTable.DynamicTableType == DynamicTableType.Complex)
                    {
                        complexTables.Add(dbTable);
                    }
                }

                dynamicTableTypeBasedDictionary.Add(DynamicTableType.Lookup, lookupTables);
                dynamicTableTypeBasedDictionary.Add(DynamicTableType.Complex, complexTables);
            }

            return dynamicTableTypeBasedDictionary;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbColumns"></param>
        private void SetActionAsCreate(DBColumnCollection dbColumns)
        {
            foreach (DBColumn dbColumn in dbColumns)
            {
                dbColumn.Action = ObjectAction.Create;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbColumns"></param>
        /// <returns></returns>
        private Boolean RemovePredefinedColumnsFromTemplateForLookup(DBColumnCollection dbColumns)
        {
            return dbColumns.Remove(new Collection<String>() { "LookupKey", "Value", "Code" });
        }

        private void CacheInvalidationOfComplexTable(Int32 complexAttributeId)
        {
            if (complexAttributeId > 0)
            {
                EntityCacheLoadContextItemCollection entityCacheLoadContextItemCollection = new EntityCacheLoadContextItemCollection();

                EntityCacheLoadContextItem entityCacheLoadContextItemForAttributeModel =
                    entityCacheLoadContextItemCollection.GetOrCreateEntityCacheLoadContextItem(EntityCacheLoadContextTypeEnum.Attribute);
                entityCacheLoadContextItemForAttributeModel.AddValues(complexAttributeId);

                EntityCacheLoadContext entityCacheLoadContext = new EntityCacheLoadContext();
                entityCacheLoadContext.CacheStatus = (Int32)(EntityCacheComponentEnum.InheritedAttributes | EntityCacheComponentEnum.OverriddenAttributes);
                entityCacheLoadContext.Add(entityCacheLoadContextItemCollection);

                String entityCacheLoadContextAsString = entityCacheLoadContext.ToXml();

                EntityActivityLog entityActivityLog = new EntityActivityLog()
                {
                    PerformedAction = EntityActivityList.EntityCacheLoad,
                    Context = entityCacheLoadContextAsString
                };

                EntityActivityLogCollection entityActivityLogCollection = new EntityActivityLogCollection() { entityActivityLog };
                CallerContext callerContext = new CallerContext(MDMCenterApplication.PIM, MDMCenterModules.AttributeGeneration);

                EntityActivityLogBL entityActivityLogBL = new EntityActivityLogBL();
                entityActivityLogBL.Process(entityActivityLogCollection, callerContext);
            }
        }

        private void LocalizeErrors(CallerContext callerContext, OperationResult operationResult)
        {
            foreach (Error error in operationResult.Errors)
            {
                if (!String.IsNullOrEmpty(error.ErrorCode))
                {
                    _localeMessage = _localeMessageBL.Get(GlobalizationHelper.GetSystemUILocale(), error.ErrorCode, false, callerContext);

                    if (_localeMessage != null)
                    {
                        error.ErrorMessage = _localeMessage.Message;
                    }
                }
            }
        }

        /// <summary>
        /// Log changes to tb_datamodelactivitylog if DA call is sucessfull and appconfig key datamodelactivitylog.enable == true
        /// </summary>
        /// <param name="dbTables"></param>
        /// <param name="callerContext"></param>
        private void LogDataModelChanges(DBTableCollection dbTables, CallerContext callerContext)
        {
            #region step: populate datamodelactivitylog object

            DataModelActivityLogBL dataModelActivityLogManager = new DataModelActivityLogBL();
            DataModelActivityLogCollection activityLogCollection = dataModelActivityLogManager.FillDataModelActivityLogCollection(dbTables);

            #endregion step: populate datamodelactivitylog object

            #region step: make api call

            if (activityLogCollection != null) // null activitylog collection means there was error in mapping
            {
                dataModelActivityLogManager.Process(activityLogCollection, callerContext);
            }

            #endregion step: make api call

        }

        #endregion

        #endregion

        #endregion
    }
}