using System;
using System.Collections.Generic;

namespace MDM.Interfaces
{
    using MDM.Core;
    using MDM.BusinessObjects;

    /// <summary>
    /// Exposes methods or properties to set or get data processor status collection.
    /// </summary>
    public interface IDataProcessorStatusCollection : IEnumerable<DataProcessorStatus>
    {
        /// <summary>
        /// XML Representation of an Object
        /// </summary>
        /// <returns>XML Representation of an Object</returns>
        String ToXml();
    }
}
