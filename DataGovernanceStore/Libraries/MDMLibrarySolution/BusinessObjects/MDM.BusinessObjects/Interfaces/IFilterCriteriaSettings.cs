using System;
using System.Collections.ObjectModel;

namespace MDM.Interfaces
{
    /// <summary>
    /// Exposes methods or properties to set or get filter criteria.
    /// </summary>
    public interface IFilterCriteriaSettings : ICloneable
    {
        /// <summary>
        /// Property denoting user id
        /// </summary>
        Int32 UserId { get; set; }

        /// <summary>
        /// Property denoting container id
        /// </summary>
        Int32? ContainerId { get; set; }

        /// <summary>
        /// Property denoting category ids
        /// </summary>
        Collection<Int64> CategoryIds { get; set; }

        /// <summary>
        /// Property denoting entity type ids
        /// </summary>
        Collection<Int32> EntityTypeIds { get; set; }

        /// <summary>
        /// Property denoting DataQualityIndicator ids
        /// </summary>
        Collection<Int16> DataQualityIndicatorIds { get; set; }

        /// <summary>
        /// Property denoting quality data Date time
        /// </summary>
        DateTime? QualityDate { get; set; }

        /// <summary>
        /// Property denoting start trends date Date time
        /// </summary>
        DateTime? StartTrendsDate { get; set; }

        /// <summary>
        /// Property denoting end trends date Date time
        /// </summary>
        DateTime? EndTrendsDate { get; set; }
    }
}
