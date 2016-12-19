using System;
using System.Text;
using System.Runtime.Serialization;
using System.Collections.ObjectModel;
using System.IO;
using System.Xml;

namespace MDM.BusinessObjects
{
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Specifies the locale message class
    /// </summary>
    [DataContract]
    [KnownType(typeof(MDMObject))]
    public class LocaleMessage : MDMObject, ILocaleMessage, IBusinessRuleObject
    {
        #region Fields

        /// <summary>
        /// Field for Locale Message code
        /// </summary>
        private String _code = String.Empty;

        /// <summary>
        /// Field for Locale Message Class
        /// </summary>
        private MessageClassEnum _messageClass = MessageClassEnum.UnKnown;

        /// <summary>
        /// Field for Locale Message
        /// </summary>
        private String _message = String.Empty;

        /// <summary>
        /// Field for Locale Message  Description
        /// </summary>
        private String _description = String.Empty;

        /// <summary>
        /// Field for knowledge base article
        /// </summary>
        private String _kbaLink = String.Empty;

        /// <summary>
        /// Field for help link for locale message
        /// </summary>
        private String _helpLink = String.Empty;

        /// <summary>
        /// Field for where used for locale message
        /// </summary>
        private String _whereUsed = String.Empty;

        /// <summary>
        /// Field for original locale message
        /// </summary>
        private LocaleMessage _originalLocaleMessage = null;

        #endregion

        #region Constructor

        /// <summary>
        /// Parameterless Constructor
        /// </summary>
        public LocaleMessage()
            : base()
        {
        }

        /// <summary>
        /// Constructor which takes Xml as input
        /// </summary>
        public LocaleMessage(String valueAsXml)
        {
            LoadLocaleMessage(valueAsXml);
        }

        /// <summary>
        ///   Constructor with Id, code,messageClass, message,description, KBALink and locale
        /// </summary>
        /// <param name="id">Indicates the identifier of the locale message</param>
        /// <param name="code">Indicates the code of the locale message</param>
        /// <param name="messageClass">Indicates the message class of locale message</param>
        /// <param name="message">Indicates the message of locale</param>
        /// <param name="description">Indicates the description of locale</param>
        /// <param name="kbaLink">Indicates the kba for locale message</param>
        /// <param name="locale">Indicates the locale </param>
        public LocaleMessage(Int32 id, String code, MessageClassEnum messageClass, String message, String description, String kbaLink, LocaleEnum locale)
            : base(id, String.Empty, String.Empty, locale)
        {
            this._code = code;
            this._messageClass = messageClass;
            this._message = message;
            this._description = description;
            this._kbaLink = kbaLink;
        }

        /// <summary>
        ///   Constructor with Id, code,messageClass, message,description, KBALink and locale
        /// </summary>
        /// <param name="id">Indicates the identifier of the locale message</param>
        /// <param name="code">Indicates the code of the locale message</param>
        /// <param name="messageClass">Indicates the message class of locale message</param>
        /// <param name="message">Indicates the message of locale</param>
        /// <param name="description">Indicates the description of locale</param>
        /// <param name="helpLink">Indicates the help link for locale message</param>
        /// <param name="locale">Indicates the locale </param>
        /// <param name="whereUsed">Indicates the where used field of locale message</param>
        public LocaleMessage(Int32 id, String code, MessageClassEnum messageClass, String message, String description, String helpLink, LocaleEnum locale, String whereUsed)
            : this(id, code, messageClass, message, description, String.Empty, locale)
        {
            this._helpLink = helpLink;
            this._whereUsed = whereUsed;
        }

        #endregion Constructor

        #region Properties

        /// <summary>
        /// Property denoting code of the locale message
        /// </summary>
        [DataMember]
        public String Code
        {
            get
            {
                return this._code;
            }
            set
            {
                this._code = value;
            }
        }

        /// <summary>
        /// Property denoting class of the locale message
        /// </summary>
        [DataMember]
        public MessageClassEnum MessageClass
        {
            get
            {
                return this._messageClass;
            }
            set
            {
                this._messageClass = value;
            }
        }

        /// <summary>
        /// Property denoting locale messsage text in the locale specified
        /// </summary>
        [DataMember]
        public String Message
        {
            get
            {
                return this._message;
            }
            set
            {
                this._message = value;
            }
        }

        /// <summary>
        /// Property denoting Description of the locale message
        /// </summary>
        [DataMember]
        public String Description
        {
            get
            {
                return this._description;
            }
            set
            {
                this._description = value;
            }
        }

        /// <summary>
        /// Property denoting knowledge base article of the locale message
        /// </summary>
        [DataMember]
        public String KBALink
        {
            get
            {
                return this._kbaLink;
            }
            set
            {
                this._kbaLink = value;
            }
        }

        /// <summary>
        /// Property denoting help link of the locale message
        /// </summary>
        [DataMember]
        public String HelpLink
        {
            get
            {
                return this._helpLink;
            }
            set
            {
                this._helpLink = value;
            }
        }

        /// <summary>
        /// Property denoting where used of the locale message
        /// </summary>
        [DataMember]
        public String WhereUsed
        {
            get
            {
                return this._whereUsed;
            }
            set
            {
                this._whereUsed = value;
            }
        }

        /// <summary>
        /// Property denoting original locale messge 
        /// </summary>
        [DataMember]
        public LocaleMessage OriginalLocaleMessage
        {
            get { return _originalLocaleMessage; }
            set { _originalLocaleMessage = value; }
        }
        #endregion

        #region Methods

        #region Public Methods

        #region ToXml methods

        /// <summary>
        /// Get Xml representation of LocaleMessage object
        /// </summary>
        /// <returns>Xml representation of object</returns>
        public new String ToXml()
        {
            String localeMessageXml = String.Empty;
            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            //LocaleMessage node start
            xmlWriter.WriteStartElement("LocaleMessage");

            #region write LocaleMessage properties for full LocaleMessage xml

            xmlWriter.WriteAttributeString("Id", this.Id.ToString());
            xmlWriter.WriteAttributeString("MessageCode", this.Code);
            xmlWriter.WriteAttributeString("MessageClass", this.MessageClass.ToString());
            xmlWriter.WriteAttributeString("Message", this.Message);
            xmlWriter.WriteAttributeString("Description", this.Description);
            xmlWriter.WriteAttributeString("KBALink", this.KBALink);
            xmlWriter.WriteAttributeString("HelpLink", this.HelpLink);
            xmlWriter.WriteAttributeString("WhereUsed", this.WhereUsed);
            xmlWriter.WriteAttributeString("Locale", base.Locale.ToString());
            xmlWriter.WriteAttributeString("Action", this.Action.ToString());

            //LocaleMessage node end
            xmlWriter.WriteEndElement();

            xmlWriter.Flush();

            //get the actual XML
            localeMessageXml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return localeMessageXml;
        }

        /// <summary>
        /// Load Locale Message
        /// </summary>
        /// <param name="valuesAsXml">Input xml Locale object</param>
        public void LoadLocaleMessage(String valuesAsXml)
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
                            if (reader.HasAttributes)
                            {
                                if (reader.MoveToAttribute("Id"))
                                {
                                    this.Id = reader.ReadContentAsInt();
                                }

                                if (reader.MoveToAttribute("MessageCode"))
                                {
                                    this.Code = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("MessageClass"))
                                {
                                    MessageClassEnum messageClassEnum = MessageClassEnum.UnKnown;
                                    Enum.TryParse(reader.ReadContentAsString(), out messageClassEnum);
                                    this.MessageClass = messageClassEnum;
                                }
                                if (reader.MoveToAttribute("Message"))
                                {
                                    this.Message = reader.ReadContentAsString();
                                }
                                if (reader.MoveToAttribute("Description"))
                                {
                                    this.Description = reader.ReadContentAsString(); ;
                                }
                                if (reader.MoveToAttribute("KBALink"))
                                {
                                    this.KBALink = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("HelpLink"))
                                {
                                    this.HelpLink = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("WhereUsed"))
                                {
                                    this.WhereUsed = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("Locale"))
                                {
                                    LocaleEnum locale = LocaleEnum.UnKnown;
                                    Enum.TryParse(reader.ReadContentAsString(), out locale);
                                    this.Locale = locale;
                                }

                                if (reader.MoveToAttribute("Action"))
                                {
                                    this.Action = ValueTypeHelper.GetAction(reader.ReadContentAsString());
                                }
                            }
                        }
                        else
                        {
                            //Keep on reading the xml until we reach expected node.
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

            #endregion

        #endregion

        #region Clone Methods

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ILocaleMessage Clone()
        {
            var clonedLocaleMessage = new LocaleMessage
            {
                Id = this.Id,
                Locale = this.Locale,
                _code = this._code,
                _messageClass = this._messageClass,
                _message = this._message,
                _description = this._description,
                _kbaLink = this._kbaLink,
                _helpLink = this._helpLink,
                _whereUsed = this._whereUsed
            };

            return clonedLocaleMessage;
        }

        /// <summary>
        /// Compares and Merges the current locale message with original message
        /// </summary>
        /// <param name="deltaLocaleMessage">Indicates the message to be compared and merged</param>
        /// <param name="returnClonedObject">Indicates whether message clone is required for compare and return</param>
        /// <returns>Returns locale message after merging the required changes</returns>
        public LocaleMessage MergeDelta(LocaleMessage deltaLocaleMessage, Boolean returnClonedObject = true)
        {
            LocaleMessage mergedLocaleMessage = (returnClonedObject == true) ? (LocaleMessage)deltaLocaleMessage.Clone() : deltaLocaleMessage;

            mergedLocaleMessage.Action = (this.Equals(mergedLocaleMessage)) ? ObjectAction.Read : ObjectAction.Update;

            return mergedLocaleMessage;
        }

        /// <summary>
        /// Determines whether specified object is equal to the current object.
        /// </summary>
        /// <param name="obj">Indicates DDG locale message object which needs to be compared.</param>
        /// <returns>Returns true if the locale messages are equal, otherwise returns false.</returns>
        public override Boolean Equals(object obj)
        {
            if (obj is LocaleMessage)
            {
                LocaleMessage objectToBeCompared = obj as LocaleMessage;

                if (!base.Equals(objectToBeCompared))
                {
                    return false;
                }

                if (this.Locale != objectToBeCompared.Locale)
                {
                    return false;
                }

                if (this.Code != objectToBeCompared.Code)
                {
                    return false;
                }

                if(this.MessageClass != objectToBeCompared.MessageClass)
                {
                    return false;
                }
                if(this.Message != objectToBeCompared.Message)
                {
                    return false;
                }
                if(this.Description != objectToBeCompared.Description)
                {
                    return false;
                }
                if(this.HelpLink != objectToBeCompared.HelpLink)
                {
                    return false;
                }
                if (this.WhereUsed != objectToBeCompared.WhereUsed)
                {
                    return false;
                }

                return true;
            }

            return false;
        }

        /// <summary>
        /// Determines whether current object is superset of specified object.
        /// </summary>
        /// <param name="obj">Indicates DDG locale message object which needs to be compared.</param>
        /// <returns>Returns true if the current object locale messages is superset of specified object, otherwise returns false.</returns>
        public Boolean IsSuperSetOf(object obj)
        {
            if (obj is LocaleMessage)
            {
                LocaleMessage objectToBeCompared = obj as LocaleMessage;

                if (!base.IsSuperSetOf(objectToBeCompared))
                {
                    return false;
                }

                if (this.Locale != objectToBeCompared.Locale)
                {
                    return false;
                }

                if (String.Compare(this.Code, objectToBeCompared.Code) != 0)
                {
                    return false;
                }

                if (this.MessageClass != objectToBeCompared.MessageClass)
                {
                    return false;
                }
                if (String.Compare(this.Message, objectToBeCompared.Message) != 0)
                {
                    return false;
                }
                if (String.Compare(this.Description, objectToBeCompared.Description) != 0)
                {
                    return false;
                }
                if (String.Compare(this.HelpLink, objectToBeCompared.HelpLink) != 0)
                {
                    return false;
                }
                if (String.Compare(this.WhereUsed, objectToBeCompared.WhereUsed) != 0)
                {
                    return false;
                }

                return true;
            }

            return false;
        }
        /// <summary>
        /// Returns the hash code for this instance
        /// </summary>
        /// <returns>Hash code of the instance</returns>
        public override Int32 GetHashCode()
        {
            return base.GetHashCode();
        }

        #endregion
        
        #endregion

        #region Private Methods

        #endregion

        #endregion

    }
}


