using System;
using System.Xml;
using System.Xml.Serialization;

namespace MDM.ExceptionManager.Config
{
    /// <summary>
    /// Contains Database settings for logging exceptions.
    /// </summary>
    public class DatabaseSettings
    {
        #region Fields

        private String _connectionString = String.Empty;
        private String _appConfigKey = String.Empty;

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor.
        /// </summary>
        public DatabaseSettings()
        {
        }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="xmlData"></param>
        public DatabaseSettings(String xmlData)
        {
            /*
             * 	<DatabaseSettings>
             *   <AppConfigKey>ConnectionString</AppConfigKey>
             *   <ConnectionString></ConnectionString>
             *  </DatabaseSettings>
             * */
            XmlTextReader reader = null;
            try
            {
                reader = new XmlTextReader(xmlData, XmlNodeType.Element, null);

                while (reader.Read())
                {
                    if (reader.IsStartElement())
                    {
                        //Dont read the start element <DatabaseSettings>
                        if (reader.Depth == 1)
                        {
                            reader.MoveToContent();
                            ConnectionString = reader.ReadElementContentAsString();
                            reader.MoveToContent();
                            AppConfigKey = reader.ReadElementContentAsString();
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
        /// Connection String 
        /// </summary>
        /// <value>Returns the connection string.</value>
        [XmlElement]
        public String ConnectionString
        {
            get
            {
                return _connectionString;
            }
            set
            {
                _connectionString = value;
            }
        }

        /// <summary>
        /// The app configuration key entry that has the connection string
        /// </summary>
        [XmlElement]
        public String AppConfigKey
        {
            get
            {
                return _appConfigKey;
            }
            set
            {
                _appConfigKey = value;
            }
        }
        #endregion

        #region Methods
        #endregion
    }
}
