using System;

namespace MDM.Interfaces
{
    using MDM.Core;

    /// <summary>
    /// Exposes methods or properties to set or get "Done Report" item workflow information. 
    /// </summary>
    public interface IDoneReportItemWorkflowInfo : IMDMObject
    {
        #region Properties

        /// <summary>
        /// Property denoting the Id of an entity
        /// </summary>
        new Int64 Id { get; set; }

        /// <summary>
        ///Property denoting the Id of the running workflow which uniquely identifies running instance
        /// </summary>
        String RuntimeInstanceId { get; set; }

        /// <summary>
        /// Property denoting WorkflowId
        /// </summary>
        Int32 WorkflowId { get; set; }

        /// <summary>
        /// Property denoting WorkflowShortName
        /// </summary>
        String WorkflowShortName { get; set; }

        /// <summary>
        /// Property denoting WorkflowLongName
        /// </summary>
        String WorkflowLongName { get; set; }

        /// <summary>
        /// Property denoting WorkflowVersionId
        /// </summary>
        Int32 WorkflowVersionId { get; set; }

        /// <summary>
        /// Property denoting ActivityId
        /// </summary>
        Int32 ActivityId { get; set; }

        /// <summary>
        /// Property denoting ActivityShortName
        /// </summary>
        String ActivityShortName { get; set; }

        /// <summary>
        /// Property denoting ActivityLongName
        /// </summary>
        String ActivityLongName { get; set; }

        /// <summary>
        /// Property denoting ActingUserId
        /// </summary>
        Int32? ActingUserId { get; set; }

        /// <summary>
        /// Property denoting ActingUserLogin
        /// </summary>
        String ActingUserLogin { get; set; }

        /// <summary>
        /// Property denoting ActingUserFirstName
        /// </summary>
        String ActingUserFirstName { get; set; }

        /// <summary>
        /// Property denoting ActingUserLastName
        /// </summary>
        String ActingUserLastName { get; set; }

        #endregion
    }
}