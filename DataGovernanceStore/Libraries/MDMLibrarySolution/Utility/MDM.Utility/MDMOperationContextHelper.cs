using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.Web;
using MDM.Core;

namespace MDM.Utility
{
    using MDM.OperationContextManager.Business;
    using MDM.BusinessObjects;
    using MDM.ExceptionManager;

    /// <summary>
    /// 
    /// </summary>
    public static class MDMOperationContextHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static IRequestContextData GetRequestContextData()
        {
            return MDMOperationContext.Current.RequestContextData;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static TraceSettings GetCurrentTraceSettings()
        {
            try
            {
                return MDMOperationContext.Current.RequestContextData.TraceSettings;
            }
            catch(Exception ex)
            {
                new ExceptionHandler(ex);
                return new TraceSettings();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static void LoadComponentTracingSettings()
        {
            var requestContextData = MDMOperationContext.Current.RequestContextData;

            Boolean isComponentTracingEnabled = MDM.Core.Constants.TRACING_ENABLED || MDM.Core.Constants.PERFORMANCE_TRACING_ENABLED;
            TracingMode tracingMode = isComponentTracingEnabled ? TracingMode.SelectiveComponentTracing : TracingMode.None;
            TracingLevel tracingLevel = isComponentTracingEnabled ? TracingLevel.Basic : TracingLevel.None;
            var traceSettings = new TraceSettings(isComponentTracingEnabled, tracingMode, tracingLevel);

            requestContextData.TraceSettings = traceSettings;
            requestContextData.OperationId = Constants.ProfileTracingOperationId;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="operationId"></param>
        /// <param name="traceSettings"></param>
        public static void LoadOperationTracingSettings(Guid operationId, TraceSettings traceSettings)
        {
            var requestContextData = MDMOperationContext.Current.RequestContextData;
            requestContextData.TraceSettings = traceSettings;
            requestContextData.OperationId = operationId;
        }

        /// <summary>
        /// 
        /// </summary>
        public static void ResetOperationContext()
        {
            MDMOperationContext.Reset();
        }
    }
}

