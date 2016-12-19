using System;
using System.Drawing;
using System.Activities;
using System.ComponentModel;
using System.Net.Mail;

using MDM.Core;
using MDM.BusinessObjects;
using MDM.BusinessObjects.Workflow;
using MDM.MessageManager.Business;
using MDM.Workflow.Activities.Designer;

namespace MDM.Workflow.Activities.Core
{
    /// <summary>
    /// Sends MDM Message
    /// </summary>
    [Designer(typeof(SendMDMMessageDesigner))]
    [ToolboxBitmap(typeof(SendMDMMessageDesigner), "Images.SendMDMMessage.bmp")]
    public class SendMDMMessage : MDMCodeActivitiyBase<OperationResult>
    {
		#region Fields

		private MessageType _messageType = MessageType.Info;
		private MailPriority _mailPriority = MailPriority.Normal;

		#endregion

		#region Members

		[DisplayName("Message Type")]
		[Category("Message Properties")]
		public MessageType MessageType
		{
			get { return _messageType; }
			set { _messageType = value; }
		}

		[Category("Message Properties")]
		[RequiredArgument]
		public InArgument<String> From { get; set; }

		[Category("Message Properties")]
		[RequiredArgument]
		public InArgument<String> To { get; set; }

		[Category("Message Properties")]
		[RequiredArgument]
		public InArgument<String> Subject { get; set; }

		[Category("Message Properties")]
		[RequiredArgument]
		public InArgument<String> Body { get; set; }

		[Category("Message Properties")]
		public MailPriority Priority
		{
			get { return _mailPriority; }
			set { _mailPriority = value; }
		}

        /// <summary>
        /// Action Context in workflow client, provides action details for Activity
        /// </summary>
        [Browsable(false)]
        public new OutArgument<WorkflowActionContext> MDMActionContext { get; set; }

        #endregion

        protected override OperationResult Execute(CodeActivityContext context)
        {
            OperationResult operationResult = new OperationResult();

            try
            {
                Message message = new Message();
                message.MessageFrom = From.Get(context);
                message.MessageTo = To.Get(context);
                message.Subject = Subject.Get(context);
                message.Body = Body.Get(context);
                message.Priority = Priority;
                message.NonRepliable = true;
                message.MessageType = MessageType;

                MessageBL messageManager = new MessageBL();

                messageManager.Create(message, operationResult);
            }
            catch (Exception ex)
            {
                Error error = new Error();
                error.ErrorMessage = ex.Message;
                operationResult.Errors.Add(error);
            }

            return operationResult;
        }
    }
}
