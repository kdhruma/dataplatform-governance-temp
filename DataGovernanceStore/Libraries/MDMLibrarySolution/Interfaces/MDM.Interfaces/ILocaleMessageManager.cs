using System;

namespace MDM.Interfaces
{
    using BusinessObjects;
    using Core;

    public interface ILocaleMessageManager
    {
        /// <summary>
        /// Get Locale Message based on locale and message code
        /// </summary>
        /// <param name="locale">Indicates the Locale</param>
        /// <param name="messageCode">Indicates the Message Code</param>
        /// <param name="loadLastest">true if load fresh copy from DB otherwise take from cache</param>
        /// <param name="callerContext">Indicates Application and Module name by which action is being performed</param>
        /// <param name="escapeSpecialCharacters">Escape special characters ['],["],[\]. Set as True whenever message get is required for JavaScript</param>
        /// <returns>Returns LocaleMessage</returns>
        LocaleMessage Get(LocaleEnum locale, String messageCode, Boolean loadLastest, CallerContext callerContext, Boolean escapeSpecialCharacters = false);
    }
}
