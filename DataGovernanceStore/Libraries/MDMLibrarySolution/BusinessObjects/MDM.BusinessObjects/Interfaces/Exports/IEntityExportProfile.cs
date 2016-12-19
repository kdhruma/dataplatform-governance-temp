using System;

namespace MDM.Interfaces.Exports
{
    using MDM.BusinessObjects;
    using MDM.BusinessObjects.Exports;
    using MDM.Core;

    /// <summary>
    /// Exposes methods or properties to set or get the export profile related information.
    /// </summary>
    public interface IEntityExportProfile : IExportProfile
    {
        /// <summary>
        /// 
        /// </summary>
        EntityExportProfileData DataObject
        { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        T GetProfileDataObject<T>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="profileDataAsXml"></param>
        void LoadProfileDataObject(String profileDataAsXml);
    }
}
