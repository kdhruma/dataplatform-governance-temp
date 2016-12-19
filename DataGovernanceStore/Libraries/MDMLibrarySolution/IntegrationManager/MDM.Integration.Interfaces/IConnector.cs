using System;

namespace MDM.Integration.Interfaces
{
    using MDM.Interfaces;

    /// <summary>
    /// Connector interface. Defines any connector needs to implement.
    /// </summary>
    public interface IConnector
    {
        /// <summary>
        /// Initial setup for connector. This will be called only once when user is clicking on UI button for registering the connector.
        /// </summary>
        /// <param name="iConnectorProfile">Profile of connector to register</param>
        /// <param name="iCallerContext">Context of caller making call to this API.</param>
        /// <returns>Result of operation</returns>
        Boolean Register(IConnectorProfile iConnectorProfile, ICallerContext iCallerContext);

        /// <summary>
        /// Create messages to be processed for outgoing (MDM to External system) messages.
        /// Take the integration activity, and create respective messages to be processed for given activity.
        /// This method is called after an integration activity is recorded.
        /// </summary>
        /// <param name="iIntegrationActivity">Integration activity which was performed.</param>
        /// <param name="iConnectorProfile">Profile of connector for which messages are to be created.</param>
        /// <param name="iCallerContext">Context of caller making call to this API.</param>
        /// <returns>IntegrationMessages to be processed due to given integration activity</returns>
        IIntegrationMessageCollection GetOutboundMessages(IIntegrationActivity iIntegrationActivity, IConnectorProfile iConnectorProfile, ICallerContext iCallerContext);

        /// <summary>
        /// Create messages to be processed for incoming (External system to MDM) messages.
        /// Take the integration activity, and create respective messages to be processed for given activity.
        /// This method is called after an integration activity is recorded.
        /// </summary>
        /// <param name="iIntegrationActivity">Integration activity which was performed.</param>
        /// <param name="iConnectorProfile">Profile of connector for which messages are to be created.</param>
        /// <param name="iCallerContext">Context of caller making call to this API.</param>
        /// <returns>IntegrationMessages to be processed due to given integration activity</returns>
        IIntegrationMessageCollection GetInboundMessages(IIntegrationActivity iIntegrationActivity, IConnectorProfile iConnectorProfile, ICallerContext iCallerContext);

        /// <summary>
        /// Check if the message is qualified to be sent to outbound system.
        /// It should indicate if the message is qualified for further processing or it needs to be re-processed after given time (ScheduledqualificationTime)
        /// Should be called by both inbound and outbound messages to check if message is qualified or if scheduling is required.
        /// </summary>
        /// <param name="iIntegrationMessage">IIntegrationMessage to be qualified</param>
        /// <param name="iConnectorProfile">Profile of connector for which message is being qualified</param>
        /// <param name="iCallerContext">Context of caller making call to this API.</param>
        /// <returns><see cref="IMessageQualificationResult"/>Indicates if message is qualified. If not, what is the next scheduled time it should be re-processed.</returns>
        IMessageQualificationResult IsQualified(IIntegrationMessage iIntegrationMessage, IConnectorProfile iConnectorProfile, ICallerContext iCallerContext);

        /// <summary>
        /// Process the outgoing (MDM -> External System) message.
        /// This method will be called when connector profile is configured to use workflow. 
        /// After this point, connector is responsible to call other required methods (e.g.,  Send, Transform).
        /// </summary>
        /// <param name="iIntegrationMessage">Message to be transformed which will be sent to external system.</param>
        /// <param name="iConnectorProfile">Profile of connector for which message is to be transformed.</param>
        /// <param name="iCallerContext">Context of caller making call to this API.</param>
        /// <returns><see cref="IIntegrationData"/> which will be sent to external system and is understood by that system</returns>
        IIntegrationData Transform(IIntegrationMessage iIntegrationMessage, IConnectorProfile iConnectorProfile, ICallerContext iCallerContext);

        /// <summary>
        /// Send message to external system. This send will happen for single message (which is transformed in <see cref="IIntegrationData"/>
        /// </summary>
        /// <param name="iIntegrationData">Transformed message - ready to be sent to external system</param>
        /// <param name="iConnectorProfile">Profile of connector for which message is to be sent</param>
        /// <param name="iCallerContext">Context of caller making call to this API.</param>
        /// <returns>Result of operation</returns>
        IOperationResult Send(IIntegrationData iIntegrationData, IConnectorProfile iConnectorProfile, ICallerContext iCallerContext);
        
        /// <summary>
        /// Indicates the received message from external system. This will be called when connector profile indicates not to use workflow.
        /// Should be called in case of inbound queue
        /// </summary>
        /// <param name="iIntegrationMessage">Indicates message received from external system.</param>
        /// <param name="iConnectorProfile">Profile for connector for which message is received.</param>
        /// <param name="iCallerContext">Context of caller making call to this API.</param>
        /// <returns>Result of operation</returns>
        IOperationResult Receive(IIntegrationMessage iIntegrationMessage, IConnectorProfile iConnectorProfile, ICallerContext iCallerContext);

        /// <summary>
        /// Process the outgoing (MDM -> External System) message.
        /// This method will be called when connector profile is configured to use workflow. 
        /// After this point, connector is responsible to call other required methods (e.g.,  Send, Transform).
        /// </summary>
        /// <param name="iIntegrationMessage">Message to be processed</param>
        /// <param name="iConnectorProfile">Profile of connector for which message is to be processed.</param>
        /// <param name="iCallerContext">Context of caller making call to this API.</param>
        /// <returns>Result of operation</returns>
        IOperationResult ProcessOutboundMessage(IIntegrationMessage iIntegrationMessage, IConnectorProfile iConnectorProfile, ICallerContext iCallerContext);

        /// <summary>
        /// Process the incoming (External System -> MDM ) message.
        /// This method will be called when connector profile is configured to use workflow. 
        /// After this point, connector is responsible to call other required methods (e.g.,  receive).
        /// </summary>
        /// <param name="iIntegrationMessage">Message to be processed</param>
        /// <param name="iConnectorProfile">Profile of connector for which message is to be processed.</param>
        /// <param name="iCallerContext">Context of caller making call to this API.</param>
        /// <returns>Result of operation</returns>
        IOperationResult ProcessInboundMessage(IIntegrationMessage iIntegrationMessage, IConnectorProfile iConnectorProfile, ICallerContext iCallerContext);

        /// <summary>
        /// Create aggregated messages to be processed for outgoing (MDM to External system) messages.
        /// Take the integration messages, and create aggregated messages to be processed.
        /// This method is called after a message is qualified.
        /// </summary>
        /// <param name="messagesToBeAggregated">The messages to be aggregated.</param>
        /// <param name="iConnectorProfile">Profile of connector for which message is to be processed.</param>
        /// <param name="iCallerContext">Context of caller making call to this API.</param>
        /// <returns>
        /// Aggregated IntegrationMessages to be processed
        /// </returns>
        IIntegrationMessageCollection GetOutboundAggregatedMessages(IIntegrationMessageCollection messagesToBeAggregated, IConnectorProfile iConnectorProfile, ICallerContext iCallerContext);
        
        /// <summary>
        /// Create aggregated messages to be processed for incoming (External system to MDM) messages.
        /// Take the integration messages, and create aggregated messages to be processed.
        /// This method is called after a message is qualified.
        /// </summary>
        /// <param name="messagesToBeAggregated">The messages to be aggregated.</param>
        /// <param name="iConnectorProfile">Profile of connector for which message is to be processed.</param>
        /// <param name="iCallerContext">Context of caller making call to this API.</param>
        /// <returns>
        /// Aggregated IntegrationMessages to be processed
        /// </returns>
        IIntegrationMessageCollection GetInboundAggregatedMessages(IIntegrationMessageCollection messagesToBeAggregated, IConnectorProfile iConnectorProfile, ICallerContext iCallerContext);
    }
}
