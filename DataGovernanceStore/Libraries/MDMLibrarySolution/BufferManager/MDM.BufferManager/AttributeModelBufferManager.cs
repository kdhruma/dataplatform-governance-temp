using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace MDM.BufferManager
{
    using MDM.BusinessObjects;
    using MDM.CacheManager.Business;
    using MDM.Core;
    using MDM.Interfaces;
    using MDM.Utility;

    /// <summary>
    /// Specifies attribute model buffer manager
    /// </summary>
    public class AttributeModelBufferManager : BusinessLogicBase
    {
        #region Fields

        /// <summary>
        /// field denoting cache manager of cache
        /// </summary>
        private ICache _cacheManager = null;

        /// <summary>
        /// field denoting attribute Model cache is enabled or not.
        /// </summary>
        private Boolean _isAttributeModelCacheEnabled = false;

        /// <summary>
        /// Field denotes whether the Distributed Cache with notification has been enabled.
        /// </summary>
        private Boolean _isDistributedCacheWithNotificationEnabled = false;

        /// <summary>
        /// Field denotes an instance of the CacheSynchronizationHelper.
        /// </summary>
        private CacheSynchronizationHelper _cacheSynchronizationHelper = null;

        #endregion

        #region Properties

        /// <summary>
        /// Property denoting AttributeModel cache is enabled or not.
        /// </summary>
        public Boolean IsAttributeModelCacheEnabled
        {
            get
            {
                return _isAttributeModelCacheEnabled;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Instantiate AttributeModel Buffer Manager
        /// </summary>
        public AttributeModelBufferManager()
        {
            try
            {
                // Get AppConfig which specify whether cache is enabled for entity or not
                this._isAttributeModelCacheEnabled = AppConfigurationHelper.GetAppConfig<Boolean>("MDMCenter.AttributeModelManager.AttributeModelCache.Enabled", false);
                this._isDistributedCacheWithNotificationEnabled = AppConfigurationHelper.GetAppConfig<Boolean>("MDMCenter.DistributedCacheWithNotification.Enabled", false);

                if (_isAttributeModelCacheEnabled)
                {
                    _cacheManager = CacheFactory.GetCache();
                    if (_isDistributedCacheWithNotificationEnabled)
                        _cacheSynchronizationHelper = new CacheSynchronizationHelper();
                }

                if (_cacheManager == null)
                    this._isAttributeModelCacheEnabled = false;
            }
            catch
            {
                this._isAttributeModelCacheEnabled = false;
                //TODO:: HANDLE CACHE DISABLE SCENARIOS
            }
        }

        #endregion

        #region Methods

        #region Public Methods

        #region Attribute Model Locale Methods

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Dictionary<String, AttributeModelLocaleProperties> FindAttributeModelLocaleProperties(LocaleEnum locale)
        {
            Dictionary<String, AttributeModelLocaleProperties> attributeModelLocaleProperties = null;

            if (this._isAttributeModelCacheEnabled)
            {
                String cacheKey = CacheKeyGenerator.GetAttributeModelLocalePropertiesCacheKey(locale);

                object attributeModelLocalePropertiesObj = null;

                if (!String.IsNullOrEmpty(cacheKey))
                {
                    try
                    {
                        attributeModelLocalePropertiesObj = _cacheManager.Get(cacheKey);
                    }
                    catch
                    {
                    }
                }

                if (attributeModelLocalePropertiesObj != null && attributeModelLocalePropertiesObj is Dictionary<String, AttributeModelLocaleProperties>)
                {
                    attributeModelLocaleProperties = (Dictionary<String, AttributeModelLocaleProperties>)attributeModelLocalePropertiesObj;
                }
            }

            return attributeModelLocaleProperties;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="attributeModelLocaleProperties"></param>
        /// <param name="locale"></param>
        /// <param name="numberOfRetry"></param>
        /// <param name="isDataModified"></param>
        public void UpdateAttributeModelLocaleProperties(Dictionary<String, AttributeModelLocaleProperties> attributeModelLocaleProperties,LocaleEnum locale, Int32 numberOfRetry, Boolean isDataModified = false)
        {
            if (this._isAttributeModelCacheEnabled)
            {
                if (attributeModelLocaleProperties == null)
                    throw new ArgumentException("Attribute Model locale properties are not available or empty.");

                String cacheKey = CacheKeyGenerator.GetAttributeModelLocalePropertiesCacheKey(locale);

                DateTime expiryTime = DateTime.Now.AddDays(5);

                if (String.IsNullOrEmpty(cacheKey))
                {
                    throw new ApplicationException(String.Format("Not able to update base attribute models cache due to cache key generation failure."));
                }

                try
                {
                    if (_isDistributedCacheWithNotificationEnabled)
                        _cacheSynchronizationHelper.NotifyLocalCacheInsert(cacheKey, expiryTime, isDataModified);

                    _cacheManager.Set(cacheKey, attributeModelLocaleProperties, expiryTime);
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
                                _cacheSynchronizationHelper.NotifyLocalCacheInsert(cacheKey, expiryTime, isDataModified);

                            _cacheManager.Set(cacheKey, attributeModelLocaleProperties, expiryTime);

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
        /// 
        /// </summary>
        /// <param name="locales"></param>
        /// <returns></returns>
        public Boolean RemoveAttributeModelLocaleProperties(Collection<LocaleEnum> locales)
        {
            Boolean success = false;

            if (this._isAttributeModelCacheEnabled)
            {
                foreach (LocaleEnum locale in locales)
                {
                    String cacheKey = CacheKeyGenerator.GetAttributeModelLocalePropertiesCacheKey(locale);

                    if (!String.IsNullOrWhiteSpace(cacheKey))
                    {
                        try
                        {
                            success = _cacheManager.Remove(cacheKey);

                            if (_isDistributedCacheWithNotificationEnabled)
                            {
                                _cacheSynchronizationHelper.NotifyLocalCacheRemoval(cacheKey);
                                _cacheSynchronizationHelper.NotifyLocalCacheRemoval(CacheKeyContants.ATTRIBUTE_MODELS_CHANGED_KEY);
                            }
                        }
                        catch
                        {
                        }
                    }
                }
            }

            DataModelVersionManager dataModelVersionManager = new DataModelVersionManager();
            dataModelVersionManager.UpdateAttributeModelsVersionNumber();

            return success;
        }

        #endregion

        #region Base Attribute Model Methods

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Dictionary<Int32, AttributeModelBaseProperties> FindBaseAttributeModels()
        {
            Dictionary<Int32, AttributeModelBaseProperties> baseAttributeModels = new Dictionary<Int32, AttributeModelBaseProperties>();

            if (this._isAttributeModelCacheEnabled)
            {
                String cacheKey = CacheKeyGenerator.GetAllBaseAttributeModelsCacheKey();

                object baseAttributeModelsObj = null;

                if (!String.IsNullOrEmpty(cacheKey))
                {
                    try
                    {
                        baseAttributeModelsObj = _cacheManager.Get(cacheKey);
                    }
                    catch
                    {
                    }
                }

                if (baseAttributeModelsObj != null && baseAttributeModelsObj is Dictionary<Int32, AttributeModelBaseProperties>)
                {
                    baseAttributeModels = (Dictionary<Int32, AttributeModelBaseProperties>)baseAttributeModelsObj;
                }
            }

            return baseAttributeModels;
        }

        /// <summary>
        /// Find all base attribute models from cache
        /// </summary>
        /// <returns>all base attribute models</returns>
        public AttributeModelCollection FindBaseAttributeModelsWithAllProperties()
        {
            AttributeModelCollection baseAttributeModels = new AttributeModelCollection();

            if (this._isAttributeModelCacheEnabled)
            {
                String cacheKey = CacheKeyGenerator.GetAllBaseAttributeModelsAllPropertiesCacheKey();

                object baseAttributeModelsObj = null;

                if (!String.IsNullOrEmpty(cacheKey))
                {
                    try
                    {
                        baseAttributeModelsObj = _cacheManager.Get(cacheKey);
                    }
                    catch
                    {
                    }
                }

                if (baseAttributeModelsObj != null && baseAttributeModelsObj is AttributeModelCollection)
                {
                    baseAttributeModels = baseAttributeModelsObj as AttributeModelCollection;
                }
            }

            return baseAttributeModels;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Dictionary<String, Int32> FindAttributeUniqueIdentifierToAttributeIdMaps()
        {
            var attributeUniqueIdentifierToAttributeIdMaps = new Dictionary<String, Int32>();

            if (this._isAttributeModelCacheEnabled)
            {
                String cacheKey = CacheKeyGenerator.GetAllAttributeUniqueIdentifierToAttributeIdMapsCacheKey();

                object attributeUniqueIdentifierToAttributeIdMapsObj = _cacheManager.Get(cacheKey);

                if (attributeUniqueIdentifierToAttributeIdMapsObj != null && attributeUniqueIdentifierToAttributeIdMapsObj is Dictionary<String, Int32>)
                {
                    attributeUniqueIdentifierToAttributeIdMaps = (Dictionary<String, Int32>)attributeUniqueIdentifierToAttributeIdMapsObj;
                }
            }

            return attributeUniqueIdentifierToAttributeIdMaps;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Dictionary<String, Int32> FindAttributeNameToAttributeIdMaps()
        {
            var attributeNameToAttributeIdMaps = new Dictionary<String, Int32>();

            if (this._isAttributeModelCacheEnabled)
            {
                String cacheKey = CacheKeyGenerator.GetAllAttributeNameToAttributeIdMapsCacheKey();

                object attributeNameToAttributeIdMapsObj = _cacheManager.Get(cacheKey);

                if (attributeNameToAttributeIdMapsObj != null && attributeNameToAttributeIdMapsObj is Dictionary<String, Int32>)
                {
                    attributeNameToAttributeIdMaps = (Dictionary<String, Int32>)attributeNameToAttributeIdMapsObj;
                }
            }

            return attributeNameToAttributeIdMaps;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="baseAttributeModels"></param>
        /// <param name="numberOfRetry"></param>
        /// <param name="isDataModified"></param>
        public void UpdateBaseAttributeModels(Dictionary<Int32, AttributeModelBaseProperties> baseAttributeModels, Int32 numberOfRetry, Boolean isDataModified = false)
        {
            if (this._isAttributeModelCacheEnabled)
            {
                if (baseAttributeModels == null)
                    throw new ArgumentException("base Attribute Model is not available or empty.");

                String cacheKey = CacheKeyGenerator.GetAllBaseAttributeModelsCacheKey();

                DateTime expiryTime = DateTime.Now.AddDays(5);

                if (String.IsNullOrEmpty(cacheKey))
                {
                    throw new ApplicationException(String.Format("Not able to update base attribute models cache due to cache key generation failure."));
                }

                try
                {
                    if (_isDistributedCacheWithNotificationEnabled)
                    { 
                        _cacheSynchronizationHelper.NotifyLocalCacheInsert(cacheKey, expiryTime, isDataModified);
                    }
                    
                    _cacheManager.Set(cacheKey, baseAttributeModels, expiryTime);
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
                                _cacheSynchronizationHelper.NotifyLocalCacheInsert(cacheKey, expiryTime, isDataModified);

                            _cacheManager.Set(cacheKey, baseAttributeModels, expiryTime);

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
        /// Update base attribute models in cache
        /// </summary>
        /// <param name="baseAttributeModels">base attribute models to be updated</param>
        /// <param name="numberOfRetry">number of retries for putting into cache</param>
        /// <param name="isDataModified">flag indicating if the data is modified or this is fresh set</param>
        public void UpdateBaseAttributeModelsWithAllProperties(AttributeModelCollection baseAttributeModels, Int32 numberOfRetry, Boolean isDataModified = false)
        {
            if (this._isAttributeModelCacheEnabled)
            {
                if (baseAttributeModels == null)
                {
                    throw new ArgumentException("base Attribute Model is not available or empty.");
                }

                String cacheKey = CacheKeyGenerator.GetAllBaseAttributeModelsAllPropertiesCacheKey();

                DateTime expiryTime = DateTime.Now.AddDays(5);

                if (String.IsNullOrEmpty(cacheKey))
                {
                    throw new ApplicationException(String.Format("Not able to update base attribute models cache due to cache key generation failure."));
                }

                try
                {
                    if (_isDistributedCacheWithNotificationEnabled)
                    {
                        _cacheSynchronizationHelper.NotifyLocalCacheInsert(cacheKey, expiryTime, isDataModified);
                    }

                    _cacheManager.Set(cacheKey, baseAttributeModels, expiryTime);
                }
                catch (Exception ex)
                {
                    //Retry < 0 means just ignore update if failed..
                    if (numberOfRetry < 0)
                    {
                        return;
                    }

                    bool retrySuccess = false;

                    for (int i = 0; i < numberOfRetry; i++)
                    {
                        try
                        {
                            if (_isDistributedCacheWithNotificationEnabled)
                            {
                                _cacheSynchronizationHelper.NotifyLocalCacheInsert(cacheKey, expiryTime, isDataModified);
                            }

                            _cacheManager.Set(cacheKey, baseAttributeModels, expiryTime);

                            retrySuccess = true;
                            break;
                        }
                        catch
                        {
                        }
                    }

                    //throw exception if cache update retry is failed too..
                    if (!retrySuccess)
                    {
                        throw new ApplicationException(String.Format("Not able to update attribute model cache due to internal error. Error: {0}", ex.Message));
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="attributeUniqueIdentifierToAttributeIdMaps"></param>
        /// <param name="attributeNameToAttributeIdMaps"></param>
        /// <param name="numberOfRetry"></param>
        /// <param name="isDataModified"></param>
        public void UpdateBaseAttributeIdMaps(Dictionary<String, Int32> attributeUniqueIdentifierToAttributeIdMaps, Dictionary<String, Int32> attributeNameToAttributeIdMaps, Int32 numberOfRetry, Boolean isDataModified = false)
        {
            if (this._isAttributeModelCacheEnabled)
            {
                if (attributeUniqueIdentifierToAttributeIdMaps == null)
                    throw new ArgumentNullException("attributeUniqueIdentifierToAttributeIdMaps");

                if (attributeNameToAttributeIdMaps == null)
                    throw new ArgumentNullException("attributeNameToAttributeIdMaps");

                String allAttributeUniqueIdentifierToAttributeIdMapsCacheKey = CacheKeyGenerator.GetAllAttributeUniqueIdentifierToAttributeIdMapsCacheKey();
                String allAttributeNameToAttributeIdMapsCacheKey = CacheKeyGenerator.GetAllAttributeNameToAttributeIdMapsCacheKey();

                DateTime expiryTime = DateTime.Now.AddDays(5);

                try
                {
                    if (_isDistributedCacheWithNotificationEnabled)
                    {
                        _cacheSynchronizationHelper.NotifyLocalCacheInsert(allAttributeUniqueIdentifierToAttributeIdMapsCacheKey, expiryTime, isDataModified);
                        _cacheSynchronizationHelper.NotifyLocalCacheInsert(allAttributeNameToAttributeIdMapsCacheKey, expiryTime, isDataModified);
                    }
                        
                    _cacheManager.Set(allAttributeUniqueIdentifierToAttributeIdMapsCacheKey, attributeUniqueIdentifierToAttributeIdMaps, expiryTime);
                    _cacheManager.Set(allAttributeNameToAttributeIdMapsCacheKey, attributeNameToAttributeIdMaps, expiryTime);
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
                                _cacheSynchronizationHelper.NotifyLocalCacheInsert(allAttributeUniqueIdentifierToAttributeIdMapsCacheKey, expiryTime, isDataModified);
                                _cacheSynchronizationHelper.NotifyLocalCacheInsert(allAttributeNameToAttributeIdMapsCacheKey, expiryTime, isDataModified);
                            }

                            _cacheManager.Set(allAttributeUniqueIdentifierToAttributeIdMapsCacheKey, attributeUniqueIdentifierToAttributeIdMaps, expiryTime);
                            _cacheManager.Set(allAttributeNameToAttributeIdMapsCacheKey, attributeNameToAttributeIdMaps, expiryTime);

                            retrySuccess = true;

                            break;
                        }
                        catch
                        {
                        }
                    }

                    //throw exception if cache update retry is failed too..
                    if (!retrySuccess)
                    {
                        throw new ApplicationException(String.Format("Not able to update attribute model cache due to internal error. Error: {0}", ex.Message));
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Boolean RemoveBaseAttributeModels()
        {
            Boolean success = false;

            if (this._isAttributeModelCacheEnabled)
            {
                String baseAttributeModelCacheKey = CacheKeyGenerator.GetAllBaseAttributeModelsCacheKey();
                String baseAttributeModelWithAllPropertiesCacheKey = CacheKeyGenerator.GetAllBaseAttributeModelsAllPropertiesCacheKey();
                String allAttributeUniqueIdentifierToAttributeIdMapsCacheKey = CacheKeyGenerator.GetAllAttributeUniqueIdentifierToAttributeIdMapsCacheKey();
                String allAttributeNameToAttributeIdMapsCacheKey = CacheKeyGenerator.GetAllAttributeNameToAttributeIdMapsCacheKey();

                if (!String.IsNullOrWhiteSpace(baseAttributeModelCacheKey)
                    && !String.IsNullOrWhiteSpace(baseAttributeModelWithAllPropertiesCacheKey)
                    && !String.IsNullOrWhiteSpace(allAttributeUniqueIdentifierToAttributeIdMapsCacheKey)
                    && !String.IsNullOrWhiteSpace(allAttributeNameToAttributeIdMapsCacheKey))
                {
                    try
                    {
                        success = _cacheManager.Remove(baseAttributeModelCacheKey);
                        success = _cacheManager.Remove(baseAttributeModelWithAllPropertiesCacheKey);
                        success = _cacheManager.Remove(allAttributeUniqueIdentifierToAttributeIdMapsCacheKey);
                        success = _cacheManager.Remove(allAttributeNameToAttributeIdMapsCacheKey);

                        if (_isDistributedCacheWithNotificationEnabled)
                        {
                            _cacheSynchronizationHelper.NotifyLocalCacheRemoval(baseAttributeModelCacheKey);
                            _cacheSynchronizationHelper.NotifyLocalCacheRemoval(baseAttributeModelWithAllPropertiesCacheKey);
                            _cacheSynchronizationHelper.NotifyLocalCacheRemoval(allAttributeUniqueIdentifierToAttributeIdMapsCacheKey);
                            _cacheSynchronizationHelper.NotifyLocalCacheRemoval(allAttributeNameToAttributeIdMapsCacheKey);
                            _cacheSynchronizationHelper.NotifyLocalCacheRemoval(CacheKeyContants.ATTRIBUTE_MODELS_CHANGED_KEY);
                        }
                    }
                    catch
                    {
                    }
                }
            }

            var dataModelVersionManager = new DataModelVersionManager();
            dataModelVersionManager.UpdateAttributeModelsVersionNumber();

            return success;
        }

        /// <summary>
        /// Remove base attribute models
        /// </summary>
        /// <returns>result of the remove action</returns>
        public Boolean RemoveBaseAttributeModelsWithAllProperties()
        {
            Boolean success = false;

            if (this._isAttributeModelCacheEnabled)
            {
                String baseAttributeModelCacheKey = CacheKeyGenerator.GetAllBaseAttributeModelsCacheKey();
                String baseAttributeModelWithAllPropertiesCacheKey = CacheKeyGenerator.GetAllBaseAttributeModelsAllPropertiesCacheKey();
                String allAttributeUniqueIdentifierToAttributeIdMapsCacheKey = CacheKeyGenerator.GetAllAttributeUniqueIdentifierToAttributeIdMapsCacheKey();
                String allAttributeNameToAttributeIdMapsCacheKey = CacheKeyGenerator.GetAllAttributeNameToAttributeIdMapsCacheKey();

                if (!String.IsNullOrWhiteSpace(baseAttributeModelCacheKey)
                    && !String.IsNullOrWhiteSpace(baseAttributeModelWithAllPropertiesCacheKey)
                    && !String.IsNullOrWhiteSpace(allAttributeUniqueIdentifierToAttributeIdMapsCacheKey)
                    && !String.IsNullOrWhiteSpace(allAttributeNameToAttributeIdMapsCacheKey))
                {
                    try
                    {
                        success = _cacheManager.Remove(baseAttributeModelCacheKey);
                        success = _cacheManager.Remove(baseAttributeModelWithAllPropertiesCacheKey);
                        success = _cacheManager.Remove(allAttributeUniqueIdentifierToAttributeIdMapsCacheKey);
                        success = _cacheManager.Remove(allAttributeNameToAttributeIdMapsCacheKey);

                        if (_isDistributedCacheWithNotificationEnabled)
                        {
                            _cacheSynchronizationHelper.NotifyLocalCacheRemoval(baseAttributeModelCacheKey);
                            _cacheSynchronizationHelper.NotifyLocalCacheRemoval(baseAttributeModelWithAllPropertiesCacheKey);
                            _cacheSynchronizationHelper.NotifyLocalCacheRemoval(allAttributeUniqueIdentifierToAttributeIdMapsCacheKey);
                            _cacheSynchronizationHelper.NotifyLocalCacheRemoval(allAttributeNameToAttributeIdMapsCacheKey);
                            _cacheSynchronizationHelper.NotifyLocalCacheRemoval(CacheKeyContants.ATTRIBUTE_MODELS_CHANGED_KEY);
                        }
                    }
                    catch
                    {
                    }
                }
            }

            var dataModelVersionManager = new DataModelVersionManager();
            dataModelVersionManager.UpdateAttributeModelsVersionNumber();

            return success;
        }

        #endregion

        #region Attribute Dependency

        /// <summary>
        /// Gets the attribute model dependencies from cache
        /// </summary>
        /// <returns></returns>
        public Dictionary<String, AttributeModel> FindAttributeModelDependencies()
        {
            Dictionary<String, AttributeModel> attributeModelDependencies = null;
            if (this._isAttributeModelCacheEnabled)
            {
                String cacheKey = CacheKeyGenerator.GetAttributeModelDependenciesCacheKey();

                try
                {
                    attributeModelDependencies = _cacheManager.Get<Dictionary<String, AttributeModel>>(cacheKey);
                }
                catch
                {
                }
            }
            return attributeModelDependencies;
        }

        /// <summary>
        /// Updates the attribute model dependencies in cache
        /// </summary>
        /// <param name="attributeModelDependencies">The attribute model dependencies to be cached</param>
        /// <param name="numberOfRetry">The number of times the cache update has to be attempted</param>
        /// <param name="isDataModified">Specifies if the data is updated and added into cache</param>
        public void UpdateAttributeModelDependencies(Dictionary<String, AttributeModel> attributeModelDependencies, Int32 numberOfRetry, Boolean isDataModified = false)
        {
            if (this._isAttributeModelCacheEnabled)
            {
                String cacheKey = CacheKeyGenerator.GetAttributeModelDependenciesCacheKey();

                DateTime expiryTime = DateTime.Now.AddDays(5);

                try
                {
                    if (_isDistributedCacheWithNotificationEnabled)
                        _cacheSynchronizationHelper.NotifyLocalCacheInsert(cacheKey, expiryTime, isDataModified);

                    _cacheManager.Set(cacheKey, attributeModelDependencies, expiryTime);
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
                                _cacheSynchronizationHelper.NotifyLocalCacheInsert(cacheKey, expiryTime, isDataModified);

                            _cacheManager.Set(cacheKey, attributeModelDependencies, expiryTime);
                            retrySuccess = true;
                            break;
                        }
                        catch
                        {
                        }
                    }

                    //throw exception if cache update retry is failed too..
                    if (!retrySuccess)
                    {
                        throw new ApplicationException(String.Format("Not able to update attribute model dependencies in cache due to internal error. Error: {0}", ex.Message));
                    }
                }
            }
        }


        #endregion Attribute Dependency

        #region Impacted Atttribute model objects invalidation

        /// <summary>
        /// 
        /// </summary>
        /// <param name="attributeIdList"></param>
        /// <param name="iAttributeModelManager"></param>
        /// <param name="isMappingCacheDirty"></param>
        /// <param name="publishCacheChangeEvent"></param>
        public void InvalidateImpactedAttributeModels(Collection<Int32> attributeIdList, IAttributeModelManager iAttributeModelManager,
                                                      Boolean isMappingCacheDirty, Boolean publishCacheChangeEvent)
        {
            #region Get list of impacted attribute model contexts

            MappingBufferManager mappingBufferManager = new MappingBufferManager();

            //Get where used list for attributes being processed.
            Collection<KeyValuePair<Int32, Collection<AttributeModelContext>>> impactedModelContexts = iAttributeModelManager.GetWhereUsed(attributeIdList);

            if (impactedModelContexts != null)
            {
                foreach (KeyValuePair<Int32, Collection<AttributeModelContext>> pair in impactedModelContexts)
                {
                    foreach (AttributeModelContext attributeModelContext in pair.Value)
                    {
                        Int32 containerId = attributeModelContext.ContainerId;
                        Int32 entityTypeId = attributeModelContext.EntityTypeId;
                        Int64 categoryId = attributeModelContext.CategoryId;
                        Int32 relationshipTypeId = attributeModelContext.RelationshipTypeId;
                        AttributeModelType attributeModelType = attributeModelContext.AttributeModelType;

                        if (isMappingCacheDirty)
                        {
                            if (attributeModelType == AttributeModelType.Common)
                            {
                                mappingBufferManager.RemoveContainerEntityTypeAttributeMappings(containerId, entityTypeId, false);
                            }
                            else if (attributeModelType == AttributeModelType.Category)
                            {
                                mappingBufferManager.RemoveCategoryAttributeMappings(categoryId, false);
                            }
                            else if (attributeModelType == AttributeModelType.Relationship)
                            {
                                mappingBufferManager.RemoveContainerRelationshipAttributeMappings(containerId, relationshipTypeId, false);
                            }
                        }

                        mappingBufferManager.NotifyToJobService(attributeModelType, containerId, entityTypeId, relationshipTypeId, categoryId, true);
                    }
                }
            }

            #endregion
        }

        #endregion

        #endregion

        #region Private Methods
        #endregion

        #endregion
    }
}