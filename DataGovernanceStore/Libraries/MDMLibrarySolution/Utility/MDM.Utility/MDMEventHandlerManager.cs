using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;

namespace MDM.Utility
{
    using Core;
    using BusinessObjects;
    using BusinessObjects.Diagnostics;
    using CacheManager.Business;
    using Interfaces;
    using DataProviderInterfaces;

    /// <summary>
    /// Provides methods to manage event subscriptions for all MDMEventHandlers
    /// </summary>
    public class MDMEventHandlerManager
    {
        #region Fields

        /// <summary>
        /// Field holds the instance of the MDMEventHandler data provider.
        /// </summary>
        private static IMDMEventHandlerDataProvider _eventHandlerDataProvider = null;

        /// <summary>
        /// Field holds the instance of the service type which is using this utility.
        /// </summary>
        private static MDMServiceType _serviceType = MDMServiceType.UnKnown;

        /// <summary>
        /// Field holds the instance of the caller context used for data provider calls.
        /// </summary>
        private static CallerContext _callerContext = new CallerContext(MDMCenterApplication.MDMCenter, MDMCenterModules.Configuration);

        #endregion Fields

        #region Public Methods

        /// <summary>
        /// Initializes the data provider for event handlers and initializes the subscriptions
        /// </summary>
        /// <param name="eventHandlerProvider">Represents an MDMEventHandler data provider</param>
        /// <param name="serviceType">Represents the MDMServiceType which is using this utility</param>
        public static void Initialize(IMDMEventHandlerDataProvider eventHandlerProvider, MDMServiceType serviceType)
        {
            DiagnosticActivity diagnosticActivity = new DiagnosticActivity();
            TraceSettings traceSettings = MDMOperationContextHelper.GetCurrentTraceSettings();

            if (traceSettings.IsBasicTracingEnabled)
            {
                diagnosticActivity.Start();
            }

            try
            {
                if (_eventHandlerDataProvider == null)
                {
                    // Perform validation for inputs                
                    ValidateParams(eventHandlerProvider, serviceType);

                    if (traceSettings.IsBasicTracingEnabled)
                    {
                        diagnosticActivity.LogInformation(String.Format("Initialize request received on server {0} with service type {1} and event handler data provider type {2}", Environment.MachineName, serviceType.ToString(), eventHandlerProvider.GetType().FullName));
                    }

                    // Initialize the member variables
                    _serviceType = serviceType;
                    _eventHandlerDataProvider = eventHandlerProvider;

                    // Subscribe the required cache notification events 
                    SubscribeCacheNotificationEvents();

                    if (traceSettings.IsBasicTracingEnabled)
                    {
                        diagnosticActivity.LogInformation("Completed subscription with cache notification events. Starting with event handler subscriptions.");
                    }

                    // Subscribes all the MDM events defined in the database 
                    InitializeAllMDMEventSubscriptions(isReloadTriggered: false);

                    //Insert cache key for the reload option
                    CacheSynchronizationHelper syncHelper = new CacheSynchronizationHelper();
                    syncHelper.NotifyLocalCacheInsert(CacheKeyContants.MDM_EVENT_HANDLER_RESET_KEY, DateTime.Now.AddDays(10), false);

                    if (traceSettings.IsBasicTracingEnabled)
                    {
                        diagnosticActivity.LogInformation(String.Format("Completed event handler subscriptions on server {0} with service type {1}.", Environment.MachineName, _serviceType.ToString()));
                    }
                }
                else
                {
                    if (traceSettings.IsBasicTracingEnabled)
                    {
                        diagnosticActivity.LogWarning(String.Format("MDMEventHandlerManager is already initialized on server {0} with service type {1}.", Environment.MachineName, _serviceType.ToString()));
                    }
                }
            }
            catch (Exception ex)
            {
                diagnosticActivity.LogError(String.Format("Error occurred while initializing the MDMEventHandlerManager on server {0} with service type {1}. Exception Details: {2}", Environment.MachineName, _serviceType.ToString(), ex.Message));
                throw;
            }
            finally
            {
                if (traceSettings.IsBasicTracingEnabled)
                {
                    diagnosticActivity.Stop();
                }
            }
        }

        /// <summary>
        /// Gets the MDM web event handlers.
        /// </summary>
        /// <returns>Collection of Web event handlers</returns>
        public static IMDMEventHandlerCollection GetMDMWebEventHandlers()
        {
            DiagnosticActivity diagnosticActivity = new DiagnosticActivity();
            TraceSettings traceSettings = MDMOperationContextHelper.GetCurrentTraceSettings();
            IMDMEventHandlerCollection webEventHandlers = MDMObjectFactory.GetIMDMEventHandlerCollection();

            if (traceSettings.IsBasicTracingEnabled)
            {
                diagnosticActivity.Start();
            }

            try
            {
                ValidateParams(_eventHandlerDataProvider, _serviceType);

                //Get from cache
                ICache localCache = CacheFactory.GetCache();
                String cacheKey = CacheKeyGenerator.GetMDMWebEventHandlersCacheKey();
                IMDMEventHandlerCollection webEventHandlersFromCache = localCache.Get<IMDMEventHandlerCollection>(cacheKey);

                if (webEventHandlersFromCache == null)
                {
                    if (traceSettings.IsBasicTracingEnabled)
                    {
                        diagnosticActivity.LogInformation("Could not find web event handlers from cache. Proceeding to get it from provider.");
                    }

                    //Get it from the provider
                    IMDMEventHandlerCollection eventHandlers = _eventHandlerDataProvider.GetMDMEventHandlers(null, _callerContext);
                    foreach (IMDMEventHandler handler in eventHandlers)
                    {
                        IMDMEventInfo eventInfo = handler.GetMDMEventInfo();
                        if (eventInfo != null && String.Compare(eventInfo.EventName, Constants.WEB_EVENT_HANDLERS_EVENT_NAME, true) == 0 && handler.Enabled
                             && IsEventSubscribedOnCurrentService(handler) && IsEventHandlerBasedOnAppConfig(handler) && IsEventHandlerBasedOnFeatureConfig(handler))
                        {
                            webEventHandlers.Add(handler);
                        }
                    }

                    //Set in cache
                    localCache.Set<IMDMEventHandlerCollection>(cacheKey, webEventHandlers, DateTime.Now.AddDays(10));
                }
                else
                {
                    //If count is not null in cache, there are no event handlers
                    webEventHandlers = webEventHandlersFromCache;
                }

                if (traceSettings.IsBasicTracingEnabled)
                {
                    diagnosticActivity.LogData(String.Format("Found {0} web event handlers.", webEventHandlers.Count), webEventHandlers.ToXml());
                }
            }
            catch (Exception ex)
            {
                diagnosticActivity.LogError(String.Format("Error occurred while fetching web event handlers. Exception Details: {0}", ex.Message));
                throw;
            }
            finally
            {
                if (traceSettings.IsBasicTracingEnabled)
                {
                    diagnosticActivity.Stop();
                }
            }

            return webEventHandlers;
        }

        #endregion Public Methods

        #region Private Methods

        #region Cache Invalidation Event Handling

        /// <summary>
        /// Subscribes the cache notification events.
        /// </summary>
        private static void SubscribeCacheNotificationEvents()
        {
            CacheNotificationEventManager.Instance.CacheUpdate += OnCacheUpdate;
        }

        /// <summary>
        /// Called on cache update notification.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="eventArgs">Instance containing the cache notification event data.</param>
        private static void OnCacheUpdate(Object sender, CacheNotificationEventArgs eventArgs)
        {
            DiagnosticActivity diagnosticActivity = new DiagnosticActivity();
            TraceSettings traceSettings = MDMOperationContextHelper.GetCurrentTraceSettings();

            if (traceSettings.IsBasicTracingEnabled)
            {
                diagnosticActivity.Start();
            }

            try
            {
                ValidateCacheNotificationEventArgs(eventArgs);

                String cacheKey = eventArgs.CacheKey;
                ICache localCache = CacheFactory.GetCache();

                if (traceSettings.IsBasicTracingEnabled)
                {
                    diagnosticActivity.LogInformation(String.Format("Cache Update notification received on server {0} with service type {1} for cache key {2}", Environment.MachineName, _serviceType.ToString(), cacheKey));
                }

                if (String.Compare(cacheKey, CacheKeyContants.MDM_EVENT_HANDLER_RESET_KEY) == 0)
                {
                    localCache.Remove(CacheKeyGenerator.GetMDMWebEventHandlersCacheKey());
                    InitializeAllMDMEventSubscriptions(isReloadTriggered: true);
                }
                else if (cacheKey.StartsWith(CacheKeyContants.MDM_EVENT_HANDLER_CACHE_KEY_PREFIX))
                {
                    Int32 eventHandlerId = ValueTypeHelper.ConvertToInt32(cacheKey.Replace(CacheKeyContants.MDM_EVENT_HANDLER_CACHE_KEY_PREFIX, String.Empty));

                    IMDMEventHandler eventHandler = GetMDMEventHandler(eventHandlerId);

                    if (eventHandler != null)
                    {
                        IMDMEventInfo eventInfo = eventHandler.GetMDMEventInfo();

                        if (eventInfo != null && _serviceType == MDMServiceType.Web && String.Compare(eventInfo.EventName, Constants.WEB_EVENT_HANDLERS_EVENT_NAME, true) == 0)
                        {
                            localCache.Remove(CacheKeyGenerator.GetMDMWebEventHandlersCacheKey());
                        }
                        else if (IsEventSubscribedOnCurrentService(eventHandler) && IsEventHandlerBasedOnAppConfig(eventHandler) && IsEventHandlerBasedOnFeatureConfig(eventHandler))
                        {
                            if (eventHandler.Enabled)
                            {
                                SubscribeEvent(eventHandler);
                            }
                            else
                            {
                                UnSubscribeEvent(eventHandler);
                            }
                        }
                    }
                    else
                    {
                        if (traceSettings.IsBasicTracingEnabled)
                        {
                            diagnosticActivity.LogWarning(String.Format("Unable to get event handler with id: {0} on cache update for updating subscription", eventHandlerId));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                diagnosticActivity.LogError(String.Format("Error occurred while handling the cache update event. Exception Details: {0}", ex.Message));
                throw;
            }
            finally
            {
                if (traceSettings.IsBasicTracingEnabled)
                {
                    diagnosticActivity.Stop();
                }
            }
        }

        /// <summary>
        /// Gets the MDM event handler.
        /// </summary>
        /// <param name="eventHandlerId">The event handler identifier.</param>
        /// <returns>MDM Event Handler</returns>
        private static IMDMEventHandler GetMDMEventHandler(Int32 eventHandlerId)
        {
            IMDMEventHandler eventHandler = null;
            IMDMEventHandlerCollection eventHandlerCollection = _eventHandlerDataProvider.GetMDMEventHandlers(new Collection<Int32>() { eventHandlerId }, _callerContext);
            if (eventHandlerCollection != null)
            {
                eventHandler = eventHandlerCollection.FirstOrDefault();
            }
            return eventHandler;
        }

        #endregion Cache Invalidation Event Handling

        #region Event Subscription/UnSubscription Methods

        /// <summary>
        /// Subscribes the event.
        /// </summary>
        /// <param name="eventHandler">The event handler.</param>
        private static void SubscribeEvent(IMDMEventHandler eventHandler)
        {
            DiagnosticActivity diagnosticActivity = new DiagnosticActivity();
            TraceSettings traceSettings = MDMOperationContextHelper.GetCurrentTraceSettings();

            if (traceSettings.IsBasicTracingEnabled)
            {
                diagnosticActivity.Start();
            }

            try
            {
                UpdateEventSubscriber(eventHandler, CombineDelegates);

                if (traceSettings.IsBasicTracingEnabled)
                {
                    diagnosticActivity.LogData(String.Format("Event Subscription is successful for event handler with id: {0} on server {1} with service type {2}.", eventHandler.Id, Environment.MachineName, _serviceType.ToString()), eventHandler.ToXml());
                }
            }
            catch (Exception ex)
            {
                diagnosticActivity.LogMessageWithData(String.Format("Error occurred while subscribing event handler with id: {0} on server {1} with service type {2}. Exception Details: {3}", eventHandler.Id, Environment.MachineName, _serviceType.ToString(), ex.Message), eventHandler.ToXml(), MessageClassEnum.Error);
                throw;
            }
            finally
            {
                if (traceSettings.IsBasicTracingEnabled)
                {
                    diagnosticActivity.Stop();
                }
            }
        }

        /// <summary>
        /// Unsubscribes the event.
        /// </summary>
        /// <param name="eventHandler">The event handler.</param>
        private static void UnSubscribeEvent(IMDMEventHandler eventHandler)
        {
            DiagnosticActivity diagnosticActivity = new DiagnosticActivity();
            TraceSettings traceSettings = MDMOperationContextHelper.GetCurrentTraceSettings();

            if (traceSettings.IsBasicTracingEnabled)
            {
                diagnosticActivity.Start();
            }

            try
            {
                UpdateEventSubscriber(eventHandler, RemoveDelegate);

                if (traceSettings.IsBasicTracingEnabled)
                {
                    diagnosticActivity.LogData(String.Format("Event unsubscription is successful for event handler with id: {0} on server {1} with service type {2}.", eventHandler.Id, Environment.MachineName, _serviceType.ToString()), eventHandler.ToXml());
                }
            }
            catch (Exception ex)
            {
                diagnosticActivity.LogMessageWithData(String.Format("Error occurred while unsubscribing event handler with id: {0} on server {1} with service type {2}. Exception Details: {3}", eventHandler.Id, Environment.MachineName, _serviceType.ToString(), ex.Message), eventHandler.ToXml(), MessageClassEnum.Error);
                throw;
            }
            finally
            {
                if (traceSettings.IsBasicTracingEnabled)
                {
                    diagnosticActivity.Stop();
                }
            }
        }

        /// <summary>
        /// Updates the event subscriber.
        /// </summary>
        /// <param name="eventHandler">The event handler.</param>
        /// <param name="method">The method.</param>
        private static void UpdateEventSubscriber(IMDMEventHandler eventHandler, Func<Delegate, Delegate, Delegate> method)
        {
            DiagnosticActivity diagnosticActivity = new DiagnosticActivity();
            TraceSettings traceSettings = MDMOperationContextHelper.GetCurrentTraceSettings();

            if (traceSettings.IsBasicTracingEnabled)
            {
                diagnosticActivity.Start();
            }

            try
            {
                IMDMEventInfo eventInfo = eventHandler.GetMDMEventInfo();

                if (eventInfo != null)
                {
                    // Get Type based on assembly name and class name
                    Type classType = GetType(eventInfo.AssemblyName, eventInfo.EventManagerClassName);

                    // Get the event's field info based on event name
                    FieldInfo eventFieldInfo = GetFieldInfoForEvent(classType, eventInfo.EventName);

                    // Get the target delegate for subscription
                    Delegate targetDelegate = GetTargetDelegate(eventHandler, eventFieldInfo.FieldType);

                    // Get the instance of the Event handling class
                    Object typeInstance = GetInstanceForType(classType);

                    // Get the current delegates associated with the event
                    Delegate currentDelegate = eventFieldInfo.GetValue(typeInstance) as Delegate;

                    // Combine or remove the event subscribers as per method
                    Delegate updatedDelegate = method.Invoke(currentDelegate, targetDelegate);

                    // Set the updated delegates back to the event
                    eventFieldInfo.SetValue(typeInstance, updatedDelegate);
                }
            }
            catch(Exception ex)
            {
                diagnosticActivity.LogError(String.Format("Error occurred while updating event subscription for event handler with id: {0}. Exception Details: {1}", eventHandler.Id, ex.Message));
                throw;
            }
            finally
            {
                if (traceSettings.IsBasicTracingEnabled)
                {
                    diagnosticActivity.Stop();
                }
            }
        }

        /// <summary>
        /// Combines the delegates.
        /// </summary>
        /// <param name="currentDelegate">The current delegate.</param>
        /// <param name="targetDelegate">The target delegate.</param>
        /// <returns>Combined Delegate</returns>
        private static Delegate CombineDelegates(Delegate currentDelegate, Delegate targetDelegate)
        {
            if (currentDelegate == null)
            {
                // If no delegates are found, set target as current
                currentDelegate = targetDelegate;
            }
            else
            {
                // Check if the same delegate is already subscribed, if so ignore it
                Boolean isTargetAlreadySubscribed = IsTargetDelegateAlreadySubscribed(currentDelegate, targetDelegate);
                if (!isTargetAlreadySubscribed)
                {
                    // If a different delegate is found, combine current with target
                    currentDelegate = Delegate.Combine(currentDelegate, targetDelegate);
                }
            }
            return currentDelegate;
        }

        /// <summary>
        /// Removes the delegate.
        /// </summary>
        /// <param name="currentDelegate">The current delegate.</param>
        /// <param name="targetDelegate">The target delegate.</param>
        /// <returns>Current delegate after removing target delegate</returns>
        private static Delegate RemoveDelegate(Delegate currentDelegate, Delegate targetDelegate)
        {
            if (currentDelegate != null)
            {
                // Remove target delegate from the list
                currentDelegate = Delegate.Remove(currentDelegate, targetDelegate);
            }
            return currentDelegate;
        }

        /// <summary>
        /// Determines whether target delegate is already subscribed in the specified current delegate.
        /// </summary>
        /// <param name="currentDelegate">The current delegate.</param>
        /// <param name="targetDelegate">The target delegate.</param>
        /// <returns>True if already subscribed else false</returns>
        private static Boolean IsTargetDelegateAlreadySubscribed(Delegate currentDelegate, Delegate targetDelegate)
        {
            Boolean doesExist = false;

            Delegate[] sourceDelegates = currentDelegate.GetInvocationList();            
            foreach (Delegate sourceDelegate in sourceDelegates)
            {
                // As delegate comparison fails across different instances of same class, we are comparing only the methodinfo 
                if (sourceDelegate.Method == targetDelegate.Method)
                {
                    doesExist = true;
                    break;
                }
            }

            return doesExist;
        }

        /// <summary>
        /// Gets the field information for event.
        /// </summary>
        /// <param name="classType">Type of the class.</param>
        /// <param name="eventName">Name of the event.</param>
        /// <returns>Field Info object</returns>
        private static FieldInfo GetFieldInfoForEvent(Type classType, String eventName)
        {
            FieldInfo eventFieldInfo = classType.GetField(eventName);
            if (eventFieldInfo == null)
            {
                throw new ArgumentException(String.Format("Event: {0} does not exist for type: {1} in Assembly: {2}", eventName, classType.FullName, classType.Assembly.FullName));
            }
            return eventFieldInfo;
        }

        /// <summary>
        /// Gets the type of the instance for.
        /// </summary>
        /// <param name="classType">Type of the class.</param>
        /// <returns>Instance of the specified type</returns>
        private static Object GetInstanceForType(Type classType)
        {
            // As of now, the static method that provides the instance is assumed to be "Instance"
            FieldInfo staticInstanceField = classType.GetField("Instance");
            if (staticInstanceField == null)
            {
                throw new ArgumentException(String.Format("Static field 'Instance' could not be found for type: {0} in Assembly: {1}", classType.FullName, 
                    classType.Assembly.FullName));
            }

            // Using the static "Instance" property, get the instance of the type
            Object typeInstance = staticInstanceField.GetValue(null);
            if (typeInstance == null)
            {
                throw new ArgumentException(String.Format("Could not create instance for type: {0} in Assembly: {1}", classType.FullName, classType.Assembly.FullName));
            }

            return typeInstance;
        }

        /// <summary>
        /// Gets the target delegate.
        /// </summary>
        /// <param name="eventHandler">The event handler.</param>
        /// <param name="targetDelegateType">Type of the target delegate.</param>
        /// <returns>Target delegate</returns>
        private static Delegate GetTargetDelegate(IMDMEventHandler eventHandler, Type targetDelegateType)
        {
            DiagnosticActivity diagnosticActivity = new DiagnosticActivity();
            TraceSettings traceSettings = MDMOperationContextHelper.GetCurrentTraceSettings();
            Delegate targetDelegate = null;

            if (traceSettings.IsBasicTracingEnabled)
            {
                diagnosticActivity.Start();
            }
            try
            {
                Type classType = GetType(eventHandler.AssemblyName, eventHandler.FullyQualifiedClassName);

                MethodInfo methodInfo = classType.GetMethod(eventHandler.EventHandlerMethodName, BindingFlags.Static | BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
                if (methodInfo == null)
                {
                    throw new ArgumentException(String.Format("Method: {0} does not exist for the Type: {1} in Assembly: {2}", eventHandler.EventHandlerMethodName,
                        eventHandler.FullyQualifiedClassName, eventHandler.AssemblyName));
                }
                
                if (eventHandler.IsHandlerMethodStatic)
                {
                    targetDelegate = Delegate.CreateDelegate(targetDelegateType, methodInfo);
                }
                else
                {
                    Object instance = Activator.CreateInstance(classType);
                    if (instance == null)
                    {
                        throw new ApplicationException(String.Format("Instance creation failed for Type: {0} in Assembly: {1}", eventHandler.FullyQualifiedClassName,
                            eventHandler.AssemblyName));
                    }

                    targetDelegate = Delegate.CreateDelegate(targetDelegateType, instance, methodInfo);
                }

                if (targetDelegate == null)
                {
                    throw new ApplicationException(String.Format("Delegate creation failed for Method: {0} in Type: {1}, Assembly: {2}", eventHandler.EventHandlerMethodName,
                        eventHandler.FullyQualifiedClassName, eventHandler.AssemblyName));
                }
            }
            catch (Exception ex)
            {
                diagnosticActivity.LogError(String.Format("Error occurred while getting the target delegate for event handler with id: {0}. Exception Details: {1}", eventHandler.Id, ex.Message));
                throw;
            }
            finally
            {
                if (traceSettings.IsBasicTracingEnabled)
                {
                    diagnosticActivity.Stop();
                }
            }

            return targetDelegate;
        }

        /// <summary>
        /// Gets the type.
        /// </summary>
        /// <param name="assemblyName">Name of the assembly.</param>
        /// <param name="className">Name of the class.</param>
        /// <returns>Type based on assembly and class name</returns>
        private static Type GetType(String assemblyName, String className)
        {
            DiagnosticActivity diagnosticActivity = new DiagnosticActivity();
            TraceSettings traceSettings = MDMOperationContextHelper.GetCurrentTraceSettings();
            Type classType = null;

            if (traceSettings.IsBasicTracingEnabled)
            {
                diagnosticActivity.Start();
            }
            try
            {
                String rootPath = AppDomain.CurrentDomain.BaseDirectory;
                if (_serviceType == MDMServiceType.Web || _serviceType == MDMServiceType.APIEngine)
                {
                    rootPath = String.Format(System.Globalization.CultureInfo.InvariantCulture, @"{0}bin\", rootPath);
                }

                String assemblyNameWithPath = String.Format(@"{0}{1}", rootPath, assemblyName);

                Assembly assembly = Assembly.LoadFrom(assemblyNameWithPath);
                if (assembly == null)
                {
                    throw new ArgumentException(String.Format("Assembly: {0} does not exist in the path: {1}", assemblyName, rootPath));
                }

                classType = assembly.GetType(className);
                if (classType == null)
                {
                    throw new ArgumentException(String.Format("Class: {0} does not exist for the Assembly: {1}", className, assemblyName));
                }
            }
            catch (Exception ex)
            {
                diagnosticActivity.LogError(String.Format("Error occurred while getting type: {0} from assembly: {1}. Exception Details: {2}", className, assemblyName, ex.Message));
                throw;
            }
            finally
            {
                if (traceSettings.IsBasicTracingEnabled)
                {
                    diagnosticActivity.Stop();
                }
            }

            return classType;
        }

        #endregion Event Subscription/UnSubscription Methods

        #region Validate Methods

        /// <summary>
        /// Validates the parameters.
        /// </summary>
        /// <param name="eventHandlerDataProvider">The event handler data provider.</param>
        /// <param name="serviceType">Type of the service.</param>
        /// <exception cref="System.ArgumentException">MDMServiceType is invalid if service type is not one of Web, APIEngine or JobService</exception>
        /// <exception cref="System.ArgumentNullException">MDMEventHandlerDataProvider is not initialized</exception>
        private static void ValidateParams(IMDMEventHandlerDataProvider eventHandlerDataProvider, MDMServiceType serviceType)
        {
            if (serviceType != MDMServiceType.APIEngine && serviceType != MDMServiceType.JobService && serviceType != MDMServiceType.Web)
            {
                throw new ArgumentException("MDMServiceType is invalid");
            }

            if (eventHandlerDataProvider == null)
            {
                throw new ArgumentNullException("MDMEventHandlerDataProvider is not initialized");
            }
        }

        /// <summary>
        /// Validates the cache notification event arguments.
        /// </summary>
        /// <param name="eventArgs">The instance containing the cache notification event data.</param>
        /// <exception cref="System.ArgumentNullException">CacheNotificationEventArgs is null</exception>
        private static void ValidateCacheNotificationEventArgs(CacheNotificationEventArgs eventArgs)
        {
            if (eventArgs == null)
            {
                throw new ArgumentNullException("CacheNotificationEventArgs is null");
            }
        }

        /// <summary>
        /// Checks if the event is applicable for the current service type
        /// </summary>
        /// <param name="eventHandler">The event handler.</param>
        /// <returns>True if the event handler is allowed to subscribe on the current instance service type</returns>
        private static Boolean IsEventSubscribedOnCurrentService(IMDMEventHandler eventHandler)
        {
            Boolean isEventSubscribed = false;            
            if (eventHandler.SubscribedOnServiceTypes != null && eventHandler.SubscribedOnServiceTypes.Contains(_serviceType))
            {
                isEventSubscribed = true;
            }
            return isEventSubscribed;
        }

        /// <summary>
        /// Validate the event handler based on appconfig value
        /// </summary>
        /// <param name="eventHandler">Indicates the event hadler</param>
        /// <returns>True if event handler is valid else False</returns>
        private static Boolean IsEventHandlerBasedOnAppConfig(IMDMEventHandler eventHandler)
        {
            Boolean isValid = true;
            String appConfigKey = eventHandler.AppConfigKeyName;

            if (!String.IsNullOrWhiteSpace(appConfigKey))
            {
                String appConfigValue = AppConfigurationHelper.GetAppConfig<String>(appConfigKey);

                if (String.Compare(eventHandler.AppConfigKeyValue, appConfigValue, true) != 0)
                {
                    isValid = false;
                }
            }
            return isValid;
        }

        /// <summary>
        /// Validate the event handler based on feature config value
        /// </summary>
        /// <param name="eventHandler">Indicates the event hadler</param>
        /// <returns>True if event handler is valid else False</returns>
        private static Boolean IsEventHandlerBasedOnFeatureConfig(IMDMEventHandler eventHandler)
        {
            Boolean isValid = true;
            String featureConfigKey = eventHandler.FeatureConfigKeyName;

            if (!String.IsNullOrWhiteSpace(featureConfigKey))
            {
                String[] featureConfigKeyValues = featureConfigKey.Split(new String[] { Constants.STRING_PATH_SEPARATOR }, StringSplitOptions.None);

                if (featureConfigKeyValues.Length > 2)
                {
                    MDMCenterApplication mdmApplication = MDMCenterApplication.MDMCenter;
                    ValueTypeHelper.EnumTryParse<MDMCenterApplication>(featureConfigKeyValues[0], true, out mdmApplication);

                    Boolean isMDMFeatureEnabled = MDMFeatureConfigHelper.IsMDMFeatureEnabled(mdmApplication, featureConfigKeyValues[1], featureConfigKeyValues[2]);
                    if (isMDMFeatureEnabled != eventHandler.FeatureConfigKeyValue)
                    {
                        isValid = false;
                    }
                }
                else
                {
                    isValid = false;
                }
            }
            return isValid;
        }

        #endregion Validate Methods        

        #region Initialize All Event handlers Methods

        /// <summary>
        /// Initializes all MDM events defined in the database, in a flush and fill manner
        /// </summary>
        /// <param name="isReloadTriggered">Flag to mention if reload of subscriptions is triggered].</param>
        private static void InitializeAllMDMEventSubscriptions(Boolean isReloadTriggered)
        {
            DiagnosticActivity diagnosticActivity = new DiagnosticActivity();
            TraceSettings traceSettings = MDMOperationContextHelper.GetCurrentTraceSettings();

            if (traceSettings.IsBasicTracingEnabled)
            {
                diagnosticActivity.Start();
            }

            try
            {
                ValidateParams(_eventHandlerDataProvider, _serviceType);

                IMDMEventHandlerCollection eventHandlerCollection = _eventHandlerDataProvider.GetMDMEventHandlers(null, _callerContext);

                if (traceSettings.IsBasicTracingEnabled)
                {
                    diagnosticActivity.LogData(String.Format("Fetched {0} event handlers from data provider.", eventHandlerCollection.Count), eventHandlerCollection.ToXml());
                }

                foreach (IMDMEventHandler eventHandler in eventHandlerCollection)
                {
                    if (IsEventSubscribedOnCurrentService(eventHandler) && eventHandler.Enabled && IsEventHandlerBasedOnAppConfig(eventHandler) && IsEventHandlerBasedOnFeatureConfig(eventHandler))
                    {
                        SubscribeEvent(eventHandler);

                        if (traceSettings.IsBasicTracingEnabled)
                        {
                            diagnosticActivity.LogMessageWithData(String.Format("Event Handler with Id: {0} is subscribed on server {1} with service type {2}.", Environment.MachineName, _serviceType.ToString(), eventHandler.Id), eventHandler.ToXml(), MessageClassEnum.Information);
                        }
                    }
                    else if (isReloadTriggered)
                    {
                        try
                        {
                            UnSubscribeEvent(eventHandler);

                            if (traceSettings.IsBasicTracingEnabled)
                            {
                                diagnosticActivity.LogMessageWithData(String.Format("Event Handler with Id: {0} is unsubscribed on server {1} with service type {2}.", Environment.MachineName, _serviceType.ToString(), eventHandler.Id), eventHandler.ToXml(), MessageClassEnum.Information);
                            }
                        }
                        catch (Exception ex)
                        {
                            //Swallow it as the exception might have occurred as the dll is not found for unsubscription on the current server
                            diagnosticActivity.LogError(String.Format("Error occurred while unsubscribing event handler with Id: {0} on server {1} with service type {2}. Exception Details: {3}", eventHandler.Id, Environment.MachineName, _serviceType.ToString(), ex.Message));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                diagnosticActivity.LogError(String.Format("Error occurred while subscribing event handlers on server {0} with service type {1}. Exception Details: {2}", Environment.MachineName, _serviceType.ToString(), ex.Message));
                throw;
            }
            finally
            {
                if (traceSettings.IsBasicTracingEnabled)
                {
                    diagnosticActivity.Stop();
                }
            }
        }

        #endregion Initialize All Event handlers Methods
        
        #endregion Private Methods
    }
}
