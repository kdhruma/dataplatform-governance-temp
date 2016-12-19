using System;
using System.Collections.ObjectModel;

namespace MDM.Interfaces
{
    using MDM.BusinessObjects;
    using MDM.Core;

    /// <summary>
    /// Exposes methods or properties to set or get word element report settings.
    /// </summary>
    public interface IWordElementReportSettings
    {
        /// <summary>
        /// Specifies parent Word List Id
        /// </summary>
        Int32 WordListId { get; set; }

        /// <summary>
        /// Specifies CountFrom parameter for paged result (including element with CountFrom index, starting from 1)
        /// </summary>
        Int32? CountFrom { get; set; }

        /// <summary>
        /// Specifies CountTo parameter for paged result (including element with CountTo index)
        /// </summary>
        Int32? CountTo { get; set; }

        /// <summary>
        /// Denotes filtering parameters for each required WordElement field
        /// </summary>
        Collection<GenericSearchRule<WordElementColumn>> WordElementsSearchColumns { get; set; }

        /// <summary>
        /// Denotes sorting parameters for each required WordElement field
        /// </summary>
        Collection<GenericSortRule<WordElementColumn>> WordElementsSortColumns { get; set; }
    }
}
