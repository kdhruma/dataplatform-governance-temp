using System;


namespace MDM.Integration.Events
{
    using MDM.Interfaces;

    /// <summary>
    /// Specifies arguments for events raise by Integration Framework
    /// </summary>
    /// <seealso cref="System.EventArgs" />
    public class IntegrationEventArgs: EventArgs
    {
        #region Properties

        /// <summary>
        /// Gets the connector profile.
        /// </summary>
        /// <value>
        /// The connector profile.
        /// </value>
        public IConnectorProfile ConnectorProfile { get; private set; }

        /// <summary>
        /// Gets the caller context.
        /// </summary>
        /// <value>
        /// The caller context.
        /// </value>
        public ICallerContext CallerContext { get; private set; }

        /// <summary>
        /// Gets the operation result.
        /// </summary>
        /// <value>
        /// The operation result.
        /// </value>
        public IOperationResult OperationResult { get; private set; }

        /// <summary>
        /// Gets the integration activity.
        /// </summary>
        /// <value>
        /// The integration activity.
        /// </value>
        public IIntegrationActivity IntegrationActivity { get; private set; }

        /// <summary>
        /// Gets the integration message.
        /// </summary>
        /// <value>
        /// The integration message.
        /// </value>
        public IIntegrationMessage IntegrationMessage { get; private set; }

        /// <summary>
        /// Gets the integration data.
        /// </summary>
        /// <value>
        /// The integration data.
        /// </value>
        public IIntegrationData IntegrationData { get; private set; }

        /// <summary>
        /// Gets the integration message collection results.
        /// </summary>
        /// <value>
        /// The integration message collection results.
        /// </value>
        public IIntegrationMessageCollection IntegrationMessageCollectionResults { get; private set; }

        /// <summary>
        /// Gets the messages to be aggregated.
        /// </summary>
        /// <value>
        /// The messages to be aggregated.
        /// </value>
        public IIntegrationMessageCollection MessagesToBeAggregated { get; private set; }

        /// <summary>
        /// Gets the message qualification result.
        /// </summary>
        /// <value>
        /// The message qualification result.
        /// </value>
        public IMessageQualificationResult MessageQualificationResult { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="IntegrationEventArgs"/> class.
        /// </summary>
        /// <param name="connectorProfile">The connector profile.</param>
        /// <param name="callerContext">The caller context.</param>
        public IntegrationEventArgs(IConnectorProfile connectorProfile, ICallerContext callerContext)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IntegrationEventArgs"/> class.
        /// </summary>
        /// <param name="integrationMessage">The integration message.</param>
        /// <param name="connectorProfile">The connector profile.</param>
        /// <param name="callerContext">The caller context.</param>
        public IntegrationEventArgs(IIntegrationMessage integrationMessage, IConnectorProfile connectorProfile, ICallerContext callerContext)
            : this (connectorProfile, callerContext)
        {
            this.IntegrationMessage = integrationMessage;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IntegrationEventArgs"/> class.
        /// </summary>
        /// <param name="iIntegrationActivity">The i integration activity.</param>
        /// <param name="connectorProfile">The connector profile.</param>
        /// <param name="callerContext">The caller context.</param>
        public IntegrationEventArgs(IIntegrationActivity iIntegrationActivity, IConnectorProfile connectorProfile, ICallerContext callerContext)
            : this(connectorProfile, callerContext)
        {
            this.IntegrationActivity = IntegrationActivity;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IntegrationEventArgs"/> class.
        /// </summary>
        /// <param name="integrationData"></param>
        /// <param name="integrationMessage"></param>
        /// <param name="connectorProfile"></param>
        /// <param name="callerContext"></param>
        public IntegrationEventArgs(IIntegrationData integrationData, IIntegrationMessage integrationMessage, IConnectorProfile connectorProfile, ICallerContext callerContext)
            : this(integrationMessage, connectorProfile, callerContext)
        {
            this.IntegrationData = integrationData;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="IntegrationEventArgs"/> class.
        /// </summary>
        /// <param name="integrationMessageCollection"></param>
        /// <param name="iIntegrationActivity"></param>
        /// <param name="connectorProfile"></param>
        /// <param name="callerContext"></param>
        public IntegrationEventArgs(IIntegrationMessageCollection integrationMessageCollection, IIntegrationActivity iIntegrationActivity, IConnectorProfile connectorProfile, ICallerContext callerContext)
            : this(iIntegrationActivity, connectorProfile, callerContext)
        {
            this.IntegrationMessageCollectionResults = integrationMessageCollection;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IntegrationEventArgs"/> class.
        /// </summary>
        /// <param name="integrationMessageCollection">The integration message collection.</param>
        /// <param name="operationResult">The operation result.</param>
        /// <param name="iIntegrationActivity">The i integration activity.</param>
        /// <param name="connectorProfile">The connector profile.</param>
        /// <param name="callerContext">The caller context.</param>
        public IntegrationEventArgs(IIntegrationMessageCollection integrationMessageCollection, IOperationResult operationResult, IIntegrationActivity iIntegrationActivity, IConnectorProfile connectorProfile, ICallerContext callerContext)
            : this(connectorProfile, callerContext)
        {
            this.IntegrationMessageCollectionResults = integrationMessageCollection;
            this.OperationResult = operationResult;
            this.IntegrationActivity = IntegrationActivity;
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="IntegrationEventArgs"/> class.
        /// </summary>
        /// <param name="messageQualificationResult">The message qualification result.</param>
        /// <param name="integrationMessage">The integration message.</param>
        /// <param name="connectorProfile">The connector profile.</param>
        /// <param name="callerContext">The caller context.</param>
        public IntegrationEventArgs(IMessageQualificationResult messageQualificationResult, IIntegrationMessage integrationMessage, IConnectorProfile connectorProfile, ICallerContext callerContext)
            :this (integrationMessage, connectorProfile, callerContext)
        {
            this.MessageQualificationResult = messageQualificationResult;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IntegrationEventArgs"/> class.
        /// </summary>
        /// <param name="integrationData">The integration data.</param>
        /// <param name="connectorProfile">The connector profile.</param>
        /// <param name="callerContext">The caller context.</param>
        public IntegrationEventArgs(IIntegrationData integrationData, IConnectorProfile connectorProfile, ICallerContext callerContext)
            : this(connectorProfile, callerContext)
        {
            this.IntegrationData = integrationData;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IntegrationEventArgs"/> class.
        /// </summary>
        /// <param name="operationResult">The operation result.</param>
        /// <param name="integrationData">The integration data.</param>
        /// <param name="connectorProfile">The connector profile.</param>
        /// <param name="callerContext">The caller context.</param>
        public IntegrationEventArgs(IOperationResult operationResult, IIntegrationData integrationData, IConnectorProfile connectorProfile, ICallerContext callerContext)
            : this (integrationData, connectorProfile, callerContext)
        {
            this.OperationResult = operationResult;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IntegrationEventArgs"/> class.
        /// </summary>
        /// <param name="operationResult">The operation result.</param>
        /// <param name="integrationMessage">The integration message.</param>
        /// <param name="connectorProfile">The connector profile.</param>
        /// <param name="callerContext">The caller context.</param>
        public IntegrationEventArgs(IOperationResult operationResult, IIntegrationMessage integrationMessage, IConnectorProfile connectorProfile, ICallerContext callerContext)
            : this (integrationMessage, connectorProfile, callerContext)
        {
            this.OperationResult = operationResult;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IntegrationEventArgs"/> class.
        /// </summary>
        /// <param name="messagesToBeAggregated"></param>
        /// <param name="connectorProfile"></param>
        /// <param name="callerContext"></param>
        public IntegrationEventArgs(IIntegrationMessageCollection messagesToBeAggregated, IConnectorProfile connectorProfile, ICallerContext callerContext)
            : this(connectorProfile, callerContext)
        {
            this.MessagesToBeAggregated = messagesToBeAggregated;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IntegrationEventArgs"/> class.
        /// </summary>
        /// <param name="integrationMessageCollection">The integration message collection.</param>
        /// <param name="messagesToBeAggregated">The messages to be aggregated.</param>
        /// <param name="connectorProfile">The connector profile.</param>
        /// <param name="callerContext">The caller context.</param>
        public IntegrationEventArgs(IIntegrationMessageCollection integrationMessageCollection, IIntegrationMessageCollection messagesToBeAggregated, IConnectorProfile connectorProfile, ICallerContext callerContext)
            : this (messagesToBeAggregated, connectorProfile, callerContext)
        {
            this.IntegrationMessageCollectionResults = integrationMessageCollection;
        }

        #endregion
    }
}