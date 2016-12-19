using System;
using System.Collections.Generic;
using MDM.BusinessObjects.Exports;
using MDM.Core;

namespace MDM.Interfaces.Exports
{
    /// <summary>
    /// Exposes methods or properties to set or get the collection of MDM object groups.
    /// </summary>
    public interface IMDMObjectGroupCollection : ICollection<MDMObjectGroup>
    {
        #region Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objectType"></param>
        /// <returns></returns>
        IMDMObjectGroup GetMDMObjectGroup(ObjectType objectType);

        /// <summary>
        /// Get Xml representation of mdmobjectgroup collection
        /// </summary>
        /// <returns>Xml representation of mdmobjectgroup collection</returns>
        String ToXml();

        /// <summary>
        /// Get Xml representation of mdmobjectgroup collection based on requested object serialization
        /// </summary>
        /// <param name="objectSerialization">Type of the serialization option</param>
        /// <returns>Xml representation of mdmobjectgroup collection</returns>
        String ToXml(ObjectSerialization objectSerialization);

        #endregion Methods
    }
}
