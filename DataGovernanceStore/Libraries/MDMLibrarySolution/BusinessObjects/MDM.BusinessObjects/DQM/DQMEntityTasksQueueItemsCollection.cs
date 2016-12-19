using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;

namespace MDM.BusinessObjects.DQM
{
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Specifies collection of DQMEntityTasksQueueItem
    /// </summary>
    [DataContract]
    public class DQMEntityTasksQueueItemsCollection : InterfaceContractCollection<IDQMEntityTasksQueueItem, DQMEntityTasksQueueItem>, IDQMEntityTasksQueueItemsCollection
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the DQMEntityTasksQueueItems Collection
        /// </summary>
        public DQMEntityTasksQueueItemsCollection()
        { }

        /// <summary>
        /// Initialize DQMEntityTasksQueueItems collection from IList
        /// </summary>
        /// <param name="DQMEntityTasksQueueItemsList">Source items</param>
        public DQMEntityTasksQueueItemsCollection(IList<DQMEntityTasksQueueItem> DQMEntityTasksQueueItemsList)
        {
            this._items = new Collection<DQMEntityTasksQueueItem>(DQMEntityTasksQueueItemsList);
        }

        #endregion
    }
}
