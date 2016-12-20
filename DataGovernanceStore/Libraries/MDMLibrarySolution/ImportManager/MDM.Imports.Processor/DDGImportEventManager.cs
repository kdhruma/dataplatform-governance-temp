using System;

namespace MDM.Imports.Processor
{
    using MDM.Core;
    
    /// <summary>
    /// Specifies DDG import jobs event manager class
    /// </summary>
    public sealed class DDGImportEventManager
    {
        public DDGImportEventManager()
        {

        }

        public static readonly DDGImportEventManager Instance = new DDGImportEventManager();

        public EventHandler<DDGImportEventArgs> ImportStarted;
        public EventHandler<DDGImportEventArgs> ImportCompleted;
        public EventHandler<DDGImportEventArgs> ImportAborted;

        public void OnDDGImportStarted(DDGImportEventArgs e)
        {
            ImportStarted.SafeInvoke(this, e);
        }

        public void OnDDGImportCompleted(DDGImportEventArgs e)
        {
            ImportCompleted.SafeInvoke(this, e);
        }

        public void OnDDGImportAborted(DDGImportEventArgs e)
        {
            ImportAborted.SafeInvoke(this, e);
        }
    }
}
