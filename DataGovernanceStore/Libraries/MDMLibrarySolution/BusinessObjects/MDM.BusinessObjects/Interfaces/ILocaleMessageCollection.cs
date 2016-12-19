using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace MDM.Interfaces
{
    using MDM.BusinessObjects;
    using MDM.Core;

    /// <summary>
    /// Exposes methods or properties to set or get locale message collection.
    /// </summary>
    public interface ILocaleMessageCollection : IEnumerable<LocaleMessage>
    {
        #region Properties

        #endregion

        #region Methods

        #region Get LocaleMessages

        /// <summary>
        /// Gets Locale Messages with specified locale
        /// </summary>
        /// <param name="locale">locale of the localeMessage to search in LoclaeMessages</param>
        /// <param name="systemUILocale">System UI Locale which will be considered when message is not available for the requested locale</param>
        /// <returns>LocaleMessageCollection interface</returns>
        ILocaleMessageCollection Get(LocaleEnum locale, LocaleEnum systemUILocale);

        /// <summary>
        /// Gets Locale Message with specified Locale and messageCode
        /// </summary>                            
        /// <param name="locale">Locale of the localeMessage to search in LoclaeMessages</param>
        /// <param name="messageCode">Message code of the LocaleMessage</param>
        /// <param name="systemUILocale">System UI Locale which will be considered when message is not available for the requested locale</param>
        /// <returns>LocaleMessage interface</returns>
        ILocaleMessage Get(LocaleEnum locale, String messageCode, LocaleEnum systemUILocale);

        /// <summary>
        /// Gets Locale Messages with specified Locale and messageCodes
        /// </summary>                            
        /// <param name="locale">Locale of the localeMessage to search in LoclaeMessages</param>
        /// <param name="messageCodeList">Message codes of the LocaleMessages</param>
        /// <param name="systemUILocale">System UI Locale which will be considered when message is not available for the requested locale</param>
        /// <returns>LocaleMessageCollection interface</returns>
        ILocaleMessageCollection Get(LocaleEnum locale, Collection<String> messageCodeList, LocaleEnum systemUILocale);

        #endregion

        #region Add LocaleMessage
        /// <summary>
        /// Add LocaleMessages object in collection
        /// </summary>
        /// <param name="item">LocaleMessages to add in collection</param>
        void Add(ILocaleMessage item);

        #endregion

        #region ToXml Methods

        /// <summary>
        /// Get Xml representation of LocaleMessageCollection object
        /// </summary>
        /// <returns>Xml string representing the LocaleMessageCollection</returns>
        String ToXml();

        #endregion

        #endregion

    }
}
