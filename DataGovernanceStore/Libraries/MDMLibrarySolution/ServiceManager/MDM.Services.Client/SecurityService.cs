using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.ServiceModel;
using System.Security.Principal;
using System.Security.Claims;
using System.Diagnostics;
using System.Linq;

namespace MDM.Services
{
    using MDM.Core;
    using MDM.Core.Exceptions;
    using MDM.BusinessObjects;
    using SSC = MDM.Services.SecurityServiceClient;
    using MDM.Interfaces;
    using MDM.Utility;
    using MDM.Services.ServiceProxies;
    using MDM.WCFServiceInterfaces;

    /// <summary>
    /// Security Service facilitates creating, updating, deleting an user, and getting the roles in MDMCenter.
    /// </summary>
    public class SecurityService : ServiceClientBase
    {
        #region Fields

        #endregion

        #region Properties

        #endregion

        #region Constructors

        /// <summary>
        /// Use this default constructor only when calling MDM service within the execution context of an MDM Application. 
        /// Example: Service call from the MDM business rule.
        /// This would try to use current execution security context for service authentication purpose. 
        /// </summary>
        public SecurityService()
            : base(typeof(SecurityService))
        {
        }

        /// <summary>
        /// Use this default constructor with security principal only when calling MDM service within the execution context of an MDM Application. 
        /// Example: Service call from the MDM business rule.
        /// This would try to use current execution security context for service authentication purpose. 
        /// </summary>
        /// <param name="iSecurityPrincipal">Current security principal</param>
        public SecurityService(ISecurityPrincipal iSecurityPrincipal)
            : base(typeof(SecurityService), iSecurityPrincipal)
        {
        }

        /// <summary>
        /// Use this default constructor with binding configuration name only when calling MDM service within the execution context of an MDM Application. 
        /// Example: Service call from the MDM business rule.
        /// This would try to use current execution security context for service authentication purpose. 
        /// </summary>
        /// <param name="endPointConfigurationName">Indicates the EndPointConfigurationName</param>
        public SecurityService(String endPointConfigurationName)
            : base(typeof(SecurityService), endPointConfigurationName)
        {
        }

        /// <summary>
        /// Use this constructor for form based authentication passing username and password
        /// </summary>
        /// <param name="endPointConfigurationName">Indicates the EndPointConfigurationName</param>
        /// <param name="userName">Name of the Login User</param>
        /// <param name="password">Password of the User</param>
        public SecurityService(String endPointConfigurationName, String userName, String password)
            : base(endPointConfigurationName, userName, password)
        {

        }

        /// <summary>
        /// Use this constructor for window authentication passing current windows identity.
        /// </summary>
        /// <param name="endPointConfigurationName">Indicates the EndPointConfigurationName</param>
        /// <param name="userIdentity">Indicates User's Identity</param>
        public SecurityService(String endPointConfigurationName, IIdentity userIdentity)
            : base(endPointConfigurationName, userIdentity)
        {
        }

        /// <summary>
        /// Constructor with client configuration.
        /// </summary>
        /// <param name="wcfClientConfiguration">Indicates WCF Client Configuration</param>
        public SecurityService(IWCFClientConfiguration wcfClientConfiguration)
            : base(wcfClientConfiguration)
        {
        }

        /// <summary>
        /// Constructor with endPointConfigurationName, authenticationType, userIdentity, userName and password
        /// </summary>
        /// <param name="endPointConfigurationName">Indicates the EndPointConfigurationName</param>
        /// <param name="authenticationType">Indicates Type of Authentication i.e Windows/Forms</param>
        /// <param name="userIdentity">Indicates INdentity of the Login User</param>
        /// <param name="userName">Name of the Login User</param>
        /// <param name="password">Password of the User</param>
        public SecurityService(String endPointConfigurationName, AuthenticationType authenticationType, IIdentity userIdentity, String userName, String password)
            : base(endPointConfigurationName, authenticationType, userIdentity, userName, password)
        {
        }

        /// <summary>
        /// Constructor with endPointConfigurationName, endPointAddress, authenticationType, userIdentity, userName and password
        /// </summary>
        /// <param name="endPointConfigurationName">Indicates the EndPointConfigurationName</param>
        /// <param name="endPointAddress"> Provides a unique network address that a client uses to communicate with a service endpoint.</param>
        /// <param name="authenticationType">Indicates Type of Authentication i.e Windows/Forms</param>
        /// <param name="userIdentity">Indicates INdentity of the Login User</param>
        /// <param name="userName">Name of the Login User</param>
        /// <param name="password">Password of the User</param>
        public SecurityService(String endPointConfigurationName, EndpointAddress endPointAddress, AuthenticationType authenticationType, IIdentity userIdentity, String userName, String password)
            : base(endPointConfigurationName, endPointAddress, authenticationType, userIdentity, userName, password)
        {
        }

        #endregion

        #region Security

        /// <summary>
        /// Authenticates users 
        /// </summary>
        /// <param name="userLoginName">Indicates the name of the login user</param>
        /// <param name="password">Indicates the password of the login user</param>
        /// <returns>Returns as true, if user is authenticated</returns>
        /// <exception cref="EndpointNotFoundException">Thrown when a remote endpoint could not be found or reached</exception>
        /// <exception cref="InvalidOperationException">Thrown when a method call is invalid</exception>
        /// <exception cref="TimeoutException">Thrown when the time allotted for an operation has expired</exception>
        /// <exception cref="MDMOperationException">Thrown when the MDM operation fails</exception>
        /// <exception cref="CommunicationException">Thrown when there is a communication error in either the service or client application</exception>
        public Boolean AuthenticateUser(String userLoginName, String password)
        {
            return MakeServiceCall("AuthenticateUser",
                                   "AuthenticateUser",
                                   service =>
                                       {
                                           if (Constants.TRACING_ENABLED)
                                           {
                                               MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("Requested Authenticate User for UserLoginName: {0}", userLoginName));
                                           }
                                           return service.AuthenticateUser(userLoginName, password);
                                       });
        }

        /// <summary>
        /// Gets the User Preference based on User Login
        /// </summary>
        /// <example> 
        /// <code>
        /// private static IUserPreferences GetUserPreference()
        /// {
        ///     // Get MDM data service
        ///     var securityService = GetSecurityService();
        /// 
        ///     // Return roles associated with the User login  
        ///     const String userLoginName = "cfadmin";
        /// 
        ///     // Make a WCF call and get a User Preferences for the given User Login Name
        ///     IUserPreferences userPreferences = securityService.GetUserPreference(userLoginName);
        /// 
        ///     return userPreferences;
        ///   }
        /// </code>
        /// </example>
        /// <param name="userLoginName">Indicates UserLogin</param>
        /// <returns>User's Preferences</returns>
        /// <exception cref="EndpointNotFoundException">Thrown when a remote endpoint could not be found or reached</exception>
        /// <exception cref="InvalidOperationException">Thrown when a method call is invalid</exception>
        /// <exception cref="TimeoutException">Thrown when the time allotted for an operation has expired</exception>
        /// <exception cref="MDMOperationException">Thrown when the MDM operation fails</exception>
        /// <exception cref="CommunicationException">Thrown when there is a communication error in either the service or client application</exception>
        public IUserPreferences GetUserPreference(String userLoginName)
        {
            return MakeServiceCall("GetUserPreference",
                                   "GetUserPreference",
                                   service =>
                                       {
                                           if (Constants.TRACING_ENABLED)
                                           {
                                               MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("Requested Get User Preference for UserLoginName: {0}", userLoginName));
                                           }
                                           return service.GetUserPreference(userLoginName);
                                       });
        }

        /// <summary>
        /// Gets all User Roles based on Login Id and Name
        /// </summary>
        /// <example> 
        /// <code> 
        /// private static ArrayList GetUserRoles()
        /// {
        ///    // Get MDM data service
        ///     var securityService = GetSecurityService();
        /// 
        ///    // Return User login associated roles 
        ///     const Int32 loginId = 1;
        ///     const String userLoginName = "cfadmin";
        /// 
        ///    // Make a WCF call and get all User Roles for the given Login Id and User Login Name
        ///     ArrayList userRoles = securityService.GetUserRoles(loginId, userLoginName);
        /// 
        ///     return userRoles;
        ///  }
        /// </code>
        /// </example>
        /// <param name="LoginId">Indicates the login Id</param>
        /// <param name="userLoginName">Indicates the user login name</param>
        /// <returns>Returns list of Roles</returns>
        /// <exception cref="EndpointNotFoundException">Thrown when a remote endpoint could not be found or reached</exception>
        /// <exception cref="InvalidOperationException">Thrown when a method call is invalid</exception>
        /// <exception cref="TimeoutException">Thrown when the time allotted for an operation has expired</exception>
        /// <exception cref="MDMOperationException">Thrown when the MDM operation fails</exception>
        /// <exception cref="CommunicationException">Thrown when there is a communication error in either the service or client application</exception>
        public ArrayList GetUserRoles(int LoginId, String userLoginName)
        {
            return MakeServiceCall("GetUserRoles",
                                   "GetUserRoles",
                                   service =>
                                       {
                                           if (Constants.TRACING_ENABLED)
                                           {
                                               MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("Requested Get User Roles for LoginId: {0}, userLoginName: {1}", LoginId, userLoginName));
                                           }
                                           SecurityRoleCollection rolecollection = service.GetUserRoles(LoginId, userLoginName);
                                           ArrayList roles = new ArrayList();
                                           if (rolecollection != null &&
                                               rolecollection.Count > 0)
                                           {
                                               foreach (SecurityRole role in rolecollection)
                                               {
                                                   roles.Add(role.Name);
                                               }
                                           }
                                           return roles;
                                       });
        }

        /// <summary>
        /// Get Users in Particular Role
        /// </summary>
        /// <param name="roleId">Indicates Role Id</param>
        /// <returns>Collection of Users</returns>
        /// <exception cref="EndpointNotFoundException">Thrown when a remote endpoint could not be found or reached</exception>
        /// <exception cref="InvalidOperationException">Thrown when a method call is invalid</exception>
        /// <exception cref="TimeoutException">Thrown when the time allotted for an operation has expired</exception>
        /// <exception cref="MDMOperationException">Thrown when the MDM operation fails</exception>
        /// <exception cref="CommunicationException">Thrown when there is a communication error in either the service or client application</exception>
        public Collection<SecurityUser> GetUsersInRole(int roleId)
        {
            return MakeServiceCall("GetUsersInRole",
                                   "GetUsersInRole",
                                   service =>
                                       {
                                           if (Constants.TRACING_ENABLED)
                                           {
                                               MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("Requested Get Users for roleId: {0}", roleId));
                                           }
                                           return service.GetUsersInRole(roleId);
                                       });
        }

        /// <summary>
        /// Loads all object permissions for the requested user Id
        /// </summary>
        /// <param name="userId">User Id permissions needs to be loaded</param>
        /// <returns></returns>
        /// <exception cref="EndpointNotFoundException">Thrown when a remote endpoint could not be found or reached</exception>
        /// <exception cref="InvalidOperationException">Thrown when a method call is invalid</exception>
        /// <exception cref="TimeoutException">Thrown when the time allotted for an operation has expired</exception>
        /// <exception cref="MDMOperationException">Thrown when the MDM operation fails</exception>
        /// <exception cref="CommunicationException">Thrown when there is a communication error in either the service or client application</exception>
        public void LoadUserPermissions(Int32 userId)
        {
            MakeServiceCall<Object>("LoadUserPermissions",
                                    "LoadUserPermissions",
                                    service =>
                                        {
                                            if (Constants.TRACING_ENABLED)
                                            {
                                                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("Requested Load User Permissions for userId: {0}", userId));
                                            }
                                            service.LoadUserPermissions(userId);
                                            return null;
                                        });
        }
        
        /// <summary>
        /// Process the user preferences for the specified user login
        /// </summary>
        /// <param name="userLogin">Indicates user login name</param>
        /// <param name="iUserPreferences">Indicates user preferences to process</param>
        /// <param name="iCallerContext">Indicates the name of application and the module that are performing the action</param>
        /// <returns>Returns true if process is successful</returns>
        /// <exception cref="EndpointNotFoundException">Thrown when a remote endpoint could not be found or reached</exception>
        /// <exception cref="InvalidOperationException">Thrown when a method call is invalid</exception>
        /// <exception cref="TimeoutException">Thrown when the time allotted for an operation has expired</exception>
        /// <exception cref="MDMOperationException">Thrown when the MDM operation fails</exception>
        /// <exception cref="CommunicationException">Thrown when there is a communication error in either the service or client application</exception>
        public Boolean ProcessUserPreferences(String userLogin, IUserPreferences iUserPreferences, ICallerContext iCallerContext)
        {
            return MakeServiceCall("ProcessUserPreferences",
                                   "ProcessUserPreferences",
                                   service =>
                                       {
                                           if (Constants.TRACING_ENABLED)
                                           {
                                               MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("SecurityServiceClient.ProcessUserReferences with userLogin: {0}", userLogin));
                                           }
                                           return service.ProcessUserPreferences(userLogin, iUserPreferences as UserPreferences, FillDiagnosticTraces(iCallerContext));
                                       });
        }

        /// <summary>
        /// Get available Menus for given User
        /// </summary>
        /// <param name="userLogin">Indicates login User Name</param>
        /// <returns>All Available Menus for User</returns>
        /// <exception cref="EndpointNotFoundException">Thrown when a remote endpoint could not be found or reached</exception>
        /// <exception cref="InvalidOperationException">Thrown when a method call is invalid</exception>
        /// <exception cref="TimeoutException">Thrown when the time allotted for an operation has expired</exception>
        /// <exception cref="MDMOperationException">Thrown when the MDM operation fails</exception>
        /// <exception cref="CommunicationException">Thrown when there is a communication error in either the service or client application</exception>
        public IMenuCollection GetUserMenus(String userLogin)
        {
            return MakeServiceCall("GetUserMenus",
                                   "GetUserMenus",
                                   service =>
                                       {
                                           if (Constants.TRACING_ENABLED)
                                           {
                                               MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("Requested Get Menus for UserLogin: {0}", userLogin));
                                           }
                                           return service.GetUserMenus(userLogin);
                                       });
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
            return MakeServiceCall("GetSecurityPrincipalForFormsAuthentication",
                                   "GetSecurityPrincipalForFormsAuthentication",
                                   service =>
                                       {
                                           if (Constants.TRACING_ENABLED)
                                           {
                                               MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("SecurityServiceClient.GetSecurityPrincipalForFormsAuthentication with userName: {0}", userName));
                                           }
                                           return service.GetSecurityPrincipalForFormsAuthentication(userName, formsAuthenticationTicket, system);
                                       });
        }

        /// <summary>
        /// Create a SecurityPrincipal for a specific user
        /// </summary>
        /// <param name="userName">Field denoting the login name of the user.</param>
        /// <param name="system">Field denoting the MDMCenterSystem.</param>
        /// <returns>Returns a SecurityPrincipal for a specific user </returns>
        public SecurityPrincipal GetSecurityPrincipalForWindowsAuthentication(String userName, MDMCenterSystem system)
        {
            return MakeServiceCall("GetSecurityPrincipalForWindowsAuthentication",
                                   "GetSecurityPrincipalForWindowsAuthentication",
                                   service =>
                                       {
                                           if (Constants.TRACING_ENABLED)
                                           {
                                               MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("SecurityServiceClient.GetSecurityPrincipalForWindowsAuthentication with userName: {0}", userName));
                                           }
                                           return service.GetSecurityPrincipalForWindowsAuthentication(userName, system);
                                       });
        }

        /// <summary>
        /// Get available Roles based on the Role Name. 
        /// </summary>
        /// <param name="securityRoleShortName">Short name of role.</param>
        /// <param name="iCallerContext">>Context indicating which application is making API call</param>
        /// <returns>Returns Security role object with details.</returns>
        /// <exception cref="EndpointNotFoundException">Thrown when a remote endpoint could not be found or reached</exception>
        /// <exception cref="InvalidOperationException">Thrown when a method call is invalid</exception>
        /// <exception cref="TimeoutException">Thrown when the time allotted for an operation has expired</exception>
        /// <exception cref="MDMOperationException">Thrown when the MDMCenter operation fails</exception>
        /// <exception cref="CommunicationException">Thrown when there is a communication error in either the service or client application</exception>
        public SecurityRole GetSecurityRoleByName(String securityRoleShortName, ICallerContext iCallerContext)
        {
            return MakeServiceCall("GetSecurityRoleByName",
                                   "GetSecurityRoleByName",
                                   service =>
                                       {
                                           if (Constants.TRACING_ENABLED)
                                           {
                                               MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("Requested Get role by name for role name: {0}", securityRoleShortName));
                                           }
                                           return service.GetSecurityRoleByName(securityRoleShortName,
                                                                                FillDiagnosticTraces(iCallerContext));
                                       });
        }


        /// <summary>
        /// Gets security user details based on login name
        /// </summary>
        /// <param name="securityUserLoginName">Indicates the user login name</param>
        /// <param name="iCallerContext">Indicates caller context such as application and modlue</param>
        /// <param name="includeDisabledUser">Indicates whether disabled user should be included or not</param>
        /// <returns>Returns security user details</returns>
        /// <exception cref="EndpointNotFoundException">Thrown when a remote endpoint could not be found or reached</exception>
        /// <exception cref="InvalidOperationException">Thrown when a method call is invalid</exception>
        /// <exception cref="TimeoutException">Thrown when the time allotted for an operation has expired</exception>
        /// <exception cref="MDMOperationException">Thrown when the MDM operation fails</exception>
        /// <exception cref="CommunicationException">Thrown when there is a communication error in either the service or client application</exception>
        /// <example>
        /// <code language="c#" title="Get security user by login name" source="..\MDM.APISamples\Administration\SecurityManager\GetSecurityUser.cs" region="GetSecurityUserByLoginName" />
        /// </example>
        /// <exception cref="EndpointNotFoundException">Thrown when a remote endpoint could not be found or reached</exception>
        /// <exception cref="InvalidOperationException">Thrown when a method call is invalid</exception>
        /// <exception cref="TimeoutException">Thrown when the time allotted for an operation has expired</exception>
        /// <exception cref="MDMOperationException">Thrown when the MDMCenter operation fails</exception>
        /// <exception cref="CommunicationException">Thrown when there is a communication error in either the service or client application</exception>
        public ISecurityUser GetSecurityUserByLoginName(String securityUserLoginName, ICallerContext iCallerContext, Boolean includeDisabledUser = false)
        {
            return MakeServiceCall("GetSecurityUserByLoginName", "GetSecurityUserByLoginName", 
                client => client.GetSecurityUserByLoginName(securityUserLoginName, FillDiagnosticTraces(iCallerContext), includeDisabledUser));
        }
        
        /// <summary>
        /// Gets security user details based on login names
        /// </summary>
        /// <param name="securityUserLoginNames">Indicates collection of user login name</param>
        /// <param name="iCallerContext">Indicates caller context such as application and modlue</param>
        /// <param name="includeDisabledUsers">Indicates whether disabled users should be included or not</param>
        /// <returns>Returns security users details</returns> 
        /// <exception cref="EndpointNotFoundException">Thrown when a remote endpoint could not be found or reached</exception>
        /// <exception cref="InvalidOperationException">Thrown when a method call is invalid</exception>
        /// <exception cref="TimeoutException">Thrown when the time allotted for an operation has expired</exception>
        /// <exception cref="MDMOperationException">Thrown when the MDM operation fails</exception>
        /// <exception cref="CommunicationException">Thrown when there is a communication error in either the service or client application</exception>
        /// <example>
        /// <code language="c#" title="Get security user by login names" source="..\MDM.APISamples\Administration\SecurityManager\GetSecurityUser.cs" region="GetSecurityUserByLoginNames"/>
        /// </example>
        /// <exception cref="EndpointNotFoundException">Thrown when a remote endpoint could not be found or reached</exception>
        /// <exception cref="InvalidOperationException">Thrown when a method call is invalid</exception>
        /// <exception cref="TimeoutException">Thrown when the time allotted for an operation has expired</exception>
        /// <exception cref="MDMOperationException">Thrown when the MDMCenter operation fails</exception>
        /// <exception cref="CommunicationException">Thrown when there is a communication error in either the service or client application</exception>
        public ISecurityUserCollection GetSecurityUsersByLoginNames(Collection<String> securityUserLoginNames, ICallerContext iCallerContext, Boolean includeDisabledUsers = false)
        {
            return MakeServiceCall("GetSecurityUsersByLoginNames", "GetSecurityUsersByLoginNames",
                client => client.GetSecurityUsersByLoginNames(securityUserLoginNames, FillDiagnosticTraces(iCallerContext), includeDisabledUsers));
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="securityRoleShortName"></param>
        /// <param name="iCallerContext"></param>
        /// <returns></returns>
        public SecurityUserCollection GetSecurityUsersInSecurityRole(String securityRoleShortName, ICallerContext iCallerContext)
        {
            return MakeServiceCall("GetSecurityUsersInSecurityRole",
                                   "GetSecurityUsersInSecurityRole",
                                   service => service.GetSecurityUsersInSecurityRole(securityRoleShortName,
                                                                                     FillDiagnosticTraces(iCallerContext)));
        }

        /// <summary>
        /// Create a SecurityPrincipal for a specific user
        /// </summary>
        /// <param name="userName">Field denoting the login name of the user.</param>
        /// <param name="claimsIdentity">Field denoting identity of user, when using claims based authentication</param>
        /// <param name="system">Field denoting the MDMCenterSystem.</param>
        /// <returns>Returns a SecurityPrincipal for a specific user </returns>
        public SecurityPrincipal GetSecurityPrincipalForClaimsBasedAuthentication(String userName, ClaimsIdentity claimsIdentity, MDMCenterSystem system)
        {
            return MakeServiceCall<SecurityPrincipal>("GetSecurityPrincipalForClaimsBasedAuthentication", "GetSecurityPrincipalForClaimsBasedAuthentication",
                client => client.GetSecurityPrincipalForClaimsBasedAuthentication(userName, claimsIdentity, system));
        }

        #endregion

        #region AuditRef

        /// <summary>
        /// Set AuditInfo for given UserLogin and ProgramName
        /// </summary>
        /// <param name="auditInfo">AuditInfo having UserLogin and ProgramName</param>
        /// <returns>AuditRef Id</returns>
        /// <exception cref="EndpointNotFoundException">Thrown when a remote endpoint could not be found or reached</exception>
        /// <exception cref="InvalidOperationException">Thrown when a method call is invalid</exception>
        /// <exception cref="TimeoutException">Thrown when the time allotted for an operation has expired</exception>
        /// <exception cref="MDMOperationException">Thrown when the MDM operation fails</exception>
        /// <exception cref="CommunicationException">Thrown when there is a communication error in either the service or client application</exception>
        public Int64 SetAuditInfo(AuditInfo auditInfo)
        {
            return MakeServiceCall("SetAuditInfo", "SetAuditInfo", service => service.SetAuditInfo(auditInfo));
        }

        /// <summary>
        /// Gets Audit information by the given Audit Reference Id
        /// </summary>
        /// <example> 
        /// <code> 
        /// private static IAuditInfo GetAuditInfoById()
        /// {
        ///    // Get MDM data service
        ///    var securityService = GetSecurityService();
        /// 
        ///    // Use Primary Key of Audit Reference to fetch audit info for any table that uses the Foreign Key of Audit Reference
        ///    const Int64 auditRefId = 1;
        /// 
        ///   // Make a WCF call and get Audit information for the given Audit Reference Id
        ///   IAuditInfo auditInfo = securityService.GetAuditInfoById(auditRefId);
        /// 
        ///   return auditInfo;
        /// }
        /// </code>
        /// </example>
        /// <param name="auditRefId">AuditRefId for which all auditInfo needs to be fetched</param>
        /// <returns>return all auditInfo</returns>
        /// <exception cref="MDMOperationException">Thrown when the MDM operation fails</exception>
        public IAuditInfo GetAuditInfoById(Int64 auditRefId)
        {
            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StartTraceActivity("SecurityServiceClient.GetAuditInfoById", false);
            IAuditInfo iAuditInfo = null;
            AuditInfoCollection iAuditInfoCollection = null;
            Collection<Int64> auditRefIdCollection = null;
            ISecurityService securityServiceClient = null;

            try
            {


                auditRefIdCollection = new Collection<Int64>();
                auditRefIdCollection.Add(auditRefId);

                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "SecurityServiceClient sends 'GetAuditInfoById' request message.");

                iAuditInfoCollection = (AuditInfoCollection)this.GetAuditInfoByIds(auditRefIdCollection);

                if (iAuditInfoCollection != null && iAuditInfoCollection.Count > 0)
                {
                    iAuditInfo = (from f in iAuditInfoCollection select f).ToList<IAuditInfo>().FirstOrDefault();
                }
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "SecurityServiceClient receives 'GetAuditInfoById' response message.");

            }
            catch (Exception ex) //Here we will catch all types of exceptions and then real handling happens in the base class method 'HandleException'
            {
                base.HandleException(ex);
            }
            finally
            {
                //Close the client
                this.DisposeClient(securityServiceClient);

                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StopTraceActivity("SecurityServiceClient.GetAuditInfoById");
            }


            return iAuditInfo;
        }

        /// <summary>
        /// Get AuditInfo By given AuditRefId.
        /// </summary>
        /// <param name="auditRefIds">AuditRefId for which all auditInfo needs to be fetched</param>
        /// <returns>return all auditInfo</returns>
        /// <exception cref="EndpointNotFoundException">Thrown when a remote endpoint could not be found or reached</exception>
        /// <exception cref="InvalidOperationException">Thrown when a method call is invalid</exception>
        /// <exception cref="TimeoutException">Thrown when the time allotted for an operation has expired</exception>
        /// <exception cref="MDMOperationException">Thrown when the MDM operation fails</exception>
        /// <exception cref="CommunicationException">Thrown when there is a communication error in either the service or client application</exception>
        public IAuditInfoCollection GetAuditInfoByIds(Collection<Int64> auditRefIds)
        {
            return MakeServiceCall("GetAuditInfoByIds",
                                   "GetAuditInfoByIds",
                                   service => service.GetAuditInfoByIds(auditRefIds));
        }

        /// <summary>
        /// Get AuditInformation for Given Entities and Attributes
        /// </summary>
        /// <param name="entityId">Entity Id for which AuditInfo needs to be fetched</param>
        /// <param name="attributeIds">Attribute Ids for which AuditInfo needs to be fetched</param>
        /// <param name="locale">Current Data Locale</param>
        /// <param name="sequence">Sequence of attribute or entity</param>
        /// <param name="returnEntityAudit">whether to return onlu Entity AUdit info or not</param>
        /// <param name="returnAttirbuteAudit">whether to return onlu Attribute Audit info or not</param>
        /// <param name="returnOnlyLatestAudit">whether to return only latest audit info or not</param>
        /// <returns>Collection of AuditInfo</returns>
        /// <exception cref="EndpointNotFoundException">Thrown when a remote endpoint could not be found or reached</exception>
        /// <exception cref="InvalidOperationException">Thrown when a method call is invalid</exception>
        /// <exception cref="TimeoutException">Thrown when the time allotted for an operation has expired</exception>
        /// <exception cref="MDMOperationException">Thrown when the MDM operation fails</exception>
        /// <exception cref="CommunicationException">Thrown when there is a communication error in either the service or client application</exception>
        public IEntityAuditInfoCollection GetAuditInfo(Int64 entityId, Collection<Int32> attributeIds, LocaleEnum locale, Decimal sequence, Boolean returnEntityAudit, Boolean returnAttirbuteAudit, Boolean returnOnlyLatestAudit)
        {
            return MakeServiceCall("GetAuditInfo",
                                   "GetAuditInfo",
                                   service => service.GetAuditInfo(entityId,
                                                                   attributeIds,
                                                                   locale,
                                                                   sequence,
                                                                   returnEntityAudit,
                                                                   returnAttirbuteAudit,
                                                                   returnOnlyLatestAudit));
        }

        #endregion

        #region User Get

        /// <summary>
        /// Get All users in the system
        /// </summary>
        /// <param name="userType">Type of the user</param>
        /// <param name="countFrom">From which User list should be started.</param>
        /// <param name="countTo">To Which User list should be.</param>
        /// <param name="sortColumn">Column Name on which user should be sorted.</param>
        /// <param name="searchColumn">Column Name on which user should be searched.</param>
        /// <param name="searchParameter">Search Parameter name</param>
        /// <param name="userLogin">Current logged in User.</param>
        /// <returns>Collection of all requested User.</returns>
        public Collection<SecurityUser> GetAllUsers(int userType, int countFrom, int countTo, string sortColumn, string searchColumn, string searchParameter, string userLogin)
        {
            CallerContext callerContext = new CallerContext(MDMCenterApplication.MDMCenter, MDMCenterModules.UIProcess);
            return MakeServiceCall("GetAllUsers",
                                   "GetAllUsers",
                                   service =>
                                       {
                                           SecurityUserCollection securityUsers = service.GetAllUsers(FillDiagnosticTraces(callerContext));
                                           Collection<SecurityUser> users = new Collection<SecurityUser>();
                                           if (securityUsers != null && securityUsers.Count > 0)
                                           {
                                               foreach (SecurityUser securityUser in securityUsers)
                                               {
                                                   users.Add(securityUser);
                                               }
                                           }
                                           return users;
                                       });
        }

        /// <summary>
        /// Get all users in the system
        /// </summary>
        /// <param name="iCallerContext">Context of application making call to this API</param>
        /// <returns>Collection of users</returns>
        public ISecurityUserCollection GetAllUsers(ICallerContext iCallerContext)
        {
            return MakeServiceCall<ISecurityUserCollection>("GetAllUsers", "GetAllUsers", client => client.GetAllUsers(FillDiagnosticTraces(iCallerContext)));
        }

        /// <summary>
        /// Get user for given user id
        /// </summary>
        /// <param name="userId">Id of user to fetch data for</param>
        /// <param name="iCallerContext"> Context of application making call to this API</param>
        /// <returns>User having given UserId</returns>
        public ISecurityUser GetUserById(Int32 userId, ICallerContext iCallerContext)
        {
            return MakeServiceCall<ISecurityUser>("GetUserById", "GetUserById", client => client.GetUserById(userId, FillDiagnosticTraces(iCallerContext)));
        }

        /// <summary>
        /// Gets the permission set for the requested MDM object id and MDM object type in the provided context
        /// </summary>
        /// <param name="mdmObjectId">Id of the MDM object for which permissions are required</param>
        /// <param name="mdmObjectTypeId">Type Id of the MDM object for which permissions are required</param>
        /// <param name="mdmObjectType">Type of the MDM object for which permissions are required</param>
        /// <param name="context">Context under which permissions needs to be loaded</param>
        /// <returns>Permission set</returns>
        public IPermission GetMDMObjectPermission(Int32 mdmObjectId, Int32 mdmObjectTypeId, String mdmObjectType, IPermissionContext context)
        {
            return MakeServiceCall("", "",
                client => client.GetMDMObjectPermission(mdmObjectId, mdmObjectTypeId, mdmObjectType, context as PermissionContext));
        }

        #endregion User Get

        #region User CUD

        /// <summary>
        /// Create new SecurityUser
        /// </summary>
        /// <param name="iSecurityUser">Represent SecurityUser Object</param>
        /// <param name="iCallerContext">Context of application making call for this API</param>
        /// <returns>OperationResult of SecurityUser Creation</returns>
        /// <exception cref="ArgumentNullException">If SecurityUser Object is Null</exception>
        public IOperationResult CreateUser(ISecurityUser iSecurityUser, ICallerContext iCallerContext)
        {
            return MakeServiceCall("CreateUser", "CreateUser",
               client =>
               client.CreateUser(iSecurityUser as SecurityUser, FillDiagnosticTraces(iCallerContext)));
        }

        /// <summary>
        /// Update SecurityUser
        /// </summary>
        /// <param name="iSecurityUser">Represent SecurityUser Object</param>
        /// <param name="iCallerContext">Context of application making call for this API</param>
        /// <returns>OperationResult of SecurityUser Updating</returns>
        /// <exception cref="ArgumentNullException">If SecurityUser Object is Null</exception>
        /// <exception cref="ArgumentNullException">If SecurityUser ShortName is Null or having empty String</exception>
        /// <exception cref="ArgumentNullException">If SecurityUser LongName is Null or having empty String</exception>
        public IOperationResult UpdateUser(ISecurityUser iSecurityUser, ICallerContext iCallerContext)
        {
            return MakeServiceCall("UpdateUser", "UpdateUser",
               client =>
               client.UpdateUser(iSecurityUser as SecurityUser, FillDiagnosticTraces(iCallerContext)));
        }

        /// <summary>
        /// Delete SecurityUser
        /// </summary>
        /// <param name="iSecurityUser">Represent SecurityUser Object</param>
        /// <param name="iCallerContext">Context of application making call for this API</param>
        /// <returns>OperationResult of SecurityUser Deletion</returns>
        /// <exception cref="ArgumentNullException">If SecurityUser Object is Null</exception>
        public IOperationResult DeleteUser(ISecurityUser iSecurityUser, ICallerContext iCallerContext)
        {
            return MakeServiceCall("DeleteUser", "DeleteUser",
               client =>
               client.DeleteUser(iSecurityUser as SecurityUser, FillDiagnosticTraces(iCallerContext)));
        }

        /// <summary>
        /// Process SecurityUsers
        /// </summary>
        /// <param name="iSecurityUsers">Represent SecurityUser Object collection to process</param>
        /// <param name="iCallerContext">Context of application making call for this API</param>
        /// <returns>OperationResult of SecurityUsers process</returns>
        /// <exception cref="ArgumentNullException">If SecurityUsers Object is Null</exception>
        public IOperationResultCollection ProcessUsers(ISecurityUserCollection iSecurityUsers, ICallerContext iCallerContext)
        {

            return MakeServiceCall("ProcessUsers", "ProcessUsers",
               client =>
               client.ProcessUsers(iSecurityUsers as SecurityUserCollection, FillDiagnosticTraces(iCallerContext)));
        }

        /// <summary>
        /// Processes the user details, using the claims-based identity provided.
        /// </summary>
        /// <example>
        /// <code>
        /// // Get MDM security service instance
        /// var securityService = GetSecurityService();
        /// 
        /// // Get new instance of ICallerContext using MDMObjectFactory
        /// ICallerContext callerContext = <![CDATA[MDMObjectFactory.GetICallerContext()]]>;
        /// callerContext.Application = MDMCenterApplication.MDMCenter;
        /// callerContext.Module = MDMCenterModules.Security;
        /// 
        /// // Retrieve the claims-based identity (if configured)
        /// ClaimsIdentity claimsIdentity = (ClaimsIdentity)Thread.CurrentPrincipal.Identity;
        /// 
        /// // The following code invokes the WCF service to process the user using the available claims-based identity.
        /// IOperationResult operationResult = securityService.ProcessUserUsingClaimsIdentity(claimsIdentity, callerContext);
        /// </code>
        /// </example>
        /// <param name="claimsIdentity">Represents the claims-based identity of the user</param>
        /// <param name="callerContext">Represents the context of application invoking this API</param>
        /// <returns>Returns the <![CDATA[IOperationResult]]> of SecurityUser process operation</returns>
        public IOperationResult ProcessUserUsingClaimsIdentity(ClaimsIdentity claimsIdentity, ICallerContext callerContext)
        {
            return MakeServiceCall("ProcessUserUsingClaimsIdentity", "ProcessUserUsingClaimsIdentity",
                client => client.ProcessUserUsingClaimsIdentity(claimsIdentity, FillDiagnosticTraces(callerContext)));
        }

        #endregion User CUD

        #region Role Get

        /// <summary>
        /// Get all securityRoles
        /// </summary>
        /// <param name="iCallerContext">Context of application making call for this API</param>
        /// <returns>All Relationship types</returns>
        public ISecurityRoleCollection GetAllRoles(ICallerContext iCallerContext)
        {
            return MakeServiceCall("GetAllRoles", "GetAllRoles",
               client =>
               client.GetAllRoles(FillDiagnosticTraces(iCallerContext)));
        }

        /// <summary>
        /// SecurityRole based on given Id
        /// </summary>
        /// <param name="iCallerContext">Context of application making call for this API</param>
        /// <param name="roleId"> Role ID </param>
        /// 
        /// <returns>All Relationship types</returns>
        public ISecurityRole GetRoleById(Int32 roleId, ICallerContext iCallerContext)
        {
            return MakeServiceCall("GetRoleById", "GetRoleById",
                client =>
                client.GetRoleById(roleId, FillDiagnosticTraces(iCallerContext)));
        }

        #endregion Role Get Methods

        #region Role CUD

        /// <summary>
        /// Create new SecurityRole
        /// </summary>
        /// <param name="iSecurityRole">Represent SecurityRole Object</param>
        /// <param name="iCallerContext">Context of application making call for this API</param>
        /// <returns>OperationResult of SecurityRole Creation</returns>
        /// <exception cref="ArgumentNullException">If SecurityRole Object is Null</exception>
        public IOperationResult CreateRole(ISecurityRole iSecurityRole, ICallerContext iCallerContext)
        {
            return MakeServiceCall("CreateRole", "CreateRole",
               client =>
               client.CreateRole(iSecurityRole as SecurityRole, FillDiagnosticTraces(iCallerContext)));
        }

        /// <summary>
        /// Update SecurityRole
        /// </summary>
        /// <param name="iSecurityRole">Represent SecurityRole Object</param>
        /// <param name="iCallerContext">Context of application making call for this API</param>
        /// <returns>OperationResult of SecurityRole Updating</returns>
        /// <exception cref="ArgumentNullException">If SecurityRole Object is Null</exception>
        /// <exception cref="ArgumentNullException">If SecurityRole ShortName is Null or having empty String</exception>
        /// <exception cref="ArgumentNullException">If SecurityRole LongName is Null or having empty String</exception>
        public IOperationResult UpdateRole(ISecurityRole iSecurityRole, ICallerContext iCallerContext)
        {
            return MakeServiceCall("UpdateRole", "UpdateRole",
               client =>
               client.UpdateRole(iSecurityRole as SecurityRole, FillDiagnosticTraces(iCallerContext)));
        }

        /// <summary>
        /// Delete SecurityRole
        /// </summary>
        /// <param name="iSecurityRole">Represent SecurityRole Object</param>
        /// <param name="iCallerContext">Context of application making call for this API</param>
        /// <returns>OperationResult of SecurityRole Deletion</returns>
        /// <exception cref="ArgumentNullException">If SecurityRole Object is Null</exception>
        public IOperationResult DeleteRole(ISecurityRole iSecurityRole, ICallerContext iCallerContext)
        {
            return MakeServiceCall("DeleteRole", "DeleteRole",
               client =>
               client.DeleteRole(iSecurityRole as SecurityRole, FillDiagnosticTraces(iCallerContext)));
        }

        /// <summary>
        /// Process SecurityRoles
        /// </summary>
        /// <param name="iSecurityRoles">Represent SecurityRole Object collection to process</param>
        /// <param name="iCallerContext">Context of application making call for this API</param>
        /// <returns>OperationResult of SecurityRoles process</returns>
        /// <exception cref="ArgumentNullException">If SecurityRoles Object is Null</exception>
        public IOperationResultCollection ProcessRoles(ISecurityRoleCollection iSecurityRoles, ICallerContext iCallerContext)
        {

            return MakeServiceCall("ProcessRoles", "ProcessRoles",
               client =>
               client.ProcessRoles(iSecurityRoles as SecurityRoleCollection, FillDiagnosticTraces(iCallerContext)));
        }

        #endregion Role CUD

        #region  User Credential Get

        /// <summary>
        /// Checks whether password request id sent by the user is valid or not
        /// </summary>
        /// <example>
        /// <code>
        /// // Get MDM security service
        /// var securityService = GetSecurityService();
        /// 
        /// // Get new instance of ICallerContext using MDMObjectFactory
        /// ICallerContext callerContext = <![CDATA[MDMObjectFactory.GetICallerContext()]]>;
        /// callerContext.Application = MDMCenterApplication.MDMCenter;
        /// callerContext.Module = MDMCenterModules.Entity;
        /// 
        /// //Password to be requested 
        /// String passwordRequestId = string.Empty;
        /// 
        /// // Below will make WCF call to check whether the password request id made is valid or not
        /// IOperationResult isPasswordRequestValid = securityService.IsPasswordResetRequestValid(passwordRequestId, callerContext);
        /// </code>
        /// </example>
        /// <param name="passwordRequestId">Password request id to be validated</param>
        /// <param name="iCallerContext">Indicates name of application and module</param>
        /// <returns>Operation result containing boolean return value as indication of validity for the password request id</returns>
        public IOperationResult IsPasswordResetRequestValid(String passwordRequestId, ICallerContext iCallerContext)
        {
            return MakeServiceCall("IsPasswordResetRequestValid", "IsPasswordResetRequestValid",
                client =>
                client.IsPasswordResetRequestValid(passwordRequestId, FillDiagnosticTraces(iCallerContext)));
        }

        /// <summary>
        /// Updates the password based on password request id and hashed new password
        /// </summary>
        /// <example>
        /// <code>
        /// // Get MDM security service
        /// var securityService = GetSecurityService();
        /// 
        /// //Password to be requested 
        /// String passwordRequestId = string.Empty;
        /// 
        /// //Hashed new password
        /// String password = string.Empty;
        /// 
        /// // Get new instance of ICallerContext using MDMObjectFactory
        /// ICallerContext callerContext = <![CDATA[MDMObjectFactory.GetICallerContext()]]>;
        /// callerContext.Application = MDMCenterApplication.MDMCenter;
        /// callerContext.Module = MDMCenterModules.Entity;
        /// 
        /// // Below will make WCF call which contains the detailed information about the updated password 
        /// IOperationResult updatePassword = securityService.UpdatePassword(passwordRequestId, password, callerContext);
        /// </code>
        /// </example>
        /// <param name="passwordRequestId">Indicates the password Id to be requested</param>
        /// <param name="password">Indicates the hashed new password</param>
        /// <param name="iCallerContext">Indicates name of application and module</param>
        /// <returns>Operation result containing detail information of update password</returns>
        public IOperationResult UpdatePassword(String passwordRequestId, String password, ICallerContext iCallerContext)
        {
            return MakeServiceCall("UpdatePassword", "UpdatePassword",
                client =>
                client.UpdatePassword(passwordRequestId, password, FillDiagnosticTraces(iCallerContext)));
        }

        /// <summary>
        /// Get  the role to be used by the Installer for checking DB permission
        /// </summary>
        /// <example>
        /// <code>
        /// // Get MDM security service
        /// var securityService = GetSecurityService();
        /// 
        /// String userName = "UserName";
        /// 
        /// // Below will make WCF call to get the role assigned to the user 
        /// RoleType roleType = securityService.GetUserRoleType(userName);
        /// </code>
        /// </example>
        /// <param name="userName">Indicates the name of the user</param>
        /// <returns>Returns Role Type.</returns>
        public RoleType GetUserRoleType(String userName)
        {
            return MakeServiceCall("GetUserRoleType", "GetUserRoleType",
                client =>
                client.GetUserRoleType(userName));
        }
        #endregion

        #region Public Methods

        /// <summary>
        /// Set User Credentials
        /// </summary>
        public void SetCredentials()
        {

        }

        #endregion Methods

        #region Private Methods

        /// <summary>
        /// Get Security Service Client
        /// </summary>
        /// <returns>ServiceClient with UerName and Password</returns>
        private ISecurityService GetClient()
        {
            ISecurityService serviceClient = null;

            if (WCFServiceInstanceLoader.IsLocalInstancesEnabled())
            {
                serviceClient = WCFServiceInstanceLoader.GetServiceInstance<ISecurityService>();
            }

            if (serviceClient == null) //Means the given type is not implemented for local load..
            {
                SecurityServiceProxy securityServiceProxy = null;

                if (String.IsNullOrEmpty(this.EndPointConfigurationName) && this.EndpointAddress == null)
                    securityServiceProxy = new SecurityServiceProxy();
                else if (!String.IsNullOrEmpty(this.EndPointConfigurationName) && this.EndpointAddress == null)
                    securityServiceProxy = new SecurityServiceProxy(this.EndPointConfigurationName);
                else if (!String.IsNullOrEmpty(this.EndPointConfigurationName) && this.EndpointAddress != null)
                    securityServiceProxy = new SecurityServiceProxy(this.EndPointConfigurationName, this.EndpointAddress);

                if (this.AuthenticationType == Core.AuthenticationType.Forms)
                {
                    securityServiceProxy.ClientCredentials.UserName.UserName = this.UserName;
                    securityServiceProxy.ClientCredentials.UserName.Password = this.Password;
                    securityServiceProxy.ClientCredentials.ServiceCertificate.Authentication.CertificateValidationMode = System.ServiceModel.Security.X509CertificateValidationMode.None;
                }

                serviceClient = securityServiceProxy;
            }

            return serviceClient;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="client"></param>
        private void DisposeClient(ISecurityService client)
        {
            if (client == null)
                return;

            if (client.GetType().Equals(typeof(SecurityServiceProxy)))
            {
                SecurityServiceProxy serviceClient = (SecurityServiceProxy)client;
                if (serviceClient.State == CommunicationState.Created || serviceClient.State == CommunicationState.Opened || serviceClient.State == CommunicationState.Opening)
                {
                    serviceClient.Close();
                }
                else if (serviceClient.State == CommunicationState.Faulted)
                {
                    serviceClient.Abort();
                }
            }
            else
            {
                //Do nothing...
            }
        }

        /// <summary>
        /// Makes the SecurityServiceClient call. Creates the client instance, executes call delegate against it in
        /// impersonated context (if necessary) and then disposes the client.
        /// Also emits traces when necessary.
        /// </summary>
        /// <typeparam name="TResult">Indicates type of the result of service call.</typeparam>
        /// <param name="clientMethodName">Indicates name of the client method to include in traces.</param>
        /// <param name="serverMethodName">Indicates name of the server method to include in traces.</param>
        /// <param name="call">Indicates call delegate to be executed against a client instance.</param>
        /// <param name="traceSource">Indicates name of the trace source</param>
        /// <returns>Returns value returned by service, or default.</returns>
        private TResult MakeServiceCall<TResult>(string clientMethodName, string serverMethodName, Func<ISecurityService, TResult> call, MDMTraceSource traceSource = MDMTraceSource.General)
        {
            //Start trace activity
            if (Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.StartTraceActivity("SecurityServiceClient." + clientMethodName, traceSource, false);
            }
            TResult result = default(TResult);
            ISecurityService securityServiceClient = null;

            try
            {
                securityServiceClient = GetClient();

                ValidateContext();
                if (Constants.TRACING_ENABLED)
                {
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Verbose, "SecurityServiceClient sends '" + serverMethodName + "' request message.", traceSource);
                }
                result = Impersonate(() => call(securityServiceClient));
                if (Constants.TRACING_ENABLED)
                {
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Verbose, "SecurityServiceClient receives '" + serverMethodName + "' response message.", traceSource);
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            finally
            {
                //Close the client
                DisposeClient(securityServiceClient);
                if (Constants.TRACING_ENABLED)
                {
                    MDMTraceHelper.StopTraceActivity("SecurityServiceClient." + clientMethodName, traceSource);
                }
            }

            return result;
        }
        #endregion
    }
}