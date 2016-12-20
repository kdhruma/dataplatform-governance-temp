using System;
using System.Xml;
using System.Xml.Serialization;

namespace MDM.ExceptionManager.Config
{
    /// <summary>
    /// Specifies various data elements that have to be logged.
    /// </summary>
    public class LogDataSettings
    {
        #region Fields

        bool formCollectionSwitch = false;
        bool serverVariablesSwitch = false;
        bool queryStringSwitch = false;
        bool exceptionSourceSwitch = false;
        bool exceptionMessageSwitch = false;
        bool exceptionTargetSiteSwitch = false;
        bool exceptionStackTraceSwitch = false;
        bool webServiceExceptionSwitch = false;

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor.
        /// </summary>
        public LogDataSettings()
        {
        }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="xmlData"></param>
        public LogDataSettings(String xmlData)
        {
            /*
             * 	<LogDataSettings>
		     *   <FormCollection>true</FormCollection>
		     *   <ServerVariables>true</ServerVariables>
		     *   <QueryString>true</QueryString>
		     *   <ExceptionSource>true</ExceptionSource>
		     *   <ExceptionMessage>true</ExceptionMessage>
		     *   <ExceptionTargetSite>true</ExceptionTargetSite>
		     *   <ExceptionStackTrace>true</ExceptionStackTrace>
		     *   <WebServiceException>false</WebServiceException>
	         *  </LogDataSettings>
             * 
             * */
            XmlTextReader reader = null;
            try
            {
                reader = new XmlTextReader(xmlData, XmlNodeType.Element, null);

                while (reader.Read())
                {
                    if (reader.IsStartElement())
                    {
                        //Dont read the start element <LogDataSettings>
                        if (reader.Depth == 1)
                        {
                            reader.MoveToContent();
                            FormCollection = reader.ReadElementContentAsBoolean();
                            reader.MoveToContent();
                            ServerVariables = reader.ReadElementContentAsBoolean();
                            reader.MoveToContent();
                            QueryString = reader.ReadElementContentAsBoolean();
                            reader.MoveToContent();
                            ExceptionSource = reader.ReadElementContentAsBoolean();
                            reader.MoveToContent();
                            ExceptionMessage = reader.ReadElementContentAsBoolean();
                            reader.MoveToContent();
                            ExceptionTargetSite = reader.ReadElementContentAsBoolean();
                            reader.MoveToContent();
                            ExceptionStackTrace = reader.ReadElementContentAsBoolean();
                            reader.MoveToContent();
                            WebServiceException = reader.ReadElementContentAsBoolean();
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
        /// <para>Specifies whether to log forms collection.</para>
        /// </summary>
        /// <value>Returns boolean indicating logging forms collections.</value>
        [XmlElement]
        public bool FormCollection
        {
            get
            {
                return formCollectionSwitch;
            }
            set
            {
                formCollectionSwitch = value;
            }
        }

        /// <summary>
        /// <para>Specifies whether to log server variables.</para>
        /// </summary>
        /// <value>Returns boolean indicating logging server variables.</value>
        [XmlElement]
        public bool ServerVariables
        {
            get
            {
                return serverVariablesSwitch;
            }
            set
            {
                serverVariablesSwitch = value;
            }
        }

        /// <summary>
        /// <para>Specifies whether to log Query String.</para>
        /// </summary>
        /// <value>Returns boolean indicating logging Query String.</value>
        [XmlElement]
        public bool QueryString
        {
            get
            {
                return queryStringSwitch;
            }
            set
            {
                queryStringSwitch = value;
            }
        }

        /// <summary>
        /// <para>Specifies whether to log Exception source.</para>
        /// </summary>
        /// <value>Returns boolean indicating logging Exception source.</value>
        [XmlElement]
        public bool ExceptionSource
        {
            get
            {
                return exceptionSourceSwitch;
            }
            set
            {
                exceptionSourceSwitch = value;
            }
        }

        /// <summary>
        /// <para>Specifies whether to log Exception Message.</para>
        /// </summary>
        /// <value>Returns boolean indicating logging Exception Message.</value>
        [XmlElement]
        public bool ExceptionMessage
        {
            get
            {
                return exceptionMessageSwitch;
            }
            set
            {
                exceptionMessageSwitch = value;
            }
        }

        /// <summary>
        /// <para>Specifies whether to log Exception TargetSite.</para>
        /// </summary>
        /// <value>Returns boolean indicating logging Exception TargetSite.</value>
        [XmlElement]
        public bool ExceptionTargetSite
        {
            get
            {
                return exceptionTargetSiteSwitch;
            }
            set
            {
                exceptionTargetSiteSwitch = value;
            }
        }

        /// <summary>
        /// <para>Specifies whether to log Exception StackTrace.</para>
        /// </summary>
        /// <value>Returns boolean indicating logging Exception StackTrace.</value>
        [XmlElement]
        public bool ExceptionStackTrace
        {
            get
            {
                return exceptionStackTraceSwitch;
            }
            set
            {
                exceptionStackTraceSwitch = value;
            }
        }

        /// <summary>
        /// <para>Specifies whether to log Web Service Exception.</para>
        /// </summary>
        /// <value>Returns boolean indicating logging Web Service Exception.</value>
        [XmlElement]
        public bool WebServiceException
        {
            get
            {
                return webServiceExceptionSwitch;
            }
            set
            {
                webServiceExceptionSwitch = value;
            }
        }
        #endregion

        #region Methods
        #endregion
    }
}
