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
    /// Specifies IntegrationItemDimensionType buffer manager
    /// </summary>
    public class IntegrationItemDimensionTypeBufferManager : BusinessLogicBase
    {
        #region Fields

        /// <summary>
        /// field denoting cache manager of cache
        /// </summary>
        private readonly ICache _cacheManager;

        /// <summary>
        /// field denoting IntegrationItemDimensionType cache is enabled or not.
        /// </summary>
        private readonly Boolean _isIntegrationItemDimensionTypeCacheEnabled;

        /// <summary>
        /// Field denotes whether the Distributed Cache with notification has been enabled.
        /// </summary>
        private readonly Boolean _isDistributedCacheWithNotificationEnabled;

        /// <summary>
        /// Field denotes an instance of the CacheSynchronizationHelper.
        /// </summary>
        private readonly CacheSynchronizationHelper _cacheSynchronizationHelper;

        /// <summary>
        /// Field denotes a name of the DimensionType Cache Config in Database.
        /// </summary>
        private const String _integrationItemDimensionConfigName = "MDMCenter.IntegrationItemDimensionTypeManager.IntegrationItemDimensionTypeCache.Enabled";

        /// <summary>
        /// Field denotes a name of the DistributedCache Config in Database.
        /// </summary>
        private const String _distributedCacheConfigName = "MDMCenter.DistributedCacheWithNotification.Enabled";

        #endregion

        #region Properties

        /// <summary>
        /// Property denoting IntegrationItemDimensionType cache is enabled or not.
        /// </summary>
        public Boolean IsIntegrationItemDimensionTypeCacheEnabled
        {
            get { return _isIntegrationItemDimensionTypeCacheEnabled; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Instantiate IntegrationItemDimensionType Buffer Manager
        /// </summary>
        public IntegrationItemDimensionTypeBufferManager()
        {
            try
            {
                _isIntegrationItemDimensionTypeCacheEnabled = AppConfigurationHelper.GetAppConfig<bool>(_integrationItemDimensionConfigName, true);
                _isDistributedCacheWithNotificationEnabled = AppConfigurationHelper.GetAppConfig<bool>(_distributedCacheConfigName, false);

                if (_isIntegrationItemDimensionTypeCacheEnabled)
                {
                    _cacheManager = CacheFactory.GetCache();
                    if (_isDistributedCacheWithNotificationEnabled)
                    {
                        _cacheSynchronizationHelper = new CacheSynchronizationHelper();
                    }
                }

                if (_cacheManager == null)
                {
                    _isIntegrationItemDimensionTypeCacheEnabled = false;
                }
            }
            catch
            {
                _isIntegrationItemDimensionTypeCacheEnabled = false;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Finds all available IntegrationItemDimensionTypes from cache
        /// </summary>
        /// <returns></returns>
        public IntegrationItemDimensionTypeCollection GetAllIntegrationItemDimensionTypesFromCache()
        {
            IntegrationItemDimensionTypeCollection integrationItemDimensionTypesCollection = null;

            if (IsIntegrationItemDimensionTypeCacheEnabled)
            {
                String cacheKey = CacheKeyGenerator.GetAllIntegrationItemDimensionTypesCacheKey();

                Object integrationItemDimensionTypesObj = null;

                try
                {
                    integrationItemDimensionTypesObj = _cacheManager.Get(cacheKey);
                }
                catch (Exception ex)
                {
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, String.Format("Error occurred : {0}", ex.Message), MDMTraceSource.Integration);
                }

                var integrationItemDimensionTypes = integrationItemDimensionTypesObj as IntegrationItemDimensionTypeCollection;
                if (integrationItemDimensionTypes != null)
                {
                    IntegrationItemDimensionTypeCollection integrationItemDimensionTypeCollection = integrationItemDimensionTypes;
                    integrationItemDimensionTypesCollection = (IntegrationItemDimensionTypeCollection)integrationItemDimensionTypeCollection.Clone();
                }
            }

            return integrationItemDimensionTypesCollection;
        }

        /// <summary>
        /// Removes all IntegrationItemDimensionTypes from cache.
        /// </summary>
        /// <returns>True if object deleted successfully otherwise false.</returns>
        public Boolean RemoveIntegrationItemDimensionTypesFromCache()
        {
            Boolean success = false;

            if (IsIntegrationItemDimensionTypeCacheEnabled)
            {
                String cacheKey = CacheKeyGenerator.GetAllIntegrationItemDimensionTypesCacheKey();

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
        /// Updates IntegrationItemDimensionTypes in Cache
        /// </summary>
        /// <param name="integrationItemDimensionTypes"></param>
        /// <param name="numberOfRetry"></param>
        /// <param name="isDataModified"></param>
        public void UpdateIntegrationItemDimensionTypesInCache(IntegrationItemDimensionTypeCollection integrationItemDimensionTypes, Int32 numberOfRetry = 3, Boolean isDataModified = false)
        {
            if (IsIntegrationItemDimensionTypeCacheEnabled)
            {
                if (integrationItemDimensionTypes == null || !integrationItemDimensionTypes.Any())
                {
                    throw new ArgumentException("IntegrationItemDimensionTypes are not available or empty.");
                }

                String cacheKey = CacheKeyGenerator.GetAllIntegrationItemDimensionTypesCacheKey();

                DateTime expiryTime = DateTime.Now.AddMonths(5);

                //Clone the object before set to cache.
                var clonedIntegrationItemDimensionTypesCollection = integrationItemDimensionTypes.Clone() as IntegrationItemDimensionTypeCollection;

                try
                {
                    if (_isDistributedCacheWithNotificationEnabled)
                    {
                        _cacheSynchronizationHelper.NotifyLocalCacheInsert(cacheKey, expiryTime, isDataModified);
                    }

                    _cacheManager.Set(cacheKey, clonedIntegrationItemDimensionTypesCollection, expiryTime);
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

                            _cacheManager.Set(cacheKey, clonedIntegrationItemDimensionTypesCollection, expiryTime);

                            retrySuccess = true;
                            break;
                        }
                        catch (Exception innerException)
                        {
                            MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, String.Format("Not able to update IntegrationItemDimensionTypes cache. Attempt {1}. Error occurred : {0}", innerException.Message, i), MDMTraceSource.Integration);
                        }
                    }

                    //throw exception if cache update retry is failed too..
                    if (!retrySuccess)
                    {
                        throw new ApplicationException(String.Format("Not able to update IntegrationItemDimensionTypes cache due to internal error. Error: {0}", ex.Message));
                    }
                }
            }
        }

        #endregion
    }
}