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
    /// Class to perform CURD operation on qualifying queue item
    /// </summary>
    public class QualifyingQueueBL : BusinessLogicBase
    {
        #region Fields

        private SecurityPrincipal _securityPrincipal = null;
        ServerInfoBL serverInfoBL = new ServerInfoBL();
        private LocaleEnum _systemDataLocale = GlobalizationHelper.GetSystemUILocale();
        private QualifyingQueueDA _qualifyingQueueDA = new QualifyingQueueDA();
        private OutboundQueueBL _outboundQueueBL = new OutboundQueueBL();
        private IntegrationOutboundAggregationQueueBL _aggregationOutboundQueueBL = new IntegrationOutboundAggregationQueueBL();
        private ConnectorProfileBL _connectorProfileBL = new ConnectorProfileBL();

        private const String _processMethodName = "MDM.IntegrationManager.Business.QualifyingQueueBL.Process";
        private const String _getMethodName = "MDM.IntegrationManager.Business.QualifyingQueueBL.Get";
        private const String _updateQualificationStatusMethodName = "MDM.IntegrationManager.Business.QualifyingQueueBL.UpdateQualificationStatus";

        #endregion Fields

        #region Constructor

        /// <summary>
        /// Parameterless Constructor
        /// </summary>
        public QualifyingQueueBL()
        {
            GetSecurityPrincipal();
        }

        #endregion Constructor

        #region CUD Methods

        /// <summary>
        /// Create a qualifying queue item
        /// </summary>
        /// <param name="qualifyingQueueItem">Qualifying queue item to be created</param>
        /// <param name="callerContext">Indicates the context of caller making call to this API</param>
        /// <returns>Status indicating the result of operation</returns>
        public OperationResult Create(QualifyingQueueItem qualifyingQueueItem, CallerContext callerContext)
        {
            this.Validate(qualifyingQueueItem, "Create");
            qualifyingQueueItem.Action = Core.ObjectAction.Create;
            return this.Process(new QualifyingQueueItemCollection { qualifyingQueueItem }, callerContext).FirstOrDefault();
        }

        /// <summary>
        /// Update a qualifying queue item
        /// </summary>
        /// <param name="qualifyingQueueItem">Qualifying queue item to be updated</param>
        /// <param name="callerContext">Indicates the context of caller making call to this API</param>
        /// <returns>Status indicating the result of operation</returns>
        public OperationResult Update(QualifyingQueueItem qualifyingQueueItem, CallerContext callerContext)
        {
            this.Validate(qualifyingQueueItem, "Update");
            qualifyingQueueItem.Action = Core.ObjectAction.Update;
            return this.Process(new QualifyingQueueItemCollection { qualifyingQueueItem }, callerContext).FirstOrDefault();
        }

        /// <summary>
        /// Delete a qualifying queue item
        /// </summary>
        /// <param name="qualifyingQueueItem">Qualifying queue item to be deleted</param>
        /// <param name="callerContext">Indicates the context of caller making call to this API</param>
        /// <returns>Status indicating the result of operation</returns>
        public OperationResult Delete(QualifyingQueueItem qualifyingQueueItem, CallerContext callerContext)
        {
            this.Validate(qualifyingQueueItem, "Delete");
            qualifyingQueueItem.Action = Core.ObjectAction.Delete;
            return this.Process(new QualifyingQueueItemCollection { qualifyingQueueItem }, callerContext).FirstOrDefault();
        }

        /// <summary>
        /// Process (create / update / delete) qualifying queue items
        /// </summary>
        /// <param name="qualifyingQueueItemCollection">Qualifying queue items to be processed</param>
        /// <param name="callerContext">Indicates the context of caller making call to this API</param>
        /// <returns>Status indicating the result of operation</returns>
        public OperationResultCollection Process(QualifyingQueueItemCollection qualifyingQueueItemCollection, CallerContext callerContext)
        {
            OperationResultCollection operationResultCollection = null;

            if (Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.StartTraceActivity(_processMethodName, MDMTraceSource.Integration, false);
            }

            #region Parameter Validation

            ValidateCallerContext(callerContext);

            if (qualifyingQueueItemCollection == null && qualifyingQueueItemCollection.Count < 0)
            {
                String message = "QualifyingQueueItemCollection must not be NULL.";
                throw new MDMOperationException("112931", message, "QualifyingQueueBL.Process", String.Empty, "Process");
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
                operationResultCollection = CreateOperationResultCollection(qualifyingQueueItemCollection);

                using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions() { IsolationLevel = Constants.DEFAULT_ISOLATION_LEVEL }))
                {
                    _qualifyingQueueDA.Process(qualifyingQueueItemCollection, callerContext.ProgramName, userName, (Int16)callerContext.ServerId, operationResultCollection, command);

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
        /// Get qualifying queue item for a given range of Ids
        /// </summary>
        /// <param name="logStatus">Qualifying queue item status based on which data is to be  fetched</param>
        /// <param name="fromCount">Start of range of values to be fetched</param>
        /// <param name="toCount">End of range of values to be fetched</param>
        /// <param name="callerContext">Indicates the context of caller making call to this API</param>
        /// <returns>Qualifying queue item object</returns>
        public QualifyingQueueItemCollection Get(String logStatus, Int64 fromCount, Int64 toCount, CallerContext callerContext)
        {
            QualifyingQueueItemCollection qualifyingQueueItems = null;
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
                    qualifyingQueueItems = _qualifyingQueueDA.Get(logStatus, fromCount, toCount, callerContext.ProgramName, userName, (Int16)callerContext.ServerId, command);
                    transactionScope.Complete();
                }
            }

            if (Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.StopTraceActivity(_getMethodName, MDMTraceSource.Integration);
            }

            return qualifyingQueueItems;
        }

        /// <summary>
        /// Update QualifyingQueueItem table record to mark the qualification status.
        /// If item is qualified, it will also insert record into Outbound queue.
        /// </summary>
        /// <param name="qualifyingQueueItem">Qualifying queue item to update for qualification status</param>
        /// <param name="scheduledAggregationTime">The scheduled aggregation time.</param>
        /// <param name="callerContext">Indicates the context of caller making call to this API</param>
        /// <returns>
        /// Status indicating the result of operation
        /// </returns>
        public Boolean UpdateQualificationStatus(QualifyingQueueItem qualifyingQueueItem, DateTime? scheduledAggregationTime, CallerContext callerContext)
        {
            Boolean result = true;
            if (Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.StartTraceActivity(_updateQualificationStatusMethodName, MDMTraceSource.Integration, false);
            }

            #region Parameter Validation

            ValidateCallerContext(callerContext);

            Validate(qualifyingQueueItem, _updateQualificationStatusMethodName);

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
                    callerContext.ProgramName = _updateQualificationStatusMethodName;
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
                    result = _qualifyingQueueDA.UpdateQualificationStatus(qualifyingQueueItem.Id, qualifyingQueueItem.MessageQualificationStatus, qualifyingQueueItem.ScheduledQualifierTime, qualifyingQueueItem.Comments, callerContext.ProgramName, userName, (Int16)callerContext.ServerId, command);

                    if (result == true)
                    {
                        if (qualifyingQueueItem.MessageQualificationStatus == MessageQualificationStatusEnum.Qualified)
                        {
                            OperationResult outBoundProcessOR = PutMessageInNextQueue(qualifyingQueueItem, scheduledAggregationTime, callerContext);
                            if (outBoundProcessOR != null &&
                                ( outBoundProcessOR.OperationResultStatus == OperationResultStatusEnum.Successful || outBoundProcessOR.OperationResultStatus == OperationResultStatusEnum.None ))
                            {
                                result = true;
                            }
                            else
                            {
                                result = false;
                            }
                        }

                        if (result == true)
                        {
                            transactionScope.Complete();
                        }
                    }
                }
            }
            finally
            {
                if (Constants.TRACING_ENABLED)
                {
                    MDMTraceHelper.StopTraceActivity(_updateQualificationStatusMethodName, MDMTraceSource.Integration);
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
        private void Validate(QualifyingQueueItem qualifyingQueueItem, String methodName)
        {
            if (qualifyingQueueItem == null)
            {
                String message = "QualifyingQueueItem must not be NULL.";
                throw new MDMOperationException("112932", message, "QualifyingQueueBL." + methodName, String.Empty, methodName);
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
                throw new MDMOperationException("111846", "CallerContext cannot be null.", "QualifyingQueueBL.Process", String.Empty, "Process");
            }
        }

        private OperationResultCollection CreateOperationResultCollection(QualifyingQueueItemCollection qualifyingQueueItemCollection)
        {
            OperationResultCollection operationResultCollection = new OperationResultCollection();
            if (qualifyingQueueItemCollection != null && qualifyingQueueItemCollection.Count > 0)
            {
                Int32 counter = -1;
                foreach (QualifyingQueueItem item in qualifyingQueueItemCollection)
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

        private OperationResult LoadOutboundQueue(QualifyingQueueItem qualifyingQueueItem, CallerContext callerContext)
        {
            OutboundQueueItem item = new OutboundQueueItem();
            item.Action = ObjectAction.Create;
            item.ConnectorId = qualifyingQueueItem.ConnectorId;
            item.IntegrationActivityLogId = qualifyingQueueItem.IntegrationActivityLogId;
            item.IntegrationMessageId = qualifyingQueueItem.IntegrationMessageId;
            item.IntegrationMessageTypeId = qualifyingQueueItem.IntegrationMessageTypeId;
            item.IntegrationType = qualifyingQueueItem.IntegrationType;
            item.QualifyingQueueItemId = qualifyingQueueItem.Id;
            item.Weightage = qualifyingQueueItem.Weightage;

            item.ScheduledProcessTime = GetScheduledTime(qualifyingQueueItem.ConnectorId, qualifyingQueueItem.Id, callerContext);

            OperationResult result = _outboundQueueBL.Create(item, callerContext);

            return result;
        }

        private OperationResult PutMessageInNextQueue(QualifyingQueueItem qualifyingQueueItem, DateTime? scheduledAggregationTime, CallerContext callerContext)
        {
            if (IsAggregationEnabled(qualifyingQueueItem.ConnectorId, qualifyingQueueItem.IntegrationType, callerContext))
            {
                return LoadAggregationQueue(qualifyingQueueItem, scheduledAggregationTime,  callerContext);
            }
            else
            {
                return LoadOutboundQueue(qualifyingQueueItem, callerContext);
            }
        }

        private OperationResult LoadAggregationQueue(QualifyingQueueItem qualifyingQueueItem, DateTime? scheduledAggregationTime, CallerContext callerContext)
        {
            AggregationOutboundQueueItem item = new AggregationOutboundQueueItem();
            item.Action = ObjectAction.Create;
            item.ConnectorId = qualifyingQueueItem.ConnectorId;
            item.IntegrationActivityLogId = qualifyingQueueItem.IntegrationActivityLogId;
            item.IntegrationMessageId = qualifyingQueueItem.IntegrationMessageId;
            item.IntegrationMessageTypeId = qualifyingQueueItem.IntegrationMessageTypeId;
            item.IntegrationType = qualifyingQueueItem.IntegrationType;
            item.QualifyingQueueItemId = qualifyingQueueItem.Id;
            item.Weightage = qualifyingQueueItem.Weightage;

            item.ScheduledProcessTime = GetAggregationScheduledTime(qualifyingQueueItem.ConnectorId, qualifyingQueueItem.Id, qualifyingQueueItem.IntegrationType, scheduledAggregationTime, callerContext);

            OperationResult result = _aggregationOutboundQueueBL.Create(item, callerContext);

            return result;
        }

        private Boolean IsAggregationEnabled(Int16 connectorId, IntegrationType integrationType, CallerContext callerContext)
        {
            Boolean isAggregationEnabled = false;
            if (connectorId > 0)
            {
                ConnectorProfile profile = _connectorProfileBL.GetById(connectorId, callerContext);
                if (profile != null && profile.AggregationConfiguration != null)
                {
                    switch (integrationType)
                    {
                        case IntegrationType.Outbound:
                            isAggregationEnabled = profile.AggregationConfiguration.IsEnabledForOutbound;
                            break;
                        case IntegrationType.Inbound:
                            isAggregationEnabled = profile.AggregationConfiguration.IsEnabledForInbound;
                            break;
                    }
                }
                else
                {
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, "Cannot find connector profile with Id = " + connectorId);
                }
            }
            else
            {
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, "ConnectorId should be greater than zero", MDMTraceSource.Integration);
            }

            return isAggregationEnabled;
        }

        private DateTime? GetScheduledTime(Int16 connectorId, Int64 qualifyingQueueItemId, CallerContext callerContext)
        {
            DateTime? result = null;
            if (connectorId > 0)
            {
                ConnectorProfile profile = _connectorProfileBL.GetById(connectorId, callerContext);
                if (profile != null)
                {
                    result = profile.ProcessingConfiguration.ScheduleCriteria.GetNextOccurrence();
                }
                else
                {
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, "Cannot find connector profile with Id = " + connectorId);
                }
            }
            else
            {
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, "ConnectorId is not available for QualifyingQueueItemId = " + qualifyingQueueItemId, MDMTraceSource.Integration);
            }

            if (result == null)
            {
                result = DateTime.Now;
            }
            return result;
        }

        private DateTime? GetAggregationScheduledTime(Int16 connectorId, Int64 qualifyingQueueItemId, IntegrationType integrationType, DateTime? scheduledAggregationTime, CallerContext callerContext)
        {
            DateTime? result = null;
            if (scheduledAggregationTime != null && scheduledAggregationTime.HasValue)
            {
                result = scheduledAggregationTime;
            }
            else
            {
                if (connectorId > 0)
                {
                    ConnectorProfile profile = _connectorProfileBL.GetById(connectorId, callerContext);
                    if (profile != null && profile.AggregationConfiguration != null)
                    {
                        switch (integrationType)
                        {
                            case IntegrationType.Outbound:
                                result = profile.AggregationConfiguration.GetOutboundScheduleCriteria().GetNextOccurrence();
                                break;
                            case IntegrationType.Inbound:
                                result = profile.AggregationConfiguration.GetInboundScheduleCriteria().GetNextOccurrence();
                                break;
                        }

                    }
                    else
                    {
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, "Cannot find connector profile with Id = " + connectorId);
                    }
                }
                else
                {
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, "ConnectorId is not available for QualifyingQueueItemId = " + qualifyingQueueItemId, MDMTraceSource.Integration);
                }
            }

            if (result == null)
            {
                result = DateTime.Now;
            }
            return result;
        }

        #endregion Private Methods
    }
}
