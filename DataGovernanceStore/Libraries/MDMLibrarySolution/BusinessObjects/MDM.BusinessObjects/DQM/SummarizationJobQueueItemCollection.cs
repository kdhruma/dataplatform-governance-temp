using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;

namespace MDM.BusinessObjects.DQM
{
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Specifies collection of SummarizationJobQueueItem
    /// </summary>
    [DataContract]
    public class SummarizationJobQueueItemCollection : InterfaceContractCollection<ISummarizationJobQueueItem, SummarizationJobQueueItem>, ISummarizationJobQueueItemCollection
    {
        #region Fields

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the SummarizationJobQueueItem Collection
        /// </summary>
        public SummarizationJobQueueItemCollection()
        { }

        /// <summary>
        /// Initialize SummarizationJobQueueItem collection from IList
        /// </summary>
        /// <param name="summarizationJobQueueItemList">Source items</param>
        public SummarizationJobQueueItemCollection(IList<SummarizationJobQueueItem> summarizationJobQueueItemList)
        {
            this._items = new Collection<SummarizationJobQueueItem>(summarizationJobQueueItemList);
        }

        #endregion

        #region Properties

        #endregion
    }
}