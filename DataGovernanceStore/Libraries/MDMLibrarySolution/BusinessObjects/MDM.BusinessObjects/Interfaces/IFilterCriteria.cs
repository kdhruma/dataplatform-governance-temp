using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDM.BusinessObjects.Interfaces
{
    /// <summary>
    /// Exposes methods or properties to set or get the filter criteria.
    /// </summary>
    public interface IFilterCriteria
    {
        /// <summary>
        /// Name of the filtered field
        /// </summary>
        String FilteredField { get; set; }

        /// <summary>
        /// Value of the filtered field
        /// </summary>
        String FilteredFieldValue { get; set; }
    }
}
