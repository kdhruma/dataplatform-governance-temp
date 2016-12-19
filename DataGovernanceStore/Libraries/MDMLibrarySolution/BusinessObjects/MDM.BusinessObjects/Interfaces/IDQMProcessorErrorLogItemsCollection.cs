using System.Collections.Generic;

namespace MDM.Interfaces
{
    using MDM.BusinessObjects.DQM;

    /// <summary>
    /// Exposes methods or properties to set or get DQM processor error log items collection.
    /// </summary>
    public interface IDQMProcessorErrorLogItemsCollection : ICollection<DQMProcessorErrorLogItem>
    {

    }
}