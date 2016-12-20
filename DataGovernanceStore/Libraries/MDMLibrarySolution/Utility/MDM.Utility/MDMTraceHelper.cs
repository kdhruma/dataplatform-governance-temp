using System;
using System.Diagnostics;

namespace MDM.Utility
{
    using Core;
    using System.Runtime.CompilerServices;
    using BusinessObjects.Diagnostics;
    using InstrumentationManager.Utility;
    using MDM.BusinessObjects;

    /// <summary>
    /// Helps in tracing events in MDM
    /// </summary>
    public class MDMTraceHelper
    {
        #region Fields
        
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
            return true;
        }

        /// <summary>
        /// Starts the new trace activity by emitting Start trace
        /// </summary>
        /// <param name="activityName">The Name of the activity</param>
        /// <param name="withNewIdentity">Boolean flag which says whether the activity needs to be started with new identity</param>
        /// <param name="filePath">Caller file path</param>
        /// <returns>Boolean flag saying whether start is successful or not</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Boolean StartTraceActivity(String activityName, Boolean withNewIdentity, [CallerFilePath] String filePath = "")
        {
            return StartTraceActivity(activityName, MDMTraceSource.General, withNewIdentity, filePath);
        }

        /// <summary>
        /// Starts the new trace activity by emitting Start trace
        /// </summary>
        /// <param name="activityName">The Name of the activity</param>
        /// <param name="traceSource">Source which is requesting for trace log</param>
        /// <param name="withNewIdentity">Boolean flag which says whether the activity needs to be started with new identity</param>
        /// <param name="filePath">Caller file path</param>
        /// <returns>Boolean flag saying whether start is successful or not</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Boolean StartTraceActivity(String activityName, MDMTraceSource traceSource, Boolean withNewIdentity, [CallerFilePath] String filePath = "")
        {
            return StartTraceActivity(Guid.Empty, activityName, traceSource, filePath);
        }

        /// <summary>
        /// Starts the new trace activity by emitting Start trace
        /// </summary>
        /// <param name="traceSource">Source which is requesting for trace log</param>
        /// <param name="activityName">The Name of the activity</param>
        /// <param name="filePath">Caller file path</param>
        /// <returns>Boolean flag saying whether start is successful or not</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Boolean StartTraceActivity(MDMTraceSource traceSource, [CallerMemberName] String activityName = "", [CallerFilePath] String filePath = "")
        {
            return StartTraceActivity(Guid.Empty, activityName, traceSource, filePath);
        }

        /// <summary>
        /// Starts the new trace activity by emitting Start trace
        /// </summary>
        /// <param name="activityId">The Id of the activity</param>
        /// <param name="activityName">The Name of the activity</param>
        /// <param name="traceSource">Source which is requesting for trace log</param>
        /// <param name="filePath">Caller file path</param>
        /// <returns>Boolean flag saying whether start is successful or not</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Boolean StartTraceActivity(Guid activityId, String activityName, MDMTraceSource traceSource, [CallerFilePath] String filePath = "")
        {
            TraceSettings traceSettings = MDMOperationContextHelper.GetCurrentTraceSettings();

            if (traceSettings.IsTracingEnabled)
            {
                DiagnosticActivity diagnosticActivity = CreateTraceActivity(traceSource, activityName, filePath);
                diagnosticActivity.Start();
            }

            return true;
        }

        /// <summary>
        /// Initializes instance of trace activity
        /// </summary>
        /// <param name="traceSource">Source which is requesting for trace log</param>
        /// <param name="activityName">The Name of the activity</param>
        /// <param name="filePath">Caller file path</param>
        /// <returns>Returns initialized instance of <see cref="DiagnosticActivity"/></returns>
        public static DiagnosticActivity CreateTraceActivity(MDMTraceSource traceSource, [CallerMemberName] String activityName = "", [CallerFilePath] String filePath = "")
        {
            return CreateTraceActivity(activityName, traceSource, null, filePath);
        }

        /// <summary>
        /// Initializes instance of trace activity
        /// </summary>
        /// <param name="activityName">The Name of the activity</param>
        /// <param name="traceSource">Source which is requesting for trace log</param>
        /// <param name="executionContext">Execution context instance or create empty Execution context if value is <b>null</b></param>
        /// <param name="filePath">Caller file path</param>
        /// <returns>Returns initialized instance of <see cref="DiagnosticActivity"/></returns>
        public static DiagnosticActivity CreateTraceActivity(String activityName, MDMTraceSource traceSource, ExecutionContext executionContext = null, [CallerFilePath] String filePath = "")
        {
            if (executionContext == null)
            {
                executionContext = new ExecutionContext();
            }

            if (!executionContext.LegacyMDMTraceSources.Contains(traceSource))
            {
                executionContext.LegacyMDMTraceSources.Add(traceSource);
            }

            DiagnosticActivity diagnosticActivity = new DiagnosticActivity(executionContext, activityName, filePath: filePath);

            TraceSettings traceSettings = MDMOperationContextHelper.GetCurrentTraceSettings();
            if (traceSettings.TracingMode == TracingMode.SelectiveComponentTracing)
            {
                diagnosticActivity.OperationId = Constants.ProfileTracingOperationId;
            }

            return diagnosticActivity;
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
        /// <returns>Boolean flag saying whether stop is successful or not</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Boolean StopTraceActivity()
        {
            return StopTraceActivity(null, MDMTraceSource.UnKnown);
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
            TraceSettings traceSettings = MDMOperationContextHelper.GetCurrentTraceSettings();

            if (traceSettings.IsTracingEnabled)
            {
                 DiagnosticActivity diagnosticActivity = (DiagnosticActivity)LogicalCallStackManager.Peek();

                 if (diagnosticActivity != null && diagnosticActivity.IsActivityStarted)
                 {
                     diagnosticActivity.Stop();
                 }
            }

            return true;
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
        public static Boolean EmitTraceEvent(TraceEventType eventType, String message, MDMTraceSource traceSource)
        {
            if (String.IsNullOrEmpty(message))
                return false;

            TraceSettings traceSettings = MDMOperationContextHelper.GetCurrentTraceSettings();

            MessageClassEnum messageClass = SvcLogTraceWriter.GetMessageClass(eventType);

            if (traceSettings.IsTracingEnabled || MessageClassEnum.Error == messageClass || MessageClassEnum.Warning == messageClass)
            {
                String messageRecord = String.Empty;
                
                if (message.Length > 2000)
                {
                    messageRecord = message.Substring(0, 2000);
                }
                else
                {
                    messageRecord = message;
                }

                DiagnosticActivity diagnosticActivity = (DiagnosticActivity)LogicalCallStackManager.Peek();

                if (diagnosticActivity != null)
                {
                    if (diagnosticActivity.ExecutionContext != null)
                    {
                        if (!diagnosticActivity.ExecutionContext.LegacyMDMTraceSources.Contains(traceSource))
                            diagnosticActivity.ExecutionContext.LegacyMDMTraceSources.Add(traceSource);
                    }

                    diagnosticActivity.LogMessage(messageClass, messageRecord);
                }
                else
                {
                    diagnosticActivity = DiagnosticActivity.GetUnClassifiedTraceActivity();
                    diagnosticActivity.LogMessage(messageClass, messageRecord);
                }
            }

            return true;
        }

        #endregion

        #region Private Methods

        #endregion

        #endregion
    }
}
