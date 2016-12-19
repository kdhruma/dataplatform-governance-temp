using System;
using System.Collections.Generic;

namespace MDM.Interfaces
{
    using MDM.BusinessObjects.DQM;
    using MDM.Core;

    /// <summary>
    /// Exposes methods or properties to set or get DQM detailed report parameters.
    /// </summary>
    public interface IDQMReport
    {       
        /// <summary>
        /// Denotes filter parameters. There are should be only one DataQualityClass in associated collection
        /// </summary>
        DQMFilter Filter { get; set; }

        /// <summary>
        /// Denotes locale for translations of the result column values
        /// </summary>
        LocaleEnum LocaleId { get; set; }

        /// <summary>
        /// Denotes countFrom parameter for paged result 
        /// </summary>
        Int64 CountFrom { get; set; }

        /// <summary>
        /// Denotes countTo parameter for paged result
        /// </summary>
        Int64 CountTo { get; set; }

        /// <summary>
        /// Denotes sort columns for paged result. It could be any column of result including attributeId e.x. SortColumn = "Category" or SortColumn = "4037"
        /// Support multiple sort columns. The order of columns defines presedence for sorting
        /// </summary>
        ICollection<String> SortColumns { get; set; }

        /// <summary>
        /// Denotes sort order for paged result. true for Descending, false for Ascending 
        /// </summary>
        Boolean SortOrder { get; set; }

        /// <summary>
        /// Denotes collection of result entities
        /// </summary>        
        ICollection<Int64> EntityIds { get; set; }

        /// <summary>
        /// Denotes collection of result attributes
        /// </summary>
        ICollection<Int32> AttributeIds { get; set; }

        /// <summary>
        /// Denotes data source for getting attributes
        /// </summary>
        String AttributeDataSource { get; set; }    
    }
}
