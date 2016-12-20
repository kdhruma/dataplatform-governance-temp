using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDM.BusinessObjects.Interfaces
{
    /// <summary>
    /// Interface for sort criteria
    /// </summary>
    public interface ISortCriteria
    {
        /// <summary>
        /// Property for Sort paramter
        /// </summary>
        String SortParameter { get; set; }

        /// <summary>
        /// Indicates whether the sorted result should be ordered on descending order,
        /// default order would be an ascending order
        /// </summary>
        bool IsDescendingOrder { get; set; }
    }
}
