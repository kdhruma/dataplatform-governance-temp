using System.Collections.Generic;

namespace MDM.Interfaces
{
    using MDM.BusinessObjects.DQM;

    /// <summary>
    /// Exposes methods or properties to set or get DQM job queue items status collection.
    /// </summary>
    public interface IDQMJobQueueItemsStatusCollection : ICollection<DQMJobQueueItemStatus>
    {

    }
}
