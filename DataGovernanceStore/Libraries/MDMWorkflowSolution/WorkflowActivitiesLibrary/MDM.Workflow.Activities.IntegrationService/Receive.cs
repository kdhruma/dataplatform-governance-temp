using System;
using System.Activities;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;

namespace MDM.Workflow.Activities.IntegrationService
{
    using BusinessObjects.Integration;
    using MDM.Core;
    using MDM.IntegrationManager.Business;
    using MDM.Interfaces;
    using MDM.Utility;
    using MDM.Workflow.Activities.Core;
    using MDM.Workflow.Activities.Designer;
    using MDMBO = BusinessObjects;

    /// <summary>
    /// Activity used for receive of integration service
    /// </summary>
    [Designer(typeof(ReceiveDesigner))]
    [ToolboxBitmap(typeof(ReceiveDesigner), "Images.receive.bmp")]
    public class Receive : MDMCodeActivitiyBase<IOperationResult>
    {
        #region Fields
        #endregion

        #region Constructors
        #endregion

        #region Properties

        [DisplayName("Connector Short Name")]
        [Category("Input Arguments")]
        public InArgument<String> ConnectorShortName { get; set; }

        [DisplayName("Integration Message")]
        [Category("Input Arguments")]
        public InArgument<IIntegrationMessage> IntegrationMessage { get; set; }

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
            IIntegrationMessage message = IntegrationMessage.Get(context);
            String errorMessage = String.Empty;

            IOperationResult iOperationResult = ValidateParameters(message, connectorProfileShortName);

            if (iOperationResult.OperationResultStatus == OperationResultStatusEnum.Failed
               || iOperationResult.OperationResultStatus == OperationResultStatusEnum.CompletedWithErrors)
            {
                return iOperationResult;
            }

            MDMBO.CallerContext callerContext = new MDMBO.CallerContext(MDMCenterApplication.PIM, MDMCenterModules.Integration);
            ConnectorProfileBL ConnectorProfileManager = new ConnectorProfileBL();
            CoreConnector coreConnector = new CoreConnector();

            ConnectorProfile connectorProfile = ConnectorProfileManager.GetByName(connectorProfileShortName, callerContext);

            if (connectorProfile == null)
            {
                errorMessage = String.Format("The Connector with ConnectorShortName = '{0}' is not found.", connectorProfileShortName);

                iOperationResult.AddOperationResult(String.Empty, errorMessage, OperationResultType.Error);
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, errorMessage, MDMTraceSource.AdvancedWorkflow);

                return iOperationResult;
            }

            iOperationResult = coreConnector.Receive(message, connectorProfile, callerContext);

            OperationResult.Set(context, iOperationResult);

            return iOperationResult;
        }

        #endregion

        #region Private Methods

        private IOperationResult ValidateParameters(IIntegrationMessage message, String connectorProfileShortName)
        {
            IOperationResult iOperationResult = MDMObjectFactory.GetIOperationResult();
            String errorMessage = String.Empty;

            if (message == null)
            {
                errorMessage = "Integration message is not available.";

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