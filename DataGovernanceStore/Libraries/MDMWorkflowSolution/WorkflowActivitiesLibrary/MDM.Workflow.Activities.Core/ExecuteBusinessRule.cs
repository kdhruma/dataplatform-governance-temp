using System;
using System.Drawing;
using System.Activities;
using System.ComponentModel;



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
    [Designer(typeof(ExecuteBusinessRuleDesigner))]
    [ToolboxBitmap(typeof(ExecuteBusinessRuleDesigner), "Images.ExecuteBusinessRule.bmp")]
    public class ExecuteBusinessRule : MDMCodeActivitiyBase<OperationResult>
    {
        #region Members

        /// <summary>
        /// Assembly Name where Business Rule will be found
        /// </summary>
        [DisplayName("Assembly Name")]
        [Category("Input Arguments")]
        [RequiredArgument]
        public InArgument<String> AssemblyName { get; set; }

        /// <summary>
        /// Name of the Type within the Assembly that needs to be invoked
        /// </summary>
        [DisplayName("Type Name")]
        [Category("Input Arguments")]
        [RequiredArgument]
        public InArgument<String> TypeName { get; set; }

        /// <summary>
        /// Name of the Type within the Assembly that needs to be invoked
        /// </summary>
        [DisplayName("Return Data")]
        [Category("Output Arguments")]
        public OutArgument<Object> DataObject { get; set; }

        /// <summary>
        /// Name of the Type within the Assembly that needs to be invoked
        /// </summary>
        [DisplayName("IsExecutionSuccessful")]
        [Category("Output Arguments")]
        public OutArgument<Boolean> IsExecutionSuccessful { get; set; }

        /// <summary>
        /// Action Context in workflow client, provides action details for Activity
        /// </summary>
        [Browsable(false)]
        public new OutArgument<WorkflowActionContext> MDMActionContext { get; set; }

        #endregion

        protected override OperationResult Execute(CodeActivityContext context)
        {
            String currentActivityName = context.GetValue(Name);
            OperationResult operationResult = new OperationResult();
            String assemblyName = context.GetValue(this.AssemblyName);
            String typeName = context.GetValue(this.TypeName);
            WorkflowDataContext participatingDataContext = context.GetValue(this.MDMDataContext);
            WorkflowActionContext previousActivityActionContext = context.GetValue(this.MDMActionContext);

            String baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            String configDirectoryPath = AppConfigurationHelper.GetAppConfig<String>("MDMCenter.BR.ConfigDirectory");
            String assemblyPath = String.Concat(baseDirectory, configDirectoryPath, "\\bin\\");

            try
            {
                //Create workflow rule context
                WorkflowBusinessRuleContext workflowContext = new WorkflowBusinessRuleContext(participatingDataContext, currentActivityName, this.DisplayName, previousActivityActionContext);

                //Process rule
                BusinessRuleProcessor businessRuleProcessor = new BusinessRuleProcessor();
                businessRuleProcessor.ProcessRules(workflowContext, assemblyPath, assemblyName, typeName, operationResult);
            }
            catch (Exception ex)
            {
                Error error = new Error();
                error.ErrorMessage = ex.Message;
                operationResult.Errors.Add(error);
            }

            //Check for errors
            if (!operationResult.HasError)
            {
                if(operationResult.ReturnValues != null && operationResult.ReturnValues.Count > 0)
                    this.DataObject.Set(context, operationResult.ReturnValues[0]);

                this.IsExecutionSuccessful.Set(context, true);
            }
            else
                this.IsExecutionSuccessful.Set(context, false);

            return operationResult;
        }
    }
}
