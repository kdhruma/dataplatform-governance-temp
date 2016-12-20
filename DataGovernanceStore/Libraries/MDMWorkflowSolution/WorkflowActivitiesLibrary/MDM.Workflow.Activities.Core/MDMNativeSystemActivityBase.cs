using System;
using System.Activities;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MDM.BusinessObjects;
using MDM.Workflow.Utility;
using WFBO = MDM.BusinessObjects.Workflow;
namespace MDM.Workflow.Activities.Core
{
    public abstract class MDMNativeSystemActivityBase : NativeActivity, IMDMSystemActivitiy
    {
        private InArgument<String> _name = Guid.NewGuid().ToString();
        private InArgument<Boolean> _isHumanActivity = true;

		/// <summary>
		/// This property defines if these workflow activities need to be persisted to tb_Workflow_Activity table
		/// </summary>
		protected Boolean _persistToRepository = true;

		/// <summary>
		/// Specifies if the Activity in a workflow will be peristed to repository.
		/// </summary> 
		public Boolean PersistToRepository
	    {
		    get
		    {
			    return _persistToRepository;
		    }
		    
	    }

        #region IMDMSystemActivitiy Members

        /// <summary>
        /// Name of the Activity, used to uniquely identify the Activity in a workflow.
        /// </summary>
        [Browsable(false)]
        public InArgument<String> Name
        {
            get
            {
                return _name;
            }
            set
            {
                //Check for GenerateNewGUIDForActivity flag..
                //If it is true, generate new GUID and assign
                //otherwise assign incoming value
                //See bug no. : 12637 for further details
                if (WorkflowHelper.GenerateNewGUIDForActivity)
                    _name = Guid.NewGuid().ToString();
                else
                    _name = value;
            }
        }

        /// <summary>
        /// Description of the Activity,  used to store the description of the workflow
        /// </summary>
        [DisplayName("Description")]
        [Category("Misc")]
        public InArgument<String> Description
        {
            get;
            set;
        }

        /// <summary>
        /// Data Context in workflow client, provides input for Activity
        /// </summary>
        [DisplayName("MDM Data Context")]
        [Category("Input Arguments")]
        [RequiredArgument]
        public InArgument<BusinessObjects.Workflow.WorkflowDataContext> MDMDataContext { get; set; }

        /// <summary>
        /// Action Context in workflow client, provides action details for Activity
        /// </summary>
        [DisplayName("MDM Action Context")]
        [Category("Input and Output Arguments")]
        public InOutArgument<BusinessObjects.Workflow.WorkflowActionContext> MDMActionContext { get; set; }

        /// <summary>
        /// Flag which indicates whether activity is human interaction activity or not
        /// </summary>
        [Browsable(false)]
        public InArgument<Boolean> IsHumanActivity
        {
            get { return _isHumanActivity; }
            set { _isHumanActivity = value; }
        }

        /// <summary>
        /// List of comma separated role names, allowed to act on this activity
        /// </summary>
        [DisplayName("Allowed Roles")]
        [Category("Actors & Assignments")]
        public virtual InArgument<String> AllowedRoles { get; set; }

        /// <summary>
        /// Possible actions for this activity
        /// Ex: Approve,Reject
        /// </summary>
        [DisplayName("Actions")]
        [Category("Actions")]
        public virtual String Actions { get; set; }

        #endregion


        #region execution Methods
        protected virtual void BeforeExecute(NativeActivityContext context)
        {
           GetArgumentActivityContextValue(context);
        }

        /// <summary>
        /// Before the Execution any initialization of variables thats need to be done.
        /// </summary>
        /// <param name="context"></param>
        protected override void Execute(NativeActivityContext context)
        {
            try
            {
                BeforeExecute(context);
                OnExecute(context);
                AfterExecute(context);
            }
            catch (Exception ex)
            {
                context.Abort(ex);
            }
        }

        /// <summary>
        /// Main Activity Execution Logic
        /// </summary>
        /// <param name="context"></param>
        protected virtual void OnExecute(NativeActivityContext context)
        {

        }

        /// <summary>
        /// Execution logic to be executed at the endsuch as bookmarking etc.
        /// </summary>
        /// <param name="context"></param>
        protected virtual void AfterExecute(NativeActivityContext context)
        {
            #region Step 4:Create Bookmark Name
            CreateBookMark(context);
            #endregion
        }

        protected virtual void CreateBookMark(NativeActivityContext context)
        {
            //Prepare Bookmark Name
            String bookmarkName = WorkflowHelper.GetBookmarkName(ActivityNameActivityContextValue, context.WorkflowInstanceId.ToString());

            //Create Bookmark
            context.CreateBookmark(bookmarkName, new BookmarkCallback(Continue));
        }
        #endregion

        protected override bool CanInduceIdle
        {
            get
            {
                return true;
            }
        }

        #region Activity Argument Context Value Properties
        protected String ActivityNameActivityContextValue
        {
            get;
            set;
        }

        protected WFBO.WorkflowDataContext DataContextActivityContextValue
        {
            get;
            set;
        }
        protected WFBO.WorkflowActionContext ActionContextActivityContextValue
        {
            get;
            set;
        }

        protected String WorkflowRuntimeInstanceIdActivityContextValue
        {
            get;
            set;
        }

        #endregion

        protected virtual void GetArgumentActivityContextValue(NativeActivityContext context)
        {
            ActivityNameActivityContextValue = context.GetValue(Name);
            ActionContextActivityContextValue = context.GetValue(MDMActionContext);
            DataContextActivityContextValue = context.GetValue(MDMDataContext);
            WorkflowRuntimeInstanceIdActivityContextValue = context.WorkflowInstanceId.ToString();
        }

        protected virtual WFBO.WorkflowActionContext GetCurrentWorkflowActionContext()
        {
            WFBO.WorkflowActionContext actionContext = new WFBO.WorkflowActionContext();
            actionContext.Comments = DataContextActivityContextValue.WorkflowComments;
            actionContext.CurrentActivityName = ActivityNameActivityContextValue;
            actionContext.CurrentActivityLongName = this.DisplayName;
            actionContext.ActingUserName = DataContextActivityContextValue.UserName;
            actionContext.WorkflowName = DataContextActivityContextValue.WorkflowName;
            actionContext.WorkflowLongName = DataContextActivityContextValue.WorkflowLongName;

            return actionContext;
        }

        void Continue(NativeActivityContext context, Bookmark bookmark, Object obj)
        {
            //Set the action context
            MDMActionContext.Set(context, obj);
        }
    }
}
