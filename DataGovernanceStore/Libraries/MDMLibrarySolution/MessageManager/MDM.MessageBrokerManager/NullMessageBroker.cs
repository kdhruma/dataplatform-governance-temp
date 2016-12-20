using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDM.MessageBrokerManager
{
    /// <summary>
    /// 
    /// </summary>
    public class NullMessageBroker : IIntegrationMessageBroker
    {
        private List<String> messages = new List<String>();

        /// <summary>
        /// Clears the messages.
        /// </summary>
        public void ClearMessages()
        {
            messages.Clear();
        }

        /// <summary>
        /// Gets the messages.
        /// </summary>
        /// <returns></returns>
        public List<String> GetMessages()
        {
            return messages;
        }

        /// <summary>
        /// MessageBrokerOptions
        /// </summary>
        public MessageBrokerOptions MessageBrokerOptions { get; set; }

        /// <summary>
        /// ReceiveEntityOperation
        /// </summary>
        /// <returns></returns>
        public string ReceiveEntityOperation()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Send multiple entities
        /// </summary>
        /// <param name="entityMessages"></param>
        public void SendEntities(Dictionary<String, String> entityMsgs)
        {
            if (entityMsgs == null)
            {
                return;
            }

            foreach (var entityMsg in entityMsgs)
            {
                SendEntity(entityMsg.Key, entityMsg.Value);
            }
        }

        /// <summary>
        /// Send entity
        /// </summary>
        /// <param name="messageKey"></param>
        /// <param name="message"></param>
        public void SendEntity(String messageKey, String message)
        {
            // Write message to the  list.
            messages.Add(message);
        }
    }
}
