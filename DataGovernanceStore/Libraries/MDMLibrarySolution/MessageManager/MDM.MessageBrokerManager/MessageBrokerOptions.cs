using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace MDM.MessageBrokerManager
{
    using Interfaces;
    using MDM.BusinessObjects.Integration;

    /// <summary>
    /// Represent class for storing MessageBroker options
    /// </summary>
    public class MessageBrokerOptions : IMessageBrokerOptions
    {
        /// <summary>
        /// Gets or sets the zoo keeper connection.
        /// </summary>
        public Collection<JigsawNode> ZooKeeperNodes { get; set; }

        /// <summary>
        /// Gets or sets the kafka nodes.
        /// </summary>
        public Collection<JigsawNode> KafkaNodes { get; set; }

        /// <summary>
        /// Gets or sets the name of the topic.
        /// </summary>
        public String TopicName { get; set; }
    }
}