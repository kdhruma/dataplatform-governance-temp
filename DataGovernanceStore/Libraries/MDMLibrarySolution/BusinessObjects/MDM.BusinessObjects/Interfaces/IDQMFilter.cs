using System;
using System.Collections.ObjectModel;

namespace MDM.Interfaces
{
    using MDM.BusinessObjects;
    using MDM.BusinessObjects.DQM;
    using MDM.Core;

    /// <summary>
    /// Exposes methods or properties to set or get DQM filter.
    /// </summary>
    public interface IDQMFilter : IMDMObject
    {
        /// <summary>
        /// Property denoting container ids
        /// </summary>
        Collection<Int32> ContainerIds { get; set; }

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
        /// Property denoting data quality class Ids
        /// </summary>
        Collection<Int16> DataQualityClassIds { get; set; }

        /// <summary>
        /// Property denoting filter Start Date time
        /// </summary>
        DateTime ? FilterStartDateTime { get; set; }

        /// <summary>
        /// Property denoting filter End Date time
        /// </summary>
        DateTime ? FilterEndDateTime { get; set; }

        /// <summary>
        /// Property denoting search attribute rules
        /// </summary>
        Collection<SearchAttributeRule> SearchAttributeRules { get; set; }

        /// <summary>
        /// Property denoting search DataQualityIndicator rules
        /// </summary>
        Collection<SearchDataQualityIndicatorRule> SearchDataQualityIndicatorRules { get; set; }

        /// <summary>
        /// Property denoting search SearchColumn rules
        /// </summary>
        Collection<SearchColumnRule> SearchColumnRules { get; set; }

    }
}