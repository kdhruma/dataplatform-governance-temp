using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Data;
using System.Collections;
using System.Collections.ObjectModel;

namespace MDM.WCFServiceInterfaces
{
    using MDM.Core;
    using MDMBO = MDM.BusinessObjects;
    using MDMBOW = MDM.BusinessObjects.Workflow;
    using MDM.BusinessObjects;
    using System.ServiceModel.Web;
    [ServiceContract(Namespace = "http://wcfservices.riversand.com")]
    public interface IWorkflowService
    {
        #region Start workflow

        [OperationContract(Name = "StartWorkflow")]
        Int32 StartWorkflow(MDMBOW.WorkflowDataContext workflowDataContext, String serviceType, Int32 serviceId, String currentUserName, ref MDMBO.OperationResult operationResult, MDMBO.CallerContext callerContext);

        [OperationContract(Name = "StartWorkflowWithRunOptions")]
        Int32 StartWorkflow(MDMBOW.WorkflowDataContext workflowDataContext, String serviceType, Int32 serviceId, String currentUserName, WorkflowInstanceRunOptions workflowInstanceRunOption, ref MDMBO.OperationResult operationResult, MDMBO.CallerContext callerContext);

        #endregion Start workflow

        #region Resume workflow

        [OperationContract(Name = "ResumeWorkflowInstancesByRuntimeInstanceIds")]
        Int32 ResumeWorkflow(String commaSeparatedRuntimeInstanceIds, MDMBOW.WorkflowActionContext actionContext, ref MDMBO.OperationResult operationResult, MDMBO.CallerContext callerContext);

        [OperationContract(Name = "ResumeWorkflowInstanceByMDMObject")]
        Int32 ResumeWorkflow(MDMBOW.WorkflowMDMObject mdmObj, MDMBOW.WorkflowActionContext actionContext, ref MDMBO.OperationResult operationResult, MDMBO.CallerContext callerContext);

        [OperationContract(Name = "ResumeWorkflowInstance")]
        Int32 ResumeWorkflow(MDMBOW.WorkflowInstance workflowInstance, MDMBOW.WorkflowActionContext actionContext, ref MDMBO.OperationResult operationResult, MDMBO.CallerContext callerContext);

        [OperationContract(Name = "ResumeMultipleWorkflowInstancesByMDMObjects")]
        Int32 ResumeWorkflow(MDMBOW.WorkflowDataContext workflowDataContext, MDMBOW.WorkflowActionContext actionContext, ref MDMBO.OperationResult operationResult, MDMBO.CallerContext callerContext);

        [OperationContract(Name = "ResumeMultipleWorkflowInstances")]
        Int32 ResumeWorkflow(Collection<MDMBOW.WorkflowInstance> instanceCollection, MDMBOW.WorkflowActionContext actionContext, ref MDMBO.OperationResult operationResult, MDMBO.CallerContext callerContext);

        [OperationContract(Name = "ResumeAbortedWorkflowInstances")]
        Int32 ResumeAbortedWorkflowInstances(Collection<MDMBOW.WorkflowInstance> instanceCollection, String instanceStatus, String loginUser, String programName, ref MDMBO.OperationResult operationResult, MDMBO.CallerContext callerContext);

        #endregion Resume workflow

        #region Terminate workflow

        [OperationContract(Name = "TerminateWorkflowInstances")]
        Int32 TerminateWorkflowInstances(Collection<MDMBOW.WorkflowInstance> instanceCollection, ref MDMBO.OperationResult operationResult, MDMBO.CallerContext callerContext);

        #endregion Terminate workflow

        #region Promote workflow

        [OperationContract(Name = "PromoteWorkflowInstances")]
        Int32 PromoteWorkflowInstances(Collection<MDMBOW.WorkflowInstance> instanceCollection, ref MDMBO.OperationResult operationResult, MDMBO.CallerContext callerContext);

        #endregion Promote workflow

        #region Find Workflow

        [OperationContract(Name = "FindWorkflowInstanceInWorkflow")]
        Collection<MDMBOW.WorkflowInstance> FindWorkflowInstance(Int32 workflowId, MDMBOW.WorkflowMDMObject mdmObj, ref MDMBO.OperationResult operationResult, MDMBO.CallerContext callerContext);

        [OperationContract(Name = "FindWorkflowInstance")]
        Collection<MDMBOW.WorkflowInstance> FindWorkflowInstance(String activityName, MDMBOW.WorkflowMDMObject mdmObj, ref MDMBO.OperationResult operationResult, MDMBO.CallerContext callerContext);

        #endregion Find Workflow

        #region UI Methods

        [OperationContract(Name = "GetAllWorkflowDetails")]
        void GetAllWorkflowDetails(ref Collection<MDMBOW.Workflow> workflowCollection, ref Collection<MDMBOW.WorkflowVersion> workflowVersionCollection, ref Collection<MDMBOW.WorkflowActivity> workflowActivityCollection, ref MDMBO.OperationResult operationResult, MDMBO.CallerContext callerContext);

        [OperationContract(Name = "GetInstanceSummary")]
        Collection<MDMBOW.WorkflowInstance> GetInstanceSummary(String workflowType, Int32 workflowId, Int32 workflowVersionId, String workflowStatus, String activityShortName, String roleIds, String userIds, String instanceId, String mdmObjectIds, Boolean? hasEscalation, Int32 returnSize, ref MDMBO.OperationResult operationResult, MDMBO.CallerContext callerContext);

        [OperationContract(Name = "GetInstanceDetails")]
        void GetInstanceDetails(String workflowType, String instanceId, ref Collection<MDMBOW.WorkflowActivity> runningActivityCollection, ref Collection<MDMBOW.WorkflowMDMObject> mdmObjectCollection, ref Collection<MDMBOW.Escalation> escalationCollection, ref MDMBO.OperationResult operationResult, MDMBO.CallerContext callerContext);

        [OperationContract(Name = "GetWorkflowStatistics")]
        String GetWorkflowStatistics(String workflowType, Int32 workflowId, Int32 workflowVersionId, ref MDMBO.OperationResult operationResult, MDMBO.CallerContext callerContext);

        [OperationContract(Name = "GetActionButtons")]
        DataTable GetActionButtons(Int32 activityId, String toolBarButtonXML, String loginUser, Boolean? checkAllowedUsersAndRoles, ref MDMBO.OperationResult operationResult, MDMBO.CallerContext callerContext);

        [OperationContract(Name = "GetActivityActionButtons")]
        MDMBO.Table GetActionButtons(Int32 activityId, String loginUser, Boolean? checkAllowedUsersAndRoles, MDMBO.CallerContext callerContext);

        [OperationContract(Name = "GetAssignmentButtons")]
        DataTable GetAssignmentButtons(Int32 activityId, String assignmentStatus, String toolBarButtonXML, String loginUser, Boolean? checkAllowedUsersAndRoles, ref MDMBO.OperationResult operationResult, MDMBO.CallerContext callerContext);

        [OperationContract(Name = "GetWorkflowPanelDetails")]
        MDMBO.Table GetWorkflowPanelDetails(Int32 orgId, Int32 catalogId, Int32 userId, Boolean showEmptyItems, Boolean showItemsAssignedToOtherUsers, MDMBO.CallerContext callerContext);

        [OperationContract(Name = "GetWorkflowPanelDetailsExt")]
        MDMBO.Table GetWorkflowPanelDetails(Int32 catalogId, Int32 userId, Boolean showEmptyItems, Boolean showItemsAssignedToOtherUsers, MDMBO.CallerContext callerContext);

        [OperationContract(Name = "GetWorkItemsPanelDetails")]
        MDMBO.Table GetWorkItemsPanelDetails(Int32 catalogId, Int32 userId, Boolean showEmptyItems, Boolean showItemsAssignedToOtherUsers, Boolean showBusinessCondition, MDMBO.CallerContext callerContext);

        [OperationContract(Name = "GetWorkItemDetails")]
        MDMBO.Table GetWorkItemDetails(Int64 entityId, MDMBO.CallerContext callerContext);

        [OperationContract(Name = "UpdateWorkflowInstances")]
        Boolean UpdateWorkflowInstances(String instanceGUIDs, String mdmObjectIDs, String mdmObjectType, Int32 workflowID, String activityShortName, String instanceStatus, String loginUser, String programName, ref MDMBO.OperationResult operationResult, MDMBO.CallerContext callerContext);

        #endregion UI Methods

        #region Escalation Service

        [OperationContract(Name = "ProcessEscalation")]
        Collection<MDMBOW.Escalation> ProcessEscalation(ref MDMBO.OperationResult operationResult, MDMBO.CallerContext callerContext);

        [OperationContract]
        [FaultContract(typeof(MDMBO.MDMExceptionDetails))]
        MDMBOW.WorkflowEscalationDataCollection GetWorkflowEscalationDetails(MDMBOW.WorkflowEscalationContext escalationContext, MDMBO.CallerContext callerContext);

        [OperationContract]
        [FaultContract(typeof(MDMBO.MDMExceptionDetails))]
        MDMBO.OperationResultCollection SendMailWithWorkflowEscalationDetails(MDMBOW.WorkflowEscalationContext escalationContext, MDMBO.EmailContext emailContext, Boolean includeAssignedUserAsRecipient, MDMBO.CallerContext callerContext);
        
        #endregion Escalation Service

        #region Designer Methods

        [OperationContract(Name = "ProcessWorkflows")]
        MDMBOW.WorkflowVersion ProcessWorkflows(Collection<MDMBOW.Workflow> workflows, String loginUser, ref MDMBO.OperationResult operationResult, MDMBO.CallerContext callerContext);

        [OperationContract(Name = "ProcessActivities")]
        Int32 ProcessActivities(Collection<MDMBOW.WorkflowActivity> workflowActivities, String loginUser, ref MDMBO.OperationResult operationResult, MDMBO.CallerContext callerContext);

        [OperationContract(Name = "GetWorkflowViewDetails")]
        void GetWorkflowViewDetails(Int32 workflowVersionID, String instanceGuid, ref MDMBOW.WorkflowVersion workflowVersion, ref Collection<MDMBOW.TrackedActivityInfo> trackedActivityCollection, ref MDMBO.OperationResult operationResult, MDMBO.CallerContext callerContext);

        #endregion Designer Methods

        #region Execution View Details Methods

        [OperationContract(Name = "GetWorkflowExecutionDetails")]
        MDMBOW.TrackedActivityInfoCollection GetWorkflowExecutionDetails(Int64 entityId, String workflowName, MDMBO.CallerContext callerContext, Boolean getAll = true);

        [OperationContract(Name = "GetWorkflowDoneReport")]
        Collection<DoneReportItem> GetWorkflowDoneReport(Int32 userId, String userParticipation, Collection<Int32> catalogIds, Collection<Int64> entityTypeIds, Collection<String> workflowNames, Collection<String> currentWorkflowActivity, Collection<Int32> attributeIds, Int32 localeId, Int64 countFrom, Int64 countTo, CallerContext callerContext);

        [OperationContract(Name = "GetWorkflowDoneReportWithAttributesDataSourceParameter")]
        Collection<DoneReportItem> GetWorkflowDoneReport(Int32 userId, String userParticipation, Collection<Int32> catalogIds, Collection<Int64> entityTypeIds, Collection<String> workflowNames, Collection<String> currentWorkflowActivity, Collection<Int32> attributeIds, Int32 localeId, Int64 countFrom, Int64 countTo, String attributesDataSource, CallerContext callerContext);

        #endregion Execution View Details Methods

        #region Entity workflow service

        [OperationContract(Name = "StartWorkflowWithEntityIdList")]
        EntityOperationResultCollection StartWorkflow(Collection<Int64> entityIdList, String workflowName, Int32 workflowVersionId, String serviceType, Int32 serviceId, CallerContext callerContext, String comments = "");

        [OperationContract(Name = "StartWorkflowWithEntityId")]
        EntityOperationResult StartWorkflow(Int64 entityId, String workflowName, Int32 workflowVersionId, String serviceType, Int32 serviceId, CallerContext callerContext, String comments = "");

        [OperationContract(Name = "StartWorkflowWithEntityList")]
        EntityOperationResultCollection StartWorkflow(EntityCollection entities, String workflowName, Int32 workflowVersionId, String serviceType, Int32 serviceId, CallerContext callerContext, String comments = "");

        [OperationContract(Name = "StartWorkflowWithEntity")]
        EntityOperationResult StartWorkflow(Entity entity, String workflowName, Int32 workflowVersionId, String serviceType, Int32 serviceId, CallerContext callerContext, String comments = "");

        [OperationContract(Name = "StartWorkflowWithEntityIdListAndRunOption")]
        EntityOperationResultCollection StartWorkflow(Collection<Int64> entityIdList, String workflowName, Int32 workflowVersionId, String serviceType, Int32 serviceId, WorkflowInstanceRunOptions workflowInstanceRunOption, CallerContext callerContext, String comments = "");

        [OperationContract(Name = "StartWorkflowWithEntityIdAndRunOption")]
        EntityOperationResult StartWorkflow(Int64 entityId, String workflowName, Int32 workflowVersionId, String serviceType, Int32 serviceId, WorkflowInstanceRunOptions workflowInstanceRunOption, CallerContext callerContext, String comments = "");

        [OperationContract(Name = "StartWorkflowWithEntityListAndRunOption")]
        EntityOperationResultCollection StartWorkflow(EntityCollection entities, String workflowName, Int32 workflowVersionId, String serviceType, Int32 serviceId, WorkflowInstanceRunOptions workflowInstanceRunOption, CallerContext callerContext, String comments = "");

        [OperationContract(Name = "StartWorkflowWithEntityAndRunOption")]
        EntityOperationResult StartWorkflow(Entity entity, String workflowName, Int32 workflowVersionId, String serviceType, Int32 serviceId, WorkflowInstanceRunOptions workflowInstanceRunOption, CallerContext callerContext, String comments = "");

        [OperationContract(Name = "ResumeWorkflowWithEntityIdList")]
        EntityOperationResultCollection ResumeWorkflow(Collection<Int64> entityIdList, String workflowName, Int32 workflowVersionId, String currentActivityName, String action, String comments, CallerContext callerContext);

        [OperationContract(Name = "ResumeWorkflowWithEntityList")]
        EntityOperationResultCollection ResumeWorkflow(EntityCollection entities, String workflowName, Int32 workflowVersionId, String currentActivityName, String action, String comments, CallerContext callerContext);

        [OperationContract(Name = "ChangeAssignment")]
        EntityOperationResultCollection ChangeAssignment(Collection<Int64> entityIds, String currentActivityName, SecurityUser newlyAssignedUser, String assignmentAction, CallerContext callerContext);

        #endregion Entity workflow service

        #region Workflow Details

        [OperationContract(Name = "GetWorkflowActivityPerformedUser")]
        [FaultContract(typeof(MDMExceptionDetails))]
        SecurityUser GetWorkflowActivityPerformedUser(Int64 entityId, String workflowName, String activityName, CallerContext callerContext);

        #endregion 

        [OperationContract(Name = "GetAppConfigValue")]
        String GetAppConfigValue(String keyName, ref MDMBO.OperationResult operationResult);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        WorkflowInfoCollection GetAllWorkflowInformation(CallerContext callerContext);
    }
}
