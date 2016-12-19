using System;

namespace MDM.Interfaces.Exports
{
    /// <summary>
    /// Exposes methods or properties to set or get the data model export profile related information.
    /// </summary>
    public interface IDataModelExportProfile : IExportProfile
    {
        /// <summary>
        /// Gets profile data object
        /// </summary>
        /// <returns>Returns data model export profile data object</returns>
        IDataModelExportProfileData GetProfileData();

        /// <summary>
        /// Represents data model export profile data in XML format
        /// </summary>
        /// <returns>String representation of current data model export profile data object</returns>
        new String ToXml();
    }
}