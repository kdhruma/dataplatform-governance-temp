using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDM.BusinessObjects.Interfaces
{
    /// <summary>
    /// Exposes methods or properties to set or get the collection of filter criteria. 
    /// </summary>
    public interface IFilterCriteriaCollection : ICollection<FilterCriteria>
    {
    }
}
