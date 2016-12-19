using System;

namespace MDM.Integration.Events
{
    using MDM.Core;
    /// <summary>
    /// Integration Event Manager
    /// </summary>
    public sealed class IntegrationEventManager
    {
        /// <summary>
        /// Prevents a default instance of the <see cref="IntegrationEventManager"/> class from being created.
        /// </summary>
        private IntegrationEventManager()
        {
        }

        /// <summary>
        /// The instance of IntegrationEventManager
        /// </summary>
        public static readonly IntegrationEventManager Instance = new IntegrationEventManager();

        /// <summary>
        /// The integration message is qualified starting
        /// </summary>
        public EventHandler<IntegrationEventArgs> IntegrationMessageIsQualifiedStarting;
        
        /// <summary>
        /// The integration message is qualified completed
        /// </summary>
        public EventHandler<IntegrationEventArgs> IntegrationMessageIsQualifiedCompleted;

        /// <summary>
        /// The integration message process outbound message starting
        /// </summary>
        public EventHandler<IntegrationEventArgs> IntegrationMessageProcessOutboundMessageStarting;

        /// <summary>
        /// The integration message process outbound message completed
        /// </summary>
        public EventHandler<IntegrationEventArgs> IntegrationMessageProcessOutboundMessageCompleted;

        /// <summary>
        /// The integration message process inbound message starting
        /// </summary>
        public EventHandler<IntegrationEventArgs> IntegrationMessageProcessInboundMessageStarting;

        /// <summary>
        /// The integration message process inbound message completed
        /// </summary>
        public EventHandler<IntegrationEventArgs> IntegrationMessageProcessInboundMessageCompleted;

        /// <summary>
        /// The integration message transform starting
        /// </summary>
        public EventHandler<IntegrationEventArgs> IntegrationMessageTransformStarting;

        /// <summary>
        /// The integration message transform completed
        /// </summary>
        public EventHandler<IntegrationEventArgs> IntegrationMessageTransformCompleted;

        /// <summary>
        /// The integration message send starting
        /// </summary>
        public EventHandler<IntegrationEventArgs> IntegrationMessageSendStarting;

        /// <summary>
        /// The integration message send completed
        /// </summary>
        public EventHandler<IntegrationEventArgs> IntegrationMessageSendCompleted;

        /// <summary>
        /// The integration message receive starting
        /// </summary>
        public EventHandler<IntegrationEventArgs> IntegrationMessageReceiveStarting;

        /// <summary>
        /// The integration message receive completed
        /// </summary>
        public EventHandler<IntegrationEventArgs> IntegrationMessageReceiveCompleted;

        /// <summary>
        /// The integration message outbound messages loading
        /// </summary>
        public EventHandler<IntegrationEventArgs> IntegrationMessageOutboundMessagesLoading;

        /// <summary>
        /// The integration message outbound messages loaded
        /// </summary>
        public EventHandler<IntegrationEventArgs> IntegrationMessageOutboundMessagesLoaded;

        /// <summary>
        /// The integration message outbound aggregated messages loading
        /// </summary>
        public EventHandler<IntegrationEventArgs> IntegrationMessageOutboundAggregatedMessagesLoading;

        /// <summary>
        /// The integration message outbound aggregated messages loaded
        /// </summary>
        public EventHandler<IntegrationEventArgs> IntegrationMessageOutboundAggregatedMessagesLoaded;

        /// <summary>
        /// The integration message inbound messages loading
        /// </summary>
        public EventHandler<IntegrationEventArgs> IntegrationMessageInboundMessagesLoading;

        /// <summary>
        /// The integration message inbound messages loaded
        /// </summary>
        public EventHandler<IntegrationEventArgs> IntegrationMessageInboundMessagesLoaded;

        /// <summary>
        /// The integration message inbound aggregated messages loading
        /// </summary>
        public EventHandler<IntegrationEventArgs> IntegrationMessageInboundAggregatedMessagesLoading;

        /// <summary>
        /// The integration message inbound aggregated messages loaded
        /// </summary>
        public EventHandler<IntegrationEventArgs> IntegrationMessageInboundAggregatedMessagesLoaded;

        /// <summary>
        /// Raises the <see cref="E:IntegrationMessageIsQualifiedStarting" /> event.
        /// </summary>
        /// <param name="e">The <see cref="IntegrationEventArgs" /> instance containing the event data.</param>
        public void OnIntegrationMessageIsQualifiedStarting(IntegrationEventArgs e)
        {
            IntegrationMessageIsQualifiedStarting.SafeInvoke(this, e);
        }

        /// <summary>
        /// Raises the <see cref="E:IntegrationMessageIsQualifiedCompleted" /> event.
        /// </summary>
        /// <param name="e">The <see cref="IntegrationEventArgs"/> instance containing the event data.</param>
        public void OnIntegrationMessageIsQualifiedCompleted(IntegrationEventArgs e)
        {
            IntegrationMessageIsQualifiedCompleted.SafeInvoke(this, e);
        }

        /// <summary>
        /// Raises the <see cref="E:IntegrationMessageProcessOutboundMessageStarting" /> event.
        /// </summary>
        /// <param name="e">The <see cref="IntegrationEventArgs"/> instance containing the event data.</param>
        public void OnIntegrationMessageProcessOutboundMessageStarting(IntegrationEventArgs e)
        {
            IntegrationMessageProcessOutboundMessageStarting.SafeInvoke(this, e);
        }

        /// <summary>
        /// Raises the <see cref="E:IntegrationMessageProcessOutboundMessageCompleted" /> event.
        /// </summary>
        /// <param name="e">The <see cref="IntegrationEventArgs"/> instance containing the event data.</param>
        public void OnIntegrationMessageProcessOutboundMessageCompleted(IntegrationEventArgs e)
        {
            IntegrationMessageProcessOutboundMessageCompleted.SafeInvoke(this, e);
        }

        /// <summary>
        /// Raises the <see cref="E:IntegrationMessageProcessInboundMessageStarting" /> event.
        /// </summary>
        /// <param name="e">The <see cref="IntegrationEventArgs"/> instance containing the event data.</param>
        public void OnIntegrationMessageProcessInboundMessageStarting(IntegrationEventArgs e)
        {
            IntegrationMessageProcessInboundMessageStarting.SafeInvoke(this, e);
        }

        /// <summary>
        /// Raises the <see cref="E:IntegrationMessageProcessInboundMessageCompleted" /> event.
        /// </summary>
        /// <param name="e">The <see cref="IntegrationEventArgs"/> instance containing the event data.</param>
        public void OnIntegrationMessageProcessInboundMessageCompleted(IntegrationEventArgs e)
        {
            IntegrationMessageProcessInboundMessageCompleted.SafeInvoke(this, e);
        }

        /// <summary>
        /// Raises the <see cref="E:IntegrationMessageTransformStarting" /> event.
        /// </summary>
        /// <param name="e">The <see cref="IntegrationEventArgs"/> instance containing the event data.</param>
        public void OnIntegrationMessageTransformStarting(IntegrationEventArgs e)
        {
            IntegrationMessageTransformStarting.SafeInvoke(this, e);
        }

        /// <summary>
        /// Raises the <see cref="E:IntegrationMessageTransformCompleted" /> event.
        /// </summary>
        /// <param name="e">The <see cref="IntegrationEventArgs"/> instance containing the event data.</param>
        public void OnIntegrationMessageTransformCompleted(IntegrationEventArgs e)
        {
            IntegrationMessageTransformCompleted.SafeInvoke(this, e);
        }

        /// <summary>
        /// Raises the <see cref="E:IntegrationMessageSendStarting" /> event.
        /// </summary>
        /// <param name="e">The <see cref="IntegrationEventArgs"/> instance containing the event data.</param>
        public void OnIntegrationMessageSendStarting(IntegrationEventArgs e)
        {
            IntegrationMessageSendStarting.SafeInvoke(this, e);
        }

        /// <summary>
        /// Raises the <see cref="E:IntegrationMessageSendCompleted" /> event.
        /// </summary>
        /// <param name="e">The <see cref="IntegrationEventArgs"/> instance containing the event data.</param>
        public void OnIntegrationMessageSendCompleted(IntegrationEventArgs e)
        {
            IntegrationMessageSendCompleted.SafeInvoke(this, e);
        }

        /// <summary>
        /// Raises the <see cref="E:IntegrationMessageReceiveStarting" /> event.
        /// </summary>
        /// <param name="e">The <see cref="IntegrationEventArgs"/> instance containing the event data.</param>
        public void OnIntegrationMessageReceiveStarting(IntegrationEventArgs e)
        {
            IntegrationMessageReceiveStarting.SafeInvoke(this, e);
        }

        /// <summary>
        /// Raises the <see cref="E:IntegrationMessageReceiveCompleted" /> event.
        /// </summary>
        /// <param name="e">The <see cref="IntegrationEventArgs"/> instance containing the event data.</param>
        public void OnIntegrationMessageReceiveCompleted(IntegrationEventArgs e)
        {
            IntegrationMessageReceiveCompleted.SafeInvoke(this, e);
        }

        /// <summary>
        /// Raises the <see cref="E:IntegrationMessageOutboundMessagesLoading" /> event.
        /// </summary>
        /// <param name="e">The <see cref="IntegrationEventArgs"/> instance containing the event data.</param>
        public void OnIntegrationMessageOutboundMessagesLoading(IntegrationEventArgs e)
        {
            IntegrationMessageOutboundMessagesLoading.SafeInvoke(this, e);
        }

        /// <summary>
        /// Raises the <see cref="E:IntegrationMessageOutboundMessagesLoaded" /> event.
        /// </summary>
        /// <param name="e">The <see cref="IntegrationEventArgs"/> instance containing the event data.</param>
        public void OnIntegrationMessageOutboundMessagesLoaded(IntegrationEventArgs e)
        {
            IntegrationMessageOutboundMessagesLoaded.SafeInvoke(this, e);
        }

        /// <summary>
        /// Raises the <see cref="E:IntegrationMessageOutboundAggregatedMessagesLoading" /> event.
        /// </summary>
        /// <param name="e">The <see cref="IntegrationEventArgs"/> instance containing the event data.</param>
        public void OnIntegrationMessageOutboundAggregatedMessagesLoading(IntegrationEventArgs e)
        {
            IntegrationMessageOutboundAggregatedMessagesLoading.SafeInvoke(this, e);
        }

        /// <summary>
        /// Raises the <see cref="E:IntegrationMessageOutboundAggregatedMessagesLoaded" /> event.
        /// </summary>
        /// <param name="e">The <see cref="IntegrationEventArgs"/> instance containing the event data.</param>
        public void OnIntegrationMessageOutboundAggregatedMessagesLoaded(IntegrationEventArgs e)
        {
            IntegrationMessageOutboundAggregatedMessagesLoaded.SafeInvoke(this, e);
        }

        /// <summary>
        /// Raises the <see cref="E:IntegrationMessageInboundMessagesLoading" /> event.
        /// </summary>
        /// <param name="e">The <see cref="IntegrationEventArgs"/> instance containing the event data.</param>
        public void OnIntegrationMessageInboundMessagesLoading(IntegrationEventArgs e)
        {
            IntegrationMessageInboundMessagesLoading.SafeInvoke(this, e);
        }

        /// <summary>
        /// Raises the <see cref="E:IntegrationMessageInboundMessagesLoaded" /> event.
        /// </summary>
        /// <param name="e">The <see cref="IntegrationEventArgs"/> instance containing the event data.</param>
        public void OnIntegrationMessageInboundMessagesLoaded(IntegrationEventArgs e)
        {
            IntegrationMessageInboundMessagesLoaded.SafeInvoke(this, e);
        }

        /// <summary>
        /// Raises the <see cref="E:IntegrationMessageInboundAggregatedMessagesLoading" /> event.
        /// </summary>
        /// <param name="e">The <see cref="IntegrationEventArgs"/> instance containing the event data.</param>
        public void OnIntegrationMessageInboundAggregatedMessagesLoading(IntegrationEventArgs e)
        {
            IntegrationMessageInboundAggregatedMessagesLoading.SafeInvoke(this, e);
        }

        /// <summary>
        /// Raises the <see cref="E:IntegrationMessageInboundAggregatedMessagesLoaded" /> event.
        /// </summary>
        /// <param name="e">The <see cref="IntegrationEventArgs"/> instance containing the event data.</param>
        public void OnIntegrationMessageInboundAggregatedMessagesLoaded(IntegrationEventArgs e)
        {
            IntegrationMessageInboundAggregatedMessagesLoaded.SafeInvoke(this, e);
        }
    }
}
