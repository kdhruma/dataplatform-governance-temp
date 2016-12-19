using System;
using System.Activities;
using System.ComponentModel;
using System.Drawing;
using System.Diagnostics;

namespace MDM.Workflow.Activities.IntegrationService
{
    using BusinessObjects.Integration;
    using MDM.Core;
    using MDM.IntegrationManager.Business;
    using MDM.Interfaces;
    using MDM.Workflow.Activities.Core;
    using MDM.Workflow.Activities.Designer;
    using MDMBO = BusinessObjects;
    using MDM.Utility;

    /// <summary>
    /// Activity used for batch send of integration service
    /// </summary>
    [Designer(typeof(BatchSendDesigner))]
    [ToolboxBitmap(typeof(BatchSendDesigner), "Images.batchSend.bmp")]
    public class BatchSend : MDMCodeActivitiyBase<IOperationResult>
    {
        #region Fields
        #endregion

        #region Constructors
        #endregion

        #region Properties

        [DisplayName("Connector Short Name")]
        [Category("Input Arguments")]
        public InArgument<String> ConnectorShortName { get; set; }

        [DisplayName("Integration Data")]
        [Category("Input Arguments")]
        public InArgument<IIntegrationData> IntegrationData { get; set; }

        [DisplayName("Operation Result")]
        [Category("Output Arguments")]
        public OutArgument<IOperationResult> OperationResult { get; set; }

        #endregion

        #region Methods

        #region Public Methods

        /// <summary>
        /// When implemented in a derived class, performs the execution of the activity.
        /// </summary>
        /// <returns>
        /// The result of the activity’s execution.
        /// </returns>
        /// <param name="context">The execution context under which the activity executes.</param>
        protected override IOperationResult Execute(CodeActivityContext context)
        {
            String connectorProfileShortName = ConnectorShortName.Get(context);
            IIntegrationData iIntegrationData = IntegrationData.Get(context);
            String errorMessage = String.Empty;

            IOperationResult iOperationResult = ValidateParameters(iIntegrationData, connectorProfileShortName);

            if (iOperationResult.OperationResultStatus == OperationResultStatusEnum.Failed
               || iOperationResult.OperationResultStatus == OperationResultStatusEnum.CompletedWithErrors)
            {
                return iOperationResult;
            }

            ConnectorProfileBL ConnectorProfileManager = new ConnectorProfileBL();
            CoreConnector coreConnector = new CoreConnector();
            MDMBO.CallerContext callerContext = new MDMBO.CallerContext(MDMCenterApplication.PIM, MDMCenterModules.Integration);

            ConnectorProfile connectorProfile = ConnectorProfileManager.GetByName(connectorProfileShortName, callerContext);

            if (connectorProfile == null)
            {
                errorMessage = String.Format("The Connector with ConnectorShortName = '{0}' is not found.", connectorProfileShortName);

                MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, errorMessage, MDMTraceSource.AdvancedWorkflow);

                iOperationResult.AddOperationResult(String.Empty, errorMessage, OperationResultType.Error);

                return iOperationResult;
            }

            IOperationResult result = coreConnector.BatchSend(iIntegrationData, connectorProfile, callerContext);

            OperationResult.Set(context, result);

            return result;
        }

        #endregion

        #region Private Methods

        private IOperationResult ValidateParameters(IIntegrationData iIntegrationData, String connectorProfileShortName)
        {
            IOperationResult iOperationResult = MDMObjectFactory.GetIOperationResult();
            String errorMessage = String.Empty;

            if (iIntegrationData == null)
            {
                errorMessage = "Integration data is not available.";

                iOperationResult.AddOperationResult(String.Empty, errorMessage, OperationResultType.Error);

                MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, errorMessage, MDMTraceSource.AdvancedWorkflow);
            }

            if (String.IsNullOrWhiteSpace(connectorProfileShortName))
            {
                errorMessage = "Connector profile name is not available.";

                iOperationResult.AddOperationResult(String.Empty, errorMessage, OperationResultType.Error);

                MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, errorMessage, MDMTraceSource.AdvancedWorkflow);
            }

            return iOperationResult;
        }

        #endregion

        #endregion
    }
}