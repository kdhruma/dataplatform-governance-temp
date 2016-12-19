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
    /// Specifies IntegrationMessageType buffer manager
    /// </summary>
    public class IntegrationMessageTypeBufferManager : BusinessLogicBase
    {
        #region Fields

        /// <summary>
        /// field denoting cache manager of cache
        /// </summary>
        private readonly ICache _cacheManager;

        /// <summary>
        /// field denoting IntegrationMessageType cache is enabled or not.
        /// </summary>
        private readonly Boolean _isIntegrationMessageTypeCacheEnabled;

        /// <summary>
        /// Field denotes whether the Distributed Cache with notification has been enabled.
        /// </summary>
        private readonly Boolean _isDistributedCacheWithNotificationEnabled;

        /// <summary>
        /// Field denotes an instance of the CacheSynchronizationHelper.
        /// </summary>
        private readonly CacheSynchronizationHelper _cacheSynchronizationHelper;

        /// <summary>
        /// Field denotes a name of the IntegrationMessageType Cache Config in Database.
        /// </summary>
        private const String _integrationMessageTypeCacheConfigName = "MDMCenter.IntegrationMessageTypeManager.IntegrationMessageTypeCache.Enabled";

        /// <summary>
        /// Field denotes a name of the Distributed Cache Config in Database.
        /// </summary>
        private const String _distributedCacheConfigName = "MDMCenter.DistributedCacheWithNotification.Enabled";

        #endregion

        #region Properties

        /// <summary>
        /// Property denoting IntegrationMessageType cache is enabled or not.
        /// </summary>
        public Boolean IsIntegrationMessageTypeCacheEnabled
        {
            get { return _isIntegrationMessageTypeCacheEnabled; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Instantiate IntegrationMessageType Buffer Manager
        /// </summary>
        public IntegrationMessageTypeBufferManager()
        {
            try
            {
                _isIntegrationMessageTypeCacheEnabled = AppConfigurationHelper.GetAppConfig<bool>(_integrationMessageTypeCacheConfigName, true);
                _isDistributedCacheWithNotificationEnabled = AppConfigurationHelper.GetAppConfig<bool>(_distributedCacheConfigName, false);

                if (_isIntegrationMessageTypeCacheEnabled)
                {
                    _cacheManager = CacheFactory.GetCache();
                    if (_isDistributedCacheWithNotificationEnabled)
                    {
                        _cacheSynchronizationHelper = new CacheSynchronizationHelper();
                    }
                }

                if (_cacheManager == null)
                {
                    _isIntegrationMessageTypeCacheEnabled = false;
                }
            }
            catch
            {
                _isIntegrationMessageTypeCacheEnabled = false;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Finds all available IntegrationMessageTypes from cache
        /// </summary>
        /// <returns></returns>
        public IntegrationMessageTypeCollection GetAllIntegrationMessageTypesFromCache()
        {
            IntegrationMessageTypeCollection objectTypes = null;

            if (IsIntegrationMessageTypeCacheEnabled)
            {
                String cacheKey = CacheKeyGenerator.GetAllIntegrationMessageTypesCacheKey();

                Object integrationMessageTypesObj = null;

                try
                {
                    integrationMessageTypesObj = _cacheManager.Get(cacheKey);
                }
                catch (Exception ex)
                {
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, String.Format("Error occurred : {0}", ex.Message), MDMTraceSource.Integration);
                }

                var integrationMessageTypes = integrationMessageTypesObj as IntegrationMessageTypeCollection;
                if (integrationMessageTypes != null)
                {
                    IntegrationMessageTypeCollection integrationMessageTypeCollection = integrationMessageTypes;
                    objectTypes = (IntegrationMessageTypeCollection)integrationMessageTypeCollection.Clone();
                }
            }

            return objectTypes;
        }

        /// <summary>
        /// Removes all IntegrationMessageTypes from cache.
        /// </summary>
        /// <returns>True if object deleted successfully otherwise false.</returns>
        public Boolean RemoveIntegrationMessageTypesFromCache()
        {
            Boolean success = false;

            if (IsIntegrationMessageTypeCacheEnabled)
            {
                String cacheKey = CacheKeyGenerator.GetAllIntegrationMessageTypesCacheKey();

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
        /// Updates IntegrationMessageTypes in Cache
        /// </summary>
        /// <param name="integrationMessageTypes"></param>
        /// <param name="numberOfRetry"></param>
        /// <param name="isDataModified"></param>
        public void UpdateIntegrationMessageTypesInCache(IntegrationMessageTypeCollection integrationMessageTypes, Int32 numberOfRetry = 3, Boolean isDataModified = false)
        {
            if (IsIntegrationMessageTypeCacheEnabled)
            {
                if (integrationMessageTypes == null || !integrationMessageTypes.Any())
                {
                    throw new ArgumentException("IntegrationMessageTypes are not available or empty.");
                }

                String cacheKey = CacheKeyGenerator.GetAllIntegrationMessageTypesCacheKey();

                DateTime expiryTime = DateTime.Now.AddMonths(5);

                //Clone the object before set to cache.
                var clonedIntegrationMessageTypesCollection = integrationMessageTypes.Clone() as IntegrationMessageTypeCollection;

                try
                {
                    if (_isDistributedCacheWithNotificationEnabled)
                    {
                        _cacheSynchronizationHelper.NotifyLocalCacheInsert(cacheKey, expiryTime, isDataModified);
                    }

                    _cacheManager.Set(cacheKey, clonedIntegrationMessageTypesCollection, expiryTime);
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

                            _cacheManager.Set(cacheKey, clonedIntegrationMessageTypesCollection, expiryTime);

                            retrySuccess = true;
                            break;
                        }
                        catch (Exception innerException)
                        {
                            MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, String.Format("Not able to update IntegrationMessageTypes cache. Attempt {1}. Error occurred : {0}", innerException.Message, i), MDMTraceSource.Integration);
                        }
                    }

                    //throw exception if cache update retry is failed too..
                    if (!retrySuccess)
                    {
                        throw new ApplicationException(String.Format("Not able to update IntegrationMessageTypes cache due to internal error. Error: {0}", ex.Message));
                    }
                }
            }
        }

        #endregion
    }
}