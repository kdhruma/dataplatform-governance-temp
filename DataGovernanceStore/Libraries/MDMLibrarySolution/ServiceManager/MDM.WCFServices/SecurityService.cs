using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.ServiceModel;
using System.Diagnostics;
using System.Security.Claims;

namespace MDM.WCFServices
{
    using MDM.BusinessObjects;
    using MDM.WCFServiceInterfaces;
    using MDM.AdminManager.Business;
    using MDM.Utility;
    using MDM.Core;
    using MDM.Workflow.PersistenceManager.Business;
    using MDM.SecurityManager.Business;
    using MDM.BusinessObjects.Diagnostics;
    using MDM.CacheManager.Business;

    [DiagnosticActivity]
    [ServiceBehavior(Namespace = "http://wcfservices.riversand.com", InstanceContextMode = InstanceContextMode.PerCall)]
    public class SecurityService : MDMWCFBase, ISecurityService
    {
        #region Constructors

        public SecurityService()
            : base(true)
        {

        }

        public SecurityService(Boolean loadSecurityPrincipal)
            : base(loadSecurityPrincipal)
        {

        }

        #endregion

        #region Security

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userLoginName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public Boolean AuthenticateUser(String userLoginName, String password)
        {
            bool returnValue = false;
            try
            {
                UserPrincipalBL userPrincipal = new UserPrincipalBL();
                returnValue = userPrincipal.AuthenticateUser(userLoginName, password);
            }
            catch (Exception ex)
            {
                this.LogException(ex);
                throw this.WrapException(ex);
            }
            return returnValue;
        }

        #endregion Security

        #region AuditRef

        /// <summary>
        /// 
        /// </summary>
        /// <param name="auditInfo"></param>
        public Int64 SetAuditInfo(AuditInfo auditInfo)
        {
            MDMTraceHelper.InitializeTraceSource();
            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StartTraceActivity("SecurityService.SetAuditInfo", false);

            Int64 auditRefId = -1;

            try
            {
                 if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "SecurityService receives 'SetAuditInfo' request message.");

                AuditInfoBL auditInfoBL = new AuditInfoBL();
                auditRefId = auditInfoBL.Create(auditInfo.UserLogin, auditInfo.ProgramName, true);

                 if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "SecurityService sends 'SetAuditInfo' response message.");
            }
            catch (Exception ex)
            {
                this.LogException(ex);
                throw this.WrapException(ex);
            }

            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StopTraceActivity("SecurityService.SetAuditInfo");

            return auditRefId;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="auditRefId"></param>
        /// <returns></returns>
        public AuditInfoCollection GetAuditInfoByIds(Collection<Int64> auditRefIds)
        {
            if (Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.InitializeTraceSource();
                MDMTraceHelper.StartTraceActivity("SecurityService.GetAuditInfoById", false);
            }
            AuditInfoCollection auditInfoCollection = new AuditInfoCollection();

            try
            {
                 if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "SecurityService receives 'GetAuditInfoById' request message.");

                AuditInfoBL auditInfoBL = new AuditInfoBL();
                auditInfoCollection = auditInfoBL.Get(auditRefIds);

                 if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "SecurityService sends 'GetAuditInfoById' response message.");
            }
            catch (Exception ex)
            {
                this.LogException(ex);
                throw this.WrapException(ex);
            }

            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StopTraceActivity("SecurityService.GetAuditInfoById");

            return auditInfoCollection;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityId"></param>
        /// <param name="attributeIds"></param>
        /// <param name="locale"></param>
        /// <param name="sequence"></param>
        /// <param name="returnEntityAudit"></param>
        /// <param name="returnAttirbuteAudit"></param>
        /// <param name="returnOnlyLatestAudit"></param>
        /// <returns></returns>
        public EntityAuditInfoCollection GetAuditInfo(Int64 entityId, Collection<Int32> attributeIds, LocaleEnum locale, Decimal sequence, Boolean returnEntityAudit, Boolean returnAttirbuteAudit, Boolean returnOnlyLatestAudit)
        {
            if (Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.InitializeTraceSource();
                MDMTraceHelper.StartTraceActivity("SecurityService.GetAuditInfo", false);
            }

            EntityAuditInfoCollection entityAuditInfoCollection = new EntityAuditInfoCollection();

            try
            {
                 if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "SecurityService receives 'GetAuditInfo' request message.");

                AuditInfoBL auditInfoBL = new AuditInfoBL();
                entityAuditInfoCollection = auditInfoBL.Get(entityId, attributeIds, locale, sequence, returnEntityAudit, returnAttirbuteAudit, returnOnlyLatestAudit);

                 if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "SecurityService sends 'GetAuditInfo' response message.");
            }
            catch (Exception ex)
            {
                this.LogException(ex);
                throw this.WrapException(ex);
            }

            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StopTraceActivity("SecurityService.GetAuditInfo");

            return entityAuditInfoCollection;
        }

        #endregion AuditRef

        #region User Get

        public Collection<SecurityUser> GetAllUsers(int userType, int countFrom, int countTo, string sortColumn, string searchColumn, string searchParameter, string userLogin)
        {
            MDMTraceHelper.InitializeTraceSource();
            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StartTraceActivity("SecurityService.GetAllUsers", false);

            Collection<SecurityUser> users = new Collection<SecurityUser>();

            try
            {
                SecurityUserBL securityUserBL = new SecurityUserBL();
                users = securityUserBL.GetAllUsers(userType, countFrom, countTo, sortColumn, searchColumn, searchParameter, userLogin);
            }
            catch (Exception ex)
            {
                this.LogException(ex);
                throw this.WrapException(ex);
            }

            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StopTraceActivity("SecurityService.GetAuditInfo");
            return users;
        }

        /// <summary>
        /// Get all users in the system
        /// </summary>
        /// <param name="context">Context of application making call to this API</param>
        /// <returns>Collection of users</returns>
        public SecurityUserCollection GetAllUsers(CallerContext callerContext)
        {
            return MakeBusinessLogicCall<SecurityUserBL, SecurityUserCollection>("GetAllUsers", businessLogic => businessLogic.GetAll(callerContext));
        }

        /// <summary>
        /// Get user for given user id
        /// </summary>
        /// <param name="userId">Id of user to fetch data for</param>
        /// <param name="context">Context of application making call to this API</param>
        /// <returns>User having given UserId</returns>
        public SecurityUser GetUserById(Int32 userId, CallerContext callerContext)
        {
            return MakeBusinessLogicCall<SecurityUserBL, SecurityUser>("GetUserById", businessLogic => businessLogic.GetById(userId, callerContext));
        }

        /// <summary>
        /// Get All available Menus for given User
        /// </summary>
        /// <param name="userLogin">Indicates Login User Name</param>
        /// <returns>All Menus</returns>
        public MenuCollection GetUserMenus(String userLogin)
        {
            MDMTraceHelper.InitializeTraceSource();
			if (Constants.TRACING_ENABLED)
            	MDMTraceHelper.StartTraceActivity("SecurityServiced.GetMenus", false);

            MenuCollection menus = new MenuCollection();

            try
            {
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "SecurityService receives 'GetMenus' request message.");

                MenuBL menuBL = new MenuBL();
                menus = menuBL.GetUserMenus(userLogin);

				if (Constants.TRACING_ENABLED)
                	MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "SecurityService sends 'GetMenus' response message.");
            }
            catch (Exception ex)
            {
                this.LogException(ex);
                throw this.WrapException(ex);
            }

		if (Constants.TRACING_ENABLED)
            MDMTraceHelper.StopTraceActivity("SecurityService.GetMenus");

            return menus;
        }

        /// <summary>
        /// Get list of username based on login id and username
        /// </summary>
        /// <param name="loginId">User Login ID</param>
        /// <param name="userLoginName">Login UserName</param>
        /// <returns></returns>
        public SecurityRoleCollection GetUserRoles(Int32 loginId, String userLoginName)
        {
            SecurityRoleCollection securityRoleCollection = new SecurityRoleCollection();

            try
            {
                SecurityRoleBL securityRoleManager = new SecurityRoleBL();
                securityRoleCollection = securityRoleManager.GetUserRoles(loginId, userLoginName);
            }
            catch (Exception ex)
            {
                this.LogException(ex);
                throw this.WrapException(ex);
            }

            return securityRoleCollection;
        }

        public Collection<SecurityUser> GetUsersInRole(Int32 roleId)
        {
            Collection<SecurityUser> users = new Collection<SecurityUser>();

            try
            {
                SecurityUserBL securityUserBL = new SecurityUserBL();
                users = securityUserBL.GetUsersInRole(roleId);
            }
            catch (Exception ex)
            {
                this.LogException(ex);
                throw this.WrapException(ex);
            }
            return users;
        }

        /// <summary>
        /// Create a SecurityPrincipal for a specific user
        /// </summary>
        /// <param name="userName">Field denoting the login name of the user.</param>
        /// <param name="system">Field denoting the MDMCenterSystem.</param>
        /// <returns>Returns a SecurityPrincipal for a specific user </returns>
        public SecurityPrincipal GetSecurityPrincipalForWindowsAuthentication(String userName, MDMCenterSystem system)
        {
            MDMTraceHelper.InitializeTraceSource();
            MDMTraceHelper.StartTraceActivity("SecurityService.GetSecurityPrincipalForWindowsAuthentication", false);

            SecurityPrincipal securityPrincipal = null;
            try
            {
                SecurityPrincipalBL securityPrincipalBL = new SecurityPrincipalBL();
                securityPrincipal = securityPrincipalBL.GetSecurityPrincipalForWindowsAuthentication(userName, system);
            }
            catch (Exception ex)
            {
                this.LogException(ex);
                throw this.WrapException(ex);
            }

            MDMTraceHelper.StopTraceActivity("SecurityService.GetSecurityPrincipalForWindowsAuthentication");
            return securityPrincipal;
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
            return MakeBusinessLogicCall<SecurityPrincipalBL, SecurityPrincipal>("GetSecurityPrincipalForClaimsBasedAuthentication",
                businessLogic => businessLogic.GetSecurityPrincipalForClaimsBasedAuthentication(userName, claimsIdentity, system));
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
            MDMTraceHelper.InitializeTraceSource();
            MDMTraceHelper.StartTraceActivity("SecurityService.GetSecurityPrincipalForFormsAuthentication", false);

            SecurityPrincipal securityPrincipal = null;
            try
            {
                SecurityPrincipalBL securityPrincipalBL = new SecurityPrincipalBL();
                securityPrincipal = securityPrincipalBL.GetSecurityPrincipalForFormsAuthentication(userName, formsAuthenticationTicket, system);
            }
            catch (Exception ex)
            {
                this.LogException(ex);
                throw this.WrapException(ex);
            }

            MDMTraceHelper.StopTraceActivity("SecurityService.GetSecurityPrincipalForFormsAuthentication");
            return securityPrincipal;
        }

        /// <summary>
        /// Get Security User details.
        /// </summary>
        /// <param name="securityUserLoginName">Indicates the user LoginId</param>
        /// <param name="callerContext">Caller context to indicate application and modlue which has called an API</param>
        /// <param name="includeDisabledUser">Indicates whether disabled user should be included or not</param>
        /// <returns>Security User Object</returns>
        public SecurityUser GetSecurityUserByLoginName(String securityUserLoginName, CallerContext callerContext, Boolean includeDisabledUser)
        {
            SecurityUser securityUser = new SecurityUser();

            try
            {
                SecurityUserBL securityUserManager = new SecurityUserBL();
                securityUser = securityUserManager.GetUser(securityUserLoginName, callerContext, includeDisabledUser);
            }
            catch (Exception ex)
            {
                this.LogException(ex);
                throw this.WrapException(ex);
            }

            return securityUser;
        }

        /// <summary>
        /// Get Security User details.
        /// </summary>
        /// <param name="securityUserLoginNames">Indicates collection of user LoginId</param>
        /// <param name="callerContext">Caller context to indicate application and modlue which has called an API</param>
        /// <param name="includeDisabledUsers">Indicates whether disabled users should be included or not</param>
        /// <returns>Returns security user collection</returns>
        public SecurityUserCollection GetSecurityUsersByLoginNames(Collection<String> securityUserLoginNames, CallerContext callerContext, Boolean includeDisabledUsers)
        {
            SecurityUserCollection securityUsers = new SecurityUserCollection();

            try
            {
                SecurityUserBL securityUserManager = new SecurityUserBL();
                securityUsers = securityUserManager.GetUsersByLoginNames(securityUserLoginNames, callerContext, includeDisabledUsers);
            }
            catch (Exception ex)
            {
                this.LogException(ex);
                throw this.WrapException(ex);
            }

            return securityUsers;
        }

        /// <summary>
        /// Gets collection of users under a specifies role
        /// </summary>
        /// <param name="securityRoleShortName"> Short name of the role.</param>
        /// <param name="callerContext">Caller context to denote application and module which has called an API</param>
        /// <returns>returns collection of users</returns>
        public SecurityUserCollection GetSecurityUsersInSecurityRole(String securityRoleShortName, CallerContext callerContext)
        {
            SecurityUserCollection securityUsers = new SecurityUserCollection();

            try
            {
                SecurityUserBL securityUserManager = new SecurityUserBL();
                securityUsers = securityUserManager.GetUsersInRole(securityRoleShortName, callerContext);
            }
            catch (Exception ex)
            {
                this.LogException(ex);
                throw this.WrapException(ex);
            }

            return securityUsers;
        }

       
        #endregion User Get

        #region User CUD

        /// <summary>
        /// Create new SecurityUser
        /// </summary>
        /// <param name="user">Represent SecurityUser Object</param>
        /// <param name="callerContext">Context of application making call for this API</param>
        /// <returns>OperationResult of SecurityUser Creation</returns>
        /// <exception cref="ArgumentNullException">If SecurityUser Object is Null</exception>
        public OperationResult CreateUser(SecurityUser user, CallerContext callerContext)
        {
            return MakeBusinessLogicCall<SecurityUserBL, OperationResult>("CreateUser",
                businessLogic => businessLogic.Create(user, callerContext));
        }

        /// <summary>
        /// Update SecurityUser
        /// </summary>
        /// <param name="user">Represent SecurityUser Object</param>
        /// <param name="callerContext">Context of application making call for this API</param>
        /// <returns>OperationResult of SecurityUser Updating</returns>
        /// <exception cref="ArgumentNullException">If SecurityUser Object is Null</exception>
        public OperationResult UpdateUser(SecurityUser user, CallerContext callerContext)
        {
            return MakeBusinessLogicCall<SecurityUserBL, OperationResult>("UpdateUser",
                businessLogic => businessLogic.Update(user, callerContext));

        }

        /// <summary>
        /// Delete SecurityUser
        /// </summary>
        /// <param name="user">Represent SecurityUser Object</param>
        /// <param name="callerContext">Context of application making call for this API</param>
        /// <returns>OperationResult of SecurityUser Updating</returns>
        /// <exception cref="ArgumentNullException">If SecurityUser Object is Null</exception>
        public OperationResult DeleteUser(SecurityUser user, CallerContext callerContext)
        {
            return MakeBusinessLogicCall<SecurityUserBL, OperationResult>("DeleteUser",
                businessLogic => businessLogic.Delete(user, callerContext));
        }

        /// <summary>
        /// Process SecurityUsers
        /// </summary>
        /// <param name="users">Represent SecurityUser Object collection to process</param>
        /// <param name="callerContext">Context of application making call for this API</param>
        /// <returns>OperationResult of SecurityUsers process</returns>
        /// <exception cref="ArgumentNullException">If SecurityUsers Object is Null</exception>
        public OperationResultCollection ProcessUsers(SecurityUserCollection users, CallerContext callerContext)
        {
            return MakeBusinessLogicCall<SecurityUserBL, OperationResultCollection>("ProcessUsers",
                businessLogic => businessLogic.Process(users, callerContext));

        }

        /// <summary>
        /// Processes the security user using the claims identity provided.
        /// </summary>
        /// <param name="claimsIdentity">The claims identity of the logged in user</param>
        /// <param name="callerContext">Context of application making call for this API</param>
        /// <returns>OperationResult of SecurityUser process</returns>
        public OperationResult ProcessUserUsingClaimsIdentity(ClaimsIdentity claimsIdentity, CallerContext callerContext)
        {
            return MakeBusinessLogicCall<SecurityUserBL, OperationResult>("ProcessUserUsingClaimsIdentity",
                businessLogic => businessLogic.ProcessUsingClaimsIdentity(claimsIdentity, callerContext));
        }

        public OperationResult RemoveUserFromCache(SecurityUser user, CallerContext callerContext)
        {
            var or = new OperationResult();
            var dac = new DiagnosticActivity();

            if (callerContext.TraceSettings.IsTracingEnabled)
                dac.Start();

            var key = CacheKeyGenerator.GetSecurityPrincipalCacheKey(user.UserName);

            var cache = CacheFactory.GetDistributedCache();

            if (cache.Get(key) != null)
            {
                if (!cache.Remove(key))
                {
                    or.OperationResultStatus = OperationResultStatusEnum.Failed;
                    var err = "Not able to remove SecurityPrinicipal from the cache. UserName : " + user.UserName + ". Cache Key : " + key;
                    dac.LogWarning(err);
                    or.Errors.Add(new Error("1001", err));
                }
                else
                    or.OperationResultStatus = OperationResultStatusEnum.Successful;
            }
            else
            {
                var err = "Key - Value not present in the cache for cache key : " + key;
                dac.LogWarning(err);
                or.Errors.Add(new Error("1001", err));
            }

            return or;
        }

        #endregion User CUD

        #region User Preference Get/Set

        /// <summary>
        /// Get User Preferences based on UserLoginName
        /// </summary>
        /// <param name="userLoginName">User Login Name</param>
        /// <returns></returns>
        public UserPreferences GetUserPreference(String userLoginName)
        {
            try
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "User Name on server side - " + userLoginName);
                
                UserPreferences userPreferences = new UserPreferences();
                UserPreferencesBL userPreferencesBL = new UserPreferencesBL();
                userPreferences = userPreferencesBL.GetUserPreferences(userLoginName);
                return userPreferences;
            }
            catch (Exception ex)
            {
                this.LogException(ex);
                throw this.WrapException(ex);
            }
        }

        /// <summary>
        /// Loads all object permissions for the requested user Id
        /// </summary>
        /// <param name="userId">User Id permissions needs to be loaded</param>
        public void LoadUserPermissions(Int32 userId)
        {
            try
            {
                PermissionContext context = new PermissionContext(0, 0, 0, 0, 0, 0, 0, 0, userId, 0);

                DataSecurityBL dataSecurityBL = new DataSecurityBL();
                dataSecurityBL.LoadPermissions(context);
            }
            catch (Exception ex)
            {
                this.LogException(ex);
                throw base.WrapException(ex);
            }
        }

        /// <summary>
        /// Process UserPreferences
        /// </summary>
        /// <param name="userLogin">User Login Name</param>
        /// <param name="userPreferences">userPreferences Object to Update</param>
        /// <param name="context">Context in which userPreferences needs to be processed</param>
        /// <returns>True if Process is successful</returns>
        public Boolean ProcessUserPreferences(String userLogin, UserPreferences userPreferences, CallerContext context)
        {
            try
            {
                UserPreferencesBL userPreferencesBL = new UserPreferencesBL();
                return userPreferencesBL.ProcessUserPreferences(userLogin, userPreferences, context); ;
            }
            catch (Exception ex)
            {
                this.LogException(ex);
                throw this.WrapException(ex);
            }
        }
        /// <summary>
        /// Gets the permission set for the requested MDM object id and MDM object type in the provided context
        /// </summary>
        /// <param name="mdmObjectId">Id of the MDM object for which permissions are required</param>
        /// <param name="mdmObjectTypeId">Type Id of the MDM object for which permissions are required</param>
        /// <param name="mdmObjectType">Type of the MDM object for which permissions are required</param>
        /// <param name="context">Context under which permissions needs to be loaded</param>
        /// <returns>Permission set</returns>
        /// <exception cref="ArgumentNullException">Thrown when passed permission context is null</exception>
        /// <exception cref="ArgumentException">Thrown when permission context is not having role id or user id for which permissions are required or user id is not associated with any role or MDM object id and type are not provided</exception>
        public Permission GetMDMObjectPermission(Int32 mdmObjectId, Int32 mdmObjectTypeId, String mdmObjectType,PermissionContext context)
        {
            Permission permission = null;
            try
            {
                DataSecurityBL dataSecurityBL = new DataSecurityBL();
                permission = dataSecurityBL.GetMDMObjectPermission(mdmObjectId, mdmObjectTypeId, mdmObjectType, context);
            }
            catch (Exception ex)
            {
                this.LogException(ex);
                throw this.WrapException(ex);
            }
            return permission;

        }
        #endregion User Preference Get/Set

        #region Role Get

        /// <summary>
        /// Get all the roles from system
        /// </summary>
        /// <param name="userType">User type</param>
        /// <param name="getPermissionSetOnly">Get only permission set</param>
        /// <param name="getSystemRole">Flag indicating is to show system role</param>
        /// <returns>Role collection</returns>
        public SecurityRoleCollection GetAllRoles(CallerContext callerContext)
        {
            return MakeBusinessLogicCall<SecurityRoleBL, SecurityRoleCollection>("GetAllRoles", businessLogic => businessLogic.GetAll(callerContext));
        }

        /// <summary>
        /// Get from system for given role Id
        /// </summary>
        /// <param name="callerContext">Context indicating which application is making API call</param>
        /// <param name="roleId">Id of role to fetch</param>
        /// <returns>Role collection</returns>
        public SecurityRole GetRoleById(Int32 roleId, CallerContext callerContext)
        {
            return MakeBusinessLogicCall<SecurityRoleBL, SecurityRole>("GetRoleById", businessLogic => businessLogic.GetById(roleId, callerContext));
        }
		
		/// <summary>
        /// Get available Roles based on the Role Name. 
        /// </summary>
        /// <param name="securityRoleShortName">Short name of role.</param>
        /// <param name="callerContext">>Context indicating which application is making API call</param>
        /// <returns>Returns Security role object with details.</returns>
        public SecurityRole GetSecurityRoleByName(String securityRoleShortName, CallerContext callerContext)
        {
            return MakeBusinessLogicCall<SecurityRoleBL, SecurityRole>("GetSecurityRoleByName", businessLogic => businessLogic.GetByName(securityRoleShortName, callerContext));
        }

        /// <summary>
        /// Get  the role to be used by the Installer for checking DB permission
        /// </summary>
        /// <param name="userName">Indicates the name of the user</param>
        /// <returns>Returns Role Type.</returns>
        public RoleType GetUserRoleType(string userName)
        {
            return MakeBusinessLogicCall<BuildInfoBL, RoleType>("GetUserRoleType", businessLogic => businessLogic.GetUserRoleType(userName));
        }
        #endregion Role Get

        #region Role CUD

        /// <summary>
        /// Create new SecurityRole
        /// </summary>
        /// <param name="role">Represent SecurityRole Object</param>
        /// <param name="callerContext">Context of application making call for this API</param>
        /// <returns>OperationResult of SecurityRole Creation</returns>
        /// <exception cref="ArgumentNullException">If SecurityRole Object is Null</exception>
        public OperationResult CreateRole(SecurityRole role, CallerContext callerContext)
        {
            return MakeBusinessLogicCall<SecurityRoleBL, OperationResult>("CreateRole",
                businessLogic => businessLogic.Create(role, callerContext));
        }

        /// <summary>
        /// Update SecurityRole
        /// </summary>
        /// <param name="role">Represent SecurityRole Object</param>
        /// <param name="callerContext">Context of application making call for this API</param>
        /// <returns>OperationResult of SecurityRole Updating</returns>
        /// <exception cref="ArgumentNullException">If SecurityRole Object is Null</exception>
        public OperationResult UpdateRole(SecurityRole role, CallerContext callerContext)
        {
            return MakeBusinessLogicCall<SecurityRoleBL, OperationResult>("UpdateRole",
                businessLogic => businessLogic.Update(role, callerContext));

        }

        /// <summary>
        /// Delete SecurityRole
        /// </summary>
        /// <param name="role">Represent SecurityRole Object</param>
        /// <param name="callerContext">Context of application making call for this API</param>
        /// <returns>OperationResult of SecurityRole Updating</returns>
        /// <exception cref="ArgumentNullException">If SecurityRole Object is Null</exception>
        public OperationResult DeleteRole(SecurityRole role, CallerContext callerContext)
        {
            return MakeBusinessLogicCall<SecurityRoleBL, OperationResult>("DeleteRole",
                businessLogic => businessLogic.Delete(role, callerContext));
        }

        /// <summary>
        /// Process SecurityRoles
        /// </summary>
        /// <param name="roles">Represent SecurityRole Object collection to process</param>
        /// <param name="callerContext">Context of application making call for this API</param>
        /// <returns>OperationResult of SecurityRoles process</returns>
        /// <exception cref="ArgumentNullException">If SecurityRoles Object is Null</exception>
        public OperationResultCollection ProcessRoles(SecurityRoleCollection roles, CallerContext callerContext)
        {
            return MakeBusinessLogicCall<SecurityRoleBL, OperationResultCollection>("ProcessRoles",
                businessLogic => businessLogic.Process(roles, callerContext));

        }

        #endregion Role CUD

        #region  User Credential Get

        /// <summary>
        /// Specifies whether the password request is valid or not.
        /// </summary>
        /// <param name="passwordRequestId">Password request id to be validated</param>
        /// <param name="callerContext">Indicates name of application and module</param>
        /// <returns>Operation result containing boolean return value as indication of validity for the password request id</returns>
        public OperationResult IsPasswordResetRequestValid(String passwordRequestId, CallerContext callerContext)
        {
            return MakeBusinessLogicCall<PasswordResetRequestBL, OperationResult>("IsPasswordResetRequestValid",
                businessLogic => businessLogic.IsPasswordResetRequestValid(passwordRequestId, callerContext));
        }

        /// <summary>
        /// Update/Reset password for a password reset id
        /// </summary>
        /// <param name="passwordRequestId">Password request id</param>
        /// <param name="password">Hashed new password</param>
        /// <param name="callerContext">Indicates name of application and module</param>
        /// <returns>Operation result containing detail information of update password</returns>
        public OperationResult UpdatePassword(String passwordRequestId, String password, CallerContext callerContext)
        {
            return MakeBusinessLogicCall<PasswordResetRequestBL, OperationResult>("UpdatePassword",
                businessLogic => businessLogic.UpdatePassword(passwordRequestId, password, callerContext));
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Makes calls of Security Business logic.
        /// Also emits traces when necessary
        /// </summary>
        /// <typeparam name="TResult">The type of the result of method of business logic</typeparam>
        /// <typeparam name="TBusinessLogic">Type of business logic</typeparam>
        /// <param name="methodName">Name of the method for tracing</param>
        /// <param name="call">The call delegate to be executed against business logic instance</param>
        /// <returns>The value returned by business logic or default</returns>
        private TResult MakeBusinessLogicCall<TBusinessLogic, TResult>(string methodName, Func<TBusinessLogic, TResult> call) where TBusinessLogic : BusinessLogicBase, new()
        {
            if (Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.InitializeTraceSource();
                MDMTraceHelper.StartTraceActivity("SecurityService." + methodName, false);
            }

            TResult operationResult;

            try
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "SecurityService receives" + methodName + " request message.");

                operationResult = call(new TBusinessLogic());
                
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "SecurityService receives" + methodName + " response message.");
            }
            catch (Exception ex)
            {
                this.LogException(ex);
                throw base.WrapException(ex);
            }

            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StopTraceActivity("SecurityService.GetSecurityPrincipalForFormsAuthentication");

            return operationResult;
        }

        #endregion #region Private Methods

       
    }
}
