using System;
using MDM.Core;

namespace MDM.BusinessObjects.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    public interface IMDMObjectInfo
    {
        /// <summary>
        /// 
        /// </summary>
        String ObjectType
        {
            get;
            set;
        }

        /// <summary>
        /// Clone ImdmObjectInfo object.
        /// </summary>
        /// <returns>cloned MDMObjectInfo object</returns>
        IMDMObjectInfo Clone();

        /// <summary>
        /// Get Xml representation of MDMObjectInfo
        /// </summary>
        /// <returns>Xml representation of object</returns>
        String ToXml();

        /// <summary>
        /// Get Xml representation of MDMObjectInfo
        /// </summary>
        /// <returns>Xml representation of object</returns>
        String ToXml(ObjectSerialization serialization);
    }
}
