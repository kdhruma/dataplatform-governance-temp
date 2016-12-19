using System;
using System.Collections.Generic;

namespace MDM.Interfaces.EntityIdentification
{
    using MDM.Core;
    using MDM.BusinessObjects.EntityIdentification;

    /// <summary>
    /// Exposes methods or properties to set or get.
    /// </summary>
    public interface IEntityUniqueIdentificationMapCollection : IEnumerable<EntityUniqueIdentificationMap>
    {
        #region Properties



        #endregion

        #region Methods

        /// <summary>
        /// Get Xml representation of Entity Identifier Map Collection
        /// </summary>
        /// <returns>Xml representation of Entity Identifier Map Collection</returns>
        String ToXml();

        /// <summary>
        /// Get Xml representation of Entity Identifier Map Collection based on requested object serialization
        /// </summary>
        /// <param name="objectSerialization">Type of the serialization option</param>
        /// <returns>Xml representation of Entity Identifier Map Collection</returns>
        String ToXml(ObjectSerialization objectSerialization);

        /// <summary>
        /// Add IEntityIdentifierMap object in IEntityIdentifierMapCollection
        /// </summary>
        /// <param name="iEntityIdentifierMap">entity map to add in IEntityIdentifierMapCollection</param>
        void Add(IEntityUniqueIdentificationMap iEntityIdentifierMap);

        /// <summary>
        /// Removes all entity Identifier map from collection
        /// </summary>
        void Clear();

        /// <summary>
        /// Determines whether the EntityIdentifierMapCollection contains a specific entity identifier map.
        /// </summary>
        /// <param name="iEntityIdentifierMap">The entity identifier map object to locate in the iEntityIdentifierMapCollection.</param>
        /// <returns>
        /// <para>true : If iEntityIdentifierMap found in IEntityIdentifierMapCollection</para>
        /// <para>false : If iEntityIdentifierMap not found in IEntityIdentifierMapCollection</para>
        /// </returns>
        bool Contains(IEntityUniqueIdentificationMap iEntityIdentifierMap);

        /// <summary>
        /// Removes the first occurrence of a specific entity identifier map from the IEntityIdentifierMapCollection.
        /// </summary>
        /// <param name="iEntityIdentifierMap">The entity map object to remove from the IEntityIdentifierMapCollection.</param>
        /// <returns>true if item is successfully removed; otherwise, false. This method also returns false if item was not found in the original collection</returns>
        bool Remove(IEntityUniqueIdentificationMap iEntityIdentifierMap);

        /// <summary>
        ///   Gets the number of elements contained in the IEntityIdentifierMapCollection.
        /// </summary>
        int Count { get; }

        /// <summary>
        /// Gets a value indicating whether the IEntityIdentifierMapCollection is read-only.
        /// </summary>
        bool IsReadOnly { get; }

        #endregion
    }
}
