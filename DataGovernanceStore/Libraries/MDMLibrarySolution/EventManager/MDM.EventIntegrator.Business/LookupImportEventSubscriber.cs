using System;
using System.IO;

namespace MDM.EventIntegrator.Business
{
    using MDM.Core;
    using MDM.BusinessObjects;
    using MDM.BusinessObjects.Imports;
    using MDM.BusinessObjects.Jobs;

    using MDM.ImportsEventsHandler;
    using MDM.Imports.Processor;

    /// <summary>
    /// Specifies subscriber for Lookup Import Events
    /// </summary>
    public class LookupImportEventSubscriber
    {
        /// <summary>
        /// This is static constructor. This binds events to the related handlers on application start..
        /// </summary>
        public static void Subscribe()
        {
            LookupImportEventManager.Instance.ImportStarted += OnImportStartedEvent;
            LookupImportEventManager.Instance.ImportCompleted += OnImportCompletedEvent;
            LookupImportEventManager.Instance.ImportAborted += OnImportAbortedEvent;
        }

        /// <summary>
        /// Sample to consume import started event..
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void OnImportStartedEvent(Object sender, LookupImportEventArgs e)
        {
            LookupImportEventHandler.ImportStarted(e.Job, e.LookupImportProfile);
        }

        /// <summary>
        /// Sample to consume import completed event..
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void OnImportCompletedEvent(Object sender, LookupImportEventArgs e)
        {
            LookupImportEventHandler.ImportCompleted(e.Job, e.LookupImportProfile, e.ExecutionStatus);
        }

        /// <summary>
        /// Sample to consume import aborted event..
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void OnImportAbortedEvent(Object sender, LookupImportEventArgs e)
        {
            LookupImportEventHandler.ImportCompleted(e.Job, e.LookupImportProfile, e.ExecutionStatus);
        }
    }
}
