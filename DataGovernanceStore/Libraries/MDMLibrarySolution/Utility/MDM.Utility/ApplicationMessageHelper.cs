using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace MDM.Utility
{
    using MDM.Interfaces;
    using MDM.BusinessObjects;
    using MDM.CacheManager.Business;

    /// <summary>
    /// Helps in managing application messages
    /// </summary>
    public class ApplicationMessageHelper
    {
        #region Fields

        #endregion

        #region Constructor

        #endregion

        #region Properties

        #endregion

        #region Methods

        #region Public Methods

        /// <summary>
        /// Get a message object from dictionary in cache
        /// </summary>
        /// <param name="messageCode">code of the message</param>
        /// <param name="locale">locale for which message has to be retrieved </param>
        /// <param name="param">Optional parameter. An array with values to replace in string.</param>
        /// <returns></returns>
        public static IApplicationMessage GetMessage(String messageCode, String locale, Object param = null)
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
            String defaultMessageCode = "000000";

            //Construct Cache Key name based on locale
            String cacheKeyName = String.Empty;
            cacheKeyName = locale + "_Messages";

            cache = CacheFactory.GetCache();
           
            // Populate messageDictionary from cache else load into cache.
            if (cache != null)
            {
                if (cache[cacheKeyName] != null)
                {
                    messageDictionary = (Dictionary<String, ApplicationMessage>)cache[cacheKeyName];
                }
            }

            //Find the message object from dictionary
            if (messageDictionary != null)
            {
                //retrieve message object based on Message code
                if (messageDictionary.ContainsKey(messageCode))
                {
                    messageObject = messageDictionary[messageCode];
                    if (param != null)
                    {
                        //Set Message property after formatting it.
                        messageObject.Message = MessageFormatter(messageObject.Message, param);
                    }
                }
                //If given code is not available, pass default code.
                else if (messageDictionary.ContainsKey(defaultMessageCode))
                {
                    messageObject = messageDictionary[defaultMessageCode];
                    messageObject.Message = MessageFormatter(messageObject.Message, messageCode);
                }
            }

            return messageObject;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Format a string based on parameters.
        /// Created a new method to escape the exception which String.Format throws if number of arguements are less then format parameters
        /// </summary>
        /// <param name="message">message text</param>
        /// <param name="param">array of parameters</param>
        /// <returns></returns>
        private static String MessageFormatter(String message, Object param)
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

        #endregion

        #endregion
    }
}
