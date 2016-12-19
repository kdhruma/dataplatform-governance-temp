using System;
using System.Activities;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
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
    /// Activity used for transform of integration service
    /// </summary>
    [Designer(typeof(TransformDesigner))]
    [ToolboxBitmap(typeof(TransformDesigner), "Images.transform.bmp")]
    public class Transform : MDMCodeActivitiyBase<IIntegrationData>
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
        [Category("Output Arguments")]
        public OutArgument<IIntegrationData> IntegrationData { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// When implemented in a derived class, performs the execution of the activity.
        /// </summary>
        /// <returns>
        /// The result of the activity’s execution.
        /// </returns>
        /// <param name="context">The execution context under which the activity executes.</param>
        protected override IIntegrationData Execute(CodeActivityContext context)
        {
            var dataContext = MDMDataContext.Get(context);
            var connectorProfileShortName = ConnectorShortName.Get(context);

            if (dataContext == null || !dataContext.MDMObjectCollection.Any() || String.IsNullOrEmpty(connectorProfileShortName))
            {
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, "MDM data context is not available.", MDMTraceSource.AdvancedWorkflow);
                return null;
            }

            var mdmObject = dataContext.MDMObjectCollection.First();

            if (mdmObject == null)
            {
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, "Workflow MDM Object is not available.", MDMTraceSource.AdvancedWorkflow);
                return null;
            }

            IntegrationMessageBL integrationMessageManager = new IntegrationMessageBL();
            ConnectorProfileBL connectorProfileManager = new ConnectorProfileBL();
            CoreConnector coreConnector = new CoreConnector();

            MDMBO.CallerContext callerContext = new MDMBO.CallerContext(MDMCenterApplication.PIM, MDMCenterModules.Integration);

            IntegrationMessage message = integrationMessageManager.GetById(mdmObject.MDMObjectId, callerContext);

            if (message == null)
            {
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, "Integration message is not available.", MDMTraceSource.AdvancedWorkflow);
                return null;
            }

            ConnectorProfile connectorProfile = connectorProfileManager.GetByName(connectorProfileShortName, callerContext);

            if (connectorProfile == null)
            {
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, String.Format("The Connector with ConnectorShortName = '{0}' is not found.", connectorProfileShortName), MDMTraceSource.AdvancedWorkflow);
                return null;
            }

            IIntegrationData transformedObject = coreConnector.Transform(message, connectorProfile, callerContext);

            if (transformedObject == null)
            {
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, "Transformed object is not available.", MDMTraceSource.AdvancedWorkflow);
                return null;
            }

            IntegrationData.Set(context, transformedObject);

            return transformedObject;
        }

        #endregion
    }
}