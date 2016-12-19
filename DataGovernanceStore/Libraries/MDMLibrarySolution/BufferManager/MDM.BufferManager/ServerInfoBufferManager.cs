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
    /// Represents class for caching server related information
    /// </summary>
    public class ServerInfoBufferManager : BusinessLogicBase
    {
        #region Fields 

        /// <summary>
        /// field denoting cache manager of cache
        /// </summary>
        private ICache _cacheManager = null;

        /// <summary>
        /// field denoting entity history cache is enabled or not.
        /// </summary>
        private Boolean _isServerInfoCacheEnabled = false;

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
        /// Instantiate server info Buffer Manager
        /// </summary>
        public ServerInfoBufferManager()
        {
            try
            {
               
                this._isDistributedCacheWithNotificationEnabled = AppConfigurationHelper.GetAppConfig<Boolean>("MDMCenter.DistributedCacheWithNotification.Enabled", true);
                this._isServerInfoCacheEnabled = AppConfigurationHelper.GetAppConfig<Boolean>("MDMCenter.MonitoringManager.ServerInfoCache.Enabled", true);

                if (_isServerInfoCacheEnabled)
                {
                    _cacheManager = CacheFactory.GetCache();
                
                     if (_isDistributedCacheWithNotificationEnabled)
                        _cacheSynchronizationHelper = new CacheSynchronizationHelper();                        
                }
            
                if (_cacheManager == null)
                    this._isServerInfoCacheEnabled = false;
            }
            catch
            {
                this._isServerInfoCacheEnabled = false;
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Property denoting server info cache is enabled or not.
        /// </summary>
        public Boolean IsServerInfoCacheEnabled
        {
            get
            {
                return _isServerInfoCacheEnabled;
            }
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ServerInfoCollection FindServerInfo()
        {
            ServerInfoCollection serverInfos = null;

            if (this.IsServerInfoCacheEnabled)
            {
                String cacheKey = CacheKeyGenerator.GetServerInfoCacheKey();

                try
                {
                    serverInfos = _cacheManager.Get <ServerInfoCollection>(cacheKey);
                }
                catch (Exception ex)
                {
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, String.Format("Error occurred : {0}", ex.Message));
                    ExceptionHandler exceptionHandler = new ExceptionHandler(ex);
                }
            }

            return serverInfos;
        }

        /// <summary>
        /// Updates sever information based on collection of server information passed as parameter
        /// </summary>
        /// <param name="serverInfos">Indicates collection of server information</param>
        /// <param name="numberOfRetry">Indicates number of retry allowed to update server information</param>
        /// <param name="isDataUpdated">Indicates true if data is updated; otherwise false</param>
        public void UpdateServerInfo(ServerInfoCollection serverInfos, Int32 numberOfRetry, Boolean isDataUpdated = false)
        {
            if (this._isServerInfoCacheEnabled)
            {
                if (serverInfos == null || serverInfos.Count < 1)
                {
                    throw new ArgumentException("Server info collection is null");
                }

                String cacheKey = CacheKeyGenerator.GetServerInfoCacheKey();
                DateTime expiryTime = DateTime.Now.AddDays(5);

                try
                {
                    if (_isDistributedCacheWithNotificationEnabled)
                    {
                        _cacheSynchronizationHelper.NotifyLocalCacheInsert(cacheKey, expiryTime, isDataUpdated);
                    }

                    _cacheManager.Set<ServerInfoCollection>(cacheKey, serverInfos, expiryTime);
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
                                _cacheSynchronizationHelper.NotifyLocalCacheInsert(cacheKey, expiryTime, isDataUpdated);
                            }

                            _cacheManager.Set<ServerInfoCollection>(cacheKey, serverInfos, expiryTime);

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

    }
}
