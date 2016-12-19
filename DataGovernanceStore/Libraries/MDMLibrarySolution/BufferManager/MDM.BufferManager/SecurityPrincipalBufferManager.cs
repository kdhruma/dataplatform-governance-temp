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
    /// Class performs caching operations for SecurityPrincipal object.
    /// </summary>
    public class SecurityPrincipalBufferManager
    {
        #region Fields

        /// <summary>
        /// Field denotes an instance of cache manager.
        /// </summary>
        private ICache _cacheManager = null;

        /// <summary>
        /// Field denotes if security principal cache is enabled or not.
        /// </summary>
        private Boolean _isSecurityPrincipalCacheEnabled = false;

        /// <summary>
        /// Field denotes an instance of the CacheSynchronizationHelper.
        /// </summary>
        private CacheSynchronizationHelper _cacheSynchronizationHelper = null;

        #endregion

        #region Constructors

        /// <summary>
        /// Instantiate SecurityPrincipal Buffer Manager
        /// </summary>
        public SecurityPrincipalBufferManager()
        {
            try
            {
                //Get AppConfig which specify whether cache is enabled for SecurityPrincipal or not
                //Cannot get the AppConfig without making a DB call all the time.. Adding ApplicationServices reference is leading to circular reference..
                //TODO:: Think about how to get AppConfig value
                this._isSecurityPrincipalCacheEnabled = true;
                if (_isSecurityPrincipalCacheEnabled)
                {
                    _cacheManager = CacheFactory.GetCache();
                    _cacheSynchronizationHelper = new CacheSynchronizationHelper();
                }

                if (_cacheManager == null)
                    this._isSecurityPrincipalCacheEnabled = false;
            }
            catch
            {
                this._isSecurityPrincipalCacheEnabled = false;
            }
        }

        #endregion

        #region Properties

        #endregion

        #region Methods

        /// <summary>
        /// Returns the SecurityPrincipal from cache based on the cache key.
        /// </summary>
        /// <param name="userName">Username for which the SecurityPrincipal is to be retrieved.</param>
        /// <returns>SecurityPrincipal object from cache</returns>
        public SecurityPrincipal GetSecurityPrincipal(String userName)
        {
            SecurityPrincipal securityPrincipal = null;

            if (this._isSecurityPrincipalCacheEnabled)
            {
                String cacheKey = CacheKeyGenerator.GetSecurityPrincipalCacheKey(userName);
                try
                {
                    securityPrincipal = _cacheManager.Get<SecurityPrincipal>(cacheKey);
                }
                catch (Exception ex)
                {
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, String.Format("Error occurred : {0}", ex.Message));
                    ExceptionHandler exceptionHandler = new ExceptionHandler(ex);
                }
            }

            return securityPrincipal;
        }

        /// <summary>
        /// Updates SecurityPrincipal in cache.
        /// </summary>
        /// <param name="securityPrincipal">The SecurityPrincipal which needs to be updated in cache.</param>
        /// <param name="numberOfRetry">Indicates number of retry</param>
        /// <param name="isDataUpdated">Specifies whether to update the cache version in distributed cache. This flag will be used only when the data is 
        /// modified and saved to cache.</param>
        public void UpdateSecurityPrincipal(SecurityPrincipal securityPrincipal, Int32 numberOfRetry, Boolean isDataUpdated = false)
        {
            if (this._isSecurityPrincipalCacheEnabled)
            {
                if (securityPrincipal == null)
                    throw new ArgumentException("SecurityPrincipal is not available or empty.");

                String cacheKey = CacheKeyGenerator.GetSecurityPrincipalCacheKey(securityPrincipal.CurrentUserName);

                Int32 userSessionTimeout = AppConfiguration.GetSettingAsInteger("UserSessionTimeOut");

                try
                {
                    _cacheSynchronizationHelper.NotifyLocalCacheInsert(cacheKey, userSessionTimeout, isDataUpdated);

                    _cacheManager.Set(cacheKey, securityPrincipal, userSessionTimeout);
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
                            _cacheSynchronizationHelper.NotifyLocalCacheInsert(cacheKey, userSessionTimeout, isDataUpdated);

                            _cacheManager.Set(cacheKey, securityPrincipal, userSessionTimeout);

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
        /// Removes SecurityPrincipal for the requested user name from the cache.
        /// </summary>
        /// <param name="userName">UserLogin for which object needs to be removed</param>
        /// <returns>Result which says whether operation is successful or not</returns>
        public Boolean RemoveSecurityPrincipal(String userName)
        {
            Boolean success = false;

            if (this._isSecurityPrincipalCacheEnabled)
            {
                String cacheKey = CacheKeyGenerator.GetSecurityPrincipalCacheKey(userName);

                try
                {
                    success = _cacheManager.Remove(cacheKey);

                    _cacheSynchronizationHelper.NotifyLocalCacheRemoval(cacheKey);
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
    }
}
