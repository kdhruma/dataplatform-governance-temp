using System;

namespace MDM.Interfaces
{
    using MDM.BusinessObjects;
    using MDM.BusinessObjects.Workflow;
    using MDM.Core;

    /// <summary>
    /// Indicates WorkflowRuntimeBL interface
    /// </summary>
    public interface IWorkflowRuntimeManager
    {
        /// <summary>
        /// Starts the requested workflow
        /// </summary>
        /// <param name="workflowDataContext">The data context which will go as part of the instance lifetime</param>
        /// <param name="serviceType">The type of the service which is invoking the workflow</param>
        /// <param name="serviceId">The unique identification of the service</param>
        /// <param name="currentUserName">The user name who is requesting for the start</param>
        /// <param name="workflowInstanceRunOption">The run option which describes the way the workflow needs to be invoked</param>
        /// <param name="operationResult">Returns the operation result</param>
        /// <param name="callerContext">Context which tells which application/module called this API</param>
        /// <returns>The success count</returns>
        Int32 StartWorkflow(WorkflowDataContext workflowDataContext, String serviceType, Int32 serviceId, String currentUserName, WorkflowInstanceRunOptions workflowInstanceRunOption, ref OperationResult operationResult, CallerContext callerContext);

        /// <summary>
        /// Resumes workflow
        /// </summary>
        /// <param name="commaSeparatedRuntimeInstanceIds">Instance Ids which needs to be resumed</param>
        /// <param name="actionContext">The context which describes the action being performed</param>
        /// <param name="operationResult">Returns the operation result</param>
        /// <returns>The success count</returns>
        Int32 ResumeWorkflow(String commaSeparatedRuntimeInstanceIds, WorkflowActionContext actionContext, ref OperationResult operationResult, CallerContext callerContext);
    }
}