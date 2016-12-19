using System;
using System.Diagnostics;
using System.Transactions;

namespace MDM.DiagnosticData.Business
{
    using MDM.BusinessObjects;
    using MDM.BusinessObjects.Diagnostics;
    using MDM.ConfigurationManager.Business;
    using MDM.Core;
    using MDM.Core.Exceptions;
    using MDM.DiagnosticData.Data.SqlClient;
    using MDM.Interfaces.Diagnostics;
    using MDM.Utility;
    using MDM.InstrumentationManager.Utility;
    using MDM.ExceptionManager.Handlers;

    /// <summary>
    /// 
    /// </summary>
    public class DiagnosticDataBL : BusinessLogicBase
    {
        #region Constants

        private const Int32 START_EVENT_ID = 1001;
        private const string START_EVENT = "Trace '{0}' was started by login '{1}'";
        private const Int32 STOP_EVENT_ID = 1002;
        private const string STOP_EVENT = "Trace '{0}' was stopped by login '{1}'";

        #endregion

        #region Fields

        /// <summary>
        /// Data access layer for diagnostic data
        /// </summary>
        private readonly DiagnosticDataDA _diagnosticDataDA = null;

        /// <summary>
        /// Field denoting the source for tracing
        /// </summary>
        private readonly TraceSource _traceSource = null;

        private static readonly Object _locker = new Object();

        /// <summary>
        /// Field to log event logs
        /// </summary>
        private readonly EventLogHandler eventLogHandler;

        #endregion

        #region Constructor

        /// <summary>
        /// Parameter less constructor
        /// </summary>
        public DiagnosticDataBL()
        {
            _diagnosticDataDA = new DiagnosticDataDA();
            _traceSource = new TraceSource("MDMTraceSource");
            eventLogHandler = new EventLogHandler();
        }

        #endregion 

        #region Public Methods

        /// <summary>
        /// Gets diagnostic activities for a given settings
        /// </summary>
        /// <param name="diagnosticReportSettings">Diagnostic report settings</param>
        /// <param name="callerContext">Context details of caller</param>        
        public DiagnosticActivityCollection GetActivities(DiagnosticReportSettings diagnosticReportSettings, CallerContext callerContext)
        {
            DiagnosticActivityCollection result = null;

            try
            {
                //TODO:: Put validations and tracing
                if (diagnosticReportSettings == null)
                {
                    diagnosticReportSettings = new DiagnosticReportSettings();
                }

                DBCommandProperties command = DBCommandHelper.Get(callerContext, MDMCenterModuleAction.Read);

                result = _diagnosticDataDA.GetActivities(diagnosticReportSettings, command);
            }
            catch (Exception exception)
            {
                String message = "DiagnosticDataBL GetActivities Failed: " + exception.ToString();

                MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, message, MDMTraceSource.Instrumentation); // todo remove
                _traceSource.TraceEvent(TraceEventType.Error, 0, message);                
            }
            finally
            {
                //MDMTraceHelper.StopTraceActivity(GetEntityActivityLogStatusProcessName);
            }

            return result;
        }

        /// <summary>
        /// Gets diagnostic records for a given operationId
        /// </summary>
        /// <param name="operationId">Operation Id</param>
        /// <param name="callerContext">Context details of caller</param>        
        public DiagnosticRecordCollection GetRecords(Guid operationId, CallerContext callerContext)
        {
            DiagnosticRecordCollection result = null;

            try
            {
                DBCommandProperties command = DBCommandHelper.Get(callerContext, MDMCenterModuleAction.Read);
                result = _diagnosticDataDA.GetRecords(operationId, command);
            }
            finally
            {
                //MDMTraceHelper.StopTraceActivity(GetEntityActivityLogStatusProcessName);
            }

            return result;
        }

        /// <summary>
        /// Process IDiagnosticDataElement DiagnosticActivityCollection and DiagnosticRecordCollection
        /// </summary>
        /// <param name="diagnosticActivities"></param>
        /// <param name="diagnosticRecords"></param>
        /// <param name="_callerContext"></param>
        /// <returns></returns>
        public Boolean Process(DiagnosticActivityCollection diagnosticActivities, DiagnosticRecordCollection diagnosticRecords, CallerContext _callerContext)
        {
            #region Diagnostics or Tracing

            // Should we do Activity based tracing here? as this may generate infinite msgs Posted to DiagDataProcessor
            //var traceSettings = MDMOperationContextHelper.GetRequestContextData().TraceSettings;
            //DiagnosticActivity activity = new DiagnosticActivity();

            //if (traceSettings.IsTracingEnabled) activity.Start();

            #endregion

            #region Create Diagnostic Elements array by merging activities and records


            Int32 diagnosticElementsCount = diagnosticActivities.Count + diagnosticRecords.Count;

            IDiagnosticDataElement[] diagnosticDataElements = new IDiagnosticDataElement[diagnosticElementsCount];

            Int32 elementIndex = 0, activityIndex = 0, recordIndex = 0;

            // Copy activities
            while (activityIndex < diagnosticActivities.Count)
            {
                diagnosticDataElements[elementIndex++] = diagnosticActivities.ElementAt(activityIndex++);
            }

            // Copy records
            while (recordIndex < diagnosticRecords.Count)
            {
                diagnosticDataElements[elementIndex++] = diagnosticRecords.ElementAt(recordIndex++);
            }


            #endregion

            try
            {
                #region Process using DiagnosticDA

                using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions() { IsolationLevel = Constants.DEFAULT_ISOLATION_LEVEL }))
                {
                    _diagnosticDataDA.Process(diagnosticDataElements);

                    transactionScope.Complete();
                }

                #endregion
            }
            catch (Exception exception)
            {
                string message = "DiagnosticDataBL Process Failed: " + exception.ToString();
                _traceSource.TraceEvent(TraceEventType.Error, 0, message);

                #region Process failed rows

                foreach (var diag in diagnosticDataElements)
                {
                    _traceSource.TraceEvent(TraceEventType.Error, 0, diag.ToXml());
                }

                return false;

                #endregion
            }

            return true;
        }

        /// <summary>
        /// Starts the diagnostic traces.
        /// </summary>
        /// <param name="callerContext">Indicates the caller context which contains the MDM application and MDM module</param>
        /// <returns>Returns the operation result based on the result</returns>
        public OperationResult StartDiagnosticTraces(CallerContext callerContext)
        {
            Boolean isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;
            if (isTracingEnabled)
            {
                MDMTraceHelper.StartTraceActivity("DiagnosticDataBL.StartDiagnosticTraces", MDMTraceSource.Instrumentation, false);
            }

            OperationResult result = new OperationResult();

            try
            {
                this.ValidateCallerContext(callerContext);
                DBCommandProperties command = DBCommandHelper.Get(callerContext, MDMCenterModuleAction.Read);

                Int32 sucessRowCount = _diagnosticDataDA.DeleteTraces(Constants.ProfileTracingOperationId, command);

                result = this.UpdateTraceSetting(true, TracingMode.SelectiveComponentTracing);
            }
            finally
            {
                result.RefreshOperationResultStatus();
                if (isTracingEnabled)
                {
                    MDMTraceHelper.StopTraceActivity("DiagnosticDataBL.StartDiagnosticTraces", MDMTraceSource.Instrumentation);
                }
            }

            return result;
        }

        /// <summary>
        /// Stop the diagnostic traces
        /// </summary>
        /// <param name="callerContext">Indicates the caller context which contains the MDM application and MDM module</param>
        /// <returns>Returns the operation result based on the result</returns>
        public OperationResult StopDiagnosticTraces(CallerContext callerContext)
        {
            Boolean isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;
            if (isTracingEnabled)
            {
                MDMTraceHelper.StartTraceActivity("DiagnosticDataBL.StopDiagnosticTraces", MDMTraceSource.Instrumentation, false);
            }

            OperationResult result = new OperationResult();

            try
            {
                this.ValidateCallerContext(callerContext);

                result = this.UpdateTraceSetting(false, TracingMode.None);
            }
            finally
            {
                result.RefreshOperationResultStatus();

                if (isTracingEnabled)
                {
                    MDMTraceHelper.StopTraceActivity("DiagnosticDataBL.StopDiagnosticTraces", MDMTraceSource.Instrumentation);
                }
            }

            return result;
        }

        #endregion

        #region Private Methods

        private OperationResult UpdateTraceSetting(Boolean isTracingEnabled, TracingMode tracingMode)
        {
            lock (_locker)
            {
                String tracingProfileXml = AppConfigurationHelper.GetAppConfig<String>("MDMCenter.TracingProfile", String.Empty);
                Double tracingProfileMaxDuration = AppConfigurationHelper.GetAppConfig<Double>("MDMCenter.Diagnostics.ProfileTracing.MaxDuration", 10);

                TracingProfile profile = null;

                if (!String.IsNullOrWhiteSpace(tracingProfileXml))
                {
                    TracingProfile.LoadCurrent(tracingProfileXml);
                }
                
                profile = TracingProfile.GetCurrent();
                Boolean oldTracingState = profile.TraceSettings.IsBasicTracingEnabled;

                if (isTracingEnabled)
                {
                    profile.StartDateTime = DateTime.Now;
                    profile.EndDateTime = DateTime.Now.AddMinutes(tracingProfileMaxDuration);
                }

                profile.TraceSettings.IsTracingEnabled = isTracingEnabled;
                profile.TraceSettings.TracingMode = tracingMode;

                OperationResult result = (OperationResult)AppConfigurationHelper.ProcessTracingProfile(profile);
                if (!result.HasError)
                {
                    result.OperationResultStatus = OperationResultStatusEnum.Successful;

                    if (isTracingEnabled != oldTracingState)
                    {
                        String user = SecurityPrincipalHelper.GetCurrentSecurityPrincipal().CurrentUserName;
                        if (isTracingEnabled)
                        {
                            eventLogHandler.WriteInformationLog(String.Format(START_EVENT, profile.Name, user), START_EVENT_ID);
                        }
                        else
                        {
                            eventLogHandler.WriteInformationLog(String.Format(STOP_EVENT, profile.Name, user), STOP_EVENT_ID);
                        }
                    }
                }

                return result;
            }
        }

        private void ValidateCallerContext(CallerContext callerContext)
        {
            if (callerContext == null)
            {
                throw new MDMOperationException("111846", "CallerContext cannot be null", "MDM.DiagnosticDataBL.Business.DiagnosticBL", String.Empty, String.Empty);
            }
        }

        #endregion 

    }
}
