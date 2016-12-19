using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDM.Interfaces
{
    using MDM.Core;

    /// <summary>
    /// Exposes methods or properties to set or get locale message related information.
    /// </summary>
    public interface ILocaleMessage : IMDMObject
    {
        #region Properties

        /// <summary>
        /// Property for Locale Message code
        /// </summary>
        String Code  { get; set; }

        /// <summary>
        /// Property for Locale Message Class
        /// </summary>
        MessageClassEnum MessageClass { get; set; }

        /// <summary>
        /// Property for Locale Message
        /// </summary>
        String Message { get; set; }

        /// <summary>
        /// Property for Locale Message  Description
        /// </summary>
        String Description { get; set; }

        /// <summary>
        /// Property for knowledge base article
        /// </summary>
        String KBALink { get; set; }

        /// <summary>
        /// Property denoting help link of the locale message
        /// </summary>
        String HelpLink { get; set; }

        /// <summary>
        /// Property denoting where used of the locale message
        /// </summary>
        String WhereUsed { get; set; }

        #endregion

        #region Methods

        #region Public Methods

        /// <summary>
        /// Clones the LocaleMessage
        /// </summary>
        /// <returns>Cloned LocaleMessage object</returns>       
        ILocaleMessage Clone();

        #endregion

        #region Private Methods

        #endregion

        #endregion
    }
}
