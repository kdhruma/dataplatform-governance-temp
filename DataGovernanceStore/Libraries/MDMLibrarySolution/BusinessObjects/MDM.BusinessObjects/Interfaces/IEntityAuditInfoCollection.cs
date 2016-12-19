using System;
using System.Collections.Generic;

namespace MDM.Interfaces
{
    using MDM.BusinessObjects;

    /// <summary>
    /// Exposes methods or properties to set or get entity audit info collection.
    /// </summary>
    public interface IEntityAuditInfoCollection : IEnumerable<EntityAuditInfo>
    {
        /// <summary>
        /// XMl Representaion of an Object
        /// </summary>
        /// <param name="objectSerialization">Serialization Oprion</param>
        /// <returns>XMl Representaion of an Object</returns>
        String ToXml(MDM.Core.ObjectSerialization objectSerialization);
        
        /// <summary>
        /// XMl Representaion of an Object
        /// </summary>
        /// <returns>XMl Representaion of an Object</returns>
        String ToXml();
    }
}
