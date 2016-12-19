using System;
using System.Collections.ObjectModel;

namespace MDM.Interfaces
{
    using MDM.BusinessObjects;
    using MDM.BusinessObjects.DQM;
    using MDM.BusinessObjects.MergeCopy;
    using MDM.Core;

    /// <summary>
    /// Exposes methods or properties used for providing merging related information.
    /// </summary>
    public interface IMergingManager
    {
        #region Merge Entities Methods Which Use Ids

        /// <summary>
        /// Merge Entity from source to target using default MergeCopyContext.Context
        /// </summary>
        /// <param name="sourceEntityId">Property denotes which entity should be used as source</param>
        /// <param name="targetEntityId">Property denotes which entity should be used as target</param>
        /// <param name="callerContext">Indicates Application and Module name by which action is being performed</param>
        /// <returns>Return 'true' if operation completed successfully</returns>
        Boolean MergeEntities(Int64 sourceEntityId, Int64 targetEntityId, CallerContext callerContext);

        /// <summary>
        /// Merge Entity from source to target using default MergeCopyContext.Context and defined MergeCopyEntityExtensionProvider
        /// </summary>
        /// <param name="sourceEntityId">Property denotes which entity should be used as source</param>
        /// <param name="targetEntityId">Property denotes which entity should be used as target</param>
        /// <param name="callerContext">Indicates Application and Module name by which action is being performed</param>
        /// <param name="mergeCopyEntityExtensionProvider">Entity-Entity extension provider</param>
        /// <returns>Return 'true' if operation completed successfully</returns>
        Boolean MergeEntities(Int64 sourceEntityId, Int64 targetEntityId,
            IMergeCopyEntityExtensionProvider mergeCopyEntityExtensionProvider, CallerContext callerContext);

        /// <summary>
        /// Merge Entity from source to target using defined MergeCopyContext.Context
        /// </summary>
        /// <param name="sourceEntityId">Property denotes which entity should be used as source</param>
        /// <param name="targetEntityId">Property denotes which entity should be used as target</param>
        /// <param name="callerContext">Indicates Application and Module name by which action is being performed</param>
        /// <param name="mergeCopyContext">Context with various flags and user info</param>
        /// <returns>Return 'true' if operation completed successfully</returns>
        Boolean MergeEntities(Int64 sourceEntityId, Int64 targetEntityId, MergeCopyContext.Context mergeCopyContext,
            CallerContext callerContext);

        #endregion

        #region Merge Entities Methods Which Use Entity-Entity approach

        /// <summary>
        /// Merge Entity from source to target using default MergeCopyContext.Context
        /// </summary>
        /// <param name="sourceEntity">Property denotes which entity should be used as source</param>
        /// <param name="targetEntity">Property denotes which entity should be used as target</param>
        /// <param name="callerContext">Indicates Application and Module name by which action is being performed</param>
        /// <returns>Return target entity</returns>
        Entity MergeEntities(Entity sourceEntity, Entity targetEntity, CallerContext callerContext);

        /// <summary>
        /// Merge Entity from source to target using default MergeCopyContext.Context and defined MergeCopyEntityExtensionProvider
        /// </summary>
        /// <param name="sourceEntity">Property denotes which entity should be used as source</param>
        /// <param name="targetEntity">Property denotes which entity should be used as target</param>
        /// <param name="mergeCopyEntityExtensionProvider">Entity-Entity extension provider</param>
        /// <param name="callerContext">Indicates Application and Module name by which action is being performed</param>
        /// <returns>Return target entity</returns>
        Entity MergeEntities(Entity sourceEntity, Entity targetEntity,
            IMergeCopyEntityExtensionProvider mergeCopyEntityExtensionProvider, CallerContext callerContext);

        /// <summary>
        /// Merge Entity from source to target using defined MergeCopyContext.Context
        /// </summary>
        /// <param name="sourceEntity">Property denotes which entity should be used as source</param>
        /// <param name="targetEntity">Property denotes which entity should be used as target</param>
        /// <param name="mergeCopyContext">Context with various flags and user info</param>
        /// <param name="callerContext">Indicates Application and Module name by which action is being performed</param>
        /// <returns>Return target entity</returns>
        Entity MergeEntities(Entity sourceEntity, Entity targetEntity, MergeCopyContext.Context mergeCopyContext,
            CallerContext callerContext);

        #endregion

        #region Merge Entities Methods Which Use EntityCollection-Entity approach

        /// <summary>
        /// Merge Entity from several sources to target using default MergeCopyContext.Context
        /// </summary>
        /// <param name="sourceEntities">Property denotes which entities should be used as source</param>
        /// <param name="targetEntity">Property denotes which entity should be used as target</param>
        /// <param name="callerContext">Indicates Application and Module name by which action is being performed</param>
        /// <returns>Return target entity</returns>
        Entity MergeEntitiesBatch(EntityCollection sourceEntities, Entity targetEntity, CallerContext callerContext);

        /// <summary>
        /// Merge Entity from several sources to target using default MergeCopyContext.Context and defined MergeCopyEntityExtensionProvider
        /// </summary>
        /// <param name="sourceEntities">Property denotes which entities should be used as source</param>
        /// <param name="targetEntity">Property denotes which entity should be used as target</param>
        /// <param name="callerContext">Indicates Application and Module name by which action is being performed</param>
        /// <param name="mergeCopyEntityExtensionProvider">Entity-Entity extension provider</param>
        /// <returns>Return target entity</returns>
        Entity MergeEntitiesBatch(EntityCollection sourceEntities, Entity targetEntity,
            IMergeCopyEntityExtensionProvider mergeCopyEntityExtensionProvider, CallerContext callerContext);

        /// <summary>
        /// Merge Entity from several sources to target using defined MergeCopyContext.Context
        /// </summary>
        /// <param name="sourceEntities">Property denotes which entities should be used as source</param>
        /// <param name="targetEntity">Property denotes which entity should be used as target</param>
        /// <param name="callerContext">Indicates Application and Module name by which action is being performed</param>
        /// <param name="mergeCopyContext">Context with various flags and user info</param>
        /// <returns>Return target entity</returns>
        Entity MergeEntitiesBatch(EntityCollection sourceEntities, Entity targetEntity,
            MergeCopyContext.Context mergeCopyContext, CallerContext callerContext);

        /// <summary>
        /// Merge Entity from several sources to target using default MergeCopyContext.Context
        /// </summary>
        /// <param name="sourceEntitiesIds">Property denotes which entities should be used as source</param>
        /// <param name="targetEntityId">Property denotes which entity should be used as target</param>
        /// <param name="callerContext">Indicates Application and Module name by which action is being performed</param>
        /// <returns>Return target entity</returns>
        Entity MergeEntitiesBatch(Collection<Int64> sourceEntitiesIds, Int64 targetEntityId, CallerContext callerContext);

        /// <summary>
        /// Merge Entity from several sources to target using defined MergeCopyEntityExtensionProvider
        /// </summary>
        /// <param name="sourceEntitiesIds">Property denotes which entities should be used as source</param>
        /// <param name="targetEntityId">Property denotes which entity should be used as target</param>
        /// <param name="mergeCopyEntityExtensionProvider">Entity-Entity extension provider</param>
        /// <param name="callerContext">Indicates Application and Module name by which action is being performed</param>
        /// <returns>Return target entity</returns>
        Entity MergeEntitiesBatch(Collection<Int64> sourceEntitiesIds, Int64 targetEntityId,
            IMergeCopyEntityExtensionProvider mergeCopyEntityExtensionProvider, CallerContext callerContext);

        /// <summary>
        /// Merge Entity from several sources to target using defined MergeCopyContext.Context
        /// </summary>
        /// <param name="sourceEntitiesIds">Property denotes which entities should be used as source</param>
        /// <param name="targetEntityId">Property denotes which entity should be used as target</param>
        /// <param name="callerContext">Indicates Application and Module name by which action is being performed</param>
        /// <param name="mergeCopyContext">Context with various flags and user info</param>
        /// <returns>Return target entity</returns>
        Entity MergeEntitiesBatch(Collection<Int64> sourceEntitiesIds, Int64 targetEntityId,
            MergeCopyContext.Context mergeCopyContext, CallerContext callerContext);

        #endregion

        #region Create Entity Methods

        /// <summary>
        /// Copy Entity from source to target container using default MergeCopyContext.Context
        /// </summary>
        /// <param name="sourceEntityId">Property denotes which entity should be used as source</param>
        /// <param name="targetContainerId">Property denotes which container should be used as target</param>
        /// <param name="callerContext">Indicates Application and Module name by which action is being performed</param> 
        /// <returns>Return Entity Id if operation completed successfully</returns>
        Int64? CreateEntity(Int64 sourceEntityId, Int64 targetContainerId, CallerContext callerContext);

        /// <summary>
        /// Copy Entity from source to target container using default MergeCopyContext.Context and defined MergeCopyEntityExtensionProvider
        /// </summary>
        /// <param name="sourceEntityId">Property denotes which entity should be used as source</param>
        /// <param name="targetContainerId">Property denotes which container should be used as target</param>
        /// <param name="mergeCopyEntityExtensionProvider">Entity-Entity extension provider</param>
        /// <param name="callerContext">Indicates Application and Module name by which action is being performed</param> 
        /// <returns>Return Entity Id if operation completed successfully</returns>
        Int64? CreateEntity(Int64 sourceEntityId, Int64 targetContainerId,
            IMergeCopyEntityExtensionProvider mergeCopyEntityExtensionProvider, CallerContext callerContext);

        /// <summary>
        /// Copy Entity from source to target container using chosed MergeCopyContext.Context
        /// </summary>
        /// <param name="sourceEntityId">Property denotes which entity should be used as source</param>
        /// <param name="targetContainerId">Property denotes which container should be used as target</param>
        /// <param name="mergeCopyContext">Context with various flags and user info</param>
        /// <param name="callerContext">Indicates Application and Module name by which action is being performed</param> 
        /// <returns>Return Entity Id if operation completed successfully</returns>
        Int64? CreateEntity(Int64 sourceEntityId, Int64 targetContainerId,
            MergeCopyContext.Context mergeCopyContext, CallerContext callerContext);

        #endregion
        
        /// <summary>
        /// Process Merge Plan item
        /// </summary>
        /// <param name="mergePlanItem">Property denoting parameters of merge process</param>
        /// <param name="mergeMode">Profile Merge Mode</param>
        /// <param name="mergeJobId">Job id which called logic</param>
        /// <param name="callerContext"></param>
        /// <returns>Returns result of merging</returns>
        MergeOperationResult ProcessMergePlanItem(MergePlanItem mergePlanItem, MergeMode mergeMode, Int64? mergeJobId,
            CallerContext callerContext);

        /// <summary>
        /// Merges the entities.
        /// </summary>
        /// <param name="sourceEntity">The source entity.</param>
        /// <param name="targetEntityId">The target entity identifier.</param>
        /// <param name="mergeProfileId">The merge profile identifier.</param>
        /// <param name="callerContext">The caller context.</param>
        /// <returns>Returns the merged target entity</returns>
        Entity MergeEntities(Entity sourceEntity, Int64 targetEntityId, Int32 mergeProfileId, CallerContext callerContext);
    }
}
