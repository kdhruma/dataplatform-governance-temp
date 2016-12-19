using System;
using System.Collections.Generic;

namespace MDM.Interfaces
{
    using MDM.Core;
    using MDM.BusinessObjects;

    /// <summary>
    /// Exposes methods or properties to set or get category collection instance.
    /// </summary>
    public interface ICategoryCollection : IEnumerable<Category>, ICollection<Category>
    {
        #region Properties
        #endregion

        #region Methods

        /// <summary>
        /// Get Xml representation of Category Collection
        /// </summary>
        /// <returns>Xml representation of Category Collection</returns>
        String ToXml();

        /// <summary>
        /// Get Xml representation of Category Collection based on requested object serialization
        /// </summary>
        /// <param name="objectSerialization">Type of the serialization option</param>
        /// <returns>Xml representation of Category Collection</returns>
        String ToXml(ObjectSerialization objectSerialization);

        #endregion
    }
}
