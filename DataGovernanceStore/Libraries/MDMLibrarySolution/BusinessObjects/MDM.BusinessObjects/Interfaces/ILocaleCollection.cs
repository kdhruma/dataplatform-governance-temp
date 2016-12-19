using System;
using System.Collections.Generic;

namespace MDM.Interfaces
{
    using MDM.BusinessObjects;
    using MDM.Core;

    /// <summary>
    ///  Exposes methods or properties used for setting locale collection related information.
    /// </summary>
    public interface ILocaleCollection : IEnumerable<Locale>
    {
        #region Properties
        #endregion

        #region Methods

        /// <summary>
        /// Determines whether the locale collection contains the locale corresponding to the specified locale enum.
        /// </summary>
        /// <param name="localeEnum">The locale enum.</param>
        /// <returns>True if locale is found else false.</returns>
        Boolean Contains(LocaleEnum localeEnum);

        #endregion Methods


        #region ToXml Methods

        /// <summary>
        /// Get Xml representation of LocaleCollection object
        /// </summary>
        /// <returns>Xml string representing the LocaleCollection</returns>
        String ToXml();

        #endregion
    }
}
