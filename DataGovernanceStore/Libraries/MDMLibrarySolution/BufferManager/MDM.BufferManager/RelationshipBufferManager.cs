using System;
using System.Collections.ObjectModel;

namespace MDM.BufferManager
{
    using MDM.BusinessObjects;
    using MDM.CacheManager.Business;
    using MDM.Core;
    using MDM.Utility;

    /// <summary>
    /// Specifies relationship buffer manager
    /// </summary>
    public class RelationshipBufferManager : BusinessLogicBase
    {
        #region Fields

        /// <summary>
        /// Field denoting cache manager of cache
        /// </summary>
        private IDistributedCache _cacheManager = null;

        /// <summary>
        /// Field denoting relationship cache is enabled or not.
        /// </summary>
        private Boolean _isRelationshipCacheEnabled = false;

        /// <summary>
        /// Field denoting relationship ProtoBuf serialization is enabled or not.
        /// </summary>
        private Boolean _isRelationshipProtoBufSerializationEnabled = true;

        #endregion

        #region Properties
        #endregion

        #region Constructors

        /// <summary>
        /// Instantiate Relationship Buffer Manager
        /// </summary>
        public RelationshipBufferManager()
        {
            try
            {
                // Get AppConfig which specify whether cache is enabled for entity or not
                _isRelationshipCacheEnabled = AppConfigurationHelper.GetAppConfig<Boolean>("MDMCenter.RelationshipManager.RelationshipCache.Enabled", true);

                // Get AppConfig which specify whether entity ProtoBufSerialization is enabled for relationships or not
                this._isRelationshipProtoBufSerializationEnabled = AppConfigurationHelper.GetAppConfig<Boolean>("MDMCenter.EntityManager.EntityCache.ProtoBufSerialization.Enabled", true);
                
                if (_isRelationshipCacheEnabled)
                    _cacheManager = CacheFactory.GetDistributedCache();

                if (_cacheManager == null)
                    this._isRelationshipCacheEnabled = false;
            }
            catch
            {
                this._isRelationshipCacheEnabled = false;
            }
        }

        #endregion

        #region Methods

        #region Public Methods

        /// <summary>
        /// Finds relationship and attributes from cache
        /// </summary>
        /// <param name="entity">Specifies entity details to fetch relationships</param>
        /// <param name="relationshipContext">Specifies relationship context details</param>
        /// <returns>Relationship if found in distributed cache otherwise null</returns>
        public RelationshipCollection FindRelationships(Entity entity, RelationshipContext relationshipContext)
        {
            RelationshipCollection allRelationships = null;

            //Currently, we support return of only All or nothing. We would return null if any of rel type id don't have cache..
            Boolean isAllRelationshipTypeFound = false;

            if (this._isRelationshipCacheEnabled)
            {
                Boolean isRelationshipInheritanceEnabled = AppConfigurationHelper.GetAppConfig<Boolean>("RelationshipManager.Relationship.Inheritance.Enabled", true);

                isAllRelationshipTypeFound = true;
                Int16 level = (isRelationshipInheritanceEnabled) ? relationshipContext.Level : Constants.RELATIONSHIP_LEVEL_ONE;

                foreach (Int32 relationshipTypeId in relationshipContext.RelationshipTypeIdList)
                {
                    RelationshipCollection relationships = null;
                    String cacheKey = CacheKeyGenerator.GetEntityRelationshipsCacheKey(entity.Id, entity.ContainerId, entity.EntityTypeId, relationshipTypeId, relationshipContext.RelationshipParentId, level);

                    if (relationshipContext.LoadSources)
                    {
                        cacheKey = CacheKeyGenerator.GetEntityRelationshipsWithSourcesCacheKey(entity.Id, entity.ContainerId, entity.EntityTypeId, relationshipTypeId, relationshipContext.RelationshipParentId, level);
                    }

                    object relationshipsObj = _cacheManager.Get(cacheKey);

                    if (relationshipsObj != null)
                    {
                        if (this._isRelationshipProtoBufSerializationEnabled)
                        {
                            if (relationshipsObj is Byte[])
                            {
                                Byte[] relationshipsObjAsByteArray = (Byte[])(relationshipsObj);
                                relationships = ProtoBufSerializationHelper.Deserialize<RelationshipCollection>(relationshipsObjAsByteArray);
                            }
                        }
                        else
                        {
                            if (relationshipsObj is RelationshipCollection)
                            {
                                relationships = (RelationshipCollection)relationshipsObj;
                            }
                        }
                    }
                    else
                    {
                        isAllRelationshipTypeFound = false;
                        break;
                    }

                    if (relationships != null)
                    {
                        if (allRelationships == null || allRelationships.Count < 1)
                        {
                            allRelationships = relationships;
                        }
                        else
                        {
                            if (allRelationships == null)
                                allRelationships = new RelationshipCollection();

                            allRelationships.AddRange(relationships);
                        }
                    }
                }

                //Here we are doing all or nothing call provided relationship types
                if (!isAllRelationshipTypeFound)
                    allRelationships = null;

                if (relationshipContext.LoadRelationshipAttributes)
                {
                    FindRelationshipAttributes(entity, allRelationships, relationshipContext, level);
                }
            }

            return allRelationships;
        }

        /// <summary>
        /// Update relationships into distributed cache. This method updates only relationships in the cache not relationship attributes.
        /// </summary>
        /// <param name="entity">Specifies entity details for which relationships to be update in cache.</param>
        /// <param name="relationships">Specifies relationship object to be update in cache.</param>
        /// <param name="relationshipTypeId">Specifies relationship type id.</param>
        /// <param name="relationshipParentId">Specifies relationship parent id.</param>
        /// <param name="level">Specifies relationship level.</param>
        /// <param name="relationshipContext">Specifies relationship context.</param>
        /// <returns>return cached key for requested entity.</returns>
        public String UpdateRelationships(Entity entity, RelationshipCollection relationships, Int32 relationshipTypeId, Int64 relationshipParentId, Int16 level, RelationshipContext relationshipContext)
        {
            String cacheKey = String.Empty;

            if (this._isRelationshipCacheEnabled)
            {
                if (relationships == null)
                    throw new ArgumentException("Relationships are not available.");

                DateTime expiryTime = DateTime.Now.AddDays(365);

                try
                {
                    cacheKey = CacheKeyGenerator.GetEntityRelationshipsCacheKey(entity.Id, entity.ContainerId, entity.EntityTypeId, relationshipTypeId, relationshipParentId, level);

                    if (relationshipContext.LoadSources)
                    {
                        cacheKey = CacheKeyGenerator.GetEntityRelationshipsWithSourcesCacheKey(entity.Id, entity.ContainerId, entity.EntityTypeId, relationshipTypeId, relationshipParentId, level);
                    }
                    else
                    {
                        this.RemoveRelationshipsWithSources(entity, relationshipContext);
                    }

                    if (this._isRelationshipProtoBufSerializationEnabled)
                    {
                        Byte[] relationshipsObjAsByteArray = ProtoBufSerializationHelper.Serialize<RelationshipCollection>(relationships);
                        _cacheManager.Set(cacheKey, relationshipsObjAsByteArray, expiryTime);
                    }
                    else
                    {
                        _cacheManager.Set(cacheKey, relationships, expiryTime);
                    }
                }
                catch (Exception ex)
                {
                    throw new ApplicationException(String.Format("Not able to update entity relationships cache due to internal error. Error: {0}", ex.Message));
                }
            }

            return cacheKey;
        }

        /// <summary>
        /// Removes relationship with source data and relationship attributes with source data from cache.
        /// </summary>
        /// <param name="entity">Specifies entity id for which relationship to be remove.</param>
        /// <param name="relationshipContext">Specifies relationship context.</param>
        /// <returns>True if object deleted successfully otherwise false.</returns>
        public Boolean RemoveRelationshipsWithSources(Entity entity, RelationshipContext relationshipContext)
        {
            Boolean success = true;

            if (this._isRelationshipCacheEnabled)
            {
                if (relationshipContext != null && relationshipContext.RelationshipTypeIdList != null)
                {
                    Int64 entityId = entity.Id;
                    Int32 containerId = entity.ContainerId;
                    Int32 entityTypeId = entity.EntityTypeId;
                    Int64 relationshipId = relationshipContext.RelationshipParentId;
                    Int16 level = relationshipContext.Level;

                    foreach (Int32 relationshipTypeId in relationshipContext.RelationshipTypeIdList)
                    {
                        String cacheKey = CacheKeyGenerator.GetEntityRelationshipsWithSourcesCacheKey(entityId, containerId, entityTypeId, relationshipTypeId, relationshipId, level);

                        success = success && _cacheManager.Remove(cacheKey);

                        foreach (LocaleEnum dataLocale in relationshipContext.DataLocales)
                        {
                            String relationshipAttributeCacheKey = CacheKeyGenerator.GetEntityRelationshipsAttributesWithSourcesCacheKey(entityId, containerId, entityTypeId, relationshipTypeId, relationshipId, dataLocale);

                            success = success && _cacheManager.Remove(relationshipAttributeCacheKey);
                        }
                    }
                }
            }

            return success;
        }

        /// <summary>
        /// Update relationships attributes into distributed cache. This method updates only relationship attributes in cache.
        /// </summary>
        /// <param name="entity">Specifies entity details for which relationship attributes to be update in cache.</param>
        /// <param name="relationshipAttributes">Specifies relationship attributes object to be update in cache.</param>
        /// <param name="relationshipTypeId">Specifies relationship type id.</param>
        /// <param name="relationshipId">Specifies relationship id.</param>
        /// <param name="locale">Specifies data locale.</param>
        /// <param name="loadSources">Specifies load sources</param>
        /// <returns>return cached key for requested entity.</returns>
        public String UpdateRelationshipAttributes(Entity entity, AttributeCollection relationshipAttributes, Int32 relationshipTypeId, Int64 relationshipId, LocaleEnum locale, Boolean loadSources = false)
        {
            String cacheKey = String.Empty;

            if (this._isRelationshipCacheEnabled)
            {
                if (relationshipAttributes == null)
                    throw new ArgumentException("Relationships attributes are not available.");

                DateTime expiryTime = DateTime.Now.AddDays(365);

                try
                {
                    cacheKey = CacheKeyGenerator.GetEntityRelationshipsAttributesCacheKey(entity.Id, entity.ContainerId, entity.EntityTypeId, relationshipTypeId, relationshipId, locale);

                    if (loadSources)
                    {
                        cacheKey = CacheKeyGenerator.GetEntityRelationshipsAttributesWithSourcesCacheKey(entity.Id, entity.ContainerId, entity.EntityTypeId, relationshipTypeId, relationshipId, locale);
                    }
                    else
                    {
                        this.RemoveRelationshipAttributesWithSources(entity, relationshipTypeId, relationshipId, locale);
                    }

                    if (this._isRelationshipProtoBufSerializationEnabled)
                    {
                        Byte[] relationshipAttributesObjAsByteArray = ProtoBufSerializationHelper.Serialize<AttributeCollection>(relationshipAttributes);
                        _cacheManager.Set(cacheKey, relationshipAttributesObjAsByteArray, expiryTime);
                    }
                    else
                    {
                        _cacheManager.Set(cacheKey, relationshipAttributes, expiryTime);
                    }
                }
                catch (Exception ex)
                {
                    throw new ApplicationException(String.Format("Not able to update entity relationships attributes cache due to internal error. Error: {0}", ex.Message));
                }
            }

            return cacheKey;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="relationshipAttributes"></param>
        /// <param name="relationshipTypeId"></param>
        /// <param name="relationshipParentId"></param>
        /// <param name="locale"></param>
        public void UpdateRelationshipAttributesWithSource(Entity entity, AttributeCollection relationshipAttributes, Int32 relationshipTypeId, Int64 relationshipParentId, LocaleEnum locale)
        {
            this.UpdateRelationshipAttributes(entity, relationshipAttributes, relationshipTypeId, relationshipParentId, locale, true);
        }

        /// <summary>
        /// Removes relationship and relationship attributes from cache.
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="relationshipTypeId"></param>
        /// <param name="relationshipId"></param>
        /// <param name="locale"></param>
        /// <returns>True if object deleted successfully otherwise false.</returns>
        public Boolean RemoveRelationshipAttributesWithSources(Entity entity, Int32 relationshipTypeId, Int64 relationshipId, LocaleEnum locale)
        {
            Boolean success = true;

            if (this._isRelationshipCacheEnabled)
            {
                String cacheKey = CacheKeyGenerator.GetEntityRelationshipsAttributesWithSourcesCacheKey(entity.Id, entity.ContainerId, entity.EntityTypeId, relationshipTypeId, relationshipId, locale);

                success = _cacheManager.Remove(cacheKey);
            }

            return success;
        }

        /// <summary>
        /// Update all cache keys for all level for given entity.
        /// </summary>
        /// <param name="entity">Specifies entity details.</param>
        /// <param name="cacheKeys">Specifies collection of cached key for all level to be update in cache.</param>
        public void UpdateCacheKeysForAllLevel(Entity entity, Collection<String> cacheKeys)
        {
            if (this._isRelationshipCacheEnabled && entity != null)
            {
                try
                {
                    if (cacheKeys != null && cacheKeys.Count > 0)
                    {
                        String cacheKey = CacheKeyGenerator.GetEntityRelationshipsCacheKeyForAllLevel(entity.Id, entity.ContainerId, entity.EntityTypeId);
                        DateTime expiryTime = DateTime.Now.AddDays(365);

                        _cacheManager.Set(cacheKey, ValueTypeHelper.JoinCollection<String>(cacheKeys, ","), expiryTime);
                    }
                }
                catch (Exception ex)
                {
                    throw new ApplicationException(String.Format("Not able to update relationship cache keys for all level in cache due to internal error for entity id: {0}. Error: {1}", entity.Id, ex.Message));
                }
            }
        }

        /// <summary>
        /// Remove all cached keys for given entity.
        /// </summary>
        /// <param name="entity">Specifies entity details for which cached keys to be remove.</param>
        /// <returns>True if object deleted successfully otherwise false.</returns>
        public Boolean RemoveCacheKeysForAllLevel(Entity entity)
        {
            Boolean success = true;

            if (this._isRelationshipCacheEnabled && entity != null)
            {
                String cacheKey = CacheKeyGenerator.GetEntityRelationshipsCacheKeyForAllLevel(entity.Id, entity.ContainerId, entity.EntityTypeId);

                object cacheKeysAllLevel = _cacheManager.Get(cacheKey);

                if (cacheKeysAllLevel != null && cacheKeysAllLevel is String)
                {
                    Collection<String> cacheKeys = ValueTypeHelper.SplitStringToStringCollection(cacheKeysAllLevel.ToString(), ',');

                    if (cacheKeys != null && cacheKeys.Count > 0)
                    {
                        foreach (String key in cacheKeys)
                        {
                            success = success && _cacheManager.Remove(key);
                        }
                    }
                }

                success = success && _cacheManager.Remove(cacheKey);
            }

            return success;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="relationships"></param>
        /// <param name="relationshipContext"></param>
        /// <param name="maxLevel"></param>
        private void FindRelationshipAttributes(Entity entity, RelationshipCollection relationships, RelationshipContext relationshipContext, Int32 maxLevel)
        {
            if (this._isRelationshipCacheEnabled && relationshipContext != null && relationships != null && relationships.Count > 0)
            {
                foreach (Relationship relationship in relationships)
                {
                    //Populate relationship attributes only for requested level.
                    if (relationship.Level <= maxLevel)
                    {
                        var allAttributes = new AttributeCollection();

                        foreach (LocaleEnum locale in relationshipContext.DataLocales)
                        {
                            AttributeCollection relationshipAttributes = null;
                            String cacheKey = CacheKeyGenerator.GetEntityRelationshipsAttributesCacheKey(entity.Id, entity.ContainerId, entity.EntityTypeId, relationship.RelationshipTypeId, relationship.Id, locale);

                            if (relationshipContext.LoadSources)
                            {
                                cacheKey = CacheKeyGenerator.GetEntityRelationshipsAttributesWithSourcesCacheKey(entity.Id, entity.ContainerId, entity.EntityTypeId, relationship.RelationshipTypeId, relationship.Id, locale);
                            }

                            object relationshipAttributesObj = _cacheManager.Get(cacheKey);

                            if (this._isRelationshipProtoBufSerializationEnabled)
                            {
                                if (relationshipAttributesObj is Byte[])
                                {
                                    Byte[] relationshipsAttributesObjAsByteArray = (Byte[])(relationshipAttributesObj);
                                    relationshipAttributes = ProtoBufSerializationHelper.Deserialize<AttributeCollection>(relationshipsAttributesObjAsByteArray);
                                }
                            }
                            else
                            {
                                if (relationshipAttributesObj is AttributeCollection)
                                {
                                    relationshipAttributes = (AttributeCollection)relationshipAttributesObj;
                                }
                            }

                            if (relationshipAttributes != null)
                            {
                                allAttributes.AddRange(relationshipAttributes);
                            }
                        }

                        relationship.RelationshipAttributes = allAttributes;
                    }

                    FindRelationshipAttributes(entity, relationship.RelationshipCollection, relationshipContext, maxLevel);
                }
            }
        }

        #endregion

        #endregion
    }
}