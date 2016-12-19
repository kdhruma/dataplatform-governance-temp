using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;

namespace MDM.Interfaces
{
    using MDM.Core;
    using MDM.BusinessObjects;
    using MDM.BusinessObjects.Imports;

    /// <summary>
    /// Exposes methods or properties to set or get the collection of reader settings.
    /// </summary>
    public interface IReaderSettingCollection : IEnumerable<ReaderSetting>
    {
        #region Properties

        #endregion

        #region Methods

        /// <summary>
        /// Get Xml representation of ReaderSettingCollection object
        /// </summary>
        /// <returns>Xml string representing the ReaderSettingCollection</returns>
        String ToXml();

        /// <summary>
        /// Get Xml representation of ReaderSettingCollection object
        /// </summary>
        /// <param name="serialization">serialization option. Based on the value selected, the different xml representation will be there</param>
        /// <returns>Xml representation of object</returns>
        String ToXml(ObjectSerialization serialization);

        #endregion
    }
}
