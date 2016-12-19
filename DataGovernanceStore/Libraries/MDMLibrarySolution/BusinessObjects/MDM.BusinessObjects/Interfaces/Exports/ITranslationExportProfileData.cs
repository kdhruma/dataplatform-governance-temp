using System;
using MDM.BusinessObjects.Exports;
using MDM.Core;

namespace MDM.Interfaces.Exports
{
    /// <summary>
    /// Exposes methods or properties to set or get the translation export profile data business object.
    /// </summary>
    public interface ITranslationExportProfileData : IMDMObject
    {
        #region Properties

        

    

        #endregion Properties

        #region Methods

        /// <summary>
        /// Represents translation ExportProfileData in Xml format
        /// </summary>
        /// <returns>translation exportProfileData in Xml format</returns>
        String ToXml();

        /// <summary>
        /// Represents translation ExportProfileData in Xml format based on requested object serialization
        /// </summary>
        /// <param name="objectSerialization">Type of the serialization option</param>
        /// <returns>Xml representation of translationExportProfileData</returns>
        String ToXml(ObjectSerialization objectSerialization);

        #endregion Methods
    }
}
