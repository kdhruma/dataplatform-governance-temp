using System;

namespace MDM.Interfaces
{
    /// <summary>
    /// Exposes methods or properties to set or get workflow escalation data details.
    /// </summary>
    public interface IWorkflowEscalationData
    {
        #region Properties

        /// <summary>
        /// Property denoting the entity Id.
        /// </summary>
        Int64 EntityId { get; set; }

        /// <summary>
        /// Property denoting the Workflow Id
        /// </summary>
        Int32 WorkflowId { get; set; }

        /// <summary>
        /// Property denoting the short name of the workflow.
        /// </summary>
        String WorkflowName { get; set; }

        /// <summary>
        /// Property denoting the long name of the workflow.
        /// </summary>
        String WorkflowLongName { get; set; }

        /// <summary>
        /// Property denoting the short name of the activity.
        /// </summary>
        String ActivityName { get; set; }

        /// <summary>
        /// Property denoting the long name of the activity.
        /// </summary>
        String ActivityLongName { get; set; }

        /// <summary>
        /// Property denoting the assigned user Id.
        /// </summary>
        Int32 AssignedUserId { get; set; }

        /// <summary>
        /// Property denoting the assigned user login.
        /// </summary>
        String AssignedUserLogin { get; set; }

        /// <summary>
        /// Property denoting the assigned user's mail address.
        /// </summary>
        String AssignedUserMailAddress { get; set; }

        /// <summary>
        /// Property denoting the elapsed time in hours for an Activity
        /// </summary>
        Int32 ElapsedTime { get; set; }

        #endregion
    }
}
