using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace MDM.BufferManager
{
    using MDM.BusinessObjects;
    using MDM.CacheManager.Business;
    using MDM.Core;
    using MDM.Utility;

    /// <summary>
    /// Specifies category buffer manager
    /// </summary>
    public class CategoryBufferManager : BusinessLogicBase
    {
        #region Fields

        /// <summary>
        /// field denoting cache manager of cache
        /// </summary>
        private ICache _cacheManager = null;

        /// <summary>
        /// field denoting Category cache is enabled or not.
        /// </summary>
        private Boolean _isCategoryCacheEnabled = false;

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
        /// Property denoting Category cache is enabled or not.
        /// </summary>
        public Boolean IsCategoryCacheEnabled
        {
            get
            {
                return _isCategoryCacheEnabled;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Instantiate Category Buffer Manager
        /// </summary>
        public CategoryBufferManager()
        {
            try
            {
                // Get AppConfig which specify whether cache is enabled for entity or not
                this._isCategoryCacheEnabled = AppConfigurationHelper.GetAppConfig<Boolean>("MDMCenter.CategoryManager.CategoryCache.Enabled", false);
                this._isDistributedCacheWithNotificationEnabled = AppConfigurationHelper.GetAppConfig<Boolean>("MDMCenter.DistributedCacheWithNotification.Enabled", false);

                if (_isCategoryCacheEnabled)
                {
                    _cacheManager = CacheFactory.GetCache();
                    if (_isDistributedCacheWithNotificationEnabled)
                        _cacheSynchronizationHelper = new CacheSynchronizationHelper();
                }

                if (_cacheManager == null)
                    this._isCategoryCacheEnabled = false;
            }
            catch
            {
                this._isCategoryCacheEnabled = false;
                //TODO:: HANDLE CACHE DISABLE SCENARIOS
            }
        }

        #endregion

        #region Methods

        #region Public Methods

        #region Category Locale Methods

        /// <summary>
        /// Retrieves Local Properties of Category from cache.
        /// </summary>
        /// <param name="locale">Locale in which properties need to be returned</param>
        /// <param name="hierarchyId">Hierarchy Id in which category resides</param>
        /// <returns>Category Locale Properties</returns>
        public Dictionary<String, CategoryLocaleProperties> FindCategoryLocaleProperties(LocaleEnum locale, Int32 hierarchyId)
        {
            Dictionary<String, CategoryLocaleProperties> categoryLocaleProperties = null;

            if (this._isCategoryCacheEnabled)
            {
                String cacheKey = CacheKeyGenerator.GetCategoryLocalePropertiesCacheKey(locale, hierarchyId);

                object categoryLocalePropertiesObj = null;

                if (!String.IsNullOrEmpty(cacheKey))
                {
                    try
                    {
                        categoryLocalePropertiesObj = _cacheManager.Get(cacheKey);
                    }
                    catch
                    {
                    }
                }

                if (categoryLocalePropertiesObj != null && categoryLocalePropertiesObj is Dictionary<String, CategoryLocaleProperties>)
                {
                    categoryLocaleProperties = (Dictionary<String, CategoryLocaleProperties>)categoryLocalePropertiesObj;
                }
            }

            return categoryLocaleProperties;
        }

        /// <summary>
        /// Update the Cache with provided locale properties
        /// </summary>
        /// <param name="categoryLocaleProperties">CategoryLocaleProperties that needed to be put in cache</param>
        /// <param name="locale">Locale in which category properties needed to be put in cache</param>
        /// <param name="hierarchyId">Hierarchy Id in which category resides</param>
        /// <param name="numberOfRetry">Number of attempt</param>
        /// <param name="isDataModified">Data is modified or not</param>
        public void UpdateCategoryLocaleProperties(Dictionary<String, CategoryLocaleProperties> categoryLocaleProperties,LocaleEnum locale, Int32 hierarchyId, Int32 numberOfRetry, Boolean isDataModified = false)
        {
            if (this._isCategoryCacheEnabled)
            {
                if (categoryLocaleProperties == null)
                    throw new ArgumentException("Category locale properties are not available or empty.");

                String cacheKey = CacheKeyGenerator.GetCategoryLocalePropertiesCacheKey(locale, hierarchyId);

                DateTime expiryTime = DateTime.Now.AddDays(5);

                if (String.IsNullOrEmpty(cacheKey))
                {
                    throw new ApplicationException(String.Format("Not able to update base Category cache due to cache key generation failure."));
                }

                try
                {
                    if (_isDistributedCacheWithNotificationEnabled)
                        _cacheSynchronizationHelper.NotifyLocalCacheInsert(cacheKey, expiryTime, isDataModified);

                    _cacheManager.Set(cacheKey, categoryLocaleProperties, expiryTime);
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

                            _cacheManager.Set(cacheKey, categoryLocaleProperties, expiryTime);

                            retrySuccess = true;
                            break;
                        }
                        catch
                        {
                        }
                    }

                    //throw exception if cache update retry is failed too..
                    if (!retrySuccess)
                        throw new ApplicationException(String.Format("Not able to update category cache due to internal error. Error: {0}", ex.Message));
                }
            }
        }

        /// <summary>
        /// Remove Category Locale Properties from Cache
        /// </summary>
        /// <param name="locales">Locale in which properties need to be returned</param>
        /// <param name="hierarchyId">Hierarchy Id in which category resides</param>
        /// <returns>True if successfully removed</returns>
        public Boolean RemoveCategoryLocaleProperties(Collection<LocaleEnum> locales, Int32 hierarchyId)
        {
            Boolean success = false;

            if (this._isCategoryCacheEnabled)
            {
                foreach (LocaleEnum locale in locales)
                {
                    String cacheKey = CacheKeyGenerator.GetCategoryLocalePropertiesCacheKey(locale, hierarchyId);

                    if (!String.IsNullOrWhiteSpace(cacheKey))
                    {
                        try
                        {
                            success = _cacheManager.Remove(cacheKey);

                            if (_isDistributedCacheWithNotificationEnabled)
                                _cacheSynchronizationHelper.NotifyLocalCacheRemoval(cacheKey);
                        }
                        catch
                        {
                        }
                    }
                }
            }

            return success;
        }

        #endregion

        #region Base Category Methods

        /// <summary>
        /// Retrieves Base Properties of Category from cache.
        /// </summary>
        /// <param name="hierarchyId">Hierarchy Id in which category resides</param>
        /// <returns>Category Base Properties</returns>
        public Dictionary<Int64, CategoryBaseProperties> FindBaseCategories(Int32 hierarchyId)
        {
            Dictionary<Int64, CategoryBaseProperties> baseCategories = new Dictionary<Int64, CategoryBaseProperties>();

            if (this._isCategoryCacheEnabled)
            {
                String cacheKey = CacheKeyGenerator.GetBaseCategoriesCacheKey(hierarchyId);

                object baseCategoriesObj = null;

                if (!String.IsNullOrEmpty(cacheKey))
                {
                    try
                    {
                        baseCategoriesObj = _cacheManager.Get(cacheKey);
                    }
                    catch
                    {
                    }
                }

                if (baseCategories != null && baseCategories is Dictionary<Int64, CategoryBaseProperties>)
                {
                    baseCategories = (Dictionary<Int64, CategoryBaseProperties>)baseCategoriesObj;
                }
            }

            return baseCategories;
        }

        /// <summary>
        /// Update the Cache with provided Base properties
        /// </summary>
        /// <param name="baseCategories">CategoryBaseProperties that needed to be put in cache</param>
        /// <param name="hierarchyId">Hierarchy Id in which category resides</param>
        /// <param name="numberOfRetry"></param>
        /// <param name="isDataModified"></param>
        public void UpdateBaseCategories(Dictionary<Int64, CategoryBaseProperties> baseCategories, Int32 hierarchyId, Int32 numberOfRetry, Boolean isDataModified = false)
        {
            if (this._isCategoryCacheEnabled)
            {
                if (baseCategories == null)
                    throw new ArgumentException("base Category is not available or empty.");

                String cacheKey = CacheKeyGenerator.GetBaseCategoriesCacheKey(hierarchyId);

                DateTime expiryTime = DateTime.Now.AddDays(5);

                if (String.IsNullOrEmpty(cacheKey))
                {
                    throw new ApplicationException(String.Format("Not able to update base categories cache due to cache key generation failure."));
                }

                try
                {
                    if (_isDistributedCacheWithNotificationEnabled)
                        _cacheSynchronizationHelper.NotifyLocalCacheInsert(cacheKey, expiryTime, isDataModified);

                    _cacheManager.Set(cacheKey, baseCategories, expiryTime);
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

                            _cacheManager.Set(cacheKey, baseCategories, expiryTime);

                            retrySuccess = true;
                            break;
                        }
                        catch
                        {
                        }
                    }

                    //throw exception if cache update retry is failed too..
                    if (!retrySuccess)
                        throw new ApplicationException(String.Format("Not able to update category cache due to internal error. Error: {0}", ex.Message));
                }
            }
        }

        /// <summary>
        /// Remove Category Base Properties from Cache
        /// </summary>
        /// <param name="hierarchyId">Hierarchy Id in which category resides</param>
        /// <returns>True if successfully removed</returns>
        public Boolean RemoveBaseCategories(Int32 hierarchyId)
        {
            Boolean success = false;

            if (this._isCategoryCacheEnabled)
            {
                String baseCategoryCacheKey = CacheKeyGenerator.GetBaseCategoriesCacheKey(hierarchyId);

                if (!String.IsNullOrWhiteSpace(baseCategoryCacheKey))
                {
                    try
                    {
                        success = _cacheManager.Remove(baseCategoryCacheKey);

                        if (_isDistributedCacheWithNotificationEnabled)
                        {
                            _cacheSynchronizationHelper.NotifyLocalCacheRemoval(baseCategoryCacheKey);
                        }
                    }
                    catch
                    {
                    }
                }
            }

            return success;
        }

        #endregion

        #endregion

        #region Private Methods
        #endregion

        #endregion
    }
}
