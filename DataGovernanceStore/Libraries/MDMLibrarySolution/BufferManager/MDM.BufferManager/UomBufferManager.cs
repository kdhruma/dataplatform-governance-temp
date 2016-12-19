using System;
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
    /// Specifies UOM buffer manager
    /// </summary>
    public class UomBufferManager : BusinessLogicBase
    {
        #region Fields

        /// <summary>
        /// field denoting cache manager of cache
        /// </summary>
        private ICache _cacheManager = null;

        /// <summary>
        /// field denoting container cache is enabled or not.
        /// </summary>
        private Boolean _isUomCacheEnabled = false;

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
        /// Instantiate UOM Buffer Manager
        /// </summary>
        public UomBufferManager()
        {
            try
            {
                //Get AppConfig which specify whether cache is enabled for container or not
                String strIsCacheEnabled = AppConfigurationHelper.GetAppConfig<String>("MDMCenter.UOMManager.UOMCache.Enabled", "true");
                Boolean isCacheEnabled = false;
                Boolean.TryParse(strIsCacheEnabled, out isCacheEnabled);

                this._isUomCacheEnabled = isCacheEnabled;
                this._isDistributedCacheWithNotificationEnabled = AppConfigurationHelper.GetAppConfig<Boolean>("MDMCenter.DistributedCacheWithNotification.Enabled", false);

                if (_isUomCacheEnabled)
                {
                    _cacheManager = CacheFactory.GetCache();
                    if (_isDistributedCacheWithNotificationEnabled)
                        _cacheSynchronizationHelper = new CacheSynchronizationHelper();
                }

                if (_cacheManager == null)
                    this._isUomCacheEnabled = false;
            }
            catch
            {
                this._isUomCacheEnabled = false;
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Property denoting Uom cache is enabled or not.
        /// </summary>
        public Boolean IsUomCacheEnabled
        {
            get
            {
                return _isUomCacheEnabled;
            }
        }

        #endregion

        #region Methods

        #region Public Methods

        /// <summary>
        /// Finds all available UOMS from cache
        /// </summary>
        /// <returns>collection of UOMs if found in internal cache otherwise null</returns>
        public UOMCollection FindAllUoms()
        {
            UOMCollection uoms = null;

            if (this.IsUomCacheEnabled)
            {
                String cacheKey = CacheKeyGenerator.GetAllUomsCacheKey();

                object uomsObj = null;

                try
                {
                    uomsObj = _cacheManager.Get(cacheKey);
                }
                catch (Exception ex)
                {
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, String.Format("Error occurred : {0}", ex.Message));
                    ExceptionHandler exceptionHandler = new ExceptionHandler(ex);
                }

                if (uomsObj != null && uomsObj is UOMCollection)
                {
                    uoms = (UOMCollection)uomsObj;
                }
            }

            return uoms;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="uoms"></param>
        /// <param name="uomContext"></param>
        /// <param name="numberOfRetry"></param>
        /// <param name="isDataUpdated"></param>
        public void UpdateUoms(UOMCollection uoms, UomContext uomContext, Int32 numberOfRetry, Boolean isDataUpdated = false)
        {
            if (this.IsUomCacheEnabled)
            {
                if (uoms == null && uoms.Count < 1)
                    throw new ArgumentException("Uoms are not available or empty.");

                String cacheKey= CacheKeyGenerator.GetAllUomsCacheKey();
              

                DateTime expiryTime = DateTime.Now.AddMonths(5);

                try
                {
                    if (_isDistributedCacheWithNotificationEnabled)
                        _cacheSynchronizationHelper.NotifyLocalCacheInsert(cacheKey, expiryTime, isDataUpdated);

                    _cacheManager.Set(cacheKey, uoms, expiryTime);
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

                            _cacheManager.Set(cacheKey, uoms, expiryTime);

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

        #endregion

        #region Private Methods

        #endregion

        #endregion
    }
}