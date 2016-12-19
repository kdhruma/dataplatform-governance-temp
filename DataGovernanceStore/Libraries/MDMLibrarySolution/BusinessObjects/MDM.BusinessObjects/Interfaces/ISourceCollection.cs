using System;
using System.Collections.Generic;
using MDM.BusinessObjects;

namespace MDM.Interfaces
{
    /// <summary>
    /// Exposes methods or properties to set or get the collection of sources.
    /// </summary>
    public interface ISourceCollection : ICollection<Source>, ICloneable
    {
        /// <summary>
        /// Get specific source by Id
        /// </summary>
        /// <param name="sourceId">Id of the source</param>
        /// <returns>Source object</returns>
        Source Get(Int32 sourceId);
    }
}