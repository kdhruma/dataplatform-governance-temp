using System;
using System.Diagnostics;

namespace MDM.BufferManager
{
    using MDM.BusinessObjects;
    using MDM.CacheManager.Business;
    using MDM.Core;
    using MDM.Utility;

    /// <summary>
    /// Specifies Organization buffer manager
    /// </summary>
    public class OrganizationBufferManager: BusinessLogicBase
    {
        #region Fields

        /// <summary>
        /// field denoting cache manager of cache
        /// </summary>
        private ICache _cacheManager;

        /// <summary>
        /// field denoting organization cache is enabled or not.
        /// </summary>
        private Boolean _isOrganizationCacheEnabled;

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
        /// Instantiate Organization Buffer Manager
        /// </summary>
        public OrganizationBufferManager()
        {
            try
            {
                //Get AppConfig which specify whether cache is enabled for organization or not
                // todo: Change place where will get ConfigParams
                String strIsCacheEnabled = AppConfigurationHelper.GetAppConfig<String>("MDMCenter.OrganizationManager.OrganizationCache.Enabled", "true");
                Boolean isCacheEnabled = false;
                Boolean.TryParse(strIsCacheEnabled, out isCacheEnabled);

                this._isOrganizationCacheEnabled = isCacheEnabled; 
                this._isDistributedCacheWithNotificationEnabled = AppConfigurationHelper.GetAppConfig<Boolean>("MDMCenter.DistributedCacheWithNotification.Enabled", false);


                if (_isOrganizationCacheEnabled)
                {
                    _cacheManager = CacheFactory.GetCache();

                    if (_isDistributedCacheWithNotificationEnabled)
                        _cacheSynchronizationHelper = new CacheSynchronizationHelper();
                }

                if (_cacheManager == null)
                    this._isOrganizationCacheEnabled = false;
            }
            catch
            {
                this._isOrganizationCacheEnabled = false;
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Property denoting Organization cache is enabled or not.
        /// </summary>
        public Boolean IsOrganizationCacheEnabled
        {
            get
            {
                return _isOrganizationCacheEnabled;
            }
        }

        #endregion

        #region Methods

        #region Public methods

        /// <summary>
        /// Finds all available organizations from cache
        /// </summary>
        /// <returns>collection of organizations if found in internal cache otherwise null</returns>
        public OrganizationCollection FindAllOrganizations()
        {
            OrganizationCollection organizations = null;

            if (this.IsOrganizationCacheEnabled)
            {
                String cacheKey = CacheKeyGenerator.GetAllOrganizationCacheKey();

                Object organizationObj = null;

                try
                {
                    organizationObj = _cacheManager.Get(cacheKey);
                }
                catch (Exception ex)
                {
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, String.Format("Error occurred : {0}", ex.Message), MDMTraceSource.DataModel);
                }

                if (organizationObj is OrganizationCollection)
                {
                    OrganizationCollection organizationCollection = (OrganizationCollection)organizationObj;
                    organizations = (OrganizationCollection)organizationCollection.Clone();
                }
            }

            return organizations;
        }

        /// <summary>
        /// Finds all available organizations with Attributes from cache
        /// </summary>
        /// <returns>collection of organizations if found in internal cache otherwise null</returns>
        public OrganizationCollection FindAllOrganizationsWithAttributes()
        {
            OrganizationCollection organizations = null;

            if (this.IsOrganizationCacheEnabled)
            {
                String cacheKey = CacheKeyGenerator.GetAllOrganizationsWithAttributesCacheKey();

                Object organizationObj = null;

                try
                {
                    organizationObj = _cacheManager.Get(cacheKey);
                }
                catch (Exception ex)
                {
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, String.Format("Error occurred : {0}", ex.Message), MDMTraceSource.DataModel);
                }

                if (organizationObj is OrganizationCollection)
                {
                    OrganizationCollection organizationCollection = (OrganizationCollection)organizationObj;
                    organizations = (OrganizationCollection)organizationCollection.Clone();
                }
            }

            return organizations;
        }

        /// <summary>
        /// Removes all organizations from cache.
        /// </summary>
        /// <param name="publishCacheChangeEvent">This parameter is specifying publish cache change event.</param>
        /// <returns>True if object deleted successfully otherwise false.</returns>
        public Boolean RemoveOrganizations(Boolean publishCacheChangeEvent)
        {
            Boolean success = false;

            if (this.IsOrganizationCacheEnabled)
            {
                String cacheKeyOnlyOrganizations = CacheKeyGenerator.GetAllOrganizationCacheKey();
                String cacheKeyOrganizationsWithAttributes = CacheKeyGenerator.GetAllOrganizationsWithAttributesCacheKey();

                try
                {
                    success = _cacheManager.Remove(cacheKeyOnlyOrganizations);
                    success = _cacheManager.Remove(cacheKeyOrganizationsWithAttributes);

                    if (_isDistributedCacheWithNotificationEnabled)
                    {
                        _cacheSynchronizationHelper.NotifyLocalCacheRemoval(cacheKeyOnlyOrganizations);
                        _cacheSynchronizationHelper.NotifyLocalCacheRemoval(cacheKeyOrganizationsWithAttributes);
                    }
                }
                catch (Exception ex)
                {
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, String.Format("Error occurred : {0}", ex.Message), MDMTraceSource.DataModel);
                }
            }

            return success;
        }

        #endregion


        #endregion

        /// <summary>
        /// Update organizations in internal cache
        /// </summary>
        /// <param name="organizations">Indicates collection of organizations to be updated in cache.</param>
        /// <param name="organizationContext">Indicates the context of organization like load attributes, etc.</param>
        /// <param name="numberOfRetry">Indicates number of retry to update object in cache.</param>
        /// <param name="isDataUpdated">Indicates whether the cache data object is updated and inserted to local cache.</param> 
        public void UpdateOrganizations(OrganizationCollection organizations, OrganizationContext organizationContext, Int32 numberOfRetry, Boolean isDataUpdated = false)
        {
            if (this.IsOrganizationCacheEnabled)
            {
                if (organizations == null || organizations.Count < 1)
                    // todo: localize
                    throw new ArgumentException("Organizations are not available or empty.");

                String cacheKey;

                if (!organizationContext.LoadAttributes)
                {
                    cacheKey = CacheKeyGenerator.GetAllOrganizationCacheKey();
                }
                else
                {
                    cacheKey = CacheKeyGenerator.GetAllOrganizationsWithAttributesCacheKey();
                }

                DateTime expiryTime = DateTime.Now.AddMonths(5);

                //Clone the object before set to cache.
                OrganizationCollection clonedOrganizations = (OrganizationCollection)organizations.Clone();

                try
                {
                    if (_isDistributedCacheWithNotificationEnabled)
                        _cacheSynchronizationHelper.NotifyLocalCacheInsert(cacheKey, expiryTime, isDataUpdated);
                    
                    _cacheManager.Set(cacheKey, clonedOrganizations, expiryTime);
                }
                catch (Exception ex)
                {
                    //Retry < 0 means just ignore update if failed..
                    if (numberOfRetry < 0)
                        return;

                    Boolean retrySuccess = false;

                    for (Int32 i = 0; i < numberOfRetry; i++)
                    {
                        try
                        {
                            _cacheManager.Set(cacheKey, organizations, expiryTime);
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
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, String.Format("Error occurred : {0}", ex.Message), MDMTraceSource.DataModel);
                    }
                }
            }
        }
    }
}
