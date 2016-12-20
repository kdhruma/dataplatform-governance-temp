using System;

namespace MDM.Interfaces
{
    /// <summary>
    /// Represents the interface for workflowinfo
    /// </summary>
    public interface IWorkflowInfo
    {
        /// <summary>
        /// Property denoting the workflow id
        /// </summary>
        Int32 WorkflowId { get; set; }

        /// <summary>
        /// Property denoting the workflow name 
        /// </summary>
        String WorkflowName { get; set; }

        /// <summary>
        /// Property denoting the workflow version 
        /// </summary>
        Int32 WorkflowVersionNumber { get; set; }

        /// <summary>
        /// Property denoting the workflow activity id 
        /// </summary>
        Int32 WorkflowActivityId { get; set; }

        /// <summary>
        /// Property denoting the workflow activity shortname
        /// </summary>
        String WorkflowActivityShortName { get; set; }

        /// <summary>
        /// Property denoting the workflow activity longname
        /// </summary>
        String WorkflowActivityLongName { get; set; }

        /// <summary>
        /// Property denoting the workflow activity action id
        /// </summary>
        Int32 WorkflowActivityActionId { get; set; }

        /// <summary>
        /// Property denoting the workflow activity action
        /// </summary>
        String WorkflowActivityAction { get; set; }
    }
}
