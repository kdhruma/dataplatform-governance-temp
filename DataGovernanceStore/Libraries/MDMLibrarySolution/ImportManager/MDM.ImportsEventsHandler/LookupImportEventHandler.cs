using System;

namespace MDM.ImportsEventsHandler
{
    using MDM.Core;
    using MDM.BusinessObjects.Jobs;
    using MDM.BusinessObjects.Imports;
    using System.Net.Mail;
    using System.Xml;
    using System.Diagnostics;
    using MDM.Utility;

    /// <summary>
    /// Specifies lookup import event handler class
    /// </summary>
    public class LookupImportEventHandler
    {
        /// <summary>
        /// Import started method
        /// </summary>
        /// <param name="job"></param>
        /// <param name="lookupImportProfile"></param>
        /// <returns></returns>
        public static Boolean ImportStarted(Job job, LookupImportProfile lookupImportProfile)
        {
            Boolean returnResult = true;
            String currentEvent = "OnImportStarted";
            String emailRecepients = "";
            MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "ImportEventsHandler; OnImportStarted; Will send.", MDMTraceSource.General);
            try
            {
                emailRecepients = GetEmailRecepients(lookupImportProfile, currentEvent);
                String subject = "Riversand Notification" + job.Id.ToString() + ", " + lookupImportProfile.Name + ", Begin";
                String message = "This is an automated mailer.  Job started at " + System.DateTime.Now.ToString() + ".  Profile Name: " + lookupImportProfile.Name + ". Job Id: " + job.Id.ToString();

                SendEmail(emailRecepients, subject, message);
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "ImportEventsHandler; OnImportStarted; Sent.", MDMTraceSource.General);
            }
            catch (Exception ex)
            {
                ExceptionManager.ExceptionHandler exceptionHander = new ExceptionManager.ExceptionHandler(ex);
            }

            return returnResult;
        }

        private static void SendEmail(string emailRecepients, string subject, string message)
        {
            MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("ImportEventsHandler;{0}", emailRecepients), MDMTraceSource.General);
            MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("ImportEventsHandler;{0}", subject), MDMTraceSource.General);
            MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("ImportEventsHandler;{0}", message), MDMTraceSource.General);
            if (emailRecepients == null || emailRecepients == String.Empty)
                return;

            using (SmtpClient smtpClient = new SmtpClient())
            {
                MailMessage mail = new MailMessage();
                mail.Subject = subject;
                mail.Body = message;
                mail.To.Add(emailRecepients.Replace(";", ","));
                String enablessl = "";
                enablessl = AppConfigurationHelper.GetAppConfig<String>("MDMCenter.Email.EnableSSL");
                if (enablessl.ToLower() == "true")
                    smtpClient.EnableSsl = true;
                else
                    smtpClient.EnableSsl = false;
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("ImportEventsHandler;Enable ssl: {0}", enablessl), MDMTraceSource.General);
                smtpClient.Send(mail);
            }
        }

        private static string GetEmailRecepients(LookupImportProfile lookupImportProfile, string currentEvent)
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
                                if (reader.Value.Equals(lookupImportProfile.Name) && reader.MoveToAttribute(currentEvent))
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

        /// <summary>
        /// Lookup Import completed
        /// </summary>
        /// <param name="job"></param>
        /// <param name="lookupImportProfile"></param>
        /// <returns></returns>
        public static Boolean ImportCompleted(Job job, LookupImportProfile lookupImportProfile, ExecutionStatus executionStatus)
        {
            Boolean returnResult = true;
            String currentEvent = "OnImportCompleted";
            String emailRecepients = "";
            MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "ImportEventsHandler; OnImportCompleted; Will send.", MDMTraceSource.General);
            try
            {
                emailRecepients = GetEmailRecepients(lookupImportProfile, currentEvent);

                string subject = "Riversand Notification" + job.Id.ToString() + ", " + lookupImportProfile.Name + ", Completed";
                string message = "This is an automated mailer.  Job completed at " + System.DateTime.Now.ToString() + ".  Profile Name: " + lookupImportProfile.Name + ". Job Id: " + job.Id.ToString();

                SendEmail(emailRecepients, subject, message);
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "ImportEventsHandler; OnImportCompleted; Sent.", MDMTraceSource.General);
            }
            catch (Exception ex)
            {
                ExceptionManager.ExceptionHandler exceptionHander = new ExceptionManager.ExceptionHandler(ex);
            }

            return returnResult;
        }

        /// <summary>
        /// Lookup Import job aborted
        /// </summary>
        /// <param name="job"></param>
        /// <param name="lookupImportProfile"></param>
        /// <param name="executionStatus"></param>
        /// <returns></returns>
        public static Boolean ImportAborted(Job job, LookupImportProfile lookupImportProfile, ExecutionStatus executionStatus)
        {
            Boolean returnResult = true;
            String currentEvent = "OnImportAborted";
            String emailRecepients = "";
            try
            {
                emailRecepients = GetEmailRecepients(lookupImportProfile, currentEvent);

                String subject = "Riversand Notification" + job.Id.ToString() + ", " + lookupImportProfile.Name + ", Aborted";
                String message = "This is an automated mailer.  Job aborted at " + System.DateTime.Now.ToString() + ".  Profile Name: " + lookupImportProfile.Name + ". Job Id: " + job.Id.ToString();

                SendEmail(emailRecepients, subject, message);
            }
            catch (Exception ex)
            {
                ExceptionManager.ExceptionHandler exceptionHander = new ExceptionManager.ExceptionHandler(ex);
            }

            return returnResult;
        }
    }
}
