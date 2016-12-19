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
    /// Specifies Connector profile's buffer manager
    /// </summary>
    public class ConnectorProfileBufferManager: BusinessLogicBase
    {
        #region Fields

        /// <summary>
        /// field denoting cache manager of cache
        /// </summary>
        private readonly ICache _cacheManager;

        /// <summary>
        /// field denoting Connector profile's cache is enabled or not.
        /// </summary>
        private readonly Boolean _isConnectorProfilesCacheEnabled;

        /// <summary>
        /// Field denotes whether the Distributed Cache with notification has been enabled.
        /// </summary>
        private readonly Boolean _isDistributedCacheWithNotificationEnabled;

        /// <summary>
        /// Field denotes an instance of the CacheSynchronizationHelper.
        /// </summary>
        private readonly CacheSynchronizationHelper _cacheSynchronizationHelper;

        /// <summary>
        /// Field denotes a name of the ConnectorProfile Cache Config in Database.
        /// </summary>
        private const String _connectorProfileConfigName = "MDMCenter.ConnectorProfileManager.ConnectorProfileCache.Enabled";

        /// <summary>
        /// Field denotes a name of the DistributedCache Config in Database.
        /// </summary>
        private const String _distributedCacheConfigName = "MDMCenter.DistributedCacheWithNotification.Enabled";

        #endregion

        #region Properties

        /// <summary>
        /// Property denoting Connector profile's cache is enabled or not.
        /// </summary>
        public Boolean IsConnectorProfilesCacheEnabled
        {
            get { return _isConnectorProfilesCacheEnabled; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Instantiate Connector Profiles Buffer Manager
        /// </summary>
        public ConnectorProfileBufferManager()
        {
            try
            {
                _isConnectorProfilesCacheEnabled = AppConfigurationHelper.GetAppConfig<bool>(_connectorProfileConfigName, true);
                _isDistributedCacheWithNotificationEnabled = AppConfigurationHelper.GetAppConfig<bool>(_distributedCacheConfigName, false);

                if (_isConnectorProfilesCacheEnabled)
                {
                    _cacheManager = CacheFactory.GetCache();
                    if (_isDistributedCacheWithNotificationEnabled)
                    {
                        _cacheSynchronizationHelper = new CacheSynchronizationHelper();
                    }
                }

                if (_cacheManager == null)
                {
                    _isConnectorProfilesCacheEnabled = false;
                }
            }
            catch
            {
                _isConnectorProfilesCacheEnabled = false;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Finds all available Connector Profiles from cache
        /// </summary>
        /// <returns></returns>
        public ConnectorProfileCollection GetAllConnectorProfilesFromCache()
        {
            ConnectorProfileCollection profiles = null;

            if (IsConnectorProfilesCacheEnabled)
            {
                String cacheKey = CacheKeyGenerator.GetAllConnectorProfilesCacheKey();

                Object connectorProfilesObj = null;

                try
                {
                    connectorProfilesObj = _cacheManager.Get(cacheKey);
                }
                catch (Exception ex)
                {
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, String.Format("Error occurred : {0}", ex.Message), MDMTraceSource.Integration);
                }

                var connectorProfiles = connectorProfilesObj as ConnectorProfileCollection;
                if (connectorProfiles != null)
                {
                    ConnectorProfileCollection connectorProfileCollection = connectorProfiles;
                    profiles = (ConnectorProfileCollection) connectorProfileCollection.Clone();
                }
            }

            return profiles;
        }

        /// <summary>
        /// Removes all Connector Profiles from cache.
        /// </summary>
        /// <returns>True if object deleted successfully otherwise false.</returns>
        public Boolean RemoveConnectorProfilesFromCache()
        {
            Boolean success = false;

            if (IsConnectorProfilesCacheEnabled)
            {
                String connectorProfilesCacheKey = CacheKeyGenerator.GetAllConnectorProfilesCacheKey();

                if (!String.IsNullOrWhiteSpace(connectorProfilesCacheKey))
                {
                    try
                    {
                        success = _cacheManager.Remove(connectorProfilesCacheKey);

                        if (_isDistributedCacheWithNotificationEnabled)
                        {
                            _cacheSynchronizationHelper.NotifyLocalCacheRemoval(connectorProfilesCacheKey);
                        }
                    }
                    catch(Exception ex)
                    {
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, String.Format("Error occurred : {0}", ex.Message), MDMTraceSource.Integration);
                    }
                }
            }

            return success;
        }

        /// <summary>
        /// Updates Connector Profiles in Cache
        /// </summary>
        /// <param name="connectorProfiles"></param>
        /// <param name="numberOfRetry"></param>
        /// <param name="isDataModified"></param>
        public void UpdateConnectorProfilesInCache(ConnectorProfileCollection connectorProfiles, Int32 numberOfRetry = 3, Boolean isDataModified = false)
        {
            if (IsConnectorProfilesCacheEnabled)
            {
                if (connectorProfiles == null || !connectorProfiles.Any())
                {
                    throw new ArgumentException("Connector Profiles are not available or empty.");
                }

                String cacheKey = CacheKeyGenerator.GetAllConnectorProfilesCacheKey();

                DateTime expiryTime = DateTime.Now.AddMonths(5);

                //Clone the object before set to cache.
                var clonedConnectorProfiles = connectorProfiles.Clone() as ConnectorProfileCollection;

                try
                {
                    if (_isDistributedCacheWithNotificationEnabled)
                    {
                        _cacheSynchronizationHelper.NotifyLocalCacheInsert(cacheKey, expiryTime, isDataModified);
                    }

                    _cacheManager.Set(cacheKey, clonedConnectorProfiles, expiryTime);
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

                            _cacheManager.Set(cacheKey, clonedConnectorProfiles, expiryTime);

                            retrySuccess = true;
                            break;
                        }
                        catch(Exception innerException)
                        {
                            MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, String.Format("Not able to update Connector Profiles cache. Attempt {1}. Error occurred : {0}", innerException.Message, i), MDMTraceSource.Integration);
                        }
                    }

                    //throw exception if cache update retry is failed too..
                    if (!retrySuccess)
                    {
                        throw new ApplicationException(String.Format("Not able to update Connector Profiles cache due to internal error. Error: {0}", ex.Message));
                    }
                }
            }
        }

        #endregion
    }
}
