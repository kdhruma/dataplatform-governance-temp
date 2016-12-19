using System;
using System.Diagnostics;
using System.Linq;

namespace MDM.BufferManager
{
    using MDM.BusinessObjects;
    using MDM.CacheManager.Business;
    using MDM.Core;
    using MDM.ExceptionManager;
    using MDM.Utility;
    using MDM.BusinessObjects.DQM;

    /// <summary>
    /// Specifies ValidationProfile buffer manager
    /// </summary>
    public class ValidationProfileBufferManager : BusinessLogicBase
    {
        #region Fields

        /// <summary>
        /// Field denoting cache manager of cache
        /// </summary>
        private ICache _cacheManager = null;

        /// <summary>
        /// Field denoting validation profile cache is enabled or not
        /// </summary>
        private Boolean _isValidationProfileCacheEnabled = false;

        /// <summary>
        /// Field denotes whether the Distributed Cache with notification has been enabled
        /// </summary>
        private Boolean _isDistributedCacheWithNotificationEnabled = false;

        /// <summary>
        /// Field denotes an instance of the CacheSynchronizationHelper
        /// </summary>
        private CacheSynchronizationHelper _cacheSynchronizationHelper = null;

        #endregion

        #region Constructors

        /// <summary>
        /// Instantiate ValidationProfile Buffer Manager
        /// </summary>
        public ValidationProfileBufferManager()
        {
            try
            {
                //Get AppConfig which specify whether cache is enabled for validation profile or not
                this._isValidationProfileCacheEnabled = AppConfigurationHelper.GetAppConfig<Boolean>("MDMCenter.DataQualityManagement.ValidationProfileCache.Enabled", true);
                this._isDistributedCacheWithNotificationEnabled = AppConfigurationHelper.GetAppConfig<Boolean>("MDMCenter.DistributedCacheWithNotification.Enabled", false);

                if (_isValidationProfileCacheEnabled)
                {
                    _cacheManager = CacheFactory.GetCache();
                    if (_isDistributedCacheWithNotificationEnabled)
                        _cacheSynchronizationHelper = new CacheSynchronizationHelper();
                }

                if (_cacheManager == null)
                    this._isValidationProfileCacheEnabled = false;
            }
            catch
            {
                this._isValidationProfileCacheEnabled = false;
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Property denoting validation profile cache is enabled or not
        /// </summary>
        public Boolean IsValidationProfileCacheEnabled
        {
            get
            {
                return _isValidationProfileCacheEnabled;
            }
        }

        #endregion

        #region Methods

        #region Public Methods

        /// <summary>
        /// Finds all available validation profiles from cache
        /// </summary>
        /// <returns>Collection of validation profiles if found in internal cache otherwise null</returns>
        public ValidationProfilesCollection FindAllValidationProfiles()
        {
            ValidationProfilesCollection result = null;

            if (this._isValidationProfileCacheEnabled)
            {
                String cacheKey = CacheKeyGenerator.GetAllValidationProfilesCacheKey();

                object validationProfilesObj = null;

                try
                {
                    validationProfilesObj = _cacheManager.Get(cacheKey);
                }
                catch (Exception ex)
                {
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, String.Format("Error occurred : {0}", ex.Message));
                    new ExceptionHandler(ex);
                }

                ValidationProfilesCollection validationProfilesCollection = validationProfilesObj as ValidationProfilesCollection;
                if (validationProfilesCollection != null)
                {
                    result = (ValidationProfilesCollection)validationProfilesCollection.Clone();
                }
            }

            return result;
        }

        /// <summary>
        /// Update ValidationProfile in internal cache
        /// </summary>
        /// <param name="validationProfiles">This parameter is specifying collection of Validation Profiles to be update in cache</param>
        /// <param name="numberOfRetry">This parameter is specifying number of retry to update object in cache</param>
        /// <param name="isDataUpdated">Specifies whether the cache data object is updated and inserted to local cache</param> 
        public void UpdateValidationProfiles(ValidationProfilesCollection validationProfiles, Int32 numberOfRetry, Boolean isDataUpdated = false)
        {
            if (this._isValidationProfileCacheEnabled)
            {
                if (validationProfiles == null || !validationProfiles.Any())
                    throw new ArgumentException("ValidationProfiles are not available or empty.");

                String cacheKey = CacheKeyGenerator.GetAllValidationProfilesCacheKey();

                DateTime expiryTime = DateTime.Now.AddHours(12);

                //Clone the object before set to cache
                ValidationProfilesCollection clonedValidationProfiles = (ValidationProfilesCollection)validationProfiles.Clone();

                try
                {
                    if (_isDistributedCacheWithNotificationEnabled)
                        _cacheSynchronizationHelper.NotifyLocalCacheInsert(cacheKey, expiryTime, isDataUpdated);

                    _cacheManager.Set(cacheKey, clonedValidationProfiles, expiryTime);
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

                            _cacheManager.Set(cacheKey, validationProfiles, expiryTime);
                            
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
        /// Removes all validation profiles from cache
        /// </summary>
        /// <param name="publishCacheChangeEvent">This parameter is specifying publish cache change event</param>
        /// <returns>True if object deleted successfully otherwise false</returns>
        public Boolean RemoveValidationProfiles(Boolean publishCacheChangeEvent)
        {
            Boolean success = false;

            if (this._isValidationProfileCacheEnabled)
            {
                String cacheKey = CacheKeyGenerator.GetAllValidationProfilesCacheKey();

                try
                {       
                    success = _cacheManager.Remove(cacheKey);
                    if (_isDistributedCacheWithNotificationEnabled)
                        _cacheSynchronizationHelper.NotifyLocalCacheRemoval(cacheKey);
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

        #region Private Methods

        #endregion

        #endregion
    }
}