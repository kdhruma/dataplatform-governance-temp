using System;
using System.Activities;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace MDM.Workflow.Activities.Entity
{
    using MDM.Core;
    using MDMBO = MDM.BusinessObjects;
    using MDMBOW = MDM.BusinessObjects.Workflow;
    using MDM.EntityManager.Business;
    using MDM.Workflow.Activities.Core;
    using MDM.Workflow.Activities.Designer;

    using MDM.Interfaces;
    using MDM.Utility;
	using MDM.Workflow.Utility;


	///<summary>
	/// Returns an instance of the MDM Entity object using Entity Id
	///</summary>
	[Designer(typeof (ExecuteMatchingReviewDecisionDesigner))]
    [ToolboxBitmap(typeof (ExecuteMatchingReviewDecisionDesigner), "Images.ExecuteMatchingReviewDecision.bmp")]
    public class ExecuteMatchingReviewDecision : MDMNativeSystemActivityBase
	{
		private String _actions = "Done,Optional,";
		 
		/// <summary>
		/// Possible actions for this activity
		/// Ex: Approve,Reject
		/// </summary>
		[Browsable(false)]
		[DisplayName("Actions")]
		[Category("Actions")]
		public override String Actions
		{
			get { return _actions; }
		}

		protected Boolean _reviewCompleted = false;

		#region Methods
		/// <summary>
		/// Perform the Match Review and when suspects are found return the control the user for taking a decision.
		/// </summary>
		/// <returns>
		/// </returns>
		/// <param name="context">The execution context under which the activity executes.</param>
		protected override void OnExecute(NativeActivityContext context)
		{
            MDMBOW.WorkflowDataContext wfDataContext = MDMDataContext.Get(context);

            if (wfDataContext == null)
            {
                return;
            }

			//Get the entity participating in the workflow
			MDMBOW.WorkflowMDMObject mdmObject = null;

			if (wfDataContext.MDMObjectCollection.Count > 0)
			{
				mdmObject = wfDataContext.MDMObjectCollection.FirstOrDefault();
			}

			if (mdmObject == null)
			{
				return;
			}

			MDMBO.CallerContext callerContext = new MDMBO.CallerContext(MDMCenterApplication.WindowsWorkflow, MDMCenterModules.WindowsWorkflow);

			//Perform the Execute Match Review Process
			_reviewCompleted = BusinessRuleProcessor.ExecuteMatchingReviewDecision(mdmObject.MDMObjectId, callerContext);
        }



		protected override void AfterExecute(NativeActivityContext context)
		{
			//BookMark if the suspects are found on the Match Review process.
			if (!_reviewCompleted)
			{
				base.AfterExecute(context);
			}
		}

		#endregion
	}
}
