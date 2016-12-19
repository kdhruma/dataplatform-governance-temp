using System;
using System.Diagnostics;
using System.Linq;

namespace MDM.BufferManager
{
    using BusinessObjects;
    using BusinessObjects.Integration;
    using CacheManager.Business;
    using Core;
    using Utility;

    /// <summary>
    /// Specifies MDMObjectType buffer manager
    /// </summary>
    public class MDMObjectTypeBufferManager : BusinessLogicBase
    {
        #region Fields

        /// <summary>
        /// field denoting cache manager of cache
        /// </summary>
        private readonly ICache _cacheManager;

        /// <summary>
        /// field denoting MDMObjectType cache is enabled or not.
        /// </summary>
        private readonly Boolean _isMDMObjectTypeCacheEnabled;

        /// <summary>
        /// Field denotes whether the Distributed Cache with notification has been enabled.
        /// </summary>
        private readonly Boolean _isDistributedCacheWithNotificationEnabled;

        /// <summary>
        /// Field denotes an instance of the CacheSynchronizationHelper.
        /// </summary>
        private readonly CacheSynchronizationHelper _cacheSynchronizationHelper;

        /// <summary>
        /// Field denotes a name of the MDMObjectType Cache Config in Database.
        /// </summary>
        private const String _mdmObjectTypeCacheConfigName = "MDMCenter.MDMObjectTypeManager.MDMObjectTypeCache.Enabled";

        /// <summary>
        /// Field denotes a name of the Distributed Cache Config in Database.
        /// </summary>
        private const String _distributedCacheConfigName = "MDMCenter.DistributedCacheWithNotification.Enabled";

        #endregion

        #region Properties

        /// <summary>
        /// Property denoting MDMObjectType cache is enabled or not.
        /// </summary>
        public Boolean IsMDMObjectTypeCacheEnabled
        {
            get { return _isMDMObjectTypeCacheEnabled; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Instantiate MDMObjectType Buffer Manager
        /// </summary>
        public MDMObjectTypeBufferManager()
        {
            try
            {
                _isMDMObjectTypeCacheEnabled = AppConfigurationHelper.GetAppConfig<bool>(_mdmObjectTypeCacheConfigName, true);
                _isDistributedCacheWithNotificationEnabled = AppConfigurationHelper.GetAppConfig<bool>(_distributedCacheConfigName, false);

                if (_isMDMObjectTypeCacheEnabled)
                {
                    _cacheManager = CacheFactory.GetCache();
                    if (_isDistributedCacheWithNotificationEnabled)
                    {
                        _cacheSynchronizationHelper = new CacheSynchronizationHelper();
                    }
                }

                if (_cacheManager == null)
                {
                    _isMDMObjectTypeCacheEnabled = false;
                }
            }
            catch
            {
                _isMDMObjectTypeCacheEnabled = false;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Finds all available MDMObjectTypes from cache
        /// </summary>
        /// <returns></returns>
        public MDMObjectTypeCollection GetAllMDMObjectTypesFromCache()
        {
            MDMObjectTypeCollection objectTypes = null;

            if (IsMDMObjectTypeCacheEnabled)
            {
                String cacheKey = CacheKeyGenerator.GetAllMDMObjectTypesCacheKey();

                Object mdmObjectTypesObj = null;

                try
                {
                    mdmObjectTypesObj = _cacheManager.Get(cacheKey);
                }
                catch (Exception ex)
                {
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, String.Format("Error occurred : {0}", ex.Message), MDMTraceSource.Integration);
                }

                var mdmObjectTypes = mdmObjectTypesObj as MDMObjectTypeCollection;
                if (mdmObjectTypes != null)
                {
                    MDMObjectTypeCollection mdmObjectTypeCollection = mdmObjectTypes;
                    objectTypes = (MDMObjectTypeCollection)mdmObjectTypeCollection.Clone();
                }
            }

            return objectTypes;
        }

        /// <summary>
        /// Removes all MDMObjectTypes from cache.
        /// </summary>
        /// <returns>True if object deleted successfully otherwise false.</returns>
        public Boolean RemoveMDMObjectTypesFromCache()
        {
            Boolean success = false;

            if (IsMDMObjectTypeCacheEnabled)
            {
                String cacheKey = CacheKeyGenerator.GetAllMDMObjectTypesCacheKey();

                if (!String.IsNullOrWhiteSpace(cacheKey))
                {
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
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, String.Format("Error occurred : {0}", ex.Message), MDMTraceSource.Integration);
                    }
                }
            }

            return success;
        }

        /// <summary>
        /// Updates MDMObjectTypes in Cache
        /// </summary>
        /// <param name="mdmObjectTypes"></param>
        /// <param name="numberOfRetry"></param>
        /// <param name="isDataModified"></param>
        public void UpdateMDMObjectTypesInCache(MDMObjectTypeCollection mdmObjectTypes, Int32 numberOfRetry = 3, Boolean isDataModified = false)
        {
            if (IsMDMObjectTypeCacheEnabled)
            {
                if (mdmObjectTypes == null || !mdmObjectTypes.Any())
                {
                    throw new ArgumentException("MDMObjectTypes are not available or empty.");
                }

                String cacheKey = CacheKeyGenerator.GetAllMDMObjectTypesCacheKey();

                DateTime expiryTime = DateTime.Now.AddMonths(5);

                //Clone the object before set to cache.
                var clonedMDMObjectTypesCollection = mdmObjectTypes.Clone() as MDMObjectTypeCollection;

                try
                {
                    if (_isDistributedCacheWithNotificationEnabled)
                    {
                        _cacheSynchronizationHelper.NotifyLocalCacheInsert(cacheKey, expiryTime, isDataModified);
                    }

                    _cacheManager.Set(cacheKey, clonedMDMObjectTypesCollection, expiryTime);
                }
                catch (Exception ex)
                {
                    //Retry < 0 means just ignore update if failed..
                    if (numberOfRetry < 0)
                    {
                        return;
                    }

                    Boolean retrySuccess = false;

                    for (Int32 i = 0; i < numberOfRetry; i++)
                    {
                        try
                        {
                            if (_isDistributedCacheWithNotificationEnabled)
                            {
                                _cacheSynchronizationHelper.NotifyLocalCacheInsert(cacheKey, expiryTime, isDataModified);
                            }

                            _cacheManager.Set(cacheKey, clonedMDMObjectTypesCollection, expiryTime);

                            retrySuccess = true;
                            break;
                        }
                        catch (Exception innerException)
                        {
                            MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, String.Format("Not able to update MDMObjectTypes cache. Attempt {1}. Error occurred : {0}", innerException.Message, i), MDMTraceSource.Integration);
                        }
                    }

                    //throw exception if cache update retry is failed too..
                    if (!retrySuccess)
                    {
                        throw new ApplicationException(String.Format("Not able to update MDMObjectTypes cache due to internal error. Error: {0}", ex.Message));
                    }
                }
            }
        }

        #endregion
    }
}