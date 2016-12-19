using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Configuration;
using System.Text.RegularExpressions;
using Sys = System.IO;

namespace MDM.ConfigurationManager.Business
{
    using MDM.Core;
    using MDM.BusinessObjects;
    using MDM.Utility;
    using MDM.CacheManager.Business;

    /// <summary>
    /// Loads ApplicationMessage object in a dictionary from XML and cache it.
    /// Exposes a method to get message from cache (dictionary)
    /// </summary>
    public class ApplicationMessageBL : BusinessLogicBase
    {
        #region Public Methods

        /// <summary>
        /// Loads a dictionary object in cache from XML for a given locale.
        /// </summary>
        /// <param name="locale">locale for which XML has to be loaded</param>
        public void LoadMessages(String locale)
        {
            //Load Core Messages in Cache
            LoadMessagesInCache(locale, "MessagesConfig.xml");

            //Load Custom Messages in Cache
            LoadMessagesInCache(locale, "MessagesConfig_Custom.xml");
            
        }

        /// <summary>
        /// Get a message object from dictionary in cache
        /// </summary>
        /// <param name="messageCode">code of the message</param>
        /// <param name="locale">locale for which message has to be retrieved </param>
        /// <param name="param">Optional parameter. An array with values to replace in string.</param>
        /// <returns></returns>
        public ApplicationMessage GetMessage(String messageCode, String locale, Object param = null)
        {
            //Validate locale.. if locale is not available, set it to en_US
            if (String.IsNullOrWhiteSpace(locale))
                locale = "en_US";

            //Convert locale name to culture name.
            //TODO::Come up with a better way to convert
            if (locale == "en_WW")
                locale = "en-US";
            else
                locale = locale.Replace("_", "-");

            ICache cache = null;
            Dictionary<String, ApplicationMessage> messageDictionary = null;
            ApplicationMessage messageObject = new ApplicationMessage();
            ApplicationMessage clonedMessageObject = new ApplicationMessage();

            String defaultMessageCode = "000000";

            //Construct Cache Key name based on locale
            String cacheKeyName = String.Empty;
            cacheKeyName = locale + "_Messages";

            try
            {
                cache = CacheFactory.GetCache();
            }
            catch
            {
                //Ignore exception..
            }

            // Populate messageDictionary from cache else load into cache.
            if (cache != null)
            {
                if (cache[cacheKeyName] != null)
                {
                    messageDictionary = (Dictionary<String, ApplicationMessage>)cache[cacheKeyName];
                }
                else
                {
                    LoadMessages(locale);
                    messageDictionary = (Dictionary<String, ApplicationMessage>)cache[cacheKeyName];
                }
            }

            //Find the message object from dictionary
            if (messageDictionary != null )
            {
                //retrieve message object based on Messagecode
                if (messageDictionary.ContainsKey(messageCode))
                {
                    messageObject = messageDictionary[messageCode];

                    //Since it is the live reference of the object, get the copy of the message object in order to avoid any changes being done.. Bug # 30376
                    clonedMessageObject.Code = messageObject.Code;
                    clonedMessageObject.Type = messageObject.Type;
                    clonedMessageObject.Message = messageObject.Message;

                    if (param != null)
                    {
                        //Set Message property after formatting it.
                        clonedMessageObject.Message = MessageFormatter(clonedMessageObject.Message, param);
                    }
                }
                //If given code is not available, pass default code.
                else if(messageDictionary.ContainsKey(defaultMessageCode))
                {
                    messageObject = messageDictionary[defaultMessageCode];

                    //Since it is the live reference of the object, get the copy of the message object in order to avoid any changes being done.. Bug # 30376
                    clonedMessageObject.Code = messageObject.Code;
                    clonedMessageObject.Type = messageObject.Type;
                    clonedMessageObject.Message = messageObject.Message;

                    clonedMessageObject.Message = MessageFormatter(clonedMessageObject.Message, messageCode);
                }
            }
           
            return clonedMessageObject;
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Format a string based on parameters.
        /// Created a new method to escape the exception which String.Format throws if number of arguements are less then format parameters
        /// </summary>
        /// <param name="message">message text</param>
        /// <param name="param">array of parameters</param>
        /// <returns></returns>

        private String MessageFormatter(String message, Object param)
        {
            // Declaring a string array to be used in regular expression
            String[] StrParam = { "" };

            // If type of param is an Array, then load StrParam from param
            if (param is Array)
            {
                //Declaring ParamObjArray and populating from param
                Array ParamObjArray = (Array)param;
                StrParam = new String[ParamObjArray.Length];

                // Populating StrParam from ParamObjArray
                for (int i = 0; i < ParamObjArray.Length; i++)
                {
                    StrParam[i] = ParamObjArray.GetValue(i).ToString();
                }
            }
            //If param is not an array just assign it to StrParam
            else
            {
                StrParam[0] = param.ToString();
            }
            // Loop through the string array of param to find and replace {i}...{n} in message statement and replace it with StrParam[0]...StrParam[n]
            for (int i = 0; i <= StrParam.Length - 1; i++)
            {
                message = Regex.Replace(message, @"{[\\d\\" + i + "]}", StrParam[i].ToString());
            }

            //Remove {i..n} which are not replaced.
            message = Regex.Replace(message, @"{[\d]}", "");

            return message;

        }

        /// <summary>
        /// Loads messages from given file name
        /// </summary>
        /// <param name="locale">locale indicates folfer name</param>
        /// <param name="fileName">inficates file name</param>
        private void LoadMessagesInCache(String locale, String fileName)
        {
            //a dictionary object to store key as code and value as message object
            Dictionary<String, ApplicationMessage> messageDictionary = new Dictionary<String, ApplicationMessage>();

            //Getting folder name for messages from web.config
            String fileDirectoryPath = AppConfigurationHelper.GetSettingAbsolutePath("ApplicationMessageManager.Path");

            //Creating full relative path
            String filePath = fileDirectoryPath + "\\" + locale + "\\" + fileName;

            if (Sys.File.Exists(filePath))
            {
                XmlDocument messagesXML = new XmlDocument();
                //Loading path in XMLDocument
                messagesXML.Load(filePath);

                //Selecting all Message nodes from XML Document
                XmlNodeList messageNodes = messagesXML.SelectNodes("Messages/Message");
                String cacheKeyName = String.Empty;
                ICache cache = null;
                try
                {
                    cache = CacheFactory.GetCache();
                }
                catch
                {
                    //Ignore exception..
                }

                //Create a cacheKey
                cacheKeyName = locale + "_Messages";

                if (cache.Get(cacheKeyName) != null)
                {
                    messageDictionary = (Dictionary<String, ApplicationMessage>)cache.Get(cacheKeyName);
                }

                //Looping through each node and convert into ApplicationMessageObject and add in dictionary
                foreach (XmlNode node in messageNodes)
                {

                    //Create ApplicationMessage object
                    ApplicationMessage message = new ApplicationMessage();

                    //Populate ApplicationMessage properties
                    if (node != null)
                    {
                        if (node.Attributes["code"] != null)
                        {
                            message.Code = node.Attributes["code"].Value.ToString();
                        }
                        if (node.HasChildNodes && node.SelectSingleNode("Type") != null)
                        {
                            message.Type = node.SelectSingleNode("Type").InnerText;
                        }
                        if (node.HasChildNodes && node.SelectSingleNode("Title") != null)
                        {
                            message.Message = node.SelectSingleNode("Title").InnerText;
                        }
                        //Add the object in dictionary
                        if (!messageDictionary.ContainsKey(message.Code))
                        {
                            messageDictionary.Add(message.Code, message);
                        }
                    }

                }

                //Add dictionary object to cache with file dependency
                cache.Set(cacheKeyName, messageDictionary, DateTime.Now.AddHours(10));
            }
        }

        #endregion
     
    }

}
