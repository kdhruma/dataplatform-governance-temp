using System;

namespace MDM.Interfaces
{
    using MDM.Core;

	/// <summary>
    /// Exposes methods or properties to set or get workflow state related information.
	/// </summary>
    public interface IWorkflowState
    {
        #region Properties

        /// <summary>
        /// Represents the Workflow Id
        /// </summary>
        Int32 WorkflowId { get; set; }

        /// <summary>
        /// Represents the Workflow Short Name 
        /// </summary>
        String WorkflowName { get; set; }

        /// <summary>
        /// Represents the Workflow Long Name 
        /// </summary>
        String WorkflowLongName { get; set; }

        /// <summary>
        /// Represents the Workflow Version Id
        /// </summary>
        Int32 WorkflowVersionId { get; set; }

        /// <summary>
        /// Represents the Workflow Version long Name 
        /// </summary>
        String WorkflowVersionName { get; set; }

        /// <summary>
        /// Represents the Workflow Instance Id
        /// </summary>
        Int32 InstanceId { get; set; }

        /// <summary>
        /// Represents the ID of the activity as per the Workflow Definition
        /// </summary>
        Int32 ActivityId { get; set; }

        /// <summary>
        /// Unique Name of an activity
        /// </summary>
        String ActivityShortName { get; set; }

        /// <summary>
        /// Name of the activity as displayed in the designer(Display Name)
        /// </summary>
        String ActivityLongName { get; set; }

        /// <summary>
        /// User to role the activity has been assigned
        /// </summary>
        String AssignedRole { get; set; }

        /// <summary>
        /// User to whom the activity has been assigned
        /// </summary>
        String AssignedUser { get; set; }

        /// <summary>
        /// UserId to whom the activity has been assigned
        /// </summary>
        Int32 AssignedUserId { get; set; }

        /// <summary>
        /// Property denoting previous activity short name.
        /// </summary>
        String PreviousActivityShortName { get; set; }

        /// <summary>
        /// Property denoting previous activity long name.
        /// </summary>
        String PreviousActivityLongName { get; set; }

        /// <summary>
        /// Property denoting previous activity user id.
        /// </summary>
        Int32 PreviousActivityUserId { get; set; }

        /// <summary>
        /// Property denoting previous activity user.
        /// </summary>
        String PreviousActivityUser { get; set; }

        /// <summary>
        /// Property denoting previous activity comments.
        /// </summary>
        String PreviousActivityComments { get; set; }

        /// <summary>
        /// Property denoting PerformedAction by the last Activity
        /// </summary>
        String PreviousActivityAction { get; set; }

        /// <summary>
        /// Property denoting event data of the last Activity
        /// </summary>
        String PreviousActivityEventDate { get; set; }

        /// <summary>
        /// Property denoting the date time at which tracking record has been generated
        /// </summary>
        String EventDate { get; set; }

         /// <summary>
         ///   Property denoting the comments entered for the Workflow.
         /// </summary>
        String WorkflowComments { get; set; }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Get XML representation of WorkflowState object
        /// </summary>
        /// <returns>XML representation of WorkflowState object</returns>
        String ToXml();

        /// <summary>
        /// Get XML representation of WorkflowState object
        /// </summary>
        /// <param name="serializationOption">serialization option. Based on the value selected, the different xml representation will be there</param>
        /// <returns>XML representation of WorkflowState object</returns>
        String ToXml(ObjectSerialization serializationOption);

        #endregion Methods
    }
}