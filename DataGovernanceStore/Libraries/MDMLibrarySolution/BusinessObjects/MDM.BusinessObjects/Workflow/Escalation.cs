using System;
using System.Runtime.Serialization;

using MDM.Core;

namespace MDM.BusinessObjects.Workflow
{
    /// <summary>
    /// Specifies the WorkflowInstanceEscalation
    /// </summary>
    [DataContract]
    public class Escalation : MDMObject
    {
        #region Fields
			
		/// <summary>
		/// Represents RuntimeInstanceId of the WorkflowInstanceEscalation
		/// </summary>
		private String _runtimeInstanceId = String.Empty;
	
		/// <summary>
		/// Represents activity id which is participating in the escalation
		/// </summary>
		private Int32 _workflowActivityId = -1;

        /// <summary>
		/// Represents activity name which is participating in the escalation
		/// </summary>
		private String _activityShortName = String.Empty;

        /// <summary>
        /// Represents activity long name which is participating in the escalation
        /// </summary>
        private String _activityLongName = String.Empty;
	
		/// <summary>
		/// Represents whether currently escalation is active or not
		/// </summary>
		private Boolean _isActive;
	
		/// <summary>
		/// Represents EscalationLevel of the WorkflowInstanceEscalation
		/// </summary>
		private EscalationLevel _escalationLevel = EscalationLevel.None;

		/// <summary>
		/// Represents the user to whom the activity is assigned to.
		/// </summary>
		private String _escalatedFrom = String.Empty;

		/// <summary>
		/// Represents the user to whom the activity is escalated to.
		/// </summary>
		private String _escalatedTo = String.Empty;

        #endregion

        #region Constructors

		/// <summary>
        /// Parameterless Constructor
        /// </summary>
        public Escalation()
		{
        }

        /// <summary>
        /// Constructor with Id as input parameter
        /// </summary>
        /// <param name="id">Indicates the Identity of the WorkflowInstanceEscalation Instance</param>
        public Escalation(Int32 id)
            : base(id)
        {
            
        }

        /// <summary>
        /// Constructor with Object array. 
        /// </summary>
        /// <param name="objectArray">Object array containing values for Escalation</param>
        public Escalation(object[] objectArray)
        {
            if (objectArray[0] != null)
                ActivityShortName = objectArray[0].ToString();

            if (objectArray[1] != null)
                ActivityLongName = objectArray[1].ToString();

            if (objectArray[2] != null)
				EscalationLevel = (EscalationLevel)Enum.Parse(typeof(EscalationLevel), objectArray[2].ToString());

			if (objectArray[3] != null)
				EscalatedTo = objectArray[3].ToString();
		}

        #endregion

        #region Property
			
		/// <summary>
        /// Property denoting the Runtime InstanceId
        /// </summary>
        [DataMember]
        public String RuntimeInstanceId
        {
            get 
			{ 
				return _runtimeInstanceId; 
			}
            set 
			{ 
				_runtimeInstanceId = value; 
			}
        }
	
		/// <summary>
        /// Property denoting the Identity of the workflow activity
        /// </summary>
        [DataMember]
        public Int32 WorkflowActivityId
        {
            get 
			{
                return _workflowActivityId; 
			}
            set 
			{
                _workflowActivityId = value; 
			}
        }

        /// <summary>
        /// Property denoting the Activity's Short Name
        /// </summary>
        [DataMember]
        public String ActivityShortName
        {
            get
            {
                return _activityShortName;
            }
            set
            {
                _activityShortName = value;
            }
        }

        /// <summary>
        /// Property denoting the Activity's Long Name
        /// </summary>
        [DataMember]
        public String ActivityLongName
        {
            get
            {
                return _activityLongName;
            }
            set
            {
                _activityLongName = value;
            }
        }
	
		/// <summary>
        /// Property denoting whether the escalation is Active or not
        /// </summary>
        [DataMember]
        public Boolean IsActive
        {
            get 
			{ 
				return _isActive; 
			}
            set 
			{ 
				_isActive = value; 
			}
        }
	
		/// <summary>
        /// Property denoting the Escalation Level
        /// </summary>
        [DataMember]
		public EscalationLevel EscalationLevel
        {
            get 
			{ 
				return _escalationLevel; 
			}
            set 
			{ 
				_escalationLevel = value; 
			}
        }

		/// <summary>
        /// Property for Assigned to
		/// </summary>
		[DataMember]
		public String EscalatedFrom
		{
			get
			{
				return _escalatedFrom;
			}
			set
			{
				_escalatedFrom = value;
			}
		}

		/// <summary>
        /// Property for escalated to
        /// </summary>
        [DataMember]
        public String EscalatedTo
        {
            get 
			{
                return _escalatedTo; 
			}
            set 
			{
                _escalatedTo = value; 
			}
        }

        #endregion

		#region Methods

        #endregion
    }

    /// <summary>
    /// Defines the level of the Escalation.
    /// </summary>
	[DataContract]
	public enum EscalationLevel
	{
        /// <summary>
        /// Indicates an escalation level where user will be alerted regarding escalation
        /// </summary>
		[EnumMember]
		AlertUser,

        /// <summary>
        /// Indicates an escalation level where task will be escalated to manager
        /// </summary>
		[EnumMember]
		EscalateToManager,

        /// <summary>
        /// Indicates an escalation level where task assignment will be removed so that any other autorized user can act on the task
        /// </summary>
		[EnumMember]
		RemoveFromQueue,

        /// <summary>
        /// Indicates none
        /// </summary>
		[EnumMember]
		None
	}
}
