using System;
using System.Collections.ObjectModel;

namespace MDM.BusinessObjects.Interfaces
{
    /// <summary>
    /// Exposes methods or properties to set or get the paginated entity result.
    /// </summary>
    public interface IEntityPaginationResult
    {
        /// <summary>
        /// Property for entity result for particular paging citeria
        /// </summary>
        EntityCollection EntityList { get; set; }

        /// <summary>
        /// Property for EntityId list for full result
        /// </summary>
        Collection<Int64> EntityIdList { get; set; }
    }
}
