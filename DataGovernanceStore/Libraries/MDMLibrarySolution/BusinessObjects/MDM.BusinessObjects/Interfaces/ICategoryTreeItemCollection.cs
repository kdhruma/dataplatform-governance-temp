using System;
using System.Collections.Generic;

namespace MDM.Interfaces
{
    using MDM.Core;
    using MDM.BusinessObjects;

    /// <summary>
    /// Exposes methods or properties to set or get category tree item collection.
    /// </summary>
    public interface ICategoryTreeItemCollection : IEnumerable<CategoryTreeItem>
    {
        #region Properties

        #endregion

        #region Methods

        /// <summary>
        /// Get Xml representation of Category Tree Item Collection
        /// </summary>
        /// <returns>Xml representation of Category Tree Item Collection</returns>
        String ToXml();

        /// <summary>
        /// Get Xml representation of Category Tree Item Collection based on requested object serialization
        /// </summary>
        /// <param name="objectSerialization">Type of the serialization option</param>
        /// <returns>Xml representation of Category Tree Item Collection</returns>
        String ToXml(ObjectSerialization objectSerialization);

        #endregion
    }
}