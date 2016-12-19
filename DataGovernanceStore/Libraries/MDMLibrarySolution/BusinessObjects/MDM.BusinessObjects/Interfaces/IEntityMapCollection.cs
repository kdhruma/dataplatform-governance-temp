using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Collections.Generic;

namespace MDM.Interfaces
{
    using MDM.Core;
    using MDM.BusinessObjects;

    /// <summary>
    /// Exposes methods or properties to set or get attribute operation result.
    /// </summary>
    public interface IEntityMapCollection : IEnumerable<EntityMap>
    {
        #region Properties



        #endregion

        #region Methods

        /// <summary>
        /// Get Xml representation of Entity Map Collection
        /// </summary>
        /// <returns>Xml representation of Entity Map Collection</returns>
        String ToXml();

        /// <summary>
        /// Get Xml representation of Entity Map Collection based on requested object serialization
        /// </summary>
        /// <param name="objectSerialization">Type of the serialization option</param>
        /// <returns>Xml representation of Entity Map Collection</returns>
        String ToXml(ObjectSerialization objectSerialization);

        /// <summary>
        /// Add IEntity map object in IEntityMapCollection
        /// </summary>
        /// <param name="iEntityMap">entity map to add in IEntityMapCollection</param>
        void Add(IEntityMap iEntityMap);

        /// <summary>
        /// Removes all entity map from collection
        /// </summary>
        void Clear();

        /// <summary>
        /// Determines whether the EntityMapCollection contains a specific entity map.
        /// </summary>
        /// <param name="iEntityMap">The entity map object to locate in the IEntityMapCollection.</param>
        /// <returns>
        /// <para>true : If iEntityMap found in IEntityMapCollection</para>
        /// <para>false : If iEntityMap not found in IEntityMapCollection</para>
        /// </returns>
        bool Contains(IEntityMap iEntityMap);

        /// <summary>
        /// Removes the first occurrence of a specific entity map from the IEntityMapCollection.
        /// </summary>
        /// <param name="iEntityMap">The entity map object to remove from the IEntityMapCollection.</param>
        /// <returns>true if item is successfully removed; otherwise, false. This method also returns false if item was not found in the original collection</returns>
        bool Remove(IEntityMap iEntityMap);

        /// <summary>
        ///   Gets the number of elements contained in the IEntityMapCollection.
        /// </summary>
        int Count { get; }

        /// <summary>
        /// Gets a value indicating whether the IEntityMapCollection is read-only.
        /// </summary>
        bool IsReadOnly { get; }

        #endregion
    }
}
