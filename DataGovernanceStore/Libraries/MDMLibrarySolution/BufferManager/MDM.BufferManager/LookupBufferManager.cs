using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Diagnostics;

namespace MDM.BufferManager
{
    using MDM.BusinessObjects;
    using MDM.CacheManager.Business;
    using MDM.Core;
    using MDM.ExceptionManager;
    using MDM.Utility;

    /// <summary>
    /// Specifies Lookup buffer manager
    /// </summary>
    public class LookupBufferManager : BusinessLogicBase
    {
        #region Fields

        /// <summary>
        /// field denoting cache manager of cache
        /// </summary>
        private ICache _cacheManager = null;

        /// <summary>
        /// Field denoting manager for distributed cache
        /// </summary>
        private IDistributedCache _distributedCacheManager = null;

        /// <summary>
        /// field denoting lookup cache is enabled or not.
        /// </summary>
        private Boolean _isLookupCacheEnabled = false;

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
        /// Instantiate Lookup Buffer Manager
        /// </summary>
        public LookupBufferManager()
        {
            try
            {
                //Get AppConfig which specify whether cache is enabled for entity or not
                String strIsCacheEnabled = AppConfigurationHelper.GetAppConfig<String>("MDMCenter.KnowledgeManager.LookupCache.Enabled", "true");
                Boolean isCacheEnabled = false;
                Boolean.TryParse(strIsCacheEnabled, out isCacheEnabled);

                this._isLookupCacheEnabled = isCacheEnabled;
                this._isDistributedCacheWithNotificationEnabled = AppConfigurationHelper.GetAppConfig<Boolean>("MDMCenter.DistributedCacheWithNotification.Enabled", false);

                if (_isLookupCacheEnabled)
                {
                    _cacheManager = CacheFactory.GetCache();
                    _distributedCacheManager = CacheFactory.GetDistributedCache();
                    if (_isDistributedCacheWithNotificationEnabled)
                        _cacheSynchronizationHelper = new CacheSynchronizationHelper();
                }

                if (_cacheManager == null)
                    this._isLookupCacheEnabled = false;
            }
            catch
            {
                this._isLookupCacheEnabled = false;
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Property denoting lookup cache is enabled or not.
        /// </summary>
        public Boolean IsLookupCacheEnabled
        {
            get
            {
                return _isLookupCacheEnabled;
            }
        }

        #endregion

        #region Methods

        #region Public Methods

        #region Lookup object

        /// <summary>
        /// Finds lookup from cache
        /// </summary>
        /// <param name="attributeId">Indicates attribute id.</param>
        /// <param name="locale">Indicates locale.</param>
        /// <returns>lookup if found in internal cache otherwise null</returns>
        public Lookup FindLookup(Int32 attributeId, LocaleEnum locale)
        {
            Lookup lookup = null;

            if (this._isLookupCacheEnabled)
            {
                String cacheKey = CacheKeyGenerator.GetLookupDataCacheKey(attributeId, locale);

                object lookupObj = null;

                try
                {
                    lookupObj = _cacheManager.Get(cacheKey);
                }
                catch (Exception ex)
                {
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, String.Format("Error occurred : {0}", ex.Message));
                    ExceptionHandler exceptionHandler = new ExceptionHandler(ex);
                }

                if (lookupObj != null && lookupObj is Lookup)
                    lookup = (Lookup)lookupObj;
            }

            return lookup;
        }

        /// <summary>
        /// Finds lookup from cache
        /// </summary>
        /// <param name="lookupTableName">Indicates lookup table name.</param>
        /// <param name="locale">Indicates locale.</param>
        /// <returns>lookup if found in internal cache otherwise null</returns>
        public Lookup FindLookup(String lookupTableName, LocaleEnum locale)
        {
            Lookup lookup = null;

            if (this._isLookupCacheEnabled)
            {
                String cacheKey = CacheKeyGenerator.GetLookupDataCacheKey(lookupTableName.Replace("tblk_", ""), locale);

                object lookupObj = null;

                try
                {
                    lookupObj = _cacheManager.Get(cacheKey);
                }
                catch (Exception ex)
                {
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, String.Format("Error occurred : {0}", ex.Message));
                    ExceptionHandler exceptionHandler = new ExceptionHandler(ex);
                }

                if (lookupObj != null && lookupObj is Lookup)
                    lookup = (Lookup)lookupObj;
            }

            return lookup;
        }

        /// <summary>
        /// Update lookup into internal cache
        /// </summary>
        /// <param name="lookup">Indicates lookup object to be update in cache.</param>
        /// <param name="attributeId">Indicates attribute id.</param>
        /// <param name="locale">Indicates locale.</param>
        /// <param name="numberOfRetry">Indicates number of retry to update object in cache.</param>
        /// <param name="isDataUpdated">Specifies whether to update the cache version in distributed cache. This flag will be used only when the data is 
        /// modified and saved to cache.</param>
        public void UpdateLookup(Lookup lookup, Int32 attributeId, LocaleEnum locale, Int32 numberOfRetry, Boolean isDataUpdated = false)
        {
            if (this._isLookupCacheEnabled)
            {
                if (lookup == null)
                    throw new ArgumentException("lookup is null");

                String cacheKey = CacheKeyGenerator.GetLookupDataCacheKey(attributeId, locale);               

                DateTime expiryTime = DateTime.Now.AddDays(5);

                try
                {
                    if (_isDistributedCacheWithNotificationEnabled)
                        _cacheSynchronizationHelper.NotifyLocalCacheInsert(cacheKey, expiryTime, isDataUpdated);

                    _cacheManager.Set(cacheKey, lookup, expiryTime);                    
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

                            _cacheManager.Set(cacheKey, lookup, expiryTime);

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
        /// Update lookup into internal cache
        /// </summary>
        /// <param name="lookup">Indicates lookup object to be update in cache.</param>
        /// <param name="numberOfRetry">Indicates number of retry to update object in cache.</param>        
        /// <param name="isDataUpdated">Specifies whether to update the cache version in distributed cache. This flag will be used only when the data is 
        /// modified and saved to cache.</param>
        public void UpdateLookup(Lookup lookup, Int32 numberOfRetry, Boolean isDataUpdated = false)
        {
            if (this._isLookupCacheEnabled)
            {
                if (lookup == null)
                    throw new ArgumentException("lookup is null");

                String cacheKey = CacheKeyGenerator.GetLookupDataCacheKey(lookup.Name.Replace("tblk_", ""), lookup.Locale);
                
                DateTime expiryTime = DateTime.Now.AddDays(5);

                try
                {
                    if (_isDistributedCacheWithNotificationEnabled)
                        _cacheSynchronizationHelper.NotifyLocalCacheInsert(cacheKey, expiryTime, isDataUpdated);

                    _cacheManager.Set(cacheKey, lookup, expiryTime);                    
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

                            _cacheManager.Set(cacheKey, lookup, expiryTime);                            

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
        /// Removes lookup from cache.
        /// </summary>
        /// <param name="attributeId">Indicates attribute id.</param>
        /// <param name="locale">Indicates locale</param>
        /// <param name="publishCacheChangeEvent">Indicates publish cache change event.</param>
        /// <param name="clearCacheAccrossWCF">Indicates Clear Cache across WCF is required or not.This flag requires to control cache key one by one or in bulk.
        /// In case of Impacted attribute models removals, we wont remove from different WCF server.Impacted method collects all impacted cache key and remove it from other WCF server.
        /// True : Remove cache key from other WCF server else don't remove.</param>
        /// <returns>True if object deleted successfully otherwise false.</returns>
        public Boolean RemoveLookup(Int32 attributeId, LocaleEnum locale, Boolean publishCacheChangeEvent, Boolean clearCacheAccrossWCF = true)
        {
            Boolean success = false;

            if (this._isLookupCacheEnabled)
            {
                String cacheKey = CacheKeyGenerator.GetLookupDataCacheKey(attributeId, locale);

                try
                {
                    success = _cacheManager.Remove(cacheKey);
                    if (_isDistributedCacheWithNotificationEnabled)
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

        /// <summary>
        /// Removes lookup from cache.
        /// </summary>
        /// <param name="lookupTableName">Indicates lookup table name.</param>
        /// <param name="locale">Indicates locale.</param>
        /// <param name="publishCacheChangeEvent">Indicates publish cache change event.</param>
        /// <param name="clearCacheAccrossWCF">Indicates Clear Cache across WCF is required or not.This flag requires to control cache key one by one or in bulk.
        /// In case of Impacted attribute models removals, we wont remove from different WCF server.Impacted method collects all impacted cache key and remove it from other WCF server.
        /// True : Remove cache key from other WCF server else don't remove.</param>
        /// <returns>True if object deleted successfully otherwise false.</returns>
        public Boolean RemoveLookup(String lookupTableName, LocaleEnum locale, Boolean publishCacheChangeEvent, Boolean clearCacheAccrossWCF = true)
        {
            Boolean success = false;

            if (this._isLookupCacheEnabled)
            {
                String cacheKey = CacheKeyGenerator.GetLookupDataCacheKey(lookupTableName.Replace("tblk_", ""), locale);

                try
                {
                    success = _cacheManager.Remove(cacheKey);
                    if (_isDistributedCacheWithNotificationEnabled)
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lookupTableName"></param>
        /// <returns></returns>
        public List<String> FindDirtyLookupObjectWebServerList(String lookupTableName)
        {
            List<String> webServersList = null;

            if (this._isLookupCacheEnabled)
            {
                if (_distributedCacheManager == null)
                {
                    _distributedCacheManager = CacheFactory.GetDistributedCache();
                }

                String cacheKey = CacheKeyGenerator.GetDirtyLookupObjectWebServerListCacheKey(lookupTableName);

                Object webServersListObject = null;

                try
                {
                    webServersListObject = _distributedCacheManager.Get(cacheKey);
                }
                catch (Exception ex)
                {
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, String.Format("Error occurred : {0}", ex.Message));
                    ExceptionHandler exceptionHandler = new ExceptionHandler(ex);
                }

                if (webServersListObject != null && webServersListObject is List<String>)
                {
                    webServersList = (List<String>)webServersListObject;
                }
            }

            return webServersList;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lookupTableName"></param>
        /// <param name="webServerNamesList"></param>
        /// <param name="numberOfRetry"></param>
        public void UpdateDirtyLookupObjectWebServerList(String lookupTableName, List<String> webServerNamesList, Int32 numberOfRetry)
        {
            if (this._isLookupCacheEnabled)
            {
                if (webServerNamesList == null)
                    throw new ArgumentException("WebServerNamesList is null");

                if (_distributedCacheManager == null)
                {
                    _distributedCacheManager = CacheFactory.GetDistributedCache();
                }

                String cacheKey = CacheKeyGenerator.GetDirtyLookupObjectWebServerListCacheKey(lookupTableName);

                DateTime expiryTime = DateTime.Now.AddDays(12);

                try
                {
                    _distributedCacheManager.Set(cacheKey, webServerNamesList, expiryTime);
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
                            _distributedCacheManager.Set(cacheKey, webServerNamesList, expiryTime);
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
        /// 
        /// </summary>
        /// <param name="lookupTableName"></param>
        /// <returns></returns>
        public Boolean RemoveDirtyLookupObjectWebServerList(String lookupTableName)
        {
            Boolean success = false;

            if (this._isLookupCacheEnabled)
            {
                if (_distributedCacheManager == null)
                {
                    _distributedCacheManager = CacheFactory.GetDistributedCache();
                }

                String cacheKey = CacheKeyGenerator.GetDirtyLookupObjectWebServerListCacheKey(lookupTableName);

                try
                {
                    success = _distributedCacheManager.Remove(cacheKey);
                }
                catch (Exception ex)
                {
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, String.Format("Error occurred : {0}", ex.Message));
                    ExceptionHandler exceptionHandler = new ExceptionHandler(ex);
                }
            }

            return success;
        }

        /// <summary>
        /// Update dirty lookup object into distributed cache with web server list.
        /// </summary>
        /// <param name="lookups">Indicates dirty lookup object to be update in cache.</param>
        public void UpdateDirtyLookupObjectWebServerListCache(LookupCollection lookups)
        {
            //Get all web server list
            BufferHelper bufferHelper = new BufferHelper();
            List<String> webServerList = bufferHelper.GetWebServerList();

            if (webServerList != null && webServerList.Count > 0)
            {
                foreach (Lookup lookup in lookups)
                {
                    UpdateDirtyLookupObjectWebServerList(lookup.Name, webServerList, 3);
                }
            }
        }

        /// <summary>
        /// Removes cache for impacted lookup collection.
        /// </summary>
        /// <param name="lookupCollection">Indicates collection of lookup.</param>
        /// <param name="allowableDataLocales">Indicates available data locales for current context</param>
        public void InvalidateImpactedData(LookupCollection lookupCollection, LocaleCollection allowableDataLocales)
        {
            LookupBufferManager lookupBufferManager = new LookupBufferManager();
            Collection<String> cacheKeys = new Collection<String>();

            //Get system data locale
            LocaleEnum systemDataLocale = MDM.Utility.GlobalizationHelper.GetSystemDataLocale();
            
            if (allowableDataLocales != null)
            {
                foreach (Lookup lookup in lookupCollection)
                {
                    //if lookup is of systemDataLocale then invalidate cache for all Data locales
                    if (lookup.Locale == systemDataLocale)
                    {
                        #region Invalidate Lookup Cache for all locales

                        foreach (Locale locale in allowableDataLocales)
                        {
                            lookupBufferManager.RemoveLookup(lookup.Name, locale.Locale, true, false);

                            cacheKeys.Add(CacheKeyGenerator.GetLookupDataCacheKey(lookup.Name.Replace("tblk_", ""), locale.Locale));
                        }

                        #endregion
                    }

                    #region Invalidate Attribute Cache for lookup

                    String[] attributeIds = GetImpactedLookupAttributes(lookup.Name);

                    if (attributeIds != null)
                    {
                        foreach (String attrId in attributeIds)
                        {
                            Int32 intAttrId = ValueTypeHelper.Int32TryParse(attrId, 0);
                            //Invalidate Attribute Cache for all allowable Data Locale

                            if (lookup.Locale == systemDataLocale)
                            {
                                foreach (Locale locale in allowableDataLocales)
                                {
                                    lookupBufferManager.RemoveLookup(intAttrId, locale.Locale, true, false);

                                    cacheKeys.Add(CacheKeyGenerator.GetLookupDataCacheKey(intAttrId, locale.Locale));
                                }
                            }
                            else
                            {
                                lookupBufferManager.RemoveLookup(intAttrId, lookup.Locale, true, false);

                                cacheKeys.Add(CacheKeyGenerator.GetLookupDataCacheKey(intAttrId, lookup.Locale));
                            }
                        }
                    }
                    #endregion
                }

                #region Clear local Cache across WCF

                if (!_isDistributedCacheWithNotificationEnabled && cacheKeys != null && cacheKeys.Count > 0)
                {
                    BufferHelper bufferHelper = new BufferHelper();
                    bufferHelper.ClearCacheAcrossWCF(cacheKeys);
                }

                #endregion
            }
        }
        
        #endregion

        #endregion

        #region Private Methods

        /// <summary>
        /// Get impacted attribute ids based on lookup table name.
        /// </summary>
        /// <param name="lookupTableName">Indicates lookup table name.</param>
        /// <returns>collection of attribute ids which are impacted.</returns>
        private String[] GetImpactedLookupAttributes(String lookupTableName)
        {
            DataSet ds = new DataSet();
            String attrIds = String.Empty;
            String[] attributeIds = null;

            //Get Attributes where this lookup is used. And clear its cache
            ds = Riversand.StoredProcedures.Common.GetObject("LookupTableWhereUsed", 0, lookupTableName, 0);
            if (ds != null && ds.Tables != null && ds.Tables.Count > 0 && ds.Tables[0].Columns != null && ds.Tables[0].Columns.Contains("attr_id"))
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    attrIds += dr["attr_id"].ToString() + ",";
                }
            }

            if (!String.IsNullOrEmpty(attrIds))
            {
                attributeIds = attrIds.Split(',');
            }

            return attributeIds;
        }

        #endregion

        #endregion
    }
}