using System;
using System.Collections.Generic;

namespace MDM.Interfaces
{
    using MDM.BusinessObjects;

    /// <summary>
    /// Exposes methods or properties to set or get attribute change context collection.
    /// </summary>
    public interface IExtensionChangeContextCollection : IEnumerable<ExtensionChangeContext>
    {
        #region Properties

        /// <summary>
        /// Presents no. of extension change context present into the collection
        /// </summary>
        Int32 Count { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Gets Xml representation of the ExtensionChangeContextCollection object
        /// </summary>
        /// <returns>Xml string representing the ExtensionChangeContextCollection</returns>
        String ToXml();

        #endregion Methods
    }
}