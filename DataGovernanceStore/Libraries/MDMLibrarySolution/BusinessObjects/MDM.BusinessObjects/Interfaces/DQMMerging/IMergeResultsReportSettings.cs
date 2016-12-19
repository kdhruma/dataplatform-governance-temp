using System;
using System.Collections.ObjectModel;

namespace MDM.Interfaces
{
    using MDM.Core;
    using MDM.BusinessObjects;

    /// <summary>
    /// Exposes methods or properties to set or get merge results report details.
    /// </summary>
    public interface IMergeResultsReportSettings
    {
        /// <summary>
        /// Denotes Ids of parent jobs
        /// </summary>
        Collection<Int64> JobIds { get; set; }

        /// <summary>
        /// Denotes countFrom parameter for paged result
        /// </summary>
        Int64? CountFrom { get; set; }

        /// <summary>
        /// Denotes countTo parameter for paged result
        /// </summary>
        Int64? CountTo { get; set; }

        /// <summary>
        /// Denotes sort columns for paged result. It can have values of MergeResultReportPredefinedColumn (negative ones) or attributeId
        /// </summary>
        Collection<Int32> SortColumns { get; set; }

        /// <summary>
        /// Denotes sort order for paged result. true for Descending, false for Ascending
        /// </summary>
        Boolean SortOrder { get; set; }

        /// <summary>
        /// Denotes DateFilterFrom
        /// </summary>
        DateTime? DateFilterFrom { get; set; }

        /// <summary>
        /// Denotes DateFilterTo
        /// </summary>
        DateTime? DateFilterTo { get; set; }
        
        /// <summary>
        /// Denotes search attribute rules
        /// </summary>
        Collection<SearchAttributeRule> SearchAttributeRules { get; set; }

        /// <summary>
        /// Denotes locale for translations of the result column values
        /// </summary>
        LocaleEnum LocaleId { get; set; }

        /// <summary>
        /// Denotes total items count including all pages
        /// </summary>
        Boolean RequestTotalItemsCount { get; set; }
    }
}