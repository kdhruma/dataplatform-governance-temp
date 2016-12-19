using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Diagnostics;
using System.Reflection;
using MDM.Interfaces;

namespace MDM.InstrumentationManager.Utility
{
    using MDM.Core;
    // using MDM.BusinessObjects;
    using System.Runtime.CompilerServices;
    using MDM.Instrumentation.Utility;

    /// <summary>
    /// Helps in tracing events in MDM
    /// </summary>
    public class MDMTraceWriter : IMDMActivityTrace
    {
        #region Fields

        /// <summary>
        /// Field denoting the source for tracing
        /// </summary>
        private static TraceSource _traceSource = null;

        /// <summary>
        /// Field denoting trace configurations
        /// </summary>
        private static MDMTraceConfig _traceConfig = null;

        /// <summary>
        /// Field denoting object for locking. 
        /// </summary>
        private static object _lockObject = new object();

        /// <summary>
        /// Field specifying whether Trace Config is being reloaded.
        /// </summary>
        private static Boolean _isTraceConfigReloadInProgress = false;

        #endregion

        #region Constructor

        #endregion

        #region Properties

        #endregion

        #region Methods

        #region Public Methods

        /// <summary>
        /// Initializes trace source
        /// </summary>
        /// <returns>Boolean flag saying whether initialization is successful or not</returns>
        public static Boolean InitializeTraceSource()
        {
            if (_traceSource == null)
            {
                _traceSource = new TraceSource("MDMTraceSource");
            }

            return true;
        }

        /// <summary>
        /// Starts the new trace activity by emitting Start trace
        /// </summary>
        /// <param name="activityName">The Name of the activity</param>
        /// <param name="withNewIdentity">Boolean flag which says whether the activity needs to be started with new identity</param>
        /// <returns>Boolean flag saying whether start is successful or not</returns>
         [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Boolean StartTraceActivity(String activityName, Boolean withNewIdentity)
        {
            return StartTraceActivity(activityName, MDMTraceSource.General, withNewIdentity);
        }

        /// <summary>
        /// Starts the new trace activity by emitting Start trace
        /// </summary>
        /// <param name="activityName">The Name of the activity</param>
        /// <param name="traceSource">Source which is requesting for trace log</param>
        /// <param name="withNewIdentity">Boolean flag which says whether the activity needs to be started with new identity</param>
        /// <returns>Boolean flag saying whether start is successful or not</returns>

         [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Boolean StartTraceActivity(String activityName, MDMTraceSource traceSource, Boolean withNewIdentity)
        {
            Boolean result = false;

            _traceConfig = GetMDMTraceConfig();
            if (_traceConfig != null && IsEventTypeEnabledForSource(TraceEventType.Start, traceSource))
            {
                if (_traceSource == null)
                {
                    _traceSource = new TraceSource("MDMTraceSource");
                }

                //Check new identity has been requested. If yes, create new GUID
                //Also check if an activity was set in scope i.e., if it was 
                //propagated from some other activity. If not, i.e., ambient activity is 
                //equal to Guid.Empty, create a new one.
                if (withNewIdentity || Trace.CorrelationManager.ActivityId == Guid.Empty)
                {
                    Guid newGuid = Guid.NewGuid();
                    Trace.CorrelationManager.ActivityId = newGuid;
                }

                _traceSource.TraceEvent(TraceEventType.Start, 0, activityName);

                result = true;
            }

            return result;
        }

        /// <summary>
        /// Stops the trace activity by emitting Stop trace
        /// </summary>
        /// <param name="activityName">The Name of the activity</param>
        /// <returns>Boolean flag saying whether stop is successful or not</returns>

         [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Boolean StopTraceActivity(String activityName)
        {
            return StopTraceActivity(activityName, MDMTraceSource.General);
        }

        /// <summary>
        /// Stops the trace activity by emitting Stop trace
        /// </summary>
        /// <param name="activityName">The Name of the activity</param>
        /// <param name="traceSource">Source which is requesting for trace log</param>
        /// <returns>Boolean flag saying whether stop is successful or not</returns>

         [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Boolean StopTraceActivity(String activityName, MDMTraceSource traceSource)
        {
            Boolean result = false;

            _traceConfig = GetMDMTraceConfig();
            if (_traceConfig != null && IsEventTypeEnabledForSource(TraceEventType.Stop, traceSource))
            {
                if (_traceSource == null)
                    return false;

                _traceSource.TraceEvent(TraceEventType.Stop, 0, activityName);

                result = true;
            }

            return result;
        }

        /// <summary>
        /// Emits the event for the specified event type with the specified message
        /// </summary>
        /// <param name="eventType">Type of the event which needs to be emitted</param>
        /// <param name="message">The message for the trace event</param>
        /// <returns>Boolean flag saying whether emit is successful or not</returns>

         [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Boolean EmitTraceEvent(TraceEventType eventType, String message)
        {
            return EmitTraceEvent(eventType, message, MDMTraceSource.General);
        }

        /// <summary>
        /// Emits the event for the specified event type with the specified message
        /// </summary>
        /// <param name="eventType">Type of the event which needs to be emitted</param>
        /// <param name="message">The message for the trace event</param>
        /// <param name="traceSource">Source which is requesting for trace log</param>
        /// <returns>Boolean flag saying whether emit is successful or not</returns>
        
         [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Boolean EmitTraceEvent(TraceEventType eventType, String message, MDMTraceSource traceSource)
        {
            Boolean result = false;

            _traceConfig = GetMDMTraceConfig();
            if (_traceConfig != null && IsEventTypeEnabledForSource(eventType, traceSource))
            {
                if (_traceSource == null)
                    return false;

                _traceSource.TraceEvent(eventType, 0, message);

                result = true;
            }

            return result;
        }

        /// <summary>
        /// Loads Trace Configuration
        /// </summary>
        /// <param name="traceConfigurationXml">Trace configuration xml to be loaded</param>
        /// <param name="isReloadOperation">Specifies whether the function is invoked for a reloading of app configs.</param>
        public static MDMTraceConfig LoadTraceConfiguration(String traceConfigurationXml, Boolean isReloadOperation = false)
        {
            MDMTraceConfig traceConfig = null;
            if (!String.IsNullOrWhiteSpace(traceConfigurationXml))
            {
                traceConfig = new MDMTraceConfig(traceConfigurationXml);
                if (traceConfig.MDMTraceConfigItems != null && traceConfig.MDMTraceConfigItems.Count >= 0)
                {
                    if (AppConfigurationHelper.IsDistributedCacheWithNotificationEnabled)
                        SetMDMTraceConfigInCache(traceConfig, isReloadOperation);
                    else
                        _traceConfig = traceConfig;
                }
            }
            return traceConfig;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Tells event type is enabled or not for given EventType and Source
        /// </summary>
        /// <param name="traceEventType">Indicates trace event type for which trace source needs to check enabled or not</param>
        /// <param name="traceSource">Indicates trace source name for which trace source needs to check enabled or not</param>
        /// <returns>returns true if event type is enabled for given trace event type and source, otherwise false</returns>
        public static Boolean IsEventTypeEnabledForSource(TraceEventType traceEventType, MDMTraceSource traceSource)
        {
            Boolean enabled = false;

            if (_traceConfig != null)
            {
                //Find trace config item for requested source
                MDMTraceConfigItem traceConfigItem = _traceConfig.MDMTraceConfigItems.FirstOrDefault(c => c.TraceSource == traceSource);

                if (traceConfigItem != null)
                {
                    switch (traceEventType)
                    {
                        case TraceEventType.Start:
                        case TraceEventType.Stop:
                            enabled = traceConfigItem.LogActivityTrace;
                            break;
                        case TraceEventType.Error:
                            enabled = traceConfigItem.LogError;
                            break;
                        case TraceEventType.Warning:
                            enabled = traceConfigItem.LogWarning;
                            break;
                        case TraceEventType.Information:
                            enabled = traceConfigItem.LogInformation;
                            break;
                        case TraceEventType.Verbose:
                            enabled = traceConfigItem.LogVerbose;
                            break;
                    }
                }
            }

            return enabled;
        }

        /// <summary>
        /// Gets the MDMTraceConfig from the Cache.
        /// </summary>
        /// <returns></returns>
        public static MDMTraceConfig GetMDMTraceConfig()
        {
            MDMTraceConfig traceConfig = null;
            if (AppConfigurationHelper.IsDistributedCacheWithNotificationEnabled)
            {
                ExceptionManager.EventLogHandler eventLogHandler = new ExceptionManager.EventLogHandler();
                try
                {
                    String cacheKey = MDMTraceConfig.GetMDMTraceConfigCacheKey();

                    ICache cacheManager = CacheFactory.GetCache();
                    traceConfig = cacheManager.Get<MDMTraceConfig>(cacheKey);
                    if (traceConfig == null && !_isTraceConfigReloadInProgress)
                    {
                        // Added condition (_isTraceConfigReloadInProgress) to ensure that the trace loading does not result in an infinite recursive call
                        // leading into a StackOverflowException. The condition ensures that the recursive calls will not try to reload the configuration.
                        lock (_lockObject)
                        {
                            // Check if the previous thread loaded data in cache.
                            traceConfig = cacheManager.Get<MDMTraceConfig>(cacheKey);
                            if (traceConfig == null)
                            {
                                // If data is not loaded, try reloading from database.
                                traceConfig = ReloadMDMTraceConfig();
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    eventLogHandler.WriteErrorLog("Error occurred while retrieving MDMTraceConfig. Internal error : " + ex.Message, 0);
                    ExceptionManager.ExceptionHandler exceptionHandler = new ExceptionManager.ExceptionHandler(ex);
                }
            }
            else
            {
                traceConfig = _traceConfig;
            }
            return traceConfig;
        }

        /// <summary>
        /// Reloads the MDMTraceConfig from database and sets into cache.
        /// </summary>
        /// <returns></returns>
        private static MDMTraceConfig ReloadMDMTraceConfig()
        {
            MDMTraceConfig traceConfig = null;            
            try
            {
                _isTraceConfigReloadInProgress = true;

                // If data is unavailable in local cache, try loading from database.
                String appConfigValue = AppConfigurationHelper.GetAppConfig<String>("MDMCenter.TraceConfiguration");
                traceConfig = LoadTraceConfiguration(appConfigValue);
            }
            catch (Exception ex)
            {
                ExceptionManager.EventLogHandler eventLogHandler = new ExceptionManager.EventLogHandler();
                eventLogHandler.WriteErrorLog("Error occurred while reloading MDMTraceConfig. Internal error : " + ex.Message, 0);                
            }
            finally
            {
                _isTraceConfigReloadInProgress = false;
            }
            return traceConfig;
        }

        /// <summary>
        /// Inserts the MDMTraceConfig to the local cache.
        /// </summary>
        /// <param name="traceConfig">MDMTraceConfig object that has to be inserted into cache.</param>
        /// <param name="isDataModified">Specifies if the data is modified and inserted to cache.The cache clearance notifications are invoked based on this parameter.</param>
        private static void SetMDMTraceConfigInCache(MDMTraceConfig traceConfig, Boolean isDataModified)
        {
            ExceptionManager.EventLogHandler eventLogHandler = new ExceptionManager.EventLogHandler();
            try
            {
                DateTime expirationTime = DateTime.Now.AddDays(10);
                String cacheKey = MDMTraceConfig.GetMDMTraceConfigCacheKey();

                CacheSynchronizationHelper cacheSynchronizationHelper = new CacheSynchronizationHelper();
                cacheSynchronizationHelper.NotifyLocalCacheInsert(cacheKey, expirationTime, isDataModified);

                ICache cacheManager = CacheFactory.GetCache();
                cacheManager.Set(cacheKey, traceConfig, expirationTime);

                if (Constants.TRACING_ENABLED)
                    eventLogHandler.WriteInformationLog("MDMTraceConfig is inserted in local cache.", 0);
            }
            catch (Exception ex)
            {
                eventLogHandler.WriteErrorLog("Error occurred while inserting MDMTraceConfig in local cache. Internal error : " + ex.Message, 0);
                ExceptionManager.ExceptionHandler exceptionHander = new ExceptionManager.ExceptionHandler(ex);
            }
        }

        #endregion

        #endregion

        public void Initialize()
        {
            throw new NotImplementedException();
        }

        public void StartActivity(string activityName = "", string message = "", MDMTraceSource traceSource = MDMTraceSource.General, bool startNewIdentity = false)
        {
            throw new NotImplementedException();
        }

        public void StopActivity(string message = "")
        {
            throw new NotImplementedException();
        }

        public void LogMessage(string message, TraceEventType eventType = TraceEventType.Information)
        {
            throw new NotImplementedException();
        }
    }
}
