using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Diagnostics;
using System.Linq;

namespace MDM.Workflow.PersistenceManager.Business
{
    using MDM.AdminManager.Business;
    using MDM.BusinessObjects;
    using MDM.ConfigurationManager.Business;
    using MDM.Core;
    using MDM.Core.Exceptions;
    using MDM.Core.Extensions;
    using MDM.MessageManager.Business;
    using MDM.PermissionManager.Business;
    using MDM.Utility;
    using MDM.Workflow.PersistenceManager.Data;
    using MDMBOW = MDM.BusinessObjects.Workflow;
    using MDM.BusinessObjects.Diagnostics;
    using MDM.Workflow.Utility;
    using MDM.BusinessObjects.Workflow;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// Provides business logic related functions for an Instance of the Workflow
    /// </summary>
    public class WorkflowInstanceBL : BusinessLogicBase
    {
        #region Fields

        /// <summary>
        /// Indicates the program name
        /// </summary>
        private String _programName = "WorkflowInstanceBL";

        /// <summary>
        /// Field denoting locale Message
        /// </summary>
        private LocaleMessage _localeMessage = null;

        /// <summary>
        /// Field denoting instance of Locale Message BL
        /// </summary>
        private LocaleMessageBL _localeMessageBL = new LocaleMessageBL();

        /// <summary>
        /// Field denoting the security principal
        /// </summary>
        private SecurityPrincipal _securityPrincipal = null;

        private  TraceSettings _currentTraceSettings = null;

        #endregion

        #region Constructors

        /// <summary>
        /// Parameter less constructor
        /// </summary>
        public WorkflowInstanceBL()
        {
            this._currentTraceSettings = MDMOperationContextHelper.GetCurrentTraceSettings();

        }

        /// <summary>
        /// Constructor with parameter as loadSecurityPrincipal flag.
        /// If flag is true , then security principal will be loaded.
        /// </summary>
        /// <param name="loadSecurityPrincipal">Indicates the boolean flag to load security principal or not </param>
        public WorkflowInstanceBL(Boolean loadSecurityPrincipal):this()
        {
            if (loadSecurityPrincipal)
            {
                GetSecurityPrincipal();
            }
        }


        #endregion

        #region Properties

        #endregion

        #region Methods

        #region Public Methods

        #region Runtime Methods

        public Collection<MDMBOW.WorkflowInstance> GetByWorkflow(Collection<MDMBOW.Workflow> workflowCollection)
        {
            WorkflowInstanceDA workflowInstanceDA = new WorkflowInstanceDA();
            return workflowInstanceDA.GetByWorkflow(workflowCollection);
        }

        public Collection<MDMBOW.WorkflowInstance> GetByMDMObjectInWorkflow(Int32 workflowId, MDMBOW.WorkflowMDMObject mdmObj, CallerContext callerContext)
        {
            //Get Command
            DBCommandProperties command = DBCommandHelper.Get(callerContext, Core.MDMCenterModuleAction.Read);

            WorkflowInstanceDA workflowInstanceDA = new WorkflowInstanceDA();
            return workflowInstanceDA.GetByMDMObjectInWorkflow(workflowId, mdmObj, command);
        }

        public Collection<MDMBOW.WorkflowInstance> GetByMDMObject(MDMBOW.WorkflowMDMObject mdmObj, String activityName, CallerContext callerContext)
        {
            //Get Command
            DBCommandProperties command = DBCommandHelper.Get(callerContext, Core.MDMCenterModuleAction.Read);

            WorkflowInstanceDA workflowInstanceDA = new WorkflowInstanceDA();
            return workflowInstanceDA.GetByMDMObject(mdmObj, activityName, command);
        }

        public Collection<MDMBOW.WorkflowInstance> GetByRuntimeInstanceIds(String commaSeparatedRuntimeInstanceIds, CallerContext callerContext)
        {
            DiagnosticActivity diagnosticActivity = null;

            if (_currentTraceSettings.IsBasicTracingEnabled)
            {
                diagnosticActivity = MDMTraceHelper.CreateTraceActivity(MDMTraceSource.AdvancedWorkflow, "WorkflowInstanceBL.GetByRuntimeInstanceIds");
                diagnosticActivity.Start();
            }

            Collection<MDMBOW.WorkflowInstance> runningInstanceIds = null;
            try
            {
                //Get Command
                DBCommandProperties command = DBCommandHelper.Get(callerContext, Core.MDMCenterModuleAction.Read);

                WorkflowInstanceDA workflowInstanceDA = new WorkflowInstanceDA();

                runningInstanceIds = workflowInstanceDA.GetByRuntimeInstanceIds(commaSeparatedRuntimeInstanceIds, command);

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

            return runningInstanceIds;
        }

        public Int32 CheckForRunningInstances(String mdmObjectIDs, String mdmObjectType, Int32 workflowID, CallerContext callerContext)
        {
            DiagnosticActivity diagnosticActivity = null;

            if (_currentTraceSettings.IsBasicTracingEnabled)
            {
                diagnosticActivity = MDMTraceHelper.CreateTraceActivity(MDMTraceSource.AdvancedWorkflow, "WorkflowInstanceBL.CheckForRunningInstances");
                diagnosticActivity.Start();
            }

            Int32 runningInstances = 0;

            //Get command
            try
            {
            DBCommandProperties command = DBCommandHelper.Get(callerContext, MDMCenterModuleAction.Read);

            WorkflowInstanceDA workflowInstanceDA = new WorkflowInstanceDA();

                runningInstances = workflowInstanceDA.CheckForRunningInstances(mdmObjectIDs, mdmObjectType, workflowID, command);
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

            return runningInstances;
        }

        public Collection<Int64> GetRunningInstanceDetails(String mdmObjectIDs, String mdmObjectType, Int32 workflowID, CallerContext callerContext)
        { 
            DiagnosticActivity diagnosticActivity = null;

            if (_currentTraceSettings.IsBasicTracingEnabled)
            {
                diagnosticActivity = MDMTraceHelper.CreateTraceActivity(MDMTraceSource.AdvancedWorkflow, "WorkflowInstanceBL.GetRunningInstanceDetails");
                diagnosticActivity.Start();
            }
            Collection<Int64> runningInstanceDetails = null;
            try
            {
            //Get command
            DBCommandProperties command = DBCommandHelper.Get(callerContext, MDMCenterModuleAction.Read);

                runningInstanceDetails = new WorkflowInstanceDA().GetRunningInstanceDetails(mdmObjectIDs, mdmObjectType, workflowID, command);
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


            return runningInstanceDetails;
        }

        public Int32 Process(Collection<MDMBOW.WorkflowInstance> workflowInstances, Int32 processingType, Int32 serviceId, String serviceType, String currentUserName, CallerContext callerContext)
        {
            DiagnosticActivity diagnosticActivity = null;

            if (_currentTraceSettings.IsBasicTracingEnabled)
            {
                diagnosticActivity = MDMTraceHelper.CreateTraceActivity(MDMTraceSource.AdvancedWorkflow, "WorkflowInstanceBL.Process");
                diagnosticActivity.Start();
            }
            Int32 instanceProcessed = 0;

            //Get Command
            try
            {
            DBCommandProperties command = DBCommandHelper.Get(callerContext, Core.MDMCenterModuleAction.Create);

            WorkflowInstanceDA workflowInstanceDA = new WorkflowInstanceDA();

                instanceProcessed = workflowInstanceDA.Process(workflowInstances, processingType, serviceId, serviceType, currentUserName, _programName, command);
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
            return instanceProcessed;
        }

        public Int32 Create(MDMBOW.WorkflowInstance workflowInstance, Int32 processingType, Int32 serviceId, String serviceType, String currentUserName, CallerContext callerContext)
        {
            Collection<MDMBOW.WorkflowInstance> workflowInstances = new Collection<MDMBOW.WorkflowInstance>();
            workflowInstances.Add(workflowInstance);
            return Process(workflowInstances, processingType, serviceId, serviceType, currentUserName, callerContext);
        }

        #endregion

        #region Workflow UI Methods

        /// <summary>
        /// Gets all the workflows, workflow versions and activities in the system
        /// </summary>
        /// <param name="workflowCollection"></param>
        /// <param name="workflowVersionCollection"></param>
        /// <param name="workflowActivityCollection"></param>
        /// <param name="callerContext">Context which tells which application/module called this API</param>
        public void GetAllWorkflowDetails(ref Collection<MDMBOW.Workflow> workflowCollection, ref Collection<MDMBOW.WorkflowVersion> workflowVersionCollection, ref Collection<MDMBOW.WorkflowActivity> workflowActivityCollection, CallerContext callerContext)
        {
            //Get Command
            DBCommandProperties command = DBCommandHelper.Get(callerContext, Core.MDMCenterModuleAction.Read);

            WorkflowInstanceDA workflowInstanceDA = new WorkflowInstanceDA();
            workflowInstanceDA.GetAllWorkflowDetails(ref workflowCollection, ref workflowVersionCollection, ref workflowActivityCollection, command);
        }

        /// <summary>
        /// Gets the summary of the instances as per the filter criteria
        /// </summary>
        /// <param name="workflowType"></param>
        /// <param name="workflowId"></param>
        /// <param name="workflowVersionId"></param>
        /// <param name="workflowStatus"></param>
        /// <param name="activityShortName"></param>
        /// <param name="roleIds"></param>
        /// <param name="userIds"></param>
        /// <param name="instanceId"></param>
        /// <param name="mdmObjectIds"></param>
        /// <param name="hasEscalation"></param>
        /// <param name="returnSize"></param>
        /// <param name="callerContext">Context which tells which application/module called this API</param>
        /// <returns>Collection of workflow instances</returns>
        public Collection<MDMBOW.WorkflowInstance> GetInstanceSummary(String workflowType, Int32 workflowId, Int32 workflowVersionId, String workflowStatus, String activityShortName, String roleIds, String userIds, String instanceId, String mdmObjectIds, Boolean? hasEscalation, Int32 returnSize, CallerContext callerContext)
        {
            //Get Command
            DBCommandProperties command = DBCommandHelper.Get(callerContext, Core.MDMCenterModuleAction.Read);

            Collection<MDMBOW.WorkflowInstance> workflowInstanceCollection = null;

            WorkflowInstanceDA workflowInstanceDA = new WorkflowInstanceDA();
            workflowInstanceCollection = workflowInstanceDA.GetInstanceSummary(workflowType, workflowId, workflowVersionId, workflowStatus, activityShortName, roleIds, userIds, instanceId, mdmObjectIds, hasEscalation, returnSize, command);

            return workflowInstanceCollection;
        }

        /// <summary>
        /// Gets the details of the requested instance
        /// </summary>
        /// <param name="workflowType"></param>
        /// <param name="instanceId"></param>
        /// <param name="runningActivityCollection">Collection of running activities</param>
        /// <param name="mdmObjectCollection">Collection of MDM objects participating in the instance</param>
        /// <param name="escalationCollection">Collection of escalations happened for this instance</param>
        /// <param name="callerContext">Context which tells which application/module called this API</param>
        public void GetInstanceDetails(String workflowType, String instanceId, ref Collection<MDMBOW.WorkflowActivity> runningActivityCollection, ref Collection<MDMBOW.WorkflowMDMObject> mdmObjectCollection, ref Collection<MDMBOW.Escalation> escalationCollection, CallerContext callerContext)
        {
            //Get Command
            DBCommandProperties command = DBCommandHelper.Get(callerContext, Core.MDMCenterModuleAction.Read);

            WorkflowInstanceDA workflowInstanceDA = new WorkflowInstanceDA();
            workflowInstanceDA.GetInstanceDetails(workflowType, instanceId, ref runningActivityCollection, ref mdmObjectCollection, ref escalationCollection, command);
        }

        /// <summary>
        /// Gets the statistics
        /// </summary>
        /// <param name="workflowType"></param>
        /// <param name="workflowId"></param>
        /// <param name="workflowVersionId"></param>
        /// <param name="callerContext">Context which tells which application/module called this API</param>
        /// <returns>Statistics in the format of string</returns>
        public String GetWorkflowStatistics(String workflowType, Int32 workflowId, Int32 workflowVersionId, CallerContext callerContext)
        {
            //Get Command
            DBCommandProperties command = DBCommandHelper.Get(callerContext, Core.MDMCenterModuleAction.Read);

            String strStatistics = String.Empty;

            WorkflowInstanceDA workflowInstanceDA = new WorkflowInstanceDA();
            strStatistics = workflowInstanceDA.GetWorkflowStatistics(workflowType, workflowId, workflowVersionId, command);

            return strStatistics;
        }

        /// <summary>
        /// Updates the instance status and instance's acting user.
        /// </summary>
        /// <param name="instanceGUIDs">Comma separated GUIDs of instances to be updated</param>
        /// <param name="mdmObjectID">Comma separated MDM object IDs</param>
        /// <param name="mdmObjectType">Type of the MDM object</param>
        /// <param name="workflowID">Workflow ID</param>
        /// <param name="activityShortName">Short Name of the activity</param>
        /// <param name="instanceStatus">Status which is going to be updated</param>
        /// <param name="loginUser">Login User which is going to be updated as Acting User</param>
        /// <param name="programName">Name of the module which is updating</param>
        /// <param name="callerContext">Context which tells which application/module called this API</param>
        /// <returns>Result of the operation</returns>
        public Boolean UpdateWorkflowInstances(String instanceGUIDs, String mdmObjectID, String mdmObjectType, Int32 workflowID, String activityShortName, String instanceStatus, String loginUser, CallerContext callerContext)
        {
            DiagnosticActivity diagnosticActivity = null;
            Boolean result = false;

            if (_currentTraceSettings.IsBasicTracingEnabled)
            {
                diagnosticActivity = MDMTraceHelper.CreateTraceActivity(MDMTraceSource.AdvancedWorkflow, "WorkflowInstanceBL.UpdateWorkflowInstances");
                diagnosticActivity.Start();
            }

            try
            {
                //Get Command
                DBCommandProperties command = DBCommandHelper.Get(callerContext, Core.MDMCenterModuleAction.Update);
               
                String modifiedUser = String.Empty;

                if (_securityPrincipal != null)
                {
                    modifiedUser = _securityPrincipal.CurrentUserName;
                }

                WorkflowInstanceDA workflowInstanceDA = new WorkflowInstanceDA();
                int rowsAffected = workflowInstanceDA.UpdateWorkflowInstances(instanceGUIDs, mdmObjectID, mdmObjectType, workflowID, activityShortName, instanceStatus, loginUser, modifiedUser, callerContext.ProgramName, command);

                if (rowsAffected > 0)
                    result = true;
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

            return result;
        }

        /// <summary>
        /// Gets the action buttons for the requested activity
        /// </summary>
        /// <param name="activityId">Id of an activity</param>
        /// <param name="toolBarButtonXML">Configuration XML for action buttons</param>
        /// <param name="loginUser">User login. Will be used during allowed users and roles check if enabled</param>
        /// <param name="checkAllowedUsersAndRoles">Set to True if you want to check activity AllowedUsers and AllowedRoles information or set to Null if you want default behavior</param>
        /// <param name="callerContext">Context which tells which application/module called this API</param>
        /// <returns>Buttons and there visibility in the format of table</returns>
        public DataTable GetActionButtons(Int32 activityId, String toolBarButtonXML, String loginUser, Boolean? checkAllowedUsersAndRoles, CallerContext callerContext)
        {
            //Get Command
            DBCommandProperties command = DBCommandHelper.Get(callerContext, Core.MDMCenterModuleAction.Read);

            DataTable toolBarButtonTable = null;

            WorkflowInstanceDA workflowInstanceDA = new WorkflowInstanceDA();
            toolBarButtonTable = workflowInstanceDA.GetActionButtons(activityId, toolBarButtonXML, loginUser, checkAllowedUsersAndRoles, command);

            return toolBarButtonTable;
        }

        /// <summary>
        /// Gets the action buttons for the requested activity
        /// </summary>
        /// <param name="activityId">Id of an activity</param>
        /// <param name="loginUser">User login. Will be used during allowed users and roles check if enabled</param>
        /// <param name="checkAllowedUsersAndRoles">Set to True if you want to check activity AllowedUsers and AllowedRoles information or set to Null if you want default behavior</param>
        /// <param name="callerContext">Context which tells which application/module called this API</param>
        /// <returns>Returns action buttons with comments details</returns>
        public Table GetActionButtons(Int32 activityId, String loginUser, Boolean? checkAllowedUsersAndRoles, CallerContext callerContext)
        {
            Table actionButtonsDetails = null;

            //Get command properties
            DBCommandProperties command = DBCommandHelper.Get(callerContext, MDMCenterModuleAction.Read);

            WorkflowInstanceDA workflowInstanceDA = new WorkflowInstanceDA();
            actionButtonsDetails = workflowInstanceDA.GetActionButtons(activityId, loginUser, checkAllowedUsersAndRoles, command);

            return actionButtonsDetails;
        }

        /// <summary>
        /// Gets the assignment buttons for the requested assignment status
        /// </summary>
        /// <param name="activityId">Activity Id for which assignment buttons has to get</param>
        /// <param name="assignmentStatus">AssignmentStatus</param>
        /// <param name="toolBarButtonXML">Configuration XML for assignment buttons</param>
        /// <param name="loginUser">Logged in User</param>
        /// <param name="checkAllowedUsersAndRoles">Set to True if you want to check activity AllowedUsers and AllowedRoles information or set to Null if you want default behavior</param>
        /// <param name="callerContext">Context which tells which application/module called this API</param>
        /// <returns>Buttons and there visibility in the format of table</returns>
        public DataTable GetAssignmentButtons(Int32 activityId, String assignmentStatus, String toolBarButtonXML, String loginUser, Boolean? checkAllowedUsersAndRoles, CallerContext callerContext)
        {
            //Get Command
            DBCommandProperties command = DBCommandHelper.Get(callerContext, Core.MDMCenterModuleAction.Read);

            DataTable toolBarButtonTable = null;

            WorkflowInstanceDA workflowInstanceDA = new WorkflowInstanceDA();
            toolBarButtonTable = workflowInstanceDA.GetAssignmentButtons(activityId, assignmentStatus, toolBarButtonXML, loginUser, checkAllowedUsersAndRoles, command);

            return toolBarButtonTable;
        }

        /// <summary>
        /// Gets the details required for the Workflow UI Panel
        /// </summary>
        /// <param name="orgId">Organization Id</param>
        /// <param name="catalogId">Id of the catalog for which details are required</param>
        /// <param name="userId">User Id for which details needs to be get</param>
        /// <param name="getSTFWorkflows">Get STF workflows data</param>
        /// <param name="getWWFWorkflows">Get WWF workflows data</param>
        /// <param name="showItemsAssignedToOtherUsers">Include data related to workflows assigned to other users</param>
        /// <param name="showEmptyItems">Do not hide empty nodes</param>
        /// <param name="callerContext">Context which tells which application/module called this API</param>
        /// <returns>Returns details in Table format</returns>
        public Table GetWorkflowPanelDetails(Int32 orgId, Int32 catalogId, Int32 userId, Boolean showEmptyItems, Boolean showItemsAssignedToOtherUsers, CallerContext callerContext)
        {
            Boolean isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;
            if (isTracingEnabled)
                MDMTraceHelper.StartTraceActivity("WorkflowInstanceManager.GetWorkflowPanelDetails", false);

            Table workflowPanelDetailsTable = null;
            Boolean getSearchResults = true;

            try
            {
                #region Validate Parameters

                if (orgId < 1)
                {
                    _localeMessage = _localeMessageBL.Get(GlobalizationHelper.GetSystemUILocale(), "111851", false, callerContext);
                    throw new MDMOperationException("111851", _localeMessage.Message, "WorkflowInstanceManager", String.Empty, "GetWorkflowPanelDetails");//Organization Id is not available. Please provide the organization Id for which workflow panel details are required.
                }

                if (catalogId < 1)
                {
                    _localeMessage = _localeMessageBL.Get(GlobalizationHelper.GetSystemUILocale(), "111852", false, callerContext);
                    throw new MDMOperationException("111852", _localeMessage.Message, "WorkflowInstanceManager", String.Empty, "GetWorkflowPanelDetails");//Container Id is not available. Please provide the container Id for which workflow panel details are required.
                }

                if (userId < 1)
                {
                    _localeMessage = _localeMessageBL.Get(GlobalizationHelper.GetSystemUILocale(), "111853", false, callerContext);
                    throw new MDMOperationException("111853", _localeMessage.Message, "WorkflowInstanceManager", String.Empty, "GetWorkflowPanelDetails");//User Id is not available. Please provide the user Id for which workflow panel details are required.
                }

                #endregion

                Collection<SearchAttributeRule> searchAttributeRules = new Collection<SearchAttributeRule>();

                if (isTracingEnabled)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("Starting determining attribute value permission for orgId : {0}, containerId {1}, and userId {2}", orgId, catalogId, userId));

                getSearchResults = DetermineAttributeValueBasedPermission(orgId, catalogId, userId, searchAttributeRules, callerContext);

                if (getSearchResults)
                {
                    //Get command properties
                    DBCommandProperties command = DBCommandHelper.Get(callerContext, MDMCenterModuleAction.Read);

                    WorkflowInstanceDA workflowInstanceDA = new WorkflowInstanceDA();
                    workflowPanelDetailsTable = workflowInstanceDA.GetWorkflowPanelDetails(catalogId, userId, searchAttributeRules, showEmptyItems, showItemsAssignedToOtherUsers, command);
                }
            }
            finally
            {
                if (isTracingEnabled)
                    MDMTraceHelper.StopTraceActivity("WorkflowInstanceManager.GetWorkflowPanelDetails");
            }

            return workflowPanelDetailsTable;
        }

        /// <summary>
        /// Gets the details required for the Workflow UI Panel
        /// </summary>
        /// <param name="catalogId">Id of the catalog for which details are required</param>
        /// <param name="userId">User Id for which details needs to be get</param>
        /// <param name="getSTFWorkflows">Get STF workflows data</param>
        /// <param name="getWWFWorkflows">Get WWF workflows data</param>
        /// <param name="showEmptyItems">Do not hide empty nodes</param>
        /// <param name="showItemsAssignedToOtherUsers">Include data related to workflows assigned to other users</param>
        /// <param name="callerContext">Context which tells which application/module called this API</param>
        /// <returns>Returns details in Table format</returns>
        public Table GetWorkflowPanelDetails(Int32 catalogId, Int32 userId, Boolean showEmptyItems, Boolean showItemsAssignedToOtherUsers, CallerContext callerContext)
        {
            Table workflowPanelDetailsTable = null;

            //Get command properties
            DBCommandProperties command = DBCommandHelper.Get(callerContext, MDMCenterModuleAction.Read);

            WorkflowInstanceDA workflowInstanceDA = new WorkflowInstanceDA();
            workflowPanelDetailsTable = workflowInstanceDA.GetWorkflowPanelDetails(catalogId, userId, showEmptyItems, showItemsAssignedToOtherUsers, command);

            return workflowPanelDetailsTable;
        }

        /// <summary>
        /// Gets role based work items details including workflow activities and business conditions for given entity id and user id
        /// </summary>
        /// <param name="entityId">Indicates entityId</param>
        /// <param name="callerContext">Indicates caller context</param>
        /// <returns>Returns a Table with all required data</returns>
        public Table GetWorkItemDetails(Int64 entityId, CallerContext callerContext)
        {
            Table workItemPanelDetailsTable = null;
            Int32 userId = -1;

            #region Diagnostics & tracing

            DiagnosticActivity diagnosticActivity = new DiagnosticActivity();
            var currentTraceSettings = MDMOperationContextHelper.GetCurrentTraceSettings();
            Boolean isTracingEnabled = currentTraceSettings.IsBasicTracingEnabled;

            if (isTracingEnabled)
            {
                diagnosticActivity.Start();
            }

            #endregion Diagnostics & tracing

            try
            {
                #region Validate Parameters

                if (callerContext == null)
                {
                    callerContext = new CallerContext();
                }

                if (String.IsNullOrWhiteSpace(callerContext.ProgramName))
                {
                    callerContext.ProgramName = "GetWorkItemDetails";
                }

                if (entityId < 1)
                {
                    _localeMessage = _localeMessageBL.Get(GlobalizationHelper.GetSystemUILocale(), "111645", false, callerContext);
                    throw new MDMOperationException("111645", _localeMessage.Message, "WorkflowInstanceManager", String.Empty, "GetWorkflowPanelDetails");//Entity Id Must be greater than 0.
                }

                #endregion Validate Parameters

                #region Load Work Items Details from database

                //Get command properties
                DBCommandProperties command = DBCommandHelper.Get(callerContext, MDMCenterModuleAction.Read);

                SecurityPrincipal securityUser = SecurityPrincipalHelper.GetCurrentSecurityPrincipal();
                if (securityUser != null)
                {
                    userId = securityUser.CurrentUserId;
                }

                WorkflowInstanceDA workflowInstanceDA = new WorkflowInstanceDA();
                workItemPanelDetailsTable = workflowInstanceDA.GetWorkItemDetails(entityId, userId, command);

                if (isTracingEnabled)
                {
                    diagnosticActivity.LogDurationInfo("WorkItemDetails database call get is completed.");
                }

                #endregion Load Work Items Details from database

                #region Publish WorkItems loaded event

                var eventArgs = new WorkItemsDataLoadedEventArgs(entityId, workItemPanelDetailsTable);
                EntityWorkflowEventManager.Instance.OnWorkItemsDataLoaded(eventArgs);

                if (isTracingEnabled)
                {
                    diagnosticActivity.LogDurationInfo("WorkItemDetails data loaded event completed.");
                }

                #endregion Publish WorkItems loaded eventt
            }
            finally
            {
                if (isTracingEnabled)
                {
                    diagnosticActivity.LogDurationInfo("WorkItemDetails Get is completed.");
                    diagnosticActivity.Stop();
                }
            }

            return workItemPanelDetailsTable;
        }

        /// <summary>
        /// Returns users list allowed for specified workflow activity
        /// </summary>
        /// <param name="workflowActivityId">Activity primary key value (optional)</param>
        /// <param name="activityShortName">Activity short name. Will be used if <paramref name="workflowActivityId"/> not specified</param>
        /// <param name="workflowVersionId">Workflow version Id. Will be used if <paramref name="workflowActivityId"/> not specified. Optional. If Null, then exist activity with maximal version will be used.</param>
        public Collection<SecurityUser> GetUsersAllowedForActivity(Int32? workflowActivityId, String activityShortName, Int32? workflowVersionId)
        {
            WorkflowInstanceDA workflowInstanceDA = new WorkflowInstanceDA();
            return workflowInstanceDA.GetUsersAllowedForActivity(workflowActivityId, activityShortName, workflowVersionId);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="catalogId"></param>
        /// <param name="userId"></param>
        /// <param name="showEmptyItems"></param>
        /// <param name="showItemsAssignedToOtherUsers"></param>
        /// <param name="showBusinessCondition"></param>
        /// <param name="callerContext"></param>
        /// <returns></returns>
        public Table GetWorkItemsPanelDetails(Int32 catalogId, Int32 userId, Boolean showEmptyItems, Boolean showItemsAssignedToOtherUsers, Boolean showBusinessCondition, CallerContext callerContext)
        {
            Boolean isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;
            if (isTracingEnabled)
            {
                MDMTraceHelper.StartTraceActivity("WorkflowInstanceManager.GetWorkItemsPanelDetails", false);
            }

            Table workItemsPanelDetailsTable = null;
            Boolean getSearchResults = true;

            try
            {
                #region Validate Parameters

                if (catalogId < 1)
                {
                    _localeMessage = _localeMessageBL.Get(GlobalizationHelper.GetSystemUILocale(), "111852", false, callerContext);
                    throw new MDMOperationException("111852", _localeMessage.Message, "WorkflowInstanceManager", String.Empty, "GetWorkItemsPanelDetails");//Container Id is not available. Please provide the container Id for which workflow panel details are required.
                }

                if (userId < 1)
                {
                    _localeMessage = _localeMessageBL.Get(GlobalizationHelper.GetSystemUILocale(), "111853", false, callerContext);
                    throw new MDMOperationException("111853", _localeMessage.Message, "WorkflowInstanceManager", String.Empty, "GetWorkItemsPanelDetails");//User Id is not available. Please provide the user Id for which workflow panel details are required.
                }

                #endregion

                if (isTracingEnabled)
                {
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("Starting determining attribute value permission for containerId {0}, and userId {1}", catalogId, userId));
                }

                if (getSearchResults)
                {
                    //Get command properties
                    DBCommandProperties command = DBCommandHelper.Get(callerContext, MDMCenterModuleAction.Read);

                    WorkflowInstanceDA workflowInstanceDA = new WorkflowInstanceDA();
                    workItemsPanelDetailsTable = workflowInstanceDA.GetWorkItemsPanelDetails(catalogId, userId, showEmptyItems, showItemsAssignedToOtherUsers, showBusinessCondition, command);
                }
            }
            finally
            {
                if (isTracingEnabled)
                    MDMTraceHelper.StopTraceActivity("WorkflowInstanceManager.GetWorkItemsPanelDetails");
            }

            return workItemsPanelDetailsTable;
        }


        #endregion

        #region Workflow Execution View Details Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityGUID"></param>
        /// <param name="workflowName"></param>
        /// <param name="callerContext"></param>
        /// <returns></returns>
        public JObject LoadWorkflowDetails(JObject requestJsonObject)
        {
            WorkflowStateCollection workflowStateCollection = null;
            JArray jArrayObj = null;
            JObject responseJsonObject = new JObject();
            JToken workflowNameJtoken = null;
            String workflowName = String.Empty;
            String status = "Success";

            #region Read input json

            Boolean returnRequest = ValueTypeHelper.BooleanTryParse(requestJsonObject["returnRequest"].ToString(), false);
            String entityGUID = requestJsonObject["dataObjects"][0]["id"].ToString();
            CallerContext callerContext = requestJsonObject["requestParams"]["callerContext"].ToObject<CallerContext>();

            workflowNameJtoken = requestJsonObject["requestParams"]["workflowName"];
            if (workflowNameJtoken != null)
            {
                workflowName = workflowNameJtoken.ToString();
            }

            #endregion Read input json

            workflowStateCollection = this.LoadWorkflowDetails(entityGUID, workflowName, callerContext);

            #region Prepare JSON output
            
            if (returnRequest)
            {
                responseJsonObject.Add(new JProperty("dataObjectOperationRequest", requestJsonObject));
            }

            if (workflowStateCollection != null && workflowStateCollection.Count > 0)
            {
                JObject dataObject = new JObject();
                dataObject.Add("id", entityGUID);

                JObject data = new JObject();
                dataObject.Add("data", data);

                data.Add("workflowStates", JArray.FromObject(workflowStateCollection));

                jArrayObj = new JArray();
                jArrayObj.Add(dataObject);
            }
            else
            {
                jArrayObj = new JArray();
            }

            responseJsonObject.Add(new JProperty("dataObjectOperationResponse", new JObject(
                                                                        new JProperty("status", status),
                                                                        new JProperty("dataObjects", jArrayObj))));

            #endregion Prepare JSON output

            return responseJsonObject;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityGUID"></param>
        /// <param name="workflowName"></param>
        /// <param name="callerContext"></param>
        /// <returns></returns>
        public WorkflowStateCollection LoadWorkflowDetails(String entityGUID, String workflowName, CallerContext callerContext)
        {
            WorkflowStateCollection workflowStateCollection = null;

            #region Validation

            if (String.IsNullOrWhiteSpace(entityGUID) || String.IsNullOrWhiteSpace(workflowName))
            {
                // ToDO replace with error message code
            }

            #endregion Validation

            #region Perform operation

            TrackedActivityInfoCollection trackedActivityInfoCollection = this.GetWorkflowExecutionDetails(entityGUID, workflowName, callerContext, getAll: true);

            if (trackedActivityInfoCollection != null && trackedActivityInfoCollection.Count > 0)
            {
                IEnumerable<TrackedActivityInfo> filteredActivities = trackedActivityInfoCollection.ToList().Where(act => act.Status.ToLowerInvariant() == "executing");
                TrackedActivityInfo[] filteredTrackedActivityInfos = filteredActivities as TrackedActivityInfo[] ?? filteredActivities.ToArray();

                if (filteredTrackedActivityInfos.Any())
                {
                    workflowStateCollection = new WorkflowStateCollection();

                    foreach (TrackedActivityInfo runningActivity in filteredTrackedActivityInfos)
                    {
                        //Create WorkflowState object from tracked activity info
                        WorkflowState wfState = new WorkflowState(runningActivity);

                        TrackedActivityInfo previousActivityInfo = trackedActivityInfoCollection.Where(act => act.ActivityShortName == runningActivity.PreviousActivityShortName).OrderByDescending(a => ValueTypeHelper.ConvertToDateTime(a.EventDate)).FirstOrDefault();

                        if (previousActivityInfo != null)
                        {
                            wfState.PreviousActivityShortName = previousActivityInfo.ActivityShortName;
                            wfState.PreviousActivityLongName = previousActivityInfo.ActivityLongName;
                            wfState.PreviousActivityUserId = previousActivityInfo.ActedUserId;
                            wfState.PreviousActivityUser = previousActivityInfo.ActedUser;
                            wfState.PreviousActivityComments = previousActivityInfo.ActivityComments;
                            wfState.PreviousActivityAction = previousActivityInfo.PerformedAction;
                            wfState.PreviousActivityEventDate = runningActivity.PreviousActivityStartDateTime;
                        }
                        else //If there is no previous Activity then running activity will be the first activity. 
                        {
                            //Here Previous Activity Comments will be the first/previous activity comments which is nothing but the workflow comments. 
                            //wfState.PreviousActivityComments = runningActivity.LastActivityComments;
                            wfState.PreviousActivityComments = "Last Activity comments";
                        }

                        workflowStateCollection.Add(wfState);
                    }
                }
            }

            #endregion Perform operation

            return workflowStateCollection;
        }

        /// <summary>
        /// Get the Workflow Execution Details based on the Entity Id and the Workflow Long Name
        /// </summary>
        /// <param name="entityId">Specifies Id of Entity</param>
        /// <param name="workflowName">Specifies long name of Workflow</param>
        /// <param name="callerContext">Caller Information details</param>
        /// <param name="getAll">Specifies whether all activity details or only current activity details are to be retrieved."
        /// <returns>Returns the TrackedActivityInfoCollection object</returns>
        /// <exception cref="MDMOperationException">Thrown when entity id is not provided </exception>
        public MDMBOW.TrackedActivityInfoCollection GetWorkflowExecutionDetails(Int64 entityId, String workflowName, CallerContext callerContext, Boolean getAll = false)
        {
            Boolean isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;
            MDMBOW.TrackedActivityInfoCollection trackedActivityInfoCollection = null;
            if (isTracingEnabled)
                MDMTraceHelper.StartTraceActivity("WorkflowInstanceManager.GetWorkflowExecutionDetails", false);

            try
            {
                #region Validation

                if (entityId < 1)
                {
                    if (isTracingEnabled)
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Warning, "Entity get request is received without entity id. Get operation is being terminated with exception.");
                    LocaleMessageBL localeMessageBL = new LocaleMessageBL();
                    LocaleMessage localeMessage = localeMessageBL.Get(GlobalizationHelper.GetSystemUILocale(), "111795", false, callerContext);
                    throw new MDMOperationException("111795", localeMessage.Message, "Workflow.PersistenceManager.WorkflowInstance", String.Empty, "GetWorkflowExecutionDetails");//EntityId must be greater than 0
                }

                #endregion

                trackedActivityInfoCollection = GetWorkflowExecutionViewDetailsFromDB(entityId, workflowName, callerContext, getAll);
            }
            finally
            {
                if (isTracingEnabled)
                    MDMTraceHelper.StopTraceActivity("WorkflowInstanceManager.GetWorkflowExecutionDetails");
            }

            return trackedActivityInfoCollection;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityGUID"></param>
        /// <param name="workflowName"></param>
        /// <param name="callerContext"></param>
        /// <param name="getAll"></param>
        /// <returns></returns>
        public MDMBOW.TrackedActivityInfoCollection GetWorkflowExecutionDetails(String entityGUID, String workflowName, CallerContext callerContext, Boolean getAll = false)
        {
            Boolean isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;
            MDMBOW.TrackedActivityInfoCollection trackedActivityInfoCollection = null;

            if (isTracingEnabled)
            {
                MDMTraceHelper.StartTraceActivity("WorkflowInstanceManager.GetWorkflowExecutionDetails", false);
            }

            try
            {
                #region Validation

                if (String.IsNullOrWhiteSpace(entityGUID))
                {
                    if (isTracingEnabled)
                    {
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Warning, "Entity get request is received without entity guid. Get operation is being terminated with exception.");
                    }

                    // TODO :Replace with String locale message
                    //LocaleMessageBL localeMessageBL = new LocaleMessageBL();
                    //LocaleMessage localeMessage = localeMessageBL.Get(GlobalizationHelper.GetSystemUILocale(), "111795", false, callerContext);
                    throw new MDMOperationException("111795", "", "Workflow.PersistenceManager.WorkflowInstance", String.Empty, "GetWorkflowExecutionDetails");
                }

                #endregion

                //Get Command
                DBCommandProperties command = DBCommandHelper.Get(callerContext, Core.MDMCenterModuleAction.Read);

                WorkflowInstanceDA workflowInstanceDA = new WorkflowInstanceDA();

                trackedActivityInfoCollection = workflowInstanceDA.GetWorkfloExecutionDetails(entityGUID, workflowName, command, getAll);

                //Populate workflow comments to FirstActivity for each workflow.
                PopulateWorkflowCommentsToFirstActivity(trackedActivityInfoCollection);
            }
            finally
            {
                if (isTracingEnabled)
                {
                    MDMTraceHelper.StopTraceActivity("WorkflowInstanceManager.GetWorkflowExecutionDetails");
                }
            }

            return trackedActivityInfoCollection;
        }

        public Collection<DoneReportItem> GetWorkflowDoneReport(Int32 userId, String userParticipation, Collection<Int32> catalogIds, Collection<Int64> entityTypeIds, Collection<String> workflowNames, Collection<String> currentWorkflowActivity, Collection<Int32> attributeIds, Int32 localeId, Int64 countFrom, Int64 countTo, String attributesDataSource, out Int64 totalCount, CallerContext callerContext)
        {
            Boolean isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;
            if (isTracingEnabled)
                MDMTraceHelper.StartTraceActivity("WorkflowInstanceManager.GetWorkflowDoneReport", false);

            Collection<DoneReportItem> collection;
            try
            {

                //Get Command
                DBCommandProperties command = DBCommandHelper.Get(callerContext, Core.MDMCenterModuleAction.Read);

                WorkflowInstanceDA workflowInstanceDA = new WorkflowInstanceDA();

                collection = workflowInstanceDA.GetWorkflowDoneReport(userId, userParticipation, catalogIds, entityTypeIds, workflowNames, currentWorkflowActivity, attributeIds, localeId, countFrom, countTo, attributesDataSource, out totalCount, command);

            }
            finally
            {
                if (isTracingEnabled)
                    MDMTraceHelper.StopTraceActivity("WorkflowInstanceManager.GetWorkflowDoneReport");
            }

            return collection;
        }

        #endregion

        #region Workflow Activity Detail

        /// <summary>
        /// Gets security user details who performs the workflow activity based on the requested inputs. 
        /// If more than one activities found for the result, it returns the first activity's user details.
        /// </summary>
        /// <param name="entityId">Indicates the entity Id</param>
        /// <param name="workflowName">Indicates the workflow short name or long name</param>
        /// <param name="activityName">Indicates the activity short name or long name</param>
        /// <param name="callerContext">Indicates the caller context details</param>
        /// <returns>Returns the security user object</returns>
        public SecurityUser GetWorkflowActivityPerformedUser(Int64 entityId, String workflowName, String activityName, CallerContext callerContext)
        {
            Boolean isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;
            if (isTracingEnabled)
            {
                MDMTraceHelper.StartTraceActivity("PersistenceManager.WorkflowInstanceBL.GetWorkflowActivityPerformedUser", MDMTraceSource.AdvancedWorkflow, false);
            }

            SecurityUser user = null;

            try
            {
                InputParameterValidation(entityId, workflowName, activityName, callerContext);

                if (isTracingEnabled)
                {
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("Requested input parameters are entityId : {0}, workflow name : {1} , activity name : {2}", entityId, workflowName, activityName), MDMTraceSource.AdvancedWorkflow);
                }

                Int32 actedUserId = GetActedUser(entityId, workflowName, activityName, callerContext);

                if (actedUserId > 0)
                {
                    user = GetSecurityUserById(actedUserId, callerContext);
                }

                if (isTracingEnabled)
                {
                    if (user == null)
                    {

                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("None of the users performed against requested activity {0}", activityName), MDMTraceSource.AdvancedWorkflow);
                    }
                    else
                    {
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("{0} user (user id is {1}) performed '{2}' activity for the workflow {3}", actedUserId, user.UserName, activityName, workflowName), MDMTraceSource.AdvancedWorkflow);
                    }
                }
            }
            finally
            {
                if (isTracingEnabled)
                {
                    MDMTraceHelper.StopTraceActivity("PersistenceManager.WorkflowInstanceBL.GetWorkflowActivityPerformedUser", MDMTraceSource.AdvancedWorkflow);
                }
            }

            return user;
        }

        #endregion 

        #endregion Public Methods

        #region Private  Methods

        private MDMBOW.TrackedActivityInfoCollection GetWorkflowExecutionViewDetailsFromDB(long entityId, String workflowName, CallerContext callerContext, Boolean getAll = false)
        {
            //Get Command
            DBCommandProperties command = DBCommandHelper.Get(callerContext, Core.MDMCenterModuleAction.Read);

            WorkflowInstanceDA workflowInstanceDA = new WorkflowInstanceDA();

            MDMBOW.TrackedActivityInfoCollection trackedActivityInfoCollection = workflowInstanceDA.GetWorkflowExecutionDetails(entityId, workflowName, command, getAll);

            //Populate workflow comments to FirstActivity for each workflow.
            PopulateWorkflowCommentsToFirstActivity(trackedActivityInfoCollection);

            return trackedActivityInfoCollection;
        }

        private void PopulateWorkflowCommentsToFirstActivity(MDMBOW.TrackedActivityInfoCollection trackedActivityInfoCollection)
        {
            if (trackedActivityInfoCollection != null && trackedActivityInfoCollection.Count > 0)
            {
                IEnumerable<String> uniqueInstanceIds = trackedActivityInfoCollection.Select(a => a.RuntimeInstanceId).Distinct();
                foreach (String instanceId in uniqueInstanceIds)
                {
                    MDMBOW.TrackedActivityInfo firstActivity = trackedActivityInfoCollection.Where(a => a.RuntimeInstanceId == instanceId).OrderBy(a => a.EventDate).FirstOrDefault();
                    if (firstActivity != null)
                    {
                        if (firstActivity.ActivityComments.HasNoValue())
                        {
                            firstActivity.ActivityComments = firstActivity.WorkflowComments;
                        }
                    }
                }
            }
        }

        private Boolean DetermineAttributeValueBasedPermission(Int32 orgId, Int32 catalogId, Int32 userId, Collection<SearchAttributeRule> searchAttributeRules, CallerContext callerContext)
        {
            Boolean isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;
            if (isTracingEnabled)
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Started determining Attribute Value Based Permission..");

            Boolean hasPermission = false;

            SecurityPermissionDefinitionBL securityPermissionDefinitionBL = new SecurityPermissionDefinitionBL();
            ApplicationContext applicationContext = new ApplicationContext();

            #region Fill applicationContext

            applicationContext.OrganizationId = orgId;
            applicationContext.ContainerId = catalogId;
            applicationContext.UserId = userId;

            #endregion

            //Get all the security Permission Definitions for the given application context
            SecurityPermissionDefinitionCollection securityPermissionDefinitions = securityPermissionDefinitionBL.Get(applicationContext, callerContext);

            if (securityPermissionDefinitions != null && securityPermissionDefinitions.Count > 0)
            {
                if (isTracingEnabled)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Security Permission definition is not empty.");

                #region Get Config values

                //Get Application config key to decide whether view only entities should be included in workflow panel count or not
                Boolean includeViewOnlyEntities = true;
                String strIncludeViewOnlyEntities = AppConfigurationHelper.GetAppConfig<String>("MDMCenter.Workflow.WorkflowPanel.IncludeViewOnlyEntities");

                try
                {
                    includeViewOnlyEntities = ValueTypeHelper.ConvertToBoolean(strIncludeViewOnlyEntities);
                }
                catch
                {
                    //Ignore error..
                    //Set includeViewOnlyEntities to true
                    includeViewOnlyEntities = true;
                }

                if (isTracingEnabled)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("Appconfig MDMCenter.Workflow.WorkflowPanel.IncludeViewOnlyEntities: {0}", strIncludeViewOnlyEntities));

                #endregion

                foreach (SecurityPermissionDefinition securityPermissionDefinition in securityPermissionDefinitions)
                {
                    if (includeViewOnlyEntities || !(securityPermissionDefinition.PermissionSet.Count == 1 && securityPermissionDefinition.PermissionSet.Contains(UserAction.View)))
                    {
                        #region Prepare/Construct Search Attribute Rule

                        //Get definition values..
                        String definitionValues = String.Empty;

                        //Get attribute Id
                        Int32 attributeId = securityPermissionDefinition.ApplicationContext.AttributeId;

                        if (securityPermissionDefinition.PermissionValues != null && securityPermissionDefinition.PermissionValues.Count > 0)
                        {
                            definitionValues = ValueTypeHelper.JoinCollection(securityPermissionDefinition.PermissionValues, ",");
                        }

                        if (attributeId < 1 && definitionValues.Contains("[rsall]"))
                        {
                            //Having all permissions..
                            //No need put security attribute rule..
                            //Clear rule collection and come out.
                            searchAttributeRules.Clear();
                            hasPermission = true;
                            break;
                        }

                        //Check whether the rule for the current definition attribute has already been added
                        SearchAttributeRule searchAttributeRule = searchAttributeRules.FirstOrDefault(s => s.Attribute.Id == attributeId);

                        if (searchAttributeRule == null)
                        {
                            //The rule has not been added..
                            //Populate the rule and add to the rule collection
                            searchAttributeRule = new SearchAttributeRule();
                            searchAttributeRule.Attribute = new Attribute();
                            searchAttributeRule.Attribute.Id = attributeId;
                            searchAttributeRule.Attribute.Name = securityPermissionDefinition.ApplicationContext.AttributeName;
                            searchAttributeRule.Operator = SearchOperator.In;

                            searchAttributeRules.Add(searchAttributeRule);
                        }
                        else
                        {
                            //Append definition value to the existing rule value
                            Object attributeRuleValue = searchAttributeRule.Attribute.GetCurrentValueInvariant();

                            if (attributeRuleValue != null)
                            {
                                definitionValues = String.Concat(attributeRuleValue.ToString(), ",", definitionValues);
                            }
                        }

                        //Set values
                        searchAttributeRule.Attribute.SetValueInvariant(new Value((Object)definitionValues));

                        #endregion
                    }
                    else
                    {
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, String.Format("Only view permission is there for entities with Security Permission Definition : {0}.", securityPermissionDefinition.Name));
                    }
                }

                if (searchAttributeRules.Count > 0)
                {
                    hasPermission = true;
                }
            }
            else
            {
                //No security permission definition available..
                //This means user is not having any permissions.
                //Log message and skip getting search results.

                MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, String.Format("Permissions are not available for the context Org: {0}, Catalog: {1} and User: {2}. Cannot return search results.", applicationContext.OrganizationId, applicationContext.ContainerId, applicationContext.UserName));

                hasPermission = false;
            }

            if (isTracingEnabled)
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Completed determining Attribute Value Based Permission.");

            return hasPermission;
        }

        private void GetSecurityPrincipal()
        {
            if (_securityPrincipal == null)
            {
                _securityPrincipal = SecurityPrincipalHelper.GetCurrentSecurityPrincipal();
            }
        }

        private void InputParameterValidation(Int64 entityId, String workflowName, String activityName, CallerContext callerContext)
        {
            String errorMsg = String.Empty;
            String errorMsgCode = String.Empty;

            if (entityId < 1)
            {
                errorMsg = "EntityId must be greater than 0";
                errorMsgCode = "111795";
            }
            else if (String.IsNullOrWhiteSpace(workflowName))
            {
                errorMsg = "Workflow name cannot be null or empty";
                errorMsgCode = "113769";
            }
            else if (String.IsNullOrWhiteSpace(activityName))
            {
                errorMsg = "Activity Name cannot be null or empty";
                errorMsgCode = "112010";
            }
            else if (callerContext == null)
            {
                errorMsg = "CallerContext cannot be null";
                errorMsgCode = "111846";
            }

            if (!String.IsNullOrWhiteSpace(errorMsgCode))
            {
                _localeMessage = _localeMessageBL.Get(GlobalizationHelper.GetSystemUILocale(), errorMsgCode, false, callerContext);
                throw new MDMOperationException(errorMsgCode, _localeMessage.Message, "WorkflowInstance", String.Empty, "GetWorkflowActivityPerformedUser"); 
            }
        }

        private Int32 GetActedUser(Int64 entityId, String workflowName, String activityName, CallerContext callerContext)
        {
            var activities = this.GetWorkflowExecutionViewDetailsFromDB(entityId, workflowName, callerContext, true);

            Int32 actedUserId = -1;

            if (activities != null && activities.Count > 0)
            {
                MDMBOW.TrackedActivityInfo requestedActivityClosedStatusInfo = null;

                foreach (MDMBOW.TrackedActivityInfo activityInfo in activities)
                {
                    if ((activityInfo.ActivityShortName == activityName || activityInfo.ActivityLongName == activityName) &&
                        activityInfo.Status.ToLowerInvariant() == "closed") //Considering 'Closed' status as we need to return Acted User which is available at closed activity level...
                    {
                        if (requestedActivityClosedStatusInfo == null)
                        {
                            requestedActivityClosedStatusInfo = activityInfo;
                        }
                        else
                        {
                            //Consider latest closed activity by considering latest time stamp
                            DateTime earlierActivityInfoEventDate = ValueTypeHelper.ConvertToDateTime(requestedActivityClosedStatusInfo.EventDate);
                            DateTime currentActivityInfoEventDate = ValueTypeHelper.ConvertToDateTime(activityInfo.EventDate);

                            if (currentActivityInfoEventDate >= earlierActivityInfoEventDate)
                            {
                                requestedActivityClosedStatusInfo = activityInfo;
                            }
                        }
                    }
                }

                if (requestedActivityClosedStatusInfo != null)
                {
                    actedUserId = requestedActivityClosedStatusInfo.ActedUserId;
                }
            }

            return actedUserId;
        }

        private SecurityUser GetSecurityUserById(Int32 userId, CallerContext callerContext)
        {
            SecurityUserBL securityUserBL = new SecurityUserBL();
            return securityUserBL.GetById(userId, callerContext);
        }

        #endregion Methods

        #endregion Methods
    }
}