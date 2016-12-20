using System;

namespace MDM.Interfaces
{
    using MDM.Core;

    /// <summary>
    /// Exposes methods or properties to set or get all tracked activities related information.
    /// </summary>
    public interface ITrackedActivityInfo : IMDMObject
    {
        #region Properties

        /// <summary>
        /// Property denoting the Workflow Id
        /// </summary>
        Int32 WorkflowId { get; set; }

        /// <summary>
        /// Property denoting the Workflow Short Name 
        /// </summary>
        String WorkflowName { get; set; }

        /// <summary>
        /// Property denoting the Workflow Long Name 
        /// </summary>
        String WorkflowLongName { get; set; }

        /// <summary>
        /// Property denoting the Workflow Version Id
        /// </summary>
        Int32 WorkflowVersionId { get; set; }

        /// <summary>
        /// Property denoting the Workflow Version long Name 
        /// </summary>
        String WorkflowVersionName { get; set; }

        /// <summary>
        /// Property denoting the Id of the running workflow which uniquely identifies running instance
        /// </summary>
        String RuntimeInstanceId { get; set; }

        /// <summary>
        /// Property denoting Unique Name of an activity
        /// </summary>
        String ActivityShortName { get; set; }

        /// <summary>
        /// Property denoting the Name of the activity as displayed in the designer(Display Name)
        /// </summary>
        String ActivityLongName { get; set; }

        /// <summary>
        /// Property denoting whether the activity is Executing(Running) or not
        /// </summary>
        Boolean IsExecuting { get; set; }

        /// <summary>
        /// Property denoting the Status of the Activity
        /// </summary>
        String Status { get; set; }

        /// <summary>
        /// Property denoting the PerformedAction by the Activity
        /// </summary>
        String PerformedAction { get; set; }

        /// <summary>
        /// Property denoting the Id of the User who took the ownership of the activity and under whom the activity is currently waiting/running
        /// </summary>
        Int32 ActingUserId { get; set; }

        /// <summary>
        /// Property denoting the name of the User who took the ownership of the activity and under whom the activity is currently waiting/running
        /// </summary>
        String ActingUser { get; set; }

        /// <summary>
        /// Property denoting the Id of the user who performed the activity
        /// </summary>
        Int32 ActedUserId { get; set; }

        /// <summary>
        /// Property denoting the user who performed the activity
        /// </summary>
        String ActedUser { get; set; }

        /// <summary>
        /// Property denoting mail address of the acting/acted user
        /// </summary>
        String UserMailAddress { get; set; }
        
        /// <summary>
        /// Property denoting the comments entered for an action
        /// </summary>
        String ActivityComments { get; set; }

         /// <summary>
        /// Property denoting the comments entered for the Workflow.
        /// </summary>
        String WorkflowComments { get; set; }

        /// <summary>
        /// Property denoting the date time at which tracking record has been generated
        /// </summary>
        String EventDate { get; set; }

        /// <summary>
        /// Property denoting previous activity short name
        /// </summary>
        String PreviousActivityShortName { get; set; }

        #endregion

        #region Methods

        #region Public Methods

        /// <summary>
        /// Get Xml representation of TrackedActivityInfo object
        /// </summary>
        /// <returns>Xml string representing the TrackedActivityInfo</returns>
        String ToXml();

        /// <summary>
        /// Get Xml representation of the object
        /// </summary>
        /// <param name="objectSerialization">Type of the serialization option</param>
        /// <param name="activityDetailOnly">If it is false then return object and collection details else only object detail </param>
        /// <returns>Xml representation of the object</returns>
        String ToXml(ObjectSerialization objectSerialization, bool activityDetailOnly);

        #endregion

        #region Private Methods
        #endregion

        #endregion
    }
}
