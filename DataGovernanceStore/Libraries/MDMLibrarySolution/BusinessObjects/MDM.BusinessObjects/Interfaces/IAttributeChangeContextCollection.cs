using System;
using System.Collections.Generic;

namespace MDM.Interfaces
{
    using MDM.BusinessObjects;

    /// <summary>
    /// Exposes methods or properties to set or get attribute change context collection.
    /// </summary>
    public interface IAttributeChangeContextCollection : IEnumerable<AttributeChangeContext>
    {
        #region Properties

        /// <summary>
        /// Presents no. of attribute change context present into the collection
        /// </summary>
        Int32 Count { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Gets Xml representation of the AttributeChangeContextCollection object
        /// </summary>
        /// <returns>Xml string representing the AttributeChangeContextCollection</returns>
        String ToXml();

        #endregion Methods
    }
}