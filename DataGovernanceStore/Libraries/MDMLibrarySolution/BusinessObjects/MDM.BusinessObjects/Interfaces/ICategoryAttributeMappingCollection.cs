using System;
using System.Collections.Generic;

namespace MDM.Interfaces
{
    using MDM.BusinessObjects;
    using MDM.Core;

    /// <summary>
    /// Exposes methods or properties to set or get category attribute mapping collection.
    /// </summary>
    public interface ICategoryAttributeMappingCollection : ICollection<CategoryAttributeMapping>, ICloneable
    {
        /// <summary>
        /// Add item to the collection
        /// </summary>
        /// <param name="iCategoryAttributeMapping">Object which implements interface <see cref="IHierarchy"/></param>
        void Add(ICategoryAttributeMapping iCategoryAttributeMapping);

        /// <summary>
        /// Verify whether or not sequence contain required item
        /// </summary>
        /// <param name="categoryAttributeMappingId">Id of the categoryAttributeMapping</param>
        /// <returns>[True] if item exists, [False] otherwise</returns>
        Boolean Contains(Int32 categoryAttributeMappingId);

        /// <summary>
        /// Compare collection with another collection
        /// </summary>
        /// <param name="obj">Collection to compare</param>
        /// <returns>[True] if collections equal and [False] otherwise</returns>
        Boolean Equals(Object obj);

        /// <summary>
        /// Gets hash code of the item
        /// </summary>
        /// <returns>Hash code of the item</returns>
        Int32 GetHashCode();

        /// <summary>
        /// Removes item by id
        /// </summary>
        /// <param name="categoryAttributeMappingId">Id of item to be removed</param>
        /// <returns>[True] if removal was successful, [False] otherwise</returns>
        Boolean Remove(Int32 categoryAttributeMappingId);

        /// <summary>
        /// Convert item to XML
        /// </summary>
        /// <returns>XML string</returns>
        String ToXml();

        /// <summary>
        /// Convert item to XML with specific rule
        /// </summary>
        /// <param name="serialization">Rules of serialization</param>
        /// <returns>XML string</returns>
        String ToXml(ObjectSerialization serialization);

        /// <summary>
        /// Clones item
        /// </summary>
        /// <returns>Cloned item</returns>
        new ICategoryAttributeMappingCollection Clone();

        /// <summary>
        /// Gets item by Id
        /// </summary>
        /// <param name="categoryAttributeMappingId">Item Id</param>
        /// <returns>Cloned item</returns>
        ICategoryAttributeMapping Get(Int32 categoryAttributeMappingId);

        /// <summary>
        /// Gets the by category identifier.
        /// </summary>
        /// <param name="categoryId">The category identifier.</param>
        /// <returns>Cloned item</returns>
        ICategoryAttributeMappingCollection GetByCategoryId(Int64 categoryId);
    }
}
