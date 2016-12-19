using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDM.Interfaces
{
    using MDM.BusinessObjects;
    using MDM.Core;

    /// <summary>
    /// Exposes methods or properties to set or get service status collection.
    /// </summary>
    public interface IServiceStatusCollection : IEnumerable<ServiceStatus>
    {
        #region Properties
        #endregion

        #region Methods

        /// <summary>
        /// XML representation of ServiceStatusCollection
        /// </summary>
        /// <returns>ServiceStatus XML</returns>
        String ToXml();

        #endregion
    }
}
