using System;

namespace MDM.Interfaces
{
    using MDM.BusinessObjects;
    using MDM.Core;

    /// <summary>
    /// Contains methods related to handling entity workflow options
    /// </summary>
    public interface IEntityWorkflowManager
    {
        EntityOperationResult StartWorkflow(Int64 entityId, String workflowName, Int32 workflowVersionId, String serviceType, Int32 serviceId, WorkflowInstanceRunOptions workflowInstanceRunOption, CallerContext callerContext, String comments = "");
    }
}
