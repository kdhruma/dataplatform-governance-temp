using System;
using System.Xml;
using System.Runtime.Caching;

namespace MDM.ExceptionManager.Config
{
    /// <summary>
    /// <para>Reads the configuration file, serializes and saves to Cache.</para>
    /// <para>The configuration can also be set dynamically and saved to the config file</para>
    /// <para>All methods are static so this does not require any instantiation.</para>
    /// </summary>
    public sealed class ModuleConfig
    {
        #region Fields

        //the name of the key in web.config file.
        private const string moduleSettingsKey = "ExceptionManager_SettingsFile";
        //the name of the cache were the settings will be stored.
        private const string moduleSettingsCache = "ExceptionManager_Settings";

        #endregion

        #region Constructors
        /// <summary>
        /// Default private constructor.
        /// </summary>
        private ModuleConfig()
        {
        }
        #endregion

        #region Properties

        /// <summary>
        /// Returns ModuleSettings class contains properties to obtain config values.
        /// </summary>
        public static ModuleSettings GetSettings
        {
            get
            {
                return GetSettingsFromFile(string.Empty);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filepath"></param>
        /// <returns></returns>
		public static ModuleSettings GetSettingsFromFile(string filepath)
        {
            ModuleSettings moduleSettings;

            ObjectCache internalObjectCache = MemoryCache.Default;

            moduleSettings = (ModuleSettings)internalObjectCache.Get(moduleSettingsCache);

            if (moduleSettings == null)
            {
                string fileName;

                if (String.IsNullOrWhiteSpace(filepath))
                {
                    fileName = ModuleWebConfig.GetAppSettingAbsolutePath(moduleSettingsKey);
                }
                else
                {
                    fileName = filepath;
                }

                XmlReader reader = XmlReader.Create(fileName);

                try
                {
                    if (reader.Read())
                    {
                        //move the reader to the content skipping xml declaration and comments
                        reader.MoveToContent();
                        String settingsXml = reader.ReadOuterXml();
                        moduleSettings = new ModuleSettings(settingsXml);
                    }

                    if (moduleSettings != null)
                    {
                        var policy = new CacheItemPolicy();
                        policy.ChangeMonitors.Add(new HostFileChangeMonitor(new[] { fileName }));
                        internalObjectCache.Set(moduleSettingsCache, moduleSettings, policy);
                    }
                }
                finally
                {
                    reader.Close();
                }
            }

            return moduleSettings;
        }

        #endregion

    }
}
