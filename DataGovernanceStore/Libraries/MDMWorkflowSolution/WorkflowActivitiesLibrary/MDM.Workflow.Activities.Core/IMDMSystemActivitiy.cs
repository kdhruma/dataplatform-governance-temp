using System.Activities;
using MDM.BusinessObjects.Workflow;
using WorkflowDataContext = MDM.BusinessObjects.Workflow.WorkflowDataContext;

namespace MDM.Workflow.Activities.Core
{
    /// <summary>
    /// Any system activity used in MDM Workflow System must implement this interface
    /// The system activity which derives from NativeActivityBase or CodeActivityBase does not need to implement this
    /// </summary>
    public interface IMDMSystemActivitiy
    {
        #region Properties

        /// <summary>
        /// Name of the Activity, used to uniquely identify the Activity in this workflow
        /// </summary>
        InArgument<string> Name { get; set; }

        /// <summary>
        /// Description of the Activity,  used to store the information of the activity
        /// </summary>
        InArgument<string> Description
        {
            get;
            set;
        }

        /// <summary>
        /// Data Context in workflow client, provides input for Activity
        /// </summary>
        InArgument<WorkflowDataContext> MDMDataContext { get; set; }

        /// <summary>
        /// Action Context in workflow client, provides action details for Activity
        /// </summary>
        InOutArgument<WorkflowActionContext> MDMActionContext { get; set; }

        #endregion

    }
}