using System;
using System.Xml;
using System.Xml.Serialization;

namespace MDM.ExceptionManager.Config
{
    /// <summary>
    /// <para>This is used to custom serialize how the 
    /// module settings are stored in the config file.</para>
    /// <para>Should contains all the properties (xml nodes) in the config setting</para>
    /// </summary>
    /// <remarks>
    /// The xml is case sensitive so care should be taken to have the same case 
    /// thats in the xml file.
    /// </remarks>
    public class ModuleSettings
    {
        #region Fields

        //node level variables
        private bool _useExManagerSwitch = false;
        private bool _logToEvtVrSwitch = false;
        private bool _logToDBSwitch = false;
        private bool _sendEmailSwitch = false;
        private String _exceptionUrl = String.Empty;
        private String _errorLogFileName = String.Empty;

        //individual settings
        private LogDataSettings _dataSettings = null;
        private EventViewerSettings _eventViewerSettings = null;
        private DatabaseSettings _dbSettings = null;
        private EmailSettings _mailSettings = null;

        #endregion

        #region Constructors
        /// <summary>
        /// Default constructor. Initializes a new instance of the ModuleSettings class.
        /// </summary>
        public ModuleSettings()
        {
        }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="xmlData"></param>
        public ModuleSettings(String xmlData)
        {
            //This comes with the <ModuleSettings> root note
            XmlTextReader reader = null;
            try
            {
                reader = new XmlTextReader(xmlData, XmlNodeType.Element, null);

                while (reader.Read())
                {
                    if (reader.IsStartElement())
                    {
                        //Dont read the start element <ModuleSettings>
                        if (reader.Depth == 1)
                        {
                            reader.MoveToContent();
                            UseExceptionManager = reader.ReadElementContentAsBoolean();
                            reader.MoveToContent();
                            LogToEventViewer = reader.ReadElementContentAsBoolean();
                            reader.MoveToContent();
                            LogToDB = reader.ReadElementContentAsBoolean();
                            reader.MoveToContent();
                            SendEmail = reader.ReadElementContentAsBoolean();
                            reader.MoveToContent();
                            ExceptionUrl = reader.ReadElementContentAsString();
                            reader.MoveToContent();

                            ErrorLog = ModuleWebConfig.ConvertToAbsolutePath(reader.ReadElementContentAsString());
                            reader.MoveToContent();
                            String logDataSettingsXml = reader.ReadOuterXml();
                            _dataSettings = new LogDataSettings(logDataSettingsXml);

                            reader.MoveToContent();
                            String eventViewerSettingsXml = reader.ReadOuterXml();
                            _eventViewerSettings = new EventViewerSettings(eventViewerSettingsXml);

                            reader.MoveToContent();
                            String databaseSettingsXml = reader.ReadOuterXml();
                            _dbSettings = new DatabaseSettings(databaseSettingsXml);

                            reader.MoveToContent();
                            String emailSettingsXml = reader.ReadOuterXml();
                            _mailSettings = new EmailSettings(emailSettingsXml);
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
        /// <para>Specifies whether to use the Exception Manager.</para>
        /// </summary>
        /// <remarks>Top Level Switch, will turn off all other settings.</remarks>
        /// <value>Returns boolean indicating the use of Exception Manager.</value>

        [XmlElement]
        public bool UseExceptionManager
        {
            get
            {
                return _useExManagerSwitch;
            }
            set
            {
                _useExManagerSwitch = value;
            }
        }

        /// <summary>
        /// <para>Specifies whether to Log into Event Viewer.</para>
        /// </summary>
        /// <value>Returns boolean indicating logging to Event Viewer.</value>
        [XmlElement]
        public bool LogToEventViewer
        {
            get
            {
                return _logToEvtVrSwitch;
            }
            set
            {
                _logToEvtVrSwitch = value;
            }
        }

        /// <summary>
        /// <para>Specifies whether to Log to Database.</para>
        /// </summary>
        /// <value>Returns boolean indicating logging to Database.</value>
        [XmlElement]
        public bool LogToDB
        {
            get
            {
                return _logToDBSwitch;
            }
            set
            {
                _logToDBSwitch = value;
            }
        }

        /// <summary>
        /// <para>Specifies whether to send Email</para>
        /// </summary>
        /// <value>Returns boolean indicating sending of Email.</value>		
        [XmlElement]
        public bool SendEmail
        {
            get
            {
                return _sendEmailSwitch;
            }
            set
            {
                _sendEmailSwitch = value;
            }
        }

        /// <summary>
        /// <para>Specifies the URL where the exception can be viewed</para>
        /// </summary>
        /// <value>Returns URL of the exception manager application</value>		
        [XmlElement]
        public String ExceptionUrl
        {
            get
            {
                return _exceptionUrl;
            }
            set
            {
                _exceptionUrl = value;
            }
        }

        /// <summary>
        /// <para>Specifies the file which ExM uses to write its own errors.</para>
        /// </summary>
        /// <value>Returns the error log file name</value>		
        [XmlElement]
        public String ErrorLog
        {
            get
            {
                return _errorLogFileName;
            }
            set
            {
                _errorLogFileName = value;
            }
        }

        /// <summary>
        /// <para>Obtains Log Data settings containing the information that should be 
        /// logged.</para>
        /// </summary>
        /// <value>Returns Log Data Object settings.</value>
        [XmlElement(ElementName = "LogDataSettings")]
        public LogDataSettings GetLogDataSettings
        {
            get
            {
                return _dataSettings;
            }
            set
            {
                _dataSettings = value;
            }
        }

        /// <summary>
        /// <para>Obtains Event Viewer settings.</para>
        /// </summary>
        /// <value>Returns Event Viewer Object settings.</value>
        [XmlElement(ElementName = "EventViewerSettings")]
        public EventViewerSettings GetEventViewerSettings
        {
            get
            {
                return _eventViewerSettings;
            }
            set
            {
                _eventViewerSettings = value;
            }
        }

        /// <summary>
        /// <para>Obtains Database settings.</para>
        /// </summary>
        /// <value>Returns Database Object settings.</value>
        [XmlElement(ElementName = "DatabaseSettings")]
        public DatabaseSettings GetDatabaseSettings
        {
            get
            {
                return _dbSettings;
            }
            set
            {
                _dbSettings = value;
            }
        }

        /// <summary>
        /// <para>Obtains Email settings.</para>
        /// </summary>
        /// <value>Returns Email Object settings.</value>
        [XmlElement(ElementName = "EmailSettings")]
        public EmailSettings GetEmailSettings
        {
            get
            {
                return _mailSettings;
            }
            set
            {
                _mailSettings = value;
            }
        }

        #endregion

        #region Methods
        #endregion
    }
}
