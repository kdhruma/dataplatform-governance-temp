using System;
using System.Collections.Generic;

namespace MDM.PermissionManager.Business
{
    using MDM.BusinessObjects;
    using MDM.CacheManager.Business;
    using MDM.Core;
    using MDM.ExceptionManager;
    using MDM.Utility;

    /// <summary>
    /// Specifies Permission Buffer Manager
    /// </summary>
    public class PermissionBufferManager : BusinessLogicBase
    {
        #region Fields
        /// <summary>
        /// field denoting cache manager of distributed cache
        /// </summary>
        private IDistributedCache _distributedCacheManager = null;

        /// <summary>
        /// Indicates instance of ICache
        /// </summary>
        private ICache _cacheManager = null;

        /// <summary>
        /// Indicates whether Permission cache is enabled or not.
        /// </summary>
        private Boolean _isPermissionCacheEnabled = false;

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
        /// Property denoting Permission cache is enabled or not.
        /// </summary>
        public Boolean IsPermissionCacheEnabled
        {
            get
            {
                return _isPermissionCacheEnabled;
            }
        }

        /// <summary>
        /// Property denoting locale cache is enabled or not.
        /// </summary>
        public Boolean IsDistributedCacheWithNotificationEnabled
        {
            get
            {
                return _isDistributedCacheWithNotificationEnabled;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Permission Buffer Manager Constructor
        /// </summary>
        public PermissionBufferManager()
        {
            try
            {
                //Get AppConfig which specify whether cache is enabled for permissions or not
                this._isPermissionCacheEnabled = AppConfigurationHelper.GetAppConfig<Boolean>("MDMCenter.PermissionManager.PermissionCache.Enabled", false);
                this._isDistributedCacheWithNotificationEnabled = AppConfigurationHelper.GetAppConfig<Boolean>("MDMCenter.DistributedCacheWithNotification.Enabled", false);

                if (IsPermissionCacheEnabled)
                {
                    if (IsDistributedCacheWithNotificationEnabled)
                    {
                        _cacheSynchronizationHelper = new CacheSynchronizationHelper();
                        _cacheManager = CacheFactory.GetCache();

                        this._isPermissionCacheEnabled = _cacheManager != null ? true : false;
                    }
                    else
                    {
                        _distributedCacheManager = CacheFactory.GetDistributedCache();
                        this._isPermissionCacheEnabled = _distributedCacheManager != null ? true : false;
                    }
                }
            }
            catch
            {
                this._isPermissionCacheEnabled = false;
                //TODO:: HANDLE CACHE DISABLE SCENARIOS
            }
        }

        #endregion

        #region Methods

        #region Public Methods

        /// <summary>
        /// Find Security Permission Definitions in Distributed Cache if available.
        /// </summary>
        /// <param name="securityRole">Indicates security role object</param>
        /// <returns>Returns Security Permission Definition Collection</returns>
        public SecurityPermissionDefinitionCollection FindSecurityPermissionDefinitions(SecurityRole securityRole)
        {
            Boolean isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;
            if (isTracingEnabled)
                MDMTraceHelper.StartTraceActivity("PermissionBufferManager.FindSecurityPermissionDefinitions", false);

            SecurityPermissionDefinitionCollection securityPermissionDefinitionCollection = null;

            try
            {
                if (IsPermissionCacheEnabled)
                {
                    String cacheKey = CacheKeyGenerator.GetSecurityPermissionDefinitionCacheKey(securityRole.Id);

                    Object securityPermissionDefinitions = null;

                    try
                    {
                        if (IsDistributedCacheWithNotificationEnabled)
                        {
                            securityPermissionDefinitions = _cacheManager.Get(cacheKey);
                        }
                        else
                        {
                            securityPermissionDefinitions = _distributedCacheManager.Get(cacheKey);
                        }
                    }
                    catch
                    {
                        //What to do here??
                    }

                    if (securityPermissionDefinitions != null && securityPermissionDefinitions is SecurityPermissionDefinitionCollection)
                    {
                        securityPermissionDefinitionCollection = (SecurityPermissionDefinitionCollection)securityPermissionDefinitions;
                    }
                }
            }

            finally
            {
                if (isTracingEnabled)
                    MDMTraceHelper.StopTraceActivity("PermissionBufferManager.FindSecurityPermissionDefinitions");
            }

            return securityPermissionDefinitionCollection;
        }

        /// <summary>
        /// Update Security Permission Definition in distributed cache.
        /// </summary>
        /// <param name="securityPermissionDefinitions">Indicates Security Permission Definition Collection </param>
        /// <param name="roleId">Indicates role identifier</param>
        /// <param name="numberOfRetry">Indicates number of retry</param>
        public void UpdateSecurityPermissionDefinitions(SecurityPermissionDefinitionCollection securityPermissionDefinitions, Int32 roleId, Int32 numberOfRetry)
        {
            Boolean isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;
            if (isTracingEnabled)
                MDMTraceHelper.StartTraceActivity("PermissionBufferManager.UpdateSecurityPermissionDefinitions", false);

            try
            {
                if (IsPermissionCacheEnabled)
                {
                    String cacheKey = CacheKeyGenerator.GetSecurityPermissionDefinitionCacheKey(roleId);

                    List<String> dataCacheTags = new List<String>();
                    dataCacheTags.Add("RoleId:" + roleId);

                    try
                    {
                        UpdateSecurityPermissionCache(cacheKey, securityPermissionDefinitions);
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
                                UpdateSecurityPermissionCache(cacheKey, securityPermissionDefinitions);

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
                            ExceptionHandler exceptionHandler = new ExceptionHandler(ex);
                        }
                    }
                }
            }

            finally
            {
                if (isTracingEnabled)
                    MDMTraceHelper.StopTraceActivity("PermissionBufferManager.UpdateSecurityPermissionDefinitions");
            }
        }

        /// <summary>
        /// Removes SecurityPermissionDefinitions for the requested roleId from the cache
        /// </summary>
        /// <param name="roleId">RoleId for which object needs to be removed</param>
        /// <returns>Result which says whether operation is successful or not</returns>
        public Boolean RemoveSecurityPermissionDefinitions(Int32 roleId)
        {
            Boolean success = true;
            Boolean isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;

            if (isTracingEnabled)
                MDMTraceHelper.StartTraceActivity("PermissionBufferManager.RemoveSecurityPermissionDefinitions", false);

            try
            {
                if (IsPermissionCacheEnabled)
                {
                    //Get cache key
                    String cacheKey = CacheKeyGenerator.GetSecurityPermissionDefinitionCacheKey(roleId);

                    if (_isDistributedCacheWithNotificationEnabled)
                    {
                        _cacheSynchronizationHelper.NotifyLocalCacheRemoval(cacheKey);
                        success = _cacheManager.Remove(cacheKey);
                    }
                    else
                    {
                        success = _distributedCacheManager.Remove(cacheKey);
                    }
                }
            }
            finally
            {
                if (isTracingEnabled)
                    MDMTraceHelper.StopTraceActivity("PermissionBufferManager.RemoveSecurityPermissionDefinitions");
            }

            return success;
        }

        #endregion

        #region Private Methods

        private void UpdateSecurityPermissionCache(String cacheKey, SecurityPermissionDefinitionCollection securityPermissionDefinitions, Boolean isDataModified = false)
        {
            DateTime expiryTime = DateTime.Now.AddDays(5);

            if (IsDistributedCacheWithNotificationEnabled)
            {
                _cacheSynchronizationHelper.NotifyLocalCacheInsert(cacheKey, expiryTime, isDataModified);
                _cacheManager.Set(cacheKey, securityPermissionDefinitions, expiryTime);
               
            }
            else
            {
                _distributedCacheManager.Set(cacheKey, securityPermissionDefinitions, expiryTime);
            }
        }

        #endregion
        #endregion
    }
}
