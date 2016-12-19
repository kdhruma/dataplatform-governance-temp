using System;
using System.Diagnostics;

namespace MDM.BufferManager
{
    using MDM.BusinessObjects;
    using MDM.CacheManager.Business;
    using MDM.Core;
    using MDM.ExceptionManager;
    using MDM.Utility;
    using System.Collections.Generic;

    /// <summary>
    /// Specifies EntityType buffer manager
    /// </summary>
    public class EntityTypeBufferManager : BusinessLogicBase
    {
        #region Fields

        /// <summary>
        /// field denoting cache manager of cache
        /// </summary>
        private ICache _cacheManager = null;

        /// <summary>
        /// field denoting entity type cache is enabled or not.
        /// </summary>
        private Boolean _isEntityTypeCacheEnabled = false;

        /// <summary>
        /// Field denotes whether the Distributed Cache with notification has been enabled.
        /// </summary>
        private Boolean _isDistributedCacheWithNotificationEnabled = false;

        /// <summary>
        /// Field denotes an instance of the CacheSynchronizationHelper.
        /// </summary>
        private CacheSynchronizationHelper _cacheSynchronizationHelper = null;

        #endregion

        #region Constructors

        /// <summary>
        /// Instantiate EntityType Buffer Manager
        /// </summary>
        public EntityTypeBufferManager()
        {
            try
            {
                //Get AppConfig which specify whether cache is enabled for entity type or not
                String strIsCacheEnabled = AppConfigurationHelper.GetAppConfig<String>("MDMCenter.DataModelManager.EntityTypeCache.Enabled", "true");
                Boolean isCacheEnabled = false;
                Boolean.TryParse(strIsCacheEnabled, out isCacheEnabled);

                this._isEntityTypeCacheEnabled = isCacheEnabled;
                this._isDistributedCacheWithNotificationEnabled = AppConfigurationHelper.GetAppConfig<Boolean>("MDMCenter.DistributedCacheWithNotification.Enabled", false);

                if (_isEntityTypeCacheEnabled)
                {
                    _cacheManager = CacheFactory.GetCache();
                    if (_isDistributedCacheWithNotificationEnabled)
                        _cacheSynchronizationHelper = new CacheSynchronizationHelper();                        
                }

                if (_cacheManager == null)
                    this._isEntityTypeCacheEnabled = false;
            }
            catch
            {
                this._isEntityTypeCacheEnabled = false;
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Property denoting entity type cache is enabled or not.
        /// </summary>
        public Boolean IsEntityTypeCacheEnabled
        {
            get
            {
                return _isEntityTypeCacheEnabled;
            }
        }

        #endregion

        #region Methods

        #region Public Methods

        /// <summary>
        /// Finds all available entity types from cache
        /// </summary>
        /// <returns>collection of entity types if found in internal cache otherwise null</returns>
        public Dictionary<Int32, EntityType> FindAllEntityTypes()
        {
            Dictionary<Int32, EntityType> entityTypes = null;

            if (this._isEntityTypeCacheEnabled)
            {
                String cacheKey = CacheKeyGenerator.GetAllEntityTypesCacheKey();

                object entityTypesObj = null;

                try
                {
                    entityTypesObj = _cacheManager.Get(cacheKey);
                }
                catch (Exception ex)
                {
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, String.Format("Error occurred : {0}", ex.Message));
                    ExceptionHandler exceptionHandler = new ExceptionHandler(ex);
                }

                if (entityTypesObj != null && entityTypesObj is Dictionary<Int32, EntityType>)
                {
                    entityTypes = (Dictionary<Int32, EntityType>)entityTypesObj;
                }
            }

            return entityTypes;
        }

        /// <summary>
        /// Update entityTypes in internal cache
        /// </summary>
        /// <param name="entityTypes">This parameter is specifying collection of entityTypes to be update in cache.</param>
        /// <param name="numberOfRetry">This parameter is specifying number of retry to update object in cache.</param>
        /// <param name="isDataUpdated">Specifies whether the cache data object is updated and inserted to local cache.</param> 
        public void UpdateEntityTypes(Dictionary<Int32, EntityType> entityTypes, Int32 numberOfRetry, Boolean isDataUpdated = false)
        {
            if (this._isEntityTypeCacheEnabled)
            {
                if (entityTypes == null || entityTypes.Count < 1)
                    throw new ArgumentException("EntityTypes are not available or empty.");

                String cacheKey = CacheKeyGenerator.GetAllEntityTypesCacheKey();

                DateTime expiryTime = DateTime.Now.AddDays(365);

                try
                {
                    if (_isDistributedCacheWithNotificationEnabled)
                        _cacheSynchronizationHelper.NotifyLocalCacheInsert(cacheKey, expiryTime, isDataUpdated);

                    _cacheManager.Set(cacheKey, entityTypes, expiryTime);                    
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

                            _cacheManager.Set(cacheKey, entityTypes, expiryTime);
                            
                            retrySuccess = true;
                            break;
                        }
                        catch
                        {
                        }
                    }

                    if (!retrySuccess)
                    {
                        //TODO:: What to do if update fails after retry too..
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, String.Format("Error occurred : {0}", ex.Message));
                        ExceptionHandler exceptionHandler = new ExceptionHandler(ex);
                    }
                }
            }
        }

        /// <summary>
        /// Removes all entityTypes from cache.
        /// </summary>
        /// <param name="publishCacheChangeEvent">This parameter is specifying publish cache change event.</param>
        /// <returns>True if object deleted successfully otherwise false.</returns>
        public Boolean RemoveEntityTypes(Boolean publishCacheChangeEvent)
        {
            Boolean success = false;

            if (this._isEntityTypeCacheEnabled)
            {
                String cacheKey = CacheKeyGenerator.GetAllEntityTypesCacheKey();

                try
                {       
                    success = _cacheManager.Remove(cacheKey);
                    if (_isDistributedCacheWithNotificationEnabled)
                        _cacheSynchronizationHelper.NotifyLocalCacheRemoval(cacheKey);
                }
                catch (Exception ex)
                {
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, String.Format("Error occurred : {0}", ex.Message));
                    ExceptionHandler exceptionHandler = new ExceptionHandler(ex);
                }

            }

            return success;
        }

        #endregion

        #region Private Methods

        #endregion

        #endregion
    }
}