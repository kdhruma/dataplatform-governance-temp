using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Transactions;

namespace MDM.ConfigurationPrimitivesManager.Business
{
    using MDM.Core;
    using MDM.Core.Exceptions;
    using MDM.Utility;
    using MDM.BusinessObjects;
    using MDM.ConfigurationManager.Business;
    using MDM.ConfigurationPrimitivesManager.Data.SqlClient;
    using MDM.BufferManager;

    /// <summary>
    /// Specifies the business operations for Application Configuration Items
    /// </summary>
    public class ApplicationConfigurationItemBL : BusinessLogicBase
    {
        #region Constants

        private const Int32 UpdatingApplicationConfigurationItemCacheAttemptsCount = 3;

        private const String EmptyProgramNameReplacementPrefix = "MDM.ConfigurationPrimitivesManager.Business.ApplicationConfigurationItemBL.";
        private const String TracingPrefix = "MDM.ConfigurationPrimitivesManager.Business.ApplicationConfigurationItemBL.";

        #endregion

        #region Fields

        private readonly ApplicationConfigurationItemBufferManager _applicationConfigurationItemBufferManager = new ApplicationConfigurationItemBufferManager();

        private SecurityPrincipal _currentSecurityPrincipal;

        #endregion

        #region Properties

        /// <summary>
        /// Property denoting User Login
        /// </summary>
        public String UserLogin
        {
            get
            {
                try
                {
                    if (_currentSecurityPrincipal == null)
                    {
                        _currentSecurityPrincipal = SecurityPrincipalHelper.GetCurrentSecurityPrincipal();
                    }
                    return _currentSecurityPrincipal.CurrentUserName;
                }
                catch
                {
                    TraceError("Unable to fetch user login");
                }
                return null;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Get all raw application configurations by specified filters and without weighting
        /// </summary>
        /// <param name="eventSource">Event source filter</param>
        /// <param name="eventSubscriber">Event subscriber filter</param>
        /// <param name="callerContext">Name of the Application and Module which calls this method</param>
        /// <returns>ApplicationConfigurationItemCollection</returns>
        public ApplicationConfigurationItemCollection GetApplicationConfigurationsRaw(EventSource eventSource, EventSubscriber eventSubscriber, CallerContext callerContext)
        {
            ApplicationConfigurationItemCollection result = null;

            ValidateContext(callerContext, "GetApplicationConfigurationsRaw");

            result = _applicationConfigurationItemBufferManager.FindApplicationConfigurationItems(eventSource, eventSubscriber);
            if (result == null || !result.Any())
            {
                ApplicationConfigurationItemDA proxy = new ApplicationConfigurationItemDA();

                DBCommandProperties command = DBCommandHelper.Get(PrepareCallerContext(callerContext, "GetApplicationConfigurationsRaw"), MDMCenterModuleAction.Read);

                result = proxy.Get(eventSource, eventSubscriber, command);

                if (result != null && result.Any())
                {
                    _applicationConfigurationItemBufferManager.UpdateApplicationConfigurationItems(eventSource, eventSubscriber, result, UpdatingApplicationConfigurationItemCacheAttemptsCount);
                }
            }

            return result;
        }

        /// <summary>
        /// Insert, Update and Delete operations for Application Configuration Items
        /// </summary>
        /// <param name="applicationConfigurationItems">Collection of ApplicationConfigurationItem to process</param>
        /// <param name="callerContext">Context details of caller</param>
        /// <returns>Operation result</returns>
        public OperationResultCollection Process(ApplicationConfigurationItemCollection applicationConfigurationItems, CallerContext callerContext)
        {
            OperationResultCollection operationResult = null;

            StartTraceActivity("Process");
            try
            {
                const String methodName = "Process";
                ValidateContext(callerContext, methodName);

                ValidateApplicationConfigurationItemCollection(applicationConfigurationItems, methodName);

                Int32 createRecordsCounter = -1;
                foreach (ApplicationConfigurationItem item in applicationConfigurationItems)
                {
                    if (item.Action == ObjectAction.Create)
                    {
                        item.Id = createRecordsCounter;
                        createRecordsCounter--;
                    }
                }

                DBCommandProperties command = DBCommandHelper.Get(callerContext, MDMCenterModuleAction.Update);

                using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = Constants.DEFAULT_ISOLATION_LEVEL }))
                {
                    operationResult = new ApplicationConfigurationItemDA().Process(applicationConfigurationItems, PrepareCallerContext(callerContext, methodName), command, UserLogin);

                    EmitTraceEvent(TraceEventType.Information, "Creating, Updating, Deleting ApplicationConfigurationItems ends", MDMTraceSource.Configuration);

                    if (operationResult.OperationResultStatus == OperationResultStatusEnum.Successful)
                    {
                        #region Invalidate cache

                        RemoveAllApplicationConfigurationItemsFromCache();

                        #endregion

                        transactionScope.Complete();
                    }
                }
            }
            finally
            {
                StopTraceActivity("Process");
            }

            return operationResult;
        }

        /// <summary>
        /// Removes application configuration all items from cache
        /// </summary>
        public void RemoveAllApplicationConfigurationItemsFromCache()
        {
            _applicationConfigurationItemBufferManager.RemoveAllApplicationConfigurationItemsFromCache(true);
        }

        #endregion

        #region Private Methods

        private void ValidateContext(CallerContext callerContext, String methodName)
        {
            if (callerContext == null)
            {
                TraceError("CallerContext cannot be null.");
                throw new MDMOperationException("111846", "CallerContext cannot be null.", PopulateTraceRecord(methodName), String.Empty, methodName);
            }
        }

        private void ValidateApplicationConfigurationItem(ApplicationConfigurationItem item, String methodName)
        {
            if (item == null)
            {
                TraceError("ApplicationConfigurationItem is null");
                throw new MDMOperationException(String.Empty, "ApplicationConfigurationItem is null", PopulateTraceRecord(methodName), String.Empty, methodName);
            }
        }

        private void ValidateApplicationConfigurationItemCollection(ApplicationConfigurationItemCollection items, String methodName)
        {
            if (items == null)
            {
                TraceError("ApplicationConfigurationItems collection is null");
                throw new MDMOperationException(String.Empty, "ApplicationConfigurationItem collection is null", PopulateTraceRecord(methodName), String.Empty, methodName);
            }
            if (!items.Any())
            {
                TraceError("ApplicationConfigurationItems collection is empty");
                throw new MDMOperationException(String.Empty, "ApplicationConfigurationItem collection is empty", PopulateTraceRecord(methodName), String.Empty, methodName);
            }
            foreach (ApplicationConfigurationItem item in items)
            {
                ValidateApplicationConfigurationItem(item, methodName);
            }
        }

        private CallerContext PrepareCallerContext(CallerContext callerContext, String methodName)
        {
            if (String.IsNullOrWhiteSpace(callerContext.ProgramName))
            {
                CallerContext context = (CallerContext)callerContext.Clone();
                context.ProgramName = EmptyProgramNameReplacementPrefix + methodName;
                return context;
            }
            return callerContext;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Boolean StartTraceActivity(String record)
        {
            return Constants.TRACING_ENABLED ? MDMTraceHelper.StartTraceActivity(PopulateTraceRecord(record), MDMTraceSource.Configuration, false) : true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Boolean StopTraceActivity(String record)
        {
            return Constants.TRACING_ENABLED ? MDMTraceHelper.StopTraceActivity(PopulateTraceRecord(record), MDMTraceSource.Configuration) : true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Boolean TraceInformation(String record)
        {
            return Constants.TRACING_ENABLED ? MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, PopulateTraceRecord(record), MDMTraceSource.Configuration) : true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Boolean TraceError(String record)
        {
            return MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, PopulateTraceRecord(record), MDMTraceSource.Configuration);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static String PopulateTraceRecord(String record)
        {
            return TracingPrefix + record;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Boolean EmitTraceEvent(TraceEventType eventType, String message, MDMTraceSource traceSource)
        {
            if (eventType == TraceEventType.Information && !Constants.TRACING_ENABLED)
            {
                return true;
            }
            return MDMTraceHelper.EmitTraceEvent(eventType, PopulateTraceRecord(message), traceSource);
        }

        #endregion
    }
}