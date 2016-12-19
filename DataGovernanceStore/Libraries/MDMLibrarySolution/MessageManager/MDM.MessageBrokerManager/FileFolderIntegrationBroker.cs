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
    public class FileFolderIntegrationBroker : IIntegrationMessageBroker
    {
        private String _folderPath;
        private Boolean _brokerReady;
        private Random _fileSuffixGenerator;

        /// <summary>
        /// FileFolderIntegrationBroker
        /// </summary>
        /// <param name="integrationMessageFolder"></param>
        public FileFolderIntegrationBroker(String integrationMessageFolder)
        {
            _folderPath = integrationMessageFolder;

            if (!String.IsNullOrEmpty(_folderPath))
            {
                try
                {
                    _brokerReady = Directory.CreateDirectory(_folderPath).Exists;
                    _fileSuffixGenerator = new Random();
                }
                catch (Exception)
                {
                    // _brokerReady will be false by default.
                }
            }
        }

        /// <summary>
        /// MessageBrokerOptions
        /// </summary>
        public MessageBrokerOptions MessageBrokerOptions { get; set; }

        /// <summary>
        /// Geenerates full file path from folder, current time stamp and 3 digit random number
        /// date-time_3digitRandom.json
        /// Example: 2016-06-30T00-20-16-6230675_152.json
        /// </summary>
        /// <returns></returns>
        public String GetNextTimeStampBasedFileName()
        {
            return Path.Combine(_folderPath, String.Format("{0}_{1}.json", DateTime.Now.ToString("yyyy-MM-ddTHH-mm-ss-fffffff"), _fileSuffixGenerator.Next(100, 1000)));
        }

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
            if (!_brokerReady)
            {
                throw new ApplicationException("Folder to drop Integraiton Message, 'Jigsaw.IntegrationMessageFolder' in web.config is missing.");
            }

            if (entityMsgs == null)
            {
                return;
            }

            foreach(var entityMsg in entityMsgs)
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
            if (!_brokerReady)
            {
                throw new ApplicationException("Folder to drop Integraiton Message, 'Jigsaw.IntegrationMessageFolder' in web.config is missing.");
            }

            // Write message to a file in that folder.`
            using (StreamWriter writer = new StreamWriter(GetNextTimeStampBasedFileName()))
            {
                writer.Write(message);
            }
        }
    }
}
