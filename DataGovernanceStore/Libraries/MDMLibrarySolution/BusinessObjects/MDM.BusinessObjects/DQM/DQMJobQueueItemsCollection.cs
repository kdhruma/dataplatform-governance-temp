using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;

namespace MDM.BusinessObjects.DQM
{
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Specifies collection of DQMJobQueueItem
    /// </summary>
    [DataContract]
    public class DQMJobQueueItemsCollection : InterfaceContractCollection<IDQMJobQueueItem, DQMJobQueueItem>, IDQMJobQueueItemsCollection
    {
        #region Fields

        /// <summary>
        /// Field denoting total items count for all pages of job report
        /// </summary>
        private Int64? _totalItemsCount = null;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the DQMJobQueueItems Collection
        /// </summary>
        public DQMJobQueueItemsCollection()
        { }

        /// <summary>
        /// Initialize DQMJobQueueItems collection from IList
        /// </summary>
        /// <param name="DQMJobQueueItemsList">Source items</param>
        public DQMJobQueueItemsCollection(IList<DQMJobQueueItem> DQMJobQueueItemsList)
        {
            this._items = new Collection<DQMJobQueueItem>(DQMJobQueueItemsList);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Property denoting total items count for all pages of job report
        /// </summary>
        [DataMember]
        public Int64? TotalItemsCount
        {
            get { return _totalItemsCount; }
            set { _totalItemsCount = value; }
        }

        #endregion
    }
}
