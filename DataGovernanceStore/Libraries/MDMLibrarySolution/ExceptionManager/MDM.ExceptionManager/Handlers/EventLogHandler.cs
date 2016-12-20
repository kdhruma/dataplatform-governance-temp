using System;
using System.Diagnostics;

namespace MDM.ExceptionManager.Handlers
{
    //Doing this would give importance to our internal types than .net types
    using MDM.ExceptionManager.Config;

    /// <summary>
    /// Represents event log handler used for logging events on server's event log.
    /// </summary>
    public class EventLogHandler
    {
        #region Fields

        private static ModuleSettings _moduleSettings = null;

        private static Object _lockObj = new Object();

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the EventLogHandler class.
        /// </summary>
        public EventLogHandler()
        {
            if (_moduleSettings == null)
            {
                lock (_lockObj)
                {
                    if (_moduleSettings == null)
                    {
                        _moduleSettings = ModuleConfig.GetSettings;
                    }
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the EventLogHandler class with the specified module settings file path.
        /// </summary>
        /// <param name="filepath">Indicates file path</param>
        public EventLogHandler(string filepath)
        {
            _moduleSettings = ModuleConfig.GetSettingsFromFile(filepath);
        }

        /// <summary>
        /// Initializes a new instance of the EventLogHandler class with the specified module settings object.
        /// </summary>
        /// <param name="config">Indicates module settings</param>
        public EventLogHandler(ModuleSettings config)
        {
            _moduleSettings = config;
        }

        #endregion

        #region Properties
        #endregion

        #region Methods

        /// <summary>
        /// Writes Exception Data to the Event Log as Error
        /// </summary>
        /// <param name="message">The message consisting of different values set in config file.</param>
        /// <param name="eventID">The ExceptionID returned from the database else a default value of 0.</param>
        public void WriteErrorLog(String message, Int32 eventID)
        {
            //TODO: The category information has to be added for specific module errors.
            WriteLog(message, eventID, EventLogEntryType.Error);
        }

        /// <summary>
        /// Writes Exception Data to the Event Log as Warning
        /// </summary>
        /// <param name="message">The message consisting of different values set in config file.</param>
        /// <param name="eventID">The ExceptionID returned from the database else a default value of 0.</param>
        public void WriteWarningLog(String message, Int32 eventID)
        {
            //TODO: The category information has to be added for specific module errors.
            WriteLog(message, eventID, EventLogEntryType.Warning);
        }

        /// <summary>
        /// Writes Exception Data to the Event Log as Information
        /// </summary>
        /// <param name="message">The message consisting of different values set in config file.</param>
        /// <param name="eventID">The ExceptionID returned from the database else a default value of 0.</param>
        public void WriteInformationLog(String message, Int32 eventID)
        {
            //TODO: The category information has to be added for specific module errors.
            WriteLog(message, eventID, EventLogEntryType.Information);
        }

        /// <summary>
        /// Create an Event Log
        /// </summary>
        public void Create()
        {
            try
            {
                String logName = _moduleSettings.GetEventViewerSettings.LogName;
                String sourceName = _moduleSettings.GetEventViewerSettings.SourceName;
                Int64 MaxLogSize = _moduleSettings.GetEventViewerSettings.MaxLogSize;

                EventSourceCreationData creationData = null;
                creationData = new EventSourceCreationData(sourceName, logName);

                if (!EventLog.SourceExists(sourceName))
                {
                    EventLog.CreateEventSource(creationData);
                }

                EventLog[] eventLogs = EventLog.GetEventLogs();
                foreach (EventLog evntlog in eventLogs)
                {
                    if (evntlog.Log == logName)
                    {
                        evntlog.MaximumKilobytes = MaxLogSize;
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                //errorlog text file
                //if there is an error writing to this file
                //the error will be passed to the web application.
                FileLogHandler fileHandler = new FileLogHandler();
                fileHandler.LogError("Event Log", ex.ToString());
            }
        }

        /// <summary>
        /// Delete an Event Log
        /// </summary>
        public void Delete()
        {
            String logName = _moduleSettings.GetEventViewerSettings.LogName;
            String sourceName = _moduleSettings.GetEventViewerSettings.SourceName;

            if (EventLog.SourceExists(sourceName))
            {
                EventLog.DeleteEventSource(sourceName);
                EventLog.Delete(logName);
            }
        }

        private void WriteLog(String message, Int32 eventID, EventLogEntryType type)
        {
            //TODO: The category information has to be added for 
            //specific module errors.
            try
            {
                String logName = _moduleSettings.GetEventViewerSettings.LogName;
                String sourceName = _moduleSettings.GetEventViewerSettings.SourceName;
                Int64 MaxLogSize = _moduleSettings.GetEventViewerSettings.MaxLogSize;

                if (!EventLog.SourceExists(sourceName))
                {
                    //create the log
                    Create();
                }

                //Write to the log
                EventLog.WriteEntry(sourceName, message, type, eventID);
            }
            catch (Exception ex)
            {
                //error-log text file
                //if there is an error writing to this file
                //the error will be passed to the web application.
                FileLogHandler fileHandler = new FileLogHandler();
                fileHandler.LogError("Event Log", ex.Message);
            }
        }

        #endregion
    }
}
