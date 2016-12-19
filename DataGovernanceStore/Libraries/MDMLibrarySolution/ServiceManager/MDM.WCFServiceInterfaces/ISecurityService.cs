using System;
using System.ServiceModel;
using System.Collections;
using System.Collections.ObjectModel;
using System.Security.Claims;

namespace MDM.WCFServiceInterfaces
{
    using MDM.BusinessObjects;
    using MDM.Core;

    /// <summary>
    /// Defines operation contracts for MDM Data operations
    /// </summary>
    [ServiceContract(Namespace = "http://wcfservices.riversand.com")]
    public interface ISecurityService
    {
        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        Boolean AuthenticateUser(String userLoginName, String password);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        UserPreferences GetUserPreference(String userLoginName);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        SecurityRoleCollection GetUserRoles(int LoginId, String userLoginName);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        void LoadUserPermissions(Int32 userId);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        Boolean ProcessUserPreferences(String userLogin, UserPreferences userPreferences, CallerContext context);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        MenuCollection GetUserMenus(String userLogin);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        Int64 SetAuditInfo(AuditInfo auditInfo);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        AuditInfoCollection GetAuditInfoByIds(Collection<Int64> auditRefIds);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        EntityAuditInfoCollection GetAuditInfo(Int64 entityId, Collection<Int32> attributeIds, LocaleEnum locale, Decimal sequence, Boolean returnEntityAudit, Boolean returnAttirbuteAudit, Boolean returnOnlyLatestAudit);
        
        #region User Get

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        Collection<SecurityUser> GetUsersInRole(int roleId);

        [OperationContract(Name = "GetAllUsersOld")]
        [Obsolete("This method has been obsoleted. Please use GetAllUsers method instead of this")]
        [FaultContract(typeof(MDMExceptionDetails))]
        Collection<SecurityUser> GetAllUsers(Int32 userType, Int32 countFrom, Int32 countTo, String sortColumn, String searchColumn, String searchParameter, String userLogin);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        SecurityUserCollection GetAllUsers(CallerContext callerContext);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        SecurityUser GetUserById(Int32 userId, CallerContext callerContext);


        #endregion User Get

        #region User CUD

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        OperationResult CreateUser(SecurityUser securityUser, CallerContext callerContext);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        OperationResult UpdateUser(SecurityUser securityUser, CallerContext callerContext);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        OperationResult DeleteUser(SecurityUser securityUser, CallerContext callerContext);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        OperationResultCollection ProcessUsers(SecurityUserCollection securityUsers, CallerContext callerContext);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        OperationResult ProcessUserUsingClaimsIdentity(ClaimsIdentity claimsIdentity, CallerContext callerContext);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        OperationResult RemoveUserFromCache(SecurityUser securityUser, CallerContext callerContext);

        #endregion User CUD

        #region Role Get

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        SecurityRoleCollection GetAllRoles(CallerContext callerContext);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        SecurityRole GetRoleById(Int32 roleId, CallerContext callerContext);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        RoleType GetUserRoleType(String userName);

        #endregion Role Get

        #region Role CUD

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        OperationResult CreateRole(SecurityRole securityRole, CallerContext callerContext);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        OperationResult UpdateRole(SecurityRole securityRole, CallerContext callerContext);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        OperationResult DeleteRole(SecurityRole securityRole, CallerContext callerContext);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        OperationResultCollection ProcessRoles(SecurityRoleCollection securityRoles, CallerContext callerContext);

        #endregion Role CUD

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        SecurityPrincipal GetSecurityPrincipalForWindowsAuthentication(String userName, MDMCenterSystem system);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        SecurityPrincipal GetSecurityPrincipalForFormsAuthentication(String userName, String formsAuthenticationTicket, MDMCenterSystem system);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        SecurityPrincipal GetSecurityPrincipalForClaimsBasedAuthentication(String userName, ClaimsIdentity claimsIdentity, MDMCenterSystem system);
    
        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        SecurityRole GetSecurityRoleByName(String securityRoleShortName, CallerContext callerContext);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        SecurityUser GetSecurityUserByLoginName(String securityUserLogingName, CallerContext callerContext, Boolean includeDisabledUser);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        SecurityUserCollection GetSecurityUsersByLoginNames(Collection<String> userLogins, CallerContext callerContext, Boolean includeDisabledUsers);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        SecurityUserCollection GetSecurityUsersInSecurityRole(String securityRoleShortName, CallerContext callerContext);
        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        Permission GetMDMObjectPermission(Int32 mdmObjectId, Int32 mdmObjectTypeId, String mdmObjectType,
            PermissionContext context);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        OperationResult IsPasswordResetRequestValid(String passwordRequestId, CallerContext callerContext);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        OperationResult UpdatePassword(String passwordRequestId, String password, CallerContext callerContext);

    }
}
