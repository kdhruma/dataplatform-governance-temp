using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;

namespace MDM.Core
{
    #region Enum Extensions

    /// <summary>
    /// Specifies extension methods for Enum
    /// </summary>
    public static class EnumExtensions
    {
        #region Fields

        /// <summary>
        /// Specifies dictionary for locale as LocaleEnum and culture name as string
        /// </summary>
        public static Dictionary<LocaleEnum, String> LocaleCultureMap = new Dictionary<LocaleEnum, String>();

        /// <summary>
        /// Specifies lock object
        /// </summary>
        private static Object _lockObj = new Object();

        #endregion

        /// <summary>
        /// Gets description of enum value
        /// </summary>
        /// <param name="value">enum value</param>
        /// <returns>Description of enum value</returns>
        public static String GetDescription(this Enum value)
        {
            String description = String.Empty;

            if (value != null)
            {
                description = value.ToString();

                FieldInfo fi = value.GetType().GetField(value.ToString());

                if (fi != null)
                {
                    DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

                    if (attributes != null && attributes.Length > 0)
                        description = attributes[0].Description;
                }
            }

            return description;
        }

        /// <summary>
        /// Gets culture name of locale enum value
        /// </summary>
        /// <param name="locale">Locale enum value</param>
        /// <returns>Culture name of locale enum value</returns>
        public static String GetCultureName(this LocaleEnum locale)
        {
            String cultureName = String.Empty;

            if (locale != LocaleEnum.UnKnown && locale != LocaleEnum.Neutral)
            {
                if (!LocaleCultureMap.TryGetValue(locale, out cultureName))
                {
                    cultureName = locale.ToString();

                    FieldInfo fi = locale.GetType().GetField(cultureName);

                    if (fi != null)
                    {
                        CultureAttribute[] attributes = (CultureAttribute[])fi.GetCustomAttributes(typeof(CultureAttribute), false);

                        if (attributes != null && attributes.Length > 0)
                            cultureName = attributes[0].CultureName;
                    }

                    if (!LocaleCultureMap.ContainsKey(locale))
                    {
                        lock (_lockObj)
                        {
                            if (!LocaleCultureMap.ContainsKey(locale))
                            {
                                LocaleCultureMap.Add(locale, cultureName);
                            }
                        }
                    }
                }
            }

            return cultureName;
        }

        /// <summary>
        /// Gets proper UI display value for EntityChangeType enum value
        /// </summary>
        /// <param name="value">EntityChangeType enum value</param>
        /// <returns>UI display name for the EntityChangeType enum value</returns>
        public static String GetUIDisplayName(this Enum value)
        {
            String uiDisplayName = String.Empty;
            uiDisplayName = value.ToString();
            FieldInfo fi = value.GetType().GetField(value.ToString());

            if (fi != null)
            {
                UIDisplayNameAttribute[] attributes = (UIDisplayNameAttribute[])fi.GetCustomAttributes(typeof(UIDisplayNameAttribute), false);

                if (attributes != null && attributes.Length > 0)
                    uiDisplayName = attributes[0].UIDisplayName;
            }

            return uiDisplayName;
        }

        /// <summary>
        /// Gets proper Localization Code value for enum value
        /// </summary>
        /// <param name="value">Enum value</param>
        /// <returns>Localization Code for the enum value</returns>
        public static String GetLocalizationCode(this Enum value)
        {
            String localizationCode = String.Empty;
            FieldInfo fi = value.GetType().GetField(value.ToString());

            if (fi != null)
            {
                LocalizationCodeAttribute attribute = (LocalizationCodeAttribute)fi.GetCustomAttribute(typeof(LocalizationCodeAttribute));

                if (attribute != null)
                {
                    localizationCode = attribute.LocalizationCode;
                }
            }

            return localizationCode;
        }
    }

    #endregion

    #region Enum Attribute Classes

    /// <summary>
    /// Specifies Culture attribute for Locale enum
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public sealed class CultureAttribute : Attribute
    {
        #region Fields

        /// <summary>
        /// Specifies culture name
        /// </summary>
        private readonly String _cultureName = String.Empty;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor initializing culture Name
        /// </summary>
        /// <param name="cultureName">Name of the Culture</param>
        public CultureAttribute(String cultureName)
        {
            this._cultureName = cultureName;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Specifies Name of the Culture
        /// </summary>
        public String CultureName
        {
            get
            {
                return _cultureName;
            }
        }

        #endregion
    }


    /// <summary>
    /// Specifies UI display attribute for ChangeType enum
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public sealed class UIDisplayNameAttribute : Attribute
    {
        #region Fields

        /// <summary>
        /// Specifies UI display name
        /// </summary>
        private readonly String _uiDisplayName = String.Empty;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor initializing UI display value
        /// </summary>
        /// <param name="uiDisplayName">Display name which has to show on UI</param>
        public UIDisplayNameAttribute(String uiDisplayName)
        {
            this._uiDisplayName = uiDisplayName;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Specifies name to display on UI 
        /// </summary>
        public String UIDisplayName
        {
            get
            {
                return _uiDisplayName;
            }
        }

        #endregion
    }

    /// <summary>
    /// Specifies localization code attribute
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public sealed class LocalizationCodeAttribute : Attribute
    {
        #region Fields

        /// <summary>
        /// Specifies localization code
        /// </summary>
        private readonly String _localizationCode = String.Empty;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor initializing localization code value
        /// </summary>
        /// <param name="localizationCode">Localization code which should be translated depends from locale</param>
        public LocalizationCodeAttribute(String localizationCode)
        {
            this._localizationCode = localizationCode;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Specifies localization code 
        /// </summary>
        public String LocalizationCode
        {
            get
            {
                return _localizationCode;
            }
        }

        #endregion
    }


    #endregion

}
