using System;
using System.Collections.ObjectModel;
using ST = System.Timers;
using System.Threading;
using System.ServiceModel;
using System.Diagnostics;

namespace MDM.WorkflowRuntimeEngine
{
    using MDM.Core;
    using MDM.Utility;
    using MDM.BusinessObjects;
    using MDM.BusinessObjects.Workflow;
    using MDM.Workflow.TrackingManager.Business;

    /// <summary>
    /// Monitors escalations and elapsed timer instances.
    /// </summary>
    public class WorkflowMonitoringService
    {
        #region Fields

        /// <summary>
        /// Thread used to monitor workflow instances
        /// </summary>
        private static Thread _workflowMonitoringService;

        /// <summary>
        /// Represents timer for escalation polling 
        /// </summary>
        private static ST.Timer _escalationTimer = null;

        /// <summary>
        /// Represents timer for elapsed timer instances polling
        /// </summary>
        private static ST.Timer _elapsedTimerInstancesPollingTimer = null;

        /// <summary>
        /// Represents current operation context
        /// </summary>
        private static OperationContext _operationContext = null;

        /// <summary>
        /// Context which tells which application/module called this API
        /// </summary>
        private static CallerContext _callerContext = new CallerContext(MDMCenterApplication.MDMCenter, MDMCenterModules.MDMAdvanceWorkflow);

        #endregion

        #region Properties

        /// <summary>
        /// Property denoting whether the Workflow Monitoring service is running or not
        /// </summary>
        public static Boolean IsRunning
        {
            get
            {
                if (_workflowMonitoringService == null || !_workflowMonitoringService.IsAlive)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        #endregion

        #region Methods

        #region Public Methods

        /// <summary>
        /// Starts the workflow monitoring service
        /// </summary>
        public static void Start(CallerContext callerContext)
        {
            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Starting Workflow Monitoring Service..");

            try
            {
                //Assign contexts
                _callerContext = callerContext;
                _operationContext = OperationContext.Current;

                _workflowMonitoringService = new Thread(new ThreadStart(MonitorWorkflows));

                _workflowMonitoringService.Start();

                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Started Workflow Monitoring Service.");
            }
            catch (Exception ex)
            {
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, String.Format("Failed to start Workflow Monitoring Service. Error: {0}", ex.Message));
            }
        }

        #endregion

        #region Private Methods

        private static void MonitorWorkflows()
        {
            OperationContext.Current = _operationContext;

            StartEscalationsPolling();

            StartElapsedTimerInstancesPolling();
        }

        private static void StartEscalationsPolling()
        {
            Double timerScheduleInterval = 0;

            if (Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Starting escalation polling..");
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Getting polling interval key - Workflow.Escalation.Service.ScheduleInterval");
            }
            try
            {
                //Get the Escalation Service Schedule Interval AppConfig key
                String appConfigValue = AppConfigurationHelper.GetAppConfig<String>("Workflow.Escalation.Service.ScheduleInterval");

                timerScheduleInterval = ValueTypeHelper.DoubleTryParse(appConfigValue, 0);

                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("Polling interval key get completed. Value: '{0} hours'", timerScheduleInterval));
            }
            catch (Exception ex)
            {
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, String.Format("Failed to get polling interval.Error: {0}", ex.Message));
            }

            if (timerScheduleInterval > 0)
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("Starting escalation polling with interval '{0} hours'..", timerScheduleInterval));

                //Convert the interval from hours into milliseconds
                timerScheduleInterval = timerScheduleInterval * 60 * 60 * 1000;

                _escalationTimer = new ST.Timer(timerScheduleInterval);
                _escalationTimer.Elapsed += new ST.ElapsedEventHandler(EscalationInterval_Elapsed);

                //Call handler once before enabling timer
                EscalationInterval_Elapsed(null, null);

                _escalationTimer.Enabled = true;
            }
            else
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Failed to start escalation polling as polling interval is not defined.");
            }
        }

        private static void StartElapsedTimerInstancesPolling()
        {
            Double timerScheduleInterval = 0;

            if (Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Starting elapsed timer instances polling..");
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Getting polling interval key - MDMCenter.Workflow.ElapsedTimerInstances.PollingInterval");
            }
            try
            {
                //Get the Elapsed Timer polling Interval AppConfig key
                String appConfigValue = AppConfigurationHelper.GetAppConfig<String>("MDMCenter.Workflow.ElapsedTimerInstances.PollingInterval");

                timerScheduleInterval = ValueTypeHelper.DoubleTryParse(appConfigValue, 0);

                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("Polling interval key get completed. Value: '{0} hours'", timerScheduleInterval));
            }
            catch (Exception ex)
            {
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, String.Format("Failed to get polling interval.Error: {0}", ex.Message));
            }

            if (timerScheduleInterval > 0)
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("Starting elapsed timer instances polling with interval '{0} hours'..", timerScheduleInterval));

                //Convert the interval from hours into milliseconds
                timerScheduleInterval = timerScheduleInterval * 60 * 60 * 1000;

                _elapsedTimerInstancesPollingTimer = new ST.Timer(timerScheduleInterval);
                _elapsedTimerInstancesPollingTimer.Elapsed += new ST.ElapsedEventHandler(ElapsedTimerInstancesPollingInterval_Elapsed);

                //Call handler once before enabling timer
                ElapsedTimerInstancesPollingInterval_Elapsed(null, null);

                _elapsedTimerInstancesPollingTimer.Enabled = true;
            }
            else
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Failed to start elapsed timer instances polling as polling interval is not defined.");
            }
        }

        private static void EscalationInterval_Elapsed(Object sender, ST.ElapsedEventArgs e)
        {
            try
            {
                _escalationTimer.Enabled = false;

                //Process Escalation
                EscalationBL escalationBL = new EscalationBL();
                Collection<Escalation> escalationData = escalationBL.Process(_callerContext);

                //TODO:: Uncomment this part once the Messaging has been included..

                ////Send Message to the respective user
                //Collection<Escalation> alertUser = new Collection<Escalation>();
                //Collection<Escalation> escalateToManager = new Collection<Escalation>();
                //Collection<Escalation> removedFromQueue = new Collection<Escalation>();

                //foreach (Escalation escalation in escalationData)
                //{
                //    switch (escalation.EscalationLevel)
                //    {
                //        case EscalationLevel.AlertUser:
                //            alertUser.Add(escalation);
                //            break;
                //        case EscalationLevel.EscalateToManager:
                //            escalateToManager.Add(escalation);
                //            break;
                //        case EscalationLevel.RemoveFromQueue:
                //            removedFromQueue.Add(escalation);
                //            break;
                //        default:
                //            break;
                //    }
                //}

                //MDMS.MessageService messageService = new MDMS.MessageService();
                //OperationResult operationResult = new OperationResult();

                //messageService.SendWorkflowMessages(alertUser, "EscalationService.AlertUser", ref operationResult);
                //messageService.SendWorkflowMessages(escalateToManager, "EscalationService.EscalateToManager", ref operationResult);
                //messageService.SendWorkflowMessages(removedFromQueue, "EscalationService.RemovedFromQueue", ref operationResult);

                _escalationTimer.Enabled = true;
            }
            catch (Exception ex)
            {
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, String.Format("Error occurred while processing escalations. Error: {0}", ex.Message));
            }
        }

        private static void ElapsedTimerInstancesPollingInterval_Elapsed(Object sender, ST.ElapsedEventArgs e)
        {
            try
            {
                _elapsedTimerInstancesPollingTimer.Enabled = false;

                WorkflowRuntime workflowRuntime = new WorkflowRuntime();
                workflowRuntime.LoadElapsedTimerInstances(_callerContext);

                _elapsedTimerInstancesPollingTimer.Enabled = true;
            }
            catch (Exception ex)
            {
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, String.Format("Error occurred while loading elapsed timer instances. Error: {0}", ex.Message));
            }
        }

        #endregion

        #endregion
    }
}
