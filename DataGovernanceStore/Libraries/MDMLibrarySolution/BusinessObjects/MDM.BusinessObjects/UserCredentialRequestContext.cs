using System;
using System.Runtime.Serialization;

namespace MDM.BusinessObjects
{
    using MDM.Interfaces;
    using MDM.Core;

    /// <summary>
    /// Represents an object used for requesting user credential request context
    /// </summary>
    [DataContract]
    [KnownType(typeof(UserCredentialRequestType))]
    public class UserCredentialRequestContext : IUserCredentialRequestContext
    {
        #region Fields

        /// <summary>
        /// Field denoting user login Id
        /// </summary>
        private String _userLoginId = String.Empty;

        /// <summary>
        /// Field denoting user email Id
        /// </summary>
        private String _userEmailId = String.Empty;

        /// <summary>
        /// Field denoting the request type
        /// </summary>
        private UserCredentialRequestType _requestType = UserCredentialRequestType.LoginId;

        #endregion Fields

        #region Constructor

        /// <summary>
        /// Default Constructor
        /// </summary>
        public UserCredentialRequestContext()
            : base()
        {

        }

        #endregion Constructor

        #region Properties

        /// <summary>
        /// Property denoting user login Id
        /// </summary>
        [DataMember]
        public String UserLoginId
        {
            get
            {
                return _userLoginId;
            }
            set
            {
                _userLoginId = value;
            }
        }

        /// <summary>
        /// Property denoting user email Id
        /// </summary>
        [DataMember]
        public String UserEmailId
        {
            get
            {
                return _userEmailId;
            }
            set
            {
                _userEmailId = value;
            }
        }

        /// <summary>
        /// Property denoting request type
        /// </summary>
        [DataMember]
        public UserCredentialRequestType RequestType
        {
            get
            {
                return _requestType;
            }
            set
            {
                _requestType = value;
            }
        }

        #endregion
    }
}
