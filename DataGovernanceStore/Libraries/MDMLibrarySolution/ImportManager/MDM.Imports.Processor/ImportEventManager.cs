using System;
using MDM.Core;

namespace MDM.Imports.Processor
{
    /// <summary>
    /// Provides import jobs event manager
    /// </summary>
    public sealed class ImportEventManager
    {
        private ImportEventManager()
        {
        }

        public static readonly ImportEventManager Instance = new ImportEventManager();

        public EventHandler<ImportEventArgs> ImportStarted;
        public EventHandler<ImportEventArgs> ImportCompleted;
        public EventHandler<ImportEventArgs> ImportAborted;
        public EventHandler<ImportEventArgs> ImportBatchStarted;
        public EventHandler<ImportEventArgs> ImportBatchProcessStarted;
        public EventHandler<ImportEventArgs> ImportBatchProcessCompleted;
        public EventHandler<ImportEventArgs> ImportBatchCompleted;

        public void OnImportStarted(ImportEventArgs e)
        {
            ImportStarted.SafeInvoke(this, e);
        }

        public void OnImportCompleted(ImportEventArgs e)
        {
            ImportCompleted.SafeInvoke(this, e);
        }

        public void OnImportAborted(ImportEventArgs e)
        {
            ImportAborted.SafeInvoke(this, e);
        }

        public void OnImportBatchStarted(ImportEventArgs e)
        {
            ImportBatchStarted.SafeInvoke(this, e);
        }

        public void OnImportBatchProcessStarted(ImportEventArgs e)
        {
            ImportBatchProcessStarted.SafeInvoke(this, e);
        }

        public void OnImportBatchProcessCompleted(ImportEventArgs e)
        {
            ImportBatchProcessCompleted.SafeInvoke(this, e);
        }

        public void OnImportBatchCompleted(ImportEventArgs e)
        {
            ImportBatchCompleted.SafeInvoke(this, e);
        }
    }
}
