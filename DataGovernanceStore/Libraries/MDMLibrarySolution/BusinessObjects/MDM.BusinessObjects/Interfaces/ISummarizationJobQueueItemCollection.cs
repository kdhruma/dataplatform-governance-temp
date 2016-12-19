using System.Collections.Generic;

namespace MDM.Interfaces
{
    using MDM.BusinessObjects.DQM;

    /// <summary>
    /// Exposes methods or properties to set or get the collection of DQM summarization job queue items.
    /// </summary>
    public interface ISummarizationJobQueueItemCollection : ICollection<SummarizationJobQueueItem>
    {
    }
}