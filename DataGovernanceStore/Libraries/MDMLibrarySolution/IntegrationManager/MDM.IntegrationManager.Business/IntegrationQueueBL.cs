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
    using MDM.Utility;

    /// <summary>
    /// Class to perform CURD operation on integration queue item
    /// </summary>
    public class IntegrationQueueBL : BusinessLogicBase
    {
        #region Fields

        private SecurityPrincipal _securityPrincipal = null;
        private LocaleEnum _systemDataLocale = GlobalizationHelper.GetSystemUILocale();
        private IntegrationQueueDA _qualifyingQueueDA = new IntegrationQueueDA();

        private const String _processMethodName = "MDM.IntegrationManager.Business.IntegrationQueueBL.Process";
        private const String _getByIdMethodName = "MDM.IntegrationManager.Business.IntegrationQueueBL.GetById";
        private const String _getNextBatchMethodName = "MDM.IntegrationManager.Business.IntegrationQueueBL.GetNextBatch";

        private const String _processQualifyingQueueMethodName = "MDM.IntegrationManager.Business.IntegrationQueueBL.ProcessQualifyingQueue";
        private const String _getQualifyingQueueMethodName = "MDM.IntegrationManager.Business.IntegrationQueueBL.GetQualifyingQueueItems";
        private const String _markQualifyingQueueItemAsLoadedMethodName = "MDM.IntegrationManager.Business.IntegrationQueueBL.MarkQualifyingQueueItemAsLoaded";

        private const String _getOutboundQueueMethodName = "MDM.IntegrationManager.Business.IntegrationQueueBL.GetOutboundQueueItems";
        private const String _markOutboundQueueItemAsLoadedMethodName = "MDM.IntegrationManager.Business.IntegrationQueueBL.MarkOutboundQueueItemAsLoaded";

        #endregion Fields

        #region Constructor

        /// <summary>
        /// Parameterless Constructor
        /// </summary>
        public IntegrationQueueBL()
        {
            GetSecurityPrincipal();
        }

        #endregion Constructor

        #region CUD Methods

        /// <summary>
        /// Create a integration queue item
        /// </summary>
        /// <param name="integrationQueueItem">Integration queue item to be created</param>
        /// <param name="callerContext">Indicates the context of caller making call to this API</param>
        /// <returns>Status indicating the result of operation</returns>
        public OperationResult Create(IntegrationQueueItem integrationQueueItem, CallerContext callerContext)
        {
            this.Validate(integrationQueueItem, "Create");
            integrationQueueItem.Action = Core.ObjectAction.Create;
            return this.Process(new IntegrationQueueItemCollection { integrationQueueItem }, callerContext).FirstOrDefault();
        }

        /// <summary>
        /// Update a integration queue item
        /// </summary>
        /// <param name="integrationQueueItem">Integration queue item to be updated</param>
        /// <param name="callerContext">Indicates the context of caller making call to this API</param>
        /// <returns>Status indicating the result of operation</returns>
        public OperationResult Update(IntegrationQueueItem integrationQueueItem, CallerContext callerContext)
        {
            this.Validate(integrationQueueItem, "Update");
            integrationQueueItem.Action = Core.ObjectAction.Update;
            return this.Process(new IntegrationQueueItemCollection { integrationQueueItem }, callerContext).FirstOrDefault();
        }

        /// <summary>
        /// Delete a integration queue item
        /// </summary>
        /// <param name="integrationQueueItem">Integration queue item to be deleted</param>
        /// <param name="callerContext">Indicates the context of caller making call to this API</param>
        /// <returns>Status indicating the result of operation</returns>
        public OperationResult Delete(IntegrationQueueItem integrationQueueItem, CallerContext callerContext)
        {
            this.Validate(integrationQueueItem, "Delete");
            integrationQueueItem.Action = Core.ObjectAction.Delete;
            return this.Process(new IntegrationQueueItemCollection { integrationQueueItem }, callerContext).FirstOrDefault();
        }

        /// <summary>
        /// Process (create / update / delete) integration queue items
        /// </summary>
        /// <param name="integrationQueueItemCollection">Integration queue items to be processed</param>
        /// <param name="callerContext">Indicates the context of caller making call to this API</param>
        /// <returns>Status indicating the result of operation</returns>
        public OperationResultCollection Process(IntegrationQueueItemCollection integrationQueueItemCollection, CallerContext callerContext)
        {
            OperationResultCollection operationResultCollection = new OperationResultCollection();

            if (Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.StartTraceActivity(_processMethodName, MDMTraceSource.Integration, false);
            }

            #region Parameter Validation

            ValidateCallerContext(callerContext);

            if (integrationQueueItemCollection == null && integrationQueueItemCollection.Count < 0)
            {
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, "IntegrationQueueMessageCollection cannot be null");
                //TODO - Ira :: Localize
                throw new MDMOperationException("XXX", "IntegrationQueueMessageCollection cannot be null", "IntegrationQueueBL.Process", String.Empty, "Process");
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

                //Get command
                DBCommandProperties command = DBCommandHelper.Get(callerContext, Core.MDMCenterModuleAction.Update);

                using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions() { IsolationLevel = Constants.DEFAULT_ISOLATION_LEVEL }))
                {
                    operationResultCollection = _qualifyingQueueDA.Process(integrationQueueItemCollection, callerContext.ProgramName, userName, command);

                    //LocalizeErrors(callerContext, operationResultCollection);
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

        #region Qualifying Queue

        /// <summary>
        /// Process (create / update / delete) integration queue items
        /// </summary>
        /// <param name="integrationQueueItemCollection">Integration queue items to be processed</param>
        /// <param name="callerContext">Indicates the context of caller making call to this API</param>
        /// <returns>Status indicating the result of operation</returns>
        public OperationResultCollection ProcessQualifyingQueueItems(IntegrationQueueItemCollection integrationQueueItemCollection, CallerContext callerContext)
        {
            OperationResultCollection operationResultCollection = new OperationResultCollection();

            if (Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.StartTraceActivity(_processQualifyingQueueMethodName, MDMTraceSource.Integration, false);
            }

            #region Parameter Validation

            ValidateCallerContext(callerContext);

            if (integrationQueueItemCollection == null && integrationQueueItemCollection.Count < 0)
            {
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, "IntegrationQueueMessageCollection cannot be null");
                //TODO - Ira :: Localize
                throw new MDMOperationException("XXX", "IntegrationQueueMessageCollection cannot be null", "IntegrationQueueBL.Process", String.Empty, "Process");
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

                //Get command
                DBCommandProperties command = DBCommandHelper.Get(callerContext, Core.MDMCenterModuleAction.Update);

                using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions() { IsolationLevel = Constants.DEFAULT_ISOLATION_LEVEL }))
                {
                    operationResultCollection = _qualifyingQueueDA.ProcessQualifyingQueueItems(integrationQueueItemCollection, callerContext.ProgramName, userName, command);
                    
                    //LocalizeErrors(callerContext, operationResultCollection);
                    transactionScope.Complete();
                }
            }
            finally
            {
                if (Constants.TRACING_ENABLED)
                {
                    MDMTraceHelper.StopTraceActivity(_processQualifyingQueueMethodName, MDMTraceSource.Integration);
                }
            }

            return operationResultCollection;
        }

        /// <summary>
        /// Get integration queue item for a given range of Ids
        /// </summary>
        /// <param name="logStatus">Integration queue item status based on which data is to be  fetched</param>
        /// <param name="fromCount">Start of range of values to be fetched</param>
        /// <param name="toCount">End of range of values to be fetched</param>
        /// <param name="callerContext">Indicates the context of caller making call to this API</param>
        /// <returns>Integration queue item object</returns>
        public IntegrationQueueItemCollection GetQualifyingQueue(String logStatus, Int64 fromCount, Int64 toCount, CallerContext callerContext)
        {
            IntegrationQueueItemCollection integrationQueueItems = null;
            if (Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.StartTraceActivity(_getQualifyingQueueMethodName, MDMTraceSource.Integration, false);
            }

            //Parameter Validation
            ValidateCallerContext(callerContext);

            DBCommandProperties command = DBCommandHelper.Get(callerContext, MDMCenterModuleAction.Read);

            if (fromCount > -1 && toCount > -1 && !String.IsNullOrWhiteSpace(logStatus))
            {
                integrationQueueItems = _qualifyingQueueDA.GetQualifyingQueueItems(0, 0, logStatus, fromCount, toCount, command);
            }

            if (Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.StopTraceActivity(_getQualifyingQueueMethodName, MDMTraceSource.Integration);
            }

            return integrationQueueItems;
        }

        /// <summary>
        /// Update QualifyingQueueItem table record to mark that it is loaded.
        /// </summary>
        /// <param name="qualifyingQueueItemId">Integration qualifying queue item Id to update for loaded status</param>
        /// <param name="callerContext">Indicates the context of caller making call to this API</param>
        /// <returns>Status indicating the result of operation</returns>
        public Boolean MarkQualifyingQueueItemAsLoaded(Int64 qualifyingQueueItemId, CallerContext callerContext)
        {
            Boolean result = true;
            if (Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.StartTraceActivity(_markQualifyingQueueItemAsLoadedMethodName, MDMTraceSource.Integration, false);
            }

            #region Parameter Validation

            ValidateCallerContext(callerContext);

            if (qualifyingQueueItemId < 0)
            {
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, "IntegrationQualifyingQueueItemId cannot be less than 0");
                //TODO - Ira :: Localize
                throw new MDMOperationException("XXX", "IntegrationQualifyingQueueItemId cannot be less than 0", "IntegrationQualifyingQueueItemBL.MarkAsLoaded", String.Empty, "MarkAsLoaded");
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
                    callerContext.ProgramName = _markQualifyingQueueItemAsLoadedMethodName;
                }

                //Get command
                DBCommandProperties command = DBCommandHelper.Get(callerContext, Core.MDMCenterModuleAction.Update);

                using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions() { IsolationLevel = Constants.DEFAULT_ISOLATION_LEVEL }))
                {
                    result = _qualifyingQueueDA.MarkQualifyingQueueItemAsQualified(qualifyingQueueItemId, callerContext.ProgramName, userName, command);

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
                    MDMTraceHelper.StopTraceActivity(_markQualifyingQueueItemAsLoadedMethodName, MDMTraceSource.Integration);
                }
            }

            return result;
        }

        #endregion Qualifying Queue

        #region Outbound Queue

        /// <summary>
        /// Get integration queue item for a given range of Ids
        /// </summary>
        /// <param name="logStatus">Integration queue item status based on which data is to be  fetched</param>
        /// <param name="fromCount">Start of range of values to be fetched</param>
        /// <param name="toCount">End of range of values to be fetched</param>
        /// <param name="callerContext">Indicates the context of caller making call to this API</param>
        /// <returns>Integration queue item object</returns>
        public IntegrationQueueItemCollection GetOutboundQueue(String logStatus, Int64 fromCount, Int64 toCount, CallerContext callerContext)
        {
            IntegrationQueueItemCollection integrationQueueItems = null;
            if (Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.StartTraceActivity(_getOutboundQueueMethodName, MDMTraceSource.Integration, false);
            }

            //Parameter Validation
            ValidateCallerContext(callerContext);

            DBCommandProperties command = DBCommandHelper.Get(callerContext, MDMCenterModuleAction.Read);

            if (fromCount > -1 && toCount > -1 && !String.IsNullOrWhiteSpace(logStatus))
            {
                integrationQueueItems = _qualifyingQueueDA.GetOutboundQueueItems(0, 0, logStatus, fromCount, toCount, command);
            }

            if (Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.StopTraceActivity(_getOutboundQueueMethodName, MDMTraceSource.Integration);
            }

            return integrationQueueItems;
        }

        /// <summary>
        /// Update OutboundQueueItem table record to mark that it is loaded.
        /// </summary>
        /// <param name="outboundQueueItemId">Integration outbound queue item Id to update for loaded status</param>
        /// <param name="callerContext">Indicates the context of caller making call to this API</param>
        /// <returns>Status indicating the result of operation</returns>
        public Boolean MarkOutboundQueueItemAsProcessed(Int64 outboundQueueItemId, CallerContext callerContext)
        {
            Boolean result = true;
            if (Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.StartTraceActivity(_markOutboundQueueItemAsLoadedMethodName, MDMTraceSource.Integration, false);
            }

            #region Parameter Validation

            ValidateCallerContext(callerContext);

            if (outboundQueueItemId < 0)
            {
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, "IntegrationOutboundQueueItemId cannot be less than 0");
                //TODO - Ira :: Localize
                throw new MDMOperationException("XXX", "IntegrationOutboundQueueItemId cannot be less than 0", "IntegrationOutboundQueueItemBL.MarkAsLoaded", String.Empty, "MarkAsLoaded");
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
                    callerContext.ProgramName = _markOutboundQueueItemAsLoadedMethodName;
                }

                //Get command
                DBCommandProperties command = DBCommandHelper.Get(callerContext, Core.MDMCenterModuleAction.Update);

                using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions() { IsolationLevel = Constants.DEFAULT_ISOLATION_LEVEL }))
                {
                    result = _qualifyingQueueDA.MarkOutboundQueueItemAsProcessed(outboundQueueItemId, callerContext.ProgramName, userName, command);

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
                    MDMTraceHelper.StopTraceActivity(_markOutboundQueueItemAsLoadedMethodName, MDMTraceSource.Integration);
                }
            }

            return result;
        }

        #endregion Outbound Queue
        
        #region Get methods

        /// <summary>
        /// Get integration queue item by id
        /// </summary>
        /// <param name="integrationQueueItemId">Integration queue item id to fetch the value for</param>
        /// <param name="callerContext">Indicates the context of caller making call to this API</param>
        /// <returns>Integration queue item object</returns>
        public IntegrationQueueItem GetById(Int64 integrationQueueItemId, CallerContext callerContext)
        {
            IntegrationQueueItem integrationQueueItem = null;
            if (Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.StartTraceActivity(_getByIdMethodName, MDMTraceSource.Integration, false);
            }

            //Parameter Validation
            ValidateCallerContext(callerContext);

            DBCommandProperties command = DBCommandHelper.Get(callerContext, MDMCenterModuleAction.Read);

            if (integrationQueueItemId > 0)
            {
                integrationQueueItem = _qualifyingQueueDA.Get(integrationQueueItemId, 0, command).FirstOrDefault();
            }

            if (Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.StopTraceActivity(_getByIdMethodName, MDMTraceSource.Integration);
            }

            return integrationQueueItem;
        }

        /// <summary>
        /// Get integration queue item by id
        /// </summary>
        /// <param name="integrationQueueItemId">Integration queue item id to fetch the value for</param>
        /// <param name="callerContext">Indicates the context of caller making call to this API</param>
        /// <returns>Integration queue item object</returns>
        public IntegrationQueueItemCollection GetNextBatch(Int32 batchSize, CallerContext callerContext)
        {
            IntegrationQueueItemCollection integrationQueueItems = null;
            if (Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.StartTraceActivity(_getNextBatchMethodName, MDMTraceSource.Integration, false);
            }

            //Parameter Validation
            ValidateCallerContext(callerContext);

            DBCommandProperties command = DBCommandHelper.Get(callerContext, MDMCenterModuleAction.Read);

            if (batchSize > 0)
            {
                integrationQueueItems = _qualifyingQueueDA.Get(0, batchSize, command);
            }

            if (Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.StopTraceActivity(_getNextBatchMethodName, MDMTraceSource.Integration);
            }

            return integrationQueueItems;
        }

        #endregion Get methods

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
        private void Validate(IntegrationQueueItem integrationQueueItem, String methodName)
        {
            if (integrationQueueItem == null)
            {
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, "IntegrationQueueMessage cannot be null");
                //TODO - Ira :: Localize
                throw new MDMOperationException("XXX", "IntegrationQueueMessage cannot be null", "IntegrationQueueBL." + methodName, String.Empty, methodName);
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
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, "CallerContext cannot be null.");
                throw new MDMOperationException("111846", "CallerContext cannot be null.", "IntegrationQueueBL.Process", String.Empty, "Process");
            }
        }

        private OperationResult Process(IntegrationQueueItem integrationQueueItem, CallerContext callerContext)
        {
            throw new NotImplementedException();
        }


        #endregion Private Methods
    }
}
