using System;
using System.IO;
using System.Xml;
using System.Linq;
using System.Transactions;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace MDM.IntegrationManager.Business
{
    using MDM.BusinessObjects;
    using MDM.BusinessObjects.Integration;
    using MDM.ConfigurationManager.Business;
    using MDM.MonitoringManager.Business;
    using MDM.Core;
    using MDM.Core.Exceptions;
    using MDM.IntegrationManager.Data;
    using MDM.Utility;

    /// <summary>
    /// Class to perform CRUD operation on integration activity log
    /// </summary>
    public class IntegrationActivityLogBL : BusinessLogicBase
    {
        #region Fields

        private SecurityPrincipal _securityPrincipal = null;
        private MDMObjectTypeBL _mdmObjectTypeBL = new MDMObjectTypeBL();
        private ServerInfoBL serverInfoBL = new ServerInfoBL();
        private const String _processMethodName = "MDM.IntegrationManager.Business.IntegrationActivityLogBL.Process";
        private const String _fillActivityLogMethodName = "MDM.IntegrationManager.Business.IntegrationActivityLogBL.FillIntegrationActivityLog";
        private const String _getMethodName = "MDM.IntegrationManager.Business.IntegrationActivityLogBL.Get";
        private const String _markAsLoadedMethodName = "MDM.IntegrationManager.Business.IntegrationActivityLogBL.MarkAsLoaded";

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Parameterless Constructor
        /// </summary>
        public IntegrationActivityLogBL()
        {
            GetSecurityPrincipal();
        }

        #endregion Constructors

        #region Methods

        #region CUD Methods

        /// <summary>
        /// Create IntegrationActivityLog
        /// </summary>
        /// <param name="mdmObjectId">MDMObjectId which for which activity log is to be created.</param>
        /// <param name="mdmObjectTypeName">Name of MDMObject.</param>
        /// <param name="context">Context for passed MDMObjectId</param>
        /// <param name="connectorShortName">ConnectorShortName for which ActivityLog is to be created</param>
        /// <param name="integrationMessageTypeShortName">ShortName of MessageType for which ActivityLog is to be created</param>
        /// <param name="integrationType">Type of integration (inbound/outbound)</param>
        /// <param name="weightage">Weightage for activity</param>
        /// <param name="messsageCount">Count of messages generated from given activity</param>
        /// <param name="callerContext">Context of caller making call to this API.</param>
        /// <returns>Status indicating the result of operation</returns>
        public OperationResult Create(Int64 mdmObjectId, String mdmObjectTypeName, String context, String connectorShortName, String integrationMessageTypeShortName,
            IntegrationType integrationType, Int32 weightage, Int32 messsageCount, CallerContext callerContext)
        {
            IntegrationActivityLog integrationActivityLog = FillIntegrationActivityLog(mdmObjectId, mdmObjectTypeName, context, connectorShortName, integrationMessageTypeShortName,
                    integrationType, weightage, messsageCount, callerContext);

            return this.Create(integrationActivityLog, callerContext);
        }

        /// <summary>
        /// Create a integration activity log
        /// </summary>
        /// <param name="integrationActivityLog">Integration activity log to be created</param>
        /// <param name="callerContext">Indicates the context of caller making call to this API</param>
        /// <returns>Status indicating the result of operation</returns>
        public OperationResult Create(IntegrationActivityLog integrationActivityLog, CallerContext callerContext)
        {
            this.Validate(integrationActivityLog, "Create");
            integrationActivityLog.Action = Core.ObjectAction.Create;
            return this.Process(new IntegrationActivityLogCollection { integrationActivityLog }, callerContext).FirstOrDefault();
        }

        /// <summary>
        /// Creates Integration Activity Logs for entities
        /// </summary>
        /// <param name="entities">Entities for which integration activity logs needs to be created</param>
        /// <param name="connectorName">ConnectorName for which ActivityLog has to be created</param>
        /// <param param name="integrationMessageTypeName">Name of MessageType for which ActivityLog has to be created</param>
        /// <param name="integrationType">Type of integration (inbound/outbound)</param>
        /// <param name="callerContext">Indicates the context of caller making call to this method</param>
        /// <returns>Status indicating the result of operation</returns>
        public OperationResultCollection Create(EntityCollection entities, String connectorName, String integrationMessageTypeName, IntegrationType integrationType, CallerContext callerContext)
        {
            OperationResultCollection operationResultCollection = new OperationResultCollection();

            if (Constants.PERFORMANCE_TRACING_ENABLED || Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.StartTraceActivity(_processMethodName, MDMTraceSource.Integration, false);
            }

            try
            {
                #region Parameter Validations

                if (entities == null || entities.Count < 1)
                {
                    String message = "Entities are not available.";
                    throw new MDMOperationException("111798", message, "IntegrationActivityLogBL.Process", String.Empty, "Process");
                }

                #endregion

                #region Initial Setup

                var integrationActivityLogs = new IntegrationActivityLogCollection();

                //Get connector profile
                ConnectorProfile connectorProfile = GetConnectorProfile(connectorName, callerContext);

                //Get integration message type 
                IntegrationMessageType integrationMessageType = GetIntegrationMessageType(integrationMessageTypeName, callerContext);

                //Get MDM Object Type
                MDMObjectType mdmObjectType = GetMDMObjectType("Entity", callerContext);

                #endregion

                foreach (Entity entity in entities)
                {
                    //Log integration activity log only in case of entity create and update
                    if (entity.Action == ObjectAction.Create
                        || entity.Action == ObjectAction.Update)
                    {
                        EntityChangeContext entityChangeContext = (EntityChangeContext)entity.GetChangeContext();

                        //Check whether attributes got changed.. Translation is needed only for attributes.
                        //So log activity only if attributes got changed..
                        if (entityChangeContext.IsAttributesChanged || entityChangeContext.IsRelationshipAttributesChanged)
                        {
                            LocaleChangeContextCollection localChangeContexts = entityChangeContext.LocaleChangeContexts;

                            if (localChangeContexts != null && localChangeContexts.Count > 0)
                            {
                                foreach (LocaleChangeContext localChangeContext in localChangeContexts)
                                {
                                    //Construct context string
                                    String context = ConstructActivityLogContext(entity, localChangeContext.DataLocale, localChangeContext);

                                    //Create Integration Activity log..
                                    IntegrationActivityLog integrationActivityLog = new IntegrationActivityLog();

                                    integrationActivityLog.ConnectorId = connectorProfile.Id;
                                    integrationActivityLog.ConnectorName = connectorProfile.Name;
                                    integrationActivityLog.ConnectorLongName = connectorProfile.LongName;
                                    integrationActivityLog.IntegrationMessageTypeId = integrationMessageType.Id;
                                    integrationActivityLog.IntegrationMessageTypeName = integrationMessageType.Name;
                                    integrationActivityLog.IntegrationMessageTypeLongName = integrationMessageType.LongName;
                                    integrationActivityLog.MDMObjectTypeId = mdmObjectType.Id;
                                    integrationActivityLog.MDMObjectTypeName = mdmObjectType.Name;
                                    integrationActivityLog.MDMObjectId = entity.Id;
                                    integrationActivityLog.IntegrationType = integrationType;
                                    integrationActivityLog.Context = context;

                                    integrationActivityLogs.Add(integrationActivityLog);
                                }
                            }
                        }
                    }
                }

                if (integrationActivityLogs.Count() > 0)
                {
                    operationResultCollection = ProcessIntegrationActivityLogs(integrationActivityLogs, callerContext);
                }
            }
            finally
            {
                if (Constants.PERFORMANCE_TRACING_ENABLED || Constants.TRACING_ENABLED)
                {
                    MDMTraceHelper.StopTraceActivity(_processMethodName, MDMTraceSource.Integration);
                }
            }

            return operationResultCollection;
        }

        /// <summary>
        /// Creates Integration Activity Logs for lookups
        /// </summary>
        /// <param name="lookups">Lookups for which integration activity logs needs to be created</param>
        /// <param name="connectorName">ConnectorName for which ActivityLog has to be created</param>
        /// <param param name="integrationMessageTypeName">Name of MessageType for which ActivityLog has to be created</param>
        /// <param name="integrationType">Type of integration (inbound/outbound)</param>
        /// <param name="callerContext">Indicates the context of caller making call to this method</param>
        /// <returns>Status indicating the result of operation</returns>
        public OperationResultCollection Create(LookupCollection lookups, String connectorName, String integrationMessageTypeName, IntegrationType integrationType, CallerContext callerContext)
        {
            OperationResultCollection operationResultCollection = new OperationResultCollection();

            if (Constants.PERFORMANCE_TRACING_ENABLED || Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.StartTraceActivity(_processMethodName, MDMTraceSource.Integration, false);
            }

            try
            {
                #region Parameter Validations

                if (lookups == null || lookups.Count < 1)
                {
                    String message = "Lookups are not available.";
                    throw new MDMOperationException("111798", message, "IntegrationActivityLogBL.Process", String.Empty, "Process");
                }

                #endregion

                #region Initial Setup

                var integrationActivityLogs = new IntegrationActivityLogCollection();

                //Get connector profile
                ConnectorProfile connectorProfile = GetConnectorProfile(connectorName, callerContext);

                //Get integration message type 
                IntegrationMessageType integrationMessageType = GetIntegrationMessageType(integrationMessageTypeName, callerContext);

                //Get MDM Object Type
                MDMObjectType mdmObjectType = GetMDMObjectType("Lookup", callerContext);
                #endregion

                Collection<Int64> recordIds = null;
                foreach (var lookup in lookups)
                {

                    recordIds = new Collection<Int64>();
                    foreach (Row row in lookup.Rows)
                    {
                        if (row.Action == ObjectAction.Create || row.Action == ObjectAction.Update)
                        {
                            recordIds.Add(row.Id);
                        }
                    }

                    if (recordIds.Count > 0)
                    {
                        //Construct context string
                        String context = ConstructActivityLogContext(lookup, recordIds);
                        //Create Integration Activity log..
                        IntegrationActivityLog integrationActivityLog = new IntegrationActivityLog();
                        integrationActivityLog.ConnectorId = connectorProfile.Id;
                        integrationActivityLog.ConnectorName = connectorProfile.Name;
                        integrationActivityLog.ConnectorLongName = connectorProfile.LongName;
                        integrationActivityLog.IntegrationMessageTypeId = integrationMessageType.Id;
                        integrationActivityLog.IntegrationMessageTypeName = integrationMessageType.Name;
                        integrationActivityLog.IntegrationMessageTypeLongName = integrationMessageType.LongName;
                        integrationActivityLog.MDMObjectTypeId = mdmObjectType.Id;
                        integrationActivityLog.MDMObjectTypeName = mdmObjectType.Name;
                        integrationActivityLog.MDMObjectId = lookup.Id;
                        integrationActivityLog.IntegrationType = integrationType;
                        integrationActivityLog.Context = context;
                        integrationActivityLogs.Add(integrationActivityLog);
                    }

                }

                if (integrationActivityLogs.Count > 0)
                {
                    operationResultCollection = ProcessIntegrationActivityLogs(integrationActivityLogs, callerContext);
                }
            }
            finally
            {
                if (Constants.PERFORMANCE_TRACING_ENABLED || Constants.TRACING_ENABLED)
                {
                    MDMTraceHelper.StopTraceActivity(_processMethodName, MDMTraceSource.Integration);
                }
            }

            return operationResultCollection;
        }

        /// <summary>
        /// Process (create) integration activity logs
        /// </summary>
        /// <param name="integrationActivityLogCollection">Integration activity logs to be processed</param>
        /// <param name="callerContext">Indicates the context of caller making call to this API</param>
        /// <returns>Status indicating the result of operation</returns>
        public OperationResultCollection Process(IntegrationActivityLogCollection integrationActivityLogCollection, CallerContext callerContext)
        {
            OperationResultCollection operationResultCollection = new OperationResultCollection();

            if (Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.StartTraceActivity(_processMethodName, MDMTraceSource.Integration, false);
            }

            try
            {
                operationResultCollection = ProcessIntegrationActivityLogs(integrationActivityLogCollection, callerContext);
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

        /// <summary>
        /// Update IntegrationActivityLog table record to mark that it is loaded, and update count of messages created from given activity log
        /// </summary>
        /// <param name="integrationActivityLogId">Integration activity log Id update for loaded status</param>
        /// <param name="messageCount">No. of messages created from given ActivityLog</param>
        /// <param name="callerContext">Indicates the context of caller making call to this API</param>
        /// <returns>Status indicating the result of operation</returns>
        public Boolean MarkAsLoaded(Int64 integrationActivityLogId, Int32 messageCount, CallerContext callerContext)
        {
            Boolean result = true;
            if (Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.StartTraceActivity(_markAsLoadedMethodName, MDMTraceSource.Integration, false);
            }

            #region Parameter Validation

            ValidateCallerContext(callerContext);

            if (integrationActivityLogId < 0)
            {
                String message = "IntegrationActivityLogID must not be less than zero.";
                throw new MDMOperationException("112914", message, _markAsLoadedMethodName, String.Empty, _markAsLoadedMethodName);
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
                    callerContext.ProgramName = _markAsLoadedMethodName;
                }

                if (!String.IsNullOrWhiteSpace(callerContext.ServerName))
                {
                    callerContext.ServerId = serverInfoBL.GetServerId(callerContext.ServerName);
                }
                else
                {
                    callerContext.ServerId = serverInfoBL.GetServerId(Environment.MachineName);
                }


                DBCommandProperties command = DBCommandHelper.Get(callerContext, Core.MDMCenterModuleAction.Update);

                using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions() { IsolationLevel = Constants.DEFAULT_ISOLATION_LEVEL }))
                {
                    IntegrationActivityLogDA integrationActivityLogDA = new IntegrationActivityLogDA();
                    result = integrationActivityLogDA.MarkAsLoaded(integrationActivityLogId, messageCount, callerContext.ProgramName, userName, (Int16)callerContext.ServerId, command);

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
                    MDMTraceHelper.StopTraceActivity(_markAsLoadedMethodName, MDMTraceSource.Integration);
                }
            }

            return result;
        }

        /// <summary>
        /// Update IntegrationActivityLog table record to mark that it is Processed, and update count of messages created from given activity log
        /// </summary>
        /// <param name="integrationActivityLogId">Integration activity log Id update for loaded status</param>
        /// <param name="messageCount">No. of messages created from given ActivityLog</param>
        /// <param name="callerContext">Indicates the context of caller making call to this API</param>
        /// <returns>Status indicating the result of operation</returns>
        public Boolean MarkAsProcessed(Int64 integrationActivityLogId, Int32 messageCount, CallerContext callerContext)
        {
            Boolean result = true;
            if (Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.StartTraceActivity(_markAsLoadedMethodName, MDMTraceSource.Integration, false);
            }

            #region Parameter Validation

            ValidateCallerContext(callerContext);

            if (integrationActivityLogId < 0)
            {
                String message = "IntegrationActivityLogID must not be less than zero.";
                throw new MDMOperationException("112914", message, _markAsLoadedMethodName, String.Empty, _markAsLoadedMethodName);
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
                    callerContext.ProgramName = _markAsLoadedMethodName;
                }

                if (!String.IsNullOrWhiteSpace(callerContext.ServerName))
                {
                    callerContext.ServerId = serverInfoBL.GetServerId(callerContext.ServerName);
                }
                else
                {
                    callerContext.ServerId = serverInfoBL.GetServerId(Environment.MachineName);
                }


                DBCommandProperties command = DBCommandHelper.Get(callerContext, Core.MDMCenterModuleAction.Update);

                using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions() { IsolationLevel = Constants.DEFAULT_ISOLATION_LEVEL }))
                {
                    IntegrationActivityLogDA integrationActivityLogDA = new IntegrationActivityLogDA();
                    result = integrationActivityLogDA.MarkAsProcessed(integrationActivityLogId, messageCount, callerContext.ProgramName, userName, (Int16)callerContext.ServerId, command);

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
                    MDMTraceHelper.StopTraceActivity(_markAsLoadedMethodName, MDMTraceSource.Integration);
                }
            }

            return result;
        }

        #endregion CUD Methods

        #region Get Methods

        /// <summary>
        /// Get integration activity log for a given range of Ids
        /// </summary>
        /// <param name="logStatus">Integration activity log status based on which data is to be  fetched</param>
        /// <param name="fromCount">Start of range of values to be fetched</param>
        /// <param name="toCount">End of range of values to be fetched</param>
        /// <param name="callerContext">Indicates the context of caller making call to this API</param>
        /// <returns>Integration activity log object</returns>
        public IntegrationActivityLogCollection Get(String logStatus, Int64 fromCount, Int64 toCount, CallerContext callerContext)
        {
            return Get(logStatus, fromCount, toCount, callerContext, -1);
        }

        /// <summary>
        /// Get integration activity log for a given connector and range of Ids
        /// </summary>
        /// <param name="connectorId">Connector id for which integratoin items will be filtered against.</param>
        /// <param name="logStatus">Integration activity log status based on which data is to be  fetched</param>
        /// <param name="fromCount">Start of range of values to be fetched</param>
        /// <param name="toCount">End of range of values to be fetched</param>
        /// <param name="callerContext">Indicates the context of caller making call to this API</param>
        /// <returns>Integration activity log object</returns>        
        public IntegrationActivityLogCollection Get(Int32 connectorId, String logStatus, Int64 fromCount, Int64 toCount, CallerContext callerContext)
        {
            return Get(logStatus, fromCount, toCount, callerContext, connectorId);
        }

        public ConnectorProfile GetConnectorProfile(String connectorShortName, CallerContext callerContext)
        {
            String message = String.Empty;
            ConnectorProfile connectorProfile = null;

            if (String.IsNullOrWhiteSpace(connectorShortName))
            {
                message = "The ConnectorShortName is not provided. The IntegrationActivityLog cannot be created.";
                throw new MDMOperationException("112917", message, _processMethodName, String.Empty, _fillActivityLogMethodName);
            }
            ConnectorProfileBL connectorProfileManager = new ConnectorProfileBL();
            connectorProfile = connectorProfileManager.GetByName(connectorShortName, callerContext);

            if (connectorProfile == null)
            {
                message = String.Format("The Connector with ConnectorShortName = '{0}' is not found. The IntegrationActivityLog cannot be created", connectorShortName);
                throw new MDMOperationException("112916", message, _processMethodName, String.Empty, _fillActivityLogMethodName);
            }

            return connectorProfile;
        }

        #endregion Get Methods

        #region Private Methods

        private IntegrationActivityLogCollection Get(String logStatus, Int64 fromCount, Int64 toCount, CallerContext callerContext, Int32 connectorId = -1)
        {
            IntegrationActivityLogCollection integrationActivityLogs = null;
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
                    IntegrationActivityLogDA integrationActivityLogDA = new IntegrationActivityLogDA();

                    if (connectorId < 1)
                    {
                        integrationActivityLogs = integrationActivityLogDA.Get(logStatus, fromCount, toCount, callerContext.ProgramName, userName, (Int16)callerContext.ServerId, command);
                    }
                    else
                    {
                        integrationActivityLogs = integrationActivityLogDA.GetByConnectorId(connectorId, logStatus, fromCount, toCount, callerContext.ProgramName, userName, (Int16)callerContext.ServerId, command);
                    }

                    transactionScope.Complete();
                }
            }

            if (Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.StopTraceActivity(_getMethodName, MDMTraceSource.Integration);
            }

            return integrationActivityLogs;
        }

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

        private OperationResultCollection ProcessIntegrationActivityLogs(IntegrationActivityLogCollection integrationActivityLogs, CallerContext callerContext)
        {
            OperationResultCollection operationResultCollection = new OperationResultCollection();

            #region Parameter Validation

            ValidateCallerContext(callerContext);

            if (integrationActivityLogs == null && integrationActivityLogs.Count < 0)
            {
                String message = "IntegrationActivityLogs must not be NULL.";
                throw new MDMOperationException("112913", message, "IntegrationActivityLogBL.Process", String.Empty, "Process");
            }

            #endregion Parameter Validation

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
            operationResultCollection = CreateOperationResultCollection(integrationActivityLogs);

            using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions() { IsolationLevel = Constants.DEFAULT_ISOLATION_LEVEL }))
            {
                IntegrationActivityLogDA integrationActivityLogDA = new IntegrationActivityLogDA();
                integrationActivityLogDA.Process(integrationActivityLogs, callerContext.ProgramName, userName, (Int16)callerContext.ServerId, operationResultCollection, command);

                if (operationResultCollection.OperationResultStatus == OperationResultStatusEnum.Successful || operationResultCollection.OperationResultStatus == OperationResultStatusEnum.None)
                {
                    transactionScope.Complete();
                }
            }

            return operationResultCollection;
        }

        /// <summary>
        /// Validate input parameters
        /// </summary>
        private void Validate(IntegrationActivityLog integrationActivityLog, String methodName)
        {
            if (integrationActivityLog == null)
            {
                String message = "IntegrationActivityLog must not be NULL.";
                throw new MDMOperationException("112915", message, "IntegrationActivityLogBL." + methodName, String.Empty, methodName);
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
                throw new MDMOperationException("111846", "CallerContext cannot be null.", "IntegrationActivityLogBL.Process", String.Empty, "Process");
            }
        }

        private OperationResultCollection CreateOperationResultCollection(IntegrationActivityLogCollection integrationActivityLogCollection)
        {
            OperationResultCollection operationResultCollection = new OperationResultCollection();
            if (integrationActivityLogCollection != null && integrationActivityLogCollection.Count > 0)
            {
                Int32 counter = -1;
                foreach (IntegrationActivityLog log in integrationActivityLogCollection)
                {
                    if (String.IsNullOrEmpty(log.ReferenceId))
                    {
                        log.ReferenceId = (counter--).ToString();
                    }

                    OperationResult or = new OperationResult();
                    or.ReferenceId = log.ReferenceId;
                    or.OperationResultStatus = OperationResultStatusEnum.None;
                    operationResultCollection.Add(or);
                }
            }
            return operationResultCollection;
        }

        private IntegrationActivityLog FillIntegrationActivityLog(Int64 mdmObjectId, String mdmObjectTypeName, String context, String connectorShortName, String integrationMessageTypeShortName,
            IntegrationType integrationType, Int32 weightage, Int32 messageCount, CallerContext callerContext)
        {
            IntegrationActivityLog integrationActivityLog = new IntegrationActivityLog();
            String message = String.Empty;

            #region Fill Connector Details

            ConnectorProfile connectorProfile = GetConnectorProfile(connectorShortName, callerContext);

            integrationActivityLog.ConnectorId = connectorProfile.Id;
            integrationActivityLog.ConnectorName = connectorProfile.Name;
            integrationActivityLog.ConnectorLongName = connectorProfile.LongName;

            #endregion

            #region Fill IntegrationMessageType Details

            IntegrationMessageType integrationMessageType = GetIntegrationMessageType(integrationMessageTypeShortName, callerContext);

            integrationActivityLog.IntegrationMessageTypeId = integrationMessageType.Id;
            integrationActivityLog.IntegrationMessageTypeName = integrationMessageType.Name;
            integrationActivityLog.IntegrationMessageTypeLongName = integrationMessageType.LongName;

            #endregion

            #region Fill MDMObjectType Details

            MDMObjectType mdmObjectType = GetMDMObjectType(mdmObjectTypeName, callerContext);

            integrationActivityLog.MDMObjectTypeId = mdmObjectType.Id;
            integrationActivityLog.MDMObjectTypeName = mdmObjectType.Name;

            #endregion

            integrationActivityLog.MDMObjectId = mdmObjectId;
            integrationActivityLog.IntegrationType = integrationType;
            integrationActivityLog.Weightage = weightage;
            integrationActivityLog.MessageCount = messageCount;
            integrationActivityLog.Context = context;

            integrationActivityLog.IsLoaded = false;
            integrationActivityLog.IsLoadingInProgress = false;
            integrationActivityLog.IsProcessed = false;
            integrationActivityLog.MessageLoadEndTime = null;
            integrationActivityLog.MessageLoadStartTime = null;
            integrationActivityLog.ProcessEndTime = null;
            integrationActivityLog.ProcessStartTime = null;
            integrationActivityLog.ServerId = 1;

            return integrationActivityLog;
        }

        private IntegrationMessageType GetIntegrationMessageType(String integrationMessageTypeShortName, CallerContext callerContext)
        {
            String message = String.Empty;
            IntegrationMessageType integrationMessageType = null;

            if (String.IsNullOrWhiteSpace(integrationMessageTypeShortName))
            {
                message = "The IntegrationMessageTypeShortName is not provided. The IntegrationActivityLog cannot be created. If Filewatcher is used, the ConnectorProfile must have configured DefaultIntegrationMessage Type.";
                throw new MDMOperationException("112919", message, _processMethodName, String.Empty, _fillActivityLogMethodName);
            }

            IntegrationMessageTypeBL integrationMessageTypeManager = new IntegrationMessageTypeBL();
            integrationMessageType = integrationMessageTypeManager.GetByName(integrationMessageTypeShortName, callerContext);

            if (integrationMessageType == null)
            {
                message = String.Format("The IntegrationMessageType with IntegrationMessageTypeShortName = '{0}' cannot be found. The IntegrationActivityLog cannot be created. If Filewatcher is used, the ConnectorProfile must have configured DefaultIntegrationMessage Type.", integrationMessageTypeShortName);
                throw new MDMOperationException("112918", message, _processMethodName, String.Empty, _fillActivityLogMethodName);
            }

            return integrationMessageType;
        }

        private MDMObjectType GetMDMObjectType(String mdmObjectTypeShortName, CallerContext callerContext)
        {
            String message = String.Empty;
            MDMObjectType mdmObjectType = null;

            if (String.IsNullOrWhiteSpace(mdmObjectTypeShortName))
            {
                message = "The MDMObjectTypeName is not provided. The IntegrationActivityLog cannot be created.";
                throw new MDMOperationException("112921", message, _processMethodName, String.Empty, _fillActivityLogMethodName);
            }
            MDMObjectTypeBL mdmObjectTypeManager = new MDMObjectTypeBL();
            mdmObjectType = mdmObjectTypeManager.GetByName(mdmObjectTypeShortName, callerContext);

            if (mdmObjectType == null)
            {
                message = String.Format("The MDMObjectType with MDMObjectTypeName = '{0}' cannot be found. The IntegrationActivityLog cannot be created.", mdmObjectTypeShortName);
                throw new MDMOperationException("112920", message, _processMethodName, String.Empty, _fillActivityLogMethodName);
            }

            return mdmObjectType;
        }

        private String ConstructActivityLogContext(Entity entity, LocaleEnum sourceLocale, LocaleChangeContext localChangeContext)
        {
            String contextXml = String.Empty;

            using (StringWriter sw = new StringWriter())
            {
                using (XmlWriter xmlWriter = XmlWriter.Create(sw, new XmlWriterSettings { ConformanceLevel = ConformanceLevel.Auto }))
                {
                    String strSourceLocale = sourceLocale.ToString();
                    String sourceCulture = (strSourceLocale.IndexOf("WW", StringComparison.OrdinalIgnoreCase) >= 0) ? strSourceLocale.Replace("_", "-") : sourceLocale.GetCultureName();

                    Collection<Int32> attributeIdList = localChangeContext.AttributeChangeContexts.GetAttributeIdList();
                    Collection<Int32> relationshipTypeIdList = localChangeContext.RelationshipChangeContexts.GetRelationshipTypeIdList();
                    Collection<Int64> relationshipIdList = localChangeContext.RelationshipChangeContexts.GetRelationshipIdList();
                    Collection<Int32> relationshipAttributeIdList = localChangeContext.RelationshipChangeContexts.GetRelationshipAttributeIdList();

                    String strAttributeIdList = (attributeIdList == null) ? String.Empty : ValueTypeHelper.JoinCollection(attributeIdList, ",");
                    String strRelationshipTypeIdList = (relationshipTypeIdList == null) ? String.Empty : ValueTypeHelper.JoinCollection(relationshipTypeIdList, ",");
                    String strRelationshipIdList = (relationshipIdList == null) ? String.Empty : ValueTypeHelper.JoinCollection(relationshipIdList, ",");
                    String strRelationshipAttributeIdList = (relationshipAttributeIdList == null) ? String.Empty : ValueTypeHelper.JoinCollection(relationshipAttributeIdList, ",");

                    xmlWriter.WriteStartElement("Context");

                    xmlWriter.WriteStartElement("IntegrationProcessingSpecifications");

                    xmlWriter.WriteStartElement("TranslationSpecifications");
                    xmlWriter.WriteAttributeString("TranslationReferenceId", String.Empty);
                    xmlWriter.WriteAttributeString("TriggerMode", "EntityUpdate");
                    xmlWriter.WriteAttributeString("SourceLanguage", sourceCulture);
                    xmlWriter.WriteAttributeString("TranslationStepIdentifier", String.Empty);
                    xmlWriter.WriteAttributeString("SourceLanguagesHistory", String.Empty);
                    xmlWriter.WriteAttributeString("TargetLanguagesHistory", String.Empty);
                    xmlWriter.WriteEndElement();    //TranslationSpecifications element end

                    xmlWriter.WriteStartElement("EntityChangeContext");
                    xmlWriter.WriteAttributeString("EntityId", entity.Id.ToString());
                    xmlWriter.WriteAttributeString("CatalogId", entity.ContainerId.ToString());
                    xmlWriter.WriteAttributeString("CategoryId", entity.CategoryId.ToString());
                    xmlWriter.WriteAttributeString("EntityTypeId", entity.EntityTypeId.ToString());
                    xmlWriter.WriteAttributeString("OrgId", entity.OrganizationId.ToString());
                    xmlWriter.WriteAttributeString("Locale", sourceCulture);

                    xmlWriter.WriteAttributeString("AttributeIdList", strAttributeIdList);
                    xmlWriter.WriteAttributeString("RelationshipTypeIdList", strRelationshipTypeIdList);
                    xmlWriter.WriteAttributeString("RelationshipIdList", strRelationshipIdList);
                    xmlWriter.WriteAttributeString("RelationshipAttributeIdList", strRelationshipAttributeIdList);

                    xmlWriter.WriteEndElement();    //EntityChangeContext element end

                    xmlWriter.WriteEndElement();    //IntegrationProcessingSpecifications element end

                    xmlWriter.WriteEndElement();    //Context element end

                    xmlWriter.Flush();

                    //Get the actual XML
                    contextXml = sw.ToString();
                }
            }

            return contextXml;
        }

        private String ConstructActivityLogContext(Lookup lookup, Collection<Int64> recordIds)
        {
            String contextXml = String.Empty;

            using (StringWriter sw = new StringWriter())
            {
                using (XmlWriter xmlWriter = XmlWriter.Create(sw, new XmlWriterSettings { ConformanceLevel = ConformanceLevel.Auto }))
                {
                    String strSourceLocale = lookup.Locale.ToString();
                    String sourceCulture = (strSourceLocale.Contains("WW")) ? strSourceLocale.Replace("_", "-") : lookup.Locale.GetCultureName();

                    xmlWriter.WriteStartElement("Context");

                    xmlWriter.WriteStartElement("IntegrationProcessingSpecifications");

                    xmlWriter.WriteStartElement("TranslationSpecifications");
                    xmlWriter.WriteAttributeString("TranslationReferenceId", String.Empty);
                    xmlWriter.WriteAttributeString("TriggerMode", "LookupUpdate");
                    xmlWriter.WriteAttributeString("SourceLanguage", sourceCulture);
                    xmlWriter.WriteAttributeString("TranslationStepIdentifier", String.Empty);
                    xmlWriter.WriteAttributeString("SourceLanguagesHistory", String.Empty);
                    xmlWriter.WriteAttributeString("TargetLanguagesHistory", String.Empty);
                    xmlWriter.WriteEndElement();    //TranslationSpecifications element end

                    xmlWriter.WriteStartElement("LookupChangeContext");
                    xmlWriter.WriteAttributeString("LookupId", lookup.Id.ToString());
                    xmlWriter.WriteAttributeString("LookupName", lookup.Name);
                    xmlWriter.WriteAttributeString("LookupRecordIdList", ValueTypeHelper.JoinCollection<Int64>(recordIds, ","));
                    xmlWriter.WriteAttributeString("Locale", sourceCulture);

                    xmlWriter.WriteEndElement();    //LookupChangeContext element end

                    xmlWriter.WriteEndElement();    //IntegrationProcessingSpecifications element end

                    xmlWriter.WriteEndElement();    //Context element end

                    xmlWriter.Flush();

                    //Get the actual XML
                    contextXml = sw.ToString();
                }
            }

            return contextXml;
        }

        #endregion Private Methods

        #endregion Methods
    }
}