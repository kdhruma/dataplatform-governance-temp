using System;
using System.Collections.Generic;

namespace MDM.Interfaces
{
    using MDM.BusinessObjects.Denorm;

    /// <summary>
    /// Exposes methods or properties to set or get entity processor error log collection.
    /// </summary>
    public interface IEntityProcessorErrorLogCollection : IEnumerable<EntityProcessorErrorLog>
    {
        /// <summary>
        /// XML presentaion of EntityProcessorErrorLogCollection
        /// </summary>
        /// <returns>XML presentaion of EntityProcessorErrorLogCollection</returns>
        string ToXml();
    }
}
