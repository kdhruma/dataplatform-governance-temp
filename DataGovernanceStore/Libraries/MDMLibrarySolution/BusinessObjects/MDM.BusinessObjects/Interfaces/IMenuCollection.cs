using System;
using System.Collections.Generic;

namespace MDM.Interfaces
{
    using MDM.BusinessObjects;
    using MDM.Core;

    /// <summary>
    /// Exposes methods or properties to set or get menu collection related information.
    /// </summary>
    public interface IMenuCollection : IEnumerable<Menu>
    {
        #region Properties

        #endregion

        #region Methods

        /// <summary>
        /// Get Xml representation of MenuCollection object
        /// </summary>
        /// <returns>Xml string representing the MenuCollection</returns>     
        String ToXml();

        /// <summary>
        /// Get Xml representation of Menu object
        /// </summary>
        /// <param name="objectSerialization">serialization option. Based on the value selected, the different xml representation will be there</param>
        /// <returns>Xml representation of object</returns>
        String ToXml(ObjectSerialization objectSerialization);

        #endregion        
    }
}
