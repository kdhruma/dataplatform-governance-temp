using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MDM.Core;

namespace MDM.Interfaces
{
    /// <summary>
    /// Interface for LocaleConfig
    /// </summary>
    public interface ILocaleConfig
    {
        /// <summary>
        /// Property representing the system data locale
        /// </summary>
        LocaleEnum SystemDataLocale { get; set; }

        /// <summary>
        /// Property representing the system UI locale
        /// </summary>
        LocaleEnum SystemUILocale { get; set; }

        /// <summary>
        /// Property representing locale type for model display
        /// </summary>
        LocaleType ModelDisplayLocaleType { get; set; }

        /// <summary>
        /// Property representing locale type for data formatting
        /// </summary>
        LocaleType DataFormattingLocaleType { get; set; }

        /// <summary>
        /// Property representing list of all allowable UI Locales
        /// </summary>
        String AllowableUILocales { get; set; }
    }
}
