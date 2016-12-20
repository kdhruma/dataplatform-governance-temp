using System;
using System.Collections.Generic;
using MDM.BusinessObjects.Exports;
using MDM.Core;

namespace MDM.Interfaces.Exports
{
    /// <summary>
    /// Exposes methods or properties to set or get the collection of export scope.
    /// </summary>
    public interface IExportScopeCollection : ICollection<ExportScope>
    {
        #region Methods

        /// <summary>
        /// Get Xml representation of export scope collection
        /// </summary>
        /// <returns>Xml representation of export scope collection</returns>
        String ToXml();

        /// <summary>
        /// Get Xml representation of export scope collection based on requested object serialization
        /// </summary>
        /// <param name="objectSerialization">Type of the serialization option</param>
        /// <returns>Xml representation of export scope collection</returns>
        String ToXml(ObjectSerialization objectSerialization);

        #endregion Methods
    }
}
