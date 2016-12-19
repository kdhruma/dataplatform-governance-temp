using System;

namespace MDM.Interfaces.Exports
{
    using MDM.BusinessObjects.Exports;

    /// <summary>
    /// Exposes methods or properties to set or get the lookup export profile.
    /// </summary>
    public interface ILookupExportProfile : IExportProfile
    {
        #region Properties

        /// <summary>
        /// Property denoting Lookup export profile data
        /// </summary>
        LookupExportProfileData LookupProfileData
        { get; set; }

        #endregion 

        #region Methods

        /// <summary>
        /// Load the Profile Data object based on the profile data.
        /// </summary>
        /// <param name="profileDataAsXml">Indicates the profile data as xml format</param>
        void LoadProfileDataObject(String profileDataAsXml);
    
        /// <summary>
        /// Represents LookupExportProfile in Xml format 
        /// </summary>
        /// <returns>LookupExport profile object as string format</returns>
        new String ToXml();

        #endregion 
    }
}
