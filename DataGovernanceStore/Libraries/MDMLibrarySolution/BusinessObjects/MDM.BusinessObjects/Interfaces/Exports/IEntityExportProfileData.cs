using System;
using MDM.Core;

namespace MDM.Interfaces.Exports
{
    /// <summary>
    /// Exposes methods or properties to set or get the export profile business object.
    /// </summary>
    public interface IEntityExportProfileData
    {
        /// <summary>
        /// Represents entity export profile data in Xml format
        /// </summary>
        /// <returns>String representation of current entity export profile data object</returns>
        String ToXml();

        /// <summary>
        /// Represents entity export profile data in Xml format
        /// </summary>
        /// <param name="objectSerialization">Specifies the object serialization type</param>
        /// <returns>String representation of current entity export profile data object</returns>
        String ToXml(ObjectSerialization objectSerialization);
    }
}
