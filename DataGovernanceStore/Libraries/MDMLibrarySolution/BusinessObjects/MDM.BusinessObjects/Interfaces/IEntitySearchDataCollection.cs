using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDM.Interfaces
{
    using MDM.Core;
    using MDM.BusinessObjects;

    /// <summary>
    /// Exposes methods or properties to set or get collection of entity search data.
    /// </summary>
    public interface IEntitySearchDataCollection : IEnumerable<EntitySearchData>
    {
        #region Properties

        #endregion

        #region Methods

        /// <summary>
        /// Get Xml representation of EntitySearchDataCollection
        /// </summary>
        /// <returns>Xml representation of EntitySearchDataCollection</returns>
        String ToXml();

        /// <summary>
        /// Get Xml representation of EntitySearchDataCollection based on requested object serialization
        /// </summary>
        /// <param name="objectSerialization">Type of the serialization option</param>
        /// <returns>Xml representation of EntitySearchDataCollection</returns>
        String ToXml(ObjectSerialization objectSerialization);

        #endregion
    }
}
