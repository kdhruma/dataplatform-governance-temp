using System;
using System.Collections.Generic;

namespace MDM.Interfaces
{
    using MDM.BusinessObjects;

    /// <summary>
    /// Exposes methods or properties to set or get the collection of merge results.
    /// </summary>
    public interface IMergeResultCollection : ICollection<MergeResult>, ICloneable
    {
        /// <summary>
        /// Denotes total results count for paging
        /// </summary>
        Int64? TotalItemsCount { get; set; }
    }
}