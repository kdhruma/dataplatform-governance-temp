using System;

namespace MDM.Interfaces
{
    using Core;

    /// <summary>
    /// Exposes methods or properties to set or get entity extension context instance.
    /// </summary>
    public interface IEntityExtensionContext : IMDMObject
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
        /// Adds IEntityDataContext for the provided container name and category path
        /// </summary>
        /// <param name="containerName">Indicates  container name</param>
        /// <param name="categoryPath">Indicates category path</param>
        /// <param name="iEntityDataContext">IEntityDataContext to be set</param>
        void AddEntityDataContext(String containerName, String categoryPath, IEntityDataContext iEntityDataContext);

        /// <summary>
        /// Adds IEntityDataContext for the provided container qualifier name
        /// </summary>
        /// <param name="containerQualifierName">Indicates container qualifier name</param>
        /// <param name="iEntityDataContext">IEntityDataContext to be set</param>
        void AddEntityDataContext(String containerQualifierName, IEntityDataContext iEntityDataContext);

        /// <summary>
        /// Gets IEntityDataContext for the provided container name and category path
        /// </summary>
        /// <param name="containerName">Indicates  container name</param>
        /// <param name="categoryPath">Indicates category path</param>
        /// <returns>Entity data context from the entity extension context for the provided container name and category path</returns>
        IEntityDataContext GetEntityDataContext(String containerName, String categoryPath);

        /// <summary>
        /// Gets IEntityDataContext for the provided container qualifier name
        /// </summary>
        /// <param name="containerQualifierName">Indicates container qualifier name</param>
        /// <returns>Entity data context from the entity extension context for the provided container qualifier name</returns>
        IEntityDataContext GetEntityDataContext(String containerQualifierName);

        /// <summary>
        /// Gets XMl representation of the entity hierarchy context
        /// </summary>
        /// <returns>XMl string representation of entity hierarchy context</returns>
        String ToXml();

        #endregion
    }
}