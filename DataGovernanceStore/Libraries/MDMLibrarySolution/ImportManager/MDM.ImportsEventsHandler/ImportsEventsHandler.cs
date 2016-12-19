using System;
using System.Diagnostics;
using System.Net.Mail;
using System.Xml;

namespace MDM.ImportsEventsHandler
{
    using MDM.Core;
    using MDM.BusinessObjects;
    using MDM.BusinessObjects.Jobs;
    using MDM.BusinessObjects.Imports;
    using MDM.Utility;

    /// <summary>
    /// Specifies the import event handler class.
    /// </summary>
    public class ImportsEventsHandler
    {
        /// <summary>
        /// Import started method
        /// </summary>
        /// <param name="job"></param>
        /// <param name="importProfile"></param>
        /// <returns></returns>
        public static Boolean ImportStarted(Job job, ImportProfile importProfile)
        {
            Boolean returnResult = true;
            String currentEvent = "OnImportStarted";
            String emailRecepients = "";
            MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "ImportEventsHandler; OnImportStarted; Will send.", MDMTraceSource.Imports);
            try
            {
                emailRecepients = GetEmailRecepients(importProfile, currentEvent);
                String subject = "Riversand Notification for Job Id: " + job.Id.ToString() + ", Profile: " + importProfile.Name + ", Begin";
                String message = "This is an automated mailer. <BR/> Job started at " + System.DateTime.Now.ToString() + ". <BR/> Profile Name: " + importProfile.Name + ". <BR/> Job Id: " + job.Id.ToString();

                sendEmail(emailRecepients, subject, message);
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "ImportEventsHandler; OnImportStarted; Sent.", MDMTraceSource.Imports);
            }
            catch (Exception ex)
            {
                ExceptionManager.ExceptionHandler exceptionHander = new ExceptionManager.ExceptionHandler(ex);
            }

            return returnResult;
        }

        /// <summary>
        /// Import completed
        /// </summary>
        /// <param name="job"></param>
        /// <param name="importProfile"></param>
        /// <returns></returns>
        public static Boolean ImportCompleted(Job job, ImportProfile importProfile, ExecutionStatus executionStatus)
        {
            Boolean returnResult = true;
            String currentEvent = "OnImportCompleted";
            String emailRecepients = "";
            MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "ImportEventsHandler; OnImportCompleted; Will send.", MDMTraceSource.Imports);
            try
            {
                emailRecepients = GetEmailRecepients(importProfile, currentEvent);

                string subject = "Riversand Notification for Job Id: " + job.Id.ToString() + ", Profile: " + importProfile.Name + ", Completed";
                string message = "This is an automated mailer. <BR/> Job completed at " + System.DateTime.Now.ToString() + ". <BR/> Profile Name: " + importProfile.Name + ". <BR/> Job Id: " + job.Id.ToString();

                sendEmail(emailRecepients, subject, message);
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "ImportEventsHandler; OnImportCompleted; Sent.", MDMTraceSource.Imports);
            }
            catch (Exception ex)
            {
                ExceptionManager.ExceptionHandler exceptionHander = new ExceptionManager.ExceptionHandler(ex);
            }

            return returnResult;
        }

        /// <summary>
        /// Import job aborted
        /// </summary>
        /// <param name="job"></param>
        /// <param name="importProfile"></param>
        /// <param name="executionStatus"></param>
        /// <returns></returns>
        public static Boolean ImportAborted(Job job, ImportProfile importProfile, ExecutionStatus executionStatus)
        {
            Boolean returnResult = true;
            String currentEvent = "OnImportAborted";
            String emailRecepients = "";
            try
            {
                emailRecepients = GetEmailRecepients(importProfile, currentEvent);

                String subject = "Riversand Notification for Job Id: " + job.Id.ToString() + ", Profile: " + importProfile.Name + ", Aborted";
                String message = "This is an automated mailer. <BR/> Job aborted at " + System.DateTime.Now.ToString() + ". <BR/> Profile Name: " + importProfile.Name + ". <BR/> Job Id: " + job.Id.ToString();

                sendEmail(emailRecepients, subject, message);
            }
            catch (Exception ex)
            {
                ExceptionManager.ExceptionHandler exceptionHander = new ExceptionManager.ExceptionHandler(ex);
            }

            return returnResult;
        }

        /// <summary>
        /// Import batch started. The entity collection has the list of all entities that came from the source.
        /// </summary>
        /// <param name="entityCollection"></param>
        /// <param name="entityOperationResultCollection"></param>
        /// <param name="job"></param>
        /// <param name="importProfile"></param>
        /// <param name="executionStatus"></param>
        /// <returns></returns>
        public static Boolean ImportBatchStarted(EntityCollection entityCollection, EntityOperationResultCollection entityOperationResultCollection, Job job, ImportProfile importProfile)
        {
            Boolean returnResult = true;
            String currentEvent = "OnImportBatchStarted";
            String emailRecepients = "";
            try
            {
                emailRecepients = GetEmailRecepients(importProfile, currentEvent);

                string subject = "Riversand Notification for Job Id: " + job.Id.ToString() + ", Profile: " + importProfile.Name + ", batch started";
                string message = "This is an automated mailer. <BR/> Job batch started at " + System.DateTime.Now.ToString() + ". <BR/> Profile Name: " + importProfile.Name + ". <BR/> Job Id: " + job.Id.ToString();

                sendEmail(emailRecepients, subject, message);
            }
            catch (Exception ex)
            {
                ExceptionManager.ExceptionHandler exceptionHander = new ExceptionManager.ExceptionHandler(ex);
            }

            return returnResult;
        }

        /// <summary>
        /// Import batch started. The entity collection has the list of all entities that came from the source. The entity operation result has the information
        /// about which ones passed the validations.
        /// </summary>
        /// <param name="entityCollection"></param>
        /// <param name="entityOperationResultCollection"></param>
        /// <param name="job"></param>
        /// <param name="importProfile"></param>
        /// <param name="executionStatus"></param>
        /// <returns></returns>
        public static Boolean ImportBatchProcessStarted(EntityCollection entityCollection, EntityOperationResultCollection entityOperationResultCollection, Job job, ImportProfile importProfile)
        {
            Boolean returnResult = true;
            String currentEvent = "OnImportBatchProcessStarted";
            String emailRecepients = "";
            try
            {
                emailRecepients = GetEmailRecepients(importProfile, currentEvent);

                String subject = "Riversand Notification for Job Id: " + job.Id.ToString() + ", Profile: " + importProfile.Name + ", batch process started";
                String message = "This is an automated mailer. <BR/> Job batch process started at " + System.DateTime.Now.ToString() + ". <BR/> Profile Name: " + importProfile.Name + ". <BR/> Job Id: " + job.Id.ToString();

                sendEmail(emailRecepients, subject, message);
            }
            catch (Exception ex)
            {
                ExceptionManager.ExceptionHandler exceptionHander = new ExceptionManager.ExceptionHandler(ex);
            }

            return returnResult;
        }

        /// <summary>
        /// Import batch completed
        /// </summary>
        /// <param name="entityCollection"></param>
        /// <param name="entityOperationResultCollection"></param>
        /// <param name="job"></param>
        /// <param name="importProfile"></param>
        /// <returns></returns>
        public static Boolean ImportBatchProcessCompleted(EntityCollection entityCollection, EntityOperationResultCollection entityOperationResultCollection, Job job, ImportProfile importProfile)
        {
            Boolean returnResult = true;
            String currentEvent = "OnImportBatchProcessCompleted";
            String emailRecepients = "";
            try
            {
                emailRecepients = GetEmailRecepients(importProfile, currentEvent);

                String subject = "Riversand Notification for Job Id: " + job.Id.ToString() + ", Profile: " + importProfile.Name + ", batch process completed";
                String message = "This is an automated mailer. <BR/> Job batch process completed at " + System.DateTime.Now.ToString() + ". <BR/> Profile Name: " + importProfile.Name + ". <BR/> Job Id: " + job.Id.ToString();

                sendEmail(emailRecepients, subject, message);
            }
            catch (Exception ex)
            {
                ExceptionManager.ExceptionHandler exceptionHander = new ExceptionManager.ExceptionHandler(ex);
            }

            return returnResult;
        }

        /// <summary>
        /// Import batch completed
        /// </summary>
        /// <param name="entityCollection"></param>
        /// <param name="entityOperationResultCollection"></param>
        /// <param name="job"></param>
        /// <param name="importProfile"></param>
        /// <returns></returns>
        public static Boolean ImportBatchCompleted(EntityCollection entityCollection, EntityOperationResultCollection entityOperationResultCollection, Job job, ImportProfile importProfile)
        {
            Boolean returnResult = true;
            String currentEvent = "OnImportBatchCompleted";
            String emailRecepients = "";
            try
            {
                emailRecepients = GetEmailRecepients(importProfile, currentEvent);

                String subject = "Riversand Notification for Job Id: " + job.Id.ToString() + ", Profile: " + importProfile.Name + ", batch completed";
                String message = "This is an automated mailer. <BR/> Job batch completed at " + System.DateTime.Now.ToString() + ". <BR/> Profile Name: " + importProfile.Name + ". <BR/> Job Id: " + job.Id.ToString();

                sendEmail(emailRecepients, subject, message);
            }
            catch (Exception ex)
            {
                ExceptionManager.ExceptionHandler exceptionHander = new ExceptionManager.ExceptionHandler(ex);
            }

            return returnResult;
        }

		private static void sendEmail(string emailRecepients, string subject, string message)
		{
			MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("sendEmail method, emailRecepients: {0}, subject: {1}, message: {2}", emailRecepients, subject, message), MDMTraceSource.Imports);

			if (emailRecepients == null || emailRecepients == String.Empty)
				return;

			using (SmtpClient smtpClient = new SmtpClient())
			{
				MailMessage mail = new MailMessage();
				mail.Subject = subject;
				mail.Body = message;
				mail.IsBodyHtml = true;
				mail.To.Add(emailRecepients.Replace(";", ","));

				String enablessl = "";
				enablessl = AppConfigurationHelper.GetAppConfig<String>("MDMCenter.Email.EnableSSL");
				if (enablessl.ToLower() == "true")
					smtpClient.EnableSsl = true;
				else
					smtpClient.EnableSsl = false;
				MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("ImportEventsHandler;Enable ssl: {0}", enablessl), MDMTraceSource.Imports);

				smtpClient.Send(mail);
			}
		}

		private static string GetEmailRecepients(ImportProfile importProfile, string currentEvent)
		{
			#region Sample Xml
			/*
                 <ImportNotifications>
                    <ImportProfile name=\"Profile1\" OnImportStarted=\"\" OnImportCompleted=\"\" OnImportAborted=\"\" OnImportBatchStarted=\"\" OnImportBatchProcessStarted=\"\" OnImportBatchProcessCompleted=\"\" OnImportBatchCompleted=\"\" />
                    <ImportProfile name=\"Profile2\" OnImportStarted=\"\" OnImportCompleted=\"\" OnImportAborted=\"\" OnImportBatchStarted=\"\" OnImportBatchProcessStarted=\"\" OnImportBatchProcessCompleted=\"\" OnImportBatchCompleted=\"\" />
                    <ImportProfile name=\"Profile3\" OnImportStarted=\"\" OnImportCompleted=\"\" OnImportAborted=\"\" OnImportBatchStarted=\"\" OnImportBatchProcessStarted=\"\" OnImportBatchProcessCompleted=\"\" OnImportBatchCompleted=\"\" />
                    <ImportProfile name=\"Profile4\" OnImportStarted=\"\" OnImportCompleted=\"\" OnImportAborted=\"\" OnImportBatchStarted=\"\" OnImportBatchProcessStarted=\"\" OnImportBatchProcessCompleted=\"\" OnImportBatchCompleted=\"\" />
                 </ImportNotifications>
                 
                 
                 */
			#endregion Sample Xml

			String ImportNotificationsXML = AppConfigurationHelper.GetAppConfig<String>("MDMCenter.ImportNotifications.Config");
			String OnImportStartedEmail = "";
			if (!String.IsNullOrWhiteSpace(ImportNotificationsXML))
			{
				XmlTextReader reader = null;
				try
				{
					reader = new XmlTextReader(ImportNotificationsXML, XmlNodeType.Element, null);

					while (!reader.EOF)
					{
						if (reader.NodeType == XmlNodeType.Element && reader.Name == "ImportProfile")
						{
							if (reader.HasAttributes && reader.MoveToAttribute("name"))
							{
								if (reader.Value.Equals(importProfile.Name) && reader.MoveToAttribute(currentEvent))
								{
									OnImportStartedEmail = reader.ReadContentAsString();
									if (!String.IsNullOrWhiteSpace(OnImportStartedEmail))
									{
										return OnImportStartedEmail;

									}
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
			return OnImportStartedEmail;
		}
	}
}
