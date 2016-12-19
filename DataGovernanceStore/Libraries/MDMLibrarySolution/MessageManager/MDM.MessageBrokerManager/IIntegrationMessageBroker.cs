using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDM.MessageBrokerManager
{
    /// <summary>
    /// Represents CSharpClientKafkaNetBroker class for sending and receiving messages from Kafka
    /// </summary>
    public interface IIntegrationMessageBroker
    {
        /// <summary>
        /// Options
        /// </summary>
        MessageBrokerOptions MessageBrokerOptions { get; set; }

        /// <summary>
        /// Sends the entities.
        /// </summary>
        /// <param name="entityMsgs">The entity messages.</param>
        void SendEntities(Dictionary<String, String> entityMsgs);

        /// <summary>
        /// Sends given message
        /// </summary>
        /// <param name="messageKey">Represents message key being sent</param>
        /// <param name="message">Represents message being sent</param>
        void SendEntity(String messageKey, String message);

        /// <summary>
        /// Receives Entity Operation
        /// </summary>
        /// <returns>Returns received message</returns>
        String ReceiveEntityOperation();
    }
}