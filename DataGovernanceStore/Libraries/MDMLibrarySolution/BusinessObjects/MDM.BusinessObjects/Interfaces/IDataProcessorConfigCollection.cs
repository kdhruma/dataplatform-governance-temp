using System;
using System.Collections.Generic;

namespace MDM.Interfaces
{
    using MDM.BusinessObjects;
    using MDM.Core;

    /// <summary>
    /// Exposes methods or properties to set or get data processor configuration collection.
    /// </summary>
    public interface IDataProcessorConfigCollection : IEnumerable<DataProcessorConfig>
    {
        /// <summary>
        /// XML Represntation of an Object
        /// </summary>
        /// <returns>XML Represntation of an Object</returns>
        String ToXml();
    }
}
