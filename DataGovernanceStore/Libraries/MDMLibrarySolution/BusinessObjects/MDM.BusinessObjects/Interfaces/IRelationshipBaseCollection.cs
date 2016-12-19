using System;
using System.Collections.Generic;
using System.Linq;

namespace MDM.Interfaces
{
    using MDM.Core;
    using MDM.BusinessObjects;

    /// <summary>
    /// Exposes methods or properties to set or get the collection of base relationship.
    /// </summary>
    public interface IRelationshipBaseCollection : IEnumerable<RelationshipBase>
    {
        #region Properties

        #endregion

        #region Methods

        /// <summary>
        /// Get Xml representation of Relationship Collection
        /// </summary>
        /// <returns>Xml representation of Relationship Collection</returns>
        String ToXml();

        /// <summary>
        /// Get Xml representation of Relationship Collection based on requested object serialization
        /// </summary>
        /// <param name="objectSerialization">Type of the serialization option</param>
        /// <returns>Xml representation of Relationship Collection</returns>
        String ToXml(ObjectSerialization objectSerialization);

        #endregion
    }
}
