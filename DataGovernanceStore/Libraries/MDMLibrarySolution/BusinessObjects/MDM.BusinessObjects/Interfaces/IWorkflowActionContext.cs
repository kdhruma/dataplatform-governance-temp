using System;

namespace MDM.Interfaces
{
    using MDM.Core;

    /// <summary>
    /// Exposes methods or properties to set or get the context for workflow actions.
    /// </summary>
    public interface IWorkflowActionContext : IMDMObject
    {
        #region Properties

        /// <summary>
        /// Property denoting the Workflow short Name 
        /// </summary>
        String WorkflowName { get; set; }

        /// <summary>
        /// Property denoting the Workflow Long Name 
        /// </summary>
        String WorkflowLongName { get; set; }

        /// <summary>
        /// Currently executing activity.
        /// This is used to generate bookmark name
        /// </summary>
        String CurrentActivityName { get; set; }

        /// <summary>
        ///Property denoting the currently executing activity Long Name.
        /// </summary>
        String CurrentActivityLongName { get; set; }

        /// <summary>
        /// Represents what action is being performed on the entity and based on the action, what action is required on the workflow.
        /// </summary>
        String UserAction { get; set; }

        /// <summary>
        /// Represents which user is working on the entity and thus indirectly on the workflow.
        /// </summary>
        String ActingUserName { get; set; }

        /// <summary>
        /// Represents which user(id) is working on the entity and thus indirectly on the workflow.
        /// </summary>
        Int32 ActingUserId { get; set; }

        /// <summary>
        /// Comment for the particular action on the workflow.
        /// </summary>
        String Comments { get; set; }

        /// <summary>
        /// Represents the data which user wants to send to workflow runtime which is not predefined.
        /// The data should be in the format of XML.
        /// <!--<ExtendedProperties><Property Key = "" Value = "" /></ExtendedProperties>-->
        /// </summary>
        String ExtendedProperties { get; set; }

        /// <summary>
        /// Property denoting Previous assigned user(id) who was working on the entity and thus indirectly on the workflow.
        /// </summary>
        Int32 PreviousAssignedUserId { get; set; }

        /// <summary>
        /// Property denoting Previous assigned user name who was working on the entity and thus indirectly on the workflow.
        /// </summary>
        String PreviousAssignedUserName { get; set; }

        /// <summary>
        /// Property denoting Newly assigned user(id) who is working on the entity and thus indirectly on the workflow.
        /// </summary>
        Int32 NewlyAssignedUserId { get; set; }

        /// <summary>
        /// Property denoting Newly assigned user name who is working on the entity and thus indirectly on the workflow.
        /// </summary>
        String NewlyAssignedUserName { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Get XML representation of WorkflowActionContext  object
        /// </summary>
        /// <returns>XML representation of WorkflowActionContext  object</returns>
        String ToXml();

        #endregion
    }
}
