using System;
using System.Collections.Generic;

namespace MDM.Interfaces
{
    using MDM.BusinessObjects.DQM;

    /// <summary>
    /// Exposes methods or properties to set or get the collection of normalization report items. 
    /// </summary>
    public interface INormalizationReportItemsCollection : ICollection<NormalizationReportItem>
    {
        /// <summary>
        /// Property denoting total items count for all pages of normalization report
        /// </summary>
        Int64? TotalItemsCount { get; set; }
    }
}