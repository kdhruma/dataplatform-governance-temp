using System;
using System.Diagnostics;

namespace MDM.BufferManager
{
    using MDM.BusinessObjects;
    using MDM.BusinessObjects.Diagnostics;
    using MDM.CacheManager.Business;
    using MDM.Core;
    using MDM.ExceptionManager;
    using MDM.Utility;
    using System.Collections.Generic;

    /// <summary>
    /// Specifies Container EntityType buffer manager
    /// </summary>
    public class ContainerEntityTypeMappingBufferManager : BusinessLogicBase
    {
        #region Fields

        /// <summary>
        /// field denoting cache manager of cache
        /// </summary>
        private ICache _cacheManager = null;

        /// <summary>
        /// field denoting entity type cache is enabled or not.
        /// </summary>
        private Boolean _isContainerEntityTypeMappingCacheEnabled = false;

        /// <summary>
        /// Field denotes whether the Distributed Cache with notification has been enabled.
        /// </summary>
        private Boolean _isDistributedCacheWithNotificationEnabled = false;

        /// <summary>
        /// Field denotes an instance of the CacheSynchronizationHelper.
        /// </summary>
        private CacheSynchronizationHelper _cacheSynchronizationHelper = null;

        /// <summary>
        /// Indicates current trace settings
        /// </summary>
        private TraceSettings _currentTraceSettings = new TraceSettings();

        /// <summary>
        /// Indicates whether diagnostic tracing is enable or not
        /// </summary>
        private Boolean isTracingEnabled = false;

        #endregion

        #region Constructors

        /// <summary>
        /// Instantiate ContainerEntityType Mapping Buffer Manager
        /// </summary>
        public ContainerEntityTypeMappingBufferManager()
        {
            try
            {
                //Get AppConfig which specify whether cache is enabled for entity type or not
                String strIsCacheEnabled = AppConfigurationHelper.GetAppConfig<String>("MDMCenter.DataModelManager.ContainerEntityTypeMappingCache.Enabled", "true");
                Boolean isCacheEnabled = false;
                Boolean.TryParse(strIsCacheEnabled, out isCacheEnabled);

                this._isContainerEntityTypeMappingCacheEnabled = isCacheEnabled;
                this._isDistributedCacheWithNotificationEnabled = AppConfigurationHelper.GetAppConfig<Boolean>("MDMCenter.DistributedCacheWithNotification.Enabled", false);

                if (_isContainerEntityTypeMappingCacheEnabled)
                {
                    _cacheManager = CacheFactory.GetCache();
                    if (_isDistributedCacheWithNotificationEnabled)
                        _cacheSynchronizationHelper = new CacheSynchronizationHelper();                        
                }

                if (_cacheManager == null)
                    this._isContainerEntityTypeMappingCacheEnabled = false;

                _currentTraceSettings = MDMOperationContextHelper.GetCurrentTraceSettings();
                isTracingEnabled = _currentTraceSettings.IsBasicTracingEnabled;
            }
            catch
            {
                this._isContainerEntityTypeMappingCacheEnabled = false;
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Property denoting entity type cache is enabled or not.
        /// </summary>
        public Boolean IsContainerEntityTypeCacheEnabled
        {
            get
            {
                return _isContainerEntityTypeMappingCacheEnabled;
            }
        }

        #endregion

        #region Methods

        #region Public Methods

        /// <summary>
        /// Finds all available entity types from cache
        /// </summary>
        /// <returns>collection of entity types if found in internal cache otherwise null</returns>
        public ContainerEntityTypeMappingCollection FindAllContainerEntityTypeMappings()
        {
            ContainerEntityTypeMappingCollection containerEntityTypeMappings = null;

            if (this._isContainerEntityTypeMappingCacheEnabled)
            {
                String cacheKey = CacheKeyGenerator.GetAllContainerEntityTypeMappingsCacheKey();

                object containerEntityTypesObj = null;

                try
                {
                    containerEntityTypesObj = _cacheManager.Get(cacheKey);
                }
                catch (Exception ex)
                {
                    WriteDiagnosticError(ex);
                }

                if (containerEntityTypesObj != null && containerEntityTypesObj is ContainerEntityTypeMappingCollection)
                {
                    containerEntityTypeMappings = (ContainerEntityTypeMappingCollection)containerEntityTypesObj;
                }
            }

            return containerEntityTypeMappings;
        }

        /// <summary>
        /// Updates the conatiner entity type mapping cache.
        /// </summary>
        /// <param name="containerEntityTypeMappings"></param>
        /// <param name="numberOfRetry"></param>
        /// <param name="isDataUpdated"></param>
        public void UpdateContainerEntityTypeMappings(ContainerEntityTypeMappingCollection containerEntityTypeMappings, Int32 numberOfRetry, Boolean isDataUpdated = false)
        {
            if (this._isContainerEntityTypeMappingCacheEnabled)
            {
                if (containerEntityTypeMappings == null || containerEntityTypeMappings.Count < 1)
                {
                    throw new ArgumentException("Container EntityType Mappings are not available or empty.");
                }

                String cacheKey = CacheKeyGenerator.GetAllContainerEntityTypeMappingsCacheKey();

                DateTime expiryTime = DateTime.Now.AddDays(365);

                try
                {
                    if (_isDistributedCacheWithNotificationEnabled)
                        _cacheSynchronizationHelper.NotifyLocalCacheInsert(cacheKey, expiryTime, isDataUpdated);

                    _cacheManager.Set(cacheKey, containerEntityTypeMappings, expiryTime);                    
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

                            _cacheManager.Set(cacheKey, containerEntityTypeMappings, expiryTime);
                            
                            retrySuccess = true;
                            break;
                        }
                        catch
                        {
                            WriteDiagnosticError(ex);
                        }
                    }

                    if (!retrySuccess)
                    {
                        //TODO:: What to do if update fails after retry too..
                        WriteDiagnosticError(ex);
                    }
                }
            }
        }

        /// <summary>
        /// Removes all containerEntityTypeMappings from cache.
        /// </summary>
        /// <returns>True if object deleted successfully otherwise false.</returns>
        public Boolean RemoveContainerEntityTypeMappings()
        {
            Boolean success = false;

            if (this._isContainerEntityTypeMappingCacheEnabled)
            {
                String cacheKey = CacheKeyGenerator.GetAllContainerEntityTypeMappingsCacheKey();

                try
                {       
                    success = _cacheManager.Remove(cacheKey);
                    if (_isDistributedCacheWithNotificationEnabled)
                        _cacheSynchronizationHelper.NotifyLocalCacheRemoval(cacheKey);
                }
                catch (Exception ex)
                {
                    WriteDiagnosticError(ex);
                }

            }

            return success;
        }

        #endregion

        #region Private Methods

        private void WriteDiagnosticError(Exception ex)
        {
            DiagnosticActivity diagnosticActivity = new DiagnosticActivity();
            diagnosticActivity.Start();
            diagnosticActivity.LogError(String.Format("The cache operation failed with the error message {0}.", ex.ToString()));
            diagnosticActivity.Stop();
        }

        #endregion

        #endregion
    }
}