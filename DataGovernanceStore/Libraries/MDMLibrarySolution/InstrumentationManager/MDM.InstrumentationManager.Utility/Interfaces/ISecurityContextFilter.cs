using System;
using System.Collections.ObjectModel;

namespace MDM.Interfaces.Diagnostics
{
    using MDM.BusinessObjects;
    using MDM.BusinessObjects.Diagnostics;

    /// <summary>
    /// Specifies interface for SecurityContext filter
    /// </summary>
    public interface ISecurityContextFilter : ICloneable
    {
        /// <summary>
        /// Specifies User Id filter
        /// </summary>
        Collection<Int32> UserIdList { get; set; }

        /// <summary>
        /// Specifies User Login Name filter
        /// </summary>
        Collection<String> UserLoginNameList { get; set; }

        /// <summary>
        /// Specifies User Role Id filter
        /// </summary>
        Collection<Int32> UserRoleIdList { get; set; }

        /// <summary>
        /// Specifies User Role Name filter
        /// </summary>
        Collection<String> UserRoleNameList { get; set; }

        /// <summary>
        /// Adds <see cref="SecurityContext"/> values into filter collections
        /// </summary>
        /// <param name="securityContext"><see cref="SecurityContext"/> instance which data should be added into filter collections</param>
        void AddSecurityContextData(SecurityContext securityContext);

        /// <summary>
        /// Returns <see cref="SecurityContextFilter"/> in Xml format
        /// </summary>
        /// <returns>String representation of current <see cref="SecurityContextFilter"/></returns>
        String ToXml();
    }
}