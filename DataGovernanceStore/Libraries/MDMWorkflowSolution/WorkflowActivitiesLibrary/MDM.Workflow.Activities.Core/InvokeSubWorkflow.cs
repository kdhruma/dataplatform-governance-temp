using System;
using System.Drawing;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Activities;
using System.Activities.Statements;
using System.ComponentModel;

using MDM.BusinessObjects;
using MDMBOW=MDM.BusinessObjects.Workflow;
using MDM.Workflow.Activities.Designer;
using MDM.WCFServices;

namespace MDM.Workflow.Activities.Core
{
    /// <summary>
    /// Invokes the sub workflow defined in the Main workflow
    /// </summary>
    [Designer(typeof(InvokeSubWorkflowDesigner))]
    [ToolboxBitmap(typeof(InvokeSubWorkflowDesigner), "Images.InvokeSubWorkflow.bmp")]
    public class InvokeSubWorkflow : MDMCodeActivitiyBase<Boolean>
    {
        #region Properties

        /// <summary>
        /// Name of the workflow to be invoked
        /// </summary>
        [DisplayName("Workflow Name")]
        [Category("Input Arguments")]
        [RequiredArgument]
        public InArgument<String> WorkflowName { get; set; }

        /// <summary>
        /// Action Context in workflow client, provides action details for Activity
        /// </summary>
        [Browsable(false)]
        public new OutArgument<MDMBOW.WorkflowActionContext> MDMActionContext { get; set; }

        #endregion

        #region Methods

        protected override Boolean Execute(CodeActivityContext context)
        {
            OperationResult operationResult = new OperationResult();
            MDMBOW.WorkflowDataContext workflowDataContext = new MDMBOW.WorkflowDataContext();
            MDMBOW.WorkflowDataContext participatingDataContext = context.GetValue(this.MDMDataContext);
            String workflowName = context.GetValue(this.WorkflowName);

            //Assign values
            workflowDataContext.MDMObjectCollection = participatingDataContext.MDMObjectCollection;
            workflowDataContext.WorkflowName = workflowName;

            //Start workflow
            WorkflowService workflowService = new WorkflowService();
            workflowService.StartWorkflow(workflowDataContext, "WorkflowRuntime", participatingDataContext.WorkflowVersionId, "system", ref operationResult, new CallerContext(workflowDataContext.Application, workflowDataContext.Module));

            if (operationResult.HasError)
                throw new Exception(operationResult.Errors[0].ErrorMessage);

            return true;
        }

        #endregion
    }
}
