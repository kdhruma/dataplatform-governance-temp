using System;
using System.Collections.Generic;

namespace MDM.Interfaces
{
    using MDM.BusinessObjects.DQM;

    /// <summary>
    /// Exposes methods or properties to set or get DQM job queue items collection.
    /// </summary>
    public interface IDQMJobQueueItemsCollection : ICollection<DQMJobQueueItem>
    {
        /// <summary>
        /// Property denoting total items count for all pages of job report
        /// </summary>
        Int64? TotalItemsCount { get; set; }
    }
}