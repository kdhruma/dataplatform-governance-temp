using System;

namespace MDM.Interfaces.Exports
{
    using MDM.Core;

    /// <summary>
    /// Exposes methods or properties to set or get the lookup export profile related information.
    /// </summary>
    public interface ILookupExportProfileData : IMDMObject
    {
        /// <summary>
        /// Represents LookupExportProfileData object in Xml format
        /// </summary>
        /// <returns>String representation of current  LookupExportProfileData object</returns>
        String ToXml();

        /// <summary>
        /// Represents LookupExportProfileData object in Xml format
        /// </summary>
        /// <param name="objectSerialization">Specifies the object serialization type</param>
        /// <returns>String representation of current LookupExportProfileData object</returns>
        String ToXml(ObjectSerialization objectSerialization);
    }
}
