using System;
using System.Diagnostics;

namespace MDM.BufferManager
{
    using MDM.BusinessObjects;
    using MDM.CacheManager.Business;
    using MDM.Core;
    using MDM.ExceptionManager;
    using MDM.Utility;

    /// <summary>
    /// Cache manager for hierarchy object
    /// </summary>
    public class HierarchyBufferManager : BusinessLogicBase
    {
        #region Fields

        /// <summary>
        /// field denoting cache manager of cache
        /// </summary>
        private ICache _cacheManager = null;

        /// <summary>
        /// field denoting taxonomy cache is enabled or not.
        /// </summary>
        private Boolean _isHierarchyCacheEnabled = false;

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
        /// Instantiate hierarchy Buffer Manager
        /// </summary>
        public HierarchyBufferManager()
        {
            try
            {
                //Get AppConfig which specify whether cache is enabled for hierarchy or not
                String strIsCacheEnabled = AppConfigurationHelper.GetAppConfig<String>("MDMCenter.HierarchyManager.HierarchyCache.Enabled", "true");
                Boolean isCacheEnabled = false;
                Boolean.TryParse(strIsCacheEnabled, out isCacheEnabled);

                this._isHierarchyCacheEnabled = isCacheEnabled;
                this._isDistributedCacheWithNotificationEnabled = AppConfigurationHelper.GetAppConfig<Boolean>("MDMCenter.DistributedCacheWithNotification.Enabled", false);

                if (_isHierarchyCacheEnabled)
                {
                    _cacheManager = CacheFactory.GetCache();

                    if (_isDistributedCacheWithNotificationEnabled)
                        _cacheSynchronizationHelper = new CacheSynchronizationHelper();
                }

                if (_cacheManager == null)
                    this._isHierarchyCacheEnabled = false;
            }
            catch
            {
                this._isHierarchyCacheEnabled = false;
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Property denoting hierarchy cache is enabled or not.
        /// </summary>
        public Boolean IsHierarchyCacheEnabled
        {
            get
            {
                return _isHierarchyCacheEnabled;
            }
        }

        #endregion

        /// <summary>
        /// Finds all available hierarchies from cache
        /// </summary>
        /// <returns>collection of hierarchies if found in internal cache otherwise null</returns>
        public HierarchyCollection FindAllTaHierarchies()
        {
            HierarchyCollection hierarchies = null;

            if (this._isHierarchyCacheEnabled)
            {
                String cacheKey = CacheKeyGenerator.GetAllHierarchiesCacheKey();

                object taxonomyObj = null;

                try
                {
                    taxonomyObj = _cacheManager.Get(cacheKey);
                }
                catch (Exception ex)
                {
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, String.Format("Error occurred : {0}", ex.Message));
                    ExceptionHandler exceptionHandler = new ExceptionHandler(ex);
                }

                if (taxonomyObj != null && taxonomyObj is HierarchyCollection)
                {
                    HierarchyCollection hierarchyCollection = (HierarchyCollection)taxonomyObj;
                    hierarchies = (HierarchyCollection)hierarchyCollection.Clone();
                }
            }

            return hierarchies;
        }

        /// <summary>
        /// Set Taxonomies in internal cache
        /// </summary>
        /// <param name="hierarchyCollection">This parameter is specifying collection of hierarchies to be update in cache.</param>
        /// <param name="numberOfRetry">This parameter is specifying number of retry to update object in cache.</param>
        /// <param name="isDataUpdated">Indicates whether the cache data object is updated and inserted to local cache.</param>
        public void SetHierarchies(HierarchyCollection hierarchyCollection, Int32 numberOfRetry, Boolean isDataUpdated = false)
        {
            if (this._isHierarchyCacheEnabled)
            {
                String cacheKey = CacheKeyGenerator.GetAllHierarchiesCacheKey();

                DateTime expiryTime = DateTime.Now.AddMonths(5);

                //Clone the object before set to cache.
                HierarchyCollection clonedHierarchies = (HierarchyCollection)hierarchyCollection.Clone();

                try
                {
                    if (_isDistributedCacheWithNotificationEnabled)
                        _cacheSynchronizationHelper.NotifyLocalCacheInsert(cacheKey, expiryTime, isDataUpdated);

                    _cacheManager.Set(cacheKey, clonedHierarchies, expiryTime);
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
                            _cacheManager.Set(cacheKey, hierarchyCollection, expiryTime);
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
        /// Removes all hierarchies from cache.
        /// </summary>
        /// <param name="publishCacheChangeEvent">This parameter is specifying publish cache change event.</param>
        /// <returns>True if object deleted successfully otherwise false.</returns>
        public Boolean RemoveHierarchies(Boolean publishCacheChangeEvent)
        {
            Boolean success = false;

            if (this._isHierarchyCacheEnabled)
            {
                String cacheKey = CacheKeyGenerator.GetAllHierarchiesCacheKey();

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
            }

            return success;
        }
    }
}
