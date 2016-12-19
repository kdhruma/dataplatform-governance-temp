using System;

namespace MDM.Interfaces
{
    using MDM.BusinessObjects.DQM;

    /// <summary>
    /// Exposes methods or properties to set or get entity data quality report.
    /// </summary>
    public interface IEntityQualityReportData : IEntity
    {
        /// <summary>
        /// Property denoting the list of DataQualityIndicator Values of the entity
        /// </summary>
        DataQualityIndicatorValueCollection DataQualityIndicatorValues { get; set; }        

        /// <summary>
        /// Property denoting DataQualityIndicatorValues overallscore
        /// </summary>
        Byte ? OverallScore { get; set; }

        /// <summary>
        /// Property denoting Measurement Date
        /// </summary>
        DateTime? MeasurementDate { get; set; }
    }
}