using System;
using System.Collections;
using System.Security.Principal;
using System.Security.Claims;
using System.Runtime.Serialization;
using MDM.BusinessObjects.DQM;
using System.Collections.ObjectModel;


namespace MDM.BusinessObjects
{
    using MDM.Core;
    using MDM.Interfaces;
    using MDM.BusinessObjects;
    
    /// <summary>
    /// Specifies security principal for user.
    /// </summary>
    [DataContract]
    public class SecurityPrincipal : ISecurityPrincipal, IPrincipal
    {
        #region Fields

        /// <summary>
        /// Field denoting the current user identity value for username
        /// </summary>
        private Int32 _currentUserId = 0;

        /// <summary>
        /// Field denoting the login name of the user
        /// </summary>
        private String _currentUserName = String.Empty;

        /// <summary>
        /// Field denoting current Locale id of the user
        /// </summary>
        private Int32 _currentUserLocaleId = -1;

        /// <summary>
        /// Field denoting the current locale name of the user
        /// </summary>
        private String _currentUserLocaleName = String.Empty;

        /// <summary>
        /// Field denoting the current System identity for username
        /// </summary>
        private MDMCenterSystem _currentSystemId;

        /// <summary>
        /// Field denoting the time when the user logged in
        /// </summary>
        private String _userLoginTimeStamp = String.Empty;

        ///// <summary>
        ///// Field denoting the value of the forms authentication ticket
        ///// </summary>
        //private String _formsAuthenticatTicket = String.Empty;

        /// <summary>
        /// Field denoting the current authentication type
        /// </summary>
        [DataMember]
        private AuthenticationType _currentAuthenticationType = AuthenticationType.Unknown;

        /// <summary>
        /// Field denoting the user identity we create and associate with this principal
        /// </summary>
        [DataMember]
        private UserIdentity _userIdentity = null;

        /// <summary>
        /// Field denoting the window identity we create and associate with this principal
        /// </summary>
        private WindowsIdentity _windowsIdentity = null;

        /// <summary>
        /// Field denoting the claims identity we create and associate with this principal
        /// </summary>
        [DataMember]
        private ClaimsIdentity _claimsIdentity = null;

        /// <summary>
        /// Field used to stores the list of security Roles the user belongs to
        /// </summary>
        private ArrayList _userRoles = null;

        /// <summary>
        /// Field denoting the Roll Identities the user belongs to
        /// </summary>
        private ArrayList _userRoleIds = null;

        /// <summary>
        /// Field storing the list of security Permissions the user has
        /// </summary>
        private ArrayList _userPermissions = null;

        /// <summary>
        /// Field denoting the list of security Permissions the user has
        /// </summary>
        private UserPreferences _userPreferences = null;

        /// <summary>
        /// Field denoting the user's data level security 
        /// </summary>
        private DataSecurity _userDataSecurity = null;

        /// <summary>
        /// Field denoting the time when the claims token should be checked for update.  
        /// </summary>
        private DateTime _claimsTokenNextUpdateTime = DateTime.MinValue;

        #endregion

        #region Constructors

        /// <summary>
        /// Parameterless constructor
        /// </summary>		
        public SecurityPrincipal(UserIdentity identity)
        {
            this._userIdentity = identity;
            this._currentAuthenticationType = AuthenticationType.Forms;
        }

        /// <summary>
        /// Parameterless constructor
        /// </summary>		
        public SecurityPrincipal(WindowsIdentity identity)
        {
            this._windowsIdentity = identity;
            this._currentAuthenticationType = AuthenticationType.Windows;
        }

        /// <summary>
        /// Instantiates the SecurityPrincipal based on the ClaimsIdentity.
        /// </summary>
        /// <param name="identity"></param>
        public SecurityPrincipal(ClaimsIdentity identity)
        {
            this._claimsIdentity = identity;
            this._currentAuthenticationType = AuthenticationType.Claims;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Property denoting the current user identity value for username
        /// </summary>
        [DataMember]
        public Int32 CurrentUserId
        {
            get { return _currentUserId; }
            set { _currentUserId = value; }
        }

        /// <summary>
        /// Property denoting the login name of the user
        /// </summary>
        [DataMember]
        public String CurrentUserName
        {
            get { return _currentUserName; }
            set { _currentUserName = value; }
        }

        /// <summary>
        /// Property denoting current Locale id of the user
        /// </summary>
        [DataMember]
        public Int32 CurrentUserLocaleId
        {
            get { return _currentUserLocaleId; }
            set { _currentUserLocaleId = value; }
        }

        /// <summary>
        /// Property denoting the current locale name of the user
        /// </summary>
        [DataMember]
        public String CurrentUserLocaleName
        {
            get { return _currentUserLocaleName; }
            set { _currentUserLocaleName = value; }
        }

        /// <summary>
        /// Property denoting the current System identity for username
        /// </summary>
        [DataMember]
        public MDMCenterSystem CurrentSystemId
        {
            get { return _currentSystemId; }
            set { _currentSystemId = value; }
        }

        /// <summary>
        /// Property denoting the time when the user logged in
        /// </summary>
        [DataMember]
        public String UserLoginTimeStamp
        {
            get { return _userLoginTimeStamp; }
            set { _userLoginTimeStamp = value; }
        }

        /// <summary>
        /// Property denoting the current authentication type for the user
        /// </summary>
        public AuthenticationType CurrentAuthenticationType
        {
            get { return _currentAuthenticationType; }
        }

        /// <summary>
        /// Property denoting the user identity we create and associate with this principal
        /// </summary>
        public UserIdentity UserIdentity
        {
            get { return _userIdentity; }
        }

        /// <summary>
        /// Property denoting the window identity we create and associate with this principal
        /// </summary>
        public WindowsIdentity WindowsIdentity
        {
            get { return _windowsIdentity; }
        }

        /// <summary>
        /// Property denoting the claims identity we create and associate with this principal
        /// </summary>
        public ClaimsIdentity ClaimsIdentity
        {
            get { return _claimsIdentity; }
        }

        /// <summary>
        /// Property denoting the list of security Roles the user belongs to
        /// </summary>
        [DataMember]
        public ArrayList UserRoles
        {
            get { return _userRoles; }
            set { _userRoles = value; }
        }

        /// <summary>
        /// Property denoting the Roll Identities the user belongs to
        /// </summary>
        [DataMember]
        public ArrayList UserRoleIds
        {
            get { return _userRoleIds; }
            set { _userRoleIds = value; }
        }

        /// <summary>
        /// Property denoting list of security Permissions the user has
        /// </summary>
        [DataMember]
        public ArrayList UserPermissions
        {
            get { return _userPermissions; }
            set { _userPermissions = value; }
        }

        /// <summary>
        /// Property denoting the list of security Permissions the user has
        /// </summary>
        [DataMember]
        public UserPreferences UserPreferences
        {
            get { return _userPreferences; }
            set { _userPreferences = value; }
        }

        /// <summary>
        /// Property denoting the user's data level security 
        /// </summary>
        [DataMember]
        public DataSecurity UserDataSecurity
        {
            get
            {
                return _userDataSecurity;
            }
            set
            {
                _userDataSecurity = value;
            }
        }

        /// <summary>
        /// Denotes the time when the claims token should be next checked for update.  
        /// </summary>
        public DateTime ClaimsTokenNextUpdateTime
        {
            get
            {
                return _claimsTokenNextUpdateTime;
            }
            set
            {
                _claimsTokenNextUpdateTime = value;
            }
        }

        /// <summary>
        ///Collection of user role ids
        /// </summary>
        /// <returns>returns collection of user role ids</returns>
        public Collection<Int32> GetUserRoleIds()
        {
            Collection<Int32> userRoleIds = new Collection<Int32>();

            if (this._userRoleIds != null && this._userRoleIds.Count > 0)
            {
                foreach (Int32 id in _userRoleIds)
                {
                    userRoleIds.Add(id);
                }
            }

            return userRoleIds;
        }

        #endregion

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public SecurityContext GetSecurityContext()
        {
            Int32 userRoleId = 0;
            String userRoleName = String.Empty;

            if(this._userPreferences != null)
            {
                userRoleId = this._userPreferences.DefaultRoleId;
                userRoleName = this._userPreferences.DefaultRoleName;
            }

            var securityContext = new SecurityContext(this._currentUserId, this._currentUserName, userRoleId, userRoleName);
            return securityContext;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public String ToXml()
        {
            var xml = new XmlOutput();

            xml.Node("SecurityPrincipal").Within()
                .Node("UserId").InnerText(_currentUserId.ToString())
                .Node("UserName").InnerText(_currentUserName)
                .Node("UserLocale").InnerText(_currentUserLocaleId.ToString())
                .Node("UserLocaleName").InnerText(_currentUserLocaleName)
                .Node("SystemId").InnerText(_currentSystemId.ToString())
                .Node("UserLoginTime").InnerText(_userLoginTimeStamp.ToString())
                .Node("AuthenticationType").InnerText(_currentAuthenticationType.ToString())
                .Node("UserIdentity").InnerText((_userIdentity == null) ? "Empty" : _userIdentity.ToString())
                .Node("WindowsIdentity").InnerText((_windowsIdentity == null) ? "Empty" : _windowsIdentity.ToString())
                .Node("Roles").InnerText((_userRoles == null) ? "Empty" : String.Join("|", _userRoles.ToArray()))
                .Node("Permissions").InnerText((_userPermissions == null) ? "Empty" : String.Join("|", _userPermissions.ToArray()))
                .Node("ClaimsTokenNextUpdateTime").InnerText(_claimsTokenNextUpdateTime.ToLongDateString())
                .EndWithin();

            return xml.GetOuterXml();
        }


        #endregion

        #region IPrincipal Members

        /// <summary>
        /// The Identity of the current user.
        /// </summary>
        /// <value>Identity which implements IIdentity interface</value>
        public IIdentity Identity
        {
            get
            {
                IIdentity identity = null;
                switch (this._currentAuthenticationType)
                {
                    case AuthenticationType.Forms:
                        identity = this._userIdentity;
                        break;
                    case AuthenticationType.Windows:
                        identity = this._windowsIdentity;
                        break;
                    case AuthenticationType.Claims:
                        identity = this._claimsIdentity;
                        break;                    
                }
                return identity;
            }
        }

        /// <summary>
        /// Checks if the current user is part of a role.
        /// </summary>
        /// <param name="role">The role to compare</param>
        /// <returns>A Boolean value indicating success or failure.</returns>
        public bool IsInRole(string role)
        {
            return _userRoles.Contains(role);
        }

        #endregion
    }
}
