using System;
using System.Activities;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MDM.BusinessObjects;
using MDM.Core;
using MDM.Workflow.Activities.Designer;
using MDM.Workflow.Utility;

namespace MDM.Workflow.Activities.Core
{
    using MDMC = MDM.Core;

    [Designer(typeof(PromoteDesigner))]
    [ToolboxBitmap(typeof(PromoteDesigner), "Images.PromoteAsyncDesigner.bmp")]
    public class Promote : MDMNativeSystemActivityBase
    {
        #region Fields

        private String _actions = "Approve,Optional,;Reject,Mandatory,";

        private InArgument<String> _allowedRoles = "Workflow Marshal;";

        #endregion

        #region Properties
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

        /// <summary>
        /// List of comma separated role names, allowed to act on this activity
        /// </summary>
        ///  
        [Browsable(false)]
        [DisplayName("Allowed Roles")]
        [Category("Actors & Assignments")]
        public override InArgument<String> AllowedRoles
        {
            get { return _allowedRoles; }
        }

        #endregion

		#region Constructors

	    public Promote():base()
	    {
	    }
		#endregion
		#region overriden methods

		protected override void OnExecute(NativeActivityContext context)
        {
            Collection<Int64> entityIds = DataContextActivityContextValue.MDMObjectCollection.GetMDMObjectIds();

            if ((entityIds != null) && entityIds.Any())
            {
                Int64 entityId = entityIds[0];
                //TODO:  Do we need to passback the operationResult??
                // Add the hook to create the queue by passing the context (context contains the queue Id)
                OperationResult operationResult = BusinessRuleProcessor.CallAsyncProcess(entityId, GetCurrentWorkflowActionContext(), DataContextActivityContextValue.WorkflowVersionId, ActivityNameActivityContextValue, context.WorkflowInstanceId.ToString(), EntityActivityList.Promote);
            }
        }
        #endregion
    }
}
