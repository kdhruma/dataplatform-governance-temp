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
    /// Specifies Permission Buffer Manager
    /// </summary>
    public class SecurityRoleBufferManager : BusinessLogicBase
    {
        #region Fields

        /// <summary>
        /// Indicates instance of ICache
        /// </summary>
        private ICache _iCacheManager = null;

        /// <summary>
        /// field denoting cache manager of distributed cache
        /// </summary>
        private IDistributedCache _distributedCacheManager = null;

        /// <summary>
        /// Indicates whether security role cache is enabled or not.
        /// </summary>
        private Boolean _isSecurityRoleCacheEnabled = false;

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
        /// Property denoting SecurityRole cache is enabled or not.
        /// </summary>
        public Boolean IsSecurityRoleCacheEnabled
        {
            get
            {
                return _isSecurityRoleCacheEnabled;
            }
        }

        /// <summary>
        /// Property denoting Distributed cache notification is enabled or not.
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
        /// Security Role Buffer Manager Constructor
        /// </summary>
        public SecurityRoleBufferManager()
        {
            try
            {
                //Get AppConfig which specify whether cache is enabled for SecurityRole or not
                //Cannot get the AppConfig without making a DB call all the time.. Adding ApplicationServices reference is leading to circular reference..
                //TODO:: Think about how to get AppConfig value
                // String strIsCacheEnabled = "True"; //AppConfigurationHelper.GetAppConfig<String>("MDMCenter.AdminManager.SecurityRoleCache.Enabled");

                this._isDistributedCacheWithNotificationEnabled = MDM.Utility.AppConfigurationHelper.IsDistributedCacheWithNotificationEnabled; //AppConfigurationHelper.IsDistributedCacheWithNotificationEnabled;

                if (_isDistributedCacheWithNotificationEnabled)
                {
                    _cacheSynchronizationHelper = new CacheSynchronizationHelper();
                    _iCacheManager = CacheFactory.GetCache();

                    this._isSecurityRoleCacheEnabled = _iCacheManager != null ? true : false;
                }
                else
                {
                    _distributedCacheManager = CacheFactory.GetDistributedCache();
                    this._isSecurityRoleCacheEnabled = _distributedCacheManager != null ? true : false;
                }
            }
            catch
            {
                this._isSecurityRoleCacheEnabled = false;
                //TODO:: HANDLE CACHE DISABLE SCENARIOS
            }
        }

        #endregion

        #region Methods

        #region Public Methods

        /// <summary>
        /// Find user roles in Distributed Cache if available.
        /// </summary>
        ///<param name="userLoginName">Login Name of the user</param>
        /// <returns>Returns Security Permission Definition Collection</returns>
        public SecurityRoleCollection FindUserRoles(String userLoginName)
        {
            //if (Constants.TRACING_ENABLED)
            // MDMTraceHelper.StartTraceActivity("SecurityRoleBufferManager.FindUserRoles", false);

            SecurityRoleCollection securityRoles = null;

            try
            {
                if (this._isSecurityRoleCacheEnabled)
                {
                    String cacheKey = CacheKeyGenerator.GetUserRolesCacheKey(userLoginName);

                    Object roles = null;

                    try
                    {
                        if (IsDistributedCacheWithNotificationEnabled)
                        {
                            roles = _iCacheManager.Get(cacheKey);
                        }
                        else
                        {
                            roles = _distributedCacheManager.Get(cacheKey);
                        }
                    }
                    catch (Exception ex)
                    {
                        ExceptionHandler innerExHandler = new ExceptionHandler(ex);
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, ex.ToString());
                    }

                    if (roles != null && roles is SecurityRoleCollection)
                    {
                        securityRoles = (SecurityRoleCollection)roles;
                    }
                }
            }

            finally
            {
                // if (Constants.TRACING_ENABLED)
                // MDMTraceHelper.StopTraceActivity("SecurityRoleBufferManager.FindUserRoles");
            }

            return securityRoles;
        }

        /// <summary>
        /// Update user roles in distributed cache.
        /// </summary>
        ///<param name="securityRoles">The object which needs to be updated</param>
        ///<param name="userLoginName">The login name for which roles needs to be updated</param>
        /// <param name="numberOfRetry">Indicates number of retry</param>
        public void UpdateUserRoles(SecurityRoleCollection securityRoles, String userLoginName, Int32 numberOfRetry)
        {
            //if (Constants.TRACING_ENABLED)
            // MDMTraceHelper.StartTraceActivity("SecurityRoleBufferManager.UpdateUserRoles", false);

            try
            {
                if (this._isSecurityRoleCacheEnabled)
                {
                    String cacheKey = CacheKeyGenerator.GetUserRolesCacheKey(userLoginName);

                    try
                    {
                        UpdateUserRolesCache(cacheKey, securityRoles);
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
                                UpdateUserRolesCache(cacheKey, securityRoles);
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
                            MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, ex.ToString());
                        }
                    }
                }
            }

            finally
            {
                // if (Constants.TRACING_ENABLED)
                //  MDMTraceHelper.StopTraceActivity("SecurityRoleBufferManager.UpdateUserRoles");
            }
        }

        /// <summary>
        /// Removes SecurityRoles for the requested user name from the cache
        /// </summary>
        /// <param name="userLoginName">UserLogin for which object needs to be removed</param>
        /// <returns>Result which says whether operation is successful or not</returns>
        public Boolean RemoveUserRoles(String userLoginName)
        {
            Boolean success = true;

            //if (Constants.TRACING_ENABLED)
            //  MDMTraceHelper.StartTraceActivity("SecurityRoleBufferManager.RemoveUserRoles", false);

            try
            {
                if (IsSecurityRoleCacheEnabled)
                {
                    //Get cache key
                    String cacheKey = CacheKeyGenerator.GetUserRolesCacheKey(userLoginName);

                    if (IsDistributedCacheWithNotificationEnabled)
                    {
                        _cacheSynchronizationHelper.NotifyLocalCacheRemoval(cacheKey);
                        success = _iCacheManager.Remove(cacheKey);
                    }
                    else
                    {
                        success = _distributedCacheManager.Remove(cacheKey);
                    }
                }
            }
            finally
            {
                //if (Constants.TRACING_ENABLED)
                // MDMTraceHelper.StopTraceActivity("SecurityRoleBufferManager.RemoveUserRoles");
            }

            return success;
        }

        #endregion

        #region Private methods

        private void UpdateUserRolesCache(String cacheKey, SecurityRoleCollection securityRoles, Boolean isDataModified = false)
        {
            if (IsSecurityRoleCacheEnabled)
            {
                DateTime expiryTime = DateTime.Now.AddDays(5);

                if (IsDistributedCacheWithNotificationEnabled)
                {
                    _cacheSynchronizationHelper.NotifyLocalCacheInsert(cacheKey, expiryTime, isDataModified);
                    _iCacheManager.Set(cacheKey, securityRoles, expiryTime);
                }
                else
                {
                    _distributedCacheManager.Set(cacheKey, securityRoles, expiryTime);
                }
            }
        }

        #endregion

        #endregion
    }
}
