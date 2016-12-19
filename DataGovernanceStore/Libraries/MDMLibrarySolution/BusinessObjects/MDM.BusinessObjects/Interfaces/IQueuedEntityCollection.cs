using System;
using System.Collections.Generic;

namespace MDM.Interfaces
{
    using MDM.Core;
    using MDM.BusinessObjects;
    using MDM.BusinessObjects.Denorm;

    /// <summary>
    /// Exposes methods or properties to set or get queued entity collection.
    /// </summary>
    public interface IQueuedEntityCollection : IEnumerable<QueuedEntity>
    {
        /// <summary>
        /// XML Presentaion of an QueuedEntityCollection
        /// </summary>
        /// <returns>XML Presentaion of an QueuedEntityCollection</returns>
        String ToXml();
    }
}
