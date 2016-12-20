using System;
using System.Diagnostics;

namespace MDM.InstrumentationManager.Utility
{
    using MDM.Core;
    using MDM.BusinessObjects.Diagnostics;
    using MDM.Interfaces.Diagnostics;
    
    /// <summary>
    /// 
    /// </summary>
    public sealed class SvcLogTraceWriter
    {
        #region Static Private Fields

        /// <summary>
        /// Field denoting the source for tracing
        /// </summary>
        private static TraceSource _traceSource = null;

        #endregion
        
        #region Static Public Methods

        /// <summary>
        ///  static constructore
        /// </summary>
        static SvcLogTraceWriter()
        {
            _traceSource = new TraceSource("MDMTraceSource");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="diagnosticDataElements"></param>
        public static void WriteTrace(IDiagnosticDataElement[] diagnosticDataElements)
        {
            foreach (IDiagnosticDataElement diagnosticDataElement in diagnosticDataElements)
            {
                if (diagnosticDataElement is DiagnosticRecord)
                {
                    var diagnosticRecord = (DiagnosticRecord)diagnosticDataElement;
                    WriteTrace(diagnosticRecord);
                }

                if (diagnosticDataElement is DiagnosticActivity)
                {
                    var diagnosticActivity = (DiagnosticActivity)diagnosticDataElement;
                    WriteTrace(diagnosticActivity);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="diagnosticRecord"></param>
        public static void WriteTrace(DiagnosticRecord diagnosticRecord)
        {
            if (diagnosticRecord != null)
            {
                var traceEventType = GetTraceEventType(diagnosticRecord.MessageClass);
                Trace.CorrelationManager.ActivityId = diagnosticRecord.OperationId;
                _traceSource.TraceEvent(traceEventType, 0, diagnosticRecord.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="diagnosticActivity"></param>
        public static void WriteTrace(DiagnosticActivity diagnosticActivity)
        {
            if (diagnosticActivity != null)
            {
                if (diagnosticActivity.IsActivityStarted && !diagnosticActivity.IsActivityStopped)
                {
                    Trace.CorrelationManager.ActivityId = diagnosticActivity.OperationId;
                    _traceSource.TraceEvent(TraceEventType.Start, 0, diagnosticActivity.ActivityName);
                }
                else if (diagnosticActivity.IsActivityStarted && diagnosticActivity.IsActivityStopped)
                {
                    Trace.CorrelationManager.ActivityId = diagnosticActivity.OperationId;
                    _traceSource.TraceEvent(TraceEventType.Stop, 0, diagnosticActivity.ActivityName);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="messageClass"></param>
        /// <returns></returns>
        public static TraceEventType GetTraceEventType(MessageClassEnum messageClass)
        {
            TraceEventType traceEventType = TraceEventType.Information;

            switch (messageClass)
            {
                case MessageClassEnum.ActivityStart:
                    traceEventType = TraceEventType.Start;
                    break;
                case MessageClassEnum.ActivityStop:
                    traceEventType = TraceEventType.Stop;
                    break;
                case MessageClassEnum.Error:
                    traceEventType = TraceEventType.Error;
                    break;
                case MessageClassEnum.Warning:
                    traceEventType = TraceEventType.Warning;
                    break;
                case MessageClassEnum.Information:
                case MessageClassEnum.Success:
                case MessageClassEnum.TimeSpan:
                    traceEventType = TraceEventType.Information;
                    break;
                default:
                    traceEventType = TraceEventType.Verbose;
                    break;
            }

            return traceEventType;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="traceEventType"></param>
        /// <returns></returns>
        public static MessageClassEnum GetMessageClass(TraceEventType traceEventType)
        {
            MessageClassEnum messageClass = MessageClassEnum.Information;

            switch (traceEventType)
            {
                case TraceEventType.Start:
                    messageClass = MessageClassEnum.ActivityStart;
                    break;
                case TraceEventType.Stop:
                    messageClass = MessageClassEnum.ActivityStop;
                    break;
                case TraceEventType.Error:
                    messageClass = MessageClassEnum.Error;
                    break;
                case TraceEventType.Warning:
                    messageClass = MessageClassEnum.Warning;
                    break;
                case TraceEventType.Information:
                    messageClass = MessageClassEnum.Information;
                    break;
				case TraceEventType.Verbose:
					messageClass = MessageClassEnum.Verbose;
					break;
				default:
                    messageClass = MessageClassEnum.Information;
                    break;
            }

            return messageClass;
        }

        /// <summary>
        /// 
        /// </summary>
        public static void WriteEvent(TraceEventType eventType, Int32 id, String message)
        {
            _traceSource.TraceEvent(eventType, id, message);
        }

        #endregion
    }
}
