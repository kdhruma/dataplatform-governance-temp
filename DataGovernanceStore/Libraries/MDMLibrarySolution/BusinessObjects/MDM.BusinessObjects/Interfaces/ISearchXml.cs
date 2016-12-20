using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDM.Interfaces
{
    using MDM.Core;
    using MDM.BusinessObjects;

    /// <summary>
    /// Exposes methods or properties to set or get the collection of search MDMRule rules.
    /// </summary>
    public interface ISearchXml
    { 
        #region Methods

        /// <summary>
        /// Get the XML 
        /// </summary>
        /// <returns>Xml representation of object</returns>
        String ToXml();

        #endregion
    }
}
