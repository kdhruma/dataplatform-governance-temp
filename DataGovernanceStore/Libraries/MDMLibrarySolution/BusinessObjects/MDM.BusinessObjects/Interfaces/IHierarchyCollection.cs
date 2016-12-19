using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDM.Interfaces
{
    using System.Collections.ObjectModel;
    using MDM.BusinessObjects;
    using MDM.Core;

    /// <summary>
    /// Exposes methods or properties to set or get collection of hierarchies related information.
    /// </summary>
    public interface IHierarchyCollection : ICollection<Hierarchy>
    {
        /// <summary>
        /// Indicates allowed user actions on the object
        /// </summary>
        Collection<UserAction> PermissionSet { get; }

        /// <summary>
        /// Add item to the collection
        /// </summary>
        /// <param name="iHierarchy">Object which implements interface <see cref="IHierarchy"/></param>
        void Add(IHierarchy iHierarchy);

        /// <summary>
        /// Verify whether or not sequence contain required item
        /// </summary>
        /// <param name="hierarchyId">Id of the hierarchy</param>
        /// <returns>[True] if item exists, [False] otherwise</returns>
        Boolean Contains(Int32 hierarchyId);

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
        /// <param name="hierarchyId">Id of item to be removed</param>
        /// <returns>[True] if removal was successful, [False] otherwise</returns>
        Boolean Remove(Int32 hierarchyId);

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
        IHierarchyCollection Clone();

        /// <summary>
        /// Gets item by Id
        /// </summary>
        /// <param name="hierarchyId">Item Id</param>
        /// <returns>Cloned item</returns>
        IHierarchy Get(Int32 hierarchyId);
    }
}
