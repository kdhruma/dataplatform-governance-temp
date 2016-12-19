using System;
using System.Web;
using System.Net;
using System.Net.Mail;

namespace MDM.ExceptionManager.Handlers
{
    //Doing this would give importance to our internal types than .net types
    using MDM.ExceptionManager.Config;

    /// <summary>
    /// Handles email using built-in MailMessage class.
    /// </summary>
    public class MailHandler
    {
        #region Fields

        private ModuleSettings moduleSettings = null;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the MailHandler class.
        /// </summary>
        public MailHandler()
        {
            moduleSettings = ModuleConfig.GetSettings;
        }

        /// <summary>
        /// Initializes a new instance of the MailHandler class with the specified module settings object.
        /// </summary>
        /// <param name="filepath">Indicates the path of file</param>
        public MailHandler(string filepath)
        {
            moduleSettings = ModuleConfig.GetSettingsFromFile(filepath);
        }

        #endregion

        #region Properties



        #endregion

        #region Methods

        /// <summary>
        /// Sends an Email to the persons specified in the config.
        /// </summary>
        /// <param name="message">The exception data which forms the body of the email message</param>
        public void SendMail(String message)
        {
            MailMessage email = null;
            String smtpServer = String.Empty;
            SmtpClient mailClient = null;

            email = new MailMessage(moduleSettings.GetEmailSettings.MailFrom, moduleSettings.GetEmailSettings.MailTo, moduleSettings.GetEmailSettings.Subject, message);

            if (moduleSettings.GetEmailSettings.MailCC != null && moduleSettings.GetEmailSettings.MailCC.Length != 0)
            {
                email.CC.Add(new MailAddress(moduleSettings.GetEmailSettings.MailCC));
            }

            smtpServer = moduleSettings.GetEmailSettings.SmtpServer;
            mailClient = new SmtpClient();
            mailClient.Credentials = CredentialCache.DefaultNetworkCredentials;

            //use smtp server only if its required else let the
            //local iis smtp server relay it to the company mail server on the network.
            if (smtpServer != null && smtpServer.Length != 0)
            {
                mailClient.Host = smtpServer;
            }

            mailClient.Send(email);
        }
        #endregion

    }
}
