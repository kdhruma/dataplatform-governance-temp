using System;
using System.Linq;

namespace MDM.EventIntegrator.Business
{
    using MDM.Workflow.Utility;

    /// <summary>
    /// Specifies subscriber for Workflow Events
    /// </summary>
    internal class EntityWorkflowEventSubscriber
    {
        #region Event Handlers

        private static void WorkItemsDataLoadedEvent(Object sender, WorkItemsDataLoadedEventArgs e)
        {
            //TODO:: Process business rules..
        }

        #endregion
    }
}