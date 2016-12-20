using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading;
using MDM.Core;
using MDM.BusinessObjects;
using MDM.Utility;
using MDM.MessageManager.Data;

namespace MDM.MessageManager.Business
{
    /// <summary>
    /// Specifies Message manager
    /// </summary>
    public class MessageBL : BusinessLogicBase
    {
        public Boolean Create(Message Message, OperationResult operationResult)
        {
            Boolean MessageDetailProcessed = ProcessMessageDetail(Message, "Log");

            MessageBL BL = new MessageBL();
            Thread TempThread = new Thread(BL.processAsync);
            TempThread.Start(Message);

			return MessageDetailProcessed;
        }

        private void processAsync(object ObjMessage)
        {
            Message Message = (Message)ObjMessage;

            Message.cleanUp();

            Boolean MessageDetailProcessed = ProcessMessageDetail(Message, "Create");

            if (!MessageDetailProcessed)
                return;

            Message.notify();
        }

        private Boolean ProcessMessageDetail(Message Message, string MessageAction)
        {
            String LoginUser = "cfadmin";// SecurityPrincipalHelper.GetCurrentSecurityPrincipal().CurrentUserName;
            String ProgramName = "MessageManager.MessageBL.ProcessMessageDetail";

            MessageDA MessageDA = new MessageDA();
            return MessageDA.ProcessMessageDetail(Message, true, MessageAction, LoginUser, ProgramName);
        }

        public List<Message> getMessages(string IsReadCondition, string StateCondition, string MsgType)
        {
            MessageDA MessageDA = new MessageDA();
            String LoginUser = SecurityPrincipalHelper.GetCurrentSecurityPrincipal().CurrentUserName;
            List<object[]> ValueList = MessageDA.getMessages(LoginUser, IsReadCondition, StateCondition, MsgType);

            List<Message> Messages = ValueList.Select(getMessage).ToList();

            return Messages;
        }

        public Message getBody(int Id)
        {
            MessageDA MessageDA = new MessageDA();
            String LoginUser = SecurityPrincipalHelper.GetCurrentSecurityPrincipal().CurrentUserName;
			String ProgramName = "MessageManager.MessageBL.getBody";

            object[] Values = MessageDA.getBody(Id, LoginUser, ProgramName);
            return getMessage(Values);
        }

        public void getKeyStatistics(out int ReadMessages, out int UnreadMessages, out int PendingMessages)
        {
            MessageDA MessageDA = new MessageDA();
            String LoginUser = SecurityPrincipalHelper.GetCurrentSecurityPrincipal().CurrentUserName;

            MessageDA.getKeyStatistics(LoginUser, out ReadMessages, out UnreadMessages, out PendingMessages);
        }

        public bool markComplete(int Id, string State)
        {
            MessageDA MessageDA = new MessageDA();
            String LoginUser = SecurityPrincipalHelper.GetCurrentSecurityPrincipal().CurrentUserName;
            String ProgramName = "MessageManager.MessageBL.markComplete";

            object[] Values = MessageDA.getBody(Id, LoginUser, ProgramName);
            Message Message1 = getMessage(Values);

            return !Message1.customAction(null) || MessageDA.markComplete(Id, LoginUser, ProgramName, State);
        }

        public Message getMessage(object[] objectArray)
        {
            Message message = new Message();

            if (objectArray[0] is int)
                message.Id = (int)objectArray[0];

            message.MessageFrom = objectArray[1] as string;
            message.Subject = objectArray[2] as string;
            message.Body = objectArray[3] as string;
			if (objectArray[4] != null && objectArray[4] is string)
				message.MessageType = (MessageType)Enum.Parse(typeof(MessageType), objectArray[4] as string);

            if (objectArray[5] is int)
                message.QueueId = (int)objectArray[5];

            if (objectArray[6] != null && objectArray[6] is string)
				message.Priority = (MailPriority)Enum.Parse(typeof(MailPriority), objectArray[6] as string);

            if (objectArray[7] != null && objectArray[7] is bool)
                message.NonRepliable = (bool)objectArray[7];

            if (objectArray[8] != null && objectArray[8] is string)
                message.State = (MessageState)Enum.Parse(typeof(MessageState), objectArray[8] as string);

            if (objectArray[9] != null)
                message.Flag = (MessageFlag)Enum.Parse(typeof(MessageFlag), objectArray[9] as string);

            if (objectArray[10] != null && objectArray[10] is bool)
                message.IsRead = (bool)objectArray[10];

            if (objectArray[11] != null && objectArray[11] is bool)
                message.IsActionRequired = (bool)objectArray[11];

            if (objectArray[12] != null && objectArray[12] is DateTime)
                message.CreateDateTime = (DateTime)objectArray[12];

            return message;
        }
    }
}
