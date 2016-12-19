using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Collections.Generic;

namespace MDM.MessageManager.Business
{
    using MDM.Core;
    using MDM.Utility;
    using MDM.BusinessObjects;
    using MDM.CacheManager.Business;
    using MDM.MessageManager.Data.SqlClient;
    using MDM.ConfigurationManager.Business;
    using ExceptionManager;
    using MDM.Interfaces;

    /// <summary>
    /// Specifies Locale Message manager
    /// </summary>
    public class LocaleMessageBL : BusinessLogicBase, ILocaleMessageManager
    {
        #region Fields

        private static Object lockObject = new Object();

        private LocaleMessageDA _localeMessageDA = null;

        private IDistributedCache _cacheManager = null;

        // private ICache _iCacheManager = null;

        private static Dictionary<LocaleEnum, LocaleMessageCollection> _localeMsgDictionary = new Dictionary<LocaleEnum, LocaleMessageCollection>();

        #endregion

        #region Constructors

        #endregion

        #region Properties

        #endregion

        #region Methods

        #region Public Methods

        #region Process

        /// <summary>
        ///  Process Locale Message based on locale
        /// </summary>
        /// <param name="localeMessage">This parameter is specifying instance of locale Message to be processed</param>
        /// <param name="callerContext">Name of application & Module which are performing action</param>
        /// <returns>Results of the operation having errors and information if any</returns>
        public OperationResult Process(LocaleMessage localeMessage, CallerContext callerContext)
        {
            #region Step : Initial Setup

            #region Parameter validations

            if (localeMessage == null)
                throw new ArgumentNullException("LocaleMessage", "LocaleMessage is null or empty");

            #endregion Parameter valudations

            #endregion

            return Process(new LocaleMessageCollection() { localeMessage }, callerContext);
        }

        /// <summary>
        ///  Process Locale Message based on different locale
        /// </summary>
        /// <param name="localeMessages">This parameter is specifying lists of instances of locale Message to be processed</param>
        /// <param name="callerContext">Name of application & Module which are performing action</param>
        /// <returns>Results of the operation having errors and information if any</returns>
        public OperationResult Process(LocaleMessageCollection localeMessages, CallerContext callerContext)
        {
            Boolean isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;

            #region Step : Initial Setup

            OperationResult operationResult = new OperationResult();
            DBCommandProperties command = DBCommandHelper.Get(callerContext.Application, callerContext.Module, MDMCenterModuleAction.Create);

            #region Parameter validations

            if (localeMessages == null || localeMessages.Count < 1)
                throw new ArgumentNullException("LocaleMessages", "LocaleMessages are null or empty");

            #endregion Parameter valudations

            #endregion

            if (isTracingEnabled)
                MDMTraceHelper.StartTraceActivity("LocaleMessageBL.LocaleMessage.Process", false);

            _localeMessageDA = new LocaleMessageDA();

            String userName = SecurityPrincipalHelper.GetCurrentSecurityPrincipal().CurrentUserName;

            operationResult = _localeMessageDA.Process(localeMessages, userName, "MessageManager.LocaleMessage.Process", command);

            if (isTracingEnabled)
                MDMTraceHelper.StopTraceActivity("LocaleMessageBL.LocaleMessage.Process");

            return operationResult;
        }

        #endregion

        #region Get Locale Messages

        /// <summary>
        /// Get Locale Message based on locale and message code
        /// </summary>
        /// <param name="locale">Indicates the Locale</param>
        /// <param name="messageCode">Indicates the Message Code</param>
        /// <param name="loadLastest">true if load fresh copy from DB otherwise take from cache</param>
        /// <param name="callerContext">Indicates Application and Module name by which action is being performed</param>
        /// <param name="escapeSpecialCharacters">Escape special characters ['],["],[\]. Set as True whenever message get is required for JavaScript</param>
        /// <returns>Returns LocaleMessage</returns>
        public LocaleMessage Get(LocaleEnum locale, String messageCode, Boolean loadLastest, CallerContext callerContext, Boolean escapeSpecialCharacters = false)
        {
            LocaleMessage localeMessage = new LocaleMessage();

            if (messageCode != null)
                messageCode = messageCode.Trim();

            LocaleMessageCollection localeMessages = Get(locale, new Collection<String>() { messageCode }, loadLastest, callerContext);

            if (localeMessages != null && localeMessages.Any())
                localeMessage = localeMessages.First();

            localeMessage.Message = localeMessage == null || localeMessage.Id < 0 ?
                        messageCode : localeMessage.Message;

            if (escapeSpecialCharacters)
                localeMessage.Message = localeMessage.Message.Replace("\\", "\\\\").Replace("'", "\\'").Replace("\"", "\\\"");

            return localeMessage;
        }

        /// <summary>
        /// Get Locale Message based on locale and message code
        /// </summary>
        /// <param name="locale">Indicates the Locale</param>
        /// <param name="messageCode">Indicates the Message Code</param>
        /// <param name="parameters">Indicates message params</param>
        /// <param name="loadLastest">true if load fresh copy from DB otherwise take from cache</param>
        /// <param name="callerContext">Indicates Application and Module name by which action is being performed</param>
        /// <param name="escapeSpecialCharacters">Escape special characters ['],["],[\]. Set as True whenever message get is required for JavaScript</param>
        /// <returns>Returns LocaleMessage</returns>
        public LocaleMessage Get(LocaleEnum locale, String messageCode, Object[] parameters, Boolean loadLastest, CallerContext callerContext, Boolean escapeSpecialCharacters = false)
        {
            LocaleMessage localeMessage = null;

            //LocaleMessage resultLocaleMessage = Get(locale, messageCode, loadLastest, callerContext, escapeSpecialCharacters);

            LocaleMessage resultLocaleMessage = new LocaleMessage();
            resultLocaleMessage.Locale = LocaleEnum.en_WW;
            resultLocaleMessage.Message = "Workflow invoked successfully for {0} entities.";

            if (resultLocaleMessage != null)
            {
                localeMessage = (LocaleMessage)resultLocaleMessage.Clone();
            }

            if (localeMessage != null && parameters != null)
            {
                localeMessage.Message = String.Format(localeMessage.Message, parameters);
            }

            return localeMessage;
        }

        /// <summary>
        /// Get Locale Message collection based on locale and message codes
        /// </summary>
        /// <param name="locale">Indicates the Locale</param>
        /// <param name="messageCodeList">Indicates the Message Code List</param>
        /// <example >
        ///  1. Requested message code is "100355"
        ///      Output for en-Us Locale:
        ///      Message ="Detail" 
        ///      Code =100355 
        ///      Id =100355 
        /// 
        /// 2. Requested message code is "Detailed Description"
        ///     Output for en-Us Locale:
        ///     Message ="Detail Description" 
        ///     Code =Detail Description
        ///     Id =2147483647 (Max value of Integer because Id is not -1)
        /// 
        /// 3. Requested message code is "Detail"
        ///     Output for en-Us Locale:
        ///     Message ="Message Code is not Available"
        ///     Code =100355 
        ///     Id = -1
        /// </example>
        /// <returns>Returns LocaleMessageCollection</returns>
        public LocaleMessageCollection Get(LocaleEnum locale, Collection<String> messageCodeList, Boolean loadLatest, CallerContext callerContext)
        {
            LocaleMessageCollection localeMessageCollection = new LocaleMessageCollection();
            LocaleMessageCollection nonDDGLocaleMessageCollection = new LocaleMessageCollection();

            //This contains DDG + other locale messages
            LocaleMessageCollection allLocaleMessages = new LocaleMessageCollection();

            Collection<String> nonDDGMsgCodeList = new Collection<String>();

            //Get system locale
            LocaleEnum systemLocale = MDM.Utility.GlobalizationHelper.GetSystemUILocale();

            #region Lock the object Cache object is not present

            if (!IsCached(locale))
            {
                lock (lockObject)
                {
                    if (!IsCached(locale))
                    {
                        GetLocaleMessages(locale, new Collection<String>(), loadLatest, callerContext);

                        if (systemLocale != locale)
                        {
                            GetLocaleMessages(systemLocale, new Collection<String>(), loadLatest, callerContext);
                        }
                    }
                }
            }

            #endregion

            Collection<String> ddgLocaleMessageCodeList = new Collection<String>();

            #region Non Message Code list

            if (messageCodeList != null && messageCodeList.Count > 0)
            {
                foreach (String code in messageCodeList)
                {
                    //If message code is more than 10 character assume message code as Message.
                    if (code.Length > 10)
                    {
                        LocaleMessage localeMessage = new LocaleMessage(Int32.MaxValue, code, MessageClassEnum.UnKnown, code, String.Empty, String.Empty, locale);
                        nonDDGLocaleMessageCollection.Add(localeMessage);
                    }
                    else if(code.StartsWith("DDG"))
                    {
                        ddgLocaleMessageCodeList.Add(code.Trim());
                    }
                    else
                    {
                        nonDDGMsgCodeList.Add(code.Trim());
                    }
                }
            }

            #endregion

            if (ddgLocaleMessageCodeList != null && ddgLocaleMessageCodeList.Count > 0)
            {
                DDGLocaleMessageBL ddgLocaleMessageManager = new DDGLocaleMessageBL();
                allLocaleMessages = ddgLocaleMessageManager.Get(locale, ddgLocaleMessageCodeList, callerContext);

                //If only DDG related messages are requested, then return these messages and no need to load other message.
                if (messageCodeList.Count == allLocaleMessages.Count)
                {
                    return allLocaleMessages;
                }
            }

            if ((nonDDGMsgCodeList != null && nonDDGMsgCodeList.Count > 0) || messageCodeList.Count == 0)
            {
                localeMessageCollection = GetLocaleMessages(locale, nonDDGMsgCodeList, false, callerContext);

                if (systemLocale != locale)
                {
                    
                    localeMessageCollection = GetLocaleMessages(locale, nonDDGMsgCodeList, localeMessageCollection, callerContext);
                }

                //If a requested locale is not system locale and message code list is null then check all message codes are present or not
                //If a message code is not available in requested locale(not system locale) then return system locale message
                if (systemLocale != locale && (nonDDGMsgCodeList == null || nonDDGMsgCodeList.Count == 0))
                {
                    LocaleMessageCollection systemMessageCollection = new LocaleMessageCollection();

                    //Get System Locale Messages
                    systemMessageCollection = GetLocaleMessages(systemLocale, new Collection<String>(), false, callerContext);

                    //Verify and return locale message collection
                    localeMessageCollection = GetSystemLocaleMessages(systemMessageCollection, localeMessageCollection);
                }
                else if (systemLocale == locale && (nonDDGMsgCodeList == null || nonDDGMsgCodeList.Count == 0))
                {
                    //If requested locale is system locale and message code list is null. This is nothing but default system locale message collection.
                    return localeMessageCollection;
                }
                else
                {
                    //If message code list is not null  and requested locale is not system locale
                    localeMessageCollection = GetLocaleMessages(locale, nonDDGMsgCodeList, localeMessageCollection, callerContext);
                }
            }

            #region Merge Un known message code's Messages & LocaleMessages

            localeMessageCollection.AddRange(nonDDGLocaleMessageCollection);
            
            #endregion

            //Add the locale messages the complete collection based on whether ddg related messages are available.
            if (allLocaleMessages != null && allLocaleMessages.Count > 0)
            {
                allLocaleMessages.AddRange(localeMessageCollection);
            }
            else
            {
                allLocaleMessages = localeMessageCollection;
            }

            return allLocaleMessages;
        }

        /// <summary>
        /// Try Get Locale Message based on locale and message code if message code is empty it will return defaultMessage
        /// </summary>
        /// <param name="locale">Indicates the Locale</param>
        /// <param name="messageCode">Indicates the Message Code</param>
        /// <param name="defaultMessage">If message code is empty it will return defaultMessage</param>
        /// <param name="loadLastest">true if load fresh copy from DB otherwise take from cache</param>
        /// <param name="callerContext">Indicates Application and Module name by which action is being performed</param>
        /// <param name="escapeSpecialCharacters">Escape special characters ['],["],[\]. Set as True whenever message get is required for JavaScript</param>
        /// <returns>Returns LocaleMessage</returns>
        public LocaleMessage TryGet(LocaleEnum locale, String messageCode, String defaultMessage, Boolean loadLastest, CallerContext callerContext, Boolean escapeSpecialCharacters = false)
        {
            LocaleMessage localeMessage = new LocaleMessage();

            if (!String.IsNullOrEmpty(messageCode))
            {
                localeMessage = Get(locale, messageCode, loadLastest, callerContext, escapeSpecialCharacters);
            }

            if (String.IsNullOrEmpty(localeMessage.Message))
            {
                localeMessage.Code = String.Empty;
                localeMessage.Message = defaultMessage;
            }

            return localeMessage;
        }

        /// <summary>
        /// Try Get Locale Message based on locale and message code if message code is empty it will return defaultMessage
        /// </summary>
        /// <param name="locale">Indicates the Locale</param>
        /// <param name="messageCode">Indicates the Message Code</param>
        /// <param name="defaultMessage">If message code is empty it will return defaultMessage</param>
        /// <param name="parameters">Indicates message params</param>
        /// <param name="loadLastest">true if load fresh copy from DB otherwise take from cache</param>
        /// <param name="callerContext">Indicates Application and Module name by which action is being performed</param>
        /// <param name="escapeSpecialCharacters">Escape special characters ['],["],[\]. Set as True whenever message get is required for JavaScript</param>
        /// <returns>Returns LocaleMessage</returns>
        public LocaleMessage TryGet(LocaleEnum locale, String messageCode, String defaultMessage, Object[] parameters, Boolean loadLastest, CallerContext callerContext, Boolean escapeSpecialCharacters = false)
        {
            LocaleMessage localeMessage = new LocaleMessage();

            if (!String.IsNullOrEmpty(messageCode))
            {
                LocaleMessage resultLocaleMessage = Get(locale, messageCode, loadLastest, callerContext, escapeSpecialCharacters);

                if (resultLocaleMessage != null)
                {
                    localeMessage = new LocaleMessage(resultLocaleMessage.ToXml());
                }
            }

            if (String.IsNullOrEmpty(localeMessage.Message))
            {
                localeMessage.Code = String.Empty;
                localeMessage.Message = defaultMessage;
            }

            if (localeMessage != null && parameters != null)
            {
                localeMessage.Message = String.Format(localeMessage.Message, parameters);
            }

            return localeMessage;
        }

        #endregion

        #endregion

        #region Private Methods

        #region Verification for Cache Object
        /// <summary>
        /// Verify whether the cache object is available or not
        /// </summary>
        /// <param name="locale">Indicates the Locale</param>
        /// <returns>If Cache object is present then True else False</returns>
        private Boolean IsCached(LocaleEnum locale)
        {
            // Priority 1. Static Dictionary.
            //          2. Distributed Cache.

            Boolean isCached = false;
            _cacheManager = CacheFactory.GetDistributedCache();

            //Get a distributed cache key name
            String cacheKeyName = CacheKeyGenerator.GetLocaleMessageCacheKey(locale);

            try
            {
                if ((_localeMsgDictionary != null && _localeMsgDictionary.ContainsKey(locale)) || (_cacheManager != null && _cacheManager.Get(cacheKeyName) != null))
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler exHandler = new ExceptionHandler(ex);
            }

            return isCached;
        }
        #endregion

        #region Get locale Messages from db/cache
        /// <summary>
        /// Get Locale Message collection based on locale and message codes
        /// </summary>
        /// <param name="locale">Indicates the Locale</param>
        /// <param name="messageCodeList">Indicates the Message Code List</param>
        /// <returns>Returns LocaleMessageCollection</returns>
        private LocaleMessageCollection GetLocaleMessages(LocaleEnum locale, Collection<String> messageCodeList, Boolean loadLatest, CallerContext callerContext)
        {
            if (locale == LocaleEnum.UnKnown)
            {
                throw new ArgumentNullException("Locale ", "Locale is not available");  //TODO::Need to change
            }

            Object localeMsgCollection = null;
            LocaleMessageCollection localeMessageCollection = new LocaleMessageCollection();

            //Read Global Cache Time. Default cache time is 10 days. 
            Int32 cacheTime = ValueTypeHelper.Int32TryParse(AppConfigurationHelper.GetAppConfig<String>("MDMCenter.Cache.Global.Duration"), 10);

            //Get command
            DBCommandProperties command = DBCommandHelper.Get(callerContext.Application, callerContext.Module, MDMCenterModuleAction.Update);

            _cacheManager = CacheFactory.GetDistributedCache();
            //_iCacheManager = CacheFactory.GetCache();

            //Get a distributed cache key name
            String cacheKeyName = CacheKeyGenerator.GetLocaleMessageCacheKey(locale);

            Collection<String> ddgMessageCodeList = new Collection<String>();
            Collection<String> nonDDGMessageCodeList = new Collection<String>();

            if (messageCodeList != null && messageCodeList.Count > 0)
            {
                foreach (String messageCode in messageCodeList)
                {
                    if (messageCode.StartsWith("DDG"))
                    {
                        ddgMessageCodeList.Add(messageCode);
                    }
                    else
                    {
                        nonDDGMessageCodeList.Add(messageCode);
                    }
                }

                messageCodeList = nonDDGMessageCodeList;
            }

            LocaleMessageCollection ddgLocaleMessages = new LocaleMessageCollection();
            if (ddgMessageCodeList.Count > 0)
            {
                DDGLocaleMessageBL ddgLocaleMessageBL = new DDGLocaleMessageBL();
                ddgLocaleMessages = ddgLocaleMessageBL.Get(locale, ddgMessageCodeList, callerContext);
            }

            //If requested for all locale messages or requested for non DDG related message code then 
            //get it from dictionary/cache/db; else we already got the DDG related messages from DB so skip below.
            if ((messageCodeList != null && messageCodeList.Count > 0) ||
              ((messageCodeList != null && messageCodeList.Count == 0) && (ddgMessageCodeList != null && ddgMessageCodeList.Count == 0)))
            {
                #region Get from Static Dictionary

                if (_localeMsgDictionary != null && _localeMsgDictionary.ContainsKey(locale))
                {
                    localeMessageCollection = _localeMsgDictionary[locale];
                }

                #endregion

                #region Get from Distributed Cache

                // Get Messages from distributed cache.
                // & If Messages available in distributed cache & not available in locale cache. Then add the messages to local cache.
                if (_cacheManager != null && (localeMessageCollection == null || localeMessageCollection.Count == 0))
                {
                    //Get from distributed cache.
                    localeMsgCollection = _cacheManager.Get(cacheKeyName);

                    if (localeMsgCollection != null && localeMsgCollection is LocaleMessageCollection)
                    {
                        localeMessageCollection = (LocaleMessageCollection)localeMsgCollection;

                        if (_localeMsgDictionary != null && localeMessageCollection != null && localeMessageCollection.Count > 0)
                        {
                            _localeMsgDictionary[locale] = localeMessageCollection;
                        }

                    }
                }

                #endregion

                //if load latest is false and massages found in cache..
                if (localeMessageCollection != null && localeMessageCollection.Count > 0 && !loadLatest)
                {
                    if (messageCodeList != null && messageCodeList.Count > 0)
                    {
                        localeMessageCollection = (LocaleMessageCollection)localeMessageCollection.Get(locale, messageCodeList, MDM.Utility.GlobalizationHelper.GetSystemUILocale());
                    }
                }
                else
                {
                    #region Get Messages from DB

                    LocaleMessageDA localeMessageDA = new LocaleMessageDA();
                    localeMessageCollection = localeMessageDA.GetLocaleMessages(locale, messageCodeList, command);

                    #endregion

                    // If message code count is greater than zero then update the message code.
                    // Else update the caches for the requested locale.
                    if (messageCodeList != null && messageCodeList.Count > 0)
                    {
                        #region Update New locale Message Codes
                        LocaleMessageCollection updatedLocaleMessage = new LocaleMessageCollection(localeMessageCollection.ToXml());

                        //Remove existing message code from the message collection and update(new values) into message Collection
                        foreach (LocaleMessage lMsg in localeMessageCollection)
                        {
                            updatedLocaleMessage.Remove(lMsg);
                            updatedLocaleMessage.Add(lMsg);
                        }

                        localeMessageCollection = updatedLocaleMessage;

                        #endregion
                    }
                    //If message codes are available in the list then no need to set to cache.
                    else if (localeMessageCollection != null && localeMessageCollection.Count > 0)
                    {
                        #region Populate Messges to Cache

                        // For distributed Cache
                        if (_cacheManager != null && !String.IsNullOrWhiteSpace(cacheKeyName))
                        {
                            _cacheManager.Set(cacheKeyName, localeMessageCollection, DateTime.Now.AddDays(cacheTime));
                        }

                        if (_localeMsgDictionary != null)
                        {
                            _localeMsgDictionary[locale] = localeMessageCollection;
                        }
                      
                        #endregion
                    }
                }
            }

            if (ddgLocaleMessages != null && ddgLocaleMessages.Count > 0)
            {
                localeMessageCollection.AddRange(ddgLocaleMessages);
            }

            return localeMessageCollection;
        }
        #endregion

        #region Fall down logic for Locale Message

        /// <summary>
        /// Verify and populate messages into message collection for requested message codes 
        /// </summary>
        /// <param name="userLocale"></param>
        /// <param name="messageCodeList"></param>
        /// <param name="sourceMessageCollection"></param>
        /// <returns></returns>
        private LocaleMessageCollection GetLocaleMessages(LocaleEnum userLocale, Collection<String> messageCodeList, LocaleMessageCollection sourceMessageCollection, CallerContext callerContext)
        {
            Collection<String> filteredMessageCodeList = new Collection<String>();
            LocaleMessageCollection resultedMessageCollection = new LocaleMessageCollection();
            LocaleEnum systemLocale = MDM.Utility.GlobalizationHelper.GetSystemUILocale();

            //Collect message codes which are not available in the collection
            filteredMessageCodeList = this.GetMessageCodes(messageCodeList, sourceMessageCollection);

            if (resultedMessageCollection != null && filteredMessageCodeList != null)
            {
                resultedMessageCollection.ToList().ForEach(lMsg =>
                {
                    sourceMessageCollection.Add(lMsg);
                });

                //Verify Again the message code 
                filteredMessageCodeList = this.GetMessageCodes(filteredMessageCodeList, resultedMessageCollection);

                if (filteredMessageCodeList != null && filteredMessageCodeList != null)
                {
                    #region Collect system Locale Messages

                    //If it not available 
                    //Get it from System UI Locale from Cache
                    if (userLocale != systemLocale)
                    {
                        resultedMessageCollection = this.GetLocaleMessages(systemLocale, filteredMessageCodeList, false, callerContext);

                        if (resultedMessageCollection != null)
                        {
                            resultedMessageCollection.ToList().ForEach(lMsg =>
                            {
                                sourceMessageCollection.Add(lMsg);
                            });

                            //Verify Again the message code 
                            filteredMessageCodeList = this.GetMessageCodes(filteredMessageCodeList, resultedMessageCollection);
                        }
                    }
                    #endregion

                    #region Populate Default Locale Messages

                    if (filteredMessageCodeList != null)
                    {
                        //If it is not available in User UI locale and System UI locale then send a default message code 
                        resultedMessageCollection = this.GetDefaultMessages(messageCodeList, userLocale);

                        foreach (LocaleMessage lMsg in resultedMessageCollection)
                        {
                            sourceMessageCollection.Add(lMsg);
                        }
                    }
                    #endregion
                }
            }

            return sourceMessageCollection;
        }
        #endregion

        #region Get Default Locale Message
        /// <summary>
        /// Get a Default Message  if it is not available in the system
        /// </summary>
        /// <param name="messageCodeList"></param>
        /// <param name="userLocale"></param>
        /// <returns></returns>
        private LocaleMessageCollection GetDefaultMessages(Collection<String> messageCodeList, LocaleEnum userLocale)
        {
            LocaleMessageCollection defaultMessageCollection = new LocaleMessageCollection();

            messageCodeList.ToList().ForEach(messagCcode =>
            {
                LocaleMessage localeMessage = new LocaleMessage(-1, messagCcode, MessageClassEnum.UnKnown, "Message Code is not Available", "Message Code is not available in the System", String.Empty, userLocale);

                defaultMessageCollection.Add(localeMessage);
            });

            return defaultMessageCollection;
        }

        #endregion

        #region Filter Message Codes

        /// <summary>
        /// Get message code list which is not available in message collection
        /// </summary>
        /// <param name="messageCodeList"></param>
        /// <param name="sourceMessageCollection"></param>
        /// <returns></returns>
        private Collection<String> GetMessageCodes(Collection<String> messageCodeList, LocaleMessageCollection sourceMessageCollection)
        {
            Collection<String> resultMessageCodeList = null;

            var sourceMessageCodes = from msg in sourceMessageCollection select msg.Code;

            var filterMessageCodes = from code in messageCodeList where !sourceMessageCodes.Contains(code) select code;

            if (filterMessageCodes != null && filterMessageCodes.ToList().Count > 0)
            {
                resultMessageCodeList = new Collection<String>();

                filterMessageCodes.ToList().ForEach(code =>
                {
                    resultMessageCodeList.Add(code);
                });
            }

            return resultMessageCodeList;
        }

        #endregion

        #region Get System Locale Messages
        /// <summary>
        /// Verify and get locale message from system locale if message code is not available in requested locale
        /// </summary>
        /// <param name="systemLocaleMessages">System Locale Messages</param>
        /// <param name="sourceMessageCollection">Requested Locale Messages</param>
        /// <returns>Returns LocaleMessageCollection</returns>
        private static LocaleMessageCollection GetSystemLocaleMessages(LocaleMessageCollection systemLocaleMessages, LocaleMessageCollection sourceMessageCollection)
        {
            LocaleMessageCollection resultMessages = new LocaleMessageCollection();

            //Get Requested locale Message codes
            var sourceMessageCodes = from msg in sourceMessageCollection select msg.Code;

            //Filter system locale messages if it is not present in requested locale message collection
            var filterMessages = from sysLMsg in systemLocaleMessages where !sourceMessageCodes.Contains(sysLMsg.Code) select sysLMsg;

            if (filterMessages != null && filterMessages.ToList().Count > 0)
            {
                filterMessages.ToList().ForEach(lMsg =>
                {
                    sourceMessageCollection.Add(lMsg);
                });
            }

            return sourceMessageCollection;
        }
        #endregion

        #endregion

        #endregion
    }
}
