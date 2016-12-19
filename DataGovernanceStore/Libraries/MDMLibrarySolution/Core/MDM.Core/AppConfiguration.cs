using System;
using System.Web;
using System.Configuration;

namespace MDM.Core
{
    /// <summary>
    /// Contains methods to access appSettings config values.
    /// </summary>
    public sealed class AppConfiguration
    {
        #region Fields

        #endregion

        #region Constructors

        /// <summary>
        /// Contains static methods. Should not be instanciated.
        /// </summary>		
        private AppConfiguration()
        {

        }

        #endregion

        #region Properties

        #endregion

        #region Methods

        /// <summary>
        /// <para>Get the appSettings value present in the web.config file.</para>
        /// </summary>
        /// <param name="key">The name of the Key to read from appSettings node.</param>
        /// <returns>Value of key passed as an argument</returns>
        public static String GetSetting(string key)
        {

            String output = String.Empty;
            
            //get the value
            output = ConfigurationManager.AppSettings[key];

            //if (output == null || output.Equals(String.Empty))
            //{
            //    throw new ArgumentException("The configuration did not have a value for the key", key);
            //}
            return output;

        }

        /// <summary>
        /// <para>Get the appSettings value present in the web.config file.</para>
        /// </summary>
        /// <param name="key">The name of the Key to read from appSettings node.</param>
        /// <returns>Value of key passed as an argument</returns>
        public static Int32 GetSettingAsInteger(string key)
        {
            String value = GetSetting(key);
            Int32 output = 0;
            Int32.TryParse(value, out output);
            return output;
        }

        /// <summary>
        /// <para>Get the appSettings value present in the web.config file.</para>
        /// </summary>
        /// <param name="key">The name of the Key to read from appSettings node.</param>
        /// <returns>Value of key passed as an argument</returns>
        public static Boolean GetSettingAsBoolean(string key)
        {
            String value = GetSetting(key);
            Boolean output = false;
            bool result = Boolean.TryParse(value, out output);

            if (!result)
            {
                throw new ArgumentException("Not able to convery key value to boolean", value);
            }

            return output;
        }

        #endregion
    }
}
