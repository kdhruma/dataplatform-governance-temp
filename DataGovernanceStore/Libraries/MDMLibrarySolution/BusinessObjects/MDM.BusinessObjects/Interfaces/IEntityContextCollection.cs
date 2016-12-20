using System;
using System.Collections.Generic;

namespace MDM.Interfaces
{
    using MDM.BusinessObjects;

    /// <summary>
    /// Exposes methods or properties used for EntityContextCollection object.
    /// </summary>
    public interface IEntityContextCollection : IEnumerable<EntityContext>, ICollection<EntityContext>
    {
        #region Methods

        /// <summary>
        /// Gets Xml representation of EntityScopeCollection object
        /// </summary>
        /// <returns>Xml string representing the EntityScopeCollection</returns>
        String ToXml();

        /// <summary>
        /// Adds EntityContext in collection
        /// </summary>
        /// <param name="iEntityContext">Indicates an EntityContext to add in collection</param>
        void Add(IEntityContext iEntityContext);

        /// <summary>
        /// Adds EntityContextCollection in collection
        /// </summary>
        /// <param name="iEntityContextCollection">Indicates an EntityContextCollection to add in collection</param>
        void AddRange(IEntityContextCollection iEntityContextCollection);

        /// <summary>
        /// Gets entity context instance by entity type id from current collection
        /// </summary>
        /// <param name="entityTypeId">Specifies entity type id</param>
        /// <returns>Entity Context</returns>
        IEntityContext GetByEntityTypeId(Int32 entityTypeId);

        /// <summary>
        /// Gets entity context instance by entity type name from current collection
        /// </summary>
        /// <param name="entityTypeName">Specifies entity type name</param>
        /// <returns>Entity Context</returns>
        IEntityContext GetByEntityTypeName(String entityTypeName);
        
        /// <summary>
        /// Gets entity context instance by container id and category id from current collection
        /// </summary>
        /// <param name="containerId">Specifies container id</param>
        /// <param name="categoryId">Specifies category id</param>
        /// <returns>Entity Context</returns>
        IEntityContext GetByContainerAndCategoryId(Int32 containerId, Int64 categoryId);

        /// <summary>
        /// Gets entity context instance by container name and category path from current collection
        /// </summary>
        /// <param name="containerName">Specifies container name</param>
        /// <param name="categoryPath">Specifies category path</param>
        /// <returns>Entity Context</returns>
        IEntityContext GetByContainerNameAndCategoryPath(String containerName, String categoryPath);

        /// <summary>
        /// Gets entity context instance by container qualifier id from current collection
        /// </summary>
        /// <param name="containerQualifierId">Specifies container qualifier id</param>
        /// <returns>Entity Context</returns>
        IEntityContext GetByContainerQualifierId(Int32 containerQualifierId);

        /// <summary>
        /// Gets entity context instance by container qualifier name from current collection
        /// </summary>
        /// <param name="containerQualifierName">Specifies container qualifier name</param>
        /// <returns>Entity Context</returns>
        IEntityContext GetByContainerQualifierName(String containerQualifierName);

        #endregion Methods
    }
}