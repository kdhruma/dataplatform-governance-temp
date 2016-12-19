using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace MDM.BufferManager
{
    using MDM.BusinessObjects;
    using MDM.CacheManager.Business;
    using MDM.Core;
    using MDM.ExceptionManager;
    using MDM.Utility;

    /// <summary>
    /// Specifies Entity History Buffer Manager
    /// </summary>
    public class EntityHistoryBufferManager : BusinessLogicBase
    {
        #region Fields 

        /// <summary>
        /// field denoting cache manager of cache
        /// </summary>
        private ICache _cacheManager = null;

        /// <summary>
        /// field denoting entity history cache is enabled or not.
        /// </summary>
        private Boolean _isEntityHistoryCacheEnabled = false;

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
        /// Instantiate Entity history Buffer Manager
        /// </summary>
        public EntityHistoryBufferManager()
        {
            try
            {
               
                //TODO: If appconfig key is introduced for entityHistory key in the future, then need to read from the key.
                //Currently, no key is there as such, so by default assigning it as true.

                Boolean isCacheEnabled = true;
                

                this._isDistributedCacheWithNotificationEnabled = AppConfigurationHelper.GetAppConfig<Boolean>("MDMCenter.DistributedCacheWithNotification.Enabled", false);
                this._isEntityHistoryCacheEnabled = isCacheEnabled;

                if(_isEntityHistoryCacheEnabled)
                {
                    _cacheManager = CacheFactory.GetCache();
                
                     if (_isDistributedCacheWithNotificationEnabled)
                        _cacheSynchronizationHelper = new CacheSynchronizationHelper();                        
                }
            
                if (_cacheManager == null)
                    this._isEntityHistoryCacheEnabled = false;
            }
            catch
            {
                this._isEntityHistoryCacheEnabled = false;
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Property denoting entity history cache is enabled or not.
        /// </summary>
        public Boolean IsEntityHistoryCacheEnabled
        {
            get
            {
                return _isEntityHistoryCacheEnabled;
            }
        }

        #endregion

        /// <summary>
        /// Find Entity History Details template from cache.
        /// </summary>
        ///<param name="locale">Indicates the locale for which templates to be fetched from cache.</param>
        /// <returns>Returns collection of entity history details template for specific locale</returns>
        public EntityHistoryDetailsTemplateCollection FindEntityHistoryDetailsTemplate(LocaleEnum locale)
        {
            EntityHistoryDetailsTemplateCollection entityHistoryDetailsTemplates = null;

            if (this.IsEntityHistoryCacheEnabled)
            {
                String cacheKey = CacheKeyGenerator.GetEntityHistoryDetailsTemplateCacheKey(locale);

                object entityHistoryTemplateObj = null;

                try
                {
                    entityHistoryTemplateObj = _cacheManager.Get(cacheKey);
                }
                catch (Exception ex)
                {
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, String.Format("Error occurred : {0}", ex.Message));
                    ExceptionHandler exceptionHandler = new ExceptionHandler(ex);
                }

                if (entityHistoryTemplateObj != null && entityHistoryTemplateObj is EntityHistoryDetailsTemplateCollection)
                {
                    entityHistoryDetailsTemplates = (EntityHistoryDetailsTemplateCollection)entityHistoryTemplateObj;
                }
            }

            return entityHistoryDetailsTemplates;
        }

        /// <summary>
        /// Update entity history templates into cache
        /// </summary>
        /// <param name="entityHistoryDetailsTemplates">Indicates the template collection which to be updated in cache</param>
        /// <param name="locale">Indicates the locale for which cache to be updated</param> 
        /// <param name="numberOfRetry">Indicates the number of retry to be done to update data in cache.</param>
        /// <param name="isDataUpdated">Specifies whether to update the cache version in distributed cache. This flag will be used only 
        /// when the data is modified and saved to cache.</param>
        public void UpdateEntityHistoryDetailsTemplate(EntityHistoryDetailsTemplateCollection entityHistoryDetailsTemplates, LocaleEnum locale, Int32 numberOfRetry, Boolean isDataUpdated = false)
        {
            if (this._isEntityHistoryCacheEnabled)
            {
                if (entityHistoryDetailsTemplates == null)
                    throw new ArgumentException("EntityHistoryDetailsTemplates is null");

                String cacheKey = CacheKeyGenerator.GetEntityHistoryDetailsTemplateCacheKey(locale);

                DateTime expiryTime = DateTime.Now.AddDays(5);

                try
                {
                    if (_isDistributedCacheWithNotificationEnabled)
                        _cacheSynchronizationHelper.NotifyLocalCacheInsert(cacheKey, expiryTime, isDataUpdated);

                    _cacheManager.Set(cacheKey, entityHistoryDetailsTemplates, expiryTime);
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

                            _cacheManager.Set(cacheKey, entityHistoryDetailsTemplates, expiryTime);

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
