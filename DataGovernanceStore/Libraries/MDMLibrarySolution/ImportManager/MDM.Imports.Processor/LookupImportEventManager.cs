using System;
using MDM.Core;

namespace MDM.Imports.Processor
{
    /// <summary>
    /// Specifies lookup import jobs event manager class
    /// </summary>
    public sealed class LookupImportEventManager
    {
        private LookupImportEventManager()
        {
        }

        public static readonly LookupImportEventManager Instance = new LookupImportEventManager();

        public EventHandler<LookupImportEventArgs> ImportStarted;
        public EventHandler<LookupImportEventArgs> ImportCompleted;
        public EventHandler<LookupImportEventArgs> ImportAborted;

        public void OnImportStarted(LookupImportEventArgs e)
        {
            ImportStarted.SafeInvoke(this, e);
        }

        public void OnImportCompleted(LookupImportEventArgs e)
        {
            ImportCompleted.SafeInvoke(this, e);
        }

        public void OnImportAborted(LookupImportEventArgs e)
        {
            ImportAborted.SafeInvoke(this, e);
        }
    }
}
