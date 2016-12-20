using System;
using System.Xml.Serialization;

namespace MDM.Interfaces
{
    /// <summary>
    /// Specifies interface for MessageBrokerOptions
    /// </summary>
    public interface IMessageBrokerOptions
    {
        /// <summary>
        /// Topic/queue name
        /// </summary>
        String TopicName { get; set; }
    }
}