using System;
using System.Activities;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;

namespace MDM.Workflow.Activities.IntegrationService
{
    using MDM.Core;
    using MDM.IntegrationManager.Business;
    using MDM.Interfaces;
    using MDM.Utility;
    using MDM.Workflow.Activities.Core;
    using MDM.Workflow.Activities.Designer;
    using MDMBO = MDM.BusinessObjects;
    using MDMBOW = MDM.BusinessObjects.Workflow;

    /// <summary>
    /// Activity used for send of integration service
    /// </summary>
    [Designer(typeof(SendDesigner))]
    [ToolboxBitmap(typeof(SendDesigner), "Images.send.bmp")]
    public class Send : MDMCodeActivitiyBase<IOperationResult>
    {
        #region Fields
        #endregion

        #region Constructors
        #endregion

        #region Properties

        /// <summary>
        /// Input Argument - IIntegrationData to be sent out
        /// </summary>
        [DisplayName("Integration Data")]
        [Category("Input Arguments")]
        public InArgument<IIntegrationData> IntegrationData { get; set; }

        /// <summary>
        /// Input Argument - Name of connector for which message is to be sent.
        /// </summary>
        [DisplayName("Connector Short Name")]
        [Category("Input Arguments")]
        public InArgument<String> ConnectorShortName { get; set; }

        /// <summary>
        /// Output Argument - Result of operation
        /// </summary>
        [DisplayName("Operation Result")]
        [Category("Output Arguments")]
        public OutArgument<IOperationResult> OperationResult { get; set; }

        #endregion Properties

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        protected override IOperationResult Execute(CodeActivityContext context)
        {
            IOperationResult iOperationResult = MDMObjectFactory.GetIOperationResult();
            String errorMessage = String.Empty;

            Object dataContext = MDMDataContext.Get(context);

            if (!(dataContext is MDMBOW.WorkflowDataContext))
            {
                errorMessage = "MDM data context is not available.";

                iOperationResult.AddOperationResult(String.Empty, errorMessage, OperationResultType.Error);
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, errorMessage , MDMTraceSource.AdvancedWorkflow);

                return iOperationResult;
            }

            //Get the message participating in the workflow
            MDMBOW.WorkflowDataContext wfDataContext = dataContext as MDMBOW.WorkflowDataContext;
            MDMBOW.WorkflowMDMObject mdmObject = null;

            if (wfDataContext.MDMObjectCollection.Count > 0)
                mdmObject = wfDataContext.MDMObjectCollection.First();

            if (mdmObject == null)
            {
                errorMessage = "Workflow MDM Object is not available.";

                iOperationResult.AddOperationResult(String.Empty, errorMessage, OperationResultType.Error);
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, errorMessage , MDMTraceSource.AdvancedWorkflow);

                return iOperationResult;
            }

            //Read input parameters
            IIntegrationData iIntegrationData = IntegrationData.Get(context);
            String connectorProfileShortName = this.ConnectorShortName.Get(context);

            MDMBO.CallerContext callerContext = new MDMBO.CallerContext(MDM.Core.MDMCenterApplication.PIM, MDM.Core.MDMCenterModules.Integration);
            ConnectorProfileBL connectorProfileManager = new ConnectorProfileBL();
            CoreConnector coreConnector = new CoreConnector();

            IConnectorProfile iConnectorProfile = connectorProfileManager.GetByName(connectorProfileShortName, callerContext);

            if (iConnectorProfile == null)
            {
                errorMessage = String.Format("The Connector with ConnectorShortName = '{0}' is not found.", connectorProfileShortName);

                iOperationResult.AddOperationResult(String.Empty, errorMessage, OperationResultType.Error);
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, errorMessage, MDMTraceSource.AdvancedWorkflow);

                return iOperationResult;
            }

            iOperationResult = coreConnector.Send(iIntegrationData, iConnectorProfile, callerContext);

            return iOperationResult;
        }

        #endregion Methods
    }
}