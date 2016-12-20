using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;

namespace MDM.BusinessObjects.DQM
{
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Specifies collection of DQMJobQueueItemStatus
    /// </summary>
    [DataContract]
    public class DQMJobQueueItemsStatusCollection : InterfaceContractCollection<IDQMJobQueueItemStatus, DQMJobQueueItemStatus>, IDQMJobQueueItemsStatusCollection
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the DQMJobQueueItemStatus Collection
        /// </summary>
        public DQMJobQueueItemsStatusCollection()
        { }

        /// <summary>
        /// Initialize DQMJobQueueItemStatus collection from IList
        /// </summary>
        /// <param name="dqmJobQueueItemStatusList">Source items</param>
        public DQMJobQueueItemsStatusCollection(IList<DQMJobQueueItemStatus> dqmJobQueueItemStatusList)
        {
            this._items = new Collection<DQMJobQueueItemStatus>(dqmJobQueueItemStatusList);
        }

        #endregion
    }
}