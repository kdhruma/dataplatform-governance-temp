using System;
using System.Diagnostics;
using System.Threading;

namespace MDM.ParallelProcessingService.Bases
{
    using MDM.BusinessObjects;
    using MDM.Core;
    using MDM.Utility;
    using System.Runtime.CompilerServices;

    public class BaseCoreDataProcessor
    {
        public BaseCoreDataProcessor(CoreDataProcessorList processorName)
        {
            ProcessorNameString = processorName.ToString();
            ProcessorName = processorName;
            GetSecurityPrincipal();
            PopulateCallerContext();
            CancellationTokenSource = new CancellationTokenSource();
        }

        protected String ProcessorNameString;
        protected CoreDataProcessorList ProcessorName;

        protected CallerContext CallerContext;
        protected SecurityPrincipal SecurityPrincipal;
        protected CancellationTokenSource CancellationTokenSource;

        protected T GetConfigValue<T>(String valueName, T defaultValue)
        {
            String key = String.Format("MDMCenter.ParallelProcessingEngine.{0}.{1}", ProcessorNameString, valueName);
            T result = AppConfigurationHelper.GetAppConfig<T>(key, defaultValue);
            TraceVerbose(String.Format("AppConfig - '{0}'. Value :  {1}", key, result));
            return result;
        }

        [MethodImpl (MethodImplOptions.AggressiveInlining)]
        protected Boolean StartTraceActivity(String record)
        {
            return !MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled || MDMTraceHelper.StartTraceActivity(PopulateTraceRecord(record), MDMTraceSource.ParallelProcessingEngine, false);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected Boolean StopTraceActivity(String record)
        {
            return !MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled || MDMTraceHelper.StopTraceActivity(PopulateTraceRecord(record), MDMTraceSource.ParallelProcessingEngine);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected Boolean TraceInformation(String record)
        {
            return !MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled || MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, PopulateTraceRecord(record), MDMTraceSource.ParallelProcessingEngine);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected Boolean TraceVerbose(String record)
        {
            return !MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled || MDMTraceHelper.EmitTraceEvent(TraceEventType.Verbose, PopulateTraceRecord(record), MDMTraceSource.ParallelProcessingEngine);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected Boolean TraceWarning(String record, MDMTraceSource traceSource = MDMTraceSource.ParallelProcessingEngine)
        {
            return !MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled || MDMTraceHelper.EmitTraceEvent(TraceEventType.Warning, PopulateTraceRecord(record), traceSource);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected Boolean TraceError(String record, MDMTraceSource traceSource = MDMTraceSource.ParallelProcessingEngine)
        {
            return MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, PopulateTraceRecord(record), traceSource);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected String PopulateTraceRecord(String record)
        {
            return String.Format("MDM.ParallelProcessingService.CoreDataProcessors.{0} {1}", ProcessorNameString, record);
        }

        protected void PopulateCallerContext()
        {
            if (CallerContext == null)
            {
                CallerContext = new CallerContext { Application = MDMCenterApplication.DataQualityManagement, Module = MDMCenterModules.DQM, ProgramName = String.Format("MDMCenter.ParallelProcessingEngine.{0}", ProcessorNameString) };
            }
        }

        protected void GetSecurityPrincipal()
        {
            if (SecurityPrincipal == null)
            {
                SecurityPrincipal = SecurityPrincipalHelper.GetCurrentSecurityPrincipal();
            }
        }
    }
}