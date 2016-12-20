using System;
using System.Xml;
using System.IO;

namespace MDM.ExceptionManager.Handlers
{
    //Doing this would give importance to our internal types than .net types
    using MDM.ExceptionManager.Config;

    /// <summary>
    /// Writes errors occurring in ExM to a xml text file.
    /// </summary>
    public class FileLogHandler
    {
        #region Fields

        private ModuleSettings moduleSettings = null;

        #endregion

        #region Constructors
  
        /// <summary>
        /// Initializes a new instance of the FileLogHandler class
        /// </summary>		
        public FileLogHandler()
        {
            moduleSettings = ModuleConfig.GetSettings;
        }

        /// <summary>
        /// Initializes a new instance of the FileLogHandler class using specified module settings file path.
        /// </summary>
        /// <param name="filepath">Module setting file path</param>
        public FileLogHandler(string filepath)
        {
            moduleSettings = ModuleConfig.GetSettingsFromFile(filepath);
        }
        #endregion

        #region Properties



        #endregion

        #region Methods

        /// <summary>
        /// Logs Application Errors.
        /// </summary>
        /// <param name="errorSource">The source that caused the error.</param>
        /// <param name="errorMessage">The error message.</param>
        public void LogError(String errorSource, String errorMessage)
        {
            try
            {
                String logFileName = String.Empty;

                logFileName = moduleSettings.ErrorLog;

                //if file does not exit create it
                if (!File.Exists(logFileName))
                {
                    /*
                        * Open the Application Error log file and add a url node to it.
                        * 
                        * <ExceptionManagerLog></ExceptionManagerLog>
                        * 
                    */
                    using (StreamWriter writer = new StreamWriter(logFileName))
                    {
                        String xmlContent = "<?xml version=\"1.0\" encoding=\"utf-8\"?><ExceptionManagerLog/>"; ;
                        writer.Write(xmlContent);
                    }
                }

                //if file exists check if it's writable before writing
                if (File.Exists(logFileName))
                {
                    //check if the file is readonly
                    if (File.GetAttributes(logFileName) != FileAttributes.Normal)
                    {
                        File.SetAttributes(logFileName, FileAttributes.Normal);
                    }
                    XmlDocument appLog = new XmlDocument();

                    appLog.Load(logFileName);

                    XmlElement msgRoot = appLog.CreateElement("message");

                    XmlAttribute src = appLog.CreateAttribute("source");
                    src.InnerText = errorSource;

                    XmlAttribute errorDate = appLog.CreateAttribute("date");
                    errorDate.InnerText = DateTime.Now.ToString();

                    msgRoot.SetAttributeNode(src);
                    msgRoot.SetAttributeNode(errorDate);
                    msgRoot.InnerText = errorMessage;

                    appLog.DocumentElement.AppendChild(msgRoot);

                    appLog.Save(logFileName);
                }
            }
            catch
            {
                //don't throw
            }
        }
        #endregion
    }
}