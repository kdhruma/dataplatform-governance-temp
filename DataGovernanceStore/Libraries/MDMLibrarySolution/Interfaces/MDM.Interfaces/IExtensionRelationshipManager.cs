using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace MDM.Interfaces
{
    using MDM.Core;
    using MDM.BusinessObjects;

    /// <summary>
    /// Indicates Extension Relationship manager interface
    /// </summary>
    public interface IExtensionRelationshipManager
    {
        /// <summary>
        /// Gets extension relationships for the requested entity Id
        /// <para>
        /// This method loads the entity object. So use this method only when if entity object is not available.
        /// </para>
        /// </summary>
        /// <param name="entityId">Entity Id for which extensions are required</param>
        /// <param name="application">Name of application which is performing action</param>
        /// <param name="module">Name of module which is performing action</param>
        /// <returns>Extension relationships</returns>
        /// <exception cref="ArgumentException">Thrown when the entity Id parameter is not available</exception>
        /// <exception cref="Exception">Thrown when entity object is not available for the requested entity id</exception>
        ExtensionRelationshipCollection Get(Int64 entityId, MDMCenterApplication application, MDMCenterModules module);

        /// <summary>
        /// Gets extension relationships for the requested entity Id
        /// <para>
        /// This method loads the entity object. So use this method only when if entity object is not available.
        /// </para>
        /// </summary>
        /// <param name="entityId">Entity Id for which extensions are required</param>
        /// <param name="getLatest">Boolean flag which says whether to get from DB or cache. True means always get from DB</param>
        /// <param name="application">Name of application which is performing action</param>
        /// <param name="module">Name of module which is performing action</param>
        /// <returns>Extension relationships</returns>
        /// <exception cref="ArgumentException">Thrown when the entity Id parameter is not available</exception>
        /// <exception cref="Exception">Thrown when entity object is not available for the requested entity id</exception>
        ExtensionRelationshipCollection Get(Int64 entityId, Boolean getLatest, MDMCenterApplication application, MDMCenterModules module);

        /// <summary>
        /// Loads the entity with the extension relationships
        /// </summary>
        /// <param name="entity">Entity for which extension relationships needs to be loaded</param>
        /// <param name="loadLatest">Boolean flag which says whether to load from DB or cache. True means always load from DB</param>
        /// <param name="entityCacheStatus">Specifies the Cache status of the Entity.</param>
        /// <param name="application">Name of application which is performing action</param>
        /// <param name="module">Name of module which is performing action</param>
        /// <param name="updateCache">Specifies whether to update cache.</param>
        /// <param name="updateCacheStatusInDB">Specifies whether to update cache status in database.</param>
        /// <returns>The operation result</returns>
        /// <exception cref="ArgumentNullException">Thrown when entity parameter is null</exception>
        /// <exception cref="ArgumentException">Thrown when the entity Id of the entity parameter is not available</exception>
        EntityOperationResult Load(Entity entity, Boolean loadLatest, EntityCacheStatus entityCacheStatus, MDMCenterApplication application, MDMCenterModules module, Boolean updateCache = false, Boolean updateCacheStatusInDB = true);

        /// <summary>
        /// Processes the extensions of the entities
        /// </summary>
        /// <param name="entityCollection">Entities for which extensions needs to be processed</param>
        /// <param name="entityCacheStatusCollection">Entities for which extensions needs to be processed</param>
        /// <param name="programName">Name of program which is performing action</param>
        /// <param name="application">Name of application which is performing action</param>
        /// <param name="module">Name of module which is performing action</param>
        /// <param name="processingMode">Mode of processing (Sync or Async)</param>
        /// <param name="saveCacheStatus">Specifies whether to save the entity cache status in database.</param>
        /// <returns>Entity Operation Results having results of the operation</returns>
        /// <exception cref="ArgumentNullException">Thrown when entityCollection is null</exception>
        /// <exception cref="ArgumentException">Thrown when entityCollection is having zero entities</exception>
        EntityOperationResultCollection Process(EntityCollection entityCollection, EntityCacheStatusCollection entityCacheStatusCollection, String programName, MDMCenterApplication application, MDMCenterModules module, ProcessingMode processingMode = ProcessingMode.Sync, Boolean saveCacheStatus = true);

        /// <summary>
        /// Changes parent of the extension and its child during reverse MDL 
        /// </summary>
        /// <param name="fromEntityId">Indicates parent extension Id as per INH path</param>
        /// <param name="toEntityId">Indicates child extension Id as per INH path</param>
        /// <param name="callerContext">Indicates Application and Module name by which action is being performed</param>
        /// <param name="processingMode">Mode of processing (Sync or Async)</param>
        /// <returns>Returns true if extension ReParent is successful, otherwise false.</returns>
        /// <exception cref="MDMOperationException">Thrown when fromEntityId or toEntityId is not available.</exception>
        Boolean ReParent(Int64 fromEntityId, Int64 toEntityId, CallerContext callerContext, ProcessingMode processingMode = ProcessingMode.Sync);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="extensionRelationships"></param>
        /// <param name="entityContext"></param>
        /// <param name="loadRecursive"></param>
        /// <param name="callerContext"></param>
        /// <returns></returns>
        OperationResult LoadRelatedEntities(ExtensionRelationshipCollection extensionRelationships, EntityContext entityContext, Boolean loadRecursive, CallerContext callerContext);

        /// <summary>
        /// Get entension entities count dictionary based on parent extension entity Ids
        /// </summary>
        /// <param name="entityIds">Indicates the parent extensions entity identifiers.</param>
        /// <param name="callerContext">Indicates application and module name by which action is being performed</param>
        /// <returns>Returns entension entities count dictionary based on parent extension entity Ids</returns>
        Dictionary<Int64, Dictionary<Int32, Int32>> GetExtensionsRelationshipsCount(Collection<Int64> entityIds, CallerContext callerContext);

    }
}
