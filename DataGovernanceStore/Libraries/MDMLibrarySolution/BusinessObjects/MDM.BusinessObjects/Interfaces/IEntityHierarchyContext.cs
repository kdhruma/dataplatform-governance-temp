using System;

namespace MDM.Interfaces
{
    using Core;

    /// <summary>
    /// Exposes methods or properties to set or get entity hierarchy context instance.
    /// </summary>
    public interface IEntityHierarchyContext : IMDMObject
    {
        #region Properties

        #endregion

        #region Methods

        /// <summary>
        /// Gets all entity data contexts for current entity hierarchy context
        /// </summary>
        /// <returns>Collection of entity data context from the entity hierarchy context</returns>
        IEntityDataContextCollection GetEntityDataContexts();

        /// <summary>
        /// Adds IEntityDataContext for the provided entity type name
        /// </summary>
        /// <param name="entityTypeName">Entity type name</param>
        /// <param name="entityDataContext">IEntityDataContext to be set</param>
        void AddEntityDataContext(String entityTypeName, IEntityDataContext entityDataContext);

        /// <summary>
        /// Gets IEntityDataContext for the provided entity type name
        /// </summary>
        /// <param name="entityTypeName">Enity type name</param>
        /// <returns>Entity data context from the entity hierarchy context for the provided entity type name</returns>
        IEntityDataContext GetEntityDataContext(String entityTypeName);

        /// <summary>
        /// Gets XMl representation of the entity hierarchy context
        /// </summary>
        /// <returns>XMl string representation of entity hierarchy context</returns>
        String ToXml();

        #endregion
    }
}
