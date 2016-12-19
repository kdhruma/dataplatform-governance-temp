using System;
using System.Collections.Generic;
using MDM.BusinessObjects.Exports;
using MDM.Core;

namespace MDM.Interfaces.Exports
{
    /// <summary>
    /// Exposes methods or properties to set or get the collection of profile settings.
    /// </summary>
    public interface IProfileSettingCollection : ICollection<ProfileSetting>
    {
        #region Methods        

        /// <summary>
        /// Get Xml representation of profile setting collection
        /// </summary>
        /// <returns>Xml representation of profile setting collection</returns>
        String ToXml();

        /// <summary>
        /// Get Xml representation of profile setting collection based on requested object serialization
        /// </summary>
        /// <param name="objectSerialization">Type of the serialization option</param>
        /// <returns>Xml representation of profile setting collection</returns>
        String ToXml(ObjectSerialization objectSerialization);

        #endregion Methods
    }
}
