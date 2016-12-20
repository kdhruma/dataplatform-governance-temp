using System;

namespace MDM.Interfaces.Exports
{
    using MDM.Core;

    /// <summary>
    /// Exposes methods or properties to set or get the export profile business object.
    /// </summary>
    public interface IExportProfile : IBaseProfile, IMDMObject
    {
        /// <summary>
        /// Indicates type of export profile
        /// </summary>
        new ExportProfileType ProfileType { get; set; }

        /// <summary>
        /// Indicates Profile Data of the profile
        /// </summary>
        String ProfileData { get; set; }

        /// <summary>
        /// Indicates Template Id for the profile
        /// </summary>
        Int32 TemplateId { get; set; }

        /// <summary>
        /// Indicates Application Config Id for the profile
        /// </summary>
        Int64 ApplicationConfigId { get; set; }

        /// <summary>
        /// Indicates Is System Profile for the profile
        /// </summary>
        Boolean IsSystemProfile { get; set; }

        /// <summary>
        /// Property denoting that profile is accessible for all Users
        /// </summary>
        Boolean IsPublicProfile { get; set; }

        /// <summary>
        /// Indicates Create User of the profile
        /// </summary>
        String CreateUser { get; set; }

        /// <summary>
        /// Serialize object in XML
        /// </summary>
        /// <returns>Serialize object in XML</returns>
        String ToXml();
    }
}
