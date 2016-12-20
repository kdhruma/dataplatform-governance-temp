using System;
using System.Collections.Generic;

namespace MDM.Interfaces
{
    using MDM.Core;

    /// <summary>
    /// Exposes methods or properties to set or get job report related information.
    /// </summary>
    public interface IJobReportSettings
    {
        /// <summary>
        /// Denotes collection of allowed job types
        /// </summary>
        ICollection<DQMJobType> JobTypes { get; set; }

        /// <summary>
        /// Denotes CountFrom parameter for paged result 
        /// </summary>
        Int64? CountFrom { get; set; }

        /// <summary>
        /// Denotes CountTo parameter for paged result
        /// </summary>
        Int64? CountTo { get; set; }

        /// <summary>
        /// Allows to request total items count including all pages
        /// </summary>
        Boolean RequestTotalItemsCount { get; set; }
    }
}