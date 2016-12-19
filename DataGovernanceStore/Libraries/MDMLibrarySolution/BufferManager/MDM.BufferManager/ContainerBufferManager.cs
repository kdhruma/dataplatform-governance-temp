using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace MDM.BufferManager
{    
    using MDM.BusinessObjects;
    using MDM.CacheManager.Business;
    using MDM.Core;
    using MDM.ExceptionManager;
    using MDM.Utility;

    /// <summary>
    /// Specifies Container buffer manager
    /// </summary>
    public class ContainerBufferManager : BusinessLogicBase
    {
        #region Fields

        /// <summary>
        /// field denoting cache manager of cache
        /// </summary>
        private ICache _cacheManager = null;

        /// <summary>
        /// field denoting container cache is enabled or not.
        /// </summary>
        private Boolean _isContainerCacheEnabled = false;

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
        /// Instantiate Container Buffer Manager
        /// </summary>
        public ContainerBufferManager()
        {
            try
            {
                //Get AppConfig which specify whether cache is enabled for container or not
                String strIsCacheEnabled = AppConfigurationHelper.GetAppConfig<String>("MDMCenter.ContainerManager.ContainerCache.Enabled", "true");
                Boolean isCacheEnabled = false;
                Boolean.TryParse(strIsCacheEnabled, out isCacheEnabled);

                this._isContainerCacheEnabled = isCacheEnabled;
                this._isDistributedCacheWithNotificationEnabled = AppConfigurationHelper.GetAppConfig<Boolean>("MDMCenter.DistributedCacheWithNotification.Enabled", false);

                if (_isContainerCacheEnabled)
                {
                    _cacheManager = CacheFactory.GetCache();
                    if (_isDistributedCacheWithNotificationEnabled)
                        _cacheSynchronizationHelper = new CacheSynchronizationHelper();
                }

                if (_cacheManager == null)
                    this._isContainerCacheEnabled = false;
            }
            catch
            {
                this._isContainerCacheEnabled = false;
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Property denoting container cache is enabled or not.
        /// </summary>
        public Boolean IsContainerCacheEnabled
        {
            get
            {
                return _isContainerCacheEnabled;
            }
        }

        #endregion

        #region Methods

        #region Public Methods

        /// <summary>
        /// Finds all available containers from cache
        /// </summary>
        /// <returns>collection of containers if found in internal cache otherwise null</returns>
        public Dictionary<Int32, Container> FindAllContainers()
        {
            Dictionary<Int32, Container> containers = null;

            if (this._isContainerCacheEnabled)
            {
                String cacheKey = CacheKeyGenerator.GetAllContainersCacheKey();

                object containerObj = null;

                try
                {
                    containerObj = _cacheManager.Get(cacheKey);
                }
                catch (Exception ex)
                {
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, String.Format("Error occurred : {0}", ex.Message));
                    ExceptionHandler exceptionHandler = new ExceptionHandler(ex);
                }

                if (containerObj != null && containerObj is Dictionary<Int32, Container>)
                {
                    containers = (Dictionary<Int32, Container>)containerObj;                    
                }
            }

            return containers;
        }

        /// <summary>
        /// Finds all available containers with Attributes from cache
        /// </summary>
        /// <returns>collection of containers if found in internal cache otherwise null</returns>
        public Dictionary<Int32, Container> FindAllContainersWithAttributes()
        {
            Dictionary<Int32, Container> containers = null;

            if (this._isContainerCacheEnabled)
            {
                String cacheKey = CacheKeyGenerator.GetAllContainersWithAttributesCacheKey();

                object containerObj = null;

                try
                {
                    containerObj = _cacheManager.Get(cacheKey);
                }
                catch (Exception ex)
                {
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, String.Format("Error occurred : {0}", ex.Message));
                    ExceptionHandler exceptionHandler = new ExceptionHandler(ex);
                }

                if (containerObj != null && containerObj is Dictionary<Int32, Container>)
                {
                    containers = (Dictionary<Int32, Container>)containerObj;
                }
            }

            return containers;
        }

        /// <summary>
        /// Update containers in internal cache
        /// </summary>
        /// <param name="containers">This parameter is specifying collection of containers to be update in cache.</param>
        /// <param name="containerContext">context indicates the properties like load attributes or not</param>        
        /// <param name="numberOfRetry">This parameter is specifying number of retry to update object in cache.</param>
        /// <param name="isDataUpdated">Specifies whether the cache data object is updated and inserted to local cache.</param> 
        public void UpdateContainers(Dictionary<Int32, Container> containers, ContainerContext containerContext, Int32 numberOfRetry, Boolean isDataUpdated = false)
        {
            if (this._isContainerCacheEnabled)
            {
                if (containers == null || containers.Count < 1)
                    throw new ArgumentException("Containers are not available or empty.");

                String cacheKey;

                if (!containerContext.LoadAttributes)
                {
                    cacheKey = CacheKeyGenerator.GetAllContainersCacheKey();
                }
                else
                {
                    cacheKey = CacheKeyGenerator.GetAllContainersWithAttributesCacheKey();
                }

                DateTime expiryTime = DateTime.Now.AddMonths(5);

                Dictionary<Int32, Container> clonedContainers = containers; //Why to clone?

                try
                {
                    if (_isDistributedCacheWithNotificationEnabled)
                        _cacheSynchronizationHelper.NotifyLocalCacheInsert(cacheKey, expiryTime, isDataUpdated);

                    _cacheManager.Set(cacheKey, clonedContainers, expiryTime);                    
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

                            _cacheManager.Set(cacheKey, containers, expiryTime);
                            
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
        /// Removes all containers from cache.
        /// </summary>
        /// <returns>True if object deleted successfully otherwise false.</returns>
        public Boolean RemoveContainers()
        {
            Boolean success = false;
                
            if (this._isContainerCacheEnabled)
            {
                String cacheKeyOnlyContainers = CacheKeyGenerator.GetAllContainersCacheKey();
                String cacheKeyContainersWithAttributes = CacheKeyGenerator.GetAllContainersWithAttributesCacheKey();

                try
                {
                    success = _cacheManager.Remove(cacheKeyOnlyContainers);
                    success = _cacheManager.Remove(cacheKeyContainersWithAttributes);

                    if (_isDistributedCacheWithNotificationEnabled)
                    {
                        _cacheSynchronizationHelper.NotifyLocalCacheRemoval(cacheKeyOnlyContainers);
                        _cacheSynchronizationHelper.NotifyLocalCacheRemoval(cacheKeyContainersWithAttributes);
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

        #endregion

        #region Private Methods

        #endregion

        #endregion
    }
}