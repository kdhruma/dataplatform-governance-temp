using System;
using System.Collections.ObjectModel;

namespace MDM.Interfaces
{
    using WFBO = MDM.BusinessObjects.Workflow;

    /// <summary>
    /// Represents the interface for workflow information
    /// </summary>
    public interface IWorkflowInfoCollection
    {
        /// <summary>
        /// Get workflowinfo by workflowname, activityname and activityaction
        /// </summary>
        /// <param name="workflowName">Indicates the workflow name</param>
        /// <param name="workflowActivityName">Indicates the workflow activity name</param>
        /// <param name="workflowActivityAction">Indicates the workflow activity actiion</param>
        /// <returns>WorkflowInfo</returns>
        IWorkflowInfo GetWorkflowInfo(String workflowName, String workflowActivityName, String workflowActivityAction);

        /// <summary>
        /// Get all workflows available in the system
        /// </summary>
        /// <returns>Collection of Workflows</returns>
        Collection<WFBO.Workflow> GetWorkflows();

        /// <summary>
        /// Get workflow activities by workflow id
        /// </summary>
        /// <param name="workflowId">Indicates the workflow id</param>
        /// <returns>Collection of workflow activities</returns>
        Collection<WFBO.WorkflowActivity> GetWorkflowActivitiesByWorkflowId(Int32 workflowId);

        /// <summary>
        /// Get workflow activity action by workflow id and workflow activity id
        /// </summary>
        /// <param name="workflowId">Indicates the workflow id</param>
        /// <param name="workflowActivityId">Indicates the workflow acticity id</param>
        /// <returns>Collection of workflow activity actions</returns>
        Collection<WFBO.ActivityAction> GetWorkflowActivityActionsByWorkflowIdAndWorkflowActivityId(Int32 workflowId, Int32 workflowActivityId);
    }
}
