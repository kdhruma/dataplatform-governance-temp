using System;
using System.Collections.Generic;
using MDM.BusinessObjects.Exports;
using MDM.Core;

namespace MDM.Interfaces.Exports
{
    /// <summary>
    /// Exposes methods or properties to set or get the export setting collection.
    /// </summary>
    public interface IExportSettingCollection : ICollection<ExportSetting>
    {
        #region Methods

        /// <summary>
        /// Get Xml representation of exportsetting collection
        /// </summary>
        /// <returns>Xml representation of exportsetting collection</returns>
        String ToXml();

        /// <summary>
        /// Get Xml representation of exportsetting collection based on requested object serialization
        /// </summary>
        /// <param name="objectSerialization">Type of the serialization option</param>
        /// <returns>Xml representation of exportsetting collection</returns>
        String ToXml(ObjectSerialization objectSerialization);

        #endregion Methods
    }
}
