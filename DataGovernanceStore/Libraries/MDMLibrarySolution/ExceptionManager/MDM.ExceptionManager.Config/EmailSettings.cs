using System;
using System.Xml;
using System.Xml.Serialization;

namespace MDM.ExceptionManager.Config
{
    /// <summary>
    /// Contains settings for sending email containing error data.
    /// </summary>
    public class EmailSettings
    {
        #region Fields

        private string _emailFrom = String.Empty;
        private string _emailTo = String.Empty;
        private string _emailCC = String.Empty;
        private string _subject = String.Empty;
        private string _mailServer = String.Empty;

        #endregion

        #region Constructors

		/// <summary>
		/// 
		/// </summary>
        public EmailSettings()
        {
        }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="xmlData"></param>
        public EmailSettings(String xmlData)
        {
            /*
	         *   <EmailSettings>
		     *       <MailFrom>client@riversand.com</MailFrom>
		     *       <MailTo>support@riversand.com</MailTo>
		     *       <MailCC>supportHeadGroup@riversand.com</MailCC>
		     *       <Subject>Web Application Error</Subject>
		     *       <SmtpServer></SmtpServer>	
	         *   </EmailSettings>
            * */
            XmlTextReader reader = null;
            try
            {
                reader = new XmlTextReader(xmlData, XmlNodeType.Element, null);

                while (reader.Read())
                {
                    if (reader.IsStartElement())
                    {
                        //Dont read the start element <EmailSettings>
                        if (reader.Depth == 1)
                        {
                            reader.MoveToContent();
                            MailFrom = reader.ReadElementContentAsString();
                            reader.MoveToContent();
                            MailTo = reader.ReadElementContentAsString();
                            reader.MoveToContent();
                            MailCC = reader.ReadElementContentAsString();
                            reader.MoveToContent();
                            Subject = reader.ReadElementContentAsString();
                            reader.MoveToContent();
                            SmtpServer = reader.ReadElementContentAsString();
                        }
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

        #endregion

        #region Properties

        /// <summary>
        /// <para>Specifies From address for email</para>
        /// </summary>
        /// <value>Returns Mail From Address</value>
        [XmlElement]
        public string MailFrom
        {
            get
            {
                return _emailFrom;
            }
            set
            {
                _emailFrom = value;
            }
        }

        /// <summary>
        /// <para>Specifies To address for email</para>
        /// </summary>
        /// <value>Returns Mail To Address</value>
        [XmlElement]
        public string MailTo
        {
            get
            {
                return _emailTo;
            }
            set
            {
                _emailTo = value;
            }
        }

        /// <summary>
        /// <para>Specifies the Carbon Copy address of email</para>
        /// </summary>
        /// <value>Returns Mail CC address</value>
        [XmlElement]
        public string MailCC
        {
            get
            {
                return _emailCC;
            }
            set
            {
                _emailCC = value;
            }
        }

        /// <summary>
        /// <para>Specifies the subject of email</para>
        /// </summary>
        /// <value>Returns Mail Subject</value>
        [XmlElement]
        public string Subject
        {
            get
            {
                return _subject;
            }
            set
            {
                _subject = value;
            }
        }

        /// <summary>
        /// <para>Specifies the SMTP server to send mail</para>
        /// </summary>
        /// <value>Returns name of SMTP server</value>
        [XmlElement]
        public string SmtpServer
        {
            get
            {
                return _mailServer;
            }
            set
            {
                _mailServer = value;
            }
        }

        #endregion

        #region Methods
        #endregion
    }
}
