using System;

namespace MDM.Interfaces
{
    using MDM.BusinessObjects.DQM;

    /// <summary>
    /// Exposes methods or properties to set or get DataQualityIndicator value.
    /// </summary>
    public interface IDataQualityIndicatorValue
    {
        /// <summary>
        /// Denotes DataQualityIndicator Id
        /// </summary>
        Int16 DataQualityIndicatorId { get; set; }

        /// <summary>
        /// Denotes DataQualityIndicator Value
        /// </summary>
        Boolean? Value { get; set; }

        /// <summary>
        /// Denotes DataQualityIndicatorFailureInfo Collection
        /// </summary>
        DataQualityIndicatorFailureInfoCollection FailureInfoCollection { get; set; }

    }
}