using System;
using System.Runtime.Serialization;
using MDM.Core;
using MDM.Interfaces;

namespace MDM.BusinessObjects
{
    /// <summary>
    /// Specifies an DoneReportItemWorkflowInfo
    /// </summary>
    [DataContract]
    public class DoneReportItemWorkflowInfo : MDMObject, IDoneReportItemWorkflowInfo
    {
        #region Fields

        #endregion

        #region Constructors

        #endregion

        #region Properties

        /// <summary>
        /// Property denoting the Id of an entity
        /// </summary>
        [DataMember]
        public new Int64 Id { get; set; }

        /// <summary>
        ///Property denoting the Id of the running workflow which uniquely identifies running instance
        /// </summary>
        [DataMember]
        public String RuntimeInstanceId { get; set; }

        /// <summary>
        /// Property denoting WorkflowId
        /// </summary>
        [DataMember]    
        public Int32 WorkflowId { get; set; }

        /// <summary>
        /// Property denoting WorkflowShortName
        /// </summary>
        [DataMember]
        public String WorkflowShortName { get; set; }

        /// <summary>
        /// Property denoting WorkflowLongName
        /// </summary>
        [DataMember]
        public String WorkflowLongName { get; set; }

        /// <summary>
        /// Property denoting WorkflowVersionId
        /// </summary>
        [DataMember]
        public Int32 WorkflowVersionId { get; set; }

        /// <summary>
        /// Property denoting ActivityId
        /// </summary>
        [DataMember]
        public Int32 ActivityId { get; set; }

        /// <summary>
        /// Property denoting ActivityShortName
        /// </summary>
        [DataMember]
        public String ActivityShortName { get; set; }

        /// <summary>
        /// Property denoting ActivityLongName
        /// </summary>
        [DataMember]
        public String ActivityLongName { get; set; }

        /// <summary>
        /// Property denoting ActingUserId
        /// </summary>
        [DataMember]
        public Int32? ActingUserId { get; set; }

        /// <summary>
        /// Property denoting ActingUserLogin
        /// </summary>
        [DataMember]
        public String ActingUserLogin { get; set; }

        /// <summary>
        /// Property denoting ActingUserFirstName
        /// </summary>
        [DataMember]
        public String ActingUserFirstName { get; set; }

        /// <summary>
        /// Property denoting ActingUserLastName
        /// </summary>
        [DataMember]
        public String ActingUserLastName { get; set; }

        #endregion
    }
}