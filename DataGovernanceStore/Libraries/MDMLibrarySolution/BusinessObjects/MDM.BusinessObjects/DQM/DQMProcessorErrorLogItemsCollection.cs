using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;

namespace MDM.BusinessObjects.DQM
{
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Specifies collection of DQMProcessorErrorLogItem
    /// </summary>
    [DataContract]
    public class DQMProcessorErrorLogItemsCollection : InterfaceContractCollection<IDQMProcessorErrorLogItem, DQMProcessorErrorLogItem>, IDQMProcessorErrorLogItemsCollection
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the DQMProcessorErrorLogItem collection
        /// </summary>
        public DQMProcessorErrorLogItemsCollection()
        { }

        /// <summary>
        /// Initialize DQMProcessorErrorLogItem collection from IList
        /// </summary>
        /// <param name="dqmJobQueueItemsList">Source items</param>
        public DQMProcessorErrorLogItemsCollection(IList<DQMProcessorErrorLogItem> dqmJobQueueItemsList)
        {
            this._items = new Collection<DQMProcessorErrorLogItem>(dqmJobQueueItemsList);
        }

        #endregion
    }
}