using System;
using System.Drawing;
using System.Activities;
using System.ComponentModel;
using System.Net.Mail;

using MDM.Core;
using MDM.BusinessObjects;
using MDM.BusinessObjects.Workflow;
using MDM.Workflow.Activities.Designer;

namespace MDM.Workflow.Activities.Core
{
    /// <summary>
    /// Sends a mail
    /// </summary>
    [Designer(typeof(SendMailDesigner))]
    [ToolboxBitmap(typeof(SendMailDesigner), "Images.SendMail.bmp")]
	public class SendMail : MDMCodeActivitiyBase<OperationResult>
    {
		#region Fields

		private MailFormat _mailFormat = MailFormat.HTML;
		private MailPriority _mailPriority = MailPriority.Normal;

		#endregion

		#region Members

		[DisplayName("EMail Format")]
		[Category("EMail Properties")]
		public MailFormat MailFormat
		{
			get { return _mailFormat; }
			set { _mailFormat = value; }
		}

		[Category("EMail Properties")]
		[RequiredArgument]
		public InArgument<String> From { get; set; }

		[Category("EMail Properties")]
		[RequiredArgument]
		public InArgument<String> To { get; set; }

		[Category("EMail Properties")]
		public InArgument<String> Cc { get; set; }

		[Category("EMail Properties")]
		[RequiredArgument]
		public InArgument<String> Subject { get; set; }

		[Category("EMail Properties")]
		[RequiredArgument]
		public InArgument<String> Body { get; set; }

		[Category("EMail Properties")]
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
            	MailMessage message = new MailMessage(new MailAddress(From.Get(context)), new MailAddress(To.Get(context)));
				message.IsBodyHtml = (MailFormat == MailFormat.HTML) ? true : false;
                if (!String.IsNullOrWhiteSpace(Cc.Get(context)))
                    message.CC.Add(Cc.Get(context));
            	message.Subject = Subject.Get(context);
            	message.Body = Body.Get(context);
            	message.Priority = Priority;

                SmtpClient smtpClient = new SmtpClient();
				smtpClient.Send(message);
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
