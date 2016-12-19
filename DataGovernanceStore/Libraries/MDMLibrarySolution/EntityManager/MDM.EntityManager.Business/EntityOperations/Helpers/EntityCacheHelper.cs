using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace MDM.EntityManager.Business.EntityOperations.Helpers
{
    using Core;
    using BusinessObjects;
    using Interfaces;
    using CacheManager.Business;
    using Utility;
    using MDM.BufferManager;

    /// <summary>
    /// 
    /// </summary>
    internal sealed class EntityCacheHelper
    {
        #region Fields
        #endregion

        #region Constructors
        #endregion

        #region Properties
        #endregion

        #region Methods

        #region Public Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityCacheInvalidateContexts"></param>
        /// <param name="entityManager"></param>
        /// <param name="callerContext"></param>
        /// <returns></returns>
        public static Boolean RemoveEntityCache(EntityCacheInvalidateContextCollection entityCacheInvalidateContexts, IEntityManager entityManager, CallerContext callerContext)
        {
            Boolean success = false;

            if (entityCacheInvalidateContexts != null && entityCacheInvalidateContexts.Count > 0)
            {
                success = true;
                var entityCacheStatusCollection = new EntityCacheStatusCollection();
                var containerEntitiesMapping = new Dictionary<Int32, Collection<Int64>>();
                foreach (EntityCacheInvalidateContext entityCacheInvalidateContext in entityCacheInvalidateContexts)
                {
                    var entityCacheStatus = new EntityCacheStatus();
                    entityCacheStatus.EntityId = entityCacheInvalidateContext.EntityId;
                    entityCacheStatus.ContainerId = entityCacheInvalidateContext.ContainerId;
                    entityCacheStatus.Locale = entityCacheInvalidateContext.Locale;
                    entityCacheStatus.MarkAllCacheDirty();
                    entityCacheStatusCollection.Add(entityCacheStatus);

                    //Check and put it in the appropriate container

                    Collection<Int64> entitiesList;
                    if (containerEntitiesMapping.TryGetValue(entityCacheInvalidateContext.ContainerId, out entitiesList))
                    {
                        entitiesList.Add(entityCacheInvalidateContext.EntityId);
                        containerEntitiesMapping[entityCacheInvalidateContext.ContainerId] = entitiesList;
                    }
                    else
                    {
                        entitiesList = new Collection<Int64>();
                        entitiesList.Add(entityCacheInvalidateContext.EntityId);
                        containerEntitiesMapping.Add(entityCacheInvalidateContext.ContainerId, entitiesList);
                    }
                }

                #region Remove Entity Map Cache

                foreach (var keyvalue in containerEntitiesMapping)
                {
                    var entityContext = new EntityContext();
                    entityContext.LoadEntityProperties = true;
                    entityContext.ContainerId = keyvalue.Key;

                    Collection<Int64> entityListPerContainer = keyvalue.Value;
                    EntityCollection entities = entityManager.Get(entityListPerContainer, entityContext, false, callerContext.Application, callerContext.Module, false, false, false);

                    if (entities != null && entities.Count > 0)
                    {
                        var entityBufferManager = new EntityBufferManager();

                        foreach (Entity entity in entities)
                        {
                            entityBufferManager.RemoveEntityMap(entity);
                        }
                    }
                }

                #endregion

                var entityCacheStatusBL = new EntityCacheStatusBL();
                success = entityCacheStatusBL.Process(entityCacheStatusCollection, callerContext);
            }

            return success;
        }

        /// <summary>
        /// </summary>
        /// <param name="entityIdList"></param>
        /// <param name="containerId"></param>
        /// <param name="callerContext"></param>
        /// <returns></returns>
        public static EntityCacheStatusCollection GetBaseEntityCacheStatusCollection(IEnumerable<Int64> entityIdList, Int32 containerId, CallerContext callerContext)
        {
            var baseEntityCacheStatusLoadRequestCollection = new Collection<EntityCacheStatusLoadRequest>();

            foreach (Int64 entityId in entityIdList)
            {
                var entityCacheStatusLoadRequest = new EntityCacheStatusLoadRequest
                {
                    EntityId = entityId,
                    ContainerId = containerId
                };

                baseEntityCacheStatusLoadRequestCollection.Add(entityCacheStatusLoadRequest);
            }

            EntityCacheStatusCollection baseEntityCacheStatusCollection = GetEntityCacheStatus(baseEntityCacheStatusLoadRequestCollection, callerContext);

            return baseEntityCacheStatusCollection;
        }

        /// <summary>
        /// </summary>
        /// <param name="entityCollection"></param>
        /// <param name="baseEntityCacheStatusCollection"></param>
        /// <param name="callerContext"></param>
        /// <returns></returns>
        public static EntityCacheStatusCollection GetEntityCacheStatusCollectionIncludingParents(EntityCollection entityCollection, EntityCacheStatusCollection baseEntityCacheStatusCollection, CallerContext callerContext)
        {
            var entityCacheStatusLoadRequestCollection = new Collection<EntityCacheStatusLoadRequest>();
            var entityCacheStatusCollection = new EntityCacheStatusCollection();

            foreach (Entity entity in entityCollection)
            {
                EntityCacheStatus baseEntityCacheStatus = baseEntityCacheStatusCollection.GetEntityCacheStatus(entity.Id, entity.ContainerId);
                if (baseEntityCacheStatus != null)
                {
                    // If Inherited attribute cache is already dirty, no need to check if entity's parents are in activity log.
                    if (baseEntityCacheStatus.IsInheritedAttributesCacheDirty)
                    {
                        entityCacheStatusCollection.Add(baseEntityCacheStatus);
                    }
                    else
                    {
                        // Check if any parent entity is in activity log.
                        var entityCacheStatusLoadRequest = new EntityCacheStatusLoadRequest
                        {
                            EntityId = entity.Id,
                            ContainerId = entity.ContainerId,
                            ParentEntityTreeIdList = entity.EntityTreeIdList
                        };

                        entityCacheStatusLoadRequestCollection.Add(entityCacheStatusLoadRequest);
                    }
                }
            }

            if (entityCacheStatusLoadRequestCollection.Count > 0)
            {
                EntityCacheStatusCollection modifiedEntityCacheStatusCollection = GetEntityCacheStatus(entityCacheStatusLoadRequestCollection, callerContext);

                // If Base entity cache was not dirty and the data was unavailable in cache, the IsBaseEntityCacheDirty has to be reloaded.
                foreach (EntityCacheStatus entityCacheStatus in modifiedEntityCacheStatusCollection)
                {
                    EntityCacheStatus baseEntityCacheStatus = baseEntityCacheStatusCollection.GetEntityCacheStatus(entityCacheStatus.EntityId, entityCacheStatus.ContainerId);
                    if (baseEntityCacheStatus != null && baseEntityCacheStatus.IsCacheStatusUpdated)
                    {
                        entityCacheStatus.IsBaseEntityCacheDirty = baseEntityCacheStatus.IsBaseEntityCacheDirty;
                    }
                }

                entityCacheStatusCollection.AddRange(modifiedEntityCacheStatusCollection);
            }

            return entityCacheStatusCollection;
        }

        /// <summary>
        ///     Returns the cache status for the specified request.
        /// </summary>
        /// <param name="entityCacheStatusLoadRequest"></param>
        /// <param name="callerContext"></param>
        /// <returns></returns>
        public static EntityCacheStatusCollection GetEntityCacheStatus(Collection<EntityCacheStatusLoadRequest> entityCacheStatusLoadRequest, CallerContext callerContext)
        {
            var durationHelper = new DurationHelper(DateTime.Now);

            var entityCacheStatusManager = new EntityCacheStatusBL();
            EntityCacheStatusCollection entityCacheStatusCollection = entityCacheStatusManager.GetEntityCacheStatusCollection(entityCacheStatusLoadRequest, callerContext);

            return entityCacheStatusCollection;
        }

        /// <summary>
        ///     Updates the cache of the current entity
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="callerContext"></param>
        /// <param name="entityCacheStatusCollection"></param>
        /// <param name="updateAttributeCache"></param>
        /// <returns></returns>
        public static Boolean UpdateCurrentEntityCache(EntityCollection entities, CallerContext callerContext, EntityCacheStatusCollection entityCacheStatusCollection, Boolean updateAttributeCache = false)
        {
            EntityBufferManager entityBufferManager = new EntityBufferManager();

            Boolean successFlag = true;

            EntityCacheStatus entityCacheStatus = null;

            if (entities != null && entities.Count > 0)
            {
                Dictionary<Int32, Collection<LocaleEnum>> hierachyIdBasedDictionary = new Dictionary<Int32, Collection<LocaleEnum>>();                

                foreach (Entity entity in entities)
                {
                    if (entity.Action != ObjectAction.Read)
                    {
                        entityCacheStatus = entityCacheStatusCollection.GetOrCreateEntityCacheStatus(entity.Id, entity.ContainerId);

                        IEntityChangeContext entityChangeContext = entity.GetChangeContext();

                        //Why we don't have update cache for create action?
                        if (entity.Action == ObjectAction.Reclassify || entity.Action == ObjectAction.Rename || entity.Action == ObjectAction.ReParent || entity.Action == ObjectAction.Delete)
                        {
                            //Mark all components as Dirty...
                            entityCacheStatus.MarkAllCacheDirty();
                            
                            entityBufferManager.RemoveEntityMap(entity);
                        }
                        else if (entity.Action == ObjectAction.Update)
                        {
                            if ( entity.OriginalEntity != null && entity.LongName != entity.OriginalEntity.LongName) //TODO :: Is this proper to put OriginalEntity null check here?
                            {
                                entityCacheStatus.IsBaseEntityCacheDirty = true;
                            }

                            if (entityChangeContext.IsAttributesChanged)
                            {
                                entityCacheStatus.IsOverriddenAttributesCacheDirty = true;
                                entityCacheStatus.IsInheritedAttributesCacheDirty = IsOverriddenAttributeCreatedOrDeleted(entity);
                                successFlag = true;
                            }

                            if (entityChangeContext.IsRelationshipsChanged)
                            {
                                UpdateRelationshipsCache(entity);
                            }
                        }
                        else if (entity.Action == ObjectAction.Create)
                        {
                            entityCacheStatus.MarkAllCacheDirty();                           
                        }                        
                        if (entity.BranchLevel == ContainerBranchLevel.Node || entity.EntityTypeId == 6)
                        {
                            Collection<LocaleEnum> dataLocales = null;
                            Int32 hierarchyId = entity.ContainerId;

                            hierachyIdBasedDictionary.TryGetValue(hierarchyId, out dataLocales);

                            if (dataLocales == null)
                            {
                                hierachyIdBasedDictionary.Add(hierarchyId, new Collection<LocaleEnum>() { entity.Locale });
                            }
                            else
                            {
                                dataLocales.Add(entity.Locale);
                            }
                        }
                    }                    
                }
                if (hierachyIdBasedDictionary.Count > 0)
                {
                    CategoryBufferManager categoryBufferManager = new CategoryBufferManager();
                    foreach (Int32 hierarchyId in hierachyIdBasedDictionary.Keys)
                    {
                        successFlag = (categoryBufferManager.RemoveBaseCategories(hierarchyId)
                         && categoryBufferManager.RemoveCategoryLocaleProperties(hierachyIdBasedDictionary[hierarchyId], hierarchyId));
                    }
                }
            }
            return successFlag;
        }

        /// <summary>
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="entityCacheStatus"></param>
        /// <param name="updateCollectionAttributeCache"></param>
        /// <returns></returns>
        public static Boolean UpdateAttributesCache(Entity entity, EntityCacheStatus entityCacheStatus, Boolean updateCollectionAttributeCache = false)
        {
            var entityBufferManager = new EntityBufferManager();

            AttributeCollection cachedLocalAttributeCollection = entityBufferManager.FindLocalAttributes(entity);

            AttributeCollection cachedInheritedAttributeCollection = entityBufferManager.FindInheritedAttributes(entity);

            if (cachedLocalAttributeCollection == null)
                cachedLocalAttributeCollection = new AttributeCollection();

            if (cachedInheritedAttributeCollection == null)
                cachedInheritedAttributeCollection = new AttributeCollection();

            foreach (Attribute attribute in entity.Attributes)
            {
                //Considering inherited attributes also on Entity Create if available.. These attributes will be available as a part of Entity Object if inherited values load is requested for BR execution.
                if (attribute.Action == ObjectAction.Create || attribute.Action == ObjectAction.Update || attribute.Action == ObjectAction.Delete ||
                    (entity.Action == ObjectAction.Create && attribute.SourceFlag == AttributeValueSource.Inherited && attribute.Action == ObjectAction.Read))
                {
                    var cachedLocalAttribute = (Attribute)cachedLocalAttributeCollection.GetAttribute(attribute.Id, attribute.Locale);
                    var cachedInheritedAttribute = (Attribute)cachedInheritedAttributeCollection.GetAttribute(attribute.Id, attribute.Locale);

                    //Remove local attribute in cached collection if found..
                    if (cachedLocalAttribute != null)
                    {
                        cachedLocalAttributeCollection.Remove(cachedLocalAttribute);
                    }
                    //Remove inherited attribute from cached collection
                    else if (cachedInheritedAttribute != null)
                    {
                        cachedInheritedAttributeCollection.Remove(cachedInheritedAttribute);
                    }

                    #region Place attributes back into cache

                    if (attribute.Action != ObjectAction.Delete && !attribute.IsComplex && (!attribute.IsCollection || updateCollectionAttributeCache))
                    {
                        Boolean isAttributeCachable = true;

                        //As a part of entity process, we would only cache attributes having PK already provided
                        //Make sure all values are having PK
                        IValueCollection values = attribute.GetCurrentValuesInvariant();

                        if (values != null && values.Count > 0)
                        {
                            foreach (Value val in values)
                            {
                                if (val.Id < 1)
                                {
                                    isAttributeCachable = false;
                                    break;
                                }
                            }

                            if (isAttributeCachable)
                            {
                                // Add attribute to cache..
                                if (attribute.SourceFlag == AttributeValueSource.Overridden)
                                {
                                    cachedLocalAttributeCollection.Add(attribute);
                                }
                                else if (attribute.SourceFlag == AttributeValueSource.Inherited)
                                {
                                    cachedInheritedAttributeCollection.Add(attribute);
                                }
                            }
                        }
                    }

                    #endregion
                }
            }

            // Replace local attributes into cache
            entityBufferManager.UpdateLocalAttributes(entity, cachedLocalAttributeCollection);

            // Replace inherited attributes into cache
            entityBufferManager.UpdateInheritedAttributes(entity, cachedInheritedAttributeCollection);

            return true;
        }

        /// <summary>
        ///     Checks if any existing overridden attribute is updated to Inherited value or vice versa.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static Boolean IsOverriddenAttributeCreatedOrDeleted(Entity entity)
        {
            foreach (Attribute attribute in entity.Attributes)
            {
                if (attribute.Action == ObjectAction.Delete || attribute.Action == ObjectAction.Create)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static Boolean UpdateRelationshipsCache(Entity entity)
        {
            Boolean successFlag = true;

            //TODO:: currently relationships cache is not enabled

            //_entityBufferManager.UpdateRelationships(entity, entity.Relationships);

            return successFlag;
        }

        #endregion

        #region Private Methods
        
        #endregion
        
        #endregion
    }
}
