using System;
using System.Collections.Generic;

namespace MDM.Interfaces
{
    using MDM.Core;

    /// <summary>
    /// Exposes methods or properties to set or get the collection of MDM objects based on requested object serialization.
    /// </summary>
    public interface IMDMObjectCollection : ICollection<MDMObject>
    {
        #region Methods

        /// <summary>
        /// Get Xml representation of mdmobject collection
        /// </summary>
        /// <returns>Xml representation of mdmobject collection</returns>
        String ToXml();

        /// <summary>
        /// Get Xml representation of mdmobject collection based on requested object serialization
        /// </summary>
        /// <param name="objectSerialization">Type of the serialization option</param>
        /// <returns>Xml representation of mdmobject collection</returns>
        String ToXml(ObjectSerialization objectSerialization);

        #endregion Methods
    }
}
