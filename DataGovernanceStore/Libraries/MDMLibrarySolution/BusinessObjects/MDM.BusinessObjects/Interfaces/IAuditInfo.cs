using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDM.Interfaces
{
    using MDM.Core;

    /// <summary>
    /// Exposes methods or properties to set or get audit information.
    /// </summary>
    public interface IAuditInfo : IMDMObject
    {
        #region Properties
        
        /// <summary>
        /// Property denoting UserLogin of AuditInfo Object
        /// </summary>
        String UserLogin { get; set; }

        /// <summary>
        /// Property denoting ChangeDateTime of AuditInfo Object
        /// </summary>
        DateTime ChangeDateTime { get; set; }
        
        #endregion

        #region Methods

        /// <summary>
        /// Get Xml representation of AuditInfo object
        /// </summary>   
        /// <returns>Xml representation of object</returns>
        String ToXml();

        /// <summary>
        /// Get Xml representation of AuditInfo object
        /// </summary>
        /// <param name="objectSerialization">serialization option. Based on the value selected, the different xml representation will be there</param>
        /// <returns>Xml representation of object</returns>
        String ToXml(ObjectSerialization objectSerialization);

        #endregion
    }
}
