using System;
using System.Collections.ObjectModel;

namespace MDM.Interfaces
{
    /// <summary>
    /// Exposes methods or properties to set or get workflow escalation context.
    /// </summary>
    public interface IWorkflowEscalationContext
    {
        #region Properties

        /// <summary>
        /// Property denoting the list of entity Ids.
        /// </summary>
        Collection<Int64> EntityIds { get; set; }

        /// <summary>
        /// Property denoting the security user login.
        /// This is an optional property.If not specified, all the users acting for the specified workflow and activities are considered.
        /// </summary>
        String UserLogin { get; set; }

        /// <summary>
        /// Property denoting the elapsed time in hours for an Activity.
        /// This is an optional property. If not specified, the elapsed time is 0.
        /// </summary>
        Int32 ElapsedTime { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Sets the workflow short name and an activity long name.
        /// Activity name is an optional parameter. If not specified, all the activities in the specified workflow are considered.
        /// </summary>
        /// <param name="workflowName">Indicates the workflow short name</param>
        /// <param name="activityName">Indicates an activity long name</param>
        void SetWorkflowAndActivityName(String workflowName, String activityName = "");

        /// <summary>
        /// Sets the workflow short name and list of activity long names.
        /// Activity names is an optional parameter. If not specified, all the activities in the specified workflow are considered.
        /// </summary>
        /// <param name="workflowName">Indicates the workflow short name</param>
        /// <param name="activityNames">Indicates the list of activity long names</param>
        void SetWorkflowAndActivityName(String workflowName, Collection<String> activityNames);

        #endregion
    }
}
