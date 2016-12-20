using System;
using System.Collections.Generic;
using System.Linq;

namespace MDM.MessageBrokerManager
{
    using MDM.BusinessObjects;
    using MDM.BusinessObjects.Integration;
    using MDM.Core;
    using MDM.IntegrationManager.Business;
    using MDM.JigsawIntegrationManager;

    /// <summary>
    /// Represents a factory for constructing MessageBrokerManagers
    /// </summary>
    public class MessageBrokerFactory
    {
        /// <summary>
        /// main broker object
        /// </summary>
        private static FileFolderIntegrationBroker _fileFolderBroker;
        private static NullMessageBroker _nullMessageBroker;
        private static JigsawIntegrationConfiguration _jigsawConfiguration;

        private static IReadOnlyDictionary<JigsawCallerProcessType, String> _topicKeysDictionary =
            new Dictionary<JigsawCallerProcessType, String>
            {
                {JigsawCallerProcessType.DataQualityMessage, "DataQualityMessageQueue"},
                {JigsawCallerProcessType.WorkflowEvent, "DataQualityEventQueue" },
                {JigsawCallerProcessType.ExportEvent, "DataQualityEventQueue" },
                {JigsawCallerProcessType.PromoteEvent, "DataQualityEventQueue" },
                {JigsawCallerProcessType.AppConfigManageMessage, "AppConfigManageMessageQueue"}
            };

        private static IDictionary<JigsawCallerProcessType, IIntegrationMessageBroker> _messageBrokerManagersDictionary = new Dictionary<JigsawCallerProcessType, IIntegrationMessageBroker>(); 

        /// <summary>
        /// The _lock object
        /// </summary>
        private static object _lockObject = new object();

        /// <summary>
        /// The _configuration lock object
        /// </summary>
        private static object _configurationLockObject = new object();


        /// <summary>
        /// Gets the message broker manager.
        /// </summary>
        /// <param name="callProcessType">Type of the call process.</param>
        /// <param name="brokerType">Type of the broker.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">JigsawIntegrationConfiguration;GetJigsawIntegrationConfiguration was not initialized from corresponding ConnectorProfile</exception>
        public static IIntegrationMessageBroker GetMessageBrokerManager(JigsawCallerProcessType callProcessType = JigsawCallerProcessType.DataQualityMessage, JigsawIntegrationBrokerType brokerType = JigsawIntegrationBrokerType.NullMessageBroker)
        {
            if (GetJigsawIntegrationConfiguration() == null)
            {
                throw new ArgumentNullException("JigsawIntegrationConfiguration", "GetJigsawIntegrationConfiguration was not initialized from corresponding ConnectorProfile");
            }

            if (brokerType == JigsawIntegrationBrokerType.CSharpClientKafkaNetBroker)
            {
                MessageBrokerOptions messageBrokerOptions = new MessageBrokerOptions
                {
                    KafkaNodes = GetJigsawIntegrationConfiguration().KafkaConfiguration,
                    ZooKeeperNodes = GetJigsawIntegrationConfiguration().ZookeeperConfiguration,
                };

                if (!_messageBrokerManagersDictionary.ContainsKey(callProcessType))
                {
                    lock (_lockObject)
                    {
                        if (!_messageBrokerManagersDictionary.ContainsKey(callProcessType))
                        {
                            messageBrokerOptions.TopicName = GetTopicName(_topicKeysDictionary[callProcessType]);
                            _messageBrokerManagersDictionary.Add(callProcessType, new CSharpClientKafkaNetBroker(messageBrokerOptions));
                        }
                    }
                }

                return _messageBrokerManagersDictionary[callProcessType];
            } else if (brokerType == JigsawIntegrationBrokerType.KafkaDashNetBroker)
            {
                MessageBrokerOptions messageBrokerOptions = new MessageBrokerOptions
                {
                    KafkaNodes = GetJigsawIntegrationConfiguration().KafkaConfiguration,
                    ZooKeeperNodes = GetJigsawIntegrationConfiguration().ZookeeperConfiguration,
                };

                if (!_messageBrokerManagersDictionary.ContainsKey(callProcessType))
                {
                    lock (_lockObject)
                    {
                        if (!_messageBrokerManagersDictionary.ContainsKey(callProcessType))
                        {
                            messageBrokerOptions.TopicName = GetTopicName(_topicKeysDictionary[callProcessType]);
                            _messageBrokerManagersDictionary.Add(callProcessType, new KafkaDashNetBroker(messageBrokerOptions));
                        }
                    }
                }

                return _messageBrokerManagersDictionary[callProcessType];
            }
            else if(brokerType == JigsawIntegrationBrokerType.FileFolderBroker)
            {
                return GetFileFolderIntegrationBroker();
            }
            else
            {
                return GetNullMessageBroker();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static FileFolderIntegrationBroker GetFileFolderIntegrationBroker()
        {
            if (_fileFolderBroker == null)
            {
                lock (_lockObject)
                {
                    _fileFolderBroker = new FileFolderIntegrationBroker(JigsawConstants.IntegrationMessageFolder);
                }
            }

            return _fileFolderBroker;
        }


        /// <summary>
        /// Gets the null message broker.
        /// </summary>
        /// <returns></returns>
        public static NullMessageBroker GetNullMessageBroker()
        {
            if (_nullMessageBroker == null)
            {
                lock (_lockObject)
                {
                    if (_nullMessageBroker == null)
                    {
                        _nullMessageBroker = new NullMessageBroker();
                    }
                }
            }

            return _nullMessageBroker;
        }

        private static String GetTopicName(String topicKey)
        {
            JigsawTopic jigsawTopic = !String.IsNullOrEmpty(topicKey)
                ? GetJigsawIntegrationConfiguration().TopicConfiguration.FirstOrDefault(topic => topic.Key == topicKey)
                : GetJigsawIntegrationConfiguration().TopicConfiguration.FirstOrDefault();

            return jigsawTopic == null ? String.Empty : jigsawTopic.Topic;
        }


        private static JigsawIntegrationConfiguration GetJigsawIntegrationConfiguration()
        {
            if (_jigsawConfiguration == null)
            {
                lock (_configurationLockObject)
                {
                    ConnectorProfile connectorProfile = new ConnectorProfileBL().GetByName(JigsawConstants.JigsawIntegrationConnectorName, new CallerContext());

                    if (connectorProfile != null && connectorProfile.JigsawIntegrationConfiguration != null)
                    {
                        _jigsawConfiguration = connectorProfile.JigsawIntegrationConfiguration;
                    }
                }
            }

            return _jigsawConfiguration;
        }

        
    }
}
