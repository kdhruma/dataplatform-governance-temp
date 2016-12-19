using System;
using System.Diagnostics;
using System.Linq;
using System.Transactions;

namespace MDM.IntegrationManager.Business
{
    using MDM.BusinessObjects;
    using MDM.BusinessObjects.Integration;
    using MDM.ConfigurationManager.Business;
    using MDM.Core;
    using MDM.Core.Exceptions;
    using MDM.IntegrationManager.Data;
    using MDM.MonitoringManager.Business;
    using MDM.Utility;
    using System.Collections.ObjectModel;

    /// <summary>
    /// Class to perform CURD operation on aggregation outbound queue item
    /// </summary>
    public class IntegrationOutboundAggregationQueueBL : BusinessLogicBase
    {
        #region Fields

        ServerInfoBL serverInfoBL = new ServerInfoBL();
        private SecurityPrincipal _securityPrincipal = null;
        private LocaleEnum _systemDataLocale = GlobalizationHelper.GetSystemUILocale();
        private IntegrationOutboundAggregationQueueDA _aggregationOutboundQueueDA = new IntegrationOutboundAggregationQueueDA();

        private const String _processMethodName = "MDM.IntegrationManager.Business.AggregationOutboundQueueBL.Process";
        private const String _getMethodName = "MDM.IntegrationManager.Business.AggregationOutboundQueueBL.Get";
        private const String _markAsProcessedMethodName = "MDM.IntegrationManager.Business.AggregationOutboundQueueBL.MarkAsProcessed";

        #endregion Fields

        #region Constructor

        /// <summary>
        /// Parameterless Constructor
        /// </summary>
        public IntegrationOutboundAggregationQueueBL()
        {
            GetSecurityPrincipal();
        }

        #endregion Constructor

        #region CUD Methods

        /// <summary>
        /// Create a aggregation outbound queue item
        /// </summary>
        /// <param name="aggregationOutboundQueueItem">Aggregation outbound queue item to be created</param>
        /// <param name="callerContext">Indicates the context of caller making call to this API</param>
        /// <returns>Status indicating the result of operation</returns>
        public OperationResult Create(AggregationOutboundQueueItem aggregationOutboundQueueItem, CallerContext callerContext)
        {
            this.Validate(aggregationOutboundQueueItem, "Create");
            aggregationOutboundQueueItem.Action = Core.ObjectAction.Create;
            return this.Process(new AggregationOutboundQueueItemCollection { aggregationOutboundQueueItem }, callerContext).FirstOrDefault();
        }

        /// <summary>
        /// Update a aggregation outbound queue item
        /// </summary>
        /// <param name="aggregationOutboundQueueItem">Aggregation outbound queue item to be updated</param>
        /// <param name="callerContext">Indicates the context of caller making call to this API</param>
        /// <returns>Status indicating the result of operation</returns>
        public OperationResult Update(AggregationOutboundQueueItem aggregationOutboundQueueItem, CallerContext callerContext)
        {
            this.Validate(aggregationOutboundQueueItem, "Update");
            aggregationOutboundQueueItem.Action = Core.ObjectAction.Update;
            return this.Process(new AggregationOutboundQueueItemCollection { aggregationOutboundQueueItem }, callerContext).FirstOrDefault();
        }

        /// <summary>
        /// Delete a aggregation outbound queue item
        /// </summary>
        /// <param name="aggregationOutboundQueueItem">Aggregation outbound queue item to be deleted</param>
        /// <param name="callerContext">Indicates the context of caller making call to this API</param>
        /// <returns>Status indicating the result of operation</returns>
        public OperationResult Delete(AggregationOutboundQueueItem aggregationOutboundQueueItem, CallerContext callerContext)
        {
            this.Validate(aggregationOutboundQueueItem, "Delete");
            aggregationOutboundQueueItem.Action = Core.ObjectAction.Delete;
            return this.Process(new AggregationOutboundQueueItemCollection { aggregationOutboundQueueItem }, callerContext).FirstOrDefault();
        }

        /// <summary>
        /// Process (create / update / delete) aggregation outbound queue items
        /// </summary>
        /// <param name="aggregationOutboundQueueItemCollection">Aggregation outbound queue items to be processed</param>
        /// <param name="callerContext">Indicates the context of caller making call to this API</param>
        /// <returns>Status indicating the result of operation</returns>
        public OperationResultCollection Process(AggregationOutboundQueueItemCollection aggregationOutboundQueueItemCollection, CallerContext callerContext)
        {
            OperationResultCollection operationResultCollection = null;

            if (Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.StartTraceActivity(_processMethodName, MDMTraceSource.Integration, false);
            }

            #region Parameter Validation

            ValidateCallerContext(callerContext);

            if (aggregationOutboundQueueItemCollection == null && aggregationOutboundQueueItemCollection.Count < 0)
            {
                String message = "AggregationOutboundQueueItemCollection must not be NULL.";
                throw new MDMOperationException("113199", message, "AggregationOutboundQueueBL.Process", String.Empty, "Process");
            }

            #endregion Parameter Validation

            try
            {
                String userName = String.Empty;
                if (_securityPrincipal != null)
                {
                    userName = _securityPrincipal.CurrentUserName;
                }

                if (String.IsNullOrWhiteSpace(callerContext.ProgramName))
                {
                    callerContext.ProgramName = _processMethodName;
                }

                if (!String.IsNullOrWhiteSpace(callerContext.ServerName))
                {
                    callerContext.ServerId = serverInfoBL.GetServerId(callerContext.ServerName);
                }
                else
                {
                    callerContext.ServerId = serverInfoBL.GetServerId(Environment.MachineName);
                }

                //Get command
                DBCommandProperties command = DBCommandHelper.Get(callerContext, Core.MDMCenterModuleAction.Update);
                operationResultCollection = CreateOperationResultCollection(aggregationOutboundQueueItemCollection);

                using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions() { IsolationLevel = Constants.DEFAULT_ISOLATION_LEVEL }))
                {
                    _aggregationOutboundQueueDA.Process(aggregationOutboundQueueItemCollection, callerContext.ProgramName, userName, (Int16)callerContext.ServerId, operationResultCollection, command);

                    transactionScope.Complete();
                }
            }
            finally
            {
                if (Constants.TRACING_ENABLED)
                {
                    MDMTraceHelper.StopTraceActivity(_processMethodName, MDMTraceSource.Integration);
                }
            }

            return operationResultCollection;
        }

        #endregion CUD Methods

        #region Get Queue

        /// <summary>
        /// Get aggregation outbound queue item for a given range of Ids
        /// </summary>
        /// <param name="logStatus">Aggregation outbound queue item status based on which data is to be  fetched</param>
        /// <param name="fromCount">Start of range of values to be fetched</param>
        /// <param name="toCount">End of range of values to be fetched</param>
        /// <param name="callerContext">Indicates the context of caller making call to this API</param>
        /// <returns>Aggregation Outbound queue item object</returns>
        public AggregationOutboundQueueItemCollection Get(String logStatus, Int64 fromCount, Int64 toCount, CallerContext callerContext)
        {
            AggregationOutboundQueueItemCollection aggregationOutboundQueueItems = null;
            if (Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.StartTraceActivity(_getMethodName, MDMTraceSource.Integration, false);
            }

            //Parameter Validation
            ValidateCallerContext(callerContext);

            String userName = String.Empty;
            if (_securityPrincipal != null)
            {
                userName = _securityPrincipal.CurrentUserName;
            }

            if (String.IsNullOrWhiteSpace(callerContext.ProgramName))
            {
                callerContext.ProgramName = _getMethodName;
            }

            if (!String.IsNullOrWhiteSpace(callerContext.ServerName))
            {
                callerContext.ServerId = serverInfoBL.GetServerId(callerContext.ServerName);
            }
            else
            {
                callerContext.ServerId = serverInfoBL.GetServerId(Environment.MachineName);
            }

            DBCommandProperties command = DBCommandHelper.Get(callerContext, MDMCenterModuleAction.Read);

            if (fromCount > -1 && toCount > -1 && !String.IsNullOrWhiteSpace(logStatus))
            {
                using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions() { IsolationLevel = Constants.DEFAULT_ISOLATION_LEVEL }))
                {
                    aggregationOutboundQueueItems = _aggregationOutboundQueueDA.Get(IntegrationType.Outbound, logStatus, fromCount, toCount, callerContext.ProgramName, userName, (Int16)callerContext.ServerId, command);
                    transactionScope.Complete();
                }
            }

            if (Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.StopTraceActivity(_getMethodName, MDMTraceSource.Integration);
            }

            return aggregationOutboundQueueItems;
        }

        /// <summary>
        /// Update AggregationOutboundQueueItem table record to mark that it is loaded.
        /// </summary>
        /// <param name="outboundQueueItemId">Aggregation outbound queue item Id to update for loaded status</param>
        /// <param name="callerContext">Indicates the context of caller making call to this API</param>
        /// <returns>Status indicating the result of operation</returns>
        public Boolean MarkAsProcessed(Collection<Int64> aggregationOutboundQueueItemIds, CallerContext callerContext)
        {
            Boolean result = true;
            if (Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.StartTraceActivity(_markAsProcessedMethodName, MDMTraceSource.Integration, false);
            }

            #region Parameter Validation

            ValidateCallerContext(callerContext);

            if (aggregationOutboundQueueItemIds.Count <= 0)
            {
                String message = "No aggregation queue items to mark as processed.";
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Warning, message);
                return false;
            }

            #endregion Parameter Validation

            try
            {
                String userName = String.Empty;
                if (_securityPrincipal != null)
                {
                    userName = _securityPrincipal.CurrentUserName;
                }

                if (String.IsNullOrWhiteSpace(callerContext.ProgramName))
                {
                    callerContext.ProgramName = _markAsProcessedMethodName;
                }

                if (!String.IsNullOrWhiteSpace(callerContext.ServerName))
                {
                    callerContext.ServerId = serverInfoBL.GetServerId(callerContext.ServerName);
                }
                else
                {
                    callerContext.ServerId = serverInfoBL.GetServerId(Environment.MachineName);
                }

                //Get command
                DBCommandProperties command = DBCommandHelper.Get(callerContext, Core.MDMCenterModuleAction.Update);

                using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions() { IsolationLevel = Constants.DEFAULT_ISOLATION_LEVEL }))
                {
                    result = _aggregationOutboundQueueDA.MarkAsProcessed(aggregationOutboundQueueItemIds, callerContext.ProgramName, userName, (Int16)callerContext.ServerId, command);

                    if (result == true)
                    {
                        transactionScope.Complete();
                    }
                }
            }
            finally
            {
                if (Constants.TRACING_ENABLED)
                {
                    MDMTraceHelper.StopTraceActivity(_markAsProcessedMethodName, MDMTraceSource.Integration);
                }
            }

            return result;
        }

        #endregion Get Queue

        #region Private Methods

        /// <summary>
        /// Get security principal for currently logged in user
        /// </summary>
        private void GetSecurityPrincipal()
        {
            if (_securityPrincipal == null)
            {
                _securityPrincipal = SecurityPrincipalHelper.GetCurrentSecurityPrincipal();
            }
        }

        /// <summary>
        /// Validate input parameters
        /// </summary>
        private void Validate(AggregationOutboundQueueItem aggregationOutboundQueueItem, String methodName)
        {
            if (aggregationOutboundQueueItem == null)
            {
                String message = "AggregationOutboundQueueItem must not be NULL.";
                throw new MDMOperationException("113200", message, "AggregationOutboundQueueBL." + methodName, String.Empty, methodName);
            }
        }

        /// <summary>
        /// Validate caller context if it is null or not.
        /// </summary>
        /// <param name="callerContext">Caller context to validate</param>
        private void ValidateCallerContext(CallerContext callerContext)
        {
            if (callerContext == null)
            {
                throw new MDMOperationException("111846", "CallerContext cannot be null.", "AggregationOutboundQueueBL.Process", String.Empty, "Process");
            }
        }

        private OperationResultCollection CreateOperationResultCollection(AggregationOutboundQueueItemCollection aggregationOutboundQueueItemCollection)
        {
            OperationResultCollection operationResultCollection = new OperationResultCollection();
            if (aggregationOutboundQueueItemCollection != null && aggregationOutboundQueueItemCollection.Count > 0)
            {
                Int32 counter = -1;
                foreach (AggregationOutboundQueueItem item in aggregationOutboundQueueItemCollection)
                {
                    if (String.IsNullOrEmpty(item.ReferenceId))
                    {
                        item.ReferenceId = ( counter-- ).ToString();
                    }

                    OperationResult or = new OperationResult();
                    or.ReferenceId = item.ReferenceId;
                    or.OperationResultStatus = OperationResultStatusEnum.None;
                    operationResultCollection.Add(or);
                }
            }
            return operationResultCollection;
        }

        #endregion Private Methods
    }
}
