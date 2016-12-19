using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;

namespace MDM.BusinessObjects
{
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Specifies the Locale Message Collection 
    /// </summary>
    [DataContract]
    public class LocaleMessageCollection : ICollection<LocaleMessage>, IEnumerable<LocaleMessage>, ILocaleMessageCollection, IBusinessRuleObjectCollection
    {
        #region Fields

        [DataMember]
        private Collection<LocaleMessage> _localeMessages = new Collection<LocaleMessage>();

        #endregion

        #region Constructors

        /// <summary>
        /// Parameterless constructor
        /// </summary>
        public LocaleMessageCollection() : base() { }

        /// <summary>
        /// Initialize LocaleMessageCollection from IList
        /// </summary>
        /// <param name="localeMessagesList">IList of LocaleMessages</param>
        public LocaleMessageCollection(IList<LocaleMessage> localeMessagesList)
        {
            this._localeMessages = new Collection<LocaleMessage>(localeMessagesList);
        }

        /// <summary>
        /// Constructor which takes Xml as input
        /// </summary>
        public LocaleMessageCollection(String valueAsXml)
        {
            LoadLocaleMessageCollection(valueAsXml);
        }

        #endregion

        #region Properties

        #endregion

        #region Indexer

        /// <summary>
        /// Returns LocaleMessage object for provided index from collection
        /// </summary>
        /// <param name="index">index</param>
        /// <returns>LocaleMessage object</returns>
        public LocaleMessage this[int index]
        {
            get 
            {
                return this._localeMessages[index];
            }
        }

        #endregion

        #region Methods

        #region Public Method

        /// <summary>
        /// Check if LocaleMessageCollection contains LocaleMessage with given locale and messageCode
        /// </summary>
        /// <param name="locale">locale to search in LocaleMessageCollection</param>
        /// <param name="messageCode">messageCode to search in LocaleMessageCollection</param>
        /// <param name="systemUILocale">System UI Locale which will be considered when message is not available for the requested locale</param>
        /// <returns>
        /// <para>true : If LocaleMessage found in LocaleMessageCollection</para>
        /// <para>false : If LocaleMessage found not in LocaleMessageCollection</para>
        /// </returns>
        public bool Contains(LocaleEnum locale, String messageCode, LocaleEnum systemUILocale)
        {
            if (Get(locale, messageCode, systemUILocale) != null)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Check if LocaleMessageCollection contains LocaleMessage with given locale and messageCode list
        /// </summary>
        /// <param name="locale">Locale to search in LocaleMessageCollection</param>
        /// <param name="messageCodeList">MessageCodeList to search in LocaleMessageCollection.</param>
        /// <param name="systemUILocale">System UI Locale which will be considered when message is not available for the requested locale</param>
        /// <returns>
        /// <para>true : If LocaleMessage found in LocaleMessageCollection</para>
        /// <para>false : If LocaleMessage found not in LocaleMessageCollection</para>
        /// </returns>
        public bool Contains(LocaleEnum locale, Collection<String> messageCodeList, LocaleEnum systemUILocale)
        {
            if (Get(locale, messageCodeList, systemUILocale) != null)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Remove LocaleMessage object from LocaleMessageCollection
        /// </summary>
        /// <param name="localeMessageId">Id of LocaleMessage which is to be removed from collection</param>
        /// <returns>true if item is successfully removed; otherwise, false. This method also returns false if item was not found in the original collection</returns>
        public bool Remove(Int32 localeMessageId)
        {
            LocaleMessage localeMessage = Get(localeMessageId, String.Empty);

            if (localeMessage == null)
                throw new ArgumentException("No localeMessage found for given localeMessage id");
            else
                return this.Remove(localeMessage);
        }

        /// <summary>
        /// Remove LocaleMessage object from LocaleMessageCollection
        /// </summary>
        /// <param name="messageCode">Message code of LocaleMessage which is to be removed from collection</param>
        /// <returns>true if item is successfully removed; otherwise, false. This method also returns false if item was not found in the original collection</returns>
        public bool Remove(String messageCode)
        {
            LocaleMessage localeMessage = Get(-1, messageCode);

            if (localeMessage == null)
                throw new ArgumentException("No localeMessage found for given message Code");
            else
                return this.Remove(localeMessage);
        }


        /// <summary>
        /// Remove LocaleMessage object from LocaleMessageCollection
        /// </summary>
        /// <param name="localeMessageId">Id of LocaleMessage which is to be removed from collection</param>
        /// <param name="messageCode">MessageCode to remove from LocaleMessage collection. This is optional parameter. Default value is -1</param>
        /// <returns>true if item is successfully removed; otherwise, false. This method also returns false if item was not found in the original collection</returns>
        public bool Remove(Int32 localeMessageId, String messageCode)
        {
            LocaleMessage localeMessage = Get(localeMessageId, messageCode);

            if (localeMessage == null)
                throw new ArgumentException("No LocaleMessage found for given LocaleMessage id");
            else
                return this.Remove(localeMessage);
        }

        /// <summary>
        /// Determines whether two Object instances are equal.
        /// </summary>
        /// <param name="obj">The Object to compare with the current Object.</param>
        /// <returns>true if the specified Object is equal to the current Object; otherwise, false.</returns>
        public override bool Equals(object obj)
        {
            if (obj is LocaleMessageCollection)
            {
                LocaleMessageCollection objectToBeCompared = obj as LocaleMessageCollection;

                Int32 localeMessagesUnion = this._localeMessages.ToList().Union(objectToBeCompared.ToList()).Count();
                Int32 localeMessagesIntersect = this._localeMessages.ToList().Intersect(objectToBeCompared.ToList()).Count();

                if (localeMessagesUnion != localeMessagesIntersect)
                    return false;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Compare locale message collection with current collection.
        /// This method will compare locale messages. If current collection has more containers than object to be compared, extra messages will be ignored.
        /// </summary>
        /// <param name="subsetLocaleMessageCollection">Indicates ContainerCollection to be compared with current collection</param>
        /// <returns>Returns True : Is one is superset of other. False : otherwise</returns>
        public bool IsSuperSetOf(LocaleMessageCollection subsetLocaleMessageCollection)
        {
            if (subsetLocaleMessageCollection != null)
            {
                foreach (LocaleMessage localeMessage in subsetLocaleMessageCollection)
                {
                    ILocaleMessage iLocaleMessage = this.GetLocaleMessageByParams(localeMessage.Locale, localeMessage.Code);

                    if (iLocaleMessage != null)
                    {
                        LocaleMessage sourceLocaleMessage = (LocaleMessage)iLocaleMessage;

                        if (!sourceLocaleMessage.IsSuperSetOf(localeMessage))
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Serves as a hash function for type
        /// </summary>
        /// <returns>A hash code for the current Object.</returns>
        public override int GetHashCode()
        {
            Int32 hashCode = 0;
            foreach (LocaleMessage lMsg in this._localeMessages)
            {
                hashCode += lMsg.GetHashCode();
            }
            return hashCode;
        }

        /// <summary>
        /// Initialize LocaleMessage Collection from Xml
        /// </summary>
        /// <param name="valuesAsXml">Xml having value for LocaleMessageCollection</param>
        public void LoadLocaleMessageCollection(String valuesAsXml)
        {

            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                XmlTextReader reader = null;
                try
                {
                    reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "LocaleMessage")
                        {
                            String localeMessageXml = reader.ReadOuterXml();

                            if (!String.IsNullOrEmpty(localeMessageXml))
                            {
                                LocaleMessage localeMessage = new LocaleMessage(localeMessageXml);
                                if (localeMessage != null)
                                {
                                    this.Add(localeMessage);
                                }
                            }

                        }
                        else
                        {
                            reader.Read();
                        }
                    }
                }
                finally
                {
                    if (reader != null)
                    {
                        reader.Close();
                    }
                }
            }
        }

        /// <summary>
        /// Removes the Locale message from Locale message collection by reference identifier
        /// </summary>
        /// <param name="referenceId">Indicates the reference identifier of locale message</param>
        /// <returns>Returns true if the locale message is removed successfully; othewise returns false</returns>
        public Boolean RemoveByReferenceId(Int64 referenceId)
        {
            Boolean result = false;

            foreach (LocaleMessage systemMessage in this._localeMessages)
            {
                if (ValueTypeHelper.Int64TryParse(systemMessage.ReferenceId, -1) == referenceId)
                {
                    result = this.Remove(systemMessage);
                    break;
                }
            }

            return result;
        }

        #endregion

        #region Private Method


        private void Validate(LocaleEnum locale)
        {
            if (this._localeMessages == null)
            {
                throw new NullReferenceException("There are no LocaleMessages to search in");
            }

            if (locale == LocaleEnum.UnKnown)
            {
                throw new ArgumentException("Culture is not available. Please check the Culture");
            }

        }

        private IEnumerable<LocaleMessage> FilterLocaleMessages(LocaleEnum locale, Collection<String> messageCodeList, LocaleEnum systemUILocale)
        {
            LocaleMessageCollection localeMessages = null;

            if (messageCodeList != null && messageCodeList.Count > 0)
            {
                localeMessages = new LocaleMessageCollection();
                LocaleMessage message = null;

                foreach (String messageCode in messageCodeList)
                {
                    //Check in requested locale..
                    message = GetLocaleMessageByParams(locale, messageCode);
                    if (message == null)
                    {
                        //Check in system locale
                        message = GetLocaleMessageByParams(systemUILocale, messageCode);
                    }
                    if (message != null)
                    {
                        localeMessages.Add(message);
                    }
                }
            }
            else
            {
                IEnumerable<LocaleMessage> localeMessagesList = from lMsg in this._localeMessages
                                    where lMsg.Locale == locale
                                    select lMsg;

                if (localeMessagesList != null && localeMessagesList.Count() > 0)
                    localeMessages = new LocaleMessageCollection(localeMessagesList.ToList());
            }

            return localeMessages;
        }

        /// <summary>
        /// Get LocaleMessage from current LocaleMessage collection based on Id and messageCode
        /// </summary>
        /// <param name="localeMessageId">Id of LocaleMessage which is to be searched</param>
        /// <param name="messageCode">messageCode of an LocaleMessage. Default value is -1. This parameter is optional</param>
        /// <returns>LocaleMessage having given localeMessageId messageCode</returns>
        private LocaleMessage Get(Int32 localeMessageId, String messageCode)
        {
            LocaleMessage localeMessage = null;

            if (String.IsNullOrWhiteSpace(messageCode))
            {
                localeMessage = GetLocaleMessageByParams(localeMessageId);
            }
            else if (localeMessageId == -1)
            {
                localeMessage = GetLocaleMessageByParams(messageCode);
            }
            else
            {
                localeMessage = GetLocaleMessageByParams(localeMessageId, messageCode);
            }
            return localeMessage;
        }

        private LocaleMessage GetLocaleMessageByParams(LocaleEnum locale, String messageCode)
        {
            Int32 localeMessagesCount = _localeMessages.Count;
            LocaleMessage localeMessage = null;

            for (Int32 index = 0; index < localeMessagesCount; index++)
            {
                localeMessage = _localeMessages[index];
                if (localeMessage.Locale == locale && localeMessage.Code == messageCode)
                    return localeMessage;
            }
            return null;
        }

        private LocaleMessage GetLocaleMessageByParams(Int32 localeMessageId)
        {
            Int32 localeMessagesCount = _localeMessages.Count;
            LocaleMessage localeMessage = null;

            for (Int32 index = 0; index < localeMessagesCount; index++)
            {
                localeMessage = _localeMessages[index];
                if (localeMessage.Id == localeMessageId)
                    return localeMessage;
            }
            return null;
        }

        private LocaleMessage GetLocaleMessageByParams(String messageCode)
        {
            Int32 localeMessagesCount = _localeMessages.Count;
            LocaleMessage localeMessage = null;

            for (Int32 index = 0; index < localeMessagesCount; index++)
            {
                localeMessage = _localeMessages[index];
                if (localeMessage.Code == messageCode)
                    return localeMessage;
            }
            return null;
        }

        private LocaleMessage GetLocaleMessageByParams(Int32 localeMessageId, String messageCode)
        {
            Int32 localeMessagesCount = _localeMessages.Count;
            LocaleMessage localeMessage = null;

            for (Int32 index = 0; index < localeMessagesCount; index++)
            {
                localeMessage = _localeMessages[index];
                if (localeMessage.Id == localeMessageId && localeMessage.Code == messageCode)
                    return localeMessage;
            }
            return null;
        }

        #endregion

        #endregion

        #region ICollection<LocaleMessage> Members

        /// <summary>
        /// Add LocaleMessage object in collection
        /// </summary>
        /// <param name="item">LocaleMessage to add in collection</param>
        public void Add(LocaleMessage item)
        {
            this._localeMessages.Add(item);
        }

        /// <summary>
        /// Add locale messages in collection
        /// </summary>
        /// <param name="items">Indicates the locale messages to add in collection</param>
        public void AddRange(LocaleMessageCollection items)
        {
            if (items != null && items.Count > 0)
            {
                foreach (LocaleMessage item in items)
                {
                    this._localeMessages.Add(item);
                }
            }
        }

        /// <summary>
        /// Add ILocaleMessage object in collection
        /// </summary>
        /// <param name="item">ILocaleMessage to add in collection</param>
        public void Add(ILocaleMessage item)
        {
            this._localeMessages.Add((LocaleMessage)item);
        }

        /// <summary>
        /// Removes all LocaleMessages from collection
        /// </summary>
        public void Clear()
        {
            this._localeMessages.Clear();
        }

        /// <summary>
        /// Determines whether the LocaleMessageCollection contains a specific LocaleMessage.
        /// </summary>
        /// <param name="item">The LocaleMessage object to locate in the LocaleMessageCollection.</param>
        /// <returns>
        /// <para>true : If LocaleMessage found in LocaleMessageCollection</para>
        /// <para>false : If LocaleMessage found not in LocaleMessageCollection</para>
        /// </returns>
        public bool Contains(LocaleMessage item)
        {
            return this._localeMessages.Contains(item);
        }

        /// <summary>
        /// Copies the elements of the LocaleMessageCollection to an
        ///  System.Array, starting at a particular System.Array index.
        /// </summary>
        /// <param name="array"> 
        ///  The one-dimensional System.Array that is the destination of the elements
        ///  copied from LocaleMessageCollection. The System.Array must have zero-based indexing.
        /// </param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
        public void CopyTo(LocaleMessage[] array, int arrayIndex)
        {
            this._localeMessages.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Get the count of no. of LocaleMessages in LocaleMessageCollection
        /// </summary>
        public int Count
        {
            get
            {
                return this._localeMessages.Count;
            }
        }

        /// <summary>
        /// Check if LocaleMessageCollection is read-only.
        /// </summary>
        public bool IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the LocaleMessageCollection.
        /// </summary>
        /// <param name="item">The LocaleMessage object to remove from the LocaleMessageCollection.</param>
        /// <returns>true if item is successfully removed; otherwise, false. This method also returns false if item was not found in the original LocaleMessageCollection</returns>
        public bool Remove(LocaleMessage item)
        {
            return this._localeMessages.Remove(item);
        }

        #endregion

        #region IEnumerable<LocaleMessage> Members

        /// <summary>
        /// Returns an enumerator that iterates through a LocaleMessageCollection.
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection.</returns>
        public IEnumerator<LocaleMessage> GetEnumerator()
        {
            return this._localeMessages.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        /// <summary>
        /// Returns an enumerator that iterates through a LocaleMessageCollection.
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator)this._localeMessages.GetEnumerator();
        }

        #endregion

        #region ILocaleMessageCollection Members

        #region ToXml methods

        /// <summary>
        /// Get Xml representation of LocaleMessageCollection object
        /// </summary>
        /// <returns>Xml string representing the LocaleMessageCollection</returns>
        public String ToXml()
        {
            String localeMessageXml = String.Empty;
            StringBuilder builder = new StringBuilder();

            foreach (LocaleMessage localeMessage in this._localeMessages)
            {
                builder.Append(localeMessage.ToXml());
            }

            localeMessageXml = String.Format("<LocaleMessages>{0}</LocaleMessages>", builder.ToString());
            return localeMessageXml;
        }

        #endregion ToXml methods

        #region LocaleMessageCollection Get

        /// <summary>
        /// Gets Locale Messages
        /// </summary>                            
        /// <param name="locale">Locale of the localeMessage to search in LoclaeMessages</param>
        /// <param name="systemUILocale">System UI Locale which will be considered when message is not available for the requested locale</param>
        /// <returns>Returns ILocaleMessageCollection </returns>
        public ILocaleMessageCollection Get(LocaleEnum locale, LocaleEnum systemUILocale)
        {
            this.Validate(locale);

            LocaleMessageCollection localeMessageCollection = null;

            IEnumerable<LocaleMessage> localeMessages = FilterLocaleMessages(locale, null, systemUILocale);

            if (localeMessages != null)
                localeMessageCollection = new LocaleMessageCollection(localeMessages.ToList());

            return localeMessageCollection;
        }

        /// <summary>
        /// Gets Locale Message with specified locale and messageCode
        /// </summary>                            
        /// <param name="locale">Locale of the localeMessage to search in LoclaeMessages</param>
        /// <param name="messageCode">Message code of the LocaleMessage</param>
        /// <param name="systemUILocale">System UI Locale which will be considered when message is not available for the requested locale</param>
        /// <returns>LocaleMessage interface</returns>
        public ILocaleMessage Get(LocaleEnum locale, String messageCode, LocaleEnum systemUILocale)
        {
            this.Validate(locale);

            LocaleMessage localeMessage = null;

            IEnumerable<LocaleMessage> localeMessages = FilterLocaleMessages(locale, new Collection<String> { messageCode }, systemUILocale);

            if (localeMessages != null && localeMessages.Count<ILocaleMessage>() > 0)
            {
                localeMessage = localeMessages.First();
            }

            return localeMessage;
        }

        /// <summary>
        /// Gets Locale Messages with specified locale and messageCodes
        /// </summary>                            
        /// <param name="locale">Indicates Locale of the localeMessage to search in LoclaeMessages</param>
        /// <param name="messageCodeList">Indicates Message codes of the LocaleMessages</param>
        /// <param name="systemUILocale">Indicates System UI Locale which will be considered when message is not available for the requested locale</param>
        /// <returns>LocaleMessageCollection interface</returns>
        public ILocaleMessageCollection Get(LocaleEnum locale, Collection<String> messageCodeList, LocaleEnum systemUILocale)
        {
            this.Validate(locale);

            LocaleMessageCollection localeMessageCollection = null;
            IEnumerable<LocaleMessage> localeMessages = FilterLocaleMessages(locale, messageCodeList, systemUILocale);

            if (localeMessages != null)
                localeMessageCollection = new LocaleMessageCollection(localeMessages.ToList());

            return localeMessageCollection;
        }

        #endregion

        #endregion ILocaleMessageCollection Memebers

    }
}