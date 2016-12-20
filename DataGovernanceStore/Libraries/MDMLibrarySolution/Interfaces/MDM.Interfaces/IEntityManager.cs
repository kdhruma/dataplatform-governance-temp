using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace MDM.Interfaces
{
    using BusinessObjects;
    using Core;
    using MDM.BusinessObjects.Workflow;

    public interface IEntityManager
    {
        EntityOperationResultCollection BulkUpdateEntityAttributes(EntityCollection templateEntities, Collection<Int64> entityIdsToProcess, string actionPerformed, CallerContext callerContext);

        EntityOperationResult Create(Entity entity, EntityProcessingOptions entityProcessingOptions, string programName, MDMCenterApplication application, MDMCenterModules module);
        EntityOperationResult Create(Entity entity, string programName, MDMCenterApplication application, MDMCenterModules module);
        EntityOperationResultCollection Create(EntityCollection entities, EntityProcessingOptions entityProcessingOptions, string programName, MDMCenterApplication application, MDMCenterModules module);
        EntityOperationResultCollection Create(EntityCollection entities, string programName, MDMCenterApplication application, MDMCenterModules module);

        EntityOperationResult Delete(Entity entity, string programName, MDMCenterApplication application, MDMCenterModules module);
        EntityOperationResultCollection Delete(EntityCollection entities, string programName, MDMCenterApplication application, MDMCenterModules module);

        Boolean EnsureAttributes(Entity entity, AttributeUniqueIdentifier attributeUniqueIdentifier, Boolean loadAttributeModels, CallerContext callerContext);
        Boolean EnsureAttributes(Entity entity, IEnumerable<int> attributeIds, Boolean loadAttributeModels, CallerContext callerContext);        
        Boolean EnsureAttributes(EntityCollection entityCollection, AttributeModelContext attributeModelContext, Boolean loadAttributeModels, CallerContext callerContext);
        Boolean EnsureAttributes(EntityCollection entityCollection, System.Collections.Generic.IEnumerable<int> attributeIds, Boolean loadAttributeModels, CallerContext callerContext);
        Boolean EnsureAttributes(EntityCollection entityCollection, IEnumerable<AttributeUniqueIdentifier> attributeUniqueIdentifiers, Boolean loadAttributeModels, CallerContext callerContext);
        Boolean EnsureEntityData(EntityCollection entities, EntityContext entityContext, CallerContext callerContext);

        Boolean EnsureRelationships(EntityCollection entityCollection, Boolean loadExistingRelationships, Collection<String> relationshipTypeNames, AttributeUniqueIdentifierCollection attributeUniqueIdentifiers, CallerContext callerContext);
        Boolean EnsureRelationships(EntityCollection entityCollection, Boolean loadExistingRelationships, Collection<Int32> relationshipTypeIds, Collection<Int32> attributeIds, AttributeUniqueIdentifierCollection attributeUniqueIdentifiers, CallerContext callerContext);
        
        Boolean EnsureInheritedAttributes(Entity entity, CallerContext callerContext);

        Boolean EnsureEntityHierarchy(Entity entity, EntityHierarchyContext entityHierarchyContext, EntityGetOptions entityGetOptions, CallerContext callerContext);

        Boolean Exists(Entity entity, Int64 entityId, int containerId, MDMCenterApplication application, MDMCenterModules module, Boolean excludeSelfReference = false);

        EntityCollection Get(Collection<Int64> entityIdList, EntityContext entityContext, EntityGetOptions entityGetOptions, CallerContext callerContext);
        EntityCollection Get(Collection<Int64> entityIdList, EntityContext entityContext, MDMCenterApplication application, MDMCenterModules module);
        EntityCollection Get(Collection<Int64> entityIdList, EntityContext entityContext, Boolean loadLatest, MDMCenterApplication application, MDMCenterModules module, Boolean publishEvents = true, Boolean applyAVS = true, Boolean applySecurity = true, int bulkGetBatchSize = 0);
        Entity Get(Int64 entityId, EntityContext entityContext, EntityGetOptions entityGetOptions, CallerContext callerContext);
        Entity Get(Int64 entityId, EntityContext entityContext, MDMCenterApplication application, MDMCenterModules module, Boolean publishEvents = true, Boolean applyAVS = true);
        Entity Get(Int64 entityId, EntityContext entityContext, Boolean loadLatest, MDMCenterApplication application, MDMCenterModules module, Boolean publishEvents = true, Boolean applyAVS = true, Boolean applySecurity = true, Boolean updateCache = false);
        Entity Get(Int64 entityId, int containerId, LocaleEnum locale, Collection<LocaleEnum> dataLocales, MDMCenterApplication application, MDMCenterModules module);
        Entity Get(Int64 entityId, int containerId, LocaleEnum locale, Collection<LocaleEnum> dataLocales, Boolean loadCompleteEntity, MDMCenterApplication application, MDMCenterModules module);

        EntityReadResult Get(EntityScopeCollection entityScopeCollection, EntityGetOptions entityGetOptions, CallerContext callerContext);
        
        Entity GetByExternalId(string externalId, EntityContext entityContext, CallerContext callerContext, Boolean publishEvents = true, Boolean applyAVS = true);

        Collection<Int64> GetChildrenByEntityType(Int64 entityId, int entityTypeId);

        Collection<Int64> GetEntitiesChildrenByEntityTypes(Collection<Int64> entityIds, Collection<Int32> entityTypeIds, Boolean getOnlyChildren);
        Collection<Int64> GetEntitiesChildrenByEntityTypes(Collection<Int64> entityIds, Collection<Int32> entityTypeIds, Boolean getOnlyChildren, Boolean generateEmptyChildren, Collection<int> cloneAttributesIds, ref EntityCollection emptySkusCollection);
        
        Collection<long> GetEntityGuidsMap(Collection<Guid> entityUniqueIdList, CallerContext callerContext);

        Entity GetEntityHierarchy(EntityUniqueIdentifier entityUniqueIdentifier, EntityContext entityContext, EntityGetOptions entityGetOptions, CallerContext callerContext);
        Boolean LoadEntityHierarchy(Entity entity, EntityContext entityContext, EntityGetOptions entityGetOptions, CallerContext callerContext);

        Entity GetModel(Int32 entityTypeId, Int64 categoryId, EntityContext entityContext, CallerContext callerContext);
        Entity GetModel(Int32 entityTypeId, Int64 categoryId, Int64 parentEntityId, EntityContext entityContext, CallerContext callerContext);
        Entity GetModelById(Int32 entityTypeId, Int32 containerId, Int64 categoryId, LocaleEnum locale, Collection<LocaleEnum> dataLocales, CallerContext callerContext);

        EntityOperationResultCollection Process(EntityCollection entities, EntityProcessingOptions entityProcessingOptions, CallerContext callerContext);
        EntityOperationResultCollection Process(EntityCollection entities, EntityProcessingOptions entityProcessingOptions, String programName, MDMCenterApplication application, MDMCenterModules module);
        EntityOperationResultCollection Process(EntityCollection entities, String programName, MDMCenterApplication application, MDMCenterModules module);

        Boolean RemoveEntityCache(EntityCacheInvalidateContextCollection entityCacheInvalidateContexts, CallerContext callerContext);

        EntityOperationResult Update(Entity entity, EntityProcessingOptions entityProcessingOptions, String programName, MDMCenterApplication application, MDMCenterModules module);
        EntityOperationResult Update(Entity entity, String programName, MDMCenterApplication application, MDMCenterModules module);
        EntityOperationResultCollection Update(EntityCollection entities, EntityProcessingOptions entityProcessingOptions, String programName, MDMCenterApplication application, MDMCenterModules module);
        EntityOperationResultCollection Update(EntityCollection entities, String programName, MDMCenterApplication application, MDMCenterModules module);

        Boolean LoadWorkflowDetails(Entity entity, CallerContext callerContext);
        WorkflowInvokableEntityInfoCollection GetWorkflowInvokableEntityIds(Collection<Int64> entityIds, CallerContext callerContext, String workflowShortName = "", String activityLongName = "");
        EntityUniqueIdentifierCollection GetEntityUniqueIdentifiers(Int64 entityId, EntityContextCollection entityContexts, CallerContext callerContext);

        Dictionary<Int32, Int32> GetEntityVariantLevel(Int64 entityId, CallerContext callerContext);
    }
}
