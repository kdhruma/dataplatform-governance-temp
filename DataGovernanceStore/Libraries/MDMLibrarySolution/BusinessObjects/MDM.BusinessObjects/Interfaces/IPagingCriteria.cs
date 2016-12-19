using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDM.BusinessObjects.Interfaces
{
    /// <summary>
    /// Interface for paging criteria for MDM entities
    /// </summary>
    public interface IPagingCriteria
    {
        /// <summary>
        /// Number of records required in the page
        /// </summary>
        Int32 PageSize { get; set; }

        /// <summary>
        /// The page number of the records to retrieve
        /// </summary>
        Int32 PageNumber { get; set; }

        /// <summary>
        /// The sort parameters for the paginated records
        /// </summary>
        ISortCriteriaCollection SortParameters { get; set; }

        /// <summary>
        /// The groupby parameter to group the paginated records
        /// </summary>
        ICollection<String> GroupByParameters { get; set; }

        /// <summary>
        /// Property to filter the records based on provided filter criteria
        /// </summary>
        IFilterCriteriaCollection FilterCriteriaList { get; set; }
    }
}
