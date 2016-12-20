//using System;
//using System.Collections.Concurrent;
//using System.Collections.Generic;
//using System.Collections.ObjectModel;
//using System.Diagnostics;
//using System.Linq;

//namespace MDM.Imports.Processor
//{
//    using BusinessObjects.Imports;
//    using MDM.BusinessObjects;
//    using MDM.BusinessObjects.Diagnostics;
//    using MDM.BusinessObjects.Jobs;
//    using MDM.BusinessRuleManagement.Business;
//    using MDM.CachedDataModelManager;
//    using MDM.Core;
//    using MDM.Core.Exceptions;
//    using MDM.ExceptionManager.Handlers;
//    using MDM.Imports.Interfaces;
//    using MDM.Interfaces;
//    using MDM.JobManager.Business;
//    using MDM.Services;
//    using MDM.Utility;

//    /// <summary>
//    /// Represents the DDG Import Engine
//    /// </summary>
//    public class DDGImportEngine : IDDGImportEngine
//    {
//        #region Fields

//        /// <summary>
//        /// Field denoting the object used to apply lock on the thread
//        /// </summary>
//        private Object _lockObject = new Object();

//        /// <summary>
//        /// Field denoting the DDGOperationResult Summary for DDGObject Type
//        /// </summary>
//        private ConcurrentDictionary<ObjectType, DDGOperationResultSummary> _ddgOperationResultSummaryDictionary = new ConcurrentDictionary<ObjectType, DDGOperationResultSummary>();

//        /// <summary>
//        /// Field denoting the cached datamodel
//        /// </summary>
//        private ICachedDataModel _cachedDataModel = null;

//        /// <summary>
//        /// Field denoting the WorkflowInfoCollection
//        /// </summary>
//        private WorkflowInfoCollection _workflowInfoCollection = null;

//        /// <summary>
//        /// Field denoting the current executing job
//        /// </summary>
//        private Job _job = null;

//        /// <summary>
//        /// Field denoting the job manager
//        /// </summary>
//        private JobBL _jobManager = null;

//        /// <summary>
//        /// Field denoting the DDGImport Profile
//        /// </summary>
//        private DDGImportProfile _ddgImportProfile = null;

//        /// <summary>
//        /// Field denoting the DDGImport source data
//        /// </summary>
//        private IDDGImportSourceData _sourceData = null;

//        /// <summary>
//        /// Field denoting the ImportJob Result manager
//        /// </summary>
//        private JobImportResultBL _jobImportResultBL = null;

//        /// <summary>
//        /// Field denoting the ImportJob Result handler
//        /// </summary>
//        private JobImportResultHandler _jobImportResultHandler = null;

//        /// <summary>
//        /// Field denoting the current trace setting 
//        /// </summary>
//        private TraceSettings _currentTraceSettings = null;

//        /// <summary>
//        /// Field denoting the diagnostic activity
//        /// </summary>
//        private DiagnosticActivity _diagnosticActivity = null;

//        /// <summary>
//        /// Field denoting the event log handler
//        /// </summary>
//        private EventLogHandler _logHandler = null;

//        /// <summary>
//        /// Field denoting the program name
//        /// </summary>
//        private String programName = "DDGImportEngine";

//        #endregion Fields

//        #region Properties

//        #endregion Properties

//        #region Constructor

//        /// <summary>
//        /// Default Constructor
//        /// </summary>
//        public DDGImportEngine()
//        {
//            _currentTraceSettings = MDMOperationContextHelper.GetCurrentTraceSettings();
//        }

//        #endregion Constructor

//        #region Methods

//        #region IDDGImportEngine Methods

//        /// <summary>
//        /// Initialize the DDGImport engine
//        /// </summary>
//        /// <param name="job">Indicates the job which engine has process</param>
//        /// <param name="ddgImportProfile">Indicate the profile information for the job</param>
//        /// <returns>True - If engine is started successfully, false - if engine is not started sucessfully</returns>
//        public Boolean Initialize(IJob job, IDDGImportProfile ddgImportProfile)
//        {
//            Boolean successFlag = true;

//            try
//            {
//                _diagnosticActivity = new DiagnosticActivity();

//                if (_currentTraceSettings.IsBasicTracingEnabled)
//                {
//                    _diagnosticActivity.Start();
//                    _diagnosticActivity.LogInformation("DDGImportEngine initializing...");
//                }

//                if (job == null)
//                {
//                    throw new ArgumentNullException("Job");
//                }

//                if (ddgImportProfile == null)
//                {
//                    throw new ArgumentNullException("DDGImportProfile");
//                }

//                this._job = job as Job;

//                this._jobManager = new JobBL();

//                this._ddgImportProfile = ddgImportProfile as DDGImportProfile;

//                CheckAndCreateEventLogHandler();

//                this._jobImportResultBL = new JobImportResultBL();
//                this._jobImportResultHandler = new JobImportResultHandler(job.Id);
//                this._jobImportResultHandler.ProgramName = programName;
//                this._jobImportResultHandler.UserName = job.CreatedUser;


//                #region Get CachedDataModel

//                if (_currentTraceSettings.IsBasicTracingEnabled)
//                {
//                    _diagnosticActivity.LogDurationInfo("Loading CacheDataModel Started.");
//                }

//                // First Get the cached data model with all the known meta data and data definitions. This way we can avoid hitting the database.
//                try
//                {
//                    _cachedDataModel = CachedDataModel.GetSingleton(false);
//                }
//                catch (Exception ex)
//                {
//                    String message = "Failed to load data model cache in the import engine. Job would be aborted.";
//                    UpdateJobStatus(JobStatus.Aborted, message);

//                    message = String.Format("Job Id {0} - {1}. Internal Error: {2}", job.Id, message, ex.Message);
//                    _diagnosticActivity.LogError(message);

//                    return false;
//                }

//                if (_currentTraceSettings.IsBasicTracingEnabled)
//                {
//                    _diagnosticActivity.LogDurationInfo("CacheDataModel Loaded Successfully.");
//                }

//                #endregion Get CachedDataModel

//                #region Load Workflow Details

//                if (_currentTraceSettings.IsBasicTracingEnabled)
//                {
//                    _diagnosticActivity.LogDurationInfo("Loading Workflow Details.");
//                }

//                _workflowInfoCollection = LoadAllWorkflowDetails();
//                if (_workflowInfoCollection == null || _workflowInfoCollection.Count < 1)
//                {
//                    String message = "Unable to load workflow details or workflow(s) is not available in the system.";

//                    _diagnosticActivity.LogInformation(message);
//                }

//                if (_currentTraceSettings.IsBasicTracingEnabled)
//                {
//                    _diagnosticActivity.LogDurationInfo("Workflow Details Loaded Successfully.");
//                }

//                #endregion Get CachedDataModel
//            }
//            finally
//            {
//                if (_currentTraceSettings.IsBasicTracingEnabled)
//                {
//                    _diagnosticActivity.LogInformation("DDGImportEngine initialized.");
//                    _diagnosticActivity.Stop();
//                }
//            }

//            return successFlag;
//        }

//        /// <summary>
//        /// Running the DDG import engine
//        /// </summary>
//        /// <param name="stepName">Indicates the step to perform</param>
//        /// <param name="stepConfiguration">Indicates the configuration for the current step</param>
//        /// <param name="importSourceData">Indicates the import source data</param>
//        /// <returns>Flag - True if engine has completed job successfully, False if engine is unable to completed the job successfully</returns>
//        public Boolean RunStep(String stepName, StepConfiguration stepConfiguration, IDDGImportSourceData ddgImportSourceData)
//        {
//            Boolean successFlag = true;
//            String message = String.Empty;

//            try
//            {
//                #region Initialize Parameters

//                _diagnosticActivity = new DiagnosticActivity();

//                message = String.Format("DDGImportEngine executing JobId: '{0}' and Step: '{1}'", _job.Id, stepName);

//                this._sourceData = ddgImportSourceData;

//                #endregion Initialize Parameters

//                #region Start Diagnostic Activity

//                if (_currentTraceSettings.IsBasicTracingEnabled)
//                {
//                    _diagnosticActivity.Start();
//                    _diagnosticActivity.LogInformation(message);
//                }

//                _logHandler.WriteInformationLog(message, 0); //What is Event Id ?

//                #endregion Start Diagnostic Activity

//                #region Validate InputParameters

//                if (stepConfiguration == null)
//                {
//                    message = String.Format("DDGImportEngine: JobId: '{0}', RunStep: {1}. Error: stepConfiguration is null", _job.Id, stepName);
//                    _diagnosticActivity.LogError(message);

//                    throw new ArgumentNullException(message);
//                }

//                if (ddgImportSourceData == null)
//                {
//                    message = String.Format("DDGImportEngine: JobId: '{0}', RunStep: {1}. Error: sourceData is null", _job.Id, stepName);
//                    _diagnosticActivity.LogError(message);

//                    throw new ArgumentNullException(message);
//                }

//                #endregion Validate InputParameters

//                #region Process

//                DurationHelper durationhelper = new DurationHelper(DateTime.Now);

//                message = String.Format("DDGImportEngine started processing for job {0}.", _job.Id.ToString());
//                this._logHandler.WriteInformationLog(message, 0);

//                ProcessDDGObjects();

//                #endregion Process

//                #region Update JobStatus

//                if (this._job.JobStatus == JobStatus.Running)
//                {
//                    _job.JobStatus = Helpers.GetJobStatus(this._job.JobData.ExecutionStatus.TotalElementsSucceed, this._job.JobData.ExecutionStatus.TotalElementsFailed, this._job.JobData.ExecutionStatus.TotalElementsPartiallySucceed);

//                    UpdateJobStatus(_job.JobStatus, "DDG import job completed.");

//                    message = String.Format("DDGImportEngine completed processing for job {0}. The process took {1} seconds.", _job.Id.ToString(), durationhelper.GetDurationInMilliseconds(DateTime.Now));
//                    this._logHandler.WriteInformationLog(message, 0);
//                }

//                #endregion Update JobStatus

//                #region Stop Diagnostic Activity

//                if (_currentTraceSettings.IsBasicTracingEnabled)
//                {
//                    message = String.Format("DDGImportEngine execution completed for JobId: '{0}' and Step: '{1}'", _job.Id, stepName);
//                    this._logHandler.WriteInformationLog(message, 0);

//                    _diagnosticActivity.LogInformation(message);
//                }

//                #endregion Stop Diagnostic Activity
//            }
//            finally
//            {
//                if (_currentTraceSettings.IsBasicTracingEnabled)
//                {
//                    _diagnosticActivity.Stop();
//                }
//            }

//            return successFlag;
//        }

//        #endregion IDDGImportEngine Methods

//        #region Private Methods

//        #region DDGImportEngine Methods

//        private Boolean CheckAndCreateEventLogHandler()
//        {
//            Boolean result = true;

//            try
//            {
//                _logHandler = new EventLogHandler();
//            }
//            catch (Exception e)
//            {
//                result = false;
//                EventLog.WriteEntry("Riversand MDMCenter Job Service Import Process CheckAndCreateEventLogHandler Failed : ", e.Message + "\n" + e.StackTrace);
//            }

//            return result;
//        }

//        private void ProcessDDGObjects()
//        {
//            String message = String.Empty;
//            _diagnosticActivity = new DiagnosticActivity();

//            if (_currentTraceSettings.IsBasicTracingEnabled)
//            {
//                _diagnosticActivity.Start();
//                _diagnosticActivity.LogInformation("DDGImportEngine processing DDG Objects.");
//            }

//            InternalCommonService internalCommonService = new InternalCommonService();
//            DDGOperationResultSummary ddgOperationResultSummary = new DDGOperationResultSummary();

//            CallerContext callerContext = new CallerContext(MDMCenterApplication.PIM, MDMCenterModules.DDGImport, programName, _job.Id, _job.ProfileId, _job.ProfileName);
//            callerContext.AdditionalProperties = new Dictionary<String, Object>();
//            callerContext.AdditionalProperties.Add("UserName", _job.CreatedUser);

//            try
//            {
//                Collection<ObjectType> ddgObjectTypes = _sourceData.GetAllDDGObjectTypesForImport(callerContext);

//                foreach (ObjectType ddgObjectType in ddgObjectTypes)
//                {
//                    IBusinessRuleObjectCollection ddgObjects = null;
//                    IBusinessRuleOperationResultCollection ddgOperationResults = new BusinessRuleOperationResultCollection();

//                    ddgOperationResultSummary.AddObjectInformation(ddgObjectType.ToString(), DDGDictionary.ObjectsDictionary[ddgObjectType], programName, 0);

//                    _ddgOperationResultSummaryDictionary.TryAdd(ddgObjectType, ddgOperationResultSummary);

//                    ddgObjects = _sourceData.GetAllDDGObjects(ddgObjectType, callerContext);

//                    UpdateOperationResultSummary(ddgObjectType, ddgObjects, null, ObjectAction.Create);

//                    switch (ddgObjectType)
//                    {
//                        case ObjectType.BusinessRules:

//                            if (ddgObjects != null && ddgObjects.Count > 0)
//                            {
//                                MDMRuleCollection businessRules = ddgObjects as MDMRuleCollection;

//                                MDMRuleCollection publishedRules = (MDMRuleCollection)businessRules.GetMDMRulesByStatus(RuleStatus.Published);
//                                ProcessRules(internalCommonService, publishedRules, ddgOperationResults, callerContext);

//                                MDMRuleCollection draftRules = (MDMRuleCollection)businessRules.GetMDMRulesByStatus(RuleStatus.Draft);
//                                ProcessRules(internalCommonService, draftRules, ddgOperationResults, callerContext);
//                            }
//                            else
//                            {
//                                if (_currentTraceSettings.IsBasicTracingEnabled)
//                                {
//                                    _diagnosticActivity.LogInformation("There are no Business Rule(s) available for processing.");
//                                }
//                            }

//                            break;
//                        case ObjectType.BusinessConditions:

//                            if (ddgObjects != null && ddgObjects.Count > 0)
//                            {
//                                MDMRuleCollection businessConditions = ddgObjects as MDMRuleCollection;

//                                MDMRuleCollection publishedBC = (MDMRuleCollection)businessConditions.GetMDMRulesByStatus(RuleStatus.Published);
//                                ProcessRules(internalCommonService, publishedBC, ddgOperationResults, callerContext, true);

//                                MDMRuleCollection draftBC = (MDMRuleCollection)businessConditions.GetMDMRulesByStatus(RuleStatus.Draft);
//                                ProcessRules(internalCommonService, draftBC, ddgOperationResults, callerContext, true);
//                            }
//                            else
//                            {
//                                if (_currentTraceSettings.IsBasicTracingEnabled)
//                                {
//                                    _diagnosticActivity.LogInformation("There are no Business Condition(s) available for processing.");
//                                }
//                            }

//                            break;
//                        case ObjectType.BusinessConditionSorting:

//                            if (ddgObjects != null && ddgObjects.Count > 0)
//                            {
//                                MDMRuleCollection businessConditions = ddgObjects as MDMRuleCollection;
//                                ProcessBusinessConditionSorting(businessConditions, ddgOperationResults);
//                            }
//                            else
//                            {
//                                if (_currentTraceSettings.IsBasicTracingEnabled)
//                                {
//                                    _diagnosticActivity.LogInformation("There are no Business Condition Rules(s) available for sorting.");
//                                }
//                            }

//                            break;
//                        case ObjectType.DynamicGovernance:

//                            if (ddgObjects != null && ddgObjects.Count > 0)
//                            {
//                                MDMRuleMapCollection ruleMaps = ddgObjects as MDMRuleMapCollection;
//                                IBusinessRuleOperationResultCollection ruleMapOperationResults = null;

//                                ValidateAndFillApplicationContextAndWorkflowDetails(ruleMaps, ddgOperationResults);
//                                ruleMaps = FilterDDGObjectsBasedOnOpResults(ruleMaps, ddgOperationResults) as MDMRuleMapCollection;

//                                //This filteration of operation result is required otherwise it will result in duplicate operation result for same rule context.
//                                //This cannot be done in ValidateAndFillApplicationContextAndWorkflowDetails() method as after that overall status is identified based on total
//                                //number of operation results vs number of operation result with failed status.
//                                ddgOperationResults = ddgOperationResults.GetErroredBusinessRuleOperationResults() as IBusinessRuleOperationResultCollection;

//                                ruleMapOperationResults = internalCommonService.ProcessMDMRuleMaps(ruleMaps, callerContext);
//                                ddgOperationResults.AddRange(ruleMapOperationResults);
//                            }
//                            else
//                            {
//                                if (_currentTraceSettings.IsBasicTracingEnabled)
//                                {
//                                    _diagnosticActivity.LogInformation("There are no Dynamic Governance Context(s) available for processing.");
//                                }
//                            }

//                            break;
//                        case ObjectType.DynamicGovernanceSorting:

//                            if (ddgObjects != null && ddgObjects.Count > 0)
//                            {
//                                MDMRuleMapRuleCollection ruleMapRules = ddgObjects as MDMRuleMapRuleCollection;
//                                ProcessDynamicGovernanceSorting(ruleMapRules, ddgOperationResults);
//                            }
//                            else
//                            {
//                                if (_currentTraceSettings.IsBasicTracingEnabled)
//                                {
//                                    _diagnosticActivity.LogInformation("There are no Dynamic Governance Context(s) available for processing.");
//                                }
//                            }

//                            break;
//                        case ObjectType.SystemMessages:
//                            if (ddgObjects != null && ddgObjects.Count > 0)
//                            {
//                                LocaleMessageCollection localeMessages = ddgObjects as LocaleMessageCollection;
//                                IBusinessRuleOperationResultCollection ddgLocaleMessageOperationResults = null;

//                                //remove the message from further processing if locale message does not exist or not available for process in SDL.
//                                LocaleMessageCollection filteredLocaleMesssages = ValidateMessageExistenceInSDL(localeMessages,
//                                                ddgOperationResults as BusinessRuleOperationResultCollection, callerContext);

//                                if (filteredLocaleMesssages != null && filteredLocaleMesssages.Count > 0)
//                                {
//                                    ddgLocaleMessageOperationResults = internalCommonService.ProcessDDGLocaleMessages(filteredLocaleMesssages, callerContext);
//                                }

//                                ddgOperationResults.AddRange(ddgLocaleMessageOperationResults);
//                            }
//                            else
//                            {
//                                if (_currentTraceSettings.IsBasicTracingEnabled)
//                                {
//                                    _diagnosticActivity.LogInformation("There are no System Message(s) available for processing.");
//                                }
//                            }
//                            break;
//                    }

//                    if (ddgOperationResults == null)
//                    {
//                        message = String.Format("DDGImportEngine.Process for [{0}] didn't return operationResult", ddgObjectType.ToString());
//                        throw new MDMOperationException("", message, "DDGImportEngine", String.Empty, "Process");
//                    }

//                    else
//                    {
//                        PopulateImportResult(ddgObjectType, ddgObjects, ddgOperationResults);
//                    }
//                }
//            }
//            catch
//            {
//                throw;
//            }
//            finally
//            {
//                if (_currentTraceSettings.IsBasicTracingEnabled)
//                {
//                    _diagnosticActivity.LogInformation("DDGImportEngine processed DDG Objects.");
//                    _diagnosticActivity.Stop();
//                }
//            }
//        }

//        private void ProcessRules(InternalCommonService internalCommonService, MDMRuleCollection businessRules, IBusinessRuleOperationResultCollection operationResults, CallerContext callerContext, Boolean processOnlyRules = false)
//        {
//            if (businessRules != null && businessRules.Count > 0)
//            {
//                var result = internalCommonService.ProcessMDMRules(businessRules, callerContext, processOnlyRules);
//                operationResults.AddRange(result);
//            }
//        }

//        private void ProcessBusinessConditionSorting(MDMRuleCollection businessConditions, IBusinessRuleOperationResultCollection ddgOperationResults)
//        {
//            IMDMRuleCollection businessConditionsToUpdate = new MDMRuleCollection();
//            ICallerContext callerContext = new CallerContext(MDMCenterApplication.PIM, MDMCenterModules.DDGImport);
//            InternalCommonService internalCommonService = new InternalCommonService();

//            IMDMRuleContextFilter mdmRuleSearchCriteria = new MDMRuleContextFilter();
//            mdmRuleSearchCriteria.MDMRuleType = MDMRuleType.BusinessCondition;

//            IMDMRuleCollection businessConditionsFromDB = internalCommonService.GetMDMRulesByContext(mdmRuleSearchCriteria, callerContext);

//            foreach (MDMRule businessCondition in businessConditions)
//            {
//                MDMRule businessConditionFromDB = businessConditionsFromDB.GetMDMRuleByName(businessCondition.Name);

//                if (businessConditionFromDB != null && businessCondition.BusinessConditionRules.Count > 0)
//                {
//                    businessConditionFromDB.BusinessConditionRules.Clear();
//                    businessConditionFromDB.BusinessConditionRules.AddRange(businessCondition.BusinessConditionRules);
//                    businessConditionFromDB.Action = ObjectAction.Create;
//                    businessConditionsToUpdate.Add(businessConditionFromDB);
//                }
//                else
//                {
//                    // Todo..
//                    BusinessRuleOperationResult ddgOpResult = new BusinessRuleOperationResult(businessCondition);
//                    Object[] parameters = new Object[] { businessCondition.Name };
//                    String errorMessage = String.Format("Failed to process sorting information for the business condition: '{0}', as it does not exist in the system.", parameters);
//                    ddgOpResult.AddOperationResult("-1", errorMessage, parameters, OperationResultType.Error);
//                    ddgOperationResults.Add(ddgOpResult);
//                }
//            }

//            if (businessConditionsToUpdate.Count > 0)
//            {
//                IBusinessRuleOperationResultCollection opResults = internalCommonService.ProcessMDMRules(businessConditionsToUpdate, callerContext, processOnlyRules: true);
//                ddgOperationResults.AddRange(opResults);
//            }
//        }

//        private void ProcessDynamicGovernanceSorting(MDMRuleMapRuleCollection mdmRuleMapRules, IBusinessRuleOperationResultCollection ddgOperationResults)
//        {
//            IMDMRuleMapCollection mdmRuleMapsToUpdate = new MDMRuleMapCollection();
//            ICallerContext callerContext = new CallerContext(MDMCenterApplication.PIM, MDMCenterModules.DDGImport);
//            InternalCommonService internalCommonService = new InternalCommonService();
//            Int32 referenceId = -1;

//            IMDMRuleMapCollection mdmRuleMapsFromDB = internalCommonService.GetMDMRuleMaps(callerContext);

//            foreach (MDMRuleMapRule mdmRuleMapRule in mdmRuleMapRules)
//            {
//                if (!mdmRuleMapsToUpdate.Contains(mdmRuleMapRule.RuleMapName))
//                {
//                    MDMRuleMap mdmRuleMapFromDB = mdmRuleMapsFromDB.Where(rmr => rmr.Name == mdmRuleMapRule.RuleMapName).FirstOrDefault();

//                    if (mdmRuleMapFromDB != null)
//                    {
//                        if (!mdmRuleMapsToUpdate.Contains(mdmRuleMapFromDB))
//                        {
//                            mdmRuleMapFromDB.ReferenceId = referenceId;
//                            var modifiedRuleMapRules = mdmRuleMapRules[mdmRuleMapRule.RuleMapName];

//                            foreach (MDMRuleMapRule modifiedRuleMapRule in modifiedRuleMapRules)
//                            {
//                                var originalRuleMapRule = mdmRuleMapFromDB.GetMDMRuleMapRule(modifiedRuleMapRule.RuleName, modifiedRuleMapRule.RuleStatus);

//                                if (originalRuleMapRule == null)
//                                {
//                                    String errorMessageCode = String.Empty;
//                                    String errorMessage = String.Empty;
//                                    if (modifiedRuleMapRule.RuleType == MDMRuleType.BusinessCondition)
//                                    {
//                                        errorMessageCode = "114201";
//                                        errorMessage = "Failed to process the Dynamic Governance '{0}', as the BusinessCondition '{1}' is not published or does not exist in the system.";
//                                    }
//                                    else
//                                    {
//                                        errorMessageCode = "114200";
//                                        errorMessage = "Failed to process the Dynamic Governance '{0}', as the BusinessRule '{1}' is not Published or does not exist in the system.";
//                                    }

//                                    BusinessRuleOperationResult ddgOpResult = new BusinessRuleOperationResult(modifiedRuleMapRule);
//                                    Object[] parameters = { modifiedRuleMapRule.RuleMapName, modifiedRuleMapRule.RuleName };
//                                    String formattedErrorMessage = String.Format(errorMessage, parameters);
//                                    ddgOpResult.AddOperationResult(errorMessageCode, formattedErrorMessage, parameters, OperationResultType.Error);
//                                    ddgOperationResults.Add(ddgOpResult);
//                                }
//                                else
//                                {
//                                    mdmRuleMapFromDB.MDMRuleMapRules.Add(modifiedRuleMapRule);
//                                    originalRuleMapRule.IgnoreChangeContext = modifiedRuleMapRule.IgnoreChangeContext;
//                                    originalRuleMapRule.Sequence = modifiedRuleMapRule.Sequence;
//                                }
//                            }
                           
//                            mdmRuleMapFromDB.Action = ObjectAction.Update;
//                            mdmRuleMapsToUpdate.Add(mdmRuleMapFromDB);
//                            referenceId--;
//                        }
//                    }
//                    else
//                    {
//                        // Todo..
//                        BusinessRuleOperationResult ddgOpResult = new BusinessRuleOperationResult(mdmRuleMapRule);
//                        Object[] parameters = new Object[] { mdmRuleMapRule.Name };
//                        String errorMessage = String.Format("Failed to process sorting information for the RuleMap: '{0}', as it does not exist in the system.", parameters);
//                        ddgOpResult.AddOperationResult("-1", errorMessage, parameters, OperationResultType.Error);
//                        ddgOperationResults.Add(ddgOpResult);
//                    }
//                }
//            }

//            if (mdmRuleMapsToUpdate.Count > 0)
//            {
//                IBusinessRuleOperationResultCollection opResults = internalCommonService.ProcessMDMRuleMaps(mdmRuleMapsToUpdate, callerContext, processApplicationContext: false);
//                ddgOperationResults.AddRange(opResults);
//            }
//        }

//        private BusinessRuleOperationResultCollection RemoveSuccessfulOperationResults(BusinessRuleOperationResultCollection operationResults)
//        {
//            BusinessRuleOperationResultCollection filteredBusinessRuleOperationResults = new BusinessRuleOperationResultCollection();

//            foreach (BusinessRuleOperationResult operationResult in operationResults)
//            {
//                if (operationResult.HasError)
//                {
//                    filteredBusinessRuleOperationResults.Add(operationResult);
//                }
//            }

//            return filteredBusinessRuleOperationResults;
//        }

//        private LocaleMessageCollection ValidateMessageExistenceInSDL(LocaleMessageCollection localeMessages, BusinessRuleOperationResultCollection businessRuleOperationResults, CallerContext callerContext)
//        {
//            LocaleMessageCollection filteredLocaleMessages = new LocaleMessageCollection();

//            LocaleEnum systemDataLocale = GlobalizationHelper.GetSystemDataLocale();
//            if (localeMessages != null && localeMessages.Count > 0)
//            {
//                LocaleMessageCollection SDLDDGMessages = new LocaleMessageCollection();
//                LocaleMessageCollection nonSDLDDGMessages = new LocaleMessageCollection();

//                //Prepare 2 bucket one for SDL message and another for non SDL messages
//                foreach (LocaleMessage localeMessage in localeMessages)
//                {
//                    if (localeMessage.Action != ObjectAction.Ignore)
//                    {
//                        if (localeMessage.Locale == systemDataLocale)
//                        {
//                            SDLDDGMessages.Add(localeMessage);
//                            filteredLocaleMessages.Add(localeMessage);
//                        }
//                        else
//                        {
//                            nonSDLDDGMessages.Add(localeMessage);
//                        }
//                    }
//                }

//                //Check if respective SDL message exist in DB or is available for process.
//                if (nonSDLDDGMessages.Count > 0)
//                {
//                    InternalCommonService internalCommonService = new InternalCommonService();
//                    LocaleMessageCollection existingLocaleMessages = (LocaleMessageCollection)internalCommonService.GetDDGLocaleMessages(callerContext as ICallerContext);

//                    Boolean isSDLMessageExist = false;

//                    foreach (LocaleMessage nonSDLDDGMessage in nonSDLDDGMessages)
//                    {
//                        isSDLMessageExist = false;

//                        if (!SDLDDGMessages.Contains(systemDataLocale, nonSDLDDGMessage.Code, systemDataLocale))
//                        {
//                            if (existingLocaleMessages.Contains(systemDataLocale, nonSDLDDGMessage.Code, systemDataLocale))
//                            {
//                                isSDLMessageExist = true;
//                            }
//                        }
//                        else
//                        {
//                            isSDLMessageExist = true;
//                        }

//                        if (!isSDLMessageExist)
//                        {
//                            BusinessRuleOperationResult businessRuleOperationResult = new BusinessRuleOperationResult(nonSDLDDGMessage);

//                            //TODO: to update the message code.
//                            Object[] parameters = new Object[] { GlobalizationHelper.GetSystemDataLocale() };
//                            String errorMessage = String.Format("Failed to process the locale message, as the message also needs to be updated in system data locale '{0}'.", parameters);
//                            _diagnosticActivity.LogError(errorMessage);
//                            businessRuleOperationResult.AddOperationResult("114442", errorMessage, parameters, OperationResultType.Error, callerContext);

//                            businessRuleOperationResults.Add(businessRuleOperationResult);
//                        }
//                        else
//                        {
//                            filteredLocaleMessages.Add(nonSDLDDGMessage);
//                        }
//                    }
//                }
//            }

//            return filteredLocaleMessages;
//        }

//        private void ValidateAndFillApplicationContextAndWorkflowDetails(IBusinessRuleObjectCollection ddgObjects, IBusinessRuleOperationResultCollection ddgOpResults)
//        {
//            //Note: Here we are validating Workflow information to avoid looping
//            MDMRuleMapCollection mdmRuleMaps = ddgObjects as MDMRuleMapCollection;

//            foreach (MDMRuleMap mdmRuleMap in mdmRuleMaps)
//            {
//                ApplicationContext applicationContext = mdmRuleMap.ApplicationContext;
//                BusinessRuleOperationResult ddgOperationResult = new BusinessRuleOperationResult(mdmRuleMap);

//                Container container = null;
//                Boolean performValidation = true;

//                #region Validate and Fill ApplicationContext

//                if (!String.IsNullOrWhiteSpace(applicationContext.OrganizationName))
//                {
//                    Organization organization = GetOrganizationByName(applicationContext.OrganizationName);
//                    if (organization != null)
//                    {
//                        mdmRuleMap.ApplicationContext.OrganizationId = organization.Id;
//                    }
//                    else
//                    {
//                        PopulateOperationResultForApplicationContext(mdmRuleMap.Name, ObjectType.Organization, applicationContext.OrganizationName, ddgOperationResult);
//                        performValidation = false;
//                    }
//                }

//                if (!String.IsNullOrWhiteSpace(applicationContext.ContainerName) && performValidation)
//                {
//                    container = GetContainerByName(applicationContext.ContainerName);
//                    if (container != null)
//                    {
//                        mdmRuleMap.ApplicationContext.ContainerId = container.Id;
//                    }
//                    else
//                    {
//                        PopulateOperationResultForApplicationContext(mdmRuleMap.Name, ObjectType.Catalog, applicationContext.ContainerName, ddgOperationResult);
//                        performValidation = false;
//                    }
//                }

//                if (container != null && !String.IsNullOrWhiteSpace(applicationContext.CategoryPath) && performValidation)
//                {
//                    Category category = GetCategoryByName(container, applicationContext.CategoryPath);
//                    if (category != null)
//                    {
//                        mdmRuleMap.ApplicationContext.CategoryId = category.Id;
//                    }
//                    else
//                    {
//                        PopulateOperationResultForApplicationContext(mdmRuleMap.Name, ObjectType.Category, applicationContext.CategoryPath, ddgOperationResult);
//                        performValidation = false;
//                    }
//                }

//                if (!String.IsNullOrWhiteSpace(applicationContext.EntityTypeName) && performValidation)
//                {
//                    EntityType entityType = GetEntityTypeByName(applicationContext.EntityTypeName);
//                    if (entityType != null)
//                    {
//                        mdmRuleMap.ApplicationContext.EntityTypeId = entityType.Id;
//                    }
//                    else
//                    {
//                        PopulateOperationResultForApplicationContext(mdmRuleMap.Name, ObjectType.EntityType, applicationContext.EntityTypeName, ddgOperationResult);
//                        performValidation = false;
//                    }
//                }

//                if (!String.IsNullOrWhiteSpace(applicationContext.RelationshipTypeName) && performValidation)
//                {
//                    RelationshipType relationshipType = GetRelationshipTypeByName(applicationContext.RelationshipTypeName);
//                    if (relationshipType != null)
//                    {
//                        mdmRuleMap.ApplicationContext.RelationshipTypeId = relationshipType.Id;
//                    }
//                    else
//                    {
//                        PopulateOperationResultForApplicationContext(mdmRuleMap.Name, ObjectType.RelationshipType, applicationContext.RelationshipTypeName, ddgOperationResult);
//                        performValidation = false;
//                    }
//                }

//                if (!String.IsNullOrWhiteSpace(applicationContext.RoleName) && performValidation)
//                {
//                    SecurityRole securityRole = GetSecurityRoleByName(applicationContext.RoleName);
//                    if (securityRole != null)
//                    {
//                        mdmRuleMap.ApplicationContext.RoleId = securityRole.Id;
//                    }
//                    else
//                    {
//                        PopulateOperationResultForApplicationContext(mdmRuleMap.Name, ObjectType.Role, applicationContext.RoleName, ddgOperationResult);
//                        performValidation = false;
//                    }
//                }

//                #endregion Validate and Fill ApplicationContext

//                #region Validate and Fill WorkflowDetails

//                if (_workflowInfoCollection != null && _workflowInfoCollection.Count > 0 && performValidation)
//                {
//                    String workflowName = mdmRuleMap.WorkflowInfo.WorkflowName;
//                    String workflowActivityName = !String.IsNullOrWhiteSpace(mdmRuleMap.WorkflowInfo.WorkflowActivityShortName) ? mdmRuleMap.WorkflowInfo.WorkflowActivityShortName : mdmRuleMap.WorkflowInfo.WorkflowActivityLongName;
//                    String workflowAction = mdmRuleMap.WorkflowInfo.WorkflowActivityAction;

//                    if (!(String.IsNullOrWhiteSpace(workflowName) && String.IsNullOrWhiteSpace(workflowActivityName)))
//                    {
//                        WorkflowInfo workflowInfo = _workflowInfoCollection.GetWorkflowInfo(workflowName, workflowActivityName, workflowAction) as WorkflowInfo;

//                        if (workflowInfo != null)
//                        {
//                            mdmRuleMap.WorkflowInfo.WorkflowActivityId = workflowInfo.WorkflowActivityId;
//                        }
//                        else
//                        {
//                            Object[] parameters;
//                            if (String.IsNullOrWhiteSpace(workflowAction))
//                            {
//                                parameters = new Object[] { mdmRuleMap.Name, workflowName, workflowActivityName };
//                                String errorMessage = String.Format("Failed to populate application context for dynamic governance {0}, as the combination of WorkflowName: {1} and WorkflowActivityName: {2} is invalid.", parameters);
//                                ddgOperationResult.AddOperationResult("-1", errorMessage, parameters, OperationResultType.Error);//ToDo: Replace message code
//                            }
//                            else
//                            {
//                                parameters = new Object[] { mdmRuleMap.Name, workflowName, workflowActivityName, workflowAction };
//                                String errorMessage = String.Format("Failed to populate application context for dynamic governance {0}, as the combination of WorkflowName: {1}, WorkflowActivityName: {2}, and WorkflowAction: {3} is invalid.", parameters);
//                                ddgOperationResult.AddOperationResult("114275", errorMessage, parameters, OperationResultType.Error);
//                            }
//                        }
//                    }
//                }

//                #endregion Validate and Fill WorkflowDetails

//                ddgOpResults.Add(ddgOperationResult);
//            }
//        }

//        private void PopulateOperationResultForApplicationContext(String mappingName, ObjectType objectType, String ddgObjectName, BusinessRuleOperationResult businessRuleOpResult)
//        {
//            if (businessRuleOpResult != null)
//            {
//                Object[] parameters = new Object[] { mappingName, objectType.ToString(), ddgObjectName };
//                String errorMessage = String.Format("Failed to populate application context for dynamic governance {0}, as '{1}: {2}' does not exist in the system.", parameters);
//                businessRuleOpResult.AddOperationResult("114274", errorMessage, parameters, OperationResultType.Error);
//            }
//        }

//        private IBusinessRuleObjectCollection FilterDDGObjectsBasedOnOpResults(IBusinessRuleObjectCollection ddgObjects, IBusinessRuleOperationResultCollection businessRuleOpResults)
//        {
//            businessRuleOpResults.RefreshBusinessRuleOperationResultStatus();

//            if (businessRuleOpResults.OperationResultStatus == OperationResultStatusEnum.Failed)
//            {
//                //Nothing to process
//                return null;
//            }
//            else if (ddgObjects != null && businessRuleOpResults != null && businessRuleOpResults.OperationResultStatus == OperationResultStatusEnum.CompletedWithErrors)
//            {
//                BusinessRuleOperationResultCollection errors = businessRuleOpResults.GetBusinessRuleOperationResultByOperationResultStatus(OperationResultStatusEnum.Failed);

//                if (errors != null && errors.Count > 0)
//                {
//                    foreach (IBusinessRuleOperationResult result in errors)
//                    {
//                        ddgObjects.RemoveByReferenceId(result.ReferenceId);
//                    }
//                }
//            }

//            return ddgObjects;
//        }

//        #endregion DDGImportEngine Methods

//        #region Cached DataModel Methods

//        private Organization GetOrganizationByName(String organizationName)
//        {
//            DiagnosticActivity activity = new DiagnosticActivity();
//            Organization organizationToReturn = null;

//            String message = String.Empty;

//            if (_currentTraceSettings.IsBasicTracingEnabled)
//            {
//                activity.Start();
//                activity.LogInformation(String.Format("Loading details for organization name {0}.", organizationName));
//            }

//            if (String.IsNullOrWhiteSpace(organizationName))
//            {
//                if (_currentTraceSettings.IsBasicTracingEnabled)
//                {
//                    activity.LogError("Organization name cannot be empty");
//                    activity.Stop();
//                }

//                return null;
//            }

//            OrganizationCollection organizations = new OrganizationCollection(_cachedDataModel.GetOrganizations());
//            organizationToReturn = organizations.Get(organizationName) as Organization;

//            if (_currentTraceSettings.IsBasicTracingEnabled)
//            {
//                activity.LogInformation(String.Format("Loaded details for organization name {0}.", organizationName));
//                activity.Stop();
//            }

//            return organizationToReturn;
//        }

//        private Container GetContainerByName(String containerName)
//        {
//            DiagnosticActivity activity = new DiagnosticActivity();
//            Container containerToReturn = null;

//            String message = String.Empty;

//            if (_currentTraceSettings.IsBasicTracingEnabled)
//            {
//                activity.Start();
//                activity.LogInformation(String.Format("Loading details for container name {0}.", containerName));
//            }

//            if (String.IsNullOrWhiteSpace(containerName))
//            {
//                if (_currentTraceSettings.IsBasicTracingEnabled)
//                {
//                    activity.LogError("Container name cannot be empty");
//                    activity.Stop();
//                }

//                return null;
//            }

//            ContainerCollection containers = new ContainerCollection(_cachedDataModel.GetContainers());
//            containerToReturn = containers.GetContainerByName(containerName) as Container;

//            if (_currentTraceSettings.IsBasicTracingEnabled)
//            {
//                activity.LogInformation(String.Format("Loaded details for container name {0}.", containerName));
//                activity.Stop();
//            }

//            return containerToReturn;
//        }

//        private Category GetCategoryByName(Container container, String categoryPathName)
//        {
//            DiagnosticActivity activity = new DiagnosticActivity();
//            Category categoryToReturn = null;

//            String message = String.Empty;

//            if (_currentTraceSettings.IsBasicTracingEnabled)
//            {
//                activity.Start();
//                activity.LogInformation(String.Format("Loading details for category path {0}.", categoryPathName));
//            }

//            if (container == null && String.IsNullOrWhiteSpace(categoryPathName))
//            {
//                if (_currentTraceSettings.IsBasicTracingEnabled)
//                {
//                    activity.LogError("Container object or Category path cannot be empty");
//                    activity.Stop();
//                }

//                return null;
//            }

//            CategoryCollection categories = _cachedDataModel.GetCategories(container.HierarchyId);
//            categoryToReturn = categories.Get(container.HierarchyId, categoryPathName);

//            if (_currentTraceSettings.IsBasicTracingEnabled)
//            {
//                activity.LogInformation(String.Format("Loaded details for category path {0}.", categoryPathName));
//                activity.Stop();
//            }

//            return categoryToReturn;
//        }

//        private EntityType GetEntityTypeByName(String entityTypeName)
//        {
//            DiagnosticActivity activity = new DiagnosticActivity();
//            EntityType entityTypeToReturn = null;

//            String message = String.Empty;

//            if (_currentTraceSettings.IsBasicTracingEnabled)
//            {
//                activity.Start();
//                activity.LogInformation(String.Format("Loading details for entitytype name {0}.", entityTypeName));
//            }

//            if (String.IsNullOrWhiteSpace(entityTypeName))
//            {
//                if (_currentTraceSettings.IsBasicTracingEnabled)
//                {
//                    activity.LogError("EntityType name cannot be empty");
//                    activity.Stop();
//                }

//                return null;
//            }

//            EntityTypeCollection entityTypes = new EntityTypeCollection(_cachedDataModel.GetEntityTypes());
//            entityTypeToReturn = entityTypes.Get(entityTypeName);

//            if (_currentTraceSettings.IsBasicTracingEnabled)
//            {
//                activity.LogInformation(String.Format("Loaded details for entitytype name {0}.", entityTypeName));
//                activity.Stop();
//            }

//            return entityTypeToReturn;
//        }

//        private RelationshipType GetRelationshipTypeByName(String relationshipTypeName)
//        {
//            DiagnosticActivity activity = new DiagnosticActivity();
//            RelationshipType relationshipTypeToReturn = null;

//            String message = String.Empty;

//            if (_currentTraceSettings.IsBasicTracingEnabled)
//            {
//                activity.Start();
//                activity.LogInformation(String.Format("Loading details for relationshiptype name {0}.", relationshipTypeName));
//            }

//            if (String.IsNullOrWhiteSpace(relationshipTypeName))
//            {
//                if (_currentTraceSettings.IsBasicTracingEnabled)
//                {
//                    activity.LogError("RelationshipType name cannot be empty");
//                    activity.Stop();
//                }

//                return null;
//            }

//            RelationshipTypeCollection relationshipTypes = new RelationshipTypeCollection(_cachedDataModel.GetRelationshipTypes());
//            relationshipTypeToReturn = relationshipTypes.Get(relationshipTypeName);

//            if (_currentTraceSettings.IsBasicTracingEnabled)
//            {
//                activity.LogInformation(String.Format("Loaded details for relationshiptype name {0}.", relationshipTypeName));
//                activity.Stop();
//            }

//            return relationshipTypeToReturn;
//        }

//        private SecurityRole GetSecurityRoleByName(String securityRoleName)
//        {
//            DiagnosticActivity activity = new DiagnosticActivity();
//            SecurityRole securityRoleToReturn = null;

//            String message = String.Empty;

//            if (_currentTraceSettings.IsBasicTracingEnabled)
//            {
//                activity.Start();
//                activity.LogInformation(String.Format("Loading details for securityrole name {0}.", securityRoleName));
//            }

//            if (String.IsNullOrWhiteSpace(securityRoleName))
//            {
//                if (_currentTraceSettings.IsBasicTracingEnabled)
//                {
//                    activity.LogError("SecurityRole name cannot be empty");
//                    activity.Stop();
//                }

//                return null;
//            }

//            // Here we are making an API call. During stress testing we will decide whether we should added this objects to CacheDataModel or not
//            SecurityService securityService = new SecurityService();
//            securityRoleToReturn = securityService.GetSecurityRoleByName(securityRoleName, new CallerContext(MDMCenterApplication.JobService, MDMCenterModules.DDGImport));

//            if (_currentTraceSettings.IsBasicTracingEnabled)
//            {
//                activity.LogInformation(String.Format("Loaded details for securityrole name {0}.", securityRoleName));
//                activity.Stop();
//            }

//            return securityRoleToReturn;
//        }

//        private WorkflowInfoCollection LoadAllWorkflowDetails()
//        {
//            WorkflowInfoCollection workflowInfoCollection = new WorkflowInfoCollection();
//            CallerContext callerContext = new CallerContext(MDMCenterApplication.JobService, MDMCenterModules.DDGImport);

//            WorkflowService workflowService = new WorkflowService();
//            workflowInfoCollection = workflowService.GetAllWorkflowInformation(callerContext) as WorkflowInfoCollection;

//            return workflowInfoCollection;
//        }

//        #endregion Cached DataModel Methods

//        #region Job Manager Methods

//        private void UpdateJobStatus(JobStatus jobStatus, String description)
//        {
//            try
//            {
//                _diagnosticActivity = new DiagnosticActivity();

//                if (_currentTraceSettings.IsBasicTracingEnabled)
//                {
//                    _diagnosticActivity.Start();

//                    String startInfo = String.Format("Updating job status for Job: {0} Status: {1} - {2}", _job.Id, jobStatus, description);
//                    _diagnosticActivity.LogInformation(startInfo);
//                }

//                lock (_lockObject)
//                {
//                    ExecutionStatus executionStatus = _job.JobData.ExecutionStatus;

//                    DateTime endTime = DateTime.Now;
//                    executionStatus.EndTime = endTime.ToString();

//                    DateTime startTime = DateTime.Now;
//                    if (DateTime.TryParse(executionStatus.StartTime, out startTime))
//                    {
//                        executionStatus.TotalMilliSeconds = (endTime - startTime).TotalMilliseconds;
//                    }

//                    executionStatus.CurrentStatusMessage = _job.JobStatus.ToString();

//                    #region Prepare job description

//                    String jobDescription = String.Empty;

//                    if (executionStatus.TotalElementsToProcess == 0 && executionStatus.TotalElementsProcessed == 0)
//                    {
//                        jobDescription = "Processed job returned with no data. Check the input and retry again.";
//                    }
//                    else
//                    {
//                        jobDescription = String.Format("DDGImport - Total-{0}; Processed-{1};  Success-{2}; CompletedWithWarnings-{3}; Failed-{4};",
//                                                                             executionStatus.TotalElementsToProcess,
//                                                                             executionStatus.TotalElementsProcessed,
//                                                                             executionStatus.TotalElementsSucceed,
//                                                                             executionStatus.TotalElementsPartiallySucceed,
//                                                                             executionStatus.TotalElementsFailed);
//                    }

//                    switch (jobStatus)
//                    {
//                        case JobStatus.Completed:
//                        case JobStatus.CompletedWithErrors:
//                        case JobStatus.CompletedWithWarningsAndErrors:
//                        case JobStatus.CompletedWithWarnings:
//                            _job.Description = jobDescription;
//                            break;
//                        case JobStatus.Aborted:
//                            _job.Description = "Job has been Aborted. Please check job details for more information.";
//                            executionStatus.CurrentStatusMessage = jobStatus.ToString();
//                            break;
//                        default:
//                            _job.Description = String.Format("{0} {1}", jobDescription, description);
//                            break;
//                    }

//                    #endregion Prepare job description
//                }

//                _jobManager.Update(_job, new CallerContext(MDMCenterApplication.JobService, Utility.GetModule(_job.JobType), programName));

//                if (_currentTraceSettings.IsBasicTracingEnabled)
//                {
//                    String stopInfo = String.Format("Updated job status for Job: {0} Status: {1} - {2}", _job.Id, jobStatus, description);
//                    _diagnosticActivity.LogInformation(stopInfo);
//                }
//            }
//            finally
//            {
//                if (_currentTraceSettings.IsBasicTracingEnabled)
//                {
//                    _diagnosticActivity.Stop();
//                }
//            }
//        }

//        /// <summary>
//        /// Populate import result table.
//        /// </summary>
//        private void PopulateImportResult(ObjectType ddgObjectType, IBusinessRuleObjectCollection ddgObjects, IBusinessRuleOperationResultCollection businessRuleOperationResults)
//        {
//            // Log Job level Error
//            UpdateOperationResult(ddgObjectType, ddgObjects, businessRuleOperationResults);

//            // Update summary of ORs
//            UpdateOperationResultSummary(ddgObjectType, ddgObjects, businessRuleOperationResults, ObjectAction.Update);

//            // Log Success ORs
//            LogSuccess(ddgObjectType, businessRuleOperationResults);

//            // Log Error ORs
//            LogErrors(ddgObjectType, businessRuleOperationResults);
//        }

//        #endregion Job Manager Methods

//        #region Job Notification

//        /// <summary>
//        /// Log successful entries if required.
//        /// </summary>
//        /// <param name="ddgObjectType">Indicates the DDG Object type</param>
//        /// <param name="objOperationResultCollection">Indicates the DDG operation result collection</param>
//        private void LogSuccess(ObjectType ddgObjectType, IBusinessRuleOperationResultCollection objOperationResultCollection)
//        {

//            // Get entity list with create action
//            List<BusinessRuleOperationResult> objOperationList = (from operationResult in objOperationResultCollection
//                                                                  where (operationResult.OperationResultStatus == OperationResultStatusEnum.Successful ||
//                                                                         operationResult.OperationResultStatus == OperationResultStatusEnum.None)
//                                                                  select operationResult).ToList();
//            // do we have anything to process?
//            if (objOperationList.Count == 0)
//            {
//                return;
//            }

//            BusinessRuleOperationResultCollection objSuccessCollection = new BusinessRuleOperationResultCollection();

//            foreach (BusinessRuleOperationResult item in objOperationList)
//            {
//                item.DDGObjectType = ddgObjectType;

//                if (item.OperationResultStatus == OperationResultStatusEnum.Successful)
//                {
//                    item.Informations.Add(new Information("", String.Format("Object modified. Action Performed [{0}].", item.PerformedAction.ToString()), new Collection<Object> { item.Id })); //saved successfully
//                }
//                else
//                {
//                    item.Informations.Add(new Information("", "Object was not modified")); //saved successfully
//                }

//                objSuccessCollection.Add(item);
//            }

//            // update the job import result
//            _jobImportResultHandler.Save(objSuccessCollection, true);
//        }


//        /// <summary>
//        /// Update the soure with the error message for a given entity. This will also REMOVE the errored items from the business rule collection from further processing.
//        /// </summary>
//        /// <param name="ddgModelObjectType">Indicates the DDG model object type</param>
//        /// <param name="operationResults">Indicates the DDG operation result collection</param>
//        private void LogErrors(ObjectType ddgModelObjectType, IBusinessRuleOperationResultCollection operationResults)
//        {
//            // Get entity operation list with failed status
//            List<BusinessRuleOperationResult> objOperationList = (from operationResult in operationResults
//                                                                  where operationResult.OperationResultStatus == OperationResultStatusEnum.Failed
//                                                                  || operationResult.OperationResultStatus == OperationResultStatusEnum.CompletedWithWarnings
//                                                                  || operationResult.OperationResultStatus == OperationResultStatusEnum.CompletedWithErrors
//                                                                  select operationResult).ToList();

//            BusinessRuleOperationResultCollection objErrorCollection = new BusinessRuleOperationResultCollection();


//            if ((objOperationList != null) && (objOperationList.Count() > 0))
//            {
//                foreach (BusinessRuleOperationResult item in objOperationList)
//                {
//                    item.DDGObjectType = ddgModelObjectType;
//                    objErrorCollection.Add(item);
//                }
//            }

//            _jobImportResultHandler.Save(objErrorCollection, true);
//        }

//        /// <summary>
//        /// Update job level Operation Result
//        /// </summary>
//        /// <param name="ddgObjectType">Indicates the DDG Object type</param>
//        /// <param name="ddgObjects">Indicates the DDG object collection.</param>
//        /// <param name="operationResults">Indicates the DDG operation result collection.</param>
//        private void UpdateOperationResult(ObjectType ddgObjectType, IBusinessRuleObjectCollection ddgObjects, IBusinessRuleOperationResultCollection operationResults)
//        {
//            if (operationResults == null || operationResults.Count() == 0)
//            {
//                return;
//            }

//            foreach (IError error in operationResults.GetErrors())
//            {
//                _job.JobData.OperationResult.Errors.Add(new Error(error.ErrorCode, error.ErrorMessage));
//            }
//        }

//        /// <summary>
//        /// Update OperationResult Summary
//        /// </summary>
//        /// <param name="ddgObjectType">Indicates the DDG object type</param>
//        /// <param name="ddgObjects">Indicates the DDG object collection</param>
//        /// <param name="operationResults">Indicates the DDG operation result collection</param>
//        /// <param name="jobResultSummaryAction">Indicates the summary action as Create or Update</param>
//        private void UpdateOperationResultSummary(ObjectType ddgObjectType, IBusinessRuleObjectCollection ddgObjects, IBusinessRuleOperationResultCollection operationResults, ObjectAction jobResultSummaryAction)
//        {
//            DDGOperationResultSummary ddgOperationResultSummary;

//            if (!_ddgOperationResultSummaryDictionary.TryGetValue(ddgObjectType, out ddgOperationResultSummary))
//            {
//                String errorMessage = String.Format("DDGImportEngine.LogSummary DataModelSummaryDictionary miss for object type:[{0}]", ddgObjectType.ToString());
//                throw new MDMOperationException("", errorMessage, "DDGImportEngine", String.Empty, "LogSummary");
//            }

//            Int32 pending = 0;
//            Int32 succeeded = 0;
//            Int32 failed = 0;
//            Int32 completedWithWarnings = 0;
//            Int32 completedWithErrors = 0;
//            Int32 processed = 0;
//            Int32 total = 0;

//            // Update current ObjectType's current batch counts
//            if (operationResults == null)
//            {
//                if (ddgObjects != null)
//                {
//                    total += ddgObjects.Count; // Before starting the process ORCollection is null - just update total objects to process
//                }
//            }
//            else
//            {
//                total = 0;
//                pending = operationResults.Count(i => (i.OperationResultStatus == OperationResultStatusEnum.Pending));
//                succeeded = operationResults.Count(i => (i.OperationResultStatus == OperationResultStatusEnum.None || i.OperationResultStatus == OperationResultStatusEnum.Successful));
//                failed = operationResults.Count(i => (i.OperationResultStatus == OperationResultStatusEnum.Failed));
//                completedWithWarnings = operationResults.Count(i => i.OperationResultStatus == OperationResultStatusEnum.CompletedWithWarnings);
//                completedWithErrors = operationResults.Count(i => i.OperationResultStatus == OperationResultStatusEnum.CompletedWithErrors);
//                processed = succeeded + failed + completedWithErrors + completedWithWarnings;
//            }

//            // Update Job wide execution numbers
//            _job.JobData.ExecutionStatus.TotalElementsToProcess += total;
//            _job.JobData.ExecutionStatus.TotalElementsProcessed += processed;
//            _job.JobData.ExecutionStatus.TotalElementsFailed += failed;
//            _job.JobData.ExecutionStatus.TotalElementsSucceed += succeeded;
//            _job.JobData.ExecutionStatus.TotalElementsPartiallySucceed += completedWithWarnings;

//            // Update overall job status with running count.
//            UpdateJobStatus(_job.JobStatus, String.Format("Processing {0};", DDGDictionary.ObjectsDictionary[ddgObjectType]));

//            // Update Data Model 
//            ddgOperationResultSummary.UpdateSummaryCounts(total, pending, succeeded, failed, completedWithWarnings, completedWithErrors);
//            ddgOperationResultSummary.UpdateSummaryStatus();

//            _jobImportResultHandler.Save(new DDGOperationResultSummaryCollection() { ddgOperationResultSummary }, jobResultSummaryAction);
//        }

//        #endregion

//        #endregion Private Methods

//        #endregion Methods
//    }
//}
