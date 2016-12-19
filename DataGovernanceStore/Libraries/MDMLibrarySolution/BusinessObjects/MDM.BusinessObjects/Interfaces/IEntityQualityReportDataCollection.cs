using System;
using System.Collections.Generic;

namespace MDM.Interfaces
{
    using MDM.BusinessObjects.DQM;

    /// <summary>
    /// Exposes methods or properties to set or get entity quality report data collection.
    /// </summary>
    public interface IEntityQualityReportDataCollection : ICollection<EntityQualityReportData>
    {
        /// <summary>
        /// Denotes total entities count for all pages of EntityQualityReportDataCollection records
        /// </summary>
        Int64 TotalEntitiesCount { get; set; }
    }
}
