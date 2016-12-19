using System;
using System.Collections.Generic;
using MDM.BusinessObjects.Exports;
using MDM.Core;

namespace MDM.Interfaces.Exports
{
    /// <summary>
    /// Exposes methods or properties to set or get the data formatter collection object.
    /// </summary>
    public interface IDataFormatterCollection : ICollection<DataFormatter>
    {
        #region Methods

        /// <summary>
        /// Get Xml representation of data formatter collection
        /// </summary>
        /// <returns>Xml representation of data formatter collection</returns>
        String ToXml();

        /// <summary>
        /// Get Xml representation of data formatter collection based on requested object serialization
        /// </summary>
        /// <param name="objectSerialization">Type of the serialization option</param>
        /// <returns>Xml representation of data formatter collection</returns>
        String ToXml(ObjectSerialization objectSerialization);

        #endregion Methods
    }
}
