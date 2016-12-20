using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using MDM.BusinessRuleManagement.Business;
using MDM.Interfaces;
using ST = System.Timers;

namespace MDM.WCFServices
{
    using MDM.AdminManager.Business;
    using MDM.BusinessObjects;
    using MDM.CacheManager.Business;
    using MDM.ConfigurationManager.Business;
    using MDM.Core;
    using MDM.ExceptionManager;
    using MDM.InstrumentationManager.DataProcessors;
    using MDM.MonitoringManager.Business;
    using MDM.ParallelProcessingService;
    using MDM.Utility;
    using MDM.WorkflowRuntimeEngine;
    using RS.MDM.Configuration;
    using RS.MDM.ConfigurationObjects;

    public class GlobalEvents
    {
        /// <summary>
        /// Represents timer for elapsed timer instances polling
        /// </summary>
        private static ST.Timer _heartbeatPollingTimer = null;

        /// <summary>
        /// 
        /// </summary>
        public static Boolean IsApplicationStarted = false;

        #region Events

        /// <summary>
        /// 
        /// </summary>
        public static void OnApplicationStart()
        {
            try
            {
                //[Phani]TODO: Need to change this to AppConfig
                String assemblyName = AppConfiguration.GetSetting("MDMCenter.StronglyTypedEntityBO.AssemblyName");

                ProtoBufSerializationHelper.InitializeMetaTypesWithSubTypes(assemblyName);

                // Reset the operation context every time a new AppDomain is created to avoid unloaded AppDomain access issue
                MDMOperationContextHelper.ResetOperationContext();

                AppConfigurationHelper.InitializeAppConfig(new AppConfigProviderUsingDB());
                MDMOperationContextHelper.LoadComponentTracingSettings();
                DIConfig.RegisterConfiguration();
            
                DiagnosticDataProcessor.Start();

                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StartTraceActivity("WCFServices.GlobalEvents.Application_Start", true);

                #region Configure Thread throttling for this WCF instance

                SetAppThreads();

                #endregion

                #region Initialize WCF ServiceUserContext

                //Initialize WCF Service level ServiceUserContext and SercurityPrincipal for all service level internal operations.

                //TODO:: We need to run all the services as service account...Require system level change...
                String serviceUser = "system";
                String timeStamp = "NoTimeStamp";

                String formsAuthenticationTicket = AppConfigurationHelper.GetAppConfig<String>("MDM.WCFServices.MessageSecurity.IdentityKey", String.Empty);

                SecurityPrincipalBL securityPrincipalBL = new SecurityPrincipalBL();
                SecurityPrincipal serviceLevelSecurityPrincipal = securityPrincipalBL.Create(serviceUser, formsAuthenticationTicket, MDMCenterSystem.WcfService, timeStamp);

                if (serviceLevelSecurityPrincipal != null)
                {
                    ServiceUserContext serviceUserContext = ServiceUserContext.Initialize(serviceLevelSecurityPrincipal);
                }

                #endregion

                #region Initialize feature configurations

                // Initialize the MDM feature config Provider for the application.
                MDMFeatureConfigHelper.InitializeMDMFeatureConfig(new MDMFeatureConfigProviderUsingDB());

                #endregion

                #region Application Event attachments

                if (Constants.TRACING_ENABLED)
                {
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Starting event integrations...");
                }

                MDMEventHandlerManager.Initialize(new MDMEventHandlerDataProviderUsingDB(), MDMServiceType.APIEngine);

                if (Constants.TRACING_ENABLED)
                {
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Done with event integrations");
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Starting external plugin integrations...");
                }
                String externalPluginsFilePath = AppDomain.CurrentDomain.BaseDirectory + "App_Data\\ExternalPlugins.xml";

                if (Constants.TRACING_ENABLED)
                {
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "External Plugins File Path:" + externalPluginsFilePath);
                }

                MDM.Utility.ExtensionHelper.IntegratePluginAssemblies(externalPluginsFilePath);

                if (Constants.TRACING_ENABLED)
                {
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Done with external plugin integrations");
                }

                #endregion

                #region Load System Locale Details

                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Loading system locales...");

                LoadSystemLocaleDetails();

                if (Constants.TRACING_ENABLED)
                {
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Loaded system locales");
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Loading application messages...");
                }

                LoadApplicationMessages();

                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Loaded application messages");

                #endregion

                #region Initialze WCFServiceInstanceLoader

                Boolean isLocalCallsToWCFEnabled = AppConfigurationHelper.GetAppConfig<Boolean>("MDMCenter.WCFService.LocalAPICalls.Enabled", true);

                if (isLocalCallsToWCFEnabled)
                {
                    MDM.Utility.WCFServiceInstanceLoader.Initialize(typeof(MDMWCFServiceFactory).GetMethod("GetService"));
                }

                #endregion

                #region Initialize parallel processing engine

                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Starting parallel processing Engine...");

                MDM.ParallelProcessingService.ParallelProcessingEngine parallelProcessingEngine = MDM.ParallelProcessingService.ParallelProcessingEngine.GetSingleton();
                parallelProcessingEngine.Start();

                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Started parallel processing engine");

                #endregion

                #region Start Workflow Monitoring Service

                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Starting workflow monitoring service...");

                StartWorkflowMonitoringService();

                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Started workflow monitoring service");

                #endregion

                #region Start heartbeat thread for monitoring services

                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Starting heart beat monitoring service...");

                StartHeartBeatThread();

                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Started heart beat monitoring service");

                #endregion Start heartbeat thread for monitoring services

                GlobalEvents.IsApplicationStarted = true;
            }
            catch (Exception ex)
            {
                ExceptionManager.ExceptionHandler exceptionHandler = new ExceptionHandler(ex);
                //MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, String.Format("WCFServices.GlobalEvents.Application_Start failed with exception:{0}", ex.ToString()));
            }
            finally
            {
                try
                {
                    if (Constants.TRACING_ENABLED)
                        MDMTraceHelper.StopTraceActivity("WCFServices.GlobalEvents.Application_Start");
                }
                catch (Exception ex)
                {
                    ExceptionManager.ExceptionHandler exceptionHandler = new ExceptionHandler(ex);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static void OnApplicationEnd()
        {
            MDMOperationContextHelper.LoadComponentTracingSettings();

            Boolean isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;
            try
            {
                if (isTracingEnabled)
                {
                    MDMTraceHelper.StartTraceActivity(MDMTraceSource.APIFramework);
                }

                //Stop the ParallelEngine in nice manner 
                ParallelProcessingEngine engine = ParallelProcessingEngine.GetSingleton();
                Boolean result = engine.Stop();

                EventLog.WriteEntry("Application_End", "Parallel Engine Stopped");

                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Initiate stop of DiagnosticDataProcessor");
            }
            finally
            {
                if (isTracingEnabled)
                {
                    MDMTraceHelper.StopTraceActivity();
                }
            }

            DiagnosticDataProcessor.Stop();
            MDMOperationContextHelper.ResetOperationContext();
        }

        #endregion

        #region Private Methods

        private static void LoadSystemLocaleDetails()
        {
            ApplicationConfigurationBL applicationConfigurationBL = new ApplicationConfigurationBL();

            var localeConfiguration = applicationConfigurationBL.GetLocaleApplicationConfigurations(new ApplicationConfiguration((Int32)EventSource.MDMCenter, (Int32)EventSubscriber.LocaleConfiguration, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0));

            //Default system locale is en_WW
            LocaleEnum systemDataLocale = LocaleEnum.en_WW;
            LocaleEnum systemUILocale = LocaleEnum.en_WW;
            LocaleType modelDisplayLocaleType = LocaleType.DataLocale;
            LocaleType dataFormattingLocaleType = LocaleType.DataLocale;

            if (localeConfiguration != null && localeConfiguration.Count > 0 && localeConfiguration.Keys.Contains("LocaleConfiguration"))
            {
                if (localeConfiguration["LocaleConfiguration"] is LocaleConfig)
                {
                    ILocaleConfig localeConfig = localeConfiguration["LocaleConfiguration"];

                    systemDataLocale = localeConfig.SystemDataLocale;
                    systemUILocale = localeConfig.SystemUILocale;
                    modelDisplayLocaleType = localeConfig.ModelDisplayLocaleType;
                    dataFormattingLocaleType = localeConfig.DataFormattingLocaleType;

                    if (Constants.TRACING_ENABLED)
                    {
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("Configured System Data Locale is:{0}", systemDataLocale.GetDescription()));
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("Configured System UI Locale is:{0}", systemUILocale.GetDescription()));
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("Configured Model Display Locale Type is:{0}", modelDisplayLocaleType.ToString()));
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("Configured Data Formatting Locale Type is:{0}", dataFormattingLocaleType.ToString()));
                    }
                }
                else
                {
                    if (Constants.TRACING_ENABLED)
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Warning, String.Format("No System Locale configuration found. System will treat 'English - World Wide(en_WW)' as default system locales(for Data and UI)"));
                }
            }

            //Locale Config List: 0th position is system data locale; 1st position is system UI locale; 2nd position is model display locale type and 3rd position is data formatting locale type;
            String[] locales = new String[] { systemDataLocale.ToString(), systemUILocale.ToString(), modelDisplayLocaleType.ToString(), dataFormattingLocaleType.ToString() };

            String cacheKey = "RS_SDL";
            ICache cache = CacheFactory.GetCache();

            cache.Set(cacheKey, locales, DateTime.Now.AddHours(1));

            MDM.Utility.GlobalizationHelper.LoadSystemLocales();
        }

        private static void LoadApplicationMessages()
        {
            //Reading name of folder from ApplicationMessageManager.Path in web.config
            String fileDirectoryPath = AppConfigurationHelper.GetSettingAbsolutePath("ApplicationMessageManager.Path");

            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("Application messages file path:" + fileDirectoryPath));

            if (!String.IsNullOrWhiteSpace(fileDirectoryPath))
            {
                //Getting names of all subdirectories under FileDirectoryPath.
                String[] localeDirectories = Directory.GetDirectories(fileDirectoryPath);

                if (localeDirectories != null && localeDirectories.Count() > 0)
                {
                    String cacheKey = String.Empty;
                    String locale = String.Empty;
                    ICache cache = CacheFactory.GetCache();

                    //Loop through LocaleDirectories to load all XMLs one by one.
                    for (Int32 localecount = 0; localecount <= localeDirectories.Length - 1; localecount++)
                    {
                        locale = localeDirectories[localecount].ToString().Replace(fileDirectoryPath + "\\", "");

                        if (!String.IsNullOrWhiteSpace(locale))
                        {
                            cacheKey = locale + "_Messages";

                            if (cache[cacheKey] == null)
                            {
                                ApplicationMessageBL applicationMessageBL = new ApplicationMessageBL();
                                applicationMessageBL.LoadMessages(locale);
                            }
                        }
                    }
                }
            }
        }

        private static void StartWorkflowMonitoringService()
        {
            Boolean isWorkflowModuleEnabled = false;

            if (Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Starting Workflow Monitoring Service..");
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Getting AppConfig 'MDMCenter.Workflow.Enabled'..");
            }

            try
            {
                String strWorkflowEnabled = AppConfigurationHelper.GetAppConfig<String>("MDMCenter.Workflow.Enabled");

                isWorkflowModuleEnabled = ValueTypeHelper.BooleanTryParse(strWorkflowEnabled, false);

                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("AppConfig 'MDMCenter.Workflow.Enabled' get completed. Value: '{0}'", isWorkflowModuleEnabled.ToString()));
            }
            catch (Exception ex)
            {
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, String.Format("Failed to get AppConfig 'MDMCenter.Workflow.Enabled'. Error: {0}", ex.Message));
            }

            if (isWorkflowModuleEnabled)
            {
                if (!WorkflowMonitoringService.IsRunning)
                {
                    CallerContext callerContext = new CallerContext(MDMCenterApplication.PIM, MDMCenterModules.MDMAdvanceWorkflow);
                    WorkflowMonitoringService.Start(callerContext);
                }
            }
            else
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Workflow Module is not enabled. Workflow Monitoring Service cannot be started.");
            }
        }

        private static void SetAppThreads()
        {
            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Setting Application threads..");

            Int32 minWorker = 0;
            Int32 minIO = 0;
            Int32 maxWorker = 0;
            Int32 maxIO = 0;
            Int32 availWorker = 0;
            Int32 availIO = 0;

            //Get system min threads
            ThreadPool.GetMinThreads(out minWorker, out minIO);

            //Get system max threads
            ThreadPool.GetMaxThreads(out maxWorker, out maxIO);

            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("Current Thread Statistics - MinWorkerThreads = {0}; MinIOThreads = {1}; MaxWorkerThreads = {2}; MaxIOThreads = {3}; AvailableWorkerThreads = {4}; AvailableIOThreads = {5};", minWorker, maxWorker, minIO, maxIO, availWorker, availIO));

            //Get processor count
            Int32 processorCount = Environment.ProcessorCount;

            //Get min worker thread key
            Int32 minWorkerThreads = 0;
            String strMinWorkerThreadKey = AppConfiguration.GetSetting("MinWorkerThreadKey");
            if (!String.IsNullOrWhiteSpace(strMinWorkerThreadKey))
            {
                Int32 minWorkerThreadKey = ValueTypeHelper.Int32TryParse(strMinWorkerThreadKey, 0);

                if (minWorkerThreadKey > 0)
                {
                    minWorkerThreads = minWorkerThreadKey * processorCount;
                    if (Constants.TRACING_ENABLED)
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Configured Minimum Worker Thread Key: " + minWorkerThreadKey);
                }
            }

            //Get min worker thread key
            Int32 minIOThreads = 0;
            String strMinIOThreadKey = AppConfiguration.GetSetting("minIOThreadKey");
            if (!String.IsNullOrWhiteSpace(strMinWorkerThreadKey))
            {
                Int32 minIOThreadKey = ValueTypeHelper.Int32TryParse(strMinIOThreadKey, 0);

                if (minIOThreadKey > 0)
                {
                    minIOThreads = minIOThreadKey * processorCount;
                    if (Constants.TRACING_ENABLED)
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Configured Minimum IO Thread Key: " + minIOThreadKey);
                }
            }

            if (minWorkerThreads > 0 && minIOThreads > 0)
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("Setting MinWorkerThreads to {0} and MinIOThreads to {1}", minWorkerThreads, minIOThreads));
                ThreadPool.SetMinThreads(minWorkerThreads, minIOThreads);

                ThreadPool.GetMinThreads(out minWorker, out minIO);
                ThreadPool.GetAvailableThreads(out availWorker, out availIO);

                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("Thread Statistics after new settings - MinWorkerThreads = {0}; MinIOThreads = {1}; MaxWorkerThreads = {2}; MaxIOThreads = {3}; AvailableWorkerThreads = {4}; AvailableIOThreads = {5};", minWorker, maxWorker, minIO, maxIO, availWorker, availIO));
            }
            else
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Skipping thread settings as proper thread configurations are not available.");
            }
        }

        #region Heart Beat Monitoring Thread

        private static void StartHeartBeatThread()
        {
            Thread heartBeatThread = new Thread(new ThreadStart(MonitorHeartBeat)); ;
            heartBeatThread.Start();
        }

        private static void MonitorHeartBeat()
        {
            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StartTraceActivity("WCFService.SelfMonitoringService", true);

            //OperationContext.Current = new OperationContext(OperationContext.Current.Channel);
            Double timerScheduleIntervalInSeconds = 10;
            Double timerScheduleIntervalInMilliSeconds = timerScheduleIntervalInSeconds * 1000;

            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Getting polling interval key - MDMCenter.Monitoring.HeartBeatThread.PollingIntervalInSeconds");

            #region Read App config keys

            try
            {
                String strIntervalInSecs = AppConfigurationHelper.GetAppConfig<String>("MDMCenter.Monitoring.HeartBeatThread.PollingIntervalInSeconds");
                if (!String.IsNullOrWhiteSpace(strIntervalInSecs))
                {
                    timerScheduleIntervalInSeconds = ValueTypeHelper.DoubleTryParse(strIntervalInSecs, timerScheduleIntervalInSeconds);

                    //Convert seconds in milli seconds.
                    timerScheduleIntervalInMilliSeconds = timerScheduleIntervalInSeconds * 1000;
                }
            }
            catch
            {
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, "Missing AppConfig - 'MDMCenter.Monitoring.HeartBeatThread.PollingIntervalInSeconds'. Will assume value = " +
                    timerScheduleIntervalInSeconds + " and continue");
            }

            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "AppConfig - 'MDMCenter.Monitoring.HeartBeatThread.PollingIntervalInSeconds'. Value :  " + timerScheduleIntervalInSeconds);

            #endregion Read App config keys

            if (timerScheduleIntervalInMilliSeconds > 0)
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("Starting heartbeat thread polling ..", timerScheduleIntervalInMilliSeconds));

                _heartbeatPollingTimer = new ST.Timer(timerScheduleIntervalInMilliSeconds);
                _heartbeatPollingTimer.Elapsed += new ST.ElapsedEventHandler(UpdateServiceStatus);

                //Call handler once before enabling timer
                UpdateServiceStatus(null, null);

                _heartbeatPollingTimer.Enabled = true;
            }
            else
            {
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Warning, "Failed to start elapsed timer instances polling as polling interval is not defined.");
            }

            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StopTraceActivity("WCFService.SelfMonitoringService");
        }

        private static void UpdateServiceStatus(Object sender, ST.ElapsedEventArgs e)
        {
            try
            {
                _heartbeatPollingTimer.Enabled = false;

                var parallelizationEngineStatus = ParallelProcessingEngine.GetStatus();

                if (parallelizationEngineStatus.IsParallizationProcessingEngineStarted)
                {
                    if (Constants.TRACING_ENABLED)
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("Saving parallelization engine status"), MDMTraceSource.STF);

                    var monitorBL = new MonitorBL();
                    monitorBL.ProcessServiceStatus(System.Environment.MachineName, MDMServiceType.APIEngine, MDMServiceSubType.ParallelProcessingEngine, parallelizationEngineStatus.ToXml(), String.Empty, new CallerContext(MDMCenterApplication.MDMCenter, MDMCenterModules.Monitoring));
                }
                
                _heartbeatPollingTimer.Enabled = true;
            }
            catch (Exception ex)
            {
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, String.Format("Error occurred while recording Parallel processing engine status. Error: {0}", ex.Message));
            }
        }

        #endregion Heart Beat Monitoring Thread


        #endregion
    }
}
