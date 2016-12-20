using System;
using MDM.Core;

namespace MDM.Imports.Processor
{
    /// <summary>
    /// Specifies dataModel import jobs event manager class
    /// </summary>
    public sealed class DataModelImportEventManager
    {
        private DataModelImportEventManager()
        {
        }

        public static readonly DataModelImportEventManager Instance = new DataModelImportEventManager();

        public EventHandler<DataModelImportEventArgs> ImportStarted;
        public EventHandler<DataModelImportEventArgs> ImportCompleted;
        public EventHandler<DataModelImportEventArgs> ImportAborted;

        public void OnImportStarted(DataModelImportEventArgs e)
        {
            ImportStarted.SafeInvoke(this, e);
        }

        public void OnImportCompleted(DataModelImportEventArgs e)
        {
            ImportCompleted.SafeInvoke(this, e);
        }

        public void OnImportAborted(DataModelImportEventArgs e)
        {
            ImportAborted.SafeInvoke(this, e);
        }
    }
}
