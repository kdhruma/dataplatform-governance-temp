using System;

namespace MDM.Interfaces.Exports
{
    using MDM.BusinessObjects;
    using MDM.BusinessObjects.Exports;
    using MDM.Core;

    /// <summary>
    /// Exposes methods or properties to set or get the translation export profile related information.
    /// </summary>
    public interface ITranslationExportProfile : IExportProfile
    {
        /// <summary>
        /// Indicates Data of the profile
        /// </summary>
        TranslationExportProfileData DataObject
        { get; set; }

        /// <summary>
        /// Sets Translation Export Profile object from input Xml String
        /// </summary>
        /// <param name="profileDataAsXml">Xml formatted values with which TranslationExportProfile object will be set.</param>
        void LoadProfileDataObject(String profileDataAsXml);
    }
}
