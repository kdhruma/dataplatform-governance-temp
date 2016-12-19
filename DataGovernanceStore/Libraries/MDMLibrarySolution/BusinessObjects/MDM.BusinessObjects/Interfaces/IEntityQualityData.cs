using System;

namespace MDM.Interfaces
{
    using MDM.BusinessObjects.DQM;

    /// <summary>
    /// Exposes methods or properties to set or get entity DataQualityIndicator data.
    /// </summary>
    public interface IEntityQualityData
    {
        /// <summary>
        /// Property denoting Entity Id
        /// </summary>
        Int64 EntityId { get; set; }

        /// <summary>
        /// Property denoting Overall Score
        /// </summary>
        Byte? OverallScore { get; set; }

        /// <summary>
        /// Property denoting Measurement Date
        /// </summary>
        DateTime? MeasurementDate { get; set; }

        /// <summary>
        /// Property denoting the list of DataQualityIndicator Values of an entity
        /// </summary>
        DataQualityIndicatorValueCollection DataQualityIndicatorValues { get; set; }
    }
}