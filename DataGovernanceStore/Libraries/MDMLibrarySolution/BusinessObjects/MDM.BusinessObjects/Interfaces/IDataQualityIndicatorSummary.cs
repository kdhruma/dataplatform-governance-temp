using System;

namespace MDM.Interfaces
{
    using MDM.BusinessObjects.DQM;

    /// <summary>
    /// Exposes methods or properties to set or get quality indicator (DataQualityIndicator) summary information.
    /// </summary>
    public interface IDataQualityIndicatorSummary
    {
        /// <summary>
        /// Property denoting the Container Id of an entity
        /// </summary>
        Int32? ContainerId { get; set; }

        /// <summary>
        /// Property denoting the Category Id of an entity
        /// </summary>
        Int64? CategoryId { get; set; }

        /// <summary>
        /// Property denoting the Type Id of an entity
        /// </summary>
        Int32? EntityTypeId { get; set; }

        /// <summary>
        /// Property denoting the Total Entities Count
        /// </summary>
        Int64? TotalEntitiesCount { get; set; }

        /// <summary>
        /// Property denoting the Not Validated Entities Count
        /// </summary>
        Int64? NotValidatedEntitiesCount { get; set; }

        /// <summary>
        /// Property denoting Measurement Date
        /// </summary>
        DateTime? MeasurementDate { get; set; }

        /// <summary>
        /// Property denoting the list of Data Quality Class Statistics of Summary
        /// </summary>
        DataQualityClassStatisticsCollection DQCStatistics { get; set; }
    }
}