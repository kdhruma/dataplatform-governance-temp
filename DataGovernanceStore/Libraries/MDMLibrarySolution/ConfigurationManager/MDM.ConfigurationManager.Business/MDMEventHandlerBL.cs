using System;
using System.Collections.ObjectModel;

namespace MDM.ConfigurationManager.Business
{
    using MDM.BusinessObjects;
    using MDM.BusinessObjects.Diagnostics;
    using MDM.CacheManager.Business;
    using MDM.ConfigurationManager.Data;
    using MDM.Core;
    using MDM.Core.Exceptions;
    using MDM.Utility;

    /// <summary>
    /// Specifies the MDMEvent handler Business Layer class
    /// </summary>
    public class MDMEventHandlerBL : BusinessLogicBase
    {
        #region Variables

        /// <summary>
        /// Indicates the current trace setting 
        /// </summary>
        private TraceSettings _currentTraceSettings = null;

        /// <summary>
        /// Indicates the Data base command properties
        /// </summary>
        private DBCommandProperties _dbcommand = null;

        /// <summary>
        /// Indicates the ICache
        /// </summary>
        private ICache _cacheManager = null;

        /// <summary>
        /// Indicates the CacheSynchronizationHelper class
        /// </summary>
        private CacheSynchronizationHelper _cacheSynchronizationHelper = null;

        #endregion Variables

        #region Properties

        /// <summary>
        /// Property denoting the Cache manager
        /// </summary>
        private ICache CacheManager
        {
            get
            {
                if (_cacheManager == null)
                {
                    _cacheManager = CacheFactory.GetCache();
                }
                return _cacheManager;
            }
        }

        /// <summary>
        /// Property denoting the CacheSynchronizationHelper class
        /// </summary>
        private CacheSynchronizationHelper CacheSynHelper
        {
            get
            {
                if (_cacheSynchronizationHelper == null)
                {
                    _cacheSynchronizationHelper = new CacheSynchronizationHelper();
                }

                return _cacheSynchronizationHelper;
            }
        }

        #endregion Properties

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public MDMEventHandlerBL()
        {
            _currentTraceSettings = MDMOperationContextHelper.GetCurrentTraceSettings();
        }

        #endregion

        #region Methods

        #region Public Methods

        /// <summary>
        /// Get all the MDMEvent Handlers from the system
        /// </summary>
        /// <param name="eventHandlerIdList">Indicates the list of event Handler Ids</param>
        /// <param name="callerContext">Indicates the name of application and the module that are performing the action</param>
        /// <returns>Returns the list of MDMEvent Handlers</returns>
        public MDMEventHandlerCollection Get(Collection<Int32> eventHandlerIdList, CallerContext callerContext)
        {
            MDMEventHandlerCollection eventHandlers = null;
            DiagnosticActivity diagnosticActivity = new DiagnosticActivity();

            try
            {
                #region Start Tracing

                if (_currentTraceSettings.IsBasicTracingEnabled)
                {
                    diagnosticActivity.Start();

                    if (eventHandlerIdList == null || eventHandlerIdList.Count == 0)
                    {
                        diagnosticActivity.LogInformation("Loading MDMevent handlers from system");
                    }
                    else
                    {
                        diagnosticActivity.LogInformation(String.Format("List of MDMevent handler Ids :{0} started loading.", ValueTypeHelper.JoinCollection(eventHandlerIdList, ",")));
                    }
                }

                #endregion Start Tracing

                #region Load Data From Cache

                var cachedData = this.LoadEventHandlersFromCache(eventHandlerIdList);

                eventHandlers = cachedData.Item1;
                Collection<Int32> missingEventHandlerIds = cachedData.Item2;

                #endregion Load Data From Cache

                if (eventHandlers == null || eventHandlers.Count < 1 || missingEventHandlerIds.Count > 0)
                {
                    this.ValidateCallerContext(callerContext);

                    if (_currentTraceSettings.IsTracingEnabled)
                    {
                        if (eventHandlers == null)
                        {
                            diagnosticActivity.LogInformation("Loading all MDMevent handler data from data base");
                        }
                        else
                        {
                            diagnosticActivity.LogInformation(String.Format("Loading Event handler data from data base. Requested Event Handler Ids : {0}", ValueTypeHelper.JoinCollection(missingEventHandlerIds, ",")));
                        }
                    }

                    #region Load Data From Data Base

                    _dbcommand = DBCommandHelper.Get(callerContext.Application, callerContext.Module, MDMCenterModuleAction.Read);
                    MDMEventHandlerDA mdmEventHandlerDA = new MDMEventHandlerDA();
                    MDMEventHandlerCollection dbData = mdmEventHandlerDA.Get(missingEventHandlerIds, _dbcommand);

                    #endregion Load Data From Data Base

                    #region Update Data to Cache

                    if (dbData != null && dbData.Count > 0)
                    {
                        if (_currentTraceSettings.IsTracingEnabled)
                        {
                            diagnosticActivity.LogData("Loaded the list of MDMevent handler data from data base.", dbData.ToXml());
                        }

                        MDMEventInfoBL mdmEventInfoBL = new MDMEventInfoBL();
                        MDMEventInfoCollection eventInfos = mdmEventInfoBL.Get(callerContext);

                        foreach (MDMEventHandler mdmEventHandler in dbData)
                        {
                            if (mdmEventHandler != null)
                            {
                                mdmEventHandler.MDMEventInfo = eventInfos.GetEventInfoById(mdmEventHandler.EventInfoId);

                                // Merge the cached data and data base data
                                eventHandlers.Add(mdmEventHandler);
                                this.SetDataInCache(eventHandlerId: mdmEventHandler.Id, data: mdmEventHandler, isDataUpdated: false);
                            }
                        }
                    }
                    else
                    {
                        if (_currentTraceSettings.IsTracingEnabled)
                        {
                            diagnosticActivity.LogInformation("There is no data loaded from data base");
                        }
                    }

                    #endregion Update Data to Cache
                }
            }
            finally
            {
                if (_currentTraceSettings.IsBasicTracingEnabled)
                {
                    diagnosticActivity.Stop();
                }
            }

            return eventHandlers;
        }

        /// <summary>
        /// Process the requested MDMEvent Handler based on their actions.
        /// </summary>
        /// <param name="mdmEventHandlerCollection">Indicates the list of MDMEvnet Handlers</param>
        /// <param name="callerContext">Indicates the name of application and the module that are performing the action</param>
        /// <returns>Returns the operation results</returns>
        public OperationResultCollection Process(MDMEventHandlerCollection mdmEventHandlerCollection, CallerContext callerContext)
        {
            OperationResultCollection opResults = new OperationResultCollection();
            String userName = SecurityPrincipalHelper.GetCurrentSecurityPrincipal().CurrentUserName;
            DiagnosticActivity diagnosticActivity = new DiagnosticActivity();

            try
            {
                if (_currentTraceSettings.IsBasicTracingEnabled)
                {
                    diagnosticActivity.Start();
                }

                #region Validations

                ValidateCallerContext(callerContext);
                ValidateMDMEventHandlerCollection(mdmEventHandlerCollection);

                #endregion Validations

                if (_currentTraceSettings.IsBasicTracingEnabled)
                {
                    diagnosticActivity.LogData("Input MDMEvent handler collection", mdmEventHandlerCollection.ToXml());
                }

                #region Update Data to data base

                _dbcommand = DBCommandHelper.Get(callerContext.Application, callerContext.Module, MDMCenterModuleAction.Read);
                MDMEventHandlerDA mdmEventHandlerDA = new MDMEventHandlerDA();
                opResults = mdmEventHandlerDA.Process(mdmEventHandlerCollection, userName, _dbcommand, callerContext);

                #endregion Update Data to data base

                opResults.RefreshOperationResultStatus();

                if (_currentTraceSettings.IsBasicTracingEnabled)
                {
                    diagnosticActivity.LogData("Processed MDMEvent handler operation result", opResults.ToXml());
                }

                UpdateCache(opResults);
            }
            finally
            {
                if (_currentTraceSettings.IsBasicTracingEnabled)
                {
                    diagnosticActivity.Stop();
                }
            }

            return opResults;
        }

        #endregion Public Methods

        #region Validation Helper Methods

        private void ValidateCallerContext(CallerContext callerContext)
        {
            if (callerContext == null)
            {
                DiagnosticActivity diagnosticActivity = new DiagnosticActivity();
                diagnosticActivity.LogError("111846", "CallerContext cannot be null.");
                throw new MDMOperationException("111846", "CallerContext cannot be null.", "ConfigurationManager.Business.MDMEventHandlerBL", String.Empty, String.Empty);
            }
        }

        private void ValidateMDMEventHandlerCollection(MDMEventHandlerCollection mdmEventHandlerCollection)
        {
            if (mdmEventHandlerCollection == null || mdmEventHandlerCollection.Count < 1)
            {
                DiagnosticActivity diagnosticActivity = new DiagnosticActivity();
                diagnosticActivity.LogError("113907", "mdmEventHandlerCollection cannot be null.");
                throw new MDMOperationException("113907", "mdmEventHandlerCollection cannot be null.", "ConfigurationManager.Business.MDMEventHandlerBL", String.Empty, String.Empty, new Object[] { "mdmEventHandlerCollection" });
            }
        }

        #endregion Validation Helper Methods

        #region Cache Helper Methods

        /// <summary>
        /// Inserts the data to cache for the specified cache key.
        /// The cache invalidation is performed when the configurations are reloaded (i.e. isDataUpdated is set to true).
        /// <param name="eventHandlerId">Indicate the event handler Id</param>
        /// <param name="data">Indicates the MDMEvent Handler Object</param>
        /// <param name="isDataUpdated">Indicates whether the data is reloaded or not</param>
        /// </summary>
        private void SetDataInCache(Int32 eventHandlerId, Object data, Boolean isDataUpdated)
        {
            String cacheKey = String.Empty;
            DiagnosticActivity diagnosticActivity = new DiagnosticActivity();

            try
            {
                DateTime expirationTime = DateTime.Now.AddDays(10);
                cacheKey = CacheKeyGenerator.GetMDMEventHandlerCacheKey(eventHandlerId);

                CacheSynHelper.NotifyLocalCacheInsert(cacheKey, expirationTime, isDataUpdated);

                if (data != null)
                {
                    CacheManager.Set(cacheKey, data, expirationTime);
                }
            }
            catch (Exception ex)
            {
                diagnosticActivity.LogError(String.Format("Error occurred while inserting {0} into cache. Internal error : {1}", cacheKey, ex.Message));
            }
        }

        /// <summary>
        /// Clears MDMEventHandler data from Cache.
        /// </summary>
        /// <param name="mdmEventHandlerId">Indicates the MDMevent handler Id</param>
        /// <param name="diagnosticActivity">Indicates the diagnostic activity</param>
        private void ClearCache(Int32 mdmEventHandlerId, DiagnosticActivity diagnosticActivity)
        {
            String cacheKey = CacheKeyGenerator.GetMDMEventHandlerCacheKey(mdmEventHandlerId);

            Boolean isNotified = CacheSynHelper.NotifyLocalCacheRemoval(cacheKey);

            if (!isNotified)
            {
                diagnosticActivity.LogError(String.Format("Notification failure for the eventHandler Id : {0},Cache key Name : {1}", mdmEventHandlerId, cacheKey));
            }
        }

        private MDMEventHandler GetDataFromCache(Int32 eventHandlerKey, DiagnosticActivity diagnosticActivity)
        {
            String cacheKey = String.Empty;
            MDMEventHandler data = null;

            try
            {
                cacheKey = CacheKeyGenerator.GetMDMEventHandlerCacheKey(eventHandlerKey);

                data = CacheManager.Get<MDMEventHandler>(cacheKey);
            }
            catch (Exception ex)
            {
                diagnosticActivity.LogError(String.Format("Error occurred while retrieving data for key {0}. Internal error : {1}", cacheKey, ex.Message));
            }

            return data;
        }

        private Tuple<MDMEventHandlerCollection, Collection<Int32>> LoadEventHandlersFromCache(Collection<Int32> eventHandlerIds)
        {
            MDMEventHandlerCollection data = new MDMEventHandlerCollection();
            DiagnosticActivity diagnosticActivity = new DiagnosticActivity();
            Collection<Int32> missingEventHandlerIds = new Collection<Int32>();

            if (eventHandlerIds != null && eventHandlerIds.Count > 0)
            {
                foreach (Int32 eventHandlerId in eventHandlerIds)
                {
                    MDMEventHandler eventHandler = this.GetDataFromCache(eventHandlerId, diagnosticActivity);

                    if (eventHandler != null)
                    {
                        data.Add(eventHandler);

                        if (_currentTraceSettings.IsBasicTracingEnabled)
                        {
                            diagnosticActivity.LogInformation(String.Format("MDMEventHandler data loaded from Cache. EventHandler Id : {0}", eventHandlerId));
                        }
                    }
                    else
                    {
                        if (_currentTraceSettings.IsBasicTracingEnabled)
                        {
                            diagnosticActivity.LogInformation(String.Format("MDMEventHandler data not available in Cache. EventHandler Id : {0}", eventHandlerId));
                        }
                    }
                }
            }

            return new Tuple<MDMEventHandlerCollection, Collection<Int32>>(data, missingEventHandlerIds);
        }

        private void UpdateCache(OperationResultCollection opResults)
        {
            if (opResults != null && opResults.Count > 0)
            {
                DiagnosticActivity diagnosticActivity = new DiagnosticActivity();

                foreach (OperationResult opResult in opResults)
                {
                    if (opResult != null)
                    {
                        switch (opResult.PerformedAction)
                        {
                            #region Create

                            case ObjectAction.Create:
                                {
                                    SetDataInCache(opResult.Id, data: null, isDataUpdated: false);
                                    break;
                                }
                            #endregion Create

                            #region Update
                            case ObjectAction.Update:
                                {
                                    SetDataInCache(opResult.Id, data: null, isDataUpdated: true);
                                    break;
                                }
                            #endregion Update

                            #region Delete
                            case ObjectAction.Delete:
                                {
                                    ClearCache(opResult.Id, diagnosticActivity);
                                    break;
                                }
                            #endregion Delete
                        }
                    }
                }
            }
        }

        #endregion Cache Helper Methods

        #endregion Methods
    }
}
