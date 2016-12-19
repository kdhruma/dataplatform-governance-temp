using System;
using System.Diagnostics;
using System.Linq;


namespace MDM.BufferManager
{
    using MDM.BusinessObjects;
    using MDM.BusinessObjects.Exports;
    using MDM.CacheManager.Business;
    using MDM.Core;
    using MDM.ExceptionManager;
    using MDM.Utility;
    
    /// <summary>
    /// Export Profile Buffer Manager
    /// </summary>
    public class ExportProfileBufferManager : BusinessLogicBase
    {
        #region Fields

        /// <summary>
        /// 
        /// </summary>
        private readonly bool _isProfilesCacheEnabled;

        /// <summary>
        /// 
        /// </summary>
        private readonly ICache _cacheManager;

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
        /// Instantiate ExportProfiles Buffer Manager
        /// </summary>
        public ExportProfileBufferManager()
        {
            try
            {
                // Get AppConfig which specify whether cache is enabled for ExportProfile or not
                this._isProfilesCacheEnabled = AppConfigurationHelper.GetAppConfig("MDMCenter.ExportProfileManager.ExportProfileCache.Enabled", true);
                this._isDistributedCacheWithNotificationEnabled = AppConfigurationHelper.GetAppConfig<Boolean>("MDMCenter.DistributedCacheWithNotification.Enabled", false);

                if (_isProfilesCacheEnabled)
                {
                    _cacheManager = CacheFactory.GetCache();
                    if (_isDistributedCacheWithNotificationEnabled)
                        _cacheSynchronizationHelper = new CacheSynchronizationHelper();
                }

                if (_cacheManager == null)
                {
                    _isProfilesCacheEnabled = false;
                }
            }
            catch (Exception exception)
            {
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, String.Format("Error occurred : {0}", exception.Message));
                new ExceptionHandler(exception);
                _isProfilesCacheEnabled = false;
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Property denoting ExportProfile cache is enabled or not.
        /// </summary>
        public Boolean IsProfilesCacheEnabled
        {
            get
            {
                return this._isProfilesCacheEnabled;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets all export profiles
        /// </summary>
        /// <returns>Returns collection of export profile</returns>
        public ExportProfileCollection FindAllExportProfiles()
        {
            ExportProfileCollection result = null;

            if (this._isProfilesCacheEnabled)
            {
                String cacheKey = CacheKeyGenerator.GetAllExportProfilesCacheKey();

                object exportProfiles = null;

                try
                {
                    exportProfiles = _cacheManager.Get(cacheKey);
                }
                catch (Exception ex)
                {
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, String.Format("Error occurred : {0}", ex.Message));
                    new ExceptionHandler(ex);
                }

                ExportProfileCollection exportProfileCollection = exportProfiles as ExportProfileCollection;

                if (exportProfileCollection != null)
                {
                    result = (ExportProfileCollection)exportProfileCollection; //.Clone();
                }
            }

            return result;
        }

        /// <summary>
        /// Updates export profiles from given export profile collection
        /// </summary>
        /// <param name="exportProfileCollection">Indicates collection of exportProfile</param>
        /// <param name="numberOfRetry">Indicates number of retry allowed to update export profile</param>
        /// <param name="isDataUpdated">Indicates true if data is updated; otherwise false</param>
        public void UpdateExportProfiles(ExportProfileCollection exportProfileCollection, Int32 numberOfRetry, Boolean isDataUpdated = false)
        {
            if (_isProfilesCacheEnabled)
            {
                if (exportProfileCollection == null || !exportProfileCollection.Any())
                {
                    throw new ArgumentException("ExportProfiles are not available or empty.");
                }

                String cacheKey = CacheKeyGenerator.GetAllExportProfilesCacheKey();

                DateTime expiryTime = DateTime.Now.AddMonths(5);

                // Clone the object before set to cache.
                ExportProfileCollection clonedProfileCollection = (ExportProfileCollection)exportProfileCollection; //.Clone();

                try
                {
                    if (_isDistributedCacheWithNotificationEnabled)
                        _cacheSynchronizationHelper.NotifyLocalCacheInsert(cacheKey, expiryTime, isDataUpdated);

                    _cacheManager.Set(cacheKey, clonedProfileCollection, expiryTime);
                }
                catch (Exception ex)
                {
                    // Retry < 0 means just ignore update if failed..
                    if (numberOfRetry < 0)
                    {
                        return;
                    }

                    bool retrySuccess = false;

                    for (int i = 0; i < numberOfRetry; i++)
                    {
                        try
                        {
                            _cacheManager.Set(cacheKey, clonedProfileCollection, expiryTime);
                            retrySuccess = true;
                            break;
                        }
                        catch 
                        {
                        }
                    }

                    if (!retrySuccess)
                    {
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, String.Format("Error occurred : {0}", ex.Message));
                        new ExceptionHandler(ex);
                    }
                }
            }
        }


        /// <summary>
        /// Get all Export Data Formatters from cache
        /// </summary>
        /// <returns></returns>
        public ExportDataFormatterCollection FindAllExportDataFormatters()
        {
            ExportDataFormatterCollection result = null;

            if (this._isProfilesCacheEnabled)
            {
                String cacheKey = CacheKeyGenerator.GetAllExportDataFormattersCacheKey();

                object exportDataFormatters = null;

                try
                {
                    exportDataFormatters = _cacheManager.Get(cacheKey);
                }
                catch (Exception ex)
                {
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, String.Format("Error occurred : {0}", ex.Message));
                    new ExceptionHandler(ex);
                }

                ExportDataFormatterCollection exportDataFormatterCollection = exportDataFormatters as ExportDataFormatterCollection;

                if (exportDataFormatterCollection != null)
                {
                    result = exportDataFormatterCollection; //.Clone();
                }
            }

            return result;
        }

        /// <summary>
        /// Updates the exoirt formatters
        /// </summary>
        /// <param name="exportFormatterCollection"></param>
        /// <param name="numberOfRetry"></param>
        /// <param name="isDataUpdated"></param>
        public void UpdateExportFormatters(ExportDataFormatterCollection exportFormatterCollection, Int32 numberOfRetry, Boolean isDataUpdated = false)
        {
            if (_isProfilesCacheEnabled)
            {
                if (exportFormatterCollection == null || !exportFormatterCollection.Any())
                {
                    throw new ArgumentException("ExportDataFormatters are not available or empty.");
                }

                String cacheKey = CacheKeyGenerator.GetAllExportDataFormattersCacheKey();

                DateTime expiryTime = DateTime.Now.AddMonths(5);

                // Clone the object before set to cache.
                ExportDataFormatterCollection clonedFormatterCollection = exportFormatterCollection; //.Clone();

                try
                {
                    if (_isDistributedCacheWithNotificationEnabled)
                        _cacheSynchronizationHelper.NotifyLocalCacheInsert(cacheKey, expiryTime, isDataUpdated);

                    _cacheManager.Set(cacheKey, clonedFormatterCollection, expiryTime);
                }
                catch (Exception ex)
                {
                    // Retry < 0 means just ignore update if failed..
                    if (numberOfRetry < 0)
                    {
                        return;
                    }

                    bool retrySuccess = false;

                    for (int i = 0; i < numberOfRetry; i++)
                    {
                        try
                        {
                            _cacheManager.Set(cacheKey, clonedFormatterCollection, expiryTime);
                            retrySuccess = true;
                            break;
                        }
                        catch
                        {
                        }
                    }

                    if (!retrySuccess)
                    {
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, String.Format("Error occurred : {0}", ex.Message));
                        new ExceptionHandler(ex);
                    }
                }
            }
        }

        /// <summary>
        /// Removes export profiles based on publish cache change event
        /// </summary>
        /// <param name="publishCacheChangeEvent">Indicates a boolean value whether to publish cache change event or not.</param>
        /// <returns>Returns true if export profile is removed successfully; otherwise false.</returns>
        public Boolean RemoveExportProfiles(Boolean publishCacheChangeEvent)
        {
            Boolean success = false;

            if (this._isProfilesCacheEnabled)
            {
                String cacheKey = CacheKeyGenerator.GetAllExportProfilesCacheKey();

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
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, String.Format("Error occurred : {0}", ex.Message));
                    new ExceptionHandler(ex);
                }
            }

            return success;
        }


        /// <summary>
        /// Remove the cache entry
        /// </summary>
        /// <param name="publishCacheChangeEvent"></param>
        /// <returns></returns>
        public Boolean RemoveExportFormatters(Boolean publishCacheChangeEvent)
        {
            Boolean success = false;

            if (this._isProfilesCacheEnabled)
            {
                String cacheKey = CacheKeyGenerator.GetAllExportDataFormattersCacheKey();

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
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, String.Format("Error occurred : {0}", ex.Message));
                    new ExceptionHandler(ex);
                }
            }

            return success;
        }

        #endregion
    }
}
