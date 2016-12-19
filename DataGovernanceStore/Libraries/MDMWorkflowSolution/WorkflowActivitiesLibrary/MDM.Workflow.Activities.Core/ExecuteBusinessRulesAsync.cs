using System;
using System.Drawing;
using System.Activities;
using System.ComponentModel;
using System.Linq;
using MDM.Core;

namespace MDM.Workflow.Activities.Core
{
    using MDM.Workflow.Utility;
    using MDM.BusinessObjects;
    using MDM.BusinessObjects.Workflow;
    using MDM.Workflow.Activities.Designer;
    using MDM.Utility;

    /// <summary>
    /// Executes specified Business Rule
    /// </summary>
    [Designer(typeof (ExecuteBusinessRulesAsyncDesigner))]
    [ToolboxBitmap(typeof (ExecuteBusinessRulesAsyncDesigner), "Images.ExecuteBusinessRulesAsync.bmp")]
    public class ExecuteBusinessRulesAsync : MDMNativeSystemActivityBase
    {
        #region Members
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
        [Browsable(false)]
        [DisplayName("Allowed Roles")]
        [Category("Actors & Assignments")]
        public override InArgument<String> AllowedRoles
        {
            get { return _allowedRoles; }
        }
        #endregion


        #region overriden methods

        protected override void OnExecute(NativeActivityContext context)
        {  
            // Add the hook to create the queue by passing the context (context contains the queue Id)
            foreach (Int64 entityId in DataContextActivityContextValue.MDMObjectCollection.GetMDMObjectIds())
            {
                OperationResult operationResult = BusinessRuleProcessor.CallAsyncProcess(entityId, GetCurrentWorkflowActionContext(), DataContextActivityContextValue.WorkflowVersionId, ActivityNameActivityContextValue, context.WorkflowInstanceId.ToString(), EntityActivityList.EntityAsyncWorkflowActivityBusinessRules);
            }
        }
        #endregion
    }
}
