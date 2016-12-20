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

    /// <summary>
    /// Class to perform CURD operation on outbound queue item
    /// </summary>
    public class OutboundQueueBL : BusinessLogicBase
    {
        #region Fields

        ServerInfoBL serverInfoBL = new ServerInfoBL();
        private SecurityPrincipal _securityPrincipal = null;
        private LocaleEnum _systemDataLocale = GlobalizationHelper.GetSystemUILocale();
        private OutboundQueueDA _outboundQueueDA = new OutboundQueueDA();

        private const String _processMethodName = "MDM.IntegrationManager.Business.OutboundQueueBL.Process";
        private const String _getMethodName = "MDM.IntegrationManager.Business.OutboundQueueBL.Get";
        private const String _markAsProcessedMethodName = "MDM.IntegrationManager.Business.OutboundQueueBL.MarkAsProcessed";

        #endregion Fields

        #region Constructor

        /// <summary>
        /// Parameterless Constructor
        /// </summary>
        public OutboundQueueBL()
        {
            GetSecurityPrincipal();
        }

        #endregion Constructor

        #region CUD Methods

        /// <summary>
        /// Create a outbound queue item
        /// </summary>
        /// <param name="outboundQueueItem">Outbound queue item to be created</param>
        /// <param name="callerContext">Indicates the context of caller making call to this API</param>
        /// <returns>Status indicating the result of operation</returns>
        public OperationResult Create(OutboundQueueItem outboundQueueItem, CallerContext callerContext)
        {
            this.Validate(outboundQueueItem, "Create");
            outboundQueueItem.Action = Core.ObjectAction.Create;
            return this.Process(new OutboundQueueItemCollection { outboundQueueItem }, callerContext).FirstOrDefault();
        }

        /// <summary>
        /// Update a outbound queue item
        /// </summary>
        /// <param name="outboundQueueItem">Outbound queue item to be updated</param>
        /// <param name="callerContext">Indicates the context of caller making call to this API</param>
        /// <returns>Status indicating the result of operation</returns>
        public OperationResult Update(OutboundQueueItem outboundQueueItem, CallerContext callerContext)
        {
            this.Validate(outboundQueueItem, "Update");
            outboundQueueItem.Action = Core.ObjectAction.Update;
            return this.Process(new OutboundQueueItemCollection { outboundQueueItem }, callerContext).FirstOrDefault();
        }

        /// <summary>
        /// Delete a outbound queue item
        /// </summary>
        /// <param name="outboundQueueItem">Outbound queue item to be deleted</param>
        /// <param name="callerContext">Indicates the context of caller making call to this API</param>
        /// <returns>Status indicating the result of operation</returns>
        public OperationResult Delete(OutboundQueueItem outboundQueueItem, CallerContext callerContext)
        {
            this.Validate(outboundQueueItem, "Delete");
            outboundQueueItem.Action = Core.ObjectAction.Delete;
            return this.Process(new OutboundQueueItemCollection { outboundQueueItem }, callerContext).FirstOrDefault();
        }

        /// <summary>
        /// Process (create / update / delete) outbound queue items
        /// </summary>
        /// <param name="outboundQueueItemCollection">Outbound queue items to be processed</param>
        /// <param name="callerContext">Indicates the context of caller making call to this API</param>
        /// <returns>Status indicating the result of operation</returns>
        public OperationResultCollection Process(OutboundQueueItemCollection outboundQueueItemCollection, CallerContext callerContext)
        {
            OperationResultCollection operationResultCollection = null;

            if (Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.StartTraceActivity(_processMethodName, MDMTraceSource.Integration, false);
            }

            #region Parameter Validation

            ValidateCallerContext(callerContext);

            if (outboundQueueItemCollection == null && outboundQueueItemCollection.Count < 0)
            {
                String message = "OutboundQueueItemCollection must not be NULL.";
                throw new MDMOperationException("112928", message, "OutboundQueueBL.Process", String.Empty, "Process");
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
                operationResultCollection = CreateOperationResultCollection(outboundQueueItemCollection);

                using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions() { IsolationLevel = Constants.DEFAULT_ISOLATION_LEVEL }))
                {
                    _outboundQueueDA.Process(outboundQueueItemCollection, callerContext.ProgramName, userName, (Int16)callerContext.ServerId, operationResultCollection, command);

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
        /// Get outbound queue item for a given range of Ids
        /// </summary>
        /// <param name="logStatus">Outbound queue item status based on which data is to be  fetched</param>
        /// <param name="fromCount">Start of range of values to be fetched</param>
        /// <param name="toCount">End of range of values to be fetched</param>
        /// <param name="callerContext">Indicates the context of caller making call to this API</param>
        /// <returns>Outbound queue item object</returns>
        public OutboundQueueItemCollection Get(String logStatus, Int64 fromCount, Int64 toCount, CallerContext callerContext)
        {
            OutboundQueueItemCollection outboundQueueItems = null;
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
                    outboundQueueItems = _outboundQueueDA.Get(logStatus, fromCount, toCount, callerContext.ProgramName, userName, (Int16)callerContext.ServerId, command);
                    transactionScope.Complete();
                }
            }

            if (Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.StopTraceActivity(_getMethodName, MDMTraceSource.Integration);
            }

            return outboundQueueItems;
        }

        /// <summary>
        /// Update OutboundQueueItem table record to mark that it is loaded.
        /// </summary>
        /// <param name="outboundQueueItemId">Outbound outbound queue item Id to update for loaded status</param>
        /// <param name="callerContext">Indicates the context of caller making call to this API</param>
        /// <returns>Status indicating the result of operation</returns>
        public Boolean MarkAsProcessed(Int64 outboundQueueItemId,CallerContext callerContext)
        {
            Boolean result = true;
            if (Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.StartTraceActivity(_markAsProcessedMethodName, MDMTraceSource.Integration, false);
            }

            #region Parameter Validation

            ValidateCallerContext(callerContext);

            if (outboundQueueItemId < 0)
            {
                String message = "OutboundQueueItemID must not be less than zero.";
                throw new MDMOperationException("112929", message, _markAsProcessedMethodName, String.Empty, _markAsProcessedMethodName);
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
                    result = _outboundQueueDA.MarkAsProcessed(outboundQueueItemId, callerContext.ProgramName, userName, (Int16)callerContext.ServerId, command);

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
        private void Validate(OutboundQueueItem outboundQueueItem, String methodName)
        {
            if (outboundQueueItem == null)
            {
                String message = "OutboundQueueItem must not be NULL.";
                throw new MDMOperationException("112930", message, "OutboundQueueBL." + methodName, String.Empty, methodName);
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
                throw new MDMOperationException("111846", "CallerContext cannot be null.", "OutboundQueueBL.Process", String.Empty, "Process");
            }
        }

        private OperationResultCollection CreateOperationResultCollection(OutboundQueueItemCollection outboundQueueItemCollection)
        {
            OperationResultCollection operationResultCollection = new OperationResultCollection();
            if (outboundQueueItemCollection != null && outboundQueueItemCollection.Count > 0)
            {
                Int32 counter = -1;
                foreach (OutboundQueueItem item in outboundQueueItemCollection)
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
