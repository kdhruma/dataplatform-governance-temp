using System;
using System.Collections.ObjectModel;

namespace MDM.Interfaces
{
    using MDM.Core;

    /// <summary>
    /// Exposes methods or properties to set or get quality indicator (DataQualityIndicator) related information.
    /// </summary>
    public interface IDataQualityIndicator : IMDMObject
    {
        /// <summary>
        /// Property denoting DataQualityIndicator Id
        /// </summary>        
        new Int16 Id { get; set; }

        /// <summary>
        /// Property denoting TableColumnName of DataQualityIndicatorValues Table
        /// </summary>
        String DataQualityIndicatorValuesTableColumnName { get; set; }

        /// <summary>
        /// Property denoting StateViewAttributeId
        /// </summary>
        Int32? StateViewAttributeId { get; set; }

        /// <summary>
        /// Property denoting Weight
        /// </summary>
        Byte? Weight { get; set; }

        /// <summary>
        /// Selected for validation container ids
        /// </summary>
        Collection<Int32> ContainerIds { get; set; }

        /// <summary>
        /// Selected for validation category ids
        /// </summary>
        Collection<Int64> CategoryIds { get; set; }

        /// <summary>
        /// Selected for validation entity type ids
        /// </summary>
        Collection<Int32> EntityTypeIds { get; set; }

        /// <summary>
        /// Specifies DataQualityIndicator description
        /// </summary>
        String Description { get; set; }
    }
}