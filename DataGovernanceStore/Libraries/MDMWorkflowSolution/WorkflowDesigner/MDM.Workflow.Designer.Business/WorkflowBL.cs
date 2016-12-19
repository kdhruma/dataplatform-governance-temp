using System;
using System.Activities;
using System.Activities.XamlIntegration;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Xml;

namespace MDM.Workflow.Designer.Business
{
    using MDM.BufferManager;
    using MDM.BusinessObjects;
    using MDM.BusinessObjects.Diagnostics;
    using MDM.ConfigurationManager.Business;
    using MDM.Core;
    using MDM.Core.Exceptions;
    using MDM.Utility;
    using MDM.Workflow.Designer.Data.SqlClient;
    using BO = MDM.BusinessObjects;
    using WFBO = MDM.BusinessObjects.Workflow;
    using MDM.CacheManager.Business;

    /// <summary>
    /// Provides business logic related functions for Workflow
    /// </summary>
    public class WorkflowBL : BusinessLogicBase
    {
        #region Fields

        private String _programName = "WorkflowBL";

        private TraceSettings _currentTraceSettings = null;

        private DiagnosticActivity _diagnosticActivity = null;

        #endregion

        #region Constructors

        /// <summary>
        /// Default Constructor
        /// </summary>
        public WorkflowBL()
        {
            _currentTraceSettings = MDMOperationContextHelper.GetCurrentTraceSettings();
        }

        #endregion

        #region Properties

        #endregion

        #region Methods

        private void ValidateActivities(WFBO.Workflow workflow, OperationResult operationResult, Type nativeSystemActivityBaseType)
        {
            foreach (WFBO.WorkflowVersion workflowVersion in workflow.WorkflowVersions)
            {
                if (workflowVersion.Action == ObjectAction.Create)
                {
                    ValidateActivities(GetActivities(workflowVersion.WorkflowDefinition, nativeSystemActivityBaseType), operationResult);
                }                
            }
        }

        private void ValidateActivities(Collection<Activity> workflowActivities, OperationResult operationResult)
        {
            Collection<String> duplicateNames = new Collection<String>();
            HashSet<String> names = new HashSet<String>();
            foreach (Activity activity in workflowActivities)
            {
                if (!names.Add(activity.DisplayName))
                {
                    duplicateNames.Add(activity.DisplayName);
                }
            }

            if (duplicateNames.Count > 0)
            {
                Object[] parameters = {String.Join(", ", duplicateNames)};
                String message = String.Format("Cannot create duplicate activity(s): {0}. The activity(s) with the specified name(s) already exist in the same workflow.", parameters);
                operationResult.AddOperationResult("", message, parameters, OperationResultType.Error);
            }
        }

        private Collection<Activity> GetActivities(string xaml, Type nativeSystemActivityBaseType)
        {
            Collection<Activity> worklflowActivityList = new Collection<Activity>();
            using (StringReader reader = new StringReader(xaml))
            using (XmlTextReader xmlReader = new XmlTextReader(reader))
            {
                Activity root = ActivityXamlServices.Load(xmlReader);

                //pass to a recursive function which populates the activity collection
                TraverseActivity(root, worklflowActivityList, nativeSystemActivityBaseType);
            }
            return worklflowActivityList;
        }

        private void TraverseActivity(Activity root, Collection<Activity> worklflowActivityList, Type nativeSystemActivityBaseType)
        {
            if (nativeSystemActivityBaseType.IsInstanceOfType(root))
            {
                worklflowActivityList.Add(root);
            }

            IEnumerable<Activity> activities = WorkflowInspectionServices.GetActivities(root);
            if (activities != null)
            {
                foreach (Activity activity in activities)
                {
                    TraverseActivity(activity, worklflowActivityList, nativeSystemActivityBaseType);
                }
            }
        }

        /// <summary>
        /// Get collection of all workflows
        /// </summary>
        /// <param name="callerContext">Context which tells which application/module called this API</param>
        /// <returns>return a collection of workflows</returns>
        public Collection<WFBO.Workflow> Get(BO.CallerContext callerContext)
        {
            //TO DO :: Implement the procedure
            throw new NotImplementedException("Functionality has not been implemented");

            //WorkflowDA workflowDataManager = new WorkflowDA();
            //return workflowDataManager.Get();
        }

        /// <summary>
        /// Get All Workflow Infomration
        /// Workflow Infomration such as WorkflowName, WorkflowVersion, ActivityName, ActivityAction
        /// </summary>
        /// <param name="callerContext">Indicates the name of Application and Module which invoked the API</param>
        /// <returns>WorkflowInfo Collection</returns>
        public WorkflowInfoCollection GetAllWorkflowInformation(CallerContext callerContext)
        {
            WorkflowInfoCollection workflowInfoCollection = new WorkflowInfoCollection();
            _diagnosticActivity = new DiagnosticActivity();

            try
            {
                #region Initialization

                if (_currentTraceSettings.IsBasicTracingEnabled)
                {
                    _diagnosticActivity.Start();
                    _diagnosticActivity.LogInformation("Loading WorkflowInformation from system.");
                }

                #endregion Initialization

                #region Load WorkflowInformation From Cache

                if (_currentTraceSettings.IsBasicTracingEnabled)
                {
                    _diagnosticActivity.LogInformation("Loading WorkflowInformation from cache.");
                }

                var bufferManager = new CacheBufferManager<WorkflowInfoCollection>(CacheKeyGenerator.GetWorkflowInfoCacheKey(), String.Empty);
                workflowInfoCollection = bufferManager.GetAllObjectsFromCache();

                if (_currentTraceSettings.IsBasicTracingEnabled)
                {
                    _diagnosticActivity.LogInformation("Loaded WorkflowInformation from cache.");
                }

                #endregion Load WorkflowInformation From Cache

                #region Load WorkflowInformation From DB

                if (workflowInfoCollection == null || workflowInfoCollection.Count < 1)
                {
                    if (callerContext == null)
                    {
                        if (_currentTraceSettings.IsTracingEnabled)
                        {
                            _diagnosticActivity.LogError("111846", "CallerContext cannot be null.");
                        }
                        throw new MDMOperationException("111846", "CallerContext cannot be null.", "MDM.Workflow.Designer.Business", String.Empty, String.Empty);
                    }

                    if (_currentTraceSettings.IsTracingEnabled)
                    {
                        _diagnosticActivity.LogInformation("Loading all WorkflowInformation from the database.");
                    }

                    DBCommandProperties dbcommand = DBCommandHelper.Get(callerContext, MDMCenterModuleAction.Read);

                    WorkflowDA workflowDA = new WorkflowDA();
                    workflowInfoCollection = workflowDA.GetAllWorkflowInformation(dbcommand);

                    if (_currentTraceSettings.IsTracingEnabled)
                    {
                        _diagnosticActivity.LogData("Loaded all WorkflowInformation from the database.", workflowInfoCollection.ToXml());
                    }

                    #region Update WorkflowInformation in Cache

                    if (workflowInfoCollection != null && workflowInfoCollection.Count > 0)
                    {
                        bufferManager.SetBusinessObjectsToCache(workflowInfoCollection, 5);

                        if (_currentTraceSettings.IsTracingEnabled)
                        {
                            _diagnosticActivity.LogInformation("WorkflowInformation updated to the cache.");
                        }
                    }
                    else
                    {
                        if (_currentTraceSettings.IsTracingEnabled)
                        {
                            _diagnosticActivity.LogInformation("There is no WorkflowInformation available in the database.");
                        }
                    }

                    #endregion Update WorkflowInformation in Cache
                }
                else
                {
                    if (_currentTraceSettings.IsBasicTracingEnabled)
                    {
                        _diagnosticActivity.LogInformation(String.Format("{0} WorkflowInformation loaded from cache.", workflowInfoCollection.Count));
                    }
                }

                #endregion Load WorkflowInformation From DB
            }
            finally
            {
                #region Stop Diagnostic

                if (_currentTraceSettings.IsBasicTracingEnabled)
                {
                    _diagnosticActivity.Stop();
                }

                #endregion Stop Diagnostic
            }

            return workflowInfoCollection;
        }

        /// <summary>
        /// Gets the workflow data by name
        /// </summary>
        /// <param name="workflowName">Short Name or Long Name of the workflow</param>
        /// <param name="callerContext">Context which tells which application/module called this API</param>
        /// <returns>Workflow Object</returns>
        public WFBO.Workflow GetWorkflowByName(String workflowName, BO.CallerContext callerContext)
        {
            //Get Command properties
            BO.DBCommandProperties command = DBCommandHelper.Get(callerContext, Core.MDMCenterModuleAction.Read);
            ICache cacheManager = CacheFactory.GetCache();

            // Create the cache key name for version object caching
            String cacheKeyName = CacheKeyGenerator.GetWorkflowCacheKey(workflowName);

            WFBO.Workflow workFlow = (WFBO.Workflow)cacheManager.Get(cacheKeyName); ;

            if (workFlow != null)
            {
                return workFlow;

            }

            WorkflowDA workflowDA = new WorkflowDA();
            workFlow = workflowDA.GetWorkflowByName(workflowName, command);
            cacheManager.Set(cacheKeyName, workFlow, DateTime.Now.AddHours(48.0));

            return workFlow;
        }

        /// <summary>
        /// Loads all the versions of a workflow, 
        /// versions are populates in version collection property of workflow object, passed as parameter
        /// </summary>
        /// <param name="workflow">workflow object, whose versions are needed</param>
        /// <param name="callerContext">Context which tells which application/module called this API</param>
        public void LoadVersions(WFBO.Workflow workflow, BO.CallerContext callerContext)
        {
            WorkflowVersionBL workflowVersionManager = new WorkflowVersionBL();
            workflow.WorkflowVersions = workflowVersionManager.Get(workflow.Id, callerContext);
        }

        /// <summary>
        /// Process workflows, based on Action
        /// </summary>
        /// <param name="workflows">workflows to process</param>
        /// <param name="loginUser">Logged in user name</param>
        /// <param name="callerContext">Context which tells which application/module called this API</param>
        /// <param name="operationResult">Object which collects results of the operation</param>
        /// <returns>Returns the newly created workflow version</returns>
        public WFBO.WorkflowVersion Process(Collection<WFBO.Workflow> workflows, String loginUser, CallerContext callerContext, OperationResult operationResult)
        {
            Type nativeSystemActivityBaseType = Type.GetType("MDM.Workflow.Activities.Core.MDMNativeSystemActivityBase,RS.MDM.Workflow.Activities.Core");

            foreach (WFBO.Workflow workflow in workflows)
            {
                ValidateActivities(workflow, operationResult, nativeSystemActivityBaseType);
            }
            operationResult.RefreshOperationResultStatus();
            if (operationResult.HasError)
            {
                return null;
            }

            WFBO.WorkflowVersion workflowVersion = new WFBO.WorkflowVersion();

            //Get Command properties
            BO.DBCommandProperties command = DBCommandHelper.Get(callerContext, Core.MDMCenterModuleAction.Create);

            WorkflowDA workflowDataManager = new WorkflowDA();
            workflowVersion = workflowDataManager.Process(workflows, loginUser, this._programName, command);

            ICache cacheManager = CacheFactory.GetCache();

            foreach (WFBO.Workflow workflow in workflows)
            {
                cacheManager.Remove(CacheKeyGenerator.GetWorkflowCacheKey(workflow.Name));
                cacheManager.Remove(CacheKeyGenerator.GetWorkflowVersionCacheKey(workflow.LatestVersion));
                cacheManager.Remove(CacheKeyGenerator.GetWorkflowVersionWithTrackingProfileCacheKey(workflow.LatestVersion));
            }

            CacheBufferManager<WorkflowInfoCollection> bufferManager = new CacheBufferManager<WorkflowInfoCollection>(CacheKeyGenerator.GetWorkflowInfoCacheKey(), String.Empty);
            bufferManager.RemoveBusinessObjectFromCache(publishCacheChangeEvent: true);

            return workflowVersion;
        }

        #endregion
    }
}
