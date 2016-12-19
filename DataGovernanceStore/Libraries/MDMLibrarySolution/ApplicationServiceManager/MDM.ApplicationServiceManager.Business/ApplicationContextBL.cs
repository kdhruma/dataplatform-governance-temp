using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Transactions;

namespace MDM.ApplicationServiceManager.Business
{
    using MDM.ApplicationServiceManager.Data;
    using MDM.BufferManager;
    using MDM.BusinessObjects;
    using MDM.BusinessObjects.Diagnostics;
    using MDM.ConfigurationManager.Business;
    using MDM.Core;
    using MDM.Core.Exceptions;
    using MDM.MessageManager.Business;
    using MDM.Utility;

    /// <summary>
    /// 
    /// </summary>
    public class ApplicationContextBL : BusinessLogicBase
    {
        #region Fields

        /// <summary>
        /// Field denoting the security principal
        /// </summary>
        private SecurityPrincipal _securityPrincipal;

        /// <summary>
        /// Data access layer for entity types
        /// </summary>
        private ApplicationContextDA _applicationContextDA = new ApplicationContextDA();

        /// <summary>
        /// Field denoting locale Message
        /// </summary>
        private LocaleMessage _localeMessage;

        /// <summary>
        /// Field denoting localeMessageBL
        /// </summary>
        private LocaleMessageBL _localeMessageBL = new LocaleMessageBL();

        /// <summary>
        /// Indicates the diagnostic activity
        /// </summary>
        private DiagnosticActivity _diagnosticActivity = null;

        /// <summary>
        /// Indicates whether tracing is eneabled or not
        /// </summary>
        private Boolean isTracingEnabled = false;

        /// <summary>
        /// Indicates the current trace setting 
        /// </summary>
        private TraceSettings _currentTraceSettings = null;




        #endregion

        #region Constructors

        /// <summary>
        /// 
        /// </summary>
        public ApplicationContextBL()
        {
            GetSecurityPrincipal();

            _currentTraceSettings = MDMOperationContextHelper.GetCurrentTraceSettings();
            isTracingEnabled = _currentTraceSettings.IsBasicTracingEnabled;
            _diagnosticActivity = new DiagnosticActivity();
        }

        #endregion

        #region Public Methods

        #region Get

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ApplicationContextId"></param>
        /// <param name="contextType"></param>
        /// <returns></returns>
        public Collection<ApplicationContext> GetApplicationContext(Int32 ApplicationContextId, ApplicationContextType contextType)
        {
            Collection<ApplicationContext> applicationContexts = new Collection<ApplicationContext>();
            ApplicationContextDA _applicationContextDA = new ApplicationContextDA();

            applicationContexts = _applicationContextDA.GetApplicationContext(ApplicationContextId, (int)contextType);

            return applicationContexts;
        }

        /// <summary>
        /// Returns all the application contexts available in the system
        /// </summary>
        /// <param name="callerContext">Indicates the name of the Application and Module which perform the action</param>
        /// <returns>Collection of ApplicationContexts</returns>
        public ApplicationContextCollection Get(CallerContext callerContext)
        {
            ApplicationContextCollection applicationContexts = new ApplicationContextCollection();

            try
            {
                #region Initialize And Start Diagnostic

                if (_currentTraceSettings.IsBasicTracingEnabled)
                {
                    _diagnosticActivity.Start();
                    _diagnosticActivity.LogInformation("Loading all ApplicationContexts available in the system.");
                }

                #endregion Initialize And Start Diagnostic

                #region Load ApplicationContexts From Cache

                if (_currentTraceSettings.IsBasicTracingEnabled)
                {
                    _diagnosticActivity.LogInformation("Loading ApplicationContexts From Cache.");
                }

                var bufferManager = new CacheBufferManager<ApplicationContextCollection>(CacheKeyGenerator.GetApplicationContextsCacheKey(), String.Empty);

                applicationContexts = bufferManager.GetAllObjectsFromCache();

                if (_currentTraceSettings.IsBasicTracingEnabled)
                {
                    _diagnosticActivity.LogInformation("Loaded ApplicationContexts From Cache.");
                }

                #endregion Load ApplicationContexts From Cache

                #region Load ApplicationContext From DB

                if (applicationContexts == null || applicationContexts.Count < 1)
                {
                    if (callerContext == null)
                    {
                        _diagnosticActivity.LogError("111846", "CallerContext cannot be null.");
                        throw new MDMOperationException("111846", "CallerContext cannot be null.", "MDM.ApplicationServiceManager.Business", String.Empty, "Get");
                    }

                    if (_currentTraceSettings.IsTracingEnabled)
                    {
                        _diagnosticActivity.LogInformation("Loading ApplicationContexts from Database.");
                    }

                    DBCommandProperties command = DBCommandHelper.Get(callerContext, MDMCenterModuleAction.Read);

                    ApplicationContextDA applicationContextDA = new ApplicationContextDA();
                    applicationContexts = applicationContextDA.Get(0, command);

                    #region Update ApplicationContexts in Cache

                    if (applicationContexts != null && applicationContexts.Count > 0)
                    {
                        if (_currentTraceSettings.IsTracingEnabled)
                        {
                            _diagnosticActivity.LogData("Loaded ApplicationContexts from Database.", applicationContexts.ToXml());
                        }

                        bufferManager.SetBusinessObjectsToCache(applicationContexts, 10);

                        if (_currentTraceSettings.IsBasicTracingEnabled)
                        {
                            _diagnosticActivity.LogInformation("ApplicationContexts updated to Cache.");
                        }
                    }
                    else
                    {
                        if (_currentTraceSettings.IsTracingEnabled)
                        {
                            _diagnosticActivity.LogInformation("There is no records for ApplicationContexts are available in the Database.");
                        }
                    }

                    #endregion Update ApplicationContexts in Cache
                }

                #endregion Load ApplicationContext From DB
            }
            finally
            {
                if (_currentTraceSettings.IsBasicTracingEnabled)
                {
                    _diagnosticActivity.Stop();
                }
            }

            return applicationContexts;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ApplicationContextId"></param>
        /// <param name="callerContext"></param>
        /// <returns></returns>
        public ApplicationContext GetById(Int32 ApplicationContextId, CallerContext callerContext)
        {
            MDMTraceHelper.InitializeTraceSource();
            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StartTraceActivity("ApplicationContextBL.GetById", false);

            #region Validation

            if (ApplicationContextId < 1)
            {
                String errorMessage = "ApplicationContext Id cannot be less than 1";
                throw new MDMOperationException("112383", errorMessage, "ApplicationServiceManager.ApplicationContextBL", String.Empty, "Get");
            }

            if (callerContext == null)
            {
                String errorMessage = "CallerContext cannot be null.";
                throw new MDMOperationException("111846", errorMessage, "ApplicationServiceManager.ApplicationContextBL", String.Empty, "Get");
            }

            #endregion Validation

            ApplicationContext applicationContext = null;

            try
            {
                ApplicationContextCollection applicationContexts = Get(callerContext);
                applicationContext = applicationContexts.GetApplicationContext(ApplicationContextId) as ApplicationContext;
            }
            finally
            {
                if (Constants.TRACING_ENABLED)
                {
                    MDMTraceHelper.StopTraceActivity("ApplicationContextBL.GetById");
                }
            }

            return applicationContext;
        }

        /// <summary>
        /// Gets the BEST match object id for a given entity, application context type and object ids
        /// </summary>
        /// <param name="appContextType"></param>
        /// <param name="entity"></param>
        /// <param name="callerContext"></param>
        /// <param name="objectIds"></param>
        /// <returns></returns>
        public Int32 GetApplicationContextObjectId(Collection<Int32> objectIds, ApplicationContextType appContextType, Entity entity, CallerContext callerContext)
        {
            MDMTraceHelper.InitializeTraceSource();
            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StartTraceActivity("ApplicationContextBL.GetApplicationContextObjectId", false);

            #region Validation

            if (objectIds.Count <= 0)
            {
                const string errorMessage = "Object Ids collection cannot be empty";
                throw new MDMOperationException("112383", errorMessage, "ApplicationServiceManager.ApplicationContextBL", String.Empty, "Get");
            }

            if (callerContext == null)
            {
                const string errorMessage = "CallerContext cannot be null.";
                throw new MDMOperationException("111846", errorMessage, "ApplicationServiceManager.ApplicationContextBL", String.Empty, "Get");
            }

            #endregion Validation

            Int32 objectId = 0;

            try
            {
                DBCommandProperties command = DBCommandHelper.Get(callerContext, MDMCenterModuleAction.Read);

                objectId = new ApplicationContextDA().GetApplicationContextObjectId(objectIds, appContextType, entity, command);
            }
            finally
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StopTraceActivity("ApplicationContextBL.GetApplicationContextObjectId");
            }

            return objectId;
        }

        /// <summary>
        /// Get applicationContexId based on attribute Id and context.
        /// </summary>
        /// <param name="applicatonContext">Indicates the Application context</param>
        /// <param name="getExactMatch">Whether want exact match or not</param>
        /// <returns>Application context Id</returns>
        public Int32 GetApplicationContextId(ApplicationContext applicatonContext, Boolean getExactMatch = false)
        {
            Int32 applicationContextID = 0;
            Collection<ApplicationContext> applicationContexts = new Collection<ApplicationContext>();
            ApplicationContextDA _applicationContextDA = new ApplicationContextDA();

            applicationContextID = _applicationContextDA.GetApplicationContextId(applicatonContext, getExactMatch);

            return applicationContextID;
        }

        /// <summary>
        /// Gets the application context ids from application context collection object and match context
        /// </summary>
        /// <param name="applicationContexts">Indicates the application contexts</param>
        /// <param name="matchContext">Indicates the match context</param>
        /// <param name="callerContext">Indicates the caller context</param>
        /// <returns>Indicates the list of application context Ids</returns>
        public Dictionary<Int64, Collection<Int32>> GetApplicationContextIds(ApplicationContextCollection applicationContexts, ApplicationContextMatchType matchContext, CallerContext callerContext)
        {
            Dictionary<Int64, Collection<Int32>> applicationContextIds = null;
            DiagnosticActivity diagnosticActivity = new DiagnosticActivity();

            try
            {
                if (_currentTraceSettings.IsBasicTracingEnabled)
                {
                    diagnosticActivity.Start();
                }

                if (callerContext == null)
                {
                    throw new MDMOperationException("113907", "CallerContext cannot be null.", "ApplicationServiceManager.ApplicationContextBL", String.Empty, "GetApplicationContextIds", new Object[] { "CallerContext" });
                }
                else if (applicationContexts == null)
                {
                    throw new MDMOperationException("113907", "applicationContext cannot be null.", "ApplicationServiceManager.ApplicationContextBL", String.Empty, "GetApplicationContextIds", new Object[] { "applicationContext" });
                }

                if (_currentTraceSettings.IsBasicTracingEnabled)
                {
                    diagnosticActivity.LogData("Input ApplicationContext :", applicationContexts.ToXml());
                    diagnosticActivity.LogData("Input Match Context :", matchContext.ToString());
                }

                DBCommandProperties dbcommand = DBCommandHelper.Get(callerContext.Application, callerContext.Module, MDMCenterModuleAction.Read);
                ApplicationContextDA applicationContextDA = new ApplicationContextDA();
                applicationContextIds = applicationContextDA.GetApplicationContextIds(applicationContexts , matchContext, dbcommand);
            }
            finally
            {
                if (_currentTraceSettings.IsBasicTracingEnabled)
                {
                    if (applicationContextIds == null || applicationContextIds.Count == 0)
                    {
                        diagnosticActivity.LogInformation("There is no possible application context match found.");
                    }
                    else
                    {
                        foreach (var result in applicationContextIds)
                        {
                            diagnosticActivity.LogInformation(String.Format("Reference Id: {0} -  Resulted application context Ids are {1}:", result.Key, ValueTypeHelper.JoinCollection<Int32>(result.Value, ",")));
                        }
                    }
                    diagnosticActivity.Stop();
                }
            }

            return applicationContextIds;
        }


        /// <summary>
        /// Get application context object mappings based on given object type and match context
        /// </summary>
        /// <param name="applicationContexts">Indicates collection of application context.</param>
        /// <param name="objectTypeId">Indicates object type identifier.</param>
        /// <param name="matchContext">Indicates match context.</param>
        /// <param name="callerContext">Indicates caller of the API specifying application and module name.</param>
        /// <returns>Returns application context object mapping based on given object type and match context.</returns>
        public ApplicationContextObjectMappingCollection GetApplicationContextObjectMappings(ApplicationContextCollection applicationContexts, Int32 objectTypeId, ApplicationContextMatchType matchContext, CallerContext callerContext)
        {
            ApplicationContextObjectMappingCollection applicationContextObjectMappings = null;
            DiagnosticActivity diagnosticActivity = new DiagnosticActivity();

            try
            {
                if (isTracingEnabled)
                {
                    diagnosticActivity.Start();
                    diagnosticActivity.LogInformation(String.Format("Input Object Type Id :{0} and Match Context :{1}", objectTypeId, matchContext.ToString()));
                }

                DBCommandProperties dbcommand = DBCommandHelper.Get(callerContext.Application, callerContext.Module, MDMCenterModuleAction.Read);
                ApplicationContextDA applicationContextDA = new ApplicationContextDA();
                applicationContextObjectMappings = applicationContextDA.GetApplicationContextObjectMappings(applicationContexts, objectTypeId, matchContext, dbcommand);
            }
            finally
            {
                if (isTracingEnabled)
                {
                    diagnosticActivity.LogMessageWithData("See 'View Data' for loaded application context object mappings : -", applicationContextObjectMappings.ToXml());
                    diagnosticActivity.Stop();
                }
            }

            return applicationContextObjectMappings;
        }

        #endregion Get

        #region CUD

        /// <summary>
        /// 
        /// </summary>
        /// <param name="applicationContext"></param>
        /// <param name="callerContext"></param>
        /// <returns></returns>
        public OperationResult Create(ApplicationContext applicationContext, CallerContext callerContext)
        {
            ValidateInputParameters(applicationContext, callerContext);
            applicationContext.Action = ObjectAction.Create;

            return this.Process(applicationContext, callerContext);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="applicationContext"></param>
        /// <param name="callerContext"></param>
        /// <returns></returns>
        public OperationResult Update(ApplicationContext applicationContext, CallerContext callerContext)
        {
            ValidateInputParameters(applicationContext, callerContext);
            applicationContext.Action = ObjectAction.Update;

            return this.Process(applicationContext, callerContext);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="applicationContext"></param>
        /// <param name="callerContext"></param>
        /// <returns></returns>
        public OperationResult Delete(ApplicationContext applicationContext, CallerContext callerContext)
        {
            ValidateInputParameters(applicationContext, callerContext);
            applicationContext.Action = ObjectAction.Delete;

            return this.Process(applicationContext, callerContext);
        }

        /// <summary>
        /// Create - Update or Delete given application context
        /// </summary>
        /// <param name="applicationContexts">ApplicationContext collection to process</param>
        /// <param name="callerContext">Context which called the application</param>
        /// <returns>Result of the operation</returns>
        public OperationResultCollection Process(ApplicationContextCollection applicationContexts, CallerContext callerContext)
        {
            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "CallerContext : Application - " + callerContext.Application + " Module - " + callerContext.Module, MDMTraceSource.DataModel);

            OperationResultCollection contextProcessOperationResult = new OperationResultCollection();

            #region Validation

            if (applicationContexts == null)
            {
                throw new MDMOperationException("112382", "Application Context cannot be null.", "ApplicationContextBL.Process", String.Empty, "Process");
            }

            if (callerContext == null)
            {
                throw new MDMOperationException("111846", "CallerContext cannot be null.", "ApplicationContextBL.JobBL", String.Empty, "Process");
            }

            #endregion Validation

            String userName = String.Empty;

            userName = PopulateUserName();

            PopulateProgramName(callerContext);

            LocaleEnum systemDataLocale = GlobalizationHelper.GetSystemDataLocale();

            try
            {
                DBCommandProperties command = DBCommandHelper.Get(callerContext, MDMCenterModuleAction.Execute);

                contextProcessOperationResult = _applicationContextDA.Process(applicationContexts, callerContext.ProgramName, userName, systemDataLocale, command);

                LocalizeErrors(callerContext, contextProcessOperationResult);

                if (contextProcessOperationResult.OperationResultStatus != OperationResultStatusEnum.Failed)
                {
                    var bufferManager = new CacheBufferManager<ApplicationContextCollection>(CacheKeyGenerator.GetApplicationContextsCacheKey(), String.Empty);
                    bufferManager.RemoveBusinessObjectFromCache(publishCacheChangeEvent: true);
                }
            }
            catch (Exception ex)
            {
                throw new MDMOperationException(ex.Message, ex);
            }

            return contextProcessOperationResult;
        }

        /// <summary>
        /// Create - Update given application context
        /// </summary>
        /// <param name="applicationContexts">ApplicationContext collection to process</param>
        /// <param name="callerContext">Indicates the name of the Application and Module which invoked the API</param>
        /// <returns>Result of the operation</returns>
        public OperationResultCollection CreateAndGetApplicationContexts(ApplicationContextCollection applicationContexts, CallerContext callerContext)
        {
            OperationResultCollection applicationContextOperationResults = new OperationResultCollection();

            ApplicationContextCollection uniqueApplicationContexts = new ApplicationContextCollection();
            ApplicationContextCollection duplicateApplicationContexts = new ApplicationContextCollection();

            try
            {
                #region Initialize And Start Diagnostic

                if (_currentTraceSettings.IsTracingEnabled)
                {
                    _diagnosticActivity.Start();
                    _diagnosticActivity.LogInformation(String.Format("ApplicationContextBL.Process - CallerContext : Application '{0}', Module '{1}' ", callerContext.Application.ToString(), callerContext.Module.ToString()));
                }

                #endregion Initialize And Start Diagnostic

                #region Validate InputParameters

                ValidateInputParameters(applicationContexts, callerContext, applicationContextOperationResults);

                if (applicationContextOperationResults.OperationResultStatus == OperationResultStatusEnum.Failed)
                {
                    return applicationContextOperationResults;
                }

                #endregion Validate InputParameters

                #region Populate ShortNames and Create Seperate Buckets for Unique And Duplicate ApplicationContexts

                if (_currentTraceSettings.IsTracingEnabled)
                {
                    _diagnosticActivity.LogError("Populating ShortNames and Create Seperate Buckets for Unique And Duplicate ApplicationContexts.");
                }

                // Note: This method populate shortname for application context and prepare seperate buckets for unique application context 
                // and duplicate application context
                // e.g. From 10 records if 3 records have the same shortname then 1 will be added into uniqueapplicationcontext bucket and other 2 will be populated 
                // into duplicateapplicationcontext bucket. Total: uniqueapplicationcontext bucket = 8, duplicateapplicationcontext bucket = 2
                PrepareApplicationContext(applicationContexts, uniqueApplicationContexts, duplicateApplicationContexts);

                if (_currentTraceSettings.IsTracingEnabled)
                {
                    _diagnosticActivity.LogError("Populated ShortNames and Create Seperate Buckets for Unique And Duplicate ApplicationContexts.");
                }

                #endregion Populate ShortNames and Create Seperate Buckets for Unique And Duplicate ApplicationContexts

                #region Filter ApplicationContexts For Create

                if (_currentTraceSettings.IsTracingEnabled)
                {
                    _diagnosticActivity.LogError("Filtering Application Contexts eligible for Create.");
                }

                // Note: This method will process uniqueapplicationcontextbucket and checks whether the applicationcontext is available in the system or not.
                // If the application is available then it will populate the details and remove it from futhur processing
                // e.g. Out of 8 if 2 records already exists then it will remove those records from futhur processing. Total: uniqueapplicationcontext bucket = 6
                FilterApplicationContextsForCreate(uniqueApplicationContexts, applicationContextOperationResults, callerContext);

                if (_currentTraceSettings.IsTracingEnabled)
                {
                    _diagnosticActivity.LogError("Filtered Application Contexts eligible for Create.");
                }

                #endregion Filter ApplicationContexts For Create

                #region Process ApplicationContexts

                // If no records are available for furthur then processing stop futhur processing.
                if (uniqueApplicationContexts != null && uniqueApplicationContexts.Count < 1)
                {
                    if (_currentTraceSettings.IsTracingEnabled)
                    {
                        _diagnosticActivity.LogInformation(String.Format("'{0}' Application Contexts are available for Create.", uniqueApplicationContexts.Count));
                    }

                    // Note: This method populates the duplicate applicationcontext with details available in database
                    PopulateDuplicateRecords(uniqueApplicationContexts, duplicateApplicationContexts, applicationContextOperationResults, callerContext);

                    return applicationContextOperationResults;
                }

                if (_currentTraceSettings.IsTracingEnabled)
                {
                    _diagnosticActivity.LogInformation("Creating Application Contexts.");
                }

                // Note: This method will send the eligible applicationcontext for furthur processing (e.g. Create)
                OperationResultCollection applicationContextOperationResultsAfterProcess = Process(uniqueApplicationContexts, callerContext);

                applicationContextOperationResults.AddRange(applicationContextOperationResultsAfterProcess);

                if (_currentTraceSettings.IsTracingEnabled)
                {
                    _diagnosticActivity.LogData("Created Application Contexts.", applicationContexts.ToXml());
                }

                #endregion Process ApplicationContexts

                #region Populate Duplicate ApplicationContexts

                if (_currentTraceSettings.IsTracingEnabled)
                {
                    _diagnosticActivity.LogInformation("Updating duplicate Application Contexts.");
                }

                // Note: This method populates the duplicate applicationcontext bucket with details available in database
                // e.g. If 3 records are eligible for Create then 1 will be sent for processing (as above) and 2 records which are duplicate
                // for them the information will be populated using below method
                PopulateDuplicateRecords(uniqueApplicationContexts, duplicateApplicationContexts, applicationContextOperationResults, callerContext);

                if (_currentTraceSettings.IsTracingEnabled)
                {
                    _diagnosticActivity.LogInformation("Updated duplicate Application Contexts.");
                }

                #endregion Populate Duplicate ApplicationContexts
            }
            finally
            {
                #region Stop Diagnostic

                if (_currentTraceSettings.IsTracingEnabled)
                {
                    _diagnosticActivity.Stop();
                }

                #endregion Stop Diagnostic
            }

            return applicationContextOperationResults;
        }

        /// <summary>
        /// Create - Update or Delete given application context object mappings
        /// </summary>
        /// <param name="applicationContextObjectMappings">Indicates ApplicationContextObjectMapping collection to process</param>
        /// <param name="callerContext">Indicates context which called the application</param>
        /// <returns>Returns result of the operation</returns>
        public OperationResultCollection ProcessObjectMapping(ApplicationContextObjectMappingCollection applicationContextObjectMappings, CallerContext callerContext)
        {

            OperationResultCollection contextObjectMappingProcessOperationResult = new OperationResultCollection();

            #region Initialize And Start Diagnostic

            if (_currentTraceSettings.IsTracingEnabled)
            {
                _diagnosticActivity.Start();
                _diagnosticActivity.LogInformation(String.Format("ApplicationContextBL.ProcessObjectMapping - CallerContext : Application '{0}', Module '{1}' ", callerContext.Application.ToString(), callerContext.Module.ToString()));
            }

            #endregion Initialize And Start Diagnostic

            #region Validation

            if (applicationContextObjectMappings == null)
            {
                throw new MDMOperationException("", "Application Context Object mapping cannot be null.", "ApplicationContextBL.ProcessObjectMapping", String.Empty, "ProcessObjectMapping");
            }

            if (callerContext == null)
            {
                throw new MDMOperationException("111846", "CallerContext cannot be null.", "ApplicationContextBL.JobBL", String.Empty, "ProcessObjectMapping");
            }

            #endregion Validation

            String userName = String.Empty;

            userName = PopulateUserName();

            PopulateProgramName(callerContext);

            LocaleEnum systemDataLocale = GlobalizationHelper.GetSystemDataLocale();

            try
            {
                using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions() { IsolationLevel = Constants.DEFAULT_ISOLATION_LEVEL }))
                {
                    DBCommandProperties command = DBCommandHelper.Get(callerContext, MDMCenterModuleAction.Execute);

                    contextObjectMappingProcessOperationResult = _applicationContextDA.ProcessObjectMapping(applicationContextObjectMappings, callerContext.ProgramName, userName, systemDataLocale, command);

                    LocalizeErrors(callerContext, contextObjectMappingProcessOperationResult);

                    transactionScope.Complete();
                }

            }
            catch (Exception ex)
            {
                throw new MDMOperationException(ex.Message, ex);
            }
            finally
            {
                #region Stop Diagnostic

                if (_currentTraceSettings.IsTracingEnabled)
                {
                    _diagnosticActivity.LogMessageWithData("See 'View Data' for Operation result of Created application context object mappings.", contextObjectMappingProcessOperationResult.ToXml());
                    _diagnosticActivity.Stop();
                }

                #endregion Stop Diagnostic
            }

            return contextObjectMappingProcessOperationResult;

        }

        #endregion CUD

        #endregion Public Methods

        #region Private Methods

        #region Get And Process

        private void PrepareApplicationContext(ApplicationContextCollection applicationContexts, ApplicationContextCollection uniqueApplicationContexts, ApplicationContextCollection duplicateApplicationContexts)
        {
            HashSet<String> uniqueApplicationContextSet = new HashSet<String>();

            if (applicationContexts != null && applicationContexts.Count > 0)
            {
                Int32 applicationContextToBeCreated = -1;

                foreach (ApplicationContext applicationContext in applicationContexts)
                {
                    applicationContext.Id = applicationContextToBeCreated;
                    applicationContext.Name = String.Format(Constants.APPLICATION_CONTEXT_NAME,
                                         applicationContext.OrganizationId, applicationContext.ContainerId, applicationContext.CategoryId,
                                         applicationContext.EntityTypeId, applicationContext.EntityId, applicationContext.AttributeId,
                                         applicationContext.RelationshipTypeId, applicationContext.RoleId, applicationContext.UserId, applicationContext.ObjectTypeId);
                    applicationContext.LongName = applicationContext.Name;

                    applicationContextToBeCreated--;

                    if (!uniqueApplicationContextSet.Add(applicationContext.Name))
                    {
                        duplicateApplicationContexts.Add(applicationContext);
                    }
                    else
                    {
                        uniqueApplicationContexts.Add(applicationContext);
                    }
                }
            }
        }

        private void ValidateInputParameters(ApplicationContextCollection applicationContexts, CallerContext callerContext, OperationResultCollection applicationContextOperationResults)
        {
            if (callerContext == null)
            {
                if (_currentTraceSettings.IsTracingEnabled)
                {
                    _diagnosticActivity.LogError("CallerContext cannot be null.");
                }

                OperationResult operationResult = new OperationResult();
                operationResult.AddOperationResult("111846", "CallerContext cannot be null.", OperationResultType.Error);

                applicationContextOperationResults.Add(operationResult);
            }

            PopulateProgramName(callerContext);

            if (applicationContexts == null || applicationContexts.Count < 1)
            {
                if (_currentTraceSettings.IsTracingEnabled)
                {
                    _diagnosticActivity.LogError("ApplicationContextCollection cannot be null.");
                }

                OperationResult operationResult = new OperationResult();
                operationResult.AddOperationResult("113907", "ApplicationContextCollection cannot be null.", new Object[] { "ApplicationContextCollection" }, OperationResultType.Error);

                applicationContextOperationResults.Add(operationResult);
            }

            applicationContextOperationResults.RefreshOperationResultStatus();
        }

        private void FilterApplicationContextsForCreate(ApplicationContextCollection applicationContexts, OperationResultCollection applicationContextOperationResults, CallerContext callerContext)
        {
            Collection<Int32> applicationContextsToRemove = CompareMergeAndCalculateActions(applicationContexts, applicationContextOperationResults, callerContext);

            foreach (Int32 applicationContextId in applicationContextsToRemove)
            {
                applicationContexts.Remove(applicationContextId);
            }
        }

        private void PopulateDuplicateRecords(ApplicationContextCollection uniqueApplicationContexts, ApplicationContextCollection duplicateApplicationContexts, OperationResultCollection duplicateApplicationContextOperationResults, CallerContext callerContext)
        {
            if (duplicateApplicationContexts != null && duplicateApplicationContexts.Count > 0)
            {
                CompareMergeAndCalculateActions(duplicateApplicationContexts, duplicateApplicationContextOperationResults, callerContext);
            }
        }

        private Collection<Int32> CompareMergeAndCalculateActions(ApplicationContextCollection applicationContexts, OperationResultCollection applicationContextOperationResults, CallerContext callerContext)
        {
            Collection<Int32> applicationContextsToRemove = new Collection<Int32>();

            ApplicationContextCollection originalApplicationContexts = Get(callerContext);

            foreach (ApplicationContext deltaApplicationContext in applicationContexts)
            {
                OperationResult applicationContextOperationResult = new OperationResult();

                if (deltaApplicationContext.Action == ObjectAction.Exclude || deltaApplicationContext.Action == ObjectAction.Ignore)
                {
                    // Todo.. Locale Message
                    applicationContextOperationResult.AddOperationResult("-1", String.Format("Application Context removed from processing because of invalid action", deltaApplicationContext.Name), OperationResultType.Information);
                    continue;
                }

                ApplicationContext originalApplicationContext = null;

                if (originalApplicationContexts != null)
                {
                    originalApplicationContext = originalApplicationContexts.GetApplicationContext(deltaApplicationContext.Name);
                }

                if (originalApplicationContext != null)
                {
                    deltaApplicationContext.Id = originalApplicationContext.Id;
                    deltaApplicationContext.Name = originalApplicationContext.Name;
                    deltaApplicationContext.LongName = originalApplicationContext.LongName;

                    applicationContextOperationResult.Id = originalApplicationContext.Id;
                    applicationContextOperationResult.ReferenceId = deltaApplicationContext.ReferenceId.ToString(); // Caller must pass unique reference id
                    applicationContextOperationResult.ReturnValues.Add(deltaApplicationContext.Id);
                    applicationContextOperationResult.AddOperationResult("-1", String.Format("Application Context: {0} already exists and hence removed from Processing", deltaApplicationContext.Name), OperationResultType.Information);
                    applicationContextOperationResult.OperationResultStatus = OperationResultStatusEnum.Successful;
                    applicationContextOperationResult.PerformedAction = ObjectAction.Read;

                    applicationContextOperationResults.Add(applicationContextOperationResult);

                    applicationContextsToRemove.Add(deltaApplicationContext.Id);
                }
                else
                {
                    if (deltaApplicationContext.Action == ObjectAction.Update || deltaApplicationContext.Action == ObjectAction.Delete)
                    {
                        // Todo.. Locale Message
                        applicationContextOperationResult.AddOperationResult("-99", "'{0}' does not exist and, therefore, could not be deleted.", new Object[] { deltaApplicationContext.Name }, OperationResultType.Error);
                        applicationContextOperationResults.Add(applicationContextOperationResult);
                    }
                    else
                    {
                        deltaApplicationContext.Action = ObjectAction.Create;
                    }
                }
            }

            return applicationContextsToRemove;
        }

        private OperationResult Process(ApplicationContext applicationContext, CallerContext callerContext)
        {
            OperationResult or = new OperationResult();
            if (applicationContext != null)
            {
                OperationResultCollection orc = this.Process(new ApplicationContextCollection { applicationContext }, callerContext);
                if (orc != null)
                {
                    or = orc.FirstOrDefault();
                }
            }
            return or;
        }

        private void PopulateApplicationContextIdsForCreate(ApplicationContextCollection applicationContexts)
        {
            Int32 applicationContextCreateCount = 0;

            foreach (ApplicationContext applicationContext in applicationContexts)
            {
                if (applicationContext.Action == ObjectAction.Create)
                {
                    applicationContext.Id = applicationContextCreateCount--;
                }
            }
        }

        private void PopulateOperationResult(ApplicationContext applicationContext, OperationResult applicationContextProcessOperationResult)
        {
            if (applicationContext.Action == ObjectAction.Create)
            {
                if (applicationContextProcessOperationResult.ReturnValues.Any())
                {
                    applicationContextProcessOperationResult.Id =
                        ValueTypeHelper.Int32TryParse(applicationContextProcessOperationResult.ReturnValues[0].ToString(), -1);
                }
            }
            else
            {
                applicationContextProcessOperationResult.Id = applicationContext.Id;
            }

            applicationContextProcessOperationResult.ReferenceId = applicationContext.ReferenceId <= 0
                ? applicationContext.Name
                : applicationContext.ReferenceId.ToString();
        }

        private static void ValidateInputParameters(ApplicationContext applicationContext, CallerContext callerContext)
        {
            if (callerContext == null)
            {
                throw new MDMOperationException("111846", "CallerContext cannot be null.", "ApplicationServiceManager.ApplicationContextBL", String.Empty, "Create");
            }

            if (applicationContext == null)
            {
                throw new MDMOperationException("112382", "Application Context cannot be null.", "ApplicationServiceManager.ApplicationContextBL", String.Empty, "Create");
            }
        }

        #endregion Get And Process

        #region Helper Methods

        private void LocalizeErrors(CallerContext callerContext, OperationResultCollection contextProcessOperationResults)
        {
            foreach (OperationResult or in contextProcessOperationResults)
            {
                foreach (Error error in or.Errors)
                {
                    if (!String.IsNullOrEmpty(error.ErrorCode))
                    {
                        Object messageParams = new object() { };
                        _localeMessage = _localeMessageBL.Get(GlobalizationHelper.GetSystemUILocale(), error.ErrorCode, false, callerContext);

                        if (_localeMessage != null)
                        {
                            error.ErrorMessage = String.Format(_localeMessage.Message, or.ReferenceId);
                            error.Params = new Collection<object> { or.ReferenceId };
                        }
                    }
                }
            }
        }

        private String PopulateProgramName(CallerContext callerContext)
        {
            return String.IsNullOrWhiteSpace(callerContext.ProgramName) ? "ApplicationContextBL.Process" : callerContext.ProgramName;
        }

        private String PopulateUserName()
        {
            String userName = String.Empty;

            if (_securityPrincipal != null)
            {
                userName = _securityPrincipal.CurrentUserName;
            }

            return userName;
        }

        private void GetSecurityPrincipal()
        {
            if (_securityPrincipal == null)
            {
                _securityPrincipal = SecurityPrincipalHelper.GetCurrentSecurityPrincipal();
            }
        }

        #endregion Helper Methods

        #endregion Private Methods
    }
}
