using System;
using System.Collections.Generic;

namespace MDM.Interfaces
{
    using MDM.BusinessObjects;

    /// <summary>
    /// Exposes methods or properties to set or get attribute change context collection.
    /// </summary>
    public interface IRelationshipChangeContextCollection : IEnumerable<RelationshipChangeContext>
    {
        #region Properties

        /// <summary>
        /// Presents no. of relationship change context present into the collection
        /// </summary>
        Int32 Count { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Gets Xml representation of the RelationshipChangeContextCollection object
        /// </summary>
        /// <returns>Xml string representing the RelationshipChangeContextCollection</returns>
        String ToXml();

        #endregion Methods
    }
}