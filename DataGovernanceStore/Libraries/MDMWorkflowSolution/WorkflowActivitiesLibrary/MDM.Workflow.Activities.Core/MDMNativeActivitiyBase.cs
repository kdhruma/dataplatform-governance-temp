using System;
using System.Activities;
using System.Activities.Presentation.Metadata;
using System.Activities.Presentation.PropertyEditing;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;

namespace MDM.Workflow.Activities.Core
{
    using MDM.AdminManager.Business;
    using MDM.BusinessObjects;
    using MDM.EntityManager.Business;
    using MDM.Interfaces;
    using MDM.NotificationManager.Business;
    using MDM.Workflow.Activities.Designer;
    using MDM.Workflow.Designer.Business;
    using MDM.Workflow.Utility;
    using MDMC = MDM.Core;
    using MDM.Utility;
    using MDM.Workflow.PersistenceManager.Business;
    using MDM.Workflow.AssignmentManager.Business;
    using WFBO = MDM.BusinessObjects.Workflow;

    /// <summary>
    /// Any custom activity used in MDM Workflow System, which requires human interaction (bookmarks), should be derived from this activity class
    /// After deriving from this class, one doesn't need to implement IMDMActivity
    /// </summary>
    public abstract class MDMNativeActivitiyBase : MDMNativeSystemActivityBase, IMDMActivitiy
	{
		#region Fields

		private MDMC.AssignmentType _assignmentType = MDMC.AssignmentType.RoundRobin;
		private InArgument<Boolean> _notifyUser = false;
		private InArgument<Boolean> _notifyAllAssignedUsers = false;
		private InArgument<Boolean> _assignToPreviousActorOnRerun = true;
		private InArgument<Boolean> _displayUnassignedEntities = true;
		private InArgument<Boolean> _displayOtherUsersEntities = false;
		private InArgument<Boolean> _delegationAllowed = true;
		private InArgument<Boolean> _enableEscalation = false;
		private InArgument<Int32> _displayOrder = 0;


        private static Boolean _isRuntimeExecution = true;

        private static Object lockObject = new Object();
		#endregion

		#region Properties

		#region IMDMActivitiy Members
		#endregion

		[Browsable(false)]
		public InArgument<Int32> AssignedUser { get; set; }

		/// <summary>
		/// List of comma separated user names, allowed to act on this activity
		/// </summary>
		[DisplayName("Allowed Users")]
		[Category("Actors & Assignments")]
		public InArgument<String> AllowedUsers { get; set; }

		/// <summary>
		/// List of comma separated role names, allowed to act on this activity
		/// </summary>
		[DisplayName("Allowed Roles")]
		[Category("Actors & Assignments")]
        public override InArgument<String> AllowedRoles { get; set; }

		/// <summary>
		/// Assignment rule for this activity
		/// </summary>
		[DisplayName("Assignment Type")]
		[Category("Actors & Assignments")]
		public MDMC.AssignmentType Assignment_Type 
		{ 
			get
			{
				return _assignmentType;
			}
			set
			{
				_assignmentType = value;
				AssignmentType = _assignmentType.ToString();
			}
		}

		/// <summary>
		/// Assignment Type property for tracking.
		/// </summary>
		[Browsable(false)]
		public InArgument<String> AssignmentType { get; set; }

		/// <summary>
		/// Business Rule Assembly Name to use, for assignment
		/// </summary>
		[DisplayName("Assignment Business Rule Assembly Name")]
		[Category("Actors & Assignments")]
		public InArgument<String> AssignmentBusinessRuleAssemblyName { get; set; }

		/// <summary>
		/// Business Rule Type Name to use, for assignment
		/// </summary>
		[DisplayName("Assignment Business Rule Type Name")]
		[Category("Actors & Assignments")]
		public InArgument<String> AssignmentBusinessRuleTypeName { get; set; }

		/// <summary>
		/// Assign To Previous Actor On Rerun?
		/// </summary>
		[DisplayName("Assign To Previous Actor On Rerun?")]
		[Category("Actors & Assignments")]
		public InArgument<Boolean> AssignToPreviousActorOnRerun
		{
			get { return _assignToPreviousActorOnRerun; }
			set { _assignToPreviousActorOnRerun = value; }
		}

		/// <summary>
		/// Show Unassigned Entities?
		/// </summary>
		[DisplayName("Display Unassigned Entities?")]
		[Category("Actors & Assignments")]
		public InArgument<Boolean> DisplayUnassignedEntities
		{
			get { return _displayUnassignedEntities; }
			set { _displayUnassignedEntities = value; }
		}

		/// <summary>
		/// Show Entities Assigned To Other Users?
		/// </summary>
		[DisplayName("Display Other Users' Entities?")]
		[Category("Actors & Assignments")]
		public InArgument<Boolean> DisplayOtherUsersEntities
		{
			get { return _displayOtherUsersEntities; }
			set { _displayOtherUsersEntities = value; }
		}

		/// <summary>
		/// Possible actions for this activity
		/// Ex: Approve,Reject
		/// </summary>
		[DisplayName("Actions")]
		[Category("Actions")]
        public override String Actions { get; set; }

		/// <summary>
		/// Sorting order for activities on UI
		/// </summary>
		[DisplayName("Display Order")]
		[Category("Misc")]
		public InArgument<Int32> DisplayOrder
		{
			get { return _displayOrder; }
			set { _displayOrder = value; }
		}

		/// <summary>
		/// Flag which indicates whether escalation is enabled for this activity
		/// </summary>
		[DisplayName("Enable Escalation?")]
		[Category("Delegation & Escalations")]
		public InArgument<Boolean> EnableEscalation
		{
			get { return _enableEscalation; }
			set { _enableEscalation = value; }
		}

		/// <summary>
		/// Duration for escalation in hours
		/// </summary>
		[Browsable(false)]
		[DisplayName("Escalation Duration (Days)")]
		[Category("Delegation & Escalations")]
		public InArgument<Double> EscalationDuration { get; set; }

		/// <summary>
		/// Specifies the number of hours after which escalation should happen to user
		/// </summary>
		[DisplayName("Alert User After (Days)")]
		[Category("Delegation & Escalations")]
		public InArgument<Double> AlertUserAfter { get; set; }

		/// <summary>
		/// Specifies the number of hours after which escalation should happen to manager
		/// </summary>
		[DisplayName("Escalate To Manager After (Days)")]
		[Category("Delegation & Escalations")]
		public InArgument<Double> EscalateToManagerAfter { get; set; }

		/// <summary>
		/// Specifies the number of hours after which activity has to be removed from queue
		/// </summary>
		[DisplayName("Move To Unassigned Pool After (Days)")]
		[Category("Delegation & Escalations")]
		public InArgument<Double> MoveToUnassignedPoolAfter { get; set; }

		/// <summary>
		/// Flag which indicates whether delegation is enabled for this activity
		/// </summary>
		[DisplayName("Delegation Allowed?")]
		[Category("Delegation & Escalations")]
		public InArgument<Boolean> DelegationAllowed
		{ 
			get { return _delegationAllowed; }
			set { _delegationAllowed = value; }
		}

		/// <summary>
		/// Flag to indicate whether to send an MDM Message to user when items are assigned to them
		/// </summary>
		[DisplayName("Notify User?")]
		[Category("Notifications")]
		public InArgument<Boolean> NotifyUser
		{
			get { return _notifyUser; }
			set { _notifyUser = value; }
		}

		/// <summary>
		/// Specifies which MDM Message template to use when sending messages
		/// </summary>
		[DisplayName("Message Template Name")]
		[Category("Notifications")]
		public InArgument<String> MessageTemplate { get; set; }

		/// <summary>
		/// Flag to indicate whether to send MDM Message to all allowed users if assignment is queue or the business rule fails
		/// </summary>
		[DisplayName("On unassigned, notify all Allowed Users?")]
		[Category("Notifications")]
		public InArgument<Boolean> NotifyAllAssignedUsers
		{ 
			get { return _notifyAllAssignedUsers; }
			set { _notifyAllAssignedUsers = value; }
		} 

		/// <summary>
		/// Defines the Default Action for the Skip Criteria
		/// </summary>
        [DisplayName("Default Action for Skip Criteria for Actvity")]
        [Category("Skip Criteria")]
        public InArgument<String> DefaultActionForSkipCriteria
        {
            get;
            set;
        }

        /// <summary>
        /// Property denoting if the class is being called from workflow designer or from workflow execution.
        /// This is a hack to identify the context and execute a piece code which is needed only for UI. So we don't execute that for actual workflow execution.
        /// The value for this will be set to false from "DesignerMainPage.xaml.cs" - indicating its UI call.
        /// </summary>
        [Browsable(false)]
        public static Boolean IsRuntimeExecution
        {
            get { return MDMNativeActivitiyBase._isRuntimeExecution; }
            set { MDMNativeActivitiyBase._isRuntimeExecution = value; }
        }

		#endregion

		#region Constructors

		protected MDMNativeActivitiyBase()
		{
            Assignment_Type = MDM.Core.AssignmentType.RoundRobin;

            if (MDMNativeActivitiyBase.IsRuntimeExecution == false)
            {
                AttributeTableBuilder builder = new AttributeTableBuilder();
                builder.AddCustomAttributes(typeof(MDMNativeActivitiyBase), "Actions", new EditorAttribute(typeof(ActivityActionsEditor), typeof(DialogPropertyValueEditor)));
                lock (lockObject)
                {
                    MetadataStore.AddAttributeTable(builder.CreateTable());
                }
            }
		}

		#endregion

		#region Methods

		#region NativeActivity Overrides

		protected override void CacheMetadata(NativeActivityMetadata metadata)
		{
            try
            {
                base.CacheMetadata(metadata);
            }
            catch(Exception ex)
            {
                //Ignore exception here..
                //During concurrency, CacheMetadata is logging the error saying
                //'The invocation of the constructor on type 'MDM.Workflow.Activities.Core.HumanWork' that matches the specified binding constraints threw an exception.'
                //Inner Exception: 'Collection was modified; enumeration operation may not execute.'
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, ex.Message, MDMC.MDMTraceSource.AdvancedWorkflow);
            }

			//Validation
			if (String.IsNullOrEmpty(Actions))
			{
				metadata.AddValidationError("Value for a required activity property 'Actions' was not supplied.");
			}

			if (Assignment_Type == MDM.Core.AssignmentType.BusinessRule && (AssignmentBusinessRuleAssemblyName == null || AssignmentBusinessRuleAssemblyName.Expression == null || String.IsNullOrEmpty(AssignmentBusinessRuleAssemblyName.Expression.ToString())) && (AssignmentBusinessRuleTypeName == null || AssignmentBusinessRuleTypeName.Expression == null || String.IsNullOrEmpty(AssignmentBusinessRuleTypeName.Expression.ToString())))
			{
				metadata.AddValidationError("Value for a property 'AssignmentBusinessRule' is mandatory as Assignment Type is 'BusinessRule'.");
			}

			if ((AllowedRoles == null || AllowedRoles.Expression == null || String.IsNullOrEmpty(AllowedRoles.Expression.ToString())) && (AllowedUsers == null || AllowedUsers.Expression == null || String.IsNullOrEmpty(AllowedUsers.Expression.ToString())) && ((Assignment_Type != MDM.Core.AssignmentType.BusinessRule) || (Assignment_Type != MDM.Core.AssignmentType.ConfigurationTable)))
			{
				metadata.AddValidationError("Value for either 'Allowed Roles' or 'Allowed Users' is mandatory.");
			}

			if ((NotifyUser != null && NotifyUser.Expression != null && Boolean.Parse(NotifyUser.Expression.ToString())) && (MessageTemplate == null || MessageTemplate.Expression == null || String.IsNullOrEmpty(MessageTemplate.Expression.ToString())))
			{
				metadata.AddValidationError("'Message Template' is mandatory when 'Notify User is set to true'.");
			}

			if ((NotifyAllAssignedUsers != null && NotifyAllAssignedUsers.Expression != null && Boolean.Parse(NotifyAllAssignedUsers.Expression.ToString())) && (MessageTemplate == null || MessageTemplate.Expression == null || String.IsNullOrEmpty(MessageTemplate.Expression.ToString())))
			{
				metadata.AddValidationError("'Message Template' is mandatory when 'Notify All Assigned Users is set to true'.");
			}

			//Validation of Escalation Context
			Boolean enableEscalation = false;
			Double alertUserAfter = 0;
			Double escalateToManagerAfter = 0;
			Double moveToUnassignedPoolAfter = 0;

			if (EnableEscalation != null && EnableEscalation.Expression != null)
			{
				Boolean.TryParse(EnableEscalation.Expression.ToString(), out enableEscalation);
			}

			if (enableEscalation)
			{
				if (AlertUserAfter != null && AlertUserAfter.Expression != null)
				{
					Double.TryParse(AlertUserAfter.Expression.ToString(), out alertUserAfter);
				}

				if (EscalateToManagerAfter != null && EscalateToManagerAfter.Expression != null)
				{
					Double.TryParse(EscalateToManagerAfter.Expression.ToString(), out escalateToManagerAfter);
				}

				if (MoveToUnassignedPoolAfter != null && MoveToUnassignedPoolAfter.Expression != null)
				{
					Double.TryParse(MoveToUnassignedPoolAfter.Expression.ToString(), out moveToUnassignedPoolAfter);
				}

				if (alertUserAfter > 0)
				{
					if (escalateToManagerAfter > 0 && alertUserAfter > escalateToManagerAfter)
					{
						metadata.AddValidationError("'Alert User After (Days)' must be less than 'Escalate To Manager After (Days)'.");
					}

					if (moveToUnassignedPoolAfter > 0 && alertUserAfter > moveToUnassignedPoolAfter)
					{
						metadata.AddValidationError("'Alert User After (Days)' must be less than 'Move To Unassigned Pool After (Days)'.");
					}
				}

				if (escalateToManagerAfter > 0 && moveToUnassignedPoolAfter > 0 && escalateToManagerAfter > moveToUnassignedPoolAfter)
				{
					metadata.AddValidationError("'Escalate To Manager After (Days)' must be less than 'Move To Unassigned Pool After (Days)'.");
				}
			}
		}

	    protected override void GetArgumentActivityContextValue(NativeActivityContext context)
	    {
            ActivityNameActivityContextValue = context.GetValue(Name);
            ActionContextActivityContextValue = context.GetValue(MDMActionContext);
            DataContextActivityContextValue = context.GetValue(MDMDataContext);
            WorkflowRuntimeInstanceIdActivityContextValue = context.WorkflowInstanceId.ToString();

            NotifyUserActivityContextValue = context.GetValue(NotifyUser);
            NotifyAllAssignedUsersActivityContextValue = context.GetValue(NotifyAllAssignedUsers);
            MessageTemplateActivityContextValue = context.GetValue(MessageTemplate); 
            DefaultActionforSkipCriteriaActivityContextValue = context.GetValue(DefaultActionForSkipCriteria); 
        }

	    #region Activity Argument Context Value Properties

        protected Boolean NotifyUserActivityContextValue
        {
            get;
            set;
        }
        protected Boolean NotifyAllAssignedUsersActivityContextValue
        {
            get;
            set;
        }
        protected String MessageTemplateActivityContextValue
        {
            get;
            set;
        }

        protected String DefaultActionforSkipCriteriaActivityContextValue
        {
            get;
            set;
        } 
        #endregion

        protected override void Execute(NativeActivityContext context)
        {
            //Intitialize the variables
            BeforeExecute(context);

            CallerContext callerContext = new CallerContext(MDMC.MDMCenterApplication.WindowsWorkflow, MDMC.MDMCenterModules.WindowsWorkflow);

            Boolean entryCriteria = false;

            

            Collection<Int64> entityIds = DataContextActivityContextValue.MDMObjectCollection.GetMDMObjectIds();
            if ((entityIds != null) && entityIds.Any())
            {
                Int64 entityId = entityIds[0];

                entryCriteria = BusinessRuleProcessor.EvaluateActivitySkipCriteria(entityId, GetCurrentWorkflowActionContext(), DataContextActivityContextValue.WorkflowVersionId, ActivityNameActivityContextValue, context.ActivityInstanceId, callerContext);
                //Evalute the Skip Criterial if false execute the activity
                if (!entryCriteria)
                {
                    OnExecute(context);
                    AfterExecute(context);
                }
                else //Since Execution is skipped set the MDMActionContext UserAction value to DefaultActionForSkipCriteria
                {
                    if (!String.IsNullOrEmpty(DefaultActionforSkipCriteriaActivityContextValue))
                    {
                        ActionContextActivityContextValue.UserAction = DefaultActionforSkipCriteriaActivityContextValue;
                        context.SetValue(MDMActionContext, ActionContextActivityContextValue);
                    }
                }
            }

        }


        protected override void OnExecute(NativeActivityContext context)
	    {

            SecurityUser assignedUser = null;
            OperationResult operationResult = new OperationResult();

            try
            {
                #region Step 1:Assign User

                WorkflowActivityBL workflowActivityBL = null;
                MDMC.AssignmentType assignmentType = MDMC.AssignmentType.RoundRobin;
                Enum.TryParse<MDMC.AssignmentType>(context.GetValue(AssignmentType), out assignmentType);
                String allowedUsers = context.GetValue(AllowedUsers);
                String allowedRoles = context.GetValue(AllowedRoles);
                Boolean assignToPreviousActorOnRerun = context.GetValue(AssignToPreviousActorOnRerun);

                //Assign user by processing assignment type
                switch (assignmentType)
                {
                    case MDMC.AssignmentType.Queue:
                        if (assignToPreviousActorOnRerun)
                        {
                            workflowActivityBL = new WorkflowActivityBL();
                            assignedUser = workflowActivityBL.AssignUser(context.WorkflowInstanceId.ToString(), ActivityNameActivityContextValue, assignmentType, allowedUsers, allowedRoles, assignToPreviousActorOnRerun);
                        }
                        break;
                    case MDMC.AssignmentType.RoundRobin:
                    case MDMC.AssignmentType.LeastQueueSize:
                        workflowActivityBL = new WorkflowActivityBL();
                        assignedUser = workflowActivityBL.AssignUser(context.WorkflowInstanceId.ToString(), ActivityNameActivityContextValue, assignmentType, allowedUsers, allowedRoles, assignToPreviousActorOnRerun);
                        break;
                    case MDMC.AssignmentType.ConfigurationTable:
                        if (assignToPreviousActorOnRerun)
                        {
                            //Find out previously acted user on this activity...
                            workflowActivityBL = new WorkflowActivityBL();
                            assignedUser = workflowActivityBL.AssignUser(context.WorkflowInstanceId.ToString(), ActivityNameActivityContextValue, assignmentType, allowedUsers, allowedRoles, assignToPreviousActorOnRerun);
                        }

                        if (assignedUser == null)
                        {
                            //Previously acted user is not available or the activity is being executed first time.

                            //Get the assignments from the assignment lookup table 
                            assignedUser = new AssignmentRuleManager().GetAssignedUser(DataContextActivityContextValue, this.DisplayName, ActivityNameActivityContextValue);
                        }
                        break;
                    case MDMC.AssignmentType.BusinessRule:
                        if (assignToPreviousActorOnRerun)
                        {
                            //Find out previously acted user on this activity...
                            workflowActivityBL = new WorkflowActivityBL();
                            assignedUser = workflowActivityBL.AssignUser(context.WorkflowInstanceId.ToString(), ActivityNameActivityContextValue, assignmentType, allowedUsers, allowedRoles, assignToPreviousActorOnRerun);
                        }

                        if (assignedUser == null)
                        {
                            //Previously acted user is not available or the activity is being executed first time.

                            //Execute Business Rule to find out assignment

                            //Get required data
                            String assignmentBRAssemblyName = context.GetValue(AssignmentBusinessRuleAssemblyName);
                            String assignmentBRTypeName = context.GetValue(AssignmentBusinessRuleTypeName);

                            if (!String.IsNullOrEmpty(assignmentBRAssemblyName) && !String.IsNullOrEmpty(assignmentBRTypeName))
                            {
                                //Create workflow rule context
                                WFBO.WorkflowBusinessRuleContext workflowContext = new WFBO.WorkflowBusinessRuleContext(DataContextActivityContextValue, ActivityNameActivityContextValue, this.DisplayName, ActionContextActivityContextValue);

                                String assemblyPath = GetAssemblyPath();

                                if (!String.IsNullOrEmpty(assemblyPath))
                                {
                                    //Process rule
                                    BusinessRuleProcessor businessRuleProcessor = new BusinessRuleProcessor();
                                    businessRuleProcessor.ProcessRules(workflowContext, assemblyPath, assignmentBRAssemblyName, assignmentBRTypeName, operationResult);

                                    if (operationResult.ReturnValues != null && operationResult.ReturnValues.Count > 0 && operationResult.ReturnValues[0] is SecurityUser)
                                    {
                                        assignedUser = operationResult.ReturnValues[0] as SecurityUser;

                                        if (assignedUser != null)
                                        {
                                            if (!String.IsNullOrWhiteSpace(assignedUser.SecurityUserLogin) && String.IsNullOrWhiteSpace(assignedUser.Smtp))
                                            {
                                                //Get User Details
                                                SecurityUserBL securityManager = new SecurityUserBL();
                                                assignedUser = securityManager.GetUser(assignedUser.SecurityUserLogin);
                                            }
                                            else if (assignedUser.Id < 1)
                                            {
                                                assignedUser = null;
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        break;
                }

                if (assignedUser != null && assignedUser.Disabled == false)
                {
                    //Set User
                    context.SetValue(AssignedUser, assignedUser.Id);
                }

                #endregion

                #region Step 2:Send Mail

                //Alert user only if requested
                if (DataContextActivityContextValue != null && (NotifyUserActivityContextValue || NotifyAllAssignedUsersActivityContextValue))
                {
                    CallerContext callerContext = new CallerContext(DataContextActivityContextValue.Application, DataContextActivityContextValue.Module);
                    MailNotificationBL mailNotificationBL = new MailNotificationBL();

                    //If assignedUser is not null, that means instance assigned to that user, so send message only to that user
                    if (assignedUser != null && NotifyUserActivityContextValue)
                    {
                        mailNotificationBL.SendMailOnWorkflowAssignment(DataContextActivityContextValue, ActivityNameActivityContextValue, this.DisplayName, assignedUser, new SecurityUserCollection() { assignedUser }, ActionContextActivityContextValue, MessageTemplateActivityContextValue, callerContext);
                    }
                    //Otherwise, send message to all users who can participate in that activity (in case of queue)
                    else if (NotifyAllAssignedUsersActivityContextValue)
                    {
                        WorkflowInstanceBL workflowInstanceManager = new WorkflowInstanceBL();
                        Collection<SecurityUser> activityAllowedUsers = workflowInstanceManager.GetUsersAllowedForActivity(null, ActivityNameActivityContextValue, DataContextActivityContextValue.WorkflowVersionId);

                        SecurityUserCollection toUsers = new SecurityUserCollection();
                        if (activityAllowedUsers != null && activityAllowedUsers.Count > 0)
                        {
                            foreach (SecurityUser user in activityAllowedUsers)
                            {
                                toUsers.Add(user);
                            }
                        }

                        mailNotificationBL.SendMailOnWorkflowAssignment(DataContextActivityContextValue, ActivityNameActivityContextValue, this.DisplayName, null, toUsers, ActionContextActivityContextValue, MessageTemplateActivityContextValue, callerContext);
                    }
                }

                #endregion
            }
            catch
            {
                //Any exceptions occurring during assignments are ignored.. 
                //Re throwing here causes the workflow to abort
                //If assignment fails, it behaves as a 'Queue' where any user in the assigned roles and assigned user can pick the activity

                //Also failure in sending message is not a failure of workflow..

                //Any exceptions occurring outside of the Try block will be handled by FaultTracking and
                //Workflow will be aborted..
            }

            
            base.OnExecute(context); 

        }

        private String GetAssemblyPath()
	    {
            String baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            String configDirectoryPath = AppConfigurationHelper.GetAppConfig<String>("MDMCenter.BR.ConfigDirectory");
            return String.Concat(baseDirectory, configDirectoryPath, "\\bin\\");
        }
        
	    protected override bool CanInduceIdle
		{
			get
			{
				return true;
			}
		}

		#endregion
		#endregion
	}
}
