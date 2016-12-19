using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDM.Interfaces
{
    using MDM.BusinessObjects;

    /// <summary>
    /// Exposes methods or properties to set or get locale change context collection.
    /// </summary>
    public interface ILocaleChangeContextCollection : IEnumerable<LocaleChangeContext>
    {
        #region Properties

        /// <summary>
        /// Presents no. of locale change context present into the collection
        /// </summary>
        Int32 Count { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Gets Xml representation of the LocaleChangeContextCollection object
        /// </summary>
        /// <returns>Xml string representing the LocaleChangeContextCollection</returns>
        String ToXml();

        #endregion Methods
    }
}