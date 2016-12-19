using System;
using System.Net.Mail;
using System.Runtime.Serialization;
using System.Text;
using MDM.Core;

namespace MDM.BusinessObjects
{
    /// <summary>
    /// Specifies the message class
    /// </summary>
    [DataContract]
    [KnownType(typeof(MDMObject))]
    public class Message : MDMObject
    {
		#region Fields

		private MessageType _MessageType;
        private int _QueueId;
        private string _MessageFrom;
        private string _MessageTo;
        private string _Subject;
        private string _Body;
		private MailPriority _Priority;
		private MessageState _State;
        private MessageFlag _Flag;
        private bool _NonRepliable;
        private DateTime _CreateDateTime;
        private bool _IsRead;
        private bool _IsActionRequired;

		#endregion

		#region Constructors

        /// <summary>
        /// Parameterless constructor
        /// </summary>
		public Message() { }

        /// <summary>
        /// Constructor with id of message as input parameter
        /// </summary>
        /// <param name="id">Indicates identifier of message</param>
		public Message(Int32 id) : base(id) { }

        /// <summary>
        /// Constructor with messageType as input parameter
        /// </summary>
        /// <param name="messageType">Indicates type of message</param>
		public Message(string messageType)
		{
			switch (messageType)
			{
				case "Info":
					MessageType = MessageType.Info;
					State = MessageState.Complete;
					IsRead = false;
					break;
				case "Alert":
					MessageType = MessageType.Alert;
					State = MessageState.Complete;
					IsRead = false;
					break;
				case "Workflow":
					MessageType = MessageType.Workflow;
					State = MessageState.Pending;
					IsRead = false;
					break;
				case "Custom":
					MessageType = MessageType.Custom;
					State = MessageState.Complete;
					IsRead = false;
					break;
				case "CustomWithAction":
					MessageType = MessageType.Custom;
					State = MessageState.Pending;
					IsRead = false;
					break;
			}
		}

    	#endregion

		#region Property

        /// <summary>
        /// Indicates the identifier of queue of message
        /// </summary>
		[DataMember]
        public int QueueId
        {
            get { return _QueueId; }
            set { _QueueId = value; }
        }

        /// <summary>
        /// Indicates the type of message
        /// </summary>
        [DataMember]
        public MessageType MessageType
        {
            get { return _MessageType; }
            set { _MessageType = value; }
        }

        /// <summary>
        /// Indicates the mail address of the owner of message
        /// </summary>
        [DataMember]
        public string MessageFrom
        {
            get { return _MessageFrom; }
            set { _MessageFrom = value; }
        }

        /// <summary>
        /// Indicates the mail address of the receiver of message
        /// </summary>
        [DataMember]
        public string MessageTo
        {
            get { return _MessageTo; }
            set { _MessageTo = value; }
        }

        /// <summary>
        /// Indicates the subject of message
        /// </summary>
        [DataMember]
        public string Subject
        {
            get { return _Subject; }
            set { _Subject = value; }
        }

        /// <summary>
        /// Indicates the body of message
        /// </summary>
        [DataMember]
        public string Body
        {
            get { return _Body; }
            set { _Body = value; }
        }

        /// <summary>
        /// Indicates the priority of message
        /// </summary>
        [DataMember]
		public MailPriority Priority
        {
            get { return _Priority; }
            set { _Priority = value; }
        }

        /// <summary>
        /// Indicates the state of message
        /// </summary>
        [DataMember]
        public MessageState State
        {
            get { return _State; }
            set { _State = value; }
        }

        /// <summary>
        /// Indicates the flag of message indicating the status of message
        /// </summary>
        [DataMember]
        public MessageFlag Flag
        {
            get { return _Flag; }
            set { _Flag = value; }
        }

        /// <summary>
        /// Indicates whether the message is repliable or not
        /// </summary>
        [DataMember]
        public bool NonRepliable
        {
            get { return _NonRepliable; }
            set { _NonRepliable = value; }
        }

        /// <summary>
        /// Indicates the created datetime of message
        /// </summary>
        [DataMember]
        public DateTime CreateDateTime
        {
            get { return _CreateDateTime; }
            set { _CreateDateTime = value; }
        }

        /// <summary>
        /// Indicates if the message is readable or not
        /// </summary>
        [DataMember]
        public bool IsRead
        {
            get { return _IsRead; }
            set { _IsRead = value; }
        }

        /// <summary>
        /// Indicates if any action is required for the message
        /// </summary>
        [DataMember]
        public bool IsActionRequired
        {
            get { return _IsActionRequired; }
            set { _IsActionRequired = value; }
        }

		#endregion

		#region Methods

        /// <summary>
        /// Specifies whether the message has custom action
        /// </summary>
		public virtual bool HasCustomAction
        {
            get { return false; }
        }

        /// <summary>
        /// Overridden ToString method to return Generic MDM Message 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "Generic MDM Message";
        }

        /// <summary>
        /// Represents message in Xml format
        /// </summary>
        /// <returns></returns>
        public new virtual String ToXml()
        {
            StringBuilder builder = new StringBuilder();

            builder.Append(" MessageType=\"" + MessageType + "\"");
            builder.Append(" Id=\"" + Id + "\"");
            builder.Append(" MessageFrom=\"" + MessageFrom + "\"");
            builder.Append(" MessageTo=\"" + MessageTo + "\"");
            builder.Append(" Subject=\"" + System.Security.SecurityElement.Escape(Subject) + "\"");
            builder.Append(" Body=\"" + System.Security.SecurityElement.Escape(Body) + "\"");
            builder.Append(" Priority=\"" + Priority + "\"");
            builder.Append(" State=\"" + State + "\"");
            builder.Append(" Flag=\"" + Flag + "\"");
            builder.Append(" NonRepliable=\"" + (NonRepliable?1:0) + "\"");
            builder.Append(" IsRead=\"" + (IsRead?1:0) + "\"");
            builder.Append(" IsActionRequired=\"" + (IsActionRequired?1:0) + "\"");

            String MessageXml = String.Format("<Message{0}></Message>", builder);

            return MessageXml;
        }

        /// <summary>
        /// Returns the body of message in formatted way,
        /// </summary>
        /// <returns></returns>
        public virtual String getBodyFormatted()
        {
            return Body;
        }

        /// <summary>
        /// Specifies the custom action
        /// </summary>
        /// <param name="Paramaters">Indicates the parameter object array</param>
        /// <returns>Default return value is true</returns>
        public virtual bool customAction(params object[] Paramaters)
        {
            return true;
        }

        /// <summary>
        /// To do any cleanup on the body before storing in the main table
        /// </summary>
        public virtual void cleanUp()
        {

        }

        /// <summary>
        /// To initiate any notification
        /// </summary>
        public virtual void notify()
        {

        }

		#endregion
	}
}
