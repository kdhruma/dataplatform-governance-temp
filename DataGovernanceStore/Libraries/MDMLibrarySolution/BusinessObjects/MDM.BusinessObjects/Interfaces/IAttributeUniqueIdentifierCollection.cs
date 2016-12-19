using System;
using System.Collections.Generic;

namespace MDM.Interfaces
{
    using MDM.BusinessObjects;

    /// <summary>
    /// Exposes methods or properties to set or get attribute unique identifier collection.
    /// </summary>
    public interface IAttributeUniqueIdentifierCollection : IEnumerable<AttributeUniqueIdentifier>
    {
        #region Fields
        #endregion

        #region Properties

        /// <summary>
        /// Number of AttributeUniqueIdentifier instance present in to the current collection
        /// </summary>
        Int32 Count { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Get Xml representation of AttributeUniqueIdentifierCollection object
        /// </summary>
        /// <returns>Xml string representing the AttributeUniqueIdentifierCollection</returns>
        String ToXml();

        /// <summary>
        /// Add AttributeUniqueIdentifier in collection
        /// </summary>
        /// <param name="iAttributeUniqueIdentifier">AttributeUniqueIdentifier to add in collection</param>
        void Add(IAttributeUniqueIdentifier iAttributeUniqueIdentifier);

        #endregion Methods
    }
}
