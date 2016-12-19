using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Threading;
using System.Security.Principal;
using System.Security.Claims;

namespace MDM.AdminManager.Business
{
    using MDM.Core;
    using MDM.BusinessObjects;
    using MDM.Utility;
    using MDM.CacheManager.Business;
    using MDM.ExceptionManager;
    using MDM.BufferManager;
        
    public class SecurityPrincipalBL : BusinessLogicBase
    {
        #region Fields

        /// <summary>
        /// Specifies whether the Distributed Cache With Notification is Enabled.
        /// </summary>
        private static Boolean boolIsDistributedCacheWithNotificationEnabled = false;

        #endregion

        #region Constructors

        static SecurityPrincipalBL()
        {
            try
            {
                AppConfigBL appConfigBL = new AppConfigBL();

                AppConfig appConfig = appConfigBL.Get("MDMCenter.DistributedCacheWithNotification.Enabled");

                if (appConfig != null && appConfig.Value != null)
                    boolIsDistributedCacheWithNotificationEnabled = ValueTypeHelper.BooleanTryParse(appConfig.Value, true);
            }
            catch (Exception ex)
            {
                ExceptionHandler exceptionHandler = new ExceptionHandler(ex);
            }
        }

        #endregion

        #region Properties

        #endregion

        #region Methods

        /// <summary>
        /// Obtain the Security Principal from the database
        /// </summary>
        /// <param name="userName">The username for the security principal</param>
        /// <param name="system">The sub system the principal is being used</param>
        /// <returns></returns>
        public SecurityPrincipal Get(String userName, MDMCenterSystem system)
        {
            return this.Get(userName, AuthenticationType.Forms, system, "NoTimeStamp");
        }

        /// <summary>
        /// Obtain the Security Principal from the database
        /// </summary>
        /// <param name="userName">The username for the security principal</param>
        /// <param name="system">The sub system the principal is being used</param>
        /// <returns></returns>
        public SecurityPrincipal Get(String userName, AuthenticationType authenticationType, MDMCenterSystem system)
        {
            return this.Get(userName, authenticationType, system, "NoTimeStamp");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="authenticationType"></param>
        /// <param name="system"></param>
        /// <param name="userLoginTimeStamp"></param>
        /// <returns></returns>
        public SecurityPrincipal Get(String userName, AuthenticationType authenticationType, MDMCenterSystem system, String userLoginTimeStamp)
        {
            SecurityPrincipal securityPrincipal = null;

            string securityPrincipalCacheKey = String.Concat("SecurityPrincipal_", userName.ToLower());
            if (!String.IsNullOrEmpty(userLoginTimeStamp))
            {
                securityPrincipalCacheKey += "_" + userLoginTimeStamp;
            }

            //check if the security principal is in cache
            ICache cache = CacheFactory.GetCache();

            if (boolIsDistributedCacheWithNotificationEnabled)
            {
                SecurityPrincipalBufferManager securityPrincipalBufferManager = new SecurityPrincipalBufferManager();
                securityPrincipal = securityPrincipalBufferManager.GetSecurityPrincipal(userName);
            }
            else
            {
                //check if cache is not null before checking if the key is found
                if (cache != null && cache[securityPrincipalCacheKey] != null)
                {
                    securityPrincipal = cache[securityPrincipalCacheKey] as SecurityPrincipal;
                }
            }
            
            if(securityPrincipal == null)
            {
                String formsAuthenticationTicket = (String)cache.Get(CacheKeyGenerator.GetFormAuthenticationTicketCacheKey(userName));

                if (String.IsNullOrWhiteSpace(formsAuthenticationTicket))
                {
                    formsAuthenticationTicket = String.Empty;
                }
                else
                {
                    cache.Remove(CacheKeyGenerator.GetFormAuthenticationTicketCacheKey(userName));
                }

                if(authenticationType == AuthenticationType.Forms)
                {
                    //Here, currentUserSecurityPrincipal is stored in local cache means its live reference. 
                    //Thus, Updating currentUserSecurityPrincipal.FormsAuthenticationTicket would update cache too..

                    securityPrincipal = this.Create(userName, formsAuthenticationTicket, system, userLoginTimeStamp);
                    //currentUserSecurityPrincipal.FormsAuthenticationTicket = formsAuthenticationTicket;
                }
                else
                {
                    WindowsIdentity _windowsIdentity = SecurityPrincipalHelper.GetCurrentWindowsIdentity();

                    if (_windowsIdentity != null)
                    {
                        securityPrincipal = this.Create(userName, new WindowsIdentity(_windowsIdentity.Token), system, userLoginTimeStamp);
                    }
                    else
                    {
                        throw new ApplicationException("The authorization is not correct for this 'Windows' authentication. Contact system administrator.");
                    }   
                }
            }

            return securityPrincipal;
        }

        /// <summary>
        /// Create a SecurityPrincipal for a specific user
        /// </summary>
        /// <param name="userName">Field denoting the login name of the user.</param>
        /// <param name="system">Field denoting the MDMCenterSystem.</param>
        /// <returns>Returns a SecurityPrincipal for a specific user </returns>
        public SecurityPrincipal GetSecurityPrincipalForWindowsAuthentication(String userName, MDMCenterSystem system)
        {
            if (Constants.TRACING_ENABLED)
            MDMTraceHelper.StartTraceActivity("AdminManager.SecurityPrincipal.Get", false);

            SecurityPrincipalBufferManager securityPrincipalBufferManager = new SecurityPrincipalBufferManager();
            SecurityPrincipal securityPrincipal = securityPrincipalBufferManager.GetSecurityPrincipal(userName);
            if (securityPrincipal == null)
            {
                WindowsIdentity windowsIdentity = SecurityPrincipalHelper.GetCurrentWindowsIdentity();
                securityPrincipal = Create(userName, AuthenticationType.Windows, windowsIdentity, null, String.Empty, system, null);
            }

            if (Constants.TRACING_ENABLED)
            MDMTraceHelper.StopTraceActivity("AdminManager.SecurityPrincipal.Get");
            return securityPrincipal;
        }

        /// <summary>
        /// Create a SecurityPrincipal for a specific user
        /// </summary>
        /// <param name="userName">Field denoting the login name of the user</param>        
        /// <param name="formsAuthenticationTicket">Field denoting identity of the user (using forms authentication).</param>
        /// <param name="system">Field denoting the MDMCenterSystem name</param>
        /// <returns>Returns a SecurityPrincipal for a specific user </returns>
        public SecurityPrincipal GetSecurityPrincipalForFormsAuthentication(String userName, String formsAuthenticationTicket, MDMCenterSystem system)
        {
            if (Constants.TRACING_ENABLED)
            MDMTraceHelper.StartTraceActivity("AdminManager.SecurityPrincipal.Get", false);

            SecurityPrincipalBufferManager securityPrincipalBufferManager = new SecurityPrincipalBufferManager();
            SecurityPrincipal securityPrincipal = securityPrincipalBufferManager.GetSecurityPrincipal(userName);
            if (securityPrincipal == null)
                securityPrincipal = Create(userName, AuthenticationType.Forms, null, null, formsAuthenticationTicket, system, null);

            MDMTraceHelper.StopTraceActivity("AdminManager.SecurityPrincipal.Get");
            return securityPrincipal;
        }

        /// <summary>
        /// Create a SecurityPrincipal for a specific user
        /// </summary>
        /// <param name="userName">Field denoting the login name of the user</param>        
        /// <param name="claimsIdentity">Field denoting identity of user, when using claims based authentication</param>
        /// <param name="system">Field denoting the MDMCenterSystem name</param>
        /// <returns>Returns a SecurityPrincipal for a specific user </returns>
        public SecurityPrincipal GetSecurityPrincipalForClaimsBasedAuthentication(String userName, ClaimsIdentity claimsIdentity, MDMCenterSystem system)
        {
            MDMTraceHelper.StartTraceActivity("AdminManager.SecurityPrincipal.Get", false);

            SecurityPrincipalBufferManager securityPrincipalBufferManager = new SecurityPrincipalBufferManager();
            SecurityPrincipal securityPrincipal = securityPrincipalBufferManager.GetSecurityPrincipal(userName);
            if (securityPrincipal == null)
                securityPrincipal = Create(userName, AuthenticationType.Claims, null, claimsIdentity, String.Empty, system, null);

            if (Constants.TRACING_ENABLED)
            MDMTraceHelper.StopTraceActivity("AdminManager.SecurityPrincipal.Get");
            return securityPrincipal;
        }

        /// <summary>
        /// Create a SecurityPrincipal for a specific user
        /// </summary>
        /// <param name="userName">Field denoting the login name of the user</param>
        /// <param name="system">Field denoting the MDMCenterSystem name</param>
        /// <param name="authenticationType">Field denoting type of Authentication</param>
        /// <returns>Returns a SecurityPrincipal for a specific user </returns>
        public SecurityPrincipal Create(String userName, WindowsIdentity windowsIdentity, MDMCenterSystem system, String userLoginTimeStamp)
        {
            return Create(userName, AuthenticationType.Windows, windowsIdentity, null, String.Empty, system, userLoginTimeStamp);
        }

        /// <summary>
        /// Create a SecurityPrincipal for a specific user
        /// </summary>
        /// <param name="userName">Field denoting the login name of the user</param>
        /// <param name="system">Field denoting the MDMCenterSystem name</param>
        /// <param name="authenticationType">Field denoting type of Authentication</param>
        /// <returns>Returns a SecurityPrincipal for a specific user </returns>
        public SecurityPrincipal Create(String userName, String formsAuthenticationTicket, MDMCenterSystem system, String userLoginTimeStamp)
        {
            return Create(userName, AuthenticationType.Forms, null, null, formsAuthenticationTicket, system, userLoginTimeStamp);
        }

        /// <summary>
        /// Create a SecurityPrincipal for a specific user
        /// </summary>
        /// <param name="userName">Field denoting the login name of the user</param>
        /// <param name="authenticationType">Field denoting type of Authentication</param>
        /// <param name="windowsIdentity">Field denoting identity of user, when using windows authentication</param>
        /// <param name="claimsIdentity">Field denoting identity of user, when using claims based authentication</param>
        /// <param name="system">Field denoting the MDMCenterSystem name</param>
        /// <returns>Returns a SecurityPrincipal for a specific user </returns>
        public SecurityPrincipal Create(String userName, AuthenticationType authenticationType, WindowsIdentity windowsIdentity, ClaimsIdentity claimsIdentity,
            String formsAuthenticationTicket, MDMCenterSystem system, String userLoginTimeStamp)
        {
            SecurityPrincipal securityPrincipal = null;

            if (authenticationType == AuthenticationType.Forms)
                securityPrincipal = new SecurityPrincipal(new UserIdentity(userName, formsAuthenticationTicket));
            else if (authenticationType == AuthenticationType.Windows)
                securityPrincipal = new SecurityPrincipal(windowsIdentity);
            else if (authenticationType == AuthenticationType.Claims)
                securityPrincipal = new SecurityPrincipal(claimsIdentity);
            else
                throw new ArgumentException(String.Format("Provided authentication type '{0}' is not supported. Contact system administrator.", authenticationType.ToString()));

            // fill login information into security principal
            securityPrincipal.CurrentUserName = userName;

            securityPrincipal.CurrentSystemId = system;

            if (String.IsNullOrEmpty(userLoginTimeStamp))
            {
                userLoginTimeStamp = DateTime.Now.ToString();
            }

            securityPrincipal.UserLoginTimeStamp = userLoginTimeStamp;

            //get the values for the current user and populate the values
            UserPrincipalBL userPrincipalData = new UserPrincipalBL();
            securityPrincipal.UserPermissions = userPrincipalData.GetUserPermissions(userName);

            //get user preferences
            UserPreferencesBL userPreferencesBL = new UserPreferencesBL();
            securityPrincipal.UserPreferences = userPreferencesBL.GetUserPreferences(userName);

            //TODO remove redundant storage of same values in multiple places
            if (securityPrincipal.UserPreferences != null)
            {
                //Move some of the property values from User preferences to Security principal
                securityPrincipal.CurrentUserId = securityPrincipal.UserPreferences.LoginId;
                securityPrincipal.CurrentUserLocaleId = (Int32)securityPrincipal.UserPreferences.DataLocale;
                securityPrincipal.CurrentUserLocaleName = securityPrincipal.UserPreferences.DataLocale.ToString();

                //Prepare user role name and ids list..
                SecurityRoleBL securityUserManager = new SecurityRoleBL();
                SecurityRoleCollection securityRoles = securityUserManager.GetUserRoles(securityPrincipal.UserPreferences.LoginId, userName);

                ArrayList userRoles = null;
                ArrayList userRoleIds = null;

                if (securityRoles != null)
                {
                    userRoles = new ArrayList();
                    userRoleIds = new ArrayList();

                    foreach (SecurityRole role in securityRoles)
                    {
                        userRoles.Add(role.Name);
                        userRoleIds.Add(role.Id);
                    }
                }

                securityPrincipal.UserRoles = userRoles;
                securityPrincipal.UserRoleIds = userRoleIds;
            }
            else
            {
                /// Added the below condition to check if the user is available in the system.
                SecurityUser securityUser = new SecurityUserBL().GetUser(userName);
                if (securityUser != null && securityUser.Id > 0)
                {
                    throw new ArgumentException(String.Format("User preference is not available for the user '{0}'. Contact system administrator.", userName));                    
                }
                else
                {
                    throw new ArgumentException(String.Format("The user '{0}' does not exist in the system. Contact system administrator.", userName));
                }
            }

            //add in cache
            SecurityPrincipalBufferManager securityPrincipalBufferManager = new SecurityPrincipalBufferManager();
            securityPrincipalBufferManager.UpdateSecurityPrincipal(securityPrincipal, 3);

            return securityPrincipal;
        }

        #endregion
    }
}
