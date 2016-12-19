using System;
using System.Configuration;
using System.IO;
using System.Web;

namespace MDM.ExceptionManager.Config
{
    /// <summary>
    /// <para>This is used to get all values that are present in appSettings node 
    /// in the web.config file </para>
    /// </summary>
    public sealed class ModuleWebConfig
    {
        #region Fields
        #endregion

        #region Constructors
        /// <summary>
        /// Default constructor. Initializes a new instance of the ModuleWebConfig class.
        /// </summary>
        private ModuleWebConfig()
        {
        }
        #endregion

        #region Properties

        #endregion

        #region Methods
        /// <summary>
        /// <para>Get the AppSettings value present in the web.config file.</para>
        /// </summary>
        /// <param name="key">The key setting in appSettings node.</param>
        /// <returns>String containing the value based on Mode Switch</returns>
        public static string GetAppSetting(string key)
        {
            String configValue = String.Empty;

            //get the value
            configValue = ConfigurationManager.AppSettings[key];
            //configValue = @"C:\Riversand Technologies, Inc\CytecApp\Application\Config\ExceptionManager.Config";
            return configValue;
        }

        /// <summary>
        /// <para>Get the AppSettings value present in the web.config file.</para>
        /// </summary>
        /// <param name="key">The key setting in appSettings node.</param>
        /// <returns>String containing the value based on Mode Switch</returns>
        public static string GetAppSettingAbsolutePath(string key)
        {
            String returnValue = String.Empty;
            String appSettingValue = String.Empty;

            //get the value
            appSettingValue = GetAppSetting(key);

            //since the values would be entered in relative path keeping
            //the calling application (website or winapp)in mind
            //the path conversion can be done here.

            //System.AppDomain.CurrentDomain.BaseDirectory will work for both WebApp and WinForms
            //for Web App, it gives you the base directory path mapped to web application
            //for WinForms, it gives you the path where exe is
            returnValue = String.Format("{0}\\{1}", System.AppDomain.CurrentDomain.BaseDirectory, appSettingValue);

            return returnValue;
        }

        /// <summary>
        /// Append the application path to the root path relative URL
        /// </summary>
        /// <param name="relativePath">The relative path from the application root</param>
        /// <returns>The absolute path to the physical location on the server</returns>
        public static string ConvertToAbsolutePath(string relativePath)
        {
            String returnValue = String.Empty;
            
            //System.AppDomain.CurrentDomain.BaseDirectory will work for both WebApp and WinForms
            //for Web App, it gives you the base directory path mapped to web application
            //for WinForms, it gives you the path where exe is
            returnValue = String.Format("{0}\\{1}", System.AppDomain.CurrentDomain.BaseDirectory, relativePath);
            
            return returnValue;
        }

        #endregion
    }
}
