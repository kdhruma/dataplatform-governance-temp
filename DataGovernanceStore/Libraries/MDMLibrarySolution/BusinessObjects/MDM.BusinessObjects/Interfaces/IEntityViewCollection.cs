using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Collections.Generic;

namespace MDM.Interfaces
{
    using MDM.Core;
    using MDM.BusinessObjects;

    /// <summary>
    /// Exposes methods or properties to set or get entity view collection.
    /// </summary>
    public interface IEntityViewCollection : IEnumerable<EntityView>
    {
        #region Properties

        #endregion

        #region Methods

        /// <summary>
        /// Get Xml representation of Entity View Collection
        /// </summary>
        /// <returns>Xml representation of Entity View Collection</returns>
        String ToXml();

        /// <summary>
        /// Get Xml representation of Entity View Collection based on requested object serialization
        /// </summary>
        /// <param name="objectSerialization">Type of the serialization option</param>
        /// <returns>Xml representation of Entity View Collection</returns>
        String ToXml(ObjectSerialization objectSerialization);

        #endregion
    }
}
