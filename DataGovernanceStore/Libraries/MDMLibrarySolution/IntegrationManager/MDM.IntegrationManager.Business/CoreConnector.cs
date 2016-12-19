using System;
using System.Diagnostics;
using System.Reflection;
using IO = System.IO;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace MDM.IntegrationManager.Business
{
    using MDM.BusinessObjects;
    using MDM.BusinessObjects.Integration;
    using MDM.Core;
    using MDM.Integration.Interfaces;
    using MDM.Interfaces;
    using MDM.Utility;
    using MDM.MessageManager.Business;
    using MDM.Integration.Events;

    /// <summary>
    /// class for Core Connector.
    /// </summary>
    public class CoreConnector : BusinessLogicBase, IConnector
    {
        #region Fields

        ConnectorProfileBL _connectorProfileBL = new ConnectorProfileBL();
        private String _registerMethodsName = "MDM.IntegrationManager.Business.CoreConnector.Register";
        private String _getOutboundMessagesMethodName = "MDM.IntegrationManager.Business.CoreConnector.GetOutboundMessages";
        private String _getOutboundAggregatedMessagesMethodName = "MDM.IntegrationManager.Business.CoreConnector.GetOutboundAggregatedMessages";
        private String _getInboundAggregatedMessagesMethodName = "MDM.IntegrationManager.Business.CoreConnector.GetInboundAggregatedMessages";
        private String _getInboundMessagesMethodName = "MDM.IntegrationManager.Business.CoreConnector.GetInboundMessages";
        private String _isQualifiedMethodName = "MDM.IntegrationManager.Business.CoreConnector.IsQualified";
        private String _processOutboundMessageMethodName = "MDM.IntegrationManager.Business.CoreConnector.ProcessOutboundMessage";
        private String _processInboundMessageMethodName = "MDM.IntegrationManager.Business.CoreConnector.ProcessInboundMessage";
        private String _transformMethodName = "MDM.IntegrationManager.Business.CoreConnector.Transform";
        private String _sendMethodName = "MDM.IntegrationManager.Business.CoreConnector.Send";
        private String _receiveMethodName = "MDM.IntegrationManager.Business.CoreConnector.Receive";
        private String _orchestrateOutboundMessageMethodName = "MDM.IntegrationManager.Business.CoreConnector.OrchestrateOutboundMessage";
        private String _orchestrateInboundMessageMethodName = "MDM.IntegrationManager.Business.CoreConnector.OrchestrateInboundMessage";

        /// <summary>
        /// Specifies integration status for an item.
        /// </summary>
        String status = String.Empty;

        /// <summary>
        /// Specifies integration comments.
        /// </summary>
        String comments = String.Empty;

        /// <summary>
        /// Specifies operation results
        /// </summary>
        OperationResultCollection operationResults = null;

        /// <summary>
        /// Specifies integration message type long name.
        /// </summary>
        String integrationMessageTypeLongName = String.Empty;

        /// <summary>
        /// Field denoting caller context
        /// </summary>
        private CallerContext _callerContext;

        /// <summary>
        /// Field denoting localeMessageBL
        /// </summary>
        private readonly LocaleMessageBL _localeMessageBL;

        /// <summary>
        /// Field denoting system ui locale
        /// </summary>
        private readonly LocaleEnum _systemUILocale;

        /// <summary>
        /// Field denoting duration helper for performance.
        /// </summary>
        private DurationHelper _durationHelper = null;

        /// <summary>
        /// Field denoting overall duration helper for performance.
        /// </summary>
        private DurationHelper _overallDurationHelper = null;

        /// <summary>
        /// Field denoting mdm object id.
        /// </summary>
        private Int64 _mdmObjectId = 0;

        #endregion Fields

        #region Properties
        #endregion

        #region Constructors

        /// <summary>
        /// Default Constructor
        /// </summary>
        public CoreConnector()
        {
            _localeMessageBL = new LocaleMessageBL();
            _systemUILocale = GlobalizationHelper.GetSystemUILocale();
        }

        #endregion

        #region Methods

        #region IConnector Methods

        /// <summary>
        /// Initial setup for connector. This will be called only once when user is clicking on UI button for registering the connector.
        /// </summary>
        /// <param name="iConnectorProfile">Profile of connector to register</param>
        /// <param name="iCallerContext">Context of caller making call to this API.</param>
        /// <returns>Result of operation</returns>
        public Boolean Register(IConnectorProfile iConnectorProfile, ICallerContext iCallerContext)
        {
            _overallDurationHelper = new DurationHelper(DateTime.Now);
            Boolean result = false;

            if (Constants.PERFORMANCE_TRACING_ENABLED || Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.StartTraceActivity(_registerMethodsName, MDMTraceSource.Integration, false);
            }

            try
            {
                IConnector connectorInstance = GetIConnectorInstance(iConnectorProfile);

                if (connectorInstance != null)
                {
                    _durationHelper = new DurationHelper(DateTime.Now);

                    result = connectorInstance.Register(iConnectorProfile, iCallerContext);

                    if (Constants.PERFORMANCE_TRACING_ENABLED || Constants.TRACING_ENABLED)
                    {
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("{0}ms - taken to register by {1} connector implementation.", _durationHelper.GetDurationInMilliseconds(DateTime.Now), iConnectorProfile.Name), MDMTraceSource.Integration);
                    }
                }
            }
            finally
            {
                if (Constants.PERFORMANCE_TRACING_ENABLED || Constants.TRACING_ENABLED)
                {
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("{0}ms - Overall time to register for {1} connector.", _overallDurationHelper.GetDurationInMilliseconds(DateTime.Now), iConnectorProfile.Name), MDMTraceSource.Integration);
                    MDMTraceHelper.StopTraceActivity(_registerMethodsName, MDMTraceSource.Integration);
                }
            }

            return result;
        }

        /// <summary>
        /// Create messages to be processed for outgoing (MDM to External system) messages.
        /// Take the integration activity, and create respective messages to be processed for given activity.
        /// This method is called after an integration activity is recorded.
        /// </summary>
        /// <param name="iIntegrationActivity">Integration activity which was performed.</param>
        /// <param name="iConnectorProfile">Profile of connector for which messages are to be created.</param>
        /// <param name="iCallerContext">Context of caller making call to this API.</param>
        /// <returns>IntegrationMessages to be processed due to given integration activity</returns>
        public IIntegrationMessageCollection GetOutboundMessages(IIntegrationActivity iIntegrationActivity, IConnectorProfile iConnectorProfile, ICallerContext iCallerContext)
        {
            _overallDurationHelper = new DurationHelper(DateTime.Now);

            if (Constants.PERFORMANCE_TRACING_ENABLED || Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.StartTraceActivity(_getOutboundMessagesMethodName, MDMTraceSource.Integration, false);
            }

            #region Initial Setup

            IIntegrationMessageCollection messages = null;
            IConnector connectorInstance = GetIConnectorInstance(iConnectorProfile);
            integrationMessageTypeLongName = iIntegrationActivity.IntegrationMessageTypeLongName;
            _callerContext = (CallerContext)iCallerContext;
            _mdmObjectId = iIntegrationActivity.MDMObjectId;

            #endregion

            try
            {
                if (connectorInstance != null)
                {
                    status = GetLocalizedMessage("113179", new Object[] { integrationMessageTypeLongName }); // {0} – Generating outbound messages has started.
                    comments = String.Empty;

                    operationResults = UpdateIntegrationItemStatus(_mdmObjectId, iIntegrationActivity.MDMObjectTypeId, iConnectorProfile, status, comments, OperationResultType.Information, iCallerContext);

                    _durationHelper = new DurationHelper(DateTime.Now);

                    var eventArgs = new IntegrationEventArgs(iIntegrationActivity, iConnectorProfile, iCallerContext);
                    IntegrationEventManager.Instance.OnIntegrationMessageOutboundMessagesLoading(eventArgs);

                    messages = connectorInstance.GetOutboundMessages(iIntegrationActivity, iConnectorProfile, iCallerContext);

                    if (Constants.PERFORMANCE_TRACING_ENABLED || Constants.TRACING_ENABLED)
                    {
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("{0}ms - taken to generate outbound messages by {1} connector implementation for MDMObjectId : {2}.", _durationHelper.GetDurationInMilliseconds(DateTime.Now), iConnectorProfile.Name, _mdmObjectId), MDMTraceSource.Integration);
                    }

                    var eventArgsCompleted = new IntegrationEventArgs(messages, iIntegrationActivity, iConnectorProfile, iCallerContext);
                    IntegrationEventManager.Instance.OnIntegrationMessageOutboundMessagesLoaded(eventArgsCompleted);

                    status = GetLocalizedMessage("113180", new Object[] { integrationMessageTypeLongName }); // {0} - Generating outbound messages has completed.
                    comments = GetLocalizedMessage("113177", new Object[] { (messages != null && messages.Count > 0) ? messages.Count : 0 }); // Total {0} outbound message(s) generated.

                    operationResults = UpdateIntegrationItemStatus(_mdmObjectId, iIntegrationActivity.MDMObjectTypeId, iConnectorProfile, status, comments, OperationResultType.Information, iCallerContext);
                }
            }
            finally
            {
                if (Constants.PERFORMANCE_TRACING_ENABLED || Constants.TRACING_ENABLED)
                {
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("{0}ms - Overall time to generate outbound messages for {1} connector and MDMObjectId : {2}.", _overallDurationHelper.GetDurationInMilliseconds(DateTime.Now), iConnectorProfile.Name, _mdmObjectId), MDMTraceSource.Integration);

                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "End - " + _getOutboundMessagesMethodName, MDMTraceSource.Integration);
                }
            }
            return messages;
        }

        /// <summary>
        /// Create messages to be processed for incoming (External system to MDM) messages.
        /// Take the integration activity, and create respective messages to be processed for given activity.
        /// This method is called after an integration activity is recorded.
        /// </summary>
        /// <param name="iIntegrationActivity">Integration activity which was performed.</param>
        /// <param name="iConnectorProfile">Profile of connector for which messages are to be created.</param>
        /// <param name="iCallerContext">Context of caller making call to this API.</param>
        /// <returns>IntegrationMessages to be processed due to given integration activity</returns>
        public IIntegrationMessageCollection GetInboundMessages(IIntegrationActivity iIntegrationActivity, IConnectorProfile iConnectorProfile, ICallerContext iCallerContext)
        {
            _overallDurationHelper = new DurationHelper(DateTime.Now);

            if (Constants.PERFORMANCE_TRACING_ENABLED || Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.StartTraceActivity(_getInboundMessagesMethodName, MDMTraceSource.Integration, false);
            }

            #region Initial Setup

            IIntegrationMessageCollection messages = null;
            IConnector connectorInstance = GetIConnectorInstance(iConnectorProfile);
            integrationMessageTypeLongName = iIntegrationActivity.IntegrationMessageTypeLongName;
            _callerContext = (CallerContext)iCallerContext;
            _mdmObjectId = iIntegrationActivity.MDMObjectId;

            #endregion

            try
            {
                if (connectorInstance != null)
                {
                    status = GetLocalizedMessage("113181", new Object[] { integrationMessageTypeLongName }); // {0} - Generating inbound messages has started.
                    comments = String.Empty;

                    operationResults = UpdateIntegrationItemStatus(_mdmObjectId, iIntegrationActivity.MDMObjectTypeId, iConnectorProfile, status, comments, OperationResultType.Information, iCallerContext);

                    _durationHelper = new DurationHelper(DateTime.Now);

                    IntegrationEventManager.Instance.OnIntegrationMessageInboundMessagesLoading(
                        new IntegrationEventArgs(iIntegrationActivity, iConnectorProfile, iCallerContext));

                    messages = connectorInstance.GetInboundMessages(iIntegrationActivity, iConnectorProfile, iCallerContext);

                    IntegrationEventManager.Instance.OnIntegrationMessageInboundMessagesLoaded(
                        new IntegrationEventArgs(messages, iIntegrationActivity, iConnectorProfile, iCallerContext));

                    if (Constants.PERFORMANCE_TRACING_ENABLED || Constants.TRACING_ENABLED)
                    {
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("{0}ms - taken to generate inbound messages by {1} connector implementation for MDMObjectId : {2}.", _durationHelper.GetDurationInMilliseconds(DateTime.Now), iConnectorProfile.Name, _mdmObjectId), MDMTraceSource.Integration);
                    }

                    status = GetLocalizedMessage("113182", new Object[] { integrationMessageTypeLongName }); // {0} - Generating inbound messages has completed.
                    comments = GetLocalizedMessage("113178", new Object[] { (messages != null && messages.Count > 0) ? messages.Count : 0 });// Total {0} inbound message(s) generated.

                    operationResults = UpdateIntegrationItemStatus(_mdmObjectId, iIntegrationActivity.MDMObjectTypeId, iConnectorProfile, status, comments, OperationResultType.Information, iCallerContext);
                }
            }
            finally
            {
                if (Constants.PERFORMANCE_TRACING_ENABLED || Constants.TRACING_ENABLED)
                {
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("{0}ms - Overall time to generate inbound messages for {1} connector and MDMObjectId : {2}.", _overallDurationHelper.GetDurationInMilliseconds(DateTime.Now), iConnectorProfile.Name, _mdmObjectId), MDMTraceSource.Integration);
                    MDMTraceHelper.StopTraceActivity(_getInboundMessagesMethodName, MDMTraceSource.Integration);
                }
            }
            return messages;
        }

        /// <summary>
        /// Check if the message is qualified to be sent to outbound system.
        /// It should indicate if the message is qualified for further processing or it needs to be re-processed after given time (ScheduledqualificationTime)
        /// </summary>
        /// <param name="iIntegrationMessage">IIntegrationMessage to be qualified</param>
        /// <param name="iConnectorProfile">Profile of connector for which message is being qualified</param>
        /// <param name="iCallerContext">Context of caller making call to this API.</param>
        /// <returns><see cref="IMessageQualificationResult"/>Indicates if message is qualified. If not, what is the next scheduled time it should be re-processed.</returns>
        public IMessageQualificationResult IsQualified(IIntegrationMessage iIntegrationMessage, IConnectorProfile iConnectorProfile, ICallerContext iCallerContext)
        {
            _overallDurationHelper = new DurationHelper(DateTime.Now);

            if (Constants.PERFORMANCE_TRACING_ENABLED || Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.StartTraceActivity(_isQualifiedMethodName, MDMTraceSource.Integration, false);
            }

            #region Initial Setup

            IMessageQualificationResult result = MDMObjectFactory.GetIMessageQualificationResult();
            IIntegrationMessageHeader iIntegrationMessageHeader = iIntegrationMessage.GetMessageHeader();
            integrationMessageTypeLongName = iIntegrationMessageHeader.IntegrationMessageTypeLongName;
            _callerContext = (CallerContext)iCallerContext;
            String concatenatedComments = String.Empty;
            _mdmObjectId = iIntegrationMessageHeader.MDMObjectId;

            #endregion

            try
            {
                IConnector connectorInstance = GetIConnectorInstance(iConnectorProfile);

                if (connectorInstance != null)
                {
                    status = GetLocalizedMessage("113183", new Object[] { integrationMessageTypeLongName }); // {0} - Qualification has started.
                    comments = String.Empty;

                    operationResults = UpdateIntegrationItemStatus(_mdmObjectId, iIntegrationMessageHeader.MDMObjectTypeId, iConnectorProfile, status, comments, OperationResultType.Information, iCallerContext);

                    _durationHelper = new DurationHelper(DateTime.Now);

                    IntegrationEventManager.Instance.OnIntegrationMessageIsQualifiedStarting(
                        new IntegrationEventArgs(iIntegrationMessage, iConnectorProfile, iCallerContext));

                    result = connectorInstance.IsQualified(iIntegrationMessage, iConnectorProfile, iCallerContext);

                    IntegrationEventManager.Instance.OnIntegrationMessageIsQualifiedCompleted(
                        new IntegrationEventArgs(result, iIntegrationMessage, iConnectorProfile, iCallerContext));

                    if (Constants.PERFORMANCE_TRACING_ENABLED || Constants.TRACING_ENABLED)
                    {
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("{0}ms - taken to qualify message by {1} connector implementation for MDMObjectId : {2}.", _durationHelper.GetDurationInMilliseconds(DateTime.Now), iConnectorProfile.Name, _mdmObjectId), MDMTraceSource.Integration);
                    }

                    status = GetLocalizedMessage("113184", new Object[] { integrationMessageTypeLongName }); // {0} - Qualification has completed.
                    comments = GetLocalizedMessage("113185", new Object[] { result.MessageQualificationStatus }); // Qualification status : {0}

                    if (result != null && result.Comments != null && result.Comments.Count > 0)
                    {
                        foreach (String comment in result.Comments)
                        {
                            concatenatedComments = String.Concat(concatenatedComments, " - ", comment, "<br/>");
                        }

                        comments = String.Concat(comments, "<br/>", GetLocalizedMessage("100255", null), ":", "<br/>", concatenatedComments);
                    }

                    operationResults = UpdateIntegrationItemStatus(_mdmObjectId, iIntegrationMessageHeader.MDMObjectTypeId, iConnectorProfile, status, comments, OperationResultType.Information, iCallerContext);
                }
            }
            finally
            {
                if (Constants.PERFORMANCE_TRACING_ENABLED || Constants.TRACING_ENABLED)
                {
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("{0}ms - Overall time to qualify message for {1} connector and MDMObjectId : {2}.", _overallDurationHelper.GetDurationInMilliseconds(DateTime.Now), iConnectorProfile.Name, _mdmObjectId), MDMTraceSource.Integration);
                    MDMTraceHelper.StopTraceActivity(_isQualifiedMethodName, MDMTraceSource.Integration);
                }
            }
            return result;
        }

        /// <summary>
        /// Process the outgoing (MDM -> External System) message.
        /// This method will be called when connector profile is configured to use workflow. 
        /// After this point, connector is responsible to call other required methods (e.g.,  Send, Transform).
        /// </summary>
        /// <param name="iIntegrationMessage">Message to be processed</param>
        /// <param name="iConnectorProfile">Profile of connector for which message is to be processed.</param>
        /// <param name="iCallerContext">Context of caller making call to this API.</param>
        /// <returns>Result of operation</returns>
        public IOperationResult ProcessOutboundMessage(IIntegrationMessage iIntegrationMessage, IConnectorProfile iConnectorProfile, ICallerContext iCallerContext)
        {
            _overallDurationHelper = new DurationHelper(DateTime.Now);

            if (Constants.PERFORMANCE_TRACING_ENABLED || Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.StartTraceActivity(_processOutboundMessageMethodName, MDMTraceSource.Integration, false);
            }

            #region Initial Setup

            IOperationResult result = null;
            IConnector connectorInstance = GetIConnectorInstance(iConnectorProfile);
            IIntegrationMessageHeader iIntegrationMessageHeader = iIntegrationMessage.GetMessageHeader();
            String integrationMessageTypeLongName = iIntegrationMessageHeader.IntegrationMessageTypeLongName;
            _callerContext = (CallerContext)iCallerContext;
            _mdmObjectId = iIntegrationMessageHeader.MDMObjectId;

            #endregion

            try
            {
                if (connectorInstance != null)
                {
                    status = GetLocalizedMessage("113186", new Object[] { integrationMessageTypeLongName }); // {0} - Process outbound message has started.
                    comments = String.Empty;

                    operationResults = UpdateIntegrationItemStatus(_mdmObjectId, iIntegrationMessageHeader.MDMObjectTypeId, iConnectorProfile, status, comments, OperationResultType.Information, iCallerContext);

                    _durationHelper = new DurationHelper(DateTime.Now);

                    IntegrationEventManager.Instance.OnIntegrationMessageProcessOutboundMessageStarting(
                        new IntegrationEventArgs(iIntegrationMessage, iConnectorProfile, iCallerContext));

                    result = connectorInstance.ProcessOutboundMessage(iIntegrationMessage, iConnectorProfile, iCallerContext);

                    IntegrationEventManager.Instance.OnIntegrationMessageProcessOutboundMessageCompleted(
                        new IntegrationEventArgs(result, iIntegrationMessage, iConnectorProfile, iCallerContext));

                    if (Constants.PERFORMANCE_TRACING_ENABLED || Constants.TRACING_ENABLED)
                    {
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("{0}ms - taken to process outbound message by {1} connector implementation for MDMObjectId : {2}.", _durationHelper.GetDurationInMilliseconds(DateTime.Now), iConnectorProfile.Name, _mdmObjectId), MDMTraceSource.Integration);
                    }

                    status = GetLocalizedMessage("113187", new Object[] { integrationMessageTypeLongName }); // {0} - Process outbound message has completed.
                    comments = String.Empty;

                    operationResults = UpdateIntegrationItemStatus(_mdmObjectId, iIntegrationMessageHeader.MDMObjectTypeId, iConnectorProfile, status, comments, OperationResultType.Information, iCallerContext);
                }
            }
            finally
            {
                if (Constants.PERFORMANCE_TRACING_ENABLED || Constants.TRACING_ENABLED)
                {
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("{0}ms - Overall time to process outbound message for {1} connector and MDMObjectId : {2}.", _overallDurationHelper.GetDurationInMilliseconds(DateTime.Now), iConnectorProfile.Name, _mdmObjectId), MDMTraceSource.Integration);
                    MDMTraceHelper.StopTraceActivity(_processOutboundMessageMethodName, MDMTraceSource.Integration);
                }
            }
            return result;
        }

        /// <summary>
        /// Process the incoming (External System -> MDM ) message.
        /// This method will be called when connector profile is configured to use workflow. 
        /// After this point, connector is responsible to call other required methods (e.g.,  receive).
        /// </summary>
        /// <param name="iIntegrationMessage">Message to be processed</param>
        /// <param name="iConnectorProfile">Profile of connector for which message is to be processed.</param>
        /// <param name="iCallerContext">Context of caller making call to this API.</param>
        /// <returns>Result of operation</returns>
        public IOperationResult ProcessInboundMessage(IIntegrationMessage iIntegrationMessage, IConnectorProfile iConnectorProfile, ICallerContext iCallerContext)
        {
            _overallDurationHelper = new DurationHelper(DateTime.Now);


            if (Constants.PERFORMANCE_TRACING_ENABLED || Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.StartTraceActivity(_processInboundMessageMethodName, MDMTraceSource.Integration, false);
            }

            #region Initial Setup

            IOperationResult result = null;
            IConnector connectorInstance = GetIConnectorInstance(iConnectorProfile);
            IIntegrationMessageHeader iIntegrationMessageHeader = iIntegrationMessage.GetMessageHeader();
            String integrationMessageTypeLongName = iIntegrationMessageHeader.IntegrationMessageTypeLongName;
            _callerContext = (CallerContext)iCallerContext;
            _mdmObjectId = iIntegrationMessageHeader.MDMObjectId;

            #endregion

            try
            {
                if (connectorInstance != null)
                {
                    status = GetLocalizedMessage("113188", new Object[] { integrationMessageTypeLongName }); // {0} - Process inbound message has started.
                    comments = String.Empty;

                    operationResults = UpdateIntegrationItemStatus(_mdmObjectId, iIntegrationMessageHeader.MDMObjectTypeId, iConnectorProfile, status, comments, OperationResultType.Information, iCallerContext);

                    _durationHelper = new DurationHelper(DateTime.Now);

                    IntegrationEventManager.Instance.OnIntegrationMessageProcessInboundMessageCompleted(
                        new IntegrationEventArgs(iIntegrationMessage, iConnectorProfile, iCallerContext));

                    result = connectorInstance.ProcessInboundMessage(iIntegrationMessage, iConnectorProfile, iCallerContext);

                    IntegrationEventManager.Instance.OnIntegrationMessageProcessInboundMessageCompleted(
                        new IntegrationEventArgs(result, iIntegrationMessage, iConnectorProfile, iCallerContext));

                    if (Constants.PERFORMANCE_TRACING_ENABLED || Constants.TRACING_ENABLED)
                    {
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("{0}ms - taken to process inbound message by {1} connector implementation for MDMObjectId : {2}.", _durationHelper.GetDurationInMilliseconds(DateTime.Now), iConnectorProfile.Name, _mdmObjectId), MDMTraceSource.Integration);
                    }

                    status = GetLocalizedMessage("113189", new Object[] { integrationMessageTypeLongName }); // {0} - Process inbound message has completed.
                    comments = String.Empty;

                    operationResults = UpdateIntegrationItemStatus(_mdmObjectId, iIntegrationMessageHeader.MDMObjectTypeId, iConnectorProfile, status, comments, OperationResultType.Information, iCallerContext);
                }
            }
            finally
            {
                if (Constants.PERFORMANCE_TRACING_ENABLED || Constants.TRACING_ENABLED)
                {
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("{0}ms - Overall time to process inbound message for {1} connector and MDMObjectId : {1}.", _overallDurationHelper.GetDurationInMilliseconds(DateTime.Now), iConnectorProfile.Name, _mdmObjectId), MDMTraceSource.Integration);
                    MDMTraceHelper.StopTraceActivity(_processInboundMessageMethodName, MDMTraceSource.Integration);
                }
            }
            return result;
        }

        /// <summary>
        /// Transform internal MDM message into format understood by external system (ThirdPartySystem).
        /// </summary>
        /// <param name="iIntegrationMessage">Message to be transformed which will be sent to external system.</param>
        /// <param name="iConnectorProfile">Profile of connector for which message is to be transformed.</param>
        /// <param name="iCallerContext">Context of caller making call to this API.</param>
        /// <returns><see cref="IIntegrationData"/> which will be sent to external system and is understood by that system</returns>
        public IIntegrationData Transform(IIntegrationMessage iIntegrationMessage, IConnectorProfile iConnectorProfile, ICallerContext iCallerContext)
        {
            _overallDurationHelper = new DurationHelper(DateTime.Now);

            if (Constants.PERFORMANCE_TRACING_ENABLED || Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.StartTraceActivity(_transformMethodName, MDMTraceSource.Integration, false);
            }

            #region Initial Setup

            IIntegrationData iIntegrationData = null;
            IConnector connectorInstance = GetIConnectorInstance(iConnectorProfile);
            IIntegrationMessageHeader iIntegrationMessageHeader = iIntegrationMessage.GetMessageHeader();
            String integrationMessageTypeLongName = iIntegrationMessageHeader.IntegrationMessageTypeLongName;
            _callerContext = (CallerContext)iCallerContext;
            _mdmObjectId = iIntegrationMessageHeader.MDMObjectId;

            #endregion

            try
            {
                if (connectorInstance != null)
                {
                    status = GetLocalizedMessage("113190", new Object[] { integrationMessageTypeLongName }); // {0} - Transform has started.
                    comments = String.Empty;

                    operationResults = UpdateIntegrationItemStatus(_mdmObjectId, iIntegrationMessageHeader.MDMObjectTypeId, iConnectorProfile, status, comments, OperationResultType.Information, iCallerContext);

                    _durationHelper = new DurationHelper(DateTime.Now);

                    IntegrationEventManager.Instance.OnIntegrationMessageTransformStarting(
                        new IntegrationEventArgs(iIntegrationMessage, iConnectorProfile, iCallerContext));

                    iIntegrationData = connectorInstance.Transform(iIntegrationMessage, iConnectorProfile, iCallerContext);

                    IntegrationEventManager.Instance.OnIntegrationMessageTransformCompleted(
                        new IntegrationEventArgs(iIntegrationData, iIntegrationMessage, iConnectorProfile, iCallerContext));


                    if (Constants.PERFORMANCE_TRACING_ENABLED || Constants.TRACING_ENABLED)
                    {
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("{0}ms - taken to transform message by {1} connector implementation for MDMObjectId : {2}.", _durationHelper.GetDurationInMilliseconds(DateTime.Now), iConnectorProfile.Name, _mdmObjectId), MDMTraceSource.Integration);
                    }

                    status = GetLocalizedMessage("113191", new Object[] { integrationMessageTypeLongName }); // {0} - Transform has completed.
                    comments = (iIntegrationData == null) ? GetLocalizedMessage("113204", null) : String.Empty; //Transformed data is not available

                    operationResults = UpdateIntegrationItemStatus(_mdmObjectId, iIntegrationMessageHeader.MDMObjectTypeId, iConnectorProfile, status, comments, OperationResultType.Information, iCallerContext);

                    if (iIntegrationData != null)
                    {
                        if (!iIntegrationData.Contains("MDMObjectId"))
                        {
                            iIntegrationData.AddAdditionalData("MDMObjectId", iIntegrationMessageHeader.MDMObjectId.ToString());
                        }

                        if (!iIntegrationData.Contains("MDMObjectTypeId"))
                        {
                            iIntegrationData.AddAdditionalData("MDMObjectTypeId", iIntegrationMessageHeader.MDMObjectTypeId.ToString());
                        }
                    }
                }
            }
            finally
            {
                if (Constants.PERFORMANCE_TRACING_ENABLED || Constants.TRACING_ENABLED)
                {
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("{0}ms - Overall time to transform message for {1} connector and MDMObjectId : {2}.", _overallDurationHelper.GetDurationInMilliseconds(DateTime.Now), iConnectorProfile.Name, _mdmObjectId), MDMTraceSource.Integration);
                    MDMTraceHelper.StopTraceActivity(_transformMethodName, MDMTraceSource.Integration);
                }
            }

            return iIntegrationData;
        }

        /// <summary>
        /// Send message to external system. This send will happen for single message (which is transformed in <see cref="IThirdPartyObject"/>
        /// </summary>
        /// <param name="iIntegrationData">Transformed message - ready to be sent to external system</param>
        /// <param name="iConnectorProfile">Profile of connector for which message is to be sent</param>
        /// <param name="iCallerContext">Context of caller making call to this API.</param>
        /// <returns>Result of operation</returns>
        public IOperationResult Send(IIntegrationData iIntegrationData, IConnectorProfile iConnectorProfile, ICallerContext iCallerContext)
        {
            _overallDurationHelper = new DurationHelper(DateTime.Now);

            if (Constants.PERFORMANCE_TRACING_ENABLED || Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.StartTraceActivity(_sendMethodName, MDMTraceSource.Integration, false);
            }

            #region Initial Setup

            IOperationResult iOperationResult = null;
            IConnector connectorInstance = GetIConnectorInstance(iConnectorProfile);
            _callerContext = (CallerContext)iCallerContext;

            #endregion

            try
            {
                if (connectorInstance != null)
                {
                    status = GetLocalizedMessage("113192", new Object[] { integrationMessageTypeLongName }); // {0} - Send has started.
                    comments = String.Empty;

                    _mdmObjectId = ValueTypeHelper.Int64TryParse(iIntegrationData.GetAdditionalData("MDMObjectId"), 0);
                    Int16 mdmObjectTypeId = ValueTypeHelper.Int16TryParse(iIntegrationData.GetAdditionalData("MDMObjectTypeId"), 0);

                    operationResults = UpdateIntegrationItemStatus(_mdmObjectId, mdmObjectTypeId, iConnectorProfile, status, comments, OperationResultType.Information, iCallerContext);

                    _durationHelper = new DurationHelper(DateTime.Now);

                    IntegrationEventManager.Instance.OnIntegrationMessageSendStarting(
                        new IntegrationEventArgs(iIntegrationData, iConnectorProfile, iCallerContext));

                    iOperationResult = connectorInstance.Send(iIntegrationData, iConnectorProfile, iCallerContext);

                    IntegrationEventManager.Instance.OnIntegrationMessageSendStarting(
                        new IntegrationEventArgs(iOperationResult, iIntegrationData, iConnectorProfile, iCallerContext));

                    if (Constants.PERFORMANCE_TRACING_ENABLED || Constants.TRACING_ENABLED)
                    {
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("{0}ms - taken to send message by {1} connector implementation for MDMObjectId : {2}.", _durationHelper.GetDurationInMilliseconds(DateTime.Now), iConnectorProfile.Name, _mdmObjectId), MDMTraceSource.Integration);
                    }

                    status = GetLocalizedMessage("113193", new Object[] { integrationMessageTypeLongName }); // {0} - Send has completed.
                    comments = ReadOperationResult(iOperationResult, (CallerContext)iCallerContext);

                    operationResults = UpdateIntegrationItemStatus(_mdmObjectId, mdmObjectTypeId, iConnectorProfile, status, comments, OperationResultType.Information, iCallerContext);
                }
            }
            finally
            {
                if (Constants.PERFORMANCE_TRACING_ENABLED || Constants.TRACING_ENABLED)
                {
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("{0}ms - Overall time to send message for {1} connector and MDMObjectId : {2}.", _overallDurationHelper.GetDurationInMilliseconds(DateTime.Now), iConnectorProfile.Name, _mdmObjectId), MDMTraceSource.Integration);
                    MDMTraceHelper.StopTraceActivity(_sendMethodName, MDMTraceSource.Integration);
                }
            }
            return iOperationResult;
        }

        /// <summary>
        /// Queue ready to send messages to be batched and sent to external system.
        /// Messages will be queued and not sent immediately. These messages will be batched based on profile configuration and then will be sent to external system.
        /// </summary>
        /// <param name="iIntegrationData">Ready to send TOP to be queued for batch send</param>
        /// <param name="iConnectorProfile">Profile of connector for which messages are to be batched and sent</param>
        /// <param name="iCallerContext">Context of caller making call to this API.</param>
        /// <returns>Result of operation</returns>
        public IOperationResult BatchSend(IIntegrationData iIntegrationData, IConnectorProfile iConnectorProfile, ICallerContext iCallerContext)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Indicates the received message from external system. This will be called when connector profile indicates not to use workflow.
        /// </summary>
        /// <param name="iIntegrationMessage">Indicates message received from external system.</param>
        /// <param name="iConnectorProfile">Profile for connector for which message is received.</param>
        /// <param name="iCallerContext">Context of caller making call to this API.</param>
        /// <returns>Result of operation</returns>
        public IOperationResult Receive(IIntegrationMessage iIntegrationMessage, IConnectorProfile iConnectorProfile, ICallerContext iCallerContext)
        {
            _overallDurationHelper = new DurationHelper(DateTime.Now);

            if (Constants.PERFORMANCE_TRACING_ENABLED || Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.StartTraceActivity(_receiveMethodName, MDMTraceSource.Integration, false);
            }

            #region Initial Setup

            IOperationResult iOperationResult = null;
            IConnector connectorInstance = GetIConnectorInstance(iConnectorProfile);
            IIntegrationMessageHeader iIntegrationMessageHeader = iIntegrationMessage.GetMessageHeader();
            String integrationMessageTypeLongName = iIntegrationMessageHeader.IntegrationMessageTypeLongName;
            _callerContext = (CallerContext)iCallerContext;
            _mdmObjectId = iIntegrationMessageHeader.MDMObjectId;

            #endregion

            try
            {
                if (connectorInstance != null)
                {
                    status = GetLocalizedMessage("113196", new Object[] { integrationMessageTypeLongName }); // {0} - Receive has started.
                    comments = String.Empty;

                    operationResults = UpdateIntegrationItemStatus(_mdmObjectId, iIntegrationMessageHeader.MDMObjectTypeId, iConnectorProfile, status, comments, OperationResultType.Information, iCallerContext);

                    _durationHelper = new DurationHelper(DateTime.Now);

                    IntegrationEventManager.Instance.OnIntegrationMessageReceiveStarting(
                        new IntegrationEventArgs(iIntegrationMessage, iConnectorProfile, iCallerContext));

                    iOperationResult = connectorInstance.Receive(iIntegrationMessage, iConnectorProfile, iCallerContext);

                    IntegrationEventManager.Instance.OnIntegrationMessageReceiveCompleted(
                        new IntegrationEventArgs(iOperationResult, iIntegrationMessage, iConnectorProfile, iCallerContext));

                    if (Constants.PERFORMANCE_TRACING_ENABLED || Constants.TRACING_ENABLED)
                    {
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("{0}ms - taken to receive message by connector {1} implementation for MDMObjectId : {2}.", _durationHelper.GetDurationInMilliseconds(DateTime.Now), iConnectorProfile.Name, _mdmObjectId), MDMTraceSource.Integration);
                    }

                    status = GetLocalizedMessage("113197", new Object[] { integrationMessageTypeLongName }); // {0} - Receive has completed.

                    comments = ReadOperationResult(iOperationResult, (CallerContext)iCallerContext);

                    operationResults = UpdateIntegrationItemStatus(_mdmObjectId, iIntegrationMessageHeader.MDMObjectTypeId, iConnectorProfile, status, comments, OperationResultType.Information, iCallerContext);
                }
            }
            finally
            {
                if (Constants.PERFORMANCE_TRACING_ENABLED || Constants.TRACING_ENABLED)
                {
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("{0}ms - Overall time to receive message for {1} connector and MDMObjectId : {2}.", _overallDurationHelper.GetDurationInMilliseconds(DateTime.Now), iConnectorProfile.Name, _mdmObjectId), MDMTraceSource.Integration);
                    MDMTraceHelper.StopTraceActivity(_receiveMethodName, MDMTraceSource.Integration);
                }
            }
            return iOperationResult;
        }

        /// <summary>
        /// Create aggregated messages to be processed for outgoing (MDM to External system) messages.
        /// Take the integration messages, and create aggregated messages to be processed.
        /// This method is called after an message is qualified.
        /// </summary>
        /// <param name="messagesToBeAggregated">The messages to be aggregated.</param>
        /// <param name="iConnectorProfile">Profile of connector for which message is to be processed.</param>
        /// <param name="iCallerContext">Context of caller making call to this API.</param>
        /// <returns>
        /// Aggregated IntegrationMessages to be processed
        /// </returns>
        public IIntegrationMessageCollection GetOutboundAggregatedMessages(IIntegrationMessageCollection messagesToBeAggregated, IConnectorProfile iConnectorProfile, ICallerContext iCallerContext)
        {
            _overallDurationHelper = new DurationHelper(DateTime.Now);


            if (Constants.PERFORMANCE_TRACING_ENABLED || Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.StartTraceActivity(_getOutboundAggregatedMessagesMethodName, MDMTraceSource.Integration, false);
            }

            #region Initial Setup

            IIntegrationMessageCollection messages = null;
            IConnector connectorInstance = GetIConnectorInstance(iConnectorProfile);

            #endregion

            try
            {
                if (connectorInstance != null)
                {
                    //TODO: call update item status for outbound aggregated messages

                    _durationHelper = new DurationHelper(DateTime.Now);

                    IntegrationEventManager.Instance.OnIntegrationMessageOutboundAggregatedMessagesLoading(
                        new IntegrationEventArgs(messagesToBeAggregated, iConnectorProfile, iCallerContext));

                    messages = connectorInstance.GetOutboundAggregatedMessages(messagesToBeAggregated, iConnectorProfile, iCallerContext);

                    IntegrationEventManager.Instance.OnIntegrationMessageOutboundAggregatedMessagesLoaded(
                        new IntegrationEventArgs(messages, messagesToBeAggregated, iConnectorProfile, iCallerContext));

                    if (Constants.PERFORMANCE_TRACING_ENABLED || Constants.TRACING_ENABLED)
                    {
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("{0}ms - taken to get outbound aggregated messages by {1} connector implementation.", _durationHelper.GetDurationInMilliseconds(DateTime.Now), iConnectorProfile.Name), MDMTraceSource.Integration);
                    }
                }
            }
            finally
            {
                if (Constants.PERFORMANCE_TRACING_ENABLED || Constants.TRACING_ENABLED)
                {
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("{0}ms - Overall time to get outbound aggregated messages for {1} connector.", _overallDurationHelper.GetDurationInMilliseconds(DateTime.Now), iConnectorProfile.Name), MDMTraceSource.Integration);
                    MDMTraceHelper.StopTraceActivity(_getOutboundAggregatedMessagesMethodName, MDMTraceSource.Integration);
                }
            }

            return messages;
        }

        /// <summary>
        /// Create aggregated messages to be processed for incoming (External system to MDM) messages.
        /// Take the integration messages, and create aggregated messages to be processed.
        /// This method is called after an message is qualified.
        /// </summary>
        /// <param name="messagesToBeAggregated">The messages to be aggregated.</param>
        /// <param name="iConnectorProfile">Profile of connector for which message is to be processed.</param>
        /// <param name="iCallerContext">Context of caller making call to this API.</param>
        /// <returns>
        /// Aggregated IntegrationMessages to be processed
        /// </returns>
        public IIntegrationMessageCollection GetInboundAggregatedMessages(IIntegrationMessageCollection messagesToBeAggregated, IConnectorProfile iConnectorProfile, ICallerContext iCallerContext)
        {
            _overallDurationHelper = new DurationHelper(DateTime.Now);

            if (Constants.PERFORMANCE_TRACING_ENABLED || Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.StartTraceActivity(_getInboundAggregatedMessagesMethodName, MDMTraceSource.Integration, false);
            }

            #region Initial Setup

            IIntegrationMessageCollection messages = null;
            IConnector connectorInstance = GetIConnectorInstance(iConnectorProfile);

            #endregion

            try
            {
                if (connectorInstance != null)
                {
                    //TODO: call update item status for inbound aggregated messages

                    _durationHelper = new DurationHelper(DateTime.Now);

                    IntegrationEventManager.Instance.OnIntegrationMessageInboundAggregatedMessagesLoading(
                        new IntegrationEventArgs(messagesToBeAggregated, iConnectorProfile, iCallerContext));

                    messages = connectorInstance.GetInboundAggregatedMessages(messagesToBeAggregated, iConnectorProfile, iCallerContext);

                    IntegrationEventManager.Instance.OnIntegrationMessageInboundAggregatedMessagesLoaded(
                        new IntegrationEventArgs(messages, messagesToBeAggregated, iConnectorProfile, iCallerContext));

                    if (Constants.PERFORMANCE_TRACING_ENABLED || Constants.TRACING_ENABLED)
                    {
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("{0}ms - taken to get inbound aggregated messages by {1} connector implementation.", _durationHelper.GetDurationInMilliseconds(DateTime.Now), iConnectorProfile.Name), MDMTraceSource.Integration);
                    }
                }
            }
            finally
            {
                if (Constants.PERFORMANCE_TRACING_ENABLED || Constants.TRACING_ENABLED)
                {
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("{0}ms - Overall time to get inbound aggregated messages for {1} connector.", _overallDurationHelper.GetDurationInMilliseconds(DateTime.Now), iConnectorProfile.Name), MDMTraceSource.Integration);
                    MDMTraceHelper.StopTraceActivity(_getInboundAggregatedMessagesMethodName, MDMTraceSource.Integration);
                }
            }

            return messages;
        }

        #endregion IConnector methods

        #region Helper Methods

        /// <summary>
        /// Get message. Calls respective GetInboundMessage or GetOutboundMessage based on IntegratiionActivityLog.IntegrationType
        /// </summary>
        /// <param name="integrationActivityLog">IntegrationActivityLog for which messages are to be created.</param>
        /// <param name="iCallerContext">Context of caller making call to this API.</param>
        /// <returns>Messages created from the given activity log</returns>
        public IntegrationMessageCollection GetMessages(IntegrationActivityLog integrationActivityLog, ICallerContext iCallerContext)
        {
            IIntegrationMessageCollection iMessages = null;

            if (integrationActivityLog != null)
            {
                IIntegrationActivity iIntegrationActivity = new IntegrationActivity(integrationActivityLog);
                ConnectorProfile connectorProfile = this.GetConnectorProfile(integrationActivityLog.ConnectorId, iCallerContext);

                if (connectorProfile != null)
                {
                    switch (integrationActivityLog.IntegrationType)
                    {
                        case IntegrationType.Inbound:
                            iMessages = this.GetInboundMessages(iIntegrationActivity, connectorProfile, iCallerContext);
                            break;

                        case IntegrationType.Outbound:
                            iMessages = this.GetOutboundMessages(iIntegrationActivity, connectorProfile, iCallerContext);
                            break;

                        default:
                            MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, "CoreConnector.GetMessage : Unknown IIntegrationActivity.IntegrationType - " + iIntegrationActivity.IntegrationType, MDMTraceSource.Integration);
                            break;
                    }
                }
                else
                {
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, "CoreConnector.GetMessages : Failed to load IConnectorProfile. Cannot call GetMessage for inbound / outbound activity", MDMTraceSource.Integration);
                }
            }
            else
            {
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, "CoreConnector.GetMessage : IntegrationActivityLog is null. No action will be performed", MDMTraceSource.Integration);
            }

            IntegrationMessageCollection messages = null;
            if (iMessages != null)
            {
                messages = (IntegrationMessageCollection)iMessages;
            }

            return messages;
        }

        /// <summary>
        /// Process the message - calls respective <see cref="ProcessOutboundMessage"/> or <see cref="ProcessInboundMessage"/> based on integration type.
        /// </summary>
        /// <param name="iIntegrationMessage">InegrationMessage to be processed</param>
        /// <param name="iCallerContext">Context of caller making call to this API.</param>
        /// <returns>Result of operation</returns>
        public IOperationResult ProcessMessage(IIntegrationMessage iIntegrationMessage, ICallerContext iCallerContext)
        {
            IOperationResult result = null;

            if (iIntegrationMessage != null)
            {
                IConnectorProfile iConnectorProfile = this.GetConnectorProfile(iIntegrationMessage.GetMessageHeader().ConnectorId, iCallerContext);

                if (iConnectorProfile != null)
                {
                    switch (iIntegrationMessage.GetMessageHeader().IntegrationType)
                    {
                        case IntegrationType.Inbound:
                            if (iConnectorProfile.GetRunTimeSpecifications().UseInplaceOrchestration == false)
                            {
                                result = this.ProcessInboundMessage(iIntegrationMessage, iConnectorProfile, iCallerContext);
                            }
                            else
                            {
                                result = this.OrchestrateInboundMessage(iIntegrationMessage, iConnectorProfile, iCallerContext);
                            }
                            break;

                        case IntegrationType.Outbound:
                            if (iConnectorProfile.GetRunTimeSpecifications().UseInplaceOrchestration == false)
                            {
                                result = this.ProcessOutboundMessage(iIntegrationMessage, iConnectorProfile, iCallerContext);
                            }
                            else
                            {
                                result = this.OrchestrateOutboundMessage(iIntegrationMessage, iConnectorProfile, iCallerContext);
                            }
                            break;

                        default:
                            MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, "CoreConnector.ProcessMessage : Unknown IIntegrationActivity.IntegrationType - " + iIntegrationMessage.GetMessageHeader().IntegrationType, MDMTraceSource.Integration);
                            break;
                    }
                }
                else
                {
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, "CoreConnector.ProcessMessages : Failed to load IConnectorProfile. Cannot call ProcessMessage for inbound / outbound activity", MDMTraceSource.Integration);
                }
            }
            else
            {
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, "CoreConnector.ProcessMessage : iIntegrationMessage is null. Cannot call ProcessMessage for inbound / outbound activity", MDMTraceSource.Integration);
            }
            return result;
        }

        /// <summary>
        /// Check if the messages is qualified to be sent for further processing.
        /// </summary>
        /// <param name="iIntegrationMessage">Indicates message to be checked for qualification</param>
        /// <param name="iCallerContext">Context of caller making call to this API.</param>
        /// <returns><see cref="IMessageQualificationResult"/> Indicates the result of qualification check.</returns>
        public IMessageQualificationResult IsQualified(IIntegrationMessage iIntegrationMessage, ICallerContext iCallerContext)
        {
            IMessageQualificationResult result = null;

            if (iIntegrationMessage != null)
            {
                IConnectorProfile iConnectorProfile = this.GetConnectorProfile(iIntegrationMessage.GetMessageHeader().ConnectorId, iCallerContext);

                if (iConnectorProfile != null)
                {
                    result = this.IsQualified(iIntegrationMessage, iConnectorProfile, iCallerContext);
                }
                else
                {
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, "CoreConnector.IsQualified : Failed to load IConnectorProfile. Cannot call IsQualified", MDMTraceSource.Integration);
                }
            }
            else
            {
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, "CoreConnector.IsQualified : iIntegrationMessage is null. Cannot call IsQualified", MDMTraceSource.Integration);
            }
            return result;
        }

        /// <summary>
        /// Get aggregated messages. Calls respective GetInboundAggregatedMessages or GetOutboundAggregatedMessages based on integration type
        /// </summary>
        /// <param name="messagesToBeAggregated">The messages to be aggregated.</param>
        /// <param name="connectorId">The connector profile identifier.</param>
        /// <param name="integrationType">Type of integration (inbound or outbound).</param>
        /// <param name="iCallerContext">Context of caller making call to this API.</param>
        /// <returns>
        /// Aggregated Messages created from the given message collection
        /// </returns>
        public IntegrationMessageCollection GetAggregatedMessages(IIntegrationMessageCollection messagesToBeAggregated, Int16 connectorId, IntegrationType integrationType, ICallerContext iCallerContext)
        {
            IIntegrationMessageCollection iMessages = null;

            if (messagesToBeAggregated != null)
            {
                ConnectorProfile connectorProfile = this.GetConnectorProfile(connectorId, iCallerContext);

                if (connectorProfile != null)
                {
                    switch (integrationType)
                    {
                        case IntegrationType.Inbound:
                            iMessages = this.GetInboundAggregatedMessages(messagesToBeAggregated, connectorProfile, iCallerContext);
                            break;

                        case IntegrationType.Outbound:
                            iMessages = this.GetOutboundAggregatedMessages(messagesToBeAggregated, connectorProfile, iCallerContext);
                            break;

                        default:
                            MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, "CoreConnector.GetAggregatedMessages : Unknown IntegrationType - " + integrationType, MDMTraceSource.Integration);
                            break;
                    }
                }
                else
                {
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, "CoreConnector.GetAggregatedMessages : Failed to load IConnectorProfile. Cannot call GetAggregatedMessages for inbound / outbound activity", MDMTraceSource.Integration);
                }
            }
            else
            {
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, "CoreConnector.GetAggregatedMessages : Messages to be aggregated is null. No action will be performed", MDMTraceSource.Integration);
            }

            IntegrationMessageCollection messages = null;
            if (iMessages != null)
            {
                messages = (IntegrationMessageCollection)iMessages;
            }

            return messages;
        }

        #endregion Helper Methods

        #region Private Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="iConnectorProfile"></param>
        /// <returns></returns>
        private IConnector GetIConnectorInstance(IConnectorProfile iConnectorProfile)
        {
            IConnector connectorInstance = null;

            if (iConnectorProfile != null)
            {
                String baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
                String assemblyPath = String.Concat(baseDirectory, "Connectors", "\\bin\\");

                String assemblyName = iConnectorProfile.GetRunTimeSpecifications().AssemblyName;
                String className = iConnectorProfile.GetRunTimeSpecifications().ClassName;

                #region Initialize class

                String assemblyFullPath = String.Format(@"{0}{1}", assemblyPath, assemblyName);
                if (IO.File.Exists(assemblyFullPath))
                {
                    Assembly connectorAssembly = Assembly.LoadFrom(assemblyFullPath);
                    Type connectorType = connectorAssembly.GetType(className);
                    connectorInstance = (IConnector)Activator.CreateInstance(connectorType);
                }
                else
                {
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, "CoreConnector.GetIConnectorInstance : Connector assembly path : " + assemblyFullPath + " doesn't exist or file is not available at this location", MDMTraceSource.Integration);
                }

                #endregion Initialize class

                if (connectorInstance == null)
                {
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, "CoreConnector.GetIConnectorInstance : Connector instance is null", MDMTraceSource.Integration);
                }
            }
            else
            {
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, "CoreConnector.GetIConnectorInstance : IConnectorProfile is null, cannot get instance of connector", MDMTraceSource.Integration);
            }
            return connectorInstance;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectorProfileId"></param>
        /// <param name="iCallerContext"></param>
        /// <returns></returns>
        private ConnectorProfile GetConnectorProfile(Int16 connectorProfileId, ICallerContext iCallerContext)
        {
            ConnectorProfile connectorProfile = null;

            if (connectorProfileId > 0)
            {
                connectorProfile = _connectorProfileBL.GetById(connectorProfileId, iCallerContext as CallerContext);
            }
            else
            {
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, "CoreConnector.GetConnectorProfile. ConnectorProfileId is not provided. Cannot fetch IConnectorProfile", MDMTraceSource.Integration);
            }
            return connectorProfile;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="iIntegrationMessage"></param>
        /// <param name="iConnectorProfile"></param>
        /// <param name="iCallerContext"></param>
        /// <returns></returns>
        private IOperationResult OrchestrateInboundMessage(IIntegrationMessage iIntegrationMessage, IConnectorProfile iConnectorProfile, ICallerContext iCallerContext)
        {
            _overallDurationHelper = new DurationHelper(DateTime.Now);

            if (Constants.PERFORMANCE_TRACING_ENABLED || Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.StartTraceActivity(_orchestrateInboundMessageMethodName, MDMTraceSource.Integration, false);
            }

            #region Initial Setup

            IOperationResult result = null;
            IConnector connectorInstance = GetIConnectorInstance(iConnectorProfile);
            IIntegrationMessageHeader iIntegrationMessageHeader = iIntegrationMessage.GetMessageHeader();
            String integrationMessageTypeLongName = iIntegrationMessageHeader.IntegrationMessageTypeLongName;

            #endregion

            try
            {
                if (connectorInstance != null)
                {
                    if (Constants.TRACING_ENABLED)
                    {
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("Start - Calling IConnector.Receive for IntegrationMessage.Id = {0}", iIntegrationMessage.Id), MDMTraceSource.Integration);
                    }

                    status = GetLocalizedMessage("113196", new Object[] { integrationMessageTypeLongName }); // {0} - Receive has started.

                    operationResults = UpdateIntegrationItemStatus(iIntegrationMessageHeader.MDMObjectId, iIntegrationMessageHeader.MDMObjectTypeId,
                                                                   iConnectorProfile, status, comments, OperationResultType.Information, iCallerContext);

                    _durationHelper = new DurationHelper(DateTime.Now);

                    IntegrationEventManager.Instance.OnIntegrationMessageReceiveStarting(
                        new IntegrationEventArgs(iIntegrationMessage, iConnectorProfile, iCallerContext));

                    result = connectorInstance.Receive(iIntegrationMessage, iConnectorProfile, iCallerContext);

                    IntegrationEventManager.Instance.OnIntegrationMessageReceiveCompleted(
                        new IntegrationEventArgs(result, iIntegrationMessage, iConnectorProfile, iCallerContext));

                    if (Constants.PERFORMANCE_TRACING_ENABLED || Constants.TRACING_ENABLED)
                    {
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("{0}ms - taken to receive message by {1} connector implementation for MDMObjectId : {2}.", _durationHelper.GetDurationInMilliseconds(DateTime.Now), iConnectorProfile.Name, _mdmObjectId), MDMTraceSource.Integration);
                    }

                    status = GetLocalizedMessage("113197", new Object[] { integrationMessageTypeLongName }); // {0} - Receive has completed.

                    comments = ReadOperationResult(result, (CallerContext)iCallerContext);

                    operationResults = UpdateIntegrationItemStatus(iIntegrationMessageHeader.MDMObjectId, iIntegrationMessageHeader.MDMObjectTypeId,
                                                                   iConnectorProfile, status, comments, OperationResultType.Information, iCallerContext);

                    if (Constants.TRACING_ENABLED)
                    {
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("End - Calling IConnector.Receive for IntegrationMessage.Id = {0}", iIntegrationMessage.Id), MDMTraceSource.Integration);
                    }
                }
            }
            finally
            {
                if (Constants.PERFORMANCE_TRACING_ENABLED || Constants.TRACING_ENABLED)
                {
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("{0}ms - Overall time to orchestrate inbound message for {1} connector and MDMObjectId : {2}.", _overallDurationHelper.GetDurationInMilliseconds(DateTime.Now), iConnectorProfile.Name, _mdmObjectId), MDMTraceSource.Integration);
                    MDMTraceHelper.StopTraceActivity(_orchestrateInboundMessageMethodName, MDMTraceSource.Integration);
                }
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="iIntegrationMessage"></param>
        /// <param name="iConnectorProfile"></param>
        /// <param name="iCallerContext"></param>
        /// <returns></returns>
        private IOperationResult OrchestrateOutboundMessage(IIntegrationMessage iIntegrationMessage, IConnectorProfile iConnectorProfile, ICallerContext iCallerContext)
        {
            _overallDurationHelper = new DurationHelper(DateTime.Now);

            if (Constants.PERFORMANCE_TRACING_ENABLED || Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.StartTraceActivity(_orchestrateOutboundMessageMethodName, MDMTraceSource.Integration, false);
            }

            #region Initial Setup

            IOperationResult iOperationResult = null;
            IConnector connectorInstance = GetIConnectorInstance(iConnectorProfile);
            IIntegrationMessageHeader iIntegrationMessageHeader = iIntegrationMessage.GetMessageHeader();
            String integrationMessageTypeLongName = iIntegrationMessageHeader.IntegrationMessageTypeLongName;
            _mdmObjectId = iIntegrationMessageHeader.MDMObjectId;

            #endregion

            try
            {
                if (connectorInstance != null)
                {
                    if (Constants.TRACING_ENABLED)
                    {
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("Start - Calling IConnector.Transform for IntegrationMessage.Id = {0}", iIntegrationMessage.Id), MDMTraceSource.Integration);
                    }

                    status = GetLocalizedMessage("113190", new Object[] { integrationMessageTypeLongName }); // {0} - Transform has started.
                    comments = String.Empty;

                    operationResults = UpdateIntegrationItemStatus(_mdmObjectId, iIntegrationMessageHeader.MDMObjectTypeId, iConnectorProfile, status, comments, OperationResultType.Information, iCallerContext);

                    _durationHelper = new DurationHelper(DateTime.Now);

                    IntegrationEventManager.Instance.OnIntegrationMessageTransformStarting(
                        new IntegrationEventArgs(iIntegrationMessage, iConnectorProfile, iCallerContext));

                    IIntegrationData iIntegrationData = connectorInstance.Transform(iIntegrationMessage, iConnectorProfile, iCallerContext);

                    IntegrationEventManager.Instance.OnIntegrationMessageTransformCompleted(
                        new IntegrationEventArgs(iIntegrationData, iIntegrationMessage, iConnectorProfile, iCallerContext));

                    if (Constants.PERFORMANCE_TRACING_ENABLED || Constants.TRACING_ENABLED)
                    {
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("{0}ms - taken to transform message by {1} connector implementation for MDMObjectId : {2}.", _durationHelper.GetDurationInMilliseconds(DateTime.Now), iConnectorProfile.Name, _mdmObjectId), MDMTraceSource.Integration);
                    }

                    status = GetLocalizedMessage("113191", new Object[] { integrationMessageTypeLongName }); // {0} - Transform has completed.
                    comments = (iIntegrationData == null) ? GetLocalizedMessage("113204", null) : String.Empty; //Transformed data is not available

                    operationResults = UpdateIntegrationItemStatus(_mdmObjectId, iIntegrationMessageHeader.MDMObjectTypeId, iConnectorProfile, status, comments, OperationResultType.Information, iCallerContext);

                    if (Constants.TRACING_ENABLED)
                    {
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("End - Calling IConnector.Transform for IntegrationMessage.Id = {0}", iIntegrationMessage.Id), MDMTraceSource.Integration);
                    }

                    if (iIntegrationData == null)
                    {
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Warning, String.Format("IConnector.Transform() returned null IntegrationData for IntegrationMessage.Id = {0}", iIntegrationMessage.Id), MDMTraceSource.Integration);
                    }
                    else
                    {
                        if (Constants.TRACING_ENABLED)
                        {
                            MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("Start - Calling IConnector.Send for IntegrationMessage.Id = {0}", iIntegrationMessage.Id), MDMTraceSource.Integration);
                        }

                        status = GetLocalizedMessage("113192", new Object[] { integrationMessageTypeLongName }); // {0} - Send has started.
                        comments = String.Empty;

                        operationResults = UpdateIntegrationItemStatus(_mdmObjectId, iIntegrationMessageHeader.MDMObjectTypeId,
                                                                       iConnectorProfile, status, comments, OperationResultType.Information, iCallerContext);

                        _durationHelper = new DurationHelper(DateTime.Now);

                        IntegrationEventManager.Instance.OnIntegrationMessageSendStarting(
                            new IntegrationEventArgs(iIntegrationData, iConnectorProfile, iCallerContext));

                        iOperationResult = connectorInstance.Send(iIntegrationData, iConnectorProfile, iCallerContext);

                        IntegrationEventManager.Instance.OnIntegrationMessageSendCompleted(
                            new IntegrationEventArgs(iOperationResult, iIntegrationData, iConnectorProfile, iCallerContext));

                        if (Constants.PERFORMANCE_TRACING_ENABLED || Constants.TRACING_ENABLED)
                        {
                            MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("{0}ms - taken to send message by {1} connector implementation for MDMObjectId : {2}.", _durationHelper.GetDurationInMilliseconds(DateTime.Now), iConnectorProfile.Name, _mdmObjectId), MDMTraceSource.Integration);
                        }

                        status = GetLocalizedMessage("113193", new Object[] { integrationMessageTypeLongName }); // {0} - Send has completed.
                        comments = ReadOperationResult(iOperationResult, (CallerContext)iCallerContext);

                        operationResults = UpdateIntegrationItemStatus(_mdmObjectId, iIntegrationMessageHeader.MDMObjectTypeId,
                                                                       iConnectorProfile, status, comments, OperationResultType.Information, iCallerContext);

                        if (Constants.TRACING_ENABLED)
                        {
                            MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("End - Calling IConnector.Send for IntegrationMessage.Id = {0}", iIntegrationMessage.Id), MDMTraceSource.Integration);
                        }
                    }
                }
            }
            finally
            {
                if (Constants.PERFORMANCE_TRACING_ENABLED || Constants.TRACING_ENABLED)
                {
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("{0}ms - Overall time to orchestrate outbound message for {1} connector and MDMObjectId : {2}.", _overallDurationHelper.GetDurationInMilliseconds(DateTime.Now), iConnectorProfile.Name, _mdmObjectId), MDMTraceSource.Integration);
                    MDMTraceHelper.StopTraceActivity(_orchestrateOutboundMessageMethodName, MDMTraceSource.Integration);
                }
            }
            return iOperationResult;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="mdmObjectId"></param>
        /// <param name="mdmObjectTypeId"></param>
        /// <param name="iConnectorProfile"></param>
        /// <param name="status"></param>
        /// <param name="comments"></param>
        /// <param name="statusType"></param>
        /// <param name="iCallerContext"></param>
        /// <returns></returns>
        private OperationResultCollection UpdateIntegrationItemStatus(Int64 mdmObjectId, Int16 mdmObjectTypeId, IConnectorProfile iConnectorProfile, String status, String comments, OperationResultType statusType, ICallerContext iCallerContext)
        {
            OperationResultCollection OperationResults = new OperationResultCollection();
            IntegrationItemStatusBL integrationItemStatusManager = new IntegrationItemStatusBL();
            IntegrationItemStatusInternal integrationItemStatusInternal = new IntegrationItemStatusInternal();

            integrationItemStatusInternal.MDMObjectId = mdmObjectId;
            integrationItemStatusInternal.MDMObjectTypeId = mdmObjectTypeId;

            if (iConnectorProfile != null)
            {
                integrationItemStatusInternal.ConnectorId = iConnectorProfile.Id;
                integrationItemStatusInternal.ConnectorName = iConnectorProfile.Name;
                integrationItemStatusInternal.ConnectorLongName = iConnectorProfile.LongName;
            }

            integrationItemStatusInternal.Status = status;
            integrationItemStatusInternal.Comments = comments;
            integrationItemStatusInternal.StatusType = statusType;
            integrationItemStatusInternal.IsExternalStatus = false;

            return integrationItemStatusManager.Process(new IntegrationItemStatusInternalCollection() { integrationItemStatusInternal }, (CallerContext)iCallerContext);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="iOperationResult"></param>
        /// <param name="callerContext"></param>
        /// <returns></returns>
        private String ReadOperationResult(IOperationResult iOperationResult, CallerContext callerContext)
        {
            String messages = String.Empty;
            String message = String.Empty;

            if (iOperationResult != null)
            {
                if (iOperationResult.OperationResultStatus == OperationResultStatusEnum.Failed || iOperationResult.OperationResultStatus == OperationResultStatusEnum.CompletedWithErrors)
                {
                    #region Read Error Messages

                    Collection<IError> errors = iOperationResult.GetErrors();

                    if (errors != null && errors.Count > 0)
                    {
                        messages = String.Concat(GetLocalizedMessage("110626", null), "<br/>");

                        foreach (Error error in errors)
                        {
                            if (!String.IsNullOrWhiteSpace(error.ErrorCode))
                            {
                                //If error contains params then concatenate params to the error code Message
                                //Otherwise just consider error code Message
                                if (error.Params != null && error.Params.Count > 0)
                                {
                                    message = GetLocalizedMessage(error.ErrorCode, ValueTypeHelper.ConvertObjectCollectionToArray(error.Params));
                                }
                                else
                                {
                                    message = GetLocalizedMessage(error.ErrorCode, null);
                                }
                            }
                            else
                            {
                                message = error.ErrorMessage;
                            }

                            messages = String.Concat(messages, " - ", message, "<br/>");
                        }
                    }

                    #endregion
                }
                else if (iOperationResult.OperationResultStatus == OperationResultStatusEnum.CompletedWithWarnings)
                {
                    #region Populate Warning Messages

                    IWarningCollection warnings = iOperationResult.GetWarnings();

                    if (warnings != null && warnings.Count > 0)
                    {
                        messages = String.Concat(GetLocalizedMessage("111720", null), " : ", "<br/>");

                        foreach (Warning warning in warnings)
                        {
                            if (!String.IsNullOrWhiteSpace(warning.WarningCode))
                            {
                                //If warning contains params then concatenate params to the warning code Message
                                //Otherwise just consider warning code Message
                                if (warning.Params != null && warning.Params.Count > 0)
                                {
                                    message = GetLocalizedMessage(warning.WarningCode, ValueTypeHelper.ConvertObjectCollectionToArray(warning.Params));
                                }
                                else
                                {
                                    message = GetLocalizedMessage(warning.WarningCode, null);
                                }
                            }
                            else
                            {
                                message = warning.WarningMessage;
                            }

                            messages = String.Concat(messages, " - ", message, "<br/>");
                        }
                    }

                    #endregion
                }
                else if (iOperationResult.OperationResultStatus == OperationResultStatusEnum.None || iOperationResult.OperationResultStatus == OperationResultStatusEnum.Successful)
                {
                    #region Populate Information Messages

                    Collection<IInformation> informations = iOperationResult.GetInformation();

                    if (informations != null && informations.Count > 0)
                    {
                        messages = String.Concat(GetLocalizedMessage("111707", null), " : ", "<br/>");

                        foreach (Information information in informations)
                        {
                            if (!String.IsNullOrWhiteSpace(information.InformationCode))
                            {
                                //If information contains params then concatenate params to the information code Message
                                //Otherwise just consider information code Message
                                if (information.Params != null && information.Params.Count > 0)
                                {
                                    message = GetLocalizedMessage(information.InformationCode, ValueTypeHelper.ConvertObjectCollectionToArray(information.Params));
                                }
                                else
                                {
                                    message = GetLocalizedMessage(information.InformationCode, null);
                                }
                            }
                            else
                            {
                                message = information.InformationMessage;
                            }

                            messages = String.Concat(messages, " - ", message, "<br/>");
                        }
                    }

                    #endregion
                }
            }

            return messages;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="messageCode"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        private String GetLocalizedMessage(String messageCode, Object[] parameters)
        {
            LocaleMessage localeMessage = null;

            if (_callerContext == null)
            {
                _callerContext = new CallerContext(MDMCenterApplication.PIM, MDMCenterModules.Integration);
            }

            if (parameters != null && parameters.Length > 0)
            {
                localeMessage = _localeMessageBL.Get(_systemUILocale, messageCode, parameters, false, _callerContext);
            }
            else
            {
                localeMessage = _localeMessageBL.Get(_systemUILocale, messageCode, false, _callerContext);
            }

            return (localeMessage != null) ? localeMessage.Message : messageCode;
        }

        #endregion Private methods

        #endregion
    }
}