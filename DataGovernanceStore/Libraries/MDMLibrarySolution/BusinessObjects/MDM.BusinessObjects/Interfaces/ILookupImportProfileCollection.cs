using System;
using System.Collections.Generic;

namespace MDM.Interfaces
{
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Exposes methods or properties to set or get lookup import profile collection related information.
    /// </summary>
    public interface ILookupImportProfileCollection : ICollection<ILookupImportProfile>
    {
        /// <summary>
        /// Verify whether or not sequence contain required item
        /// </summary>
        /// <param name="id">Id of Lookup Import Profile</param>
        /// <returns>[True] if item exists, [False] otherwise</returns>
        Boolean Contains(Int32 id);

        /// <summary>
        /// Compare collection with another collection
        /// </summary>
        /// <param name="obj">Collection to compare</param>
        /// <returns>[True] if collections equal and [False] otherwise</returns>
        Boolean Equals(Object obj);

        /// <summary>
        /// Gets hash code of the item
        /// </summary>
        /// <returns>Gets hash code of the item</returns>
        Int32 GetHashCode();

        /// <summary>
        /// Removes item by id
        /// </summary>
        /// <param name="lookupImportProfileId">Id of item to be removed</param>
        /// <returns>[True] if removal was successful, [False] otherwise</returns>
        Boolean Remove(Int32 lookupImportProfileId);

        /// <summary>
        /// Convert item to XML
        /// </summary>
        /// <returns>XML string</returns>
        String ToXml();

        /// <summary>
        /// Convert item to XML
        /// </summary>
        /// <param name="serialization">Rules of serialization</param>
        /// <returns>XML string</returns>
        String ToXml(ObjectSerialization serialization);
    }
}
