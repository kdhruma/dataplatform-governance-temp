using System;
using System.Drawing;

namespace MDM.Interfaces
{
    using MDM.BusinessObjects;
    using MDM.Core;

    /// <summary>
    /// Exposes methods or properties to set or get data quality class.
    /// </summary>
    public interface IDataQualityClass : IMDMObject, ICloneable
    {
        /// <summary>
        /// Property for the Id of DataQualityClass
        /// </summary>
        new Int16 Id { get; set; }

        /// <summary>
        /// Property for ValueSegment
        /// </summary>
        ValueSegment ValueSegment { get; set; }

        /// <summary>
        /// Property for DataQualityIndicatorSummaryTableColumnName
        /// </summary>
        String DataQualityIndicatorSummaryTableColumnName { get; set; }

        /// <summary>
        /// Property for FillColor
        /// </summary>
        Color? FillColor { get; set; }

        /// <summary>
        /// Property for SortOrder
        /// </summary>
        Int32? SortOrder { get; set; }

        /// <summary>
        /// Property for Localization Message Code
        /// </summary>
        String LocalizationMessageCode { get; set; }
    }
}