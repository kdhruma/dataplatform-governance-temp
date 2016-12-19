using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace MDM.EntityManager.Business
{
    using MDM.Core;
    using MDM.Utility;
    using MDM.ExceptionManager;
    using MDM.Interfaces;
    using MDM.BusinessObjects;
    using MDM.CacheManager.Business;
    using MDM.KnowledgeManager.Business;

    /// <summary>
    /// Specifies entity buffer manager
    /// </summary>
    public class EntityMapBufferManager : BusinessLogicBase
    {
        #region Fields

        private IDistributedCache _cacheManager = null;

        private Boolean _isEntityMapCacheEnabled  = false;

        #endregion

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public Boolean IsEntityMapCacheEnabled
        {
            get
            {
                return _isEntityMapCacheEnabled;
            }
        }

        #endregion

        #region Constructors

        public EntityMapBufferManager()
        {
            try
            {
                // Get AppConfig which specify whether cache is enabled for entity or not
                _isEntityMapCacheEnabled = AppConfigurationHelper.GetAppConfig<Boolean>("MDMCenter.EntityManager.EntityMapCache.Enabled", false);

                if (_isEntityMapCacheEnabled)
                {
                    _cacheManager = CacheFactory.GetDistributedCache();
                }

                if (_cacheManager == null)
                {
                    this._isEntityMapCacheEnabled = false;
                }
            }
            catch
            {
                this._isEntityMapCacheEnabled = false;
                //TODO:: HANDLE CACHE DISABLE SCENARIOS
            }
        }

        #endregion
        
        #region Methods

        #region Public Methods

        #region Entity Base object

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityId"></param>
        /// <param name="entityContext"></param>
        /// <returns></returns>
        public EntityMap FindEntityMap(String systemId, String externalId, Int32 containerId, Int32 entityTypeId, Int64 categoryId)
        {
            //TODO::Entity Buffer Manager has to get requested different data fragments and return the merged cloned entity object
            EntityMap entityMap = null;

            if (this._isEntityMapCacheEnabled)
            {
                //Get cache key
                String cacheKey = CacheKeyGenerator.GetEntityMapCacheKey(systemId, externalId, containerId, entityTypeId, categoryId);

                object entityMapObj = _cacheManager.Get(cacheKey);

                if (entityMapObj != null && entityMapObj is EntityMap)
                    entityMap = (EntityMap)entityMapObj;
            }

            return entityMap;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="entityContext"></param>
        public void UpdateEntityMap(EntityMap entityMap)
        {
            if (this._isEntityMapCacheEnabled)
            {
                if (entityMap == null)
                    throw new ArgumentException("entity is null");

                //Get cache key
                String cacheKey = CacheKeyGenerator.GetEntityMapCacheKey(entityMap.SystemId, entityMap.ExternalId, entityMap.ContainerId, entityMap.EntityTypeId, entityMap.CategoryId);

                DateTime expiryTime = DateTime.Now.AddDays(365);

                try
                {
                    _cacheManager.Set(cacheKey, entityMap, expiryTime);
                }
                catch (Exception ex)
                { 
                    throw new ApplicationException(String.Format("Not able to update entity map cache due to internal error. Error: {0}", ex.Message));
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public Boolean RemoveEntityMap(EntityMap entityMap)
        {
            Boolean success = true;

            if (this._isEntityMapCacheEnabled)
            {
                if (entityMap == null)
                    throw new ArgumentException("entityMap is null");

                //Get cache key
                String cacheKey = CacheKeyGenerator.GetEntityMapCacheKey(entityMap.SystemId, entityMap.ExternalId, entityMap.ContainerId, entityMap.EntityTypeId, entityMap.CategoryId);

                success = _cacheManager.Remove(cacheKey);
                
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
