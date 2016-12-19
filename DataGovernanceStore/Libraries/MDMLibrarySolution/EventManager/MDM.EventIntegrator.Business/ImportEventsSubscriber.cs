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
    /// Provides import jobs event manager
    /// </summary>
    public class ImportEventsSubscriber
    {
        /// <summary>
        /// This is static constructor. This binds events to the related handlers on application start..
        /// </summary>
        public static void Subscribe()
        {
            ImportEventManager.Instance.ImportStarted += OnImportStartedEvent;
            ImportEventManager.Instance.ImportCompleted += OnImportCompletedEvent;
            ImportEventManager.Instance.ImportAborted += OnImportAbortedEvent;
            ImportEventManager.Instance.ImportBatchStarted += OnImportBatchStartedEvent;
            ImportEventManager.Instance.ImportBatchProcessStarted += OnImportBatchProcessStartedEvent;
            ImportEventManager.Instance.ImportBatchProcessCompleted += OnImportBatchProcessCompletedEvent;
            ImportEventManager.Instance.ImportBatchCompleted += OnImportBatchCompletedEvent;
        }

        /// <summary>
        /// Sample to consume import started event..
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void OnImportStartedEvent(Object sender, ImportEventArgs e)
        {
            ImportsEventsHandler.ImportStarted(e.Job, e.ImportProfile);
        }

        /// <summary>
        /// Sample to consume import completed event..
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void OnImportCompletedEvent(Object sender, ImportEventArgs e)
        {
            ImportsEventsHandler.ImportCompleted(e.Job, e.ImportProfile, e.ExecutionStatus);
        }

        /// <summary>
        /// Sample to consume import aborted event..
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void OnImportAbortedEvent(Object sender, ImportEventArgs e)
        {
            ImportsEventsHandler.ImportAborted(e.Job, e.ImportProfile, e.ExecutionStatus);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void OnImportBatchStartedEvent(Object sender, ImportEventArgs e)
        {
            ImportsEventsHandler.ImportBatchStarted(e.EntityCollection, e.EntityOperationResultCollection, e.Job, e.ImportProfile);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void OnImportBatchProcessStartedEvent(Object sender, ImportEventArgs e)
        {
            ImportsEventsHandler.ImportBatchProcessStarted(e.EntityCollection, e.EntityOperationResultCollection, e.Job, e.ImportProfile);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void OnImportBatchProcessCompletedEvent(Object sender, ImportEventArgs e)
        {
            ImportsEventsHandler.ImportBatchProcessCompleted(e.EntityCollection, e.EntityOperationResultCollection, e.Job, e.ImportProfile);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void OnImportBatchCompletedEvent(Object sender, ImportEventArgs e)
        {
            ImportsEventsHandler.ImportBatchCompleted(e.EntityCollection, e.EntityOperationResultCollection, e.Job, e.ImportProfile);
        }
    }
}
