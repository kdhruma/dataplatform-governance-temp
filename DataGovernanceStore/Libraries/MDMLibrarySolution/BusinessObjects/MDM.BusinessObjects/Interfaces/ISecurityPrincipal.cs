using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDM.Interfaces
{
    /// <summary>
    /// Exposes methods or properties to set or get security principal for user.
    /// </summary>
    public interface ISecurityPrincipal
    {
        #region Properties

        /// <summary>
        /// Property denoting the current user identity value for username
        /// </summary>
        Int32 CurrentUserId { get; }

        /// <summary>
        /// Property denoting the login name of the user
        /// </summary>
        String CurrentUserName { get; }

        #endregion

        #region Methods

        #endregion
    }
}
