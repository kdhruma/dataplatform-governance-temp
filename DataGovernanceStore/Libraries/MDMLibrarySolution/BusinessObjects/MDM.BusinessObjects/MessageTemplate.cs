using System;
using System.Net.Mail;
using System.Runtime.Serialization;
using System.Xml;
using MDM.Core;

namespace MDM.BusinessObjects
{
    /// <summary>
    /// Contains definition of MDMMessage Template
    /// </summary>
    [DataContract]
    public class MessageTemplate : MDMObject
    {
        #region Fields

		private String _messageSubject;
		private String _messageBody;
		private String _mdmObjectContentTemplate;
		private String _returnAttributes;
		private MessageType _messageType;
		private MailPriority _priority;
		private Boolean _consolidateMessages;
		private Boolean _disabled;

        #endregion

        #region Constructors

        /// <summary>
        /// Parameterless constructor
        /// </summary>
        public MessageTemplate() { }

        /// <summary>
        /// Constructor which takes message template as an input parameter
        /// </summary>
        /// <param name="MessageTemplate"></param>
        public MessageTemplate(string MessageTemplate)
        {
            if (!String.IsNullOrWhiteSpace(MessageTemplate))
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(MessageTemplate);

                XmlNodeList nodes = doc.SelectNodes("MessageTemplate");

                if (nodes.Count > 0)
                {
                    Name = nodes[0].Attributes["Name"].Value;
                    MessageSubject = nodes[0].Attributes["MessageSubject"].Value;
                    MessageBody = nodes[0].Attributes["MessageBody"].Value;
                    MDMObjectContentTemplate = nodes[0].Attributes["MDMObjectContentTemplate"].Value;
                    ReturnAttributes = nodes[0].Attributes["ReturnAttributes"].Value;
                    MessageType = (MessageType)Enum.Parse(typeof(MessageType), nodes[0].Attributes["MessageType"].Value);
                    Priority = (MailPriority)Enum.Parse(typeof(MailPriority), nodes[0].Attributes["MessagePriority"].Value);
                    ConsolidateMessages = nodes[0].Attributes["ConsolidateMessages"].Value == "1" ? true : false;
                    Disabled = nodes[0].Attributes["Disabled"].Value == "1" ? true : false;
                }
            }
        }

        #endregion

        #region Property
        
        /// <summary>
        /// The MessgeSubject holds the subject of the Message. 
        /// </summary>
        [DataMember]
        public String MessageSubject
        {
            get { return _messageSubject; }
            set { _messageSubject = value; }
        }

        /// <summary>
        /// The MessageBody holds the body of the message.
        /// </summary>
        [DataMember]
        public String MessageBody
        {
            get { return _messageBody; }
            set { _messageBody = value; }
        }

        /// <summary>
        /// MDMObjectContentTemplate is also a part of MessageBody but mainly used to embed URL in the message body. 
        /// </summary>
        [DataMember]
        public String MDMObjectContentTemplate
        {
            get { return _mdmObjectContentTemplate; }
            set { _mdmObjectContentTemplate = value; }
        }

        /// <summary>
        /// ReturnAttributes Property holds the attribute Ids list which are used in the templates.
        /// </summary>
        [DataMember]
        public String ReturnAttributes
        {
            get { return _returnAttributes; }
            set { _returnAttributes = value; }
        }

        /// <summary>
        /// The MessageType describes the type of message. The supported messagetypes in the system are Info, Alert, Workflow and Custom.
        /// </summary>
        [DataMember]
        public MessageType MessageType
        {
            get { return _messageType; }
            set { _messageType = value; }
        }

        /// <summary>
        /// The Priority denotes the priority of the mail.
        /// </summary>
        [DataMember]
        public MailPriority Priority
        {
            get { return _priority; }
            set { _priority = value; }
        }

        /// <summary>
        /// ConsolidateMessages flag holds information whether to send the consolidated mail for all the Entities requested or separate mails for each Entity.
        /// </summary>
        [DataMember]
        public Boolean ConsolidateMessages
        {
            get { return _consolidateMessages; }
            set { _consolidateMessages = value; }
        }

        /// <summary>
        /// Property is for disabling the mail template.
        /// </summary>
        [DataMember]
        public Boolean Disabled
        {
            get { return _disabled; }
            set { _disabled = value; }
        }

        #endregion

        #region Methods

        #endregion
    }
}
