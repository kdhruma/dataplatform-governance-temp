using System;
using System.Collections.ObjectModel;

namespace MDM.BufferManager
{
    using MDM.BusinessObjects;
    using MDM.CacheManager.Business;
    using MDM.Core;
    using MDM.Interfaces;
    using MDM.Utility;

    /// <summary>
    /// Specifies entity buffer manager
    /// </summary>
    public class EntityBufferManager : BusinessLogicBase
    {
        #region Fields

        private IDistributedCache _cacheManager = null;

        private Boolean _isEntityCacheEnabled = false;

        private Boolean _isEntityHierarchyRelationshipCacheEnabled;

        private Boolean _isEntityExtensionRelationshipCacheEnabled;

        private Boolean _isEntityProtoBufSerializationEnabled = true;

        /// <summary>
        /// Field specifies the entity cache expiration duration in days
        /// </summary>
        private Int32 _entityCacheExpirationDurationInDays = 0;

        #endregion

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public Boolean IsEntityCacheEnabled
        {
            get
            {
                return _isEntityCacheEnabled;
            }
        }

        /// <summary>
        /// Defines EntityProtoBufSerialization is enabled or not
        /// </summary>
        public Boolean IsEntityProtoBufSerializationEnabled
        {
            get
            {
                return _isEntityProtoBufSerializationEnabled;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Default Constructor for entity buffer manager
        /// </summary>
        public EntityBufferManager()
        {
            try
            {
                var isEntityCacheEnabled = AppConfigurationHelper.GetAppConfig<Boolean>("MDMCenter.EntityManager.EntityCache.Enabled", false);
                this._isEntityCacheEnabled = isEntityCacheEnabled;

                this._isEntityHierarchyRelationshipCacheEnabled = AppConfigurationHelper.GetAppConfig<Boolean>("MDMCenter.EntityManager.EntityHierarchyRelationshipCache.Enabled", isEntityCacheEnabled);
                this._isEntityExtensionRelationshipCacheEnabled = AppConfigurationHelper.GetAppConfig<Boolean>("MDMCenter.EntityManager.EntityExtensionRelationshipCache.Enabled", isEntityCacheEnabled);

                this._isEntityProtoBufSerializationEnabled = AppConfigurationHelper.GetAppConfig<Boolean>("MDMCenter.EntityManager.EntityCache.ProtoBufSerialization.Enabled", true);

                if (_isEntityCacheEnabled)
                {
                    _cacheManager = CacheFactory.GetDistributedCache();
                    _entityCacheExpirationDurationInDays = AppConfigurationHelper.GetAppConfig<Int32>("MDMCenter.EntityManager.EntityCache.ExpirationDurationInDays", 7);
                }

                if (_cacheManager == null)
                {
                    this._isEntityCacheEnabled = false;
                }
            }
            catch
            {
                this._isEntityCacheEnabled = false;
                //TODO:: HANDLE CACHE DISABLE SCENARIOS
            }
        }

        #endregion
        
        #region Methods

        #region Public Methods

        #region Full Entity Object

        /// <summary>
        /// Removes whole entity from cache, including all Attribute.
        /// This method does not remove relationship and relationship attributes from cache.Please use DataService.RemoveEntityCache API.
        /// </summary>
        /// <param name="entity">Entity to be removed from cache</param>
        /// <param name="localeCollection">Indicates available locale collection</param>
        /// <returns>Flag for the status of the operation</returns>
        public Boolean RemoveEntity(Entity entity, Collection<Locale> localeCollection)
        {
            Boolean success = true;

            if (localeCollection != null && localeCollection.Count > 0)
            {
                foreach (Locale locale in localeCollection)
                {
                    success = success & RemoveEntityData(entity.Id, entity.ContainerId, entity.EntityTypeId, locale.Locale);
                    success = success & this.RemoveBaseEntity(entity);
                    success = success & this.RemoveEntityMap(entity);
                }
            }

            return success;
        }

        /// <summary>
        /// Removes whole entity from cache, including all Attribute.
        /// This method does not remove relationship and relationship attributes from cache.Please use DataService.RemoveEntityCache API.
        /// </summary>
        /// <param name="entity">Entity to be removed from cache</param>
        /// <param name="localeEnumCollection">Indicates available locale collection</param>
        /// <returns>Flag for the status of the operation</returns>
        public Boolean RemoveEntity(Entity entity, Collection<LocaleEnum> localeEnumCollection)
        {
            Boolean success = true;

            if (localeEnumCollection != null && localeEnumCollection.Count > 0)
            {
                foreach (LocaleEnum localeEnum in localeEnumCollection)
                {
                    success = success & RemoveEntityData(entity.Id, entity.ContainerId, entity.EntityTypeId, localeEnum);
                    success = success & this.RemoveBaseEntity(entity);
                    success = success & this.RemoveEntityMap(entity);
                }
            }

            return success;
        }

        /// <summary>
        /// Removes whole entity from cache, including all Attributes.
        /// This method does not remove relationship and relationship attributes from cache.Please use DataService.RemoveEntityCache API.
        /// </summary>
        /// <param name="entities">Entitities to be removed from cache</param>
        /// <param name="localeCollection">indicates available locale collection</param>
        /// <returns>Flag for the status of the operation</returns>
        public Boolean RemoveEntities(EntityCollection entities, Collection<Locale> localeCollection)
        {
            Boolean success = true;

            if (entities != null && entities.Count > 0)
            {
                foreach (Entity entity in entities)
                {
                    success = success & RemoveEntity(entity, localeCollection);
                }
            }

            return success;
        }

        /// <summary>
        /// Clear the cache data for entity.
        /// This method does not remove relationships and relationships attributes from distributed cache.Please use DataService.RemoveEntityCache API.
        /// </summary>
        /// <param name="entityId">Indicates entityId</param>
        /// <param name="containerId">Indicates ContainerId</param>
        /// <param name="entityTypeId">Indicates Id of the type of entity</param>
        /// <param name="locale">Indicates locale</param>
        /// <returns>Returns boolean value </returns>
        public Boolean RemoveEntityData(Int64 entityId, Int32 containerId, Int32 entityTypeId, LocaleEnum locale)
        {
            Boolean success = true;

            success = success & this.RemoveLocalAttributes(entityId, containerId, entityTypeId, locale);
            success = success & this.RemoveInheritedAttributes(entityId, containerId, entityTypeId, locale);
            success = success & this.RemoveHierarchyRelationships(entityId, containerId, entityTypeId);
            success = success & this.RemoveExtensionRelationships(entityId);
            //success = success & relationshipBufferManager.RemoveRelationships(null,null); TODO

            // While updating attribute into cache at that time we are maintaining two different buckets.
            // 1st bucket for localize attribute(s) and 2nd bucket for non-localizable attribute(s).
            // so while removing entity from cache, we need to remove from non-localizable attribute(s) from cache also.
            #region Get non-localizable cache.

            //Get cache key for non-localizable attributes
            success = success & this.RemoveLocalAttributes(entityId, containerId, entityTypeId, LocaleEnum.Neutral);
            success = success & this.RemoveInheritedAttributes(entityId, containerId, entityTypeId, LocaleEnum.Neutral);
            //success = success & relationshipBufferManager.RemoveRelationships(null,null); TODO

            #endregion Get non-localizable cache.

            return success;
        }

        #endregion Full Entity Object

        #region Entity Base object

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityId"></param>
        /// <param name="entityContext"></param>
        /// <returns></returns>
        public Entity FindBaseEntity(Int64 entityId, EntityContext entityContext)
        {
            //TODO::Entity Buffer Manager has to get requested different data fragments and return the merged cloned entity object
            Entity entity = null;

            if (this._isEntityCacheEnabled)
            {
                String cacheKey = CacheKeyGenerator.GetEntityBaseCacheKey(entityId, entityContext.ContainerId, entityContext.EntityTypeId, entityContext.Locale);

                if (entityContext.LoadSources)
                {
                    cacheKey = CacheKeyGenerator.GetEntityBaseWithSourcesCacheKey(entityId, entityContext.ContainerId, entityContext.EntityTypeId, entityContext.Locale);
                }

                object entityObj = _cacheManager.Get(cacheKey);

                if (this._isEntityProtoBufSerializationEnabled)
                {
                    if (entityObj != null && entityObj is Byte[])
                    {
                        Byte[] entityObjAsByteArray = (Byte[])(entityObj);
                        entity = ProtoBufSerializationHelper.Deserialize<Entity>(entityObjAsByteArray);
                    }
                }
                else
                {
                    if (entityObj != null && entityObj is Entity)
                        entity = (Entity)entityObj;
                }

            }
            return entity;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="entityContext"></param>
        /// <param name="cacheWithSources"></param>
        public void UpdateBaseEntity(Entity entity, EntityContext entityContext, Boolean cacheWithSources = false)
        {
            if (this._isEntityCacheEnabled)
            {
                if (entity == null)
                    throw new ArgumentException("entity is null");

                String cacheKey = CacheKeyGenerator.GetEntityBaseCacheKey(entity.Id, entityContext.ContainerId, entityContext.EntityTypeId, entityContext.Locale);

                if (cacheWithSources)
                {
                    cacheKey = CacheKeyGenerator.GetEntityBaseWithSourcesCacheKey(entity.Id, entityContext.ContainerId, entityContext.EntityTypeId, entityContext.Locale);
                }
                else
                {
                    this.RemoveBaseEntityWithSources(entity);
                }

                DateTime expiryTime = DateTime.Now.AddDays(_entityCacheExpirationDurationInDays);

                try
                {
                    if (this._isEntityProtoBufSerializationEnabled)
                    {
                        Byte[] entityObjAsByteArray = ProtoBufSerializationHelper.Serialize<Entity>(entity);
                        _cacheManager.Set(cacheKey, entityObjAsByteArray, expiryTime);
                    }
                    else
                    {
                        _cacheManager.Set(cacheKey, entity, expiryTime);
                    }
                }
                catch (Exception ex)
                { 
                    throw new ApplicationException(String.Format("Not able to update entity cache due to internal error. Error: {0}", ex.Message));
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="entityContext"></param>
        public void UpdateBaseEntityWithSources(Entity entity, EntityContext entityContext)
        {
            this.UpdateBaseEntity(entity, entityContext, true);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public Boolean RemoveBaseEntity(Entity entity)
        {
            Boolean success = false;

            if (this._isEntityCacheEnabled)
            {
                String cacheKey = CacheKeyGenerator.GetEntityBaseCacheKey(entity.Id, entity.ContainerId, entity.EntityTypeId, entity.Locale);

                success = _cacheManager.Remove(cacheKey);

                success = success && this.RemoveBaseEntityWithSources(entity);
            }

            return success;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public Boolean RemoveBaseEntityWithSources(Entity entity)
        {
            Boolean success = false;

            if (this._isEntityCacheEnabled)
            {
                String cacheKey = CacheKeyGenerator.GetEntityBaseWithSourcesCacheKey(entity.Id, entity.ContainerId, entity.EntityTypeId, entity.Locale);

                success = _cacheManager.Remove(cacheKey);
            }

            return success;
        }

        /// <summary>
        /// Remove from the entity cache if we delete entity or reclassify
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public Boolean RemoveEntityMap(Entity entity)
        {
            Boolean success = false;

            if (this._isEntityCacheEnabled)
            {
                String externalId = String.IsNullOrWhiteSpace(entity.ExternalId) ? entity.Name : entity.ExternalId;
                Int64 categoryId = (entity.Id == entity.CategoryId) ? 0 : entity.CategoryId;

                String cacheKeyWithEntityId = CacheKeyGenerator.GetEntityMapCacheKey("Internal", entity.Id.ToString(), entity.ContainerId, entity.EntityTypeId, categoryId);
                String cacheKeyWithExternalId = CacheKeyGenerator.GetEntityMapCacheKey("Internal", externalId, entity.ContainerId, entity.EntityTypeId, categoryId);

                success = _cacheManager.Remove(cacheKeyWithEntityId);
                success = success || _cacheManager.Remove(cacheKeyWithExternalId);
            }

            return success;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityId"></param>
        /// <param name="containerId"></param>
        /// <param name="entityTypeId"></param>
        /// <param name="locale"></param>
        /// <returns></returns>
        public Boolean RemoveBaseEntity(Int64 entityId, Int32 containerId, Int32 entityTypeId, LocaleEnum locale)
        {
            Boolean success = false;

            if (this._isEntityCacheEnabled)
            {
                String cacheKey = CacheKeyGenerator.GetEntityBaseCacheKey(entityId, containerId, entityTypeId, locale);

                success = _cacheManager.Remove(cacheKey);

                success = success && this.RemoveBaseEntityWithSources(entityId, containerId, entityTypeId, locale);
            }

            return success;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityId"></param>
        /// <param name="containerId"></param>
        /// <param name="entityTypeId"></param>
        /// <param name="locale"></param>
        /// <returns></returns>
        public Boolean RemoveBaseEntityWithSources(Int64 entityId, Int32 containerId, Int32 entityTypeId, LocaleEnum locale)
        {
            Boolean success = false;

            if (this._isEntityCacheEnabled)
            {
                String cacheKey = CacheKeyGenerator.GetEntityBaseWithSourcesCacheKey(entityId, containerId, entityTypeId, locale);

                success = _cacheManager.Remove(cacheKey);
            }

            return success;
        }

        #endregion

        #region Entity Local Attributes

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="cacheWithSources"></param>
        /// <returns></returns>
        public AttributeCollection FindLocalAttributes(Entity entity, Boolean cacheWithSources = false)
        {
            AttributeCollection attributeCollection = new AttributeCollection();
            Boolean isAnyCacheFound = false;
            var ignoreDuplicateCheck = false; // Here ,we need to make sure we do dup check while adding attributes into final collection and ignore the onces we are already been in the final collection

            if (this._isEntityCacheEnabled)
            {
                foreach (LocaleEnum locale in entity.EntityContext.DataLocales)
                {
                    String cacheKey = CacheKeyGenerator.GetEntityLocalAttributesCacheKey(entity.Id, entity.ContainerId, entity.EntityTypeId, locale);

                    if (cacheWithSources)
                    {
                        cacheKey = CacheKeyGenerator.GetEntityLocalAttributesWithSourcesCacheKey(entity.Id, entity.ContainerId, entity.EntityTypeId, locale);
                    }

                    object attributeCollectionObj = _cacheManager.Get(cacheKey);

                    if (this._isEntityProtoBufSerializationEnabled)
                    {
                        if (attributeCollectionObj != null && attributeCollectionObj is Byte[])
                        {
                            Byte[] attributeCollectionObjAsByteArray = (Byte[])(attributeCollectionObj);
                            attributeCollection.AddRange(ProtoBufSerializationHelper.Deserialize<AttributeCollection>(attributeCollectionObjAsByteArray), ignoreDuplicateCheck);
                            isAnyCacheFound = true;
                        }
                    }
                    else
                    {
                        if (attributeCollectionObj != null && attributeCollectionObj is AttributeCollection)
                        {
                            attributeCollection.AddRange((IAttributeCollection)attributeCollectionObj, ignoreDuplicateCheck);
                            isAnyCacheFound = true;
                        }
                    }
                }

                #region Get non-localizable cache.

                //Get cache key for non-localizable attributes
                String nlAttrCacheKey = CacheKeyGenerator.GetEntityLocalAttributesCacheKey(entity.Id, entity.ContainerId, entity.EntityTypeId, LocaleEnum.Neutral);

                if (cacheWithSources)
                {
                    nlAttrCacheKey = CacheKeyGenerator.GetEntityLocalAttributesWithSourcesCacheKey(entity.Id, entity.ContainerId, entity.EntityTypeId, LocaleEnum.Neutral);
                }
                //Get cached object
                object nlAttributeCollectionObj = _cacheManager.Get(nlAttrCacheKey);

                if (this._isEntityProtoBufSerializationEnabled)
                {
                    if (nlAttributeCollectionObj != null && nlAttributeCollectionObj is Byte[])
                    {
                        Byte[] nlAttributeCollectionObjAsByteArray = (Byte[])(nlAttributeCollectionObj);
                        attributeCollection.AddRange(ProtoBufSerializationHelper.Deserialize<AttributeCollection>(nlAttributeCollectionObjAsByteArray), ignoreDuplicateCheck);
                        isAnyCacheFound = true;
                    }
                }
                else
                {
                    if (nlAttributeCollectionObj != null && nlAttributeCollectionObj is AttributeCollection)
                    {
                        attributeCollection.AddRange((IAttributeCollection)nlAttributeCollectionObj, ignoreDuplicateCheck);
                        isAnyCacheFound = true;
                    }
                }

                #endregion Get non-localizable cache.
            }

            if (!isAnyCacheFound)
                attributeCollection = null;

            return attributeCollection;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="localAttributesCollection"></param>
        /// <param name="cacheWithSources"></param>
        public void UpdateLocalAttributes(Entity entity, AttributeCollection localAttributesCollection, Boolean cacheWithSources = false)
        {
            if (this._isEntityCacheEnabled)
            {
                if (localAttributesCollection == null)
                    throw new ArgumentException("localAttributesCollection is null");

                //Take each locale from EntityContext.DataLocales, separate Attribute being updated for different locales and cache it.
                foreach (LocaleEnum locale in entity.Attributes.GetLocaleList())
                {
                    //Filter attribute based on locale. This will also include non-localizable attribute
                    AttributeCollection localeBasedAttributes = (AttributeCollection)localAttributesCollection.GetAttributes(locale);

                    //TODO:: Write synchronization logic here...
                    String cacheKey = CacheKeyGenerator.GetEntityLocalAttributesCacheKey(entity.Id, entity.ContainerId, entity.EntityTypeId, locale);
                    if (cacheWithSources)
                    {
                        cacheKey = CacheKeyGenerator.GetEntityLocalAttributesWithSourcesCacheKey(entity.Id, entity.ContainerId, entity.EntityTypeId, locale);
                    }
                    else
                    {
                        this.RemoveLocalAttributesWithSorces(entity.Id, entity.ContainerId, entity.EntityTypeId, locale);
                    }

                    if (this._isEntityProtoBufSerializationEnabled)
                    {
                        Byte[] localeBasedAttributeObjAsByteArray = ProtoBufSerializationHelper.Serialize<AttributeCollection>(localeBasedAttributes);
                        this.SetCache(cacheKey, localeBasedAttributeObjAsByteArray, "Not able to update entity local attributes cache due to internal error. Error: {0}");
                    }
                    else
                    {
                        this.SetCache(cacheKey, localeBasedAttributes, "Not able to update entity local attributes cache due to internal error. Error: {0}");
                    }
                }

                #region Update non-localizable cache.

                //TODO:: Write synchronization logic here...
                String nlAttrCacheKey = CacheKeyGenerator.GetEntityLocalAttributesCacheKey(entity.Id, entity.ContainerId, entity.EntityTypeId, LocaleEnum.Neutral);

                if (cacheWithSources)
                {
                    nlAttrCacheKey = CacheKeyGenerator.GetEntityLocalAttributesWithSourcesCacheKey(entity.Id, entity.ContainerId, entity.EntityTypeId, LocaleEnum.Neutral);
                }

                //Get non-localizable attribute
                AttributeCollection nonLocalizableAttr = (AttributeCollection)localAttributesCollection.GetNonLocalizableAttributes();

                //Put into cache
                if (this._isEntityProtoBufSerializationEnabled)
                {
                    Byte[] nonLocalizableAttrObjAsByteArray = ProtoBufSerializationHelper.Serialize<AttributeCollection>(nonLocalizableAttr);
                    this.SetCache(nlAttrCacheKey, nonLocalizableAttrObjAsByteArray, "Not able to update entity local attributes cache due to internal error. Error: {0}");
                }
                else
                {
                    this.SetCache(nlAttrCacheKey, nonLocalizableAttr, "Not able to update entity local attributes cache due to internal error. Error: {0}");
                }

                #endregion Update non-localizable cache.
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="localAttributesCollection"></param>
        public void UpdateLocalAttributesWithSources(Entity entity, AttributeCollection localAttributesCollection)
        {
            this.UpdateLocalAttributes(entity, localAttributesCollection, true);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityId"></param>
        /// <param name="containerId"></param>
        /// <param name="entityTypeId"></param>
        /// <param name="locale"></param>
        /// <returns></returns>
        public Boolean RemoveLocalAttributes(Int64 entityId, Int32 containerId, Int32 entityTypeId, LocaleEnum locale)
        {
            Boolean success = false;

            if (this._isEntityCacheEnabled)
            {
                String cacheKey = CacheKeyGenerator.GetEntityLocalAttributesCacheKey(entityId, containerId, entityTypeId, locale);
                success = _cacheManager.Remove(cacheKey);
                
                // remove cache with source values
                success = success && this.RemoveLocalAttributesWithSorces(entityId, containerId, entityTypeId, locale);
            }

            return success;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityId"></param>
        /// <param name="containerId"></param>
        /// <param name="entityTypeId"></param>
        /// <param name="locale"></param>
        /// <returns></returns>
        public Boolean RemoveLocalAttributesWithSorces(Int64 entityId, Int32 containerId, Int32 entityTypeId, LocaleEnum locale)
        {
            Boolean success = false;

            if (this._isEntityCacheEnabled)
            {
                String cacheKey = CacheKeyGenerator.GetEntityLocalAttributesWithSourcesCacheKey(entityId, containerId, entityTypeId, locale);
                success = _cacheManager.Remove(cacheKey);
            }

            return success;
        }

        #endregion

        #region Entity Inherited Attributes

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="cacheWithSources"></param>
        /// <returns></returns>
        public AttributeCollection FindInheritedAttributes(Entity entity, Boolean cacheWithSources = false)
        {
            AttributeCollection attributeCollection = new AttributeCollection();
            Boolean isAnyCacheFound = false;
            var ignoreDuplicateCheck = false; // Here ,we need to make sure we do dup check while adding attributes into final collection and ignore the onces we are already been in the final collection

            if (this._isEntityCacheEnabled)
            {
                foreach (LocaleEnum locale in entity.EntityContext.DataLocales)
                {
                    String cacheKey = CacheKeyGenerator.GetEntityInheritedAttributesCacheKey(entity.Id, entity.ContainerId, entity.EntityTypeId, locale);
                    if (cacheWithSources)
                    {
                        cacheKey = CacheKeyGenerator.GetEntityInheritedAttributesWithSourcesCacheKey(entity.Id, entity.ContainerId, entity.EntityTypeId, locale);
                    }

                    object attributeCollectionObj = _cacheManager.Get(cacheKey);

                    if (this._isEntityProtoBufSerializationEnabled)
                    {
                        if (attributeCollectionObj != null && attributeCollectionObj is Byte[])
                        {
                            Byte[] attributeCollectionObjAsByteArray = (Byte[])(attributeCollectionObj);
                            attributeCollection.AddRange(ProtoBufSerializationHelper.Deserialize<AttributeCollection>(attributeCollectionObjAsByteArray), ignoreDuplicateCheck);
                            isAnyCacheFound = true;
                        }
                    }
                    else
                    {
                        if (attributeCollectionObj != null && attributeCollectionObj is AttributeCollection)
                        {
                            attributeCollection.AddRange((IAttributeCollection)attributeCollectionObj, ignoreDuplicateCheck);
                            isAnyCacheFound = true;
                        }
                    }
                }

                #region Get non-localizable cache.

                //Get cache key for non-localizable attributes
                String nlAttrCacheKey = CacheKeyGenerator.GetEntityInheritedAttributesCacheKey(entity.Id, entity.ContainerId, entity.EntityTypeId, LocaleEnum.Neutral);

                if (cacheWithSources)
                {
                    nlAttrCacheKey = CacheKeyGenerator.GetEntityInheritedAttributesWithSourcesCacheKey(entity.Id, entity.ContainerId, entity.EntityTypeId, LocaleEnum.Neutral);
                }

                //Get cached object
                object nlAttributeCollectionObj = _cacheManager.Get(nlAttrCacheKey);

                if (this._isEntityProtoBufSerializationEnabled)
                {
                    if (nlAttributeCollectionObj != null && nlAttributeCollectionObj is Byte[])
                    {
                        Byte[] nlAttributeCollectionObjAsByteArray = (Byte[])(nlAttributeCollectionObj);
                        attributeCollection.AddRange(ProtoBufSerializationHelper.Deserialize<AttributeCollection>(nlAttributeCollectionObjAsByteArray), ignoreDuplicateCheck);
                        isAnyCacheFound = true;
                    }
                }
                else
                {
                    if (nlAttributeCollectionObj != null && nlAttributeCollectionObj is AttributeCollection)
                    {
                        attributeCollection.AddRange((IAttributeCollection)nlAttributeCollectionObj, ignoreDuplicateCheck);
                        isAnyCacheFound = true;
                    }
                }

                #endregion Get non-localizable cache.
            }

            if (!isAnyCacheFound)
                attributeCollection = null;

            return attributeCollection;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="inheritedAttributesCollection"></param>
        /// <param name="cacheWithSources"></param>
        public void UpdateInheritedAttributes(Entity entity, AttributeCollection inheritedAttributesCollection, Boolean cacheWithSources = false)
        {
            if (this._isEntityCacheEnabled)
            {
                if (inheritedAttributesCollection == null)
                    throw new ArgumentException("inheritedAttributesCollection is null");

                //Take each locale from EntityContext.DataLocales, separate Attribute being updated for different locales and cache it.
                foreach (LocaleEnum locale in entity.Attributes.GetLocaleList())
                {
                    //Filter attribute based on locale. This will also include non-localizable attribute
                    AttributeCollection localeBasedAttributes = (AttributeCollection)inheritedAttributesCollection.GetAttributes(locale);

                    //TODO:: Write synchronization logic here...
                    String cacheKey = CacheKeyGenerator.GetEntityInheritedAttributesCacheKey(entity.Id, entity.ContainerId, entity.EntityTypeId, locale);
                    if (cacheWithSources)
                    {
                        cacheKey = CacheKeyGenerator.GetEntityInheritedAttributesWithSourcesCacheKey(entity.Id,
                            entity.ContainerId, entity.EntityTypeId, locale);
                    }
                    else
                    {
                        this.RemoveInheritedAttributesWithSources(entity.Id,
                            entity.ContainerId, entity.EntityTypeId, locale);
                    }

                    if (this._isEntityProtoBufSerializationEnabled)
                    {
                        Byte[] localeBasedAttributeObjAsByteArray = ProtoBufSerializationHelper.Serialize<AttributeCollection>(localeBasedAttributes);
                        this.SetCache(cacheKey, localeBasedAttributeObjAsByteArray, "Not able to update entity inherited attributes cache due to internal error. Error: {0}");
                    }
                    else
                    {
                        this.SetCache(cacheKey, localeBasedAttributes, "Not able to update entity inherited attributes cache due to internal error. Error: {0}");
                    }

                    #region Update non-localizable cache.

                    //TODO:: Write synchronization logic here...
                    String nlAttrCacheKey = CacheKeyGenerator.GetEntityInheritedAttributesCacheKey(entity.Id, entity.ContainerId, entity.EntityTypeId, LocaleEnum.Neutral);
                    if (cacheWithSources)
                    {
                        nlAttrCacheKey = CacheKeyGenerator.GetEntityInheritedAttributesWithSourcesCacheKey(entity.Id, entity.ContainerId, entity.EntityTypeId, locale);
                    }

                    //Get non-localizable attribute
                    AttributeCollection nonLocalizableAttr = (AttributeCollection)inheritedAttributesCollection.GetNonLocalizableAttributes();

                    //Put into cache
                    if (this._isEntityProtoBufSerializationEnabled)
                    {
                        Byte[] nonLocalizableAttrObjAsByteArray = ProtoBufSerializationHelper.Serialize<AttributeCollection>(nonLocalizableAttr);
                        this.SetCache(nlAttrCacheKey, nonLocalizableAttrObjAsByteArray, "Not able to update entity inherited attributes cache due to internal error. Error: {0}");
                    }
                    else
                    {
                        this.SetCache(nlAttrCacheKey, nonLocalizableAttr, "Not able to update entity inherited attributes cache due to internal error. Error: {0}");
                    }

                    #endregion Update non-localizable cache.
                }

            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="inheritedAttributesCollection"></param>
        public void UpdateInheritedAttributesWithSources(Entity entity, AttributeCollection inheritedAttributesCollection)
        {
            this.UpdateInheritedAttributes(entity, inheritedAttributesCollection, true);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityId"></param>
        /// <param name="containerId"></param>
        /// <param name="entityTypeId"></param>
        /// <param name="locale"></param>
        /// <returns></returns>
        public Boolean RemoveInheritedAttributes(Int64 entityId, Int32 containerId, Int32 entityTypeId, LocaleEnum locale)
        {
            Boolean success = false;

            if (this._isEntityCacheEnabled)
            {
                String cacheKey = CacheKeyGenerator.GetEntityInheritedAttributesCacheKey(entityId, containerId, entityTypeId, locale);
                success = _cacheManager.Remove(cacheKey);
  
                //Get cache key for non-localizable attributes
                String nlAttrCacheKey = CacheKeyGenerator.GetEntityInheritedAttributesCacheKey(entityId, containerId, entityTypeId, LocaleEnum.Neutral);
                success = _cacheManager.Remove(nlAttrCacheKey);

                //Remove cache with Source values
                success = success && this.RemoveLocalAttributesWithSorces(entityId, containerId, entityTypeId, locale);
            }

            return success;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityId"></param>
        /// <param name="containerId"></param>
        /// <param name="entityTypeId"></param>
        /// <param name="locale"></param>
        /// <returns></returns>
        public Boolean RemoveInheritedAttributesWithSources(Int64 entityId, Int32 containerId, Int32 entityTypeId, LocaleEnum locale)
        {
            Boolean success = false;

            if (this._isEntityCacheEnabled)
            {
                String cacheKey = CacheKeyGenerator.GetEntityInheritedAttributesWithSourcesCacheKey(entityId, containerId, entityTypeId, locale);
                success = _cacheManager.Remove(cacheKey);
  
                //Get cache key for non-localizable attributes
                String nlAttrCacheKey = CacheKeyGenerator.GetEntityInheritedAttributesWithSourcesCacheKey(entityId, containerId, entityTypeId, LocaleEnum.Neutral);
                success = _cacheManager.Remove(nlAttrCacheKey);
            }

            return success;
        }

        #endregion

        #region Entity Hierarchies

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public HierarchyRelationshipCollection FindHierarchyRelationships(Entity entity)
        {
            return FindHierarchyRelationships(entity.Id, entity.ContainerId, entity.EntityTypeId);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityId"></param>
        /// <param name="containerId"></param>
        /// <param name="entityTypeId"></param>
        /// <returns></returns>
        public HierarchyRelationshipCollection FindHierarchyRelationships(Int64 entityId, Int32 containerId, Int32 entityTypeId)
        {
            HierarchyRelationshipCollection hierarchyRelationshipCollection = null;

            if (this._isEntityCacheEnabled && this._isEntityHierarchyRelationshipCacheEnabled)
            {
                String cacheKey = CacheKeyGenerator.GetEntityHierarchyRelationshipsCacheKey(entityId, containerId, entityTypeId);

                object hierarchyRelationshipCollectionObj = _cacheManager.Get(cacheKey);

                if (this._isEntityProtoBufSerializationEnabled)
                {
                    if (hierarchyRelationshipCollectionObj != null && hierarchyRelationshipCollectionObj is Byte[])
                    {
                        Byte[] hierarchyRelationshipCollectionObjAsByteArray = (Byte[])(hierarchyRelationshipCollectionObj);
                        hierarchyRelationshipCollection = ProtoBufSerializationHelper.Deserialize<HierarchyRelationshipCollection>(hierarchyRelationshipCollectionObjAsByteArray);
                    }
                }
                else
                {
                    if (hierarchyRelationshipCollectionObj != null && hierarchyRelationshipCollectionObj is HierarchyRelationshipCollection)
                        hierarchyRelationshipCollection = (HierarchyRelationshipCollection)hierarchyRelationshipCollectionObj;
                }
            }   

            return hierarchyRelationshipCollection;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="hierarchyRelationshipCollection"></param>
        public void UpdateHierarchyRelationships(Entity entity, HierarchyRelationshipCollection hierarchyRelationshipCollection)
        {
            UpdateHierarchyRelationships(entity.Id, entity.ContainerId, entity.EntityTypeId, hierarchyRelationshipCollection);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityId"></param>
        /// <param name="containerId"></param>
        /// <param name="entityTypeId"></param>
        /// <param name="hierarchyRelationshipCollection"></param>
        public void UpdateHierarchyRelationships(Int64 entityId, Int32 containerId, Int32 entityTypeId, HierarchyRelationshipCollection hierarchyRelationshipCollection)
        {
            if (this._isEntityCacheEnabled && this._isEntityHierarchyRelationshipCacheEnabled)
            {
                if (hierarchyRelationshipCollection == null)
                    throw new ArgumentException("hierarchyRelationshipCollection is null");

                //TODO:: Write synchronization logic here...
                String cacheKey = CacheKeyGenerator.GetEntityHierarchyRelationshipsCacheKey(entityId, containerId, entityTypeId);

                DateTime expiryTime = DateTime.Now.AddDays(_entityCacheExpirationDurationInDays);

                try
                {

                    if (this._isEntityProtoBufSerializationEnabled)
                    {
                        Byte[] hierarchyRelationshipCollectionObjAsByteArray = ProtoBufSerializationHelper.Serialize<HierarchyRelationshipCollection>(hierarchyRelationshipCollection);
                        _cacheManager.Set(cacheKey, hierarchyRelationshipCollectionObjAsByteArray, expiryTime);
                    }
                    else
                    {
                        _cacheManager.Set(cacheKey, hierarchyRelationshipCollection, expiryTime);
                    }
                }
                catch (Exception ex)
                {
                    throw new ApplicationException(String.Format("Not able to update entity hierarchy relationships cache due to internal error. Error: {0}", ex.Message));
                }
            }
        }

        /// <summary>
        /// Removes hierarchy relatinships from cache
        /// </summary>
        /// <param name="entityId"></param>
        /// <param name="containerId"></param>
        /// <param name="entityTypeId"></param>
        /// <returns></returns>
        public Boolean RemoveHierarchyRelationships(Int64 entityId, Int32 containerId, Int32 entityTypeId)
        {
            Boolean success = false;

            if (this._isEntityCacheEnabled && this._isEntityHierarchyRelationshipCacheEnabled)
            {
                String cacheKey = CacheKeyGenerator.GetEntityHierarchyRelationshipsCacheKey(entityId, containerId, entityTypeId);
                success = _cacheManager.Remove(cacheKey);
            }

            return success;
        }

        #endregion

        #region Entity Extensions

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityId"></param>
        /// <returns></returns>
        public ExtensionRelationshipCollection FindExtensionRelationships(Int64 entityId)
        {
            ExtensionRelationshipCollection extensionRelationships = null;

            if (this._isEntityCacheEnabled && this._isEntityExtensionRelationshipCacheEnabled)
            {
                String cacheKey = CacheKeyGenerator.GetEntityExtensionRelationshipsCacheKey(entityId);

                object extensionRelationshipCollectionObj = _cacheManager.Get(cacheKey);

                if (this._isEntityProtoBufSerializationEnabled)
                {
                    if (extensionRelationshipCollectionObj != null && extensionRelationshipCollectionObj is Byte[])
                    {
                        Byte[] extensionRelationshipCollectionObjAsByteArray = (Byte[])(extensionRelationshipCollectionObj);
                        extensionRelationships = ProtoBufSerializationHelper.Deserialize<ExtensionRelationshipCollection>(extensionRelationshipCollectionObjAsByteArray);
                    }
                }
                else
                {
                    if (extensionRelationshipCollectionObj != null && extensionRelationshipCollectionObj is ExtensionRelationshipCollection)
                        extensionRelationships = (ExtensionRelationshipCollection)extensionRelationshipCollectionObj;
                }
            }

            return extensionRelationships;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityId"></param>
        /// <param name="extensionRelationships"></param>
        public void UpdateExtensionRelationships(Int64 entityId, ExtensionRelationshipCollection extensionRelationships)
        {
            if (this._isEntityCacheEnabled && this._isEntityExtensionRelationshipCacheEnabled)
            {
                if (extensionRelationships == null)
                    throw new ArgumentException("extensionRelationships is null");

                String cacheKey = CacheKeyGenerator.GetEntityExtensionRelationshipsCacheKey(entityId);

                DateTime expiryTime = DateTime.Now.AddDays(_entityCacheExpirationDurationInDays);

                try
                {
                    if (this._isEntityProtoBufSerializationEnabled)
                    {
                        Byte[] extensionRelationshipsObjAsByteArray = ProtoBufSerializationHelper.Serialize<ExtensionRelationshipCollection>(extensionRelationships);
                        _cacheManager.Set(cacheKey, extensionRelationshipsObjAsByteArray, expiryTime);
                    }
                    else
                    {
                        _cacheManager.Set(cacheKey, extensionRelationships, expiryTime);
                    }
                }
                catch (Exception ex)
                {
                    throw new ApplicationException(String.Format("Not able to update entity extensions cache due to internal error. Error: {0}", ex.Message));
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityId"></param>
        /// <returns></returns>
        public Boolean RemoveExtensionRelationships(Int64 entityId)
        {
            Boolean success = false;

            if (this._isEntityCacheEnabled && this._isEntityExtensionRelationshipCacheEnabled)
            {
                String cacheKey = CacheKeyGenerator.GetEntityExtensionRelationshipsCacheKey(entityId);

                success = _cacheManager.Remove(cacheKey);
            }

            return success;
        }

        #endregion

        #endregion

        #region Private Methods

        private void SetCache(String cacheKey, Object value, String errorMessage)
        {
            DateTime expiryTime = DateTime.Now.AddDays(_entityCacheExpirationDurationInDays);

            try
            {
                _cacheManager.Set(cacheKey, value, expiryTime);
            }
            catch (Exception ex)
            {
                throw new ApplicationException(String.Format(errorMessage, ex.Message));
            }
        }

        #endregion

        #endregion
    }
}
