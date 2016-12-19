using System;
using System.Collections.Generic;

namespace MDM.Interfaces
{
    using MDM.Core;
    using MDM.BusinessObjects;
    using MDM.BusinessObjects.Denorm;

    /// <summary>
    /// Exposes methods or properties to set or get entity activity log collection.
    /// </summary>
    public interface IEntityActivityLogCollection : IEnumerable<EntityActivityLog>
    {
        /// <summary>
        /// XML Representation of an Object
        /// </summary>
        /// <returns>XML Representation of an Object</returns>
        String ToXml();
    }
}
