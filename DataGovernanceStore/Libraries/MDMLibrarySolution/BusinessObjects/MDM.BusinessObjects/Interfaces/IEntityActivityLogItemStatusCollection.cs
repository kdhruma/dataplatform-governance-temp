using System;
using System.Collections.Generic;

namespace MDM.Interfaces
{
    using MDM.BusinessObjects;

    /// <summary>
    /// Exposes methods or properties to set or get entity activity log item status collection for data quality management system.
    /// </summary>
    public interface IEntityActivityLogItemStatusCollection : ICollection<EntityActivityLogItemStatus>
    {
        /// <summary>
        /// Find EntityActivityLogItemStatus from EntityActivityLogItemStatusCollection based on entityActivityLogId
        /// </summary>
        /// <param name="entityActivityLogId">entityActivityLogId to search in result collection</param>
        /// <returns>EntityActivityLogItemStatus object having given entityActivityLogId</returns>
        EntityActivityLogItemStatus this[Int64 entityActivityLogId] { get; set; }

        /// <summary>
        /// Get the EntityActivityLogItemStatus based on the unique entityActivityLogId
        /// </summary>
        /// <param name="entityActivityLogId">pk of EntityActivityLogItemStatus</param>
        /// <returns>EntityActivityLogItemStatus object having given entityActivityLogId</returns>
        EntityActivityLogItemStatus GetEntityActivityLogStatus(Int64 entityActivityLogId);
    }
}
