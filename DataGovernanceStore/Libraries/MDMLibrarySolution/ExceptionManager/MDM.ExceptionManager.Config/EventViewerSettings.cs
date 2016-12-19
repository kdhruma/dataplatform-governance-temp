using System;
using System.Xml;
using System.Xml.Serialization;

namespace MDM.ExceptionManager.Config
{
    /// <summary>
    /// Contain settings specific to Event Viewer.
    /// </summary>
    public class EventViewerSettings
    {
        #region Fields

        string _logName = String.Empty;
        string _logSourceName = String.Empty;
        Int64 _MaxLogSize = 51200;

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor.
        /// </summary>
        public EventViewerSettings()
        {
        }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="xmlData"></param>
        public EventViewerSettings(String xmlData)
        {
            /*	
             * 
             * <EventViewerSettings>
		     *   <LogName>RiversandMDMCenter</LogName>
		     *   <SourceName>MDM Web System</SourceName>
             *   <MaximumKilobytes>51200</MaximumKilobytes>
	         * </EventViewerSettings>
             * */
            XmlTextReader reader = null;
            try
            {
                reader = new XmlTextReader(xmlData, XmlNodeType.Element, null);

                while (reader.Read())
                {
                    if (reader.IsStartElement())
                    {
                        //Dont read the start element <EventViewerSettings>
                        if (reader.Depth == 1)
                        {
                            reader.MoveToContent();
                            LogName = reader.ReadElementContentAsString();
                            reader.MoveToContent();
                            SourceName = reader.ReadElementContentAsString();
                            reader.MoveToContent();
                            MaxLogSize = reader.ReadElementContentAsLong();
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
        /// <para>Specifies the name of custom log in Event Viewer</para>
        /// </summary>
        /// <value>Returns name of the custom log</value>	
        [XmlElement]
        public string LogName
        {
            get
            {
                return _logName;
            }
            set
            {
                _logName = value;
            }
        }

        /// <summary>
        /// <para>Specifies the name of source that logs entries Event Viewer</para>
        /// </summary>
        /// <value>Returns name of the custom log source</value>
        [XmlElement]
        public string SourceName
        {
            get
            {
                return _logSourceName;
            }
            set
            {
                _logSourceName = value;
            }
        }

        /// <summary>
        /// <para>Specifies the maximum size of EventLog</para>
        /// </summary>
        /// <value>Returns maximum size of the custom EventLog</value>
        [XmlElement]
        public Int64 MaxLogSize
        {
            get
            {
                return _MaxLogSize;
            }
            set
            {
                _MaxLogSize = value;
            }
        }

        #endregion

        #region Methods

        #endregion
    }
}
