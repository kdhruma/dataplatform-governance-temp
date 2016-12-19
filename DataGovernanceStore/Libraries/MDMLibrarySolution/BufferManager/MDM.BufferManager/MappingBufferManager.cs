using System;
using System.Diagnostics;
using System.Collections.ObjectModel;
using System.Collections.Generic;

namespace MDM.BufferManager
{
    using MDM.BusinessObjects;
    using MDM.CacheManager.Business;
    using MDM.ExceptionManager;
    using MDM.Core;
    using MDM.Utility;
    using MDM.Interfaces;
    using MDM.Services;

    /// <summary>
    /// Specifies attribute model mappings buffer manager
    /// </summary>
    public class MappingBufferManager : BusinessLogicBase
    {
        #region Fields

        /// <summary>
        /// field denoting cache manager of cache
        /// </summary>
        private ICache _cacheManager = null;

        /// <summary>
        /// field denoting attribute Model cache is enabled or not.
        /// </summary>
        private Boolean _isMappingCacheEnabled = false;

        /// <summary>
        /// Field denotes whether the Distributed Cache with notification has been enabled.
        /// </summary>
        private Boolean _isDistributedCacheWithNotificationEnabled = false;

        /// <summary>
        /// Field denotes an instance of the CacheSynchronizationHelper.
        /// </summary>
        private CacheSynchronizationHelper _cacheSynchronizationHelper = null;

        /// <summary>
        /// field denoting exclusion context cache is enabled or not.
        /// </summary>
        private Boolean _isExclusionContextCacheEnabled = false;

        #endregion

        #region Properties

        /// <summary>
        /// Property denoting AttributeModel cache is enabled or not.
        /// </summary>
        public Boolean IsMappingCacheEnabled
        {
            get
            {
                return _isMappingCacheEnabled;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Instantiate AttributeModel Buffer Manager
        /// </summary>
        public MappingBufferManager()
        {
            try
            {
                // Get AppConfig which specify whether cache is enabled for entity or not
                this._isMappingCacheEnabled = AppConfigurationHelper.GetAppConfig<Boolean>("MDMCenter.AttributeModelManager.AttributeModelCache.Enabled", true);
                this._isExclusionContextCacheEnabled = AppConfigurationHelper.GetAppConfig<Boolean>("MDMCenter.DataModelExclusionContextManager.ExclusionContextCache.Enabled", true);
                this._isDistributedCacheWithNotificationEnabled = AppConfigurationHelper.GetAppConfig<Boolean>("MDMCenter.DistributedCacheWithNotification.Enabled", false);

                if (_isMappingCacheEnabled)
                {
                    _cacheManager = CacheFactory.GetCache();

                    if (_isDistributedCacheWithNotificationEnabled)
                        _cacheSynchronizationHelper = new CacheSynchronizationHelper();
                }

                if (_cacheManager == null)
                    this._isMappingCacheEnabled = false;
            }
            catch
            {
                this._isMappingCacheEnabled = false;
            }
        }

        #endregion

        #region Methods

        #region Public Methods

        #region EntityType Mappings Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityTypeId"></param>
        /// <returns></returns>
        public EntityTypeAttributeMappingCollection FindEntityTypeAttributeMappings(Int32 entityTypeId)
        {
            EntityTypeAttributeMappingCollection entityTypeAttributeMappings = null;

            if (this._isMappingCacheEnabled)
            {
                String cacheKey = CacheKeyGenerator.GetAttributeModelMappingCacheKey(AttributeModelType.Common, 0, entityTypeId, 0, 0);

                object mappingObj = null;

                try
                {
                    mappingObj = _cacheManager.Get(cacheKey);
                }
                catch (Exception ex)
                {
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, String.Format("Error occurred : {0}", ex.Message));
                    ExceptionHandler exceptionHandler = new ExceptionHandler(ex);
                }

                if (mappingObj != null && mappingObj is EntityTypeAttributeMappingCollection)
                    entityTypeAttributeMappings = (EntityTypeAttributeMappingCollection)mappingObj;
            }

            return entityTypeAttributeMappings;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityTypeAttributeMappings"></param>
        /// <param name="entityTypeId"></param>
        /// <param name="numberOfRetry"></param>
        /// <param name="isDataUpdated"></param>
        public void UpdateEntityTypeAttributeMappings(EntityTypeAttributeMappingCollection entityTypeAttributeMappings, Int32 entityTypeId, Int32 numberOfRetry, Boolean isDataUpdated = false)
        {
            if (this._isMappingCacheEnabled)
            {
                if (entityTypeAttributeMappings == null)
                    throw new ArgumentException("EntityType attribute mappings are empty or not available.");

                String cacheKey = CacheKeyGenerator.GetAttributeModelMappingCacheKey(AttributeModelType.Common, 0, entityTypeId, 0, 0);

                DateTime expiryTime = DateTime.Now.AddDays(5);

                try
                {
                    if (_isDistributedCacheWithNotificationEnabled)
                        _cacheSynchronizationHelper.NotifyLocalCacheInsert(cacheKey, expiryTime, isDataUpdated);

                    _cacheManager.Set(cacheKey, entityTypeAttributeMappings, expiryTime);
                }
                catch (Exception ex)
                {
                    //Retry < 0 means just ignore update if failed..
                    if (numberOfRetry < 0)
                        return;

                    bool retrySuccess = false;

                    for (int i = 0; i < numberOfRetry; i++)
                    {
                        try
                        {
                            if (_isDistributedCacheWithNotificationEnabled)
                                _cacheSynchronizationHelper.NotifyLocalCacheInsert(cacheKey, expiryTime, isDataUpdated);

                            _cacheManager.Set(cacheKey, entityTypeAttributeMappings, expiryTime);

                            retrySuccess = true;
                            break;
                        }
                        catch
                        {
                        }
                    }

                    if (!retrySuccess)
                    {
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, String.Format("Error occurred : {0}", ex.Message));
                        ExceptionHandler exceptionHandler = new ExceptionHandler(ex);
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityTypeId"></param>
        /// <param name="publishCacheChangeEvent"></param>
        /// <returns></returns>
        public Boolean RemoveEntityTypeAttributeMappings(Int32 entityTypeId, Boolean publishCacheChangeEvent)
        {
            Boolean success = false;

            success = RemoveMappings(AttributeModelType.Common, 0, entityTypeId, 0, 0, publishCacheChangeEvent);

            return success;
        }

        #endregion

        #region Container - EntityType Mappings Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="containerId"></param>
        /// <param name="entityTypeId"></param>
        /// <returns></returns>
        public ContainerEntityTypeAttributeMappingCollection FindContainerEntityTypeAttributeMappingsCompleteDetails(Int32 containerId, Int32 entityTypeId)
        {
            ContainerEntityTypeAttributeMappingCollection containerEntityTypeAttributeMappings = null;

            if (this._isMappingCacheEnabled)
            {
                String cacheKey = CacheKeyGenerator.GetAttributeModelMappingCacheKey(AttributeModelType.Common, containerId, entityTypeId, 0, 0);

                object mappingObj = null;

                try
                {
                    mappingObj = _cacheManager.Get(cacheKey);
                }
                catch (Exception ex)
                {
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, String.Format("Error occurred : {0}", ex.Message));
                    ExceptionHandler exceptionHandler = new ExceptionHandler(ex);
                }

                if (mappingObj != null && mappingObj is ContainerEntityTypeAttributeMappingCollection)
                    containerEntityTypeAttributeMappings = (ContainerEntityTypeAttributeMappingCollection)mappingObj;
            }

            return containerEntityTypeAttributeMappings;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="containerId"></param>
        /// <param name="entityTypeId"></param>
        /// <returns></returns>
        public AttributeModelMappingPropertiesCollection FindContainerEntityTypeAttributeMappingsPartialDetails(Int32 containerId, Int32 entityTypeId)
        {
            AttributeModelMappingPropertiesCollection attributeModelMappingProperties = null;

            if (this._isMappingCacheEnabled)
            {
                String cacheKey = CacheKeyGenerator.GetAttributeModelMappingCacheKey(AttributeModelType.Common, containerId, entityTypeId, 0, 0, false);

                object mappingObj = null;

                try
                {
                    mappingObj = _cacheManager.Get(cacheKey);
                }
                catch (Exception ex)
                {
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, String.Format("Error occurred : {0}", ex.Message));
                    ExceptionHandler exceptionHandler = new ExceptionHandler(ex);
                }

                if (mappingObj != null && mappingObj is AttributeModelMappingPropertiesCollection)
                    attributeModelMappingProperties = (AttributeModelMappingPropertiesCollection)mappingObj;
            }

            return attributeModelMappingProperties;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ContainerEntityTypeAttributeMappingCollection FindAllContainerEntityTypeAttributeMappings()
        {
            ContainerEntityTypeAttributeMappingCollection containerEntityTypeAttributeMappings = null;

            if (this._isMappingCacheEnabled)
            {
                String cacheKey = CacheKeyGenerator.GetAllAttributeModelMappingCacheKey(AttributeModelType.Common);

                object mappingObj = null;

                try
                {
                    mappingObj = _cacheManager.Get(cacheKey);
                }
                catch (Exception ex)
                {
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, String.Format("Error occurred : {0}", ex.Message));
                    ExceptionHandler exceptionHandler = new ExceptionHandler(ex);
                }

                containerEntityTypeAttributeMappings = mappingObj as ContainerEntityTypeAttributeMappingCollection;
            }

            return containerEntityTypeAttributeMappings;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="containerEntityTypeAttributeMappings"></param>
        /// <param name="containerId"></param>
        /// <param name="entityTypeId"></param>
        /// <param name="numberOfRetry"></param>
        /// <param name="isDataUpdated"></param>
        public void UpdateContainerEntityTypeAttributeMappings(ContainerEntityTypeAttributeMappingCollection containerEntityTypeAttributeMappings, Int32 containerId, Int32 entityTypeId, Int32 numberOfRetry, Boolean isDataUpdated = false)
        {
            if (this._isMappingCacheEnabled)
            {
                AttributeModelType attributeModelType = AttributeModelType.Common;

                if (containerEntityTypeAttributeMappings == null)
                    throw new ArgumentException("Container-EntityType attribute mappings are empty not available.");

                if (containerId < 1)
                    throw new ArgumentNullException("ContainerId is not available.");

                if (entityTypeId < 1)
                    throw new ArgumentNullException("EntityTypeId is not available.");

                String cacheKeyForCompleteDetails = CacheKeyGenerator.GetAttributeModelMappingCacheKey(attributeModelType, containerId, entityTypeId, 0, 0);
                String cacheKeyForPartialDetails = CacheKeyGenerator.GetAttributeModelMappingCacheKey(attributeModelType, containerId, entityTypeId, 0, 0, false);
                String cacheKeyForAllMappings = CacheKeyGenerator.GetAllAttributeModelMappingCacheKey(AttributeModelType.Common);

                DateTime expiryTime = DateTime.Now.AddDays(5);

                AttributeModelMappingPropertiesCollection attributeModelMappingProperties = ConvertToAttributeModelMappingProperties(containerEntityTypeAttributeMappings);

                try
                {
                    if (_isDistributedCacheWithNotificationEnabled)
                    {
                        _cacheSynchronizationHelper.NotifyLocalCacheInsert(cacheKeyForCompleteDetails, expiryTime, isDataUpdated);
                        _cacheSynchronizationHelper.NotifyLocalCacheInsert(cacheKeyForPartialDetails, expiryTime, isDataUpdated);

                        //Web caches All common mappings, so removing it here
                        _cacheSynchronizationHelper.NotifyLocalCacheRemoval(cacheKeyForAllMappings);
                    }

                    _cacheManager.Set(cacheKeyForCompleteDetails, containerEntityTypeAttributeMappings, expiryTime);
                    _cacheManager.Set(cacheKeyForPartialDetails, attributeModelMappingProperties, expiryTime);
                }
                catch (Exception ex)
                {
                    //Retry < 0 means just ignore update if failed..
                    if (numberOfRetry < 0)
                        return;

                    bool retrySuccess = false;

                    for (int i = 0; i < numberOfRetry; i++)
                    {
                        try
                        {
                            if (_isDistributedCacheWithNotificationEnabled)
                            {
                                _cacheSynchronizationHelper.NotifyLocalCacheInsert(cacheKeyForCompleteDetails, expiryTime, isDataUpdated);
                                _cacheSynchronizationHelper.NotifyLocalCacheInsert(cacheKeyForPartialDetails, expiryTime, isDataUpdated);

                                //Web caches All common mappings, so removing it here
                                _cacheSynchronizationHelper.NotifyLocalCacheRemoval(cacheKeyForAllMappings);
                            }

                            _cacheManager.Set(cacheKeyForCompleteDetails, containerEntityTypeAttributeMappings, expiryTime);
                            _cacheManager.Set(cacheKeyForPartialDetails, attributeModelMappingProperties, expiryTime);

                            retrySuccess = true;
                            break;
                        }
                        catch
                        {
                        }
                    }

                    if (!retrySuccess)
                    {
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, String.Format("Error occurred : {0}", ex.Message));
                        ExceptionHandler exceptionHandler = new ExceptionHandler(ex);
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="numberOfRetry"></param>
        /// <param name="isDataUpdated"></param>
        /// <param name="containerEntityTypeAttributeMappings"></param>
        public void UpdateAllContainerEntityTypeAttributeMappings(ContainerEntityTypeAttributeMappingCollection containerEntityTypeAttributeMappings, Int32 numberOfRetry, Boolean isDataUpdated = false)
        {
            if (this._isMappingCacheEnabled)
            {
                AttributeModelType attributeModelType = AttributeModelType.Common;

                if (containerEntityTypeAttributeMappings == null)
                {
                    throw new ArgumentException("Container-EntityType attribute mappings are empty not available.");
                }

                String cacheKey = CacheKeyGenerator.GetAllAttributeModelMappingCacheKey(attributeModelType);

                DateTime expiryTime = DateTime.Now.AddDays(5);

                try
                {
                    if (_isDistributedCacheWithNotificationEnabled)
                    {
                        _cacheSynchronizationHelper.NotifyLocalCacheInsert(cacheKey, expiryTime, isDataUpdated);
                    }

                    _cacheManager.Set(cacheKey, containerEntityTypeAttributeMappings, expiryTime);
                }
                catch (Exception ex)
                {
                    //Retry < 0 means just ignore update if failed..
                    if (numberOfRetry < 0)
                        return;

                    bool retrySuccess = false;

                    for (int i = 0; i < numberOfRetry; i++)
                    {
                        try
                        {
                            if (_isDistributedCacheWithNotificationEnabled)
                            {
                                _cacheSynchronizationHelper.NotifyLocalCacheInsert(cacheKey, expiryTime, isDataUpdated);
                            }

                            _cacheManager.Set(cacheKey, containerEntityTypeAttributeMappings, expiryTime);

                            retrySuccess = true;
                            break;
                        }
                        catch
                        {
                        }
                    }

                    if (!retrySuccess)
                    {
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, String.Format("Error occurred : {0}", ex.Message));
                        ExceptionHandler exceptionHandler = new ExceptionHandler(ex);
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="containerId"></param>
        /// <param name="entityTypeId"></param>
        /// <param name="publishCacheChangeEvent"></param>
        /// <returns></returns>
        public Boolean RemoveContainerEntityTypeAttributeMappings(Int32 containerId, Int32 entityTypeId, Boolean publishCacheChangeEvent)
        {
            return RemoveMappings(AttributeModelType.Common, containerId, entityTypeId, 0, 0, publishCacheChangeEvent);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="containerIds"></param>
        /// <param name="entityTypeIds"></param>
        /// <param name="publishCacheChangeEvent"></param>
        /// <returns></returns>
        public Boolean RemoveContainerEntityTypeAttributeMappings(Collection<Int32> containerIds, Collection<Int32> entityTypeIds, Boolean publishCacheChangeEvent)
        {
            Boolean success = true;

            if (containerIds != null && containerIds.Count > 0)
            {
                var containersEnumerator = containerIds.GetEnumerator();

                while (containersEnumerator.MoveNext())
                {
                    if (entityTypeIds == null || entityTypeIds.Count < 1)
                        return success;

                    var entityTypesEnumerator = entityTypeIds.GetEnumerator();
                    {
                        while (entityTypesEnumerator.MoveNext())
                        {
                            success = success && RemoveMappings(AttributeModelType.Common, containersEnumerator.Current, entityTypesEnumerator.Current, 0, 0, publishCacheChangeEvent);
                        }
                    }
                }
            }

            _cacheSynchronizationHelper.NotifyLocalCacheRemoval(CacheKeyContants.RELATIONSHIP_HIERARCHY_CHANGED_KEY);

            return success;
        }

        /// <summary>
        /// Clears local cache when the information related to cache key is changed
        /// </summary>
        /// <param name="modelDataMasterkey">Indicates the local data master cache key name</param>
        public void NotifyModelDataChange(String modelDataMasterkey)
        {
            if (_isDistributedCacheWithNotificationEnabled)
            {
                try
                {
                    if ((modelDataMasterkey == CacheKeyContants.ATTRIBUTE_MODELS_CHANGED_KEY && _isMappingCacheEnabled)
                         || (modelDataMasterkey != CacheKeyContants.ATTRIBUTE_MODELS_CHANGED_KEY))
                    {
                        _cacheSynchronizationHelper.NotifyLocalCacheRemoval(modelDataMasterkey);
                    }
                }
                catch (Exception ex)
                {
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, String.Format("Error occurred : {0}", ex.Message));
                    ExceptionHandler exceptionHandler = new ExceptionHandler(ex);
                }
            }
        }

        #endregion

        #region Category - Attribute Mappings Methods

        /// <summary>
        /// Find CategoryAttributeModelMappings from the cache
        /// </summary>
        /// <param name="categoryId">Category Id for which CategoryAttributeMappings needed</param>
        /// <returns>CategoryAttributeMappingCollection</returns>
        public CategoryAttributeMappingCollection FindCategoryAttributeMappingsCompleteDetails(Int64 categoryId)
        {
            CategoryAttributeMappingCollection categoryAttributeMappings = null;

            if (this._isMappingCacheEnabled)
            {
                String cacheKey = CacheKeyGenerator.GetAttributeModelMappingCacheKey(AttributeModelType.Category, 0, 0, 0, categoryId);

                object mappingObj = null;

                try
                {
                    mappingObj = _cacheManager.Get(cacheKey);
                }
                catch (Exception ex)
                {
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, String.Format("Error occurred : {0}", ex.Message));
                    ExceptionHandler exceptionHandler = new ExceptionHandler(ex);
                }

                if (mappingObj != null && mappingObj is CategoryAttributeMappingCollection)
                    categoryAttributeMappings = (CategoryAttributeMappingCollection)mappingObj;
            }

            return categoryAttributeMappings;
        }

        /// <summary>
        /// Find CategoryAttributeModel MappingProperties from the cache
        /// </summary>
        /// <param name="categoryId">Category Id for which CategoryAttributeModelMappingProperties needed</param>
        /// <returns>CategoryAttributeMappingCollection</returns>
        public AttributeModelMappingPropertiesCollection FindCategoryAttributeModelMappingsPartialDetails(Int64 categoryId)
        {
            AttributeModelMappingPropertiesCollection attributeModelMappingProperties = null;

            if (this._isMappingCacheEnabled)
            {
                String cacheKey = CacheKeyGenerator.GetAttributeModelMappingCacheKey(AttributeModelType.Category, 0, 0, 0, categoryId, false);

                object mappingObj = null;

                try
                {
                    mappingObj = _cacheManager.Get(cacheKey);
                }
                catch (Exception ex)
                {
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, String.Format("Error occurred : {0}", ex.Message));
                    ExceptionHandler exceptionHandler = new ExceptionHandler(ex);
                }

                if (mappingObj != null && mappingObj is AttributeModelMappingPropertiesCollection)
                    attributeModelMappingProperties = (AttributeModelMappingPropertiesCollection)mappingObj;
            }

            return attributeModelMappingProperties;
        }

        /// <summary>
        /// Update Category Attribute Mapping Cache.
        /// </summary>
        /// <param name="categoryAttributeMappingCollection">Collection of Category Attribute Mappings</param>
        /// <param name="categoryId">Category Id for which mappings needs to be updated</param>
        /// <param name="numberOfRetry">Number of try to update the cache</param>
        /// <param name="isDataUpdated">true if date updated</param>
        public void UpdateCategoryAttributeMappings(CategoryAttributeMappingCollection categoryAttributeMappingCollection, Int64 categoryId, Int32 numberOfRetry, Boolean isDataUpdated = false)
        {
            if (this._isMappingCacheEnabled)
            {
                if (categoryAttributeMappingCollection == null)
                    throw new ArgumentException("Category attribute mappings are empty not available.");

                if (categoryId < 1)
                    throw new ArgumentNullException("CategoryId is not available.");

                String cacheKey = CacheKeyGenerator.GetAttributeModelMappingCacheKey(AttributeModelType.Category, 0, 0, 0, categoryId);
                AttributeModelType attributeModelType = AttributeModelType.Category;

                String cacheKeyForCompleteDetails = CacheKeyGenerator.GetAttributeModelMappingCacheKey(attributeModelType, 0, 0, 0, categoryId);
                String cacheKeyForPartialDetails = CacheKeyGenerator.GetAttributeModelMappingCacheKey(attributeModelType, 0, 0, 0, categoryId, false);

                DateTime expiryTime = DateTime.Now.AddDays(5);

                if (String.IsNullOrWhiteSpace(cacheKeyForCompleteDetails) || String.IsNullOrWhiteSpace(cacheKeyForPartialDetails))
                {
                    throw new ApplicationException(String.Format("Not able to update category attribute mapping cache due to cache key generation failure."));
                }

                AttributeModelMappingPropertiesCollection attributeModelProperties = ConvertToAttributeModelMappingProperties(categoryAttributeMappingCollection);

                try
                {
                    if (_isDistributedCacheWithNotificationEnabled)
                    {
                        _cacheSynchronizationHelper.NotifyLocalCacheInsert(cacheKeyForCompleteDetails, expiryTime, isDataUpdated);
                        _cacheSynchronizationHelper.NotifyLocalCacheInsert(cacheKeyForPartialDetails, expiryTime, isDataUpdated);
                    }

                    _cacheManager.Set(cacheKeyForCompleteDetails, categoryAttributeMappingCollection, expiryTime);
                    _cacheManager.Set(cacheKeyForPartialDetails, attributeModelProperties, expiryTime);
                }
                catch (Exception ex)
                {
                    //Retry < 0 means just ignore update if failed..
                    if (numberOfRetry < 0)
                        return;

                    Boolean retrySuccess = false;

                    for (int i = 0; i < numberOfRetry; i++)
                    {
                        try
                        {
                            if (_isDistributedCacheWithNotificationEnabled)
                            {
                                _cacheSynchronizationHelper.NotifyLocalCacheInsert(cacheKeyForCompleteDetails, expiryTime, isDataUpdated);
                                _cacheSynchronizationHelper.NotifyLocalCacheInsert(cacheKeyForPartialDetails, expiryTime, isDataUpdated);
                            }

                            _cacheManager.Set(cacheKeyForCompleteDetails, categoryAttributeMappingCollection, expiryTime);
                            _cacheManager.Set(cacheKeyForPartialDetails, attributeModelProperties, expiryTime);

                            retrySuccess = true;
                            break;
                        }
                        catch
                        {
                        }
                    }

                    //throw exception if cache update retry is failed too..
                    if (!retrySuccess)
                        throw new ApplicationException(String.Format("Not able to update attribute model cache due to internal error. Error: {0}", ex.Message));

                }
            }
        }

        /// <summary>
        /// Remove Category Attribute Mappings from the cache
        /// </summary>
        /// <param name="categoryId">Category Id for which mappings needs to be removed</param>
        /// <param name="publishCacheChangeEvent">Decides whether to notify JobService or not.</param>
        /// <returns>True if removed from the cache</returns>
        public Boolean RemoveCategoryAttributeMappings(Int64 categoryId, Boolean publishCacheChangeEvent)
        {
            return RemoveMappings(AttributeModelType.Category, 0, 0, 0, categoryId, publishCacheChangeEvent);
        }

        /// <summary>
        /// Remove Category Attribute Mappings from the cache
        /// </summary>
        /// <param name="categoryAttributeMappings">Category Attribute Mapping Collection that needs to be removed.</param>
        /// <param name="publishCacheChangeEvent">Decides whether to notify JobService or not.</param>
        /// <returns>True if removed from the cache</returns>

        public Boolean RemoveCategoryAttributeMappings(CategoryAttributeMappingCollection categoryAttributeMappings, Boolean publishCacheChangeEvent)
        {
            return RemoveMappings(AttributeModelType.Category, 0, 0, 0, categoryAttributeMappings, publishCacheChangeEvent);
        }

        /// <summary>
        /// Removes impacted category attribute mappings from cache
        /// </summary>
        /// <param name="categoryIdList"></param>
        /// <param name="entityManager"></param>
        public void RemoveImpactedCategoryAttributeMappingFromCache(IEnumerable<Int64> categoryIdList, IEntityManager entityManager)
        {
            foreach (Int64 parentCategoryId in categoryIdList)
            {
                this.RemoveCategoryAttributeMappings(parentCategoryId, false);
                Collection<Int64> impactedCollection = entityManager.GetChildrenByEntityType(parentCategoryId, 6);

                foreach (Int64 childCategoryId in impactedCollection)
                {
                    this.RemoveCategoryAttributeMappings(childCategoryId, false);
                }
            }
        }

        #endregion

        #region RelationshipType - Attribute Mappings Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="relationshipTypeId"></param>
        /// <returns></returns>
        public RelationshipTypeAttributeMappingCollection FindRelationshipAttributeMappings(Int32 relationshipTypeId)
        {
            RelationshipTypeAttributeMappingCollection relationshipTypeAttributeMappings = null;

            if (this._isMappingCacheEnabled)
            {
                String cacheKey = CacheKeyGenerator.GetAttributeModelMappingCacheKey(AttributeModelType.Relationship, 0, 0, relationshipTypeId, 0);

                object mappingObj = null;

                try
                {
                    mappingObj = _cacheManager.Get(cacheKey);
                }
                catch (Exception ex)
                {
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, String.Format("Error occurred : {0}", ex.Message));
                    ExceptionHandler exceptionHandler = new ExceptionHandler(ex);
                }

                if (mappingObj != null && mappingObj is RelationshipTypeAttributeMappingCollection)
                    relationshipTypeAttributeMappings = (RelationshipTypeAttributeMappingCollection)mappingObj;
            }

            return relationshipTypeAttributeMappings;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="relationshipTypeAttributeMappings"></param>
        /// <param name="relationshipTypeId"></param>
        /// <param name="numberOfRetry"></param>
        /// <param name="isDataUpdated"></param>
        public void UpdateRelationshipAttributeMappings(RelationshipTypeAttributeMappingCollection relationshipTypeAttributeMappings, Int32 relationshipTypeId, Int32 numberOfRetry, Boolean isDataUpdated = false)
        {
            if (this._isMappingCacheEnabled)
            {
                if (relationshipTypeAttributeMappings == null)
                    throw new ArgumentException("Relationship attribute mappings are empty not available.");

                String cacheKey = CacheKeyGenerator.GetAttributeModelMappingCacheKey(AttributeModelType.Relationship, 0, 0, relationshipTypeId, 0);

                DateTime expiryTime = DateTime.Now.AddDays(5);

                try
                {
                    if (_isDistributedCacheWithNotificationEnabled)
                        _cacheSynchronizationHelper.NotifyLocalCacheInsert(cacheKey, expiryTime, isDataUpdated);

                    _cacheManager.Set(cacheKey, relationshipTypeAttributeMappings, expiryTime);
                }
                catch (Exception ex)
                {
                    //Retry < 0 means just ignore update if failed..
                    if (numberOfRetry < 0)
                        return;

                    bool retrySuccess = false;

                    for (int i = 0; i < numberOfRetry; i++)
                    {
                        try
                        {
                            if (_isDistributedCacheWithNotificationEnabled)
                                _cacheSynchronizationHelper.NotifyLocalCacheInsert(cacheKey, expiryTime, isDataUpdated);

                            _cacheManager.Set(cacheKey, relationshipTypeAttributeMappings, expiryTime);

                            retrySuccess = true;
                            break;
                        }
                        catch
                        {
                        }
                    }

                    if (!retrySuccess)
                    {
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, String.Format("Error occurred : {0}", ex.Message));
                        ExceptionHandler exceptionHandler = new ExceptionHandler(ex);
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="relationshipTypeId"></param>
        /// <param name="publishCacheChangeEvent"></param>
        /// <returns></returns>
        public Boolean RemoveRelationshipAttributeMappings(Int32 relationshipTypeId, Boolean publishCacheChangeEvent)
        {
            Boolean success = false;

            success = RemoveMappings(AttributeModelType.Relationship, 0, 0, relationshipTypeId, 0, publishCacheChangeEvent);

            return success;
        }

        #endregion

        #region Container - RelationshipType Mappings Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="containerId"></param>
        /// <param name="relationshipTypeId"></param>
        /// <returns></returns>
        public ContainerRelationshipTypeAttributeMappingCollection FindContainerRelationshipAttributeMappingsCompleteDetails(Int32 containerId, Int32 relationshipTypeId)
        {
            ContainerRelationshipTypeAttributeMappingCollection containerRelationshipTypeAttributeMappings = null;

            if (this._isMappingCacheEnabled)
            {
                String cacheKey = CacheKeyGenerator.GetAttributeModelMappingCacheKey(AttributeModelType.Relationship, containerId, 0, relationshipTypeId, 0);

                object mappingObj = null;

                try
                {
                    mappingObj = _cacheManager.Get(cacheKey);
                }
                catch (Exception ex)
                {
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, String.Format("Error occurred : {0}", ex.Message));
                    ExceptionHandler exceptionHandler = new ExceptionHandler(ex);
                }

                if (mappingObj != null && mappingObj is ContainerRelationshipTypeAttributeMappingCollection)
                    containerRelationshipTypeAttributeMappings = (ContainerRelationshipTypeAttributeMappingCollection)mappingObj;
            }

            return containerRelationshipTypeAttributeMappings;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="containerId"></param>
        /// <param name="relationshipTypeId"></param>
        /// <returns></returns>
        public AttributeModelMappingPropertiesCollection FindContainerRelationshipAttributeModelMappingsPartialDetails(Int32 containerId, Int32 relationshipTypeId)
        {
            AttributeModelMappingPropertiesCollection attributeModelMappingProperties = null;

            if (this._isMappingCacheEnabled)
            {
                String cacheKey = CacheKeyGenerator.GetAttributeModelMappingCacheKey(AttributeModelType.Relationship, containerId, 0, relationshipTypeId, 0, false);

                object mappingObj = null;

                try
                {
                    mappingObj = _cacheManager.Get(cacheKey);
                }
                catch (Exception ex)
                {
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, String.Format("Error occurred : {0}", ex.Message));
                    ExceptionHandler exceptionHandler = new ExceptionHandler(ex);
                }

                if (mappingObj != null && mappingObj is AttributeModelMappingPropertiesCollection)
                    attributeModelMappingProperties = (AttributeModelMappingPropertiesCollection)mappingObj;
            }

            return attributeModelMappingProperties;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="containerRelationshipTypeAttributeMappings"></param>
        /// <param name="containerId"></param>
        /// <param name="relationshipTypeId"></param>
        /// <param name="numberOfRetry"></param>
        /// <param name="isDataUpdated"></param>
        public void UpdateContainerRelationshipAttributeMappings(ContainerRelationshipTypeAttributeMappingCollection containerRelationshipTypeAttributeMappings, Int32 containerId, Int32 relationshipTypeId, Int32 numberOfRetry, Boolean isDataUpdated = false)
        {
            if (this._isMappingCacheEnabled)
            {
                if (containerRelationshipTypeAttributeMappings == null)
                    throw new ArgumentException("Container-Relationship attribute mappings are empty not available.");

                //if (containerId < 1)
                //throw new ArgumentNullException("ContainerId is not available.");

                if (relationshipTypeId < 1)
                    throw new ArgumentNullException("RelationshipTypeId is not available.");

                AttributeModelType attributeModelType = AttributeModelType.Relationship;

                String cacheKeyForCompleteDetails = CacheKeyGenerator.GetAttributeModelMappingCacheKey(attributeModelType, containerId, 0, relationshipTypeId, 0);
                String cacheKeyForPartialDetails = CacheKeyGenerator.GetAttributeModelMappingCacheKey(attributeModelType, containerId, 0, relationshipTypeId, 0, false);

                DateTime expiryTime = DateTime.Now.AddDays(5);
                AttributeModelMappingPropertiesCollection attributeModelMappingProperties = ConvertToAttributeModelMappingProperties(containerRelationshipTypeAttributeMappings);

                try
                {
                    if (_isDistributedCacheWithNotificationEnabled)
                    {
                        _cacheSynchronizationHelper.NotifyLocalCacheInsert(cacheKeyForCompleteDetails, expiryTime, isDataUpdated);
                        _cacheSynchronizationHelper.NotifyLocalCacheInsert(cacheKeyForPartialDetails, expiryTime, isDataUpdated);
                    }

                    _cacheManager.Set(cacheKeyForCompleteDetails, containerRelationshipTypeAttributeMappings, expiryTime);
                    _cacheManager.Set(cacheKeyForPartialDetails, attributeModelMappingProperties, expiryTime);
                }
                catch (Exception ex)
                {
                    //Retry < 0 means just ignore update if failed..
                    if (numberOfRetry < 0)
                        return;

                    bool retrySuccess = false;

                    for (int i = 0; i < numberOfRetry; i++)
                    {
                        try
                        {
                            if (_isDistributedCacheWithNotificationEnabled)
                            {
                                _cacheSynchronizationHelper.NotifyLocalCacheInsert(cacheKeyForCompleteDetails, expiryTime, isDataUpdated);
                                _cacheSynchronizationHelper.NotifyLocalCacheInsert(cacheKeyForPartialDetails, expiryTime, isDataUpdated);
                            }

                            _cacheManager.Set(cacheKeyForCompleteDetails, containerRelationshipTypeAttributeMappings, expiryTime);
                            _cacheManager.Set(cacheKeyForPartialDetails, attributeModelMappingProperties, expiryTime);

                            retrySuccess = true;
                            break;
                        }
                        catch
                        {
                        }
                    }

                    if (!retrySuccess)
                    {
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, String.Format("Error occurred : {0}", ex.Message));
                        ExceptionHandler exceptionHandler = new ExceptionHandler(ex);
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="containerId"></param>
        /// <param name="relationshipTypeId"></param>
        /// <param name="publishCacheChangeEvent"></param>
        /// <returns></returns>
        public Boolean RemoveContainerRelationshipAttributeMappings(Int32 containerId, Int32 relationshipTypeId, Boolean publishCacheChangeEvent)
        {
            return RemoveMappings(AttributeModelType.Relationship, containerId, 0, relationshipTypeId, 0, publishCacheChangeEvent);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="containerIds"></param>
        /// <param name="relationshipTypeIds"></param>
        /// <param name="publishCacheChangeEvent"></param>
        /// <returns></returns>
        public Boolean RemoveContainerRelationshipAttributeMappings(Collection<Int32> containerIds, Collection<Int32> relationshipTypeIds, Boolean publishCacheChangeEvent)
        {
            Boolean success = true;

            if (containerIds != null && containerIds.Count > 0)
            {
                var containersEnumerator = containerIds.GetEnumerator();

                while (containersEnumerator.MoveNext())
                {
                    if (relationshipTypeIds == null || relationshipTypeIds.Count < 1)
                        return success;

                    var relationshipTypeIdsEnumerator = relationshipTypeIds.GetEnumerator();
                    {
                        while (relationshipTypeIdsEnumerator.MoveNext())
                        {
                            success = success && RemoveMappings(AttributeModelType.Relationship, containersEnumerator.Current, 0, relationshipTypeIdsEnumerator.Current, 0, publishCacheChangeEvent);
                        }
                    }
                }
            }

            return success;
        }

        #endregion

        #region System And MetaData Attribute Model Mappings Methods

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public AttributeModelMappingPropertiesCollection FindSystemAndMetaDataAttributeModelMappingsPartialDetails()
        {
            AttributeModelMappingPropertiesCollection attributeModelMappingProperties = null;

            if (this._isMappingCacheEnabled)
            {
                String cacheKey = CacheKeyGenerator.GetAttributeModelMappingCacheKey(AttributeModelType.System, 0, 0, 0, 0, false);

                object mappingObj = null;

                try
                {
                    mappingObj = _cacheManager.Get(cacheKey);
                }
                catch (Exception ex)
                {
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, String.Format("Error occurred : {0}", ex.Message));
                    ExceptionHandler exceptionHandler = new ExceptionHandler(ex);
                }

                if (mappingObj != null && mappingObj is AttributeModelMappingPropertiesCollection)
                    attributeModelMappingProperties = (AttributeModelMappingPropertiesCollection)mappingObj;
            }

            return attributeModelMappingProperties;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="attributeModelMappingProperties"></param>
        /// <param name="numberOfRetry"></param>
        /// <param name="isDataUpdated"></param>
        public void UpdateSystemAndMetaDataAttributeMappings(AttributeModelMappingPropertiesCollection attributeModelMappingProperties, Int32 numberOfRetry, Boolean isDataUpdated = false)
        {
            if (this._isMappingCacheEnabled)
            {
                if (attributeModelMappingProperties == null)
                    throw new ArgumentException("System or MetaData attribute model mappings are empty not available.");

                String cacheKey = CacheKeyGenerator.GetAttributeModelMappingCacheKey(AttributeModelType.System, 0, 0, 0, 0, false);

                DateTime expiryTime = DateTime.Now.AddDays(5);

                try
                {
                    if (_isDistributedCacheWithNotificationEnabled)
                        _cacheSynchronizationHelper.NotifyLocalCacheInsert(cacheKey, expiryTime, isDataUpdated);

                    _cacheManager.Set(cacheKey, attributeModelMappingProperties, expiryTime);
                }
                catch (Exception ex)
                {
                    //Retry < 0 means just ignore update if failed..
                    if (numberOfRetry < 0)
                        return;

                    bool retrySuccess = false;

                    for (int i = 0; i < numberOfRetry; i++)
                    {
                        try
                        {
                            if (_isDistributedCacheWithNotificationEnabled)
                                _cacheSynchronizationHelper.NotifyLocalCacheInsert(cacheKey, expiryTime, isDataUpdated);

                            _cacheManager.Set(cacheKey, attributeModelMappingProperties, expiryTime);

                            retrySuccess = true;
                            break;
                        }
                        catch
                        {
                        }
                    }

                    if (!retrySuccess)
                    {
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, String.Format("Error occurred : {0}", ex.Message));
                        ExceptionHandler exceptionHandler = new ExceptionHandler(ex);
                    }
                }
            }
        }

        /// <summary>
        /// Removes System Attributes Mappings available in Cache
        /// </summary>
        /// <returns>True - If the Attributes are removed successfully else false</returns>
        public Boolean RemoveSystemAndMetaDataAttributeModelMappings()
        {
            return RemoveMappings(AttributeModelType.System, 0, 0, 0, 0, false);
        }

        #endregion

        #region DataModel Exclusion Context Methods

        /// <summary>
        /// Gets All DataModelExclusionContexts from Cache
        /// </summary>
        /// <returns>DataModelExclusionContextCollection</returns>
        public DataModelExclusionContextCollection FindAllDataModelExclusionContext()
        {
            DataModelExclusionContextCollection dataModelExclusionContext = null;

            if (_isExclusionContextCacheEnabled)
            {
                String cacheKey = CacheKeyGenerator.GetDataModelServiceExclusionContextCacheKey();

                object mappingObj = null;

                try
                {
                    mappingObj = _cacheManager.Get(cacheKey);

                    if (mappingObj != null && mappingObj is DataModelExclusionContextCollection)
                    {
                        dataModelExclusionContext = mappingObj as DataModelExclusionContextCollection;
                    }
                }
                catch (Exception ex)
                {
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, String.Format("Error occurred : {0}", ex.Message));
                    ExceptionHandler exceptionHandler = new ExceptionHandler(ex);
                }
            }

            return dataModelExclusionContext;
        }

        /// <summary>
        /// Update DataModelExclusionContextCollection Cache
        /// </summary>
        /// <param name="dataModelExclusionContext">Specifies DataModelExclusionContextCollection</param>
        /// <param name="numberOfRetry">Specifies Number if retry to do</param>
        /// <param name="isDataUpdated">Specifies is data updated in given DataModelExclusionContextCollection</param>
        public void UpdateDataModelExclusionContext(DataModelExclusionContextCollection dataModelExclusionContext, Int32 numberOfRetry, Boolean isDataUpdated = false)
        {
            if (_isExclusionContextCacheEnabled && _isMappingCacheEnabled)
            {
                if (dataModelExclusionContext != null)
                {
                    String cacheKey = CacheKeyGenerator.GetDataModelServiceExclusionContextCacheKey();
                    DateTime expiryTime = DateTime.Now.AddDays(5);

                    try
                    {
                        if (_isDistributedCacheWithNotificationEnabled)
                        {
                            _cacheSynchronizationHelper.NotifyLocalCacheInsert(cacheKey, expiryTime, isDataUpdated);
                        }

                        _cacheManager.Set(cacheKey, dataModelExclusionContext, expiryTime);
                    }
                    catch (Exception ex)
                    {
                        //Retry < 0 means just ignore update if failed..
                        if (numberOfRetry < 0)
                            return;

                        bool retrySuccess = false;

                        for (int i = 0; i < numberOfRetry; i++)
                        {
                            try
                            {
                                if (_isDistributedCacheWithNotificationEnabled)
                                {
                                    _cacheSynchronizationHelper.NotifyLocalCacheInsert(cacheKey, expiryTime, isDataUpdated);
                                }

                                _cacheManager.Set(cacheKey, dataModelExclusionContext, expiryTime);

                                retrySuccess = true;
                                break;
                            }
                            catch
                            {
                            }
                        }

                        if (!retrySuccess)
                        {
                            MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, String.Format("Error occurred : {0}", ex.Message));
                            ExceptionHandler exceptionHandler = new ExceptionHandler(ex);
                        }
                    }
                }
                else
                {
                    throw new ArgumentException("Exclusion Contexts are empty or not available.");
                }
            }
        }

        /// <summary>
        /// Removes DataModelExclusionContextCollection from Cache
        /// </summary>
        /// <returns>True if successfully removed</returns>
        public Boolean RemoveDataModelExclusionContext()
        {
            Boolean success = false;

            String cacheKey = CacheKeyGenerator.GetDataModelServiceExclusionContextCacheKey();

            try
            {
                success = _cacheManager.Remove(cacheKey);

                if (_isDistributedCacheWithNotificationEnabled)
                {
                    _cacheSynchronizationHelper.NotifyLocalCacheRemoval(cacheKey);
                }
            }
            catch (Exception ex)
            {
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, String.Format("Error occurred : {0}", ex.Message));
                ExceptionHandler exceptionHandler = new ExceptionHandler(ex);
            }

            return success;
        }

        #endregion DataModel Exclusion Context Methods

        #region Common Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="containerIds"></param>
        /// <param name="entityTypeId"></param>
        /// <param name="relationshipTypeId"></param>
        /// <param name="attributeModelType"></param>
        /// <param name="publishCacheChangeEvent"></param>
        /// <returns></returns>
        public Boolean InvalidateImpactedMappings(Collection<Int32> containerIds, Int32 entityTypeId, Int32 relationshipTypeId, AttributeModelType attributeModelType, Boolean publishCacheChangeEvent)
        {
            Boolean success = false;

            if (attributeModelType == AttributeModelType.Common)
            {
                foreach (Int32 containerId in containerIds)
                {
                    success = RemoveMappings(attributeModelType, containerId, entityTypeId, 0, 0, publishCacheChangeEvent);
                }
            }
            else if (attributeModelType == AttributeModelType.Relationship)
            {
                foreach (Int32 containerId in containerIds)
                {
                    success = RemoveMappings(attributeModelType, containerId, 0, relationshipTypeId, 0, publishCacheChangeEvent);
                }
            }

            return success;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="containerEntityTypeAttributeMappings"></param>
        /// <returns></returns>
        public AttributeModelMappingPropertiesCollection ConvertToAttributeModelMappingProperties(ContainerEntityTypeAttributeMappingCollection containerEntityTypeAttributeMappings)
        {
            AttributeModelMappingPropertiesCollection attributeModelMappingPropertiesCollection = null;

            if (containerEntityTypeAttributeMappings != null)
            {
                attributeModelMappingPropertiesCollection = new AttributeModelMappingPropertiesCollection();

                foreach (ContainerEntityTypeAttributeMapping containerEntityTypeAttributeMapping in containerEntityTypeAttributeMappings)
                {
                    AttributeModelMappingProperties attributeModelMappingProperties = new AttributeModelMappingProperties(
                        containerEntityTypeAttributeMapping.AttributeId,
                        containerEntityTypeAttributeMapping.AttributeParentId,
                        containerEntityTypeAttributeMapping.Required,
                        containerEntityTypeAttributeMapping.ReadOnly,
                        containerEntityTypeAttributeMapping.ShowAtCreation,
                        containerEntityTypeAttributeMapping.SortOrder,
                        containerEntityTypeAttributeMapping.InheritableOnly,
                        containerEntityTypeAttributeMapping.AutoPromotable,
                        AttributeModelType.Common,
                        containerEntityTypeAttributeMapping.IsSpecialized);

                    attributeModelMappingPropertiesCollection.Add(attributeModelMappingProperties);
                }
            }

            return attributeModelMappingPropertiesCollection;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="categoryAttributeMappings"></param>
        /// <returns></returns>
        public AttributeModelMappingPropertiesCollection ConvertToAttributeModelMappingProperties(CategoryAttributeMappingCollection categoryAttributeMappings)
        {
            AttributeModelMappingPropertiesCollection attributeModelMappingPropertiesCollection = null;

            if (categoryAttributeMappings != null)
            {
                attributeModelMappingPropertiesCollection = new AttributeModelMappingPropertiesCollection();

                foreach (CategoryAttributeMapping categoryAttributeMapping in categoryAttributeMappings)
                {
                    AttributeModelMappingProperties attributeModelMappingProperties = new AttributeModelMappingProperties(categoryAttributeMapping.AttributeId, categoryAttributeMapping, AttributeModelType.Category);
                    attributeModelMappingPropertiesCollection.Add(attributeModelMappingProperties);
                }
            }

            return attributeModelMappingPropertiesCollection;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="containerRelationshipTypeAttributeMappings"></param>
        /// <returns></returns>
        public AttributeModelMappingPropertiesCollection ConvertToAttributeModelMappingProperties(ContainerRelationshipTypeAttributeMappingCollection containerRelationshipTypeAttributeMappings)
        {
            AttributeModelMappingPropertiesCollection attributeModelMappingPropertiesCollection = null;

            if (containerRelationshipTypeAttributeMappings != null)
            {
                attributeModelMappingPropertiesCollection = new AttributeModelMappingPropertiesCollection();

                foreach (ContainerRelationshipTypeAttributeMapping containerRelationshipTypeAttributeMapping in containerRelationshipTypeAttributeMappings)
                {
                    AttributeModelMappingProperties attributeModelMappingProperties = new AttributeModelMappingProperties(
                        containerRelationshipTypeAttributeMapping.AttributeId,
                        containerRelationshipTypeAttributeMapping.AttributeParentId,
                        containerRelationshipTypeAttributeMapping.Required,
                        containerRelationshipTypeAttributeMapping.ReadOnly,
                        containerRelationshipTypeAttributeMapping.ShowAtCreation,
                        containerRelationshipTypeAttributeMapping.SortOrder,
                        false, //as inheritable only flag is not supported for relationship type related mapping
                        containerRelationshipTypeAttributeMapping.AutoPromotable,
                        AttributeModelType.Relationship,
                        containerRelationshipTypeAttributeMapping.IsSpecialized);

                    attributeModelMappingPropertiesCollection.Add(attributeModelMappingProperties);
                }
            }

            return attributeModelMappingPropertiesCollection;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="attributeModelType"></param>
        /// <param name="containerId"></param>
        /// <param name="entityTypeId"></param>
        /// <param name="relationshipTypeId"></param>
        /// <param name="categoryId"></param>
        /// <param name="publishCacheChangeEvent"></param>
        public void NotifyToJobService(AttributeModelType attributeModelType, Int32 containerId, Int32 entityTypeId, Int32 relationshipTypeId, Int64 categoryId, Boolean publishCacheChangeEvent)
        {
            KnowledgeBaseService knowledgeBaseService = new KnowledgeBaseService();
            LocaleCollection locales = (LocaleCollection)knowledgeBaseService.GetAvailableLocales();

            if (locales != null && locales.Count > 0)
            {
                foreach (Locale locale in locales)
                {
                    String cacheKey = CacheKeyGenerator.GetAttributeModelCacheKey(0, attributeModelType, containerId, entityTypeId, relationshipTypeId, categoryId, locale.Locale);

                    if (_isDistributedCacheWithNotificationEnabled)
                    {
                        _cacheSynchronizationHelper.NotifyLocalCacheRemoval(cacheKey);
                    }
                }
            }
        }

        #endregion

        #region Container RelationshipType EntityType Mapping Methods

        /// <summary>
        /// Finds Container RelationshipType EntityType Mapping in cache
        /// </summary>
        /// <returns>ContainerRelationshipTypeEntityTypeMappings</returns>
        public ContainerRelationshipTypeEntityTypeMappingCollection FindContainerRelationshipTypeEntityTypeMapping()
        {
            ContainerRelationshipTypeEntityTypeMappingCollection mappings = null;

            String cacheKey = CacheKeyGenerator.GetContainerRelationshipTypeEntityTypeMappingCacheKey();

            object mappingsObj = null;

            try
            {
                mappingsObj = _cacheManager.Get(cacheKey);
            }
            catch (Exception ex)
            {
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, String.Format("Error occurred : {0}", ex.Message));
                ExceptionHandler exceptionHandler = new ExceptionHandler(ex);
            }

            if (mappingsObj != null && mappingsObj is ContainerRelationshipTypeEntityTypeMappingCollection)
            {
                mappings = (ContainerRelationshipTypeEntityTypeMappingCollection)mappingsObj;
            }

            return mappings;
        }

        /// <summary>
        /// Update Container RelationshipType EntityType Mapping in cache
        /// </summary>
        ///<param name="containerRelationshipTypeEntityTypeMappingCollection">ContainerRelationshipTypeEntityTypeMappingCollection to update in cache</param>
        public void UpdateContainerRelationshipTypeEntityTypeMapping(ContainerRelationshipTypeEntityTypeMappingCollection containerRelationshipTypeEntityTypeMappingCollection)
        {
            if (this._isMappingCacheEnabled)
            {
                if (containerRelationshipTypeEntityTypeMappingCollection == null || containerRelationshipTypeEntityTypeMappingCollection.Count < 1)
                    throw new ArgumentException("ContainerRelationshipTypeEntityTypeMappingCollection are not available.");

                DateTime expiryTime = DateTime.Now.AddDays(365);

                try
                {
                    String cacheKey = CacheKeyGenerator.GetContainerRelationshipTypeEntityTypeMappingCacheKey();
                    _cacheManager.Set(cacheKey, containerRelationshipTypeEntityTypeMappingCollection, expiryTime);

                    if (_isDistributedCacheWithNotificationEnabled)
                    {
                        _cacheSynchronizationHelper.NotifyLocalCacheInsert(cacheKey, expiryTime, false);
                    }
                }
                catch (Exception ex)
                {
                    throw new ApplicationException(String.Format("Not able to update ContainerRelationshipTypeEntityTypeMapping cache due to internal error. Error: {0}", ex.Message));
                }
            }
        }

        /// <summary>
        /// Remove ContainerRelationshipTypeEntityTypeMappingCollection from cache.
        /// </summary>
        /// <returns>Result of deletion from cache.</returns>
        public Boolean RemoveContainerRelationshipTypeEntityTypeMapping()
        {
            Boolean success = false;

            if (this._isMappingCacheEnabled)
            {
                String cacheKey = CacheKeyGenerator.GetContainerRelationshipTypeEntityTypeMappingCacheKey();

                try
                {
                    success = _cacheManager.Remove(cacheKey);

                    if (_isDistributedCacheWithNotificationEnabled)
                    {
                        _cacheSynchronizationHelper.NotifyLocalCacheRemoval(cacheKey);
                        _cacheSynchronizationHelper.NotifyLocalCacheRemoval(CacheKeyContants.ATTRIBUTE_MODELS_CHANGED_KEY);
                    }
                }
                catch (Exception ex)
                {
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, String.Format("Error occurred : {0}", ex.Message));
                    ExceptionHandler exceptionHandler = new ExceptionHandler(ex);
                }

            }

            return success;
        }

        #endregion Container RelationshipType EntityType MappingC Methods

        #region Relationship Cardinality Methods

        /// <summary>
        /// Finds relationship cardinality in cache
        /// </summary>
        /// <param name="relationshipTypeId">Indicates relationshipType for which cardinality is requested</param>
        /// <param name="containerId">Indicates container Id</param>
        /// <param name="fromEntityTypeId">Indicates EntityTypeId for which cardinality is requested</param>
        /// <returns>Relationship Cardinalities</returns>
        public RelationshipCardinalityCollection FindRelationshipCardinalities(Int32 relationshipTypeId, Int32 containerId, Int32 fromEntityTypeId)
        {
            RelationshipCardinalityCollection cardinalities = null;

            if (this._isMappingCacheEnabled)
            {
                String cacheKey = CacheKeyGenerator.GetRelationshipCardinalitiesCacheKey(relationshipTypeId, containerId, fromEntityTypeId);

                object cardinalitiesObj = null;

                try
                {
                    cardinalitiesObj = _cacheManager.Get(cacheKey);
                }
                catch (Exception ex)
                {
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, String.Format("Error occurred : {0}", ex.Message));
                    ExceptionHandler exceptionHandler = new ExceptionHandler(ex);
                }

                if (cardinalitiesObj != null && cardinalitiesObj is RelationshipCardinalityCollection)
                {
                    cardinalities = (RelationshipCardinalityCollection)cardinalitiesObj;
                }
            }
            return cardinalities;
        }

        /// <summary>
        /// Update relationship cardinality in cache
        /// </summary>
        /// <param name="relationshipCardinalityCollection">Indicates collection of relationship cardinalities to put in cache</param>
        /// <param name="relationshipTypeId">Indicates relationshipType for which cardinality is requested</param>
        /// <param name="containerId">Indicates container Id</param>
        /// <param name="fromEntityTypeId">Indicates EntityTypeId for which cardinality is requested</param>
        /// <returns>Relationship Cardinalities</returns>
        public void UpdateRelationshipCardinalities(RelationshipCardinalityCollection relationshipCardinalityCollection, Int32 relationshipTypeId, Int32 containerId, Int32 fromEntityTypeId)
        {
            if (this._isMappingCacheEnabled)
            {
                if (relationshipCardinalityCollection == null)
                    throw new ArgumentException("RelationshipCardinalityCollection are not available.");

                DateTime expiryTime = DateTime.Now.AddDays(365);

                try
                {
                    String cacheKey = CacheKeyGenerator.GetRelationshipCardinalitiesCacheKey(relationshipTypeId, containerId, fromEntityTypeId);
                    _cacheManager.Set(cacheKey, relationshipCardinalityCollection, expiryTime);

                    if (_isDistributedCacheWithNotificationEnabled)
                    {
                        _cacheSynchronizationHelper.NotifyLocalCacheInsert(cacheKey, expiryTime, false);
                    }
                }
                catch (Exception ex)
                {
                    throw new ApplicationException(String.Format("Not able to update relationship cardinality cache due to internal error. Error: {0}", ex.Message));
                }
            }
        }

        /// <summary>
        /// Remove RelationshipCardinalities from cache.
        /// </summary>
        /// <param name="relationshipTypeId">Indicates relationshipType for which cardinality is requested</param>
        /// <param name="containerId">Indicates container Id</param>
        /// <param name="fromEntityTypeId">Indicates EntityTypeId for which cardinality is requested</param>
        /// <returns>Result of deletion from cache.</returns>
        public Boolean RemoveRelationshipCardinalities(Int32 relationshipTypeId, Int32 containerId, Int32 fromEntityTypeId)
        {
            return RemoveRelationshipCardinalities(new Collection<Int32>() { containerId }, relationshipTypeId, fromEntityTypeId);
        }

        /// <summary>
        /// Remove RelationshipCardinalities from cache.
        /// </summary>
        /// <param name="containerIds">Indicates collection of container ids</param>
        /// <param name="relationshipTypeId">Indicates relationshipType for which cardinality is requested</param>
        /// <param name="fromEntityTypeId">Indicates EntityTypeId for which cardinality is requested</param>
        /// <returns>Result of deletion from cache.</returns>
        public Boolean RemoveRelationshipCardinalities(Collection<Int32> containerIds, Int32 relationshipTypeId, Int32 fromEntityTypeId)
        {
            Boolean success = false;

            if (this._isMappingCacheEnabled)
            {
                if (containerIds != null && containerIds.Count > 0)
                {
                    success = true;

                    foreach (Int32 containerId in containerIds)
                    {
                        String cacheKey = CacheKeyGenerator.GetRelationshipCardinalitiesCacheKey(relationshipTypeId, containerId, fromEntityTypeId);

                        try
                        {
                            success = success && _cacheManager.Remove(cacheKey);

                            if (_isDistributedCacheWithNotificationEnabled)
                            {
                                _cacheSynchronizationHelper.NotifyLocalCacheRemoval(cacheKey);
                            }
                        }
                        catch (Exception ex)
                        {
                            MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, String.Format("Error occurred : {0}", ex.Message));
                            ExceptionHandler exceptionHandler = new ExceptionHandler(ex);
                        }
                    }
                }
            }

            return success;
        }

        #endregion

        #endregion

        #region Private Methods

        /// <summary>
        /// Gets the cache key.
        /// </summary>
        /// <param name="categoryId">The category identifier.</param>
        /// <returns></returns>
        String GetCacheKey(Int64 categoryId)
        {
            return CacheKeyGenerator.GetCategoryAttributeMappingCacheKey(categoryId);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="attributeModelType"></param>
        /// <param name="containerId"></param>
        /// <param name="entityTypeId"></param>
        /// <param name="relationshipTypeId"></param>
        /// <param name="categoryId"></param>
        /// <param name="publishCacheChangeEvent"></param>
        /// <returns></returns>
        private Boolean RemoveMappings(AttributeModelType attributeModelType, Int32 containerId, Int32 entityTypeId, Int32 relationshipTypeId, Int64 categoryId, Boolean publishCacheChangeEvent)
        {
            Boolean success = false;

            if (this._isMappingCacheEnabled)
            {
                String cacheKeyForCompleteDetails = CacheKeyGenerator.GetAttributeModelMappingCacheKey(attributeModelType, containerId, entityTypeId, relationshipTypeId, categoryId);
                String cacheKeyForPartialDetails = CacheKeyGenerator.GetAttributeModelMappingCacheKey(attributeModelType, containerId, entityTypeId, relationshipTypeId, categoryId, false);
                String cacheKeyForAllMappings = CacheKeyGenerator.GetAllAttributeModelMappingCacheKey(attributeModelType);

                try
                {
                    success = _cacheManager.Remove(cacheKeyForCompleteDetails);
                    success = _cacheManager.Remove(cacheKeyForPartialDetails);

                    if (_isDistributedCacheWithNotificationEnabled)
                    {
                        _cacheSynchronizationHelper.NotifyLocalCacheRemoval(cacheKeyForCompleteDetails);
                        _cacheSynchronizationHelper.NotifyLocalCacheRemoval(cacheKeyForPartialDetails);
                        _cacheSynchronizationHelper.NotifyLocalCacheRemoval(CacheKeyContants.ATTRIBUTE_MODELS_CHANGED_KEY);
                        //Need to check when to remove the below key and should technical to be included

                        if (attributeModelType == AttributeModelType.Common)
                        {
                            _cacheSynchronizationHelper.NotifyLocalCacheRemoval(CacheKeyContants.COMMON_ATTRIBUTE_CHANGED_KEY);
                        }
                        else if (attributeModelType == AttributeModelType.Relationship)
                        {
                            _cacheSynchronizationHelper.NotifyLocalCacheRemoval(CacheKeyContants.RELATIONSHIP_HIERARCHY_CHANGED_KEY);
                        }
                        else if (attributeModelType == AttributeModelType.Category)
                        {
                            _cacheSynchronizationHelper.NotifyLocalCacheRemoval(CacheKeyContants.TECHNICAL_ATTRIBUTE_CHANGED_KEY);
                        }
                        //Web caches all common models, so removing it here
                        _cacheSynchronizationHelper.NotifyLocalCacheRemoval(cacheKeyForAllMappings);
                    }
                }
                catch (Exception ex)
                {
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, String.Format("Error occurred : {0}", ex.Message));
                    ExceptionHandler exceptionHandler = new ExceptionHandler(ex);
                }

                // NotifyToJobService(attributeModelType, containerId, entityTypeId, relationshipTypeId, categoryId, publishCacheChangeEvent);
            }

            if (publishCacheChangeEvent)
            {
                DataModelVersionManager dataModelVersionManager = new DataModelVersionManager();
                dataModelVersionManager.UpdateAttributeModelsVersionNumber();
            }

            return success;
        }


        /// <summary>
        /// RemoveMappings
        /// </summary>
        /// <param name="attributeModelType"></param>
        /// <param name="containerId"></param>
        /// <param name="entityTypeId"></param>
        /// <param name="relationshipTypeId"></param>
        /// <param name="categoryAttributeMappings"></param>
        /// <param name="publishCacheChangeEvent"></param>
        /// <returns></returns>
        private Boolean RemoveMappings(AttributeModelType attributeModelType, Int32 containerId, Int32 entityTypeId, Int32 relationshipTypeId, CategoryAttributeMappingCollection categoryAttributeMappings, Boolean publishCacheChangeEvent)
        {
            Boolean success = false;

            if (this._isMappingCacheEnabled)
            {
                foreach (var mapping in categoryAttributeMappings)
                {

                    String cacheKeyForCompleteDetails = CacheKeyGenerator.GetAttributeModelMappingCacheKey(attributeModelType, containerId, entityTypeId, relationshipTypeId, mapping.CategoryId);
                    String cacheKeyForPartialDetails = CacheKeyGenerator.GetAttributeModelMappingCacheKey(attributeModelType, containerId, entityTypeId, relationshipTypeId, mapping.CategoryId, false);
                    String cacheKeyForAllMappings = CacheKeyGenerator.GetAllAttributeModelMappingCacheKey(attributeModelType);

                    try
                    {
                        success = _cacheManager.Remove(cacheKeyForCompleteDetails);
                        success = _cacheManager.Remove(cacheKeyForPartialDetails);

                        if (_isDistributedCacheWithNotificationEnabled)
                        {
                            _cacheSynchronizationHelper.NotifyLocalCacheRemoval(cacheKeyForCompleteDetails);
                            _cacheSynchronizationHelper.NotifyLocalCacheRemoval(cacheKeyForPartialDetails);
                            _cacheSynchronizationHelper.NotifyLocalCacheRemoval(CacheKeyContants.ATTRIBUTE_MODELS_CHANGED_KEY);

                            //Web caches all common models, so removing it here
                            _cacheSynchronizationHelper.NotifyLocalCacheRemoval(cacheKeyForAllMappings);

                            if (attributeModelType == AttributeModelType.Category)
                            {
                                _cacheSynchronizationHelper.NotifyLocalCacheRemoval(CacheKeyContants.TECHNICAL_ATTRIBUTE_CHANGED_KEY);
                            }
                        }

                        // We will be depricating this call soon - comenting out for now - this line should not be ported down.
                        // NotifyToJobService(attributeModelType, containerId, entityTypeId, relationshipTypeId, categoryId, publishCacheChangeEvent);
                    }
                    catch (Exception ex)
                    {
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, String.Format("Error occurred : {0}", ex.Message));
                        ExceptionHandler exceptionHandler = new ExceptionHandler(ex);
                    }
                }

            }

            if (publishCacheChangeEvent)
            {
                DataModelVersionManager dataModelVersionManager = new DataModelVersionManager();
                dataModelVersionManager.UpdateAttributeModelsVersionNumber();
            }

            return success;
        }

        #endregion

        #endregion
    }
}