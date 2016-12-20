using System;

namespace MDM.Interfaces
{
    using MDM.Core;

    /// <summary>
    /// Exposes methods or properties to set or get the user credential request context.
    /// </summary>
    public interface IUserCredentialRequestContext
    {
        #region Properties

        /// <summary>
        /// Property denoting user login Id
        /// </summary>
        String UserLoginId { get; set; }

        /// <summary>
        /// Property denoting user email Id
        /// </summary>
        String UserEmailId { get; set; }

        /// <summary>
        /// Property denoting request type
        /// </summary>
        UserCredentialRequestType RequestType { get; set; }

        #endregion
    }
}
