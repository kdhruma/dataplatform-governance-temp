using System;
using System.Activities;
using System.ComponentModel;

using MDM.Workflow.Utility;
using WFBO = MDM.BusinessObjects.Workflow;

namespace MDM.Workflow.Activities.Core
{
    /// <summary>
    /// Any custom activity used in MDM Workflow System, which does not require human interaction (bookmarks), should be derived from this activity class
    /// After deriving from this class, one doesn't need to implement IMDMActivity
    /// </summary>
    public abstract class MDMCodeActivitiyBase<T> : CodeActivity<T>, IMDMActivitiy
    {
        #region Fields

        private InArgument<String> _name = Guid.NewGuid().ToString();

        private InArgument<Boolean> _isHumanActivity = false;

        #endregion

        #region Properties

        #region IMDMWorkflowBaseActivitiy Members

        /// <summary>
        /// Name of the Activity, used to uniquely identify the Activity in this workflow
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
        /// Flag which indicates whether activity is human interaction activity or not
        /// </summary>
        [Browsable(false)]
        public InArgument<Boolean> IsHumanActivity
        {
            get
            {
                return _isHumanActivity;
            }
            set
            {
                _isHumanActivity = value;
            }
        }

        /// <summary>
        /// Data Context in workflow client, provides input for Activity
        /// </summary>
        [DisplayName("MDM Data Context")]
        [Category("Input Arguments")]
        [RequiredArgument]
        public InArgument<WFBO.WorkflowDataContext> MDMDataContext { get; set; }

        /// <summary>
        /// Action Context in workflow client, provides action details for Activity
        /// </summary>
        [DisplayName("MDM Action Context")]
        [Category("Input and Output Arguments")]
        public InOutArgument<WFBO.WorkflowActionContext> MDMActionContext { get; set; }

        #endregion

        #endregion

    }
}
