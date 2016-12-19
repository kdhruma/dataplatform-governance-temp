using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace MDM.Interfaces
{
    using MDM.Core;

    /// <summary>
    /// Exposes methods or properties to set or get security context having all information about MDM user and security permissions.
    /// </summary>
    public interface ISecurityContext
    {
        #region Properties

        /// <summary>
        /// Indicates Id of User
        /// </summary>
        Int32 UserId
        {
            get;
            set;
        }

        /// <summary>
        /// Indicates User Login Name
        /// </summary>
        String UserLoginName
        {
            get;
            set;
        }

        /// <summary>
        /// Indicates User Role Id
        /// </summary>
        Int32 UserRoleId
        {
            get;
            set;
        }

        /// <summary>
        /// Indicates User Role Name
        /// </summary>
        String UserRoleName
        {
            get;
            set;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Represents SecurityContext in xml format
        /// </summary>
        /// <returns>SecurityContext in xml format</returns>
        String ToXml();

        #endregion
    }
}