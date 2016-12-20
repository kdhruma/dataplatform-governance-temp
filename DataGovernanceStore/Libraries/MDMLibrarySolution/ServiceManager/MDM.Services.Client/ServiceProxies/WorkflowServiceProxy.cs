using System;
using System.Collections.ObjectModel;

namespace MDM.Services.ServiceProxies
{
    using MDM.BusinessObjects;
    using MDM.Core;
    using MDM.Services.WorkflowServiceClient;

    /// <summary>
    /// Represents class for workflow service proxy
    /// </summary>
    public class WorkflowServiceProxy : WorkflowServiceClient, MDM.WCFServiceInterfaces.IWorkflowService
    {
        #region Constructors

        /// <summary>
        /// Parameterless Constructor
        /// </summary>
        public WorkflowServiceProxy()
        {

        }

        /// <summary>
        /// Constructor with endpoint configuration name
        /// </summary>
        /// <param name="endpointConfigurationName">Indicates the endpoint configuration name</param>
        public WorkflowServiceProxy(String endpointConfigurationName)
            : base(endpointConfigurationName)
        {
        }

        /// <summary>
        /// Constructor with endpoint configuration name and remote address
        /// </summary>
        /// <param name="endpointConfigurationName">Indicates the endpoint configuration name</param>
        /// <param name="remoteAddress">Indicates the remote address</param>
        public WorkflowServiceProxy(String endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress)
            : base(endpointConfigurationName, remoteAddress)
        {
        }

        #endregion

        #region IWorkflowService Members

        /// <summary>
        /// Starts the requested workflow
        /// </summary>
        /// <param name="workflowDataContext">Indicates data context which will go as part of the instance lifetime</param>
        /// <param name="serviceType">Indicates type of the service which is invoking the workflow</param>
        /// <param name="serviceId">Indicates unique identification of the service</param>
        /// <param name="currentUserName">Indicates user name who is requesting for the start</param>
        /// <param name="workflowInstanceRunOption">Indicates run option which describes the way the workflow needs to be invoked</param>
        /// <param name="operationResult">Indicates object which collects results of the operation as output parameter</param>
        /// <param name="callerContext">Indicates name of application and module that are performing the action</param>
        ///<returns>Returns the success count</returns>
        public int StartWorkflow(BusinessObjects.Workflow.WorkflowDataContext workflowDataContext, string serviceType, int serviceId, string currentUserName, WorkflowInstanceRunOptions workflowInstanceRunOption, ref OperationResult operationResult, CallerContext callerContext)
        {
            return base.StartWorkflowWithRunOptions(workflowDataContext, serviceType, serviceId, currentUserName, workflowInstanceRunOption, ref operationResult, callerContext);
        }

        /// <summary>
        /// Resumes workflow
        /// </summary>
        /// <param name="commaSeparatedRuntimeInstanceIds">Indicates instance identifiers which needs to be resumed</param>
        /// <param name="actionContext">The context which describes the action being performed</param>
        ///<param name="operationResult">Indicates object which collects results of the operation as output parameter</param>
        ///<param name="callerContext">Indicates name of application and module that are performing the action</param>
        ///<returns>Returns the success count</returns>
        public int ResumeWorkflow(string commaSeparatedRuntimeInstanceIds, BusinessObjects.Workflow.WorkflowActionContext actionContext, ref OperationResult operationResult, CallerContext callerContext)
        {
            return base.ResumeWorkflowInstancesByRuntimeInstanceIds(commaSeparatedRuntimeInstanceIds, actionContext, ref operationResult, callerContext);
        }

        /// <summary>
        /// Resumes workflow
        /// </summary>
        ///<param name="mdmObj">Indicates MDM Object for which workflow needs to be resumed</param>
        /// <param name="actionContext">The context which describes the action being performed</param>
        ///<param name="operationResult">Indicates object which collects results of the operation as output parameter</param>
        ///<param name="callerContext">Indicates name of application and module that are performing the action</param>
        ///<returns>Returns the success count</returns>
        public int ResumeWorkflow(BusinessObjects.Workflow.WorkflowMDMObject mdmObj, BusinessObjects.Workflow.WorkflowActionContext actionContext, ref OperationResult operationResult, CallerContext callerContext)
        {
            return base.ResumeWorkflowInstanceByMDMObject(mdmObj, actionContext, ref operationResult, callerContext);
        }

        /// <summary>
        /// Resumes workflow
        /// </summary>
        /// <param name="workflowInstance">Indicates instance object which needs to be resumed</param>
        /// <param name="actionContext">The context which describes the action being performed</param>
        ///<param name="operationResult">Indicates object which collects results of the operation as output parameter</param>
        ///<param name="callerContext">Indicates name of application and module that are performing the action</param>
        ///<returns>Returns the success count</returns>
        public int ResumeWorkflow(BusinessObjects.Workflow.WorkflowInstance workflowInstance, BusinessObjects.Workflow.WorkflowActionContext actionContext, ref OperationResult operationResult, CallerContext callerContext)
        {
            return base.ResumeWorkflowInstance(workflowInstance, actionContext, ref operationResult, callerContext);
        }

        /// <summary>
        /// Resumes workflow
        /// </summary>
        /// <param name="workflowDataContext">Indicates data context holding MDM Objects for which workflow needs to be resumed</param>
        /// <param name="actionContext">The context which describes the action being performed</param>
        ///<param name="operationResult">Indicates object which collects results of the operation as output parameter</param>
        ///<param name="callerContext">Indicates name of application and module that are performing the action</param>
        ///<returns>Returns the success count</returns>
        public int ResumeWorkflow(BusinessObjects.Workflow.WorkflowDataContext workflowDataContext, BusinessObjects.Workflow.WorkflowActionContext actionContext, ref OperationResult operationResult, CallerContext callerContext)
        {
            return base.ResumeMultipleWorkflowInstancesByMDMObjects(workflowDataContext, actionContext, ref operationResult, callerContext);
        }

        /// <summary>
        /// Resumes workflow
        /// </summary>
        ///<param name="instanceCollection">Indicates collection of Instance objects which needs to be resumed</param>
        /// <param name="actionContext">The context which describes the action being performed</param>
        ///<param name="operationResult">Indicates object which collects results of the operation as output parameter</param>
        ///<param name="callerContext">Indicates name of application and module that are performing the action</param>
        ///<returns>Returns the success count</returns>
        public int ResumeWorkflow(Collection<BusinessObjects.Workflow.WorkflowInstance> instanceCollection, BusinessObjects.Workflow.WorkflowActionContext actionContext, ref OperationResult operationResult, CallerContext callerContext)
        {
            return base.ResumeMultipleWorkflowInstances(instanceCollection, actionContext, ref operationResult, callerContext);
        }

        /// <summary>
        ///  Get the collection of Workflow Instances based on the Workflow Id
        /// </summary>
        /// <param name="workflowId">Indicates the workflow identifier</param>
        /// <param name="mdmObj">Indicates the MDM object</param>
        /// <param name="operationResult">Indicates object which collects results of the operation</param>
        /// <param name="callerContext">Indicates name of application and module that are performing the action</param>
        /// <returns>Returns collections of workflow instances based on the workflow id</returns>
        public Collection<BusinessObjects.Workflow.WorkflowInstance> FindWorkflowInstance(int workflowId, BusinessObjects.Workflow.WorkflowMDMObject mdmObj, ref OperationResult operationResult, CallerContext callerContext)
        {
            return FindWorkflowInstanceInWorkflow(workflowId, mdmObj, ref operationResult, callerContext);
        }

        /// <summary>
        /// Gets the action buttons for the requested activity
        /// </summary>
        /// <param name="activityId">Indicates identifier of an activity</param>
        /// <param name="loginUser">Indicates user login. Will be used during allowed users and roles check if enabled</param>
        /// <param name="checkAllowedUsersAndRoles">Indicates true if you want to check activity allowed users and allowed roles information
        /// Set to null if you want default behavior</param>
        /// <param name="callerContext">Indicates name of application and module that are performing the action</param>
        /// <returns>Returns action buttons with comments details</returns>
        public Table GetActionButtons(int activityId, string loginUser, bool? checkAllowedUsersAndRoles, CallerContext callerContext)
        {
            return base.GetActivityActionButtons(activityId, loginUser, checkAllowedUsersAndRoles, callerContext);
        }

        /// <summary>
        /// Gets the details required for the workflow UI Panel
        /// </summary>
        /// <param name="catalogId">Indicates identifier of the catalog for which details are required</param>
        /// <param name="userId">Indicates user identifier for which details needs to be get</param>
        /// <param name="showEmptyItems">Indicates whether to hide empty nodes or not</param>
        /// <param name="showItemsAssignedToOtherUsers">Indicates whether to include data related to workflows assigned to other users or not</param>
        /// <param name="callerContext">Indicates name of application and module that are performing the action</param>
        /// <returns>Returns workflow panel details in table format</returns>
        public Table GetWorkflowPanelDetails(int catalogId, int userId, bool showEmptyItems, bool showItemsAssignedToOtherUsers, CallerContext callerContext)
        {
            return base.GetWorkflowPanelDetailsExt(catalogId, userId, showEmptyItems, showItemsAssignedToOtherUsers, callerContext);
        }

        /// <summary>
        /// Start workflow using entity workflow service. This will trigger business rule before and after starting workflow.
        /// </summary>
        /// <param name="entityIdList">Indicates collection of entity identifiers for which workflow is to be triggered</param>
        /// <param name="workflowName">Indicates name of workflow to invoke</param>
        /// <param name="workflowVersionId">Indicates workflow version to invoke</param>
        /// <param name="serviceType">Indicates service type which triggered workflow</param>
        /// <param name="serviceId">Indicates service type identifier - which triggered workflow</param>
        /// <param name="callerContext">Indicates name of application and module that are performing the action</param>
        /// <param name="comments">Indicates comments for invoking workflow</param>
        /// <returns>Returns collection of entity operation result indicating business rule + workflow invocation result</returns>
        public EntityOperationResultCollection StartWorkflow(Collection<long> entityIdList, string workflowName, int workflowVersionId, string serviceType, int serviceId, CallerContext callerContext, String comments = "")
        {
            return base.StartWorkflowWithEntityIdList(entityIdList, workflowName, workflowVersionId, serviceType, serviceId, callerContext, comments);
        }

        /// <summary>
        /// Start workflow using entity workflow service. This will trigger business rule before and after starting workflow.
        /// </summary>
        /// <param name="entityId">Indicates entity identifier for which workflow is to be triggered</param>
        /// <param name="workflowName">Indicates name of workflow to invoke</param>
        /// <param name="workflowVersionId">Indicates workflow version to invoke</param>
        /// <param name="serviceType">Indicates service type which triggered workflow</param>
        /// <param name="serviceId">Indicates service type identifier - which triggered workflow</param>
        /// <param name="callerContext">Indicates name of application and module that are performing the action</param>
        /// <param name="comments">Indicates comments for invoking workflow</param>
        /// <returns>Returns entity operation result indicating business rule + workflow invocation result</returns>
        public EntityOperationResult StartWorkflow(long entityId, string workflowName, int workflowVersionId, string serviceType, int serviceId, CallerContext callerContext, String comments = "")
        {
            return base.StartWorkflowWithEntityId(entityId, workflowName, workflowVersionId, serviceType, serviceId, callerContext, comments);
        }

        /// <summary>
        /// Start workflow using entity workflow service. This will trigger business rule before and after starting workflow.
        /// </summary>
        /// <param name="entities">Indicates collection of entity for which workflow is to be triggered</param>
        /// <param name="workflowName">Indicates name of workflow to invoke</param>
        /// <param name="workflowVersionId">Indicates workflow version to invoke</param>
        /// <param name="serviceType">Indicates service type which triggered workflow</param>
        /// <param name="serviceId">Indicates service type identifier - which triggered workflow</param>
        /// <param name="callerContext">Indicates name of application and module that are performing the action</param>
        /// <param name="comments">Indicates comments for invoking workflow</param>
        /// <returns>Returns collection of entity operation result indicating business rule + workflow invocation result</returns>
        public EntityOperationResultCollection StartWorkflow(EntityCollection entities, string workflowName, int workflowVersionId, string serviceType, int serviceId, CallerContext callerContext, String comments = "")
        {
            return base.StartWorkflowWithEntityList(entities, workflowName, workflowVersionId, serviceType, serviceId, callerContext, comments);
        }

        /// <summary>
        /// Start workflow using entity workflow service. This will trigger business rule before and after starting workflow.
        /// </summary>
        /// <param name="entity">Indicates entity for which workflow is to be triggered</param>
        /// <param name="workflowName">Indicates name of workflow to invoke</param>
        /// <param name="workflowVersionId">Indicates workflow version to invoke</param>
        /// <param name="serviceType">Indicates service type which triggered workflow</param>
        /// <param name="serviceId">Indicates service type identifier - which triggered workflow</param>
        /// <param name="callerContext">Indicates name of application and module that are performing the action</param>
        /// <param name="comments">Indicates comments for invoking workflow</param>
        /// <returns>Returns entity operation result indicating business rule + workflow invocation result</returns>
        public EntityOperationResult StartWorkflow(Entity entity, string workflowName, int workflowVersionId, string serviceType, int serviceId, CallerContext callerContext, String comments = "")
        {
            return base.StartWorkflowWithEntity(entity, workflowName, workflowVersionId, serviceType, serviceId, callerContext, comments);
        }

        /// <summary>
        /// Start workflow using entity workflow service. This will trigger business rule before and after starting workflow.
        /// </summary>
        /// <param name="entityIdList">Indicates collection of entity identifiers for which workflow is to be triggered</param>
        /// <param name="workflowName">Indicates name of workflow to invoke</param>
        /// <param name="workflowVersionId">Indicates workflow version to invoke</param>
        /// <param name="serviceType">Indicates service type which triggered workflow</param>
        /// <param name="serviceId">Indicates service type identifier - which triggered workflow</param>
        /// <param name="workflowInstanceRunOption">Indicates whether to run bulk entities as 1 instance for workflow or to treat them individually</param>
        /// <param name="callerContext">Indicates name of application and module that are performing the action</param>
        /// <param name="comments">Indicates comments for invoking workflow</param>
        /// <returns>Returns collection of entity operation result indicating business rule + workflow invocation result</returns>
        public EntityOperationResultCollection StartWorkflow(Collection<long> entityIdList, string workflowName, int workflowVersionId, string serviceType, int serviceId, WorkflowInstanceRunOptions workflowInstanceRunOption, CallerContext callerContext, String comments = "")
        {
            return base.StartWorkflowWithEntityIdListAndRunOption(entityIdList, workflowName, workflowVersionId, serviceType, serviceId, workflowInstanceRunOption, callerContext, comments);
        }

        /// <summary>
        /// Start workflow using entity workflow service. This will trigger business rule before and after starting workflow.
        /// </summary>
        /// <param name="entityId">Indicates entity identifier for which workflow is to be triggered</param>
        /// <param name="workflowName">Indicates name of workflow to invoke</param>
        /// <param name="workflowVersionId">Indicates workflow version to invoke</param>
        /// <param name="serviceType">Indicates service type which triggered workflow</param>
        /// <param name="serviceId">Indicates service type identifier - which triggered workflow</param>
        /// <param name="workflowInstanceRunOption">Indicates whether to run bulk entities as 1 instance for workflow or to treat them individually</param>
        /// <param name="callerContext">Indicates name of application and module that are performing the action</param>
        /// <param name="comments">Indicates comments for invoking workflow</param>
        /// <returns>Returns entity operation result indicating business rule + workflow invocation result</returns>
        public EntityOperationResult StartWorkflow(long entityId, string workflowName, int workflowVersionId, string serviceType, int serviceId, WorkflowInstanceRunOptions workflowInstanceRunOption, CallerContext callerContext, String comments = "")
        {
            return base.StartWorkflowWithEntityIdAndRunOption(entityId, workflowName, workflowVersionId, serviceType, serviceId, workflowInstanceRunOption, callerContext, comments);
        }

        /// <summary>
        /// Start workflow using entity workflow service. This will trigger business rule before and after starting workflow.
        /// </summary>
        /// <param name="entities">Indicates collection of entity for which workflow is to be triggered</param>
        /// <param name="workflowName">Indicates name of workflow to invoke</param>
        /// <param name="workflowVersionId">Indicates workflow version to invoke</param>
        /// <param name="serviceType">Indicates service type which triggered workflow</param>
        /// <param name="serviceId">Indicates service type identifier - which triggered workflow</param>
        /// <param name="workflowInstanceRunOption">Indicates whether to run bulk entities as 1 instance for workflow or to treat them individually</param>
        /// <param name="callerContext">Indicates name of application and module that are performing the action</param>
        /// <param name="comments">Indicates comments for invoking workflow</param>
        /// <returns>Returns collection of entity operation result indicating business rule + workflow invocation result</returns>
        public EntityOperationResultCollection StartWorkflow(EntityCollection entities, string workflowName, int workflowVersionId, string serviceType, int serviceId, WorkflowInstanceRunOptions workflowInstanceRunOption, CallerContext callerContext, String comments = "")
        {
            return base.StartWorkflowWithEntityListAndRunOption(entities, workflowName, workflowVersionId, serviceType, serviceId, workflowInstanceRunOption, callerContext, comments);
        }

        /// <summary>
        /// Start workflow using entity workflow service. This will trigger business rule before and after starting workflow.
        /// </summary>
        /// <param name="entity">Indicates entity for which workflow is to be triggered</param>
        /// <param name="workflowName">Indicates name of workflow to invoke</param>
        /// <param name="workflowVersionId">Indicates workflow version to invoke</param>
        /// <param name="serviceType">Indicates service type which triggered workflow</param>
        /// <param name="serviceId">Indicates service type identifier - which triggered workflow</param>
        /// <param name="workflowInstanceRunOption">Indicates whether to run bulk entities as 1 instance for workflow or to treat them individually</param>
        /// <param name="callerContext">Indicates name of application and module that are performing the action</param>
        /// <param name="comments">Indicates comments for invoking workflow</param>
        /// <returns>Returns entity operation result indicating business rule + workflow invocation result</returns>
        public EntityOperationResult StartWorkflow(Entity entity, string workflowName, int workflowVersionId, string serviceType, int serviceId, WorkflowInstanceRunOptions workflowInstanceRunOption, CallerContext callerContext, String comments = "")
        {
            return base.StartWorkflowWithEntityAndRunOption(entity, workflowName, workflowVersionId, serviceType, serviceId, workflowInstanceRunOption, callerContext, comments);
        }

        /// <summary>
        /// Resume workflow for given entities. This will trigger business rule before and after resuming workflow.
        /// </summary>
        /// <param name="entityIdList">Indicates collection of entity ids for which workflow is to be resumed</param>
        /// <param name="workflowName">Indicates name of workflow which is to be resumed</param>
        /// <param name="workflowVersionId">Indicates Version identifier of workflow which is to be resumed.</param>
        /// <param name="currentActivityName">Indicates name of activity on which workflow is currently waiting.</param>
        /// <param name="action">Indicates action performed by user for an activity on which workflow is currently waiting</param>
        /// <param name="comments">Indicates comments entered by user while resuming workflow</param>
        /// <param name="callerContext">Indicates name of application and module that are performing the action</param>
        /// <returns>Returns collection of entity operation result indicating business rule + workflow invocation result</returns>
        public EntityOperationResultCollection ResumeWorkflow(Collection<long> entityIdList, string workflowName, int workflowVersionId, string currentActivityName, string action, string comments, CallerContext callerContext)
        {
            return base.ResumeWorkflowWithEntityIdList(entityIdList, workflowName, workflowVersionId, currentActivityName, action, comments, callerContext);
        }

        /// <summary>
        /// Resume workflow for given entities. This will trigger business rule before and after resuming workflow.
        /// </summary>
        /// <param name="entities">Indicates collection of entity for which workflow is to be resumed</param>
        /// <param name="workflowName">Indicates name of workflow which is to be resumed</param>
        /// <param name="workflowVersionId">Indicates Version identifier of workflow which is to be resumed.</param>
        /// <param name="currentActivityName">Indicates name of activity on which workflow is currently waiting.</param>
        /// <param name="action">Indicates action performed by user for an activity on which workflow is currently waiting</param>
        /// <param name="comments">Indicates comments entered by user while resuming workflow</param>
        /// <param name="callerContext">Indicates name of application and module that are performing the action</param>
        /// <returns>Returns collection of entity operation result indicating business rule + workflow invocation result</returns>
        public EntityOperationResultCollection ResumeWorkflow(EntityCollection entities, string workflowName, int workflowVersionId, string currentActivityName, string action, string comments, CallerContext callerContext)
        {
            return base.ResumeWorkflowWithEntityList(entities, workflowName, workflowVersionId, currentActivityName, action, comments, callerContext);
        }

        /// <summary>
        /// Get done report for current user
        /// </summary>
        /// <param name="userId">Indicates identifier of the user</param>
        /// <param name="userParticipation">Indicates participation of the user</param>
        /// <param name="catalogIds">Indicates identifiers of category</param>
        /// <param name="entityTypeIds">Indicates identifiers of entity type</param>
        /// <param name="workflowNames">Indicates name of all workflows</param>
        /// <param name="currentWorkflowActivity">Indicates current workFlow activity</param>
        /// <param name="attributeIds">Indicates identifiers of attribute</param>
        /// <param name="localeId">Indicates identifier of locale</param>
        /// <param name="countFrom">Indicates report starting boundary</param>
        /// <param name="attributesDataSource">Specifies attributes data source. Allowed values: 'DNTables' (default if not specified) and 'CoreTables'.</param>
        /// <param name="countTo">Indicates report ending boundary</param>
        /// <param name="callerContext">Indicates name of application and module that are performing the action</param>
        /// <returns>Returns collection of done report item</returns>
        public Collection<DoneReportItem> GetWorkflowDoneReport(Int32 userId, String userParticipation, Collection<Int32> catalogIds, Collection<Int64> entityTypeIds,
                                                                Collection<String> workflowNames, Collection<String> currentWorkflowActivity, Collection<Int32> attributeIds,
                                                                Int32 localeId, Int64 countFrom, Int64 countTo, String attributesDataSource, CallerContext callerContext)
        {
            return base.GetWorkflowDoneReportWithAttributesDataSourceParameter(userId, userParticipation, catalogIds, entityTypeIds, workflowNames, currentWorkflowActivity, attributeIds, localeId, countFrom, countTo, attributesDataSource, callerContext);
        }

        #endregion
    }
}
