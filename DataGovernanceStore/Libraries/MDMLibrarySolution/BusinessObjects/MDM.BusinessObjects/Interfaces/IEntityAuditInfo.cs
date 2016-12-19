using System;

namespace MDM.Interfaces
{
    /// <summary>
    /// Exposes methods or properties to set or get entity audit information.
    /// </summary>
    public interface IEntityAuditInfo : IAuditInfo
    {
        /// <summary>
        /// Attribute id for current audit info
        /// </summary>
        Int32 AttributeId { get; set; }

        /// <summary>
        /// Entity id for current audit info
        /// </summary>
        Int64 EntityId { get; set; }

        /// <summary>
        /// Get Xml representation of AuditInfo object
        /// </summary>
        /// <returns>XML String of AuditInfo Object</returns>
        new String ToXml();

        /// <summary>
        /// Get Xml representation of Menu object
        /// </summary>
        /// <param name="objectSerialization">Indicates the serialization option. Based on the value selected, the different xml representation will be there</param>
        /// <returns>Xml representation of object</returns>
        new String ToXml(MDM.Core.ObjectSerialization objectSerialization);
    }
}
