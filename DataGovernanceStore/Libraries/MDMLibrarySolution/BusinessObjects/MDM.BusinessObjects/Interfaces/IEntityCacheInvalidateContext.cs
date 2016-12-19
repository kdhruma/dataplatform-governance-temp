using System;

namespace MDM.Interfaces
{
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Exposes methods or properties to set or get entity cache invalidate context.
    /// </summary>
    public interface IEntityCacheInvalidateContext : IMDMObject
    {
        #region Fields

        /// <summary>
        /// Specifies entity id for cache invalidation
        /// </summary>
        Int64 EntityId { get; set; }

        /// <summary>
        /// Specifies container id for cache invalidation
        /// </summary>
        Int32 ContainerId { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Represents EntityCacheInvalidateContext in XML format
        /// </summary>
        /// <returns>String representation of current EntityCacheInvalidateContext object</returns>
        String ToXml();

        #endregion
    }
}