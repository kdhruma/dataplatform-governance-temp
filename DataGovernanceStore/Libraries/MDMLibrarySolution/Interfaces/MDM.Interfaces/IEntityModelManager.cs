using System;
using System.Collections.ObjectModel;

namespace MDM.Interfaces
{
    using MDM.BusinessObjects;

    /// <summary>
    /// Interface specifies the definition for an entity model manager
    /// </summary>
    public interface IEntityModelManager
    {
        #region Methods

        /// <summary>
        /// Builds and returns an entity model context based on the various model names provided
        /// </summary>
        /// <param name="callerContext">Represents the caller context to indicate application and module which has called the method</param>
        /// <param name="organizationName">Represents the short name of the organization</param>
        /// <param name="containerName">Represents the short name of the container</param>
        /// <param name="hierarchyName">Represents the short name of the hierarchy</param>
        /// <param name="entityTypeName">Represents the short name of the entity type</param>
        /// <param name="categoryName">Represents the short name of the category</param>
        /// <param name="relationshipTypeNames">Represents the short name of the relationship type</param>
        /// <returns>An entity model context object</returns>
        EntityModelContext GetEntityModelContextByName(CallerContext callerContext, String organizationName = null, String containerName = null,
            String hierarchyName = null, String entityTypeName = null, String categoryName = null, Collection<String> relationshipTypeNames = null);

        /// <summary>
        /// Fills the entity model context based on the model names present in the context
        /// </summary>
        /// <param name="entityModelContext">Represents the entity model context to be filled</param>
        /// <param name="callerContext">Represents the caller context to indicate application and module which has called the method</param>
        void FillEntityModelContextByName(ref EntityModelContext entityModelContext, CallerContext callerContext);

        /// <summary>
        /// Gets the Organization Id based on organization name.
        /// </summary>
        /// <param name="organizationName">Represents the short name of the organization</param>
        /// <param name="callerContext">Represents the caller context to indicate application and module which has called the method</param>
        /// <returns>The organization id</returns>
        Int32 GetOrganizationIdByName(String organizationShortName, CallerContext callerContext);

        /// <summary>
        /// Gets the Container Id based on container name.
        /// </summary>
        /// <param name="containerName">Represents the short name of the container</param>
        /// <param name="callerContext">Represents the caller context to indicate application and module which has called the method</param>
        /// <returns>The container id</returns>
        Int32 GetContainerIdByName(String containerName, CallerContext callerContext);

        /// <summary>
        /// Gets the hierarchy Id based on container name.
        /// </summary>
        /// <param name="hierarchyName">Represents the short name of the hierarchy</param>
        /// <param name="callerContext">Represents the caller context to indicate application and module which has called the method</param>
        /// <returns>The hierarchy id</returns>
        Int32 GetHierarchyIdByName(String hierarchyName, CallerContext callerContext);

        /// <summary>
        /// Gets the entity type Id based on container name.
        /// </summary>
        /// <param name="entityTypeName">Represents the short name of the entity type</param>
        /// <param name="callerContext">Represents the caller context to indicate application and module which has called the method</param>
        /// <returns>The entity type id</returns>
        Int32 GetEntityTypeIdByName(String entityTypeName, CallerContext callerContext);

        /// <summary>
        /// Gets the category Id based on hierarchy id and category name.
        /// </summary>
        /// <param name="hierarchyId">Represents the hierarchy id of the category</param>
        /// <param name="categoryName">Represents the short name of the category</param>
        /// <param name="callerContext">Represents the caller context to indicate application and module which has called the method</param>
        /// <returns>The category id</returns>
        Int64 GetCategoryIdByName(Int32 hierarchyId, String categoryName, CallerContext callerContext);

        /// <summary>
        /// Gets the relationship type Id based on relationship type name.
        /// </summary>
        /// <param name="relationshipTypeName">Represents the short name of the entity type</param>
        /// <param name="callerContext">Represents the caller context to indicate application and module which has called the method</param>
        /// <returns>The relationship type id</returns>
        Int32 GetRelationshipTypeIdByName(String relationshipTypeName, CallerContext callerContext);

        /// <summary>
        /// Gets the relationship type Id based on relationship type name.
        /// </summary>
        /// <param name="relationshipTypeNames">Represents the short names of the entity type</param>
        /// <param name="callerContext">Represents the caller context to indicate application and module which has called the method</param>
        /// <returns>The relationship type ids</returns>
        Collection<Int32> GetRelationshipTypeIdByNames(Collection<String> relationshipTypeNames, CallerContext callerContext);

        #endregion
    }
}
