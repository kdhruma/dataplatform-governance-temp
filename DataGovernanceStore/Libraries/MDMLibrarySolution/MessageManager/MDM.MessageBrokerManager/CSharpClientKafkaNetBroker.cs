using System;
using System.Collections.Generic;
using System.Text;
using Kafka.Client.Cfg;
using Kafka.Client.Consumers;
using Kafka.Client.Helper;
using Kafka.Client.Messages;
using Kafka.Client.Producers;
using Kafka.Client.Requests;

namespace MDM.MessageBrokerManager
{
    using MDM.BusinessObjects.Diagnostics;
    using MDM.Core;
    using MDM.Utility;
    using MDM.BusinessObjects.Integration;
    using MDM.Core.Extensions;
    using MDM.Interfaces;

    /// <summary>
    /// Represents CSharpClientKafkaNetBroker class for sending and receiving messages from Kafka
    /// </summary>
    public class CSharpClientKafkaNetBroker : IIntegrationMessageBroker
    {
        private MessageBrokerOptions _messageBrokerOptions = null;
        private TraceSettings _currentTraceSettings = MDMOperationContextHelper.GetCurrentTraceSettings();
        private ProducerConfiguration _config = null;
        private Producer _kafkaProducer = null;

        public MessageBrokerOptions MessageBrokerOptions
        {
            get { return _messageBrokerOptions; }
            set { _messageBrokerOptions = value; }
        }

        /// <summary>
        /// Constructs a new instance of CSharpClientKafkaNetBroker
        /// </summary>
        /// <param name="options"></param>
        public CSharpClientKafkaNetBroker(MessageBrokerOptions options)
        {
            MessageBrokerOptions = options;

            List<BrokerConfiguration> brokerConfigurations = new List<BrokerConfiguration>();

            foreach (JigsawNode jigsawNode in MessageBrokerOptions.KafkaNodes)
            {
                brokerConfigurations.Add(new BrokerConfiguration()
                {
                    Host = jigsawNode.HostName,
                    Port = jigsawNode.Port
                });
            }

            _config = new ProducerConfiguration(brokerConfigurations);
            _config.ClientId = "CSharpKafkaClient";

            String zooKeeperConnection = GetZooKeeperConnection();

            if (!String.IsNullOrEmpty(zooKeeperConnection))
            {
                _config.ZooKeeper = new ZooKeeperConfiguration
                {
                    ZkConnect = zooKeeperConnection
                };
            }

            _kafkaProducer = new Producer(_config);
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
                    _kafkaProducer.Send(GetProducerData(MessageBrokerOptions.TopicName, messageKey, message));
                }
                else
                {
                    throw new ApplicationException("CSharpClientKafkaNetBroker Producer Not available or not initialized. Cannot send messages");
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
            DiagnosticActivity diagnosticActivity = new DiagnosticActivity();
            if (_currentTraceSettings.IsBasicTracingEnabled)
            { 
                diagnosticActivity.Start();
            }

            try
            {
                String consumerId = "";
                var managerConfig = new KafkaSimpleManagerConfiguration
                {
                    FetchSize = 100,
                    BufferSize = 1000,
                    //Zookeeper = String.Format("{0}:{1}", MessageBrokerOptions.HostNames, MessageBrokerOptions.Port)
                };
                var partitionId = 0;

                var m_consumerManager = new KafkaSimpleManager<int, Message>(managerConfig);
                var allPartitions = m_consumerManager.GetTopicPartitionsFromZK(MessageBrokerOptions.TopicName);
                m_consumerManager.RefreshMetadata(0, consumerId, 0, MessageBrokerOptions.TopicName, true);
                var partitionConsumer = m_consumerManager.GetConsumer(MessageBrokerOptions.TopicName, partitionId);

                var request = ConstructRequest();
                var result = partitionConsumer.Fetch(request);
                var messages = result.MessageSet(MessageBrokerOptions.TopicName, partitionId).Messages;

                return "";
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
        /// 
        /// </summary>
        /// <param name="topicName"></param>
        /// <param name="messageKey"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        private ProducerData<String, Message> GetProducerData(String topicName, String messageKey, String message)
        {
            return new ProducerData<String, Message>(topicName, messageKey, new Message(Encoding.ASCII.GetBytes(message)));
        }

        private FetchRequest ConstructRequest()
        {
            Int32 correlationId = 0;
            String clientId = "";
            Int32 maxWait = 0;
            Int32 minBytes = 0;
            Dictionary<string, List<PartitionFetchInfo>> fetchInfos = new Dictionary<string, List<PartitionFetchInfo>>();

            return new FetchRequest(correlationId, clientId, maxWait, minBytes, fetchInfos);
        }
        
        private String GetZooKeeperConnection()
        {
            var sb = new StringBuilder();

            if (!MessageBrokerOptions.ZooKeeperNodes.IsNullOrEmpty())
            {
                foreach (JigsawNode node in MessageBrokerOptions.ZooKeeperNodes)
                {
                    sb.Append(node.HostName);
                    sb.Append(':');
                    sb.Append(node.Port);
                    sb.Append(',');
                }

                sb.Remove(sb.Length - 1, 1);
            }

            return sb.ToString();
        }
    }
}