using System;
using System.Activities;
using System.Activities.DurableInstancing;
using System.Activities.Tracking;
using System.Activities.XamlIntegration;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.DurableInstancing;
using System.Xml;

namespace MDM.WorkflowRuntimeEngine
{
    using MDM.CacheManager.Business;
    using MDM.ConfigurationManager.Business;
    using MDM.Core;
    using MDM.ExceptionManager;
    using MDM.Utility;
    using MDM.Workflow.Designer.Business;
    using MDM.Workflow.PersistenceManager.Business;
    using MDM.Workflow.TrackingManager.Business;
    using MDM.Workflow.Utility;
    using MDMBO = MDM.BusinessObjects;
    using MDMBOW = MDM.BusinessObjects.Workflow;
    using MDM.BusinessObjects.Diagnostics;

    /// <summary>
    /// Workflow runtime class which encompasses the methods related to invoking / resuming the workflow
    /// </summary>
    public class WorkflowRuntime
    {
        #region Private Fields

        /// <summary>
        /// The instance Id of workflow instance
        /// </summary>
        private Guid _instanceID = Guid.Empty;

        /// <summary>
        /// Allows workflows to persist state information about workflow instances in a SQL Server 
        /// </summary>
        private SqlWorkflowInstanceStore _instanceStore;

        /// <summary>
        /// Input parameters for the workflow.
        /// Used when needed to pass some parameters to workflow while invoking it.
        /// </summary>
        private Dictionary<String, Object> _inputParameters = new Dictionary<string, object>();

        /// <summary>
        /// Field denoting CallerContext  : Who called API
        /// </summary>
        private MDMBO.CallerContext _callerContext = null;

        private TraceSettings _currentTraceSettings = null;

        #endregion Private Fields

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public WorkflowRuntime()
        {
            _currentTraceSettings = MDMOperationContextHelper.GetCurrentTraceSettings();
        }

        #endregion Constructor

        #region Public properties

        /// <summary>
        /// Represents the InstanceID of running workflow instance.
        /// This is readonly property. It will be assigned the value once instance is started.
        /// </summary>
        public Guid InstanceID
        {
            get
            {
                return this._instanceID;
            }
        }

        /// <summary>
        /// Property denoting denoting CallerContext  : Who called API
        /// </summary>
        public MDMBO.CallerContext CallerContext
        {
            get { return _callerContext; }
            set { _callerContext = value; }
        }

        #endregion Public properties

        #region Methods

        #region Public Methods

        /// <summary>
        /// This method is called when workflow instance is invoked for the first time.
        /// </summary>
        /// <param name="workflowVersion"></param>
        /// <param name="inputParameters"></param>
        /// <param name="operationResult"></param>
        /// <param name="callerContext"></param>
        /// <returns></returns>
        public Guid StartWorkflow(MDMBOW.WorkflowVersion workflowVersion, Dictionary<String, Object> inputParameters, MDMBO.OperationResult operationResult, MDMBO.CallerContext callerContext)
        {
            Guid instanceId = Guid.Empty;
            WorkflowApplication wfapp = null;
            DiagnosticActivity diagnosticActivity = null;
            DurationHelper durationHelper = null;

            if (_currentTraceSettings.IsBasicTracingEnabled)
            {
                diagnosticActivity = MDMTraceHelper.CreateTraceActivity(MDMTraceSource.AdvancedWorkflow, "WorkflowRuntime.StartWorkflow");
                diagnosticActivity.Start();
                durationHelper = new DurationHelper(DateTime.Now);

            }

            try
            {
                this._inputParameters = inputParameters;
                this._callerContext = callerContext;

                //Create WorkflowApplication object to hold workflow object and instantiate.
                wfapp = new WorkflowApplication(workflowVersion.WorkflowDefinitionActivity, inputParameters);

                wfapp = StartWorkflowInstance(wfapp, workflowVersion.WorkflowDefinition, workflowVersion.TrackingProfileObject, inputParameters, operationResult, ref instanceId, callerContext);
            }
            catch (Exception ex)
            {
                RetryStartWorkflowInstance(wfapp, workflowVersion.WorkflowDefinition, workflowVersion.TrackingProfileObject, inputParameters, operationResult, ref instanceId, callerContext, ex);
            }
            finally
            {
                if (_currentTraceSettings.IsBasicTracingEnabled)
                {
                    if (durationHelper != null)
                    {
                        diagnosticActivity.LogMessageWithDuration(MessageClassEnum.Information, "", "WorkflowRuntime.StartWorkflow", durationHelper.GetCumulativeTimeSpanInMilliseconds());
                    }
                    if (diagnosticActivity != null)
                    {
                        diagnosticActivity.Stop();
                    }
                }
            }

            return instanceId;
        }

        /// <summary>
        /// This method is called when workflow instance is invoked for the first time.
        /// </summary>
        /// <param name="workflowVersion"></param>
        /// <param name="inputParameters"></param>
        /// <param name="operationResult"></param>
        /// <param name="instanceId"></param>
        /// <param name="callerContext"></param>
        /// <returns></returns>
        public WorkflowApplication StartWorkflow(MDMBOW.WorkflowVersion workflowVersion, Dictionary<String, Object> inputParameters, MDMBO.OperationResult operationResult, ref Guid instanceId, MDMBO.CallerContext callerContext)
        {
            WorkflowApplication wfapp = null;
            try
            {
                this._inputParameters = inputParameters;
                this._callerContext = callerContext;

                //Create WorkflowApplication object to hold workflow object and instantiate.
                wfapp = new WorkflowApplication(workflowVersion.WorkflowDefinitionActivity, inputParameters);

                wfapp = StartWorkflowInstance(wfapp, workflowVersion.WorkflowDefinition, workflowVersion.TrackingProfileObject, inputParameters, operationResult, ref instanceId, callerContext);
            }
            catch (Exception ex)
            {
                RetryStartWorkflowInstance(wfapp, workflowVersion.WorkflowDefinition, workflowVersion.TrackingProfileObject, inputParameters, operationResult, ref instanceId, callerContext, ex);
            }

            //return this._instanceID;
            return wfapp;
        }
        
        /// <summary>
        /// Resume the bookmark.
        /// When workflow is persisted and invoked after some time, using the InstanceId and bookmark, the wf is resumed back.
        /// </summary>
        /// <param name="wfVersion">
        ///     The workflow definition.
        /// </param>
        /// <param name="currentActivityName">
        ///     Current activity after which the bookmark is resumed. 
        ///     For e.g., if the wf is waiting for user input on approval activity, then the bookmark name will contain Approval activity + object type + object id
        ///     So when user acts on the Approval activity, Approval activity name will be passed to resume bookmark method along with mdm object info.
        ///     then the method will create the proper bookmark name and resume the bookmark.
        /// </param>
        /// <param name="workflowInstanceId">
        ///     Instance Id of workflow which is to be resumed after Wf has been idle
        /// </param>
        /// <param name="valueToPassToBookmarkResumeMethos">
        ///     When wf instance is resumed, it calls a callback method which can 
        ///     take input parameter which can be used in the activity.
        /// </param>
        /// <param name="workflowMDMObjects">Indicates for which workflow objects need to resume bookmark</param>
        /// <param name="callerContext">Context which tells which application/module called this API</param>
        public Boolean ResumeBookmark(MDMBOW.WorkflowVersion wfVersion, String currentActivityName, String workflowInstanceId, object valueToPassToBookmarkResumeMethos, MDMBOW.WorkflowMDMObjectCollection workflowMDMObjects, MDMBO.OperationResult operationResult, MDMBO.CallerContext callerContext)
        {
            Boolean result = false;

            try
            {
                ResumeBookmark(wfVersion, currentActivityName, workflowInstanceId, valueToPassToBookmarkResumeMethos, workflowMDMObjects, operationResult, ref result, callerContext);
            }
            catch (InstanceLockedException ex)
            {
                String errorMessage = "The requested workflow is under process by another user for some of the entities. Please wait for some time. This action might have aborted the workflow for those entities. In that case resume the workflow to proceed.";
                RetryResumeBookmark(wfVersion, currentActivityName, workflowInstanceId, valueToPassToBookmarkResumeMethos, workflowMDMObjects, operationResult, ref result, callerContext, ex, errorMessage);
            }
            catch (Exception ex)
            {
                RetryResumeBookmark(wfVersion, currentActivityName, workflowInstanceId, valueToPassToBookmarkResumeMethos, workflowMDMObjects, operationResult, ref result, callerContext, ex);
            }

            //Return the result
            return result;
        }
        
        /// <summary>
        /// Terminates instance
        /// </summary>
        /// <param name="workflowVersion"></param>
        /// <param name="workflowInstanceId"></param>
        /// <param name="operationResult"></param>
        /// <param name="callerContext"></param>
        /// <returns></returns>
        public Boolean TerminateInstance(MDMBOW.WorkflowVersion workflowVersion, String workflowInstanceId, MDMBO.OperationResult operationResult, MDMBO.CallerContext callerContext)
        {
            Boolean result = false;

            try
            {
                this._callerContext = callerContext;

                //Load Instance
                WorkflowApplication wfapp = LoadInstance(workflowVersion, workflowInstanceId);

                //Terminate the workflow
                wfapp.Terminate("Workflow Instance has been terminated manually by Workflow Marshal.");

                result = true;
            }
            catch (InstanceLockedException ex)
            {
                MDMBO.Error error = new MDMBO.Error();
                error.ErrorMessage = "The requested workflow is under process by another user for some of the entities. Please wait for some time. This action might have aborted the workflow for those entities. In that case resume the workflow to proceed.";
                operationResult.Errors.Add(error);

                //Log exception
                this.LogException(ex);
            }
            catch (Exception ex)
            {
                MDMBO.Error error = new MDMBO.Error();
                error.ErrorMessage = ex.Message;
                operationResult.Errors.Add(error);

                //Log exception
                this.LogException(ex);
            }

            return result;
        }

        /// <summary>
        ///  Promotes requested instance
        /// </summary>
        /// <param name="workflowVersion"></param>
        /// <param name="workflowInstanceId"></param>
        /// <param name="operationResult"></param>
        /// <param name="callerContext"></param>
        /// <returns></returns>
        public Boolean PromoteInstance(MDMBOW.WorkflowVersion workflowVersion, String workflowInstanceId, MDMBO.OperationResult operationResult, MDMBO.CallerContext callerContext)
        {
            Boolean result = false;

            try
            {
                this._callerContext = callerContext;

                //Load Instance
                WorkflowApplication wfapp = LoadInstance(workflowVersion, workflowInstanceId);

                //Promote is nothing but canceling the remaining activities and completing the workflow.
                //Canceling workflow.. Status is overridden while tracking from Canceled to Completed
                wfapp.Cancel();

                result = true;
            }
            catch (InstanceLockedException ex)
            {
                MDMBO.Error error = new MDMBO.Error();
                error.ErrorMessage = "The requested workflow is under process by another user for some of the entities. Please wait for some time. This action might have aborted the workflow for those entities. In that case resume the workflow to proceed.";
                operationResult.Errors.Add(error);

                //Log exception
                this.LogException(ex);
            }
            catch (Exception ex)
            {
                MDMBO.Error error = new MDMBO.Error();
                error.ErrorMessage = ex.Message;
                operationResult.Errors.Add(error);

                //Log exception
                this.LogException(ex);
            }

            return result;
        }

        /// <summary>
        /// Resume Aborted Instance of workflow
        /// </summary>
        /// <param name="workflowVersion"></param>
        /// <param name="workflowInstanceId"></param>
        /// <param name="operationResult"></param>
        /// <param name="callerContext"></param>
        /// <returns></returns>
        public Boolean ResumeAbortedInstance(MDMBOW.WorkflowVersion workflowVersion, String workflowInstanceId, MDMBO.OperationResult operationResult, MDMBO.CallerContext callerContext)
        {
            Boolean result = false;

            try
            {
                this._callerContext = callerContext;

                //Load Instance
                WorkflowApplication wfapp = LoadInstance(workflowVersion, workflowInstanceId);

                //Resume the execution of workflow
                wfapp.Run();

                result = true;
            }
            catch (Exception ex)
            {
                MDMBO.Error error = new MDMBO.Error();
                error.ErrorMessage = ex.Message;
                operationResult.Errors.Add(error);

                //Log exception
                this.LogException(ex);
            }

            return result;
        }

        /// <summary>
        /// Loads requested instance into memory from persistence
        /// </summary>
        /// <param name="workflowVersion">Object containing workflow version details</param>
        /// <param name="runtimeInstanceId">Instance Id of the workflow</param>
        /// <param name="callerContext">Indicates Application and Module name by which action is being performed</param>
        /// <returns>Flag indicating whether Load is successful or not</returns>
        public Boolean LoadInstance(MDMBOW.WorkflowVersion workflowVersion, String runtimeInstanceId, MDMBO.CallerContext callerContext)
        {
            Boolean result = false;

            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StartTraceActivity("WorkflowRuntime.LoadInstances", false);

            try
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("Request is to load instance: {0}", runtimeInstanceId));

                this._callerContext = callerContext;

                WorkflowApplication wfApp = LoadInstance(workflowVersion.WorkflowDefinition, workflowVersion.TrackingProfileObject, runtimeInstanceId);

                if (wfApp != null)
                {
                    wfApp.Run();
                    if (Constants.TRACING_ENABLED)
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Load is successful.");
                    result = true;
                }
                else
                {
                    if (Constants.TRACING_ENABLED)
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Failed to load.");
                }
            }
            finally
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StopTraceActivity("WorkflowRuntime.LoadInstances");
            }

            return result;
        }

        /// <summary>
        /// Loads elapsed timer instances in to memory from persistence
        /// </summary>
        /// <param name="callerContext">Indicates Application and Module name by which action is being performed</param>
        /// <returns>Flag indicating whether Load is successful or not</returns>
        public Boolean LoadElapsedTimerInstances(MDMBO.CallerContext callerContext)
        {
            Boolean result = false;
            WorkflowInstanceBL workflowInstanceManager = null;
            Collection<MDMBOW.WorkflowInstance> workflowInstances = null;

            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StartTraceActivity("WorkflowRuntime.LoadElapsedTimerInstances", false);

            try
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Getting elapsed timer instances..");

                InstanceTrackingBL instanceTrackingManager = new InstanceTrackingBL();
                Collection<String> elapsedTimerInstances = instanceTrackingManager.GetElapsedTimerInstances();

                String commaSeparatedInstanceIds = ValueTypeHelper.JoinCollection(elapsedTimerInstances, ",");

                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Elapsed timer instances get completed.");

                if (!String.IsNullOrWhiteSpace(commaSeparatedInstanceIds))
                {
                    if (Constants.TRACING_ENABLED)
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("Request is to load instances: {0}", commaSeparatedInstanceIds));

                    #region Get Instance Details

                    if (Constants.TRACING_ENABLED)
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Getting details of instances..");

                    workflowInstanceManager = new WorkflowInstanceBL();
                    workflowInstances = workflowInstanceManager.GetByRuntimeInstanceIds(commaSeparatedInstanceIds, callerContext);

                    if (workflowInstances == null || workflowInstances.Count < 1)
                    {
                        if (Constants.TRACING_ENABLED)
                            MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Failed to get instance details. Exiting..");
                        return result;
                    }
                    else
                    {
                        if (Constants.TRACING_ENABLED)
                            MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Instance details get completed.");
                    }

                    #endregion

                    foreach (MDMBOW.WorkflowInstance wfInstance in workflowInstances)
                    {
                        if (wfInstance.Status.Equals("Completed"))
                        {
                            continue;
                        }

                        if (Constants.TRACING_ENABLED)
                            MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("Getting version details for version '{0}' of the instance '{1}'", wfInstance.WorkflowVersionId, wfInstance.RuntimeInstanceId));

                        MDMBOW.WorkflowVersion wfVersion = GetVersionDetails(wfInstance.WorkflowVersionId, false, callerContext);

                        if (wfVersion != null)
                        {
                            if (Constants.TRACING_ENABLED)
                                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Version details get completed.");

                            WorkflowRuntime wfRuntime = new WorkflowRuntime();
                            result = wfRuntime.LoadInstance(wfVersion, wfInstance.RuntimeInstanceId, callerContext);
                        }
                        else
                        {
                            if (Constants.TRACING_ENABLED)
                                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("Failed to get version details for instance '{0}'.", wfInstance.RuntimeInstanceId));
                        }
                    }
                }
                else
                {
                    if (Constants.TRACING_ENABLED)
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "No instances are available to load. Exiting..");
                }
            }
            catch (Exception ex)
            {
                this.LogException(ex);
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, ex.Message);
            }
            finally
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StopTraceActivity("WorkflowRuntime.LoadElapsedTimerInstances");
            }

            return result;
        }

        #endregion Public Methods

        #region Private methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xamlFile"></param>
        /// <param name="trackingProfileObject"></param>
        /// <param name="workflowInstanceId"></param>
        /// <returns></returns>
        private WorkflowApplication LoadInstance(String xamlFile, TrackingProfile trackingProfileObject, String workflowInstanceId)
        {
            var wfVersion = new MDMBOW.WorkflowVersion{ WorkflowDefinition =  xamlFile, TrackingProfileObject = trackingProfileObject };

            return LoadInstance(wfVersion, workflowInstanceId);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="wfVersion"></param>
        /// <param name="workflowInstanceId"></param>
        /// <returns></returns>
        private WorkflowApplication LoadInstance(MDMBOW.WorkflowVersion wfVersion, String workflowInstanceId)
        {
            WorkflowApplication wfapp = null;
            #region Convert string WorkflowInstanceID into Guid

            //Convert string workflow instance id into Guid workflow instance id
            Guid guidWorkflowInstanceId = Guid.Empty;

            try
            {
                guidWorkflowInstanceId = Guid.Parse(workflowInstanceId);
            }
            catch (Exception ex)
            {
                throw new ArgumentException("WorkflowInstanceId is not a proper Guid.", workflowInstanceId, ex.InnerException);
            }

            #endregion Convert string WorkflowInstanceID into Guid

            // Resume the bookmark only if Guid instance id is available
            if (!guidWorkflowInstanceId.Equals(Guid.Empty))
            {
                //Create Workflow object (Activity) from XML String containing workflow definition
                Activity wf = wfVersion.WorkflowDefinitionActivity;  

                wfapp = new WorkflowApplication(wf);

                //On Unhandled Exception, default behavior of the runtime is to terminate workflow which will remove running instance context
                //from in memory as well as from persistence layer which will result in loosing of last persisted point.
                //Since we do not want to loose, we are overriding default behavior to Abort.
                //Abort will just remove the context from in memory leaving the persistence as it is which can be resumed after solving error.
                wfapp.OnUnhandledException = delegate(WorkflowApplicationUnhandledExceptionEventArgs e)
                {
                    // Instruct the runtime to abort the workflow.
                    return UnhandledExceptionAction.Abort;
                };

                wfapp.Aborted = delegate(WorkflowApplicationAbortedEventArgs e)
                {
                    //Log Abort Reason
                    String abortReason = String.Format("The instance {0} has been aborted. Reason: {1}", e.InstanceId.ToString(), e.Reason);
                    this.LogException(new Exception(abortReason));
                };

                SetupInstance(wfapp);

                //Setup tracking profile and tracking participant for record tracking
                SetupTracking(wfapp, wfVersion.TrackingProfileObject);

                //Resume persisted workflow instance using WorkflowInstanceId which is Guid.
                wfapp.Load(guidWorkflowInstanceId);
            }
            else
            {
                throw new Exception("WorkflowInstanceId is empty.");
            }

            return wfapp;
        }

        /// <summary>
        /// Configure the instance store for WorkflowApplication object.
        /// </summary>
        /// <param name="wfapp"></param>
        private void SetupInstance(WorkflowApplication wfapp)
        {
            Boolean serverSplitEnabled = false;
            String strSplitEnabled = "false";
            String connectionString = String.Empty;

            try
            {
                strSplitEnabled = AppConfigurationHelper.GetAppConfig<String>("MDMCenter.PhysicalServerSplit.Enabled");
            }
            catch (Exception ex)
            {
                LogException(ex);
            }
            
            DiagnosticActivity diagnosticActivity = null;

            if (_currentTraceSettings.IsBasicTracingEnabled)
            {
                diagnosticActivity = MDMTraceHelper.CreateTraceActivity(MDMTraceSource.AdvancedWorkflow, "WorkflowRuntimeBL.SetupInstance");
                diagnosticActivity.Start();
            }


            try
            {
                serverSplitEnabled = ValueTypeHelper.ConvertToBoolean(strSplitEnabled);

                //If server split is enabled, take connection string from db else take it from separate WF connection string.
                if (serverSplitEnabled == true)
                {
                    /*
                     * For DBCommanProperties, we are hard-coding application, module and action here.
                     * The reason behind this is, "SqlWorkflowInstanceStore" is used by WWF internals for their own tracking. So these tracking has to be offloaded to another server.
                     * The connection string for this has to be fetched from tb_ConnectionString table based on the configuration.
                     */

                    //Get Connection String from configuration.
                    MDMBO.DBCommandProperties command = DBCommandHelper.Get(Core.MDMCenterApplication.WindowsWorkflow, Core.MDMCenterModules.WindowsWorkflow, Core.MDMCenterModuleAction.Read);
                    connectionString = command.ConnectionString;
                }
                else
                {
                    connectionString = System.Configuration.ConfigurationManager.AppSettings.Get("MSWorkflowConnectionString");
                }

                //Create instance store for SQL server persistence
                _instanceStore = new SqlWorkflowInstanceStore(connectionString);
                _instanceStore.RunnableInstancesDetectionPeriod = new TimeSpan(24, 0, 0);
                _instanceStore.InstanceEncodingOption = InstanceEncodingOption.None;

                #region Assigning Owner to Instance - Commented

                //Instance owning is required when multiple hosts tries to access runtime..
                //In our case, we are having a single service host which will interact with the runtime
                //Also, assigning the owner to an instance will try to extend the lock even though instance- 
                //is done with its job and no longer requires the lock. This extend lock process is a overhead
                //and not serving any purpose as per now.

                //If we are going for multiple hosts then Instance Owner may serve a purpose and we need to think 
                //twice before setting the owner..

                //First Method
                //--------------

                //InstanceHandle instanceHandle = _instanceStore.CreateInstanceHandle();
                //CreateWorkflowOwnerCommand createWFOwnerCmd = new CreateWorkflowOwnerCommand();

                //InstanceView view = _instanceStore.Execute(instanceHandle, createWFOwnerCmd, TimeSpan.FromSeconds(30));
                //_instanceStore.DefaultInstanceOwner = view.InstanceOwner;

                //Second Method
                //----------------

                //IAsyncResult result = _instanceStore.BeginExecute(instanceHandle, createWFOwnerCmd, TimeSpan.FromSeconds(90), null, null);
                //InstanceView view = _instanceStore.EndExecute(result);
                //_instanceStore.DefaultInstanceOwner = view.InstanceOwner;

                #endregion

                // Setup the instance store
                wfapp.InstanceStore = _instanceStore;

                // Setup the PersistableIdle event handler
                // Possible values:
                // None	: Specifies that no action is taken.
                // Unload : Specifies that the WorkflowApplication should persist and unload the workflow.
                // Persist : Specifies that the WorkflowApplication should persist the workflow.
                wfapp.PersistableIdle = (waiea) =>
                {
                    return PersistableIdleAction.Unload;
                };
            }
            finally
            {
                if (_currentTraceSettings.IsBasicTracingEnabled)
                {
                    if (diagnosticActivity != null)
                    {
                        diagnosticActivity.Stop();
                    }
                }

            }

        }

        /// <summary>
        /// Setup tracking profile and tracking participant for record tracking
        /// </summary>
        /// <param name="wfapp">WorkflowApplication object - workflow instance</param>
        /// <param name="trackingProfileObject">Tracking Profile</param>
        private void SetupTracking(WorkflowApplication wfapp, TrackingProfile trackingProfileObject)
        {
            DiagnosticActivity diagnosticActivity = null;

            if (_currentTraceSettings.IsBasicTracingEnabled)
            {
                diagnosticActivity = MDMTraceHelper.CreateTraceActivity(MDMTraceSource.AdvancedWorkflow, "WorkflowRuntimeBL.SetupTracking");
                diagnosticActivity.Start();
            }

            try
            {
                if (trackingProfileObject == null)
                    throw new Exception("Tracking Profile is not available. Please verify workflow version or web.config.");

                //Bind the tracking profile with the tracking participant
                CustomTrackingParticipant trackingParticipant = new CustomTrackingParticipant();
                trackingParticipant.TrackingProfile = trackingProfileObject;
                trackingParticipant.CallerContext = this._callerContext;

                //Add the tracking participant as an extension to the current workflow instance
                wfapp.Extensions.Add(trackingParticipant);
            } 
            finally
            {
                if (_currentTraceSettings.IsBasicTracingEnabled)
                {
                    if (diagnosticActivity != null)
                    {
                        diagnosticActivity.Stop();
                    }
                }

            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="versionId"></param>
        /// <param name="makeRoundTrip"></param>
        /// <param name="callerContext"></param>
        /// <returns></returns>
        public MDMBOW.WorkflowVersion GetVersionDetails(Int32 versionId, Boolean makeRoundTrip, MDMBO.CallerContext callerContext)
        {
            MDMBOW.WorkflowVersion wfVersion = null;

            ICache cacheManager = CacheFactory.GetCache();

            // Create the cache key name for version object caching
            String cacheKeyName = MDMBO.CacheKeyGenerator.GetWorkflowVersionWithTrackingProfileCacheKey(versionId);

            // Otherwise get the cached version
            if (!makeRoundTrip)
            {
                //Get the Workflow version from cache
                wfVersion = (MDMBOW.WorkflowVersion)cacheManager.Get(cacheKeyName);
            }

            if (wfVersion == null)
            {
                // If workflow version returned from cache IS NULL then take version detail from DB and put it into cache
                // cache is set to expire after 2 hr.
                wfVersion = GetWorkflowVersionDetailsFromDB(versionId, callerContext);

                if (wfVersion != null)
                {
                    //Load the tracking profile object
                    wfVersion.TrackingProfileObject = TrackingProfileLoader.LoadFromInputXML(wfVersion.TrackingProfile);

                    cacheManager.Set(cacheKeyName, wfVersion, DateTime.Now.AddHours(24.0));
                }
            }

            return wfVersion;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="versionId"></param>
        /// <param name="callerContext"></param>
        /// <returns></returns>
        private MDMBOW.WorkflowVersion GetWorkflowVersionDetailsFromDB(Int32 versionId, MDMBO.CallerContext callerContext)
        {
            MDMBOW.WorkflowVersion wfVersion = null;
            WorkflowVersionBL wfVersionBL = new WorkflowVersionBL();

            //Get the version object through Id
            wfVersion = wfVersionBL.GetById(versionId, callerContext);

            return wfVersion;
        }

        /// <summary>
        /// Resume the bookmark.
        /// When workflow is persisted and invoked after some time, using the InstanceId and bookmark, the wf is resumed back.
        /// </summary>
        /// <param name="wfVersion">The workflow definition.</param>
        /// <param name="currentActivityName">
        ///     Current activity after which the bookmark is resumed. 
        ///     For e.g., if the wf is waiting for user input on approval activity, then the bookmark name will contain Approval activity + object type + object id
        ///     So when user acts on the Approval activity, Approval activity name will be passed to resume bookmark method along with mdm object info.
        ///     then the method will create the proper bookmark name and resume the bookmark.
        /// </param>
        /// <param name="workflowInstanceId">Instance Id of workflow which is to be resumed after Wf has been idle</param>
        /// <param name="valueToPassToBookmarkResumeMethods">
        ///     When wf instance is resumed, it calls a callback method which can 
        ///     take input parameter which can be used in the activity.
        /// </param>
        /// <param name="workflowMDMObjects" />
        /// <param name="operationResult">Returns result as output paramater</param>
        /// <param name="callerContext">Context which tells which application/module called this API</param>
        private void ResumeBookmark(MDMBOW.WorkflowVersion wfVersion, String currentActivityName, String workflowInstanceId, object valueToPassToBookmarkResumeMethods, MDMBOW.WorkflowMDMObjectCollection workflowMDMObjects, MDMBO.OperationResult operationResult, ref Boolean result, MDMBO.CallerContext callerContext)
        {
            this._callerContext = callerContext;

            //Load Instance
            WorkflowApplication wfapp = LoadInstance(wfVersion, workflowInstanceId);

            //Get bookmark name based on activity name and mdm object information
            String bookmarkName = WorkflowHelper.GetBookmarkName(currentActivityName, workflowInstanceId);

            //Resume the bookmark in the loaded instance by passing the input parameters to the ResumeBookmark call back method
            BookmarkResumptionResult resumeResult = wfapp.ResumeBookmark(bookmarkName, valueToPassToBookmarkResumeMethods);

            if (resumeResult == BookmarkResumptionResult.Success)
                result = true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="wfVersion"></param>
        /// <param name="currentActivityName"></param>
        /// <param name="workflowInstanceId"></param>
        /// <param name="valueToPassToBookmarkResumeMethos"></param>
        /// <param name="workflowMDMObjects"></param>
        /// <param name="operationResult"></param>
        /// <param name="result"></param>
        /// <param name="callerContext"></param>
        /// <param name="ex"></param>
        /// <param name="errorMessage"></param>
        /// <param name="numberOfRetry"></param>
        private void RetryResumeBookmark(MDMBOW.WorkflowVersion wfVersion, String currentActivityName, String workflowInstanceId, object valueToPassToBookmarkResumeMethos, MDMBOW.WorkflowMDMObjectCollection workflowMDMObjects, MDMBO.OperationResult operationResult, ref Boolean result, MDMBO.CallerContext callerContext, Exception ex, String errorMessage = "", Int32 numberOfRetry = 3)
        {
            //Retry < 0 means just ignore resume bookmark if failed..
            if (numberOfRetry < 0)
            {
                return;
            }

            Boolean retrySuccess = false;

            for (int i = 0; i < numberOfRetry; i++)
            {
                try
                {
                    ResumeBookmark(wfVersion, currentActivityName, workflowInstanceId, valueToPassToBookmarkResumeMethos, workflowMDMObjects, operationResult, ref result, callerContext);
                    retrySuccess = true;
                    break;
                }
                catch (Exception exception)
                {
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, String.Format("Not able to Resume bookmark. Attempt {1}. Error occurred : {0}", exception.Message, i), MDMTraceSource.AdvancedWorkflow);
                }
            }

            if (!retrySuccess)
            {
                MDMBO.Error error = new MDMBO.Error();

                if (!String.IsNullOrEmpty(errorMessage))
                {
                    error.ErrorMessage = errorMessage;
                }
                else
                {
                    error.ErrorMessage = ex.Message;
                }

                operationResult.ExtendedProperties.Add(workflowMDMObjects.WorkflowMDMObjects.FirstOrDefault().MDMObjectId.ToString(), error);

                //Log exception
                this.LogException(ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="wfapp"></param>
        /// <param name="xamlDefinition"></param>
        /// <param name="trackingProfileObject"></param>
        /// <param name="inputParameters"></param>
        /// <param name="operationResult"></param>
        /// <param name="instanceId"></param>
        /// <param name="callerContext"></param>
        /// <returns></returns>
        private WorkflowApplication StartWorkflowInstance(WorkflowApplication wfapp, String xamlDefinition, TrackingProfile trackingProfileObject, Dictionary<String, Object> inputParameters, MDMBO.OperationResult operationResult, ref Guid instanceId, MDMBO.CallerContext callerContext)
        {
            DiagnosticActivity diagnosticActivity = null;
            DurationHelper durationHelper = null;

            if (_currentTraceSettings.IsBasicTracingEnabled)
            {
                diagnosticActivity = MDMTraceHelper.CreateTraceActivity(MDMTraceSource.AdvancedWorkflow, "WorkflowRuntime.StartWorkflowInstance");
                diagnosticActivity.Start();
                durationHelper = new DurationHelper(DateTime.Now);
            }

            try
            {
                //On Unhandled Exception, default behavior of the runtime is to terminate workflow which will remove running instance context
                //from in memory as well as from persistence layer which will result in loosing of last persisted point.
                //Since we do not want to loose, we are overriding default behavior to Abort.
                //Abort will just remove the context from in memory leaving the persistence as it is which can be resumed after solving error.
                wfapp.OnUnhandledException = delegate (WorkflowApplicationUnhandledExceptionEventArgs e)
                {
                // Instruct the runtime to abort the workflow.
                return UnhandledExceptionAction.Abort;
                };

                wfapp.Aborted = delegate (WorkflowApplicationAbortedEventArgs e)
                {
                //Log Abort Reason
                String abortReason = String.Format("The instance {0} has been aborted. Reason: {1}", e.InstanceId.ToString(), e.Reason);
                    this.LogException(new Exception(abortReason));
                };

                //Setup instance store for sql persistance.
                SetupInstance(wfapp);

                
                //Setup tracking profile and tracking participant for record tracking
                SetupTracking(wfapp, trackingProfileObject);

                //if (_currentTraceSettings.IsBasicTracingEnabled && (durationHelper != null))
                //{
                //    durationHelper.ResetDuration();
                //}

                //Before starting the workflow, Persist and continue
                wfapp.Persist();

                //if (_currentTraceSettings.IsBasicTracingEnabled && (diagnosticActivity != null) && (durationHelper != null))
                //{
                //    diagnosticActivity.LogMessageWithDuration(MessageClassEnum.Information, "", "WorkflowRuntime.StartWorkflowInstance:Persisted Workflow", durationHelper.GetCumulativeTimeSpanInMilliseconds());
                //    durationHelper.ResetDuration();
                //}

                //Start workflow
                wfapp.Run();

                //if (_currentTraceSettings.IsBasicTracingEnabled && (diagnosticActivity != null) && (durationHelper != null))
                //{
                //    diagnosticActivity.LogMessageWithDuration(MessageClassEnum.Information, "", "WorkflowRuntime.StartWorkflowInstance:Started Workflow", durationHelper.GetCumulativeTimeSpanInMilliseconds()); 
                //}

                //Get Guid of currently running workflow instance
                this._instanceID = wfapp.Id;
                instanceId = this._instanceID;
            }
            finally
            {
                if (_currentTraceSettings.IsBasicTracingEnabled && (diagnosticActivity != null) )
                {
                    if (durationHelper != null)
                    {
                        diagnosticActivity.LogMessageWithDuration(MessageClassEnum.Information, "", "WorkflowRuntime.StartWorkflowInstance", durationHelper.GetCumulativeTimeSpanInMilliseconds());
                    }
                    diagnosticActivity.Stop(); 
                }

            }


            return wfapp;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="wfapp"></param>
        /// <param name="xamlDefinition"></param>
        /// <param name="trackingProfileObject"></param>
        /// <param name="inputParameters"></param>
        /// <param name="operationResult"></param>
        /// <param name="instanceId"></param>
        /// <param name="callerContext"></param>
        /// <param name="ex"></param>
        /// <param name="numberOfRetry"></param>
        private void RetryStartWorkflowInstance(WorkflowApplication wfapp, String xamlDefinition, TrackingProfile trackingProfileObject, Dictionary<String, Object> inputParameters, MDMBO.OperationResult operationResult, ref Guid instanceId, MDMBO.CallerContext callerContext, Exception ex, Int32 numberOfRetry = 3)
        {
            var diagnosticActivity = new DiagnosticActivity();

            if (_currentTraceSettings.IsBasicTracingEnabled)
            {
                diagnosticActivity = new DiagnosticActivity(new MDMBO.ExecutionContext((MDMTraceSource.AdvancedWorkflow))); ;
                diagnosticActivity.Start();
            }

            try
            {
                //Retry < 0 means just ignore start workflow if failed..
                if (numberOfRetry < 0)
                {
                    return;
                }

                Boolean retrySuccess = false;

                for (Int32 i = 0; i < numberOfRetry; i++)
                {
                    try
                    {
                        StartWorkflowInstance(wfapp, xamlDefinition, trackingProfileObject, inputParameters, operationResult, ref instanceId, callerContext);

                        retrySuccess = true;
                        break;
                    }
                    catch (Exception innerException)
                    {
                        if (_currentTraceSettings.IsBasicTracingEnabled)
                        { 
                            diagnosticActivity.LogError(String.Format("Not able to Start workflow. Attempt {1}. Error occurred : {0}", innerException.Message, i));
                        }
                        else
                        {
                            diagnosticActivity = new DiagnosticActivity(new MDMBO.ExecutionContext((MDMTraceSource.AdvancedWorkflow)));
                            diagnosticActivity.Start();
                            diagnosticActivity.LogError(String.Format("Not able to Start workflow. Attempt {1}. Error occurred : {0}", innerException.Message, i));
                        }
                    }
                }

                //throw exception if cache update retry is failed too..
                if (!retrySuccess)
                {
                    MDMBO.Error error = new MDMBO.Error();
                    error.ErrorMessage = ex.Message;
                    operationResult.Errors.Add(error);

                    //Log exception
                    this.LogException(ex);
                }
            }
            finally
            {
                if (diagnosticActivity != null)
                { 
                    diagnosticActivity.Stop(); 
                }
            }

        }

        /// <summary>
        /// Logs the exception into Event Log
        /// </summary>
        /// <param name="ex">Exception occurred</param>
        private void LogException(Exception ex)
        {
            try
            {
                ExceptionHandler exceptionHandler = new ExceptionHandler(ex);
            }
            catch
            {
                //Do not throw
            }
        }

        #endregion Private methods

        #endregion Methods
    }
}
