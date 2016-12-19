using System;

namespace MDM.Interfaces
{
    using MDM.Core;
    
    /// <summary>
    /// Exposes methods or properties to set or get locale related information.
    /// </summary>
    public interface ILocale : IMDMObject
    {
        #region Properties

        /// <summary>
        /// Property denoting the Region id of a Locale
        /// </summary>
        Int32 RegionId
        {
            get;
            set;
        }

        /// <summary>
        /// Property denoting the Region Name of a Locale
        /// </summary>
        String RegionName
        {
            get;
            set;
        }

        /// <summary>
        /// Property denoting the Language id of a Locale
        /// </summary>
        Int32 LanguageId
        {
            get;
            set;
        }

        /// <summary>
        /// Property denoting the Language Name of a Locale
        /// </summary>
        String LanguageName
        {
            get;
            set;
        }

        /// <summary>
        /// Property denoting the Culture id of a Locale
        /// </summary>
        Int32 CultureId
        {
            get;
            set;
        }

        /// <summary>
        /// Property denoting the Culture Name of a Locale
        /// </summary>
        String CultureName
        {
            get;
            set;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Get Xml representation of Locale object
        /// </summary>
        /// <returns>Xml representation of object</returns>
        String ToXml();

        #endregion Methods
    }
}
