using System;
using System.Collections.Generic;
using System.Text;


namespace MDM.MessageBrokerManager
{
    using MDM.BusinessObjects.Diagnostics;
    using MDM.Core;
    using MDM.Utility;
    using MDM.BusinessObjects.Integration;
    using MDM.Core.Extensions;
    using KafkaNet;
    using KafkaNet.Model;
    using KafkaNet.Protocol;

    using MDM.Interfaces;

    /// <summary>
    /// Represents KafkaDashNetBroker class for sending and receiving messages from Kafka
    /// </summary>
    public class KafkaDashNetBroker : IIntegrationMessageBroker
    {
        private MessageBrokerOptions _messageBrokerOptions = null;
        private TraceSettings _currentTraceSettings = MDMOperationContextHelper.GetCurrentTraceSettings();
        KafkaOptions _kafkaOptions;
        Producer _kafkaProducer;

        public MessageBrokerOptions MessageBrokerOptions
        {
            get { return _messageBrokerOptions; }
            set { _messageBrokerOptions = value; }
        }

        /// <summary>
        /// Constructs a new instance of CSharpClientKafkaNetBroker
        /// </summary>
        /// <param name="options"></param>
        public KafkaDashNetBroker(MessageBrokerOptions options)
        {
            MessageBrokerOptions = options;

            List<Uri> kafkaUris = new List<Uri>();

            foreach (JigsawNode jigsawNode in MessageBrokerOptions.KafkaNodes)
            {
                kafkaUris.Add(new Uri(String.Format("http://{0}:{1}", jigsawNode.HostName, jigsawNode.Port)));
            }

            _kafkaOptions = new KafkaOptions(kafkaUris.ToArray());

            _kafkaProducer = new Producer(new BrokerRouter(_kafkaOptions))
            {
                BatchSize = 1,
                BatchDelayTime = TimeSpan.FromMilliseconds(1000)
            };

        }

        /// <summary>
        /// Sends the entities.
        /// </summary>
        /// <param name="entityMsgs">The entity messages.</param>
        public void SendEntities(Dictionary<String, String> entityMsgs)
        {
            foreach (var entityMsg in entityMsgs)
            {
                SendEntity(entityMsg.Key, entityMsg.Value);
            }
        }

        /// <summary>
        /// Sends given message
        /// </summary>
        /// <param name="message">Represents message being sent</param>
        public void SendEntity(String messageKey, String message)
        {
            DiagnosticActivity diagnosticActivity = new DiagnosticActivity();

            if (_currentTraceSettings.IsBasicTracingEnabled)
            {
                diagnosticActivity.Start();
            }

            try
            {
                if (_kafkaProducer != null)
                {
                    _kafkaProducer.SendMessageAsync(MessageBrokerOptions.TopicName, new[] { new Message(message, messageKey) });
                }
                else
                {
                    throw new ApplicationException("KafkaDashNetBroker Producer Not available or not initialized. Cannot send messages");
                }

            }
            finally
            {
                if (_currentTraceSettings.IsBasicTracingEnabled)
                {
                    diagnosticActivity.Stop();
                }
            }
        }

        /// <summary>
        /// Receives Entity Operation
        /// TODO: configure and test consumer properly
        /// </summary>
        /// <returns>Returns received message</returns>
        public String ReceiveEntityOperation()
        {
            throw new NotImplementedException();
        }
    }
}