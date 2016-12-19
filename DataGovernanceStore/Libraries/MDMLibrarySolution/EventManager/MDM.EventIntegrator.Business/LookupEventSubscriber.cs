using System;

namespace MDM.EventIntegrator.Business
{
    using RS.MDM.Events;
    using MDM.BusinessObjects;
    using MDM.LookupManager.Business;

    /// <summary>
    /// Specifies subscriber for Lookup Events
    /// </summary>
    internal class LookupEventSubscriber
    {
        #region Methods

        internal void Subscribe()
        {
            LookupEventManager.Instance.LookupDataLoad += LookupDataLoadEvent;
			LookupEventManager.Instance.LookupDataProcessing += LookupDataProcessingEvent;
			LookupEventManager.Instance.LookupDataProcessed += LookupDataProcessedEvent;
        }

        #endregion

        #region Event Handlers

        private static void LookupDataLoadEvent(Object sender, LookupDataLoadEventArgs e)
        {
            //TODO:: Process business rules..
        }

		private static void LookupDataProcessingEvent(Object sender, LookupDataProcessEventArgs e)
		{
			//TODO:: Process business rules..
		}

		private static void LookupDataProcessedEvent(Object sender, LookupDataProcessEventArgs e)
		{
			//TODO:: Process business rules..
		}

        #endregion
    }
}
