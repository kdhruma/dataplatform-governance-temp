using System;
using System.Runtime.Serialization;

namespace MDM.BusinessObjects.Workflow
{
    using MDM.Interfaces;

    /// <summary>
    /// Specifies the workflow escalation data object
    /// </summary>
    [DataContract]
    public class WorkflowEscalationData : IWorkflowEscalationData
    {
        #region Fields

        /// <summary>
        ///  Field denoting the entity Id
        /// </summary>
        private Int64 _entityId = -1;

        /// <summary>
        ///  Field denoting the workflow id
        /// </summary>
        private Int32 _workflowId = -1;

        /// <summary>
        /// Field denoting the workflow short name
        /// </summary>
        private String _workflowName = String.Empty;

        /// <summary>
        /// Field denoting the workflow long name
        /// </summary>
        private String _workflowLongName = String.Empty;

        /// <summary>
        /// Field denoting the activity short name
        /// </summary>
        private String _activityName = String.Empty;

        /// <summary>
        /// Field denoting the activity long name
        /// </summary>
        private String _activityLongName = String.Empty;

        /// <summary>
        /// Field denoting the Id of the user to whom the activity has been assigned
        /// </summary>
        private Int32 _assignedUserId = -1;

        /// <summary>
        /// Field denoting the user (login name) to whom the activity has been assigned
        /// </summary>
        private String _assignedUserLogin = String.Empty;

        /// <summary>
        /// Field denoting the email address of the assigned user
        /// </summary>
        private String _assignedUserMailAddress = String.Empty;

        /// <summary>
        /// Field denoting the elapsed time in hours
        /// </summary>
        private Int32 _elapsedTime = -1;

        #endregion

        #region Constructor

        /// <summary>
        /// Parameter less constructor
        /// </summary>
        public WorkflowEscalationData()
        {
            this._elapsedTime = 0;
        }

        #endregion 

        #region Properties

        /// <summary>
        /// Property denoting the entity Id.
        /// </summary>
        [DataMember]
        public Int64 EntityId
        {
            get
            {
                return this._entityId;
            }
            set
            {
                this._entityId = value;
            }
        }

        /// <summary>
        /// Property denoting the Workflow Id
        /// </summary>
        [DataMember]
        public Int32 WorkflowId
        {
            get
            {
                return this._workflowId;
            }
            set
            {
                this._workflowId = value;
            }
        }

        /// <summary>
        /// Property denoting the short name of the workflow.
        /// </summary>
        [DataMember]
        public String WorkflowName
        {
            get
            {
                return this._workflowName;
            }
            set
            {
                this._workflowName = value;
            }
        }

        /// <summary>
        /// Property denoting the long name of the workflow.
        /// </summary>
        [DataMember]
        public String WorkflowLongName
        {
            get
            {
                return this._workflowLongName;
            }
            set
            {
                this._workflowLongName = value;
            }
        }

        /// <summary>
        /// Property denoting the short name of the activity.
        /// </summary>
        [DataMember]
        public String ActivityName
        {
            get
            {
                return this._activityName;
            }
            set
            {
                this._activityName = value;
            }
        }

        /// <summary>
        /// Property denoting the long name of the activity.
        /// </summary>
        [DataMember]
        public String ActivityLongName
        {
            get
            {
                return this._activityLongName;
            }
            set
            {
                this._activityLongName = value;
            }
        }

        /// <summary>
        /// Property denoting the assigned user Id.
        /// </summary>
        [DataMember]
        public Int32 AssignedUserId
        {
            get
            {
                return this._assignedUserId;
            }
            set
            {
                this._assignedUserId = value;
            }
        }

        /// <summary>
        /// Property denoting the assigned user login.
        /// </summary>
        [DataMember]
        public String AssignedUserLogin
        {
            get
            {
                return this._assignedUserLogin;
            }
            set
            {
                this._assignedUserLogin = value;
            }
        }

        /// <summary>
        /// Property denoting the assigned user's mail address.
        /// </summary>
        [DataMember]
        public String AssignedUserMailAddress
        {
            get
            {
                return this._assignedUserMailAddress;
            }
            set
            {
                this._assignedUserMailAddress = value;
            }
        }

        /// <summary>
        /// Property denoting the elapsed time in hours for an Activity
        /// </summary>
        [DataMember]
        public Int32 ElapsedTime
        {
            get
            {
                return this._elapsedTime;
            }
            set
            {
                this._elapsedTime = value;
            }
        } 

        #endregion
    }
}


