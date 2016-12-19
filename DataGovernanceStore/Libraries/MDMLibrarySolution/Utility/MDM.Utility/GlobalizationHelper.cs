using System;
using System.Globalization;
using System.Collections.Concurrent;

namespace MDM.Utility
{
    using MDM.Core;
    using MDM.CacheManager.Business;

    /// <summary>
    /// This class contains helper methods for Globalization. eg handling number formats, date format and timezones etc.
    /// </summary>
    public class GlobalizationHelper
    {
        #region Fields

        /// <summary>
        /// Field denoting system data locale
        /// </summary>
        private static LocaleEnum _systemDataLocale = LocaleEnum.en_WW;

        /// <summary>
        /// Field denoting system UI locale
        /// </summary>
        private static LocaleEnum _systemUILocale = LocaleEnum.en_WW;

        /// <summary>
        /// Field denoting locale type for model display
        /// </summary>
        private static LocaleType _modelDisplayLocaleType = LocaleType.DataLocale;

        /// <summary>
        /// Field denoting locale type for data formatting
        /// </summary>
        private static LocaleType _dataFormattingLocaleType = LocaleType.DataLocale;

        #endregion

        #region Constructors

        #endregion

        #region Properties

        #endregion

        #region Methods

        /// <summary>
        ///  Gets system data locale
        /// </summary>
        /// <returns></returns>
        public static LocaleEnum GetSystemDataLocale()
        {
            return _systemDataLocale;
        }

        /// <summary>
        ///  Gets system UI locale
        /// </summary>
        /// <returns></returns>
        public static LocaleEnum GetSystemUILocale()
        {
            return _systemUILocale;
        }

        /// <summary>
        ///  Gets type of the locale configured for model display
        /// </summary>
        /// <returns></returns>
        public static LocaleType GetModelDisplayLocaleType()
        {
            return _modelDisplayLocaleType;
        }

        /// <summary>
        ///  Gets type of the locale configured for data formatting
        /// </summary>
        /// <returns></returns>
        public static LocaleType GetDataFormattingLocaleType()
        {
            return _dataFormattingLocaleType;
        }

        /// <summary>
        /// Gets model display locale
        /// </summary>
        /// <param name="dataLocale">Current data locale of the user</param>
        /// <param name="uiLocale">Current UI locale of the user</param>
        /// <returns>Model display locale as per the configuration and current locales of the user</returns>
        public static LocaleEnum GetModelDisplayLocale(LocaleEnum dataLocale, LocaleEnum uiLocale)
        {
            LocaleEnum modelDisplayLocale = LocaleEnum.en_WW;

            if (_modelDisplayLocaleType == LocaleType.DataLocale)
            {
                modelDisplayLocale = dataLocale;
            }
            else if (_modelDisplayLocaleType == LocaleType.UILocale)
            {
                modelDisplayLocale = uiLocale;
            }

            return modelDisplayLocale;
        }

        /// <summary>
        /// Gets data formatting locale
        /// </summary>
        /// <param name="dataLocale">Current data locale of the user</param>
        /// <param name="uiLocale">Current UI locale of the user</param>
        /// <returns>Data formatting locale as per the configuration and current locales of the user</returns>
        public static LocaleEnum GetDataFormattingLocale(LocaleEnum dataLocale, LocaleEnum uiLocale)
        {
            LocaleEnum dataFormattingLocale = LocaleEnum.en_WW;

            if (_dataFormattingLocaleType == LocaleType.DataLocale)
            {
                dataFormattingLocale = dataLocale;
            }
            else if (_dataFormattingLocaleType == LocaleType.UILocale)
            {
                dataFormattingLocale = uiLocale;
            }

            return dataFormattingLocale;
        }

        /// <summary>
        ///  Set system locales
        /// </summary>
        public static void LoadSystemLocales()
        {
            //Get system locales cache
            String cacheKey = "RS_SDL";

            ICache cache = CacheFactory.GetCache();
            Object cachedObject = cache.Get(cacheKey);

            if(cachedObject != null && cachedObject is String[])
            {
                String[] localeConfigs = (String[])cachedObject;

                if(localeConfigs != null && localeConfigs.Length > 3)
                {
                    Enum.TryParse<LocaleEnum>(localeConfigs[0], true, out _systemDataLocale);
                    Enum.TryParse<LocaleEnum>(localeConfigs[1], true, out _systemUILocale);
                    Enum.TryParse<LocaleType>(localeConfigs[2], true, out _modelDisplayLocaleType);
                    Enum.TryParse<LocaleType>(localeConfigs[3], true, out _dataFormattingLocaleType);
                }

                cache.Remove(cacheKey);
            }
        }
        
        #endregion
    }
}
