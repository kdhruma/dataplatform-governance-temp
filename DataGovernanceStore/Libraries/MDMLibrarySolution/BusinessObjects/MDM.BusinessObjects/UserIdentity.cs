using System;
using System.Security.Principal;
using System.Runtime.Serialization;

namespace MDM.BusinessObjects
{
    /// <summary>
    /// Specifies the User Identity
    /// </summary>
    [DataContract]
    public class UserIdentity : IIdentity
    {
        #region Fields

        /// <summary>
        /// Field denoting username of the current user
        /// </summary>
        [DataMember]
        private String _currentUsername;

        /// <summary>
        /// 
        /// </summary>
        private String _formsAuthenticationTicket;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor with User Name as input parameter
        /// </summary>		
        public UserIdentity(String userName, String formsAuthenticationTicket)
        {
            _currentUsername = userName;
            _formsAuthenticationTicket = formsAuthenticationTicket;
        }

        #endregion

        #region Properties

        #endregion

        #region Methods

        #endregion

        #region IIdentity Members

        /// <summary>
        /// Property denoting if the user is Authenticated.
        /// </summary>
        public bool IsAuthenticated
        {
            get
            {
                // assumption: all instances of an UserIdentity have already
                // been authenticated because we create this only after validating 
                // credentials
                return true;
            }
        }

        /// <summary>
        /// Property denoting the UserName of the current user.
        /// </summary>
        
        public string Name
        {
            get
            {
                return _currentUsername;
            }
        }

        /// <summary>
        /// Property denoting the Authentication Type used.
        /// </summary>
        public string AuthenticationType
        {
            get
            {
                // the authentication type for us is always database
                return "Forms";
            }
        }

        /// <summary>
        /// Property denoting the Authentication Type used.
        /// </summary>
        [DataMember]
        public string FormsAuthenticationTicket
        {
            get
            {
                // the authentication type for us is always database
                return _formsAuthenticationTicket;
            }
            set
            {
                _formsAuthenticationTicket = value;
            }
        }

        #endregion

    }
}
